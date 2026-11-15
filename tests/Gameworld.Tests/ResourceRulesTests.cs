using Gameworld.Domain;
using Gameworld.Generation.Resources;

namespace Gameworld.Tests;

/// <summary>The patch resource rules of map-generation.md section 14 and Appendices B–D.</summary>
public class ResourceRulesTests
{
    private const uint Seed = 20261114;

    // A plausible climate per biome: T (after cooling), T₀ (sea level), P (mm).
    private static readonly (Biome Biome, double T, double T0, double P)[] Climates =
    [
        (Biome.IceAndSnow, -15, -12, 300),
        (Biome.Tundra, -6, -4, 300),
        (Biome.ColdSteppe, 0, 2, 100),
        (Biome.BorealForest, 0, 2, 400),
        (Biome.ColdDesert, 8, 10, 100),
        (Biome.TemperateGrassland, 10, 12, 250),
        (Biome.WoodlandAndShrubland, 14, 15, 600),
        (Biome.TemperateMixedForest, 8, 9, 700),
        (Biome.TemperateBroadleafForest, 12, 13, 800),
        (Biome.TemperateRainForest, 10, 11, 2500),
        (Biome.HotDesert, 26, 26, 150),
        (Biome.ThornScrub, 25, 25, 450),
        (Biome.Savanna, 25, 25, 900),
        (Biome.TropicalDryForest, 25, 26, 1700),
        (Biome.TropicalRainForest, 26, 26, 3000),
        (Biome.Mangrove, 26, 26, 2000),
    ];

    private static readonly string[] Bedrocks =
    [
        BedrockIds.Shield, BedrockIds.FoldBelt, BedrockIds.Granite, BedrockIds.Volcanic, BedrockIds.Basalt,
        BedrockIds.Ophiolite, BedrockIds.Carbonate, BedrockIds.Clastic, BedrockIds.Alluvium,
    ];

    private static readonly ElevationType[] LandTypes =
        [ElevationType.FlatLand, ElevationType.Lowland, ElevationType.Hills, ElevationType.Mountains];

    private static BiomeRecord Record(Biome biome, double t, double t0, double p)
    {
        var belt = t < -10 ? ThermalBelt.Polar
            : t < -3 ? ThermalBelt.Subpolar
            : t < 3 ? ThermalBelt.Boreal
            : t < 18 ? ThermalBelt.Temperate
            : ThermalBelt.Tropical;
        return new BiomeRecord(biome, belt, t, t0, p, p / Math.Max(40, 20 * t + 140));
    }

    private static BiomeRecord Record(Biome biome)
    {
        var c = Climates.Single(x => x.Biome == biome);
        return Record(c.Biome, c.T, c.T0, c.P);
    }

    private static LandResourceInput Land(
        BiomeRecord biome,
        ElevationType zoneType,
        string bedrock,
        bool coastal = false,
        bool riverPoor = false,
        WaterSite water = default,
        double prosperity = 0.5,
        GridCoord patch = default,
        GridCoord zone = default,
        double supernatural = 0) =>
        new(Seed, patch, zone, biome, zoneType, bedrock, coastal, riverPoor, water, prosperity, supernatural);

    private static WaterSite River(double river, double goldLoad = 0, double tinLoad = 0) =>
        new(river, 0, 0, false, goldLoad, tinLoad);

    private static IEnumerable<GridCoord> Coords(int count)
    {
        for (var n = 0; n < count; n++) yield return new GridCoord(n % 50 - 20, n / 50 - 7);
    }

    // ---- Tables -----------------------------------------------------------------------------

    [Fact]
    public void CropTable_Has108CropsNumberedInTheListedOrder()
    {
        Assert.Equal(108, CropTables.Count);
        Assert.Equal(108, CropTables.All.Count);
        for (var n = 0; n < CropTables.All.Count; n++) Assert.Equal(n, CropTables.All[n].Number);

        Assert.Equal(15, CropTables.Of(Resource.Grain).Count);
        Assert.Equal(18, CropTables.Of(Resource.Produce).Count);
        Assert.Equal(29, CropTables.Of(Resource.Orchard).Count);
        Assert.Equal(13, CropTables.Of(Resource.Fibre).Count);
        Assert.Equal(14, CropTables.Of(Resource.Spices).Count);
        Assert.Equal(9, CropTables.Of(Resource.StimulantsAndSugar).Count);
        Assert.Equal(10, CropTables.Of(Resource.Dyes).Count);
        Assert.Empty(CropTables.Of(Resource.Timber));

        // Grain first, dyes last, each table one unbroken run.
        var runs = new List<Resource>();
        foreach (var crop in CropTables.All)
            if (runs.Count == 0 || runs[^1] != crop.Resource) runs.Add(crop.Resource);
        Assert.Equal(CropTables.CropResources, runs);

        Assert.Equal("bread wheat", CropTables.All[0].Name);
        Assert.Equal("potato", CropTables.All[15].Name);
        Assert.Equal("olive", CropTables.All[33].Name);
        Assert.Equal("flax", CropTables.All[62].Name);
        Assert.Equal("black pepper", CropTables.All[75].Name);
        Assert.Equal("sugar cane", CropTables.All[89].Name);
        Assert.Equal("woad", CropTables.All[98].Name);
        Assert.Equal("cochineal", CropTables.All[107].Name);
        Assert.Equal(108, CropTables.All.Select(c => c.Name).Distinct().Count());
    }

    [Fact]
    public void CropTable_MatchesTheRowsQuotedInTheDoc()
    {
        var wheat = CropTables.All[0];
        Assert.Equal(new Trapezoid(5, 15, 23, 27), wheat.Temperature);
        Assert.Equal(new Trapezoid(300, 750, 900, 1600), wheat.Water);
        Assert.Equal(new Farmland(1, 1, 0.6, 0.3), wheat.Farmland);
        Assert.Null(wheat.MinT);
        Assert.Null(wheat.MaxT);
        Assert.False(wheat.Tree);
        Assert.Equal(1, wheat.K);

        var rice = CropTables.All.Single(c => c.Name == "paddy rice");
        Assert.Equal(new Trapezoid(10, 20, 30, 36), rice.Temperature);
        Assert.Equal(new Trapezoid(1000, 1500, 2000, 4000), rice.Water);
        Assert.Equal(new Farmland(1, 1, 0.35, 0), rice.Farmland);
        Assert.Equal(12, rice.MinT);

        var coca = CropTables.All.Single(c => c.Name == "coca");
        Assert.Equal(Resource.StimulantsAndSugar, coca.Resource);
        Assert.Equal(new Farmland(0, 0, 0.8, 1), coca.Farmland);
        Assert.True(coca.Tree);
        Assert.Equal(CropNeed.Highland, coca.Needs);
        Assert.Equal(0.15, coca.Gate);
        Assert.Equal(0.8, coca.K);

        var lentil = CropTables.All.Single(c => c.Name == "lentil");
        Assert.Equal(8, lentil.MinT);
        Assert.Equal(19, lentil.MaxT);

        Assert.Equal(CropNeed.River, CropTables.All.Single(c => c.Name == "date palm").Needs);
        Assert.Equal(CropNeed.Coast, CropTables.All.Single(c => c.Name == "coconut").Needs);
    }

    [Fact]
    public void CropTable_GatesOnlyTheLuxuryCropsAndSilk()
    {
        foreach (var crop in CropTables.All)
        {
            var luxury = crop.Resource is Resource.Spices or Resource.StimulantsAndSugar or Resource.Dyes;
            Assert.Equal(luxury || crop.Name == "silk", crop.IsGated);
            if (crop.IsGated) Assert.InRange(crop.Gate, 0.05, 0.3);
        }
        Assert.Equal(0.2, CropTables.All.Single(c => c.Name == "silk").Gate);
    }

    [Fact]
    public void CropTable_ValuesAreWellFormed()
    {
        foreach (var crop in CropTables.All)
        {
            foreach (var trap in new[] { crop.Temperature, crop.Water })
            {
                Assert.True(trap.Min < trap.OptMin, crop.Name);
                Assert.True(trap.OptMin <= trap.OptMax, crop.Name);
                Assert.True(trap.OptMax < trap.Max, crop.Name);
            }
            foreach (var type in LandTypes) Assert.InRange(crop.Farmland.Of(type), 0, 1);
            Assert.InRange(crop.K, 0.3, 1);
            if (crop.MinT is { } min && crop.MaxT is { } max) Assert.True(min < max, crop.Name);
        }
    }

    [Fact]
    public void BiomeFactors_FollowAppendixC()
    {
        foreach (var biome in Enum.GetValues<Biome>())
        {
            var row = BiomeFactors.Of(biome);
            Assert.InRange(row.Soil, 0, 1);
            Assert.InRange(row.TreeGround, 0, 1);
            Assert.InRange(row.Veg, 1, 2);
            Assert.InRange(row.RoadCost, 1, 5);
        }

        Assert.Equal(new BiomeFactorRow(0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1), BiomeFactors.Ocean);
        Assert.Equal(new BiomeFactorRow(0, 0, 0, 0.05, 0, 0, 0.25, 0, 0, 1.5, 5), BiomeFactors.Of(Biome.IceAndSnow));
        Assert.Equal(new BiomeFactorRow(1, 1, 1, 0.4, 0.1, 0, 0.9, 0.2, 0.4, 1, 1), BiomeFactors.Of(Biome.TemperateGrassland));
        Assert.Equal(new BiomeFactorRow(0.55, 1, 0.25, 0.7, 1, 0.85, 0.25, 0.2, 0.4, 1.3, 1.4), BiomeFactors.Of(Biome.BorealForest));
        Assert.Equal(new BiomeFactorRow(0.6, 1, 0.8, 0.6, 0.35, 0.4, 0.3, 0.2, 0.4, 1, 1.05), BiomeFactors.Of(Biome.Savanna));
        Assert.Equal(new BiomeFactorRow(0.3, 0.4, 0.1, 0.4, 0.5, 0, 0, 0.1, 0.3, 2, 3), BiomeFactors.Of(Biome.Mangrove));
    }

    [Theory]
    [InlineData(4, 0)]
    [InlineData(5, 0)]      // 0 outside the open interval (a, d)
    [InlineData(10, 0.5)]
    [InlineData(15, 1)]
    [InlineData(19, 1)]
    [InlineData(23, 1)]
    [InlineData(25, 0.5)]
    [InlineData(27, 0)]
    [InlineData(40, 0)]
    public void Trapezoid_IsZeroOutsideOneOnTheOptimumAndLinearBetween(double v, double expected)
    {
        Assert.Equal(expected, new Trapezoid(5, 15, 23, 27).At(v), 12);
    }

    // ---- Land rules -------------------------------------------------------------------------

    [Fact]
    public void Land_IsDeterministic()
    {
        var input = Land(Record(Biome.TemperateMixedForest), ElevationType.Hills, BedrockIds.FoldBelt,
            coastal: true, water: new WaterSite(12, 3, 4e6, true, 0.4, 0.2), prosperity: 0.8,
            patch: new GridCoord(17, -5), zone: new GridCoord(3, -1));

        var a = ResourceRules.Land(input);
        var b = ResourceRules.Land(input);

        Assert.Equal(a.Amounts, b.Amounts);
        Assert.Equal(a.Food, b.Food);
        Assert.Equal(a.Wealth, b.Wealth);
    }

    [Fact]
    public void Land_AmountsStayInRangeAndEveryAmountHasAVariant()
    {
        var water = new WaterSite(9, 3, 6e6, true, 0.5, 0.5);
        foreach (var climate in Climates)
        foreach (var type in LandTypes)
        foreach (var bedrock in Bedrocks)
        foreach (var patch in Coords(6))
        foreach (var wet in new[] { false, true })
        {
            var result = ResourceRules.Land(Land(Record(climate.Biome), type, bedrock,
                coastal: wet, water: wet ? water : default, prosperity: 0.9, patch: patch, zone: patch));

            Assert.Equal(ResourceCatalog.Count, result.Amounts.Count);
            foreach (var amount in result.Amounts)
            {
                Assert.InRange(amount.Amount, 0, 1);
                Assert.Equal(amount.Amount > 0, amount.Variant is not null);
            }
            Assert.InRange(result.Food, 0, 1);
            Assert.True(result.Wealth >= 0);
        }
    }

    [Fact]
    public void Land_TemperateHumidLowlandOnAFloodplain()
    {
        // T = 12, T₀ = 13, P = 800: m = 800 / 380 = 2.1, a broadleaf forest. tg = 12 + 5.5 = 17.5, season = 1.
        var result = ResourceRules.Land(Land(Record(Biome.TemperateBroadleafForest),
            ElevationType.Lowland, BedrockIds.Carbonate, water: River(3)));

        // Bread wheat: 17.5 °C is on [15, 23] and 800 mm on [750, 900], lowland farmland 1, soil 1.
        Assert.Equal(1, result.AmountOf(Resource.Grain), 12);
        Assert.Equal("bread wheat", result[Resource.Grain].Variant);
        // Potato: on [15, 25] and [500, 800], lowland 1, well below its max T of 17.
        Assert.Equal(1, result.AmountOf(Resource.Produce), 12);
        Assert.Equal("potato", result[Resource.Produce].Variant);

        Assert.Equal(1, result.AmountOf(Resource.Timber), 12);
        Assert.Contains(result[Resource.Timber].Variant, new[] { "oak", "ash", "elm" });

        // Clay: lowland 0.6 + 0.3 beside a river on low ground.
        Assert.Equal(0.9, result.AmountOf(Resource.Clay), 12);
        Assert.Equal("brick and pottery clay", result[Resource.Clay].Variant);
        Assert.Equal(ResourceOrigin.Potential, result[Resource.Clay].Origin);

        // Livestock: pasture 0.6 + 0.1 beside a river.
        Assert.Equal(0.7, result.AmountOf(Resource.Livestock), 12);
        Assert.Contains(result[Resource.Livestock].Variant, new[] { "pigs", "cattle" });

        // Fuel reads the timber: 0.9 × 1.
        Assert.Equal(0.9, result.AmountOf(Resource.Fuel), 12);
        Assert.Contains(result[Resource.Fuel].Variant, new[] { "firewood", "charcoal" });

        Assert.Equal(0.3, result.AmountOf(Resource.Fish), 12);
        Assert.Equal("river fish", result[Resource.Fish].Variant);

        // Wild food: game 0.8 + 0.15 on a floodplain.
        Assert.Equal(0.95, result.AmountOf(Resource.WildFoodAndGame), 12);
        Assert.Contains(result[Resource.WildFoodAndGame].Variant, new[] { "deer", "wild boar", "acorns" });

        // Honey is the only stimulant a zone always has.
        Assert.True(result.AmountOf(Resource.StimulantsAndSugar) >= 0.3);

        var food = Math.Clamp(0.75 * 1 + 0.2 * 0.7 + 0.1 * result.AmountOf(Resource.Orchard) + 0.05 * 0.95, 0, 1);
        Assert.Equal(food, result.Food, 12);
        Assert.True(result.Food > 0.93);
    }

    [Fact]
    public void Land_PolarPatchGetsEssentiallyNothing()
    {
        foreach (var patch in Coords(200))
        {
            var result = ResourceRules.Land(Land(Record(Biome.IceAndSnow),
                ElevationType.FlatLand, BedrockIds.Alluvium, patch: patch, zone: patch));

            // Stone is the one resource any land patch can roll (2 % here).
            foreach (var resource in ResourceCatalog.All)
                if (resource != Resource.Stone)
                    Assert.Equal(0, result.AmountOf(resource));
            Assert.Equal(0, result.Food);
            Assert.Equal(0, result.Wealth);
        }
    }

    [Fact]
    public void Land_ClasticBasinCanRollCoal()
    {
        // Cold desert hills without a river: no timber, dung fuel 0.35 · 0.18, so coal wins where it is present.
        var coal = 0;
        foreach (var patch in Coords(2000))
        {
            var fuel = ResourceRules.Land(Land(Record(Biome.ColdDesert), ElevationType.Hills, BedrockIds.Clastic,
                patch: patch))[Resource.Fuel];
            if (fuel.Variant != "coal") continue;
            coal++;
            Assert.Equal(ResourceOrigin.Deposit, fuel.Origin);
            Assert.InRange(fuel.Amount, 0.35, 1);
        }
        Assert.InRange(coal, 40, 130); // chance 0.04 of 2000

        foreach (var patch in Coords(2000))
        {
            var fuel = ResourceRules.Land(Land(Record(Biome.ColdDesert), ElevationType.Hills, BedrockIds.Shield,
                patch: patch))[Resource.Fuel];
            Assert.Equal("dung fuel", fuel.Variant);
            Assert.Equal(ResourceOrigin.Potential, fuel.Origin);
        }
    }

    [Fact]
    public void Land_StreamTinNeedsATinLoad()
    {
        int Count(double tinLoad, double river, string bedrock, double maxAmount)
        {
            var found = 0;
            foreach (var patch in Coords(2000))
            {
                var tin = ResourceRules.Land(Land(Record(Biome.TemperateGrassland), ElevationType.Lowland, bedrock,
                    water: River(river, tinLoad: tinLoad), patch: patch))[Resource.Tin];
                if (tin.Amount == 0) continue;
                found++;
                Assert.Equal("stream tin", tin.Variant);
                Assert.Equal(ResourceOrigin.Deposit, tin.Origin);
                Assert.InRange(tin.Amount, 0.35 * maxAmount, maxAmount);
            }
            return found;
        }

        Assert.Equal(0, Count(0, 3, BedrockIds.Shield, 1));
        Assert.InRange(Count(1, 3, BedrockIds.Shield, 1), 600, 800);        // chance 0.35
        Assert.InRange(Count(0.5, 3, BedrockIds.Shield, 0.7), 280, 420);    // chance 0.175, rich 0.4 + 0.3
        Assert.Equal(0, Count(1, 0.5, BedrockIds.Shield, 1));               // needs river ≥ 1
        Assert.Equal(0, Count(1, 3, BedrockIds.Clastic, 1));                // not on copper bedrock
    }

    [Fact]
    public void Land_RiversIrrigateTheDatePalm()
    {
        // Hot desert, 150 mm. Date palm water 100 / 200 / 300 / 400, needs a river.
        var dry = ResourceRules.Land(Land(Record(Biome.HotDesert), ElevationType.FlatLand, BedrockIds.Alluvium));
        var wet = ResourceRules.Land(Land(Record(Biome.HotDesert), ElevationType.FlatLand, BedrockIds.Alluvium,
            water: River(10)));

        // Without a river: trap(150) = 0.5, × 0.25 for the missing river.
        Assert.Equal(0.125, dry.AmountOf(Resource.Orchard), 12);
        Assert.Equal("date palm", dry[Resource.Orchard].Variant);
        // With one: 150 + 900 mm of irrigation, capped at the optimum minimum.
        Assert.Equal(1, wet.AmountOf(Resource.Orchard), 12);
        Assert.Equal("date palm", wet[Resource.Orchard].Variant);

        // Deserts have timber only along a river, and camels.
        Assert.Equal(0, dry.AmountOf(Resource.Timber));
        Assert.Equal(0.3, wet.AmountOf(Resource.Timber), 12);
        Assert.Equal("poplar", wet[Resource.Timber].Variant);
        Assert.Equal(0.5, dry.AmountOf(Resource.HorsesAndPackAnimals), 12);
        Assert.Equal("dromedary camel", dry[Resource.HorsesAndPackAnimals].Variant);
    }

    [Fact]
    public void Land_LuxuryCropsAreRegionalSpecialities()
    {
        var withSpices = 0;
        var poorWithSpices = 0;
        foreach (var zone in Coords(400))
        {
            var rich = ResourceRules.Land(Land(Record(Biome.TropicalRainForest), ElevationType.Lowland,
                BedrockIds.Shield, zone: zone));
            if (rich.AmountOf(Resource.Spices) > 0) withSpices++;

            // Boost 0 closes every boosted gate; honey still gives the stimulant.
            var poor = ResourceRules.Land(Land(Record(Biome.TropicalRainForest), ElevationType.Lowland,
                BedrockIds.Shield, prosperity: 0, zone: zone));
            if (poor.AmountOf(Resource.Spices) > 0) poorWithSpices++;
            Assert.Equal(0, poor.AmountOf(Resource.Dyes));
            Assert.Equal(0, poor.AmountOf(Resource.FursAndIvory));
            Assert.Equal(0.15, poor.AmountOf(Resource.StimulantsAndSugar), 12);
            Assert.Equal("honey and beeswax", poor[Resource.StimulantsAndSugar].Variant);
        }

        Assert.InRange(withSpices, 40, 360); // some zones, not all
        Assert.Equal(0, poorWithSpices);
    }

    [Fact]
    public void Land_FursPassAZoneGate()
    {
        var gated = 0;
        foreach (var zone in Coords(1000))
        {
            var furs = ResourceRules.Land(Land(Record(Biome.BorealForest), ElevationType.Lowland,
                BedrockIds.Shield, zone: zone))[Resource.FursAndIvory];
            if (furs.Amount == 0) continue;
            gated++;
            Assert.Equal(0.85, furs.Amount, 12);
            Assert.Contains(furs.Variant, new[] { "sable and marten", "squirrel", "ermine" });
        }
        Assert.InRange(gated, 420, 580); // half of the zones outside the tropical belt
    }

    [Fact]
    public void Land_FishTakesTheBestOfCoastRiverAndLake()
    {
        var broadleaf = Record(Biome.TemperateBroadleafForest);

        var open = ResourceRules.Land(Land(broadleaf, ElevationType.Lowland, BedrockIds.Shield, coastal: true));
        Assert.Equal(0.8, open.AmountOf(Resource.Fish), 12);
        Assert.Contains(open[Resource.Fish].Variant, new[] { "sardine and anchovy", "oysters and mussels" });

        var harbour = ResourceRules.Land(Land(broadleaf, ElevationType.Lowland, BedrockIds.Shield, coastal: true,
            water: new WaterSite(0, 0, 0, true, 0, 0)));
        Assert.Equal(1, harbour.AmountOf(Resource.Fish), 12);

        var mangrove = ResourceRules.Land(Land(Record(Biome.Mangrove), ElevationType.FlatLand, BedrockIds.Alluvium,
            coastal: true));
        Assert.Equal(0.9, mangrove.AmountOf(Resource.Fish), 12);
        Assert.Equal("crabs, lobsters and shrimp", mangrove[Resource.Fish].Variant);

        // A 10 km² lake: 0.45 + 0.3 · 0.5, above the 0.3 of its small river.
        var lake = ResourceRules.Land(Land(broadleaf, ElevationType.Lowland, BedrockIds.Shield,
            water: new WaterSite(2, 0, 10e6, false, 0, 0)));
        Assert.Equal(0.6, lake.AmountOf(Resource.Fish), 12);
        Assert.Equal("lake fish", lake[Resource.Fish].Variant);

        var largeRiver = ResourceRules.Land(Land(broadleaf, ElevationType.Lowland, BedrockIds.Shield, water: River(90)));
        Assert.Equal(0.7, largeRiver.AmountOf(Resource.Fish), 12);
        Assert.Equal("sturgeon", largeRiver[Resource.Fish].Variant);
    }

    [Fact]
    public void Land_StoneFollowsElevationTypeAndBedrock()
    {
        // Carbonate mountains: chance 0.95 × 2.5, so always present; richness 1 · (0.35 … 1).
        foreach (var patch in Coords(300))
        {
            var stone = ResourceRules.Land(Land(Record(Biome.BorealForest), ElevationType.Mountains,
                BedrockIds.Carbonate, patch: patch))[Resource.Stone];
            Assert.InRange(stone.Amount, 0.35, 1);
            Assert.Contains(stone.Variant, new[] { "limestone", "marble" });
            Assert.Equal(ResourceOrigin.Deposit, stone.Origin);
        }

        // Flat cover: chance 0.04 × 0.5, richness 0.35 · (0.35 … 1).
        var found = 0;
        foreach (var patch in Coords(2000))
        {
            var stone = ResourceRules.Land(Land(Record(Biome.TemperateGrassland), ElevationType.FlatLand,
                BedrockIds.Alluvium, patch: patch))[Resource.Stone];
            if (stone.Amount == 0) continue;
            found++;
            Assert.Equal("field stone and boulders", stone.Variant);
            Assert.InRange(stone.Amount, 0.35 * 0.35, 0.35);
        }
        Assert.InRange(found, 15, 75);
    }

    [Fact]
    public void Land_ClasticBasinCanRollGypsum()
    {
        // Flat clastic basin: kind 1 gives sandstone at 0.04 × 1.6 with richness up to 0.35; gypsum (kind 52)
        // at 0.04 with richness 0.8 · (0.35 … 1), so it wins nearly everywhere it is present.
        var gypsum = 0;
        foreach (var patch in Coords(2000))
        {
            var stone = ResourceRules.Land(Land(Record(Biome.TemperateGrassland), ElevationType.FlatLand,
                BedrockIds.Clastic, patch: patch))[Resource.Stone];
            if (stone.Variant != "gypsum and alabaster") continue;
            gypsum++;
            Assert.Equal(ResourceOrigin.Deposit, stone.Origin);
            Assert.InRange(stone.Amount, 0.8 * 0.35, 0.8);
        }
        Assert.InRange(gypsum, 45, 125); // chance 0.04 of 2000

        foreach (var patch in Coords(500))
        {
            Assert.NotEqual("gypsum and alabaster", ResourceRules.Land(Land(Record(Biome.TemperateGrassland),
                ElevationType.Mountains, BedrockIds.Clastic, patch: patch))[Resource.Stone].Variant);
            Assert.NotEqual("gypsum and alabaster", ResourceRules.Land(Land(Record(Biome.TemperateGrassland),
                ElevationType.FlatLand, BedrockIds.Carbonate, patch: patch))[Resource.Stone].Variant);
        }
    }

    [Fact]
    public void Land_ReagentsLieOnTheirBedrock()
    {
        // Volcanic highlands: alum 0.06, sulphur 0.05, quicksilver 0.015, so about 12 % of the patches.
        var found = 0;
        foreach (var patch in Coords(2000))
        {
            var reagents = ResourceRules.Land(Land(Record(Biome.TemperateMixedForest), ElevationType.Mountains,
                BedrockIds.Volcanic, patch: patch))[Resource.Reagents];
            if (reagents.Amount == 0) continue;
            found++;
            Assert.Contains(reagents.Variant, new[] { "alum", "sulphur", "quicksilver" });
            Assert.Equal(ResourceOrigin.Deposit, reagents.Origin);
            Assert.InRange(reagents.Amount, 0.35, 1);
        }
        Assert.InRange(found, 180, 300);

        var shale = 0;
        var cave = 0;
        foreach (var patch in Coords(2000))
        {
            Assert.Equal(0, ResourceRules.Land(Land(Record(Biome.TemperateMixedForest), ElevationType.Mountains,
                BedrockIds.Shield, patch: patch)).AmountOf(Resource.Reagents));
            // The low volcanic ground has none either.
            Assert.Equal(0, ResourceRules.Land(Land(Record(Biome.TemperateMixedForest), ElevationType.Lowland,
                BedrockIds.Volcanic, patch: patch)).AmountOf(Resource.Reagents));

            var clastic = ResourceRules.Land(Land(Record(Biome.TemperateMixedForest), ElevationType.Lowland,
                BedrockIds.Clastic, patch: patch))[Resource.Reagents];
            if (clastic.Amount > 0)
            {
                shale++;
                Assert.Equal("alum shale and vitriol", clastic.Variant);
                Assert.InRange(clastic.Amount, 0.7 * 0.35, 0.7);
            }

            var carbonate = ResourceRules.Land(Land(Record(Biome.TemperateMixedForest), ElevationType.Hills,
                BedrockIds.Carbonate, patch: patch))[Resource.Reagents];
            if (carbonate.Amount > 0)
            {
                cave++;
                Assert.Equal("cave saltpetre", carbonate.Variant);
            }
        }
        Assert.InRange(shale, 30, 100); // chance 0.03 of 2000
        Assert.InRange(cave, 30, 100);
    }

    [Fact]
    public void Land_SaltpetreSharesTheZoneGateOfThePlayaSalt()
    {
        // Hot desert, 150 mm: m = 150 / 660 < 0.5. The salt and the reagents pass the same 8 % gate.
        var gated = 0;
        foreach (var zone in Coords(1000))
        {
            var saltHere = false;
            var reagentsHere = false;
            foreach (var patch in Coords(60))
            {
                var result = ResourceRules.Land(Land(Record(Biome.HotDesert), ElevationType.FlatLand,
                    BedrockIds.Alluvium, riverPoor: true, patch: patch, zone: zone));
                saltHere |= result.AmountOf(Resource.Salt) > 0;
                var reagents = result[Resource.Reagents];
                if (reagents.Amount == 0) continue;
                reagentsHere = true;
                Assert.Contains(reagents.Variant, new[] { "saltpetre", "borax" });
            }
            Assert.Equal(saltHere, reagentsHere); // 60 patches at 0.3 and at 0.15: both show where the gate is open
            if (saltHere) gated++;
        }
        Assert.InRange(gated, 50, 115);
    }

    [Fact]
    public void Land_HerbsFollowTheBiomeAndTheHeight()
    {
        var shrubland = ResourceRules.Land(Land(Record(Biome.WoodlandAndShrubland), ElevationType.Lowland, BedrockIds.Shield));
        Assert.Equal(0.8, shrubland.AmountOf(Resource.Herbs), 12);
        Assert.Contains(shrubland[Resource.Herbs].Variant, new[] { "sage", "thyme and rosemary", "poppy" });
        Assert.Equal(ResourceOrigin.Potential, shrubland[Resource.Herbs].Origin);

        // + 0.15 in hills and mountains.
        var hills = ResourceRules.Land(Land(Record(Biome.WoodlandAndShrubland), ElevationType.Hills, BedrockIds.Shield));
        Assert.Equal(0.95, hills.AmountOf(Resource.Herbs), 12);
        Assert.Contains(hills[Resource.Herbs].Variant, new[] { "sage", "thyme and rosemary", "poppy" });

        var mountains = ResourceRules.Land(Land(Record(Biome.TemperateBroadleafForest), ElevationType.Mountains, BedrockIds.Shield));
        Assert.Equal(0.75, mountains.AmountOf(Resource.Herbs), 12);
        Assert.Contains(mountains[Resource.Herbs].Variant, new[] { "gentian", "arnica" });

        // A desert keeps its own herbs in the mountains, and reaches the listing only there.
        var desert = ResourceRules.Land(Land(Record(Biome.HotDesert), ElevationType.Mountains, BedrockIds.Shield));
        Assert.Equal(0.25, desert.AmountOf(Resource.Herbs), 12);
        Assert.Contains(desert[Resource.Herbs].Variant, new[] { "aloe", "senna" });

        foreach (var type in LandTypes)
            Assert.Equal(0, ResourceRules.Land(Land(Record(Biome.IceAndSnow), type, BedrockIds.Shield)).AmountOf(Resource.Herbs));

        // Every other land biome has herbs, with a variant.
        foreach (var climate in Climates)
        {
            if (climate.Biome == Biome.IceAndSnow) continue;
            var herbs = ResourceRules.Land(Land(Record(climate.Biome), ElevationType.Lowland, BedrockIds.Shield))[Resource.Herbs];
            Assert.True(herbs.Amount > 0);
            Assert.NotNull(herbs.Variant);
        }
    }

    [Fact]
    public void Land_PackAnimalsOfTheColdAndOfTheTropicalForests()
    {
        var tundra = ResourceRules.Land(Land(Record(Biome.Tundra), ElevationType.Lowland, BedrockIds.Shield));
        Assert.Equal(0.3, tundra.AmountOf(Resource.HorsesAndPackAnimals), 12);
        Assert.Contains(tundra[Resource.HorsesAndPackAnimals].Variant, new[] { "reindeer", "sled dogs" });

        var boreal = ResourceRules.Land(Land(Record(Biome.BorealForest), ElevationType.Lowland, BedrockIds.Shield));
        Assert.Equal(0.25, boreal.AmountOf(Resource.HorsesAndPackAnimals), 12);
        Assert.Equal("reindeer", boreal[Resource.HorsesAndPackAnimals].Variant);

        // Ice: sled dogs on the coast only.
        var inland = ResourceRules.Land(Land(Record(Biome.IceAndSnow), ElevationType.FlatLand, BedrockIds.Shield));
        Assert.Equal(0, inland.AmountOf(Resource.HorsesAndPackAnimals));
        var coast = ResourceRules.Land(Land(Record(Biome.IceAndSnow), ElevationType.FlatLand, BedrockIds.Shield, coastal: true));
        Assert.Equal(0.25, coast.AmountOf(Resource.HorsesAndPackAnimals), 12);
        Assert.Equal("sled dogs", coast[Resource.HorsesAndPackAnimals].Variant);

        var rainForest = ResourceRules.Land(Land(Record(Biome.TropicalRainForest), ElevationType.Lowland, BedrockIds.Shield));
        Assert.Equal(0.25, rainForest.AmountOf(Resource.HorsesAndPackAnimals), 12);
        Assert.Equal("elephants", rainForest[Resource.HorsesAndPackAnimals].Variant);

        var dryForest = ResourceRules.Land(Land(Record(Biome.TropicalDryForest), ElevationType.FlatLand, BedrockIds.Shield));
        Assert.Equal(0.3, dryForest.AmountOf(Resource.HorsesAndPackAnimals), 12);
        Assert.Equal("elephants", dryForest[Resource.HorsesAndPackAnimals].Variant);

        // The high ground keeps its mules: mounts 0.3 × 0.7 against 0.3 × pasture 0.45.
        var dryHills = ResourceRules.Land(Land(Record(Biome.TropicalDryForest), ElevationType.Hills, BedrockIds.Shield));
        Assert.Equal(0.21, dryHills.AmountOf(Resource.HorsesAndPackAnimals), 12);
        Assert.Equal("donkeys and mules", dryHills[Resource.HorsesAndPackAnimals].Variant);

        var grassland = ResourceRules.Land(Land(Record(Biome.TemperateGrassland), ElevationType.Lowland, BedrockIds.Shield));
        Assert.Equal("horses", grassland[Resource.HorsesAndPackAnimals].Variant);
    }

    [Fact]
    public void Power_IsZeroUpToSixtyAndOneFromNinety()
    {
        Assert.Equal(0, ResourceRules.Power(0));
        Assert.Equal(0, ResourceRules.Power(60));
        Assert.Equal(0.5, ResourceRules.Power(75), 12);
        Assert.Equal(1, ResourceRules.Power(90), 12);
        Assert.Equal(1, ResourceRules.Power(100));
    }

    [Fact]
    public void Land_ArcaneMaterialsNeedTheSupernaturalField()
    {
        var water = new WaterSite(9, 3, 6e6, true, 0.5, 0.5);

        // At a Supernatural of 60 and below there is none, whatever the patch.
        foreach (var supernatural in new[] { 0.0, 30, 60 })
        foreach (var climate in Climates)
        foreach (var type in LandTypes)
        foreach (var bedrock in Bedrocks)
        foreach (var patch in Coords(4))
        {
            var result = ResourceRules.Land(Land(Record(climate.Biome), type, bedrock, coastal: true, water: water,
                prosperity: 0.2, patch: patch, zone: patch, supernatural: supernatural));
            Assert.Equal(0, result.AmountOf(Resource.ArcaneMaterials));
        }

        // Cold desert on shield lowland: star metal is the only kind that fits, at 0.4 × power.
        int StarMetal(double supernatural)
        {
            var found = 0;
            foreach (var patch in Coords(2000))
            {
                var arcane = ResourceRules.Land(Land(Record(Biome.ColdDesert), ElevationType.Lowland, BedrockIds.Shield,
                    patch: patch, supernatural: supernatural))[Resource.ArcaneMaterials];
                if (arcane.Amount == 0) continue;
                found++;
                Assert.Equal("star metal", arcane.Variant);
                Assert.Equal(ResourceOrigin.Deposit, arcane.Origin);
                Assert.InRange(arcane.Amount, 0.35, 1);
            }
            return found;
        }

        Assert.InRange(StarMetal(100), 720, 880);  // chance 0.4
        Assert.InRange(StarMetal(75), 340, 460);   // chance 0.2
        Assert.Equal(0, StarMetal(60));

        // The prosperity boost does not apply: poor land has as much.
        var poor = 0;
        foreach (var patch in Coords(2000))
            if (ResourceRules.Land(Land(Record(Biome.ColdDesert), ElevationType.Lowland, BedrockIds.Shield,
                    prosperity: 0, patch: patch, supernatural: 100)).AmountOf(Resource.ArcaneMaterials) > 0)
                poor++;
        Assert.Equal(StarMetal(100), poor);
    }

    [Fact]
    public void Land_ArcaneKindsLieWhereTheirConditionsHold()
    {
        string[] Variants(BiomeRecord biome, ElevationType type, string bedrock, bool coastal = false,
            WaterSite water = default, double prosperity = 0.5)
        {
            var found = new SortedSet<string>(StringComparer.Ordinal);
            foreach (var patch in Coords(1500))
            {
                var arcane = ResourceRules.Land(Land(biome, type, bedrock, coastal: coastal, water: water,
                    prosperity: prosperity, patch: patch, supernatural: 100))[Resource.ArcaneMaterials];
                if (arcane.Amount > 0) found.Add(arcane.Variant!);
            }
            return [.. found];
        }

        Assert.Equal(new[] { "truesilver" }, Variants(Record(Biome.TemperateGrassland), ElevationType.Mountains, BedrockIds.Granite));
        Assert.Equal(new[] { "stormglass" }, Variants(Record(Biome.TemperateGrassland), ElevationType.Lowland, BedrockIds.Shield));
        Assert.Empty(Variants(Record(Biome.TemperateGrassland), ElevationType.Hills, BedrockIds.Shield));

        // Broadleaf lowland, T = 12: the sap of Elanor, and elderwood only where the prosperity is low.
        Assert.Equal(new[] { "sap of Elanor" }, Variants(Record(Biome.TemperateBroadleafForest), ElevationType.Lowland, BedrockIds.Shield));
        Assert.Equal(new[] { "elderwood", "sap of Elanor" },
            Variants(Record(Biome.TemperateBroadleafForest), ElevationType.Lowland, BedrockIds.Shield, prosperity: 0.2));

        // Hot desert at 26 °C: star metal and sunstone. Clastic hills add dragonbone (m < 1).
        Assert.Equal(new[] { "star metal", "sunstone" },
            Variants(Record(Biome.HotDesert), ElevationType.Lowland, BedrockIds.Shield));
        Assert.Contains("dragonbone", Variants(Record(Biome.HotDesert), ElevationType.Hills, BedrockIds.Clastic));

        // Tundra: the frost-lotus needs water; in subpolar mountains there is windcrystal.
        Assert.Empty(Variants(Record(Biome.Tundra), ElevationType.Lowland, BedrockIds.Shield));
        Assert.Equal(new[] { "frost-lotus" }, Variants(Record(Biome.Tundra), ElevationType.Lowland, BedrockIds.Shield, water: River(2)));
        Assert.Equal(new[] { "windcrystal" }, Variants(Record(Biome.Tundra), ElevationType.Mountains, BedrockIds.Shield));

        // Rain forest at T₀ = 26: moonpearls on the coast.
        Assert.Empty(Variants(Record(Biome.TropicalRainForest), ElevationType.Lowland, BedrockIds.Shield));
        Assert.Equal(new[] { "moonpearls" }, Variants(Record(Biome.TropicalRainForest), ElevationType.Lowland, BedrockIds.Shield, coastal: true));

        // Wet volcanic and carbonate highlands (rain forest, m > 2).
        Assert.Equal(new[] { "mana crystal" }, Variants(Record(Biome.TropicalRainForest), ElevationType.Hills, BedrockIds.Volcanic));
        Assert.Equal(new[] { "glowcap fungi" }, Variants(Record(Biome.TropicalRainForest), ElevationType.Hills, BedrockIds.Carbonate));
    }

    [Fact]
    public void Ocean_HasNoneOfTheThreeNewResources()
    {
        foreach (var patch in Coords(300))
        foreach (var onShelf in new[] { false, true })
        {
            var result = ResourceRules.Ocean(new OceanResourceInput(Seed, patch, onShelf, 24, 1));
            Assert.Equal(ResourceCatalog.Count, result.Amounts.Count);
            Assert.Equal(0, result.AmountOf(Resource.Reagents));
            Assert.Equal(0, result.AmountOf(Resource.Herbs));
            Assert.Equal(0, result.AmountOf(Resource.ArcaneMaterials));
        }
    }

    [Fact]
    public void Land_SummariesFollowTheFormulas()
    {
        var water = new WaterSite(9, 3, 6e6, true, 0.8, 0.8);
        var wealthy = 0;
        foreach (var climate in Climates)
        foreach (var type in LandTypes)
        foreach (var bedrock in Bedrocks)
        {
            var result = ResourceRules.Land(Land(Record(climate.Biome), type, bedrock, coastal: true, water: water,
                prosperity: 1, patch: new GridCoord((int)type, 7), zone: new GridCoord(2, (int)climate.Biome)));

            var food = Math.Clamp(
                0.75 * Math.Max(result.AmountOf(Resource.Grain), 0.8 * result.AmountOf(Resource.Produce))
                + 0.2 * result.AmountOf(Resource.Livestock)
                + 0.1 * result.AmountOf(Resource.Orchard)
                + 0.05 * result.AmountOf(Resource.WildFoodAndGame), 0, 1);
            Assert.Equal(food, result.Food, 12);

            var wealth = result.Listed.Sum(x => ResourceRules.TradeValue(x.Resource) * x.Amount.Amount);
            Assert.Equal(wealth, result.Wealth, 12);
            if (result.Wealth > 0) wealthy++;
        }
        Assert.True(wealthy > 100);

        Assert.Equal(1, ResourceRules.TradeValue(Resource.Gold));
        Assert.Equal(0.9, ResourceRules.TradeValue(Resource.Tin));
        Assert.Equal(0.9, ResourceRules.TradeValue(Resource.ArcaneMaterials));
        Assert.Equal(0.5, ResourceRules.TradeValue(Resource.Reagents));
        Assert.Equal(0, ResourceRules.TradeValue(Resource.Herbs));
        Assert.Equal(0.7, ResourceRules.TradeValue(Resource.Salt));
        Assert.Equal(0.1, ResourceRules.TradeValue(Resource.Fibre));
        Assert.Equal(0, ResourceRules.TradeValue(Resource.Grain));
        Assert.Equal(0, ResourceRules.TradeValue(Resource.Fuel));
    }

    [Fact]
    public void Boost_IsTheSquareOfTwiceTheProsperity()
    {
        Assert.Equal(0.25, ResourceRules.Boost(0.25), 12);
        Assert.Equal(1, ResourceRules.Boost(0.5), 12);
        Assert.Equal(4, ResourceRules.Boost(1), 12);
    }

    // ---- Ocean rules ------------------------------------------------------------------------

    [Theory]
    [InlineData(true, 2, 0.9, "cod banks")]
    [InlineData(true, 8, 0.9, "herring")]
    [InlineData(true, 15, 0.9, "sardine and anchovy")]
    [InlineData(true, 24, 0.9, "reef fish and shellfish")]
    [InlineData(false, 2, 0.5, "cod banks")]
    [InlineData(false, 12, 0.5, "herring")]
    [InlineData(false, 20, 0.5, "tuna and large pelagics")]
    public void Ocean_FishByShelfAndTemperature(bool onShelf, double t0, double amount, string variant)
    {
        var result = ResourceRules.Ocean(new OceanResourceInput(Seed, new GridCoord(4, 9), onShelf, t0, 0.5));

        Assert.Equal(amount, result.AmountOf(Resource.Fish));
        Assert.Equal(variant, result[Resource.Fish].Variant);
        Assert.Equal(ResourceOrigin.Potential, result[Resource.Fish].Origin);
    }

    [Fact]
    public void Ocean_ShelfExtrasFollowTheTemperature()
    {
        int seals = 0, whales = 0, pearls = 0, coral = 0;
        foreach (var patch in Coords(2000))
        {
            var cold = ResourceRules.Ocean(new OceanResourceInput(Seed, patch, true, 1, 0.5));
            Assert.Equal(0.3, cold.AmountOf(Resource.WildFoodAndGame));
            Assert.Equal("edible seaweed and kelp", cold[Resource.WildFoodAndGame].Variant);
            Assert.Equal(0, cold.AmountOf(Resource.GemsAndPearls));
            Assert.Equal(0.05 * 0.3, cold.Food, 12);
            if (cold.AmountOf(Resource.FursAndIvory) > 0)
            {
                seals++;
                Assert.Equal(0.4, cold.AmountOf(Resource.FursAndIvory));
                Assert.Equal("seals", cold[Resource.FursAndIvory].Variant);
                Assert.Equal(0.35 * 0.4, cold.Wealth, 12);
            }

            var deep = ResourceRules.Ocean(new OceanResourceInput(Seed, patch, false, 1, 0.5));
            Assert.Equal(0, deep.AmountOf(Resource.WildFoodAndGame));
            if (deep.AmountOf(Resource.FursAndIvory) > 0)
            {
                whales++;
                Assert.Equal(0.3, deep.AmountOf(Resource.FursAndIvory));
                Assert.Equal("whales", deep[Resource.FursAndIvory].Variant);
            }

            var warm = ResourceRules.Ocean(new OceanResourceInput(Seed, patch, true, 24, 0.5))[Resource.GemsAndPearls];
            Assert.Equal(0, ResourceRules.Ocean(new OceanResourceInput(Seed, patch, true, 24, 0.5)).AmountOf(Resource.WildFoodAndGame));
            if (warm.Amount > 0)
            {
                pearls++;
                Assert.Equal("pearl oyster", warm.Variant);
                Assert.Equal(ResourceOrigin.Deposit, warm.Origin);
                Assert.InRange(warm.Amount, 0.35, 1);
            }

            var mild = ResourceRules.Ocean(new OceanResourceInput(Seed, patch, true, 15, 0.5))[Resource.GemsAndPearls];
            if (mild.Amount > 0)
            {
                coral++;
                Assert.Equal("precious red coral", mild.Variant);
                Assert.Equal(ResourceOrigin.Deposit, mild.Origin);
            }

            Assert.Equal(0, ResourceRules.Ocean(new OceanResourceInput(Seed, patch, false, 24, 0.5)).AmountOf(Resource.GemsAndPearls));
        }

        Assert.InRange(seals, 900, 1100);  // fur gate 0.5
        Assert.Equal(seals, whales);       // the same gate hash for the same patch
        Assert.InRange(pearls, 60, 145);   // chance 0.05
        Assert.InRange(coral, 30, 95);     // chance 0.03
    }

    [Fact]
    public void Ocean_IsDeterministic()
    {
        var input = new OceanResourceInput(Seed, new GridCoord(-3, 12), true, 22, 0.9);

        Assert.Equal(ResourceRules.Ocean(input).Amounts, ResourceRules.Ocean(input).Amounts);
    }
}
