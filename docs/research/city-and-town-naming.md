# City and Town Naming: How Settlement Names Are Made, and a Rule for This Generator

*Research date: 2026-11-20. Scope: how city and town names are built in real toponymy (the base fantasy borrows from), in fantasy literature and in computer games and their generators. The second half assesses what fits this codebase and proposes a rule. §1 gives the findings that matter most. §2 reads real place-name structure. §3 reads what authors say about their naming. §4 reads the generators and games, from their source where it is public. §5 is the summary table. §6 is the assessment and the proposed rule. §7 lists the open decisions. §8 lists what could not be verified. §9 lists the sources.*

**Status.** A rule for the names of cities and towns was adopted on 2026-11-20: [society-generation.md](../society-generation.md) section 5.3. It keeps the structure §6.3 proposes (a specific and a generic, the generic from the facts of the site, names unique within a region) and takes the words of each race from [city-and-town-names.md](../city-and-town-names.md) instead of the surname models. Section 5.3 lists what differs from §6.3 and what it decided of §7.

**How to read the citations**

- Each claim cites the source that owns it, by key in square brackets. The keys are listed in §9.
- **[partially verified]** means the source exists and is cited, but the claim was confirmed only from a search-index excerpt, an abstract, a secondary article or a summary of the source, not from the text itself.
- **[unverified]** means general knowledge that was not checked against any source in this pass.
- Prose is paraphrased, not quoted. Code constants, word lists and raw tokens are given as the source has them.
- All of §6 and every paragraph headed **Rule a generator could take** are my own engineering judgement. They map a finding onto the inputs of this repo; they are not sourced claims. The repo facts in §6 were read from the code at commit `f370cb6`.

**Fetch notes.** `mewo2.com/notes/naming-language/` and `mewo2.com/notes/terrain/` now return 404; both were read from Internet Archive snapshots of August 2024 (the fetch tool refuses `web.archive.org`, so they were fetched with a plain HTTP client and stripped to text). `ck3.paradoxwikis.com` served a JavaScript "client challenge" page instead of the article, both to the fetch tool and to a plain request; it was not worked around, so the Crusader Kings III `cultural_names` mechanism is not read here (§8). The EU4 wiki is on the same host family and was not tried. `civilization.fandom.com` gave 404 for a city-list page. The Key to English Place-Names (`kepn.nottingham.ac.uk`) is a JavaScript map: its front page gave no entries, and its HALOGEN query page (`halogen.le.ac.uk`) gave no counts. The Johnson paper on `pcgworkshop.com` arrived as a binary PDF and was read after text extraction. Tolkien's *Nomenclature* was read only through Wikipedia and Tolkien Gateway; copies of the full text found on unofficial hosts were not used. The GitHub repositories (Azgaar, mewo2, Wesnoth, Freeciv, Unciv, watabou) and the Dwarf Fortress wiki (raw wikitext) answered normally.

**Inputs the rule may read** (from `readme.md`, `docs/society-generation.md` and the domain types at commit `f370cb6`)

| Input | What it is | Stage |
|---|---|---|
| Prospect | Patch, spot, `ForCity`, score, class (Rich, Average, Poor), `OnShow`, the five factors (resources, geography, trade, climate, prosperity) | Planned |
| Prospect facts | `Coast`, `Harbour` (borders a bay), `River` (patches drained; 8 and up navigable, 80 and up large), `Mouth`, `Confluence`, `HeadOfNavigation`, `LakeAreaM2`, `Depth` | Planned |
| Patch | Estimated biome, 27 resources with amounts (Salt, IronOre, Gold, Stone …), flora and fauna kinds | Detailed |
| Zone | Elevation type (Flat Land, Lowland, Hills, Mountains), bedrock | Outlined |
| Races of the Prospect | The race ids of a city or a town on it, the largest around it first (section 5.1). Each race has a `NameStyle`: Common, Exotic or Dark, and a `Human` flag | Planned |
| Factions | Ruling faction with its seat (the capital), type, form and lead race; opposition and lesser factions with their seats (a Guild, an Order, a Chartered town …) | Planned |
| Region name and ruling house | The name of the region and the surname of the ruling house when a house rules (section 4.10) | Planned |
| City or town | Kind (city or town), capital flag, slot, rank, persons, the persons of each race | Settled |

The rule must be a pure function of the foundation: every random choice goes through `Hashing` with a purpose number, and no step may depend on the order regions are generated in. A Settled region may read its own data and the Planned data of its bordering regions (readme, region generation stage table).

---

## 1. Overview

Eight findings shape the rule.

1. **Real settlement names are two parts: a specific and a generic.** The generic says what kind of place it is (farmstead, ford, fort, river mouth, clearing); the specific qualifies it, often with a personal name or a descriptive word [SNSBI]. The generic usually describes the site or the founding function, not the present size: Old English *tūn* meant farmstead or estate, not "town" [SNSBI]. So a city and a town are told apart by the generics that fit them (forts and boroughs for central places, farmsteads and clearings for small ones) and by a few affixes (Market, Chipping, Great, Little, New), not by a size marker.
2. **Generics follow the site, and the site facts this repo has map onto real generics almost one to one.** River mouth (*-mouth*), ford (*-ford*), bridge (*-bridge*, German *-brück*), landing place (*-hithe*), hill (*-don*), clearing (*-ley*), open land (*-field*), fort (*-bury*, *-chester*, German *-burg*), church (*-minster*, *-kirk*), market (*Chipping*) [SNSBI] [WP-UKGEN] [WP-DE]. The Prospect facts already carry mouth, harbour, river size, confluence, head of navigation and lake.
3. **Language layers give a land its texture.** English names stack Celtic, Old English, Norse and Norman French layers, each with its own generics [SNSBI]. Tolkien did the same on purpose: the English-form names of the Shire stand for translated Common Speech, Rohan's names are Old English, and Bree keeps a Celtic word for "hill" as a trace of an older language [TOL-TR] [TG-BREE] **[partially verified]**. In this repo the three name styles are the natural layers.
4. **Authors separate translated-meaning names from invented-language names.** Tolkien told translators to translate English-form names by their meaning and to leave the invented-language names untouched [TOL-TR] **[partially verified]**. Sanderson builds a per-culture naming structure around a few names he likes, so the names of one people share their shape [WE-724]. Martin admires Tolkien's places that carry one name per people [GRRM-WOT] **[partially verified]**.
5. **The generator closest to this repo's needs is Wesnoth's village namer, and it is a site-to-generic table.** A village takes a name class from what lies next to it (road, river, bridge, forest, lake, mountain, swamp; else counts of grassland, forest, mountain and hill tiles), then a small grammar per class joins a base name to a generic: *-ford*, *-cross* and *" on {river}"* by a river, *-harbor* and *-port* by a lake, *-wood* in forest, *-hill* and *-crest* on hills [WES-JOB] [WES-EN].
6. **Most other generators either pick from fixed lists or run a Markov chain over real names.** Azgaar's map generator runs a syllable Markov chain over a culture's base of real town names, with no site input and no duplicate check [AZ-NAMES] [AZ-BURG]. Freeciv and Unciv (and the Civilization games they follow) walk a fixed per-nation list; Freeciv alone weights the list by terrain labels on each name [FC-NAT] [FC-CT] [UNCIV]. Dwarf Fortress composes names from meaning-tagged words, filtered by the symbols a civilisation prefers, and translates them into the civilisation's language [DF-40D] [DF-ET].
7. **The naming-language approach makes recurring elements per people.** O'Leary's generator gives every word class (city, region) a small pool of morphemes that recur often, so the place names of one map share elements as real names do [MEWO-NOTES] [MEWO-LANG]. Johnson's Ultima Ratio Regum puts geographic references of a culture's homeland into its city names [URR-PCG].
8. **Every generator read here that guarantees unique names does it with a global "used names" set, which this repo cannot have.** Wesnoth draws up to 30 times against a per-map set [WES-JOB]; O'Leary rejects any name that contains or is contained in an earlier one [MEWO-LANG]; Freeciv and Unciv skip names already used in the game [FC-CT] [UNCIV]. All of them depend on the order places are created. Real toponymy tolerates duplicates (dozens of places are called Barton [SNSBI]) and tells them apart with affixes and qualifiers. So the rule here should make names unique inside a region, reduce clashes with the bordering regions in one order-independent step, and accept rare duplicates further away.

---

## 2. Real toponymy: the structure fantasy borrows

### 2.1 Specific and generic

Most major English place-names have two elements. The second is the generic, the first the specific or qualifier; the specific is often a personal name or a descriptive adjective or noun [SNSBI]. German follows the same pattern with its own generics (*-dorf*, *-heim*, *-hausen*, *-burg*, *-furt*, *-brück*, *-stadt*, *-feld*, *-bach*, *-kirchen*) [WP-DE]. A generic with *-ing-* before it (*-ingham*, German *-ingen*) means "the people of" a named leader [WP-UKGEN] [WP-DE].

### 2.2 The generics and what they mean

Glosses from [SNSBI] unless another key is given.

| Generic | Language | Meaning | Examples |
|---|---|---|---|
| *-tūn* / *-ton* | Old English | farmstead, village, estate; originally not "town" | |
| *-hām* / *-ham* | Old English | homestead; early, later not used for new names | Nottingham, Birmingham [WP-UKGEN] |
| *-by* | Norse | farmstead, village | Derby, Grimsby |
| *thorp* | Norse | outlying farmstead | |
| *wīc* / *-wick*, *-wich* | Old English | secondary or specialised settlement, trading place | Ipswich, Norwich [web search excerpt] |
| *-ford* | Old English | ford | Bradford, Guildford [WP-UKGEN] |
| *burh* / *-bury*, *-borough* | Old English | fortified site | Peterborough, Salisbury |
| *ceaster* / *-chester*, *-caster* | Old English from Latin | fort, often Roman | Gloucester, Lancaster, Manchester |
| *mūth* / *-mouth* | Old English | mouth, estuary | Plymouth, Portsmouth |
| *-hithe*, *-hythe* | Old English | wharf, landing place | Rotherhithe [WP-UKGEN] |
| *-minster* | Old English | large church, monastery | Westminster [WP-UKGEN] |
| *-kirk* | Norse | church | Falkirk [WP-UKGEN] |
| *lēah* / *-ley* | Old English | woodland clearing, wood pasture | |
| *feld* / *-field* | Old English | open land | |
| *dūn* / *-don* | Old English | hill | |
| *worth* | Old English | enclosure, often humble sites | |
| *stōc* / *Stoke* | Old English | secondary, outlying settlement | |
| *aber-*, *inver-* | Brythonic, Gaelic | river mouth or confluence | Aberdeen, Inverness [WP-UKGEN] |
| *caer-*, *dun-* | Brythonic, Gaelic | fort | Cardiff, Dundee [WP-UKGEN] |
| *-brück*, *-furt*, *-burg*, *-berg* | German | bridge, ford, fort, mountain | Saarbrücken, Frankfurt, Hamburg, Heidelberg [WP-DE] |

That *-wich* names cluster on the old salt-works (Droitwich, Nantwich) is **[unverified]**; the sources read give *wīc* only as a specialised or trading settlement. That Old English *port* often meant a market town (Stockport, Newport) is **[unverified]**; [WP-UKGEN] glosses *port* as Middle English "harbour or landing ground".

### 2.3 Layers

Celtic names survive for major landscape features and a few settlements (Leeds, York, Carlisle, London); most settlement names are Old English from the fifth century; Norse names come in the late ninth to early eleventh centuries; Norman French adds a thin post-Conquest layer (beau, mont, castle, grange), used in only nine of about 140 new towns in one study [SNSBI]. Each layer has its own generics, so a reader sees the layers in the endings alone (*-by* against *-ton*).

### 2.4 Size, function and affixes

Real generics mark the founding site or function, not today's size. *Lēah* places were loose settlements on poorer land, *tūn* places more organised ones; *wīc* marks a specialised site; *burh* and *ceaster* mark defence [SNSBI]. Size and function appear in affixes added later [SNSBI]:

- manorial affixes, a family name of the owner (Acton Burnell);
- *Great* and *Little* (Great and Little Snoring), *Lower*, *Over*, *Middle* for position;
- *Chipping* from Middle English *cheping*, "a market" (Chipping Sodbury);
- *New* for post-Conquest settlements (Newcastle upon Tyne).

Duplicates are normal: dozens of places are called Barton [SNSBI]. Real usage tells them apart with these affixes and with qualifiers such as a river (Newcastle upon Tyne, Frankfurt am Main) **[unverified as a general rule]**.

**Rule a generator could take.** Build a name as specific + generic. Draw the generic from the facts of the site, weighted by what the site has. Let the kind of place shift the weights (forts and boroughs for cities, farmsteads and clearings for towns) instead of marking size. Use the real affixes as the means of telling near-duplicates apart: *Little*, *Upper*, *Nether*.

---

## 3. Fantasy literature

### 3.1 Tolkien: translated names and invented names

The English of *The Lord of the Rings* is presented as a translation of Westron, the Common Speech; the English-form names stand for Westron names. Rohan's language is represented by Old English and Dale's by Old Norse [TOL-TR] **[partially verified]**. Bree is an English place-name built on a Celtic word for "hill", kept as a trace of the old language of the Men of Bree (Appendix F, "On Translation") [TG-BREE] **[partially verified: through Tolkien Gateway, which cites the appendix and the *Nomenclature*]**. Hobbiton is a *-ton* name [TG-HOB].

Tolkien wrote his *Guide to the Names* (1967, published 1975, retranscribed in 2005 as *Nomenclature*) after the Dutch and Swedish translations. It tells translators to translate English-form names by their meaning, to leave Elvish names alone, and to translate the meaning of Elvish names he had himself rendered in English (Rivendell for *Imladris*). Names not in his list are to be left "entirely unchanged in any language used in translation" [TOL-TR] [TG-GUIDE] **[partially verified]**.

What to take: two classes of name. A meaning-name is stored as its meaning and rendered in the reader's language; an invented-language name is kept as it is.

### 3.2 Le Guin

Le Guin held that the names of Earthsea should not be explained; her line is that names "like dreams, must explain themselves" ("Dreams Must Explain Themselves", 1973) [LG-DREAMS] **[partially verified: search excerpt quoting the essay]**. Robinson's study of her "onomaturgy" treats sound, form and meaning as building elements that move from one name to another and recur across her works [LG-ROB] **[partially verified: abstract]**. No concrete construction rule for place names was found.

What to take: invented names carry a sound that is consistent within a people; meaning can stay hidden.

### 3.3 Sanderson

In *Writing Excuses* 7.24 (June 2012) Sanderson says he starts from names he likes the sound of and then builds a linguistic structure for the culture around them, so later names fit it; a second, "holy" naming pattern of a culture follows its religion (symmetrical names). He renamed a main character late because the old name did not fit the structure [WE-724].

What to take: a naming style is a structure per people, and a name that breaks the structure stands out.

### 3.4 Martin

Martin says naming is hard, admires Jack Vance's place names for their sound, and praises Tolkien's places that have a name for each people that uses them. For his mountains he chose a descriptive English compound (Frostfangs) after checking earlier authors had not used the obvious ones [GRRM-WOT] **[partially verified: a 2023 fan-site article citing a YouTube interview that was not watched]**.

What to take: descriptive English compounds read as "translated" names; one place can have a name per people.

### 3.5 Not found

No primary statement on settlement naming was found for Pratchett or Jordan in this pass (§8).

---

## 4. Generators and games

### 4.1 Azgaar's Fantasy Map Generator

Read from the source at `master`, commit `7413629` (2026-11-16) [AZ-NAMES] [AZ-NB] [AZ-BURG].

- **Inputs.** The culture of the burg's cell only. Capitals take `Names.getCultureShort(culture)`, a shorter name; towns take `Names.getCulture(culture)` [AZ-BURG]. No terrain, river, port or size fact enters the name. The port, citadel, temple and the group (capital, city, fort, monastery, caravanserai, trading post, village, hamlet, town) are features of a burg, not of its name [AZ-BURG].
- **Method.** Each culture points to a name base: a comma-separated list of real or invented place-names with a minimum and maximum length and the letters allowed to double. There are 43 bases: 32 real-world (German, English, French … Mongolian) by Azgaar, ten fantasy bases by another author (Human Generic, Elven, Dark Elven, Dwarven, Goblin, Orc, Giant, Draconic, Arachnid, Serpents) and one more [AZ-NB]. `calculateChain` splits each name into pseudo-syllables and records which syllable follows a given last letter; `getBase` walks that chain until the word ends within the length limits, then removes doubled letters not in `d`, three identical letters in a row and `ae`, and capitalises after spaces and hyphens [AZ-NAMES].
- **States from cities.** `getState` builds a state name from its capital's name: it strips endings such as *-berg* and *-ton*, then adds a base-specific suffix (*-ia*, *-land*, German *-land* in half the short names, Turkish *-yurt*, Nahuatl *-tlan*) through `validateSuffix`, which keeps a name that already ends in the suffix and trims letters at the seam [AZ-NAMES].
- **Variety and uniqueness.** The chain gives variety. No duplicate check was found for burg names [AZ-BURG].
- **Defects.** Names say nothing about the site. The random calls (`ra`, `P`, `rand`) draw from one stream in generation order (`@/utils/probability-utils`), so a burg's name depends on what was generated before it **[partially verified: the stream behind these helpers was not read]**.

### 4.2 Martin O'Leary: naming languages

Read from `language.js` at commit `4abc8d3` (2016) [MEWO-LANG] and the archived notes [MEWO-NOTES] [MEWO-TERRAIN].

- **Inputs.** A word class key (`'city'`, `'region'`); the map generator calls `makeName(lang, 'city')` for cities and towns and `makeName(lang, 'region')` for regions [MEWO-TERRAIN-JS]. No terrain fact enters a name; one language serves the whole map [MEWO-TERRAIN].
- **Method.** A random language has a consonant and vowel inventory, a syllable structure (such as optional sibilant, consonant, vowel, final), forbidden clusters and a spelling scheme. One syllable is one morpheme. Words are built from morphemes; a word of a class holds one morpheme of that class. The morpheme pools grow as they are drawn: the generic pool has ten "new morpheme" tokens, a class pool only one, so class morphemes recur often (a 3/4 chance of reuse once three exist). Names are one word, or two words joined directly or by a genitive particle, sometimes with a definite particle in front; names outside 5 to 12 characters are rejected [MEWO-NOTES] [MEWO-LANG].
- **Variety and uniqueness.** `makeName` rejects a name that contains, or is contained in, a name already made, and words are reused at word level so multi-word names reincorporate earlier names [MEWO-LANG] [MEWO-NOTES].
- **Defects.** The author dropped one language per region because readers could not see it [MEWO-NOTES]. All state (pools, used names) grows in call order, so the names depend on the order places are named [MEWO-LANG].

### 4.3 Battle for Wesnoth: the random-map village namer

Read from `src/generators/default_map_generator_job.cpp`, `data/english.cfg` and `data/core/macros/names.cfg` at commit `9544ef5` (2026-11-20) [WES-JOB] [WES-EN] [WES-NAMES].

- **Inputs.** The terrain and the named features next to the village. Rivers, lakes, bridges, roads, mountains, forests and swamps are named first, each from a base name; the village then looks at its six neighbouring hexes in a fixed order and takes the first that is a named road, river (river with bridge if there is one), forest, lake, mountain or swamp. If none, it counts terrain: six grassland gives `grassland`, two forest `forest`, one mountain `mountain_anon`, two hills `hill`; otherwise `village` [WES-JOB].
- **Method.** A context-free grammar per class joins `$base` (or the base name of the neighbouring feature) and a generic [WES-EN]:

  | Class | Generics |
  |---|---|
  | village | bury, bury, ham, ton, ville |
  | river | ham, ford, cross, " on {river}" |
  | river with bridge | bridge, bridge, ham, ton |
  | lake | harbor, port |
  | grassland | ham, ton, field |
  | forest | ham, ton, wood, " Forest" |
  | hill | ham, bury, ton, hill, crest |
  | mountain | mont, cliff, bury, ham |
  | road | 's Rest, 's Waypoint, bury, bury, ham, ham |
  | swamp | bury, ham, ton |

  The default base names are a list of 35 short stems (Bal, Cam, Corn, … Water, Wet, York) and a small grammar of prefix, vowel and final consonant (`C`/`F`/other prefixes, `a|e|o|i|u`, `l|m|rn|x|w|ld|ck|k|rk`) [WES-NAMES]. Repeating a generic in a list weights it.
- **Variety and uniqueness.** A per-map `used_names` set; the village name is drawn again while it is in the set, at most 30 draws in all, and the last draw is kept even if it is still a duplicate [WES-JOB].
- **What it shows.** A village by a named river reuses the river's base name, which is how real towns take river names. The structure is close to what §6 proposes. **Defects:** the used-names set and the shared random stream make names depend on the scan order of the map; English-only generics serve every faction.

### 4.4 Dwarf Fortress

Read from the wiki in raw wikitext [DF-LANG] [DF-40D] [DF-LT] [DF-ET] and the vanilla raws of a February 2020 mirror [DF-RAW].

- **Words with meaning.** All names come from one English vocabulary of 3,300 words (1,737 nouns, 698 verbs, 849 adjectives, 16 prefixes) under 2,195 roots [DF-LANG]. Each word lists the slots it may fill: front or rear of a compound, after "the", after "of" [DF-LT] [DF-40D].
- **Symbols per civilisation.** Words belong to symbol sets (ARTIFICE, EARTH, NATURE, FLOWERY, VIOLENT …) and to name sets for map features (NAME_FOREST, NAME_HILLS, NAME_LAKE, NAME_MOUNTAINS, NAME_SWAMP, NAME_REGION …) [DF-40D]. A civilisation's entity selects and culls symbols per kind of thing named (SITE, ROAD, BRIDGE, TEMPLE, WAR …) [DF-ET]. In the raws read, dwarves fill the rest from ARTIFICE and EARTH, elves from NATURE and FLOWERY, and humans name their sites from NAME_ENTITY_TOWN_FOUNDER; all three cull UGLY, NEGATIVE and similar sets [DF-RAW].
- **Translation.** Each race has a translation file mapping every word to a word of its language (`[T_WORD:MOUNTAIN:onol]`), so a name exists both in the race's language and in English [DF-40D].
- **Defects.** The wiki itself notes the system yields nonsense such as "The Lazy Sabre of Buttering" [DF-40D]. Words overlap between languages by chance [DF-LANG].
- **What it shows.** The meaning-first model: a name is a structure of meaning keys, rendered in a language per people. It is the same split as Tolkien's (§3.1).

### 4.5 Freeciv and Unciv: fixed lists per nation

**Freeciv** (commit `4cbb83a`, 2026-10-20) [FC-NAT] [FC-CT] [FC-NOR]. Each of its several hundred nations has an ordered list of real city names. An entry may carry terrain labels: `"Trondheim (river, grassland, ocean)"`, with `!` to negate; a label matches when the terrain is on or next to the city's tile, `river` only on the tile itself [FC-NAT]. `evaluate_city_name_priority` gives each unused name the priority `(index + 10) · (10 + rand(5))`, then multiplies by 1.4 for each label that does not match and divides by 1.4 for each that does; the lowest priority wins [FC-CT]. When the list is used up the server searches civil-war, parent and then random nations, and at last makes "City no. N" [FC-CT]. A game setting decides whether names must be unique per player or globally [FC-CT]. The guidelines ask for native forms, ordered by founding date and importance [FC-NAT].

**Unciv**, an open remake of Civilization V (commit `0d2032e`, 2026-11-20) [UNCIV]: `generateNewCityName` returns the first name of the nation's list that no living civilisation's city uses; a nation with the `BorrowsCityNames` unique then borrows the last unused name of another nation; otherwise it prefixes the list with "New", "Neo", "Nova" and similar, repeated up to ten times. The prefix list is from the translation template; the constant itself was not read **[partially verified]**.

What it shows: a fixed list gives perfect names and no variety; terrain labels are a cheap way to make a list site-aware; uniqueness again comes from a global used set.

### 4.6 Ultima Ratio Regum

Johnson's 2016 paper (DiGRA-FDG) [URR-PCG]: each civilisation gets a random set of syllables and preferred letters, with a strictness from 70 % to 100 %; a set of references built from the climate and terrain of its capital and later settlements, used in the names of cities and persons; and a procedurally generated "name archetype" per culture that combines references, syllables, hyphens and connecting particles (examples in the paper: Icelandic-like, Homeric, names with geographic, animal or plant references). The paper gives no formulas.

What it shows: geography enters names through a culture-level reference set, not through each site.

### 4.7 watabou's town generator

The open-source version of the Medieval Fantasy City Generator (`watabou/TownGeneratorOS`, last commit 2019-04-07) has no naming code: its sources are geometry, wards and rendering [WATA-TG]. How the later closed versions name towns was not checked (§8).

---

## 5. Summary

| Source | Inputs to the name | Method | Variety | Uniqueness | Order-independent? |
|---|---|---|---|---|---|
| Real English and German toponymy | Site, owner, function, language layer | Specific + generic; later affixes | Layers, many generics | None; affixes and qualifiers tell duplicates apart | – |
| Tolkien | People, meaning | Meaning-names translated; invented names kept | Language per people | – | – |
| Azgaar FMG | Culture | Syllable Markov over a base of real names | 43 bases | None found | No (shared stream) |
| O'Leary naming language | Word class (city, region) | Generated phonology, morpheme pools per class | Class morphemes recur | Global used list, substring check | No |
| Wesnoth | Neighbouring features and terrain | Grammar per site class: base + generic | Per-class generic lists | Per-map set, 30 retries | No |
| Dwarf Fortress | Civilisation symbols, kind of thing, region biome | Meaning-tagged words, translated per race | Large vocabulary | Not documented | No (world history order) |
| Freeciv | Nation, terrain labels | Fixed list scored by labels | List length | Game setting, global set | No |
| Unciv / Civilization | Nation | Fixed list in order, then New X | List length | Global used set | No |
| Ultima Ratio Regum | Culture syllables and homeland references | Name archetypes per culture | Archetype per culture | Not stated | Not stated |

---

## 6. Assessment: what fits this codebase

Everything in this section is my own engineering judgement.

### 6.1 What the codebase already gives

- **Three name styles with Markov models.** `NameGenerator` holds nine models (Common, Exotic, Dark × male, female, surname); a race picks a style with `NameStyle`. The surname models are the right source for a place specific: many British surnames are themselves place-names, and the Common reference surnames already look like toponyms (Sampton, Fenwickson, Hearents) [repo: NameGenerator-Reuse.md]. Drawing from a model with an `Sfc32` stream does not touch the model files, so the rule "never re-sort or regenerate them" holds.
- **A tested pattern for a name rule.** `RegionNameRules` (inputs) and `RegionNameGenerator` (weighted word lists and rules in code) seeded with `Hash4(seed, purpose, i, j)`, with a surname redrawn while it is longer than 12 characters, up to ten draws. The city rule can copy this shape.
- **Site facts that map onto real generics.** `ProspectFacts` has `Mouth`, `Harbour`, `Coast`, `River`, `Confluence`, `HeadOfNavigation`, `LakeAreaM2`; the patch has a biome and resource amounts (Salt, Stone, IronOre, Gold …); the zone has an elevation type; the factions have seats. Nearly every row of the table in §2.2 has a fact behind it.
- **What is missing.** Rivers, lakes, hills and forests have no names, so a town cannot take a river's name (Wesnoth's strongest trick, §4.3) and there is no "upon X" qualifier yet. There is no history, so *New* and *Old* have nothing to stand on.

### 6.2 Which approach fits

| Approach | Fit | Why |
|---|---|---|
| Fixed lists per race (Civilization, Freeciv) | Poor | 28 races, an unbounded world and up to 20 Prospects per region use up any list; a list needs a global used set |
| Pure Markov per race (Azgaar) | Partial | Already available through the surname models; but the name then says nothing about the site, and the Exotic and Dark surname models run long (9.1 % and 5.1 % over 12 characters) |
| Generated naming language (O'Leary, Dwarf Fortress translation) | Too large for now | Needs a phonology and a lexicon per style; growing morpheme pools are order-dependent unless every morpheme is a pure hash of (style, meaning) |
| Site-class grammar: specific + generic from facts (Wesnoth, real toponymy) | Best | Pure function of one Prospect; reuses the Markov models for the specific; the generic carries meaning a later LLM authoring step can use |

The recommendation is the fourth row, with the Markov surname model of the race style as the specific, and a stored meaning key so the name can be rendered or explained later (Tolkien's translated meaning-names, §3.1; Dwarf Fortress's meaning-first names, §4.4).

### 6.3 Rule a generator could take: the name of a city or a town

**Stage.** Settled, as part of the Stub of the city or town. The name reads only Planned data: of its own region, and in step 6 of its bordering regions, which the Settled stage already requires to be Planned. So it needs no new stage precondition.

**Which Prospects are named.** Every Prospect of the region, on show or not, so that a settlement founded later on a Prospect not on show keeps a name that was already fixed (society-generation.md section 5 keeps that slot free). Only the cities and towns carry the name in the Stub. For naming, a Prospect is the *capital* if it is the seat of the ruling faction, a *city* if it is for a city, else a *town*.

**Step 1: the style.** The name style of the first race of the Prospect's race list (section 5.1: the largest around it). The capital takes the style of the lead race of the ruling faction. *(Decision D3.)*

**Step 2: the generic.** Every class whose condition holds gets its weight; the kind of place always adds its own classes; one class is drawn by weight. Weights are constants in code to start with. The words below are the Common (English) lexicon and are illustrative.

| Class (meaning key) | Condition | Common generics |
|---|---|---|
| Mouth | `Mouth ≥ 8` (a navigable river meets the sea) | mouth |
| Haven | `Harbour` | haven, port, hithe |
| Shore | `Coast`, no harbour | ness, strand |
| Landing | `HeadOfNavigation` | hithe |
| Meeting | `Confluence ≥ 4` | meet |
| Ford | `2 ≤ River < 8` (a river that can be forded) | ford |
| Bridge | `River ≥ 8` | bridge |
| Mere | `LakeAreaM2 > 0` | mere, water |
| Hill | zone Hills or Mountains | don, hill (berg for Mountains) |
| Clearing | a forest biome | ley, hurst, wood |
| Field | a grassland, steppe or savanna biome | field |
| Salt | `Salt` amount > 0 on the patch | wich |
| Delve | IronOre, CopperOre, Tin, SilverAndLead or Gold deposit on the patch | delve |
| Church | an Order or a Temple-state faction seated here | minster, kirk |
| Fort | city or capital | bury, borough, chester |
| Farm | town | ton, ham, by, thorp, stead, worth |

**Step 3: the specific.** One of:

- a surname from the surname model of the style, drawn again while longer than a limit (8 characters suggested), up to ten draws, the last kept (the rule of the ruling house, section 4.10);
- for the capital of a region ruled by a house, in some share of cases, the surname of the ruling house (a manorial name, §2.4) — this ties the capital to the region name;
- for the Common style only, a short descriptive word from a list (Ash, Oak, Black, White, Red, Salt, Stone, Wolf, North, East …), the descriptive specific of §2.1.

**Step 4: the join.** If the specific already ends in a generic of the style's lexicon ("Sampton"), the specific is the whole name (Azgaar's `validateSuffix` does the same, §4.1). A letter doubled across the seam is written once. The first letter is a capital. A name outside 4 to 14 characters is drawn again from the same stream.

**Step 5: the affix.** A town with a Guild or a Chartered-town faction seated on it, or with a trade factor of 0.7 or more, gets *Market* in front in some share of cases (§2.4: *Chipping*, "a market"). No *New* or *Old*: there is no history to justify them yet.

**Step 6: unique within the region.** The Prospects are taken in slot order. A name already given to a Prospect of a lower slot is drawn again from the Prospect's own stream (steps 2 to 5), up to five times; if it still clashes, a town takes *Little*, *Upper* or *Nether* in front of it and a city *Great* (§2.4). This step reads the region only, so the result does not depend on the order regions are generated in.

**Step 7: fewer clashes with the bordering regions (optional, decision D5).** For each bordering region, compute the step-6 names of its Prospects from its Planned data (a pure function of that region, so the same whenever it is computed). Where a name of this region equals one of them, the Prospect with the larger priority `hash4(seed, 0x4c02, patch i, patch j)` (ties by patch key) keeps it; the other draws again as in step 6, also avoiding the step-6 names of all bordering regions. This is one round: a redrawn name is not checked against redrawn names two regions away, so a rare duplicate remains. It is order-independent because it compares only step-6 names, which every region can compute for its neighbours. Duplicates further away are accepted, as in real toponymy (§2.4); a display can add the region name as a qualifier ("Ashford in the Western Watson Kingdom").

**Random numbers.**

- One sfc32 stream per Prospect, seeded with `hash4(seed, 0x4c01, I, J)`, `(I, J)` the root cell of the Prospect's patch (as purpose 0x4a03 does in section 5.1). Its numbers are taken in a fixed order: class, specific (with its redraws), affix, then the redraws of steps 6 and 7.
- The house surname is the one of section 4.10 (purpose 0x4b08), not a new draw.
- 0x4c03 can be reserved for the generated-lexicon option of decision D2.
- The purposes 0x4c01 to 0x4c03 are free: no `0x4c` value appears in `src/` or `docs/`, and the computed families (0x5e10 + resource, 0x5e100 + crop, 0x5e400 + kind, 0x5e800 + kind, 0xf10000 + kind, 0xfa0000 + kind) do not reach them. Taken so far: 0x0c01, 0x10c1, 0x1a01–02, 0x3a01–02, 0x4a01–03, 0x4b01–08, 0x5e01–05, 0x7e01, 0x7e02, 0x7e04, 0x7e11, 0x7e12, 0xb101, 0xf10a, 0xfa0a and the families above.

**What is stored.** In `Settlement`: the name, and the meaning key of its generic (Mouth, Ford, Fort …). The meaning key lets an authoring step describe the name ("the ford of the Sampton family") and lets another race render its own name for the same place later, which is the multi-name pattern Martin praises in Tolkien (§3.4).

**What is language-neutral and what is English.** The structure (meaning key, the specific drawn from a Markov model, the affix key) is neutral. The rendering of the generic and the affix is per style. For the Common style it is English, read as a translation of the human speech, as Tolkien's Shire names are (§3.1). For Exotic and Dark, see decision D2.

### 6.4 Uniqueness across the world

- A world-wide "used names" set cannot exist: the world is unbounded and a set filled as regions are generated would make names depend on the order of exploration. Every generator of §4 that guarantees uniqueness does exactly that, so none can be copied here.
- Identity does not need unique names: a city's identity is its region and slot (readme, Identity). A duplicate name is a presentation problem, not a data problem.
- Inside a region the names are unique (step 6). Against the bordering regions one order-independent round removes most clashes (step 7). Two regions apart a duplicate can stand, as Barton does dozens of times in England (§2.4).
- How often a clash happens before steps 6 and 7 was not measured. The surname models gave 19,531 (Dark) to 54,967 (Common) distinct names in 300,000 seeds, but the distribution is skewed towards common names, and a length limit shrinks it; a measurement on the four test seeds, like the one in section 4.10, should precede adoption.

### 6.5 Parameter Set

- Start with the word lists and weights in code, in a `SettlementNameGenerator` beside `RegionNameGenerator`, for the same reason: changing a word renames every world, and a test holds reference names. Moving the lists of both generators into the Parameter Set is one open item, already listed in section 4.10.
- A few numbers could be parameters in a new group, for example `Settlement.Names`: the longest specific (8), the name length limits (4 and 14), the share of capitals named after the ruling house, the trade factor for *Market* and its share. Each needs a `[Description]`, a line in `docs/default-parameter-set.json` and a regenerated schema.
- A per-race place-name lexicon (Northmen leaning to *-by* and *-thorp*, for instance) would be a new field on the race, which the readme lists; it is a later step.

### 6.6 Tests

- **Determinism:** the same foundation gives the same names; a name is a pure function of the seed (as `A_name_is_a_pure_function_of_the_generator_seed`).
- **Order independence:** covered by `Generation_does_not_depend_on_the_order_regions_are_explored_in` once the name is a field of `Settlement`, since that test compares the serialized `Settlement`. Step 7 needs it to approach from both sides of a border.
- **Reference names:** a table of seeds and Prospects with their names, so a change of a word list or of the stream order shows.
- **Facts to classes:** a Prospect with a navigable mouth can draw Mouth, one without water never draws Ford, a town never draws Fort.
- **Join rules:** no name ends with a generic twice ("Samptonton"), no doubled letter at the seam, every name within the length limits, no stray spaces.
- **Uniqueness:** no two Prospects of a region share a name; a measured clash rate across bordering regions on the four test seeds.
- **Styles:** a Prospect led by an Exotic race draws its specific from the Exotic surname model.

### 6.7 Docs that change

- `docs/society-generation.md`: a new section 5.3 "The name of a city or a town" with the rule, its parameters and purposes; a "Name" row in the Stub table of 5.2; the open items about city and town names removed from §5 and from the end of 4.10.
- `readme.md`: the "City and town" concept gains a sentence on its name (derived or authored, decision D1), and the Status paragraph lists it. The readme and the society doc must agree on whether the name is derived data in the Stub, and on its stage.
- `CLAUDE.md`: the list of specified sections of society-generation.md adds 5.3.
- `docs/RegionNameGenerator-Reuse.md`, "Not covered": a pointer from the unfinished `GenerateCityName` stub to section 5.3 (optional).
- `docs/default-parameter-set.json` and the generated schema, if parameters are added.

### 6.8 Effort and risk

- **Effort:** about two days. A rule class and a word-list class, a field on `Settlement`, a few lines in `SettlementBuilder` (and a read of the bordering regions' plans for step 7), the tests, section 5.3 and the readme lines, and a column in the demo's city rows. Step 7 adds about half a day.
- **Risk to existing names:** none. The new purposes do not touch any other draw, and the Markov models are only read.
- **Quality risk:** Markov surnames as specifics can give awkward or unfortunate words (the models have no content filter); long Exotic and Dark stems; and the Exotic and Dark lexicons must be written (decision D2).
- **Lock-in risk:** once names ship, every change of a word list or of the draw order renames the cities of every world; the reference-name test makes that visible.

---

## 7. Open decisions for the user

- **D1. Derived or authored.** Is the name of a city or a town derived data in its Stub, like a region name and a person name, or authored by the LLM when the city is Realised? The readme says persons and locations depend on LLMs for text; the region-name precedent says names are derived. Recommendation: derived, so a city has a name before it is authored.
- **D2. The language of Exotic and Dark names.** (a) English generics for every race, read as a translation (simplest, matches the region names, which use the same lists for all races); (b) a short invented lexicon per style, one generic per meaning key, written once in code; (c) a generic per (style, meaning) generated once per foundation from the style's Markov model with purpose 0x4c03 (O'Leary's recurring class morphemes, made order-independent; quality untested). Recommendation: (b), with (a) as the first step if the lexicons are not ready.
- **D3. Whose race names the place.** The largest race around the Prospect (first of its race list), the lead race of the faction seated on it, or for the capital the lead race of the ruling faction. Recommendation: the largest race, and the ruling faction's lead race for the capital.
- **D4. Name every Prospect or only those on show.** Recommendation: every Prospect, so a later settlement keeps a fixed name.
- **D5. Step 7.** Check against the bordering regions in one order-independent round, or accept clashes across borders and rely on a region qualifier in the display.
- **D6. The ruling house in the capital's name.** The surname of the ruling house is not yet a fact of the faction Stub (section 4.10, open); using it in the capital's name makes the families of section 6 carry it twice.
- **D7. Real names.** The Common surname model returns real British surnames, so some city names will be real English place-names or near ones. Acceptable?
- **D8. Word lists in code or in the Parameter Set**, decided together with the region-name lists (section 4.10).

---

## 8. What could not be verified

- **Crusader Kings III.** The wiki host served a bot challenge; the `cultural_names` blocks of landed titles and the per-culture name lists were not read. That CK3 renames a title by the holder's culture is **[unverified]**.
- **Civilization (Firaxis).** No game file or official page was read. That the commercial games use fixed per-civilisation lists in order is inferred from Unciv's code and its comment citing the fandom wiki [UNCIV] **[partially verified]**.
- **Tolkien.** *Nomenclature*, Appendix F and the *Letters* were not read directly; everything in §3.1 comes through Wikipedia and Tolkien Gateway.
- **Le Guin.** Only an excerpt of "Dreams Must Explain Themselves" and the abstract of Robinson's article were read. No construction rule of hers for place names was found.
- **Martin.** Through a fan-site article; the interview video was not watched.
- **Pratchett, Jordan, Caves of Qud, Mount & Blade, the closed versions of watabou's generators.** Nothing primary on settlement naming was found or read in this pass.
- **English place-name dictionaries.** Watts' *Cambridge Dictionary of English Place-Names*, Ekwall and the Key to English Place-Names entries were not read (the Key is a JavaScript map that gave no data to the fetch tool). The glosses come from the SNSBI introduction, which is by English Place-Name Society scholars, and from Wikipedia. No frequency counts of the elements (how many *-ton* names, for instance) were found; the common claim that Newton is the most frequent English place-name is **[unverified]**.
- **Salt *-wich* and market *port*.** See §2.2; both **[unverified]**.
- **Dwarf Fortress site names.** The raws are a 2020 mirror; how the current version builds the full site name from the selected symbols (the order of compound and "the … of …" forms) was not traced in code.
- **Azgaar's random stream.** That `ra`, `P` and `rand` draw from one seeded stream in generation order was not read in `probability-utils`.
- **Clash rates.** No measurement was made for the proposed rule; every figure in §6.4 is about the person-name models, not about place names.

---

## 9. Sources

### 9.1 Toponymy

- **SNSBI** — Diana Whaley and Keith Briggs, "Introducing the place-names of England", Society for Name Studies in Britain and Ireland (2024). https://www.snsbi.org.uk/exploring-names/place-names-of-england/
- **KEPN** — Institute for Name-Studies, University of Nottingham, *Key to English Place-Names* (front page only; no entries read). https://kepn.nottingham.ac.uk; HALOGEN query interface https://halogen.le.ac.uk/query/kepn
- **WP-UKGEN** — Wikipedia, "List of generic forms in place names in the United Kingdom and Ireland" (read to character 100,000 of 147,000). https://en.wikipedia.org/wiki/List_of_generic_forms_in_place_names_in_the_United_Kingdom_and_Ireland
- **WP-DE** — Wikipedia, "German placename etymology". https://en.wikipedia.org/wiki/German_placename_etymology
- The Ipswich and Norwich examples for *wīc* are from a search-index excerpt of a Nottinghamshire Historic Environment Record page, not read in full: https://her.nottinghamshire.gov.uk/whats-new/what-can-a-place-name-reveal-about-its-history **[partially verified]**

### 9.2 Fantasy literature

- **TOL-TR** — Wikipedia, "Translating *The Lord of the Rings*" (read to character 100,000 of 145,000), on Appendix F2, Letters 190 and 204, and *Nomenclature* (Hammond and Scull 2005, pp. 750–782). https://en.wikipedia.org/wiki/Translating_The_Lord_of_the_Rings **[partially verified]**
- **TG-GUIDE** — Tolkien Gateway, "Guide to the Names in The Lord of the Rings". https://tolkiengateway.net/wiki/Guide_to_the_Names_in_The_Lord_of_the_Rings
- **TG-BREE** — Tolkien Gateway, "Bree", etymology. https://tolkiengateway.net/wiki/Bree **[partially verified]**
- **TG-HOB** — Tolkien Gateway, "Hobbiton", etymology. https://tolkiengateway.net/wiki/Hobbiton
- **LG-DREAMS** — Ursula K. Le Guin, "Dreams Must Explain Themselves" (1973), in *The Language of the Night* (1979), known through search excerpts quoting it. https://www.risingshadow.net/book/1117-dreams-must-explain-themselves **[partially verified]**
- **LG-ROB** — Christopher L. Robinson, "Onomaturgy vs. Onomastics: An Introduction to the Namecraft of Ursula K. Le Guin", *Names* 59 (3), 2011, doi:10.1179/002777311X12976826704208. https://ans-names.pitt.edu/ans/article/view/1926 **[partially verified: abstract]**
- **WE-724** — *Writing Excuses* 7.24, "Project in Depth: *The Way of Kings*" (about 10 June 2012). https://writingexcuses.com/?p=2036
- **GRRM-WOT** — Aryan Singh, "George R. R. Martin discusses how he names locations in Game of Thrones", Wiki of Thrones, 30 August 2023, citing https://www.youtube.com/watch?v=rW6YlZbTLqE. https://wikiofthrones.com/george-r-r-martin-naming-places-game-of-thrones **[partially verified]**

### 9.3 Generators and games

- **AZ-NAMES** — Azgaar, *Fantasy Map Generator*, `src/model/society/names-generator.ts` at commit `7413629` (2026-11-16): `calculateChain`, `getBase`, `getCultureShort`, `validateSuffix`, `getState`. https://github.com/Azgaar/Fantasy-Map-Generator/blob/74136292b8012abe0d57a11ac7780ddc43d9433d/src/model/society/names-generator.ts
- **AZ-NB** — same repository, `src/data/name-bases.ts`: the 43 name bases. https://github.com/Azgaar/Fantasy-Map-Generator/blob/74136292b8012abe0d57a11ac7780ddc43d9433d/src/data/name-bases.ts
- **AZ-BURG** — same repository, `src/model/political/burgs-generator.ts`: capitals and towns, burg groups. https://github.com/Azgaar/Fantasy-Map-Generator/blob/74136292b8012abe0d57a11ac7780ddc43d9433d/src/model/political/burgs-generator.ts
- **MEWO-LANG** — Martin O'Leary, `naming-language`, `language.js` at commit `4abc8d3` (2016-07-29): `getMorpheme`, `makeWord`, `getWord`, `makeName`. https://github.com/mewo2/naming-language/blob/4abc8d392f9e536f86d546d446201002528cffc0/language.js
- **MEWO-NOTES** — Martin O'Leary, "Generating naming languages", read from the Internet Archive snapshot of 29 August 2024. https://web.archive.org/web/20240829092747/https://mewo2.com/notes/naming-language/
- **MEWO-TERRAIN** — Martin O'Leary, "Generating fantasy maps", read from the Internet Archive snapshot of 13 August 2024. https://web.archive.org/web/20240813023130/https://mewo2.com/notes/terrain/
- **MEWO-TERRAIN-JS** — `mewo2/terrain`, `terrain.js` at `master` (last commit `a2511d6`, 2016-08-15): `makeName(lang, 'city')` and `makeName(lang, 'region')`. https://github.com/mewo2/terrain/blob/master/terrain.js
- **WES-JOB** — Battle for Wesnoth, `src/generators/default_map_generator_job.cpp` at commit `9544ef5` (2026-11-20): feature naming and village naming. https://github.com/wesnoth/wesnoth/blob/9544ef5d7c6e5cf25cf06e77bba5833ea74bfb42/src/generators/default_map_generator_job.cpp
- **WES-EN** — same repository, `data/english.cfg`: `[naming]` and `[village_naming]` grammars. https://github.com/wesnoth/wesnoth/blob/9544ef5d7c6e5cf25cf06e77bba5833ea74bfb42/data/english.cfg
- **WES-NAMES** — same repository, `data/core/macros/names.cfg`: `VILLAGE_NAMES`. https://github.com/wesnoth/wesnoth/blob/9544ef5d7c6e5cf25cf06e77bba5833ea74bfb42/data/core/macros/names.cfg
- **DF-LANG** — Dwarf Fortress Wiki, "DF2014:Language" (raw wikitext). https://dwarffortresswiki.org/index.php/DF2014:Language
- **DF-40D** — Dwarf Fortress Wiki, "40d:Language" (raw wikitext). https://dwarffortresswiki.org/index.php/40d:Language
- **DF-LT** — Dwarf Fortress Wiki, "DF2014:Language token" (raw wikitext). https://dwarffortresswiki.org/index.php/DF2014:Language_token
- **DF-ET** — Dwarf Fortress Wiki, "DF2014:Entity token" (raw wikitext): `SELECT_SYMBOL`, `SUBSELECT_SYMBOL`, `CULL_SYMBOL`, `TRANSLATION`. https://dwarffortresswiki.org/index.php/DF2014:Entity_token
- **DF-RAW** — Dwarf Fortress vanilla raws, `objects/entity_default.txt`, mirror by BenLubar (last change February 2020). https://github.com/BenLubar/raws/blob/master/objects/entity_default.txt
- **FC-NAT** — Freeciv, `doc/README.nations` at commit `4cbb83a` (2026-10-20): "Cities" and "Natural city names". https://github.com/freeciv/freeciv/blob/4cbb83a065439a60e3b1234b1964688cc72d5d40/doc/README.nations
- **FC-CT** — same repository, `server/citytools.c`: `evaluate_city_name_priority`, `search_for_city_name`, `is_allowed_city_name`, `city_name_suggestion`. https://github.com/freeciv/freeciv/blob/4cbb83a065439a60e3b1234b1964688cc72d5d40/server/citytools.c
- **FC-NOR** — same repository, `data/nation/norwegian.ruleset`: labelled city list. https://github.com/freeciv/freeciv/blob/4cbb83a065439a60e3b1234b1964688cc72d5d40/data/nation/norwegian.ruleset
- **UNCIV** — Unciv, `core/src/com/unciv/logic/city/managers/CityFounder.kt` at `master` (commit `0d2032e`, 2026-11-20): `generateNewCityName`, `borrowCityName`. https://github.com/yairm210/Unciv/blob/master/core/src/com/unciv/logic/city/managers/CityFounder.kt
- **URR-PCG** — Mark R. Johnson, "Procedural Generation of Linguistics, Dialects, Naming Conventions and Spoken Sentences", paper for DiGRA-FDG 2016 (PCG workshop archive). https://pcgworkshop.com/archive/johnson2016procedural.pdf
- **WATA-TG** — watabou, `TownGeneratorOS` (Haxe source of the Medieval Fantasy City Generator, last commit 2019-04-07). https://github.com/watabou/TownGeneratorOS

### 9.4 This repository

- `docs/NameGenerator-Reuse.md` (the nine models, reference names, distinct names per 300,000 seeds), `docs/RegionNameGenerator-Reuse.md` ("Not covered": the `GenerateCityName` stub), `docs/society-generation.md` sections 2, 4.10, 5.1 and 5.2, `docs/map-generation.md` section 15, `src/Gameworld.Generation/Names/`, `src/Gameworld.Domain/Prospects.cs`, `src/Gameworld.Domain/Region.cs`, `src/Gameworld.Generation/Stages/SettlementBuilder.cs`, `tests/Gameworld.Tests/GenerationTests.cs`, at commit `f370cb6`.
