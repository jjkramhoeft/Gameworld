using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Gameworld.Domain;

// The portrait components (society-generation.md section 9): the lists the looks of a person
// are picked from. Each component is one fixed piece of the description of a person, repeated
// word for word in every picture of that person. The person generator draws them.

/// <summary>
/// A skin colour: one of the tones a person can have. The skin colours are a list in the
/// Parameter Set, and a race names the ones its persons have (society-generation.md section 9.2).
/// </summary>
[Description("A skin colour: a tone of skin a person can have. A race lists the skin colours of its persons by id (society §9.2).")]
public sealed partial record SkinColourParameters(
    [property: Description("The id the generated data knows the skin colour by, in camelCase like goldenTan. Unique in the list; keep it when the name changes.")] string Id,
    [property: Description("The name shown for the skin colour.")] string Name,
    [property: Description("What a picture shows: the tone, the undertone and any marking of the skin itself, written to be set into a sentence about the person. A picture of the person is drawn from this text, word for word every time. No generation rule reads it.")] string Description);

/// <summary>
/// An eye colour, with the pupil where the pupil is the mark of a race. The eye colours are a
/// list in the Parameter Set, and a race names the ones its persons have
/// (society-generation.md section 9.2).
/// </summary>
[Description("An eye colour: the colour of the eyes of a person, and the pupil where it is the mark of a race. A race lists the eye colours of its persons by id (society §9.2).")]
public sealed partial record EyeColourParameters(
    [property: Description("The id the generated data knows the eye colour by, in camelCase like earthBrown. Unique in the list; keep it when the name changes.")] string Id,
    [property: Description("The name shown for the eye colour.")] string Name,
    [property: Description("What a picture shows: the colour of the eyes and what else sets them apart, written to be set into a sentence about the person. A picture of the person is drawn from this text, word for word every time. No generation rule reads it.")] string Description);

/// <summary>
/// A hair colour. The hair colours are a list in the Parameter Set, and a race names the ones
/// its persons have (society-generation.md section 9.2). A colour can name the colour it turns
/// into as the person greys.
/// </summary>
[Description("A hair colour: the colour of the hair of a person. A race lists the hair colours of its persons by id (society §9.2).")]
public sealed partial record HairColourParameters(
    [property: Description("The id the generated data knows the hair colour by, in camelCase like copperRed. Unique in the list; keep it when the name changes.")] string Id,
    [property: Description("The name shown for the hair colour.")] string Name,
    [property: Description("What a picture shows: the colour of the hair, written to be set into a sentence about the person. A picture of the person is drawn from this text, word for word every time. No generation rule reads it.")] string Description)
{
    [Description("The id of the hair colour this one turns into as the person greys: black turns to greying dark, greying dark to iron grey, iron grey to white. It must be another hair colour of the list, and the chain must end. Left out, the colour does not change with age (society §9.2).")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Greying { get; init; }
}

/// <summary>
/// Who wears a hairstyle, an outfit or an accessory (society-generation.md section 9.3). Every
/// value is a limit: a person wears the thing only when all of them hold for the person. A
/// value that is left out limits nothing.
/// </summary>
[Description("Who wears a hairstyle, an outfit or an accessory. Every value is a limit, and a person can wear the thing only when all of them hold. A value that is left out limits nothing (society §9.3).")]
public sealed record WearerParameters
{
    [Description("The sex it is worn by: Male or Female. Left out, both sexes wear it.")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Sex? Sex { get; init; }

    [Description("The ids of the races that wear it. Every id must be a race of the Races list. Left out or empty, every race wears it.")]
    public IReadOnlyList<string> Races { get; init; } = [];

    [Description("The lowest wealth of the household of a person who wears it: Destitute, Poor, Common, Comfortable or Rich. Left out, Destitute.")]
    public WealthLevel LowestWealth { get; init; } = WealthLevel.Destitute;

    [Description("The highest wealth of the household of a person who wears it. Not below LowestWealth. Left out, Rich.")]
    public WealthLevel HighestWealth { get; init; } = WealthLevel.Rich;

    [Description("The ids of the vocations it is the dress or the mark of: only a person with one of them wears it. Every id must be a vocation of the Vocations list. Left out or empty, the vocation does not matter.")]
    public IReadOnlyList<string> Vocations { get; init; } = [];

    [Description("The lowest age it is worn at, in years. Left out, no lower limit.")]
    [Range(0, 1000)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? LowestAge { get; init; }

    [Description("The highest age it is worn at, in years. Not below LowestAge. Left out, no upper limit.")]
    [Range(0, 1000)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? HighestAge { get; init; }

    [Description("Whether it is worn by the married only (true) or by the unmarried only (false). Married is a person whose husband or wife lives in the household. Left out, both wear it.")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool? Married { get; init; }
}

/// <summary>
/// A hairstyle: how a person wears the hair, with the beard or the head covering where that is
/// the point of it. The hairstyles are a list in the Parameter Set, so the code knows no
/// hairstyle by name (society-generation.md section 9.3).
/// </summary>
[Description("A hairstyle: how a person wears the hair, with the beard or the head covering where that is the point of it, and who wears it (society §9.3).")]
public sealed partial record HairstyleParameters(
    [property: Description("The id the generated data knows the hairstyle by, in camelCase like crownBraid. Unique in the list; keep it when the name changes.")] string Id,
    [property: Description("The name shown for the hairstyle.")] string Name,
    [property: Description("Whether the hairstyle exists in the world. A hairstyle that is off is worn by nobody.")] bool Enabled)
{
    [Description("Who wears the hairstyle: the sex, the races, the wealth, the vocations, the ages and whether married. Left out, everybody can wear it (society §9.3).")]
    public WearerParameters WornBy { get; init; } = new();

    [Description("What the hairstyle looks like: the cut, the length and how it is dressed (society §9.3). It names no hair colour: that is a component of its own. A picture of the person is drawn from this text, word for word every time. No generation rule reads it. The text \"**missing description**\" marks one that is not written yet.")]
    public string Description { get; init; } = DefaultTexts.Missing;

    [Description("The lore of the hairstyle: who wears it, what it says of the wearer and what is told about it (society §9.3). No generation rule reads it. The text \"**missing description**\" marks one that is not written yet.")]
    public string Lore { get; init; } = DefaultTexts.Missing;

    private static HairstyleParameters Make(string id, string name, WearerParameters wornBy)
    {
        var (description, lore) = DefaultTexts.Of(DefaultTexts.Hairstyles, id);
        return new(id, name, true) { WornBy = wornBy, Description = description, Lore = lore };
    }
}

/// <summary>
/// An outfit: what a person wears, from the head covering to the shoes. The outfits are a list
/// in the Parameter Set (society-generation.md section 9.4). The outfit of a person has a main
/// colour and an accent colour, two colours of the Colours list: an outfit either fixes them or
/// leaves them to the person.
/// </summary>
[Description("An outfit: what a person wears, who wears it and, where the outfit decides them, its two colours (society §9.4).")]
public sealed partial record OutfitParameters(
    [property: Description("The id the generated data knows the outfit by, in camelCase like kirtleAndApron. Unique in the list; keep it when the name changes.")] string Id,
    [property: Description("The name shown for the outfit.")] string Name,
    [property: Description("Whether the outfit exists in the world. An outfit that is off is worn by nobody.")] bool Enabled)
{
    /// <summary>What stands for the main colour of the outfit of the person in a description.</summary>
    public const string MainColourMark = "{main}";

    /// <summary>What stands for the accent colour of the outfit of the person in a description.</summary>
    public const string AccentColourMark = "{accent}";

    [Description("Who wears the outfit: the sex, the races, the wealth, the vocations, the ages and whether married. Left out, everybody can wear it (society §9.3).")]
    public WearerParameters WornBy { get; init; } = new();

    [Description("The id of the main colour of the outfit, where the outfit decides it: the black of a habit, the undyed grey of a peasant gown. It must be a colour of the Colours list. Left out, the main colour is drawn for the person (society §9.4 and §9.7).")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? MainColour { get; init; }

    [Description("The id of the accent colour of the outfit, where the outfit decides it: the colour of its trim, its sash or its lining. It must be a colour of the Colours list. Left out, the accent colour is drawn for the person (society §9.4 and §9.7).")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? AccentColour { get; init; }

    [Description("What the outfit looks like: what a head-and-shoulders picture shows first, and the rest after \"Below:\" (society §9.4). The marks {main} and {accent} stand for the main and the accent colour of the outfit of the person, and a picture is drawn from the text with the names of those two colours in their place. No generation rule reads it. The text \"**missing description**\" marks one that is not written yet.")]
    public string Description { get; init; } = DefaultTexts.Missing;

    [Description("The lore of the outfit: who wears it, what it says of the wearer and what is told about it (society §9.4). No generation rule reads it. The text \"**missing description**\" marks one that is not written yet.")]
    public string Lore { get; init; } = DefaultTexts.Missing;

    /// <summary>
    /// The description with the names of the two colours of the outfit of a person in place of
    /// the marks. A colour that is not given is written as "undyed".
    /// </summary>
    public string DescriptionIn(string? mainColourName, string? accentColourName) => Description
        .Replace(MainColourMark, Word(mainColourName), StringComparison.Ordinal)
        .Replace(AccentColourMark, Word(accentColourName), StringComparison.Ordinal);

    private static string Word(string? colourName) =>
        string.IsNullOrWhiteSpace(colourName) ? "undyed" : colourName.Trim().ToLowerInvariant();

    private static OutfitParameters Make(string id, string name, WearerParameters wornBy, string? main = null, string? accent = null)
    {
        var (description, lore) = DefaultTexts.Of(DefaultTexts.Outfits, id);
        return new(id, name, true) { WornBy = wornBy, MainColour = main, AccentColour = accent, Description = description, Lore = lore };
    }
}

/// <summary>What kind of accessory a piece is: where on the person it sits (society-generation.md section 9.5).</summary>
public enum AccessoryKind
{
    /// <summary>Worn around the neck or at the throat: a string of beads, a pendant, a neck ring, a collar.</summary>
    Necklace,
    /// <summary>Worn in or on the ears.</summary>
    Earrings,
    /// <summary>Worn on the wrist or the upper arm: bracelets, bangles and arm bands.</summary>
    Bracelet,
    /// <summary>Worn on a finger or a thumb.</summary>
    Ring,
    /// <summary>Pinned to the dress: brooches, badges and plaques.</summary>
    Brooch,
    /// <summary>Worn on the head: crowns, circlets, diadems and wreaths.</summary>
    Crown,
    /// <summary>Worn anywhere else: on the nose, the ankle, a tusk or the brow, or hung from the belt.</summary>
    BodyJewellery,
    /// <summary>Worn in the hair.</summary>
    HairJewellery,
}

/// <summary>
/// An accessory: a piece of jewellery worn on the body or in the hair. The accessories are a
/// list in the Parameter Set (society-generation.md section 9.5). A person can have none, one or
/// several, and at most one of a kind.
/// </summary>
[Description("An accessory: a piece of jewellery worn on the body or in the hair, its kind and who wears it. A person can have none, one or several (society §9.5).")]
public sealed partial record AccessoryParameters(
    [property: Description("The id the generated data knows the accessory by, in camelCase like goldSignetRing. Unique in the list; keep it when the name changes.")] string Id,
    [property: Description("The name shown for the accessory.")] string Name,
    [property: Description("Whether the accessory exists in the world. An accessory that is off is worn by nobody.")] bool Enabled,
    [property: Description("The kind of the accessory: Necklace, Earrings, Bracelet, Ring, Brooch, Crown, BodyJewellery or HairJewellery. It says where the piece sits, and a person wears at most one of a kind (society §9.5).")] AccessoryKind Kind)
{
    [Description("Who wears the accessory: the sex, the races, the wealth, the vocations, the ages and whether married. Left out, everybody can wear it (society §9.3).")]
    public WearerParameters WornBy { get; init; } = new();

    [Description("What the accessory looks like: its form, what it is made of and where it sits (society §9.5). A picture of the person is drawn from this text, word for word every time. No generation rule reads it. The text \"**missing description**\" marks one that is not written yet.")]
    public string Description { get; init; } = DefaultTexts.Missing;

    [Description("The lore of the accessory: who gives it, when it comes off and what it means (society §9.5). No generation rule reads it. The text \"**missing description**\" marks one that is not written yet.")]
    public string Lore { get; init; } = DefaultTexts.Missing;

    private static AccessoryParameters Make(string id, string name, AccessoryKind kind, WearerParameters wornBy)
    {
        var (description, lore) = DefaultTexts.Of(DefaultTexts.Accessories, id);
        return new(id, name, true, kind) { WornBy = wornBy, Description = description, Lore = lore };
    }
}

/// <summary>
/// The looks of a person: its portrait components (society-generation.md section 9). Each value
/// is the id of an entry of a list of the Parameter Set. The looks are derived data and part of
/// the Stub of the person: a portrait is authored from them, and every portrait of the person
/// repeats them.
/// </summary>
/// <param name="SkinColour">The id of the skin colour. Null when the race of the person lists none.</param>
/// <param name="EyeColour">The id of the eye colour. Null when the race of the person lists none.</param>
/// <param name="HairColour">The id of the hair colour a picture shows: the natural hair colour, or the colour it has turned into at the age of the person. Null when the race of the person lists none: a race without hair.</param>
/// <param name="NaturalHairColour">The id of the hair colour before it greys: a colour of the race, and the one a child shares with its father or its mother. Null when the race of the person lists none.</param>
/// <param name="Hairstyle">The id of the hairstyle. Null when no hairstyle of the list fits the person.</param>
/// <param name="Outfit">The id of the outfit. Null when no outfit of the list fits the person.</param>
/// <param name="OutfitMainColour">The id of the main colour of the outfit: a colour of the Colours list. Null without an outfit.</param>
/// <param name="OutfitAccentColour">The id of the accent colour of the outfit: a colour of the Colours list. Null without an outfit.</param>
/// <param name="Accessories">The ids of the accessories the person wears, at most one of a kind. Empty without any.</param>
public sealed record Looks(
    string? SkinColour,
    string? EyeColour,
    string? HairColour,
    string? NaturalHairColour,
    string? Hairstyle,
    string? Outfit,
    string? OutfitMainColour,
    string? OutfitAccentColour,
    IReadOnlyList<string> Accessories);
