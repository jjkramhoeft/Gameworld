using System.Security.Cryptography;
using System.Text;

namespace Gameworld.Domain.Model;

/// <summary>The seed for all the generation algorithms of a foundation.</summary>
public readonly record struct WorldSeed(ulong Value)
{
    public override string ToString() => Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
}

/// <summary>
/// Versions the generation algorithms only - not the parameters and not the storage schema.
/// </summary>
public readonly record struct GeneratorVersion(int Value)
{
    /// <summary>The version of the generation algorithms in this build.</summary>
    public static readonly GeneratorVersion Current = new(1);

    public override string ToString() => Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
}

/// <summary>
/// Identity of a <see cref="Foundation"/>. A content hash of seed, generator version and
/// parameter set, so the same inputs always give the same foundation.
/// </summary>
public readonly record struct FoundationId(string Value)
{
    public override string ToString() => Value;
}

/// <summary>
/// The root of a game world: seed, generator version and exactly one parameter set.
/// A foundation is immutable. Changing anything means creating a new foundation, so every
/// generated map can be traced to exactly what produced it.
/// </summary>
public sealed class Foundation : IEquatable<Foundation>
{
    public Foundation(WorldSeed seed, GeneratorVersion generatorVersion, ParameterSet parameters, string? label = null)
    {
        Seed = seed;
        GeneratorVersion = generatorVersion;
        Parameters = parameters;
        ParameterSetId = parameters.ComputeId();
        Label = label;
        Id = ComputeId(seed, generatorVersion, ParameterSetId);
    }

    public FoundationId Id { get; }
    public WorldSeed Seed { get; }
    public GeneratorVersion GeneratorVersion { get; }
    public ParameterSet Parameters { get; }
    public ParameterSetId ParameterSetId { get; }

    /// <summary>A free-text name for humans. Not part of the identity.</summary>
    public string? Label { get; }

    public bool Equals(Foundation? other) => other is not null && Id == other.Id;
    public override bool Equals(object? obj) => Equals(obj as Foundation);
    public override int GetHashCode() => Id.GetHashCode();
    public override string ToString() => Label is null ? Id.Value : $"{Label} ({Id.Value})";

    private static FoundationId ComputeId(WorldSeed seed, GeneratorVersion version, ParameterSetId parameters)
    {
        var content = Encoding.UTF8.GetBytes(FormattableString.Invariant($"{seed.Value}/{version.Value}/{parameters.Value}"));
        return new(Convert.ToHexStringLower(SHA256.HashData(content).AsSpan(0, 16)));
    }
}
