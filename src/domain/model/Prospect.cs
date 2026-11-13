namespace Gameworld.Domain.Model;

/// <summary>A deposit of a resource in a patch, and how much of it there is.</summary>
public class Prospect
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required Resource Resource { get; set; }
    public int Quantity { get; set; }
}
