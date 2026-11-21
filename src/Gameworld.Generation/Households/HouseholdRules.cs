using Gameworld.Domain;
using Gameworld.Generation.Basics;
using Gameworld.Generation.Names;

namespace Gameworld.Generation.Households;

/// <summary>
/// The households of a region (society-generation.md section 6): the households of the
/// factions first, then the other households of the cities, then those of the towns. The
/// households of a city or a town hold exactly its persons, race by race. A pure function of
/// the region's own factions, cities and towns; nothing is read from another region.
/// </summary>
public sealed class HouseholdRules
{
    /// <summary>Hash purpose of the draws for a household of a faction.</summary>
    public const uint FactionPurpose = 0x4d01;
    /// <summary>Hash purpose of the draws for the other households of a city or a town.</summary>
    public const uint SettlementPurpose = 0x4d02;
    /// <summary>Hash purpose of the draws for the surname of a household.</summary>
    public const uint SurnamePurpose = 0x4d03;

    /// <summary>The head of a household is at least this old, whatever the Parameter Set says.</summary>
    public const int MinHeadAge = 16;
    /// <summary>The head of a branch household that is a child of the head of the house is at least this much younger.</summary>
    public const int BranchAgeGap = 18;
    /// <summary>The head of a branch household that is a sibling of the head of the house is at most this much older or younger.</summary>
    public const int SiblingAgeSpread = 10;
    /// <summary>The chance that the head of a branch household is a child of the head of the house, where the ages allow it; a sibling otherwise.</summary>
    public const double BranchChildChance = 0.5;

    private readonly HousingParameters _parameters;
    private readonly Dictionary<string, NameStyle> _styles = new(StringComparer.Ordinal);
    private readonly uint _seed;

    /// <param name="seed">The seed of the foundation: it gives the draws.</param>
    public HouseholdRules(ParameterSet parameters, uint seed = 0)
    {
        _parameters = parameters.Housing;
        foreach (var race in parameters.Races) _styles.TryAdd(race.Id, race.NameStyle);
        _seed = seed;
    }

    /// <summary>The youngest and the oldest head of a household, after the clamp.</summary>
    public (int Min, int Max) HeadAge
    {
        get
        {
            var min = Math.Max(MinHeadAge, _parameters.HeadAge.Min);
            return (min, Math.Max(min, _parameters.HeadAge.Max));
        }
    }

    /// <summary>The key of a draw inside a region: the hash of the region, hashed again with two numbers of the thing drawn for.</summary>
    internal static uint Key(uint seed, uint purpose, GridCoord region, int a, int b) =>
        Hashing.Hash4(Hashing.Hash4(seed, purpose, region.I, region.J), purpose, a, b);

    /// <summary>The households of a region.</summary>
    /// <param name="region">The region: the identity the draws are seeded from.</param>
    /// <param name="factions">The factions of the region, by slot.</param>
    /// <param name="settlements">The cities and towns of the region, by rank.</param>
    /// <param name="rulingHouse">The surname of the ruling house; null when the ruling faction is not of the type Family.</param>
    public RegionHouseholds Build(
        GridCoord region, IReadOnlyList<Faction> factions, IReadOnlyList<Settlement> settlements, string? rulingHouse)
    {
        // The persons of each city or town that are not yet in a household, per race of its list.
        var left = settlements.Select(s => s.Races.Select(r => Math.Max(0, r.Persons)).ToArray()).ToArray();

        // Step 1: the heads of the households of the factions. Each takes one person.
        var heads = Heads(factions, settlements, left);

        var households = new List<Household>();
        var surnames = new HashSet<string>(StringComparer.Ordinal);
        if (rulingHouse != null) surnames.Add(rulingHouse);
        var (minAge, maxAge) = HeadAge;

        // Step 1, continued: the households of the factions are filled, by faction and round.
        for (var f = 0; f < factions.Count; f++)
        {
            var faction = factions[f];
            var house = faction.Type == FactionType.Family;
            Household? first = null;
            for (var round = 0; round < heads[f].Count; round++)
            {
                var (s, r) = heads[f][round];
                var settlement = settlements[s];
                var race = settlement.Races[r].Race;

                var rng = new Sfc32(Key(_seed, FactionPurpose, region, faction.Slot, round));
                var (uWealth, uFamily, uServants, uSex, uAge, uKin, uKinAge) =
                    (rng.Next(), rng.Next(), rng.Next(), rng.Next(), rng.Next(), rng.Next(), rng.Next());

                var wealth = faction.Role == FactionRole.Ruling && round == 0
                    ? WealthLevel.Rich
                    : WealthOf(_parameters.FactionWealth, uWealth);
                var more = (int)Math.Min(FamilySize(uFamily) - 1, left[s][r]);
                left[s][r] -= more;
                var servants = Servants(settlement, left[s], wealth, uServants, faction.Type == FactionType.Race ? r : -1);

                var sex = HeadSex(1 + more, uSex);
                var age = Hashing.Between(minAge, maxAge, uAge);
                HouseholdKin? kin = null;
                if (house && first != null)
                {
                    // A branch of the house: its head is a child or a sibling of the head of the first household.
                    if (first.HeadAge - BranchAgeGap >= minAge && uKin < BranchChildChance)
                    {
                        kin = new HouseholdKin(first.Slot, HouseholdRole.Child);
                        age = Hashing.Between(minAge, first.HeadAge - BranchAgeGap, uKinAge);
                    }
                    else
                    {
                        kin = new HouseholdKin(first.Slot, HouseholdRole.Sibling);
                        age = Math.Clamp(first.HeadAge + Hashing.Between(-SiblingAgeSpread, SiblingAgeSpread, uKinAge), minAge, maxAge);
                    }
                }

                var slot = households.Count;
                var surname = house && first != null ? first.Surname
                    : house && faction.Role == FactionRole.Ruling && rulingHouse != null ? rulingHouse
                    : Surname(region, slot, race, surnames);
                surnames.Add(surname);

                var household = new Household(slot, settlement.Slot, faction.Slot, race, wealth, surname, sex, age, 1 + more, servants, kin);
                households.Add(household);
                first ??= household;
            }
        }

        // Steps 2 and 3: the other households, city by city and then town by town, by rank.
        for (var s = 0; s < settlements.Count; s++)
        {
            var settlement = settlements[s];
            var levels = settlement.Kind == SettlementKind.City ? _parameters.CityWealth : _parameters.TownWealth;
            var patch = settlement.Patch.Coord;
            var rng = new Sfc32(Hashing.Hash4(_seed, SettlementPurpose, patch.I, patch.J));
            while (left[s].Sum() > 0)
            {
                var (uRace, uWealth, uFamily, uServants, uSex, uAge) =
                    (rng.Next(), rng.Next(), rng.Next(), rng.Next(), rng.Next(), rng.Next());

                var r = RaceOf(left[s], uRace);
                var race = settlement.Races[r].Race;
                var family = (int)Math.Min(FamilySize(uFamily), left[s][r]);
                left[s][r] -= family;
                var wealth = WealthOf(levels, uWealth);
                var servants = Servants(settlement, left[s], wealth, uServants, -1);
                var sex = HeadSex(family, uSex);
                var age = Hashing.Between(minAge, maxAge, uAge);

                var slot = households.Count;
                var surname = Surname(region, slot, race, surnames);
                surnames.Add(surname);
                households.Add(new Household(slot, settlement.Slot, null, race, wealth, surname, sex, age, family, servants));
            }
        }

        return new RegionHouseholds(households);
    }

    /// <summary>
    /// The heads of the households of the factions: for each faction, the city or town and the
    /// race of each of its households, as indexes. A faction looks at its seat for its lead race
    /// first, then in the other cities and towns by rank; then, unless it is of the type Race,
    /// for any race the same way. Every faction gets its first head before any gets a second in
    /// the same pass, and a later household of a house keeps the race of the first.
    /// </summary>
    private List<(int Settlement, int Race)>[] Heads(
        IReadOnlyList<Faction> factions, IReadOnlyList<Settlement> settlements, long[][] left)
    {
        var heads = new List<(int Settlement, int Race)>[factions.Count];
        var seats = new int[factions.Count];
        for (var f = 0; f < factions.Count; f++)
        {
            heads[f] = [];
            seats[f] = -1;
            if (factions[f].Seat is { } seat)
                for (var s = 0; s < settlements.Count && seats[f] < 0; s++)
                    if (settlements[s].Patch == seat) seats[f] = s;
        }

        var rounds = Math.Max(0, _parameters.FactionHouseholds);
        foreach (var (atSeat, leadRace) in new[] { (true, true), (false, true), (true, false), (false, false) })
            for (var round = 0; round < rounds; round++)
                for (var f = 0; f < factions.Count; f++)
                {
                    if (heads[f].Count != round) continue;
                    var faction = factions[f];
                    var kept = round > 0 && faction.Type == FactionType.Family
                        ? settlements[heads[f][0].Settlement].Races[heads[f][0].Race].Race
                        : null;
                    if (!leadRace && (kept != null || faction.Type == FactionType.Race)) continue;
                    var wanted = leadRace ? kept ?? faction.LeadRace : null;

                    for (var s = 0; s < settlements.Count; s++)
                    {
                        if (atSeat != (s == seats[f])) continue;
                        var r = wanted != null ? IndexOf(settlements[s], wanted, left[s]) : MostLeft(left[s]);
                        if (r < 0) continue;
                        left[s][r]--;
                        heads[f].Add((s, r));
                        break;
                    }
                }
        return heads;
    }

    /// <summary>The index of a race in the list of a city or a town when a person of it is left; −1 otherwise.</summary>
    private static int IndexOf(Settlement settlement, string race, long[] left)
    {
        for (var r = 0; r < settlement.Races.Count; r++)
            if (left[r] > 0 && string.Equals(settlement.Races[r].Race, race, StringComparison.Ordinal)) return r;
        return -1;
    }

    /// <summary>The index of the race with the most persons left, the first of equals; −1 when nobody is left.</summary>
    private static int MostLeft(long[] left)
    {
        var best = -1;
        for (var r = 0; r < left.Length; r++)
            if (left[r] > 0 && (best < 0 || left[r] > left[best])) best = r;
        return best;
    }

    /// <summary>A race by the persons it has left. Somebody must be left.</summary>
    private static int RaceOf(long[] left, double u)
    {
        var target = u * left.Sum();
        var sum = 0.0;
        var last = 0;
        for (var r = 0; r < left.Length; r++)
        {
            if (left[r] <= 0) continue;
            sum += left[r];
            last = r;
            if (target < sum) return r;
        }
        return last;
    }

    /// <summary>The size of a family by the weights of the Parameter Set: 1 when no size has a weight.</summary>
    public int FamilySize(double u)
    {
        var weights = _parameters.FamilySizeWeights;
        var total = 0.0;
        foreach (var w in weights) total += Weight(w);
        if (!(total > 0)) return 1;
        var target = u * total;
        var sum = 0.0;
        var last = 1;
        for (var i = 0; i < weights.Count; i++)
        {
            var w = Weight(weights[i]);
            if (!(w > 0)) continue;
            sum += w;
            last = i + 1;
            if (target < sum) return last;
        }
        return last;
    }

    /// <summary>A wealth level by its weights: Common when no level has a weight.</summary>
    public static WealthLevel WealthOf(PerWealthLevel<double> weights, double u)
    {
        var levels = Enum.GetValues<WealthLevel>();
        var total = 0.0;
        foreach (var level in levels) total += Weight(weights[level]);
        if (!(total > 0)) return WealthLevel.Common;
        var target = u * total;
        var sum = 0.0;
        var last = WealthLevel.Common;
        foreach (var level in levels)
        {
            var w = Weight(weights[level]);
            if (!(w > 0)) continue;
            sum += w;
            last = level;
            if (target < sum) return level;
        }
        return last;
    }

    private static double Weight(double w) => double.IsFinite(w) && w > 0 ? w : 0;

    /// <summary>
    /// The sex of the head of a household: a man by the chance of the Parameter Set in a family
    /// of two or more. A person who is alone in its family is a man or a woman with even chance.
    /// </summary>
    public Sex HeadSex(int family, double u) =>
        u < (family >= 2 ? _parameters.MaleHeadChance : 0.5) ? Sex.Male : Sex.Female;

    /// <summary>
    /// The servants of a household: as many as its wealth wants and the city or town has left,
    /// each from the race with the most persons left, or from one race only.
    /// </summary>
    private RacePersons[] Servants(Settlement settlement, long[] left, WealthLevel wealth, double u, int onlyRace)
    {
        var range = _parameters.Servants[wealth];
        var wanted = Hashing.Between(Math.Max(0, range.Min), Math.Max(0, range.Max), u);
        if (wanted == 0) return [];
        var taken = new long[left.Length];
        for (var i = 0; i < wanted; i++)
        {
            var r = onlyRace >= 0 ? (left[onlyRace] > 0 ? onlyRace : -1) : MostLeft(left);
            if (r < 0) break;
            left[r]--;
            taken[r]++;
        }
        var servants = new List<RacePersons>();
        for (var r = 0; r < taken.Length; r++)
            if (taken[r] > 0) servants.Add(new RacePersons(settlement.Races[r].Race, taken[r]));
        return servants.ToArray();
    }

    /// <summary>The name style of a race; Common for an id that is not a race of the Parameter Set.</summary>
    public NameStyle StyleOf(string race) => _styles.GetValueOrDefault(race, NameStyle.Common);

    /// <summary>The surname of a household, in the name style of its race: one that no earlier household of the region has, where the stream gives one.</summary>
    private string Surname(GridCoord region, int slot, string race, HashSet<string> taken) =>
        RegionNameRules.Surname(StyleOf(race), new Sfc32(Key(_seed, SurnamePurpose, region, slot, 0)), taken.Contains);
}
