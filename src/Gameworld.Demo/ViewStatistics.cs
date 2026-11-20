using Gameworld.Domain;
using Gameworld.Generation;
using Gameworld.Generation.Cells;
using Gameworld.Generation.Factions;
using Gameworld.Generation.Stages;

namespace Gameworld.Demo;

public sealed partial class WorldHost
{
    /// <summary>The height bands of the statistics, by their upper bound in metres. The last band is open.</summary>
    private static readonly double[] HeightBandsM = [50, 100, 200, 400, 800, 1600];

    /// <summary>How many kinds of flora, and of fauna, the statistics list.</summary>
    private const int TopKinds = 12;

    /// <summary>
    /// The statistics of a rectangle of the world. The rectangle is sampled on an even lattice of points, so every
    /// share by area is that of the picture the map shows. It takes the regions in the rectangle to Detailed, or to
    /// Elevated when the final biomes are asked for: the biomes, the climate, the resources and the flora and fauna
    /// are then the final ones.
    /// <para>
    /// The Prospects, the potential population and the factions are counted in the regions of the rectangle that
    /// are Planned, each region whole. With <paramref name="plan"/> the regions in the rectangle are taken to
    /// Planned, which costs a height map each; without it only the regions that are Planned already are counted.
    /// The heights are read in the regions that are Elevated already, and the cities and towns in the regions that
    /// are Settled already: the statistics take no region there.
    /// </para>
    /// </summary>
    public ViewStatistics Statistics(double left, double top, double width, double height, bool finalBiomes, bool plan)
    {
        const int MaxSamples = 160;
        var step = Math.Max(width, height) / MaxSamples;
        var nx = Math.Max(1, (int)Math.Round(width / step));
        var ny = Math.Max(1, (int)Math.Round(height / step));
        lock (_gate)
        {
            var world = _world;
            var cells = world.Cells;
            var seen = new Dictionary<long, SampledPatch?>();
            var regions = new Dictionary<long, GridCoord>();
            var landZones = new Dictionary<long, CellInfo>();
            var oceanZones = new HashSet<long>();
            var heights = new SpreadSum();
            var heightBands = new int[HeightBandsM.Length + 1];
            int land = 0, ocean = 0, heightSamples = 0;

            for (var j = 0; j < ny; j++)
                for (var i = 0; i < nx; i++)
                {
                    var point = new WorldPoint(left + (i + 0.5) * width / nx, top + (j + 0.5) * height / ny);
                    var c = cells.CellAt(point);
                    regions[c.Region.Key] = c.Region;
                    if (!seen.TryGetValue(c.Patch.Key, out var patch))
                    {
                        var region = new RegionId(c.Region);
                        var detail = world.DetailOf(region).Find(c.Patch);
                        patch = detail is null
                            ? null
                            : new SampledPatch(detail, finalBiomes ? world.ElevationOf(region).Find(c.Patch) : null);
                        seen[c.Patch.Key] = patch;
                    }
                    if (patch != null) patch.Samples++;
                    if (c.Ocean)
                    {
                        ocean++;
                        oceanZones.Add(c.Zone.Key);
                        continue;
                    }

                    land++;
                    landZones[c.Zone.Key] = c;
                    if (world.FindElevation(new RegionId(c.Region)) is not { } elevation) continue;
                    var h = elevation.Heights.Map.Sample(point);
                    heights.Add(h, 1);
                    heightSamples++;
                    var band = 0;
                    while (band < HeightBandsM.Length && h >= HeightBandsM[band]) band++;
                    heightBands[band]++;
                }

            var sampleKm2 = width / nx * (height / ny) / 1e6;
            var landKm2 = land * sampleKm2;

            // Everything by area: every patch weighs the samples that fell in it.
            var types = new AreaSum<ElevationType>();
            var bedrocks = new AreaSum<string>();
            var biomes = new AreaSum<Biome>();
            var belts = new AreaSum<ThermalBelt>();
            var flora = new AreaSum<string>();
            var fauna = new AreaSum<string>();
            SpreadSum temperature = new(), precipitation = new();
            var resources = new ResourceSum();
            double river = 0, navigable = 0, lake = 0, coastal = 0, harbour = 0;
            int landPatches = 0, oceanPatches = 0, landPatchCells = 0, oceanPatchCells = 0;
            foreach (var patch in seen.Values)
            {
                if (patch is null) continue;
                var detail = patch.Detail;
                var area = patch.Samples * sampleKm2;
                foreach (var kind in patch.Final?.Flora ?? detail.Flora) flora.Add(kind, area);
                foreach (var kind in patch.Final?.Fauna ?? detail.Fauna) fauna.Add(kind, area);
                if (detail.Shape.Ocean)
                {
                    oceanPatches++;
                    oceanPatchCells += detail.Shape.Cells.Length;
                    continue;
                }

                landPatches++;
                landPatchCells += detail.Shape.Cells.Length;
                types.Add(detail.ZoneType, area);
                if (detail.Bedrock is { } bedrock) bedrocks.Add(bedrock, area);
                if ((patch.Final?.Biome ?? detail.Biome) is { } biome)
                {
                    biomes.Add(biome.Biome, area);
                    belts.Add(biome.Belt, area);
                    temperature.Add(biome.TemperatureC, area);
                    precipitation.Add(biome.PrecipitationMm, area);
                }
                resources.Add(patch.Final?.Resources ?? detail.Resources, area);
                var water = detail.Water;
                if (water.River > 0) river += area;
                if (water.River >= 8) navigable += area;
                if (water.HasLake) lake += area;
                if (water.Harbour) harbour += area;
                if (detail.Shape.Coastal) coastal += area;
            }

            // The zone cells of the zones in view, from the outline of their regions.
            var zoneCells = new Dictionary<long, int>();
            foreach (var region in regions.Values)
                foreach (var zone in world.Advance(new RegionId(region), RegionStage.Outlined).Outline.Zones)
                    zoneCells[zone.Id.Coord.Key] = zone.Cells.Count;
            var zonesInView = landZones.Keys.Concat(oceanZones).ToArray();
            double Share(double area) => landKm2 > 0 ? area / landKm2 : 0;
            string BedrockName(string id) => world.Parameters.Bedrocks.FirstOrDefault(b => b.Id == id)?.Name ?? id;

            var regionIds = regions.Values.OrderBy(r => r.Key).Select(r => new RegionId(r)).ToArray();
            var plans = regionIds
                .Select(id => (Id: id, Plan: plan ? world.PlanOf(id) : world.FindPlan(id)))
                .Where(p => p.Plan != null)
                .Select(p => (p.Id, Plan: p.Plan!))
                .ToArray();

            return new ViewStatistics(
                finalBiomes,
                regionIds.Length,
                regionIds.GroupBy(world.StageOf).OrderBy(g => g.Key).Select(g => new NamedCount(g.Key.ToString(), g.Count())).ToArray(),
                landKm2,
                ocean * sampleKm2,
                new CellStatistics(
                    landZones.Count, oceanZones.Count,
                    zonesInView.Length > 0 ? zonesInView.Average(z => (double)zoneCells.GetValueOrDefault(z, 1)) : 0,
                    landPatches, oceanPatches,
                    landPatches > 0 ? (double)landPatchCells / landPatches : 0,
                    oceanPatches > 0 ? (double)oceanPatchCells / oceanPatches : 0),
                types.Shares(landKm2, t => t.ToString(), MapColours.Of, inKeyOrder: true),
                bedrocks.Shares(landKm2, BedrockName),
                biomes.Shares(landKm2, b => b.ToString(), MapColours.Of),
                belts.Shares(landKm2, b => b.ToString(), inKeyOrder: true),
                temperature.ToSpread(),
                precipitation.ToSpread(),
                heightSamples == 0
                    ? null
                    : new HeightStatistics(
                        heights.ToSpread()!,
                        (double)heightSamples / land,
                        heightBands.Select((n, k) => new AreaShare(
                            k == HeightBandsM.Length ? $"{HeightBandsM[^1]:0} m and up"
                                : $"{(k == 0 ? 0 : HeightBandsM[k - 1]):0} to {HeightBandsM[k]:0} m",
                            null, n * sampleKm2, (double)n / heightSamples)).ToArray()),
                land == 0 ? null : new WaterStatistics(Share(river), Share(navigable), Share(lake), Share(coastal), Share(harbour)),
                resources.ToArray(),
                resources.Food,
                resources.Wealth,
                Kinds(flora, (land + ocean) * sampleKm2,
                    world.Parameters.Flora.Where(k => k.Enabled).Select(k => (k.Id, k.Name, Type: k.Type.ToString())).ToArray()),
                Kinds(fauna, (land + ocean) * sampleKm2,
                    world.Parameters.Fauna.Where(k => k.Enabled).Select(k => (k.Id, k.Name, Type: k.Type.ToString())).ToArray()),
                CountProspects(world, plans.Select(p => p.Plan).ToArray(), landZones.Values),
                Population(world, plans),
                Factions(plans.Select(p => p.Plan).ToArray()),
                Settlements(world, regionIds));
        }
    }

    /// <summary>A patch with a sample of the view in it.</summary>
    private sealed class SampledPatch(PatchDetail detail, PatchFinal? final)
    {
        public PatchDetail Detail { get; } = detail;
        /// <summary>The final form of a land patch, when the statistics are those of the final biomes.</summary>
        public PatchFinal? Final { get; } = final;
        public int Samples { get; set; }
    }

    /// <summary>The kinds of flora or of fauna in view: how many of the kinds that are on show up, by type, and the most widespread ones.</summary>
    private static KindStatistics Kinds(AreaSum<string> found, double viewKm2, (string Id, string Name, string Type)[] enabled)
    {
        var byId = new Dictionary<string, (string Id, string Name, string Type)>();
        foreach (var kind in enabled) byId.TryAdd(kind.Id, kind);
        var shares = found.Shares(viewKm2, id => id);
        return new KindStatistics(
            shares.Length,
            enabled.Length,
            enabled.GroupBy(k => k.Type)
                .Select(g => new KindTypeCount(g.Key, shares.Count(s => byId.TryGetValue(s.Name, out var k) && k.Type == g.Key), g.Count()))
                .ToArray(),
            shares.Take(TopKinds)
                .Select(s => byId.TryGetValue(s.Name, out var k) ? new KindShare(k.Name, k.Type, s.Share) : new KindShare(s.Name, "", s.Share))
                .ToArray());
    }

    /// <summary>The Prospects of the land zones in view, for the regions that are Planned. Null when none is.</summary>
    private static ProspectStatistics? CountProspects(World world, PlanWork[] plans, IEnumerable<CellInfo> landZones)
    {
        if (plans.Length == 0) return null;
        var zonesOfRegion = landZones.ToLookup(z => z.Region, z => z.Zone);
        var perZone = new Dictionary<GridCoord, List<Prospect>>();
        var all = new List<Prospect>();
        foreach (var work in plans)
        {
            foreach (var zone in zonesOfRegion[work.Region]) perZone[zone] = [];
            foreach (var prospect in work.Prospects)
                if (perZone.TryGetValue(world.Cells.CellAt(prospect.Spot).Zone, out var list))
                {
                    list.Add(prospect);
                    all.Add(prospect);
                }
        }

        var counts = perZone.Values.Select(ProspectCount.Of).ToArray();
        var most = counts.Length == 0 ? 0 : counts.Max(c => c.OnShow);
        var onShow = all.Where(p => p.OnShow).ToArray();
        return new ProspectStatistics(
            plans.Length,
            counts.Length,
            new ProspectCount(counts.Sum(c => c.City), counts.Sum(c => c.Town), counts.Sum(c => c.CityOnShow), counts.Sum(c => c.TownOnShow)),
            Enumerable.Range(0, most + 1)
                .Select(n => new ZoneBin(n, counts.Count(c => c.OnShow == n), counts.Count(c => c.OnShow == n && c.CityOnShow > 0)))
                .ToArray(),
            Enum.GetValues<ProspectClass>().Reverse()
                .Select(c => new NamedCount(c.ToString(), onShow.Count(p => p.Class == c)))
                .ToArray(),
            onShow.Length > 0 ? onShow.Average(p => p.Score) : null,
            plans.Average(p => p.ScoreSum));
    }

    /// <summary>The potential population of the Planned regions in view, each region whole. Null when none is Planned.</summary>
    private static PopulationStatistics? Population(World world, (RegionId Id, PlanWork Plan)[] plans)
    {
        if (plans.Length == 0) return null;
        var km = world.Cells.PatchSize / 1000;
        var races = new Dictionary<string, (double Persons, int Main, int Minority)>();
        double persons = 0, landKm2 = 0;
        foreach (var (id, work) in plans)
        {
            landKm2 += world.DetailOf(id).Patches.Where(p => !p.Shape.Ocean).Sum(p => p.Shape.Cells.Length) * km * km;
            foreach (var race in work.Population.Races)
            {
                if (race.Standing == RaceStanding.Absent) continue;
                persons += race.Persons;
                var sum = races.GetValueOrDefault(race.Race);
                races[race.Race] = (
                    sum.Persons + race.Persons,
                    sum.Main + (race.Standing == RaceStanding.Main ? 1 : 0),
                    sum.Minority + (race.Standing == RaceStanding.Minority ? 1 : 0));
            }
        }
        string NameOf(string id) => world.Parameters.Races.FirstOrDefault(r => r.Id == id)?.Name ?? id;
        return new PopulationStatistics(
            plans.Length,
            persons,
            landKm2,
            landKm2 > 0 ? persons / landKm2 : 0,
            races.OrderByDescending(r => r.Value.Persons).ThenBy(r => r.Key, StringComparer.Ordinal)
                .Select(r => new RaceStatistics(NameOf(r.Key), r.Value.Persons, r.Value.Main, r.Value.Minority))
                .ToArray());
    }

    /// <summary>The factions of the Planned regions in view. Null when none is Planned.</summary>
    private static FactionStatistics? Factions(PlanWork[] plans)
    {
        if (plans.Length == 0) return null;
        var factions = plans.SelectMany(p => p.Factions.Factions).ToArray();
        var ruling = factions.Where(f => f.Role == FactionRole.Ruling).ToArray();
        var opposition = factions.Where(f => f.Role == FactionRole.Opposition).ToArray();
        var lesser = factions.Where(f => f.Role == FactionRole.Lesser).ToArray();
        static NamedCount[] Counts<T>(IEnumerable<T> items) where T : notnull => items
            .GroupBy(x => x)
            .OrderByDescending(g => g.Count()).ThenBy(g => g.Key.ToString(), StringComparer.Ordinal)
            .Select(g => new NamedCount(g.Key.ToString()!, g.Count()))
            .ToArray();
        var read = plans.Where(p => p.Factions.Reading != null).ToArray();
        return new FactionStatistics(
            ruling.Length,
            Counts(ruling.Select(f => f.Form)),
            Counts(ruling.Select(f => f.Type)),
            opposition.Length,
            Counts(opposition.Where(f => f.Ground != null).Select(f => f.Ground!.Value)),
            lesser.Length,
            Counts(lesser.Select(f => f.Kind is null ? f.Form.ToString() : FactionRules.KindName(f.Kind))),
            read.Length > 0 ? read.Average(p => p.Factions.Tension) : null,
            ruling.Length > 0 ? ruling.Average(f => f.Grip) : null);
    }

    /// <summary>The cities and towns of the Settled regions in view, each region whole. Null when none is Settled.</summary>
    private static SettlementStatistics? Settlements(World world, RegionId[] regions)
    {
        var settled = regions.Select(world.FindSettlement).Where(s => s != null).Select(s => s!).ToArray();
        if (settled.Length == 0) return null;
        var places = settled.SelectMany(s => s.Places.Settlements).ToArray();
        var cities = places.Where(p => p.Kind == SettlementKind.City).ToArray();
        var towns = places.Where(p => p.Kind == SettlementKind.Town).ToArray();
        var links = settled.SelectMany(s => s.Links).ToArray();
        return new SettlementStatistics(
            settled.Length,
            cities.Length,
            cities.Sum(c => c.Persons),
            cities.Length > 0 ? cities.Max(c => c.Persons) : 0,
            towns.Length,
            towns.Sum(t => t.Persons),
            towns.Length > 0 ? towns.Max(t => t.Persons) : 0,
            settled.Sum(s => s.Places.RuralPersons),
            links.Count(l => l.Kind == TravelLinkKind.Road),
            links.Count(l => l.Kind == TravelLinkKind.RiverRoute),
            links.Count(l => l.Kind == TravelLinkKind.SeaLane));
    }
}

/// <summary>The statistics of a rectangle of the world. Every list by area is sorted, the largest first, unless it has an order of its own.</summary>
/// <param name="FinalBiomes">The biomes, the climate, the resources and the flora and fauna are the final ones.</param>
/// <param name="Stages">The regions of the rectangle by their stage.</param>
/// <param name="ElevationTypes">The land by the elevation type of its zone, in the order of the types.</param>
/// <param name="Belts">The land by thermal belt, coldest first.</param>
/// <param name="TemperatureC">The temperature of the land patches, cooled by their height. Null without land.</param>
/// <param name="Heights">Null when no region of the rectangle is Elevated.</param>
/// <param name="Water">Null without land.</param>
/// <param name="Resources">The resources of the land: the share of the land where each is listed, and its mean amount.</param>
/// <param name="Prospects">Null when no region of the rectangle is Planned.</param>
/// <param name="Population">Null when no region of the rectangle is Planned.</param>
/// <param name="Factions">Null when no region of the rectangle is Planned.</param>
/// <param name="Settlements">Null when no region of the rectangle is Settled.</param>
public sealed record ViewStatistics(
    bool FinalBiomes,
    int Regions,
    NamedCount[] Stages,
    double LandKm2,
    double OceanKm2,
    CellStatistics Cells,
    AreaShare[] ElevationTypes,
    AreaShare[] Bedrocks,
    AreaShare[] Biomes,
    AreaShare[] Belts,
    ValueSpread? TemperatureC,
    ValueSpread? PrecipitationMm,
    HeightStatistics? Heights,
    WaterStatistics? Water,
    ResourceSummary[] Resources,
    double Food,
    double Wealth,
    KindStatistics Flora,
    KindStatistics Fauna,
    ProspectStatistics? Prospects,
    PopulationStatistics? Population,
    FactionStatistics? Factions,
    SettlementStatistics? Settlements);

/// <summary>The zones and patches with a sample of the rectangle in them: what the mergers left.</summary>
/// <param name="CellsPerZone">The mean number of zone cells of a zone.</param>
/// <param name="CellsPerLandPatch">The mean number of patch cells of a land patch.</param>
public sealed record CellStatistics(
    int LandZones, int OceanZones, double CellsPerZone, int LandPatches, int OceanPatches, double CellsPerLandPatch, double CellsPerOceanPatch);

/// <summary>The heights of the land, read from the height maps of the regions that are Elevated.</summary>
/// <param name="Coverage">The share of the land of the rectangle that lies in an Elevated region, 0–1.</param>
/// <param name="Bands">The land by height band, lowest first.</param>
public sealed record HeightStatistics(ValueSpread HeightM, double Coverage, AreaShare[] Bands);

/// <summary>The share of the land in patches with fresh water or a coast, each 0–1.</summary>
public sealed record WaterStatistics(double River, double NavigableRiver, double Lake, double Coastal, double Harbour);

/// <summary>The kinds of flora, or of fauna, in a rectangle.</summary>
/// <param name="Found">How many kinds have a patch in the rectangle.</param>
/// <param name="Enabled">How many kinds of the Parameter Set are on.</param>
/// <param name="Top">The most widespread kinds, by the share of the rectangle in patches that hold them.</param>
public sealed record KindStatistics(int Found, int Enabled, KindTypeCount[] Types, KindShare[] Top);

public sealed record KindTypeCount(string Type, int Found, int Enabled);

/// <param name="Share">The share of the rectangle, land and sea, in patches that hold the kind, 0–1.</param>
public sealed record KindShare(string Name, string Type, double Share);

/// <summary>The Prospects of the land zones of a rectangle, counted in the regions that are Planned.</summary>
/// <param name="PlannedRegions">How many of the regions of the rectangle are Planned.</param>
/// <param name="LandZones">The land zones of those regions that the rectangle shows. A zone counts whole, also when part of it lies outside.</param>
/// <param name="Total">The Prospects of those zones.</param>
/// <param name="Zones">The histogram: the zones by their number of Prospects on show, from 0 up to the largest number.</param>
/// <param name="Classes">The Prospects on show of those zones by class, Rich first.</param>
/// <param name="MeanScore">The mean score of the Prospects on show. Null when none is.</param>
/// <param name="MeanScoreSum">The mean over the Planned regions of the sum of the scores of their Prospects.</param>
public sealed record ProspectStatistics(
    int PlannedRegions, int LandZones, ProspectCount Total, ZoneBin[] Zones, NamedCount[] Classes, double? MeanScore, double MeanScoreSum);

/// <param name="ZonesWithCity">How many of the zones hold a city Prospect on show.</param>
public sealed record ZoneBin(int Prospects, int Zones, int ZonesWithCity);

/// <summary>The potential population of the Planned regions of a rectangle, each region whole.</summary>
/// <param name="LandKm2">The land of those regions.</param>
public sealed record PopulationStatistics(int Regions, double Persons, double LandKm2, double PerKm2, RaceStatistics[] Races);

/// <param name="MainIn">The regions where the race is a main race.</param>
/// <param name="MinorityIn">The regions where the race is a minority race.</param>
public sealed record RaceStatistics(string Name, double Persons, int MainIn, int MinorityIn);

/// <summary>The factions of the Planned regions of a rectangle.</summary>
/// <param name="Ruling">The ruling factions: one for every region with persons.</param>
/// <param name="Lesser">The lesser factions by the name of their kind.</param>
/// <param name="MeanTension">The mean tension of the regions with persons. Null when there is none.</param>
public sealed record FactionStatistics(
    int Ruling, NamedCount[] RulingForms, NamedCount[] RulingTypes, int Oppositions, NamedCount[] Grounds,
    int LesserCount, NamedCount[] Lesser, double? MeanTension, double? MeanGrip);

/// <summary>The cities and towns of the Settled regions of a rectangle, each region whole, and the travel links those regions decide.</summary>
public sealed record SettlementStatistics(
    int Regions, int Cities, long CityPersons, long LargestCity, int Towns, long TownPersons, long LargestTown, long RuralPersons,
    int Roads, int RiverRoutes, int SeaLanes);
