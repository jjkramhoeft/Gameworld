using Gameworld.Domain.Model;

namespace Gameworld.Domain.Tests;

public class FoundationTests
{
    private static readonly WorldSeed Seed = new(42);

    [Fact]
    public void Parameter_sets_with_the_same_values_have_the_same_id()
    {
        Assert.Equal(TestParameters.Create().ComputeId(), TestParameters.Create().ComputeId());
    }

    [Fact]
    public void Changing_one_parameter_gives_a_new_parameter_set_id()
    {
        Assert.NotEqual(TestParameters.Create(0.5).ComputeId(), TestParameters.Create(0.6).ComputeId());
    }

    /// <summary>
    /// Fails when a parameter is added, renamed or reordered. That is expected - update the
    /// value, and know that the ids of parameter sets already in storage no longer match.
    /// </summary>
    [Fact]
    public void Parameter_set_id_is_pinned()
    {
        Assert.Equal("43df6924ac7d67cf9a9a6f8196410c5d", TestParameters.Create().ComputeId().Value);
    }

    [Fact]
    public void Foundation_id_is_pinned()
    {
        var foundation = new Foundation(Seed, new GeneratorVersion(1), TestParameters.Create());

        Assert.Equal("396d30b27b45124b4fda9c24d6815535", foundation.Id.Value);
    }

    [Fact]
    public void Foundations_with_the_same_content_are_equal_whatever_the_label()
    {
        var first = new Foundation(Seed, GeneratorVersion.Current, TestParameters.Create(), "first");
        var second = new Foundation(Seed, GeneratorVersion.Current, TestParameters.Create(), "second");

        Assert.Equal(first.Id, second.Id);
        Assert.Equal(first, second);
    }

    [Fact]
    public void Seed_generator_version_and_parameters_each_change_the_foundation_id()
    {
        var parameters = TestParameters.Create();
        var foundation = new Foundation(Seed, new GeneratorVersion(1), parameters);

        Assert.NotEqual(foundation.Id, new Foundation(new WorldSeed(43), new GeneratorVersion(1), parameters).Id);
        Assert.NotEqual(foundation.Id, new Foundation(Seed, new GeneratorVersion(2), parameters).Id);
        Assert.NotEqual(foundation.Id, new Foundation(Seed, new GeneratorVersion(1), TestParameters.Create(0.6)).Id);
    }

    [Fact]
    public void A_foundation_keeps_the_id_of_its_parameter_set()
    {
        var parameters = TestParameters.Create();

        Assert.Equal(parameters.ComputeId(), new Foundation(Seed, GeneratorVersion.Current, parameters).ParameterSetId);
    }

    [Fact]
    public void Only_land_elevation_types_have_a_per_land_value()
    {
        var weights = new PerLandElevation<double>(Flat: 1, Lowland: 2, Hills: 3, Mountains: 4);

        Assert.Equal(1, weights[ElevationType.Flat]);
        Assert.Equal(4, weights[ElevationType.Mountains]);
        Assert.Throws<ArgumentOutOfRangeException>(() => weights[ElevationType.Ocean]);
    }
}
