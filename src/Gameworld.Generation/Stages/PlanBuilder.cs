using Gameworld.Domain;
using Gameworld.Generation.Basics;
using Gameworld.Generation.Resources;

namespace Gameworld.Generation.Stages;

/// <summary>A land patch scored as a place to settle.</summary>
public sealed class PatchScore
{
    public required PatchDetail Patch { get; init; }
    public required ProspectFacts Facts { get; init; }
    public required ProspectFactors Factors { get; init; }
    public required WorldPoint Spot { get; init; }
    public required double Score { get; init; }
    /// <summary>The share of ocean patches among the neighbours.</summary>
    public required double OceanFraction { get; init; }
    /// <summary>The patch touches the land of another region.</summary>
    public required bool Border { get; init; }
    public required double Centrality { get; init; }
}

/// <summary>The working data of a Planned region.</summary>
public sealed class PlanWork
{
    public required GridCoord Region { get; init; }
    /// <summary>Every land patch of the region with its score, sorted by key.</summary>
    public required PatchScore[] Scores { get; init; }
    /// <summary>The Prospects: the ones for cities first, then the ones for towns, each in the order taken.</summary>
    public required Prospect[] Prospects { get; init; }

    /// <summary>The sum of the scores of the Prospects: it decides how many of them are on show.</summary>
    public double ScoreSum => Prospects.Sum(p => p.Score);

    /// <summary>The potential population of the region, per race that is on.</summary>
    public required PotentialPopulation Population { get; init; }

    public RegionPlan ToDomain() => new(Prospects, Population);
}

/// <summary>
/// Prospects for cities and towns (map-generation.md section 15) and the potential population
/// (section 19). All inputs use the estimated biomes and resources, so neither depends on the
/// height map.
/// </summary>
internal static class PlanBuilder
{
    private const double NavigablePatches = 8;

    public static PlanWork Build(World world, DetailWork detail)
    {
        var cells = world.Cells;
        var p = world.Parameters;
        var seed = world.Foundation.Seed.Value;
        var region = detail.Region;
        var perPatch = (double)Math.Max(1, p.Detail.Rivers.AttractorsPerPatchCell);
        var nodes = detail.Rivers.Nodes;

        var land = detail.Patches.Where(x => !x.Shape.Ocean).ToArray();
        var index = new Dictionary<long, int>();
        for (var i = 0; i < land.Length; i++) index[land[i].Root.Key] = i;

        // 15.1 Neighbours.
        var landNeighbours = new int[land.Length][];
        var oceanNeighbours = new int[land.Length];
        var otherLand = new bool[land.Length];
        for (var i = 0; i < land.Length; i++)
        {
            var own = new List<int>();
            foreach (var nb in land[i].Shape.Neighbours)
            {
                var info = cells.InfoOf(nb);
                if (info.Ocean) oceanNeighbours[i]++;
                else if (info.Region == region && index.TryGetValue(nb.Key, out var k)) own.Add(k);
                else otherLand[i] = true;
            }
            landNeighbours[i] = own.ToArray();
        }

        // Depth: patch steps to the nearest border patch, breadth-first over own land.
        var depth = new int[land.Length];
        Array.Fill(depth, 99);
        var queue = new Queue<int>();
        for (var i = 0; i < land.Length; i++)
            if (otherLand[i])
            {
                depth[i] = 0;
                queue.Enqueue(i);
            }
        while (queue.Count > 0)
        {
            var i = queue.Dequeue();
            foreach (var k in landNeighbours[i])
                if (depth[k] == 99)
                {
                    depth[k] = depth[i] + 1;
                    queue.Enqueue(k);
                }
        }

        // River facts. Only children on the region's own land count.
        var children = new List<int>[nodes.Count];
        for (var k = 0; k < nodes.Count; k++)
        {
            if (!detail.Counts(k)) continue;
            var parent = nodes[k].Parent;
            (children[parent] ??= new List<int>()).Add(k);
        }
        var navigableFlow = NavigablePatches * perPatch;
        var confluence = new double[land.Length];
        var head = new bool[land.Length];
        for (var i = 0; i < land.Length; i++)
        {
            foreach (var k in land[i].RiverNodes)
            {
                int first = 0, second = 0;
                var anyNavigableChild = false;
                if (children[k] != null)
                    foreach (var c in children[k])
                    {
                        var f = nodes[c].Flow;
                        if (f > first) { second = first; first = f; }
                        else if (f > second) second = f;
                        if (f >= navigableFlow) anyNavigableChild = true;
                    }
                confluence[i] = Math.Max(confluence[i], second / perPatch);
                if (nodes[k].Flow >= navigableFlow && !anyNavigableChild) head[i] = true;
            }
        }

        var centrality = Centrality(world, detail, land, index, landNeighbours);

        // 15.3 and 15.4: the five factors and the score.
        var weights = p.Planning.Weights;
        var scores = new PatchScore[land.Length];
        for (var i = 0; i < land.Length; i++)
        {
            var patch = land[i];
            var biome = patch.Biome!;
            var water = patch.Water;
            var res = patch.Resources;
            var coast = oceanNeighbours[i] > 0;
            var neighbourCount = patch.Shape.Neighbours.Length;
            var oceanFrac = neighbourCount > 0 ? (double)oceanNeighbours[i] / neighbourCount : 0;
            var lake = water.HasLake;
            var spot = Spot(world, detail, patch);

            // resources
            var foodSum = 2 * res.Food;
            double bestTimberOrStone = 0, bestFuel = 0, bestWealth = 0;
            foreach (var k in landNeighbours[i])
            {
                var nr = land[k].Resources;
                foodSum += nr.Food;
                bestTimberOrStone = Math.Max(bestTimberOrStone, Math.Max(nr.AmountOf(Resource.Timber), nr.AmountOf(Resource.Stone)));
                bestFuel = Math.Max(bestFuel, nr.AmountOf(Resource.Fuel));
                bestWealth = Math.Max(bestWealth, nr.Wealth);
            }
            var food = foodSum / (2 + landNeighbours[i].Length + oceanNeighbours[i]);
            var waterFactor = Max(Clamp(water.River / 2), Ramp(biome.Aridity, 0.5, 1.5), coast ? 0.5 : 0, lake ? 1 : 0);
            var fuelNeed = biome.Belt switch
            {
                ThermalBelt.Polar => 1,
                ThermalBelt.Subpolar => 1,
                ThermalBelt.Boreal => 0.8,
                ThermalBelt.Temperate => 0.5,
                _ => 0.1,
            };
            var materials =
                0.5 * Max(res.AmountOf(Resource.Timber), res.AmountOf(Resource.Stone), res.AmountOf(Resource.Clay), 0.7 * bestTimberOrStone)
                + 0.5 * Max(res.AmountOf(Resource.Fuel), 0.7 * bestFuel, 1 - fuelNeed, 0);
            var goods = Clamp((res.Wealth + 0.5 * bestWealth) / 1.2);
            var resources = 0.45 * food + 0.2 * waterFactor + 0.15 * materials
                          + 0.1 * res.AmountOf(Resource.Fish) + 0.1 * goods;

            // geography
            var buildable = patch.ZoneType switch
            {
                ElevationType.FlatLand => 1,
                ElevationType.Lowland => 0.9,
                ElevationType.Hills => 0.55,
                _ => 0.25,
            };
            if (biome.Biome == Biome.Mangrove) buildable *= 0.5;
            var waterside = Max(
                water.River >= NavigablePatches ? 1 : water.River >= 2 ? 0.7 : water.River > 0 ? 0.4 : 0,
                water.Harbour ? 1 : coast ? 0.7 : 0,
                lake ? 0.8 : 0,
                0);
            var defence = Max(
                patch.ZoneType switch
                {
                    ElevationType.FlatLand => 0,
                    ElevationType.Lowland => 0.1,
                    ElevationType.Hills => 0.6,
                    _ => 0.7,
                },
                oceanFrac >= 0.5 ? 0.5 : 0,
                confluence[i] >= 4 ? 0.4 : 0,
                0);
            var geography = buildable * (0.5 + 0.3 * waterside + 0.2 * defence) * (0.75 + 0.25 * Clamp(depth[i] / 3.0));

            // trade
            double[] items =
            [
                Clamp(water.Mouth / 80),
                water.Harbour ? 0.6 : coast ? 0.24 : 0,
                0.5 * Ramp(water.River, 8, 80),
                head[i] ? 0.6 : 0,
                0.4 * Clamp(confluence[i] / 8),
                lake ? 0.15 : 0,
            ];
            var largest = items.Max();
            var gateway = Clamp((largest + 0.25 * (items.Sum() - largest)) / 1.5);
            var trade = Clamp(Math.Max(gateway, centrality[i]) + 0.25 * Math.Min(gateway, centrality[i]));

            // climate
            var comfort = Trap(biome.TemperatureC, -8, 4, 24, 34);
            var climate = comfort * (0.4 + 0.6 * Ramp(biome.Aridity, 0.2, 1)) * (biome.Aridity > 6 ? 0.85 : 1);

            var prosperity = world.Fields.Sample01(FieldKind.Prosperity, spot);

            // 15.4 Score.
            var need = Clamp(2 * Math.Min(waterFactor, comfort));
            double wr = Math.Max(0, weights.Resources), wg = Math.Max(0, weights.Geography), wt = Math.Max(0, weights.Trade),
                   wc = Math.Max(0, weights.Climate), wp = Math.Max(0, weights.Prosperity);
            var wSum = wr + wg + wt + wc + wp;
            var mean = wSum > 0
                ? (wr * resources + wg * geography + wt * trade + wc * climate + wp * prosperity) / wSum
                : (resources + geography + trade + climate + prosperity) / 5;

            scores[i] = new PatchScore
            {
                Patch = patch,
                Facts = new ProspectFacts(coast, water.Harbour, water.River, water.Mouth, confluence[i], head[i], water.LakeAreaM2, depth[i]),
                Factors = new ProspectFactors(resources, geography, trade, climate, prosperity),
                Spot = spot,
                Score = 100 * need * mean,
                OceanFraction = oceanFrac,
                Border = otherLand[i],
                Centrality = centrality[i],
            };
        }

        // 19.3 Potential population: every land patch of the region, in key order.
        var patchKm = cells.PatchSize / 1000;
        var population = world.PopulationRules.Region(land.Select(x =>
            (x.Biome!.Biome, x.Shape.Cells.Length * patchKm * patchKm, x.Resources, x.Prosperity)));

        return new PlanWork
        {
            Region = region,
            Scores = scores,
            Prospects = Select(world, scores),
            Population = population,
        };
    }

    /// <summary>15.1 Spot: where in the patch the Prospect is placed.</summary>
    private static WorldPoint Spot(World world, DetailWork detail, PatchDetail patch)
    {
        var nodes = detail.Rivers.Nodes;
        var best = -1;
        var bestMouth = false;
        foreach (var k in patch.RiverNodes)
        {
            var n = nodes[k];
            if (detail.InLake(n.Position.X, n.Position.Y)) continue;
            var parent = nodes[n.Parent];
            var mouth = parent.IsRoot || parent.Wet;
            // A mouth node beats any other node; within the same kind the larger flow wins, then the lower index.
            if (best < 0 || (mouth && !bestMouth) || (mouth == bestMouth && n.Flow > nodes[best].Flow))
            {
                best = k;
                bestMouth = mouth;
            }
        }
        if (best >= 0 && nodes[best].Flow >= 3) return nodes[best].Position;

        var cells = world.Cells;
        bool Fits(double x, double y) => cells.CellAt(x, y).Patch == patch.Root && !detail.InLake(x, y);

        var site = patch.Shape.Site;
        if (Fits(site.X, site.Y)) return site;

        var members = patch.Shape.Cells.Select(c => cells.Patches.Site(c)).ToArray();
        foreach (var m in members)
            if (Fits(m.X, m.Y)) return m;

        // E, S, W, N, SE, SW, NW, NE (y runs south).
        (double X, double Y)[] directions =
            [(1, 0), (0, 1), (-1, 0), (0, -1), (0.7, 0.7), (-0.7, 0.7), (-0.7, -0.7), (0.7, -0.7)];
        for (var ring = 1; ring <= 4; ring++)
        {
            var r = ring * cells.PatchSize / 4;
            foreach (var m in members)
                foreach (var (dx, dy) in directions)
                {
                    var x = m.X + dx * r;
                    var y = m.Y + dy * r;
                    if (Fits(x, y)) return new WorldPoint(x, y);
                }
        }
        return site;
    }

    /// <summary>15.2 Central places: how much of the region's food lies within reach of each patch.</summary>
    private static double[] Centrality(
        World world, DetailWork detail, PatchDetail[] land, Dictionary<long, int> index, int[][] landNeighbours)
    {
        var n = land.Length;
        var nodes = detail.Rivers.Nodes;
        var perPatch = (double)Math.Max(1, world.Parameters.Detail.Rivers.AttractorsPerPatchCell);
        var patchKm = world.Cells.PatchSize / 1000;

        static double Rough(ElevationType t) => t switch
        {
            ElevationType.Hills => 1.3,
            ElevationType.Mountains => 2,
            _ => 1,
        };
        double Km(int a, int b) => land[a].Shape.Site.DistanceTo(land[b].Shape.Site) / 1000;

        var edges = new List<(int To, double Cost)>[n];
        for (var i = 0; i < n; i++) edges[i] = new List<(int, double)>();
        for (var i = 0; i < n; i++)
        {
            var vi = BiomeFactors.Of(land[i].Biome!.Biome).Veg;
            foreach (var k in landNeighbours[i])
            {
                var vk = BiomeFactors.Of(land[k].Biome!.Biome).Veg;
                edges[i].Add((k, Km(i, k) * 0.5 * (Rough(land[i].ZoneType) + Rough(land[k].ZoneType)) * 0.5 * (vi + vk)));
            }
        }

        // River edges between consecutive patches along a navigable river, on top of the land edges.
        var riverEdges = new HashSet<(int, int)>();
        for (var k = 0; k < nodes.Count; k++)
        {
            if (!detail.Counts(k) || nodes[k].Flow < NavigablePatches * perPatch) continue;
            var parent = nodes[k].Parent;
            if (!detail.Counts(parent)) continue;
            if (!index.TryGetValue(detail.NodeInfo[k].Patch.Key, out var a)) continue;
            if (!index.TryGetValue(detail.NodeInfo[parent].Patch.Key, out var b)) continue;
            if (a == b || !riverEdges.Add((Math.Min(a, b), Math.Max(a, b)))) continue;
            var cost = Km(a, b) * 0.4;
            edges[a].Add((b, cost));
            edges[b].Add((a, cost));
        }

        // ideal: the same sum for the centre of a disc of full farmland with the area of one region.
        var r = world.Cells.RegionSize / 1000 / Math.Sqrt(Math.PI) / 35;
        var ideal = 2 * Math.PI * 35 * 35 * (DetMath.Ln(1 + r) + 1 / (1 + r) - 1);

        var result = new double[n];
        var dist = new double[n];
        var heap = new MinHeap();
        for (var s = 0; s < n; s++)
        {
            Array.Fill(dist, double.PositiveInfinity);
            dist[s] = 0;
            heap.Clear();
            heap.Push(0, s);
            while (heap.Count > 0)
            {
                var (d, u) = heap.Pop();
                if (d > dist[u]) continue;
                foreach (var (to, cost) in edges[u])
                {
                    var nd = d + cost;
                    if (nd < dist[to])
                    {
                        dist[to] = nd;
                        heap.Push(nd, to);
                    }
                }
            }
            var acc = 0.0;
            for (var u = 0; u < n; u++)
            {
                if (double.IsPositiveInfinity(dist[u])) continue;
                var area = land[u].Shape.Cells.Length * patchKm * patchKm;
                var f = 1 + dist[u] / 35;
                acc += land[u].Resources.Food * area / (f * f);
            }
            result[s] = Clamp(acc / (0.55 * ideal));
        }
        return result;
    }

    /// <summary>15.5 Selection.</summary>
    private static Prospect[] Select(World world, PatchScore[] scores)
    {
        var seed = world.Foundation.Seed.Value;
        var planning = world.Parameters.Planning;
        var total = Math.Max(0, planning.ProspectsPerRegion);
        var cityCount = Math.Min(Math.Max(0, planning.CityProspectsPerRegion), total);
        var patchSize = world.Cells.PatchSize;

        // Eligible candidates, best first; ties by hash, then by patch key.
        var candidates = scores
            .Where(s => s.Score > 0 && s.Facts.Depth >= 1)
            .OrderByDescending(s => s.Score)
            .ThenBy(s => Hashing.Hash4(seed, 0x10c1, s.Patch.Root.I, s.Patch.Root.J))
            .ThenBy(s => s.Patch.Root.Key)
            .ToArray();

        static bool Apart(PatchScore c, List<PatchScore> taken, double distance)
        {
            foreach (var t in taken)
                if (c.Spot.DistanceTo(t.Spot) < distance) return false;
            return true;
        }

        // 1. Prospects for cities: chosen once and kept.
        var spacing = planning.SpacingKm * 1000;
        var cities = new List<PatchScore>();
        foreach (var c in candidates)
        {
            if (cities.Count >= cityCount) break;
            if (c.Facts.Depth >= 2 && Apart(c, cities, 2.5 * spacing)) cities.Add(c);
        }

        // 2. and 3. Prospects for towns, with the spacing relaxed until the region has enough.
        List<PatchScore> taken;
        while (true)
        {
            taken = new List<PatchScore>(cities);
            foreach (var c in candidates)
            {
                if (taken.Count >= total) break;
                if (cities.Contains(c)) continue;
                if (Apart(c, taken, spacing)) taken.Add(c);
            }
            if (taken.Count >= total || spacing <= patchSize) break;
            spacing = Math.Max(patchSize, spacing * 0.85);
        }

        // 15.6 On show: the sum of the scores decides how many, and the best by score are.
        // The candidates are in that order already, ties included.
        var onShow = OnShowCount(taken.Sum(s => s.Score), taken.Count, planning);
        var shown = new HashSet<PatchScore>();
        foreach (var c in candidates)
        {
            if (shown.Count >= onShow) break;
            if (taken.Contains(c)) shown.Add(c);
        }

        return taken.Select((s, i) => new Prospect(
            new PatchId(new PatchCellId(s.Patch.Root)),
            s.Spot,
            i < cities.Count,
            s.Score,
            ClassOf(s.Score, planning),
            shown.Contains(s),
            s.Factors,
            s.Facts)).ToArray();
    }

    /// <summary>
    /// 15.6 How many of a region's Prospects are on show: 1 at the low score sum, all of a full
    /// region at the high one, evenly in between. Never more than the region has.
    /// </summary>
    internal static int OnShowCount(double scoreSum, int prospects, PlanningParameters planning)
    {
        if (prospects <= 0) return 0;
        var all = Math.Max(1, planning.ProspectsPerRegion);
        double low = planning.OneOnShowScoreSum, high = planning.AllOnShowScoreSum;
        var t = high > low ? Clamp((scoreSum - low) / (high - low)) : scoreSum >= high ? 1 : 0;
        return Math.Min(prospects, (int)Math.Floor(1 + (all - 1) * t + 0.5));
    }

    /// <summary>15.6 Rich from the rich score up, Poor below the poor score, Average between.</summary>
    internal static ProspectClass ClassOf(double score, PlanningParameters planning) =>
        score >= planning.RichScore ? ProspectClass.Rich
        : score < planning.PoorScore ? ProspectClass.Poor
        : ProspectClass.Average;

    private static double Clamp(double v) => Math.Clamp(v, 0, 1);

    private static double Ramp(double v, double a, double b) => Clamp((v - a) / (b - a));

    private static double Trap(double v, double a, double b, double c, double d)
    {
        if (v <= a || v >= d) return 0;
        if (v < b) return (v - a) / (b - a);
        if (v <= c) return 1;
        return (d - v) / (d - c);
    }

    private static double Max(double a, double b, double c, double d) => Math.Max(Math.Max(a, b), Math.Max(c, d));
}
