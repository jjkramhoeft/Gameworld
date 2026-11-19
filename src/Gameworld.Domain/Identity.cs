namespace Gameworld.Domain;

/// <summary>
/// Integer indices of a grid square. Every grid square of a level holds exactly one Voronoi site,
/// so a grid coordinate is the deterministic identity of a cell.
/// </summary>
public readonly record struct GridCoord(int I, int J) : IComparable<GridCoord>
{
    /// <summary>
    /// Packs (i, j) into one number: (i + 2^21)·2^22 + (j + 2^21). Keys order cells by i first,
    /// then j. Several generation steps break ties "by key".
    /// </summary>
    public long Key => ((long)I + (1L << 21)) * (1L << 22) + ((long)J + (1L << 21));

    public static GridCoord FromKey(long key) =>
        new((int)(key / (1L << 22) - (1L << 21)), (int)(key % (1L << 22) - (1L << 21)));

    public int CompareTo(GridCoord other) => Key.CompareTo(other.Key);

    public GridCoord Offset(int di, int dj) => new(I + di, J + dj);

    public override string ToString() => $"({I}, {J})";
}

/// <summary>Region = coordinate in the region grid.</summary>
public readonly record struct RegionId(GridCoord Coord)
{
    public RegionId(int i, int j) : this(new GridCoord(i, j)) { }
    public override string ToString() => $"R{Coord}";
}

/// <summary>Zone cell = coordinate in the zone grid.</summary>
public readonly record struct ZoneCellId(GridCoord Coord)
{
    public ZoneCellId(int i, int j) : this(new GridCoord(i, j)) { }
    public override string ToString() => $"Z{Coord}";
}

/// <summary>Patch cell = coordinate in the patch grid.</summary>
public readonly record struct PatchCellId(GridCoord Coord)
{
    public PatchCellId(int i, int j) : this(new GridCoord(i, j)) { }
    public override string ToString() => $"P{Coord}";
}

/// <summary>
/// A Zone is one or more zone cells that have merged. It has the identity of its root cell -
/// the cell the others have merged into.
/// </summary>
public readonly record struct ZoneId(ZoneCellId Root)
{
    public ZoneId(int i, int j) : this(new ZoneCellId(i, j)) { }
    public GridCoord Coord => Root.Coord;
    public override string ToString() => $"Zone{Root.Coord}";
}

/// <summary>
/// A Patch is one or more patch cells that have merged. It has the identity of its root cell,
/// and it belongs to the zone of that cell.
/// </summary>
public readonly record struct PatchId(PatchCellId Root)
{
    public PatchId(int i, int j) : this(new PatchCellId(i, j)) { }
    public GridCoord Coord => Root.Coord;
    public override string ToString() => $"Patch{Root.Coord}";
}

/// <summary>The four kinds of entities with a generation state of their own.</summary>
public enum EntityKind
{
    City,
    Faction,
    Family,
    Person,
}

/// <summary>
/// City, Faction, Family and Person = parent + slot index. The identity is deterministic, so a
/// Stub is addressable before it is Realised.
/// </summary>
/// <remarks>
/// The parent of a faction is its region (<see cref="Faction.IdIn"/>). The readme does not yet
/// decide the parent of a city, a family and a person, so the parent is a type parameter here.
/// </remarks>
public readonly record struct SlotId<TParent>(TParent Parent, int Slot)
    where TParent : struct
{
    public override string ToString() => $"{Parent}#{Slot}";
}
