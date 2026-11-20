using Gameworld.Domain;
using Gameworld.Generation.Basics;
using Gameworld.Generation.Rivers;
using Gameworld.Generation.Settlements;

namespace Gameworld.Generation.Travel;

/// <summary>The working data of a Settled region.</summary>
public sealed class SettlementWork
{
    public required GridCoord Region { get; init; }
    /// <summary>The links this region decides.</summary>
    public required TravelLink[] Links { get; init; }
    /// <summary>All candidate links between the Prospects of the 3 × 3 regions, before pruning.</summary>
    public int CandidateCount { get; init; }
    /// <summary>The cities and towns of the region, and the persons on the land between them.</summary>
    public required RegionSettlements Places { get; init; }

    public RegionSettlement ToDomain() => new(new TravelNetwork(Links), Places.Settlements, Places.Rural);
}

/// <summary>
/// The travel network (map-generation.md section 16): roads, river routes and sea lanes
/// between the Prospects on show, computed from the Prospects and cost grids of the 3 × 3
/// regions around a region.
/// </summary>
internal static class TravelBuilder
{
    private const double Cell = CostGrid.CellSize;
    private const double CellKm = Cell / 1000;
    private const double Sqrt2 = 1.4142135623730951;
    private const double Sqrt5 = 2.23606797749979;

    private static readonly (int Dx, int Dy)[] Moves8 =
        [(1, 0), (0, 1), (-1, 0), (0, -1), (1, 1), (-1, 1), (-1, -1), (1, -1)];

    private static readonly (int Dx, int Dy)[] Knights =
        [(2, 1), (2, -1), (-2, 1), (-2, -1), (1, 2), (1, -2), (-1, 2), (-1, -2)];

    private sealed record Place(int Index, GridCoord Region, Prospect Prospect);

    private sealed record Candidate(int A, int B, bool Water, double Cost, int CellA, int CellB);

    /// <param name="settlements">The cities and towns of the region: they are carried into the working data as they are.</param>
    public static SettlementWork Build(World world, GridCoord region, RegionSettlements settlements)
    {
        var cells = world.Cells;

        // 16.2 Put the nine regions' cells on one raster.
        var around = new List<GridCoord>();
        for (var dj = -1; dj <= 1; dj++)
            for (var di = -1; di <= 1; di++)
                around.Add(region.Offset(di, dj));
        around.Sort();
        var grids = around.Select(r => world.CostGridOf(new RegionId(r))).ToArray();

        var i0 = grids.Min(g => g.I0);
        var j0 = grids.Min(g => g.J0);
        var w = grids.Max(g => g.I0 + g.Width) - i0;
        var h = grids.Max(g => g.J0 + g.Height) - j0;
        var n = w * h;
        var kind = new byte[n];
        var cost = new double[n];      // land: road cost × (1 + height / 1500 m); sea: lane cost
        var height = new float[n];
        var gridOf = new sbyte[n];
        var hasRiver = new bool[n];
        var linkNode = new int[n * 8]; // per 8-move: the river node of the link, −1 without one
        var linkGrid = new sbyte[n * 8];
        Array.Fill(linkNode, -1);

        for (var g = 0; g < grids.Length; g++)
        {
            var grid = grids[g];
            for (var y = 0; y < grid.Height; y++)
                for (var x = 0; x < grid.Width; x++)
                {
                    var k = grid.Kind[y * grid.Width + x];
                    if (k == CostGrid.NotOwn) continue;
                    var c = (grid.J0 - j0 + y) * w + grid.I0 - i0 + x;
                    kind[c] = k;
                    height[c] = grid.HeightM[y * grid.Width + x];
                    gridOf[c] = (sbyte)g;
                    cost[c] = k == CostGrid.Land
                        ? grid.Cost[y * grid.Width + x] * (1 + height[c] / 1500.0)
                        : grid.Cost[y * grid.Width + x];
                }
        }
        for (var g = 0; g < grids.Length; g++)
            foreach (var link in grids[g].Links)
            {
                int ax = link.Ia - i0, ay = link.Ja - j0, bx = link.Ib - i0, by = link.Jb - j0;
                if (ax < 0 || ay < 0 || bx < 0 || by < 0 || ax >= w || bx >= w || ay >= h || by >= h) continue;
                var a = ay * w + ax;
                var b = by * w + bx;
                if (kind[a] == CostGrid.NotOwn || kind[b] == CostGrid.NotOwn) continue;
                var m = MoveIndex(bx - ax, by - ay);
                var back = MoveIndex(ax - bx, ay - by);
                if (m < 0 || linkNode[a * 8 + m] >= 0) continue;
                linkNode[a * 8 + m] = link.Node;
                linkGrid[a * 8 + m] = (sbyte)g;
                linkNode[b * 8 + back] = link.Node;
                linkGrid[b * 8 + back] = (sbyte)g;
                hasRiver[a] = true;
                hasRiver[b] = true;
            }

        // The Prospects on show, in a fixed order: by region key, then in each region's own order.
        var places = new List<Place>();
        foreach (var r in around)
            foreach (var prospect in world.PlanOf(new RegionId(r)).Prospects)
                if (prospect.OnShow)
                    places.Add(new Place(places.Count, r, prospect));

        // Seeds.
        var roadDist = Filled(n);
        var roadOwner = FilledInt(n);
        var roadPrev = FilledInt(n);
        var waterDist = Filled(n);
        var waterOwner = FilledInt(n);
        var waterPrev = FilledInt(n);
        var roadHeap = new MinHeap();
        var waterHeap = new MinHeap();
        var portReach = (int)Math.Ceiling(1.5 * cells.PatchSize / Cell);

        foreach (var place in places)
        {
            var spot = place.Prospect.Spot;
            var px = (int)Math.Floor(spot.X / Cell) - i0;
            var py = (int)Math.Floor(spot.Y / Cell) - j0;

            // The nearest matching cell centre in a square window around the Prospect's own cell.
            int Nearest(int reach, Func<int, bool> matches)
            {
                var best = -1;
                var bestD = double.PositiveInfinity;
                for (var y = Math.Max(0, py - reach); y <= Math.Min(h - 1, py + reach); y++)
                    for (var x = Math.Max(0, px - reach); x <= Math.Min(w - 1, px + reach); x++)
                    {
                        var c = y * w + x;
                        if (!matches(c)) continue;
                        var d = CentreDistance(spot, i0 + x, j0 + y);
                        if (d < bestD)
                        {
                            bestD = d;
                            best = c;
                        }
                    }
                return best;
            }

            // Road forest: the nearest land cell that is not already another Prospect's seed, at cost 0.
            var road = Nearest(2, c => kind[c] == CostGrid.Land && roadOwner[c] < 0);
            if (road >= 0)
            {
                roadDist[road] = 0;
                roadOwner[road] = place.Index;
                roadHeap.Push(0, road);
            }

            void WaterSeed(int c, double seedCost)
            {
                if (c < 0 || seedCost >= waterDist[c]) return; // only where it is cheaper than a seed already there
                waterDist[c] = seedCost;
                waterOwner[c] = place.Index;
                waterPrev[c] = -1;
                waterHeap.Push(seedCost, c);
            }

            // Water forest: a river landing, and a port for coastal Prospects.
            var landing = Nearest(2, c => hasRiver[c]);
            if (landing >= 0)
                WaterSeed(landing, 6 + CentreDistance(spot, i0 + landing % w, j0 + landing / w) / 1000);
            if (place.Prospect.Facts.Coast)
            {
                var port = Nearest(portReach, c => kind[c] == CostGrid.Sea);
                if (port >= 0)
                    WaterSeed(port, 12 + CentreDistance(spot, i0 + port % w, j0 + port / w) / 1000);
            }
        }

        // Step costs.
        double RoadStep(int a, int dx, int dy)
        {
            var ax = a % w;
            var ay = a / w;
            var bx = ax + dx;
            var by = ay + dy;
            if (bx < 0 || by < 0 || bx >= w || by >= h) return -1;
            var b = by * w + bx;
            if (kind[a] != CostGrid.Land || kind[b] != CostGrid.Land) return -1;
            double c, climb, lengthKm;
            if (Math.Abs(dx) == 2 || Math.Abs(dy) == 2)
            {
                // A knight move needs the two cells it passes between to be land.
                int p1, p2;
                if (Math.Abs(dx) == 2)
                {
                    p1 = ay * w + ax + Math.Sign(dx);
                    p2 = by * w + ax + Math.Sign(dx);
                }
                else
                {
                    p1 = (ay + Math.Sign(dy)) * w + ax;
                    p2 = (ay + Math.Sign(dy)) * w + bx;
                }
                if (kind[p1] != CostGrid.Land || kind[p2] != CostGrid.Land) return -1;
                c = (cost[a] + cost[b] + cost[p1] + cost[p2]) / 4;
                var hm = (height[p1] + height[p2]) / 2.0;
                climb = Math.Abs(hm - height[a]) + Math.Abs(height[b] - hm);
                lengthKm = CellKm * Sqrt5;
            }
            else
            {
                c = (cost[a] + cost[b]) / 2;
                climb = Math.Abs(height[b] - height[a]);
                lengthKm = dx != 0 && dy != 0 ? CellKm * Sqrt2 : CellKm;
            }
            var slope = climb / (lengthKm * 1000);
            var steep = slope / 0.05;
            var step = lengthKm * c * (1 + Math.Min(12, steep * steep));
            if (hasRiver[a] != hasRiver[b]) step += 1.5; // half a bridge or ferry
            return step;
        }

        double WaterStep(int a, int move)
        {
            var (dx, dy) = Moves8[move];
            var ax = a % w;
            var ay = a / w;
            var bx = ax + dx;
            var by = ay + dy;
            if (bx < 0 || by < 0 || bx >= w || by >= h) return -1;
            var b = by * w + bx;
            var lengthKm = dx != 0 && dy != 0 ? CellKm * Sqrt2 : CellKm;
            if (linkNode[a * 8 + move] >= 0) return lengthKm * 0.4;
            if (kind[a] != CostGrid.Sea || kind[b] != CostGrid.Sea) return -1;
            // A diagonal sea move is not allowed when both cells it squeezes between are not sea.
            if (dx != 0 && dy != 0 && kind[ay * w + bx] != CostGrid.Sea && kind[by * w + ax] != CostGrid.Sea) return -1;
            return lengthKm * (cost[a] + cost[b]) / 2;
        }

        // Road forest: multi-source Dijkstra over land cells with 16 moves.
        while (roadHeap.Count > 0)
        {
            var (d, a) = roadHeap.Pop();
            if (d > roadDist[a]) continue;
            for (var m = 0; m < 16; m++)
            {
                var (dx, dy) = m < 8 ? Moves8[m] : Knights[m - 8];
                var step = RoadStep(a, dx, dy);
                if (step < 0) continue;
                var b = (a / w + dy) * w + a % w + dx;
                var nd = d + step;
                if (nd < roadDist[b])
                {
                    roadDist[b] = nd;
                    roadOwner[b] = roadOwner[a];
                    roadPrev[b] = a;
                    roadHeap.Push(nd, b);
                }
            }
        }

        // Water forest: multi-source Dijkstra with 8 moves.
        while (waterHeap.Count > 0)
        {
            var (d, a) = waterHeap.Pop();
            if (d > waterDist[a]) continue;
            for (var m = 0; m < 8; m++)
            {
                var step = WaterStep(a, m);
                if (step < 0) continue;
                var b = (a / w + Moves8[m].Dy) * w + a % w + Moves8[m].Dx;
                var nd = d + step;
                if (nd < waterDist[b])
                {
                    waterDist[b] = nd;
                    waterOwner[b] = waterOwner[a];
                    waterPrev[b] = a;
                    waterHeap.Push(nd, b);
                }
            }
        }

        // 16.3 Candidates: the cheapest crossing per pair of Prospects, separately for road and water.
        var best = new Dictionary<(int, int, bool), Candidate>();
        void Offer(int a, int b, bool water, double total, int[] owner)
        {
            int pa = owner[a], pb = owner[b];
            if (pa < 0 || pb < 0 || pa == pb) return;
            if (pa > pb)
            {
                (pa, pb) = (pb, pa);
                (a, b) = (b, a);
            }
            var key = (pa, pb, water);
            if (!best.TryGetValue(key, out var current) || total < current.Cost)
                best[key] = new Candidate(pa, pb, water, total, a, b);
        }
        for (var a = 0; a < n; a++)
        {
            if (roadOwner[a] >= 0)
                for (var m = 0; m < 16; m++)
                {
                    var (dx, dy) = m < 8 ? Moves8[m] : Knights[m - 8];
                    var step = RoadStep(a, dx, dy);
                    if (step < 0) continue;
                    var b = (a / w + dy) * w + a % w + dx;
                    if (roadOwner[b] >= 0 && roadOwner[b] != roadOwner[a])
                        Offer(a, b, false, roadDist[a] + step + roadDist[b], roadOwner);
                }
            if (waterOwner[a] >= 0)
                for (var m = 0; m < 8; m++)
                {
                    var step = WaterStep(a, m);
                    if (step < 0) continue;
                    var b = (a / w + Moves8[m].Dy) * w + a % w + Moves8[m].Dx;
                    if (waterOwner[b] >= 0 && waterOwner[b] != waterOwner[a])
                        Offer(a, b, true, waterDist[a] + step + waterDist[b], waterOwner);
                }
        }

        // "Cheaper" is a strict order: lower cost, then road before water, then the lower Prospect indices.
        var candidates = best.Values
            .OrderBy(c => c.Cost).ThenBy(c => c.Water).ThenBy(c => c.A).ThenBy(c => c.B)
            .ToArray();

        // Pruning, once per link against all candidate links, and ownership.
        var kept = new List<Candidate>();
        for (var k = 0; k < candidates.Length; k++)
        {
            var link = candidates[k];
            if (Owner(places[link.A].Region, places[link.B].Region) != region) continue;
            if (link.Water)
            {
                if (ShortestPath(candidates, k, places.Count, roadsOnly: false) <= 1.6 * link.Cost) continue;
            }
            else
            {
                // Other roads count at 1.6 ×; a route that uses water replaces a road only at 0.35 ×.
                if (ShortestPath(candidates, k, places.Count, roadsOnly: true) <= 1.6 * link.Cost) continue;
                if (ShortestPath(candidates, k, places.Count, roadsOnly: false) <= 0.35 * link.Cost) continue;
            }
            kept.Add(link);
        }

        // 16.4 Routes.
        var links = new List<TravelLink>();
        foreach (var link in kept)
        {
            var from = places[link.A];
            var to = places[link.B];
            var prev = link.Water ? waterPrev : roadPrev;
            var path = new List<int>();
            for (var c = link.CellA; c >= 0; c = prev[c]) path.Add(c);
            path.Reverse();
            for (var c = link.CellB; c >= 0; c = prev[c]) path.Add(c);

            WorldPoint Centre(int c) => new((i0 + c % w + 0.5) * Cell, (j0 + c / w + 0.5) * Cell);

            IReadOnlyList<TravelStretch> route;
            TravelLinkKind linkKind;
            if (!link.Water)
            {
                var points = path.Select(Centre).ToList();
                points[0] = from.Prospect.Spot;
                points[^1] = to.Prospect.Spot;
                route = [new TravelStretch(TravelLinkKind.Road, Smooth(points))];
                linkKind = TravelLinkKind.Road;
            }
            else
            {
                route = WaterRoute(world, grids, path, kind, linkNode, linkGrid, w, Centre, from.Prospect.Spot, to.Prospect.Spot, out linkKind);
            }

            links.Add(new TravelLink(
                linkKind,
                new RegionId(from.Region), from.Prospect.Patch,
                new RegionId(to.Region), to.Prospect.Patch,
                link.Cost,
                route));
        }

        return new SettlementWork
        {
            Region = region,
            Links = links.ToArray(),
            CandidateCount = candidates.Length,
            Places = settlements,
        };
    }

    /// <summary>
    /// The region that decides a link between Prospects of two regions: that region if they are
    /// the same; the smaller key if they are neighbours; the region in the middle if they are two
    /// apart; none if they are further apart.
    /// </summary>
    public static GridCoord? Owner(GridCoord a, GridCoord b)
    {
        var reach = Math.Max(Math.Abs(a.I - b.I), Math.Abs(a.J - b.J));
        return reach switch
        {
            0 => a,
            1 => a.Key < b.Key ? a : b,
            2 => new GridCoord(FloorHalf(a.I + b.I), FloorHalf(a.J + b.J)),
            _ => null,
        };
    }

    private static int FloorHalf(int v) => (int)Math.Floor(v / 2.0);

    /// <summary>The cheapest path between the ends of link k over the links that are strictly cheaper than it.</summary>
    private static double ShortestPath(Candidate[] sorted, int k, int placeCount, bool roadsOnly)
    {
        var target = sorted[k];
        var dist = new double[placeCount];
        Array.Fill(dist, double.PositiveInfinity);
        dist[target.A] = 0;
        var heap = new MinHeap();
        heap.Push(0, target.A);
        while (heap.Count > 0)
        {
            var (d, u) = heap.Pop();
            if (d > dist[u]) continue;
            if (u == target.B) return d;
            // The candidates are sorted by the strict order, so the cheaper links are those before k.
            for (var e = 0; e < k; e++)
            {
                var link = sorted[e];
                if (roadsOnly && link.Water) continue;
                int v;
                if (link.A == u) v = link.B;
                else if (link.B == u) v = link.A;
                else continue;
                var nd = d + link.Cost;
                if (nd < dist[v])
                {
                    dist[v] = nd;
                    heap.Push(nd, v);
                }
            }
        }
        return dist[target.B];
    }

    /// <summary>Two passes of a ¼ ½ ¼ filter; the end points stay on the Prospect spots.</summary>
    private static List<WorldPoint> Smooth(List<WorldPoint> points)
    {
        for (var pass = 0; pass < 2; pass++)
        {
            var next = new List<WorldPoint>(points);
            for (var i = 1; i < points.Count - 1; i++)
                next[i] = new WorldPoint(
                    0.25 * points[i - 1].X + 0.5 * points[i].X + 0.25 * points[i + 1].X,
                    0.25 * points[i - 1].Y + 0.5 * points[i].Y + 0.25 * points[i + 1].Y);
            points = next;
        }
        return points;
    }

    /// <summary>
    /// One water link can mix river and sea stretches: split its cell path wherever the steps
    /// change between sea-to-sea and anything else.
    /// </summary>
    private static List<TravelStretch> WaterRoute(
        World world, CostGrid[] grids, List<int> path, byte[] kind, int[] linkNode, sbyte[] linkGrid, int w,
        Func<int, WorldPoint> centre, WorldPoint fromSpot, WorldPoint toSpot, out TravelLinkKind linkKind)
    {
        var stretches = new List<TravelStretch>();
        double riverLength = 0, seaLength = 0;

        bool SeaStep(int a, int b) => kind[a] == CostGrid.Sea && kind[b] == CostGrid.Sea;

        var start = 0;
        while (start < path.Count - 1)
        {
            var sea = SeaStep(path[start], path[start + 1]);
            var end = start + 1;
            while (end < path.Count - 1 && SeaStep(path[end], path[end + 1]) == sea) end++;

            List<WorldPoint> points;
            if (sea)
            {
                points = PullStraight(world, path.GetRange(start, end - start + 1).Select(centre).ToList());
                seaLength += Length(points);
            }
            else
            {
                points = RiverStretch(world, grids, path, start, end, linkNode, linkGrid, w)
                         ?? path.GetRange(start, end - start + 1).Select(centre).ToList();
                riverLength += Length(points);
            }
            stretches.Add(new TravelStretch(sea ? TravelLinkKind.SeaLane : TravelLinkKind.RiverRoute, points));
            start = end;
        }

        if (stretches.Count == 0)
        {
            // Both Prospects seed the same cell: a direct hop.
            stretches.Add(new TravelStretch(TravelLinkKind.SeaLane, path.Select(centre).ToList()));
        }

        // The route runs from one Prospect spot to the other.
        var first = stretches[0];
        stretches[0] = first with { Points = first.Points.Prepend(fromSpot).ToList() };
        var last = stretches[^1];
        stretches[^1] = last with { Points = last.Points.Append(toSpot).ToList() };

        // The link is called a river route or a sea lane after its longer part.
        linkKind = riverLength >= seaLength && riverLength > 0 ? TravelLinkKind.RiverRoute : TravelLinkKind.SeaLane;
        return stretches;
    }

    /// <summary>The river's own nodes between the two ends of a river stretch; null if the ends are not on the same river tree.</summary>
    private static List<WorldPoint>? RiverStretch(
        World world, CostGrid[] grids, List<int> path, int start, int end, int[] linkNode, sbyte[] linkGrid, int w)
    {
        (int Grid, int Node)? StepNode(int a, int b)
        {
            var m = MoveIndex(b % w - a % w, b / w - a / w);
            if (m < 0 || linkNode[a * 8 + m] < 0) return null;
            return (linkGrid[a * 8 + m], linkNode[a * 8 + m]);
        }

        if (StepNode(path[start], path[start + 1]) is not { } first) return null;
        if (StepNode(path[end - 1], path[end]) is not { } last) return null;
        if (first.Grid != last.Grid) return null;

        var nodes = world.DetailOf(new RegionId(grids[first.Grid].Region)).Rivers.Nodes;
        if (nodes[first.Node].Tree != nodes[last.Node].Tree) return null;

        // Down from the first node to the point where the two branches meet, then up to the last node.
        var down = new List<int>();
        var position = new Dictionary<int, int>();
        for (var k = first.Node; k >= 0; k = nodes[k].Parent)
        {
            position[k] = down.Count;
            down.Add(k);
        }
        var up = new List<int>();
        var meet = -1;
        for (var k = last.Node; k >= 0; k = nodes[k].Parent)
        {
            if (position.TryGetValue(k, out meet)) break;
            up.Add(k);
            meet = -1;
        }
        if (meet < 0) return null;

        var points = new List<WorldPoint>();
        for (var i = 0; i <= meet; i++) points.Add(nodes[down[i]].Position);
        for (var i = up.Count - 1; i >= 0; i--) points.Add(nodes[up[i]].Position);
        return points;
    }

    /// <summary>The cell path, pulled straight wherever the line of sight is open water (sampled every 1 km).</summary>
    private static List<WorldPoint> PullStraight(World world, List<WorldPoint> points)
    {
        if (points.Count <= 2) return points;
        var result = new List<WorldPoint> { points[0] };
        var i = 0;
        while (i < points.Count - 1)
        {
            var next = i + 1;
            for (var j = points.Count - 1; j > i + 1; j--)
                if (OpenWater(world, points[i], points[j]))
                {
                    next = j;
                    break;
                }
            result.Add(points[next]);
            i = next;
        }
        return result;
    }

    private static bool OpenWater(World world, WorldPoint a, WorldPoint b)
    {
        var length = a.DistanceTo(b);
        var steps = Math.Max(1, (int)Math.Ceiling(length / 1000));
        for (var s = 0; s <= steps; s++)
        {
            var f = (double)s / steps;
            if (!world.Cells.CellAt(a.X + (b.X - a.X) * f, a.Y + (b.Y - a.Y) * f).Ocean) return false;
        }
        return true;
    }

    private static double Length(List<WorldPoint> points)
    {
        var sum = 0.0;
        for (var i = 1; i < points.Count; i++) sum += points[i - 1].DistanceTo(points[i]);
        return sum;
    }

    private static int MoveIndex(int dx, int dy)
    {
        for (var m = 0; m < 8; m++)
            if (Moves8[m].Dx == dx && Moves8[m].Dy == dy) return m;
        return -1;
    }

    private static double CentreDistance(WorldPoint p, int i, int j)
    {
        var dx = (i + 0.5) * Cell - p.X;
        var dy = (j + 0.5) * Cell - p.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }

    private static double[] Filled(int n)
    {
        var a = new double[n];
        Array.Fill(a, double.PositiveInfinity);
        return a;
    }

    private static int[] FilledInt(int n)
    {
        var a = new int[n];
        Array.Fill(a, -1);
        return a;
    }
}
