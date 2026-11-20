using Gameworld.Domain;
using Gameworld.Generation.Biomes;
using Gameworld.Generation.Rendering;

namespace Gameworld.Demo;

// What the inspection of a point and the statistics of a view share: the small records the
// browser reads, and the sums that fill them from a set of patches weighted by area.

/// <summary>A part of an area: its name, the colour the map draws it in when it has one, its area and its share of the whole.</summary>
/// <param name="Share">0–1.</param>
public sealed record AreaShare(string Name, string? Colour, double AreaKm2, double Share);

/// <summary>The lowest, the area-weighted mean and the highest of a value over an area.</summary>
public sealed record ValueSpread(double Min, double Mean, double Max);

public sealed record NamedValue(string Name, double Value);

public sealed record NamedCount(string Name, int Count);

/// <summary>One resource over an area.</summary>
/// <param name="Mean">The area-weighted mean amount, 0–1.</param>
/// <param name="ListedShare">The share of the area where the resource is listed, 0–1.</param>
/// <param name="Best">The largest amount of a patch.</param>
public sealed record ResourceSummary(Resource Resource, string Abbreviation, ResourceGroup Group, double Mean, double ListedShare, double Best);

/// <summary>
/// How many Prospects a region or a zone holds, for cities and for towns. The ones on show are the ones the map
/// draws and the travel network connects: the sum of the scores of a region's Prospects decides how many they are.
/// </summary>
public sealed record ProspectCount(int City, int Town, int CityOnShow, int TownOnShow)
{
    public int OnShow => CityOnShow + TownOnShow;

    public static ProspectCount Of(IEnumerable<Prospect> prospects)
    {
        int city = 0, town = 0, cityOnShow = 0, townOnShow = 0;
        foreach (var p in prospects)
        {
            if (p.ForCity) city++; else town++;
            if (!p.OnShow) continue;
            if (p.ForCity) cityOnShow++; else townOnShow++;
        }
        return new ProspectCount(city, town, cityOnShow, townOnShow);
    }
}

/// <summary>The colours the map draws things in, as #rrggbb.</summary>
internal static class MapColours
{
    public static string Hex(Rgb c) => $"#{c.R:x2}{c.G:x2}{c.B:x2}";

    public static string Of(Biome biome)
    {
        var (r, g, b) = BiomeRules.Colour(biome);
        return $"#{r:x2}{g:x2}{b:x2}";
    }

    public static string Of(ElevationType type) => Hex(MapRenderer.ElevationColour(type));
}

/// <summary>A weighted sum of one value: it gives the lowest, the mean and the highest.</summary>
internal sealed class SpreadSum
{
    private double _sum, _weight;
    private double _min = double.PositiveInfinity, _max = double.NegativeInfinity;

    public void Add(double value, double weight)
    {
        if (!(weight > 0) || !double.IsFinite(value)) return;
        _sum += value * weight;
        _weight += weight;
        if (value < _min) _min = value;
        if (value > _max) _max = value;
    }

    public double? Mean => _weight > 0 ? _sum / _weight : null;

    public ValueSpread? ToSpread() => _weight > 0 ? new ValueSpread(_min, _sum / _weight, _max) : null;
}

/// <summary>The area of each part of a whole, by a key.</summary>
internal sealed class AreaSum<T> where T : notnull
{
    private readonly Dictionary<T, double> _areas = new();

    public void Add(T key, double area) => _areas[key] = _areas.GetValueOrDefault(key) + area;

    public int Count => _areas.Count;

    /// <summary>The parts with their share of <paramref name="total"/>, the largest first, or in the order of the keys.</summary>
    public AreaShare[] Shares(double total, Func<T, string> name, Func<T, string?>? colour = null, bool inKeyOrder = false)
    {
        var parts = inKeyOrder
            ? _areas.OrderBy(p => p.Key)
            : _areas.OrderByDescending(p => p.Value).ThenBy(p => name(p.Key), StringComparer.Ordinal);
        return parts
            .Select(p => new AreaShare(name(p.Key), colour?.Invoke(p.Key), p.Value, total > 0 ? p.Value / total : 0))
            .ToArray();
    }
}

/// <summary>The resources of an area: the mean amount of each, the share of the area where it is listed and the best patch.</summary>
internal sealed class ResourceSum
{
    private readonly double[] _amount = new double[ResourceCatalog.Count];
    private readonly double[] _listed = new double[ResourceCatalog.Count];
    private readonly double[] _best = new double[ResourceCatalog.Count];
    private double _area, _food, _wealth;

    public void Add(PatchResources resources, double area)
    {
        if (!(area > 0)) return;
        _area += area;
        _food += resources.Food * area;
        _wealth += resources.Wealth * area;
        for (var k = 0; k < ResourceCatalog.Count; k++)
        {
            var amount = resources.Amounts[k];
            _amount[k] += amount.Amount * area;
            if (amount.IsListed) _listed[k] += area;
            if (amount.Amount > _best[k]) _best[k] = amount.Amount;
        }
    }

    public double Food => _area > 0 ? _food / _area : 0;

    public double Wealth => _area > 0 ? _wealth / _area : 0;

    /// <summary>The resources that are listed somewhere in the area, the most widely listed first.</summary>
    public ResourceSummary[] ToArray() => _area > 0
        ? ResourceCatalog.All
            .Where(r => _listed[(int)r] > 0)
            .Select(r => new ResourceSummary(
                r, ResourceCatalog.AbbreviationOf(r), ResourceCatalog.GroupOf(r),
                _amount[(int)r] / _area, _listed[(int)r] / _area, _best[(int)r]))
            .OrderByDescending(r => r.ListedShare).ThenByDescending(r => r.Mean).ThenBy(r => r.Resource)
            .ToArray()
        : [];
}
