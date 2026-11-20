using Gameworld.Domain;
using Gameworld.Generation;
using Gameworld.Generation.Vocations;

namespace Gameworld.Tests;

/// <summary>The vocations and their tools: society-generation.md sections 8 and 8.1.</summary>
public class VocationTests
{
    private static readonly VocationParameters Smith = new("smith", "Smith", true, VocationSector.Craft, SettlementTier.Countryside);
    private static readonly VocationParameters Farmer = new("farmer", "Farmer", true, VocationSector.LandAndWater, SettlementTier.Countryside);

    private static ToolParameters Tool(string id, double emblemWeight, params string[] vocations) =>
        new(id, id, true, vocations) { EmblemWeight = emblemWeight };

    private static ParameterSet With(IReadOnlyList<VocationParameters> vocations, params ToolParameters[] tools) =>
        ParameterSet.Default with { Vocations = vocations, Tools = tools };

    [Fact]
    public void The_default_tools_fit_the_default_vocations()
    {
        var parameters = ParameterSet.Default;
        var tools = parameters.Tools;
        Assert.Equal(146, tools.Count);
        Assert.Equal(tools.Count, tools.Select(t => t.Id).Distinct().Count());
        Assert.All(tools, t => Assert.True(t.Enabled, t.Id));
        Assert.All(tools, t => Assert.InRange(t.EmblemWeight, 0, 3));

        // Every default vocation has at least two tools, and at least one of them can be its emblem.
        var rules = new VocationRules(parameters);
        Assert.Equal(51, parameters.Vocations.Count);
        foreach (var vocation in parameters.Vocations)
        {
            Assert.True(rules.ToolsOf(vocation.Id).Count >= 2, vocation.Id);
            Assert.NotEmpty(rules.EmblemsOf(vocation.Id));
        }

        // A tool stands under the vocation it belongs to most, which it lists first.
        Assert.Equal(["anvil", "hammersAndTongs"], tools.Where(t => t.Vocations[0] == "smith").Select(t => t.Id));
        Assert.Equal(["bellows", "anvil", "hammersAndTongs"], rules.ToolsOf("smith").Select(t => t.Id));
        Assert.Equal(["hunter", "soldier", "bandit"], tools.Single(t => t.Id == "longbow").Vocations);
        Assert.DoesNotContain(rules.EmblemsOf("tanner"), t => t.Id == "tanPits");
        Assert.Contains(rules.ToolsOf("tanner"), t => t.Id == "tanPits");

        // The list is read back from a file.
        var read = ParameterSet.FromJson(parameters.ToJson()).Tools;
        Assert.Equal(tools.Select(t => (t.Id, t.Name, t.Enabled, t.EmblemWeight, string.Join(",", t.Vocations))),
            read.Select(t => (t.Id, t.Name, t.Enabled, t.EmblemWeight, string.Join(",", t.Vocations))));

        // The world of the defaults has the rules.
        Assert.NotEmpty(World.Create("tools").VocationRules.ToolsOf("farmer"));
    }

    [Fact]
    public void The_default_tools_follow_the_tools_basis()
    {
        // Every entry of docs/tools-basis.md is a tool: its heading is the name, and its line
        // "Fitting vocations" (which can wrap) names the vocations, in the names of the list.
        var idOf = ParameterSet.Default.Vocations.ToDictionary(v => v.Name, v => v.Id);
        var lines = File.ReadAllLines(Path.Combine(DomainTests.RepoRoot(), "docs", "tools-basis.md"));
        var documented = new List<(string Name, string Vocations)>();
        string? name = null;
        for (var i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith("#### ")) name = lines[i][5..].Trim();
            if (!lines[i].StartsWith("- **Fitting vocations.**")) continue;
            var text = lines[i]["- **Fitting vocations.**".Length..];
            while (i + 1 < lines.Length && lines[i + 1].StartsWith("  ")) text += " " + lines[++i].Trim();
            var vocations = text.Trim().TrimEnd('.').Split(',', StringSplitOptions.TrimEntries).Select(v => idOf[v]);
            documented.Add((name!, string.Join(",", vocations)));
        }
        Assert.Equal(documented, ParameterSet.Default.Tools.Select(t => (t.Name, string.Join(",", t.Vocations))));
    }

    [Fact]
    public void A_tool_or_a_vocation_that_is_off_drops_out()
    {
        var hammer = Tool("hammer", 3, "smith");
        var tongs = Tool("tongs", 0, "smith", "farmer");
        var off = Tool("file", 2, "smith") with { Enabled = false };
        var rules = new VocationRules(With([Smith, Farmer], hammer, tongs, off));
        Assert.Equal(["hammer", "tongs"], rules.ToolsOf("smith").Select(t => t.Id));
        Assert.Equal(["hammer"], rules.EmblemsOf("smith").Select(t => t.Id));
        Assert.Equal(["tongs"], rules.ToolsOf("farmer").Select(t => t.Id));
        Assert.Empty(rules.EmblemsOf("farmer"));
        Assert.Empty(rules.ToolsOf("mason"));

        var noSmith = new VocationRules(With([Smith with { Enabled = false }, Farmer], hammer, tongs));
        Assert.Empty(noSmith.ToolsOf("smith"));
        Assert.Equal(["tongs"], noSmith.ToolsOf("farmer").Select(t => t.Id));

        // A tool left without its emblem weight weighs 1.
        Assert.Equal(1, new ToolParameters("x", "X", true, ["smith"]).EmblemWeight);
    }

    [Fact]
    public void A_wrong_tool_or_vocation_is_refused()
    {
        VocationParameters[] vocations = [Smith, Farmer];
        Assert.Throws<ArgumentException>(() => new VocationRules(With(vocations, Tool("hammer", 1, "mason"))));
        Assert.Throws<ArgumentException>(() => new VocationRules(With(vocations, Tool("hammer", 1))));
        Assert.Throws<ArgumentException>(() => new VocationRules(With(vocations, Tool("hammer", 1, "smith"), Tool("hammer", 2, "farmer"))));
        Assert.Throws<ArgumentException>(() => new VocationRules(With(vocations, Tool("", 1, "smith"))));
        Assert.Throws<ArgumentException>(() => new VocationRules(With(vocations, Tool("hammer", -1, "smith"))));
        Assert.Throws<ArgumentException>(() => new VocationRules(With(vocations, Tool("hammer", double.NaN, "smith"))));
        Assert.Throws<ArgumentException>(() => new VocationRules(With([Smith, Smith], Tool("hammer", 1, "smith"))));
        Assert.Throws<ArgumentException>(() => new VocationRules(With([Smith with { Id = "" }])));

        // The world is refused with its Parameter Set.
        Assert.Throws<ArgumentException>(() => World.Create("tools", With(vocations, Tool("hammer", 1, "mason"))));
    }
}
