using Gameworld.Domain;
using Gameworld.Generation;
using Gameworld.Generation.Races;

namespace Gameworld.Tests;

/// <summary>The races of the Parameter Set and the potential population of map-generation.md section 19.</summary>
public class PopulationTests
{
    private static readonly string[] ClassicSix = ["human", "elf", "dwarf", "halfling", "orc", "goblin"];

    private static PerBiome<double> Everywhere(double f) =>
        new(f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f);

    private static PatchResources Resources(double food, double fish = 0)
    {
        var amounts = new ResourceAmount[ResourceCatalog.Count];
        amounts[(int)Resource.Fish] = new ResourceAmount(fish, null, ResourceOrigin.Potential);
        return new PatchResources(amounts, food, 0);
    }

    private static ParameterSet With(params RaceParameters[] races) => ParameterSet.Default with { Races = races };

    private static ParameterSet AllRacesOn() =>
        ParameterSet.Default with { Races = ParameterSet.Default.Races.Select(r => r with { Enabled = true }).ToArray() };

    [Fact]
    public void The_default_list_holds_every_race_once_and_the_classic_six_are_on()
    {
        var races = ParameterSet.Default.Races;
        Assert.Equal(23, races.Count);
        Assert.Equal(races.Count, races.Select(r => r.Id).Distinct().Count());
        Assert.Equal(ClassicSix, races.Where(r => r.Enabled).Select(r => r.Id));
        Assert.All(races, r => Assert.False(string.IsNullOrWhiteSpace(r.Name)));

        // Every race has a frequency for every biome, and lives in at least one of them.
        foreach (var race in races)
        {
            var frequencies = Enum.GetValues<Biome>().Select(b => race.BiomeFrequency[b]).ToArray();
            Assert.All(frequencies, f => Assert.True(f >= 0));
            Assert.Contains(frequencies, f => f > 0);
        }

        var human = races.Single(r => r.Id == "human");
        Assert.Equal(0, human.BiomeFrequency[Biome.IceAndSnow]);
        Assert.Equal(30, human.BiomeFrequency[Biome.TemperateGrassland]);
        Assert.Equal(10, human.BiomeFrequency[Biome.Mangrove]);
    }

    [Fact]
    public void A_race_is_added_and_toggled_in_the_parameter_set_json()
    {
        var p = ParameterSet.FromJson(
            """
            {
              "Races": [
                { "Id": "human", "Name": "Human", "Enabled": true, "BiomeFrequency": { "Savanna": 3, "Tundra": 1 } },
                { "Id": "moonfolk", "Name": "Moon folk", "Enabled": false, "BiomeFrequency": { "Savanna": 1 } }
              ]
            }
            """);
        Assert.Equal(["human", "moonfolk"], p.Races.Select(r => r.Id));
        Assert.False(p.Races[1].Enabled);
        Assert.Equal(3, p.Races[0].BiomeFrequency[Biome.Savanna]);
        Assert.Equal(0, p.Races[0].BiomeFrequency[Biome.HotDesert]); // a biome that is left out is 0

        var rules = new PopulationRules(p);
        Assert.Equal(["human"], rules.Races.Select(r => r.Id));

        var toggled = p with { Races = p.Races.Select(r => r with { Enabled = true }).ToArray() };
        Assert.Equal(toggled.ToJson(), ParameterSet.FromJson(toggled.ToJson()).ToJson());
        Assert.Equal(["human", "moonfolk"], new PopulationRules(toggled).Races.Select(r => r.Id));
    }

    [Fact]
    public void The_json_of_a_race_lists_only_the_biomes_it_lives_in()
    {
        var json = System.Text.Json.Nodes.JsonNode.Parse(ParameterSet.Default.ToJson())!;
        var races = json["Races"]!.AsArray();
        Assert.Equal(23, races.Count);
        foreach (var race in races)
        {
            var frequencies = race!["BiomeFrequency"]!.AsObject();
            Assert.NotEmpty(frequencies);
            Assert.All(frequencies, f => Assert.True(f.Value!.GetValue<double>() > 0));
        }

        var elf = races.Single(r => r!["Id"]!.GetValue<string>() == "elf")!["BiomeFrequency"]!.AsObject();
        Assert.Equal(
            ["BorealForest", "TemperateGrassland", "WoodlandAndShrubland", "TemperateMixedForest",
             "TemperateBroadleafForest", "TemperateRainForest", "TropicalDryForest", "TropicalRainForest"],
            elf.Select(f => f.Key));

        // The biomes that are left out read back as 0, so the list survives the round trip.
        var back = ParameterSet.FromJson(ParameterSet.Default.ToJson());
        Assert.Equal(ParameterSet.Default.Races, back.Races);
        Assert.Equal(0, back.Races.Single(r => r.Id == "elf").BiomeFrequency[Biome.Tundra]);

        // The schema does not ask for the biomes that are left out.
        var schema = ParameterSetSchema.Build()["properties"]!["Races"]!["items"]!["properties"]!["BiomeFrequency"]!;
        Assert.Null(schema["required"]);
        Assert.Equal(16, schema["properties"]!.AsObject().Count);
    }

    [Fact]
    public void The_races_that_are_on_share_a_patch_by_their_frequency_in_its_biome()
    {
        var rules = new PopulationRules(ParameterSet.Default);
        Assert.Equal(ClassicSix, rules.Races.Select(r => r.Id));

        foreach (var biome in Enum.GetValues<Biome>())
        {
            var shares = rules.Shares(biome).ToArray();
            Assert.Equal(6, shares.Length);
            Assert.Equal(1, shares.Sum(), 12);
            var sum = rules.Races.Sum(r => r.BiomeFrequency[biome]);
            for (var k = 0; k < shares.Length; k++)
                Assert.Equal(rules.Races[k].BiomeFrequency[biome] / sum, shares[k], 12);
        }

        // Temperate broadleaf forest: human 30, elf 6, dwarf 2, halfling 4, orc 2, goblin 4.
        var forest = rules.Shares(Biome.TemperateBroadleafForest);
        Assert.Equal(30.0 / 48, forest[0], 12);
        Assert.Equal(6.0 / 48, forest[1], 12);
        // Ice & snow: of the six only dwarves live there.
        Assert.Equal([0, 0, 1, 0, 0, 0], rules.Shares(Biome.IceAndSnow).ToArray());
    }

    [Fact]
    public void A_race_that_is_off_gets_nothing_and_the_others_share_the_land_without_it()
    {
        var human = new RaceParameters("human", "Human", true, Everywhere(3));
        var elf = new RaceParameters("elf", "Elf", true, Everywhere(1));
        (Biome, double, PatchResources, double)[] land = [(Biome.Savanna, 64, Resources(0.5), 0.5)];

        var both = new PopulationRules(With(human, elf)).Region(land);
        Assert.Equal(1920, both.Total); // 64 km² × 60 × 0.5
        Assert.Equal(1440, both.Of("human"));
        Assert.Equal(480, both.Of("elf"));

        var humansOnly = new PopulationRules(With(human, elf with { Enabled = false })).Region(land);
        Assert.Equal(["human"], humansOnly.Races.Select(r => r.Race));
        Assert.Equal(1920, humansOnly.Of("human"));
        Assert.Equal(0, humansOnly.Of("elf"));
        Assert.Equal(both.Total, humansOnly.Total);
    }

    [Fact]
    public void Nobody_lives_in_a_biome_that_no_race_that_is_on_lives_in()
    {
        var frost = new RaceParameters("frostFolk", "Frost folk", true, Everywhere(0) with { IceAndSnow = 3, Tundra = -5 });
        var rules = new PopulationRules(With(frost));
        Assert.Equal([1.0], rules.Shares(Biome.IceAndSnow).ToArray());
        Assert.True(rules.Shares(Biome.Savanna).IsEmpty);
        Assert.True(rules.Shares(Biome.Tundra).IsEmpty); // a negative frequency counts as 0

        var population = rules.Region(
        [
            (Biome.Savanna, 64, Resources(1), 0.5),
            (Biome.Tundra, 64, Resources(1), 0.5),
            (Biome.IceAndSnow, 64, Resources(0.25), 0.5),
        ]);
        Assert.Equal(960, population.Of("frostFolk"));
        Assert.Equal(960, population.Total);

        // With no race on, the region has no potential population at all.
        var nobody = new PopulationRules(With(frost with { Enabled = false })).Region([(Biome.IceAndSnow, 64, Resources(1), 0.5)]);
        Assert.Empty(nobody.Races);
        Assert.Equal(0, nobody.Total);
    }

    [Fact]
    public void The_capacity_of_a_patch_follows_its_area_food_and_prosperity()
    {
        var rules = new PopulationRules(ParameterSet.Default);
        Assert.Equal(64 * 60 * 0.5, rules.Capacity(64, Resources(0.5), 0.5), 9);
        Assert.Equal(2 * rules.Capacity(64, Resources(0.5), 0.5), rules.Capacity(128, Resources(0.5), 0.5), 9);
        Assert.Equal(0, rules.Capacity(64, Resources(0), 0.5));

        // Prosperity: from half at the lowest to one and a half at the highest.
        Assert.Equal(64 * 60 * 0.5 * 0.5, rules.Capacity(64, Resources(0.5), 0), 9);
        Assert.Equal(64 * 60 * 0.5 * 1.5, rules.Capacity(64, Resources(0.5), 1), 9);

        // Fish adds a fifth of its amount to the food supply, which never passes 1.
        Assert.Equal(0.5, PopulationRules.FoodSupply(Resources(0.5)), 12);
        Assert.Equal(0.6, PopulationRules.FoodSupply(Resources(0.5, fish: 0.5)), 12);
        Assert.Equal(1, PopulationRules.FoodSupply(Resources(0.95, fish: 1)), 12);

        var p = ParameterSet.Default;
        var flat = new PopulationRules(p with
        {
            Planning = p.Planning with { Population = new PopulationParameters { PersonsPerKm2 = 10, ProsperityEffect = 0 } },
        });
        Assert.Equal(640, flat.Capacity(64, Resources(1), 0), 9);
        Assert.Equal(640, flat.Capacity(64, Resources(1), 1), 9);
    }

    [Fact]
    public void A_race_id_must_be_given_and_unique()
    {
        var human = new RaceParameters("human", "Human", true, Everywhere(1));
        Assert.Throws<ArgumentException>(() => new PopulationRules(With(human, human with { Name = "Other", Enabled = false })));
        Assert.Throws<ArgumentException>(() => new PopulationRules(With(human with { Id = " " })));
        Assert.Throws<ArgumentException>(() => World.Create("cellgen", With(human, human)));
    }

    [Fact]
    public void A_planned_region_holds_the_potential_population_of_its_land_per_race()
    {
        var world = World.Create("cellgen");
        var rules = world.PopulationRules;
        var patchKm = world.Cells.PatchSize / 1000;
        long people = 0;
        foreach (var id in RegionStageRules.RegionsWithin(new RegionId(0, 0), 1))
        {
            var population = world.Advance(id, RegionStage.Planned).Plan!.Population;
            Assert.Equal(ClassicSix, population.Races.Select(r => r.Race));
            Assert.All(population.Races, r => Assert.True(r.Persons >= 0));
            Assert.Same(population, world.PlanOf(id).Population);

            // The same sum from the Detailed data: the estimated biomes and resources of the land patches.
            var expected = new double[6];
            foreach (var patch in world.DetailOf(id).Patches.Where(x => !x.Shape.Ocean))
            {
                var capacity = rules.Capacity(patch.Shape.Cells.Length * patchKm * patchKm, patch.Resources, patch.Prosperity);
                var shares = rules.Shares(patch.Biome!.Biome);
                for (var k = 0; k < shares.Length; k++) expected[k] += capacity * shares[k];
            }
            for (var k = 0; k < 6; k++)
                Assert.Equal((long)Math.Floor(expected[k] + 0.5), population.Races[k].Persons);
            people += population.Total;
        }
        // Nine regions of the default world: a few hundred thousand persons each.
        Assert.InRange(people / 9, 100_000, 1_000_000);
    }

    [Fact]
    public void The_potential_population_of_a_region_does_not_depend_on_how_many_races_are_on()
    {
        var region = new RegionId(0, 0);
        var six = World.Create("cellgen").Advance(region, RegionStage.Planned).Plan!;
        var all = World.Create("cellgen", AllRacesOn()).Advance(region, RegionStage.Planned).Plan!;

        Assert.Equal(23, all.Population.Races.Count);
        // The six live in every biome, so the races only share the same land differently. Each race is rounded on its own.
        Assert.InRange(all.Population.Total - six.Population.Total, -23, 23);
        Assert.True(all.Population.Of("human") < six.Population.Of("human"));
        // The races do not move the Prospects.
        Assert.Equal(DomainJson.Serialize(six.Prospects), DomainJson.Serialize(all.Prospects));
    }
}
