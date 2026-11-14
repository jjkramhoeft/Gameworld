using System.Globalization;

namespace Gameworld.Domain;

/// <summary>
/// The seed for all the generation algorithms: the seed text and the number made from it.
/// A text of 1–9 digits is used as a number, anything else is hashed (xmur3).
/// </summary>
public readonly record struct Seed
{
    public const string DefaultText = "cellgen";

    public string Text { get; }
    public uint Value { get; }

    private Seed(string text, uint value)
    {
        Text = text;
        Value = value;
    }

    public static Seed Default => FromText(DefaultText);

    public static Seed FromText(string text)
    {
        var trimmed = (text ?? string.Empty).Trim();
        if (trimmed.Length is >= 1 and <= 9 && trimmed.All(c => c is >= '0' and <= '9'))
            return new Seed(trimmed, uint.Parse(trimmed, CultureInfo.InvariantCulture));
        return new Seed(trimmed, HashString(trimmed));
    }

    /// <summary>xmur3 over the UTF-16 code units of the text.</summary>
    private static uint HashString(string s)
    {
        unchecked
        {
            uint h = 1779033703u ^ (uint)s.Length;
            foreach (var c in s)
            {
                h = (h ^ c) * 3432918353u;
                h = (h << 13) | (h >> 19);
            }
            h = (h ^ (h >> 16)) * 2246822507u;
            h = (h ^ (h >> 13)) * 3266489909u;
            return h ^ (h >> 16);
        }
    }

    public override string ToString() => Text;
}

/// <summary>
/// Versions the generation algorithms only - not the parameters and not the storage schema.
/// </summary>
public readonly record struct GeneratorVersion(int Number)
{
    public override string ToString() => Number.ToString(CultureInfo.InvariantCulture);
}

/// <summary>
/// The root of the game world: the seed, the generator version and exactly one Parameter Set.
/// All derived data is a pure function of these three.
/// </summary>
/// <remarks>
/// A foundation is a value. During phase 1 the parameters can be changed and reloaded with
/// <see cref="WithParameters"/> or <see cref="ReloadParameters"/>; the result is a foundation whose
/// derived data must be discarded and recomputed. After phase 1 a foundation never changes.
/// </remarks>
public sealed record Foundation(Seed Seed, GeneratorVersion GeneratorVersion, ParameterSet Parameters)
{
    /// <summary>Phase 1 only: the same seed and generator version with another Parameter Set.</summary>
    public Foundation WithParameters(ParameterSet parameters) => this with { Parameters = parameters };

    /// <summary>Phase 1 only: reload all parameters from a json file.</summary>
    public Foundation ReloadParameters(string path) => WithParameters(ParameterSet.Load(path));

    /// <summary>Save all parameters to a json file.</summary>
    public void SaveParameters(string path) => Parameters.Save(path);
}
