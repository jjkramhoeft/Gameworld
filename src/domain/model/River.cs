namespace Gameworld.Domain.Model;

/// <summary>A river, described by the patches it flows through from source to mouth.</summary>
public class River
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; set; }
    public List<Patch> Course { get; } = [];
}
