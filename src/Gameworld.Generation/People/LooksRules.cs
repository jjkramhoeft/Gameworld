using Gameworld.Domain;

namespace Gameworld.Generation.People;

/// <summary>
/// What the portrait components ask about a person: who can wear a hairstyle, an outfit or an
/// accessory (society-generation.md section 9.3).
/// </summary>
/// <param name="Race">The id of the race of the person.</param>
/// <param name="Sex">The sex of the person.</param>
/// <param name="Age">The age of the person, in years.</param>
/// <param name="Wealth">The wealth of the household of the person.</param>
/// <param name="Married">Whether the husband or the wife of the person lives in the household.</param>
/// <param name="Vocation">The id of the vocation of the person. Null without one.</param>
public readonly record struct Wearer(string Race, Sex Sex, int Age, WealthLevel Wealth, bool Married, string? Vocation)
{
    /// <summary>The wearer a person of a household is.</summary>
    public static Wearer Of(Person person, Household household) =>
        new(person.Race, person.Sex, person.Age, household.Wealth, person.Spouse != null, person.Vocation);
}

/// <summary>
/// The portrait components (society-generation.md section 9): the skin, eye and hair colours,
/// the hairstyles, the outfits and the accessories. It checks the six lists of the Parameter
/// Set when the world is created and answers which of them a person can have. Nothing is drawn
/// here: the looks of a person are a later step that reads these answers.
/// </summary>
public sealed class LooksRules
{
    private readonly Dictionary<string, SkinColourParameters[]> _skinColoursOf = new(StringComparer.Ordinal);
    private readonly Dictionary<string, EyeColourParameters[]> _eyeColoursOf = new(StringComparer.Ordinal);
    private readonly Dictionary<string, HairColourParameters[]> _hairColoursOf = new(StringComparer.Ordinal);
    private readonly Dictionary<string, HairColourParameters> _hairColours = new(StringComparer.Ordinal);
    private readonly Dictionary<string, ColourParameters> _colours = new(StringComparer.Ordinal);
    private readonly HairstyleParameters[] _hairstyles;
    private readonly OutfitParameters[] _outfits;
    private readonly AccessoryParameters[] _accessories;

    /// <exception cref="ArgumentException">
    /// An entry of one of the six lists has no id or shares its id; a race lists a colour that is
    /// not in its list; a hair colour greys into a colour that is not in the list, or the chain
    /// does not end; a hairstyle, an outfit or an accessory is worn by a race or a vocation that
    /// is not in the list, or its wealth or its ages run backwards; an outfit names a colour that
    /// is not in the Colours list.
    /// </exception>
    public LooksRules(ParameterSet parameters)
    {
        var skin = ById(parameters.SkinColours, c => c.Id, "skin colour", parameters);
        var eyes = ById(parameters.EyeColours, c => c.Id, "eye colour", parameters);
        foreach (var (id, colour) in ById(parameters.HairColours, c => c.Id, "hair colour", parameters)) _hairColours[id] = colour;
        foreach (var colour in parameters.Colours)
            if (colour.Id != null) _colours.TryAdd(colour.Id, colour);

        // A hair colour greys into another colour of the list, and the greying ends.
        foreach (var colour in _hairColours.Values)
        {
            var seen = new HashSet<string>(StringComparer.Ordinal) { colour.Id };
            for (var next = colour.Greying; next != null; next = _hairColours[next].Greying)
            {
                if (!_hairColours.ContainsKey(next))
                    throw new ArgumentException($"The hair colour \"{colour.Id}\" greys into \"{next}\", which is not a hair colour of the Parameter Set.", nameof(parameters));
                if (!seen.Add(next))
                    throw new ArgumentException($"The greying of the hair colour \"{colour.Id}\" does not end: it comes back to \"{next}\".", nameof(parameters));
            }
        }

        // The colours of a race are colours of the three lists.
        var races = new HashSet<string>(StringComparer.Ordinal);
        foreach (var race in parameters.Races)
        {
            if (race.Id is null) continue; // the population rules refuse a race without an id
            races.Add(race.Id);
            _skinColoursOf[race.Id] = Listed(race.SkinColours, skin, race.Id, nameof(race.SkinColours), "skin colour", parameters);
            _eyeColoursOf[race.Id] = Listed(race.EyeColours, eyes, race.Id, nameof(race.EyeColours), "eye colour", parameters);
            _hairColoursOf[race.Id] = Listed(race.HairColours, _hairColours, race.Id, nameof(race.HairColours), "hair colour", parameters);
        }

        var vocations = parameters.Vocations.Select(v => v.Id).Where(id => id != null).ToHashSet(StringComparer.Ordinal);
        foreach (var hairstyle in ById(parameters.Hairstyles, h => h.Id, "hairstyle", parameters).Values)
            Check(hairstyle.WornBy, $"The hairstyle \"{hairstyle.Id}\"", races, vocations, parameters);
        foreach (var outfit in ById(parameters.Outfits, o => o.Id, "outfit", parameters).Values)
        {
            Check(outfit.WornBy, $"The outfit \"{outfit.Id}\"", races, vocations, parameters);
            foreach (var (colour, name) in new[] { (outfit.MainColour, nameof(outfit.MainColour)), (outfit.AccentColour, nameof(outfit.AccentColour)) })
                if (colour != null && !_colours.ContainsKey(colour))
                    throw new ArgumentException($"The outfit \"{outfit.Id}\" has the {name} \"{colour}\", which is not a colour of the Parameter Set.", nameof(parameters));
        }
        foreach (var accessory in ById(parameters.Accessories, a => a.Id, "accessory", parameters).Values)
        {
            Check(accessory.WornBy, $"The accessory \"{accessory.Id}\"", races, vocations, parameters);
            if (!Enum.IsDefined(accessory.Kind))
                throw new ArgumentException($"The accessory \"{accessory.Id}\" has the kind {accessory.Kind}, which is not a kind of accessory.", nameof(parameters));
        }

        // An entry that is off is worn by nobody.
        _hairstyles = parameters.Hairstyles.Where(h => h.Enabled).ToArray();
        _outfits = parameters.Outfits.Where(o => o.Enabled).ToArray();
        _accessories = parameters.Accessories.Where(a => a.Enabled).ToArray();
    }

    /// <summary>The skin colours a person of a race can have, the commonest first. Empty for a race that lists none or is not in the list.</summary>
    public IReadOnlyList<SkinColourParameters> SkinColoursOf(string raceId) =>
        _skinColoursOf.TryGetValue(raceId, out var colours) ? colours : [];

    /// <summary>The eye colours a person of a race can have, the commonest first. Empty for a race that lists none or is not in the list.</summary>
    public IReadOnlyList<EyeColourParameters> EyeColoursOf(string raceId) =>
        _eyeColoursOf.TryGetValue(raceId, out var colours) ? colours : [];

    /// <summary>The hair colours a person of a race can have before the hair greys, the commonest first. Empty for a race without hair or not in the list.</summary>
    public IReadOnlyList<HairColourParameters> HairColoursOf(string raceId) =>
        _hairColoursOf.TryGetValue(raceId, out var colours) ? colours : [];

    /// <summary>The hair colour a hair colour turns into as the person greys. Null for one that does not change, or is not in the list.</summary>
    public HairColourParameters? GreyingOf(string hairColourId) =>
        _hairColours.TryGetValue(hairColourId, out var colour) && colour.Greying != null ? _hairColours[colour.Greying] : null;

    /// <summary>The hairstyles a person can wear, in the order of the Hairstyles list: those that are on and fit the person.</summary>
    public IReadOnlyList<HairstyleParameters> HairstylesOf(Wearer wearer) =>
        _hairstyles.Where(h => Fits(h.WornBy, wearer)).ToArray();

    /// <summary>The outfits a person can wear, in the order of the Outfits list: those that are on and fit the person.</summary>
    public IReadOnlyList<OutfitParameters> OutfitsOf(Wearer wearer) =>
        _outfits.Where(o => Fits(o.WornBy, wearer)).ToArray();

    /// <summary>The accessories a person can wear, in the order of the Accessories list: those that are on and fit the person. A person wears at most one of a kind.</summary>
    public IReadOnlyList<AccessoryParameters> AccessoriesOf(Wearer wearer) =>
        _accessories.Where(a => Fits(a.WornBy, wearer)).ToArray();

    /// <summary>
    /// Whether a person can wear what the limits are of: every limit holds for the person. A limit
    /// that is left out holds for everybody.
    /// </summary>
    public static bool Fits(WearerParameters? wornBy, Wearer wearer)
    {
        if (wornBy is null) return true;
        if (wornBy.Sex is { } sex && sex != wearer.Sex) return false;
        if (wornBy.Races is { Count: > 0 } races && !races.Contains(wearer.Race, StringComparer.Ordinal)) return false;
        if (wearer.Wealth < wornBy.LowestWealth || wearer.Wealth > wornBy.HighestWealth) return false;
        if (wornBy.Vocations is { Count: > 0 } vocations && (wearer.Vocation is null || !vocations.Contains(wearer.Vocation, StringComparer.Ordinal))) return false;
        if (wearer.Age < wornBy.LowestAge || wearer.Age > wornBy.HighestAge) return false;
        return wornBy.Married is not { } married || married == wearer.Married;
    }

    /// <summary>
    /// What a picture shows of an outfit on a person: its description with the names of the two
    /// colours in the place of the marks. A colour the outfit decides is used before the one of
    /// the person, and a colour that is not in the Colours list is written as "undyed".
    /// </summary>
    public string Describe(OutfitParameters outfit, string? mainColourId, string? accentColourId) =>
        outfit.DescriptionIn(NameOf(outfit.MainColour ?? mainColourId), NameOf(outfit.AccentColour ?? accentColourId));

    private string? NameOf(string? colourId) =>
        colourId != null && _colours.TryGetValue(colourId, out var colour) ? colour.Name : null;

    private static string Article(string what) => what[0] is 'a' or 'e' or 'o' ? "an" : "a";

    private static Dictionary<string, T> ById<T>(IReadOnlyList<T> list, Func<T, string> idOf, string what, ParameterSet parameters)
    {
        var byId = new Dictionary<string, T>(StringComparer.Ordinal);
        foreach (var entry in list)
        {
            var id = idOf(entry);
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException($"{(Article(what) == "an" ? "An" : "A")} {what} of the Parameter Set has no id.", nameof(parameters));
            if (!byId.TryAdd(id, entry))
                throw new ArgumentException($"The Parameter Set lists the {what} id \"{id}\" more than once.", nameof(parameters));
        }
        return byId;
    }

    private static T[] Listed<T>(IReadOnlyList<string>? ids, Dictionary<string, T> byId, string race, string list, string what, ParameterSet parameters)
    {
        var listed = new List<T>();
        foreach (var id in ids ?? [])
        {
            if (id is null || !byId.TryGetValue(id, out var entry))
                throw new ArgumentException($"The race \"{race}\" lists \"{id}\" in {list}, which is not {Article(what)} {what} of the Parameter Set.", nameof(parameters));
            listed.Add(entry);
        }
        return listed.ToArray();
    }

    private static void Check(WearerParameters? wornBy, string what, HashSet<string> races, HashSet<string> vocations, ParameterSet parameters)
    {
        if (wornBy is null) return;
        foreach (var race in wornBy.Races ?? [])
            if (race is null || !races.Contains(race))
                throw new ArgumentException($"{what} lists \"{race}\" in WornBy.Races, which is not a race of the Parameter Set.", nameof(parameters));
        foreach (var vocation in wornBy.Vocations ?? [])
            if (vocation is null || !vocations.Contains(vocation))
                throw new ArgumentException($"{what} lists \"{vocation}\" in WornBy.Vocations, which is not a vocation of the Parameter Set.", nameof(parameters));
        if (!Enum.IsDefined(wornBy.LowestWealth) || !Enum.IsDefined(wornBy.HighestWealth) || wornBy.LowestWealth > wornBy.HighestWealth)
            throw new ArgumentException($"{what} is worn from {wornBy.LowestWealth} to {wornBy.HighestWealth}: the lowest wealth must not be above the highest.", nameof(parameters));
        if (wornBy.LowestAge < 0 || wornBy.HighestAge < 0 || wornBy.LowestAge > wornBy.HighestAge)
            throw new ArgumentException($"{what} is worn from the age {wornBy.LowestAge} to {wornBy.HighestAge}: the ages must be 0 or more, and the lowest not above the highest.", nameof(parameters));
    }
}
