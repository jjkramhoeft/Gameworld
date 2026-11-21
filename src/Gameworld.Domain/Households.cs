using System.ComponentModel;

namespace Gameworld.Domain;

/// <summary>
/// The wealth of a household, lowest first (society-generation.md section 6). A household shares
/// its wealth. A level fits the standing of a vocation: Destitute 1, Poor 2, Common 3,
/// Comfortable 4 and Rich 5 and 6.
/// </summary>
public enum WealthLevel
{
    /// <summary>Lives on alms or outside the law.</summary>
    Destitute,
    /// <summary>Sells unskilled labour and owns no tools of a trade.</summary>
    Poor,
    /// <summary>Holds land or a simple trade.</summary>
    Common,
    /// <summary>A skilled trade, a shop or a small office.</summary>
    Comfortable,
    /// <summary>Capital, learning, high office or rule.</summary>
    Rich,
}

/// <summary>The sex of a person.</summary>
public enum Sex
{
    Male,
    Female,
}

/// <summary>The position of a person in its household (society-generation.md section 7).</summary>
public enum HouseholdRole
{
    /// <summary>The head of the household. Every household has exactly one.</summary>
    Head,
    /// <summary>The husband or the wife of the head.</summary>
    Spouse,
    /// <summary>A child of the head.</summary>
    Child,
    /// <summary>The father or the mother of the head.</summary>
    Parent,
    /// <summary>A brother or a sister of the head.</summary>
    Sibling,
    /// <summary>A servant: of the household, not of the family.</summary>
    Servant,
}

/// <summary>One value per wealth level.</summary>
public sealed record PerWealthLevel<T>(
    [property: Description("Value for a Destitute household: it lives on alms or outside the law.")] T Destitute,
    [property: Description("Value for a Poor household: unskilled labour.")] T Poor,
    [property: Description("Value for a Common household: land or a simple trade.")] T Common,
    [property: Description("Value for a Comfortable household: a skilled trade, a shop or a small office.")] T Comfortable,
    [property: Description("Value for a Rich household: capital, learning, high office or rule.")] T Rich)
{
    public T this[WealthLevel level] => level switch
    {
        WealthLevel.Destitute => Destitute,
        WealthLevel.Poor => Poor,
        WealthLevel.Common => Common,
        WealthLevel.Comfortable => Comfortable,
        WealthLevel.Rich => Rich,
        _ => throw new ArgumentOutOfRangeException(nameof(level), level, null),
    };
}

/// <summary>
/// How the head of a household is kin of the head of another household of the same house
/// (society-generation.md section 6).
/// </summary>
/// <param name="Household">The slot of the other household: the first household of the house.</param>
/// <param name="Relation">What the head of this household is to the head of the other: Child or Sibling.</param>
public sealed record HouseholdKin(int Household, HouseholdRole Relation);

/// <summary>
/// A household as a Stub: a family and its servants, living in one city or town
/// (society-generation.md section 6). A household shares its wealth and its faction. Its
/// identity is the region and the slot. Everything in it is derived data.
/// </summary>
/// <param name="Slot">The slot in the region: the index among the households of the region. The households of the factions come first, by faction and round, then the others by the rank of their city or town.</param>
/// <param name="Settlement">The slot of the city or town the household lives in.</param>
/// <param name="Faction">The slot of the faction the household belongs to. Null for a household of no faction.</param>
/// <param name="Race">The id of the race of the family.</param>
/// <param name="Surname">The surname of the family. The households of one faction of the type Family are one house and share it.</param>
/// <param name="HeadSex">The sex of the head of the household.</param>
/// <param name="HeadAge">The age of the head of the household, in years.</param>
/// <param name="Family">How many persons the family holds, the head included. At least 1.</param>
/// <param name="Servants">The servants of the household, per race, in the order of the races of the city or town. Empty without servants.</param>
/// <param name="Kin">The household is a branch of a house: how its head is kin of the head of the first household of the house. Null otherwise.</param>
public sealed record Household(
    int Slot,
    int Settlement,
    int? Faction,
    string Race,
    WealthLevel Wealth,
    string Surname,
    Sex HeadSex,
    int HeadAge,
    int Family,
    IReadOnlyList<RacePersons> Servants,
    HouseholdKin? Kin = null)
{
    /// <summary>How many servants the household holds.</summary>
    public long ServantCount => Servants.Sum(s => s.Persons);

    /// <summary>How many persons the household holds: the family and the servants.</summary>
    public long Persons => Family + ServantCount;

    /// <summary>The identity of the household: its region and its slot.</summary>
    public SlotId<RegionId> IdIn(RegionId region) => new(region, Slot);
}

/// <summary>What the Housed stage adds: the households of the cities and towns of the region.</summary>
/// <param name="Households">The households of the region, by slot. Those of a city or a town hold exactly its persons, race by race.</param>
public sealed record RegionHouseholds(IReadOnlyList<Household> Households)
{
    /// <summary>The households of one faction, by slot.</summary>
    public IEnumerable<Household> Of(Faction faction) => Households.Where(h => h.Faction == faction.Slot);

    /// <summary>The households of one city or town, by slot.</summary>
    public IEnumerable<Household> In(Settlement settlement) => Households.Where(h => h.Settlement == settlement.Slot);
}

/// <summary>
/// A person as a Stub (society-generation.md section 7). Its identity is its household and its
/// slot in it. Everything in it is derived data.
/// </summary>
/// <param name="Household">The slot of the household of the person.</param>
/// <param name="Slot">The slot in the household: 0 the head, then the family, then the servants.</param>
/// <param name="GivenName">The given name, from the name generator in the name style of the race.</param>
/// <param name="Surname">The surname: that of the household for the family, its own for a servant.</param>
/// <param name="Race">The id of the race of the person.</param>
/// <param name="Age">The age in years.</param>
/// <param name="Role">The position of the person in the household.</param>
/// <param name="Vocation">The id of the vocation of the person. Null for a child and where no vocation fits.</param>
/// <param name="Spouse">The slot in the household of the husband or the wife. Null without one in the household.</param>
/// <param name="Father">The slot in the household of the father. Null when he does not live in it.</param>
/// <param name="Mother">The slot in the household of the mother. Null when she does not live in it.</param>
/// <param name="Looks">The looks of the person: its portrait components (society-generation.md section 9), drawn with the person. Null on a person that was not generated.</param>
public sealed record Person(
    int Household,
    int Slot,
    string GivenName,
    string Surname,
    string Race,
    Sex Sex,
    int Age,
    HouseholdRole Role,
    string? Vocation,
    int? Spouse = null,
    int? Father = null,
    int? Mother = null,
    Looks? Looks = null)
{
    /// <summary>The given name and the surname.</summary>
    public string Name => GivenName + " " + Surname;

    /// <summary>The identity of the person: its household and its slot in it.</summary>
    public SlotId<SlotId<RegionId>> IdIn(RegionId region) => new(new SlotId<RegionId>(region, Household), Slot);
}

/// <summary>What the Peopled stage adds: the persons of every household of the region.</summary>
/// <param name="Persons">The persons of the region, by household and by slot in it.</param>
public sealed record RegionPeople(IReadOnlyList<Person> Persons)
{
    /// <summary>The persons of one household, by slot.</summary>
    public IEnumerable<Person> Of(Household household) => Persons.Where(p => p.Household == household.Slot);
}
