using System.Collections.Immutable;

namespace Gameworld.Domain.Model;

/// <summary>
/// What siting yields for a region. Sited is not a state a region is in: it needs only the
/// foundation, can be calculated for any region at any time, and is never persisted.
/// </summary>
public sealed record SitedRegion(
    RegionId Id,
    WorldPoint Site,
    ImmutableArray<ZoneCell> ZoneCells,
    OutlineQuota Quota) : IRegionStage;

/// <summary>A zone-level Voronoi site, with what the zone starts out as.</summary>
public sealed record ZoneCell(
    ZoneCellId Id,
    WorldPoint Site,
    ElevationType InitialElevation,
    BedrockId Bedrock,
    bool IsOceanSeed,
    ImmutableArray<PatchCell> PatchCells);

/// <summary>A patch-level Voronoi site.</summary>
public sealed record PatchCell(PatchCellId Id, WorldPoint Site);

/// <summary>
/// The determined number of links, expansions and mergers for a region. Rolled when the
/// region is Sited and carried out when it is Outlined.
/// </summary>
public sealed record OutlineQuota(ImmutableArray<OceanSeedQuota> OceanSeeds, int ZoneMergers);

/// <summary>What one ocean seed is going to do when its region is Outlined.</summary>
/// <param name="Seed">The ocean seed.</param>
/// <param name="Links">Whether the seed links to its nearest ocean seed, if one is within the maximum link distance.</param>
/// <param name="Expansions">The number of nearest land zones the seed converts to ocean.</param>
public sealed record OceanSeedQuota(ZoneCellId Seed, bool Links, int Expansions);
