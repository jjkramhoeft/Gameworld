using Gameworld.Domain;
using Gameworld.Generation.Cells;
using Gameworld.Generation.Stages;

namespace Gameworld.Generation.Rendering;

public static partial class MapRenderer
{
    private static readonly Rgb RoadColour = new(112, 62, 30);
    private static readonly Rgb RiverRouteColour = new(206, 236, 255);
    private static readonly Rgb SeaLaneColour = new(238, 242, 250);
    private static readonly Rgb MarkerEdge = new(24, 20, 20);

    private static readonly (double HeightM, Rgb Colour)[] HypsometricStops =
    [
        (0, new Rgb(72, 128, 76)),
        (60, new Rgb(104, 152, 84)),
        (180, new Rgb(150, 176, 98)),
        (400, new Rgb(206, 194, 122)),
        (700, new Rgb(182, 142, 94)),
        (1100, new Rgb(150, 120, 98)),
        (1600, new Rgb(168, 160, 156)),
        (2400, new Rgb(246, 246, 246)),
    ];

    /// <summary>The biome of a land patch: the estimated one of the Detailed stage, or the final one of the Elevated stage.</summary>
    private static Biome? BiomeOf(World world, CellInfo c, bool finalBiomes)
    {
        var region = new RegionId(c.Region);
        if (finalBiomes && world.ElevationOf(region).Find(c.Patch) is { } final) return final.Biome.Biome;
        return world.DetailOf(region).Find(c.Patch)?.Biome?.Biome;
    }

    /// <summary>The height of every pixel of the classified buffer, read from the height map of the pixel's region.</summary>
    private static float[] HeightBuffer(World world, MapView view, CellInfo[] info, int bw, int bh, int margin)
    {
        var heights = new float[bw * bh];
        var mpp = view.MetresPerPixel;
        GridCoord? lastRegion = null;
        HeightMap? map = null;
        for (var y = 0; y < bh; y++)
        {
            var wy = view.Top + (y - margin + 0.5) * mpp;
            for (var x = 0; x < bw; x++)
            {
                var region = info[y * bw + x].Region;
                if (region != lastRegion)
                {
                    map = world.ElevationOf(new RegionId(region)).Heights.Map;
                    lastRegion = region;
                }
                heights[y * bw + x] = (float)map!.Sample(new WorldPoint(view.Left + (x - margin + 0.5) * mpp, wy));
            }
        }
        return heights;
    }

    /// <summary>A hypsometric tint, linear between the stops; the sea from the shore colour to the colour at 1200 m depth.</summary>
    private static Rgb HeightColour(float h)
    {
        if (h < 0) return Rgb.Lerp(new Rgb(84, 150, 200), new Rgb(22, 46, 100), -h / 1200.0);
        for (var i = 1; i < HypsometricStops.Length; i++)
            if (h <= HypsometricStops[i].HeightM)
            {
                var (h0, c0) = HypsometricStops[i - 1];
                var (h1, c1) = HypsometricStops[i];
                return Rgb.Lerp(c0, c1, (h - h0) / (h1 - h0));
            }
        return HypsometricStops[^1].Colour;
    }

    /// <summary>
    /// Hillshade from the height differences to the left and upper pixel, exaggerated 4 ×, lit
    /// from the north-west at 45°: the factor the pixel is scaled by.
    /// </summary>
    private static double Shade(float[] heights, int bw, int x, int y, double mpp)
    {
        var h = heights[y * bw + x];
        var dzdx = (h - heights[y * bw + x - 1]) * 4 / mpp;
        var dzdy = (h - heights[(y - 1) * bw + x]) * 4 / mpp;
        var length = Math.Sqrt(dzdx * dzdx + dzdy * dzdy + 1);
        // The light comes from (−1, −1, √2) / 2: north-west, 45° above the horizon.
        var shade = (0.5 * dzdx + 0.5 * dzdy + 0.7071067811865476) / length;
        return Math.Clamp(1 + 1.25 * (shade - 0.7071067811865476), 0.35, 1.45);
    }

    private static void DrawTravel(World world, Canvas canvas, MapView view, HashSet<GridCoord> regions)
    {
        var zoom = Math.Clamp(2000 / view.MetresPerPixel / 7, 0.6, 2.2);
        foreach (var region in regions.OrderBy(r => r.Key))
            foreach (var link in world.SettlementOf(new RegionId(region)).Links)
                foreach (var stretch in link.Route)
                {
                    var (colour, width) = stretch.Kind switch
                    {
                        TravelLinkKind.Road => (RoadColour, 1.5 * zoom),
                        TravelLinkKind.RiverRoute => (RiverRouteColour, 1.1 * zoom),
                        _ => (SeaLaneColour, 1.1 * zoom),
                    };
                    for (var i = 1; i < stretch.Points.Count; i++)
                    {
                        var a = stretch.Points[i - 1];
                        var b = stretch.Points[i];
                        canvas.Line(view.ToPixelX(a.X), view.ToPixelY(a.Y), view.ToPixelX(b.X), view.ToPixelY(b.Y),
                            width, colour, stretch.Kind == TravelLinkKind.Road ? 1 : 0.85);
                    }
                }
    }

    /// <summary>A diamond for a city Prospect and a dot for a town Prospect, coloured by score. Only Prospects on show are drawn.</summary>
    private static void DrawProspects(World world, Canvas canvas, MapView view, HashSet<GridCoord> regions)
    {
        var minScore = world.Parameters.Travel.MinProspectScore;
        var zoom = Math.Clamp(2000 / view.MetresPerPixel / 7, 0.7, 1.8);
        foreach (var region in regions.OrderBy(r => r.Key))
            foreach (var prospect in world.PlanOf(new RegionId(region)).Prospects)
            {
                if (prospect.Score < minScore) continue;
                var x = view.ToPixelX(prospect.Spot.X);
                var y = view.ToPixelY(prospect.Spot.Y);
                var colour = ScoreColour(prospect.Score);
                if (prospect.ForCity)
                {
                    canvas.Diamond(x, y, 7.5 * zoom, MarkerEdge);
                    canvas.Diamond(x, y, 5.5 * zoom, colour);
                }
                else
                {
                    canvas.Disc(x, y, 4.2 * zoom, MarkerEdge);
                    canvas.Disc(x, y, 2.9 * zoom, colour);
                }
            }
    }

    /// <summary>Pale yellow at a score of 50, through orange, to deep red at 90.</summary>
    public static Rgb ScoreColour(double score)
    {
        var t = Math.Clamp((score - 50) / 40, 0, 1);
        return t < 0.5
            ? Rgb.Lerp(new Rgb(255, 240, 150), new Rgb(250, 150, 50), t * 2)
            : Rgb.Lerp(new Rgb(250, 150, 50), new Rgb(190, 20, 30), t * 2 - 1);
    }
}
