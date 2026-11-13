namespace Gameworld.Domain.Model;

/// <summary>An inclusive range of whole numbers that a count is rolled from.</summary>
public readonly record struct IntRange(int Min, int Max);

/// <summary>The lowest and highest height, in metres, a land zone of one elevation type may have.</summary>
public readonly record struct HeightBand(double MinM, double MaxM);

/// <summary>One value for each of the four land elevation types.</summary>
public readonly record struct PerLandElevation<T>(T Flat, T Lowland, T Hills, T Mountains)
{
    public T this[ElevationType type] => type switch
    {
        ElevationType.Flat => Flat,
        ElevationType.Lowland => Lowland,
        ElevationType.Hills => Hills,
        ElevationType.Mountains => Mountains,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Only land elevation types have a value."),
    };
}

/// <summary>Parameters for what a region gets when it is Sited.</summary>
public sealed record SitingParameters
{
    /// <summary>Side length of the region grid, in km (approximately 100-200).</summary>
    public required double RegionGridSizeKm { get; init; }

    /// <summary>How far a region site may move from its grid centre, as a fraction of the grid size.</summary>
    public required double RegionJitter { get; init; }

    /// <summary>Number of zone cells per region (approximately 8-12).</summary>
    public required IntRange ZoneCellsPerRegion { get; init; }

    /// <summary>Number of patch cells per zone cell (approximately 8-18).</summary>
    public required IntRange PatchCellsPerZoneCell { get; init; }

    /// <summary>Number of zone cells per region that start as ocean seeds.</summary>
    public required IntRange OceanSeedsPerRegion { get; init; }

    /// <summary>Relative weight of each land elevation type when a land zone cell picks its type.</summary>
    public required PerLandElevation<double> ElevationWeights { get; init; }
}

/// <summary>Parameters for ocean linking, ocean expansion and zone mergers.</summary>
public sealed record OutlineParameters
{
    /// <summary>Chance (0-1) that an ocean seed links to its nearest ocean seed.</summary>
    public required double OceanLinkChance { get; init; }

    /// <summary>An ocean seed with no other seed within this distance does not link.</summary>
    public required double MaxOceanLinkDistanceKm { get; init; }

    /// <summary>Number of expansions per ocean seed.</summary>
    public required IntRange ExpansionsPerOceanSeed { get; init; }

    /// <summary>Number of zone mergers attempted per region.</summary>
    public required IntRange ZoneMergersPerRegion { get; init; }
}

/// <summary>
/// Parameters for patch mergers and rivers. The river (space-colonization) parameters are
/// added together with the river algorithm.
/// </summary>
public sealed record DetailParameters
{
    /// <summary>Number of patch mergers per land zone.</summary>
    public required IntRange PatchMergersPerZone { get; init; }
}

/// <summary>Parameters for heights and biomes.</summary>
public sealed record ElevationParameters
{
    /// <summary>The band the height of a land zone must stay within, per elevation type.</summary>
    public required PerLandElevation<HeightBand> HeightBands { get; init; }
}

/// <summary>The shape of one noise map.</summary>
public sealed record NoiseParameters
{
    /// <summary>The size of the largest features, in km.</summary>
    public required double ScaleKm { get; init; }
    public required int Octaves { get; init; }
    public required double Persistence { get; init; }
    public required double Lacunarity { get; init; }
}

/// <summary>Noise parameters for each <see cref="FieldKind"/>.</summary>
public sealed record FieldParameters
{
    public required NoiseParameters Precipitation { get; init; }
    public required NoiseParameters InitialTemperature { get; init; }
    public required NoiseParameters Prosperity { get; init; }
    public required NoiseParameters Supernatural { get; init; }

    public NoiseParameters this[FieldKind kind] => kind switch
    {
        FieldKind.Precipitation => Precipitation,
        FieldKind.InitialTemperature => InitialTemperature,
        FieldKind.Prosperity => Prosperity,
        FieldKind.Supernatural => Supernatural,
        _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null),
    };
}
