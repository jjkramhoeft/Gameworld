using Gameworld.Domain;
using Gameworld.Generation;
using Gameworld.Generation.People;
using Gameworld.Generation.Stages;

namespace Gameworld.Tests;

/// <summary>The persons of the households of a region: society-generation.md section 7.</summary>
public class PersonTests
{
    private static readonly GridCoord Here = new(3, -2);
    private static readonly HouseholdPlace Town = new(SettlementTier.Town, false);
    private static readonly HouseholdPlace Capital = new(SettlementTier.Capital, true);

    private static PersonRules Rules(ParameterSet? parameters = null, uint seed = 7)
    {
        var p = parameters ?? ParameterSet.Default;
        return new PersonRules(p, new LooksRules(p, seed), seed);
    }

    private static Household Home(
        int slot, int family, WealthLevel wealth = WealthLevel.Common, Sex sex = Sex.Male, int age = 40,
        int? faction = null, HouseholdKin? kin = null, params (string Race, long Persons)[] servants) =>
        new(slot, 0, faction, "midlanders", wealth, "Alder", sex, age, family,
            servants.Select(s => new RacePersons(s.Race, s.Persons)).ToArray(), kin);

    private static Faction Faction(FactionType type, params string[] vocations) =>
        new(0, FactionRole.Ruling, type, FactionForm.Crown, "midlanders", new PatchId(0, 0), null, vocations, null, 0.5);

    /// <summary>What holds for the persons of every household.</summary>
    private static void AssertSound(Household household, IReadOnlyList<Person> persons, ParameterSet parameters)
    {
        Assert.Equal(household.Persons, persons.Count);
        Assert.Equal(Enumerable.Range(0, persons.Count), persons.Select(p => p.Slot));
        Assert.All(persons, p => Assert.Equal(household.Slot, p.Household));
        Assert.All(persons, p => Assert.InRange(p.Age, 0, PersonRules.MaxAge));
        Assert.All(persons, p => Assert.False(string.IsNullOrWhiteSpace(p.GivenName)));
        Assert.All(persons, p => Assert.NotNull(p.Looks));

        // The head is the one of the Stub, and the family is as large as the Stub says.
        var head = persons[0];
        Assert.Equal((HouseholdRole.Head, household.HeadSex, household.HeadAge, household.Race), (head.Role, head.Sex, head.Age, head.Race));
        Assert.Single(persons, p => p.Role == HouseholdRole.Head);
        var family = persons.Where(p => p.Role != HouseholdRole.Servant).ToList();
        Assert.Equal(household.Family, family.Count);
        Assert.All(family, p => Assert.Equal((household.Surname, household.Race), (p.Surname, p.Race)));

        // The servants follow the family, each of the race the Stub gives.
        var servants = persons.Skip(household.Family).ToList();
        Assert.All(servants, p => Assert.Equal(HouseholdRole.Servant, p.Role));
        Assert.Equal(
            household.Servants.SelectMany(s => Enumerable.Repeat(s.Race, (int)s.Persons)),
            servants.Select(p => p.Race));
        Assert.All(servants, p => Assert.InRange(p.Age, PersonRules.MinServantAge, PersonRules.MaxServantAge));
        Assert.All(servants, p => Assert.Null(p.Spouse ?? p.Father ?? p.Mother));

        // Husband and wife point at each other and are of age.
        foreach (var p in persons.Where(p => p.Spouse != null))
        {
            var other = persons[p.Spouse!.Value];
            Assert.Equal(p.Slot, other.Spouse);
            Assert.NotEqual(p.Sex, other.Sex);
            Assert.True(p.Age >= PersonRules.MinSpouseAge);
        }
        Assert.True(persons.Count(p => p.Role == HouseholdRole.Spouse) <= 1);
        if (persons.SingleOrDefault(p => p.Role == HouseholdRole.Spouse) is { } spouse) Assert.Equal(0, spouse.Spouse);

        // A father is a man and a mother a woman, each old enough to be the parent.
        foreach (var p in persons)
        {
            if (p.Father is { } f)
            {
                Assert.Equal(Sex.Male, persons[f].Sex);
                Assert.True(persons[f].Age - p.Age >= PersonRules.ParentAgeGap, $"A father of {persons[f].Age} with a child of {p.Age}.");
            }
            if (p.Mother is { } m)
            {
                Assert.Equal(Sex.Female, persons[m].Sex);
                Assert.True(persons[m].Age - p.Age >= PersonRules.ParentAgeGap, $"A mother of {persons[m].Age} with a child of {p.Age}.");
                Assert.True(persons[m].Age - p.Age <= PersonRules.MotherAgeSpan, $"A mother of {persons[m].Age} with a child of {p.Age}.");
            }
        }

        // A child is the child of the head and of its husband or wife; a parent is the parent of the head and its siblings.
        var couple = persons.Where(p => p.Role is HouseholdRole.Head or HouseholdRole.Spouse).ToList();
        foreach (var child in persons.Where(p => p.Role == HouseholdRole.Child))
        {
            Assert.Equal(couple.SingleOrDefault(p => p.Sex == Sex.Male)?.Slot, child.Father);
            Assert.Equal(couple.SingleOrDefault(p => p.Sex == Sex.Female)?.Slot, child.Mother);
        }
        var parents = persons.Where(p => p.Role == HouseholdRole.Parent).ToList();
        Assert.True(parents.Count <= 2);
        Assert.Equal(parents.Count, parents.Select(p => p.Sex).Distinct().Count());
        if (household.Kin != null) Assert.Empty(parents);
        foreach (var born in persons.Where(p => p.Role is HouseholdRole.Head or HouseholdRole.Sibling))
        {
            Assert.Equal(parents.SingleOrDefault(p => p.Sex == Sex.Male)?.Slot, born.Father);
            Assert.Equal(parents.SingleOrDefault(p => p.Sex == Sex.Female)?.Slot, born.Mother);
        }
        Assert.All(persons.Where(p => p.Role == HouseholdRole.Sibling), p =>
            Assert.True(Math.Abs(p.Age - head.Age) <= PersonRules.SiblingAgeSpread && p.Age >= PersonRules.MinSiblingAge));

        // A vocation is one of the list, and nobody below the working age has one.
        var vocations = parameters.Vocations.Where(v => v.Enabled).Select(v => v.Id).ToHashSet();
        Assert.All(persons.Where(p => p.Vocation != null), p => Assert.Contains(p.Vocation!, vocations));
        Assert.All(family.Where(p => p.Age < parameters.People.WorkingAge), p => Assert.Null(p.Vocation));
    }

    [Fact]
    public void The_defaults_are_those_of_the_doc_and_are_read_from_a_file()
    {
        var p = ParameterSet.Default.People;
        Assert.Equal((0.75, 14, 0.1, 0.6, "servant"), (p.SpouseChance, p.WorkingAge, p.LandWorkFactor, p.SameVocationChance, p.ServantVocation));

        var read = ParameterSet.FromJson("""{ "People": { "WorkingAge": 12 } }""").People;
        Assert.Equal((12, 0.75), (read.WorkingAge, read.SpouseChance));

        // Every default vocation has a standing on the scale of six, as research/vocations.md gives it.
        var vocations = ParameterSet.Default.Vocations;
        Assert.All(vocations, v => Assert.True(1 <= v.Standing.Min && v.Standing.Min <= v.Standing.Max && v.Standing.Max <= 6, v.Id));
        IntRange Standing(string id) => vocations.Single(v => v.Id == id).Standing;
        Assert.Equal(
            (new IntRange(6, 6), new IntRange(4, 5), new IntRange(1, 5), new IntRange(2, 2), new IntRange(1, 1)),
            (Standing("noble"), Standing("priest"), Standing("courtesan"), Standing("servant"), Standing("beggar")));
        Assert.Equal(13, vocations.Count(v => v.Standing.Min < v.Standing.Max));
    }

    [Fact]
    public void A_household_gets_its_head_its_family_and_its_servants()
    {
        var parameters = ParameterSet.Default;
        var rules = Rules();
        var household = Home(4, family: 5, WealthLevel.Rich, Sex.Female, 44, servants: [("midlanders", 2), ("dwarf", 1)]);
        var persons = rules.PersonsOf(Here, household, null, Capital);
        AssertSound(household, persons, parameters);
        Assert.Equal(8, persons.Count);

        // The servants have the vocation of a servant and a surname of their own.
        var servants = persons.Skip(5).ToList();
        Assert.Equal(["midlanders", "midlanders", "dwarf"], servants.Select(p => p.Race));
        Assert.All(servants, p => Assert.Equal("servant", p.Vocation));
        Assert.All(servants, p => Assert.False(string.IsNullOrWhiteSpace(p.Surname)));

        // The same household gives the same persons; another household, region or seed gives others.
        string Json(IReadOnlyList<Person> list) => DomainJson.Serialize(list.Select(p => p with { Household = 0 }));
        Assert.Equal(Json(persons), Json(rules.PersonsOf(Here, household, null, Capital)));
        Assert.NotEqual(Json(persons), Json(rules.PersonsOf(Here, household with { Slot = 5 }, null, Capital)));
        Assert.NotEqual(Json(persons), Json(rules.PersonsOf(new GridCoord(3, -1), household, null, Capital)));
        Assert.NotEqual(Json(persons), Json(Rules(seed: 8).PersonsOf(Here, household, null, Capital)));

        // A household of one is its head alone.
        var alone = Home(9, family: 1, WealthLevel.Poor, Sex.Female, 23);
        var only = Assert.Single(rules.PersonsOf(Here, alone, null, Town));
        Assert.Equal((HouseholdRole.Head, Sex.Female, 23, "Alder"), (only.Role, only.Sex, only.Age, only.Surname));
        Assert.Null(only.Spouse);
    }

    [Fact]
    public void Ages_and_family_links_hold_in_every_household()
    {
        var parameters = ParameterSet.Default;
        var rules = Rules();
        var roles = new Dictionary<HouseholdRole, int>();
        var withSpouse = 0;
        var households = 0;
        var levels = Enum.GetValues<WealthLevel>();
        for (var slot = 0; slot < 1200; slot++)
        {
            var household = Home(
                slot,
                family: 1 + slot % 8,
                levels[slot % levels.Length],
                slot % 5 == 0 ? Sex.Female : Sex.Male,
                age: 20 + slot * 7 % 46,
                kin: slot % 6 == 0 ? new HouseholdKin(0, HouseholdRole.Sibling) : null,
                servants: slot % 4 == 0 ? [("midlanders", 1 + slot % 3)] : []);
            var persons = rules.PersonsOf(Here, household, null, slot % 2 == 0 ? Town : Capital);
            AssertSound(household, persons, parameters);
            foreach (var p in persons) roles[p.Role] = roles.GetValueOrDefault(p.Role) + 1;
            if (household.Family >= 2)
            {
                households++;
                if (persons.Any(p => p.Role == HouseholdRole.Spouse)) withSpouse++;
            }
        }

        // Every position occurs, children most of all, and three heads in four of a family of two or more have a husband or a wife.
        Assert.All(Enum.GetValues<HouseholdRole>(), role => Assert.True(roles.GetValueOrDefault(role) > 0, role.ToString()));
        Assert.True(roles[HouseholdRole.Child] > roles[HouseholdRole.Parent] + roles[HouseholdRole.Sibling]);
        Assert.InRange(withSpouse / (double)households, 0.70, 0.80);

        // Without the chance of a spouse there is none, and with a working age of 0 every person of the family works.
        var changed = parameters with { People = new PeopleParameters { SpouseChance = 0, WorkingAge = 0 } };
        var single = Rules(changed).PersonsOf(Here, Home(3, family: 6), null, Town);
        AssertSound(Home(3, family: 6), single, changed);
        Assert.DoesNotContain(single, p => p.Role == HouseholdRole.Spouse);
        Assert.All(single, p => Assert.NotNull(p.Vocation));
    }

    [Fact]
    public void The_head_of_a_household_holds_a_vocation_that_fits_its_wealth_and_its_place()
    {
        var parameters = ParameterSet.Default;
        var rules = Rules();
        var byId = parameters.Vocations.ToDictionary(v => v.Id);
        var seen = new Dictionary<WealthLevel, HashSet<string>>();
        foreach (var wealth in Enum.GetValues<WealthLevel>())
            foreach (var place in new[] { Town, Capital, new HouseholdPlace(SettlementTier.City, false) })
                for (var slot = 0; slot < 150; slot++)
                {
                    var household = Home(slot, family: 4, wealth);
                    var persons = rules.PersonsOf(Here, household, null, place);
                    var vocation = byId[persons[0].Vocation!];
                    Assert.True(PersonRules.Fits(vocation, wealth), $"{vocation.Id} for a {wealth} household.");
                    Assert.True(vocation.LowestTier <= place.Tier, $"{vocation.Id} in a {place.Tier}.");
                    Assert.True(!vocation.OnWater || place.OnWater, $"{vocation.Id} away from water.");
                    if (!seen.TryGetValue(wealth, out var set)) seen[wealth] = set = [];
                    set.Add(vocation.Id);

                    // The others of working age hold the vocation of the head or one that fits the same wealth.
                    foreach (var p in persons.Skip(1).Where(p => p.Vocation != null))
                    {
                        var other = byId[p.Vocation!];
                        Assert.True(other.Id == vocation.Id || PersonRules.Fits(other, wealth), $"{other.Id} in a {wealth} household.");
                        Assert.True(other.Id == vocation.Id || rules.WeightIn(other, place) > 0);
                    }
                }
        Assert.Contains("beggar", seen[WealthLevel.Destitute]);
        Assert.Contains("labourer", seen[WealthLevel.Poor]);
        Assert.Contains("weaver", seen[WealthLevel.Common]);
        Assert.Contains("smith", seen[WealthLevel.Comfortable]);
        Assert.Contains("merchant", seen[WealthLevel.Rich]);
        Assert.DoesNotContain("merchant", seen[WealthLevel.Common]);

        // The standing of a vocation against the five levels of wealth.
        Assert.Equal((1, 1), PersonRules.StandingOf(WealthLevel.Destitute));
        Assert.Equal((5, 6), PersonRules.StandingOf(WealthLevel.Rich));
        Assert.True(PersonRules.Fits(byId["noble"], WealthLevel.Rich));
        Assert.False(PersonRules.Fits(byId["noble"], WealthLevel.Comfortable));
        Assert.True(PersonRules.Fits(byId["healer"], WealthLevel.Common) && PersonRules.Fits(byId["healer"], WealthLevel.Rich));

        // The work of the land is rare inside a city or a town, a city trade is not found in a town, and a sailor needs water.
        Assert.Equal(3.5, rules.WeightIn(byId["farmer"], Town), 9);
        Assert.Equal(0, rules.WeightIn(byId["merchant"], Town));
        Assert.Equal(0.5, rules.WeightIn(byId["merchant"], Capital));
        Assert.Equal(0, rules.WeightIn(byId["sailor"], Town));
        Assert.Equal(1.2, rules.WeightIn(byId["sailor"], Capital));

        // Where no vocation fits the wealth, the nearest lower level gives one; with no vocation at all there is none.
        // (What is worn names vocations of the list, so the short lists go without the dress.)
        var few = parameters with { Vocations = [byId["labourer"], byId["smith"]], Hairstyles = [], Outfits = [], Accessories = [] };
        Assert.Equal("smith", Rules(few).FittingVocation(WealthLevel.Rich, Town, 0.5));
        Assert.Equal("labourer", Rules(few).FittingVocation(WealthLevel.Common, Town, 0.5));
        Assert.Equal("labourer", Rules(few).FittingVocation(WealthLevel.Destitute, Town, 0.5));
        Assert.Null(Rules(few with { Vocations = [] }).FittingVocation(WealthLevel.Common, Town, 0.5));
    }

    [Fact]
    public void The_head_of_a_household_of_a_faction_holds_a_vocation_of_the_faction()
    {
        var parameters = ParameterSet.Default;
        var rules = Rules();
        string? Head(Faction faction, PersonRules? with = null, WealthLevel wealth = WealthLevel.Rich) =>
            (with ?? rules).PersonsOf(Here, Home(1, family: 3, wealth, faction: 0), faction, Town)[0].Vocation;

        // Those who lead a faction that gathers no vocation, by its type. The tier of the place does not hold them back.
        Assert.Equal("noble", Head(Faction(FactionType.Family)));
        Assert.Equal("noble", Head(Faction(FactionType.Race)));
        Assert.Equal("merchant", Head(Faction(FactionType.Vocation)));
        Assert.Equal("priest", Head(Faction(FactionType.Belief)));
        Assert.Equal("mage", Head(Faction(FactionType.Magic)));
        Assert.Equal("official", Head(Faction(FactionType.Place)));

        // A faction that gathers vocations: one of them, whatever the wealth of the household.
        Assert.Equal("smith", Head(Faction(FactionType.Vocation, "smith"), wealth: WealthLevel.Common));
        var company = Enumerable.Range(0, 60)
            .Select(slot => rules.PersonsOf(Here, Home(slot, family: 1, faction: 0), Faction(FactionType.Vocation, "soldier", "guard"), Town)[0].Vocation)
            .ToHashSet();
        Assert.Equal(["guard", "soldier"], company.OrderBy(v => v));

        // A vocation that is off is not held: the head gets one that fits the wealth of the household.
        var off = parameters with { Vocations = parameters.Vocations.Select(v => v.Id is "noble" or "servant" ? v with { Enabled = false } : v).ToArray() };
        var fallback = Head(Faction(FactionType.Family), Rules(off));
        Assert.NotEqual("noble", fallback);
        Assert.True(PersonRules.Fits(parameters.Vocations.Single(v => v.Id == fallback), WealthLevel.Rich));

        // And a servant has no vocation when the vocation of the servants is off.
        var served = Rules(off).PersonsOf(Here, Home(2, family: 1, servants: [("midlanders", 2)]), null, Town);
        Assert.All(served.Skip(1), p => Assert.Null(p.Vocation));
    }

    [Fact]
    public void The_tier_of_a_place_follows_from_what_it_is()
    {
        Settlement Place(SettlementKind kind, bool capital) => new(0, "Alderbury", kind, capital, new PatchId(0, 0), 1, 10, []);
        Assert.Equal(SettlementTier.Capital, PersonRules.TierOf(Place(SettlementKind.City, true)));
        Assert.Equal(SettlementTier.City, PersonRules.TierOf(Place(SettlementKind.City, false)));
        Assert.Equal(SettlementTier.Town, PersonRules.TierOf(Place(SettlementKind.Town, false)));
    }

    [Fact]
    public void Generated_regions_people_every_household()
    {
        var world = World.Create("cellgen");
        var total = 0;
        foreach (var id in RegionStageRules.RegionsWithin(new RegionId(0, 0), 1))
        {
            var plan = world.PlanOf(id);
            var settlements = SettlementBuilder.Build(world, plan).Settlements;
            var households = HouseholdBuilder.Build(world, plan, settlements);
            var people = PeopleBuilder.Build(world, plan, settlements, households);
            total += people.Persons.Count;

            foreach (var household in households.Households)
                AssertSound(household, people.Of(household).ToList(), world.Parameters);

            // Every person of a city or a town is a person of a household, race by race.
            foreach (var settlement in settlements)
            {
                var slots = households.In(settlement).Select(h => h.Slot).ToHashSet();
                var living = people.Persons.Where(p => slots.Contains(p.Household)).ToList();
                Assert.Equal(settlement.Persons, living.Count);
                foreach (var race in settlement.Races)
                    Assert.Equal(race.Persons, living.Count(p => p.Race == race.Race));
            }

            // The head of a branch of a house is the age the kinship allows.
            foreach (var branch in households.Households.Where(h => h.Kin != null))
            {
                var first = households.Households[branch.Kin!.Household];
                Assert.Equal((first.Faction, first.Surname), (branch.Faction, branch.Surname));
                if (branch.Kin.Relation == HouseholdRole.Child) Assert.True(first.HeadAge - branch.HeadAge >= 18);
            }
        }
        Assert.True(total > 300, $"Only {total} persons.");
    }

    [Fact]
    public void The_households_and_the_persons_of_a_region_are_stored_with_it()
    {
        var world = World.Create("cellgen");
        var id = new RegionId(0, 0);
        Assert.Null(world.FindHouseholds(id));
        Assert.Null(world.FindPeople(id));

        var region = world.Advance(id, RegionStage.Housed);
        Assert.Equal(RegionStage.Housed, region.Stage);
        Assert.Same(world.FindHouseholds(id), region.Households);
        Assert.Null(region.People);
        Assert.Null(world.FindPeople(id));

        var people = world.PeopleOf(id);
        Assert.Equal(RegionStage.Peopled, region.Stage);
        Assert.Same(people, region.People);
        Assert.Same(region.Households, world.HouseholdsOf(id));
        Assert.Equal(region.Settlement!.CityPersons + region.Settlement.TownPersons, people.Persons.Count);
        Assert.Equal(region.Households!.Households.Sum(h => h.Persons), people.Persons.Count);

        // The stages of the regions around it are not moved by the two stages: they need the region itself only.
        Assert.All(RegionStageRules.Bordering(id), r => Assert.True(world.StageOf(r) < RegionStage.Settled));

        // They come back from the stored form of the region.
        var restored = Region.FromSnapshot(DomainJson.Deserialize<RegionSnapshot>(DomainJson.Serialize(region.ToSnapshot())));
        Assert.Equal(RegionStage.Peopled, restored.Stage);
        Assert.Equal(DomainJson.Serialize(region.Households), DomainJson.Serialize(restored.Households));
        Assert.Equal(DomainJson.Serialize(region.People), DomainJson.Serialize(restored.People));
    }
}
