# Races: Recurring Humanlike Peoples in RPGs, Fantasy Settings and Strategy Games, as Input for the Generator

*Research date: 2026-11-14. Scope: which **sapient, people-forming races** computer RPGs, tabletop settings, fantasy strategy games and colony or city builders use again and again, reduced to generic **race archetypes** that a generator can place in a generated world. §1 states the method and counts how often each archetype occurs. §2 sets out the axes that tell races apart. §3 is the curated list of 41 archetypes (29 in full plus a table of human cultural variants, and 12 rarer peoples in a short table), each with appearance, lifespan, temperament, society, economy, magic, relations, where it lives (in the readme's terms: biome, elevation type, bedrock, coast and river, the Supernatural Field), the resources it lives on, its settlement style, its variants and its sources. §4 is the race × biome and race × terrain matrix, with the recurring race relations. §5 is a design note on what a generator needs per race and how races could spread over regions, factions, families and persons, with the open questions. §6 lists the sources. The sibling note [factions.md](factions.md) covers the political groups; this note covers the peoples those groups are made of, and points to factions.md archetypes instead of repeating them.*

**How to read the citations**

- Each claim cites the source that owns it, by key in square brackets. The keys are listed in §6. The order of trust is the same as in `factions.md`: official rules text and developer material (SRD 5.1 and 5.2, Archives of Nethys, dev diaries), then established community wikis, then press and guides.
- **[partially verified]** means the source exists and is cited, but the claim was confirmed only from a search-index excerpt or a secondary source, not by reading the page itself. In this pass UESP (403), the Dwarf Fortress wiki and its catsplode mirror (429), Fandom (402: Total War: Warhammer, Age of Wonders 3, TV Tropes), the Hooded Horse wiki for Against the Storm (403), dnd-wiki.org (403) and the Songs of Syx wiki (connection aborted) refused the fetch tool. Elder Scrolls, Dwarf Fortress, Total War, Age of Wonders 3, Against the Storm, Songs of Syx, Witcher, Baldur's Gate 3 and Kenshi claims are at this level.
- **[unverified]** means general knowledge of the game or setting that was not checked against any source in this pass. In the frequency table of §1.3 an asterisk marks the same thing for one roster entry.
- Proprietary race names (Khajiit, Bosmer, Qunari, Tauren and the like) appear only as **evidence**. The archetype names are generic and are the recommended names for the generator. No archetype is a copy of one game's race.
- Everything under **Base**, **Resources**, **Settlement density**, the matrix values in §4.1–§4.2, and every **Design note**, is my own engineering judgement: it maps the archetype onto this repo's concepts and is not a sourced claim, even where a sourced claim stands next to it.

**Hooks in this repo** (working tree at commit `3a9c381`)

| Hook | Where | What it gives a race pass |
|---|---|---|
| Person | `readme.md`, "Person"; "Entity Generation States" | The Stub holds "name, age, sex, role, family links, faction membership and home". Race is not in that list yet; it would be one more Stub fact |
| Family | `readme.md`, "Family" | "Typically living together in a household. Can include servants or slaves", so a household can mix races |
| Faction | `readme.md`, "Faction"; `factions.md` §3 | A group in one region. factions.md H3–H6 (dwarven hold, beastfolk tribe, elven enclave, goblin warband) already assume non-human peoples |
| Open question | `factions.md` §5.5 item 4 | Whether races are factions or a separate "people" layer under factions is not decided |
| Region stage | `readme.md`, "Region Generation Stages" | Factions and Prospects come in **Planned**; actual cities, families and persons in **Settled**. A race map would have to be ready by Planned |
| Patch biome | `map-generation.md` §13.2 | 15 land biomes plus Mangrove, from temperature belt and aridity. The estimated biome (typical height per elevation type) is what Planned reads |
| Zone | `map-generation.md` §8, §14.1 | Elevation type (Ocean, Flat Land, Lowland, Hills, Mountains) and bedrock (`shield`, `foldBelt`, `granite`, `volcanic`, `basalt`, `ophiolite`, `carbonate`, `clastic`, `alluvium`) |
| Resources | `map-generation.md` §14 | 24 raw resources per patch with an amount in [0, 1]; `food` and `wealth` summaries (§14.5) |
| Water site | `map-generation.md` §14.1 | `river`, `mouth`, `lake`, `harbour`, coastal flag; navigable river from 8 |
| Fields | `readme.md`, "Fields" | Prosperity and Supernatural, sampled anywhere, never stored |
| Name generator | `docs/NameGenerator-Reuse.md` | Three name styles: Common (`GB_` models), Exotic (`elven_`), Dark (`darkelve_`). The doc says the repo "needs its own mapping" from race, culture or faction to a style |

---

## 1. Overview and method

### 1.1 Scope

A race here is a **sapient people that forms households and communities**: it has families, speaks, builds or at least camps, and can be a member of a faction. That excludes monsters that are only beasts (dragons, wolves), lone spirits, and gods. It includes peoples that the sources mostly use as enemies (orcs, gnolls, fishfolk), because a world generator needs their villages too. Humanlike means two arms, speech and tools, not human shape: centaurs, merfolk and lizardfolk are in.

### 1.2 Sample

Twenty **race rosters**, chosen to cover the genres the task names. Each counts once in the frequency table:

| Code | Roster | Genre |
|---|---|---|
| TOL | J. R. R. Tolkien, Middle-earth | the root source |
| S51 | D&D 5e SRD 5.1 (player races and monster entries) | tabletop |
| S52 | D&D SRD 5.2 / 2024 Free Rules (species) | tabletop |
| PF2 | Pathfinder 2e (Archives of Nethys ancestries) | tabletop |
| TES | The Elder Scrolls (Morrowind, Oblivion, Skyrim) | CRPG |
| WOW | Warcraft / World of Warcraft | CRPG, RTS |
| DA | Dragon Age | CRPG |
| WIT | The Witcher | CRPG |
| BG3 | Baldur's Gate 3 | CRPG |
| POE | Pillars of Eternity I–II | CRPG |
| DOS2 | Divinity: Original Sin 2 | CRPG |
| ARC | Arcanum | CRPG |
| AOW | Age of Wonders (1 and 3) | 4X |
| MOM | Master of Magic (1994 and 2022) | 4X |
| H3 | Heroes of Might and Magic III | strategy |
| DF | Dwarf Fortress | colony sim, world generator |
| TWW | Total War: Warhammer | strategy |
| ATS | Against the Storm | city builder |
| RW | RimWorld (xenotypes) | colony sim |
| KEN | Kenshi | sandbox RPG |

**Supporting sources, not counted:** Stellaris (species classes and climate habitability, a taxonomy reference only) [ST-SP] [ST-HAB]; Songs of Syx (race climate preferences) [SOS] [partially verified]; Endless Legend minor factions [EL-MF]; Mount & Blade II (human cultures only) [MB2-MF] [partially verified].

**Not surveyed in this pass:** Warlock: Master of the Arcane, Majesty, Endless Space. Their rosters are close to Age of Wonders and Master of Magic as far as I know [unverified].

**Method.** For each roster I listed the peoples and noted size, lifespan, habitat, society and who they fight. I grouped peoples that do the same job in their world into one archetype, kept an archetype in full (§3, A–I) if it occurs in at least three rosters or fills a niche no other archetype covers (ratfolk in towns, fishfolk at sea), and put the rest in the short table §3.J.

### 1.3 Frequency: how typical is each archetype

`*` marks a roster entry from general knowledge [unverified]; all others are backed by a cited source in §3.

| Archetype | Rosters | Count /20 |
|---|---|---|
| A1 Human | all twenty | 20 |
| B1 Elf | TOL, S51, S52, PF2, TES, WOW, DA, WIT, BG3, POE, DOS2, ARC, AOW, MOM, H3, DF, TWW* | 17 |
| B3 Dwarf | TOL, S51, S52, PF2, TES* (vanished), WOW, DA, WIT, BG3, POE, DOS2, ARC, AOW, MOM, H3, DF, TWW* | 17 |
| C1 Orc | TOL, S51, S52, PF2, TES, WOW, BG3, ARC (half-orc), AOW, MOM, H3, TWW* | 12 |
| C2 Goblin | TOL, S51, PF2, TES*, WOW, BG3*, AOW, H3, DF, TWW* | 10 |
| B4 Halfling | TOL, S51, S52, PF2, WIT, BG3, ARC, AOW, MOM | 9 |
| F1 Lizardfolk | S51, PF2, TES, DOS2, AOW, MOM, H3, TWW*, ATS | 9 |
| B2 Dark elf | S51, S52, TES*, BG3, AOW, MOM, TWW* | 7 |
| B5 Gnome | S51, S52, PF2, WOW, WIT, BG3, ARC | 7 |
| H3 Dragonkin | S51, S52, PF2, BG3, AOW, MOM*, WOW | 7 |
| I1 Undead people | PF2, WOW, DOS2, AOW, H3, TWW* | 6 |
| E1 Giant-blooded | S52, PF2, DA, POE, ARC (half-ogre) | 5 |
| H1 Fiend-touched / H2 other planetouched | S51, S52, PF2, BG3, POE | 5 |
| C5 Gnoll | S51, PF2, MOM*, H3, BG3* | 5 |
| D1 Half-elf | S51, PF2, BG3, ARC | 4 |
| D2 Half-orc | S51, PF2, BG3, ARC | 4 |
| C4 Kobold | S51, PF2, WOW*, DF | 4 |
| E2 Ogre | S51, WOW*, H3, TWW* | 4 |
| E3 Troll | TOL, S51, WOW, MOM | 4 |
| F5 Bull-folk | S51, PF2, WOW, H3 | 4 |
| F6 Centaur | S51, PF2, WOW*, H3 | 4 |
| C3 Hobgoblin | S51, PF2, BG3* | 3 |
| F2 Catfolk | PF2, TES, AOW | 3 |
| F4 Birdfolk | PF2, H3, ATS* | 3 |
| G1 Merfolk | S51, PF2, WOW* | 3 |
| I2 Construct people | PF2, WOW, KEN | 3 |
| F3 Ratfolk | PF2, TWW* | 2 |
| G2 Fishfolk raiders | S51, WOW* | 2 |
| §3.J rarer peoples | 1–3 each | — |

### 1.4 Five findings that shape everything below

1. **The core roster is very stable, and it is Tolkien's.** Humans, elves and dwarves appear in 17–20 of 20 rosters; orcs and goblins in 10–12; halflings in 9. Tolkien supplies all five non-human members [TOLK-ELF] [TOLK-DW] [TOLK-HOB] [TOLK-ORC], and D&D, which adopted "halfling" as its name for hobbits [TOLK-HOB], passed them to the computer games. A generator that ships only these six already matches what players expect.
2. **Habitat follows race, and the strategy games make it a rule.** Heroes III gives every town a native terrain (Rampart with its elves, dwarves and centaurs on grass; Stronghold's goblins and orcs on rough ground; Fortress's gnolls and lizardmen in swamp; Dungeon's troglodytes and minotaurs underground) [H3-T] [H3-C]; Total War: Warhammer rates every settlement's climate as suitable, unpleasant or uninhabitable per race, with heavy penalties for the wrong climate [TWW-C]; Age of Wonders 3 gives each race liked, disliked and hated terrain and climate [AOW3-C] [partially verified]; Stellaris and Songs of Syx do the same with climate groups [ST-HAB] [SOS] [partially verified]. The readme's biomes and elevation types are enough to drive the same rule.
3. **One race, several biome variants.** Warcraft's trolls come in jungle, forest, ice, sand, swamp and cave kinds, each in its own biome [WOW-TRO]; Pillars of Eternity splits elves into wood and pale (cold-hardy) kinds, dwarves into mountain and boreal kinds, and humans into meadow, ocean and savannah folk [POE-SR] [POE-H] [partially verified]; Pathfinder's elves slowly take on the colouring of where they live [PF2-ELF]. So a race is best modelled as a base plus a variant that the biome picks.
4. **Humans are the generalists; their cultures carry the climate.** No source gives humans a native biome: SRD 5.2 puts them "throughout the multiverse" [S52], Dwarf Fortress starts them in grassland, savanna or shrubland [DF-CIV] [partially verified], and Songs of Syx makes humans one of the two most climate-tolerant races [SOS] [partially verified]. Where games want a cold, desert or steppe people, they make a human culture (Skyrim's northerners, Hammerfell's desert people, Bannerlord's steppe nomads) [TES] [MB2-MF] [partially verified] or a human xenotype (RimWorld's furred cold-adapted kind) [RW-X].
5. **Long life means few children.** The long-lived races breed slowly (Tolkien's dwarves have few women and children late [TOLK-DW]; elves live 600–750 years [PF2-ELF] [S51]), while goblins, hobgoblins, kobolds and orcs mature at 12–17 and die by 50–75 [PF2-GOBLIN] [PF2-HOB] [PF2-KOB] [PF2-ORC]. That is the lever for population density: the generator can tie density to lifespan instead of setting it by hand.

---

## 2. Taxonomy: the axes that tell races apart

Eight axes describe every archetype in §3. The **Axes** line of each entry gives the first six in this order: size · lifespan · fertility · magic · disposition · diet. Habitat and technology lean are given in the entry's **Base** and **Society** lines.

### 2.1 Size

| Value | Height | Examples |
|---|---|---|
| **Small** | 0.6–1.2 m | halflings 2–3 ft, gnomes 3–4 ft [S52]; goblins, kobolds [PF2-ANC] |
| **Medium** | 1.2–2.1 m | humans, elves 5–6 ft, dwarves 4–5 ft, orcs 6–7 ft [S52] |
| **Large** | 2.1–3.5 m | goliaths 7–8 ft [S52]; Warcraft's bull-folk 9–10 ft [WOW-TAU]; ogres, trolls |
| **Huge** | over 3.5 m | true giants; in the sources they are monsters rather than peoples [SRD51-M] |

Size changes a person's food need and a house's size; for the generator it is mostly flavour plus a food multiplier (Design note).

### 2.2 Lifespan

| Value | Age at death | Examples |
|---|---|---|
| **Short** | 50–80 | goblins (few reach 20 unprotected) [PF2-GOBLIN]; kobolds ~60, orcs ~60, catfolk 60–70, hobgoblins ~70 [PF2-KOB] [PF2-ORC] [PF2-CAT] [PF2-HOB]; half-orcs rarely over 75, dragonborn ~80 [S51] |
| **Human** | ~100 | humans under a century [S51]; Tolkien's hobbits ~100 [TOLK-HOB] |
| **Long** | 120–200 | lizardfolk ~120 [PF2-LIZ]; halflings ~150 [S51] [S52] [PF2-HALFLING]; half-elves 180+ [S51] |
| **Very long** | 250–500 | Tolkien's dwarves ~250 [TOLK-DW]; dwarves ~350 [S51] [S52] [PF2-DWARF]; gnomes 350–500 [S51] [PF2-GNOME] |
| **Ageless** | 600+ or none | elves 600–750 [PF2-ELF] [S51] [S52]; Tolkien's elves do not die of age [TOLK-ELF]; leshies do not age [PF2-LESHY]; undead and constructs |

### 2.3 Fertility

How fast a people grows. Few sources give numbers. Stellaris has explicit traits (Rapid Breeders: +10 % pop growth) [ST-BT]; Dwarf Fortress weights the mix of a civilisation by creature tokens [DF-BS] [partially verified]. Elsewhere it is implied by lifespan and maturity (§1.4 finding 5). Values: **low** (elves, dwarves, giant-kin), **human**, **high** (goblins, kobolds, ratfolk, orcs).

### 2.4 Magic affinity

How strongly a people is tied to magic, which the generator can read as a weight on the **Supernatural Field**. Arcanum makes this an axis of its own: elves cling to magic, dwarves lead technology, humans are split [ARC]. Values: **mundane** (dwarves in Arcanum, hobgoblins, who rarely practise magic [PF2-HOB]), **ordinary**, **gifted** (elves, gnomes, merfolk, whose rulers are expected to be strong sorcerers [PF2-MER]), **innate** (planetouched, fey, undead, leshies).

### 2.5 Disposition

Whether the sources usually make the race **playable** (a player race, friendly or neutral NPCs), **mixed** (playable in some rosters, enemy in others) or **hostile** (enemy-only or monster-entry only). SRD 5.1 and 5.2 split cleanly: nine player species versus monster entries such as bugbear, gnoll, goblin, hobgoblin, kobold, lizardfolk, merfolk, minotaur, orc and sahuagin [S51] [S52] [SRD51-M]. Pathfinder's rarity (common, uncommon, rare) gives a second grading [PF2-ANC]. Alignment was a third, now dropped in the 2024 rules; SRD 5.1 still gives tendencies (dwarves lawful, elves chaotic good, half-orcs often evil if raised among orcs) [S51].

### 2.6 Diet

Most peoples eat like humans. The exceptions matter for resources: **carnivore-leaning** (gnolls, lizardfolk, minotaurs whose feasts follow monthly hunts [PF2-MIN] [unverified for gnolls]); **plant-fed** (leshies feed like their plant, often by photosynthesis [PF2-LESHY]; Stellaris' phototrophs replace half their upkeep with energy [ST-BT]); **none** (undead and constructs; Kenshi's robot people need no food [KEN] [partially verified]).

### 2.7 Habitat

The axis the strategy games use for placement: native terrain [H3-T], climate suitability [TWW-C], liked and hated terrain [AOW3-C] [partially verified], climate groups [ST-HAB]. Each §3 entry states it in the repo's terms under **Base**. Four kinds:

| Kind | Lives | Examples |
|---|---|---|
| **Surface, biome-bound** | prefers a few biomes | elves (forest), lizardfolk (swamp), centaurs (plains) |
| **Elevation-bound** | prefers Hills and Mountains, any biome | dwarves, goliaths, minotaurs, birdfolk |
| **Underground** | under any biome, entrances in Hills and Mountains | dark elves, deep gnomes, kobolds, ratfolk warrens |
| **Aquatic** | Ocean zones, coasts | merfolk, fishfolk |
| **Follower** | lives where its host race lives | half-breeds, planetouched, halflings among humans |

### 2.8 Technology lean

Arcanum's magic-versus-technology split [ARC] and Warcraft's tinker races (gnomes, goblins, mechagnomes) [WOW-GNO] [WOW-GOB] show a second cultural axis: **traditional**, **craft** (dwarves), **inventive** (gnomes, goblins). It matters for which factions.md archetypes a race seeds (C4 artificers, D2 craft guild).

---

## 3. The curated archetypes

41 archetypes in ten groups: 29 in full (A–I, with A2 a table of human cultural variants rather than a race of its own) and 12 rarer peoples in a table (J). Every full entry uses the same fields. **Relations** name other races by their number and factions.md archetypes as "factions.md H3". The **Axes** line reads: size · lifespan · fertility · magic · disposition · diet.

Biomes in **Base** use the repo's names. The weight in brackets, 3 = native, 2 = good, 1 = tolerated, 0 = avoided, is the value carried into the matrix of §4.1; any biome not named is 1.

### Group A. Humans

#### A1. Human

The baseline people: medium-sized, short-lived next to the elder races, fast to spread, found in every climate, and organised in every way from tribe to empire.

- **Axes:** medium · human (under 100 years [S51]) · human · ordinary · playable · omnivore.
- **Names in games (evidence only):** humans, Men, folk, high men (an exalted human kind in Age of Wonders and Master of Magic [AOW1] [MOM-DD]), baseliners [RW-X].
- **Appearance and life:** the sources define humans by range rather than type: SRD 5.2 lets a human be Medium or Small and gives no lifespan [S52]; Pathfinder calls them "diverse, adaptable, and ambitious" [PF2-ANC].
- **Society and economy:** every form of state and every trade. Master of Magic's high men are highly structured and religious [MOM-DD]; Arcanum's humans are split between magic and technology [ARC].
- **Magic:** ordinary; some cultures magical, some not [ARC].
- **Relations:** neutral to friendly with most playable races; in several settings the newcomers who pushed the elder races back. The Witcher's humans arrived after a cataclysm, finding elves, dwarves and gnomes already there [WIT-R] [partially verified]; Dragon Age's elves live as second-class citizens in human cities [DAO]; Pathfinder's centaurs are open enemies of two human nations and clash with expansionists [PF2-CEN]. In factions.md terms humans fill every archetype from A1 to G4.
- **Base:** any land biome; the farmed biomes most: Temperate grassland (3), Woodland & shrubland (3), Temperate mixed forest (3), Temperate broadleaf forest (3); Cold steppe, Boreal forest, Temperate rain forest, Savanna, Tropical dry forest (2); Ice & snow (0). Flat Land and Lowland on rivers and coasts. The Prospect score of `map-generation.md` §15 already is a human siting rule.
- **Resources:** grain, produce, livestock, orchard, fish; then everything else by trade.
- **Settlement:** villages, towns and cities; the only race that fills every Prospect class. Density 1.0 (the reference).
- **Variants:** by culture and climate, see A2; by blood, see D1, D2, E1, H1, H2.
- **Playable or hostile:** playable in all twenty rosters.
- **Frequency:** 20/20.
- **Evidence:** [S51] [S52] [PF2-ANC] [DF-CIV] [partially verified] [RW-X] [MOM-DD] [ARC].

#### A2. Human cultural variants

Many rosters split humans by climate and way of life instead of adding more races. The variants below recur; each is a culture of A1 with its own biome lean, not a race of its own (Design note).

| Variant | Biome lean (repo terms) | Way of life | Evidence |
|---|---|---|---|
| **Northerners** | Boreal forest, Tundra, Cold steppe; Hills and Mountains | hardy, cold-resistant warrior-farmers, sea raiders | Skyrim's cold, mountainous north and its cold-resistant people [TES] [partially verified]; Bannerlord's land of fur [MB2-MF] [partially verified]; RimWorld's furred, cold-suited Yttakin [RW-X]; Pillars' meadow folk said to come from the far north [POE-H] [partially verified] |
| **Desert peoples** | Hot desert, Thorn scrub, Cold desert; oases (`river`, `lake`) | caravan traders, horse and camel riders, warrior clans | Hammerfell's Alik'r desert people [TES] [partially verified]; Bannerlord's Aserai who control the desert's few water sources [MB2-MF] [partially verified]; Age of Wonders' Azracs [AOW1]; Kenshi's Scorchlanders [KEN] [partially verified] |
| **Steppe nomads** | Cold steppe, Temperate grassland, Savanna; Flat Land | herders on horseback (factions.md H1) | Bannerlord's tribal nomads [MB2-MF] [partially verified]; Shadow Magic's steppe-roaming Nomads (search excerpt of a review) [AOW1]; Master of Magic's Nomads [unverified] |
| **Savanna and tropical folk** | Savanna, Tropical dry forest, Tropical rain forest | farmers and herders of the hot belt | Pillars' savannah folk, settled south of the equator for ten thousand years [POE-H] [partially verified] |
| **Seafarers** | coasts of any biome: `harbour`, `mouth`, Mangrove | sailors, traders, colonists (factions.md D3, D4, F5) | Pillars' ocean folk, dominant in a trading republic [POE-H] [partially verified]; Warcraft's large-built island humans [WOW-PR] |
| **Heartland farmers** | Temperate grassland, Woodland & shrubland, Temperate mixed and broadleaf forest; Flat Land, Lowland | the imperial or feudal core (factions.md A1, A2) | Pillars' meadow folk on the open plains [POE-H] [partially verified]; Bannerlord's horse-breeding western kingdom [MB2-MF] [partially verified] |
| **Highland and forest clans** | Hills, Temperate rain forest, Boreal forest | clan societies outside the kingdoms (factions.md H2) | Bannerlord's Battania [unverified for its terrain]; Master of Magic's Barbarians [unverified] |
| **Exalted humans** | as heartland | a magically ennobled or ancient human line | Age of Wonders' Highmen [AOW1]; Master of Magic's religious high men [MOM-DD]; Tolkien's Númenóreans [unverified] |

RimWorld shows the other way to vary humans: by body rather than culture. Its xenotypes include cave-suited diggers who dislike light, cold-adapted furred humans, pollution-tolerant humans, very tough slow Neanderthals and light-shy blood-drinkers [RW-X]. A generator can treat these as the same mechanism: a human variant with a habitat modifier.

### Group B. The elder races

#### B1. Elf

A tall, slender, very long-lived people of old forests: graceful, proud, magical, slow to trust, and often in retreat before younger races.

- **Axes:** medium · ageless (600–750 years [PF2-ELF] [S51] [S52]; undying in Tolkien [TOLK-ELF]) · low · gifted · playable · omnivore.
- **Names in games (evidence only):** elves, high elves, wood elves, Silvan, Bosmer, Altmer, kaldorei, Dalish, Aen Seidhe.
- **Appearance and life:** 5–6 ft [S52]; adult around 100 in D&D [S51], around 20 in body but a century in mind in Pathfinder [PF2-ELF]. Tolkien's elves fade or sail west rather than die [TOLK-ELF].
- **Society and economy:** individualist, secretive, tied to groves and kinship groups; they try many crafts before settling on one; scholarship, philosophy and magic are their renown [PF2-ELF]. Hunters, rangers, artists; little farming in most sources [unverified]. Dragon Age has both nomadic clans that keep the old ways and an urban underclass in human cities [DAO]; Warcraft's forest elves are druidic and keep a sentinel army [WOW-NE].
- **Magic:** gifted. Arcanum's elves practise magic almost exclusively [ARC]; Master of Magic's high elves have natural magical protection [MOM-DD].
- **Relations:** aloof towards humans and grieved by outliving them [PF2-ELF]; long estrangement from dwarves in Tolkien, with famous exceptions [TOLK-DW]; enemies of orcs and goblins. Pairs with factions.md H5 (elven enclave), B7 (druidic circle) and G4 (a dispossessed people's resistance); rival to D6 (frontier settlers).
- **Base:** Temperate broadleaf forest (3), Temperate rain forest (3), Temperate mixed forest (3); Boreal forest, Tropical rain forest (2); Temperate grassland, Woodland & shrubland, Tropical dry forest (1); Ice & snow, Tundra, Cold steppe, Cold desert, Hot desert, Thorn scrub, Savanna, Mangrove (0). Lowland and Hills; high Supernatural. Sourced leans: D&D elves are accustomed to twilit forests [S51] and wood elves live in primeval forests [S52]; Tolkien's realms are forests (Lothlórien, Mirkwood) and a coastal haven [TOLK-ELF]; Warcraft's are Kalimdor's forests [WOW-NE]; Skyrim's wood elves still hide in their forests [TES] [partially verified]; Master of Magic's high elves revere woodlands [MOM-DD]; Age of Wonders 3's high elves dislike tropical climate and hate blighted and volcanic land [AOW3-C] [partially verified].
- **Resources:** wild food & game, timber (fine woods), fibre, honey (stimulants & sugar), herbs; fantasy goods where the Supernatural Field is high (the readme's Elanor sap).
- **Settlement:** hidden forest retreats, tree towns, a few old coastal or valley cities; Dwarf Fortress elves live in the canopy of forest retreats [DF-SITE] [partially verified]. Density 0.2–0.3 (Design note).
- **Variants:**
  - **Wood elf**: deep temperate and tropical forest [S52] [POE-SR].
  - **High elf**: valley and coastal cities, more magic, more trade; Tolkien's Rivendell and Grey Havens [TOLK-ELF]; Warcraft's high elves are exiled nobles who sailed to another continent [WOW-NE].
  - **Pale or snow elf**: Boreal forest, Tundra; Pillars' pale elves resist both burn and freeze damage [POE-SR].
  - **Sea elf**: coasts and islands [unverified].
  - **Nomad elf**: wanders in carts between forests, as Dragon Age's clans do [DAO].
  - **Dark elf**: see B2.
- **Playable or hostile:** playable in all seventeen rosters that have them.
- **Frequency:** 17/20.
- **Evidence:** [TOLK-ELF] [S51] [S52] [PF2-ELF] [WOW-NE] [DAO] [ARC] [MOM-DD] [POE-SR] [H3-C] [AOW3-C] [partially verified] [TES] [partially verified] [DF-SITE] [partially verified].

#### B2. Dark elf

Elves of the deep places or the dark side of elven history: underground cities, cruel aristocracies, strong magic. In most rosters a separate race from B1.

- **Axes:** medium · ageless · low · gifted · mixed (playable in BG3, Age of Wonders, Master of Magic; villains elsewhere) · omnivore.
- **Names in games (evidence only):** drow, dark elves, Dunmer, duskwalkers of some settings.
- **Appearance and life:** as elves; dark or grey skin in most sources [unverified]. Master of Magic's dark elves are gifted in magic and unusually tough [MOM-DD].
- **Society and economy:** matriarchal or aristocratic cities, slave-holding, poison and spider cults in the D&D line [unverified]; Baldur's Gate 3 splits them by faith into two subraces [BG3] [partially verified].
- **Magic:** gifted to innate.
- **Relations:** enemies of surface elves (B1) and dwarves (B3); masters of goblins, kobolds and slaves. Pairs with factions.md B5 (forbidden cult), C2 (cabal) and F2 (assassins).
- **Base:** underground under any biome (biome weight 1 throughout), entrances in Hills and Mountains; high Supernatural. SRD 5.2 places drow usually in the Underdark, a few in rain forests and ruins [S52]. Master of Magic moves them to its second, more magical world [MOM-DD]. Skyrim's dark elves come from a volcanic land [unverified]; that variant would favour `volcanic` bedrock.
- **Resources:** what grows or is mined underground: stone, iron ore, silver & lead, gems; fungi (wild food); slaves and trade for the rest.
- **Settlement:** cavern cities; density 0.3 (Design note).
- **Variants:** **underground dark elf** (Underdark, the commonest); **ash or volcanic dark elf** (surface, volcanic bedrock) [unverified]; **corsair dark elf** (coastal raiders, Total War) [unverified].
- **Playable or hostile:** mixed.
- **Frequency:** 7/20.
- **Evidence:** [S52] [SRD51-M] [MOM-DD] [AOW1] [BG3] [partially verified].

#### B3. Dwarf

Short, broad, bearded, long-lived people of the mountains: miners, smiths, masons and builders who live in halls under the rock and trade metal for food.

- **Axes:** medium (4–5 ft [S52]) · very long (~350 years [S51] [S52] [PF2-DWARF]; ~250 in Tolkien [TOLK-DW]) · low (Tolkien: few women, children late [TOLK-DW]) · mundane to ordinary (technology over magic in Arcanum [ARC]; rune magic in others [unverified]) · playable · omnivore, meat and ale [unverified].
- **Names in games (evidence only):** dwarves, Khazad, Dwemer (vanished), Bronzebeards, Orzammar.
- **Appearance and life:** stocky, stubborn, fierce and devoted [PF2-ANC]; adult around 25–50 [PF2-DWARF] [S51].
- **Society and economy:** clans and clan-kingdoms with strong traditions and wide cultural gaps between clans [PF2-DWARF]; Dragon Age's underground realm has a rigid caste system [DAO]; Master of Magic's dwarves value mineral wealth and craft and follow the rule of law [MOM-DD]. Mining, stoneworking, metalworking and gem cutting [WOW-DWF] [TOLK-DW]; Tolkien's dwarves are lovers of stone and gems [TOLK-DW].
- **Magic:** low to ordinary; craft-bound.
- **Relations:** allied with gnomes (Warcraft: close allies who swapped stonework for engineering [WOW-GNO]; the Witcher: gnomes live with dwarves in the dwarven mountains [WIT-R] [partially verified]); trading partners of humans [WOW-DWF]; old enemies of orcs and goblins (Pathfinder's dwarven empire fell to orcs and goblinoids [PF2-DWARF]); cool towards elves [TOLK-DW]. Pairs with factions.md H3 (dwarven hold), D5 (miners' guild), D2 (craft guild), C4 (artificers).
- **Base:** elevation first: Mountains (3), Hills (3), Lowland (1), Flat Land (0). Biome second: Tundra, Boreal forest, Temperate mixed forest (2); Ice & snow, Cold steppe, Cold desert, Temperate grassland, Woodland & shrubland, Temperate broadleaf forest, Temperate rain forest (1); Hot desert, Thorn scrub, Savanna, Tropical dry forest, Tropical rain forest, Mangrove (0). Metal-bearing bedrock: `foldBelt`, `granite`, `volcanic`, `ophiolite`, `shield`. Sourced leans: underground life [S51] [S52]; Dwarf Fortress starts all dwarven civilisations in mountains and builds at the edge of ranges [DF-CIV] [partially verified]; Tolkien's halls under mountains [TOLK-DW]; Warcraft's mountain realm and a rival clan's city under another mountain [WOW-DWF]; the Witcher's mountain homeland famed for mines [WIT-R] [partially verified]; Age of Wonders 3's dwarves dislike tropical climate [AOW3-C] [partially verified].
- **Resources:** stone, iron ore, copper ore, tin, silver & lead, gold, gems & pearls, fuel (coal); they buy grain, livestock and timber.
- **Settlement:** halls under a mountain with a gate on a valley; small hillside villages ("hillocks" in Dwarf Fortress [DF-SITE] [partially verified]); quarters in human cities. Density 0.5 overall, high inside a hold (Design note).
- **Variants:**
  - **Mountain dwarf**: Mountains, fold belt and granite [POE-SR] [unverified for D&D's mountain dwarf in SRD 5.2, which names only community names [S52]].
  - **Hill dwarf**: Hills, farms as well as mines [S51].
  - **Boreal dwarf**: Boreal forest, Tundra [POE-SR].
  - **Deep dwarf**: far underground, dour or enslaved; SRD 5.1's duergar [SRD51-M]; Baldur's Gate 3's duergar subrace [BG3] [partially verified].
  - **Sky or gryphon dwarf**: highland clans who ride gryphons [WOW-DWF].
  - **Vanished dwarf**: ruins only, a lost civilisation (the Elder Scrolls' Dwemer) [unverified].
- **Playable or hostile:** playable.
- **Frequency:** 17/20.
- **Evidence:** [TOLK-DW] [S51] [S52] [PF2-DWARF] [WOW-DWF] [WOW-GNO] [DAO] [MOM-DD] [ARC] [H3-C] [DF-CIV] [partially verified] [WIT-R] [partially verified] [AOW3-C] [partially verified].

#### B4. Halfling

Small, cheerful, homely people of gentle farmland: farmers, cooks and innkeepers, quick and lucky, rarely rulers.

- **Axes:** small (2–3 ft [S52]; Tolkien ~3 ft 6 in [TOLK-HOB]) · long (~150 years [S51] [S52] [PF2-HALFLING]; ~100 in Tolkien [TOLK-HOB]) · human · ordinary (luck) · playable · omnivore, many meals [unverified].
- **Names in games (evidence only):** halflings, hobbits, lightfoot, stout, Harfoots.
- **Appearance and life:** resilient, curious, humorous [PF2-ANC]; adult at 20 [S51].
- **Society and economy:** farming and simple rural life [TOLK-HOB]; in Pathfinder few settlements are bigger than villages, many halflings live among humans in larger cities and do menial or service work, and some travel in small nomadic bands [PF2-HALFLING]. SRD 5.2 speaks of bucolic havens, with stout halflings underground and lightfoot halflings nomadic or living among taller folk [S52]. The Witcher's halflings live inside human economies [WIT-R] [partially verified].
- **Magic:** ordinary.
- **Relations:** friendly with most peoples [PF2-HALFLING]; live under human rule. Pairs with factions.md A5 (peasant commune) and D1–D2 (a craft or inn trade inside human towns); targets of F4 (bandits) and slavers (factions.md G4's liberators).
- **Base:** Temperate grassland (3), Woodland & shrubland (3); Temperate mixed and broadleaf forest (2); Cold steppe, Boreal forest, Savanna, Tropical dry forest (1); Ice & snow, Tundra, Cold desert, Hot desert, Thorn scrub, Mangrove (0). Flat Land, Lowland and Hills; rivers. Sourced leans: Tolkien's rolling hills, woods and riverbanks (see variants) [TOLK-HOB]; Age of Wonders 3's halflings hate arctic, blighted and volcanic land [AOW3-C] [partially verified].
- **Resources:** grain, produce, orchard, livestock, stimulants (tobacco-like pipeweed in Tolkien [unverified]), fish on rivers.
- **Settlement:** villages of burrows in hillsides and cottages [TOLK-HOB]; a halfling quarter in human towns [PF2-HALFLING]. Density 1.0–1.2 in their own country (Design note).
- **Variants (Tolkien's three breeds [TOLK-HOB]):**
  - **Hill halfling**: rolling hills, the most numerous and most agrarian (Woodland & shrubland, Temperate grassland, Hills).
  - **Woodland halfling**: woods, hunters and archers (Temperate mixed and broadleaf forest).
  - **River halfling**: riverbanks and marshes, the only boatmen (Lowland, `river`, `lake`).
  - **Wanderer**: nomadic bands on the roads [PF2-HALFLING] [S52].
- **Playable or hostile:** playable.
- **Frequency:** 9/20.
- **Evidence:** [TOLK-HOB] [S51] [S52] [PF2-HALFLING] [MOM-W] [AOW3-C] [partially verified] [WIT-R] [partially verified].

#### B5. Gnome

Small, curious, eccentric people: tinkers, illusionists and burrow-dwellers, often fey in origin, often living beside dwarves.

- **Axes:** small (3–4 ft [S52]; ~3 ft 4–6 in in Warcraft [WOW-GNO]) · very long (350–500 years [S51]; ~425 [S52]; ~400 [PF2-GNOME]) · human · gifted (illusion, fey) · playable · omnivore.
- **Names in games (evidence only):** gnomes, rock gnomes, forest gnomes, svirfneblin, tinkers.
- **Appearance and life:** short, hardy, curious and eccentric [PF2-ANC]. Pathfinder's gnomes came from a fey world and die of a wasting grey despair if they stop seeking new experiences [PF2-GNOME].
- **Society and economy:** small families, communal child-rearing [PF2-GNOME]; engineers and inventors in Warcraft, whose gnomes built an underground techno-city in the foothills beside the dwarves [WOW-GNO]; artisans, jewellers, alchemists [unverified].
- **Magic:** gifted; illusion and fey magic; inventive technology in Warcraft [WOW-GNO].
- **Relations:** close allies of dwarves [WOW-GNO] [WIT-R] [partially verified]; rivals of goblins in engineering [WOW-GOB]; liked by humans for their enthusiasm, found bewildering [PF2-GNOME]. Pairs with factions.md C4 (artificers) and C1 (mages' college).
- **Base:** Hills (3), Mountains (2), Lowland (1). Temperate broadleaf forest (3), Temperate mixed forest, Boreal forest, Woodland & shrubland (2); Hot desert, Ice & snow, Tropical rain forest, Mangrove (0). Sourced: forests and burrows [S52]; underground vision [S51]; foothills of a mountain range [WOW-GNO]; no surface homeland of their own in Pathfinder [PF2-GNOME].
- **Resources:** gems & pearls, copper ore, tin, crystal (for craft); wild food in forests.
- **Settlement:** burrow villages in wooded hills, workshops in dwarven or human cities. Density 0.3 (Design note).
- **Variants:** **rock or tinker gnome** (Hills, near dwarves, inventors) [S51] [S52]; **forest gnome** (Temperate broadleaf and mixed forest, illusionists, friends of animals) [S52]; **deep gnome** (underground, SRD 5.1's svirfneblin) [SRD51-M]; **clockwork gnome** (part-machine; Warcraft's mechagnomes) [WOW-PR].
- **Playable or hostile:** playable.
- **Frequency:** 7/20.
- **Evidence:** [S51] [S52] [PF2-GNOME] [WOW-GNO] [WOW-GOB] [ARC] [WIT-R] [partially verified] [BG3] [partially verified].

### Group C. Goblinoids and greenskins

#### C1. Orc

Big, strong, tusked people who honour strength and glory: in older sources a race of raiders bred for war; in newer ones a proud warrior culture that can be friend or foe.

- **Axes:** medium (6–7 ft [S52]) · short (~60 years, adult ~17 [PF2-ORC]) · high [unverified] · ordinary (shamans) · mixed (playable in SRD 5.2, Pathfinder, Warcraft and Age of Wonders; enemies in Tolkien, SRD 5.1 monsters, Heroes III) · omnivore, meat-heavy [unverified].
- **Names in games (evidence only):** orcs, Uruks, Orsimer, greenskins, the Horde.
- **Appearance and life:** proud and strong [PF2-ANC]. Tolkien's orcs are mostly small and shun daylight, with larger later breeds that tolerate the sun; Tolkien gave several contradictory origins (corrupted elves, corrupted men, made by the dark lord) [TOLK-ORC].
- **Society and economy:** communities called holds where leadership passes by proven strength and children are raised in common [PF2-ORC]; tribes in D&D [TOLK-ORC]; Master of Magic's orcs are moderate in all things and a good workforce [MOM-DD]. Herding, hunting, raiding, smithing for war [unverified].
- **Magic:** ordinary; shamanism [unverified].
- **Relations:** seen as brutes by others but loyal once won [PF2-ORC]; old enemies of dwarves and elves [PF2-DWARF] [TOLK-ORC]; can be allied to humans (Warcraft's Horde [WOW-PR]) or to centaurs [PF2-CEN]. Pairs with factions.md H6 (goblin warband), H1 (nomad horde), E1 (mercenaries).
- **Base:** Cold steppe (3); Temperate grassland, Woodland & shrubland, Cold desert, Hot desert, Thorn scrub, Savanna (2); Tundra, Boreal forest, Temperate mixed, broadleaf and rain forest, Tropical dry forest (1); Ice & snow, Tropical rain forest, Mangrove (0). Hills and Mountains (3), Flat Land (2); low Prosperity. Sourced: SRD 5.2 says orcs are suited to plains, caverns and seas [S52]; Tolkien's orcs live in mountains, caves and dark forests [TOLK-ORC]; D&D orcs often dwell underground [TOLK-ORC]; Heroes III's orcs are on rough ground [H3-T] [H3-C]; Skyrim's orcs have a mountain stronghold between two provinces [TES] [partially verified].
- **Resources:** livestock, horses & pack animals, wild food & game, iron ore; they take grain by raid or tribute.
- **Settlement:** fortified holds, war camps, tent camps of herding clans. Density 0.7 (Design note).
- **Variants:** **mountain orc** (Hills, Mountains, caves; the Tolkien line); **steppe orc** (Cold steppe, Temperate grassland, mounted; Warcraft's orcs in arid land [unverified]); **sea orc** (coastal raiders, after SRD 5.2's "seas") [S52]; **civilised orc** (a settled orc nation, Pathfinder) [unverified].
- **Playable or hostile:** mixed, trending playable in recent rules.
- **Frequency:** 12/20.
- **Evidence:** [TOLK-ORC] [S51] [S52] [PF2-ORC] [WOW-PR] [MOM-DD] [H3-C] [AOW1] [ARC] [TES] [partially verified].

#### C2. Goblin

Small, quick, fast-breeding people: scavengers, trappers and raiders in most sources, greedy traders and engineers in Warcraft.

- **Axes:** small · short (few reach 20 without protection [PF2-GOBLIN]) · high · ordinary · mixed (common ancestry in Pathfinder [PF2-ANC]; enemies in most others) · omnivore, anything [unverified].
- **Names in games (evidence only):** goblins, snagas (Tolkien's goblin = orc [TOLK-ORC]), greenskins.
- **Appearance and life:** energetic and scrappy, long maligned [PF2-ANC].
- **Society and economy:** small tribes under a strong leader, usually under a hundred, valuing play and creativity [PF2-GOBLIN]. Warcraft's goblins are traders, miners and engineers ruled by trade princes and cartels, neutral merchants to all [WOW-GOB]. Dwarf Fortress goblins build dark fortresses and take captives [DF-SITE] [DF-CIV] [partially verified].
- **Magic:** ordinary.
- **Relations:** distrust of taller peoples [PF2-GOBLIN]; enemies of dwarves (C2 and B3 are the classic pair, factions.md §4.1); servants or allies of orcs and hobgoblins (Heroes III's Stronghold puts goblins, wolf riders and orcs together [H3-C]); engineering rivals of gnomes [WOW-GOB]. Pairs with factions.md H6 (goblin warband), or for the merchant kind F3 (black network) and D1 (merchant guild).
- **Base:** Hills and Mountains (3), any elevation (1). Boreal forest, Temperate mixed and broadleaf forest, Woodland & shrubland, Temperate grassland, Thorn scrub (2); Ice & snow, Mangrove (0); other biomes 1. Sourced: rough ground [H3-T]; Age of Wonders 3's goblins dislike arctic and tropical climate and hate volcanic land [AOW3-C] [partially verified]; Warcraft's goblins come from an island with a volcano and a rich ore deposit [WOW-GOB].
- **Resources:** whatever is near; iron ore and fuel for the engineering kind; captives.
- **Settlement:** warrens, caves, ruins, shanty towns, dark fortresses. Density 1.5 (Design note).
- **Variants:** **cave goblin** (Hills, Mountains, underground); **forest goblin** (forests, trappers); **merchant goblin** (ports and trade towns, cartels) [WOW-GOB]; **goblin under a hobgoblin or orc master** (a caste inside C1 or C3).
- **Playable or hostile:** mixed, mostly hostile.
- **Frequency:** 10/20.
- **Evidence:** [TOLK-ORC] [SRD51-M] [PF2-GOBLIN] [WOW-GOB] [H3-C] [AOW1] [DF-CIV] [partially verified] [AOW3-C] [partially verified].

#### C3. Hobgoblin

Taller, disciplined kin of goblins who organise everything as an army: drilled legions, chains of command, little magic.

- **Axes:** medium · short (adult ~14, ~70 years [PF2-HOB]) · high (walk and hold a weapon by age 1 [PF2-HOB]) · mundane (magic rarely practised, often disdained [PF2-HOB]) · mixed · omnivore.
- **Society and economy:** even farming collectives and trading houses are organised as regiments and companies; veterans lead [PF2-HOB]. Smithing; military art and marches [PF2-HOB].
- **Relations:** goblins are their smaller kin [PF2-HOB]; feared by humans as a pause before conquest [PF2-HOB]; masters of goblins (C2). Pairs with factions.md A1 (a militarist realm), E1 (mercenaries), H6.
- **Base:** Hills and Mountains (3); Temperate grassland, Woodland & shrubland, Cold steppe, Temperate mixed forest (2); Ice & snow, Tropical rain forest, Mangrove (0). Sourced: Pathfinder's hobgoblin nation lies in mountains [PF2-HOB]; a community wiki lists SRD 5.2's hobgoblin habitats as desert, forest, grassland, mountain, planar and underground [partially verified: search excerpt of dnd-wiki.org, which refused the fetch].
- **Resources:** iron ore, horses, grain from subject peoples.
- **Settlement:** fortified camps and garrison towns; a hobgoblin realm can hold a city Prospect. Density 0.8 (Design note).
- **Variants:** **legion** (a state); **mercenary cohort**.
- **Frequency:** 3/20. Kept in full because it is the one goblinoid that forms a state.
- **Evidence:** [PF2-HOB] [SRD51-M] [BG3] [unverified].

#### C4. Kobold

Small reptilian or dog-like trap-makers of mines and warrens, who serve a powerful patron, most often a dragon.

- **Axes:** small · short (adult ~12, ~60 years [PF2-KOB]) · high · ordinary, rising with the patron [PF2-KOB] · hostile to mixed · omnivore.
- **Society and economy:** cautious and secretive; respect hierarchy; laws follow their benefactor's character; famed crafters of defences and traps [PF2-KOB]. Dwarf Fortress kobolds skulk in caves and steal from nearby civilisations [DF-CIV] [partially verified].
- **Magic:** borrowed from the patron; eggs hatched near a dragon can grow its traits [PF2-KOB].
- **Relations:** servants of dragons and dragonkin (H3) [PF2-KOB]; thieves among dwarves and humans [DF-CIV] [partially verified]. Pairs with factions.md H6 and a dragon lair.
- **Base:** underground under Hills and Mountains (3); biome-neutral (1), Ice & snow (0). Mines and caves; near any metal deposit; high Supernatural near a patron.
- **Resources:** stone, iron ore, copper ore; scavenged goods.
- **Settlement:** warrens and mines. Density 1.5 (Design note).
- **Variants:** **dragon-sworn** (near a lair); **mine kobold** (in exhausted mines); **dog-faced kobold** (the older European folklore image) [unverified].
- **Frequency:** 4/20.
- **Evidence:** [PF2-KOB] [SRD51-M] [DF-CIV] [partially verified].

#### C5. Gnoll

Hyena-headed hunters and raiders of the dry plains: packs under the strongest, pragmatic, feared by farmers.

- **Axes:** medium to large · short [unverified] · high [unverified] · ordinary · mostly hostile (Pathfinder makes its hyena-folk playable [PF2-ANC]) · carnivore [unverified].
- **Names in games (evidence only):** gnolls, kholo.
- **Society and economy:** hunting packs and war bands; slaving and raiding [unverified]. Pathfinder calls its hyena-headed people pragmatic [PF2-ANC].
- **Relations:** enemies of farmers (A1, B4) and of centaurs on the same plains [unverified]. Pairs with factions.md F4 (bandit clan) and H4 (beastfolk tribe).
- **Base:** Savanna (3), Thorn scrub (3); Hot desert, Temperate grassland, Cold steppe, Woodland & shrubland (2); forests (1); Ice & snow, Tundra, Tropical rain forest, Temperate rain forest (0). Flat Land. Heroes III is the outlier: its gnolls are a swamp town's creatures [H3-C].
- **Resources:** wild food & game, livestock (stolen), horses.
- **Settlement:** camps and dens, ruins. Density 0.3 (Design note).
- **Frequency:** 5/20.
- **Evidence:** [SRD51-M] [PF2-ANC] [H3-C] [MOM-W] [unverified for the full Master of Magic roster].

### Group D. Half-breeds

#### D1. Half-elf

Children of elves and humans: longer-lived than humans, at home in neither people, often go-betweens.

- **Axes:** medium · long (180+ years [S51]) · human · ordinary to gifted · playable · omnivore.
- **Names in games (evidence only):** half-elves, aiuvarin.
- **Society:** no society of their own; they live in the community of one parent [S51] [unverified for the go-between role]. Pathfinder makes the type a "versatile heritage" that can attach to any ancestry, covering descendants of elves and any other people [PF2-VH].
- **Base:** follows the host community (Follower); slightly more where elf and human ranges meet (forest edge, Temperate mixed and broadleaf forest next to farmland).
- **Settlement:** as the host. Density: a share of the mixed population (§5.4).
- **Variants:** the elf parent's variant decides the look (wood, high, dark).
- **Frequency:** 4/20 (as a named race; removed as a separate species in SRD 5.2, which has no half-races [S52]).
- **Evidence:** [S51] [PF2-VH] [ARC] [BG3] [partially verified].

#### D2. Half-orc

Children of orcs and humans: strong, short-lived, mistrusted by both sides.

- **Axes:** medium · short (rarely over 75 [S51]) · human · ordinary · playable · omnivore.
- **Names in games (evidence only):** half-orcs, dromaar.
- **Society:** no society of their own; SRD 5.1 says they lean evil when raised among orcs [S51]; Pathfinder's orc-human heritage is common in an orc-ruled border region [PF2-VH].
- **Base:** follows the host; more on the border between orc and human ranges (Hills, Cold steppe, Temperate grassland edges).
- **Frequency:** 4/20 (dropped from SRD 5.2 like the half-elf [S52]).
- **Evidence:** [S51] [PF2-VH] [ARC] [BG3] [partially verified].

### Group E. Giant-kin

#### E1. Giant-blooded

Large, strong humanlike peoples descended from or related to giants: highlanders who prize fair contest and self-reliance, or a regimented horned people from over the sea.

- **Axes:** large (7–8 ft [S52]) · human [unverified] · low [unverified] · ordinary · playable · omnivore.
- **Names in games (evidence only):** goliaths, jotunborn, Qunari, half-ogres, aumaua (a large amphibious people of Pillars [unverified for the size and amphibious traits]).
- **Society and economy:** SRD 5.2's goliaths are distant descendants of giants who seek heights beyond their ancestors [S52]; Dragon Age's horned people are deeply regimented [DAO]; Arcanum's half-ogres are a human hybrid [ARC].
- **Relations:** respected as mercenaries and porters by humans [unverified]; tied to the true giants (Huge) of the monster lists [SRD51-M]. Pairs with factions.md E1 (mercenaries), H2 (highland tribe).
- **Base:** Mountains (3), Hills (2); Tundra, Boreal forest, Cold steppe (2); Ice & snow (1); Hot desert, Tropical rain forest, Mangrove (0). Coastal variant on islands.
- **Resources:** livestock (highland herds), wild food & game, stone.
- **Settlement:** small highland villages and stone camps. Density 0.2 (Design note).
- **Variants:** by giant ancestry, each with its own biome: **frost** (Ice & snow, Tundra), **fire** (`volcanic` bedrock), **stone** (Mountains), **hill** (Hills), **cloud** (high Mountains), **storm** (coasts) [S52] for the six ancestries; the biome mapping is a Design note. **Horned sea people** (islands, a regimented state) [DAO]; **island giants** (coasts, `harbour`) [POE-SR].
- **Frequency:** 5/20.
- **Evidence:** [S52] [PF2-ANC] [DAO] [ARC] [POE-SR].

#### E2. Ogre

Huge, dull, hungry brutes in small clans: hired muscle, raiders and, in Total War, a whole kingdom of mercenary gluttons [unverified].

- **Axes:** large · human [unverified] · low · mundane · hostile · omnivore, voracious [unverified].
- **Relations:** muscle for orcs and goblins (Heroes III's Stronghold: goblins, orcs, ogres, cyclopes [H3-C]); enemies of farmers.
- **Base:** Hills and Mountains (3); Cold steppe, Tundra, Boreal forest, Woodland & shrubland (2); Tropical rain forest, Mangrove (0).
- **Resources:** livestock (stolen), wild food & game.
- **Settlement:** caves and crude halls. Density 0.1 (Design note).
- **Frequency:** 4/20.
- **Evidence:** [SRD51-M] [H3-C] [ARC] (as half-ogre).

#### E3. Troll

Big, regenerating, often savage people: monsters in Tolkien and D&D, a full people with an old empire in Warcraft.

- **Axes:** large · human to long [unverified] · human · ordinary (voodoo-like magic in Warcraft [unverified]) · mixed (playable in Warcraft; monsters in Tolkien, SRD 5.1, Master of Magic) · carnivore [unverified].
- **Appearance and life:** fast healing that can regrow limbs [WOW-TRO]; Master of Magic's trolls are brutal and regenerate, and lazy about expanding [MOM-DD].
- **Society and economy:** tribes and old empires; Warcraft's troll tribes once held great territories and one built an empire to unite them against an insect people [WOW-TRO].
- **Relations:** enemies of elves in Warcraft [unverified]; allies of orcs [WOW-PR].
- **Base:** the best example of biome variants in the sample. Base weight 2 for every biome; the variant decides the native one [WOW-TRO]:
  - **Jungle troll**: Tropical rain forest, Tropical dry forest.
  - **Forest troll**: Temperate mixed and broadleaf forest, Boreal forest.
  - **Ice troll**: Ice & snow, Tundra.
  - **Sand troll**: Hot desert, Thorn scrub.
  - **Swamp troll**: Mangrove, wet Lowland.
  - **Cave troll**: underground.
  - **Highland troll**: plateaus, Hills and Mountains in the tropics.
- **Resources:** wild food & game, fish.
- **Settlement:** temple cities and villages for the playable kind; lairs for the monster kind. Density 0.2 (Design note).
- **Frequency:** 4/20.
- **Evidence:** [WOW-TRO] [WOW-PR] [MOM-DD] [SRD51-M] [TOLK-ORC] (Tolkien's trolls as a creature beside the orcs [unverified]).

### Group F. Beastfolk

#### F1. Lizardfolk

Cold-blooded, scaled people of swamps and warm wetlands: patient, traditional, practical hunters with an ancient past.

- **Axes:** medium · long (~120 years; adult 15; grow bigger with age [PF2-LIZ]) · human · ordinary (astrologers, nature priests [PF2-LIZ]) · mixed (playable in Pathfinder, Divinity, Age of Wonders, Master of Magic, Skyrim; monsters in SRD 5.1) · carnivore-leaning [unverified].
- **Names in games (evidence only):** lizardfolk, lizardmen, Argonians, Lizards, saurians.
- **Society and economy:** communal child-rearing, oral epics, carvings, ancestor rites, patience [PF2-LIZ]; Master of Magic's lizardmen are small, tough and agile, move easily on water, prefer wetlands and seldom expand [MOM-DD]; Against the Storm's Lizards are resilient and love warmth [ATS] [partially verified].
- **Relations:** reserved, thought cold by others [PF2-LIZ]; enemies of swamp-draining settlers (factions.md D6); allies of druids (B7). Pairs with factions.md H4 (beastfolk tribe).
- **Base:** Mangrove (3), Tropical rain forest (3); Tropical dry forest, Temperate rain forest (2); Savanna, Temperate broadleaf forest, Thorn scrub, Hot desert (1); Ice & snow, Tundra, Boreal forest, Cold steppe, Cold desert (0). Flat Land and Lowland; `river`, `lake`, coast. Sourced: water's edge, half-submerged settlements [PF2-LIZ]; Skyrim's lizard people thrive in a great swamp [TES] [partially verified]; Heroes III's lizardmen belong to the swamp town [H3-T] [H3-C]; wetlands [MOM-DD]; warmth [ATS] [partially verified].
- **Resources:** fish, wild food & game, timber (mangrove wood), dyes and spices of the wet tropics.
- **Settlement:** stilt villages, half-flooded halls. Density 0.4 (Design note).
- **Variants:** **swamp lizardfolk** (Mangrove, wet Lowland); **jungle lizardfolk** (Tropical rain forest, temple cities; Total War's Lizardmen [unverified]); **sand lizardfolk** (Hot desert oases) [unverified]; **dragon-sworn lizardfolk** (see C4, H3).
- **Frequency:** 9/20.
- **Evidence:** [PF2-LIZ] [SRD51-M] [MOM-DD] [H3-T] [H3-C] [DOS2] [AOW1] [TES] [partially verified] [ATS] [partially verified].

#### F2. Catfolk

Feline people of warm plains and forests: curious, independent wanderers, traders and guardians of wild places.

- **Axes:** medium · short (60–70 years [PF2-CAT]) · human · ordinary (animist [PF2-CAT]) · playable · omnivore, meat-leaning [unverified].
- **Names in games (evidence only):** catfolk, Khajiit, Tigrans, tabaxi.
- **Society and economy:** large extended families, a speaker chosen to lead and mediate, several trades learned by loose apprenticeship, love of travel [PF2-CAT]. Caravan traders [unverified].
- **Relations:** see themselves as guardians of natural places [PF2-CAT]; allies of centaurs [PF2-CEN]. Pairs with factions.md D8 (caravan company) and B7 (druids).
- **Base:** Savanna (3); Hot desert, Thorn scrub, Tropical dry forest, Tropical rain forest, Temperate grassland (2); Ice & snow, Tundra, Boreal forest (0). Sourced: Skyrim's cat people come from a land of dry plains with lush southern forests [TES] [partially verified]; Age of Wonders 3's Tigrans hate arctic and blighted climates [AOW3-C] [partially verified].
- **Resources:** wild food & game, spices, stimulants & sugar, horses & pack animals (caravans).
- **Settlement:** villages and caravan towns. Density 0.5 (Design note).
- **Variants:** **plains catfolk** (Savanna, Temperate grassland); **desert catfolk** (Hot desert, caravans); **jungle catfolk** (Tropical rain forest).
- **Frequency:** 3/20.
- **Evidence:** [PF2-CAT] [PF2-CEN] [AOW3-C] [partially verified] [TES] [partially verified].

#### F3. Ratfolk

Small, clever rodent people of warrens, tunnels and wagon caravans: hoarders, tinkers and traders, or in the dark version a teeming plague under the cities.

- **Axes:** small · short [unverified] · high · ordinary · mixed (friendly traders in Pathfinder; Total War's ratmen are a villain empire [unverified]) · omnivore.
- **Society and economy:** communal, cooperative, strong extended families, strict social codes; warrens of up to a hundred per home, crowded with hoarded goods; traders and tinkers who travel between towns in wagon trains [PF2-RAT].
- **Relations:** others find their looks disturbing and confuse them with wererats [PF2-RAT]. Pairs with factions.md D8 (caravans), F1 (thieves' guild) for the dark version.
- **Base:** under and around towns: high affinity for Prospects; biome-neutral (1), Ice & snow (0); caves in Hills.
- **Resources:** trade goods; anything stored.
- **Settlement:** warrens in towns, caves used as stores, caravans [PF2-RAT]. Density 2.0 (Design note).
- **Variants:** **caravan ratfolk** (the travel network); **undercity ratfolk** (under city Prospects).
- **Frequency:** 2/20. Kept in full for its unusual urban niche.
- **Evidence:** [PF2-RAT] [TWW-R] [partially verified].

#### F4. Birdfolk

Winged or beaked people of mountains, cliffs and ports: gregarious collectors of knowledge, or shrieking harpies of the crags.

- **Axes:** small to medium · human [unverified] · human · ordinary · mixed (playable tengu, strix and Against the Storm's harpies; hostile harpies in Heroes III and D&D) · omnivore.
- **Names in games (evidence only):** tengu, strix, harpies, aarakocra.
- **Society and economy:** tengu keep an ancestral home in mountains and a diaspora in cities and ports, work as fishers, crews and bladesmiths, and collect knowledge and tools [PF2-TENGU]; strix are flying avian people [PF2-ANC].
- **Relations:** viewed with suspicion and superstition [PF2-TENGU]. Pairs with factions.md F5 (pirates), E1 (mercenaries).
- **Base:** Mountains (3), Hills (2); coast and `harbour` (2); biome-neutral otherwise (1), Ice & snow (0).
- **Resources:** fish, wild food & game, gems (collectors).
- **Settlement:** cliff roosts; quarters in port towns. Density 0.3 (Design note).
- **Variants:** **roosted** (mountain homeland, traditional) and **migrant** (in other peoples' cities) [PF2-TENGU]; **harpy** (crags, hostile; Heroes III's underground town [H3-C]).
- **Frequency:** 3/20.
- **Evidence:** [PF2-TENGU] [PF2-ANC] [H3-C]; Against the Storm's Harpies [unverified].

#### F5. Bull-folk

Horned, bovine people: either labyrinth-builders of mountains and caves, or great nomads of the plains.

- **Axes:** large (9–10 ft for Warcraft's plains kind [WOW-TAU]) · human [unverified] · low [unverified] · ordinary (shamanism on the plains [WOW-TAU]) · mixed (playable in Pathfinder and Warcraft; monsters in D&D and Heroes III) · omnivore; Pathfinder's minotaurs eat lichen and plants with monthly meat feasts [PF2-MIN].
- **Names in games (evidence only):** minotaurs, tauren, sarangay.
- **Society and economy:** Pathfinder's minotaurs build insular stone enclaves with twisting halls, value puzzles and lasting architecture and are mostly hunter-gatherers [PF2-MIN]. Warcraft's plains people were nomads with tent towns moving with the herds, now settled in longhouses, with a nature faith and honoured hunters [WOW-TAU].
- **Relations:** minotaurs often settle with dwarves, hobgoblins and orcs [PF2-MIN]; Pathfinder's centaurs clash with them [PF2-CEN]; Warcraft's plains people took their land from centaurs [WOW-TAU]. Pairs with factions.md H4 (beastfolk tribe), H1 (nomads) for the plains kind.
- **Base:** two variants with opposite leans:
  - **Labyrinth bull-folk**: Mountains (3), underground; biome-neutral [PF2-MIN]; Heroes III's minotaurs are in the underground town [H3-C].
  - **Plains bull-folk**: Temperate grassland, Savanna (3), Cold steppe (2), on mesas in Hills [WOW-TAU].
  - Matrix values use the plains kind; Ice & snow, Tropical rain forest, Mangrove (0).
- **Resources:** livestock, wild food & game, stone (labyrinth kind).
- **Settlement:** stone labyrinth enclaves; tent towns and longhouses. Density 0.2 (Design note).
- **Frequency:** 4/20.
- **Evidence:** [PF2-MIN] [WOW-TAU] [SRD51-M] [H3-C] [EL-MF].

#### F6. Centaur

Half-horse people of open country: archers, herbalists and nomads in bands with wide, well-guarded ranges.

- **Axes:** large · human [unverified] · low [unverified] · ordinary (nature priests and priests of the gods [PF2-CEN]) · mixed (playable in Pathfinder; Heroes III's elf-town troops; minor factions in Endless Legend; hostile tribes in Warcraft [unverified]) · omnivore.
- **Society and economy:** mostly nomadic hunter-gatherer bands in large defined territories, each with a central camp for the sick and old, some camps permanent; leaders by experience; exile is the worst punishment [PF2-CEN].
- **Relations:** allied with catfolk, frog-folk, elves and orcs; enemies of expansionist human states, harpies and minotaurs [PF2-CEN]; hate being ridden [PF2-CEN]. Heroes III groups them with elves and dwarves [H3-C]. Pairs with factions.md H1 (nomad horde), H4 (beastfolk tribe), B7 (druids).
- **Base:** Temperate grassland (3), Savanna (3), Cold steppe (3); Woodland & shrubland, Temperate mixed and broadleaf forest (2); Ice & snow, Tundra, Mangrove, Tropical rain forest (0). Flat Land (3), Lowland (2), Mountains (0). Sourced: plains, forests, alpine meadows, savannas [PF2-CEN]; grass is the native terrain of the town that has them [H3-T] [H3-C].
- **Resources:** wild food & game, horses (kin, not mounts), herbs.
- **Settlement:** camps; few permanent villages. Density 0.2 (Design note).
- **Frequency:** 4/20.
- **Evidence:** [PF2-CEN] [SRD51-M] [H3-T] [H3-C] [EL-MF].

### Group G. Aquatic peoples

#### G1. Merfolk

Half-fish people of the sea, with courts, kingdoms and magic used as an everyday tool; aloof towards the surface.

- **Axes:** medium · human [unverified] · human · gifted (rulers are expected to master sea magic [PF2-MER]) · mixed (monster entry in SRD 5.1; uncommon ancestry in Pathfinder) · fish-eaters [unverified].
- **Society and economy:** villages and cities in aquatic kingdoms ruled by aristocratic families with court politics and active diplomacy [PF2-MER].
- **Relations:** many communities standoffish towards surface folk, some curious [PF2-MER]; enemies of fishfolk raiders (G2) [unverified]. Pairs with factions.md D3 (merchant league) as a partner, F5 (pirates) as an enemy.
- **Base:** Ocean zones next to land; biome weight 0 everywhere on land. Largest numbers on temperate continental shelves rich in fish; coral cities on tropical reefs; small clans hunting seals on polar ice; abyssal kin in deep water, which they see as poor land [PF2-MER]. In repo terms: ocean patches with high fish (`map-generation.md` §14.5 and Appendix D.3), pearls or coral in warm water, seals in cold.
- **Resources:** fish, gems & pearls, seaweed (wild food).
- **Settlement:** underwater villages and cities in reach of the coast. Density 0.3 per ocean patch next to land (Design note).
- **Variants:** **shelf merfolk** (temperate coasts), **reef merfolk** (warm coasts, coral, pearls), **ice merfolk** (polar coasts, seal hunters), **abyssal merfolk** (open ocean) [PF2-MER]; **sea-elves of a drowned empire** (Pathfinder's azarketi, who still visit the surface [PF2-ANC]).
- **Frequency:** 3/20.
- **Evidence:** [PF2-MER] [PF2-ANC] [SRD51-M].

#### G2. Fishfolk raiders

Fish-, eel- or shark-like sea people who raid coasts, worship sea monsters and are hostile in nearly every source.

- **Names in games (evidence only):** sahuagin [SRD51-M], murlocs, kuo-toa, deep ones [unverified for all but the first].
- **Base:** Ocean next to land; Mangrove and coastal Lowland for raids (Design note).
- **Relations:** enemies of merfolk, coastal humans, lizardfolk [unverified]. Pairs with factions.md F5 (pirates), B5 (forbidden cult).
- **Frequency:** 2/20.
- **Evidence:** [SRD51-M]; Warcraft's fish-people [unverified].

### Group H. Planetouched and draconic

#### H1. Fiend-touched

Humans (or others) with fiendish blood: horns, tails, odd eyes; distrusted but not evil by nature.

- **Axes:** medium or small · slightly longer than human [S51] · human · innate · playable · omnivore.
- **Names in games (evidence only):** tieflings, nephilim.
- **Society:** no homeland; outcasts in human cities [unverified]. SRD 5.1 says they are not innately evil but many turn to evil [S51]; SRD 5.2 gives three fiendish legacies tied to three groups of lower planes [S52]; Pathfinder now folds them into a "nephilim" heritage with fiendish or celestial features [PF2-VH].
- **Base:** Follower of the host race; more where the Supernatural Field is high; `volcanic` bedrock for an infernal lineage (Design note).
- **Relations:** hunted by factions.md B4 (inquisition); recruited by B5 (forbidden cult).
- **Frequency:** 5/20 counting all planetouched.
- **Evidence:** [S51] [S52] [PF2-VH] [BG3] [partially verified].

#### H2. Celestial and elemental-touched

People born with the mark of a god, an angel or an element: glowing eyes, flame hair, stone skin.

- **Names in games (evidence only):** aasimar, godlike, ifrit, oread, sylph, undine, talos, suli.
- **Society:** no homeland; born at random into ordinary families. Pillars' godlike come in death, fire, moon and nature kinds [POE-SR]; Pathfinder's geniekin are descended from beings of fire, earth, air, water and metal [PF2-VH].
- **Base:** Follower; a birth weight per element (Design note): fire → `volcanic` bedrock, Hot desert; earth → Mountains; water → coasts; air → high Mountains; metal → metal deposits; divine → high Supernatural.
- **Frequency:** with H1, 5/20.
- **Evidence:** [POE-SR] [PF2-VH].

#### H3. Dragonkin

Dragon-blooded humanoids, scaled and often breathing fire, frost or lightning by lineage: proud clans, or a lost empire's soldiers.

- **Axes:** medium (5–7 ft [S52]) · short (~80 years [S51]) · human · innate (breath) · playable · omnivore.
- **Names in games (evidence only):** dragonborn, draconians, dracthyr, dragonblood.
- **Society:** clans; hatched from dragon eggs in SRD 5.2 [S52]; Pathfinder treats dragon descent as a heritage that can attach to any ancestry [PF2-VH].
- **Relations:** kobolds (C4) serve dragons as patrons [PF2-KOB], and in many settings dragonkin too [unverified]; kin to lizardfolk in looks [unverified].
- **Base:** by lineage. SRD 5.2 names ten colours with five damage types (acid, lightning, fire, poison, cold) [S52]; mapping a colour to a biome (red and gold to `volcanic` bedrock and Hot desert, white and silver to Ice & snow and Tundra, green to forest, black to Mangrove, blue and brass to deserts) is a Design note. Age of Wonders 3's draconians like barren and lava terrain and dislike arctic climate [AOW3-C] [partially verified]. Matrix values: Hot desert, Cold desert, Thorn scrub, Savanna (2); Ice & snow, Tundra (0).
- **Resources:** gold, gems (dragon hoards).
- **Settlement:** clan halls near a dragon lair or old ruins. Density 0.2 (Design note).
- **Frequency:** 7/20.
- **Evidence:** [S51] [S52] [PF2-VH] [PF2-KOB] [WOW-PR] [AOW3-C] [partially verified].

### Group I. Undead and constructed peoples

#### I1. Undead people

Dead who kept their minds and became a people: a free undead nation, the court of a vampire, a tomb empire, or skeletons who simply go on living.

- **Axes:** medium · ageless · none (they recruit, not breed) · innate · mixed (playable in Warcraft, Divinity 2, Pathfinder; strategy factions in Heroes III, Age of Wonders, Total War) · none, or blood.
- **Names in games (evidence only):** Forsaken, undead, skeletons, vampires, tomb kings.
- **Society:** Divinity: Original Sin 2 lets each playable race also be played as undead [DOS2]; Pathfinder's skeleton ancestry keeps its former mind [PF2-ANC]; Heroes III's undead town has dirt as native terrain [H3-T] [H3-C]; RimWorld's blood-drinkers are nearly immortal, need blood and suffer in light [RW-X].
- **Relations:** enemies of every faith and of the living; pairs with factions.md C3 (necromancer cabal), B5.
- **Base:** high Supernatural (3); ruins, old battlefields, burial grounds; Tundra, Cold desert, Hot desert (2: dry land preserves the dead, tomb empires); otherwise 1 (Design note).
- **Resources:** none needed; gold and gems; for vampires, a living population nearby.
- **Settlement:** necropolises, ruined cities, crypt towns. Density 0.1 (Design note).
- **Variants:** **free undead** (a people of the risen dead); **vampire court** (rules living serfs); **tomb dynasty** (deserts) [unverified]; **ghost folk** [unverified].
- **Frequency:** 6/20.
- **Evidence:** [DOS2] [PF2-ANC] [H3-T] [H3-C] [AOW1] [RW-X] [WOW-PR].

#### I2. Construct people

Made bodies with living minds: clockwork soldiers of a dead empire, stone-born earth spirits, robots with a soul.

- **Axes:** small to large · ageless · none (made) · innate or mundane · playable · none.
- **Names in games (evidence only):** automatons, poppets, earthen, mechagnomes, warforged, Kenshi's robot "skeletons".
- **Society:** Pathfinder's automatons are constructs with souls from a dying empire's technology [PF2-ANC]; Warcraft's dwarves descend from living-stone earthen made by titan servants [WOW-DWF]; Kenshi's robot people need no food and repair instead of heal [KEN] [partially verified].
- **Base:** any; near the ruins of the civilisation that made them; metal deposits (Design note).
- **Frequency:** 3/20.
- **Evidence:** [PF2-ANC] [WOW-DWF] [WOW-PR] [KEN] [partially verified].

### Group J. Rarer peoples

Each recurs in one to three rosters. Kept for variety; a generator can use them as region-rare "exotic" races.

| Archetype | Names (evidence only) | Habitat lean (repo terms, Design note) | Disposition | Evidence |
|---|---|---|---|---|
| **J1 Insect folk** | Klackons, the Hive, surki, anadi | Hot desert, Thorn scrub, underground; hive-cities | hostile or alien | Master of Magic's insect race [MOM-W]; Kenshi's three hive castes [KEN] [partially verified]; Pathfinder's insectile and spider peoples [PF2-ANC]; Stellaris' arthropoid class [ST-SP] |
| **J2 Plant folk** | leshies, ghoran, dryads | old forest, high Supernatural; Temperate rain forest, Tropical rain forest | neutral | Pathfinder's leshies (immortal nature spirits in plant bodies, made by a druid's ritual) [PF2-LESHY]; plant people who live on through seeds [PF2-ANC]; Stellaris' plantoid class [ST-SP] |
| **J3 Frost folk** | Frostlings | Ice & snow, Tundra | mixed | Age of Wonders' Frostlings [AOW1]; they hate tropical and volcanic climate [AOW3-C] [partially verified] |
| **J4 Small fey** | sprites, pixies, fauns, nymphs | forest glades, high Supernatural | neutral | Pathfinder's sprites, fauns, nymphs [PF2-ANC]; Heroes III's pixies [H3-C] |
| **J5 Shapeshifters** | worgen, beastkin, kitsune, changelings, tanuki | Follower of humans; forests | mixed | Warcraft's wolf-people [WOW-PR]; Pathfinder's beastkin, changelings, kitsune, tanuki [PF2-VH] [PF2-ANC] |
| **J6 Foxfolk and other small mammal folk** | vulpera, Foxes, Beavers, Bats | Hot desert (vulpera) [unverified]; forest and river (beavers); caves (bats) | playable | Warcraft's fox-people [WOW-PR]; Against the Storm's Foxes, Beavers and Bats, each with its own house: beavers want cosy wooden homes, bats spacious dark ones [ATS] [partially verified] |
| **J7 Frogfolk** | tripkees, Frogs, bullywugs | Mangrove, Tropical rain forest, wet Lowland | mixed | Pathfinder's frog-like people [PF2-ANC]; Against the Storm's Frogs [unverified] |
| **J8 Bearfolk** | pandaren | Temperate mixed and broadleaf forest, Hills; bamboo-like highlands | playable | Warcraft [WOW-PR] |
| **J9 Snake folk** | nagaji, vishkanya, nagas, yuan-ti | Tropical rain forest, Hot desert ruins; temples | mixed | Pathfinder's serpent-headed and venomous peoples [PF2-ANC]; Heroes III's nagas [H3-C] |
| **J10 Horned warrior folk** | Shek | Cold desert, Thorn scrub, rocky Hills | playable | Kenshi [KEN] [partially verified] |
| **J11 Chaos beastmen** | beastmen | old dark forest, high Supernatural; a horde without towns | hostile | Master of Magic's aggressive, adaptable beastmen [MOM-DD]; Total War's Beastmen are a horde that occupies no settlements and ignores climate [TWW-CF] [partially verified] |
| **J12 Simian folk** | vanara | Tropical rain forest, Tropical dry forest | playable | Pathfinder [PF2-ANC] |

Songs of Syx's own species (lizard-like Garthimi of temperate and warm land, Cretonians of warm land, Dondorians of cold land, aquatic shoreline Amevia, warm-land Argonosh and versatile Tilapi) [SOS] [partially verified] are original creations and fit J-type exotic peoples; the game is more useful as evidence that a city builder can run on per-race climate preferences (§4.1).

---

## 4. Matrices

### 4.1 Race × biome

Weights from the **Base** lines of §3 (Design note). 3 native, 2 good, 1 tolerated, 0 avoided. Columns follow `map-generation.md` §13.2: Ice & snow, Tundra, Cold steppe, Boreal forest, Cold desert, Temperate grassland, Woodland & shrubland, Temperate mixed forest, Temperate broadleaf forest, Temperate rain forest, Hot desert, Thorn scrub, Savanna, Tropical dry forest, Tropical rain forest, Mangrove.

| Race | Ice | Tun | CSt | Bor | CDs | TGr | WSh | TMx | TBl | TRn | HDs | Thn | Sav | TDf | TRf | Mgv |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| A1 Human | 0 | 1 | 2 | 2 | 1 | 3 | 3 | 3 | 3 | 2 | 1 | 1 | 2 | 2 | 1 | 1 |
| B1 Elf | 0 | 0 | 0 | 2 | 0 | 1 | 1 | 3 | 3 | 3 | 0 | 0 | 0 | 1 | 2 | 0 |
| B2 Dark elf (underground) | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 |
| B3 Dwarf | 1 | 2 | 1 | 2 | 1 | 1 | 1 | 2 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 |
| B4 Halfling | 0 | 0 | 1 | 1 | 0 | 3 | 3 | 2 | 2 | 1 | 0 | 0 | 1 | 1 | 1 | 0 |
| B5 Gnome | 0 | 1 | 1 | 2 | 1 | 1 | 2 | 2 | 3 | 1 | 0 | 1 | 1 | 1 | 0 | 0 |
| C1 Orc | 0 | 1 | 3 | 1 | 2 | 2 | 2 | 1 | 1 | 1 | 2 | 2 | 2 | 1 | 0 | 0 |
| C2 Goblin | 0 | 1 | 1 | 2 | 1 | 2 | 2 | 2 | 2 | 1 | 1 | 2 | 1 | 1 | 1 | 0 |
| C3 Hobgoblin | 0 | 1 | 2 | 1 | 1 | 2 | 2 | 2 | 1 | 1 | 1 | 1 | 1 | 1 | 0 | 0 |
| C4 Kobold (underground) | 0 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 |
| C5 Gnoll | 0 | 0 | 2 | 1 | 1 | 2 | 2 | 1 | 1 | 0 | 2 | 3 | 3 | 1 | 0 | 0 |
| E1 Giant-blooded | 1 | 2 | 2 | 2 | 1 | 1 | 1 | 1 | 1 | 1 | 0 | 1 | 1 | 1 | 0 | 0 |
| E2 Ogre | 1 | 2 | 2 | 2 | 1 | 1 | 2 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 0 | 0 |
| E3 Troll (by variant) | 2 | 2 | 1 | 2 | 1 | 1 | 1 | 2 | 2 | 2 | 2 | 2 | 1 | 2 | 2 | 2 |
| F1 Lizardfolk | 0 | 0 | 0 | 0 | 0 | 1 | 1 | 1 | 1 | 2 | 1 | 1 | 1 | 2 | 3 | 3 |
| F2 Catfolk | 0 | 0 | 1 | 0 | 1 | 2 | 1 | 1 | 1 | 1 | 2 | 2 | 3 | 2 | 2 | 1 |
| F3 Ratfolk (towns) | 0 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 |
| F4 Birdfolk | 0 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 | 1 |
| F5 Bull-folk (plains) | 0 | 1 | 2 | 1 | 1 | 3 | 2 | 1 | 1 | 1 | 1 | 1 | 3 | 1 | 0 | 0 |
| F6 Centaur | 0 | 0 | 3 | 1 | 1 | 3 | 2 | 2 | 2 | 1 | 1 | 1 | 3 | 1 | 0 | 0 |
| H3 Dragonkin | 0 | 0 | 1 | 1 | 2 | 1 | 1 | 1 | 1 | 1 | 2 | 2 | 2 | 1 | 1 | 1 |
| I1 Undead | 1 | 2 | 1 | 1 | 2 | 1 | 1 | 1 | 1 | 1 | 2 | 1 | 1 | 1 | 1 | 1 |
| J3 Frost folk | 3 | 3 | 1 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |

Not in the table: D1, D2, H1, H2, I2 and most of J follow their host race; G1 and G2 live in Ocean zones (§4.2).

**Sourced anchors behind the numbers:** elves in forest [S51] [S52] [TOLK-ELF] [WOW-NE] [MOM-DD]; dwarves in mountains [DF-CIV] [partially verified] [TOLK-DW] [WOW-DWF] and not in the tropics [AOW3-C] [partially verified]; halflings on farmland and not in arctic or volcanic land [TOLK-HOB] [AOW3-C] [partially verified]; orcs on plains [S52] and rough ground [H3-T]; lizardfolk in swamp and wetland [H3-T] [MOM-DD] [TES] [partially verified]; centaurs on plains and savanna [PF2-CEN] [H3-T]; catfolk not in the arctic [AOW3-C] [partially verified]; trolls by variant [WOW-TRO]; frost folk not in the tropics [AOW3-C] [partially verified]. Humans start in grassland, savanna or shrubland in Dwarf Fortress [DF-CIV] [partially verified].

### 4.2 Race × terrain and settlement

Elevation types from `map-generation.md` §8, plus underground, sea, rivers, the Supernatural Field and closeness to Prospects. Same scale (Design note). "Prospect" is how much the race lives in or next to the cities and towns of `map-generation.md` §15: 3 lives in them, 0 avoids them.

| Race | Flat | Low | Hills | Mtn | Ocean / coast | Under-ground | River / lake | Super-natural | Prospect |
|---|---|---|---|---|---|---|---|---|---|
| A1 Human | 3 | 3 | 2 | 1 | 2 | 0 | 3 | 1 | 3 |
| B1 Elf | 1 | 3 | 2 | 1 | 1 | 0 | 2 | 3 | 0 |
| B2 Dark elf | 0 | 0 | 2 | 3 | 0 | 3 | 1 | 3 | 0 |
| B3 Dwarf | 0 | 1 | 3 | 3 | 0 | 3 | 2 | 1 | 1 |
| B4 Halfling | 2 | 3 | 3 | 0 | 0 | 1 | 3 | 1 | 2 |
| B5 Gnome | 1 | 2 | 3 | 2 | 0 | 2 | 1 | 2 | 2 |
| C1 Orc | 2 | 1 | 3 | 3 | 1 | 2 | 1 | 1 | 0 |
| C2 Goblin | 1 | 1 | 3 | 3 | 1 | 2 | 1 | 1 | 1 |
| C3 Hobgoblin | 1 | 2 | 3 | 3 | 0 | 1 | 1 | 0 | 1 |
| C4 Kobold | 0 | 1 | 3 | 3 | 0 | 3 | 1 | 2 | 0 |
| C5 Gnoll | 3 | 2 | 1 | 0 | 0 | 0 | 1 | 1 | 0 |
| D1, D2 Half-breeds | as host | | | | | | | | 3 |
| E1 Giant-blooded | 0 | 1 | 2 | 3 | 1 | 0 | 1 | 1 | 0 |
| E2 Ogre | 0 | 1 | 3 | 3 | 0 | 1 | 1 | 1 | 0 |
| E3 Troll | 1 | 2 | 2 | 2 | 1 | 1 | 2 | 2 | 0 |
| F1 Lizardfolk | 3 | 3 | 1 | 0 | 2 | 0 | 3 | 1 | 0 |
| F2 Catfolk | 3 | 2 | 1 | 0 | 1 | 0 | 2 | 1 | 1 |
| F3 Ratfolk | 1 | 1 | 1 | 1 | 1 | 2 | 1 | 1 | 3 |
| F4 Birdfolk | 0 | 1 | 2 | 3 | 2 | 0 | 1 | 1 | 1 |
| F5 Bull-folk | 3 (plains) | 2 | 2 | 3 (labyrinth) | 0 | 2 | 1 | 1 | 0 |
| F6 Centaur | 3 | 2 | 1 | 0 | 0 | 0 | 2 | 2 | 0 |
| G1 Merfolk | 0 | 0 | 0 | 0 | 3 | 0 | 0 | 2 | 0 |
| G2 Fishfolk | 0 | 1 | 0 | 0 | 3 | 0 | 0 | 2 | 0 |
| H1, H2 Planetouched | as host | | | | | | | 3 | 3 |
| H3 Dragonkin | 1 | 1 | 2 | 3 | 0 | 1 | 1 | 2 | 1 |
| I1 Undead | 1 | 1 | 2 | 2 | 0 | 2 | 0 | 3 | 0 |
| I2 Construct | 1 | 1 | 1 | 1 | 0 | 1 | 0 | 2 | 1 |

### 4.3 Recurring race relations

| Pair | Relation | Evidence |
|---|---|---|
| Dwarf B3 × goblin C2, orc C1 | old war for the mountains | Pathfinder's dwarven empire fell to orcs and goblinoids [PF2-DWARF]; factions.md §4.1 |
| Dwarf B3 + gnome B5 | close allies, shared halls | Warcraft [WOW-GNO]; the Witcher [WIT-R] [partially verified] |
| Dwarf B3 × elf B1 | estrangement, old grudges | Tolkien [TOLK-DW] |
| Elf B1 × dark elf B2 | kin enemies | D&D's drow lineage underground [S52] [unverified for the enmity] |
| Elf B1 × human A1 (settlers) | forest against field | Dragon Age's elves under human rule [DAO]; the Witcher [WIT-R] [partially verified]; factions.md D6 × H5 |
| Goblin C2 × gnome B5 | rival engineers | Warcraft [WOW-GOB] |
| Goblin C2 + orc C1 + ogre E2 | the greenskin horde | Heroes III's Stronghold [H3-C] |
| Hobgoblin C3 + goblin C2 | master and smaller kin | [PF2-HOB] |
| Kobold C4 + dragons, dragonkin H3 | servant and patron | [PF2-KOB] for dragons; dragonkin [unverified] |
| Centaur F6 + catfolk F2, elf B1, orc C1 | the wild-land coalition | [PF2-CEN] |
| Centaur F6 × bull-folk F5 | rivals for the plains | [PF2-CEN] [WOW-TAU] |
| Centaur F6 × expansionist human A1 | settlers against nomads | [PF2-CEN] |
| Minotaur F5 + dwarf B3, hobgoblin C3, orc C1 | neighbours under the mountains | [PF2-MIN] |
| Undead I1 × every living race | the living against the dead | Heroes III's undead town [H3-C]; factions.md C3 |
| Merfolk G1 × surface peoples | aloof, wary | [PF2-MER] |

---

## 5. Design note: races in the generator

*Everything in this section is my own engineering judgement, not a sourced claim.*

### 5.1 What a race needs as data

A race is static configuration, like the bedrock and biome tables: it belongs in the Parameter Set (or a sibling data file), not in generated data. Per race:

| Field | Type | Use |
|---|---|---|
| Id and display name | generic name of §3 | the recommended name, never a proprietary one |
| Group | A–J | for UI and for mixing rules |
| Size | Small / Medium / Large | food multiplier (0.6 / 1 / 2), house size, portrait prompt |
| Lifespan | maturity age, old age, max age | Person Stub ages (`readme.md`: age is a Stub fact) |
| Fertility | children per family, relative to humans | family size in Settled; with lifespan it sets density |
| Density | persons per settled patch, relative to humans | Prospect population; derived from fertility, or set |
| Biome weights | 16 numbers 0–3 (§4.1) | habitat score |
| Elevation weights | 5 numbers 0–3 plus underground and ocean (§4.2) | habitat score |
| Bedrock weights | optional, 9 numbers | dwarves, gnomes, kobolds, dragonkin, fire-touched |
| Water weights | river, lake, coast, harbour | lizardfolk, halflings, merfolk |
| Supernatural weight | −1 to +3 | elves, dark elves, undead, planetouched; negative for mundane races |
| Prospect affinity | 0–3 (§4.2) | whether the race lives in towns, beside them or far from them |
| Resources it lives on | subset of the 24 | which patches it values; what it trades for |
| Disposition | playable / mixed / hostile | which faction archetypes it can found (factions.md §3) |
| Default stance to other races | −3 … +3 per race (§4.3) | seeds faction relations, as factions.md §5.1 does |
| Faction archetypes it seeds | list of factions.md ids | e.g. dwarf → H3, D5, D2, C4; elf → H5, B7, G4 |
| Variants | list, each with its own weight overrides | §5.3 |
| Name style | Common / Exotic / Dark, or a future model | `NameGenerator-Reuse.md`: elves → Exotic, dark elves → Dark, humans → Common |
| Habitat kind | surface / elevation / underground / aquatic / follower | how the habitat score is computed |

### 5.2 Habitat score

For each land patch and each race, a pure function of the Planned-stage data:

```
habitat(race, patch) = biome[race][patch.biome]            # 0–3, estimated biome
                     · elevation[race][zone.type] / 3
                     · (1 + bedrockBonus[race][zone.bedrock])
                     · (1 + waterBonus(race, patch))
                     · (1 + supernaturalWeight[race] · (S − 0.5))   # S = Supernatural Field 0–1
```

A 0 in either the biome or the elevation weight rules the patch out, which is what Total War's "uninhabitable" and Age of Wonders 3's "hated" do. A 1 is the "unpleasant" middle. Stellaris' scheme (80 % in the exact class, 60 % in the same climate group, 20 % elsewhere) is an alternative that needs fewer numbers per race: give each race one native biome and derive the rest from three climate groups (cold: Ice & snow, Tundra, Boreal forest, Cold steppe; dry: Cold desert, Hot desert, Thorn scrub, Savanna; wet: the forests, Temperate grassland, Woodland & shrubland, Mangrove). The 0–3 table is more expressive; the Stellaris scheme is cheaper to tune.

### 5.3 Variants from the biome

Many sources split one race by habitat (§1.4 finding 3). The generator can pick the variant from where the race lives instead of drawing it: a troll community in Tropical rain forest is a jungle troll, in Ice & snow an ice troll; an elf community in Boreal forest is a pale elf; a human community on Cold steppe is a steppe-nomad culture. Each variant is a small override of the base weights and of the name style, portrait prompt and faction archetypes. This keeps the number of races small and the visible variety large.

### 5.4 Distribution: region, faction, family, person

A suggested layering, from coarse to fine, so every level stays a pure function of the foundation and order-independent:

1. **World level: which races exist.** The Parameter Set lists the enabled races and a global weight for each. A "classic" set (humans, elves, dwarves, halflings, orcs, goblins) matches 9–20 of 20 rosters (§1.3); exotic races (group J) can be off by default.
2. **Race field.** For each race, a race presence field: a low-frequency noise per race, like the climate fields, multiplied by its habitat score. This lets dwarves cluster in some mountain ranges and not others, and makes the race map a pure function of coordinates. Sampled at Planned.
3. **Region level: the peoples of a region.** From the race fields over the region's land patches, take the races with the largest summed score: a **majority race** (the ruling people of the region's A1 realm in factions.md), zero to two **minority races** in their own patches (an elven forest, a dwarven range), and **diaspora** shares in the towns (halflings, gnomes, half-breeds, planetouched, ratfolk: the races with Prospect affinity 3).
4. **Faction level.** Each faction gets a race: a kin-based faction (factions.md H2–H6, A2 noble house) gets the race of its seat patch; an urban faction (guilds, churches, crime) gets the majority race of its city with a chance of a minority race. Some faction archetypes are tied to a race (H3 → dwarf, H5 → elf, H6 → goblinoids).
5. **Family level.** A family has one race, drawn from its home Prospect's mix; servants and slaves (`readme.md`, "Family") can be of another race, typically a minority or a subject people.
6. **Person level.** The person inherits the family's race. A **mixed-blood chance** per family, raised where two races both have high presence, produces half-breeds (D1, D2); a small **touched chance** per birth, raised by the Supernatural Field, produces planetouched (H1, H2). Name style follows the race (§5.1).

Hostile races (C4, C5, E2, G2, I1, J11) normally get no families in Prospects; they appear as factions.md wilderness factions with seats away from Prospects (factions.md §5.3), and as Persons only for leaders the player can meet.

### 5.5 Mixing and half-races

- **Which pairs can mix.** The sources only mix humans with elves and with orcs as named races [S51] [ARC], and with giants (half-ogres) [ARC]; Pathfinder generalises half-races into heritages that attach to any parent [PF2-VH]. A small table of allowed pairs (human–elf, human–orc, human–giant-kin, human–any for planetouched) keeps the population believable.
- **Where.** Where both races have high presence in the same Prospect, and in border regions between their ranges.
- **How much.** A few per cent of families in a mixed town; more in a frontier region.
- **What the half-race inherits.** Lifespan between the parents (half-elf 180 [S51]); habitat as the host community; name style of the parent whose family it lives in.

### 5.6 Open questions

1. **Domain model.** Race is not in the readme's Person Stub list. Is race a Stub fact of Person (and Family), a fact of Faction, or a separate "people" layer under factions? This repeats factions.md §5.5 item 4; it touches the domain model, so the user decides before any code. The readme has authority over the names.
2. **Which races at all.** The readme allows "fantasy/magical elements" and lists elven fantasy resources (Elanor sap), so at least elves seem intended. Whether the game has non-human peoples, only the classic six, or a long list, is a product decision.
3. **Stage.** A race map must be ready when factions are made (Planned), which reads the estimated biomes of the Detailed stage. Race fields are pure functions, so they can be sampled at Planned with no new precondition. Region-to-region consistency (a dwarven range across a region border) comes free from the field, not from reading the neighbour.
4. **Purpose numbers.** Race fields and every per-family and per-person race draw need `Hashing` purpose numbers that `map-generation.md` would have to assign.
5. **Underground.** Dark elves, deep dwarves, deep gnomes and kobolds live underground. The map has no underground layer; for now "underground" can mean Hills and Mountains zones with a hidden seat.
6. **Sea peoples.** Merfolk need ocean patches next to land to be inhabited. Prospects are land-only (`map-generation.md` §15). Either merfolk are factions without Prospects, or the Prospect model grows a sea kind.
7. **Density and Prospect score.** Should a race's density change the number of Prospects or only their population? The Prospect score is human-centred (grain, rivers, trade); a dwarven region scores low on food but should still carry a hold.
8. **Name models.** There are three name styles (Common, Exotic, Dark). Dwarves, orcs, goblins and the beastfolk would need new models or a mapping onto the three; the doc forbids regenerating the existing nine.
9. **Proprietary look-alikes.** Some archetypes (bull-folk, catfolk, trolls) are close to well-known proprietary designs. Authored text and portraits should stay generic; the LLM and diffusion prompts should not name proprietary races.

### 5.7 Gaps in this research

- **Wikis refused the fetch tool:** UESP, the Dwarf Fortress wiki, Fandom (Total War, Age of Wonders 3, TV Tropes), the Against the Storm wiki, dnd-wiki.org; the Songs of Syx wiki aborted. Their claims rest on search excerpts and are marked [partially verified].
- **Total War's per-race climate table** was not read; only the mechanism is confirmed [TWW-C] [TWW-CF] [partially verified]. The same goes for Age of Wonders 3, where only dislikes and hates were seen, not likes [AOW3-C] [partially verified].
- **D&D monster habitats.** The SRD 5.2 monster data on Open5e has empty environment fields, and the Roll20 habitat page could not be read in full, so the biome leans of the D&D monster peoples (gnolls, ogres, fishfolk) are general knowledge, marked [unverified].
- **Dwarf Fortress raw values.** The entity `BIOME_SUPPORT` numbers per civilisation [DF-BS] would give a ready-made biome weight table; the raws were not reachable.
- **Not surveyed:** Warlock, Majesty, Endless Space; Mount & Blade's culture terrain is only inferred from faction blurbs [MB2-MF] [partially verified].
- **Lifespans and density** for most beastfolk are not in the sources; density values in §3 are all Design notes.

---

## 6. Sources

### 6.1 Tabletop (first-party or licensed)

- **S51** — Wizards of the Coast, *System Reference Document 5.1* (CC-BY-4.0), races and subraces, via the Open5e API. https://api.open5e.com/v1/races/?document__slug=wotc-srd&format=json
- **SRD51-M** — *D&D 5th Edition System Reference Document* (2016), monster index (bugbear, centaur, duergar, drow, gnoll, goblin, hobgoblin, kobold, lizardfolk, merfolk, minotaur, orc, sahuagin, ogre, troll, harpy, deep gnome, giants, vampires, mummies), dnd-srd-sphinx. https://dnd-srd-sphinx.readthedocs.io/en/latest/monster-descriptions.html
- **S52** — Wizards of the Coast, *D&D Free Rules (2024)*, "Character Origins" (species: dragonborn, dwarf, elf, gnome, goliath, halfling, human, orc, tiefling), D&D Beyond. These are the species of SRD 5.2 [unverified: the page does not name SRD 5.2]. https://www.dndbeyond.com/sources/dnd/free-rules/character-origins
- **PF2-ANC** — Archives of Nethys (Paizo-licensed), Pathfinder 2e "Ancestries" index with rarity and one-line descriptions. https://2e.aonprd.com/Ancestries.aspx
- **PF2-DWARF**, **PF2-ELF**, **PF2-GNOME**, **PF2-GOBLIN**, **PF2-HALFLING**, **PF2-LESHY**, **PF2-ORC** — Archives of Nethys ancestry pages. https://2e.aonprd.com/Ancestries.aspx?ID=59, …?ID=60, …?ID=61, …?ID=62, …?ID=63, …?ID=65, …?ID=66
- **PF2-CEN**, **PF2-MER**, **PF2-MIN**, **PF2-CAT**, **PF2-HOB**, **PF2-KOB**, **PF2-LIZ**, **PF2-RAT**, **PF2-TENGU** — Archives of Nethys ancestry pages. https://2e.aonprd.com/Ancestries.aspx?ID=73, …?ID=74, …?ID=75, …?ID=77, …?ID=78, …?ID=80, …?ID=81, …?ID=82, …?ID=83
- **PF2-VH** — Archives of Nethys, "Versatile Heritages" (aiuvarin, dromaar, nephilim, dragonblood, geniekin, beastkin, changeling and others). https://2e.aonprd.com/Ancestries.aspx?Versatile=true

### 6.2 Tolkien

- **TOLK-ELF** — Wikipedia, "Elves in Middle-earth" (read to character 100,000 of 180,000). https://en.wikipedia.org/wiki/Elves_in_Middle-earth
- **TOLK-DW** — Wikipedia, "Dwarves in Middle-earth" (read to character 100,000 of 150,000). https://en.wikipedia.org/wiki/Dwarves_in_Middle-earth
- **TOLK-HOB** — Wikipedia, "Hobbit" (read to character 100,000 of 172,000). https://en.wikipedia.org/wiki/Hobbit
- **TOLK-ORC** — Wikipedia, "Orc (Middle-earth)" (read to character 100,000 of 190,000). https://en.wikipedia.org/wiki/Orc_(Middle-earth)

### 6.3 Games: developer material and first-party pages

- **MOM-DD** — MuHa Games, *Master of Magic* (2022) Dev Diary #4, October 2021 (eight of the fourteen races, their planes and traits). https://muhagames.com/mom-dev-diary-4-october-2021

### 6.4 Games: community wikis

- **WOW-PR** — Warcraft Wiki (wiki.gg), "Playable race". https://warcraft.wiki.gg/wiki/Playable_race
- **WOW-TAU**, **WOW-TRO**, **WOW-GOB**, **WOW-DWF**, **WOW-NE**, **WOW-GNO** — Warcraft Wiki, "Tauren", "Troll", "Goblin", "Dwarf", "Night elf", "Gnome" (each read to character 100,000). https://warcraft.wiki.gg/wiki/Tauren, https://warcraft.wiki.gg/wiki/Troll, https://warcraft.wiki.gg/wiki/Goblin, https://warcraft.wiki.gg/wiki/Dwarf, https://warcraft.wiki.gg/wiki/Night_elf, https://warcraft.wiki.gg/wiki/Gnome
- **H3-T** — Heroes 3 wiki (heroes.thelazy.net), "Native terrain". https://heroes.thelazy.net/index.php/Native_terrain
- **H3-C** — Heroes 3 wiki, "List of creatures" (town rosters; read to character 100,000 of 165,000). https://heroes.thelazy.net/index.php/List_of_creatures
- **RW-X** — RimWorld Wiki, "Xenotypes". https://rimworldwiki.com/wiki/Xenotypes
- **ST-HAB** — Stellaris Wiki, "Habitability" (climate groups; 80/60/20 %). https://stellaris.paradoxwikis.com/Habitability
- **ST-SP** — Stellaris Wiki, "Species" (species classes). https://stellaris.paradoxwikis.com/Species
- **ST-BT** — Stellaris Wiki, "Biological traits" (read to character 100,000 of 315,000). https://stellaris.paradoxwikis.com/Biological_traits
- **EL-MF** — Endless Legend Wiki (wiki.gg), "Minor Factions" (17 minor factions; no habitat given). The centaur and minotaur minor factions are carried from `factions.md` [EL-MF]. https://endlesslegend.wiki.gg/wiki/Minor_Factions
- **POE-SR** — Pillars of Eternity II Wiki (Fextralife), "Subraces" (aumaua island and coastal, dwarf boreal and mountain, elf pale and wood, four godlike, three human folk, orlan hearth and wild). https://pillarsofeternity2.wiki.fextralife.com/Subraces
- **POE-H** — Fextralife and Pillars of Eternity Wiki pages on the human folk (Meadow Folk, Ocean Folk, Savannah Folk origins). https://pillarsofeternity2.wiki.fextralife.com/Meadow+Folk, https://pillarsofeternity.fandom.com/wiki/Ocean_folk **[partially verified: search excerpts]**
- **AOW3-C** — Age of Wonders 3 Wiki (Fandom), race terrain and climate likes, dislikes and hates. https://age-of-wonders-3.fandom.com/wiki/Racial_Governance **[partially verified: search excerpt; Fandom returned 402]**
- **TWW-CF** — Total War: Warhammer Wiki (Fandom), "Jungle" (climate preferences, penalties, hordes exempt). https://totalwarwarhammer.fandom.com/wiki/Jungle **[partially verified: search excerpt; Fandom returned 402]**
- **TWW-R** — Wargamer, "Total War: Warhammer 3 factions" (race list), as in `factions.md`. https://www.wargamer.com/total-war-warhammer-3/factions **[partially verified: not re-read in this pass]**
- **DF-CIV** — Dwarf Fortress Wiki, "Biome", "Civilization", "World gen" (dwarves start in mountains, humans in grassland, savanna or shrubland, kobolds in caves; the five default civilised races). https://dwarffortresswiki.org/index.php/Biome, https://dwarffortresswiki.org/index.php/v0.34:Civilization **[partially verified: search excerpts; the wiki returned 429]**
- **DF-BS** — Dwarf Fortress Wiki, "Entity token" (`BIOME_SUPPORT`, creature tokens weight a civilisation's race mix). https://dwarffortresswiki.org/index.php/Entity_token **[partially verified: search excerpts]**
- **DF-SITE** — Dwarf Fortress Wiki, "Site", "Forest retreat", "Dark fortress", as in `factions.md`. https://dwarffortresswiki.org/index.php/Site **[partially verified: search excerpts]**
- **TES** — UESP pages on Skyrim, Hammerfell, Black Marsh, Valenwood, Elsweyr and the Orcs; the archived elderscrolls.com "People and Places in Tamriel". https://en.uesp.net/wiki/Lore:Places, https://content3.uesp.net/wiki/General:Elderscrolls.com_Archive/People_and_Places_in_Tamriel **[partially verified: search excerpts; UESP returned 403]**
- **WIT-R** — Witcher wiki mirrors and guides on the elder races, Mahakam and the Conjunction of the Spheres. https://breezewiki.discard.no/witcher/wiki/Elf, https://consolepulse.com/multiplatform/the-witcher/guides/witcher-human-conquest-fall-elder-races **[partially verified: search excerpts]**
- **SOS** — Songs of Syx Wiki, "Race" and "Climate". https://songsofsyx.com/wiki/index.php/Race, https://songsofsyx.com/wiki/index.php/Climate **[partially verified: search excerpts; the fetch aborted]**
- **ATS** — Against the Storm Wiki (Hooded Horse), "Species", "Lizards", "Lizard House". https://wiki.hoodedhorse.com/Against_the_Storm/Species, https://wiki.hoodedhorse.com/Against_the_Storm/Lizard_House **[partially verified: search excerpts; the wiki returned 403]**
- **MB2-MF** — Mount & Blade II: Bannerlord Wiki (Fextralife), "Factions" (culture blurbs; terrain only by inference). https://mountandblade2bannerlord.wiki.fextralife.com/Factions **[partially verified]**

### 6.5 Games: press, guides and encyclopaedias

- **TWW-C** — Gamepressure, "Total War Warhammer 3: Settlement development, buildings and climate" (three suitability levels and the penalties for the worst one). https://www.gamepressure.com/total-war-warhammer-3/settlement-development-buildings-and-climate/zdf977
- **AOW1** — Wikipedia, "Age of Wonders" (the twelve races of the first game). https://en.wikipedia.org/wiki/Age_of_Wonders; Shadow Magic's Nomads from a GameSpot review excerpt https://gamespot.com/reviews/age-of-wonders-shadow-magic-review/1900-6073277/ **[partially verified for the Shadow Magic races]**
- **MOM-W** — Wikipedia, "Master of Magic" (fourteen races; elves, halflings and the insectoid Klackons named; two planes). https://en.wikipedia.org/wiki/Master_of_Magic
- **ARC** — Wikipedia, "Arcanum: Of Steamworks and Magick Obscura" (races; elves and magic, dwarves and technology). https://en.wikipedia.org/wiki/Arcanum:_Of_Steamworks_and_Magick_Obscura
- **DOS2** — Wikipedia, "Divinity: Original Sin II" (elves, dwarves, lizards, each living or undead; humans). https://en.wikipedia.org/wiki/Divinity:_Original_Sin_II
- **DAO** — Wikipedia, "Dragon Age: Origins" (human, elf and dwarf origins; the Qunari; read to character 100,000 of 140,000). https://en.wikipedia.org/wiki/Dragon_Age:_Origins
- **BG3** — Baldur's Gate 3 race guides (eleven races; dwarf, gnome, drow and tiefling subraces). https://alcasthq.com/baldurs-gate-3-races-and-subraces-list/, https://bg3.wiki/wiki/Race **[partially verified: search excerpts]**
- **KEN** — pindrop.gg, "Kenshi races" (four families, nine playable variants; robot people need no food). https://pindrop.gg/kenshi/races **[partially verified: search excerpt]**
