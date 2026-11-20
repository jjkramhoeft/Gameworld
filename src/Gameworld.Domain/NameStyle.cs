namespace Gameworld.Domain;

/// <summary>
/// Which name model the names of a race are drawn from (society-generation.md sections 2 and 7).
/// Each style is three pregenerated models of the name generator: male, female and surname.
/// </summary>
public enum NameStyle
{
    /// <summary>British given names and surnames (files: GB_*).</summary>
    Common,
    /// <summary>Elven (files: elven_*).</summary>
    Exotic,
    /// <summary>Dark elven (files: darkelve_*).</summary>
    Dark,
}
