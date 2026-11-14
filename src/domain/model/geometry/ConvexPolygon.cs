using System.Collections.Immutable;

namespace Gameworld.Domain.Model;

/// <summary>
/// A convex polygon with its vertices in counter-clockwise order. A Voronoi cell is one: a
/// square that is clipped by the bisector to every neighbouring site.
/// </summary>
public sealed class ConvexPolygon
{
    public ConvexPolygon(ImmutableArray<WorldPoint> vertices)
    {
        if (vertices.Length < 3)
            throw new ArgumentException("A polygon needs at least three vertices.", nameof(vertices));
        Vertices = vertices;
        Area = FanArea(vertices);
    }

    public ImmutableArray<WorldPoint> Vertices { get; }

    public double Area { get; }

    /// <summary>The axis-aligned square with its centre in <paramref name="centre"/>.</summary>
    public static ConvexPolygon Square(WorldPoint centre, double halfSize) => new(
    [
        new(centre.X - halfSize, centre.Y - halfSize),
        new(centre.X + halfSize, centre.Y - halfSize),
        new(centre.X + halfSize, centre.Y + halfSize),
        new(centre.X - halfSize, centre.Y + halfSize),
    ]);

    /// <summary>
    /// The part of the polygon that is nearer to <paramref name="site"/> than to
    /// <paramref name="other"/>. The site must lie inside the polygon.
    /// </summary>
    public ConvexPolygon ClipNearerTo(WorldPoint site, WorldPoint other)
    {
        var normalX = other.X - site.X;
        var normalY = other.Y - site.Y;
        var middleX = (site.X + other.X) / 2;
        var middleY = (site.Y + other.Y) / 2;

        // Positive on the far side of the bisector, the side that is cut away.
        double Side(WorldPoint point) => (point.X - middleX) * normalX + (point.Y - middleY) * normalY;

        if (Vertices.All(vertex => Side(vertex) <= 0))
            return this;

        var kept = ImmutableArray.CreateBuilder<WorldPoint>(Vertices.Length + 1);
        var previous = Vertices[^1];
        var previousSide = Side(previous);
        foreach (var current in Vertices)
        {
            var currentSide = Side(current);
            if ((currentSide <= 0) != (previousSide <= 0))
            {
                var t = previousSide / (previousSide - currentSide);
                kept.Add(new(previous.X + t * (current.X - previous.X), previous.Y + t * (current.Y - previous.Y)));
            }
            if (currentSide <= 0)
                kept.Add(current);
            previous = current;
            previousSide = currentSide;
        }

        if (kept.Count < 3)
            throw new InvalidOperationException("Nothing is left of the polygon. The site must lie inside it.");
        return new(kept.ToImmutable());
    }

    public bool Contains(WorldPoint point)
    {
        var previous = Vertices[^1];
        foreach (var current in Vertices)
        {
            if (Cross(previous, current, point) < 0)
                return false;
            previous = current;
        }
        return true;
    }

    /// <summary>The distance from a point inside the polygon to its nearest edge.</summary>
    public double DistanceToBoundary(WorldPoint point)
    {
        var nearest = double.MaxValue;
        var previous = Vertices[^1];
        foreach (var current in Vertices)
        {
            var length = previous.DistanceTo(current);
            if (length > 0)
                nearest = Math.Min(nearest, Cross(previous, current, point) / length);
            previous = current;
        }
        return nearest;
    }

    /// <summary>The distance from a point to the vertex farthest from it.</summary>
    public double MaxDistanceFrom(WorldPoint point) => Vertices.Max(vertex => vertex.DistanceTo(point));

    /// <summary>
    /// A point in the polygon. Three independent uniform numbers in [0, 1) give a point that
    /// is uniform over the area.
    /// </summary>
    public WorldPoint PointAt(double pick, double u, double v)
    {
        var origin = Vertices[0];
        var target = pick * Area;
        var last = Vertices.Length - 2;
        for (var i = 1; i <= last; i++)
        {
            var triangle = Cross(origin, Vertices[i], Vertices[i + 1]) / 2;
            if (target < triangle || i == last)
            {
                if (u + v > 1)
                {
                    u = 1 - u;
                    v = 1 - v;
                }
                var a = Vertices[i];
                var b = Vertices[i + 1];
                return new(
                    origin.X + u * (a.X - origin.X) + v * (b.X - origin.X),
                    origin.Y + u * (a.Y - origin.Y) + v * (b.Y - origin.Y));
            }
            target -= triangle;
        }
        throw new InvalidOperationException("Unreachable: a polygon has at least one triangle.");
    }

    private static double FanArea(ImmutableArray<WorldPoint> vertices)
    {
        var area = 0.0;
        for (var i = 1; i < vertices.Length - 1; i++)
            area += Cross(vertices[0], vertices[i], vertices[i + 1]) / 2;
        return area;
    }

    /// <summary>Twice the signed area of the triangle a-b-c. Positive when c is left of a-b.</summary>
    private static double Cross(WorldPoint a, WorldPoint b, WorldPoint c) =>
        (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
}
