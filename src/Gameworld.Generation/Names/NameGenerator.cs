using Gameworld.Domain;
using Gameworld.Generation.Basics;

namespace Gameworld.Generation.Names;

public enum NameKind { Male, Female, Surname }

/// <summary>
/// The person-name generator (docs/NameGenerator-Reuse.md): nine pre-generated Markov models,
/// one per <see cref="NameStyle"/> and <see cref="NameKind"/>. The style maps to the file
/// prefix, and a race says which style its names have. The models are the files of
/// <c>pregenerated-data/</c>, embedded in this assembly. Immutable and safe to share.
/// </summary>
public sealed class NameGenerator
{
    private static readonly (NameStyle Style, string Prefix)[] Styles =
        [(NameStyle.Common, "GB"), (NameStyle.Exotic, "elven"), (NameStyle.Dark, "darkelve")];

    private static readonly (NameKind Kind, string Name)[] Kinds =
        [(NameKind.Male, "male"), (NameKind.Female, "female"), (NameKind.Surname, "sur")];

    private static readonly Lazy<NameGenerator> Shared = new(Load);

    private readonly Dictionary<(NameStyle, NameKind), MarkovNameModel> _models;

    private NameGenerator(Dictionary<(NameStyle, NameKind), MarkovNameModel> models) => _models = models;

    /// <summary>The generator of the embedded models. They are loaded on first use.</summary>
    public static NameGenerator Default => Shared.Value;

    private static NameGenerator Load()
    {
        var assembly = typeof(NameGenerator).Assembly;
        var models = new Dictionary<(NameStyle, NameKind), MarkovNameModel>();
        foreach (var (style, prefix) in Styles)
            foreach (var (kind, kindName) in Kinds)
            {
                // The resource name is the file name as it is on disk, with this namespace in front.
                var resource = $"{typeof(NameGenerator).Namespace}.{prefix}_{kindName}_names_sets.json";
                using var stream = assembly.GetManifestResourceStream(resource)
                    ?? throw new InvalidOperationException($"The name model '{resource}' is not embedded.");
                models[(style, kind)] = MarkovNameModel.Load(stream);
            }
        return new NameGenerator(models);
    }

    /// <summary>
    /// One name, drawn from <paramref name="rng"/>. Seed the generator from a
    /// <see cref="Hashing"/> hash of the person and the purpose, so that the name is a pure
    /// function of the foundation.
    /// </summary>
    public string Generate(NameStyle style, NameKind kind, Sfc32 rng) => _models[(style, kind)].Generate(rng);

    /// <summary>Given name and surname from one random stream, e.g. "Usman Carry".</summary>
    public string GenerateFullName(NameStyle style, NameKind givenNameKind, Sfc32 rng) =>
        Generate(style, givenNameKind, rng) + " " + Generate(style, NameKind.Surname, rng);

    /// <summary>
    /// One name with the seeding of the original generator: the same seed gives the same name
    /// as it did there. See <see cref="MarkovNameModel.Generate(int)"/> for why generation code
    /// does not use it.
    /// </summary>
    public string Generate(NameStyle style, NameKind kind, int seed) => _models[(style, kind)].Generate(seed);

    /// <summary>"Davies" -> "Davies'", "Owen" -> "Owen's".</summary>
    public static string Genitive(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return name;
        return name.EndsWith('s') ? name + "'" : name + "'s";
    }
}
