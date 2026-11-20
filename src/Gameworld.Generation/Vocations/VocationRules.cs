using Gameworld.Domain;

namespace Gameworld.Generation.Vocations;

/// <summary>
/// The vocations and their tools (society-generation.md sections 8 and 8.1). It checks the two
/// lists of the Parameter Set when the world is created and answers which tools a vocation works
/// with. Nothing is drawn here: the vocations of a place and a person, and the emblems of a coat
/// of arms, are later steps that read these answers.
/// </summary>
public sealed class VocationRules
{
    private readonly Dictionary<string, VocationParameters> _vocations = new(StringComparer.Ordinal);
    private readonly Dictionary<string, ToolParameters[]> _toolsOf = new(StringComparer.Ordinal);

    /// <exception cref="ArgumentException">
    /// A vocation or a tool has no id or shares its id; a tool lists no vocation, a vocation that is
    /// not in the list, or an emblem weight that is not a number of 0 or more.
    /// </exception>
    public VocationRules(ParameterSet parameters)
    {
        foreach (var vocation in parameters.Vocations)
        {
            if (string.IsNullOrWhiteSpace(vocation.Id))
                throw new ArgumentException("A vocation of the Parameter Set has no id.", nameof(parameters));
            if (!_vocations.TryAdd(vocation.Id, vocation))
                throw new ArgumentException($"The Parameter Set lists the vocation id \"{vocation.Id}\" more than once.", nameof(parameters));
        }

        var toolIds = new HashSet<string>(StringComparer.Ordinal);
        foreach (var tool in parameters.Tools)
        {
            if (string.IsNullOrWhiteSpace(tool.Id))
                throw new ArgumentException("A tool of the Parameter Set has no id.", nameof(parameters));
            if (!toolIds.Add(tool.Id))
                throw new ArgumentException($"The Parameter Set lists the tool id \"{tool.Id}\" more than once.", nameof(parameters));
            if (tool.Vocations is not { Count: > 0 })
                throw new ArgumentException($"The tool \"{tool.Id}\" lists no vocation.", nameof(parameters));
            foreach (var vocation in tool.Vocations)
                if (vocation is null || !_vocations.ContainsKey(vocation))
                    throw new ArgumentException($"The tool \"{tool.Id}\" lists \"{vocation}\" in Vocations, which is not a vocation of the Parameter Set.", nameof(parameters));
            if (!(tool.EmblemWeight >= 0) || double.IsInfinity(tool.EmblemWeight))
                throw new ArgumentException($"The tool \"{tool.Id}\" has the emblem weight {tool.EmblemWeight}; it must be a number of 0 or more.", nameof(parameters));
        }

        // A tool that is off is no tool of any vocation, and a vocation that is off has no tools.
        foreach (var vocation in parameters.Vocations.Where(v => v.Enabled))
            _toolsOf[vocation.Id] = parameters.Tools
                .Where(t => t.Enabled && t.Vocations.Contains(vocation.Id, StringComparer.Ordinal))
                .ToArray();
    }

    /// <summary>
    /// The tools a vocation works with, in the order of the Tools list: the tools that are on and
    /// list the vocation. Empty for a vocation that is off or not in the list.
    /// </summary>
    public IReadOnlyList<ToolParameters> ToolsOf(string vocationId) =>
        _toolsOf.TryGetValue(vocationId, out var tools) ? tools : [];

    /// <summary>
    /// The tools of a vocation that can stand for it as an emblem: those of <see cref="ToolsOf"/>
    /// with an emblem weight above 0, in the order of the Tools list. A draw of an emblem weighs
    /// them by <see cref="ToolParameters.EmblemWeight"/>.
    /// </summary>
    public IReadOnlyList<ToolParameters> EmblemsOf(string vocationId) =>
        ToolsOf(vocationId).Where(t => t.EmblemWeight > 0).ToArray();
}
