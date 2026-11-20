using Gameworld.Generation.Basics;

namespace Gameworld.Generation.Names;

/// <summary>How large a region is, as the region-name generator reads it.</summary>
public enum RegionNameSize
{
    /// <summary>Few persons: a human region can be a Duchy, a Province, a Len or a County.</summary>
    Small,
    /// <summary>Neither small nor large: the size does not show in the name.</summary>
    Middling,
    /// <summary>Many persons: a human region can be Gran, Grand or Great.</summary>
    Large,
}

/// <summary>Who rules a region, as the region-name generator reads it.</summary>
public enum RegionNameRuler
{
    /// <summary>Not said: every suffix can be drawn, as in the original generator.</summary>
    Any,
    /// <summary>A house rules: no Republic, Alliance or Union.</summary>
    House,
    /// <summary>No house rules: no Kingdom, Dynasty, Empire, Duchy, County, Len, Khaganate or Sultanate.</summary>
    Other,
}

/// <summary>
/// The region-name generator (docs/RegionNameGenerator-Reuse.md, society-generation.md section
/// 4.10): names such as "South Davies Kingdom" or "New Sokoto Republic" from weighted word
/// lists. A name is prefix + middle + suffix, where the prefix and the suffix may be empty. It
/// is the generator of the reuse doc in its Corrected mode: the draws are independent and every
/// word has exactly its listed weight. One rule is added to it: the suffix follows who rules
/// the region. Stateless and thread-safe.
/// </summary>
public static class RegionNameGenerator
{
    /// <summary>Below this population potential a region was small in the original generator.</summary>
    public const int OriginalSmallBelow = 10_000;

    /// <summary>Above this population potential a region was large in the original generator.</summary>
    public const int OriginalLargeAbove = 100_000;

    /// <summary>
    /// One name, drawn from <paramref name="rng"/>. This is the overload for generation code:
    /// seed the <see cref="Sfc32"/> from a <see cref="Hashing"/> hash of the region and the
    /// purpose, so that the name is a pure function of the foundation.
    /// </summary>
    /// <param name="human">True picks the word lists of the human races, false those of the other races.</param>
    /// <param name="ruler">Who rules the region: it leaves out the suffixes that do not fit.</param>
    /// <param name="rulingHouse">The surname of the ruling house, or null. It can take the place of the middle.</param>
    public static string Generate(
        Sfc32 rng, bool human, RegionNameSize size, RegionNameRuler ruler = RegionNameRuler.Any, string? rulingHouse = null) =>
        Generate(n => Math.Min(n - 1, (int)(rng.Next() * n)), human, size, ruler, rulingHouse);

    /// <summary>
    /// One name from <c>new Random(seed)</c> and the population limits of the original: the name
    /// the Corrected mode of the reuse doc gives for the seed. Kept to check the port and the
    /// word lists against the reference names. .NET does not promise the sequence across
    /// versions, so generation code uses the <see cref="Sfc32"/> overload.
    /// </summary>
    public static string Generate(int seed, bool human, int populationPotential, string? rulingFamilyName = null)
    {
        var size = populationPotential < OriginalSmallBelow ? RegionNameSize.Small
            : populationPotential > OriginalLargeAbove ? RegionNameSize.Large
            : RegionNameSize.Middling;
        return Generate(new Random(seed).Next, human, size, RegionNameRuler.Any, rulingFamilyName);
    }

    /// <param name="next">A uniform integer from 0 up to, not including, its argument.</param>
    private static string Generate(
        Func<int, int> next, bool human, RegionNameSize size, RegionNameRuler ruler, string? rulingHouse)
    {
        if (string.IsNullOrWhiteSpace(rulingHouse)) rulingHouse = null;

        List<(string Text, int Weight)> pre = [.. Prefix, .. (human ? PrefixHuman : PrefixNonhuman)];
        List<(string Text, int Weight)> mid = [.. Middle, .. (human ? MiddleHuman : MiddleNonhuman)];
        List<(string Text, int Weight)> post = [.. Suffix, .. (human ? SuffixHuman : SuffixNonhuman)];
        if (human && size == RegionNameSize.Small) post.AddRange(SuffixSmall);

        // The suffix follows the ruler: no realm of a house without one, and no republic under one.
        if (ruler == RegionNameRuler.House) post.RemoveAll(option => NoHouseSuffixes.Contains(option.Text));
        if (ruler == RegionNameRuler.Other) post.RemoveAll(option => HouseSuffixes.Contains(option.Text));

        // The order of the draws is the order of the original: suffix, middle, prefix, ruling house.
        var chosenPost = Pick(post, next);
        var chosenMid = Pick(mid, next);
        var hasPost = chosenPost.Length > 0;

        // The prefixes that need a suffix: the sizes of a large human region, the ordinals of the others.
        if (human)
        {
            if (size == RegionNameSize.Large && hasPost) pre.AddRange(PrefixBig);
        }
        else if (hasPost)
        {
            pre.AddRange(PrefixNumber);
        }

        var chosenPre = Pick(pre, next);

        // "United ... Union" and "Second ... Sect": no prefix that starts with the same two letters as the suffix.
        if (chosenPre.Length > 0 && hasPost && chosenPre.Trim()[..2].Equals(chosenPost.Trim()[..2]))
            chosenPre = "";

        // The ruling house takes the place of the middle half of the time, in a name with a suffix.
        if (rulingHouse is not null && hasPost && next(6) <= 2)
            chosenMid = rulingHouse;

        // The middle may be empty only between a prefix and a suffix ("North Kingdom").
        if (chosenMid.Length == 0 && (!hasPost || chosenPre.Length == 0))
            chosenMid = Pick(mid.Where(option => option.Text.Length > 0).ToList(), next);

        return (chosenPre + chosenMid + chosenPost).Replace("  ", " ").Trim();
    }

    /// <summary>One of the options by weight: the first one where the running sum of the weights passes the draw.</summary>
    private static string Pick(IReadOnlyList<(string Text, int Weight)> options, Func<int, int> next)
    {
        var sum = 0;
        foreach (var option in options) sum += option.Weight;
        var x = next(sum);
        var runningSum = 0;
        foreach (var option in options)
        {
            runningSum += option.Weight;
            if (x < runningSum) return option.Text;
        }
        return "";
    }

    /// <summary>The suffixes of a realm that a house rules: they are left out where no house rules.</summary>
    internal static readonly HashSet<string> HouseSuffixes =
        [" Kingdom", " Dynasty", " Empire", " Duchy", " County", " Len", " Khaganate", " Sultanate"];

    /// <summary>The suffixes of a realm without a ruling house: they are left out where a house rules.</summary>
    internal static readonly HashSet<string> NoHouseSuffixes = [" Republic", " Alliance", " Union"];

    // The word lists of the original. Each entry is (text, weight). A prefix carries its trailing
    // space and a suffix its leading space. This file is UTF-8: the lists hold é, ö, å, â and ’.

    internal static readonly (string Text, int Weight)[] Prefix =
    [
        ("North ", 2), ("East ", 2), ("South ", 2), ("West ", 2), ("Northern ", 2), ("Eastern ", 2), ("Southern ", 2),
        ("Western ", 2),
    ];

    internal static readonly (string Text, int Weight)[] PrefixHuman =
    [
        ("", 15), ("New ", 5), ("Neo-", 2), ("United ", 2), ("Free ", 2),
    ];

    internal static readonly (string Text, int Weight)[] PrefixNonhuman =
    [
        ("", 15), ("Old ", 5),
    ];

    internal static readonly (string Text, int Weight)[] PrefixBig =
    [
        ("", 15), ("Gran ", 1), ("Grand ", 2), ("Great ", 4),
    ];

    internal static readonly (string Text, int Weight)[] PrefixNumber =
    [
        ("", 15), ("Second ", 1), ("Third ", 2), ("Fourth ", 3), ("Fifth ", 2), ("Sixth ", 1),
    ];

    internal static readonly (string Text, int Weight)[] Middle =
    [
        ("", 10), ("Abbasid", 1), ("Aceh", 1), ("Achaemenid", 1), ("Afsharid", 1), ("Aizak", 1), ("Ajuran", 1),
        ("Akkadian", 1), ("Angevin", 1), ("Austro", 1), ("Avar", 1), ("Balhae", 1), ("Bamana", 1), ("Banjar", 1),
        ("Benin", 1), ("Bese", 1), ("Blanind", 1), ("Bogd", 1), ("Bornu", 1), ("Brazilian", 1), ("Bretoth", 1),
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
        ("Seleucid", 1), ("Seleucid", 1), ("Seljuk", 1), ("Seljuk", 1), ("Seorrul", 1), ("Shah", 1), ("Shang", 1),
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
        ("Wu Zhou", 1), ("Xia", 1), ("Xianbei", 1), ("Xin", 1), ("Xiongnu", 1), ("Xiongnu", 1), ("Yaswun", 1),
        ("Yuan", 1), ("Zand", 1), ("Zhou", 1), ("Zikil", 1), ("Zimshath", 1), ("Zirid", 1), ("Zogni", 1),
        ("Zoize", 1), ("Aarloe", 1),
    ];

    internal static readonly (string Text, int Weight)[] MiddleHuman =
    [
        ("Assyria", 1), ("Assyrian", 1), ("Athens", 1), ("Austrian", 1), ("Aztec", 1), ("Babylonia", 1),
        ("Babylonian", 1), ("Belgian", 1), ("Bengal", 1), ("Brazil", 1), ("Britannic", 1), ("British", 1),
        ("Bulgarian", 1), ("Byzantine", 1), ("Danish", 1), ("Dutch", 1), ("Egyptian", 1), ("England", 1),
        ("Ethiopian", 1), ("France", 1), ("Frankish", 1), ("French", 1), ("Georgia", 1), ("German", 1), ("Haiti", 1),
        ("Inca", 1), ("Indian", 1), ("Israel", 1), ("Italian", 1), ("Korean", 1), ("Latin", 1), ("Macedonian", 1),
        ("Mongol", 1), ("Napoleonic", 1), ("Norway", 1), ("Norwegian", 1), ("Ottoman", 1), ("Portuguese", 1),
        ("Portuguese", 1), ("Roman", 1), ("Russian", 1), ("Scottish", 1), ("Serbian", 1), ("Spanish", 1),
        ("Spartan", 1), ("Sweden", 1), ("Swedish", 1), ("Armenian", 1), ("Baltikum", 1), ("Japanese", 1),
        ("Thessalonica", 1), ("Vietnam", 1), ("Zimbabwe", 1), ("Zulu", 1),
    ];

    internal static readonly (string Text, int Weight)[] MiddleNonhuman =
    [
        ("Eternia", 1), ("Giania", 1), ("Florania", 1), ("Aghlabids", 1), ("Aiwuk", 1), ("Aksumite", 1),
        ("Akwamu", 1), ("Alaouite", 1), ("Almohad", 1), ("Almoravid", 1), ("Aq Qoyunlu", 1), ("Aragon", 1),
        ("Ashanti", 1), ("Aulikara", 1), ("Ayutthaya", 1), ("Ayyubid", 1), ("Ghaznavid", 1), ("Hoysala", 1),
        ("Khwarazmian", 1), ("Knaupzond", 1), ("Mlechchha", 1), ("Ograq", 1), ("Purépecha", 1), ("Qoyunlu", 1),
        ("Schihhish", 1), ("Sphephrasha", 1), ("Swomzuq", 1), ("Vijayanagara", 1), ("Xopfand", 1),
    ];

    internal static readonly (string Text, int Weight)[] Suffix =
    [
        (" Kingdom", 6), (" Dynasty", 2), (" Empire", 3), (" Region", 2), (" Nation", 3),
    ];

    internal static readonly (string Text, int Weight)[] SuffixHuman =
    [
        ("", 15), (" Republic", 5), (" Alliance", 4), (" State", 3), (" Union", 2), (" Territory", 2),
    ];

    internal static readonly (string Text, int Weight)[] SuffixNonhuman =
    [
        ("", 15), (" Ground", 2), (" Sect", 1), (" Land", 3), ("land", 1), (" Khaganate", 1), (" Sultanate", 1),
    ];

    internal static readonly (string Text, int Weight)[] SuffixSmall =
    [
        (" Duchy", 1), (" Province", 1), (" Len", 3), (" County", 3),
    ];
}
