namespace Gameworld.Domain.Model;

/// <summary>A kind of terrain and climate, such as forest, desert or tundra.</summary>
public class Biome
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; set; }
}
