using Gameworld.Domain;
using Gameworld.Generation.Basics;

namespace Gameworld.Generation;

/// <summary>
/// The four Fields (map-generation.md section 9): stateless functions of foundation and
/// coordinate. Each is an independent Perlin fBm stretched to a [min, max] range, read at a
/// position that a warp noise has moved, which draws the features out into belts. A field with
/// a distribution spreads its values over the map as its points say, instead of in the bell
/// shape of the noise.
/// </summary>
public sealed class Fields
{
    private readonly Field[] _fields;

    public Fields(Foundation foundation)
    {
        var seed = foundation.Seed.Value;
        var p = foundation.Parameters.Fields;
        _fields =
        [
            new Field(seed ^ 0x2b9d40f3u, p.Precipitation, logarithmic: true),
            new Field(seed ^ 0x7e3a11c5u, p.InitialTemperature, logarithmic: false),
            new Field(seed ^ 0x5c1f86a9u, p.Prosperity, logarithmic: false),
            new Field(seed ^ 0x3ad5e217u, p.Supernatural, logarithmic: false),
        ];
    }

    public double Sample(FieldKind kind, WorldPoint p) => _fields[(int)kind].Value(p.X, p.Y);

    public double Sample(FieldKind kind, double x, double y) => _fields[(int)kind].Value(x, y);

    /// <summary>The field rescaled to 0–1 between its min and max.</summary>
    public double Sample01(FieldKind kind, WorldPoint p)
    {
        var f = _fields[(int)kind];
        var span = f.Max - f.Min;
        return span == 0 ? 0 : Math.Clamp((f.Value(p.X, p.Y) - f.Min) / span, 0, 1);
    }

    /// <summary>mm/yr.</summary>
    public double Precipitation(WorldPoint p) => Sample(FieldKind.Precipitation, p);

    /// <summary>°C at sea level.</summary>
    public double InitialTemperature(WorldPoint p) => Sample(FieldKind.InitialTemperature, p);

    private sealed class Field
    {
        private readonly uint _seed;
        private readonly double _f;
        private readonly double _octaves, _lacunarity, _gain;
        private readonly double _spread;
        private readonly double _warp, _warpF;
        private readonly bool _log;
        private readonly double _lnMin, _lnMax;
        private readonly MonotoneSpline? _distribution;

        public Field(uint seed, FieldParameters p, bool logarithmic)
        {
            _seed = seed;
            _f = 1 / (p.FeatureSizeKm * 1000);
            _octaves = p.Octaves;
            _lacunarity = p.Lacunarity;
            _gain = p.Gain;
            _log = logarithmic;
            _warp = p.WarpFeatureSizeKm > 0 ? Math.Max(0, p.WarpKm) * 1000 : 0;
            _warpF = _warp > 0 ? 1 / (p.WarpFeatureSizeKm * 1000) : 0;
            Min = logarithmic ? Math.Max(1, p.Min) : p.Min;
            Max = logarithmic ? Math.Max(1, p.Max) : p.Max;
            if (logarithmic)
            {
                _lnMin = DetMath.Ln(Min);
                _lnMax = DetMath.Ln(Max);
            }

            // Σ a / √(Σ a²) with a = 1, gain, gain², …: the spread is independent of the octave count.
            double sum = 0, sumSq = 0, amp = 1;
            var n = Math.Max(1, (int)Math.Floor(p.Octaves));
            for (var o = 0; o < n; o++)
            {
                sum += amp;
                sumSq += amp * amp;
                amp *= p.Gain;
            }
            _spread = sum / Math.Sqrt(sumSq);

            // The distribution: the value at every share of the map, from Min at 0 to Max at 1.
            var low = Math.Min(Min, Max);
            var high = Math.Max(Min, Max);
            var points = new List<(double X, double Y)> { (0, Scale(Min)) };
            foreach (var q in p.Distribution)
                if (q.Share > 0 && q.Share < 1)
                    points.Add((q.Share, Scale(Math.Clamp(q.Value, low, high))));
            points.Add((1, Scale(Max)));
            if (points.Count > 2) _distribution = new MonotoneSpline(points);
        }

        private double Scale(double value) => _log ? DetMath.Ln(Math.Max(1, value)) : value;

        public double Min { get; }
        public double Max { get; }

        public double Value(double x, double y)
        {
            if (_warp > 0)
            {
                // The warp moves the position the field is read at; both offsets come from the unmoved position.
                var wx = x * _warpF;
                var wy = y * _warpF;
                x += _warp * Noise.PerlinFbm(_seed ^ 0x2f6b1e3du, wx, wy, 2, 2, 0.5);
                y += _warp * Noise.PerlinFbm(_seed ^ 0x5bd1e995u, wx + 31.7, wy - 47.3, 2, 2, 0.5);
            }
            var n = Noise.PerlinFbm(_seed, x * _f, y * _f, _octaves, _lacunarity, _gain);
            n *= _spread;
            if (_distribution != null)
            {
                // The share of the map with a lower noise value, then the value the distribution gives that share.
                var d = _distribution.Evaluate(NormalShare(n / NoiseSd));
                return Math.Clamp(_log ? DetMath.Exp(d) : d, Math.Min(Min, Max), Math.Max(Min, Max));
            }
            n = Math.Clamp(n / 0.8, -1, 1);
            var t = (n + 1) / 2;
            if (!_log) return Min + t * (Max - Min);
            var v = DetMath.Exp(_lnMin + t * (_lnMax - _lnMin));
            return Math.Clamp(v, Math.Min(Min, Max), Math.Max(Min, Max));
        }

        /// <summary>The standard deviation of the noise after the spread: it is close to a normal distribution.</summary>
        private const double NoiseSd = 0.305;

        /// <summary>
        /// The share of a normal distribution below z (Abramowitz &amp; Stegun 7.1.26, within 2·10⁻⁷),
        /// built on the deterministic exp.
        /// </summary>
        private static double NormalShare(double z)
        {
            var x = Math.Abs(z) * 0.7071067811865476; // |z| / √2
            var k = 1 / (1 + 0.3275911 * x);
            var poly = ((((1.061405429 * k - 1.453152027) * k + 1.421413741) * k - 0.284496736) * k + 0.254829592) * k;
            var tail = 0.5 * poly * DetMath.Exp(-x * x);
            return z < 0 ? tail : 1 - tail;
        }
    }
}
