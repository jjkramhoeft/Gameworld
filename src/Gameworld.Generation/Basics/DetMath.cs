namespace Gameworld.Generation.Basics;

/// <summary>
/// Deterministic exp, ln, sin and cos (map-generation.md Appendix A), built from + − × ÷,
/// rounding and the float's bit pattern only, so the result is the same on every machine.
/// </summary>
public static class DetMath
{
    private const double Ln2 = 0.6931471805599453;
    private const double Sqrt2 = 1.4142135623730951;
    private const double TwoPi = 6.283185307179586;
    private const double HalfPi = 1.5707963267948966;

    private static double Round(double x) => Math.Floor(x + 0.5);

    public static double Exp(double x)
    {
        var k = Round(x / Ln2);
        var r = x - k * 0.6931471803691238 - k * 1.9082149292705877e-10;
        var s = 1.0;
        for (var n = 16; n >= 1; n--) s = 1 + r * s / n;
        return s * Pow2((int)k);
    }

    public static double Ln(double x)
    {
        if (!(x > 0)) return double.NegativeInfinity;
        var bits = BitConverter.DoubleToInt64Bits(x);
        var e = (int)((bits >> 52) & 0x7ff);
        if (e == 0)
        {
            // Subnormal: scale up first.
            return Ln(x * 4503599627370496.0) - 52 * Ln2;
        }
        e -= 1023;
        var m = BitConverter.Int64BitsToDouble((bits & 0x000fffffffffffffL) | 0x3ff0000000000000L);
        if (m > Sqrt2)
        {
            m /= 2;
            e += 1;
        }
        var s = (m - 1) / (m + 1);
        var s2 = s * s;
        var t = 0.0;
        for (var n = 25; n >= 1; n -= 2) t = 1.0 / n + s2 * t;
        return 2 * s * t + e * Ln2;
    }

    public static double Sin(double x)
    {
        Reduce(x, out var q, out var r);
        return q switch
        {
            0 => SinP(r),
            1 => CosP(r),
            2 => -SinP(r),
            _ => -CosP(r),
        };
    }

    public static double Cos(double x)
    {
        Reduce(x, out var q, out var r);
        return q switch
        {
            0 => CosP(r),
            1 => -SinP(r),
            2 => -CosP(r),
            _ => SinP(r),
        };
    }

    private static void Reduce(double x, out int q, out double r)
    {
        x -= TwoPi * Round(x / TwoPi);
        var qd = Round(x / HalfPi);
        r = x - qd * HalfPi;
        q = (((int)qd % 4) + 4) % 4;
    }

    private static double SinP(double r)
    {
        var r2 = r * r;
        var t = 1.0;
        for (var n = 17; n >= 3; n -= 2) t = 1 - r2 / (n * (n - 1)) * t;
        return r * t;
    }

    private static double CosP(double r)
    {
        var r2 = r * r;
        var t = 1.0;
        for (var n = 18; n >= 2; n -= 2) t = 1 - r2 / (n * (n - 1)) * t;
        return t;
    }

    /// <summary>2^k built from the exponent bits.</summary>
    private static double Pow2(int k)
    {
        if (k < -1022) return 0;
        if (k > 1023) return double.PositiveInfinity;
        return BitConverter.Int64BitsToDouble((long)(k + 1023) << 52);
    }
}
