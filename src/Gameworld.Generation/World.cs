using Gameworld.Domain;
using Gameworld.Generation.Biomes;
using Gameworld.Generation.Cells;
using Gameworld.Generation.Factions;
using Gameworld.Generation.Heights;
using Gameworld.Generation.Households;
using Gameworld.Generation.Names;
using Gameworld.Generation.People;
using Gameworld.Generation.Races;
using Gameworld.Generation.Settlements;
using Gameworld.Generation.Stages;
using Gameworld.Generation.Travel;
using Gameworld.Generation.Vocations;

namespace Gameworld.Generation;

/// <summary>
/// The generated world of one foundation. Everything in here is derived data: a pure function
/// of the foundation that is computed on demand and cached. The world is unbounded and regions
/// come into existence lazily. Not thread-safe.
/// </summary>
public sealed class World
{
    /// <summary>The version of the generation algorithms in this library.</summary>
    public static readonly GeneratorVersion GeneratorVersion = new(1);

    private readonly Dictionary<RegionId, Region> _regions = new();
    private readonly Dictionary<RegionId, DetailWork> _detail = new();
    private readonly Dictionary<RegionId, ElevationWork> _elevation = new();
    private readonly Dictionary<RegionId, PlanWork> _plan = new();
    private readonly Dictionary<RegionId, SettlementWork> _settlement = new();
    private readonly Dictionary<RegionId, RegionHouseholds> _households = new();
    private readonly Dictionary<RegionId, RegionPeople> _people = new();
    private readonly Dictionary<RegionId, CostGrid> _costGrids = new();
    private readonly Dictionary<long, bool> _riverPoor = new();

    public World(Foundation foundation)
    {
        if (foundation.GeneratorVersion != GeneratorVersion)
            throw new ArgumentException(
                $"The foundation is for generator version {foundation.GeneratorVersion}; this generator is version {GeneratorVersion}.",
                nameof(foundation));
        Foundation = foundation;
        Cells = new CellMap(foundation);
        Fields = new Fields(foundation);
        BiomeRules = new BiomeRules(foundation.Parameters.Elevation.Biomes);
        PopulationRules = new PopulationRules(foundation.Parameters, foundation.Seed.Value);
        FactionRules = new FactionRules(foundation.Parameters, foundation.Seed.Value);
        RegionNameRules = new RegionNameRules(foundation.Parameters, foundation.Seed.Value);
        SettlementNameRules = new SettlementNameRules(foundation.Parameters, foundation.Seed.Value);
        SettlementRules = new SettlementRules(foundation.Parameters, PopulationRules);
        HouseholdRules = new HouseholdRules(foundation.Parameters, foundation.Seed.Value);
        LooksRules = new LooksRules(foundation.Parameters, foundation.Seed.Value);
        PersonRules = new PersonRules(foundation.Parameters, LooksRules, foundation.Seed.Value);
        FloraFaunaRules = new FloraFaunaRules(foundation.Parameters);
        VocationRules = new VocationRules(foundation.Parameters);
        RiverBeds = new RiverBeds(this);
    }

    /// <summary>A world for a seed text and a Parameter Set, at the current generator version.</summary>
    public static World Create(string seedText, ParameterSet? parameters = null) =>
        new(new Foundation(Seed.FromText(seedText), GeneratorVersion, parameters ?? ParameterSet.Default));

    public Foundation Foundation { get; }
    public ParameterSet Parameters => Foundation.Parameters;
    public CellMap Cells { get; }
    public Fields Fields { get; }
    public BiomeRules BiomeRules { get; }
    public PopulationRules PopulationRules { get; }
    public FactionRules FactionRules { get; }
    public RegionNameRules RegionNameRules { get; }
    public SettlementNameRules SettlementNameRules { get; }
    public SettlementRules SettlementRules { get; }
    public HouseholdRules HouseholdRules { get; }
    public PersonRules PersonRules { get; }
    public FloraFaunaRules FloraFaunaRules { get; }
    public VocationRules VocationRules { get; }
    public LooksRules LooksRules { get; }

    /// <summary>
    /// The river bed profiles of the height maps (map-generation.md section 12.2), cached per
    /// region for the whole world: every height map that a region's rivers cross reads the same beds.
    /// </summary>
    internal RiverBeds RiverBeds { get; }

    /// <summary>
    /// A river-poor flat zone: a flat zone whose site (the site of its root cell) gets less
    /// precipitation than the cutoff. It gets few rivers, is drier for the biomes and holds no lakes.
    /// </summary>
    public bool IsRiverPoorZone(GridCoord zoneCell)
    {
        var root = Cells.ZoneRoot(zoneCell);
        if (_riverPoor.TryGetValue(root.Key, out var poor)) return poor;
        poor = Cells.ZoneType(root) == ElevationType.FlatLand
            && Fields.Precipitation(Cells.Zones.Site(root)) < Parameters.Detail.Rivers.RiverPoorPrecipitationCutoffMm;
        _riverPoor[root.Key] = poor;
        return poor;
    }

    /// <summary>The stored regions.</summary>
    public IReadOnlyCollection<Region> Regions => _regions.Values;

    /// <summary>The region whose land or sea contains a point.</summary>
    public RegionId RegionAt(WorldPoint p) => new(Cells.CellAt(p).Region);

    /// <summary>The stage of a region. A region that is not stored is Sited.</summary>
    public RegionStage StageOf(RegionId id) =>
        _regions.TryGetValue(id, out var region) ? region.Stage : RegionStage.Sited;

    /// <summary>Sited is a pure function of the foundation: it is calculated, never stored.</summary>
    public SitedRegion Sited(RegionId id) => OutlineBuilder.Sited(Cells, id.Coord);

    /// <summary>The stored region, or null when it is only Sited.</summary>
    public Region? Find(RegionId id) => _regions.GetValueOrDefault(id);

    /// <summary>
    /// The region at <paramref name="stage"/> or later. The region advances stage by stage, and
    /// the regions around it advance first as far as each stage needs.
    /// </summary>
    public Region Advance(RegionId id, RegionStage stage)
    {
        if (stage == RegionStage.Sited) stage = RegionStage.Outlined; // stored regions start at Outlined
        if (!_regions.TryGetValue(id, out var region))
        {
            region = new Region(id, OutlineBuilder.Outline(Cells, id.Coord));
            _regions[id] = region;
        }

        while (region.Stage < stage)
        {
            var next = region.Stage + 1;
            if (RegionStageRules.NeedOf(next, Parameters) is { } need && need.Stage > RegionStage.Sited)
                foreach (var other in RegionStageRules.RegionsWithin(id, need.Rings))
                    if (other != id)
                        Advance(other, need.Stage);

            if (!RegionStageRules.CanAdvance(id, region.Stage, StageOf, Parameters))
                throw new InvalidOperationException($"Region {id} cannot advance from {region.Stage}.");

            AdvanceOne(region, next);
        }
        return region;
    }

    /// <summary>
    /// The Detailed working data of a region. Asking for it takes the region to the Detailed
    /// stage first, as every layer accessor does for its own stage: the stages are strictly
    /// linear, so no layer of a region exists before the region has reached the layer's stage.
    /// </summary>
    public DetailWork DetailOf(RegionId id)
    {
        Advance(id, RegionStage.Detailed);
        return _detail[id];
    }

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

    /// <summary>The Planned working data of a region. The region is taken to the Planned stage first, through Elevated.</summary>
    public PlanWork PlanOf(RegionId id)
    {
        Advance(id, RegionStage.Planned);
        return _plan[id];
    }

    /// <summary>The Planned working data of a region if the region is Planned or later; null otherwise.</summary>
    public PlanWork? FindPlan(RegionId id) => _plan.GetValueOrDefault(id);

    /// <summary>
    /// The Settled working data of a region: the travel links the region decides and its cities
    /// and towns. The region is taken to the Settled stage first, which takes the regions around
    /// it to Planned.
    /// </summary>
    public SettlementWork SettlementOf(RegionId id)
    {
        Advance(id, RegionStage.Settled);
        return _settlement[id];
    }

    /// <summary>The Settled working data of a region if the region is Settled or later; null otherwise.</summary>
    public SettlementWork? FindSettlement(RegionId id) => _settlement.GetValueOrDefault(id);

    /// <summary>The households of a region. The region is taken to the Housed stage first, through Settled.</summary>
    public RegionHouseholds HouseholdsOf(RegionId id)
    {
        Advance(id, RegionStage.Housed);
        return _households[id];
    }

    /// <summary>The households of a region if the region is Housed or later; null otherwise.</summary>
    public RegionHouseholds? FindHouseholds(RegionId id) => _households.GetValueOrDefault(id);

    /// <summary>The persons of the households of a region. The region is taken to the Peopled stage first, through Housed.</summary>
    public RegionPeople PeopleOf(RegionId id)
    {
        Advance(id, RegionStage.Peopled);
        return _people[id];
    }

    /// <summary>The persons of the households of a region if the region is Peopled; null otherwise.</summary>
    public RegionPeople? FindPeople(RegionId id) => _people.GetValueOrDefault(id);

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

    private void AdvanceOne(Region region, RegionStage next)
    {
        var id = region.Id;
        switch (next)
        {
            case RegionStage.Detailed:
                var detail = DetailBuilder.Build(this, id.Coord);
                _detail[id] = detail;
                region.Advance(detail.ToDomain());
                break;
            case RegionStage.Elevated:
                var elevation = ElevationBuilder.Build(this, _detail[id]);
                _elevation[id] = elevation;
                region.Advance(elevation.ToDomain());
                break;
            case RegionStage.Planned:
                var plan = PlanBuilder.Build(this, _detail[id], _elevation[id]);
                _plan[id] = plan;
                region.Advance(plan.ToDomain());
                break;
            case RegionStage.Settled:
                var places = SettlementBuilder.Build(this, _plan[id]);
                var settlement = TravelBuilder.Build(this, id.Coord, places);
                _settlement[id] = settlement;
                region.Advance(settlement.ToDomain());
                break;
            case RegionStage.Housed:
                var households = HouseholdBuilder.Build(this, _plan[id], _settlement[id].Places.Settlements);
                _households[id] = households;
                region.Advance(households);
                break;
            case RegionStage.Peopled:
                var people = PeopleBuilder.Build(this, _plan[id], _settlement[id].Places.Settlements, _households[id]);
                _people[id] = people;
                region.Advance(people);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(next), next, null);
        }
    }
}
