using System.ComponentModel;

namespace Gameworld.Domain;

/// <summary>What binds the members of a faction (society-generation.md section 4.1).</summary>
public enum FactionType
{
    /// <summary>Blood and marriage: a house and its household.</summary>
    Family,
    /// <summary>A people: one race, its clans and its customs.</summary>
    Race,
    /// <summary>Work: a trade, its masters and its monopoly.</summary>
    Vocation,
    /// <summary>A faith: a temple, an order, a sect.</summary>
    Belief,
    /// <summary>The arcane: a college, a circle, a cabal.</summary>
    Magic,
    /// <summary>A home: a town or a valley that rules itself.</summary>
    Place,
}

/// <summary>How a faction stands in its region (society-generation.md section 4.2).</summary>
public enum FactionRole
{
    /// <summary>It governs the region. A region with persons has exactly one.</summary>
    Ruling,
    /// <summary>It wants the rule, or wants out of it. A region has one or none.</summary>
    Opposition,
    /// <summary>It lives under the ruler: a guild, an order, a circle, a great house, a chartered town, an enclave.</summary>
    Lesser,
}

/// <summary>
/// The form of a faction: the name of a type in a role. It is a label for the authoring; no
/// rule reads it. It is race-neutral: a crown of the Northmen is a jarldom.
/// </summary>
public enum FactionForm
{
    /// <summary>A ruling house.</summary>
    Crown,
    /// <summary>A rival house in opposition, or a great house under the ruler.</summary>
    House,
    /// <summary>A herding people of open land.</summary>
    Horde,
    /// <summary>A people under its chiefs.</summary>
    Clan,
    /// <summary>A people in rough land with ore.</summary>
    Hold,
    /// <summary>A people that lives under the ruler of another.</summary>
    Enclave,
    /// <summary>Rule by the guilds of the trading towns.</summary>
    MerchantRepublic,
    /// <summary>A guild of one trade, in opposition or under the ruler.</summary>
    Guild,
    /// <summary>Rule by a faith.</summary>
    TempleState,
    /// <summary>A faith of the poor against the ruler.</summary>
    Sect,
    /// <summary>A faith with a rule of its own, in opposition or under the ruler.</summary>
    Order,
    /// <summary>Rule by the mages.</summary>
    Magocracy,
    /// <summary>The mages against the ruler.</summary>
    Cabal,
    /// <summary>The mages under the ruler.</summary>
    Circle,
    /// <summary>Towns that rule themselves, as the ruler or against it.</summary>
    FreeCity,
    /// <summary>Free farmers that rule themselves, as the ruler or against it.</summary>
    Commune,
    /// <summary>A town with a charter of its own under the ruler.</summary>
    CharteredTown,
}

/// <summary>What an opposition stands on: the largest weighted term of the tension (society-generation.md section 4.6).</summary>
public enum OppositionGround
{
    /// <summary>The land and the races would as well carry another kind of ruler.</summary>
    Contest,
    /// <summary>Hills and mountains shelter those who do not obey.</summary>
    RoughLand,
    /// <summary>A second city nearly as good as the first, and far from it.</summary>
    RivalSeat,
    /// <summary>Silver, gold and gems pay for a rising and are worth one.</summary>
    PreciousFinds,
    /// <summary>Poor Prospects, and rich and poor Prospects side by side.</summary>
    PoorLand,
}

/// <summary>
/// A faction as a Stub: a group of people in one region, a political grouping. Its identity is
/// the region and the slot. Everything in it is derived data.
/// </summary>
/// <param name="Slot">The slot in the region: 0 the ruling faction, 1 the opposition, 2 and up the lesser factions. Slot 1 is empty in a region without an opposition.</param>
/// <param name="LeadRace">The id of the race of its leaders. A faction of the type Race holds that race only; any other faction is open to the races of its seat.</param>
/// <param name="Seat">The patch of the Prospect the faction sits on. Null in a region without a Prospect on show.</param>
/// <param name="Kind">The id of the kind of a lesser faction, and of the trade of a guild in opposition. Null otherwise.</param>
/// <param name="Vocations">The ids of the vocations the faction gathers, for a guild, an order and a circle. Empty otherwise.</param>
/// <param name="Ground">What an opposition stands on. Null for the other roles.</param>
/// <param name="Grip">0 to 1. The ruling faction: 1 − the tension of the region. The opposition: the tension. A lesser faction: its fit.</param>
/// <param name="MainColour">The id of its main colour: the field of its banner and the cloth of its livery. Null when the Parameter Set has no colours.</param>
/// <param name="SecondaryColour">The id of its secondary colour: the device on the banner and the trim of the livery. Never the main colour. Null when the Parameter Set has fewer than two colours.</param>
public sealed record Faction(
    int Slot,
    FactionRole Role,
    FactionType Type,
    FactionForm Form,
    string LeadRace,
    PatchId? Seat,
    string? Kind,
    IReadOnlyList<string> Vocations,
    OppositionGround? Ground,
    double Grip,
    string? MainColour = null,
    string? SecondaryColour = null)
{
    /// <summary>The slot of the ruling faction.</summary>
    public const int RulingSlot = 0;
    /// <summary>The slot of the opposition.</summary>
    public const int OppositionSlot = 1;
    /// <summary>The slot of the first lesser faction.</summary>
    public const int FirstLesserSlot = 2;

    /// <summary>The identity of the faction: its region and its slot.</summary>
    public SlotId<RegionId> IdIn(RegionId region) => new(region, Slot);
}

/// <summary>One value per type of faction.</summary>
public sealed record PerFactionType<T>(
    [property: Description("Value for the type Family: a house and its household.")] T Family,
    [property: Description("Value for the type Race: a people.")] T Race,
    [property: Description("Value for the type Vocation: a trade.")] T Vocation,
    [property: Description("Value for the type Belief: a faith.")] T Belief,
    [property: Description("Value for the type Magic: the arcane.")] T Magic,
    [property: Description("Value for the type Place: a town or a valley that rules itself.")] T Place)
{
    public T this[FactionType type] => type switch
    {
        FactionType.Family => Family,
        FactionType.Race => Race,
        FactionType.Vocation => Vocation,
        FactionType.Belief => Belief,
        FactionType.Magic => Magic,
        FactionType.Place => Place,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
    };
}
