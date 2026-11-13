namespace Gameworld.Domain.Model;

/// <summary>
/// Identity of a zone cell: region + index. A cell is the Voronoi site fixed when the region
/// is Sited, and its path never changes.
/// </summary>
public readonly record struct ZoneCellId(RegionId Region, int Index)
{
    public override string ToString() => FormattableString.Invariant($"{Region}/z{Index}");
}

/// <summary>
/// Identity of a patch cell: zone cell + index. The parent is always the original zone cell,
/// also when that zone cell has been absorbed by another zone.
/// </summary>
public readonly record struct PatchCellId(ZoneCellId ZoneCell, int Index)
{
    public RegionId Region => ZoneCell.Region;

    public override string ToString() => FormattableString.Invariant($"{ZoneCell}/p{Index}");
}

/// <summary>
/// Identity of a zone: the path of its surviving zone cell. The indices of absorbed zone
/// cells are retired, so there can be gaps.
/// </summary>
public readonly record struct ZoneId(ZoneCellId Survivor)
{
    public RegionId Region => Survivor.Region;

    public override string ToString() => Survivor.ToString();
}

/// <summary>
/// Identity of a patch: the path of its surviving patch cell. The zone a patch belongs to
/// cannot be read from this path - see <see cref="Patch.Zone"/>.
/// </summary>
public readonly record struct PatchId(PatchCellId Survivor)
{
    public RegionId Region => Survivor.Region;

    public override string ToString() => Survivor.ToString();
}
