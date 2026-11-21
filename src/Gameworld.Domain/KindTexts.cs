using System.ComponentModel;

namespace Gameworld.Domain;

/// <summary>
/// The texts of one value of a fixed set of the domain model: an elevation type, a type or a
/// form of faction, a kind of settlement or a settlement tier. The sets are enums and the
/// rules decide which value a thing gets, so a list of these in the Parameter Set adds nothing
/// to the rules: it holds what a picture or a text is made from.
/// </summary>
[Description("The name, the description and the lore of one value of a fixed set, such as an elevation type or a form of faction.")]
public sealed record KindTextParameters<TKind>(
    [property: Description("The value the texts belong to.")] TKind Kind,
    [property: Description("The name shown for it.")] string Name)
    where TKind : struct, Enum
{
    [Description("What it is and looks like. A picture of it is drawn from this text. No generation rule reads it. The text \"**missing description**\" marks one that is not written yet.")]
    public string Description { get; init; } = DefaultTexts.Missing;

    [Description("Its lore: what the peoples of the world make of it and tell about it. No generation rule reads it. The text \"**missing description**\" marks one that is not written yet.")]
    public string Lore { get; init; } = DefaultTexts.Missing;
}

/// <summary>
/// The default texts of the fixed sets. A description is the line the docs have for the value;
/// where they have none it is written here, and so is every lore. A value that K gets no text
/// for has DefaultTexts.Missing.
/// </summary>
public static class KindTexts
{
    private static KindTextParameters<T> K<T>(T kind, string name, string description = DefaultTexts.Missing, string lore = DefaultTexts.Missing)
        where T : struct, Enum =>
        new(kind, name) { Description = description, Lore = lore };

    /// <summary>The elevation types (readme, "Elevation type"). The descriptions say what the land looks like.</summary>
    public static IReadOnlyList<KindTextParameters<ElevationType>> ElevationTypes { get; } =
    [
        K(ElevationType.Ocean, "Ocean", "Open salt water to the horizon: grey-green swell, white surf on the shore, gulls and a far sail. No one lives on it, but ships cross it and fishers work its edge.", "The Northmen call it the whale road and the Shoalfolk call it home. To everyone else it is the edge of the world, and what comes over it is trade or raiders."),
        K(ElevationType.FlatLand, "Flat Land", "Level land under a wide sky, with a far, straight horizon: plains, river flats and marsh. Rivers wind slowly across it, roads run straight, and a tower is seen from a day away.", "There is nowhere to hide and nothing to stop an army or a flood. The people of the plain build mounds, dikes and walls, and watch the horizon."),
        K(ElevationType.Lowland, "Lowland", "Gently rolling country of low rises and broad, shallow valleys, easy to plough and to cross. Fields, hedges and woods cover the swells, and a village lies in every hollow.", "The settled land, where the Old Realms laid their roads and the kingdoms of the Scattered Age hold their courts. Every field has a name and an owner."),
        K(ElevationType.Hills, "Hills", "Steep, rounded heights and narrow valleys with quick streams. Pasture and wood on the slopes, rock showing at the crests, winding roads, and a fort or a tower on many a hilltop.", "The hill people keep sheep, feuds and their own counsel. The plains have always called them thieves, and have always hired them as soldiers."),
        K(ElevationType.Mountains, "Mountains", "High ranges of bare rock, with snow on the peaks, cliffs, scree and deep gorges. Forest clings to the lower slopes, torrents fall white to the valleys, and the few roads cross by high passes.", "The gods live on the peaks and the Dwarves under them. A pass is a kingdom's gate, and the peoples of the heights have never been ruled for long."),
    ];

    /// <summary>The types of faction: what binds the members (society-generation.md section 4.1).</summary>
    public static IReadOnlyList<KindTextParameters<FactionType>> FactionTypes { get; } =
    [
        K(FactionType.Family, "Family", "Bound by blood and marriage: a house and its household.", "The oldest bond, and the one that outlasts every other. A house remembers a slight for five generations and a kindness for two."),
        K(FactionType.Race, "Race", "Bound by a people: one race, its clans and its customs.", "A people looks after its own first. In the Ashen Years each of them learnt who would open a gate to it, and none has forgotten."),
        K(FactionType.Vocation, "Vocation", "Bound by work: a trade, its masters and its monopoly.", "A trade is a brotherhood with secrets, dues and a feast day. Its masters decide who may work, and call that protecting the craft."),
        K(FactionType.Belief, "Belief", "Bound by a faith: a temple, an order, a sect.", "A faith holds what the crown cannot reach: the conscience, the calendar and the dead. It is also the largest landlord in most kingdoms."),
        K(FactionType.Magic, "Magic", "Bound by the arcane: a college, a circle, a cabal.", "The sorcerer-kings ruled the Old Realms and broke them. Every bond of mages since has been watched for the same ambition, usually with reason."),
        K(FactionType.Place, "Place", "Bound by a home: a town or a valley that rules itself.", "Town air makes free, the saying goes: a year and a day inside the walls, and no lord may claim you. Those who share a wall or a valley defend it together."),
    ];

    /// <summary>The forms of faction: the name of a type in a role (society-generation.md section 4.2).</summary>
    public static IReadOnlyList<KindTextParameters<FactionForm>> FactionForms { get; } =
    [
        K(FactionForm.Crown, "Crown", "A ruling house.", "Each kingdom is sure that it is the heir of an Old Realm. A crown reaches as far as its court can ride, and is disputed at every death."),
        K(FactionForm.House, "House", "A rival house in opposition, or a great house under the ruler.", "A great house keeps its own hall, its own colours and its own list of wrongs. It serves the crown until the day it sees a better claim, which is often its own."),
        K(FactionForm.Horde, "Horde", "A herding people of open land.", "The herding peoples are a scatter of camps until one khan is raised over all. Then they are a single army, and the settled lands learn its name."),
        K(FactionForm.Clan, "Clan", "A people under its chiefs.", "Everyone in it is kin, and the chief is the first among cousins. He rules by the consent of the elders, and they remind him of it."),
        K(FactionForm.Hold, "Hold", "A people in rough land with ore.", "A gate, a granary and a ledger. The hold keeps seven years of grain and every contract it ever signed, and has never yet opened to a siege."),
        K(FactionForm.Enclave, "Enclave", "A people that lives under the ruler of another.", "A people living by its own law inside another's land, by an old grant. It pays for the grant in each reign, and keeps the parchment safe."),
        K(FactionForm.MerchantRepublic, "Merchant republic", "Rule by the guilds of the trading towns.", "Ruled by those with ships and warehouses, who elect one of themselves and watch him closely. It fights its wars with hired soldiers and its ledgers."),
        K(FactionForm.Guild, "Guild", "A guild of one trade, in opposition or under the ruler.", "The masters of one trade, sworn together. They fix the price, the wage and the number of shops, bury their dead and feast their saint."),
        K(FactionForm.TempleState, "Temple state", "Rule by a faith.", "The god is the ruler and the high priest the steward. The tithe is the tax, the temple granary is the treasury, and heresy is treason."),
        K(FactionForm.Sect, "Sect", "A faith of the poor against the ruler.", "It begins with a preacher and the poor. The temple calls it heresy and the crown calls it rebellion, and it thinks little of either."),
        K(FactionForm.Order, "Order", "A faith with a rule of its own, in opposition or under the ruler.", "Sworn men and women under a rule and a master, with houses in many lands. An order answers to its own head and outlives the kings who gave it land."),
        K(FactionForm.Magocracy, "Magocracy", "Rule by the mages.", "The sorcerer-kings come again, its neighbours say. The mages answer that those who understand the world should govern it, as they said the last time."),
        K(FactionForm.Cabal, "Cabal", "The mages against the ruler.", "Mages who took no oath to a college or broke the one they took. They meet masked, and want what the sorcerer-kings had."),
        K(FactionForm.Circle, "Circle", "The mages under the ruler.", "The licensed mages of a realm, sworn to the ruler and to each other. They police their own, which is the price of being left alone."),
        K(FactionForm.FreeCity, "Free city", "Towns that rule themselves, as the ruler or against it.", "It owes no lord anything but a yearly gift. The citizens man the walls themselves, ring the great bell in danger, and shut the gates on princes."),
        K(FactionForm.Commune, "Commune", "Free farmers that rule themselves, as the ruler or against it.", "Farmers who swore an oath together and kept it. They meet under a tree, vote by raised hands, and have beaten armies of knights with pikes."),
        K(FactionForm.CharteredTown, "Chartered town", "A town with a charter of its own under the ruler.", "Its freedoms are written in a charter kept in a chest with three locks. Each new ruler confirms it, for a fee."),
    ];

    /// <summary>The kinds of settlement: a city or a town (society-generation.md section 5.2). The descriptions say what the place looks like.</summary>
    public static IReadOnlyList<KindTextParameters<SettlementKind>> SettlementKinds { get; } =
    [
        K(SettlementKind.Town, "Town", "A small market place: a square with a well, a temple, an inn, a row of workshops and a weekly market, behind a ditch and a palisade or a low wall. Fields and gardens begin at the last house.", "A town is where the country comes on market day. Everyone knows whose daughter you are, and the lord's bailiff lives in the big house by the square."),
        K(SettlementKind.City, "City", "A large walled place: stone walls with towers and gates, crowded streets of tall, narrow houses, guild halls, a great temple, markets for every trade, and a castle or a palace. Its smoke is seen from far off, and suburbs spill out along the roads.", "A city eats more people than are born in it, and fills up again from the villages. They come because behind its walls no one asks who their father was."),
    ];

    /// <summary>The tiers of a place, lowest first (society-generation.md section 8). The descriptions say what life in the tier is like.</summary>
    public static IReadOnlyList<KindTextParameters<SettlementTier>> SettlementTiers { get; } =
    [
        K(SettlementTier.Countryside, "Countryside", "The land outside the towns: villages, hamlets and lone farms among fields, pasture and wood, with a mill on the stream, a small shrine and a manor house. Where most people live, and where nearly all of them work the land.", "Nine people in ten live here and are born, wed and buried within sight of one tower. The year is the plough, the scythe and the feast days."),
        K(SettlementTier.Town, "Town", "The life of a small market town: a few streets of craftsmen and shopkeepers round a market square, where the country comes to sell and buy. Everyone knows everyone, and a guild is a dozen masters.", "A townsman is a farmer's grandson with a trade. The guild, the market cross and the council of twelve are the whole of public life."),
        K(SettlementTier.City, "City", "The life of a city: whole streets given to one trade, great merchants and their warehouses, schools, courts and temples, and crowds of labourers, servants, beggars and thieves. Here are the rare crafts that a town cannot feed.", "A city has a stranger in every street and a guild for every craft, the thieves and the beggars included. What is new begins here, and what is banned is sold here."),
        K(SettlementTier.Capital, "Capital", "The seat of a ruler: a city with a court. A palace or a citadel above the roofs, the halls of the officials, the mint and the treasury, the houses of the great families, envoys from other lands, and the finest craftsmen there are, who work for the court.", "Where the court sits, everything is for sale and everyone is a petitioner. The capital lives on the taxes of the realm, and is the first place to riot."),
    ];
}
