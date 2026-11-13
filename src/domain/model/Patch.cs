namespace Gameworld.Domain.Model;

/// <summary>The smallest unit of land. Has one biome and may hold resource prospects.</summary>
public class Patch
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required Biome Biome { get; set; }
    public List<Prospect> Prospects { get; } = [];
}
