using Gameworld.Domain.Model;

namespace Gameworld.Domain.Tests;

public class SeedHashTests
{
    private static readonly WorldSeed Seed = new(42);
    private static readonly RegionId Region = new(3, -2);
    private static readonly ZoneCellId ZoneCell = new(Region, 4);
    private static readonly PatchCellId PatchCell = new(ZoneCell, 11);

    /// <summary>
    /// Every generated map depends on these values. If this test fails the generation
    /// algorithms have changed, and that is a generator version change.
    /// </summary>
    [Fact]
    public void Hashes_are_pinned()
    {
        Assert.Equal(4879427608721286176UL, SeedHash.For(Seed, GenerationPurpose.PrecipitationField));
        Assert.Equal(2222910039078826818UL, SeedHash.For(Seed, Region, GenerationPurpose.RegionSite));
        Assert.Equal(11357925643223128520UL, SeedHash.For(Seed, ZoneCell, GenerationPurpose.Bedrock));
        Assert.Equal(9148150788426051924UL, SeedHash.For(Seed, PatchCell, GenerationPurpose.PatchCellSite));
        Assert.Equal(10817279086581313707UL, SeedHash.For(new WorldSeed(0), new RegionId(0, 0), GenerationPurpose.RegionSite));
    }

    /// <summary>The numeric values feed the hash, so they must never change or be reused.</summary>
    [Fact]
    public void Purpose_values_are_pinned()
    {
        var expected = new Dictionary<string, int>
        {
            ["RegionSite"] = 1,
            ["ZoneCellCount"] = 2,
            ["ZoneCellSite"] = 3,
            ["PatchCellCount"] = 4,
            ["PatchCellSite"] = 5,
            ["OceanSeeds"] = 6,
            ["ElevationType"] = 7,
            ["Bedrock"] = 8,
            ["OceanLink"] = 9,
            ["OceanExpansionCount"] = 10,
            ["ZoneMergerCount"] = 11,
            ["ZoneMergerChoice"] = 12,
            ["PatchMergerCount"] = 13,
            ["PatchMergerChoice"] = 14,
            ["PatchBorderWarp"] = 15,
            ["Rivers"] = 16,
            ["Lakes"] = 17,
            ["PrecipitationField"] = 18,
            ["InitialTemperatureField"] = 19,
            ["ProsperityField"] = 20,
            ["SupernaturalField"] = 21,
        };

        var actual = Enum.GetValues<GenerationPurpose>().ToDictionary(purpose => purpose.ToString(), purpose => (int)purpose);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public void The_seed_changes_the_hash()
    {
        Assert.NotEqual(
            SeedHash.For(Seed, Region, GenerationPurpose.RegionSite),
            SeedHash.For(new WorldSeed(43), Region, GenerationPurpose.RegionSite));
    }

    [Fact]
    public void The_purpose_changes_the_hash()
    {
        Assert.NotEqual(
            SeedHash.For(Seed, ZoneCell, GenerationPurpose.Bedrock),
            SeedHash.For(Seed, ZoneCell, GenerationPurpose.ElevationType));
    }

    [Fact]
    public void A_region_and_its_first_cells_do_not_share_a_hash()
    {
        var firstZoneCell = new ZoneCellId(Region, 0);
        var hashes = new[]
        {
            SeedHash.For(Seed, GenerationPurpose.Bedrock),
            SeedHash.For(Seed, Region, GenerationPurpose.Bedrock),
            SeedHash.For(Seed, firstZoneCell, GenerationPurpose.Bedrock),
            SeedHash.For(Seed, new PatchCellId(firstZoneCell, 0), GenerationPurpose.Bedrock),
        };

        Assert.Equal(hashes.Length, hashes.Distinct().Count());
    }

    [Fact]
    public void Swapping_the_coordinates_of_a_region_changes_the_hash()
    {
        Assert.NotEqual(
            SeedHash.For(Seed, new RegionId(3, -2), GenerationPurpose.RegionSite),
            SeedHash.For(Seed, new RegionId(-2, 3), GenerationPurpose.RegionSite));
    }

    [Fact]
    public void Hashes_are_distinct_and_evenly_spread_over_a_block_of_regions()
    {
        var hashes = new HashSet<ulong>();
        var buckets = new int[16];
        for (var x = -100; x < 100; x++)
            for (var y = -100; y < 100; y++)
            {
                var hash = SeedHash.For(Seed, new RegionId(x, y), GenerationPurpose.RegionSite);
                hashes.Add(hash);
                buckets[hash >> 60]++;
            }

        Assert.Equal(40_000, hashes.Count);
        Assert.All(buckets, count => Assert.InRange(count, 2_250, 2_750));
    }
}
