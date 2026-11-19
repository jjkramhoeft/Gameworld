using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gameworld.Domain;

// Every parameter carries a [Description]: what it does, its unit and the section of
// docs/map-generation.md or docs/society-generation.md it belongs to. docs/parameter-set.schema.json is generated from these
// (ParameterSetSchema), so a description is written once, here.

/// <summary>
/// The set of all map and world parameters, grouped by region generation stage. Referenced by
/// a foundation and immutable after phase 1 like the foundation. The defaults are the same as
/// docs/default-parameter-set.json and the text of docs/map-generation.md.
/// </summary>
[Description("The set of all map and world parameters of a foundation, grouped by region generation stage. A parameter that is left out keeps its default. Section numbers refer to docs/map-generation.md, and those marked society to docs/society-generation.md.")]
public sealed record ParameterSet
{
    [Description("Sited stage: the three levels of cells, the ocean seeds and the elevation types (§3, §4, §5.1, §8).")]
    public SitingParameters Siting { get; init; } = new();

    [Description("Outlined stage: ocean links, ocean expansion and zone mergers (§5.2, §5.3, §6.1).")]
    public OutlineParameters Outline { get; init; } = new();

    [Description("Detailed stage: patch mergers, rivers and lakes (§6.3, §10, §11).")]
    public DetailParameters Detail { get; init; } = new();

    [Description("Elevated stage: the height map and the biomes (§12, §13).")]
    public ElevationParameters Elevation { get; init; } = new();

    [Description("Planned stage: Prospects for cities and towns, which of them are on show (§15), the potential population (society §3) and the factions (society §4). The Settled stage has no parameters of its own: the travel network connects the Prospects on show (§16).")]
    public PlanningParameters Planning { get; init; } = new();

    [Description("The four Fields: noise maps that drive climate, biomes and resources. They have no stage (§9).")]
    public FieldsParameters Fields { get; init; } = new();

    [Description("The bedrock types and their relative weight under each elevation type. The bedrock of a zone is drawn by these weights (§14.1). The resource rules know the nine ids of the defaults; a bedrock with another id gives no bedrock-bound resources.")]
    public IReadOnlyList<BedrockParameters> Bedrocks { get; init; } = BedrockParameters.Defaults;

    [Description("The races: the peoples that can live in the world. Add a race by adding an entry, and switch a race on or off with Enabled. The potential population of a region is shared between the races that are on, by their frequency in the biome of each patch and their height modifier at its height (society §2 and §3.2). A file that gives this list replaces the whole default list.")]
    public IReadOnlyList<RaceParameters> Races { get; init; } = RaceParameters.Defaults;

    [Description("The vocations: the work a person does, in nine sectors. Add a vocation by adding an entry, and switch one on or off with Enabled. The vocations are not drawn for places or persons yet (society §8). A file that gives this list replaces the whole default list.")]
    public IReadOnlyList<VocationParameters> Vocations { get; init; } = VocationParameters.Defaults;

    [Description("The flora: the plants that can grow in the world. Every patch gets a few of the kinds that fit its biome (§13.4). A file that gives this list replaces the whole default list.")]
    public IReadOnlyList<FloraParameters> Flora { get; init; } = FloraParameters.Defaults;

    [Description("The fauna: the creatures that can live in the world. Every patch gets a few of the kinds that fit its biome (§13.4). A file that gives this list replaces the whole default list.")]
    public IReadOnlyList<FaunaParameters> Fauna { get; init; } = FaunaParameters.Defaults;

    public static ParameterSet Default => new();

    internal static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        IndentSize = 2,
        NewLine = "\n",
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true,
        Converters = { new JsonStringEnumConverter() },
    };

    public string ToJson() => JsonSerializer.Serialize(this, JsonOptions);

    public static ParameterSet FromJson(string json) =>
        JsonSerializer.Deserialize<ParameterSet>(json, JsonOptions)
        ?? throw new JsonException("The parameter set json is empty.");

    public void Save(string path) => File.WriteAllText(path, ToJson() + "\n");

    public static ParameterSet Load(string path) => FromJson(File.ReadAllText(path));
}

/// <summary>An inclusive integer range a count is drawn from.</summary>
public sealed record IntRange(
    [property: Description("Smallest count, inclusive.")] int Min,
    [property: Description("Largest count, inclusive.")] int Max);

/// <summary>One value per land elevation type.</summary>
public sealed record PerElevationType<T>(
    [property: Description("Value for Flat Land zones.")] T FlatLand,
    [property: Description("Value for Lowland zones.")] T Lowland,
    [property: Description("Value for Hills zones.")] T Hills,
    [property: Description("Value for Mountains zones.")] T Mountains)
{
    public T this[ElevationType type] => type switch
    {
        ElevationType.FlatLand => FlatLand,
        ElevationType.Lowland => Lowland,
        ElevationType.Hills => Hills,
        ElevationType.Mountains => Mountains,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Only land elevation types carry a value."),
    };
}

/// <summary>
/// One value per biome. In json a biome whose value is the default of the type (0 for a number)
/// is left out, and a biome that is left out reads as that default.
/// </summary>
public sealed record PerBiome<T>(
    [property: Description("Value for Ice & snow patches."), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] T IceAndSnow,
    [property: Description("Value for Tundra patches."), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] T Tundra,
    [property: Description("Value for Cold steppe patches."), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] T ColdSteppe,
    [property: Description("Value for Boreal forest patches."), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] T BorealForest,
    [property: Description("Value for Cold desert patches."), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] T ColdDesert,
    [property: Description("Value for Temperate grassland patches."), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] T TemperateGrassland,
    [property: Description("Value for Woodland & shrubland patches."), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] T WoodlandAndShrubland,
    [property: Description("Value for Temperate mixed forest patches."), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] T TemperateMixedForest,
    [property: Description("Value for Temperate broadleaf forest patches."), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] T TemperateBroadleafForest,
    [property: Description("Value for Temperate rain forest patches."), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] T TemperateRainForest,
    [property: Description("Value for Hot desert patches."), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] T HotDesert,
    [property: Description("Value for Thorn scrub patches."), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] T ThornScrub,
    [property: Description("Value for Savanna patches."), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] T Savanna,
    [property: Description("Value for Tropical dry forest patches."), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] T TropicalDryForest,
    [property: Description("Value for Tropical rain forest patches."), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] T TropicalRainForest,
    [property: Description("Value for Mangrove patches."), JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)] T Mangrove)
{
    public T this[Biome biome] => biome switch
    {
        Biome.IceAndSnow => IceAndSnow,
        Biome.Tundra => Tundra,
        Biome.ColdSteppe => ColdSteppe,
        Biome.BorealForest => BorealForest,
        Biome.ColdDesert => ColdDesert,
        Biome.TemperateGrassland => TemperateGrassland,
        Biome.WoodlandAndShrubland => WoodlandAndShrubland,
        Biome.TemperateMixedForest => TemperateMixedForest,
        Biome.TemperateBroadleafForest => TemperateBroadleafForest,
        Biome.TemperateRainForest => TemperateRainForest,
        Biome.HotDesert => HotDesert,
        Biome.ThornScrub => ThornScrub,
        Biome.Savanna => Savanna,
        Biome.TropicalDryForest => TropicalDryForest,
        Biome.TropicalRainForest => TropicalRainForest,
        Biome.Mangrove => Mangrove,
        _ => throw new ArgumentOutOfRangeException(nameof(biome), biome, null),
    };
}

/// <summary>Parameters of the Sited stage (map-generation.md sections 3, 4, 5.1 and 8).</summary>
public sealed record SitingParameters
{
    [Description("Grid spacing of the region level in km: the size of a region (§4).")]
    public double RegionGridSizeKm { get; init; } = 128;

    [Description("How far a region site may lie from the centre of its grid square: 0 gives a square grid, 1 lets the site be anywhere in its square (§3.1).")]
    [Range(0, 1)]
    public double RegionJitter { get; init; } = 1;

    [Description("Grid spacing of the zone level in km. The sizes are kept nested: it is clamped to between 1/16 and 1/1.5 of the region size (§4).")]
    public double ZoneGridSizeKm { get; init; } = 32;

    [Description("How far a zone site may lie from the centre of its grid square: 0 gives a square grid, 1 lets the site be anywhere in its square (§3.1).")]
    [Range(0, 1)]
    public double ZoneJitter { get; init; } = 1;

    [Description("Border warp of the zone level, in cell sizes. 0 leaves the borders of zone cells unwarped. It affects which zone cell owns a patch cell (§3.2, §4).")]
    [Range(0, double.PositiveInfinity)]
    public double ZoneWarp { get; init; } = 0;

    [Description("Warp noise cycles per zone cell. Only used when ZoneWarp is above 0 (§3.2).")]
    public double ZoneWarpFrequency { get; init; } = 0.6;

    [Description("Grid spacing of the patch level in km. It is clamped to between 1/16 and 1/1.5 of the zone size (§4).")]
    public double PatchGridSizeKm { get; init; } = 8;

    [Description("How far a patch site may lie from the centre of its grid square: 0 gives a square grid, 1 lets the site be anywhere in its square (§3.1).")]
    [Range(0, 1)]
    public double PatchJitter { get; init; } = 1;

    [Description("Border warp of the patch level, in cell sizes. Coastlines and zone and region borders run along patch borders, so this gives all of them their wavy shape. 0 gives straight borders (§3.2, §4).")]
    [Range(0, double.PositiveInfinity)]
    public double PatchWarp { get; init; } = 0.26;

    [Description("Warp noise cycles per patch cell: lower gives broader waves in the borders (§3.2).")]
    public double PatchWarpFrequency { get; init; } = 0.33;

    [Description("Classify a point through its patch only (point → patch → zone → region), so zone borders, region borders and coastlines all run along patch borders. Only true is supported: every later layer assumes nesting (§4).")]
    public bool NestLevels { get; init; } = true;

    [Description("How many of the zone cells it owns each region chooses as ocean seeds: a count drawn evenly from Min to Max. More seeds give more sea (§5.1).")]
    public IntRange OceanSeedsPerRegion { get; init; } = new(0, 2);

    [Description("Relative weights of the four land elevation types, drawn per zone cell. Whole numbers; with the defaults 5 of 13 zone cells are Flat Land (§8).")]
    public PerElevationType<int> ElevationWeights { get; init; } = new(5, 6, 1, 1);
}

public enum OceanExpansionFrom
{
    /// <summary>Pass 1 expands from every zone cell on a link.</summary>
    Linked,
    /// <summary>Pass 1 expands from every ocean zone cell, lone ocean seeds too.</summary>
    All,
}

/// <summary>Parameters of the Outlined stage (sections 5.2, 5.3 and 6.1).</summary>
public sealed record OutlineParameters
{
    [Description("Chance that an ocean seed makes one link, to its nearest other ocean seed. Tested when the seed does not make two links. Every zone cell under a link becomes ocean (§5.2).")]
    [Range(0, 1)]
    public double OceanLinkChance { get; init; } = 0.4;

    [Description("Chance that an ocean seed makes two links, to its two nearest other ocean seeds. Tested first (§5.2).")]
    [Range(0, 1)]
    public double OceanDoubleLinkChance { get; init; } = 0.05;

    [Description("How many regions away an ocean seed looks for seeds to link to: 2 is a 5 × 5 block of regions. A region can be Outlined when all regions within this range + 5 rings are Sited, so a larger range makes every lookup reach further (§5.2).")]
    [Range(0, int.MaxValue)]
    public int OceanLinkRangeRegions { get; init; } = 2;

    [Description("Which ocean zone cells expand in the first pass. Linked: every zone cell on a link, so a lone ocean seed stays a sea of one zone cell. All: every ocean zone cell, lone seeds too (§5.3).")]
    public OceanExpansionFrom OceanExpansionFrom { get; init; } = OceanExpansionFrom.Linked;

    [Description("Number of ocean expansion passes. Each pass after the first expands from the zone cells the previous pass converted. More passes give more sea; 0 turns expansion off (§5.3).")]
    [Range(0, int.MaxValue)]
    public int OceanExpansionPasses { get; init; } = 1;

    [Description("Relative weights for an expanding zone cell converting 0, 1, 2 or 3 of its nearest land neighbours to ocean: four numbers in that order. A negative weight counts as 0 (§5.3).")]
    public IReadOnlyList<double> OceanExpansionWeights { get; init; } = [23, 33, 33, 10];

    [Description("How many of the zone cells it owns each region chooses to merge into their nearest neighbour: a count drawn evenly from Min to Max. A merger that finds no neighbour on its side of the coast still counts (§6.1).")]
    public IntRange ZoneMergersPerRegion { get; init; } = new(2, 4);

    [Description("Let a zone cell merge into its nearest neighbour even when one is land and the other ocean. Off by default: only land-land and ocean-ocean mergers are allowed (§6.1).")]
    public bool MergeAcrossCoast { get; init; } = false;
}

/// <summary>Parameters of the Detailed stage (sections 6.3, 10 and 11).</summary>
public sealed record DetailParameters
{
    [Description("How many of the patch cells it owns each zone cell chooses to merge into their nearest neighbour patch cell in the same zone: a count drawn evenly from Min to Max (§6.3).")]
    public IntRange PatchMergersPerZoneCell { get; init; } = new(2, 4);

    [Description("Rivers: grown per region with space colonization, from the sea into the land (§10).")]
    public RiverParameters Rivers { get; init; } = new();

    [Description("Lakes: placed per region on the rivers (§11).")]
    public LakeParameters Lakes { get; init; } = new();
}

/// <summary>The weight of one pull of an attractor at distance d, relative to the influence radius.</summary>
public enum PullPower
{
    /// <summary>Every pull has equal weight.</summary>
    Normal,
    /// <summary>(d / influence)².</summary>
    Squared,
    /// <summary>√(d / influence).</summary>
    Root,
}

public sealed record RiverParameters
{
    [Description("Attraction points scattered in every eligible patch cell. More give denser rivers. River sizes are flows divided by this number, so they read as patches drained (§10.1).")]
    [Range(1, int.MaxValue)]
    public int AttractorsPerPatchCell { get; init; } = 10;

    [Description("Length of one growth step, which is one river segment, in km (§10).")]
    public double StepKm { get; init; } = 1;

    [Description("An attractor pulls its nearest river node only within this radius, in km. Raised to at least 1.5 × the step. A large radius gives fewer, straighter rivers; a small one many independent branches (§10, §18.2).")]
    public double InfluenceKm { get; init; } = 47.8;

    [Description("An attractor is reached once a river node is this close, in km. Raised to at least 1.05 × the step (§10).")]
    public double KillKm { get; init; } = 5;

    [Description("Weight of a random unit vector added to the direction of every growth step. 0 gives straight runs towards the attractors (§10.3).")]
    [Range(0, double.PositiveInfinity)]
    public double RandomMotion { get; init; } = 0.4;

    [Description("How much of a step's random vector is inherited from the parent node, 0 to 1. Higher makes bends persist over several steps instead of wiggling (§10.3).")]
    [Range(0, 1)]
    public double Wander { get; init; } = 0.23;

    [Description("A river node is only pulled by attractors within ± half this angle of its heading, in degrees. It stops branches hooking back; 360 lets nodes see all round (§10.3).")]
    [Range(0, 360)]
    public double PerceptionConeDegrees { get; init; } = 120;

    [Description("A node that already has a child only sprouts a side branch if its own pulls are at least this share of the pulls on the stem beyond it. Higher removes more short side twigs; 0 turns it off (§10.5).")]
    [Range(0, double.PositiveInfinity)]
    public double MainStemDominance { get; init; } = 0.24;

    [Description("Weight of one pull by its distance d relative to the influence radius. Normal: every pull has equal weight. Squared: (d / influence)², far attractors pull harder. Root: √(d / influence) (§10.3).")]
    public PullPower PullPower { get; init; } = PullPower.Normal;

    [Description("Weight of the node's current heading in the direction of its next step. 0 is no inertia; higher gives straighter rivers (§10.3).")]
    public double Inertia { get; init; } = 0;

    [Description("Put no attractors in coastal patch cells. It keeps river mouths apart and gives the rivers a clear run from the coast into the interior (§10.1).")]
    public bool SkipCoastalPatchCells { get; init; } = true;

    [Description("A flat zone whose site gets less precipitation than this, in mm/yr, is river-poor: its patch cells get 0 or 1 attractor instead of the full number, it is drier for the biomes and it holds no lakes. Higher makes more of the flat zones dry plains (§10.1).")]
    public double RiverPoorPrecipitationCutoffMm { get; init; } = 1470;
}

public sealed record LakeParameters
{
    [Description("Chance that a patch cell with a suitable river node gets a lake (§11).")]
    [Range(0, 1)]
    public double Frequency { get; init; } = 0.06;

    [Description("Smallest lake in km, measured as the diameter of a circle of equal area (§11).")]
    public double MinSizeKm { get; init; } = 1.5;

    [Description("Largest lake in km, measured as the diameter of a circle of equal area. Sizes are skewed towards small lakes (§11).")]
    public double MaxSizeKm { get; init; } = 5;
}

/// <summary>Parameters of the Elevated stage (sections 12 and 13).</summary>
public sealed record ElevationParameters
{
    [Description("The height map: a raster per region, fitted to the rivers and the coast (§12).")]
    public HeightMapParameters HeightMap { get; init; } = new();

    [Description("Biomes: one per land patch, from temperature and aridity, and the flora and fauna of a patch (§13).")]
    public BiomeParameters Biomes { get; init; } = new();
}

public sealed record HeightMapParameters
{
    [Description("Raster cell size in metres (§12.1).")]
    public double ResolutionM { get; init; } = 1000;

    [Description("River gradient at a flow of 1, in m/m. Higher gives steeper rivers, and with them higher land, since the land rises from the river beds (§12.2).")]
    public double ChannelSlope { get; init; } = 0.01;

    [Description("Rivers flatten downstream as flow to the power −concavity: higher gives flatter large rivers. The default is 7/16; multiples of 1/16 are computed exactly (§12.2).")]
    public double Concavity { get; init; } = 0.4375;

    [Description("Scales the hillslope of every elevation type (§12).")]
    public double HillslopeMultiplier { get; init; } = 1;

    [Description("How strongly zones with a river gradient factor below 1 (flat zones by default) are levelled towards the height of their lowest river entry, so they read as a plain at roughly one level. 0 turns levelling off (§12.2).")]
    public double LevelFlatZones { get; init; } = 1;

    [Description("Strength of the region-scale relief field, which makes groups of neighbouring zones rugged or gentle together. 0 turns it off (§12.1).")]
    public double MountainBelts { get; init; } = 0.5;

    [Description("Ridge height along region borders, in metres. Region borders are watersheds (§12.5).")]
    public double RegionRidgeM { get; init; } = 80;

    [Description("Ridge height between two river systems, in metres (§12.5).")]
    public double BasinDivideM { get; init; } = 30;

    [Description("Multiplier on the surface detail noise. 0 gives a smooth surface (§12.5).")]
    public double DetailNoise { get; init; } = 1;

    [Description("How deep rivers are cut below their bed profile, in metres (§12.6).")]
    public double ChannelDepthM { get; init; } = 3;

    [Description("Sea depth 40 km or more from the coast, in metres (§12.5).")]
    public double SeaDepthM { get; init; } = 300;

    [Description("How steeply the land rises away from rivers and the coast, in m/m, per elevation type. This is what makes the land between the rivers a plain or a range (§12).")]
    public PerElevationType<double> Hillslope { get; init; } = new(0.006, 0.06, 0.15, 0.277);

    [Description("Multiplier on the river gradient, per elevation type. A type with a factor below 1 is levelled, see LevelFlatZones (§12.2).")]
    public PerElevationType<double> RiverGradientFactor { get; init; } = new(0.7, 1, 1.8, 3);

    [Description("Floodplain half-width as a function of the river bed height (§12).")]
    public FloodplainParameters Floodplain { get; init; } = new();
}

[Description("One control point: the floodplain half-width at a river bed height.")]
public sealed record FloodplainPoint(
    [property: Description("River bed height in metres.")] double BedHeightM,
    [property: Description("Floodplain half-width in metres at that bed height.")] double HalfWidthM);

public sealed record FloodplainParameters
{
    [Description("Control points of a monotone spline from river bed height to floodplain half-width. The half-width is constant beyond the end points and never less than 0.75 × the raster cell size (§12).")]
    public IReadOnlyList<FloodplainPoint> Points { get; init; } =
    [
        new(0, 400),
        new(100, 200),
        new(400, 90),
        new(1000, 30),
    ];

    [Description("When on, the points give the half-width at a flow of 100 and other flows get × √(flow / 100), so big rivers get wide floodplains (§12).")]
    public bool ScaleWithRiverSize { get; init; } = true;
}

[Description("One control point: the cooling at a height.")]
public sealed record CoolingPoint(
    [property: Description("Height in metres.")] double HeightM,
    [property: Description("Cooling in °C at that height.")] double CoolingC);

public sealed record BiomeParameters
{
    [Description("Control points of a monotone spline from height to the cooling of the Initial Temperature. Constant below the first point; above the last it continues along the last segment's slope. The defaults are much steeper than the real atmosphere, so land below 1000 m still gets cold highlands (§13.1).")]
    public IReadOnlyList<CoolingPoint> Cooling { get; init; } =
    [
        new(0, 0),
        new(200, 1),
        new(400, 5),
        new(900, 11),
        new(1250, 20),
    ];

    [Description("The precipitation of a patch in a river-poor flat zone is multiplied by this for the biomes. Lower makes the dry plains drier (§13.1).")]
    [Range(0, double.PositiveInfinity)]
    public double RiverPoorPrecipitationFactor { get; init; } = 0.2;

    [Description("A coastal patch in a flat or lowland zone becomes Mangrove when it is at least this warm, in °C, and at least as wet as MangroveMinAridity (§13.2).")]
    public double MangroveMinTemperatureC { get; init; } = 20;

    [Description("The smallest aridity (precipitation ÷ the dryness threshold of the temperature) at which a warm low coast becomes Mangrove. Below 0.5 is desert, 0.5 to 1 steppe, above 1 humid (§13.2).")]
    public double MangroveMinAridity { get; init; } = 0.5;

    [Description("How many kinds of flora a patch gets: a count drawn evenly from Min to Max. A patch gets fewer when fewer kinds fit it (§13.4).")]
    public IntRange FloraPerPatch { get; init; } = new(3, 6);

    [Description("How many kinds of fauna a patch gets: a count drawn evenly from Min to Max. A patch gets fewer when fewer kinds fit it (§13.4).")]
    public IntRange FaunaPerPatch { get; init; } = new(3, 6);
}

/// <summary>Parameters of the Planned stage (section 15).</summary>
public sealed record PlanningParameters
{
    [Description("Prospects per region. Regions with little habitable land end up with fewer (§15.5).")]
    [Range(0, int.MaxValue)]
    public int ProspectsPerRegion { get; init; } = 20;

    [Description("How many of a region's Prospects are for cities, at most. The rest are for towns (§15.5).")]
    [Range(0, int.MaxValue)]
    public int CityProspectsPerRegion { get; init; } = 4;

    [Description("Smallest distance between two Prospects, in km. Prospects for cities keep 2.5 × this from each other. When a region cannot fit its Prospects the spacing is relaxed by 15 % at a time, down to one patch size (§15.5).")]
    public double SpacingKm { get; init; } = 20;

    [Description("Weights of the five factors in the score of a patch. Only their proportions matter. A negative weight counts as 0; if all are 0 the plain mean is used (§15.4).")]
    public ProspectWeights Weights { get; init; } = new();

    [Description("The sum of the scores of a region's Prospects at or below which only 1 of them is on show. The number on show grows evenly with the sum from here to AllOnShowScoreSum. Prospects on show are drawn on a map and connected by roads, river routes and sea lanes. With the defaults about 1 region in 20 shows a single Prospect and the median region shows 13 of its 20 (§15.6, §16).")]
    [Range(0, double.MaxValue)]
    public double OneOnShowScoreSum { get; init; } = 700;

    [Description("The sum of the scores of a region's Prospects at or above which all of them are on show. With the defaults about 1 region in 10 shows all 20 (§15.6).")]
    [Range(0, double.MaxValue)]
    public double AllOnShowScoreSum { get; init; } = 1500;

    [Description("A Prospect with at least this score (0 to 100) is Rich: a map draws it in gold. With the defaults about a quarter of the Prospects on show are Rich (§15.6, §17).")]
    [Range(0, 100)]
    public double RichScore { get; init; } = 75;

    [Description("A Prospect with a score (0 to 100) below this is Poor: a map draws it in bronze. The ones between are Average, in silver. With the defaults about a fifth of the Prospects on show are Poor (§15.6, §17).")]
    [Range(0, 100)]
    public double PoorScore { get; init; } = 62;

    [Description("The potential population of a region: how many persons its land can feed. The races that are on share it (society §3).")]
    public PopulationParameters Population { get; init; } = new();

    [Description("The factions of a region: its ruling faction, its opposition and its lesser factions, from the races, the biomes, the resources and the wealth of the region (society §4).")]
    public FactionParameters Factions { get; init; } = new();
}

public sealed record PopulationParameters
{
    [Description("Persons per km² on land with a food supply of 1 at a middling Prosperity. The potential population of a patch is its area × this × its food supply × its Prosperity factor (society §3.1).")]
    [Range(0, double.PositiveInfinity)]
    public double PersonsPerKm2 { get; init; } = 0.06;

    [Description("How much the Prosperity field moves the potential population of a patch: the factor runs from 1 − this at the lowest Prosperity to 1 + this at the highest. 0 turns the effect of Prosperity off (society §3.1).")]
    [Range(0, 1)]
    public double ProsperityEffect { get; init; } = 0.5;

    [Description("The most races a region holds: its main races and its minority races. Only they share the potential population of the region. A number at or above the number of races that are on, with no minority races, gives every race its plain share (society §3.3).")]
    [Range(0, int.MaxValue)]
    public int RacesPerRegion { get; init; } = 6;

    [Description("How many of the races of a region are minority races: the last places, which go to the race whose turn it is in the region. The other places go to the main races, the largest by their plain shares. Every race that is on has one turn per place in every 6 × 5 block of regions with the default 28 races (society §3.3).")]
    [Range(0, int.MaxValue)]
    public int MinorityRacesPerRegion { get; init; } = 2;

    [Description("The most races a city holds. They are races of its region: the largest around the Prospect, and a last one drawn with the minority races weighted up (society §5.1).")]
    [Range(0, int.MaxValue)]
    public int RacesPerCity { get; init; } = 5;

    [Description("The most races a town holds. They are races of its region: the largest around the Prospect, and a last one drawn with the minority races weighted up (society §5.1).")]
    [Range(0, int.MaxValue)]
    public int RacesPerTown { get; init; } = 4;

    [Description("How many persons of a race the land of a region must feed by the plain shares for the race to take a minority place there (society §3.3).")]
    [Range(0, double.PositiveInfinity)]
    public double MinorityMinPersons { get; init; } = 0.1;

    [Description("The share of the potential population of a region that a minority race is lifted towards, 0 to 0.9. A minority race that already holds more keeps its share, and one that lives on a few patches only stays below it. 0 turns the lift off (society §3.4).")]
    [Range(0, 0.9)]
    public double MinorityShare { get; init; } = 0.05;

    [Description("The factor on a minority race of the region in the draw for the last race of a city or a town. 1 gives it no advantage (society §5.1).")]
    [Range(0, double.PositiveInfinity)]
    public double MinorityBoost { get; init; } = 5;

    [Description("How much of the potential population of the whole region a city or a town draws on, beside the patch of its Prospect and the land patches around it, 0 to 1. It lets a race of the region that does not live nearby into the city or town (society §5.1).")]
    [Range(0, 1)]
    public double HinterlandShare { get; init; } = 0.02;
}

/// <summary>Parameters of the factions of a region (society-generation.md section 4).</summary>
public sealed record FactionParameters
{
    [Description("How much the size of a race counts when the lead race of the ruling faction is drawn: the share of the race in the persons of the region is raised to this power. With the default the largest race leads in about 6 regions in 10 and a minority race in 1 in 100. A higher number makes the largest race lead more often (society §4.4).")]
    [Range(0, double.PositiveInfinity)]
    public double RacePower { get; init; } = 5;

    [Description("How much the land decides the type of the ruling faction: the fit of each type is raised to this power. 1 lets the leaning of the races decide more; a high number makes the type with the best fit rule nearly always (society §4.4).")]
    [Range(0, double.PositiveInfinity)]
    public double Sharpness { get; init; } = 2;

    [Description("The fit of each type as the ruler in a land that says nothing for it, 0 to 1. The land moves the fit from here up to 1. A higher base makes the type rule more regions (society §4.4).")]
    public PerFactionType<double> BaseFit { get; init; } = new(0.35, 0.12, 0.05, 0.05, 0, 0.10);

    [Description("The weights of the five terms of the tension of a region. The tension runs from 0 to their sum, 1 with the defaults (society §4.5).")]
    public TensionWeights Tension { get; init; } = new();

    [Description("The tension from which a region has an opposition. With the default about 4 regions in 10 have an opposition; 0.4 gives 6 in 10 and 0.6 gives 2 in 10 (society §4.5).")]
    [Range(0, double.PositiveInfinity)]
    public double OppositionTension { get; init; } = 0.5;

    [Description("The width of the random part of the tension: a number from minus half this to plus half this is added, drawn per region. 0 makes the tension alone decide (society §4.5).")]
    [Range(0, double.PositiveInfinity)]
    public double TensionJitter { get; init; } = 0.2;

    [Description("The persons a region needs to hold an opposition (society §4.5).")]
    [Range(0, double.PositiveInfinity)]
    public double OppositionMinPersons { get; init; } = 80;

    [Description("Which type opposes which ruler: for each type of the ruling faction, the weight of each type as its opposition. A crown is opposed by a rival house first, a people by another people, a guild by a rival guild, a faith by a sect or by the mages, a free town by a lord who wants it (society §4.6).")]
    public PerFactionType<PerFactionType<double>> Cleavage { get; init; } = new(
        Family: new(3, 2, 2, 2, 1, 2),
        Race: new(2, 3, 1, 1, 1, 1),
        Vocation: new(2, 1, 3, 1, 1, 2),
        Belief: new(2, 1, 1, 3, 3, 1),
        Magic: new(2, 1, 1, 3, 2, 2),
        Place: new(3, 2, 2, 1, 1, 1));

    [Description("Which type of opposition each term of the tension feeds, 0 to 2. The contest has no entry: it feeds every type by its fit (society §4.6).")]
    public TensionFavour Favour { get; init; } = new();

    [Description("The persons a region needs for each lesser faction: a region holds at most its persons divided by this, rounded down (society §4.7).")]
    [Range(1, double.PositiveInfinity)]
    public double PersonsPerLesserFaction { get; init; } = 150;

    [Description("The most lesser factions a region holds (society §4.7).")]
    [Range(0, int.MaxValue)]
    public int MaxLesserFactions { get; init; } = 4;

    [Description("The lowest fit, 0 to 1, with which a kind of lesser faction is taken. A lower number gives more lesser factions (society §4.7).")]
    [Range(0, 1)]
    public double LesserFit { get; init; } = 0.75;

    [Description("The width of the random part of the fit of a kind of lesser faction: a number from minus half this to plus half this is added, drawn per region and kind (society §4.7).")]
    [Range(0, double.PositiveInfinity)]
    public double LesserJitter { get; init; } = 0.1;

    [Description("The ranges the indices of a region are read through: each index runs from 0 at Low to 1 at High. The defaults are about the 10th and the 90th percentile of generated regions, so a change to the map parameters can call for new ranges (society §4.3).")]
    public FactionIndexRanges Indices { get; init; } = new();
}

[Description("A range an index is read through: 0 at Low and below, 1 at High and above, a straight line between.")]
public sealed record IndexRange(
    [property: Description("The value at and below which the index is 0.")] double Low,
    [property: Description("The value at and above which the index is 1.")] double High);

public sealed record TensionWeights
{
    [Description("Weight of the contest: the land and the races would as well carry another kind of ruler (society §4.5).")]
    [Range(0, double.PositiveInfinity)]
    public double Contest { get; init; } = 0.25;

    [Description("Weight of rough land: hills and mountains shelter those who do not obey (society §4.5).")]
    [Range(0, double.PositiveInfinity)]
    public double RoughLand { get; init; } = 0.20;

    [Description("Weight of a rival seat: a second Prospect on show nearly as good as the best, and far from it (society §4.5).")]
    [Range(0, double.PositiveInfinity)]
    public double RivalSeat { get; init; } = 0.20;

    [Description("Weight of precious finds: silver and lead, gold, gems and pearls (society §4.5).")]
    [Range(0, double.PositiveInfinity)]
    public double PreciousFinds { get; init; } = 0.15;

    [Description("Weight of poor land: Poor Prospects, and rich and poor Prospects side by side (society §4.5).")]
    [Range(0, double.PositiveInfinity)]
    public double PoorLand { get; init; } = 0.20;
}

public sealed record TensionFavour
{
    [Description("Rough land feeds a people and a commune first, then a hill lord and a faith (society §4.6).")]
    public PerFactionType<double> RoughLand { get; init; } = new(1, 2, 0, 1, 0, 2);

    [Description("A rival seat feeds a rival house, the guilds of the second city and its council (society §4.6).")]
    public PerFactionType<double> RivalSeat { get; init; } = new(2, 0, 2, 0, 0, 2);

    [Description("Precious finds feed a rival house and a guild (society §4.6).")]
    public PerFactionType<double> PreciousFinds { get; init; } = new(2, 0, 2, 0, 0, 0);

    [Description("Poor land feeds a sect and a commune first, then a subject people (society §4.6).")]
    public PerFactionType<double> PoorLand { get; init; } = new(0, 1, 0, 2, 0, 2);
}

public sealed record FactionIndexRanges
{
    [Description("Grain: the mean amount of grain on the land of the region (society §4.3).")]
    public IndexRange Grain { get; init; } = new(0.17, 0.45);

    [Description("Half of the index herd: the mean amount of livestock (society §4.3).")]
    public IndexRange Livestock { get; init; } = new(0.33, 0.53);

    [Description("Half of the index herd: the mean amount of horses and pack animals (society §4.3).")]
    public IndexRange Horses { get; init; } = new(0.22, 0.40);

    [Description("Open: the share of the land in Cold steppe, Temperate grassland, Savanna, Cold desert, Hot desert, Thorn scrub and Tundra (society §4.3).")]
    public IndexRange OpenLand { get; init; } = new(0.25, 0.66);

    [Description("Forest: the share of the land in the six forest biomes, Mangrove and Woodland & shrubland (society §4.3).")]
    public IndexRange Forest { get; init; } = new(0.29, 0.69);

    [Description("Cold: the share of the land in Ice & snow, Tundra and Boreal forest (society §4.3).")]
    public IndexRange ColdLand { get; init; } = new(0.1, 0.6);

    [Description("Dry: the share of the land in Hot desert, Cold desert, Cold steppe and Thorn scrub (society §4.3).")]
    public IndexRange DryLand { get; init; } = new(0.1, 0.6);

    [Description("Rough: the share of the land in Hills zones plus 1.5 times the share in Mountains zones (society §4.3).")]
    public IndexRange RoughLand { get; init; } = new(0, 0.45);

    [Description("Half of the index trade: the best Trade factor of a Prospect on show (society §4.3).")]
    public IndexRange TradeFactor { get; init; } = new(0.5, 1);

    [Description("Half of the index trade: the number of Prospects on show with a harbour or a navigable river mouth (society §4.3).")]
    public IndexRange Harbours { get; init; } = new(0, 4);

    [Description("Half of the index wealth: the mean Prosperity factor of the Prospects of the region (society §4.3).")]
    public IndexRange Prosperity { get; init; } = new(0.33, 0.69);

    [Description("Half of the index wealth: the number of Rich Prospects on show (society §4.3).")]
    public IndexRange RichProspects { get; init; } = new(0, 9);

    [Description("Urban: the number of Prospects on show (society §4.3).")]
    public IndexRange ProspectsOnShow { get; init; } = new(6, 19);

    [Description("Ore: the shares of the land with a listed amount of iron ore, copper ore, tin, silver and lead, gold, gems and pearls and salt, added (society §4.3).")]
    public IndexRange Ore { get; init; } = new(0.03, 0.18);

    [Description("Precious: the shares of the land with a listed amount of silver and lead, gold and gems and pearls, added (society §4.3).")]
    public IndexRange Precious { get; init; } = new(0, 0.06);

    [Description("Arcane: the mean over the land of how far the Supernatural field lies above 60, from 0 at 60 to 1 at 100 (society §4.3).")]
    public IndexRange Supernatural { get; init; } = new(0.014, 0.032);

    [Description("People: the persons of the region (society §4.3).")]
    public IndexRange Persons { get; init; } = new(160, 600);

    [Description("Small: the persons of the region, read the other way round: the index is 1 at Low and below and 0 at High and above (society §4.3).")]
    public IndexRange SmallRegion { get; init; } = new(60, 260);

    [Description("Mixed: the share of the largest race in the persons of the region, read the other way round: the index is 1 at Low and below and 0 at High and above (society §4.3).")]
    public IndexRange LargestRace { get; init; } = new(0.24, 0.32);

    [Description("Poor: the share of the Prospects on show that are Poor (society §4.3).")]
    public IndexRange PoorProspects { get; init; } = new(0, 0.5);

    [Description("Uneven: the highest Prosperity factor of a Prospect on show minus the lowest (society §4.3).")]
    public IndexRange UnevenProsperity { get; init; } = new(0.1, 0.5);
}

public sealed record ProspectWeights
{
    [Description("Weight of the resources factor: food, fresh water, building materials and fuel, fish and trade goods on and around the patch (§15.3).")]
    public double Resources { get; init; } = 0.3;

    [Description("Weight of the geography factor: buildable ground, waterside and defence (§15.3).")]
    public double Geography { get; init; } = 0.15;

    [Description("Weight of the trade factor: gateways (river mouths, harbours, heads of navigation, confluences) and how much of the region's food is within reach (§15.3).")]
    public double Trade { get; init; } = 0.25;

    [Description("Weight of the climate factor: a comfortable temperature and enough moisture (§15.3).")]
    public double Climate { get; init; } = 0.15;

    [Description("Weight of the prosperity factor: the Prosperity field at the spot (§15.3).")]
    public double Prosperity { get; init; } = 0.15;
}

/// <summary>The four Fields. They have no generation stage.</summary>
public sealed record FieldsParameters
{
    [Description("Precipitation in mm/yr, on a logarithmic scale. Its Distribution spreads the moisture classes of the biomes evenly: the points lie near the bounds between desert, steppe, sub-humid, humid and wet. Warped into belts. Feeds the biomes and thins the rivers of dry flat zones (§9, §13.2).")]
    public FieldParameters Precipitation { get; init; } = new(80, 2, 2.2, 0.55, 100, 12000, 200, 500)
    {
        Distribution = [new(0.05, 150), new(0.12, 350), new(0.43, 700), new(0.66, 1400), new(0.88, 2800)],
    };

    [Description("Initial Temperature in °C at sea level, on a linear scale, warped into belts. It is cooled by the height of the patch for the biomes. Its Distribution has one point at each bound between two thermal belts, so the shares are the shares of the belts at sea level (§9, §13.1, §13.2).")]
    public FieldParameters InitialTemperature { get; init; } = new(110, 2, 2, 0.5, -16, 30, 300, 800)
    {
        Distribution = [new(0.055, -10), new(0.09, -3), new(0.21, 3), new(0.59, 18)],
    };

    [Description("Prosperity index, on a linear scale. Feeds the resources (it boosts the valuable ones), the Prospect scores and the potential population (§9, society §3.1).")]
    public FieldParameters Prosperity { get; init; } = new(200, 3, 2, 0.5, 1, 100);

    [Description("Supernatural index, on a linear scale. Its Distribution keeps the supernatural rare: most of the map is low and 1 % lies above 80. It feeds the arcane materials of the resources, which lie only where it is above 60 (§14.1).")]
    public FieldParameters Supernatural { get; init; } = new(10, 3, 2, 0.5, 0, 100)
    {
        Distribution = [new(0.4, 20), new(0.7, 40), new(0.9, 60), new(0.99, 80)],
    };

    public FieldParameters this[FieldKind kind] => kind switch
    {
        FieldKind.Precipitation => Precipitation,
        FieldKind.InitialTemperature => InitialTemperature,
        FieldKind.Prosperity => Prosperity,
        FieldKind.Supernatural => Supernatural,
        _ => throw new ArgumentOutOfRangeException(nameof(kind)),
    };
}

public sealed record FieldParameters(
    [property: Description("Size of the features of the noise in km: larger gives slower changes across the map.")] double FeatureSizeKm,
    [property: Description("Number of noise octaves: a whole number, at least 1. More octaves add finer detail.")] double Octaves,
    [property: Description("Frequency multiplier from one octave to the next.")] double Lacunarity,
    [property: Description("Amplitude multiplier from one octave to the next: lower makes the finer octaves weaker.")] double Gain,
    [property: Description("The value at the strongest lows of the noise. At least 1 for the logarithmic Precipitation field.")] double Min,
    [property: Description("The value at the strongest highs of the noise. Only the strongest highs and lows reach Min and Max.")] double Max,
    [property: Description("Strength of the warp in km: about the furthest the position the field is read at is moved. A warp of a few feature sizes draws the round features of the noise out into winding belts. 0 turns the warp off.")] double WarpKm = 0,
    [property: Description("Size of the features of the warp noise in km: larger gives longer, straighter belts. Only used when WarpKm is above 0.")] double WarpFeatureSizeKm = 200)
{
    [Description("How the values are spread over the map: each point gives the share of the map that lies below a value. Min is the point at share 0 and Max the point at share 1, and a monotone spline runs through the points (through the logarithm of the values for Precipitation). The shares hold over a large area; a single view of the map can differ. Empty gives the bell shape of the noise: half the map lies in the middle quarter of the range between Min and Max (§9).")]
    public IReadOnlyList<FieldShare> Distribution { get; init; } = [];
}

[Description("One point of the distribution of a field: the share of the map that lies below a value.")]
public sealed record FieldShare(
    [property: Description("Share of the map that lies below the value: above 0 and below 1. A point outside that is ignored.")][property: Range(0, 1)] double Share,
    [property: Description("The value, in the unit of the field. Between Min and Max, and the values rise with the shares.")] double Value);

/// <summary>A bedrock type and its weight under each elevation type.</summary>
[Description("A bedrock type and its weight under each elevation type.")]
public sealed record BedrockParameters(
    [property: Description("The id the resource rules know the bedrock by: shield, foldBelt, granite, volcanic, basalt, ophiolite, carbonate, clastic or alluvium.")] string Id,
    [property: Description("The name shown for the bedrock.")] string Name,
    [property: Description("Relative weight of the bedrock under each elevation type. Whole numbers; 0 means never under that type.")] PerElevationType<int> Weights)
{
    public static IReadOnlyList<BedrockParameters> Defaults { get; } =
    [
        new(BedrockIds.Shield, "Crystalline shield", new(20, 20, 20, 10)),
        new(BedrockIds.FoldBelt, "Fold belt", new(0, 5, 30, 40)),
        new(BedrockIds.Granite, "Granite intrusion", new(0, 3, 15, 20)),
        new(BedrockIds.Volcanic, "Volcanic arc", new(0, 2, 10, 30)),
        new(BedrockIds.Basalt, "Flood basalt", new(10, 10, 15, 5)),
        new(BedrockIds.Ophiolite, "Ophiolite", new(0, 2, 10, 15)),
        new(BedrockIds.Carbonate, "Carbonate platform", new(15, 30, 30, 15)),
        new(BedrockIds.Clastic, "Clastic basin", new(40, 40, 20, 5)),
        new(BedrockIds.Alluvium, "Unconsolidated cover", new(60, 40, 0, 0)),
    ];
}

/// <summary>
/// A race: a people that can live in the world. The races are a list in the Parameter Set, so
/// a race is added, changed or switched off without touching the generation code.
/// </summary>
[Description("One control point of the height modifier of a race: the factor at a height.")]
public sealed record HeightFactorPoint(
    [property: Description("Height in metres above sea level.")] double HeightM,
    [property: Description("What the biome frequencies of the race are multiplied by at that height. 0 means the race does not live at that height; a negative number counts as 0.")] double Factor);

[Description("A race: a people that can live in the world, and how frequent it is in each biome and at each height.")]
public sealed record RaceParameters(
    [property: Description("The id the generated data knows the race by. Unique in the list; keep it when the name changes.")] string Id,
    [property: Description("The name shown for the race.")] string Name,
    [property: Description("Whether the race lives in the world. A race that is off gets no population, and the races that are on share the land without it.")] bool Enabled,
    [property: Description("How frequent the race is in each biome, relative to the other races. The potential population of a patch is shared between the races that are on in proportion to these numbers, so only the proportions between the races within one biome matter. List only the biomes the race lives in: a biome that is left out has the frequency 0, and a frequency of 0 is not written to the file. A negative number counts as 0 (society §3.2).")] PerBiome<double> BiomeFrequency)
{
    [Description("How the height of the land changes the frequency of the race: control points from the mean height of a patch to a factor on its biome frequencies. The factor runs in a straight line between two points and is constant beyond the first and the last. Two points at the same height make a step: the first holds below that height, the second from that height and up. So [200 → 0, 200 → 1] is a race that only lives from 200 m and up, and [200 → 1, 200 → 0] one that only lives below 200 m. Left out, the height does not matter to the race (society §3.2).")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyList<HeightFactorPoint>? HeightModifier { get; init; }

    [Description("How much the race leans to each type of faction, 0 to 3: 3 for the kind of group it forms first, 0 for a kind it never forms. It weighs the race in the draws for the type and the lead race of a faction. Left out, the race leans to every type alike (society §4.4).")]
    public PerFactionType<double> FactionLeaning { get; init; } = new(1, 1, 1, 1, 1, 1);

    /// <summary>
    /// The leaning of each default race to each type of faction (society-generation.md
    /// Appendix A), from the "Factions" line of its entry in races-basis.md.
    /// </summary>
    private static readonly Dictionary<string, PerFactionType<double>> DefaultLeanings = new()
    {
        //                     Family Race Vocation Belief Magic Place
        ["northmen"] = new(2, 1, 2, 1, 1, 3),
        ["midlanders"] = new(3, 1, 2, 2, 1, 2),
        ["saltroadFolk"] = new(2, 2, 3, 2, 1, 1),
        ["sunlanders"] = new(3, 2, 2, 2, 1, 2),
        ["grassRiders"] = new(2, 3, 1, 1, 1, 0),
        ["rainkeepers"] = new(2, 1, 2, 3, 1, 2),
        ["canalFolk"] = new(2, 1, 3, 1, 1, 1),
        ["elf"] = new(2, 3, 1, 1, 2, 1),
        ["dwarf"] = new(1, 3, 3, 1, 1, 1),
        ["halfling"] = new(2, 1, 1, 1, 0, 3),
        ["gnome"] = new(1, 1, 3, 1, 2, 2),
        ["orc"] = new(1, 3, 2, 1, 1, 0),
        ["hoarkin"] = new(1, 2, 2, 1, 1, 3),
        ["hearthbloods"] = new(2, 2, 1, 1, 1, 2),
        ["widegaze"] = new(1, 2, 2, 1, 2, 2),
        ["amberbloods"] = new(1, 2, 2, 2, 1, 2),
        ["brinefolk"] = new(1, 1, 3, 2, 1, 2),
        ["duskreaders"] = new(2, 1, 2, 1, 2, 2),
        ["noonfolk"] = new(0, 1, 2, 2, 1, 3),
        ["boughwalkers"] = new(2, 2, 2, 1, 1, 2),
        ["shoalfolk"] = new(1, 1, 2, 1, 1, 3),
        ["swiftborn"] = new(0, 1, 2, 1, 1, 3),
        ["flinchless"] = new(1, 1, 3, 1, 1, 1),
        ["lanternkin"] = new(1, 1, 3, 1, 2, 1),
        ["stillfaces"] = new(1, 2, 2, 1, 2, 1),
        ["twofold"] = new(3, 1, 1, 2, 1, 1),
        ["bitterbloods"] = new(1, 2, 3, 1, 2, 2),
        ["catfolk"] = new(1, 2, 2, 2, 1, 1),
    };

    /// <summary>The height modifier of a race that only lives from a height and up.</summary>
    public static IReadOnlyList<HeightFactorPoint> OnlyFrom(double heightM) => [new(heightM, 0), new(heightM, 1)];

    /// <summary>The height modifier of a race that only lives below a height.</summary>
    public static IReadOnlyList<HeightFactorPoint> OnlyBelow(double heightM) => [new(heightM, 1), new(heightM, 0)];

    /// <summary>
    /// The default list (society-generation.md Appendix A): 28 races, all on. Seven are human
    /// peoples; there is no plain human race. Each frequency is the habitat weight of the race
    /// in the biome (0 avoided, 1 tolerated, 2 good, 3 native) times how common the race is in
    /// the world. Five races have a height modifier; the height does not matter to the others.
    /// </summary>
    public static IReadOnlyList<RaceParameters> Defaults { get; } =
    [
        //                                    common   Ice Tun CSt Bor CDs TGr WSh TMx TBl TRn HDs Thn Sav TDf TRf Mgv
        Make("northmen", "Northmen", 3, /*          */ 1, 2, 1, 3, 0, 1, 0, 3, 1, 2, 0, 0, 0, 0, 0, 0),
        Make("midlanders", "Midlanders", 5, /*      */ 0, 0, 1, 1, 0, 3, 2, 2, 3, 2, 0, 0, 0, 0, 0, 0),
        Make("saltroadFolk", "Saltroad folk", 3, /* */ 0, 0, 0, 0, 2, 1, 2, 0, 0, 0, 3, 3, 2, 1, 0, 0),
        Make("sunlanders", "Sunlanders", 4, /*      */ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 3, 3, 2, 1),
        Make("grassRiders", "Grass riders", 3, /*   */ 0, 1, 3, 1, 2, 3, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0),
        Make("rainkeepers", "Rainkeepers", 4, /*    */ 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 2, 3, 2, 2),
        Make("canalFolk", "Canal folk", 5, /*       */ 0, 0, 1, 0, 0, 2, 1, 2, 3, 3, 0, 0, 0, 2, 1, 1),
        Make("elf", "Elves", 2, /*                  */ 0, 0, 0, 2, 0, 1, 1, 3, 3, 3, 0, 0, 0, 1, 2, 0) with { HeightModifier = OnlyBelow(200) },
        Make("dwarf", "Dwarves", 2, /*              */ 1, 2, 1, 2, 1, 1, 1, 2, 1, 1, 0, 0, 0, 0, 0, 0) with { HeightModifier = OnlyFrom(200) },
        Make("halfling", "Halflings", 2, /*         */ 0, 0, 1, 1, 0, 3, 3, 2, 2, 1, 0, 0, 1, 1, 1, 0),
        Make("gnome", "Gnomes", 1, /*               */ 0, 1, 1, 2, 1, 1, 2, 2, 3, 1, 0, 1, 1, 1, 0, 0),
        Make("orc", "Orcs", 2, /*                   */ 0, 1, 3, 1, 2, 2, 2, 1, 1, 1, 2, 2, 2, 1, 0, 0),
        Make("hoarkin", "Hoarkin", 1, /*            */ 3, 2, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0),
        Make("hearthbloods", "Hearthbloods", 0.5, 2, 3, 1, 2, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0) with { HeightModifier = [new(100, 0.5), new(400, 1)] },
        Make("widegaze", "Widegaze", 1, /*          */ 0, 0, 3, 0, 1, 3, 0, 0, 0, 0, 0, 1, 2, 0, 0, 0) with { HeightModifier = [new(300, 1), new(500, 0)] },
        Make("amberbloods", "Amberbloods", 1, /*    */ 0, 0, 0, 3, 0, 0, 0, 3, 2, 2, 0, 0, 0, 0, 0, 0),
        Make("brinefolk", "Brinefolk", 1, /*        */ 0, 0, 1, 0, 3, 0, 0, 0, 0, 0, 2, 1, 0, 0, 0, 1),
        Make("duskreaders", "Duskreaders", 1, /*    */ 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 3, 2, 1, 0, 0, 0),
        Make("noonfolk", "Noonfolk", 1, /*          */ 0, 0, 0, 0, 0, 1, 1, 0, 0, 0, 2, 3, 3, 1, 0, 0),
        Make("boughwalkers", "Boughwalkers", 1, /*  */ 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 2, 3, 1),
        Make("shoalfolk", "Shoalfolk", 1, /*        */ 0, 0, 0, 1, 0, 0, 1, 0, 0, 2, 0, 0, 0, 1, 1, 3) with { HeightModifier = [new(20, 1), new(80, 0)] },
        Make("swiftborn", "Swiftborn", 1, /*        */ 0, 0, 0, 0, 0, 2, 3, 1, 2, 0, 0, 1, 1, 0, 0, 0),
        Make("flinchless", "Flinchless", 0.5, /*    */ 0, 0, 1, 0, 0, 2, 2, 1, 2, 0, 0, 0, 1, 0, 0, 0),
        Make("lanternkin", "Lanternkin", 0.5, /*    */ 0, 0, 0, 0, 0, 1, 1, 1, 1, 2, 0, 0, 1, 2, 2, 1),
        Make("stillfaces", "Stillfaces", 1, /*      */ 0, 0, 0, 2, 0, 0, 1, 3, 2, 1, 0, 0, 0, 0, 0, 0),
        Make("twofold", "Twofold", 1, /*            */ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 2, 3, 2, 1),
        Make("bitterbloods", "Bitterbloods", 0.5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 3, 3),
        Make("catfolk", "Catfolk", 1, /*            */ 0, 0, 1, 0, 1, 2, 1, 1, 1, 1, 2, 2, 3, 2, 2, 1),
    ];

    /// <summary>A race from its habitat weight per biome (in the order of <see cref="Biome"/>) and how common it is.</summary>
    private static RaceParameters Make(string id, string name, double common, params int[] habitat) =>
        habitat.Length != 16
            ? throw new ArgumentException($"The race {id} needs one habitat weight per biome.", nameof(habitat))
            : new(id, name, true, new PerBiome<double>(
            common * habitat[0], common * habitat[1], common * habitat[2], common * habitat[3],
            common * habitat[4], common * habitat[5], common * habitat[6], common * habitat[7],
            common * habitat[8], common * habitat[9], common * habitat[10], common * habitat[11],
            common * habitat[12], common * habitat[13], common * habitat[14], common * habitat[15]))
            {
                FactionLeaning = DefaultLeanings[id],
            };
}

/// <summary>The ids of the nine bedrock types the generation currently uses.</summary>
public static class BedrockIds
{
    public const string Shield = "shield";
    public const string FoldBelt = "foldBelt";
    public const string Granite = "granite";
    public const string Volcanic = "volcanic";
    public const string Basalt = "basalt";
    public const string Ophiolite = "ophiolite";
    public const string Carbonate = "carbonate";
    public const string Clastic = "clastic";
    public const string Alluvium = "alluvium";
}
