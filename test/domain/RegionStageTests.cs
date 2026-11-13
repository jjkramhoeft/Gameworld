using Gameworld.Domain.Model;

namespace Gameworld.Domain.Tests;

public class RegionStageTests
{
    private static readonly RegionId Centre = new(3, -2);

    private static SitedRegion Sited(RegionId id) => new(id, new WorldPoint(id.X * 150, id.Y * 150), [], new OutlineQuota([], 0));

    [Fact]
    public void A_neighbourhood_holds_the_centre_and_every_region_within_its_rings()
    {
        var neighbourhood = new Neighbourhood<SitedRegion>(Centre, 2, Centre.Within(2).Select(Sited));

        Assert.Equal(Centre, neighbourhood.Centre.Id);
        Assert.Equal(25, neighbourhood.All.Count);
        Assert.Equal(8, neighbourhood.Ring(1).Count());
        Assert.Equal(16, neighbourhood.Ring(2).Count());
        Assert.Equal(Centre.Offset(2, -2), neighbourhood[Centre.Offset(2, -2)].Id);
    }

    [Fact]
    public void A_neighbourhood_cannot_be_made_with_a_region_missing()
    {
        var incomplete = Centre.Within(2).Where(id => id != Centre.Offset(2, 0)).Select(Sited);

        var error = Assert.Throws<ArgumentException>(() => new Neighbourhood<SitedRegion>(Centre, 2, incomplete));

        Assert.Contains(Centre.Offset(2, 0).ToString(), error.Message);
    }

    [Fact]
    public void A_neighbourhood_leaves_out_regions_beyond_its_rings()
    {
        var neighbourhood = new Neighbourhood<SitedRegion>(Centre, 1, Centre.Within(3).Select(Sited));

        Assert.Equal(9, neighbourhood.All.Count);
        Assert.Throws<KeyNotFoundException>(() => neighbourhood[Centre.Offset(2, 0)]);
        Assert.Throws<ArgumentOutOfRangeException>(() => neighbourhood.Ring(2));
    }

    /// <summary>The Needs column of the Region Generation Stages table in the readme.</summary>
    [Theory]
    [InlineData(RegionStage.Outlined, null, 2)]
    [InlineData(RegionStage.Detailed, RegionStage.Outlined, 1)]
    [InlineData(RegionStage.Elevated, RegionStage.Detailed, 1)]
    [InlineData(RegionStage.Planned, RegionStage.Detailed, 0)]
    [InlineData(RegionStage.Settled, RegionStage.Planned, 0)]
    public void Stage_preconditions_follow_the_readme(RegionStage stage, RegionStage? neighbourStage, int rings)
    {
        Assert.Equal(new StagePrecondition(neighbourStage, rings), RegionStages.PreconditionOf(stage));
    }

    [Fact]
    public void Stages_only_advance_in_the_order_of_the_readme()
    {
        Assert.Equal(
            [RegionStage.Outlined, RegionStage.Detailed, RegionStage.Elevated, RegionStage.Planned, RegionStage.Settled],
            Enum.GetValues<RegionStage>().Order());
    }

    [Fact]
    public void An_outlined_region_has_sea_access_when_it_has_an_ocean_zone()
    {
        var land = new ZoneCellId(Centre, 0);
        var ocean = new ZoneCellId(Centre, 1);
        var bedrock = new BedrockId("shield");
        var landZone = new Zone(new ZoneId(land), [land], ElevationType.Hills, bedrock, OceanOrigin: null);
        var oceanZone = new Zone(new ZoneId(ocean), [ocean, new ZoneCellId(Centre, 2)], ElevationType.Ocean, bedrock, OceanOrigin.Seed);

        var landlocked = new OutlinedRegion(Sited(Centre), [landZone]);
        var coastal = new OutlinedRegion(Sited(Centre), [landZone, oceanZone]);

        Assert.False(landlocked.HasSeaAccess);
        Assert.True(coastal.HasSeaAccess);
        Assert.Equal(oceanZone.Id, coastal.ZoneOf(new ZoneCellId(Centre, 2)).Id);
    }
}
