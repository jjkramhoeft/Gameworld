using Gameworld.Domain;

namespace Gameworld.Generation.Races;

/// <summary>
/// The potential population of the land and how the races share it (map-generation.md
/// section 19). There is no random choice in it: it is a plain function of the Parameter Set
/// and of the biome, the height, the resources and the Prosperity of a patch.
/// </summary>
public sealed class PopulationRules
{
    /// <summary>How much the fish of a patch adds to its food supply.</summary>
    public const double FishShare = 0.2;

    private readonly PopulationParameters _parameters;
    private readonly RaceParameters[] _races;
    /// <summary>Per race that is on: the control points of its height modifier, by height; empty when the height does not matter to it.</summary>
    private readonly HeightFactorPoint[][] _heightPoints;

    public PopulationRules(ParameterSet parameters)
    {
        _parameters = parameters.Planning.Population;

        var ids = new HashSet<string>(StringComparer.Ordinal);
        foreach (var race in parameters.Races)
        {
            if (string.IsNullOrWhiteSpace(race.Id))
                throw new ArgumentException("A race of the Parameter Set has no id.", nameof(parameters));
            if (!ids.Add(race.Id))
                throw new ArgumentException($"The Parameter Set lists the race id \"{race.Id}\" more than once.", nameof(parameters));
        }
        _races = parameters.Races.Where(r => r.Enabled).ToArray();

        // Sorted by height; points at the same height keep the order of the list (the sort is stable).
        _heightPoints = _races
            .Select(r => (r.HeightModifier ?? [])
                .Where(point => double.IsFinite(point.HeightM))
                .OrderBy(point => point.HeightM)
                .ToArray())
            .ToArray();
    }

    /// <summary>The races that are on, in the order of the Parameter Set.</summary>
    public IReadOnlyList<RaceParameters> Races => _races;

    /// <summary>19.1 The food supply of a land patch, 0 to 1: the food summary of its resources and a share of its fish.</summary>
    public static double FoodSupply(PatchResources resources) =>
        Math.Clamp(resources.Food + FishShare * resources.AmountOf(Resource.Fish), 0, 1);

    /// <summary>19.1 How many persons a land patch can feed.</summary>
    /// <param name="areaKm2">The area of the patch.</param>
    /// <param name="prosperity">The Prosperity field at the site of the patch, rescaled to 0–1.</param>
    public double Capacity(double areaKm2, PatchResources resources, double prosperity)
    {
        var effect = Math.Clamp(_parameters.ProsperityEffect, 0, 1);
        var factor = 1 + effect * (2 * Math.Clamp(prosperity, 0, 1) - 1);
        return Math.Max(0, areaKm2) * Math.Max(0, _parameters.PersonsPerKm2) * FoodSupply(resources) * factor;
    }

    /// <summary>
    /// 19.2 The height factor of a race that is on (by its index in <see cref="Races"/>) at a height:
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
    /// 19.2 The share of each race that is on in a biome at a height, in the order of
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

    /// <summary>19.3 The potential population of a region from its land patches, taken in the given order.</summary>
    public PotentialPopulation Region(
        IEnumerable<(Biome Biome, double HeightM, double AreaKm2, PatchResources Resources, double Prosperity)> landPatches)
    {
        var persons = new double[_races.Length];
        foreach (var patch in landPatches)
        {
            var shares = Shares(patch.Biome, patch.HeightM);
            if (shares.Length == 0) continue;
            var capacity = Capacity(patch.AreaKm2, patch.Resources, patch.Prosperity);
            for (var k = 0; k < shares.Length; k++) persons[k] += capacity * shares[k];
        }
        return new PotentialPopulation(
            _races.Select((race, k) => new RacePopulation(race.Id, (long)Math.Floor(persons[k] + 0.5))).ToArray());
    }

    private static double Frequency(RaceParameters race, Biome biome)
    {
        var f = race.BiomeFrequency[biome];
        return f > 0 && double.IsFinite(f) ? f : 0;
    }

    private static double Factor(HeightFactorPoint point) =>
        point.Factor > 0 && double.IsFinite(point.Factor) ? point.Factor : 0;
}
