using Gameworld.Generation.Basics;
using Gameworld.Generation.Names;

namespace Gameworld.Tests;

/// <summary>The person-name generator of docs/NameGenerator-Reuse.md.</summary>
public class NameGeneratorTests
{
    private static readonly NameStyle[] AllStyles = [NameStyle.Common, NameStyle.Exotic, NameStyle.Dark];
    private static readonly NameKind[] AllKinds = [NameKind.Male, NameKind.Female, NameKind.Surname];

    /// <summary>
    /// The reference names of the reuse doc: seeds 0 to 9 of the original generator. They catch
    /// a changed data file, a bad port and a .NET version that changes the seeded Random.
    /// </summary>
    public static TheoryData<NameStyle, NameKind, string> ReferenceNames => new()
    {
        { NameStyle.Common, NameKind.Male, "Owen, Darry, Reece, Erris, Ryan, Felix, Shawn, Gregan, Sal, Ioan" },
        { NameStyle.Common, NameKind.Female, "Niki, Danie, Pamelaine, Emma, Ruth, Farzenata, Shonda, Hola, Salisha, Jeanel" },
        { NameStyle.Common, NameKind.Surname, "Osullisburnetts, Davies, Pearching, Ell, Ruthers, Fenwickson, Snedy, Griffinnormicholm, Sampton, Hearents" },
        { NameStyle.Exotic, NameKind.Male, "Ruvyn, Ehlmas, Shaunavan, Fin, Syth, Galaithrail, Thuryn, Hor, Tamnaerith, Jhar" },
        // "Rentári": the escaped characters of the data survive the embedding.
        { NameStyle.Exotic, NameKind.Female, "Polla, Daena, Rentári, Eowyn, Synn, Elanni, Sionia, Gwyn, Takaria, Halaedra" },
        { NameStyle.Exotic, NameKind.Surname, "Stone, Faren, Silverond, Felis, Tyrider, Glavien, Veverondan, Irian, Waesmer, Kelgarwyn" },
        { NameStyle.Dark, NameKind.Male, "Pysha, Eirsolin, Relyr, Ghuhre, Sylkalyn, Helran, Trisneirsin, Koraedis, Uplik, Keethasil" },
        { NameStyle.Dark, NameKind.Female, "Rhyonis, Eceriina, Shara, Eronna, Sylmadra, Fednea, Terysn, Hilanren, Vanhice, Ilim" },
        { NameStyle.Dark, NameKind.Surname, "Qugren, Canmiu, Slim, Drallasaloth, Twinbirthtwingerbrook, Falnon, Venwarmfeatdreathezire, Flamenei, Xaris, Giedenvon" },
    };

    [Theory]
    [MemberData(nameof(ReferenceNames))]
    public void The_original_seeds_give_the_reference_names(NameStyle style, NameKind kind, string expected)
    {
        var names = NameGenerator.Default;
        var actual = string.Join(", ", Enumerable.Range(0, 10).Select(seed => names.Generate(style, kind, seed)));
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void A_name_is_a_pure_function_of_the_generator_seed()
    {
        var names = NameGenerator.Default;
        foreach (var style in AllStyles)
            foreach (var kind in AllKinds)
                for (uint seed = 0; seed < 200; seed++)
                {
                    var hash = Hashing.Hash4(20261114, 0x4a01, (int)seed, 0);
                    Assert.Equal(
                        names.Generate(style, kind, new Sfc32(hash)),
                        names.Generate(style, kind, new Sfc32(hash)));
                }
    }

    [Fact]
    public void Names_are_capitalised_and_at_most_29_characters()
    {
        var names = NameGenerator.Default;
        foreach (var style in AllStyles)
            foreach (var kind in AllKinds)
            {
                var rng = new Sfc32(Hashing.Hash4(20261114, 0x4a02, (int)style, (int)kind));
                var distinct = new HashSet<string>();
                for (var i = 0; i < 5000; i++)
                {
                    var name = names.Generate(style, kind, rng);
                    Assert.InRange(name.Length, 1, 29);
                    Assert.True(char.IsUpper(name[0]), $"'{name}' does not start with a capital letter");
                    Assert.DoesNotContain('£', name);
                    Assert.DoesNotContain('$', name);
                    distinct.Add(name);
                }
                // The models are not degenerate: one stream gives many different names.
                Assert.True(distinct.Count > 1000, $"{style} {kind}: only {distinct.Count} distinct names in 5000");
            }
    }

    [Fact]
    public void A_full_name_is_a_given_name_and_a_surname_from_one_stream()
    {
        var names = NameGenerator.Default;
        var hash = Hashing.Hash4(20261114, 0x4a03, 7, 3);

        var full = names.GenerateFullName(NameStyle.Dark, NameKind.Female, new Sfc32(hash));

        var rng = new Sfc32(hash);
        var given = names.Generate(NameStyle.Dark, NameKind.Female, rng);
        var surname = names.Generate(NameStyle.Dark, NameKind.Surname, rng);
        Assert.Equal(given + " " + surname, full);
    }

    /// <summary>
    /// The reuse doc measured 58 % alliteration when given name and surname share a seed, and
    /// 5 % with independent randomness. One Sfc32 stream must give the independent figure.
    /// </summary>
    [Fact]
    public void Given_name_and_surname_are_not_alliterative_by_construction()
    {
        var names = NameGenerator.Default;
        const int count = 5000;
        var alliterative = 0;
        for (var i = 0; i < count; i++)
        {
            var rng = new Sfc32(Hashing.Hash4(20261114, 0x4a04, i, 0));
            var given = names.Generate(NameStyle.Common, NameKind.Male, rng);
            var surname = names.Generate(NameStyle.Common, NameKind.Surname, rng);
            if (given[0] == surname[0]) alliterative++;
        }
        Assert.InRange(alliterative / (double)count, 0.0, 0.12);
    }

    [Theory]
    [InlineData("Owen", "Owen's")]
    [InlineData("Davies", "Davies'")]
    [InlineData("", "")]
    public void Genitive_adds_the_possessive_ending(string name, string expected) =>
        Assert.Equal(expected, NameGenerator.Genitive(name));
}
