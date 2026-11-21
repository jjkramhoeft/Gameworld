using static Gameworld.Domain.AccessoryKind;
using static Gameworld.Domain.Sex;
using static Gameworld.Domain.WealthLevel;
using static Gameworld.Domain.Worn;

namespace Gameworld.Domain;

// The default lists of the portrait components (society-generation.md section 9): the entries
// of docs/research/portrait-components.md that a race of the default list uses, in the order
// of that note, with its names in sentence case. Who wears a hairstyle, an outfit or an
// accessory follows the column "Worn by" of the note and its tables by race (sections 9.3 and
// 11.2). It is the judgement of society-generation.md section 9: starting values, not tuned.

/// <summary>Who wears what, in the short form the default lists are written in.</summary>
internal static class Worn
{
    /// <summary>The races that dress as the Midlanders do: the Midlanders and the races whose entry in races-basis.md names no dress of its own.</summary>
    internal static readonly string[] Midland = ["midlanders", "flinchless", "stillfaces", "amberbloods"];

    /// <summary>The races that wear the dress of a trade of the ploughed lands: the habit, the apron, the mail.</summary>
    internal static readonly string[] Trades = ["northmen", "midlanders", "flinchless", "stillfaces", "amberbloods", "swiftborn"];

    /// <summary>The races whose entry in races-basis.md names no hairstyle: they wear the plain ones.</summary>
    internal static readonly string[] PlainHair = ["amberbloods", "brinefolk", "duskreaders", "flinchless", "lanternkin", "stillfaces", "catfolk"];

    /// <summary>The races whose destitute wear rags: those whose dress is not the mark of the race.</summary>
    internal static readonly string[] Ragged =
    [
        "northmen", "midlanders", "saltroadFolk", "sunlanders", "grassRiders", "rainkeepers", "canalFolk",
        "halfling", "gnome", "orc", "amberbloods", "swiftborn", "flinchless", "stillfaces", "catfolk",
    ];

    internal static WearerParameters By(
        Sex? sex = null, string[]? races = null, WealthLevel from = Destitute, WealthLevel to = Rich,
        string[]? vocations = null, int? fromAge = null, int? toAge = null, bool? married = null) => new()
        {
            Sex = sex,
            Races = races ?? [],
            LowestWealth = from,
            HighestWealth = to,
            Vocations = vocations ?? [],
            LowestAge = fromAge,
            HighestAge = toAge,
            Married = married,
        };
}

public sealed partial record SkinColourParameters
{
    /// <summary>The default list: 32 skin colours, twelve human tones and twenty of the other races.</summary>
    public static IReadOnlyList<SkinColourParameters> Defaults { get; } =
    [
        // The human tones, from light to dark
        new("veryFairCool", "Very fair, cool", "very fair skin with a cool pink undertone, lightly freckled"),
        new("fairNeutral", "Fair, neutral", "fair skin with a neutral ivory undertone"),
        new("ruddyFair", "Ruddy fair", "fair skin with a ruddy, wind-reddened flush across the cheeks and nose"),
        new("lightBeigeWarm", "Light beige, warm", "light beige skin with a warm yellow undertone"),
        new("olive", "Olive", "light olive skin, a light brown with a muted green-grey undertone"),
        new("goldenTan", "Golden tan", "golden-tan skin with a warm yellow-gold undertone"),
        new("weatheredBronze", "Weathered bronze", "sun-weathered bronze skin with a warm red-brown cast, creased at the eyes"),
        new("lightBrownGolden", "Light brown, golden", "light brown skin with a warm golden undertone"),
        new("redBrown", "Red-brown", "medium red-brown skin with a warm copper undertone"),
        new("mediumBrownNeutral", "Medium brown, neutral", "medium brown skin with a neutral undertone"),
        new("darkBrownWarm", "Dark brown, warm", "dark brown skin with a warm red undertone"),
        new("deepBrownCool", "Deep brown, cool", "very deep brown skin, near black, with a cool blue sheen in the highlights"),
        // The tones of single races
        new("deepRedBrown", "Deep red-brown", "deep red-brown skin with a warm copper undertone"),
        new("deepOlive", "Deep olive", "deep olive skin, a medium brown with a muted green undertone"),
        new("seamedRuddy", "Seamed ruddy", "ruddy, wind-reddened skin, thick and deeply creased like worked leather"),
        new("seamedBrown", "Seamed brown", "medium brown skin, thick and deeply creased like worked leather"),
        new("greyGreen", "Grey-green", "dull grey-green skin, the colour of weathered slate with a moss cast"),
        new("dunBrown", "Dun brown", "dull grey-brown skin with no warmth in it"),
        new("frostPale", "Frost pale", "very pale white skin with a cold cast, shading to blue-grey at the lips, the eyelids, the nails and the tips of the ears"),
        new("flushedCopper", "Flushed copper", "flushed copper-red skin, as if just in from a hard run"),
        new("tawny", "Tawny", "tawny skin, a light yellow-brown"),
        new("resinFreckledGold", "Resin-freckled gold", "brown-gold skin scattered with small freckles the colour of clear amber resin"),
        new("shellGreyWhite", "Shell grey-white", "smooth grey-white skin with the faint pearly sheen of the inside of a shell"),
        new("deepBrownGrey", "Deep brown, grey", "deep brown skin with a cool grey undertone"),
        new("copperBrownGreenVeined", "Copper-brown, green-veined", "copper-brown skin with a fine tracery of green-bronze lines, like the veins of a leaf, across the shoulders and upper arms"),
        new("deepOliveGreenVeined", "Deep olive, green-veined", "deep olive skin with a fine tracery of green-bronze lines, like the veins of a leaf, across the shoulders and upper arms"),
        new("greyCastBrown", "Grey-cast brown", "smooth medium brown skin with a grey cast and a faint oiled sheen"),
        new("greenCastBrown", "Green-cast brown", "smooth medium brown skin with a green cast and a faint oiled sheen"),
        new("clearPale", "Clear pale", "very pale, thin skin that seems almost clear, with a faint blue-green tracery of veins at the wrists, the throat and the temples"),
        new("tawnyGrey", "Tawny-grey", "tawny-grey skin, a light yellow-brown dulled with grey"),
        new("oliveBrownBrightPatched", "Olive-brown, bright-patched", "dark olive-brown skin broken by patches of bright colour, sky blue and sulphur yellow, across the throat, the shoulders and the outer arms"),
        new("smokeGrey", "Smoke grey", "smooth, even grey skin with a cool cast, the colour of wood smoke"),
    ];
}

public sealed partial record EyeColourParameters
{
    /// <summary>The default list: 27 eye colours, eight human ones and nineteen of the other races.</summary>
    public static IReadOnlyList<EyeColourParameters> Defaults { get; } =
    [
        // The human colours
        new("blue", "Blue", "clear blue eyes"),
        new("grey", "Grey", "clear grey eyes"),
        new("green", "Green", "green eyes"),
        new("hazelGreen", "Hazel-green", "hazel-green eyes, green at the rim and brown around the pupil"),
        new("clearBrown", "Clear brown", "clear light brown eyes"),
        new("earthBrown", "Earth brown", "dark brown eyes"),
        new("jet", "Jet", "eyes so dark that iris and pupil read as one black"),
        new("amber", "Amber", "amber eyes, a clear golden brown"),
        // The colours of single races
        new("gold", "Gold", "clear gold eyes"),
        new("yellow", "Yellow", "small yellow eyes"),
        new("rustBrown", "Rust brown", "small red-brown eyes"),
        new("rimmedPaleGrey", "Rimmed pale grey", "pale grey eyes with a dark rim around the iris"),
        new("emberDark", "Ember dark", "dark eyes with a ruddy light in them, like a coal seen through ash"),
        new("barPupilGold", "Bar-pupil gold", "pale gold eyes whose pupil is a dark horizontal bar, level with the ground"),
        new("beadAmber", "Bead amber", "amber-gold eyes that catch the light like a bead of resin held to the sun"),
        new("veryLightGrey", "Very light grey", "very light grey eyes, almost without colour"),
        new("veryLightGreen", "Very light green", "very light green eyes, almost without colour"),
        new("allBlack", "All black", "large eyes that are wholly black from lid to lid, with no white and no iris, glossy as wet stone"),
        new("silverGrey", "Silver-grey", "pale silver-grey eyes"),
        new("lightGreyBlue", "Light grey-blue", "light grey-blue eyes"),
        new("yellowRingedDark", "Yellow-ringed dark", "dark eyes with a narrow yellow ring around the iris"),
        // The Catfolk: the slit pupil is part of the colour
        new("slitPupilAmber", "Slit-pupil amber", "bright amber eyes whose pupil is a narrow upright slit, like a cat's"),
        new("slitPupilGold", "Slit-pupil gold", "bright gold eyes whose pupil is a narrow upright slit, like a cat's"),
        new("slitPupilCopper", "Slit-pupil copper", "bright copper eyes, a red-gold, whose pupil is a narrow upright slit, like a cat's"),
        new("slitPupilGreen", "Slit-pupil green", "bright green eyes whose pupil is a narrow upright slit, like a cat's"),
        new("slitPupilPaleBlue", "Slit-pupil pale blue", "bright pale blue eyes whose pupil is a narrow upright slit, like a cat's"),
        new("oddSlitPupilEyes", "Odd slit-pupil eyes", "one bright amber eye and one bright pale blue eye, each with a pupil that is a narrow upright slit, like a cat's"),
    ];
}

public sealed partial record HairColourParameters
{
    /// <summary>The default list: 23 hair colours. Ten are the base colours, four those of age and nine those of single races.</summary>
    public static IReadOnlyList<HairColourParameters> Defaults { get; } =
    [
        // The base colours
        new("black", "Black", "jet black hair with a faint blue sheen") { Greying = "greyingDark" },
        new("softBlack", "Soft black", "off-black hair that shows dark brown in sunlight") { Greying = "greyingDark" },
        new("darkBrown", "Dark brown", "dark brown hair") { Greying = "greyingDark" },
        new("chestnut", "Chestnut", "chestnut brown hair with a reddish cast") { Greying = "greyingDark" },
        new("lightBrown", "Light brown", "light ash-brown hair") { Greying = "greyingDark" },
        new("sandy", "Sandy", "sandy dark-blond hair") { Greying = "greyingFair" },
        new("goldenBlond", "Golden blond", "golden blond hair") { Greying = "greyingFair" },
        new("flaxen", "Flaxen", "pale flaxen blond hair, the colour of dry straw") { Greying = "greyingFair" },
        new("copperRed", "Copper red", "bright copper-red hair") { Greying = "greyingFair" },
        new("auburn", "Auburn", "dark auburn hair, red-brown") { Greying = "greyingFair" },
        // The colours of age
        new("greyingDark", "Greying dark", "dark hair streaked with grey at the temples and through the length") { Greying = "ironGrey" },
        new("greyingFair", "Greying fair", "faded blond or red hair turning ash-white at the temples") { Greying = "white" },
        new("ironGrey", "Iron grey", "fully iron-grey hair") { Greying = "white" },
        new("white", "White", "thin white hair of old age"),
        // The colours of single races. Not from age: they do not grey.
        new("silverWhite", "Silver-white", "smooth, shining silver-white hair on a young face"),
        new("ashGrey", "Ash grey", "pale ash-grey hair, even in colour from root to tip, on a young face"),
        new("snowWhite", "Snow white", "dense pure white hair with no yellow in it, on an unlined face"),
        new("darkRed", "Dark red", "deep dark red hair, near black in shadow and wine-red in the light") { Greying = "greyingDark" },
        new("barkBrown", "Bark brown", "dull grey-brown hair, the colour of tree bark") { Greying = "greyingDark" },
        new("saltPale", "Salt pale", "pale, stiff hair bleached almost white, with a dull straw cast"),
        new("ringedDarkAndPale", "Ringed dark and pale", "long hair banded along its whole length in rings of dark brown and pale blond, each ring a hand's width"),
        new("brindledBrownGrey", "Brindled brown-grey", "thick hair streaked unevenly in brown and grey"),
        new("patchedTwoColour", "Patched two-colour", "hair of two colours, black broken by uneven patches and streaks of ginger"),
    ];
}

public sealed partial record HairstyleParameters
{
    /// <summary>The default list: 46 hairstyles, all on. Every default race has one for each sex.</summary>
    public static IReadOnlyList<HairstyleParameters> Defaults { get; } =
    [
        // Women
        Make("looseWithCirclet", "Loose with circlet", By(Female, ["midlanders", "flinchless", "elf"], toAge: 30, married: false)),
        Make("singleBackBraid", "Single back braid", By(Female, ["northmen", "midlanders", "grassRiders", "rainkeepers", "swiftborn", .. PlainHair])),
        Make("twinLongPlaits", "Twin long plaits", By(Female, ["midlanders", "flinchless"], from: Comfortable, toAge: 35)),
        Make("crownBraid", "Crown braid", By(Female, ["northmen", "midlanders", "flinchless"], to: Comfortable)),
        Make("knottedTail", "Knotted tail", By(Female, ["northmen"])),
        Make("veilAndWimple", "Veil and wimple", By(Female, ["midlanders", "flinchless"], from: Common, married: true)),
        Make("nettedSideCoils", "Netted side coils", By(Female, ["midlanders", "flinchless"], from: Rich)),
        Make("napeKnot", "Nape knot", By(Female, ["saltroadFolk", .. PlainHair])),
        Make("tieredCurls", "Tiered curls", By(Female, ["saltroadFolk"], from: Rich)),
        Make("bluntBraidedWig", "Blunt braided wig", By(races: ["saltroadFolk"], from: Comfortable)),
        Make("beadedCornrows", "Beaded cornrows", By(races: ["sunlanders", "shoalfolk"])),
        Make("ochrePlaits", "Ochre plaits", By(Female, ["sunlanders"], fromAge: 13)),
        Make("shavedHead", "Shaved head", By(Female, ["saltroadFolk", "sunlanders"])),
        Make("tallHeadwrap", "Tall headwrap", By(Female, ["sunlanders"], married: true)),
        Make("pinnedHighKnot", "Pinned high knot", By(Female, ["canalFolk"])),
        Make("trailingCourtHair", "Trailing court hair", By(Female, ["canalFolk"], from: Rich)),
        Make("floweredBraid", "Flowered braid", By(Female, ["rainkeepers"])),
        Make("wingedHorns", "Winged horns", By(Female, ["grassRiders"], from: Comfortable, married: true)),
        Make("croppedShort", "Cropped short", By(Female, ["hearthbloods", "shoalfolk", "flinchless", "catfolk"])),
        Make("halfUpSideBraids", "Half-up side braids", By(Female, ["elf", "midlanders"], from: Comfortable)),
        // Men
        Make("shortCrop", "Short crop", By(Male, ["midlanders", "saltroadFolk", "hearthbloods", "shoalfolk", .. PlainHair])),
        Make("bowlCrop", "Bowl crop", By(Male, ["midlanders", "flinchless"])),
        Make("chinLengthBob", "Chin-length bob", By(Male, ["midlanders", "flinchless"], from: Common)),
        Make("tonsure", "Tonsure", By(Male, ["midlanders", "flinchless"], vocations: ["priest", "scribe"])),
        Make("longAndLoose", "Long and loose", By(Male, ["northmen", "midlanders", "swiftborn", .. PlainHair])),
        Make("topknot", "Topknot", By(Male, ["canalFolk"])),
        Make("shavedCrownWithSideBraids", "Shaved crown with side braids", By(Male, ["grassRiders"])),
        Make("bareScalp", "Bare scalp", By(Male, ["saltroadFolk", "sunlanders"])),
        Make("warriorBraids", "Warrior braids", By(Male, ["sunlanders"], toAge: 35)),
        Make("oiledRinglets", "Oiled ringlets", By(Male, ["saltroadFolk"], from: Comfortable)),
        // Written for single races
        Make("combedLongWithBraidedBeard", "Combed long with braided beard", By(Male, ["northmen"])),
        Make("patternShavedScalp", "Pattern-shaved scalp", By(races: ["sunlanders"])),
        Make("clayAndBeadCrown", "Clay and bead crown", By(races: ["sunlanders"], from: Comfortable)),
        Make("oiledLongHair", "Oiled long hair", By(races: ["rainkeepers", "twofold"])),
        Make("longAndSweptBack", "Long and swept back", By(races: ["elf"])),
        Make("ringedBeardBraids", "Ringed beard braids", By(races: ["dwarf"])),
        Make("curlyMop", "Curly mop", By(races: ["halfling"])),
        Make("whiteTufts", "White tufts", By(races: ["gnome"])),
        Make("pointedCap", "Pointed cap", By(races: ["gnome"])),
        Make("coarseCrest", "Coarse crest", By(races: ["orc"])),
        Make("roughKnot", "Rough knot", By(races: ["orc"])),
        Make("denseCloseCrop", "Dense close crop", By(races: ["hoarkin"])),
        Make("hardTieBack", "Hard tie-back", By(races: ["widegaze"])),
        Make("hairless", "Hairless", By(races: ["noonfolk"])),
        Make("flatSkullBraids", "Flat skull braids", By(races: ["boughwalkers"])),
        Make("oiledSlickBack", "Oiled slick-back", By(races: ["bitterbloods"])),
    ];
}

public sealed partial record OutfitParameters
{
    /// <summary>The default list: 72 outfits, all on. Every default race has one for each sex at each wealth.</summary>
    public static IReadOnlyList<OutfitParameters> Defaults { get; } =
    [
        // Women
        Make("kirtleAndApron", "Kirtle and apron", By(Female, Midland, from: Poor, to: Common), "grey", "white"),
        Make("strapDressAndBrooches", "Strap dress and brooches", By(Female, ["northmen"], from: Poor)),
        Make("sidelessSurcoat", "Sideless surcoat", By(Female, Midland, from: Rich)),
        Make("highBeltedGown", "High-belted gown", By(Female, Midland, from: Comfortable)),
        Make("queensMantle", "Queen's mantle", By(Female, Midland, from: Rich, vocations: ["noble"])),
        Make("nunsHabit", "Nun's habit", By(Female, Trades, vocations: ["priest"]), "black", "white"),
        Make("furParka", "Fur parka", By(races: ["northmen"], vocations: ["hunter", "fisher"]), "brown", "cream"),
        Make("bandedHerderTunic", "Banded herder tunic", By(races: ["northmen"], vocations: ["herder"])),
        Make("pinnedTunicAndWrap", "Pinned tunic and wrap", By(Female, ["swiftborn"])),
        Make("priestessWhites", "Priestess whites", By(Female, ["swiftborn"], vocations: ["priest"]), "white", "purple"),
        Make("jewelledCourtCollar", "Jewelled court collar", By(Female, ["saltroadFolk"], from: Rich), accent: "gold"),
        Make("linenSheathAndBroadCollar", "Linen sheath and broad collar", By(Female, ["saltroadFolk"], from: Comfortable), "white", "blue"),
        Make("desertBodyWrap", "Desert body wrap", By(Female, ["saltroadFolk"])),
        Make("courtKaftan", "Court kaftan", By(races: ["saltroadFolk"], from: Rich)),
        Make("wrappedRidingCoat", "Wrapped riding coat", By(races: ["grassRiders"])),
        Make("drapedSari", "Draped sari", By(Female, ["rainkeepers"])),
        Make("jacketAndHighSkirt", "Jacket and high skirt", By(Female, ["canalFolk"], from: Comfortable)),
        Make("layeredCourtRobes", "Layered court robes", By(Female, ["canalFolk"], from: Rich)),
        Make("fieldJacketAndStrawHat", "Field jacket and straw hat", By(races: ["canalFolk"], to: Common), "cream", "tan"),
        Make("breastClothAndSarong", "Breast cloth and sarong", By(Female, ["rainkeepers"])),
        Make("wrapperAndBlouse", "Wrapper and blouse", By(Female, ["sunlanders"], from: Poor, to: Comfortable)),
        Make("stripWovenRoyalCloth", "Strip-woven royal cloth", By(races: ["sunlanders"], from: Rich)),
        Make("redHerderCloth", "Red herder cloth", By(races: ["sunlanders"], to: Common), "red"),
        Make("healersGownAndSatchel", "Healer's gown and satchel", By(Female, Trades, vocations: ["healer"]), "cream", "white"),
        Make("huntersHoodAndJerkin", "Hunter's hood and jerkin", By(races: Trades, vocations: ["hunter", "forester"]), "olive", "brown"),
        Make("sorceressRobe", "Sorceress robe", By(Female, Trades, from: Common, vocations: ["mage", "seer"])),
        Make("paddedCoatAndMailShirt", "Padded coat and mail shirt", By(Female, Trades, vocations: ["soldier", "guard", "bandit"]), "cream", "grey"),
        Make("beggarsRags", "Beggar's rags", By(races: Ragged, to: Destitute), "brown"),
        Make("playersMotley", "Player's motley", By(races: Trades, vocations: ["entertainer"])),
        // Men
        Make("fieldTunicAndHood", "Field tunic and hood", By(Male, Midland, from: Poor, to: Common), "brown", "cream"),
        Make("smithsLeatherApron", "Smith's leather apron", By(races: [.. Trades, "dwarf"], vocations: ["smith", "smelter"]), "brown", "cream"),
        Make("minersHoodAndSeatLeather", "Miner's hood and seat leather", By(races: [.. Trades, "dwarf"], vocations: ["miner", "quarryman"]), "cream", "brown"),
        Make("furredGownAndWoundHood", "Furred gown and wound hood", By(Male, Midland, from: Comfortable)),
        Make("doubletAndHose", "Doublet and hose", By(Male, Midland, from: Comfortable, toAge: 40)),
        Make("kingsMantleAndCrown", "King's mantle and crown", By(Male, Midland, from: Rich, vocations: ["noble"])),
        Make("monksHabit", "Monk's habit", By(Male, Trades, vocations: ["priest", "scribe", "scholar"]), "brown"),
        Make("mailAndSurcoat", "Mail and surcoat", By(Male, Midland, from: Comfortable, vocations: ["noble", "soldier"])),
        Make("paddedJackAndKettleHat", "Padded jack and kettle hat", By(Male, Trades, vocations: ["guard", "soldier"]), "cream", "grey"),
        Make("lamellarCoat", "Lamellar coat", By(Male, ["grassRiders", "canalFolk"], vocations: ["soldier", "guard"])),
        Make("shoulderPinnedCloakAndTunic", "Shoulder-pinned cloak and tunic", By(Male, ["northmen"], from: Poor)),
        Make("citizensToga", "Citizen's toga", By(Male, ["swiftborn"], from: Common), "white"),
        Make("oneShoulderWorkTunic", "One-shoulder work tunic", By(Male, ["swiftborn"], to: Common), "cream"),
        Make("linenKiltAndBroadCollar", "Linen kilt and broad collar", By(Male, ["saltroadFolk"]), "white"),
        Make("veilTurbanAndRobe", "Veil-turban and robe", By(Male, ["saltroadFolk"]), "indigo", "white"),
        Make("waistClothAndShoulderCloth", "Waist cloth and shoulder cloth", By(Male, ["rainkeepers"])),
        Make("officialsBadgeRobe", "Official's badge robe", By(Male, ["canalFolk"], from: Comfortable, vocations: ["official", "scribe"])),
        Make("wideSleevedGrandRobe", "Wide-sleeved grand robe", By(races: ["saltroadFolk", "sunlanders"], from: Comfortable)),
        Make("crossedRobeAndSash", "Crossed robe and sash", By(races: ["canalFolk"])),
        Make("wizardsRobeAndPointedHat", "Wizard's robe and pointed hat", By(Male, Trades, vocations: ["mage", "seer", "alchemist"]), "grey"),
        // Written for single races
        Make("guildCutTownDress", "Guild-cut town dress", By(races: Midland, from: Common, to: Comfortable)),
        Make("indigoWrappedCotton", "Indigo wrapped cotton", By(races: ["sunlanders"]), "indigo"),
        Make("woodlandGreysAndGreens", "Woodland greys and greens", By(races: ["elf"]), "forestGreen", "grey"),
        Make("thickWoolAndToolBelt", "Thick wool and tool belt", By(races: ["dwarf"])),
        Make("brightWaistcoatAndWideHat", "Bright waistcoat and wide hat", By(races: ["halfling"])),
        Make("manyPocketedCoat", "Many-pocketed coat", By(races: ["gnome"])),
        Make("hideAndWonArmour", "Hide and won armour", By(races: ["orc"]), "brown", "grey"),
        Make("singleSealskin", "Single sealskin", By(races: ["hoarkin"]), "grey"),
        Make("kiltAndOpenVest", "Kilt and open vest", By(races: ["hearthbloods"])),
        Make("plainLongCoatAndCutAwayHat", "Plain long coat and cut-away hat", By(races: ["widegaze"]), "grey"),
        Make("paleWrappedLinen", "Pale wrapped linen", By(races: ["brinefolk"]), "cream", "black"),
        Make("quiltedRobeAndClothMask", "Quilted robe and cloth mask", By(races: ["duskreaders"]), "black"),
        Make("quiltedRobeAndHornVisor", "Quilted robe and horn visor", By(races: ["duskreaders"]), "black"),
        Make("hipWrapBareBack", "Hip wrap, bare back", By(races: ["noonfolk"])),
        Make("shortWrapsAndPouchBelts", "Short wraps and pouch belts", By(races: ["boughwalkers"]), "brown"),
        Make("loinclothAndDiveBelt", "Loincloth and dive belt", By(races: ["shoalfolk"]), "cream"),
        Make("hoodLongSleevesAndGloves", "Hood, long sleeves and gloves", By(races: ["lanternkin"])),
        Make("wrappedCottonSaffronSash", "Wrapped cotton, saffron sash", By(races: ["twofold"], toAge: 29), "cream", "saffron"),
        Make("wrappedCottonIndigoSash", "Wrapped cotton, indigo sash", By(races: ["twofold"], fromAge: 30), "cream", "indigo"),
        Make("bareShoulderedHomeWrap", "Bare-shouldered home wrap", By(races: ["bitterbloods"])),
        Make("glovesAndWaxedSkin", "Gloves and waxed skin", By(races: ["bitterbloods"])),
        Make("looseTailSlitClothes", "Loose tail-slit clothes", By(races: ["catfolk"])),
        Make("hoodAndBoundTail", "Hood and bound tail", By(races: ["catfolk"], vocations: ["merchant", "official"])),
    ];
}

public sealed partial record AccessoryParameters
{
    /// <summary>The default list: 51 accessories, all on, of eight kinds.</summary>
    public static IReadOnlyList<AccessoryParameters> Defaults { get; } =
    [
        // Necklaces
        Make("amberAndGlassBeadString", "Amber and glass bead string", Necklace, By(Female, ["northmen"])),
        Make("hammerAmulet", "Hammer amulet", Necklace, By(races: ["northmen"])),
        Make("twistedNeckRing", "Twisted neck ring", Necklace, By(races: ["northmen", "midlanders", "grassRiders", "orc", "hearthbloods", "widegaze"], from: Rich)),
        Make("toothAndBoneString", "Tooth and bone string", Necklace, By(races: ["sunlanders", "orc", "hoarkin", "amberbloods", "catfolk"])),
        Make("blueGlazedBeadString", "Blue glazed bead string", Necklace, By(races: ["saltroadFolk"], to: Common)),
        Make("cowrieShellString", "Cowrie shell string", Necklace, By(Female, ["sunlanders", "shoalfolk", "catfolk"], to: Common)),
        Make("redCoralBeadCollar", "Red coral bead collar", Necklace, By(races: ["sunlanders"], from: Rich)),
        Make("marriageThread", "Marriage thread", Necklace, By(Female, ["rainkeepers"], married: true)),
        Make("silverAmuletCase", "Silver amulet case", Necklace, By(races: ["saltroadFolk"], from: Common)),
        Make("chainOfOffice", "Chain of office", Necklace, By(Male, ["midlanders", "flinchless"], from: Rich)),
        Make("ivoryAmuletString", "Ivory amulet string", Necklace, By(races: ["hoarkin"])),
        // Earrings
        Make("plainHoopEarrings", "Plain hoop earrings", Earrings, By(races: ["saltroadFolk", "sunlanders", "grassRiders", "rainkeepers", "brinefolk", "noonfolk", "swiftborn", "bitterbloods"])),
        Make("pearlDropEarrings", "Pearl drop earrings", Earrings, By(Female, ["saltroadFolk"], from: Comfortable)),
        Make("filigreeFlowerEarrings", "Filigree flower earrings", Earrings, By(Female, ["rainkeepers"], from: Comfortable)),
        Make("singleEarRing", "Single ear ring", Earrings, By(Male, ["saltroadFolk", "grassRiders"])),
        // Bracelets
        Make("bronzeUpperArmBand", "Bronze upper-arm band", Bracelet, By(races: ["sunlanders", "rainkeepers", "dwarf", "orc", "hearthbloods", "catfolk"])),
        Make("stackedGlassBangles", "Stacked glass bangles", Bracelet, By(Female, ["saltroadFolk", "rainkeepers"], married: true)),
        Make("beastHeadBangle", "Beast-head bangle", Bracelet, By(races: ["saltroadFolk", "brinefolk"], from: Rich)),
        Make("serpentArmlet", "Serpent armlet", Bracelet, By(Female, ["saltroadFolk"], from: Comfortable)),
        Make("openBrassBangle", "Open brass bangle", Bracelet, By(races: ["sunlanders", "noonfolk", "catfolk"])),
        // Rings
        Make("goldSignetRing", "Gold signet ring", Ring, By(races: ["midlanders", "saltroadFolk", "rainkeepers", "dwarf", "halfling", "gnome", "brinefolk", "flinchless"], from: Rich)),
        Make("bronzeSealRing", "Bronze seal ring", Ring, By(races: ["midlanders", "dwarf", "halfling", "gnome", "flinchless"], from: Common, to: Comfortable)),
        Make("claspedHandsRing", "Clasped-hands ring", Ring, By(races: ["midlanders", "halfling", "flinchless"], married: true)),
        Make("cabochonStoneRing", "Cabochon stone ring", Ring, By(races: ["midlanders", "dwarf", "halfling", "gnome", "lanternkin", "bitterbloods"], from: Rich)),
        Make("scarabSwivelRing", "Scarab swivel ring", Ring, By(races: ["saltroadFolk"], from: Common)),
        Make("archersThumbRing", "Archer's thumb ring", Ring, By(races: ["grassRiders", "canalFolk"])),
        // Brooches
        Make("bowFibula", "Bow fibula", Brooch, By(races: ["midlanders"], to: Comfortable)),
        Make("ringBrooch", "Ring brooch", Brooch, By(races: ["midlanders", "halfling", "flinchless"])),
        Make("penannularCloakBrooch", "Penannular cloak brooch", Brooch, By(Male, ["northmen", "widegaze"], from: Common)),
        Make("garnetDiscBrooch", "Garnet disc brooch", Brooch, By(Female, ["midlanders", "widegaze"], from: Comfortable)),
        Make("pilgrimBadge", "Pilgrim badge", Brooch, By(races: ["midlanders"], to: Common)),
        Make("chainedTriangleBrooches", "Chained triangle brooches", Brooch, By(Female, ["saltroadFolk"], married: true)),
        Make("goldStagPlaque", "Gold stag plaque", Brooch, By(races: ["grassRiders"], from: Rich)),
        // Crowns
        Make("plainBrowCirclet", "Plain brow circlet", Crown, By(races: ["midlanders", "elf"], from: Comfortable)),
        Make("whiteClothDiadem", "White cloth diadem", Crown, By(Male, ["saltroadFolk"], from: Rich, vocations: ["noble"])),
        Make("oakLeafWreath", "Oak-leaf wreath", Crown, By(Male, ["midlanders"], vocations: ["soldier", "guard"])),
        Make("beadedFringeCrown", "Beaded fringe crown", Crown, By(Male, ["sunlanders"], from: Rich, vocations: ["noble"])),
        Make("phoenixCrown", "Phoenix crown", Crown, By(Female, ["canalFolk"], from: Rich)),
        // Body jewellery
        Make("goldNoseRing", "Gold nose ring", BodyJewellery, By(Female, ["saltroadFolk", "rainkeepers"], from: Comfortable, married: true)),
        Make("belledAnklets", "Belled anklets", BodyJewellery, By(Female, ["saltroadFolk", "rainkeepers"])),
        Make("jadeBeltPendant", "Jade belt pendant", BodyJewellery, By(races: ["canalFolk"], from: Comfortable)),
        // Hair jewellery
        Make("foreheadPendant", "Forehead pendant", HairJewellery, By(Female, ["rainkeepers"], from: Comfortable, married: true)),
        Make("carvedHairComb", "Carved hair comb", HairJewellery, By(Female, ["canalFolk"])),
        Make("templeRings", "Temple rings", HairJewellery, By(Female, ["midlanders"])),
        // Written for single races
        Make("bloodLampVial", "Blood-lamp vial", Necklace, By(races: ["lanternkin"], married: true)),
        Make("signalMirrorDisc", "Signal mirror disc", Necklace, By(races: ["widegaze"])),
        Make("scentedWristCord", "Scented wrist cord", Bracelet, By(races: ["stillfaces"])),
        Make("groovedRelayRing", "Grooved relay ring", Ring, By(races: ["swiftborn"], fromAge: 7)),
        Make("shapedSaltCrust", "Shaped salt crust", BodyJewellery, By(races: ["brinefolk"])),
        Make("bandedTusks", "Banded tusks", BodyJewellery, By(races: ["orc"])),
        Make("shellBraidBeads", "Shell braid beads", HairJewellery, By(races: ["shoalfolk"])),
    ];
}
