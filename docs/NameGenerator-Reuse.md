# Reusing the NameGenerator in another repo

This guide covers lifting the person-name generator out of IdleGame: what to copy, a self-contained version of the code that drops the IdleGame dependencies, how the data files work, and the behaviour you need to know about before relying on it.

The generator is a character-level Markov chain. Each of the nine files in [Storage/PreGenData](../Storage/PreGenData) is one trained model, and generating a name is a short random walk through it. There is no training code in this repo, only the finished models.

The code in this guide was compiled and run against the original `Generate` method from [Generator/NameGenerator.cs](../Generator/NameGenerator.cs). It returned the same name for every seed from 0 to 299,999 on all nine files (2.7 million names), on both .NET 8.0.34 and .NET 10.0.15.

## What to take and what to leave

| Item | Where | Take it? |
|---|---|---|
| Nine `*_names_sets.json` files (1.44 MB in total) | [Storage/PreGenData](../Storage/PreGenData) | Yes, unchanged |
| `Generate`, `GetNext`, `TokenSet`, `TokenFrequency` | [NameGenerator.cs:265-346](../Generator/NameGenerator.cs#L265) | Yes, as the rewritten version below |
| `Genitive` ("Owen's", "Davies'") | [NameGenerator.cs:233](../Generator/NameGenerator.cs#L233) | Optional; included below |
| `InitAsync`, `LoadTokenSet`, the nine static fields | [NameGenerator.cs:8-42](../Generator/NameGenerator.cs#L8) | No. Replaced below; see [Problems in the original](#problems-in-the-original) |
| `GenerateName`, `GenerateFirstName`, `GenerateSurName` | [NameGenerator.cs:168-263](../Generator/NameGenerator.cs#L168) | No. They depend on `Model.Language`, `Model.RaceIds` and `World.Races` |
| `GenerateRegionName`, `Choose`, `GenerateCityName` | [NameGenerator.cs:44-166](../Generator/NameGenerator.cs#L44) | No. See [Not covered](#not-covered) |

The only dependency of the extracted code is `System.Text.Json`, which is part of .NET. The `Generator` project's NuGet packages (AutoGen, OllamaSharp) and its reference to `Model` are not needed.

## Steps

1. Copy the nine JSON files into the new repo, for example into a `NameData` folder in the project that will use them. Keep the file names exactly as they are on disk, including casing.
2. Make the build copy them next to the binaries. Add this to the `.csproj`:

   ```xml
   <ItemGroup>
     <Content Include="NameData\*.json" CopyToOutputDirectory="PreserveNewest" />
   </ItemGroup>
   ```

3. Add a file `NameGeneration.cs` with [the code below](#the-code). Change the namespace to suit.
4. Load the generator once at startup and share the instance:

   ```csharp
   var names = await NameGenerator.LoadAsync(Path.Combine(AppContext.BaseDirectory, "NameData"));

   string given  = names.Generate(NameStyle.Common, NameKind.Female, seed: 3);   // "Emma"
   string family = names.Generate(NameStyle.Common, NameKind.Surname, seed: 1);  // "Davies"
   string full   = names.GenerateFullName(NameStyle.Exotic, NameKind.Male, seed: 42);
   ```

   With dependency injection: `builder.Services.AddSingleton(names);`.
5. Add a test with the [reference names](#reference-names) so that a bad copy, a changed data file or a runtime change is caught.

## The code

Requires .NET 8 or later (it uses collection expressions and ranges). This is the exact text that was verified.

```csharp
using System.Text;
using System.Text.Json;

namespace NameGeneration;

/// <summary>Which trained "language" to draw from. Maps to the file prefix.</summary>
public enum NameStyle
{
    /// <summary>British given names and surnames (files: GB_*).</summary>
    Common,
    /// <summary>Elven (files: elven_*).</summary>
    Exotic,
    /// <summary>Dark elven (files: darkelve_*).</summary>
    Dark,
}

public enum NameKind { Male, Female, Surname }

/// <summary>
/// Loads the nine pre-generated name models and generates names from them.
/// Immutable after <see cref="LoadAsync"/>; safe to share as a singleton.
/// </summary>
public sealed class NameGenerator
{
    private static readonly Dictionary<NameStyle, string> FilePrefix = new()
    {
        [NameStyle.Common] = "GB",
        [NameStyle.Exotic] = "elven",
        [NameStyle.Dark] = "darkelve",
    };

    private static readonly Dictionary<NameKind, string> FileKind = new()
    {
        [NameKind.Male] = "male",
        [NameKind.Female] = "female",
        [NameKind.Surname] = "sur",
    };

    private readonly Dictionary<(NameStyle, NameKind), MarkovNameModel> _models;

    private NameGenerator(Dictionary<(NameStyle, NameKind), MarkovNameModel> models) => _models = models;

    /// <param name="dataDirectory">Folder holding the nine *_names_sets.json files.</param>
    public static async Task<NameGenerator> LoadAsync(string dataDirectory, CancellationToken ct = default)
    {
        var models = new Dictionary<(NameStyle, NameKind), MarkovNameModel>();
        foreach (var (style, prefix) in FilePrefix)
            foreach (var (kind, kindName) in FileKind)
            {
                // File names are matched exactly; keep the on-disk casing (GB_, elven_, darkelve_).
                string path = Path.Combine(dataDirectory, $"{prefix}_{kindName}_names_sets.json");
                await using var stream = File.OpenRead(path);
                models[(style, kind)] = await MarkovNameModel.LoadAsync(stream, ct);
            }
        return new NameGenerator(models);
    }

    /// <summary>One name. The same seed always gives the same name.</summary>
    public string Generate(NameStyle style, NameKind kind, int seed) => _models[(style, kind)].Generate(seed);

    /// <summary>One name, drawing from a caller-owned random stream.</summary>
    public string Generate(NameStyle style, NameKind kind, Random random) => _models[(style, kind)].Generate(random);

    /// <summary>Given name and surname, e.g. "Usman Carry". Deterministic per seed.</summary>
    public string GenerateFullName(NameStyle style, NameKind givenNameKind, int seed)
    {
        var random = new Random(SeedMixer.Mix(seed));
        return Generate(style, givenNameKind, random) + " " + Generate(style, NameKind.Surname, random);
    }

    /// <summary>"Davies" -> "Davies'", "Owen" -> "Owen's".</summary>
    public static string Genitive(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return name;
        return name.EndsWith('s') ? name + "'" : name + "'s";
    }
}

/// <summary>
/// Scrambles a seed so that neighbouring seeds (1, 2, 3 ...) stop producing related names.
/// </summary>
public static class SeedMixer
{
    public static int Mix(int seed, int salt = 0)
    {
        unchecked
        {
            uint x = (uint)seed + 0x9E3779B9u * (uint)(salt + 1);
            x ^= x >> 16; x *= 0x85EBCA6Bu;
            x ^= x >> 13; x *= 0xC2B2AE35u;
            x ^= x >> 16;
            return (int)x;
        }
    }
}

/// <summary>
/// One trained model: a character-level Markov chain with up to three characters of context.
/// </summary>
public sealed class MarkovNameModel
{
    private const char Start = '\u00A3'; // the pound sign; escaped so file encoding cannot break it
    private const char End = '$';
    private const int Order = 3;
    private const int MaxLength = 30; // includes the start marker, so names are at most 29 characters

    private readonly Dictionary<string, TokenSet> _contexts;

    public MarkovNameModel(IDictionary<string, TokenSet> contexts)
    {
        if (contexts.Count == 0) throw new ArgumentException("Name model is empty.", nameof(contexts));
        _contexts = new Dictionary<string, TokenSet>(contexts, StringComparer.Ordinal);
    }

    public static async Task<MarkovNameModel> LoadAsync(Stream utf8Json, CancellationToken ct = default)
    {
        var contexts = await JsonSerializer.DeserializeAsync<Dictionary<string, TokenSet>>(utf8Json, cancellationToken: ct)
            ?? throw new InvalidDataException("Name model JSON was null.");
        return new MarkovNameModel(contexts);
    }

    public string Generate(int seed) => Generate(new Random(seed));

    public string Generate(Random random)
    {
        var name = new StringBuilder(MaxLength).Append(Start);
        while (name.Length < MaxLength && name[^1] != End)
        {
            int length = Math.Min(Order, name.Length);
            string context = name.ToString(name.Length - length, length);
            // The retry and the '#' fallback mirror the original so that seeds keep giving the same names.
            char? next = GetNext(context, random) ?? (context.Length < 2 ? '#' : GetNext(context, random));
            if (next is not null) name.Append(next.Value);
        }
        return Capitalize(name.ToString().Trim(Start).Trim(End));
    }

    private char? GetNext(string context, Random random)
    {
        while (true)
        {
            if (_contexts.TryGetValue(context, out var set))
            {
                float f = (float)random.NextDouble();
                float runningSum = 0;
                foreach (var candidate in set.NextTokenFreq)
                {
                    runningSum += candidate.F;
                    if (f < runningSum) return candidate.C;
                }
                return null;
            }
            if (context.Length < 2) return null;
            context = context[..^1];
        }
    }

    private static string Capitalize(string name)
    {
        name = name[..1].ToUpper() + name[1..];
        if (!name.Contains('-')) return name;
        var parts = name.Split('-');
        var result = new StringBuilder(parts[0]);
        for (int i = 1; i < parts.Length; i++)
            result.Append('-').Append(parts[i][..1].ToUpper()).Append(parts[i][1..]);
        return result.ToString();
    }
}

// Property names must match the JSON exactly (System.Text.Json is case-sensitive by default).
public sealed class TokenFrequency
{
    public char C { get; set; }
    public float F { get; set; }
}

public sealed class TokenSet
{
    public string PreTokens { get; set; } = "";
    public List<TokenFrequency> NextTokenFreq { get; set; } = [];
}
```

### Old API to new API

| IdleGame call | Extracted equivalent | Same name for same seed? |
|---|---|---|
| `NameGenerator.InitAsync()` | `await NameGenerator.LoadAsync(dir)` | n/a |
| `Generate(seed, set)` | `names.Generate(style, kind, seed)` | Yes |
| `GenerateFirstName(seed, Language.Common, NameSex.Male)` | `names.Generate(NameStyle.Common, NameKind.Male, seed)` | Yes |
| `GenerateSurName(seed, Language.Dark)` | `names.Generate(NameStyle.Dark, NameKind.Surname, seed)` | Yes |
| `GenerateName(seed, language, sex)` | `names.GenerateFullName(style, kind, seed)` | **No**, on purpose; see [Seeds](#seeds) |
| `Genitive(name)` | `NameGenerator.Genitive(name)` | Yes |

`NameStyle` replaces `Model.Language` with the same three members (`Common`, `Exotic`, `Dark`). `NameKind` replaces `NameSex` and adds `Surname`. The old `NameSex.None` made `GenerateFirstName` return an empty string; the new enum has no such member.

If you need the old `GenerateName` output exactly, call `names.Generate(style, kind, seed) + " " + names.Generate(style, NameKind.Surname, seed)`.

In IdleGame a race picks the language through `World.Races[raceId].Language` ([Model/World.cs](../Model/World.cs)). The new repo needs its own mapping from whatever it has (race, culture, faction) to `NameStyle`.

## How it works

### Data format

Each file is one JSON object. Every property is a *context*: the last one to three characters written so far. Its value lists which character can come next and with what probability.

```json
{
  "\u00A3":   { "PreTokens": "\u00A3",   "NextTokenFreq": [ { "C": "a", "F": 0.109770805 }, { "C": "b", "F": 0.042219542 } ] },
  "\u00A3a":  { "PreTokens": "\u00A3a",  "NextTokenFreq": [ { "C": "b", "F": 0.07777778 },  { "C": "d", "F": 0.08888889 } ] },
  "\u00A3ab": { "PreTokens": "\u00A3ab", "NextTokenFreq": [ { "C": "d", "F": 0.42857143 } ] }
}
```

- `\u00A3` is the pound sign `£`, used as the start-of-name marker. `$` as a next character means end of name.
- `PreTokens` always equals the property name. The code does not read it.
- `C` is a single character. `F` is its probability. The `F` values of one context add up to 1 (within 0.0000001).
- Everything is lower case. The files are pure ASCII; accented letters and `£` are written as `\uXXXX` escapes.
- Property names are case-sensitive when read with `System.Text.Json`, so `C`, `F`, `PreTokens` and `NextTokenFreq` must keep their casing in the C# classes.

### Algorithm

1. Start with the text `£`.
2. Take the last three characters (fewer if the text is shorter) as the context.
3. Look the context up. Draw a random number between 0 and 1, walk the `NextTokenFreq` list adding up `F`, and pick the first character whose running total exceeds the number.
4. If the context is not in the file, drop its **last** character and try again with the shorter one.
5. Append the character. Stop when it is `$` or the text reaches 30 characters.
6. Strip `£` and `$`, upper-case the first letter and the letter after each hyphen.

The random numbers come from `new Random(seed)`, so a seed always gives the same name from the same file.

## Problems in the original

These are defects in [NameGenerator.cs](../Generator/NameGenerator.cs) that the code above already avoids. They matter if you copy the original file instead.

1. **Hard-coded data path.** [Line 39](../Generator/NameGenerator.cs#L39) reads from `D:\VisualStudioProjects\IdleGame\Storage\PreGenData\`. That folder does not exist on this machine, so the original cannot load its data here as it stands.
2. **File name casing.** [Lines 27-29](../Generator/NameGenerator.cs#L27) ask for `Elven_male_names_sets.json`, but the files on disk are `elven_...`. Windows ignores the difference. Linux, macOS and containers do not, and the load fails there.
3. **Use before load.** `GenerateFirstName` and `GenerateName` do not wait for the data. Called before `InitAsync` has finished, they return the literal text `No set - no name!`. `InitAsync` returns `false` without waiting when another caller is already loading. Only `GenerateSurName` waits, and it does so by spinning in a loop for up to a minute.
4. **Culture-sensitive lookup.** The models are held in `SortedDictionary<string, TokenSet>` with the default comparer, which compares strings by the current culture's rules. The lookups should be exact. The extracted code uses `Dictionary` with `StringComparer.Ordinal`; both gave identical names in the comparison run (culture `en-DK`).
5. **Literal `£` in source.** [Line 272](../Generator/NameGenerator.cs#L272) contains a raw `£`. The file is UTF-8 without a byte-order mark, so a tool that guesses the wrong encoding turns the marker into something else, and every lookup then misses. The extracted code writes it as `'\u00A3'`.

## Behaviour to design around

These are properties of the algorithm and the data. They are unchanged in the extracted code.

### Seeds

**Neighbouring seeds give related names.** .NET's seeded `Random` produces a first value that rises steadily with the seed, and the first value picks the first letter. Seeds 0 to 9 on `GB_male` give Owen, Darry, Reece, Erris, Ryan, Felix, Shawn, Gregan, Sal, Ioan: the even seeds run O, R, R, S, S and the odd seeds run D, E, F, G, I. Across 200,000 seeds, seed `n` and seed `n + 2` gave first letters at most one apart 74% of the time.

**The same seed on two files gives alliterative pairs.** IdleGame passes one seed to both the given-name and surname model ([NameGenerator.cs:174-175](../Generator/NameGenerator.cs#L174), [PersonGenerator.cs:165-166](../Generator/PersonGenerator.cs#L165)), which yields "Owen Osullisburnetts", "Felix Fenwickson", "Sal Sampton". Given name and surname started with the same letter in 58% of 200,000 cases. With independent randomness the figure is 5%.

What to do:

- For a full name, use `GenerateFullName`. It scrambles the seed and draws both parts from one random stream (5% alliteration, measured).
- When your seeds are consecutive (`seed + i` for family members, row ids, tile indexes), pass them through `SeedMixer.Mix` first: `names.Generate(style, kind, SeedMixer.Mix(seed))`. Use the `salt` argument to get several unrelated names from one seed.
- Raw seeds are only needed if you want the same names IdleGame produces.

**Negative seeds repeat positive ones.** Seed `-1` gives the same name as `1`, and `int.MaxValue` the same as `0`, on all nine files.

### Output

- **Length.** Names are 3 to 29 characters. The chain has no notion of overall length, so it sometimes runs on: "Griffinnormicholm", "Twinbirthtwingerbrook", "Venwarmfeatdreathezire". Between 0.4% and 9% of names exceed 12 characters depending on the file (see the [table](#the-data-files)). Fewer than 0.1% reach the 29-character cap and are cut off there. If you want a limit, retry:

  ```csharp
  string name;
  int attempt = 0;
  do name = names.Generate(style, kind, SeedMixer.Mix(seed, attempt++));
  while (name.Length > 12 && attempt < 10);
  ```

- **Not unique.** Different seeds often give the same name. 300,000 seeds produced between 11,535 and 69,034 distinct names per file. If names must be unique, check for collisions yourself.
- **Characters.** Some files produce accented letters, hyphens, apostrophes or spaces ("Rentári", "Da silva"). Store and display names as Unicode. Only the first letter and the letter after a hyphen are capitalised, so the part after a space or apostrophe stays lower case.
- **No content filter.** The `GB_*` models regularly return real names (Owen, Emma, Ruth, Davies), which is the intent. Nothing screens for unfortunate words.

### Determinism

- The same seed gave the same name on .NET 8.0.34 and .NET 10.0.15. Microsoft's documentation does not promise that `Random`'s sequence stays the same across major versions, so keep the reference-name test.
- A seed's name also depends on the order of the entries in each `NextTokenFreq` list and on the sums being done in 32-bit floats. Reformatting the JSON is harmless; re-sorting those lists or regenerating the files changes the names.
- A port to another language works from the same files and the six steps above, but it will not give the same name per seed unless it also reproduces .NET's seeded `Random` and the 32-bit float arithmetic.

### Oddities kept for compatibility

The extracted code keeps three quirks of the original so that seeds keep their names. All three are rare:

- When a context is missing, the original drops the **newest** character (`a[..^1]`), where a Markov back-off would normally drop the oldest. This path is taken in three files (`elven_male`, `elven_sur`, `darkelve_sur`), a few hundred to a few thousand times per 300,000 names.
- When no character is picked because of float rounding, the original retries once with the same context. [Lines 286-287](../Generator/NameGenerator.cs#L286) compute a shortened context `aS` and then do not use it. This happened once in 2.7 million names.
- If the retry case occurs on the very first character, a `#` is written into the name. This did not happen in 2.7 million names, but it is possible.

## The data files

Measured over 200,000 to 300,000 seeds per file.

| File | Style, kind | Size | Contexts | Average length | Longer than 12 | Distinct names in 300,000 seeds | Characters beyond a-z |
|---|---|---|---|---|---|---|---|
| `GB_male_names_sets.json` | Common, Male | 171 kB | 1,776 | 5.3 | 0.4% | 11,535 | none |
| `GB_female_names_sets.json` | Common, Female | 152 kB | 1,554 | 5.6 | 0.9% | 18,766 | `-` é ë |
| `GB_sur_names_sets.json` | Common, Surname | 221 kB | 2,199 | 7.1 | 5.0% | 54,967 | space |
| `elven_male_names_sets.json` | Exotic, Male | 182 kB | 1,899 | 7.2 | 8.0% | 69,034 | `'` `-` á ä é |
| `elven_female_names_sets.json` | Exotic, Female | 140 kB | 1,488 | 6.7 | 4.5% | 34,701 | `-` á ä é ë í ú |
| `elven_sur_names_sets.json` | Exotic, Surname | 156 kB | 1,670 | 7.7 | 9.1% | 32,763 | `'` é |
| `darkelve_male_names_sets.json` | Dark, Male | 124 kB | 1,357 | 6.6 | 2.3% | 11,908 | none |
| `darkelve_female_names_sets.json` | Dark, Female | 121 kB | 1,301 | 7.0 | 2.9% | 21,916 | none |
| `darkelve_sur_names_sets.json` | Dark, Surname | 176 kB | 1,881 | 7.5 | 5.1% | 19,531 | none |

Loading takes a few milliseconds per file and generating a name a few microseconds.

**Provenance.** The repo holds neither the program that built these files nor the name lists they were built from. Both arrived finished in the first commit. If the other repo is published or licensed differently, the origin of the source lists is something only you can confirm.

### Making a new model

To add a style you have to write the builder. The rules below are read off the existing files, not taken from code:

1. Lower-case each source name and wrap it as `£name$`.
2. For every position, record which character follows the preceding three characters (fewer at the start, where the context begins with `£`: `£`, `£a`, `£ab`).
3. Also record the same counts for the shorter contexts made of the last two characters and the last one character. These are what the generator falls back on. In the existing files, the set of next characters of every two-character context equals the combined set of all three-character contexts ending in it.
4. Turn the counts of each context into probabilities that add up to 1 and write them as `F`.
5. Save as `<prefix>_<male|female|sur>_names_sets.json` and add the prefix to `FilePrefix` in the code.

## Reference names

Seeds 0 to 9 with `names.Generate(style, kind, seed)`, identical to the original `Generate(seed, set)`:

| Style, kind | Seeds 0 to 9 |
|---|---|
| Common, Male | Owen, Darry, Reece, Erris, Ryan, Felix, Shawn, Gregan, Sal, Ioan |
| Common, Female | Niki, Danie, Pamelaine, Emma, Ruth, Farzenata, Shonda, Hola, Salisha, Jeanel |
| Common, Surname | Osullisburnetts, Davies, Pearching, Ell, Ruthers, Fenwickson, Snedy, Griffinnormicholm, Sampton, Hearents |
| Exotic, Male | Ruvyn, Ehlmas, Shaunavan, Fin, Syth, Galaithrail, Thuryn, Hor, Tamnaerith, Jhar |
| Exotic, Female | Polla, Daena, Rentári, Eowyn, Synn, Elanni, Sionia, Gwyn, Takaria, Halaedra |
| Exotic, Surname | Stone, Faren, Silverond, Felis, Tyrider, Glavien, Veverondan, Irian, Waesmer, Kelgarwyn |
| Dark, Male | Pysha, Eirsolin, Relyr, Ghuhre, Sylkalyn, Helran, Trisneirsin, Koraedis, Uplik, Keethasil |
| Dark, Female | Rhyonis, Eceriina, Shara, Eronna, Sylmadra, Fednea, Terysn, Hilanren, Vanhice, Ilim |
| Dark, Surname | Qugren, Canmiu, Slim, Drallasaloth, Twinbirthtwingerbrook, Falnon, Venwarmfeatdreathezire, Flamenei, Xaris, Giedenvon |

`names.GenerateFullName(style, kind, seed)` for seeds 0, 2 and 4 with `NameKind.Male`:

| Style | Seed 0 | Seed 2 | Seed 4 |
|---|---|---|---|
| Common | Lorentin Mckett | Usman Carry | Gor Haydennandle |
| Exotic | Mlar Trivvin | Voropherynspieir Felinaunglavien | Glore Zylkian |
| Dark | Maloce Horniu | Venvyn Blaknon | Ine Gararuraeh |

A minimal xUnit test:

```csharp
[Fact]
public async Task Generates_the_reference_names()
{
    var names = await NameGenerator.LoadAsync(Path.Combine(AppContext.BaseDirectory, "NameData"));

    Assert.Equal("Owen", names.Generate(NameStyle.Common, NameKind.Male, 0));
    Assert.Equal("Davies", names.Generate(NameStyle.Common, NameKind.Surname, 1));
    Assert.Equal("Rentári", names.Generate(NameStyle.Exotic, NameKind.Female, 2));
    Assert.Equal("Twinbirthtwingerbrook", names.Generate(NameStyle.Dark, NameKind.Surname, 4));
    Assert.Equal("Usman Carry", names.GenerateFullName(NameStyle.Common, NameKind.Male, 2));
}
```

The "Rentári" case also proves that the escaped characters in the data survive the copy.

## Not covered

- **`GenerateRegionName`** ([NameGenerator.cs:44-115](../Generator/NameGenerator.cs#L44)) does not use the data files. It combines hard-coded prefix, middle and suffix word lists ("New", "Khmer", "Kingdom") by weight. It depends on `Model.Region`, `Model.Race`, `RaceIds` and `World.Races`, so reusing it means replacing those with plain parameters (is the race human, population, ruling family name). It has defects of its own: [line 54](../Generator/NameGenerator.cs#L54) sets `small` where `big` was meant, so the "Grand"/"Greath" prefixes are never used; `Choose` ([line 128](../Generator/NameGenerator.cs#L128)) compares with `<=` where `<` is correct, which skews the weights slightly; and prefix, middle and suffix are all drawn with the same seed, so they are not independent.
- **`GenerateCityName`** ([NameGenerator.cs:134-166](../Generator/NameGenerator.cs#L134)) is an unfinished stub that always returns an empty string.
