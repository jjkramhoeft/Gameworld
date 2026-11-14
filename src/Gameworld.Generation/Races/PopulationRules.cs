using Gameworld.Domain;

namespace Gameworld.Generation.Races;

/// <summary>
/// The potential population of the land and how the races share it (map-generation.md
/// section 19). There is no random choice in it: it is a plain function of the Parameter Set
/// and of the biome, the resources and the Prosperity of a patch.
/// </summary>
public sealed class PopulationRules
{
    /// <summary>How much the fish of a patch adds to its food supply.</summary>
    public const double FishShare = 0.2;

    private static readonly Biome[] Biomes = Enum.GetValues<Biome>();

    private readonly PopulationParameters _parameters;
    private readonly RaceParameters[] _races;
    /// <summary>Per biome: the share of each race that is on, in the order of <see cref="Races"/>; null where none of them lives.</summary>
    private readonly double[]?[] _shares;

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

        _shares = new double[]?[Biomes.Length];
        foreach (var biome in Biomes)
        {
            var frequencies = _races.Select(r => Frequency(r, biome)).ToArray();
            var sum = 0.0;
            foreach (var f in frequencies) sum += f;
            if (sum <= 0) continue;
            for (var k = 0; k < frequencies.Length; k++) frequencies[k] /= sum;
            _shares[(int)biome] = frequencies;
        }
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
    /// 19.2 The share of each race that is on in a biome, in the order of <see cref="Races"/>:
    /// its frequency over the sum of the frequencies. Empty when none of them lives in the biome.
    /// </summary>
    public ReadOnlySpan<double> Shares(Biome biome) => _shares[(int)biome];

    /// <summary>19.3 The potential population of a region from its land patches, taken in the given order.</summary>
    public PotentialPopulation Region(IEnumerable<(Biome Biome, double AreaKm2, PatchResources Resources, double Prosperity)> landPatches)
    {
        var persons = new double[_races.Length];
        foreach (var patch in landPatches)
        {
            var shares = Shares(patch.Biome);
            if (shares.IsEmpty) continue;
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
}
