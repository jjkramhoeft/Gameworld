using System.Collections.Immutable;

namespace Gameworld.Domain.Model;

/// <summary>
/// A region with its zones linked, expanded and merged. From here on the zones are final.
/// </summary>
public sealed record OutlinedRegion(SitedRegion Sited, ImmutableArray<Zone> Zones) : IRegionStage
{
    public RegionId Id => Sited.Id;

    /// <summary>
    /// A region has sea access if it has at least one ocean zone. Whether two coasts are on
    /// the same sea is not a region fact - it is answered by a search over Outlined regions.
    /// </summary>
    public bool HasSeaAccess => Zones.Any(zone => zone.IsOcean);

    /// <summary>The zone a zone cell ended up in.</summary>
    public Zone ZoneOf(ZoneCellId cell) => Zones.First(zone => zone.Cells.Contains(cell));
}

/// <summary>
/// One or more zone cells. A zone has the identity of its surviving cell, and the surviving
/// cell's elevation type and bedrock.
/// </summary>
/// <param name="Id">The path of the surviving zone cell.</param>
/// <param name="Cells">All zone cells of the zone, the survivor included.</param>
/// <param name="Elevation">The final elevation type.</param>
/// <param name="Bedrock">The bedrock type.</param>
/// <param name="OceanOrigin">How the zone became ocean. Null for a land zone.</param>
public sealed record Zone(
    ZoneId Id,
    ImmutableArray<ZoneCellId> Cells,
    ElevationType Elevation,
    BedrockId Bedrock,
    OceanOrigin? OceanOrigin)
{
    public bool IsOcean => Elevation == ElevationType.Ocean;
}
