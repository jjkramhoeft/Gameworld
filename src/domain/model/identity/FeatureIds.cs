namespace Gameworld.Domain.Model;

/// <summary>Identity of a river: region + index. Rivers never cross region borders.</summary>
public readonly record struct RiverId(RegionId Region, int Index)
{
    public override string ToString() => FormattableString.Invariant($"{Region}/river{Index}");
}

/// <summary>Identity of a lake: region + index.</summary>
public readonly record struct LakeId(RegionId Region, int Index)
{
    public override string ToString() => FormattableString.Invariant($"{Region}/lake{Index}");
}
