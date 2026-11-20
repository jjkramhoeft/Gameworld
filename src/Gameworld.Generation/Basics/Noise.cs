namespace Gameworld.Generation.Basics;

/// <summary>
/// Simplex and Perlin noise without a permutation table, fBm and ridged noise
/// (map-generation.md section 2.3 and Appendix A).
/// </summary>
public static class Noise
{
    private const double F = 0.3660254037844386;   // (√3 − 1) / 2
    private const double G = 0.21132486540518713;  // (3 − √3) / 6
    private const double H = 0.8660254037844387;

    private static readonly double[] GradX = [1, H, 0.5, 0, -0.5, -H, -1, -H, -0.5, 0, 0.5, H];
    private static readonly double[] GradY = [0, 0.5, H, 1, H, 0.5, 0, -0.5, -H, -1, -H, -0.5];

    private static int Floor(double v)
    {
        var i = (int)v;
        return v < i ? i - 1 : i;
    }

    private static double Corner(uint seed, int cx, int cy, double dx, double dy)
    {
        var w = 0.5 - dx * dx - dy * dy;
        if (w <= 0) return 0;
        var g = (int)(Hashing.Pcg3(unchecked((uint)cx), unchecked((uint)cy), seed) % 12);
        var w2 = w * w;
        return w2 * w2 * (GradX[g] * dx + GradY[g] * dy);
    }

    /// <summary>2D simplex noise in about [−1, 1].</summary>
    public static double Simplex2(uint seed, double x, double y)
    {
        var s = (x + y) * F;
        var i = Floor(x + s);
        var j = Floor(y + s);
        var t = (i + j) * G;
        var x0 = x - (i - t);
        var y0 = y - (j - t);
        int i1, j1;
        if (x0 > y0) { i1 = 1; j1 = 0; } else { i1 = 0; j1 = 1; }
        var x1 = x0 - i1 + G;
        var y1 = y0 - j1 + G;
        var x2 = x0 - 1 + 2 * G;
        var y2 = y0 - 1 + 2 * G;
        var n = Corner(seed, i, j, x0, y0)
              + Corner(seed, i + i1, j + j1, x1, y1)
              + Corner(seed, i + 1, j + 1, x2, y2);
        return 99.204 * n;
    }

    private static double Fade(double t) => t * t * t * (t * (6 * t - 15) + 10);

    private static double Dot(uint seed, int a, int b, double dx, double dy)
    {
        var g = (int)(Hashing.Pcg3(unchecked((uint)a), unchecked((uint)b), seed) % 12);
        return GradX[g] * dx + GradY[g] * dy;
    }

    /// <summary>Classic gradient noise in [−1, 1].</summary>
    public static double Perlin2(uint seed, double x, double y)
    {
        var i = Floor(x);
        var j = Floor(y);
        var fx = x - i;
        var fy = y - j;
        var u = Fade(fx);
        var v = Fade(fy);
        var n00 = Dot(seed, i, j, fx, fy);
        var n10 = Dot(seed, i + 1, j, fx - 1, fy);
        var n01 = Dot(seed, i, j + 1, fx, fy - 1);
        var n11 = Dot(seed, i + 1, j + 1, fx - 1, fy - 1);
        var a = n00 + u * (n10 - n00);
        var b = n01 + u * (n11 - n01);
        return DetMath.Sqrt2 * (a + v * (b - a));
    }

    private static int OctaveCount(double octaves) => Math.Max(1, (int)Math.Floor(octaves));

    /// <summary>Fractal sum over simplex noise.</summary>
    public static double Fbm(uint seed, double x, double y, double octaves, double lacunarity, double gain) =>
        Fractal(seed, x, y, octaves, lacunarity, gain, perlin: false);

    /// <summary>Fractal sum over Perlin noise.</summary>
    public static double PerlinFbm(uint seed, double x, double y, double octaves, double lacunarity, double gain) =>
        Fractal(seed, x, y, octaves, lacunarity, gain, perlin: true);

    private static double Fractal(uint seed, double x, double y, double octaves, double lacunarity, double gain, bool perlin)
    {
        double sum = 0, norm = 0, amp = 1, px = x, py = y;
        var n = OctaveCount(octaves);
        for (var o = 0; o < n; o++)
        {
            var s = unchecked(seed + (uint)o * 0x3c6ef372u);
            sum += amp * (perlin ? Perlin2(s, px, py) : Simplex2(s, px, py));
            norm += amp;
            amp *= gain;
            var nx = (0.8 * px - 0.6 * py) * lacunarity + 17.3;
            var ny = (0.6 * px + 0.8 * py) * lacunarity - 9.1;
            px = nx;
            py = ny;
        }
        return sum / norm;
    }

    /// <summary>Musgrave's ridged multifractal over simplex noise, in [−1, 1].</summary>
    public static double Ridged(uint seed, double x, double y, double octaves, double lacunarity, double gain)
    {
        double sum = 0, norm = 0, amp = 1, weight = 1, px = x, py = y;
        var n = OctaveCount(octaves);
        for (var o = 0; o < n; o++)
        {
            var s = unchecked(seed + (uint)o * 0x3c6ef372u);
            var v = 1 - Math.Abs(Simplex2(s, px, py));
            var r = v * v * weight;
            weight = Math.Min(1, 2 * r);
            sum += amp * r;
            norm += amp;
            amp *= gain;
            var nx = (0.8 * px - 0.6 * py) * lacunarity + 17.3;
            var ny = (0.6 * px + 0.8 * py) * lacunarity - 9.1;
            px = nx;
            py = ny;
        }
        return 2 * sum / norm - 1;
    }
}
