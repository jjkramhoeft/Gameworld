namespace Gameworld.Domain.Model;



/// <summary>The five kinds of elevation zone. The numeric value is the index into <see cref="ElevationZoneType.All"/>.</summary>
public enum ZoneKind
{
    Ocean = 0,
    Flat = 1,
    Lowland = 2,
    Hills = 3,
    Mountains = 4,
}

/// <summary>
/// Elevation zone type. Every elevation zone (the medium cells) is ocean or one of four land
/// types. Ocean comes from the ocean genesis; a land zone's type is picked from a hash of its
/// key, by weight. The type sets the terrain's hillslope, river gradient and detail noise.
/// </summary>
public sealed record Zone(
    ZoneKind Kind, 
    uint Index,
    string Name)
{
    
}

