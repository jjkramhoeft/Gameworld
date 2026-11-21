using System.ComponentModel;

namespace Gameworld.Domain;

/// <summary>
/// The texts of one biome. The biomes are fixed (the <see cref="Biome"/> enum) and the biome
/// rules decide which one a patch gets, so the list of the Parameter Set adds nothing to the
/// rules: it holds what a picture of the land is drawn from.
/// </summary>
[Description("The texts of one biome (§13).")]
public sealed partial record BiomeTextParameters(
    [property: Description("The biome the texts belong to.")] Biome Biome,
    [property: Description("The name shown for the biome.")] string Name)
{
    [Description("What the land of the biome looks like: its ground, its plant cover and its light. A picture of it is drawn from this text. No generation rule reads it. The text \"**missing description**\" marks one that is not written yet.")]
    public string Description { get; init; } = DefaultTexts.Missing;

    [Description("The lore of the biome: what the peoples of the world make of it and tell about it. No generation rule reads it. The text \"**missing description**\" marks one that is not written yet.")]
    public string Lore { get; init; } = DefaultTexts.Missing;

    private static BiomeTextParameters B(Biome biome, string name, string description = DefaultTexts.Missing, string lore = DefaultTexts.Missing) =>
        new(biome, name) { Description = description, Lore = lore };
}
