using Gameworld.Domain;
using Gameworld.Generation.Basics;
using Gameworld.Generation.Heights;
using Gameworld.Generation.Resources;
using Gameworld.Generation.Stages;

namespace Gameworld.Generation.Travel;

/// <summary>A link between two neighbouring cost cells along a navigable river, remembering the river node.</summary>
internal readonly record struct RiverLink(int Ia, int Ja, int Ib, int Jb, int Node);

/// <summary>
/// The travel cost grid of one region (map-generation.md section 16.1): world-aligned 2 km
/// cells, each classified at its centre. It reads the region's Detailed data only.
/// </summary>
internal sealed class CostGrid
{
    public const double CellSize = 2000;
    public const byte NotOwn = 0, Sea = 1, Land = 2;

    public required GridCoord Region { get; init; }
    /// <summary>The global cell coordinate of the first column and row.</summary>
    public required int I0 { get; init; }
    public required int J0 { get; init; }
    public required int Width { get; init; }
    public required int Height { get; init; }
    /// <summary>NotOwn for a cell of another region; Sea or Land for a cell of this region.</summary>
    public required byte[] Kind { get; init; }
    /// <summary>The travel cost per km: sea lane cost for sea cells, road cost for land cells.</summary>
    public required float[] Cost { get; init; }
    /// <summary>The estimated height of land cells in metres.</summary>
    public required float[] HeightM { get; init; }
    public required List<RiverLink> Links { get; init; }

    public static CostGrid Build(World world, DetailWork detail)
    {
        var cells = world.Cells;
        var region = detail.Region;
        var hp = world.Parameters.Elevation.HeightMap;

        // The bounding box of the region's zone cell sites, with room for the cells around them.
        var margin = 2 * cells.ZoneSize * (1 + cells.Zones.Warp) + 2 * cells.PatchSize;
        double minX = double.PositiveInfinity, minY = double.PositiveInfinity, maxX = double.NegativeInfinity, maxY = double.NegativeInfinity;
        foreach (var z in cells.ZoneCellsDrawnIn(region))
        {
            var s = cells.Zones.Site(z);
            minX = Math.Min(minX, s.X); maxX = Math.Max(maxX, s.X);
            minY = Math.Min(minY, s.Y); maxY = Math.Max(maxY, s.Y);
        }
        if (double.IsInfinity(minX))
        {
            var s = cells.Regions.Site(region);
            minX = maxX = s.X;
            minY = maxY = s.Y;
        }
        var i0 = (int)Math.Floor((minX - margin) / CellSize);
        var j0 = (int)Math.Floor((minY - margin) / CellSize);
        var w = (int)Math.Floor((maxX + margin) / CellSize) - i0 + 1;
        var h = (int)Math.Floor((maxY + margin) / CellSize) - j0 + 1;

        // Classify the box plus one cell on every side, so the shore is known at the edge.
        var ew = w + 2;
        var eh = h + 2;
        var ocean = new bool[ew * eh];
        var own = new bool[ew * eh];
        var patch = new GridCoord[ew * eh];
        var zone = new GridCoord[ew * eh];
        for (var y = 0; y < eh; y++)
            for (var x = 0; x < ew; x++)
            {
                var info = cells.CellAt((i0 - 1 + x + 0.5) * CellSize, (j0 - 1 + y + 0.5) * CellSize);
                var e = y * ew + x;
                ocean[e] = info.Ocean;
                own[e] = info.Region == region;
                patch[e] = info.Patch;
                zone[e] = info.Zone;
            }

        var kind = new byte[w * h];
        var cost = new float[w * h];
        var height = new float[w * h];
        var slope = new double[w * h];
        for (var y = 0; y < h; y++)
            for (var x = 0; x < w; x++)
            {
                var e = (y + 1) * ew + x + 1;
                if (!own[e]) continue;
                var c = y * w + x;
                var shore = false;
                for (var dy = -1; dy <= 1 && !shore; dy++)
                    for (var dx = -1; dx <= 1; dx++)
                        if (ocean[e + dy * ew + dx] != ocean[e])
                        {
                            shore = true;
                            break;
                        }

                if (ocean[e])
                {
                    kind[c] = Sea;
                    cost[c] = shore ? 0.6f : 0.3f; // lanes keep off capes
                    continue;
                }

                kind[c] = Land;
                var cx = (i0 + x + 0.5) * CellSize;
                var cy = (j0 + y + 0.5) * CellSize;
                var type = cells.ZoneType(zone[e]);
                var zoneFactor = type switch
                {
                    ElevationType.FlatLand => 1,
                    ElevationType.Lowland => 1.15,
                    ElevationType.Hills => 1.8,
                    _ => 4,
                };
                var biome = detail.Find(patch[e])?.Biome?.Biome;
                var biomeFactor = biome is { } b ? BiomeFactors.Of(b).RoadCost : 1;
                cost[c] = (float)(zoneFactor * biomeFactor * (detail.InLake(cx, cy) ? 8 : 1));
                // The unblurred hillslope: the type's hillslope × the multiplier × the relief at the point.
                slope[c] = hp.Hillslope[type] * hp.HillslopeMultiplier * HeightRules.Relief(world, cx, cy);
                height[c] = float.NaN;
                if (shore) height[c] = (float)(slope[c] * 1000);
            }

        // Estimated height: river beds with the simple rise seed the cells that hold river nodes.
        var nodes = detail.Rivers.Nodes;
        var beds = HeightRules.SimpleBeds(world, detail, levelling: false);
        for (var k = 0; k < nodes.Count; k++)
        {
            if (nodes[k].Wet) continue;
            var x = (int)Math.Floor(nodes[k].Position.X / CellSize) - i0;
            var y = (int)Math.Floor(nodes[k].Position.Y / CellSize) - j0;
            if (x < 0 || y < 0 || x >= w || y >= h) continue;
            var c = y * w + x;
            if (kind[c] != Land) continue;
            if (float.IsNaN(height[c]) || beds[k] < height[c]) height[c] = (float)beds[k];
        }

        // Spread over the region's own land cells.
        var dist = new double[w * h];
        var heap = new MinHeap();
        for (var c = 0; c < dist.Length; c++)
        {
            dist[c] = double.PositiveInfinity;
            if (kind[c] == Land && !float.IsNaN(height[c]))
            {
                dist[c] = height[c];
                heap.Push(dist[c], c);
            }
        }
        while (heap.Count > 0)
        {
            var (d, c) = heap.Pop();
            if (d > dist[c]) continue;
            var x = c % w;
            var y = c / w;
            for (var dy = -1; dy <= 1; dy++)
                for (var dx = -1; dx <= 1; dx++)
                {
                    if (dx == 0 && dy == 0) continue;
                    var nx = x + dx;
                    var ny = y + dy;
                    if (nx < 0 || ny < 0 || nx >= w || ny >= h) continue;
                    var n = ny * w + nx;
                    if (kind[n] != Land) continue;
                    var length = dx != 0 && dy != 0 ? CellSize * 1.4142135623730951 : CellSize;
                    var nd = d + length * 0.5 * (slope[c] + slope[n]);
                    if (nd < dist[n])
                    {
                        dist[n] = nd;
                        heap.Push(nd, n);
                    }
                }
        }
        for (var c = 0; c < dist.Length; c++)
            if (kind[c] == Land)
                height[c] = double.IsPositiveInfinity(dist[c]) ? 150f : (float)dist[c]; // land that nothing reaches

        // Navigable river links: walk each navigable segment to the parent in quarter-cell steps.
        var links = new List<RiverLink>();
        var navigable = 8 * Math.Max(1, world.Parameters.Detail.Rivers.AttractorsPerPatchCell);
        var seen = new HashSet<(int, int, int, int)>();
        for (var k = 0; k < nodes.Count; k++)
        {
            var n = nodes[k];
            if (n.IsRoot || n.Flow < navigable) continue;
            var parent = nodes[n.Parent].Position;
            var length = n.Position.DistanceTo(parent);
            var steps = Math.Max(1, (int)Math.Ceiling(length / (CellSize / 4)));
            var pi = (int)Math.Floor(n.Position.X / CellSize);
            var pj = (int)Math.Floor(n.Position.Y / CellSize);
            for (var s = 1; s <= steps; s++)
            {
                var f = (double)s / steps;
                var ci = (int)Math.Floor((n.Position.X + (parent.X - n.Position.X) * f) / CellSize);
                var cj = (int)Math.Floor((n.Position.Y + (parent.Y - n.Position.Y) * f) / CellSize);
                if (ci == pi && cj == pj) continue;
                // A link between two sea cells is dropped: boats need no river there.
                var bothSea = cells.CellAt((pi + 0.5) * CellSize, (pj + 0.5) * CellSize).Ocean
                           && cells.CellAt((ci + 0.5) * CellSize, (cj + 0.5) * CellSize).Ocean;
                if (!bothSea && Math.Abs(ci - pi) <= 1 && Math.Abs(cj - pj) <= 1
                    && seen.Add(pi < ci || (pi == ci && pj < cj) ? (pi, pj, ci, cj) : (ci, cj, pi, pj)))
                    links.Add(new RiverLink(pi, pj, ci, cj, k));
                pi = ci;
                pj = cj;
            }
        }

        return new CostGrid
        {
            Region = region,
            I0 = i0,
            J0 = j0,
            Width = w,
            Height = h,
            Kind = kind,
            Cost = cost,
            HeightM = height,
            Links = links,
        };
    }
}
