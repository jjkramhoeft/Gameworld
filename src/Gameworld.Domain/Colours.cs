using System.ComponentModel;
using System.Globalization;

namespace Gameworld.Domain;

/// <summary>
/// A colour: a named colour of cloth, paint and banners. The colours are a list in the Parameter
/// Set, like the races, so the code knows no colour by name. A race has favourite colours and a
/// faction has a main and a secondary colour (society-generation.md sections 2, 4.9 and 9.1).
/// </summary>
[Description("A colour: a named colour of cloth, paint and banners. Races point at their favourite colours and factions at their main and secondary colour (society §9.1).")]
public sealed record ColourParameters(
    [property: Description("The id the generated data knows the colour by, in camelCase like skyBlue. Unique in the list; keep it when the name changes.")] string Id,
    [property: Description("The name shown for the colour.")] string Name,
    [property: Description("The colour as #RRGGBB: six hexadecimal digits after a #. It is what a picture shows, and its lightness decides which secondary colour stands out against a main colour (society §4.9).")] string Hex)
{
    /// <summary>
    /// The default list (society-generation.md section 9.1): 24 colours a dyer, a painter or a
    /// smith can make, from undyed cloth to the costly dyes.
    /// </summary>
    public static IReadOnlyList<ColourParameters> Defaults { get; } =
    [
        new("white", "White", "#F2F0E6"),
        new("cream", "Cream", "#E8DCC0"),
        new("silver", "Silver", "#C0C4C8"),
        new("grey", "Grey", "#8A8D8F"),
        new("black", "Black", "#1E1E22"),
        new("tan", "Tan", "#C8A977"),
        new("brown", "Brown", "#6B4423"),
        new("russet", "Russet", "#8B3A1E"),
        new("ochre", "Ochre", "#C98A2B"),
        new("gold", "Gold", "#D4A017"),
        new("yellow", "Yellow", "#F2CB05"),
        new("saffron", "Saffron", "#F28C0F"),
        new("orange", "Orange", "#E0651A"),
        new("red", "Red", "#C1272D"),
        new("crimson", "Crimson", "#8C1127"),
        new("rose", "Rose", "#D9778B"),
        new("purple", "Purple", "#6A2C8C"),
        new("indigo", "Indigo", "#2B2D6E"),
        new("blue", "Blue", "#1F5FA8"),
        new("skyBlue", "Sky blue", "#7FB6DE"),
        new("teal", "Teal", "#1F7A7A"),
        new("green", "Green", "#2E8B3D"),
        new("forestGreen", "Forest green", "#1F4D2B"),
        new("olive", "Olive", "#6B7A2A"),
    ];

    /// <summary>
    /// The lightness of a colour given as #RRGGBB, from 0 (black) to 1 (white):
    /// (0.299 · R + 0.587 · G + 0.114 · B) / 255. False when the text is not a colour.
    /// </summary>
    public static bool TryLightness(string? hex, out double lightness)
    {
        lightness = 0;
        if (hex is null || hex.Length != 7 || hex[0] != '#') return false;
        if (!int.TryParse(hex.AsSpan(1), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out var rgb)) return false;
        lightness = (0.299 * ((rgb >> 16) & 0xff) + 0.587 * ((rgb >> 8) & 0xff) + 0.114 * (rgb & 0xff)) / 255;
        return true;
    }
}
