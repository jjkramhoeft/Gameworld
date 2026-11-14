using Gameworld.Domain.Model;

namespace Gameworld.App;

/// <summary>
/// The parameter set the app uses when no parameter file is given. A starting point for
/// phase 1, not a decision: only the Siting values are used by the generator so far, and the
/// rest are placeholders until their steps are implemented.
/// </summary>
internal static class DefaultParameters
{
    public static ParameterSet Create() => new()
    {
        Siting = new()
        {
            RegionGridSizeKm = 150,
            RegionJitter = 0.3,
            ZoneCellsPerRegion = new(8, 12),
            PatchCellsPerZoneCell = new(8, 18),
            ZoneCellCandidates = 16,
            PatchCellCandidates = 12,
            OceanSeedsPerRegion = new(0, 3),
            ElevationWeights = new(Flat: 3, Lowland: 3, Hills: 2, Mountains: 1),
        },
        Outline = new()
        {
            OceanLinkChance = 0.5,
            MaxOceanLinkDistanceKm = 200,
            ExpansionsPerOceanSeed = new(0, 2),
            ZoneMergersPerRegion = new(1, 3),
        },
        Detail = new() { PatchMergersPerZone = new(1, 4) },
        Elevation = new()
        {
            HeightBands = new(
                Flat: new(0, 50),
                Lowland: new(20, 300),
                Hills: new(200, 1000),
                Mountains: new(800, 4000)),
        },
        Fields = new()
        {
            Precipitation = Noise(),
            InitialTemperature = Noise(),
            Prosperity = Noise(),
            Supernatural = Noise(),
        },
        // The example table of the readme: weight 1 under the elevation types a bedrock is mostly found under.
        Bedrocks =
        [
            Bedrock("shield", "Crystalline shield", flat: 1, lowland: 1, hills: 1),
            Bedrock("foldBelt", "Fold belt", hills: 1, mountains: 1),
            Bedrock("granite", "Granite intrusion", hills: 1, mountains: 1),
            Bedrock("volcanic", "Volcanic arc", mountains: 1),
            Bedrock("basalt", "Flood basalt", flat: 1, lowland: 1, hills: 1),
            Bedrock("ophiolite", "Ophiolite", hills: 1, mountains: 1),
            Bedrock("carbonate", "Carbonate platform", lowland: 1, hills: 1),
            Bedrock("clastic", "Clastic basin", flat: 1, lowland: 1),
            Bedrock("alluvium", "Unconsolidated cover", flat: 1, lowland: 1),
        ],
        // Biomes are added together with the Elevate step.
        Biomes = [],
    };

    private static NoiseParameters Noise() => new() { ScaleKm = 500, Octaves = 4, Persistence = 0.5, Lacunarity = 2 };

    private static Bedrock Bedrock(string id, string name, double flat = 0, double lowland = 0, double hills = 0, double mountains = 0) =>
        new() { Id = new(id), Name = name, Weights = new(flat, lowland, hills, mountains) };
}
