using Gameworld.Domain.Model;

namespace Gameworld.Domain.Generation;

/// <summary>The generation algorithms of <see cref="GeneratorVersion.Current"/>.</summary>
public sealed class RegionGenerator : IRegionGenerator
{
    public SitedRegion Site(Foundation foundation, RegionId region)
    {
        RequireCurrentVersion(foundation);
        return new Siting(foundation).Site(region);
    }

    public OutlinedRegion Outline(Foundation foundation, Neighbourhood<SitedRegion> sited) =>
        throw new NotImplementedException("The Outline step is not implemented yet.");

    public DetailedRegion Detail(Foundation foundation, Neighbourhood<OutlinedRegion> outlined) =>
        throw new NotImplementedException("The Detail step is not implemented yet.");

    public ElevatedRegion Elevate(Foundation foundation, Neighbourhood<DetailedRegion> detailed, IFields fields) =>
        throw new NotImplementedException("The Elevate step is not implemented yet.");

    /// <summary>
    /// Derived data is a function of the generator version too, so these algorithms can only
    /// generate for a foundation of their own version.
    /// </summary>
    private static void RequireCurrentVersion(Foundation foundation)
    {
        if (foundation.GeneratorVersion != GeneratorVersion.Current)
            throw new NotSupportedException(
                $"Foundation {foundation} needs generator version {foundation.GeneratorVersion}, but this is version {GeneratorVersion.Current}.");
    }
}
