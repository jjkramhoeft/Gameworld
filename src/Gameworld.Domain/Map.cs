namespace Gameworld.Domain;

/// <summary>A position on the unbounded map plane, in metres. X runs east and Y runs south.</summary>
public readonly record struct WorldPoint(double X, double Y)
{
    public double DistanceTo(WorldPoint other)
    {
        var dx = X - other.X;
        var dy = Y - other.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}

/// <summary>The three levels of nested Voronoi cells.</summary>
public enum CellLevel
{
    Region,
    Zone,
    Patch,
}

/// <summary>
/// The authoritative elevation class of a zone. It does not give a height band - it gives how
/// steeply the land rises from rivers and coast, the gradient of the rivers and the roughness
/// of the surface.
/// </summary>
public enum ElevationType
{
    Ocean,
    FlatLand,
    Lowland,
    Hills,
    Mountains,
}

/// <summary>The four Fields: stateless functions of foundation and coordinate.</summary>
public enum FieldKind
{
    Precipitation,
    InitialTemperature,
    Prosperity,
    Supernatural,
}

/// <summary>The thermal belts of the biome classification, coldest first.</summary>
public enum ThermalBelt
{
    Polar,
    Subpolar,
    Boreal,
    Temperate,
    Tropical,
}

/// <summary>The current set of biomes: 15 land biomes, and the two overrides Mangrove and Volcano.</summary>
public enum Biome
{
    IceAndSnow,
    Tundra,
    ColdSteppe,
    BorealForest,
    ColdDesert,
    TemperateGrassland,
    WoodlandAndShrubland,
    TemperateMixedForest,
    TemperateBroadleafForest,
    TemperateRainForest,
    HotDesert,
    ThornScrub,
    Savanna,
    TropicalDryForest,
    TropicalRainForest,
    Mangrove,
    Volcano,
}

/// <summary>
/// The biome of a land patch with the climate it came from. The temperature is the Initial
/// Temperature field cooled by the height of the patch; the aridity is the precipitation
/// relative to what the temperature demands.
/// </summary>
public sealed record BiomeRecord(
    Biome Biome,
    ThermalBelt Belt,
    double TemperatureC,
    double SeaLevelTemperatureC,
    double PrecipitationMm,
    double Aridity);

/// <summary>
/// A zone cell as it is when its region is Sited: the Voronoi site of one square of the zone
/// grid, with the facts drawn for it from the foundation alone.
/// </summary>
/// <param name="Id">The coordinate in the zone grid. It never changes.</param>
/// <param name="Site">The Voronoi site.</param>
/// <param name="Owner">The region that contains the site. Derived, not part of the identity.</param>
/// <param name="ElevationType">The initial type: Ocean for an ocean seed, else the land type drawn by weight.</param>
/// <param name="Bedrock">The bedrock id drawn by the weights of the elevation type; null for an ocean seed.</param>
/// <param name="IsOceanSeed">One of the zone cells its region chose as ocean seeds.</param>
/// <param name="Links">The determined number of ocean links (0, 1 or 2). 0 for a cell that is not an ocean seed.</param>
/// <param name="Expansions">The determined number of land neighbours the cell converts in each expansion pass, if it expands in that pass.</param>
/// <param name="MergesAway">One of the zone cells its region chose to merge into a neighbour.</param>
public sealed record ZoneCell(
    ZoneCellId Id,
    WorldPoint Site,
    RegionId Owner,
    ElevationType ElevationType,
    string? Bedrock,
    bool IsOceanSeed,
    int Links,
    IReadOnlyList<int> Expansions,
    bool MergesAway);

/// <summary>A patch cell as it is when its region is Sited.</summary>
/// <param name="Id">The coordinate in the patch grid. It never changes.</param>
/// <param name="Site">The Voronoi site.</param>
/// <param name="Owner">The zone cell that contains the site. Derived, not part of the identity.</param>
/// <param name="MergesAway">One of the patch cells its zone cell chose to merge into a neighbour.</param>
public sealed record PatchCell(
    PatchCellId Id,
    WorldPoint Site,
    ZoneCellId Owner,
    bool MergesAway);

/// <summary>
/// What the Sited function yields for a region. It needs the foundation only and is never persisted.
/// </summary>
/// <param name="Id">The coordinate in the region grid.</param>
/// <param name="Site">The Voronoi site of the region.</param>
/// <param name="ZoneCells">The zone cells the region owns, in grid order.</param>
/// <param name="PatchCells">The patch cells owned by those zone cells, per zone cell in grid order.</param>
/// <param name="ZoneMergers">The determined number of zone mergers of the region.</param>
public sealed record SitedRegion(
    RegionId Id,
    WorldPoint Site,
    IReadOnlyList<ZoneCell> ZoneCells,
    IReadOnlyList<PatchCell> PatchCells,
    int ZoneMergers)
{
    public IEnumerable<ZoneCell> OceanSeeds => ZoneCells.Where(c => c.IsOceanSeed);
}

/// <summary>
/// A zone: one or more zone cells that have merged. It takes everything from its root cell:
/// ocean or land, the region, the elevation type and the bedrock.
/// </summary>
/// <param name="Id">The identity of the root cell.</param>
/// <param name="Region">The region that owns the root cell.</param>
/// <param name="Cells">All zone cells with this root, sorted by key. They can be owned by other regions.</param>
/// <param name="ElevationType">Ocean, or the land type of the root cell.</param>
/// <param name="Bedrock">The bedrock id of the root cell; null for an ocean zone.</param>
public sealed record Zone(
    ZoneId Id,
    RegionId Region,
    IReadOnlyList<ZoneCellId> Cells,
    ElevationType ElevationType,
    string? Bedrock)
{
    public bool IsOcean => ElevationType == ElevationType.Ocean;
}

/// <summary>
/// A patch: one or more patch cells that have merged. It belongs to the zone of its root cell.
/// For gameplay a merged patch is one large cell.
/// </summary>
/// <param name="Id">The identity of the root cell.</param>
/// <param name="Zone">The zone the patch belongs to.</param>
/// <param name="Site">The site of the root cell.</param>
/// <param name="Cells">All patch cells with this root, sorted by key.</param>
/// <param name="IsOcean">The patch belongs to an ocean zone.</param>
/// <param name="IsCoastal">One of its patch cells has a neighbour on the other side of the coast.</param>
/// <param name="Biome">The biome of a land patch; null for an ocean patch.</param>
/// <param name="Resources">The resources of the patch. An ocean patch only carries sea resources.</param>
/// <param name="Flora">The ids of the kinds of flora of the patch, in the order of the Parameter Set. They fit its biome; an ocean patch holds flora of the sea.</param>
/// <param name="Fauna">The ids of the kinds of fauna of the patch, in the order of the Parameter Set. They fit its biome; an ocean patch holds fauna of the sea.</param>
public sealed record Patch(
    PatchId Id,
    ZoneId Zone,
    WorldPoint Site,
    IReadOnlyList<PatchCellId> Cells,
    bool IsOcean,
    bool IsCoastal,
    BiomeRecord? Biome,
    PatchResources Resources,
    IReadOnlyList<string> Flora,
    IReadOnlyList<string> Fauna);

/// <summary>
/// A node of a river tree. Rivers grow from the sea into the land, so the root of a tree sits
/// in the sea and the flow runs from the leaves to the root.
/// </summary>
/// <param name="Position">The position of the node.</param>
/// <param name="Parent">The index of the node downstream, or -1 for the root of a tree.</param>
/// <param name="Flow">The attractors reached at this node and upstream of it. It stands in for drainage area and discharge.</param>
/// <param name="Wet">The node lies over ocean (the hidden stretch between the root and the coast).</param>
/// <param name="Tree">The index of the root of the node's tree. One tree is one river system.</param>
public readonly record struct RiverNode(WorldPoint Position, int Parent, int Flow, bool Wet, int Tree)
{
    public bool IsRoot => Parent < 0;
}

/// <summary>
/// The rivers of a region. A river never runs on the land of another region, except in a
/// landlocked region whose rivers cross a neighbour to reach an ocean.
/// </summary>
/// <param name="Nodes">All river nodes of the region. A parent always has a smaller index than its children.</param>
/// <param name="CrossesNeighbours">The region is landlocked and its rivers may cross the land of neighbouring regions.</param>
public sealed record Rivers(IReadOnlyList<RiverNode> Nodes, bool CrossesNeighbours);

/// <summary>
/// A lake. It always sits on a river, so it has a river running in and out. Lakes do not
/// change the height map; they count as fresh water for resources and Prospects.
/// </summary>
/// <param name="Patch">The patch the lake is placed in.</param>
/// <param name="Centre">The river node position the lake is centred on.</param>
/// <param name="Outline">The outline polygon, star-shaped around the centre.</param>
/// <param name="AreaM2">The area of the outline.</param>
public sealed record Lake(PatchId Patch, WorldPoint Centre, IReadOnlyList<WorldPoint> Outline, double AreaM2);

/// <summary>
/// The height map of a region: a world-aligned raster. Cell (i, j) has its centre at
/// ((i + 0.5)·ResolutionM, (j + 0.5)·ResolutionM). Heights are metres above sea level, negative in the sea.
/// </summary>
public sealed record HeightMap(int OriginI, int OriginJ, int Width, int Height, double ResolutionM, float[] Heights)
{
    public bool Contains(int i, int j) =>
        i >= OriginI && j >= OriginJ && i < OriginI + Width && j < OriginJ + Height;

    public float At(int i, int j) => Heights[(j - OriginJ) * Width + (i - OriginI)];

    /// <summary>The height at a point, read with bilinear interpolation between cell centres.</summary>
    public double Sample(WorldPoint p)
    {
        var fx = p.X / ResolutionM - 0.5 - OriginI;
        var fy = p.Y / ResolutionM - 0.5 - OriginJ;
        fx = Math.Clamp(fx, 0, Width - 1);
        fy = Math.Clamp(fy, 0, Height - 1);
        var x0 = Math.Min((int)fx, Width - 2 < 0 ? 0 : Width - 2);
        var y0 = Math.Min((int)fy, Height - 2 < 0 ? 0 : Height - 2);
        var x1 = Math.Min(x0 + 1, Width - 1);
        var y1 = Math.Min(y0 + 1, Height - 1);
        var tx = fx - x0;
        var ty = fy - y0;
        var a = Heights[y0 * Width + x0] + tx * (Heights[y0 * Width + x1] - Heights[y0 * Width + x0]);
        var b = Heights[y1 * Width + x0] + tx * (Heights[y1 * Width + x1] - Heights[y1 * Width + x0]);
        return a + ty * (b - a);
    }
}
