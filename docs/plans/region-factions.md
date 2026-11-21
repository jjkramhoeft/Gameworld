# Plan: the factions of a region

Status: implemented 2026-11-19, as proposed. The rule is in
[society-generation.md](../society-generation.md) section 4, and the readme has the names:
the six types, the three roles, "lead race", "seat" and "lesser faction". This document
keeps the proposal, the test behind it and what was weighed. "Implemented" below says what
changed on the way into the generator and what the generator measures. Where the two
differ, society-generation.md holds.

The numbers in persons below are those of the density of the time, 0.06 persons per km².
The default is 0.3 since 2026-11-21, and the limits in persons were raised five times with
it: 400 persons for an opposition, 750 for each lesser faction, 800 to 3,000 for the index
people and 300 to 1,300 for the index small. The factions of a region did not change.

## What was asked

- Every region has factions: at least one ruling faction.
- Some regions have an opposition faction.
- Some regions have guild factions, where they fit.
- Settle the types of faction: family, vocation, race, belief, magic and so on.
- A way to calculate the factions of a region from its races, biomes, resources and wealth.

## What exists

- [research/factions.md](../research/factions.md): 48 faction archetypes from games,
  tabletop settings and history, their axes and their relations. It says what kinds of
  faction there are. It does not say how a region gets its factions.
- [research/faction-generation.md](../research/faction-generation.md): how generators and
  games compute the kind of ruler of a land, and what the scholarship says about which
  ruler and which opposition a land gets. Written for this plan.
- [races-basis.md](../races-basis.md): every race has a "Factions" line, the kinds of group
  the race forms.

## What the sources say

The keys are those of research/faction-generation.md, which holds the detail and marks
what was only partly verified.

| Finding | What the rule takes from it |
|---|---|
| The one generator that publishes its weights, Azgaar's, draws Monarchy, Republic and Union 25 : 2 : 1 inland and 25 : 8 : 3 for a seafaring state. A theocracy is a roll of 10 %. Resources, wealth and the peoples do not enter, and it has no opposition and no guilds [AZ-STATES] | A default ruler and a few signals that shift the weights. The coast and trade raise rule by the towns. Rule by a faith is about one region in ten |
| Dwarf Fortress lets the people decide how a polity is built: its offices, whether it forms guilds and merchant companies, how much of it turns to banditry [DF-RAW] [DF-ET] | A leaning per race and type (Appendix A) |
| Hierarchy follows grain, which can be seized, stored and carried, not fertile land as such [MMP] | Grain, flat land and many persons give a house. Plenty alone gives nothing |
| Hills, marsh, steppe and desert are where states did not reach [SCOTT-ANBG] [SCOTT-ATG]. Herding peoples of the steppe form hordes [BARFIELD] | Rough land, open herding land and forest without grain give a people |
| Bands and tribes are small: tens to a few thousand persons [OPENSTAX] | A very small region has no house: a people or a place rules it |
| Merchant republics are small, compact and mercantile [STAS] | Trade, wealth and many towns give rule by the guilds |
| A mine gave its lord a tenth and its miners a charter and a court of their own. It did not make the miners rulers [BERGREGAL] [BERGFREI] | Ore gives a miners' guild under the ruler. It does not give a ruler |
| The Empire had 53 church states among about three hundred estates [PB] [IE] | Rule by a faith is a minority form |
| Civil war follows rough terrain, poverty and numbers. Ethnic and religious diversity do not predict it [FL] | Rough land and poor land raise the tension. The number of races does not |
| Polarization, two large groups, predicts conflict where diversity does not [MRQ] | Tested and dropped: the regions of this world are all about equally polarized (see "Weighed and dropped") |
| Wealth that can be taken feeds a rising: near the centre a claimant or a mass rising, far from it a breakaway or warlords [LB] [CH] | Silver, gold and gems raise the tension and feed a rival house or a guild. Near and far are not read yet (see "Open") |
| The lasting rivals are few: crown, nobility, towns, church and commons [EST], along four cleavages [LR]. A Crusader Kings faction presses its demand when it has 80 % of the strength of its liege [CK3-SUB] | The cleavage table of section 6, and "contest": an opposition arises when another kind of ruler fits the region nearly as well |
| Guilds follow towns, not wealth. Merchant guilds came first, craft guilds a century later, and primary production was rarely guilded. Florence had 21 guilds with 100,000 inhabitants and Fulda 21 with 8,500 [OG14] | The merchants are the most frequent guild. Miners, salters, foresters and drovers get a lower fit. The crafts follow the resources |

## What a region knows when its factions are made

Factions belong to the Planned stage. The region is Elevated, and its Prospects, its races
and its potential population are calculated. Measured on 1,156 regions with land (the
17 × 17 regions around (0, 0) of the seeds `cellgen`, `gameworld`, `atlas` and `ember`,
default Parameter Set):

| | Lowest | 10 % | Median | 90 % | Highest |
|---|---|---|---|---|---|
| Persons (potential population) | 0 | 151 | 360 | 604 | 1,188 |
| Share of the largest race | 19 % | 24 % | 27 % | 32 % | 43 % |
| Share of the second race | 16 % | 21 % | 24 % | 27 % | 34 % |
| Prospects on show | 0 | 6 | 13 | 19 | 20 |
| Rich Prospects on show | 0 | 0 | 2 | 9 | 20 |
| Prospects on show with a harbour or a navigable river mouth | 0 | 0 | 1 | 4 | 7 |
| Open biomes (steppe, grassland, savanna, desert, scrub, tundra), share of the land | 0 % | 24 % | 44 % | 65 % | 100 % |
| Hills and Mountains, share of the land | 0 % | 0 % | 14 % | 34 % | 100 % |
| Grain, mean amount | 0.00 | 0.17 | 0.32 | 0.45 | 0.68 |
| Metals, gems and salt: the shares of the land with 0.25 or more of each, added | 0 % | 4 % | 9 % | 15 % | 50 % |

Two of the 1,156 regions hold nobody: their land feeds no race. A region without persons
gets no faction, so the test below runs on the other 1,154.

Three things in these numbers shape the rule:

- **A region is small in persons.** The median region holds 360 persons, so it carries a
  handful of factions, not dozens. The number of factions must follow the persons.
- **A region is mixed.** It spans many biomes, so its averages are flat: four regions in
  five have a mean grain amount between 0.17 and 0.45. No single number of the land
  decides the ruler. Several weak signals and the leaning of the races must work together.
- **The races are flat too.** Every region holds six races and the largest has about a
  quarter. How divided a region is by race is nearly the same everywhere (see "Weighed and
  dropped"). So the races can say who rules and who opposes, but not whether there is an
  opposition.

## Proposal

### 1. Six types of faction

The type of a faction is what binds its members. Each type rests on a fact that a person
already has, or will have, as a Stub. So membership can be worked out from the Stub.

| Type | Bound by | The fact of a person it rests on | Archetypes of research/factions.md |
|---|---|---|---|
| **Family** | blood and marriage: a house and its household | family links | A1 crown, A2 noble house, A6 pretenders |
| **Race** | a people: one race, its clans and its customs | race | H1 to H6, G4 a dispossessed people |
| **Vocation** | work: a trade, its masters and its monopoly | vocation | D1 to D8 trade and craft, C4 artificers, E1 to E6 arms, F1 to F6 crime, G2, G3 |
| **Belief** | a faith: a temple, an order, a sect | belief (not a concept of the readme yet) | B1 to B8 |
| **Magic** | the arcane: a college, a circle, a cabal | the supernatural vocations, and the Supernatural field at the home | C1 to C3 |
| **Place** | a home: a town or a valley that rules itself | home | A3 free city, A4 league, A5 commune |

- Family, vocation, race, belief and magic were asked for. **Place** is added: the free
  city, the Thing district and the village commune are bound by none of the five, and
  six of the 28 races lean to it first (Appendix A).
- Arms and crime are not types. A free company and a thieves' guild are bound by work:
  the Soldier and the Thief are vocations.
- A cause (rebels, a resistance) is not a type either. It is a role: the opposition.

### 2. Three roles

The role is how the faction stands in its region.

| Role | How many per region | Meaning |
|---|---|---|
| **Ruling** | exactly 1, and none in a region without persons | It governs the region |
| **Opposition** | 0 or 1 | It wants the rule, or wants out of it |
| **Lesser** | 0 to 4 | It lives under the ruler: a guild, an order, a circle, a great house, a chartered town, an enclave |

Every type can hold every role. The **form** is the name of a type in a role. It is a
label for the authoring, the rule does not read it:

| Type | Ruling | Opposition | Lesser |
|---|---|---|---|
| Family | crown | rival house | great house |
| Race | horde, clan or hold | horde, clan or hold | enclave |
| Vocation | merchant republic | guild | guild of one trade (15 trades, section 7) |
| Belief | temple state | sect or order | order |
| Magic | magocracy | cabal | circle |
| Place | free city or commune | free city or commune | chartered town |

The form is race-neutral. A crown of the Northmen is a jarldom and a clan of the Halflings
a Hundred: that naming belongs to the authoring, with the "Factions" line of the race.

### 3. What a faction holds as a Stub

| Fact | Meaning |
|---|---|
| Identity | The region and a slot: 0 the ruling faction, 1 the opposition (empty when there is none), 2 to 5 the lesser factions |
| Type, role, form | Sections 1 and 2 |
| Lead race | The race of its leaders. A faction of the type Race holds that race only. Any other faction is open to the races of its seat |
| Seat | A Prospect of the region |
| Trade | For a guild: the vocations it gathers |
| Ground | For an opposition: what it stands on (section 6) |
| Grip | The ruling faction: 1 − tension. The opposition: the tension. A lesser faction: its fit |

A faction is of one region, as the readme says. Networks across regions and relations
across region borders are left open (see "Open").

### 4. The indices of a region

The rule reads the region through indices from 0 to 1. `mean` is the area-weighted mean
amount over the land patches in their final form. `listed` is the share of the land area
where the amount is 0.25 or more. `ramp(x, lo, hi) = clamp((x − lo) / (hi − lo), 0, 1)`.
The lo and hi values are about the 10th and the 90th percentile of the regions of the
first two seeds.

| Index | Value |
|---|---|
| grain | ramp(mean Grain, 0.17, 0.45) |
| herd | 0.5 · ramp(mean Livestock, 0.33, 0.53) + 0.5 · ramp(mean Horses & pack animals, 0.22, 0.40) |
| open | ramp(share of Cold steppe, Temperate grassland, Savanna, Cold desert, Hot desert, Thorn scrub and Tundra, 0.25, 0.66) |
| forest | ramp(share of Boreal forest, the three temperate forests, the two tropical forests, Mangrove and Woodland & shrubland, 0.29, 0.69) |
| cold | ramp(share of Ice & snow, Tundra and Boreal forest, 0.1, 0.6) |
| dry | ramp(share of Hot desert, Cold desert, Cold steppe and Thorn scrub, 0.1, 0.6) |
| rough | ramp(share of Hills + 1.5 · share of Mountains, 0, 0.45) |
| trade | 0.5 · ramp(the best Trade factor of a Prospect on show, 0.5, 1) + 0.5 · ramp(Prospects on show with a harbour or a navigable river mouth, 0, 4) |
| wealth | 0.5 · ramp(mean Prosperity factor of the Prospects, 0.33, 0.69) + 0.5 · ramp(Rich Prospects on show, 0, 9) |
| urban | ramp(Prospects on show, 6, 19) |
| ore | ramp(listed Iron ore + Copper ore + Tin + Silver & lead + Gold + Gems & pearls + Salt, 0.03, 0.18) |
| precious | ramp(listed Silver & lead + Gold + Gems & pearls, 0, 0.06) |
| arcane | The Supernatural of the region. The test used ramp(mean Arcane materials, 0.002, 0.018) in its place |
| people | ramp(persons, 160, 600) |
| small | 1 − ramp(persons, 60, 260) |
| mixed | 1 − ramp(share of the largest race, 0.24, 0.32) |
| poor | ramp(Poor Prospects on show / Prospects on show, 0, 0.5) |
| uneven | ramp(highest − lowest Prosperity factor of the Prospects on show, 0.1, 0.5) |

So the four things that were asked for all enter: the races (their shares and their
leanings, below), the biomes (open, forest, cold, dry), the resources (grain, herd, ore,
precious, and each trade in section 7) and the wealth (wealth, poor, uneven, trade, urban).

### 5. The ruling faction

**The fit of the land** for each type as the ruler:

```
fit(Family)   = 0.35 + 0.65 · (0.5·grain + 0.3·people + 0.2·(1 − rough))
fit(Race)     = 0.12 + 0.88 · max(herd·open, 0.6·rough + 0.4·forest·(1 − grain), 0.8·small)
fit(Vocation) = 0.05 + 0.95 · trade·(0.4 + 0.6·wealth) · (0.5 + 0.5·urban)
fit(Belief)   = 0.05 + 0.95 · (0.5·arcane + 0.15·mixed + 0.3·poor·(1 − trade))
fit(Magic)    = 0.8 · arcane³
fit(Place)    = 0.10 + 0.90 · max(urban·trade, 0.8·small, 0.7·rough·(1 − people), 0.6·cold)
```

In words: grain land with many persons gets a house. Open herding land, rough land,
forest without grain and very small regions get a people. Trade cities with wealth get
the guilds. Supernatural land, and poor land off the trade routes, get a faith.
Only the most supernatural land gets the mages. Land with many trading towns, very small
regions, thinly peopled rough land and the cold north get self-rule.

**The leaning of the races.** Every race has a leaning from 0 to 3 to each type
(Appendix A): 3 for the kind of group it forms first, 0 for a kind it never forms.

**The draw.** The lead race and the type are drawn together:

```
weight(r, t) = share(r)^5 · lean(r, t) / 3 · fit(t)^2      over the races r of the region and the six types t
(lead race, type) = one draw by weight                       # purpose 0x4b01, the region (i, j)
```

- `share(r)` is the share of the race in the persons of the region. The power 5 makes the
  largest race the lead race in most regions without making it certain.
- The power 2 on the fit is how much the land decides. With 1 the races decide more, with
  a high power the best fit always wins (see "The dials").
- The form: a Race ruler is a horde when `herd·open ≥ max(0.6·rough, 0.35)`, a hold when
  `rough ≥ 0.5` and `ore ≥ 0.4`, else a clan. A Vocation ruler is a merchant republic. A
  Place ruler is a free city when `urban·trade ≥ 0.35`, else a commune.

**The seat** is the best Prospect on show, by score, that holds the lead race (section
5.1 of society-generation.md). When none holds it, the best Prospect on show. This is also
an answer to "what makes a city the capital": the seat of the ruling faction.

### 6. The opposition

**The tension** is how much stands against the ruler:

```
typeWeight(t) = Σ weight(r, t) over the races                       # section 5
contest   = ramp(1 − typeWeight(ruling type) / Σ typeWeight, 0.4, 0.9)
rivalSeat = the highest ramp(score(p) / score(best), 0.8, 1) · ramp(km from p to the best, 40, 100)
            over the Prospects p on show other than the best
low       = 0.5·poor + 0.5·uneven

tension = 0.25·contest + 0.20·rough + 0.20·rivalSeat + 0.15·precious + 0.20·low

there is an opposition when persons ≥ 80 and tension + 0.2·(u − 0.5) ≥ 0.50     # u: purpose 0x4b02
```

| Term | What it says |
|---|---|
| contest | The land and the races would as well carry another kind of ruler. A merchant republic in grain land is contested, a crown there is not |
| rough | Hills and mountains shelter those who do not obey |
| rivalSeat | A second city nearly as good as the first, and far from it |
| precious | Silver, gold and gems pay for a rising and are worth one |
| low | Poor Prospects, and rich and poor Prospects side by side |

**Who opposes.** The type and the lead race of the opposition are drawn together:

```
ground(t)    = 0.25·contest·fit(t) + Σ over the other four terms g: weight(g) · g · favour(g, t) / 2
weight(r, t) = share(r) · lean(r, t) / 3 · cleavage(ruling type, t) · ground(t)
(lead race, type) = one draw by weight                               # purpose 0x4b03
```

- A faction of the type Race is never of the ruler's lead race: a people does not oppose
  its own ruler as a people. A Magic opposition needs `arcane ≥ 0.3`.
- `cleavage` says which type opposes which ruler, 1 to 3:

  | Ruler ↓, opposition → | Family | Race | Vocation | Belief | Magic | Place |
  |---|---|---|---|---|---|---|
  | Family | 3 | 2 | 2 | 2 | 1 | 2 |
  | Race | 2 | 3 | 1 | 1 | 1 | 1 |
  | Vocation | 2 | 1 | 3 | 1 | 1 | 2 |
  | Belief | 2 | 1 | 1 | 3 | 3 | 1 |
  | Magic | 2 | 1 | 1 | 3 | 2 | 2 |
  | Place | 3 | 2 | 2 | 1 | 1 | 1 |

  A crown is opposed by a rival house first, a people by another people, a guild by a
  rival guild, a faith by a sect or by the mages, and a free town by a lord who wants it.
- `favour` says which type each term feeds, 0 to 2:

  | Term | Family | Race | Vocation | Belief | Magic | Place |
  |---|---|---|---|---|---|---|
  | rough | 1 | 2 | 0 | 1 | 0 | 2 |
  | rivalSeat | 2 | 0 | 2 | 0 | 0 | 2 |
  | precious | 2 | 0 | 2 | 0 | 0 | 0 |
  | low | 0 | 1 | 0 | 2 | 0 | 2 |

- **The ground** of the opposition is the largest weighted term of the tension. It is
  kept as a fact, so the authoring knows why the opposition is there.
- **The seat** is a Prospect on show other than the ruler's seat that holds the lead race
  of the opposition: the one with the highest `score · (0.5 + 0.5·ramp(km from the ruler's
  seat, 20, 80))`.

### 7. The lesser factions

```
slots = min(4, ⌊persons / 150⌋)
every kind of the table gets its fit + 0.1·(u − 0.5)         # u: purpose 0x4b04, per kind
the kinds are taken best first while the fit is 0.75 or more and a slot is free
```

- At most one lesser faction of each type other than Vocation.
- No chartered town under a Place ruler and no circle under a Magic ruler: the ruler is
  that council or that circle already.
- `city` is 1 when a Prospect for a city is on show, else 0.4.
- The merchants have a floor of 0.4 in a region with a city, and the four trades of primary
  production (miners, salters, foresters, drovers) a factor of 0.9: merchant guilds came
  first and primary production was rarely guilded [OG14].

| Kind | Type | Vocations it gathers | Fit |
|---|---|---|---|
| Merchants | Vocation | Merchant, Shopkeeper, Carter | (0.4 + 0.6·(0.65·trade + 0.35·wealth)) · city |
| Miners | Vocation | Miner, Quarryman, Smelter | 0.9 · ore |
| Smiths | Vocation | Smith, Artificer | ramp(listed Iron ore, 0.02, 0.08) · (0.5 + 0.5·ramp(mean Fuel, 0.33, 0.6)) · city |
| Mariners | Vocation | Sailor, Fisher, Shipwright | ramp(harbours and mouths on show, 0, 3) · (0.5 + 0.5·ramp(mean Fish, 0.28, 0.41)) |
| Clothiers | Vocation | Weaver, Tailor, Tanner, Leatherworker | (0.5·ramp(mean Fibre, 0.2, 0.47) + 0.3·ramp(mean Dyes, 0.03, 0.17) + 0.2·ramp(mean Livestock, 0.33, 0.53)) · city |
| Builders | Vocation | Mason, Carpenter | (0.6·ramp(mean Stone, 0.04, 0.22) + 0.4·ramp(mean Timber, 0.3, 0.65)) · city · (0.5 + 0.5·urban) |
| Salters | Vocation | Salter | 0.9 · ramp(listed Salt, 0, 0.04) |
| Caravaneers | Vocation | Carter, Merchant, Innkeeper | dry · ramp(mean Horses & pack animals, 0.22, 0.4) · (0.5 + 0.5·ramp(best Trade factor, 0.5, 1)) |
| Foresters | Vocation | Forester, Hunter, Charcoal burner | 0.9 · (0.5·ramp(mean Timber, 0.3, 0.65) + 0.5·ramp(mean Furs & ivory, 0.02, 0.21)) |
| Drovers | Vocation | Herder, Butcher | 0.9 · herd · open |
| Apothecaries | Vocation | Healer, Alchemist | 0.4·ramp(mean Herbs, 0.3, 0.52) + 0.3·ramp(mean Reagents, 0, 0.015) + 0.3·ramp(mean Spices, 0, 0.12) |
| Victuallers | Vocation | Miller, Baker, Brewer, Innkeeper | (0.5·grain + 0.5·ramp(mean Orchard, 0.22, 0.56)) · urban · city |
| Goldsmiths | Vocation | Jeweller | precious · (0.5 + 0.5·wealth) · city |
| Free company | Vocation | Soldier, Guard | 0.5·rough + 0.5·poor |
| Thieves | Vocation | Thief, Beggar | wealth · urban · (0.5 + 0.5·uneven) · city |
| Order | Belief | Priest, Scribe, Healer | 0.35 + 0.65 · max(arcane, 0.6·poor) |
| Circle | Magic | Mage, Alchemist, Seer | arcane |
| Great house | Family | Noble | 0.8 · (0.6·grain + 0.4·people) |
| Chartered town | Place | | urban · (0.4 + 0.6·trade) |
| Enclave | Race | | 0.85 · the highest lean(r, Race) / 3 · (1 for a minority race, 0.6 for a main race) over the races of the region |

**The lead race** of a lesser faction is drawn among the races of the region with the
weight `share(r) · lean(r, type) / 3 · (5 for a minority race, else 1)` (purpose 0x4b05,
per slot). The 5 is the minority boost of society-generation.md section 5.1. An enclave is
of the race that gave its fit. This is where the rare races get their factions (see
"Measured").

**The seat** of a lesser faction was not tested. The proposal: the Prospect on show that
holds its lead race and has the most of what the fit reads within its own and its
neighbouring patches. An enclave sits on the Prospect where its race is largest.

### 8. Where the numbers live

- `Planning.Factions` in the Parameter Set: the race power (5), the sharpness (2), the base
  of each fit, the tension weights, the tension an opposition needs (0.50) and its jitter
  (0.2), the persons an opposition needs (80), the persons per lesser faction (150), the
  most lesser factions (4), the lowest fit of a lesser faction (0.75), and the cleavage
  and favour tables.
- The leaning of a race to each type goes on the race, like the factors per vocation that
  section 8 of society-generation.md plans. Adding a race still touches one record.
- The kinds of lesser faction would be a list in the Parameter Set, like the vocations.
  Whether their fit can be written as data is open.
- Hash purposes 0x4b01 to 0x4b05, as given above.
- Nothing is read from another region, so the stage rules do not change: a region is
  Planned from itself.

## Measured

A prototype of the rule on the 1,154 regions, with the values above.

**The ruling faction**

| Type | Regions | Forms |
|---|---|---|
| Family | 41.1 % | crown 41.1 % |
| Race | 19.9 % | horde 8.8 %, clan 6.3 %, hold 4.9 % |
| Place | 17.4 % | commune 10.7 %, free city 6.8 % |
| Vocation | 10.0 % | merchant republic 10.0 % |
| Belief | 9.7 % | temple state 9.7 % |
| Magic | 1.9 % | magocracy 1.9 % |

- The four seeds agree: Family 35 to 45 %, Race 18 to 24 %, Place 16 to 19 %, Vocation 9
  to 11 %, Belief 8 to 11 %, Magic 1 to 4 %.
- The land shows in the result, moderately. The mean index of the regions under each type
  of ruler, against the mean of all regions:

  | | grain | open | rough | trade | urban | ore | arcane | small |
  |---|---|---|---|---|---|---|---|---|
  | All regions | 0.51 | 0.47 | 0.41 | 0.48 | 0.54 | 0.42 | 0.32 | 0.13 |
  | Family | 0.59 | 0.46 | 0.35 | 0.51 | 0.59 | 0.41 | 0.27 | 0.08 |
  | Race | 0.43 | 0.59 | 0.47 | 0.43 | 0.45 | 0.43 | 0.27 | 0.17 |
  | Vocation | 0.59 | 0.42 | 0.39 | 0.65 | 0.73 | 0.46 | 0.27 | 0.01 |
  | Belief | 0.41 | 0.45 | 0.46 | 0.40 | 0.42 | 0.39 | 0.55 | 0.17 |
  | Magic | 0.46 | 0.49 | 0.49 | 0.40 | 0.33 | 0.35 | 0.94 | 0.25 |
  | Place | 0.42 | 0.40 | 0.46 | 0.45 | 0.48 | 0.41 | 0.31 | 0.20 |

- The ruling type is the type with the best fit in 43 % of the regions. In the others the
  leaning of the races or the draw decided.
- The races show too. Of the regions each race leads: the Orcs are a people in 74 %, the
  Grass riders in 47 %, the Northmen rule themselves (Place) in 42 %, the Midlanders, the
  Canal folk and the Sunlanders have a house in 45 to 46 %, and the Saltroad folk are a
  people in 29 %.
- The lead race is the largest race of the region in 57 % of the regions and a minority
  race in 0.9 %.
- **Only the large races rule.** Midlanders lead 32 % of the ruling factions, Canal folk
  24 %, Saltroad folk 18 %, Sunlanders 15 %, Northmen 4 %, Orcs 3 %, Rainkeepers 2 % and
  Grass riders 1 %. The Halflings rule three regions of the 1,154, the Elves, the Dwarves,
  the Gnomes and the Hoarkin one each, and the other fifteen new races none.
- The seat: a Prospect on show holds the lead race in 99.7 % of the regions, and it is the
  best Prospect of the region in 94 %. It is a Prospect for a city in 81 %.

**The opposition**

- 42.1 % of the regions have one (39 to 46 % per seed).
- By the type of the ruler: Family 19 %, Magic 50 %, Race 54 %, Place 55 %, Belief 64 %,
  Vocation 66 %. A crown sits firmly, because the land that draws a crown seldom suits
  another ruler as well.
- The type of the opposition: Family 31 %, Vocation 21 %, Place 18 %, Belief 14 %, Race
  14 %, Magic 0.8 %. As forms: rival house 31 %, guild 21 %, commune 12 %, sect 10 %,
  hold 6 %, free city 6 %, horde 5 %, order 4 %, clan 4 %, cabal 0.8 %.
- The most frequent pairs: a house against a free town or a commune 9 %, a house against
  a ruling people 8 %, a guild against a ruling guild 7 %, a people against a people 7 %,
  a town or a commune against a crown 6 %, a guild against a free town 6 %, a house
  against a faith 6 %.
- The ground: contest 65 %, rough land 19 %, poor and uneven land 9 %, a rival city 6 %,
  precious finds 1 %.
- The opposition has the lead race of the ruler in 21 % of the cases. The Elves, the
  Dwarves, the Gnomes and the new races together lead 7 % of the oppositions.
- The seat: another Prospect on show holds its lead race in 99.8 % of the cases, 86 km
  from the ruler's seat in the median case.

**The lesser factions**

| Lesser factions in a region | 0 | 1 | 2 | 3 | 4 |
|---|---|---|---|---|---|
| Regions | 22.0 % | 35.4 % | 22.5 % | 14.1 % | 5.9 % |

- 63 % of the regions have at least one guild. 72 % of the lesser factions are guilds,
  12 % enclaves, 8 % orders, 3 % circles, 3 % chartered towns and 1 % great houses.
- Regions with each kind: merchants 20 %, enclave 18 %, clothiers 14 %, mariners 12 %,
  order 12 %, victuallers 10 %, salters 9 %, free company 9 %, smiths 7 %, goldsmiths 5 %,
  chartered town 5 %, circle 5 %, thieves 5 %, foresters 4 %, miners 4 %, apothecaries
  3 %, builders 2 %, great house 2 %, caravaneers 2 %, drovers 1 %.
- **The rare races lead here.** With the boost of 5, a minority race leads 44 % of the
  lesser factions, and half of the regions have a faction led by a minority race. Without
  the boost it is 23 % and 31 %. All 28 races lead a lesser faction somewhere. Of the
  lesser factions the human races lead 63 %, the classic races 19 % and the new races 18 %.

**All factions**

| Factions in a region | 1 | 2 | 3 | 4 | 5 | 6 |
|---|---|---|---|---|---|---|
| Regions | 15.6 % | 26.2 % | 27.6 % | 17.8 % | 10.6 % | 2.3 % |

The mean is 2.9 factions in a region, and 122 persons per faction in the median region.

**Five regions**

| Region | Land and races | Factions |
|---|---|---|
| `cellgen` (8, −8) | 603 persons. Tropical dry forest, Savanna, Hot desert. Canal folk 34 %, Saltroad folk 23 %, Sunlanders 20 % | Ruling: a crown of the Canal folk. Opposition: a free city of the Sunlanders, 105 km away, on the ground of a rival city. Lesser: victuallers, apothecaries, clothiers, merchants |
| `gameworld` (7, 8) | 448 persons. Tropical dry forest, Thorn scrub, Hot desert. Sunlanders 26 %, Saltroad folk 21 %, Rainkeepers 20 % | Ruling: a temple state of the Rainkeepers. Opposition: a house of the Sunlanders, 109 km away, on the ground of contest. Lesser: an enclave, an order |
| `cellgen` (0, 0) | 202 persons. Boreal forest, Temperate mixed forest, Tundra. Midlanders 26 %, Northmen 25 %, Canal folk 24 % | Ruling: a hold of the Northmen. Opposition: a guild of the Midlanders, 51 km away, on the ground of rough land. Lesser: a free company |
| `ember` (0, 0) | 324 persons. Hot desert, Temperate rain forest, Cold desert. Saltroad folk 31 %, Midlanders 19 %, Sunlanders 19 % | Ruling: a horde of the Saltroad folk. No opposition. Lesser: none reached the fit |
| `atlas` (0, 1) | 749 persons. Hot desert, Cold desert, Tropical dry forest. Saltroad folk 27 %, Sunlanders 26 %, Canal folk 23 % | Ruling: a crown of the Saltroad folk. No opposition. Lesser: thieves, merchants, clothiers, victuallers |

## The dials

Each line is one value changed, the others as above. The value of the proposal is bold.

| Dial | Value | Result |
|---|---|---|
| Sharpness (the power on the fit) | 1 | Family 35 %, Place 19 %, Race 17 %, Vocation 15 %, Belief 13 %. The best fit rules in 33 %. Opposition in 50 % |
| | **2** | Family 41 %, Race 20 %, Place 17 %, Vocation 10 %, Belief 10 %. The best fit rules in 43 %. Opposition in 42 % |
| | 4 | Family 52 %, Race 20 %, Place 15 %, Belief 7 %, Vocation 4 %. The best fit rules in 59 %. Opposition in 31 % |
| | 50 (the best fit always) | Family 60 %, Race 24 %, Place 8 %, Belief 5 %, Magic 3 %, Vocation 0.3 %. Opposition in 13 % |
| Race power | 2 | The largest race leads in 38 %, a minority race in 5 % |
| | 3 | 45 % and 2.5 % |
| | **5** | 57 % and 0.9 % |
| | 8 | 67 % and 0.4 % |
| Tension an opposition needs | 0.40 | Opposition in 63 % (under a crown 40 %) |
| | 0.45 | 53 % (30 %) |
| | **0.50** | 42 % (19 %) |
| | 0.55 | 33 % (14 %) |
| | 0.60 | 21 % (8 %) |
| Persons per lesser faction, lowest fit | 150, 0.55 | No lesser faction in 9 % of the regions, a guild in 78 %, 3.4 factions in the mean region |
| | **150, 0.75** | 22 %, 63 %, 2.9 |
| | 150, 0.85 | 43 %, 45 %, 2.3 |
| | 120, 0.75 | 19 %, 66 %, 3.1 |

## What the numbers rest on

- **The fits, the weights and the two tables are starting values.** They were tuned until
  the mix looked right to me: about four crowns in ten, an opposition in four regions in
  ten, a guild in six in ten. No source gives these shares. They are dials for phase 1.
- **The mix is more varied than the one generator that publishes its weights.** Azgaar's
  draws a monarchy for 89 % of its inland states and 69 % of its seafaring ones, after
  the theocracies are taken out [AZ-STATES]. Here a house rules four regions in ten. Part of
  the gap is a matter of names: a horde, a clan and a commune are a tribal monarchy or a
  council in Azgaar's titles. The rest is a choice for variety, and the sharpness and the
  base of each fit move it.
- **The sources give directions, not sizes.** They say that grain favours a house and
  rough land an opposition. How much is my choice.
- **The leanings of the races** (Appendix A) are my reading of one line per race in
  races-basis.md. They were not checked with the user.
- **The Supernatural was not in the data.** The test read the mean amount of Arcane
  materials in its place. The rule must read the Supernatural field of the patches, and
  the lo and hi of `arcane` must then be measured again. Belief, Magic, the order and the
  circle depend on it.
- **Distances were approximate**: 8 km times the difference of the patch coordinates, not
  the distance between the Spots.
- **The prototype is outside the repository**: the generator wrote the regions as json, a
  script reduced them and a second one held the rule, with its own hash. The implemented
  rule will give slightly different numbers.
- **The seats of the lesser factions and their grip were not tested.**

## Weighed and dropped

- **How divided a region is by race, as a cause of opposition.** Measured, it is nearly a
  constant. The polarization index (4 · Σ share² · (1 − share)) is 0.61 to 0.68 in four
  regions in five, and the fractionalization (1 − Σ share²) 0.77 to 0.81. Every region has
  six races with about the same shares. The races decide who opposes, not whether. The
  source note proposes polarization as the sign of an opposition by a people [MRQ]. Its
  simpler test, two races that hold 60 % of the region together, is true in 5 % of the
  regions.
- **Grain against roots as the sign of a house** [MMP]. The share of Grain in Grain and
  Produce together is 0.43 to 0.48 in four regions in five, and the two amounts follow
  each other (correlation 0.97): the map grows both in the same places. The rule reads the
  amount of Grain instead.
- **A mining league as a ruler.** The first version let ore feed rule by the guilds, and
  5 % of the regions were ruled by miners. The sources know chartered miners under a lord,
  not miners as lords [BERGFREI]. Ore now gives a miners' guild and feeds the tension.
- **Sea access as a sign of trade.** 94 % of the regions have it. The harbours and river
  mouths of the Prospects on show tell regions apart.
- **The best fit always rules** (no draw): a crown in 60 % of the regions and a people in
  24 %, a merchant republic in 3 regions of 1,154, and opposition in 13 %. The land is too
  flat for it.
- **Drawing one of the 48 archetypes for each slot** (research/factions.md section 5.1).
  48 weights against 18 indices cannot be tuned by eye. A type first and a form second
  gives six weights.
- **Only the largest race may rule.** A foreign ruler is a story worth having, and with
  the power 5 the lead race is still a main race in 99 % of the regions.
- **A ruling faction for each race of the region.** Six rulers in a region of 360 persons
  is 60 persons each, and the region is the size of one kingdom.
- **Looking at the factions of the neighbouring regions** (a league, a shared church, a
  war). The Planned stage would need the neighbours Planned, and the result would depend
  on the order of generation.
- **Guilds from the vocations of each city.** It is the better rule: a guild where the
  trade has many hands. No vocation is drawn yet, so the fits of section 7 read the
  resources themselves. They should be replaced when section 8 of society-generation.md
  is specified.

## Implemented

**What changed on the way into the generator**

- **The Supernatural.** The index `arcane` reads the field itself: the mean over the land
  of how far it lies above 60, from 0 at 60 to 1 at 100. The field has features of 10 km,
  so every region holds some: the value is 0.010 to 0.028 in four regions in five, where
  the amount of Arcane materials of the test was 0 in many regions. With the range of the
  test (0.002 to 0.018) a faith ruled 19 % of the regions and the mages 14 %. The range is
  now 0.014 to 0.032, the 25th percentile and about the 95th, which gives the index the
  spread it had in the test.
- **A guild in opposition has a trade**: the kind of the type Vocation with the best fit.
  That kind is then not also a lesser faction of the region.
- **An enclave is never of the lead race of the ruler.** Its fit is taken over the other
  races of the region.
- **The seats of the lesser factions** follow a pull: the score of a Prospect, raised by
  the resources of the kind around it, and cut for a kind on water without a harbour or a
  navigable river. An enclave sits where its race has the most persons, away from the seat
  of the ruler.
- **A kind whose vocations are all off is not taken**, and a vocation that is off is left
  out of what a kind gathers.
- **The kinds of lesser faction are a table in the code**, not a list in the Parameter Set:
  their fits are formulas.
- **The ranges of the indices are parameters** (`Planning.Factions.Indices`), 21 of them.
  Only the range of `arcane` was changed after the measurement.
- **The powers** go through the deterministic exponential and logarithm, like the rest of
  the generation.
- **Distances** are between the Spots of the Prospects, and the persons are the potential
  population before each race is rounded.

**Measured with the generator** on the same 1,154 regions:

| | Prototype | Generator |
|---|---|---|
| Regions without persons | 2 of 1,156 | 0 of 1,154: the prototype dropped two regions whose persons round to 0 |
| Ruling: Family / Race / Place / Vocation / Belief / Magic | 41.1 / 19.9 / 17.4 / 10.0 / 9.7 / 1.9 % | 42.8 / 18.5 / 16.6 / 10.6 / 10.0 / 1.6 % |
| The type with the best fit rules | 43 % | 43 % |
| The lead race is the largest race / a minority race | 57 % / 0.9 % | 54 % / 1.0 % |
| The seat of the ruler holds its lead race / is the best Prospect / is for a city | 99.7 / 94 / 81 % | 99.6 / 94 / 80 % |
| Regions with an opposition | 42.1 % | 41.7 % |
| Opposition under a Crown / under another ruler | 19 % / 50 to 66 % | 22 % / 50 to 67 % |
| Opposition: House / Guild / Sect / Commune / Free city | 31 / 21 / 10 / 12 / 6 % | 35 / 19 / 10 / 9 / 8 % |
| Ground: contest / rough land / rival seat / poor land / precious finds | 65 / 19 / 6 / 9 / 1 % | 64 / 20 / 7 / 6 / 3 % |
| Km from the seat of the ruler to that of the opposition, median | 86 | 86 |
| Lesser factions in a region: 0 / 1 / 2 / 3 / 4 | 22.0 / 35.4 / 22.5 / 14.1 / 5.9 % | 24.3 / 33.9 / 23.5 / 12.5 / 5.9 % |
| Regions with a guild | 63 % | 60 % |
| Merchants / enclave / clothiers / order / mariners, regions with one | 20 / 18 / 14 / 12 / 12 % | 21 / 18 / 15 / 12 / 10 % |
| A minority race leads a lesser faction / regions with one | 44 % / 51 % | 45 % / 50 % |
| Lesser factions on the seat of the ruler | not tested | 53 %: merchants 75 %, mariners 39 %, salters 33 %, enclaves 0 % |
| Factions in a region, mean | 2.9 | 2.8 |

The four seeds agree: a Crown in 42 to 44 % of the regions, a people in 12 to 24 %, an
opposition in 41 to 42 %.

**The tests** (`tests/Gameworld.Tests/FactionTests.cs`): the leanings of the default races;
the kinds and their vocations; a region without persons has no faction; one ruling faction
of a type its race leans to; the race power; the land moves the fits; the tension and the
persons decide the opposition; the lesser factions follow the persons and the fit; a
Planned region holds its factions by slot, with lead races of the region and seats on its
Prospects on show. The test that generation does not depend on the order of the regions
compares the whole plan of a region, so it covers the factions.

## Open

Decided with the adoption (2026-11-19): the six types with Place (1), the name "lesser
faction" (2), the mix as measured (3), and that a region without persons has no faction
(11). The names follow what was asked for, not the five types of the source note (12). The
rest is open:

1. **The types.** Is Place a type? Are six right, and are these the names?
2. **The roles.** "Lesser faction" is a new name. "Minor faction" is the usual one in
   games, but it reads like "minority race".
3. **The mix.** Four crowns in ten, an opposition in four regions in ten, a guild in six in
   ten, 1 to 6 factions in a region: are these the world that is wanted?
4. **Who rules.** The human races lead nearly every ruling faction and most oppositions.
   The rare races lead lesser factions. Should a Dwarven hold or an Elven realm rule a
   region now and then? A lower race power gives it (5 % at the power 2), at the cost of
   rulers from small races everywhere.
5. **Belief.** A Belief faction has no faith to believe in: the readme has no such
   concept. The rule only says that there is a temple state, a sect or an order.
6. **Factions that cross regions.** A church, a merchant league and a horde span realms in
   every source (research/factions.md section 5.5). The proposal keeps a faction in one
   region, as the readme defines it.
7. **Relations.** Inside a region the ruler and the opposition are opposed, and a lesser
   faction could lean to one of them. Across borders it needs the neighbours Planned, so
   it belongs to the Settled stage.
8. **The whole region or the seat.** The rule reads the whole region, which is flat. A
   rule that reads the land around each Prospect, and lets the Prospects contend for the
   rule, would follow the land more closely. It was not tested.
9. **Outlaws.** The thieves are a guild here. Bandits, pirates and cults away from the
   Prospects (research/factions.md section 5.3) are not covered.
10. **Near and far.** The sources tie the kind of opposition to where the wealth lies:
    near the seat a claimant, far from it a breakaway [LB]. The rule reads how much
    precious ore a region has, not where. The patches are known, so it can be added.
11. **A region without persons** has no faction: two of the 1,156 regions. Is that right,
    or should every region with land hold at least a ruler?
12. **The five types of the source note.** It proposes Dynasty, Kin, Guild, Faith and
    Arcane (research/faction-generation.md section 6.2). They are the first five types
    here under other names. The names here follow what was asked for.

## Steps

All nine are done.

1. `readme.md`: Faction gets the types, the roles, the lead race and the seat. The Race
   concept says how a faction gets its race.
2. `docs/society-generation.md` section 4: the rule, the parameters and the purposes.
   Section 2: the leaning per type on a race. Section 8: the capital is the seat of the
   ruling faction.
3. `src/Gameworld.Domain`: `FactionParameters` on `Planning`, the leanings on
   `RaceParameters`, a `Faction` record (`Factions.cs`), and `RegionPlan` gets the factions
   of the region. `docs/default-parameter-set.json` and the schema with them.
4. `src/Gameworld.Generation/Factions/FactionRules.cs`: the indices, the fits, the three
   steps and the kinds of lesser faction. The Supernatural of the patches is read here.
5. `PlanBuilder`: call it after the races and the Prospects.
6. The demo app: the inspect panel shows the factions of a region. The map generation app
   prints the number of factions of each region and writes them with the region.
7. Tests: every region has exactly one ruling faction, at most one opposition and at most
   four lesser factions; every lead race is a race of the region; every seat is a Prospect
   on show of the region; the result does not depend on the order of generation.
8. Measure again with the generator, and set the lo and hi of the indices from it. Only
   the range of `arcane` moved.
9. `docs/map-generation-research.md`: a section for the factions with the design table.

## Appendix A. The leaning of the races

0 never, 1 seldom, 2 fits, 3 the first kind of group the race forms. From the "Factions"
line of each entry of races-basis.md.

| Race | Family | Race | Vocation | Belief | Magic | Place | Read from |
|---|---|---|---|---|---|---|---|
| Northmen | 2 | 1 | 2 | 1 | 1 | 3 | A Thing district of free farmers, a jarldom, a ship fellowship |
| Midlanders | 3 | 1 | 2 | 2 | 1 | 2 | A kingdom, a barony, a free town, a craft guild, a monastery |
| Saltroad folk | 2 | 2 | 3 | 2 | 1 | 1 | A caravan company, a merchant house, an emirate, a confederation of clans |
| Sunlanders | 3 | 2 | 2 | 2 | 1 | 2 | A grassland empire, a cattle clan, a walled city-state, a caste of smiths |
| Grass riders | 2 | 3 | 1 | 1 | 1 | 0 | A clan and its herds, a horde under a khan |
| Rainkeepers | 2 | 1 | 2 | 3 | 1 | 2 | A temple town and its lands, a king's court, a merchant guild |
| Canal folk | 2 | 1 | 3 | 1 | 1 | 1 | A governor's court, an academy, a merchant house, a state monopoly |
| Elves | 2 | 3 | 1 | 1 | 2 | 1 | A forest realm, a house, an Open house that teaches |
| Dwarves | 1 | 3 | 3 | 1 | 1 | 1 | A hold under its king, a clan, a craft guild inside a hold |
| Halflings | 2 | 1 | 1 | 1 | 0 | 3 | A Hundred under its elected reeve, a moot, a great family |
| Gnomes | 1 | 1 | 3 | 1 | 2 | 2 | A warren under its masters, a family workshop, a college of makers' marks |
| Orcs | 1 | 3 | 2 | 1 | 1 | 0 | A warband under a chief, a horde, a company for hire |
| Hoarkin | 1 | 2 | 2 | 1 | 1 | 3 | A winter lodge, the summer moot, a hunting company |
| Hearthbloods | 2 | 2 | 1 | 1 | 1 | 2 | A hall under its larder-lord, a league of halls |
| Widegaze | 1 | 2 | 2 | 1 | 2 | 2 | A mound village, the Watch, a herding kindred, a circle of star watchers |
| Amberbloods | 1 | 2 | 2 | 2 | 1 | 2 | A grove village, a wardenship, a timber court |
| Brinefolk | 1 | 1 | 3 | 2 | 1 | 2 | A pan town under its pan masters, a salt league, the Keepers of the Bed |
| Duskreaders | 2 | 1 | 2 | 1 | 2 | 2 | A canyon town under the elders of its houses, a night caravan company |
| Noonfolk | 0 | 1 | 2 | 2 | 1 | 3 | A free commune under a noon council, a grain cooperative, the Vigil |
| Boughwalkers | 2 | 2 | 2 | 1 | 1 | 2 | A canopy village, a tree lineage, a dye house |
| Shoalfolk | 1 | 1 | 2 | 1 | 1 | 3 | A stilt village under a tide council, a pilots' guild |
| Swiftborn | 0 | 1 | 2 | 1 | 1 | 3 | A town and its Long Work, a harvest gang, a year-class |
| Flinchless | 1 | 1 | 3 | 1 | 1 | 1 | A legion village under a captain, a free company, a surgeons' hall |
| Lanternkin | 1 | 1 | 3 | 1 | 2 | 1 | A guild of notaries, a ring of insurers, a lamp court |
| Stillfaces | 1 | 2 | 2 | 1 | 2 | 1 | A village kindred, a lodge of trackers, a circle of diagnosticians |
| Twofold | 3 | 1 | 1 | 2 | 1 | 1 | A house and its chain, a temple of the Pair |
| Bitterbloods | 1 | 2 | 3 | 1 | 2 | 2 | A swamp village, an apothecaries' house, a school of poisoners |
| Catfolk | 1 | 2 | 2 | 2 | 1 | 1 | A granary ward, a hunting band, a temple of the Small Huntress |
