using Gameworld.Domain;
using Gameworld.Generation.Basics;
using Gameworld.Generation.Stages;

namespace Gameworld.Generation.Heights;

/// <summary>
/// The bed profile of one region's river nodes (map-generation.md section 12.2): the rise of
/// every node above its parent, the caps, the levelling of low-gradient zones and the pushing
/// of caps downstream.
/// </summary>
internal sealed class BedProfile
{
    private const double GorgeSlope = 0.0005;
    private const int MaxLevellingRounds = 32;

    private readonly double _level;
    private readonly int[] _zone;        // a dense zone index per node
    private readonly double[] _zoneC;    // the gradient factor of each zone
    private readonly double[] _lowest;   // per zone: the lowest z(parent) over its entries
    private readonly double[] _channelSlope;

    public BedProfile(World world, DetailWork detail)
    {
        var hp = world.Parameters.Elevation.HeightMap;
        var nodes = detail.Rivers.Nodes;
        var n = nodes.Count;
        var seedR = HeightRules.ReliefSeed(world);
        _level = hp.LevelFlatZones;

        Count = n;
        Parent = new int[n];
        Fixed = new bool[n];
        X = new double[n];
        Y = new double[n];
        Dx = new double[n];
        Flow = new int[n];
        TypeHillslope = new double[n];
        Rise = new double[n];
        Cap = new double[n];
        Z = new double[n];
        _zone = new int[n];
        _channelSlope = new double[n];

        var zoneIndex = new Dictionary<long, int>();
        var zoneC = new List<double>();
        for (var k = 0; k < n; k++)
        {
            var node = nodes[k];
            Parent[k] = node.Parent;
            X[k] = node.Position.X;
            Y[k] = node.Position.Y;
            Flow[k] = node.Flow;
            Cap[k] = double.PositiveInfinity;

            var zoneRoot = detail.NodeInfo[k].Zone;
            var type = node.Wet ? ElevationType.Ocean : world.Cells.ZoneType(zoneRoot);
            if (!zoneIndex.TryGetValue(zoneRoot.Key, out var zi))
            {
                zi = zoneC.Count;
                zoneIndex[zoneRoot.Key] = zi;
                zoneC.Add(type == ElevationType.Ocean ? 1 : hp.RiverGradientFactor[type]);
            }
            _zone[k] = zi;

            // Roots and nodes over the sea are at 0.
            Fixed[k] = node.IsRoot || type == ElevationType.Ocean;
            if (Fixed[k]) continue;

            var parent = nodes[node.Parent];
            Dx[k] = node.Position.DistanceTo(parent.Position);
            var relief = HeightRules.Relief(seedR, hp.MountainBelts, X[k], Y[k]);
            _channelSlope[k] = hp.ChannelSlope * hp.RiverGradientFactor[type] * relief
                               * HeightRules.FlowFactor(node.Flow, hp.Concavity);
            TypeHillslope[k] = hp.Hillslope[type] * hp.HillslopeMultiplier * relief;
        }
        _zoneC = zoneC.ToArray();
        _lowest = new double[_zoneC.Length];
    }

    public int Count { get; }
    public int[] Parent { get; }
    /// <summary>A root or a node over the sea: its bed is 0.</summary>
    public bool[] Fixed { get; }
    public double[] X { get; }
    public double[] Y { get; }
    /// <summary>The distance to the parent.</summary>
    public double[] Dx { get; }
    public int[] Flow { get; }
    /// <summary>The unblurred hillslope at the node: type hillslope × multiplier × relief.</summary>
    public double[] TypeHillslope { get; }
    public double[] Rise { get; }
    public double[] Cap { get; }
    public double[] Z { get; }

    /// <summary>rise(k) = Δx · min(channelSlope, hillslope); null = the unblurred hillslope at the node.</summary>
    public void UseSlopes(double[]? hillslope)
    {
        for (var k = 0; k < Count; k++)
            Rise[k] = Fixed[k] ? 0 : Dx[k] * Math.Min(_channelSlope[k], (hillslope ?? TypeHillslope)[k]);
    }

    /// <summary>
    /// z(k) = max(z(parent), min(z(parent) + rise(k), cap(k))), in index order: a parent always
    /// has a smaller index than its children. Returns the largest change of z.
    /// </summary>
    public double Solve()
    {
        var largest = 0.0;
        for (var k = 0; k < Count; k++)
        {
            var z = 0.0;
            if (!Fixed[k])
            {
                var zp = Z[Parent[k]];
                z = Math.Max(zp, Math.Min(zp + Rise[k], Cap[k]));
            }
            var change = Math.Abs(z - Z[k]);
            if (change > largest) largest = change;
            Z[k] = z;
        }
        return largest;
    }

    /// <summary>
    /// A river cannot run uphill, so a cap also limits everything downstream of it:
    /// cap(parent) = min(cap(parent), max(0, cap(k) − min(rise(k), 0.0005 · Δx))), from the last node to the first.
    /// </summary>
    public void PushCaps()
    {
        for (var k = Count - 1; k > 0; k--)
        {
            // A node over the sea carries no cap: its bed is 0 whatever lies upstream.
            if (Fixed[k] || double.IsPositiveInfinity(Cap[k])) continue;
            var down = Math.Max(0, Cap[k] - Math.Min(Rise[k], GorgeSlope * Dx[k]));
            var p = Parent[k];
            if (down < Cap[p]) Cap[p] = down;
        }
    }

    /// <summary>
    /// Recomputes z and levels the low-gradient zones: every entry of a zone with a gradient
    /// factor below 1 is capped towards the zone's lowest entry, the caps are pushed downstream
    /// and z is recomputed, at most 32 times.
    /// </summary>
    public void Level()
    {
        Solve();
        for (var round = 0; round < MaxLevellingRounds; round++)
        {
            Array.Fill(_lowest, double.PositiveInfinity);
            for (var k = 0; k < Count; k++)
                if (IsEntry(k) && Z[Parent[k]] < _lowest[_zone[k]])
                    _lowest[_zone[k]] = Z[Parent[k]];

            var lowered = false;
            for (var k = 0; k < Count; k++)
            {
                if (!IsEntry(k)) continue;
                var c = _zoneC[_zone[k]];
                if (!(c < 1)) continue;
                var keep = 1 - _level * (1 - Math.Clamp(c, 0, 1));
                var lowest = _lowest[_zone[k]];
                var cap = lowest + keep * (Z[Parent[k]] - lowest) + Rise[k];
                if (cap < Cap[k] - 1)
                {
                    Cap[k] = cap;
                    lowered = true;
                }
            }
            if (!lowered) break;
            PushCaps();
            if (Solve() < 1e-6) break;
        }
    }

    /// <summary>An entry: a land node whose zone differs from its parent's zone.</summary>
    private bool IsEntry(int k) => !Fixed[k] && _zone[k] != _zone[Parent[k]];
}

/// <summary>
/// The river beds of the regions the height maps need (map-generation.md section 12.2). The
/// final beds of a region are a function of that region and the simple beds of its 8 grid
/// neighbours only, so they are the same on every raster they appear on. One instance belongs
/// to the World and caches the beds of every region it is asked for, whatever the order: a
/// region's beds are computed once, not once per height map that reads them. The arrays it
/// returns are shared and must not be changed.
/// </summary>
internal sealed class RiverBeds
{
    private const double CoarseCellM = 2000;
    private const int CoarseMarginCells = 4;
    private const double NeighbourReachM = 10_000;
    private const int MaxCapRounds = 6;
    private const double CapSlackM = 5;

    private readonly World _world;
    private readonly HeightMapParameters _hp;
    private readonly uint _seedR;
    private readonly Dictionary<long, double[]> _simple = new();
    private readonly Dictionary<long, double[]> _final = new();

    public RiverBeds(World world)
    {
        _world = world;
        _hp = world.Parameters.Elevation.HeightMap;
        _seedR = HeightRules.ReliefSeed(world);
        Floodplain = HeightRules.FloodplainOf(world);
    }

    public Floodplain Floodplain { get; }

    public DetailWork Detail(GridCoord region) => _world.DetailOf(new RegionId(region));

    /// <summary>The beds of a region with rise and levelling only, from the unblurred hillslope at each node.</summary>
    public double[] SimpleLevelled(GridCoord region)
    {
        if (_simple.TryGetValue(region.Key, out var beds)) return beds;
        beds = HeightRules.SimpleBeds(_world, Detail(region), levelling: true);
        _simple[region.Key] = beds;
        return beds;
    }

    /// <summary>The final beds of a region: rise, levelling and the "no river above its land" caps.</summary>
    public double[] Final(GridCoord region)
    {
        if (_final.TryGetValue(region.Key, out var beds)) return beds;
        beds = ComputeFinal(region);
        _final[region.Key] = beds;
        return beds;
    }

    private struct Segment
    {
        public double Ax, Ay, Za, Bx, By, Zb, HalfWidth, Slope;
        public int Ci, Cj;
    }

    private double[] ComputeFinal(GridCoord region)
    {
        var detail = Detail(region);
        var profile = new BedProfile(_world, detail);
        var n = profile.Count;
        if (n == 0) return [];
        var cells = _world.Cells;

        // The coarse grid: world-aligned 2 km cells over the bounding box of the region's river
        // nodes plus 4 cells on every side.
        double minX = double.PositiveInfinity, minY = double.PositiveInfinity;
        double maxX = double.NegativeInfinity, maxY = double.NegativeInfinity;
        for (var k = 0; k < n; k++)
        {
            minX = Math.Min(minX, profile.X[k]);
            maxX = Math.Max(maxX, profile.X[k]);
            minY = Math.Min(minY, profile.Y[k]);
            maxY = Math.Max(maxY, profile.Y[k]);
        }
        var gi0 = (int)Math.Floor(minX / CoarseCellM) - CoarseMarginCells;
        var gj0 = (int)Math.Floor(minY / CoarseCellM) - CoarseMarginCells;
        var gw = (int)Math.Floor(maxX / CoarseCellM) + CoarseMarginCells - gi0 + 1;
        var gh = (int)Math.Floor(maxY / CoarseCellM) + CoarseMarginCells - gj0 + 1;
        var size = gw * gh;

        // Sea or land at the cell centre, and the type of the land cells.
        var type = new byte[size];
        for (var j = 0; j < gh; j++)
            for (var i = 0; i < gw; i++)
            {
                var info = cells.CellAt((gi0 + i + 0.5) * CoarseCellM, (gj0 + j + 0.5) * CoarseCellM);
                type[j * gw + i] = info.Ocean ? (byte)0 : (byte)cells.ZoneType(info.Zone);
            }

        // The slope of a land cell: the type hillslope averaged over the land cells within ±2
        // cells (the window is clipped at the grid edge), × the multiplier × the relief at the centre.
        var slope = new double[size];
        var counts = new TypeCounts(type, gw, gh);
        Span<int> count = stackalloc int[5];
        for (var j = 0; j < gh; j++)
            for (var i = 0; i < gw; i++)
            {
                var c = j * gw + i;
                if (type[c] == 0) continue;
                counts.Window(i, j, 2, count);
                var total = count[1] + count[2] + count[3] + count[4];
                var mean = (count[1] * _hp.Hillslope.FlatLand + count[2] * _hp.Hillslope.Lowland
                            + count[3] * _hp.Hillslope.Hills + count[4] * _hp.Hillslope.Mountains) / total;
                slope[c] = mean * _hp.HillslopeMultiplier
                           * HeightRules.Relief(_seedR, _hp.MountainBelts, (gi0 + i + 0.5) * CoarseCellM, (gj0 + j + 0.5) * CoarseCellM);
            }

        // A node's slope is the slope of its cell; this is also the hillslope in rise(k).
        var nodeCi = new int[n];
        var nodeCj = new int[n];
        var nodeSlope = new double[n];
        for (var k = 0; k < n; k++)
        {
            nodeCi[k] = (int)Math.Floor(profile.X[k] / CoarseCellM) - gi0;
            nodeCj[k] = (int)Math.Floor(profile.Y[k] / CoarseCellM) - gj0;
            var c = nodeCj[k] * gw + nodeCi[k];
            // A land node whose coarse cell has its centre in the sea has no cell slope: it uses the unblurred hillslope at the node.
            nodeSlope[k] = type[c] != 0 ? slope[c] : profile.TypeHillslope[k];
        }
        profile.UseSlopes(nodeSlope);
        profile.Level();

        // The segments of the neighbouring regions (the 8 grid neighbours) whose node lies within
        // 10 km of the region's river bounding box, at their simple levelled beds.
        var foreign = new List<Segment>();
        for (var dj = -1; dj <= 1; dj++)
            for (var di = -1; di <= 1; di++)
            {
                if (di == 0 && dj == 0) continue;
                var other = region.Offset(di, dj);
                var otherDetail = Detail(other);
                var otherNodes = otherDetail.Rivers.Nodes;
                double[]? otherBeds = null;
                for (var k = 0; k < otherNodes.Count; k++)
                {
                    var node = otherNodes[k];
                    if (node.IsRoot || node.Wet) continue;
                    var p = node.Position;
                    if (p.X < minX - NeighbourReachM || p.X > maxX + NeighbourReachM
                        || p.Y < minY - NeighbourReachM || p.Y > maxY + NeighbourReachM) continue;
                    otherBeds ??= SimpleLevelled(other);
                    var parent = otherNodes[node.Parent];
                    var ci = (int)Math.Floor(p.X / CoarseCellM) - gi0;
                    var cj = (int)Math.Floor(p.Y / CoarseCellM) - gj0;
                    var inGrid = ci >= 0 && cj >= 0 && ci < gw && cj < gh && type[cj * gw + ci] != 0;
                    foreign.Add(new Segment
                    {
                        Ax = parent.Position.X, Ay = parent.Position.Y, Za = otherBeds[node.Parent],
                        Bx = p.X, By = p.Y, Zb = otherBeds[k],
                        HalfWidth = Floodplain.HalfWidth((otherBeds[node.Parent] + otherBeds[k]) / 2, node.Flow),
                        // Outside the grid or over a sea cell: the unblurred hillslope at the node.
                        Slope = inGrid
                            ? slope[cj * gw + ci]
                            : HeightRules.TypeHillslope(_world, cells.ZoneType(otherDetail.NodeInfo[k].Zone), p.X, p.Y),
                        Ci = ci, Cj = cj,
                    });
                }
            }

        // No river above its land: build the land surface on the coarse grid, cap the nodes that
        // stand above it, push the caps downstream, redo the levelling; at most 6 rounds.
        var surface = new double[size];
        var heap = new MinHeap();
        for (var round = 0; round < MaxCapRounds; round++)
        {
            for (var c = 0; c < size; c++) surface[c] = type[c] == 0 ? 0 : double.PositiveInfinity;

            for (var k = 0; k < n; k++)
            {
                if (profile.Fixed[k]) continue;
                var p = profile.Parent[k];
                Seed(new Segment
                {
                    Ax = profile.X[p], Ay = profile.Y[p], Za = profile.Z[p],
                    Bx = profile.X[k], By = profile.Y[k], Zb = profile.Z[k],
                    HalfWidth = Floodplain.HalfWidth((profile.Z[p] + profile.Z[k]) / 2, profile.Flow[k]),
                    Slope = nodeSlope[k], Ci = nodeCi[k], Cj = nodeCj[k],
                });
            }
            foreach (var segment in foreign) Seed(segment);

            // Spread with Dijkstra over land, 8 neighbours. Sea cells next to land are seeds at 0;
            // a step out of the sea costs the slope of the land cell, as on the raster (12.3).
            heap.Clear();
            for (var j = 0; j < gh; j++)
                for (var i = 0; i < gw; i++)
                {
                    var c = j * gw + i;
                    if (type[c] != 0)
                    {
                        if (!double.IsPositiveInfinity(surface[c])) heap.Push(surface[c], c);
                        continue;
                    }
                    if (Geometry.TouchesLand(type, gw, gh, i, j)) heap.Push(0, c);
                }
            while (heap.Count > 0)
            {
                var (key, c) = heap.Pop();
                if (key > surface[c]) continue;
                int ci = c % gw, cj = c / gw;
                var sea = type[c] == 0;
                for (var dj = -1; dj <= 1; dj++)
                    for (var di = -1; di <= 1; di++)
                    {
                        if (di == 0 && dj == 0) continue;
                        int ni = ci + di, nj = cj + dj;
                        if (ni < 0 || nj < 0 || ni >= gw || nj >= gh) continue;
                        var nc = nj * gw + ni;
                        if (type[nc] == 0) continue;
                        var mean = sea ? slope[nc] : 0.5 * (slope[c] + slope[nc]);
                        var next = key + CoarseCellM * mean * (di != 0 && dj != 0 ? DetMath.Sqrt2 : 1);
                        if (next < surface[nc])
                        {
                            surface[nc] = next;
                            heap.Push(next, nc);
                        }
                    }
            }

            // Cap every node that stands above the surface around it.
            var changed = false;
            for (var k = 0; k < n; k++)
            {
                if (profile.Fixed[k]) continue;
                var lowest = double.PositiveInfinity;
                for (var dj = -1; dj <= 1; dj++)
                    for (var di = -1; di <= 1; di++)
                    {
                        int ci = nodeCi[k] + di, cj = nodeCj[k] + dj;
                        if (ci < 0 || cj < 0 || ci >= gw || cj >= gh) continue;
                        var c = cj * gw + ci;
                        var dx = (gi0 + ci + 0.5) * CoarseCellM - profile.X[k];
                        var dy = (gj0 + cj + 0.5) * CoarseCellM - profile.Y[k];
                        var v = surface[c] + Math.Max(nodeSlope[k], slope[c]) * Math.Sqrt(dx * dx + dy * dy);
                        if (v < lowest) lowest = v;
                    }
                var cap = lowest + CapSlackM;
                if (profile.Z[k] > cap && cap < profile.Cap[k] - 1)
                {
                    profile.Cap[k] = cap;
                    changed = true;
                }
            }
            if (!changed) break;
            profile.PushCaps();
            profile.Level();
        }
        return profile.Z;

        // Seeds the 3 × 3 cells around the segment's node: the bed at the projection of the cell
        // centre + max(node slope, cell slope) · max(0, distance − floodplain half-width).
        void Seed(in Segment s)
        {
            for (var dj = -1; dj <= 1; dj++)
                for (var di = -1; di <= 1; di++)
                {
                    int ci = s.Ci + di, cj = s.Cj + dj;
                    if (ci < 0 || cj < 0 || ci >= gw || cj >= gh) continue;
                    var c = cj * gw + ci;
                    if (type[c] == 0) continue;
                    var d = Geometry.DistanceToSegment(
                        (gi0 + ci + 0.5) * CoarseCellM, (gj0 + cj + 0.5) * CoarseCellM, s.Ax, s.Ay, s.Bx, s.By, out var t);
                    var v = s.Za + t * (s.Zb - s.Za) + Math.Max(s.Slope, slope[c]) * Math.Max(0, d - s.HalfWidth);
                    if (v < surface[c]) surface[c] = v;
                }
        }
    }
}

/// <summary>
/// How many cells of each elevation type lie in a window of a grid, from integer summed-area
/// tables: exact, so a box blur built on it does not depend on where the grid starts.
/// </summary>
internal sealed class TypeCounts
{
    private readonly int[][] _sums = new int[5][];
    private readonly int _width, _height, _stride;

    /// <param name="type">0 = ocean, 1–4 = the land elevation types.</param>
    public TypeCounts(byte[] type, int width, int height)
    {
        _width = width;
        _height = height;
        _stride = width + 1;
        for (var t = 1; t <= 4; t++) _sums[t] = new int[_stride * (height + 1)];
        for (var j = 0; j < height; j++)
            for (var i = 0; i < width; i++)
            {
                var here = type[j * width + i];
                for (var t = 1; t <= 4; t++)
                {
                    var s = _sums[t];
                    s[(j + 1) * _stride + i + 1] =
                        s[j * _stride + i + 1] + s[(j + 1) * _stride + i] - s[j * _stride + i] + (here == t ? 1 : 0);
                }
            }
    }

    /// <summary>The land cells of each type within ±radius cells of (i, j), clipped to the grid; counts[1..4].</summary>
    public void Window(int i, int j, int radius, Span<int> counts)
    {
        var i0 = Math.Max(0, i - radius);
        var j0 = Math.Max(0, j - radius);
        var i1 = Math.Min(_width, i + radius + 1);
        var j1 = Math.Min(_height, j + radius + 1);
        for (var t = 1; t <= 4; t++)
        {
            var s = _sums[t];
            counts[t] = s[j1 * _stride + i1] - s[j0 * _stride + i1] - s[j1 * _stride + i0] + s[j0 * _stride + i0];
        }
    }
}

internal static class Geometry
{
    /// <summary>A cell of a grid of types (0 = sea) has a land cell among the eight around it.</summary>
    public static bool TouchesLand(byte[] type, int width, int height, int i, int j)
    {
        for (var dj = -1; dj <= 1; dj++)
            for (var di = -1; di <= 1; di++)
            {
                int ni = i + di, nj = j + dj;
                if (ni >= 0 && nj >= 0 && ni < width && nj < height && type[nj * width + ni] != 0) return true;
            }
        return false;
    }

    /// <summary>The distance from a point to the segment a → b; t is the position of the projection (0 at a, 1 at b).</summary>
    public static double DistanceToSegment(double px, double py, double ax, double ay, double bx, double by, out double t)
    {
        var vx = bx - ax;
        var vy = by - ay;
        var l2 = vx * vx + vy * vy;
        t = l2 > 0 ? ((px - ax) * vx + (py - ay) * vy) / l2 : 0;
        if (t < 0) t = 0;
        else if (t > 1) t = 1;
        var dx = px - (ax + t * vx);
        var dy = py - (ay + t * vy);
        return Math.Sqrt(dx * dx + dy * dy);
    }
}
