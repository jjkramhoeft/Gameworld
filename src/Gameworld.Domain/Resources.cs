namespace Gameworld.Domain;

/// <summary>Resources are split in groups to make trade and needs calculation more manageable.</summary>
public enum ResourceGroup
{
    Food,
    Materials,
    Fuel,
    Luxuries,
    Metals,
    Animals,
    Building,
    Tools,
    Gear,
    Weapons,
    Clothing,
}

/// <summary>
/// The 27 raw resources the generation gives every patch an amount for. The order is the
/// resource index of map-generation.md Appendix D: a new resource goes last, so the picks of
/// the others keep their hashes.
/// </summary>
public enum Resource
{
    Grain,
    Produce,
    Orchard,
    Livestock,
    Fish,
    WildFoodAndGame,
    Timber,
    Stone,
    Clay,
    Crystal,
    Fibre,
    Fuel,
    Salt,
    IronOre,
    CopperOre,
    Tin,
    SilverAndLead,
    Gold,
    GemsAndPearls,
    Spices,
    StimulantsAndSugar,
    FursAndIvory,
    Dyes,
    HorsesAndPackAnimals,
    Reagents,
    Herbs,
    ArcaneMaterials,
}

/// <summary>Potentials are what the land can yield. Deposits are rare finds that often need to be mined.</summary>
public enum ResourceOrigin
{
    Potential,
    Deposit,
}

public static class ResourceCatalog
{
    public const int Count = 27;

    /// <summary>A resource is listed for a patch when its amount is at least this.</summary>
    public const double ListedAmount = 0.25;

    public static IReadOnlyList<Resource> All { get; } = Enum.GetValues<Resource>();

    /// <summary>The groups that only hold produced resources: nothing in them is generated with the map.</summary>
    public static bool IsOnlyProduced(ResourceGroup group) =>
        group is ResourceGroup.Building or ResourceGroup.Tools or ResourceGroup.Gear
            or ResourceGroup.Weapons or ResourceGroup.Clothing;

    public static ResourceGroup GroupOf(Resource resource) => resource switch
    {
        Resource.Grain or Resource.Produce or Resource.Orchard or Resource.Livestock
            or Resource.Fish or Resource.WildFoodAndGame => ResourceGroup.Food,
        // Salt is a generated resource of its own; the readme table lists it under Materials (Preserver).
        Resource.Timber or Resource.Stone or Resource.Clay or Resource.Crystal
            or Resource.Fibre or Resource.Salt or Resource.Reagents or Resource.Herbs => ResourceGroup.Materials,
        Resource.Fuel => ResourceGroup.Fuel,
        Resource.IronOre or Resource.CopperOre or Resource.Tin or Resource.SilverAndLead
            or Resource.Gold => ResourceGroup.Metals,
        Resource.GemsAndPearls or Resource.Spices or Resource.StimulantsAndSugar
            or Resource.FursAndIvory or Resource.Dyes or Resource.ArcaneMaterials => ResourceGroup.Luxuries,
        Resource.HorsesAndPackAnimals => ResourceGroup.Animals,
        _ => throw new ArgumentOutOfRangeException(nameof(resource)),
    };
}

/// <summary>The amount of one resource in a patch, in [0, 1], with its variant ("grain: rye").</summary>
public readonly record struct ResourceAmount(double Amount, string? Variant, ResourceOrigin Origin)
{
    public bool IsListed => Amount >= ResourceCatalog.ListedAmount;
}

/// <summary>
/// The resources of one patch: an amount and a variant for each of the 27 raw resources, and
/// the two summary numbers that feed the Prospect scores.
/// </summary>
public sealed record PatchResources(IReadOnlyList<ResourceAmount> Amounts, double Food, double Wealth)
{
    public static PatchResources None { get; } =
        new(new ResourceAmount[ResourceCatalog.Count], 0, 0);

    public ResourceAmount this[Resource resource] => Amounts[(int)resource];

    public double AmountOf(Resource resource) => Amounts[(int)resource].Amount;

    /// <summary>The resources with an amount of at least 0.25.</summary>
    public IEnumerable<(Resource Resource, ResourceAmount Amount)> Listed =>
        ResourceCatalog.All.Select(r => (r, Amounts[(int)r])).Where(x => x.Item2.IsListed);
}
