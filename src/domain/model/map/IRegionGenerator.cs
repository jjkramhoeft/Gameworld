namespace Gameworld.Domain.Model;

/// <summary>
/// The generation steps of the map half. Every step is a pure function of the foundation
/// and its input, so the result can be evicted from storage and recomputed identically.
/// </summary>
public interface IRegionGenerator
{
    /// <summary>Sites a region. Needs the foundation only.</summary>
    SitedRegion Site(Foundation foundation, RegionId region);

    /// <summary>Ocean linking, ocean expansion and zone mergers.</summary>
    /// <param name="sited">The region and all regions within two rings, Sited.</param>
    OutlinedRegion Outline(Foundation foundation, Neighbourhood<SitedRegion> sited);

    /// <summary>Patch mergers and rivers.</summary>
    /// <param name="outlined">The region and all bordering regions, Outlined.</param>
    DetailedRegion Detail(Foundation foundation, Neighbourhood<OutlinedRegion> outlined);

    /// <summary>Heights and biomes.</summary>
    /// <param name="detailed">The region and all bordering regions, Detailed.</param>
    ElevatedRegion Elevate(Foundation foundation, Neighbourhood<DetailedRegion> detailed, IFields fields);
}
