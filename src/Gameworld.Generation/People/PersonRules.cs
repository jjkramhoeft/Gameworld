using Gameworld.Domain;
using Gameworld.Generation.Basics;
using Gameworld.Generation.Households;
using Gameworld.Generation.Names;

namespace Gameworld.Generation.People;

/// <summary>What the persons of a household read of its city or town.</summary>
/// <param name="Tier">The tier of the place: Town, City or Capital.</param>
/// <param name="OnWater">The place is on water: a harbour, a river mouth or a navigable river.</param>
public readonly record struct HouseholdPlace(SettlementTier Tier, bool OnWater);

/// <summary>
/// The persons of the households of a region (society-generation.md section 7): for every
/// household its head, the family around the head and the servants, each with a name, a sex, an
/// age, a position in the household, family links, a vocation and looks. A pure function of the
/// region's own households, factions, cities and towns; a household is drawn from a stream of
/// its own, so no household depends on another.
/// </summary>
public sealed class PersonRules
{
    /// <summary>Hash purpose of the draws for the persons of a household: who they are, their ages and their vocations.</summary>
    public const uint HouseholdPurpose = 0x4e01;
    /// <summary>Hash purpose of the draws for the name of a person.</summary>
    public const uint NamePurpose = 0x4e02;

    /// <summary>A parent is at least this much older than its child.</summary>
    public const int ParentAgeGap = 16;
    /// <summary>A mother is at most this much older than a child of the head, and than the youngest of her children in the household.</summary>
    public const int MotherAgeSpan = 45;
    /// <summary>A parent of the head is at most this much older than the eldest of the head and its siblings.</summary>
    public const int ParentAgeSpan = 35;
    /// <summary>A husband or a wife is at least this old.</summary>
    public const int MinSpouseAge = 16;
    /// <summary>A husband is from this many years younger than his wife.</summary>
    public const int HusbandYounger = 3;
    /// <summary>A husband is up to this many years older than his wife.</summary>
    public const int HusbandOlder = 10;
    /// <summary>A sibling of the head is at most this much older or younger than the head.</summary>
    public const int SiblingAgeSpread = 10;
    /// <summary>A sibling of the head that lives in the household is at least this old.</summary>
    public const int MinSiblingAge = 14;
    /// <summary>Nobody is older than this.</summary>
    public const int MaxAge = 90;
    /// <summary>The youngest and the oldest servant.</summary>
    public const int MinServantAge = 12, MaxServantAge = 50;
    /// <summary>The weights of what a further person of the family is to the head.</summary>
    public const double ChildWeight = 6, ParentWeight = 1, SiblingWeight = 1;
    /// <summary>A given name that a person of the household already has is drawn again, this many times at most.</summary>
    public const int GivenNameAttempts = 5;

    /// <summary>
    /// The vocation of the head of a household of a faction that gathers no vocations, by the
    /// type of the faction: those who lead such a faction. Used when the vocation is on.
    /// </summary>
    private static readonly PerFactionType<string> LeaderVocations =
        new(Family: "noble", Race: "noble", Vocation: "merchant", Belief: "priest", Magic: "mage", Place: "official");

    private readonly PeopleParameters _parameters;
    private readonly LooksRules _looks;
    private readonly VocationParameters[] _vocations;
    private readonly Dictionary<string, VocationParameters> _byId = new(StringComparer.Ordinal);
    private readonly Dictionary<string, NameStyle> _styles = new(StringComparer.Ordinal);
    private readonly uint _seed;

    /// <param name="looks">The rules of the looks: they draw the looks of every person (section 9.7).</param>
    /// <param name="seed">The seed of the foundation: it gives the draws.</param>
    public PersonRules(ParameterSet parameters, LooksRules looks, uint seed = 0)
    {
        _parameters = parameters.People;
        _looks = looks;
        _vocations = parameters.Vocations.Where(v => v.Enabled).ToArray();
        foreach (var vocation in _vocations) _byId.TryAdd(vocation.Id, vocation);
        foreach (var race in parameters.Races) _styles.TryAdd(race.Id, race.NameStyle);
        _seed = seed;
    }

    /// <summary>The lowest and the highest standing a wealth level fits.</summary>
    public static (int Min, int Max) StandingOf(WealthLevel wealth) => wealth switch
    {
        WealthLevel.Destitute => (1, 1),
        WealthLevel.Poor => (2, 2),
        WealthLevel.Common => (3, 3),
        WealthLevel.Comfortable => (4, 4),
        _ => (5, 6),
    };

    /// <summary>Whether the standing of a vocation fits a wealth level.</summary>
    public static bool Fits(VocationParameters vocation, WealthLevel wealth)
    {
        var (min, max) = StandingOf(wealth);
        return vocation.Standing.Min <= max && vocation.Standing.Max >= min;
    }

    /// <summary>The tier of a city or a town.</summary>
    public static SettlementTier TierOf(Settlement settlement) =>
        settlement.Capital ? SettlementTier.Capital
        : settlement.Kind == SettlementKind.City ? SettlementTier.City
        : SettlementTier.Town;

    /// <summary>The persons of the households of a region.</summary>
    /// <param name="region">The region: the identity the draws are seeded from.</param>
    /// <param name="households">The households of the region, by slot.</param>
    /// <param name="factions">The factions of the region, by slot.</param>
    /// <param name="places">What the persons read of each city or town, by its slot.</param>
    public RegionPeople Build(
        GridCoord region,
        IReadOnlyList<Household> households,
        IReadOnlyList<Faction> factions,
        IReadOnlyDictionary<int, HouseholdPlace> places)
    {
        var persons = new List<Person>();
        foreach (var household in households)
        {
            var faction = household.Faction is { } slot ? factions.FirstOrDefault(f => f.Slot == slot) : null;
            var place = places.TryGetValue(household.Settlement, out var p) ? p : new HouseholdPlace(SettlementTier.Town, false);
            persons.AddRange(PersonsOf(region, household, faction, place));
        }
        return new RegionPeople(persons);
    }

    private sealed class Draft(HouseholdRole role, Sex sex, int age, string race)
    {
        public HouseholdRole Role { get; } = role;
        public Sex Sex { get; } = sex;
        public int Age { get; } = age;
        public string Race { get; } = race;
        public string? Vocation { get; set; }
        public Draft? Spouse { get; set; }
        public Draft? Father { get; set; }
        public Draft? Mother { get; set; }
    }

    /// <summary>The persons of one household: the head, the family and the servants.</summary>
    public IReadOnlyList<Person> PersonsOf(GridCoord region, Household household, Faction? faction, HouseholdPlace place)
    {
        var rng = new Sfc32(HouseholdRules.Key(_seed, HouseholdPurpose, region, household.Slot, 0));
        var (uHeadVocation, uSpouse, uGap, uSpouseSame, uSpouseVocation) =
            (rng.Next(), rng.Next(), rng.Next(), rng.Next(), rng.Next());

        var drafts = new List<Draft>();
        var head = new Draft(HouseholdRole.Head, household.HeadSex, household.HeadAge, household.Race)
        {
            Vocation = HeadVocation(household, faction, place, uHeadVocation),
        };
        drafts.Add(head);
        var family = Math.Max(1, household.Family);

        // The husband or the wife of the head.
        Draft? spouse = null;
        if (family >= 2 && uSpouse < _parameters.SpouseChance)
        {
            var gap = Hashing.Between(-HusbandYounger, HusbandOlder, uGap); // the husband's age minus the wife's
            var age = head.Sex == Sex.Male ? head.Age - gap : head.Age + gap;
            spouse = new Draft(HouseholdRole.Spouse, head.Sex == Sex.Male ? Sex.Female : Sex.Male,
                Math.Clamp(age, MinSpouseAge, MaxAge), household.Race) { Spouse = head };
            spouse.Vocation = OtherVocation(spouse.Age, head, household.Wealth, place, uSpouseSame, uSpouseVocation);
            head.Spouse = spouse;
            drafts.Add(spouse);
        }
        var couple = spouse == null ? new[] { head } : [head, spouse];
        var father = couple.FirstOrDefault(d => d.Sex == Sex.Male);
        var mother = couple.FirstOrDefault(d => d.Sex == Sex.Female);

        // The rest of the family: children of the head, its parents and its siblings.
        var born = new List<Draft> { head }; // the head and its siblings: the children of the parents of the head
        Draft? headFather = null, headMother = null;
        while (drafts.Count < family)
        {
            var (uRole, uSex, uAge, uSame, uVocation) = (rng.Next(), rng.Next(), rng.Next(), rng.Next(), rng.Next());
            var eldest = born.Max(d => d.Age);
            var youngest = born.Min(d => d.Age);

            var childLow = mother != null ? Math.Max(0, mother.Age - MotherAgeSpan) : 0;
            var childHigh = couple.Min(d => d.Age) - ParentAgeGap;
            var child = childHigh >= childLow ? ChildWeight : 0;
            var parent = household.Kin == null && (headFather == null || headMother == null) && eldest + ParentAgeGap <= MaxAge
                ? ParentWeight : 0;
            var pick = uRole * (child + parent + SiblingWeight);

            Draft draft;
            if (pick < child)
            {
                draft = new Draft(HouseholdRole.Child, uSex < 0.5 ? Sex.Male : Sex.Female,
                    Hashing.Between(childLow, childHigh, uAge), household.Race) { Father = father, Mother = mother };
            }
            else if (pick < child + parent)
            {
                var sex = headFather != null ? Sex.Female : headMother != null ? Sex.Male : uSex < 0.5 ? Sex.Male : Sex.Female;
                var low = eldest + ParentAgeGap;
                var high = Math.Min(MaxAge, sex == Sex.Female ? youngest + MotherAgeSpan : eldest + ParentAgeSpan);
                draft = new Draft(HouseholdRole.Parent, sex, Hashing.Between(low, Math.Max(low, high), uAge), household.Race);
                if (sex == Sex.Male) headFather = draft; else headMother = draft;
            }
            else
            {
                var low = Math.Max(MinSiblingAge, head.Age - SiblingAgeSpread);
                var high = head.Age + SiblingAgeSpread;
                if (headFather != null) high = Math.Min(high, headFather.Age - ParentAgeGap);
                if (headMother != null)
                {
                    high = Math.Min(high, headMother.Age - ParentAgeGap);
                    low = Math.Max(low, headMother.Age - MotherAgeSpan);
                }
                // The head itself fits every bound but the youngest age, so the range holds an age.
                var age = low <= high ? Hashing.Between(low, high, uAge) : head.Age;
                draft = new Draft(HouseholdRole.Sibling, uSex < 0.5 ? Sex.Male : Sex.Female, age, household.Race);
                born.Add(draft);
            }
            draft.Vocation = OtherVocation(draft.Age, head, household.Wealth, place, uSame, uVocation);
            drafts.Add(draft);
        }

        // The parents of the head are the parents of its siblings, and husband and wife.
        foreach (var d in born)
        {
            d.Father = headFather;
            d.Mother = headMother;
        }
        if (headFather != null && headMother != null)
        {
            headFather.Spouse = headMother;
            headMother.Spouse = headFather;
        }

        // The servants.
        var servantVocation = _byId.ContainsKey(_parameters.ServantVocation) ? _parameters.ServantVocation : null;
        foreach (var group in household.Servants)
            for (var i = 0; i < group.Persons; i++)
            {
                var (uSex, uAge) = (rng.Next(), rng.Next());
                drafts.Add(new Draft(HouseholdRole.Servant, uSex < 0.5 ? Sex.Male : Sex.Female,
                    Hashing.Between(MinServantAge, MaxServantAge, uAge), group.Race) { Vocation = servantVocation });
            }

        // The names, each from a stream of its own.
        var given = new HashSet<string>(StringComparer.Ordinal);
        var persons = new List<Person>(drafts.Count);
        for (var slot = 0; slot < drafts.Count; slot++)
        {
            var d = drafts[slot];
            var style = _styles.GetValueOrDefault(d.Race, NameStyle.Common);
            var names = new Sfc32(HouseholdRules.Key(_seed, NamePurpose, region, household.Slot, slot));
            var name = "";
            for (var attempt = 0; attempt < GivenNameAttempts; attempt++)
            {
                name = NameGenerator.Default.Generate(style, d.Sex == Sex.Male ? NameKind.Male : NameKind.Female, names);
                if (!given.Contains(name)) break;
            }
            given.Add(name);
            var surname = d.Role == HouseholdRole.Servant ? RegionNameRules.Surname(style, names) : household.Surname;
            persons.Add(new Person(household.Slot, slot, name, surname, d.Race, d.Sex, d.Age, d.Role, d.Vocation,
                SlotOf(d.Spouse), SlotOf(d.Father), SlotOf(d.Mother)));
        }

        // The looks, each from a stream of its own: they read the persons and change nothing of them.
        var looks = _looks.LooksOf(region, household, faction, persons);
        for (var slot = 0; slot < persons.Count; slot++) persons[slot] = persons[slot] with { Looks = looks[slot] };
        return persons;

        int? SlotOf(Draft? other) => other == null ? null : drafts.IndexOf(other);
    }

    /// <summary>
    /// The vocation of the head of a household. The head of a household of a faction holds a
    /// vocation of the faction: one it gathers, or the one of those who lead a faction of its
    /// type. Any other head holds a vocation whose standing fits the wealth of the household.
    /// </summary>
    private string? HeadVocation(Household household, Faction? faction, HouseholdPlace place, double u)
    {
        if (faction != null)
        {
            var own = faction.Vocations.Where(_byId.ContainsKey).Select(id => _byId[id]).ToList();
            if (own.Count == 0 && _byId.TryGetValue(LeaderVocations[faction.Type], out var leader)) own.Add(leader);
            if (own.Count > 0) return Draw(own, v => Math.Max(0, v.SharePercent), u)?.Id ?? own[0].Id;
        }
        return FittingVocation(household.Wealth, place, u);
    }

    /// <summary>
    /// The vocation of a person of the family other than the head: none below the working age,
    /// the vocation of the head by chance, otherwise one that fits the wealth of the household.
    /// </summary>
    private string? OtherVocation(int age, Draft head, WealthLevel wealth, HouseholdPlace place, double uSame, double uVocation)
    {
        if (age < _parameters.WorkingAge) return null;
        if (head.Vocation != null && uSame < _parameters.SameVocationChance) return head.Vocation;
        return FittingVocation(wealth, place, uVocation);
    }

    /// <summary>
    /// A vocation found in the place whose standing fits the wealth level, by its share. Where
    /// none fits, the nearest lower level that has one, then the nearest higher; null when the
    /// place has no vocation at all.
    /// </summary>
    public string? FittingVocation(WealthLevel wealth, HouseholdPlace place, double u)
    {
        var levels = Enum.GetValues<WealthLevel>();
        var order = levels.Where(l => l <= wealth).OrderByDescending(l => l).Concat(levels.Where(l => l > wealth));
        foreach (var level in order)
        {
            var found = Draw(_vocations.Where(v => Fits(v, level)).ToList(), v => WeightIn(v, place), u);
            if (found != null) return found.Id;
        }
        return null;
    }

    /// <summary>
    /// The weight of a vocation in a place: its share, less for the work of the land and the
    /// water; none below its lowest tier, and none away from water for a vocation on water.
    /// </summary>
    public double WeightIn(VocationParameters vocation, HouseholdPlace place)
    {
        if (vocation.LowestTier > place.Tier) return 0;
        if (vocation.OnWater && !place.OnWater) return 0;
        var share = double.IsFinite(vocation.SharePercent) ? Math.Max(0, vocation.SharePercent) : 0;
        var factor = vocation.Sector == VocationSector.LandAndWater ? Math.Max(0, _parameters.LandWorkFactor) : 1;
        return share * factor;
    }

    /// <summary>One of the candidates by weight; null when none has a weight.</summary>
    private static VocationParameters? Draw(IReadOnlyList<VocationParameters> candidates, Func<VocationParameters, double> weight, double u)
    {
        var total = 0.0;
        foreach (var c in candidates) total += weight(c);
        if (!(total > 0)) return null;
        var target = u * total;
        var sum = 0.0;
        VocationParameters? last = null;
        foreach (var c in candidates)
        {
            var w = weight(c);
            if (!(w > 0)) continue;
            sum += w;
            last = c;
            if (target < sum) return c;
        }
        return last;
    }
}
