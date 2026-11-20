using Gameworld.Domain;
using Gameworld.Generation.Basics;

namespace Gameworld.Generation.Races;

/// <summary>A land patch as the potential population reads it: its final biome, height and resources.</summary>
public readonly record struct PopulationPatch(Biome Biome, double HeightM, double AreaKm2, PatchResources Resources, double Prosperity)
{
    public static implicit operator PopulationPatch((Biome Biome, double HeightM, double AreaKm2, PatchResources Resources, double Prosperity) p) =>
        new(p.Biome, p.HeightM, p.AreaKm2, p.Resources, p.Prosperity);
}

/// <summary>
/// The races of one region and how they share its land (society-generation.md sections 3.3
/// and 3.4): the working data behind the potential population. A race is an index into
/// <see cref="PopulationRules.Races"/>.
/// </summary>
public sealed class RegionRaces
{
    /// <summary>The races of the region: the main races, the largest first, then the minority races in the order of their places.</summary>
    public required int[] Races { get; init; }
    /// <summary>How many of <see cref="Races"/> are main races.</summary>
    public required int MainCount { get; init; }
    /// <summary>Per race that is on: the persons the land would feed by the plain shares, with every race that is on sharing it.</summary>
    public required double[] Plain { get; init; }
    /// <summary>Per race that is on: its potential population, not rounded. 0 for a race that is not a race of the region.</summary>
    public required double[] Persons { get; init; }
    /// <summary>Per race of the region, in the order of <see cref="Races"/>: the factor on its weights. 1 for a main race.</summary>
    public required double[] Boost { get; init; }
    /// <summary>Per land patch, in the order given, and per race of the region, in the order of <see cref="Races"/>: the persons of the race on the patch.</summary>
    public required double[][] PatchPersons { get; init; }
    public required PotentialPopulation Population { get; init; }

    /// <summary>The minority races of the region, in the order of their places.</summary>
    public ReadOnlySpan<int> Minority => Races.AsSpan(MainCount);

    /// <summary>Whether a race holds a minority place of the region.</summary>
    public bool IsMinority(int race) => Array.IndexOf(Races, race) >= MainCount;
}

/// <summary>
/// The potential population of the land and how the races share it (society-generation.md
/// section 3), and the races of a city or a town (section 5.1). The capacity and the plain
/// shares are a plain function of the Parameter Set and of the biome, the height, the
/// resources and the Prosperity of a patch. Which races a region, a city or a town holds also
/// depends on the seed: the turns of the minority races and the draws.
/// </summary>
public sealed class PopulationRules
{
    /// <summary>Hash purpose of the turn pattern: the order of the races and its offset.</summary>
    public const uint TurnPurpose = 0x4a01;
    /// <summary>Hash purpose of the draws for the minority places of a region.</summary>
    public const uint MinorityDrawPurpose = 0x4a02;
    /// <summary>Hash purpose of the draw for the last race of a city or a town.</summary>
    public const uint SettlementDrawPurpose = 0x4a03;

    /// <summary>How much the fish of a patch adds to its food supply.</summary>
    public const double FishShare = 0.2;

    private readonly PopulationParameters _parameters;
    private readonly RaceParameters[] _races;
    /// <summary>Per race that is on: the control points of its height modifier, by height; empty when the height does not matter to it.</summary>
    private readonly HeightFactorPoint[][] _heightPoints;
    private readonly uint _seed;
    /// <summary>The turn pattern: the races that are on in the order of their turns.</summary>
    private readonly int[] _turnOrder;
    private readonly int _turnOffset;

    /// <param name="seed">The seed of the foundation: it gives the turn pattern and the draws.</param>
    public PopulationRules(ParameterSet parameters, uint seed = 0)
    {
        _seed = seed;
        _parameters = parameters.Planning.Population;

        var ids = new HashSet<string>(StringComparer.Ordinal);
        foreach (var race in parameters.Races)
        {
            if (string.IsNullOrWhiteSpace(race.Id))
                throw new ArgumentException("A race of the Parameter Set has no id.", nameof(parameters));
            if (!ids.Add(race.Id))
                throw new ArgumentException($"The Parameter Set lists the race id \"{race.Id}\" more than once.", nameof(parameters));
        }
        // The colours have an id each and are colours, and a favourite colour of a race is one of them.
        var colours = new HashSet<string>(StringComparer.Ordinal);
        foreach (var colour in parameters.Colours)
        {
            if (string.IsNullOrWhiteSpace(colour.Id))
                throw new ArgumentException("A colour of the Parameter Set has no id.", nameof(parameters));
            if (!colours.Add(colour.Id))
                throw new ArgumentException($"The Parameter Set lists the colour id \"{colour.Id}\" more than once.", nameof(parameters));
            if (!ColourParameters.TryLightness(colour.Hex, out _))
                throw new ArgumentException($"The colour \"{colour.Id}\" has the value \"{colour.Hex}\", which is not a colour as #RRGGBB.", nameof(parameters));
        }
        foreach (var race in parameters.Races)
            foreach (var colour in race.FavouriteColours ?? [])
                if (colour is null || !colours.Contains(colour))
                    throw new ArgumentException($"The race \"{race.Id}\" lists \"{colour}\" in FavouriteColours, which is not a colour of the Parameter Set.", nameof(parameters));

        // The races a race appreciates and hates are other races of the list, and no race is in both.
        foreach (var race in parameters.Races)
        {
            foreach (var (list, name) in new[] { (race.AppreciatedRaces, nameof(race.AppreciatedRaces)), (race.HatedRaces, nameof(race.HatedRaces)) })
                foreach (var other in list ?? [])
                {
                    if (other == race.Id)
                        throw new ArgumentException($"The race \"{race.Id}\" lists itself in {name}.", nameof(parameters));
                    if (other is null || !ids.Contains(other))
                        throw new ArgumentException($"The race \"{race.Id}\" lists \"{other}\" in {name}, which is not a race of the Parameter Set.", nameof(parameters));
                }
            var both = (race.AppreciatedRaces ?? []).Intersect(race.HatedRaces ?? [], StringComparer.Ordinal).FirstOrDefault();
            if (both is not null)
                throw new ArgumentException($"The race \"{race.Id}\" both appreciates and hates \"{both}\".", nameof(parameters));
        }
        _races = parameters.Races.Where(r => r.Enabled).ToArray();

        // Sorted by height; points at the same height keep the order of the list (the sort is stable).
        _heightPoints = _races
            .Select(r => (r.HeightModifier ?? [])
                .Where(point => double.IsFinite(point.HeightM))
                .OrderBy(point => point.HeightM)
                .ToArray())
            .ToArray();

        // 3.3 The turn pattern: the order of the races and the offset come from the seed.
        var count = _races.Length;
        _turnOrder = Enumerable.Range(0, count)
            .OrderBy(k => Hashing.Hash4(seed, TurnPurpose, k, 0))
            .ThenBy(k => k)
            .ToArray();
        _turnOffset = count > 0 ? (int)(Hashing.Hash4(seed, TurnPurpose, -1, 0) % (uint)count) : 0;
        TurnStride = StrideFor(count);
    }

    /// <summary>The races that are on, in the order of the Parameter Set.</summary>
    public IReadOnlyList<RaceParameters> Races => _races;

    /// <summary>3.1 The food supply of a land patch, 0 to 1: the food summary of its resources and a share of its fish.</summary>
    public static double FoodSupply(PatchResources resources) =>
        Math.Clamp(resources.Food + FishShare * resources.AmountOf(Resource.Fish), 0, 1);

    /// <summary>3.1 How many persons a land patch can feed.</summary>
    /// <param name="areaKm2">The area of the patch.</param>
    /// <param name="prosperity">The Prosperity field at the site of the patch, rescaled to 0–1.</param>
    public double Capacity(double areaKm2, PatchResources resources, double prosperity)
    {
        var effect = Math.Clamp(_parameters.ProsperityEffect, 0, 1);
        var factor = 1 + effect * (2 * Math.Clamp(prosperity, 0, 1) - 1);
        return Math.Max(0, areaKm2) * Math.Max(0, _parameters.PersonsPerKm2) * FoodSupply(resources) * factor;
    }

    /// <summary>
    /// 3.2 The height factor of a race that is on (by its index in <see cref="Races"/>) at a height:
    /// a straight line between its control points and constant beyond them. Of two points at the
    /// same height the first holds below that height and the second from it and up. 1 for a
    /// race without a height modifier.
    /// </summary>
    public double HeightFactor(int race, double heightM)
    {
        var points = _heightPoints[race];
        if (points.Length == 0) return 1;
        if (heightM < points[0].HeightM) return Factor(points[0]);

        // The last point at or below the height.
        var at = 0;
        while (at + 1 < points.Length && points[at + 1].HeightM <= heightM) at++;
        if (at + 1 == points.Length) return Factor(points[at]);

        var (a, b) = (points[at], points[at + 1]);
        var t = (heightM - a.HeightM) / (b.HeightM - a.HeightM);
        return Factor(a) + t * (Factor(b) - Factor(a));
    }

    /// <summary>
    /// 3.2 The share of each race that is on in a biome at a height, in the order of
    /// <see cref="Races"/>: its frequency in the biome times its height factor, over the sum of
    /// these. Empty when none of them lives there.
    /// </summary>
    public double[] Shares(Biome biome, double heightM)
    {
        var shares = new double[_races.Length];
        var sum = 0.0;
        for (var k = 0; k < shares.Length; k++)
        {
            shares[k] = Frequency(_races[k], biome) * HeightFactor(k, heightM);
            sum += shares[k];
        }
        if (sum <= 0) return [];
        for (var k = 0; k < shares.Length; k++) shares[k] /= sum;
        return shares;
    }

    /// <summary>
    /// 3.3 The stride W of the turn pattern: region (i, j) has the turn number (i + W·j + offset) mod M,
    /// with M the number of races that are on. 6 for 28 races.
    /// </summary>
    public int TurnStride { get; }

    /// <summary>
    /// 3.3 The stride of the turn pattern for a number of races: the W in 1 … M−1 for which the
    /// shortest step between two regions with the same turn number is the longest, and the
    /// lowest such W. 0 for fewer than two races.
    /// </summary>
    public static int StrideFor(int races)
    {
        var best = 0;
        long bestLength = -1;
        for (var w = 1; w < races; w++)
        {
            // The shortest non-zero (di, dj) with di + w·dj ≡ 0 (mod races), by its squared length.
            // (races, 0) is always one, so dj beyond races never gives a shorter one.
            var shortest = (long)races * races;
            for (var dj = 0; dj <= races; dj++)
                for (var di = -races; di <= races; di++)
                {
                    if (di == 0 && dj == 0) continue;
                    if (((di + (long)w * dj) % races + races) % races != 0) continue;
                    shortest = Math.Min(shortest, (long)di * di + (long)dj * dj);
                }
            if (shortest > bestLength)
            {
                bestLength = shortest;
                best = w;
            }
        }
        return best;
    }

    /// <summary>
    /// 3.3 The race whose turn it is in a region for one of its minority places, as an index into
    /// <see cref="Races"/>; −1 when no race is on or the region has no minority places.
    /// </summary>
    public int TurnRace(GridCoord region, int place)
    {
        var count = _races.Length;
        var places = MinorityPlaces;
        if (count == 0 || places == 0) return -1;
        var number = region.I + (long)TurnStride * region.J + _turnOffset + (long)place * (count / places);
        return _turnOrder[(int)((number % count + count) % count)];
    }

    /// <summary>The most races a region holds.</summary>
    private int RegionPlaces => Math.Max(0, _parameters.RacesPerRegion);

    /// <summary>How many of the places of a region are minority places.</summary>
    private int MinorityPlaces => Math.Clamp(_parameters.MinorityRacesPerRegion, 0, RegionPlaces);

    /// <summary>3.3 and 3.4 The potential population of a region from its land patches, taken in the given order.</summary>
    public PotentialPopulation Region(IReadOnlyList<PopulationPatch> landPatches, GridCoord region = default) =>
        Share(region, landPatches).Population;

    /// <summary>
    /// 3.3 and 3.4 The races of a region and how they share its land patches, taken in the given
    /// order: the main races are the largest by the plain shares, a minority place goes to the
    /// race whose turn it is or to a drawn one, and only the races of the region share the land.
    /// </summary>
    public RegionRaces Share(GridCoord region, IReadOnlyList<PopulationPatch> landPatches)
    {
        var count = _races.Length;

        // The plain shares: every race that is on shares every patch.
        var capacity = new double[landPatches.Count];
        var shares = new double[landPatches.Count][];
        var plain = new double[count];
        for (var p = 0; p < landPatches.Count; p++)
        {
            var patch = landPatches[p];
            shares[p] = Shares(patch.Biome, patch.HeightM);
            if (shares[p].Length == 0) continue;
            capacity[p] = Capacity(patch.AreaKm2, patch.Resources, patch.Prosperity);
            for (var k = 0; k < count; k++) plain[k] += capacity[p] * shares[p][k];
        }

        // The main races: the largest by the plain shares; ties in the order of the Parameter Set.
        var minorityPlaces = MinorityPlaces;
        var races = Enumerable.Range(0, count)
            .Where(k => plain[k] > 0)
            .OrderByDescending(k => plain[k])
            .ThenBy(k => k)
            .Take(RegionPlaces - minorityPlaces)
            .ToList();
        var mainCount = races.Count;

        // The minority places: the race whose turn it is, or a drawn one. One number is taken
        // per place, also for a place that is taken by turn.
        var rng = new Sfc32(Hashing.Hash4(_seed, MinorityDrawPurpose, region.I, region.J));
        var minimum = Math.Max(0, _parameters.MinorityMinPersons);
        for (var place = 0; place < minorityPlaces; place++)
        {
            var u = rng.Next();
            var eligible = Enumerable.Range(0, count)
                .Where(k => plain[k] > 0 && plain[k] >= minimum && !races.Contains(k))
                .ToArray();
            if (eligible.Length == 0) continue;
            var turn = TurnRace(region, place);
            races.Add(Array.IndexOf(eligible, turn) >= 0 ? turn : Draw(eligible, k => plain[k], u));
        }

        // 3.4 The lift of a minority race: from its share when the races of the region share the land unboosted.
        var boost = new double[races.Count];
        Array.Fill(boost, 1.0);
        var target = Math.Clamp(_parameters.MinorityShare, 0, 0.9);
        if (target > 0 && races.Count > mainCount)
        {
            var (unboosted, _) = Shared(races, boost, capacity, shares);
            var total = unboosted.Sum();
            for (var r = mainCount; r < races.Count; r++)
            {
                var share = total > 0 ? unboosted[r] / total : 0;
                if (share > 0 && share < target) boost[r] = target * (1 - share) / (share * (1 - target));
            }
        }

        var (ofRace, patchPersons) = Shared(races, boost, capacity, shares);
        var persons = new double[count];
        for (var r = 0; r < races.Count; r++) persons[races[r]] = ofRace[r];

        var population = new PotentialPopulation(_races.Select((race, k) =>
        {
            var at = races.IndexOf(k);
            var standing = at < 0 ? RaceStanding.Absent : at < mainCount ? RaceStanding.Main : RaceStanding.Minority;
            return new RacePopulation(race.Id, (long)Math.Floor(persons[k] + 0.5), standing);
        }).ToArray());

        return new RegionRaces
        {
            Races = races.ToArray(),
            MainCount = mainCount,
            Plain = plain,
            Persons = persons,
            Boost = boost,
            PatchPersons = patchPersons,
            Population = population,
        };
    }

    /// <summary>
    /// 3.4 The races of a region share each patch by their plain weight times their boost. A
    /// patch where none of them lives holds nobody.
    /// </summary>
    private static (double[] OfRace, double[][] OfPatch) Shared(List<int> races, double[] boost, double[] capacity, double[][] shares)
    {
        var ofRace = new double[races.Count];
        var ofPatch = new double[shares.Length][];
        for (var p = 0; p < shares.Length; p++)
        {
            var persons = new double[races.Count];
            ofPatch[p] = persons;
            if (shares[p].Length == 0) continue;
            var sum = 0.0;
            for (var r = 0; r < races.Count; r++)
            {
                persons[r] = shares[p][races[r]] * boost[r];
                sum += persons[r];
            }
            if (sum <= 0)
            {
                Array.Clear(persons);
                continue;
            }
            for (var r = 0; r < races.Count; r++)
            {
                persons[r] *= capacity[p] / sum;
                ofRace[r] += persons[r];
            }
        }
        return (ofRace, ofPatch);
    }

    /// <summary>
    /// 5.1 The races of a city or a town on a Prospect, as indexes into <see cref="Races"/>: the
    /// largest races of the region around the Prospect first, then a last one drawn among the
    /// other races of the region, with its minority races weighted up.
    /// </summary>
    /// <param name="region">The races of the region and how they share its land.</param>
    /// <param name="patches">The patch of the Prospect and its neighbouring land patches of the region, as indexes into the land patches the region was shared over.</param>
    /// <param name="forCity">The Prospect is for a city; otherwise for a town.</param>
    /// <param name="prospectPatch">The root cell of the patch of the Prospect: the identity the draw is seeded from.</param>
    public int[] SettlementRaces(RegionRaces region, IEnumerable<int> patches, bool forCity, GridCoord prospectPatch)
    {
        var places = Math.Max(0, forCity ? _parameters.RacesPerCity : _parameters.RacesPerTown);
        if (places == 0 || region.Races.Length == 0) return [];

        var weight = SettlementWeights(region, patches);

        // Positions in the region's race list, in the order of the Parameter Set.
        var present = Enumerable.Range(0, weight.Length)
            .Where(r => weight[r] > 0)
            .OrderBy(r => region.Races[r])
            .ToArray();
        var main = present.OrderByDescending(r => weight[r]).ThenBy(r => region.Races[r]).Take(places - 1).ToList();
        var rest = present.Where(r => !main.Contains(r)).ToArray();
        if (rest.Length > 0)
        {
            var minorityBoost = Math.Max(0, _parameters.MinorityBoost);
            var u = Hashing.Unit(_seed, SettlementDrawPurpose, prospectPatch.I, prospectPatch.J);
            main.Add(Draw(rest, r => weight[r] * (r >= region.MainCount ? minorityBoost : 1), u));
        }
        return main.Select(r => region.Races[r]).ToArray();
    }

    /// <summary>
    /// 5.1 The weight of each race of a region in a city or a town on a Prospect, in the order of
    /// <see cref="RegionRaces.Races"/>: its persons on the patches around the Prospect, and a
    /// share of its persons in the whole region.
    /// </summary>
    /// <param name="patches">The patch of the Prospect and its neighbouring land patches of the region, as indexes into the land patches the region was shared over.</param>
    public double[] SettlementWeights(RegionRaces region, IEnumerable<int> patches)
    {
        var weight = new double[region.Races.Length];
        foreach (var p in patches)
            for (var r = 0; r < weight.Length; r++) weight[r] += region.PatchPersons[p][r];
        var hinterland = Math.Clamp(_parameters.HinterlandShare, 0, 1);
        for (var r = 0; r < weight.Length; r++) weight[r] += hinterland * region.Persons[region.Races[r]];
        return weight;
    }

    /// <summary>One of the candidates by weight, walked in the order given; the first one when no weight is positive.</summary>
    private static int Draw(IReadOnlyList<int> candidates, Func<int, double> weight, double u)
    {
        var total = 0.0;
        foreach (var c in candidates) total += Math.Max(0, weight(c));
        if (total <= 0) return candidates[0];
        var target = u * total;
        var sum = 0.0;
        var last = candidates[0];
        foreach (var c in candidates)
        {
            var w = Math.Max(0, weight(c));
            if (w <= 0) continue;
            sum += w;
            last = c;
            if (target < sum) return c;
        }
        return last;
    }

    private static double Frequency(RaceParameters race, Biome biome)
    {
        var f = race.BiomeFrequency[biome];
        return f > 0 && double.IsFinite(f) ? f : 0;
    }

    private static double Factor(HeightFactorPoint point) =>
        point.Factor > 0 && double.IsFinite(point.Factor) ? point.Factor : 0;
}
