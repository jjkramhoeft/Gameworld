using Gameworld.Domain.Generation;
using Gameworld.Domain.Model;

namespace Gameworld.Domain.Tests;

public class GeometryTests
{
    private static readonly WorldPoint Centre = new(10, 20);
    private static readonly ConvexPolygon Square = ConvexPolygon.Square(Centre, 5);

    [Fact]
    public void A_square_has_the_area_of_its_sides()
    {
        Assert.Equal(100, Square.Area, 9);
    }

    [Fact]
    public void Clipping_keeps_the_side_nearer_to_the_site()
    {
        var half = Square.ClipNearerTo(Centre, new WorldPoint(14, 20));

        Assert.Equal(70, half.Area, 9);
        Assert.True(half.Contains(new WorldPoint(11.9, 20)));
        Assert.False(half.Contains(new WorldPoint(12.1, 20)));
    }

    [Fact]
    public void A_site_too_far_away_to_cut_leaves_the_polygon_as_it_is()
    {
        Assert.Same(Square, Square.ClipNearerTo(Centre, new WorldPoint(40, 20)));
    }

    [Fact]
    public void Clipping_by_a_ring_of_sites_gives_their_voronoi_cell()
    {
        var cell = ConvexPolygon.Square(Centre, 100);
        foreach (var (dx, dy) in new[] { (4, 0), (-4, 0), (0, 4), (0, -4) })
            cell = cell.ClipNearerTo(Centre, new WorldPoint(Centre.X + dx, Centre.Y + dy));

        Assert.Equal(16, cell.Area, 9);
        Assert.Equal(2, cell.DistanceToBoundary(Centre), 9);
        Assert.Equal(Math.Sqrt(8), cell.MaxDistanceFrom(Centre), 9);
    }

    [Fact]
    public void The_distance_to_the_boundary_is_the_distance_to_the_nearest_edge()
    {
        Assert.Equal(1, Square.DistanceToBoundary(new WorldPoint(14, 22)), 9);
    }

    [Fact]
    public void Points_at_are_inside_the_polygon_and_cover_it()
    {
        var triangle = Square.ClipNearerTo(new WorldPoint(6, 16), new WorldPoint(15, 25));
        var random = new GenerationRandom(1);
        var points = Enumerable.Range(0, 2_000)
            .Select(_ => triangle.PointAt(random.NextDouble(), random.NextDouble(), random.NextDouble()))
            .ToList();

        Assert.All(points, point => Assert.True(triangle.Contains(point)));
        // Uniform over the area: about a quarter of the points fall in the corner quarter.
        Assert.InRange(points.Count(point => point.X < 10 && point.Y < 20), 800, 1_200);
    }

    /// <summary>The published SplitMix64 sequence for seed 0.</summary>
    [Fact]
    public void The_random_stream_is_split_mix_64()
    {
        var random = new GenerationRandom(0);

        Assert.Equal(0xE220A8397B1DCDAFUL, random.NextULong());
        Assert.Equal(0x6E789E6AA1B965F4UL, random.NextULong());
        Assert.Equal(0x06C45D188009454FUL, random.NextULong());
    }

    [Fact]
    public void Random_numbers_stay_in_their_range()
    {
        var random = new GenerationRandom(7);
        var range = new IntRange(8, 12);
        var rolls = Enumerable.Range(0, 1_000).Select(_ => random.NextInt(range)).ToList();

        Assert.Equal([8, 9, 10, 11, 12], rolls.Distinct().Order());
        Assert.All(Enumerable.Range(0, 1_000), _ => Assert.InRange(random.NextDouble(), 0, 0.9999999999999999));
    }
}
