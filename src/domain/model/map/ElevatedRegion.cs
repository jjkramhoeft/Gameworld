using System.Collections.Immutable;

namespace Gameworld.Domain.Model;

/// <summary>
/// A region with heights and biomes. The resources of the patches also belong to this stage,
/// and are added with the resource half of the model.
/// </summary>
/// <param name="Detailed">The region as it was Detailed.</param>
/// <param name="Terrain">Height, climate and biome of each land patch. Ocean patches have no entry.</param>
/// <param name="RiverProfiles">The heights along each river.</param>
public sealed record ElevatedRegion(
    DetailedRegion Detailed,
    ImmutableDictionary<PatchId, PatchTerrain> Terrain,
    ImmutableArray<RiverProfile> RiverProfiles) : IRegionStage
{
    public RegionId Id => Detailed.Id;
}

/// <summary>The terrain of one land patch.</summary>
/// <param name="HeightM">The height of the patch in metres, within the band of its zone's elevation type.</param>
/// <param name="Climate">The climate values the stage sampled for the patch.</param>
/// <param name="Biome">The biome of the patch.</param>
public sealed record PatchTerrain(double HeightM, ClimateSample Climate, BiomeId Biome);

/// <summary>
/// The climate of a patch, recorded when the region was Elevated. Temperature is the
/// Initial Temperature field adjusted for height.
/// </summary>
public readonly record struct ClimateSample(double Precipitation, double Temperature);

/// <summary>The heights along one river, in metres.</summary>
/// <param name="River">The river.</param>
/// <param name="NodeHeightsM">One height per node, in the order of <see cref="Model.River.Nodes"/>.</param>
public sealed record RiverProfile(RiverId River, ImmutableArray<double> NodeHeightsM);
