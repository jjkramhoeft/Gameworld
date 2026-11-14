using Gameworld.Domain;
using Gameworld.Generation.Basics;

namespace Gameworld.Generation.Cells;

/// <summary>
/// One level of cells: an infinite jittered-grid Voronoi diagram defined without a mesh
/// (map-generation.md section 3). Not thread-safe: the neighbour lists are cached.
/// </summary>
public sealed class VoronoiLevel
{
    private readonly uint _seed;
    private readonly double _inv;
    private readonly Dictionary<long, GridCoord[]> _neighbours = new();

    public VoronoiLevel(uint seed, double size, double jitter, double warp, double warpFrequency)
    {
        _seed = seed;
        Size = size;
        _inv = 1 / size;
        Jitter = Math.Clamp(jitter, 0, 1);
        Warp = Math.Max(0, warp);
        WarpFrequency = warpFrequency;
    }

    /// <summary>Grid spacing in metres.</summary>
    public double Size { get; }
    public double Jitter { get; }
    /// <summary>Border warp amplitude in cell sizes.</summary>
    public double Warp { get; }
    /// <summary>Warp noise cycles per cell.</summary>
    public double WarpFrequency { get; }

    public double SiteX(int i, int j) =>
        (i + 0.5 + (Hashing.Unit(_seed, 0x3a01, i, j) - 0.5) * Jitter) * Size;

    public double SiteY(int i, int j) =>
        (j + 0.5 + (Hashing.Unit(_seed, 0x3a02, i, j) - 0.5) * Jitter) * Size;

    public WorldPoint Site(GridCoord c) => new(SiteX(c.I, c.J), SiteY(c.I, c.J));

    /// <summary>
    /// The cell that contains a point. The warp moves the query point, not the sites, so cell
    /// borders become wavy while the sites stay put.
    /// </summary>
    public GridCoord Query(double wx, double wy)
    {
        var x = wx;
        var y = wy;
        if (Warp > 0)
        {
            var f = WarpFrequency * _inv;
            var a = Warp * Size;
            x += a * Noise.Fbm(_seed ^ 0x2f6b1e3du, wx * f, wy * f, 2, 2, 0.5);
            y += a * Noise.Fbm(_seed ^ 0x5bd1e995u, wx * f + 31.7, wy * f - 47.3, 2, 2, 0.5);
        }
        return Nearest(x, y);
    }

    /// <summary>The grid square whose site is nearest to a point, without warp.</summary>
    public GridCoord Nearest(double x, double y)
    {
        var ci = (int)Math.Floor(x * _inv);
        var cj = (int)Math.Floor(y * _inv);
        var best = double.PositiveInfinity;
        int bi = ci, bj = cj;
        for (var r = 0; r <= 3; r++)
        {
            // A site in ring r is at least r − 1 + (1 − jitter)/2 grid units away.
            var bound = (r - 1 + (1 - Jitter) / 2) * Size;
            if (bound > 0 && bound * bound > best) break;
            for (var dj = -r; dj <= r; dj++)
            {
                var edgeRow = dj == -r || dj == r;
                for (var di = -r; di <= r; di += edgeRow ? 1 : 2 * r)
                {
                    var i = ci + di;
                    var j = cj + dj;
                    var sx = SiteX(i, j) - x;
                    var sy = SiteY(i, j) - y;
                    var d = sx * sx + sy * sy;
                    // Equal distances are resolved in favour of the smaller j, then the smaller i.
                    if (d < best || (d == best && (j < bj || (j == bj && i < bi))))
                    {
                        best = d;
                        bi = i;
                        bj = j;
                    }
                    if (r == 0) break;
                }
            }
        }
        return new GridCoord(bi, bj);
    }

    /// <summary>
    /// The Gabriel neighbours of a cell, sorted by site distance, nearest first (ties by key).
    /// Computed on the unwarped sites.
    /// </summary>
    public GridCoord[] Neighbours(GridCoord c)
    {
        if (_neighbours.TryGetValue(c.Key, out var cached)) return cached;

        var px = SiteX(c.I, c.J);
        var py = SiteY(c.I, c.J);
        var found = new List<(double D, long Key, GridCoord Cell)>();
        for (var dj = -2; dj <= 2; dj++)
        {
            for (var di = -2; di <= 2; di++)
            {
                if (di == 0 && dj == 0) continue;
                var q = new GridCoord(c.I + di, c.J + dj);
                var qx = SiteX(q.I, q.J);
                var qy = SiteY(q.I, q.J);
                var d2 = (qx - px) * (qx - px) + (qy - py) * (qy - py);
                if (IsGabriel(c, px, py, q, qx, qy, d2))
                    found.Add((d2, q.Key, q));
            }
        }
        found.Sort((a, b) => a.D != b.D ? a.D.CompareTo(b.D) : a.Key.CompareTo(b.Key));
        var result = found.Select(f => f.Cell).ToArray();
        _neighbours[c.Key] = result;
        return result;
    }

    public bool AreNeighbours(GridCoord a, GridCoord b) => Array.IndexOf(Neighbours(a), b) >= 0;

    /// <summary>No third site lies strictly inside the circle whose diameter is the segment between the two sites.</summary>
    private bool IsGabriel(GridCoord p, double px, double py, GridCoord q, double qx, double qy, double d2)
    {
        var mx = (px + qx) / 2;
        var my = (py + qy) / 2;
        var r = Math.Sqrt(d2) / 2;
        var i0 = (int)Math.Floor((mx - r) * _inv);
        var i1 = (int)Math.Floor((mx + r) * _inv);
        var j0 = (int)Math.Floor((my - r) * _inv);
        var j1 = (int)Math.Floor((my + r) * _inv);
        var r2 = d2 / 4;
        for (var j = j0; j <= j1; j++)
        {
            for (var i = i0; i <= i1; i++)
            {
                if ((i == p.I && j == p.J) || (i == q.I && j == q.J)) continue;
                var sx = SiteX(i, j) - mx;
                var sy = SiteY(i, j) - my;
                if (sx * sx + sy * sy < r2) return false;
            }
        }
        return true;
    }
}
