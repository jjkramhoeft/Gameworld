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
        Assert.False(DefaultTexts.IsMissing(description), $"{what} has no description.");
        Assert.False(DefaultTexts.IsMissing(lore), $"{what} has no lore.");
        // The citations and the research remarks of the basis docs stay in the docs.
        foreach (var text in new[] { description, lore })
        {
            Assert.DoesNotContain("http", text);
            Assert.DoesNotContain("verified", text);
            Assert.DoesNotContain("§", text);
            Assert.DoesNotContain("\n ", text);
            Assert.Equal(text.Trim(), text);
        }
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
    public void Every_default_flora_but_the_giant_mushrooms_has_a_description_and_a_lore()
    {
        foreach (var flora in ParameterSet.Default.Flora)
        {
            if (flora.Id == "giantMushrooms")
            {
                Assert.Equal(DefaultTexts.Missing, flora.Description);
                Assert.Equal(DefaultTexts.Missing, flora.Lore);
            }
            else AssertWritten(flora.Id, flora.Description, flora.Lore);
        }
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
        Assert.Equal(DefaultTexts.Missing, arcane.Variant("star metal")!.Description);
        Assert.Null(arcane.Variant("no such thing"));
        Assert.Null(arcane.Variant(null));
    }

    [Fact]
    public void The_default_biomes_are_the_17_biomes_in_order_and_all_but_the_volcano_are_described()
    {
        var biomes = ParameterSet.Default.Biomes;
        Assert.Equal(Enum.GetValues<Biome>(), biomes.Select(b => b.Biome));
        Assert.Equal("Ice & snow", biomes[0].Name);
        foreach (var biome in biomes)
        {
            Assert.Equal(biome.Biome == Biome.Volcano, DefaultTexts.IsMissing(biome.Description));
            Assert.Equal(DefaultTexts.Missing, biome.Lore); // no biome has a lore yet
        }
    }

    [Fact]
    public void A_vocation_variant_has_a_name_a_description_and_a_lore()
    {
        var p = ParameterSet.Default;
        Assert.Equal(476, p.Vocations.Sum(v => v.Variants.Count));
        var shepherd = p.Vocations.Single(v => v.Id == "herder").Variants.Single(v => v.Name == "shepherd");
        Assert.Equal(DefaultTexts.Missing, shepherd.Description); // no variant is described yet
        Assert.Equal(DefaultTexts.Missing, shepherd.Lore);

        var set = ParameterSet.FromJson("""
            { "Vocations": [ { "Id": "herder", "Name": "Herder", "Enabled": true, "Sector": "LandAndWater", "LowestTier": "Countryside",
              "Variants": [ { "Name": "shepherd", "Description": "Carries a crook." }, { "Name": "cowherd" } ] } ] }
            """);
        var variants = set.Vocations[0].Variants;
        Assert.Equal("Carries a crook.", variants[0].Description);
        Assert.Equal(DefaultTexts.Missing, variants[0].Lore);
        Assert.Equal(DefaultTexts.Missing, variants[1].Description);
    }
}
