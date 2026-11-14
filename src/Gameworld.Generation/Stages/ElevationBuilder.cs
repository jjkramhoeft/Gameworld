using Gameworld.Domain;
using Gameworld.Generation.Biomes;
using Gameworld.Generation.Heights;

namespace Gameworld.Generation.Stages;

/// <summary>The final form of a land patch: its biome and resources with the height from the height map.</summary>
public sealed class PatchFinal
{
    public required PatchDetail Patch { get; init; }
    /// <summary>The mean height of the patch's land; null when no raster cell centre lies in it.</summary>
    public required double? MeanHeightM { get; init; }
    public required BiomeRecord Biome { get; init; }
    public required PatchResources Resources { get; init; }
}

/// <summary>The working data of an Elevated region.</summary>
public sealed class ElevationWork
{
    private readonly Dictionary<long, PatchFinal> _byRoot;

    public ElevationWork(GridCoord region, HeightMapResult heights, PatchFinal[] patches)
    {
        Region = region;
        Heights = heights;
        Patches = patches;
        _byRoot = patches.ToDictionary(p => p.Patch.Root.Key);
    }

    public GridCoord Region { get; }
    public HeightMapResult Heights { get; }
    /// <summary>The land patches of the region, sorted by key.</summary>
    public PatchFinal[] Patches { get; }

    public PatchFinal? Find(GridCoord root) => _byRoot.GetValueOrDefault(root.Key);

    public RegionElevation ToDomain() => new(
        Heights.Map,
        Patches.Select(p => new PatchElevation(
            new PatchId(new PatchCellId(p.Patch.Root)), p.MeanHeightM, p.Biome, p.Resources)).ToArray());
}

/// <summary>
/// The Elevated stage: the height map (map-generation.md section 12) and the final biomes and
/// resources, calculated with the mean height of each patch instead of the typical height of
/// its elevation type (sections 13.1 and 14).
/// </summary>
internal static class ElevationBuilder
{
    public static ElevationWork Build(World world, DetailWork detail)
    {
        var heights = HeightMapBuilder.Build(world, detail.Region);
        var patches = new List<PatchFinal>();
        foreach (var patch in detail.Patches)
        {
            if (patch.Shape.Ocean) continue;
            double? mean = heights.MeanHeightByPatch.TryGetValue(patch.Root.Key, out var m) ? m : null;
            var h = mean is { } v ? Math.Max(0, v) : BiomeRules.TypicalHeightM(patch.ZoneType);
            var biome = world.BiomeRules.Classify(
                patch.SeaLevelTemperatureC, patch.FieldPrecipitationMm, h, patch.ZoneType, patch.Shape.Coastal, patch.RiverPoor);
            patches.Add(new PatchFinal
            {
                Patch = patch,
                MeanHeightM = mean,
                Biome = biome,
                Resources = DetailBuilder.ResourcesOf(world, patch, biome),
            });
        }
        return new ElevationWork(detail.Region, heights, patches.ToArray());
    }
}
