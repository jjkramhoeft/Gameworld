using Gameworld.Domain;
using Gameworld.Generation;
using Gameworld.Generation.Biomes;

namespace Gameworld.Tests;

public class FloraFaunaTests
{
    private static readonly Biome[] AllBiomes = Enum.GetValues<Biome>();

    [Fact]
    public void The_default_lists_hold_the_documented_flora_fauna_and_vocations()
    {
        var p = ParameterSet.Default;
        Assert.Equal(208, p.Flora.Count);
        Assert.Equal(167, p.Fauna.Count);
        Assert.Equal(51, p.Vocations.Count);

        // An id is unique in its list, and no kind of flora shares its id with a kind of fauna.
        Assert.Equal(375, p.Flora.Select(f => f.Id).Concat(p.Fauna.Select(f => f.Id)).Distinct().Count());
        Assert.Equal(51, p.Vocations.Select(v => v.Id).Distinct().Count());

        Assert.All(p.Flora, f => Assert.True(f.Enabled && f.Frequency > 0 && (f.Sea || f.Biomes.Count > 0), f.Id));
        Assert.All(p.Fauna, f => Assert.True(f.Enabled && f.Frequency > 0 && (f.Sea || f.Biomes.Count > 0), f.Id));

        // Only the arcane plants and the monsters need the Supernatural field, and all of them do.
        Assert.All(p.Flora, f => Assert.Equal(f.Type == FloraType.Arcane, f.MinSupernatural >= 60));
        Assert.All(p.Fauna, f => Assert.Equal(f.Type == FaunaType.Monster, f.MinSupernatural >= 60));

        // One kind stands in for a type: the giant mushrooms, an arcane plant in the place of the trees.
        var giant = Assert.Single(p.Flora, f => f.StandsInFor is not null);
        Assert.Equal(("giantMushrooms", FloraType.Arcane, FloraType.Tree), (giant.Id, giant.Type, giant.StandsInFor!.Value));
        Assert.Equal(giant, p.Flora[^1]);
        Assert.All(giant.Biomes, b => Assert.Contains(p.Flora, f => f.Type == FloraType.Tree && f.Biomes.Contains(b)));

        // Every biome and the sea have ordinary flora and fauna to draw from.
        foreach (var biome in AllBiomes)
        {
            Assert.Contains(p.Flora, f => f.Biomes.Contains(biome) && f.MinSupernatural == 0);
            Assert.Contains(p.Fauna, f => f.Biomes.Contains(biome) && f.MinSupernatural == 0);
        }
        Assert.Contains(p.Flora, f => f.Sea);
        Assert.Contains(p.Fauna, f => f.Sea);

        // The vocations: nine sectors, and shares that sum to 100 %.
        Assert.Equal(Enum.GetValues<VocationSector>(), p.Vocations.Select(v => v.Sector).Distinct());
        Assert.Equal(100, p.Vocations.Sum(v => v.SharePercent), 9);
        Assert.Equal(["shipwright", "sailor"], p.Vocations.Where(v => v.OnWater).Select(v => v.Id));
        Assert.Contains("armourer", p.Vocations.Single(v => v.Id == "smith").Variants.Select(v => v.Name));
        Assert.Equal(SettlementTier.Capital, p.Vocations.Single(v => v.Id == "mage").LowestTier);
    }

    [Fact]
    public void The_lists_round_trip_through_json_and_a_file_may_replace_them()
    {
        var back = ParameterSet.FromJson(ParameterSet.Default.ToJson());
        Assert.Equal(ParameterSet.Default.ToJson(), back.ToJson());
        var kelp = back.Flora.Single(f => f.Id == "kelp");
        Assert.True(kelp.Sea);
        Assert.Empty(kelp.Biomes);
        Assert.Equal(90, back.Fauna.Single(f => f.Id == "phoenix").MinSupernatural);
        Assert.Equal(FloraType.Tree, back.Flora.Single(f => f.Id == "giantMushrooms").StandsInFor);
        Assert.Null(kelp.StandsInFor);

        var own = ParameterSet.FromJson("""
            {
              "Flora": [ { "Id": "moonreed", "Name": "Moonreed", "Type": "Grass", "Enabled": true, "Biomes": ["Savanna"] } ],
              "Fauna": [],
              "Vocations": [ { "Id": "smith", "Name": "Smith", "Enabled": true, "Sector": "Craft", "LowestTier": "Town" } ]
            }
            """);
        Assert.Equal(["moonreed"], own.Flora.Select(f => f.Id));
        Assert.Equal(1, own.Flora[0].Frequency);
        Assert.Equal(0, own.Flora[0].MinSupernatural);
        Assert.False(own.Flora[0].Sea);
        Assert.Null(own.Flora[0].StandsInFor);
        Assert.Empty(own.Fauna);
        Assert.Empty(own.Vocations[0].Variants);
        Assert.Equal(28, own.Races.Count); // a list that is left out keeps its default
    }

    [Fact]
    public void A_patch_gets_kinds_that_fit_its_biome_and_its_supernatural()
    {
        var p = ParameterSet.Default;
        var rules = new FloraFaunaRules(p);
        var flora = p.Flora.ToDictionary(f => f.Id);
        var fauna = p.Fauna.ToDictionary(f => f.Id);
        var monsters = 0;
        foreach (var biome in AllBiomes)
            for (var k = 0; k < 200; k++)
            {
                var patch = new GridCoord(k * 7 - 300, 40 - k);
                var supernatural = k % 2 == 0 ? 30.0 : 95.0;
                var gotFlora = rules.Flora(42, patch, biome, supernatural);
                var gotFauna = rules.Fauna(42, patch, biome, supernatural);

                Assert.InRange(gotFauna.Count, 3, 6);
                Assert.InRange(gotFlora.Count, 1, 6); // Ice & snow has three ordinary plants only
                Assert.Equal(gotFlora.Distinct(), gotFlora);
                Assert.All(gotFlora, id => Assert.Contains(biome, flora[id].Biomes));
                Assert.All(gotFauna, id => Assert.Contains(biome, fauna[id].Biomes));
                Assert.All(gotFlora, id => Assert.True(flora[id].MinSupernatural <= supernatural));
                Assert.All(gotFauna, id => Assert.True(fauna[id].MinSupernatural <= supernatural));
                monsters += gotFauna.Count(id => fauna[id].Type == FaunaType.Monster);
            }
        Assert.True(monsters > 0, "A patch with a high Supernatural can hold a monster.");

        // An ocean patch has no biome and gets kinds of the sea.
        var sea = rules.Fauna(42, new GridCoord(3, 4), null, 0);
        Assert.NotEmpty(sea);
        Assert.All(sea, id => Assert.True(fauna[id].Sea));
        Assert.All(rules.Flora(42, new GridCoord(3, 4), null, 0), id => Assert.True(flora[id].Sea));
    }

    [Fact]
    public void A_kind_is_drawn_by_its_frequency_and_never_when_it_is_off()
    {
        FaunaParameters Kind(string id, double frequency, bool enabled = true) =>
            new(id, id, FaunaType.Animal, enabled, [Biome.Savanna]) { Frequency = frequency };
        var p = ParameterSet.Default with
        {
            Fauna = [Kind("common", 4), Kind("rare", 1), Kind("off", 100, enabled: false), Kind("never", 0)],
            Elevation = ParameterSet.Default.Elevation with
            {
                Biomes = ParameterSet.Default.Elevation.Biomes with { FaunaPerPatch = new(1, 1) },
            },
        };
        var rules = new FloraFaunaRules(p);
        var counts = new Dictionary<string, int> { ["common"] = 0, ["rare"] = 0 };
        for (var k = 0; k < 4000; k++)
        {
            var got = rules.Fauna(7, new GridCoord(k, -k * 3), Biome.Savanna, 0);
            counts[Assert.Single(got)]++;
        }
        Assert.InRange(counts["common"] / 4000.0, 0.77, 0.83); // 4 of 5
        Assert.Empty(rules.Fauna(7, new GridCoord(1, 1), Biome.Tundra, 0));

        // Switching a kind off leaves the draws of the other kinds as they were.
        var without = new FloraFaunaRules(p with { Fauna = [Kind("common", 4, enabled: false), Kind("rare", 1)] });
        Assert.Equal(["rare"], without.Fauna(7, new GridCoord(5, 5), Biome.Savanna, 0));
    }

    [Fact]
    public void Giant_mushrooms_stand_in_for_the_trees_of_a_rare_magical_patch()
    {
        var p = ParameterSet.Default;
        var rules = new FloraFaunaRules(p);
        var without = new FloraFaunaRules(p with { Flora = p.Flora.Select(f => f.Id == "giantMushrooms" ? f with { Enabled = false } : f).ToArray() });
        var flora = p.Flora.ToDictionary(f => f.Id);
        var giant = flora["giantMushrooms"];
        int patches = 0, withMushrooms = 0;
        foreach (var biome in giant.Biomes)
            for (var k = 0; k < 3000; k++)
            {
                var patch = new GridCoord(k * 5 - 900, 17 - k * 3);
                var magical = rules.Flora(42, patch, biome, 75);
                var plain = without.Flora(42, patch, biome, 75);
                patches++;

                // Below a Supernatural of 60 the patch never has them.
                Assert.DoesNotContain("giantMushrooms", rules.Flora(42, patch, biome, 59));

                if (!magical.Contains("giantMushrooms"))
                {
                    Assert.Equal(plain, magical); // the rule changes nothing in a patch without them
                    continue;
                }
                withMushrooms++;
                Assert.DoesNotContain(magical, id => flora[id].Type == FloraType.Tree);
                Assert.Equal(plain.Count, magical.Count); // the place of the trees is filled
                // The other kinds of the patch stay; one gives way when the patch had no tree to replace.
                var others = plain.Where(id => flora[id].Type != FloraType.Tree).ToArray();
                Assert.InRange(others.Except(magical).Count(), 0, others.Length == plain.Count ? 1 : 0);
            }
        Assert.InRange(withMushrooms / (double)patches, 0.01, 0.10);

        // No biome outside the forests and the woodland has them.
        foreach (var biome in AllBiomes.Except(giant.Biomes))
            for (var k = 0; k < 200; k++)
                Assert.DoesNotContain("giantMushrooms", rules.Flora(42, new GridCoord(k, k * 2), biome, 100));
    }

    [Fact]
    public void A_stand_in_takes_the_place_of_the_kinds_of_its_type()
    {
        FloraParameters Kind(string id, FloraType type, double frequency = 1, FloraType? standsInFor = null) =>
            new(id, id, type, true, [Biome.Savanna]) { Frequency = frequency, StandsInFor = standsInFor };
        ParameterSet With(IReadOnlyList<FloraParameters> list, int perPatch) => ParameterSet.Default with
        {
            Flora = list,
            Elevation = ParameterSet.Default.Elevation with
            {
                Biomes = ParameterSet.Default.Elevation.Biomes with { FloraPerPatch = new(perPatch, perPatch) },
            },
        };

        // A patch with room for every kind: the stand-in is always taken, so no tree is.
        var all = new FloraFaunaRules(With([Kind("oak", FloraType.Tree), Kind("elm", FloraType.Tree), Kind("reed", FloraType.Grass), Kind("cap", FloraType.Arcane, 0.01, FloraType.Tree)], 4));
        for (var k = 0; k < 50; k++)
            Assert.Equal(["reed", "cap"], all.Flora(7, new GridCoord(k, 3 - k), Biome.Savanna, 0));

        // A patch with one kind has the trees or the stand-in, by the frequency: 1 of 5 here.
        var one = new FloraFaunaRules(With([Kind("oak", FloraType.Tree, 2), Kind("elm", FloraType.Tree, 2), Kind("cap", FloraType.Arcane, 1, FloraType.Tree)], 1));
        var caps = Enumerable.Range(0, 4000).Count(k => one.Flora(7, new GridCoord(k, -k * 3), Biome.Savanna, 0).Single() == "cap");
        Assert.InRange(caps / 4000.0, 0.17, 0.23);

        // A kind that names its own type stands in for nothing, and a stand-in is never left out.
        var own = new FloraFaunaRules(With([Kind("oak", FloraType.Tree), Kind("tall", FloraType.Tree, 1, FloraType.Tree)], 2));
        Assert.Equal(["oak", "tall"], own.Flora(7, new GridCoord(1, 1), Biome.Savanna, 0));
        var two = new FloraFaunaRules(With([Kind("a", FloraType.Arcane, 1, FloraType.Fungus), Kind("b", FloraType.Fungus, 1, FloraType.Arcane), Kind("cep", FloraType.Fungus)], 3));
        Assert.Equal(["a", "b"], two.Flora(7, new GridCoord(1, 1), Biome.Savanna, 0));
    }

    [Fact]
    public void Every_patch_of_a_region_has_the_flora_and_fauna_of_its_biome_in_both_forms()
    {
        var world = World.Create("cellgen");
        var p = world.Parameters;
        var flora = p.Flora.ToDictionary(f => f.Id);
        var fauna = p.Fauna.ToDictionary(f => f.Id);
        var id = new RegionId(0, 0);
        var detail = world.DetailOf(id);
        var elevation = world.ElevationOf(id);
        var land = 0;

        foreach (var patch in detail.Patches)
        {
            if (patch.Shape.Ocean)
            {
                Assert.All(patch.Flora, f => Assert.True(flora[f].Sea));
                Assert.All(patch.Fauna, f => Assert.True(fauna[f].Sea));
                Assert.NotEmpty(patch.Fauna);
                continue;
            }
            land++;
            var estimated = patch.Biome!.Biome;
            Assert.NotEmpty(patch.Flora);
            Assert.NotEmpty(patch.Fauna);
            Assert.All(patch.Flora, f => Assert.Contains(estimated, flora[f].Biomes));
            Assert.All(patch.Fauna, f => Assert.Contains(estimated, fauna[f].Biomes));

            var final = elevation.Find(patch.Root)!;
            Assert.All(final.Flora, f => Assert.Contains(final.Biome.Biome, flora[f].Biomes));
            Assert.All(final.Fauna, f => Assert.Contains(final.Biome.Biome, fauna[f].Biomes));
            if (final.Biome.Biome == estimated)
            {
                Assert.Equal(patch.Flora, final.Flora);
                Assert.Equal(patch.Fauna, final.Fauna);
            }
        }
        Assert.True(land > 0);

        // The domain form carries the same ids, and another world with the same foundation the same kinds.
        var domain = detail.ToDomain().Patches;
        Assert.Equal(detail.Patches.Select(x => x.Fauna), domain.Select(x => x.Fauna));
        var again = World.Create("cellgen").DetailOf(id);
        Assert.Equal(detail.Patches.SelectMany(x => x.Flora), again.Patches.SelectMany(x => x.Flora));
    }
}
