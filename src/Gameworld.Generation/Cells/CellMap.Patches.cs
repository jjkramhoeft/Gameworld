using Gameworld.Domain;

namespace Gameworld.Generation.Cells;

/// <summary>A patch as the cell map knows it: its member cells and its neighbours.</summary>
public sealed class PatchShape
{
    /// <summary>The root cell: the identity of the patch.</summary>
    public required GridCoord Root { get; init; }
    /// <summary>The zone cell that owns the root cell.</summary>
    public required GridCoord ZoneCell { get; init; }
    /// <summary>The root cell of the patch's zone.</summary>
    public required GridCoord Zone { get; init; }
    public required GridCoord Region { get; init; }
    /// <summary>The site of the root cell: "the patch's site".</summary>
    public required WorldPoint Site { get; init; }
    /// <summary>The member patch cells, sorted by key.</summary>
    public required GridCoord[] Cells { get; init; }
    public required bool Ocean { get; init; }
    /// <summary>One of the member cells has a neighbour patch cell on the other side of the coast.</summary>
    public required bool Coastal { get; init; }
    /// <summary>The neighbouring patches (via the member cells' Gabriel neighbours), sorted by key.</summary>
    public required GridCoord[] Neighbours { get; init; }
}

/// <summary>The patches of one region: the patches of the zones whose root cell the region owns.</summary>
public sealed class RegionPatches
{
    private readonly Dictionary<long, PatchShape> _byRoot;

    public RegionPatches(GridCoord region, PatchShape[] patches)
    {
        Region = region;
        Patches = patches;
        _byRoot = patches.ToDictionary(p => p.Root.Key);
    }

    public GridCoord Region { get; }
    /// <summary>Sorted by key.</summary>
    public PatchShape[] Patches { get; }
    public PatchShape? Find(GridCoord root) => _byRoot.GetValueOrDefault(root.Key);
}

public sealed partial class CellMap
{
    private readonly Dictionary<long, RegionPatches> _regionPatches = new();
    private readonly Dictionary<long, bool> _bay = new();

    /// <summary>The patches of a region. They depend on the cell map only, not on any later layer.</summary>
    public RegionPatches PatchesOf(GridCoord region)
    {
        if (_regionPatches.TryGetValue(region.Key, out var cached)) return cached;

        var members = new Dictionary<long, List<GridCoord>>();
        foreach (var zoneCell in ZoneCellsDrawnIn(region))
            foreach (var pc in OwnedPatchCells(zoneCell))
            {
                var root = PatchRoot(pc);
                if (!members.TryGetValue(root.Key, out var list))
                {
                    list = new List<GridCoord>();
                    members[root.Key] = list;
                }
                list.Add(pc);
            }

        var shapes = new List<PatchShape>(members.Count);
        foreach (var (rootKey, list) in members)
        {
            var root = GridCoord.FromKey(rootKey);
            var info = InfoOf(root);
            list.Sort();
            var coastal = false;
            var neighbours = new SortedSet<GridCoord>();
            foreach (var pc in list)
            {
                coastal |= IsCoastalPatchCell(pc);
                foreach (var nb in Patches.Neighbours(pc))
                {
                    var other = PatchRoot(nb);
                    if (other != root) neighbours.Add(other);
                }
            }
            shapes.Add(new PatchShape
            {
                Root = root,
                ZoneCell = info.ZoneCell,
                Zone = info.Zone,
                Region = info.Region,
                Site = Patches.Site(root),
                Cells = list.ToArray(),
                Ocean = info.Ocean,
                Coastal = coastal,
                Neighbours = neighbours.ToArray(),
            });
        }
        shapes.Sort((a, b) => a.Root.Key.CompareTo(b.Root.Key));
        cached = new RegionPatches(region, shapes.ToArray());
        _regionPatches[region.Key] = cached;
        return cached;
    }

    /// <summary>The shape of any patch, by its root cell.</summary>
    public PatchShape PatchShapeOf(GridCoord patchRoot)
    {
        var region = InfoOf(patchRoot).Region;
        return PatchesOf(region).Find(patchRoot)
            ?? throw new InvalidOperationException($"Patch {patchRoot} is not a patch root of region {region}.");
    }

    /// <summary>A bay: an ocean patch with at least 3 land neighbours and more land than ocean neighbours.</summary>
    public bool IsBay(GridCoord oceanPatchRoot)
    {
        if (_bay.TryGetValue(oceanPatchRoot.Key, out var bay)) return bay;
        var shape = PatchShapeOf(oceanPatchRoot);
        bay = false;
        if (shape.Ocean)
        {
            int land = 0, ocean = 0;
            foreach (var nb in shape.Neighbours)
                if (InfoOf(nb).Ocean) ocean++; else land++;
            bay = land >= 3 && land > ocean;
        }
        _bay[oceanPatchRoot.Key] = bay;
        return bay;
    }

    /// <summary>The land patch borders a bay.</summary>
    public bool HasHarbour(PatchShape landPatch)
    {
        if (landPatch.Ocean) return false;
        foreach (var nb in landPatch.Neighbours)
            if (InfoOf(nb).Ocean && IsBay(nb)) return true;
        return false;
    }
}
