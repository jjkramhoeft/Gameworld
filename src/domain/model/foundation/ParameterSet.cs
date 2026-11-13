using System.Collections.Immutable;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gameworld.Domain.Model;

/// <summary>Identity of a <see cref="ParameterSet"/>. A content hash of all its values.</summary>
public readonly record struct ParameterSetId(string Value)
{
    public override string ToString() => Value;
}

/// <summary>
/// An immutable set of all map and world parameters. Referenced by a foundation.
/// Only the map half is modelled so far - the settlement, people and resource parameters
/// are added with that half.
/// </summary>
public sealed record ParameterSet
{
    public required SitingParameters Siting { get; init; }
    public required OutlineParameters Outline { get; init; }
    public required DetailParameters Detail { get; init; }
    public required ElevationParameters Elevation { get; init; }
    public required FieldParameters Fields { get; init; }

    /// <summary>The bedrock catalog. Order is significant for weighted picks.</summary>
    public required ImmutableArray<Bedrock> Bedrocks { get; init; }

    /// <summary>The biome catalog.</summary>
    public required ImmutableArray<Biome> Biomes { get; init; }

    private static readonly JsonSerializerOptions CanonicalJson = new()
    {
        Converters = { new JsonStringEnumConverter() },
    };

    /// <summary>
    /// The content hash of this parameter set. Two sets with the same values have the same id.
    /// Computed on each call - a <see cref="Foundation"/> computes it once and keeps it.
    /// </summary>
    public ParameterSetId ComputeId()
    {
        var content = JsonSerializer.SerializeToUtf8Bytes(this, CanonicalJson);
        return new(Convert.ToHexStringLower(SHA256.HashData(content).AsSpan(0, 16)));
    }
}
