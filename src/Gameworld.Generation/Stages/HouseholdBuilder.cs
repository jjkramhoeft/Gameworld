using Gameworld.Domain;
using Gameworld.Generation.Factions;
using Gameworld.Generation.People;

namespace Gameworld.Generation.Stages;

/// <summary>
/// The households of a Settled region (society-generation.md section 6): the Housed stage. It
/// reads the region's own Planned and Settled data only: its factions, the surname of its
/// ruling house and its cities and towns.
/// </summary>
public static class HouseholdBuilder
{
    /// <param name="settlements">The cities and towns of the region, by rank.</param>
    public static RegionHouseholds Build(World world, PlanWork plan, IReadOnlyList<Settlement> settlements) =>
        world.HouseholdRules.Build(plan.Region, plan.Factions.Factions, settlements, plan.Naming?.RulingHouse);
}

/// <summary>
/// The persons of a Housed region (society-generation.md section 7): the Peopled stage. It
/// reads the region's own households, factions, cities and towns, and of the Prospect of each
/// city or town whether it is on water.
/// </summary>
public static class PeopleBuilder
{
    /// <param name="settlements">The cities and towns of the region, by rank.</param>
    public static RegionPeople Build(
        World world, PlanWork plan, IReadOnlyList<Settlement> settlements, RegionHouseholds households) =>
        world.PersonRules.Build(plan.Region, households.Households, plan.Factions.Factions, PlacesOf(plan, settlements));

    /// <summary>What the persons read of each city or town of the region, by its slot.</summary>
    public static IReadOnlyDictionary<int, HouseholdPlace> PlacesOf(PlanWork plan, IReadOnlyList<Settlement> settlements) =>
        settlements.ToDictionary(
            s => s.Slot,
            s => new HouseholdPlace(PersonRules.TierOf(s), FactionRules.OnWater(plan.Prospects[s.Slot])));
}
