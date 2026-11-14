using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using Gameworld.Domain.Generation;
using Gameworld.Domain.Model;

namespace Gameworld.Domain.Tests;

public class SitingTests
{
    private static readonly WorldSeed Seed = new(42);
    private static readonly RegionId Region = new(3, -2);
    private static readonly ParameterSet Parameters = TestParameters.Create();
    private static readonly Foundation Foundation = new(Seed, GeneratorVersion.Current, Parameters);
    private static readonly RegionGenerator Generator = new();

    /// <summary>The region and everything within two rings of it.</summary>
    private static readonly Lazy<Dictionary<RegionId, SitedRegion>> Block = new(() =>
        Region.Within(2).ToDictionary(id => id, id => Generator.Site(Foundation, id)));

    private static SitedRegion Sited => Block.Value[Region];

    // ---- Reproducible -------------------------------------------------------------------

    [Fact]
    public void Siting_a_region_twice_gives_the_same_result()
    {
        Assert.Equal(Describe(Sited), Describe(new RegionGenerator().Site(Foundation, Region)));
    }

    [Fact]
    public void A_region_is_sited_the_same_whatever_was_sited_before_it()
    {
        var generator = new RegionGenerator();
        foreach (var other in new RegionId(-40, 17).Within(1))
            generator.Site(Foundation, other);

        Assert.Equal(Describe(Sited), Describe(generator.Site(Foundation, Region)));
    }

    /// <summary>
    /// Every generated map depends on this. If this test fails the Site step has changed,
    /// and that is a generator version change.
    /// </summary>
    [Fact]
    public void The_sited_region_is_pinned()
    {
        Assert.Equal("485.6513447754845, -212.41040660843214", string.Create(CultureInfo.InvariantCulture, $"{Sited.Site.X:R}, {Sited.Site.Y:R}"));
        Assert.Equal(12, Sited.ZoneCells.Length);
        Assert.Equal("b2d914959b2e42bb447051ade037d95292244fbeb48543143f0c59d123ae330b", Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(Describe(Sited)))));
    }

    [Fact]
    public void Another_seed_gives_another_region()
    {
        var other = new Foundation(new WorldSeed(43), GeneratorVersion.Current, Parameters);

        Assert.NotEqual(Describe(Sited), Describe(Generator.Site(other, Region)));
    }

    /// <summary>The parameter set is not part of the seed hash, so one parameter can be compared in isolation.</summary>
    [Fact]
    public void Changing_one_parameter_keeps_every_other_roll()
    {
        var changed = new Foundation(Seed, GeneratorVersion.Current, TestParameters.Create(oceanLinkChance: 1));
        var sited = Generator.Site(changed, Region);

        Assert.Equal(Describe(Sited with { Quota = sited.Quota }), Describe(sited));
        Assert.Equal(
            Sited.Quota.OceanSeeds.Select(quota => (quota.Seed, quota.Expansions)),
            sited.Quota.OceanSeeds.Select(quota => (quota.Seed, quota.Expansions)));
        Assert.All(sited.Quota.OceanSeeds, quota => Assert.True(quota.Links));
    }

    // ---- Shape of the result ------------------------------------------------------------

    [Fact]
    public void A_region_site_stays_within_the_jitter_of_its_grid_centre()
    {
        var size = Parameters.Siting.RegionGridSizeKm;
        var jitter = Parameters.Siting.RegionJitter * size;

        Assert.All(Block.Value.Values, region =>
        {
            Assert.InRange(region.Site.X, (region.Id.X + 0.5) * size - jitter, (region.Id.X + 0.5) * size + jitter);
            Assert.InRange(region.Site.Y, (region.Id.Y + 0.5) * size - jitter, (region.Id.Y + 0.5) * size + jitter);
        });
    }

    [Fact]
    public void Without_jitter_a_region_site_is_its_grid_centre()
    {
        var parameters = Parameters with { Siting = Parameters.Siting with { RegionJitter = 0 } };
        var sited = Generator.Site(new Foundation(Seed, GeneratorVersion.Current, parameters), Region);

        Assert.Equal(new WorldPoint(3.5 * 150, -1.5 * 150), sited.Site);
    }

    [Fact]
    public void Cell_counts_are_within_the_ranges_of_the_parameter_set()
    {
        var zones = Parameters.Siting.ZoneCellsPerRegion;
        var patches = Parameters.Siting.PatchCellsPerZoneCell;

        Assert.All(Block.Value.Values, region =>
        {
            Assert.InRange(region.ZoneCells.Length, zones.Min, zones.Max);
            Assert.All(region.ZoneCells, zone => Assert.InRange(zone.PatchCells.Length, patches.Min, patches.Max));
        });
        Assert.True(Block.Value.Values.Select(region => region.ZoneCells.Length).Distinct().Count() > 1);
    }

    [Fact]
    public void Cells_are_numbered_from_zero_within_their_parent()
    {
        Assert.Equal(
            Enumerable.Range(0, Sited.ZoneCells.Length).Select(index => new ZoneCellId(Region, index)),
            Sited.ZoneCells.Select(zone => zone.Id));
        Assert.All(Sited.ZoneCells, zone => Assert.Equal(
            Enumerable.Range(0, zone.PatchCells.Length).Select(index => new PatchCellId(zone.Id, index)),
            zone.PatchCells.Select(patch => patch.Id)));
    }

    // ---- Nested Voronoi cells -----------------------------------------------------------

    [Fact]
    public void A_zone_cell_site_is_nearer_to_its_own_region_site_than_to_any_other()
    {
        Assert.All(Sited.ZoneCells, zone =>
        {
            var nearest = Block.Value.Values.MinBy(region => region.Site.DistanceTo(zone.Site))!;
            Assert.Equal(Region, nearest.Id);
        });
    }

    [Fact]
    public void A_patch_cell_site_is_nearer_to_its_own_zone_cell_site_than_to_any_other()
    {
        var zones = Block.Value.Values.SelectMany(region => region.ZoneCells).ToList();

        Assert.All(Sited.ZoneCells.SelectMany(zone => zone.PatchCells), patch =>
        {
            var nearest = zones.MinBy(zone => zone.Site.DistanceTo(patch.Site))!;
            Assert.Equal(patch.Id.ZoneCell, nearest.Id);
        });
    }

    [Fact]
    public void Zone_cell_sites_keep_their_distance_also_across_region_borders()
    {
        var sites = Block.Value.Values.SelectMany(region => region.ZoneCells).Select(zone => zone.Site).ToList();
        var closest = sites.Min(site => sites.Where(other => other != site).Min(other => other.DistanceTo(site)));

        // Evenly spread zone cell sites would be about 47 km apart.
        Assert.True(closest > 15, $"Two zone cell sites are only {closest:F1} km apart.");
    }

    [Theory]
    [InlineData(0.5, 8, 12, 16)]
    [InlineData(0.5, 1, 2, 1)]
    [InlineData(0.0, 1, 1, 1)]
    [InlineData(0.3, 20, 30, 4)]
    public void Cells_stay_nested_at_the_edges_of_the_parameter_ranges(double jitter, int fewestZoneCells, int mostZoneCells, int candidates)
    {
        var parameters = Parameters with
        {
            Siting = Parameters.Siting with
            {
                RegionJitter = jitter,
                ZoneCellsPerRegion = new(fewestZoneCells, mostZoneCells),
                PatchCellsPerZoneCell = new(1, 6),
                ZoneCellCandidates = candidates,
                PatchCellCandidates = candidates,
            },
        };
        var foundation = new Foundation(Seed, GeneratorVersion.Current, parameters);
        var block = Region.Within(3).Select(id => Generator.Site(foundation, id)).ToList();
        var zones = block.SelectMany(region => region.ZoneCells).ToList();

        foreach (var centre in block.Where(region => region.Id.RingsTo(Region) <= 1))
        {
            Assert.All(centre.ZoneCells, zone =>
                Assert.Equal(centre.Id, block.MinBy(region => region.Site.DistanceTo(zone.Site))!.Id));
            Assert.All(centre.ZoneCells.SelectMany(zone => zone.PatchCells), patch =>
                Assert.Equal(patch.Id.ZoneCell, zones.MinBy(zone => zone.Site.DistanceTo(patch.Site))!.Id));
        }
    }

    // ---- What the zones start out as ----------------------------------------------------

    [Fact]
    public void Ocean_seeds_are_ocean_and_all_other_zone_cells_are_land()
    {
        var seeds = Parameters.Siting.OceanSeedsPerRegion;

        Assert.All(Block.Value.Values, region =>
        {
            Assert.InRange(region.ZoneCells.Count(zone => zone.IsOceanSeed), seeds.Min, seeds.Max);
            Assert.All(region.ZoneCells, zone => Assert.Equal(zone.IsOceanSeed, zone.InitialElevation == ElevationType.Ocean));
        });
        Assert.Contains(Block.Value.Values, region => region.ZoneCells.Any(zone => zone.IsOceanSeed));
    }

    [Fact]
    public void Land_elevation_types_follow_their_weights()
    {
        var land = Block.Value.Values.SelectMany(region => region.ZoneCells).Where(zone => !zone.IsOceanSeed).ToList();
        double Share(ElevationType type) => land.Count(zone => zone.InitialElevation == type) / (double)land.Count;

        // The weights are 3 : 3 : 2 : 1.
        Assert.InRange(Share(ElevationType.Flat), 0.23, 0.43);
        Assert.InRange(Share(ElevationType.Lowland), 0.23, 0.43);
        Assert.InRange(Share(ElevationType.Hills), 0.13, 0.32);
        Assert.InRange(Share(ElevationType.Mountains), 0.04, 0.20);
    }

    [Fact]
    public void Bedrock_is_only_found_under_the_elevation_types_it_has_a_weight_for()
    {
        var zones = Block.Value.Values.SelectMany(region => region.ZoneCells).ToList();
        var catalog = Parameters.Bedrocks.ToDictionary(bedrock => bedrock.Id);

        Assert.All(zones.Where(zone => !zone.IsOceanSeed), zone => Assert.True(catalog[zone.Bedrock].Weights[zone.InitialElevation] > 0));
        Assert.All(zones.Where(zone => zone.IsOceanSeed), zone => Assert.Contains(zone.Bedrock, catalog.Keys));
        Assert.Contains(zones, zone => zone.Bedrock == new BedrockId("volcanic"));
    }

    // ---- Quota --------------------------------------------------------------------------

    [Fact]
    public void Every_ocean_seed_has_a_quota_within_the_ranges_of_the_parameter_set()
    {
        var expansions = Parameters.Outline.ExpansionsPerOceanSeed;
        var mergers = Parameters.Outline.ZoneMergersPerRegion;

        Assert.All(Block.Value.Values, region =>
        {
            Assert.Equal(
                region.ZoneCells.Where(zone => zone.IsOceanSeed).Select(zone => zone.Id),
                region.Quota.OceanSeeds.Select(quota => quota.Seed));
            Assert.All(region.Quota.OceanSeeds, quota => Assert.InRange(quota.Expansions, expansions.Min, expansions.Max));
            Assert.InRange(region.Quota.ZoneMergers, mergers.Min, mergers.Max);
        });
    }

    [Fact]
    public void Ocean_seeds_link_with_about_the_configured_chance()
    {
        var quotas = Block.Value.Values.SelectMany(region => region.Quota.OceanSeeds).ToList();

        Assert.InRange(quotas.Count(quota => quota.Links) / (double)quotas.Count, 0.25, 0.75);
    }

    // ---- Refusals -----------------------------------------------------------------------

    [Fact]
    public void A_foundation_of_another_generator_version_is_refused()
    {
        var other = new Foundation(Seed, new GeneratorVersion(GeneratorVersion.Current.Value + 1), Parameters);

        Assert.Throws<NotSupportedException>(() => Generator.Site(other, Region));
    }

    [Fact]
    public void A_jitter_that_lets_a_region_site_leave_its_grid_cell_is_refused()
    {
        var parameters = Parameters with { Siting = Parameters.Siting with { RegionJitter = 0.6 } };

        var error = Assert.Throws<ArgumentException>(() => Generator.Site(new Foundation(Seed, GeneratorVersion.Current, parameters), Region));
        Assert.Contains("RegionJitter", error.Message);
    }

    [Fact]
    public void An_elevation_type_without_any_bedrock_is_refused()
    {
        var parameters = Parameters with { Bedrocks = [Parameters.Bedrocks[0]] };

        var error = Assert.Throws<ArgumentException>(() => Generator.Site(new Foundation(Seed, GeneratorVersion.Current, parameters), Region));
        Assert.Contains("Mountains", error.Message);
    }

    /// <summary>Everything in a sited region as text, with every coordinate exact.</summary>
    private static string Describe(SitedRegion region)
    {
        var text = new StringBuilder();
        text.AppendLine(CultureInfo.InvariantCulture, $"{region.Id} {region.Site.X:R} {region.Site.Y:R} mergers {region.Quota.ZoneMergers}");
        foreach (var quota in region.Quota.OceanSeeds)
            text.AppendLine(CultureInfo.InvariantCulture, $"seed {quota.Seed} links {quota.Links} expansions {quota.Expansions}");
        foreach (var zone in region.ZoneCells)
        {
            text.AppendLine(CultureInfo.InvariantCulture, $"{zone.Id} {zone.Site.X:R} {zone.Site.Y:R} {zone.InitialElevation} {zone.Bedrock} {zone.IsOceanSeed}");
            foreach (var patch in zone.PatchCells)
                text.AppendLine(CultureInfo.InvariantCulture, $"{patch.Id} {patch.Site.X:R} {patch.Site.Y:R}");
        }
        return text.ToString();
    }
}
