using Gameworld.Domain;

namespace Gameworld.Generation.Names;

/// <summary>
/// The place names of the default races (society-generation.md section 5.3): the first parts
/// and the place words of each race, built from what docs/city-and-town-names.md says its
/// names are made of. A race that has no lexicon here, such as one added through a Parameter
/// Set file, takes the lexicon of its name style.
/// <para>
/// Adding, removing or reordering a word changes the names of every world; a test holds
/// reference names. In the lists a space separates the words, and an underscore stands for a
/// space inside a word: "_Gate" is the place word " Gate" and "Bir_" the place word "Bir ".
/// </para>
/// </summary>
public static class SettlementNameLexicons
{
    /// <summary>The lexicon of a race by its id; null for a race without one.</summary>
    public static SettlementNameLexicon? Of(string race) => ByRace.GetValueOrDefault(race);

    /// <summary>
    /// The lexicon of a race without one of its own: the first part is a surname from the
    /// model of its name style, and the place words are those of the trade tongue.
    /// </summary>
    public static SettlementNameLexicon OfStyle(NameStyle style) => ByStyle[style];

    /// <summary>The ids of the races with a lexicon of their own.</summary>
    public static IReadOnlyCollection<string> Races => ByRace.Keys;

    private static string[] W(string words) =>
        words.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(w => w.Replace('_', ' ')).ToArray();

    private static IReadOnlyDictionary<SiteClass, string[]> Words(params (SiteClass Site, string Words)[] lists) =>
        lists.ToDictionary(l => l.Site, l => W(l.Words));

    private static NameSyllables Syllables(string first, string middle, string last, int min, int max, bool mirror = false) =>
        new(W(first), W(middle), W(last), min, max, mirror);

    /// <summary>A tongue of listed first parts.</summary>
    private static NameTongue Listed(int weight, string heads, params (SiteClass Site, string Words)[] words) =>
        new(weight, W(heads), null, null, 0, 0, false, Words(words));

    /// <summary>A tongue of listed first parts that wears its seams down.</summary>
    private static NameTongue Worn(int weight, string heads, params (SiteClass Site, string Words)[] words) =>
        new(weight, W(heads), null, null, 0, 0, true, Words(words));

    /// <summary>A tongue that coins its first parts, beside the listed ones when it has any.</summary>
    private static NameTongue Coined(
        int weight, string heads, NameSyllables syllables, double coinedShare, double bareShare, bool elide,
        params (SiteClass Site, string Words)[] words) =>
        new(weight, W(heads), syllables, null, coinedShare, bareShare, elide, Words(words));

    private static SettlementNameLexicon One(NameTongue tongue) => new([tongue]);

    private static SettlementNameLexicon Many(params NameTongue[] tongues) => new(tongues);

    // The trade tongue: the place words a race without a lexicon uses.
    private static readonly (SiteClass Site, string Words)[] TradeWords =
    [
        (SiteClass.Mouth, "mouth"), (SiteClass.Haven, "haven port hithe"), (SiteClass.Shore, "ness strand"),
        (SiteClass.Landing, "hithe"), (SiteClass.Meeting, "meet"), (SiteClass.Ford, "ford"), (SiteClass.Bridge, "bridge"),
        (SiteClass.Mere, "mere water"), (SiteClass.Hill, "don hill"), (SiteClass.Wood, "ley hurst wood"),
        (SiteClass.Field, "field"), (SiteClass.Waste, "moor heath"), (SiteClass.Salt, "wich"), (SiteClass.Delve, "delve"),
        (SiteClass.Shrine, "minster kirk"), (SiteClass.Market, "_Market"), (SiteClass.Fort, "bury borough chester"),
        (SiteClass.Farm, "ton ham by thorp stead worth"),
    ];

    private static readonly Dictionary<NameStyle, SettlementNameLexicon> ByStyle =
        Enum.GetValues<NameStyle>().ToDictionary(
            style => style,
            style => One(new NameTongue(1, [], null, style, 1, 0, false, Words(TradeWords))));

    private static readonly Dictionary<string, SettlementNameLexicon> ByRace = new(StringComparer.Ordinal)
    {
        // ───────────────────────── The human races ─────────────────────────

        ["northmen"] = One(Worn(1,
            "Hrafn Skarv Sild Tjar Hval Ulf Varg Kobb Orm Bjarn Storm Frost Ask Eld Brim Ring Kjol Gyll Hjalm Hrim Skjald "
            + "Torv Knarr Ravn Odd Ketil Rogn Kolbrand Jarl Sigurd Tor Grim Stein Hauk Brand Nord Sudr Vestr Austr Hvit "
            + "Svart Raud Lang Bratt Myrk Dimm Is Vinter Rune Skip Toll Elg Bjork Furu Gran Hegg Laks Geit Sol",
            (SiteClass.Mouth, "os aros minne"), (SiteClass.Haven, "vik hamn vag havn"),
            (SiteClass.Shore, "nes strond skar ey holm"), (SiteClass.Landing, "stod naust eid"), (SiteClass.Meeting, "mot"),
            (SiteClass.Ford, "vad"), (SiteClass.Bridge, "bru"), (SiteClass.Mere, "vatn sjo tjern"),
            (SiteClass.Hill, "haug fell berg brekka"), (SiteClass.Wood, "skog lund holt vid"),
            (SiteClass.Field, "voll mark eng slette"), (SiteClass.Waste, "mo hei"), (SiteClass.Delve, "gruva verk"),
            (SiteClass.Shrine, "hov ve kirk"), (SiteClass.Market, "kaupang torg"), (SiteClass.Fort, "borg gard stad heim"),
            (SiteClass.Farm, "by bu stad heim dal tun seter thorp rud"))),

        ["midlanders"] = One(Listed(1,
            "Alder Brenn Thorn Rook Moss Haver Corven Lind Fallow Hart Gilden Grey Ashen Fullers Bel Temple Kings Regen "
            + "High Ostre Abbots Oxen Lan Marden Ravel Marl Weir Crown Pellen Briar Hallow Dun Merrow Aure Plough Wains "
            + "Torre Vintry Westry Nor Aubrey Halbrecht Mill Stane Elm Wheat Barley Bishops Priors Monks Hay Guild Yeo "
            + "Sheriff Reeve Tithe Chantry Harrow Furrow Godric Leofric Wulfric Osric Aldred Dunstan Oswin Hereward Wystan "
            + "Sigbert Alfric Godwin Egbert Cenric Aurel Marcel Valen Cassian Lucan Severin Florian Octavian Tiber Garin "
            + "Roland Bertram Amaury Guiscard Odo Hugh Raoul Thibault Gervase Lambert Anselm Baldwin Fulk Drogo Ranulf Emeric",
            (SiteClass.Mouth, "mouth"), (SiteClass.Haven, "haven port hythe"), (SiteClass.Shore, "ness strand cliffe"),
            (SiteClass.Landing, "hythe staithe wharf"), (SiteClass.Meeting, "meet conde"), (SiteClass.Ford, "ford wade wath"),
            (SiteClass.Bridge, "bridge brigg Pont_ _Bridge"), (SiteClass.Mere, "mere pool lac"),
            (SiteClass.Hill, "mont don barrow spire law"), (SiteClass.Wood, "wood holt hurst ley shaw"),
            (SiteClass.Field, "field mead wold lea"), (SiteClass.Waste, "moor heath"), (SiteClass.Salt, "wich"),
            (SiteClass.Delve, "delve pit"), (SiteClass.Shrine, "minster kirk _Abbey _Priory"),
            (SiteClass.Market, "_Cross Chipping_ _Market cheap _Fair"),
            (SiteClass.Fort, "bury caster burg bourg borough Castel_"),
            (SiteClass.Farm, "ton ham wick stede stow worth thorpe cote mark dale vale court ville vaux"))),

        ["saltroadFolk"] = One(Coined(1,
            "Sallah Tefouri Kheru Meddoul Selem Tamanat Melloul Bayyina Hadar Annouz Zaribah Mehret Nakhil Hamada Haddoun "
            + "Bouqal Ilmeddin Azawat Kitaba Midad Aqlam Sefra Ashenti Hikma Najma Warda Rihla Qafila Zaytun Tamra Rimal "
            + "Nujum Safar",
            Syllables(
                "Ta Ti Te Tam Tim Taz Tad Im Ar Az Ifa Ou Za Kha Gha Ha Dje Se Me As Il Zan",
                "ka gha ro ze me na la zou de le fra re se nou ha ske ma zga dje ti mi",
                "ret rt lt mert st t ne ine ar aq em oust azir eq alt elt our ane", 1, 1),
            coinedShare: 0.75, bareShare: 0.35, elide: true,
            (SiteClass.Mouth, "Fam_"), (SiteClass.Haven, "Marsa_ Mina_"), (SiteClass.Shore, "Ras_ Sahel_"),
            (SiteClass.Landing, "Maadi_"), (SiteClass.Ford, "Wadi_ Ouadi_"), (SiteClass.Bridge, "Qantarat_ Wadi_"),
            (SiteClass.Mere, "Birket_ Ghadir_"), (SiteClass.Hill, "Debre_ Jebel_ Tell_"), (SiteClass.Wood, "Ghabat_"),
            (SiteClass.Field, "Marj_"), (SiteClass.Waste, "Bir_ Ain_ Hassi_ Erg_"), (SiteClass.Salt, "Sabkhet_ Mellahat_"),
            (SiteClass.Delve, "Maaden_"), (SiteClass.Shrine, "Sidi_ Zawiyat_ Debre_"), (SiteClass.Market, "Souk_ Khan_ Funduq_"),
            (SiteClass.Fort, "Qasr_ Ksar_ Qalat_ Medinat_ Dar_"), (SiteClass.Farm, "Tin_ Dar_ Ain_ Bir_ Bled_ Abu_"))),

        ["sunlanders"] = One(Coined(1,
            "",
            Syllables(
                "Ku Ka Ki Ma Mba Mbo Nga Ngo Nta Nka Nya Sa Si So Ta Te Tu Wa Za Zi Ba Be Bo Da Di Do Fa Lu Mu Mwe Chi Kpa "
                + "Ogu Oye Esu Agba Iba Aka Okwe Asa Yato Teno Niara Wanga Djeli Kiri Sanka",
                "ra ba nda la ka ma ta sa za fe ko ru nde mba nza ngo le wa ye du lo so bi",
                "", 1, 2),
            coinedShare: 1, bareShare: 0.4, elide: true,
            (SiteClass.Haven, "bando"), (SiteClass.Ford, "ezi ba"), (SiteClass.Bridge, "ezi ba"), (SiteClass.Mere, "ziwa"),
            (SiteClass.Hill, "kulu Ntaba_"), (SiteClass.Wood, "yiri"), (SiteClass.Field, "ombe nga"),
            (SiteClass.Delve, "wura irin"), (SiteClass.Market, "oja sugu"), (SiteClass.Fort, "koro Ile- Mbanza_ kro"),
            (SiteClass.Farm, "dugu bougou kro la so fo"))),

        ["grassRiders"] = One(Listed(1,
            "Khar Kara Tsagaan Ulaan Khokh Kok Shar Altan Bayan Nogoon Bor Khuren Chono Burged Morin Khulan Arslan Gurvan "
            + "Dorvon Mingghan Tumen Baga Ikh Erdene Temur Sukh Monkh Khadan Elsen Davaan Sengur Toosgo Jalgan Uurga Zuun "
            + "Baruun Khoit Urd Naran Saran Tenger Salkhin Tsast Mogoi Buga Tarvagan Shonkhor Khatan Noyon Baatar Yeson "
            + "Doloon Zurgaan Tavan Tsenkher Mongon",
            (SiteClass.Mouth, "_Adag"), (SiteClass.Haven, "_Boomt"), (SiteClass.Shore, "_Ereg"), (SiteClass.Landing, "_Gatlaga"),
            (SiteClass.Meeting, "_Belchir"), (SiteClass.Ford, "_Garam _Gol"), (SiteClass.Bridge, "_Guur _Gol"),
            (SiteClass.Mere, "_Nuur"), (SiteClass.Hill, "_Tolgoi _Uul _Khad _Davaa"), (SiteClass.Wood, "_Shugui _Taiga"),
            (SiteClass.Field, "_Tal _Kheer _Belcheer"), (SiteClass.Waste, "_Bulag _Gobi _Khudag"),
            (SiteClass.Salt, "_Khujir _Davst"), (SiteClass.Delve, "_Uurkhai"), (SiteClass.Shrine, "_Khiid _Ovoo _Sum"),
            (SiteClass.Market, "_Zakh _Ortoo"), (SiteClass.Fort, "_Balgas _Khot _Ordu _Khuree"),
            (SiteClass.Farm, "_Ordu _Ger _Khoton _Ail _Khoshuu _Khural"))),

        ["rainkeepers"] = One(new NameTongue(1,
            W("Jala Megha Varsha Padma Hema Sindhu Deva Ratna Indra Aranya Kalyana Siddha Mallika Tunga Kanaka Matsya Ilan "
              + "Sapta Agastya Mayura Bhadra Surya Chandra Nanda Usha Gandha Vaidya Amrita Karpura Vasanta Vijaya Ambu Neera "
              + "Sravana Shunya Mukti Yuga Gaja Mani Mukta Chandana Kesara Champa Ketaki Kumuda Nila Shveta Maha Nava Purva "
              + "Dakshina Uttara Madhya Simha Hamsa Kokila Mriga"),
            null, null, 0, 0, Elide: true,
            Words(
                (SiteClass.Mouth, "mukham"), (SiteClass.Haven, "pattinam turai"), (SiteClass.Shore, "karai tiram"),
                (SiteClass.Landing, "ghat"), (SiteClass.Meeting, "sangam prayag"), (SiteClass.Ford, "tirtha ghat"),
                (SiteClass.Bridge, "setu palam bandh"), (SiteClass.Mere, "kulam neri wewa vapi taka saram"),
                (SiteClass.Hill, "giri malai konda"), (SiteClass.Wood, "vanam kadu"), (SiteClass.Field, "kshetra vayal"),
                (SiteClass.Waste, "sthali"), (SiteClass.Salt, "uppalam"), (SiteClass.Delve, "khani"),
                (SiteClass.Shrine, "koil alayam matha"), (SiteClass.Market, "pete sandai hatta"),
                (SiteClass.Fort, "pura puri kota garh kottai nagara"), (SiteClass.Farm, "palli grama vadi patti halli gaon kheda wada kuppam cheri")))),

        ["canalFolk"] = One(Coined(1,
            "",
            Syllables(
                "Shui He Jin Tian Liang Wen Sang Ming Duan Yuan Zhi Feng Ying Lian Qian Shou Yan Bi Zhao Jiu Long Ci Tong "
                + "Luo Lan Wan Yong Qing Xun Shu Tao Xiang Mo Wu Jing Chuan Dao Bao Fu Gong Ling Yu Zhong Huang Xiao Dong "
                + "Jiang Ping Dai Bai Hong Lu Song Zhu Mei Yun Xi Nan Bei An Tai",
                "ding ming jing cang kao shui dao yu li pan chuan liang yuan yin shou xi hua hai yang ning de bao fang guang "
                + "long men",
                "", 1, 1),
            coinedShare: 1, bareShare: 0.3, elide: false,
            (SiteClass.Mouth, "kou"), (SiteClass.Haven, "gang jin bu"), (SiteClass.Shore, "tan wan"),
            (SiteClass.Landing, "du bu"), (SiteClass.Meeting, "cha"), (SiteClass.Ford, "du"),
            (SiteClass.Bridge, "qiao ba zha di"), (SiteClass.Mere, "hu ze chi"), (SiteClass.Hill, "shan ling guan"),
            (SiteClass.Wood, "lin"), (SiteClass.Field, "tian ping"), (SiteClass.Waste, "quan sha"), (SiteClass.Salt, "yanchi"),
            (SiteClass.Delve, "kuang"), (SiteClass.Shrine, "si miao"), (SiteClass.Market, "shi ji"),
            (SiteClass.Fort, "cheng fu zhou jing"), (SiteClass.Farm, "zhen cun zhuang tun"))),

        // ───────────────────────── The classic races ─────────────────────────

        ["elf"] = One(Coined(1,
            "",
            Syllables(
                "Ael Vael Orin Sor Quen Lir Eth Ith Mir Ys Thal Sil Cael Fael Nal Es Tael Cir Sen Val Sal El Cal Liath Ais Mel "
                + "Ild Raen Vin Yl Cor Aess Amar Ser Ond Nav Ell Aer Fenn Shaer Quess This Mereth Ilor Isel",
                "a e i a e i va la re ri na ne sa se",
                "", 1, 1),
            coinedShare: 1, bareShare: 0, elide: true,
            (SiteClass.Mouth, "lirenne"), (SiteClass.Haven, "saer saereth"), (SiteClass.Shore, "saer"),
            (SiteClass.Ford, "ovel"), (SiteClass.Bridge, "ovel"), (SiteClass.Mere, "lir liriel"), (SiteClass.Hill, "taen"),
            (SiteClass.Wood, "eth ith ethel"), (SiteClass.Shrine, "senn"), (SiteClass.Fort, "ys andor ethys inor oran aril"),
            (SiteClass.Farm, "quen vael orin quessa wen iel enne elle ossa wyn nith reth"))),

        ["dwarf"] = Many(
            // The hold's own name, in the dwarven tongue.
            Coined(5,
                "",
                Syllables(
                    "Dur Thol Had Var Brak Grom Kel Ost Ham Vor Tor Gund Braz Fel Orr Hol Bar Morr Dol Heth Ul Khar Brek Mag "
                    + "Thrum Grim Rok Drum Thand Mal Nar Skar Dorn",
                    "ma na ra da in um ak ek ar im un or",
                    "", 0, 1),
                coinedShare: 1, bareShare: 0, elide: true,
                (SiteClass.Hill, "kar krag"), (SiteClass.Delve, "drak gund bund"), (SiteClass.Fort, "dun grund _Dun hold"),
                (SiteClass.Farm, "rak gund bund rum rim ruk grath hel")),
            // The gate name, used by the surface peoples.
            Listed(4,
                "Sevengrain Ledger Oath Iron Silver Anvil Stone Tunnel Hunger Deep Malt Seam Hammer Tally Granite Slate Chisel "
                + "Rivet Ingot Bellows Crucible Brazen Lead Flint Basalt Quarry Soot Mattock Ore Gem Beard Barrel Keg Brew "
                + "Assay Measure Bond Pledge Adze Tongs Bronze Zinc Cobalt Nickel Schist Gneiss",
                (SiteClass.Hill, "crag horn _Crag"), (SiteClass.Delve, "delve bore seam _Mine"),
                (SiteClass.Mere, "cistern well"), (SiteClass.Bridge, "_Span"), (SiteClass.Market, "_Scales _Gate"),
                (SiteClass.Fort, "_Gate hold hall _Hold _Deep vault"), (SiteClass.Farm, "hall holt deep mouth plate march _Gate"))),

        ["halfling"] = One(Listed(1,
            "Apple Cider Butter Honey Pippin Goose Hedge Plum Kettle Barge Crumple Pudding Turnip Pickle Elder Ham Quince "
            + "Bramble Mallow Hives Sloe Curdle Wicker Dumpling Russet Snug Hop Tansy Orchard Clover Pig Ginger Pantry "
            + "Muddle Fat Oat Cobble Larder Bacon Custard Bun Scone Jam Parsnip Radish Cress Bilberry Damson Medlar "
            + "Mushroom Thimble Button Quilt Pipe Biscuit Gravy Crumb Loaf",
            (SiteClass.Mouth, "mouth"), (SiteClass.Haven, "hythe _Quay"), (SiteClass.Shore, "_Strand sands"),
            (SiteClass.Landing, "hythe"), (SiteClass.Ford, "ford brook"), (SiteClass.Bridge, "bridge brook"),
            (SiteClass.Mere, "well marsh pond mere"), (SiteClass.Hill, "down _Hill bank knoll"),
            (SiteClass.Wood, "wood holt copse _Thicket"), (SiteClass.Field, "mead fold garden _Green croft"),
            (SiteClass.Market, "_Moot _Fair market"), (SiteClass.Fort, "bury _Moot _Hundred ton stead"),
            (SiteClass.Farm, "cote bottom wick by sett hollow side _Green _End burrow croft combe dale down bank"))),

        ["gnome"] = One(Listed(1,
            "Cog Coggle Lens Gear Tick Sprocket Quill Fobble Ratchet Penny Gimbal Tumbler Facet Gnarl Bevel Crankle Plumb "
            + "Riddle Wimble Quibble Spindle Brass Tinder Fennick Pinion Graft Milli Burrin Twistle Caliper Nibben Puzzle "
            + "Tickle Widget Tock Clinker Hallmark Key Dapple Snicker Bobbin Loupe Flange Gasket Dial Chime Bezel Fidget "
            + "Whittle Tinker Pendle Ferrule Trundle Wobble Ruby Sprig Cam Tappet Shim",
            (SiteClass.Haven, "_Landing"), (SiteClass.Landing, "_Landing"), (SiteClass.Ford, "beck brook ford"),
            (SiteClass.Bridge, "bridge beck"), (SiteClass.Mere, "mere well marsh mire"),
            (SiteClass.Hill, "down _Underhill crook rigg"), (SiteClass.Wood, "wood root -under-Oak -under-Elm"),
            (SiteClass.Field, "fold dell fern"), (SiteClass.Delve, "_Diggings bore"), (SiteClass.Market, "_Cross _Market gate"),
            (SiteClass.Fort, "bury hall holme gate _Warren _Cross"),
            (SiteClass.Farm, "warren burrow _Hollow wick worth dell root crook whistle bottle berry"))),

        ["orc"] = Many(
            // Orcish: short and guttural.
            Coined(5,
                "",
                Syllables(
                    "Gha Kru Uz Zha Sku Mu Du Bro Gra Thro Na Ug Bur Mor Kha Vro Hro Ru Go Gor Ul Og Ska Kra Dur Zo Urz Gru Sna Bol",
                    "rra zga sha lga rga gha kha zha rna rza ska gma",
                    "k l sh g r th kh zh rg sk z t", 1, 1),
                coinedShare: 1, bareShare: 0.5, elide: true,
                (SiteClass.Hill, "_Crag _Rock"), (SiteClass.Ford, "_Ford"), (SiteClass.Bridge, "_Crossing"),
                (SiteClass.Delve, "_Pit"), (SiteClass.Fort, "Castra_ Fort_ _Keep _Wall _Gate Legio_ _Tower _Watch"),
                (SiteClass.Farm, "_Hold _Hearth _Camp _Ring _Den")),
            // The names the legion left on the old frontier forts.
            Listed(3,
                "First Second Third Fourth Fifth Sixth Seventh Eighth Ninth Tenth Eleventh Twelfth Thirteenth Fourteenth "
                + "Twentieth Old Last Black Red Grey Broken Burnt Far Low Lost Lone Bloody Ragged Stubborn Forgotten Bitterest",
                (SiteClass.Hill, "_Redoubt"), (SiteClass.Ford, "_Ford"), (SiteClass.Bridge, "_Bridgehead"),
                (SiteClass.Fort, "_Standard _Rampart _Wall _Palisade _Garrison _Cohort _Gate _Tower"),
                (SiteClass.Farm, "_Milestone _Watch _Camp _Ditch _Picket _Outpost")),
            // Plain names of a hard land.
            Listed(3,
                "Bone Vulture Sinew Lean Tusk Carrion Hide Ash Skull Blood Gristle Marrow Crow Jackal Scar Dust Rust Gore "
                + "Fang Claw Hyena Buzzard Boar Cleaver Hook Chain Shackle Slag Gut Hack Maul Grudge Spite Wrath",
                (SiteClass.Hill, "_Hill crag rock"), (SiteClass.Ford, "_Ford wade"), (SiteClass.Mere, "pool mire"),
                (SiteClass.Delve, "pit _Diggings"), (SiteClass.Waste, "waste flat"), (SiteClass.Fort, "fort hold gate wall"),
                (SiteClass.Farm, "pit maw hearth hold march camp feast split"))),

        // ───────────────────────── The new races ─────────────────────────

        ["hoarkin"] = Many(
            // Their own tongue: long and slow.
            Coined(6,
                "",
                Syllables(
                    "Hoo Saa Vuu Nee Lu Oo Haa Sii Kuu Moo Tee Huu Vaa Eel Noo Saal Um Hii Kaal Loh Vee Suun Koo Iil Tuu Maa "
                    + "Yeel Nuu Laa Miil",
                    "lva me lna ska mma ve ma va ra lma ri lvee mee muu lu ho suu ha na",
                    "skaan resk ham voo haal vaan lumm roolk leesk huul soon kaa nuum haask kuul skoon vaal vesk leek moor "
                    + "huun skeel mak naam", 0, 1),
                coinedShare: 1, bareShare: 1, elide: true),
            // The trade names the Burning use.
            Listed(4,
                "Twenty Cold Whale Hush Tusk Slow Seal Ivory Meat Stranded Moon White Thick Pack Rime Hoar Winter Night "
                + "Aurora North Bear Walrus Narwhal Blubber Sled Polar Glacier Berg Snowblind Longdark Floe Icicle Sleet "
                + "Hail Thaw",
                (SiteClass.Mouth, "_Narrows"), (SiteClass.Haven, "_Narrows floe"), (SiteClass.Shore, "floe shore _Strand _Edge"),
                (SiteClass.Mere, "_Ice hole"), (SiteClass.Hill, "_Ridge"), (SiteClass.Waste, "drift snow _Ice waste"),
                (SiteClass.Fort, "_Moot table _Floe hold"),
                (SiteClass.Farm, "drift snow wait wake pit heart table _Lying _Breath sleep beats"))),

        ["hearthbloods"] = One(Listed(1,
            "Ember Warm Broth Kindle Forty Yak Steam Sweat Feast Coal Heat Oven Meal Fever Banked Simmer Hearth Snowmelt "
            + "Cinder Ration Sulphur Smoulder Plenty Kiln Flush Tallow Stoker Scald Boil Flame Furnace Stew Suet Blister "
            + "Toast Scorch Singe Sear Roast Fuel Char Soup Grill Brazier Griddle Poker",
            (SiteClass.Mere, "spring well mere"), (SiteClass.Hill, "pass crag fell peak ridge stair horn"),
            (SiteClass.Waste, "snow burn"), (SiteClass.Delve, "pit"), (SiteClass.Market, "pact _Rest"),
            (SiteClass.Fort, "hall _Hall hold gate"),
            (SiteClass.Farm, "hall fold warm hollow rock stone wall cradle back breath fed"))),

        ["widegaze"] = One(Listed(1,
            "Farsight All Mirror Brownwall Wide Horizon Smoke Stripe Open Kestrel Seen Edge Beacon Signal Rider Wind Bare "
            + "Glance Witness Herd Sight Glint Four Earth Hawk Tall Sentry Sky Lark Plover Haze Shimmer Mile League Flag "
            + "Lookout Vantage Clear Flat Level Distant Yonder Far Long Low Skyline Prairie",
            (SiteClass.Hill, "_Rise _Knoll hill"), (SiteClass.Field, "grass range _Barrow ripple"),
            (SiteClass.Fort, "_Mound seat _Beacon _Watch watch"),
            (SiteClass.Farm, "_Rise _Knoll _Barrow mound barrow knoll rise mark range look line speck"))),

        ["amberbloods"] = One(Listed(1,
            "Resin Burl Amber Root Sap Pitch Larch Golden Bark Spruce Tear Birch Wound Pine Gum Knot Drip Freckle Wander Fir "
            + "Cedar Yew Tamarack Juniper Needle Cone Ooze Bead Honeydew Lichen Tap Bleed Varnish Rosin Copal Balsam "
            + "Myrrh Tannin Alburn Whorl Bough Sliver Catkin Pollen Timber Cambium",
            (SiteClass.Ford, "_Ford"), (SiteClass.Mere, "well seep"),
            (SiteClass.Wood, "grove stand _Stand wood bole _Grove glade"), (SiteClass.Fort, "keep hold moot _Council fast"),
            (SiteClass.Farm, "holm well stand bole hollow seep root ward _Hollow _Ring face"))),

        ["brinefolk"] = One(Listed(1,
            "Dry Salt Bitter Ebb Seabed Shell Pink Brine Soda Wait Fin Crust Sweet Weep Stamp Shallow Dried Gypsum Bay Gone "
            + "Ninety Brack Fossil True Halite Alkali Natron Borax Selenite Chalk Lime Bleach Crystal Caked Parch Glare "
            + "Thirst White Briny Powder Flake Saline Bittern Rust Ashy Silt",
            (SiteClass.Shore, "shore strand tide"), (SiteClass.Mere, "lake mere water well"),
            (SiteClass.Waste, "flat _Flats crust glare"), (SiteClass.Salt, "pan pans cake crust lick _Pans purse dish block"),
            (SiteClass.Market, "weight purse square"), (SiteClass.Fort, "_Keep harbour holm _Barrows gallery square"),
            (SiteClass.Farm, "harbour shore mark reef strand flat stone rim shelf wait lip"))),

        ["duskreaders"] = One(Listed(1,
            "Dusk Cool Canyon Midnight Shadow Kind Silent Blackeye Cellar Fading Veil Gloved Dim Hare Dark Quiet Dew After "
            + "Owl Lid Blind Vow Redrock Mute Gloam Twilight Umber Ochre Sand Bat Even Vesper Shade Nightjar Cricket Gecko "
            + "Agave Mesa Arroyo Sundown Slate Thick Heat Starlit Wane Lull Murmur",
            (SiteClass.Mere, "well seep"), (SiteClass.Hill, "canyon cliff gorge _Canyon _Gorge shelf rock _Shelf"),
            (SiteClass.Waste, "sand seep rock _Shade"), (SiteClass.Shrine, "vow"), (SiteClass.Market, "_Market"),
            (SiteClass.Fort, "vault gate _Market wall _Wall hold"),
            (SiteClass.Farm, "wall seep stone script cut well sleep tale hour fall writ sill"))),

        ["noonfolk"] = One(Listed(1,
            "Noon Sun Bright Leaf Dawn Vigil Free Gruel Light Green Morning River Kola Water Midday Gold Cloud Smooth Guard "
            + "East Day Zenith Solar Radiant Yellow Blaze Meridian Summer Ray Halo Petal Frond Long First Grain "
            + "Seed Sprout Broad Full Mild Glad Honest Bud Sunlit Daybreak Stem",
            (SiteClass.Ford, "water _Shallows"), (SiteClass.Mere, "well water bowl"),
            (SiteClass.Field, "field grass yard fold"), (SiteClass.Fort, "tower moot pact yard _Court"),
            (SiteClass.Farm, "yard fed well vein wake barn roof bowl feast stand fold sip face hand stone walk"))),

        ["boughwalkers"] = One(Listed(1,
            "Vine Rope Fruit Dye Swing Mother Tree Liana Ripe Floor Hanging Mist Nectar Grip Nest Thousand Shoot Egg Woven "
            + "Top Rigger Fig Branch Feather Upper Bloom Sapling Rain Orchid Parrot Gibbon Canopy Creeper Tendril Mango "
            + "Papaya Palm Bamboo Rattan Monkey Hornbill Sloth Bromeliad Strangler Kapok Banyan Treetop Highest",
            (SiteClass.Ford, "bridge _Span"), (SiteClass.Bridge, "bridge _Span way"),
            (SiteClass.Wood, "bough crown fork _Fork _Crown leaf vine"), (SiteClass.Market, "market"),
            (SiteClass.Fort, "market hall bridge _Span hold"),
            (SiteClass.Farm, "fork hang reach sway roof weave swing house hold fall light way tree"))),

        ["shoalfolk"] = One(Listed(1,
            "Stilt Pearl Tide Plank Reef Oyster Drift Shoal Sponge Wreck Channel Hull Coral Float Post Pilot Swirl Raft "
            + "Slack Sunk Fog Glass Cowrie Nacre Crab Lagoon Wrack Neap Kelp Conch Whelk Limpet Barnacle Urchin Gull Tern "
            + "Heron Net Oar Buoy Anchor Mussel Eel Skiff Sandbar Breaker Spume Low",
            (SiteClass.Mouth, "_Channel water _Ebb"), (SiteClass.Haven, "cove holm fleet row moot house stilts"),
            (SiteClass.Shore, "water mark reef shoal walk post wrack"), (SiteClass.Mere, "_Lagoon water"),
            (SiteClass.Market, "moot row"), (SiteClass.Fort, "gate hold moot town fleet"),
            (SiteClass.Farm, "holm walk row bed stead town house post hold fast skin"))),

        ["swiftborn"] = One(Listed(1,
            "Quick Thirty Month Olive Swift Seven Arch Season Jig Heart Harvest Now Haste Torch Wild Class Song Race Fleet "
            + "Brisk Dash Spark Whirl Stride Flick Blink Dart Leap Rush Skip Flit Snap Trice Minute Hour Week Morrow Relay "
            + "Hand Hurry Sprint Bolt Zest Nimble Sudden Fortnight Instant",
            (SiteClass.Ford, "water run leap"), (SiteClass.Bridge, "water run _Arch"), (SiteClass.Mere, "water"),
            (SiteClass.Hill, "hill _Hill crown terrace"), (SiteClass.Field, "field row vine run"),
            (SiteClass.Fort, "tower gate hold crown hill"),
            (SiteClass.Farm, "vine year feast leap run step shift dial row mark pass rush pick spent"))),

        ["flinchless"] = One(Listed(1,
            "Look Marshal's Oath Round Luke Splint Line Count Mitten Paired Surgeon's Foot Cohort Steady Drill Test Rule "
            + "Stitch Captain's Mend Scalp Eagle Gauze Shield March Mourn Linen Empty Numb Double Pair Lancet Suture "
            + "Bandage Poultice Splice Rank File Muster Roll Sergeant's Tourniquet Probe Clamp Cautery Square Sworn "
            + "Ready",
            (SiteClass.Ford, "ford"), (SiteClass.Mere, "well water _Wells"), (SiteClass.Hill, "_Hill"),
            (SiteClass.Field, "field _Field yard"), (SiteClass.Fort, "hold wall yard _Post hall _Legion"),
            (SiteClass.Farm, "check log kept camp corner well stead fold watch break count warm mark"))),

        ["lanternkin"] = One(Listed(1,
            "Glow Vial Bell Fox Candle Glimmer Lumen Pale Wax Worm Lantern Never Lamp Cap Ghost Wan Spore Phial Wisp Gleam "
            + "Taper Moth Firefly Mycel Gill Opal Lustre Sheen Twinkle Beam Flare Mirk Murk Tinsel Puffball Morel "
            + "Bracket Hypha Dimmer Shine Lambent Gloss Flicker Glitter Dazzle Nimbus",
            (SiteClass.Mere, "fen water"), (SiteClass.Wood, "wood bough canopy moss root floor"),
            (SiteClass.Market, "glass rack vials quarter _Court"), (SiteClass.Fort, "hall keep gate quarter _Court"),
            (SiteClass.Farm, "cap light glow vein water moss floor shine fire thread room _Hollow"))),

        ["stillfaces"] = One(Listed(1,
            "Still Musk Scent Fern Close Truffle Hound Wet Face Plain Clove Calm Hold South Perfume Under Mint Thyme Nettle "
            + "Beech Sage Rosemary Lavender Loam Peat Damp Wood Petrichor Bracken Heather Marjoram Fennel Anise Camphor "
            + "Incense Odour Whiff Spice Balm Tang Savour Fume Aroma Spoor Trail",
            (SiteClass.Ford, "water _After_Rain"), (SiteClass.Mere, "water _and_Rain"),
            (SiteClass.Hill, "_Below_the_Ridge _Slope"), (SiteClass.Wood, "wood glade _Under_Beeches _and_Birch"),
            (SiteClass.Field, "bank _and_Clover"), (SiteClass.Fort, "hall _Village _Posts _Place"),
            (SiteClass.Farm, "hollow smell post breath house ground mould scent note bank fog _Hollow _Village _Lodge"))),

        ["twofold"] = Many(
            // Their own tongue: every name reads the same both ways.
            Coined(6,
                "",
                Syllables(
                    "Ela Tira Nali Oka Sela Mira Ana Uve Sanu Kira Ota Ime Luna Rava Tole Aya Kova Eya Ilo Sero Pavi Olu Ati "
                    + "Ura Ine Soti Hanu Dora Ame Keli Bera Dumi Fano Gise Haro Ivu Jali Nemo Rido Tasi",
                    "ra li no se ta mi du ke",
                    "n m l r s d t v k b", 0, 1, mirror: true),
                coinedShare: 1, bareShare: 1, elide: false),
            // The trade names: the two halves of life and the Turning between them.
            Listed(4,
                "Turning Indigo Staying Going Pair Year Both Strangers' Mid Teak Twin Other Halfway Return Meeting Parting "
                + "Twice Either Saffron Coupled Yoke Hinge Reverse Facing Mirrored Wedded Parted Homeward Outward "
                + "Gold Chain Double Second Even",
                (SiteClass.Wood, "_Grove fig"), (SiteClass.Field, "field"), (SiteClass.Shrine, "shrine"),
                (SiteClass.Fort, "house hall _Hall shrine _Door"),
                (SiteClass.Farm, "house field road faces ways hearth mothers going turn dawn fig"))),

        ["bitterbloods"] = One(Listed(1,
            "Glove Venom Frog Toadstool Snake Mixing Seasoned Nightshade Mottle Speckle Cure Leech Sting Clean "
            + "Puffer Arrow Gall Taster Wormwood Hemlock Blend Sour Dead Rash Stun Rue Spit Swamp Hard Viper Newt Adder "
            + "Wasp Hornet Wolfsbane Henbane Mandrake Oleander Tincture Draught Philtre Antidote Dose Bile Canker Quinine",
            (SiteClass.Ford, "ford water"), (SiteClass.Mere, "fen mire pool marsh water _Lagoon well"),
            (SiteClass.Wood, "wood _Rise"), (SiteClass.Fort, "hold garden _Mound post"),
            (SiteClass.Farm, "fen well pen garden pool drop mire bank cup post hollow gut stilts _Stilts"))),

        ["catfolk"] = One(Listed(1,
            "Mouse Granary Huntress Portion Roof Tail Pounce Whisker Tawny Nap Prowl Fish Slit Rat Sash Ledge Paw Stores "
            + "Lion Fowl Soft Squeak Ear Ambush Bread Cat Streak Litter Plains Purr Velvet Tabby Stalk Crouch Pad Mew Hiss "
            + "Yowl Scruff Whisk Pelt Tuft Lynx Serval Caracal Ocelot Civet Amble",
            (SiteClass.Ford, "ford"), (SiteClass.Hill, "_Hill _Rock ledge shelf"),
            (SiteClass.Field, "grass _Grass run stalk"), (SiteClass.Market, "tithe trust ward"),
            (SiteClass.Fort, "ward wall hold _Ward moot way roof"),
            (SiteClass.Farm, "wall ward moot stone grass tail holm yard nap paw well run reed"))),
    };
}
