using System.Collections.Immutable;
using Gameworld.Domain.Model;

namespace Gameworld.Domain.Generation;

/// <summary>
/// Sites regions: places the three levels of nested Voronoi sites and rolls what the zones
/// start out as. Needs the foundation only.
///
/// The levels are nested by nearest site. A zone cell site lies in the Voronoi cell of its
/// region site, and a patch cell site lies in the Voronoi cell of its zone cell site, where
/// the zone cells of all regions form one Voronoi diagram.
///
/// Only +, -, *, / and square root are used on coordinates, so the result is the same on
/// every platform.
/// </summary>
internal sealed class Siting
{
    private readonly WorldSeed seed;
    private readonly ParameterSet parameters;
    private readonly SitingParameters siting;
    private readonly double size;

    /// <summary>No point of a region's Voronoi cell is farther than this from the region site.</summary>
    private readonly double reach;

    private readonly Dictionary<RegionId, ZoneLayer> zoneLayers = [];

    /// <summary>The Voronoi cell of a region site, and the zone cell sites placed in it.</summary>
    private sealed record ZoneLayer(ConvexPolygon Cell, WorldPoint[] Sites);

    public Siting(Foundation foundation)
    {
        seed = foundation.Seed;
        parameters = foundation.Parameters;
        siting = parameters.Siting;
        Validate(parameters);
        size = siting.RegionGridSizeKm;
        reach = (0.5 + siting.RegionJitter) * Math.Sqrt(2) * size;
    }

    public SitedRegion Site(RegionId region)
    {
        var layer = ZoneLayerOf(region);
        var isOceanSeed = PickOceanSeeds(region, layer.Sites.Length);

        var zoneCells = ImmutableArray.CreateBuilder<ZoneCell>(layer.Sites.Length);
        var oceanSeeds = ImmutableArray.CreateBuilder<OceanSeedQuota>();
        for (var index = 0; index < layer.Sites.Length; index++)
        {
            var id = new ZoneCellId(region, index);
            var site = layer.Sites[index];
            var elevation = isOceanSeed[index] ? ElevationType.Ocean : PickElevation(id);
            zoneCells.Add(new(id, site, elevation, PickBedrock(id, elevation), isOceanSeed[index], SitePatchCells(id, site)));

            if (isOceanSeed[index])
                oceanSeeds.Add(new(
                    id,
                    Links: Random(id, GenerationPurpose.OceanLink).NextDouble() < parameters.Outline.OceanLinkChance,
                    Expansions: Random(id, GenerationPurpose.OceanExpansionCount).NextInt(parameters.Outline.ExpansionsPerOceanSeed)));
        }

        var zoneMergers = Random(region, GenerationPurpose.ZoneMergerCount).NextInt(parameters.Outline.ZoneMergersPerRegion);
        return new(region, RegionSite(region), zoneCells.MoveToImmutable(), new(oceanSeeds.ToImmutable(), zoneMergers));
    }

    // ---- Region level -------------------------------------------------------------------

    /// <summary>The site of a region: the centre of its grid cell, moved by the jitter.</summary>
    private WorldPoint RegionSite(RegionId region)
    {
        var random = Random(region, GenerationPurpose.RegionSite);
        var x = region.X + 0.5 + (2 * random.NextDouble() - 1) * siting.RegionJitter;
        var y = region.Y + 0.5 + (2 * random.NextDouble() - 1) * siting.RegionJitter;
        return new(x * size, y * size);
    }

    private ConvexPolygon RegionCell(RegionId region) =>
        VoronoiCell(RegionSite(region), bound: reach, spread: 0, id => [RegionSite(id)]);

    // ---- Zone level ---------------------------------------------------------------------

    private ZoneLayer ZoneLayerOf(RegionId region)
    {
        if (zoneLayers.TryGetValue(region, out var layer))
            return layer;

        var cell = RegionCell(region);
        var count = Random(region, GenerationPurpose.ZoneCellCount).NextInt(siting.ZoneCellsPerRegion);
        var sites = Scatter(cell, count, siting.ZoneCellCandidates,
            index => Random(new ZoneCellId(region, index), GenerationPurpose.ZoneCellSite));

        layer = new(cell, sites);
        zoneLayers.Add(region, layer);
        return layer;
    }

    /// <summary>Chooses which zone cells of a region start as ocean seeds.</summary>
    private bool[] PickOceanSeeds(RegionId region, int zoneCells)
    {
        var random = Random(region, GenerationPurpose.OceanSeeds);
        var count = Math.Min(random.NextInt(siting.OceanSeedsPerRegion), zoneCells);

        var order = Enumerable.Range(0, zoneCells).ToArray();
        var isOceanSeed = new bool[zoneCells];
        for (var i = 0; i < count; i++)
        {
            var pick = i + random.NextInt(zoneCells - i);
            (order[i], order[pick]) = (order[pick], order[i]);
            isOceanSeed[order[i]] = true;
        }
        return isOceanSeed;
    }

    private ElevationType PickElevation(ZoneCellId cell)
    {
        var weights = siting.ElevationWeights;
        var pick = PickWeighted(
            [weights.Flat, weights.Lowland, weights.Hills, weights.Mountains],
            Random(cell, GenerationPurpose.ElevationType));
        return ElevationType.Flat + pick;
    }

    private BedrockId PickBedrock(ZoneCellId cell, ElevationType elevation)
    {
        var weights = parameters.Bedrocks.Select(bedrock => BedrockWeight(bedrock, elevation)).ToArray();
        return parameters.Bedrocks[PickWeighted(weights, Random(cell, GenerationPurpose.Bedrock))].Id;
    }

    /// <summary>Under the ocean every bedrock counts with its weight for all land types together.</summary>
    private static double BedrockWeight(Bedrock bedrock, ElevationType elevation) => elevation == ElevationType.Ocean
        ? bedrock.Weights.Flat + bedrock.Weights.Lowland + bedrock.Weights.Hills + bedrock.Weights.Mountains
        : bedrock.Weights[elevation];

    // ---- Patch level --------------------------------------------------------------------

    private ImmutableArray<PatchCell> SitePatchCells(ZoneCellId zoneCell, WorldPoint zoneSite)
    {
        // A zone cell can reach into a neighbouring region, but never farther than twice the
        // reach: every region has a zone cell site within that distance of any of its points.
        var cell = VoronoiCell(zoneSite, bound: 2 * reach, spread: reach, id => ZoneLayerOf(id).Sites);
        var count = Random(zoneCell, GenerationPurpose.PatchCellCount).NextInt(siting.PatchCellsPerZoneCell);
        var sites = Scatter(cell, count, siting.PatchCellCandidates,
            index => Random(new PatchCellId(zoneCell, index), GenerationPurpose.PatchCellSite));

        var patchCells = ImmutableArray.CreateBuilder<PatchCell>(count);
        for (var index = 0; index < count; index++)
            patchCells.Add(new(new PatchCellId(zoneCell, index), sites[index]));
        return patchCells.MoveToImmutable();
    }

    // ---- Shared -------------------------------------------------------------------------

    /// <summary>
    /// The exact Voronoi cell of a site among the sites of all regions.
    /// </summary>
    /// <param name="centre">The site the cell belongs to.</param>
    /// <param name="bound">The cell is known to lie within this distance of the centre, on each axis.</param>
    /// <param name="spread">No site of a region is farther than this from the region site.</param>
    /// <param name="sitesOf">The sites of one region.</param>
    private ConvexPolygon VoronoiCell(WorldPoint centre, double bound, double spread, Func<RegionId, IReadOnlyList<WorldPoint>> sitesOf)
    {
        var cell = ConvexPolygon.Square(centre, bound);
        var radius = cell.MaxDistanceFrom(centre);

        // A site can only cut the cell when it is nearer than twice the radius of the cell.
        // Regions are visited nearest first, and the radius only shrinks, so the visit can
        // stop at the first region that is too far away to hold such a site.
        foreach (var (region, distance) in GridCellsWithin(centre, 2 * radius + spread))
        {
            if (distance > 2 * radius + spread)
                break;
            if (RegionSite(region).DistanceTo(centre) > 2 * radius + spread)
                continue;

            foreach (var site in sitesOf(region))
                if (site != centre && site.DistanceTo(centre) < 2 * radius)
                    cell = cell.ClipNearerTo(centre, site);
            radius = cell.MaxDistanceFrom(centre);
        }
        return cell;
    }

    /// <summary>
    /// The regions whose grid cell is within a distance of a point, nearest first. A region
    /// site always lies in its own grid cell.
    /// </summary>
    private List<(RegionId Region, double Distance)> GridCellsWithin(WorldPoint point, double distance)
    {
        var minX = (int)Math.Floor((point.X - distance) / size);
        var maxX = (int)Math.Floor((point.X + distance) / size);
        var minY = (int)Math.Floor((point.Y - distance) / size);
        var maxY = (int)Math.Floor((point.Y + distance) / size);

        var cells = new List<(RegionId Region, double Distance)>();
        for (var y = minY; y <= maxY; y++)
            for (var x = minX; x <= maxX; x++)
            {
                var dx = Math.Max(0, Math.Max(x * size - point.X, point.X - (x + 1) * size));
                var dy = Math.Max(0, Math.Max(y * size - point.Y, point.Y - (y + 1) * size));
                var cellDistance = Math.Sqrt(dx * dx + dy * dy);
                if (cellDistance <= distance)
                    cells.Add((new(x, y), cellDistance));
            }

        // A total order, so the clipping happens in the same order on every run.
        cells.Sort((a, b) =>
        {
            var byDistance = a.Distance.CompareTo(b.Distance);
            if (byDistance != 0) return byDistance;
            var byY = a.Region.Y.CompareTo(b.Region.Y);
            return byY != 0 ? byY : a.Region.X.CompareTo(b.Region.X);
        });
        return cells;
    }

    /// <summary>
    /// Places sites in a cell one at a time. Each site is the best of a number of random
    /// candidates: the one farthest from the sites already placed and from the border of the
    /// cell. The border counts double, because the neighbouring cell keeps the same distance
    /// on its side.
    /// </summary>
    private static WorldPoint[] Scatter(ConvexPolygon cell, int count, int candidates, Func<int, GenerationRandom> randomOf)
    {
        var sites = new WorldPoint[count];
        for (var index = 0; index < count; index++)
        {
            var random = randomOf(index);
            var bestScore = double.MinValue;
            for (var candidate = 0; candidate < candidates; candidate++)
            {
                var point = cell.PointAt(random.NextDouble(), random.NextDouble(), random.NextDouble());
                var score = 2 * cell.DistanceToBoundary(point);
                for (var placed = 0; placed < index; placed++)
                    score = Math.Min(score, point.DistanceTo(sites[placed]));

                if (score > bestScore)
                {
                    bestScore = score;
                    sites[index] = point;
                }
            }
        }
        return sites;
    }

    /// <summary>The index of one of the weights, chosen with a chance proportional to its weight.</summary>
    private static int PickWeighted(ReadOnlySpan<double> weights, GenerationRandom random)
    {
        var total = 0.0;
        foreach (var weight in weights)
            total += weight;

        var target = random.NextDouble() * total;
        var last = 0;
        for (var index = 0; index < weights.Length; index++)
        {
            if (weights[index] <= 0)
                continue;
            last = index;
            if (target < weights[index])
                return index;
            target -= weights[index];
        }
        return last;
    }

    private GenerationRandom Random(RegionId region, GenerationPurpose purpose) => new(SeedHash.For(seed, region, purpose));
    private GenerationRandom Random(ZoneCellId cell, GenerationPurpose purpose) => new(SeedHash.For(seed, cell, purpose));
    private GenerationRandom Random(PatchCellId cell, GenerationPurpose purpose) => new(SeedHash.For(seed, cell, purpose));

    private static void Validate(ParameterSet parameters)
    {
        var siting = parameters.Siting;
        Require(siting.RegionGridSizeKm > 0, "RegionGridSizeKm must be positive.");
        Require(siting.RegionJitter is >= 0 and <= 0.5, "RegionJitter must be between 0 and 0.5, so a region site stays in its grid cell.");
        Require(siting.ZoneCellCandidates >= 1, "ZoneCellCandidates must be at least 1.");
        Require(siting.PatchCellCandidates >= 1, "PatchCellCandidates must be at least 1.");
        RequireRange(siting.ZoneCellsPerRegion, 1, "ZoneCellsPerRegion");
        RequireRange(siting.PatchCellsPerZoneCell, 1, "PatchCellsPerZoneCell");
        RequireRange(siting.OceanSeedsPerRegion, 0, "OceanSeedsPerRegion");
        RequireRange(parameters.Outline.ExpansionsPerOceanSeed, 0, "ExpansionsPerOceanSeed");
        RequireRange(parameters.Outline.ZoneMergersPerRegion, 0, "ZoneMergersPerRegion");

        var elevation = siting.ElevationWeights;
        Require(
            elevation.Flat >= 0 && elevation.Lowland >= 0 && elevation.Hills >= 0 && elevation.Mountains >= 0
            && elevation.Flat + elevation.Lowland + elevation.Hills + elevation.Mountains > 0,
            "ElevationWeights must not be negative, and at least one must be positive.");

        foreach (var bedrock in parameters.Bedrocks)
            Require(
                bedrock.Weights.Flat >= 0 && bedrock.Weights.Lowland >= 0 && bedrock.Weights.Hills >= 0 && bedrock.Weights.Mountains >= 0,
                $"The weights of bedrock '{bedrock.Id}' must not be negative.");
        foreach (var type in new[] { ElevationType.Flat, ElevationType.Lowland, ElevationType.Hills, ElevationType.Mountains })
            Require(
                elevation[type] <= 0 || parameters.Bedrocks.Any(bedrock => bedrock.Weights[type] > 0),
                $"No bedrock has a weight for elevation type {type}.");
        Require(parameters.Bedrocks.Any(bedrock => BedrockWeight(bedrock, ElevationType.Ocean) > 0), "At least one bedrock must have a weight.");
    }

    private static void RequireRange(IntRange range, int lowest, string name) =>
        Require(range.Min >= lowest && range.Max >= range.Min, $"{name} must be a range from at least {lowest}, with Max not below Min.");

    private static void Require(bool condition, string message)
    {
        if (!condition)
            throw new ArgumentException($"Invalid parameter set: {message}");
    }
}
