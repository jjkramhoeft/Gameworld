namespace Gameworld.Domain.Model;

/// <summary>
/// The authoritative elevation class of a zone. The actual height is calculated in the
/// Elevated stage and stays within the band the elevation type allows.
/// </summary>
public enum ElevationType
{
    Ocean = 0,
    Flat = 1,
    Lowland = 2,
    Hills = 3,
    Mountains = 4,
}

/// <summary>How an ocean zone became ocean. Kept to evaluate the ocean steps in phase 1.</summary>
public enum OceanOrigin
{
    /// <summary>The zone was an ocean seed when the region was Sited.</summary>
    Seed = 0,

    /// <summary>The zone lay on the line of an ocean link.</summary>
    Link = 1,

    /// <summary>The zone was converted by an ocean expansion.</summary>
    Expansion = 2,
}
