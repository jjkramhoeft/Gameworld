using System.Text.Json.Nodes;
using Gameworld.Domain;
using Gameworld.Generation;
using Gameworld.Generation.Races;

namespace Gameworld.Tests;

/// <summary>The races of the Parameter Set and the potential population of society-generation.md sections 2 and 3.</summary>
public class PopulationTests
{
    /// <summary>The ids of the default list, in its order: seven human races, five classic races and sixteen new ones.</summary>
    private static readonly string[] DefaultRaces =
    [
        "northmen", "midlanders", "saltroadFolk", "sunlanders", "grassRiders", "rainkeepers", "canalFolk",
        "elf", "dwarf", "halfling", "gnome", "orc",
        "hoarkin", "hearthbloods", "widegaze", "amberbloods", "brinefolk", "duskreaders", "noonfolk", "boughwalkers",
        "shoalfolk", "swiftborn", "flinchless", "lanternkin", "stillfaces", "twofold", "bitterbloods", "catfolk",
    ];

    /// <summary>The seven human races of the default list.</summary>
    private static readonly string[] HumanRaces = DefaultRaces[..7];

    private static PerBiome<double> Everywhere(double f) =>
        new(f, f, f, f, f, f, f, f, f, f, f, f, f, f, f, f);

    private static PatchResources Resources(double food, double fish = 0)
    {
        var amounts = new ResourceAmount[ResourceCatalog.Count];
        amounts[(int)Resource.Fish] = new ResourceAmount(fish, null, ResourceOrigin.Potential);
        return new PatchResources(amounts, food, 0);
    }

    private static ParameterSet With(params RaceParameters[] races) => ParameterSet.Default with { Races = races };

    private static ParameterSet OnlyOn(params string[] ids) =>
        ParameterSet.Default with { Races = ParameterSet.Default.Races.Select(r => r with { Enabled = ids.Contains(r.Id) }).ToArray() };

    private static RaceParameters Race(string id, IReadOnlyList<HeightFactorPoint>? heightModifier = null) =>
        new(id, id, true, Everywhere(1)) { HeightModifier = heightModifier };

    [Fact]
    public void The_default_list_holds_the_28_races_once_and_all_are_on()
    {
        var races = ParameterSet.Default.Races;
        Assert.Equal(28, races.Count);
        Assert.Equal(DefaultRaces, races.Select(r => r.Id));
        Assert.Equal(races.Count, races.Select(r => r.Id).Distinct().Count());
        Assert.All(races, r => Assert.True(r.Enabled));
        Assert.DoesNotContain(races, r => r.Id == "human"); // the human peoples are separate races
        Assert.All(races, r => Assert.False(string.IsNullOrWhiteSpace(r.Name)));

        // Every race has a frequency for every biome, and lives in at least one of them.
        foreach (var race in races)
        {
            var frequencies = Enum.GetValues<Biome>().Select(b => race.BiomeFrequency[b]).ToArray();
            Assert.All(frequencies, f => Assert.True(f >= 0));
            Assert.Contains(frequencies, f => f > 0);
        }

        // A frequency is the habitat weight times how common the race is: Midlanders 3 · 5 in Temperate grassland.
        RaceParameters Of(string id) => races.Single(r => r.Id == id);
        Assert.Equal(0, Of("midlanders").BiomeFrequency[Biome.IceAndSnow]);
        Assert.Equal(15, Of("midlanders").BiomeFrequency[Biome.TemperateGrassland]);
        Assert.Equal(5, Of("midlanders").BiomeFrequency[Biome.BorealForest]);
        Assert.Equal(1.5, Of("bitterbloods").BiomeFrequency[Biome.Mangrove]);

        // Five races have a height modifier; the height does not matter to the others.
        Assert.Equal(RaceParameters.OnlyFrom(200), Of("dwarf").HeightModifier);
        Assert.Equal(RaceParameters.OnlyBelow(200), Of("elf").HeightModifier);
        Assert.Equal([new HeightFactorPoint(100, 0.5), new HeightFactorPoint(400, 1)], Of("hearthbloods").HeightModifier);
        Assert.Equal([new HeightFactorPoint(300, 1), new HeightFactorPoint(500, 0)], Of("widegaze").HeightModifier);
        Assert.Equal([new HeightFactorPoint(20, 1), new HeightFactorPoint(80, 0)], Of("shoalfolk").HeightModifier);
        Assert.All(
            races.Where(r => r.Id is not ("dwarf" or "elf" or "hearthbloods" or "widegaze" or "shoalfolk")),
            r => Assert.Null(r.HeightModifier));
    }

    [Fact]
    public void A_race_is_added_and_toggled_in_the_parameter_set_json()
    {
        var p = ParameterSet.FromJson(
            """
            {
              "Races": [
                { "Id": "midlanders", "Name": "Midlanders", "Enabled": true, "BiomeFrequency": { "Savanna": 3, "Tundra": 1 } },
                { "Id": "moonfolk", "Name": "Moon folk", "Enabled": false, "BiomeFrequency": { "Savanna": 1 },
                  "HeightModifier": [ { "HeightM": 300, "Factor": 0 }, { "HeightM": 500, "Factor": 1 } ] }
              ]
            }
            """);
        Assert.Equal(["midlanders", "moonfolk"], p.Races.Select(r => r.Id));
        Assert.False(p.Races[1].Enabled);
        Assert.Equal(3, p.Races[0].BiomeFrequency[Biome.Savanna]);
        Assert.Equal(0, p.Races[0].BiomeFrequency[Biome.HotDesert]); // a biome that is left out is 0
        Assert.Null(p.Races[0].HeightModifier);
        Assert.Equal([new HeightFactorPoint(300, 0), new HeightFactorPoint(500, 1)], p.Races[1].HeightModifier);

        var rules = new PopulationRules(p);
        Assert.Equal(["midlanders"], rules.Races.Select(r => r.Id));

        var toggled = p with { Races = p.Races.Select(r => r with { Enabled = true }).ToArray() };
        Assert.Equal(toggled.ToJson(), ParameterSet.FromJson(toggled.ToJson()).ToJson());
        var both = new PopulationRules(toggled);
        Assert.Equal(["midlanders", "moonfolk"], both.Races.Select(r => r.Id));
        Assert.Equal(0.5, both.HeightFactor(1, 400), 12);
    }

    [Fact]
    public void The_json_of_a_race_lists_only_the_biomes_it_lives_in()
    {
        var json = JsonNode.Parse(ParameterSet.Default.ToJson())!;
        var races = json["Races"]!.AsArray();
        Assert.Equal(28, races.Count);
        foreach (var race in races)
        {
            var frequencies = race!["BiomeFrequency"]!.AsObject();
            Assert.NotEmpty(frequencies);
            Assert.All(frequencies, f => Assert.True(f.Value!.GetValue<double>() > 0));
        }

        JsonObject Entry(string id) => races.Single(r => r!["Id"]!.GetValue<string>() == id)!.AsObject();
        Assert.Equal(
            ["BorealForest", "TemperateGrassland", "WoodlandAndShrubland", "TemperateMixedForest",
             "TemperateBroadleafForest", "TemperateRainForest", "TropicalDryForest", "TropicalRainForest"],
            Entry("elf")["BiomeFrequency"]!.AsObject().Select(f => f.Key));

        // A race without a height modifier does not carry one in the file.
        Assert.False(Entry("midlanders").ContainsKey("HeightModifier"));
        Assert.Equal(
            """[{"HeightM":200,"Factor":0},{"HeightM":200,"Factor":1}]""",
            Entry("dwarf")["HeightModifier"]!.ToJsonString());

        // The biomes that are left out read back as 0, so the list survives the round trip.
        var back = ParameterSet.FromJson(ParameterSet.Default.ToJson());
        Assert.Equal(ParameterSet.Default.ToJson(), back.ToJson());
        Assert.Equal(ParameterSet.Default.Races.Select(r => r.BiomeFrequency), back.Races.Select(r => r.BiomeFrequency));
        Assert.Equal(0, back.Races.Single(r => r.Id == "elf").BiomeFrequency[Biome.Tundra]);
        Assert.Equal(RaceParameters.OnlyBelow(200), back.Races.Single(r => r.Id == "elf").HeightModifier);
        Assert.Null(back.Races.Single(r => r.Id == "midlanders").HeightModifier);

        // The schema does not ask for the biomes that are left out, nor for a height modifier.
        var schema = ParameterSetSchema.Build()["properties"]!["Races"]!["items"]!;
        var frequency = schema["properties"]!["BiomeFrequency"]!;
        Assert.Null(frequency["required"]);
        Assert.Equal(16, frequency["properties"]!.AsObject().Count);
        Assert.DoesNotContain("HeightModifier", schema["required"]!.AsArray().Select(n => n!.GetValue<string>()));
        Assert.False(string.IsNullOrWhiteSpace(schema["properties"]!["HeightModifier"]!["description"]?.GetValue<string>()));
    }

    [Fact]
    public void The_races_that_are_on_share_a_patch_by_their_frequency_in_its_biome()
    {
        var rules = new PopulationRules(ParameterSet.Default);
        Assert.Equal(DefaultRaces, rules.Races.Select(r => r.Id));
        int At(string id) => Array.IndexOf(DefaultRaces, id);

        // Without height modifiers the shares are the frequencies over their sum, at any height.
        var plain = new PopulationRules(With(ParameterSet.Default.Races.Select(r => r with { HeightModifier = null }).ToArray()));
        foreach (var biome in Enum.GetValues<Biome>())
        {
            var shares = plain.Shares(biome, 100);
            Assert.Equal(28, shares.Length);
            Assert.Equal(1, shares.Sum(), 12);
            var sum = plain.Races.Sum(r => r.BiomeFrequency[biome]);
            for (var k = 0; k < shares.Length; k++)
                Assert.Equal(plain.Races[k].BiomeFrequency[biome] / sum, shares[k], 12);
            Assert.Equal(shares, plain.Shares(biome, 900));
        }

        // Temperate broadleaf forest: the frequencies sum to 62.5, of which Midlanders and Canal folk 15 each,
        // elves 6 and dwarves 2. Below 200 m there are no dwarves, from 200 m and up no elves.
        var low = rules.Shares(Biome.TemperateBroadleafForest, 199);
        Assert.Equal(15 / 60.5, low[At("midlanders")], 12);
        Assert.Equal(15 / 60.5, low[At("canalFolk")], 12);
        Assert.Equal(6 / 60.5, low[At("elf")], 12);
        Assert.Equal(0, low[At("dwarf")]);
        var high = rules.Shares(Biome.TemperateBroadleafForest, 200);
        Assert.Equal(15 / 56.5, high[At("midlanders")], 12);
        Assert.Equal(0, high[At("elf")]);
        Assert.Equal(2 / 56.5, high[At("dwarf")], 12);

        // Ice & snow: Northmen 3, Hoarkin 3, dwarves 2 from 200 m and up, and Hearthbloods 1,
        // at half weight below 100 m and full weight from 400 m.
        var ice = rules.Shares(Biome.IceAndSnow, 450);
        Assert.Equal(4, ice.Count(x => x > 0));
        Assert.Equal(3 / 9.0, ice[At("northmen")], 12);
        Assert.Equal(3 / 9.0, ice[At("hoarkin")], 12);
        Assert.Equal(2 / 9.0, ice[At("dwarf")], 12);
        Assert.Equal(1 / 9.0, ice[At("hearthbloods")], 12);
        var shore = rules.Shares(Biome.IceAndSnow, 50);
        Assert.Equal(3 / 6.5, shore[At("hoarkin")], 12);
        Assert.Equal(0, shore[At("dwarf")]);
        Assert.Equal(0.5 / 6.5, shore[At("hearthbloods")], 12);
    }

    [Fact]
    public void The_height_modifier_is_a_line_through_its_points_with_a_step_where_two_share_a_height()
    {
        var rules = new PopulationRules(With(
            Race("anywhere"),
            Race("high", RaceParameters.OnlyFrom(200)),
            Race("low", RaceParameters.OnlyBelow(200)),
            Race("ramp", [new(300, 1), new(100, 0)]), // given out of order
            Race("band", [new(100, 0), new(100, 2), new(400, 2), new(400, -1)]),
            Race("empty", [])));

        void Check(double heightM, params double[] expected)
        {
            for (var k = 0; k < expected.Length; k++)
                Assert.Equal(expected[k], rules.HeightFactor(k, heightM), 12);
        }

        Check(0, 1, 0, 1, 0, 0, 1);
        Check(100, 1, 0, 1, 0, 2, 1);
        Check(150, 1, 0, 1, 0.25, 2, 1);
        Check(199, 1, 0, 1, 0.495, 2, 1);
        Check(200, 1, 1, 0, 0.5, 2, 1);
        Check(300, 1, 1, 0, 1, 2, 1);
        Check(400, 1, 1, 0, 1, 0, 1); // a negative factor counts as 0
        Check(2500, 1, 1, 0, 1, 0, 1);

        // The factor weighs the frequency: at 150 m the ramp race counts a quarter and the band race double.
        var shares = rules.Shares(Biome.Savanna, 150);
        double[] weights = [1, 0, 1, 0.25, 2, 1];
        for (var k = 0; k < weights.Length; k++) Assert.Equal(weights[k] / 5.25, shares[k], 12);
    }

    [Fact]
    public void A_race_with_a_height_modifier_only_gets_the_land_at_its_heights()
    {
        var rules = new PopulationRules(With(Race("high", RaceParameters.OnlyFrom(200)), Race("low", RaceParameters.OnlyBelow(200))));
        var population = rules.Region(
        [
            (Biome.Savanna, 80, 64, Resources(0.5), 0.5),
            (Biome.Savanna, 199.9, 64, Resources(0.5), 0.5),
            (Biome.Savanna, 650, 64, Resources(0.25), 0.5),
        ]);
        Assert.Equal(96, population.Of("high")); // 64 km² × 6 × 0.25
        Assert.Equal(384, population.Of("low")); // 2 × 64 km² × 6 × 0.5

        // With only the high race on, the lowland holds nobody.
        var highOnly = new PopulationRules(With(Race("high", RaceParameters.OnlyFrom(200)))).Region(
        [
            (Biome.Savanna, 80, 64, Resources(0.5), 0.5),
            (Biome.Savanna, 650, 64, Resources(0.25), 0.5),
        ]);
        Assert.Equal(96, highOnly.Total);
    }

    [Fact]
    public void A_race_that_is_off_gets_nothing_and_the_others_share_the_land_without_it()
    {
        var midlanders = new RaceParameters("midlanders", "Midlanders", true, Everywhere(3));
        var elf = new RaceParameters("elf", "Elves", true, Everywhere(1));
        (Biome, double, double, PatchResources, double)[] land = [(Biome.Savanna, 120, 64, Resources(0.5), 0.5)];

        var both = new PopulationRules(With(midlanders, elf)).Region(land);
        Assert.Equal(192, both.Total); // 64 km² × 6 × 0.5
        Assert.Equal(144, both.Of("midlanders"));
        Assert.Equal(48, both.Of("elf"));

        var withoutElves = new PopulationRules(With(midlanders, elf with { Enabled = false })).Region(land);
        Assert.Equal(["midlanders"], withoutElves.Races.Select(r => r.Race));
        Assert.Equal(192, withoutElves.Of("midlanders"));
        Assert.Equal(0, withoutElves.Of("elf"));
        Assert.Equal(both.Total, withoutElves.Total);
    }

    [Fact]
    public void Nobody_lives_in_a_biome_that_no_race_that_is_on_lives_in()
    {
        var frost = new RaceParameters("frostFolk", "Frost folk", true, Everywhere(0) with { IceAndSnow = 3, Tundra = -5 });
        var rules = new PopulationRules(With(frost));
        Assert.Equal([1.0], rules.Shares(Biome.IceAndSnow, 120));
        Assert.Empty(rules.Shares(Biome.Savanna, 120));
        Assert.Empty(rules.Shares(Biome.Tundra, 120)); // a negative frequency counts as 0

        var population = rules.Region(
        [
            (Biome.Savanna, 120, 64, Resources(1), 0.5),
            (Biome.Tundra, 120, 64, Resources(1), 0.5),
            (Biome.IceAndSnow, 120, 64, Resources(0.25), 0.5),
        ]);
        Assert.Equal(96, population.Of("frostFolk"));
        Assert.Equal(96, population.Total);

        // With no race on, the region has no potential population at all.
        var nobody = new PopulationRules(With(frost with { Enabled = false })).Region([(Biome.IceAndSnow, 120, 64, Resources(1), 0.5)]);
        Assert.Empty(nobody.Races);
        Assert.Equal(0, nobody.Total);
    }

    [Fact]
    public void The_capacity_of_a_patch_follows_its_area_food_and_prosperity()
    {
        var rules = new PopulationRules(ParameterSet.Default);
        Assert.Equal(64 * 6 * 0.5, rules.Capacity(64, Resources(0.5), 0.5), 9);
        Assert.Equal(2 * rules.Capacity(64, Resources(0.5), 0.5), rules.Capacity(128, Resources(0.5), 0.5), 9);
        Assert.Equal(0, rules.Capacity(64, Resources(0), 0.5));

        // Prosperity: from half at the lowest to one and a half at the highest.
        Assert.Equal(64 * 6 * 0.5 * 0.5, rules.Capacity(64, Resources(0.5), 0), 9);
        Assert.Equal(64 * 6 * 0.5 * 1.5, rules.Capacity(64, Resources(0.5), 1), 9);

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
        var race = new RaceParameters("midlanders", "Midlanders", true, Everywhere(1));
        Assert.Throws<ArgumentException>(() => new PopulationRules(With(race, race with { Name = "Other", Enabled = false })));
        Assert.Throws<ArgumentException>(() => new PopulationRules(With(race with { Id = " " })));
        Assert.Throws<ArgumentException>(() => World.Create("cellgen", With(race, race)));
    }

    [Fact]
    public void A_planned_region_holds_the_potential_population_of_its_land_per_race()
    {
        var world = World.Create("cellgen");
        var rules = world.PopulationRules;
        var patchKm = world.Cells.PatchSize / 1000;
        long people = 0, dwarves = 0, elves = 0;
        foreach (var id in RegionStageRules.RegionsWithin(new RegionId(0, 0), 1))
        {
            var population = world.Advance(id, RegionStage.Planned).Plan!.Population;
            Assert.Equal(DefaultRaces, population.Races.Select(r => r.Race));
            Assert.All(population.Races, r => Assert.True(r.Persons >= 0));
            Assert.Same(population, world.PlanOf(id).Population);

            // The same sum from the Elevated data: the mean height of the land patches and their final biomes and resources.
            var land = world.ElevationOf(id).Patches;
            Assert.Equal(world.DetailOf(id).Patches.Count(x => !x.Shape.Ocean), land.Length);
            var expected = new double[DefaultRaces.Length];
            foreach (var patch in land)
            {
                if (patch.MeanHeightM is { } mean) Assert.Equal(Math.Max(0, mean), patch.HeightM);
                var capacity = rules.Capacity(patch.Patch.Shape.Cells.Length * patchKm * patchKm, patch.Resources, patch.Patch.Prosperity);
                var shares = rules.Shares(patch.Biome.Biome, patch.HeightM);
                for (var k = 0; k < shares.Length; k++) expected[k] += capacity * shares[k];
            }
            for (var k = 0; k < expected.Length; k++)
                Assert.Equal((long)Math.Floor(expected[k] + 0.5), population.Races[k].Persons);

            // A region without land from 200 m and up has no dwarves; one without land below it no elves.
            if (!land.Any(x => x.HeightM >= 200)) Assert.Equal(0, population.Of("dwarf"));
            if (!land.Any(x => x.HeightM < 200)) Assert.Equal(0, population.Of("elf"));

            people += population.Total;
            dwarves += population.Of("dwarf");
            elves += population.Of("elf");
        }
        // Nine regions of the default world: a few tens of thousands of persons each, with both highland and lowland.
        Assert.InRange(people / 9, 10_000, 100_000);
        Assert.True(dwarves > 0);
        Assert.True(elves > 0);
    }

    [Fact]
    public void Turning_races_off_shares_the_same_land_between_fewer_races()
    {
        var region = new RegionId(0, 0);
        var all = World.Create("cellgen").Advance(region, RegionStage.Planned).Plan!;
        var humans = World.Create("cellgen", OnlyOn(HumanRaces)).Advance(region, RegionStage.Planned).Plan!;

        Assert.Equal(28, all.Population.Races.Count);
        Assert.Equal(HumanRaces, humans.Population.Races.Select(r => r.Race));
        // The seven human races live in every biome and at every height between them, so the land feeds
        // as many persons with them alone. Each race is rounded on its own.
        Assert.InRange(all.Population.Total - humans.Population.Total, -28, 28);
        foreach (var race in HumanRaces)
            Assert.True(humans.Population.Of(race) >= all.Population.Of(race));
        Assert.True(humans.Population.Of("midlanders") > all.Population.Of("midlanders"));
        // The races do not move the Prospects.
        Assert.Equal(DomainJson.Serialize(all.Prospects), DomainJson.Serialize(humans.Prospects));
    }
}
