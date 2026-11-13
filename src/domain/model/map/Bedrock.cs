namespace Gameworld.Domain.Model;

/// <summary>Identity of a bedrock type in the catalog, e.g. <c>shield</c> or <c>foldBelt</c>.</summary>
public readonly record struct BedrockId(string Value)
{
    public override string ToString() => Value;
}

/// <summary>
/// A bedrock type. A zone has one bedrock, and it drives the mined resources in its patches.
/// What a bedrock gives belongs to the resource half of the model.
/// </summary>
public sealed record Bedrock
{
    public required BedrockId Id { get; init; }
    public required string Name { get; init; }

    /// <summary>Relative weight of this bedrock under each land elevation type. Zero means never.</summary>
    public required PerLandElevation<double> Weights { get; init; }
}
