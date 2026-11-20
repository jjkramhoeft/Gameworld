using Gameworld.Domain;
using Gameworld.Generation.Basics;
using Gameworld.Generation.Races;

namespace Gameworld.Generation.Settlements;

/// <summary>A Prospect on show as the cities and towns read it.</summary>
/// <param name="Slot">The index of the Prospect among the Prospects of the region.</param>
/// <param name="Patch">The patch of the Prospect.</param>
/// <param name="ForCity">The Prospect is for a city; otherwise for a town.</param>
/// <param name="Races">The races of a city or a town on the Prospect, as indexes into <see cref="PopulationRules.Races"/>, the largest first and the drawn one last.</param>
/// <param name="Weights">Per race of the region, in the order of <see cref="RegionRaces.Races"/>: its weight in a city or a town on the Prospect.</param>
public readonly record struct SettlementSite(int Slot, PatchId Patch, bool ForCity, int[] Races, double[] Weights);

/// <summary>The cities and towns of one region, and the persons that live on the land between them.</summary>
public sealed class RegionSettlements
{
    /// <summary>The cities and towns of the region, by rank.</summary>
    public required Settlement[] Settlements { get; init; }
    /// <summary>The persons on the land: one entry for every race that is on, in the order of the Parameter Set.</summary>
    public required RacePersons[] Rural { get; init; }

    public long CityPersons => Settlements.Where(s => s.Kind == SettlementKind.City).Sum(s => s.Persons);
    public long TownPersons => Settlements.Where(s => s.Kind == SettlementKind.Town).Sum(s => s.Persons);
    public long RuralPersons => Rural.Sum(r => r.Persons);
}

/// <summary>
/// The cities and towns of a region (society-generation.md section 5.2): a city or a town on
/// every Prospect on show, the persons of the region that live in each, and their races. A pure
/// function of the region's own Prospects, races and capital: it holds no random choice and
/// reads nothing from another region.
/// </summary>
public sealed class SettlementRules
{
    private readonly SettlementParameters _parameters;
    private readonly PopulationRules _population;

    public SettlementRules(ParameterSet parameters, PopulationRules population)
    {
        _parameters = parameters.Settlement;
        _population = population;
    }

    /// <summary>The share of the persons of a region in its cities, after the clamp and the scaling of the two shares.</summary>
    public double CityShare => Shares().City;

    /// <summary>The share of the persons of a region in its towns, after the clamp and the scaling of the two shares.</summary>
    public double TownShare => Shares().Town;

    /// <summary>Each share is clamped to 0–1, and both are scaled down when they sum to more than 1.</summary>
    private (double City, double Town) Shares()
    {
        static double Clean(double share) => double.IsFinite(share) ? Math.Clamp(share, 0, 1) : 0;
        var (city, town) = (Clean(_parameters.CityShare), Clean(_parameters.TownShare));
        var sum = city + town;
        return sum > 1 ? (city / sum, town / sum) : (city, town);
    }

    /// <summary>The weight of a rank: the rank raised to minus the rank exponent. 1/rank for an exponent of 1.</summary>
    public double RankWeight(int rank)
    {
        var exponent = double.IsFinite(_parameters.RankExponent) ? Math.Max(0, _parameters.RankExponent) : 0;
        if (exponent == 0 || rank <= 1) return 1;
        return exponent == 1 ? 1.0 / rank : DetMath.Exp(-exponent * DetMath.Ln(rank));
    }

    /// <summary>The cities and towns of a region.</summary>
    /// <param name="races">The races of the region and its potential population.</param>
    /// <param name="sites">The Prospects on show of the region, in the order of its Prospects.</param>
    /// <param name="capital">The patch of the seat of the ruling faction; null in a region without one.</param>
    public RegionSettlements Build(RegionRaces races, IReadOnlyList<SettlementSite> sites, PatchId? capital)
    {
        // The persons of the region that are not yet placed in a city or a town, per race that is on.
        var left = races.Population.Races.Select(r => r.Persons).ToArray();
        var total = left.Sum();

        // The order: the capital, the other cities, then the towns, each in the order of the Prospects.
        var capitalAt = -1;
        if (capital is { } seat)
            for (var i = 0; i < sites.Count && capitalAt < 0; i++)
                if (sites[i].Patch == seat) capitalAt = i;
        var cities = new List<int>();
        var towns = new List<int>();
        if (capitalAt >= 0) cities.Add(capitalAt);
        for (var i = 0; i < sites.Count; i++)
        {
            if (i == capitalAt) continue;
            (sites[i].ForCity ? cities : towns).Add(i);
        }

        // The persons of the cities and of the towns: a share of the region, and one person
        // for each where the region has one. Together never more than the region holds.
        var (cityShare, townShare) = Shares();
        var cityPool = Pool(cityShare, cities.Count, total);
        var townPool = Pool(townShare, towns.Count, total);
        if (cityPool + townPool > total)
        {
            cityPool = Math.Min(cityPool, total);
            townPool = total - cityPool;
        }

        // The cities share their persons by rank, and so do the towns.
        var cityPersons = Apportion(cityPool, cities.Select((_, k) => RankWeight(k + 1)).ToArray());
        var townPersons = Apportion(townPool, towns.Select((_, k) => RankWeight(cities.Count + k + 1)).ToArray());

        var settlements = new List<Settlement>(sites.Count);
        for (var k = 0; k < cities.Count + towns.Count; k++)
        {
            var isCity = k < cities.Count;
            var at = isCity ? cities[k] : towns[k - cities.Count];
            var persons = isCity ? cityPersons[k] : townPersons[k - cities.Count];
            var site = sites[at];
            settlements.Add(new Settlement(
                site.Slot,
                isCity ? SettlementKind.City : SettlementKind.Town,
                at == capitalAt,
                site.Patch,
                k + 1,
                persons,
                RacesOf(races, site, persons, left)));
        }

        return new RegionSettlements
        {
            Settlements = settlements.ToArray(),
            Rural = _population.Races.Select((race, k) => new RacePersons(race.Id, left[k])).ToArray(),
        };
    }

    /// <summary>The persons of a kind: its share of the region, half up, and not fewer than its cities or towns. None without a city or town, or with a share of 0.</summary>
    private static long Pool(double share, int count, long total) =>
        count == 0 || share <= 0 ? 0 : Math.Max((long)Math.Floor(share * total + 0.5), count);

    /// <summary>
    /// Shares a number of persons by weight: the whole part of each quota, then one more to the
    /// largest remainders, ties to the first. A city or a town left with nobody then takes one
    /// person from the largest that holds two or more.
    /// </summary>
    internal static long[] Apportion(long persons, double[] weights)
    {
        var result = new long[weights.Length];
        var sum = weights.Sum();
        if (persons <= 0 || weights.Length == 0 || !(sum > 0)) return result;

        var remainder = new double[weights.Length];
        var rest = persons;
        for (var i = 0; i < weights.Length; i++)
        {
            var quota = persons * weights[i] / sum;
            result[i] = (long)Math.Floor(quota);
            remainder[i] = quota - result[i];
            rest -= result[i];
        }
        foreach (var i in Enumerable.Range(0, weights.Length).OrderByDescending(i => remainder[i]).ThenBy(i => i))
        {
            if (rest <= 0) break;
            result[i]++;
            rest--;
        }

        for (var i = 0; i < result.Length; i++)
        {
            if (result[i] > 0) continue;
            var donor = 0;
            for (var k = 1; k < result.Length; k++)
                if (result[k] > result[donor]) donor = k;
            if (result[donor] < 2) break;
            result[donor]--;
            result[i] = 1;
        }
        return result;
    }

    /// <summary>
    /// The persons of each race of a city or a town: its persons shared between the races of
    /// its Prospect by their weight, a race never above the persons it has left in the region.
    /// What the races of the list cannot fill goes to the other races of the region, which
    /// join the list at its end.
    /// </summary>
    private RacePersons[] RacesOf(RegionRaces region, SettlementSite site, long persons, long[] left)
    {
        var list = site.Races;
        var weight = new double[list.Length];
        for (var j = 0; j < list.Length; j++)
        {
            var at = Array.IndexOf(region.Races, list[j]);
            weight[j] = at >= 0 && at < site.Weights.Length ? Math.Max(0, site.Weights[at]) : 0;
        }
        var sum = weight.Sum();

        var held = new long[list.Length];
        var quota = new double[list.Length];
        var rest = persons;
        for (var j = 0; j < list.Length; j++)
        {
            quota[j] = sum > 0 ? persons * weight[j] / sum : (double)persons / list.Length;
            held[j] = Math.Min((long)Math.Floor(quota[j]), left[list[j]]);
            rest -= held[j];
        }
        while (rest > 0)
        {
            var best = -1;
            for (var j = 0; j < list.Length; j++)
                if (left[list[j]] > held[j] && (best < 0 || quota[j] - held[j] > quota[best] - held[best])) best = j;
            if (best < 0) break;
            held[best]++;
            rest--;
        }

        var result = new List<RacePersons>(list.Length);
        for (var j = 0; j < list.Length; j++)
        {
            left[list[j]] -= held[j];
            result.Add(new RacePersons(_population.Races[list[j]].Id, held[j]));
        }

        // The list is used up: the other races of the region, the one with the most persons left first.
        while (rest > 0)
        {
            var best = -1;
            for (var k = 0; k < left.Length; k++)
                if (left[k] > 0 && (best < 0 || left[k] > left[best])) best = k;
            if (best < 0) break;
            var take = Math.Min(rest, left[best]);
            left[best] -= take;
            rest -= take;
            result.Add(new RacePersons(_population.Races[best].Id, take));
        }
        return result.ToArray();
    }
}
