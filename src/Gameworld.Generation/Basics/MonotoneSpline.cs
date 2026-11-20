namespace Gameworld.Generation.Basics;

/// <summary>
/// A monotone cubic spline through a few control points (Fritsch &amp; Carlson 1980,
/// map-generation.md section 2.4). The curve is constant beyond its end points, unless
/// <c>extendLastSlope</c> lets it continue along the last segment's slope.
/// </summary>
public sealed class MonotoneSpline
{
    private readonly double[] _x;
    private readonly double[] _y;
    private readonly double[] _m;
    private readonly bool _extendLastSlope;

    public MonotoneSpline(IEnumerable<(double X, double Y)> points, bool extendLastSlope = false)
    {
        var sorted = new List<(double X, double Y)>();
        foreach (var p in points.OrderBy(p => p.X))
        {
            // Points with an x already used are dropped.
            if (sorted.Count > 0 && sorted[^1].X == p.X) continue;
            sorted.Add(p);
        }
        if (sorted.Count == 0) sorted.Add((0, 0));

        var n = sorted.Count;
        _x = sorted.Select(p => p.X).ToArray();
        _y = sorted.Select(p => p.Y).ToArray();
        _m = new double[n];
        _extendLastSlope = extendLastSlope;
        if (n == 1) return;

        var d = new double[n - 1];
        for (var i = 0; i < n - 1; i++) d[i] = (_y[i + 1] - _y[i]) / (_x[i + 1] - _x[i]);

        _m[0] = d[0];
        _m[n - 1] = d[n - 2];
        for (var i = 1; i < n - 1; i++)
            _m[i] = d[i - 1] * d[i] <= 0 ? 0 : (d[i - 1] + d[i]) / 2;

        for (var i = 0; i < n - 1; i++)
        {
            if (d[i] == 0)
            {
                _m[i] = 0;
                _m[i + 1] = 0;
                continue;
            }
            var a = _m[i] / d[i];
            var b = _m[i + 1] / d[i];
            var s = a * a + b * b;
            if (s > 9)
            {
                var t = 3 / Math.Sqrt(s);
                _m[i] = t * a * d[i];
                _m[i + 1] = t * b * d[i];
            }
        }
    }

    public double Evaluate(double x)
    {
        var n = _x.Length;
        if (n == 1 || x <= _x[0]) return _y[0];
        if (x >= _x[n - 1])
        {
            if (!_extendLastSlope) return _y[n - 1];
            var slope = (_y[n - 1] - _y[n - 2]) / (_x[n - 1] - _x[n - 2]);
            return _y[n - 1] + slope * (x - _x[n - 1]);
        }
        var i = 0;
        while (i < n - 2 && x > _x[i + 1]) i++;
        var h = _x[i + 1] - _x[i];
        var t = (x - _x[i]) / h;
        var t2 = t * t;
        var t3 = t2 * t;
        return (2 * t3 - 3 * t2 + 1) * _y[i]
             + (t3 - 2 * t2 + t) * h * _m[i]
             + (-2 * t3 + 3 * t2) * _y[i + 1]
             + (t3 - t2) * h * _m[i + 1];
    }
}
