using Gameworld.Domain;
using Gameworld.Generation.Basics;

namespace Gameworld.Generation.Cells;

/// <summary>The classification of a point: every point of a patch cell gives the same answer.</summary>
/// <param name="PatchCell">The patch cell that contains the point (warped borders).</param>
/// <param name="Patch">The root cell of the patch.</param>
/// <param name="ZoneCell">The zone cell that owns the patch, by the site of the patch's root cell.</param>
/// <param name="Zone">The root cell of the zone.</param>
/// <param name="Region">The region that owns the zone, by the site of the zone's root cell.</param>
/// <param name="Ocean">The zone is ocean after the last expansion pass.</param>
public readonly record struct CellInfo(
    GridCoord PatchCell,
    GridCoord Patch,
    GridCoord ZoneCell,
    GridCoord Zone,
    GridCoord Region,
    bool Ocean);

/// <summary>
/// "The map" in the narrow sense (map-generation.md sections 4–8): the three-level cell
/// hierarchy, the oceans, the zone and patch mergers, the classification of a point and the
/// elevation type and bedrock of the zones. Every answer is a pure function of the foundation;
/// the dictionaries in here are only caches. Not thread-safe.
/// </summary>
public sealed partial class CellMap
{
    private const byte SeedFlag = 1;
    private const byte LinkedFlag = 2;
    private const long None = long.MinValue;

    private readonly uint _seed;
    private readonly ParameterSet _p;
    private readonly int _linkRange;
    private readonly int _passes;
    private readonly double[] _expansionCdf;
    private readonly int[] _elevationWeights;

    private readonly Dictionary<long, GridCoord> _zoneOwner = new();
    private readonly Dictionary<long, GridCoord> _patchOwner = new();
    private readonly Dictionary<long, GridCoord[]> _ownedZoneCells = new();
    private readonly Dictionary<long, GridCoord[]> _ownedPatchCells = new();
    private readonly Dictionary<long, GridCoord[]> _oceanSeeds = new();
    private readonly Dictionary<long, byte> _flags = new();
    private readonly HashSet<long> _linksProcessed = new();
    private readonly HashSet<long> _flagsReady = new();
    private readonly Dictionary<long, bool>[] _oceanAfter;
    private readonly Dictionary<long, GridCoord[]>[] _expansionPicks;
    private readonly Dictionary<long, HashSet<long>> _zoneMergeChosen = new();
    private readonly Dictionary<long, long> _zoneTarget = new();
    private readonly Dictionary<long, GridCoord> _zoneRoot = new();
    private readonly Dictionary<long, HashSet<long>> _patchMergeChosen = new();
    private readonly Dictionary<long, long> _patchTarget = new();
    private readonly Dictionary<long, GridCoord> _patchRoot = new();
    private readonly Dictionary<long, CellInfo> _info = new();
    private readonly Dictionary<long, GridCoord[]> _drawnZoneCells = new();
    private readonly Dictionary<long, bool> _coastalPatchCell = new();

    public CellMap(Foundation foundation)
    {
        _seed = foundation.Seed.Value;
        _p = foundation.Parameters;
        var s = _p.Siting;
        if (!s.NestLevels)
            throw new NotSupportedException(
                "NestLevels = false is not supported: every layer after the cell hierarchy assumes nesting.");

        // The sizes are kept nested.
        RegionSize = s.RegionGridSizeKm * 1000;
        ZoneSize = Math.Clamp(s.ZoneGridSizeKm * 1000, RegionSize / 16, RegionSize / 1.5);
        PatchSize = Math.Clamp(s.PatchGridSizeKm * 1000, ZoneSize / 16, ZoneSize / 1.5);

        Regions = new VoronoiLevel(_seed ^ 0x1b873593u, RegionSize, s.RegionJitter, 0, 0);
        Zones = new VoronoiLevel(_seed ^ 0x7f4a7c15u, ZoneSize, s.ZoneJitter, s.ZoneWarp, s.ZoneWarpFrequency);
        Patches = new VoronoiLevel(_seed ^ 0x6c8e9cf5u, PatchSize, s.PatchJitter, s.PatchWarp, s.PatchWarpFrequency);

        _linkRange = Math.Max(0, _p.Outline.OceanLinkRangeRegions);
        _passes = Math.Max(0, _p.Outline.OceanExpansionPasses);
        _oceanAfter = new Dictionary<long, bool>[_passes + 1];
        _expansionPicks = new Dictionary<long, GridCoord[]>[_passes + 1];
        for (var i = 0; i <= _passes; i++)
        {
            _oceanAfter[i] = new Dictionary<long, bool>();
            _expansionPicks[i] = new Dictionary<long, GridCoord[]>();
        }

        // Normalised weights (negative → 0) as a cdf.
        var w = _p.Outline.OceanExpansionWeights.Take(4).Select(v => Math.Max(0, v)).ToArray();
        var total = w.Sum();
        _expansionCdf = new double[w.Length];
        var acc = 0.0;
        for (var k = 0; k < w.Length; k++)
        {
            acc += total > 0 ? w[k] / total : 0;
            _expansionCdf[k] = acc;
        }

        var ew = s.ElevationWeights;
        _elevationWeights = [Math.Max(0, ew.FlatLand), Math.Max(0, ew.Lowland), Math.Max(0, ew.Hills), Math.Max(0, ew.Mountains)];
    }

    public double RegionSize { get; }
    public double ZoneSize { get; }
    public double PatchSize { get; }
    public VoronoiLevel Regions { get; }
    public VoronoiLevel Zones { get; }
    public VoronoiLevel Patches { get; }
    public int ExpansionPasses => _passes;

    // ───────────────────────── Ownership (section 4) ─────────────────────────

    /// <summary>The region that contains the site of a zone cell.</summary>
    public GridCoord RegionOfZoneCell(GridCoord zoneCell)
    {
        if (_zoneOwner.TryGetValue(zoneCell.Key, out var r)) return r;
        r = Regions.Query(Zones.SiteX(zoneCell.I, zoneCell.J), Zones.SiteY(zoneCell.I, zoneCell.J));
        _zoneOwner[zoneCell.Key] = r;
        return r;
    }

    /// <summary>The zone cell that contains the site of a patch cell.</summary>
    public GridCoord ZoneCellOfPatchCell(GridCoord patchCell)
    {
        if (_patchOwner.TryGetValue(patchCell.Key, out var z)) return z;
        z = Zones.Query(Patches.SiteX(patchCell.I, patchCell.J), Patches.SiteY(patchCell.I, patchCell.J));
        _patchOwner[patchCell.Key] = z;
        return z;
    }

    /// <summary>The zone cells a region owns, in grid order (row by row: j outer, i inner).</summary>
    public GridCoord[] OwnedZoneCells(GridCoord region)
    {
        if (_ownedZoneCells.TryGetValue(region.Key, out var cells)) return cells;
        var i0 = (int)Math.Floor((region.I - 1) * RegionSize / ZoneSize);
        var i1 = (int)Math.Floor((region.I + 2) * RegionSize / ZoneSize);
        var j0 = (int)Math.Floor((region.J - 1) * RegionSize / ZoneSize);
        var j1 = (int)Math.Floor((region.J + 2) * RegionSize / ZoneSize);
        var list = new List<GridCoord>();
        for (var j = j0; j <= j1; j++)
            for (var i = i0; i <= i1; i++)
            {
                var c = new GridCoord(i, j);
                if (RegionOfZoneCell(c) == region) list.Add(c);
            }
        cells = list.ToArray();
        _ownedZoneCells[region.Key] = cells;
        return cells;
    }

    /// <summary>The patch cells a zone cell owns, in grid order.</summary>
    public GridCoord[] OwnedPatchCells(GridCoord zoneCell)
    {
        if (_ownedPatchCells.TryGetValue(zoneCell.Key, out var cells)) return cells;
        var m = (int)Math.Ceiling(Zones.Warp) + 1;
        var i0 = (int)Math.Floor((zoneCell.I - m) * ZoneSize / PatchSize);
        var i1 = (int)Math.Floor((zoneCell.I + 1 + m) * ZoneSize / PatchSize);
        var j0 = (int)Math.Floor((zoneCell.J - m) * ZoneSize / PatchSize);
        var j1 = (int)Math.Floor((zoneCell.J + 1 + m) * ZoneSize / PatchSize);
        var list = new List<GridCoord>();
        for (var j = j0; j <= j1; j++)
            for (var i = i0; i <= i1; i++)
            {
                var c = new GridCoord(i, j);
                if (ZoneCellOfPatchCell(c) == zoneCell) list.Add(c);
            }
        cells = list.ToArray();
        _ownedPatchCells[zoneCell.Key] = cells;
        return cells;
    }

    // ───────────────────────── Oceans (section 5) ─────────────────────────

    /// <summary>The zone cells a region chose as ocean seeds.</summary>
    public GridCoord[] OceanSeeds(GridCoord region)
    {
        if (_oceanSeeds.TryGetValue(region.Key, out var seeds)) return seeds;
        var rng = new Sfc32(Hashing.Hash4(_seed, 0x5e01, region.I, region.J));
        var range = _p.Siting.OceanSeedsPerRegion;
        var n = Hashing.Between(range.Min, range.Max, rng.Next());
        seeds = Hashing.Pick(OwnedZoneCells(region), n, rng);
        _oceanSeeds[region.Key] = seeds;
        return seeds;
    }

    public bool IsOceanSeed(GridCoord zoneCell) =>
        Array.IndexOf(OceanSeeds(RegionOfZoneCell(zoneCell)), zoneCell) >= 0;

    /// <summary>How many links an ocean seed makes: 0, 1 or 2.</summary>
    public int LinkCount(GridCoord seedCell)
    {
        if (Hashing.Unit(_seed, 0x5e05, seedCell.I, seedCell.J) < _p.Outline.OceanDoubleLinkChance) return 2;
        if (Hashing.Unit(_seed, 0x5e03, seedCell.I, seedCell.J) < _p.Outline.OceanLinkChance) return 1;
        return 0;
    }

    private void ProcessLinks(GridCoord region)
    {
        foreach (var s in OceanSeeds(region))
        {
            Flag(s, SeedFlag);
            var n = LinkCount(s);
            if (n == 0) continue;

            var sx = Zones.SiteX(s.I, s.J);
            var sy = Zones.SiteY(s.I, s.J);
            var others = new List<(double D, long Key, GridCoord Cell)>();
            for (var dj = -_linkRange; dj <= _linkRange; dj++)
                for (var di = -_linkRange; di <= _linkRange; di++)
                    foreach (var o in OceanSeeds(region.Offset(di, dj)))
                    {
                        if (o == s) continue;
                        var dx = Zones.SiteX(o.I, o.J) - sx;
                        var dy = Zones.SiteY(o.I, o.J) - sy;
                        others.Add((dx * dx + dy * dy, o.Key, o));
                    }
            others.Sort((a, b) => a.D != b.D ? a.D.CompareTo(b.D) : a.Key.CompareTo(b.Key));

            foreach (var (_, _, t) in others.Take(n))
            {
                Flag(s, LinkedFlag);
                Flag(t, LinkedFlag);
                var tx = Zones.SiteX(t.I, t.J);
                var ty = Zones.SiteY(t.I, t.J);
                var length = Math.Sqrt((tx - sx) * (tx - sx) + (ty - sy) * (ty - sy));
                var parts = Math.Max(1, (int)Math.Ceiling(length / (0.12 * ZoneSize)));
                for (var k = 1; k < parts; k++)
                {
                    var f = (double)k / parts;
                    Flag(Zones.Query(sx + (tx - sx) * f, sy + (ty - sy) * f), LinkedFlag);
                }
            }
        }
    }

    private void Flag(GridCoord zoneCell, byte flag)
    {
        _flags.TryGetValue(zoneCell.Key, out var f);
        _flags[zoneCell.Key] = (byte)(f | flag);
    }

    /// <summary>
    /// The SEED and LINKED flags of a zone cell. A zone cell can be flagged by links that start
    /// several regions away, so the seeds and links of every region within link range + 5 grid
    /// squares of its region are processed first.
    /// </summary>
    private byte Flags(GridCoord zoneCell)
    {
        var region = RegionOfZoneCell(zoneCell);
        if (_flagsReady.Add(region.Key))
        {
            var reach = _linkRange + RegionStageRules.OutlineRingsBeyondLinkRange;
            for (var dj = -reach; dj <= reach; dj++)
                for (var di = -reach; di <= reach; di++)
                {
                    var r = region.Offset(di, dj);
                    if (_linksProcessed.Add(r.Key)) ProcessLinks(r);
                }
        }
        return _flags.TryGetValue(zoneCell.Key, out var f) ? f : (byte)0;
    }

    /// <summary>The zone cell is on a link: an end seed or a chain cell.</summary>
    public bool IsLinked(GridCoord zoneCell) => (Flags(zoneCell) & LinkedFlag) != 0;

    /// <summary>How many land neighbours an expanding zone cell converts in a pass (1-based).</summary>
    public int ExpansionCount(GridCoord zoneCell, int pass)
    {
        var u = Hashing.Unit(_seed, unchecked((uint)(0x5e10 + pass)), zoneCell.I, zoneCell.J);
        for (var k = 0; k < _expansionCdf.Length; k++)
            if (u < _expansionCdf[k]) return k;
        return _expansionCdf.Length > 0 && _expansionCdf[^1] > 0 ? _expansionCdf.Length - 1 : 0;
    }

    private bool Expands(GridCoord zoneCell, int pass)
    {
        if (pass == 1)
        {
            var f = Flags(zoneCell);
            return _p.Outline.OceanExpansionFrom == OceanExpansionFrom.All ? f != 0 : (f & LinkedFlag) != 0;
        }
        return OceanAfter(zoneCell, pass - 1) && !OceanAfter(zoneCell, pass - 2);
    }

    private GridCoord[] ExpansionPicks(GridCoord zoneCell, int pass)
    {
        var memo = _expansionPicks[pass];
        if (memo.TryGetValue(zoneCell.Key, out var picks)) return picks;
        picks = [];
        if (Expands(zoneCell, pass))
        {
            var n = ExpansionCount(zoneCell, pass);
            if (n > 0)
                picks = Zones.Neighbours(zoneCell).Where(nb => !OceanAfter(nb, pass - 1)).Take(n).ToArray();
        }
        memo[zoneCell.Key] = picks;
        return picks;
    }

    /// <summary>The zone cell is ocean after an expansion pass (0 = seeds and links only).</summary>
    public bool OceanAfter(GridCoord zoneCell, int pass)
    {
        var memo = _oceanAfter[pass];
        if (memo.TryGetValue(zoneCell.Key, out var ocean)) return ocean;
        if (pass == 0)
        {
            ocean = Flags(zoneCell) != 0;
        }
        else
        {
            ocean = OceanAfter(zoneCell, pass - 1);
            if (!ocean)
                foreach (var g in Zones.Neighbours(zoneCell))
                    if (Array.IndexOf(ExpansionPicks(g, pass), zoneCell) >= 0)
                    {
                        ocean = true;
                        break;
                    }
        }
        memo[zoneCell.Key] = ocean;
        return ocean;
    }

    /// <summary>The zone cell is ocean after the last expansion pass, before the mergers.</summary>
    public bool IsOceanCell(GridCoord zoneCell) => OceanAfter(zoneCell, _passes);

    // ───────────────────────── Mergers (section 6) ─────────────────────────

    /// <summary>The determined number of zone mergers of a region.</summary>
    public int ZoneMergerCount(GridCoord region) => ZoneMergeChosen(region).Count;

    private HashSet<long> ZoneMergeChosen(GridCoord region)
    {
        if (_zoneMergeChosen.TryGetValue(region.Key, out var chosen)) return chosen;
        var rng = new Sfc32(Hashing.Hash4(_seed, 0x5e02, region.I, region.J));
        var range = _p.Outline.ZoneMergersPerRegion;
        var n = Hashing.Between(range.Min, range.Max, rng.Next());
        chosen = Hashing.Pick(OwnedZoneCells(region), n, rng).Select(c => c.Key).ToHashSet();
        _zoneMergeChosen[region.Key] = chosen;
        return chosen;
    }

    /// <summary>The zone cell is one of the cells its region chose to merge away.</summary>
    public bool IsChosenForZoneMerger(GridCoord zoneCell) =>
        ZoneMergeChosen(RegionOfZoneCell(zoneCell)).Contains(zoneCell.Key);

    /// <summary>The zone cell a zone cell merges into, or null when it does not merge.</summary>
    public GridCoord? ZoneMergeTarget(GridCoord zoneCell)
    {
        if (!_zoneTarget.TryGetValue(zoneCell.Key, out var target))
        {
            target = None;
            if (IsChosenForZoneMerger(zoneCell))
            {
                var ocean = IsOceanCell(zoneCell);
                foreach (var nb in Zones.Neighbours(zoneCell))
                    if (_p.Outline.MergeAcrossCoast || IsOceanCell(nb) == ocean)
                    {
                        target = nb.Key;
                        break;
                    }
            }
            _zoneTarget[zoneCell.Key] = target;
        }
        return target == None ? null : GridCoord.FromKey(target);
    }

    /// <summary>The root cell of the zone a zone cell belongs to.</summary>
    public GridCoord ZoneRoot(GridCoord zoneCell) => Root(zoneCell, _zoneRoot, ZoneMergeTarget);

    private HashSet<long> PatchMergeChosen(GridCoord zoneCell)
    {
        if (_patchMergeChosen.TryGetValue(zoneCell.Key, out var chosen)) return chosen;
        var rng = new Sfc32(Hashing.Hash4(_seed, 0x5e04, zoneCell.I, zoneCell.J));
        var range = _p.Detail.PatchMergersPerZoneCell;
        var n = Hashing.Between(range.Min, range.Max, rng.Next());
        chosen = Hashing.Pick(OwnedPatchCells(zoneCell), n, rng).Select(c => c.Key).ToHashSet();
        _patchMergeChosen[zoneCell.Key] = chosen;
        return chosen;
    }

    /// <summary>The patch cell is one of the cells its zone cell chose to merge away.</summary>
    public bool IsChosenForPatchMerger(GridCoord patchCell) =>
        PatchMergeChosen(ZoneCellOfPatchCell(patchCell)).Contains(patchCell.Key);

    /// <summary>The patch cell a patch cell merges into, or null when it does not merge.</summary>
    public GridCoord? PatchMergeTarget(GridCoord patchCell)
    {
        if (!_patchTarget.TryGetValue(patchCell.Key, out var target))
        {
            target = None;
            if (IsChosenForPatchMerger(patchCell))
            {
                var zone = ZoneRoot(ZoneCellOfPatchCell(patchCell));
                foreach (var nb in Patches.Neighbours(patchCell))
                    if (ZoneRoot(ZoneCellOfPatchCell(nb)) == zone)
                    {
                        target = nb.Key;
                        break;
                    }
            }
            _patchTarget[patchCell.Key] = target;
        }
        return target == None ? null : GridCoord.FromKey(target);
    }

    /// <summary>The root cell of the patch a patch cell belongs to.</summary>
    public GridCoord PatchRoot(GridCoord patchCell) => Root(patchCell, _patchRoot, PatchMergeTarget);

    /// <summary>
    /// Follow the targets until a cell that has not merged. If the chain loops, the root is the
    /// cell with the smallest key in the loop. Give up after 64 steps.
    /// </summary>
    private static GridCoord Root(GridCoord cell, Dictionary<long, GridCoord> memo, Func<GridCoord, GridCoord?> targetOf)
    {
        if (memo.TryGetValue(cell.Key, out var root)) return root;
        var path = new List<GridCoord>();
        var cur = cell;
        while (true)
        {
            if (memo.TryGetValue(cur.Key, out root)) break;
            var target = targetOf(cur);
            if (target is null)
            {
                root = cur;
                break;
            }
            path.Add(cur);
            var loopStart = path.IndexOf(target.Value);
            if (loopStart >= 0)
            {
                root = path[loopStart];
                for (var k = loopStart + 1; k < path.Count; k++)
                    if (path[k].Key < root.Key) root = path[k];
                break;
            }
            if (path.Count >= 64)
            {
                root = target.Value;
                break;
            }
            cur = target.Value;
        }
        foreach (var c in path) memo[c.Key] = root;
        memo[cell.Key] = root;
        return root;
    }

    // ───────────────────────── Classifying a point (section 7) ─────────────────────────

    public CellInfo CellAt(double x, double y) => InfoOf(Patches.Query(x, y));

    public CellInfo CellAt(WorldPoint p) => InfoOf(Patches.Query(p.X, p.Y));

    /// <summary>The classification of every point of a patch cell.</summary>
    public CellInfo InfoOf(GridCoord patchCell)
    {
        if (_info.TryGetValue(patchCell.Key, out var info)) return info;
        var patch = PatchRoot(patchCell);
        var zoneCell = ZoneCellOfPatchCell(patch);
        var zone = ZoneRoot(zoneCell);
        info = new CellInfo(patchCell, patch, zoneCell, zone, RegionOfZoneCell(zone), IsOceanCell(zone));
        _info[patchCell.Key] = info;
        return info;
    }

    /// <summary>
    /// How many grid rings of regions are searched for the zone cells drawn in a region.
    /// map-generation.md section 7 says the 8 grid neighbours (1 ring), but with jitter 1 two
    /// regions can share a border while their grid squares are 2 apart, and a zone cell that
    /// merges across such a border would be drawn in a region that never lists it.
    /// </summary>
    public const int DrawnSearchRings = 2;

    /// <summary>
    /// The zone cells drawn in a region: the zone cells whose root cell belongs to the region,
    /// sorted by key. This differs from the zone cells the region owns because of mergers
    /// across region borders.
    /// </summary>
    public GridCoord[] ZoneCellsDrawnIn(GridCoord region)
    {
        if (_drawnZoneCells.TryGetValue(region.Key, out var cells)) return cells;
        var list = new List<GridCoord>();
        for (var dj = -DrawnSearchRings; dj <= DrawnSearchRings; dj++)
            for (var di = -DrawnSearchRings; di <= DrawnSearchRings; di++)
                foreach (var c in OwnedZoneCells(region.Offset(di, dj)))
                    if (RegionOfZone(c) == region)
                        list.Add(c);
        list.Sort();
        cells = list.ToArray();
        _drawnZoneCells[region.Key] = cells;
        return cells;
    }

    /// <summary>The region a zone cell is drawn in: the region that owns its root cell.</summary>
    public GridCoord RegionOfZone(GridCoord zoneCell) => RegionOfZoneCell(ZoneRoot(zoneCell));

    /// <summary>The zone cell belongs to an ocean zone.</summary>
    public bool IsOceanZone(GridCoord zoneCell) => IsOceanCell(ZoneRoot(zoneCell));

    /// <summary>A patch cell with at least one neighbour patch cell on the other side of the coast.</summary>
    public bool IsCoastalPatchCell(GridCoord patchCell)
    {
        if (_coastalPatchCell.TryGetValue(patchCell.Key, out var coastal)) return coastal;
        var ocean = InfoOf(patchCell).Ocean;
        coastal = false;
        foreach (var nb in Patches.Neighbours(patchCell))
            if (InfoOf(nb).Ocean != ocean)
            {
                coastal = true;
                break;
            }
        _coastalPatchCell[patchCell.Key] = coastal;
        return coastal;
    }

    // ───────────────────────── Elevation types and bedrock (sections 8 and 14.1) ─────────────────────────

    /// <summary>The land type drawn by weight for a zone cell, whether or not the cell is ocean.</summary>
    public ElevationType LandType(GridCoord zoneCell)
    {
        var total = _elevationWeights.Sum();
        if (total <= 0) return ElevationType.FlatLand;
        var h = (int)(Hashing.Hash4(_seed, 0x0c01, zoneCell.I, zoneCell.J) % (uint)total);
        for (var k = 0; k < 4; k++)
        {
            h -= _elevationWeights[k];
            if (h < 0) return ElevationType.FlatLand + k;
        }
        return ElevationType.Mountains;
    }

    /// <summary>The elevation type of the zone a zone cell belongs to: the type of its root cell, or Ocean.</summary>
    public ElevationType ZoneType(GridCoord zoneCell)
    {
        var root = ZoneRoot(zoneCell);
        return IsOceanCell(root) ? ElevationType.Ocean : LandType(root);
    }

    /// <summary>The bedrock id drawn for a zone cell by the weights of its land type; null when all weights are 0.</summary>
    public string? BedrockOfCell(GridCoord zoneCell)
    {
        var type = LandType(zoneCell);
        var total = 0;
        foreach (var b in _p.Bedrocks) total += Math.Max(0, b.Weights[type]);
        if (total <= 0) return null;
        var h = (int)(Hashing.Hash4(_seed, 0x5e01, zoneCell.I, zoneCell.J) % (uint)total);
        foreach (var b in _p.Bedrocks)
        {
            h -= Math.Max(0, b.Weights[type]);
            if (h < 0) return b.Id;
        }
        return null;
    }

    /// <summary>The bedrock of the zone a zone cell belongs to: the bedrock of its root cell; null for an ocean zone.</summary>
    public string? ZoneBedrock(GridCoord zoneCell)
    {
        var root = ZoneRoot(zoneCell);
        return IsOceanCell(root) ? null : BedrockOfCell(root);
    }
}
