# Faction Generation: How the Kind of Ruler, Opposition and Guilds of a Land Can Be Calculated

*Research date: 2026-11-19. Scope: how the **kind** of ruling faction, of opposition faction and of guild-type factions of one territory can be **calculated** from its land, its people and its wealth. `factions.md` already lists 48 faction archetypes, their axes and their relations; this note does not redo that. It covers what that note lacks: the rules. §1 gives the findings that matter most. §2 reads the generators and games that compute a polity or a faction from terrain, culture or wealth, with their real weights. §3 reads the scholarship on what makes one kind of ruler arise. §4 does the same for oppositions, with the fractionalization and polarization formulas. §5 covers guilds. §6 is the summary table from input signal to outcome. §7 lists what could not be verified. §8 lists the sources.*

**How to read the citations**

- Each claim cites the source that owns it, by key in square brackets. The keys are listed in §8. Keys that `factions.md` already defines (GUILD-B, HANSA, COMMUNE, RW-F, BITD-FG) are reused and not listed again.
- **[partially verified]** means the source exists and is cited, but the claim was confirmed only from a search-index excerpt, an abstract or a secondary source, not from the text itself.
- **[unverified]** means general knowledge that was not checked against any source in this pass.
- Prose is paraphrased, not quoted. Numbers, formulas, code constants and raw tokens are given as the source has them.
- For three papers the text read is the working-paper version, not the journal version. The source list says which.
- Every paragraph headed **Rule a generator could take**, all of §6.2 and every "What to take" line are my own engineering judgement. They map a finding onto the inputs below; they are not sourced claims.

**Fetch notes.** `ck3.paradoxwikis.com` refused the fetch tool and plain HTTP (a script check), so its pages were read through a browser. `is.muni.cz` (course copies of two papers) did the same. `stanford.edu` failed on its certificate. `cambridge.org`, `journals.uchicago.edu` and `aeaweb.org` gave only abstracts or citation data. `bladesinthedark.com` returned 404 for the crew page. The Dwarf Fortress wiki answered this time, as raw wikitext. The Azgaar file named in the task (`modules/burgs-and-states.js`) no longer exists; the repository was restructured into TypeScript and the code is now in `src/model/political/states-generator.ts`.

**Inputs the rule may read** (from the task brief and `readme.md` at commit `f8c25e6`)

| Input | What it is |
|---|---|
| Biome shares | 15 land biomes and Mangrove, one per land patch (8 km) |
| Elevation type shares | Flat Land, Lowland, Hills, Mountains |
| Bedrock | per zone |
| Resources | 27 raw resources per patch, each with an amount and a variant. **Grain** and **Produce** are separate resources; Produce holds the roots and tubers (potato, cassava, yam, taro). Gold has lode and placer forms, tin has lode and stream forms (`map-generation.md` §14) |
| Fields | Prosperity and Supernatural, 0 to 1 |
| Races | up to six per region with a head count each; the largest holds 19 to 43 %, never a majority |
| Prospects | 20 per region, up to 4 for cities; score; class Rich, Average or Poor; facts: coast, harbour, navigable river, river mouth, distance to the region border |
| Sea access | yes or landlocked |

The rule must be a pure function of the region's own data. That excludes every finding that needs a neighbour; where a source needs one, the text says so.

---

## 1. Overview

Eight findings shape the rule.

1. **Only one generator publishes real weights for the form of government, and terrain barely enters.** Azgaar's Fantasy Map Generator draws Monarchy : Republic : Union at 25 : 2 : 1 for every state except a Naval one, which gets 25 : 8 : 3. Theocracy comes from the religion at the capital (always for a state religion, else a 10 % roll for an organized religion or a cult) and Anarchy is a 1 % roll. Terrain acts only through the culture type, and only "Naval" changes the form [AZ-STATES]. So the common practice is: monarchy is the default at about 70 to 90 %, and one or two land signals shift the rest.
2. **Dwarf Fortress ties the kind of polity to the people, and the people to a start biome.** Government structure, guilds, merchant companies and banditry all follow from tokens and value thresholds of the entity, not from wealth [DF-RAW] [DF-ET]. This is the model for "the race decides how the faction is built".
3. **The most testable rule in the scholarship is cereals against roots.** Hierarchy follows crops that can be seized and stored, not fertile land as such. One standard deviation more cereal advantage raises the hierarchy level by 0.27 on a five-level scale; land productivity adds nothing once cereals are in the model [MMP]. The repo already separates Grain from Produce, so this input exists.
4. **Rough and marginal land is weak-state land.** Hills above the valleys, marsh, steppe and desert are where states did not reach [SCOTT-ANBG] [SCOTT-ATG] **[partially verified]**. The same terrain is the base of insurgency: the ten-year civil war risk goes from 6.5 % for a flat country to 13.2 % for one that is about half mountainous [FL].
5. **Rule by merchants and rule by a church are minority forms with clear triggers.** Assemblies that merchants control survive in geographically small polities, and those with the best credit were the most closed and oligarchic [STAS]; a port or trade node with a weak land base is the place for one. The Holy Roman Empire, the extreme case for church rule, had 53 ecclesiastical principalities on the eve of the Reformation, against about three hundred estates in all [PB] [IE].
6. **Diversity does not predict an opposition; polarization does.** Ethnic and religious fractionalization are insignificant for civil war onset [FL] and even lower the risk [CH]. The polarization index RQ is significant: one standard deviation (0.24) more raises the probability of conflict by 67 % [MRQ]. Because no race in a region is a majority, the "dominant group" tests of the literature never fire here, and RQ over the race shares is the usable signal (§4.2).
7. **The kind of opposition follows where the wealth lies and which estate is second.** Concentrated wealth near the centre gives a coup, concentrated wealth far away gives secession, spread wealth near the centre gives mass rebellion, spread wealth far away gives warlords [LB]. The medieval estates (clergy, nobility, burghers, in some realms peasants) [EST] and Lipset and Rokkan's four cleavages (centre against periphery, state against church, land against industry, owner against worker) [LR] give the same small, closed set of rivals. Crusader Kings III adds the threshold: a faction presses its demand when its strength is above 80 % of the ruler's [CK3-SUB].
8. **Guilds follow towns, not wealth, and their number is not proportional to size.** By the thirteenth century most European towns had them. Florence with 100,000 people had 21 guilds in 1300; Fulda with 8,500 had 21 in 1784. Merchant guilds came first, craft guilds about a century later, and primary production was rarely guilded [OG14].

---

## 2. Generators and games

### 2.1 Azgaar's Fantasy Map Generator

Read from the source at `master`, commit `7413629` (2026-11-16). All numbers below are from the code [AZ-STATES] [AZ-CULT] [AZ-REL] [AZ-BURG] [AZ-BIOME].

**Step 1: the culture type comes from the terrain at the culture's centre cell.** The tests run in this order and the first hit wins [AZ-CULT]. Height runs 0 to 100 with the sea below 20. Biome indices are those of [AZ-BIOME]: 1 hot desert, 2 cold desert, 3 savanna, 4 grassland, 5 tropical seasonal forest, 6 temperate deciduous forest, 7 tropical rainforest, 8 temperate rainforest, 9 taiga, 10 tundra, 11 glacier, 12 wetland.

| Order | Culture type | Condition at the centre cell | Base expansionism |
|---|---|---|---|
| 1 | Nomadic | height < 70 and biome is hot desert, cold desert or grassland | 1.5 |
| 2 | Highland | height > 50 | 1.2 |
| 3 | Lake | the nearest water body is a lake of more than 5 cells | 0.8 |
| 4 | Naval | a harbour value above 0 on water that is not a lake, with 10 % chance; or a harbour value of exactly 1, with 60 %; or on an isle, with 40 % | 1.5 |
| 5 | River | on a river with flux above 100 | 0.9 |
| 6 | Hunting | more than 2 cells from the coast and biome is savanna, tropical rainforest, temperate rainforest, taiga, tundra or wetland | 0.7 |
| 7 | Generic | everything else | 1.0 |

The type then steers how the culture spreads: Nomads pay a tenfold cost in forest biomes and a huge one for crossing water, Highlanders pay 3,000 on lowland, River cultures pay 100 off a river, Naval cultures cross the sea cheaply [AZ-CULT].

**Step 2: the state type comes from the capital.** A state is Nomadic if its capital cell is hot desert, cold desert, savanna or grassland. Otherwise it takes the type of the capital's culture, except that a Nomadic culture outside those biomes gives a Generic state [AZ-STATES].

**Step 3: the size tier.** `tier = min(floor(area / median area × 2.6), 4)`. Tier 4 also needs the state to be among the largest: the cut is the area of the state at rank `ceil(n^0.4) − 2` (zero-based, largest first), with `n` the number of states. A state below it falls back to tier 3 [AZ-STATES].

**Step 4: the form of government** [AZ-STATES].

| Test, in order | Result |
|---|---|
| The religion at the capital cell has expansion "state" | Theocracy |
| Else 10 % chance, if that religion is of type Organized or Cult | Theocracy |
| Else chance `0.01 − tier / 500` (1 % at tier 0, 0.2 % at tier 4) | Anarchy |
| Else, state type Naval | weighted draw Monarchy 25, Republic 8, Union 3 (69 %, 22 %, 8 %) |
| Else | weighted draw Monarchy 25, Republic 2, Union 1 (89 %, 7 %, 4 %) |

A religion has expansion "state" when its name was built from a place name. For a polytheist religion that is 7 of 20 naming weights [AZ-REL]. Of the non-folk religions, 10 to 40 % are cults and the rest organized [AZ-REL].

**Step 5: the title** [AZ-STATES].

| Form | Title |
|---|---|
| Monarchy | by tier: Duchy, Grand Duchy, Principality, Kingdom, Empire. The name base of the culture replaces some (Khanate, Tsardom, Shogunate, Emirate, Horde and so on). A vassal can become Marches, Dominion or Protectorate |
| Republic | with tier below 2 and exactly one burg: Free City when the state is named after its capital, else City-state at 30 %. Otherwise a weighted draw: Republic 75, Federation 4, Trade Company 4, Most Serene Republic 2, Oligarchy 2, Tetrarchy 1, Triumvirate 1, Diarchy 1, Junta 1 |
| Union | weighted draw: League 4, Union 3, United Provinces 2, Confederation 1, United Kingdom 1, United Republic 1, Commonwealth 1, Heptarchy 1 |
| Theocracy | weighted draw: Theocracy 15, Thearchy 2, Brotherhood 1, See 1, Holy State 1; with name-base variants (Diocese or Bishopric below tier 2, Eparchy, Exarchate, Patriarchate, Imamah, Caliphate) |
| Anarchy | weighted draw: Council 3, Free Territory 2, Commune 1, Community 1 |

When the development at the capital is below 12, the title becomes tribal: a Monarchy is a Horde if the state is Nomadic, else Tribe (tier below 2), Chiefdom (tier 2 or 3) or Paramount Chiefdom (tier 4); a Republic is a Tribal Council, a Union a Clans Union, a Theocracy a Shamanate [AZ-STATES].

**The form then drives money and buildings.** Base taxes by form: Monarchy sales 8 %, poll 7 %; Theocracy 5 % and 7 %; Republic 5 % and 5 %; Union 10 % and none; Anarchy none. Base corruption: Monarchy 15, Theocracy 12, Republic 8, Union 4, Anarchy 0. Court spending: Monarchy 20, Theocracy 30, Republic and Union 5 [AZ-STATES]. A burg gets a temple above population thresholds (always above 50, 75 % above 35, 50 % above 20), or at 50 % in any theocracy that has a religion at the cell. Walls, citadel and shanty town use the same kind of threshold on population [AZ-BURG].

**What Azgaar does not do.** Wealth, resources and the make-up of the population do not enter the form. There is no opposition and no guild. Diversity enters only later, as a corruption term: ruling people of another culture and faith adds up to 10 points [AZ-STATES].

**What to take.** (1) The structure "default monarchy, a few signals shift the weights" with the default near 70 to 90 %. (2) Coast triples to quadruples the republic weight. (3) Size tier is separate from form. (4) One development threshold swaps the whole title set to tribal. (5) The ruling faith needs its own input; Azgaar uses "is the faith tied to this state", which for this repo could be the Supernatural Field.

### 2.2 Dwarf Fortress

Read from a mirror of the vanilla raws [DF-RAW] and from the wiki's token pages [DF-ET] [DF-VP] [DF-GUILD] [DF-MC] [DF-MON]. The mirror's last change is February 2020, which fits the 0.47 line **[version unverified]**.

**The entity decides everything, and the entity is tied to a biome.** Each entity in `entity_default.txt` names where a civilisation may be born, which sites it builds, how fast it spreads over each biome, its religion type, its positions and its values [DF-RAW].

| Entity (creature) | Start biome | Site type | Biome support (relative spread rate) | Religion | Positions | Banditry | Values that matter |
|---|---|---|---|---|---|---|---|
| MOUNTAIN (dwarves) | `EXCLUSIVE_START_BIOME:MOUNTAIN` | `CAVE_DETAILED` | mountain 3; forest, grassland, savanna, shrubland, river 1 | PANTHEON | fixed list: monarch, duke, count, baron, mayor, general, sheriff and more | none | craftsmanship 50, cooperation 15, knowledge 15, commerce 15, power 0 |
| FOREST (elves) | `START_BIOME:ANY_FOREST` | `TREE_CITY` | forest 3; lake, river 1 | REGIONAL_FORCE | fixed list: druid, acolyte, leader, general, ranger captain, diplomat | none | craftsmanship 0, cooperation 0, knowledge 0, commerce −5 |
| PLAINS (humans) | grassland, savanna, shrubland | `CITY` | ocean 12; river 4; lake, grassland 3; forest, savanna, shrubland 2; wetland, desert 1 | PANTHEON | `VARIABLE_POSITIONS:ALL`, `SITE_VARIABLE_POSITIONS:ALL` | `BANDITRY:10` | `VARIABLE_VALUE:ALL:-30:30`: rolled per civilisation |
| EVIL (goblins) | `ANY_LAND` | `DARK_FORTRESS` | all main biomes 1; river 1 | none set | all variable | `BANDITRY:50`, `LOCAL_BANDITRY` | power 50, cooperation −15, knowledge −15 |
| SKULKING (kobolds) | mountain, not freezing | `CAVE` | wetland 3, mountain 3; forest, lake 2; others 1 | none set | none | `BANDITRY:10`, `LOCAL_BANDITRY` | cooperation 30; population cap 2,000 instead of 10,000 |

The wiki explains the tokens. `CAVE_DETAILED` civilisations build fortresses in mountainous regions and hillocks elsewhere. `DARK_FORTRESS` gives the civilisation a demon overlord. The number of free start-biome tiles limits how often an entity appears, so an entity with a rare start biome is rare. `REGIONAL_FORCE` makes the people worship one force tied to the terrain of their first biome; `PANTHEON` gives a group of gods. `BANDITRY` is the percentage of the population used as bandits [DF-ET].

**Government differs by entity.** Dwarves and elves have fixed position lists. Humans and goblins have variable positions: world generation invents a law-maker first and then officials and market officials for the civilisation, and lords and similar for each site [DF-VP]. So two human realms can have differently named and differently built governments, while every dwarven realm has a monarch with dukes, counts and barons.

**Guilds, companies and religious orders form from thresholds.**

| Group | Condition | Source |
|---|---|---|
| Craft guild in world generation | the civilisation values COOPERATION at 15 or more and CRAFTSMANSHIP at 15 or more, and has guild-valid professions among its permitted jobs | [DF-ET] |
| Library | KNOWLEDGE at 15 or more | [DF-ET] |
| Merchant company | the entity has `MERCHANT_NOBILITY` (only humans in vanilla); the headquarters, a counting house, is usually founded in a town; branches follow trade routes into other civilisations | [DF-ET] [DF-MC] [DF-RAW] |
| Guild in a player fortress | 10 members of one profession, by default; a grand guildhall is asked for at 25 | [DF-GUILD] |
| Monastic order | builds a monastery and is headed by an abbot, the only position of a religious entity that rules a site | [DF-MON] [DF-VP] |

With the vanilla values, dwarves (50 and 15) always can form craft guilds, elves and goblins never can, and humans can only when both rolled values reach 15 [DF-RAW]. A generated guild always has four positions: a leader, an alderman, a steward and a clerk. A merchant company has a leader who is either elected or hereditary, fixed per company, and one factor per branch [DF-GUILD] [DF-VP].

**What to take.** (1) Give each race a small set of faction-relevant values and let thresholds on them decide which institutions it can form at all. (2) Let the race, not the land, decide how a government is built (fixed ladder or invented offices); the land already chose the race through the biome frequencies. (3) A banditry share per race is a one-number source of outlaw factions.

### 2.3 Crusader Kings III

Read from the wiki at game version 1.21 [CK3-GOV] [CK3-SUB].

**Governments.** The page lists Tribal, Feudal, Clan, Theocratic, Mandala and Wanua as landed governments and names eleven more on their own pages (Adventurer, Merchant, Republican, Ecclesiastical, Administrative, Nomadic, Celestial and others), which were not read.

| Government | What makes it different | What decides it, as far as the page says |
|---|---|---|
| Tribal | vassals owe taxes and levies by the liege's level of fame; men-at-arms are paid in prestige; no holdings or innovations past the tribal era; partition succession | set at the start; changed only by decision |
| Feudal | holdings, cadet branches, crown authority law | the default of settled Christian Europe **[unverified]** |
| Clan | vassals pay through tax jurisdictions run by a tax collector; strong vassals want an alliance with the liege and lose 30 opinion without one; a house unity value from 0 to 200 | Muslim realms **[unverified]** |
| Theocratic | holds temples; the realm priest gives all taxes and levies whatever the opinion; a theocratic vassal gives a liege of another faith 10 % | clergy of a faith with the theocratic clerical tradition that is not Christian |
| Mandala | title creation costs piety; domain and vassal limit capped at 4; a radiance value that falls with distance from the capital | only rulers of a polytheist faith; converting to a monotheist one turns it Feudal |
| Nomadic | not on the page read | herders become nomadic; a county with a nomadic or herder capital holding can rise as a Nomads faction and form a nomadic realm [CK3-SUB] |

A government changes only by decision, and a ruler who inherits a higher realm takes that realm's government [CK3-GOV]. The game has no formula from terrain to government: the start is authored in the history files **[unverified]**. What the game does encode is that the **government type is the rule for what subjects owe**: fame, contract, kin alliance or faith.

**Factions** [CK3-SUB].

- A faction has two values. **Military power** is the strength of its members divided by the strength of the liege. **Discontent** rises while power is above 80 % and falls while it is below. At 100 % discontent the faction sends its ultimatum; refusal starts a civil war.
- A vassal cannot join when opinion of the liege is 80 or more, or when the liege holds a strong hook, an alliance or a truce with them.
- **Vassal factions:** Liberty (lower the crown's authority), Independence (members leave), Dissolution (the primary title is destroyed), Claimant (another person takes the title).
- **Popular factions** arise by themselves from counties with negative popular opinion. Populists need a county of another faith or culture than the ruler; the liege must convert or let the counties go. Peasant Rabble lists no such difference. A third kind, Nomads, turns its counties into a nomadic realm. Their power is the counties' levies against the domain's levies. The requirement column of this table lost its icons in my extract, so the holding conditions are not given here.
- A liege who accepts an ultimatum loses 20 dread and legitimacy; one who wins the war gains 20 dread.

**What to take.** (1) The four vassal demands are a ready typology of what a noble opposition wants: less crown, out, break it up, another king. (2) Whether an opposition is *active* can be one ratio with a threshold near 0.8. (3) A popular opposition needs a difference of faith or people between ruler and county, or plain misery.

### 2.4 Ultima Ratio Regum

The developer describes civilisations as a set of policy choices in eight categories, each of which puts buildings and people into the generated cities [URR-ID].

| Category | Options |
|---|---|
| Leadership | Representation, Stratocracy, Monarchy, Theocracy |
| Religious | Collective Faith, Organized Religion, Religious Freedom, Zealotry, Monastic |
| Military | Conscription, Standing Army, Militia, Pacifism, Vassalage |
| Trade | Free Trade, Mercantilism, Planned Economy, Protectionism, Barter |
| Foreign | Exploration, Isolationist, Internationalist, Interventionist, Imperialist |
| Intellectual, Cultural, Justice | five, four and five options |

Examples of the effect: Representation puts a parliament in the city centre, Theocracy puts archives under the cathedral, Vassalage replaces village halls with manors, Monastic puts monasteries outside the cities. The post does not say how a civilisation gets its policies, so the choice is presumably random **[unverified]**.

**What to take.** The direction of the arrow: the faction type decides what is built and who lives in the capital. That is the reverse of this note's question, and it is the reason the ruler type is worth computing at the Planned stage.

### 2.5 Caves of Qud

A generated village usually has one mayor, a warden, a tinker, an apothecary and a merchant. A less common type is parliamentary, with about three parliamentarians in place of the mayor and a history event about the founding of the parliament. Each village gets two generated history events, and each event has a 1 in 20 chance of leaving the village abandoned. The number of villages is a fixed average per biome [QUD-V]. The page does not say how the village's people or its government type are chosen; it points to the game code for that **[partially verified: the page marks itself incomplete]**.

### 2.6 Not found

No published rule was found that derives a government from terrain in RimWorld, Songs of Syx, Europa Universalis or the *Without Number* games. *Worlds Without Number* gives a faction the attributes Force, Cunning, Wealth and Magic with hit points and assets, but the game master chooses the faction; nothing is derived from the land **[partially verified: blog summaries only]**. Blades in the Dark has a rule for rivals; it is in §4.6.

---

## 3. What decides the kind of ruler

### 3.1 Cereals against roots and tubers

**Source says.** Mayshar, Moav and Pascali reject the usual account that farming raised output, output gave a surplus, and the surplus gave a state. They argue that hierarchy arose where people came to depend on cereal grains, because grain can be taken [MMP]. Grain ripens in one season, must be stored, and keeps and travels well, so a collector can seize a share and carry it to a distant elite. Roots and tubers are mostly perennial, can stay in the ground, rot soon after harvest and are heavy with water, so they are hard to confiscate and nearly impossible to carry far. Their example is a cassava-growing society that has a surplus and still no hierarchy beyond local chiefs. New Guinea took up farming about as early as Egypt, and the arrival of the far more productive sweet potato in the seventeenth century still left the highlands fragmented [MMP].

The measure is the productivity advantage of cereals over roots and tubers, from potential yields. The outcome is the Ethnographic Atlas variable for levels of authority beyond the local community, with five levels: none, petty chiefdoms, larger chiefdoms, states, large states [MMP].

| Result | Number |
|---|---|
| One standard deviation more cereal advantage, effect on the chance that a cereal is the main crop | about +20 % |
| Same, effect on the hierarchy level (with continent fixed effects) | +0.27 |
| Same, ordered logit: log odds of a higher level | about +50 % |
| Same, by step: above tribe, above chiefdom, above small state | +32 %, +65 %, +84 % |
| Growing cereals as the main crop, instrumented | more than one full level |
| Land productivity, once cereal advantage is included | very small and not significant, or negative |

The effect is strongest for the step from chiefdom to state [MMP]. Scott makes the same point in prose: grain alone is visible, divisible, assessable, storable, transportable and rationable at once, and tubers and legumes each lack some of these [SCOTT-ATG].

**Rule a generator could take.** Compute an *appropriability* score for the region: the Grain amount summed over the patches, divided by the sum of Grain and Produce (and, if wanted, the other food resources). High appropriability on Flat Land and Lowland pushes a strong crown: a dynasty that taxes. Low appropriability with a fair population (root, orchard, fish, game or herd country) pushes rule that stops at the chief: clans, tribes, a council of kin. Livestock is the middle case: herds can be seized and driven, which fits the chiefdom and the horde (§3.4). Do not use the total food amount or the Prosperity Field for this; the paper's point is that plenty alone does not make a hierarchy.

### 3.2 Hills, marsh and the reach of the state

**Source says.** Scott's *The Art of Not Being Governed* treats the highlands of mainland Southeast Asia as land the valley states never held. He sets the line at about 300 metres; the area is about 2.5 million square kilometres with about 100 million people of very many small peoples. His claim is that hill societies are built to keep the state away: their farming, their flat political structure, their prophets and their lack of writing all serve that [SCOTT-ANBG]. Reviews add the mechanics: difficult terrain slows every tax collector and army, so the state's real reach is set by travel cost and not by the claimed border; root crops that stay hidden in the ground suit people who avoid armies; and displaced peoples pile up in such refuges, which makes them very mixed in language and people [SCOTT-ANBG] **[partially verified: reviews]**. *Against the Grain* generalises the zones the early states could not rule to mountains, steppes, marshes and deserts, and notes that their people both raided the grain states and traded with them [SCOTT-ATG]. Critics call the hill thesis too binary and too close to environmental determinism, and note that highlands also founded cities and states [SCOTT-ANBG].

**Rule a generator could take.** Compute a *state space* share: the share of patches that are Flat Land or Lowland, not marsh or dense forest, and rich in Grain. Compute a *refuge* share: Hills and Mountains, Mangrove, rain forest, desert, tundra, steppe. A region that is mostly refuge gets a kin-type ruler by default (clan chiefs, a tribal council, a mountain hold). A region with both gets a crown in the state space and a ready home for an opposition in the refuge (§4.1). Refuge land should also raise the count of small independent factions, since Scott's refuges are mixed.

### 3.3 Size, density and enclosure

**Source says.** Service's scheme runs band, tribe, chiefdom, state. A teaching text gives bands as 20 to 30 people of foragers without formal leaders, and tribes as a few hundred to several thousand herders or gardeners led by families and councils [OPENSTAX]. A chiefdom is a set of villages under the lasting control of a paramount chief, with inherited office and tribute; a simple one has a centre and its satellites, a complex one has two or three tiers. It sits between tribe and state [CHIEF]. States add taxation, a bureaucracy, a claimed monopoly of force and classes [OPENSTAX]. The usual figures of "thousands to tens of thousands" for a chiefdom and more for a state are **[unverified]**; neither page gives them.

Carneiro's circumscription theory says where the step to the state happens. Where good farmland is hemmed in by mountains, desert or sea, the losers of a war cannot move away, must submit, and are merged into a larger unit. Where land is open, as in the Amazon, the defeated disperse and no state forms. The coastal valleys of Peru are the type case. Critics point to enclosed and violent places that never formed states, such as highland New Guinea [CARNEIRO].

**Rule a generator could take.** Use the region's potential population as a floor: below a low threshold (ice, tundra, deep desert) there is no state-level ruler at all, only bands or tribes of kin. Above it, add an *enclosure* term: the good farmland is a small share of the region and the rest is poor (desert, mountain, sea). High enclosure with high appropriability gives the most centralised ruler (a river-valley kingdom, a temple state). Low enclosure with the same population gives a looser one (a league of chiefs, a confederation). The Prospect count gives the tier test that defines a chiefdom: one city Prospect and its towns is a simple hierarchy, four city Prospects make room for rival seats.

### 3.4 Steppe and herders

**Source says.** Barfield's *The Perilous Frontier* argues that the great steppe confederations were not home-grown states. They rose and fell together with the Chinese dynasties they faced. Their "outer frontier" policy was to raid the border and demand tribute while refusing to occupy farmland; the leader kept his followers with the loot. His later book calls such polities shadow empires: they form on the edge of a large settled empire to extract payment from it [BARFIELD] **[partially verified: a review and search excerpts]**. Khazanov's *Nomads and the Outside World* gives the economic root: the more specialised herders become, the more they need the grain and craft goods of settled people, so the nomad economy is never self-sufficient and its politics always involve the neighbours [KHAZ] **[partially verified: catalogue summary]**.

**Rule a generator could take.** Steppe-type biome share (Cold steppe, Temperate grassland, Savanna, Thorn scrub) together with a high amount of horses and pack animals and livestock pushes a horde or a clan confederation as the kin-type faction. The sources say its *scale* depends on a rich farming neighbour, which this rule may not read. Two in-region stand-ins: (a) if the same region holds both steppe and good grain land, the horde is the opposition or the overlord of the farmers; (b) if the region is nearly all steppe, the ruler is a loose council of clans, not an empire. Tell the user that (a) and (b) are proxies for a cross-border fact.

### 3.5 Ports, trade nodes and merchant republics

**Source says.** Stasavage's *States of Credit* studies assemblies and public borrowing in thirty-one European polities over five centuries (the count is from a review, **[partially verified]**). Small city-states such as Genoa and Cologne borrowed on better terms than large territorial states, because merchant elites sat in the assembly and watched the public debt. Active assemblies lasted in geographically small polities; distance made them hard to keep in large ones. The city-states with the best credit were often the most closed and oligarchic, and some turned into republics of rentiers [STAS]. Puga and Trefler show the same arc inside Venice: growing long-distance trade enriched many merchants, who ended the near-hereditary doge in 1032 and set up the Great Council in 1172; from 1297 the richest families made seats hereditary and closed the best trade to others [PT] **[partially verified: abstract]**. The German Hansa by about 1300 joined the merchant guilds of some 70 core cities and about 100 smaller towns [OG14]. A free city's council was in practice its guilds [GUILD-B] [COMMUNE].

**Rule a generator could take.** A vocation-type ruler (a merchant council, a league of towns) needs all of: sea access or a large navigable river; a best city Prospect that is Rich and has a harbour or a river mouth; and a *weak land base*, meaning low appropriability or little state space, so that no landed dynasty outweighs the town. Several Rich Prospects on water with no dominant one push a league instead of one city. Azgaar's numbers give the size of the shift: republic weight 2 of 28 inland, 8 of 36 on the coast [AZ-STATES]. Mark such a ruler as oligarchic by default.

### 3.6 Mining districts

**Source says.** In German-speaking Europe the right to unmined ore was a royal right, the *Bergregal*, first written down in 1158. It passed to the electors in 1356 and to lesser lords in 1648. Its holder took a fixed share of the output of every pit, usually a tenth, and that tithe was a main income of rulers such as those of Saxony and Hanover [BERGREGAL]. To get the ore dug, lords granted *Bergfreiheit* to miners and mining settlements: tax exemptions, release from duties, an own mining court, special economic rights and personal freedom for the miners, who thereby stood apart from the rural population. The first such charter for the Harz is from 1532; Württemberg issued them in 1530, 1558, 1574 and 1597. The towns that grew from these grants were called free mining towns, and outsiders needed the mining captain's written leave to settle there [BERGFREI].

**Rule a generator could take.** A high amount of a lode metal (silver and lead, gold, copper, tin, iron ore) in Hills or Mountains does two things. It strengthens the ruler, whoever that is, because the tithe is concentrated wealth that is easy to hold. And it creates a miners' corporation with a charter: a vocation-type faction that is sanctioned, sits in a town Prospect near the deposit, and is free of the local lords. If the metal patches lie far from the capital, see §4.3: the same faction is the natural core of a secession.

### 3.7 Rule by a church

**Source says.** Prince-bishops arose in the Empire when Henry the Fowler and Otto I gave loyal bishops land and royal rights to weaken the dukes. On the eve of the Reformation the Empire held 53 ecclesiastical principalities; three of the seven electors were archbishops (Mainz, Cologne, Trier). The page lists them by imperial circle, among them the Upper Rhenish, the Lower Rhenish-Westphalian and the Franconian. They were secularised in 1803. Outside the Empire the form was rare: Urgell, whose bishop is still co-prince of Andorra, and Montenegro are examples [PB]. The total number of imperial estates was about three hundred just before 1803 [IE], so by count roughly one estate in six was a church state; the dates differ, so this is an order of magnitude only. The share of land and people was not found. Tibet under the Dalai Lamas and the temple states of the ancient Near East are further cases **[unverified]**.

**Rule a generator could take.** Treat a faith-type ruler as a minority outcome with a base chance of 5 to 10 %, in line with Azgaar's 10 % roll. Raise it with the Supernatural Field at the capital Prospect, and raise it where the land suits a small rich lordship: old dense farmland on a navigable river. An arcane-type ruler is the same slot with a different trigger: very high Supernatural and a high amount of arcane materials. In a region where the church does not rule, it is still an estate with land and is a candidate opposition (§4.5).

---

## 4. What decides an opposition

### 4.1 Rough terrain, poverty and numbers

**Source says.** Fearon and Laitin study civil war onset in 161 countries from 1945 to 1999. Their unit of explanation is insurgency: small, lightly armed bands fighting from rural bases. What favours it is whatever lets rebels hide and whatever marks a weak state [FL].

| Factor | Effect |
|---|---|
| Income per head | 1,000 dollars less goes with 41 % greater yearly odds of onset. Over ten years: 18 % at the tenth percentile of income, 11 % at the median, 1 % at the ninetieth |
| Mountains | ten-year risk 6.5 % for a country with none (tenth percentile), 13.2 % for one about half mountainous (ninetieth percentile) |
| Population | ten-year risk 6.4 % at the tenth percentile (1.4 million), 16.4 % at the ninetieth (56.3 million) |
| Political instability in the last three years | yearly odds up 67 % |
| New state, first two years | odds 5.25 times higher |
| Oil exporter (fuel above a third of export income) | ten-year risk from 10 % to 21 % |
| Ethnic fractionalization, religious fractionalization | not significant, in size or in statistics |
| A majority facing a large minority (largest above 49 %, second above 7 %) | tested as a separate hypothesis; the result sits in the paper's extra tables, which were not read **[partially verified]** |

They add that a rural base matters: in a city the government can find rebels through informers. Rough land poorly served by roads and far from the centre of power is the favoured ground [FL].

**Rule a generator could take.** Compute an *insurgency* score from four inputs the region has: the share of Hills and Mountains (use a logarithm, as the paper does), the inverse of mean Prosperity, the logarithm of the potential population, and the share of the population that lives far from the capital Prospect. A high score means an armed opposition exists whatever the make-up of the people. Do not feed the number of races into this score. The score says *that* there is an opposition; §4.3 to §4.5 say *which kind*.

### 4.2 Fractionalization and polarization

**Source says.** Montalvo and Reynal-Querol separate two measures of diversity [MRQ]. With `π_i` the population share of group `i` and `N` groups:

```
FRAC = 1 − Σ π_i²

RQ   = 1 − Σ ((1/2 − π_i) / (1/2))² · π_i   =   4 · Σ π_i² · (1 − π_i)
```

FRAC is the chance that two random people belong to different groups. It keeps rising as groups multiply. RQ measures the distance from the two-halves case and is 1 for two groups of 50 % each; it falls as groups multiply. For two groups the two are the same up to a factor (`FRAC = 2·π_1·π_2`, `RQ = 4·π_1·π_2`). Beyond two they part: the correlation between them is positive at low fractionalization, zero in the middle range and negative at high fractionalization [MRQ].

In a logit on 138 countries, 1960 to 1999, in five-year periods, fractionalization is not significant and polarization is. Mean polarization is 0.51 with a standard deviation of 0.24. One standard deviation more raises the probability of civil war by 67 %; going from 0.51 to 0.84 nearly doubles it [MRQ]. Collier and Hoeffler agree from the other side: more ethnic and religious diversity *lowers* the risk, because rebels recruit among their own kind and a mixed population is hard to unite; but where one group holds 45 to 90 % of the population the risk nearly doubles [CH]. Both papers cite the older observation that the worst case is a large majority facing a large minority, and that very uniform and very mixed societies are both calm [MRQ] [FL].

**Rule a generator could take.** Compute RQ over the region's race shares. The repo's own constraints matter here. Since the largest race holds at most 43 %, the dominance test (45 to 90 %) and the majority test (above 49 %) can never be true, so those rules are dead in this world. RQ still varies. Worked values, my arithmetic:

| Race shares | FRAC | RQ |
|---|---|---|
| 43, 43, 14 | 0.61 | 0.91 |
| 35, 25, 18, 12, 6, 4 | 0.76 | 0.68 |
| 25, 25, 25, 25 | 0.75 | 0.75 |
| 19, 19, 19, 19, 12, 12 | 0.83 | 0.57 |
| six equal | 0.83 | 0.56 |

So RQ runs from about 0.55 to about 0.9 here, against a real-world mean of 0.51. A kin-type opposition should appear when RQ is in the upper part of that band, which in practice means two races that together hold most of the region and are near each other in size. A simpler test with the same meaning: the second race holds at least two thirds of the first, and the two together hold at least 60 %. Then the ruler is of the first race and the opposition of the second. With five or six races of similar size, give no kin opposition; give several small kin factions instead.

### 4.3 Wealth that can be taken

**Source says.** Collier and Hoeffler find that dependence on primary commodity exports raises the risk of civil war up to a peak and lowers it beyond: the highest risk is at a share of about 32 % of national income in the working paper [CH]; a critic reports 33 % for the published version **[partially verified]**. Their reading: commodities pay rebels, and at very high levels they pay the government more. Mountains and an unevenly spread population also raise the risk; schooling and growth lower it [CH]. Ross checks the mechanisms in thirteen wars and finds that oil, non-fuel minerals and drugs are causally tied to conflict, while legal farm commodities are not [ROSS] **[partially verified: abstract]**.

Le Billon sorts resources on two axes [LB]. *Point* resources are concentrated in a small area and worked by few hands with much capital: deep-shaft mines, oil. *Diffuse* resources are spread over wide areas and worked by many: alluvial gems and minerals, timber, crops, fish. *Proximate* resources are near the centre of power and firmly held by it; *distant* ones are not.

| | Point | Diffuse |
|---|---|---|
| **Proximate** | coup d'état: fight for the centre itself | mass rebellion, peasant war |
| **Distant** | secession | warlordism |

His reasoning: a point resource cannot be tapped without sovereign control, so those near it either seize the state or leave it; it employs few, so it gives no mass movement. A diffuse resource can be worked and taxed by anyone on the spot, so it feeds either a broad rising or local strongmen [LB]. Lujala, Gleditsch and Gilmore find the same split for diamonds: alluvial (secondary) diamonds raise the onset of ethnic war, mined (primary) diamonds lower it [LGG] **[partially verified: abstract]**.

**Rule a generator could take.** Classify each resource once. Point: silver and lead, lode gold, copper, lode tin, iron ore, mined salt, bedrock gems. Diffuse and lootable: placer gold, stream tin, alluvial gems, furs, spices, dyes, timber, arcane plants. Diffuse and bulky: grain, livestock, fish. Find the region's most valuable resource and measure its distance from the capital Prospect relative to the region's size.

- Point and near: the opposition is a **court faction**: a rival branch of the dynasty or a great house that wants the throne (the Claimant of §2.3).
- Point and far: the opposition is a **provincial power** that wants out: a mining town, a marcher lord, a second city (Independence).
- Lootable and near: the opposition is a **popular movement**: peasants, a commune, a sect.
- Lootable and far: the opposition is **warlords and outlaws**: robber barons, a bandit league, smugglers; with high RQ, a kin faction funded by the loot.

A region whose wealth is all grain and livestock gets no resource-driven opposition, in line with Ross's finding on legal farm goods.

### 4.4 Distance from the capital and the four cleavages

**Source says.** Buhaug and Rød cut Africa into cells of 100 by 100 km and ask where the fighting was, 1970 to 2001. Conflicts over territory are more likely in thinly peopled cells near the state border, far from the capital, and without much rough terrain. Conflicts over who governs are more likely in densely peopled cells near the capital and near diamond fields [BR] **[partially verified: abstract]**. Scott's travel-cost argument (§3.2) gives the reason for the first half.

Lipset and Rokkan reduce the lasting conflicts of European politics to four cleavages from two upheavals [LR].

| Cleavage | From | Sides | Parties it produced |
|---|---|---|---|
| Centre against periphery | the national revolution | the elite of the core against the outlying regions | regional and separatist parties |
| State against church | the national revolution | secular rulers against the faithful | Christian parties against liberal ones |
| Land against industry | the industrial revolution | farm interest against town interest, over trade and tariffs | agrarian and peasant parties |
| Owner against worker | the industrial revolution | those who own against those who labour | parties of the left and right |

They held that the party systems of the 1960s were still frozen along these lines a century later [LR].

**Rule a generator could take.** The four cleavages are four opposition types, and the region's data can score each.

| Cleavage | Opposition | Signal |
|---|---|---|
| Centre against periphery | a regional lord, a second city, a border people | a Rich Prospect far from the capital; a large race that is strong in the periphery; a low `distance to the region border` for populous Prospects |
| State against church | the temple against the crown, or the crown against a ruling temple | high Supernatural away from the capital, or at the capital under a secular ruler |
| Land against town | landed houses against the merchant and craft towns | high appropriability (strong nobility) together with several Rich Prospects on water (strong towns) |
| Owner against worker | journeymen, miners, peasants | low Prosperity with a large population; point resources worked by many miners |

Score all four and take the highest as the opposition. Buhaug and Rød give the tie-break: a dense region with its wealth near the capital produces a rival for the throne; a sparse one with wealth at the edge produces a breakaway.

### 4.5 The estates

**Source says.** Medieval Christendom sorted people into estates: usually clergy, nobility and commoners, with the king outside them. Sweden and Finland had four: nobility, clergy, burghers and land-owning peasants, and a bill needed three of the four. Scotland's three were prelates, nobles and the commissioners of the burghs. In the Imperial Diet clergy, nobility and burghers sat. The recorded struggles are the ruler against the estates as a body, the nobility against taxation, the crown against the church over investiture, and towns against peasants over the right to trade [EST].

**Rule a generator could take.** Give every region the same four or five corporate actors and a strength for each, from its own data:

| Estate | Faction type | Strength grows with |
|---|---|---|
| Crown | family or dynasty | appropriability, state space, point resources near the capital |
| Nobility | family or dynasty (several houses) | grain and livestock land away from the capital, hills with castles, horses |
| Towns | vocation or guild | number and class of Prospects on water, sea access, Prosperity |
| Church | faith (or arcane) | Supernatural, old dense farmland |
| Peasants or clans | kin or race | refuge share, low appropriability, RQ |

The ruler is the strongest, the opposition is the strongest of the rest, and the opposition is *active* when its strength is at least 0.8 of the ruler's, after §2.3. This one table yields the ruler types of §3 as special cases: towns strongest gives the merchant republic, church strongest the prince-bishopric, clans strongest the tribal council.

### 4.6 Blades in the Dark: rivals by construction

**Source says.** A new crew gets its first relations as it is made. The faction that claims the crew's hunting ground is either paid 1 coin, paid 2 coin for +1 status, or not paid for −1 status. For each upgrade, one faction helped (+1 status, or +2 for 1 coin) and one was wronged (−2, or −1 for 1 coin). For the favourite contact, one faction is the contact's friend (+1) and one its enemy (−1), or +2 and −2 if the players choose. The game master names the factions. Status runs from −3 to +3 [BITD-CC] [BITD-FG].

**Rule a generator could take.** Build relations as a by-product of how a faction came to be, not as a separate roll. Each faction gets one landlord (who holds its seat), one helper and one it harmed. For the opposition this is automatic: the harmed party is the ruler.

---

## 5. What decides guilds

**Source says.** Ogilvie's survey gives the numbers [OG14].

- **Where.** Guilds were most common in manufacturing. Almost every urban craftsman was in one, and in central and southern Europe many rural craftsmen as well. Nearly all premodern economies had guilds of merchants and retailers. Guilds were rarest in primary production, though guilds of farmers, gardeners, wine-growers, shepherds, miners and fishermen existed.
- **When.** Local merchant guilds reappear from the early eleventh century, craft guilds from about 1100. By the thirteenth century local traders, long-distance merchants and craftsmen had guilds across much of Europe, and for the next 300 to 600 years a guild licence was needed to trade or work in most towns. Some towns and enclaves were guild-free.
- **Leagues.** Merchant guilds of several towns could join in a hansa; the German Hansa had about 70 core cities and about 100 smaller towns around 1300.
- **How many.** London had 72 livery companies and 14 other trade bodies in 1500. Paris had 103 guilds in 1250, 124 in 1700 and 133 in 1766. Florence, one of the largest cities of Europe with 100,000 people, had 21 in 1300, of about 350 members on average (100 to 1,600). Fulda, with 8,500 people in 1784, also had 21, of 13 members on average (4 dyers to 60 shoemakers).
- **How many people.** Masters were a minority: 12 to 13 % of the inhabitants in sixteenth-century London and Augsburg, 9 to 10 % in Barcelona, Rouen and Venice, at most 5 % in Paris, Florence and Turin. Membership usually needed town citizenship, which under half the inhabitants of a typical town held.
- **Where they faded.** After 1500 merchant guilds declined in England and the Low Countries; Amsterdam barred them and Leiden abolished its craft guilds, while rural industry grew outside guild control. Elsewhere guilds lasted past 1800.

Her book rests on a database of 12,051 and 5,333 observations from twenty-three countries, and finds the strongest guilds in the German, Nordic and Iberian lands and the weakest in the Low Countries and Britain [OG19] **[partially verified: reviews]**. Britannica records the usual political sequence: the merchant guild first runs the town, then the craft guilds contest the council [GUILD-B]. Ogilvie's own reading is that guilds were bodies of middle-class businessmen that shared their rents with the rulers who enforced their privileges [OG14].

Dwarf Fortress gives the thresholds a generator uses (§2.2): a people must value cooperation and craftsmanship, and a guild needs about ten members of one trade [DF-ET] [DF-GUILD].

**Rule a generator could take.**

1. **Existence.** Every city Prospect has guilds; a town Prospect has them when it is Rich or Average. Prosperity changes their strength, not their existence. A race can switch them off (the Dwarf Fortress threshold).
2. **Merchant guild first.** One merchant guild per city, and per town with a harbour, a river mouth or a navigable river. Several such Prospects in one region make a league (the Hansa pattern), which is the candidate ruler of §3.5.
3. **Craft guilds by what the land yields.** Pick the crafts from the strongest resources in the Prospect's reach: smiths with iron ore, masons with stone, weavers and tanners with livestock, shipwrights with timber on a coast, jewellers with gems or gold, dyers with dyes. This is the catchment rule of `factions.md` §5.2.
4. **Count by regime, not by size.** Draw whether a region lumps its trades into a few large guilds (the Florence pattern, 7 to 21) or splits them into many small ones (the Paris pattern, about 100 for a great city). The count is a property of the region's ruler, not of the town's population.
5. **Which side leads.** Merchants lead in ports and on long-distance routes; crafts lead in inland production towns with one strong craft resource. In a region with a strong crown both are sanctioned and neither rules.
6. **Primary producers.** Miners get a corporation where §3.6 applies. Fishers, herders and farmers get one only rarely.
7. **As opposition.** Guilds are the Towns estate of §4.5. They become the opposition where towns are strong and the ruler is a landed dynasty, and their inner conflict (merchants against crafts, masters against journeymen) is the owner-against-worker line of §4.4.

---

## 6. Summary

### 6.1 From input signal to outcome

| Input signal | Pushes the ruler toward | Pushes the opposition toward | Pushes guilds | Source |
|---|---|---|---|---|
| Grain high relative to Produce, on Flat Land and Lowland | a taxing dynasty, more centralised | nobility of the grain land | | [MMP] [SCOTT-ATG] |
| Produce, fish, game dominate; Grain low | chiefs, clans, councils of kin | | | [MMP] |
| Total food or Prosperity high by itself | nothing: plenty alone does not centralise | | stronger guilds, not more | [MMP] [OG14] |
| Share of Hills and Mountains | kin-type rule if it covers the region | armed rural opposition; risk about doubles from none to half | miners where metal | [FL] [SCOTT-ANBG] |
| Marsh, rain forest, desert, tundra | no state-level ruler when dominant | refuge for outlaws and small peoples | | [SCOTT-ATG] |
| Steppe biomes with horses and livestock | horde or clan confederation | herders against farmers when both are present | | [BARFIELD] [KHAZ] |
| Good land small and enclosed by poor land or sea | strongly centralised ruler | | | [CARNEIRO] |
| Very low population | bands and tribes only | | none | [OPENSTAX] |
| Large population | | more likely, by itself | | [FL] |
| Low Prosperity | | more likely; popular type | | [FL] [CH] |
| Sea access, Rich city Prospect with harbour or river mouth, weak land base | merchant council or town league (republic weight 2/28 inland, 8/36 coastal) | towns against landed houses | merchant guild leads | [STAS] [PT] [AZ-STATES] [OG14] |
| Lode metal in highlands | richer ruler of any type | secession if far from the capital | chartered miners | [BERGREGAL] [BERGFREI] [LB] |
| Point resource near the capital | | claimant, court faction | | [LB] [BR] |
| Point resource far from the capital | | breakaway province | | [LB] [BR] |
| Lootable resource near the capital (placer gold, stream tin, alluvial gems, furs, spices) | | mass rising | | [LB] |
| Lootable resource far from the capital | | warlords, bandit league; kin faction if RQ is high | | [LB] [LGG] |
| Wealth only in grain and livestock | | no resource-driven opposition | | [ROSS] |
| Race polarization RQ high (two large races) | ruler of the first race | kin faction of the second race | | [MRQ] |
| Many races of similar size (FRAC high, RQ low) | | none from kin; several small kin factions | | [MRQ] [CH] [FL] |
| Supernatural high at the capital | faith-type ruler, base 5 to 10 % | | | [PB] [AZ-STATES] |
| Supernatural high elsewhere | | temple against crown | | [LR] [EST] |
| Rich Prospect far from the capital | | regional lord or second city | its own guilds | [BR] [LR] |
| Opposition strength at least 0.8 of the ruler's | | opposition is active, not latent | | [CK3-SUB] |
| Race values (cooperation, craft, commerce) | fixed ladder or invented offices | banditry share | guilds possible at all | [DF-RAW] [DF-ET] |
| City Prospect present | | | guilds exist; count by regime, not size | [OG14] |

### 6.2 Faction types the evidence supports

My judgement, for the user to decide. The estates of §4.5 and the bases of `factions.md` §2.1 reduce to five types that every signal above can address:

| Type | Covers | Rules when | Opposes when |
|---|---|---|---|
| **Dynasty** (family) | crown, noble houses, claimants | default; grain land, state space | point wealth near the capital; strong landed houses |
| **Kin** (race, clan, tribe, horde) | clans, tribes, hordes, a people | refuge land, low appropriability, steppe, thin population | high RQ; refuge share beside a crown |
| **Guild** (vocation) | merchant council, league, craft guilds, miners | port with weak land base | strong towns under a dynasty; distant mines |
| **Faith** | church, temple, order | high Supernatural at the capital | high Supernatural elsewhere |
| **Arcane** | college, cabal | very high Supernatural with arcane materials | as Faith, rarer |

Outlaws (bandits, pirates, smugglers) are not a sixth ruler type. They are what the insurgency score of §4.1 and the lootable-and-far cell of §4.3 produce when no estate claims the opposition.

---

## 7. Gaps

- **Crusader Kings III starting governments.** The wiki pages read do not say what decides a realm's government at the start. That it is authored per realm in the history files, and that feudal and clan follow religion, is **[unverified]**. The Nomadic, Administrative, Republican and Merchant government pages were not read.
- **Thresholds for bands, tribes, chiefdoms and states.** Only the band (20 to 30) and tribe (hundreds to several thousand) figures were confirmed, from a textbook. The usual chiefdom and state figures are **[unverified]**, and Service's own books were not read.
- **Scott.** Both books are known through Wikipedia and reviews only. The 300 metre line and the Zomia figures are from Wikipedia; "escape crops" and the travel-cost argument are from reviews.
- **Barfield and Khazanov.** Known through a review and a catalogue summary. No numbers.
- **Share of church states.** The 53 ecclesiastical principalities and the "about three hundred" estates are from different dates. The 1521 register counts by category were not found on either Wikipedia. No figure for the share of land or people, and nothing checked for Tibet or other regions.
- **Merchant republics and poor hinterland.** No source read states "poor hinterland" as a tested condition. Stasavage gives small size and a merchant elite; the hinterland condition in §3.5 is my inference. Greif was not read.
- **Collier and Hoeffler.** Read in the 2002 working paper, which gives the 32 % peak. The published 2004 figures are **[partially verified]**.
- **Montalvo and Reynal-Querol.** Read in the March 2005 working paper. The Greek letters were lost in the text extraction; the formulas in §4.2 are restored from the paper's own two-group and three-group examples, which survived.
- **Le Billon.** The table is from his 2007 review article, which restates his 2001 typology. The 2001 article was not read.
- **Ross, Lujala and others, Buhaug and Rød, Puga and Trefler.** Abstracts only.
- **Ogilvie's 2019 book.** Not read; the two database counts are labelled differently by a review and by the project page. Whether small towns below some size lacked guilds is not answered by the 2014 article, which only says most towns had them.
- **Guild revolutions** (Zurich 1336, Cologne 1396 and similar), as evidence for when crafts take the council, were not checked.
- **Dwarf Fortress.** The raws are from a 2020 mirror, not the current version. The chance that a human civilisation rolls both guild values to 15 or more is not stated anywhere; the distribution of `VARIABLE_VALUE` was not found.
- **Other generators.** Nothing sourced for RimWorld faction placement, Songs of Syx, Europa Universalis or the *Without Number* faction rules. Caves of Qud's choice of village government and Ultima Ratio Regum's choice of policies are not documented on the pages read.
- **Cross-border findings.** Barfield's rule and Fearon and Laitin's foreign sanctuaries need a neighbour. They are stated but cannot be used under the constraint of this task.

---

## 8. Sources

### 8.1 Generators and games

- **AZ-STATES** — Azgaar, *Fantasy Map Generator*, `src/model/political/states-generator.ts` at `master`, commit `7413629` (2026-11-16): `defineStateForms`, `getTribalForm`, state type, taxes and corruption by form. https://github.com/Azgaar/Fantasy-Map-Generator/blob/master/src/model/political/states-generator.ts
- **AZ-CULT** — same repository, `src/model/society/cultures-generator.ts`: `defineCultureType`, `defineCultureExpansionism`, spread costs. https://github.com/Azgaar/Fantasy-Map-Generator/blob/master/src/model/society/cultures-generator.ts
- **AZ-REL** — same repository, `src/model/society/religions-generator.ts`: religion types, expansion, naming methods. https://github.com/Azgaar/Fantasy-Map-Generator/blob/master/src/model/society/religions-generator.ts
- **AZ-BURG** — same repository, `src/model/political/burgs-generator.ts`: `defineFeatures`. https://github.com/Azgaar/Fantasy-Map-Generator/blob/master/src/model/political/burgs-generator.ts
- **AZ-BIOME** — same repository, `src/model/physical/biomes-generator.ts`: biome order. https://github.com/Azgaar/Fantasy-Map-Generator/blob/master/src/model/physical/biomes-generator.ts
- **DF-RAW** — Dwarf Fortress vanilla raws, `objects/entity_default.txt`, mirror by BenLubar (last change February 2020). https://github.com/BenLubar/raws/blob/master/objects/entity_default.txt
- **DF-ET** — Dwarf Fortress Wiki, "Entity token" (raw wikitext). https://dwarffortresswiki.org/index.php/Entity_token
- **DF-VP** — Dwarf Fortress Wiki, "Variable positions". https://dwarffortresswiki.org/index.php/Variable_positions
- **DF-GUILD** — Dwarf Fortress Wiki, "Guild". https://dwarffortresswiki.org/index.php/Guild
- **DF-MC** — Dwarf Fortress Wiki, "Merchant company". https://dwarffortresswiki.org/index.php/Merchant_company
- **DF-MON** — Dwarf Fortress Wiki, "Monastery". https://dwarffortresswiki.org/index.php/Monastery
- **CK3-GOV** — Crusader Kings III Wiki, "Government" (version 1.21), read through a browser. https://ck3.paradoxwikis.com/Government
- **CK3-SUB** — Crusader Kings III Wiki, "Subjects", section "Factions" (version 1.21; the "Factions" link redirects here), read through a browser. https://ck3.paradoxwikis.com/Subjects#Factions
- **URR-ID** — Mark R. Johnson, "Making ideology physical", Ultima Ratio Regum development post, ModDB. https://www.moddb.com/news/making-ideology-physical
- **QUD-V** — Caves of Qud Wiki, "Village". https://wiki.cavesofqud.com/wiki/Village **[partially verified: the page marks itself incomplete]**
- **BITD-CC** — *Blades in the Dark*, "Crew creation", Roll20 compendium (licensed). https://roll20.net/compendium/BITD/Crew%20creation

### 8.2 What decides the ruler

- **MMP** — Joram Mayshar, Omer Moav and Luigi Pascali, "The Origin of the State: Land Productivity or Appropriability?", *Journal of Political Economy* 130 (4), 2022, 1091–1144. Read in the working-paper version of 11 October 2019 (with Zvika Neeman). https://doi.org/10.1086/718372, https://warwick.ac.uk/fac/soc/economics/staff/omoav/mayshar_et_al_jpe_2nd_11_oct_2019.pdf
- **SCOTT-ANBG** — James C. Scott, *The Art of Not Being Governed* (Yale, 2009), through Wikipedia and a review in Reason. https://en.wikipedia.org/wiki/The_Art_of_Not_Being_Governed, https://reason.com/2010/05/25/life-on-the-edge/ **[partially verified: book not read; the review through search excerpts]**
- **SCOTT-ATG** — James C. Scott, *Against the Grain* (Yale, 2017), through Wikipedia. https://en.wikipedia.org/wiki/Against_the_Grain:_A_Deep_History_of_the_Earliest_States **[partially verified: book not read]**
- **CARNEIRO** — Wikipedia, "Carneiro's circumscription theory". https://en.wikipedia.org/wiki/Carneiro%27s_circumscription_theory
- **CHIEF** — Wikipedia, "Chiefdom". https://en.wikipedia.org/wiki/Chiefdom
- **OPENSTAX** — OpenStax, *Introduction to Anthropology*, §8.2 "Acephalous Societies: Bands and Tribes" and §8.3 "Centralized Societies: Chiefdoms and States". https://openstax.org/books/introduction-anthropology/pages/8-2-acephalous-societies-bands-and-tribes, https://openstax.org/books/introduction-anthropology/pages/8-3-centralized-societies-chiefdoms-and-states
- **BARFIELD** — Thomas J. Barfield, *The Perilous Frontier* (1989) and *Shadow Empires* (2023), through a review in the Asian Review of Books. https://asianreviewofbooks.com/shadow-empires-an-alternative-imperial-history-by-thomas-j-barfield/ **[partially verified: search excerpts]**
- **KHAZ** — Anatoly M. Khazanov, *Nomads and the Outside World* (1984; 2nd ed. 1994), catalogue summary. https://vtechworks.lib.vt.edu/handle/10919/66222 **[partially verified: search excerpts]**
- **STAS** — David Stasavage, *States of Credit: Size, Power, and the Development of European Polities* (Princeton, 2011), publisher's description. https://press.princeton.edu/books/paperback/9780691166735/states-of-credit **[book not read]**
- **PT** — Diego Puga and Daniel Trefler, "International Trade and Institutional Change: Medieval Venice's Response to Globalization", *Quarterly Journal of Economics* 129 (2), 2014. https://repec.cepr.org/repec/cpr/ceprdp/DP9076.pdf **[partially verified: abstract through search excerpts]**
- **BERGREGAL** — Wikipedia, "Bergregal". https://en.wikipedia.org/wiki/Bergregal
- **BERGFREI** — German Wikipedia, "Bergfreiheit". https://de.wikipedia.org/wiki/Bergfreiheit
- **PB** — Wikipedia, "Prince-bishop" (read to character 100,000 of 171,000). https://en.wikipedia.org/wiki/Prince-bishop
- **IE** — Wikipedia, "Imperial Estate". https://en.wikipedia.org/wiki/Imperial_Estate

### 8.3 What decides an opposition

- **FL** — James D. Fearon and David D. Laitin, "Ethnicity, Insurgency, and Civil War", *American Political Science Review* 97 (1), 2003, 75–90. Abstract from Cambridge; the numbers from a scanned course copy of the article, read through a browser. https://doi.org/10.1017/S0003055403000534, https://is.muni.cz/el/fss/podzim2023/CDSn4001/um/readings/fearon_and_laitin.txt
- **MRQ** — José G. Montalvo and Marta Reynal-Querol, "Ethnic Polarization, Potential Conflict, and Civil Wars", *American Economic Review* 95 (3), 2005, 796–816. Read in the working-paper version of March 2005. https://doi.org/10.1257/0002828054201468, https://econ-papers.upf.edu/papers/770.pdf
- **CH** — Paul Collier and Anke Hoeffler, "Greed and Grievance in Civil War", *Oxford Economic Papers* 56 (4), 2004, 563–595. Read in the working-paper version CSAE WPS/2002-01 (13 March 2002). https://econpapers.repec.org/RePEc:oup:oxecpp:v:56:y:2004:i:4:p:563-595, https://econwpa.ub.uni-muenchen.de/econ-wp/dev/papers/0409/0409007.pdf
- **LB** — Philippe Le Billon, "Geographies of War: Perspectives on 'Resource Wars'", *Geography Compass* 1 (2), 2007, 163–182 (Table 2 restates the typology of his "The political ecology of war", *Political Geography* 20, 2001, 561–584). Read through a browser from a course copy. https://doi.org/10.1111/j.1749-8198.2007.00010.x, https://is.muni.cz/el/fss/podzim2024/CDSn4001/um/readings/le_billon.txt
- **ROSS** — Michael L. Ross, "How Do Natural Resources Influence Civil War? Evidence from Thirteen Cases", *International Organization* 58 (1), 2004, 35–67. https://doi.org/10.1017/S002081830458102X **[partially verified: abstract]**
- **LGG** — Päivi Lujala, Nils Petter Gleditsch and Elisabeth Gilmore, "A Diamond Curse? Civil War and a Lootable Resource", *Journal of Conflict Resolution* 49 (4), 2005, 538–562. https://www.prio.org/publications/3296 **[partially verified: abstract through search excerpts]**
- **BR** — Halvard Buhaug and Jan Ketil Rød, "Local Determinants of African Civil Wars, 1970–2001", *Political Geography* 25 (3), 2006, 315–335. https://www.prio.org/publications/3415 **[partially verified: abstract through search excerpts]**
- **LR** — Wikipedia, "Cleavage (politics)", on Seymour Martin Lipset and Stein Rokkan, *Party Systems and Voter Alignments* (1967). https://en.wikipedia.org/wiki/Cleavage_(politics) **[book not read]**
- **EST** — Wikipedia, "Estates of the realm" (read to character 100,000 of 116,000). https://en.wikipedia.org/wiki/Estates_of_the_realm

### 8.4 Guilds

- **OG14** — Sheilagh Ogilvie, "The Economics of Guilds", *Journal of Economic Perspectives* 28 (4), 2014, 169–192. https://bollettinoadapt.it/wp-content/uploads/2019/12/Ogilvie-2014-The-Economics-of-Guilds.pdf
- **OG19** — Sheilagh Ogilvie, *The European Guilds: An Economic Analysis* (Princeton, 2019), through the review at EH.net. https://eh.net/book_reviews/the-european-guilds-an-economic-analysis/ **[partially verified: search excerpts]**
