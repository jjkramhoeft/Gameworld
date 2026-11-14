using Gameworld.Domain;

namespace Gameworld.Generation.Rendering;

/// <summary>One band of a field layer: the values from <paramref name="From"/> up to, not including, <paramref name="To"/>. The first band has no lower bound and the last no upper bound.</summary>
public sealed record FieldBand(double? From, double? To, Rgb Colour);

public static partial class MapRenderer
{
    private static readonly Rgb BlankColour = new(255, 255, 255);

    private static readonly FieldKind[] FieldOrder =
        [FieldKind.InitialTemperature, FieldKind.Precipitation, FieldKind.Prosperity, FieldKind.Supernatural];

    // The bands of the field layers (section 17). The bounds are fixed values, not shares of
    // the field's range, so a band means the same under every Parameter Set.
    private static readonly (double[] Bounds, Rgb[] Colours) TemperatureBands = (
        [-5, 0, 5, 10, 15, 20, 25],
        [
            new(69, 117, 180), new(116, 173, 209), new(171, 217, 233), new(224, 243, 248),
            new(254, 224, 144), new(253, 174, 97), new(244, 109, 67), new(215, 48, 39),
        ]);

    private static readonly (double[] Bounds, Rgb[] Colours) PrecipitationBands = (
        [250, 500, 1000, 2000, 4000, 8000],
        [
            new(255, 255, 204), new(199, 233, 180), new(127, 205, 187), new(65, 182, 196),
            new(29, 145, 192), new(34, 94, 168), new(12, 44, 132),
        ]);

    private static readonly (double[] Bounds, Rgb[] Colours) ProsperityBands = (
        [20, 40, 60, 80],
        [new(255, 255, 212), new(254, 217, 142), new(254, 153, 41), new(217, 95, 14), new(153, 52, 4)]);

    private static readonly (double[] Bounds, Rgb[] Colours) SupernaturalBands = (
        [20, 40, 60, 80],
        [new(254, 235, 226), new(251, 180, 185), new(247, 104, 161), new(197, 27, 138), new(122, 1, 119)]);

    private static (double[] Bounds, Rgb[] Colours) BandsOf(FieldKind kind) => kind switch
    {
        FieldKind.InitialTemperature => TemperatureBands,
        FieldKind.Precipitation => PrecipitationBands,
        FieldKind.Prosperity => ProsperityBands,
        _ => SupernaturalBands,
    };

    /// <summary>The bands a field layer is drawn in, from the lowest values to the highest: the legend of the layer.</summary>
    public static IReadOnlyList<FieldBand> FieldBands(FieldKind kind)
    {
        var (bounds, colours) = BandsOf(kind);
        var bands = new FieldBand[colours.Length];
        for (var i = 0; i < colours.Length; i++)
            bands[i] = new FieldBand(i == 0 ? null : bounds[i - 1], i == bounds.Length ? null : bounds[i], colours[i]);
        return bands;
    }

    /// <summary>The colour of the band a field value falls in.</summary>
    public static Rgb FieldColour(FieldKind kind, double value)
    {
        var (bounds, colours) = BandsOf(kind);
        var i = 0;
        while (i < bounds.Length && value >= bounds[i]) i++;
        return colours[i];
    }

    /// <summary>The fields the layers show, in a fixed order.</summary>
    private static FieldKind[] FieldLayers(MapLayers layers) => FieldOrder.Where(layers.Shows).ToArray();
}
