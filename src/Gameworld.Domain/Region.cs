namespace Gameworld.Domain;

/// <summary>
/// What the Outlined stage adds: zones linked, expanded and merged, and sea access.
/// </summary>
/// <param name="Zones">The zones of the region: the zones whose root cell the region owns - not the zone cells the region owns. Sorted by key.</param>
/// <param name="HasSeaAccess">An ocean zone touches the land of the region. The ocean zone can belong to a neighbouring region. A region without sea access is landlocked.</param>
public sealed record RegionOutline(IReadOnlyList<Zone> Zones, bool HasSeaAccess)
{
    public IEnumerable<Zone> LandZones => Zones.Where(z => !z.IsOcean);
    public IEnumerable<Zone> OceanZones => Zones.Where(z => z.IsOcean);
}

/// <summary>
/// What the Detailed stage adds: patch mergers, rivers, lakes and the estimated biomes and
/// resources. The estimated form is calculated with a typical height per elevation type
/// instead of the real height.
/// </summary>
/// <param name="Patches">The patches of the region's zones, with their estimated biome and resources. Sorted by key.</param>
public sealed record RegionDetail(IReadOnlyList<Patch> Patches, Rivers Rivers, IReadOnlyList<Lake> Lakes);

/// <summary>The final form of a land patch, calculated with the height from the height map.</summary>
/// <param name="MeanHeightM">The mean height of the patch's land; null when no raster cell centre lies in it.</param>
public sealed record PatchElevation(PatchId Patch, double? MeanHeightM, BiomeRecord Biome, PatchResources Resources);

/// <summary>What the Elevated stage adds: the height map and the final biomes and resources.</summary>
public sealed record RegionElevation(HeightMap HeightMap, IReadOnlyList<PatchElevation> Patches);

/// <summary>What the Planned stage adds: the Prospects for cities and towns.</summary>
/// <remarks>Factions belong to this stage too. The readme does not yet say what a faction holds, so they are not modelled.</remarks>
public sealed record RegionPlan(IReadOnlyList<Prospect> Prospects);

/// <summary>What the Settled stage adds: the travel network.</summary>
/// <remarks>Actual cities and towns, families and important persons belong to this stage too and are not modelled yet.</remarks>
public sealed record RegionSettlement(TravelNetwork Travel);

/// <summary>
/// A region as it is stored: its stage and what each stage it has reached added. Everything in
/// it is derived data, so the store is only a cache of it.
/// </summary>
public sealed record RegionSnapshot(
    RegionId Id,
    RegionStage Stage,
    RegionOutline Outline,
    RegionDetail? Detail,
    RegionElevation? Elevation,
    RegionPlan? Plan,
    RegionSettlement? Settlement);

/// <summary>
/// A stored region: the generation unit of the map. A region moves through the stages and only
/// advances. Everything it holds is derived data, so it can be evicted and recomputed.
/// </summary>
public sealed class Region
{
    /// <summary>Stored regions start at Outlined.</summary>
    public Region(RegionId id, RegionOutline outline)
    {
        Id = id;
        Outline = outline;
        Stage = RegionStage.Outlined;
    }

    public RegionId Id { get; }
    public RegionStage Stage { get; private set; }
    public RegionOutline Outline { get; }
    public RegionDetail? Detail { get; private set; }
    public RegionElevation? Elevation { get; private set; }
    public RegionPlan? Plan { get; private set; }
    public RegionSettlement? Settlement { get; private set; }

    /// <summary>The region as it is stored.</summary>
    public RegionSnapshot ToSnapshot() => new(Id, Stage, Outline, Detail, Elevation, Plan, Settlement);

    /// <summary>A region read back from storage. The snapshot must hold what every stage up to its own added.</summary>
    public static Region FromSnapshot(RegionSnapshot snapshot)
    {
        var region = new Region(snapshot.Id, snapshot.Outline);
        if (snapshot.Stage >= RegionStage.Detailed) region.Advance(snapshot.Detail ?? throw Missing(snapshot, RegionStage.Detailed));
        if (snapshot.Stage >= RegionStage.Elevated) region.Advance(snapshot.Elevation ?? throw Missing(snapshot, RegionStage.Elevated));
        if (snapshot.Stage >= RegionStage.Planned) region.Advance(snapshot.Plan ?? throw Missing(snapshot, RegionStage.Planned));
        if (snapshot.Stage >= RegionStage.Settled) region.Advance(snapshot.Settlement ?? throw Missing(snapshot, RegionStage.Settled));
        return region;
    }

    private static InvalidOperationException Missing(RegionSnapshot snapshot, RegionStage stage) =>
        new($"The snapshot of region {snapshot.Id} is {snapshot.Stage} but holds no {stage} data.");

    /// <summary>Outlined → Detailed.</summary>
    public void Advance(RegionDetail detail)
    {
        Require(RegionStage.Outlined);
        Detail = detail;
        Stage = RegionStage.Detailed;
    }

    /// <summary>Detailed → Elevated.</summary>
    public void Advance(RegionElevation elevation)
    {
        Require(RegionStage.Detailed);
        Elevation = elevation;
        Stage = RegionStage.Elevated;
    }

    /// <summary>Elevated → Planned.</summary>
    public void Advance(RegionPlan plan)
    {
        Require(RegionStage.Elevated);
        Plan = plan;
        Stage = RegionStage.Planned;
    }

    /// <summary>Planned → Settled.</summary>
    public void Advance(RegionSettlement settlement)
    {
        Require(RegionStage.Planned);
        Settlement = settlement;
        Stage = RegionStage.Settled;
    }

    private void Require(RegionStage stage)
    {
        if (Stage != stage)
            throw new InvalidOperationException(
                $"Region {Id} is {Stage}; only a region that is {stage} can advance to {stage + 1}.");
    }
}
