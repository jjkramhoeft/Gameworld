namespace Gameworld.Domain.Model;

/// <summary>An sRGB colour.</summary>
public readonly record struct Rgb(byte R, byte G, byte B)
{
    public string Hex => $"#{R:x2}{G:x2}{B:x2}";
}