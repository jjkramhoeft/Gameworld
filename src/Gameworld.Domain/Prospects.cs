namespace Gameworld.Domain;

/// <summary>The five factors every land patch is scored on, each in [0, 1].</summary>
public sealed record ProspectFactors(
    double Resources,
    double Geography,
    double Trade,
    double Climate,
    double Prosperity);

/// <summary>
/// What a Prospect knows about its patch: water, coast and position in the region.
/// River sizes are in patches drained.
/// </summary>
/// <param name="Coast">The patch has an ocean patch as neighbour.</param>
/// <param name="Harbour">The patch borders a bay.</param>
/// <param name="River">The largest river on the patch; 8 and up is navigable, 80 and up a large river.</param>
/// <param name="Mouth">The largest river mouth on the patch.</param>
/// <param name="Confluence">The smaller branch of the biggest river junction on the patch.</param>
/// <param name="HeadOfNavigation">The patch holds the upstream end of navigation.</param>
/// <param name="LakeAreaM2">The area of the largest lake in the patch, 0 without a lake.</param>
/// <param name="Depth">Patch steps to the nearest patch that touches the land of another region; 99 without a land connection to one.</param>
public sealed record ProspectFacts(
    bool Coast,
    bool Harbour,
    double River,
    double Mouth,
    double Confluence,
    bool HeadOfNavigation,
    double LakeAreaM2,
    int Depth);

/// <summary>
/// A potential location for a City or Town. It never touches the land of another region, and
/// it only reads the estimated biomes and resources.
/// </summary>
/// <param name="Patch">The patch the Prospect is on. A patch holds at most one Prospect.</param>
/// <param name="Spot">Where in the patch the Prospect is placed.</param>
/// <param name="ForCity">The Prospect is one of the region's Prospects for cities; otherwise it is for a town.</param>
/// <param name="Score">0 to 100.</param>
public sealed record Prospect(
    PatchId Patch,
    WorldPoint Spot,
    bool ForCity,
    double Score,
    ProspectFactors Factors,
    ProspectFacts Facts);

/// <summary>What a link of the travel network is.</summary>
public enum TravelLinkKind
{
    Road,
    RiverRoute,
    SeaLane,
}

/// <summary>One stretch of a route: a line of points of one kind.</summary>
public sealed record TravelStretch(TravelLinkKind Kind, IReadOnlyList<WorldPoint> Points);

/// <summary>
/// A link between two Prospects. The Prospects can be in two regions; the link is decided by
/// exactly one region, so the network does not depend on the order the regions are generated in.
/// </summary>
/// <param name="Kind">Road, or a water link named a river route or a sea lane after its longer part.</param>
/// <param name="FromRegion">The region of the first Prospect.</param>
/// <param name="From">The patch of the first Prospect.</param>
/// <param name="ToRegion">The region of the second Prospect.</param>
/// <param name="To">The patch of the second Prospect.</param>
/// <param name="Cost">The travel cost of the link.</param>
/// <param name="Route">The route. A water link can mix river and sea stretches.</param>
public sealed record TravelLink(
    TravelLinkKind Kind,
    RegionId FromRegion,
    PatchId From,
    RegionId ToRegion,
    PatchId To,
    double Cost,
    IReadOnlyList<TravelStretch> Route);

/// <summary>Roads, river routes and sea lanes that connect the Prospects: the links one region decides.</summary>
public sealed record TravelNetwork(IReadOnlyList<TravelLink> Links);
