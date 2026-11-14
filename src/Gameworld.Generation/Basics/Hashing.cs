namespace Gameworld.Generation.Basics;

/// <summary>
/// Hashing and random numbers (map-generation.md section 2.2 and Appendix A). All arithmetic
/// is 32-bit integer arithmetic with wrap-around.
/// </summary>
public static class Hashing
{
    /// <summary>pcg3d (Jarzynski &amp; Olano 2020), first lane.</summary>
    public static uint Pcg3(uint a, uint b, uint c)
    {
        unchecked
        {
            var x = a * 1664525u + 1013904223u;
            var y = b * 1664525u + 1013904223u;
            var z = c * 1664525u + 1013904223u;
            x += y * z; y += z * x; z += x * y;
            x ^= x >> 16; y ^= y >> 16; z ^= z >> 16;
            x += y * z;
            return x;
        }
    }

    /// <summary>MurmurHash3 finalizer.</summary>
    public static uint Fmix32(uint h)
    {
        unchecked
        {
            h ^= h >> 16; h *= 0x85ebca6bu;
            h ^= h >> 13; h *= 0xc2b2ae35u;
            h ^= h >> 16;
            return h;
        }
    }

    public static uint Hash4(uint seed, uint purpose, int x, int y) =>
        unchecked(Fmix32(Pcg3((uint)x, (uint)y, seed ^ (purpose * 0x9e3779b9u))));

    /// <summary>Uniform in [0, 1).</summary>
    public static double ToUnit(uint h) => h / 4294967296.0;

    /// <summary>The per-cell random number of one purpose.</summary>
    public static double Unit(uint seed, uint purpose, int x, int y) => ToUnit(Hash4(seed, purpose, x, y));

    /// <summary>Uniform integer in [lo, hi] from u in [0, 1).</summary>
    public static int Between(int lo, int hi, double u)
    {
        var a = Math.Min(lo, hi);
        var b = Math.Max(lo, hi);
        return a + Math.Min(b - a, (int)Math.Floor(u * (b - a + 1)));
    }

    /// <summary>k distinct items: a partial Fisher–Yates shuffle on a copy.</summary>
    public static T[] Pick<T>(IReadOnlyList<T> items, int k, Sfc32 rng)
    {
        var a = items.ToArray();
        var n = Math.Min(k, a.Length);
        for (var t = 0; t < n; t++)
        {
            var r = t + (int)Math.Floor(rng.Next() * (a.Length - t));
            (a[t], a[r]) = (a[r], a[t]);
        }
        return a[..Math.Max(0, n)];
    }
}

/// <summary>The small sequential generator: sfc32 seeded from SplitMix32, 12 outputs discarded.</summary>
public sealed class Sfc32
{
    private uint _a, _b, _c, _d;

    public Sfc32(uint seed)
    {
        unchecked
        {
            var s = seed;
            uint SplitMix()
            {
                s += 0x9e3779b9u;
                var z = s;
                z = (z ^ (z >> 16)) * 0x85ebca6bu;
                z = (z ^ (z >> 13)) * 0xc2b2ae35u;
                return z ^ (z >> 16);
            }
            _a = SplitMix(); _b = SplitMix(); _c = SplitMix(); _d = SplitMix();
        }
        for (var i = 0; i < 12; i++) Next();
    }

    /// <summary>The next float in [0, 1).</summary>
    public double Next()
    {
        unchecked
        {
            var t = _a + _b + _d;
            _d += 1;
            _a = _b ^ (_b >> 9);
            _b = _c + (_c << 3);
            _c = (_c << 21) | (_c >> 11);
            _c += t;
            return t / 4294967296.0;
        }
    }

    /// <summary>A random unit vector by rejection sampling; <paramref name="minD"/> is the smallest accepted squared length.</summary>
    public (double X, double Y) NextUnitVector(double minD)
    {
        while (true)
        {
            var x = 2 * Next() - 1;
            var y = 2 * Next() - 1;
            var d = x * x + y * y;
            if (d <= 1 && d > minD)
            {
                var s = Math.Sqrt(d);
                return (x / s, y / s);
            }
        }
    }
}
