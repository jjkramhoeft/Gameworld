namespace Gameworld.Domain;

/// <summary>
/// What the Outlined stage adds: zones linked, expanded and merged, and sea access.
/// </summary>
/// <param name="Zones">The zones of the region: the zones whose root cell the region owns - not the zone cells the region owns. Sorted by key.</param>
/// <param name="HasSeaAccess">An ocean zone touches the land of the region. The ocean zone can belong to a neighbouring region. A region without sea access is landlocked.</param>
public sealed record RegionOutline(IReadOnlyList<Zone> Zones, bool HasSeaAccess)
{
    public IEnumerable<Zone> LandZones => Zones.Where(z => !z.IsOcean);
    public IEnumerable<Zone> OceanZones => Zones.Where(z => z.IsOcean);
}

/// <summary>
/// What the Detailed stage adds: patch mergers, rivers, lakes and the estimated biomes and
/// resources. The estimated form is calculated with a typical height per elevation type
/// instead of the real height.
/// </summary>
/// <param name="Patches">The patches of the region's zones, with their estimated biome and resources and the flora and fauna of the estimated biome. Sorted by key.</param>
public sealed record RegionDetail(IReadOnlyList<Patch> Patches, Rivers Rivers, IReadOnlyList<Lake> Lakes);

/// <summary>The final form of a land patch, calculated with the height from the height map.</summary>
/// <param name="MeanHeightM">The mean height of the patch's land; null when no raster cell centre lies in it.</param>
/// <param name="Flora">The ids of the kinds of flora of the final biome.</param>
/// <param name="Fauna">The ids of the kinds of fauna of the final biome.</param>
public sealed record PatchElevation(
    PatchId Patch,
    double? MeanHeightM,
    BiomeRecord Biome,
    PatchResources Resources,
    IReadOnlyList<string> Flora,
    IReadOnlyList<string> Fauna);

/// <summary>What the Elevated stage adds: the height map and the final biomes, resources, flora and fauna.</summary>
public sealed record RegionElevation(HeightMap HeightMap, IReadOnlyList<PatchElevation> Patches);

/// <summary>How a race stands in a region.</summary>
public enum RaceStanding
{
    /// <summary>The race is not a race of the region: it holds none of its potential population.</summary>
    Absent,
    /// <summary>One of the largest races of the region by the plain shares.</summary>
    Main,
    /// <summary>A race that holds one of the last places of the region, by its turn or by a draw.</summary>
    Minority,
}

/// <summary>The potential population of one race in a region.</summary>
/// <param name="Race">The id of the race in the Parameter Set.</param>
/// <param name="Persons">How many persons of the race the land of the region can feed. 0 for a race that is not a race of the region.</param>
/// <param name="Standing">Whether the race is a main race or a minority race of the region, or not a race of it.</param>
public sealed record RacePopulation(string Race, long Persons, RaceStanding Standing = RaceStanding.Main);

/// <summary>
/// The potential population of a region: how many persons its land can feed, by its food
/// resources and the Prosperity field, shared between the races of the region by their
/// frequency in the biome of each patch and their height modifier at its height. A region holds
/// a few races only: its main races and its minority races. It is an estimate of what the land
/// can carry, not a count of people that exist.
/// </summary>
/// <param name="Races">One entry for every race that is on, in the order of the Parameter Set. A race that is not a race of the region has 0 persons.</param>
public sealed record PotentialPopulation(IReadOnlyList<RacePopulation> Races)
{
    public static PotentialPopulation None { get; } = new([]);

    /// <summary>The potential population of all races together.</summary>
    public long Total => Races.Sum(r => r.Persons);

    /// <summary>The potential population of one race; 0 for a race that is off, not in the Parameter Set or not a race of the region.</summary>
    public long Of(string race) => Races.FirstOrDefault(r => r.Race == race)?.Persons ?? 0;

    /// <summary>The ids of the races of the region, main and minority, in the order of the Parameter Set.</summary>
    public IEnumerable<string> RegionRaces => Races.Where(r => r.Standing != RaceStanding.Absent).Select(r => r.Race);

    /// <summary>The ids of the main races of the region, in the order of the Parameter Set.</summary>
    public IEnumerable<string> MainRaces => Races.Where(r => r.Standing == RaceStanding.Main).Select(r => r.Race);

    /// <summary>The ids of the minority races of the region, in the order of the Parameter Set.</summary>
    public IEnumerable<string> MinorityRaces => Races.Where(r => r.Standing == RaceStanding.Minority).Select(r => r.Race);
}

/// <summary>
/// What the Planned stage adds: the Prospects for cities and towns, the potential population
/// per race, the races a city or a town on each Prospect holds and its name, the factions of
/// the region and the name of the region.
/// </summary>
/// <param name="ProspectRaces">For each Prospect, in the order of <paramref name="Prospects"/>: the ids of the races of a city or a town on it, the largest first and the drawn one last. They are races of the region.</param>
/// <param name="ProspectNames">For each Prospect, in the order of <paramref name="Prospects"/>: the name of a city or a town on it, from the city and town name generator (society-generation.md section 5.3). No two Prospects of a region have the same name.</param>
/// <param name="Factions">The factions of the region, by slot: the ruling faction, the opposition when there is one, then the lesser factions. Empty in a region without persons.</param>
/// <param name="Name">The name of the region, from the region-name generator (society-generation.md section 4.10). Null in a region without a ruling faction.</param>
public sealed record RegionPlan(
    IReadOnlyList<Prospect> Prospects,
    PotentialPopulation Population,
    IReadOnlyList<IReadOnlyList<string>> ProspectRaces,
    IReadOnlyList<string> ProspectNames,
    IReadOnlyList<Faction> Factions,
    string? Name = null)
{
    /// <summary>The ruling faction; null in a region without persons.</summary>
    public Faction? Ruling => Factions.FirstOrDefault(f => f.Role == FactionRole.Ruling);

    /// <summary>The opposition; null when the region has none.</summary>
    public Faction? Opposition => Factions.FirstOrDefault(f => f.Role == FactionRole.Opposition);

    /// <summary>The lesser factions, by slot.</summary>
    public IEnumerable<Faction> Lesser => Factions.Where(f => f.Role == FactionRole.Lesser);
}

/// <summary>What a place of the society is: a city or a town.</summary>
public enum SettlementKind
{
    /// <summary>A town: on a Prospect on show for a town.</summary>
    Town,
    /// <summary>A city: on a Prospect on show for a city, or the capital of the region.</summary>
    City,
}

/// <summary>A number of persons of one race.</summary>
/// <param name="Race">The id of the race in the Parameter Set.</param>
public sealed record RacePersons(string Race, long Persons);

/// <summary>
/// A city or a town as a Stub: an actual settlement on a Prospect on show
/// (society-generation.md section 5.2). Its identity is the region and the slot. Everything in
/// it is derived data.
/// </summary>
/// <param name="Slot">The slot in the region: the index of its Prospect among the Prospects of the region.</param>
/// <param name="Name">Its name: the name of its Prospect (society-generation.md section 5.3). No other city or town of the region has it.</param>
/// <param name="Kind">A city or a town. The capital is a city whatever its Prospect is for.</param>
/// <param name="Capital">The city is the capital of the region: the seat of its ruling faction.</param>
/// <param name="Patch">The patch of its Prospect.</param>
/// <param name="Rank">Its rank among the cities and towns of the region, from 1: the capital, the other cities, then the towns.</param>
/// <param name="Persons">How many persons live in it.</param>
/// <param name="Races">The persons of each of its races, in the order of the races of the Prospect: the largest around it first and the drawn one last. A race of the list can hold nobody in a small place.</param>
public sealed record Settlement(
    int Slot,
    string Name,
    SettlementKind Kind,
    bool Capital,
    PatchId Patch,
    int Rank,
    long Persons,
    IReadOnlyList<RacePersons> Races)
{
    /// <summary>The identity of the city or town: its region and its slot.</summary>
    public SlotId<RegionId> IdIn(RegionId region) => new(region, Slot);
}

/// <summary>
/// What the Settled stage adds: the travel network, the cities and towns of the region and the
/// persons that live on the land between them.
/// </summary>
/// <param name="Settlements">The cities and towns of the region, by rank: one on every Prospect on show.</param>
/// <param name="Rural">The persons on the land, outside the cities and towns: one entry for every race that is on, in the order of the Parameter Set.</param>
/// <remarks>Families and important persons belong to this stage too and are not modelled yet.</remarks>
public sealed record RegionSettlement(
    TravelNetwork Travel,
    IReadOnlyList<Settlement> Settlements,
    IReadOnlyList<RacePersons> Rural)
{
    /// <summary>The capital of the region; null in a region without a ruling faction or without a Prospect on show.</summary>
    public Settlement? Capital => Settlements.FirstOrDefault(s => s.Capital);

    /// <summary>The persons that live in the cities of the region.</summary>
    public long CityPersons => Settlements.Where(s => s.Kind == SettlementKind.City).Sum(s => s.Persons);

    /// <summary>The persons that live in the towns of the region.</summary>
    public long TownPersons => Settlements.Where(s => s.Kind == SettlementKind.Town).Sum(s => s.Persons);

    /// <summary>The persons that live on the land, outside the cities and towns.</summary>
    public long RuralPersons => Rural.Sum(r => r.Persons);
}

/// <summary>
/// A region as it is stored: its stage and what each stage it has reached added. Everything in
/// it is derived data, so the store is only a cache of it.
/// </summary>
public sealed record RegionSnapshot(
    RegionId Id,
    RegionStage Stage,
    RegionOutline Outline,
    RegionDetail? Detail,
    RegionElevation? Elevation,
    RegionPlan? Plan,
    RegionSettlement? Settlement);

/// <summary>
/// A stored region: the generation unit of the map. A region moves through the stages and only
/// advances. Everything it holds is derived data, so it can be evicted and recomputed.
/// </summary>
public sealed class Region
{
    /// <summary>Stored regions start at Outlined.</summary>
    public Region(RegionId id, RegionOutline outline)
    {
        Id = id;
        Outline = outline;
        Stage = RegionStage.Outlined;
    }

    public RegionId Id { get; }
    public RegionStage Stage { get; private set; }
    public RegionOutline Outline { get; }
    public RegionDetail? Detail { get; private set; }
    public RegionElevation? Elevation { get; private set; }
    public RegionPlan? Plan { get; private set; }
    public RegionSettlement? Settlement { get; private set; }

    /// <summary>The region as it is stored.</summary>
    public RegionSnapshot ToSnapshot() => new(Id, Stage, Outline, Detail, Elevation, Plan, Settlement);

    /// <summary>A region read back from storage. The snapshot must hold what every stage up to its own added.</summary>
    public static Region FromSnapshot(RegionSnapshot snapshot)
    {
        var region = new Region(snapshot.Id, snapshot.Outline);
        if (snapshot.Stage >= RegionStage.Detailed) region.Advance(snapshot.Detail ?? throw Missing(snapshot, RegionStage.Detailed));
        if (snapshot.Stage >= RegionStage.Elevated) region.Advance(snapshot.Elevation ?? throw Missing(snapshot, RegionStage.Elevated));
        if (snapshot.Stage >= RegionStage.Planned) region.Advance(snapshot.Plan ?? throw Missing(snapshot, RegionStage.Planned));
        if (snapshot.Stage >= RegionStage.Settled) region.Advance(snapshot.Settlement ?? throw Missing(snapshot, RegionStage.Settled));
        return region;
    }

    private static InvalidOperationException Missing(RegionSnapshot snapshot, RegionStage stage) =>
        new($"The snapshot of region {snapshot.Id} is {snapshot.Stage} but holds no {stage} data.");

    /// <summary>Outlined → Detailed.</summary>
    public void Advance(RegionDetail detail)
    {
        Require(RegionStage.Outlined);
        Detail = detail;
        Stage = RegionStage.Detailed;
    }

    /// <summary>Detailed → Elevated.</summary>
    public void Advance(RegionElevation elevation)
    {
        Require(RegionStage.Detailed);
        Elevation = elevation;
        Stage = RegionStage.Elevated;
    }

    /// <summary>Elevated → Planned.</summary>
    public void Advance(RegionPlan plan)
    {
        Require(RegionStage.Elevated);
        Plan = plan;
        Stage = RegionStage.Planned;
    }

    /// <summary>Planned → Settled.</summary>
    public void Advance(RegionSettlement settlement)
    {
        Require(RegionStage.Planned);
        Settlement = settlement;
        Stage = RegionStage.Settled;
    }

    private void Require(RegionStage stage)
    {
        if (Stage != stage)
            throw new InvalidOperationException(
                $"Region {Id} is {Stage}; only a region that is {stage} can advance to {stage + 1}.");
    }
}
