namespace Gameworld.Domain.Model;

/// <summary>A large area of the world, divided into zones.</summary>
public class Region
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; set; }
    public List<Zone> Zones { get; } = [];
}
