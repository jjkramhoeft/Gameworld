using Gameworld.Domain;

namespace Gameworld.Generation.Resources;

/// <summary>What a crop requires of its patch (map-generation.md Appendix B, column "Needs").</summary>
public enum CropNeed
{
    None,
    /// <summary>Scores a quarter without a river or lake.</summary>
    River,
    /// <summary>Grows on coastal patches only.</summary>
    Coast,
    /// <summary>Grows in tropical highlands only.</summary>
    Highland,
}

/// <summary>
/// A trapezoid of section 14.2: absolute min / optimum min / optimum max / absolute max.
/// </summary>
public readonly record struct Trapezoid(double Min, double OptMin, double OptMax, double Max)
{
    /// <summary>trap(v): 0 outside (Min, Max), 1 on [OptMin, OptMax], linear in between.</summary>
    public double At(double v)
    {
        if (v <= Min || v >= Max) return 0;
        if (v < OptMin) return (v - Min) / (OptMin - Min);
        if (v <= OptMax) return 1;
        return (Max - v) / (Max - OptMax);
    }
}

/// <summary>The farmland factor of a crop by elevation type (flat / lowland / hills / mountains).</summary>
public readonly record struct Farmland(double Flat, double Lowland, double Hills, double Mountains)
{
    public double Of(ElevationType type) => type switch
    {
        ElevationType.FlatLand => Flat,
        ElevationType.Lowland => Lowland,
        ElevationType.Hills => Hills,
        ElevationType.Mountains => Mountains,
        _ => 0,
    };
}

/// <summary>One crop of Appendix B.</summary>
/// <param name="Number">The crop number 0 … 107 in the listed order; it is part of the gate's hash purpose.</param>
/// <param name="Resource">The resource the crop counts for.</param>
/// <param name="Name">The variant name.</param>
/// <param name="Temperature">The trapezoid over the growing-season temperature, °C.</param>
/// <param name="Water">The trapezoid over the yearly water, mm.</param>
/// <param name="Farmland">The farmland factor by elevation type.</param>
/// <param name="MinT">The lower limit on the yearly mean temperature, if any.</param>
/// <param name="MaxT">The upper limit on the yearly mean temperature, if any.</param>
/// <param name="Tree">A tree crop reads tree ground instead of season · soil.</param>
/// <param name="Needs">The requirement.</param>
/// <param name="Gate">The share of zones where the crop grows at all; 0 = not gated.</param>
/// <param name="K">The yield factor (1 when the table leaves it blank).</param>
public sealed record Crop(
    int Number,
    Resource Resource,
    string Name,
    Trapezoid Temperature,
    Trapezoid Water,
    Farmland Farmland,
    double? MinT,
    double? MaxT,
    bool Tree,
    CropNeed Needs,
    double Gate,
    double K)
{
    public bool IsGated => Gate > 0;
}

/// <summary>
/// The crop tables (map-generation.md Appendix B): 108 crops over seven resources, numbered in
/// the listed order, grain first and dyes last.
/// </summary>
public static class CropTables
{
    /// <summary>All crops; the index is the crop number.</summary>
    public static IReadOnlyList<Crop> All { get; }

    /// <summary>The seven crop resources, in table order.</summary>
    public static IReadOnlyList<Resource> CropResources { get; } =
    [
        Resource.Grain, Resource.Produce, Resource.Orchard, Resource.Fibre,
        Resource.Spices, Resource.StimulantsAndSugar, Resource.Dyes,
    ];

    private static readonly Crop[][] ByResource;

    static CropTables()
    {
        var rows = Rows();
        for (var n = 0; n < rows.Length; n++) rows[n] = rows[n] with { Number = n };
        All = rows;
        ByResource = new Crop[ResourceCatalog.Count][];
        for (var r = 0; r < ByResource.Length; r++)
            ByResource[r] = rows.Where(c => (int)c.Resource == r).ToArray();
    }

    /// <summary>The table of one resource, in listed order; empty for a resource that is not a crop.</summary>
    public static IReadOnlyList<Crop> Of(Resource resource) => ByResource[(int)resource];

    private static Trapezoid T(double a, double b, double c, double d) => new(a, b, c, d);

    private static Trapezoid W(double a, double b, double c, double d) => new(a, b, c, d);

    private static Farmland F(double flat, double lowland, double hills, double mountains) =>
        new(flat, lowland, hills, mountains);

    private static Crop C(
        Resource resource,
        string name,
        Trapezoid t,
        Trapezoid w,
        Farmland f,
        double? minT = null,
        double? maxT = null,
        bool tree = false,
        CropNeed needs = CropNeed.None,
        double gate = 0,
        double k = 1) =>
        new(-1, resource, name, t, w, f, minT, maxT, tree, needs, gate, k);

    // Transcribed from Appendix B row by row: T = temperature °C, W = water mm, F = farmland F/L/H/M.
    private static Crop[] Rows() =>
    [
        // Grain
        C(Resource.Grain, "bread wheat", T(5, 15, 23, 27), W(300, 750, 900, 1600), F(1, 1, 0.6, 0.3)),
        C(Resource.Grain, "durum wheat", T(6, 17, 25, 30), W(400, 500, 700, 800), F(1, 1, 0.6, 0.3), minT: 9),
        C(Resource.Grain, "barley", T(2, 15, 20, 40), W(200, 500, 1000, 2000), F(1, 1, 0.6, 0.3), k: 0.85),
        C(Resource.Grain, "rye", T(3, 15, 20, 31), W(400, 600, 1000, 2000), F(1, 1, 0.6, 0.3), maxT: 12, k: 0.8),
        C(Resource.Grain, "oats", T(5, 16, 20, 30), W(250, 600, 1000, 1500), F(1, 1, 0.6, 0.3), maxT: 13, k: 0.8),
        C(Resource.Grain, "paddy rice", T(10, 20, 30, 36), W(1000, 1500, 2000, 4000), F(1, 1, 0.35, 0), minT: 12),
        C(Resource.Grain, "maize", T(10, 18, 33, 47), W(400, 600, 1200, 1800), F(1, 1, 0.6, 0.3), minT: 9),
        C(Resource.Grain, "sorghum", T(8, 22, 35, 40), W(300, 400, 600, 700), F(1, 1, 0.6, 0.3), minT: 16, k: 0.6),
        C(Resource.Grain, "pearl millet", T(12, 25, 35, 40), W(200, 400, 900, 1700), F(1, 1, 0.6, 0.3), minT: 18, k: 0.5),
        C(Resource.Grain, "finger millet", T(8, 18, 30, 35), W(300, 500, 1100, 4300), F(0.4, 0.6, 1, 0.9), minT: 14, k: 0.6),
        C(Resource.Grain, "proso millet", T(15, 20, 32, 45), W(200, 500, 750, 1000), F(1, 1, 0.6, 0.3), maxT: 16, k: 0.6),
        C(Resource.Grain, "buckwheat", T(7, 17, 27, 40), W(400, 700, 1000, 1300), F(0.6, 0.85, 1, 0.6), maxT: 12, k: 0.6),
        C(Resource.Grain, "fonio", T(18, 22, 27, 31), W(400, 900, 1600, 2800), F(1, 1, 0.6, 0.3), minT: 20, k: 0.4),
        C(Resource.Grain, "teff", T(2, 22, 28, 30), W(300, 600, 1200, 2500), F(0.4, 0.6, 1, 0.9), needs: CropNeed.Highland, k: 0.6),
        C(Resource.Grain, "quinoa", T(2, 14, 18, 35), W(250, 500, 1000, 2600), F(0.4, 0.6, 1, 0.9), needs: CropNeed.Highland, k: 0.6),

        // Produce
        C(Resource.Produce, "potato", T(7, 15, 25, 30), W(250, 500, 800, 2000), F(0.8, 1, 1, 0.7), maxT: 17),
        C(Resource.Produce, "cassava", T(10, 20, 29, 35), W(500, 1000, 1500, 5000), F(1, 1, 0.6, 0.3), minT: 18),
        C(Resource.Produce, "sweet potato", T(10, 18, 28, 38), W(350, 750, 2000, 5000), F(1, 1, 0.6, 0.3), minT: 15),
        C(Resource.Produce, "greater yam", T(14, 20, 32, 40), W(700, 1200, 4000, 8000), F(1, 1, 0.6, 0.3), minT: 20),
        C(Resource.Produce, "taro", T(10, 21, 28, 35), W(1000, 1800, 2700, 4100), F(1, 1, 0.35, 0), minT: 19),
        C(Resource.Produce, "banana and plantain", T(12, 23, 33, 42), W(650, 1200, 3600, 5000), F(1, 1, 0.6, 0.3), minT: 20, tree: true, k: 0.6),
        C(Resource.Produce, "sago palm", T(18, 25, 36, 40), W(2100, 3000, 4500, 5800), F(1, 1, 0, 0), minT: 23, tree: true, k: 0.6),
        C(Resource.Produce, "faba bean", T(5, 18, 28, 32), W(250, 650, 1000, 2600), F(1, 1, 0.6, 0.3), maxT: 16),
        C(Resource.Produce, "pea", T(4, 10, 24, 30), W(350, 800, 1200, 2500), F(1, 1, 0.6, 0.3), maxT: 14),
        C(Resource.Produce, "lentil", T(5, 15, 29, 32), W(250, 600, 1000, 2500), F(1, 1, 0.6, 0.3), minT: 8, maxT: 19),
        C(Resource.Produce, "chickpea", T(7, 15, 29, 35), W(300, 600, 1000, 1800), F(1, 1, 0.6, 0.3), minT: 10),
        C(Resource.Produce, "common bean", T(7, 16, 25, 32), W(300, 500, 2000, 4300), F(1, 1, 0.6, 0.3), minT: 10),
        C(Resource.Produce, "cowpea", T(15, 25, 35, 40), W(300, 500, 1500, 4100), F(1, 1, 0.6, 0.3), minT: 19),
        C(Resource.Produce, "cabbage and kale", T(5, 10, 25, 30), W(450, 600, 1100, 1900), F(1, 1, 0.6, 0.3), maxT: 15),
        C(Resource.Produce, "onion and alliums", T(4, 12, 25, 30), W(300, 350, 600, 2800), F(1, 1, 0.6, 0.3), k: 0.8),
        C(Resource.Produce, "carrot and root vegetables", T(3, 15, 24, 30), W(400, 600, 1200, 4000), F(1, 1, 0.6, 0.3), maxT: 16),
        C(Resource.Produce, "melons", T(15, 20, 35, 40), W(400, 500, 700, 1800), F(1, 1, 0.6, 0.3), minT: 14, k: 0.8),
        C(Resource.Produce, "enset", T(8, 16, 24, 30), W(900, 1100, 1500, 1700), F(0.4, 0.6, 1, 0.9), tree: true, needs: CropNeed.Highland),

        // Orchard
        C(Resource.Orchard, "olive", T(5, 20, 34, 40), W(200, 400, 700, 1200), F(0.6, 0.85, 1, 0.6), minT: 12, maxT: 21, tree: true),
        C(Resource.Orchard, "grape", T(10, 18, 30, 38), W(400, 700, 850, 1200), F(0.6, 0.85, 1, 0.6), minT: 9, maxT: 20, tree: true),
        C(Resource.Orchard, "date palm", T(10, 26, 45, 52), W(100, 200, 300, 400), F(1, 1, 0.35, 0), minT: 19, tree: true, needs: CropNeed.River),
        C(Resource.Orchard, "fig", T(4, 16, 26, 38), W(300, 700, 1500, 2700), F(1, 1, 0.6, 0.3), minT: 11, tree: true, k: 0.8),
        C(Resource.Orchard, "pomegranate", T(8, 23, 32, 40), W(400, 900, 1200, 4200), F(1, 1, 0.6, 0.3), minT: 13, tree: true, k: 0.8),
        C(Resource.Orchard, "almond", T(10, 12, 35, 40), W(250, 600, 900, 1500), F(0.6, 0.85, 1, 0.6), minT: 11, maxT: 21, tree: true, k: 0.8),
        C(Resource.Orchard, "pistachio", T(12, 25, 35, 40), W(250, 400, 700, 1100), F(0.6, 0.85, 1, 0.6), minT: 13, maxT: 22, tree: true, k: 0.7),
        C(Resource.Orchard, "apple", T(8, 14, 27, 33), W(500, 700, 2500, 3200), F(0.6, 0.85, 1, 0.6), minT: 5, maxT: 15, tree: true),
        C(Resource.Orchard, "pear", T(10, 20, 35, 37), W(400, 600, 900, 2100), F(0.6, 0.85, 1, 0.6), minT: 6, maxT: 16, tree: true, k: 0.8),
        C(Resource.Orchard, "peach and stone fruit", T(7, 20, 33, 35), W(750, 900, 1100, 1600), F(1, 1, 0.6, 0.3), minT: 10, maxT: 19, tree: true, k: 0.9),
        C(Resource.Orchard, "citrus", T(13, 20, 30, 38), W(450, 1200, 2000, 2700), F(1, 1, 0.6, 0.3), minT: 16, tree: true),
        C(Resource.Orchard, "walnut", T(7, 15, 30, 40), W(400, 800, 1700, 2200), F(0.6, 0.85, 1, 0.6), minT: 7, maxT: 17, tree: true, k: 0.8),
        C(Resource.Orchard, "sweet chestnut", T(7, 15, 25, 30), W(500, 900, 1300, 1600), F(0.6, 0.85, 1, 0.6), minT: 8, maxT: 16, tree: true, k: 0.8),
        C(Resource.Orchard, "hazel", T(5, 10, 24, 35), W(600, 900, 1100, 1400), F(1, 1, 0.6, 0.3), minT: 4, maxT: 14, tree: true, k: 0.6),
        C(Resource.Orchard, "coconut", T(14, 22, 34, 38), W(650, 1200, 2400, 4000), F(1, 1, 0.6, 0.3), minT: 22, tree: true, needs: CropNeed.Coast),
        C(Resource.Orchard, "oil palm", T(12, 20, 35, 38), W(1000, 1500, 3000, 8000), F(1, 1, 0.35, 0), minT: 23, tree: true),
        C(Resource.Orchard, "mango", T(8, 24, 30, 48), W(300, 600, 1500, 2600), F(1, 1, 0.6, 0.3), minT: 20, tree: true, k: 0.8),
        C(Resource.Orchard, "cashew", T(5, 15, 35, 46), W(400, 750, 1600, 4000), F(1, 1, 0.6, 0.3), minT: 21, tree: true, k: 0.7),
        C(Resource.Orchard, "shea", T(18, 24, 38, 43), W(300, 500, 1000, 1800), F(1, 1, 0.6, 0.3), minT: 23, tree: true, k: 0.7),
        C(Resource.Orchard, "baobab", T(14, 26, 36, 42), W(250, 600, 1500, 4200), F(1, 1, 0.6, 0.3), minT: 23, tree: true, k: 0.5),
        C(Resource.Orchard, "tamarind", T(12, 20, 35, 45), W(300, 800, 3000, 4500), F(1, 1, 0.6, 0.3), minT: 21, tree: true, k: 0.6),
        C(Resource.Orchard, "breadfruit", T(16, 21, 33, 40), W(1000, 1500, 3000, 3500), F(1, 1, 0.6, 0.3), minT: 23, tree: true, k: 0.8),
        C(Resource.Orchard, "Brazil nut", T(12, 20, 36, 40), W(1400, 2400, 2800, 3500), F(1, 1, 0.35, 0), minT: 23, tree: true, k: 0.7),
        C(Resource.Orchard, "avocado", T(10, 14, 40, 45), W(300, 500, 2000, 2500), F(0.4, 0.6, 1, 0.9), tree: true, needs: CropNeed.Highland, k: 0.8),
        C(Resource.Orchard, "sunflower", T(5, 17, 34, 45), W(300, 600, 1000, 1600), F(1, 1, 0.6, 0.3), minT: 6, maxT: 17, k: 0.6),
        C(Resource.Orchard, "rapeseed", T(5, 15, 25, 41), W(400, 500, 1000, 2800), F(1, 1, 0.6, 0.3), maxT: 13, k: 0.6),
        C(Resource.Orchard, "sesame", T(10, 20, 30, 40), W(300, 500, 1000, 1500), F(1, 1, 0.6, 0.3), minT: 17, k: 0.6),
        C(Resource.Orchard, "groundnut", T(10, 22, 32, 45), W(400, 600, 1500, 4000), F(1, 1, 0.6, 0.3), minT: 18, k: 0.6),
        C(Resource.Orchard, "soybean", T(10, 20, 33, 38), W(450, 600, 1500, 1800), F(1, 1, 0.6, 0.3), minT: 10, maxT: 19, k: 0.6),

        // Fibre
        C(Resource.Fibre, "flax", T(5, 16, 24, 30), W(250, 500, 800, 1300), F(1, 1, 0.6, 0.3), maxT: 13),
        C(Resource.Fibre, "hemp", T(6, 15, 28, 32), W(350, 600, 1200, 4000), F(1, 1, 0.6, 0.3), minT: 5, maxT: 17),
        C(Resource.Fibre, "cotton", T(15, 22, 36, 42), W(450, 750, 1200, 1500), F(1, 1, 0.6, 0.3), minT: 17),
        C(Resource.Fibre, "jute", T(13, 20, 37, 45), W(900, 1500, 3000, 3600), F(1, 1, 0.35, 0), minT: 21, needs: CropNeed.River),
        C(Resource.Fibre, "ramie", T(10, 17, 27, 34), W(1000, 1500, 2500, 4000), F(1, 1, 0.6, 0.3), minT: 14, k: 0.8),
        C(Resource.Fibre, "kenaf", T(10, 15, 28, 35), W(450, 600, 2000, 3000), F(1, 1, 0.6, 0.3), minT: 21, k: 0.7),
        C(Resource.Fibre, "sisal and agave", T(10, 15, 27, 45), W(500, 900, 1250, 1800), F(0.6, 0.85, 1, 0.6), minT: 18, tree: true, k: 0.7),
        C(Resource.Fibre, "abaca", T(16, 20, 29, 33), W(700, 1800, 3000, 4400), F(1, 1, 0.6, 0.3), minT: 23, tree: true, k: 0.7),
        C(Resource.Fibre, "esparto grass", T(3, 15, 25, 35), W(100, 200, 500, 600), F(0.6, 0.85, 1, 0.6), minT: 10, tree: true, k: 0.5),
        C(Resource.Fibre, "silk", T(13, 20, 28, 45), W(300, 700, 2500, 5100), F(0.6, 0.85, 1, 0.6), minT: 12, maxT: 20, tree: true, gate: 0.2),
        C(Resource.Fibre, "kapok", T(12, 17, 38, 40), W(750, 1500, 2500, 3000), F(1, 1, 0.6, 0.3), minT: 22, tree: true, k: 0.5),
        C(Resource.Fibre, "New Zealand flax", T(9, 13, 25, 30), W(1300, 2000, 3000, 3800), F(1, 1, 0.6, 0.3), minT: 8, maxT: 15, tree: true, k: 0.6),
        C(Resource.Fibre, "nettle and lime-bast", T(5, 12, 22, 28), W(400, 500, 700, 800), F(1, 1, 0.6, 0.3), maxT: 12, tree: true, k: 0.35),

        // Spices & incense
        C(Resource.Spices, "black pepper", T(10, 22, 35, 40), W(2000, 2500, 4000, 5500), F(1, 1, 0.6, 0.3), minT: 22, tree: true, gate: 0.08),
        C(Resource.Spices, "clove", T(20, 24, 34, 38), W(1200, 1500, 3600, 4000), F(1, 1, 0.35, 0), minT: 24, tree: true, needs: CropNeed.Coast, gate: 0.06),
        C(Resource.Spices, "nutmeg and mace", T(12, 22, 34, 38), W(1200, 1500, 3500, 4000), F(1, 1, 0.35, 0), minT: 24, tree: true, needs: CropNeed.Coast, gate: 0.06),
        C(Resource.Spices, "cinnamon", T(18, 24, 30, 36), W(1200, 2000, 2600, 3000), F(1, 1, 0.6, 0.3), minT: 22, tree: true, gate: 0.08),
        C(Resource.Spices, "cardamom", T(10, 22, 30, 35), W(1500, 3000, 6000, 7000), F(0.4, 0.6, 1, 0.9), minT: 20, tree: true, gate: 0.1),
        C(Resource.Spices, "ginger", T(13, 19, 29, 35), W(700, 1400, 3000, 4000), F(1, 1, 0.6, 0.3), minT: 20, gate: 0.1, k: 0.8),
        C(Resource.Spices, "turmeric", T(18, 20, 28, 32), W(800, 1000, 2000, 3000), F(1, 1, 0.6, 0.3), minT: 20, gate: 0.1, k: 0.8),
        C(Resource.Spices, "vanilla", T(10, 21, 30, 33), W(1500, 2000, 2500, 3000), F(1, 1, 0.35, 0), minT: 22, tree: true, gate: 0.05),
        C(Resource.Spices, "chili pepper", T(8, 17, 30, 35), W(500, 600, 1250, 1700), F(1, 1, 0.6, 0.3), minT: 20, gate: 0.1, k: 0.7),
        C(Resource.Spices, "myrrh", T(18, 23, 33, 40), W(250, 400, 800, 1000), F(0.6, 0.85, 1, 0.6), minT: 22, tree: true, gate: 0.1),
        C(Resource.Spices, "frankincense", T(18, 26, 40, 47), W(100, 150, 400, 600), F(0, 0.5, 1, 1), minT: 22, tree: true, gate: 0.1),
        C(Resource.Spices, "gum arabic", T(14, 24, 32, 48), W(200, 300, 800, 2300), F(1, 1, 0.6, 0.3), minT: 21, tree: true, gate: 0.12, k: 0.7),
        C(Resource.Spices, "sandalwood", T(10, 22, 30, 38), W(450, 800, 1400, 3000), F(0.6, 0.85, 1, 0.6), minT: 21, tree: true, gate: 0.07),
        C(Resource.Spices, "rubber latex", T(10, 24, 33, 45), W(1200, 2000, 4000, 6000), F(1, 1, 0.35, 0), minT: 23, tree: true, gate: 0.07, k: 0.7),

        // Stimulants & sugar
        C(Resource.StimulantsAndSugar, "sugar cane", T(15, 24, 37, 41), W(1000, 1500, 2000, 5000), F(1, 1, 0.35, 0), minT: 20, gate: 0.2),
        C(Resource.StimulantsAndSugar, "palm sugar and toddy", T(16, 24, 32, 36), W(700, 1000, 1500, 4200), F(1, 1, 0.6, 0.3), minT: 22, tree: true, gate: 0.12, k: 0.5),
        C(Resource.StimulantsAndSugar, "tea", T(8, 20, 30, 35), W(1000, 1400, 2000, 5000), F(0, 0.4, 1, 1), minT: 12, tree: true, gate: 0.15),
        C(Resource.StimulantsAndSugar, "coffee", T(10, 14, 28, 34), W(750, 1400, 2300, 4200), F(0, 0.4, 1, 1), tree: true, needs: CropNeed.Highland, gate: 0.3),
        C(Resource.StimulantsAndSugar, "cacao", T(10, 21, 32, 38), W(900, 1200, 3000, 7600), F(1, 1, 0.35, 0), minT: 22, tree: true, gate: 0.12),
        C(Resource.StimulantsAndSugar, "kola nut", T(10, 21, 30, 35), W(1200, 1400, 2500, 3500), F(1, 1, 0.35, 0), minT: 23, tree: true, gate: 0.08, k: 0.7),
        C(Resource.StimulantsAndSugar, "betel nut", T(7, 16, 33, 39), W(750, 1500, 3000, 5000), F(1, 1, 0.6, 0.3), minT: 21, tree: true, gate: 0.1, k: 0.7),
        C(Resource.StimulantsAndSugar, "coca", T(14, 17, 23, 27), W(700, 1000, 2100, 4000), F(0, 0, 0.8, 1), tree: true, needs: CropNeed.Highland, gate: 0.15, k: 0.8),
        C(Resource.StimulantsAndSugar, "tobacco", T(7, 15, 30, 35), W(350, 500, 750, 3000), F(1, 1, 0.6, 0.3), minT: 11, gate: 0.12, k: 0.8),

        // Dyes
        C(Resource.Dyes, "woad", T(5, 14, 22, 28), W(450, 600, 1000, 1500), F(1, 1, 0.6, 0.3), maxT: 14, gate: 0.12, k: 0.7),
        C(Resource.Dyes, "madder", T(8, 16, 26, 32), W(350, 500, 900, 1300), F(1, 1, 0.6, 0.3), minT: 9, maxT: 19, gate: 0.12, k: 0.7),
        C(Resource.Dyes, "weld", T(6, 14, 24, 30), W(400, 500, 900, 1200), F(1, 1, 0.6, 0.3), minT: 7, maxT: 17, gate: 0.1, k: 0.5),
        C(Resource.Dyes, "indigo", T(7, 22, 28, 32), W(640, 1300, 1700, 3000), F(1, 1, 0.6, 0.3), minT: 19, gate: 0.15),
        C(Resource.Dyes, "safflower", T(5, 20, 32, 45), W(300, 600, 1000, 1400), F(1, 1, 0.6, 0.3), minT: 13, gate: 0.12, k: 0.6),
        C(Resource.Dyes, "henna", T(13, 19, 27, 33), W(250, 500, 1500, 4200), F(1, 1, 0.6, 0.3), minT: 20, tree: true, gate: 0.12, k: 0.6),
        C(Resource.Dyes, "annatto", T(15, 20, 28, 36), W(1250, 1500, 2000, 2500), F(1, 1, 0.6, 0.3), minT: 22, tree: true, gate: 0.1, k: 0.6),
        C(Resource.Dyes, "red dyewoods", T(7, 26, 32, 46), W(875, 1000, 1900, 2800), F(1, 1, 0.6, 0.3), minT: 22, tree: true, gate: 0.07),
        C(Resource.Dyes, "lac", T(12, 22, 32, 40), W(700, 1000, 1800, 2600), F(1, 1, 0.6, 0.3), minT: 21, tree: true, gate: 0.1, k: 0.7),
        C(Resource.Dyes, "cochineal", T(10, 18, 26, 30), W(300, 700, 1300, 1700), F(0.6, 0.85, 1, 0.6), minT: 14, tree: true, gate: 0.06),
    ];
}
