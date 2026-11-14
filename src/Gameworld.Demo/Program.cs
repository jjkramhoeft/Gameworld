using System.Collections.Concurrent;
using System.Globalization;
using System.Text.Json;
using Gameworld.Demo;
using Gameworld.Domain;
using Gameworld.Generation;
using Gameworld.Generation.Rendering;

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
}, new JsonSerializerOptions()));

app.MapGet("/api/inspect", (double x, double y, WorldHost host) => Json(host.Inspect(new WorldPoint(x, y))));

app.MapGet("/api/regions/{i:int}/{j:int}", (int i, int j, string? stage, WorldHost host) =>
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
                    + $"\"MinProspectScore\":{f.Parameters.Travel.MinProspectScore.ToString(CultureInfo.InvariantCulture)},"
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
                var prospect = plan?.Prospects.FirstOrDefault(p => p.Patch.Coord == info.Patch);
                var elevation = world.FindElevation(regionId);
                var zoneCells = world.Advance(regionId, RegionStage.Outlined).Outline.Zones
                    .FirstOrDefault(z => z.Id.Coord == info.Zone)?.Cells.Count ?? 1;

                return new PointInfo(
                    point,
                    regionId,
                    world.StageOf(regionId),
                    world.Advance(regionId, RegionStage.Outlined).Outline.HasSeaAccess,
                    info.Ocean,
                    new ZoneInfo(
                        new ZoneId(new ZoneCellId(info.Zone)),
                        cells.ZoneType(info.Zone),
                        cells.ZoneBedrock(info.Zone),
                        world.Parameters.Bedrocks.FirstOrDefault(b => b.Id == cells.ZoneBedrock(info.Zone))?.Name,
                        zoneCells,
                        !info.Ocean && world.IsRiverPoorZone(info.Zone)),
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
                            .Select(r => new ResourceInfo(r.Resource, ResourceCatalog.GroupOf(r.Resource), r.Amount.Amount, r.Amount.Variant, r.Amount.Origin))
                            .ToArray(),
                        patch.Resources.Food,
                        patch.Resources.Wealth,
                        score?.Score,
                        score?.Factors),
                    prospect,
                    elevation?.Heights.Map.Sample(point),
                    new FieldValues(
                        world.Fields.Sample(FieldKind.InitialTemperature, point),
                        world.Fields.Sample(FieldKind.Precipitation, point),
                        world.Fields.Sample(FieldKind.Prosperity, point),
                        world.Fields.Sample(FieldKind.Supernatural, point)));
            }
        }
    }

    public sealed record PointInfo(
        WorldPoint Point,
        RegionId Region,
        RegionStage Stage,
        bool RegionHasSeaAccess,
        bool Ocean,
        ZoneInfo Zone,
        PatchInfo? Patch,
        Prospect? Prospect,
        double? HeightM,
        FieldValues Fields);

    /// <summary>The four Fields sampled at the point itself, not at the site of its patch.</summary>
    public sealed record FieldValues(double InitialTemperatureC, double PrecipitationMm, double Prosperity, double Supernatural);

    public sealed record ZoneInfo(ZoneId Id, ElevationType ElevationType, string? Bedrock, string? BedrockName, int Cells, bool RiverPoor);

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
        double? Score,
        ProspectFactors? Factors);

    public sealed record ResourceInfo(Resource Resource, ResourceGroup Group, double Amount, string? Variant, ResourceOrigin Origin);
}
