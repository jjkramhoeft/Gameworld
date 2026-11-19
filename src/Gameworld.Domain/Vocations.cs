using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Gameworld.Domain;

/// <summary>The nine sectors the vocations are grouped in.</summary>
public enum VocationSector
{
    LandAndWater,
    Processing,
    Craft,
    TradeAndTransport,
    Service,
    GovernanceAndWar,
    FaithAndLearning,
    Supernatural,
    Margins,
}

/// <summary>
/// The tiers of a place, lowest first. A tier follows from what the place is, not from its
/// head count.
/// </summary>
public enum SettlementTier
{
    Countryside,
    Town,
    City,
    Capital,
}

/// <summary>
/// A vocation: the work a person does. The vocations are a list in the Parameter Set, like the
/// races, so the code knows no vocation by name. How the vocations of a place and of a person
/// are drawn is not specified yet (society-generation.md section 8).
/// </summary>
[Description("A vocation: the work a person does. The vocations are not drawn for places or persons yet (society §8).")]
public sealed partial record VocationParameters(
    [property: Description("The id the generated data knows the vocation by. Unique in the list; keep it when the name changes.")] string Id,
    [property: Description("The name shown for the vocation.")] string Name,
    [property: Description("Whether the vocation exists in the world.")] bool Enabled,
    [property: Description("The sector of the vocation: one of nine (society §8).")] VocationSector Sector,
    [property: Description("The lowest tier of place in which the vocation is found: Countryside, Town, City or Capital (society §8).")] SettlementTier LowestTier)
{
    [Description("The starting share of the labour force of a whole region that holds the vocation, in per cent. The shares of the defaults sum to 100. They are starting values, not tuned (society §8).")]
    [Range(0, 100)]
    public double SharePercent { get; init; }

    [Description("Whether the vocation is only found in a place on water: the shipwright and the sailor (society §8).")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool OnWater { get; init; }

    [Description("The names of the variants of the vocation: the smaller trades it covers, such as the shepherd of the herder and the armourer of the smith (society §8).")]
    public IReadOnlyList<string> Variants { get; init; } = [];

    private static VocationParameters Make(
        string id, string name, VocationSector sector, SettlementTier lowestTier, double sharePercent, bool onWater, string[] variants) =>
        new(id, name, true, sector, lowestTier)
        {
            SharePercent = sharePercent,
            OnWater = onWater,
            Variants = variants,
        };
}
