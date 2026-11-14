using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gameworld.Domain;

// Every parameter carries a [Description]: what it does, its unit and the section of
// docs/map-generation.md it belongs to. docs/parameter-set.schema.json is generated from these
// (ParameterSetSchema), so a description is written once, here.

/// <summary>
/// The set of all map and world parameters, grouped by region generation stage. Referenced by
/// a foundation and immutable after phase 1 like the foundation. The defaults are the same as
/// docs/default-parameter-set.json and the text of docs/map-generation.md.
/// </summary>
[Description("The set of all map and world parameters of a foundation, grouped by region generation stage. A parameter that is left out keeps its default. Section numbers refer to docs/map-generation.md.")]
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

    [Description("Planned stage: Prospects for cities and towns (§15).")]
    public PlanningParameters Planning { get; init; } = new();

    [Description("Settled stage: the travel network (§16).")]
    public TravelParameters Travel { get; init; } = new();

    [Description("The four Fields: noise maps that drive climate, biomes and resources. They have no stage (§9).")]
    public FieldsParameters Fields { get; init; } = new();

    [Description("The bedrock types and their relative weight under each elevation type. The bedrock of a zone is drawn by these weights (§14.1). The resource rules know the nine ids of the defaults; a bedrock with another id gives no bedrock-bound resources.")]
    public IReadOnlyList<BedrockParameters> Bedrocks { get; init; } = BedrockParameters.Defaults;

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

    [Description("Biomes: one per land patch, from temperature and aridity (§13).")]
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

/// <summary>Parameters of the Settled stage (section 16).</summary>
public sealed record TravelParameters
{
    /// <summary>Only Prospects with at least this score are on show and get travel links.</summary>
    [Description("Only Prospects with at least this score (0 to 100) are on show and are connected by roads, river routes and sea lanes. With the defaults a median of 13 of a region's 20 Prospects pass (§15.5, §16).")]
    [Range(0, 100)]
    public double MinProspectScore { get; init; } = 59;
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

    [Description("Prosperity index, on a linear scale. Feeds the resources (it boosts the valuable ones) and the Prospect scores (§9).")]
    public FieldParameters Prosperity { get; init; } = new(200, 3, 2, 0.5, 1, 100);

    [Description("Supernatural index, on a linear scale. It feeds nothing yet (§9).")]
    public FieldParameters Supernatural { get; init; } = new(10, 3, 2, 0.5, 0, 100);

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
