namespace Gameworld.Domain;

// The default list of biome texts: one entry per biome, in the order of the Biome enum. The
// description of a biome is the real land it stands for, from the column "Stands for" of
// docs/research/flora-and-fauna.md section 2. The Volcano is not in that table, and no biome has
// a lore yet: give B a description and a lore to fill them in.

public sealed partial record BiomeTextParameters
{
    /// <summary>The default list: the 17 biomes.</summary>
    public static IReadOnlyList<BiomeTextParameters> Defaults { get; } =
    [
        B(Biome.IceAndSnow, "Ice & snow",
            "Ice caps and polar barrens: the high Arctic, Greenland, the Antarctic coast."),
        B(Biome.Tundra, "Tundra",
            "Arctic tundra, and alpine tundra above the tree line."),
        B(Biome.ColdSteppe, "Cold steppe",
            "The Mongolian and Kazakh steppe, the high plateau steppe, Patagonia."),
        B(Biome.BorealForest, "Boreal forest",
            "The taiga of Siberia, Scandinavia and Canada."),
        B(Biome.ColdDesert, "Cold desert",
            "The Gobi, the Taklamakan, the Great Basin."),
        B(Biome.TemperateGrassland, "Temperate grassland",
            "Prairie, the Pontic steppe, the pampas. Most of it is farmland where people live."),
        B(Biome.WoodlandAndShrubland, "Woodland & shrubland",
            "Mediterranean woodland and scrub, chaparral. At its cool end an open oak, pine and juniper woodland."),
        B(Biome.TemperateMixedForest, "Temperate mixed forest",
            "The conifer and hardwood forests of the Baltic, New England and Manchuria."),
        B(Biome.TemperateBroadleafForest, "Temperate broadleaf forest",
            "The oak and beech forests of western Europe, and the broadleaf forests of eastern China and Japan."),
        B(Biome.TemperateRainForest, "Temperate rain forest",
            "The Pacific Northwest, Valdivia, New Zealand, the Atlantic oakwoods."),
        B(Biome.HotDesert, "Hot desert",
            "The Sahara, Arabia, the Sonoran desert."),
        B(Biome.ThornScrub, "Thorn scrub",
            "The Sahel, the Horn of Africa, the Caatinga, the Chaco, the Indian thorn forest."),
        B(Biome.Savanna, "Savanna",
            "The African and Indian savannas."),
        B(Biome.TropicalDryForest, "Tropical dry forest",
            "The monsoon forests of India and Indochina (teak and sal), the dry forests of Central America."),
        B(Biome.TropicalRainForest, "Tropical rain forest",
            "The Amazon, the Congo, the Malay archipelago."),
        B(Biome.Mangrove, "Mangrove",
            "The Sundarbans and other mangrove coasts."),
        B(Biome.Volcano, "Volcano"),
    ];
}
