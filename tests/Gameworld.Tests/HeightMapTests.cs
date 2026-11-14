using System.Diagnostics;
using Gameworld.Domain;
using Gameworld.Generation;
using Gameworld.Generation.Heights;
using Gameworld.Generation.Stages;
using Xunit.Abstractions;

namespace Gameworld.Tests;

/// <summary>The height maps of a 2 × 2 block of regions of the default world, built once for all tests.</summary>
public sealed class HeightMapFixture
{
    public static readonly GridCoord[] Block = [new(0, 0), new(1, 0), new(0, 1), new(1, 1)];

    public HeightMapFixture()
    {
        World = World.Create("cellgen");
        foreach (var region in Block)
        {
            var sw = Stopwatch.StartNew();
            Results[region] = HeightMapBuilder.Build(World, region);
            FirstBuildMs[region] = sw.Elapsed.TotalMilliseconds;
        }
        // Again, now that the rivers around every region are cached: the cost of the height map alone.
        foreach (var region in Block)
        {
            var sw = Stopwatch.StartNew();
            HeightMapBuilder.Build(World, region);
            BuildMs[region] = sw.Elapsed.TotalMilliseconds;
        }
    }

    public World World { get; }
    public Dictionary<GridCoord, HeightMapResult> Results { get; } = new();
    public Dictionary<GridCoord, double> FirstBuildMs { get; } = new();
    public Dictionary<GridCoord, double> BuildMs { get; } = new();

    public DetailWork Detail(GridCoord region) => World.DetailOf(new RegionId(region));
}

/// <summary>The height map of map-generation.md section 12, on real regions of the default world.</summary>
public class HeightMapTests(HeightMapFixture fixture, ITestOutputHelper output) : IClassFixture<HeightMapFixture>
{
    private World World => fixture.World;

    [Fact]
    public void The_raster_is_world_aligned_and_covers_the_region_with_its_halo()
    {
        foreach (var region in HeightMapFixture.Block)
        {
            var result = fixture.Results[region];
            var map = result.Map;
            Assert.Equal(World.Parameters.Elevation.HeightMap.ResolutionM, map.ResolutionM);
            Assert.Equal(map.Width * map.Height, map.Heights.Length);

            // No land of the region on the outer 24 km of the raster: that is the halo.
            var halo = (int)(24_000 / map.ResolutionM);
            for (var j = 0; j < map.Height; j++)
                for (var i = 0; i < map.Width; i++)
                {
                    if (i >= halo && j >= halo && i < map.Width - halo && j < map.Height - halo) continue;
                    var info = Classify(map, i, j);
                    Assert.False(!info.Ocean && info.Region == region, $"land of {region} in the halo at ({i}, {j})");
                }
            output.WriteLine(
                $"{region}: {map.Width} x {map.Height} cells, first build {fixture.FirstBuildMs[region]:F0} ms " +
                $"(with the rivers of the regions around), again {fixture.BuildMs[region]:F0} ms");
        }
    }

    [Fact]
    public void The_height_map_does_not_depend_on_the_order_regions_are_generated_in()
    {
        var region = new GridCoord(0, 0);
        var expected = fixture.Results[region];

        // A fresh world that builds two other regions first.
        var other = World.Create("cellgen");
        HeightMapBuilder.Build(other, new GridCoord(1, 1));
        HeightMapBuilder.Build(other, new GridCoord(-1, 0));
        var actual = HeightMapBuilder.Build(other, region);

        Assert.Equal(expected.Map.OriginI, actual.Map.OriginI);
        Assert.Equal(expected.Map.OriginJ, actual.Map.OriginJ);
        Assert.Equal(expected.Map.Width, actual.Map.Width);
        Assert.Equal(expected.Map.Height, actual.Map.Height);
        Assert.True(expected.Map.Heights.AsSpan().SequenceEqual(actual.Map.Heights), "the heights differ");
        Assert.True(expected.BedHeights.AsSpan().SequenceEqual(actual.BedHeights), "the bed heights differ");
        Assert.Equal(expected.FilledCells, actual.FilledCells);
        Assert.Equal(expected.MaxRaiseM, actual.MaxRaiseM);
        Assert.Equal(expected.MeanHeightByPatch.OrderBy(p => p.Key), actual.MeanHeightByPatch.OrderBy(p => p.Key));
    }

    [Fact]
    public void Ocean_cells_are_below_sea_level_and_land_is_above_the_river_trench()
    {
        var hp = World.Parameters.Elevation.HeightMap;
        foreach (var region in HeightMapFixture.Block)
        {
            var map = fixture.Results[region].Map;
            int land = 0, landBelowZero = 0;
            var deepest = 0f;
            var highest = float.NegativeInfinity;
            for (var j = 0; j < map.Height; j++)
                for (var i = 0; i < map.Width; i++)
                {
                    var h = map.Heights[j * map.Width + i];
                    Assert.True(float.IsFinite(h));
                    if (Classify(map, i, j).Ocean)
                    {
                        Assert.InRange(h, -(2 + hp.SeaDepthM) - 0.001, -2);
                        deepest = Math.Min(deepest, h);
                    }
                    else
                    {
                        // A river at sea level is cut the channel depth below it; nothing on land is lower.
                        Assert.True(h >= -hp.ChannelDepthM - 0.001, $"land at {h} m");
                        land++;
                        if (h < 0) landBelowZero++;
                        highest = Math.Max(highest, h);
                    }
                }
            Assert.True(landBelowZero < 0.05 * land, $"{landBelowZero} of {land} land cells are below 0");
            output.WriteLine(
                $"{region}: highest {highest:F0} m, deepest {deepest:F0} m, {land} land cells, {landBelowZero} of them below 0 " +
                $"({100.0 * landBelowZero / Math.Max(1, land):F1} %: river trenches at sea level)");
        }
    }

    [Fact]
    public void River_beds_never_fall_going_upstream()
    {
        foreach (var region in HeightMapFixture.Block)
        {
            var nodes = fixture.Detail(region).Rivers.Nodes;
            var beds = fixture.Results[region].BedHeights;
            Assert.Equal(nodes.Count, beds.Length);
            var atSeaLevelOnLand = 0;
            for (var k = 0; k < nodes.Count; k++)
            {
                var node = nodes[k];
                if (node.IsRoot || node.Wet)
                {
                    Assert.Equal(0, beds[k]);
                    continue;
                }
                Assert.True(beds[k] >= beds[node.Parent], $"node {k} at {beds[k]} lies below its parent at {beds[node.Parent]}");
                if (beds[k] == 0) atSeaLevelOnLand++;
            }
            output.WriteLine($"{region}: {nodes.Count} nodes, highest bed {beds.DefaultIfEmpty().Max():F0} m, {atSeaLevelOnLand} land nodes at 0 m");
        }
    }

    [Fact]
    public void Simple_beds_rise_at_every_land_node_and_levelling_only_lowers_them()
    {
        foreach (var region in HeightMapFixture.Block)
        {
            var detail = fixture.Detail(region);
            var nodes = detail.Rivers.Nodes;
            var plain = HeightRules.SimpleBeds(World, detail, levelling: false);
            var levelled = HeightRules.SimpleBeds(World, detail, levelling: true);
            for (var k = 0; k < nodes.Count; k++)
            {
                var node = nodes[k];
                if (node.IsRoot || node.Wet)
                {
                    Assert.Equal(0, plain[k]);
                    Assert.Equal(0, levelled[k]);
                    continue;
                }
                Assert.True(plain[k] > plain[node.Parent]);
                Assert.True(levelled[k] >= levelled[node.Parent]);
                Assert.True(levelled[k] <= plain[k] + 1e-9);
            }
            output.WriteLine(
                $"{region}: highest bed plain {plain.DefaultIfEmpty().Max():F0} m, levelled {levelled.DefaultIfEmpty().Max():F0} m, " +
                $"final {fixture.Results[region].BedHeights.DefaultIfEmpty().Max():F0} m");
        }
    }

    [Fact]
    public void The_cell_under_a_river_node_is_cut_below_the_bed()
    {
        var hp = World.Parameters.Elevation.HeightMap;
        foreach (var region in HeightMapFixture.Block)
        {
            var result = fixture.Results[region];
            var map = result.Map;
            var detail = fixture.Detail(region);
            var nodes = detail.Rivers.Nodes;
            var checkedNodes = 0;
            for (var k = 0; k < nodes.Count; k++)
            {
                var node = nodes[k];
                if (node.IsRoot || node.Wet) continue;
                var i = (int)Math.Floor(node.Position.X / map.ResolutionM);
                var j = (int)Math.Floor(node.Position.Y / map.ResolutionM);
                Assert.True(map.Contains(i, j));
                // The river shapes land of its own region only (a landlocked region's rivers shape any land).
                var info = Classify(map, i - map.OriginI, j - map.OriginJ);
                if (info.Ocean || (info.Region != region && !detail.Rivers.Rivers.CrossesNeighbours)) continue;
                checkedNodes++;
                var limit = result.BedHeights[k] - hp.ChannelDepthM + result.MaxRaiseM + 0.02;
                Assert.True(map.At(i, j) <= limit, $"node {k}: the cell is at {map.At(i, j)} m, the bed at {result.BedHeights[k]} m");
            }
            Assert.True(checkedNodes > nodes.Count / 2);
        }
    }

    [Fact]
    public void Every_land_cell_of_the_region_drains_to_a_lower_cell_or_is_a_pit_fill_seed()
    {
        foreach (var region in HeightMapFixture.Block)
        {
            var result = fixture.Results[region];
            var map = result.Map;
            var infos = new Gameworld.Generation.Cells.CellInfo[map.Width * map.Height];
            for (var j = 0; j < map.Height; j++)
                for (var i = 0; i < map.Width; i++)
                    infos[j * map.Width + i] = Classify(map, i, j);

            int land = 0, seeds = 0;
            for (var j = 0; j < map.Height; j++)
                for (var i = 0; i < map.Width; i++)
                {
                    var c = j * map.Width + i;
                    if (infos[c].Ocean || infos[c].Region != region) continue;
                    land++;
                    // The region's land never reaches the raster edge, so every neighbour exists.
                    Assert.True(i > 0 && j > 0 && i < map.Width - 1 && j < map.Height - 1);

                    // A seed: it touches (4 neighbours) another region's land and keeps its height.
                    var seed = false;
                    foreach (var n in new[] { c - 1, c + 1, c - map.Width, c + map.Width })
                        if (!infos[n].Ocean && infos[n].Region != region) seed = true;
                    if (seed)
                    {
                        seeds++;
                        continue;
                    }

                    var drains = false;
                    for (var dj = -1; dj <= 1; dj++)
                        for (var di = -1; di <= 1; di++)
                        {
                            var n = c + dj * map.Width + di;
                            if (n == c || (!infos[n].Ocean && infos[n].Region != region)) continue;
                            if (map.Heights[n] < map.Heights[c]) drains = true;
                        }
                    Assert.True(drains, $"{region}: the land cell ({i}, {j}) at {map.Heights[c]} m is a pit");
                }
            Assert.True(land > 0);
            output.WriteLine(
                $"{region}: {land} land cells, {seeds} border seeds, the pit fill raised {result.FilledCells} cells by at most {result.MaxRaiseM:F2} m");
        }
    }

    [Fact]
    public void Neighbouring_rasters_agree_where_they_overlap_away_from_the_outer_edge()
    {
        var block = HeightMapFixture.Block;
        for (var a = 0; a < block.Length; a++)
            for (var b = a + 1; b < block.Length; b++)
            {
                var ma = fixture.Results[block[a]].Map;
                var mb = fixture.Results[block[b]].Map;
                var line = $"{block[a]} vs {block[b]}:";
                foreach (var marginKm in new[] { 0, 8, 16, 24, 40, 48 })
                {
                    var o = Overlap(ma, mb, marginKm);
                    line += $"  [{marginKm} km: {o.Cells} cells, {o.Differing} differ, max {o.MaxDiff:F2} m" +
                            (o.MaxDiff > 0 ? $" ({o.Where})]" : "]");

                    // Inside the halo almost every cell is bit-identical. What still differs there is
                    // (a) land of river-poor flat zones, whose nearest river or coast can lie farther
                    // away than the 24 km halo at a hillslope of 0.006, and (b) river trenches at sea
                    // level in a region the raster edge cuts through: its edge cells are pit-fill seeds.
                    if (marginKm >= 24 && o.Cells > 0)
                        Assert.True(o.Differing <= 0.03 * o.Cells, $"{line}: more than 3 % of the cells differ");
                    if (marginKm >= 24)
                        Assert.True(o.MaxDiffOutsideRiverPoor < 2,
                            $"{line}: a difference of {o.MaxDiffOutsideRiverPoor} m outside the river-poor flat zones");
                }
                output.WriteLine(line);
            }
    }

    [Fact]
    public void Patch_heights_are_in_the_neighbourhood_of_the_measured_medians()
    {
        var byType = new Dictionary<ElevationType, List<double>>();
        var highestCell = 0f;
        foreach (var region in HeightMapFixture.Block)
        {
            var result = fixture.Results[region];
            highestCell = Math.Max(highestCell, result.Map.Heights.Max());
            var landPatches = 0;
            foreach (var patch in fixture.Detail(region).Patches)
            {
                var hasMean = result.MeanHeightByPatch.TryGetValue(patch.Root.Key, out var mean);
                if (patch.Shape.Ocean)
                {
                    Assert.False(hasMean, "an ocean patch has no mean height");
                    continue;
                }
                landPatches++;
                if (!hasMean) continue;   // no land cell centre in it
                if (!byType.TryGetValue(patch.ZoneType, out var list)) byType[patch.ZoneType] = list = new List<double>();
                list.Add(mean);
            }
            // Every mean belongs to a land patch of the region.
            Assert.True(result.MeanHeightByPatch.Count <= landPatches);
            Assert.True(result.MeanHeightByPatch.Count > 0.9 * landPatches);
        }

        var medians = new Dictionary<ElevationType, double>();
        foreach (var (type, list) in byType.OrderBy(p => p.Key))
        {
            list.Sort();
            medians[type] = list[list.Count / 2];
            output.WriteLine($"{type}: {list.Count} patches, median {medians[type]:F0} m, highest {list[^1]:F0} m");
        }
        output.WriteLine($"highest cell {highestCell:F0} m");

        // map-generation.md 12.8 measured 85 / 160 / 260 / 410 m on one tile; these are sanity bands.
        Assert.InRange(medians[ElevationType.FlatLand], 30, 200);
        Assert.InRange(medians[ElevationType.Lowland], 70, 350);
        Assert.InRange(medians[ElevationType.Hills], 120, 600);
        Assert.InRange(medians[ElevationType.Mountains], 200, 1000);
        Assert.True(medians[ElevationType.FlatLand] < medians[ElevationType.Lowland]);
        Assert.True(medians[ElevationType.Lowland] < medians[ElevationType.Mountains]);
        Assert.InRange(highestCell, 1000, 6000);
    }

    [Fact]
    public void The_relief_field_stays_in_its_range()
    {
        double min = double.PositiveInfinity, max = double.NegativeInfinity;
        for (var j = -20; j <= 20; j++)
            for (var i = -20; i <= 20; i++)
            {
                var r = HeightRules.Relief(World, i * 37_000.0, j * 41_000.0);
                Assert.Equal(r, HeightRules.Relief(World, i * 37_000.0, j * 41_000.0));
                min = Math.Min(min, r);
                max = Math.Max(max, r);
            }
        Assert.InRange(min, 0.25, 1);
        Assert.InRange(max, 1, 1.9);
        output.WriteLine($"relief between {min:F2} and {max:F2}");
    }

    [Theory]
    [InlineData(7.0 / 16)]
    [InlineData(0.5)]
    [InlineData(1.0)]
    [InlineData(1.25)]
    [InlineData(0)]
    [InlineData(0.3)]      // not a multiple of 1/16: the deterministic exp and ln
    public void The_flow_factor_is_flow_to_the_power_minus_theta(double theta)
    {
        foreach (var flow in new[] { 1, 2, 3, 10, 100, 1234, 30_000 })
        {
            var expected = Math.Pow(flow, -theta);
            Assert.InRange(HeightRules.FlowFactor(flow, theta), expected * (1 - 1e-12), expected * (1 + 1e-12));
        }
        if (theta > 0) Assert.Equal(double.PositiveInfinity, HeightRules.FlowFactor(0, theta));
    }

    [Fact]
    public void The_floodplain_half_width_follows_the_table()
    {
        // At a 100 m raster the lower limit of 0.75 cell is out of the way of the table.
        var fine = new Floodplain(new HeightMapParameters { ResolutionM = 100 });
        Assert.Equal(400, fine.HalfWidth(0, 100), 9);
        Assert.Equal(200, fine.HalfWidth(100, 100), 9);
        Assert.Equal(90, fine.HalfWidth(400, 100), 9);
        Assert.Equal(75, fine.HalfWidth(1000, 100), 9);          // 30 m, raised to 0.75 cell
        Assert.Equal(75, fine.HalfWidth(5000, 100), 9);          // constant beyond the last point
        Assert.Equal(800, fine.HalfWidth(0, 400), 9);            // × √(flow / 100)
        Assert.InRange(fine.HalfWidth(50, 100), 200, 400);

        var unscaled = new Floodplain(new HeightMapParameters
        {
            ResolutionM = 100,
            Floodplain = new FloodplainParameters { ScaleWithRiverSize = false },
        });
        Assert.Equal(400, unscaled.HalfWidth(0, 400), 9);

        var standard = HeightRules.FloodplainOf(World);
        Assert.Equal(750, standard.HalfWidth(0, 100), 9);
        Assert.Equal(750, standard.MinHalfWidthM, 9);
    }

    private Gameworld.Generation.Cells.CellInfo Classify(HeightMap map, int i, int j) =>
        World.Cells.CellAt((map.OriginI + i + 0.5) * map.ResolutionM, (map.OriginJ + j + 0.5) * map.ResolutionM);

    private sealed record OverlapStats(int Cells, int Differing, double MaxDiff, double MaxDiffOutsideRiverPoor, string Where);

    /// <summary>The cells two rasters share, at least <paramref name="marginKm"/> inside the edges of both.</summary>
    private OverlapStats Overlap(HeightMap a, HeightMap b, int marginKm)
    {
        var m = (int)(marginKm * 1000 / a.ResolutionM);
        var i0 = Math.Max(a.OriginI, b.OriginI) + m;
        var j0 = Math.Max(a.OriginJ, b.OriginJ) + m;
        var i1 = Math.Min(a.OriginI + a.Width, b.OriginI + b.Width) - m;
        var j1 = Math.Min(a.OriginJ + a.Height, b.OriginJ + b.Height) - m;
        int cells = 0, differing = 0;
        double max = 0, maxOutside = 0;
        var where = "";
        for (var j = j0; j < j1; j++)
            for (var i = i0; i < i1; i++)
            {
                cells++;
                var d = Math.Abs(a.At(i, j) - b.At(i, j));
                if (d == 0) continue;
                differing++;
                var info = World.Cells.CellAt((i + 0.5) * a.ResolutionM, (j + 0.5) * a.ResolutionM);
                var riverPoor = !info.Ocean && World.IsRiverPoorZone(info.Zone);
                if (!riverPoor && d > maxOutside) maxOutside = d;
                if (d > max)
                {
                    max = d;
                    where = info.Ocean
                        ? $"ocean, {a.At(i, j):F1} / {b.At(i, j):F1} m"
                        : $"{World.Cells.ZoneType(info.Zone)}{(riverPoor ? ", river-poor" : "")} of {info.Region}, {a.At(i, j):F1} / {b.At(i, j):F1} m";
                }
            }
        return new OverlapStats(cells, differing, max, maxOutside, where);
    }
}
