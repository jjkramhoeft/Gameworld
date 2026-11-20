using Gameworld.Domain;
using Gameworld.Generation.Settlements;

namespace Gameworld.Generation.Stages;

/// <summary>
/// The cities and towns of a Planned region (society-generation.md section 5.2). They belong
/// to the Settled stage, but they read the region's own Planned data only: its Prospects on
/// show with their races, its potential population and the seat of its ruling faction.
/// </summary>
public static class SettlementBuilder
{
    public static RegionSettlements Build(World world, PlanWork plan)
    {
        var sites = new List<SettlementSite>();
        for (var i = 0; i < plan.Prospects.Length; i++)
        {
            var prospect = plan.Prospects[i];
            if (!prospect.OnShow) continue;
            var place = plan.Places[i];
            sites.Add(new SettlementSite(
                i, prospect.Patch, prospect.ForCity, place.Races,
                world.PopulationRules.SettlementWeights(plan.Races, place.Around)));
        }
        var capital = plan.Factions.Factions.FirstOrDefault(f => f.Role == FactionRole.Ruling)?.Seat;
        return world.SettlementRules.Build(plan.Races, sites, capital);
    }
}
