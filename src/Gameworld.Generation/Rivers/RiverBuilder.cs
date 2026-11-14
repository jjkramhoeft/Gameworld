using Gameworld.Domain;
using Gameworld.Generation.Basics;
using Gameworld.Generation.Cells;

namespace Gameworld.Generation.Rivers;

/// <summary>The grown rivers of one region with the counts the later layers and the statistics read.</summary>
/// <param name="Rivers">The river nodes as the domain sees them.</param>
/// <param name="Reached">The attractors reached at each node (flow before it is summed downstream).</param>
/// <param name="AttractorCount">The attractors scattered over the region.</param>
/// <param name="UnreachedCount">The attractors that were never reached or were given up.</param>
public sealed record RiverResult(Domain.Rivers Rivers, int[] Reached, int AttractorCount, int UnreachedCount)
{
    public IReadOnlyList<RiverNode> Nodes => Rivers.Nodes;
}

/// <summary>
/// Rivers (map-generation.md section 10): grown per region with the space colonization
/// algorithm, from river roots in the sea towards attraction points scattered over the land.
/// </summary>
internal sealed class RiverBuilder
{
    private const int MaxIterations = 6000;
    private const int MaxNodes = 120_000;
    private const int MaxChildren = 8;
    private const int DominanceGenerations = 47;
    private const int MaxBansPerAttractor = 12;
    private const int MaxFailedBridges = 6;

    private readonly CellMap _cells;
    private readonly GridCoord _region;
    private readonly uint _seed;
    private readonly RiverParameters _p;
    private readonly Func<GridCoord, bool> _isRiverPoorZone;

    private readonly double _step, _influence, _kill, _influence2, _kill2, _cosHalfCone;
    private readonly bool _seesAllRound;
    private bool _exempt;

    // Attractors.
    private readonly List<double> _ax = new(), _ay = new();
    private bool[] _alive = [];
    private int[] _near = [];
    private double[] _nearD2 = [];
    private int[] _banCount = [];
    private readonly Dictionary<int, HashSet<int>> _bans = new();
    private readonly Dictionary<long, List<int>> _attractorGrid = new();
    private int _aliveCount;

    // Nodes.
    private readonly List<double> _nx = new(), _ny = new(), _hx = new(), _hy = new(), _wx = new(), _wy = new();
    private readonly List<int> _parent = new(), _firstChild = new(), _lastChild = new(), _grown = new(), _reached = new();
    private readonly List<bool> _hasHeading = new(), _dead = new();
    private readonly Dictionary<long, List<int>> _nodeGrid = new();

    private RiverBuilder(CellMap cells, GridCoord region, uint seed, RiverParameters p, Func<GridCoord, bool> isRiverPoorZone)
    {
        _cells = cells;
        _region = region;
        _seed = seed;
        _p = p;
        _isRiverPoorZone = isRiverPoorZone;
        _step = p.StepKm * 1000;
        _kill = Math.Max(p.KillKm * 1000, 1.05 * _step);
        _influence = Math.Max(p.InfluenceKm * 1000, 1.5 * _step);
        _influence2 = _influence * _influence;
        _kill2 = _kill * _kill;
        _seesAllRound = p.PerceptionConeDegrees >= 360;
        _cosHalfCone = DetMath.Cos(p.PerceptionConeDegrees / 2 * Math.PI / 180);
    }

    public static RiverResult Build(CellMap cells, GridCoord region, uint seed, RiverParameters p, Func<GridCoord, bool> isRiverPoorZone)
    {
        var b = new RiverBuilder(cells, region, seed, p, isRiverPoorZone);
        b.ScatterAttractors();
        if (b._ax.Count > 0)
        {
            b.PlaceRoots();
            if (b._nx.Count > 0) b.Grow();
        }
        return b.Result();
    }

    // ───────────────────────── 10.1 Attractors ─────────────────────────

    private void ScatterAttractors()
    {
        var rng = new Sfc32(Hashing.Hash4(_seed, 0x7e01, _region.I, _region.J));
        var patches = _cells.Patches;
        var spread = 1.6 * _cells.PatchSize;
        foreach (var zoneCell in _cells.ZoneCellsDrawnIn(_region))
        {
            if (_cells.IsOceanZone(zoneCell)) continue;
            var sparse = _isRiverPoorZone(zoneCell);
            foreach (var pc in _cells.OwnedPatchCells(zoneCell))
            {
                var info = _cells.InfoOf(pc);
                if (info.Ocean || info.Region != _region) continue;
                if (_p.SkipCoastalPatchCells && _cells.IsCoastalPatchCell(pc)) continue;
                var count = sparse
                    ? (int)(Hashing.Hash4(_seed, 0x7e04, pc.I, pc.J) & 1)
                    : _p.AttractorsPerPatchCell;
                var sx = patches.SiteX(pc.I, pc.J);
                var sy = patches.SiteY(pc.I, pc.J);
                for (var c = 0; c < count; c++)
                {
                    var accepted = false;
                    for (var t = 0; t < 24 && !accepted; t++)
                    {
                        var qx = sx + (rng.Next() - 0.5) * spread;
                        var qy = sy + (rng.Next() - 0.5) * spread;
                        if (patches.Query(qx, qy) == pc)
                        {
                            _ax.Add(qx);
                            _ay.Add(qy);
                            accepted = true;
                        }
                    }
                    if (accepted) continue;
                    // Fall back to the site itself, provided it is land of this region.
                    var site = _cells.CellAt(sx, sy);
                    if (!site.Ocean && site.Region == _region)
                    {
                        _ax.Add(sx);
                        _ay.Add(sy);
                    }
                }
            }
        }

        var n = _ax.Count;
        _alive = new bool[n];
        _near = new int[n];
        _nearD2 = new double[n];
        _banCount = new int[n];
        for (var a = 0; a < n; a++)
        {
            _alive[a] = true;
            _near[a] = -1;
            _nearD2[a] = double.PositiveInfinity;
            Bucket(_attractorGrid, _ax[a], _ay[a]).Add(a);
        }
        _aliveCount = n;
    }

    // ───────────────────────── 10.2 River roots ─────────────────────────

    private void PlaceRoots()
    {
        var zones = _cells.Zones;
        var drawn = _cells.ZoneCellsDrawnIn(_region);
        var touching = new SortedSet<GridCoord>();
        foreach (var z in drawn)
        {
            if (_cells.IsOceanZone(z)) continue;
            foreach (var nb in zones.Neighbours(z))
                if (_cells.IsOceanZone(nb)) touching.Add(nb);
        }

        var roots = new SortedSet<GridCoord>(touching);
        if (touching.Count > 0)
        {
            // Every ocean zone cell drawn in the region that has a land neighbour.
            foreach (var z in drawn)
            {
                if (!_cells.IsOceanZone(z)) continue;
                if (zones.Neighbours(z).Any(nb => !_cells.IsOceanZone(nb))) roots.Add(z);
            }
        }
        else
        {
            // Landlocked: search outwards breadth-first over zone cell neighbours, up to 4 steps,
            // and take the first ocean zone cells found. The region is exempt from the border rule.
            _exempt = true;
            var seen = new HashSet<long>();
            var frontier = new List<GridCoord>();
            foreach (var z in drawn)
                if (!_cells.IsOceanZone(z) && seen.Add(z.Key)) frontier.Add(z);
            for (var depth = 1; depth <= 4 && roots.Count == 0 && frontier.Count > 0; depth++)
            {
                var next = new List<GridCoord>();
                foreach (var z in frontier)
                    foreach (var nb in zones.Neighbours(z))
                    {
                        if (!seen.Add(nb.Key)) continue;
                        if (_cells.IsOceanZone(nb)) roots.Add(nb);
                        else next.Add(nb);
                    }
                frontier = next;
            }
        }

        // Roots are created in order of ocean zone cell key.
        foreach (var z in roots)
        {
            var x = zones.SiteX(z.I, z.J);
            var y = zones.SiteY(z.I, z.J);
            if (!_cells.CellAt(x, y).Ocean)
            {
                // The site fell on land: use the site of the nearest patch cell of the zone cell whose site is ocean.
                var best = double.PositiveInfinity;
                var found = false;
                double bx = 0, by = 0;
                foreach (var pc in _cells.OwnedPatchCells(z))
                {
                    var px = _cells.Patches.SiteX(pc.I, pc.J);
                    var py = _cells.Patches.SiteY(pc.I, pc.J);
                    if (!_cells.CellAt(px, py).Ocean) continue;
                    var d = (px - x) * (px - x) + (py - y) * (py - y);
                    if (d < best)
                    {
                        best = d;
                        bx = px;
                        by = py;
                        found = true;
                    }
                }
                if (!found) continue;
                x = bx;
                y = by;
            }
            AddNode(x, y, -1, 0, 0, false, 0, 0, bridge: false);
        }
    }

    // ───────────────────────── 10.3 Growth ─────────────────────────

    private void Grow()
    {
        var rng = new Sfc32(Hashing.Hash4(_seed, 0x7e02, _region.I, _region.J));
        var apical = _p.MainStemDominance;
        var pullX = new List<double>();
        var pullY = new List<double>();
        var pullCount = new List<int>();
        var closest = new List<int>();
        var subtree = new List<int>();

        for (var iteration = 0; iteration < MaxIterations; iteration++)
        {
            if (_aliveCount == 0 || _nx.Count >= MaxNodes) break;

            // 1. Pulls.
            var n0 = _nx.Count;
            Resize(pullX, n0); Resize(pullY, n0); Resize(pullCount, n0); Resize(closest, n0, -1); Resize(subtree, n0);
            var anyPull = false;
            for (var a = 0; a < _alive.Length; a++)
            {
                if (!_alive[a]) continue;
                var k = _near[a];
                if (k < 0 || _nearD2[a] > _influence2) continue;
                var d = Math.Sqrt(_nearD2[a]);
                var w = _p.PullPower switch
                {
                    PullPower.Squared => (d / _influence) * (d / _influence),
                    PullPower.Root => Math.Sqrt(d / _influence),
                    _ => 1.0,
                };
                if (d > 0)
                {
                    pullX[k] += w * (_ax[a] - _nx[k]) / d;
                    pullY[k] += w * (_ay[a] - _ny[k]) / d;
                }
                pullCount[k]++;
                if (closest[k] < 0 || _nearD2[a] < _nearD2[closest[k]]) closest[k] = a;
                anyPull = true;
            }

            // 2. No pulls at all → bridge.
            if (!anyPull)
            {
                if (!Bridge()) break;
                continue;
            }

            // Main-stem dominance: the pulls on each node's subtree, up to 47 generations away.
            if (apical > 0)
                for (var k = 0; k < n0; k++)
                {
                    if (pullCount[k] == 0) continue;
                    var n = k;
                    for (var g = 0; g <= DominanceGenerations && n >= 0; g++)
                    {
                        subtree[n] += pullCount[k];
                        n = _parent[n];
                    }
                }

            // 3. Each pulled node, in index order, grows one child.
            for (var k = 0; k < n0; k++)
            {
                if (pullCount[k] == 0 || _dead[k]) continue;
                if (_nx.Count >= MaxNodes) break;
                var aim = closest[k];

                // 3.1 Main-stem dominance.
                if (apical > 0 && _grown[k] > 0 && _firstChild[k] >= 0
                    && pullCount[k] < apical * subtree[_firstChild[k]])
                {
                    var reach = _kill + 2 * _step;
                    for (var a = 0; a < _alive.Length; a++)
                    {
                        if (!_alive[a] || _near[a] != k || _nearD2[a] > _influence2) continue;
                        if (_nearD2[a] <= reach * reach) Reach(a, k);
                    }
                    continue;
                }

                // 3.2 Direction: the summed pull, normalised.
                double vx, vy;
                var len = Math.Sqrt(pullX[k] * pullX[k] + pullY[k] * pullY[k]);
                if (len > 1e-9)
                {
                    vx = pullX[k] / len;
                    vy = pullY[k] / len;
                }
                else
                {
                    (vx, vy) = Towards(k, aim);
                }

                // 3.3 Inertia.
                if (_p.Inertia != 0 && _hasHeading[k])
                    (vx, vy) = Normalise(vx + _p.Inertia * _hx[k], vy + _p.Inertia * _hy[k], vx, vy);

                // 3.4 Random motion, with the wander inherited from the step that created this node.
                var (rx, ry) = rng.NextUnitVector(1e-12);
                if (_p.Wander != 0)
                    (rx, ry) = Normalise(
                        _p.Wander * _wx[k] + (1 - _p.Wander) * rx,
                        _p.Wander * _wy[k] + (1 - _p.Wander) * ry,
                        rx, ry);
                (vx, vy) = Normalise(vx + _p.RandomMotion * rx, vy + _p.RandomMotion * ry, vx, vy);

                // 3.5 Target.
                var tx = _nx[k] + _step * vx;
                var ty = _ny[k] + _step * vy;

                // 3.6 Stall guard.
                var last = _lastChild[k];
                if (last >= 0)
                {
                    var ddx = tx - _nx[last];
                    var ddy = ty - _ny[last];
                    if (ddx * ddx + ddy * ddy < 0.05 * _step * 0.05 * _step)
                    {
                        (vx, vy) = Towards(k, aim);
                        tx = _nx[k] + _step * vx;
                        ty = _ny[k] + _step * vy;
                    }
                }

                // 3.7 Border rule.
                if (!StepAllowed(k, tx, ty))
                {
                    var banned = false;
                    for (var a = 0; a < _alive.Length; a++)
                    {
                        if (!_alive[a] || _near[a] != k) continue;
                        if (Clear(_nx[k], _ny[k], _ax[a], _ay[a])) continue;
                        Ban(a, k);
                        Reassign(a);
                        banned = true;
                    }
                    if (banned) continue; // try again next iteration with the remaining pulls

                    (vx, vy) = Towards(k, aim);
                    tx = _nx[k] + _step * vx;
                    ty = _ny[k] + _step * vy;
                    if (!StepAllowed(k, tx, ty))
                    {
                        Stop(k);
                        continue;
                    }
                }

                // 3.8 A node that has grown 8 children and wants another is stopped.
                if (_grown[k] >= MaxChildren)
                {
                    Stop(k);
                    continue;
                }

                // 3.9 Add the child.
                AddNode(tx, ty, k, vx, vy, true, rx, ry, bridge: false);
            }
        }
    }

    private (double X, double Y) Towards(int node, int attractor)
    {
        if (attractor < 0) return (1, 0);
        var dx = _ax[attractor] - _nx[node];
        var dy = _ay[attractor] - _ny[node];
        var d = Math.Sqrt(dx * dx + dy * dy);
        return d > 0 ? (dx / d, dy / d) : (1, 0);
    }

    private static (double X, double Y) Normalise(double x, double y, double fallbackX, double fallbackY)
    {
        var d = Math.Sqrt(x * x + y * y);
        return d > 1e-12 ? (x / d, y / d) : (fallbackX, fallbackY);
    }

    private int AddNode(double x, double y, int parent, double hx, double hy, bool hasHeading, double wx, double wy, bool bridge)
    {
        var index = _nx.Count;
        _nx.Add(x); _ny.Add(y);
        _parent.Add(parent);
        _hx.Add(hx); _hy.Add(hy); _hasHeading.Add(hasHeading);
        _wx.Add(wx); _wy.Add(wy);
        _firstChild.Add(-1); _lastChild.Add(-1); _grown.Add(0); _reached.Add(0); _dead.Add(false);
        if (parent >= 0)
        {
            if (_firstChild[parent] < 0) _firstChild[parent] = index;
            _lastChild[parent] = index;
            if (!bridge) _grown[parent]++;
        }
        Bucket(_nodeGrid, x, y).Add(index);

        // An attractor within the kill distance is reached; otherwise the new node may become its nearest node.
        var (gi, gj) = GridCell(x, y);
        for (var dj = -1; dj <= 1; dj++)
            for (var di = -1; di <= 1; di++)
            {
                if (!_attractorGrid.TryGetValue(Pack(gi + di, gj + dj), out var list)) continue;
                foreach (var a in list)
                {
                    if (!_alive[a]) continue;
                    var dx = _ax[a] - x;
                    var dy = _ay[a] - y;
                    var d2 = dx * dx + dy * dy;
                    if (d2 <= _kill2)
                        Reach(a, index);
                    else if (d2 < _nearD2[a] && CanSee(index, a))
                    {
                        _near[a] = index;
                        _nearD2[a] = d2;
                    }
                }
            }
        return index;
    }

    private void Reach(int attractor, int node)
    {
        _alive[attractor] = false;
        _aliveCount--;
        _reached[node]++;
    }

    /// <summary>The perception cone: the attractor must lie within ±cone/2 of the node's heading. Roots see all round.</summary>
    private bool CanSee(int node, int attractor)
    {
        if (_seesAllRound || !_hasHeading[node]) return true;
        var dx = _ax[attractor] - _nx[node];
        var dy = _ay[attractor] - _ny[node];
        var d = Math.Sqrt(dx * dx + dy * dy);
        if (d <= 0) return true;
        return (dx * _hx[node] + dy * _hy[node]) / d >= _cosHalfCone;
    }

    // ───────────────────────── 10.4 Bridges ─────────────────────────

    private bool Bridge()
    {
        for (var attempt = 0; attempt < MaxFailedBridges; attempt++)
        {
            var pairs = new List<(double D2, int Attractor, int Node)>();
            for (var a = 0; a < _alive.Length; a++)
            {
                if (!_alive[a]) continue;
                _bans.TryGetValue(a, out var banned);
                var best = double.PositiveInfinity;
                var bestNode = -1;
                for (var n = 0; n < _nx.Count; n++)
                {
                    if (_dead[n] || (banned != null && banned.Contains(n))) continue;
                    var dx = _ax[a] - _nx[n];
                    var dy = _ay[a] - _ny[n];
                    var d2 = dx * dx + dy * dy;
                    if (d2 < best)
                    {
                        best = d2;
                        bestNode = n;
                    }
                }
                if (bestNode >= 0) pairs.Add((best, a, bestNode));
            }
            pairs.Sort((p, q) => p.D2 != q.D2 ? p.D2.CompareTo(q.D2) : p.Attractor.CompareTo(q.Attractor));

            var newBans = false;
            foreach (var (d2, a, node) in pairs)
            {
                if (Clear(_nx[node], _ny[node], _ax[a], _ay[a]))
                {
                    // A straight run of nodes one step apart, stopping early if the attractor is reached.
                    var d = Math.Sqrt(d2);
                    var steps = Math.Max(1, (int)Math.Ceiling((d - _influence / 2) / _step));
                    var ux = d > 0 ? (_ax[a] - _nx[node]) / d : 1;
                    var uy = d > 0 ? (_ay[a] - _ny[node]) / d : 0;
                    var x0 = _nx[node];
                    var y0 = _ny[node];
                    var prev = node;
                    for (var s = 1; s <= steps && _alive[a] && _nx.Count < MaxNodes; s++)
                        prev = AddNode(x0 + ux * _step * s, y0 + uy * _step * s, prev, ux, uy, true, 0, 0, bridge: true);
                    return true;
                }
                Ban(a, node);
                newBans = true;
                if (_banCount[a] >= MaxBansPerAttractor && _alive[a])
                {
                    // An attractor with 12 banned nodes is given up.
                    _alive[a] = false;
                    _aliveCount--;
                    if (_near[a] >= 0) { _near[a] = -1; _nearD2[a] = double.PositiveInfinity; }
                }
            }
            if (!newBans) return false;
        }
        return false;
    }

    // ───────────────────────── 10.6 Staying inside the region ─────────────────────────

    /// <summary>A river node may lie over any ocean or on land of its own region, never on another region's land.</summary>
    private bool Inside(double x, double y)
    {
        if (_exempt) return true;
        var info = _cells.CellAt(x, y);
        return info.Ocean || info.Region == _region;
    }

    /// <summary>A step is allowed if its end point and its midpoint are inside.</summary>
    private bool StepAllowed(int node, double tx, double ty) =>
        _exempt || (Inside(tx, ty) && Inside((_nx[node] + tx) / 2, (_ny[node] + ty) / 2));

    /// <summary>A straight line that stays inside the region, sampled every half step.</summary>
    private bool Clear(double x0, double y0, double x1, double y1)
    {
        if (_exempt) return true;
        var d = Math.Sqrt((x1 - x0) * (x1 - x0) + (y1 - y0) * (y1 - y0));
        var n = Math.Max(1, (int)Math.Ceiling(d / (_step / 2)));
        for (var s = 1; s <= n; s++)
        {
            var f = (double)s / n;
            if (!Inside(x0 + (x1 - x0) * f, y0 + (y1 - y0) * f)) return false;
        }
        return true;
    }

    private void Ban(int attractor, int node)
    {
        if (!_bans.TryGetValue(attractor, out var set))
        {
            set = new HashSet<int>();
            _bans[attractor] = set;
        }
        if (set.Add(node)) _banCount[attractor]++;
    }

    private bool IsBanned(int attractor, int node) =>
        _bans.TryGetValue(attractor, out var set) && set.Contains(node);

    /// <summary>
    /// Among the nodes around the attractor that are not stopped, not banned for it and can see
    /// it, nearest first: the first one within the influence radius with a clear straight line
    /// becomes its nearest node; each one that fails the straight-line test is banned on the way.
    /// </summary>
    private void Reassign(int attractor)
    {
        _near[attractor] = -1;
        _nearD2[attractor] = double.PositiveInfinity;
        var x = _ax[attractor];
        var y = _ay[attractor];
        var (gi, gj) = GridCell(x, y);
        var candidates = new List<(double D2, int Node)>();
        for (var dj = -1; dj <= 1; dj++)
            for (var di = -1; di <= 1; di++)
            {
                if (!_nodeGrid.TryGetValue(Pack(gi + di, gj + dj), out var list)) continue;
                foreach (var n in list)
                {
                    if (_dead[n] || IsBanned(attractor, n) || !CanSee(n, attractor)) continue;
                    var dx = x - _nx[n];
                    var dy = y - _ny[n];
                    var d2 = dx * dx + dy * dy;
                    if (d2 <= _influence2) candidates.Add((d2, n));
                }
            }
        candidates.Sort((p, q) => p.D2 != q.D2 ? p.D2.CompareTo(q.D2) : p.Node.CompareTo(q.Node));
        foreach (var (d2, n) in candidates)
        {
            if (Clear(_nx[n], _ny[n], x, y))
            {
                _near[attractor] = n;
                _nearD2[attractor] = d2;
                return;
            }
            Ban(attractor, n);
        }
    }

    /// <summary>The node is stopped ("dead") and its attractors are re-assigned.</summary>
    private void Stop(int node)
    {
        _dead[node] = true;
        for (var a = 0; a < _alive.Length; a++)
            if (_alive[a] && _near[a] == node)
                Reassign(a);
    }

    // ───────────────────────── 10.7 Flow ─────────────────────────

    private RiverResult Result()
    {
        var n = _nx.Count;
        var flow = _reached.ToArray();
        for (var k = n - 1; k >= 0; k--)
            if (_parent[k] >= 0) flow[_parent[k]] += flow[k];

        var nodes = new RiverNode[n];
        var tree = new int[n];
        for (var k = 0; k < n; k++)
        {
            tree[k] = _parent[k] < 0 ? k : tree[_parent[k]];
            nodes[k] = new RiverNode(
                new WorldPoint(_nx[k], _ny[k]),
                _parent[k],
                flow[k],
                _cells.CellAt(_nx[k], _ny[k]).Ocean,
                tree[k]);
        }
        return new RiverResult(new Domain.Rivers(nodes, _exempt), _reached.ToArray(), _ax.Count, _aliveCount + GivenUp());
    }

    private int GivenUp()
    {
        // Attractors that are no longer alive but were never reached.
        var reached = 0;
        foreach (var r in _reached) reached += r;
        return _ax.Count - _aliveCount - reached;
    }

    // ───────────────────────── The grid of cell size dᵢ ─────────────────────────

    private (int I, int J) GridCell(double x, double y) =>
        ((int)Math.Floor(x / _influence), (int)Math.Floor(y / _influence));

    private static long Pack(int i, int j) => ((long)i << 32) ^ (uint)j;

    private List<int> Bucket(Dictionary<long, List<int>> grid, double x, double y)
    {
        var (i, j) = GridCell(x, y);
        var key = Pack(i, j);
        if (!grid.TryGetValue(key, out var list))
        {
            list = new List<int>();
            grid[key] = list;
        }
        return list;
    }

    private static void Resize<T>(List<T> list, int count, T fill = default!)
    {
        list.Clear();
        for (var i = 0; i < count; i++) list.Add(fill);
    }
}
