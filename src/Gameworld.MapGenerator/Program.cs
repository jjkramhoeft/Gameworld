using System.Diagnostics;
using System.Globalization;
using Gameworld.Domain;
using Gameworld.Generation;
using Gameworld.Generation.Rendering;

// The map generation app: generates a block of regions of one foundation up to a stage and
// writes maps (png), the foundation and the stored regions (json) to a folder.

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

var options = Options.Parse(args);
if (options is null) return 1;

if (options.ExportParameters is { } exportPath)
{
    ParameterSet.Default.Save(exportPath);
    Console.WriteLine($"Wrote the default Parameter Set to {exportPath}");
    return 0;
}

var parameters = options.ParametersPath is { } path ? ParameterSet.Load(path) : ParameterSet.Default;
var foundation = new Foundation(Seed.FromText(options.Seed), World.GeneratorVersion, parameters);
var world = new World(foundation);

Directory.CreateDirectory(options.OutDir);
File.WriteAllText(Path.Combine(options.OutDir, "foundation.json"), DomainJson.Serialize(foundation, indented: true) + "\n");

Console.WriteLine($"Foundation: seed \"{foundation.Seed.Text}\" ({foundation.Seed.Value}), generator version {foundation.GeneratorVersion}");
Console.WriteLine($"Regions: {2 * options.Rings + 1} × {2 * options.Rings + 1} around {options.Centre}, to stage {options.Stage}");
Console.WriteLine();

// Generate.
var total = Stopwatch.StartNew();
var regions = new List<Region>();
Console.WriteLine($"{"region",-10} {"stage",-9} {"zones",5} {"sea",5} {"patches",7} {"rivers",6} {"lakes",5} {"prospects",9} {"links",5} {"ms",6}");
foreach (var id in RegionStageRules.RegionsWithin(options.Centre, options.Rings))
{
    var sw = Stopwatch.StartNew();
    var region = world.Advance(id, options.Stage);
    regions.Add(region);
    Console.WriteLine(
        $"{id.Coord,-10} {region.Stage,-9} {region.Outline.Zones.Count,5} {(region.Outline.HasSeaAccess ? "yes" : "no"),5} "
        + $"{region.Detail?.Patches.Count.ToString() ?? "-",7} {region.Detail?.Rivers.Nodes.Count.ToString() ?? "-",6} "
        + $"{region.Detail?.Lakes.Count.ToString() ?? "-",5} {region.Plan?.Prospects.Count.ToString() ?? "-",9} "
        + $"{region.Settlement?.Travel.Links.Count.ToString() ?? "-",5} {sw.ElapsedMilliseconds,6}");
}
Console.WriteLine($"Generated {regions.Count} regions in {total.Elapsed.TotalSeconds:F1} s ({world.Regions.Count} stored, the rest as far as the stages around them need)");

// Store the regions.
if (options.WriteRegions)
{
    var dir = Path.Combine(options.OutDir, "regions");
    Directory.CreateDirectory(dir);
    foreach (var region in regions)
        File.WriteAllText(
            Path.Combine(dir, $"{region.Id.Coord.I}_{region.Id.Coord.J}.json"),
            DomainJson.Serialize(region.ToSnapshot()));
    Console.WriteLine($"Wrote {regions.Count} regions to {dir}");
}

// Draw the maps: the bounding box of the block of regions.
var size = world.Cells.RegionSize;
var left = (options.Centre.Coord.I - options.Rings) * size;
var top = (options.Centre.Coord.J - options.Rings) * size;
var span = (2 * options.Rings + 1) * size;
var pixels = (int)Math.Ceiling(span / options.MetresPerPixel);
if (pixels > 8192)
{
    Console.Error.WriteLine($"The map would be {pixels} px wide; raise --scale (now {options.MetresPerPixel} m/px).");
    return 1;
}
var view = new MapView(left, top, options.MetresPerPixel, pixels, pixels);
var layers = new MapLayers
{
    Rivers = options.Stage >= RegionStage.Detailed,
    Lakes = options.Stage >= RegionStage.Detailed,
    Prospects = options.Stage >= RegionStage.Planned,
    Travel = options.Stage >= RegionStage.Settled,
    FinalBiomes = options.Stage >= RegionStage.Elevated,
};
foreach (var mode in options.Modes)
{
    if (mode == MapMode.Biomes && options.Stage < RegionStage.Detailed) continue;
    if (mode == MapMode.Height && options.Stage < RegionStage.Elevated) continue;
    var sw = Stopwatch.StartNew();
    var modeLayers = mode == MapMode.Height ? layers with { Hillshade = true, Borders = false } : layers;
    var file = Path.Combine(options.OutDir, $"{mode.ToString().ToLowerInvariant()}.png");
    File.WriteAllBytes(file, MapRenderer.Render(world, view, mode, modeLayers).ToPng());
    Console.WriteLine($"Wrote {file} ({pixels} × {pixels} px, {sw.ElapsedMilliseconds} ms)");
}
return 0;

internal sealed record Options
{
    public string Seed { get; init; } = Gameworld.Domain.Seed.DefaultText;
    public string? ParametersPath { get; init; }
    public RegionId Centre { get; init; } = new(0, 0);
    public int Rings { get; init; } = 1;
    public RegionStage Stage { get; init; } = RegionStage.Settled;
    public string OutDir { get; init; } = "out";
    public double MetresPerPixel { get; init; } = 250;
    public MapMode[] Modes { get; init; } = [MapMode.Map, MapMode.Biomes, MapMode.Height];
    public bool WriteRegions { get; init; } = true;
    public string? ExportParameters { get; init; }

    public static Options? Parse(string[] args)
    {
        var o = new Options();
        try
        {
            for (var i = 0; i < args.Length; i++)
            {
                string Value() => i + 1 < args.Length ? args[++i] : throw new FormatException($"{args[i]} needs a value.");
                switch (args[i])
                {
                    case "--seed": o = o with { Seed = Value() }; break;
                    case "--parameters": o = o with { ParametersPath = Value() }; break;
                    case "--region":
                        var parts = Value().Split(',');
                        o = o with { Centre = new RegionId(int.Parse(parts[0]), int.Parse(parts[1])) };
                        break;
                    case "--rings": o = o with { Rings = Math.Max(0, int.Parse(Value())) }; break;
                    case "--stage":
                        var stage = Enum.Parse<RegionStage>(Value(), ignoreCase: true);
                        o = o with { Stage = stage == RegionStage.Sited ? RegionStage.Outlined : stage };
                        break;
                    case "--out": o = o with { OutDir = Value() }; break;
                    case "--scale": o = o with { MetresPerPixel = double.Parse(Value(), CultureInfo.InvariantCulture) }; break;
                    case "--modes":
                        o = o with { Modes = Value().Split(',').Select(m => Enum.Parse<MapMode>(m, ignoreCase: true)).ToArray() };
                        break;
                    case "--no-regions": o = o with { WriteRegions = false }; break;
                    case "--export-parameters": o = o with { ExportParameters = Value() }; break;
                    case "-h" or "--help":
                        Help();
                        return null;
                    default:
                        throw new FormatException($"Unknown option {args[i]}.");
                }
            }
        }
        catch (Exception e) when (e is FormatException or ArgumentException or IndexOutOfRangeException or OverflowException)
        {
            Console.Error.WriteLine(e.Message);
            Help();
            return null;
        }
        return o;
    }

    private static void Help() => Console.WriteLine(
        """
        Gameworld map generator

          --seed <text>               the seed text of the foundation (default "cellgen")
          --parameters <file.json>    the Parameter Set (default: the built-in defaults)
          --region <i,j>              the centre region (default 0,0)
          --rings <n>                 rings of regions around the centre (default 1: 3 × 3 regions)
          --stage <stage>             Outlined, Detailed, Elevated, Planned or Settled (default Settled)
          --out <folder>              where to write (default "out")
          --scale <m/px>              metres per pixel of the maps (default 250)
          --modes <list>              map,biomes,height (default all three)
          --no-regions                do not write the regions as json
          --export-parameters <file>  write the default Parameter Set to a json file and stop
        """);
}
