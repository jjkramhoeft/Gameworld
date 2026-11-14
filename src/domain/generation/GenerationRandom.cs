namespace Gameworld.Domain.Generation;

/// <summary>
/// The random numbers of one generation step. It is seeded from a <see cref="Model.SeedHash"/>
/// and never shared between steps, so a step gives the same result whenever it runs.
/// A SplitMix64 sequence: the same on every platform and .NET version.
/// </summary>
public sealed class GenerationRandom(ulong seed)
{
    private ulong state = seed;

    public ulong NextULong()
    {
        state += 0x9E3779B97F4A7C15UL;
        var z = state;
        z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
        z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;
        return z ^ (z >> 31);
    }

    /// <summary>A number in [0, 1).</summary>
    public double NextDouble() => (NextULong() >> 11) * (1.0 / (1UL << 53));

    /// <summary>A whole number in [0, <paramref name="exclusiveMax"/>).</summary>
    public int NextInt(int exclusiveMax)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(exclusiveMax);
        return (int)Math.BigMul(NextULong(), (ulong)exclusiveMax, out _);
    }

    /// <summary>A whole number in the range, both ends included.</summary>
    public int NextInt(Model.IntRange range) => range.Min + NextInt(range.Max - range.Min + 1);
}
