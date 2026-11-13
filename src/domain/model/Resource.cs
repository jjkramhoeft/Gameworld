namespace Gameworld.Domain.Model;

/// <summary>A raw material found in the world, such as timber or iron ore.</summary>
public class Resource
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; set; }
}
