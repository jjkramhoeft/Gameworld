using Gameworld.Domain;

namespace Gameworld.Generation.Resources;

/// <summary>
/// The water site of a patch, from the region's rivers and lakes (map-generation.md section 14.1).
/// River sizes are flows divided by the attractors per patch, so they read as "patches drained".
/// </summary>
/// <param name="River">The largest flow of a river node in the patch; 8 and up is navigable, 80 and up a large river.</param>
/// <param name="Mouth">The same for nodes whose parent is a root or lies over the sea.</param>
/// <param name="LakeAreaM2">The area of the largest lake placed in the patch; 0 without a lake.</param>
/// <param name="Harbour">The patch borders a bay.</param>
/// <param name="GoldLoad">The share of the land river nodes at and upstream of the patch's largest river node that lie in hills or mountains zones on gold-bearing bedrock.</param>
/// <param name="TinLoad">The same share for granite.</param>
public readonly record struct WaterSite(
    double River,
    double Mouth,
    double LakeAreaM2,
    bool Harbour,
    double GoldLoad,
    double TinLoad)
{
    public bool HasLake => LakeAreaM2 > 0;
}

/// <summary>Everything the resource rules read for one land patch. The layer is a pure function of this.</summary>
/// <param name="Seed">The foundation seed.</param>
/// <param name="Patch">The grid coordinate of the patch (its root cell).</param>
/// <param name="Zone">The grid coordinate of the patch's zone (its root cell).</param>
/// <param name="Biome">The biome record, estimated or final.</param>
/// <param name="ZoneType">The elevation type of the zone (never Ocean).</param>
/// <param name="Bedrock">The bedrock id of the zone, one of <see cref="BedrockIds"/>.</param>
/// <param name="Coastal">One of the patch's cells has an ocean patch cell as neighbour.</param>
/// <param name="RiverPoor">The patch lies in a river-poor flat zone ("sparse").</param>
/// <param name="Water">The water site.</param>
/// <param name="Prosperity">The Prosperity field at the patch's site, rescaled to 0–1.</param>
/// <param name="Supernatural">The Supernatural field at the patch's site, 0–100.</param>
public readonly record struct LandResourceInput(
    uint Seed,
    GridCoord Patch,
    GridCoord Zone,
    BiomeRecord Biome,
    ElevationType ZoneType,
    string? Bedrock,
    bool Coastal,
    bool RiverPoor,
    WaterSite Water,
    double Prosperity,
    double Supernatural);

/// <summary>Everything the resource rules read for one ocean patch (Appendix D.3).</summary>
/// <param name="Seed">The foundation seed.</param>
/// <param name="Patch">The grid coordinate of the patch (its root cell).</param>
/// <param name="OnShelf">One of the patch's cells has a land patch as neighbour.</param>
/// <param name="SeaLevelTemperatureC">The Initial Temperature field at the patch's site.</param>
/// <param name="Prosperity">The Prosperity field at the patch's site, rescaled to 0–1.</param>
public readonly record struct OceanResourceInput(
    uint Seed,
    GridCoord Patch,
    bool OnShelf,
    double SeaLevelTemperatureC,
    double Prosperity);
