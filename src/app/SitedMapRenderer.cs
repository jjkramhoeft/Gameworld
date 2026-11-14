using Gameworld.Domain.Model;

namespace Gameworld.App;

/// <summary>
/// Draws regions as they are Sited. Every pixel belongs to its nearest patch cell site, which
/// gives the nested cells: a zone cell is the patch cells of its sites, and a region is its
/// zone cells.
/// </summary>
internal static class SitedMapRenderer
{
    private static readonly Rgb RegionSiteColor = new(220, 30, 30);
    private static readonly Rgb ZoneSiteColor = new(30, 30, 30);

    private static Rgb ColorOf(ElevationType elevation) => elevation switch
    {
        ElevationType.Ocean => new(70, 120, 190),
        ElevationType.Flat => new(190, 215, 150),
        ElevationType.Lowland => new(140, 185, 110),
        ElevationType.Hills => new(200, 175, 120),
        ElevationType.Mountains => new(150, 145, 140),
        _ => throw new ArgumentOutOfRangeException(nameof(elevation), elevation, null),
    };

    private readonly record struct PatchSite(WorldPoint Site, ZoneCell Zone);

    /// <summary>
    /// The image of a square block of regions, three bytes per pixel, row by row from the top.
    /// North (higher Y) is up.
    /// </summary>
    /// <param name="regions">The regions to draw. Include one ring more than is shown, so the cells at the edge are right.</param>
    /// <param name="centre">The region in the middle of the image.</param>
    /// <param name="rings">How many rings of regions around the centre the image shows.</param>
    /// <param name="gridSizeKm">The side length of the region grid.</param>
    /// <param name="pixels">The width and height of the image.</param>
    public static byte[] Render(IReadOnlyCollection<SitedRegion> regions, RegionId centre, int rings, double gridSizeKm, int pixels)
    {
        var left = (centre.X - rings) * gridSizeKm;
        var bottom = (centre.Y - rings) * gridSizeKm;
        var span = (2 * rings + 1) * gridSizeKm;

        var sites = regions
            .SelectMany(region => region.ZoneCells)
            .SelectMany(zone => zone.PatchCells.Select(patch => new PatchSite(patch.Site, zone)))
            .ToArray();
        var lookup = new NearestSiteLookup(sites, bucketSize: Math.Sqrt(gridSizeKm * gridSizeKm * regions.Count / sites.Length));

        var owner = new int[pixels * pixels];
        Parallel.For(0, pixels, row =>
        {
            var y = bottom + (pixels - row - 0.5) / pixels * span;
            for (var column = 0; column < pixels; column++)
                owner[row * pixels + column] = lookup.Nearest(new(left + (column + 0.5) / pixels * span, y));
        });

        var rgb = new byte[pixels * pixels * 3];
        for (var row = 0; row < pixels; row++)
            for (var column = 0; column < pixels; column++)
            {
                var own = owner[row * pixels + column];
                var shade = 1.0;
                if (column + 1 < pixels)
                    shade = Math.Min(shade, BorderShade(sites[own], sites[owner[row * pixels + column + 1]]));
                if (row + 1 < pixels)
                    shade = Math.Min(shade, BorderShade(sites[own], sites[owner[(row + 1) * pixels + column]]));

                var color = ColorOf(sites[own].Zone.InitialElevation);
                Set(rgb, pixels, column, row, new((byte)(color.R * shade), (byte)(color.G * shade), (byte)(color.B * shade)));
            }

        void Dot(WorldPoint point, int radius, Rgb color)
        {
            var centreColumn = (int)((point.X - left) / span * pixels);
            var centreRow = pixels - 1 - (int)((point.Y - bottom) / span * pixels);
            for (var dy = -radius; dy <= radius; dy++)
                for (var dx = -radius; dx <= radius; dx++)
                    if (dx * dx + dy * dy <= radius * radius)
                        Set(rgb, pixels, centreColumn + dx, centreRow + dy, color);
        }

        var scale = Math.Max(1, pixels / 500);
        foreach (var region in regions)
        {
            foreach (var zone in region.ZoneCells)
                Dot(zone.Site, scale, ZoneSiteColor);
            Dot(region.Site, 2 * scale + 1, RegionSiteColor);
        }
        return rgb;
    }

    /// <summary>Black between regions, dark between zone cells, faint between patch cells.</summary>
    private static double BorderShade(PatchSite own, PatchSite other)
    {
        if (own.Site == other.Site) return 1;
        if (own.Zone.Id.Region != other.Zone.Id.Region) return 0;
        return own.Zone.Id != other.Zone.Id ? 0.45 : 0.85;
    }

    private static void Set(byte[] rgb, int pixels, int column, int row, Rgb color)
    {
        if (column < 0 || row < 0 || column >= pixels || row >= pixels)
            return;
        var index = (row * pixels + column) * 3;
        rgb[index] = color.R;
        rgb[index + 1] = color.G;
        rgb[index + 2] = color.B;
    }

    /// <summary>Finds the nearest site to a point, with the sites sorted into square buckets.</summary>
    private sealed class NearestSiteLookup
    {
        private readonly PatchSite[] sites;
        private readonly double bucketSize;
        private readonly Dictionary<(int X, int Y), List<int>> buckets = [];

        public NearestSiteLookup(PatchSite[] sites, double bucketSize)
        {
            this.sites = sites;
            this.bucketSize = bucketSize;
            for (var index = 0; index < sites.Length; index++)
            {
                var key = BucketOf(sites[index].Site);
                if (!buckets.TryGetValue(key, out var bucket))
                    buckets[key] = bucket = [];
                bucket.Add(index);
            }
        }

        public int Nearest(WorldPoint point)
        {
            var (x, y) = BucketOf(point);
            for (var reach = 1; ; reach++)
            {
                var nearest = -1;
                var distance = double.MaxValue;
                for (var by = y - reach; by <= y + reach; by++)
                    for (var bx = x - reach; bx <= x + reach; bx++)
                        if (buckets.TryGetValue((bx, by), out var bucket))
                            foreach (var index in bucket)
                            {
                                var candidate = point.DistanceTo(sites[index].Site);
                                if (candidate < distance)
                                {
                                    distance = candidate;
                                    nearest = index;
                                }
                            }

                // Everything within reach buckets of the point has been searched.
                if (nearest >= 0 && distance <= reach * bucketSize)
                    return nearest;
                if (reach > 1_000)
                    throw new InvalidOperationException("There are no sites to draw.");
            }
        }

        private (int X, int Y) BucketOf(WorldPoint point) =>
            ((int)Math.Floor(point.X / bucketSize), (int)Math.Floor(point.Y / bucketSize));
    }
}
