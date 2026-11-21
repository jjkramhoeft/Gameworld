using Gameworld.Domain;
using Gameworld.Generation.Basics;
using Gameworld.Generation.Households;

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
/// Set when the world is created, answers which of them a person can have, and draws the looks
/// of the persons of a household (section 9.7): a pure function of the household, its faction
/// and its persons, each person from a stream of its own.
/// </summary>
public sealed class LooksRules
{
    /// <summary>Hash purpose of the draws for the looks of a person.</summary>
    public const uint PersonPurpose = 0x4e03;
    /// <summary>Hash purpose of the draws for the colours of a father or a mother who does not live in the household.</summary>
    public const uint AbsentParentPurpose = 0x4e04;

    /// <summary>The hair of a person starts to grey at an age drawn evenly from this range.</summary>
    public const int GreyingFrom = 35, GreyingTo = 55;
    /// <summary>The hair takes a further step of its greying every this many years.</summary>
    public const int GreyingStep = 12;
    /// <summary>The weight of a hairstyle, an outfit or an accessory that is the dress or the mark of the vocation of the person. Any other weighs 1.</summary>
    public const double VocationWeight = 4;
    /// <summary>The weight of the first favourite colour of the race of the person when a colour of its outfit is drawn. The second has half of it, the third a third.</summary>
    public const double FavouriteColourWeight = 4;
    /// <summary>The weight of a colour that is not a favourite colour of the race of the person.</summary>
    public const double OtherColourWeight = 0.2;
    /// <summary>How much the lightness of the accent colour must differ from that of the main colour, where a colour does.</summary>
    public const double ColourContrast = 0.25;
    /// <summary>The chance that a person of a household of a faction wears the colours of the faction.</summary>
    public const double LiveryChance = 0.5;
    /// <summary>Nobody wears more accessories than this.</summary>
    public const int MostAccessories = 3;
    /// <summary>How many numbers the stream of a person gives: every person takes them all, in a fixed order.</summary>
    private const int NumbersPerPerson = 10 + MostAccessories;

    /// <summary>How many accessories a person of a household of a wealth wears: a count, and the chance of one more.</summary>
    public static (int Count, double OneMoreChance) AccessoryCountOf(WealthLevel wealth) => wealth switch
    {
        WealthLevel.Destitute => (0, 0.1),
        WealthLevel.Poor => (0, 0.3),
        WealthLevel.Common => (0, 0.5),
        WealthLevel.Comfortable => (1, 0.5),
        _ => (2, 0.5),
    };

    private readonly uint _seed;
    private readonly (string Id, double Lightness)[] _palette;
    private readonly Dictionary<string, int[]> _favouriteOf = new(StringComparer.Ordinal);
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
    /// <param name="seed">The seed of the foundation: it gives the draws.</param>
    public LooksRules(ParameterSet parameters, uint seed = 0)
    {
        _seed = seed;
        // The colours an outfit can be drawn in: those with an id and a value, as for the factions (section 4.9).
        _palette = parameters.Colours
            .Select(c => (c.Id, Known: ColourParameters.TryLightness(c.Hex, out var lightness), Lightness: lightness))
            .Where(c => c.Known && !string.IsNullOrWhiteSpace(c.Id))
            .DistinctBy(c => c.Id)
            .Select(c => (c.Id, c.Lightness))
            .ToArray();
        foreach (var race in parameters.Races)
        {
            if (race.Id is null) continue;
            // The place of a colour that is listed twice is its first one.
            var favourites = (race.FavouriteColours ?? []).Where(id => Array.Exists(_palette, c => c.Id == id)).Distinct().ToArray();
            _favouriteOf.TryAdd(race.Id, _palette.Select(c => Array.IndexOf(favourites, c.Id) + 1).ToArray());
        }

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

    // ───────────────────────── The draw (section 9.7) ─────────────────────────

    /// <summary>The colours a person is born with: of the skin, of the eyes and of the hair before it greys.</summary>
    private readonly record struct Colouring(string? Skin, string? Eyes, string? Hair);

    /// <summary>
    /// The hair colour a picture shows at an age: the natural colour until the hair starts to
    /// grey, then one step along its greying, and a further step every twelve years, to the end
    /// of the chain. A colour that does not grey stays.
    /// </summary>
    /// <param name="naturalHairColourId">The id of the hair colour before it greys. Null for a person without hair.</param>
    /// <param name="greyingAge">The age at which the hair of the person starts to grey.</param>
    public string? HairColourAt(string? naturalHairColourId, int age, int greyingAge)
    {
        if (naturalHairColourId is null || age < greyingAge) return naturalHairColourId;
        var colour = naturalHairColourId;
        for (var steps = 1 + (age - greyingAge) / GreyingStep; steps > 0; steps--)
        {
            if (!_hairColours.TryGetValue(colour, out var entry) || entry.Greying is null) break;
            colour = entry.Greying;
        }
        return colour;
    }

    /// <summary>
    /// The looks of the persons of one household (society-generation.md section 9.7), by slot.
    /// The head, its siblings and its children take each colour of skin, eyes and hair from their
    /// father or their mother; everybody else has the colours of the race. The hairstyle, the
    /// outfit, its two colours and the accessories are drawn among what the person can wear.
    /// </summary>
    /// <param name="region">The region: the identity the draws are seeded from.</param>
    /// <param name="household">The household: its wealth and its race.</param>
    /// <param name="faction">The faction of the household: its colours. Null for a household of no faction.</param>
    /// <param name="persons">The persons of the household, by slot.</param>
    public IReadOnlyList<Looks> LooksOf(GridCoord region, Household household, Faction? faction, IReadOnlyList<Person> persons)
    {
        var numbers = new double[persons.Count][];
        for (var slot = 0; slot < persons.Count; slot++)
        {
            var rng = new Sfc32(HouseholdRules.Key(_seed, PersonPurpose, region, household.Slot, slot));
            var u = numbers[slot] = new double[NumbersPerPerson];
            for (var n = 0; n < u.Length; n++) u[n] = rng.Next();
        }

        // A father or a mother who does not live in the household has the colours of the race of
        // the family, drawn once for each of the two couples: 0 the parents of the head, 1 the
        // head and its husband or wife. So brothers and sisters share them.
        var absent = new (Colouring Father, Colouring Mother)?[2];
        (Colouring Father, Colouring Mother) Absent(int couple)
        {
            if (absent[couple] is { } known) return known;
            var rng = new Sfc32(HouseholdRules.Key(_seed, AbsentParentPurpose, region, household.Slot, couple));
            var father = OfRace(household.Race, rng.Next(), rng.Next(), rng.Next());
            var mother = OfRace(household.Race, rng.Next(), rng.Next(), rng.Next());
            absent[couple] = (father, mother);
            return (father, mother);
        }

        var born = new Colouring?[persons.Count];
        var asked = new bool[persons.Count]; // the persons whose colours are being settled: a link back to one of them is not followed
        Colouring Born(int slot)
        {
            if (born[slot] is { } known) return known;
            var person = persons[slot];
            var u = numbers[slot];
            Colouring colouring;
            if (person.Role is HouseholdRole.Head or HouseholdRole.Sibling or HouseholdRole.Child)
            {
                asked[slot] = true;
                var couple = person.Role == HouseholdRole.Child ? 1 : 0;
                var father = Living(person.Father, slot) is { } f ? Born(f) : Absent(couple).Father;
                var mother = Living(person.Mother, slot) is { } m ? Born(m) : Absent(couple).Mother;
                colouring = new Colouring(
                    u[0] < 0.5 ? father.Skin : mother.Skin,
                    u[1] < 0.5 ? father.Eyes : mother.Eyes,
                    u[2] < 0.5 ? father.Hair : mother.Hair);
            }
            else colouring = OfRace(person.Race, u[0], u[1], u[2]);
            born[slot] = colouring;
            return colouring;
        }
        int? Living(int? parent, int child) =>
            parent is { } slot && slot >= 0 && slot < persons.Count && slot != child && !(asked[slot] && born[slot] is null) ? slot : null;

        var looks = new Looks[persons.Count];
        for (var slot = 0; slot < persons.Count; slot++)
        {
            var person = persons[slot];
            var u = numbers[slot];
            var colouring = Born(slot);
            var wearer = Wearer.Of(person, household);

            var hairstyle = Pick(HairstylesOf(wearer), h => WeightOf(h.WornBy), u[4]);
            var outfit = Pick(OutfitsOf(wearer), o => WeightOf(o.WornBy), u[5]);
            var (main, accent) = outfit is null ? (null, null) : ColoursOf(outfit, person.Race, faction, u[6], u[7], u[8]);

            // The accessories: one after the other, never two of a kind.
            var (count, oneMore) = AccessoryCountOf(household.Wealth);
            if (u[9] < oneMore) count++;
            var open = AccessoriesOf(wearer).ToList();
            var worn = new List<string>();
            for (var n = 0; n < Math.Min(count, MostAccessories); n++)
            {
                if (Pick(open, a => WeightOf(a.WornBy), u[10 + n]) is not { } accessory) break;
                worn.Add(accessory.Id);
                open.RemoveAll(a => a.Kind == accessory.Kind);
            }

            looks[slot] = new Looks(
                colouring.Skin, colouring.Eyes,
                HairColourAt(colouring.Hair, person.Age, Hashing.Between(GreyingFrom, GreyingTo, u[3])), colouring.Hair,
                hairstyle?.Id, outfit?.Id, main, accent, worn);
        }
        return looks;
    }

    /// <summary>The colours of a person of a race: the k-th colour of each list of the race weighs 1/k.</summary>
    private Colouring OfRace(string raceId, double uSkin, double uEyes, double uHair) => new(
        Commonest(SkinColoursOf(raceId), uSkin)?.Id,
        Commonest(EyeColoursOf(raceId), uEyes)?.Id,
        Commonest(HairColoursOf(raceId), uHair)?.Id);

    private static T? Commonest<T>(IReadOnlyList<T> colours, double u) where T : class
    {
        var picked = PickIndex(colours.Count, k => 1.0 / (k + 1), u);
        return picked < 0 ? null : colours[picked];
    }

    /// <summary>The weight of what a person can wear: more for the dress or the mark of its vocation.</summary>
    private static double WeightOf(WearerParameters? wornBy) => wornBy?.Vocations is { Count: > 0 } ? VocationWeight : 1;

    /// <summary>
    /// The main and the accent colour of the outfit of a person. A colour the outfit decides
    /// stands. A person who wears the colours of its faction takes the main and the secondary
    /// colour of the faction. Any other colour is drawn, mostly from the favourite colours of the
    /// race, and the accent colour is one that stands out against the main colour.
    /// </summary>
    private (string? Main, string? Accent) ColoursOf(OutfitParameters outfit, string raceId, Faction? faction, double uLivery, double uMain, double uAccent)
    {
        var livery = faction?.MainColour != null && uLivery < LiveryChance;
        var favourite = _favouriteOf.GetValueOrDefault(raceId);
        double Weight(int c) => favourite != null && favourite[c] > 0 ? FavouriteColourWeight / favourite[c] : OtherColourWeight;

        var main = outfit.MainColour
            ?? (livery ? faction!.MainColour : null)
            ?? (_palette.Length > 0 ? _palette[PickIndex(_palette.Length, Weight, uMain)].Id : null);

        var accent = outfit.AccentColour ?? (livery && faction!.SecondaryColour != main ? faction.SecondaryColour : null);
        if (accent is null)
        {
            var at = Array.FindIndex(_palette, c => c.Id == main);
            bool Other(int c) => _palette[c].Id != main;
            bool StandsOut(int c) => Other(c) && (at < 0 || Math.Abs(_palette[c].Lightness - _palette[at].Lightness) >= ColourContrast);
            var allowed = Enumerable.Range(0, _palette.Length).Any(StandsOut) ? StandsOut : (Func<int, bool>)Other;
            var picked = PickIndex(_palette.Length, c => allowed(c) ? Weight(c) : 0, uAccent);
            accent = picked < 0 ? null : _palette[picked].Id;
        }
        return (main, accent);
    }

    /// <summary>One of the candidates by weight, in the order of the list; null when none has a weight.</summary>
    private static T? Pick<T>(IReadOnlyList<T> candidates, Func<T, double> weight, double u) where T : class
    {
        var weights = new double[candidates.Count];
        for (var k = 0; k < weights.Length; k++) weights[k] = weight(candidates[k]);
        var picked = PickIndex(weights.Length, k => weights[k], u);
        return picked < 0 ? null : candidates[picked];
    }

    /// <summary>
    /// An index by weight: the first one where the running sum of the weights passes u times the
    /// total. -1 when no index has a weight.
    /// </summary>
    private static int PickIndex(int count, Func<int, double> weight, double u)
    {
        var total = 0.0;
        for (var k = 0; k < count; k++) total += Math.Max(0, weight(k));
        if (!(total > 0)) return -1;
        var target = u * total;
        var sum = 0.0;
        var last = -1;
        for (var k = 0; k < count; k++)
        {
            var w = weight(k);
            if (!(w > 0)) continue;
            sum += w;
            last = k;
            if (target < sum) return k;
        }
        return last;
    }

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
