using Gameworld.Domain;
using Gameworld.Generation;
using Gameworld.Generation.Cells;
using Gameworld.Generation.Factions;
using Gameworld.Generation.Lakes;
using Gameworld.Generation.Names;
using Gameworld.Generation.Stages;
using Gameworld.Generation.Travel;

namespace Gameworld.Demo;

public sealed partial class WorldHost
{
    /// <summary>
    /// The domain data behind a point: its region, its zone and its patch, each with what the stages the region has
    /// reached say about it. Looking takes the region to Detailed, no further, unless <paramref name="advanceTo"/>
    /// asks for a later stage.
    /// </summary>
    public PointInfo Inspect(WorldPoint point, RegionStage? advanceTo = null)
    {
        lock (_gate) return new Inspection(_world, point, advanceTo).Build();
    }
}

/// <summary>One look at a point of the world: it reads the region of the point and builds what the browser shows.</summary>
internal sealed class Inspection
{
    /// <summary>A river this large, in patches drained, is navigable (map-generation.md section 15).</summary>
    private const double NavigablePatches = 8;

    private readonly World _world;
    private readonly CellMap _cells;
    private readonly WorldPoint _point;
    private readonly CellInfo _info;
    private readonly RegionId _regionId;
    private readonly RegionOutline _outline;
    private readonly DetailWork _detail;
    private readonly ElevationWork? _elevation;
    private readonly PlanWork? _plan;
    private readonly SettlementWork? _settled;
    private readonly RegionHouseholds? _households;
    private readonly RegionPeople? _people;
    private readonly double _patchKm2;
    /// <summary>The place of a land patch among the Elevated patches of the region, which is the order the persons of the patches are in.</summary>
    private readonly Dictionary<long, int> _finalIndex = new();

    public Inspection(World world, WorldPoint point, RegionStage? advanceTo)
    {
        _world = world;
        _cells = world.Cells;
        _point = point;
        _info = _cells.CellAt(point);
        _regionId = new RegionId(_info.Region);
        if (advanceTo is { } stage) world.Advance(_regionId, stage);
        _detail = world.DetailOf(_regionId);
        _outline = world.Advance(_regionId, RegionStage.Outlined).Outline;
        _elevation = world.FindElevation(_regionId);
        _plan = world.FindPlan(_regionId);
        _settled = world.FindSettlement(_regionId);
        _households = world.FindHouseholds(_regionId);
        _people = world.FindPeople(_regionId);
        var km = _cells.PatchSize / 1000;
        _patchKm2 = km * km;
        if (_elevation != null)
            for (var i = 0; i < _elevation.Patches.Length; i++) _finalIndex[_elevation.Patches[i].Patch.Root.Key] = i;
    }

    public PointInfo Build()
    {
        var patch = _detail.Find(_info.Patch);
        return new PointInfo(
            _point,
            _elevation?.Heights.Map.Sample(_point),
            new FieldValues(
                _world.Fields.Sample(FieldKind.InitialTemperature, _point),
                _world.Fields.Sample(FieldKind.Precipitation, _point),
                _world.Fields.Sample(FieldKind.Prosperity, _point),
                _world.Fields.Sample(FieldKind.Supernatural, _point)),
            _info.Ocean,
            Region(),
            Zone(),
            patch is null ? null : Patch(patch));
    }

    // ───────────────────────── Region ─────────────────────────

    private RegionInfo Region()
    {
        var coord = _regionId.Coord;
        var nodes = _detail.Rivers.Nodes;
        var systems = new HashSet<int>();
        for (var k = 0; k < nodes.Count; k++)
            if (_detail.Counts(k)) systems.Add(nodes[k].Tree);

        var land = Summarise(_detail.Patches, _detail.Lakes);
        return new RegionInfo(
            _regionId,
            _world.StageOf(_regionId),
            _outline.HasSeaAccess,
            _cells.Regions.Site(coord),
            _cells.OwnedZoneCells(coord).Length,
            _cells.OceanSeeds(coord).Length,
            _cells.ZoneMergerCount(coord),
            _outline.LandZones.Count(),
            _outline.OceanZones.Count(),
            land,
            systems.Count,
            _detail.Rivers.Rivers.CrossesNeighbours,
            _plan is null ? null : ProspectsOf(Enumerable.Range(0, _plan.Prospects.Length), _plan.ScoreSum),
            RegionPopulation(land.LandKm2),
            Factions(),
            Reading(),
            Settled(),
            _plan?.Naming?.Name,
            _plan?.Naming?.RulingHouse);
    }

    private ProspectsInfo ProspectsOf(IEnumerable<int> slots, double? scoreSum)
    {
        var lines = slots.Select(ProspectLine).OrderByDescending(p => p.Score).ThenBy(p => p.Slot).ToArray();
        return new ProspectsInfo(
            ProspectCount.Of(lines.Select(l => _plan!.Prospects[l.Slot])),
            scoreSum,
            lines.Count(l => l.Class == ProspectClass.Rich),
            lines.Count(l => l.Class == ProspectClass.Average),
            lines.Count(l => l.Class == ProspectClass.Poor),
            lines);
    }

    private ProspectLine ProspectLine(int slot)
    {
        var p = _plan!.Prospects[slot];
        var naming = _plan.Names[slot];
        return new ProspectLine(
            slot, naming.Name, p.Patch, p.Spot, p.ForCity, p.Score, p.Class, p.OnShow,
            new ZoneId(new ZoneCellId(ZoneOf(p.Patch))),
            _plan.ProspectRaces[slot].Select(RaceName).ToArray(),
            naming.Race is null ? null : RaceName(naming.Race),
            naming.Site,
            p.Patch.Coord == _info.Patch);
    }

    /// <summary>The zone of a patch of the region; the zone under the patch's site for a patch the region does not hold.</summary>
    private GridCoord ZoneOf(PatchId patch) =>
        _detail.Find(patch.Coord)?.Shape.Zone ?? _cells.InfoOf(patch.Coord).Zone;

    private PopulationInfo? RegionPopulation(double landKm2)
    {
        if (_plan is null) return null;
        var total = (double)_plan.Population.Total;
        return new PopulationInfo(
            total,
            landKm2 > 0 ? total / landKm2 : 0,
            null,
            _plan.Population.Races
                .Where(r => r.Standing != RaceStanding.Absent)
                .OrderByDescending(r => r.Persons)
                .Select(r => new RaceLine(RaceName(r.Race), r.Persons, r.Standing))
                .ToArray());
    }

    /// <summary>The persons of a set of land patches, by the shares the Planned stage gave every patch. Null until the region is Planned, and for the sea.</summary>
    private PopulationInfo? PopulationOf(IEnumerable<PatchDetail> patches)
    {
        if (_plan is null || _elevation is null) return null;
        var races = _plan.Races;
        var persons = new double[races.Races.Length];
        double area = 0, capacity = 0;
        foreach (var patch in patches)
        {
            if (!_finalIndex.TryGetValue(patch.Root.Key, out var i)) continue;
            var km2 = patch.Shape.Cells.Length * _patchKm2;
            area += km2;
            capacity += _world.PopulationRules.Capacity(km2, _elevation.Patches[i].Resources, patch.Prosperity);
            if (i >= races.PatchPersons.Length) continue;
            var onPatch = races.PatchPersons[i];
            for (var r = 0; r < persons.Length && r < onPatch.Length; r++) persons[r] += onPatch[r];
        }
        if (!(area > 0)) return null;
        var total = persons.Sum();
        return new PopulationInfo(
            total,
            total / area,
            capacity,
            races.Races
                .Select((race, r) => new RaceLine(
                    _world.PopulationRules.Races[race].Name, persons[r],
                    r < races.MainCount ? RaceStanding.Main : RaceStanding.Minority))
                .Where(l => l.Persons > 0)
                .OrderByDescending(l => l.Persons)
                .ToArray());
    }

    private FactionInfo[]? Factions() => _plan?.Factions.Factions
        .Select(f => new FactionInfo(
            f.Slot, f.Role, f.Type, f.Form, RaceName(f.LeadRace),
            f.Kind is null ? null : FactionRules.KindName(f.Kind),
            f.Vocations.Select(VocationName).ToArray(),
            f.Ground, f.Grip, f.Seat,
            f.Seat is { } seat ? SpotOf(seat) : null,
            f.Seat is { } here && here.Coord == _info.Patch,
            Colour(f.MainColour), Colour(f.SecondaryColour)))
        .ToArray();

    /// <summary>How the factions read the region: the tension and its terms, the fit of every type as the ruler and the indices.</summary>
    private FactionReadingInfo? Reading()
    {
        if (_plan?.Factions is not { Reading: { } r } work) return null;
        var t = work.Terms;
        return new FactionReadingInfo(
            work.Tension,
            [
                new("Contest", t.Contest), new("Rough land", t.RoughLand), new("Rival seat", t.RivalSeat),
                new("Precious finds", t.PreciousFinds), new("Poor land", t.PoorLand),
            ],
            Enum.GetValues<FactionType>()
                .Where(type => (int)type < work.Fits.Length)
                .Select(type => new NamedValue(type.ToString(), work.Fits[(int)type]))
                .ToArray(),
            [
                new("Grain", r.Grain), new("Herd", r.Herd), new("Open", r.Open), new("Forest", r.Forest),
                new("Cold", r.Cold), new("Dry", r.Dry), new("Rough", r.Rough), new("Trade", r.Trade),
                new("Wealth", r.Wealth), new("Urban", r.Urban), new("Ore", r.Ore), new("Precious", r.Precious),
                new("Arcane", r.Arcane), new("People", r.People), new("Small", r.Small), new("Mixed", r.Mixed),
                new("Poor", r.Poor), new("Uneven", r.Uneven),
            ],
            r.Harbours,
            r.BestTradeFactor,
            r.HasCity);
    }

    private SettledInfo? Settled()
    {
        if (_settled is null) return null;
        var places = _settled.Places;
        return new SettledInfo(
            places.Settlements.Count(s => s.Kind == SettlementKind.City), places.CityPersons,
            places.Settlements.Count(s => s.Kind == SettlementKind.Town), places.TownPersons,
            places.RuralPersons,
            places.Settlements.Select(PlaceLine).ToArray(),
            _settled.Links.Count(l => l.Kind == TravelLinkKind.Road),
            _settled.Links.Count(l => l.Kind == TravelLinkKind.RiverRoute),
            _settled.Links.Count(l => l.Kind == TravelLinkKind.SeaLane),
            _households?.Households.Count,
            _people?.Persons.Count);
    }

    private PlaceLine PlaceLine(Settlement s) => new(
        s.Slot, s.Name, s.Kind, s.Capital, s.Rank, s.Persons, s.Patch, SpotOf(s.Patch),
        new ZoneId(new ZoneCellId(ZoneOf(s.Patch))),
        s.Races.Select(r => new RaceLine(RaceName(r.Race), r.Persons, RaceStanding.Main)).ToArray(),
        s.Patch.Coord == _info.Patch,
        _households?.In(s).Count());

    /// <summary>The households of a city or a town, with their persons once the region is Peopled. Null until the region is Housed.</summary>
    private HouseholdInfo[]? HouseholdsOf(Settlement place) => _households?.In(place).Select(h =>
    {
        var faction = h.Faction is { } slot ? _plan?.Factions.Factions.FirstOrDefault(f => f.Slot == slot) : null;
        var first = h.Kin is { } kin ? _households.Households[kin.Household] : null;
        return new HouseholdInfo(
            h.Slot, h.Surname, RaceName(h.Race), h.Wealth,
            faction is null ? null : new HouseholdFaction(
                faction.Slot, faction.Role, faction.Kind is null ? faction.Form.ToString() : FactionRules.KindName(faction.Kind)),
            h.Family, h.ServantCount, h.HeadSex, h.HeadAge,
            first is null ? null : new HouseholdKinInfo(
                first.Slot, h.Kin!.Relation, _settled?.Places.Settlements.FirstOrDefault(s => s.Slot == first.Settlement)?.Name),
            _people?.Of(h).Select(p => new PersonLine(
                p.Slot, p.GivenName, p.Surname, RaceName(p.Race), p.Sex, p.Age, p.Role,
                p.Vocation is null ? null : VocationName(p.Vocation), LooksOf(p.Looks))).ToArray());
    }).ToArray();

    /// <summary>The looks of a person, by the names of its portrait components. Null for a person without looks.</summary>
    private PersonLooks? LooksOf(Looks? looks)
    {
        if (looks is null) return null;
        var p = _world.Parameters;
        return new PersonLooks(
            p.SkinColours.FirstOrDefault(c => c.Id == looks.SkinColour)?.Name,
            p.EyeColours.FirstOrDefault(c => c.Id == looks.EyeColour)?.Name,
            p.HairColours.FirstOrDefault(c => c.Id == looks.HairColour)?.Name,
            looks.NaturalHairColour == looks.HairColour ? null : p.HairColours.FirstOrDefault(c => c.Id == looks.NaturalHairColour)?.Name,
            p.Hairstyles.FirstOrDefault(h => h.Id == looks.Hairstyle)?.Name,
            p.Outfits.FirstOrDefault(o => o.Id == looks.Outfit)?.Name,
            Colour(looks.OutfitMainColour), Colour(looks.OutfitAccentColour),
            looks.Accessories.Select(id => p.Accessories.FirstOrDefault(a => a.Id == id)?.Name ?? id).ToArray());
    }

    // ───────────────────────── Zone ─────────────────────────

    private ZoneInfo Zone()
    {
        var root = _info.Zone;
        var zone = _outline.Zones.FirstOrDefault(z => z.Id.Coord == root);
        var patches = _detail.Patches.Where(p => p.Shape.Zone == root).ToArray();
        var roots = patches.Select(p => p.Root.Key).ToHashSet();
        var bedrock = _cells.ZoneBedrock(root);
        var site = _cells.Zones.Site(root);

        var prospects = _plan is null || _info.Ocean
            ? null
            : ProspectsOf(
                Enumerable.Range(0, _plan.Prospects.Length).Where(i => roots.Contains(_plan.Prospects[i].Patch.Coord.Key)),
                null);

        return new ZoneInfo(
            new ZoneId(new ZoneCellId(root)),
            new RegionId(_cells.RegionOfZone(root)),
            _cells.ZoneType(root),
            _info.Ocean,
            bedrock,
            _world.Parameters.Bedrocks.FirstOrDefault(b => b.Id == bedrock)?.Name,
            site,
            zone?.Cells.Count ?? 1,
            OceanOrigin(root),
            !_info.Ocean && _world.IsRiverPoorZone(root),
            _world.Fields.Precipitation(site),
            Summarise(patches, _detail.Lakes.Where(l => roots.Contains(l.Patch.Key))),
            prospects,
            PopulationOf(patches),
            _settled?.Places.Settlements.Where(s => roots.Contains(s.Patch.Coord.Key)).Select(PlaceLine).ToArray());
    }

    /// <summary>How a zone cell became ocean: as a seed, on a link between two seeds, or by the expansion from a link. Null for land.</summary>
    private string? OceanOrigin(GridCoord cell)
    {
        if (!_cells.IsOceanCell(cell)) return null;
        if (_cells.IsOceanSeed(cell)) return "Ocean seed";
        if (_cells.IsLinked(cell)) return "Ocean link";
        for (var pass = 1; pass <= _cells.ExpansionPasses; pass++)
            if (_cells.OceanAfter(cell, pass))
                return _cells.ExpansionPasses > 1 ? $"Ocean expansion, pass {pass}" : "Ocean expansion";
        return "Ocean";
    }

    // ───────────────────────── Patch ─────────────────────────

    private PatchInfo Patch(PatchDetail patch)
    {
        var final = _elevation?.Find(patch.Root);
        var score = _plan?.Scores.FirstOrDefault(s => s.Patch.Root == patch.Root);
        var slot = _plan is null ? -1 : Array.FindIndex(_plan.Prospects, p => p.Patch.Coord == patch.Root);
        var place = _settled?.Places.Settlements.FirstOrDefault(s => s.Patch.Coord == patch.Root);
        var shape = patch.Shape;
        var water = patch.Water;

        var resources = ResourceCatalog.All
            .Select(r => (Resource: r, Estimated: patch.Resources[r], Final: final?.Resources[r]))
            .Where(x => x.Estimated.Amount >= 0.005 || x.Final?.Amount >= 0.005)
            .OrderByDescending(x => x.Estimated.Amount)
            .ThenBy(x => x.Resource)
            .Select(x => new ResourceInfo(
                x.Resource, ResourceCatalog.AbbreviationOf(x.Resource), ResourceCatalog.GroupOf(x.Resource),
                x.Estimated.Amount, x.Estimated.Variant ?? x.Final?.Variant, x.Estimated.Origin,
                x.Estimated.IsListed, x.Final?.Amount))
            .ToArray();

        var biome = final?.Biome ?? patch.Biome;
        return new PatchInfo(
            new PatchId(new PatchCellId(patch.Root)),
            shape.Cells.Length,
            shape.Cells.Length * _patchKm2,
            shape.Ocean,
            shape.Coastal,
            shape.Site,
            shape.Neighbours.Length,
            patch.Biome,
            final?.Biome,
            biome is null ? null : MapColours.Of(biome.Biome),
            final?.MeanHeightM,
            new FieldValues(
                patch.SeaLevelTemperatureC,
                patch.FieldPrecipitationMm,
                _world.Fields.Sample(FieldKind.Prosperity, shape.Site),
                patch.Supernatural),
            shape.Ocean
                ? null
                : new PatchWater(water.River, water.Mouth, water.LakeAreaM2 / 1e6, water.Harbour, water.GoldLoad, water.TinLoad, patch.RiverNodes.Count),
            resources,
            patch.Resources.Food,
            patch.Resources.Wealth,
            final?.Resources.Food,
            final?.Resources.Wealth,
            (final?.Flora ?? patch.Flora).Select(FloraKind).ToArray(),
            (final?.Fauna ?? patch.Fauna).Select(FaunaKind).ToArray(),
            score is null
                ? null
                : new PatchScoreInfo(
                    score.Score, score.Factors, score.Facts, score.OceanFraction, score.Border, score.Centrality,
                    1 + _plan!.Scores.Count(s => s.Score > score.Score), _plan.Scores.Length),
            slot < 0 ? null : ProspectLine(slot),
            place is null ? null : PlaceLine(place),
            shape.Ocean ? null : PopulationOf([patch]),
            place is null ? null : HouseholdsOf(place));
    }

    // ───────────────────────── A set of patches ─────────────────────────

    /// <summary>
    /// What a set of patches holds together, by area: a region or a zone. The biomes, the resources and the heights
    /// are the final ones where the region is Elevated, else the estimated ones. An all-ocean set gives the
    /// resources of the sea.
    /// </summary>
    private LandSummary Summarise(IReadOnlyList<PatchDetail> patches, IEnumerable<LakeShape> lakes)
    {
        double land = 0, ocean = 0;
        int landCount = 0, oceanCount = 0, coastal = 0, withRiver = 0, navigable = 0, harbours = 0;
        double largestRiver = 0, largestMouth = 0;
        var types = new AreaSum<ElevationType>();
        var bedrocks = new AreaSum<string>();
        var biomes = new AreaSum<Biome>();
        SpreadSum temperature = new(), precipitation = new(), prosperity = new(), supernatural = new(), heights = new();
        var resources = new ResourceSum();
        var anyLand = patches.Any(p => !p.Shape.Ocean);

        foreach (var patch in patches)
        {
            var area = patch.Shape.Cells.Length * _patchKm2;
            var final = _elevation?.Find(patch.Root);
            // The resources of the land, or of the sea when the set holds no land.
            if (patch.Shape.Ocean != anyLand) resources.Add(final?.Resources ?? patch.Resources, area);
            if (patch.Shape.Ocean)
            {
                ocean += area;
                oceanCount++;
                continue;
            }

            land += area;
            landCount++;
            if (patch.Shape.Coastal) coastal++;
            types.Add(patch.ZoneType, area);
            if (patch.Bedrock is { } bedrock) bedrocks.Add(bedrock, area);
            if ((final?.Biome ?? patch.Biome) is { } biome)
            {
                biomes.Add(biome.Biome, area);
                temperature.Add(biome.TemperatureC, area);
                precipitation.Add(biome.PrecipitationMm, area);
            }
            prosperity.Add(_world.Fields.Sample(FieldKind.Prosperity, patch.Shape.Site), area);
            supernatural.Add(patch.Supernatural, area);
            if (final?.MeanHeightM is { } height) heights.Add(height, area);

            var water = patch.Water;
            if (water.River > 0) withRiver++;
            if (water.River >= NavigablePatches) navigable++;
            if (water.Harbour) harbours++;
            largestRiver = Math.Max(largestRiver, water.River);
            largestMouth = Math.Max(largestMouth, water.Mouth);
        }

        var lakeList = lakes.ToArray();
        return new LandSummary(
            land, ocean, landCount, oceanCount, coastal,
            _elevation != null,
            types.Shares(land, t => t.ToString(), MapColours.Of, inKeyOrder: true),
            bedrocks.Shares(land, BedrockName),
            biomes.Shares(land, b => b.ToString(), MapColours.Of),
            temperature.ToSpread(),
            precipitation.ToSpread(),
            prosperity.Mean,
            supernatural.Mean,
            heights.ToSpread(),
            new WaterSummary(withRiver, navigable, largestRiver, largestMouth, lakeList.Length, lakeList.Sum(l => l.AreaM2) / 1e6, harbours),
            resources.ToArray(),
            resources.Food,
            resources.Wealth);
    }

    // ───────────────────────── Names ─────────────────────────

    private string RaceName(string id) => _world.Parameters.Races.FirstOrDefault(x => x.Id == id)?.Name ?? id;

    private string VocationName(string id) => _world.Parameters.Vocations.FirstOrDefault(x => x.Id == id)?.Name ?? id;

    private string BedrockName(string id) => _world.Parameters.Bedrocks.FirstOrDefault(x => x.Id == id)?.Name ?? id;

    private KindInfo FloraKind(string id) =>
        _world.Parameters.Flora.FirstOrDefault(x => x.Id == id) is { } kind ? new(kind.Name, kind.Type.ToString()) : new(id, "");

    private KindInfo FaunaKind(string id) =>
        _world.Parameters.Fauna.FirstOrDefault(x => x.Id == id) is { } kind ? new(kind.Name, kind.Type.ToString()) : new(id, "");

    private ColourInfo? Colour(string? id) =>
        _world.Parameters.Colours.FirstOrDefault(x => x.Id == id) is { } colour ? new(colour.Name, colour.Hex) : null;

    /// <summary>Where the Prospect of a patch is placed. Null for a patch without one.</summary>
    private WorldPoint? SpotOf(PatchId patch) => _plan?.Prospects.FirstOrDefault(p => p.Patch == patch)?.Spot;
}

/// <summary>What lies under a point of the world.</summary>
/// <param name="HeightM">The height at the point itself. Null until the region is Elevated.</param>
/// <param name="Fields">The four Fields sampled at the point itself, not at the site of its patch.</param>
/// <param name="Patch">Null only for a point whose patch the region does not hold.</param>
public sealed record PointInfo(
    WorldPoint Point,
    double? HeightM,
    FieldValues Fields,
    bool Ocean,
    RegionInfo Region,
    ZoneInfo Zone,
    PatchInfo? Patch);

/// <summary>The four Fields at one place.</summary>
public sealed record FieldValues(double InitialTemperatureC, double PrecipitationMm, double Prosperity, double Supernatural);

/// <summary>A region: what it was Sited with, its zones and patches, and what each later stage it has reached added.</summary>
/// <param name="ZoneCells">The zone cells the region owns, before the mergers.</param>
/// <param name="OceanSeeds">How many of them the region chose as ocean seeds.</param>
/// <param name="ZoneMergers">The determined number of zone mergers of the region.</param>
/// <param name="RiverSystems">The river trees with a node on the land of the region.</param>
/// <param name="RiversCrossNeighbours">The region is landlocked: its rivers cross the land of a neighbouring region to reach an ocean.</param>
/// <param name="Prospects">Null until the region is Planned.</param>
/// <param name="Population">The potential population. Null until the region is Planned.</param>
/// <param name="Factions">By slot: the ruling faction, the opposition when there is one, then the lesser factions. Null until the region is Planned.</param>
/// <param name="Reading">How the factions read the region. Null until the region is Planned, and in a region without persons.</param>
/// <param name="Settled">The cities and towns. Null until the region is Settled.</param>
/// <param name="Name">The name of the region. Null until the region is Planned, and in a region without a ruling faction.</param>
/// <param name="RulingHouse">The surname of the ruling house. Null when the ruling faction is not of the type Family.</param>
public sealed record RegionInfo(
    RegionId Id,
    RegionStage Stage,
    bool HasSeaAccess,
    WorldPoint Site,
    int ZoneCells,
    int OceanSeeds,
    int ZoneMergers,
    int LandZones,
    int OceanZones,
    LandSummary Land,
    int RiverSystems,
    bool RiversCrossNeighbours,
    ProspectsInfo? Prospects,
    PopulationInfo? Population,
    FactionInfo[]? Factions,
    FactionReadingInfo? Reading,
    SettledInfo? Settled,
    string? Name,
    string? RulingHouse);

/// <summary>A zone, with what its patches hold together.</summary>
/// <param name="Region">The region that owns the root cell of the zone.</param>
/// <param name="Cells">The zone cells that have merged into the zone.</param>
/// <param name="OceanOrigin">How the root cell became ocean. Null for land.</param>
/// <param name="SitePrecipitationMm">The Precipitation field at the site of the root cell, which decides whether a flat zone is river-poor.</param>
/// <param name="Prospects">Null for an ocean zone and until the region is Planned.</param>
/// <param name="Population">The potential population on the land of the zone. Null for an ocean zone and until the region is Planned.</param>
/// <param name="Places">The cities and towns in the zone. Null until the region is Settled.</param>
public sealed record ZoneInfo(
    ZoneId Id,
    RegionId Region,
    ElevationType ElevationType,
    bool Ocean,
    string? Bedrock,
    string? BedrockName,
    WorldPoint Site,
    int Cells,
    string? OceanOrigin,
    bool RiverPoor,
    double SitePrecipitationMm,
    LandSummary Land,
    ProspectsInfo? Prospects,
    PopulationInfo? Population,
    PlaceLine[]? Places);

/// <summary>A patch.</summary>
/// <param name="BiomeColour">The colour the map draws the biome in: of the final biome when there is one.</param>
/// <param name="SiteFields">The four Fields at the site of the patch, which its climate and resources are calculated from.</param>
/// <param name="Water">Null for an ocean patch.</param>
/// <param name="Resources">Every resource the patch holds any of, the largest estimated amount first.</param>
/// <param name="Flora">The kinds of flora of the patch: of the final biome when the region is Elevated, else of the estimated biome.</param>
/// <param name="Score">Null for an ocean patch and until the region is Planned.</param>
/// <param name="Prospect">Null when the patch has no Prospect.</param>
/// <param name="Settlement">The city or town on the Prospect of the patch. Null when it has none, and until the region is Settled.</param>
/// <param name="Population">The potential population on the patch. Null for an ocean patch and until the region is Planned.</param>
/// <param name="Households">The households of the city or town on the patch. Null when it has none, and until the region is Housed.</param>
public sealed record PatchInfo(
    PatchId Id,
    int Cells,
    double AreaKm2,
    bool Ocean,
    bool Coastal,
    WorldPoint Site,
    int Neighbours,
    BiomeRecord? EstimatedBiome,
    BiomeRecord? FinalBiome,
    string? BiomeColour,
    double? MeanHeightM,
    FieldValues SiteFields,
    PatchWater? Water,
    ResourceInfo[] Resources,
    double Food,
    double Wealth,
    double? FinalFood,
    double? FinalWealth,
    KindInfo[] Flora,
    KindInfo[] Fauna,
    PatchScoreInfo? Score,
    ProspectLine? Prospect,
    PlaceLine? Settlement,
    PopulationInfo? Population,
    HouseholdInfo[]? Households);

/// <summary>What a set of patches holds together, by area.</summary>
/// <param name="CoastalPatches">The land patches with a patch cell on the coast.</param>
/// <param name="Final">The biomes, heights and resources are the final ones: the region is Elevated.</param>
/// <param name="ElevationTypes">The land by the elevation type of its zone, in the order of the types.</param>
/// <param name="TemperatureC">The temperature of the land patches, cooled by their height. Null without land.</param>
/// <param name="HeightM">The mean heights of the land patches. Null until the region is Elevated.</param>
/// <param name="Resources">The resources of the land, or of the sea when the set holds no land.</param>
public sealed record LandSummary(
    double LandKm2,
    double OceanKm2,
    int LandPatches,
    int OceanPatches,
    int CoastalPatches,
    bool Final,
    AreaShare[] ElevationTypes,
    AreaShare[] Bedrocks,
    AreaShare[] Biomes,
    ValueSpread? TemperatureC,
    ValueSpread? PrecipitationMm,
    double? Prosperity,
    double? Supernatural,
    ValueSpread? HeightM,
    WaterSummary Water,
    ResourceSummary[] Resources,
    double Food,
    double Wealth);

/// <summary>The fresh water of a set of land patches. River sizes are in patches drained.</summary>
/// <param name="NavigablePatches">The patches with a river of 8 patches drained or more.</param>
public sealed record WaterSummary(
    int RiverPatches, int NavigablePatches, double LargestRiver, double LargestMouth, int Lakes, double LakeKm2, int Harbours);

/// <summary>The water site of a land patch. River sizes are in patches drained.</summary>
/// <param name="GoldLoad">The share of the land upstream of the largest river that lies in hills or mountains on gold-bearing bedrock.</param>
/// <param name="TinLoad">The same share for granite.</param>
/// <param name="RiverNodes">The counting river nodes on the patch.</param>
public sealed record PatchWater(double River, double Mouth, double LakeKm2, bool Harbour, double GoldLoad, double TinLoad, int RiverNodes);

/// <param name="Amount">The estimated amount, which the labels on the map and the Prospects read.</param>
/// <param name="FinalAmount">The amount with the height from the height map. Null until the region is Elevated.</param>
public sealed record ResourceInfo(
    Resource Resource, string Abbreviation, ResourceGroup Group, double Amount, string? Variant, ResourceOrigin Origin,
    bool Listed, double? FinalAmount);

/// <summary>A kind of flora or fauna: its name and its type.</summary>
public sealed record KindInfo(string Name, string Type);

/// <summary>The score of a land patch as a place to settle, with everything behind it.</summary>
/// <param name="OceanFraction">The share of ocean patches among the neighbours.</param>
/// <param name="Border">The patch touches the land of another region.</param>
/// <param name="Rank">Its place among the land patches of the region by score, from 1.</param>
public sealed record PatchScoreInfo(
    double Score, ProspectFactors Factors, ProspectFacts Facts, double OceanFraction, bool Border, double Centrality, int Rank, int Of);

/// <summary>The Prospects of a region or a zone, the best score first.</summary>
/// <param name="ScoreSum">The sum of the scores of the region's Prospects, which decides how many are on show. Null for a zone.</param>
public sealed record ProspectsInfo(ProspectCount Count, double? ScoreSum, int Rich, int Average, int Poor, ProspectLine[] List);

/// <param name="Slot">The index of the Prospect among the Prospects of its region.</param>
/// <param name="Name">The name of a city or a town on the Prospect.</param>
/// <param name="Races">The names of the races of a city or a town on the Prospect, the largest first and the drawn one last.</param>
/// <param name="NamedBy">The name of the race that named the place. Null where the Prospect holds no race.</param>
/// <param name="NameSite">What the place word of the name says about the site. Null for a name without a place word.</param>
/// <param name="Here">The Prospect is on the patch under the pointer.</param>
public sealed record ProspectLine(
    int Slot, string Name, PatchId Patch, WorldPoint Spot, bool ForCity, double Score, ProspectClass Class, bool OnShow, ZoneId Zone,
    string[] Races, string? NamedBy, SiteClass? NameSite, bool Here);

/// <summary>Persons and the races they belong to, the largest race first.</summary>
/// <param name="Capacity">The persons the land can feed before the races share it. Null for a region, where it is the total.</param>
public sealed record PopulationInfo(double Persons, double PerKm2, double? Capacity, RaceLine[] Races);

public sealed record RaceLine(string Name, double Persons, RaceStanding Standing);

/// <summary>A faction of a region, with the names of its lead race, its kind and its vocations.</summary>
/// <param name="Kind">The name of the kind of a lesser faction, and of the trade of a guild in opposition.</param>
/// <param name="Seat">The patch of the Prospect the faction sits on.</param>
/// <param name="SeatSpot">Where that Prospect is placed.</param>
/// <param name="SeatHere">The seat is the patch under the pointer.</param>
/// <param name="MainColour">Its main colour. Null when the Parameter Set has no colours.</param>
/// <param name="SecondaryColour">Its secondary colour. Null when the Parameter Set has fewer than two colours.</param>
public sealed record FactionInfo(
    int Slot, FactionRole Role, FactionType Type, FactionForm Form, string LeadRace, string? Kind,
    string[] Vocations, OppositionGround? Ground, double Grip, PatchId? Seat, WorldPoint? SeatSpot, bool SeatHere,
    ColourInfo? MainColour, ColourInfo? SecondaryColour);

/// <summary>A colour of a faction: its name and its value as #RRGGBB.</summary>
public sealed record ColourInfo(string Name, string Hex);

/// <summary>How the factions read a region (society-generation.md sections 4.3 to 4.5). Every value is 0 to 1.</summary>
/// <param name="Tension">The tension of the region, without its random part.</param>
/// <param name="Fits">The fit of each type of faction as the ruler.</param>
/// <param name="Harbours">The Prospects on show with a harbour or a navigable river mouth.</param>
public sealed record FactionReadingInfo(
    double Tension, NamedValue[] Terms, NamedValue[] Fits, NamedValue[] Indices, int Harbours, double BestTradeFactor, bool HasCity);

/// <summary>The cities and towns of a Settled region, the persons in them and on the land, and the travel links the region decides.</summary>
/// <param name="Households">The households of its cities and towns. Null until the region is Housed.</param>
/// <param name="Persons">The persons of those households. Null until the region is Peopled.</param>
public sealed record SettledInfo(
    int Cities, long CityPersons, int Towns, long TownPersons, long RuralPersons, PlaceLine[] Places,
    int Roads, int RiverRoutes, int SeaLanes, int? Households, int? Persons);

/// <summary>A city or a town.</summary>
/// <param name="Rank">Its rank among the cities and towns of its region, from 1.</param>
/// <param name="Spot">Where its Prospect is placed.</param>
/// <param name="Races">The persons of each of its races, in the order of the races of its Prospect.</param>
/// <param name="Here">It stands on the patch under the pointer.</param>
/// <param name="Households">How many households it holds. Null until the region is Housed.</param>
public sealed record PlaceLine(
    int Slot, string Name, SettlementKind Kind, bool Capital, int Rank, long Persons, PatchId Patch, WorldPoint? Spot, ZoneId Zone,
    RaceLine[] Races, bool Here, int? Households);

/// <summary>A household of a city or a town (society-generation.md section 6).</summary>
/// <param name="Race">The name of the race of the family.</param>
/// <param name="Faction">The faction the household belongs to. Null for a household of no faction.</param>
/// <param name="Family">How many persons the family holds, the head included.</param>
/// <param name="Kin">The household is a branch of a house: how its head is kin of the head of the first household. Null otherwise.</param>
/// <param name="Persons">The persons of the household, the head first. Null until the region is Peopled.</param>
public sealed record HouseholdInfo(
    int Slot, string Surname, string Race, WealthLevel Wealth, HouseholdFaction? Faction, int Family, long Servants,
    Sex HeadSex, int HeadAge, HouseholdKinInfo? Kin, PersonLine[]? Persons);

/// <summary>The faction of a household: its slot, its role and what it is.</summary>
public sealed record HouseholdFaction(int Slot, FactionRole Role, string What);

/// <summary>How the head of a branch household is kin of the head of the first household of its house.</summary>
/// <param name="Household">The slot of the first household of the house.</param>
/// <param name="Relation">Child or Sibling.</param>
/// <param name="Place">The name of the city or town of the first household.</param>
public sealed record HouseholdKinInfo(int Household, HouseholdRole Relation, string? Place);

/// <summary>A person of a household (society-generation.md section 7).</summary>
/// <param name="Race">The name of the race of the person.</param>
/// <param name="Vocation">The name of the vocation. Null for a child and where no vocation fits.</param>
/// <param name="Looks">The looks of the person. Null for a person without looks.</param>
public sealed record PersonLine(
    int Slot, string GivenName, string Surname, string Race, Sex Sex, int Age, HouseholdRole Role, string? Vocation, PersonLooks? Looks);

/// <summary>The looks of a person (society-generation.md section 9), by the names of its portrait components.</summary>
/// <param name="Hair">The name of the hair colour a picture shows. Null for a person without hair.</param>
/// <param name="NaturalHair">The name of the hair colour before it greyed. Null when the hair has not greyed.</param>
/// <param name="OutfitMain">The main colour of the outfit. Null without an outfit.</param>
/// <param name="OutfitAccent">The accent colour of the outfit. Null without an outfit.</param>
public sealed record PersonLooks(
    string? Skin, string? Eyes, string? Hair, string? NaturalHair, string? Hairstyle, string? Outfit,
    ColourInfo? OutfitMain, ColourInfo? OutfitAccent, string[] Accessories);
