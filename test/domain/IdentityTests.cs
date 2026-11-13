using System.Globalization;
using Gameworld.Domain.Model;

namespace Gameworld.Domain.Tests;

public class IdentityTests
{
    private static readonly RegionId Region = new(3, -2);
    private static readonly ZoneCellId ZoneCell = new(Region, 4);
    private static readonly PatchCellId PatchCell = new(ZoneCell, 11);

    [Fact]
    public void Identities_print_as_paths()
    {
        Assert.Equal("r3,-2", Region.ToString());
        Assert.Equal("r3,-2/z4", ZoneCell.ToString());
        Assert.Equal("r3,-2/z4/p11", PatchCell.ToString());
        Assert.Equal("r3,-2/river1", new RiverId(Region, 1).ToString());
        Assert.Equal("r3,-2/lake0", new LakeId(Region, 0).ToString());
    }

    [Fact]
    public void A_zone_and_a_patch_print_as_the_path_of_their_surviving_cell()
    {
        Assert.Equal(ZoneCell.ToString(), new ZoneId(ZoneCell).ToString());
        Assert.Equal(PatchCell.ToString(), new PatchId(PatchCell).ToString());
    }

    [Fact]
    public void Paths_do_not_depend_on_the_culture_of_the_machine()
    {
        var culture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
        culture.NumberFormat.NegativeSign = "−";
        var previous = CultureInfo.CurrentCulture;
        CultureInfo.CurrentCulture = culture;
        try
        {
            Assert.Equal("r-1200,-2/z4/p11", new PatchCellId(new ZoneCellId(new RegionId(-1200, -2), 4), 11).ToString());
        }
        finally
        {
            CultureInfo.CurrentCulture = previous;
        }
    }

    [Fact]
    public void A_cell_knows_its_region()
    {
        Assert.Equal(Region, PatchCell.Region);
        Assert.Equal(Region, new ZoneId(ZoneCell).Region);
        Assert.Equal(Region, new PatchId(PatchCell).Region);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 8)]
    [InlineData(2, 16)]
    [InlineData(3, 24)]
    public void A_ring_holds_the_regions_exactly_that_many_rings_away(int ring, int count)
    {
        var regions = Region.Ring(ring).ToList();

        Assert.Equal(count, regions.Distinct().Count());
        Assert.All(regions, region => Assert.Equal(ring, Region.RingsTo(region)));
    }

    [Fact]
    public void Within_holds_the_region_itself_and_every_ring_out_to_the_distance()
    {
        var expected = Region.Ring(0).Concat(Region.Ring(1)).Concat(Region.Ring(2)).ToHashSet();

        Assert.Equal(25, expected.Count);
        Assert.Equal(expected, Region.Within(2).ToHashSet());
    }

    [Fact]
    public void Diagonal_neighbours_are_one_ring_apart()
    {
        Assert.Equal(1, Region.RingsTo(Region.Offset(-1, 1)));
        Assert.Equal(2, Region.RingsTo(Region.Offset(2, -1)));
    }
}
