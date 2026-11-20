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
    Biomes = Enum.GetValues<Biome>().Select(b => new { Biome = b.ToString(), Colour = MapColours.Of(b) }),
    Fields = WorldHost.FieldLayers.Select(f => new
    {
        f.Layer,
        f.Name,
        f.Unit,
        Bands = MapRenderer.FieldBands(f.Kind).Select(b => new { b.From, b.To, Colour = MapColours.Hex(b.Colour) }),
    }),
    Prospects = Enum.GetValues<ProspectClass>().Reverse().Select(c =>
        new { Class = c.ToString(), Colour = MapColours.Hex(MapRenderer.ProspectColour(c)) }),
}, new JsonSerializerOptions()));

// The region, zone and patch at a point. Looking takes the region to Detailed; with a stage it is taken that far first.
app.MapGet("/api/inspect", (double x, double y, string? stage, WorldHost host) =>
{
    if (!double.IsFinite(x + y)) return Results.BadRequest();
    RegionStage? advanceTo = null;
    if (stage != null)
    {
        if (!Enum.TryParse<RegionStage>(stage, ignoreCase: true, out var target)) return Results.BadRequest();
        advanceTo = target;
    }
    return Json(host.Inspect(new WorldPoint(x, y), advanceTo));
});

// The statistics of a rectangle of the world, which the browser sets to the current view.
app.MapGet("/api/statistics", (double left, double top, double width, double height, bool? final, bool? plan, WorldHost host) =>
{
    if (!(width > 0 && height > 0) || !double.IsFinite(left + top + width + height)) return Results.BadRequest();
    return Json(host.Statistics(left, top, width, height, final ?? false, plan ?? false));
});

// The listed resources of the patches of a rectangle of the world, which the browser sets to the current view at full zoom.
app.MapGet("/api/resources", (double left, double top, double width, double height, WorldHost host) =>
{
    if (!(width > 0 && height > 0) || !double.IsFinite(left + top + width + height)) return Results.BadRequest();
    return host.ResourceLabels(left, top, width, height) is { } labels
        ? Results.Json(labels, new JsonSerializerOptions())
        : Results.BadRequest();
});

// The names of the cities and towns of a rectangle of the world, which the browser sets to the current view.
app.MapGet("/api/places", (double left, double top, double width, double height, WorldHost host) =>
{
    if (!(width > 0 && height > 0) || !double.IsFinite(left + top + width + height)) return Results.BadRequest();
    return host.PlaceLabels(left, top, width, height) is { } labels
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
    public sealed partial class WorldHost
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
    }

    /// <summary>The label of a patch on the map: where it stands, and the listed resources of the patch, the largest amount first.</summary>
    public sealed record PatchLabel(double X, double Y, ResourceLabel[] Resources);

    /// <param name="Resource">The name of the resource, as the domain model spells it.</param>
    public sealed record ResourceLabel(string Abbreviation, string Resource, string? Variant, double Amount, bool Deposit);
}
