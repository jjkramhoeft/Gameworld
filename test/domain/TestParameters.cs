using Gameworld.Domain.Model;

namespace Gameworld.Domain.Tests;

/// <summary>A fixed parameter set for tests. The pinned ids depend on every value here.</summary>
internal static class TestParameters
{
    public static ParameterSet Create(double oceanLinkChance = 0.5) => new()
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
            ElevationWeights = new(3, 3, 2, 1),
        },
        Outline = new()
        {
            OceanLinkChance = oceanLinkChance,
            MaxOceanLinkDistanceKm = 200,
            ExpansionsPerOceanSeed = new(0, 2),
            ZoneMergersPerRegion = new(1, 3),
        },
        Detail = new() { PatchMergersPerZone = new(1, 4) },
        Elevation = new()
        {
            HeightBands = new(new(0, 50), new(20, 300), new(200, 1000), new(800, 4000)),
        },
        Fields = new()
        {
            Precipitation = Noise(),
            InitialTemperature = Noise(),
            Prosperity = Noise(),
            Supernatural = Noise(),
        },
        Bedrocks =
        [
            new() { Id = new("shield"), Name = "Crystalline shield", Weights = new(1, 1, 1, 0) },
            new() { Id = new("volcanic"), Name = "Volcanic arc", Weights = new(0, 0, 0, 1) },
        ],
        Biomes = [new() { Id = new("tundra"), Name = "Tundra", Color = new(200, 210, 190) }],
    };

    private static NoiseParameters Noise() => new() { ScaleKm = 500, Octaves = 4, Persistence = 0.5, Lacunarity = 2 };
}
