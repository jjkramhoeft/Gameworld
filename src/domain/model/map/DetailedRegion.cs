using System.Collections.Immutable;

namespace Gameworld.Domain.Model;

/// <summary>A region with its patches merged and its rivers and lakes laid out.</summary>
public sealed record DetailedRegion(
    OutlinedRegion Outlined,
    ImmutableArray<Patch> Patches,
    ImmutableArray<River> Rivers,
    ImmutableArray<Lake> Lakes) : IRegionStage
{
    public RegionId Id => Outlined.Id;
}

/// <summary>
/// One or more patch cells. A patch has the identity of its surviving cell. Patches in ocean
/// zones skip the patch merger, so they always have exactly one cell.
/// </summary>
/// <param name="Id">The path of the surviving patch cell.</param>
/// <param name="Zone">The zone the patch currently belongs to. Not readable from the patch path.</param>
/// <param name="Cells">All patch cells of the patch, the survivor included.</param>
public sealed record Patch(PatchId Id, ZoneId Zone, ImmutableArray<PatchCellId> Cells);

/// <summary>
/// A river system: a tree of nodes that drains to one outlet. Rivers never cross region
/// borders.
/// </summary>
/// <param name="Id">Region + index.</param>
/// <param name="Nodes">The nodes of the tree. A node is referred to by its position in this list.</param>
/// <param name="Outlet">What the river drains into.</param>
public sealed record River(RiverId Id, ImmutableArray<RiverNode> Nodes, RiverOutlet Outlet);

/// <summary>A point on a river.</summary>
/// <param name="Position">Where the node is.</param>
/// <param name="Patch">The patch the node lies in.</param>
/// <param name="Downstream">The index of the next node towards the outlet. Null for the node at the outlet.</param>
public sealed record RiverNode(WorldPoint Position, PatchId Patch, int? Downstream);

/// <summary>What a river drains into.</summary>
public abstract record RiverOutlet;

/// <summary>The river reaches the sea in an ocean patch of its own region.</summary>
public sealed record SeaOutlet(PatchId Patch) : RiverOutlet;

/// <summary>The river ends in a lake.</summary>
public sealed record LakeOutlet(LakeId Lake) : RiverOutlet;

/// <summary>A lake. A feature inside one or more land patches - not a kind of patch.</summary>
public sealed record Lake(LakeId Id, WorldPoint Centre, ImmutableArray<PatchId> Patches);
