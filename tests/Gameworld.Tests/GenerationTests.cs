using Gameworld.Domain;
using Gameworld.Generation;
using Gameworld.Generation.Basics;
using Gameworld.Generation.Cells;
using Gameworld.Generation.Rendering;
using Gameworld.Generation.Travel;

namespace Gameworld.Tests;

public class BasicsTests
{
    [Fact]
    public void Hash_and_generator_are_stable_and_spread_over_the_unit_interval()
    {
        Assert.Equal(Hashing.Hash4(1, 2, 3, 4), Hashing.Hash4(1, 2, 3, 4));
        Assert.NotEqual(Hashing.Hash4(1, 2, 3, 4), Hashing.Hash4(1, 2, 4, 3));
        Assert.NotEqual(Hashing.Hash4(1, 2, 3, 4), Hashing.Hash4(1, 3, 3, 4));
        Assert.NotEqual(Hashing.Hash4(1, 2, -3, 4), Hashing.Hash4(1, 2, 3, 4));

        var sum = 0.0;
        for (var i = 0; i < 10000; i++)
        {
            var u = Hashing.Unit(42, 0x3a01, i, -i);
            Assert.InRange(u, 0, 1);
            sum += u;
        }
        Assert.InRange(sum / 10000, 0.48, 0.52);

        var a = new Sfc32(99);
        var b = new Sfc32(99);
        for (var i = 0; i < 100; i++) Assert.Equal(a.Next(), b.Next());
    }

    [Fact]
    public void Between_and_pick_follow_the_document()
    {
        Assert.Equal(0, Hashing.Between(0, 2, 0));
        Assert.Equal(2, Hashing.Between(0, 2, 0.999999));
        Assert.Equal(3, Hashing.Between(4, 2, 0.4)); // the bounds may come in either order
        var picked = Hashing.Pick([1, 2, 3, 4, 5], 3, new Sfc32(7));
        Assert.Equal(3, picked.Length);
        Assert.Equal(3, picked.Distinct().Count());
        Assert.Equal(5, Hashing.Pick([1, 2, 3, 4, 5], 9, new Sfc32(7)).Length);
    }

    [Fact]
    public void Deterministic_maths_agrees_with_the_platform_to_many_digits()
    {
        foreach (var x in new[] { -5.0, -0.3, 0, 0.7, 2.5, 9.2 })
        {
            Assert.Equal(Math.Exp(x), DetMath.Exp(x), 12 - (int)Math.Max(0, Math.Log10(Math.Exp(x))));
            Assert.Equal(Math.Sin(x), DetMath.Sin(x), 12);
            Assert.Equal(Math.Cos(x), DetMath.Cos(x), 12);
        }
        foreach (var x in new[] { 0.01, 1.0, 1.5, 100, 12000 })
            Assert.Equal(Math.Log(x), DetMath.Ln(x), 12);
    }

    [Fact]
    public void Noise_stays_in_range_and_the_spline_is_monotone()
    {
        for (var i = 0; i < 2000; i++)
        {
            double x = i * 0.37 - 300, y = i * -0.21 + 77;
            Assert.InRange(Noise.Simplex2(5, x, y), -1.05, 1.05);
            Assert.InRange(Noise.Perlin2(5, x, y), -1.0001, 1.0001);
            Assert.InRange(Noise.Ridged(5, x, y, 4, 2.1, 0.5), -1.0001, 1.0001);
        }

        var spline = new MonotoneSpline([(0, 400.0), (100, 200), (400, 90), (1000, 30)]);
        Assert.Equal(400, spline.Evaluate(-50));
        Assert.Equal(200, spline.Evaluate(100), 9);
        Assert.Equal(30, spline.Evaluate(5000));
        var previous = double.PositiveInfinity;
        for (var h = 0.0; h <= 1000; h += 10)
        {
            var v = spline.Evaluate(h);
            Assert.True(v <= previous + 1e-9);
            previous = v;
        }
    }

    [Fact]
    public void Heap_pops_by_key_then_by_the_smaller_index()
    {
        var heap = new MinHeap();
        heap.Push(2, 5); heap.Push(1, 9); heap.Push(1, 3); heap.Push(0.5, 100); heap.Push(2, 1);
        Assert.Equal((0.5, 100), heap.Pop());
        Assert.Equal((1, 3), heap.Pop());
        Assert.Equal((1, 9), heap.Pop());
        Assert.Equal((2, 1), heap.Pop());
        Assert.Equal((2, 5), heap.Pop());
        Assert.Equal(0, heap.Count);
    }
}

public class CellMapTests
{
    private static readonly World Shared = World.Create("cellgen");

    [Fact]
    public void A_level_finds_the_nearest_site_and_its_neighbours_are_symmetric()
    {
        var level = new VoronoiLevel(123, 1000, 1, 0, 0);
        var rng = new Sfc32(5);
        for (var t = 0; t < 300; t++)
        {
            double x = (rng.Next() - 0.5) * 40000, y = (rng.Next() - 0.5) * 40000;
            var found = level.Query(x, y);
            var best = double.PositiveInfinity;
            var bestCell = default(GridCoord);
            int ci = (int)Math.Floor(x / 1000), cj = (int)Math.Floor(y / 1000);
            for (var j = cj - 4; j <= cj + 4; j++)
                for (var i = ci - 4; i <= ci + 4; i++)
                {
                    var d = Math.Pow(level.SiteX(i, j) - x, 2) + Math.Pow(level.SiteY(i, j) - y, 2);
                    if (d < best) { best = d; bestCell = new GridCoord(i, j); }
                }
            Assert.Equal(bestCell, found);
        }

        for (var j = -3; j <= 3; j++)
            for (var i = -3; i <= 3; i++)
            {
                var c = new GridCoord(i, j);
                var neighbours = level.Neighbours(c);
                Assert.NotEmpty(neighbours);
                foreach (var nb in neighbours) Assert.Contains(c, level.Neighbours(nb));
                // Sorted nearest first.
                var site = level.Site(c);
                for (var k = 1; k < neighbours.Length; k++)
                    Assert.True(site.DistanceTo(level.Site(neighbours[k - 1])) <= site.DistanceTo(level.Site(neighbours[k])));
            }
    }

    [Fact]
    public void Every_zone_cell_and_patch_cell_has_exactly_one_owner_that_lists_it()
    {
        var cells = Shared.Cells;
        for (var j = 0; j < 12; j++)
            for (var i = 0; i < 12; i++)
            {
                var z = new GridCoord(i, j);
                Assert.Contains(z, cells.OwnedZoneCells(cells.RegionOfZoneCell(z)));
            }
        for (var j = 0; j < 30; j++)
            for (var i = 0; i < 30; i++)
            {
                var p = new GridCoord(i, j);
                Assert.Contains(p, cells.OwnedPatchCells(cells.ZoneCellOfPatchCell(p)));
            }
    }

    [Fact]
    public void A_zone_takes_ocean_region_and_type_from_its_root_and_mergers_stay_on_one_side_of_the_coast()
    {
        var cells = Shared.Cells;
        var merged = 0;
        for (var j = -20; j < 20; j++)
            for (var i = -20; i < 20; i++)
            {
                var z = new GridCoord(i, j);
                var root = cells.ZoneRoot(z);
                Assert.Equal(root, cells.ZoneRoot(root));
                Assert.Equal(cells.IsOceanCell(z), cells.IsOceanCell(root));
                if (root != z) merged++;
                if (cells.IsOceanSeed(z)) Assert.True(cells.IsOceanCell(z));
            }
        Assert.InRange(merged, 150, 500); // 2–4 of about 16 zone cells per region merge away
    }

    [Fact]
    public void A_point_is_classified_through_its_patch()
    {
        var cells = Shared.Cells;
        var rng = new Sfc32(11);
        for (var t = 0; t < 500; t++)
        {
            var info = cells.CellAt(rng.Next() * 600000, rng.Next() * 600000);
            Assert.Equal(cells.PatchRoot(info.PatchCell), info.Patch);
            Assert.Equal(cells.ZoneRoot(cells.ZoneCellOfPatchCell(info.Patch)), info.Zone);
            Assert.Equal(cells.RegionOfZoneCell(info.Zone), info.Region);
            Assert.Contains(info.ZoneCell, cells.ZoneCellsDrawnIn(info.Region));
            Assert.NotNull(cells.PatchesOf(info.Region).Find(info.Patch));
        }
    }

    [Fact]
    public void About_a_fifth_of_the_map_is_sea_and_the_elevation_types_follow_their_weights()
    {
        var cells = Shared.Cells;
        int ocean = 0, total = 0;
        var types = new int[5];
        for (double y = 0; y < 2560000; y += 8000)
            for (double x = 0; x < 2560000; x += 8000)
            {
                var info = cells.CellAt(x, y);
                total++;
                if (info.Ocean) ocean++;
                types[(int)cells.ZoneType(info.Zone)]++;
            }
        double land = total - ocean;
        Assert.InRange((double)ocean / total, 0.12, 0.26);
        Assert.InRange(types[(int)ElevationType.FlatLand] / land, 0.30, 0.46);
        Assert.InRange(types[(int)ElevationType.Lowland] / land, 0.38, 0.54);
        Assert.InRange(types[(int)ElevationType.Hills] / land, 0.04, 0.13);
        Assert.InRange(types[(int)ElevationType.Mountains] / land, 0.04, 0.13);
    }

    [Fact]
    public void The_sited_function_needs_the_foundation_only()
    {
        var a = World.Create("cellgen").Sited(new RegionId(4, -2));
        var other = World.Create("cellgen");
        other.Advance(new RegionId(5, -2), RegionStage.Detailed); // generate something else first
        var b = other.Sited(new RegionId(4, -2));

        Assert.Equal(a.ZoneCells.Count, b.ZoneCells.Count);
        Assert.Equal(DomainJson.Serialize(a), DomainJson.Serialize(b));
        Assert.All(a.ZoneCells, z => Assert.Equal(a.Id, z.Owner));
        Assert.All(a.ZoneCells.Where(z => z.IsOceanSeed), z => Assert.Equal(ElevationType.Ocean, z.ElevationType));
        Assert.All(a.ZoneCells.Where(z => !z.IsOceanSeed), z => Assert.NotNull(z.Bedrock));
        Assert.InRange(a.OceanSeeds.Count(), 0, 2);
        Assert.InRange(a.ZoneMergers, 2, 4);
        Assert.InRange(a.ZoneCells.Count, 4, 40);
    }

    [Fact]
    public void The_fields_stay_in_their_ranges()
    {
        var fields = Shared.Fields;
        var rng = new Sfc32(3);
        for (var t = 0; t < 2000; t++)
        {
            var p = new WorldPoint((rng.Next() - 0.5) * 4e6, (rng.Next() - 0.5) * 4e6);
            Assert.InRange(fields.InitialTemperature(p), -16, 30);
            Assert.InRange(fields.Precipitation(p), 100, 12000);
            Assert.InRange(fields.Sample(FieldKind.Prosperity, p), 1, 100);
            Assert.InRange(fields.Sample(FieldKind.Supernatural, p), 0, 100);
        }
    }
}

public class WorldTests
{
    [Fact]
    public void Rivers_reach_every_attractor_and_stay_on_their_own_land()
    {
        var world = World.Create("cellgen");
        foreach (var id in new[] { new RegionId(0, 0), new RegionId(2, 2), new RegionId(3, 1) })
        {
            var detail = world.DetailOf(id);
            var rivers = detail.Rivers;
            Assert.True(rivers.AttractorCount > 100);
            Assert.Equal(0, rivers.UnreachedCount);
            for (var k = 0; k < rivers.Nodes.Count; k++)
            {
                var n = rivers.Nodes[k];
                Assert.True(n.Parent < k); // a parent always has a smaller index
                if (n.IsRoot)
                {
                    Assert.True(n.Wet);
                    Assert.Equal(k, n.Tree);
                    continue;
                }
                Assert.True(rivers.Nodes[n.Parent].Flow >= n.Flow);
                if (!n.Wet && !rivers.Rivers.CrossesNeighbours)
                    Assert.Equal(id.Coord, detail.NodeInfo[k].Region);
            }
            // The flow at the river mouths is the number of attractors reached.
            Assert.Equal(rivers.AttractorCount, rivers.Nodes.Where(n => n.IsRoot).Sum(n => n.Flow));
        }
    }

    [Fact]
    public void Lakes_sit_on_rivers_on_land_that_may_hold_them()
    {
        var world = World.Create("cellgen");
        var lakes = 0;
        for (var j = 0; j < 3; j++)
            for (var i = 0; i < 3; i++)
            {
                var id = new RegionId(i, j);
                var detail = world.DetailOf(id);
                var p = world.Parameters.Detail.Lakes;
                foreach (var lake in detail.Lakes)
                {
                    lakes++;
                    var patch = detail.Find(lake.Patch);
                    Assert.NotNull(patch);
                    Assert.True(patch.MayHoldLake);
                    Assert.Contains(detail.Rivers.Nodes, n => n.Position.X == lake.X && n.Position.Y == lake.Y && !n.Wet);
                    var size = 2 * Math.Sqrt(lake.AreaM2 / Math.PI);
                    Assert.InRange(size, p.MinSizeKm * 1000 - 1, p.MaxSizeKm * 1000 + 1);
                    for (var k = 0; k < Gameworld.Generation.Lakes.LakeShape.RayCount; k++)
                    {
                        var edge = lake.OutlinePoint(k);
                        var info = world.Cells.CellAt(edge);
                        Assert.False(info.Ocean);
                        Assert.Equal(id.Coord, info.Region);
                    }
                }
            }
        Assert.True(lakes > 5);
    }

    [Fact]
    public void Prospects_keep_their_spacing_and_stay_off_the_region_border()
    {
        var world = World.Create("cellgen");
        var shown = new List<int>();
        for (var j = 0; j < 4; j++)
            for (var i = 0; i < 4; i++)
            {
                var plan = world.PlanOf(new RegionId(i, j));
                var p = world.Parameters.Planning;
                Assert.True(plan.Prospects.Length <= p.ProspectsPerRegion);
                Assert.True(plan.Prospects.Count(x => x.ForCity) <= p.CityProspectsPerRegion);
                Assert.Equal(plan.Prospects.Length, plan.Prospects.Select(x => x.Patch).Distinct().Count());
                shown.Add(plan.Prospects.Count(x => x.Score >= world.Parameters.Travel.MinProspectScore));
                foreach (var prospect in plan.Prospects)
                {
                    Assert.InRange(prospect.Score, 0.0001, 100);
                    Assert.True(prospect.Facts.Depth >= 1);
                    if (prospect.ForCity) Assert.True(prospect.Facts.Depth >= 2);
                    Assert.Equal(new GridCoord(i, j), world.Cells.CellAt(prospect.Spot).Region);
                }
                var cities = plan.Prospects.Where(x => x.ForCity).ToArray();
                for (var a = 0; a < cities.Length; a++)
                    for (var b = a + 1; b < cities.Length; b++)
                        Assert.True(cities[a].Spot.DistanceTo(cities[b].Spot) >= 2.5 * p.SpacingKm * 1000);
                for (var a = 0; a < plan.Prospects.Length; a++)
                    for (var b = a + 1; b < plan.Prospects.Length; b++)
                        Assert.True(plan.Prospects[a].Spot.DistanceTo(plan.Prospects[b].Spot) >= world.Cells.PatchSize);
            }
        shown.Sort();
        // The document measures a median of 13 of the 20 per region on show.
        Assert.InRange(shown[shown.Count / 2], 9, 17);
    }

    [Fact]
    public void A_travel_link_is_decided_by_exactly_one_region()
    {
        Assert.Equal(new GridCoord(3, 3), TravelBuilder.Owner(new GridCoord(3, 3), new GridCoord(3, 3)));
        Assert.Equal(new GridCoord(2, 4), TravelBuilder.Owner(new GridCoord(3, 3), new GridCoord(2, 4)));
        Assert.Equal(new GridCoord(2, 4), TravelBuilder.Owner(new GridCoord(2, 4), new GridCoord(3, 3)));
        Assert.Equal(new GridCoord(1, -1), TravelBuilder.Owner(new GridCoord(0, 0), new GridCoord(2, -1)));
        Assert.Equal(new GridCoord(-1, 0), TravelBuilder.Owner(new GridCoord(-2, 0), new GridCoord(0, 1)));
        Assert.Null(TravelBuilder.Owner(new GridCoord(0, 0), new GridCoord(3, 0)));

        var world = World.Create("cellgen");
        var seen = new HashSet<string>();
        for (var j = 1; j <= 3; j++)
            for (var i = 1; i <= 3; i++)
            {
                var region = new GridCoord(i, j);
                foreach (var link in world.SettlementOf(new RegionId(region)).Links)
                {
                    Assert.Equal(region, TravelBuilder.Owner(link.FromRegion.Coord, link.ToRegion.Coord));
                    Assert.True(link.Cost > 0);
                    Assert.NotEmpty(link.Route);
                    Assert.All(link.Route, s => Assert.True(s.Points.Count >= 2));
                    // No link is decided twice.
                    Assert.True(seen.Add($"{link.Kind == TravelLinkKind.Road}|{link.From}|{link.To}"));
                }
            }
        Assert.True(seen.Count > 50);
    }

    [Fact]
    public void Generation_does_not_depend_on_the_order_regions_are_explored_in()
    {
        var target = new RegionId(2, 2);

        var first = World.Create("cellgen");
        var a = first.Advance(target, RegionStage.Settled).ToSnapshot();

        var second = World.Create("cellgen");
        // Approach from the other side, through other regions and stages first.
        second.Advance(new RegionId(4, 3), RegionStage.Settled);
        second.Advance(new RegionId(1, 1), RegionStage.Elevated);
        second.Advance(new RegionId(2, 3), RegionStage.Planned);
        var b = second.Advance(target, RegionStage.Settled).ToSnapshot();

        Assert.Equal(DomainJson.Serialize(a.Outline), DomainJson.Serialize(b.Outline));
        Assert.Equal(DomainJson.Serialize(a.Detail), DomainJson.Serialize(b.Detail));
        Assert.Equal(DomainJson.Serialize(a.Plan), DomainJson.Serialize(b.Plan));
        Assert.Equal(DomainJson.Serialize(a.Settlement), DomainJson.Serialize(b.Settlement));
        Assert.Equal(a.Elevation!.HeightMap.Heights, b.Elevation!.HeightMap.Heights);
        Assert.Equal(DomainJson.Serialize(a.Elevation.Patches), DomainJson.Serialize(b.Elevation.Patches));
    }

    [Fact]
    public void Advancing_a_region_takes_the_regions_around_it_as_far_as_each_stage_needs()
    {
        var world = World.Create("atlas");
        var id = new RegionId(0, 0);
        Assert.Equal(RegionStage.Sited, world.StageOf(id));

        var region = world.Advance(id, RegionStage.Settled);
        Assert.Equal(RegionStage.Settled, region.Stage);
        Assert.NotNull(region.Detail);
        Assert.NotNull(region.Elevation);
        Assert.NotNull(region.Plan);
        Assert.NotNull(region.Settlement);

        foreach (var r in RegionStageRules.Bordering(id))
            Assert.True(world.StageOf(r) >= RegionStage.Planned);
        Assert.True(world.StageOf(new RegionId(2, 0)) >= RegionStage.Detailed);
        Assert.True(world.StageOf(new RegionId(3, 0)) >= RegionStage.Outlined);
        Assert.Equal(RegionStage.Sited, world.StageOf(new RegionId(9, 9)));

        // The zones of a region are the zones whose root cell the region owns.
        Assert.All(region.Outline.Zones, z => Assert.Equal(id, z.Region));
        Assert.All(region.Outline.Zones, z => Assert.Contains(z.Id.Root, z.Cells));
        Assert.All(region.Detail.Patches, p => Assert.Equal(p.IsOcean, p.Biome is null));
    }

    [Fact]
    public void Two_ocean_zones_are_on_the_same_sea_when_ocean_connects_them_through_outlined_regions()
    {
        var world = World.Create("cellgen");
        var oceans = new List<Zone>();
        for (var j = 0; j < 5; j++)
            for (var i = 0; i < 5; i++)
                oceans.AddRange(world.Advance(new RegionId(i, j), RegionStage.Outlined).Outline.OceanZones);
        Assert.True(oceans.Count > 10);

        var cells = world.Cells;
        var same = 0;
        foreach (var a in oceans)
        {
            Assert.True(world.AreOnSameSea(a.Id, a.Id));
            foreach (var b in oceans)
            {
                // The relation is symmetric, and two neighbouring ocean zones are always on one sea.
                Assert.Equal(world.AreOnSameSea(a.Id, b.Id), world.AreOnSameSea(b.Id, a.Id));
                var touching = a.Id != b.Id && a.Cells.Any(c => cells.Zones.Neighbours(c.Coord).Any(n => cells.ZoneRoot(n) == b.Id.Coord));
                if (touching) Assert.True(world.AreOnSameSea(a.Id, b.Id));
                if (a.Id != b.Id && world.AreOnSameSea(a.Id, b.Id)) same++;
            }
        }
        Assert.True(same > 0);
        Assert.True(same < oceans.Count * (oceans.Count - 1)); // not all one sea

        var land = world.Advance(new RegionId(2, 2), RegionStage.Outlined).Outline.LandZones.First();
        Assert.False(world.AreOnSameSea(land.Id, oceans[0].Id));
    }

    [Fact]
    public void No_layer_of_a_region_exists_before_the_region_has_reached_its_stage()
    {
        var world = World.Create("cellgen");
        var id = new RegionId(1, 1);

        world.DetailOf(id);
        Assert.Equal(RegionStage.Detailed, world.StageOf(id));
        Assert.Null(world.FindElevation(id));
        Assert.Null(world.FindPlan(id));

        // The stages are linear: Prospects take the region through Elevated first.
        var plan = world.PlanOf(id);
        Assert.Equal(RegionStage.Planned, world.StageOf(id));
        Assert.NotNull(world.FindElevation(id));
        Assert.NotNull(world.Find(id)!.Elevation);
        Assert.Same(plan, world.FindPlan(id));

        // The travel network takes the region to Settled and the regions around it to Planned.
        world.SettlementOf(id);
        Assert.Equal(RegionStage.Settled, world.StageOf(id));
        Assert.All(RegionStageRules.Bordering(id), r => Assert.True(world.StageOf(r) >= RegionStage.Planned));
        Assert.All(RegionStageRules.Bordering(id), r => Assert.NotNull(world.FindElevation(r)));
    }

    [Fact]
    public void Another_seed_or_other_parameters_give_another_world()
    {
        var a = World.Create("cellgen").Advance(new RegionId(0, 0), RegionStage.Detailed);
        var b = World.Create("atlas").Advance(new RegionId(0, 0), RegionStage.Detailed);
        Assert.NotEqual(DomainJson.Serialize(a.Outline), DomainJson.Serialize(b.Outline));

        var wetter = ParameterSet.Default with
        {
            Siting = ParameterSet.Default.Siting with { OceanSeedsPerRegion = new IntRange(2, 4) },
        };
        var c = World.Create("cellgen", wetter);
        int oceanDefault = 0, oceanWetter = 0;
        var cells = World.Create("cellgen").Cells;
        for (double y = 0; y < 1280000; y += 16000)
            for (double x = 0; x < 1280000; x += 16000)
            {
                if (cells.CellAt(x, y).Ocean) oceanDefault++;
                if (c.Cells.CellAt(x, y).Ocean) oceanWetter++;
            }
        Assert.True(oceanWetter > oceanDefault);
    }

    [Fact]
    public void The_map_renders_the_same_png_twice()
    {
        var view = new MapView(128000, 128000, 1000, 160, 160);
        var a = MapRenderer.Render(World.Create("cellgen"), view, MapMode.Biomes, MapLayers.All).ToPng();
        var b = MapRenderer.Render(World.Create("cellgen"), view, MapMode.Biomes, MapLayers.All).ToPng();
        Assert.Equal(a, b);
        Assert.Equal(0x89, a[0]);
        Assert.Equal((byte)'P', a[1]);
        Assert.True(a.Length > 1000);
    }

    [Fact]
    public void The_none_mode_is_white_and_a_field_layer_draws_its_band_colours_on_it()
    {
        var world = World.Create("cellgen");
        var view = new MapView(128000, 128000, 1000, 96, 96);

        var blank = MapRenderer.Render(world, view, MapMode.None, MapLayers.None);
        Assert.All(blank.Pixels, b => Assert.Equal(255, b));

        var layers = MapLayers.None with { InitialTemperature = true };
        var tinted = MapRenderer.Render(world, view, MapMode.None, layers);
        var land = 0;
        for (var y = 0; y < view.Height; y++)
            for (var x = 0; x < view.Width; x++)
            {
                var wx = view.Left + (x + 0.5) * view.MetresPerPixel;
                var wy = view.Top + (y + 0.5) * view.MetresPerPixel;
                var ocean = world.Cells.CellAt(wx, wy).Ocean;
                var expected = ocean
                    ? new Rgb(255, 255, 255)
                    : MapRenderer.FieldColour(FieldKind.InitialTemperature, world.Fields.Sample(FieldKind.InitialTemperature, wx, wy));
                Assert.Equal(expected, tinted.Get(x, y));
                if (!ocean) land++;
            }
        Assert.True(land > 0);
    }

    [Fact]
    public void The_warp_moves_where_a_field_is_read_and_keeps_its_range()
    {
        ParameterSet With(double warpKm) => ParameterSet.Default with
        {
            Fields = ParameterSet.Default.Fields with
            {
                InitialTemperature = ParameterSet.Default.Fields.InitialTemperature with { WarpKm = warpKm },
            },
        };
        var plain = World.Create("cellgen", With(0)).Fields;
        var warped = World.Create("cellgen", With(300)).Fields;
        var builtIn = World.Create("cellgen").Fields;
        var range = ParameterSet.Default.Fields.InitialTemperature;

        var moved = 0;
        for (double y = 0; y < 640000; y += 16000)
            for (double x = 0; x < 640000; x += 16000)
            {
                var p = new WorldPoint(x, y);
                var t = warped.InitialTemperature(p);
                Assert.InRange(t, range.Min, range.Max);
                Assert.Equal(builtIn.InitialTemperature(p), t); // 300 km is the default
                if (Math.Abs(t - plain.InitialTemperature(p)) > 1) moved++;
                // A field without warp is the plain noise: the temperature warp does not touch it.
                Assert.Equal(plain.Sample(FieldKind.Prosperity, p), warped.Sample(FieldKind.Prosperity, p));
            }
        Assert.True(moved > 400);
    }

    [Fact]
    public void Cold_steppe_takes_the_steppe_and_sub_humid_classes_of_the_boreal_belt()
    {
        var rules = new Gameworld.Generation.Biomes.BiomeRules(ParameterSet.Default.Elevation.Biomes);
        Biome At(double temperatureC, double precipitationMm) =>
            rules.Classify(temperatureC, precipitationMm, 0, ElevationType.Lowland, coastal: false, riverPoor: false).Biome;

        // At 0 °C the dryness threshold is 140 mm/yr: desert below 70, steppe to 280, forest above.
        Assert.Equal(Biome.ColdDesert, At(0, 60));
        Assert.Equal(Biome.ColdSteppe, At(0, 100));
        Assert.Equal(Biome.ColdSteppe, At(0, 200));
        Assert.Equal(Biome.BorealForest, At(0, 300));
        // The temperate belt keeps its own sub-humid biome.
        Assert.Equal(Biome.WoodlandAndShrubland, At(10, 500));
    }

    [Fact]
    public void A_distribution_gives_each_share_of_the_map_its_value()
    {
        var parameters = ParameterSet.Default.Fields;
        var fields = World.Create("cellgen").Fields;
        foreach (var kind in new[] { FieldKind.InitialTemperature, FieldKind.Precipitation, FieldKind.Supernatural })
        {
            var points = parameters[kind].Distribution;
            Assert.NotEmpty(points);
            var below = new int[points.Count];
            var samples = 0;
            // The step is not a multiple of a feature size: steps of 10 km would only hit the
            // lattice points of the Supernatural noise, where it is not spread like the rest.
            for (double y = -2e6; y < 2e6; y += 9700)
                for (double x = -2e6; x < 2e6; x += 9700)
                {
                    var value = fields.Sample(kind, x, y);
                    Assert.InRange(value, parameters[kind].Min, parameters[kind].Max);
                    for (var k = 0; k < points.Count; k++)
                        if (value < points[k].Value) below[k]++;
                    samples++;
                }
            // The shares hold over a large area: here 4000 km × 4000 km of one seed.
            for (var k = 0; k < points.Count; k++)
                Assert.InRange((double)below[k] / samples, points[k].Share - 0.04, points[k].Share + 0.04);
        }
    }

    [Fact]
    public void A_distribution_reshapes_a_field_without_moving_its_highs_and_lows()
    {
        var shaped = World.Create("cellgen").Fields;
        var bell = World.Create("cellgen", ParameterSet.Default with
        {
            Fields = ParameterSet.Default.Fields with
            {
                InitialTemperature = ParameterSet.Default.Fields.InitialTemperature with { Distribution = [] },
                Precipitation = ParameterSet.Default.Fields.Precipitation with { Distribution = [] },
            },
        }).Fields;

        var rng = new Sfc32(11);
        var changed = 0;
        for (var t = 0; t < 2000; t++)
        {
            var a = new WorldPoint((rng.Next() - 0.5) * 4e6, (rng.Next() - 0.5) * 4e6);
            var b = new WorldPoint((rng.Next() - 0.5) * 4e6, (rng.Next() - 0.5) * 4e6);
            foreach (var kind in new[] { FieldKind.InitialTemperature, FieldKind.Precipitation })
            {
                // The point with the higher value is the same one under both: the order is kept.
                var order = bell.Sample(kind, a).CompareTo(bell.Sample(kind, b));
                if (order != 0) Assert.Equal(order, Math.Sign(shaped.Sample(kind, a).CompareTo(shaped.Sample(kind, b)) + order));
                if (Math.Abs(shaped.Sample(kind, a) - bell.Sample(kind, a)) > 1e-9) changed++;
            }
        }
        Assert.True(changed > 3000);
    }

    [Fact]
    public void A_field_band_holds_its_lower_bound_and_not_its_upper()
    {
        var bands = MapRenderer.FieldBands(FieldKind.Precipitation);
        Assert.Null(bands[0].From);
        Assert.Null(bands[^1].To);
        Assert.Equal(bands[0].Colour, MapRenderer.FieldColour(FieldKind.Precipitation, 249.9));
        Assert.Equal(bands[1].Colour, MapRenderer.FieldColour(FieldKind.Precipitation, 250));
        Assert.Equal(bands[^1].Colour, MapRenderer.FieldColour(FieldKind.Precipitation, 12000));
    }
}
