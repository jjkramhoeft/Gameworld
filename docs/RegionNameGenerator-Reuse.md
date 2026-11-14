# Reusing GenerateRegionName in another repo

This guide covers lifting the region-name generator out of IdleGame: a self-contained version of the code, the inputs it needs, a serious limitation in the original that you should decide about before reusing it, and a description of IdleGame's races so the other repo can line its own race model up with what the generator expects.

It is the companion to [NameGenerator-Reuse.md](NameGenerator-Reuse.md), which covers person names. The two meet in one place: a region name can contain the surname of the ruling family, and that surname comes from the person-name generator.

`GenerateRegionName` ([Generator/NameGenerator.cs:44-115](../Generator/NameGenerator.cs#L44)) does not use the data files in `Storage/PreGenData`. It picks a prefix, a middle and a suffix from weighted word lists that are written into the method.

The code in this guide was compiled and run next to a verbatim copy of the original method. In `Original` mode it returned the same name in all 28 million comparisons on .NET 8.0.34 (five races, seven population values, four ruling-family values, seeds -1,000 to 199,999) and in a further 7 million on .NET 10.0.15.

## What a region name is made of

A name is **prefix + middle + suffix**. Any of the three can be empty.

| Part | Lists | Entries | Examples |
|---|---|---|---|
| Prefix | all races | 8 | North, East, Southen, Western |
| | human only | 5 | (empty), New, Neo-, United, Free |
| | non-human only | 2 | (empty), Old |
| | non-human with a suffix | 6 | (empty), Second, Third, Fourth, Fifth, Sixth |
| | human, large population, with a suffix | 4 | (empty), Gran, Grand, Greath |
| Middle | all races | 309 | (empty), Abbasid, Khmer, Sokoto, Zimshath |
| | human only | 54 | Roman, British, Ottoman, Zulu |
| | non-human only | 29 | Eternia, Florania, Ashanti, Xopfand |
| Suffix | all races | 5 | Kingdom, Dynasty, Empire, Region, Nation |
| | human only | 6 | (empty), Republic, Alliance, State, Union, Territory |
| | human, small population | 4 | Duchy, Province, Len, County |
| | non-human only | 7 | (empty), Ground, Sect, Land, land, Khaganate, Sultanate |

The rules, in the order the code applies them:

1. Build the three lists for the race (human or not) and population.
2. Pick the suffix, then the middle.
3. If there is a suffix, a non-human region also gets the ordinal prefixes (Second, Third ...) and a large human region the Gran/Grand/Greath prefixes. Then pick the prefix.
4. If prefix and suffix start with the same two letters ("United ... Union", "Second ... Sect"), drop the prefix.
5. If there is a suffix and a ruling family name was supplied, the family name replaces the middle half of the time.
6. Join the parts and collapse double spaces.

## What the generator needs from the caller

| Input | Meaning | Where IdleGame gets it |
|---|---|---|
| `seed` | Makes the result repeatable | `region.Box.GetSeed()`, derived from the region's map position |
| `mainRaceIsHuman` | Chooses the human or the non-human word lists. This is the **only** thing race decides | `region.MainRaceId`, see [Race](#race-what-the-other-repo-needs-to-align) |
| `populationPotential` | Below 10,000 a human region can get Duchy, Province, Len or County | `region.CombinedPopulationPotential` |
| `rulingFamilyName` | Optional surname that can become the middle part | `region.RulingFamilyName`, a surname from the person-name generator |

## Steps

1. Add a file `RegionNames.cs` with [the code below](#the-code). It has no dependencies and needs no data files. Save it as UTF-8: the word lists contain é, ö, å, â and a curly apostrophe.
2. Decide between `Original` and `Corrected` mode. See [Original or Corrected](#original-or-corrected); unless you need the exact names IdleGame produces, use `Corrected`.
3. Map your race type to the `mainRaceIsHuman` flag and, if you want ruling-family names, to a person-name style. See [Race](#race-what-the-other-repo-needs-to-align).
4. Call it:

   ```csharp
   string family = names.Generate(NameStyle.Common, NameKind.Surname, regionSeed);   // from NameGenerator-Reuse.md
   string regionName = RegionNameGenerator.Generate(
       regionSeed,
       mainRaceIsHuman: true,
       populationPotential: 50_000,
       rulingFamilyName: family,
       mode: RegionNameMode.Corrected);
   ```

5. Add a test with the [reference names](#reference-names).

## The code

Requires .NET 8 or later with implicit usings enabled (the default in new projects); otherwise add `using System.Linq;` and `using System.Collections.Generic;`. The word lists were generated from the original source by script, not retyped, and this is the exact text that was verified.

```csharp
namespace NameGeneration;

/// <summary>How closely to follow IdleGame's GenerateRegionName.</summary>
public enum RegionNameMode
{
    /// <summary>The same name as IdleGame for the same inputs, defects included.</summary>
    Original,

    /// <summary>The same word lists and rules with the defects fixed. Names differ from IdleGame's.</summary>
    Corrected,
}

/// <summary>
/// Builds region names such as "South Davies Kingdom" or "New Sokoto Republic" from weighted word lists.
/// A name is prefix + middle + suffix, where each part may be empty. Stateless and thread-safe.
/// </summary>
public static class RegionNameGenerator
{
    /// <summary>Below this population potential a human region can get a small-realm suffix (Duchy, Province, Len, County).</summary>
    public const int SmallBelow = 10_000;

    /// <summary>Above this population potential a human region can get a Gran/Grand/Greath prefix (Corrected mode only).</summary>
    public const int BigAbove = 100_000;

    /// <param name="seed">The same inputs and seed always give the same name.</param>
    /// <param name="mainRaceIsHuman">True for a human region and for a region with no main race.</param>
    /// <param name="populationPotential">The region's combined population potential.</param>
    /// <param name="rulingFamilyName">Surname of the ruling family, or null. Can replace the middle part.</param>
    public static string Generate(
        int seed,
        bool mainRaceIsHuman,
        int populationPotential,
        string? rulingFamilyName = null,
        RegionNameMode mode = RegionNameMode.Original)
    {
        bool original = mode == RegionNameMode.Original;

        // The original sets "small" for large regions too and never sets "big".
        bool small = populationPotential < SmallBelow || (original && BigAbove < populationPotential);
        bool big = !original && BigAbove < populationPotential;

        // The original restarts the random sequence for every draw, so one number decides every part.
        var shared = new Random(seed);
        Random NextRandom() => original ? new Random(seed) : shared;

        List<(string Text, int Weight)> pre = [.. Prefix, .. (mainRaceIsHuman ? PrefixHuman : PrefixNonhuman)];
        List<(string Text, int Weight)> mid = [.. Middle, .. (mainRaceIsHuman ? MiddleHuman : MiddleNonhuman)];
        List<(string Text, int Weight)> post = [.. Suffix, .. (mainRaceIsHuman ? SuffixHuman : SuffixNonhuman)];
        if (mainRaceIsHuman && small) post.AddRange(SuffixSmall);

        string chosenPost = Pick(post, NextRandom(), original);
        string chosenMid = Pick(mid, NextRandom(), original);
        bool hasPost = !string.IsNullOrWhiteSpace(chosenPost);

        if (mainRaceIsHuman)
        {
            if (big && hasPost) pre.AddRange(PrefixBig);
        }
        else if (hasPost)
        {
            pre.AddRange(PrefixNumber);
        }

        string chosenPre = Pick(pre, NextRandom(), original);

        // "United ... Union" and "Second ... Sect": drop the prefix when it starts with the same two letters as the suffix.
        if (!string.IsNullOrWhiteSpace(chosenPre) && hasPost && chosenPre.Trim()[..2].Equals(chosenPost.Trim()[..2]))
            chosenPre = "";

        if (rulingFamilyName is not null && hasPost && NextRandom().Next(6) <= 2)
            chosenMid = rulingFamilyName;

        if (original)
            return (chosenPre + chosenMid + chosenPost).Replace("  ", " ");

        // Corrected mode only: the middle may be empty only between a prefix and a suffix ("North Kingdom").
        if (chosenMid.Length == 0 && (!hasPost || string.IsNullOrWhiteSpace(chosenPre)))
            chosenMid = Pick(mid.Where(option => option.Text.Length > 0).ToList(), shared, original: false);
        return (chosenPre + chosenMid + chosenPost).Replace("  ", " ").Trim();
    }

    private static string Pick(IReadOnlyList<(string Text, int Weight)> options, Random random, bool original)
    {
        int sum = 0;
        foreach (var option in options) sum += option.Weight;
        int x = random.Next(sum);
        int runningSum = 0;
        foreach (var option in options)
        {
            runningSum += option.Weight;
            // The original compares with <=, which moves one unit of weight from the last entry to the first.
            if (original ? x <= runningSum : x < runningSum) return option.Text;
        }
        return "";
    }

    // Each entry is (text, weight). The order matters in Original mode. Save this file as UTF-8.

    private static readonly (string Text, int Weight)[] Prefix =
    [
        ("North ", 2), ("East ", 2), ("South ", 2), ("West ", 2), ("Northern ", 2), ("Eastern ", 2), ("Southen ", 2),
        ("Western ", 2),
    ];

    private static readonly (string Text, int Weight)[] PrefixHuman =
    [
        ("", 15), ("New ", 5), ("Neo-", 2), ("United ", 2), ("Free ", 2),
    ];

    private static readonly (string Text, int Weight)[] PrefixNonhuman =
    [
        ("", 15), ("Old ", 5),
    ];

    private static readonly (string Text, int Weight)[] PrefixBig =
    [
        ("", 15), ("Gran", 1), ("Grand ", 2), ("Greath ", 4),
    ];

    private static readonly (string Text, int Weight)[] PrefixNumber =
    [
        ("", 15), ("Second ", 1), ("Third ", 2), ("Fourth ", 3), ("Fifth ", 2), ("Sixth ", 1),
    ];

    private static readonly (string Text, int Weight)[] Middle =
    [
        ("", 10), ("Abbasid", 1), ("Aceh", 1), ("Achaemenid", 1), ("Afsharid", 1), ("Aizak", 1), ("Ajuran", 1),
        ("Akkadian", 1), ("Angevin", 1), ("Austro", 1), ("Avar", 1), ("Balhae", 1), ("Bamana", 1), ("Banjar", 1),
        ("Benin", 1), ("Bese", 1), ("Blanind", 1), ("Bogd", 1), ("Bornu", 1), ("Brazillian", 1), ("Bretoth", 1),
        ("Bruneian", 1), ("Bukhara", 1), ("Burgundian", 1), ("Buyid", 1), ("Calakmul", 1), ("Cao Wei", 1),
        ("Carolingian", 1), ("Carthaginian", 1), ("Central", 1), ("Cewer", 1), ("Chagatai", 1), ("Chalukya", 1),
        ("Chauhan", 1), ("Chenla", 1), ("Thera", 1), ("Chimor", 1), ("Chola", 1), ("Cihgecu", 1), ("Clapeth", 1),
        ("Clozqiet", 1), ("Comanche", 1), ("Congo", 1), ("Córdoba", 1), ("Crimean", 1), ("Cruunsilu", 1),
        ("Dacian", 1), ("Dagbon", 1), ("Dega", 1), ("Delhi", 1), ("Demak", 1), ("Dohoq", 1), ("Dumut", 1),
        ("Durrani", 1), ("Dzungar", 1), ("Egrium", 1), ("Ehni", 1), ("Eki", 1), ("Elamite", 1), ("Elba", 1),
        ("Gallic", 1), ("Ganat", 1), ("Ganga", 1), ("Gaza", 1), ("Genoese", 1), ("Ghana", 1), ("Ghurid", 1),
        ("Glepei", 1), ("Gnehrei", 1), ("Goguryeo", 1), ("Golden", 1), ("Gorkha", 1), ("Goryeo", 1), ("Gowa", 1),
        ("Grersah", 1), ("Gunbax", 1), ("Gupta", 1), ("Gurjara-Pratihara", 1), ("Göktürk", 1), ("Hafsid", 1),
        ("Han", 1), ("Harni", 1), ("Harsha", 1), ("Hephthalite", 1), ("Hittite", 1), ("Hocuiq", 1), ("Hofesh", 1),
        ("Hontea", 1), ("Hospitaller", 1), ("Hotak", 1), ("Hugax", 1), ("Huilstin", 1), ("Hunnic", 1),
        ("Husainid", 1), ("Iberian", 1), ("Icag", 1), ("Idisid", 1), ("Ifkush", 1), ("Ihreo", 1), ("Ikomau", 1),
        ("Ikush", 1), ("Ilkhanate", 1), ("Iqaq", 1), ("Irfi", 1), ("Isfa", 1), ("Iursar", 1), ("Jaftaag", 1),
        ("Jihu", 1), ("Jin", 1), ("Johor", 1), ("Jolof", 1), ("Joseon", 1), ("Judah", 1), ("Juncolu", 1),
        ("Kachari", 1), ("Kadamba", 1), ("Kadu", 1), ("Kagho", 1), ("Kalmar", 1), ("Frulshom", 1), ("Fulo", 1),
        ("Funan", 1), ("Kamtel", 1), ("Kanem", 1), ("Kanva", 1), ("Kara-Khanid", 1), ("Kazakh", 1), ("Kelrus", 1),
        ("Khazar", 1), ("Khilji", 1), ("Khitai", 1), ("Khmer", 1), ("Kievan Rus'", 1), ("Konbaung", 1), ("Kong", 1),
        ("Kushan", 1), ("Kyrgyz", 1), ("Kaabu", 1), ("Later", 1), ("Lavo", 1), ("Lenig", 1), ("Liao", 1),
        ("Lidquax", 1), ("Lingus", 1), ("Lithuania", 1), ("Lodi", 1), ("Lodrya", 1), ("Mabe", 1),
        ("Madurai Nayak", 1), ("Magadha", 1), ("Majapahit", 1), ("Malacca", 1), ("Mali", 1), ("Mamluk", 1),
        ("Maratha", 1), ("Marhasi", 1), ("Marinid", 1), ("Massina", 1), ("Mataram", 1), ("Mauretania", 1),
        ("Maurya", 1), ("Mauryan", 1), ("Median", 1), ("Meqiak", 1), ("Mexican", 1), ("Ming", 1), ("Mitanni", 1),
        ("Moavian", 1), ("Mughal", 1), ("Nanda", 1), ("Nawhesh", 1), ("Nocqili", 1), ("Nuake", 1), ("Numidia", 1),
        ("Odrysian", 1), ("Omani", 1), ("Ostrogothic", 1), ("Oyo", 1), ("Pagan", 1), ("Pahlavi", 1), ("Pala", 1),
        ("Pallava", 1), ("Palmyrene", 1), ("Pontus", 1), ("Potix", 1), ("Prussian", 1), ("Ptolemaic", 1),
        ("Qajar", 1), ("Qasend", 1), ("Qiluum", 1), ("Qimuc", 1), ("Qin", 1), ("Qing", 1), ("Qzoth", 1),
        ("Rapeq", 1), ("Rashidun", 1), ("Rashtrakuta", 1), ("Repa", 1), ("Rispinde", 1), ("Rodge", 1), ("Rouran", 1),
        ("Rozvi", 1), ("Rum", 1), ("Rustamid", 1), ("Safavid", 1), ("Saffarid", 1), ("Saga", 1), ("Samanid", 1),
        ("Samo's", 1), ("Sassanian", 1), ("Satavahana", 1), ("Scepeoc", 1), ("Scte", 1), ("Scuamthal", 1),
        ("Selecuid", 1), ("Seleucid", 1), ("Seljuk", 1), ("Seljuk", 1), ("Seorrul", 1), ("Shah", 1), ("Shang", 1),
        ("Shihmuh", 1), ("Shilo", 1), ("Shu Han", 1), ("Shunga", 1), ("Siafur", 1), ("Siam", 1), ("Sikh", 1),
        ("Singhasari", 1), ("Skåne", 1), ("Smefti", 1), ("Snubme", 1), ("Venetian", 1), ("Visigothic", 1),
        ("Wari", 1), ("Wassoulou", 1), ("Wattasid", 1), ("Weermosh", 1), ("Fatimid", 1), ("Feomaq", 1),
        ("Flewbio", 1), ("Pandyan", 1), ("Parthian", 1), ("Pifo", 1), ("Sokoto", 1), ("Song", 1), ("Songhai", 1),
        ("Songhay", 1), ("Sphermo", 1), ("Srivijaya", 1), ("Stopmuk", 1), ("Strappam", 1), ("Sui", 1),
        ("Sukhothai", 1), ("Sultanate", 1), ("Sumerian", 1), ("Sur", 1), ("Sutat", 1), ("Swofdon", 1), ("Saadi", 1),
        ("Tahirid", 1), ("Tang", 1), ("Tawo", 1), ("Tây Son", 1), ("Tebo", 1), ("Thonburi", 1), ("Thresmousho", 1),
        ("Tibet", 1), ("Tibetan", 1), ("Tikal", 1), ("Timurid", 1), ("Tlemcen", 1), ("Tokugawa", 1), ("Toltec", 1),
        ("Toucouleur", 1), ("Toungoo", 1), ("Trebizond", 1), ("Triey Skijan", 1), ("Tsardom", 1), ("Tu'i Tonga", 1),
        ("Tui Manu’a", 1), ("Turgesh", 1), ("Turkic", 1), ("Twaaptik", 1), ("Uhhu", 1), ("Uhoureo", 1),
        ("Umayyad", 1), ("Unu", 1), ("Uyghur", 1), ("Uzbek", 1), ("Vandal", 1), ("Varman", 1), ("Wezwir", 1),
        ("Wu Zhou", 1), ("Xia", 1), ("Xianbei", 1), ("Xin", 1), ("Xiognu", 1), ("Xiongnu", 1), ("Yaswun", 1),
        ("Yuan", 1), ("Zand", 1), ("Zhou", 1), ("Zikil", 1), ("Zimshath", 1), ("Zirid", 1), ("Zogni", 1),
        ("Zoize", 1), ("Aarloe", 1),
    ];

    private static readonly (string Text, int Weight)[] MiddleHuman =
    [
        ("Assyria", 1), ("Assyrian", 1), ("Athens", 1), ("Austrian", 1), ("Aztec", 1), ("Babylonia", 1),
        ("Babylonian", 1), ("Belgian", 1), ("Bengal", 1), ("Bazil", 1), ("Britannic", 1), ("British", 1),
        ("Bulgarian", 1), ("Byzantine", 1), ("Danish", 1), ("Dutch", 1), ("Egyptian", 1), ("England", 1),
        ("Ethioian", 1), ("France", 1), ("Frankish", 1), ("French", 1), ("Georgia", 1), ("German", 1), ("Haiti", 1),
        ("Inca", 1), ("Indian", 1), ("Israel", 1), ("Italin", 1), ("Korean", 1), ("Latin", 1), ("Macedonian", 1),
        ("Mongol", 1), ("Napoleonic", 1), ("Norway", 1), ("Norwegian", 1), ("Ottoman", 1), ("Portugese", 1),
        ("Portuguese", 1), ("Roman", 1), ("Russian", 1), ("Scottish", 1), ("Serbian", 1), ("Spanish", 1),
        ("Spartan", 1), ("Sweden", 1), ("Swedish", 1), ("Armenian", 1), ("Baltikum", 1), ("Japanese", 1),
        ("Thessalonica", 1), ("Vietnam", 1), ("Zimbabwe", 1), ("Zulu", 1),
    ];

    private static readonly (string Text, int Weight)[] MiddleNonhuman =
    [
        ("Eternia", 1), ("Giania", 1), ("Florania", 1), ("Aghlabids", 1), ("Aiwuk", 1), ("Aksumite", 1),
        ("Akwamu", 1), ("Alaouite", 1), ("Almohad", 1), ("Almoravid", 1), ("Aq Qoyunlu", 1), ("Aragon", 1),
        ("Ashanti", 1), ("Aulikara", 1), ("Ayutthaya", 1), ("Ayyubid", 1), ("Ghaznavid", 1), ("Hoysala", 1),
        ("Khwarazmian", 1), ("Knaupzond", 1), ("Mlechchha", 1), ("Ograq", 1), ("Purépecha", 1), ("Qoyunlu", 1),
        ("Schihhish", 1), ("Sphephrasha", 1), ("Swomzuq", 1), ("Vijayanagara", 1), ("Xopfand", 1),
    ];

    private static readonly (string Text, int Weight)[] Suffix =
    [
        (" Kingdom", 6), (" Dynasty", 2), (" Empire", 3), (" Region", 2), (" Nation", 3),
    ];

    private static readonly (string Text, int Weight)[] SuffixHuman =
    [
        ("", 15), (" Republic", 5), (" Alliance", 4), (" State", 3), (" Union", 2), (" Territory", 2),
    ];

    private static readonly (string Text, int Weight)[] SuffixNonhuman =
    [
        ("", 15), (" Ground", 2), (" Sect", 1), (" Land", 3), ("land", 1), (" Khaganate", 1), (" Sultanate", 1),
    ];

    private static readonly (string Text, int Weight)[] SuffixSmall =
    [
        (" Duchy", 1), (" Province", 1), (" Len", 3), (" County", 3),
    ];
}
```

### Old call to new call

```csharp
// IdleGame
region.Name = NameGenerator.GenerateRegionName(regionSeed, region);

// Extracted, same result
region.Name = RegionNameGenerator.Generate(
    regionSeed,
    mainRaceIsHuman: region.MainRaceId is RaceIds.None or RaceIds.Human,
    populationPotential: region.CombinedPopulationPotential,
    rulingFamilyName: region.RulingFamilyName);
```

## Original or Corrected

### The main defect: one random number decides everything

`Choose` creates `new Random(seed)` on every call ([line 122](../Generator/NameGenerator.cs#L122)), and so does the ruling-family check ([line 109](../Generator/NameGenerator.cs#L109)). All four draws therefore use the same first random number. Prefix, middle, suffix and the family decision are not independent choices; they are four readings of one number.

Measured over 200,000 seeds per row:

| Inputs | Distinct names, `Original` | Distinct names, `Corrected` |
|---|---|---|
| Human, population 5,000, no family name | 386 | 57,635 |
| Human, population 5,000, family name | 283 | 42,228 |
| Human, population 50,000, no family name | 382 | 46,845 |
| Human, population 50,000, family name | 259 | 36,170 |
| Human, population 500,000, no family name | 386 | 51,096 |
| Human, population 500,000, family name | 283 | 36,272 |
| Non-human, no family name | 358 | 43,218 |
| Non-human, family name | 221 | 30,743 |

In `Original` mode the parts are also locked to each other. For a human region with population 50,000 and family name "Davies":

- Every name ending in Kingdom, Dynasty, Empire, Region or Nation uses the family name (100% of them). Every name ending in Republic, Alliance, State, Union or Territory uses a word from the list (the family name appears in 0% of them).
- "Kingdom" only ever follows North, East or South. "Alliance" only follows New. "Territory" only follows Free.
- "North Davies Kingdom" alone is 7.1% of all seeds. In a world with many human regions, about one in fourteen is "North &lt;family&gt; Kingdom".

### Other defects in the original

| Defect | Where | Effect |
|---|---|---|
| Large regions are flagged `small`; `big` is never set | [Lines 53-54](../Generator/NameGenerator.cs#L53) | Gran, Grand and Greath are never used. A human region above 100,000 gets Duchy, Province, Len and County, exactly like one below 10,000 |
| `Choose` compares with `<=` where `<` is correct | [Line 128](../Generator/NameGenerator.cs#L128) | The first entry of each list gains one unit of weight and the last loses one. Last entries with weight 1 can never be picked |
| Combined effect of the above | | These words never appeared in any `Original` run: Old, Sixth, Gran, Grand, Greath, Zulu, Xopfand |
| `RaceIds.All` as main race | [Line 48](../Generator/NameGenerator.cs#L48) | `KeyNotFoundException`. The extracted code takes a flag and cannot hit this |
| Comment says "3 letters", code compares 2 | [Lines 104-105](../Generator/NameGenerator.cs#L104) | None. Two letters is what catches "United Union" and "Second Sect" |

### What Corrected mode changes

`Corrected` keeps the word lists, weights and rules and changes five things:

1. The four draws come from one random sequence, so the parts are independent.
2. A population above 100,000 sets `big`, not `small`, so the Gran/Grand/Greath prefixes work (7.4% of names for a large human region).
3. `Choose` compares with `<`, so every entry has exactly its listed weight.
4. An empty middle is only allowed between a prefix and a suffix ("North Kingdom", about 1% of names). Otherwise the middle is drawn again without the empty entry. Independent draws would otherwise produce bare names such as "Kingdom" or "New", and occasionally an empty string.
5. The result is trimmed.

Every word in the lists appeared in the `Corrected` runs. No run in either mode produced an empty name, a leading or trailing space, or a double space.

Use `Original` only if the new repo must show the same region names as IdleGame for the same seeds.

## Behaviour in both modes

- **Race has two values.** Elf, orc, dwarf and every other non-human race share one set of lists. Nothing in a region name reflects which non-human race rules it, apart from the ruling family's surname.
- **Population.** Only human regions react to it. Non-human names are identical for any population.
- **The word lists are a mixed bag.** The middle list holds real dynasties, peoples and countries (Roman, British, German, Ottoman, Israel, Zulu, Mongol), invented words (Zimshath, Xopfand, Thresmousho) and fragments that are not names on their own (Later, Central, Golden, Shah, Sultanate, Tsardom). Results such as "United German Republic", "Free Israel State" or "Western Later" are possible. Review the lists if real-world names are unwelcome.
- **Spelling.** The lists contain misspellings that end up in names: Southen, Greath, Bazil, Brazillian, Ethioian, Italin, Portugese (next to Portuguese), Selecuid (next to Seleucid), Xiognu (next to Xiongnu). "Seljuk" is listed twice, so it is twice as likely. "Gran" has no trailing space, so it joins the next word: "GranDelhi Kingdom". Fixing a word is safe in `Corrected` mode; in `Original` mode changing a word changes only the names that contain it, but adding, removing or reordering entries shifts other names.
- **Length.** The longest name seen was 36 characters.
- **Not unique.** Two regions can get the same name. Check for collisions if that matters.
- **Seeds.** A negative seed gives the same name as its positive counterpart. Names were identical on .NET 8.0.34 and .NET 10.0.15; Microsoft's documentation does not promise that the seeded `Random` sequence stays the same across major versions, so keep the reference test.
- **Seed reuse in IdleGame.** `Survey` passes the same `regionSeed` to the surname generator and to the region-name generator ([MapGenerator.cs:671-676](../Generator/MapGenerator.cs#L671)). To reproduce IdleGame's names you must do the same. Otherwise give the surname its own seed, for example `SeedMixer.Mix(regionSeed, 1)` from the person-name guide.

## Race: what the other repo needs to align

### What region naming uses from race

Very little:

| Need | Rule in IdleGame |
|---|---|
| Human or not | `MainRaceId == RaceIds.Human` selects the human lists. `RaceIds.None` (no main race) is treated as human ([lines 46-48](../Generator/NameGenerator.cs#L46)). Every other race is non-human |
| Ruling family surname | `GenerateSurName(regionSeed, MainRaceId)` looks up the race's `Language` and uses the matching surname model ([MapGenerator.cs:671](../Generator/MapGenerator.cs#L671)) |

So the minimum the other repo's race model must offer is: a way to tell whether a race is the human one, and a name style per race. A sketch:

```csharp
bool mainRaceIsHuman = race is null || race.IsHuman;
NameStyle style = race?.NameStyle ?? NameStyle.Common;
```

Everything below describes how IdleGame models races beyond that minimum, for the case where the other repo should match it more closely.

### The race type

[Model/Race.cs](../Model/Race.cs) defines a `Race` class with these properties:

| Property | Type | Meaning |
|---|---|---|
| `Id` | `RaceIds` | Identity; set in the constructor |
| `Name` | `string` | Display name |
| `AltName` | `string` | Alternative display name; empty for most races |
| `Language` | `Language` | Which person-name models the race uses: `Common`, `Exotic` or `Dark` |
| `PrimaryBioms` | `List<BiomIds>` | Biomes where the race thrives |
| `SecondaryBioms` | `List<BiomIds>` | Biomes where the race can live at reduced numbers |
| `Filter` | `LocationFilter?` | Height and temperature band the race is limited to; `null` means anywhere |
| `PopulationModifier` | `float` | Multiplier on a location's population potential; human is 1 |

The instances live in the static dictionary `World.Races`, filled by `InitRaces` ([Model/World.cs:1355](../Model/World.cs#L1355)) when `World.InitWorld()` is called. `Race.AllIds` lists the 16 real races in a fixed order that matters for tie-breaking: Human, Elve, Dwarf, Orc, Goblin, Haflings, Lizard, Hare, Centaur, Ent, Mer, Minotaur, Satyr, Fae, Nymph, Thiefling.

The identifiers, with their numeric values. `None` and `All` are not races; `All` is only used by the debug map.

```csharp
public enum RaceIds
{
    None = 0,
    Centaur = 1,
    Dwarf = 2,
    Elve = 3,
    Ent = 4,
    Fae = 5,
    Goblin = 6,
    Haflings = 7,
    Hare = 8,
    Human = 9,
    Lizard = 10,
    Mer = 11,
    Minotaur = 12,
    Nymph = 13,
    Orc = 14,
    Satyr = 15,
    Thiefling = 16,
    All = 99,
}
```

The numeric values matter wherever a race is stored or sent as a number (the debug-map endpoint in `WorldApi` takes them as integers). Several identifiers are spelled differently from the display name, so match on the enum member or number, not on text.

### The 16 races

`Language` decides the person-name files: `Common` uses `GB_*`, `Exotic` uses `elven_*`, `Dark` uses `darkelve_*`. In the extracted person-name code these are `NameStyle.Common`, `NameStyle.Exotic` and `NameStyle.Dark`.

| `RaceIds` | Value | Name | AltName | Language | Population modifier | Height band | Temperature band |
|---|---|---|---|---|---|---|---|
| `Human` | 9 | Human |  | Common | 1 | any | any |
| `Elve` | 3 | Elve |  | Exotic | 0.55 | MainlandPlusAllHills | NormalToHot |
| `Dwarf` | 2 | Dwarf |  | Common | 0.55 | HillsAndUp | All |
| `Orc` | 14 | Orc |  | Dark | 0.75 | MainlandPlusAllHills | NormalToHot |
| `Goblin` | 6 | Goblin |  | Common | 0.8 | CoastToLowHills | NormalToHot |
| `Haflings` | 7 | Haflings | Hobbit | Common | 0.27 | MainlandPlusAllHills | NormalToHot |
| `Lizard` | 10 | Lizard Folk | Lizardmen | Dark | 0.5 | CoastToLowHills | WarmAndUp |
| `Hare` | 8 | Harengon |  | Common | 0.5 | MainlandPlusAllHills | NormalToHot |
| `Centaur` | 1 | Centaur |  | Common | 0.4 | MainlandPlusAllHills | NormalToHot |
| `Ent` | 4 | Treant | Ent | Common | 0.13 | MainlandPlusAllHills | NotVeryCold |
| `Mer` | 11 | Mer Folk | Mermen | Exotic | 13 | ShallowWater | NotVeryCold |
| `Minotaur` | 12 | Minotaur |  | Common | 0.13 | MainlandPlusAllHills | NotVeryCold |
| `Satyr` | 15 | Satyr |  | Common | 0.26 | MainlandPlusAllHills | NotVeryCold |
| `Fae` | 5 | Fae | Faery | Exotic | 0.34 | ShallowWater | NormalToHot |
| `Nymph` | 13 | Nymph |  | Common | 0.18 | ShallowWater | NormalToHot |
| `Thiefling` | 16 | Tiefling |  | Common | 0.13 | ShallowWater | NotVeryCold |

Height bands: `ShallowWater` is below sea level down to the deep-water line; `CoastToLowHills` is sea level up to the hills; `MainlandPlusAllHills` is above the coast up to the mountains; `HillsAndUp` is hills and mountains.

Biomes per race. Names are `BiomIds` members ([Model/Biom.cs](../Model/Biom.cs)).

| `RaceIds` | Primary biomes | Secondary biomes |
|---|---|---|
| `Human` | BorealForests, TemperateForests, TemperateConiferousForests, TemperateRainForests, GrassSteppe, Plains, Fields, RiverDelta, SandBeach, ReedsBeach | Tundra, MountainTundra, TropicalRainForests, TropicalMoistForests, TropicalDryForests, Mangrove, Savannah, Marsh |
| `Elve` | TemperateForests, TemperateConiferousForests, TemperateRainForests, TropicalRainForests, TropicalMoistForests, TropicalDryForests, TropicalCloudForests, AncientForest | BorealForests, Mangrove, MushroomForest |
| `Dwarf` | BareRock, Tundra, MountainTundra, BorealForests, TemperateConiferousForests, Vulcano | AncientForest*, Permafrost, TemperateForests, TemperateRainForests, TropicalRainForests, TropicalMoistForests, TropicalDryForests, TropicalCloudForests, LavaPlain, Cliffs |
| `Orc` | Savannah, GrassSteppe, Plains | Tundra, BorealForests, TemperateForests, TemperateConiferousForests |
| `Goblin` | GrassSteppe, Plains, RiverDelta, SandBeach, ReedsBeach | TemperateForests, Mangrove, Savannah, Permafrost*, Fields, Swamp, Bog, Marsh |
| `Haflings` | TemperateForests, TemperateConiferousForests, GrassSteppe, Plains, Fields | BorealForests, TemperateRainForests, MushroomForest |
| `Lizard` | TropicalRainForests, TropicalMoistForests, Mangrove, Swamp, Bog, Marsh, RiverDelta, ReedsBeach | Desert, TemperateConiferousForests, TemperateRainForests, TropicalMoistForests, Savannah, GrassSteppe, Plains, SandBeach |
| `Hare` | BorealForests, TemperateForests, TemperateConiferousForests, GrassSteppe, Plains | Tundra, MountainTundra, TemperateRainForests, Savannah |
| `Centaur` | BorealForests, TemperateForests, TemperateConiferousForests, TemperateRainForests, TropicalMoistForests, TropicalDryForests, TropicalCloudForests | TropicalRainForests |
| `Ent` | BorealForests, TemperateForests, TemperateConiferousForests, TemperateRainForests, TropicalRainForests, TropicalMoistForests, TropicalDryForests, Mangrove, AncientForest | Swamp |
| `Mer` | CoralReef, SeaweedForest | Bank, Sea, Ocean |
| `Minotaur` | TemperateForests, TemperateConiferousForests | BorealForests, TemperateRainForests, LavaPlain |
| `Satyr` | TemperateForests, TemperateConiferousForests | BorealForests, TemperateRainForests |
| `Fae` | TemperateForests, TemperateConiferousForests, TropicalCloudForests, TropicalMoistForests | BorealForests, TemperateRainForests, TropicalRainForests, TropicalDryForests |
| `Nymph` | TemperateForests, TemperateConiferousForests, TropicalMoistForests | BorealForests, TemperateRainForests, TropicalRainForests, TropicalDryForests |
| `Thiefling` | TemperateForests, TemperateConiferousForests, TropicalMoistForests, Mangrove, Plains, LavaPlain | BorealForests, TemperateRainForests, TropicalRainForests, TropicalDryForests, Savannah, GrassSteppe |

### How a region gets its main race

This happens in `Survey` ([MapGenerator.cs:567](../Generator/MapGenerator.cs#L567)), before the region is named.

1. Every location in the region gets a general population potential. Its sum over the region is `CombinedPopulationPotential`, the number the name generator compares with 10,000 and 100,000. A region covers roughly 10,000 locations (a 100,000 by 100,000 box with one location per 1,000 by 1,000). I did not run a survey, so I cannot say how often real regions fall below 10,000 or above 100,000.
2. For each race whose `Filter` matches the location, `CalcRacialPopulationPotential` ([MapGenerator.cs:725](../Generator/MapGenerator.cs#L725)) computes a racial potential:
   - location potential × `PopulationModifier` × 1.1 in a primary biome, × 0.5 in a secondary biome, otherwise 0;
   - then adjusted by the location's geology:

     | Geology | Adjustment |
     |---|---|
     | Forest | Human ÷ 2, Elve × 3, Ent × 2 |
     | Minerals | Dwarf × 3 |
     | Dark | Orc, Goblin, Lizard, Minotaur × 2; Human, Elve, Ent, Fae, Hare, Haflings, Centaur ÷ 2 |
     | Magic | Fae × 3, Human ÷ 3, every other race × 2 |

3. The racial potentials are summed per race over the region (`RacialPopulationPotential`).
4. `PickMainRace` ([MapGenerator.cs:864](../Generator/MapGenerator.cs#L864)) chooses:
   - If more than half of the region's locations have Dark geology, the choice is between Lizard, Goblin, Orc and Human:
     - Orc below Lizard: Lizard if Human is below twice Lizard, otherwise Human.
     - Otherwise, twice Orc below Goblin: Goblin if Human is below twice Goblin, otherwise Human.
     - Otherwise: Orc if Human is below twice Orc, otherwise Human.
   - In any other region, the race with the highest summed potential wins. On a tie the earlier race in the `Race.AllIds` order wins, so a region where nobody can live is Human.

   `PickMainRace` never returns `None`, so in practice the "no main race means human" rule in the name generator is a safety net.
5. The ruling family surname is generated from the main race's `Language`, and then the region name.

### Supporting and opposing races

Not used for naming, listed for completeness. `PickSupportingRaces` ([MapGenerator.cs:954](../Generator/MapGenerator.cs#L954)) returns up to three races with the highest potential in the region, taken from a fixed list of allies of the main race. Ent counts three times and Minotaur twice when ranking.

| Main race | Possible supporting races |
|---|---|
| Human, region more than half Dark | Orc, Goblin, Lizard, Minotaur |
| Human, otherwise | Elve, Dwarf, Haflings, Hare |
| Elve | Human, Haflings, Hare, Ent, Fae, Centaur |
| Dwarf | Human, Haflings |
| Orc | Goblin, Lizard, Minotaur |
| Goblin | Orc, Lizard, Minotaur |
| Haflings | Human, Elve, Hare |
| Lizard | Goblin, Orc |
| Hare | Elve, Haflings, Ent, Fae |
| Centaur | Elve, Haflings, Ent, Fae, Hare |
| Ent | Elve, Fae, Hare, Centaur |
| Minotaur | Goblin, Orc |
| Satyr | Fae, Nymph |
| Fae | Satyr, Nymph |
| Nymph | Satyr, Fae |
| Mer, Thiefling | none |

A race is included even when its potential in the region is zero. `PickOpposingRaces` is a stub that always returns an empty list.

### Things in the race data to check before copying

These look unintended. I have not confirmed the intent, so treat them as questions:

- **Fae, Nymph and Thiefling are limited to shallow water.** Their `Filter` uses the `ShallowWater` height band, which is the range below sea level ([Model/Height.cs:59](../Model/Height.cs#L59), [:78](../Model/Height.cs#L78)), the same band as Mer. Their biomes are forests and plains. A location has to be under water and forested for them to live there, so their potential is probably zero almost everywhere and they can hardly ever become a main race.
- **Mer has `PopulationModifier` 13**, where every other race is between 0.13 and 1. This may be deliberate, to compensate for the few biomes Mer can use.
- **Two biomes are written as numbers.** Dwarf's secondary list has `(BiomIds)1` and Goblin's has `(BiomIds)19`. By the enum order these are AncientForest and Permafrost; the tables above show them resolved and marked with `*`. If `BiomIds` is ever reordered, these silently change meaning.
- **Lizard lists TropicalMoistForests as both primary and secondary.** The primary entry wins.
- **Spelling of identifiers.** `Elve`, `Haflings`, `Thiefling` (display name "Tiefling"), `Hare` (display name "Harengon") and `Ent` (display name "Treant"). If the other repo spells them differently, keep an explicit mapping.

## Reference names

Seeds 0 to 9. `Original` mode, identical to IdleGame's `GenerateRegionName`:

| Inputs | Seeds 0 to 9 |
|---|---|
| Human, 50,000, "Davies" | Tahirid Republic, Northern Davies Empire, New Trebizond Republic, Eastern Davies Region, New Wu Zhou Alliance, Southen Davies Nation, New Assyrian Alliance, Western Kievan Rus', Neo-England State, Magadha |
| Human, 5,000, no family | Tahirid Alliance, Northern Harsha Region, New Trebizond State, Eastern Ikomau Nation, New Wu Zhou Union, Southen Kadamba, New Assyrian Territory, Western Kievan Rus', Neo-England Province, Magadha |
| Non-human, 50,000, "Stone" | Parthian, Southen Stone Empire, Sutat, Stone Empire, Toltec Ground, Stone Region, Uyghur Sect, Stone Nation, Third Zimshath Land, Western Later |
| Non-human, 50,000, no family | Parthian, Southen Gupta Empire, Sutat, Huilstin Empire, Toltec Ground, Jihu Region, Uyghur Sect, Kamtel Nation, Third Zimshath Land, Western Later |

`Corrected` mode:

| Inputs | Seeds 0 to 9 |
|---|---|
| Human, 50,000, "Davies" | New Xianbei Republic, Chalukya Region, West Liao Alliance, Neo-Davies Nation, Davies Alliance, Eastern Idisid Nation, Free Saffarid State, Babylonian, Western Davies State, East Mauryan |
| Human, 500,000, "Davies" | Xianbei Republic, Chalukya Region, Eastern Liao Alliance, Davies Nation, Neo-Davies Alliance, Idisid Nation, Greath Saffarid State, Babylonian, Davies State, East Mauryan |
| Non-human, 50,000, "Stone" | Toucouleur, Central Empire, South Khmer, Second Stone Region, Old Stone Ground, Western Hotak Nation, Fifth Qin Land, Stone Nation, Stone Land, East Magadha |

A minimal xUnit test (not compiled; the expected names come from the verification run):

```csharp
[Fact]
public void Generates_the_reference_region_names()
{
    Assert.Equal("Northern Davies Empire", RegionNameGenerator.Generate(1, true, 50_000, "Davies"));
    Assert.Equal("Neo-England Province", RegionNameGenerator.Generate(8, true, 5_000));
    Assert.Equal("Third Zimshath Land", RegionNameGenerator.Generate(8, false, 50_000, "Stone"));

    Assert.Equal("Old Stone Ground", RegionNameGenerator.Generate(4, false, 50_000, "Stone", RegionNameMode.Corrected));
    Assert.Equal("Greath Saffarid State", RegionNameGenerator.Generate(6, true, 500_000, "Davies", RegionNameMode.Corrected));
}
```

## Not covered

`GenerateCityName` ([NameGenerator.cs:134-166](../Generator/NameGenerator.cs#L134)) is an unfinished stub that always returns an empty string. It is the only place in the file that would use a race together with a location's height, biome and climate.
