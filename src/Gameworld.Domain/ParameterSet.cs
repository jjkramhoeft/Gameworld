using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gameworld.Domain;

/// <summary>
/// The set of all map and world parameters, grouped by region generation stage. Referenced by
/// a foundation and immutable after phase 1 like the foundation. The defaults are the same as
/// docs/default-parameter-set.json and the text of docs/map-generation.md.
/// </summary>
public sealed record ParameterSet
{
    public SitingParameters Siting { get; init; } = new();
    public OutlineParameters Outline { get; init; } = new();
    public DetailParameters Detail { get; init; } = new();
    public ElevationParameters Elevation { get; init; } = new();
    public PlanningParameters Planning { get; init; } = new();
    public TravelParameters Travel { get; init; } = new();
    public FieldsParameters Fields { get; init; } = new();
    public IReadOnlyList<BedrockParameters> Bedrocks { get; init; } = BedrockParameters.Defaults;

    public static ParameterSet Default => new();

    private static readonly JsonSerializerOptions JsonOptions = new()
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
public sealed record IntRange(int Min, int Max);

/// <summary>One value per land elevation type.</summary>
public sealed record PerElevationType<T>(T FlatLand, T Lowland, T Hills, T Mountains)
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
    public double RegionGridSizeKm { get; init; } = 128;
    public double RegionJitter { get; init; } = 1;
    public double ZoneGridSizeKm { get; init; } = 32;
    public double ZoneJitter { get; init; } = 1;
    public double ZoneWarp { get; init; } = 0;
    public double ZoneWarpFrequency { get; init; } = 0.6;
    public double PatchGridSizeKm { get; init; } = 8;
    public double PatchJitter { get; init; } = 1;
    public double PatchWarp { get; init; } = 0.26;
    public double PatchWarpFrequency { get; init; } = 0.33;
    public bool NestLevels { get; init; } = true;
    public IntRange OceanSeedsPerRegion { get; init; } = new(0, 2);
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
    public double OceanLinkChance { get; init; } = 0.4;
    public double OceanDoubleLinkChance { get; init; } = 0.05;
    public int OceanLinkRangeRegions { get; init; } = 2;
    public OceanExpansionFrom OceanExpansionFrom { get; init; } = OceanExpansionFrom.Linked;
    public int OceanExpansionPasses { get; init; } = 1;
    public IReadOnlyList<double> OceanExpansionWeights { get; init; } = [23, 33, 33, 10];
    public IntRange ZoneMergersPerRegion { get; init; } = new(2, 4);
    public bool MergeAcrossCoast { get; init; } = false;
}

/// <summary>Parameters of the Detailed stage (sections 6.3, 10 and 11).</summary>
public sealed record DetailParameters
{
    public IntRange PatchMergersPerZoneCell { get; init; } = new(2, 4);
    public RiverParameters Rivers { get; init; } = new();
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
    public int AttractorsPerPatchCell { get; init; } = 10;
    public double StepKm { get; init; } = 1;
    public double InfluenceKm { get; init; } = 47.8;
    public double KillKm { get; init; } = 5;
    public double RandomMotion { get; init; } = 0.4;
    public double Wander { get; init; } = 0.23;
    public double PerceptionConeDegrees { get; init; } = 120;
    public double MainStemDominance { get; init; } = 0.24;
    public PullPower PullPower { get; init; } = PullPower.Normal;
    public double Inertia { get; init; } = 0;
    public bool SkipCoastalPatchCells { get; init; } = true;
    public double RiverPoorPrecipitationCutoffMm { get; init; } = 1660;
}

public sealed record LakeParameters
{
    public double Frequency { get; init; } = 0.06;
    public double MinSizeKm { get; init; } = 1.5;
    public double MaxSizeKm { get; init; } = 5;
}

/// <summary>Parameters of the Elevated stage (sections 12 and 13).</summary>
public sealed record ElevationParameters
{
    public HeightMapParameters HeightMap { get; init; } = new();
    public BiomeParameters Biomes { get; init; } = new();
}

public sealed record HeightMapParameters
{
    public double ResolutionM { get; init; } = 1000;
    public double ChannelSlope { get; init; } = 0.01;
    public double Concavity { get; init; } = 0.4375;
    public double HillslopeMultiplier { get; init; } = 1;
    public double LevelFlatZones { get; init; } = 1;
    public double MountainBelts { get; init; } = 0.5;
    public double RegionRidgeM { get; init; } = 80;
    public double BasinDivideM { get; init; } = 30;
    public double DetailNoise { get; init; } = 1;
    public double ChannelDepthM { get; init; } = 3;
    public double SeaDepthM { get; init; } = 300;
    public PerElevationType<double> Hillslope { get; init; } = new(0.006, 0.06, 0.15, 0.277);
    public PerElevationType<double> RiverGradientFactor { get; init; } = new(0.7, 1, 1.8, 3);
    public FloodplainParameters Floodplain { get; init; } = new();
}

public sealed record FloodplainPoint(double BedHeightM, double HalfWidthM);

public sealed record FloodplainParameters
{
    public IReadOnlyList<FloodplainPoint> Points { get; init; } =
    [
        new(0, 400),
        new(100, 200),
        new(400, 90),
        new(1000, 30),
    ];
    public bool ScaleWithRiverSize { get; init; } = true;
}

public sealed record CoolingPoint(double HeightM, double CoolingC);

public sealed record BiomeParameters
{
    public IReadOnlyList<CoolingPoint> Cooling { get; init; } =
    [
        new(0, 0),
        new(200, 1),
        new(400, 5),
        new(900, 11),
        new(1250, 20),
    ];
    public double RiverPoorPrecipitationFactor { get; init; } = 0.2;
    public double MangroveMinTemperatureC { get; init; } = 20;
    public double MangroveMinAridity { get; init; } = 0.5;
}

/// <summary>Parameters of the Planned stage (section 15).</summary>
public sealed record PlanningParameters
{
    public int ProspectsPerRegion { get; init; } = 20;
    public int CityProspectsPerRegion { get; init; } = 4;
    public double SpacingKm { get; init; } = 20;
    public ProspectWeights Weights { get; init; } = new();
}

public sealed record ProspectWeights
{
    public double Resources { get; init; } = 0.3;
    public double Geography { get; init; } = 0.15;
    public double Trade { get; init; } = 0.25;
    public double Climate { get; init; } = 0.15;
    public double Prosperity { get; init; } = 0.15;
}

/// <summary>Parameters of the Settled stage (section 16).</summary>
public sealed record TravelParameters
{
    /// <summary>Only Prospects with at least this score are on show and get travel links.</summary>
    public double MinProspectScore { get; init; } = 59;
}

/// <summary>The four Fields. They have no generation stage.</summary>
public sealed record FieldsParameters
{
    public FieldParameters Precipitation { get; init; } = new(80, 2, 2.2, 0.55, 100, 12000);
    public FieldParameters InitialTemperature { get; init; } = new(110, 2, 2, 0.5, -10, 30);
    public FieldParameters Prosperity { get; init; } = new(200, 3, 2, 0.5, 1, 100);
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
    double FeatureSizeKm,
    double Octaves,
    double Lacunarity,
    double Gain,
    double Min,
    double Max);

/// <summary>A bedrock type and its weight under each elevation type.</summary>
public sealed record BedrockParameters(string Id, string Name, PerElevationType<int> Weights)
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
