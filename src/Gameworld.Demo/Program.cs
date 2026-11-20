using System.Collections.Concurrent;
using System.Globalization;
using System.Text.Json;
using Gameworld.Demo;
using Gameworld.Domain;
using Gameworld.Generation;
using Gameworld.Generation.Cells;
using Gameworld.Generation.Factions;
using Gameworld.Generation.Rendering;
using Gameworld.Generation.Stages;

// The demo app: a small map viewer on top of the generator. The browser pans and zooms a
// tiled map; the backend holds one foundation, generates regions on demand and serves tiles
// and the domain data behind a point. Changing the seed or the Parameter Set replaces the
// foundation, which discards all derived data.

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<WorldHost>();
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

static IResult Json<T>(T value) => Results.Content(DomainJson.Serialize(value), "application/json");

app.MapGet("/api/foundation", (WorldHost host) => Results.Content(host.DescribeFoundation(), "application/json"));

app.MapPut("/api/foundation", async (HttpRequest request, WorldHost host) =>
{
    try
    {
        using var body = await JsonDocument.ParseAsync(request.Body);
        var root = body.RootElement;
        var seed = root.TryGetProperty("Seed", out var s) ? s.GetString() ?? Seed.DefaultText : Seed.DefaultText;
        var parameters = root.TryGetProperty("Parameters", out var p) && p.ValueKind == JsonValueKind.Object
            ? ParameterSet.FromJson(p.GetRawText())
            : ParameterSet.Default;
        host.Replace(seed, parameters);
        return Results.Content(host.DescribeFoundation(), "application/json");
    }
    catch (Exception e) when (e is JsonException or NotSupportedException or ArgumentException or InvalidOperationException)
    {
        return Results.BadRequest(new { error = e.Message });
    }
});

app.MapGet("/api/tiles/{version:int}/{z:int}/{x:int}/{y:int}.png",
    (int version, int z, int x, int y, string? mode, string? layers, WorldHost host, HttpResponse response) =>
    {
        if (z < 0 || z > WorldHost.MaxZoom) return Results.NotFound();
        if (version != host.Version) return Results.NotFound();
        if (!Enum.TryParse<MapMode>(mode ?? "Map", ignoreCase: true, out var mapMode)) return Results.BadRequest();
        var png = host.Tile(z, x, y, mapMode, WorldHost.ParseLayers(layers));
        response.Headers.CacheControl = "public, max-age=3600";
        return Results.Bytes(png, "image/png");
    });

// The legends: every biome with the colour the renderer draws it in, and the bands of every field layer.
app.MapGet("/api/legend", () => Results.Json(new
{
    Biomes = Enum.GetValues<Biome>().Select(b =>
    {
        var (r, g, bl) = Gameworld.Generation.Biomes.BiomeRules.Colour(b);
        return new { Biome = b.ToString(), Colour = $"#{r:x2}{g:x2}{bl:x2}" };
    }),
    Fields = WorldHost.FieldLayers.Select(f => new
    {
        f.Layer,
        f.Name,
        f.Unit,
        Bands = MapRenderer.FieldBands(f.Kind).Select(b => new { b.From, b.To, Colour = $"#{b.Colour.R:x2}{b.Colour.G:x2}{b.Colour.B:x2}" }),
    }),
    Prospects = Enum.GetValues<ProspectClass>().Reverse().Select(c =>
    {
        var colour = MapRenderer.ProspectColour(c);
        return new { Class = c.ToString(), Colour = $"#{colour.R:x2}{colour.G:x2}{colour.B:x2}" };
    }),
}, new JsonSerializerOptions()));

app.MapGet("/api/inspect", (double x, double y, WorldHost host) => Json(host.Inspect(new WorldPoint(x, y))));

// The statistics of a rectangle of the world, which the browser sets to the current view.
app.MapGet("/api/statistics", (double left, double top, double width, double height, bool? final, bool? plan, WorldHost host) =>
{
    if (!(width > 0 && height > 0) || !double.IsFinite(left + top + width + height)) return Results.BadRequest();
    return Results.Json(host.Statistics(left, top, width, height, final ?? false, plan ?? false), new JsonSerializerOptions());
});

// The listed resources of the patches of a rectangle of the world, which the browser sets to the current view at full zoom.
app.MapGet("/api/resources", (double left, double top, double width, double height, WorldHost host) =>
{
    if (!(width > 0 && height > 0) || !double.IsFinite(left + top + width + height)) return Results.BadRequest();
    return host.ResourceLabels(left, top, width, height) is { } labels
        ? Results.Json(labels, new JsonSerializerOptions())
        : Results.BadRequest();
});

app.MapGet("/api/regions/{i:int}/{j:int}",(int i, int j, string? stage, WorldHost host) =>
{
    if (!Enum.TryParse<RegionStage>(stage ?? "Detailed", ignoreCase: true, out var target)) return Results.BadRequest();
    return Json(host.Region(new RegionId(i, j), target));
});

app.Run();

namespace Gameworld.Demo
{
    /// <summary>Holds the one foundation of the demo and its generated world. The world is not thread-safe, so every use is serialised.</summary>
    public sealed class WorldHost
    {
        public const int TileSize = 256;
        public const int MaxZoom = 6;
        public const double BaseMetresPerPixel = 2000;

        private readonly object _gate = new();
        private readonly ConcurrentDictionary<string, byte[]> _tiles = new();
        private World _world = World.Create(Seed.DefaultText);

        /// <summary>
        /// Part of every tile address, so the browser never shows a tile of an earlier foundation.
        /// It starts at the clock, not at 1, so tiles cached from an earlier run of the app are not reused either.
        /// </summary>
        public int Version { get; private set; } = (int)(DateTimeOffset.UtcNow.ToUnixTimeSeconds() % 1_000_000_000);

        public static double MetresPerPixel(int zoom) => BaseMetresPerPixel / (1 << zoom);

        /// <summary>Phase 1: the seed and the parameters can be changed and reloaded. All derived data is then discarded.</summary>
        public void Replace(string seedText, ParameterSet parameters)
        {
            var world = World.Create(seedText, parameters);
            lock (_gate)
            {
                _world = world;
                _tiles.Clear();
                Version++;
            }
        }

        public string DescribeFoundation()
        {
            lock (_gate)
            {
                var f = _world.Foundation;
                return "{"
                    + $"\"Version\":{Version},"
                    + $"\"Seed\":{JsonSerializer.Serialize(f.Seed.Text)},"
                    + $"\"SeedValue\":{f.Seed.Value},"
                    + $"\"GeneratorVersion\":{f.GeneratorVersion.Number},"
                    + $"\"TileSize\":{TileSize},\"MaxZoom\":{MaxZoom},\"BaseMetresPerPixel\":{BaseMetresPerPixel},"
                    + $"\"RegionSizeM\":{_world.Cells.RegionSize.ToString(CultureInfo.InvariantCulture)},"
                    + $"\"Parameters\":{f.Parameters.ToJson()},"
                    + $"\"DefaultParameters\":{ParameterSet.Default.ToJson()}"
                    + "}";
            }
        }

        /// <summary>The field layers: the name of the layer in a tile address, and the name and unit its legend shows.</summary>
        public static readonly (FieldKind Kind, string Layer, string Name, string Unit)[] FieldLayers =
        [
            (FieldKind.InitialTemperature, "temperature", "Initial temperature", "°C"),
            (FieldKind.Precipitation, "precipitation", "Precipitation", "mm/yr"),
            (FieldKind.Prosperity, "prosperity", "Prosperity", "index"),
            (FieldKind.Supernatural, "supernatural", "Supernatural", "index"),
        ];

        public static MapLayers ParseLayers(string? layers)
        {
            var set = (layers ?? "borders,rivers,lakes").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);
            return new MapLayers
            {
                Borders = set.Contains("borders"),
                Rivers = set.Contains("rivers"),
                Lakes = set.Contains("lakes"),
                Prospects = set.Contains("prospects"),
                Travel = set.Contains("travel"),
                Hillshade = set.Contains("hillshade"),
                FinalBiomes = set.Contains("final"),
                InitialTemperature = set.Contains("temperature"),
                Precipitation = set.Contains("precipitation"),
                Prosperity = set.Contains("prosperity"),
                Supernatural = set.Contains("supernatural"),
            };
        }

        public byte[] Tile(int zoom, int x, int y, MapMode mode, MapLayers layers)
        {
            var key = $"{zoom}/{x}/{y}/{mode}/{layers}";
            if (_tiles.TryGetValue(key, out var cached)) return cached;
            lock (_gate)
            {
                if (_tiles.TryGetValue(key, out cached)) return cached;
                var mpp = MetresPerPixel(zoom);
                var view = new MapView(x * TileSize * mpp, y * TileSize * mpp, mpp, TileSize, TileSize);
                var png = MapRenderer.Render(_world, view, mode, layers).ToPng();
                if (_tiles.Count > 4000) _tiles.Clear();
                _tiles[key] = png;
                return png;
            }
        }

        public RegionSnapshot Region(RegionId id, RegionStage stage)
        {
            lock (_gate) return _world.Advance(id, stage).ToSnapshot();
        }

        /// <summary>
        /// The share of every biome in a rectangle of the world, by area. The rectangle is sampled on an even lattice of
        /// points, so the shares are those of the picture the map shows. It takes the regions in the rectangle to Detailed,
        /// or to Elevated when the final biomes are asked for.
        /// <para>
        /// It also counts the Prospects of the land zones in the rectangle, zone by zone. Only a Planned region has
        /// Prospects: with <paramref name="plan"/> the regions in the rectangle are taken to Planned, which costs a
        /// height map each; without it only the regions that are Planned already are counted.
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
                var counts = new Dictionary<Biome, int>();
                var patchBiome = new Dictionary<long, Biome?>();
                var regions = new Dictionary<long, GridCoord>();
                var landZones = new Dictionary<long, CellInfo>();
                int land = 0, ocean = 0;
                for (var j = 0; j < ny; j++)
                    for (var i = 0; i < nx; i++)
                    {
                        var c = cells.CellAt(left + (i + 0.5) * width / nx, top + (j + 0.5) * height / ny);
                        regions[c.Region.Key] = c.Region;
                        if (c.Ocean) { ocean++; continue; }
                        land++;
                        landZones[c.Zone.Key] = c;
                        if (!patchBiome.TryGetValue(c.Patch.Key, out var biome))
                        {
                            var region = new RegionId(c.Region);
                            biome = finalBiomes && world.ElevationOf(region).Find(c.Patch) is { } final
                                ? final.Biome.Biome
                                : world.DetailOf(region).Find(c.Patch)?.Biome?.Biome;
                            patchBiome[c.Patch.Key] = biome;
                        }
                        if (biome is { } b) counts[b] = counts.GetValueOrDefault(b) + 1;
                    }

                var sampleKm2 = width / nx * (height / ny) / 1e6;
                return new ViewStatistics(
                    finalBiomes,
                    regions.Count,
                    land * sampleKm2,
                    ocean * sampleKm2,
                    counts.OrderByDescending(p => p.Value).ThenBy(p => p.Key)
                        .Select(p =>
                        {
                            var (r, g, bl) = Gameworld.Generation.Biomes.BiomeRules.Colour(p.Key);
                            return new BiomeShare(p.Key.ToString(), $"#{r:x2}{g:x2}{bl:x2}", p.Value * sampleKm2, (double)p.Value / land);
                        })
                        .ToArray(),
                    CountProspects(world, regions.Values, landZones.Values, plan));
            }
        }

        /// <summary>The Prospects of the land zones in view, for the regions that are Planned. Null when none is.</summary>
        private static ProspectStatistics? CountProspects(World world, IEnumerable<GridCoord> regions, IEnumerable<CellInfo> landZones, bool plan)
        {
            var zonesOfRegion = landZones.ToLookup(z => z.Region, z => z.Zone);
            var perZone = new Dictionary<GridCoord, List<Prospect>>();
            var planned = 0;
            foreach (var region in regions.OrderBy(r => r.Key))
            {
                var id = new RegionId(region);
                var work = plan ? world.PlanOf(id) : world.FindPlan(id);
                if (work is null) continue;
                planned++;
                foreach (var zone in zonesOfRegion[region]) perZone[zone] = [];
                foreach (var prospect in work.Prospects)
                    if (perZone.TryGetValue(world.Cells.CellAt(prospect.Spot).Zone, out var list)) list.Add(prospect);
            }
            if (planned == 0) return null;

            var counts = perZone.Values.Select(ProspectCount.Of).ToArray();
            var most = counts.Length == 0 ? 0 : counts.Max(c => c.OnShow);
            return new ProspectStatistics(
                planned,
                counts.Length,
                new ProspectCount(counts.Sum(c => c.City), counts.Sum(c => c.Town), counts.Sum(c => c.CityOnShow), counts.Sum(c => c.TownOnShow)),
                Enumerable.Range(0, most + 1)
                    .Select(n => new ZoneBin(n, counts.Count(c => c.OnShow == n), counts.Count(c => c.OnShow == n && c.CityOnShow > 0)))
                    .ToArray());
        }

        /// <summary>
        /// The listed resources of every patch with a patch cell in a rectangle of the world, the largest amount first,
        /// for the labels the map shows at full zoom. A label stands at the mean of the sites of the patch's cells, so
        /// it does not move when the view does. It takes the regions in the rectangle to Detailed. Null when the
        /// rectangle holds too many patch cells: the labels are for a close view only.
        /// </summary>
        public PatchLabel[]? ResourceLabels(double left, double top, double width, double height)
        {
            const int MaxPatchCells = 4000;
            lock (_gate)
            {
                var world = _world;
                var cells = world.Cells;
                var level = cells.Patches;
                // One grid square more on every side: the site of a patch cell is jittered and its border warped.
                int i0 = (int)Math.Floor(left / level.Size) - 1, i1 = (int)Math.Floor((left + width) / level.Size) + 1;
                int j0 = (int)Math.Floor(top / level.Size) - 1, j1 = (int)Math.Floor((top + height) / level.Size) + 1;
                if ((long)(i1 - i0 + 1) * (j1 - j0 + 1) > MaxPatchCells) return null;

                var labels = new List<PatchLabel>();
                var seen = new HashSet<long>();
                for (var j = j0; j <= j1; j++)
                    for (var i = i0; i <= i1; i++)
                    {
                        var info = cells.InfoOf(new GridCoord(i, j));
                        if (!seen.Add(info.Patch.Key)) continue;
                        var patch = world.DetailOf(new RegionId(info.Region)).Find(info.Patch);
                        if (patch is null) continue;
                        var listed = patch.Resources.Listed
                            .OrderByDescending(r => r.Amount.Amount)
                            .Select(r => new ResourceLabel(ResourceCatalog.AbbreviationOf(r.Resource), r.Resource.ToString(), r.Amount.Variant, r.Amount.Amount, r.Amount.Origin == ResourceOrigin.Deposit))
                            .ToArray();
                        if (listed.Length == 0) continue;
                        var members = patch.Shape.Cells;
                        labels.Add(new PatchLabel(
                            members.Average(c => level.SiteX(c.I, c.J)),
                            members.Average(c => level.SiteY(c.I, c.J)),
                            listed));
                    }
                return labels.ToArray();
            }
        }

        /// <summary>The domain data behind a point: its region, zone and patch, and what the stages the region has reached say about the patch. Looking takes the region to Detailed, no further.</summary>
        public PointInfo Inspect(WorldPoint point)
        {
            lock (_gate)
            {
                var world = _world;
                var cells = world.Cells;
                var info = cells.CellAt(point);
                var regionId = new RegionId(info.Region);
                var detail = world.DetailOf(regionId);
                var patch = detail.Find(info.Patch);
                // Scores and Prospects belong to the Planned stage: they are shown once the region has reached it.
                var plan = world.FindPlan(regionId);
                var score = plan?.Scores.FirstOrDefault(s => s.Patch.Root == info.Patch);
                var prospectAt = plan is null ? -1 : Array.FindIndex(plan.Prospects, p => p.Patch.Coord == info.Patch);
                var prospect = prospectAt < 0 ? null : plan!.Prospects[prospectAt];
                string NameOf(string race) => world.Parameters.Races.FirstOrDefault(x => x.Id == race)?.Name ?? race;
                string VocationNameOf(string id) => world.Parameters.Vocations.FirstOrDefault(x => x.Id == id)?.Name ?? id;
                FactionInfo InfoOf(Faction f) => new(
                    f.Slot, f.Role, f.Type, f.Form, NameOf(f.LeadRace),
                    f.Kind is null ? null : FactionRules.KindName(f.Kind),
                    f.Vocations.Select(VocationNameOf).ToArray(), f.Ground, f.Grip, f.Seat,
                    ColourOf(f.MainColour), ColourOf(f.SecondaryColour));
                ColourInfo? ColourOf(string? id) =>
                    world.Parameters.Colours.FirstOrDefault(x => x.Id == id) is { } colour ? new(colour.Name, colour.Hex) : null;
                string FloraNameOf(string id) => world.Parameters.Flora.FirstOrDefault(x => x.Id == id)?.Name ?? id;
                string FaunaNameOf(string id) => world.Parameters.Fauna.FirstOrDefault(x => x.Id == id)?.Name ?? id;
                var elevation = world.FindElevation(regionId);
                var zoneCells = world.Advance(regionId, RegionStage.Outlined).Outline.Zones
                    .FirstOrDefault(z => z.Id.Coord == info.Zone)?.Cells.Count ?? 1;
                var zoneProspects = plan is null || info.Ocean
                    ? null
                    : ProspectCount.Of(plan.Prospects.Where(p => cells.CellAt(p.Spot).Zone == info.Zone));

                return new PointInfo(
                    point,
                    regionId,
                    world.StageOf(regionId),
                    world.Advance(regionId, RegionStage.Outlined).Outline.HasSeaAccess,
                    plan is null ? null : ProspectCount.Of(plan.Prospects),
                    plan?.ScoreSum,
                    plan?.Population.Races
                        .Select(r => new RacePopulationInfo(r.Race, NameOf(r.Race), r.Persons, r.Standing))
                        .ToArray(),
                    plan?.Factions.Factions.Select(InfoOf).ToArray(),
                    info.Ocean,
                    new ZoneInfo(
                        new ZoneId(new ZoneCellId(info.Zone)),
                        cells.ZoneType(info.Zone),
                        cells.ZoneBedrock(info.Zone),
                        world.Parameters.Bedrocks.FirstOrDefault(b => b.Id == cells.ZoneBedrock(info.Zone))?.Name,
                        zoneCells,
                        !info.Ocean && world.IsRiverPoorZone(info.Zone),
                        zoneProspects),
                    patch is null ? null : new PatchInfo(
                        new PatchId(new PatchCellId(info.Patch)),
                        patch.Shape.Cells.Length,
                        patch.Shape.Coastal,
                        patch.Biome,
                        elevation?.Find(info.Patch)?.Biome,
                        elevation?.Find(info.Patch)?.MeanHeightM,
                        patch.Shape.Ocean ? null : patch.Water.River,
                        patch.Shape.Ocean ? null : patch.Water.Mouth,
                        patch.Water.LakeAreaM2 / 1e6,
                        patch.Water.Harbour,
                        patch.Resources.Listed
                            .OrderByDescending(r => r.Amount.Amount)
                            .Select(r => new ResourceInfo(r.Resource, ResourceCatalog.AbbreviationOf(r.Resource), ResourceCatalog.GroupOf(r.Resource), r.Amount.Amount, r.Amount.Variant, r.Amount.Origin))
                            .ToArray(),
                        patch.Resources.Food,
                        patch.Resources.Wealth,
                        (elevation?.Find(info.Patch)?.Flora ?? patch.Flora).Select(FloraNameOf).ToArray(),
                        (elevation?.Find(info.Patch)?.Fauna ?? patch.Fauna).Select(FaunaNameOf).ToArray(),
                        score?.Score,
                        score?.Factors),
                    prospect,
                    prospectAt < 0 ? null : plan!.ProspectRaces[prospectAt].Select(NameOf).ToArray(),
                    elevation?.Heights.Map.Sample(point),
                    new FieldValues(
                        world.Fields.Sample(FieldKind.InitialTemperature, point),
                        world.Fields.Sample(FieldKind.Precipitation, point),
                        world.Fields.Sample(FieldKind.Prosperity, point),
                        world.Fields.Sample(FieldKind.Supernatural, point)));
            }
        }
    }

    /// <summary>The statistics of a rectangle of the world. The biomes are sorted by area, largest first.</summary>
    /// <param name="Prospects">Null when no region of the rectangle is Planned.</param>
    public sealed record ViewStatistics(bool FinalBiomes, int Regions, double LandKm2, double OceanKm2, BiomeShare[] Biomes, ProspectStatistics? Prospects);

    /// <param name="Share">The share of the land of the rectangle, 0–1.</param>
    public sealed record BiomeShare(string Biome, string Colour, double AreaKm2, double Share);

    /// <summary>
    /// How many Prospects a region or a zone holds, for cities and for towns. The ones on show are the ones the map
    /// draws and the travel network connects: the sum of the scores of a region's Prospects decides how many they are.
    /// </summary>
    public sealed record ProspectCount(int City, int Town, int CityOnShow, int TownOnShow)
    {
        public int OnShow => CityOnShow + TownOnShow;

        public static ProspectCount Of(IEnumerable<Prospect> prospects)
        {
            int city = 0, town = 0, cityOnShow = 0, townOnShow = 0;
            foreach (var p in prospects)
            {
                if (p.ForCity) city++; else town++;
                if (!p.OnShow) continue;
                if (p.ForCity) cityOnShow++; else townOnShow++;
            }
            return new ProspectCount(city, town, cityOnShow, townOnShow);
        }
    }

    /// <summary>The Prospects of the land zones of a rectangle, counted in the regions that are Planned.</summary>
    /// <param name="PlannedRegions">How many of the regions of the rectangle are Planned.</param>
    /// <param name="LandZones">The land zones of those regions that the rectangle shows. A zone counts whole, also when part of it lies outside.</param>
    /// <param name="Total">The Prospects of those zones.</param>
    /// <param name="Zones">The histogram: the zones by their number of Prospects on show, from 0 up to the largest number.</param>
    public sealed record ProspectStatistics(int PlannedRegions, int LandZones, ProspectCount Total, ZoneBin[] Zones);

    /// <param name="ZonesWithCity">How many of the zones hold a city Prospect on show.</param>
    public sealed record ZoneBin(int Prospects, int Zones, int ZonesWithCity);

    /// <summary>The potential population of one race in a region, with the name of the race and whether it is a main or a minority race of the region.</summary>
    public sealed record RacePopulationInfo(string Race, string Name, long Persons, RaceStanding Standing);

    /// <param name="RegionProspects">Null until the region is Planned.</param>
    /// <param name="RegionScoreSum">The sum of the scores of the region's Prospects, which decides how many are on show. Null until the region is Planned.</param>
    /// <param name="RegionPopulation">The potential population of the region per race that is on, in the order of the Parameter Set. Null until the region is Planned.</param>
    /// <param name="ProspectRaces">The names of the races of a city or a town on the Prospect of the patch, the largest first and the drawn one last. Null when the patch has no Prospect.</param>
    /// <param name="RegionFactions">The factions of the region, by slot: the ruling faction, the opposition when there is one, then the lesser factions. Null until the region is Planned.</param>
    public sealed record PointInfo(
        WorldPoint Point,
        RegionId Region,
        RegionStage Stage,
        bool RegionHasSeaAccess,
        ProspectCount? RegionProspects,
        double? RegionScoreSum,
        RacePopulationInfo[]? RegionPopulation,
        FactionInfo[]? RegionFactions,
        bool Ocean,
        ZoneInfo Zone,
        PatchInfo? Patch,
        Prospect? Prospect,
        string[]? ProspectRaces,
        double? HeightM,
        FieldValues Fields);

    /// <summary>A faction of a region, with the names of its lead race, its kind and its vocations.</summary>
    /// <param name="Kind">The name of the kind of a lesser faction, and of the trade of a guild in opposition.</param>
    /// <param name="Seat">The patch of the Prospect the faction sits on.</param>
    /// <param name="MainColour">Its main colour. Null when the Parameter Set has no colours.</param>
    /// <param name="SecondaryColour">Its secondary colour. Null when the Parameter Set has fewer than two colours.</param>
    public sealed record FactionInfo(
        int Slot, FactionRole Role, FactionType Type, FactionForm Form, string LeadRace, string? Kind,
        string[] Vocations, OppositionGround? Ground, double Grip, PatchId? Seat,
        ColourInfo? MainColour, ColourInfo? SecondaryColour);

    /// <summary>A colour of a faction: its name and its value as #RRGGBB.</summary>
    public sealed record ColourInfo(string Name, string Hex);

    /// <summary>The four Fields sampled at the point itself, not at the site of its patch.</summary>
    public sealed record FieldValues(double InitialTemperatureC, double PrecipitationMm, double Prosperity, double Supernatural);

    /// <param name="Prospects">The Prospects in the zone. Null for an ocean zone and until the region is Planned.</param>
    public sealed record ZoneInfo(ZoneId Id, ElevationType ElevationType, string? Bedrock, string? BedrockName, int Cells, bool RiverPoor, ProspectCount? Prospects);

    /// <param name="Flora">The names of the kinds of flora of the patch: of the final biome when the region is Elevated, else of the estimated biome.</param>
    /// <param name="Fauna">The names of the kinds of fauna of the patch, as for the flora.</param>
    public sealed record PatchInfo(
        PatchId Id,
        int Cells,
        bool Coastal,
        BiomeRecord? EstimatedBiome,
        BiomeRecord? FinalBiome,
        double? MeanHeightM,
        double? River,
        double? Mouth,
        double LakeAreaKm2,
        bool Harbour,
        ResourceInfo[] Resources,
        double Food,
        double Wealth,
        string[] Flora,
        string[] Fauna,
        double? Score,
        ProspectFactors? Factors);

    public sealed record ResourceInfo(Resource Resource, string Abbreviation, ResourceGroup Group, double Amount, string? Variant, ResourceOrigin Origin);

    /// <summary>The label of a patch on the map: where it stands, and the listed resources of the patch, the largest amount first.</summary>
    public sealed record PatchLabel(double X, double Y, ResourceLabel[] Resources);

    /// <param name="Resource">The name of the resource, as the domain model spells it.</param>
    public sealed record ResourceLabel(string Abbreviation, string Resource, string? Variant, double Amount, bool Deposit);
}
