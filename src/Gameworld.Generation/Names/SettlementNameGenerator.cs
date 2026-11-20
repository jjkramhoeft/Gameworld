using Gameworld.Domain;
using Gameworld.Generation.Basics;

namespace Gameworld.Generation.Names;

/// <summary>
/// What the place word of a name says about the site of a city or a town
/// (society-generation.md section 5.3). A site holds every class whose condition is true for
/// its Prospect, and one of them is drawn for the name.
/// </summary>
public enum SiteClass
{
    /// <summary>A navigable river meets the sea.</summary>
    Mouth,
    /// <summary>A harbour: the patch borders a bay.</summary>
    Haven,
    /// <summary>A coast without a harbour.</summary>
    Shore,
    /// <summary>The upstream end of navigation.</summary>
    Landing,
    /// <summary>Two rivers meet.</summary>
    Meeting,
    /// <summary>A river that can be forded.</summary>
    Ford,
    /// <summary>A navigable river, which takes a bridge or a dam.</summary>
    Bridge,
    /// <summary>A lake.</summary>
    Mere,
    /// <summary>Hills or mountains.</summary>
    Hill,
    /// <summary>A forest biome.</summary>
    Wood,
    /// <summary>Open grass land.</summary>
    Field,
    /// <summary>A desert, dry scrub, tundra or ice.</summary>
    Waste,
    /// <summary>Salt is listed on the patch.</summary>
    Salt,
    /// <summary>A deposit of ore, gold or gems on the patch.</summary>
    Delve,
    /// <summary>A faction of the type Belief sits here.</summary>
    Shrine,
    /// <summary>A faction of the type Vocation or Place sits here, or the trade factor is high.</summary>
    Market,
    /// <summary>A city: a walled or a great place.</summary>
    Fort,
    /// <summary>A town: a homestead.</summary>
    Farm,
}

/// <summary>
/// How a tongue coins a first part from syllables: one of <paramref name="First"/>, some of
/// <paramref name="Middle"/> and one of <paramref name="Last"/> when the list is not empty.
/// </summary>
/// <param name="First">The opening syllables. They are written with a capital.</param>
/// <param name="Middle">The syllables between the first and the last.</param>
/// <param name="Last">The closing syllables; empty for a tongue whose first parts end on a middle syllable. In a mirrored tongue they are the letters in the middle.</param>
/// <param name="MinMiddle">The fewest middle syllables.</param>
/// <param name="MaxMiddle">The most middle syllables.</param>
/// <param name="Mirror">The first part reads the same both ways: the syllables, one of <paramref name="Last"/>, then the syllables backwards.</param>
public sealed record NameSyllables(
    string[] First, string[] Middle, string[] Last, int MinMiddle, int MaxMiddle, bool Mirror = false);

/// <summary>
/// One tongue of a race, as its place names read it: where a first part comes from, and the
/// place words of each site class. A place word that ends in a space or a hyphen stands in
/// front of the first part ("Bir ", "Ile-"), one that starts with a space or a hyphen is
/// written after it as it stands (" Gate", "-under-Oak"), and any other is joined to it
/// ("vik").
/// </summary>
/// <param name="Weight">How often the tongue is drawn among the tongues of its lexicon.</param>
/// <param name="Heads">The listed first parts. Empty for a tongue that coins every first part.</param>
/// <param name="Syllables">How a first part is coined; null for a tongue that does not coin from syllables.</param>
/// <param name="Surnames">The first part is coined from the surname model of this name style; null for a tongue that does not.</param>
/// <param name="CoinedShare">How often the first part is coined, where the tongue has both listed first parts and a way to coin.</param>
/// <param name="BareShare">How often a coined first part is the whole name, without a place word.</param>
/// <param name="Elide">The tongue wears its seams down: a letter doubled across a seam is written once, and a vowel at the end of a part falls before a vowel. In a tongue that does not, a name with a letter doubled where the first part and a joined place word meet is drawn again.</param>
/// <param name="Words">The place words of the site classes the tongue has words for.</param>
public sealed record NameTongue(
    int Weight,
    string[] Heads,
    NameSyllables? Syllables,
    NameStyle? Surnames,
    double CoinedShare,
    double BareShare,
    bool Elide,
    IReadOnlyDictionary<SiteClass, string[]> Words)
{
    /// <summary>The tongue can coin a first part.</summary>
    public bool Coins => Syllables is not null || Surnames is not null;
}

/// <summary>The place names of a race: its tongues. A race with one tongue has one entry.</summary>
public sealed record SettlementNameLexicon(IReadOnlyList<NameTongue> Tongues);

/// <summary>One drawn name.</summary>
/// <param name="Name">The name.</param>
/// <param name="Site">The site class of its place word; null for a name without a place word.</param>
/// <param name="Valid">The name keeps the length limits and holds plain letters only, its first part is not one syllable said twice and does not end in its place word, and no letter is doubled where the two are joined in a tongue that keeps its seams. A name that is not valid is drawn again.</param>
public readonly record struct DrawnName(string Name, SiteClass? Site, bool Valid);

/// <summary>
/// The city and town name generator (society-generation.md section 5.3): names such as
/// "Jarlhavn", "Khar Nuur" or "Qasr Mehret". A name is a first part and a place word. The
/// race gives the tongue: its first parts and its place words. The site gives the class of the
/// place word: a harbour, a ford, a hill, a city or a town. Stateless and thread-safe.
/// </summary>
public static class SettlementNameGenerator
{
    /// <summary>A name shorter than this is drawn again.</summary>
    public const int MinLength = 4;
    /// <summary>A name longer than this is drawn again.</summary>
    public const int MaxLength = 24;
    /// <summary>A surname longer than this is drawn again, where a tongue coins from a surname model.</summary>
    public const int SurnameMaxLength = 8;
    /// <summary>How many surnames are drawn at most; the last one is kept whatever its length.</summary>
    public const int SurnameAttempts = 10;

    /// <summary>
    /// How much each site class weighs in the draw for the place word. The rarer and the more
    /// telling a fact is, the more it weighs.
    /// </summary>
    public static int WeightOf(SiteClass site) => site switch
    {
        SiteClass.Mouth => 6,
        SiteClass.Haven => 5,
        SiteClass.Shrine => 5,
        SiteClass.Landing => 4,
        SiteClass.Meeting => 4,
        SiteClass.Mere => 4,
        SiteClass.Salt => 4,
        SiteClass.Delve => 4,
        SiteClass.Market => 4,
        SiteClass.Fort => 4,
        SiteClass.Farm => 4,
        SiteClass.Shore => 3,
        SiteClass.Ford => 3,
        SiteClass.Bridge => 3,
        SiteClass.Hill => 3,
        _ => 2,
    };

    /// <summary>
    /// One name, drawn from <paramref name="rng"/>. Seed the generator from a
    /// <see cref="Hashing"/> hash of the Prospect and the purpose, so that the name is a pure
    /// function of the foundation. The numbers are taken in a fixed order: the tongue, the
    /// site class, the place word, coined or listed, the first part, bare or not.
    /// </summary>
    /// <param name="sites">The site classes of the Prospect. The order does not matter.</param>
    public static DrawnName Draw(Sfc32 rng, SettlementNameLexicon lexicon, IEnumerable<SiteClass> sites)
    {
        var tongue = PickTongue(lexicon.Tongues, rng.Next());

        // The site class: among the classes of the site the tongue has words for, by weight.
        var siteDraw = rng.Next();
        var wordDraw = rng.Next();
        var candidates = sites.Distinct().Where(tongue.Words.ContainsKey).OrderBy(s => s).ToArray();
        SiteClass? site = null;
        string? word = null;
        if (candidates.Length > 0)
        {
            var x = siteDraw * candidates.Sum(WeightOf);
            var chosen = candidates[^1];
            var running = 0;
            foreach (var candidate in candidates)
            {
                running += WeightOf(candidate);
                if (x < running)
                {
                    chosen = candidate;
                    break;
                }
            }
            var words = tongue.Words[chosen];
            if (words.Length > 0)
            {
                site = chosen;
                word = words[Index(wordDraw, words.Length)];
            }
        }

        // The first part: coined in the tongue, or one of its listed first parts.
        var coinedDraw = rng.Next();
        var coined = tongue.Coins && (tongue.Heads.Length == 0 || coinedDraw < tongue.CoinedShare);
        var first = coined ? Coin(tongue, rng)
            : tongue.Heads.Length > 0 ? tongue.Heads[Index(rng.Next(), tongue.Heads.Length)]
            : "";

        var bareDraw = rng.Next();
        if (word is null || (coined && bareDraw < tongue.BareShare))
            return new DrawnName(first, null, InLimits(first) && !Repeats(first));

        var name = Join(first, word, tongue.Elide);
        return new DrawnName(
            name, site,
            InLimits(name) && !Repeats(first) && !EndsIn(first, word) && (tongue.Elide || !Doubles(first, word)));
    }

    /// <summary>The name keeps the length limits and holds the letters A to Z, spaces, hyphens and apostrophes only.</summary>
    private static bool InLimits(string name) =>
        name.Length is >= MinLength and <= MaxLength
        && name.All(c => c is (>= 'A' and <= 'Z') or (>= 'a' and <= 'z') or ' ' or '-' or '\'');

    private static NameTongue PickTongue(IReadOnlyList<NameTongue> tongues, double u)
    {
        var x = u * tongues.Sum(t => Math.Max(0, t.Weight));
        var running = 0;
        foreach (var tongue in tongues)
        {
            running += Math.Max(0, tongue.Weight);
            if (x < running) return tongue;
        }
        return tongues[^1];
    }

    private static int Index(double u, int count) => Math.Min(count - 1, (int)(u * count));

    /// <summary>A first part in the tongue: from its syllables, or from the surname model of its name style.</summary>
    private static string Coin(NameTongue tongue, Sfc32 rng)
    {
        if (tongue.Syllables is not { } s)
        {
            var surname = "";
            for (var attempt = 0; attempt < SurnameAttempts; attempt++)
            {
                surname = NameGenerator.Default.Generate(tongue.Surnames!.Value, NameKind.Surname, rng);
                if (surname.Length <= SurnameMaxLength) break;
            }
            return surname;
        }

        var middles = s.MinMiddle + Index(rng.Next(), Math.Max(1, s.MaxMiddle - s.MinMiddle + 1));
        var stem = s.First[Index(rng.Next(), s.First.Length)];
        for (var k = 0; k < middles && s.Middle.Length > 0; k++)
            stem = Seam(stem, s.Middle[Index(rng.Next(), s.Middle.Length)], tongue.Elide);
        if (s.Last.Length == 0) return stem;

        var last = s.Last[Index(rng.Next(), s.Last.Length)];
        if (!s.Mirror) return Seam(stem, last, tongue.Elide);

        var back = stem.ToLowerInvariant().ToCharArray();
        Array.Reverse(back);
        return stem + last + new string(back);
    }

    /// <summary>
    /// The first part and the place word as one name. A place word that ends in a space or a
    /// hyphen stands in front, one that starts with a space or a hyphen is written after the
    /// first part as it stands, and any other is joined to it.
    /// </summary>
    internal static string Join(string first, string word, bool elide)
    {
        if (word.EndsWith(' ') || word.EndsWith('-')) return word + first;
        if (word.StartsWith(' ') || word.StartsWith('-')) return first + word;
        // "Marshal's" and "Strangers'" keep the place word as a word of its own.
        if (first.EndsWith("'s", StringComparison.Ordinal) || first.EndsWith('\''))
            return first + " " + char.ToUpperInvariant(word[0]) + word[1..];
        return Seam(first, word, elide);
    }

    /// <summary>
    /// Two parts joined. Where <paramref name="elide"/> holds, a letter doubled across the seam
    /// is written once and a vowel at the end of the first part falls before a vowel.
    /// </summary>
    internal static string Seam(string left, string right, bool elide)
    {
        if (!elide || left.Length == 0 || right.Length == 0) return left + right;
        var a = char.ToLowerInvariant(left[^1]);
        var b = char.ToLowerInvariant(right[0]);
        if (a == b) return left + right[1..];
        if (IsVowel(a) && IsVowel(b) && left.Length > 2) return left[..^1] + right;
        return left + right;
    }

    /// <summary>The first part is one syllable said twice: "Yuyu".</summary>
    internal static bool Repeats(string first)
    {
        if (first.Length < 2 || first.Length % 2 != 0) return false;
        var half = first.Length / 2;
        return string.Compare(first, 0, first, half, half, StringComparison.OrdinalIgnoreCase) == 0;
    }

    /// <summary>The first part and a joined place word meet in the same letter: "Raft" and "town".</summary>
    internal static bool Doubles(string first, string word)
    {
        if (first.Length == 0 || word.Length == 0) return false;
        if (word.EndsWith(' ') || word.EndsWith('-') || word.StartsWith(' ') || word.StartsWith('-')) return false;
        if (first.EndsWith('\'') || first.EndsWith("'s", StringComparison.Ordinal)) return false;
        return char.ToLowerInvariant(first[^1]) == char.ToLowerInvariant(word[0]);
    }

    private static bool IsVowel(char c) => c is 'a' or 'e' or 'i' or 'o' or 'u';

    /// <summary>The first part already ends in the place word: "Reef" and "reef", "Hollow" and " Hollow".</summary>
    internal static bool EndsIn(string first, string word)
    {
        var bare = word.Trim(' ', '-');
        return bare.Length > 0 && first.EndsWith(bare, StringComparison.OrdinalIgnoreCase);
    }
}
