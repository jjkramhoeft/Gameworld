using Gameworld.Domain;

namespace Gameworld.Generation.Resources;

/// <summary>One row of the factors by biome (map-generation.md Appendix C).</summary>
/// <param name="Soil">Soil quality for field crops (14.2).</param>
/// <param name="TreeGround">Ground quality for tree crops (14.2).</param>
/// <param name="Pasture">Livestock potential (D.1).</param>
/// <param name="Game">Wild food potential (D.1).</param>
/// <param name="Wood">Timber potential (D.1).</param>
/// <param name="Fur">Furs and ivory potential (D.1).</param>
/// <param name="Mounts">Horses and pack animals potential (D.1).</param>
/// <param name="Honey">Honey potential, a stimulant (D.1).</param>
/// <param name="Herb">Herbs potential (D.1).</param>
/// <param name="Veg">The travel slowdown of the central-place graph (15.2).</param>
/// <param name="RoadCost">The biome factor of the travel grid (16.1).</param>
public readonly record struct BiomeFactorRow(
    double Soil,
    double TreeGround,
    double Pasture,
    double Game,
    double Wood,
    double Fur,
    double Mounts,
    double Honey,
    double Herb,
    double Veg,
    double RoadCost);

/// <summary>The factors by biome (map-generation.md Appendix C).</summary>
public static class BiomeFactors
{
    /// <summary>The Ocean row.</summary>
    public static BiomeFactorRow Ocean { get; } = new(0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1);

    //                                                       soil  tree  past  game  wood  fur   mount honey herb  veg  road
    private static readonly BiomeFactorRow Ice =         new(0,    0,    0,    0.05, 0,    0,    0.25, 0,    0,    1.5, 5);
    private static readonly BiomeFactorRow Tundra =      new(0.3,  0.2,  0.25, 0.3,  0,    0.5,  0.3,  0,    0.15, 1.1, 1.6);
    private static readonly BiomeFactorRow ColdSteppe =  new(1,    1,    0.7,  0.3,  0.08, 0.15, 0.9,  0.1,  0.3,  1,   1.1);
    private static readonly BiomeFactorRow Boreal =      new(0.55, 1,    0.25, 0.7,  1,    0.85, 0.25, 0.2,  0.4,  1.3, 1.4);
    private static readonly BiomeFactorRow ColdDesert =  new(1,    1,    0.2,  0.1,  0,    0,    0.5,  0,    0.1,  1,   3.5);
    private static readonly BiomeFactorRow Grassland =   new(1,    1,    1,    0.4,  0.1,  0,    0.9,  0.2,  0.4,  1,   1);
    private static readonly BiomeFactorRow Shrubland =   new(1,    1,    0.7,  0.45, 0.4,  0,    0.3,  0.3,  0.8,  1.1, 1.1);
    private static readonly BiomeFactorRow Mixed =       new(1,    1,    0.6,  0.8,  1,    0.3,  0.15, 0.2,  0.6,  1.3, 1.3);
    private static readonly BiomeFactorRow Broadleaf =   new(1,    1,    0.6,  0.8,  1,    0.1,  0.15, 0.3,  0.6,  1.3, 1.3);
    private static readonly BiomeFactorRow TempRain =    new(0.55, 1,    0.5,  0.8,  1,    0.2,  0,    0.15, 0.6,  2,   2);
    private static readonly BiomeFactorRow HotDesert =   new(1,    1,    0.15, 0.05, 0,    0,    0.5,  0,    0.1,  1.1, 4);
    private static readonly BiomeFactorRow ThornScrub =  new(0.7,  1,    0.5,  0.3,  0.25, 0.25, 0.4,  0.15, 0.35, 1.1, 1.5);
    private static readonly BiomeFactorRow Savanna =     new(0.6,  1,    0.8,  0.6,  0.35, 0.4,  0.3,  0.2,  0.4,  1,   1.05);
    private static readonly BiomeFactorRow DryForest =   new(0.7,  1,    0.45, 0.6,  0.8,  0.35, 0.3,  0.2,  0.6,  1.3, 1.3);
    private static readonly BiomeFactorRow RainForest =  new(0.4,  1,    0.15, 0.6,  1,    0.35, 0.25, 0.15, 0.8,  2,   2.2);
    private static readonly BiomeFactorRow Mangrove =    new(0.3,  0.4,  0.1,  0.4,  0.5,  0,    0,    0.1,  0.3,  2,   3);

    /// <summary>The row of a land biome.</summary>
    public static BiomeFactorRow Of(Biome biome) => biome switch
    {
        Biome.IceAndSnow => Ice,
        Biome.Tundra => Tundra,
        Biome.ColdSteppe => ColdSteppe,
        Biome.BorealForest => Boreal,
        Biome.ColdDesert => ColdDesert,
        Biome.TemperateGrassland => Grassland,
        Biome.WoodlandAndShrubland => Shrubland,
        Biome.TemperateMixedForest => Mixed,
        Biome.TemperateBroadleafForest => Broadleaf,
        Biome.TemperateRainForest => TempRain,
        Biome.HotDesert => HotDesert,
        Biome.ThornScrub => ThornScrub,
        Biome.Savanna => Savanna,
        Biome.TropicalDryForest => DryForest,
        Biome.TropicalRainForest => RainForest,
        Biome.Mangrove => Mangrove,
        _ => throw new ArgumentOutOfRangeException(nameof(biome)),
    };
}
