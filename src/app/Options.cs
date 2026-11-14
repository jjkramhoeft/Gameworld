using System.Globalization;
using Gameworld.Domain.Model;

namespace Gameworld.App;

/// <summary>The command line of the app.</summary>
internal sealed record Options
{
    public ulong Seed { get; init; } = 42;
    public RegionId Centre { get; init; } = new(0, 0);
    public int Rings { get; init; } = 2;
    public int Pixels { get; init; } = 1000;
    public string Out { get; init; } = Path.Combine("artifacts", "sited.png");
    public string? ParametersFile { get; init; }
    public string? SaveParametersFile { get; init; }
    public bool Help { get; init; }

    public const string Usage = """
        Sites a block of regions and draws them to a PNG file.

        Usage: dotnet run --project src/app -- [options]

          --seed <number>            The seed of the foundation. Default 42.
          --centre <x,y>             The region in the middle of the image. Default 0,0.
          --rings <number>           Rings of regions around the centre. Default 2, a block of 5 x 5.
          --pixels <number>          Width and height of the image. Default 1000.
          --out <file>               The image file. Default artifacts/sited.png.
          --parameters <file>        A parameter set as JSON. Default: the built-in set.
          --save-parameters <file>   Writes the parameter set in use as JSON, to edit and pass to --parameters.
          --help                     Shows this text.

        Black lines are region borders, dark lines zone cells and faint lines patch cells.
        Blue is an ocean seed; light green, green, tan and grey are flat, lowland, hills and mountains.
        Red dots are region sites and black dots zone cell sites.
        """;

    public static Options Parse(string[] args)
    {
        var options = new Options();
        for (var index = 0; index < args.Length; index++)
        {
            var name = args[index];
            if (name is "--help" or "-h" or "-?")
                return options with { Help = true };

            if (index + 1 >= args.Length)
                throw new ArgumentException($"Option '{name}' needs a value. Use --help to see the options.");
            var value = args[++index];

            options = name switch
            {
                "--seed" => options with { Seed = Number<ulong>(name, value) },
                "--centre" or "--center" => options with { Centre = Region(name, value) },
                "--rings" => options with { Rings = AtLeast(0, name, Number<int>(name, value)) },
                "--pixels" => options with { Pixels = AtLeast(1, name, Number<int>(name, value)) },
                "--out" => options with { Out = value },
                "--parameters" => options with { ParametersFile = value },
                "--save-parameters" => options with { SaveParametersFile = value },
                _ => throw new ArgumentException($"Unknown option '{name}'. Use --help to see the options."),
            };
        }
        return options;
    }

    private static T Number<T>(string name, string value) where T : IParsable<T> =>
        T.TryParse(value, CultureInfo.InvariantCulture, out var number)
            ? number
            : throw new ArgumentException($"Option '{name}' needs a whole number, but got '{value}'.");

    private static int AtLeast(int lowest, string name, int value) =>
        value >= lowest ? value : throw new ArgumentException($"Option '{name}' must be at least {lowest}.");

    private static RegionId Region(string name, string value)
    {
        var parts = value.Split(',');
        if (parts.Length != 2)
            throw new ArgumentException($"Option '{name}' needs a region as x,y, but got '{value}'.");
        return new(Number<int>(name, parts[0]), Number<int>(name, parts[1]));
    }
}
