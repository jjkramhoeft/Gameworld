namespace Gameworld.Domain;

/// <summary>
/// The region generation stages. Stages only advance - a region never moves backwards.
/// Evicting derived data from storage and recomputing it is not a stage change.
/// </summary>
/// <remarks>
/// Sited is not a state a stored region is in: it needs only the foundation, so it is a pure
/// function that can be calculated for any region at any time, and it is never persisted.
/// Stored regions start at <see cref="Outlined"/>.
/// </remarks>
public enum RegionStage
{
    /// <summary>Region, zone and patch cell location, zone cells initial type and bedrock, ocean seeds, the determined number of links, expansions and mergers.</summary>
    Sited,
    /// <summary>Zones linked, expanded and merged. Sea access.</summary>
    Outlined,
    /// <summary>Patch mergers, rivers, lakes. Estimated biomes and resources.</summary>
    Detailed,
    /// <summary>Height map. Final biomes and resources.</summary>
    Elevated,
    /// <summary>Factions and Prospects for cities and towns.</summary>
    Planned,
    /// <summary>Travel network. Actual cities and towns, families and important persons.</summary>
    Settled,
}

/// <summary>
/// What a stage needs: every region within <paramref name="Rings"/> grid rings of the region
/// (the region itself included, 0 rings = the region only) must be at least at
/// <paramref name="Stage"/>.
/// </summary>
public readonly record struct StageNeed(RegionStage Stage, int Rings);

/// <summary>
/// The stage preconditions of the readme. The policy for when a region advances is not part
/// of the domain model - the model only knows the stage and its preconditions.
/// </summary>
public static class RegionStageRules
{
    /// <summary>The rings beyond the link range that must be Sited before a region can be Outlined.</summary>
    public const int OutlineRingsBeyondLinkRange = 5;

    /// <summary>
    /// "Bordering regions" are taken as the 8 grid neighbours (1 ring), which is the
    /// neighbourhood map-generation.md works with (sections 7 and 16).
    /// </summary>
    public const int BorderingRings = 1;

    /// <summary>The stage that follows, or null after Settled.</summary>
    public static RegionStage? Next(RegionStage stage) =>
        stage == RegionStage.Settled ? null : stage + 1;

    /// <summary>What must hold around a region before it can be at <paramref name="stage"/>.</summary>
    public static StageNeed? NeedOf(RegionStage stage, ParameterSet parameters) => stage switch
    {
        // Foundation only.
        RegionStage.Sited => null,
        // All regions within the link range + 5 rings Sited (7 rings at the defaults).
        RegionStage.Outlined => new StageNeed(
            RegionStage.Sited,
            parameters.Outline.OceanLinkRangeRegions + OutlineRingsBeyondLinkRange),
        // All bordering regions Outlined.
        RegionStage.Detailed => new StageNeed(RegionStage.Outlined, BorderingRings),
        // All bordering regions Detailed.
        RegionStage.Elevated => new StageNeed(RegionStage.Detailed, BorderingRings),
        // The region Elevated. (Prospects read the Detailed data only, but the stages are linear.)
        RegionStage.Planned => new StageNeed(RegionStage.Elevated, 0),
        // The region and all bordering regions Planned.
        RegionStage.Settled => new StageNeed(RegionStage.Planned, BorderingRings),
        _ => throw new ArgumentOutOfRangeException(nameof(stage)),
    };

    /// <summary>All regions within <paramref name="rings"/> grid rings, the region itself included, row by row.</summary>
    public static IEnumerable<RegionId> RegionsWithin(RegionId region, int rings)
    {
        for (var dj = -rings; dj <= rings; dj++)
            for (var di = -rings; di <= rings; di++)
                yield return new RegionId(region.Coord.Offset(di, dj));
    }

    /// <summary>The 8 bordering regions.</summary>
    public static IEnumerable<RegionId> Bordering(RegionId region) =>
        RegionsWithin(region, BorderingRings).Where(r => r != region);

    /// <summary>
    /// True when the region can advance from <paramref name="current"/> to the next stage.
    /// <paramref name="stageOf"/> gives the stage of any region; a region that is not stored is Sited.
    /// </summary>
    public static bool CanAdvance(
        RegionId region,
        RegionStage current,
        Func<RegionId, RegionStage> stageOf,
        ParameterSet parameters)
    {
        if (Next(current) is not { } next)
            return false;
        if (NeedOf(next, parameters) is not { } need)
            return true;
        if (need.Stage == RegionStage.Sited)
            return true; // Sited can be calculated for any region at any time.
        return RegionsWithin(region, need.Rings).All(r => r == region
            ? current >= need.Stage
            : stageOf(r) >= need.Stage);
    }
}
