using Gameworld.Domain;
using Gameworld.Generation.Stages;
using Gameworld.Generation.Travel;

namespace Gameworld.Generation;

public sealed partial class World
{
    private readonly Dictionary<RegionId, ElevationWork> _elevation = new();
    private readonly Dictionary<RegionId, CostGrid> _costGrids = new();
    private readonly Dictionary<RegionId, SettlementWork> _settlement = new();

    /// <summary>
    /// The Elevated working data of a region: the height map and the final biomes and resources.
    /// The region is taken to the Elevated stage first.
    /// </summary>
    public ElevationWork ElevationOf(RegionId id)
    {
        Advance(id, RegionStage.Elevated);
        return _elevation[id];
    }

    /// <summary>The Elevated working data of a region if the region is Elevated or later; null otherwise.</summary>
    public ElevationWork? FindElevation(RegionId id) => _elevation.GetValueOrDefault(id);

    /// <summary>The travel cost grid of a region. It reads the region's Detailed data only.</summary>
    internal CostGrid CostGridOf(RegionId id)
    {
        if (!_costGrids.TryGetValue(id, out var grid))
        {
            grid = CostGrid.Build(this, DetailOf(id));
            _costGrids[id] = grid;
        }
        return grid;
    }

    /// <summary>
    /// The Settled working data of a region: the travel links the region decides. The region is
    /// taken to the Settled stage first, which takes the regions around it to Planned.
    /// </summary>
    public SettlementWork SettlementOf(RegionId id)
    {
        Advance(id, RegionStage.Settled);
        return _settlement[id];
    }

    /// <summary>
    /// Whether two ocean zones are on the same sea: a sea is a connected set of ocean zones. This
    /// is not stored as a region fact, because the world is unbounded; it is answered by a search
    /// over the regions that are Outlined (stored) now, so a sea that connects only through
    /// regions that are not yet Outlined is not found.
    /// </summary>
    public bool AreOnSameSea(ZoneId a, ZoneId b)
    {
        var start = Cells.ZoneRoot(a.Coord);
        var goal = Cells.ZoneRoot(b.Coord);
        if (!Cells.IsOceanCell(start) || !Cells.IsOceanCell(goal)) return false;
        if (start == goal) return true;

        var seen = new HashSet<long> { start.Key };
        var queue = new Queue<GridCoord>();
        queue.Enqueue(start);
        while (queue.Count > 0)
        {
            var cell = queue.Dequeue();
            foreach (var nb in Cells.Zones.Neighbours(cell))
            {
                if (!seen.Add(nb.Key)) continue;
                if (!Cells.IsOceanZone(nb)) continue;
                if (Cells.ZoneRoot(nb) == goal) return true;
                if (!_regions.ContainsKey(new RegionId(Cells.RegionOfZone(nb)))) continue;
                queue.Enqueue(nb);
            }
        }
        return false;
    }

    private void AdvanceLater(Region region, RegionStage next)
    {
        switch (next)
        {
            case RegionStage.Elevated:
                var elevation = ElevationBuilder.Build(this, _detail[region.Id]);
                _elevation[region.Id] = elevation;
                region.Advance(elevation.ToDomain());
                break;
            case RegionStage.Settled:
                var settlement = TravelBuilder.Build(this, region.Id.Coord);
                _settlement[region.Id] = settlement;
                region.Advance(settlement.ToDomain());
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(next), next, null);
        }
    }
}
