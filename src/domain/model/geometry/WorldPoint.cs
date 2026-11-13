namespace Gameworld.Domain.Model;

/// <summary>A position in the world, in km. Absolute - not relative to a region.</summary>
public readonly record struct WorldPoint(double X, double Y)
{
    public double DistanceTo(WorldPoint other)
    {
        var dx = X - other.X;
        var dy = Y - other.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}
