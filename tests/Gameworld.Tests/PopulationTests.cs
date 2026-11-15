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

    /// <summary>
    /// The rules are tested at 6 persons per km², a hundred times the default, so that a patch cell of
    /// 64 km² feeds whole persons: 384 at a food supply of 1. The minority minimum is 10 persons with it.
    /// </summary>
    private static readonly PopulationParameters Dense = new() { PersonsPerKm2 = 6, MinorityMinPersons = 10 };

    private static ParameterSet With(params RaceParameters[] races) => ParameterSet.Default with
    {
        Races = races,
        Planning = ParameterSet.Default.Planning with { Population = Dense },
    };

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
        PopulationPatch[] land = [(Biome.Savanna, 120, 64, Resources(0.5), 0.5)];

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
        Assert.Equal(64 * 0.06 * 0.5, rules.Capacity(64, Resources(0.5), 0.5), 9);
        Assert.Equal(2 * rules.Capacity(64, Resources(0.5), 0.5), rules.Capacity(128, Resources(0.5), 0.5), 9);
        Assert.Equal(0, rules.Capacity(64, Resources(0), 0.5));

        // Prosperity: from half at the lowest to one and a half at the highest.
        Assert.Equal(64 * 0.06 * 0.5 * 0.5, rules.Capacity(64, Resources(0.5), 0), 9);
        Assert.Equal(64 * 0.06 * 0.5 * 1.5, rules.Capacity(64, Resources(0.5), 1), 9);

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

    /// <summary>Eight races that live everywhere, with the frequencies 8 down to 1, and one patch that feeds 384 persons.</summary>
    private static readonly string[] Eight = ["a", "b", "c", "d", "e", "f", "g", "h"];

    private static ParameterSet EightRaces(PopulationParameters population) => ParameterSet.Default with
    {
        Races = Eight.Select((id, k) => new RaceParameters(id, id, true, Everywhere(8 - k))).ToArray(),
        Planning = ParameterSet.Default.Planning with { Population = population },
    };

    private static readonly PopulationPatch[] OnePatch = [(Biome.Savanna, 120, 64, Resources(1), 0.5)];

    [Fact]
    public void The_turn_pattern_gives_every_race_a_turn_in_every_block_of_regions()
    {
        // The stride is the one with the longest shortest step between two regions with the same turn: 6 for 28 races.
        Assert.Equal(6, PopulationRules.StrideFor(28));
        Assert.Equal(0, PopulationRules.StrideFor(0));
        Assert.Equal(0, PopulationRules.StrideFor(1));
        Assert.Equal(1, PopulationRules.StrideFor(2));

        foreach (var seed in new uint[] { 0, 7, 123456789 })
        {
            var rules = new PopulationRules(ParameterSet.Default, seed);
            Assert.Equal(6, rules.TurnStride);
            // Every 6 × 5 block of regions holds a turn for each of the 28 races, in each of the two minority places.
            foreach (var (i0, j0) in new[] { (0, 0), (-9, -7), (13, -40), (-1000, 2000) })
                for (var place = 0; place < 2; place++)
                {
                    var turns = new HashSet<int>();
                    for (var j = j0; j < j0 + 5; j++)
                        for (var i = i0; i < i0 + 6; i++)
                            turns.Add(rules.TurnRace(new GridCoord(i, j), place));
                    Assert.Equal(28, turns.Count);
                    Assert.All(turns, k => Assert.InRange(k, 0, 27));
                }
            // The two places of one region are half the list apart, so they never go to the same race.
            Assert.NotEqual(rules.TurnRace(new GridCoord(3, 4), 0), rules.TurnRace(new GridCoord(3, 4), 1));
            // A race has its turns on a lattice: the steps (4, 4) and (−2, 5) lead to the same race.
            Assert.Equal(rules.TurnRace(new GridCoord(3, 4), 0), rules.TurnRace(new GridCoord(7, 8), 0));
            Assert.Equal(rules.TurnRace(new GridCoord(3, 4), 0), rules.TurnRace(new GridCoord(1, 9), 0));
        }

        // The order of the turns comes from the seed.
        var a = new PopulationRules(ParameterSet.Default, 1);
        var b = new PopulationRules(ParameterSet.Default, 2);
        Assert.Contains(Enumerable.Range(0, 28), i => a.TurnRace(new GridCoord(i, 0), 0) != b.TurnRace(new GridCoord(i, 0), 0));

        // Without minority places no race has a turn.
        var p = ParameterSet.Default;
        var none = new PopulationRules(p with
        {
            Planning = p.Planning with { Population = p.Planning.Population with { MinorityRacesPerRegion = 0 } },
        });
        Assert.Equal(-1, none.TurnRace(new GridCoord(0, 0), 0));
    }

    [Fact]
    public void A_region_holds_its_main_races_and_a_minority_race_by_turn_or_by_draw()
    {
        var population = Dense with { RacesPerRegion = 4, MinorityRacesPerRegion = 1, MinorityShare = 0.1 };
        var rules = new PopulationRules(EightRaces(population), 42);
        Assert.Equal(384, rules.Capacity(64, Resources(1), 0.5), 9);

        var minorities = new HashSet<int>();
        int byTurn = 0, byDraw = 0;
        for (var j = 0; j < 8; j++)
            for (var i = 0; i < 8; i++)
            {
                var region = new GridCoord(i, j);
                var races = rules.Share(region, OnePatch);

                // The plain shares: 384 persons by the frequencies 8 … 1 of 36.
                for (var k = 0; k < 8; k++) Assert.Equal(384 * (8 - k) / 36.0, races.Plain[k], 9);

                // Three main races, the largest first, and one minority race among the other five.
                Assert.Equal([0, 1, 2], races.Races[..3]);
                Assert.Equal(3, races.MainCount);
                Assert.Equal(4, races.Races.Length);
                var minority = races.Races[3];
                Assert.InRange(minority, 3, 7);
                Assert.True(races.IsMinority(minority));
                Assert.False(races.IsMinority(0));
                Assert.Equal([minority], races.Minority.ToArray());
                minorities.Add(minority);

                // The race whose turn it is takes the place, unless it is a main race already.
                var turn = rules.TurnRace(region, 0);
                if (turn >= 3)
                {
                    Assert.Equal(turn, minority);
                    byTurn++;
                }
                else byDraw++;

                // Only the four races of the region share the land, and they share all of it.
                var result = races.Population;
                Assert.Equal(Eight, result.Races.Select(r => r.Race));
                Assert.Equal(["a", "b", "c"], result.MainRaces);
                Assert.Equal([Eight[minority]], result.MinorityRaces);
                Assert.Equal(4, result.RegionRaces.Count());
                Assert.All(result.Races.Where(r => r.Standing == RaceStanding.Absent), r => Assert.Equal(0, r.Persons));
                Assert.Equal(384, races.Persons.Sum(), 9);

                // The lift: a minority race below a tenth of the region is lifted to a tenth. On one
                // patch the lift is exact. A larger one keeps its share and its boost of 1.
                var frequency = 8 - minority;
                var unboosted = frequency / (21.0 + frequency);
                Assert.Equal(Math.Max(0.1, unboosted) * 384, races.Persons[minority], 9);
                Assert.Equal(unboosted < 0.1 ? 0.1 * (1 - unboosted) / (unboosted * 0.9) : 1, races.Boost[3], 9);
                Assert.Equal([1.0, 1.0, 1.0], races.Boost[..3]);
                Assert.Equal(races.Persons[minority], races.PatchPersons[0][3], 9);
            }
        // Over 64 regions every one of the five other races holds the place somewhere, most often by its turn.
        Assert.Equal(5, minorities.Count);
        Assert.Equal(40, byTurn); // five of the eight races have their turn in a region whose place they can take
        Assert.Equal(24, byDraw);

        // The region does not matter to a rule without minority places, and the lift can be turned off.
        var plainRules = new PopulationRules(EightRaces(population with { MinorityShare = 0 }), 42);
        var unlifted = plainRules.Share(new GridCoord(0, 0), OnePatch);
        var last = unlifted.Races[3];
        Assert.Equal(384 * (8 - last) / (21.0 + 8 - last), unlifted.Persons[last], 9);

        // A race takes a minority place only where the land would feed enough of it: h has 10.67 persons.
        var strict = new PopulationRules(EightRaces(population with { MinorityMinPersons = 11 }), 42);
        for (var i = 0; i < 16; i++)
            Assert.NotEqual(7, strict.Share(new GridCoord(i, 0), OnePatch).Races[3]);
        // When no race would be fed enough, the place stays empty.
        var empty = new PopulationRules(EightRaces(population with { MinorityMinPersons = 100 }), 42).Share(new GridCoord(0, 0), OnePatch);
        Assert.Equal([0, 1, 2], empty.Races);
        Assert.Empty(empty.Population.MinorityRaces);

        // Enough places for every race, and none of them a minority place: the plain shares of every race.
        var open = new PopulationRules(EightRaces(Dense with { RacesPerRegion = 8, MinorityRacesPerRegion = 0 }), 42)
            .Share(new GridCoord(5, 5), OnePatch);
        Assert.Equal(8, open.MainCount);
        for (var k = 0; k < 8; k++) Assert.Equal(open.Plain[k], open.Persons[k], 9);

        // No places at all: the region holds nobody.
        var closed = new PopulationRules(EightRaces(Dense with { RacesPerRegion = 0 }), 42).Share(new GridCoord(5, 5), OnePatch);
        Assert.Empty(closed.Races);
        Assert.Equal(0, closed.Population.Total);
    }

    [Fact]
    public void A_patch_where_none_of_the_races_of_the_region_lives_holds_nobody()
    {
        var warm = new RaceParameters("warm", "Warm folk", true, Everywhere(0) with { Savanna = 5 });
        var cold = new RaceParameters("cold", "Cold folk", true, Everywhere(0) with { IceAndSnow = 1 });
        var p = With(warm, cold);
        PopulationPatch[] land =
        [
            (Biome.Savanna, 120, 128, Resources(0.5), 0.5), // 384 persons
            (Biome.IceAndSnow, 120, 64, Resources(0.25), 0.5), // 96 persons
        ];

        // With a place for each, each race holds the land it lives on.
        var both = new PopulationRules(p).Region(land);
        Assert.Equal(384, both.Of("warm"));
        Assert.Equal(96, both.Of("cold"));

        // With one place the region holds the larger race only, and the ice is left empty.
        var one = new PopulationRules(p with
        {
            Planning = p.Planning with { Population = Dense with { RacesPerRegion = 1, MinorityRacesPerRegion = 0 } },
        }).Share(new GridCoord(0, 0), land);
        Assert.Equal(["warm"], one.Population.RegionRaces);
        Assert.Equal(384, one.Population.Total);
        Assert.Equal(RaceStanding.Absent, one.Population.Races[1].Standing);
        Assert.Equal(96, one.Plain[1], 9);
        Assert.Equal([0.0], one.PatchPersons[1]);
    }

    [Fact]
    public void A_city_or_a_town_holds_the_largest_races_around_it_and_one_drawn_race_of_its_region()
    {
        // a, b, c and e live everywhere; d only on the tundra. With five places of which one is a minority
        // place, the four largest are main races and e, the smallest, is the minority race.
        RaceParameters[] races =
        [
            new("a", "a", true, Everywhere(4)), new("b", "b", true, Everywhere(3)), new("c", "c", true, Everywhere(2)),
            new("d", "d", true, Everywhere(0) with { Tundra = 30 }), new("e", "e", true, Everywhere(1)),
        ];
        PopulationPatch[] land =
        [
            (Biome.Savanna, 120, 64, Resources(1), 0.5),
            (Biome.Savanna, 120, 64, Resources(1), 0.5),
            (Biome.Tundra, 120, 64, Resources(1), 0.5),
        ];
        PopulationRules Rules(double minorityBoost, double hinterland, int perCity = 3, int perTown = 2) => new(ParameterSet.Default with
        {
            Races = races,
            Planning = ParameterSet.Default.Planning with
            {
                Population = new PopulationParameters
                {
                    RacesPerRegion = 5, MinorityRacesPerRegion = 1, MinorityShare = 0, RacesPerCity = perCity, RacesPerTown = perTown,
                    MinorityBoost = minorityBoost, HinterlandShare = hinterland,
                },
            },
        }, 99);

        var rules = Rules(minorityBoost: 1e12, hinterland: 0);
        var region = rules.Share(new GridCoord(0, 0), land);
        Assert.Equal(4, region.MainCount);
        Assert.Equal([4], region.Minority.ToArray());

        // On the savanna d does not live. A town holds the largest race and the drawn one, a city two and the drawn one.
        // With a very large boost the drawn one is the minority race.
        var spot = new GridCoord(11, 12);
        Assert.Equal([0, 4], rules.SettlementRaces(region, [0, 1], forCity: false, spot));
        Assert.Equal([0, 1, 4], rules.SettlementRaces(region, [0, 1], forCity: true, spot));
        // On the tundra d is the largest: 30 of 40.
        Assert.Equal([3, 4], rules.SettlementRaces(region, [2], forCity: false, spot));

        // Without a boost the draw goes by the weights, and with a boost of 0 the minority race is never drawn.
        var never = Rules(minorityBoost: 0, hinterland: 0);
        var drawn = new HashSet<int>();
        for (var i = 0; i < 200; i++)
        {
            var town = never.SettlementRaces(never.Share(new GridCoord(0, 0), land), [0, 1], forCity: false, new GridCoord(i, 3));
            Assert.Equal(2, town.Length);
            Assert.Equal(0, town[0]);
            drawn.Add(town[1]);
        }
        Assert.Equal([1, 2], drawn.Order());

        // The hinterland lets a race of the region that does not live nearby into the draw.
        var wide = Rules(minorityBoost: 1, hinterland: 1);
        var wideRegion = wide.Share(new GridCoord(0, 0), land);
        Assert.Contains(Enumerable.Range(0, 200), i => wide.SettlementRaces(wideRegion, [0, 1], false, new GridCoord(i, 3))[1] == 3);

        // A city or a town never holds more races than the region has, and none without a place.
        var roomy = Rules(minorityBoost: 5, hinterland: 0.02, perCity: 9, perTown: 0);
        var roomyRegion = roomy.Share(new GridCoord(0, 0), land);
        Assert.Equal([0, 1, 2, 3, 4], roomy.SettlementRaces(roomyRegion, [0, 1, 2], forCity: true, spot).Order());
        Assert.Empty(roomy.SettlementRaces(roomyRegion, [0, 1, 2], forCity: false, spot));
    }

    [Fact]
    public void A_planned_region_holds_six_races_that_share_its_potential_population()
    {
        var world = World.Create("cellgen");
        var rules = world.PopulationRules;
        var patchKm = world.Cells.PatchSize / 1000;
        long people = 0;
        foreach (var id in RegionStageRules.RegionsWithin(new RegionId(0, 0), 1))
        {
            var plan = world.Advance(id, RegionStage.Planned).Plan!;
            var population = plan.Population;
            Assert.Equal(DefaultRaces, population.Races.Select(r => r.Race));
            Assert.All(population.Races, r => Assert.True(r.Persons >= 0));
            Assert.Same(population, world.PlanOf(id).Population);

            // Four main races and two minority races; the other 22 hold nobody.
            Assert.Equal(4, population.MainRaces.Count());
            Assert.Equal(2, population.MinorityRaces.Count());
            Assert.All(population.Races.Where(r => r.Standing == RaceStanding.Absent), r => Assert.Equal(0, r.Persons));

            // The plain shares from the Elevated data: the mean height of the land patches and their final biomes and resources.
            var land = world.ElevationOf(id).Patches;
            Assert.Equal(world.DetailOf(id).Patches.Count(x => !x.Shape.Ocean), land.Length);
            var plain = new double[DefaultRaces.Length];
            var capacity = 0.0;
            foreach (var patch in land)
            {
                if (patch.MeanHeightM is { } mean) Assert.Equal(Math.Max(0, mean), patch.HeightM);
                var c = rules.Capacity(patch.Patch.Shape.Cells.Length * patchKm * patchKm, patch.Resources, patch.Patch.Prosperity);
                var shares = rules.Shares(patch.Biome.Biome, patch.HeightM);
                if (shares.Length > 0) capacity += c;
                for (var k = 0; k < shares.Length; k++) plain[k] += c * shares[k];
            }
            var races = world.PlanOf(id).Races;
            for (var k = 0; k < plain.Length; k++) Assert.Equal(plain[k], races.Plain[k], 6);

            // The main races are the four largest by the plain shares, the largest first.
            var largest = Enumerable.Range(0, plain.Length).OrderByDescending(k => plain[k]).ThenBy(k => k).Take(4).ToArray();
            Assert.Equal(largest, races.Races[..4]);
            // A minority race is the one whose turn it is, or a drawn one, and the land would feed at least a tenth of a person of it.
            for (var place = 0; place < 2; place++)
            {
                var minority = races.Races[4 + place];
                Assert.True(plain[minority] >= 0.1);
                var turn = rules.TurnRace(id.Coord, place);
                if (!largest.Contains(turn) && plain[turn] >= 0.1 && turn != races.Races[4]) Assert.Equal(turn, minority);
            }

            // The six races share the capacity of the land they live on: never more than all of it.
            Assert.InRange(population.Total, 0.8 * capacity, capacity + 6);
            for (var k = 0; k < plain.Length; k++)
                Assert.Equal((long)Math.Floor(races.Persons[k] + 0.5), population.Races[k].Persons);
            // A main race holds more than its plain share, since fewer races share the land.
            foreach (var k in largest) Assert.True(races.Persons[k] > plain[k]);

            // A region without land from 200 m and up has no dwarves; one without land below it no elves.
            if (!land.Any(x => x.HeightM >= 200)) Assert.Equal(0, population.Of("dwarf"));
            if (!land.Any(x => x.HeightM < 200)) Assert.Equal(0, population.Of("elf"));

            // A city holds up to five and a town up to four of the races of the region.
            Assert.Equal(plan.Prospects.Count, plan.ProspectRaces.Count);
            var ofRegion = population.RegionRaces.ToHashSet();
            for (var n = 0; n < plan.Prospects.Count; n++)
            {
                var held = plan.ProspectRaces[n];
                Assert.Equal(plan.Prospects[n].ForCity ? 5 : 4, held.Count);
                Assert.Equal(held.Count, held.Distinct().Count());
                Assert.All(held, race => Assert.Contains(race, ofRegion));
            }

            people += population.Total;
        }
        // Nine regions of the default world: a few hundred persons each.
        Assert.InRange(people / 9, 100, 1_000);
    }

    [Fact]
    public void A_cluster_of_thirty_regions_holds_nearly_every_race_in_its_regions_and_its_settlements()
    {
        var world = World.Create("cellgen");
        var inRegions = new HashSet<string>();
        var inSettlements = new HashSet<string>();
        for (var j = -2; j <= 2; j++)
            for (var i = -3; i <= 2; i++)
            {
                var plan = world.Advance(new RegionId(i, j), RegionStage.Planned).Plan!;
                Assert.InRange(plan.Population.RegionRaces.Count(), 1, 6);
                inRegions.UnionWith(plan.Population.RegionRaces);
                for (var n = 0; n < plan.Prospects.Count; n++)
                    if (plan.Prospects[n].OnShow)
                        inSettlements.UnionWith(plan.ProspectRaces[n]);
            }
        // 6 × 5 regions hold a turn for each of the 28 races in both minority places. A race can
        // miss its turns where the land does not feed it, so one race may be missing.
        Assert.InRange(inRegions.Count, 27, 28);
        Assert.InRange(inSettlements.Count, 26, 28);
    }

    [Fact]
    public void Turning_races_off_shares_the_same_land_between_fewer_races()
    {
        var region = new RegionId(0, 0);
        // With a place for every race and no minority places, every race that is on has its plain share.
        ParameterSet Open(ParameterSet p) => p with
        {
            Planning = p.Planning with { Population = p.Planning.Population with { RacesPerRegion = 28, MinorityRacesPerRegion = 0 } },
        };
        var all = World.Create("cellgen", Open(ParameterSet.Default)).Advance(region, RegionStage.Planned).Plan!;
        var humans = World.Create("cellgen", Open(OnlyOn(HumanRaces))).Advance(region, RegionStage.Planned).Plan!;

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

        // With the default places a region holds six of the races that are on, and the Prospects stay where they are.
        var capped = World.Create("cellgen").Advance(region, RegionStage.Planned).Plan!;
        var cappedHumans = World.Create("cellgen", OnlyOn(HumanRaces)).Advance(region, RegionStage.Planned).Plan!;
        Assert.Equal(6, capped.Population.RegionRaces.Count());
        Assert.Equal(6, cappedHumans.Population.RegionRaces.Count());
        Assert.All(cappedHumans.Population.RegionRaces, race => Assert.Contains(race, HumanRaces));
        Assert.True(capped.Population.Total <= all.Population.Total + 6);
        Assert.Equal(DomainJson.Serialize(all.Prospects), DomainJson.Serialize(capped.Prospects));
    }
}
