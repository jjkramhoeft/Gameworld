# Factions: Recurring Archetypes in RPGs, Fantasy and Building Games, as Input for the Generator

*Research date: 2026-11-14. Scope: what kinds of **factions** computer RPGs, fantasy strategy games, tabletop settings and city or kingdom builders use again and again, reduced to generic **archetypes** that a generator can place in a generated world. §1 states the method. §2 sets out the axes that tell factions apart. §3 is the curated list of 48 archetypes, each with goals, structure, where it is based (in the readme's terms: elevation type, biome, bedrock, coast and river, city or town Prospect, the Prosperity and Supernatural Fields), the resources it lives on, its usual friends and enemies, its variants and the games and sources it is drawn from. §4 collects the recurring rivalries and alliances in a matrix. §5 is a design note on what a generator needs per faction and how factions spread over settlement sizes and terrain, with the open questions. §6 lists the sources.*

**How to read the citations**

- Each claim cites the source that owns it, by key in square brackets. The keys are listed in §6. For games, the order of trust is the same as in `game-resources-and-trade.md`: developer material (dev blogs, dev diaries, official rules text), then established community wikis, then press and guides.
- **[partially verified]** means the source exists and is cited, but the claim was confirmed only from a search-index excerpt or a secondary source that quotes it, not by reading the page itself. In this pass the UESP, Fandom, Dwarf Fortress and TaleWorlds sites refused the fetch tool (403, 402 or 429), so most Elder Scrolls, Dwarf Fortress, Witcher, Dragon Age, Fallout and Mount & Blade claims are at this level.
- **[unverified]** means general knowledge of the game or of history that was not checked against any source in this pass.
- Game-specific proper names appear only as **evidence**. The archetype names are generic and are the recommended names for the generator. No archetype is a copy of one game's faction.
- Everything under **Base**, **Resources** in the readme's terms, and every **Design note**, is my own engineering judgement: it maps the archetype onto this repo's concepts, it is not a sourced claim.

**Hooks in this repo** (working tree at commit `280db6c`)

| Hook | Where | What it gives a faction pass |
|---|---|---|
| Faction | `readme.md`, "Faction" | "A group of people in one region. A political grouping. Could be a Kingdom, a thieves guild or a tribe of people." |
| Identity | `readme.md`, "Identity" | City, Faction, Family and Person = parent + slot index. A faction is addressable as a Stub before it is Realised |
| Region stage | `readme.md`, "Region Generation Stages" | Factions are made in the **Planned** stage, together with the Prospects, and read the Detailed data (estimated biomes and resources). Actual cities, families and important persons come in **Settled** |
| Region size | `readme.md`, "Region" | A region is the size of a medieval European kingdom (128 km by default), so one realm-level ruler per region is the natural default |
| Zone | `readme.md`, "Zone"; `map-generation.md` §8, §14.1 | Elevation type (Ocean, Flat Land, Lowland, Hills, Mountains) and bedrock (shield, fold belt, granite, volcanic, basalt, ophiolite, carbonate, clastic, alluvium) |
| Patch | `map-generation.md` §13, §14 | One of 15 land biomes or Mangrove; 24 raw resources with an amount in [0, 1] and a variant |
| Water site | `map-generation.md` §14.1, §15.1 | `river` (navigable from 8, large from 80), `mouth`, `lake`, `harbour`, `coast`, `confluence`, `head` (upstream end of navigation), `border`, `depth` (steps to the region border) |
| Prospect | `readme.md`, "Prospect"; `map-generation.md` §15 | 20 per region, up to 4 for cities; class Rich, Average or Poor; on show or not |
| Fields | `readme.md`, "Fields" | Prosperity and Supernatural, sampled anywhere |
| Travel network | `map-generation.md` §16 | Roads, river routes and sea lanes between the Prospects on show |
| Earlier question | `settlement-siting.md` §6.14 item 8 | "Should the county towns seed faction capitals?" |

---

## 1. Overview and method

**Scope.** A faction here is any organised group with its own goals that a player can deal with as a whole: join, serve, fight, bribe or be hunted by. That is wider than the political factions of strategy games (realms and vassal blocs) and wider than the joinable guilds of RPGs, and it covers both, because the readme's own examples (a kingdom, a thieves' guild, a tribe) span both.

**Sample.** Twenty-odd games and settings, chosen to cover the three genres the task names:

- **Computer RPGs:** The Elder Scrolls III–V (Morrowind, Oblivion, Skyrim), Dragon Age, The Witcher, Pillars of Eternity II: Deadfire, Fallout: New Vegas, Baldur's Gate (through the Forgotten Realms).
- **Fantasy strategy:** Heroes of Might and Magic III, Age of Wonders 4, Endless Legend, Total War: Warhammer, Mount & Blade II: Bannerlord, Crusader Kings III.
- **City, colony and kingdom builders:** Frostpunk 2, Manor Lords, Anno, Dwarf Fortress, RimWorld.
- **Tabletop, as the most explicit faction design writing:** the D&D 5e Basic Rules (the five Forgotten Realms factions), Pathfinder (Paizo's *Adventurer's Guide* organisations), Blades in the Dark (the faction ladder) and Worlds Without Number (faction turns).
- **History, as grounding:** merchant and craft guilds, the Hanseatic League, monastic and military orders, the Inquisition, condottieri, communes, hermandades, steppe nomads, the Nizari "Assassins", the Victual Brothers.

**Method.** For each source I noted the factions, what they want, how they are ranked, where they sit and who they fight. I then grouped factions that do the same job in their world into one archetype and kept an archetype only if it recurs in at least three independent sources or is grounded in a real institution. Proper names are evidence only.

**Five findings shape everything below.**

1. **Nearly every fantasy faction is a real medieval institution with one fantasy twist.** Mages' colleges are universities or guilds; holy orders are the Templars and the Teutonic Order; trading companies are the Hansa or the Dutch East India Company, which Obsidian names as its model [POE-29]; Bannerlord's minor factions are modelled on "historical or legendary bands" such as the Jomsvikings and the Fianna [MB2-DB7]. So history gives the structure (ranks, economics, siting) and the fantasy layer adds a few kinds that have no real counterpart (necromancers, beastfolk, the elder races).
2. **Factions split cleanly by what binds them.** Kin, faith, trade, craft, crime, arms, magic and rule are the recurring bases (§2). The base predicts the structure, the seat and the rivals better than any game-specific label.
3. **Settlement size decides which factions can exist.** Guilds, thieves' guilds and colleges need a city; temples and militias need a town; tribes, bandits, druids and cults live away from Prospects. Dwarf Fortress makes this explicit: each kind of entity builds its own kind of site (elven forest retreats, goblin dark fortresses, dwarven hillocks, bandit camps, monastic orders' monasteries) [DF-SITE].
4. **Terrain affinity is a first-class rule in the strategy games.** Each Heroes III town has a native terrain [H3-T]; Total War: Warhammer II and III give every race climate preferences and penalise settling the wrong climate [TWW-C]; Age of Wonders 4 ties dwarves to underground movement and quarries [AOW4-DD5]. A generator can do the same with the biome and elevation type it already has.
5. **Relations are few and structural.** The same pairs fight in every setting: thieves against the watch, mages against witch hunters, holy order against cult, merchant company against pirates, crown against rebellious nobles, settlers against forest folk. A small rivalry table (§4) plus a status score per pair, as in Blades in the Dark (−3 war to +3 allies) [BITD-FG] or RimWorld's goodwill (−100 to +100, hostile below −75, ally from +75) [RW-F], is what the games actually run on.

---

## 2. Taxonomy: the axes that tell factions apart

Seven axes are enough to describe every archetype in §3. Each archetype entry gives its values on the first five; the last two are per-instance values the generator draws.

### 2.1 Basis: what binds the members

| Basis | Members are bound by | Typical examples |
|---|---|---|
| **Rule** | holding land and the right to rule it | realm, noble house, free city, commune |
| **Kin** | descent, a people or a language | tribe, clan, horde, dwarven hold, elven enclave |
| **Faith** | a god, a creed or a cosmic duty | church, monastery, holy order, cult, druids |
| **Trade** | money, contracts and markets | merchant guild, league, company, bank |
| **Craft** | a skill and a monopoly on it | craft guild, miners, artificers |
| **Arms** | violence for pay, honour or duty | mercenaries, knights, wardens, militia |
| **Arcane** | magic and the knowledge of it | college, cabal, necromancers |
| **Crime** | illegal profit | thieves, assassins, smugglers, bandits, pirates |
| **Cause** | a political or moral goal | spy network, resistance, rebels, healers |

Blades in the Dark groups its city factions in much the same way (underworld, institutions, labour and trade, the fringe) [BITD-FG] [partially verified for the category names], and Paizo's *Adventurer's Guide* lists 19 organisations that fall into these bins: swordsmen, knights, assassins, thieves, scholars, druid-like animists, barbarian tribes, rebels, monasteries [PF-AG].

### 2.2 Scale

| Scale | Reach | In this repo |
|---|---|---|
| **Local** | one settlement or one valley | one Prospect, or a few patches |
| **Regional** | one realm | one region (the readme's definition of a faction) |
| **Supra-regional** | several realms | several regions; see the open question in §5.5 |

Most factions in CK3 and the Paradox games are realm-internal (vassal blocs inside one realm) [CK3-F]; most RPG guilds are supra-regional networks with a chapter in each city (the Elder Scrolls guilds, the Harpers, the Zhentarim) [DND-5F] [SKY-G].

### 2.3 Legality and stance toward the ruler

| Value | Meaning |
|---|---|
| **Ruling** | is the government |
| **Sanctioned** | chartered or licensed by the ruler; pays tax, gets privileges |
| **Tolerated** | legal grey zone; bribes and favours |
| **Outlawed** | hunted when found |
| **Independent** | outside any ruler's reach (wilderness, sea, another realm) |

The same archetype can take several values: Morrowind's assassins' guild is legal [MW-G] [partially verified], most others are outlawed; CK3 lets a ruler revoke a holy order's leases [CK3-HO].

### 2.4 Settlement affinity

| Value | Lives in |
|---|---|
| **Urban** | cities (a Prospect for a city), sometimes large towns |
| **Town** | any town or city |
| **Rural** | villages and farmland around Prospects |
| **Wilderness** | away from Prospects: forest, mountains, desert, steppe, swamp |
| **Maritime** | coast, harbours, islands, the sea itself |
| **Itinerant** | moves along the travel network |

### 2.5 Terrain affinity

Elevation type, biome, bedrock, coast and river, and the two Fields. This is the axis the strategy games use for placement: native terrain in Heroes III [H3-T], climate suitability in Total War: Warhammer [TWW-C], terrain bonuses for Primal cultures in Age of Wonders 4 [AOW4-PF] [partially verified]. Each entry in §3 states it in the repo's terms.

### 2.6 Strength (per instance)

How big and rich one instance is. Blades in the Dark rates it as **tier** (0 to VI, with a gang of 1–2 people at tier 0 and about 80 at tier V) and **hold** (weak or strong: how firmly it keeps its tier) [BITD-FG]. CK3 sizes mercenary companies by the number of counties of their culture (one to three companies, 400 to 1,200 levies) [CK3-M].

### 2.7 Disposition (per instance)

How it treats the player and each other faction. Blades: status −3 (war) to +3 (allies) [BITD-FG]. RimWorld: goodwill −100 to +100, with fixed ranges per faction type (a "savage tribe" is locked at −100, a "gentle tribe" floats between −50 and +50) [RW-F]. Fallout: New Vegas tracks fame and infamy separately per faction [FNV-R] [partially verified].

---

## 3. The curated archetypes

48 archetypes in nine groups. Every entry uses the same fields. **Relations** name other archetypes by their number. The axis line reads: basis · scale · legality · settlement affinity.

### Group A. Rule and land

#### A1. Realm crown

The ruling house and court of a whole realm: the king, prince or khan with the high officers, the treasury and the royal army. In strategy games it is the player's own faction or the major AI faction; in RPGs it is the power that the plot turns on.

- **Axes:** rule · regional · ruling · urban (the capital).
- **Goals:** keep the realm together, raise taxes and levies, hold the borders, keep vassals and the church in check.
- **Structure:** ruler, heir, council of great officers (chancellor, marshal, steward, spymaster in CK3's council [unverified]), royal household, appointed governors.
- **Base:** the best city Prospect of the region (Rich, on a navigable river or a harbour, low `depth`, on the travel network). Any biome that carries cities; Flat Land and Lowland most often.
- **Resources:** taxes from every Prospect; grain and livestock for the army; iron, horses; gold and silver for coinage.
- **Relations:** allied with A2 (loyal houses), B1, D2, E3; rival to A2 (rebellious houses), A6, A7 when it rises, F-group criminals, H1 on the border.
- **Variants:** feudal kingdom; elective monarchy; empire with client kings; khanate (see H1); theocracy (merge with B1).
- **Evidence:** the readme's own example ("a Kingdom"); Bannerlord kingdoms with their minor factions [MB2-MF] [partially verified]; CK3 realms [CK3-F]; Total War: Warhammer races [TWW-R] [partially verified]; the five Heroes III and Age of Wonders 4 player realms [H3-T] [AOW4-DD5].

#### A2. Noble house

A landed family with its retainers: a duke, count or baron, holding a castle and a set of villages, owing service to the crown and pursuing its own interest.

- **Axes:** kin and rule · local to regional · sanctioned · rural (castle) and urban (town house).
- **Goals:** more land, a better marriage, a claim to a higher title, less crown authority.
- **Structure:** head of house, heir, kin, household knights, steward, bailiffs; vassal houses under it.
- **Base:** a town Prospect or a strong point: Hills on a river crossing or a pass, the edge of good farmland. Often near a region `border` (marcher lords).
- **Resources:** grain and livestock from its villages, timber, horses; tolls on roads and rivers.
- **Relations:** allied with A1 (if loyal), B1, E2; rival to other A2 houses, A1 (via A6), A7, F4 on its land.
- **Variants:** marcher house; cadet branch; merchant-noble (Morrowind's trading house) [MW-G] [partially verified]; warrior house; house of mages (Morrowind's wizard house in its towers) [MW-G] [partially verified].
- **Evidence:** CK3 vassal factions (liberty, independence, claimant) are blocs of noble houses [CK3-F]; Bannerlord clans [MB2-MF] [partially verified]; Morrowind Great Houses, rivals to each other [MW-G] [partially verified]; Manor Lords rival lords [ML] [partially verified].

#### A3. Free city council

A city that rules itself through a council of wealthy citizens, under a charter bought or won from a lord, often with its own militia and walls.

- **Axes:** rule and trade · local · ruling or sanctioned · urban.
- **Goals:** keep its charter and privileges, keep tolls low and markets open, keep lords out.
- **Structure:** mayor or doge, council of patricians drawn from the guilds (D1, D2), magistrates, city watch (E5).
- **Base:** a Rich city Prospect with a `harbour`, a river `mouth` or a navigable `confluence`; far from the capital or on another realm's border.
- **Resources:** trade through the travel network; fish, salt, cloth (fibre, dyes); grain bought in.
- **Relations:** allied with D1, D2, D3, E5; rival to A1 and A2 (who want its taxes), F5 (pirates), F2 (thieves) inside.
- **Variants:** Italian-style city-state; free imperial city; port republic; the "Lords' Alliance"-type league of city rulers (A4).
- **Evidence:** medieval communes and free imperial cities, which bought charters from bishops, counts and kings [COMMUNE]; merchant guilds dominating town councils by the 13th century [GUILD-B]; the D&D Lords' Alliance of northern cities [DND-5F].

#### A4. League of rulers

A defensive alliance of several cities or lords against a common threat, with a council but no single ruler.

- **Axes:** rule · supra-regional · ruling (its members) · urban.
- **Goals:** common defence, safe roads and trade, keeping a larger power or a monster threat at bay.
- **Structure:** a council of member rulers; agents and envoys; no standing administration.
- **Base:** its members' cities, linked by the travel network; along a coast or a great river.
- **Resources:** contributions from members; trade.
- **Relations:** allied with A3, D3, E4; rival to H1, H6, the threat it was formed against; internally, rivalry and pride between members [DND-5F].
- **Variants:** league of free cities; league of lords; alliance of elder races against a common enemy.
- **Evidence:** the Lords' Alliance (rulers of northern cities united against evil, with tension between member cities) [DND-5F]; town and rural leagues outlawed in the Holy Roman Empire by the Golden Bull of 1356 [COMMUNE]; the Hanseatic Diet, a consensus assembly with no permanent treasury or army [HANSA].

#### A5. Peasant commune

Villagers sworn together to manage their common land, defend their valley and, when pressed, to resist a lord.

- **Axes:** kin and rule · local · tolerated, or outlawed when it rises · rural.
- **Goals:** keep common pasture, water and woodland, low dues, safe roads.
- **Structure:** village assembly, elders or reeve, sworn men as militia.
- **Base:** Hills and Mountains valleys; poorer (Average and Poor) Prospects; high `depth`; Temperate grassland, Woodland & shrubland, Temperate mixed forest; alpine pastures.
- **Resources:** livestock, grain, timber, cheese and wool; salt from outside.
- **Relations:** allied with B2 (local monastery), E5; rival to A2 (the local lord), F4 (bandits); can turn into A6.
- **Variants:** alpine valley league; frontier settlers' commune; Frostpunk-style community with its own politics [FP2] [partially verified].
- **Evidence:** rural communes that managed shared resources and kept trade routes safe, among them the alpine communities that became the Old Swiss Confederacy [COMMUNE]; CK3's "popular factions" formed from counties with negative opinion [CK3-F]; Frostpunk 2's communities, smaller groups distinct from political factions [FP2] [partially verified].

#### A6. Rebels and pretenders

A movement to replace the ruler, break away, or wring concessions: a claimant's party, a separatist province or a peasant revolt.

- **Axes:** cause and rule · regional · outlawed · rural or urban.
- **Goals:** a new ruler (claimant), independence (separatists), less crown power (liberty), or a change of faith and culture (populist).
- **Structure:** a figurehead, a council of sponsoring nobles or leaders, a rebel army raised from members.
- **Base:** the far side of the region from the capital (high `depth` from the capital's Prospect, near the `border`), or a different culture's land within the region.
- **Resources:** whatever its member lands give; foreign gold.
- **Relations:** allied with a foreign A1, E1 (mercenaries), G1; rival to A1, B4.
- **Variants:** claimant faction; independence faction; peasant rabble; nomad uprising.
- **Evidence:** CK3's faction types (claimant, independence, liberty, dissolution, populists, peasant rabble, nomads) [CK3-F]; Paizo's rebels against an oppressive regime [PF-AG]; Frostpunk 2 factions that form in opposition to an existing one [FP2] [partially verified].

### Group B. Faith

#### B1. Established church

The official faith of the realm: a hierarchy of priests and temples with its own lands, courts and tithes.

- **Axes:** faith · regional to supra-regional · sanctioned or ruling · town and urban.
- **Goals:** souls, tithes, influence over the ruler, suppression of heresy and cults.
- **Structure:** high priest or archbishop, bishops per city, parish priests per town, temple guards; often a military arm (B3) and an inquisition (B4).
- **Base:** a cathedral in every city Prospect, a temple in every town; the seat of its head in the capital or in a holy site of its own.
- **Resources:** tithes on grain and livestock; church lands; incense (part of spices), wax, wine (orchard: grape).
- **Relations:** allied with A1, B2, B3, B4; rival to B5, B6, C1 (in many settings), C3, B7 (pagan druids) in some.
- **Variants:** state church; theocracy (the church is A1); pantheon of temples, one per god; ancestor cult.
- **Evidence:** Dragon Age's Chantry with its Templar Order [DA] [partially verified]; the Witcher's Church of the Eternal Fire [WIT] [partially verified]; Morrowind's Temple and Imperial Cult [MW-G] [partially verified]; Dwarf Fortress religions, which get a shrine and a priest as a site grows [DF-REL] [partially verified]; the medieval Church as owner of the Inquisition [INQ].

#### B2. Monastic order

A community of monks or nuns living by a rule, often in remote places, often the best farmers, brewers, scholars and metalworkers of the land.

- **Axes:** faith · local, part of a supra-regional order · sanctioned · wilderness and rural.
- **Goals:** a holy life, learning and copying books, care for the poor, wealth from land (in practice).
- **Structure:** abbot or prior, monks or nuns, lay brothers who work the farms, outlying farms (granges).
- **Base:** remote river valleys away from Prospects: a patch with a `river` but high `depth`, Hills or Lowland, Temperate mixed or broadleaf forest, marsh. Water power and solitude.
- **Resources:** livestock (wool), grain, orchard (wine), fish ponds, iron ore and forges, water mills.
- **Relations:** allied with B1, A5, B8; rival to A2 (over land), F4 (raiders).
- **Variants:** contemplative order; scholarly order (the library); warrior monks (see B3, E3); elemental monasteries [PF-AG].
- **Evidence:** the Cistercians: remote sites, marshland, a "remote river valley", lay brothers who ran the farms, wool, water mills in nearly every house, the leading iron producers of Champagne [CIST]; Dwarf Fortress monasteries, built by monastic orders headed by an abbot [DF-SITE] [partially verified]; Paizo's Houses of Perfection [PF-AG].

#### B3. Holy order of knights

Warrior monks: knights who take religious vows and fight for the faith, with commanderies spread across the land and a grand master at the top.

- **Axes:** faith and arms · supra-regional · sanctioned · rural strongholds and frontier.
- **Goals:** defend and spread the faith, fight its enemies (cults, undead, infidels), keep its estates.
- **Structure:** grand master, commanders per house (commandery), knights, sergeants, chaplains.
- **Base:** fortified houses on the frontier (region `border`, Hills, passes) and estates in the farmland; one chapter house per city.
- **Resources:** estates (grain, livestock), donations, banking; horses and iron for arms.
- **Relations:** allied with B1, A1 (its patron); rival to B5, C3, F-group, H5, the faith's foreign enemies; feared by rulers when it grows too rich.
- **Variants:** crusading order; hospital order (merges with G3); paladin order against evil (D&D) [DND-5F]; zealot order turned villain (the Witcher's knights of the Flaming Rose) [WIT] [partially verified]; law-enforcing order [PF-AG].
- **Evidence:** the military orders (Templars, Hospitallers, Teutonic Knights, Santiago, Calatrava), with lay members under monastic vows, commanderies across Europe, a grand master, papal protection and Templar banking [MILORD]; CK3 holy orders, founded by a king for 1,000 piety and 500 gold, based on leased holdings, hired by rulers of the same faith at war with a hostile faith [CK3-HO]; D&D's Order of the Gauntlet (paladins, monks and clerics devoted to fighting evil) [DND-5F]; Paizo's Hellknights [PF-AG].

#### B4. Inquisition and witch hunters

The church's court and enforcers against heresy, witchcraft and forbidden magic.

- **Axes:** faith · regional · sanctioned · urban, travelling.
- **Goals:** root out heretics, cultists, rogue mages and monsters in human shape.
- **Structure:** grand inquisitor, regional tribunals, inquisitors drawn from a preaching order, informers, armed escorts lent by the ruler.
- **Base:** cities with a cathedral; travels to wherever heresy is reported; more active where the Supernatural Field is high.
- **Resources:** confiscated property; church funds.
- **Relations:** allied with B1, B3, A1 (the secular arm); rival to C1, C2, C3, B5, B6, B7, F2 (when they shelter mages).
- **Variants:** church tribunal; secular witch-hunter brotherhood; mage-hunters who guard a sanctioned college (Dragon Age's Templars) [DA] [partially verified].
- **Evidence:** the medieval Inquisition: church judges, Dominican and Franciscan inquisitors, Cathars and Waldensians as targets, unrepentant convicts handed to the secular arm [INQ]; the Witcher's witch hunters of the Eternal Fire [WIT] [partially verified]; Dragon Age's Templars, who hunt apostate mages and watch the Circle [DA] [partially verified].

#### B5. Forbidden cult

The secret worshippers of a dark or banned power: a demon, a dead god, an elder horror. The standard villain faction of fantasy RPGs.

- **Axes:** faith · local cells of a supra-regional cult · outlawed · urban underground and wilderness shrines.
- **Goals:** summon or free its power, gain magic and immortality, convert or sacrifice.
- **Structure:** a hidden high priest, inner circle, cells in cities that do not know each other, dupes and thralls.
- **Base:** ruins, caves and wild places where the Supernatural Field is high; cells in cities under respectable cover. Volcanic arc bedrock, Ice & snow, deep forest or swamp make good shrine sites.
- **Resources:** gems and gold for rites; its members' wealth; captives.
- **Relations:** allied with C3, F3 (as hired knives), sometimes H5; rival to B1, B3, B4, B7, G1, everyone.
- **Variants:** demon cult; cult of a dead or banned god; doom cult; assassin cult (see F3); cult that converts minor peoples (Endless Legend's Cultists) [EL-C] [partially verified]; Dwarf Fortress persecuted religions [DF-REL] [partially verified].
- **Evidence:** the Red Mantis, "halfway between a cult and a guild" of assassins [PF-AON] [partially verified]; RimWorld's raiding cult (Anomaly) [RW-F]; Endless Legend's Cultists [EL-C] [partially verified]; the Heroes III Inferno town of demons, native to lava terrain [H3-T] [partially verified].

#### B6. Popular sect

A movement of ordinary believers who reject the church's authority: preachers, pilgrims, a reformed creed. Not dark, but heretical in the church's eyes.

- **Axes:** faith and cause · regional · tolerated or outlawed · town and rural.
- **Goals:** a purer faith, the poor over the rich, freedom from the hierarchy.
- **Structure:** wandering preachers, a few "perfect" leaders, lay followers; little hierarchy.
- **Base:** towns and villages far from the capital; trade routes (preachers travel with merchants).
- **Resources:** donations; members' crafts (weavers were proverbial heretics [unverified]).
- **Relations:** allied with A5, G3; rival to B1, B4.
- **Variants:** dualist heresy; pilgrims' movement; faith of a migrating people; Frostpunk 2's faith-based factions [FP2] [partially verified].
- **Evidence:** the Cathars and Waldensians, main targets of the Inquisition [INQ]; Frostpunk 2's Faithkeepers and Pilgrims, with "Tradition" in their ideology [FP2] [partially verified].

#### B7. Druidic circle

Keepers of the old nature faith and of the balance between the wild and the farmed: druids, rangers, wardens of the forest.

- **Axes:** faith · local circles in a supra-regional network · tolerated or independent · wilderness.
- **Goals:** protect wild land and sacred places, keep civilisation from taking too much, oppose the unnatural (undead, demons, aberrations).
- **Structure:** archdruid, circles of druids per forest, rangers and scouts; branches work on their own.
- **Base:** old forest with high `depth` and little farmland: Temperate rain forest, Temperate broadleaf forest, Boreal forest, Tropical rain forest; sacred groves and springs where the Supernatural Field is high.
- **Resources:** wild food & game, timber (as protectors, not loggers), herbs (part of wild food), honey.
- **Relations:** allied with H5 (elves), H4 (beastfolk), A5; rival to D5 (miners), D6 (loggers and settlers), C3, B5; uneasy with B1.
- **Variants:** druid circle; ranger company; animist shamans; storm priests of a sea or weather god [PF-AG].
- **Evidence:** D&D's Emerald Enclave, protecting nature and keeping civilisation and nature in balance, with branches that act on their own [DND-5F]; Paizo's Rivethun animists and Storm Kindlers [PF-AG]; Bannerlord's forest people of Sturgia [MB2-MF] [partially verified]; Heroes III Rampart (grass) and Age of Wonders 4 Primal cultures with totem animals and terrain bonuses [H3-T] [AOW4-PF] [partially verified].

#### B8. Pilgrimage shrine keepers

The guardians of a holy site that draws pilgrims: a shrine, a relic, a sacred mountain or spring. A small faction with outsized wealth and influence.

- **Axes:** faith · local · sanctioned · town (the shrine town).
- **Goals:** keep the shrine, its relics and its income; protect pilgrims.
- **Structure:** shrine priests, keepers, a guard; inns and hospices around it.
- **Base:** a singular spot: a mountain peak, a spring, a lake, a ruin, where the Supernatural Field peaks.
- **Resources:** pilgrims' gifts; inns; the road.
- **Relations:** allied with B1, G3, E5; rival to F4 (who prey on pilgrims), B5.
- **Variants:** relic shrine; oracle; healing spring; tomb of a saint or hero.
- **Evidence:** towns that guarded the pilgrim route to Santiago de Compostela with their brotherhoods [HERM]; the Elder Scrolls shrines to the gods [unverified]. Thin in the sample: few games give shrines a faction of their own.

### Group C. Arcane

#### C1. Mages' college

The sanctioned school and guild of magic: a tower or college that trains, licenses and governs mages.

- **Axes:** arcane · regional, often a chapter of a supra-regional guild · sanctioned · urban, or an isolated tower.
- **Goals:** knowledge, artefacts, the right to practise, freedom from church control.
- **Structure:** archmage, council of masters, magisters, journeymen, apprentices (the guild ladder).
- **Base:** a city Prospect, or a remote tower where the Supernatural Field is high: a cold coast (Skyrim's college at Winterhold [SKY-G] [partially verified]), Ice & snow (Heroes III Tower town on snow [H3-T] [partially verified]), a mountain.
- **Resources:** crystal, gems, rare metals; books; whatever fantasy reagents the Supernatural Field gives.
- **Relations:** allied with A1 (as court wizards), D7, G2; rival to B4, B1 (in many settings), C2, C3.
- **Variants:** university; guild of mages with a hall per city (Oblivion's guild [unverified]); a supervised circle in a tower (Dragon Age's Circle under the Templars) [DA] [partially verified]; ancient academy [PF-AG].
- **Evidence:** Skyrim's College of Winterhold, a guild for the study and practice of magic [SKY-G] [partially verified]; Dragon Age's Circle of Magi [DA] [partially verified]; Paizo's Magaambya and Cyphermages [PF-AG]; the D&D Harpers as a network of spellcasters [DND-5F].

#### C2. Secret cabal of mages

A closed circle of powerful mages who meddle in politics from behind thrones. Not evil by definition; self-interested by nature.

- **Axes:** arcane and cause · supra-regional · tolerated or hidden · urban (courts).
- **Goals:** keep magic in the hands of mages, steer kings, survive the next purge.
- **Structure:** a few members, each with a court position or a tower; no visible ranks.
- **Base:** royal courts in city Prospects; isolated towers (Morrowind's wizard house) [MW-G] [partially verified].
- **Resources:** its members' patrons; magic.
- **Relations:** allied with A1 (as advisers, while useful), G1; rival to C1 (the official order), B4, A1 (when exposed).
- **Variants:** lodge of sorceresses; house of wizard-lords; council of archmages.
- **Evidence:** the Witcher's Lodge of Sorceresses, a secret circle formed to preserve mage influence after the fall of the brotherhood of sorcerers [WIT] [partially verified]; Morrowind's wizard house, aloof from politics in its towers [MW-G] [partially verified].

#### C3. Necromancer cabal

Mages who raise the dead, and the undead who serve or rule them. In strategy games a whole race (the undead realm); in RPGs a hidden cabal.

- **Axes:** arcane · local to regional · outlawed · wilderness (ruins, barrows, swamps) or a ruling undead realm.
- **Goals:** immortality, power over death, an army that needs no food.
- **Structure:** a master necromancer or vampire lord, apprentices, thralls, undead.
- **Base:** ruins, old battlefields and burial grounds; swamps, Tundra, Cold desert; high Supernatural Field; away from Prospects (high `depth`). Heroes III Necropolis has dirt as native terrain [H3-T] [partially verified].
- **Resources:** almost none from the land (undead need none); gems, gold, corpses.
- **Relations:** allied with B5; rival to every faith faction (B1, B3, B4, B7), E4, G1.
- **Variants:** necromancer cabal; vampire court (Total War's vampire counts [TWW-R] [partially verified]; Morrowind's vampire clans [MW-G] [partially verified]); lich's realm; tomb kings of a dead empire [TWW-R] [partially verified].
- **Evidence:** Heroes III Necropolis [H3-T] [partially verified]; Total War: Warhammer Vampire Counts and Tomb Kings [TWW-R] [partially verified]; Age of Wonders 4's Dark culture [AOW4-DD24] [partially verified].

#### C4. Artificers' society

Engineers, alchemists and inventors who trust machines over spells or blend the two: makers of gunpowder, clockwork and siege engines.

- **Axes:** craft and arcane · regional · sanctioned · urban.
- **Goals:** progress, patents and monopolies, patronage from rulers.
- **Structure:** a guild ladder, a few famous masters, workshops.
- **Base:** cities near metal and fuel: carbonate, clastic and fold belt bedrock with iron, copper, coal; on a river for water power.
- **Resources:** iron ore, copper ore, tin, fuel (coal), salt and saltpetre; glass (crystal).
- **Relations:** allied with A1, D5, D2; rival to C1 (rival paths to power), B7 (which hates the machines), B1 (in conservative settings).
- **Variants:** alchemists' guild; gunpowder makers (Deadfire's RDC trades in gunpowder and saltpetre [POE-29]); runesmiths (Age of Wonders 4 dwarves) [AOW4-DD5]; technocrats (Frostpunk 2) [FP2] [partially verified].
- **Evidence:** Frostpunk 2 Technocrats ("Progress", "Reason") [FP2] [partially verified]; Age of Wonders 4 Industrious culture and its Runesmiths [AOW4-DD5]; Pillars of Eternity II's Rauatai cannon trade [POE-29]; Paizo's Cyphermages [PF-AG].

### Group D. Trade and craft

#### D1. Merchant guild

The town's association of traders, which controls markets, weights and measures, and often the council.

- **Axes:** trade · local · sanctioned · urban and town.
- **Goals:** monopoly on local trade, low tolls, a seat on the council, control of the market.
- **Structure:** guild master and wardens, members (merchants), factors abroad; a guildhall.
- **Base:** every city Prospect and larger towns; strongest at `harbour`, river `mouth`, `confluence` and `head` of navigation (break-of-bulk points, `settlement-siting.md` §2.3).
- **Resources:** anything traded: grain, salt, cloth (fibre, dyes), luxuries.
- **Relations:** allied with A3, D3, D8, E5; rival to D2 (over the council), F2, F6, F5.
- **Variants:** merchant guild; trading house; fair or market authority.
- **Evidence:** medieval merchant guilds, which appeared first and by the 13th century often dominated town councils [GUILD-B]; Patrician-style and Anno trading [unverified]; the Skyrim and Oblivion trading houses [unverified].

#### D2. Craft guild

The association of one trade in a town: smiths, masons, weavers, brewers, tanners. Sets prices and quality, trains apprentices, guards its monopoly.

- **Axes:** craft · local · sanctioned · urban and town.
- **Goals:** monopoly of its craft, fair prices, a share of town power, care for members' widows.
- **Structure:** apprentice (5–9 years, unpaid), journeyman (paid wages), master (with a masterpiece and a workshop); wardens; guild court; patron saint and chapel.
- **Base:** cities and towns. Which guilds a town has follows the resources around it: smiths where iron ore and fuel are, masons where stone is, weavers where fibre and wool (livestock) are, tanners where livestock and game are.
- **Resources:** one input resource each (iron ore, stone, fibre, livestock, clay, timber).
- **Relations:** allied with A3, D1 (often uneasy), B1 (chapels); rival to D1 (council seats), other craft guilds, outside craftsmen.
- **Variants:** smiths and armourers; masons and builders; weavers and dyers; brewers; Dwarf Fortress craft guilds [DF-ENT] [partially verified].
- **Evidence:** the three-tier guild ladder, monopolies, guild courts and religious services, and the clash with merchant guilds for town government [GUILD-B]; craft guilds in Dwarf Fortress world generation since 0.47.01 [DF-ENT] [partially verified]; Age of Wonders 4 "Great Builders" society trait [AOW4-DD5].

#### D3. Merchant league

A confederation of trading towns and their merchants that spans many realms, with trading posts abroad and the power to blockade a king.

- **Axes:** trade · supra-regional · sanctioned (with privileges) · urban and maritime.
- **Goals:** trading privileges, safe sea lanes, monopolies on bulk goods.
- **Structure:** member towns with one informal leader, an irregular diet deciding by consensus, regional circles, trading posts (kontors) with their own law abroad; no standing army or treasury.
- **Base:** port cities: `harbour`, river `mouth`, on sea lanes; northern coasts with Boreal forest and Temperate mixed forest behind them.
- **Resources:** bulk goods: grain, timber, furs, honey and wax, fish (herring, stockfish), salt, cloth, beer, metal ore.
- **Relations:** allied with A3, D1, B3 (the Teutonic Order was treated as a head of the Hansa); rival to F5 (pirates), A1 (kings who revoke privileges), D4 (rival companies).
- **Variants:** league of free ports; river league; guild of caravan cities.
- **Evidence:** the Hanseatic League: nearly 200 towns, kontors in Bergen, Bruges, London and Novgorod, the Hansetag, goods (wax, furs, timber, resin, flax, honey, grain, cloth, herring, salt, stockfish, beer), blockades and wars with Denmark [HANSA].

#### D4. Chartered company

A company with a charter from a state to trade, settle and fight overseas or on a frontier: the flag follows the trade.

- **Axes:** trade and arms · supra-regional · sanctioned (by a foreign crown) · maritime and frontier.
- **Goals:** profit for investors, monopoly of a frontier's goods, eventually land.
- **Structure:** directors, governors of posts, captains, soldiers and clerks; ranks like a navy for the more military kind.
- **Base:** coastal forts and trading posts at `harbour` patches in a region not its own; spices, sugar and dye country (Tropical rain forest, Savanna, Tropical dry forest, Mangrove).
- **Resources:** spices, stimulants & sugar, dyes, gems & pearls, furs; gunpowder and saltpetre in.
- **Relations:** allied with its home A1; rival to other D4 companies (espionage, sabotage), H2 (natives whose land it wants), F5 (pirates).
- **Variants:** profit company (Deadfire's VTC, modelled on the Dutch East India Company); imperial company run like a navy (Deadfire's RDC); colonial company.
- **Evidence:** Obsidian's description of the Royal Deadfire Company (naval structure, hawkish, wants the archipelago) and the Vailian Trading Company (charter to enrich investors, rivalry with the RDC, target of pirates) [POE-29]; RimWorld's orbital traders' guild [RW-F].

#### D5. Miners' guild

The privileged community of miners in a mining district, with its own law and courts, working the ore under licence from the ruler.

- **Axes:** craft · local · sanctioned · Hills and Mountains, mining towns.
- **Goals:** mining rights, freedom from the local lord, a fair share of the ore.
- **Structure:** mine masters, crews, a mining court, smelters and assayers.
- **Base:** zones with metal deposits: fold belt (silver, gold), granite (tin), volcanic arc (copper, gold), carbonate (lead, zinc), ophiolite (copper), shield (iron, gold); Hills and Mountains.
- **Resources:** iron ore, copper ore, tin, silver & lead, gold, gems; timber and fuel (charcoal) in.
- **Relations:** allied with A1 (who taxes the silver), D2 (smiths), C4, H3 (dwarves); rival to B7, A2 (local lord), F4.
- **Variants:** free miners; salt miners; quarrymen; a dwarven guild in human lands; Bannerlord's mining villages [MB2-V] [partially verified].
- **Evidence:** the Cistercians as iron producers with forges on ore deposits [CIST]; Hanseatic trade in metal ore [HANSA]; Age of Wonders 4 Great Builders boosting quarries [AOW4-DD5]; free miners' privileges [unverified].

#### D6. Frontier settlers' company

Pioneers who clear forest and swamp and found new villages under a lord's or a company's contract: the force that eats the wilderness.

- **Axes:** kin and trade · local · sanctioned · rural, at the edge of the wild.
- **Goals:** land of their own, low dues for the first years, protection from raiders.
- **Structure:** a locator or headman, settler families, a priest; a charter.
- **Base:** forest and marsh near but outside existing Prospects: Temperate mixed and broadleaf forest, Boreal forest, Lowland marsh; patches with fair farmland but no Prospect.
- **Resources:** timber, clay, grain once cleared.
- **Relations:** allied with A2, B2, B1; rival to B7, H2, H4, H5 (forest folk), F4.
- **Variants:** pioneer colony; Frostpunk-style survivors' community; Banished-style nomads who join a settlement [unverified].
- **Evidence:** the Cistercians clearing marginal land [CIST]; Frostpunk 2's frontier-minded factions (exploration and expansion abilities) [FP2] [partially verified]; the Witcher and Dragon Age settlers against the forest elves [WIT] [DA] [partially verified]. Thin as a named faction in games.

#### D7. Banking house

A family or order that lends money to kings and merchants, holds deposits, issues letters of credit.

- **Axes:** trade · supra-regional · sanctioned · urban.
- **Goals:** interest, repayment, leverage over debtors.
- **Structure:** a family head, branch managers in foreign cities, factors.
- **Base:** capitals and great trade cities; Rich Prospects with high Prosperity.
- **Resources:** gold and silver; trade flows.
- **Relations:** allied with A1 (the main debtor), D1, D3; rival to B1 (usury), A2 (debtors), other banks.
- **Variants:** merchant bank; military order acting as banker; moneylenders' guild.
- **Evidence:** Templar banking [MILORD]; the Aspis Consortium, a profit-driven syndicate [PF-AG]; the D&D Zhentarim's view of wealth as power [DND-5F]. Thin in games as a faction of its own [unverified].

#### D8. Caravan company

Traders who cross deserts, steppes and mountains with pack trains, and keep the wells, inns and guards that make it possible.

- **Axes:** trade · supra-regional · sanctioned · itinerant, desert and steppe towns.
- **Goals:** safe routes, low tolls, deals with the tribes along the way.
- **Structure:** caravan masters, guards, guides, a network of caravanserais.
- **Base:** the travel network across dry land: Hot desert, Cold desert, Cold steppe, Thorn scrub; oases (a `lake` or `river` in a desert patch); passes.
- **Resources:** horses & pack animals (camels in deserts, mules in hills), salt, spices, stimulants, dyes, gems.
- **Relations:** allied with H1 (paying for passage), D1, D3; rival to F4 (desert raiders), A1 (tolls).
- **Variants:** desert caravaneers; salt traders; steppe road merchants; river barge guild.
- **Evidence:** steppe nomads trading with and raiding settled states [NOMAD]; Paizo's traditionalists of the deep desert [PF-AG]; Bannerlord's desert faction and its minor factions [MB2-MF] [partially verified]; Anno 1404's Orient as a trading partner [ANNO] [partially verified].

### Group E. Arms

#### E1. Mercenary company

Soldiers for hire under a captain and a contract: a free company that serves whoever pays, and plunders when no one does.

- **Axes:** arms · supra-regional · tolerated · itinerant, a home base.
- **Goals:** pay, plunder, and for the best captains a lordship of their own.
- **Structure:** captain (condottiere), lieutenants, companies, camp followers; a written contract (condotta).
- **Base:** a home county of its culture; follows the wars. Rich cities hire it; poor borderlands breed it.
- **Resources:** pay in gold and silver; horses, iron, food from the hirer.
- **Relations:** allied with whoever pays (A1, A3, A6); rival to A5 and the countryside it loots, E2 (scorned by knights), other E1.
- **Variants:** free company; sworn warrior brotherhood (Bannerlord's mercenary minor factions modelled on the Jomsvikings) [MB2-SB] [partially verified]; cultural company (CK3) [CK3-M]; mercenary order with its own fort and temple (Dwarf Fortress) [DF-MERC] [partially verified]; the Zhentarim hiring out elite warriors [DND-5F].
- **Evidence:** condottieri and the free companies (the White Company, the Grand Company), the condotta, avoiding battle to protect their troops, some seizing power [COND]; CK3: one to three companies per culture, a headquarters county, hired for 3 years [CK3-M]; Skyrim's Companions, a guild of mercenaries in Whiterun [SKY-G] [partially verified]; Bannerlord minor factions [MB2-DB7] [partially verified]; Dwarf Fortress mercenary companies [DF-ENT] [partially verified].

#### E2. Secular knightly order

A brotherhood of knights under a prince or a code: a chivalric order, a famous sword school, a royal guard.

- **Axes:** arms · regional · sanctioned · urban (court) and rural (castles).
- **Goals:** honour, glory, the prince's favour, the code.
- **Structure:** a grand master (often the prince), knights by rank, squires; a chapter house.
- **Base:** the capital and the castles of its members.
- **Resources:** horses, iron (arms), its members' estates.
- **Relations:** allied with A1, A2, B1; rival to E1, F4, H6.
- **Variants:** royal chivalric order; sword school (Paizo's Swordlords) [PF-AG]; knights of a lady or a grail (Total War's Bretonnia [TWW-R] [partially verified]); witcher-style school of monster fighters (see E6).
- **Evidence:** secular orders founded by kings to bind their nobility (Charles I of Hungary, Alfonso XI of Castile) [MILORD]; Paizo's Aldori Swordlords and Eagle Knights [PF-AG]; Total War's knights of Bretonnia [TWW-R] [partially verified].

#### E3. Fighters' and adventurers' guild

A guild of armed professionals for hire for small jobs: guards, monster clearing, escort. The RPG's quest board.

- **Axes:** arms · supra-regional with a hall per town · sanctioned · town and urban.
- **Goals:** contracts, reputation, the guild's monopoly on sword work.
- **Structure:** guildmaster, hall masters, ranked members.
- **Base:** every larger town; more halls where Prospects are Poor and dangerous (frontier).
- **Resources:** fees; iron for arms.
- **Relations:** allied with A3, D1, E5; rival to E1 (undercutting), F2 (in Oblivion's plot [unverified]), D4 (a rival armed company).
- **Variants:** fighters' guild; adventurers' guild; mead-hall warrior band (Skyrim's Companions) [SKY-G] [partially verified]; explorers' society (G2).
- **Evidence:** Skyrim's Companions [SKY-G] [partially verified]; Oblivion's Fighters Guild [unverified]; Paizo's Pathfinder Society [PF-AG].

#### E4. Wardens against a great threat

An old, sworn order that guards the realm against one supernatural enemy: a horde from below, a breach in the world, the undead. Above politics by oath, mistrusted in peace.

- **Axes:** arms and cause · supra-regional · sanctioned (by old treaty) · frontier fortress.
- **Goals:** stop the threat, recruit, keep its ancient rights.
- **Structure:** a commander, wardens drawn from all peoples (and criminals, by right of conscription), a few fortresses.
- **Base:** where the threat is: high Supernatural Field, a region `border`, Mountains, Tundra, Ice & snow; a fortress on a pass.
- **Resources:** levies from realms; iron, horses, food bought in.
- **Relations:** allied with A4, B3, G1; rival to C3, B5, H6; strained with A1 (who resents its rights).
- **Variants:** wardens of the deep roads; watch on the wall; lantern bearers against a coming darkness [PF-AG].
- **Evidence:** Dragon Age's Grey Wardens [DA] [partially verified]; Paizo's Lantern Bearers [PF-AG]. Thin elsewhere: a strong archetype of story games, rare in strategy games.

#### E5. Town watch and militia

Citizens under arms and the town's police: the watch, the hermandad, the city militia.

- **Axes:** arms · local · ruling arm of A1 or A3 · urban and town.
- **Goals:** order, safe roads, the town's freedom.
- **Structure:** a captain, sergeants, watchmen; citizen levies by quarter or guild.
- **Base:** every town and city; rural brotherhoods along roads.
- **Resources:** town taxes.
- **Relations:** allied with A1, A3, D1, D2; rival to F2, F3, F4, F6.
- **Variants:** city watch; brotherhood of towns against bandits (hermandad); road wardens; enforcers (Frostpunk 2's guard squads) [FP2] [partially verified].
- **Evidence:** the Castilian hermandades, town leagues against bandits and lawless nobles, later a royal police [HERM]; communes' sworn defence [COMMUNE].

#### E6. Monster hunters

Professionals who kill monsters for pay: mutated, trained or blessed, few, feared and needed.

- **Axes:** arms · supra-regional, a school or keep · tolerated · itinerant, a remote keep.
- **Goals:** contracts, the survival of the school, neutrality.
- **Structure:** a school in a keep, masters, hunters on the road.
- **Base:** a remote keep in Mountains or Hills; hunters go where the Supernatural Field is high and Prospects are Poor.
- **Resources:** fees; silver (for weapons in many settings).
- **Relations:** allied with A5 (the villages that hire them); rival to B4 (who call them abominations), C3, H4.
- **Variants:** monster-hunter school; bounty hunters; Mount & Blade-style beast hunters [unverified].
- **Evidence:** the Witcher schools (Wolf, Cat, Griffin, Bear, Viper and others; game lore, not canon in the books) [WIT-S] [partially verified]. Thin elsewhere as a faction; common as a class.

### Group F. Crime

#### F1. Thieves' guild

The organised underworld of a city: burglars, pickpockets, fences, beggars, under one master, paying off the watch.

- **Axes:** crime · local, sometimes a network · outlawed (tolerated in practice) · urban.
- **Goals:** loot, control of the city's crime, influence over officials.
- **Structure:** a master, lieutenants, fences, specialists, beggars and street children as eyes; a hidden hall (sewers, an inn).
- **Base:** large city Prospects (Rich, harbour cities most); the poorest quarter, sewers, the docks.
- **Resources:** stolen goods; luxuries (gems, spices, furs) fenced through D1's markets.
- **Relations:** allied with F6 (smugglers), F3 (sometimes), E5 (bribed); rival to E5, D1, A3, B4.
- **Variants:** thieves' guild; council of thieves ruling a city [PF-AG]; beggars' guild; dwarven crime syndicate (Dragon Age's Carta [unverified]).
- **Evidence:** Skyrim's Thieves Guild, a secret guild of professional thieves under Riften [SKY-G] [partially verified]; Oblivion's Thieves Guild [unverified]; Paizo's Council of Thieves [PF-AG]; Blades in the Dark's underworld factions [BITD-FG].

#### F2. Assassins' brotherhood

Killers for hire with a creed, a hidden sanctuary and a reputation that does half the work.

- **Axes:** crime (or faith) · supra-regional · outlawed (legal in a few settings) · urban cells, remote sanctuary.
- **Goals:** contracts, fear, the creed, sometimes a political cause.
- **Structure:** a hidden master, a council, sanctuaries, killers, contacts; often a cult core.
- **Base:** a sanctuary away from Prospects (Mountains, Hills, forest) and cells in cities.
- **Resources:** fees; poisons (part of spices or wild food).
- **Relations:** allied with B5, A2 (as clients); rival to E5, B4, G1, F1 (sometimes).
- **Variants:** brotherhood of a death god; legal assassins' guild (Morrowind's Morag Tong, used by the Great Houses in their wars) [MW-G] [partially verified]; assassin cult (Paizo's Red Mantis) [PF-AON] [partially verified]; witcher school turned assassins [WIT-S] [partially verified]; mountain-fortress sect.
- **Evidence:** the Nizari Ismailis: Alamut and a network of mountain fortresses, targeted killing as policy against rulers [NIZARI]; Skyrim's Dark Brotherhood, a secret guild of assassins with a sanctuary west of Falkreath [SKY-G] [partially verified]; Dragon Age's Antivan Crows [DA] [partially verified].

#### F3. Black network

A semi-legal empire of smugglers, mercenaries, fixers and protection rackets that spans realms and buys rulers.

- **Axes:** crime and trade · supra-regional · tolerated where it pays, outlawed elsewhere · urban and itinerant.
- **Goals:** wealth as power, control of trade routes, influence through debt and fear.
- **Structure:** a ruling council, regional bosses, hired swords, front businesses; rewards ambition and results.
- **Base:** trade cities on the travel network; border towns between realms.
- **Resources:** contraband, protection money, mercenary contracts.
- **Relations:** allied with F1, F6, E1, corrupt A2; rival to G1, A4, E5, D1.
- **Variants:** smuggling syndicate; organised-crime family; slaver network (opposed by Paizo's Bellflower Network) [PF-AG].
- **Evidence:** D&D's Zhentarim, the "Black Network" of mercenaries, rogues and warlocks who see wealth as power and hire out warriors [DND-5F]; Paizo's Aspis Consortium [PF-AG]; Morrowind's Camonna Tong [MW-G] [partially verified].

#### F4. Bandit clan

Outlaws on the roads and in the woods: a band of robbers under a chief, sometimes deserters, sometimes dispossessed peasants, sometimes a romantic brotherhood.

- **Axes:** crime · local · outlawed · wilderness near roads.
- **Goals:** loot, survival, sometimes revenge on a lord.
- **Structure:** a chief, a lieutenant, a dozen to a hundred men; a camp or a fort.
- **Base:** wild patches next to the travel network: forest, Hills and Mountains, swamp, steppe; far from E5 and A1's garrisons (high `depth`, Poor Prospects nearby). Bannerlord types its bandits by terrain: looters, steppe, forest, mountain bandits, sea raiders, deserters [MB2-MF] [partially verified].
- **Resources:** what they steal; wild food & game; timber.
- **Relations:** allied with F3 (fences), A6 (in a revolt); rival to E5, A2, D8, A5, B8.
- **Variants:** forest outlaws; mountain brigands; steppe raiders; deserters; sworn forest brotherhood of young warriors (Bannerlord's Wolfskins, modelled on the Fianna) [MB2-BAT] [partially verified]; raider gangs (RimWorld's pirates, locked hostile) [RW-F].
- **Evidence:** Dwarf Fortress bandit camps and forts [DF-SITE] [partially verified]; Bannerlord bandit types [MB2-MF] [partially verified]; Manor Lords bandit camps [ML] [partially verified]; RimWorld raider factions [RW-F]; the hermandades formed against them [HERM].

#### F5. Pirates

Sea raiders and privateers with island bases, preying on merchant shipping and coastal towns, or selling to anyone.

- **Axes:** crime · supra-regional (a sea) · outlawed, or licensed as privateers · maritime.
- **Goals:** plunder, ransom, a free port of their own.
- **Structure:** captains, each with a crew; an elected or strongest admiral; a free port.
- **Base:** islands and coves: a small enclosed sea, a `harbour` patch with high `depth` from the capital, Mangrove coasts, rocky Hills coasts; sea lanes nearby.
- **Resources:** plunder; fish, timber for ships, salt.
- **Relations:** allied with F3, F6, sometimes A1 (as privateers); rival to D3, D4, A3.
- **Variants:** privateers turned pirates; pirate federation that trades with all [POE-29] [partially verified for the Príncipi]; northern sea raiders [MB2-MF] [partially verified]; undead pirates (Total War's Vampire Coast) [TWW-R] [partially verified].
- **Evidence:** the Victual Brothers: privateers hired in 1392, pirates who took Visby on Gotland, enemies of Hanseatic Lübeck [VICTUAL]; Deadfire's pirate federation that frequently targets the trading company [POE-28] [partially verified]; RimWorld pirates [RW-F].

#### F6. Smugglers' ring

Small-scale runners of untaxed or banned goods across borders, coasts and mountain passes.

- **Axes:** crime and trade · local · outlawed (tolerated by locals) · maritime and border.
- **Goals:** profit from tolls and bans, keep the local officials sweet.
- **Structure:** a ring leader, boatmen or mule drivers, safe houses, an inn as a front.
- **Base:** coves (coast with no Prospect), mountain passes on a region `border`, river crossings at borders.
- **Resources:** salt (a classic smuggled good), spices, stimulants, dyes, banned magic goods.
- **Relations:** allied with F1, F3, F5, A5 (locals); rival to E5, D1, A1 (customs).
- **Variants:** salt smugglers; border runners; river smugglers.
- **Evidence:** Morrowind's Camonna Tong and smuggler caves [MW-G] [partially verified]; the Zhentarim [DND-5F]. Mostly a subtype of F3 in games; separate here because it is local and rural.

### Group G. Cause

#### G1. Benevolent secret society

A hidden network of spies and agents who fight tyranny and dark powers behind the scenes.

- **Axes:** cause · supra-regional · hidden (outlawed by tyrants) · urban and itinerant.
- **Goals:** topple tyrants, protect the weak, keep dangerous artefacts from evil hands, balance of power.
- **Structure:** cells, a few senior agents, safe houses, disguises; fierce loyalty, little hierarchy.
- **Base:** cities and courts; no fixed seat.
- **Resources:** members' wealth; information.
- **Relations:** allied with B7, E4, C2 (sometimes), A5; rival to F3, B5, C3, tyrant A1.
- **Variants:** spy network; balance-keepers; a ruler's own spymaster's network (CK3 council [unverified]).
- **Evidence:** D&D's Harpers, a secret network of spellcasters and spies who topple tyrants and help the oppressed [DND-5F]; Blades in the Dark's institutions and fringe factions [BITD-FG] [partially verified].

#### G2. Scholars' and explorers' society

A learned society that collects lore, explores ruins and sells or shelters what it finds.

- **Axes:** cause and arcane · supra-regional · sanctioned · urban lodges, expeditions.
- **Goals:** knowledge, relics, fame.
- **Structure:** a council of elders, lodges, venture captains, field agents.
- **Base:** lodges in cities; expeditions to ruins in high-Supernatural wilderness.
- **Resources:** patrons; relics.
- **Relations:** allied with C1, B2; rival to D7-type profiteers (Paizo pairs the explorers with a rival syndicate) [PF-AON] [partially verified], F1, B5.
- **Variants:** explorers' lodge; archaeologists; cartographers.
- **Evidence:** Paizo's Pathfinder Society, "adventuring scholars of historical lore" [PF-AG], and its perennial enemy the Aspis Consortium [PF-AON] [partially verified]. Thin in computer games.

#### G3. Healers' order

A hospital order or a band of physicians who care for the sick and the poor, neutral in wars.

- **Axes:** faith or cause · supra-regional · sanctioned · town and urban hospitals, pilgrim roads.
- **Goals:** heal, teach medicine, stay neutral.
- **Structure:** a master, physicians, nurses, hospitals and hospices.
- **Base:** hospitals in cities; hospices on pilgrim roads and passes.
- **Resources:** donations; herbs (wild food), salt.
- **Relations:** allied with B1, B8, A5; rival to C3; tolerated by all.
- **Variants:** hospitaller order; healing sisters; physicians' college; Fallout's healer-scholars [FNV] [partially verified].
- **Evidence:** the Hospitallers, recognised in 1113 [MILORD]; Endless Legend's Sisters of Mercy, a healing minor faction [EL-MF]; Fallout: New Vegas Followers of the Apocalypse [FNV] [partially verified].

#### G4. Resistance of a dispossessed people

A guerrilla movement of a people pushed off its land: forest commandos, partisans, freedom fighters. Heroes to their own, terrorists to the crown.

- **Axes:** kin and cause · regional · outlawed · wilderness near settled land.
- **Goals:** their land back, revenge, survival of their people.
- **Structure:** commandos under a leader, sympathisers in towns, a refuge deep in the wild.
- **Base:** forest and hills on the edge of the settled land, near the Prospects that took it.
- **Resources:** wild food & game; what they take.
- **Relations:** allied with H5 or H2 (their kin), G1; rival to A1, D6, B4, E5.
- **Variants:** elven commandos; tribal resistance to a company; slave liberators [PF-AG].
- **Evidence:** the Witcher's Scoia'tael, a non-human guerrilla movement [WIT] [partially verified]; Paizo's Silver Ravens (rebels) and Bellflower Network (against halfling slavery) [PF-AG]; Deadfire's Huana against the companies [POE-29].

### Group H. Kin and peoples of the wild

#### H1. Nomad horde

Horse or camel pastoralists of the steppe and desert, moving with their herds under a khan; trading, raiding, and now and then uniting into an empire.

- **Axes:** kin · regional to supra-regional · independent · itinerant, steppe and desert.
- **Goals:** pasture, tribute or trade for grain, cloth and iron; glory for the khan.
- **Structure:** khan, clan chiefs, warriors; a confederation of tribes.
- **Base:** open dry land: Cold steppe, Temperate grassland, Savanna, Cold desert, Hot desert; Flat Land; few Prospects, low prosperity of farmland.
- **Resources:** horses & pack animals, livestock, furs; wants grain, textiles (fibre), iron.
- **Relations:** allied with D8 (who pay for passage); rival to A1, A2 and A5 on the farm frontier; other H1 clans.
- **Variants:** steppe khanate; desert tribes; reindeer herders of the Tundra; mounted barbarian tribes riding beasts (Paizo's Mammoth Lords) [PF-AG].
- **Evidence:** Eurasian nomads: horses, no permanent abode, trade and raids with settled states, demands for grain, textiles and ironware, confederations like the Xiongnu and the Göktürks [NOMAD]; CK3's nomad popular faction [CK3-F]; Bannerlord's steppe khanate and its minor faction [MB2-MF] [partially verified].

#### H2. Highland and forest tribe

A people of clans in the hills, forests or islands, outside the kingdoms: proud, warlike, holding to old ways and old gods.

- **Axes:** kin · regional · independent · wilderness and rural.
- **Goals:** independence, their land and customs, honour and feud.
- **Structure:** a chief or council of elders, clans, warriors, a shaman or druid.
- **Base:** Hills and Mountains, Temperate rain forest, Boreal forest, islands; far from the capital (high `depth`), Poor Prospects.
- **Resources:** livestock, wild food & game, timber, furs.
- **Relations:** allied with B7, H5, G4; rival to A1, D4, D6, other H2 clans.
- **Variants:** highland clans; island traditionalists (Deadfire's Huana) [POE-29]; ash-land nomads who call the settled people usurpers (Morrowind's Ashlanders) [MW-G] [partially verified]; RimWorld's tribes, from "gentle" (neutral) to "savage" (locked hostile) [RW-F]; desert traditionalists [PF-AG].
- **Evidence:** RimWorld tribes [RW-F]; Deadfire's Huana [POE-29]; Bannerlord's Battania and its forest bands [MB2-BAT] [partially verified]; Heroes III Stronghold on rough terrain [H3-T] [partially verified]; Age of Wonders 4 Primal culture [AOW4-PF] [partially verified].

#### H3. Dwarven hold

An underground kin-realm under the mountains: master miners, smiths and builders who trade metal for food.

- **Axes:** kin and craft · regional · independent or allied · Mountains and Hills, underground.
- **Goals:** guard the hold, mine deeper, keep old grudges and old treaties.
- **Structure:** a king of the hold, clan lords, guilds of miners and smiths, runesmiths.
- **Base:** Mountains and Hills zones with metal-rich bedrock (fold belt, granite, volcanic arc, ophiolite); gates on valleys with a river.
- **Resources:** iron ore, copper ore, tin, silver & lead, gold, gems, stone; buys grain, livestock, timber.
- **Relations:** allied with D5, D2, A1 (trade); rival to H6 (goblins), H5 (in many settings), C3; wary of B7 (which dislikes mining).
- **Variants:** mountain hold; hill hillocks; exiled clans in human cities; dwarven crime syndicate [unverified].
- **Evidence:** Dwarf Fortress dwarven hillocks and fortresses [DF-SITE] [partially verified]; Age of Wonders 4 dwarves: Industrious culture, underground adaptation, quarries, runesmiths [AOW4-DD5]; Total War's Dwarfs [TWW-R] [partially verified].

#### H4. Beastfolk tribe

A tribe of animal-headed or half-animal people: centaurs, minotaurs, beastmen, ratfolk, lizardfolk. Usually a minor faction to pacify or fight.

- **Axes:** kin · local · independent · wilderness.
- **Goals:** hunting grounds, survival, raiding (or, for the friendly kind, being left alone).
- **Structure:** a chief or shaman, war bands; a herd or nest.
- **Base:** one biome each: centaurs on grassland and Savanna, beastmen in deep forest, lizardfolk in swamp and Mangrove and Tropical rain forest, ratfolk under cities, ice beasts in Tundra; high Supernatural Field.
- **Resources:** wild food & game; horses (centaurs).
- **Relations:** allied with B7 (some), H2; rival to D6, A5, E6.
- **Variants:** centaur herd, minotaur clan, beastman warherd (a horde that occupies no settlements in Total War) [TWW-C] [partially verified], ratfolk underworld [TWW-R] [partially verified], lizardfolk tribe [TWW-R] [partially verified].
- **Evidence:** Endless Legend minor factions: centaurs, minotaurs, ice wargs, driders, each holding one region's villages, pacified by quest, bribe or force, then assimilated [EL-MF]; Total War's Beastmen, Skaven and Lizardmen [TWW-R] [partially verified]; Heroes III Fortress town (swamp) [H3-T] [partially verified].

#### H5. Elven enclave

A hidden, long-lived people in an old forest: aloof, magical, hostile to woodcutters.

- **Axes:** kin and arcane · regional · independent · wilderness (deep forest).
- **Goals:** guard the forest, keep apart, preserve old lore.
- **Structure:** a lord or council of elders, wardens and rangers, mages.
- **Base:** Temperate broadleaf forest, Temperate rain forest, Tropical rain forest; high Supernatural Field; no Prospect for a city in the enclave (high `depth`).
- **Resources:** timber (fine woods), wild food, the Elanor flower and other fantasy goods the readme lists [readme, "Generated resources"].
- **Relations:** allied with B7, H4 (some); rival to D6, H3, H6, A1 (when it expands).
- **Variants:** forest realm; wandering elven clans (Dragon Age's Dalish [DA] [partially verified]); elven forest retreats (Dwarf Fortress) [DF-SITE] [partially verified]; high elves of a coast (Total War) [TWW-R] [partially verified].
- **Evidence:** Dwarf Fortress forest retreats, elves living in the canopy [DF-SITE] [partially verified]; Total War's Wood Elves [TWW-R] [partially verified]; Heroes III Rampart (grass) [H3-T] [partially verified].

#### H6. Goblin warband

Goblins, orcs or ogres in a dark fortress, raiding everyone around: the classic "evil race" faction.

- **Axes:** kin · regional · independent · wilderness strongholds.
- **Goals:** loot, captives, a stronger warlord.
- **Structure:** warlord, chieftains, war bands; slaves.
- **Base:** dark fortresses and pits in Mountains, Hills, volcanic arc bedrock, Cold desert, Tundra, rough ground; high Supernatural Field; near region `border`s.
- **Resources:** iron (looted or mined), captives; little farming.
- **Relations:** allied with B5, C3 (sometimes); rival to H3, H5, A1, E4, A4.
- **Variants:** goblin dark fortress; orc horde; ogre kingdom; Endless Legend's orc minor faction [EL-MF].
- **Evidence:** Dwarf Fortress goblin dark fortresses and dark pits [DF-SITE] [partially verified]; Total War's Greenskins [TWW-R] [partially verified]; Heroes III Stronghold and Dungeon [H3-T] [partially verified].

---

## 4. Relations

### 4.1 Recurring rivalries (with evidence)

| Pair | Why they fight | Evidence |
|---|---|---|
| Thieves' guild F1 × town watch E5 | crime against order | Skyrim, Oblivion [SKY-G] [partially verified]; Blades in the Dark [BITD-FG] |
| Mages' college C1 × inquisition B4 | magic against faith | Dragon Age Circle and Templars [DA] [partially verified]; Witcher witch hunters [WIT] [partially verified] |
| Holy order B3 × forbidden cult B5 or necromancers C3 | faith against darkness | Order of the Gauntlet [DND-5F]; Hellknights [PF-AG] |
| Merchant league D3 or company D4 × pirates F5 | trade against plunder | Hansa and Victual Brothers [VICTUAL]; VTC and Príncipi [POE-29] |
| Two chartered companies D4 × D4 | same land, same goods | RDC and VTC, espionage and sabotage [POE-29] |
| Crown A1 × noble houses A2 (via A6) | central against local power | CK3 factions [CK3-F] |
| Merchant guild D1 × craft guilds D2 | control of the town council | [GUILD-B] |
| Settlers D6 × forest folk H5, H2, B7 | farmland against forest | Scoia'tael [WIT] [partially verified]; Huana and companies [POE-29] |
| Explorers G2 × profiteers D7, F3 | knowledge against profit | Pathfinder Society and Aspis Consortium [PF-AON] [partially verified] |
| Law order B3, E5 × liberators G4 | order against freedom | Hellknights and Bellflower Network [PF-AON] [partially verified] |
| Dwarves H3 × goblins H6 | ancient war for the deep | Total War [TWW-R] [partially verified]; Dwarf Fortress [unverified] |
| Nomads H1 × farmers A1, A5 | pasture against field | [NOMAD]; CK3 nomads [CK3-F] |
| Benevolent society G1 × black network F3 | the two hidden networks | Harpers and Zhentarim [DND-5F] |
| Great houses A2 × A2 | rivals for the same throne or trade | Morrowind houses [MW-G] [partially verified] |

### 4.2 Recurring alliances

| Pair | Why they stand together | Evidence |
|---|---|---|
| Crown A1 + church B1 | legitimacy for protection | the secular arm [INQ]; CK3 holy orders and patrons [CK3-HO] |
| Church B1 + holy order B3 + inquisition B4 | one institution, three arms | [MILORD] [INQ] |
| Free city A3 + guilds D1, D2 + watch E5 | the guilds are the council | [GUILD-B] [COMMUNE] |
| League D3 + military order B3 | protection of the trade | the Teutonic Grand Master as a head of the Hansa [HANSA] |
| Druids B7 + elves H5 + beastfolk H4 | the forest coalition | Emerald Enclave [DND-5F]; Endless Legend [EL-MF] |
| Thieves F1 + smugglers F6 + black network F3 | the underworld supply chain | Zhentarim [DND-5F] |
| Dwarves H3 + miners D5 + smiths D2 | the metal chain | Age of Wonders 4 dwarves [AOW4-DD5] |
| Caravans D8 + nomads H1 | passage for payment | [NOMAD] |
| Assassins F2 + noble houses A2 | the house wars | Morag Tong [MW-G] [partially verified] |

### 4.3 Matrix by group

`+` usually allied, `−` usually hostile, `±` it depends on the instance, blank neutral or unrelated. Read along the row.

| | A rule | B1 church | B3–B4 order, inquis. | B5 cult | B7 druids | C1 college | C3 necro | D trade | D2 craft | E1 merc | E5 watch | F crime | G1 secret soc. | H1 nomads | H2–H4 tribes | H5 elves | H6 goblins |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| **A rule** | ± | + | + | − | ± | + | − | + | + | ± | + | − | ± | − | ± | ± | − |
| **B1 church** | + | | + | − | ± | ± | − | + | + | | + | − | | − | ± | ± | − |
| **B3–B4** | + | + | | − | ± | − | − | | | − | + | − | ± | − | | ± | − |
| **B5 cult** | − | − | − | | − | − | + | | | ± | − | ± | − | | ± | − | + |
| **B7 druids** | ± | ± | ± | − | | | − | − | | | | | + | | + | + | − |
| **C1 college** | + | ± | − | − | | | − | + | + | | | − | ± | | | ± | − |
| **C3 necro** | − | − | − | + | − | − | | | | | − | | − | | | − | ± |
| **D trade** | + | + | | | − | + | | ± | ± | + | + | − | | ± | − | − | − |
| **D2 craft** | + | + | | | | + | | ± | ± | | + | − | | | | | |
| **E1 merc** | ± | | − | ± | | | | + | | − | | ± | | ± | | | |
| **E5 watch** | + | + | + | − | | | − | + | + | | | − | | | | | |
| **F crime** | − | − | − | ± | | − | | − | − | ± | − | ± | − | | | | |
| **G1 secret soc.** | ± | | ± | − | + | ± | − | | | | | − | | | | + | − |
| **H1 nomads** | − | − | − | | | | | ± | | ± | | | | ± | | | |
| **H2–H4 tribes** | ± | ± | | ± | + | | | − | | | | | | | ± | + | − |
| **H5 elves** | ± | ± | ± | − | + | ± | − | − | | | | | + | | + | | − |
| **H6 goblins** | − | − | − | + | − | − | ± | − | | | | | − | | − | − | ± |

---

## 5. Design note: factions in the generator

*Everything in this section is my own engineering judgement, not a sourced claim.*

### 5.1 What a faction needs as a Stub

The readme says a Stub "holds every gameplay-relevant fact" and is derived data. For a faction that means:

| Attribute | Type | Source in the generator |
|---|---|---|
| Identity | region + slot index | readme, "Identity" |
| Archetype | one of §3 (A1 … H6) | drawn per slot |
| Variant | one of the archetype's variants | drawn per faction |
| Basis, scale, legality, affinity | the archetype's values (§2) | a table, not drawn |
| Seat | a Prospect or a patch | §5.3 |
| Domain | a set of patches or Prospects it controls or operates in | grows from the seat over the patch graph (`map-generation.md` §15.2) |
| Strength | tier 0–5 and hold weak/strong, after Blades in the Dark [BITD-FG] | from the seat's Prospect score and the domain's resources |
| Resources it lives on | a subset of the 24 resources | the archetype's list ∩ what the domain has |
| Stance to the ruler | ruling / sanctioned / tolerated / outlawed / independent | archetype default, then drawn |
| Relations | status −3 … +3 to every other faction of the region and its neighbours | §4.3 default plus a hashed offset |
| Ranks | the archetype's rank ladder | a table; the Persons of the Settled stage fill it |
| Name | generated | a faction-name generator, like the person-name generator |
| Links abroad | the faction it is a chapter of, if any | §5.5 |

The Realised form adds only the authored text: history, creed, heraldry, description.

### 5.2 Which factions a settlement carries

The games put more and more kinds of faction in larger places. A table to start from (counts are per Prospect on show, probabilities are starting calibrations):

| Settlement | Always | Often (p ≈ 0.5–0.8) | Sometimes (p ≈ 0.1–0.3) |
|---|---|---|---|
| **Capital** (best city Prospect) | A1, B1 seat, D1, 2–4 × D2, E5 | F1, C1, D7, E2, B4, D3 kontor | F2 cell, C2, G1, G2, B5 cell |
| **Other city** (Prospect for a city) | B1 cathedral, D1, 1–3 × D2, E5 | A2 or A3, F1, E3 | C1 chapter, F3, B3 chapter, D3 kontor |
| **Town** (Rich or Average) | B1 temple, E5 | A2, D2 (one craft by resource) | E3, F6, G3, D5 (if mining) |
| **Town** (Poor) | B1 shrine | A5 | F4 nearby, E6 visit |
| **No Prospect** (wilderness) | | | B2, B7, B5 shrine, C3, F4, F5, H1–H6, E4, B8 |

Which craft guilds (D2) a town has follows its catchment: smiths with iron ore and fuel, masons with stone, weavers with fibre or livestock, tanners with livestock or game. A mining town (D5) needs a metal deposit in reach.

### 5.3 Where wilderness factions sit

Score every land patch of the region for each wilderness archetype, as the Prospects are scored (`map-generation.md` §15.3), but with the archetype's own weights:

| Term | Meaning |
|---|---|
| **Biome fit** | a weight per biome, from the Base lines of §3 (e.g. H5: 1 in broadleaf and rain forest, 0 in desert) |
| **Elevation fit** | a weight per elevation type (H3: Mountains 1, Hills 0.6, Lowland 0) |
| **Bedrock fit** | for D5, H3: the metal-bearing bedrocks |
| **Remoteness** | `depth` and the distance to the nearest Prospect on show; most wilderness factions want it high, bandits want it medium (near roads but not near garrisons) |
| **Supernatural** | the Field at the patch, for B5, C3, B7, H4–H6, E4, E6, B8 |
| **Water and coast** | `harbour` and coast for F5, `river` for B2 |

Take the best patches with a spacing, as in `map-generation.md` §15.5. This is what Heroes III does with native terrain [H3-T] and Total War with climate suitability [TWW-C], and it reuses facts the Planned stage already has.

### 5.4 How many, and which mix

- **One realm-level ruler per region** fits the readme's statement that a region is the size of a medieval kingdom. Some regions can instead hold a league (A4), a set of free cities (A3) or a horde (H1) with no crown, which gives variety.
- **Region character from the Fields and the land:** a region with high Prosperity and many Rich Prospects gets more trade and craft factions and more crime; a region with high Supernatural gets more arcane and faith factions, cults and monsters; a region with much steppe gets nomads; a region of mountains with metal bedrock gets dwarves and miners. This is the RimWorld idea of faction types with fixed goodwill ranges [RW-F], driven by the map instead of a menu.
- **Two of a kind make a story.** Rivalries need a counterpart in reach: if a region gets a thieves' guild it should also have a watch; if it gets a cult, a holy order or an inquisition should be in it or a neighbour. A pass that checks the §4.1 pairs after the draw can add the missing side.
- **Not monolithic.** Josh Sawyer credits the depth of New Vegas's big factions to members who disagree with one another [SAWYER] [partially verified]. The Settled stage can give each faction's important persons a hashed stance (loyalist, reformer, schemer) so that a faction is not one voice.

### 5.5 Open questions

1. **Factions that span regions.** The readme defines a faction as "a group of people in one region", but half the archetypes (B1, B3, C1, D3, D4, E1, F3, G1, H1) are networks across realms in every source. Options: (a) keep the readme's definition and model a network as one faction per region with a shared "network" key that is a pure function of the foundation (e.g. hashed from a coarse super-grid of regions), so each regional chapter is computed locally; (b) add a separate supra-regional entity. Option (a) keeps generation order-independent. This touches the domain model, so it is for the user to decide before any code.
2. **Region stage.** Factions are listed with the Prospects in the Planned stage, which needs only the region Elevated. Relations to neighbours' factions and the "missing counterpart" pass of §5.4 would need the bordering regions Planned, which is the Settled precondition. Either faction relations across borders move to Settled, or Planned factions only relate within their region.
3. **Villages.** The repo has Prospects for cities and towns only. Rural factions (A2 manors, A5 communes, B2 monasteries) need either villages or patch seats.
4. **Peoples and races.** H3–H6 assume non-human peoples. Whether the game has them, and whether they are factions or a separate "people" layer under factions (as Total War and Age of Wonders 4 treat race as separate from faction), is not decided anywhere in the repo.
5. **Fantasy resources.** C1, C3, B5 and H5 want fantasy goods that the generation has no rule for yet (readme: star metal, Elanor sap). Their placement can lean on the Supernatural Field until those exist.
6. **Strength and Prospect class.** Should a Rich Prospect always seat a strong faction, or should strength be independent so a rich town can have a weak guild? The games do both.
7. **Purpose numbers.** Every random choice must go through `Hashing` with a purpose number that `map-generation.md` gives. A faction section there would need to assign them; this note does not.

### 5.6 Gaps in this research

- **Fan wikis refused the fetch tool** (UESP, Fandom, Dwarf Fortress wiki, TaleWorlds). Elder Scrolls, Dwarf Fortress, Witcher, Dragon Age, Fallout and Bannerlord claims rest on search excerpts and are marked [partially verified]. Oblivion, Stronghold and Banished claims are [unverified].
- **City builders have few factions.** Banished, Stronghold and Anno use rival lords, bandits and NPC traders rather than factions with goals; only Frostpunk 2 and Dwarf Fortress model factions in depth. The archetype list leans on RPGs, strategy games and history.
- **Shrine keepers (B8), frontier settlers (D6), bankers (D7), monster hunters (E6), wardens (E4) and explorers (G2)** each rest on fewer than three independent game sources and are kept for their value to a generated world.
- **The Blades in the Dark category names** and the **Worlds Without Number faction-turn rules** were not read from the primary text.

---

## 6. Sources

### 6.1 Tabletop and design (first-party or licensed)

- **DND-5F** — Wizards of the Coast, *D&D Basic Rules (2014)*, "Appendix C: The Five Factions" (Harpers, Order of the Gauntlet, Emerald Enclave, Lords' Alliance, Zhentarim), D&D Beyond. https://www.dndbeyond.com/sources/dnd/basic-rules-2014/appendix-c-the-five-factions
- **PF-AG** — Paizo, "Organizations of the Adventurer's Guide", Paizo blog (19 organisations with one-line purposes). https://paizo.com/community/blog/v5748dyo5ljs2
- **PF-AON** — Archives of Nethys (Paizo-licensed), faction rivalries (Aspis Consortium × Pathfinder Society, Hellknights × Bellflower Network) and the Red Mantis. https://aonprd.com/Rules.aspx?ID=2369, https://aonprd.com/Rules.aspx?ID=2366 **[partially verified: search excerpts]**
- **BITD-FG** — *Blades in the Dark*, "The Faction Game", Roll20 compendium (licensed): tier, hold, status −3 to +3. https://roll20.net/compendium/BITD/The%20Faction%20Game
- **WWN** — Kevin Crawford, *Worlds Without Number* free edition, faction rules (reported by review blogs). https://www.tenkarstavern.com/2022/02/free-rpg-worlds-without-number-free.html **[partially verified: not read]**
- **SAWYER** — PC Gamer, "Fallout: New Vegas director Josh Sawyer says the NCR and Caesar's Legion feel so real because their members disagree…". https://pcgamer.com/games/fallout/fallout-new-vegas-director-josh-sawyer-says-the-ncr-and-caesars-legion-feel-so-real-because-their-members-disagree-and-challenge-each-other-you-have-to-wrestle-with-the-fact-that-it-contains-multitudes/ **[partially verified: search excerpt]**

### 6.2 Games: developer material

- **POE-29** — Obsidian, "Pillars of Eternity II: Deadfire Update #29 – Factions of the Deadfire Part II" (RDC, VTC). https://eternity.obsidian.net/eternity/news/pillars-of-eternity-ii-deadfire-update-29---factions-of-the-deadfire-part-ii
- **POE-28** — Obsidian, "Update #28 – Factions of the Deadfire Part I" (Príncipi, Huana). https://eternity.obsidian.net/eternity/news/pillars-of-eternity-ii-deadfire-update-28---factions-of-the-deadfire-part-i **[partially verified: search excerpt]**
- **AOW4-DD5** — Paradox Interactive / Triumph, *Age of Wonders 4* Dev Diary #5 (dwarves, Industrious culture, Underground Adaptation, Great Builders, Runesmiths). https://www.paradoxinteractive.com/games/age-of-wonders-4/news/age-of-wonders-4-dev-diary-5
- **AOW4-PF** — *Age of Wonders 4*, Primal Fury Dev Diary #1 (Primal cultures, totem animals, terrain bonuses). https://www.paradoxinteractive.com/games/age-of-wonders-4/news/primal-fury-dev-diary-1 **[partially verified: search excerpt]**
- **AOW4-DD24** — *Age of Wonders 4* Dev Diary #24 (Dark culture). https://admin-forum.paradoxplaza.com/forum/developer-diary/dev-diary-24-seals-tomes.1604169/page-2 **[partially verified: search excerpt]**
- **MB2-DB7** — TaleWorlds, *Bannerlord* Developer Blog 7, "Imperial Declines" (minor factions as outlaws and mercenaries modelled on historical or legendary bands). https://www.taleworlds.com/en/Games/Bannerlord/Blog/8 **[partially verified: search excerpt; taleworlds.com refused the fetch]**
- **MB2-BAT**, **MB2-SB** — TaleWorlds developer blogs "Battania" and "Sturgia" (Wolfskins after the Fianna; a mercenary brotherhood after the Jomsvikings), GameBanshee reposts. https://gamebanshee.com/k4k4p, https://gamebanshee.com/k4x34 **[partially verified: search excerpts]**
- **MB2-V** — TaleWorlds blogs on village production types, as cited in `game-resources-and-trade.md` §7.1. **[partially verified]**

### 6.3 Games: community wikis and press

- **CK3-F** — Crusader Kings III Wiki, "Factions". https://ck3.paradoxwikis.com/Factions
- **CK3-M** — Crusader Kings III Wiki, "Mercenaries" (version 1.20). https://ck3.paradoxwikis.com/Mercenaries
- **CK3-HO** — Crusader Kings III Wiki, "Holy order" (redirects to "Hired forces", military orders). https://ck3.paradoxwikis.com/Holy_order
- **RW-F** — RimWorld Wiki, "Factions". https://rimworldwiki.com/wiki/Factions
- **EL-MF** — Endless Legend Wiki (wiki.gg), "Minor Factions". https://endlesslegend.wiki.gg/wiki/Minor_Factions
- **EL-C** — Endless Legend Wiki, "Cultists". https://endlesslegend.wiki.gg/wiki/Cultists **[partially verified: search excerpt]**
- **SKY-G** — UESP, "Skyrim:Guilds" and "Skyrim:Guild Halls". https://en.uesp.net/wiki/Skyrim:Guilds, https://en.uesp.net/wiki/Skyrim:Guild_Halls **[partially verified: search excerpts; UESP refused the fetch]**
- **MW-G** — UESP, "Morrowind:Guilds", "Morrowind:Factions". https://www.uesp.net/wiki/Morrowind:Guilds, https://en.m.uesp.net/wiki/Morrowind:Factions **[partially verified: search excerpts]**
- **DF-SITE** — Dwarf Fortress Wiki, "Site", "Dark fortress", "Forest retreat", "Monastery". https://dwarffortresswiki.org/index.php/Site, https://dwarffortresswiki.org/index.php/Dark_fortress, https://dwarffortresswiki.org/Forest%20retreat, https://dwarffortresswiki.org/Monastery **[partially verified: search excerpts; the wiki returned 429]**
- **DF-ENT** — Dwarf Fortress Wiki, "Variable positions" and release notes (merchant companies, military orders, mercenary companies, craft guilds in world generation since 0.47.01). https://dwarffortresswiki.org/index.php/Variable_positions **[partially verified]**
- **DF-MERC** — Dwarf Fortress Wiki, "Mercenary". https://www.dwarffortresswiki.org/index.php/Mercenary **[partially verified]**
- **DF-REL** — Dwarf Fortress Wiki, "Religion". https://dwarffortresswiki.org/index.php/Religion **[partially verified]**
- **H3-T** — Heroes of Might and Magic Community Wiki (shoutwiki), town pages and "Random Map" (native terrain per town). https://heroes.shoutwiki.com/wiki/Random_Map, https://heroes.shoutwiki.com/wiki/Castle **[partially verified: search excerpts]**
- **TWW-C** — Total War: Warhammer Wiki (Fandom), climate and habitability; PCGamesN campaign preview. https://totalwarwarhammer.fandom.com/wiki/Jungle, https://www.pcgamesn.com/total-war-warhammer-ii/total-war-warhammer-2-campaign-gameplay-preview **[partially verified: search excerpts]**
- **TWW-R** — Wargamer, "Total War: Warhammer 3 factions" (race list). https://www.wargamer.com/total-war-warhammer-3/factions **[partially verified: search excerpt]**
- **MB2-MF** — TV Tropes and Fextralife lists of Bannerlord kingdoms, minor factions and bandit types. https://tvtropes.org/pmwiki/pmwiki.php/Characters/MountAndBladeIIBannerlordMinorFactions, https://mountandblade2bannerlord.wiki.fextralife.com/Factions **[partially verified: fan sources, search excerpts]**
- **DA** — Dragon Age Wiki (Fandom), "The Templar Order"; Wikipedia, "Dragon Age". https://dragonage.fandom.com/wiki/The_Templar_Order, https://www.wikipedia.org/wiki/Dragon_Age **[partially verified: search excerpts]**
- **WIT** — Witcher Wiki (Fandom), "Order of the Flaming Rose", "Lodge of Sorceresses"; CD Projekt RED forum summary. https://witcher-games.fandom.com/wiki/Order_of_the_Flaming_Rose, https://witcher.fandom.com/wiki/Lodge_of_Sorceresses **[partially verified: search excerpts]**
- **WIT-S** — Witcher Wiki and GameRant on the witcher schools. https://witcher.fandom.com/wiki/_School_of_the_Griffin, https://gamerant.com/witcher-3-8-witcher-schools-explained/ **[partially verified: search excerpts]**
- **FNV**, **FNV-R** — Fallout Wiki (Fandom), "Fallout: New Vegas factions"; The Vault, "Fallout: New Vegas reputations". https://fallout.fandom.com/wiki/Fallout:_New_Vegas_factions, https://fallout-archive.fandom.com/wiki/Fallout:_New_Vegas_reputations **[partially verified: search excerpts]**
- **FP2** — PC Gamer, "Frostpunk 2 factions and communities explained"; Prima Games guide. https://www.pcgamer.com/games/city-builder/frostpunk-2-factions, https://primagames.com/gaming/all-frostpunk-2-factions-and-communities-explained-differences-research-and-laws **[partially verified: search excerpts]**
- **ML** — PC Gamer and PCGamesN on the Manor Lords trade rework and rival lords; Steam patch notes on bandit camps. https://www.pcgamer.com/games/city-builder/manor-lords-dev-is-experimenting-with-a-new-trading-system-that-allows-for-deliberate-economic-strategies-including-establishing-monopolies-on-resources **[partially verified: search excerpts]**
- **ANNO** — Wikipedia, "Anno 1404" (Occident and Orient). https://wikipedia.com/wiki/Anno_1404 **[partially verified: search excerpt]**

### 6.4 History

- **GUILD-B** — Encyclopaedia Britannica, "Guild". https://www.britannica.com/topic/guild-trade-association
- **HANSA** — Wikipedia, "Hanseatic League" (read to character 100,000 of 245,000). https://en.wikipedia.org/wiki/Hanseatic_League
- **MILORD** — Wikipedia, "Military order (religious society)" (read to character 100,000 of 225,000). https://en.wikipedia.org/wiki/Military_order_(religious_society)
- **CIST** — Wikipedia, "Cistercians" (read to character 100,000 of 170,000); Britannica, "Cistercians". https://en.wikipedia.org/wiki/Cistercians, https://www.britannica.com/topic/Cistercians
- **INQ** — Wikipedia, "Inquisition" (medieval part). https://en.wikipedia.org/wiki/Inquisition
- **COND** — Wikipedia, "Condottieri". https://en.wikipedia.org/wiki/Condottieri
- **COMMUNE** — Wikipedia, "Medieval commune". https://en.wikipedia.org/wiki/Medieval_commune
- **HERM** — Wikipedia, "Santa Hermandad". https://en.wikipedia.org/wiki/Santa_Hermandad
- **NIZARI** — Wikipedia, "Order of Assassins". https://en.wikipedia.org/wiki/Order_of_Assassins
- **VICTUAL** — Wikipedia, "Victual Brothers". https://en.wikipedia.org/wiki/Victual_Brothers
- **NOMAD** — Wikipedia, "Eurasian nomads". https://en.wikipedia.org/wiki/Eurasian_nomads
