using Gameworld.Domain;
using Gameworld.Generation.Biomes;
using Gameworld.Generation.Cells;
using Gameworld.Generation.Resources;
using Gameworld.Generation.Rivers;
using Gameworld.Generation.Stages;

namespace Gameworld.Generation;

/// <summary>
/// The generated world of one foundation. Everything in here is derived data: a pure function
/// of the foundation that is computed on demand and cached. The world is unbounded and regions
/// come into existence lazily. Not thread-safe.
/// </summary>
public sealed partial class World
{
    /// <summary>The version of the generation algorithms in this library.</summary>
    public static readonly GeneratorVersion GeneratorVersion = new(1);

    private readonly Dictionary<RegionId, Region> _regions = new();
    private readonly Dictionary<RegionId, DetailWork> _detail = new();
    private readonly Dictionary<RegionId, PlanWork> _plan = new();
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
    }

    /// <summary>A world for a seed text and a Parameter Set, at the current generator version.</summary>
    public static World Create(string seedText, ParameterSet? parameters = null) =>
        new(new Foundation(Seed.FromText(seedText), GeneratorVersion, parameters ?? ParameterSet.Default));

    public Foundation Foundation { get; }
    public ParameterSet Parameters => Foundation.Parameters;
    public CellMap Cells { get; }
    public Fields Fields { get; }
    public BiomeRules BiomeRules { get; }

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

    /// <summary>Grows the rivers of a region. Pure: nothing is stored.</summary>
    public RiverResult GrowRivers(RegionId id) =>
        RiverBuilder.Build(Cells, id.Coord, Foundation.Seed.Value, Parameters.Detail.Rivers, IsRiverPoorZone);

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

    /// <summary>The Planned working data of a region. The region is taken to the Planned stage first, through Elevated.</summary>
    public PlanWork PlanOf(RegionId id)
    {
        Advance(id, RegionStage.Planned);
        return _plan[id];
    }

    /// <summary>The Planned working data of a region if the region is Planned or later; null otherwise.</summary>
    public PlanWork? FindPlan(RegionId id) => _plan.GetValueOrDefault(id);

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
            case RegionStage.Planned:
                var plan = PlanBuilder.Build(this, _detail[id]);
                _plan[id] = plan;
                region.Advance(plan.ToDomain());
                break;
            default:
                AdvanceLater(region, next);
                break;
        }
    }

    internal PatchResources LandResources(in LandResourceInput input) => ResourceRules.Land(input);

    internal PatchResources OceanResources(in OceanResourceInput input) => ResourceRules.Ocean(input);
}
