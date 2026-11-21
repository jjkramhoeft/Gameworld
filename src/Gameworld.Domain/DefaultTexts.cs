namespace Gameworld.Domain;

/// <summary>
/// The description and the lore of the default races, vocations, tools, flora and fauna, keyed
/// by id. They are the texts a picture of the thing is to be drawn from, so that an invented
/// thing looks the same every time. Each list is a file of its own, built from the basis doc
/// its header names; change a text there and in docs/default-parameter-set.json together.
/// </summary>
public static partial class DefaultTexts
{
    /// <summary>What a description or a lore holds until it is written.</summary>
    public const string Missing = "**missing description**";

    /// <summary>Whether a description or a lore is still to be written.</summary>
    public static bool IsMissing(string? text) => string.IsNullOrWhiteSpace(text) || text == Missing;

    internal static (string Description, string Lore) Of(Dictionary<string, (string Description, string Lore)> texts, string id) =>
        texts.TryGetValue(id, out var text) ? text : (Missing, Missing);
}
