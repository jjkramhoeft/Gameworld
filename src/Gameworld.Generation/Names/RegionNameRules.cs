using Gameworld.Domain;
using Gameworld.Generation.Basics;

namespace Gameworld.Generation.Names;

/// <summary>The name of a region and what it was drawn from.</summary>
/// <param name="Name">The name of the region.</param>
/// <param name="RulingHouse">The surname of the ruling house; null when the ruling faction is not of the type Family.</param>
/// <param name="Human">The lead race of the ruling faction is human: the name comes from the word lists of the human races.</param>
/// <param name="Size">How large the region is to the generator.</param>
public sealed record RegionNaming(string Name, string? RulingHouse, bool Human, RegionNameSize Size);

/// <summary>
/// The name of a region (society-generation.md section 4.10): what the region-name generator
/// is given for a region. The lead race of the ruling faction picks the word lists, the persons
/// of the region its size, and a ruling faction of the type Family is a house: it brings its
/// surname, in the name style of its lead race, and the suffixes of a house. A pure function of the region's own ruling faction and
/// persons.
/// </summary>
public sealed class RegionNameRules
{
    /// <summary>Hash purpose of the draws for the name of a region.</summary>
    public const uint NamePurpose = 0x4b07;
    /// <summary>Hash purpose of the draws for the surname of the ruling house.</summary>
    public const uint HousePurpose = 0x4b08;

    /// <summary>A surname longer than this is drawn again.</summary>
    public const int HouseNameMaxLength = 12;
    /// <summary>How many surnames are drawn at most; the last one is kept whatever its length.</summary>
    public const int HouseNameAttempts = 10;

    private readonly RegionNameParameters _parameters;
    private readonly Dictionary<string, RaceParameters> _races = new(StringComparer.Ordinal);
    private readonly uint _seed;

    /// <param name="seed">The seed of the foundation: it gives the draws.</param>
    public RegionNameRules(ParameterSet parameters, uint seed = 0)
    {
        _parameters = parameters.Planning.Names;
        foreach (var race in parameters.Races) _races.TryAdd(race.Id, race);
        _seed = seed;
    }

    /// <summary>Small below the small limit, large above the large limit, middling between. Small wins where the limits cross.</summary>
    public RegionNameSize SizeOf(long persons) =>
        persons < _parameters.SmallRegionPersons ? RegionNameSize.Small
        : persons > _parameters.LargeRegionPersons ? RegionNameSize.Large
        : RegionNameSize.Middling;

    /// <summary>Whether a race is human; false for an id that is not a race of the Parameter Set.</summary>
    public bool IsHuman(string race) => _races.TryGetValue(race, out var r) && r.Human;

    /// <summary>The name style of a race; Common for an id that is not a race of the Parameter Set.</summary>
    public NameStyle StyleOf(string race) => _races.TryGetValue(race, out var r) ? r.NameStyle : NameStyle.Common;

    /// <summary>
    /// The surname of the house that rules a region, from the surname model of a name style:
    /// the style of the lead race of the house. A surname of more than
    /// <see cref="HouseNameMaxLength"/> characters is drawn again from the same stream.
    /// </summary>
    public string RulingHouse(GridCoord region, NameStyle style) =>
        Surname(style, new Sfc32(Hashing.Hash4(_seed, HousePurpose, region.I, region.J)));

    /// <summary>
    /// A surname from the surname model of a name style. A surname of more than
    /// <see cref="HouseNameMaxLength"/> characters, or one that is <paramref name="taken"/>, is
    /// drawn again from the same stream, <see cref="HouseNameAttempts"/> times at most: the last
    /// one is kept whatever it is.
    /// </summary>
    public static string Surname(NameStyle style, Sfc32 rng, Func<string, bool>? taken = null)
    {
        var name = "";
        for (var attempt = 0; attempt < HouseNameAttempts; attempt++)
        {
            name = NameGenerator.Default.Generate(style, NameKind.Surname, rng);
            if (name.Length <= HouseNameMaxLength && taken?.Invoke(name) != true) break;
        }
        return name;
    }

    /// <summary>The name of a region; null for a region without a ruling faction.</summary>
    /// <param name="region">The region: the identity the draws are seeded from.</param>
    /// <param name="ruling">The ruling faction of the region, or null.</param>
    /// <param name="persons">The potential population of the region.</param>
    public RegionNaming? Name(GridCoord region, Faction? ruling, long persons)
    {
        if (ruling is null) return null;
        var human = IsHuman(ruling.LeadRace);
        var size = SizeOf(persons);
        var house = ruling.Type == FactionType.Family ? RulingHouse(region, StyleOf(ruling.LeadRace)) : null;
        var ruler = house is null ? RegionNameRuler.Other : RegionNameRuler.House;
        var rng = new Sfc32(Hashing.Hash4(_seed, NamePurpose, region.I, region.J));
        return new RegionNaming(RegionNameGenerator.Generate(rng, human, size, ruler, house), house, human, size);
    }
}
