using Gameworld.Domain;
using Gameworld.Generation;
using Gameworld.Generation.Basics;
using Gameworld.Generation.Names;
using static Gameworld.Tests.TestData;

namespace Gameworld.Tests;

/// <summary>The city and town name generator of society-generation.md section 5.3.</summary>
public class SettlementNameTests
{
    private static readonly SiteClass[] AllSites = Enum.GetValues<SiteClass>();

    private static SettlementNameLexicon Lexicon(string race) => SettlementNameLexicons.Of(race)!;

    /// <summary>The first valid names of a stream, each for a city or a town with one more site class.</summary>
    internal static string[] Names(SettlementNameLexicon lexicon, Sfc32 rng, int count)
    {
        var names = new List<string>();
        for (var i = 0; names.Count < count; i++)
        {
            var drawn = SettlementNameGenerator.Draw(rng, lexicon, [i % 3 == 0 ? SiteClass.Fort : SiteClass.Farm, AllSites[i % AllSites.Length]]);
            if (drawn.Valid) names.Add(drawn.Name);
        }
        return names.ToArray();
    }

    /// <summary>
    /// Reference names: they catch a word list that was changed or reordered, and a change in
    /// the order of the draws. Changing them renames the cities and towns of every world.
    /// </summary>
    public static TheoryData<string, string> ReferenceNames => new()
    {
        { "northmen", "Grangard, Jarlhavn, Knarrheim, Grimgard, Skjaldby, Brandbu, Tjarstad, Jarlsjo" },
        { "midlanders", "Gervaseburg, Priorshythe, Westryworth, Yeoburg, Ploughton, Harrowham, Corvenbourg, Bishopspool" },
        { "saltroadFolk", "Ksar Tiharet, Marsa Nujum, Ifaraq, Qasr Mehret, Tin Midad, Bir Timfrar, Qalat Senaq, Birket Kheru" },
        { "sunlanders", "Ile-Kingora, Ziruzabando, Kibidu, Mboyembakro, Ntandela, Sawa, Akabezi, Ogukowala" },
        { "grassRiders", "Doloon Khot, Khadan Boomt, Baga Khoton, Davaan Khot, Mingghan Ordu, Jalgan Ordu, Ulaan Ordu, Khadan Nuur" },
        { "rainkeepers", "Madhyakota, Amritaturai, Chandrahalli, Vijayakota, Bhadrapalli, Sravanagrama, Padmagarh, Amritavapi" },
        { "canalFolk", "Tianhai, Beichuanjin, Wuyin, Lumencheng, Chuanhua, Xiangyuandu, Wanyu, Daishou" },
        { "elf", "Orinrethys, Merethisaer, Aislelle, Aerseys, Melrenne, Liathvovel, Valovel, Navlaquen" },
        { "dwarf", "Tholargrund, Adzemouth, Thrumumrum, Rokhold, Pledgeplate, Hadrim, Morr Dun, Vorbund" },
        { "orc", "Krurnak Wall, Burghar Hold, Last Camp, Marrowfort, Clawpit, Sinewcamp, Old Garrison, Burlgal" },
        { "twofold", "Tiraminimarit, Kovaseresavok, Tirakekekarit, Yoke Door, Coupledhearth, Atidita, Pavisivap, Miradududarim" },
    };

    [Theory]
    [MemberData(nameof(ReferenceNames))]
    public void The_lexicons_give_the_reference_names(string race, string expected) =>
        Assert.Equal(expected, string.Join(", ", Names(Lexicon(race), Rng(SettlementNameRules.NamePurpose, 1), 8)));

    [Fact]
    public void A_surname_model_gives_the_reference_names_of_a_race_without_a_lexicon()
    {
        Assert.Equal("Tanlonborough, Goodby, Ashtonstrand, Drightchester, Mahonymeet, Storthorp", string.Join(", ", Names(SettlementNameLexicons.OfStyle(NameStyle.Common), Rng(SettlementNameRules.NamePurpose, 1), 6)));
        Assert.Equal("Waelvorborough, Tyrport, Firworth, Bravehithe, Moonbrynmeet, Vendoveford", string.Join(", ", Names(SettlementNameLexicons.OfStyle(NameStyle.Exotic), Rng(SettlementNameRules.NamePurpose, 1), 6)));
        Assert.Equal("Zahahiborough, Favruhby, Fireton, Crulahichester, Favahiton, Levpionestead", string.Join(", ", Names(SettlementNameLexicons.OfStyle(NameStyle.Dark), Rng(SettlementNameRules.NamePurpose, 1), 6)));
    }

    [Fact]
    public void Every_default_race_has_a_lexicon_of_its_own()
    {
        Assert.Equal(
            ParameterSet.Default.Races.Select(r => r.Id).Order(StringComparer.Ordinal),
            SettlementNameLexicons.Races.Order(StringComparer.Ordinal));
        Assert.Null(SettlementNameLexicons.Of("nobody"));
    }

    [Fact]
    public void Every_tongue_is_sound()
    {
        static bool Plain(string text) => text.All(c => c is (>= 'A' and <= 'Z') or (>= 'a' and <= 'z') or ' ' or '-' or '\'');

        var lexicons = SettlementNameLexicons.Races.Select(race => (Name: race, Lexicon: Lexicon(race)))
            .Concat(Enum.GetValues<NameStyle>().Select(style => (Name: style.ToString(), Lexicon: SettlementNameLexicons.OfStyle(style))));
        foreach (var (name, lexicon) in lexicons)
        {
            Assert.NotEmpty(lexicon.Tongues);
            foreach (var tongue in lexicon.Tongues)
            {
                Assert.True(tongue.Weight > 0, name);
                Assert.True(tongue.Heads.Length > 0 || tongue.Coins, $"{name}: a tongue without first parts.");
                Assert.InRange(tongue.CoinedShare, 0, 1);
                Assert.InRange(tongue.BareShare, 0, 1);

                // The listed first parts: plain letters, a capital first, none twice.
                Assert.Equal(tongue.Heads.Length, tongue.Heads.Distinct(StringComparer.OrdinalIgnoreCase).Count());
                foreach (var head in tongue.Heads)
                {
                    Assert.True(Plain(head) && !head.Contains(' '), $"{name}: {head}");
                    Assert.True(char.IsUpper(head[0]), $"{name}: {head}");
                }

                if (tongue.Syllables is { } syllables)
                {
                    Assert.NotEmpty(syllables.First);
                    Assert.InRange(syllables.MinMiddle, 0, syllables.MaxMiddle);
                    Assert.True(syllables.MaxMiddle == 0 || syllables.Middle.Length > 0, name);
                    Assert.True(!syllables.Mirror || syllables.Last.Length > 0, name);
                    Assert.All(syllables.First.Concat(syllables.Middle).Concat(syllables.Last), s => Assert.True(Plain(s) && !s.Contains(' '), $"{name}: {s}"));
                    Assert.All(syllables.First, s => Assert.True(char.IsUpper(s[0]), $"{name}: {s}"));
                }

                // A tongue with place words has them for a city and for a town, so that every site has one.
                if (tongue.BareShare < 1)
                {
                    Assert.Contains(SiteClass.Fort, tongue.Words.Keys);
                    Assert.Contains(SiteClass.Farm, tongue.Words.Keys);
                }
                foreach (var (site, words) in tongue.Words)
                {
                    Assert.True(words.Length > 0, $"{name}: no word for {site}.");
                    Assert.Equal(words.Length, words.Distinct(StringComparer.Ordinal).Count());
                    foreach (var word in words)
                    {
                        Assert.True(Plain(word) && word.Trim(' ', '-').Length > 0, $"{name}: '{word}'");
                        // A word in front starts with a capital, and a joined one does not.
                        if (word.EndsWith(' ') || word.EndsWith('-')) Assert.True(char.IsUpper(word[0]), $"{name}: '{word}'");
                        else if (!word.StartsWith(' ') && !word.StartsWith('-')) Assert.True(char.IsLower(word[0]), $"{name}: '{word}'");
                    }
                }
            }
        }
    }

    [Fact]
    public void Valid_names_keep_the_limits_and_use_plain_letters()
    {
        var stream = 0;
        var lexicons = SettlementNameLexicons.Races.Select(Lexicon)
            .Concat(Enum.GetValues<NameStyle>().Select(SettlementNameLexicons.OfStyle));
        foreach (var lexicon in lexicons)
        {
            var rng = Rng(0x4c11, stream++);
            var distinct = new HashSet<string>();
            var valid = 0;
            for (var i = 0; i < 4000; i++)
            {
                SiteClass[] sites = [i % 3 == 0 ? SiteClass.Fort : SiteClass.Farm, AllSites[i % AllSites.Length], AllSites[i / 7 % AllSites.Length]];
                var drawn = SettlementNameGenerator.Draw(rng, lexicon, sites);
                Assert.True(drawn.Site is null || sites.Contains(drawn.Site.Value), drawn.Name);
                if (!drawn.Valid) continue;
                valid++;
                var name = drawn.Name;
                Assert.InRange(name.Length, SettlementNameGenerator.MinLength, SettlementNameGenerator.MaxLength);
                Assert.Equal(name.Trim(), name);
                Assert.DoesNotContain("  ", name);
                Assert.True(char.IsUpper(name[0]), $"The name {name} does not start with a capital letter.");
                Assert.All(name, c => Assert.True(c is (>= 'A' and <= 'Z') or (>= 'a' and <= 'z') or ' ' or '-' or '\'', name));
                distinct.Add(name);
            }
            // Few draws are drawn again, and a lexicon gives far more names than a region needs.
            Assert.True(valid > 3200, $"Only {valid} of 4000 draws are valid.");
            Assert.True(distinct.Count > 500, $"Only {distinct.Count} distinct names.");
        }
    }

    [Fact]
    public void The_first_part_and_the_place_word_are_joined_by_the_form_of_the_word()
    {
        Assert.Equal("Hrafnvik", SettlementNameGenerator.Join("Hrafn", "vik", elide: true));
        Assert.Equal("Khar Ordu", SettlementNameGenerator.Join("Khar", " Ordu", elide: false));
        Assert.Equal("Bir Sallah", SettlementNameGenerator.Join("Sallah", "Bir ", elide: true));
        Assert.Equal("Ile-Odunmare", SettlementNameGenerator.Join("Odunmare", "Ile-", elide: true));
        Assert.Equal("Bobbinwick-under-Oak", SettlementNameGenerator.Join("Bobbinwick", "-under-Oak", elide: false));
        Assert.Equal("Marshal's Field", SettlementNameGenerator.Join("Marshal's", "field", elide: false));
        Assert.Equal("Strangers' Door", SettlementNameGenerator.Join("Strangers'", " Door", elide: false));

        // A worn tongue writes a doubled letter once and drops a vowel before a vowel.
        Assert.Equal("Ulfell", SettlementNameGenerator.Join("Ulf", "fell", elide: true));
        Assert.Equal("Jalalayam", SettlementNameGenerator.Join("Jala", "alayam", elide: true));
        Assert.Equal("Jaluppalam", SettlementNameGenerator.Join("Jala", "uppalam", elide: true));
        Assert.Equal("Rafttown", SettlementNameGenerator.Join("Raft", "town", elide: false));

        // What makes a name one to draw again.
        Assert.True(SettlementNameGenerator.Doubles("Raft", "town"));
        Assert.False(SettlementNameGenerator.Doubles("Raft", " Town"));
        Assert.False(SettlementNameGenerator.Doubles("Raft", "holm"));
        Assert.True(SettlementNameGenerator.EndsIn("Reef", "reef"));
        Assert.True(SettlementNameGenerator.EndsIn("Hollow", " Hollow"));
        Assert.False(SettlementNameGenerator.EndsIn("Reef", "hold"));
        Assert.True(SettlementNameGenerator.Repeats("Yuyu"));
        Assert.False(SettlementNameGenerator.Repeats("Yuyan"));
    }

    [Fact]
    public void No_valid_name_doubles_its_place_word_or_a_letter_at_the_seam()
    {
        // The Midlanders keep their seams and have "Dun" beside "don" and "Mill" beside "lea".
        var tongue = Lexicon("midlanders").Tongues[0];
        var rng = Rng(0x4c12, 0);
        for (var i = 0; i < 20_000; i++)
        {
            var drawn = SettlementNameGenerator.Draw(rng, Lexicon("midlanders"), AllSites);
            if (!drawn.Valid) continue;
            var head = tongue.Heads.Where(h => drawn.Name.Contains(h, StringComparison.Ordinal)).MaxBy(h => h.Length)!;
            var word = drawn.Name.Replace(head, "").Trim();
            Assert.False(head.EndsWith(word, StringComparison.OrdinalIgnoreCase), drawn.Name);
            if (!drawn.Name.Contains(' '))
                Assert.NotEqual(char.ToLowerInvariant(head[^1]), char.ToLowerInvariant(word[0]));
        }
    }

    [Fact]
    public void The_own_names_of_the_Twofold_read_the_same_both_ways()
    {
        var own = new SettlementNameLexicon([Lexicon("twofold").Tongues[0]]);
        var rng = Rng(0x4c13, 0);
        var distinct = new HashSet<string>();
        for (var i = 0; i < 2000; i++)
        {
            var drawn = SettlementNameGenerator.Draw(rng, own, AllSites);
            Assert.Null(drawn.Site);
            var letters = drawn.Name.ToLowerInvariant();
            Assert.Equal(letters, new string(letters.Reverse().ToArray()));
            distinct.Add(drawn.Name);
        }
        Assert.True(distinct.Count > 500, $"Only {distinct.Count} distinct names.");
    }

    [Fact]
    public void The_site_gives_the_class_of_the_place_word()
    {
        var northmen = Lexicon("northmen");
        var tongue = northmen.Tongues[0];

        // A town with nothing else to say is a homestead, and a city a fort.
        var rng = Rng(0x4c14, 0);
        for (var i = 0; i < 500; i++)
        {
            var town = SettlementNameGenerator.Draw(rng, northmen, [SiteClass.Farm]);
            Assert.Equal(SiteClass.Farm, town.Site);
            if (town.Valid) Assert.Contains(tongue.Words[SiteClass.Farm], word => town.Name.EndsWith(word[^1..], StringComparison.Ordinal));
            Assert.Equal(SiteClass.Fort, SettlementNameGenerator.Draw(rng, northmen, [SiteClass.Fort]).Site);
        }

        // A town with a harbour is named for the harbour in 5 cases of 9: the weights are 5 and 4.
        var haven = 0;
        for (var i = 0; i < 9000; i++)
            if (SettlementNameGenerator.Draw(rng, northmen, [SiteClass.Farm, SiteClass.Haven]).Site == SiteClass.Haven) haven++;
        Assert.InRange(haven, 4700, 5300);

        // A class the tongue has no word for is never drawn: the Northmen have none for salt.
        Assert.DoesNotContain(SiteClass.Salt, tongue.Words.Keys);
        for (var i = 0; i < 200; i++)
            Assert.Equal(SiteClass.Farm, SettlementNameGenerator.Draw(rng, northmen, [SiteClass.Salt, SiteClass.Farm]).Site);
    }

    [Fact]
    public void The_facts_of_a_Prospect_give_its_site_classes()
    {
        var none = new ProspectFacts(false, false, 0, 0, 0, false, 0, 3);
        var amounts = new ResourceAmount[ResourceCatalog.Count];
        var bare = new PatchResources(amounts, 0, 0);
        SiteClass[] Sites(
            ProspectFacts facts, ElevationType zone = ElevationType.Lowland, Biome biome = Biome.Volcano, PatchResources? resources = null,
            double trade = 0, bool city = false, params FactionType[] seated) =>
            SettlementNameRules.SitesOf(facts, zone, biome, resources ?? bare, trade, city, seated);

        // Nothing to say: a town is a homestead and a city a fort.
        Assert.Equal([SiteClass.Farm], Sites(none));
        Assert.Equal([SiteClass.Fort], Sites(none, city: true));

        // The water.
        Assert.Equal([SiteClass.Haven, SiteClass.Farm], Sites(none with { Coast = true, Harbour = true }));
        Assert.Equal([SiteClass.Shore, SiteClass.Farm], Sites(none with { Coast = true }));
        Assert.Equal([SiteClass.Farm], Sites(none with { River = 1.9 }));
        Assert.Equal([SiteClass.Ford, SiteClass.Farm], Sites(none with { River = 2 }));
        Assert.Equal([SiteClass.Ford, SiteClass.Farm], Sites(none with { River = 7.9 }));
        Assert.Equal([SiteClass.Bridge, SiteClass.Farm], Sites(none with { River = 8 }));
        Assert.Equal([SiteClass.Farm], Sites(none with { River = 7.9, Mouth = 7.9 }).Except([SiteClass.Ford]));
        Assert.Equal(
            [SiteClass.Mouth, SiteClass.Shore, SiteClass.Landing, SiteClass.Meeting, SiteClass.Bridge, SiteClass.Mere, SiteClass.Farm],
            Sites(new ProspectFacts(true, false, 30, 30, 4, true, 2e6, 3)));
        Assert.DoesNotContain(SiteClass.Meeting, Sites(none with { Confluence = 3.9 }));

        // The land.
        Assert.Equal([SiteClass.Hill, SiteClass.Farm], Sites(none, ElevationType.Hills));
        Assert.Equal([SiteClass.Hill, SiteClass.Farm], Sites(none, ElevationType.Mountains));
        Assert.Equal([SiteClass.Farm], Sites(none, ElevationType.FlatLand));
        foreach (var biome in Enum.GetValues<Biome>())
        {
            var sites = Sites(none, biome: biome);
            var expected = biome switch
            {
                Biome.Volcano => (SiteClass?)null,
                Biome.ColdSteppe or Biome.TemperateGrassland or Biome.Savanna => SiteClass.Field,
                Biome.IceAndSnow or Biome.Tundra or Biome.ColdDesert or Biome.HotDesert or Biome.ThornScrub => SiteClass.Waste,
                _ => SiteClass.Wood,
            };
            Assert.Equal(expected is { } site ? [site, SiteClass.Farm] : [SiteClass.Farm], sites);
        }

        // What the patch holds: salt that is listed, and a deposit of ore, gold or gems.
        PatchResources With(Resource resource, double amount, ResourceOrigin origin)
        {
            var held = new ResourceAmount[ResourceCatalog.Count];
            held[(int)resource] = amounts[(int)resource] with { Amount = amount, Origin = origin };
            return new PatchResources(held, 0, 0);
        }
        Assert.Equal([SiteClass.Salt, SiteClass.Farm], Sites(none, resources: With(Resource.Salt, 0.25, ResourceOrigin.Potential)));
        Assert.Equal([SiteClass.Farm], Sites(none, resources: With(Resource.Salt, 0.24, ResourceOrigin.Potential)));
        Assert.Equal([SiteClass.Delve, SiteClass.Farm], Sites(none, resources: With(Resource.Gold, 0.5, ResourceOrigin.Deposit)));
        Assert.Equal([SiteClass.Farm], Sites(none, resources: With(Resource.Gold, 0.5, ResourceOrigin.Potential)));
        Assert.Equal([SiteClass.Farm], Sites(none, resources: With(Resource.Stone, 0.9, ResourceOrigin.Deposit)));

        // The factions that sit there, and the trade.
        Assert.Equal([SiteClass.Shrine, SiteClass.Fort], Sites(none, city: true, seated: [FactionType.Family, FactionType.Belief]));
        Assert.Equal([SiteClass.Market, SiteClass.Farm], Sites(none, seated: FactionType.Vocation));
        Assert.Equal([SiteClass.Market, SiteClass.Farm], Sites(none, seated: FactionType.Place));
        Assert.Equal([SiteClass.Market, SiteClass.Farm], Sites(none, trade: SettlementNameRules.MarketTrade));
        Assert.Equal([SiteClass.Farm], Sites(none, trade: SettlementNameRules.MarketTrade - 0.01, seated: [FactionType.Race, FactionType.Magic]));
    }

    [Fact]
    public void A_race_without_a_lexicon_takes_that_of_its_name_style()
    {
        var parameters = ParameterSet.Default with
        {
            Races = [.. ParameterSet.Default.Races, ParameterSet.Default.Races[0] with { Id = "newcomers", NameStyle = NameStyle.Dark }],
        };
        var rules = new SettlementNameRules(parameters, 7);
        Assert.Same(SettlementNameLexicons.Of("elf"), rules.LexiconOf("elf"));
        Assert.Same(SettlementNameLexicons.OfStyle(NameStyle.Dark), rules.LexiconOf("newcomers"));
        Assert.Same(SettlementNameLexicons.OfStyle(NameStyle.Common), rules.LexiconOf("nobody"));
        Assert.Same(SettlementNameLexicons.OfStyle(NameStyle.Common), rules.LexiconOf(null));

        // Its first part is a surname of at most eight letters, and its place words are those of the trade tongue.
        var rng = Rng(0x4c15, 0);
        string[] words = ["ton", "ham", "by", "thorp", "stead", "worth"];
        for (var i = 0; i < 500; i++)
        {
            var drawn = SettlementNameGenerator.Draw(rng, rules.LexiconOf("newcomers"), [SiteClass.Farm]);
            if (drawn.Valid) Assert.Contains(words, word => drawn.Name.EndsWith(word[1..], StringComparison.Ordinal));
        }
    }

    [Fact]
    public void The_race_that_names_a_place_is_drawn_by_its_weight_there()
    {
        var prospect = new NamedProspect(new GridCoord(0, 0), ["a", "b", "c"], [6, 3, 1], [SiteClass.Farm]);
        Assert.Equal("a", SettlementNameRules.NamerOf(prospect, 0));
        Assert.Equal("a", SettlementNameRules.NamerOf(prospect, 0.599));
        Assert.Equal("b", SettlementNameRules.NamerOf(prospect, 0.6));
        Assert.Equal("b", SettlementNameRules.NamerOf(prospect, 0.899));
        Assert.Equal("c", SettlementNameRules.NamerOf(prospect, 0.9));
        Assert.Equal("c", SettlementNameRules.NamerOf(prospect, 0.9999999));

        // Without a weight the first race names the place, and a Prospect without a race has no race to name it.
        Assert.Equal("a", SettlementNameRules.NamerOf(prospect with { Weights = [0, 0, 0] }, 0.7));
        Assert.Equal("a", SettlementNameRules.NamerOf(prospect with { Weights = [] }, 0.7));
        Assert.Equal("b", SettlementNameRules.NamerOf(prospect with { Weights = [-1, 2, double.NaN] }, 0.7));
        Assert.Null(SettlementNameRules.NamerOf(prospect with { Races = [], Weights = [] }, 0.7));
    }

    [Fact]
    public void No_two_Prospects_of_a_region_get_the_same_name()
    {
        var rules = new SettlementNameRules(ParameterSet.Default, 20261120);

        // Twenty towns of one race with nothing to tell them apart.
        var towns = Enumerable.Range(0, 20)
            .Select(i => new NamedProspect(new GridCoord(i, 3), ["halfling"], [1], [SiteClass.Farm])).ToArray();
        var names = rules.Name(towns);
        Assert.Equal(20, names.Select(n => n.Name).Distinct().Count());
        Assert.All(names, n => Assert.Equal(("halfling", SiteClass.Farm), (n.Race, n.Site)));

        // The same patch twice has the same stream: the second one draws on until its name is free.
        var twins = rules.Name([towns[0], towns[0], towns[0]]);
        Assert.Equal(names[0], twins[0]);
        Assert.Equal(3, twins.Select(n => n.Name).Distinct().Count());

        // The name of a Prospect is that of its stream: the race first, then the name.
        var mixed = new NamedProspect(new GridCoord(5, -2), ["northmen", "orc"], [1, 1], [SiteClass.Haven, SiteClass.Farm]);
        var rng = new Sfc32(Hashing.Hash4(20261120, SettlementNameRules.NamePurpose, 5, -2));
        var race = SettlementNameRules.NamerOf(mixed, rng.Next());
        var drawn = SettlementNameGenerator.Draw(rng, Lexicon(race!), mixed.Sites);
        while (!drawn.Valid) drawn = SettlementNameGenerator.Draw(rng, Lexicon(race!), mixed.Sites);
        Assert.Equal(new SettlementNaming(drawn.Name, race, drawn.Site), rules.Name([mixed])[0]);

        // Another seed gives other names.
        var other = new SettlementNameRules(ParameterSet.Default, 7);
        Assert.NotEqual(names.Select(n => n.Name), other.Name(towns).Select(n => n.Name));
    }

    [Fact]
    public void A_planned_region_names_a_city_or_a_town_on_every_Prospect()
    {
        var world = World.Create("cellgen");
        foreach (var id in RegionStageRules.RegionsWithin(new RegionId(0, 0), 1))
        {
            var region = world.Advance(id, RegionStage.Settled);
            var plan = region.Plan!;
            var work = world.PlanOf(id);

            Assert.Equal(plan.Prospects.Count, plan.ProspectNames.Count);
            Assert.Equal(work.Names.Select(n => n.Name), plan.ProspectNames);
            Assert.Equal(plan.ProspectNames.Count, plan.ProspectNames.Distinct(StringComparer.OrdinalIgnoreCase).Count());
            Assert.All(plan.ProspectNames, name => Assert.InRange(name.Length, SettlementNameGenerator.MinLength, SettlementNameGenerator.MaxLength));

            for (var slot = 0; slot < plan.Prospects.Count; slot++)
            {
                var naming = work.Names[slot];
                var prospect = plan.Prospects[slot];
                var capital = plan.Ruling?.Seat == prospect.Patch;

                // The capital is named by the lead race of the ruling faction, any other place by one of its races.
                if (capital) Assert.Equal(plan.Ruling!.LeadRace, naming.Race);
                else if (naming.Race is not null) Assert.Contains(naming.Race, plan.ProspectRaces[slot]);

                // The place word is one of the site: a city is no homestead and a town no fort.
                Assert.NotEqual(capital || prospect.ForCity ? SiteClass.Farm : SiteClass.Fort, naming.Site);
                if (naming.Site == SiteClass.Haven) Assert.True(prospect.Facts.Harbour);
                if (naming.Site == SiteClass.Mouth) Assert.True(prospect.Facts.Mouth >= SettlementNameRules.NavigableRiver);
            }

            // A city or a town has the name of its Prospect.
            Assert.All(region.Settlement!.Settlements, s => Assert.Equal(plan.ProspectNames[s.Slot], s.Name));
        }

        // The names do not depend on the regions generated before them.
        var alone = World.Create("cellgen").Advance(new RegionId(1, 1), RegionStage.Planned).Plan!;
        Assert.Equal(world.Find(new RegionId(1, 1))!.Plan!.ProspectNames, alone.ProspectNames);
    }
}
