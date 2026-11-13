namespace Gameworld.Domain.Model;

/// <summary>A group of families with shared allegiance.</summary>
public class Faction
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; set; }
    public List<Family> Families { get; } = [];
}
