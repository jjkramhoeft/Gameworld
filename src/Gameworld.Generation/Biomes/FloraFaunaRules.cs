using Gameworld.Domain;
using Gameworld.Generation.Basics;

namespace Gameworld.Generation.Biomes;

/// <summary>
/// The flora and fauna of a patch (map-generation.md section 13.4): a few of the kinds of the
/// Parameter Set that fit the patch, drawn by their frequency. A kind fits a land patch when it
/// lists the biome of the patch, and an ocean patch when it is of the sea; in both cases the
/// Supernatural of the patch must reach what the kind needs. A kind of flora can stand in for a
/// type: a patch that gets it gets no kind of that type.
/// </summary>
public sealed class FloraFaunaRules
{
    /// <summary>The purpose of the number of kinds of flora of a patch.</summary>
    public const uint FloraCountPurpose = 0xf10a;
    /// <summary>The purpose of the number of kinds of fauna of a patch.</summary>
    public const uint FaunaCountPurpose = 0xfa0a;
    /// <summary>The purpose of the draw of the first kind of flora of the list. The kind at index k has this + k.</summary>
    public const uint FloraKindPurpose = 0xf10000;
    /// <summary>The purpose of the draw of the first kind of fauna of the list. The kind at index k has this + k.</summary>
    public const uint FaunaKindPurpose = 0xfa0000;

    private const int SeaBit = 16;

    private const int NoType = -1;

    /// <param name="Type">The type of a kind of flora; NoType for a kind of fauna.</param>
    /// <param name="StandsInFor">The type of flora the kind stands in for; NoType when it stands in for nothing.</param>
    private readonly record struct Kind(
        string Id, int Index, uint Habitats, double Frequency, double MinSupernatural, int Type = NoType, int StandsInFor = NoType);

    private readonly Kind[] _flora;
    private readonly Kind[] _fauna;
    private readonly IntRange _floraPerPatch;
    private readonly IntRange _faunaPerPatch;

    public FloraFaunaRules(ParameterSet parameters)
    {
        _flora = parameters.Flora
            .Select((f, k) => (Kind: new Kind(
                f.Id, k, Habitats(f.Biomes, f.Sea), f.Frequency, f.MinSupernatural,
                (int)f.Type, f.StandsInFor is { } type && type != f.Type ? (int)type : NoType), f.Enabled))
            .Where(x => x.Enabled && x.Kind.Frequency > 0)
            .Select(x => x.Kind)
            .ToArray();
        _fauna = parameters.Fauna
            .Select((f, k) => (Kind: new Kind(f.Id, k, Habitats(f.Biomes, f.Sea), f.Frequency, f.MinSupernatural), f.Enabled))
            .Where(x => x.Enabled && x.Kind.Frequency > 0)
            .Select(x => x.Kind)
            .ToArray();
        _floraPerPatch = parameters.Elevation.Biomes.FloraPerPatch;
        _faunaPerPatch = parameters.Elevation.Biomes.FaunaPerPatch;
    }

    /// <summary>The ids of the kinds of flora of a patch, in the order of the Parameter Set.</summary>
    /// <param name="biome">The biome of a land patch; null for an ocean patch.</param>
    /// <param name="supernatural">The Supernatural field at the site of the patch, 0 to 100.</param>
    public IReadOnlyList<string> Flora(uint seed, GridCoord patch, Biome? biome, double supernatural) =>
        Draw(_flora, _floraPerPatch, FloraCountPurpose, FloraKindPurpose, seed, patch, biome, supernatural);

    /// <summary>The ids of the kinds of fauna of a patch, in the order of the Parameter Set.</summary>
    /// <param name="biome">The biome of a land patch; null for an ocean patch.</param>
    /// <param name="supernatural">The Supernatural field at the site of the patch, 0 to 100.</param>
    public IReadOnlyList<string> Fauna(uint seed, GridCoord patch, Biome? biome, double supernatural) =>
        Draw(_fauna, _faunaPerPatch, FaunaCountPurpose, FaunaKindPurpose, seed, patch, biome, supernatural);

    private static uint Habitats(IReadOnlyList<Biome> biomes, bool sea)
    {
        var mask = sea ? 1u << SeaBit : 0u;
        foreach (var biome in biomes) mask |= 1u << (int)biome;
        return mask;
    }

    /// <summary>
    /// Every kind that fits gets the key −ln(1 − u) ÷ frequency from its own hash of the patch,
    /// and the kinds with the smallest keys are the kinds of the patch. A kind is so drawn in
    /// proportion to its frequency, and its key does not depend on the other kinds or on the
    /// biome: a patch whose estimated and final biome are the same has the same kinds in both forms.
    /// The kinds are taken in the order of their keys. A kind that stands in for a type takes the
    /// place of the kinds of that type: those taken before it are left out again, and those after
    /// it are passed over. A kind that stands in for a type is itself never left out.
    /// </summary>
    private static IReadOnlyList<string> Draw(
        Kind[] kinds, IntRange perPatch, uint countPurpose, uint kindPurpose,
        uint seed, GridCoord patch, Biome? biome, double supernatural)
    {
        var count = Hashing.Between(
            Math.Max(0, perPatch.Min), Math.Max(0, perPatch.Max), Hashing.Unit(seed, countPurpose, patch.I, patch.J));
        if (count == 0) return [];

        var habitat = 1u << (biome is { } b ? (int)b : SeaBit);
        var drawn = new List<(double Key, int At)>();
        for (var at = 0; at < kinds.Length; at++)
        {
            var kind = kinds[at];
            if ((kind.Habitats & habitat) == 0 || supernatural < kind.MinSupernatural) continue;
            var u = Hashing.Unit(seed, kindPurpose + (uint)kind.Index, patch.I, patch.J);
            drawn.Add((-DetMath.Ln(1 - u) / kind.Frequency, at));
        }
        drawn.Sort();

        var taken = new List<int>(count);
        var replaced = 0u; // a bit per type of flora that a taken kind stands in for
        foreach (var (_, at) in drawn)
        {
            if (taken.Count == count) break;
            var kind = kinds[at];
            var standsIn = kind.StandsInFor != NoType;
            if (!standsIn && kind.Type != NoType && (replaced & 1u << kind.Type) != 0) continue;
            taken.Add(at);
            if (!standsIn) continue;
            replaced |= 1u << kind.StandsInFor;
            taken.RemoveAll(t => kinds[t].StandsInFor == NoType && kinds[t].Type == kind.StandsInFor);
        }
        taken.Sort();
        return taken.Select(at => kinds[at].Id).ToArray();
    }
}
