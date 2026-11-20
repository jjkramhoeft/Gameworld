using Gameworld.Domain;

namespace Gameworld.Demo;

public sealed partial class WorldHost
{
    /// <summary>
    /// The names of the cities and towns in a rectangle of the world, for the labels the map shows with the
    /// Prospects layer: one for every Prospect on show whose spot lies in the rectangle. It takes the regions in the
    /// rectangle to Planned, as the Prospects layer of the tiles does. Null when the rectangle holds too many
    /// regions: the labels are not for the widest view.
    /// </summary>
    public PlaceLabel[]? PlaceLabels(double left, double top, double width, double height)
    {
        const int MaxRegions = 240;
        lock (_gate)
        {
            var world = _world;
            var cells = world.Cells;

            // The regions with land or sea in the rectangle: region borders follow the patches, so they are sampled.
            var step = cells.RegionSize / 8;
            if ((width / step + 3) * (height / step + 3) > 40_000) return null;
            var regions = new HashSet<GridCoord>();
            for (var y = top - step; y <= top + height + step; y += step)
                for (var x = left - step; x <= left + width + step; x += step)
                    regions.Add(cells.CellAt(new WorldPoint(x, y)).Region);
            if (regions.Count > MaxRegions) return null;

            var labels = new List<PlaceLabel>();
            foreach (var region in regions.OrderBy(r => r.Key))
            {
                var plan = world.PlanOf(new RegionId(region));
                var capital = plan.Factions.Factions.FirstOrDefault(f => f.Role == FactionRole.Ruling)?.Seat;
                for (var i = 0; i < plan.Prospects.Length; i++)
                {
                    var prospect = plan.Prospects[i];
                    if (!prospect.OnShow) continue;
                    var spot = prospect.Spot;
                    if (spot.X < left || spot.X > left + width || spot.Y < top || spot.Y > top + height) continue;
                    labels.Add(new PlaceLabel(
                        spot.X, spot.Y, plan.Names[i].Name,
                        prospect.Patch == capital ? "Capital" : prospect.ForCity ? "City" : "Town"));
                }
            }
            return labels.ToArray();
        }
    }
}

/// <summary>The label of a city or a town on the map: where its Prospect is placed, its name and what it is.</summary>
/// <param name="Kind">Capital, City or Town. The capital is a city whatever its Prospect is for.</param>
public sealed record PlaceLabel(double X, double Y, string Name, string Kind);
