using Gameworld.Domain;
using Gameworld.Generation;
using Gameworld.Generation.Races;
using Gameworld.Generation.Settlements;
using Gameworld.Generation.Stages;
using static Gameworld.Tests.TestData;

namespace Gameworld.Tests;

/// <summary>The cities and towns of a region: society-generation.md section 5.2.</summary>
public class SettlementTests
{
    private static RaceParameters Race(string id, double frequency) => new(id, id, true, Everywhere(frequency));

    /// <summary>
    /// A Parameter Set with the given races at 10 persons per km², so that a patch of 100 km² with a
    /// food supply of 1 feeds 1,000 persons. Every race of the region is a main race.
    /// </summary>
    private static ParameterSet With(SettlementParameters? settlement, params RaceParameters[] races) => ParameterSet.Default with
    {
        Races = races,
        Planning = ParameterSet.Default.Planning with
        {
            Population = new PopulationParameters { PersonsPerKm2 = 10, ProsperityEffect = 0, MinorityRacesPerRegion = 0 },
        },
        Settlement = settlement ?? new SettlementParameters(),
    };

    /// <summary>The rules, and the races of a region of one patch: "a" with three quarters of its persons and "b" with a quarter.</summary>
    private static (SettlementRules Rules, RegionRaces Races) Rules(SettlementParameters? settlement = null, double areaKm2 = 100)
    {
        var parameters = With(settlement, Race("a", 3), Race("b", 1));
        var population = new PopulationRules(parameters, 7);
        var amounts = new ResourceAmount[ResourceCatalog.Count];
        var races = population.Share(default, [new PopulationPatch(Biome.TemperateGrassland, 100, areaKm2, new PatchResources(amounts, 1, 0), 0.5)]);
        return (new SettlementRules(parameters, population), races);
    }

    /// <summary>A Prospect on show that holds both races, "a" three times as heavy as "b".</summary>
    private static SettlementSite Site(int slot, bool city) => new(slot, $"Place {slot}", new PatchId(slot, 0), city, [0, 1], [3, 1]);

    private static long Of(IEnumerable<RacePersons> races, string race) => races.Where(r => r.Race == race).Sum(r => r.Persons);

    [Fact]
    public void The_default_shares_are_a_tenth_in_cities_and_a_tenth_in_towns()
    {
        var p = ParameterSet.Default.Settlement;
        Assert.Equal((0.1, 0.1, 1), (p.CityShare, p.TownShare, p.RankExponent));

        // The shares are read from a Parameter Set file, and a file that leaves them out keeps the defaults.
        var read = ParameterSet.FromJson("""{ "Settlement": { "CityShare": 0.25 } }""");
        Assert.Equal((0.25, 0.1, 1), (read.Settlement.CityShare, read.Settlement.TownShare, read.Settlement.RankExponent));
    }

    [Fact]
    public void The_cities_and_the_towns_take_their_share_of_the_region_and_split_it_by_rank()
    {
        var (rules, races) = Rules();
        Assert.Equal(1000, races.Population.Total);

        var built = rules.Build(races, [Site(0, city: true), Site(1, city: true), Site(2, city: false), Site(3, city: false), Site(4, city: false)], new PatchId(0, 0));

        // A tenth of the 1,000 persons in the cities and a tenth in the towns.
        Assert.Equal((100, 100, 800), (built.CityPersons, built.TownPersons, built.RuralPersons));

        // The capital has rank 1, the other city rank 2 and the towns the ranks 3 to 5. A place
        // weighs 1 / its rank, and each kind shares its persons by largest remainder.
        Assert.Equal([0, 1, 2, 3, 4], built.Settlements.Select(s => s.Slot));
        Assert.Equal([1, 2, 3, 4, 5], built.Settlements.Select(s => s.Rank));
        Assert.Equal(
            [SettlementKind.City, SettlementKind.City, SettlementKind.Town, SettlementKind.Town, SettlementKind.Town],
            built.Settlements.Select(s => s.Kind));
        Assert.Equal([true, false, false, false, false], built.Settlements.Select(s => s.Capital));
        Assert.Equal([67L, 33, 43, 32, 25], built.Settlements.Select(s => s.Persons));

        // The persons of a place are shared between its races by their weight: three to one.
        Assert.Equal([new RacePersons("a", 50), new RacePersons("b", 17)], built.Settlements[0].Races);
        Assert.Equal([new RacePersons("a", 25), new RacePersons("b", 8)], built.Settlements[1].Races);
        Assert.Equal([new RacePersons("a", 19), new RacePersons("b", 6)], built.Settlements[4].Races);

        // Every person of the region is in a city, in a town or on the land, race by race.
        Assert.Equal([new RacePersons("a", 600), new RacePersons("b", 200)], built.Rural);
        foreach (var race in races.Population.Races)
            Assert.Equal(race.Persons, Of(built.Rural, race.Race) + built.Settlements.Sum(s => Of(s.Races, race.Race)));
    }

    [Fact]
    public void The_capital_is_a_city_whatever_its_Prospect_is_for_and_ranks_first()
    {
        var (rules, races) = Rules();
        var built = rules.Build(races, [Site(0, city: true), Site(1, city: true), Site(2, city: false)], new PatchId(2, 0));

        Assert.Equal([2, 0, 1], built.Settlements.Select(s => s.Slot));
        Assert.All(built.Settlements, s => Assert.Equal(SettlementKind.City, s.Kind));
        Assert.Equal([true, false, false], built.Settlements.Select(s => s.Capital));
        Assert.Equal([55L, 27, 18], built.Settlements.Select(s => s.Persons));

        // The region has no town, so the share of the towns stays on the land.
        Assert.Equal((100, 0, 900), (built.CityPersons, built.TownPersons, built.RuralPersons));

        // Without a capital the cities and the towns follow their Prospects.
        var plain = rules.Build(races, [Site(0, city: true), Site(1, city: false)], null);
        Assert.Equal([SettlementKind.City, SettlementKind.Town], plain.Settlements.Select(s => s.Kind));
        Assert.DoesNotContain(plain.Settlements, s => s.Capital);
        Assert.Equal([100L, 100], plain.Settlements.Select(s => s.Persons));

        // A region without a Prospect on show has no city and no town: all its persons are on the land.
        var none = rules.Build(races, [], null);
        Assert.Empty(none.Settlements);
        Assert.Equal(1000, none.RuralPersons);
    }

    [Fact]
    public void No_city_or_town_stands_empty_where_the_region_has_a_person_for_it()
    {
        SettlementSite[] sites = [Site(0, city: true), .. Enumerable.Range(1, 5).Select(i => Site(i, city: false))];

        // 30 persons: a tenth is 3 for the city, and the 5 towns get a person each, not 3 between them.
        var (rules, races) = Rules(areaKm2: 3);
        var total = races.Population.Total;
        Assert.InRange(total, 30, 31);
        var small = rules.Build(races, sites, new PatchId(0, 0));
        Assert.Equal([3L, 1, 1, 1, 1, 1], small.Settlements.Select(s => s.Persons));
        Assert.Equal(total - 8, small.RuralPersons);

        // 4 persons and 6 places: the capital and the first towns hold one each, and nobody is counted twice.
        (rules, races) = Rules(areaKm2: 0.4);
        Assert.Equal(4, races.Population.Total);
        var tiny = rules.Build(races, sites, new PatchId(0, 0));
        Assert.Equal([1L, 1, 1, 1, 0, 0], tiny.Settlements.Select(s => s.Persons));
        Assert.Equal(0, tiny.RuralPersons);

        // A region without persons has its cities and towns, with nobody in them.
        (rules, races) = Rules(areaKm2: 0);
        var empty = rules.Build(races, sites, null);
        Assert.Equal(6, empty.Settlements.Length);
        Assert.All(empty.Settlements, s => Assert.Equal(0, s.Persons));
    }

    [Fact]
    public void Persons_are_shared_by_largest_remainder_and_the_last_ranks_are_not_left_with_nobody()
    {
        // 36 persons by 1, 1/2, 1/3, 1/4: quotas 17.28, 8.64, 5.76 and 4.32.
        Assert.Equal([17L, 9, 6, 4], SettlementRules.Apportion(36, [1, 1.0 / 2, 1.0 / 3, 1.0 / 4]));
        Assert.Equal([9L, 9, 9, 9], SettlementRules.Apportion(36, [1, 1, 1, 1]));
        // Ties go to the first.
        Assert.Equal([2L, 2, 1], SettlementRules.Apportion(5, [1, 1, 1]));
        // The first would take all five: the others take one each from it.
        Assert.Equal([2L, 1, 1, 1], SettlementRules.Apportion(5, [1, 0.01, 0.01, 0.01]));
        // Fewer persons than places: the first ranks hold one each.
        Assert.Equal([1L, 1, 0], SettlementRules.Apportion(2, [1, 0.5, 0.25]));
        Assert.Equal([0L, 0], SettlementRules.Apportion(0, [1, 1]));
        Assert.Empty(SettlementRules.Apportion(7, []));
    }

    [Fact]
    public void The_shares_and_the_rank_exponent_are_parameters()
    {
        SettlementSite[] sites = [Site(0, city: true), Site(1, city: true), Site(2, city: false), Site(3, city: false)];

        // A share of 0 leaves the kind without persons, and its places stand empty.
        var (rules, races) = Rules(new SettlementParameters { CityShare = 0, TownShare = 0.3 });
        var noCities = rules.Build(races, sites, new PatchId(0, 0));
        Assert.Equal((0, 300, 700), (noCities.CityPersons, noCities.TownPersons, noCities.RuralPersons));

        // Two shares that sum to more than 1 are scaled down together: 0.9 and 0.6 are 0.6 and 0.4.
        (rules, races) = Rules(new SettlementParameters { CityShare = 0.9, TownShare = 0.6 });
        Assert.Equal(0.6, rules.CityShare, 12);
        Assert.Equal(0.4, rules.TownShare, 12);
        var all = rules.Build(races, sites, new PatchId(0, 0));
        Assert.Equal((600, 400, 0), (all.CityPersons, all.TownPersons, all.RuralPersons));
        Assert.All(all.Rural, r => Assert.Equal(0, r.Persons));
        Assert.Equal(750, all.Settlements.Sum(s => Of(s.Races, "a")));
        Assert.Equal(250, all.Settlements.Sum(s => Of(s.Races, "b")));

        // A rank exponent of 0 gives the cities the same size, and the towns the same size.
        (rules, races) = Rules(new SettlementParameters { RankExponent = 0 });
        var even = rules.Build(races, sites, new PatchId(0, 0));
        Assert.Equal([50L, 50, 50, 50], even.Settlements.Select(s => s.Persons));

        // A higher exponent gives the capital more: by 1 / rank² the two cities share 100 as 80 and 20.
        (rules, races) = Rules(new SettlementParameters { RankExponent = 2 });
        var steep = rules.Build(races, sites, new PatchId(0, 0));
        Assert.Equal([80L, 20], steep.Settlements.Take(2).Select(s => s.Persons));
        Assert.Equal(1, rules.RankWeight(1));
        Assert.Equal(0.25, rules.RankWeight(2), 12);
    }

    [Fact]
    public void A_race_is_never_placed_beyond_its_persons_in_the_region()
    {
        // Half the region in one city that weighs "b" nine times as heavy as "a": "b" has 250 persons only.
        var (rules, races) = Rules(new SettlementParameters { CityShare = 0.5, TownShare = 0 });
        var built = rules.Build(races, [new SettlementSite(0, "Alderbury", new PatchId(0, 0), true, [1, 0], [1, 9])], new PatchId(0, 0));
        Assert.Equal([new RacePersons("b", 250), new RacePersons("a", 250)], built.Settlements[0].Races);
        Assert.Equal([new RacePersons("a", 500), new RacePersons("b", 0)], built.Rural);

        // A city that lists "b" only: the persons "b" cannot fill are of the other race of the region, last in the list.
        var one = rules.Build(races, [new SettlementSite(0, "Alderbury", new PatchId(0, 0), true, [1], [1, 9])], new PatchId(0, 0));
        Assert.Equal(500, one.Settlements[0].Persons);
        Assert.Equal([new RacePersons("b", 250), new RacePersons("a", 250)], one.Settlements[0].Races);
    }

    [Fact]
    public void Generated_regions_hold_a_tenth_of_their_persons_in_cities_and_a_tenth_in_towns()
    {
        var world = World.Create("cellgen");
        long persons = 0, inCities = 0, inTowns = 0;
        foreach (var id in RegionStageRules.RegionsWithin(new RegionId(0, 0), 1))
        {
            var plan = world.PlanOf(id);
            var built = SettlementBuilder.Build(world, plan);
            var total = plan.Population.Total;
            persons += total;
            inCities += built.CityPersons;
            inTowns += built.TownPersons;

            // A city or a town on every Prospect on show, and on no other.
            var onShow = Enumerable.Range(0, plan.Prospects.Length).Where(i => plan.Prospects[i].OnShow).ToArray();
            Assert.Equal(onShow, built.Settlements.Select(s => s.Slot).OrderBy(s => s));
            Assert.All(built.Settlements, s => Assert.Equal(plan.Prospects[s.Slot].Patch, s.Patch));
            Assert.Equal(Enumerable.Range(1, built.Settlements.Length), built.Settlements.Select(s => s.Rank));

            // The capital is the seat of the ruling faction: a city, and the first by rank.
            var seat = plan.Factions.Factions.Single(f => f.Role == FactionRole.Ruling).Seat;
            var capital = Assert.Single(built.Settlements, s => s.Capital);
            Assert.Equal(seat, capital.Patch);
            Assert.Equal((1, SettlementKind.City), (capital.Rank, capital.Kind));
            Assert.All(built.Settlements, s => Assert.Equal(
                plan.Prospects[s.Slot].ForCity || s.Capital ? SettlementKind.City : SettlementKind.Town, s.Kind));

            // The cities come before the towns, and within a kind no place is larger than one before it.
            var kinds = built.Settlements.Select(s => s.Kind).ToArray();
            Assert.Equal(kinds.OrderByDescending(k => k), kinds);
            foreach (var kind in Enum.GetValues<SettlementKind>())
            {
                var sizes = built.Settlements.Where(s => s.Kind == kind).Select(s => s.Persons).ToArray();
                Assert.Equal(sizes.OrderByDescending(n => n), sizes);
            }

            // Nobody stands empty, and the races of a place are races of its Prospect, in its order.
            Assert.All(built.Settlements, s => Assert.True(s.Persons >= 1));
            foreach (var s in built.Settlements)
            {
                Assert.Equal(plan.ProspectRaces[s.Slot], s.Races.Select(r => r.Race));
                Assert.Equal(s.Persons, s.Races.Sum(r => r.Persons));
            }

            // Every person of the region is in a city, in a town or on the land, race by race.
            Assert.Equal(plan.Population.Races.Select(r => r.Race), built.Rural.Select(r => r.Race));
            foreach (var race in plan.Population.Races)
            {
                Assert.True(Of(built.Rural, race.Race) >= 0);
                Assert.Equal(race.Persons, Of(built.Rural, race.Race) + built.Settlements.Sum(s => Of(s.Races, race.Race)));
            }

            // Each region is within a person or so of a tenth for each kind.
            Assert.InRange(built.CityPersons, 0.1 * total - 1, 0.1 * total + 1);
            Assert.InRange(built.TownPersons, 0.1 * total - 1, 0.1 * total + 1);
        }
        Assert.InRange((double)inCities / persons, 0.098, 0.102);
        Assert.InRange((double)inTowns / persons, 0.098, 0.102);
    }

    [Fact]
    public void A_settled_region_stores_its_cities_and_towns()
    {
        var world = World.Create("cellgen");
        var id = new RegionId(0, 0);
        Assert.Null(world.FindSettlement(id));

        var region = world.Advance(id, RegionStage.Settled);
        var expected = SettlementBuilder.Build(world, world.PlanOf(id));
        var settlement = region.Settlement!;
        Assert.Equal(DomainJson.Serialize(expected.Settlements), DomainJson.Serialize(settlement.Settlements));
        Assert.Equal(DomainJson.Serialize(expected.Rural), DomainJson.Serialize(settlement.Rural));
        Assert.Same(world.FindSettlement(id)!.Places.Settlements, settlement.Settlements);

        Assert.NotNull(settlement.Capital);
        Assert.Equal(region.Plan!.Population.Total, settlement.CityPersons + settlement.TownPersons + settlement.RuralPersons);

        // The cities and towns come back from the stored form of the region.
        var restored = Region.FromSnapshot(DomainJson.Deserialize<RegionSnapshot>(DomainJson.Serialize(region.ToSnapshot())));
        Assert.Equal(DomainJson.Serialize(settlement.Settlements), DomainJson.Serialize(restored.Settlement!.Settlements));
    }
}
