namespace Gameworld.Domain.Model;

/// <summary>Something made from resources.</summary>
public class Product
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; set; }
    public List<Resource> Ingredients { get; } = [];
}
