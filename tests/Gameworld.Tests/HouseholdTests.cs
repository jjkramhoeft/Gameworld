using Gameworld.Domain;
using Gameworld.Generation;
using Gameworld.Generation.Households;
using Gameworld.Generation.Stages;

namespace Gameworld.Tests;

/// <summary>The households of a region: society-generation.md section 6.</summary>
public class HouseholdTests
{
    private static readonly GridCoord Here = new(3, -2);

    private static HouseholdRules Rules(HousingParameters? housing = null, uint seed = 7) =>
        new(ParameterSet.Default with { Housing = housing ?? new HousingParameters() }, seed);

    private static Settlement Place(int slot, int rank, bool city, params (string Race, long Persons)[] races) =>
        new(slot, $"Place {slot}", city ? SettlementKind.City : SettlementKind.Town, rank == 1, new PatchId(slot, 0), rank,
            races.Sum(r => r.Persons), races.Select(r => new RacePersons(r.Race, r.Persons)).ToArray());

    private static Faction Faction(int slot, FactionType type, string leadRace, int? seat, FactionRole? role = null) =>
        new(slot, role ?? (slot == 0 ? FactionRole.Ruling : slot == 1 ? FactionRole.Opposition : FactionRole.Lesser), type,
            type == FactionType.Family ? FactionForm.House : FactionForm.Guild, leadRace,
            seat is { } s ? new PatchId(s, 0) : null, null, [], null, 0.5);

    /// <summary>The households of every city or town hold exactly its persons, race by race.</summary>
    private static void AssertExact(IReadOnlyList<Settlement> settlements, RegionHouseholds built)
    {
        Assert.Equal(Enumerable.Range(0, built.Households.Count), built.Households.Select(h => h.Slot));
        foreach (var settlement in settlements)
        {
            var households = built.In(settlement).ToList();
            foreach (var race in settlement.Races)
            {
                var held = households.Where(h => h.Race == race.Race).Sum(h => (long)h.Family)
                    + households.SelectMany(h => h.Servants).Where(s => s.Race == race.Race).Sum(s => s.Persons);
                Assert.True(race.Persons == held, $"{settlement.Name}: {race.Race} has {race.Persons} persons and {held} in households.");
            }
            Assert.Equal(settlement.Persons, households.Sum(h => h.Persons));
            Assert.All(households, h => Assert.Contains(settlement.Races, r => r.Race == h.Race));
        }
        Assert.All(built.Households, h => Assert.True(h.Family >= 1));
        Assert.All(built.Households, h => Assert.Contains(settlements, s => s.Slot == h.Settlement));
    }

    [Fact]
    public void The_defaults_are_those_of_the_doc_and_are_read_from_a_file()
    {
        var p = ParameterSet.Default.Housing;
        Assert.Equal(2, p.FactionHouseholds);
        Assert.Equal(new double[] { 2, 3, 3, 3, 2, 1, 0.5, 0.25 }, p.FamilySizeWeights);
        Assert.Equal(new PerWealthLevel<double>(1, 3, 4, 2, 0.3), p.TownWealth);
        Assert.Equal(new PerWealthLevel<double>(1.5, 3, 3.5, 2.5, 0.8), p.CityWealth);
        Assert.Equal(new PerWealthLevel<double>(0, 0, 1, 3, 2), p.FactionWealth);
        Assert.Equal((new IntRange(0, 0), new IntRange(0, 1), new IntRange(1, 3)), (p.Servants.Common, p.Servants.Comfortable, p.Servants.Rich));
        Assert.Equal((new IntRange(20, 65), 0.8), (p.HeadAge, p.MaleHeadChance));

        var read = ParameterSet.FromJson("""{ "Housing": { "FactionHouseholds": 3, "FamilySizeWeights": [1, 1] } }""").Housing;
        Assert.Equal((3, 2, 0.8), (read.FactionHouseholds, read.FamilySizeWeights.Count, read.MaleHeadChance));
    }

    [Fact]
    public void The_households_of_a_city_or_a_town_hold_exactly_its_persons_race_by_race()
    {
        Settlement[] settlements =
        [
            Place(0, 1, city: true, ("a", 14), ("b", 6)),
            Place(1, 2, city: true, ("a", 5), ("b", 3), ("c", 0)),
            Place(2, 3, city: false, ("b", 2), ("a", 1)),
            Place(3, 4, city: false, ("a", 1)),
            Place(4, 5, city: false, ("a", 0)),
        ];
        Faction[] factions = [Faction(0, FactionType.Family, "a", 0), Faction(2, FactionType.Vocation, "b", 1)];

        var built = Rules().Build(Here, factions, settlements, "Alder");
        AssertExact(settlements, built);

        // A town of one person is one household of one person, and an empty town has none.
        var alone = Assert.Single(built.In(settlements[3]));
        Assert.Equal((1, 0L), (alone.Family, alone.ServantCount));
        Assert.Empty(built.In(settlements[4]));

        // The households of the factions come first, by faction and round; the others follow by the rank of their place.
        Assert.Equal([0, 0, 2, 2], built.Households.Take(4).Select(h => h.Faction));
        var plain = built.Households.Skip(4).ToList();
        Assert.All(plain, h => Assert.Null(h.Faction));
        var ranks = plain.Select(h => settlements.Single(s => s.Slot == h.Settlement).Rank).ToList();
        Assert.Equal(ranks.OrderBy(r => r), ranks);

        // The same region gives the same households, and another seed gives others.
        Assert.Equal(DomainJson.Serialize(built), DomainJson.Serialize(Rules().Build(Here, factions, settlements, "Alder")));
        Assert.NotEqual(DomainJson.Serialize(built), DomainJson.Serialize(Rules(seed: 8).Build(Here, factions, settlements, "Alder")));
    }

    [Fact]
    public void A_house_is_one_family_in_two_households()
    {
        Settlement[] settlements = [Place(0, 1, city: true, ("a", 30), ("b", 10)), Place(1, 2, city: false, ("a", 6))];
        var (minAge, maxAge) = Rules().HeadAge;
        var relations = new HashSet<HouseholdRole>();
        for (var i = 0; i < 60; i++)
        {
            var region = new GridCoord(i, 5);
            Faction[] factions = [Faction(0, FactionType.Family, "a", 0), Faction(2, FactionType.Family, "a", 1)];
            var built = Rules().Build(region, factions, settlements, "Alder");
            AssertExact(settlements, built);

            // The ruling house: its surname is the one in the name of the region, and its first household is Rich.
            var ruling = built.Of(factions[0]).ToList();
            Assert.Equal(2, ruling.Count);
            Assert.All(ruling, h => Assert.Equal(("Alder", "a", 0), (h.Surname, h.Race, h.Settlement)));
            Assert.Equal(WealthLevel.Rich, ruling[0].Wealth);
            Assert.Null(ruling[0].Kin);

            // A great house under it draws a surname of its own, and no other household of the region has either.
            var great = built.Of(factions[1]).ToList();
            Assert.Equal(2, great.Count);
            Assert.Equal(great[0].Surname, great[1].Surname);
            Assert.NotEqual("Alder", great[0].Surname);
            Assert.DoesNotContain(built.Households, h => h.Faction == null && (h.Surname == "Alder" || h.Surname == great[0].Surname));

            // The head of the second household is a child or a sibling of the head of the first.
            foreach (var house in new[] { ruling, great })
            {
                var kin = house[1].Kin!;
                Assert.Equal(house[0].Slot, kin.Household);
                Assert.InRange(house[1].HeadAge, minAge, maxAge);
                if (kin.Relation == HouseholdRole.Child)
                    Assert.True(house[1].HeadAge <= house[0].HeadAge - HouseholdRules.BranchAgeGap);
                else
                {
                    Assert.Equal(HouseholdRole.Sibling, kin.Relation);
                    Assert.True(Math.Abs(house[1].HeadAge - house[0].HeadAge) <= HouseholdRules.SiblingAgeSpread);
                }
                relations.Add(kin.Relation);
            }
        }
        Assert.Equal(2, relations.Count);
    }

    [Fact]
    public void A_faction_takes_its_households_at_its_seat_first_and_then_in_the_other_places()
    {
        // The seat holds one person: the first household is there, the second in the next place by rank that holds the race.
        Settlement[] small = [Place(0, 1, city: true, ("b", 9)), Place(1, 2, city: false, ("a", 1)), Place(2, 3, city: false, ("b", 4)), Place(3, 4, city: false, ("a", 5))];
        var guild = Faction(2, FactionType.Vocation, "a", 1);
        var built = Rules().Build(Here, [guild], small, null);
        AssertExact(small, built);
        Assert.Equal([1, 3], built.Of(guild).Select(h => h.Settlement));
        Assert.All(built.Of(guild), h => Assert.Equal("a", h.Race));

        // Three factions on one seat of four persons: each gets its first head before any gets a second.
        Settlement[] shared = [Place(0, 1, city: true, ("a", 4))];
        Faction[] three = [Faction(0, FactionType.Place, "a", 0), Faction(1, FactionType.Belief, "a", 0), Faction(2, FactionType.Vocation, "a", 0)];
        built = Rules().Build(Here, three, shared, null);
        AssertExact(shared, built);
        Assert.Equal([2, 1, 1], three.Select(f => built.Of(f).Count()));
        Assert.All(built.Households, h => Assert.Equal(1, h.Family));

        // More households per faction is a parameter.
        Settlement[] large = [Place(0, 1, city: true, ("a", 40))];
        built = Rules(new HousingParameters { FactionHouseholds = 3 }).Build(Here, [Faction(0, FactionType.Place, "a", 0)], large, null);
        Assert.Equal(3, built.Households.Count(h => h.Faction == 0));
        built = Rules(new HousingParameters { FactionHouseholds = 0 }).Build(Here, [Faction(0, FactionType.Place, "a", 0)], large, null);
        Assert.DoesNotContain(built.Households, h => h.Faction != null);
        AssertExact(large, built);
    }

    [Fact]
    public void The_leaders_of_a_faction_are_of_its_lead_race_where_a_city_or_town_holds_one()
    {
        // The lead race is not at the seat but in another town: the households are there.
        Settlement[] settlements = [Place(0, 1, city: true, ("b", 8)), Place(1, 2, city: false, ("a", 3), ("b", 2))];
        var order = Faction(2, FactionType.Belief, "a", 0);
        var built = Rules().Build(Here, [order], settlements, null);
        AssertExact(settlements, built);
        Assert.Equal([(1, "a"), (1, "a")], built.Of(order).Select(h => (h.Settlement, h.Race)));

        // The lead race is in no city or town: the households are at the seat, of the race with the most persons there.
        Settlement[] without = [Place(0, 1, city: true, ("b", 3), ("c", 5)), Place(1, 2, city: false, ("b", 2))];
        built = Rules().Build(Here, [order], without, null);
        AssertExact(without, built);
        Assert.Equal([(0, "c"), (0, "c")], built.Of(order).Select(h => (h.Settlement, h.Race)));

        // A faction of the type Race holds its race only: it has no household without it, and no servant of another race.
        var enclave = Faction(2, FactionType.Race, "a", 0);
        built = Rules().Build(Here, [enclave], without, null);
        AssertExact(without, built);
        Assert.Empty(built.Of(enclave));
        Settlement[] mixed = [Place(0, 1, city: true, ("a", 2), ("b", 30))];
        var rich = new HousingParameters { FactionWealth = new(0, 0, 0, 0, 1), Servants = new(new(0, 0), new(0, 0), new(0, 0), new(0, 0), new(3, 3)) };
        built = Rules(rich).Build(Here, [enclave], mixed, null);
        AssertExact(mixed, built);
        Assert.Equal(2, built.Of(enclave).Count());
        Assert.All(built.Of(enclave), h => Assert.Empty(h.Servants));

        // A house keeps its race: the second household is not of another race.
        Settlement[] one = [Place(0, 1, city: true, ("a", 1), ("b", 9))];
        var house = Faction(2, FactionType.Family, "a", 0);
        built = Rules().Build(Here, [house], one, null);
        AssertExact(one, built);
        Assert.Equal(["a"], built.Of(house).Select(h => h.Race));
    }

    [Fact]
    public void A_region_without_a_city_or_a_town_has_no_household()
    {
        var built = Rules().Build(Here, [Faction(0, FactionType.Family, "a", null)], [], "Alder");
        Assert.Empty(built.Households);

        Settlement[] empty = [Place(0, 1, city: true, ("a", 0))];
        Assert.Empty(Rules().Build(Here, [Faction(0, FactionType.Family, "a", 0)], empty, "Alder").Households);
    }

    [Fact]
    public void Wealth_family_size_and_servants_follow_the_parameters()
    {
        var rules = Rules();
        Assert.Equal([1, 8], new[] { rules.FamilySize(0), rules.FamilySize(0.9999) });
        Assert.Equal(1, Rules(new HousingParameters { FamilySizeWeights = [] }).FamilySize(0.5));
        Assert.Equal(3, Rules(new HousingParameters { FamilySizeWeights = [0, 0, 1] }).FamilySize(0.5));
        Assert.Equal(WealthLevel.Destitute, HouseholdRules.WealthOf(new(1, 1, 1, 1, 1), 0));
        Assert.Equal(WealthLevel.Rich, HouseholdRules.WealthOf(new(1, 1, 1, 1, 1), 0.9999));
        Assert.Equal(WealthLevel.Common, HouseholdRules.WealthOf(new(0, 0, 0, 0, 0), 0.5));

        // Rich households of two with two servants each: a place of 40 holds ten of them.
        var housing = new HousingParameters
        {
            FamilySizeWeights = [0, 1],
            TownWealth = new(0, 0, 0, 0, 1),
            Servants = new(new(0, 0), new(0, 0), new(0, 0), new(0, 0), new(2, 2)),
            HeadAge = new IntRange(30, 30),
            MaleHeadChance = 1,
        };
        Settlement[] town = [Place(0, 2, city: false, ("a", 40))];
        var built = Rules(housing).Build(Here, [], town, null);
        AssertExact(town, built);
        Assert.Equal(10, built.Households.Count);
        Assert.All(built.Households, h => Assert.Equal((WealthLevel.Rich, 2, 2L, 30, Sex.Male), (h.Wealth, h.Family, h.ServantCount, h.HeadAge, h.HeadSex)));

        // The servants come from the race with the most persons left.
        Settlement[] two = [Place(0, 2, city: false, ("a", 2), ("b", 2))];
        built = Rules(housing).Build(Here, [], two, null);
        AssertExact(two, built);
        var household = Assert.Single(built.Households);
        Assert.Equal(2, household.ServantCount);
        Assert.NotEqual(household.Race, Assert.Single(household.Servants).Race);
    }

    [Fact]
    public void Generated_regions_house_every_person_of_their_cities_and_towns()
    {
        var world = World.Create("cellgen");
        var households = 0;
        var houses = 0;
        foreach (var id in RegionStageRules.RegionsWithin(new RegionId(0, 0), 1))
        {
            var plan = world.PlanOf(id);
            var settlements = SettlementBuilder.Build(world, plan).Settlements;
            var built = HouseholdBuilder.Build(world, plan, settlements);
            AssertExact(settlements, built);
            households += built.Households.Count;

            foreach (var faction in plan.Factions.Factions)
            {
                var own = built.Of(faction).ToList();
                Assert.True(own.Count <= world.Parameters.Housing.FactionHouseholds);
                if (faction.Type == FactionType.Race) Assert.All(own, h => Assert.Equal(faction.LeadRace, h.Race));
                if (faction.Type != FactionType.Family) continue;
                Assert.True(own.Select(h => h.Surname).Distinct().Count() <= 1);
                Assert.True(own.Select(h => h.Race).Distinct().Count() <= 1);
                if (faction.Role == FactionRole.Ruling && own.Count > 0)
                {
                    Assert.Equal(plan.Naming!.RulingHouse, own[0].Surname);
                    houses++;
                }
            }
        }
        Assert.True(households > 50, $"Only {households} households.");
        Assert.True(houses > 0);
    }
}
