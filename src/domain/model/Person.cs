namespace Gameworld.Domain.Model;

/// <summary>An individual in the world.</summary>
public class Person
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; set; }
}
