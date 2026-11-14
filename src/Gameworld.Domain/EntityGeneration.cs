namespace Gameworld.Domain;

/// <summary>
/// The generation state of a city, faction, family or person. It is independent of the region
/// stage: Stub → Pending → Realised.
/// </summary>
public enum GenerationState
{
    /// <summary>Holds every gameplay-relevant fact. Derived data.</summary>
    Stub,
    /// <summary>The authoring by LLM or diffusion model is in flight. On failure the entity returns to Stub.</summary>
    Pending,
    /// <summary>Adds the authored presentation only. Authored data must not contradict the Stub.</summary>
    Realised,
}

/// <summary>One authoring track of an entity and its allowed transitions.</summary>
public readonly record struct AuthoringTrack(GenerationState State)
{
    public static AuthoringTrack Stub => new(GenerationState.Stub);

    /// <summary>Stub → Pending: the authoring starts.</summary>
    public AuthoringTrack Begin() => State == GenerationState.Stub
        ? new AuthoringTrack(GenerationState.Pending)
        : throw new InvalidOperationException($"Authoring can only begin from Stub, not from {State}.");

    /// <summary>Pending → Realised: the authored data has arrived and is stored.</summary>
    public AuthoringTrack Complete() => State == GenerationState.Pending
        ? new AuthoringTrack(GenerationState.Realised)
        : throw new InvalidOperationException($"Authoring can only complete from Pending, not from {State}.");

    /// <summary>Pending → Stub: the authoring failed.</summary>
    public AuthoringTrack Fail() => State == GenerationState.Pending
        ? new AuthoringTrack(GenerationState.Stub)
        : throw new InvalidOperationException($"Authoring can only fail from Pending, not from {State}.");
}

/// <summary>
/// Text and portrait are tracked separately, so an entity can be shown with a name and
/// biography before the picture arrives.
/// </summary>
public readonly record struct EntityGeneration(AuthoringTrack Text, AuthoringTrack Portrait)
{
    public static EntityGeneration Stub => new(AuthoringTrack.Stub, AuthoringTrack.Stub);
}

/// <summary>The class a piece of generated data belongs to.</summary>
public enum DataClass
{
    /// <summary>A pure function of the foundation. The database is only a cache of it: it can be evicted and recomputed with an identical result.</summary>
    Derived,
    /// <summary>Comes from an LLM or a diffusion model. Stored as the source of truth, never regenerated or evicted.</summary>
    Authored,
}
