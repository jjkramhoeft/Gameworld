# Vocations: the Work of a Pre-industrial Fantasy World, as Input for the Generator

*Research date: 2026-11-15. Scope: which **vocations** (the work a person does) a medieval or fantasy world needs so that its economy and society function, reduced to 51 generic vocations that a generator can assign to persons. §1 states the method. §2 sets out the axes that tell vocations apart and the numbers the sources give. §3 is the curated list, each vocation with its variants, description, lore, resources (in the names of `map-generation.md` section 14 and Appendix G), siting (biome, elevation type, bedrock, water site, city or town, the Prosperity and Supernatural Fields), frequency, races and factions, and evidence. §4 covers the resources in both directions: which vocation wins and uses each of the 24 generated resources, and which resources the vocations need that the catalogue lacks. §5 is the view per biome. §6 is the view per race and per faction archetype. §7 is a design note on what a generator needs per vocation, how the vocation mix of a place could follow from its resources, size and races, and how a vocation stands to the role of a person, with the open questions. §8 lists the sources.*

**How to read the citations**

- Each claim cites the source that owns it, by key in square brackets. The keys are listed in §8. The order of trust is: historical primary sources and the scholarship that publishes them, then economic history, then games (developer material and official rules first, then established wikis, then guides).
- Web pages were read through a fetch tool that returns an extraction of the page, not the page itself. Counts and quotes are as the tool returned them. Two PDFs ([BCL] and [MDME]) were converted to text and read directly.
- **[partially verified]** means the source exists and is cited, but the claim was confirmed only from a search-index excerpt, from a secondary page that quotes it, or from part of a page. In this pass the Dwarf Fortress wiki answered 429, the Hooded Horse wiki 403, wiki.gg and one British History Online address 401, so Dwarf Fortress, Worlds Without Number, Anno 1404 and several history claims are at this level.
- **[unverified]** means general knowledge of history or of a game that was not checked against any source in this pass.
- The **Lore** lines draw on general knowledge of medieval custom and of the fantasy genre. They are [unverified] unless a key follows, and they are kept generic.
- Game-specific proper names appear only as **evidence**. The vocation names are generic and are the recommended names for the generator. No vocation is a copy of one game's class or profession.
- Everything under **Resources**, **Siting** and **Races and factions**, every share marked "starting share", and all of §4.4, §5, §6 and §7 is my own engineering judgement: it maps the vocation onto this repo's concepts and is not a sourced claim. Proposals for new resources are labelled as proposals.
- A code such as A2 or D5 is a faction archetype of `factions.md` §3. Vocations are numbered 1 to 51. Courtesan, number 51, was added on 2026-11-15 at the user's request, after the list was made: it stands in the Service sector, out of number order, and the numbers 1 to 50 are unchanged.

**Hooks in this repo** (working tree at commit `2c59ebe`, with uncommitted edits to the readme, society-generation.md and races-basis.md)

| Hook | Where | What it gives a vocation pass |
|---|---|---|
| Vocation | `readme.md`, "Vocation" | "The work a person does. Vocations are not specified yet, and how a vocation stands to the role of a person is not decided." |
| Person | `readme.md`, "Person"; `society-generation.md` §1.2, §7 | The Stub of a person holds name, age, sex, role, family links, faction membership and home. The vocation is not in that list |
| Open questions | `society-generation.md` §8 | Vocation and role: one fact or two. The list, and whether it is a Parameter Set list like the races. How the vocations of a place follow from its resources, its size and its races |
| Family | `readme.md`, "Family" | A household that "can include servants or slaves" |
| Race | `society-generation.md` §2, Appendix A; `races-basis.md` | 28 default races, each with a frequency per biome and a height modifier. Every entry of races-basis.md lists the work the race is known for |
| Potential population | `society-generation.md` §3 | Persons per race and region. The median region holds about 35,100 persons at the defaults, the range is 11,200 to 83,400 |
| Faction | `readme.md`, "Faction"; `research/factions.md` §3 | 48 archetypes, with ranks to fill with persons |
| City and town | `readme.md`, "City and town"; `map-generation.md` §15 | 20 Prospects per region, up to 4 for cities. Class Rich, Average or Poor. There is no village: a person outside a Prospect lives on a patch |
| Patch | `map-generation.md` §13, §14 | One of 15 land biomes or Mangrove. 24 raw resources with an amount in [0, 1] and a variant. A resource is listed from 0.25 |
| Zone | `map-generation.md` §8, §14.1, Appendix F | Elevation type (Flat Land, Lowland, Hills, Mountains) and one of nine bedrock types |
| Water site | `map-generation.md` §14.1, §15.1 | `river` (navigable from 8, large from 80), `mouth`, `lake`, `harbour`, `coast`, `confluence`, `head` |
| Resource groups | `map-generation.md` Appendix G.1 | Food, Materials, Fuel, Luxuries, Metals, Animals are found in patches. Building, Tools, Gear, Weapons, Clothing are only produced |
| Fields | `readme.md`, "Field"; `map-generation.md` §9 | Prosperity and Supernatural, sampled anywhere. "The supernatural field feeds nothing yet" |
| Travel network | `map-generation.md` §16 | Roads, river routes and sea lanes between the Prospects on show |
| Region stage | `readme.md`, "Region generation stage" | Planned holds factions, Prospects and the potential population. Settled holds actual cities and towns, families and important persons. The stage of the vocations is not decided |
| Earlier research | `research/game-resources-and-trade.md` §5.2 | 24 raw, 9 intermediate and 6 finished game resources with their production chains. The intermediates are what the processing vocations of §3 make |

---

## 1. Overview and method

**Scope.** A vocation here is a kind of work that a person can be said to do for a living, at the level a generator can assign and a player can recognise: farmer, smith, priest, guard. It is not a single trade of a guild roll. The Paris tax roll of 1292 names more than 300 occupational by-names [P1292-SCA], and a generator cannot tell a buckle maker from a clasp maker. Such trades are merged and named as variants.

**Sample.**

- **Historical primary sources, through the scholarship that publishes them:** the Paris *taille* of 1292 [P1292-SCA] [GIES-BC]; Étienne Boileau's *Livre des métiers* of about 1268 [LDM]; the English Poll Tax returns of 1377 to 1381 [BCL] [HOWDEN] [POOS]; Domesday Book [DB] [DB-HULL]; the Florentine Catasto of 1427 [CAT]; the register of the freemen of York [YORK]; the estate-management treatises of about 1275, among them the *Seneschaucy* [SEN]; monastic offices [OBED].
- **Economic history:** the sectoral shares of the English labour force from 1381 [BCL]; guild structure [GUILD] [LDM]; the share of nobles [NOB].
- **Games and tabletop, as evidence of what works in a generator:** Banished [BAN-P], Manor Lords [ML-B], Medieval Dynasty [MD-P], RimWorld [RW-W], Mount & Blade II: Bannerlord [MB2-W], Crusader Kings III [CK3-C], Dwarf Fortress [DF-L], Anno 1404 [ANNO-1404], the D&D 5e Basic Rules [DND-T] [DND-B], Pathfinder second edition [PF-B], Worlds Without Number [WWN-B] and S. John Ross's "Medieval Demographics Made Easy" [MDME].
- **This repo:** the resources and biomes of `map-generation.md`, the 28 races of `races-basis.md` and the 48 faction archetypes of `factions.md`.

**Method.** I collected the occupations of each source, grouped those that do the same job in the economy, and kept a group as a vocation when three things hold: it is a step of a production chain or a function that a settlement cannot do without; it appears in a historical source and in at least two games; and a generator can tell it from its neighbours by its inputs, its outputs or its siting. Where only the product differs (a cordwainer, a saddler and a glover all work leather) the trades are variants of one vocation. Where the resource decides the kind (an iron miner and a tin miner) the kind is a variant that the generator picks from the map. This gives 50 vocations in nine sectors. The user added a 51st, Courtesan, which does not pass the test (§3, entry 51).

**Six findings shape everything below.**

1. **The land takes most hands, but fewer than the usual rule says.** The best-documented medieval figure puts 57 % of the English labour force of 1381 in agriculture, 19 % in industry and 24 % in services, against earlier assumptions of 75 to 80 % [BCL]. The country was still 80 % rural [BCL]: a fifth of the work outside farming was done in villages.
2. **Status and occupation are two different descriptions of a person.** Domesday Book records nearly everyone by status (villager, smallholder, slave) and hardly anyone by craft [DB-HULL]. The graduated poll tax of 1379 spends about 95 % of its schedule on ranks and names the crafts once, as "artificers" [POOS]. The English Statute of Additions of 1413 demanded a person's "Estate or Degree, or Mystery" [POOS]: three facts, of which only the mystery is the vocation. This bears on the open question of vocation and role (§7.3).
3. **Town trades are dominated by food, clothing and leather, not by smiths.** In Paris in 1292 the largest groups are shoemakers, furriers, maidservants, tailors and barbers [GIES-BC]. In York between 1350 and 1399 a quarter of all new freemen were in textiles, 14 % were merchants, 13 % worked leather, 10 % sold food, 9 % worked metal and 6 % built [YORK]. In the small town of Howden in 1379 the victualling trades, mainly brewers and butchers, were 40 % of those with a named occupation [HOWDEN].
4. **Games converge on about 20 working roles.** Banished has 20 professions [BAN-P], RimWorld 21 work types in the base game [RW-W], Medieval Dynasty a little over 20 villager professions [MD-P], and the D&D Basic Rules 17 kinds of artisan's tools [DND-T]. The builders model the chains (ore, bloom, tool); the role-playing games model the person (background, tool). A generator that must do both lands between: 51 here, of which about 20 are the chain.
5. **The one widely used demographic tool for game masters rests on a single tax roll.** Ross took his Support Values "almost entirely from the Paris tax list of 1292", in the truncated form printed by Joseph and Frances Gies [MDME]. The values are those of the largest city of its day, and they miss the countryside. They are used below as an urban upper bound only.
6. **This repo's regions are thinly peopled, so thresholds must be relative.** A median region holds 35,100 persons (`society-generation.md` §3.3) on 16,384 km². Ross's "city" starts at 8,000 persons [MDME]; few regions here could hold one. The settlement tiers of §2.2 are therefore the repo's own (countryside, town, city, capital) and not head counts.

---

## 2. Axes: what tells vocations apart

Six axes describe every vocation of §3. The index table of §3 gives the values.

### 2.1 Sector

| Sector | What the work is | Vocations |
|---|---|---|
| **Land and water** (primary) | Wins a raw resource from a patch | 1 to 9 |
| **Processing** | Turns a raw resource into food, drink, fuel or a material: the first steps of a chain | 10 to 17 |
| **Craft** | Turns materials into finished goods | 18 to 28 |
| **Trade and transport** | Moves goods and people, buys and sells | 29 to 33 |
| **Service** | Works on persons, not on goods | 34 to 36 and 51 |
| **Governance and war** | Rules, administers, fights, keeps order | 37 to 40 |
| **Faith and learning** | Keeps the cult, the records and the knowledge | 41 to 43 |
| **Supernatural** | Works with what the Supernatural Field stands for | 44 to 46 |
| **Margins** | Unskilled, dependent or outlawed | 47 to 50 |

The split into primary, processing and craft follows the production chains of the builder games (Manor Lords: mining pit, bloomery, smithy [ML-B]) and the tiers of `game-resources-and-trade.md` §5.2. The economic historians use three sectors: agriculture, industry (with mining) and services [BCL]. Land and water without the miners is their agriculture; processing, craft and the miners are their industry; the rest is their services.

### 2.2 Rural or urban, and the settlement tier

The sources agree that settlement size decides which trades exist, and they disagree on the numbers. Ross calls 20 to 1,000 persons a village, 1,000 to 8,000 a town, 8,000 to 12,000 a city and above that a big city [MDME]. The poll-tax study counts towns above 2,000 as urban, places with more than 70 % of occupations in agriculture as rural, and the rest as semi-rural, and weights England 80 % rural, 10 % urban and 10 % semi-rural [BCL]. Small towns in England had fewer than 2,000 inhabitants and mostly more than 300 [DYER] [partially verified]. Goldberg tells a town from a village in the poll taxes by four signs: fewer married adults, more servants, more women than men, and more taxpayers with a named occupation [HOWDEN].

The repo has no head-count classes yet, so the note uses four tiers that it does have:

| Tier | In the repo | Vocations that begin here |
|---|---|---|
| **Countryside** | A land patch without a Prospect | All of land and water; miller, brewer, charcoal burner, smelter, smith, carpenter, weaver (spinning), priest, seer, healer (midwife), servant, labourer, noble (manor), official (reeve), bandit |
| **Town** | A Prospect for a town | Baker, butcher, tanner, potter, mason, leatherworker, tailor, shopkeeper, carter, innkeeper, soldier, guard, scribe, entertainer, courtesan, beggar, thief; sailor and shipwright where the town lies on water |
| **City** | A Prospect for a city | Merchant, jeweller, glassmaker, artist, scholar, alchemist |
| **Capital** | The best city Prospect of the region | Mage, artificer, the high officials, the moneyers |

A vocation of a lower tier is also found in every higher tier.

### 2.3 Inputs, outputs and the step in the chain

Each vocation consumes and produces resources. The raw ones are the 24 generated resources. The produced ones are named as in `game-resources-and-trade.md` §5.2 (charcoal, iron, bronze, cloth, leather, bricks and lime, glass, preserved food, naval stores, tools, arms and armour, clothing, drink and oil, fine goods, coin), with a few that the vocations add (flour, bread, meat, hides, wool, planks, pottery, parchment and paper, ships, medicine). §4.3 sets them against the five produced groups of Appendix G.1.

### 2.4 Social standing

| Standing | Meaning | Evidence for the scale |
|---|---|---|
| 1 Outcast | Outside the law or living on alms | The 1379 poll tax exempts "genuine beggars" [POOS] |
| 2 Poor | Sells unskilled labour, owns no tools of a trade | D&D: an untrained hireling earns 2 sp a day [DND-T] |
| 3 Common | Holds land or a simple trade | Domesday's villagers: over 40 % of the recorded population [DB-HULL] |
| 4 Middling | A skilled trade, a shop, a small office | D&D: a skilled hireling (a mercenary, an artisan, a scribe) earns 2 gp a day, ten times the untrained [DND-T] |
| 5 Wealthy | Capital, learning or high office | The merchants and lawyers that the 1379 schedule grades one by one [POOS] |
| 6 Ruling | Rules land and people | The same schedule's lords |

### 2.5 Entry: how a person comes to the work

- **Born:** the child takes the work of the house. Farmers, herders, nobles, most rural trades.
- **Apprenticed:** a guild or a master admits the person. An apprentice lived in the master's household without pay for five to nine years, then worked for wages as a journeyman, and became a master by a masterpiece [GUILD]. In Paris about three quarters of the trades were open to anyone who showed skill and swore the oath, the rest had to be bought; most trades had two or three sworn wardens, and night work was forbidden to most [LDM].
- **Appointed or elected:** officials. The manorial reeve was a peasant chosen yearly at Michaelmas, by the lord or by his fellows [REEVE] [SEN].
- **Ordained or sworn:** priests, monks, members of an order.
- **Hired:** servants, soldiers, labourers, sailors.
- **Fallen or driven:** beggars, thieves, bandits.

### 2.6 What the sources give as frequencies

**England, 1381, the labour force by sector** [BCL]. From the poll-tax returns edited by Fenwick: 30,292 individuals in 892 villages of 22 counties, re-weighted to 80 % rural, 10 % urban and 10 % semi-rural.

| | Agriculture | Industry (with mining) | Services |
|---|---|---|---|
| Male workers | 67.0 % | 15.2 % | 17.8 % |
| Female workers | 34.5 % | 28.4 % | 37.1 % |
| Total labour force (women taken as 30 % of the work) | 57.2 % | 19.2 % | 23.6 % |

15.0 % of the male workers are named only "labourer"; in the muster rolls of 1522 the figure is 25.9 % [BCL]. The study counts persons under 16 as outside the labour force (37.5 % of the population) and assumes that 97.1 % of men and 43.0 % of women worked [BCL]. With those rates about 43 % of all persons are in the labour force: about 15,100 of the 35,100 persons of a median region here (my arithmetic). A competing reading of the same returns, cited by Poos and Rust, puts only about 12 % of adult men in the secondary sector and 6 % in the tertiary [POOS].

**York, new freemen by trade group** [YORK].

| Trade group | 1350 to 1399 | 1400 to 1449 | 1450 to 1509 |
|---|---|---|---|
| Textile | 25 % | 19 % | 20 % |
| Mercantile | 14 % | 14 % | 10 % |
| Leather | 13 % | 11 % | 12 % |
| Provision | 10 % | 12 % | 15 % |
| Metal | 9 % | 9 % | 9 % |
| Building | 6 % | 7 % | 6 % |
| Miscellaneous | 11 % | 17 % | 19 % |
| Admissions in the period | 4,838 | 4,870 | 4,086 |

The freemen are the masters and traders of a regional capital. Servants, labourers, most women and the clergy are not among them.

**Paris, 1292, taxpayers by occupational by-name** [P1292-SCA]. The index gives the men's by-names in Géraud's edition of the roll with a count each. The roll has about 14,500 taxpayers [P1292-N] [partially verified]. The largest groups, merged by me into the vocations of §3:

| Vocation of §3 | By-names (count) | Sum |
|---|---|---|
| Leatherworker | cordoanier 222, çavetier 139, courraier 78, selier 51, gueinier 50, lormier 39, boursier 30, sueur 25, gantier 21, bourrelier 20 | 675 |
| Tailor | tailleeur 123, ferpier 112, cousturier 57, chapelier 47, tailleeur de robes 15 | 354 |
| Tanner (with furriers) | peletier 209, mesgeicier 21, conreeur 19, parcheminier 18, basanier 13, taneeur 2 | 282 |
| Baker | fournier 86, pataier 64, talemelier 61, oublier 27, queu 23, cuisinier 21 | 282 |
| Smith | fevre 74, fourbeeur 35, marechal 32, serrurier 26, armeurier 22, coutelier 21, clooutier 17 | 227 |
| Carpenter | charpentier 88, tounelier 70, huchier 29, charron 18, tourneeur 12 | 217 |
| Healer | barbier 150, mire 29 | 179 |
| Mason | maçon 102, plastrier 36, recouvreeur 21 | 159 |
| Weaver | tesserant 73, foulon 24, tapicier 23, tondeeur 20, tainturier 15 | 155 |
| Jeweller | orfevre 115, monnoier 19, perrier 10 | 144 |
| Innkeeper | tavernier 86, estuveeur 21, hostellier 12 | 119 |
| Carter and porters | porteeur d'yaue 58, charretier 46, descharcheeur 45, porteeur 42 | 191 |

The roll counts those who paid, in the largest city of the West; by-names are not always occupations; the clergy and most nobles did not pay; and the index's English glosses are not always right (it glosses *çavetier* as "cloth roller"; the word is the cobbler [unverified]). The list in Gies, which Ross used, gives shoemakers 366, furriers 214, maidservants 199, tailors 197, barbers 151, jewellers 131 and so on, for 51 trades [GIES-BC].

**Ross's Support Values** [MDME]: the number of persons that support one business. Shoemakers 150, furriers, maidservants and tailors 250, barbers 350, jewellers, taverns and old-clothes dealers 400, masons 500, carpenters 550, weavers 600, bakers 800, butchers 1,200, blacksmiths 1,500, doctors 1,700 (all healers 350), inns and tanners 2,000, booksellers 6,300. One noble household per 200 persons, one advocate per 650, one clergyman per 40, one law officer per 150 citizens. A critic who went back to the roll finds the list cut from several hundred entries to 51 and the smiths understated by about a factor of six [RAVEN].

**Domesday Book, 1086** [DB] [DB-HULL]. 268,984 recorded heads of household. Villagers are over 40 % and never less than 30 % in a county [DB-HULL]; smallholders are about a third and slaves a little under a tenth [DB-X] [partially verified]; the article gives 28,000 slaves [DB]. 5,624 mills, a low estimate, which Duby reads as one mill per 46 peasant households [DB]. About 95 % of the recorded population are peasants [DB-X] [partially verified]. Occupations appear only in passing: bakers, beekeepers, boatmen, brewers, carpenters, cobblers, cooks, fishermen, foresters, millers, potters, shepherds, soldiers and others [DB-HULL].

**Florence, 1427** [CAT]. The Catasto codes 99 trades for the heads of household, from peasant landlord and share-cropper to wool carder, notary, innkeeper and beggar. Only 44 % of the households are identified with a trade.

**Starting shares.** The index table of §3 gives each vocation a starting share of the labour force of a whole temperate region, town and country together. The shares are my own calibration. They are set so that the three sectors come out at the 1381 figures (58 % agriculture with two thirds of the labourers, 19 % industry, 23 % services), the miller at one per 46 households, and the town trades in the order of the Paris and York lists. They are a place to start a Parameter Set from, not a finding.

---

## 3. The curated vocations

Fifty-one vocations in nine sectors. Each entry has the same parts: what the vocation covers, a description, the lore, the resources, the siting, the frequency, the races and factions, and the evidence. The index gives the axes of §2. "Tier" is the lowest settlement tier of §2.2 in which the vocation is found. "Share" is the starting share of the labour force of a whole region (§2.6), my own calibration.

| # | Vocation | Sector | Tier | Standing | Entry | Share |
|---|---|---|---|---|---|---|
| 1 | Farmer | Land and water | Countryside | 3 | Born | 35 % |
| 2 | Gardener | Land and water | Countryside | 3 | Born | 3 % |
| 3 | Herder | Land and water | Countryside | 3 | Born | 8 % |
| 4 | Fisher | Land and water | Countryside | 3 | Born | 2 % |
| 5 | Hunter | Land and water | Countryside | 3 | Born or hired | 1 % |
| 6 | Forester | Land and water | Countryside | 3 | Born or appointed | 1.5 % |
| 7 | Miner | Land and water | Countryside | 3 | Born or hired | 0.7 % |
| 8 | Quarryman | Land and water | Countryside | 2 to 3 | Hired | 0.4 % |
| 9 | Salter | Land and water | Countryside | 3 | Born | 0.2 % |
| 10 | Miller | Processing | Countryside | 4 | Appointed or born | 1 % |
| 11 | Baker | Processing | Town | 4 | Apprenticed | 0.6 % |
| 12 | Brewer | Processing | Countryside | 3 to 4 | Born or apprenticed | 1.2 % |
| 13 | Butcher | Processing | Town | 4 | Apprenticed | 0.5 % |
| 14 | Charcoal burner | Processing | Countryside | 2 | Born | 0.3 % |
| 15 | Smelter | Processing | Countryside | 3 to 4 | Apprenticed | 0.2 % |
| 16 | Tanner | Processing | Town | 3 | Apprenticed | 0.4 % |
| 17 | Weaver | Processing | Countryside | 3 | Born or apprenticed | 3.5 % |
| 18 | Smith | Craft | Countryside | 4 | Apprenticed | 1 % |
| 19 | Carpenter | Craft | Countryside | 4 | Apprenticed | 1.5 % |
| 20 | Shipwright | Craft | Town on water | 4 | Apprenticed | 0.2 % |
| 21 | Mason | Craft | Town | 4 | Apprenticed | 1 % |
| 22 | Potter | Craft | Town | 3 | Born or apprenticed | 0.4 % |
| 23 | Glassmaker | Craft | City | 4 to 5 | Apprenticed | 0.05 % |
| 24 | Leatherworker | Craft | Town | 4 | Apprenticed | 1.2 % |
| 25 | Tailor | Craft | Town | 4 | Apprenticed | 1.2 % |
| 26 | Jeweller | Craft | City | 5 | Apprenticed | 0.15 % |
| 27 | Artist | Craft | City | 4 | Apprenticed | 0.1 % |
| 28 | Artificer | Craft | Capital | 5 | Apprenticed | 0.02 % |
| 29 | Merchant | Trade and transport | City | 5 | Born or apprenticed | 0.5 % |
| 30 | Shopkeeper | Trade and transport | Town | 3 | Open | 1.5 % |
| 31 | Carter | Trade and transport | Town | 3 | Hired | 1.2 % |
| 32 | Sailor | Trade and transport | Town on water | 3 | Hired | 1.2 % |
| 33 | Innkeeper | Trade and transport | Town | 4 | Open | 0.8 % |
| 34 | Healer | Service | Countryside | 3 to 5 | Apprenticed or learned | 0.5 % |
| 35 | Servant | Service | Countryside | 2 | Hired | 7.9 % |
| 36 | Entertainer | Service | Town | 2 to 4 | Open | 0.2 % |
| 37 | Noble | Governance and war | Countryside | 6 | Born | 1 % |
| 38 | Official | Governance and war | Countryside | 4 to 5 | Appointed or elected | 0.8 % |
| 39 | Soldier | Governance and war | Town | 3 to 4 | Hired or sworn | 1 % |
| 40 | Guard | Governance and war | Town | 3 | Hired | 0.6 % |
| 41 | Priest | Faith and learning | Countryside | 4 to 5 | Ordained | 2.5 % |
| 42 | Scribe | Faith and learning | Town | 4 to 5 | Learned | 0.4 % |
| 43 | Scholar | Faith and learning | City | 5 | Learned | 0.15 % |
| 44 | Mage | Supernatural | Capital | 5 | Learned or born | 0.03 % |
| 45 | Alchemist | Supernatural | City | 4 to 5 | Apprenticed or learned | 0.05 % |
| 46 | Seer | Supernatural | Countryside | 2 to 4 | Born or called | 0.2 % |
| 47 | Labourer | Margins | Countryside | 2 | Hired | 11 % |
| 48 | Beggar | Margins | Town | 1 | Fallen | 1.5 % |
| 49 | Thief | Margins | Town | 1 | Fallen | 0.3 % |
| 50 | Bandit | Margins | Countryside | 1 | Driven | 0.25 % |
| 51 | Courtesan | Service | Town | 1 to 5 | Fallen or apprenticed | 0.1 % |

The shares sum to 100 %.

### Sector 1. Land and water

#### 1. Farmer

Works arable land: ploughs, sows, weeds, reaps and threshes, and keeps a few animals and a garden beside it. Most farmers feed their own house first and owe the rest to a lord, a temple or the market.

- **Covers:** ploughman, peasant, free farmer, tenant, share-cropper, paddy or rice farmer, millet or yam farmer, field hand with land of his own.
- **Lore:** the common estate, the measure against which every other standing is set. Bound by custom more than by law: the days owed on the lord's land, the common field, the village assembly. Harvest and sowing rule the year and its feasts. The conflicts are with the lord over dues, with the herder over trampled crops, with the miller over his toll. Fantasy twists: a people that will not plough (the Catfolk), a people that works only by day (the Noonfolk), land whose yield a seer blesses.
- **Resources:** produces **grain** and **produce**, with **fibre** (flax, hemp, cotton, jute) as a field crop. Consumes Tools (plough, sickle), **horses & pack animals** or **livestock** for draught, seed. The first step of the food chain; hands grain to the miller (10) and the brewer (12).
- **Siting:** wherever the crop score of `map-generation.md` §14.2 is above nothing. Best in Temperate grassland, Temperate broadleaf forest, Temperate mixed forest and Woodland & shrubland (soil 1), on Flat Land and Lowland (farmland 1), on a floodplain or with irrigation from a river. Weak in Boreal forest, Temperate rain forest, Savanna and Tropical rain forest (soil 0.4 to 0.6). Absent on Ice & snow, nearly absent on Tundra, and in the deserts only along a river. Countryside; the fields of a town are worked from the town.
- **Frequency:** 57.2 % of the English labour force of 1381 was in agriculture, where earlier estimates assumed 75 to 80 % [BCL]. Villagers are over 40 % of the Domesday population [DB-HULL] and smallholders about a third more [DB-X] [partially verified]. Starting share 35 %, to which most of the labourers (47), the gardeners and the herders add.
- **Races and factions:** Midlanders (ploughman), Canal folk (paddy farmer), Rainkeepers (rice farmer), Sunlanders (millet or yam farmer), Noonfolk (grain farmer), Northmen (free farmer), Flinchless, Twofold of the Staying. Never the Catfolk. Factions: A5 peasant commune, A2 noble house (as tenants), B2 monastic order (as tenants), D6 frontier settlers' company.
- **Evidence:** Domesday's villagers and smallholders [DB] [DB-HULL]; the ploughmen, harrowers, mowers and reapers of the *Seneschaucy* [SEN]; Catasto codes 01 to 04, from peasant landlord to agricultural labourer [CAT]. Banished Farmer [BAN-P], Manor Lords farmhouse and fields [ML-B], Medieval Dynasty Farmer [MD-P], RimWorld Grow [RW-W], the D&D Folk Hero [DND-B].

#### 2. Gardener

Tends trees, vines and gardens: crops that stand for years or need daily care, and that are sold more than eaten at home.

- **Covers:** orchard keeper, vine dresser, olive grower, date gardener, spice grower, tea, coffee or tobacco grower, dye-plant grower, silk grower, beekeeper, mushroom grower, seedsman, grafter.
- **Lore:** patient work with a long horizon: an olive tree outlives its planter. Orchards and vineyards are wealth that an army can destroy in a day, so gardeners favour peace and strong walls. The luxury crops are regional secrets, guarded by a town or a lineage. Fantasy twists: a grove that only one people can tend, a poison garden, a spice whose source is kept off every map.
- **Resources:** produces **orchard**, **spices**, **stimulants & sugar** (with honey and beeswax), **dyes** (the dye crops) and the tree **fibre** (silk, kapok, sisal). Consumes Tools, water. First step; hands fruit to the brewer (12) and the miller (10, oil press), spices and dyes to the merchant (29), dyes to the weaver (17).
- **Siting:** Woodland & shrubland and the temperate forests on Hills and Lowland for olive, grape, apple and nuts (farmland factor 1 in hills). Hot desert and Thorn scrub on a river for the date palm. Tropical dry forest, Tropical rain forest and warm coasts for spices, cacao and sugar; tea and coffee in Hills and Mountains of the tropical highland. The luxury crops are gated to a hashed 5 to 30 % of the zones, more where Prosperity is high (`map-generation.md` §14.2), so a spice grower exists only in such a zone. Countryside, and inside towns as gardens.
- **Frequency:** no general figure found. Six *courtilliers* (gardeners) and two vine growers pay tax inside Paris itself [P1292-SCA]. Starting share 3 %, far higher in a zone with a gated crop.
- **Races and factions:** Halflings (gardener, orchard keeper, beekeeper, tobacco grower), Swiftborn (vine dresser, olive picker), Saltroad folk (date gardener, incense gatherer, coffee grower), Rainkeepers (spice grower), Canal folk (silk grower, tea grower), Gnomes (seedsman, grafter, mushroom grower), Noonfolk (orchard keeper), Boughwalkers (spice gatherer), Amberbloods and Stillfaces (beekeeper), Lanternkin (fungus gardener), Bitterbloods (poison gardener). Factions: B2 monastic order, D4 chartered company (plantations), A2 noble house.
- **Evidence:** Domesday beekeepers [DB-HULL]; the monastery garden under the cellarer [OBED]; Catasto code 07 *ortolano* [CAT]. Manor Lords apiary, apple orchard and vegetable garden [ML-B], Medieval Dynasty Beekeeper [MD-P], Banished Gatherer and orchards [BAN-P], Crusader Kings III Court Gardener [CK3-C], Anno 1404 monastery garden [ANNO-1404] [partially verified].

#### 3. Herder

Keeps animals on pasture and moves with them: guards, breeds, milks, shears and drives to market.

- **Covers:** shepherd, cowherd, swineherd, goatherd, reindeer herder, yak herder, nomad herder, dairymaid and cheesemaker, horse breeder and horse breaker, camel breeder.
- **Lore:** lives apart from the village for much of the year, on the summer pasture or the open range, and is thought half wild for it. Wealth is counted in heads. Whole peoples are herders and scorn the plough. The conflicts are over grazing rights, wells, rustling and winter fodder. Fantasy twists: herds of beasts no farmer could keep, mounts bred for one clan only.
- **Resources:** produces **livestock** and **horses & pack animals**, and from them the by-products wool, hides, milk and cheese, tallow, horn and dung **fuel**. Consumes **salt**, pasture, winter fodder (**grain**). First step; hands animals to the butcher (13), hides to the tanner (16), wool to the weaver (17), mounts to the carter (31) and the soldier (39).
- **Siting:** follows `pasture` and `mounts` (Appendix C). Best in Temperate grassland (pasture 1, mounts 0.9), Cold steppe (sheep, horses), Savanna (cattle), Woodland & shrubland (sheep, goats). Reindeer on Tundra and in Boreal forest, yak and goats in Mountains, camels in Hot desert and Cold desert, pigs in Temperate broadleaf forest. Poor in Tropical rain forest and Mangrove (0.1 to 0.15), absent on Ice & snow. Countryside, far from Prospects for the nomads.
- **Frequency:** no share found. Ross puts livestock at 2.2 times the human population, two thirds of it fowl [MDME]. Shepherds, swineherds and cowherds pay tax even in Paris (10, 11 and 8) [P1292-SCA]. Starting share 8 %; the majority in Cold steppe and Temperate grassland.
- **Races and factions:** Grass riders (herder, horse breaker), Sunlanders (cattle herder), Saltroad folk (camel breeder), Orcs, Hearthbloods (yak herder), Widegaze (range herder), Dwarves and Duskreaders (goat herd), Stillfaces (swineherd), Halflings (cheesemaker), Twofold of the Going. Factions: H1 nomad horde, H2 highland and forest tribe, A2 noble house.
- **Evidence:** Domesday shepherds, goatherds and dairy maids [DB-HULL]; the keeper of cattle and the dairymaids of the *Seneschaucy* [SEN]; Catasto code 05 shepherd [CAT]. Banished Herdsman [BAN-P], Manor Lords pasture and sheep farm [ML-B], Medieval Dynasty Animal Breeder [MD-P], RimWorld Handle [RW-W].

#### 4. Fisher

Takes food and goods from water: with net, line, trap and spear, from a boat or the shore, and by diving.

- **Covers:** sea fisher, river fisher, eel trapper, shellfish gatherer, crab and shrimp catcher, whaler, sealer, pearl diver, sponge and coral diver, fish drier.
- **Lore:** a hard and uncertain living with its own saints and taboos: words not said at sea, days on which no boat goes out. The catch spoils in a day, so the fisher depends on the salter and the fishmonger. Fishing villages are poor, close and suspicious of the land. Fantasy twists: a people that dives where others drown, things in the net that should not be.
- **Resources:** produces **fish**, and by diving and sealing **gems & pearls** (pearl oyster, precious red coral), **furs & ivory** (seals, walrus, whales) and **dyes** (murex). Consumes **salt**, **fibre** (nets, line), boats from the shipwright (20). First step; hands the catch to the butcher (13, fishmonger and fish salter).
- **Siting:** every coastal patch (fish 0.8, 1 in a harbour, 0.9 in Mangrove, 0.6 in the polar belt), the shelf of ocean patches, rivers from `river` 2 (0.3) to large rivers (0.7), and lakes (0.45 to 0.75). Cold river mouths give salmon runs. Pearls on warm coasts, coral on temperate coasts in Hills and Mountains, seals and whales in cold water. Absent inland away from rivers and lakes. Countryside and every town on water.
- **Frequency:** no share found. Domesday records fisheries by their renders, such as 1,000 eels a year from one millpond [DB-LIFE]. Paris has 8 fishers against 33 fishmongers [P1292-SCA]. Starting share 2 %, 10 to 30 % on a coast without farmland.
- **Races and factions:** Shoalfolk (pearl diver, sponge diver, fisher, shellfish gatherer, fish drier), Hoarkin (seal hunter, whaler), Northmen (herring fisher, whaler), Catfolk (fisher). Factions: A5 peasant commune on the coast, F5 pirates (as a second living).
- **Evidence:** Domesday fishermen and fisheries [DB-HULL]; Catasto code 06 *pescatore* [CAT]. Banished Fisherman [BAN-P], Medieval Dynasty Fisher [MD-P], RimWorld Fish [RW-W], Dwarf Fortress fishery work [DF-L] [partially verified].

#### 5. Hunter

Lives from the wild land: hunts, traps, gathers, and knows the way through it.

- **Covers:** hunter, trapper, fowler, falconer, forager, honey hunter, tracker, guide, scout, ratcatcher, monster hunter.
- **Lore:** free where the game is free, a poacher where the lord has claimed it. Hunting is the sport of nobles and the bread of the poor, and the two meet in the courts. The hunter is the one who knows the pass, the ford and the safe spring, and is hired for that knowledge by caravans and armies. Fantasy twists: prey that hunts back, a guild that takes contracts on beasts, a people that tracks by scent.
- **Resources:** produces **wild food & game**, **furs & ivory**, and the wild **dyes** (kermes, lichen) and herbs. Consumes Weapons (bow, spear, traps). First step; hands meat to the butcher (13), pelts to the tanner (16), ivory to the jeweller (26).
- **Siting:** follows `game` and `fur` (Appendix C). Best in the temperate forests (game 0.8), Boreal forest (game 0.7, fur 0.85), Savanna and the tropical forests (game 0.6, ivory). Furs count only in a hashed share of the zones (§14.3). Poor in Hot desert and Cold desert (0.05 to 0.1). As a guide: wherever the road cost is high (Ice & snow 5, deserts 3.5 to 4, Mangrove 3, the rain forests 2). Countryside; in towns as ratcatcher and as a guide for hire.
- **Frequency:** no share found. Starting share 1 %; the largest vocation on Tundra and in Boreal forest without farmland.
- **Races and factions:** Stillfaces (tracker, hunting guide, truffle hunter, herb gatherer), Catfolk (hunter, tracker, trapper, fowler, ratcatcher, caravan scout), Elves (ranger of the forest edge), Orcs (hunter, scout), Northmen (trapper), Duskreaders (night guide, tracker, night hunter), Boughwalkers (fruit gatherer, honey hunter, bird catcher), Saltroad folk (guide), Grass riders (scout, falconer), Hoarkin, Brinefolk and Bitterbloods (guide over ice, salt flat and swamp), Widegaze, Amberbloods, Hearthbloods, Sunlanders, Twofold. Factions: E6 monster hunters, E4 wardens, H2 tribe, H4 beastfolk tribe, D8 caravan company, G2 explorers' society.
- **Evidence:** Domesday foresters, fowlers and falconers [DB-HULL]; the parker of the *Seneschaucy* [SEN]; five falconers and three bird sellers in Paris [P1292-SCA]. Banished Hunter and Gatherer [BAN-P], Manor Lords hunting camp and forager hut [ML-B], Medieval Dynasty Hunter [MD-P], RimWorld Hunt [RW-W], Crusader Kings III Master of the Hunt [CK3-C].

#### 6. Forester

Fells, hauls and saws timber, cuts firewood, and keeps the wood: plants, coppices and guards it.

- **Covers:** woodcutter, lumberjack, sawyer, forest warden, grove warden, resin tapper, tree shaper, firewood seller.
- **Lore:** two faces. The woodcutter is a poor man with an axe. The forester is the lord's officer who fines him. Forest law is hated law. Old woods have their own rules about which tree may fall. Fantasy twists: woods with a will, elder trees worth a ship each, peoples who judge every felling.
- **Resources:** produces **timber** and **fuel** (firewood). Consumes Tools (axe, saw), **horses & pack animals** or **livestock** (oxen) for hauling. First step; hands logs to the carpenter (19), the shipwright (20) and the charcoal burner (14), bark to the tanner (16), pit props to the miner (7).
- **Siting:** follows `wood` (Appendix C). Boreal forest, the three temperate forests and Tropical rain forest (1), Tropical dry forest (0.8), Mangrove (0.5). Gallery woods along rivers in Temperate grassland and Cold steppe. None in the deserts away from a river, on Tundra or on Ice & snow. Best beside a river that can float logs. Countryside.
- **Frequency:** no share found. Paris has 18 firewood sellers and 7 sawyers [P1292-SCA]. Starting share 1.5 %, several times that in a forest biome.
- **Races and factions:** Elves (forester, tree shaper), Amberbloods (forester, grove warden, resin tapper), Boughwalkers (tree keeper). Factions: H5 elven enclave, B7 druidic circle, D6 frontier settlers' company, A2 noble house (the forest officers).
- **Evidence:** Domesday foresters [DB-HULL]; the hayward who guards the woods [SEN]. Banished Forester and Woodcutter [BAN-P], Manor Lords logging camp, woodcutter's lodge, sawpit and forester's hut [ML-B], Medieval Dynasty Lumberjack [MD-P], RimWorld Plant cut [RW-W], Dwarf Fortress woodworking [DF-L] [partially verified].

#### 7. Miner

Wins ore, coal and gems from the ground: digs pits and shafts, washes river gravel, follows a vein.

- **Covers:** ore miner, prospector, gold washer, tin streamer, coal miner, gem miner, crystal miner. The variant follows the deposit of the patch.
- **Lore:** dangerous work, and free where it is rare: miners held their own courts and laws in many lands [unverified]. A strike makes a town in a year and an empty one in ten. Miners are clannish and proud and carry tales of what knocks in the dark. Fantasy twists: the dwarven hold, things dug up that should have stayed, ore that only one people can smelt.
- **Resources:** produces **iron ore**, **copper ore**, **tin**, **silver & lead**, **gold**, **gems & pearls** (the mined gems), **crystal** (rock crystal) and **fuel** (coal, lignite). Consumes Tools (picks), **timber** (props), **fuel**, lamps. First step; hands ore to the smelter (15), gems to the jeweller (26).
- **Siting:** by bedrock and elevation type (`map-generation.md` §14.4, Appendix D.2, Appendix F). Fold belt in Hills and Mountains: silver, lode gold, garnet, emerald. Granite intrusion: tin, skarn iron, topaz. Volcanic arc: copper, gold, rock crystal, amethyst. Ophiolite: copper, jade. Carbonate platform: lead and zinc, skarn iron. Crystalline shield: banded iron, lode gold in Hills, diamond. Flood basalt: native copper, agate. Clastic basin: coal, lignite, red-bed copper, opal. Placer gold and stream tin on rivers below the hills. Bog iron in wet forest lowland and laterite in the tropical lowland need no mine, only a digger. Any biome; a deposit is rare (a few per cent of the patches), so a miner exists only where the map has one. Countryside, with a mining town where the deposit is rich.
- **Frequency:** no share found; the 1381 study counts miners in industry without a figure of their own [BCL]. Starting share 0.7 % of a region, all of it in the few patches with a deposit.
- **Races and factions:** Dwarves (miner, prospector), Midlanders (miner), Sunlanders (gold washer), Hearthbloods (winter miner). Never the Noonfolk. Factions: D5 miners' guild, H3 dwarven hold, A1 realm crown (the royal mines).
- **Evidence:** Domesday iron-workers [DB-X] [partially verified]. Banished Miner [BAN-P], Manor Lords mining pit and deep mine [ML-B], Medieval Dynasty Miner [MD-P], RimWorld Mine [RW-W], Dwarf Fortress, where mining is the one labour that is a category of its own [DF-L] [partially verified].

#### 8. Quarryman

Breaks, splits and dresses stone in an open quarry, and digs sand, gravel and clay.

- **Covers:** quarrier, stonecutter, slate splitter, millstone cutter, sand and gravel digger, clay digger.
- **Lore:** heavy, dusty work for the strong and the unfree: quarries are where convicts and captives are sent. A good quarry is known by name, and a cathedral may be built from one hill. Fantasy twists: stone that holds a charm, quarries worked by giants or by the dead.
- **Resources:** produces **stone** (with the variant of the bedrock: granite, slate, marble, basalt, limestone, chalk, sandstone, millstone, whetstone, flint, obsidian, soapstone), **clay** and **crystal** (glass sand). Consumes Tools (wedges, hammers), **horses & pack animals** or **livestock** (oxen) for haulage. First step; hands stone to the mason (21) and the lime burner (14), millstones to the miller (10), clay to the potter (22), sand to the glassmaker (23).
- **Siting:** the chance of stone per patch is 0.75 in Hills and 0.95 in Mountains, 0.12 in Lowland and 0.04 on Flat Land, times a bedrock factor: more on Carbonate platform, Flood basalt and Clastic basin, less on Unconsolidated cover. Clay is the reverse: Flat Land and Lowland, more beside a lowland river. Glass sand on coasts and navigable rivers. Any biome. Best next to water, since stone travels badly by land. Countryside.
- **Frequency:** Paris has 18 quarriers, 12 stone carvers and 4 gravel diggers against 102 masons [P1292-SCA]. Starting share 0.4 %.
- **Races and factions:** Noonfolk (quarryman), Orcs (quarry hand), Dwarves. Factions: D2 craft guild (with the masons), A1 realm crown (the works of a castle), H3 dwarven hold.
- **Evidence:** [P1292-SCA]. Banished Stonecutter [BAN-P], Manor Lords stonecutter camp [ML-B], RimWorld stonecutting under Craft [RW-W].

#### 9. Salter

Makes salt: boils brine, rakes sea salt from pans, cuts rock salt.

- **Covers:** salt boiler, pan master, salt raker, rock-salt miner, brine worker, soda burner.
- **Lore:** salt is the one thing every house must buy, so rulers tax it and smugglers run it. A salt town is rich, hot and bare, since the pans eat the woods around it. Oaths are sworn on salt. Fantasy twists: a people with salt in the blood, salts that keep the dead from rising.
- **Resources:** produces **salt** (solar sea salt, lake and playa salt, natron and soda, rock salt, brine springs). Consumes **fuel** in great amounts where brine is boiled, **clay** or lead (pans), Tools. First step; hands salt to the butcher (13), the fisher (4), the tanner (16), the herder (3) and the glassmaker (23, natron).
- **Siting:** only where the map has the deposit. Dry, warm, low coasts (chance 0.25 per patch). Arid river-poor flat zones, in 8 % of them (playa salt, natron). Clastic basin outside Mountains, in 5 % of the zones (rock salt, brine springs). So Hot desert, Cold desert, Thorn scrub and dry Woodland & shrubland coasts, and any biome on a salt-bearing Clastic basin. Countryside, with a town at a rich works.
- **Frequency:** no share found. Domesday centres salt-making on one town with its brine pits and salt pans [DB-X] [partially verified]. Paris has 8 salt sellers, 5 sworn salt measurers and a salt carrier [P1292-SCA]. Starting share 0.2 %.
- **Races and factions:** Brinefolk (salt boiler, pan master, rock-salt miner, soda burner), Canal folk (the salt commissioner taxes them). Factions: D2 craft guild, D3 merchant league, A1 realm crown (the salt tax), F6 smugglers' ring.
- **Evidence:** Domesday salt-workers [DB-X] [partially verified]. Anno 1404 salt mine and salt works in the leather chain [ANNO-1404] [partially verified]. Salt is "deliberately sparse" in the game resource set (`game-resources-and-trade.md` §5.2).

### Sector 2. Processing

#### 10. Miller

Runs a mill: grinds grain to flour and meal, presses oil, and takes a share of what he grinds as toll.

- **Covers:** water miller, windmill keeper, horse-mill keeper, oil presser, and the mill's other uses: fulling mill, sawmill.
- **Lore:** holds the lord's monopoly, since tenants must grind at the lord's mill, and is suspected by all of a heavy thumb on the scale. The mill stands apart, by the water, and draws tales. The miller is the richest commoner of a village and the least loved. Fantasy twists: a mill that grinds at night for other customers.
- **Resources:** consumes **grain**, **orchard** (olives, oil seeds), **stone** (millstone and quern stone), **timber** (the machinery). Produces flour, meal and oil (drink and oil). Second step of the bread chain, between the farmer (1) and the baker (11).
- **Siting:** wherever grain is grown. A water mill needs `river` of 1 or more, with enough fall: best in Lowland and Hills. Windmills on Flat Land, coasts and in dry land without streams. Countryside, about one for each village.
- **Frequency:** 5,624 mills in Domesday, a low estimate, read as one mill per 46 peasant households [DB]. Paris has 51 *muniers* [P1292-SCA]. Starting share 1 %.
- **Races and factions:** Midlanders, Halflings. Never the Catfolk. Factions: A2 noble house and B2 monastic order (the mill owners), A5 peasant commune.
- **Evidence:** [DB]; Catasto code 42 *mugnaio* [CAT]. Manor Lords windmill [ML-B], Bannerlord mill (grain to flour) [MB2-W], Medieval Dynasty Miller [MD-P].

#### 11. Baker

Turns flour into bread and other cooked food for sale, and bakes the dough that households bring to the common oven.

- **Covers:** baker, keeper of the common oven, pastry cook, pie maker, cook, cookshop keeper.
- **Lore:** the most watched trade in a town. The weight and price of the loaf are fixed by law, and a baker who cheats is paraded with the loaf round his neck [unverified]. Up before dawn. Bread riots start at the baker's door.
- **Resources:** consumes flour (from **grain**), **fuel**, **salt**, **produce**, honey (**stimulants & sugar**). Produces bread and cooked food. Last step of the bread chain.
- **Siting:** follows people, not land: every town and city, and the common oven of a village. Needs fuel within reach. Any biome with grain or with a grain trade.
- **Frequency:** Paris: 86 *fourniers* (public oven keepers), 61 bread bakers, 64 pastry makers, 27 wafer sellers and 44 cooks [P1292-SCA]. Ross: one baker per 800 persons, one pastry cook per 500 [MDME]. The bakers had up to twelve sworn wardens where most trades had two or three [LDM]. Starting share 0.6 %.
- **Races and factions:** Halflings (baker, cook), Swiftborn (cook). Never the Catfolk. Factions: D2 craft guild, A3 free city council (which sets the assize).
- **Evidence:** Domesday bakers and cooks [DB-HULL]; Catasto codes 41 *fornaio* and 16 *cuoco* [CAT]. Manor Lords communal oven and bakery extension [ML-B], RimWorld Cook [RW-W], Medieval Dynasty Cook [MD-P], D&D cook's utensils [DND-T].

#### 12. Brewer

Makes drink: malts and brews ale and beer, presses and ferments wine and cider, makes mead.

- **Covers:** brewer, alewife, maltster, wine maker, cider maker, mead maker, distiller.
- **Lore:** in the country brewing is women's by-work in every second house, and the ale-stake over the door is the first inn. In towns it becomes a trade, and in monasteries an art. Drink is safer than water and is wages, medicine and sacrament. Fantasy twists: dwarven beer, halfling cider, a vintage that remembers.
- **Resources:** consumes **grain** (barley malt), **orchard** (grape, apple), honey (**stimulants & sugar**), **fuel**, water, barrels from the carpenter (19). Produces drink (drink and oil). Second step, from the farmer (1) and the gardener (2) to the innkeeper (33).
- **Siting:** ale where grain grows, wine where the grape does (Woodland & shrubland and the temperate forests on Hills and Lowland), palm wine and rice wine in the tropical biomes. Needs good water: `river` or a spring. Countryside and every town.
- **Frequency:** in Howden in 1379 the victualling trades, mainly brewers and butchers, were 40 % of those with a named occupation; in the village of Riccall 7 of 11 [HOWDEN]. Paris has 35 beer makers, 86 taverners and 49 wine sellers [P1292-SCA]. Starting share 1.2 %, with many more who brew beside other work.
- **Races and factions:** Halflings (brewer, cider maker), Dwarves (brewer), Midlanders (brewer, vintner), Elves (vintner). Factions: B2 monastic order, D2 craft guild.
- **Evidence:** Domesday brewers [DB-HULL]. Banished Brewer [BAN-P], Manor Lords malthouse and brewery extension [ML-B], Bannerlord brewery (grain to beer) [MB2-W], Crusader Kings III Court Brewmaster [CK3-C], D&D brewer's supplies [DND-T], Anno 1404 monastery brewery [ANNO-1404] [partially verified].

#### 13. Butcher

Slaughters and cuts up animals and sells the meat, and deals with all that an animal or a fish leaves: salts and smokes it, renders the fat.

- **Covers:** butcher, slaughterer, poulterer, tripe seller, fishmonger, fish salter, pickler, renderer, tallow chandler, soap boiler.
- **Lore:** a rich and rough trade, kept to its own street and banned from fouling the river. Butchers are strong, armed with their tools and quick to riot, and town councils fear them. Blood and offal give the trade a taint that money does not wash off.
- **Resources:** consumes **livestock**, **fish**, **wild food & game**, **salt**, **fuel** (smoke). Produces meat and preserved food, hides for the tanner (16), tallow for candles and soap, horn and bone. Second step of the animal chain.
- **Siting:** every town, on the downstream side and by running water. Fish salting on the coast and at river mouths, next to salt. Preserved food is "the main salt sink" (`game-resources-and-trade.md` §5.2).
- **Frequency:** Paris: 33 butchers, 46 poultry sellers, 33 fishmongers, 61 tallow chandlers [P1292-SCA]. Ross: one butcher per 1,200 persons, one chandler per 700 [MDME]. The butchers' corporation is notably absent from the *Livre des métiers* [LDM]. Starting share 0.5 %.
- **Races and factions:** Orcs (butcher), Catfolk (butcher), Brinefolk (pickler, fish salter), Hoarkin (blubber renderer). Factions: D2 craft guild.
- **Evidence:** Catasto codes 28 *beccaio* and 47 *pizzicagnolo* [CAT]. Bannerlord butcher (livestock to meat) [MB2-W], RimWorld, where the cook also butchers [RW-W].

#### 14. Charcoal burner

Turns wood into the fuels and stuffs that industry needs, by slow fire in the forest: charcoal, tar, pitch, potash, and lime from limestone.

- **Covers:** collier, tar burner, pitch boiler, ash burner, varnish maker, lime burner.
- **Lore:** lives in the wood by the smoking clamp for weeks, black, poor and seldom seen in the village, and is suspected of dealing with whatever else lives there. Outlaws hide among charcoal burners. Without them no iron is made.
- **Resources:** consumes **timber** and **fuel** (firewood); the lime burner also **stone** (limestone, chalk). Produces charcoal (a variant of **fuel**), naval stores (tar and pitch), potash, lime. Second step, between the forester (6) and the smelter (15), smith (18), glassmaker (23), shipwright (20) and mason (21).
- **Siting:** in the forest, since charcoal is lighter to carry than the wood it came from: Boreal forest (tar from pine and larch), the temperate forests, Tropical dry forest, Mangrove (the fuel variant there is charcoal). Within a short haul of a smelter or a town. Lime burners on Carbonate platform. Absent where `wood` is near 0.
- **Frequency:** no share found. Experimental smelts need 10 to 18 kg of charcoal for 1 kg of iron, made from 63 to 150 kg of wood [IRON] [partially verified], so every smelter keeps several burners at work (my inference). Paris has 13 charcoal sellers [P1292-SCA]. Starting share 0.3 %.
- **Races and factions:** Northmen (tar burner), Amberbloods (varnish maker, resin tapper). Factions: D5 miners' guild (as suppliers), F4 bandit clan (as cover).
- **Evidence:** Manor Lords charcoal kiln (charcoal from firewood) [ML-B], Anno 1404 charcoal burner [ANNO-1404] [partially verified], Dwarf Fortress wood burning [unverified]. Charcoal and naval stores are intermediates of the game resource set (`game-resources-and-trade.md` §5.2).

#### 15. Smelter

Turns ore into metal: roasts, smelts and refines it in a furnace, and casts or hammers it into bars.

- **Covers:** bloomery man, iron smelter, furnace master, copper smelter, silver refiner, steel maker, bronze founder.
- **Lore:** a secret art. The smelter knows by the colour of the flame when the bloom is ready, and passes that on to a son only. In many peoples the smelter is a ritual figure, feared and set apart. The works move when the wood is gone.
- **Resources:** consumes **iron ore**, **copper ore**, **tin**, **silver & lead**, **gold**, charcoal (**fuel**), **clay** (furnace), **stone** (limestone flux). Produces iron, steel, copper, bronze, lead, silver and gold in bars. Second step, between the miner (7) and the smith (18) and jeweller (26). Bronze needs copper and tin, which never lie in the same zone (`map-generation.md` §14.4), so a bronze founder sits on a trade route.
- **Siting:** where ore and fuel are within a short haul (`game-resources-and-trade.md` §5.2): a mining patch in or beside a forest biome, or on coal. Water power (`river` 1 or more in Hills) for bellows and hammers. Bog iron in Boreal forest and the temperate forests and laterite in Savanna and the tropical forests are smelted by farmers in the off season. Countryside.
- **Frequency:** no share found. Paris has only two refiners and two founders [P1292-SCA]: smelting is not town work. Starting share 0.2 %.
- **Races and factions:** Sunlanders (iron smelter), Rainkeepers (steel maker), Dwarves. Factions: D5 miners' guild, H3 dwarven hold, C4 artificers' society.
- **Evidence:** Manor Lords bloomery (iron slabs from ore, needs fuel) [ML-B], RimWorld smelting under Craft [RW-W], Dwarf Fortress furnace operating [unverified].

#### 16. Tanner

Makes leather, fur and parchment from raw hides and pelts, by months of soaking in lime, bark and worse.

- **Covers:** tanner, tawyer, currier, skinner, furrier (pelt dresser), parchment maker.
- **Lore:** the foulest trade in a town, banished downstream and downwind, and its people marry among themselves. Yet leather is in every shoe, saddle, belt and book, and furs are the mark of rank. The furriers are rich where the tanners are shunned.
- **Resources:** consumes hides (from **livestock** and **wild food & game**), pelts (**furs & ivory**), bark (from **timber**: oak, mangrove), lime (from **stone**), **salt**, alum, much water. Produces leather, dressed furs, parchment. Second step, between the butcher (13) or hunter (5) and the leatherworker (24), tailor (25) and scribe (42).
- **Siting:** a town with livestock or game in its catchment and bark within reach: "pairs steppe with forest" (`game-resources-and-trade.md` §5.2). On a river, below the town. Oak bark in Temperate broadleaf forest, mangrove bark in Mangrove. Furriers where furs are traded: Boreal forest, Tundra, and every city.
- **Frequency:** Paris: 209 *peletiers* (furriers), 21 tawyers, 19 curriers, 18 parchment makers and only 2 tanners by that name [P1292-SCA]. Ross: one furrier per 250 persons, one tanner per 2,000 [MDME]. Leather was 13 % of the York freemen of 1350 to 1399, with the leatherworkers [YORK]. Starting share 0.4 %.
- **Races and factions:** Orcs (tanner), Catfolk (tanner, furrier), Brinefolk (tanner). Factions: D2 craft guild.
- **Evidence:** Catasto codes 36 *galigaio*, 78 leather tanner and 27 furrier [CAT]. Manor Lords tannery (leather from hides) [ML-B], Bannerlord tannery [MB2-W], Anno 1404 tannery, fed by pig farms, salt and charcoal [ANNO-1404] [partially verified].

#### 17. Weaver

Makes cloth and cordage from fibre and wool: spins, weaves, fulls, dyes and finishes.

- **Covers:** spinner, weaver, linen weaver, silk weaver, fuller, dyer, shearman, felt maker, carpet weaver, rope maker, net maker.
- **Lore:** the first industry, and the first to split master from hand. Spinning is done in every cottage, mostly by women; weaving is a guild trade; the dyers and fullers are its foul end, the cloth merchants its rich one. Weavers' quarters are crowded, radical and quick to strike. Fantasy twists: cloth that hides its wearer, dyes that only one people can see.
- **Resources:** consumes **fibre** (flax, hemp, cotton, silk, jute), wool (from **livestock**), **dyes**, alum, fuller's earth (**clay**), **fuel**, water. Produces cloth (Clothing), rope, nets and sails (Gear). Second step, between the farmer (1), gardener (2) and herder (3) and the tailor (25) and shipwright (20).
- **Siting:** spinning anywhere with fibre or sheep. Weaving in towns. Fulling and dyeing on a river (`river` 1 or more), with a fulling mill where the stream has fall. Linen in the temperate biomes, wool in Temperate grassland, Cold steppe and Woodland & shrubland, cotton in Savanna, Thorn scrub and Tropical dry forest, silk in a gated zone. A dyer needs a dye in reach, and dyes are gated to a share of the zones.
- **Frequency:** textiles were the largest group of the York freemen: 25 % in 1350 to 1399, with 51 master weavers in one guild [YORK]. Paris: 73 weavers, 24 fullers, 23 tapestry weavers, 20 shearmen, 15 dyers [P1292-SCA]. In the hamlet of Kilpin in 1379, 4 of the 5 named occupations were weavers [HOWDEN]. The Catasto gives the wool trade ten codes of its own [CAT]. Starting share 3.5 %.
- **Races and factions:** Midlanders (weaver, fuller), Rainkeepers (cotton weaver, dyer), Sunlanders (weaver, indigo dyer), Grass riders (felt maker), Boughwalkers (dyer), Shoalfolk (net maker), Elves, Noonfolk, Twofold of the Staying. Factions: D2 craft guild, D1 merchant guild (the clothiers), A6 rebels (the weavers' risings).
- **Evidence:** [YORK]; Catasto codes 24, 25, 43, 45, 46, 53, 60 to 62, 64, 80, 91, 93, 97 and 98 [CAT]. Manor Lords weaver's workshop and dyer's workshop [ML-B], Bannerlord linen weaver and weavery [MB2-W], D&D weaver's tools [DND-T]. Cloth is "the main manufactured trade good" of the game resource set (`game-resources-and-trade.md` §5.2).

### Sector 3. Craft

#### 18. Smith

Works hot metal with hammer and anvil: makes and mends tools, shoes horses, and in a town forges blades, locks, nails and armour.

- **Covers:** blacksmith, farrier, toolsmith, nailer, cutler, locksmith, weaponsmith, armourer, sword polisher, bronze founder and bell caster, forge hand.
- **Lore:** the one craftsman every village must have, and the one most wrapped in tale: fire, iron and a strength that seems more than natural. The smithy is where news is traded. In a city the trade splits into a dozen guilds that quarrel over who may make what. Fantasy twists: the smith who forges under a ban or a blessing, rune blades, a people whose smiths are its priests.
- **Resources:** consumes iron, steel, copper and bronze (from the smelter, 15), charcoal or coal (**fuel**), **timber** (hafts), **stone** (whetstone), leather. Produces Tools, Weapons and Gear (arms and armour, horseshoes, nails, locks). Third step of the metal chain; supplies nearly every other vocation with its tools.
- **Siting:** everywhere people farm or ride: one in a village, many in a town, the armourers in cities and at garrisons. Needs fuel within reach, so thin on Tundra and in the deserts. Thrives beside a smelter (iron and fuel in one place) and on a road (farriery).
- **Frequency:** Paris: 74 *fevres*, 32 farriers, 35 sword polishers, 26 locksmiths, 22 armourers, 21 cutlers, 17 nailers [P1292-SCA]. Ross: one blacksmith per 1,500 persons [MDME], which a critic reckons about six times too few [RAVEN]. Metal trades were 9 % of the York freemen in every period [YORK]. Domesday records six smiths in one city, each paying a penny for his forge [DB-X] [partially verified]. Starting share 1 %.
- **Races and factions:** Dwarves (smith, armourer), Sunlanders (smith, bronze caster), Northmen, Midlanders, Orcs, Hearthbloods (forge hand), Flinchless (striker at a forge), Duskreaders (temper judge at a forge). Factions: D2 craft guild, H3 dwarven hold, C4 artificers' society, E1 mercenary company (the armourer in its train).
- **Evidence:** Catasto codes 30 *fabbro*, 57 *maniscalco*, 76 ironworker, 38 armour maker, 94 sword maker, 85 cutler, 39 keymaker [CAT]. Banished Blacksmith [BAN-P], Manor Lords smithy, blacksmith's workshop and armorer's workshop [ML-B], Bannerlord smithy (iron to weapons and armour) [MB2-W], Medieval Dynasty Blacksmith [MD-P], RimWorld Smith [RW-W], D&D smith's tools [DND-T].

#### 19. Carpenter

Works wood into buildings, vehicles, vessels and tools.

- **Covers:** carpenter, joiner, wheelwright, cartwright, cooper, turner, chest maker, bowyer, fletcher, sled maker, woodcarver.
- **Lore:** the builder of the common world: house frames, carts, barrels, looms and mills are all his. A master carpenter raises a hall with a crew and a rope. Less secret than the smith and less grand than the mason, and found everywhere. The cooper's barrel is the shipping container of the age.
- **Resources:** consumes **timber** and planks, iron nails and Tools (from the smith, 18), glue and pitch. Produces Building (frames, roofs), Tools (carts, barrels, looms, furniture), Weapons (bows, arrows, shields). Second step after the forester (6).
- **Siting:** everywhere with timber within reach; where there is none (Hot desert, Cold desert, Tundra, Temperate grassland away from rivers) wood is imported and the trade is small and dear. Bowyers want yew, ash and elm of the temperate forests. One in a village, a guild in a town.
- **Frequency:** Paris: 88 carpenters, 70 coopers, 29 chest makers, 18 wheelwrights, 12 turners [P1292-SCA]. Ross: one carpenter per 550 persons, one cooper per 700 [MDME]. Building trades were 6 to 7 % of the York freemen [YORK]. Carpenters are named among the few crafts of the Howden villages [HOWDEN]. Starting share 1.5 %.
- **Races and factions:** Midlanders (carpenter), Elves, Grass riders and Amberbloods (bowyer), Amberbloods (woodcarver), Hoarkin (sled maker). Factions: D2 craft guild, D6 frontier settlers' company.
- **Evidence:** Domesday carpenters [DB-HULL]; Catasto codes 40 *legnaiolo* and 48 *bottaio* [CAT]. Manor Lords sawpit, joiner's workshop and fletcher's workshop [ML-B], Bannerlord wood workshop [MB2-W], RimWorld Construct [RW-W], D&D carpenter's tools and woodcarver's tools [DND-T].

#### 20. Shipwright

Builds and repairs boats and ships.

- **Covers:** shipwright, boatbuilder, caulker, hull mender, sail maker, barge builder.
- **Lore:** a ship is the largest machine the age can build, and the yard that builds it is a town's pride and its debt. The craft is by eye and by inheritance, with no drawings. Launches are blessed, and a ship has a name and a luck. Fantasy twists: keels of elder wood, hulls sung into shape.
- **Resources:** consumes **timber** (oak, pine, teak, giant conifers), naval stores (tar and pitch, from the charcoal burner, 14), **fibre** as rope and sailcloth (from the weaver, 17), iron nails. Produces ships and boats. Third step; supplies the sailor (32), the fisher (4) and the merchant (29).
- **Siting:** a `harbour`, a coast with timber behind it, or a navigable river (`river` 8 or more) or a large lake. Best where a forest biome meets the water: Temperate broadleaf forest (oak), Boreal forest (pine, tar), Temperate rain forest, Tropical dry forest (teak). Absent inland away from navigable water. Small boats in any fishing place.
- **Frequency:** rare even in a river city: Paris has 2 ship carpenters, 4 boatbuilders and 2 ship launchers [P1292-SCA]. Starting share 0.2 %, all of it on water.
- **Races and factions:** Northmen (shipwright), Shoalfolk (hull mender, boatbuilder). Factions: D3 merchant league, D4 chartered company, A1 realm crown (the royal yard), F5 pirates.
- **Evidence:** Catasto code 10, the sailor "and ship trades" [CAT]. The D&D proficiency in water vehicles [DND-T]. Naval stores are "a classic boreal export" in the game resource set (`game-resources-and-trade.md` §5.2).

#### 21. Mason

Builds in stone, brick and earth: cuts and lays stone, burns and lays brick, plasters, tiles and thatches, and plans works such as walls, bridges, wells and canals.

- **Covers:** stonemason, bricklayer, builder, plasterer, tiler, roofer, thatcher, steeplejack, well digger, bridge builder, engineer.
- **Lore:** the travelling aristocracy of the crafts. Masons go where the great works are, lodge together on the site, and keep their geometry to themselves. A master mason is architect and contractor in one. Fantasy twists: lodges with real secrets, dwarven stonework that needs no mortar, walls with something bound into the foundation.
- **Resources:** consumes **stone**, bricks and tiles (from the potter, 22), lime (from the lime burner, 14), **clay** (daub, adobe, rammed earth), **timber** (scaffold, centring), **silver & lead** (lead for roofs and pipes), Tools. Produces Building. Third step; the main consumer of stone.
- **Siting:** towns and cities, castles, temples and monasteries, wherever a wall is raised. Stone building where stone is in the catchment or comes by water: Hills and Mountains, and Lowland on Carbonate platform. Brick and adobe on Flat Land and Lowland with clay. Thatch and timber elsewhere, where the mason gives way to the carpenter. Higher Prosperity means more building in stone.
- **Frequency:** Paris: 102 masons, 36 plasterers, 21 roofers [P1292-SCA]. Ross: one mason per 500 persons [MDME]. Masons are named among the few crafts of Howden in 1379 [HOWDEN]. Starting share 1 %.
- **Races and factions:** Dwarves (mason, engineer), Midlanders (mason), Saltroad folk (mud-brick mason, well digger), Canal folk (canal engineer), Duskreaders (wall mason), Boughwalkers (steeplejack, bridge builder), Catfolk (roofer, steeplejack). Factions: D2 craft guild, B1 established church and A1 realm crown (the patrons), H3 dwarven hold.
- **Evidence:** Catasto code 54 *muratore* [CAT]. Banished Builder [BAN-P], Medieval Dynasty Builder [MD-P], RimWorld Construct [RW-W], D&D mason's tools [DND-T], Dwarf Fortress stoneworking [DF-L] [partially verified].

#### 22. Potter

Shapes and fires clay: pots, bowls and jars, bricks and roof tiles.

- **Covers:** potter, brickmaker, tile maker, kiln master, maker of fine ware.
- **Lore:** a humble and ancient trade that leaves more behind than any other. Potters work at the edge of town, where the clay and the fire risk are. A fine ware (a glaze, a white body) can make a town's name and is a guarded recipe.
- **Resources:** consumes **clay** (brick and pottery clay; kaolin for fine ware), **fuel**, lead for glaze (**silver & lead**), water. Produces pottery (Tools), bricks and tiles (Building). Second step after the clay digger (8); supplies the mason (21), the brewer (12), the merchant (29) and every kitchen.
- **Siting:** on clay: Flat Land and Lowland (0.7 and 0.6), more beside a lowland river and on Unconsolidated cover; poor in Hills and Mountains, none on Ice & snow. Needs fuel in reach, so brick is rare in the deserts, where sun-dried adobe takes its place. Kaolin on Crystalline shield and Granite intrusion in warm humid land. Edge of a town.
- **Frequency:** Paris has 49 potters and 12 tile makers [P1292-SCA]. Domesday names potters on one manor only, though archaeology shows a wide industry [DB-X] [partially verified]. Starting share 0.4 %.
- **Races and factions:** Canal folk (potter), Noonfolk (brick maker, potter), Duskreaders (kiln reader). Factions: D2 craft guild.
- **Evidence:** Catasto codes 51 (bowl maker, potter and glassmaker in one code) and 59 kiln maker [CAT]. Manor Lords clay furnace (roof tiles from clay, needs fuel) [ML-B], Bannerlord pottery (clay to pottery) [MB2-W], D&D potter's tools [DND-T].

#### 23. Glassmaker

Melts sand or crystal with a flux and blows, casts or grinds the glass.

- **Covers:** glassblower, glazier, bead maker, mirror maker, lens grinder, vial blower.
- **Lore:** a prized and jealous art. Glass towns forbid their masters to leave, on pain of death in the tale. Window glass is for temples and the rich; a lens is a wonder. The furnaces never go out. Fantasy twists: glass that holds light, a scrying mirror, vials that keep a living flame.
- **Resources:** consumes **crystal** (glass sand, rock crystal), **fuel** in great amounts, a flux (plant ash from the charcoal burner, 14, or natron from the salter, 9), lead (**silver & lead**), **dyes** and metal oxides for colour. Produces glass (Building as panes; Tools and fine goods as vessels, beads, lenses). Second step; like iron it needs two places, crystal and fuel, "so it creates trade" (`game-resources-and-trade.md` §5.2).
- **Siting:** a city, or a forest works, with crystal in reach: glass sand on low coasts and navigable rivers, rock crystal in Hills and Mountains, three times as often on Volcanic arc. Needs a forest biome or coal for fuel. High Prosperity.
- **Frequency:** Paris has 10 glassmakers, 17 *cristaliers* and 4 mirror sellers [P1292-SCA]. Starting share 0.05 %.
- **Races and factions:** Brinefolk (glassmaker), Lanternkin (vial blower), Gnomes (lens grinder). Factions: D2 craft guild, C4 artificers' society, C1 mages' college (as a customer).
- **Evidence:** [P1292-SCA]; Catasto code 51 [CAT]. D&D glassblower's tools [DND-T]. Crystal and Glass were added to the game resource set at the owner's request (`game-resources-and-trade.md` §5.2).

#### 24. Leatherworker

Makes goods of leather: shoes above all, and saddles, harness, belts, gloves, purses, sheaths and bottles.

- **Covers:** shoemaker (cordwainer), cobbler, saddler, harness maker, girdler, glover, purse maker, scabbard maker, bookbinder.
- **Lore:** the most numerous craft in any town, since shoes wear out. The shoemaker in his open stall hears and repeats everything, and is the proverbial town philosopher and radical. Shoemakers of new leather and cobblers of old fight over the line between them.
- **Resources:** consumes leather (from the tanner, 16), thread (**fibre**), wax, iron buckles (from the smith, 18), wood (lasts, saddle trees). Produces Clothing (shoes, gloves, belts) and Gear (saddles, harness, sheaths). Third step of the animal chain.
- **Siting:** every town and city; a cobbler in a large village. Follows people and horses, not land. More where **livestock** is plentiful and leather cheap (Temperate grassland, Cold steppe, Savanna), and where **horses & pack animals** are bred (saddlers).
- **Frequency:** the largest group in Paris: 222 cordwainers, 139 cobblers, 78 belt makers, 51 saddlers, 50 scabbard makers, 39 lorimers, 30 purse makers, 21 glovers [P1292-SCA]. Ross: one shoemaker per 150 persons, the lowest Support Value of all [MDME]. Starting share 1.2 %.
- **Races and factions:** Saltroad folk (leatherworker). Factions: D2 craft guild.
- **Evidence:** Domesday cobblers [DB-HULL]; Catasto codes 29 *calzolaio*, 37 belt maker, 55 saddler, 63 purse maker, 65 slipper maker, 81 leather worker [CAT]. Manor Lords cobbler's workshop (shoes from leather) [ML-B], D&D cobbler's tools and leatherworker's tools [DND-T].

#### 25. Tailor

Cuts and sews cloth and fur into clothes, makes hats and hose, and deals in used clothes.

- **Covers:** tailor, seamstress, robe maker, hosier, hatter, embroiderer, furrier of garments, old-clothes dealer.
- **Lore:** dress is rank made visible, and laws say who may wear what, so the tailor knows everyone's true standing and debts. Most people wear clothes second or third hand; the old-clothes dealer is as common as the tailor. A sedentary trade, mocked as unmanly and feared as gossip.
- **Resources:** consumes cloth (from the weaver, 17), dressed furs (from the tanner, 16), thread, **dyes**, feathers. Produces Clothing. Last step of the textile chain.
- **Siting:** every town and city; most rural clothes are made at home. More and finer in cities and where Prosperity is high. Fur garments in the cold biomes.
- **Frequency:** Paris: 123 tailors, 57 *cousturiers*, 15 robe makers, 47 hatters and 112 old-clothes dealers [P1292-SCA]. Ross: one tailor per 250 persons, one old-clothes dealer per 400 [MDME]. Starting share 1.2 %.
- **Races and factions:** none of the 28 races names the tailor; the Elves and the Rainkeepers name weavers. Factions: D2 craft guild.
- **Evidence:** Domesday robe makers [DB-HULL]; Catasto codes 67 *sarto*, 58 doublet maker, 68 cloak maker, 74 hat maker [CAT]. Banished Tailor [BAN-P], Manor Lords tailor's workshop [ML-B], Medieval Dynasty Seamster [MD-P], RimWorld Tailor [RW-W].

#### 26. Jeweller

Works precious metal, stones and ivory: makes plate, rings and reliquaries, cuts gems, strikes coin, and tests the fineness of metal.

- **Covers:** goldsmith, silversmith, gem cutter, lapidary, engraver, enameller, moneyer, assayer, ivory and coral carver, amber worker.
- **Lore:** the richest of the crafts and the nearest to banking: the goldsmith keeps other people's treasure and strikes the ruler's coin. Trusted with wealth, so bound by the hardest oaths and the longest terms of office. Fantasy twists: gems that hold a spell, coin that cannot be clipped, a people that reads a stone's flaws at a glance.
- **Resources:** consumes **gold**, **silver & lead** (silver), **gems & pearls**, **furs & ivory** (ivory), **copper ore** (as alloy), **crystal** (rock crystal). Produces fine goods and coin. Last step of the precious chain.
- **Siting:** cities and the capital (the mint), courts and great temples. High Prosperity. A mining town with gold, silver or gems keeps cutters and assayers at the source. Coral and pearl carvers on warm coasts, amber workers on cool Clastic basin coasts, ivory carvers where furs & ivory is traded.
- **Frequency:** Paris: 115 goldsmiths, 19 moneyers, 10 lapidaries [P1292-SCA]: a capital with a court. Ross: one jeweller per 400 persons [MDME], which only a capital could reach. The goldsmiths' wardens served three years where other trades served one [LDM]. Starting share 0.15 %.
- **Races and factions:** Gnomes (gem cutter, jeweller, engraver, assayer), Rainkeepers (jeweller), Sunlanders and Hoarkin (ivory carver), Shoalfolk (coral cutter), Boughwalkers (gem grader), Lanternkin (assayer), Orcs (bone carver). Factions: D2 craft guild, D7 banking house, A1 realm crown (the mint).
- **Evidence:** Domesday goldsmiths [DB-HULL]; Catasto codes 49 *orafo* and 88 employee of the mint [CAT]. D&D jeweler's tools [DND-T], Dwarf Fortress jewelling [DF-L] [partially verified]. Fine goods and coin are the top tier of the game resource set (`game-resources-and-trade.md` §5.2).

#### 27. Artist

Makes images and ornaments: paints, carves and gilds for temples, courts and rich houses.

- **Covers:** painter, sculptor, image carver, illuminator, glass painter, temple sculptor, mosaic maker, map engraver, instrument maker.
- **Lore:** a craftsman, not a genius: the painter belongs to a guild, often the saddlers' or the apothecaries', and is paid by the foot. Work is to order, for a patron who sets the subject. Only at a court does an artist gain a name. Fantasy twists: portraits that age for the sitter, statues that watch.
- **Resources:** consumes **dyes** and mineral pigments (from **copper ore**: malachite and azurite; from **gems & pearls**: lapis lazuli), **gold** (leaf), **stone** (marble, soapstone), **timber**, parchment. Produces fine goods and the fittings of Building. Last step.
- **Siting:** cities, the capital, and wherever a great temple or palace is being built. High Prosperity. Sculptors where marble is in reach: Fold belt in Mountains, Carbonate platform in Hills and Mountains.
- **Frequency:** Paris: 32 painters, 23 image makers, 13 illuminators [P1292-SCA]. Ross: one painter per 1,500 persons, one sculptor per 2,000 [MDME]. Starting share 0.1 %.
- **Races and factions:** Rainkeepers (temple sculptor), Boughwalkers (painter of hidden cloth), Gnomes (map engraver, instrument maker), Northmen (rune carver). Factions: D2 craft guild, B1 established church and A1 realm crown (the patrons).
- **Evidence:** Catasto code 95 *dipintore* [CAT]. RimWorld Art [RW-W], D&D painter's supplies and calligrapher's supplies [DND-T], Dwarf Fortress engraving [unverified].

#### 28. Artificer

Makes mechanisms and instruments, and in a world with magic, devices that hold it.

- **Covers:** clockmaker, locksmith of fine locks, instrument maker, lens grinder, tinker, maker of automata, siege engineer, enchanter of objects.
- **Lore:** the fantasy vocation with the thinnest historical root: the makers of clocks, scales and locks, who were few and admired. In a fantasy world the artificer stands between the smith and the mage, and is distrusted by both guilds. Patents and secrets are the trade's whole capital. Typical conflicts: a device that puts a craft out of work, a commission from a ruler that cannot be refused.
- **Resources:** consumes iron, bronze and brass (from the smelter, 15), **crystal** (lenses), **gold** and **silver & lead** (fine parts), **gems & pearls**, **timber**. In a world with magic also the fantasy materials that the catalogue names but the generation has no rule for (star metal; §4.4). Produces Tools and Gear of the highest grade, fine goods. Last step.
- **Siting:** the capital and a few cities, at a court, a college or a rich guild. High Prosperity; with enchanted work also high Supernatural. In a dwarven or gnomish hold in Hills and Mountains.
- **Frequency:** Paris has 2 scale makers beside its 26 locksmiths [P1292-SCA]. Ross's "magic shop", one per 2,800 persons, is his own addition to the Paris list [MDME]. Starting share 0.02 %: one workshop in a region, more in a rich one.
- **Races and factions:** Gnomes (clockmaker, locksmith, lens grinder, instrument maker, tinker), Dwarves (engineer). Factions: C4 artificers' society, C1 mages' college, A1 realm crown (siege works).
- **Evidence:** D&D tinker's tools [DND-T]; the artificers' society of `factions.md` C4 and its sources. No historical primary source covers the enchanted part.

### Sector 4. Trade and transport

#### 29. Merchant

Buys in one place and sells in another, in bulk and over distance, and lends, changes and insures money.

- **Covers:** long-distance merchant, wholesaler, draper, mercer, spicer, grain factor, broker, moneychanger, banker, moneylender, insurer.
- **Lore:** the estate that fits no estate: neither fights, prays nor ploughs, and grows richer than those who do. Merchants rule the towns through their guild and buy their way into the nobility within three generations. Usury is a sin they pay the temple to forgive. Fantasy twists: houses that trade between peoples who cannot meet, contracts witnessed by something that enforces them.
- **Resources:** consumes nothing; moves everything. Lives on the high-value resources of §14.5: **gold**, **tin**, **silver & lead**, **gems & pearls**, **salt**, **spices**, **copper ore**, **iron ore**, **crystal**, **stimulants & sugar**, **furs & ivory**, **dyes**, and on bulk **grain**, cloth and wine. Uses coin, ships (32) and carts (31).
- **Siting:** by the trade factor of a Prospect (`map-generation.md` §15.3): a `harbour`, a river `mouth`, the `head` of navigation, a `confluence`, a central place. Cities and the capital; a factor in a town. More where Prosperity is high and where the catchment has a resource with a trade value.
- **Frequency:** mercantile trades were 14 % of the York freemen of 1350 to 1449 [YORK]. Paris: 63 mercers, 44 "Lombards" (moneylenders), 28 spicers, 23 brokers, 19 drapers, 16 moneychangers [P1292-SCA]. The poll tax of 1379 grades the merchants one class at a time [POOS]. Starting share 0.5 %.
- **Races and factions:** Lanternkin (banker, moneylender, insurer), Saltroad folk (merchant banker), Rainkeepers (sea merchant), Sunlanders (river trader), Northmen (trader), Midlanders, Canal folk, Dwarves (gate merchant), Gnomes (moneychanger), Noonfolk (grain factor), Twofold of the Going (caravan trader). Factions: D1 merchant guild, D3 merchant league, D4 chartered company, D7 banking house, A3 free city council, F3 black network.
- **Evidence:** the merchant guilds [GUILD]; Domesday merchants and corn-dealers [DB-HULL]; Catasto codes 22 *mercatante*, 23 *cambiatore*, 24 *lanaio*, 79 broker [CAT]. Banished Trader [BAN-P], Manor Lords trading post [ML-B].

#### 30. Shopkeeper

Sells small quantities to those who live near: from a stall, a shop or a pack.

- **Covers:** retailer, stallholder, huckster, pedlar, chapman, market trader, seller of oil, hay, fruit, cheese or firewood.
- **Lore:** the small end of trade, mostly poor and often a woman. Hucksters buy at dawn and sell dearer by noon, and are fined for it every week. The pedlar carries needles, ribbons and news to places no merchant visits, and is welcome and suspected in equal parts.
- **Resources:** consumes nothing; retails food (**grain**, **produce**, **orchard**, **fish**), **fuel**, Clothing and Tools.
- **Siting:** a market: every town and city. Pedlars walk the countryside between them, along the travel network. Follows population.
- **Frequency:** Paris: 74 *regratiers* (food resellers), 39 oil sellers, 20 hay sellers, 17 cheese sellers, 11 fruit sellers [P1292-SCA]. Ross: one wine seller per 900 persons, one hay merchant per 2,300 [MDME]. Starting share 1.5 %.
- **Races and factions:** Halflings (peddler), Swiftborn (market trader), Duskreaders (trader of the midnight market), Twofold of the Going (pedlar), Hoarkin (keeper of an ice cellar). Factions: D1 merchant guild (at its edge), F1 thieves' guild (the fence).
- **Evidence:** Catasto codes 52 *merciaio* (peddler), 31 second-hand dealer, 35 oil merchant [CAT]. Banished Vendor [BAN-P], Medieval Dynasty Stallholder and Vendor [MD-P], Manor Lords marketplace stalls [ML-B].

#### 31. Carter

Moves goods and messages by land: with cart, pack animal or his own back.

- **Covers:** carter, teamster, muleteer, caravan driver, caravan master, drover, porter, water carrier, courier, post rider, elephant driver.
- **Lore:** rough men of the road who know every inn, toll and ford. Caravan masters are trusted with fortunes; porters are the poorest free men in a town. The road has its own law, and its own tolls, bandits and guides. Fantasy twists: beasts of burden no stable could hold, roads that are only open at certain hours.
- **Resources:** consumes **horses & pack animals** (horses, mules, donkeys, camels, llamas), oats (**grain**), carts (from the carpenter, 19), harness (from the leatherworker, 24). Produces transport.
- **Siting:** on the roads of the travel network (`map-generation.md` §16), most at its nodes: towns and cities, river heads and harbours, where goods change from water to land. Carts on Flat Land and Lowland; pack animals in Hills and Mountains (mules) and in the deserts (camels); porters where `mounts` is 0: Tropical rain forest, Mangrove, Ice & snow.
- **Frequency:** Paris: 46 carters, 45 unloaders, 42 porters and 58 water carriers, 13 messengers [P1292-SCA]. Ross: one water carrier per 850 persons [MDME]. Starting share 1.2 %.
- **Races and factions:** Saltroad folk (caravan master), Grass riders (cart driver, post rider), Rainkeepers (elephant driver), Brinefolk (salt carrier), Swiftborn (runner, courier), Hoarkin and Hearthbloods (winter courier), Noonfolk (day courier), Boughwalkers (runner of the high roads), Flinchless (courier of secrets), Orcs (porter), Catfolk, Twofold. Factions: D8 caravan company, D1 merchant guild, F6 smugglers' ring.
- **Evidence:** Catasto codes 20 *vetturale* and 86 *mulattiere* [CAT]. Manor Lords pack station (mules) and hitching post [ML-B], Medieval Dynasty Waggoner [MD-P], RimWorld Haul [RW-W], Crusader Kings III Caravan Master [CK3-C], the D&D proficiency in land vehicles [DND-T].

#### 32. Sailor

Works a vessel: on the sea, on a river, on a lake or a canal.

- **Covers:** seaman, ship's master, pilot, rigger, bargeman, boatman, ferryman, raftsman, ship's lookout.
- **Lore:** a people apart, with their own speech, pay by share and long absences. Half of every port is waiting for a ship. Pilots are licensed and guard their knowledge of the bar and the channel. The ferryman owns his crossing by right and is paid by all. Fantasy twists: winds that are bought, a people that sees the reef through the water.
- **Resources:** consumes ships and boats (from the shipwright, 20), rope and sailcloth (from the weaver, 17), preserved food. Produces transport: water carriage is the cheap half of trade (river edges cost 0.4 of land in `map-generation.md` §15.2).
- **Siting:** a `harbour` or coast for seamen; a navigable river (`river` 8 or more), a large lake or a river `mouth` for boatmen; a ferry at any river crossing on the travel network. Absent in a landlocked region without a navigable river.
- **Frequency:** Paris, a river port: 29 ferrymen, 12 mariners, 2 ship launchers [P1292-SCA]. Starting share 1.2 %; a fifth of the workers of a harbour city.
- **Races and factions:** Northmen (sailor, pilot), Shoalfolk (pilot, ferryman), Halflings (bargeman), Canal folk (boatman), Hoarkin (ice pilot), Widegaze (ship's lookout), Boughwalkers and Catfolk (rigger, topman), Twofold of the Going. Never the Noonfolk. Factions: D3 merchant league, D4 chartered company, F5 pirates, F6 smugglers' ring.
- **Evidence:** Domesday boatmen and sailors [DB-HULL]; Catasto code 10 *marinaio* [CAT]. D&D navigator's tools [DND-T]. The Pathfinder Sailor background [unverified].

#### 33. Innkeeper

Keeps a house where strangers eat, drink, sleep, wash and stable their animals.

- **Covers:** innkeeper, taverner, hosteller, alehouse keeper, cookshop keeper, bathhouse keeper, stable keeper.
- **Lore:** the host knows who came, with what and with whom, and is by turns informer, banker, broker and fence. An inn is where a stranger must lodge by law, and where a deal is witnessed. The tavern is the common room of the poor; the bathhouse has a worse name.
- **Resources:** consumes drink (from the brewer, 12), bread and meat, **fuel**, hay and oats (**grain**), **stimulants & sugar**. Produces lodging.
- **Siting:** on the travel network: towns and cities, harbours, fords and passes, and a day's journey apart along a road. A tavern in every town; a true inn only where travellers pass.
- **Frequency:** Paris: 86 taverners, 12 innkeepers, 21 bathhouse keepers [P1292-SCA]. Ross: one tavern per 400 persons, one inn per 2,000 [MDME]. The river village of Cawood had seven hostellers in 1379 [HOWDEN]. Starting share 0.8 %.
- **Races and factions:** Halflings (innkeeper). Factions: D8 caravan company (its stations), B8 pilgrimage shrine keepers, F1 thieves' guild and F6 smugglers' ring (the safe house).
- **Evidence:** Catasto codes 34 *albergatore* and 33 *vinattiere* [CAT]. Manor Lords tavern [ML-B], Medieval Dynasty Innkeeper [MD-P].

### Sector 5. Service

#### 34. Healer

Treats the sick, the wounded and women in childbed: with herbs, the knife, the hand and advice.

- **Covers:** physician, surgeon, barber-surgeon, barber, bonesetter, tooth puller, midwife, nurse, herbalist.
- **Lore:** three ranks that despise each other. The learned physician reads and prescribes and never cuts. The barber-surgeon cuts, bleeds and shaves. The village healer and the midwife know herbs and are one accusation away from being called witches. Plague makes and breaks them all. Fantasy twists: healing that works, and its price.
- **Resources:** consumes herbs (from **wild food & game**: wild herbs; and from **spices**: the resins and barks), honey (**stimulants & sugar**), drink, Tools (knives). Produces care. §4.4 proposes a herb resource.
- **Siting:** a midwife and a herb woman in every village, a barber in every town, a physician only in a city or at a court. Monasteries keep an infirmary. Herb gatherers in Woodland & shrubland and the forest biomes.
- **Frequency:** Paris: 150 barbers and 29 *mires* (doctors) [P1292-SCA]. Ross: one licensed doctor per 1,700 persons, one healer of any kind per 350, one barber per 350 [MDME]. Starting share 0.5 %.
- **Races and factions:** Flinchless (surgeon, bonesetter, tooth puller, anatomist, nurse), Elves (herbalist, healer), Amberbloods (wound healer, herbalist), Saltroad folk, Rainkeepers and Canal folk (physician), Duskreaders (fever reader, midwife), Stillfaces (diagnostician), Bitterbloods (physician of fevers), Hearthbloods (warming nurse), Swiftborn and Twofold (midwife). Factions: G3 healers' order, B2 monastic order, C1 mages' college.
- **Evidence:** the monastic infirmarian, who also bleeds the monks [OBED]; Catasto codes 32 *maestro* (doctor), 44 *barbiere*, 26 *medico e speziale* [CAT]. Banished Physician and Herbalist [BAN-P], Medieval Dynasty Herbalist and Barber [MD-P], RimWorld Doctor [RW-W], Crusader Kings III Personal Physician [CK3-C], the D&D herbalism kit [DND-T].

#### 35. Servant

Works in another's household for board, lodging and a small wage: in the house, the kitchen, the stable and the yard.

- **Covers:** maid, manservant, cook, groom, nurse, wet nurse, laundress, valet, page, taster, household steward. A slave who does this work has the same vocation and another status.
- **Lore:** service is a stage of life as much as a class: the young of the poor, and of the not so poor, serve in another house until they marry. A great household is a small court with its own ranks. The servant sees everything and is seen by no one, which makes the vocation the natural spy.
- **Resources:** consumes nothing of its own; works the household's food, **fuel** and water.
- **Siting:** every household of standing 4 and up. Many more in towns than in the country, and most at courts, in cities and where Prosperity is high.
- **Frequency:** in the town of Howden 22.4 % of the taxpayers of 1379 were servants; a high share of servants is one of the four signs of a town [HOWDEN]. Maidservants are the third largest entry of the Paris list as Gies gives it (199) [GIES-BC], and Ross gives one per 250 persons [MDME]. Domesday counts 28,000 slaves, about a tenth of those recorded [DB]. Starting share 7.9 %: about 6 % in the country, 20 % in a town.
- **Races and factions:** Halflings (taster), Stillfaces (poison finder at a court), Boughwalkers (food taster), Bitterbloods (taster at a court). The readme's family "can include servants or slaves". Factions: A1 realm crown and A2 noble house (the household), B1 established church.
- **Evidence:** the men and women "servants of the court" who obey the reeve [SEN]; Catasto codes 11 and 12, servant of a private family and of a church [CAT]; Domesday cooks, dairy maids and washerwomen [DB-HULL]. Crusader Kings III court positions such as Wet Nurse and Seneschal [CK3-C], RimWorld Clean and Childcare [RW-W].

#### 36. Entertainer

Performs for pay: sings, plays, tells, acts, tumbles, dances or wrestles.

- **Covers:** minstrel, bard, harper, singer, drummer, praise-singer, storyteller, poet, actor, acrobat, rope dancer, dancer, jester, wrestler. The courtesan is a vocation of its own (51).
- **Lore:** masterless and on the road, so classed with vagabonds; yet the same person at a court is a poet with a pension. Entertainers carry news and reputations between courts, and a mocking song can do a lord more harm than a raid. Fantasy twists: songs that bind, a people that lives a whole career in seven years.
- **Resources:** consumes instruments (from the carpenter, 19, and the artist, 27), fine Clothing. Produces nothing that can be stored.
- **Siting:** courts, cities, fairs and inns; travelling along the travel network between them. High Prosperity.
- **Frequency:** few who pay tax: Paris has 3 jongleurs, 3 trumpeters, 2 drummers, 2 flute players, a minstrel and a dancer [P1292-SCA]. Starting share 0.2 %.
- **Races and factions:** Swiftborn (player, singer, dancer, acrobat), Sunlanders (praise-singer, drummer), Elves (harper), Northmen (poet), Rainkeepers (dancer), Grass riders and Hearthbloods (wrestler), Catfolk (rope dancer). Factions: A1 realm crown (the court), G1 benevolent secret society (as its eyes).
- **Evidence:** Domesday harpers and jesters [DB-HULL]; Catasto code 83, musician and jester [CAT]. D&D musical instruments [DND-T], the fiddler, drummer and oud player of Medieval Dynasty's game data [MD-P], Crusader Kings III Court Chronicler [CK3-C].

#### 51. Courtesan

Sells company and the bed: from the woman or man of a town's public house to the kept companion of a court, who is paid as much for wit, music and discretion as for anything else.

- **Covers:** courtesan, companion, kept mistress, prostitute, bathhouse attendant, brothel keeper. Added on 2026-11-15 at the user's request. Until then the courtesan was a variant of Entertainer (36). The numbers 1 to 50 are unchanged, so the entry stands in its sector out of number order.
- **Lore:** tolerated, taxed and despised at once. A town licenses a public house or a street and makes the trade wear a mark; a preacher closes the house and the next council opens it again. At the top the courtesan keeps a salon, reads, plays and is painted, and hears what a minister says when he is off guard, so the vocation is a natural keeper of secrets, beside the servant. Most enter through poverty or are sold into it, and few leave rich. The conflicts are with the church, with the wives of the town and with the keeper who holds the debts. Fantasy twists: a people that changes sex in mid-life, a charm that is more than charm.
- **Resources:** consumes Clothing and fine goods, **dyes**, perfume (from **spices**: the incense and the resins), drink. Produces nothing that can be stored. The end of the luxury chains, with the noble (37).
- **Siting:** towns and up: the bathhouse, the inn, the harbour, the garrison and the fair. The courtesan proper only in a city, in the capital or at a court, and where Prosperity is high. Absent from the countryside. More where sailors, soldiers and merchants pass: harbours, the heads of river navigation and the crossings of the travel network.
- **Frequency:** no share found. Public prostitution was tolerated and condoned by clerics and municipal officials alike, and Rossiaud's sample for Dijon is 130 cases between 1440 and 1540 [ROSSIAUD] [partially verified]. The stewholders of Southwark worked under ordinances of the Bishop of Winchester that were said to date from 1161 or 1162, with a traditional count of eighteen licensed houses, until the stews were closed in 1546 [STEWS] [partially verified]. A catalogue printed in Venice about 1565 lists 212 courtesans with their prices [VEN-CAT] [partially verified]. Starting share 0.1 %, taken from the servants (35), so the shares of the sectors do not move: none in the country, about 1 % of the labour force of a city.
- **Races and factions:** no entry of `races-basis.md` names the work. Factions: F1 thieves' guild (which keeps the houses), A3 free city council (which licenses them), A1 realm crown (the court).
- **Evidence:** [ROSSIAUD], [STEWS] and [VEN-CAT], all three read through search excerpts only. No game of the sample has the vocation as a profession [unverified]. The vocation does not pass the test of §1 (a historical source and at least two games); it is on the list by the user's decision.

### Sector 6. Governance and war

#### 37. Noble

Holds land and people by right of birth or grant: rules, judges, leads in war and lives from rents and dues.

- **Covers:** lord, lady, knight, squire, landholder, chief, jarl, khan, title-holder, head of a great house.
- **Lore:** those who fight, in the old division of society into those who pray, fight and work [unverified]. Honour, lineage and land are the whole of life, and each is defended with the sword and the lawsuit. A noble who works with his hands loses the rank. Younger sons go to the temple, to war or to the bad. Fantasy twists: houses of another race, bloodlines with a gift or a curse.
- **Resources:** consumes the surplus of everyone on the land: **grain**, **livestock**, **horses & pack animals** (war horses), Weapons, fine goods, **furs & ivory**, **spices**. Produces rule and protection, or the claim to them.
- **Siting:** a seat on good farmland or at a strong point: Hills at a river crossing or a pass, a town or city Prospect. A manor per group of villages. More, and poorer, on a region `border`.
- **Frequency:** about 1 to 2 % of the population of western Europe was noble, and near 10 % in Castile and Poland [NOB] [partially verified: search excerpts]. Ross: one noble household per 200 persons [MDME]. About 190 lay tenants-in-chief held the lay half of England in 1086 [DB-LIFE]. Starting share 1 %.
- **Races and factions:** Midlanders (knight), Sunlanders (title-holder), Orcs (chief, hearth-mother), Twofold (head of a house). Factions: A1 realm crown, A2 noble house, E2 secular knightly order, A4 league of rulers, H1 nomad horde (the khan and his kin).
- **Evidence:** the ranks that fill most of the 1379 poll-tax schedule [POOS]; Catasto codes 08 *messere* and 89 rural noble [CAT]; 12 squires in Paris [P1292-SCA]. The D&D Noble background [DND-B].
- **Note:** noble is as much a status as a vocation. §7.3 treats the difference.

#### 38. Official

Carries out the orders of a ruler, a lord, a town or a temple: collects, counts, judges, inspects and announces.

- **Covers:** steward, bailiff, reeve, hayward, magistrate, judge, tax collector, toll keeper, customs searcher, weigh-master, sworn measurer, surveyor, mayor, herald, town crier, envoy, treasurer, gaoler.
- **Lore:** the reeve was "the pivot man of the manorial system" [REEVE]: a peasant, chosen each year, who answered with his own goods for the lord's harvest. Above him the bailiff and the steward ride between manors and audit the rolls. Officials are hated for the tax and needed for the peace, and every one of them is assumed to take a cut. Fantasy twists: an office that passes by examination, a judge who can tell a lie when he hears one.
- **Resources:** consumes parchment and ink (from the scribe, 42), coin. Produces order: the taxes, tolls and judgements that the rest of the model runs on.
- **Siting:** a reeve and a hayward in the countryside; a bailiff, toll keeper and market officers in a town; magistrates and the treasury in a city; the high officers in the capital. Toll keepers at the fords, passes and harbours of the travel network.
- **Frequency:** no share found. Paris: 43 criers, about 30 sworn measurers and gaugers of wheat, wood, salt, cloth and wine, 3 provosts [P1292-SCA]. Starting share 0.8 %.
- **Races and factions:** Canal folk (magistrate, examiner, salt commissioner), Midlanders (bailiff), Saltroad folk (judge), Rainkeepers (reservoir keeper), Grass riders (tax collector), Northmen (law-speaker), Widegaze (surveyor), Brinefolk (weigh-master), Stillfaces (customs searcher, grader of wine, tea and spice), Amberbloods (timber judge), Hearthbloods (larder-lord), Noonfolk (well keeper, speaker of the noon council), Elves and Lanternkin (envoy), Swiftborn (magistrate for a season), Twofold of the Turning (judge, go-between). Factions: A1 realm crown, A2 noble house, A3 free city council, B4 inquisition.
- **Evidence:** the steward, bailiff, provost, reeve, hayward, parker, granger, auditor and sworn surveyors of the estate treatises [SEN]; the yearly choice of the reeve [REEVE]; Catasto codes 13 employee of the commune, 66 weigher, 69 town crier [CAT]. Crusader Kings III Seneschal [CK3-C], Medieval Dynasty Castellan and Herald [MD-P].

#### 39. Soldier

Fights for pay, for a lord or for an oath: in the field, on the wall and at sea.

- **Covers:** man-at-arms, archer, crossbowman, spearman, mounted archer, cavalryman, sergeant, captain, mercenary, drill master, marine, champion.
- **Lore:** a trade like another, hired by the season and paid late. In peace the soldier is a guard, a bully or a bandit; in war a looter. Companies have their own law, share out plunder by rank and sell themselves to the higher bidder. Veterans come home with scars, a little money and no craft. Fantasy twists: a people that feels no pain, companies of another race for hire.
- **Resources:** consumes Weapons and Gear (from the smith, 18, and the carpenter, 19), **horses & pack animals**, **grain**, preserved food, coin. Produces force.
- **Siting:** garrisons at castles, cities, region `border` patches, passes and harbours; mercenaries in cities between contracts. Cavalry where horses are bred: Temperate grassland and Cold steppe. Low Prosperity and a border raise the share.
- **Frequency:** no share found. Paris has 10 champions and 5 crossbow makers but no by-name for the soldier [P1292-SCA]. Starting share 1 %, several times that on a border and in a horde.
- **Races and factions:** Flinchless (soldier, shield bearer, drill master), Orcs (warrior, soldier for hire), Dwarves (heavy foot soldier for hire), Grass riders (mounted archer), Midlanders (man-at-arms), Sunlanders (cavalryman), Elves and Amberbloods (archer), Canal folk (wall soldier), Swiftborn, Twofold of the Going. Factions: E1 mercenary company, E2 knightly order, B3 holy order of knights, E3 fighters' and adventurers' guild, E4 wardens, A1 realm crown, H1 nomad horde, A6 rebels.
- **Evidence:** Domesday soldiers [DB-HULL]; Catasto code 14 soldier [CAT]. The D&D Soldier background with its specialities: officer, scout, infantry, cavalry, healer, quartermaster, standard bearer, support staff [DND-B]. A mercenary is the D&D example of a skilled hireling [DND-T]. Crusader Kings III Personal Champion [CK3-C].

#### 40. Guard

Keeps watch and order in one place: a gate, a wall, a road, a store, a person.

- **Covers:** watchman, town guard, gate guard, night watch, bodyguard, caravan guard, road warden, pass warden, lookout, beacon keeper, granary warden, vault keeper, firefighter.
- **Lore:** the watch is a duty of every householder that the rich pay others to do, so the guard is a poor man with a lantern and a staff. He knows every face in his ward and can be bought for a drink. A bodyguard or a vault keeper is another matter: chosen for loyalty and paid for it.
- **Resources:** consumes Weapons and Gear, **fuel** (beacons, lamps). Produces safety for the trade and stores of the rest.
- **Siting:** every town and city (gates, walls, markets, harbours), toll points and passes on the travel network, mines and treasuries, caravans. Lookouts on coasts and on open land with a long view: Temperate grassland, Cold steppe, Hills.
- **Frequency:** Ross: one law officer per 150 citizens in a well-kept city, half that in a slack one [MDME]. Starting share 0.6 %.
- **Races and factions:** Widegaze (lookout, signaller, beacon keeper, sentry, road warden), Catfolk (granary warden, night watch, herd guard), Hearthbloods (pass warden, avalanche rescuer), Dwarves (tunnel warden, vault keeper), Duskreaders (night watch), Flinchless (bodyguard, firefighter), Northmen (sworn guard abroad), Halflings (slinger of the watch), Grass riders and Orcs (caravan guard), Shoalfolk (reef warden), Amberbloods (fire-watcher). Never the Noonfolk as sentries. Factions: E5 town watch and militia, E4 wardens, D8 caravan company, A3 free city council.
- **Evidence:** [MDME]. Crusader Kings III Bodyguard [CK3-C]. The Pathfinder Guard background [unverified]. The town watch of `factions.md` E5.

### Sector 7. Faith and learning

#### 41. Priest

Serves a god or the spirits for a community: leads the rites, marks birth, marriage and death, teaches, and keeps the holy place. Includes those who live under a rule apart from the world.

- **Covers:** village priest, temple priest, chaplain, shaman, monk, nun, friar, hermit, ascetic, sexton, and the officers of a monastery.
- **Lore:** those who pray. The village priest is a farmer's son with a little learning and a field of his own; the high priest is a lord. A monastery is a great estate with a rule: the cellarer feeds it, the sacrist keeps the church, the infirmarian nurses, the precentor leads the singing, the guest-master receives travellers [OBED]. The clergy are judged by their own courts and pay no lay tax. Fantasy twists: gods that answer, rites that hold something back.
- **Resources:** consumes beeswax and incense (**stimulants & sugar**, **spices**), wine (**orchard**), parchment, fine goods, **stone** (the temple). Lives on a tenth of the harvest or on its own land, so in effect on **grain** and **livestock**. Produces the rites, alms, schooling and, in a monastery, drink, books and care.
- **Siting:** a priest wherever people live: a shrine in the countryside, a temple in a town, a great temple and its chapter in a city. Monasteries apart: remote land, often poor, on a river. Shamans among herders and hunters. More where Supernatural is high.
- **Frequency:** Ross: one clergyman per 40 persons, and one priest per 25 to 30 clergy [MDME]. The clerical poll taxes of 1377 to 1381 show a very numerous clergy, most without a benefice [CLERGY] [partially verified]. No primary figure was verified, so the starting share is set lower than Ross: 2.5 % of the labour force, one in about 90 persons.
- **Races and factions:** Midlanders (priest, monk), Rainkeepers (priest, ascetic), Grass riders (shaman), Twofold (priest of the Pair), Catfolk (priest of the Small Huntress). Factions: B1 established church, B2 monastic order, B3 holy order of knights, B6 popular sect, B8 pilgrimage shrine keepers, B5 forbidden cult.
- **Evidence:** the obedientiaries [OBED]; Catasto codes 70 priest, 71 religious, 82 religious woman living at home [CAT]; only 11 priests pay the Paris taille [P1292-SCA]; the clergy were largely outside the roll [unverified]. Banished Cleric [BAN-P], Manor Lords church [ML-B], the D&D Acolyte background [DND-B], Crusader Kings III High Almoner [CK3-C].

#### 42. Scribe

Writes for others: letters, deeds, accounts, court rolls and copies of books; and knows the forms that make a writing valid.

- **Covers:** clerk, copyist, notary, bookkeeper, secretary, advocate, records clerk, printer, bookbinder, bookseller, witness for hire.
- **Lore:** literacy is a trade. The clerk is often in minor orders and half a priest; the notary's sign makes a contract stand in any court; the advocate is rich and unloved. Scribes serve whoever pays, know every secret that was ever put in writing, and can forge it. Fantasy twists: a people whose word is physically binding, script that only some eyes can read.
- **Resources:** consumes parchment (from the tanner, 16), paper (from **fibre** rags), ink (oak galls, **dyes**), beeswax for seals (**stimulants & sugar**), quills. Produces documents and books.
- **Siting:** towns (a clerk of the court, a notary), cities and the capital (chancery, courts of law, merchants' counting houses, colleges), monasteries. High Prosperity and trade raise the share.
- **Frequency:** Paris: 53 clerks, 24 scriveners, 12 notaries, 17 bookbinders, 8 booksellers [P1292-SCA]. Ross: one copyist per 2,000 persons, one bookseller per 6,300, one advocate per 650 [MDME]. Starting share 0.4 %.
- **Races and factions:** Lanternkin (notary, clerk, bookkeeper), Canal folk (clerk, printer), Midlanders (clerk), Saltroad folk (scribe), Dwarves (records clerk), Boughwalkers (marker of seals and contracts), Widegaze (witness for hire), Brinefolk (witness to a salt oath). Factions: A1 realm crown (the chancery), D1 merchant guild, D7 banking house, B1 established church, C1 mages' college.
- **Evidence:** Catasto codes 18 *scriba*, 21 judge and notary, 56 stationer, 99 lawyer [CAT]. A scribe is a D&D example of a skilled hireling, and the scribe is one of the Sage's specialities [DND-T] [DND-B]. D&D calligrapher's supplies and forgery kit [DND-T].

#### 43. Scholar

Studies and teaches: letters, law, the stars, numbers, the past.

- **Covers:** teacher, schoolmaster, tutor, professor, sage, astronomer, mathematician, lore keeper, chronicler, librarian, keeper of genealogies.
- **Lore:** learning is slow, costly and mostly in the hands of the temple. A school teaches boys their letters; a college of masters and students is a rare and privileged body that answers to no town court. Scholars are poor until a patron finds them. Fantasy twists: lore older than the kingdoms, an examination that makes or breaks a house.
- **Resources:** consumes books, parchment and paper (from the scribe, 42), instruments (from the artificer, 28). Produces teaching and advice.
- **Siting:** a schoolmaster in a town; scholars in a city with a great temple, a court or a college; a tutor in a noble house. High Prosperity.
- **Frequency:** Paris, a university city, has 11 schoolmasters who pay tax [P1292-SCA]; the masters and students of the university were clerks and exempt [unverified]. Ross: one university per 27.3 million persons [MDME], which a critic rejects as far too few [RAVEN]. Starting share 0.15 %.
- **Races and factions:** Saltroad folk (scholar, astronomer), Rainkeepers (mathematician), Canal folk (tutor, examiner), Elves (lore keeper, tutor at a human court), Halflings (keeper of genealogies), Gnomes (riddling tutor), Swiftborn (chronicler), Noonfolk (keeper of the sundial), Widegaze (star watcher), Twofold (teacher), Orcs (the greybacks, who hold the law and the lore). Factions: G2 scholars' and explorers' society, C1 mages' college, B2 monastic order, A1 realm crown (the tutor and the chronicler).
- **Evidence:** Catasto code 77, teacher of letters or abacus, and student [CAT]. Banished Teacher [BAN-P], RimWorld Research [RW-W], Crusader Kings III Tutor, Antiquarian and Court Chronicler [CK3-C], the D&D Sage background with its specialities: astronomer, librarian, professor, researcher [DND-B].

### Sector 8. Supernatural

These three have no medieval census behind them. Their numbers are a design choice. The games and the tabletop rules are the evidence, and the Supernatural Field is the natural driver.

#### 44. Mage

Works magic by learning or by gift: casts, wards, enchants and advises.

- **Covers:** wizard, sorcerer, enchanter, court mage, battle mage, hedge wizard, rune carver.
- **Lore:** the learned mage is a scholar with a dangerous subject: trained for years in a college or by a single master, bound by the college's oath, and watched by temple and crown. The born mage is a child that a village fears and a college claims. Mages serve courts as advisers and weapons, and are the first blamed when a harvest fails. Typical conflicts: college against cabal, mage against witch hunter (`factions.md` §4.1).
- **Resources:** consumes books and instruments, **crystal** (foci), **gems & pearls**, **gold** and **silver & lead** (silver), the reagents and fantasy materials of §4.4. Produces spells and enchanted goods, at whatever rate the game allows.
- **Siting:** the capital and a few cities (college, court); a tower apart. Where the Supernatural Field is high. High Prosperity for the college, none needed for the tower.
- **Frequency:** a design choice. Ross adds a "magic shop" to the Paris list at one per 2,800 persons [MDME]. Starting share 0.03 %: about five in a median region.
- **Races and factions:** Northmen (rune carver), Elves and Amberbloods (who ask for a high Supernatural Field in `races-basis.md`). Factions: C1 mages' college, C2 secret cabal of mages, C3 necromancer cabal, A1 realm crown (the court mage), B4 inquisition (as the hunted).
- **Evidence:** the wizard's apprentice among the D&D Sage's specialities [DND-B]; RimWorld Dark study, an expansion's work type for dealing with anomalies [RW-W]; the arcane factions of `factions.md` Group C.

#### 45. Alchemist

Prepares substances: medicines, poisons, perfumes, acids, pigments and, by repute, gold.

- **Covers:** apothecary, alchemist, potion maker, antidote maker, poisoner, perfumer, embalmer, maker of insect smoke.
- **Lore:** the respectable end is the apothecary, a guild shopkeeper who sells spices, drugs and sweets alike. The other end works at night with a furnace and a patron's money, promising gold. Both are licensed, taxed and suspected. An alchemist's shop is where a healer buys, a mage orders and a murderer asks no questions. Fantasy twists: potions that work, a people immune to their own poisons.
- **Resources:** consumes **spices** (resins, barks, incense), herbs (**wild food & game**), honey and sugar (**stimulants & sugar**), **salt** (natron), **fuel** (oil and naphtha seeps), **silver & lead**, **copper ore**, glass vessels (from the glassmaker, 23), and the mineral reagents that the catalogue lacks: alum, sulphur, saltpetre, quicksilver (§4.4). Produces medicine, poisons, perfumes, pigments.
- **Siting:** cities and the capital, a court, a college, a large monastery; the apothecary also in a rich town. Where spices are grown or traded: a `harbour`, the tropical biomes. Volcanic arc for the mineral reagents. High Supernatural if the potions are to work.
- **Frequency:** Paris has 28 *espiciers*, the spice sellers who were also the apothecaries [P1292-SCA]. Ross: one spice merchant per 1,400 persons [MDME]. Starting share 0.05 %.
- **Races and factions:** Bitterbloods (apothecary, antidote maker, keeper of snakes and frogs, maker of arrow and fish poison, maker of insect smoke), Stillfaces (perfumer, composer of scents), Brinefolk (embalmer). Factions: C1 mages' college, G3 healers' order, F2 assassins' brotherhood, C3 necromancer cabal, E6 monster hunters.
- **Evidence:** Catasto code 26 *medico e speziale* [CAT]. D&D alchemist's supplies and poisoner's kit, and the alchemist among the Sage's specialities [DND-T] [DND-B]. RimWorld drug synthesis under Craft [RW-W].

#### 46. Seer

Knows what others do not: reads signs, finds water and lost things, speaks with spirits, lifts and lays curses.

- **Covers:** diviner, astrologer, geomancer, soothsayer, oracle, medium, dowser, well-finder, weather watcher, wise woman, hedge witch, cunning man.
- **Lore:** the village's own magic, cheap, unlicensed and older than the temple. The seer is consulted for a lost cow, a sick child, a good day to sow, and is the first accused when things go wrong. At a court the same art is the astrologer's, well paid and politically deadly. The line between seer, healer and priest is thin, and the temple polices it. Typical conflicts: seer against priest, and the inquisition against anyone unlicensed (`factions.md` B4).
- **Resources:** consumes herbs, **stimulants & sugar**, small offerings. Produces advice, charms and warnings.
- **Siting:** the countryside everywhere, most where the Supernatural Field is high and the temple is far: forest, hill and marsh. Astrologers at courts in cities. Well-finders in Hot desert, Cold desert and Thorn scrub.
- **Frequency:** a design choice. Starting share 0.2 %: about one for every five villages, more where Supernatural is high.
- **Races and factions:** Sunlanders (diviner), Rainkeepers (astrologer), Canal folk (geomancer), Duskreaders (well-finder), Widegaze (weather watcher), Grass riders (shaman, shared with the priest). Factions: B7 druidic circle, B5 forbidden cult, H2 highland and forest tribe, A1 realm crown (the court astrologer).
- **Evidence:** Crusader Kings III Guru and Garden Hermit [CK3-C]; the Pathfinder Fortune Teller background [unverified]. No census names them: they are the people a tax roll calls by another trade.

### Sector 9. Margins

#### 47. Labourer

Sells the strength of the body by the day: digs, carries, loads, mows, threshes and cleans, wherever hands are wanted.

- **Covers:** day labourer, field hand, harvest hand, builder's labourer, digger, ditcher, dock hand, quarry hand, peat cutter, ice cutter, gravedigger.
- **Lore:** the landless and the toolless. In the country the labourer has a cottage and a garden and works other men's fields; in the town he waits at the hiring place each dawn. Wages are set by law after a plague and paid above it. The labourer is the reserve of every other vocation and the first to go hungry.
- **Resources:** consumes nothing of its own. Wins peat and turf (**fuel**) and ice, and adds hands to every chain: harvest (**grain**), building, haulage, the mines.
- **Siting:** everywhere. In the country most where farms are large; in towns at harbours, building sites and markets. Low Prosperity raises the share.
- **Frequency:** 15.0 % of the male workers of the 1381 poll tax and 25.9 % of those of the 1522 muster rolls are called only "labourer" [BCL]. Starting share 11 %, two thirds of it in farm work.
- **Races and factions:** Swiftborn (harvest hand, builder's labourer), Noonfolk (field hand), Hearthbloods (ice cutter). Factions: A5 peasant commune, A6 rebels and pretenders (the risings).
- **Evidence:** [BCL]; Catasto codes 50 *manovale* (urban labourer), 04 agricultural labourer and 17 gravedigger [CAT]; the mowers, reapers and binders hired in bands of five [SEN]. Banished Laborer, the default of every citizen without a profession [BAN-P]; RimWorld Haul, Clean and Basic [RW-W]; the D&D untrained hireling at 2 sp a day [DND-T].

#### 48. Beggar

Lives on alms: too old, sick, maimed or unlucky to work, or unwilling.

- **Covers:** beggar, pauper, vagrant, almsman, cripple, pilgrim without means, refugee, orphan of the street.
- **Lore:** the deserving poor have a place: at the temple door, on the almoner's list, in the hospital. The sturdy beggar is whipped out of town. Beggars see and hear everything in a street and sell it. Fantasy twists: a beggars' king, a curse that passes with the coin.
- **Resources:** consumes the alms of the temple and the rich: bread, old Clothing.
- **Siting:** towns and cities, at temples, gates, markets and shrines; on the roads to a pilgrimage site. More where Prosperity is low, and after war or famine.
- **Frequency:** the 1379 poll tax exempts "genuine beggars" [POOS], so they are in no return. The social tables used for 1688 and after have a class of "labourers, cottagers and paupers" and one of vagrants [BCL]. Postan held that Domesday may leave out the landless, up to a third of the people [DB]. Starting share 1.5 %.
- **Races and factions:** none of the 28 races names the beggar. Factions: F1 thieves' guild (its eyes), B6 popular sect, B1 established church (as the giver of alms).
- **Evidence:** Catasto code 72, beggar or ward of the Church [CAT]; the monastic almoner [OBED]; Crusader Kings III High Almoner [CK3-C]; the D&D wretched and squalid lifestyles [DND-T].

#### 49. Thief

Takes by stealth or deceit, in a town: cuts purses, breaks houses, receives, smuggles, forges, and kills for pay.

- **Covers:** cutpurse, pickpocket, burglar, fence, smuggler, forger, swindler, blackmailer, enforcer, poisoner, hired killer.
- **Lore:** crime in a town is organised like a craft: with masters, territories, dues and a peace of its own. The fence is a shopkeeper by day; the smuggler is a sailor or a carter who carries one more bale. The law hangs the poor thief and fines the rich one.
- **Resources:** consumes nothing; moves coin, fine goods and the taxed goods (**salt**, **spices**, **stimulants & sugar**).
- **Siting:** cities and large towns, most where trade and Prosperity are high: harbours, markets, the capital. Smugglers at a `harbour`, a region `border` or a toll point.
- **Frequency:** a design choice; no roll counts them, though Paris taxes four *houliers*, glossed as "low life" [P1292-SCA]. Starting share 0.3 %.
- **Races and factions:** Shoalfolk (smuggler), Bitterbloods (the poisoner whom most villages disown). Factions: F1 thieves' guild, F2 assassins' brotherhood, F3 black network, F6 smugglers' ring.
- **Evidence:** the D&D Criminal background with its specialities: blackmailer, burglar, enforcer, fence, highway robber, hired killer, pickpocket, smuggler [DND-B]; D&D thieves' tools, forgery kit and disguise kit [DND-T].

#### 50. Bandit

Takes by force, outside the towns: robs on the road, raids, rustles, poaches, and at sea takes ships.

- **Covers:** highwayman, robber, raider, rustler, poacher, outlaw, pirate, wrecker, rebel in the hills.
- **Lore:** most bandits were something else first: a soldier without pay, a peasant without land, a herder in a hungry year. The outlaw is outside the law's protection and may be killed by anyone. Some bands are a lord's deniable arm, some a people's resistance, some only hungry. Raiding is a season's work for whole peoples, with no shame in it.
- **Resources:** consumes Weapons, **horses & pack animals**, ships. Lives on what the carter (31), the sailor (32) and the herder (3) carry.
- **Siting:** remote patches near a road: forest, Hills and Mountains, marsh, a region `border`, a pass. Pirates in a hidden `harbour` near a sea lane. Low Prosperity raises the share. `factions.md` §5.3: bandits want a medium remoteness, "near roads but not near garrisons".
- **Frequency:** a design choice. Starting share 0.25 %, several times that on a poor border.
- **Races and factions:** Northmen (raider), Saltroad folk (veiled raider), Hearthbloods (raider in a hungry year). Factions: F4 bandit clan, F5 pirates, A6 rebels and pretenders, G4 resistance of a dispossessed people, H6 goblin warband.
- **Evidence:** the bandit, bandit archer and bandit crossbowman of Medieval Dynasty's game data [MD-P]; the highway robber among the D&D Criminal specialities [DND-B]; the bandit clans and pirates of `factions.md` F4 and F5.

---

## 4. Resource coverage

*This section is my own mapping of §3 onto `map-generation.md` section 14 and Appendices B, D and G.*

### 4.1 The 24 generated resources against the vocations

"Won by" is the vocation that takes the resource from the patch. "First worked by" is the next step of the chain. Numbers are the vocations of §3.

| Resource | Group | Won by | First worked by | Also used by |
|---|---|---|---|---|
| Grain | Food | Farmer 1 | Miller 10, Brewer 12 | Baker 11, Innkeeper 33, Herder 3 and Carter 31 (fodder), Priest 41 (tithe), Noble 37 (rent), Merchant 29 |
| Produce | Food | Farmer 1 | Baker 11 (cook) | Shopkeeper 30, Innkeeper 33. Not traded beyond its zone |
| Orchard | Food | Gardener 2 | Brewer 12 (wine, cider), Miller 10 (oil) | Baker 11, Priest 41 (wine), Merchant 29, Shopkeeper 30 |
| Livestock | Food | Herder 3 | Butcher 13 (meat, tallow), Tanner 16 (hides), Weaver 17 (wool) | Farmer 1 and Forester 6 (draught oxen), Leatherworker 24, Scribe 42 (parchment), Noble 37 |
| Fish | Food | Fisher 4 | Butcher 13 (fishmonger, fish salter) | Innkeeper 33, Shopkeeper 30, Merchant 29 (salt fish) |
| Wild food & game | Food | Hunter 5 | Butcher 13, Tanner 16 (hides) | Healer 34, Alchemist 45 and Seer 46 (wild herbs), Noble 37 (the hunt) |
| Timber | Materials | Forester 6 | Carpenter 19, Shipwright 20, Charcoal burner 14 | Mason 21, Miner 7 (props), Miller 10, Smith 18 (hafts), Tanner 16 (bark), Artist 27, Artificer 28 |
| Stone | Materials | Quarryman 8 | Mason 21, Charcoal burner 14 (lime) | Miller 10 (millstone), Smith 18 (whetstone), Smelter 15 (flux), Artist 27 (marble), Priest 41 |
| Clay | Materials | Quarryman 8, Potter 22 | Potter 22 | Mason 21 (brick, daub, adobe), Smelter 15 (furnace), Weaver 17 (fuller's earth), Salter 9 (pans) |
| Crystal | Materials | Miner 7 (rock crystal), Quarryman 8 (glass sand) | Glassmaker 23 | Jeweller 26, Artificer 28 (lenses), Mage 44 (foci) |
| Fibre | Materials | Farmer 1 (flax, hemp, cotton, jute), Gardener 2 (silk, tree fibres) | Weaver 17 | Tailor 25, Shipwright 20 and Sailor 32 (rope, sail), Fisher 4 (nets), Leatherworker 24 (thread), Scribe 42 (paper) |
| Fuel | Fuel | Forester 6 (firewood), Charcoal burner 14 (charcoal), Labourer 47 (peat, turf), Miner 7 (coal, lignite), Herder 3 (dung) | Smelter 15, Smith 18 | Potter 22, Glassmaker 23, Salter 9, Brewer 12, Baker 11, Butcher 13 (smoke), Guard 40 (beacons), Alchemist 45 (oil and naphtha), every household |
| Salt | (own) | Salter 9 | Butcher 13 (curing), Fisher 4 | Tanner 16, Herder 3 (cheese, salt licks), Baker 11, Glassmaker 23 and Alchemist 45 (natron), Merchant 29, Thief 49 (smuggling) |
| Iron ore | Metals | Miner 7 | Smelter 15 | Smith 18 and through the smith every tool user; Merchant 29 |
| Copper ore | Metals | Miner 7 | Smelter 15 | Smith 18 (bronze, bells), Jeweller 26 (alloy), Artist 27 (malachite and azurite pigment), Alchemist 45 |
| Tin | Metals | Miner 7 (lode, stream tin) | Smelter 15 (bronze) | Smith 18 (bronze, pewter), Merchant 29 (the long tin route) |
| Silver & lead | Metals | Miner 7 | Smelter 15 (refiner) | Jeweller 26 (plate, coin), Mason 21 (lead roofs), Potter 22 (glaze), Glassmaker 23, Alchemist 45, Merchant 29 |
| Gold | Metals | Miner 7 (lode, gold washer) | Smelter 15, Jeweller 26 | Artist 27 (leaf), Artificer 28, Mage 44, Merchant 29, Noble 37, Priest 41 |
| Gems & pearls | Luxuries | Miner 7 (stones), Fisher 4 (pearl, coral) | Jeweller 26 | Artist 27 (lapis), Artificer 28, Mage 44, Merchant 29, Noble 37, Priest 41 |
| Spices | Luxuries | Gardener 2 | Merchant 29 (spicer), Alchemist 45 | Healer 34, Priest 41 (incense), Baker 11, Innkeeper 33, Thief 49 (smuggling), Courtesan 51 (perfume) |
| Stimulants & sugar | Luxuries | Gardener 2 (with the beekeeper) | Brewer 12 (mead), Merchant 29 | Innkeeper 33, Baker 11 (honey), Healer 34, Priest 41 and Scribe 42 (beeswax), Seer 46 |
| Furs & ivory | Luxuries | Hunter 5, Fisher 4 (seals, walrus, whales) | Tanner 16 (furrier), Jeweller 26 (ivory carver) | Tailor 25, Merchant 29, Noble 37 |
| Dyes | Luxuries | Gardener 2 (dye crops), Fisher 4 (murex), Hunter 5 (kermes, lichen) | Weaver 17 (dyer) | Tailor 25, Artist 27 (pigment), Scribe 42 (ink), Glassmaker 23 |
| Horses & pack animals | Animals | Herder 3 (horse and camel breeder) | Carter 31, Soldier 39 | Noble 37, Farmer 1 (plough), Forester 6 and Quarryman 8 (haulage), Bandit 50, Smith 18 (farrier), Leatherworker 24 (saddler) |

**No generated resource is without a vocation.** Three are thin:

- **Crystal** has one real user, the glassmaker, and two small ones.
- **Tin** has no use except with copper, and the two never share a zone (`map-generation.md` §14.4). It matters only where a merchant carries it.
- **Produce** feeds people but employs no vocation after the farmer, since it is not traded beyond its zone (`game-resources-and-trade.md` §5.2).

Salt has no group in section 14 or in the readme's list of groups. Appendix G.3 files it under Materials as a variant of "Preserver".

### 4.2 Variants and catalogue rows

The vocations use the variant of a resource, not only the resource. Three things stand out.

**Variants that decide the variant of the vocation.** The generator already picks them; the vocation can read them.

| Resource variant | What it makes of the vocation |
|---|---|
| Grain: paddy rice | Farmer becomes paddy farmer, with water works for the mason and the official |
| Livestock: sheep / cattle / pigs / herded reindeer / yak / goats / water buffalo / poultry | Herder becomes shepherd, cowherd, swineherd, reindeer herder and so on; sheep also feed the weaver |
| Horses & pack animals: horses / donkeys and mules / dromedary camel / Bactrian camel / llama and alpaca-like camelids | Carter becomes teamster, muleteer or camel driver; only horses give cavalry |
| Fish: cod banks, herring, salmon runs / pearl oyster and precious red coral (gems) / seals, walrus, whales (furs) | Fisher becomes deep-sea fisher, diver or whaler |
| Timber: oak / Scots pine, spruce, larch, fir / teak / mangrove timber | Shipwright's oak and teak; the tar burner's conifers; the tanner's oak and mangrove bark |
| Stone: limestone, chalk, marble / millstone and quern stone / slate / flint and chert, obsidian | Lime burner; miller's stones; roofer's slate; sculptor's marble |
| Clay: brick and pottery clay / adobe and mud-brick earth / wattle and daub / rammed earth and cob / kaolin | Potter and brickmaker, or a mason who builds in earth; kaolin gives fine ware |
| Fuel: firewood / charcoal / peat / turf and sod / dung fuel / coal / lignite / oil and naphtha seeps | Forester, charcoal burner, labourer (peat cutter), herder, miner; only charcoal and coal serve the smelter |
| Salt: solar sea salt / lake and playa salt / natron and soda / rock salt / brine springs | Salt raker, soda burner, rock-salt miner or salt boiler; brine needs fuel |
| Iron ore: bog iron, lateritic iron / banded iron formation, magnetite skarn iron | A farmer's off-season digging, or a true mine |
| Gold: placer gold, beach gold / lode gold | Gold washer or hard-rock miner |

**Variants with no pre-industrial user among the vocations.** Rubber latex (a spice crop in Appendix B); coca, kola nut and betel nut beyond the merchant who sells them; oil and naphtha seeps beyond lamps and the alchemist. They are harmless: they are luxuries and trade goods and need no vocation of their own.

**Catalogue rows that the vocations need and the generation cannot pick.** Appendix G.3 says itself that the catalogue is finer than the generation. For the vocations three of its rows matter: **wool** (a Fibre variant; the generation's fibre is crops only), **dairy products** (a livestock variant) and **pine resin, tar and pitch** (an incense variant; Appendix B has no crop for it). §4.4 proposes to derive them.

### 4.3 What the vocations produce

Products are not generated with the map (Appendix G.2). The vocations of §3 make these, named as in `game-resources-and-trade.md` §5.2 where it has a name:

| Step | Product | Made by | From | Group of Appendix G.1 |
|---|---|---|---|---|
| Intermediate | Flour and meal, oil | Miller 10 | Grain, orchard | none |
| Intermediate | Meat, hides, tallow, horn | Butcher 13 | Livestock, wild food & game | none |
| Intermediate | Wool, milk and cheese | Herder 3 | Livestock | none |
| Intermediate | Charcoal, naval stores, potash, lime | Charcoal burner 14 | Timber, fuel, stone | none (lime: Building) |
| Intermediate | Iron, steel, copper, bronze, lead, silver, gold | Smelter 15 | The five metal resources, fuel | none |
| Intermediate | Leather, dressed furs, parchment | Tanner 16 | Hides, furs & ivory, bark, lime, salt | none |
| Intermediate | Cloth, rope, nets, sailcloth | Weaver 17 | Fibre, wool, dyes | Clothing, Gear |
| Intermediate | Planks | Forester 6 (sawyer) | Timber | Building |
| Intermediate | Bricks and tiles, pottery | Potter 22 | Clay, fuel | Building, Tools |
| Intermediate | Glass | Glassmaker 23 | Crystal, fuel, a flux | Building |
| Finished | Bread and cooked food | Baker 11 | Flour, produce, fuel | none |
| Finished | Drink (ale, wine, cider, mead) | Brewer 12 | Grain, orchard, honey | none |
| Finished | Preserved food | Butcher 13, Fisher 4 | Meat, fish, salt | none |
| Finished | Tools | Smith 18, Carpenter 19 | Iron, timber | Tools |
| Finished | Arms and armour | Smith 18, Carpenter 19, Leatherworker 24 | Iron or bronze, timber, leather | Weapons, Gear |
| Finished | Clothing and shoes | Tailor 25, Leatherworker 24 | Cloth, leather, furs | Clothing |
| Finished | Saddlery and harness | Leatherworker 24 | Leather | Gear |
| Finished | Buildings | Mason 21, Carpenter 19 | Stone, bricks, lime, timber | Building |
| Finished | Ships, boats, carts, barrels | Shipwright 20, Carpenter 19 | Timber, naval stores, rope, iron | none |
| Finished | Fine goods and coin | Jeweller 26, Artist 27, Glassmaker 23 | Gold, silver, gems, ivory, dyes | none |
| Finished | Instruments and devices | Artificer 28 | Metals, crystal | Tools, Gear |
| Finished | Books and documents | Scribe 42 | Parchment, paper, ink | none |
| Finished | Medicine, poisons, perfumes | Alchemist 45, Healer 34 | Spices, herbs, reagents | none |

**Finding.** The five produced groups of Appendix G.1 (Building, Tools, Gear, Weapons, Clothing) have no place for about half of what the vocations make: processed food and drink, the refined materials between ore and tool, fine goods and coin, books, medicine, and ships and vehicles. This is not a disagreement between documents: Appendix G says that produced resources are "only covered lightly". It is a gap that a production model would have to close.

### 4.4 Missing resources: proposals

*Every row is a proposal, not a rule. The user decides. `patch-resources.md` already holds most of the raw material as catalogue rows that were left out when the 24 were chosen; the row numbers are given.*

| # | Proposal | What it is | Vocations that need it | Where it would be found | Generated raw resource or derived good |
|---|---|---|---|---|---|
| P1 | **Reagents** | The mineral salts and earths of the workshop: alum (`patch-resources.md` #173), sulphur (#315), quicksilver from cinnabar (#351), saltpetre (#172), vitriol, borax | Weaver 17 (alum fixes most dyes: madder "needs alum mordant", #232), Tanner 16 (alum tawing), Glassmaker 23, Alchemist 45, Healer 34, Artist 27 (vermilion), Jeweller 26 and Smelter 15 (gilding and gold amalgam), Mage 44 | Alum, sulphur and cinnabar on Volcanic arc in Hills and Mountains. Alum shale and vitriol on Clastic basin. Saltpetre and borax in arid river-poor flat zones (the gate of playa salt) and in caves on Carbonate platform | **A new generated raw resource**, a deposit like salt: rare, by bedrock, with a trade value near that of copper. One resource with variants, not six. It could sit in Materials beside salt |
| P2 | **Herbs** | Medicinal and culinary wild plants (#93, "all forest and grassland"). Today "wild herbs" is one variant of wild food & game in Woodland & shrubland and the tropical forests, so a patch shows either game or herbs | Healer 34, Alchemist 45, Seer 46, Brewer 12 (hops and brewing herbs), Priest 41 | A potential by biome, like honey: high in Woodland & shrubland, the temperate forests and Tropical rain forest (fever barks), low on Tundra and in the deserts, none on Ice & snow. Rare variants could be scaled by the Supernatural Field | **A new generated raw resource**, a potential. The cheap alternative is to derive it: herbs = a share of wild food & game. A resource of its own gives the healer a siting and the herb trade a reason |
| P3 | **Arcane materials** | The fantasy resources. Appendix G.3 names star metal and the sap of Elanor and says the generation has no rule for them. `patch-resources.md` lists 28 (F1 to F28) with placement rules | Mage 44, Artificer 28, Alchemist 45, Smith 18, Jeweller 26, Seer 46 | By the F-rules, each tied to a real parent (sky-iron in dry or frozen land, mana crystal on wet volcanic ground, elderwood in untouched forest). At the least: a deposit whose chance rises with the Supernatural Field at the patch | **A new generated raw resource**, behind a toggle, as `game-resources-and-trade.md` §5.2 suggests ("Arcane reagents"). It would be the first thing the Supernatural Field feeds |
| P4 | **Wool, hides, dairy, tallow, horn** | The by-products of animals. The catalogue lists wool under Fibre and dairy under livestock; the generation picks neither | Weaver 17, Tanner 16, Butcher 13, Herder 3, Leatherworker 24, Scribe 42 | Wherever livestock is: wool from the variants sheep, goats and yak and from the camelids; hides from every variant and from game; dairy from cattle, sheep, goats, yak, water buffalo and herded reindeer | **Derived**, not generated: a by-product table keyed on the livestock variant. Without it the weaver of Cold steppe and Tundra has no fibre |
| P5 | **Tanbark, potash, resin and pitch** | What the forest gives beside logs | Tanner 16, Glassmaker 23, Shipwright 20, Charcoal burner 14 | Bark from the timber variants oak, cork oak and mangrove timber. Resin, tar and pitch from Scots pine, spruce, larch, fir and Mediterranean pines. Potash from any hardwood | **Derived** from the timber variant. `game-resources-and-trade.md` §5.3 reached the same conclusion |
| P6 | **Gypsum and alabaster** | The stone of plaster (#312). Not among the stone variants of Appendix D.2 or G.3 | Mason 21 (plasterer: 36 in Paris [P1292-SCA]), Artist 27 | Clastic basin, with rock salt (the same evaporite beds) | **A new variant** of the generated resource Stone |
| P7 | **Elephant, reindeer and sled dog** | Pack and draught animals of the biomes where `mounts` is 0: Tundra, Boreal forest, Ice & snow, the rain forests | Carter 31, Herder 3, Soldier 39 | Elephant in Tropical dry forest, Savanna and Tropical rain forest; reindeer and sled dogs on Tundra, in Boreal forest and on Ice & snow coasts | **New variants** of Horses & pack animals, with a `mounts` factor above 0 in those biomes. The Rainkeepers name an elephant driver and the Hoarkin a sled maker |
| P8 | **Water power** | The fall of a stream that turns a wheel | Miller 10, Weaver 17 (fulling), Smelter 15 (bellows, hammers), Forester 6 (sawmill), Smith 18 | `river` of 1 or more in Lowland and Hills | **A derived site flag**, not a resource, like `harbour`. `game-resources-and-trade.md` §5.3 lists it among the enablers |
| P9 | **Produced groups** | Groups for what §4.3 could not place: provisions (food and drink), refined materials, fine goods, vehicles | All of sectors 2 and 3 | Not on the map | **New groups** in Appendix G.1, if products are ever modelled |

P1 and P2 are the two the vocations need most. Without P1 the dyer, the tawyer, the glassmaker and the alchemist work from nothing the map can show. Without P4 the cloth of half the biomes has no source.

---

## 5. Biome view

*Own judgement, read off the factors of `map-generation.md` Appendix C (soil, pasture, game, wood, fur, mounts, honey, road cost), the crop tables of Appendix B and the rules of Appendix D.*

The vocations of sectors 4 to 9 follow people, not land: a town in any biome has its baker, tailor, priest and thief. The table therefore lists the vocations that the land decides. "Typical" means the biome raises the share well above the starting share. "Absent" means the resource the vocation lives on is 0 or nearly so.

### 5.1 By biome

| Biome | What the land gives | Typical | Rare | Absent |
|---|---|---|---|---|
| **Ice & snow** | No soil, pasture or wood. Seals, walrus and narwhal on the coast; fish 0.6 | Fisher (sealer, whaler), Hunter (guide) | Miner (by bedrock), Carter (sled only) | Farmer, Gardener, Herder, Forester, Charcoal burner, Miller, Potter |
| **Tundra** | Soil 0.3, pasture 0.25 (herded reindeer), fur 0.5, no wood, turf for fuel | Herder (reindeer), Hunter (fox, caribou), Fisher | Farmer, Miller, Carpenter, Smith (no fuel) | Forester, Charcoal burner, Gardener, Shipwright |
| **Cold steppe** | Soil 1 but dry; pasture 0.7 (sheep), mounts 0.9 (horses); wood 0.08 | Herder (shepherd, horse breeder), Weaver (wool, felt), Tanner, Carter, Soldier (cavalry) | Farmer (rye, barley, oats where there is water), Forester (gallery woods) | Charcoal burner, Shipwright, Gardener of tree crops |
| **Boreal forest** | Wood 1, fur 0.85, game 0.7; soil 0.55; bog iron; peat | Forester, Charcoal burner (tar), Hunter (trapper), Smelter (bog iron), Fisher, Tanner (furrier), Shipwright on water | Farmer (rye, barley), Herder (reindeer) | Gardener of tree crops; Carter with horses (mounts 0) |
| **Cold desert** | Pasture 0.2, mounts 0.5 (Bactrian camel), no wood; playa salt | Carter (caravan), Herder (sheep, camel), Salter, Hunter (guide), Seer (well-finder) | Farmer and Gardener (only on a river), Miner (turquoise, opal) | Forester, Charcoal burner, Fisher away from water |
| **Temperate grassland** | Soil 1, pasture 1, mounts 0.9; wood 0.1 | Farmer, Herder (cattle, sheep, horses), Miller, Carter, Leatherworker, Weaver (wool, flax) | Forester, Carpenter (imported wood) | Charcoal burner away from rivers |
| **Woodland & shrubland** | Soil 1, pasture 0.7, wood 0.4, honey 0.3; olive, grape, nuts; kermes | Gardener (olive, vine, beekeeper), Brewer (wine), Herder (sheep, goats), Farmer, Miller (oil press), Potter, Healer (herbs) | Forester, Shipwright | None of the land vocations |
| **Temperate mixed forest** | Soil 1, wood 1, game 0.8, pasture 0.6; bog iron | Farmer, Forester, Charcoal burner, Carpenter, Hunter, Smelter, Smith, Brewer (ale) | Gardener of warm crops | None |
| **Temperate broadleaf forest** | Soil 1, wood 1 (oak), game 0.8, pasture 0.6 (pigs, cattle); the best all-round land | Farmer, Herder (swineherd), Forester, Carpenter, Shipwright (oak), Tanner (oak bark), Weaver (flax, hemp), Brewer | None | None |
| **Temperate rain forest** | Wood 1 (giant conifers), game 0.8, soil 0.55; salmon runs on cold coasts; road cost 2 | Forester, Shipwright, Fisher, Hunter, Carpenter | Farmer, Herder | Carter with horses (mounts 0) |
| **Hot desert** | Pasture 0.15, mounts 0.5 (dromedary), no wood; solar and playa salt; dates on rivers | Carter (caravan), Herder (goats, camels), Gardener (date palm, on a river), Salter, Hunter (guide), Seer (well-finder), Mason (adobe) | Farmer (irrigated), Miner (turquoise) | Forester, Charcoal burner, Potter of fired ware, Hunter of game |
| **Thorn scrub** | Soil 0.7, pasture 0.5 (goats), wood 0.25; myrrh, frankincense, gum arabic; ostrich | Herder (goats), Gardener (incense gatherer), Farmer (sorghum, millet), Carter (donkeys, camels) | Forester, Carpenter | Shipwright |
| **Savanna** | Soil 0.6, pasture 0.8 (cattle), game 0.6, ivory; lateritic iron; cotton | Herder (cattle), Farmer (sorghum, millet), Hunter (ivory), Smelter and Smith (laterite iron), Weaver (cotton) | Forester (wood 0.35) | None |
| **Tropical dry forest** | Soil 0.7, wood 0.8 (teak), game 0.6; rice on floodplains; cotton, indigo, lac, spices | Farmer (paddy), Gardener (spices, dyes), Forester (teak), Shipwright, Weaver (cotton, dyer), Herder (water buffalo) | Carter with horses (mounts 0.1) | None |
| **Tropical rain forest** | Wood 1 (mahogany, ebony), soil 0.4, pasture 0.15; pepper, clove, nutmeg, cacao; road cost 2.2 | Gardener (spice grower), Forester, Hunter (gatherer), Fisher (river), Sailor (river boatman), Alchemist (poisons, barks) | Farmer (yam, taro, cassava), Herder (pigs, poultry) | Carter with animals (mounts 0): porters instead |
| **Mangrove** | Fish 0.9 (crab, lobster, shrimp); mangrove timber 0.5, charcoal, bark; soil 0.3; buildable × 0.5; road cost 3 | Fisher, Sailor (boatman), Charcoal burner, Tanner (mangrove bark), Shipwright (small craft) | Farmer, Salter (only where it is dry enough) | Herder, Carter, Quarryman, Miner |

### 5.2 By elevation type, bedrock and water site

| Site | Raises | Lowers |
|---|---|---|
| **Flat Land, Lowland** | Farmer (farmland 1), Potter and brickmaker (clay 0.7 and 0.6), Carter with carts, Salter on a dry coast, Miner of coal and lignite on Clastic basin | Quarryman, Mason in stone, Miner of metals |
| **Hills** | Quarryman (stone 0.75), Miner, Smelter, Gardener (vine, olive, tea: farmland 1 in hills for those crops), Herder (goats, mules), Miller (fall for the wheel), Noble (a strong seat), Bandit | Farmer (0.6), Potter |
| **Mountains** | Quarryman (0.95), Miner, Herder (yak, alpine summer pasture), Hunter (guide, wild sheep and ibex), Guard (pass warden), Bandit | Farmer (0.3), Carter with carts (road cost 4), Shipwright |
| **Fold belt** | Miner (silver, lode gold, garnet, emerald), Quarryman (slate, marble), Artist (marble) | |
| **Granite intrusion** | Miner (tin, skarn iron, topaz), Potter (kaolin, where warm and humid) | |
| **Volcanic arc** | Miner (copper, gold, rock crystal, amethyst), Quarryman (obsidian, pumice), Glassmaker; the reagents of P1 | |
| **Ophiolite** | Miner (copper, jade), Quarryman (soapstone) | |
| **Carbonate platform** | Quarryman (limestone, chalk, flint, marble), Charcoal burner (lime burner), Mason, Miner (lead and zinc) | |
| **Crystalline shield** | Miner (banded iron, lode gold in Hills, diamond), Quarryman (granite) | |
| **Flood basalt** | Quarryman (basalt, millstone), Miner (native copper, agate) | |
| **Clastic basin** | Miner (coal, lignite, red-bed copper), Salter (rock salt, brine), Quarryman (sandstone, millstone) | |
| **Unconsolidated cover** | Potter (clay + 0.1), Farmer | Quarryman (stone × 0.5), Miner |
| **River, `river` 1 or more** | Miller, Weaver (fuller, dyer), Tanner, Brewer, Farmer (irrigation, floodplain), Miner (gold washer, tin streamer) | |
| **Navigable river, `river` 8 or more** | Sailor (boatman), Shipwright, Merchant, Fisher, Glassmaker (glass sand) | Carter (the river takes the freight) |
| **`harbour` and coast** | Fisher, Sailor, Shipwright, Merchant, Salter (dry warm low coast), Thief (smuggler), Bandit (pirate) | |
| **Lake** | Fisher (0.45 to 0.75), Sailor (ferryman) | |
| **High Prosperity** | Merchant, Jeweller, Artist, Glassmaker, Tailor, Servant, Scribe, Scholar, Entertainer, Courtesan, Thief, Mason; and the miners of the valuables, whose deposits the Prosperity boost multiplies | Beggar, Bandit, Labourer, Soldier |
| **High Supernatural** | Mage, Seer, Alchemist, Artificer, Priest, Hunter (monster hunter) | |

---

## 6. Races and factions

### 6.1 Race to vocation

*From the **Vocations** line of each entry of `races-basis.md`, mapped by me onto the 51 vocations. The race's own word is in brackets where it differs. "Never" is stated by the entry.*

| Race | Vocations it is known for |
|---|---|
| Northmen | Shipwright; Sailor (pilot); Bandit (raider); Merchant (trader); Farmer (free farmer); Fisher (herring fisher, whaler); Hunter (trapper); Charcoal burner (tar burner); Smith; Artist or Mage (rune carver); Entertainer (poet); Official (law-speaker); Guard (sworn guard abroad) |
| Midlanders | Farmer (ploughman); Herder (shepherd); Miller; Brewer (brewer, vintner); Mason; Carpenter; Weaver (weaver, fuller); Smith; Merchant; Scribe (clerk); Priest (priest, monk); Noble (knight); Soldier (man-at-arms); Official (bailiff); Miner |
| Saltroad folk | Carter (caravan master); Hunter (guide); Herder (camel breeder); Mason (well digger, mud-brick mason); Gardener (date gardener, incense gatherer, coffee grower); Scholar (scholar, astronomer); Official (judge); Scribe; Healer (physician); Merchant (merchant banker); Leatherworker; Bandit (veiled raider) |
| Sunlanders | Smith (smith, bronze caster); Smelter (iron smelter); Miner (gold washer); Herder (cattle herder); Farmer (millet or yam farmer); Entertainer (praise-singer, drummer); Jeweller (ivory carver); Weaver (weaver, indigo dyer); Hunter; Soldier (cavalryman); Merchant (river trader); Seer (diviner); Noble (title-holder) |
| Grass riders | Herder (herder, horse breaker); Soldier (mounted archer); Carpenter (bowyer); Weaver (felt maker); Carter (cart driver, post rider); Hunter (scout, falconer); Entertainer (wrestler); Priest or Seer (shaman); Guard (caravan guard); Official (tax collector over conquered farmers) |
| Rainkeepers | Farmer (rice farmer); Official (reservoir keeper); Weaver (cotton weaver, dyer); Gardener (spice grower); Carter (elephant driver); Artist (temple sculptor); Priest (priest, ascetic); Seer (astrologer); Scholar (mathematician); Healer (physician); Jeweller; Smelter (steel maker); Merchant (sea merchant); Entertainer (dancer) |
| Canal folk | Farmer (paddy farmer); Gardener (silk grower, tea grower); Potter; Mason (canal engineer); Sailor (boatman); Official (magistrate, examiner, salt commissioner); Scribe (clerk, printer); Scholar (tutor); Healer (physician); Soldier (wall soldier); Merchant; Seer (geomancer) |
| Elves | Forester (forester, tree shaper); Carpenter (bowyer); Soldier (archer); Hunter (ranger of the forest edge); Healer (herbalist, healer); Entertainer (harper); Weaver; Brewer (vintner); Scholar (lore keeper, tutor at a human court); Official (envoy) |
| Dwarves | Miner (miner, prospector); Smith (smith, armourer); Mason (mason, engineer); Guard (tunnel warden, vault keeper); Brewer; Herder (goat herd); Scribe (records clerk); Merchant (gate merchant); Soldier (heavy foot soldier for hire) |
| Halflings | Gardener (gardener, orchard keeper, beekeeper, tobacco grower); Brewer (brewer, cider maker); Herder (cheesemaker); Baker (baker, cook); Innkeeper; Miller; Sailor (bargeman); Shopkeeper (peddler); Servant (taster); Guard (slinger of the watch); Scholar (keeper of genealogies) |
| Gnomes | Jeweller (gem cutter, jeweller, engraver, assayer); Artificer (clockmaker, locksmith, lens grinder, instrument maker, tinker); Merchant (moneychanger); Gardener (seedsman, grafter, mushroom grower); Artist (map engraver); Scholar (riddling tutor) |
| Orcs | Soldier (warrior, soldier for hire); Herder; Hunter (hunter, scout); Smith; Butcher; Tanner; Jeweller (bone carver); Quarryman (quarry hand); Carter (porter); Guard (caravan guard); Noble (chief, hearth-mother); Scholar (greyback) |
| Hoarkin | Fisher (seal hunter, whaler, diver in freezing water, wreck salvager); Sailor (ice pilot); Jeweller (ivory carver); Butcher (blubber renderer); Hunter (guide over the frozen sea); Carpenter (sled maker); Carter (winter courier); Shopkeeper (keeper of an ice cellar in a southern town) |
| Hearthbloods | Guard (pass warden, avalanche rescuer); Miner (winter miner); Smith (forge hand); Herder (yak herder); Hunter; Official (larder-lord); Healer (warming nurse); Carter (winter courier); Labourer (ice cutter); Entertainer (wrestler); Bandit (raider in a hungry year) |
| Widegaze | Guard (lookout, signaller, beacon keeper, sentry, road warden); Herder (range herder); Hunter (scout, spotter for a hunt); Official (surveyor); Sailor (ship's lookout); Scribe (witness for hire); Seer (weather watcher); Scholar (star watcher) |
| Amberbloods | Forester (forester, grove warden, resin tapper); Official (timber judge); Charcoal burner (varnish maker); Healer (wound healer, herbalist); Carpenter (bowyer, woodcarver); Soldier (archer); Gardener (beekeeper); Guard (fire-watcher); Hunter (tracker) |
| Brinefolk | Salter (salt boiler, pan master, rock-salt miner, soda burner); Carter (salt carrier); Official (weigh-master); Butcher (pickler, fish salter); Tanner; Alchemist (embalmer); Glassmaker; Hunter (guide over waterless flats); Scribe (witness to a salt oath) |
| Duskreaders | Hunter (night guide, tracker, night hunter); Seer (well-finder); Guard (night watch); Official (examiner of witnesses); Healer (fever reader, midwife); Potter (kiln reader); Smith (temper judge at a forge); Mason (wall mason); Herder (goat herd); Shopkeeper (trader of the midnight market) |
| Noonfolk | Labourer (field hand); Farmer (grain farmer); Gardener (orchard keeper); Official (well keeper, speaker of the noon council); Potter (brick maker, potter); Quarryman; Weaver; Merchant (grain factor); Carter (day courier); Scholar (keeper of the sundial). **Never** Sailor, Miner or Guard (sentry) |
| Boughwalkers | Hunter (fruit gatherer, honey hunter, bird catcher); Forester (tree keeper); Weaver (dyer); Artist (painter of hidden cloth); Scribe (marker of seals and contracts); Gardener (spice gatherer); Jeweller (gem grader); Servant (food taster); Sailor (rigger, topman); Mason (steeplejack, bridge builder); Carter (runner of the high roads) |
| Shoalfolk | Fisher (pearl diver, sponge diver, fisher, shellfish gatherer, fish drier, salvager, harbour diver); Jeweller (coral cutter); Sailor (pilot, ferryman); Shipwright (hull mender, boatbuilder); Weaver (net maker); Thief (smuggler); Guard (reef warden) |
| Swiftborn | Labourer (harvest hand, builder's labourer); Gardener (vine dresser, olive picker); Carter (runner, courier); Entertainer (player, singer, dancer, acrobat); Baker (cook); Soldier; Healer (midwife); Shopkeeper (market trader); Scholar (chronicler); Official (magistrate for a season) |
| Flinchless | Soldier (soldier, shield bearer, drill master); Guard (bodyguard, firefighter); Healer (surgeon, bonesetter, tooth puller, anatomist, nurse); Farmer; Smith (striker at a forge); Carter (courier of secrets) |
| Lanternkin | Scribe (notary, clerk, bookkeeper); Merchant (banker, moneylender, insurer); Official (envoy, keeper of the lamp hall); Sailor or Carter (lamp on a ship or caravan); Jeweller (assayer); Glassmaker (vial blower); Gardener (fungus gardener) |
| Stillfaces | Hunter (tracker, hunting guide, finder of the lost, truffle hunter, herb gatherer); Alchemist (perfumer, composer of scents); Healer (diagnostician); Servant (poison finder at a court); Official (customs searcher, grader of wine, tea and spice); Gardener (beekeeper); Herder (swineherd) |
| Twofold | Of the Going: Herder, Hunter, Merchant (caravan trader), Shopkeeper (pedlar), Soldier, Sailor, Carter (messenger). Of the Turning: Official (judge, go-between). Of the Staying: Farmer, Weaver, Noble (head of a house), Priest (priest of the Pair), Healer (midwife, physician), Scholar (teacher), Official (keeper of the house's wealth) |
| Bitterbloods | Alchemist (apothecary, antidote maker, keeper of snakes and frogs, maker of arrow and fish poison, maker of insect smoke); Gardener (poison gardener); Hunter (gatherer of fever bark, swamp guide); Healer (physician of fevers); Servant (taster at a court); Thief (the poisoner whom most villages disown) |
| Catfolk | Guard (granary warden, night watch, herd guard); Hunter (ratcatcher, hunter, tracker, trapper, fowler, caravan scout); Fisher; Sailor (rigger); Mason (roofer, steeplejack); Entertainer (rope dancer); Carter (courier over a short distance); Butcher; Tanner (tanner, furrier); Priest (priest of the Small Huntress). **Never** Farmer (ploughman), Miller or Baker |

**What the view shows.**

- **Every vocation but two is named by some race.** No race names the Tailor or the Beggar. Several are named by one or two races only: the Salter, the Innkeeper, the Leatherworker, the Mage, the Quarryman, the Miller, the Baker, the Shipwright and the Smelter.
- **The human races carry the base economy.** Midlanders, Canal folk, Rainkeepers and Sunlanders name the farmer, the smith, the weaver, the merchant and the official. The new races name narrow specialities (the Brinefolk salt, the Lanternkin contracts, the Bitterbloods poison). A generator can therefore give a race a few strong affinities on top of a common base, and need not give every race a full table.
- **Two races state exclusions**, which a list of affinities must be able to hold as a factor of 0.
- **Many race words are a rank or a role, not work:** chief, hearth-mother, larder-lord, title-holder, head of a house, greyback. They belong to the role side of §7.3.

### 6.2 Faction archetype to vocation

*From the names and the summary tables of `factions.md` §3 and §5.2. Only the first entries of Group A were read line by line for this note; the rest is mapped by the archetype's name and base, so treat the table as a first draft.*

| Faction archetype | Vocations it employs |
|---|---|
| A1 Realm crown | Noble, Official, Soldier, Guard, Scribe, Servant, Jeweller (the mint), Scholar, Mage (court), Courtesan (court) |
| A2 Noble house | Noble, Official (steward, bailiff, reeve), Soldier, Servant, Farmer and Herder (tenants), Miller, Forester |
| A3 Free city council | Merchant, Official, Guard, Scribe, and the masters of the crafts; Courtesan (the licensed house) |
| A4 League of rulers | Noble, Official (envoy), Soldier |
| A5 Peasant commune | Farmer, Herder, Fisher, Miller, Labourer |
| A6 Rebels and pretenders | Noble, Soldier, Bandit, Weaver, Labourer |
| B1 Established church | Priest, Scribe, Mason and Artist (as patron), Servant, Beggar (as almsgiver) |
| B2 Monastic order | Priest (monk), Brewer, Gardener, Scribe, Healer, Scholar, Farmer (tenants) |
| B3 Holy order of knights | Soldier, Priest, Noble |
| B4 Inquisition and witch hunters | Official, Priest, Guard, Scribe |
| B5 Forbidden cult | Priest, Seer, Mage |
| B6 Popular sect | Priest, Beggar, Weaver, Labourer |
| B7 Druidic circle | Seer, Healer, Forester, Hunter |
| B8 Pilgrimage shrine keepers | Priest, Innkeeper, Guard, Shopkeeper |
| C1 Mages' college | Mage, Scholar, Scribe, Alchemist, Artificer, Glassmaker (supplier) |
| C2 Secret cabal of mages | Mage, Alchemist |
| C3 Necromancer cabal | Mage, Alchemist, Labourer (gravedigger) |
| C4 Artificers' society | Artificer, Smith, Jeweller, Glassmaker, Smelter |
| D1 Merchant guild | Merchant, Shopkeeper, Scribe, Carter |
| D2 Craft guild | One of sectors 2 and 3, by the resources of the town (`factions.md` §5.2) |
| D3 Merchant league | Merchant, Sailor, Shipwright, Guard |
| D4 Chartered company | Merchant, Sailor, Soldier, Gardener (plantations), Scribe |
| D5 Miners' guild | Miner, Smelter, Quarryman, Charcoal burner, Smith |
| D6 Frontier settlers' company | Farmer, Forester, Hunter, Carpenter, Smith |
| D7 Banking house | Merchant (banker), Scribe, Jeweller, Guard |
| D8 Caravan company | Carter, Merchant, Guard, Hunter (guide), Innkeeper |
| E1 Mercenary company | Soldier, Smith (armourer), Healer (surgeon) |
| E2 Secular knightly order | Noble, Soldier |
| E3 Fighters' and adventurers' guild | Soldier, Hunter, Guard |
| E4 Wardens against a great threat | Soldier, Guard, Hunter (scout) |
| E5 Town watch and militia | Guard, Official |
| E6 Monster hunters | Hunter, Alchemist, Smith |
| F1 Thieves' guild | Thief, Beggar, Shopkeeper (fence), Innkeeper, Courtesan |
| F2 Assassins' brotherhood | Thief, Alchemist (poisoner) |
| F3 Black network | Thief, Merchant, Carter |
| F4 Bandit clan | Bandit, Charcoal burner (cover), Hunter |
| F5 Pirates | Bandit, Sailor, Shipwright, Fisher |
| F6 Smugglers' ring | Thief, Sailor, Carter, Innkeeper, Salter |
| G1 Benevolent secret society | Scholar, Entertainer, Scribe, Merchant |
| G2 Scholars' and explorers' society | Scholar, Hunter (guide), Scribe, Sailor |
| G3 Healers' order | Healer, Alchemist, Priest |
| G4 Resistance of a dispossessed people | Bandit, Soldier, Hunter |
| H1 Nomad horde | Herder, Soldier, Noble (khan), Carter, Priest (shaman) |
| H2 Highland and forest tribe | Hunter, Herder, Seer, Soldier |
| H3 Dwarven hold | Miner, Smelter, Smith, Mason, Jeweller, Brewer, Guard |
| H4 Beastfolk tribe | Hunter, Fisher, Seer |
| H5 Elven enclave | Forester, Hunter, Healer, Entertainer, Mage, Carpenter (bowyer) |
| H6 Goblin warband | Bandit, Miner, Smith |

The two lists meet in the ranks. `factions.md` §5.1 asks for a rank ladder per archetype, which "the Persons of the Settled stage fill". A rank of a faction is then a role (§7.3), and the vocation says which persons can hold it: the master of a craft guild is a person whose vocation is that craft.

---

## 7. Design note: vocations in the generator

*Everything in this section is my own engineering judgement, not a sourced claim. It is material for `society-generation.md` §8, which is the user's to write.*

### 7.1 What a vocation needs as a record

| Attribute | Type | Where it comes from |
|---|---|---|
| Id and name | text | §3. Generated data would point at the id, as it does for a race |
| Sector | one of nine | §2.1 |
| Variants | list of (name, condition) | The **Covers** line. The condition is a resource variant, a water site, a tier or a race: "shepherd when the livestock variant is sheep" (§4.2) |
| Inputs | resources and products with a kind: *needs all* (smelter: ore and fuel) or *needs any* (weaver: fibre or wool) | The **Resources** line and §4.1 |
| Outputs | resources and products | §4.1, §4.3 |
| Lowest tier | countryside, town, city, capital | §2.2 |
| Weight per tier | four numbers | How much a higher tier raises the share (a smith 1 / 1.5 / 2 / 2, a merchant 0 / 0.3 / 1 / 1.5) |
| Siting | a weight per biome (16), per elevation type (4), optional bedrock (9) and water-site flags | §5; for sectors 1 and 2 mostly redundant, since the resource amounts already carry the biome |
| Field response | exponent on Prosperity and on Supernatural | §5.2, last two rows |
| Share | starting share of the labour force | §3 index |
| Standing | 1 to 6 | §2.4 |
| Entry | born, apprenticed, appointed, ordained, hired, fallen | §2.5. Decides whether a child inherits the vocation |
| Rank ladder | apprentice, journeyman, master; novice, priest, high priest; recruit, sergeant, captain | Feeds the role (§7.3) |
| Faction links | archetype codes | §6.2 |

The Realised form adds nothing to a vocation: it is a table, not an entity. What is authored is the person, and the vocation is one of the facts the authoring must not contradict.

### 7.2 From a place to its vocation mix

The question of `society-generation.md` §8 is how the vocations of a place follow from its resources, its size and its races. A sketch that uses only facts the Planned and Settled stages already have:

```
weight(v, place) = share(v) · tier(v, place) · land(v, place) · fields(v, place) · races(v, place)
count(v, place)  = labourForce(place) · weight(v, place) / Σ weight(u, place)
```

- **place** is a land patch (countryside) or a Prospect that became a town or a city. A Prospect reads a catchment, not its own patch: the patches within reach on the central-place graph of `map-generation.md` §15.2, weighted by distance as `acc` is.
- **labourForce** is about 43 % of the persons of the place, by the rates of [BCL] (§2.6). How the potential population of a region is split between its Prospects and the land is the open question of `society-generation.md` §5, and this sketch needs its answer.
- **tier** is 0 below the vocation's lowest tier.
- **land** is 1 for the vocations that follow people. For sectors 1 and 2 it is the amount of the input resources in the place: the product of the amounts for *needs all*, the largest for *needs any*, and 0 below the listing threshold of 0.25. A smelter then exists only where ore and fuel meet, and a salter only on salt.
- **fields** raises or lowers by Prosperity and Supernatural at the place.
- **races** is the mean affinity of the persons of the place: `Σ share(r, place) · affinity(r, v)`, with the race shares of `society-generation.md` §3.2, an affinity of 1 by default, 2 or 3 where §6.1 names the vocation, and 0 for a "never".

Four refinements:

1. **Demand follows the catchment, supply follows the land.** The baker, the tailor and the priest scale with the persons in reach. The tanner and the weaver scale with the hides and the fibre in reach. The merchant, the carter, the sailor and the innkeeper scale with the trade factor of the Prospect (`map-generation.md` §15.3) and its links in the travel network.
2. **A floor for the central functions.** Every town has at least one baker, smith, priest and innkeeper, whatever the rounding says. This is Christaller's threshold read backwards (`game-resources-and-trade.md` §4.5).
3. **Chain closure makes trade.** If a place has smiths and no smelter in reach, iron is imported, and the share of merchants and carters rises. If it has a smelter and no smith, iron is exported. The pass is a table lookup over §4.1 and needs no simulation. It gives every town a line such as "exports salt fish, imports iron and wine" for the authoring.
4. **Rounding without a random stream.** The counts can be rounded by largest remainder in the order of the list, so the mix of a place needs no hash. A hash is needed only when a person is drawn: `hash(seed, person identity, purpose)` picks a vocation from the counts of the person's home, and the purpose number would have to be assigned in `society-generation.md`.

**A worked example.** A median region holds 35,100 persons, so about 15,100 in the labour force. The starting shares of §3 give:

| Vocation | Count | Vocation | Count | Vocation | Count |
|---|---|---|---|---|---|
| Farmer | 5,290 | Miller | 150 | Scribe | 60 |
| Labourer | 1,660 | Smith | 150 | Charcoal burner | 45 |
| Herder | 1,210 | Mason | 150 | Thief | 45 |
| Servant | 1,190 | Noble | 150 | Bandit | 38 |
| Weaver | 530 | Soldier | 150 | Smelter | 30 |
| Gardener | 450 | Hunter | 150 | Salter | 30 |
| Priest | 380 | Innkeeper | 120 | Shipwright | 30 |
| Fisher | 300 | Official | 120 | Entertainer | 30 |
| Forester | 225 | Miner | 105 | Seer | 30 |
| Carpenter | 225 | Baker | 90 | Jeweller | 23 |
| Shopkeeper | 225 | Guard | 90 | Scholar | 23 |
| Beggar | 225 | Butcher | 75 | Artist | 15 |
| Brewer | 180 | Healer | 75 | Courtesan | 15 |
| Carter | 180 | Merchant | 75 | Glassmaker | 8 |
| Sailor | 180 | Quarryman | 60 | Alchemist | 8 |
| Leatherworker | 180 | Tanner | 60 | Mage | 5 |
| Tailor | 180 | Potter | 60 | Artificer | 3 |

Two checks. The 150 millers match one mill per 46 households [DB] at 4.5 persons a household (170 mills). The 150 smiths are six times Ross's 23 (35,100 persons at one per 1,500 [MDME]), which is the size of the correction his critic asks for [RAVEN]. The real region will differ from this table in every row: a landlocked region has no sailors, a region without a deposit no miners, and that difference is the point of the model.

### 7.3 Vocation and role

`society-generation.md` §8 asks whether the vocation and the role of a person's Stub are one fact or two. The sources say two, and arguably three.

- The medieval records keep them apart. Domesday gives the status and almost never the work [DB-HULL]. The poll tax of 1379 grades by rank first and by trade second [POOS]. The Statute of Additions of 1413 asks for "Estate or Degree, or Mystery" [POOS]. The guilds add a rank inside the vocation: apprentice, journeyman, master [GUILD].
- The games keep them apart. In Crusader Kings III a court position (Seneschal, Physician, Bodyguard) is an office that a character holds, not what the character is [CK3-C]. In Dwarf Fortress a profession is only "a reflection of the highest skill" and does not decide what a dwarf does [DF-L] [partially verified]. The tabletop games file former work under a background that gives a lore skill and a tool, beside the character's class [DND-B] [PF-B]. The background list of Worlds Without Number is reported to mix work with status (it has Barbarian and Slave) [WWN-B] [partially verified], which is the confusion to avoid.
- §6.1 shows the same in this repo: chief, hearth-mother, larder-lord and head of a house are in the races' vocation lines, and they are positions, not work.

Three options:

| Option | The Stub holds | For | Against |
|---|---|---|---|
| A. One fact | role = vocation | Simplest. Enough for "a smith" | No master and apprentice, no office in a faction, no slave who is a smith. Noble and servant must be vocations |
| B. Two facts (recommended) | vocation (one of the list, with a variant) and role (the person's position: in the household, in the vocation, in a faction) | Matches the sources. A faction rank (`factions.md` §5.1) and a guild rank are both roles. A slave smith is vocation Smith, role slave | Two draws and a table of which roles fit which vocations |
| C. Three facts | status (estate), vocation, office | Closest to the Statute of Additions | Status mostly follows from the vocation's standing and the family; a third fact buys little |

With option B, Noble, Servant and Beggar stay in the vocation list, so that every adult has exactly one vocation, and the fine grain (knight or baron, steward or scullion) is the role. A child has no vocation, or the house's as an apprentice. Whether the readme's list of Stub facts then gains "vocation" beside "role" is for the user: the readme's tree already says "Person with a vocation and looks", while the three places that list the Stub facts name the role and not the vocation.

### 7.4 A list in the Parameter Set?

The readme says the Parameter Set "holds the lists that are expected to change", and names the bedrock types and the races. The vocations fit that description in phase 1: the shares and the siting weights are exactly what will be tuned against generated regions. Three points to weigh:

- **Size.** Fifty-one records with a weight per biome are about 800 numbers before the rest. Most of them are redundant for sectors 1 and 2 (§7.1), so the record can be small: id, name, sector, tier, share, standing, inputs, outputs, two field exponents.
- **What the code must know.** The chain closure of §7.2 reads inputs and outputs by resource id. If those are data, the code needs no vocation by name, as it needs no race by name.
- **Where the race affinities live.** `adding-a-race.md` lists every place that must change with the default race list. If each race carries its own optional map of vocation id to factor, adding a race touches one record. If each vocation carries a map of race ids, adding a race touches up to fifty-one.

### 7.5 Open questions for the user

1. **Vocation and role:** option A, B or C of §7.3. If B, should "vocation" be added to the Stub facts in the readme and in `society-generation.md` §1.2 and §7?
2. **The list:** are these 51 the right grain? Candidates to split again: monk from Priest, armourer from Smith, guide from Hunter, dyer from Weaver, boatman from Sailor, banker from Merchant. Candidates to merge: Quarryman into Miner, Glassmaker into Potter, Artist into Jeweller.
3. **Parameter Set:** is the vocation list a list in the Parameter Set like the races (§7.4)?
4. **Products:** will the generator model products (iron, cloth, flour), or does a vocation read the raw resource directly? The second is much simpler and loses the "needs two places" trade of iron, bronze and glass.
5. **New resources (§4.4):** accept Reagents (P1) and Herbs (P2) as generated resources 25 and 26? Arcane materials (P3) as the first consumer of the Supernatural Field? The derived by-products of P4 and P5? The variants of P6 and P7?
6. **Countryside:** the repo has Prospects for cities and towns and no village. Does a rural person belong to a patch? Most of the labour force is rural, so most vocations need this answer.
7. **Density and tiers:** at the default of 6 persons per km² a median region has 35,100 persons, and its largest city may hold a few thousand. Are the tiers relative (countryside, town, city, capital), as assumed here, or head counts?
8. **Race exclusions and affinities:** should a race be able to say "never" (Noonfolk: sailor, miner, sentry; Catfolk: ploughman, miller, baker), and where do affinities live?
9. **Slavery:** the readme allows slaves in a family. Is slave a role (§7.3), and in which regions or among which races does it exist?
10. **Sex and age:** the medieval record gives women 30 % of the paid work and other trades than men [BCL]. Does the fantasy world keep, soften or drop that? The Twofold change sex at about thirty and the Swiftborn live seven-year generations (`races-basis.md`), so any rule by sex or age meets them at once.
11. **By-work:** most country people did several things (the farmer who brews, the smith with a field). One vocation per person, or a main one and a by-work?
12. **Stage:** the mix of a patch needs only the Elevated resources and the potential population (Planned). The mix of a Prospect needs the travel network (Settled). Does the vocation layer split the same way?
13. **The supernatural vocations:** how common are mages, alchemists and seers, and does the Supernatural Field set it? The shares here (0.03 %, 0.05 %, 0.2 %) are placeholders.
14. **Vocation and looks:** `portrait-components.md` fits outfits to races and climates. Should an outfit list per vocation be added there?
15. **Purpose numbers** for every draw, as the readme's randomness rule requires.

### 7.6 Gaps in this research

- **The primary sources were read through editions, indexes and summaries, not in the original.** The Paris roll through an index of by-names made for name research [P1292-SCA], whose glosses are sometimes wrong; the *Livre des métiers* through an encyclopedia article [LDM]; the poll taxes through the economists who used Fenwick's edition [BCL]; Domesday through the Hull project's glossary [DB-HULL]. Géraud, Lespinasse and Bonnardot, Fenwick and Darby were not opened.
- **No verified count of rural crafts.** How many households support one village smith, carpenter or weaver is the number a generator needs most, and no source read here gives it. The poll-tax returns hold it (Fenwick's three volumes); Goldberg's article and the Howden pages [HOWDEN] show the way. The shares of §3 for the rural crafts are therefore calibrated to the sector totals only.
- **The clergy and the nobility** rest on Ross [MDME] and on search excerpts [NOB]. The clerical poll taxes would give the first properly.
- **The Catasto's counts per trade** were not retrieved, only the code list [CAT]. The Brown database can be queried for them.
- **York** is given by trade group only [YORK]. Swanson's *Medieval Artisans* has the crafts one by one and was not read.
- **Dwarf Fortress, Worlds Without Number, Anno 1404 and the Manor Lords official wiki** refused the fetch tool or were not found. Dwarf Fortress has the most detailed labour list of any game and deserves a second pass. The Pathfinder background names were not read from the page.
- **Bannerlord's village production types and Kingdom Come: Deliverance's crafts** were not looked at.
- **The non-European record is thin.** The seven human races are built on peoples of several continents (`races-basis.md`), and the sources here are French, English and Italian. A second pass could add a Chinese, an Indian, an Islamic and a West African occupational source.
- **The lore lines are general knowledge**, written to be generic, and carry no citation.

---

## 8. Sources

### 8.1 History: primary sources and the scholarship that publishes them

- **P1292-SCA**: Colm Dubh (Scott Catledge), "Occupational By-Names in the 1292 Tax Role of Paris", SCA College of Arms, first published in the Proceedings of the Known World Heraldic Symposium 2000. An index, with counts, of the men's occupational by-names in H. Géraud (ed.), *Paris sous Philippe-le-Bel* (Paris, 1837), which prints "Le livre de la taille de Paris pour l'an 1292". https://heraldry.sca.org/names/parisbynames.html. Géraud's edition itself was not read.
- **GIES-BC**: Ben Crowder, "Occupations in 1292 Paris", 2015: the list of 51 trades with counts from Joseph and Frances Gies, *Life in a Medieval City* (Harper and Row), notes, p. 236. https://bencrowder.net/blog/2015/occupations-in-1292-paris/. The book itself was not read.
- **P1292-N**: the number of taxpayers of the 1292 roll (14,566), from a table in a working paper by A. Slivinski and N. Sussman on taxation in medieval Paris. https://econ.biu.ac.il/sites/econ/files/seminars/taxation_and_growthmarch2010.pdf **[partially verified: search excerpt; the index did not say which result carried the figure]**
- **LDM**: Étienne Boileau, *Livre des métiers* (c. 1268), read through the French Wikipedia article "Livre des métiers", which describes the edition of R. de Lespinasse and F. Bonnardot, *Les métiers et corporations de la Ville de Paris: XIIIe siècle. Le Livre des métiers d'Étienne Boileau* (Paris, 1879). https://fr.wikipedia.org/wiki/Livre_des_m%C3%A9tiers **[partially verified: encyclopedia article; the edition was not read]**
- **BCL**: S. Broadberry, B. M. S. Campbell and B. van Leeuwen, "When did Britain industrialise? The sectoral distribution of the labour force and labour productivity in Britain, 1381–1851", *Explorations in Economic History* 50(1), 2013, 16–27, doi:10.1016/j.eeh.2012.08.004. Read as the accepted manuscript. https://pureadmin.qub.ac.uk/ws/portalfiles/portal/1640964/EEH_Sectoral_shares_paper.pdf. Its 1381 data are from C. C. Fenwick (ed.), *The Poll Taxes of 1377, 1379 and 1381*, 3 parts (Oxford, 1998, 2001, 2005), which was not read.
- **HOWDEN**: "Howden: urban or rural?", pages on the poll tax of 1379 in Howdenshire, built on P. J. P. Goldberg, "Urban identity and the poll taxes of 1377, 1379, and 1381", *Economic History Review* 43(2), 1990, 194–216. https://sites.google.com/site/howdenmiddleages/home/poll-tax-of-1379---introduction/howden-urban-or-rural. Goldberg's article itself was not read.
- **POOS**: L. R. Poos and Martha D. Rust, "Of Piers, Polltaxes and Parliament: Articulating Status and Occupation in Late Medieval England", *Fragments* 5 (2016), as reposted by Brewminate, 25 April 2017. The first 100,000 characters were read. https://brewminate.com/of-piers-polltaxes-and-parliament-articulating-status-and-occupation-in-late-medieval-england/
- **DB**: Wikipedia, "Domesday Book" (recorded households, slaves, mills and Duby's ratio after Gies and Gies 1994, Postan's caveat). https://en.wikipedia.org/wiki/Domesday_Book
- **DB-HULL**: Hull Domesday Project, glossary pages "Villager" and "Occupations and titles". https://sites.google.com/site/domesdaymaterials/domesday-book/data-terminology/peasantry/villager, https://sites.google.com/site/domesdaymaterials/domesday-book/data-terminology/occupations-titles. Only part of each page was returned by the fetch tool.
- **DB-X**: further Domesday claims (smallholders about a third, slaves under a tenth, 95 % peasantry, potters on one manor, six smiths in one city, salt-making centred on one town), from the Hull Domesday Project pages "Peasants" and "Industries" and from a source book of excerpts. https://sites.google.com/site/domesdaymaterials/domesday-book/data-terminology/peasantry/peasants, https://sites.google.com/site/domesdaymaterials/domesday-book/data-terminology/borough-castle/industries **[partially verified: search excerpts]**
- **DB-LIFE**: domesdaybook.co.uk, the page on life in 1086 (mills, fisheries, the shares of the land held). https://mail.domesdaybook.co.uk/life.html. A popular summary.
- **CAT**: D. Herlihy and C. Klapisch-Zuber, the data file of the Florentine Catasto of 1427 to 1429, codebook of the online edition, Brown University. https://cds.library.brown.edu/projects/catasto/newsearch/catasto_codebook.html
- **YORK**: P. M. Tillott (ed.), *A History of the County of York: the City of York* (Victoria County History, 1961), "The later middle ages: Economy and industrial prosperity", pp. 84–91, British History Online. https://www.british-history.ac.uk/vch/yorks/city-of-york/pp84-91
- **SEN**: "Manorial Management and Organization, c. 1275": extracts from the *Seneschaucy*, the *Hosebonderie*, Walter of Henley and the Rules of Robert Grosseteste, trans. E. Lamond, *Walter of Henley's Husbandry* (London, 1890), in the Internet Medieval Sourcebook, Fordham University. https://sourcebooks.fordham.edu/source/1275manors1.asp
- **REEVE**: Wikipedia, "Reeve (England)", citing H. S. Bennett, *Life on the English Manor* (1938). https://en.wikipedia.org/wiki/Reeve_(England)
- **OBED**: Wikipedia, "Obedientiary", after the *Catholic Encyclopedia* (1913), "Obedientiaries". https://en.wikipedia.org/wiki/Obedientiary
- **GUILD**: *Encyclopaedia Britannica*, "Guild (trade association)". https://www.britannica.com/topic/guild-trade-association
- **DYER**: C. Dyer, "Small towns 1270–1540", in D. M. Palliser (ed.), *The Cambridge Urban History of Britain*, vol. 1 (Cambridge, 2000). https://www.cambridge.org/core/books/cambridge-urban-history-of-britain/small-towns-12701540/E58FDA2AE278959341001729657B56BE **[partially verified: search excerpt]**
- **CLERGY**: A. K. McHardy (ed.), *Clerical Poll-Taxes of the Diocese of Lincoln, 1377–1381* (Lincoln Record Society), publisher's description. https://lincoln-record-society.org.uk/publications/clerical-poll-taxes-of-the-diocese-of-lincoln-1377-1381/ **[partially verified: search excerpt]**
- **NOB**: "Class, Status, and Order", Encyclopedia.com (nobles as 1 to 2 % of most European societies, about 10 % in Castile and Poland-Lithuania). https://www.encyclopedia.com/history/encyclopedias-almanacs-transcripts-and-maps/class-status-and-order **[partially verified: search excerpts]**
- **IRON**: charcoal and wood per kilogram of bloomery iron, from reviews of experimental smelts. https://pmc.ncbi.nlm.nih.gov/articles/PMC7661058, https://hmsjournal.org/index.php/home/article/view/682/678 **[partially verified: search excerpt; the index did not say which of the two carried the figures]**
- **ROSSIAUD**: Jacques Rossiaud, *La prostitution médiévale* (Paris, 1988), English as *Medieval Prostitution*, trans. L. G. Cochrane (Blackwell), from the publisher's description and from the summary in "Brothels and Stewes: the Late Medieval Sex Trade in Europe", Brewminate. https://brewminate.com/brothels-and-stewes-the-late-medieval-sex-trade-in-europe/ **[partially verified: search excerpts; the book was not read]**
- **STEWS**: Wellcome Collection, "The bishop's profitable sex workers", and Southwark News, "The bishop's brothels". https://wellcomecollection.org/stories/the-bishop-s-profitable-sex-workers, https://southwarknews.co.uk/history/in-depth-history/the-bishops-brothels/ **[partially verified: search excerpts. The accounts differ on the date (1161 or 1162) and on the number of rules (36 or 39), and the count of eighteen houses is a traditional figure]**
- **VEN-CAT**: "A Sexual Tour of Venice: Mapping a Sixteenth-Century Catalogue of Courtesans", History Workshop, 5 January 2022. https://historyjournal.org.uk/2022/01/05/a-sexual-tour-of-venice-mapping-a-sixteenth-century-catalogue-of-courtesans/ **[partially verified: search excerpt]**

### 8.2 Games and tabletop

- **MDME**: S. John Ross, "Medieval Demographics Made Easy", file v1.10, © 1993, 1999–2018 (Cumberland Games & Diversions). Read from the PDF. https://www.martinralya.com/wp-content/uploads/2021/05/Medieval-Demographics-Made-Easy.pdf. Its bibliography says: "The SV list was taken almost entirely from the Paris tax list of 1292, with tweaks from elsewhere", found "in truncated form" in the Gies book.
- **RAVEN**: "Medieval Demographics Done Right", Ravenswing blog, October 2013: a critique of [MDME] from the Paris roll. https://ravenswing59.blogspot.com/2013/10/medieval-demographics-done-right.html
- **BAN-P**: Banished Wiki, "Professions". https://banished-wiki.com/wiki/Professions
- **ML-B**: Game8, "Manor Lords: list of all buildings", updated 3 May 2024. https://game8.co/games/Manor-Lords/archives/451055. A guide; the official Hooded Horse wiki answered 403.
- **MD-P**: pindrop.gg, "Medieval Dynasty: skills", from the game data of version 2.7.0.4. https://pindrop.gg/medieval-dynasty/skills
- **RW-W**: RimWorld Wiki, "Work". https://rimworldwiki.com/wiki/Work
- **MB2-W**: TaleWorlds, *Mount & Blade II: Bannerlord* developer blog "Productive Enterprises", 11 May 2018, read as reposted by GameBanshee. http://www.gamebanshee.com/k46hy
- **CK3-C**: Crusader Kings III Wiki, "Court", court positions (verified for version 1.20; the first 100,000 characters were read). https://ck3.paradoxwikis.com/Court
- **DND-T**: Wizards of the Coast, *D&D Basic Rules (2014)*, chapter 5 "Equipment" (tools, lifestyle expenses, hirelings), D&D Beyond. https://www.dndbeyond.com/sources/dnd/basic-rules-2014/equipment
- **DND-B**: the same, chapter 4 "Personality and Background" (Acolyte, Criminal, Folk Hero, Noble, Sage, Soldier). https://www.dndbeyond.com/sources/dnd/basic-rules-2014/personality-and-background
- **PF-B**: Archives of Nethys (Paizo-licensed), *Pathfinder* second edition, "Backgrounds" (Player Core p. 84: a background trains two skills, one of them a Lore). https://2e.aonprd.com/Backgrounds.aspx. The names of the single backgrounds were not returned and are **[unverified]** where cited.
- **DF-L**: Dwarf Fortress Wiki, "Labor" and "Profession". https://dwarffortresswiki.org/DF2014:Labor **[partially verified: search excerpts; the wiki answered 429]**
- **WWN-B**: Kevin Crawford, *Worlds Without Number* (Sine Nomine, 2021), the background list on p. 11, as reported by a play group's character-creation notes. https://wiki.rpg.net/index.php/Exploring_the_Halls_of_Arden_Vul:_WWN_chargen **[partially verified: search excerpt]**
- **ANNO-1404**: *Anno 1404* production chains (bread, beer, leather jerkins), from a guide. https://www.ludo.guide/guide/anno-1404/resources **[partially verified: search excerpt]**

### 8.3 Repository

- `readme.md`, `docs/society-generation.md`, `docs/map-generation.md` (sections 9, 13 to 16, Appendices B, C, D, F, G) and `docs/races-basis.md`, working tree at commit `2c59ebe`.
- `docs/research/factions.md` (§3 archetype list, §4.1, §5), `docs/research/game-resources-and-trade.md` (§5.2, §5.3), `docs/research/patch-resources.md` (rows #93, #172, #173, #312, #315, #351 and F1 to F28).
