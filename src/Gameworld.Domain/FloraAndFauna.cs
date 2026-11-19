using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Gameworld.Domain;

/// <summary>The types of flora.</summary>
public enum FloraType
{
    /// <summary>Woody plants of tree height, palms included.</summary>
    Tree,
    /// <summary>Woody or succulent plants below tree height.</summary>
    Shrub,
    /// <summary>Plants known for the bloom.</summary>
    Flower,
    /// <summary>Soft plants gathered or grown for medicine, poison, dye, fibre or fodder.</summary>
    Herb,
    /// <summary>Plants known for an edible fruit, nut or berry.</summary>
    Fruit,
    /// <summary>Grasses, sedges, reeds and bamboo.</summary>
    Grass,
    /// <summary>Climbers and trailers.</summary>
    Vine,
    /// <summary>Mushrooms and the fungi of grain and wood.</summary>
    Fungus,
    /// <summary>Mosses, lichens, ferns and algae.</summary>
    Moss,
    /// <summary>Plants of fresh water and of the sea.</summary>
    Aquatic,
    /// <summary>Plants and fungi with a supernatural property. The counterpart of a monster.</summary>
    Arcane,
}

/// <summary>The types of fauna.</summary>
public enum FaunaType
{
    /// <summary>Small animals: harmless, a nuisance, or useful in a small way.</summary>
    Critter,
    /// <summary>Ordinary wildlife and livestock, fish included.</summary>
    Animal,
    /// <summary>Large or dangerous natural creatures.</summary>
    Beast,
    /// <summary>Supernatural or legendary creatures.</summary>
    Monster,
}

/// <summary>
/// A kind of flora: a plant that can grow in the world. The flora are a list in the Parameter
/// Set, so a kind is added, changed or switched off without touching the generation code.
/// </summary>
[Description("A kind of flora: a plant that can grow in the world, and the biomes it grows in.")]
public sealed partial record FloraParameters(
    [property: Description("The id the generated data knows the kind by. Unique in the list; keep it when the name changes.")] string Id,
    [property: Description("The name shown for the kind.")] string Name,
    [property: Description("The type of the kind: Tree, Shrub, Flower, Herb, Fruit, Grass, Vine, Fungus, Moss, Aquatic or Arcane.")] FloraType Type,
    [property: Description("Whether the kind grows in the world. A kind that is off is never drawn for a patch.")] bool Enabled,
    [property: Description("The biomes the kind grows in. A land patch can only get a kind that lists its biome (§13.4).")] IReadOnlyList<Biome> Biomes)
{
    [Description("Whether the kind grows in the sea: an ocean patch can only get a kind that is of the sea (§13.4).")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Sea { get; init; }

    [Description("How frequent the kind is, relative to the other kinds that fit a patch. A kind with twice the frequency is drawn about twice as often. 0 or below means never (§13.4).")]
    public double Frequency { get; init; } = 1;

    [Description("The Supernatural (0 to 100) a patch needs for the kind to grow there. 0 means anywhere. The arcane plants of the defaults need 60, 75 or 90 (§13.4).")]
    [Range(0, 100)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public double MinSupernatural { get; init; }

    /// <summary>A kind from the biome codes of its entry. An arcane plant is rare beside the ordinary plants.</summary>
    private static FloraParameters Make(string id, string name, FloraType type, string habitat, double minSupernatural = 0) =>
        new(id, name, type, true, BiomeCodes.Biomes(habitat))
        {
            Sea = BiomeCodes.Sea(habitat),
            Frequency = type == FloraType.Arcane ? 0.25 : 1,
            MinSupernatural = minSupernatural,
        };
}

/// <summary>
/// A kind of fauna: a creature that can live in the world. The fauna are a list in the
/// Parameter Set, like the flora.
/// </summary>
[Description("A kind of fauna: a creature that can live in the world, and the biomes it lives in.")]
public sealed partial record FaunaParameters(
    [property: Description("The id the generated data knows the kind by. Unique in the list; keep it when the name changes.")] string Id,
    [property: Description("The name shown for the kind.")] string Name,
    [property: Description("The type of the kind: Critter (small), Animal (ordinary wildlife and livestock), Beast (large or dangerous) or Monster (supernatural or legendary).")] FaunaType Type,
    [property: Description("Whether the kind lives in the world. A kind that is off is never drawn for a patch.")] bool Enabled,
    [property: Description("The biomes the kind lives in. A land patch can only get a kind that lists its biome (§13.4).")] IReadOnlyList<Biome> Biomes)
{
    [Description("Whether the kind lives in the sea: an ocean patch can only get a kind that is of the sea (§13.4).")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Sea { get; init; }

    [Description("How frequent the kind is, relative to the other kinds that fit a patch. A kind with twice the frequency is drawn about twice as often. 0 or below means never (§13.4).")]
    public double Frequency { get; init; } = 1;

    [Description("The Supernatural (0 to 100) a patch needs for the kind to live there. 0 means anywhere. The monsters of the defaults need 60, 75 or 90 (§13.4).")]
    [Range(0, 100)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public double MinSupernatural { get; init; }

    /// <summary>A kind from the biome codes of its entry. A beast is less frequent than the smaller animals, and a monster is rare.</summary>
    private static FaunaParameters Make(string id, string name, FaunaType type, string habitat, double minSupernatural = 0) =>
        new(id, name, type, true, BiomeCodes.Biomes(habitat))
        {
            Sea = BiomeCodes.Sea(habitat),
            Frequency = type switch
            {
                FaunaType.Beast => 0.5,
                FaunaType.Monster => 0.25,
                _ => 1,
            },
            MinSupernatural = minSupernatural,
        };
}

/// <summary>The biome codes of docs/society-generation.md Appendix A, and Sea for the ocean patches.</summary>
internal static class BiomeCodes
{
    private const string SeaCode = "Sea";

    private static readonly Dictionary<string, Biome> ByCode = new()
    {
        ["Ice"] = Biome.IceAndSnow,
        ["Tun"] = Biome.Tundra,
        ["CSt"] = Biome.ColdSteppe,
        ["Bor"] = Biome.BorealForest,
        ["CDs"] = Biome.ColdDesert,
        ["TGr"] = Biome.TemperateGrassland,
        ["WSh"] = Biome.WoodlandAndShrubland,
        ["TMx"] = Biome.TemperateMixedForest,
        ["TBl"] = Biome.TemperateBroadleafForest,
        ["TRn"] = Biome.TemperateRainForest,
        ["HDs"] = Biome.HotDesert,
        ["Thn"] = Biome.ThornScrub,
        ["Sav"] = Biome.Savanna,
        ["TDf"] = Biome.TropicalDryForest,
        ["TRf"] = Biome.TropicalRainForest,
        ["Mgv"] = Biome.Mangrove,
    };

    /// <summary>The biomes of a list of codes separated by spaces. The code Sea is not a biome and is skipped.</summary>
    public static IReadOnlyList<Biome> Biomes(string codes) =>
        codes.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Where(c => c != SeaCode)
            .Select(c => ByCode.TryGetValue(c, out var biome) ? biome : throw new ArgumentException($"Unknown biome code {c}.", nameof(codes)))
            .ToArray();

    public static bool Sea(string codes) =>
        codes.Split(' ', StringSplitOptions.RemoveEmptyEntries).Contains(SeaCode);
}
