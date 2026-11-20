using Gameworld.Domain;
using Gameworld.Generation.Basics;
using Gameworld.Generation.Races;

namespace Gameworld.Generation.Factions;

/// <summary>A land patch as the factions read it: its final biome and resources, its zone and the Supernatural field.</summary>
public readonly record struct FactionPatch(
    double AreaKm2, Biome Biome, ElevationType ZoneType, PatchResources Resources, double Supernatural);

/// <summary>A Prospect as the factions read it.</summary>
/// <param name="Races">The races of a city or a town on the Prospect, as indexes into <see cref="PopulationRules.Races"/>.</param>
/// <param name="Around">The patch of the Prospect and its neighbouring land patches of the region, as indexes into the land patches.</param>
/// <param name="LocalPersons">Per race of the region, in the order of <see cref="RegionRaces.Races"/>: the persons of the race on the patches around the Prospect.</param>
public sealed record FactionProspect(Prospect Prospect, int[] Races, int[] Around, double[] LocalPersons);

/// <summary>
/// How the factions read a region (society-generation.md section 4.3): the indices from 0 to 1,
/// and the plain numbers the fits of the lesser factions read.
/// </summary>
public sealed class FactionReading
{
    /// <summary>The area-weighted mean amount of each resource over the land, by resource index.</summary>
    public required double[] Mean { get; init; }
    /// <summary>The share of the land area with a listed amount of each resource, by resource index.</summary>
    public required double[] Listed { get; init; }
    /// <summary>The persons of the region, not rounded.</summary>
    public required double Persons { get; init; }
    /// <summary>The Prospects on show with a harbour or a navigable river mouth.</summary>
    public required int Harbours { get; init; }
    /// <summary>The best Trade factor of a Prospect on show.</summary>
    public required double BestTradeFactor { get; init; }
    /// <summary>A Prospect for a city is on show.</summary>
    public required bool HasCity { get; init; }

    public required double Grain { get; init; }
    public required double Herd { get; init; }
    public required double Open { get; init; }
    public required double Forest { get; init; }
    public required double Cold { get; init; }
    public required double Dry { get; init; }
    public required double Rough { get; init; }
    public required double Trade { get; init; }
    public required double Wealth { get; init; }
    public required double Urban { get; init; }
    public required double Ore { get; init; }
    public required double Precious { get; init; }
    public required double Arcane { get; init; }
    public required double People { get; init; }
    public required double Small { get; init; }
    public required double Mixed { get; init; }
    public required double Poor { get; init; }
    public required double Uneven { get; init; }

    public double MeanOf(Resource resource) => Mean[(int)resource];
    public double ListedOf(Resource resource) => Listed[(int)resource];
}

/// <summary>The terms of the tension of a region, each 0 to 1 (society-generation.md section 4.5).</summary>
public readonly record struct TensionTerms(double Contest, double RoughLand, double RivalSeat, double PreciousFinds, double PoorLand);

/// <summary>The working data behind the factions of a region.</summary>
public sealed class FactionWork
{
    public static FactionWork None { get; } = new() { Reading = null, Fits = [], Terms = default, Tension = 0, Factions = [] };

    /// <summary>How the region was read; null in a region without persons.</summary>
    public required FactionReading? Reading { get; init; }
    /// <summary>The fit of each type as the ruler, by <see cref="FactionType"/>.</summary>
    public required double[] Fits { get; init; }
    public required TensionTerms Terms { get; init; }
    /// <summary>The tension of the region, without its random part.</summary>
    public required double Tension { get; init; }
    /// <summary>The factions of the region, by slot.</summary>
    public required IReadOnlyList<Faction> Factions { get; init; }

    /// <summary>The ruling faction; null in a region without persons.</summary>
    public Faction? Ruling => Factions.FirstOrDefault(f => f.Role == FactionRole.Ruling);
}

/// <summary>A kind of lesser faction (society-generation.md section 4.7).</summary>
/// <param name="Vocations">The ids of the vocations the kind gathers.</param>
/// <param name="SeatResources">The resources that pull its seat to a Prospect.</param>
/// <param name="OnWater">Its seat wants a harbour, a river mouth or a navigable river.</param>
public sealed record LesserKind(
    string Id, string Name, FactionType Type, string[] Vocations, Resource[] SeatResources, bool OnWater, Func<FactionReading, double> Fit);

/// <summary>
/// The factions of a region (society-generation.md section 4): one ruling faction, an opposition
/// where the tension of the region is high, and the lesser factions that fit it. A pure function
/// of the region's own land, races and Prospects; nothing is read from another region.
/// </summary>
public sealed class FactionRules
{
    /// <summary>Hash purpose of the draw for the ruling faction.</summary>
    public const uint RulingPurpose = 0x4b01;
    /// <summary>Hash purpose of the random part of the tension.</summary>
    public const uint TensionPurpose = 0x4b02;
    /// <summary>Hash purpose of the draw for the opposition.</summary>
    public const uint OppositionPurpose = 0x4b03;
    /// <summary>Hash purpose of the random part of the fits of the lesser factions.</summary>
    public const uint LesserPurpose = 0x4b04;
    /// <summary>Hash purpose of the draws for the lead races of the lesser factions.</summary>
    public const uint LesserRacePurpose = 0x4b05;
    /// <summary>Hash purpose of the draws for the colours of the factions.</summary>
    public const uint ColourPurpose = 0x4b06;

    /// <summary>The id of the kind Enclave: its fit comes from the races, not from the land.</summary>
    public const string EnclaveKind = "enclave";

    /// <summary>A river mouth this large, in patches drained, counts as a harbour: the river is navigable.</summary>
    private const double NavigableMouth = 8;

    private const int Types = 6;

    private static readonly Biome[] OpenBiomes =
        [Biome.ColdSteppe, Biome.TemperateGrassland, Biome.Savanna, Biome.ColdDesert, Biome.HotDesert, Biome.ThornScrub, Biome.Tundra];
    private static readonly Biome[] ForestBiomes =
    [
        Biome.BorealForest, Biome.TemperateMixedForest, Biome.TemperateBroadleafForest, Biome.TemperateRainForest,
        Biome.TropicalDryForest, Biome.TropicalRainForest, Biome.Mangrove, Biome.WoodlandAndShrubland,
    ];
    private static readonly Biome[] ColdBiomes = [Biome.IceAndSnow, Biome.Tundra, Biome.BorealForest];
    private static readonly Biome[] DryBiomes = [Biome.HotDesert, Biome.ColdDesert, Biome.ColdSteppe, Biome.ThornScrub];

    private static readonly Resource[] OreResources =
    [
        Resource.IronOre, Resource.CopperOre, Resource.Tin, Resource.SilverAndLead, Resource.Gold,
        Resource.GemsAndPearls, Resource.Salt,
    ];
    private static readonly Resource[] PreciousResources = [Resource.SilverAndLead, Resource.Gold, Resource.GemsAndPearls];

    /// <summary>
    /// The kinds of lesser faction, in the order their random numbers are taken in. Fifteen are
    /// guilds of one trade. The merchants have a floor in a region with a city and the four
    /// trades of primary production a factor of 0.9.
    /// </summary>
    public static IReadOnlyList<LesserKind> Kinds { get; } =
    [
        new("merchants", "Merchants' guild", FactionType.Vocation, ["merchant", "shopkeeper", "carter"], [], false,
            r => (0.4 + 0.6 * (0.65 * r.Trade + 0.35 * r.Wealth)) * City(r)),
        new("miners", "Miners' guild", FactionType.Vocation, ["miner", "quarryman", "smelter"],
            [Resource.IronOre, Resource.CopperOre, Resource.Tin, Resource.SilverAndLead, Resource.Gold, Resource.GemsAndPearls], false,
            r => 0.9 * r.Ore),
        new("smiths", "Smiths' guild", FactionType.Vocation, ["smith", "artificer"], [Resource.IronOre], false,
            r => Ramp(r.ListedOf(Resource.IronOre), 0.02, 0.08) * (0.5 + 0.5 * Ramp(r.MeanOf(Resource.Fuel), 0.33, 0.6)) * City(r)),
        new("mariners", "Mariners' guild", FactionType.Vocation, ["sailor", "fisher", "shipwright"], [Resource.Fish], true,
            r => Ramp(r.Harbours, 0, 3) * (0.5 + 0.5 * Ramp(r.MeanOf(Resource.Fish), 0.28, 0.41))),
        new("clothiers", "Clothiers' guild", FactionType.Vocation, ["weaver", "tailor", "tanner", "leatherworker"],
            [Resource.Fibre, Resource.Dyes], false,
            r => (0.5 * Ramp(r.MeanOf(Resource.Fibre), 0.2, 0.47) + 0.3 * Ramp(r.MeanOf(Resource.Dyes), 0.03, 0.17)
                  + 0.2 * Ramp(r.MeanOf(Resource.Livestock), 0.33, 0.53)) * City(r)),
        new("builders", "Builders' guild", FactionType.Vocation, ["mason", "carpenter"], [Resource.Stone, Resource.Timber], false,
            r => (0.6 * Ramp(r.MeanOf(Resource.Stone), 0.04, 0.22) + 0.4 * Ramp(r.MeanOf(Resource.Timber), 0.3, 0.65))
                 * City(r) * (0.5 + 0.5 * r.Urban)),
        new("salters", "Salters' guild", FactionType.Vocation, ["salter"], [Resource.Salt], false,
            r => 0.9 * Ramp(r.ListedOf(Resource.Salt), 0, 0.04)),
        new("caravaneers", "Caravan company", FactionType.Vocation, ["carter", "merchant", "innkeeper"],
            [Resource.HorsesAndPackAnimals], false,
            r => r.Dry * Ramp(r.MeanOf(Resource.HorsesAndPackAnimals), 0.22, 0.4) * (0.5 + 0.5 * Ramp(r.BestTradeFactor, 0.5, 1))),
        new("foresters", "Foresters' guild", FactionType.Vocation, ["forester", "hunter", "charcoalBurner"],
            [Resource.Timber, Resource.FursAndIvory], false,
            r => 0.9 * (0.5 * Ramp(r.MeanOf(Resource.Timber), 0.3, 0.65) + 0.5 * Ramp(r.MeanOf(Resource.FursAndIvory), 0.02, 0.21))),
        new("drovers", "Drovers' guild", FactionType.Vocation, ["herder", "butcher"],
            [Resource.Livestock, Resource.HorsesAndPackAnimals], false,
            r => 0.9 * r.Herd * r.Open),
        new("apothecaries", "Apothecaries' guild", FactionType.Vocation, ["healer", "alchemist"],
            [Resource.Herbs, Resource.Reagents, Resource.Spices], false,
            r => 0.4 * Ramp(r.MeanOf(Resource.Herbs), 0.3, 0.52) + 0.3 * Ramp(r.MeanOf(Resource.Reagents), 0, 0.015)
                 + 0.3 * Ramp(r.MeanOf(Resource.Spices), 0, 0.12)),
        new("victuallers", "Victuallers' guild", FactionType.Vocation, ["miller", "baker", "brewer", "innkeeper"],
            [Resource.Grain, Resource.Orchard], false,
            r => (0.5 * r.Grain + 0.5 * Ramp(r.MeanOf(Resource.Orchard), 0.22, 0.56)) * r.Urban * City(r)),
        new("goldsmiths", "Goldsmiths' guild", FactionType.Vocation, ["jeweller"],
            [Resource.Gold, Resource.SilverAndLead, Resource.GemsAndPearls], false,
            r => r.Precious * (0.5 + 0.5 * r.Wealth) * City(r)),
        new("freeCompany", "Free company", FactionType.Vocation, ["soldier", "guard"], [], false,
            r => 0.5 * r.Rough + 0.5 * r.Poor),
        new("thieves", "Thieves' guild", FactionType.Vocation, ["thief", "beggar"], [], false,
            r => r.Wealth * r.Urban * (0.5 + 0.5 * r.Uneven) * City(r)),
        new("order", "Order", FactionType.Belief, ["priest", "scribe", "healer"], [], false,
            r => 0.35 + 0.65 * Math.Max(r.Arcane, 0.6 * r.Poor)),
        new("circle", "Circle", FactionType.Magic, ["mage", "alchemist", "seer"], [Resource.ArcaneMaterials], false,
            r => r.Arcane),
        new("greatHouse", "Great house", FactionType.Family, ["noble"], [], false,
            r => 0.8 * (0.6 * r.Grain + 0.4 * r.People)),
        new("charteredTown", "Chartered town", FactionType.Place, [], [], false,
            r => r.Urban * (0.4 + 0.6 * r.Trade)),
        new(EnclaveKind, "Enclave", FactionType.Race, [], [], false, _ => 0),
    ];

    /// <summary>The name of a kind of lesser faction; the id itself for an id that is not a kind.</summary>
    public static string KindName(string id) => Kinds.FirstOrDefault(k => k.Id == id)?.Name ?? id;

    private readonly FactionParameters _parameters;
    private readonly double _minorityBoost;
    private readonly RaceParameters[] _races;
    private readonly HashSet<string> _vocations;
    /// <summary>The colours that are colours, in the order of the Parameter Set, with their lightness.</summary>
    private readonly (string Id, double Lightness)[] _colours;
    /// <summary>Per race that is on, per colour of <see cref="_colours"/>: the place of the colour among the favourite colours of the race from 1, or 0.</summary>
    private readonly int[][] _favourite;
    private readonly uint _seed;

    /// <param name="seed">The seed of the foundation: it gives the draws.</param>
    public FactionRules(ParameterSet parameters, uint seed = 0)
    {
        _parameters = parameters.Planning.Factions;
        _minorityBoost = Math.Max(0, parameters.Planning.Population.MinorityBoost);
        _races = parameters.Races.Where(r => r.Enabled).ToArray();
        _vocations = parameters.Vocations.Where(v => v.Enabled).Select(v => v.Id).ToHashSet(StringComparer.Ordinal);
        _colours = parameters.Colours
            .Select(c => (c.Id, Known: ColourParameters.TryLightness(c.Hex, out var lightness), Lightness: lightness))
            .Where(c => c.Known && !string.IsNullOrWhiteSpace(c.Id))
            .DistinctBy(c => c.Id)
            .Select(c => (c.Id, c.Lightness))
            .ToArray();
        _favourite = _races
            .Select(r =>
            {
                // The place of a colour that is listed twice is its first one.
                var favourites = (r.FavouriteColours ?? []).Where(id => Array.Exists(_colours, c => c.Id == id)).Distinct().ToArray();
                return _colours.Select(c => Array.IndexOf(favourites, c.Id) + 1).ToArray();
            })
            .ToArray();
        _seed = seed;
    }

    /// <summary>4.3 How the factions read a region.</summary>
    public FactionReading Read(IReadOnlyList<FactionPatch> land, RegionRaces races, IReadOnlyList<FactionProspect> prospects)
    {
        var ranges = _parameters.Indices;
        var mean = new double[ResourceCatalog.Count];
        var listed = new double[ResourceCatalog.Count];
        double area = 0, open = 0, forest = 0, cold = 0, dry = 0, hills = 0, mountains = 0, arcane = 0;
        foreach (var patch in land)
        {
            var a = Math.Max(0, patch.AreaKm2);
            area += a;
            for (var k = 0; k < mean.Length; k++)
            {
                var amount = patch.Resources.Amounts[k].Amount;
                mean[k] += a * amount;
                if (amount >= ResourceCatalog.ListedAmount) listed[k] += a;
            }
            if (Array.IndexOf(OpenBiomes, patch.Biome) >= 0) open += a;
            if (Array.IndexOf(ForestBiomes, patch.Biome) >= 0) forest += a;
            if (Array.IndexOf(ColdBiomes, patch.Biome) >= 0) cold += a;
            if (Array.IndexOf(DryBiomes, patch.Biome) >= 0) dry += a;
            if (patch.ZoneType == ElevationType.Hills) hills += a;
            if (patch.ZoneType == ElevationType.Mountains) mountains += a;
            arcane += a * Math.Clamp((patch.Supernatural - 60) / 40, 0, 1);
        }
        if (area > 0)
        {
            for (var k = 0; k < mean.Length; k++)
            {
                mean[k] /= area;
                listed[k] /= area;
            }
            open /= area; forest /= area; cold /= area; dry /= area; hills /= area; mountains /= area; arcane /= area;
        }

        var persons = races.Persons.Sum();
        var largest = persons > 0 ? races.Persons.Max() / persons : 1;

        int onShow = 0, rich = 0, poor = 0, harbours = 0;
        double bestTrade = 0, prosperitySum = 0, highest = double.NegativeInfinity, lowest = double.PositiveInfinity;
        var city = false;
        foreach (var p in prospects)
        {
            var prospect = p.Prospect;
            prosperitySum += prospect.Factors.Prosperity;
            if (!prospect.OnShow) continue;
            onShow++;
            if (prospect.Class == ProspectClass.Rich) rich++;
            if (prospect.Class == ProspectClass.Poor) poor++;
            if (OnHarbour(prospect)) harbours++;
            if (prospect.ForCity) city = true;
            bestTrade = Math.Max(bestTrade, prospect.Factors.Trade);
            highest = Math.Max(highest, prospect.Factors.Prosperity);
            lowest = Math.Min(lowest, prospect.Factors.Prosperity);
        }
        var prosperity = prospects.Count > 0 ? prosperitySum / prospects.Count : 0;

        double ListedSum(Resource[] resources)
        {
            var sum = 0.0;
            foreach (var r in resources) sum += listed[(int)r];
            return sum;
        }

        return new FactionReading
        {
            Mean = mean,
            Listed = listed,
            Persons = persons,
            Harbours = harbours,
            BestTradeFactor = bestTrade,
            HasCity = city,
            Grain = Ramp(mean[(int)Resource.Grain], ranges.Grain),
            Herd = 0.5 * Ramp(mean[(int)Resource.Livestock], ranges.Livestock)
                 + 0.5 * Ramp(mean[(int)Resource.HorsesAndPackAnimals], ranges.Horses),
            Open = Ramp(open, ranges.OpenLand),
            Forest = Ramp(forest, ranges.Forest),
            Cold = Ramp(cold, ranges.ColdLand),
            Dry = Ramp(dry, ranges.DryLand),
            Rough = Ramp(hills + 1.5 * mountains, ranges.RoughLand),
            Trade = 0.5 * Ramp(bestTrade, ranges.TradeFactor) + 0.5 * Ramp(harbours, ranges.Harbours),
            Wealth = 0.5 * Ramp(prosperity, ranges.Prosperity) + 0.5 * Ramp(rich, ranges.RichProspects),
            Urban = Ramp(onShow, ranges.ProspectsOnShow),
            Ore = Ramp(ListedSum(OreResources), ranges.Ore),
            Precious = Ramp(ListedSum(PreciousResources), ranges.Precious),
            Arcane = Ramp(arcane, ranges.Supernatural),
            People = Ramp(persons, ranges.Persons),
            Small = 1 - Ramp(persons, ranges.SmallRegion),
            Mixed = 1 - Ramp(largest, ranges.LargestRace),
            Poor = onShow > 0 ? Ramp((double)poor / onShow, ranges.PoorProspects) : 0,
            Uneven = onShow > 1 ? Ramp(highest - lowest, ranges.UnevenProsperity) : 0,
        };
    }

    /// <summary>4.4 The fit of the land for each type as the ruler, by <see cref="FactionType"/>, each 0 to 1.</summary>
    public double[] Fits(FactionReading r)
    {
        var b = _parameters.BaseFit;
        static double From(double baseFit, double land) => Math.Clamp(baseFit, 0, 1) + (1 - Math.Clamp(baseFit, 0, 1)) * Math.Clamp(land, 0, 1);

        var fits = new double[Types];
        fits[(int)FactionType.Family] = From(b.Family, 0.5 * r.Grain + 0.3 * r.People + 0.2 * (1 - r.Rough));
        fits[(int)FactionType.Race] = From(b.Race,
            Max(r.Herd * r.Open, 0.6 * r.Rough + 0.4 * r.Forest * (1 - r.Grain), 0.8 * r.Small));
        fits[(int)FactionType.Vocation] = From(b.Vocation, r.Trade * (0.4 + 0.6 * r.Wealth) * (0.5 + 0.5 * r.Urban));
        fits[(int)FactionType.Belief] = From(b.Belief, 0.5 * r.Arcane + 0.15 * r.Mixed + 0.3 * r.Poor * (1 - r.Trade));
        fits[(int)FactionType.Magic] = From(b.Magic, 0.8 * r.Arcane * r.Arcane * r.Arcane);
        fits[(int)FactionType.Place] = From(b.Place,
            Math.Max(Max(r.Urban * r.Trade, 0.8 * r.Small, 0.7 * r.Rough * (1 - r.People)), 0.6 * r.Cold));
        return fits;
    }

    /// <summary>
    /// 4.4 to 4.7 The factions of a region: its ruling faction, its opposition when the tension is
    /// high enough, and its lesser factions. A region without persons has none.
    /// </summary>
    /// <param name="region">The region: the identity the draws are seeded from.</param>
    /// <param name="land">The land patches of the region in their final form.</param>
    /// <param name="races">The races of the region and how they share its land.</param>
    /// <param name="prospects">The Prospects of the region, in their order.</param>
    public FactionWork Build(
        GridCoord region, IReadOnlyList<FactionPatch> land, RegionRaces races, IReadOnlyList<FactionProspect> prospects)
    {
        var persons = races.Persons.Sum();
        if (races.Races.Length == 0 || !(persons > 0)) return FactionWork.None;

        var reading = Read(land, races, prospects);
        var fits = Fits(reading);

        // The races of the region in the order of the Parameter Set, with their share and their standing.
        var members = races.Races.OrderBy(k => k).ToArray();
        int Largest() => members.OrderByDescending(k => races.Persons[k]).ThenBy(k => k).First();
        double Share(int race) => races.Persons[race] / persons;
        double Lean(int race, FactionType type) => Math.Max(0, _races[race].FactionLeaning[type]) / 3;

        // 4.4 The ruling faction: the lead race and the type are drawn together.
        var weights = new double[members.Length, Types];
        var typeWeight = new double[Types];
        var total = 0.0;
        for (var m = 0; m < members.Length; m++)
            for (var t = 0; t < Types; t++)
            {
                var w = Pow(Share(members[m]), _parameters.RacePower) * Lean(members[m], (FactionType)t) * Pow(fits[t], _parameters.Sharpness);
                weights[m, t] = w;
                typeWeight[t] += w;
                total += w;
            }

        int leadRace;
        FactionType rulingType;
        if (total > 0)
        {
            var at = Draw(weights, total, Hashing.Unit(_seed, RulingPurpose, region.I, region.J));
            leadRace = members[at / Types];
            rulingType = (FactionType)(at % Types);
        }
        else
        {
            // No race leans to any type: the largest race and the type with the best fit.
            leadRace = Largest();
            rulingType = (FactionType)Array.IndexOf(fits, fits.Max());
        }

        // 4.5 The tension.
        var onShow = prospects.Where(p => p.Prospect.OnShow).ToArray();
        var best = Best(onShow, p => p.Prospect.Score);
        var rivalSeat = 0.0;
        if (best != null && best.Prospect.Score > 0)
            foreach (var p in onShow)
            {
                if (ReferenceEquals(p, best)) continue;
                var km = p.Prospect.Spot.DistanceTo(best.Prospect.Spot) / 1000;
                rivalSeat = Math.Max(rivalSeat, Ramp(p.Prospect.Score / best.Prospect.Score, 0.8, 1) * Ramp(km, 40, 100));
            }
        var terms = new TensionTerms(
            Contest: total > 0 ? Ramp(1 - typeWeight[(int)rulingType] / total, 0.4, 0.9) : 0,
            RoughLand: reading.Rough,
            RivalSeat: rivalSeat,
            PreciousFinds: reading.Precious,
            PoorLand: 0.5 * reading.Poor + 0.5 * reading.Uneven);
        var tw = _parameters.Tension;
        double[] weighted =
        [
            Math.Max(0, tw.Contest) * terms.Contest,
            Math.Max(0, tw.RoughLand) * terms.RoughLand,
            Math.Max(0, tw.RivalSeat) * terms.RivalSeat,
            Math.Max(0, tw.PreciousFinds) * terms.PreciousFinds,
            Math.Max(0, tw.PoorLand) * terms.PoorLand,
        ];
        var tension = weighted.Sum();

        var rulingSeat = Best(onShow.Where(p => Array.IndexOf(p.Races, leadRace) >= 0), p => p.Prospect.Score) ?? best;
        var factions = new List<Faction>
        {
            new(Faction.RulingSlot, FactionRole.Ruling, rulingType, FormOf(rulingType, FactionRole.Ruling, reading),
                _races[leadRace].Id, SeatOf(rulingSeat), null, [], null, Math.Clamp(1 - tension, 0, 1)),
        };

        // 4.6 The opposition.
        string? oppositionKind = null;
        var jitter = (Hashing.Unit(_seed, TensionPurpose, region.I, region.J) - 0.5) * Math.Max(0, _parameters.TensionJitter);
        if (persons >= _parameters.OppositionMinPersons && tension + jitter >= _parameters.OppositionTension)
        {
            var favour = _parameters.Favour;
            var against = new double[members.Length, Types];
            var sum = 0.0;
            for (var t = 0; t < Types; t++)
            {
                var type = (FactionType)t;
                if (type == FactionType.Magic && reading.Arcane < 0.3) continue;
                var ground = weighted[0] * fits[t]
                    + weighted[1] * favour.RoughLand[type] / 2
                    + weighted[2] * favour.RivalSeat[type] / 2
                    + weighted[3] * favour.PreciousFinds[type] / 2
                    + weighted[4] * favour.PoorLand[type] / 2;
                var cleavage = Math.Max(0, _parameters.Cleavage[rulingType][type]);
                for (var m = 0; m < members.Length; m++)
                {
                    // A people does not oppose a ruler of its own people as a people.
                    if (type == FactionType.Race && members[m] == leadRace) continue;
                    var w = Share(members[m]) * Lean(members[m], type) * cleavage * Math.Max(0, ground);
                    against[m, t] = w;
                    sum += w;
                }
            }
            if (sum > 0)
            {
                var at = Draw(against, sum, Hashing.Unit(_seed, OppositionPurpose, region.I, region.J));
                var race = members[at / Types];
                var type = (FactionType)(at % Types);

                // A guild in opposition is the guild of the trade that fits the region best.
                LesserKind? trade = null;
                if (type == FactionType.Vocation)
                {
                    var bestFit = double.NegativeInfinity;
                    foreach (var kind in Kinds)
                    {
                        if (kind.Type != FactionType.Vocation || !Exists(kind)) continue;
                        var fit = kind.Fit(reading);
                        if (fit > bestFit) (bestFit, trade) = (fit, kind);
                    }
                    oppositionKind = trade?.Id;
                }

                var rulingSpot = rulingSeat?.Prospect.Spot;
                double Far(FactionProspect p) => p.Prospect.Score
                    * (0.5 + 0.5 * Ramp(rulingSpot is { } spot ? p.Prospect.Spot.DistanceTo(spot) / 1000 : 0, 20, 80));
                var others = onShow.Where(p => !ReferenceEquals(p, rulingSeat)).ToArray();
                var seat = Best(others.Where(p => Array.IndexOf(p.Races, race) >= 0), Far) ?? Best(others, Far) ?? rulingSeat;

                var ground = (OppositionGround)Array.IndexOf(weighted, weighted.Max());
                factions.Add(new Faction(Faction.OppositionSlot, FactionRole.Opposition, type, FormOf(type, FactionRole.Opposition, reading),
                    _races[race].Id, SeatOf(seat), trade?.Id, VocationsOf(trade), ground, Math.Clamp(tension, 0, 1)));
            }
        }

        // 4.7 The lesser factions.
        var slots = Math.Min(Math.Max(0, _parameters.MaxLesserFactions),
            (int)Math.Floor(persons / Math.Max(1, _parameters.PersonsPerLesserFaction)));
        var enclave = EnclaveOf(members, races, leadRace);
        var rng = new Sfc32(Hashing.Hash4(_seed, LesserPurpose, region.I, region.J));
        var candidates = new List<(LesserKind Kind, double Fit, double Jittered, int Order)>();
        for (var k = 0; k < Kinds.Count; k++)
        {
            // One number per kind, also for a kind that cannot be taken.
            var u = rng.Next();
            var kind = Kinds[k];
            if (!Exists(kind)) continue;
            var fit = kind.Id == EnclaveKind ? enclave.Fit : kind.Fit(reading);
            candidates.Add((kind, fit, fit + (u - 0.5) * Math.Max(0, _parameters.LesserJitter), k));
        }

        var raceRng = new Sfc32(Hashing.Hash4(_seed, LesserRacePurpose, region.I, region.J));
        var seen = new HashSet<FactionType>();
        var slot = Faction.FirstLesserSlot;
        foreach (var (kind, fit, jittered, _) in candidates.OrderByDescending(c => c.Jittered).ThenBy(c => c.Order))
        {
            if (slot - Faction.FirstLesserSlot >= slots || jittered < _parameters.LesserFit) break;
            // One lesser faction of each type but Vocation.
            if (kind.Type != FactionType.Vocation && seen.Contains(kind.Type)) continue;
            // The ruler is that council or that circle already.
            if (kind.Type == rulingType && kind.Type is FactionType.Place or FactionType.Magic) continue;
            // The guild in opposition is not also a lesser faction.
            if (kind.Id == oppositionKind) continue;
            if (kind.Id == EnclaveKind && enclave.Race < 0) continue;
            seen.Add(kind.Type);

            int race;
            FactionProspect? seat;
            if (kind.Id == EnclaveKind)
            {
                race = enclave.Race;
                var at = Array.IndexOf(races.Races, race);
                double Local(FactionProspect p) => p.LocalPersons[at];
                var held = onShow.Where(p => Array.IndexOf(p.Races, race) >= 0).ToArray();
                seat = Best(held.Where(p => !ReferenceEquals(p, rulingSeat)), Local) ?? Best(held, Local) ?? rulingSeat;
            }
            else
            {
                // The lead race: by size and leaning, with the minority races weighted up.
                var u = raceRng.Next();
                var w = new double[members.Length, 1];
                var sum = 0.0;
                for (var m = 0; m < members.Length; m++)
                {
                    w[m, 0] = Share(members[m]) * Lean(members[m], kind.Type) * (races.IsMinority(members[m]) ? _minorityBoost : 1);
                    sum += w[m, 0];
                }
                race = sum > 0 ? members[Draw(w, sum, u)] : Largest();

                double Pull(FactionProspect p)
                {
                    var local = 0.0;
                    foreach (var patch in p.Around)
                        foreach (var resource in kind.SeatResources)
                            local = Math.Max(local, land[patch].Resources.AmountOf(resource));
                    return p.Prospect.Score * (1 + local) * (kind.OnWater && !OnWater(p.Prospect) ? 0.25 : 1);
                }
                seat = Best(onShow.Where(p => Array.IndexOf(p.Races, race) >= 0), Pull) ?? Best(onShow, Pull);
            }

            factions.Add(new Faction(slot++, FactionRole.Lesser, kind.Type, FormOf(kind.Type, FactionRole.Lesser, reading),
                _races[race].Id, SeatOf(seat), kind.Id, VocationsOf(kind), null, Math.Clamp(fit, 0, 1)));
        }

        Colour(region, factions);
        return new FactionWork { Reading = reading, Fits = fits, Terms = terms, Tension = tension, Factions = factions };
    }

    /// <summary>
    /// 4.9 The colours: every faction gets a main and a secondary colour, in the order of the
    /// slots, mostly from the favourite colours of its lead race. Two numbers are drawn per faction.
    /// </summary>
    private void Colour(GridCoord region, List<Faction> factions)
    {
        var rng = new Sfc32(Hashing.Hash4(_seed, ColourPurpose, region.I, region.J));
        var taken = new bool[_colours.Length];
        var weights = new double[_colours.Length];
        for (var f = 0; f < factions.Count; f++)
        {
            double uMain = rng.Next(), uSecondary = rng.Next();
            if (_colours.Length == 0) continue;
            var race = Array.FindIndex(_races, r => r.Id == factions[f].LeadRace);
            double Weight(int c)
            {
                var place = race < 0 ? 0 : _favourite[race][c];
                return place > 0 ? Math.Max(0, _parameters.FavouriteColourWeight) / place : Math.Max(0, _parameters.OtherColourWeight);
            }

            // The main colour: a colour that a faction in a lower slot has taken is seldom taken again.
            for (var c = 0; c < weights.Length; c++)
                weights[c] = Weight(c) * (taken[c] ? Math.Clamp(_parameters.TakenColourFactor, 0, 1) : 1);
            var main = DrawColour(weights, uMain, c => true);
            taken[main] = true;

            // The secondary colour: another colour, one that stands out against the main colour when there is one.
            for (var c = 0; c < weights.Length; c++) weights[c] = Weight(c);
            bool Other(int c) => c != main;
            bool StandsOut(int c) => Other(c) && Math.Abs(_colours[c].Lightness - _colours[main].Lightness) >= _parameters.ColourContrast;
            var secondary = _colours.Length < 2 ? -1
                : DrawColour(weights, uSecondary, Enumerable.Range(0, _colours.Length).Any(StandsOut) ? StandsOut : Other);

            factions[f] = factions[f] with
            {
                MainColour = _colours[main].Id,
                SecondaryColour = secondary < 0 ? null : _colours[secondary].Id,
            };
        }
    }

    /// <summary>
    /// One of the colours that are allowed, by weight. When none of them has a weight, they
    /// weigh alike. At least one colour must be allowed.
    /// </summary>
    private static int DrawColour(double[] weights, double u, Func<int, bool> allowed)
    {
        var total = 0.0;
        var count = 0;
        for (var c = 0; c < weights.Length; c++)
            if (allowed(c))
            {
                count++;
                if (weights[c] > 0) total += weights[c];
            }
        var even = !(total > 0);
        var target = u * (even ? count : total);
        var sum = 0.0;
        var last = -1;
        for (var c = 0; c < weights.Length; c++)
        {
            if (!allowed(c)) continue;
            var w = even ? 1 : weights[c];
            if (!(w > 0)) continue;
            sum += w;
            last = c;
            if (target < sum) return c;
        }
        return last;
    }

    /// <summary>
    /// 4.7 The race of an enclave and its fit: the race of the region, other than the lead race of
    /// the ruling faction, that leans most to being a people, a minority race before a main race.
    /// </summary>
    private (int Race, double Fit) EnclaveOf(int[] members, RegionRaces races, int rulingRace)
    {
        var race = -1;
        var best = 0.0;
        foreach (var k in members)
        {
            if (k == rulingRace) continue;
            var value = Math.Max(0, _races[k].FactionLeaning.Race) / 3 * (races.IsMinority(k) ? 1 : 0.6);
            if (value > best) (best, race) = (value, k);
        }
        return (race, 0.85 * best);
    }

    /// <summary>A kind exists when it gathers no vocations, or when one of its vocations is on.</summary>
    private bool Exists(LesserKind kind) => kind.Vocations.Length == 0 || kind.Vocations.Any(_vocations.Contains);

    private IReadOnlyList<string> VocationsOf(LesserKind? kind) =>
        kind == null ? [] : kind.Vocations.Where(_vocations.Contains).ToArray();

    /// <summary>4.2 The form: the name of a type in a role.</summary>
    private static FactionForm FormOf(FactionType type, FactionRole role, FactionReading r) => type switch
    {
        FactionType.Family => role == FactionRole.Ruling ? FactionForm.Crown : FactionForm.House,
        FactionType.Race when role == FactionRole.Lesser => FactionForm.Enclave,
        FactionType.Race => r.Herd * r.Open >= Math.Max(0.6 * r.Rough, 0.35) ? FactionForm.Horde
            : r.Rough >= 0.5 && r.Ore >= 0.4 ? FactionForm.Hold
            : FactionForm.Clan,
        FactionType.Vocation => role == FactionRole.Ruling ? FactionForm.MerchantRepublic : FactionForm.Guild,
        FactionType.Belief => role switch
        {
            FactionRole.Ruling => FactionForm.TempleState,
            FactionRole.Opposition when r.Poor > 0.4 => FactionForm.Sect,
            _ => FactionForm.Order,
        },
        FactionType.Magic => role switch
        {
            FactionRole.Ruling => FactionForm.Magocracy,
            FactionRole.Opposition => FactionForm.Cabal,
            _ => FactionForm.Circle,
        },
        FactionType.Place when role == FactionRole.Lesser => FactionForm.CharteredTown,
        FactionType.Place => r.Urban * r.Trade >= 0.35 ? FactionForm.FreeCity : FactionForm.Commune,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
    };

    private static PatchId? SeatOf(FactionProspect? prospect) => prospect?.Prospect.Patch;

    private static bool OnHarbour(Prospect p) => p.Facts.Harbour || p.Facts.Mouth >= NavigableMouth;

    private static bool OnWater(Prospect p) => OnHarbour(p) || p.Facts.River >= NavigableMouth;

    private static double City(FactionReading r) => r.HasCity ? 1 : 0.4;

    /// <summary>The first of the candidates with the highest value; null when there is none.</summary>
    private static FactionProspect? Best(IEnumerable<FactionProspect> candidates, Func<FactionProspect, double> value)
    {
        FactionProspect? best = null;
        var highest = double.NegativeInfinity;
        foreach (var c in candidates)
        {
            var v = value(c);
            if (v > highest) (highest, best) = (v, c);
        }
        return best;
    }

    /// <summary>One cell of the table by weight, walked row by row; the index is row · columns + column.</summary>
    private static int Draw(double[,] weights, double total, double u)
    {
        var target = u * total;
        var sum = 0.0;
        var last = 0;
        var columns = weights.GetLength(1);
        for (var row = 0; row < weights.GetLength(0); row++)
            for (var column = 0; column < columns; column++)
            {
                var w = weights[row, column];
                if (!(w > 0)) continue;
                sum += w;
                last = row * columns + column;
                if (target < sum) return last;
            }
        return last;
    }

    private static double Ramp(double value, IndexRange range) => Ramp(value, range.Low, range.High);

    private static double Ramp(double value, double low, double high) =>
        high > low ? Math.Clamp((value - low) / (high - low), 0, 1) : value >= high ? 1 : 0;

    private static double Max(double a, double b, double c) => Math.Max(a, Math.Max(b, c));

    /// <summary>x to the power y through the deterministic exponential and logarithm; 0 for x at or below 0.</summary>
    private static double Pow(double x, double y) => y == 0 ? 1 : x > 0 ? DetMath.Exp(y * DetMath.Ln(x)) : 0;
}
