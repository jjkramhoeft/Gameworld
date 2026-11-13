namespace Gameworld.Domain.Model;

/// <summary>
/// What a generation step draws randomness for. The numeric values feed the seed hash, so
/// they must never change or be reused - doing so is a generator version change.
/// </summary>
public enum GenerationPurpose
{
    RegionSite = 1,
    ZoneCellCount = 2,
    ZoneCellSite = 3,
    PatchCellCount = 4,
    PatchCellSite = 5,
    OceanSeeds = 6,
    ElevationType = 7,
    Bedrock = 8,
    OceanLink = 9,
    OceanExpansionCount = 10,
    ZoneMergerCount = 11,
    ZoneMergerChoice = 12,
    PatchMergerCount = 13,
    PatchMergerChoice = 14,
    PatchBorderWarp = 15,
    Rivers = 16,
    Lakes = 17,
    PrecipitationField = 18,
    InitialTemperatureField = 19,
    ProsperityField = 20,
    SupernaturalField = 21,
}

/// <summary>
/// There is no shared random stream. Every generation step seeds itself from a hash of
/// (foundation seed, entity identity, purpose), so the result is independent of the order
/// regions are generated in. The parameter set is deliberately not part of the hash:
/// changing one parameter keeps every other roll the same.
/// </summary>
public static class SeedHash
{
    /// <summary>The seed for a step that belongs to the whole foundation, e.g. a field.</summary>
    public static ulong For(WorldSeed seed, GenerationPurpose purpose) =>
        Start(seed, purpose, level: 0);

    public static ulong For(WorldSeed seed, RegionId region, GenerationPurpose purpose) =>
        Mix(Start(seed, purpose, level: 1), region);

    public static ulong For(WorldSeed seed, ZoneCellId cell, GenerationPurpose purpose) =>
        Mix(Mix(Start(seed, purpose, level: 2), cell.Region), cell.Index);

    public static ulong For(WorldSeed seed, PatchCellId cell, GenerationPurpose purpose) =>
        Mix(Mix(Mix(Start(seed, purpose, level: 3), cell.Region), cell.ZoneCell.Index), cell.Index);

    private static ulong Start(WorldSeed seed, GenerationPurpose purpose, int level) =>
        Mix(Mix(Finalize(seed.Value), (int)purpose), level);

    private static ulong Mix(ulong hash, RegionId region) => Mix(Mix(hash, region.X), region.Y);

    private static ulong Mix(ulong hash, int value) =>
        Finalize(hash ^ (unchecked((ulong)value) + 0x9E3779B97F4A7C15UL + (hash << 6) + (hash >> 2)));

    /// <summary>The SplitMix64 finaliser.</summary>
    private static ulong Finalize(ulong z)
    {
        z = (z ^ (z >> 30)) * 0xBF58476D1CE4E5B9UL;
        z = (z ^ (z >> 27)) * 0x94D049BB133111EBUL;
        return z ^ (z >> 31);
    }
}
