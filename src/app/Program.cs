using System.Diagnostics;
using System.Text.Json;
using Gameworld.App;
using Gameworld.Domain.Generation;
using Gameworld.Domain.Model;

try
{
    var options = Options.Parse(args);
    if (options.Help)
    {
        Console.WriteLine(Options.Usage);
        return 0;
    }
    Run(options);
    return 0;
}
catch (Exception error) when (error is ArgumentException or JsonException or IOException or UnauthorizedAccessException)
{
    Console.Error.WriteLine(error.Message);
    return 1;
}

static void Run(Options options)
{
    var parameters = options.ParametersFile is null
        ? DefaultParameters.Create()
        : ParameterFile.Load(options.ParametersFile);
    if (options.SaveParametersFile is not null)
    {
        ParameterFile.Save(parameters, options.SaveParametersFile);
        Console.WriteLine($"Parameters  {Path.GetFullPath(options.SaveParametersFile)}");
    }

    var foundation = new Foundation(new WorldSeed(options.Seed), GeneratorVersion.Current, parameters);
    Console.WriteLine($"Foundation  {foundation.Id} (seed {foundation.Seed}, parameter set {foundation.ParameterSetId})");

    // One ring more than is shown, so the cells at the edge of the image are right.
    var generator = new RegionGenerator();
    var watch = Stopwatch.StartNew();
    var regions = options.Centre.Within(options.Rings + 1)
        .Select(region => generator.Site(foundation, region))
        .ToList();
    watch.Stop();

    var zones = regions.SelectMany(region => region.ZoneCells).ToList();
    Console.WriteLine($"Sited       {regions.Count} regions in {watch.ElapsedMilliseconds} ms");
    Console.WriteLine($"Zone cells  {zones.Count}, of which {zones.Count(zone => zone.IsOceanSeed)} ocean seeds");
    Console.WriteLine($"Patch cells {zones.Sum(zone => zone.PatchCells.Length)}");

    var image = SitedMapRenderer.Render(regions, options.Centre, options.Rings, parameters.Siting.RegionGridSizeKm, options.Pixels);
    ParameterFile.CreateFolderOf(options.Out);
    PngWriter.Write(options.Out, options.Pixels, options.Pixels, image);
    Console.WriteLine($"Image       {Path.GetFullPath(options.Out)}");
}
