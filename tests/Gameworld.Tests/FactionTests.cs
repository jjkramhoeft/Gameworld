using Gameworld.Domain;
using Gameworld.Generation;
using Gameworld.Generation.Factions;
using Gameworld.Generation.Races;

namespace Gameworld.Tests;

/// <summary>The factions of a region: society-generation.md section 4.</summary>
public class FactionTests
{
    private static PerBiome<double> Everywhere(double f) =>
        new(f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f);

    private static RaceParameters Race(string id, double frequency, PerFactionType<double> leaning) =>
        new(id, id, true, Everywhere(frequency)) { FactionLeaning = leaning };

    /// <summary>A leaning to one type only.</summary>
    private static PerFactionType<double> Only(FactionType type) => new(
        type == FactionType.Family ? 3 : 0, type == FactionType.Race ? 3 : 0, type == FactionType.Vocation ? 3 : 0,
        type == FactionType.Belief ? 3 : 0, type == FactionType.Magic ? 3 : 0, type == FactionType.Place ? 3 : 0);

    private static PatchResources Resources(double food, params (Resource Resource, double Amount)[] amounts)
    {
        var list = new ResourceAmount[ResourceCatalog.Count];
        foreach (var (resource, amount) in amounts) list[(int)resource] = new ResourceAmount(amount, null, ResourceOrigin.Potential);
        return new PatchResources(list, food, 0);
    }

    /// <summary>
    /// A Parameter Set with the given races, at 6 persons per km²: a patch cell of 64 km² feeds 384
    /// persons at a food supply of 1. Every race of the region is a main race.
    /// </summary>
    private static ParameterSet With(FactionParameters? factions, params RaceParameters[] races) => ParameterSet.Default with
    {
        Races = races,
        Planning = ParameterSet.Default.Planning with
        {
            Population = new PopulationParameters { PersonsPerKm2 = 6, ProsperityEffect = 0, MinorityRacesPerRegion = 0 },
            Factions = factions ?? new FactionParameters(),
        },
    };

    private static Prospect Town(int i, double score, bool onShow = true, bool city = false, bool harbour = false) => new(
        new PatchId(i, 0), new WorldPoint(i * 50_000.0, 0), city, score, ProspectClass.Average, onShow,
        new ProspectFactors(0.5, 0.5, 0.5, 0.5, 0.5),
        new ProspectFacts(harbour, harbour, 0, 0, 0, false, 0, 3));

    /// <summary>The factions of a region of the given land, with two Prospects on show that hold every race.</summary>
    private static FactionWork Build(ParameterSet parameters, FactionPatch[] land, GridCoord region = default, uint seed = 7)
    {
        var population = new PopulationRules(parameters, seed);
        var races = population.Share(region, land.Select(x => new PopulationPatch(x.Biome, 100, x.AreaKm2, x.Resources, 0.5)).ToArray());
        var all = Enumerable.Range(0, population.Races.Count).ToArray();
        var local = races.Races.Select(k => races.Persons[k]).ToArray();
        FactionProspect[] prospects =
        [
            new(Town(0, 80, city: true), all, [0], local),
            new(Town(2, 78), all, [0], local),
        ];
        return new FactionRules(parameters, seed).Build(region, land, races, prospects);
    }

    private static FactionPatch GrainLand(double areaKm2 = 64, double food = 1) =>
        new(areaKm2, Biome.TemperateBroadleafForest, ElevationType.FlatLand, Resources(food, (Resource.Grain, 0.9)), 0);

    [Fact]
    public void The_default_races_carry_the_leanings_of_the_appendix()
    {
        var races = ParameterSet.Default.Races.ToDictionary(r => r.Id);
        Assert.Equal(new PerFactionType<double>(3, 1, 2, 2, 1, 2), races["midlanders"].FactionLeaning);
        Assert.Equal(new PerFactionType<double>(2, 3, 1, 1, 1, 0), races["grassRiders"].FactionLeaning);
        Assert.Equal(new PerFactionType<double>(1, 3, 3, 1, 1, 1), races["dwarf"].FactionLeaning);
        Assert.Equal(new PerFactionType<double>(0, 1, 2, 2, 1, 3), races["noonfolk"].FactionLeaning);
        foreach (var race in races.Values)
            foreach (var type in Enum.GetValues<FactionType>())
            {
                Assert.InRange(race.FactionLeaning[type], 0, 3);
                // Every race leans to some type more than to the others.
                Assert.Contains(Enum.GetValues<FactionType>(), t => race.FactionLeaning[t] >= 2);
            }

        // A race that says nothing leans to every type alike, and the leaning is read back from a file.
        Assert.Equal(new PerFactionType<double>(1, 1, 1, 1, 1, 1), new RaceParameters("x", "X", true, Everywhere(1)).FactionLeaning);
        var read = ParameterSet.FromJson(ParameterSet.Default.ToJson());
        Assert.Equal(races["twofold"].FactionLeaning, read.Races.Single(r => r.Id == "twofold").FactionLeaning);
    }

    [Fact]
    public void The_default_races_carry_the_appreciated_and_hated_races_of_the_appendix()
    {
        var races = ParameterSet.Default.Races.ToDictionary(r => r.Id);
        Assert.Equal(["halfling", "dwarf", "gnome", "flinchless", "catfolk"], races["midlanders"].AppreciatedRaces);
        Assert.Equal(["northmen", "grassRiders", "elf"], races["midlanders"].HatedRaces);
        Assert.Equal(["northmen"], races["hoarkin"].AppreciatedRaces);
        Assert.Equal(["hearthbloods"], races["hoarkin"].HatedRaces);
        foreach (var race in races.Values)
        {
            // Every default race appreciates and hates some race, each another race of the list, and none in both.
            Assert.NotEmpty(race.AppreciatedRaces);
            Assert.NotEmpty(race.HatedRaces);
            Assert.All(race.AppreciatedRaces.Concat(race.HatedRaces), id => Assert.True(races.ContainsKey(id) && id != race.Id, $"{race.Id}: {id}"));
            Assert.Empty(race.AppreciatedRaces.Intersect(race.HatedRaces));
            Assert.Equal(race.AppreciatedRaces.Count, race.AppreciatedRaces.Distinct().Count());
            Assert.Equal(race.HatedRaces.Count, race.HatedRaces.Distinct().Count());
        }
        _ = new PopulationRules(ParameterSet.Default); // the default list passes the check of the lists

        // A race that says nothing has empty lists, and the lists are read back from a file.
        var plain = new RaceParameters("x", "X", true, Everywhere(1));
        Assert.Empty(plain.AppreciatedRaces);
        Assert.Empty(plain.HatedRaces);
        var read = ParameterSet.FromJson(ParameterSet.Default.ToJson());
        Assert.Equal(races["elf"].AppreciatedRaces, read.Races.Single(r => r.Id == "elf").AppreciatedRaces);
        Assert.Equal(races["elf"].HatedRaces, read.Races.Single(r => r.Id == "elf").HatedRaces);

        // A race that lists itself, an unknown race, or one race in both lists is refused.
        var other = new RaceParameters("y", "Y", true, Everywhere(1));
        Assert.Throws<ArgumentException>(() => new PopulationRules(With(null, plain with { HatedRaces = ["x"] }, other)));
        Assert.Throws<ArgumentException>(() => new PopulationRules(With(null, plain with { AppreciatedRaces = ["z"] }, other)));
        Assert.Throws<ArgumentException>(() => new PopulationRules(With(null, plain with { AppreciatedRaces = ["y"], HatedRaces = ["y"] }, other)));
        _ = new PopulationRules(With(null, plain with { AppreciatedRaces = ["y"] }, other with { HatedRaces = ["x"], Enabled = false }));
    }

    [Fact]
    public void The_kinds_of_lesser_faction_gather_vocations_of_the_default_list()
    {
        var vocations = ParameterSet.Default.Vocations.Select(v => v.Id).ToHashSet();
        var kinds = FactionRules.Kinds;
        Assert.Equal(20, kinds.Count);
        Assert.Equal(kinds.Count, kinds.Select(k => k.Id).Distinct().Count());
        Assert.Equal(15, kinds.Count(k => k.Type == FactionType.Vocation));
        // One kind of each other type.
        foreach (var type in Enum.GetValues<FactionType>().Where(t => t != FactionType.Vocation))
            Assert.Single(kinds, k => k.Type == type);
        Assert.All(kinds, k => Assert.All(k.Vocations, v => Assert.Contains(v, vocations)));
        Assert.All(kinds.Where(k => k.Type == FactionType.Vocation), k => Assert.NotEmpty(k.Vocations));
        Assert.Equal("Merchants' guild", FactionRules.KindName("merchants"));
        Assert.Equal("unknown", FactionRules.KindName("unknown"));
    }

    [Fact]
    public void A_region_without_persons_has_no_faction()
    {
        var parameters = With(null, Race("a", 1, Only(FactionType.Family)));
        var work = Build(parameters, [GrainLand(food: 0)]);
        Assert.Empty(work.Factions);
        Assert.Null(work.Reading);

        // No land at all.
        Assert.Empty(Build(parameters, []).Factions);
    }

    [Fact]
    public void Every_region_with_persons_has_exactly_one_ruling_faction_of_a_type_its_race_leans_to()
    {
        foreach (var type in Enum.GetValues<FactionType>())
        {
            var parameters = With(null, Race("a", 1, Only(type)));
            // Supernatural grain land: every type has a fit above 0, the mages too.
            var land = GrainLand() with { Supernatural = 100 };
            for (var i = 0; i < 20; i++)
            {
                var work = Build(parameters, [land, land], new GridCoord(i, 3));
                var ruling = Assert.Single(work.Factions, f => f.Role == FactionRole.Ruling);
                Assert.Same(work.Factions[0], ruling);
                Assert.Equal(Faction.RulingSlot, ruling.Slot);
                Assert.Equal(type, ruling.Type);
                Assert.Equal("a", ruling.LeadRace);
                // The best Prospect on show that holds the lead race.
                Assert.Equal(new PatchId(0, 0), ruling.Seat);
                Assert.Null(ruling.Ground);
                Assert.Null(ruling.Kind);
            }
        }
    }

    [Fact]
    public void The_larger_race_leads_more_often_and_the_race_power_decides_how_often()
    {
        // Two races that lean alike, one with twice the persons of the other.
        ParameterSet Parameters(double power) => With(
            new FactionParameters { RacePower = power },
            Race("large", 2, Only(FactionType.Family)), Race("small", 1, Only(FactionType.Family)));
        int Led(ParameterSet parameters) => Enumerable.Range(0, 400)
            .Count(i => Build(parameters, [GrainLand()], new GridCoord(i, 0)).Factions[0].LeadRace == "large");

        var byShare = Led(Parameters(1));     // 2 : 1
        var byDefault = Led(Parameters(5));   // 32 : 1
        Assert.InRange(byShare, 230, 300);
        Assert.InRange(byDefault, 375, 399);
        // With no power the size does not count.
        Assert.InRange(Led(Parameters(0)), 160, 240);
    }

    [Fact]
    public void The_land_moves_the_fits()
    {
        var rules = new FactionRules(With(null, Race("a", 1, Only(FactionType.Family))), 1);
        // One patch of the given kind, as large as it takes to feed the persons at 6 per km².
        FactionReading Reading(FactionPatch patch, int persons)
        {
            var population = new PopulationRules(With(null, Race("a", 1, Only(FactionType.Family))), 1);
            FactionPatch[] land = [patch with { AreaKm2 = persons / 6.0 }];
            var races = population.Share(default, land.Select(x => new PopulationPatch(x.Biome, 100, x.AreaKm2, x.Resources, 0.5)).ToArray());
            Assert.Equal(persons, races.Persons.Sum(), 6);
            return rules.Read(land, races, []);
        }

        var grain = rules.Fits(Reading(GrainLand(), 700));
        var steppe = rules.Fits(Reading(
            new(64, Biome.ColdSteppe, ElevationType.FlatLand, Resources(1, (Resource.Livestock, 0.8), (Resource.HorsesAndPackAnimals, 0.6)), 0), 300));
        var peaks = rules.Fits(Reading(new(64, Biome.BorealForest, ElevationType.Mountains, Resources(1), 0), 300));
        var haunted = rules.Fits(Reading(new(64, Biome.TemperateMixedForest, ElevationType.Lowland, Resources(1), 100), 300));
        var tiny = rules.Fits(Reading(GrainLand(), 40));

        // Grain land with many persons suits a house best, and better than any other land does.
        Assert.Equal(1, grain[(int)FactionType.Family], 6);
        Assert.True(grain[(int)FactionType.Family] > steppe[(int)FactionType.Family]);
        Assert.True(grain[(int)FactionType.Family] > peaks[(int)FactionType.Family]);
        // Open herding land and rough land suit a people.
        Assert.Equal(1, steppe[(int)FactionType.Race], 6);
        Assert.True(peaks[(int)FactionType.Race] > grain[(int)FactionType.Race]);
        // Only supernatural land suits a faith and the mages.
        Assert.True(haunted[(int)FactionType.Belief] > 2 * grain[(int)FactionType.Belief]);
        Assert.Equal(0.8, haunted[(int)FactionType.Magic], 6);
        Assert.Equal(0, grain[(int)FactionType.Magic], 6);
        // A very small region suits a people and a place more than a house.
        Assert.True(tiny[(int)FactionType.Race] > tiny[(int)FactionType.Family]);
        Assert.True(tiny[(int)FactionType.Place] > grain[(int)FactionType.Place]);
        // Every fit lies between the base of its type and 1.
        var b = new FactionParameters().BaseFit;
        foreach (var fits in new[] { grain, steppe, peaks, haunted, tiny })
            foreach (var type in Enum.GetValues<FactionType>())
                Assert.InRange(fits[(int)type], b[type] - 1e-9, 1 + 1e-9);
    }

    [Fact]
    public void The_tension_decides_whether_a_region_has_an_opposition()
    {
        RaceParameters[] races =
        [
            Race("a", 3, new(3, 2, 2, 2, 1, 2)), Race("b", 2, new(2, 3, 2, 1, 1, 2)), Race("c", 1, new(1, 1, 3, 2, 1, 3)),
        ];
        FactionPatch[] land = [GrainLand(), new(64, Biome.BorealForest, ElevationType.Mountains, Resources(0.6, (Resource.Gold, 0.5)), 0)];

        var always = With(new FactionParameters { OppositionTension = 0, TensionJitter = 0 }, races);
        var never = With(new FactionParameters { OppositionTension = 5 }, races);
        var crowded = With(new FactionParameters { OppositionTension = 0, TensionJitter = 0, OppositionMinPersons = 100_000 }, races);
        for (var i = 0; i < 60; i++)
        {
            var region = new GridCoord(i, -4);
            var work = Build(always, land, region);
            var ruling = work.Factions[0];
            var opposition = work.Factions[1];
            Assert.Equal(FactionRole.Opposition, opposition.Role);
            Assert.Equal(Faction.OppositionSlot, opposition.Slot);
            Assert.NotNull(opposition.Ground);
            Assert.Contains(opposition.LeadRace, new[] { "a", "b", "c" });
            // A people does not oppose a ruler of its own people as a people.
            if (opposition.Type == FactionType.Race) Assert.NotEqual(ruling.LeadRace, opposition.LeadRace);
            // No mages against the ruler where the land is not supernatural.
            Assert.NotEqual(FactionType.Magic, opposition.Type);
            // It sits on another Prospect than the ruler when there is one.
            Assert.Equal(new PatchId(2, 0), opposition.Seat);
            // A guild in opposition has a trade, and that trade is not also a lesser faction.
            Assert.Equal(opposition.Type == FactionType.Vocation, opposition.Kind != null);
            if (opposition.Kind != null) Assert.DoesNotContain(work.Factions, f => f.Role == FactionRole.Lesser && f.Kind == opposition.Kind);
            // The grip of the ruler and of the opposition are the two sides of the tension.
            Assert.Equal(1, ruling.Grip + opposition.Grip, 6);
            Assert.Equal(work.Tension, opposition.Grip, 6);

            Assert.DoesNotContain(Build(never, land, region).Factions, f => f.Role == FactionRole.Opposition);
            Assert.DoesNotContain(Build(crowded, land, region).Factions, f => f.Role == FactionRole.Opposition);
        }
    }

    [Fact]
    public void Lesser_factions_follow_the_persons_and_the_fit()
    {
        RaceParameters[] races = [Race("a", 3, new(3, 1, 2, 2, 1, 2)), Race("b", 1, new(1, 3, 3, 1, 1, 1))];
        // Three patch cells of full land feed 1,152 persons.
        FactionPatch[] land = [GrainLand(), GrainLand(), new(64, Biome.TemperateGrassland, ElevationType.FlatLand, Resources(1, (Resource.Salt, 0.5)), 0)];

        var open = With(new FactionParameters { LesserFit = 0, LesserJitter = 0, OppositionTension = 5 }, races);
        var two = With(new FactionParameters { LesserFit = 0, LesserJitter = 0, OppositionTension = 5, MaxLesserFactions = 2 }, races);
        var none = With(new FactionParameters { LesserFit = 0, MaxLesserFactions = 0, OppositionTension = 5 }, races);
        var sparse = With(new FactionParameters { LesserFit = 0, PersonsPerLesserFaction = 400, OppositionTension = 5 }, races);
        var strict = With(new FactionParameters { LesserFit = 1.5 }, races);
        for (var i = 0; i < 30; i++)
        {
            var region = new GridCoord(i, 9);
            var work = Build(open, land, region);
            var lesser = work.Factions.Where(f => f.Role == FactionRole.Lesser).ToArray();
            Assert.Equal(4, lesser.Length);
            Assert.Equal([2, 3, 4, 5], lesser.Select(f => f.Slot));
            Assert.All(lesser, f => Assert.Contains(f.Kind, FactionRules.Kinds.Select(k => k.Id)));
            Assert.Equal(lesser.Length, lesser.Select(f => f.Kind).Distinct().Count());
            // The best fits first.
            Assert.Equal(lesser.Select(f => f.Grip).OrderByDescending(g => g), lesser.Select(f => f.Grip));
            // One lesser faction of each type but Vocation.
            foreach (var type in Enum.GetValues<FactionType>().Where(t => t != FactionType.Vocation))
                Assert.True(lesser.Count(f => f.Type == type) <= 1);
            // The ruler is that council or that circle already.
            var ruling = work.Factions[0];
            if (ruling.Type is FactionType.Place or FactionType.Magic) Assert.DoesNotContain(lesser, f => f.Type == ruling.Type);
            // An enclave is a people other than the ruler's.
            Assert.All(lesser.Where(f => f.Kind == FactionRules.EnclaveKind), f =>
            {
                Assert.Equal(FactionForm.Enclave, f.Form);
                Assert.NotEqual(ruling.LeadRace, f.LeadRace);
            });
            Assert.All(lesser, f => Assert.Null(f.Ground));

            Assert.Equal(2, Build(two, land, region).Factions.Count(f => f.Role == FactionRole.Lesser));
            Assert.Single(Build(none, land, region).Factions);
            // 1,152 persons at 400 persons for each: two.
            Assert.Equal(2, Build(sparse, land, region).Factions.Count(f => f.Role == FactionRole.Lesser));
            Assert.DoesNotContain(Build(strict, land, region).Factions, f => f.Role == FactionRole.Lesser);
        }
    }

    [Fact]
    public void A_kind_whose_vocations_are_all_off_is_not_taken()
    {
        var races = new[] { Race("a", 1, new(3, 1, 2, 2, 1, 2)) };
        var open = With(new FactionParameters { LesserFit = 0, LesserJitter = 0, MaxLesserFactions = 20, PersonsPerLesserFaction = 1, OppositionTension = 5 }, races);
        var kinds = Build(open, [GrainLand()]).Factions.Where(f => f.Role == FactionRole.Lesser).Select(f => f.Kind).ToArray();
        Assert.Contains("merchants", kinds);
        Assert.Contains("order", kinds);

        string[] off = ["merchant", "shopkeeper", "carter"];
        var without = open with { Vocations = open.Vocations.Select(v => v with { Enabled = !off.Contains(v.Id) }).ToArray() };
        var left = Build(without, [GrainLand()]).Factions.Where(f => f.Role == FactionRole.Lesser).ToArray();
        Assert.DoesNotContain(left, f => f.Kind == "merchants");
        // The caravan company loses two of its three vocations and keeps the third.
        Assert.Equal(["innkeeper"], left.Single(f => f.Kind == "caravaneers").Vocations);
    }

    [Fact]
    public void The_factions_of_a_region_are_a_pure_function_of_the_region_and_the_seed()
    {
        RaceParameters[] races = [Race("a", 3, new(3, 2, 2, 2, 1, 2)), Race("b", 2, new(2, 3, 2, 1, 1, 2))];
        // Without sharpness the leanings alone decide the type, so the rulers vary.
        var parameters = With(new FactionParameters { Sharpness = 0 }, races);
        FactionPatch[] land = [GrainLand(), GrainLand()];
        var first = Build(parameters, land, new GridCoord(3, 4));
        Assert.Equal(DomainJson.Serialize(first.Factions), DomainJson.Serialize(Build(parameters, land, new GridCoord(3, 4)).Factions));

        // Another region or another seed draws again.
        string Ruling(GridCoord region, uint seed) =>
            string.Join(",", Enumerable.Range(0, 40).Select(i => Build(parameters, land, new GridCoord(region.I + i, region.J), seed).Factions[0].Type));
        Assert.NotEqual(Ruling(new GridCoord(0, 0), 7), Ruling(new GridCoord(0, 1), 7));
        Assert.NotEqual(Ruling(new GridCoord(0, 0), 7), Ruling(new GridCoord(0, 0), 8));
    }

    [Fact]
    public void A_planned_region_holds_its_factions()
    {
        var kinds = FactionRules.Kinds.ToDictionary(k => k.Id);
        var types = new HashSet<FactionType>();
        var oppositions = 0;
        var regions = 0;
        foreach (var seed in new[] { "cellgen", "atlas" })
        {
            var world = World.Create(seed);
            foreach (var id in RegionStageRules.RegionsWithin(new RegionId(0, 0), 1))
            {
                var plan = world.Advance(id, RegionStage.Planned).Plan!;
                var factions = plan.Factions;
                Assert.Same(world.PlanOf(id).Factions.Factions, factions);
                if (plan.Population.Total == 0)
                {
                    Assert.Empty(factions);
                    continue;
                }
                regions++;

                // One ruling faction, at most one opposition, at most four lesser factions, by slot.
                Assert.Same(factions[0], plan.Ruling);
                Assert.Equal(Faction.RulingSlot, plan.Ruling!.Slot);
                Assert.Single(factions, f => f.Role == FactionRole.Ruling);
                Assert.True(factions.Count(f => f.Role == FactionRole.Opposition) <= 1);
                if (plan.Opposition is { } opposition)
                {
                    oppositions++;
                    Assert.Equal(Faction.OppositionSlot, opposition.Slot);
                    Assert.Same(factions[1], opposition);
                    Assert.NotNull(opposition.Ground);
                    Assert.True(plan.Population.Total >= 77); // 80 persons before each race is rounded
                }
                Assert.InRange(plan.Lesser.Count(), 0, 4);
                Assert.True(plan.Lesser.Count() <= (plan.Population.Total + 6) / 150);
                Assert.Equal(Enumerable.Range(Faction.FirstLesserSlot, plan.Lesser.Count()), plan.Lesser.Select(f => f.Slot));
                Assert.Equal(factions.Select(f => f.Slot).Order(), factions.Select(f => f.Slot));

                var ofRegion = plan.Population.RegionRaces.ToHashSet();
                var onShow = plan.Prospects.Where(p => p.OnShow).ToArray();
                foreach (var faction in factions)
                {
                    types.Add(faction.Type);
                    Assert.Contains(faction.LeadRace, ofRegion);
                    Assert.InRange(faction.Grip, 0, 1);
                    Assert.Equal(new SlotId<RegionId>(id, faction.Slot), faction.IdIn(id));

                    // The seat is a Prospect on show of the region.
                    if (onShow.Length == 0) Assert.Null(faction.Seat);
                    else Assert.Contains(onShow, p => p.Patch == faction.Seat);

                    if (faction.Role == FactionRole.Lesser)
                    {
                        var kind = kinds[faction.Kind!];
                        Assert.Equal(kind.Type, faction.Type);
                        Assert.Equal(kind.Vocations, faction.Vocations.ToArray());
                        Assert.True(faction.Grip >= 0.75 - 0.05 - 1e-9);
                    }
                    else if (faction.Kind is { } trade)
                    {
                        Assert.Equal(FactionForm.Guild, faction.Form);
                        Assert.Equal(FactionType.Vocation, kinds[trade].Type);
                    }
                }

                // The ruler sits on the best Prospect on show that holds its lead race.
                var holding = Enumerable.Range(0, plan.Prospects.Count)
                    .Where(n => plan.Prospects[n].OnShow && plan.ProspectRaces[n].Contains(plan.Ruling.LeadRace))
                    .Select(n => plan.Prospects[n]).ToArray();
                if (holding.Length > 0) Assert.Equal(holding.MaxBy(p => p.Score)!.Patch, plan.Ruling.Seat);
            }
        }
        Assert.Equal(18, regions);
        // Eighteen regions hold several types of ruler and lesser faction, and some oppositions.
        Assert.True(types.Count >= 4);
        Assert.InRange(oppositions, 2, 16);
    }
}
