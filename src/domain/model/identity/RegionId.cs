namespace Gameworld.Domain.Model;

/// <summary>
/// Identity of a region: its coordinate in the region grid. The world is unbounded, so any
/// coordinate is a valid region.
/// </summary>
public readonly record struct RegionId(int X, int Y)
{
    public RegionId Offset(int dx, int dy) => new(X + dx, Y + dy);

    /// <summary>The number of rings between two regions. Bordering regions are one ring apart.</summary>
    public int RingsTo(RegionId other) => Math.Max(Math.Abs(X - other.X), Math.Abs(Y - other.Y));

    /// <summary>The regions exactly <paramref name="ring"/> rings away. Ring 0 is the region itself.</summary>
    public IEnumerable<RegionId> Ring(int ring)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(ring);
        for (var dy = -ring; dy <= ring; dy++)
            for (var dx = -ring; dx <= ring; dx++)
                if (Math.Max(Math.Abs(dx), Math.Abs(dy)) == ring)
                    yield return Offset(dx, dy);
    }

    /// <summary>The region itself and all regions within <paramref name="rings"/> rings.</summary>
    public IEnumerable<RegionId> Within(int rings)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(rings);
        for (var dy = -rings; dy <= rings; dy++)
            for (var dx = -rings; dx <= rings; dx++)
                yield return Offset(dx, dy);
    }

    public override string ToString() => FormattableString.Invariant($"r{X},{Y}");
}
