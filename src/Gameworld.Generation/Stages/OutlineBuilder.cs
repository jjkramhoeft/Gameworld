using Gameworld.Domain;
using Gameworld.Generation.Cells;

namespace Gameworld.Generation.Stages;

/// <summary>The Sited function and the Outlined stage, as domain records read off the cell map.</summary>
internal static class OutlineBuilder
{
    /// <summary>What the Sited function yields for a region. It needs the foundation only.</summary>
    public static SitedRegion Sited(CellMap cells, GridCoord region)
    {
        var zoneCells = new List<ZoneCell>();
        var patchCells = new List<PatchCell>();
        foreach (var z in cells.OwnedZoneCells(region))
        {
            var seed = cells.IsOceanSeed(z);
            var expansions = new int[cells.ExpansionPasses];
            for (var p = 1; p <= cells.ExpansionPasses; p++) expansions[p - 1] = cells.ExpansionCount(z, p);
            zoneCells.Add(new ZoneCell(
                new ZoneCellId(z),
                cells.Zones.Site(z),
                new RegionId(region),
                seed ? ElevationType.Ocean : cells.LandType(z),
                seed ? null : cells.BedrockOfCell(z),
                seed,
                seed ? cells.LinkCount(z) : 0,
                expansions,
                cells.IsChosenForZoneMerger(z)));

            foreach (var p in cells.OwnedPatchCells(z))
                patchCells.Add(new PatchCell(
                    new PatchCellId(p),
                    cells.Patches.Site(p),
                    new ZoneCellId(z),
                    cells.IsChosenForPatchMerger(p)));
        }
        return new SitedRegion(
            new RegionId(region),
            cells.Regions.Site(region),
            zoneCells,
            patchCells,
            cells.ZoneMergerCount(region));
    }

    /// <summary>Zones linked, expanded and merged, and sea access.</summary>
    public static RegionOutline Outline(CellMap cells, GridCoord region)
    {
        var drawn = cells.ZoneCellsDrawnIn(region);
        var zones = drawn
            .GroupBy(cells.ZoneRoot)
            .OrderBy(g => g.Key.Key)
            .Select(g => new Zone(
                new ZoneId(new ZoneCellId(g.Key)),
                new RegionId(region),
                g.OrderBy(c => c.Key).Select(c => new ZoneCellId(c)).ToArray(),
                cells.ZoneType(g.Key),
                cells.ZoneBedrock(g.Key)))
            .ToArray();
        return new RegionOutline(zones, HasSeaAccess(cells, region));
    }

    /// <summary>An ocean zone touches the land of the region. The ocean zone can belong to a neighbouring region.</summary>
    public static bool HasSeaAccess(CellMap cells, GridCoord region)
    {
        foreach (var z in cells.ZoneCellsDrawnIn(region))
        {
            if (cells.IsOceanZone(z)) continue;
            foreach (var nb in cells.Zones.Neighbours(z))
                if (cells.IsOceanZone(nb)) return true;
        }
        return false;
    }
}
