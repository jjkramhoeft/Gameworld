using Gameworld.Domain;
using Gameworld.Generation.Basics;
using Gameworld.Generation.Cells;

namespace Gameworld.Generation.Lakes;

/// <summary>A placed lake: a star-shaped polygon of 64 rays around a river node.</summary>
public sealed class LakeShape
{
    public const int RayCount = 64;

    internal static readonly double[] DirX = new double[RayCount];
    internal static readonly double[] DirY = new double[RayCount];
    internal static readonly double SinStep;

    static LakeShape()
    {
        for (var k = 0; k < RayCount; k++)
        {
            DirX[k] = DetMath.Cos(DetMath.TwoPi * k / RayCount);
            DirY[k] = DetMath.Sin(DetMath.TwoPi * k / RayCount);
        }
        SinStep = DetMath.Sin(DetMath.TwoPi / RayCount);
    }

    /// <summary>The root cell of the patch the lake is placed in.</summary>
    public required GridCoord Patch { get; init; }
    public required double X { get; init; }
    public required double Y { get; init; }
    /// <summary>Half the nominal size: the radius the spacing between lakes is measured with.</summary>
    public required double NominalRadius { get; init; }
    /// <summary>The length of each ray. Set once: the array must not be changed after the shape is made.</summary>
    public required double[] Rays
    {
        get => _rays;
        init
        {
            _rays = value;
            MaxRay = value.Max();
        }
    }
    private readonly double[] _rays = [];

    public required double AreaM2 { get; init; }

    /// <summary>The longest ray, computed once when the rays are set: no point of the outline lies further from the centre.</summary>
    public double MaxRay { get; private init; }

    public WorldPoint OutlinePoint(int k) => new(X + DirX[k] * Rays[k], Y + DirY[k] * Rays[k]);

    /// <summary>The point lies inside the outline (even-odd rule on the polygon).</summary>
    public bool Contains(double x, double y)
    {
        var dx = x - X;
        var dy = y - Y;
        var max = MaxRay;
        if (dx * dx + dy * dy > max * max) return false;
        var inside = false;
        for (int k = 0, prev = RayCount - 1; k < RayCount; prev = k++)
        {
            var x1 = X + DirX[k] * Rays[k];
            var y1 = Y + DirY[k] * Rays[k];
            var x0 = X + DirX[prev] * Rays[prev];
            var y0 = Y + DirY[prev] * Rays[prev];
            if ((y1 > y) != (y0 > y) && x < (x0 - x1) * (y - y1) / (y0 - y1) + x1)
                inside = !inside;
        }
        return inside;
    }

    public Lake ToDomain() => new(
        new PatchId(new PatchCellId(Patch)),
        new WorldPoint(X, Y),
        Enumerable.Range(0, RayCount).Select(OutlinePoint).ToArray(),
        AreaM2);
}

/// <summary>
/// Lakes (map-generation.md section 11): placed per region after the rivers, always on a river.
/// </summary>
internal static class LakeBuilder
{
    /// <param name="mayHoldLake">Whether a land patch (by root cell) may hold a lake: not in a mountains zone, not a desert at sea level, not in a river-poor flat zone.</param>
    public static List<LakeShape> Build(
        CellMap cells,
        GridCoord region,
        uint seed,
        LakeParameters p,
        IReadOnlyList<RiverNode> nodes,
        Func<GridCoord, bool> mayHoldLake)
    {
        var lakes = new List<LakeShape>();
        var minSize = p.MinSizeKm * 1000;
        var maxSize = p.MaxSizeKm * 1000;

        bool Eligible(double x, double y)
        {
            var info = cells.CellAt(x, y);
            return !info.Ocean && info.Region == region && mayHoldLake(info.Patch);
        }

        // Candidates: river nodes on land that are not roots, carry a flow of at least 4 and lie
        // at a lake-eligible point; grouped by patch cell, in node index order.
        var candidates = new Dictionary<long, List<int>>();
        for (var k = 0; k < nodes.Count; k++)
        {
            var n = nodes[k];
            if (n.Wet || n.IsRoot || n.Flow < 4) continue;
            var info = cells.CellAt(n.Position);
            if (info.Ocean || info.Region != region || !mayHoldLake(info.Patch)) continue;
            if (!candidates.TryGetValue(info.PatchCell.Key, out var list))
            {
                list = new List<int>();
                candidates[info.PatchCell.Key] = list;
            }
            list.Add(k);
        }

        var rng = new Sfc32(Hashing.Hash4(seed, 0x1a01, region.I, region.J));
        foreach (var zoneCell in cells.ZoneCellsDrawnIn(region))
        {
            var type = cells.ZoneType(zoneCell);
            if (type is ElevationType.Ocean or ElevationType.Mountains) continue;
            foreach (var pc in cells.OwnedPatchCells(zoneCell))
            {
                var info = cells.InfoOf(pc);
                if (info.Ocean || info.Region != region) continue;

                // Three numbers for every such patch cell, so the stream does not depend on which patch cells have rivers.
                var roll = rng.Next();
                var u = rng.Next();
                var pickU = rng.Next();
                if (!candidates.TryGetValue(pc.Key, out var list)) continue;
                if (roll >= p.Frequency) continue;

                var centre = nodes[list[Math.Min(list.Count - 1, (int)Math.Floor(pickU * list.Count))]].Position;
                var nominal = minSize + (maxSize - minSize) * u * u;
                var radius = nominal / 2;

                var tooClose = false;
                foreach (var other in lakes)
                {
                    var dx = centre.X - other.X;
                    var dy = centre.Y - other.Y;
                    var min = 1.15 * (radius + other.NominalRadius);
                    if (dx * dx + dy * dy < min * min)
                    {
                        tooClose = true;
                        break;
                    }
                }
                if (tooClose) continue;

                var lake = Shape(seed, info.Patch, lakes.Count, centre, radius, minSize, maxSize, Eligible);
                if (lake != null) lakes.Add(lake);
            }
        }
        return lakes;
    }

    private static LakeShape? Shape(
        uint seed,
        GridCoord patch,
        int id,
        WorldPoint centre,
        double radius,
        double minSize,
        double maxSize,
        Func<double, double, bool> eligible)
    {
        const int n = LakeShape.RayCount;
        var s = Hashing.Hash4(seed, 0x1a02, unchecked(patch.I * 4099 + id), patch.J);
        var srng = new Sfc32(s);
        var noiseSeed = s ^ 0x5bd1e995u;

        // The draws, in order: the axis, u₁, u₂, nx, ny and the ring u.
        var (axisX, axisY) = srng.NextUnitVector(1e-6);
        var u1 = srng.Next();
        var u2 = srng.Next();
        var nx = 1000 * srng.Next();
        var ny = 1000 * srng.Next();
        var ring = 0.75 * (0.6 + 0.8 * srng.Next());

        // An ellipse of unchanged area with semi-axes √e along the axis and 1/√e across it.
        var e = 1 + 1.2 * u1 * u2;
        var a = Math.Sqrt(e);
        var b = 1 / a;

        var step = Math.Max(60, radius / 12);
        var r = new double[n];
        for (var k = 0; k < n; k++)
        {
            var dx = LakeShape.DirX[k];
            var dy = LakeShape.DirY[k];
            var along = dx * axisX + dy * axisY;
            var across = dx * axisY - dy * axisX;
            var ellipse = a * b / Math.Sqrt(b * along * b * along + a * across * a * across);
            var noise = Noise.Fbm(noiseSeed, nx + dx * ring, ny + dy * ring, 2, 2, 0.35);
            var wanted = radius * Math.Max(0.25, ellipse * (1 + 0.3 * noise));

            // Clip: walk the ray outwards and stop one step before the first point that is not lake-eligible.
            var length = wanted;
            for (var t = step; ; t += step)
            {
                var at = Math.Min(t, wanted);
                if (!eligible(centre.X + dx * at, centre.Y + dy * at))
                {
                    length = Math.Max(0, t - step);
                    break;
                }
                if (t >= wanted) break;
            }
            r[k] = length;
        }

        // Edges: while the edge between two neighbouring ray ends crosses ineligible ground, shorten both rays.
        for (var k = 0; k < n; k++)
        {
            var k1 = (k + 1) % n;
            for (var attempt = 0; ; attempt++)
            {
                if (!EdgeCrossesIneligible(centre, r, k, k1, step, eligible)) break;
                if (attempt >= 12)
                {
                    r[k] = 0;
                    r[k1] = 0;
                    break;
                }
                r[k] *= 0.85;
                r[k1] *= 0.85;
            }
        }

        // Taper: neighbouring rays differ by at most 0.09·R.
        var taper = 0.09 * radius;
        for (var pass = 0; pass < 2; pass++)
        {
            for (var k = 0; k < n; k++) r[k] = Math.Min(r[k], r[(k + n - 1) % n] + taper);
            for (var k = n - 1; k >= 0; k--) r[k] = Math.Min(r[k], r[(k + 1) % n] + taper);
        }

        // Round: three passes, each computed from the previous pass's values.
        for (var pass = 0; pass < 3; pass++)
        {
            var prev = (double[])r.Clone();
            for (var k = 0; k < n; k++)
                r[k] = Math.Min(prev[k], (prev[(k + n - 1) % n] + 2 * prev[k] + prev[(k + 1) % n]) / 4);
        }

        var area = Area(r);
        var size = 2 * Math.Sqrt(area / Math.PI);
        if (size < minSize || r.Min() < 0.2 * radius) return null;
        if (size > maxSize)
        {
            var scale = maxSize / size;
            for (var k = 0; k < n; k++) r[k] *= scale;
            area = Area(r);
        }

        return new LakeShape
        {
            Patch = patch,
            X = centre.X,
            Y = centre.Y,
            NominalRadius = radius,
            Rays = r,
            AreaM2 = area,
        };
    }

    private static double Area(double[] r)
    {
        var sum = 0.0;
        for (var k = 0; k < r.Length; k++) sum += r[k] * r[(k + 1) % r.Length];
        return 0.5 * LakeShape.SinStep * sum;
    }

    private static bool EdgeCrossesIneligible(
        WorldPoint centre, double[] r, int k, int k1, double step, Func<double, double, bool> eligible)
    {
        var x0 = centre.X + LakeShape.DirX[k] * r[k];
        var y0 = centre.Y + LakeShape.DirY[k] * r[k];
        var x1 = centre.X + LakeShape.DirX[k1] * r[k1];
        var y1 = centre.Y + LakeShape.DirY[k1] * r[k1];
        var length = Math.Sqrt((x1 - x0) * (x1 - x0) + (y1 - y0) * (y1 - y0));
        var parts = (int)Math.Ceiling(length / step);
        for (var s = 1; s < parts; s++)
        {
            var f = (double)s / parts;
            if (!eligible(x0 + (x1 - x0) * f, y0 + (y1 - y0) * f)) return true;
        }
        return false;
    }
}
