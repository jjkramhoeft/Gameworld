namespace Gameworld.Domain;

// The default list of vocations: the 51 vocations of docs/society-generation.md section 8, in
// the order of its table. The lowest tier, the share, the standing (lowest, highest) and the
// variants are those of docs/research/vocations.md section 3: starting values, not tuned
// against generated regions.

public sealed partial record VocationParameters
{
    /// <summary>The default list: 51 vocations in nine sectors, all on. The shares sum to 100.</summary>
    public static IReadOnlyList<VocationParameters> Defaults { get; } =
    [
        Make("farmer", "Farmer", VocationSector.LandAndWater, SettlementTier.Countryside, 35, 3, 3, false, ["ploughman", "peasant", "free farmer", "tenant", "share-cropper", "paddy farmer", "rice farmer", "millet farmer", "yam farmer"]),
        Make("gardener", "Gardener", VocationSector.LandAndWater, SettlementTier.Countryside, 3, 3, 3, false, ["orchard keeper", "vine dresser", "olive grower", "date gardener", "spice grower", "tea grower", "coffee grower", "tobacco grower", "dye-plant grower", "silk grower", "beekeeper", "mushroom grower", "seedsman", "grafter"]),
        Make("herder", "Herder", VocationSector.LandAndWater, SettlementTier.Countryside, 8, 3, 3, false, ["shepherd", "cowherd", "swineherd", "goatherd", "reindeer herder", "yak herder", "nomad herder", "dairymaid", "cheesemaker", "horse breeder", "horse breaker", "camel breeder"]),
        Make("fisher", "Fisher", VocationSector.LandAndWater, SettlementTier.Countryside, 2, 3, 3, false, ["sea fisher", "river fisher", "eel trapper", "shellfish gatherer", "crab catcher", "shrimp catcher", "whaler", "sealer", "pearl diver", "sponge diver", "coral diver", "fish drier"]),
        Make("hunter", "Hunter", VocationSector.LandAndWater, SettlementTier.Countryside, 1, 3, 3, false, ["hunter", "trapper", "fowler", "falconer", "forager", "honey hunter", "tracker", "guide", "scout", "ratcatcher", "monster hunter"]),
        Make("forester", "Forester", VocationSector.LandAndWater, SettlementTier.Countryside, 1.5, 3, 3, false, ["woodcutter", "lumberjack", "sawyer", "forest warden", "grove warden", "resin tapper", "tree shaper", "firewood seller"]),
        Make("miner", "Miner", VocationSector.LandAndWater, SettlementTier.Countryside, 0.7, 3, 3, false, ["ore miner", "prospector", "gold washer", "tin streamer", "coal miner", "gem miner", "crystal miner"]),
        Make("quarryman", "Quarryman", VocationSector.LandAndWater, SettlementTier.Countryside, 0.4, 2, 3, false, ["quarrier", "stonecutter", "slate splitter", "millstone cutter", "sand digger", "gravel digger", "clay digger"]),
        Make("salter", "Salter", VocationSector.LandAndWater, SettlementTier.Countryside, 0.2, 3, 3, false, ["salt boiler", "pan master", "salt raker", "rock-salt miner", "brine worker", "soda burner"]),
        Make("miller", "Miller", VocationSector.Processing, SettlementTier.Countryside, 1, 4, 4, false, ["water miller", "windmill keeper", "horse-mill keeper", "oil presser", "fuller", "sawyer"]),
        Make("baker", "Baker", VocationSector.Processing, SettlementTier.Town, 0.6, 4, 4, false, ["baker", "keeper of the common oven", "pastry cook", "pie maker", "cook", "cookshop keeper"]),
        Make("brewer", "Brewer", VocationSector.Processing, SettlementTier.Countryside, 1.2, 3, 4, false, ["brewer", "alewife", "maltster", "wine maker", "cider maker", "mead maker", "distiller"]),
        Make("butcher", "Butcher", VocationSector.Processing, SettlementTier.Town, 0.5, 4, 4, false, ["butcher", "slaughterer", "poulterer", "tripe seller", "fishmonger", "fish salter", "pickler", "renderer", "tallow chandler", "soap boiler"]),
        Make("charcoalBurner", "Charcoal burner", VocationSector.Processing, SettlementTier.Countryside, 0.3, 2, 2, false, ["collier", "tar burner", "pitch boiler", "ash burner", "varnish maker", "lime burner"]),
        Make("smelter", "Smelter", VocationSector.Processing, SettlementTier.Countryside, 0.2, 3, 4, false, ["bloomery man", "iron smelter", "furnace master", "copper smelter", "silver refiner", "steel maker", "bronze founder"]),
        Make("tanner", "Tanner", VocationSector.Processing, SettlementTier.Town, 0.4, 3, 3, false, ["tanner", "tawyer", "currier", "skinner", "furrier", "parchment maker"]),
        Make("weaver", "Weaver", VocationSector.Processing, SettlementTier.Countryside, 3.5, 3, 3, false, ["spinner", "weaver", "linen weaver", "silk weaver", "fuller", "dyer", "shearman", "felt maker", "carpet weaver", "rope maker", "net maker"]),
        Make("smith", "Smith", VocationSector.Craft, SettlementTier.Countryside, 1, 4, 4, false, ["blacksmith", "farrier", "toolsmith", "nailer", "cutler", "locksmith", "weaponsmith", "armourer", "sword polisher", "bronze founder", "bell caster", "forge hand"]),
        Make("carpenter", "Carpenter", VocationSector.Craft, SettlementTier.Countryside, 1.5, 4, 4, false, ["carpenter", "joiner", "wheelwright", "cartwright", "cooper", "turner", "chest maker", "bowyer", "fletcher", "sled maker", "woodcarver"]),
        Make("shipwright", "Shipwright", VocationSector.Craft, SettlementTier.Town, 0.2, 4, 4, true, ["shipwright", "boatbuilder", "caulker", "hull mender", "sail maker", "barge builder"]),
        Make("mason", "Mason", VocationSector.Craft, SettlementTier.Town, 1, 4, 4, false, ["stonemason", "bricklayer", "builder", "plasterer", "tiler", "roofer", "thatcher", "steeplejack", "well digger", "bridge builder", "engineer"]),
        Make("potter", "Potter", VocationSector.Craft, SettlementTier.Town, 0.4, 3, 3, false, ["potter", "brickmaker", "tile maker", "kiln master", "maker of fine ware"]),
        Make("glassmaker", "Glassmaker", VocationSector.Craft, SettlementTier.City, 0.05, 4, 5, false, ["glassblower", "glazier", "bead maker", "mirror maker", "lens grinder", "vial blower"]),
        Make("leatherworker", "Leatherworker", VocationSector.Craft, SettlementTier.Town, 1.2, 4, 4, false, ["shoemaker", "cobbler", "saddler", "harness maker", "girdler", "glover", "purse maker", "scabbard maker", "bookbinder"]),
        Make("tailor", "Tailor", VocationSector.Craft, SettlementTier.Town, 1.2, 4, 4, false, ["tailor", "seamstress", "robe maker", "hosier", "hatter", "embroiderer", "garment furrier", "old-clothes dealer"]),
        Make("jeweller", "Jeweller", VocationSector.Craft, SettlementTier.City, 0.15, 5, 5, false, ["goldsmith", "silversmith", "gem cutter", "lapidary", "engraver", "enameller", "moneyer", "assayer", "ivory carver", "coral carver", "amber worker"]),
        Make("artist", "Artist", VocationSector.Craft, SettlementTier.City, 0.1, 4, 4, false, ["painter", "sculptor", "image carver", "illuminator", "glass painter", "temple sculptor", "mosaic maker", "map engraver", "instrument maker"]),
        Make("artificer", "Artificer", VocationSector.Craft, SettlementTier.Capital, 0.02, 5, 5, false, ["clockmaker", "fine locksmith", "instrument maker", "lens grinder", "tinker", "automaton maker", "siege engineer", "object enchanter"]),
        Make("merchant", "Merchant", VocationSector.TradeAndTransport, SettlementTier.City, 0.5, 5, 5, false, ["long-distance merchant", "wholesaler", "draper", "mercer", "spicer", "grain factor", "broker", "moneychanger", "banker", "moneylender", "insurer"]),
        Make("shopkeeper", "Shopkeeper", VocationSector.TradeAndTransport, SettlementTier.Town, 1.5, 3, 3, false, ["retailer", "stallholder", "huckster", "pedlar", "chapman", "market trader"]),
        Make("carter", "Carter", VocationSector.TradeAndTransport, SettlementTier.Town, 1.2, 3, 3, false, ["carter", "teamster", "muleteer", "caravan driver", "caravan master", "drover", "porter", "water carrier", "courier", "post rider", "elephant driver"]),
        Make("sailor", "Sailor", VocationSector.TradeAndTransport, SettlementTier.Town, 1.2, 3, 3, true, ["seaman", "ship's master", "pilot", "rigger", "bargeman", "boatman", "ferryman", "raftsman", "ship's lookout"]),
        Make("innkeeper", "Innkeeper", VocationSector.TradeAndTransport, SettlementTier.Town, 0.8, 4, 4, false, ["innkeeper", "taverner", "hosteller", "alehouse keeper", "cookshop keeper", "bathhouse keeper", "stable keeper"]),
        Make("healer", "Healer", VocationSector.Service, SettlementTier.Countryside, 0.5, 3, 5, false, ["physician", "surgeon", "barber-surgeon", "barber", "bonesetter", "tooth puller", "midwife", "nurse", "herbalist"]),
        Make("servant", "Servant", VocationSector.Service, SettlementTier.Countryside, 7.9, 2, 2, false, ["maid", "manservant", "cook", "groom", "nurse", "wet nurse", "laundress", "valet", "page", "taster", "household steward"]),
        Make("entertainer", "Entertainer", VocationSector.Service, SettlementTier.Town, 0.2, 2, 4, false, ["minstrel", "bard", "harper", "singer", "drummer", "praise-singer", "storyteller", "poet", "actor", "acrobat", "rope dancer", "dancer", "jester", "wrestler"]),
        Make("courtesan", "Courtesan", VocationSector.Service, SettlementTier.Town, 0.1, 1, 5, false, ["courtesan", "companion", "kept mistress", "prostitute", "bathhouse attendant", "brothel keeper"]),
        Make("noble", "Noble", VocationSector.GovernanceAndWar, SettlementTier.Countryside, 1, 6, 6, false, ["lord", "lady", "knight", "squire", "landholder", "chief", "jarl", "khan", "title-holder", "head of a great house"]),
        Make("official", "Official", VocationSector.GovernanceAndWar, SettlementTier.Countryside, 0.8, 4, 5, false, ["steward", "bailiff", "reeve", "hayward", "magistrate", "judge", "tax collector", "toll keeper", "customs searcher", "weigh-master", "sworn measurer", "surveyor", "mayor", "herald", "town crier", "envoy", "treasurer", "gaoler"]),
        Make("soldier", "Soldier", VocationSector.GovernanceAndWar, SettlementTier.Town, 1, 3, 4, false, ["man-at-arms", "archer", "crossbowman", "spearman", "mounted archer", "cavalryman", "sergeant", "captain", "mercenary", "drill master", "marine", "champion"]),
        Make("guard", "Guard", VocationSector.GovernanceAndWar, SettlementTier.Town, 0.6, 3, 3, false, ["watchman", "town guard", "gate guard", "night watch", "bodyguard", "caravan guard", "road warden", "pass warden", "lookout", "beacon keeper", "granary warden", "vault keeper", "firefighter"]),
        Make("priest", "Priest", VocationSector.FaithAndLearning, SettlementTier.Countryside, 2.5, 4, 5, false, ["village priest", "temple priest", "chaplain", "shaman", "monk", "nun", "friar", "hermit", "ascetic", "sexton"]),
        Make("scribe", "Scribe", VocationSector.FaithAndLearning, SettlementTier.Town, 0.4, 4, 5, false, ["clerk", "copyist", "notary", "bookkeeper", "secretary", "advocate", "records clerk", "printer", "bookbinder", "bookseller", "witness for hire"]),
        Make("scholar", "Scholar", VocationSector.FaithAndLearning, SettlementTier.City, 0.15, 5, 5, false, ["teacher", "schoolmaster", "tutor", "professor", "sage", "astronomer", "mathematician", "lore keeper", "chronicler", "librarian", "keeper of genealogies"]),
        Make("mage", "Mage", VocationSector.Supernatural, SettlementTier.Capital, 0.03, 5, 5, false, ["wizard", "sorcerer", "enchanter", "court mage", "battle mage", "hedge wizard", "rune carver"]),
        Make("alchemist", "Alchemist", VocationSector.Supernatural, SettlementTier.City, 0.05, 4, 5, false, ["apothecary", "alchemist", "potion maker", "antidote maker", "poisoner", "perfumer", "embalmer", "maker of insect smoke"]),
        Make("seer", "Seer", VocationSector.Supernatural, SettlementTier.Countryside, 0.2, 2, 4, false, ["diviner", "astrologer", "geomancer", "soothsayer", "oracle", "medium", "dowser", "well-finder", "weather watcher", "wise woman", "hedge witch", "cunning man"]),
        Make("labourer", "Labourer", VocationSector.Margins, SettlementTier.Countryside, 11, 2, 2, false, ["day labourer", "field hand", "harvest hand", "builder's labourer", "digger", "ditcher", "dock hand", "quarry hand", "peat cutter", "ice cutter", "gravedigger"]),
        Make("beggar", "Beggar", VocationSector.Margins, SettlementTier.Town, 1.5, 1, 1, false, ["beggar", "pauper", "vagrant", "almsman", "cripple", "pilgrim", "refugee", "street orphan"]),
        Make("thief", "Thief", VocationSector.Margins, SettlementTier.Town, 0.3, 1, 1, false, ["cutpurse", "pickpocket", "burglar", "fence", "smuggler", "forger", "swindler", "blackmailer", "enforcer", "poisoner", "hired killer"]),
        Make("bandit", "Bandit", VocationSector.Margins, SettlementTier.Countryside, 0.25, 1, 1, false, ["highwayman", "robber", "raider", "rustler", "poacher", "outlaw", "pirate", "wrecker", "rebel"]),
    ];
}
