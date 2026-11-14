using System.Text.Json.Nodes;
using Gameworld.Domain;

namespace Gameworld.Tests;

public class DomainTests
{
    /// <summary>The repository root, found from where this source file lies.</summary>
    internal static string RepoRoot([System.Runtime.CompilerServices.CallerFilePath] string sourceFile = "")
    {
        var dir = new DirectoryInfo(Path.GetDirectoryName(sourceFile)!);
        while (dir != null && !File.Exists(Path.Combine(dir.FullName, "readme.md"))) dir = dir.Parent;
        return dir?.FullName ?? throw new InvalidOperationException("The repository root was not found.");
    }

    [Fact]
    public void Seed_text_of_digits_is_the_number_and_other_text_is_hashed()
    {
        Assert.Equal(123456789u, Seed.FromText("123456789").Value);
        Assert.Equal(7u, Seed.FromText("  7 ").Value);
        Assert.Equal("7", Seed.FromText("  7 ").Text);
        // Ten digits is no longer "1–9 digits": it is hashed like any other text.
        Assert.NotEqual(1234567890u, Seed.FromText("1234567890").Value);
        Assert.Equal(Seed.FromText("cellgen").Value, Seed.Default.Value);
        Assert.NotEqual(Seed.FromText("cellgen").Value, Seed.FromText("Cellgen").Value);
    }

    [Fact]
    public void Grid_key_orders_by_i_then_j_and_round_trips()
    {
        var coords = new[] { new GridCoord(0, 0), new GridCoord(-3, 7), new GridCoord(1000, -1000), new GridCoord(-2000000, 2000000) };
        foreach (var c in coords) Assert.Equal(c, GridCoord.FromKey(c.Key));
        Assert.True(new GridCoord(-1, 99).Key < new GridCoord(0, -99).Key);
        Assert.True(new GridCoord(5, 1).Key < new GridCoord(5, 2).Key);
    }

    [Fact]
    public void Default_parameter_set_is_the_documented_one()
    {
        var documented = JsonNode.Parse(File.ReadAllText(Path.Combine(RepoRoot(), "docs", "default-parameter-set.json")))!.AsObject();
        Assert.Equal("./" + ParameterSetSchema.FileName, documented["$schema"]?.GetValue<string>());
        documented.Remove("$schema"); // the pointer to the schema is not a parameter
        var builtIn = JsonNode.Parse(ParameterSet.Default.ToJson());
        Assert.True(JsonNode.DeepEquals(documented, builtIn), "docs/default-parameter-set.json and ParameterSet.Default differ.");
    }

    [Fact]
    public void The_documented_schema_is_the_generated_one_and_describes_every_parameter()
    {
        var path = Path.Combine(RepoRoot(), "docs", ParameterSetSchema.FileName);
        var documented = JsonNode.Parse(File.ReadAllText(path));
        var generated = ParameterSetSchema.Build();
        Assert.True(JsonNode.DeepEquals(documented, generated),
            $"docs/{ParameterSetSchema.FileName} is out of date. Regenerate it: dotnet run --project src/Gameworld.MapGenerator -- --export-schema docs/{ParameterSetSchema.FileName}");

        // Every parameter of the default Parameter Set has a description, and every plain value its default.
        var missing = new List<string>();
        void Check(JsonObject schema, JsonNode value, string at)
        {
            if (at.Length > 0 && string.IsNullOrWhiteSpace(schema["description"]?.GetValue<string>())) missing.Add(at + " (description)");
            Assert.Null(schema["$ref"]);
            switch (value)
            {
                case JsonObject obj:
                    Assert.False(schema["additionalProperties"]!.GetValue<bool>());
                    foreach (var (name, child) in obj)
                    {
                        if (schema["properties"]?[name] is JsonObject childSchema) Check(childSchema, child!, $"{at}/{name}");
                        else missing.Add($"{at}/{name}");
                    }
                    break;
                case JsonArray array when array.Count > 0 && array[0] is JsonObject first:
                    Check((JsonObject)schema["items"]!, first, at + "/items");
                    break;
                default:
                    if (!at.Contains("/items")) Assert.True(JsonNode.DeepEquals(value, schema["default"]), $"{at} has no default in the schema.");
                    break;
            }
        }
        Check(generated.AsObject(), JsonNode.Parse(ParameterSet.Default.ToJson())!, "");
        Assert.Empty(missing);

        Assert.Equal(1, generated["properties"]!["Siting"]!["properties"]!["RegionJitter"]!["maximum"]!.GetValue<double>());
        Assert.Equal(["Linked", "All"], generated["properties"]!["Outline"]!["properties"]!["OceanExpansionFrom"]!["enum"]!.AsArray().Select(v => v!.GetValue<string>()));
    }

    [Fact]
    public void A_parameter_file_may_point_at_its_schema_and_leave_parameters_out()
    {
        var p = ParameterSet.FromJson("""{ "$schema": "./parameter-set.schema.json", "Siting": { "PatchWarp": 0.4 } }""");
        Assert.Equal(0.4, p.Siting.PatchWarp);
        Assert.Equal(ParameterSet.Default.Siting.PatchGridSizeKm, p.Siting.PatchGridSizeKm);
        var expected = ParameterSet.Default with { Siting = ParameterSet.Default.Siting with { PatchWarp = 0.4 } };
        Assert.Equal(expected.ToJson(), p.ToJson());
    }

    [Fact]
    public void Parameter_set_round_trips_through_json_and_a_file()
    {
        var changed = ParameterSet.Default with
        {
            Siting = ParameterSet.Default.Siting with { PatchWarp = 0.4, OceanSeedsPerRegion = new IntRange(1, 3) },
            Outline = ParameterSet.Default.Outline with { OceanExpansionFrom = OceanExpansionFrom.All },
        };
        var path = Path.Combine(Path.GetTempPath(), $"gameworld-parameters-{Guid.NewGuid():N}.json");
        try
        {
            var foundation = new Foundation(Seed.Default, new GeneratorVersion(1), changed);
            foundation.SaveParameters(path);
            var reloaded = new Foundation(Seed.Default, new GeneratorVersion(1), ParameterSet.Default).ReloadParameters(path);
            Assert.Equal(changed.ToJson(), reloaded.Parameters.ToJson());
            Assert.Equal(0.4, reloaded.Parameters.Siting.PatchWarp);
            Assert.Equal(OceanExpansionFrom.All, reloaded.Parameters.Outline.OceanExpansionFrom);
            Assert.Equal(foundation.Seed, reloaded.Seed);
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void Stage_needs_follow_the_readme_table()
    {
        var p = ParameterSet.Default;
        Assert.Null(RegionStageRules.NeedOf(RegionStage.Sited, p));
        Assert.Equal(new StageNeed(RegionStage.Sited, 7), RegionStageRules.NeedOf(RegionStage.Outlined, p));
        Assert.Equal(new StageNeed(RegionStage.Outlined, 1), RegionStageRules.NeedOf(RegionStage.Detailed, p));
        Assert.Equal(new StageNeed(RegionStage.Detailed, 1), RegionStageRules.NeedOf(RegionStage.Elevated, p));
        Assert.Equal(new StageNeed(RegionStage.Elevated, 0), RegionStageRules.NeedOf(RegionStage.Planned, p));
        Assert.Equal(new StageNeed(RegionStage.Planned, 1), RegionStageRules.NeedOf(RegionStage.Settled, p));

        var wider = p with { Outline = p.Outline with { OceanLinkRangeRegions = 3 } };
        Assert.Equal(8, RegionStageRules.NeedOf(RegionStage.Outlined, wider)!.Value.Rings);
    }

    [Fact]
    public void A_region_can_advance_only_when_the_regions_around_it_are_far_enough()
    {
        var p = ParameterSet.Default;
        var region = new RegionId(0, 0);
        var stages = new Dictionary<RegionId, RegionStage>();
        RegionStage StageOf(RegionId id) => stages.GetValueOrDefault(id, RegionStage.Sited);

        // Sited → Outlined needs only Sited regions, which any region is.
        Assert.True(RegionStageRules.CanAdvance(region, RegionStage.Sited, StageOf, p));

        // Outlined → Detailed needs all bordering regions Outlined.
        Assert.False(RegionStageRules.CanAdvance(region, RegionStage.Outlined, StageOf, p));
        foreach (var r in RegionStageRules.Bordering(region)) stages[r] = RegionStage.Outlined;
        Assert.True(RegionStageRules.CanAdvance(region, RegionStage.Outlined, StageOf, p));
        stages[new RegionId(1, -1)] = RegionStage.Sited;
        Assert.False(RegionStageRules.CanAdvance(region, RegionStage.Outlined, StageOf, p));

        // Elevated → Planned needs the region itself only.
        Assert.True(RegionStageRules.CanAdvance(region, RegionStage.Elevated, StageOf, p));

        // Settled is the last stage.
        Assert.False(RegionStageRules.CanAdvance(region, RegionStage.Settled, StageOf, p));
        Assert.Equal(8, RegionStageRules.Bordering(region).Count());
    }

    [Fact]
    public void A_stored_region_starts_outlined_and_only_advances_one_stage_at_a_time()
    {
        var region = new Region(new RegionId(2, 3), new RegionOutline([], HasSeaAccess: false));
        Assert.Equal(RegionStage.Outlined, region.Stage);

        var plan = new RegionPlan([], new PotentialPopulation([new RacePopulation("human", 1200)]));
        Assert.Throws<InvalidOperationException>(() => region.Advance(plan));

        region.Advance(new RegionDetail([], new Rivers([], false), []));
        Assert.Equal(RegionStage.Detailed, region.Stage);
        Assert.Throws<InvalidOperationException>(() => region.Advance(new RegionDetail([], new Rivers([], false), [])));

        region.Advance(new RegionElevation(new HeightMap(0, 0, 1, 1, 1000, [0f]), []));
        region.Advance(plan);
        region.Advance(new RegionSettlement(new TravelNetwork([])));
        Assert.Equal(RegionStage.Settled, region.Stage);

        var restored = Region.FromSnapshot(DomainJson.Deserialize<RegionSnapshot>(DomainJson.Serialize(region.ToSnapshot())));
        Assert.Equal(RegionStage.Settled, restored.Stage);
        Assert.Equal(region.Id, restored.Id);
    }

    [Fact]
    public void Entity_generation_runs_stub_pending_realised_and_returns_to_stub_on_failure()
    {
        var generation = EntityGeneration.Stub;
        Assert.Equal(GenerationState.Stub, generation.Text.State);

        // Text and portrait are tracked separately.
        generation = generation with { Text = generation.Text.Begin() };
        Assert.Equal(GenerationState.Pending, generation.Text.State);
        Assert.Equal(GenerationState.Stub, generation.Portrait.State);

        generation = generation with { Text = generation.Text.Complete(), Portrait = generation.Portrait.Begin() };
        Assert.Equal(GenerationState.Realised, generation.Text.State);

        generation = generation with { Portrait = generation.Portrait.Fail() };
        Assert.Equal(GenerationState.Stub, generation.Portrait.State);

        Assert.Throws<InvalidOperationException>(() => AuthoringTrack.Stub.Complete());
        Assert.Throws<InvalidOperationException>(() => generation.Text.Begin());
    }

    [Fact]
    public void Every_generated_resource_has_a_group_and_no_generated_resource_is_only_produced()
    {
        Assert.Equal(ResourceCatalog.Count, ResourceCatalog.All.Count);
        foreach (var resource in ResourceCatalog.All)
            Assert.False(ResourceCatalog.IsOnlyProduced(ResourceCatalog.GroupOf(resource)));
        Assert.Equal(ResourceGroup.Food, ResourceCatalog.GroupOf(Resource.WildFoodAndGame));
        Assert.Equal(ResourceGroup.Animals, ResourceCatalog.GroupOf(Resource.HorsesAndPackAnimals));
    }

    [Fact]
    public void Identities_and_points_have_a_compact_json_form()
    {
        Assert.Equal("[3,-4]", DomainJson.Serialize(new RegionId(3, -4)));
        Assert.Equal(new PatchId(5, 6), DomainJson.Deserialize<PatchId>("[5,6]"));
        Assert.Equal("[1.5,2]", DomainJson.Serialize(new WorldPoint(1.5, 2)));
        Assert.Equal("\"Hills\"", DomainJson.Serialize(ElevationType.Hills));

        var foundation = new Foundation(Seed.FromText("atlas"), new GeneratorVersion(1), ParameterSet.Default);
        var back = DomainJson.Deserialize<Foundation>(DomainJson.Serialize(foundation));
        Assert.Equal(foundation.Seed, back.Seed);
        Assert.Equal(foundation.GeneratorVersion, back.GeneratorVersion);
        Assert.Equal(foundation.Parameters.ToJson(), back.Parameters.ToJson());
    }
}
