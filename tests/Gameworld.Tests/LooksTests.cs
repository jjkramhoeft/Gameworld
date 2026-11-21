using System.Text.RegularExpressions;
using Gameworld.Domain;
using Gameworld.Generation;
using Gameworld.Generation.People;

namespace Gameworld.Tests;

/// <summary>The portrait components and the looks of a person: society-generation.md section 9.</summary>
public class LooksTests
{
    private static readonly Sex[] Sexes = [Sex.Female, Sex.Male];

    private static Wearer Adult(string race, Sex sex = Sex.Female, int age = 30, WealthLevel wealth = WealthLevel.Common, bool married = false, string? vocation = null) =>
        new(race, sex, age, wealth, married, vocation);

    [Fact]
    public void The_default_lists_hold_the_portrait_components()
    {
        var p = ParameterSet.Default;
        Assert.Equal((32, 27, 23), (p.SkinColours.Count, p.EyeColours.Count, p.HairColours.Count));
        Assert.Equal((46, 72, 51), (p.Hairstyles.Count, p.Outfits.Count, p.Accessories.Count));

        // An id is unique in its list, and every entry is on.
        foreach (var ids in new[]
                 {
                     p.SkinColours.Select(c => c.Id), p.EyeColours.Select(c => c.Id), p.HairColours.Select(c => c.Id),
                     p.Hairstyles.Select(h => h.Id), p.Outfits.Select(o => o.Id), p.Accessories.Select(a => a.Id),
                 })
        {
            var list = ids.ToArray();
            Assert.Equal(list.Length, list.Distinct().Count());
            Assert.All(list, id => Assert.Matches("^[a-z][A-Za-z]*$", id));
        }
        Assert.All(p.Hairstyles, h => Assert.True(h.Enabled, h.Id));
        Assert.All(p.Outfits, o => Assert.True(o.Enabled, o.Id));
        Assert.All(p.Accessories, a => Assert.True(a.Enabled, a.Id));

        // Every default entry names the races that wear it, and every kind of accessory is in the list.
        Assert.All(p.Hairstyles, h => Assert.NotEmpty(h.WornBy.Races));
        Assert.All(p.Outfits, o => Assert.NotEmpty(o.WornBy.Races));
        Assert.All(p.Accessories, a => Assert.NotEmpty(a.WornBy.Races));
        Assert.Equal(Enum.GetValues<AccessoryKind>(), p.Accessories.Select(a => a.Kind).Distinct().Order());

        // The lists are read back from a file.
        var read = ParameterSet.FromJson(p.ToJson());
        Assert.Equal(p.SkinColours, read.SkinColours);
        Assert.Equal(p.EyeColours, read.EyeColours);
        Assert.Equal(p.HairColours, read.HairColours);
        Assert.Equal(p.Hairstyles.Select(Flat), read.Hairstyles.Select(Flat));
        Assert.Equal(p.Outfits.Select(o => (o.Id, o.Name, o.MainColour, o.AccentColour, o.Description, o.Lore, Flat(o.WornBy))),
            read.Outfits.Select(o => (o.Id, o.Name, o.MainColour, o.AccentColour, o.Description, o.Lore, Flat(o.WornBy))));
        Assert.Equal(p.Accessories.Select(a => (a.Id, a.Name, a.Kind, a.Description, a.Lore, Flat(a.WornBy))),
            read.Accessories.Select(a => (a.Id, a.Name, a.Kind, a.Description, a.Lore, Flat(a.WornBy))));

        // The world of the defaults has the rules.
        Assert.NotEmpty(World.Create("looks").LooksRules.SkinColoursOf("elf"));
    }

    private static string Flat(HairstyleParameters h) => $"{h.Id}|{h.Name}|{h.Enabled}|{h.Description}|{h.Lore}|{Flat(h.WornBy)}";

    private static string Flat(WearerParameters w) =>
        $"{w.Sex}|{string.Join(",", w.Races)}|{w.LowestWealth}-{w.HighestWealth}|{string.Join(",", w.Vocations)}|{w.LowestAge}-{w.HighestAge}|{w.Married}";

    [Fact]
    public void The_default_lists_follow_the_research_note()
    {
        // Every row of the tables of docs/research/portrait-components.md is an entry, in the order
        // of the note and with its name, apart from the rows that no race of the default list uses.
        string[] leftOut =
        [
            "SC20", "SC30", "SC31", "HC22", "HC23", "EC12", "EC20", "EC22", "EC23",
            "RH16", "RO9", "RO18", "RO19", "RO20", "WO24", "AC16", "AC47", "AC53",
        ];
        var rows = File.ReadAllLines(Path.Combine(DomainTests.RepoRoot(), "docs", "research", "portrait-components.md"))
            .Select(line => Regex.Match(line, @"^\| ([A-Z]{2})(\d+) \| ([^|]+) \|"))
            .Where(m => m.Success && !leftOut.Contains(m.Groups[1].Value + m.Groups[2].Value))
            .Select(m => (Prefix: m.Groups[1].Value, Name: m.Groups[3].Value.Trim()))
            .ToArray();
        string[] Names(params string[] prefixes) =>
            prefixes.SelectMany(prefix => rows.Where(r => r.Prefix == prefix).Select(r => r.Name.ToLowerInvariant())).ToArray();

        var p = ParameterSet.Default;
        Assert.Equal(Names("SC"), p.SkinColours.Select(c => c.Name.ToLowerInvariant()));
        Assert.Equal(Names("EC"), p.EyeColours.Select(c => c.Name.ToLowerInvariant()));
        Assert.Equal(Names("HC"), p.HairColours.Select(c => c.Name.ToLowerInvariant()));
        Assert.Equal(Names("WH", "MH", "RH"), p.Hairstyles.Select(h => h.Name.ToLowerInvariant()));
        Assert.Equal(Names("WO", "MO", "RO"), p.Outfits.Select(o => o.Name.ToLowerInvariant()));
        Assert.Equal(Names("AC"), p.Accessories.Select(a => a.Name.ToLowerInvariant()));
    }

    [Fact]
    public void Every_default_race_has_its_colours()
    {
        var p = ParameterSet.Default;
        var rules = new LooksRules(p);
        foreach (var race in p.Races)
        {
            Assert.NotEmpty(rules.SkinColoursOf(race.Id));
            Assert.NotEmpty(rules.EyeColoursOf(race.Id));
            Assert.Equal(race.SkinColours, rules.SkinColoursOf(race.Id).Select(c => c.Id));
            Assert.Equal(race.EyeColours, rules.EyeColoursOf(race.Id).Select(c => c.Id));
            Assert.Equal(race.HairColours, rules.HairColoursOf(race.Id).Select(c => c.Id));
            // The Noonfolk have no hair at all; every other race has a hair colour.
            Assert.Equal(race.Id == "noonfolk", rules.HairColoursOf(race.Id).Count == 0);
        }
        Assert.Equal(["frostPale"], p.Races.Single(r => r.Id == "hoarkin").SkinColours);
        Assert.Equal(["silverGrey"], p.Races.Single(r => r.Id == "flinchless").EyeColours);
        Assert.Equal(12, p.Races.Single(r => r.Id == "flinchless").SkinColours.Count);

        // Every colour of the three lists is the colour of some race.
        Assert.Equal(p.SkinColours.Select(c => c.Id).Order(), p.Races.SelectMany(r => r.SkinColours).Distinct().Order());
        Assert.Equal(p.EyeColours.Select(c => c.Id).Order(), p.Races.SelectMany(r => r.EyeColours).Distinct().Order());
        // A race lists the hair before it greys: the four colours of age are reached by greying only.
        string[] ofAge = ["greyingDark", "greyingFair", "ironGrey", "white"];
        Assert.Equal(p.HairColours.Select(c => c.Id).Except(ofAge).Order(), p.Races.SelectMany(r => r.HairColours).Distinct().Order());

        // Black greys to greying dark, to iron grey, to white, and there it ends.
        Assert.Equal("greyingDark", rules.GreyingOf("black")!.Id);
        Assert.Equal("ironGrey", rules.GreyingOf("greyingDark")!.Id);
        Assert.Equal("white", rules.GreyingOf("ironGrey")!.Id);
        Assert.Equal("white", rules.GreyingOf("greyingFair")!.Id);
        Assert.Null(rules.GreyingOf("white"));
        Assert.Null(rules.GreyingOf("silverWhite"));
        Assert.Null(rules.GreyingOf("no such colour"));

        // A race that is not in the list has no colours.
        Assert.Empty(rules.SkinColoursOf("goblin"));
        Assert.Empty(rules.EyeColoursOf("goblin"));
        Assert.Empty(rules.HairColoursOf("goblin"));
    }

    [Fact]
    public void Every_default_race_has_a_hairstyle_and_an_outfit_for_each_sex_and_wealth()
    {
        var p = ParameterSet.Default;
        var rules = new LooksRules(p);
        foreach (var race in p.Races)
            foreach (var sex in Sexes)
                foreach (var wealth in Enum.GetValues<WealthLevel>())
                    foreach (var age in new[] { 18, 30, 45, 70 })
                        foreach (var married in new[] { false, true })
                        {
                            var wearer = Adult(race.Id, sex, age, wealth, married);
                            Assert.True(rules.HairstylesOf(wearer).Count > 0, $"No hairstyle for {wearer}.");
                            Assert.True(rules.OutfitsOf(wearer).Count > 0, $"No outfit for {wearer}.");
                        }

        // Every default hairstyle, outfit and accessory is worn by somebody.
        var wearers = (
            from race in p.Races
            from sex in Sexes
            from wealth in Enum.GetValues<WealthLevel>()
            from age in new[] { 20, 50 }
            from married in new[] { false, true }
            from vocation in p.Vocations.Select(v => (string?)v.Id).Append(null)
            select new Wearer(race.Id, sex, age, wealth, married, vocation)).ToArray();
        Assert.Equal(p.Hairstyles.Select(h => h.Id), p.Hairstyles.Where(h => wearers.Any(w => LooksRules.Fits(h.WornBy, w))).Select(h => h.Id));
        Assert.Equal(p.Outfits.Select(o => o.Id), p.Outfits.Where(o => wearers.Any(w => LooksRules.Fits(o.WornBy, w))).Select(o => o.Id));
        Assert.Equal(p.Accessories.Select(a => a.Id), p.Accessories.Where(a => wearers.Any(w => LooksRules.Fits(a.WornBy, w))).Select(a => a.Id));
    }

    [Fact]
    public void The_defaults_dress_a_person_by_race_sex_wealth_vocation_age_and_marriage()
    {
        var rules = new LooksRules(ParameterSet.Default);
        string[] Hair(Wearer w) => rules.HairstylesOf(w).Select(h => h.Id).ToArray();
        string[] Dress(Wearer w) => rules.OutfitsOf(w).Select(o => o.Id).ToArray();
        string[] Jewels(Wearer w) => rules.AccessoriesOf(w).Select(a => a.Id).ToArray();

        // A race with one dress wears it whatever the sex and the wealth.
        Assert.Equal(["singleSealskin"], Dress(Adult("hoarkin", Sex.Male, wealth: WealthLevel.Rich)));
        Assert.Equal(["ringedBeardBraids"], Hair(Adult("dwarf")));
        Assert.Equal(["hairless"], Hair(Adult("noonfolk", Sex.Male)));

        // The wealth: a Midlander woman from rags to the surcoat.
        Assert.Equal(["beggarsRags"], Dress(Adult("midlanders", wealth: WealthLevel.Destitute)));
        Assert.Equal(["kirtleAndApron"], Dress(Adult("midlanders", wealth: WealthLevel.Poor)));
        Assert.Equal(["kirtleAndApron", "guildCutTownDress"], Dress(Adult("midlanders")));
        Assert.Equal(["sidelessSurcoat", "highBeltedGown"], Dress(Adult("midlanders", wealth: WealthLevel.Rich)));

        // The vocation: only a smith wears the apron, a Dwarf smith too.
        Assert.Contains("smithsLeatherApron", Dress(Adult("midlanders", Sex.Male, vocation: "smith")));
        Assert.DoesNotContain("smithsLeatherApron", Dress(Adult("midlanders", Sex.Male, vocation: "farmer")));
        Assert.Equal(["smithsLeatherApron", "thickWoolAndToolBelt"], Dress(Adult("dwarf", vocation: "smith")));
        Assert.Contains("tonsure", Hair(Adult("midlanders", Sex.Male, vocation: "priest")));
        Assert.DoesNotContain("tonsure", Hair(Adult("midlanders", Sex.Male)));

        // The age: the sash of a Twofold changes at the Turning.
        Assert.Equal(["wrappedCottonSaffronSash"], Dress(Adult("twofold", age: 29)));
        Assert.Equal(["wrappedCottonIndigoSash"], Dress(Adult("twofold", age: 30)));

        // The marriage: a wife covers her hair, and wears the ring.
        Assert.Contains("veilAndWimple", Hair(Adult("midlanders", married: true)));
        Assert.DoesNotContain("veilAndWimple", Hair(Adult("midlanders")));
        Assert.Contains("looseWithCirclet", Hair(Adult("midlanders", age: 20)));
        Assert.DoesNotContain("looseWithCirclet", Hair(Adult("midlanders", age: 20, married: true)));
        Assert.Contains("claspedHandsRing", Jewels(Adult("midlanders", married: true)));
        Assert.DoesNotContain("claspedHandsRing", Jewels(Adult("midlanders")));

        // The sex and the race.
        Assert.Contains("amberAndGlassBeadString", Jewels(Adult("northmen")));
        Assert.DoesNotContain("amberAndGlassBeadString", Jewels(Adult("northmen", Sex.Male)));
        Assert.DoesNotContain("amberAndGlassBeadString", Jewels(Adult("amberbloods")));
        Assert.Empty(Jewels(Adult("duskreaders", wealth: WealthLevel.Rich)));

        // The person of a household.
        var household = new Household(0, 0, null, "midlanders", WealthLevel.Rich, "Alder", Sex.Male, 41, 2, []);
        var head = new Person(0, 0, "Edric", "Alder", "midlanders", Sex.Male, 41, HouseholdRole.Head, "noble", Spouse: 1);
        Assert.Equal(new Wearer("midlanders", Sex.Male, 41, WealthLevel.Rich, true, "noble"), Wearer.Of(head, household));
        Assert.Contains("kingsMantleAndCrown", Dress(Wearer.Of(head, household)));
    }

    [Fact]
    public void An_outfit_decides_its_colours_or_leaves_them_to_the_person()
    {
        var p = ParameterSet.Default;
        var rules = new LooksRules(p);
        var colours = p.Colours.Select(c => c.Id).ToHashSet();
        Assert.All(p.Outfits, o =>
        {
            // The main colour is either decided by the outfit or marked in its description.
            Assert.True(o.MainColour is null == o.Description.Contains(OutfitParameters.MainColourMark), o.Id);
            // A decided accent colour is written out; an accent that is not decided may be unused.
            if (o.AccentColour != null) Assert.DoesNotContain(OutfitParameters.AccentColourMark, o.Description);
            if (o.MainColour != null) Assert.Contains(o.MainColour, colours);
            if (o.AccentColour != null) Assert.Contains(o.AccentColour, colours);
            // With two colours no mark is left.
            Assert.DoesNotContain("{", rules.Describe(o, "skyBlue", "red"));
        });

        var coat = p.Outfits.Single(o => o.Id == "wrappedRidingCoat");
        Assert.StartsWith("Long coat of wool or cotton in sky blue, ", rules.Describe(coat, "skyBlue", "red"));
        Assert.Contains("a broad red silk sash", rules.Describe(coat, "skyBlue", "red"));
        // A colour that is not given, or not in the list, is undyed.
        Assert.StartsWith("Long coat of wool or cotton in undyed, ", rules.Describe(coat, null, "mauve"));
        Assert.Contains("a broad undyed silk sash", rules.Describe(coat, null, "mauve"));

        // The colour of the outfit wins over the colour of the person.
        var toga = p.Outfits.Single(o => o.Id == "citizensToga");
        Assert.Equal(("white", null), (toga.MainColour, toga.AccentColour));
        Assert.Contains("a border in purple along its edge", rules.Describe(toga, "red", "purple"));
        var custom = new OutfitParameters("smock", "Smock", true) { MainColour = "black", Description = "A {main} smock with {accent} cuffs." };
        Assert.Equal("A black smock with red cuffs.", new LooksRules(p with { Outfits = [custom] }).Describe(custom, "skyBlue", "red"));
        Assert.Equal("A forest green smock with undyed cuffs.", custom.DescriptionIn("Forest green", " "));
    }

    [Fact]
    public void A_limit_that_is_left_out_limits_nothing()
    {
        var everybody = new WearerParameters();
        Assert.True(LooksRules.Fits(everybody, Adult("orc", Sex.Male, 3, WealthLevel.Destitute)));
        Assert.True(LooksRules.Fits(null, Adult("orc")));

        var limits = new WearerParameters
        {
            Sex = Sex.Female, Races = ["elf", "orc"], LowestWealth = WealthLevel.Poor, HighestWealth = WealthLevel.Comfortable,
            Vocations = ["smith"], LowestAge = 20, HighestAge = 40, Married = false,
        };
        var fits = Adult("orc", vocation: "smith");
        Assert.True(LooksRules.Fits(limits, fits));
        Assert.False(LooksRules.Fits(limits, fits with { Sex = Sex.Male }));
        Assert.False(LooksRules.Fits(limits, fits with { Race = "dwarf" }));
        Assert.False(LooksRules.Fits(limits, fits with { Wealth = WealthLevel.Destitute }));
        Assert.False(LooksRules.Fits(limits, fits with { Wealth = WealthLevel.Rich }));
        Assert.True(LooksRules.Fits(limits, fits with { Wealth = WealthLevel.Poor }));
        Assert.False(LooksRules.Fits(limits, fits with { Vocation = "farmer" }));
        Assert.False(LooksRules.Fits(limits, fits with { Vocation = null }));
        Assert.False(LooksRules.Fits(limits, fits with { Age = 19 }));
        Assert.False(LooksRules.Fits(limits, fits with { Age = 41 }));
        Assert.True(LooksRules.Fits(limits, fits with { Age = 40 }));
        Assert.False(LooksRules.Fits(limits, fits with { Married = true }));

        // An entry that is off is worn by nobody, and one without limits by everybody.
        var p = ParameterSet.Default with
        {
            Hairstyles = [new("plain", "Plain", true), new("off", "Off", false)],
            Outfits = [new("smock", "Smock", true), new("off", "Off", false)],
            Accessories = [new("bead", "Bead", true, AccessoryKind.Necklace), new("off", "Off", false, AccessoryKind.Ring)],
        };
        var rules = new LooksRules(p);
        Assert.Equal(["plain"], rules.HairstylesOf(Adult("orc")).Select(h => h.Id));
        Assert.Equal(["smock"], rules.OutfitsOf(Adult("orc")).Select(o => o.Id));
        Assert.Equal(["bead"], rules.AccessoriesOf(Adult("orc")).Select(a => a.Id));
    }

    [Fact]
    public void A_file_can_leave_out_the_limits_the_texts_and_the_colours_of_a_race()
    {
        var set = ParameterSet.FromJson("""
            {
              "Hairstyles": [ { "Id": "plain", "Name": "Plain", "Enabled": true },
                              { "Id": "bun", "Name": "Bun", "Enabled": true, "WornBy": { "Sex": "Female", "Races": ["elf"], "Married": true } } ],
              "Outfits": [ { "Id": "smock", "Name": "Smock", "Enabled": true, "MainColour": "grey", "Description": "A grey smock." } ],
              "Accessories": [ { "Id": "bead", "Name": "Bead", "Enabled": true, "Kind": "BodyJewellery" } ],
              "HairColours": [ { "Id": "black", "Name": "Black", "Description": "black hair", "Greying": "grey" }, { "Id": "grey", "Name": "Grey", "Description": "grey hair" } ],
              "Races": [ { "Id": "elf", "Name": "Elves", "Enabled": true, "BiomeFrequency": { "Tundra": 1 }, "HairColours": ["black"] } ]
            }
            """);
        Assert.Equal((null, 0, WealthLevel.Destitute, WealthLevel.Rich, 0, null, null, null),
            (set.Hairstyles[0].WornBy.Sex, set.Hairstyles[0].WornBy.Races.Count, set.Hairstyles[0].WornBy.LowestWealth, set.Hairstyles[0].WornBy.HighestWealth,
                set.Hairstyles[0].WornBy.Vocations.Count, set.Hairstyles[0].WornBy.LowestAge, set.Hairstyles[0].WornBy.HighestAge, set.Hairstyles[0].WornBy.Married));
        Assert.Equal((Sex.Female, true), (set.Hairstyles[1].WornBy.Sex, set.Hairstyles[1].WornBy.Married));
        Assert.Equal(DefaultTexts.Missing, set.Hairstyles[0].Description);
        Assert.Equal(DefaultTexts.Missing, set.Outfits[0].Lore);
        Assert.Equal(("grey", null), (set.Outfits[0].MainColour, set.Outfits[0].AccentColour));
        Assert.Equal(AccessoryKind.BodyJewellery, set.Accessories[0].Kind);
        Assert.Equal(DefaultTexts.Missing, set.Accessories[0].Description);

        var elf = Assert.Single(set.Races);
        Assert.Empty(elf.SkinColours);
        Assert.Empty(elf.EyeColours);
        var rules = new LooksRules(set);
        Assert.Empty(rules.SkinColoursOf("elf"));
        Assert.Equal(["black"], rules.HairColoursOf("elf").Select(c => c.Id));
        Assert.Equal("grey", rules.GreyingOf("black")!.Id);
        Assert.Equal(["plain"], rules.HairstylesOf(Adult("elf")).Select(h => h.Id));
        Assert.Equal(["plain", "bun"], rules.HairstylesOf(Adult("elf", married: true)).Select(h => h.Id));
    }

    [Fact]
    public void A_parameter_set_with_a_broken_list_is_refused()
    {
        var p = ParameterSet.Default;
        void Refused(ParameterSet set, string because)
        {
            var e = Assert.Throws<ArgumentException>(() => new LooksRules(set));
            Assert.Contains(because, e.Message);
            Assert.Throws<ArgumentException>(() => World.Create("looks", set));
        }
        static WearerParameters By(string[]? races = null, string[]? vocations = null, WealthLevel from = WealthLevel.Destitute, WealthLevel to = WealthLevel.Rich, int? fromAge = null, int? toAge = null) =>
            new() { Races = races ?? [], Vocations = vocations ?? [], LowestWealth = from, HighestWealth = to, LowestAge = fromAge, HighestAge = toAge };

        // An id is given and unique in its list.
        Refused(p with { SkinColours = [new("", "Pale", "pale skin")] }, "A skin colour of the Parameter Set has no id.");
        Refused(p with { EyeColours = [.. p.EyeColours, p.EyeColours[0]] }, "the eye colour id \"blue\" more than once");
        Refused(p with { Hairstyles = [new("a", "A", true), new("a", "A", true)] }, "the hairstyle id \"a\" more than once");
        Refused(p with { Outfits = [new(" ", "A", true)] }, "An outfit of the Parameter Set has no id.");
        Refused(p with { Accessories = [new("", "A", true, AccessoryKind.Ring)] }, "An accessory of the Parameter Set has no id.");

        // A race lists colours of the three lists.
        var elf = p.Races.Single(r => r.Id == "elf");
        Refused(p with { Races = [elf with { SkinColours = ["mauve"] }] }, "The race \"elf\" lists \"mauve\" in SkinColours");
        Refused(p with { Races = [elf with { EyeColours = ["mauve"] }] }, "in EyeColours, which is not an eye colour");
        Refused(p with { Races = [elf with { HairColours = ["mauve"] }] }, "in HairColours, which is not a hair colour");

        // The greying is a hair colour of the list, and it ends.
        Refused(p with { HairColours = [new("black", "Black", "black hair") { Greying = "mauve" }] }, "greys into \"mauve\"");
        Refused(p with { HairColours = [new("a", "A", "a") { Greying = "b" }, new("b", "B", "b") { Greying = "a" }], Races = [] }, "does not end");
        Refused(p with { HairColours = [new("a", "A", "a") { Greying = "a" }], Races = [] }, "does not end");

        // What is worn is worn by races and vocations of the lists, within a wealth and ages that do not run backwards.
        Refused(p with { Hairstyles = [new("a", "A", true) { WornBy = By(races: ["goblin"]) }] }, "The hairstyle \"a\" lists \"goblin\" in WornBy.Races");
        Refused(p with { Outfits = [new("a", "A", true) { WornBy = By(vocations: ["dentist"]) }] }, "The outfit \"a\" lists \"dentist\" in WornBy.Vocations");
        Refused(p with { Accessories = [new("a", "A", true, AccessoryKind.Ring) { WornBy = By(from: WealthLevel.Rich, to: WealthLevel.Poor) }] }, "The accessory \"a\" is worn from Rich to Poor");
        Refused(p with { Hairstyles = [new("a", "A", true) { WornBy = By(fromAge: 40, toAge: 30) }] }, "is worn from the age 40 to 30");
        Refused(p with { Hairstyles = [new("a", "A", true) { WornBy = By(fromAge: -1) }] }, "the ages must be 0 or more");
        Refused(p with { Accessories = [new("a", "A", true, (AccessoryKind)99)] }, "is not a kind of accessory");

        // The colours of an outfit are colours of the Colours list.
        Refused(p with { Outfits = [new("a", "A", true) { MainColour = "mauve" }] }, "has the MainColour \"mauve\"");
        Refused(p with { Outfits = [new("a", "A", true) { AccentColour = "mauve" }] }, "has the AccentColour \"mauve\"");

        // An entry that is off is checked like the others, and limits that are left out are fine.
        Refused(p with { Hairstyles = [new("a", "A", false) { WornBy = By(races: ["goblin"]) }] }, "goblin");
        _ = new LooksRules(p with { Hairstyles = [new("a", "A", true) { WornBy = null! }], Outfits = [], Accessories = [] });
    }

    [Fact]
    public void The_looks_of_a_person_are_written_and_read_back()
    {
        var bare = new Person(0, 0, "Edric", "Alder", "midlanders", Sex.Male, 41, HouseholdRole.Head, "noble");
        Assert.Null(bare.Looks);

        var looks = new Looks("ruddyFair", "blue", "chestnut", "chinLengthBob", "furredGownAndWoundHood", "blue", "gold", ["goldSignetRing", "ringBrooch"]);
        var person = bare with { Looks = looks };
        var read = DomainJson.Deserialize<Person>(DomainJson.Serialize(person));
        Assert.Equal(looks with { Accessories = [] }, read.Looks! with { Accessories = [] });
        Assert.Equal(looks.Accessories, read.Looks!.Accessories);
        Assert.Null(DomainJson.Deserialize<Person>(DomainJson.Serialize(bare)).Looks);

        // Every part of the looks of the example is an entry of a default list.
        var p = ParameterSet.Default;
        Assert.Contains(p.SkinColours, c => c.Id == looks.SkinColour);
        Assert.Contains(p.EyeColours, c => c.Id == looks.EyeColour);
        Assert.Contains(p.HairColours, c => c.Id == looks.HairColour);
        Assert.Contains(p.Hairstyles, h => h.Id == looks.Hairstyle);
        Assert.Contains(p.Outfits, o => o.Id == looks.Outfit);
        Assert.Contains(p.Colours, c => c.Id == looks.OutfitMainColour);
        Assert.Contains(p.Colours, c => c.Id == looks.OutfitAccentColour);
        Assert.All(looks.Accessories, id => Assert.Contains(p.Accessories, a => a.Id == id));

        // No rule draws the looks yet: a generated person has none.
        Assert.All(World.Create("cellgen").PeopleOf(new RegionId(0, 0)).Persons, generated => Assert.Null(generated.Looks));
    }
}
