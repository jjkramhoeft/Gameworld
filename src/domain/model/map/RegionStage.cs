namespace Gameworld.Domain.Model;

/// <summary>
/// The stage of a stored region. Stages only advance. Sited is not here: it is a pure
/// function of the foundation that is never persisted, so stored regions start at Outlined.
/// </summary>
public enum RegionStage
{
    Outlined = 1,
    Detailed = 2,
    Elevated = 3,
    Planned = 4,
    Settled = 5,
}

/// <summary>
/// What must be true before a region can advance to a stage: every region within
/// <see cref="Rings"/> rings (the region itself included) is at least at
/// <see cref="NeighbourStage"/>. A null stage means Sited, which is always available.
/// </summary>
public readonly record struct StagePrecondition(RegionStage? NeighbourStage, int Rings);

public static class RegionStages
{
    /// <summary>
    /// The precondition for advancing to <paramref name="stage"/>. The policy for when a
    /// region advances is not part of the domain model.
    /// </summary>
    public static StagePrecondition PreconditionOf(RegionStage stage) => stage switch
    {
        RegionStage.Outlined => new(null, Rings: 2),
        RegionStage.Detailed => new(RegionStage.Outlined, Rings: 1),
        RegionStage.Elevated => new(RegionStage.Detailed, Rings: 1),
        RegionStage.Planned => new(RegionStage.Detailed, Rings: 0),
        RegionStage.Settled => new(RegionStage.Planned, Rings: 0),
        _ => throw new ArgumentOutOfRangeException(nameof(stage), stage, null),
    };
}

/// <summary>A region as it is at one generation stage.</summary>
public interface IRegionStage
{
    RegionId Id { get; }
}
