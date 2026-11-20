using Gameworld.Domain;
using Gameworld.Generation.Basics;

namespace Gameworld.Tests;

/// <summary>Values that several test classes build the same way.</summary>
internal static class TestData
{
    /// <summary>One frequency for every biome.</summary>
    public static PerBiome<double> Everywhere(double f) =>
        new(f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f);

    /// <summary>A generator of its own for a purpose and an index.</summary>
    public static Sfc32 Rng(uint purpose, int i) => new(Hashing.Hash4(20261120, purpose, i, 0));
}
