using Gameworld.Domain;
using Gameworld.Generation.Basics;

namespace Gameworld.Generation.Names;

/// <summary>A Prospect as the name of a city or a town reads it.</summary>
/// <param name="Patch">The root cell of the patch of the Prospect: the identity the draws are seeded from.</param>
/// <param name="Races">The ids of the races that can name the place: the races of the Prospect, or the lead race of the ruling faction alone for the capital. Empty where the Prospect holds no race.</param>
/// <param name="Weights">The weight of each race of <paramref name="Races"/> in the draw for the race that names the place.</param>
/// <param name="Sites">The site classes of the Prospect.</param>
public sealed record NamedProspect(
    GridCoord Patch, IReadOnlyList<string> Races, IReadOnlyList<double> Weights, IReadOnlyList<SiteClass> Sites);

/// <summary>The name of a city or a town on a Prospect, and what it was drawn from.</summary>
/// <param name="Name">The name.</param>
/// <param name="Race">The id of the race that named the place; null where the Prospect holds no race.</param>
/// <param name="Site">The site class of the place word of the name; null for a name without a place word.</param>
public sealed record SettlementNaming(string Name, string? Race, SiteClass? Site);

/// <summary>
/// The name of a city or a town (society-generation.md section 5.3): what the name generator
/// is given for the Prospects of a region. The site classes come from the facts of a Prospect,
/// its patch and the factions that sit on it. The race that names the place is drawn among the
/// races of the Prospect by their weight there, and it picks the lexicon. No two Prospects of a
/// region get the same name. A pure function of the region's own Planned data.
/// </summary>
public sealed class SettlementNameRules
{
    /// <summary>Hash purpose of the draws for the name of a city or a town.</summary>
    public const uint NamePurpose = 0x4c01;

    /// <summary>How many names are drawn at most for one Prospect; the last one is kept.</summary>
    public const int Attempts = 20;

    /// <summary>A river of this many patches drained and up is navigable (map-generation.md section 15).</summary>
    public const double NavigableRiver = 8;
    /// <summary>A river of this many patches drained and up, below navigable, is one to ford.</summary>
    public const double FordableRiver = 2;
    /// <summary>The smaller branch of a river junction from this many patches drained makes a meeting of rivers.</summary>
    public const double MeetingConfluence = 4;
    /// <summary>A trade factor from this and up makes a market.</summary>
    public const double MarketTrade = 0.9;

    private readonly Dictionary<string, RaceParameters> _races = new(StringComparer.Ordinal);
    private readonly uint _seed;

    /// <param name="seed">The seed of the foundation: it gives the draws.</param>
    public SettlementNameRules(ParameterSet parameters, uint seed = 0)
    {
        foreach (var race in parameters.Races) _races.TryAdd(race.Id, race);
        _seed = seed;
    }

    /// <summary>
    /// The lexicon of a race: its own, or that of its name style where it has none. A Prospect
    /// without a race takes the lexicon of the Common style.
    /// </summary>
    public SettlementNameLexicon LexiconOf(string? race)
    {
        if (race is null) return SettlementNameLexicons.OfStyle(NameStyle.Common);
        return SettlementNameLexicons.Of(race)
            ?? SettlementNameLexicons.OfStyle(_races.TryGetValue(race, out var r) ? r.NameStyle : NameStyle.Common);
    }

    /// <summary>The site classes of a Prospect, in the order of <see cref="SiteClass"/>.</summary>
    /// <param name="facts">The facts of the Prospect.</param>
    /// <param name="zoneType">The elevation type of the zone of its patch.</param>
    /// <param name="biome">The estimated biome of its patch.</param>
    /// <param name="resources">The estimated resources of its patch.</param>
    /// <param name="trade">The trade factor of the Prospect.</param>
    /// <param name="city">The place is a city: the capital or a Prospect for a city. Otherwise it is a town.</param>
    /// <param name="seated">The types of the factions that sit on the Prospect.</param>
    public static SiteClass[] SitesOf(
        ProspectFacts facts, ElevationType zoneType, Biome biome, PatchResources resources, double trade, bool city,
        IEnumerable<FactionType> seated)
    {
        var sites = new List<SiteClass>();
        if (facts.Mouth >= NavigableRiver) sites.Add(SiteClass.Mouth);
        if (facts.Harbour) sites.Add(SiteClass.Haven);
        else if (facts.Coast) sites.Add(SiteClass.Shore);
        if (facts.HeadOfNavigation) sites.Add(SiteClass.Landing);
        if (facts.Confluence >= MeetingConfluence) sites.Add(SiteClass.Meeting);
        if (facts.River >= NavigableRiver) sites.Add(SiteClass.Bridge);
        else if (facts.River >= FordableRiver) sites.Add(SiteClass.Ford);
        if (facts.LakeAreaM2 > 0) sites.Add(SiteClass.Mere);
        if (zoneType is ElevationType.Hills or ElevationType.Mountains) sites.Add(SiteClass.Hill);

        switch (biome)
        {
            case Biome.BorealForest or Biome.WoodlandAndShrubland or Biome.TemperateMixedForest
                or Biome.TemperateBroadleafForest or Biome.TemperateRainForest or Biome.TropicalDryForest
                or Biome.TropicalRainForest or Biome.Mangrove:
                sites.Add(SiteClass.Wood);
                break;
            case Biome.ColdSteppe or Biome.TemperateGrassland or Biome.Savanna:
                sites.Add(SiteClass.Field);
                break;
            case Biome.IceAndSnow or Biome.Tundra or Biome.ColdDesert or Biome.HotDesert or Biome.ThornScrub:
                sites.Add(SiteClass.Waste);
                break;
        }

        if (resources[Resource.Salt].IsListed) sites.Add(SiteClass.Salt);
        if (Mined.Any(r => resources[r] is { IsListed: true, Origin: ResourceOrigin.Deposit })) sites.Add(SiteClass.Delve);

        var types = seated.ToHashSet();
        if (types.Contains(FactionType.Belief)) sites.Add(SiteClass.Shrine);
        if (types.Contains(FactionType.Vocation) || types.Contains(FactionType.Place) || trade >= MarketTrade)
            sites.Add(SiteClass.Market);

        sites.Add(city ? SiteClass.Fort : SiteClass.Farm);
        sites.Sort();
        return sites.ToArray();
    }

    /// <summary>The resources a place is named a delve for, when the patch holds a deposit of one.</summary>
    private static readonly Resource[] Mined =
        [Resource.IronOre, Resource.CopperOre, Resource.Tin, Resource.SilverAndLead, Resource.Gold, Resource.GemsAndPearls];

    /// <summary>
    /// The race that names a place: one of the races of the Prospect, by weight. The first
    /// race where the running sum of the weights passes the draw; the first race of the list
    /// where no race has a weight. Null for a Prospect without a race.
    /// </summary>
    internal static string? NamerOf(NamedProspect prospect, double u)
    {
        if (prospect.Races.Count == 0) return null;
        var total = 0.0;
        for (var k = 0; k < prospect.Races.Count; k++) total += WeightOf(prospect, k);
        if (!(total > 0)) return prospect.Races[0];
        var x = u * total;
        var running = 0.0;
        for (var k = 0; k < prospect.Races.Count; k++)
        {
            running += WeightOf(prospect, k);
            if (x < running) return prospect.Races[k];
        }
        return prospect.Races[^1];
    }

    private static double WeightOf(NamedProspect prospect, int k) =>
        k < prospect.Weights.Count && double.IsFinite(prospect.Weights[k]) ? Math.Max(0, prospect.Weights[k]) : 0;

    /// <summary>
    /// The names of the Prospects of a region, in the order of the Prospects. The stream of a
    /// Prospect gives the race that names it first, then its name. A name that is not valid,
    /// or that a Prospect before it in the order already has, is drawn again from the same
    /// stream; the last of <see cref="Attempts"/> draws is kept.
    /// </summary>
    public SettlementNaming[] Name(IReadOnlyList<NamedProspect> prospects)
    {
        var taken = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var result = new SettlementNaming[prospects.Count];
        for (var i = 0; i < prospects.Count; i++)
        {
            var prospect = prospects[i];
            var rng = new Sfc32(Hashing.Hash4(_seed, NamePurpose, prospect.Patch.I, prospect.Patch.J));
            var race = NamerOf(prospect, rng.Next());
            var lexicon = LexiconOf(race);
            var drawn = default(DrawnName);
            for (var attempt = 0; attempt < Attempts; attempt++)
            {
                drawn = SettlementNameGenerator.Draw(rng, lexicon, prospect.Sites);
                if (drawn.Valid && !taken.Contains(drawn.Name)) break;
            }
            taken.Add(drawn.Name);
            result[i] = new SettlementNaming(drawn.Name, race, drawn.Site);
        }
        return result;
    }
}
