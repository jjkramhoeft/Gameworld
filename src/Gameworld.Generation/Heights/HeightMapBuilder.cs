using Gameworld.Domain;
using Gameworld.Generation.Basics;

namespace Gameworld.Generation.Heights;

/// <summary>The height map of a region with what the later layers read from it (map-generation.md section 12.8).</summary>
public sealed class HeightMapResult
{
    public required HeightMap Map { get; init; }
    /// <summary>
    /// The mean height over the land cells of each patch of the region, keyed by the key of the
    /// patch's root cell. A patch with no land cell centre in it is absent.
    /// </summary>
    public required IReadOnlyDictionary<long, double> MeanHeightByPatch { get; init; }
    /// <summary>The final bed height of every river node of this region (after levelling and caps).</summary>
    public required double[] BedHeights { get; init; }
    /// <summary>The land cells of this region the pit fill raised.</summary>
    public int FilledCells { get; init; }
    /// <summary>The largest raise of the pit fill on the land of this region.</summary>
    public double MaxRaiseM { get; init; }
}

/// <summary>
/// The height map (map-generation.md section 12): a raster fitted to the map and its rivers.
/// Every river runs downhill to the sea, land rises away from rivers and coasts, and region
/// borders and the boundaries between river systems become ridges. One raster per region; a
/// pure function of the foundation.
/// </summary>
internal static class HeightMapBuilder
{
    private const double HaloM = 24_000;
    private const double BlurRadiusM = 4_000;
    private const double BorderCutM = 10_000;
    private const double DivideCutM = 6_000;
    private const double CoastCutM = 40_000;
    private const double ValleyMaskM = 40;
    private const double NoiseScaleM = 6_000;
    private const float Epsilon = 0.01f;

    /// <summary>A river segment parent → node with the node on land (12.3).</summary>
    private struct Segment
    {
        public double Ax, Ay, Za, Bx, By, Zb, HalfWidth, Buffer;
        /// <summary>The river system: region + tree. 0 is "no label".</summary>
        public int Label;
        /// <summary>The raster's index of the river's region; −1 when that region has no land on the raster.</summary>
        public int Region;
        /// <summary>The river's region is landlocked: its rivers shape the land of any region.</summary>
        public bool Exempt;
    }

    public static HeightMapResult Build(World world, GridCoord region)
    {
        var hp = world.Parameters.Elevation.HeightMap;
        var cells = world.Cells;
        var seed = world.Foundation.Seed.Value;
        var res = hp.ResolutionM;
        var seedR = HeightRules.ReliefSeed(world);

        // ───────────── 12.1 The raster ─────────────

        // The bounding box of the sites of all zone cells drawn in or owned by the region, plus
        // 1.6 × zone size × (1 + zone warp) and the halo.
        double minX = double.PositiveInfinity, minY = double.PositiveInfinity;
        double maxX = double.NegativeInfinity, maxY = double.NegativeInfinity;
        foreach (var zoneCell in cells.ZoneCellsDrawnIn(region).Concat(cells.OwnedZoneCells(region)))
        {
            var site = cells.Zones.Site(zoneCell);
            minX = Math.Min(minX, site.X);
            maxX = Math.Max(maxX, site.X);
            minY = Math.Min(minY, site.Y);
            maxY = Math.Max(maxY, site.Y);
        }
        var margin = 1.6 * cells.ZoneSize * (1 + cells.Zones.Warp) + HaloM;
        // Every raster cell that overlaps the box is part of the raster.
        var i0 = (int)Math.Floor((minX - margin) / res);
        var j0 = (int)Math.Floor((minY - margin) / res);
        var width = (int)Math.Floor((maxX + margin) / res) - i0 + 1;
        var height = (int)Math.Floor((maxY + margin) / res) - j0 + 1;
        var size = width * height;

        // Classify every cell at its centre: ocean or land, region, zone type, patch.
        var regionOf = new int[size];          // −1 = ocean, 0 = this region, 1.. = other regions
        var type = new byte[size];             // 0 = ocean, 1–4 = the land elevation types
        var patchOf = new int[size];           // the index of the patch for land of this region, else −1
        var regions = new List<GridCoord> { region };
        var regionIndex = new Dictionary<long, int> { [region.Key] = 0 };
        var patchKeys = new List<long>();
        var patchIndex = new Dictionary<long, int>();
        var classOf = new Dictionary<long, (int Region, byte Type, int Patch)>();
        for (var j = 0; j < height; j++)
        {
            var y = (j0 + j + 0.5) * res;
            for (var i = 0; i < width; i++)
            {
                var patchCell = cells.Patches.Query((i0 + i + 0.5) * res, y);
                if (!classOf.TryGetValue(patchCell.Key, out var cls))
                {
                    var info = cells.InfoOf(patchCell);
                    if (info.Ocean)
                    {
                        cls = (-1, 0, -1);
                    }
                    else
                    {
                        if (!regionIndex.TryGetValue(info.Region.Key, out var ri))
                        {
                            ri = regions.Count;
                            regions.Add(info.Region);
                            regionIndex[info.Region.Key] = ri;
                        }
                        var pi = -1;
                        if (ri == 0 && !patchIndex.TryGetValue(info.Patch.Key, out pi))
                        {
                            pi = patchKeys.Count;
                            patchKeys.Add(info.Patch.Key);
                            patchIndex[info.Patch.Key] = pi;
                        }
                        cls = (ri, (byte)cells.ZoneType(info.Zone), pi);
                    }
                    classOf[patchCell.Key] = cls;
                }
                var c = j * width + i;
                regionOf[c] = cls.Region;
                type[c] = cls.Type;
                patchOf[c] = cls.Patch;
            }
        }

        // Per-type parameters blended across zone borders: a box blur over land cells only with a
        // radius of 4 km (the window is clipped at the raster edge), then × the multiplier and the relief.
        var slope = new double[size];
        var amplitude = new double[size];
        var ridged = new double[size];
        {
            var counts = new TypeCounts(type, width, height);
            var radius = (int)Math.Floor(BlurRadiusM / res + 1e-9);
            Span<int> n = stackalloc int[5];
            Span<double> hill = [0, hp.Hillslope.FlatLand, hp.Hillslope.Lowland, hp.Hillslope.Hills, hp.Hillslope.Mountains];
            Span<double> amp = stackalloc double[5];
            Span<double> share = stackalloc double[5];
            for (var t = 1; t <= 4; t++)
            {
                amp[t] = HeightRules.NoiseAmplitudeM((ElevationType)t);
                share[t] = HeightRules.RidgedShare((ElevationType)t);
            }
            for (var j = 0; j < height; j++)
                for (var i = 0; i < width; i++)
                {
                    var c = j * width + i;
                    if (type[c] == 0) continue;
                    counts.Window(i, j, radius, n);
                    double total = n[1] + n[2] + n[3] + n[4];
                    var relief = HeightRules.Relief(seedR, hp.MountainBelts, (i0 + i + 0.5) * res, (j0 + j + 0.5) * res);
                    slope[c] = (n[1] * hill[1] + n[2] * hill[2] + n[3] * hill[3] + n[4] * hill[4]) / total
                               * hp.HillslopeMultiplier * relief;
                    amplitude[c] = (n[1] * amp[1] + n[2] * amp[2] + n[3] * amp[3] + n[4] * amp[4]) / total
                                   * hp.DetailNoise * relief;
                    ridged[c] = (n[1] * share[1] + n[2] * share[2] + n[3] * share[3] + n[4] * share[4]) / total;
                }
        }

        // ───────────── 12.2 Step 0: river bed profiles ─────────────

        // The rivers on the raster: those of every region with land on it, and of this region and
        // its 8 grid neighbours in any case (a landlocked neighbour's rivers cross other regions' land).
        // Every region's rivers get that region's own final beds, so two rasters see the same river.
        var beds = world.RiverBeds;
        var riverRegions = new List<GridCoord>(regions);
        for (var dj = -1; dj <= 1; dj++)
            for (var di = -1; di <= 1; di++)
            {
                var other = region.Offset(di, dj);
                if (!regionIndex.ContainsKey(other.Key) && beds.Detail(other).Rivers.Rivers.CrossesNeighbours)
                    riverRegions.Add(other);
            }
        // A fixed order (by key), whatever the raster: ties between segments then fall the same way on every raster.
        riverRegions.Sort();

        var floodplain = beds.Floodplain;
        var segments = new List<Segment>();
        double rasterX0 = i0 * res, rasterY0 = j0 * res, rasterX1 = (i0 + width) * res, rasterY1 = (j0 + height) * res;
        var labelBase = 0;
        foreach (var riverRegion in riverRegions)
        {
            var detail = beds.Detail(riverRegion);
            var nodes = detail.Rivers.Nodes;
            var z = beds.Final(riverRegion);
            var exempt = detail.Rivers.Rivers.CrossesNeighbours;
            var index = regionIndex.GetValueOrDefault(riverRegion.Key, -1);
            for (var k = 0; k < nodes.Count; k++)
            {
                var node = nodes[k];
                if (node.IsRoot || node.Wet) continue;
                var parent = nodes[node.Parent];
                var w = floodplain.HalfWidth((z[node.Parent] + z[k]) / 2, node.Flow);
                var buffer = Math.Max(2 * res, w);
                var reach = w + buffer;
                if (Math.Max(node.Position.X, parent.Position.X) + reach < rasterX0
                    || Math.Min(node.Position.X, parent.Position.X) - reach > rasterX1
                    || Math.Max(node.Position.Y, parent.Position.Y) + reach < rasterY0
                    || Math.Min(node.Position.Y, parent.Position.Y) - reach > rasterY1) continue;
                segments.Add(new Segment
                {
                    Ax = parent.Position.X, Ay = parent.Position.Y, Za = z[node.Parent],
                    Bx = node.Position.X, By = node.Position.Y, Zb = z[k],
                    HalfWidth = w, Buffer = buffer,
                    Label = labelBase + node.Tree + 1,
                    Region = index, Exempt = exempt,
                });
            }
            labelBase += nodes.Count;
        }

        // ───────────── 12.3 Step 1: the envelope ─────────────

        // Nearest river: for every land cell the distance to the nearest segment that may shape
        // it, within w + max(2 cells, w) of the segment.
        var nearest = new double[size];
        Array.Fill(nearest, double.PositiveInfinity);
        var nearCell = 0.75 * res;
        foreach (var s in segments)
        {
            var reach = s.HalfWidth + s.Buffer;
            CellRange(s, reach, out var ia, out var ib, out var ja, out var jb);
            for (var j = ja; j <= jb; j++)
                for (var i = ia; i <= ib; i++)
                {
                    var c = j * width + i;
                    if (type[c] == 0 || !(s.Exempt || regionOf[c] == s.Region)) continue;
                    var d = Geometry.DistanceToSegment((i0 + i + 0.5) * res, (j0 + j + 0.5) * res, s.Ax, s.Ay, s.Bx, s.By, out _);
                    if (d <= reach && d < nearest[c]) nearest[c] = d;
                }
        }

        // Stamp the floodplains: every land cell within w of a segment that shapes it gets the bed
        // height at its projection onto the segment (the lowest if several apply).
        var envelope = new double[size];
        var source = new double[size];
        var label = new int[size];
        for (var c = 0; c < size; c++) envelope[c] = type[c] == 0 ? 0 : double.PositiveInfinity;
        foreach (var s in segments)
        {
            CellRange(s, s.HalfWidth, out var ia, out var ib, out var ja, out var jb);
            for (var j = ja; j <= jb; j++)
                for (var i = ia; i <= ib; i++)
                {
                    var c = j * width + i;
                    if (type[c] == 0 || !(s.Exempt || regionOf[c] == s.Region)) continue;
                    var d = Geometry.DistanceToSegment((i0 + i + 0.5) * res, (j0 + j + 0.5) * res, s.Ax, s.Ay, s.Bx, s.By, out var t);
                    if (d > s.HalfWidth || !(d <= nearest[c] + 1 || d <= nearCell)) continue;
                    var bed = s.Za + t * (s.Zb - s.Za);
                    if (bed < envelope[c])
                    {
                        envelope[c] = bed;
                        source[c] = bed;
                        label[c] = s.Label;
                    }
                }
        }

        // Coast: every ocean cell with a land neighbour (8 neighbours) is a source at 0 without a
        // label. Spread with Dijkstra over land cells; the source height and the label travel with the path.
        var coast = new bool[size];
        var heap = new MinHeap();
        for (var j = 0; j < height; j++)
            for (var i = 0; i < width; i++)
            {
                var c = j * width + i;
                if (type[c] != 0)
                {
                    if (!double.IsPositiveInfinity(envelope[c])) heap.Push(envelope[c], c);
                    continue;
                }
                coast[c] = Geometry.TouchesLand(type, width, height, i, j);
                if (coast[c]) heap.Push(0, c);
            }
        while (heap.Count > 0)
        {
            var (key, c) = heap.Pop();
            if (key > envelope[c]) continue;
            int ci = c % width, cj = c / width;
            var sea = type[c] == 0;
            for (var dj = -1; dj <= 1; dj++)
                for (var di = -1; di <= 1; di++)
                {
                    if (di == 0 && dj == 0) continue;
                    int ni = ci + di, nj = cj + dj;
                    if (ni < 0 || nj < 0 || ni >= width || nj >= height) continue;
                    var nc = nj * width + ni;
                    if (type[nc] == 0) continue;
                    // From an ocean cell, use the slope of the land cell.
                    var mean = sea ? slope[nc] : 0.5 * (slope[c] + slope[nc]);
                    var next = key + res * mean * (di != 0 && dj != 0 ? DetMath.Sqrt2 : 1);
                    if (next < envelope[nc])
                    {
                        envelope[nc] = next;
                        source[nc] = sea ? 0 : source[c];
                        label[nc] = sea ? 0 : label[c];
                        heap.Push(next, nc);
                    }
                }
        }

        // ───────────── 12.4 Step 2: ridges ─────────────

        // Land cells that touch (4 neighbours) land of another region, and labelled land cells
        // that touch a land cell with a different river-system label.
        var isBorder = new bool[size];
        var isDivide = new bool[size];
        for (var j = 0; j < height; j++)
            for (var i = 0; i < width; i++)
            {
                var c = j * width + i;
                if (type[c] == 0) continue;
                for (var side = 0; side < 4; side++)
                {
                    var ni = i + (side == 0 ? -1 : side == 1 ? 1 : 0);
                    var nj = j + (side == 2 ? -1 : side == 3 ? 1 : 0);
                    if (ni < 0 || nj < 0 || ni >= width || nj >= height) continue;
                    var nc = nj * width + ni;
                    if (type[nc] == 0) continue;
                    if (regionOf[nc] != regionOf[c]) isBorder[c] = true;
                    if (label[c] != 0 && label[nc] != 0 && label[nc] != label[c]) isDivide[c] = true;
                }
            }
        var dBorder = Chamfer(isBorder, width, height, res);
        var dDivide = Chamfer(isDivide, width, height, res);
        var dCoast = Chamfer(coast, width, height, res);

        // ───────────── 12.5 Step 3: assemble and add detail ─────────────

        var largest = 0.0;
        var anyReached = false;
        for (var c = 0; c < size; c++)
            if (type[c] != 0 && !double.IsPositiveInfinity(envelope[c]) && (!anyReached || envelope[c] > largest))
            {
                largest = envelope[c];
                anyReached = true;
            }

        var seedN = Hashing.Hash4(seed, 0x7e12, 0, 0);
        var heights = new float[size];
        for (var j = 0; j < height; j++)
            for (var i = 0; i < width; i++)
            {
                var c = j * width + i;
                if (type[c] == 0)
                {
                    heights[c] = (float)-(2 + hp.SeaDepthM * Smooth(dCoast[c] / CoastCutM));
                    continue;
                }
                var e = envelope[c];
                var src = source[c];
                if (double.IsPositiveInfinity(e))
                {
                    // A land cell the envelope never reached uses the largest E on the raster; it drains to nothing, so its source height is taken as 0.
                    e = largest;
                    src = 0;
                }
                var m = Smooth((e - src) / ValleyMaskM);
                var h = e;
                if (dBorder[c] < BorderCutM)
                {
                    var f = 1 - dBorder[c] / BorderCutM;
                    h += hp.RegionRidgeM * f * f * m;
                }
                if (dDivide[c] < DivideCutM)
                {
                    var f = 1 - dDivide[c] / DivideCutM;
                    h += hp.BasinDivideM * f * f * m;
                }
                var gain = amplitude[c] * m;
                if (gain != 0)
                {
                    var nx = (i0 + i + 0.5) * res / NoiseScaleM;
                    var ny = (j0 + j + 0.5) * res / NoiseScaleM;
                    var r = ridged[c];
                    var noise = 0.0;
                    if (r < 1) noise += (1 - r) * Noise.Fbm(seedN, nx, ny, 4, 2.1, 0.5);
                    if (r > 0) noise += r * Noise.Ridged(seedN, nx, ny, 4, 2.1, 0.5);
                    h += gain * (0.5 * noise + 0.5);
                }
                // From here on heights are 32-bit floats.
                heights[c] = (float)h;
            }

        // ───────────── 12.6 Step 4: burn the rivers in ─────────────

        // Under the same nearest-river and own-region rules as the stamping: a trench along the
        // channel and a linear ramp from the floodplain edge up to the surrounding surface.
        foreach (var s in segments)
        {
            var reach = s.HalfWidth + s.Buffer;
            CellRange(s, reach, out var ia, out var ib, out var ja, out var jb);
            for (var j = ja; j <= jb; j++)
                for (var i = ia; i <= ib; i++)
                {
                    var c = j * width + i;
                    if (type[c] == 0 || !(s.Exempt || regionOf[c] == s.Region)) continue;
                    var d = Geometry.DistanceToSegment((i0 + i + 0.5) * res, (j0 + j + 0.5) * res, s.Ax, s.Ay, s.Bx, s.By, out var t);
                    if (d > reach || !(d <= nearest[c] + 1 || d <= nearCell)) continue;
                    var bed = s.Za + t * (s.Zb - s.Za);
                    double h = heights[c];
                    double target;
                    if (d <= s.HalfWidth) target = bed - hp.ChannelDepthM;
                    else if (h > bed) target = bed + (h - bed) * (d - s.HalfWidth) / s.Buffer;
                    else continue;
                    var cut = (float)target;
                    if (cut < heights[c]) heights[c] = cut;
                }
        }

        // ───────────── 12.7 Step 5: fill pits ─────────────

        // ε-Priority-Flood per region. One heap serves every region on the raster: a land cell is
        // only ever raised from the sea or from land of its own region, so the regions do not mix.
        // Seeds keep their heights: the ocean cells next to land (8 neighbours), and the land cells
        // on the raster edge or touching (4 neighbours, as for the border ridge) another region's land.
        var visited = new bool[size];
        heap.Clear();
        for (var j = 0; j < height; j++)
            for (var i = 0; i < width; i++)
            {
                var c = j * width + i;
                var isSeed = type[c] == 0
                    ? coast[c]
                    : isBorder[c] || i == 0 || j == 0 || i == width - 1 || j == height - 1;
                if (!isSeed) continue;
                visited[c] = true;
                heap.Push(heights[c], c);
            }
        var filled = 0;
        var maxRaise = 0.0;
        while (heap.Count > 0)
        {
            var (_, c) = heap.Pop();
            int ci = c % width, cj = c / width;
            var floor = heights[c] + Epsilon;   // compared and raised in 32-bit precision
            var from = regionOf[c];
            for (var dj = -1; dj <= 1; dj++)
                for (var di = -1; di <= 1; di++)
                {
                    if (di == 0 && dj == 0) continue;
                    int ni = ci + di, nj = cj + dj;
                    if (ni < 0 || nj < 0 || ni >= width || nj >= height) continue;
                    var nc = nj * width + ni;
                    if (visited[nc] || type[nc] == 0 || (from >= 0 && regionOf[nc] != from)) continue;
                    visited[nc] = true;
                    if (heights[nc] < floor)
                    {
                        if (regionOf[nc] == 0)
                        {
                            filled++;
                            maxRaise = Math.Max(maxRaise, (double)floor - heights[nc]);
                        }
                        heights[nc] = floor;
                    }
                    heap.Push(heights[nc], nc);
                }
        }

        // ───────────── 12.8 Results ─────────────

        // The mean height of every patch of the region, over its land cells.
        var sums = new double[patchKeys.Count];
        var cellCounts = new int[patchKeys.Count];
        for (var c = 0; c < size; c++)
        {
            var p = patchOf[c];
            if (p < 0) continue;
            sums[p] += heights[c];
            cellCounts[p]++;
        }
        var means = new Dictionary<long, double>(patchKeys.Count);
        for (var p = 0; p < patchKeys.Count; p++) means[patchKeys[p]] = sums[p] / cellCounts[p];

        return new HeightMapResult
        {
            Map = new HeightMap(i0, j0, width, height, res, heights),
            MeanHeightByPatch = means,
            BedHeights = beds.Final(region),
            FilledCells = filled,
            MaxRaiseM = maxRaise,
        };

        // The raster cells whose centre lies within reach of the segment's bounding box.
        void CellRange(in Segment s, double reach, out int ia, out int ib, out int ja, out int jb)
        {
            ia = Math.Max(0, (int)Math.Ceiling((Math.Min(s.Ax, s.Bx) - reach) / res - 0.5) - i0);
            ib = Math.Min(width - 1, (int)Math.Floor((Math.Max(s.Ax, s.Bx) + reach) / res - 0.5) - i0);
            ja = Math.Max(0, (int)Math.Ceiling((Math.Min(s.Ay, s.By) - reach) / res - 0.5) - j0);
            jb = Math.Min(height - 1, (int)Math.Floor((Math.Max(s.Ay, s.By) + reach) / res - 0.5) - j0);
        }
    }

    /// <summary>smooth(t) = t²(3 − 2t) clamped to [0, 1].</summary>
    private static double Smooth(double t)
    {
        if (t <= 0) return 0;
        if (t >= 1) return 1;
        return t * t * (3 - 2 * t);
    }

    /// <summary>
    /// An 8-neighbour chamfer distance to the nearest source cell, starting at half a cell
    /// (12.4): two passes over the raster. Infinite where there is no source.
    /// </summary>
    private static double[] Chamfer(bool[] sources, int width, int height, double res)
    {
        var d = new double[sources.Length];
        var diagonal = DetMath.Sqrt2 * res;
        for (var c = 0; c < d.Length; c++) d[c] = sources[c] ? 0.5 * res : double.PositiveInfinity;
        for (var j = 0; j < height; j++)
            for (var i = 0; i < width; i++)
            {
                var c = j * width + i;
                var best = d[c];
                if (i > 0) best = Math.Min(best, d[c - 1] + res);
                if (j > 0)
                {
                    best = Math.Min(best, d[c - width] + res);
                    if (i > 0) best = Math.Min(best, d[c - width - 1] + diagonal);
                    if (i < width - 1) best = Math.Min(best, d[c - width + 1] + diagonal);
                }
                d[c] = best;
            }
        for (var j = height - 1; j >= 0; j--)
            for (var i = width - 1; i >= 0; i--)
            {
                var c = j * width + i;
                var best = d[c];
                if (i < width - 1) best = Math.Min(best, d[c + 1] + res);
                if (j < height - 1)
                {
                    best = Math.Min(best, d[c + width] + res);
                    if (i < width - 1) best = Math.Min(best, d[c + width + 1] + diagonal);
                    if (i > 0) best = Math.Min(best, d[c + width - 1] + diagonal);
                }
                d[c] = best;
            }
        return d;
    }
}
