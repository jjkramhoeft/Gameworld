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
/// where they have none it is DefaultTexts.Missing, and so is every lore. Give K a description
/// and a lore to fill them in.
/// </summary>
public static class KindTexts
{
    private static KindTextParameters<T> K<T>(T kind, string name, string description = DefaultTexts.Missing, string lore = DefaultTexts.Missing)
        where T : struct, Enum =>
        new(kind, name) { Description = description, Lore = lore };

    /// <summary>The elevation types (readme, "Elevation type"). No doc describes one of them.</summary>
    public static IReadOnlyList<KindTextParameters<ElevationType>> ElevationTypes { get; } =
    [
        K(ElevationType.Ocean, "Ocean"),
        K(ElevationType.FlatLand, "Flat Land"),
        K(ElevationType.Lowland, "Lowland"),
        K(ElevationType.Hills, "Hills"),
        K(ElevationType.Mountains, "Mountains"),
    ];

    /// <summary>The types of faction: what binds the members (society-generation.md section 4.1).</summary>
    public static IReadOnlyList<KindTextParameters<FactionType>> FactionTypes { get; } =
    [
        K(FactionType.Family, "Family", "Bound by blood and marriage: a house and its household."),
        K(FactionType.Race, "Race", "Bound by a people: one race, its clans and its customs."),
        K(FactionType.Vocation, "Vocation", "Bound by work: a trade, its masters and its monopoly."),
        K(FactionType.Belief, "Belief", "Bound by a faith: a temple, an order, a sect."),
        K(FactionType.Magic, "Magic", "Bound by the arcane: a college, a circle, a cabal."),
        K(FactionType.Place, "Place", "Bound by a home: a town or a valley that rules itself."),
    ];

    /// <summary>The forms of faction: the name of a type in a role (society-generation.md section 4.2).</summary>
    public static IReadOnlyList<KindTextParameters<FactionForm>> FactionForms { get; } =
    [
        K(FactionForm.Crown, "Crown", "A ruling house."),
        K(FactionForm.House, "House", "A rival house in opposition, or a great house under the ruler."),
        K(FactionForm.Horde, "Horde", "A herding people of open land."),
        K(FactionForm.Clan, "Clan", "A people under its chiefs."),
        K(FactionForm.Hold, "Hold", "A people in rough land with ore."),
        K(FactionForm.Enclave, "Enclave", "A people that lives under the ruler of another."),
        K(FactionForm.MerchantRepublic, "Merchant republic", "Rule by the guilds of the trading towns."),
        K(FactionForm.Guild, "Guild", "A guild of one trade, in opposition or under the ruler."),
        K(FactionForm.TempleState, "Temple state", "Rule by a faith."),
        K(FactionForm.Sect, "Sect", "A faith of the poor against the ruler."),
        K(FactionForm.Order, "Order", "A faith with a rule of its own, in opposition or under the ruler."),
        K(FactionForm.Magocracy, "Magocracy", "Rule by the mages."),
        K(FactionForm.Cabal, "Cabal", "The mages against the ruler."),
        K(FactionForm.Circle, "Circle", "The mages under the ruler."),
        K(FactionForm.FreeCity, "Free city", "Towns that rule themselves, as the ruler or against it."),
        K(FactionForm.Commune, "Commune", "Free farmers that rule themselves, as the ruler or against it."),
        K(FactionForm.CharteredTown, "Chartered town", "A town with a charter of its own under the ruler."),
    ];

    /// <summary>The kinds of settlement: a city or a town (society-generation.md section 5.2). No doc describes one of them.</summary>
    public static IReadOnlyList<KindTextParameters<SettlementKind>> SettlementKinds { get; } =
    [
        K(SettlementKind.Town, "Town"),
        K(SettlementKind.City, "City"),
    ];

    /// <summary>The tiers of a place, lowest first (society-generation.md section 8). No doc describes one of them.</summary>
    public static IReadOnlyList<KindTextParameters<SettlementTier>> SettlementTiers { get; } =
    [
        K(SettlementTier.Countryside, "Countryside"),
        K(SettlementTier.Town, "Town"),
        K(SettlementTier.City, "City"),
        K(SettlementTier.Capital, "Capital"),
    ];
}
