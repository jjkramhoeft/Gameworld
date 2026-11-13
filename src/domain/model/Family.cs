namespace Gameworld.Domain.Model;

/// <summary>A family of people, belonging to a faction.</summary>
public class Family
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; set; }
    public List<Person> Members { get; } = [];
}
