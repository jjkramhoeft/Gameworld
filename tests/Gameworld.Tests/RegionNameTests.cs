using Gameworld.Domain;
using Gameworld.Generation;
using Gameworld.Generation.Basics;
using Gameworld.Generation.Names;
using static Gameworld.Tests.TestData;

namespace Gameworld.Tests;

/// <summary>The region-name generator of docs/RegionNameGenerator-Reuse.md and society-generation.md section 4.10.</summary>
public class RegionNameTests
{
    private static readonly RegionNameSize[] AllSizes = [RegionNameSize.Small, RegionNameSize.Middling, RegionNameSize.Large];

    /// <summary>
    /// The reference names of the reuse doc: seeds 0 to 9 of its Corrected mode. They catch a
    /// word list that was copied wrong, a bad port of the rules and a .NET version that changes
    /// the seeded Random. One name differs from the doc: its "Greath" is "Great" here, since the
    /// misspelled words of the original lists are corrected.
    /// </summary>
    public static TheoryData<bool, int, string, string> ReferenceNames => new()
    {
        { true, 50_000, "Davies", "New Xianbei Republic, Chalukya Region, West Liao Alliance, Neo-Davies Nation, Davies Alliance, Eastern Idisid Nation, Free Saffarid State, Babylonian, Western Davies State, East Mauryan" },
        { true, 500_000, "Davies", "Xianbei Republic, Chalukya Region, Eastern Liao Alliance, Davies Nation, Neo-Davies Alliance, Idisid Nation, Great Saffarid State, Babylonian, Davies State, East Mauryan" },
        { false, 50_000, "Stone", "Toucouleur, Central Empire, South Khmer, Second Stone Region, Old Stone Ground, Western Hotak Nation, Fifth Qin Land, Stone Nation, Stone Land, East Magadha" },
    };

    [Theory]
    [MemberData(nameof(ReferenceNames))]
    public void The_original_seeds_give_the_reference_names(bool human, int populationPotential, string family, string expected)
    {
        var actual = string.Join(", ", Enumerable.Range(0, 10)
            .Select(seed => RegionNameGenerator.Generate(seed, human, populationPotential, family)));
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void Names_are_never_empty_and_carry_no_stray_spaces()
    {
        var distinct = new HashSet<string>();
        var stream = 0;
        foreach (var human in new[] { true, false })
            foreach (var size in AllSizes)
                foreach (var ruler in Enum.GetValues<RegionNameRuler>())
                    foreach (var house in new[] { null, "Davies" })
                    {
                        var rng = Rng(0x4b10, stream++);
                        for (var i = 0; i < 3000; i++)
                        {
                            var name = RegionNameGenerator.Generate(rng, human, size, ruler, house);
                            Assert.InRange(name.Length, 2, 40);
                            Assert.Equal(name.Trim(), name);
                            Assert.DoesNotContain("  ", name);
                            Assert.True(char.IsUpper(name[0]), $"The name {name} does not start with a capital letter.");
                            distinct.Add(name);
                        }
                    }
        // The draws are independent: the lists give far more names than one draw per name would.
        Assert.True(distinct.Count > 20_000, $"Only {distinct.Count} distinct names.");
    }

    [Fact]
    public void The_lead_race_and_the_size_pick_the_word_lists()
    {
        string[] humanPrefixes = ["New ", "Neo-", "United ", "Free "];
        string[] otherPrefixes = ["Old ", "Second ", "Third ", "Fourth ", "Fifth ", "Sixth "];
        string[] largePrefixes = ["Gran ", "Grand ", "Great "];
        string[] smallSuffixes = [" Duchy", " Province", " Len", " County"];
        string[] otherSuffixes = [" Ground", " Sect", " Land", " Khaganate"];

        foreach (var human in new[] { true, false })
            foreach (var size in AllSizes)
            {
                var names = new List<string>();
                var rng = Rng(0x4b11, (human ? 10 : 0) + (int)size);
                for (var i = 0; i < 20_000; i++) names.Add(RegionNameGenerator.Generate(rng, human, size));

                // A list is used where it belongs, every word of it, and nowhere else.
                void Check(string[] words, bool used, bool atStart)
                {
                    foreach (var word in words)
                    {
                        var found = names.Any(n => atStart
                            ? n.StartsWith(word, StringComparison.Ordinal)
                            : n.EndsWith(word, StringComparison.Ordinal));
                        Assert.True(found == used, $"{word.Trim()} with human {human} and {size}: expected used = {used}.");
                    }
                }
                Check(humanPrefixes, human, atStart: true);
                Check(otherPrefixes, !human, atStart: true);
                Check(largePrefixes, human && size == RegionNameSize.Large, atStart: true);
                Check(smallSuffixes, human && size == RegionNameSize.Small, atStart: false);
                Check(otherSuffixes, !human, atStart: false);
            }
    }

    [Fact]
    public void A_prefix_never_starts_with_the_two_letters_of_the_suffix()
    {
        var rng = Rng(0x4b12, 0);
        for (var i = 0; i < 50_000; i++)
        {
            var name = RegionNameGenerator.Generate(rng, i % 2 == 0, RegionNameSize.Middling);
            Assert.False(name.StartsWith("United ", StringComparison.Ordinal) && name.EndsWith(" Union", StringComparison.Ordinal), name);
            Assert.False(name.StartsWith("Second ", StringComparison.Ordinal) && name.EndsWith(" Sect", StringComparison.Ordinal), name);
        }
    }

    [Fact]
    public void The_suffix_follows_the_ruler()
    {
        foreach (var human in new[] { true, false })
            foreach (var size in AllSizes)
            {
                var rng = Rng(0x4b13, (human ? 10 : 0) + (int)size);
                var underHouse = new List<string>();
                var underOther = new List<string>();
                for (var i = 0; i < 10_000; i++)
                {
                    underHouse.Add(RegionNameGenerator.Generate(rng, human, size, RegionNameRuler.House, "Davies"));
                    underOther.Add(RegionNameGenerator.Generate(rng, human, size, RegionNameRuler.Other));
                }
                // "Sultanate" is a middle too and can end a name without a suffix, so it is not asked for.
                foreach (var suffix in RegionNameGenerator.NoHouseSuffixes)
                    Assert.DoesNotContain(underHouse, n => n.EndsWith(suffix, StringComparison.Ordinal));
                foreach (var suffix in RegionNameGenerator.HouseSuffixes.Where(s => s != " Sultanate"))
                    Assert.DoesNotContain(underOther, n => n.EndsWith(suffix, StringComparison.Ordinal));
                Assert.Contains(underHouse, n => n.EndsWith(" Kingdom", StringComparison.Ordinal));
                Assert.Contains(underOther, n => n.EndsWith(human ? " Republic" : " Land", StringComparison.Ordinal));
                // Both give names with a suffix that fits any ruler.
                Assert.Contains(underHouse, n => n.EndsWith(" Nation", StringComparison.Ordinal));
                Assert.Contains(underOther, n => n.EndsWith(" Nation", StringComparison.Ordinal));
            }

        // The two sets are suffixes of the lists, and no suffix is in both.
        var suffixes = RegionNameGenerator.Suffix.Concat(RegionNameGenerator.SuffixHuman).Concat(RegionNameGenerator.SuffixNonhuman)
            .Concat(RegionNameGenerator.SuffixSmall).Select(s => s.Text).ToHashSet();
        Assert.Subset(suffixes, RegionNameGenerator.HouseSuffixes);
        Assert.Subset(suffixes, RegionNameGenerator.NoHouseSuffixes);
        Assert.Empty(RegionNameGenerator.HouseSuffixes.Intersect(RegionNameGenerator.NoHouseSuffixes));
    }

    /// <summary>
    /// The ruling house takes the middle in half of the names with a suffix. Under a house a
    /// human region of middling size has a suffix with the weight 21 of 36, so the house is in
    /// 29 % of the names.
    /// </summary>
    [Fact]
    public void The_ruling_house_takes_the_middle_in_half_of_the_names_with_a_suffix()
    {
        const int count = 20_000;
        var rng = Rng(0x4b14, 0);
        var withHouse = 0;
        for (var i = 0; i < count; i++)
        {
            var name = RegionNameGenerator.Generate(rng, true, RegionNameSize.Middling, RegionNameRuler.House, "Zzyzx");
            if (!name.Contains("Zzyzx")) continue;
            withHouse++;
            // Never without a suffix: the house is followed by one.
            Assert.Contains("Zzyzx ", name);
        }
        Assert.InRange(withHouse / (double)count, 0.27, 0.31);

        // A blank surname is no surname.
        Assert.Equal(
            RegionNameGenerator.Generate(Rng(0x4b14, 1), true, RegionNameSize.Middling),
            RegionNameGenerator.Generate(Rng(0x4b14, 1), true, RegionNameSize.Middling, RegionNameRuler.Any, " "));
    }

    [Fact]
    public void The_seven_human_races_of_the_default_list_are_human()
    {
        var races = ParameterSet.Default.Races;
        Assert.Equal(
            ["northmen", "midlanders", "saltroadFolk", "sunlanders", "grassRiders", "rainkeepers", "canalFolk"],
            races.Where(r => r.Human).Select(r => r.Id));
        Assert.All(races.Take(7), r => Assert.True(r.Human));

        // A race that says nothing is not human, and the flag is read back from a file.
        Assert.False(new RaceParameters("x", "X", true, Everywhere(1)).Human);
        var read = ParameterSet.FromJson(ParameterSet.Default.ToJson());
        Assert.Equal(races.Select(r => r.Human), read.Races.Select(r => r.Human));
    }

    [Fact]
    public void The_default_races_carry_the_name_styles_of_the_appendix()
    {
        var races = ParameterSet.Default.Races;
        string[] Of(NameStyle style) => races.Where(r => r.NameStyle == style).Select(r => r.Id).ToArray();

        Assert.Equal(
            [
                "northmen", "midlanders", "saltroadFolk", "sunlanders", "grassRiders", "rainkeepers", "canalFolk",
                "dwarf", "halfling", "gnome", "hoarkin", "swiftborn", "flinchless", "lanternkin",
            ],
            Of(NameStyle.Common));
        Assert.Equal(
            ["elf", "amberbloods", "noonfolk", "boughwalkers", "shoalfolk", "stillfaces", "twofold", "catfolk"],
            Of(NameStyle.Exotic));
        Assert.Equal(["orc", "hearthbloods", "widegaze", "brinefolk", "duskreaders", "bitterbloods"], Of(NameStyle.Dark));
        // Every human race has the common style.
        Assert.All(races.Where(r => r.Human), r => Assert.Equal(NameStyle.Common, r.NameStyle));

        // A race that says nothing has the common style, and the style is read back from a file, by its name.
        Assert.Equal(NameStyle.Common, new RaceParameters("x", "X", true, Everywhere(1)).NameStyle);
        var json = ParameterSet.Default.ToJson();
        Assert.Contains("\"NameStyle\": \"Dark\"", json);
        Assert.Equal(races.Select(r => r.NameStyle), ParameterSet.FromJson(json).Races.Select(r => r.NameStyle));

        // Every style has its three models.
        foreach (var style in Enum.GetValues<NameStyle>())
            Assert.NotEmpty(NameGenerator.Default.Generate(style, NameKind.Surname, Rng(0x4b15, (int)style)));
    }

    private static Faction Ruling(FactionType type, string race) =>
        new(Faction.RulingSlot, FactionRole.Ruling, type, FactionForm.Crown, race, null, null, [], null, 1);

    [Fact]
    public void A_region_is_named_from_its_ruling_faction_and_its_persons()
    {
        var rules = new RegionNameRules(ParameterSet.Default, 20261120);
        var names = ParameterSet.Default.Planning.Names;
        Assert.Equal(1250, names.SmallRegionPersons);
        Assert.Equal(2500, names.LargeRegionPersons);

        // The size: small below the small limit, large above the large limit.
        Assert.Equal(RegionNameSize.Small, rules.SizeOf(1249));
        Assert.Equal(RegionNameSize.Middling, rules.SizeOf(1250));
        Assert.Equal(RegionNameSize.Middling, rules.SizeOf(2500));
        Assert.Equal(RegionNameSize.Large, rules.SizeOf(2501));

        // A region without a ruling faction has no name.
        Assert.Null(rules.Name(new GridCoord(0, 0), null, 0));

        for (var i = 0; i < 300; i++)
        {
            var region = new GridCoord(i, -i);

            // A house: its surname from the name model of its lead race.
            var human = rules.Name(region, Ruling(FactionType.Family, "midlanders"), 1500)!;
            var elven = rules.Name(region, Ruling(FactionType.Family, "elf"), 1500)!;
            var orcish = rules.Name(region, Ruling(FactionType.Family, "orc"), 1500)!;
            Assert.True(human.Human);
            Assert.False(elven.Human);
            Assert.False(orcish.Human);
            Assert.Equal(rules.RulingHouse(region, NameStyle.Common), human.RulingHouse);
            Assert.Equal(rules.RulingHouse(region, NameStyle.Exotic), elven.RulingHouse);
            Assert.Equal(rules.RulingHouse(region, NameStyle.Dark), orcish.RulingHouse);
            Assert.InRange(human.RulingHouse!.Length, 1, RegionNameRules.HouseNameMaxLength);
            Assert.InRange(elven.RulingHouse!.Length, 1, RegionNameRules.HouseNameMaxLength);
            Assert.InRange(orcish.RulingHouse!.Length, 1, RegionNameRules.HouseNameMaxLength);
            Assert.Equal(RegionNameSize.Middling, human.Size);

            // The style is the race's own: a human race with another style gets a surname of that style.
            var dark = new RegionNameRules(
                ParameterSet.Default with
                {
                    Races = ParameterSet.Default.Races.Select(r => r.Id == "midlanders" ? r with { NameStyle = NameStyle.Dark } : r).ToArray(),
                },
                20261120);
            var darkHuman = dark.Name(region, Ruling(FactionType.Family, "midlanders"), 1500)!;
            Assert.True(darkHuman.Human);
            Assert.Equal(orcish.RulingHouse, darkHuman.RulingHouse);

            // The name is the generator's, from the stream of the region.
            var rng = new Sfc32(Hashing.Hash4(20261120, RegionNameRules.NamePurpose, region.I, region.J));
            Assert.Equal(
                RegionNameGenerator.Generate(rng, true, RegionNameSize.Middling, RegionNameRuler.House, human.RulingHouse),
                human.Name);

            // Any other ruler is no house, and a race that is not in the Parameter Set is not human and has the common style.
            var guilds = rules.Name(region, Ruling(FactionType.Vocation, "nobody"), 4500)!;
            Assert.Null(guilds.RulingHouse);
            Assert.False(guilds.Human);
            Assert.Equal(NameStyle.Common, rules.StyleOf("nobody"));
            Assert.Equal(RegionNameSize.Large, guilds.Size);
        }

        // Another seed gives other names.
        var other = new RegionNameRules(ParameterSet.Default, 7);
        Assert.Contains(Enumerable.Range(0, 20), i =>
            rules.Name(new GridCoord(i, 0), Ruling(FactionType.Race, "orc"), 1500)
            != other.Name(new GridCoord(i, 0), Ruling(FactionType.Race, "orc"), 1500));
    }

    [Fact]
    public void A_planned_region_with_a_ruling_faction_has_a_name()
    {
        var world = World.Create("cellgen");
        foreach (var id in RegionStageRules.RegionsWithin(new RegionId(0, 0), 1))
        {
            var plan = world.Advance(id, RegionStage.Planned).Plan!;
            var work = world.PlanOf(id);
            Assert.Equal(plan.Ruling is null, plan.Name is null);
            Assert.Equal(work.Naming?.Name, plan.Name);
            if (plan.Ruling is not { } ruling) continue;

            Assert.Equal(world.RegionNameRules.Name(id.Coord, ruling, plan.Population.Total), work.Naming);
            Assert.Equal(ruling.Type == FactionType.Family, work.Naming!.RulingHouse is not null);
            Assert.Equal(ParameterSet.Default.Races.Single(r => r.Id == ruling.LeadRace).Human, work.Naming.Human);
        }

        // The name does not depend on the regions generated before it.
        var alone = World.Create("cellgen").Advance(new RegionId(1, 1), RegionStage.Planned).Plan!;
        Assert.Equal(world.Find(new RegionId(1, 1))!.Plan!.Name, alone.Name);
    }
}
