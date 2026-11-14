using Gameworld.Domain;
using Gameworld.Generation.Basics;

namespace Gameworld.Generation.Biomes;

/// <summary>The moisture classes of the biome table, by aridity m = P / P_th.</summary>
public enum MoistureClass
{
    /// <summary>m &lt; 0.5.</summary>
    Desert,
    /// <summary>0.5 ≤ m &lt; 1.</summary>
    Steppe,
    /// <summary>1 ≤ m &lt; 2.</summary>
    SubHumid,
    /// <summary>2 ≤ m &lt; wet bound.</summary>
    Humid,
    /// <summary>m ≥ wet bound (6 temperate, 4 tropical).</summary>
    Wet,
}

/// <summary>Biomes (map-generation.md section 13): one biome per land patch from temperature and aridity.</summary>
public sealed class BiomeRules
{
    private readonly MonotoneSpline _cooling;
    private readonly BiomeParameters _p;

    public BiomeRules(BiomeParameters parameters)
    {
        _p = parameters;
        _cooling = new MonotoneSpline(
            parameters.Cooling.Select(c => (c.HeightM, c.CoolingC)),
            extendLastSlope: true);
    }

    /// <summary>The typical height per elevation type, used by the layers that must not depend on the height map.</summary>
    public static double TypicalHeightM(ElevationType type) => type switch
    {
        ElevationType.FlatLand => 70,
        ElevationType.Lowland => 140,
        ElevationType.Hills => 270,
        ElevationType.Mountains => 470,
        _ => 0,
    };

    /// <summary>The cooling in °C at a height: constant below the first point, along the last segment's slope above the last.</summary>
    public double Cooling(double heightM) => _cooling.Evaluate(Math.Max(0, heightM));

    /// <summary>Precipitation relative to Köppen's dryness threshold, which grows with temperature.</summary>
    public static double Aridity(double temperatureC, double precipitationMm) =>
        precipitationMm / Math.Max(40, 20 * temperatureC + 140);

    public static ThermalBelt BeltOf(double temperatureC) => temperatureC switch
    {
        < -10 => ThermalBelt.Polar,
        < -3 => ThermalBelt.Subpolar,
        < 3 => ThermalBelt.Boreal,
        < 18 => ThermalBelt.Temperate,
        _ => ThermalBelt.Tropical,
    };

    public static MoistureClass MoistureOf(ThermalBelt belt, double aridity)
    {
        if (aridity < 0.5) return MoistureClass.Desert;
        if (aridity < 1) return MoistureClass.Steppe;
        if (aridity < 2) return MoistureClass.SubHumid;
        var wetBound = belt == ThermalBelt.Tropical ? 4 : 6;
        return aridity < wetBound ? MoistureClass.Humid : MoistureClass.Wet;
    }

    /// <summary>
    /// The biome of a land patch.
    /// </summary>
    /// <param name="seaLevelTemperatureC">The Initial Temperature field at the patch's site.</param>
    /// <param name="fieldPrecipitationMm">The Precipitation field at the patch's site.</param>
    /// <param name="heightM">The mean height of the patch's land, or the typical height of its elevation type.</param>
    /// <param name="zoneType">The elevation type of the patch's zone.</param>
    /// <param name="coastal">One of the patch's cells has an ocean patch cell as neighbour.</param>
    /// <param name="riverPoor">The patch lies in a river-poor flat zone.</param>
    public BiomeRecord Classify(
        double seaLevelTemperatureC,
        double fieldPrecipitationMm,
        double heightM,
        ElevationType zoneType,
        bool coastal,
        bool riverPoor)
    {
        var t = seaLevelTemperatureC - Cooling(heightM);
        var p = riverPoor ? fieldPrecipitationMm * _p.RiverPoorPrecipitationFactor : fieldPrecipitationMm;
        var m = Aridity(t, p);
        var belt = BeltOf(t);
        var biome = Table(belt, MoistureOf(belt, m), t);

        // Mangrove overrides the table on warm, wet and low coasts.
        if (coastal
            && zoneType is ElevationType.FlatLand or ElevationType.Lowland
            && t >= _p.MangroveMinTemperatureC
            && m >= _p.MangroveMinAridity)
            biome = Biome.Mangrove;

        return new BiomeRecord(biome, belt, t, seaLevelTemperatureC, p, m);
    }

    private static Biome Table(ThermalBelt belt, MoistureClass moisture, double t) => belt switch
    {
        ThermalBelt.Polar => Biome.IceAndSnow,
        ThermalBelt.Subpolar => Biome.Tundra,
        ThermalBelt.Boreal => moisture switch
        {
            MoistureClass.Desert => Biome.ColdDesert,
            MoistureClass.Steppe => Biome.ColdSteppe,
            _ => Biome.BorealForest,
        },
        ThermalBelt.Temperate => moisture switch
        {
            MoistureClass.Desert => Biome.ColdDesert,
            MoistureClass.Steppe => Biome.TemperateGrassland,
            MoistureClass.SubHumid => Biome.WoodlandAndShrubland,
            MoistureClass.Humid => t < 10 ? Biome.TemperateMixedForest : Biome.TemperateBroadleafForest,
            _ => Biome.TemperateRainForest,
        },
        _ => moisture switch
        {
            MoistureClass.Desert => Biome.HotDesert,
            MoistureClass.Steppe => Biome.ThornScrub,
            MoistureClass.SubHumid => Biome.Savanna,
            MoistureClass.Humid => Biome.TropicalDryForest,
            _ => Biome.TropicalRainForest,
        },
    };

    /// <summary>The map colour of a biome (section 13.3).</summary>
    public static (byte R, byte G, byte B) Colour(Biome biome) => biome switch
    {
        Biome.IceAndSnow => (234, 239, 243),
        Biome.Tundra => (146, 154, 138),
        Biome.ColdSteppe => (178, 171, 132),
        Biome.BorealForest => (56, 94, 78),
        Biome.ColdDesert => (204, 189, 152),
        Biome.TemperateGrassland => (186, 186, 112),
        Biome.WoodlandAndShrubland => (144, 152, 90),
        Biome.TemperateMixedForest => (68, 114, 72),
        Biome.TemperateBroadleafForest => (90, 140, 64),
        Biome.TemperateRainForest => (40, 102, 80),
        Biome.HotDesert => (229, 200, 138),
        Biome.ThornScrub => (198, 166, 100),
        Biome.Savanna => (190, 176, 80),
        Biome.TropicalDryForest => (122, 144, 58),
        Biome.TropicalRainForest => (28, 94, 44),
        Biome.Mangrove => (66, 132, 118),
        _ => (38, 84, 132),
    };
}
