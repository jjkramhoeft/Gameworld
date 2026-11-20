namespace Gameworld.Domain;

// The default list of tools: the 146 tools of docs/tools-basis.md, in the order of that
// document, which follows the order of the vocation list. The vocations of a tool are its line
// "Fitting vocations", the vocation it stands under first. The emblem weights are the
// judgement of society-generation.md section 8.1: starting values, not tuned.

public sealed partial record ToolParameters
{
    /// <summary>The default list: 146 tools, all on, at least two for each of the 51 default vocations.</summary>
    public static IReadOnlyList<ToolParameters> Defaults { get; } =
    [
        // Farmer
        Make("heavyPlough", "Heavy plough", 2, ["farmer"]),
        Make("sickle", "Sickle", 3, ["farmer"]),
        Make("flail", "Flail", 1, ["farmer"]),
        Make("scythe", "Scythe", 2, ["farmer", "labourer"]),
        // Gardener
        Make("spadeIron", "Spade iron", 2, ["gardener", "labourer"]),
        Make("beeSkep", "Bee skep", 3, ["gardener"]),
        Make("billhook", "Billhook", 2, ["gardener", "forester"]),
        // Herder
        Make("shepherdsCrook", "Shepherd's crook", 3, ["herder"]),
        Make("ironShears", "Iron shears", 2, ["herder", "tailor"]),
        Make("plungeChurn", "Plunge churn", 1, ["herder"]),
        // Fisher
        Make("seineNet", "Seine net", 1, ["fisher"]),
        Make("eelTrap", "Eel trap", 1, ["fisher"]),
        Make("eelSpear", "Eel spear", 2, ["fisher"]),
        // Hunter
        Make("boarSpear", "Boar spear", 2, ["hunter"]),
        Make("falconsHood", "Falcon's hood", 1, ["hunter"]),
        Make("huntingHorn", "Hunting horn", 3, ["hunter"]),
        Make("longbow", "Longbow", 2, ["hunter", "soldier", "bandit"]),
        // Forester
        Make("fellingAxe", "Felling axe", 3, ["forester"]),
        Make("twoManSaw", "Two-man saw", 1, ["forester", "carpenter"]),
        // Miner
        Make("minersIronAndHammer", "Miner's iron and hammer", 3, ["miner"]),
        Make("wedgeCrowbarAndPick", "Wedge, crowbar and pick", 2, ["miner", "quarryman"]),
        Make("windlass", "Windlass", 1, ["miner"]),
        Make("diviningRod", "Divining rod", 1, ["miner", "seer"]),
        // Quarryman
        Make("quarryWedges", "Quarry wedges", 1, ["quarryman"]),
        Make("quarryPick", "Quarry pick", 2, ["quarryman"]),
        Make("crowbarAndSledge", "Crowbar and sledge", 1, ["quarryman"]),
        // Salter
        Make("leadSaltPan", "Lead salt pan", 1, ["salter"]),
        Make("saltRakeAndShovel", "Salt rake and shovel", 1, ["salter"]),
        Make("saltBasket", "Salt basket", 1, ["salter"]),
        // Miller
        Make("millstones", "Millstones", 2, ["miller"]),
        Make("millBillAndThrift", "Mill bill and thrift", 1, ["miller"]),
        Make("rotaryQuern", "Rotary quern", 1, ["miller"]),
        // Baker
        Make("bakersPeel", "Baker's peel", 2, ["baker"]),
        Make("kneadingTrough", "Kneading trough", 0, ["baker"]),
        // Brewer
        Make("mashTunAndMasher", "Mash tun and masher", 1, ["brewer"]),
        Make("brewingKettle", "Brewing kettle", 1, ["brewer"]),
        Make("alembicAndCucurbit", "Alembic and cucurbit", 2, ["brewer", "alchemist"]),
        // Butcher
        Make("cleaver", "Cleaver", 2, ["butcher"]),
        Make("poleAxe", "Pole axe", 3, ["butcher"]),
        // Charcoal burner
        Make("charcoalClamp", "Charcoal clamp", 0, ["charcoalBurner"]),
        Make("corrackAndLongShovel", "Corrack and long shovel", 1, ["charcoalBurner"]),
        Make("limeKiln", "Lime kiln", 0, ["charcoalBurner", "mason"]),
        // Smelter
        Make("bloomeryFurnace", "Bloomery furnace", 0, ["smelter"]),
        Make("bellows", "Bellows", 2, ["smelter", "smith"]),
        Make("cupel", "Cupel", 1, ["smelter", "jeweller"]),
        // Tanner
        Make("unhairingKnifeAndBeam", "Unhairing knife and beam", 1, ["tanner"]),
        Make("tanPits", "Tan pits", 0, ["tanner"]),
        Make("lunellumAndHerse", "Lunellum and herse", 1, ["tanner"]),
        // Weaver
        Make("spindleWhorlAndDistaff", "Spindle, whorl and distaff", 2, ["weaver"]),
        Make("loom", "Loom", 1, ["weaver"]),
        Make("croppingShearsAndTeasels", "Cropping shears and teasels", 2, ["weaver"]),
        // Smith
        Make("anvil", "Anvil", 3, ["smith"]),
        Make("hammersAndTongs", "Hammers and tongs", 3, ["smith"]),
        // Carpenter
        Make("carpentersAxe", "Carpenter's axe", 2, ["carpenter", "shipwright"]),
        Make("adze", "Adze", 2, ["carpenter", "shipwright"]),
        Make("auger", "Auger", 1, ["carpenter"]),
        Make("plane", "Plane", 1, ["carpenter"]),
        Make("poleLathe", "Pole lathe", 0, ["carpenter"]),
        Make("plumbRuleLevelAndSquare", "Plumb rule, level and square", 2, ["carpenter", "mason"]),
        // Shipwright
        Make("caulkingIronAndMallet", "Caulking iron and mallet", 1, ["shipwright"]),
        Make("clenchNailsAndRoves", "Clench nails and roves", 0, ["shipwright"]),
        // Mason
        Make("malletAndChisels", "Mallet and chisels", 2, ["mason"]),
        Make("treadwheelCrane", "Treadwheel crane", 0, ["mason"]),
        Make("lewis", "Lewis", 1, ["mason"]),
        Make("trowel", "Trowel", 2, ["mason"]),
        // Potter
        Make("pottersWheel", "Potter's wheel", 2, ["potter"]),
        Make("potteryKiln", "Pottery kiln", 0, ["potter"]),
        Make("stampsAndRoulettes", "Stamps and roulettes", 0, ["potter"]),
        // Glassmaker
        Make("blowpipe", "Blowpipe", 1, ["glassmaker"]),
        Make("pontil", "Pontil", 0, ["glassmaker"]),
        Make("jacksAndShears", "Jacks and shears", 1, ["glassmaker"]),
        Make("grozingIron", "Grozing iron", 2, ["glassmaker"]),
        // Leatherworker
        Make("roundKnife", "Round knife", 2, ["leatherworker"]),
        Make("awl", "Awl", 1, ["leatherworker"]),
        Make("shoemakersLast", "Shoemaker's last", 1, ["leatherworker"]),
        // Tailor
        Make("thimbleAndNeedle", "Thimble and needle", 1, ["tailor"]),
        // Jeweller
        Make("touchstoneAndTouchNeedles", "Touchstone and touch needles", 0, ["jeweller", "merchant"]),
        Make("coinDies", "Coin dies", 1, ["jeweller"]),
        Make("drawPlate", "Draw plate", 0, ["jeweller"]),
        Make("graver", "Graver", 1, ["jeweller"]),
        Make("hessianCrucible", "Hessian crucible", 1, ["jeweller", "alchemist"]),
        // Artist
        Make("grindingSlabAndMuller", "Grinding slab and muller", 0, ["artist"]),
        Make("mineverAndBristleBrushes", "Minever and bristle brushes", 1, ["artist"]),
        Make("silverStyle", "Silver style", 0, ["artist"]),
        // Artificer
        Make("turnsAndBow", "Turns and bow", 0, ["artificer"]),
        Make("screwPlate", "Screw plate", 1, ["artificer"]),
        // Merchant
        Make("coinBalance", "Coin balance", 3, ["merchant", "shopkeeper"]),
        Make("jettonsAndCountingBoard", "Jettons and counting board", 1, ["merchant", "shopkeeper", "official", "scribe", "scholar"]),
        Make("tallyStick", "Tally stick", 1, ["merchant", "innkeeper", "official"]),
        Make("steelyard", "Steelyard", 2, ["merchant", "shopkeeper"]),
        // Shopkeeper
        Make("standardYard", "Standard yard", 1, ["shopkeeper", "official"]),
        Make("standardBushel", "Standard bushel", 1, ["shopkeeper", "official"]),
        // Carter
        Make("horseCollar", "Horse collar", 2, ["carter"]),
        Make("twoWheeledCart", "Two-wheeled cart", 2, ["carter"]),
        Make("packSaddle", "Pack saddle", 1, ["carter"]),
        // Sailor
        Make("soundingLeadAndLine", "Sounding lead and line", 1, ["sailor"]),
        Make("marinersCompass", "Mariner's compass", 2, ["sailor"]),
        Make("marinersAstrolabe", "Mariner's astrolabe", 1, ["sailor"]),
        // Innkeeper
        Make("blackJack", "Black jack", 2, ["innkeeper"]),
        Make("aleStake", "Ale-stake", 2, ["innkeeper"]),
        // Healer
        Make("fleamAndBleedingBowl", "Fleam and bleeding bowl", 3, ["healer"]),
        Make("uroscopyFlask", "Uroscopy flask", 2, ["healer"]),
        Make("mortarAndPestle", "Mortar and pestle", 3, ["healer", "alchemist"]),
        Make("foldingAlmanac", "Folding almanac", 0, ["healer", "seer"]),
        // Servant
        Make("cauldron", "Cauldron", 2, ["servant"]),
        Make("roastingSpit", "Roasting spit", 1, ["servant"]),
        Make("boxwoodComb", "Boxwood comb", 2, ["servant", "courtesan"]),
        // Entertainer
        Make("harp", "Harp", 3, ["entertainer"]),
        Make("pipeAndTabor", "Pipe and tabor", 2, ["entertainer"]),
        Make("hurdyGurdy", "Hurdy-gurdy", 1, ["entertainer", "beggar"]),
        // Courtesan
        Make("mirrorInAnIvoryCase", "Mirror in an ivory case", 2, ["courtesan"]),
        Make("pomander", "Pomander", 1, ["courtesan"]),
        // Noble
        Make("sealMatrix", "Seal matrix", 1, ["noble", "official"]),
        Make("knightlySword", "Knightly sword", 3, ["noble", "soldier"]),
        Make("spurs", "Spurs", 2, ["noble"]),
        // Official
        Make("maceOfOffice", "Mace of office", 3, ["official"]),
        // Soldier
        Make("billAndPike", "Bill and pike", 2, ["soldier", "guard"]),
        Make("mailShirt", "Mail shirt", 1, ["soldier"]),
        Make("halberd", "Halberd", 2, ["soldier", "guard"]),
        Make("caltrop", "Caltrop", 2, ["soldier", "bandit"]),
        // Guard
        Make("watchmansHorn", "Watchman's horn", 2, ["guard"]),
        Make("hornLantern", "Horn lantern", 2, ["guard"]),
        Make("fireHook", "Fire hook", 1, ["guard"]),
        // Priest
        Make("censerAndIncenseBoat", "Censer and incense boat", 2, ["priest"]),
        Make("chalice", "Chalice", 3, ["priest"]),
        Make("shamansDrum", "Shaman's drum", 2, ["priest"]),
        // Scribe
        Make("quillPenAndPenknife", "Quill pen and penknife", 2, ["scribe"]),
        Make("waxTabletAndStylus", "Wax tablet and stylus", 1, ["scribe", "scholar"]),
        Make("pricker", "Pricker", 0, ["scribe"]),
        // Scholar
        Make("astrolabe", "Astrolabe", 2, ["scholar", "seer"]),
        Make("armillarySphere", "Armillary sphere", 3, ["scholar"]),
        // Mage
        Make("sealsOfWax", "Seals of wax", 1, ["mage"]),
        Make("bookOfConjurations", "Book of conjurations", 2, ["mage"]),
        Make("runeStick", "Rune stick", 1, ["mage"]),
        // Alchemist
        Make("athanor", "Athanor", 1, ["alchemist"]),
        // Seer
        Make("shewStone", "Shew-stone", 1, ["seer"]),
        Make("geomanticInstrument", "Geomantic instrument", 0, ["seer"]),
        // Labourer
        Make("woodenShovel", "Wooden shovel", 1, ["labourer"]),
        Make("wheelbarrow", "Wheelbarrow", 1, ["labourer"]),
        // Beggar
        Make("lepersClapper", "Leper's clapper", 1, ["beggar"]),
        Make("almsBowl", "Alms bowl", 1, ["beggar"]),
        Make("pilgrimBadge", "Pilgrim badge", 2, ["beggar"]),
        // Thief
        Make("falseDice", "False dice", 2, ["thief"]),
        Make("picklock", "Picklock", 1, ["thief"]),
        Make("bollockDagger", "Bollock dagger", 2, ["thief", "bandit"]),
        // Bandit
        Make("falchion", "Falchion", 2, ["bandit"]),
    ];
}
