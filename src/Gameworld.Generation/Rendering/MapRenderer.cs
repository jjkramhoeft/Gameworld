using Gameworld.Domain;
using Gameworld.Generation.Basics;
using Gameworld.Generation.Biomes;
using Gameworld.Generation.Cells;

namespace Gameworld.Generation.Rendering;

/// <summary>What the base colour of the map shows.</summary>
public enum MapMode
{
    /// <summary>The elevation type of the zones.</summary>
    Map,
    /// <summary>The biome of the patches.</summary>
    Biomes,
    /// <summary>A hypsometric tint of the height map.</summary>
    Height,
    /// <summary>A blank white base, land and sea alike, for looking at the layers alone.</summary>
    None,
}

/// <summary>The layers drawn on top of the base colour.</summary>
public sealed record MapLayers
{
    public bool Borders { get; init; } = true;
    public bool Rivers { get; init; } = true;
    public bool Lakes { get; init; } = true;
    public bool Prospects { get; init; } = false;
    public bool Travel { get; init; } = false;
    public bool Hillshade { get; init; } = false;
    /// <summary>Biomes mode shows the final biomes (Elevated) instead of the estimated ones (Detailed).</summary>
    public bool FinalBiomes { get; init; } = false;
    /// <summary>The field layers: each tints the land in the bands of one field (section 9).</summary>
    public bool InitialTemperature { get; init; } = false;
    public bool Precipitation { get; init; } = false;
    public bool Prosperity { get; init; } = false;
    public bool Supernatural { get; init; } = false;

    public bool Shows(FieldKind kind) => kind switch
    {
        FieldKind.InitialTemperature => InitialTemperature,
        FieldKind.Precipitation => Precipitation,
        FieldKind.Prosperity => Prosperity,
        FieldKind.Supernatural => Supernatural,
        _ => false,
    };

    public static MapLayers None { get; } = new() { Borders = false, Rivers = false, Lakes = false };
    public static MapLayers All { get; } = new() { Prospects = true, Travel = true };
}

/// <summary>The part of the world an image shows: the world position of its top-left corner and the scale.</summary>
public readonly record struct MapView(double Left, double Top, double MetresPerPixel, int Width, int Height)
{
    public static MapView Centred(WorldPoint centre, double metresPerPixel, int width, int height) =>
        new(centre.X - width * metresPerPixel / 2, centre.Y - height * metresPerPixel / 2, metresPerPixel, width, height);

    public double ToPixelX(double worldX) => (worldX - Left) / MetresPerPixel;
    public double ToPixelY(double worldY) => (worldY - Top) / MetresPerPixel;
}

/// <summary>
/// Drawing the map (map-generation.md section 17): per pixel, classify the pixel's world
/// position and colour it; then the borders, rivers, lakes, Prospects and travel on top.
/// Rendering advances the regions in view as far as the requested mode and layers need.
/// </summary>
public static partial class MapRenderer
{
    private static readonly Rgb OceanColour = new(38, 84, 132);
    private static readonly Rgb CoastColour = new(14, 20, 30);
    private static readonly Rgb RegionBorderColour = new(235, 28, 36);
    private static readonly Rgb WaterColour = new(64, 122, 178);

    public static Rgb ElevationColour(ElevationType type) => type switch
    {
        ElevationType.FlatLand => new Rgb(143, 174, 93),
        ElevationType.Lowland => new Rgb(92, 134, 74),
        ElevationType.Hills => new Rgb(154, 138, 98),
        ElevationType.Mountains => new Rgb(124, 116, 112),
        _ => OceanColour,
    };

    public static Canvas Render(World world, MapView view, MapMode mode, MapLayers? layers = null)
    {
        layers ??= new MapLayers();
        var cells = world.Cells;
        var canvas = new Canvas(view.Width, view.Height);
        var mpp = view.MetresPerPixel;

        // Classify every pixel, with a margin of 5 so borders match across neighbouring tiles:
        // a border dot looks up to 5 pixels beyond its own pixel.
        const int margin = 5;
        var bw = view.Width + 2 * margin;
        var bh = view.Height + 2 * margin;
        var info = new CellInfo[bw * bh];
        var regionsInView = new HashSet<GridCoord>();
        for (var y = 0; y < bh; y++)
        {
            var wy = view.Top + (y - margin + 0.5) * mpp;
            for (var x = 0; x < bw; x++)
            {
                var c = cells.CellAt(view.Left + (x - margin + 0.5) * mpp, wy);
                info[y * bw + x] = c;
                regionsInView.Add(c.Region);
            }
        }

        var needHeight = mode == MapMode.Height || layers.Hillshade;
        var heights = needHeight ? HeightBuffer(world, view, info, bw, bh, margin) : null;

        // Base colour, then the field layers on the land.
        var fields = FieldLayers(layers);
        var patchColour = new Dictionary<long, Rgb>();
        for (var y = 0; y < view.Height; y++)
            for (var x = 0; x < view.Width; x++)
            {
                var c = info[(y + margin) * bw + x + margin];
                Rgb colour;
                if (mode == MapMode.Height)
                {
                    colour = HeightColour(heights![(y + margin) * bw + x + margin]);
                }
                else if (mode == MapMode.None)
                {
                    colour = BlankColour;
                }
                else if (!patchColour.TryGetValue(c.PatchCell.Key, out colour))
                {
                    colour = BaseColour(world, c, mode, layers.FinalBiomes);
                    patchColour[c.PatchCell.Key] = colour;
                }
                if (fields.Length > 0 && !c.Ocean)
                {
                    var wx = view.Left + (x + 0.5) * mpp;
                    var wy = view.Top + (y + 0.5) * mpp;
                    foreach (var kind in fields)
                        colour = colour.Multiply(FieldColour(kind, world.Fields.Sample(kind, wx, wy)));
                }
                if (layers.Hillshade && heights != null)
                    colour = colour.Scale(Shade(heights, bw, x + margin, y + margin, mpp));
                canvas.Set(x, y, colour);
            }

        if (layers.Borders) DrawBorders(canvas, view, cells, info, bw, margin);

        // Rivers of a landlocked region can cross its neighbours, so look one ring further.
        var around = new HashSet<GridCoord>();
        foreach (var r in regionsInView)
            for (var dj = -1; dj <= 1; dj++)
                for (var di = -1; di <= 1; di++)
                    around.Add(r.Offset(di, dj));

        if (layers.Rivers || layers.Lakes)
            foreach (var r in around.OrderBy(r => r.Key))
                DrawWater(world, canvas, view, r, layers);
        if (layers.Travel) DrawTravel(world, canvas, view, around);
        if (layers.Prospects) DrawProspects(world, canvas, view, regionsInView);
        return canvas;
    }

    private static double Vary(uint purpose, GridCoord c, double amount) =>
        1 + (Hashing.Unit(0x51ed270bu, purpose, c.I, c.J) * 2 - 1) * amount;

    private static Rgb BaseColour(World world, CellInfo c, MapMode mode, bool finalBiomes)
    {
        if (c.Ocean)
            return OceanColour.Scale(Vary(1, c.Zone, 0.07) * Vary(2, c.Patch, 0.03));
        if (mode == MapMode.Biomes)
        {
            var biome = BiomeOf(world, c, finalBiomes);
            if (biome is { } b)
            {
                var (r, g, bl) = BiomeRules.Colour(b);
                return new Rgb(r, g, bl); // exact, so a patch can be matched against the legend
            }
        }
        return ElevationColour(world.Cells.ZoneType(c.Zone)).Scale(Vary(4, c.Patch, 0.08));
    }

    private static void DrawBorders(Canvas canvas, MapView view, CellMap cells, CellInfo[] info, int bw, int margin)
    {
        var mpp = view.MetresPerPixel;
        var showZones = cells.ZoneSize / mpp >= 24;
        var showPatches = cells.PatchSize / mpp >= 14;
        // Dots are anchored to world pixels so tiles join.
        var gx0 = (long)Math.Round(view.Left / mpp);
        var gy0 = (long)Math.Round(view.Top / mpp);

        for (var y = 0; y < view.Height; y++)
            for (var x = 0; x < view.Width; x++)
            {
                var bx = x + margin;
                var by = y + margin;
                var here = info[by * bw + bx];
                var left = info[by * bw + bx - 1];
                var up = info[(by - 1) * bw + bx];

                if (here.Ocean != left.Ocean || here.Ocean != up.Ocean)
                {
                    canvas.Set(x, y, CoastColour);
                    continue;
                }
                if (here.Ocean) continue;

                // Region border: a 3 px dot on every point of a 6 px lattice that the border passes.
                var dx = DotOffset(gx0 + x);
                var dy = DotOffset(gy0 + y);
                if (Math.Abs(dx) <= 1 && Math.Abs(dy) <= 1 && RegionEdgeAround(info, bw, bx - dx, by - dy))
                {
                    canvas.Set(x, y, RegionBorderColour);
                    continue;
                }
                if (showZones && (here.Zone != left.Zone || here.Zone != up.Zone))
                    canvas.Set(x, y, canvas.Get(x, y).Scale(0.5));
                else if (showPatches && (here.Patch != left.Patch || here.Patch != up.Patch))
                    canvas.Set(x, y, canvas.Get(x, y).Scale(0.8));
            }
    }

    private const int DotSpacing = 6;

    /// <summary>The offset of a world pixel from the nearest dot lattice line, -3 to 2.</summary>
    private static int DotOffset(long worldPixel)
    {
        var m = (int)(((worldPixel + DotSpacing / 2) % DotSpacing + DotSpacing) % DotSpacing);
        return m - DotSpacing / 2;
    }

    /// <summary>
    /// A region border passes within half a dot spacing of the lattice point. An edge lies on the
    /// left or upper side of its pixel, hence the half pixel shift: a straight border then hits
    /// exactly one lattice line, so the dots neither double nor leave gaps.
    /// </summary>
    private static bool RegionEdgeAround(CellInfo[] info, int bw, int bx, int by)
    {
        bool Edge(int x, int y)
        {
            var here = info[y * bw + x];
            var left = info[y * bw + x - 1];
            var up = info[(y - 1) * bw + x];
            return (!left.Ocean && !here.Ocean && here.Region != left.Region)
                || (!up.Ocean && !here.Ocean && here.Region != up.Region);
        }
        for (var dy = -2; dy <= 3; dy++)
            for (var dx = -2; dx <= 3; dx++)
                if ((dx - 0.5) * (dx - 0.5) + (dy - 0.5) * (dy - 0.5) <= 9 && Edge(bx + dx, by + dy))
                    return true;
        return false;
    }

    private static void DrawWater(World world, Canvas canvas, MapView view, GridCoord region, MapLayers layers)
    {
        var detail = world.DetailOf(new RegionId(region));
        var mpp = view.MetresPerPixel;
        if (layers.Lakes)
            foreach (var lake in detail.Lakes)
            {
                var points = new (double, double)[Lakes.LakeShape.RayCount];
                for (var k = 0; k < points.Length; k++)
                {
                    var p = lake.OutlinePoint(k);
                    points[k] = (view.ToPixelX(p.X), view.ToPixelY(p.Y));
                }
                canvas.FillPolygon(points, WaterColour);
            }

        if (!layers.Rivers) return;
        // Width = scale · zoom · (0.45 + 3 · (flow / 100)^0.4), zoom = (2 km on screen in px) / 7 clamped to [0.45, 2.2].
        var zoom = Math.Clamp(2000 / mpp / 7, 0.45, 2.2);
        var nodes = detail.Rivers.Nodes;
        foreach (var n in nodes)
        {
            if (n.IsRoot || n.Wet) continue; // the stretch over the sea is hidden
            var parent = nodes[n.Parent];
            var width = 0.8 * zoom * (0.45 + 3 * Math.Pow(n.Flow / 100.0, 0.4));
            canvas.Line(
                view.ToPixelX(n.Position.X), view.ToPixelY(n.Position.Y),
                view.ToPixelX(parent.Position.X), view.ToPixelY(parent.Position.Y),
                width, WaterColour);
        }
    }
}
