using Gameworld.Domain;
using Gameworld.Generation.Resources;

namespace Gameworld.Tests;

/// <summary>
/// The description and the lore of the races, the vocations, the tools, the flora, the fauna
/// and the resources: the texts a picture is to be drawn from.
/// </summary>
public class DefaultTextTests
{
    private static void AssertWritten(string what, string description, string lore)
    {
        AssertDescribed(what, description);
        Assert.False(DefaultTexts.IsMissing(lore), $"{what} has no lore.");
        AssertClean(lore);
    }

    private static void AssertDescribed(string what, string description)
    {
        Assert.False(DefaultTexts.IsMissing(description), $"{what} has no description.");
        AssertClean(description);
    }

    // The citations and the research remarks of the basis docs stay in the docs.
    private static void AssertClean(string text)
    {
        Assert.DoesNotContain("http", text);
        Assert.DoesNotContain("verified", text);
        Assert.DoesNotContain("§", text);
        Assert.DoesNotContain("\n ", text);
        Assert.Equal(text.Trim(), text);
    }

    [Fact]
    public void Every_default_race_vocation_tool_and_fauna_has_a_description_and_a_lore()
    {
        var p = ParameterSet.Default;
        Assert.All(p.Races, r => AssertWritten(r.Id, r.Description, r.Lore));
        Assert.All(p.Vocations, v => AssertWritten(v.Id, v.Description, v.Lore));
        Assert.All(p.Tools, t => AssertWritten(t.Id, t.Description, t.Lore));
        Assert.All(p.Fauna, f => AssertWritten(f.Id, f.Description, f.Lore));
    }

    [Fact]
    public void Every_default_flora_has_a_description_and_a_lore()
    {
        Assert.All(ParameterSet.Default.Flora, f => AssertWritten(f.Id, f.Description, f.Lore));
    }

    [Fact]
    public void No_text_of_the_default_parameter_set_is_missing()
    {
        Assert.DoesNotContain(DefaultTexts.Missing, ParameterSet.Default.ToJson());
    }

    [Fact]
    public void A_kind_without_texts_in_a_file_gets_the_missing_text()
    {
        var set = ParameterSet.FromJson("""{ "Tools": [ { "Id": "hook", "Name": "Hook", "Enabled": true, "Vocations": ["fisher"] } ] }""");
        var tool = Assert.Single(set.Tools);
        Assert.Equal("**missing description**", tool.Description);
        Assert.Equal("**missing description**", tool.Lore);
        Assert.True(DefaultTexts.IsMissing(tool.Description));
    }

    [Fact]
    public void The_default_resources_are_the_27_generated_ones_in_index_order()
    {
        var resources = ParameterSet.Default.Resources;
        Assert.Equal(ResourceCatalog.All, resources.Select(r => r.Resource));
        Assert.All(resources, r =>
        {
            Assert.NotEmpty(r.Variants);
            Assert.Equal(r.Variants.Count, r.Variants.Select(v => v.Name).Distinct().Count());
            // Every resource and every variant has both texts, and no two variants of a resource are alike.
            AssertWritten(r.Name, r.Description, r.Lore);
            Assert.All(r.Variants, v => AssertWritten($"{r.Name}: {v.Name}", v.Description, v.Lore));
            Assert.Equal(r.Variants.Count, r.Variants.Select(v => v.Description).Distinct().Count());
            Assert.Equal(r.Variants.Count, r.Variants.Select(v => v.Lore).Distinct().Count());
        });
        // The crops are variants of their resource, in table order.
        foreach (var resource in CropTables.CropResources)
        {
            var crops = CropTables.Of(resource).Select(c => c.Name).ToArray();
            var variants = resources[(int)resource].Variants.Select(v => v.Name).Take(crops.Length);
            Assert.Equal(crops, variants);
        }
    }

    [Fact]
    public void The_arcane_materials_that_are_flora_take_the_texts_of_the_flora()
    {
        var p = ParameterSet.Default;
        var arcane = p.Resources[(int)Resource.ArcaneMaterials];
        Assert.Equal(14, arcane.Variants.Count);
        var frostLotus = p.Flora.Single(f => f.Id == "frostLotus");
        Assert.Equal(frostLotus.Description, arcane.Variant("frost-lotus")!.Description);
        Assert.Equal(frostLotus.Lore, arcane.Variant("frost-lotus")!.Lore);
        foreach (var name in new[] { "glowcap fungi", "elderwood", "sap of Elanor", "dreamleaf" })
            Assert.False(DefaultTexts.IsMissing(arcane.Variant(name)!.Description));
        // The other nine have their texts in the list itself.
        Assert.StartsWith("Iron that fell from the sky", arcane.Variant("star metal")!.Description);
        Assert.False(DefaultTexts.IsMissing(arcane.Variant("star metal")!.Lore));
        Assert.Null(arcane.Variant("no such thing"));
        Assert.Null(arcane.Variant(null));
    }

    [Fact]
    public void The_default_biomes_are_the_17_biomes_in_order_and_all_have_both_texts()
    {
        var biomes = ParameterSet.Default.Biomes;
        Assert.Equal(Enum.GetValues<Biome>(), biomes.Select(b => b.Biome));
        Assert.Equal("Ice & snow", biomes[0].Name);
        Assert.All(biomes, biome => AssertWritten(biome.Name, biome.Description, biome.Lore));
    }

    [Fact]
    public void A_vocation_variant_has_a_name_a_description_and_a_lore()
    {
        var p = ParameterSet.Default;
        Assert.Equal(476, p.Vocations.Sum(v => v.Variants.Count));
        var shepherd = p.Vocations.Single(v => v.Id == "herder").Variants.Single(v => v.Name == "shepherd");
        Assert.StartsWith("Minds a flock of sheep", shepherd.Description);
        Assert.StartsWith("Alone for weeks with the flock", shepherd.Lore);

        // Every variant has both texts, and no two of them the same.
        var all = p.Vocations.SelectMany(v => v.Variants.Select(variant => (Id: $"{v.Id}/{variant.Name}", variant.Description, variant.Lore))).ToArray();
        Assert.All(all, variant => AssertWritten(variant.Id, variant.Description, variant.Lore));
        Assert.Equal(all.Length, all.Select(variant => variant.Description).Distinct().Count());
        Assert.Equal(all.Length, all.Select(variant => variant.Lore).Distinct().Count());

        var set = ParameterSet.FromJson("""
            { "Vocations": [ { "Id": "herder", "Name": "Herder", "Enabled": true, "Sector": "LandAndWater", "LowestTier": "Countryside",
              "Variants": [ { "Name": "shepherd", "Description": "Carries a crook." }, { "Name": "cowherd" } ] } ] }
            """);
        var variants = set.Vocations[0].Variants;
        Assert.Equal("Carries a crook.", variants[0].Description);
        Assert.Equal(DefaultTexts.Missing, variants[0].Lore);
        Assert.Equal(DefaultTexts.Missing, variants[1].Description);
    }

    [Fact]
    public void The_fixed_sets_have_a_text_entry_for_every_value_in_order()
    {
        var p = ParameterSet.Default;
        Assert.Equal(Enum.GetValues<ElevationType>(), p.ElevationTypes.Select(k => k.Kind));
        Assert.Equal(Enum.GetValues<FactionType>(), p.FactionTypes.Select(k => k.Kind));
        Assert.Equal(Enum.GetValues<FactionForm>(), p.FactionForms.Select(k => k.Kind));
        Assert.Equal(Enum.GetValues<SettlementKind>(), p.SettlementKinds.Select(k => k.Kind));
        Assert.Equal(Enum.GetValues<SettlementTier>(), p.SettlementTiers.Select(k => k.Kind));

        // Every value has a description and a lore.
        Assert.All(p.FactionTypes, k => AssertWritten(k.Name, k.Description, k.Lore));
        Assert.All(p.FactionForms, k => AssertWritten(k.Name, k.Description, k.Lore));
        Assert.All(p.ElevationTypes, k => AssertWritten(k.Name, k.Description, k.Lore));
        Assert.All(p.SettlementKinds, k => AssertWritten(k.Name, k.Description, k.Lore));
        Assert.All(p.SettlementTiers, k => AssertWritten(k.Name, k.Description, k.Lore));
        Assert.Equal("Merchant republic", p.FactionForms.Single(k => k.Kind == FactionForm.MerchantRepublic).Name);
        Assert.Equal("Flat Land", p.ElevationTypes[1].Name);
    }

    [Fact]
    public void Every_default_bedrock_has_a_description_and_a_lore()
    {
        Assert.All(ParameterSet.Default.Bedrocks, b =>
        {
            Assert.StartsWith("Mostly under ", b.Description);
            AssertWritten(b.Id, b.Description, b.Lore);
        });
    }
}
