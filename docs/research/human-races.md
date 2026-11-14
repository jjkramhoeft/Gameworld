# Human-Looking Races: Near-Human Races from Pop Culture and Myth, and Human Peoples

*Research date: 2026-11-14 (second pass, same day). Scope: a candidate race list in which every race **looks like a regular human or nearly so**: a person an actor could play with light make-up. The list has two groups. Group N is 20 **near-human races** drawn from pop culture and folklore: an odd skin colour, horns, eyes, stature, markings, a long life or an uncanny gift on a human body and face. Group P is 16 **human peoples** that differ only in way of life, build, colouring and homeland. §1 states the method and the findings. §2 is the survey of near-human races, by archetype. §3 is the survey behind the human peoples. §4 is the proposed list, race by race. §5 holds the tables: the list in the format of `map-generation.md` Appendix E, the coverage of the 16 biomes, the measured shares and the list as Parameter Set json. §6 is the design note with the open questions, the overlaps with [races.md](races.md) and the conflicts with the readme and `map-generation.md`. §7 lists the sources. The sibling note [races.md](races.md) covers the beast-folk and monster races; this note does not repeat it.*

**How to read the citations**

- Each claim cites the source that owns it, by key in square brackets. The keys are listed in §7.
- **[partially verified]** means the source exists and is cited, but the claim was confirmed only from a search-index excerpt or a secondary source, not by reading the page itself.
- **[unverified]** means general knowledge that was not checked against any source in this pass.
- The sources were read with a fetch tool that returns the first 100,000 characters of a page. Official and publisher-licensed sources: the Guild Wars 2 wiki (ArenaNet's own), the D&D Free Rules, Archives of Nethys (Paizo-licensed), the Star Wars Databank, the Crusader Kings III wiki (Paradox's own). Independent fan wikis: bg3.wiki, Destinypedia, Warcraft Wiki, Coppermind, the FFXIV community wiki. Everything else is a Wikipedia article about the work, not the work. Fandom-hosted wikis (Wookieepedia among them) refused the tool.
- Names of races from games, books and films appear as **evidence** and in the "Based on" column, so that the reader can find a favourite race in the list. A name that is a franchise's own invention (Chiss, Mirialan, Awoken, Time Lord, Tiefling, Aasimar and the like) is not used as the name of a proposed race; the proposed race has an original working name. Names from folklore and myth (Fae, Changeling, Nephilim, Selkie, Dhampir, Yaksha, Jinn) are used as they are.
- Source wording is paraphrased, not quoted. Every **Appearance** and **Story** in §4 is written fresh for this note; every habitat weight, every `common` value and every **Design note** is my own judgement, not a sourced claim.

**Hooks in this repo** (working tree at commit `5eedf67`)

| Hook | Where | What it means for this list |
|---|---|---|
| Race | `readme.md`, "Race" | "A people that can live in the world: humans, elves, dwarves and so on." A race has an id, a name, a toggle, a frequency per biome and an optional height modifier. Nothing else |
| The rule | `map-generation.md` §19.2 | On one patch the races that are on share the capacity by `frequency(race, biome) · heightFactor(race, height)`. Only the proportions within one biome matter |
| The default list | `map-generation.md` Appendix E | `frequency = habitat · common`, habitat 0 avoided, 1 tolerated, 2 good, 3 native. `human` has common 10; the other races that are on have 2; the rest 1 or 0.5 |
| Biomes | `map-generation.md` §13.2 | 15 land biomes from thermal belt and aridity, and Mangrove as an override on warm coasts of flat and lowland zones |
| Height | `adding-a-race.md` §1 | Half of the land patches lie below 155 m, nine tenths below 432 m, and the highest patch is at 2,000 m |
| Open topics | `map-generation-research.md` §10.7 | Water, bedrock and Supernatural weights per race, a presence field per race, variants picked by the biome, and a density per race are all open |
| Earlier archetypes | `races.md` §3 | E1 Giant-blooded, H1 Fiend-touched, H2 Celestial and elemental-touched, H3 Dragonkin, J3 Frost folk and J5 Shapeshifters already cover part of group N (§6.3) |

---

## 1. Overview and method

### 1.1 Scope

**Near-human** here means: human body plan, human face, human size or close to it. What differs is on the surface or inside: skin of an unusual colour, horns, pointed or furred ears, a tail, unusual eyes, markings, great or small stature, a very long life, or a gift. The test is whether an actor could play the race with make-up, contact lenses and a prosthetic or two. That leaves out the races with animal heads, scales all over or more than two arms, which `races.md` covers.

**Human peoples** are plain humans. They are kept from the first pass of this note, condensed.

Every entry is a **race** in the readme's sense: the user has decided that "race" is the only term, and that every people is a separate race (§6.4 item 1).

### 1.2 Method

1. Survey near-human races across tabletop games, computer and mobile games, film and television, literature, anime and folklore: what makes each visibly not a standard human, its hook, and the environment its source ties it to (§2).
2. Group them into archetypes that recur across sources (§2).
3. Propose one original race per archetype, mixed from several sources, so that none is a copy of one franchise's race (§4, group N).
4. Take the biome weights from the environment of the sources where they have one, and from reasoned judgement where they do not. Each entry says which.
5. Keep the 16 human peoples of the first pass (§3, §4 group P).
6. Check the combined list of 36 by generating 50 regions (§5.3).

### 1.3 Findings

1. **The same dozen near-human ideas recur everywhere.** A giant northern people, a horned people of fiendish blood, a radiant people of heavenly blood, a faceless shapechanger, a blue people of the ice, a tattooed mystic people, a pale star-touched people, the fair folk, a long-lived lordly people, a seal-skin sea people, an elemental-blooded desert people, an animal-eared people. Each appears in at least three unrelated sources (§2).
2. **Folklore got there first.** Nearly every archetype has a mythic root that is free to use: jötnar for the giants, nephilim for the heaven-touched, changelings, the aos sí for the fair folk, selkies, jinn, kitsune, dhampirs, the huldra, yakshas [JOTUNN] [NEPH] [CHG-FOLK] [AOS] [SELKIE] [JINN] [KITSUNE] [DHAMPIR] [HULDER] [YAKSHA]. The franchises supply the modern look; the myths supply the names.
3. **About half of the near-human races have a home environment in their source, and half do not.** The giants live in snow and mountains [GW2-NORN] [WOW-VRY]; the blue people on an ice moon [ANDOR]; the jinn in deserts and abandoned places [JINN]; the huldra in the forest [HULDER]; selkies on northern coasts [SELKIE]. The fiend-touched, the heaven-touched and the shapechangers have no homeland: they are born among humans and hide or are shunned there [BG3-TIEF] [PF2-VH] [EBERRON]. The star-born and the time lords live off-world [DESTINY] [TL]. For those the biome weights are judgement.
4. **Near-human races are minorities in their sources.** They are exiles, hidden lineages, remnants or rare births. The list makes them so: together they hold about a quarter of all persons when the list stands alone, and no one of them more than 3.2 % (§5.3).
5. **Human peoples follow the land** (first pass, §3): the way of life follows the biome, and the numbers follow the way of life.
6. **The generator cannot give a race a homeland yet.** With the combined list the median region holds 23 of the 36 races at 1 % or more of its persons, and its largest race has 17 % (§5.3). This is the largest gap between what the list wants and what the rule gives (§6.4 item 2).

---

## 2. Survey of near-human races

Each table row is one source race: what marks it, its hook, and the environment its source gives it. "None" under Environment means the source gives no homeland or only an off-world one.

### 2.1 Giants among men

| Source race | Visibly different | Hook | Environment in the source |
|---|---|---|---|
| Norn, Guild Wars 2 | Human-looking but about nine feet tall and heavily built; can take a furred beast form | Hunters who fear being forgotten more than death and seek a legend; revere animal spirits, Bear first | Snowy mountains: the Shiverpeaks [GW2-NORN] |
| Goliath, D&D | Seven to eight feet; some resemble stone or fire giants | Descendants of giants who mean to outdo their forebears | None stated in the free rules [DND-FR] |
| Vrykul, World of Warcraft | Very large, muscular, bearded and tattooed; some with skin like ice, metal or stone | Honour-bound warriors devoted to a war god; the ancestors of humans | The frozen north [WOW-VRY] |
| Roegadyn, Final Fantasy XIV | Massive frames, the tallest playable race | Descended from seafarers of the northern seas | Northern seas [FF14] |
| Qunari, Dragon Age | Large and horned (some hornless) | A people ruled by one philosophy that gives everyone a role | Northern islands; climate not on the page [QUNARI] |
| Jötnar, Norse myth | Not always large; often at odds with gods and men, yet they marry gods | The old powers beyond the fence | Mountains, forests, a realm of their own [JOTUNN] |
| Horneaters, the Cosmere | Tall, pale, red-haired humans of mixed ancestry who can eat shells | Cooks and peak-dwellers with their own gods | Mountain peaks with warm lakes [CM-UNK] |

`races.md` E1 (Giant-blooded) is the same archetype and is already in the default list as `giantBlooded`, off (§6.3).

### 2.2 Fiend-touched and horned peoples

| Source race | Visibly different | Hook | Environment in the source |
|---|---|---|---|
| Tiefling, D&D and Baldur's Gate 3 | Horns, a tail, ember-bright eyes; by lineage also spines, fur, tusks or golden eyes | Mortals with infernal blood from an old pact or ancestor; met with suspicion wherever they live | None: they live among others [BG3-TIEF] [DND-FR] |
| Nephilim (fiendish), Pathfinder | Fiendish features | Shaped by the magic of the outer planes | None [PF2-VH] |
| Hungerseed, Pathfinder | Half-oni | Hot-tempered youths | None [PF2-VH] |
| Diclonius, *Elfen Lied* | Human with two horn-like growths on the skull | Unseen arms that cut; feared and caged | None [ELFEN] |
| Draenei, World of Warcraft | Tall, blue to violet skin, horns, hooves, a tail, glowing eyes | Exiles who fled a demon legion and hold to a holy Light | Off-world exiles [WOW-DRA] |
| Zabrak, Star Wars; oni, Japanese folklore | Crowns of small horns; horns and red or blue skin | | [unverified] |

`races.md` H1 (Fiend-touched) is the same archetype. It is not in the default list (§6.3).

### 2.3 Heaven-touched

| Source race | Visibly different | Hook | Environment in the source |
|---|---|---|---|
| Aasimar, D&D | Angelic looks; can make the eyes glow and call up wings for a while | Mortals with celestial blood. Not playable in Baldur's Gate 3: there they are non-player characters, among them an immortal daughter of a moon goddess | None [BG3-AAS]. Not in the D&D Free Rules [DND-FR] |
| Nephilim (celestial), Pathfinder | Celestial features | As above | None [PF2-VH] |
| Nephilim, Genesis | Mighty men of old, later pictured as giants | Children of heavenly beings and human women | None [NEPH] |
| Valkyries, Norse myth | Women, some linked to swans | Choosers of the slain who take mortal heroes as husbands | None [VALK] |
| High Entia, Xenoblade Chronicles | Bird-like humanoids (small head wings [unverified]) | Lives of centuries | A capital of their own [XENO] |
| Moon people, *The Tale of the Bamboo Cutter* | Heavenly beings, never described | A robe that takes away sorrow and all care for the earth | The moon [KAGUYA] |

`races.md` H2 (Celestial and elemental-touched) covers this and §2.11.

### 2.4 Shapechangers and the faceless

| Source race | Visibly different | Hook | Environment in the source |
|---|---|---|---|
| Changeling, European folklore | A human-like substitute, sickly, stunted or too wise | Left by fairies or trolls in place of a stolen child | The cradle [CHG-FOLK] |
| Changeling, D&D (Eberron) | Based on the doppelganger [EBERRON]; pale and blank-featured in its own face [unverified] | Can wear any face | None |
| Changeling, Pathfinder | Children of hags, with occult gifts | Hear a call to become hags themselves | None [PF2-VH] |
| Reflection, Pathfinder | A duplicate of a person | Made by a failed ritual; may try to replace the original | None [PF2-VH] |
| Kitsune, Japanese folklore and Pathfinder | Foxes in human form, betrayed by a tail or a fox-shaped shadow | Fox wives whose children have gifts; messengers of a rice god | Among humans [KITSUNE] [PF2-ANC] |
| Changeling, *Changeling: The Dreaming* | Fae souls born into human bodies | Fed by human dreams, worn away by disbelief | Among humans [WOD-CTD] |
| Face Dancers, *Dune* | Shape-shifting servants | | Named only on the page read [BENE] [unverified] |

`races.md` J5 (Shapeshifters) names changelings, kitsune and beastkin (§6.3).

### 2.5 Blue peoples of the cold

| Source race | Visibly different | Hook | Environment in the source |
|---|---|---|---|
| Chiss, Star Wars | Blue skin, glowing red eyes | A closed, disciplined power beyond the known galaxy that recalls its officers | A home world in unknown space [THRAWN]. That the world is ice-bound is general knowledge [unverified]; the Databank page for the species was not found |
| Andorian, Star Trek | Blue or blue-green skin, white hair, antennae | | An icy moon; cities underground for the warmth of the rock [ANDOR] |
| Samsaran, Pathfinder | Blue skin | Reborn again and again, with scraps of memory from past lives | A hidden land [PF2-ANC] |
| Sebacean, *Farscape* | Human-looking | Cannot bear heat | None [FARSCAPE] |
| Asari, Mass Effect | Blue skin, head fins in place of hair, one sex | Live over a thousand years in three stages; mediators | A home world; no climate given [ASARI] |
| Na'vi, *Avatar* | Ten feet tall, blue, tailed, cat-eyed | Clans of forest, sea and volcano | Giant-tree forest [NAVI]. Too far from human for this list |
| Pantoran, Star Wars; the blue-skinned Natan people, the Cosmere | Blue skin | | [unverified] |

`races.md` J3 (Frost folk) shares the biomes and is in the default list as `frostFolk`, off (§6.3).

### 2.6 Marked and tattooed peoples

| Source race | Visibly different | Hook | Environment in the source |
|---|---|---|---|
| Mirialan, Star Wars | Yellow-green skin and black patterned markings on the face | The Databank gives looks only [SW-MIR]. That the marks are tattoos earned by deeds, and that the home world is cold and dry, is general knowledge [unverified] | None verified |
| Bajoran, Star Trek | Human but for ridges across the nose; a chained earring of faith | A devout people freed from a fifty-year occupation; family name first | A home world [BAJOR] |
| Trill, Star Trek | Human-looking (spots [unverified]) | A chosen few carry a long-lived symbiont with the memories of its earlier hosts | A home world [TRILL] |
| Draenei, World of Warcraft | A rune on the forehead | Given by their holy patrons | [WOW-DRA] |
| Thaylen, the Cosmere | Short, tanned humans with long white eyebrows that are waxed and styled | Merchants and sailors | Cold mountainous islands [CM-THAY] |
| Rattataki, Star Wars | Chalk-white skin | | [SW-PT] |

### 2.7 Star-born and void-touched

| Source race | Visibly different | Hook | Environment in the source |
|---|---|---|---|
| Awoken, Destiny | Human with pale blue, purple, green or grey skin and luminous eyes | Descendants of a ship's people who fell through a rift into a pocket world where they did not age; some returned, some stayed away, some rejoined humankind | An asteroid reef, a hidden city, and Earth [DESTINY] |
| Void elf, World of Warcraft | Pale skin, blue-violet hair, an echoing voice | Exiled scholars changed by the Void, which whispers to them still | A rift [WOW-VOID] |
| Fetchling, Pathfinder | Former humans with colourless skin and glowing eyes | Shaped by generations in a shadow world | A shadow plane [PF2-ANC] |
| Kryptonian, DC Comics | Exactly human in looks | Ordinary at home, mighty under another sun | A lost world [KRYPTON] |

### 2.8 The fair folk

| Source race | Visibly different | Hook | Environment in the source |
|---|---|---|---|
| Aos sí (the Sidhe), Irish and Scottish folklore | Dazzlingly beautiful, sometimes terrible | Never named outright; given food, drink and a share of the crop; their places are left alone. Said to descend from the old gods | Mounds that are doors to the Otherworld [AOS] |
| Sylvari, Guild Wars 2 | Near-human with tilted eyes and pointed ears, but made of plant: leaf hair, bark skin, a glow at night | Born grown from a great tree; none has yet died of age | A grove by a bay [GW2-SYL] |
| Fadrim, Pathfinder | People steeped in the energy of the First World, changeable | | None [PF2-VH] |
| Hulder, Scandinavian folklore | A lovely woman who hides a cow's or fox's tail, sometimes a back like a hollow tree | Kind to charcoal burners who leave her food | The forest [HULDER] |
| Elves of *Frieren* | Human-looking with long ears [unverified] | So long-lived that ten years pass like a season | [FRIEREN] |
| Elves of *The Witcher* | | Came from over the sea and took the plains and forests | Plains and forests [WITCHER] |
| Hylian, The Legend of Zelda | Long pointed ears | Ears said to hear the gods | A temperate kingdom [ZELDA] |
| Elezen, Final Fantasy XIV | Tall, slender, elfin, somewhat longer-lived | Once claimed the whole land | [FF14] |

The default list's `elf` (B1) and `races.md` J4 (Small fey) are neighbours (§6.3).

### 2.9 Long-lived lords of time and lore

| Source race | Visibly different | Hook | Environment in the source |
|---|---|---|---|
| Time Lords, *Doctor Who* | Look human; two hearts; at the point of death they renew into a new body and temper, keeping their memories, a set number of times; lives of millennia | Masters of time who swore not to interfere and mostly watch; those who leave are renegades | A home world with one great citadel [TL] |
| Ancients, *Stargate* | Human-like | Far ahead of humankind, built the gates, then left the flesh behind | Another galaxy, then Earth [SG-ANC] |
| Númenóreans, Tolkien | Taller than other Men (about 6 ft 4 in) and living some 200 years | Sea-kings and shipwrights whose island sank when they reached for immortality; the survivors founded kingdoms in exile | A star-shaped island in the western sea [NUMENOR] |
| Valyrians, *A Song of Ice and Fire* | Silver-blond hair and violet eyes | Forty families of dragonlords from a lost realm | A volcanic peninsula [TARG] [ASOIAF] |
| Azarketi, Pathfinder | (Aquatic humans [unverified]) | Heirs of a shattered empire who keep its pride | The sea [PF2-ANC] |
| Citizens of the Culture, Iain M. Banks | Engineered humans: lives of 350 to 400 years or as long as they like, drug glands, a change of sex at will | A post-scarcity people | Ships and orbitals [CULTURE] |
| Hainish peoples, Ursula K. Le Guin | All human worlds were seeded from one; most can pass for each other, some were altered: one people with no fixed sex, one very hairy | | A world each [HAIN] |
| Aes Sedai, *The Wheel of Time* | (An ageless face [unverified]) | A feared order of women who work the One Power | A tower [WOT-AES] |

### 2.10 Sea-kin

| Source race | Visibly different | Hook | Environment in the source |
|---|---|---|---|
| Selkie, Scottish folklore | Human once the seal skin is off; their children have webbed fingers | Whoever hides the skin keeps the selkie ashore | The coasts of the Northern Isles [SELKIE] |
| Finfolk, Orkney and Shetland folklore | | Confused with selkies | The same coasts [SELKIE] |
| Undine, Pathfinder | People descended from beings of the Plane of Water | | None [PF2-VH] |
| Roegadyn Sea Wolves, Lalafell, Final Fantasy XIV | | From northern seas and southern seas | [FF14] |
| Real sea nomads | Larger spleens in their free divers | | Shallow warm coasts [BAJAU] [ILARDO] [partially verified] |

`races.md` G1 (Merfolk) lives in the water; these live on the shore (§6.3).

### 2.11 Elemental-blooded

| Source race | Visibly different | Hook | Environment in the source |
|---|---|---|---|
| Jinn, Islamic and Arabian tradition | Made of smokeless fire, usually unseen, able to take human shape | Live in tribes like people; tales tell of marriages with humans | Deserts and abandoned places [JINN] |
| Geniekin, Pathfinder (ifrit, oread, sylph, undine, suli, talos) | Marked by fire, earth, air, water or metal | Descendants of genies and elemental beings | None [PF2-VH] |
| Genasi, D&D | Elemental features | | [unverified]; the page for the planetouched was not found |
| Godlike, Pillars of Eternity | Fire, moon, nature and death kinds | Born at random to ordinary parents | None (`races.md` [POE-SR]) |
| Vision holders and archons, Genshin Impact | Human; a gem grants command of one element | Seven nations, one element and one ideal each | A nation per element [GENSHIN] |

### 2.12 Animal-eared and tailed peoples

| Source race | Visibly different | Hook | Environment in the source |
|---|---|---|---|
| Kemonomimi, anime and manga | Human with animal ears and sometimes a tail: cat, rabbit, fox, dog, wolf | A whole genre convention | None [KEMONO] |
| Miqo'te, Final Fantasy XIV | Large cat ears and a furred tail | Came over the ice in an age of frost; clans of the sun and of the moon | [FF14]; desert and forest clans [unverified] |
| Viera, Final Fantasy XIV | Long furred rabbit ears | | [FF14]; forest [unverified] |
| Beastkin, Pathfinder | Take on animal traits, part or whole | One foot in the wild, one in town | None [PF2-VH] |
| Shifter, D&D (Eberron) | Based on the lycanthrope | | None [EBERRON] |
| Norn beast form, Guild Wars 2 | Furred, fanged, clawed | | [GW2-NORN] |
| Saiyan, *Dragon Ball*; Faunus, *RWBY* | A tail; one animal trait | | [unverified] |

`races.md` F2 (Catfolk, in the default list as `catfolk`, off) has a full cat's head; these have a human face (§6.3).

### 2.13 The night-pale and death-touched

| Source race | Visibly different | Hook | Environment in the source |
|---|---|---|---|
| Dhampir, Balkan folklore | Child of a vampire father and a human mother; odd signs such as a snub nose or a missing nail | Can sense vampires and destroy them; the gift runs in the male line | Villages [DHAMPIR] |
| Dhampir, Pathfinder | Pale and long-lived | Between life and undeath; society is hard for them | None [PF2-VH] |
| Duskwalker, Pathfinder | Made, not born, in places tied to death | Guides of souls | None [PF2-VH] |
| Kindred, *Vampire: The Masquerade* | Pass as human; some clans show it | A hidden society that punishes exposure | Cities [VTM] |
| Sheikah, The Legend of Zelda | White hair and red eyes | Hidden guardians of a royal house | A village [ZELDA] |

`races.md` I1 (Undead people, in the default list as `undead`, off) is the dead themselves; these are living (§6.3).

### 2.14 Green-kin of the forest

Sylvari [GW2-SYL] and the hulder [HULDER] from §2.8; Ardande, Pathfinder: forest-born people with the traits of trees, bloom and moss [PF2-VH]; Delvians, *Farscape*: a blue-skinned plant people [FARSCAPE]; yakshas, South Asian tradition: nature spirits of water, trees and treasure who guard wells, lakes and gates, kind or deadly by turns [YAKSHA]; tengu, Japan: mountain and forest spirits in human shape with a long nose, tied to mountain ascetics, teachers of the sword [TENGU]. `races.md` J2 (Plant folk) is the non-human end of this.

### 2.15 Single-sex and matriarchal peoples

Gerudo, The Legend of Zelda: tall, red-haired, bronze-skinned desert people who are nearly all women; one man is born in a century and becomes king [ZELDA]. Asari (§2.5): one sex, three ages of life [ASARI]. Hyldnir, World of Warcraft: a matriarchal branch of the northern giants [WOW-VRY]. Amazons, Greek myth [unverified].

### 2.16 Disciplined minds

Vulcans, Star Trek: human but for arched brows and pointed ears; stronger and longer-lived (some past 220 years); a hot, dry, thin-aired home world [VULCAN]. Betazoids, Star Trek: human-looking telepaths [TRILL]. Kalashtar, D&D (Eberron): descendants of monks who merged with dream spirits, hunted, living in mountain temple-keeps [EBERRON]. Bene Gesserit, *Dune*: a trained sisterhood with command of body and voice and a breeding plan of many generations [BENE]. Minbari, *Babylon 5*: three castes, religious, warrior and worker [B5].

### 2.17 Altered by craft

Witchers, Sapkowski: monster hunters given superhuman abilities, trained in a keep [WITCHER]; cat-like eyes and white hair [unverified]. Space Marines, Warhammer 40,000: recruits remade with implanted organs, a second heart among them, taller and stronger, living for centuries [WH40K]. Metahumans, Shadowrun: elves, dwarfs, orks and trolls are humans who changed or were born changed when magic returned [SHADOWRUN]. Culture citizens (§2.9) [CULTURE]. Dragon-marked peoples sit beside these: Au Ra, Final Fantasy XIV, with horns, patches of scale and a tail [FF14]; dragonblood, Pathfinder [PF2-VH]; Valyrians (§2.9).

### 2.18 Borderline and left out

| Kind | Sources | Why not proposed |
|---|---|---|
| Small folk | Lalafell [FF14]; hobbits, called a kind of human by one scholar [TOLK-MEN] | The default list's `halfling` and `gnome` already are this |
| Pointed-eared tall folk and their dark kin | §2.8; Tiste peoples of Malazan [unverified: the page read says nothing of them [MALAZAN]] | The default list's `elf` and `darkElf` |
| Head-tailed and crested | Togruta with striped head-tails and horn-like crowns [SW-PT]; asari head fins [ASARI]; Twi'lek, Minbari bone crests [unverified] | More than light make-up; one step past near-human |
| One-feature humans | Thaylen brows [CM-THAY]; a hairy people [HAIN]; the three looks of one desert kingdom [ASOIAF] | Covered by the human peoples of group P, which are humans with their own looks |

---

## 3. Survey behind the human peoples (condensed from the first pass)

**Fiction** ties a human people to one kind of land: horse nomads on a steppe, sea raiders on bare islands, short marsh dwellers in a swamp, a desert south whose people look different on coast, sand and mountain [ASOIAF]; one desert people whose whole culture turns on saving water [FREMEN]; a polar nation modelled on Arctic cultures [ATLA]; a fair desert people and a dark sea people, against expectation [WOT]; an archipelago where most people are brown-skinned and colouring carries no moral weight [EARTHSEA]. Tolkien's Men differ by language, housing and livelihood, but scholars note a moral map from a good west to an evil east [TOLK-MEN], and Howard's peoples are each one real people under another name [HYB]. Both are things to avoid. A person can be born to one people and raised by another [SAXON].

**Games** make the tie a rule. Bannerlord gives each of six human cultures a homeland and a way of war: infantry from northern forests, horse archers from the eastern steppe, a desert people, a woodland people of ambush and archery [MB2-W]. Crusader Kings III builds a culture from five pillars plus traditions, and traditions such as Forest Folk, Hill Dwellers, Mountain Homes, Dryland Dwellers, Jungle Dwellers, Agrarian and Maritime Mercantilism need that terrain and pay off only there [CK3-T] [CK3-C]. Horizon Zero Dawn has mountain hunter-gatherers and desert city builders [HZD].

**Real peoples**: which way of life occupies which land. This grounds the frequencies of group P.

| Way of life | Land (repo biome) | What the sources say |
|---|---|---|
| Sea-mammal hunting | Ice & snow, Tundra coasts | Hunters and fishers of sea, ice and tundra; skin boats, dog sleds, snow, sod and skin dwellings [INUIT] |
| Reindeer herding | Tundra, Boreal forest | About 100,000 herders round the pole; meat herds in some places, small riding herds in others [REIN] |
| Burn-beating and forest trades | Boreal and Temperate mixed forest | Rye sown in fresh ash, 10 to 30 years of regrowth, a large area per family [SLASH] [SHIFT]; tar as a leading export [TAR] |
| Mounted herding | Cold steppe, Temperate grassland, Cold desert | The steppe is the heartland of nomadic herding, where little can be ploughed; herders trade with, raid and sometimes conquer farmers [NP]. 1 to 2 persons per km² [SMIL] [partially verified] |
| Plough farming | Temperate grassland and forests | The best land, held by farmers for 6,000 years [TALL]. 25 to 500 persons per km² [SMIL] [partially verified] |
| Salmon and shore foraging | Temperate rain forest | Far denser than foragers usually are, with permanent villages and ranked societies [PNW] [HG] |
| Olive, vine, grain and moving flocks | Woodland & shrubland | The Mediterranean triad, bounded by frost and water [MEDIT]; flocks moved by season from fixed villages [TRANS] |
| Terrace farming at several heights | High land of any belt | Land held at several heights, herds above the fields [VERT]; adaptation to thin air above 2,500 m [ALT]; farming begun on its own in a tropical highland [KUK] |
| Oasis and canal gardening | Hot and Cold desert | Gravity tunnels from the foothills [QANAT]; layered palm gardens on caravan roads [OASIS]; dense on poor land where there is water [MED] |
| Camel and goat nomadism | Hot desert, Thorn scrub | Camps moved after pasture, clans under no central power, income from caravans [BED]; a few per cent of the population [NP] |
| Cattle keeping | Savanna, Thorn scrub | Herds follow the rain [PAST]; thorn-fenced cattle folds [KRAAL]; milk-drinking adults [LP] |
| Monsoon farming with stored rain | Tropical dry forest, Savanna | Chains of reservoirs make settlement and cities possible [TANK] |
| Forest gardening | Tropical rain forest | Shifting plots on leached soils, at low to middling density [SHIFT] [SLASH] [MED]; forest peoples trade with farmers [PYG] |
| Stilt, boat and reed living | Mangrove, river mouths, lakes | Stilt houses against flood [STILT]; sea nomads who barter fish for farm goods [BAJAU]; reed houses on reed islands [MARSH]; outrigger voyagers [AUST] |

Looks vary by cline: skin colour with ultraviolet light [SKIN], body proportions with temperature [ALLEN].

---

## 4. The proposed list

Thirty-six races: 20 near-human (group N) and 16 human peoples (group P). **Base** lists the habitat weights: 3 native, 2 good, 1 tolerated; a biome that is **not named is 0**. The frequency in the Parameter Set file is habitat times common (§5.4). **Environment** says where the weights come from: *source* (the sources place the race there), *judgement* (they do not, and the placing is mine), or *mixed*.

**Share aimed for (Design note).** Near-human races should be minorities, as they are in their sources. The aim was about a quarter of all persons for the 20 together when the list stands alone, with no single one above about 3 %. So `common` is 1 for the ten that have a homeland of their own and 0.5 for the ten that live scattered or hidden, against 1 to 5 for the human peoples. Measured: 25.7 % together, 0.07 % to 3.2 % each (§5.3).

**Working names (Design note).** Folklore names are used as they are. The other names are plain English compounds coined for this note, to be replaced when the user chooses. A web search found no published race called Gloamkin, Rimeborn, Hourkeepers, Wardbred or Stillminds. Two first choices, "Emberkin" and "Jotunkin", turned up in hobby settings with the same meaning and were dropped. Fellkin, Pactblood, Isle-heirs, Drakemarked, Marked folk, Brushtails, Motherline and Jinn-blooded were not searched [unverified].

### Group N. Near-human races

#### N1. Fellkin — based on: Norn (Guild Wars 2)

- **Also draws on:** goliaths [DND-FR], vrykul [WOW-VRY], Roegadyn [FF14], jötnar [JOTUNN].
- **Appearance:** human in every feature but scale: eight feet and more, broad as a door, with hands like shovels. Any colouring; hair worn long and braided with tokens of deeds.
- **Story:** the Fellkin live in halls on the high snow and hold that a life is worth the tale told of it afterwards. Each chooses a beast as patron and tries to earn its nature; the greatest are said to wear its shape. They follow no king, only whoever has the best story, and they come down to the lowlands to find deeds worth doing.
- **Base:** Tundra (3), Ice & snow (2), Boreal forest (2), Cold steppe (1). Common 1. **Height modifier:** `(0, 0.5), (300, 1)`: half weight at sea level, full weight from 300 m.
- **Environment:** source. Snowy mountains and the frozen north [GW2-NORN] [WOW-VRY]; mountains in myth [JOTUNN].

#### N2. Pactblood — based on: Tiefling (D&D, Baldur's Gate 3)

- **Also draws on:** fiendish nephilim and hungerseeds [PF2-VH], the horned girls of [ELFEN].
- **Appearance:** human with a pair of horns, curled, swept or stubbed, that differ by family; eyes of one flat colour like a coal or a coin; often a slender tail. Skin from ordinary shades to brick red or slate.
- **Story:** somewhere far back an ancestor signed something. The debt is long paid, but the mark breeds true, and Pactblood children are born to ordinary-looking lines without warning. Shunned in good times and blamed in bad ones, they keep to their own quarter of a town, look after each other and are the first to be hired for work others fear.
- **Base:** Hot desert, Thorn scrub, Savanna (2); Cold steppe, Cold desert, Temperate grassland, Woodland & shrubland, Temperate mixed forest, Temperate broadleaf forest, Tropical dry forest (1). Common 0.5.
- **Environment:** judgement. The sources give no homeland: they live among others and are distrusted [BG3-TIEF] [DND-FR]. Spread thin wherever towns are; the lean to hot, dry land is flavour.

#### N3. Nephilim — based on: Aasimar (D&D)

- **Also draws on:** the Nephilim of Genesis [NEPH], celestial nephilim [PF2-VH], valkyries [VALK], the moon people of [KAGUYA].
- **Appearance:** human, and handsome in a way that is hard to look at for long. In strong feeling the eyes brighten to a steady glow and a pale shape like wings stands behind the shoulders for a few breaths. Hair often white-gold or silver from birth.
- **Story:** the Nephilim are born of a line that something from above once loved. Each grows up hearing a quiet voice that is not their own, and most spend their lives deciding whether to obey it. Temples want them, rulers fear them, and villages on high ground claim their protection.
- **Base:** every biome except Ice & snow and Mangrove (1). Common 0.5. **Height modifier:** `(0, 0.5), (500, 1.5)`: rarer in the lowlands, more frequent on the heights.
- **Environment:** judgement. No homeland in any source [BG3-AAS] [PF2-VH]. Scattered births everywhere; the lean to high ground is flavour.

#### N4. Changeling — based on: Changeling (D&D), the changeling of folklore

- **Also draws on:** hag-born changelings and reflections [PF2-VH], fox wives [KITSUNE], fae souls in human bodies [WOD-CTD].
- **Appearance:** in their own face, pale and unfinished-looking, with features a little too smooth and eyes without much colour. Few ever see that face: a Changeling can wear any human face it has studied.
- **Story:** the old tale says the hidden folk left them in cradles. The Changelings say they were always here, and that a people who can be anyone learns to keep a true name for use only at home. They live in the largest towns, a few to a street, and their own law has one rule above all: never wear the face of the living to do harm.
- **Base:** Temperate grassland, Woodland & shrubland, Temperate mixed forest, Temperate broadleaf forest (2); Cold steppe, Boreal forest, Temperate rain forest, Hot desert, Thorn scrub, Savanna, Tropical dry forest, Tropical rain forest, Mangrove (1). Common 0.5.
- **Environment:** judgement. The sources put them among humans [CHG-FOLK] [EBERRON]. Most where the human peoples are most numerous.

#### N5. Rimeborn — based on: Chiss (Star Wars)

- **Also draws on:** Andorians [ANDOR], samsarans [PF2-ANC], Sebaceans [FARSCAPE].
- **Appearance:** human with skin from pale blue to deep slate blue, hair blue-black or white, and eyes that catch the light red, like an animal's at night. They sicken in heat.
- **Story:** the Rimeborn build downward. Their cities lie under the ice where the rock is warm, ruled by a council of old families that trusts no outsider and forgets no slight. They are patient, exact and proud, and an officer who acts without leave is called home, however right he was.
- **Base:** Ice & snow (3), Tundra (2), Boreal forest (1), Cold desert (1). Common 1.
- **Environment:** mixed. An ice moon with underground cities is sourced for one of the peoples drawn on [ANDOR]; for the Chiss the ice world is general knowledge [unverified].

#### N6. Marked folk — based on: Mirialan (Star Wars)

- **Also draws on:** Bajorans [BAJOR], the joined Trill [TRILL], the rune of [WOW-DRA].
- **Appearance:** human with skin of a yellow-olive to green cast and dark geometric marks on cheeks, brow and chin. The marks are added through life, one for each vow kept or trial passed, so an elder's face can be read like a page.
- **Story:** the Marked folk hold that what a person has done should be plain to see. A child's face is bare; the first mark is given at the first vow, and a broken vow is marked too. They are a devout, exact people of the dry uplands, valued as judges, witnesses and healers because they cannot hide their record.
- **Base:** Cold steppe, Cold desert (2); Temperate grassland, Woodland & shrubland (1). Common 1.
- **Environment:** judgement. The Databank gives only the looks [SW-MIR]; the cold, dry home world is general knowledge [unverified].

#### N7. Gloamkin — based on: Awoken (Destiny)

- **Also draws on:** void elves [WOW-VOID], fetchlings [PF2-ANC], the moon people of [KAGUYA].
- **Appearance:** human with skin of a pale dusk colour, blue-grey, lilac or grey-green, and eyes that give off a faint light of their own. A soft shimmer moves under the skin when they are roused.
- **Story:** their ancestors were ordinary people who went into the dark between places and stayed too long. What came back did not age as it had and did not quite belong. The Gloamkin are split to this day: those who keep to their hidden courts, those who walked back to live among humankind, and the queens' watchers who go between.
- **Base:** Tundra, Temperate rain forest (2); Boreal forest, Cold desert (1). Common 0.5.
- **Environment:** judgement. The source places them on an asteroid reef, in a hidden city and on Earth [DESTINY]. Lands of long twilight and mist are my stand-in.

#### N8. Fae — based on: the Fae of folklore (the Sidhe)

- **Also draws on:** sylvari [GW2-SYL], fadrim [PF2-VH], the long-lived elves of [FRIEREN].
- **Appearance:** human, and lovelier than is comfortable: features too even, eyes tilted, a stillness no mortal holds. They do not seem to age.
- **Story:** the Fae live under the old mounds and behind certain trees, in halls larger than the hill that holds them. Country people call them the Good Neighbours, leave a share of the harvest at the mound and never cut the thorn that stands alone. A bargain with them is kept to the letter, by both sides, whatever the letter turns out to mean.
- **Base:** Temperate broadleaf forest, Temperate rain forest (3); Temperate mixed forest (2); Boreal forest, Temperate grassland, Woodland & shrubland (1). Common 0.5.
- **Environment:** source. Mounds in farmed country [AOS], the forest [HULDER], a grove [GW2-SYL].

#### N9. Hourkeepers — based on: Time Lords (Doctor Who)

- **Also draws on:** the Ancients [SG-ANC], the joined Trill [TRILL], samsarans [PF2-ANC], Culture citizens [CULTURE].
- **Appearance:** human. A healer would find a second heartbeat. At the point of death an Hourkeeper burns through a fever and rises with a new face, a new temper and every memory of the old one; this happens a counted number of times and no more.
- **Story:** the Hourkeepers live in one or two high citadels and keep the true reckoning of years, past and to come. Their law forbids them to change what they record, and most obey it and grow old many times over their ledgers. Now and then one cannot bear to watch any longer, takes a staff and walks out to meddle, and the citadel strikes that name from its rolls.
- **Base:** Tundra, Cold steppe, Cold desert, Temperate grassland, Woodland & shrubland (1). Common 0.5. **Height modifier:** `(200, 0), (500, 1)`: none below 200 m, full weight from 500 m.
- **Environment:** judgement. The source has a home world with one citadel [TL]. High, remote, open country is my stand-in. The rarest race of the list (0.07 %).

#### N10. Isle-heirs — based on: Númenóreans (Tolkien)

- **Also draws on:** Valyrians [TARG], azarketi [PF2-ANC], Elezen [FF14].
- **Appearance:** human, a head taller than most, long-boned and slow to age: an Isle-heir of ninety looks forty. Grey or sea-green eyes; hair dark or, in some houses, silver-fair from childhood.
- **Story:** their island was the greatest realm of the world until the sea took it in a night. The ships that were at sea that night carried the Isle-heirs to every temperate shore, where they raised stone harbours and made themselves lords over shorter-lived folk. Each generation lives a little less long than the last, and they know it.
- **Base:** Woodland & shrubland, Temperate broadleaf forest (2); Temperate grassland, Temperate mixed forest, Temperate rain forest (1). Common 1.
- **Environment:** mixed. An island and then kingdoms in exile on temperate coasts [NUMENOR]; the coast itself cannot be expressed (§6.4 item 3).

#### N11. Drakemarked — based on: Au Ra (Final Fantasy XIV)

- **Also draws on:** dragonblood [PF2-VH], Valyrian dragonlords [TARG], Qunari horns [QUNARI].
- **Appearance:** human with a crest of horn sweeping back from the temples, patches of fine scale at the brow, neck and forearms, and pupils that narrow to slits in bright light.
- **Story:** the Drakemarked say a dragon once took human shape for love, and that they are what came of it. They live in clans on the open dry country, keep their dead in high caves and count descent through whichever parent has the finer horns. A clan with no feud is thought to be hiding something.
- **Base:** Cold steppe (2); Cold desert, Hot desert, Thorn scrub, Savanna (1). Common 0.5.
- **Environment:** judgement. The source read gives the looks only [FF14]; steppe clans are general knowledge [unverified].

#### N12. Selkie — based on: the selkie of folklore

- **Also draws on:** finfolk [SELKIE], undines [PF2-VH], real sea nomads [BAJAU].
- **Appearance:** human with large dark eyes, sleek dark hair and a web of skin between the fingers. Each keeps a seal-grey cloak from birth and is never seen to lend it.
- **Story:** the Selkies live in low stone houses on skerries and headlands and are at home in water too cold for anyone else. They fish, salvage and pilot, and marry inland more often than their elders like. A Selkie whose cloak is taken cannot go back to the sea, and the old songs are mostly about that.
- **Base:** Boreal forest, Temperate rain forest (2); Ice & snow, Tundra, Woodland & shrubland, Temperate mixed forest, Mangrove (1). Common 1. **Height modifier:** `(20, 1), (80, 0)`: full weight below 20 m, none from 80 m.
- **Environment:** source for the cold northern coasts [SELKIE]; the height modifier is a stand-in for "at the shore" (§6.4 item 3).

#### N13. Jinn-blooded — based on: genasi (D&D), the jinn of tradition

- **Also draws on:** geniekin [PF2-VH], the godlike (`races.md` [POE-SR]), element bearers [GENSHIN].
- **Appearance:** human with one sign of an element: hair that stirs like flame or smoke, skin with the grain of sandstone, eyes like deep water, a voice with wind in it. No two families show it alike.
- **Story:** the jinn are a people of smokeless fire who live unseen in the empty places, and the Jinn-blooded are their kin by marriage. They hold the ruined cities of the desert that others fear to enter, trade in what they find there and can call on a cousin among the unseen once in a lifetime, at a price.
- **Base:** Hot desert (3); Cold desert, Thorn scrub (2); Savanna (1). Common 1.
- **Environment:** source. Deserts and abandoned places [JINN].

#### N14. Brushtails — based on: Miqo'te (Final Fantasy XIV)

- **Also draws on:** kemonomimi [KEMONO], Viera [FF14], kitsune [KITSUNE], beastkin [PF2-VH].
- **Appearance:** human with tall furred ears set high on the head and a long furred tail, both as expressive as a face. Quick, light on their feet, and able to hear a footstep through a wall.
- **Story:** the Brushtails live in bands of a few families, some by day in open country and some by night under trees, and the two kinds claim to pity each other. They are hunters, runners and scouts, restless under a roof, and their word for a liar is someone who has learned to keep the tail still.
- **Base:** Savanna, Tropical dry forest (2); Woodland & shrubland, Temperate mixed forest, Temperate broadleaf forest, Tropical rain forest (1). Common 1.
- **Environment:** judgement. The source read gives the looks only [FF14]; warm open country and woodland are my choice.

#### N15. Dhampir — based on: the dhampir of folklore

- **Also draws on:** Pathfinder's dhampirs and duskwalkers [PF2-VH], the hidden society of [VTM], the white-haired guardians of [ZELDA].
- **Appearance:** human, pale as candle wax whatever the parents' colouring, with hair that whitens young and eyes that reflect in the dark. They age slowly and scar hardly at all.
- **Story:** a Dhampir is born when the dead father a child on the living. The gift that comes with it, to know the unquiet dead by smell, runs down the family, so there are whole lineages now. Villages that fear them still send for them, pay them and ask them to be gone by morning.
- **Base:** Boreal forest, Temperate rain forest (2); Tundra, Temperate mixed forest (1). Common 0.5.
- **Environment:** judgement. The folklore places them in ordinary villages [DHAMPIR]. Dark, wet, long-night country is flavour.

#### N16. Huldrefolk — based on: the hulder of folklore

- **Also draws on:** sylvari [GW2-SYL], ardande [PF2-VH], tengu of the mountain forest [TENGU].
- **Appearance:** human from the front. Bark-patterned skin runs down the spine, the hair has the green-gold of new leaves, and each carries a tail, cow-tufted or fox-brushed, kept under the clothes among strangers.
- **Story:** the Huldrefolk are the people of the deep forest, born, they say, of the trees. They herd their own small cattle in clearings no one else can find and are good neighbours to those who ask before they cut. Charcoal burners leave them a bowl by the kiln, and wake to find the fire tended.
- **Base:** Boreal forest (3); Temperate mixed forest, Temperate broadleaf forest (2); Temperate rain forest (1). Common 1.
- **Environment:** source. The forest, beside charcoal burners [HULDER].

#### N17. Motherline — based on: Gerudo (The Legend of Zelda)

- **Also draws on:** asari [ASARI], the matriarchal northern giants of [WOW-VRY].
- **Appearance:** human, tall and long-limbed, bronze-skinned, with red or white hair and eyes of gold or green. Nearly all are women.
- **Story:** among the Motherline a son is born perhaps once in a generation, and is either made king or sent away, by turns of custom. Daughters take partners from other peoples and bring the children home. They live long, in three ages: a wandering youth abroad, a middle life building the house, and an old age in council.
- **Base:** Hot desert, Thorn scrub (2); Cold desert, Savanna (1). Common 0.5.
- **Environment:** source. A desert named after the people [ZELDA].

#### N18. Stillminds — based on: Vulcans (Star Trek)

- **Also draws on:** kalashtar [EBERRON], the Bene Gesserit [BENE], Betazoids [TRILL], Minbari castes [B5].
- **Appearance:** human with upswept brows and ears that rise to a point, spare of flesh and stronger than they look. Their faces give nothing away.
- **Story:** the Stillminds were once the most violent people of the dry lands and chose, as one people, to stop. Each child is trained from the age of five to master every feeling, and an adult can share thoughts by touch and slow the heart at will. They live in cliff monasteries, live long, and are sought as arbiters by rulers who trust nobody else.
- **Base:** Cold desert, Hot desert (2); Woodland & shrubland, Thorn scrub (1). Common 0.5.
- **Environment:** source. A hot, dry home world [VULCAN]; mountain temple-keeps [EBERRON].

#### N19. Wardbred — based on: witchers (Sapkowski)

- **Also draws on:** Space Marines [WH40K], the remade humans of [SHADOWRUN] and [CULTURE], the hidden guardians of [ZELDA].
- **Appearance:** human, big-framed, with eyes whose pupils close to slits by day and shine by night, and hair that goes grey-white in the twenties. They heal fast and feel poison late.
- **Story:** an old realm remade its frontier soldiers by craft that is now lost, and the change bred true. The realm is gone; the Wardbred still garrison its border forts, a few hundred to a keep, and still hunt what comes over the wall. They have few children, take in foundlings, and are paid by the kingdoms behind them less often than they are thanked.
- **Base:** Tundra, Cold steppe, Boreal forest, Woodland & shrubland, Temperate mixed forest (1). Common 0.5.
- **Environment:** judgement. The sources give a keep and recruiting grounds, not a climate [WITCHER] [WH40K]. A thin line along cold and wooded marches is my choice.

#### N20. Yaksha — based on: the yaksha of tradition

- **Also draws on:** tengu [TENGU], yaksha as a playable ancestry [PF2-ANC].
- **Appearance:** human, thick-set and strong, with a broad calm face, heavy brows and a liking for gold ornament. A Yaksha is always found near the same tree, well, gate or ford.
- **Story:** each Yaksha is bound to one place and keeps it: a spring that must not be fouled, a banyan that must not be cut, a hoard that must not be found. Treated with respect they are generous hosts and tireless builders. Wronged, they are the reason travellers in the wet forest do not arrive.
- **Base:** Tropical dry forest, Tropical rain forest (2); Savanna, Mangrove (1). Common 0.5.
- **Environment:** source. Woods and wilds, water and trees [YAKSHA].

### Group P. Human peoples

Plain humans, kept from the first pass. Build follows the land (compact in the cold, long-limbed in the heat [ALLEN]); colouring follows the sun loosely [SKIN]; every people has a range of looks, not a type, and none copies the real peoples who share its way of life. Sources for each way of life are in §3.

| # | Race (id) | Way of life and story | Appearance | Base (habitat), common, height | Grounding |
|---|---|---|---|---|---|
| P1 | Floe hunters (`floeHunters`) | Follow seal, walrus and whale along the ice edge; count standing in dogs and boats; carve ivory | Compact, deep-chested; tan to brown skin; hair black to rust | Ice & snow (3), Tundra (2). Common 1 | [INUIT] |
| P2 | Snow herders (`snowHerders`) | Reindeer nomads between summer tundra and winter forest; the carriers of the north | Short, sturdy; fair to light brown; hair flaxen to black | Tundra (3), Boreal forest (2), Cold steppe (1). Common 2 | [REIN] [NP] |
| P3 | Tarburners (`tarburners`) | Burn a clearing, sow rye, move on; trap, fish and cook pine into tar; no lords | Big-framed; fair to ruddy; pale eyes common | Boreal forest (3), Temperate mixed forest (2). Common 3 | [SLASH] [TAR] |
| P4 | Grass riders (`grassRiders`) | Mounted herders in felt tents; sell horses to the farmers in good years and take grain in bad ones | Lean, wiry; ruddy to bronze; hair black to sandy red | Cold steppe (3), Temperate grassland (2), Cold desert (1). Common 3 | [NP] [LP] |
| P5 | Ploughlanders (`ploughlanders`) | Grain farmers with villages, towns and kings; land unploughed is land unfinished | The most varied; any colouring | Temperate grassland (3), Temperate broadleaf forest (3), Woodland & shrubland (2), Temperate mixed forest (2), Cold steppe (1), Temperate rain forest (1). Common 5 | [TALL] [SMIL] [partially verified] |
| P6 | Holt folk (`holtFolk`) | Woodland villages with a common wood; charcoal, iron and bows; never clear a whole forest | Rangy; fair to light brown; brown to red hair | Temperate broadleaf forest (3), Temperate mixed forest (3), Temperate rain forest (2), Woodland & shrubland (1). Common 3 | [MB2-W] [CK3-T]; the forest trades are general knowledge [unverified] |
| P7 | Rainwood folk (`rainwoodFolk`) | Salmon fishers in great plank halls; rank is won by giving feasts | Stocky, strong-shouldered; pale to light brown; dark hair | Temperate rain forest (3), Boreal forest (1). Common 3 | [PNW] [HG] |
| P8 | Grove folk (`groveFolk`) | Walled hill towns on grain, olive and vine; flocks go up the mountain in summer; leagues of towns | Slight; olive to brown; dark curly hair, some fair | Woodland & shrubland (3), Temperate grassland (1), Temperate broadleaf forest (1). Common 4 | [MEDIT] [TRANS] |
| P9 | Terrace folk (`terraceFolk`) | Stepped fields at several heights, herds above; walls and channels kept up by the whole village | Short, broad, deep-chested; ruddy to dark brown | Woodland & shrubland (3); Tundra, Cold steppe, Cold desert, Temperate grassland, Temperate mixed forest (2); Temperate broadleaf forest, Savanna, Tropical dry forest, Tropical rain forest (1). Common 4. Height `(150, 0), (450, 1)` | [VERT] [ALT] [KUK] |
| P10 | Wellkeepers (`wellkeepers`) | Garden towns fed by tunnels from the hills; water measured by the hour; whoever holds the wells holds the road | Slender; light to dark brown | Hot desert (3), Cold desert (3). Common 2 | [QANAT] [OASIS] [MED] |
| P11 | Dunefarers (`dunefarers`) | Camel nomads who know every well; guide, tax or rob the caravans | Lean, long-limbed; bronze to dark brown | Hot desert (3), Cold desert (2), Thorn scrub (2). Common 1 | [BED] [NP] |
| P12 | Thornfold folk (`thornfoldFolk`) | Cattle are bride-price, blood-price and poetry; homesteads ring a fold of cut thorn | Tall, slender; dark brown to nearly black | Savanna (3), Thorn scrub (3), Tropical dry forest (1), Hot desert (1). Common 2 | [PAST] [KRAAL] [LP] |
| P13 | Rainkeepers (`rainkeepers`) | Catch the monsoon in chains of reservoirs; kings are judged by their dams | Slight; brown to dark brown; black hair | Tropical dry forest (3), Savanna (2), Tropical rain forest (1), Thorn scrub (1). Common 4 | [TANK] |
| P14 | Clearing folk (`clearingFolk`) | Cut a garden, plant it three years, let the forest take it back; one longhouse, one community | Short, light-built; many browns | Tropical rain forest (3), Tropical dry forest (2), Mangrove (1). Common 2 | [SHIFT] [SLASH] [PYG] |
| P15 | Stilt folk (`stiltFolk`) | Villages on posts in the shallows; fishers, divers and boat builders who own no fields | Lean swimmers; sun-darkened brown | Mangrove (3), Tropical rain forest (1), Tropical dry forest (1). Common 2 | [STILT] [BAJAU] [MARSH] [AUST] |
| P16 | Longshore folk (`longshoreFolk`) | Cool-coast fishers, traders and raiders; a ship is owned in shares and a captain voted in | Raw-boned; any colouring, crews take anyone | Boreal forest, Woodland & shrubland, Temperate rain forest (2); Ice & snow, Tundra, Temperate grassland, Temperate mixed forest, Temperate broadleaf forest, Mangrove (1). Common 2. Height `(30, 1), (120, 0)` | [ASOIAF] [EARTHSEA] [AUST] |

---

## 5. Tables

### 5.1 The list in the format of Appendix E

```
frequency(race, biome) = habitat(race, biome) · common(race)
```

The habitat weights are 0 avoided, 1 tolerated, 2 good and 3 native. Columns: Ice & snow, Tundra, Cold steppe, Boreal forest, Cold desert, Temperate grassland, Woodland & shrubland, Temperate mixed forest, Temperate broadleaf forest, Temperate rain forest, Hot desert, Thorn scrub, Savanna, Tropical dry forest, Tropical rain forest, Mangrove. "Based on" is the source race the working name stands for; it is not part of the Parameter Set.

| Id | Name | Based on | On | Common | Ice | Tun | CSt | Bor | CDs | TGr | WSh | TMx | TBl | TRn | HDs | Thn | Sav | TDf | TRf | Mgv |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| `fellkin` | Fellkin | Norn (Guild Wars 2) | yes | 1 | 2 | 3 | 1 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| `pactblood` | Pactblood | Tiefling (D&D, Baldur's Gate 3) | yes | 0.5 | 0 | 0 | 1 | 0 | 1 | 1 | 1 | 1 | 1 | 0 | 2 | 2 | 2 | 1 | 0 | 0 |
| `nephilim` | Nephilim | Aasimar (D&D) | yes | 0.5 | 0 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 0 |
| `changeling` | Changeling | Changeling (D&D; folklore) | yes | 0.5 | 0 | 0 | 1 | 1 | 0 | 2 | 2 | 2 | 2 | 1 | 1 | 1 | 1 | 1 | 1 | 1 |
| `rimeborn` | Rimeborn | Chiss (Star Wars) | yes | 1 | 3 | 2 | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| `markedFolk` | Marked folk | Mirialan (Star Wars) | yes | 1 | 0 | 0 | 2 | 0 | 2 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| `gloamkin` | Gloamkin | Awoken (Destiny) | yes | 0.5 | 0 | 2 | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 |
| `fae` | Fae | the Fae (folklore) | yes | 0.5 | 0 | 0 | 0 | 1 | 0 | 1 | 1 | 2 | 3 | 3 | 0 | 0 | 0 | 0 | 0 | 0 |
| `hourkeepers` | Hourkeepers | Time Lords (Doctor Who) | yes | 0.5 | 0 | 1 | 1 | 0 | 1 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| `isleHeirs` | Isle-heirs | Númenóreans (Tolkien) | yes | 1 | 0 | 0 | 0 | 0 | 0 | 1 | 2 | 1 | 2 | 1 | 0 | 0 | 0 | 0 | 0 | 0 |
| `drakemarked` | Drakemarked | Au Ra (Final Fantasy XIV) | yes | 0.5 | 0 | 0 | 2 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 1 | 1 | 1 | 0 | 0 | 0 |
| `selkie` | Selkie | selkie (folklore) | yes | 1 | 1 | 1 | 0 | 2 | 0 | 0 | 1 | 1 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 1 |
| `jinnBlooded` | Jinn-blooded | genasi (D&D); jinn (tradition) | yes | 1 | 0 | 0 | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 3 | 2 | 1 | 0 | 0 | 0 |
| `brushtails` | Brushtails | Miqo'te (Final Fantasy XIV) | yes | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 1 | 1 | 1 | 0 | 0 | 0 | 2 | 2 | 1 | 0 |
| `dhampir` | Dhampir | dhampir (folklore) | yes | 0.5 | 0 | 1 | 0 | 2 | 0 | 0 | 0 | 1 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 |
| `huldrefolk` | Huldrefolk | hulder (folklore) | yes | 1 | 0 | 0 | 0 | 3 | 0 | 0 | 0 | 2 | 2 | 1 | 0 | 0 | 0 | 0 | 0 | 0 |
| `motherline` | Motherline | Gerudo (The Legend of Zelda) | yes | 0.5 | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 2 | 2 | 1 | 0 | 0 | 0 |
| `stillminds` | Stillminds | Vulcans (Star Trek) | yes | 0.5 | 0 | 0 | 0 | 0 | 2 | 0 | 1 | 0 | 0 | 0 | 2 | 1 | 0 | 0 | 0 | 0 |
| `wardbred` | Wardbred | witchers (Sapkowski) | yes | 0.5 | 0 | 1 | 1 | 1 | 0 | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| `yaksha` | Yaksha | yaksha (tradition) | yes | 0.5 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 1 | 2 | 2 | 1 |
| `floeHunters` | Floe hunters | | yes | 1 | 3 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| `snowHerders` | Snow herders | | yes | 2 | 0 | 3 | 1 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| `tarburners` | Tarburners | | yes | 3 | 0 | 0 | 0 | 3 | 0 | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| `grassRiders` | Grass riders | | yes | 3 | 0 | 0 | 3 | 0 | 1 | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| `ploughlanders` | Ploughlanders | | yes | 5 | 0 | 0 | 1 | 0 | 0 | 3 | 2 | 2 | 3 | 1 | 0 | 0 | 0 | 0 | 0 | 0 |
| `holtFolk` | Holt folk | | yes | 3 | 0 | 0 | 0 | 0 | 0 | 0 | 1 | 3 | 3 | 2 | 0 | 0 | 0 | 0 | 0 | 0 |
| `rainwoodFolk` | Rainwood folk | | yes | 3 | 0 | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 3 | 0 | 0 | 0 | 0 | 0 | 0 |
| `groveFolk` | Grove folk | | yes | 4 | 0 | 0 | 0 | 0 | 0 | 1 | 3 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| `terraceFolk` | Terrace folk | | yes | 4 | 0 | 2 | 2 | 0 | 2 | 2 | 3 | 2 | 1 | 0 | 0 | 0 | 1 | 1 | 1 | 0 |
| `wellkeepers` | Wellkeepers | | yes | 2 | 0 | 0 | 0 | 0 | 3 | 0 | 0 | 0 | 0 | 0 | 3 | 0 | 0 | 0 | 0 | 0 |
| `dunefarers` | Dunefarers | | yes | 1 | 0 | 0 | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 3 | 2 | 0 | 0 | 0 | 0 |
| `thornfoldFolk` | Thornfold folk | | yes | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 1 | 3 | 3 | 1 | 0 | 0 |
| `rainkeepers` | Rainkeepers | | yes | 4 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 1 | 2 | 3 | 1 | 0 |
| `clearingFolk` | Clearing folk | | yes | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 2 | 3 | 1 |
| `stiltFolk` | Stilt folk | | yes | 2 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 1 | 1 | 3 |
| `longshoreFolk` | Longshore folk | | yes | 2 | 1 | 1 | 0 | 2 | 0 | 1 | 2 | 1 | 1 | 2 | 0 | 0 | 0 | 0 | 0 | 1 |

**Height modifiers.** Fellkin `(0, 0.5), (300, 1)`. Nephilim `(0, 0.5), (500, 1.5)`. Hourkeepers `(200, 0), (500, 1)`. Selkie `(20, 1), (80, 0)`. Terrace folk `(150, 0), (450, 1)`. Longshore folk `(30, 1), (120, 0)`. The other 30 races have none.

### 5.2 Coverage: biomes against races

The cells are frequencies (habitat times common). **(h)** marks a race with a height modifier: it counts fully only on high land (Fellkin, Nephilim, Hourkeepers, Terrace folk) or on low land (Selkie, Longshore folk).

| Race | Ice | Tun | CSt | Bor | CDs | TGr | WSh | TMx | TBl | TRn | HDs | Thn | Sav | TDf | TRf | Mgv |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| Fellkin (h) | 2 | 3 | 1 | 2 |  |  |  |  |  |  |  |  |  |  |  |  |
| Pactblood |  |  | 0.5 |  | 0.5 | 0.5 | 0.5 | 0.5 | 0.5 |  | 1 | 1 | 1 | 0.5 |  |  |
| Nephilim (h) |  | 0.5 | 0.5 | 0.5 | 0.5 | 0.5 | 0.5 | 0.5 | 0.5 | 0.5 | 0.5 | 0.5 | 0.5 | 0.5 | 0.5 |  |
| Changeling |  |  | 0.5 | 0.5 |  | 1 | 1 | 1 | 1 | 0.5 | 0.5 | 0.5 | 0.5 | 0.5 | 0.5 | 0.5 |
| Rimeborn | 3 | 2 |  | 1 | 1 |  |  |  |  |  |  |  |  |  |  |  |
| Marked folk |  |  | 2 |  | 2 | 1 | 1 |  |  |  |  |  |  |  |  |  |
| Gloamkin |  | 1 |  | 0.5 | 0.5 |  |  |  |  | 1 |  |  |  |  |  |  |
| Fae |  |  |  | 0.5 |  | 0.5 | 0.5 | 1 | 1.5 | 1.5 |  |  |  |  |  |  |
| Hourkeepers (h) |  | 0.5 | 0.5 |  | 0.5 | 0.5 | 0.5 |  |  |  |  |  |  |  |  |  |
| Isle-heirs |  |  |  |  |  | 1 | 2 | 1 | 2 | 1 |  |  |  |  |  |  |
| Drakemarked |  |  | 1 |  | 0.5 |  |  |  |  |  | 0.5 | 0.5 | 0.5 |  |  |  |
| Selkie (h) | 1 | 1 |  | 2 |  |  | 1 | 1 |  | 2 |  |  |  |  |  | 1 |
| Jinn-blooded |  |  |  |  | 2 |  |  |  |  |  | 3 | 2 | 1 |  |  |  |
| Brushtails |  |  |  |  |  |  | 1 | 1 | 1 |  |  |  | 2 | 2 | 1 |  |
| Dhampir |  | 0.5 |  | 1 |  |  |  | 0.5 |  | 1 |  |  |  |  |  |  |
| Huldrefolk |  |  |  | 3 |  |  |  | 2 | 2 | 1 |  |  |  |  |  |  |
| Motherline |  |  |  |  | 0.5 |  |  |  |  |  | 1 | 1 | 0.5 |  |  |  |
| Stillminds |  |  |  |  | 1 |  | 0.5 |  |  |  | 1 | 0.5 |  |  |  |  |
| Wardbred |  | 0.5 | 0.5 | 0.5 |  |  | 0.5 | 0.5 |  |  |  |  |  |  |  |  |
| Yaksha |  |  |  |  |  |  |  |  |  |  |  |  | 0.5 | 1 | 1 | 0.5 |
| Floe hunters | 3 | 2 |  |  |  |  |  |  |  |  |  |  |  |  |  |  |
| Snow herders |  | 6 | 2 | 4 |  |  |  |  |  |  |  |  |  |  |  |  |
| Tarburners |  |  |  | 9 |  |  |  | 6 |  |  |  |  |  |  |  |  |
| Grass riders |  |  | 9 |  | 3 | 6 |  |  |  |  |  |  |  |  |  |  |
| Ploughlanders |  |  | 5 |  |  | 15 | 10 | 10 | 15 | 5 |  |  |  |  |  |  |
| Holt folk |  |  |  |  |  |  | 3 | 9 | 9 | 6 |  |  |  |  |  |  |
| Rainwood folk |  |  |  | 3 |  |  |  |  |  | 9 |  |  |  |  |  |  |
| Grove folk |  |  |  |  |  | 4 | 12 |  | 4 |  |  |  |  |  |  |  |
| Terrace folk (h) |  | 8 | 8 |  | 8 | 8 | 12 | 8 | 4 |  |  |  | 4 | 4 | 4 |  |
| Wellkeepers |  |  |  |  | 6 |  |  |  |  |  | 6 |  |  |  |  |  |
| Dunefarers |  |  |  |  | 2 |  |  |  |  |  | 3 | 2 |  |  |  |  |
| Thornfold folk |  |  |  |  |  |  |  |  |  |  | 2 | 6 | 6 | 2 |  |  |
| Rainkeepers |  |  |  |  |  |  |  |  |  |  |  | 4 | 8 | 12 | 4 |  |
| Clearing folk |  |  |  |  |  |  |  |  |  |  |  |  |  | 4 | 6 | 2 |
| Stilt folk |  |  |  |  |  |  |  |  |  |  |  |  |  | 2 | 2 | 6 |
| Longshore folk (h) | 2 | 2 |  | 4 |  | 2 | 4 | 2 | 2 | 4 |  |  |  |  |  | 2 |
| **Near-human races in the biome** | 3 | 8 | 8 | 10 | 10 | 7 | 11 | 10 | 7 | 8 | 7 | 7 | 8 | 5 | 4 | 3 |
| **Human peoples in the biome** | 2 | 4 | 4 | 4 | 4 | 5 | 5 | 5 | 5 | 4 | 3 | 3 | 3 | 5 | 4 | 3 |
| **Sum, near-human** | 6 | 9 | 6.5 | 11.5 | 9 | 5 | 9 | 9 | 8.5 | 8.5 | 7.5 | 6 | 6.5 | 4.5 | 3 | 2 |
| **Sum, human peoples** | 5 | 18 | 24 | 20 | 19 | 35 | 41 | 35 | 34 | 24 | 11 | 12 | 18 | 24 | 16 | 10 |

- **No biome is empty.** Every biome has at least three near-human races and two human peoples. Ice & snow and Mangrove have the fewest races (five and six).
- **The temperate and dry biomes are crowded on paper.** Woodland & shrubland lists 16 races, Cold desert, Boreal forest and Temperate mixed forest 14 or 15. Most of those are near-human races at a frequency of 0.5 or 1 against a human sum of 19 to 41, so each holds a few per cent of the biome at most. Whether that reads as rich or as noise is a question for the user (§6.4 item 4).
- **Near-human races are a minority in every biome but one.** Their summed frequency is a seventh to two thirds of the human sum everywhere except Ice & snow, where it is larger (6 against 5): the ice belongs to the Rimeborn, the Fellkin and the Floe hunters together.
- **Human peoples:** as in the first pass. Each biome has two to five; twelve of the 16 lead in at least one biome.

### 5.3 Measured shares

Measured on 50 regions: the 25 regions around region (0, 0) of the seeds `cellgen` and `gameworld`, taken to the Planned stage with the default Parameter Set and only the `Races` list changed. These are the regions `map-generation.md` §19.3 and `adding-a-race.md` measure. Shares are of all persons. Three lists were run:

- **Alone**: the 36 races are the whole list.
- **Beside five**: the 36 in place of `human`, beside the default elf, dwarf, halfling, orc and goblin.
- **Beside six**: the 36 beside all six default races that are on, `human` included.

| Race | Alone | Beside five | Beside six | Regions with the race, of 50 |
|---|---|---|---|---|
| Brushtails | 3.17 % | 2.32 % | 1.44 % | 50 |
| Jinn-blooded | 2.71 % | 1.98 % | 1.38 % | 50 |
| Changeling | 2.42 % | 1.76 % | 1.11 % | 50 |
| Pactblood | 2.16 % | 1.56 % | 1.02 % | 50 |
| Huldrefolk | 2.04 % | 1.44 % | 0.92 % | 50 |
| Isle-heirs | 1.93 % | 1.36 % | 0.86 % | 50 |
| Nephilim | 1.60 % | 1.16 % | 0.76 % | 50 |
| Fae | 1.39 % | 0.99 % | 0.63 % | 50 |
| Yaksha | 1.10 % | 0.85 % | 0.52 % | 48 |
| Motherline | 1.08 % | 0.78 % | 0.54 % | 50 |
| Marked folk | 1.04 % | 0.75 % | 0.50 % | 50 |
| Stillminds | 0.91 % | 0.67 % | 0.47 % | 50 |
| Drakemarked | 0.79 % | 0.57 % | 0.38 % | 50 |
| Rimeborn | 0.75 % | 0.57 % | 0.43 % | 50 |
| Fellkin | 0.70 % | 0.51 % | 0.36 % | 47 |
| Dhampir | 0.59 % | 0.42 % | 0.27 % | 49 |
| Gloamkin | 0.50 % | 0.36 % | 0.25 % | 50 |
| Wardbred | 0.50 % | 0.35 % | 0.22 % | 50 |
| Selkie | 0.25 % | 0.20 % | 0.13 % | 48 |
| Hourkeepers | 0.07 % | 0.05 % | 0.03 % | 50 |
| **The 20 near-human races** | **25.7 %** | **18.6 %** | **12.2 %** | |
| Ploughlanders | 14.47 % | 10.14 % | 6.41 % | 50 |
| Rainkeepers | 12.59 % | 9.33 % | 5.85 % | 49 |
| Holt folk | 7.50 % | 5.33 % | 3.39 % | 50 |
| Thornfold folk | 7.00 % | 5.06 % | 3.29 % | 49 |
| Grove folk | 5.93 % | 4.13 % | 2.62 % | 50 |
| Clearing folk | 3.87 % | 2.99 % | 1.85 % | 46 |
| Tarburners | 3.62 % | 2.50 % | 1.60 % | 48 |
| Terrace folk | 3.35 % | 2.48 % | 1.63 % | 50 |
| Wellkeepers | 3.29 % | 2.49 % | 1.78 % | 47 |
| Stilt folk | 2.89 % | 2.52 % | 1.40 % | 46 |
| Rainwood folk | 2.33 % | 1.67 % | 1.10 % | 48 |
| Dunefarers | 2.22 % | 1.62 % | 1.16 % | 49 |
| Grass riders | 2.16 % | 1.51 % | 0.98 % | 49 |
| Snow herders | 1.60 % | 1.14 % | 0.77 % | 47 |
| Longshore folk | 1.16 % | 0.86 % | 0.55 % | 49 |
| Floe hunters | 0.32 % | 0.26 % | 0.22 % | 43 |
| **The 16 human peoples** | **74.3 %** | **54.1 %** | **34.8 %** | |
| `human` (default) | | | 35.47 % | |
| `elf` | | 3.83 % | 2.47 % | |
| `dwarf` | | 1.31 % | 0.87 % | |
| `halfling` | | 6.17 % | 3.89 % | |
| `orc` | | 8.15 % | 5.34 % | |
| `goblin` | | 7.87 % | 5.14 % | |

- **Near-human share.** A quarter of all persons when the list stands alone, as aimed for. In one region it runs from 22 % to 32 %.
- **Beside the default races.** With the default list alone, `human` has 66.6 % in the same regions, elves 4.5 %, dwarves 1.8 %, halflings 7.1 %, orcs 10.3 % and goblins 9.8 %. Adding the 20 near-human races takes about a fifth from each of them ("Beside five"). Keeping `human` on as well halves everything ("Beside six"); that list has the default `human` and the 16 human peoples side by side and is not recommended.
- **Total.** The 50 regions hold 1,824,333 persons with the 36 alone, against 1,820,603 with the default list: 0.2 % more, because Ice & snow below 200 m now has races. The median region holds about 35,160 in every run.
- **No homelands.** With the 36 alone, the median region has 23 races at 1 % or more of its persons (range 15 to 28) and 5 or 6 at 5 % or more (range 3 to 7). The largest race of a region has 17 % in the median region, 11 % at the least and 33 % at the most. Every near-human race is present in 47 to 50 of the 50 regions. See §6.4 item 2.
- **Rarest.** Hourkeepers hold 0.07 %: about 25 persons in a median region. Selkies hold 0.25 %, because the height modifier leaves them only the lowest land.

### 5.4 The list as Parameter Set json

The `Races` entries with the frequencies multiplied out, as `adding-a-race.md` §2 asks for. They are the entries that gave the "Alone" column of §5.3. A file that gives `Races` replaces the whole default list.

```json
[
  { "Id": "fellkin", "Name": "Fellkin", "Enabled": true,
    "BiomeFrequency": {"IceAndSnow": 2, "Tundra": 3, "ColdSteppe": 1, "BorealForest": 2},
    "HeightModifier": [{"HeightM": 0, "Factor": 0.5}, {"HeightM": 300, "Factor": 1}] },
  { "Id": "pactblood", "Name": "Pactblood", "Enabled": true,
    "BiomeFrequency": {"ColdSteppe": 0.5, "ColdDesert": 0.5, "TemperateGrassland": 0.5, "WoodlandAndShrubland": 0.5, "TemperateMixedForest": 0.5, "TemperateBroadleafForest": 0.5, "HotDesert": 1, "ThornScrub": 1, "Savanna": 1, "TropicalDryForest": 0.5} },
  { "Id": "nephilim", "Name": "Nephilim", "Enabled": true,
    "BiomeFrequency": {"Tundra": 0.5, "ColdSteppe": 0.5, "BorealForest": 0.5, "ColdDesert": 0.5, "TemperateGrassland": 0.5, "WoodlandAndShrubland": 0.5, "TemperateMixedForest": 0.5, "TemperateBroadleafForest": 0.5, "TemperateRainForest": 0.5, "HotDesert": 0.5, "ThornScrub": 0.5, "Savanna": 0.5, "TropicalDryForest": 0.5, "TropicalRainForest": 0.5},
    "HeightModifier": [{"HeightM": 0, "Factor": 0.5}, {"HeightM": 500, "Factor": 1.5}] },
  { "Id": "changeling", "Name": "Changeling", "Enabled": true,
    "BiomeFrequency": {"ColdSteppe": 0.5, "BorealForest": 0.5, "TemperateGrassland": 1, "WoodlandAndShrubland": 1, "TemperateMixedForest": 1, "TemperateBroadleafForest": 1, "TemperateRainForest": 0.5, "HotDesert": 0.5, "ThornScrub": 0.5, "Savanna": 0.5, "TropicalDryForest": 0.5, "TropicalRainForest": 0.5, "Mangrove": 0.5} },
  { "Id": "rimeborn", "Name": "Rimeborn", "Enabled": true,
    "BiomeFrequency": {"IceAndSnow": 3, "Tundra": 2, "BorealForest": 1, "ColdDesert": 1} },
  { "Id": "markedFolk", "Name": "Marked folk", "Enabled": true,
    "BiomeFrequency": {"ColdSteppe": 2, "ColdDesert": 2, "TemperateGrassland": 1, "WoodlandAndShrubland": 1} },
  { "Id": "gloamkin", "Name": "Gloamkin", "Enabled": true,
    "BiomeFrequency": {"Tundra": 1, "BorealForest": 0.5, "ColdDesert": 0.5, "TemperateRainForest": 1} },
  { "Id": "fae", "Name": "Fae", "Enabled": true,
    "BiomeFrequency": {"BorealForest": 0.5, "TemperateGrassland": 0.5, "WoodlandAndShrubland": 0.5, "TemperateMixedForest": 1, "TemperateBroadleafForest": 1.5, "TemperateRainForest": 1.5} },
  { "Id": "hourkeepers", "Name": "Hourkeepers", "Enabled": true,
    "BiomeFrequency": {"Tundra": 0.5, "ColdSteppe": 0.5, "ColdDesert": 0.5, "TemperateGrassland": 0.5, "WoodlandAndShrubland": 0.5},
    "HeightModifier": [{"HeightM": 200, "Factor": 0}, {"HeightM": 500, "Factor": 1}] },
  { "Id": "isleHeirs", "Name": "Isle-heirs", "Enabled": true,
    "BiomeFrequency": {"TemperateGrassland": 1, "WoodlandAndShrubland": 2, "TemperateMixedForest": 1, "TemperateBroadleafForest": 2, "TemperateRainForest": 1} },
  { "Id": "drakemarked", "Name": "Drakemarked", "Enabled": true,
    "BiomeFrequency": {"ColdSteppe": 1, "ColdDesert": 0.5, "HotDesert": 0.5, "ThornScrub": 0.5, "Savanna": 0.5} },
  { "Id": "selkie", "Name": "Selkie", "Enabled": true,
    "BiomeFrequency": {"IceAndSnow": 1, "Tundra": 1, "BorealForest": 2, "WoodlandAndShrubland": 1, "TemperateMixedForest": 1, "TemperateRainForest": 2, "Mangrove": 1},
    "HeightModifier": [{"HeightM": 20, "Factor": 1}, {"HeightM": 80, "Factor": 0}] },
  { "Id": "jinnBlooded", "Name": "Jinn-blooded", "Enabled": true,
    "BiomeFrequency": {"ColdDesert": 2, "HotDesert": 3, "ThornScrub": 2, "Savanna": 1} },
  { "Id": "brushtails", "Name": "Brushtails", "Enabled": true,
    "BiomeFrequency": {"WoodlandAndShrubland": 1, "TemperateMixedForest": 1, "TemperateBroadleafForest": 1, "Savanna": 2, "TropicalDryForest": 2, "TropicalRainForest": 1} },
  { "Id": "dhampir", "Name": "Dhampir", "Enabled": true,
    "BiomeFrequency": {"Tundra": 0.5, "BorealForest": 1, "TemperateMixedForest": 0.5, "TemperateRainForest": 1} },
  { "Id": "huldrefolk", "Name": "Huldrefolk", "Enabled": true,
    "BiomeFrequency": {"BorealForest": 3, "TemperateMixedForest": 2, "TemperateBroadleafForest": 2, "TemperateRainForest": 1} },
  { "Id": "motherline", "Name": "Motherline", "Enabled": true,
    "BiomeFrequency": {"ColdDesert": 0.5, "HotDesert": 1, "ThornScrub": 1, "Savanna": 0.5} },
  { "Id": "stillminds", "Name": "Stillminds", "Enabled": true,
    "BiomeFrequency": {"ColdDesert": 1, "WoodlandAndShrubland": 0.5, "HotDesert": 1, "ThornScrub": 0.5} },
  { "Id": "wardbred", "Name": "Wardbred", "Enabled": true,
    "BiomeFrequency": {"Tundra": 0.5, "ColdSteppe": 0.5, "BorealForest": 0.5, "WoodlandAndShrubland": 0.5, "TemperateMixedForest": 0.5} },
  { "Id": "yaksha", "Name": "Yaksha", "Enabled": true,
    "BiomeFrequency": {"Savanna": 0.5, "TropicalDryForest": 1, "TropicalRainForest": 1, "Mangrove": 0.5} },
  { "Id": "floeHunters", "Name": "Floe hunters", "Enabled": true,
    "BiomeFrequency": {"IceAndSnow": 3, "Tundra": 2} },
  { "Id": "snowHerders", "Name": "Snow herders", "Enabled": true,
    "BiomeFrequency": {"Tundra": 6, "ColdSteppe": 2, "BorealForest": 4} },
  { "Id": "tarburners", "Name": "Tarburners", "Enabled": true,
    "BiomeFrequency": {"BorealForest": 9, "TemperateMixedForest": 6} },
  { "Id": "grassRiders", "Name": "Grass riders", "Enabled": true,
    "BiomeFrequency": {"ColdSteppe": 9, "ColdDesert": 3, "TemperateGrassland": 6} },
  { "Id": "ploughlanders", "Name": "Ploughlanders", "Enabled": true,
    "BiomeFrequency": {"ColdSteppe": 5, "TemperateGrassland": 15, "WoodlandAndShrubland": 10, "TemperateMixedForest": 10, "TemperateBroadleafForest": 15, "TemperateRainForest": 5} },
  { "Id": "holtFolk", "Name": "Holt folk", "Enabled": true,
    "BiomeFrequency": {"WoodlandAndShrubland": 3, "TemperateMixedForest": 9, "TemperateBroadleafForest": 9, "TemperateRainForest": 6} },
  { "Id": "rainwoodFolk", "Name": "Rainwood folk", "Enabled": true,
    "BiomeFrequency": {"BorealForest": 3, "TemperateRainForest": 9} },
  { "Id": "groveFolk", "Name": "Grove folk", "Enabled": true,
    "BiomeFrequency": {"TemperateGrassland": 4, "WoodlandAndShrubland": 12, "TemperateBroadleafForest": 4} },
  { "Id": "terraceFolk", "Name": "Terrace folk", "Enabled": true,
    "BiomeFrequency": {"Tundra": 8, "ColdSteppe": 8, "ColdDesert": 8, "TemperateGrassland": 8, "WoodlandAndShrubland": 12, "TemperateMixedForest": 8, "TemperateBroadleafForest": 4, "Savanna": 4, "TropicalDryForest": 4, "TropicalRainForest": 4},
    "HeightModifier": [{"HeightM": 150, "Factor": 0}, {"HeightM": 450, "Factor": 1}] },
  { "Id": "wellkeepers", "Name": "Wellkeepers", "Enabled": true,
    "BiomeFrequency": {"ColdDesert": 6, "HotDesert": 6} },
  { "Id": "dunefarers", "Name": "Dunefarers", "Enabled": true,
    "BiomeFrequency": {"ColdDesert": 2, "HotDesert": 3, "ThornScrub": 2} },
  { "Id": "thornfoldFolk", "Name": "Thornfold folk", "Enabled": true,
    "BiomeFrequency": {"HotDesert": 2, "ThornScrub": 6, "Savanna": 6, "TropicalDryForest": 2} },
  { "Id": "rainkeepers", "Name": "Rainkeepers", "Enabled": true,
    "BiomeFrequency": {"ThornScrub": 4, "Savanna": 8, "TropicalDryForest": 12, "TropicalRainForest": 4} },
  { "Id": "clearingFolk", "Name": "Clearing folk", "Enabled": true,
    "BiomeFrequency": {"TropicalDryForest": 4, "TropicalRainForest": 6, "Mangrove": 2} },
  { "Id": "stiltFolk", "Name": "Stilt folk", "Enabled": true,
    "BiomeFrequency": {"TropicalDryForest": 2, "TropicalRainForest": 2, "Mangrove": 6} },
  { "Id": "longshoreFolk", "Name": "Longshore folk", "Enabled": true,
    "BiomeFrequency": {"IceAndSnow": 2, "Tundra": 2, "BorealForest": 4, "TemperateGrassland": 2, "WoodlandAndShrubland": 4, "TemperateMixedForest": 2, "TemperateBroadleafForest": 2, "TemperateRainForest": 4, "Mangrove": 2},
    "HeightModifier": [{"HeightM": 30, "Factor": 1}, {"HeightM": 120, "Factor": 0}] }
]
```

---

## 6. Design note

*Everything in this section is my own engineering judgement, not a sourced claim.*

### 6.1 Ways to use the list

| Use | What to do | Effect |
|---|---|---|
| The 36 alone | The 36 entries are the whole `Races` list | A world of humans and near-humans. §5.3, "Alone" |
| In place of `human` | Remove `human`, keep the other 22 default entries, add the 36 | §5.3, "Beside five" |
| Group N only, beside the default six | Keep the default list, add the 20 | Not measured. The near-human frequencies sum to 2 to 11.5 per biome against 10 to 30 for `human`, so their share would be lower than in "Beside five" |
| Group P only | The 16 entries, alone or in place of `human` | Measured in the first pass: the 16 hold 66.3 % in place of `human`, against 66.6 % for `human` |

### 6.2 What these races need that a race does not have

A race today is an id, a name, a toggle, biome frequencies and a height modifier. Those place a race. They carry nothing of what makes it this race: the looks, the story, the gift. Three places could hold that: more fields per race in the Parameter Set; authored data keyed by race id, like the description of a person; or a file beside the list that is read when prompts are built. That is a decision for the user. Until it is taken, §4 is the only place the descriptions live.

Several near-human races also carry a **rule**, not only a look: Changelings wear other faces; Hourkeepers renew; the Motherline bears almost only daughters; Nephilim, Pactblood and Jinn-blooded are born into ordinary families; Wardbred take in foundlings. None of these can be said with a biome frequency. They touch how a family and a person get their race, which the readme leaves undecided.

### 6.3 Overlap with `races.md` and the default list

| Proposed | Overlaps | In the default list (Appendix E)? | Difference |
|---|---|---|---|
| Fellkin | E1 Giant-blooded | `giantBlooded`, off, common 0.5 | Same archetype. The default has it in more biomes (weight 1 in most temperate ones). One of the two is redundant |
| Rimeborn | J3 Frost folk | `frostFolk`, off, common 1 | Same biomes; `frostFolk` has no look or story beyond the name |
| Pactblood | H1 Fiend-touched | Not listed: `races.md` §4.1 leaves it out as a follower of its host race | This note gives it its own frequencies |
| Nephilim, Jinn-blooded | H2 Celestial and elemental-touched | Not listed, for the same reason | H2 is one archetype; here it is two races |
| Drakemarked | H3 Dragonkin | `dragonkin`, off, common 0.5 | Dragonkin are scaled all over; Drakemarked are human with marks |
| Changeling, Brushtails | J5 Shapeshifters | Not listed | J5 lumps them |
| Brushtails | F2 Catfolk | `catfolk`, off | Catfolk have a cat's head; Brushtails a human face |
| Fae | B1 Elf, J4 Small fey | `elf`, on, common 2 | The nearest overlap of all. Fae share the elf's forests. If both are on, the Fae need their difference (mounds, bargains, taboo) to carry them |
| Huldrefolk | J2 Plant folk | Not listed | Plant folk are plants; Huldrefolk look human from the front |
| Dhampir | I1 Undead people | `undead`, off | The undead are dead; Dhampirs are living |
| Selkie | G1 Merfolk | Not listed (sea peoples are open) | Merfolk live in the water; Selkies on the shore |
| Isle-heirs | A2 "Exalted humans" | Not listed (a variant of `human`) | Same idea, now a race |
| (none proposed) | B4 Halfling, B5 Gnome, B2 Dark elf | `halfling` on, `gnome` and `darkElf` off | The small folk and the dark pointed-eared folk of §2.18 are already there |

### 6.4 Open questions and conflicts with the existing docs

1. **"Race" for human peoples and near-humans.** The readme defines a race as "a people that can live in the world: humans, elves, dwarves and so on", so `human` is one race, and `races.md` A2 and §5.3 called human cultures variants of one race. The user has since decided that every people is a separate race and that "race" is the only term. This list follows that. The readme's sentence that the default list "holds 23 races" with Human as one of them, and Appendix E, would change if the list is adopted.
2. **A race has no homeland at the scale of a region (gap in `map-generation.md` §19).** The rule shares each patch by biome, and biomes change within a region. So the median region holds 23 of the 36 races and none has a majority (§5.3), and every near-human race turns up in nearly every region. For rare, strange races that is the wrong feel: a Rimeborn city or a Fae mound should be a thing a player hears of and travels to. This is the "presence field per race" that `map-generation-research.md` §10.7 lists as open, and group N needs it more than any race before it.
3. **A race cannot ask for coast, river or lake (open in §10.7).** Selkies, Longshore folk and Isle-heirs want the coast; Stilt folk want tidal water; Wellkeepers want a river or well. Selkies and Longshore folk use a low-height modifier as a stand-in, which also puts them on low inland ground and leaves the Selkies very little land. The facts exist per patch (`Coast`, `Harbour`, `River`, `Mouth` in the region files), but a race has no weight for them.
4. **How many races per biome is too many.** With 36 races the temperate biomes list 12 to 16 each (§5.2), and a median region has 23 at 1 % or more. The rule handles it, but a player may not. Ways to thin it: turn on only a few near-human races per world (the toggle exists); or a presence field (item 2).
5. **Followers.** Pactblood, Nephilim and Changelings have no homeland in their sources: they are born or hidden among other peoples. `races.md` §4.1 left such races out of the biome table for that reason and proposed a birth chance per family instead (§5.4 there). Here they have flat, low frequencies, which makes them a thin share of every patch. That gives the right numbers and the wrong mechanism. How a family and a person get a race is still undecided in the readme.
6. **The Supernatural Field.** Most of group N is uncanny, and `races.md` already proposed a Supernatural weight per race. A race has no such weight today (open in §10.7). Fae, Gloamkin, Nephilim, Yaksha and Jinn-blooded would use it before any biome.
7. **Very rare races.** Hourkeepers come out at about 25 persons in a median region and Selkies at about 90. That is deliberate, but whether a race that small can form a family, a faction or a Prospect is not something the current rule answers.
8. **The height modifier acts on the mean height of a patch.** Half of the land lies below 155 m, so "high" starts very low: Fellkin get full weight from 300 m, Hourkeepers from 500 m, Terrace folk from 450 m. A weight by elevation type would place them better; §10.7 records that the user asked for a limit in metres.
9. **A race changes the share of the land, not its capacity.** Real ways of life differ in density by two orders of magnitude (§3). `common` is the only lever.
10. **Looks follow sun, and the map has no latitude.** Temperature is a noise field. The colouring of the human peoples can only follow open-and-hot against shaded-or-cold.
11. **Ice & snow.** Appendix E says Ice & snow below 200 m holds nobody with the six default races. With this list it holds five races.
12. **Names of persons.** The name generator has three styles and nothing maps a race to a style. Thirty-six races would share them.
13. **Working names.** Twelve near-human races have coined working names. Each stands for a named source race (§5.1, "Based on"), and the user chooses the final names. A final name needs a proper trademark and originality check; the web searches here were light (§4).
14. **Closeness to one source.** Each race is mixed from several sources, but some will be recognised at once: Hourkeepers (two hearts, renewal, a vow not to meddle) as Time Lords, Stillminds as Vulcans, Motherline as Gerudo, Fellkin as Norn. That is what the user asked for. Authored text and portraits should still be written fresh and should not use the source names or designs.
15. **Making the list the default.** `adding-a-race.md` §3 lists what changes with the default list: code, the default json, Appendix E, §19, the readme, §10.7 and the tests. This note changes none of them.
16. **Stereotype review of the human peoples.** Each is built from a way of life with looks that do not match the real peoples behind it. Authored text and portraits should still be reviewed.

### 6.5 Gaps in this research

- **Sources that refused or were missing.** Fandom wikis returned 402 (Wookieepedia for the Mirialans). The Star Wars Databank has no page at the address tried for the Chiss. The Wikipedia pages tried for the planetouched (genasi), the Witcher franchise and the Saiyans were missing or were disambiguation pages. TaleWorlds returned 403 in the first pass.
- **Thin on the page.** Several Wikipedia articles say little about the race itself: Eberron on changelings and shifters, the Malazan article on the Tiste, the Aes Sedai and Minbari pages on looks, the Genshin Impact article on its non-human peoples, the Tiki article on manaketes (nothing, so manaketes are not cited), the FFXIV wiki on the lands of its clans.
- **Not read:** the works themselves, and any rulebook beyond the free rules and Archives of Nethys.
- **Not surveyed, though asked for or obvious:** The Elder Scrolls beyond what `races.md` has; Warhammer Fantasy and Age of Sigmar; World of Darkness beyond two games; Fire Emblem; mobile and gacha games beyond Genshin Impact; Babylon 5 and Farscape beyond one page each; Pratchett; Octavia Butler; Sanderson beyond three Coppermind pages; anime and manga beyond four titles.
- **Unverified claims carried in the note:** that the Chiss home world is ice-bound; that Mirialan marks are tattoos earned by deeds and their world cold and dry; the looks of D&D's changelings, genasi and kalashtar; the spots of the Trill; the head wings of the High Entia; the ageless faces of the Aes Sedai; the witchers' eyes and hair; the Au Ra steppe clans and the Miqo'te and Viera homelands; Zabrak, Pantoran, Twi'lek, Natan, oni, Amazons, Saiyans, Faunus, the Tiste and Face Dancers, which are named from memory only; the forest trades of the Holt folk; the density figures by way of life [partially verified]; the diving study [partially verified]; the originality of the working names that were not searched.
- **Invented:** every Appearance and Story in §4, all habitat weights and all `common` values.

---

## 7. Sources

### 7.1 Tabletop games

- **DND-FR** — Wizards of the Coast, *D&D Free Rules (2024)*, "Character Origins" (goliath, tiefling; no aasimar). https://www.dndbeyond.com/sources/dnd/free-rules/character-origins
- **PF2-VH** — Archives of Nethys (Paizo-licensed), "Versatile Heritages". https://2e.aonprd.com/Ancestries.aspx?Versatile=true
- **PF2-ANC** — Archives of Nethys, "Ancestries". https://2e.aonprd.com/Ancestries.aspx
- **EBERRON** — Wikipedia, "Eberron" (read to character 100,000 of 106,000). https://en.wikipedia.org/wiki/Eberron
- **SHADOWRUN** — Wikipedia, "Shadowrun". https://en.wikipedia.org/wiki/Shadowrun
- **WH40K** — Wikipedia, "Space Marine (Warhammer 40,000)". https://en.wikipedia.org/wiki/Space_Marine_(Warhammer_40,000)
- **WOD-CTD** — Wikipedia, "Changeling: The Dreaming". https://en.wikipedia.org/wiki/Changeling:_The_Dreaming
- **VTM** — Wikipedia, "Vampire: The Masquerade" (read to character 100,000 of 140,000). https://en.wikipedia.org/wiki/Vampire:_The_Masquerade

### 7.2 Computer and mobile games

- **GW2-NORN**, **GW2-SYL** — Guild Wars 2 Wiki (ArenaNet's official wiki), "Norn", "Sylvari". https://wiki.guildwars2.com/wiki/Norn, https://wiki.guildwars2.com/wiki/Sylvari
- **BG3-TIEF**, **BG3-AAS** — bg3.wiki (community wiki), "Tiefling", "Aasimar". https://bg3.wiki/wiki/Tiefling, https://bg3.wiki/wiki/Aasimar
- **DESTINY** — Destinypedia (community wiki), "Awoken". https://www.destinypedia.com/Awoken
- **WOW-VRY**, **WOW-VOID**, **WOW-DRA** — Warcraft Wiki (community wiki), "Vrykul", "Void elf", "Draenei" (each read to character 100,000). https://warcraft.wiki.gg/wiki/Vrykul, https://warcraft.wiki.gg/wiki/Void_elf, https://warcraft.wiki.gg/wiki/Draenei
- **FF14** — Final Fantasy XIV community wiki, "Races". https://ffxiv.consolegameswiki.com/wiki/Races
- **QUNARI** — Wikipedia, "Qunari". https://en.wikipedia.org/wiki/Qunari
- **ASARI** — Wikipedia, "Asari (Mass Effect)". https://en.wikipedia.org/wiki/Asari_(Mass_Effect)
- **ZELDA** — Wikipedia, "Universe of The Legend of Zelda" (read to character 100,000 of 236,000). https://en.wikipedia.org/wiki/Universe_of_The_Legend_of_Zelda
- **XENO** — Wikipedia, "Xenoblade Chronicles (video game)" (read to character 100,000 of 200,000). https://en.wikipedia.org/wiki/Xenoblade_Chronicles_(video_game)
- **GENSHIN** — Wikipedia, "Genshin Impact" (read to character 100,000 of 193,000). https://en.wikipedia.org/wiki/Genshin_Impact
- **CK3-T**, **CK3-C** — Crusader Kings III Wiki (Paradox), "Traditions", "Culture" (each read to character 100,000). https://ck3.paradoxwikis.com/Traditions, https://ck3.paradoxwikis.com/Culture
- **MB2-W** — Wikipedia, "Mount & Blade II: Bannerlord". https://en.wikipedia.org/wiki/Mount_%26_Blade_II:_Bannerlord
- **HZD** — Wikipedia, "Horizon Zero Dawn" (read to character 100,000 of 243,000). https://en.wikipedia.org/wiki/Horizon_Zero_Dawn

### 7.3 Film and television

- **THRAWN** — Wikipedia, "Thrawn" (read to character 100,000 of 158,000). https://en.wikipedia.org/wiki/Thrawn
- **SW-MIR** — StarWars.com Databank, "Mirialan". https://www.starwars.com/databank/mirialan
- **SW-PT** — Wikipedia, "List of Star Wars species (P–T)" (read to character 100,000 of 147,000). https://en.wikipedia.org/wiki/List_of_Star_Wars_species_(P%E2%80%93T)
- **TL** — Wikipedia, "Time Lord" (read to character 100,000 of 138,000). https://en.wikipedia.org/wiki/Time_Lord
- **VULCAN**, **ANDOR**, **BAJOR** — Wikipedia, "Vulcan (Star Trek)", "Andorian", "Bajoran". https://en.wikipedia.org/wiki/Vulcan_(Star_Trek), https://en.wikipedia.org/wiki/Andorian, https://en.wikipedia.org/wiki/Bajoran
- **TRILL** — Wikipedia, the article reached at "Trill (Star Trek)" (read to character 100,000 of 148,000; Trill and Betazoids). https://en.wikipedia.org/wiki/Trill_(Star_Trek)
- **B5** — Wikipedia, the article reached at "Minbari" (read to character 100,000 of 238,000). https://en.wikipedia.org/wiki/Minbari
- **SG-ANC** — Wikipedia, "Ancient (Stargate)". https://en.wikipedia.org/wiki/Ancient_(Stargate)
- **FARSCAPE** — Wikipedia, "Farscape" (read to character 100,000 of 158,000). https://en.wikipedia.org/wiki/Farscape
- **NAVI** — Wikipedia, "Na'vi" (read to character 100,000 of 138,000). https://en.wikipedia.org/wiki/Na%27vi
- **KRYPTON** — Wikipedia, "Kryptonian". https://en.wikipedia.org/wiki/Kryptonian
- **FREMEN**, **BENE** — Wikipedia, "Fremen", "Bene Gesserit". https://en.wikipedia.org/wiki/Fremen, https://en.wikipedia.org/wiki/Bene_Gesserit
- **ATLA** — Wikipedia, "Avatar: The Last Airbender" (read to character 100,000 of 221,000). https://en.wikipedia.org/wiki/Avatar:_The_Last_Airbender

### 7.4 Literature, anime and manga

- **NUMENOR** — Wikipedia, "Númenor" (read to character 100,000 of 117,000). https://en.wikipedia.org/wiki/N%C3%BAmenor
- **TOLK-MEN** — Wikipedia, "Men in Middle-earth" (read to character 100,000 of 142,000). https://en.wikipedia.org/wiki/Men_in_Middle-earth
- **ASOIAF** — Wikipedia, "World of A Song of Ice and Fire" (read to character 200,000 of 259,000). https://en.wikipedia.org/wiki/World_of_A_Song_of_Ice_and_Fire
- **TARG** — Wikipedia, "House Targaryen" (read to character 100,000 of 265,000). https://en.wikipedia.org/wiki/House_Targaryen
- **CM-THAY**, **CM-UNK** — Coppermind (community wiki of the Cosmere), "Thaylenah", "Unkalaki". https://coppermind.net/wiki/Thaylenah, https://coppermind.net/wiki/Unkalaki
- **HAIN** — Wikipedia, "Hainish Cycle". https://en.wikipedia.org/wiki/Hainish_Cycle
- **CULTURE** — Wikipedia, "The Culture" (read to character 100,000 of 145,000). https://en.wikipedia.org/wiki/The_Culture
- **MALAZAN** — Wikipedia, "Malazan Book of the Fallen" (says nothing of the Tiste peoples' looks). https://en.wikipedia.org/wiki/Malazan_Book_of_the_Fallen
- **WOT**, **WOT-AES** — Wikipedia, the article reached at "Aiel" and at "Aes Sedai" (read to character 100,000 of 172,000). https://en.wikipedia.org/wiki/Aiel, https://en.wikipedia.org/wiki/Aes_Sedai
- **WITCHER** — Wikipedia, "The Witcher" (read to character 100,000 of 118,000). https://en.wikipedia.org/wiki/The_Witcher
- **EARTHSEA**, **HYB**, **SAXON** — Wikipedia, "Earthsea", "Hyborian Age", "The Saxon Stories". https://en.wikipedia.org/wiki/Earthsea, https://en.wikipedia.org/wiki/Hyborian_Age, https://en.wikipedia.org/wiki/The_Saxon_Stories
- **FRIEREN** — Wikipedia, "Frieren" (read to character 100,000 of 150,000). https://en.wikipedia.org/wiki/Frieren
- **ELFEN** — Wikipedia, "Elfen Lied". https://en.wikipedia.org/wiki/Elfen_Lied
- **KEMONO** — Wikipedia, "Kemonomimi". https://en.wikipedia.org/wiki/Kemonomimi
- **KAGUYA** — Wikipedia, "The Tale of the Bamboo Cutter". https://en.wikipedia.org/wiki/The_Tale_of_the_Bamboo_Cutter

### 7.5 Folklore and myth

- **AOS** — Wikipedia, "Aos Sí" (read to character 100,000 of 135,000). https://en.wikipedia.org/wiki/Aos_S%C3%AD
- **CHG-FOLK** — Wikipedia, "Changeling" (read to character 100,000 of 121,000). https://en.wikipedia.org/wiki/Changeling
- **SELKIE** — Wikipedia, "Selkie" (read to character 100,000 of 114,000). https://en.wikipedia.org/wiki/Selkie
- **HULDER** — Wikipedia, "Hulder". https://en.wikipedia.org/wiki/Hulder
- **JINN** — Wikipedia, "Jinn" (read to character 100,000 of 224,000). https://en.wikipedia.org/wiki/Jinn
- **NEPH** — Wikipedia, "Nephilim" (read to character 100,000 of 111,000). https://en.wikipedia.org/wiki/Nephilim
- **JOTUNN**, **NORNS**, **VALK** — Wikipedia, "Jötunn", "Norns", "Valkyrie" (each read to character 100,000). In myth the Norns are three fate-spinning goddesses, some called giants [NORNS]; the game race of that name is a different thing. https://en.wikipedia.org/wiki/J%C3%B6tunn, https://en.wikipedia.org/wiki/Norns, https://en.wikipedia.org/wiki/Valkyrie
- **DHAMPIR** — Wikipedia, "Dhampir". https://en.wikipedia.org/wiki/Dhampir
- **KITSUNE** — Wikipedia, "Kitsune" (read to character 100,000 of 191,000). https://en.wikipedia.org/wiki/Kitsune
- **TENGU**, **YAKSHA** — Wikipedia, "Tengu", "Yaksha" (each read to character 100,000). https://en.wikipedia.org/wiki/Tengu, https://en.wikipedia.org/wiki/Yaksha

### 7.6 Real peoples, ways of life and human variation

- **NP**, **PAST**, **TRANS** — Wikipedia, "Nomadic pastoralism", "Pastoralism", "Transhumance". https://en.wikipedia.org/wiki/Nomadic_pastoralism, https://en.wikipedia.org/wiki/Pastoralism, https://en.wikipedia.org/wiki/Transhumance
- **HG** — Wikipedia, "Hunter-gatherer" (read to character 100,000 of 185,000). https://en.wikipedia.org/wiki/Hunter-gatherer
- **TALL** — University of Helsinki news release on M. Tallavaara, M. Luoto and J. T. Eronen, *PNAS*, 2018. https://www.helsinki.fi/en/news/science/bound-by-nature-cultural-evolution-has-not-freed-hunter-gatherers-from-environmental-forcing-0
- **MED** — D. Medupe, S. G. Roberts, M. K. Shenk and L. Glowacki, "Why did foraging, horticulture and pastoralism persist after the Neolithic transition? The oasis theory of agricultural intensification", *Philosophical Transactions of the Royal Society B* 378 (1883), 2023. https://pmc.ncbi.nlm.nih.gov/articles/PMC10291438
- **SMIL** — V. Smil, a table of population densities by way of life, and a Sage encyclopaedia entry on population density. https://muse.jhu.edu/books/9780262312264/9780262312264-12.pdf, https://sk.sagepub.com/ency/edvol/embed/geography/chpt/population-density **[partially verified: search excerpts; the first returned 403, the second was not opened]**
- **INUIT**, **REIN**, **SLASH**, **SHIFT**, **TAR**, **PNW**, **MEDIT**, **VERT**, **KUK**, **QANAT**, **OASIS**, **BED**, **KRAAL**, **TANK**, **PYG**, **STILT**, **BAJAU**, **MARSH**, **AUST** — Wikipedia, "Inuit", "Reindeer herding", "Slash-and-burn", "Shifting cultivation", "Pine tar", "Indigenous peoples of the Pacific Northwest Coast", "Mediterranean cuisine", "Vertical archipelago", "Kuk Swamp", "Qanat", "Oasis", "Bedouin", "Kraal", "Tank cascade system", "Pygmy peoples", "Stilt house", "Sama-Bajau", "Marsh Arabs", "Austronesian peoples" (the long ones read to character 100,000). Each at https://en.wikipedia.org/wiki/ followed by the title with underscores.
- **SKIN**, **ALLEN**, **ALT**, **LP** — Wikipedia, "Human skin color", "Allen's rule", "High-altitude adaptation in humans", "Lactase persistence". https://en.wikipedia.org/wiki/Human_skin_color, https://en.wikipedia.org/wiki/Allen%27s_rule, https://en.wikipedia.org/wiki/High-altitude_adaptation_in_humans, https://en.wikipedia.org/wiki/Lactase_persistence
- **ILARDO** — M. A. Ilardo et al., "Physiological and Genetic Adaptations to Diving in Sea Nomads", *Cell* 173 (3), 569–580, 2018. https://www.eurekalert.org/news-releases/647015 **[partially verified: search excerpts]**
