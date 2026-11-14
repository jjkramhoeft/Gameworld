using System.Text;
using System.Text.Json;
using Gameworld.Generation.Basics;

namespace Gameworld.Generation.Names;

/// <summary>
/// One trained name model: a character-level Markov chain with up to three characters of
/// context (docs/NameGenerator-Reuse.md, "How it works"). Immutable after loading.
/// </summary>
public sealed class MarkovNameModel
{
    private const char Start = '£'; // the pound sign; escaped so the file encoding cannot break it
    private const char End = '$';
    private const int Order = 3;
    private const int MaxLength = 30; // includes the start marker, so a name is at most 29 characters

    private readonly Dictionary<string, Candidate[]> _contexts;

    private MarkovNameModel(Dictionary<string, Candidate[]> contexts) => _contexts = contexts;

    /// <summary>Reads one <c>*_names_sets.json</c> file.</summary>
    public static MarkovNameModel Load(Stream utf8Json)
    {
        var sets = JsonSerializer.Deserialize<Dictionary<string, TokenSet>>(utf8Json)
            ?? throw new InvalidDataException("The name model json is null.");
        if (sets.Count == 0) throw new InvalidDataException("The name model is empty.");

        // The order of the candidates decides which name a random number gives: keep it as in the file.
        var contexts = new Dictionary<string, Candidate[]>(sets.Count, StringComparer.Ordinal);
        foreach (var (context, set) in sets)
            contexts[context] = set.NextTokenFreq.Select(t => new Candidate(t.C, t.F)).ToArray();
        return new MarkovNameModel(contexts);
    }

    /// <summary>
    /// One name from the repo's own generator. This is the overload for generation code: seed
    /// the <see cref="Sfc32"/> from a <see cref="Hashing"/> hash of the entity and the purpose.
    /// </summary>
    public string Generate(Sfc32 rng) => Generate(rng.Next);

    /// <summary>
    /// One name from <c>new Random(seed)</c>: the same name the original generator gave for the
    /// seed. Kept to check the port and the data files against the reference names. Neighbouring
    /// seeds give related names, and .NET does not promise the sequence across versions, so
    /// generation code uses the <see cref="Sfc32"/> overload.
    /// </summary>
    public string Generate(int seed) => Generate(new Random(seed).NextDouble);

    private string Generate(Func<double> nextUnit)
    {
        var name = new StringBuilder(MaxLength).Append(Start);
        while (name.Length < MaxLength && name[^1] != End)
        {
            var length = Math.Min(Order, name.Length);
            var context = name.ToString(name.Length - length, length);
            // The retry and the '#' fallback mirror the original, so that a seed keeps its name.
            var next = GetNext(context, nextUnit) ?? (context.Length < 2 ? '#' : GetNext(context, nextUnit));
            if (next is not null) name.Append(next.Value);
        }
        return Capitalize(name.ToString().Trim(Start).Trim(End));
    }

    private char? GetNext(string context, Func<double> nextUnit)
    {
        while (true)
        {
            if (_contexts.TryGetValue(context, out var candidates))
            {
                // 32-bit floats on purpose: the sums of the original decide the name of a seed.
                var f = (float)nextUnit();
                float runningSum = 0;
                foreach (var candidate in candidates)
                {
                    runningSum += candidate.F;
                    if (f < runningSum) return candidate.C;
                }
                return null;
            }
            if (context.Length < 2) return null;
            // A missing context drops its newest character, as the original does.
            context = context[..^1];
        }
    }

    /// <summary>Upper-cases the first letter and the letter after each hyphen.</summary>
    private static string Capitalize(string name)
    {
        var chars = name.ToCharArray();
        for (var i = 0; i < chars.Length; i++)
            if (i == 0 || chars[i - 1] == '-')
                chars[i] = char.ToUpperInvariant(chars[i]);
        return new string(chars);
    }

    private readonly record struct Candidate(char C, float F);

    // The property names must match the json exactly (System.Text.Json is case-sensitive).
    private sealed class TokenSet
    {
        public string PreTokens { get; set; } = "";
        public List<TokenFrequency> NextTokenFreq { get; set; } = [];
    }

    private sealed class TokenFrequency
    {
        public char C { get; set; }
        public float F { get; set; }
    }
}
