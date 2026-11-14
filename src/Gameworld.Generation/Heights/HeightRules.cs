using Gameworld.Domain;
using Gameworld.Generation.Basics;
using Gameworld.Generation.Stages;

namespace Gameworld.Generation.Heights;

/// <summary>
/// The floodplain half-width of a river segment (the table under map-generation.md section 12):
/// a monotone spline over the bed height, scaled with the river size and never narrower than
/// 0.75 raster cells.
/// </summary>
public sealed class Floodplain
{
    private readonly MonotoneSpline _spline;
    private readonly bool _scaleWithRiverSize;

    public Floodplain(HeightMapParameters p)
    {
        _spline = new MonotoneSpline(p.Floodplain.Points.Select(q => (q.BedHeightM, q.HalfWidthM)));
        _scaleWithRiverSize = p.Floodplain.ScaleWithRiverSize;
        MinHalfWidthM = 0.75 * p.ResolutionM;
    }

    /// <summary>0.75 × the raster cell size.</summary>
    public double MinHalfWidthM { get; }

    /// <summary>The half-width at a bed height for a flow. With "scale with river size" the spline is the half-width at a flow of 100.</summary>
    public double HalfWidth(double bedHeightM, double flow)
    {
        var w = _spline.Evaluate(bedHeightM);
        if (_scaleWithRiverSize) w *= Math.Sqrt(Math.Max(0, flow) / 100);
        return Math.Max(w, MinHalfWidthM);
    }
}

/// <summary>
/// The pieces of the height map (map-generation.md section 12) that other layers reuse: the
/// relief field, the per-type constants, the simple river bed profile and the floodplain width.
/// </summary>
public static class HeightRules
{
    /// <summary>The feature size of the relief field (12.1).</summary>
    public const double ReliefScaleM = 220_000;

    /// <summary>The noise amplitude of an elevation type in metres (the table under section 12).</summary>
    public static double NoiseAmplitudeM(ElevationType type) => type switch
    {
        ElevationType.FlatLand => 8,
        ElevationType.Lowland => 25,
        ElevationType.Hills => 80,
        ElevationType.Mountains => 180,
        _ => 0,
    };

    /// <summary>The ridged share of an elevation type's surface noise (the table under section 12).</summary>
    public static double RidgedShare(ElevationType type) => type switch
    {
        ElevationType.Lowland => 0.15,
        ElevationType.Hills or ElevationType.Mountains => 1,
        _ => 0,
    };

    /// <summary>seedR = hash4(seed, 0x7e11, 0, 0).</summary>
    public static uint ReliefSeed(World world) => Hashing.Hash4(world.Foundation.Seed.Value, 0x7e11, 0, 0);

    /// <summary>
    /// The relief field of 12.1 ("mountain belts"): a slow noise that makes groups of
    /// neighbouring zones rugged or gentle together. Never below 0.25.
    /// </summary>
    public static double Relief(World world, double x, double y) =>
        Relief(ReliefSeed(world), world.Parameters.Elevation.HeightMap.MountainBelts, x, y);

    internal static double Relief(uint seedR, double strength, double x, double y) =>
        Math.Max(0.25, 1 + strength * 1.6 * Noise.Fbm(seedR, x / ReliefScaleM, y / ReliefScaleM, 3, 2, 0.5));

    /// <summary>The unblurred hillslope at a point: the type's hillslope × the hillslope multiplier × the relief (12.2, 16.1).</summary>
    public static double TypeHillslope(World world, ElevationType type, double x, double y)
    {
        var p = world.Parameters.Elevation.HeightMap;
        return type == ElevationType.Ocean ? 0 : p.Hillslope[type] * p.HillslopeMultiplier * Relief(world, x, y);
    }

    /// <summary>The floodplain half-width function of a world's Parameter Set.</summary>
    public static Floodplain FloodplainOf(World world) => new(world.Parameters.Elevation.HeightMap);

    /// <summary>
    /// flow^−θ without pow (18.3): built from square roots and multiplication when θ is a multiple
    /// of 1/16 (the default is 7/16), from the deterministic exp and ln otherwise.
    /// </summary>
    public static double FlowFactor(double flow, double theta)
    {
        // A flow of 0 (a node that reached no attractor) gives an infinite channel slope: the hillslope then limits the rise (12.2, the min).
        if (!(flow > 0)) return theta > 0 ? double.PositiveInfinity : 1;
        var sixteenths = theta * 16;
        var m = Math.Round(sixteenths);
        if (theta >= 0 && m <= 16 * 64 && Math.Abs(sixteenths - m) < 1e-9)
        {
            var n = (int)m;
            var s2 = Math.Sqrt(flow);
            var s4 = Math.Sqrt(s2);
            var s8 = Math.Sqrt(s4);
            var s16 = Math.Sqrt(s8);
            var power = 1.0;
            for (var whole = n >> 4; whole > 0; whole--) power *= flow;
            if ((n & 8) != 0) power *= s2;
            if ((n & 4) != 0) power *= s4;
            if ((n & 2) != 0) power *= s8;
            if ((n & 1) != 0) power *= s16;
            return 1 / power;
        }
        return DetMath.Exp(-theta * DetMath.Ln(flow));
    }

    /// <summary>
    /// A bed height per river node with the rise of 12.2, using the unblurred type hillslope ×
    /// hillslope multiplier × relief at the node and none of the "no river above its land" caps.
    /// With <paramref name="levelling"/> the low-gradient zones are levelled too: that is what
    /// 12.2 prescribes for the beds of neighbouring regions. Without it only the plain rise is
    /// applied, which is what the travel cost grid uses (16.1). Roots and nodes over the sea are at 0.
    /// </summary>
    public static double[] SimpleBeds(World world, DetailWork detail, bool levelling)
    {
        var profile = new BedProfile(world, detail);
        profile.UseSlopes(null);
        if (levelling) profile.Level();
        else profile.Solve();
        return profile.Z;
    }
}
