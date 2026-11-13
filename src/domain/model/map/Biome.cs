namespace Gameworld.Domain.Model;

/// <summary>Identity of a biome in the catalog, e.g. <c>borealForest</c>.</summary>
public readonly record struct BiomeId(string Value)
{
    public override string ToString() => Value;
}

/// <summary>
/// A biome. A land patch has one biome, and it drives the patch flora and fauna.
/// How geography and climate pick a biome is added together with the biome algorithm.
/// </summary>
public sealed record Biome
{
    public required BiomeId Id { get; init; }
    public required string Name { get; init; }

    /// <summary>The colour of the biome on the map.</summary>
    public required Rgb Color { get; init; }
}
