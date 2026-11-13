namespace Gameworld.Domain.Model;

/// <summary>
/// A region and every region within a number of rings of it, all at the same stage. It is
/// the input of a generation step, so a step cannot run before its neighbours are ready.
/// </summary>
public sealed class Neighbourhood<TStage> where TStage : IRegionStage
{
    private readonly Dictionary<RegionId, TStage> regions;

    /// <exception cref="ArgumentException">A region within the rings is missing.</exception>
    public Neighbourhood(RegionId centre, int rings, IEnumerable<TStage> regions)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(rings);
        CentreId = centre;
        Rings = rings;
        this.regions = regions
            .Where(region => region.Id.RingsTo(centre) <= rings)
            .ToDictionary(region => region.Id);

        var missing = centre.Within(rings).Where(id => !this.regions.ContainsKey(id)).ToList();
        if (missing.Count > 0)
            throw new ArgumentException(
                $"The neighbourhood of {centre} with {rings} rings is missing {string.Join(", ", missing)}.",
                nameof(regions));
    }

    public RegionId CentreId { get; }
    public int Rings { get; }
    public TStage Centre => regions[CentreId];
    public IReadOnlyCollection<TStage> All => regions.Values;

    public TStage this[RegionId id] => regions[id];

    /// <summary>The regions exactly <paramref name="ring"/> rings from the centre.</summary>
    public IEnumerable<TStage> Ring(int ring)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(ring, Rings);
        return CentreId.Ring(ring).Select(id => regions[id]);
    }
}
