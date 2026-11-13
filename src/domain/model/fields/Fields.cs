namespace Gameworld.Domain.Model;

/// <summary>The noise maps that drive climate, biomes and resources.</summary>
public enum FieldKind
{
    Precipitation = 0,
    InitialTemperature = 1,
    Prosperity = 2,
    Supernatural = 3,
}

/// <summary>
/// The Fields of one foundation: stateless functions of foundation and coordinate that can
/// be sampled anywhere at any time. Fields are never stored and have no generation stage -
/// a stage only records the values it sampled.
/// </summary>
public interface IFields
{
    double Sample(FieldKind kind, WorldPoint point);
}
