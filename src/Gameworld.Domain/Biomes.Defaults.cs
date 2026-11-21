namespace Gameworld.Domain;

// The default list of biome texts: one entry per biome, in the order of the Biome enum. The
// description of a biome is the real land it stands for, from the column "Stands for" of
// docs/research/flora-and-fauna.md section 2. The Volcano is not in that table: its description
// is written here, and so is the lore of every biome: what the peoples of docs/races-basis.md
// make of the land.

public sealed partial record BiomeTextParameters
{
    /// <summary>The default list: the 17 biomes.</summary>
    public static IReadOnlyList<BiomeTextParameters> Defaults { get; } =
    [
        B(Biome.IceAndSnow, "Ice & snow",
            "Ice caps and polar barrens: the high Arctic, Greenland, the Antarctic coast.",
            "The Hoarkin gave up their own warmth to live here, and sleep through the dark half of the year. Everyone else holds that the ice is where the world was left unfinished."),
        B(Biome.Tundra, "Tundra",
            "Arctic tundra, and alpine tundra above the tree line.",
            "A land with no trees to hide in and no night in summer. The herders follow the reindeer across it, and the dead of the Dawn lie whole in its frozen ground."),
        B(Biome.ColdSteppe, "Cold steppe",
            "The Mongolian and Kazakh steppe, the high plateau steppe, Patagonia.",
            "The sea of grass. The Grass riders hold that it belongs to no one and that walls are an insult to it. Every few generations it empties itself onto the settled lands."),
        B(Biome.BorealForest, "Boreal forest",
            "The taiga of Siberia, Scandinavia and Canada.",
            "The Northmen farm its edge and do not go far in. It is a week of the same tree in every direction, and what lives in the middle has no dealings with people."),
        B(Biome.ColdDesert, "Cold desert",
            "The Gobi, the Taklamakan, the Great Basin.",
            "The caravans cross it from well to well, and a well is worth a kingdom. Buried cities of the Old Realms show in the sand after a storm, and are gone after the next."),
        B(Biome.TemperateGrassland, "Temperate grassland",
            "Prairie, the Pontic steppe, the pampas. Most of it is farmland where people live.",
            "The bread land. Most of it is under the plough, and every army that ever marched came across it. The barrows of forgotten kings are the only hills."),
        B(Biome.WoodlandAndShrubland, "Woodland & shrubland",
            "Mediterranean woodland and scrub, chaparral. At its cool end an open oak, pine and juniper woodland.",
            "The land of the olive, the vine and the goat. The Old Realms were richest here and cut the forest for their fleets. The goats have seen to it that it did not come back."),
        B(Biome.TemperateMixedForest, "Temperate mixed forest",
            "The conifer and hardwood forests of the Baltic, New England and Manchuria.",
            "Half cleared and half not: the Midlanders call the cleared half the kingdom and the rest the wood. Charcoal burners, outlaws and hermits live in the rest."),
        B(Biome.TemperateBroadleafForest, "Temperate broadleaf forest",
            "The oak and beech forests of western Europe, and the broadleaf forests of eastern China and Japan.",
            "The old forest of the Elves, who garden it and are losing it, an acre at a time, to the plough. The oldest oaks were saplings in the Dawn."),
        B(Biome.TemperateRainForest, "Temperate rain forest",
            "The Pacific Northwest, Valdivia, New Zealand, the Atlantic oakwoods.",
            "It rains two days in three and moss covers whatever stands still. The Amberbloods end their lives here, standing as trees, and the groves are counted as graveyards."),
        B(Biome.HotDesert, "Hot desert",
            "The Sahara, Arabia, the Sonoran desert.",
            "The Duskreaders cross it by night and the Saltroad folk from well to well. The sorcerer-kings are said to have made it, out of a land that had displeased them."),
        B(Biome.ThornScrub, "Thorn scrub",
            "The Sahel, the Horn of Africa, the Caatinga, the Chaco, the Indian thorn forest.",
            "Everything here has a thorn or a sting, and every tree is someone's. The herders fold their cattle in rings of cut thorn, and the lions test them nightly."),
        B(Biome.Savanna, "Savanna",
            "The African and Indian savannas.",
            "The long grass of the Sunlanders, their cattle and their kings. It burns each dry season and is green a week after the rain, and the herds of game follow the green."),
        B(Biome.TropicalDryForest, "Tropical dry forest",
            "The monsoon forests of India and Indochina (teak and sal), the dry forests of Central America.",
            "The Rainkeepers' land of two seasons: half the year the trees are bare and the reservoirs sink, and then the monsoon breaks and everything is forgiven."),
        B(Biome.TropicalRainForest, "Tropical rain forest",
            "The Amazon, the Congo, the Malay archipelago.",
            "The Boughwalkers live in its crown and seldom touch the ground. Below, it is dusk at noon. Outsiders die of its fevers, and so it has never been conquered."),
        B(Biome.Mangrove, "Mangrove",
            "The Sundarbans and other mangrove coasts.",
            "Neither land nor sea, and under the law of neither. The Shoalfolk live on stilts at its edge, and the Bitterbloods fled into it to be let alone."),
        B(Biome.Volcano, "Volcano",
            "A lone fire mountain: a steep, dark cone with a smoking crater, black lava fields and grey ash on its flanks, and hot springs and yellow sulphur vents at its foot. Little grows on it but birch, heather and fireweed. Etna, Hekla, Fuji.",
            "Every people says a smith or a dragon lives under it, and the Dwarves do not contradict either. The soil at its foot is the richest there is, so it is farmed between its tempers."),
    ];
}
