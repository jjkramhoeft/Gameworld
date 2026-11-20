using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Gameworld.Domain;

/// <summary>
/// A tool: a thing a vocation works with, such as the anvil of the smith or the sickle of the
/// farmer. The tools are a list in the Parameter Set, like the vocations, so the code knows no
/// tool by name. A tool names the vocations that work with it. Nothing draws a tool yet: the
/// coats of arms of families, guilds and some cities are to take their charges from the tools
/// of a vocation, and the portrait of a person can show them (society-generation.md
/// section 8.1).
/// </summary>
[Description("A tool: a thing a vocation works with, and the vocations that work with it. Tools are not drawn yet; they are meant for the coats of arms of families, guilds and some cities (society §8.1).")]
public sealed partial record ToolParameters(
    [property: Description("The id the generated data knows the tool by, in camelCase like heavyPlough. Unique in the list; keep it when the name changes.")] string Id,
    [property: Description("The name shown for the tool.")] string Name,
    [property: Description("Whether the tool exists in the world. A tool that is off is no tool of any vocation.")] bool Enabled,
    [property: Description("The ids of the vocations that work with the tool, the vocation it belongs to most first. Every id must be a vocation of the Vocations list, and a tool lists at least one (society §8.1).")] IReadOnlyList<string> Vocations)
{
    [Description("How well the tool serves as the emblem of its vocations on a coat of arms, a banner or a sign: the weight it has when an emblem is drawn among the tools of a vocation. 0 means never: a tool that cannot be drawn as one plain figure, such as a kiln or a pit. The defaults run from 0 to 3, and 3 is the tool that is the sign of its trade, such as the anvil of the smith (society §8.1).")]
    [Range(0, 10)]
    public double EmblemWeight { get; init; } = 1;

    private static ToolParameters Make(string id, string name, double emblemWeight, string[] vocations) =>
        new(id, name, true, vocations) { EmblemWeight = emblemWeight };
}
