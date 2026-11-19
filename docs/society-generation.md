# How the society is generated

This document describes how the map is populated: the races, the potential population, the
factions, the cities and towns, the families and the persons, with their vocations and
their looks. It is the specification of the society generation, as
[map-generation.md](map-generation.md) is of the map. The [readme](../readme.md) defines
the concepts and holds the names, and the names in this document follow the readme.

Only the first steps are specified and implemented: the races (section 2), the races of a
region with their potential population (section 3), the factions of a region (section 4)
and the races of a city or a town (section 5.1). The rest of sections 5 to 9 holds what is
decided and what is still open. Detailing it is a later task.

Sections 2 and 3 and Appendix A were section 19 and Appendix E of map-generation.md until
2026-11-15. The research notes cite them under those numbers.

## Contents

1. [Overview](#1-overview)
2. [Races](#2-races)
3. [Potential population](#3-potential-population)
4. [Factions](#4-factions)
5. [Cities and towns](#5-cities-and-towns)
6. [Families](#6-families)
7. [Persons](#7-persons)
8. [Vocations](#8-vocations)
9. [Looks](#9-looks)

Appendices: [A. The default races](#appendix-a-the-default-races) ·
[B. Source material](#appendix-b-source-material)

---

## 1. Overview

The society is a layer on top of the map. It reads the map and never changes it: the
biome, the height and the resources of the patches, the Prosperity field, the Prospects
and the travel network are its inputs.

| Step | Region generation stage | Status |
|---|---|---|
| Races | none: a list in the Parameter Set | Specified and implemented (section 2) |
| The races of a region and its potential population | Planned | Specified and implemented (section 3) |
| Factions | Planned | Specified and implemented (section 4): a ruling faction, an opposition and lesser factions per region |
| Cities and towns | Settled; their races Planned | The races of a city or a town are specified and implemented (section 5.1), the rest is not specified |
| Families | Settled | Not specified (section 6) |
| Persons | Settled for the important persons, later for the rest | The names are implemented, the rest is not specified (section 7) |
| Vocations | Planned for the countryside, Settled for the cities and towns | The list is in the Parameter Set. The rules are decided in part and not specified (section 8) |
| Looks | not decided | Not specified (section 9) |

### 1.1 What holds for every step

These rules come from the readme and are decided.

- **Stubs are derived data.** Every gameplay-relevant fact of a city, a faction, a family
  or a person is a pure function of the foundation. The rules of the map hold here too:
  every random choice comes from a hash of (foundation seed, entity identity, purpose),
  and no result depends on the order the regions are generated in.
- **Presentation is authored data.** Descriptions, histories and portraits come from an
  LLM or a diffusion model. They cost to produce, so they are stored as the source of
  truth and are never regenerated or evicted. Authored data must not contradict the Stub.
- **Identity is deterministic.** A city, a faction, a family or a person is its parent
  plus a slot index. So a Stub is addressable before it is Realised, and player state and
  authored data can point at it. The parent of a faction is its region (section 4.2). The
  parent of a city, a family and a person has not been decided.
- **Only a small and relevant set is precalculated.** The game supports a very large
  number of persons, cities and towns. Only a small and relevant set of them is generated
  at game start, and more are generated as play progresses.

### 1.2 Entity generation states

Cities, factions, families and persons each have their own generation state. It is
independent of the region generation stage:

```
Stub → Pending → Realised
```

- A **Stub** holds every gameplay-relevant fact, and is derived data. For a person: name,
  age, sex, vocation, role, family links, faction membership and home.
- **Pending** means the authoring by LLM or diffusion model is in flight. On failure the
  entity returns to Stub.
- **Realised** adds the authored presentation only. For a person: the textual
  description, the history and the portrait.

Text and portrait are tracked separately, so a person can be shown with a description and
a history before the picture arrives. A city can exist with a population count long before
any individual in it is authored.

---

## 2. Races

A race is a people that can live in the world: Northmen, Elves, Dwarves and so on. Races
are needed to generate factions, families and persons.

The races are a list in the Parameter Set, not a fixed set in the code. The list is
expected to change late in phase 2, so a race is added or changed by editing the list
(see [adding-a-race.md](adding-a-race.md)). Each race has:

- An **id** and a **name**. Generated data points at a race by its id. The ids must be
  given and unique. A Parameter Set that breaks this is refused when the world is created.
- A **toggle**: the race is on or off. A race that is off does not live in the world.
- A **frequency for each of the 16 biomes**: how frequent the race is in that biome,
  relative to the other races. 0 means the race does not live there.
- A **height modifier**: a factor on the frequencies of the race that depends on the
  height of the land. It can cut a race off at a height or fade it in and out. A race
  without a height modifier lives at every height.
- A **leaning to each of the six types of faction**, from 0 to 3: 3 for the kind of group
  the race forms first, 0 for a kind it never forms. It weighs the race in the draws for
  the type and the lead race of a faction (section 4.4). A race without a leaning leans to
  every type alike, with 1.

In a Parameter Set file a race lists only the biomes it lives in. A biome that is left out
has the frequency 0, and a frequency of 0 is not written to the file. A race without a
height modifier does not carry one in the file.

| Parameter | Default |
|---|---|
| Races | 28, all on (Appendix A) |
| Height modifiers | Elves, Dwarves, Hearthbloods, Widegaze and Shoalfolk (Appendix A) |
| Leanings to the types of faction | One row per race (Appendix A) |

The default list was decided on 2026-11-15. It holds:

- 7 human races: Northmen, Midlanders, Saltroad folk, Sunlanders, Grass riders,
  Rainkeepers and Canal folk.
- 5 classic races: Elves, Dwarves, Halflings, Gnomes and Orcs.
- 16 new near-human races: Hoarkin, Hearthbloods, Widegaze, Amberbloods, Brinefolk,
  Duskreaders, Noonfolk, Boughwalkers, Shoalfolk, Swiftborn, Flinchless, Lanternkin,
  Stillfaces, Twofold, Bitterbloods and Catfolk.

All 28 are on. The height does not matter to the races without a height modifier.
[races-basis.md](races-basis.md) describes every race: its looks, its history, its way of
life, its vocations and its factions.

**Human peoples are separate races** (decided 2026-11-14). Race is the only term: there is
no people or culture level under one human race, and the default list has no plain human
race.

**A race has no sub-races** (decided 2026-11-15). A people that differs is a race of its
own in the list.

Open:

- How a family or a person gets its race, and whether a family can be mixed. A faction
  has a lead race (section 4.2).
- What a race can ask for beyond the biome and the height: coast, river, elevation type,
  bedrock, the Supernatural and Prosperity fields, a town, another race to live among
  (collected in races-basis.md, "What the entries ask for that a race cannot say yet").
- A presence per race, so that a race clusters in some regions. A region holds six races
  (section 3.3), but the turns spread a minority race evenly over the world.
- Size, lifespan and fertility per race.
- The factors per vocation, the switch for the division of work by sex and whether the
  race keeps slaves. They are decided in section 8 and are not in the Parameter Set yet.

---

## 3. Potential population

Stage: Planned.

The potential population is an estimate of how many persons the land of a region can
feed, for each race of the region. It is not a count of people that exist: cities, families
and persons come in the Settled stage.

A region does not hold every race that is on. It holds its **main races**, the largest by
the plain shares of section 3.2, and its **minority races**, which get their place by turn
(section 3.3). Only the races of the region share its land (section 3.4).

| Parameter | Default |
|---|---|
| Persons per km² at a food supply of 1 and a middling Prosperity | 0.06 |
| Prosperity effect | 0.5 |
| Races per region | 6 |
| Minority races per region | 2 |
| Persons a race needs in a region to take a minority place | 0.1 |
| Share of the region a minority race is lifted towards | 0.05 |

Races per region 28 with no minority races gives every race that is on its plain share.

The inputs are the final form of the region's land patches (the Elevated stage,
map-generation.md sections 12 to 14): the height of each patch and the biome and resources
calculated with it. The height of a patch is its mean height in the height map, not below
0, or the typical height of its elevation type (map-generation.md section 13.1) when no
raster cell centre lies in the patch. So the potential population depends on the height
map, unlike the Prospects (map-generation.md section 15). Sections 3.1, 3.2 and 3.4 hold no
random choice. The turns and the draws of section 3.3 use the hash purposes 0x4a01 and
0x4a02.

### 3.1 Capacity of a patch

Every land patch of the region has a capacity in persons:

```
supply   = clamp(food + 0.2 · fish, 0, 1)        # food: the summary of map-generation.md 14.5; fish: the amount on the patch
factor   = 1 + effect · (2 · prosperity − 1)     # prosperity at the patch's site, rescaled to 0–1
capacity = area · personsPerKm2 · supply · factor
```

- `area` is the number of patch cells of the patch times the square of the patch grid
  size, in km² (64 km² per patch cell at the defaults). Lakes are not taken out.
- `effect` is the Prosperity effect, clamped to 0–1. With the default 0.5 the factor runs
  from 0.5 in the poorest land to 1.5 in the richest. 0 turns the effect off.
- Ocean patches have no capacity.

### 3.2 Plain shares of the races

The plain shares say how well a patch suits each race. They decide the main races of a
region (section 3.3), and their weights are what the races of a region share a patch by
(section 3.4).

On one patch the races that are on share the land by their frequency in its biome, each
multiplied by the height factor of the race at the height of the patch:

```
weight(r, patch) = frequency(r, biome of patch) · heightFactor(r, height of patch)
share(r, patch)  = weight(r, patch) / Σ weight(q, patch)   over the races q that are on
```

- A negative frequency counts as 0.
- If the sum is 0, no race that is on lives on the patch, by its biome or its height. The
  patch holds nobody, and its capacity is not handed to another patch.
- A race that is off is left out of the sum. Its share goes to the races that are on, so
  toggling a race changes how the land is shared, not how many persons it feeds.
- Only the proportions between the races on one patch matter. Doubling every frequency
  of one race doubles its weight against the others everywhere.

So the races compete for the same land.

**Height factor.** The height modifier of a race is a list of control points
`(height in m, factor)`. A race without one has the factor 1 at every height.

```
heightFactor(r, h):
    points = the control points of r, sorted by height; points at the same height keep the order of the list
    if there are no points:        return 1
    if h < height of the first:    return factor of the first
    a = the last point with height ≤ h
    if a is the last point:        return factor of a
    b = the point after a
    return factor(a) + (h − height(a)) / (height(b) − height(a)) · (factor(b) − factor(a))
```

- The factor runs in a straight line between two points and is constant below the first
  and above the last.
- Two points at the same height make a step: the first holds below that height, the
  second from that height and up. `(200, 0), (200, 1)` is a race that only lives from
  200 m and up. `(200, 1), (200, 0)` is a race that only lives below 200 m.
- `(100, 0), (300, 1)` fades a race in: none below 100 m, half weight at 200 m, full
  weight from 300 m.
- A negative factor counts as 0. A factor above 1 makes the race more frequent at that
  height.

### 3.3 The races of a region

A region holds up to 6 races: 4 main races and 2 minority races.

```
plain(r) = Σ capacity(patch) · share(r, patch)        # sections 3.1 and 3.2, over the region's land patches in key order

M      = the number of races that are on
b      = minority races per region, not above races per region
order  = the races that are on, sorted by hash4(seed, 0x4a01, k, 0), then by k   # k: index among the races that are on
offset = hash4(seed, 0x4a01, −1, 0) mod M
W      = the stride of the turn pattern (below); 6 for 28 races

main races:
    the (races per region − b) races with the largest plain(r) > 0; ties in the order of the Parameter Set

minority place z = 0 … b−1 of region (i, j):
    eligible = the races that are on, are not yet a race of the region, and have plain(r) > 0 and plain(r) ≥ 0.1
    turn     = order[(i + W·j + offset + z·⌊M / b⌋) mod M]
    if turn is eligible:     turn takes the place
    else if any is eligible: one is drawn, each with the weight plain(r)
    else:                    the place stays empty
```

- **The main races** follow the frequencies and the height modifiers alone: they are the
  races the land of the region suits best.
- **The turn** is how an under-represented race is boosted: it takes the place by its
  turn, not by its size. It only needs the land of the region to feed 0.1 of it by the
  plain shares, so it takes no place where it has next to no land.
- **The turn pattern.** The regions where one race has its turn form a lattice: the
  regions with the same `(i + W·j) mod M`. `W` is the number in 1 … M−1 for which the
  shortest step between two such regions is the longest, and the lowest such number. With
  28 races `W` is 6 and the steps are (−2, 5) and (4, 4) regions, about 5.5 regions or
  700 km. Every 6 × 5 block of regions then holds a turn for every race, in each of the two
  minority places.
- **The draw** uses one sfc32 generator per region, seeded with
  `hash4(seed, 0x4a02, i, j)`. It gives one number per minority place, in the order of the
  places, also for a place that is taken by turn. The eligible races are walked in the
  order of the Parameter Set, and the first whose running sum of weights passes
  `u · the sum of the weights` is drawn.
- The order and the offset come from the seed, so two worlds do not share a pattern. They
  also depend on which races are on: toggling a race moves every turn.
- The region is identified by its grid coordinate `(i, j)`. Nothing is read from another
  region, so the result does not depend on the order the regions are generated in.

### 3.4 Potential population of a region

Only the races of the region share its land. A minority race is lifted towards a share
of the region, so that it is large enough to be met.

```
boost(r) = 1                                           for a main race
s(r)     = the share of r in the region when every boost is 1
boost(r) = t · (1 − s) / (s · (1 − t))  if 0 < s < t   for a minority race, t = the minority share (0.05)
           1                            otherwise

weight(r, patch) = frequency(r, biome of patch) · heightFactor(r, height of patch) · boost(r)
share(r, patch)  = weight(r, patch) / Σ weight(q, patch)   over the races q of the region
population(r)    = round(Σ capacity(patch) · share(r, patch))   over the region's land patches, in key order
```

- The result has one entry for every race that is on, in the order of the Parameter Set,
  and says whether the race is a main race or a minority race of the region or not a race
  of it. A race that is not a race of the region has 0.
- If the sum is 0 on a patch, none of the races of the region lives there. The patch holds
  nobody, and its capacity is not handed to another patch or another race.
- The lift is exact for a race that is spread evenly over the region. A race that lives on
  a few patches takes those patches and stays below the share: the Shoalfolk hold the low
  coast of a region, not a twentieth of all of it. A minority share of 0 turns the lift
  off, and it is clamped to 0–0.9.
- Each race is rounded on its own, half up, so the sum over the races can differ from the
  rounded sum of the capacities by a few persons.

The land patches of a region are the patches of its zones (map-generation.md section
6.2), so every land patch counts in exactly one region.

With the defaults the median region has a potential population of about 364 and the
range is 7 to 1,188 (measured on 1,079 regions of four seeds at the earlier density of 6
persons per km², and divided by 100). Every one of them holds
six races. The largest race of a region holds 27 % of it in the median region and 19 to
43 % over all regions, so no race has a majority anywhere. A minority race holds 4.9 % of
its region in the median case, and about 20 persons. 0.9 % of the capacity lies on patches
that hold nobody.

Of all persons 83.5 % are of the seven human races, 10.4 % of the five classic races and
6.1 % of the sixteen new races. By the plain shares it would be 58.7 %, 20.2 % and 21.1 %:
the main races are human in most regions. The largest race is the Canal folk with 22 %.
They are a race of 90 % of the regions, the Elves of 10 %, the Dwarves of 8 % and each
new race of 6 to 10 %.

A cluster of 30 regions holds all 28 races in 96 % of the cases and 27 in nearly all the
others (the 30 regions nearest a region, 432 clusters). With one minority race per region
it is about a quarter, whether the region holds five or six races. The shares per race are in
[races-basis.md](races-basis.md), "Measured shares", and the test of the rule in
[plans/race-caps.md](plans/race-caps.md).

---

## 4. Factions

Stage: Planned. The factions need the region itself only: its land in its final form, its
races (section 3) and its Prospects with their races (section 5.1). Nothing is read from
another region.

A faction is a group of people in one region: a political grouping. Every region with
persons has one **ruling faction**. Some have an **opposition**, and some have **lesser
factions**: guilds mostly. The rule was adopted on 2026-11-19 as proposed in
[plans/region-factions.md](plans/region-factions.md), which holds the test behind it and
what was weighed. Its fits, weights and tables are starting values for phase 1.

| Parameter | Default |
|---|---|
| Race power | 5 |
| Sharpness | 2 |
| Base fit: Family, Race, Vocation, Belief, Magic, Place | 0.35, 0.12, 0.05, 0.05, 0, 0.10 |
| Tension weights: contest, rough land, rival seat, precious finds, poor land | 0.25, 0.20, 0.20, 0.15, 0.20 |
| Tension an opposition needs | 0.50 |
| Tension jitter | 0.2 |
| Persons an opposition needs | 80 |
| Persons per lesser faction | 150 |
| Most lesser factions | 4 |
| Lowest fit of a lesser faction | 0.75 |
| Lesser jitter | 0.1 |
| Cleavage and favour | the tables of section 4.6 |
| Index ranges | the table of section 4.3 |
| Leaning of each race to each type | Appendix A |

The parameters are `Planning.Factions` in the Parameter Set. The leaning is
`FactionLeaning` on each race. The draws use the hash purposes 0x4b01 to 0x4b05.

### 4.1 The types

The type of a faction is what binds its members. Each type rests on a fact of a person.

| Type | Bound by | The fact of a person it rests on |
|---|---|---|
| **Family** | blood and marriage: a house and its household | family links |
| **Race** | a people: one race, its clans and its customs | race |
| **Vocation** | work: a trade, its masters and its monopoly | vocation |
| **Belief** | a faith: a temple, an order, a sect | belief (not a concept of the readme yet) |
| **Magic** | the arcane: a college, a circle, a cabal | the supernatural vocations |
| **Place** | a home: a town or a valley that rules itself | home |

Arms and crime are not types: a free company and a thieves' guild are bound by work. A
cause is not a type: it is a role, the opposition.

### 4.2 The roles, the forms and the Stub

| Role | Per region | Meaning |
|---|---|---|
| **Ruling** | exactly 1; none in a region without persons | It governs the region |
| **Opposition** | 0 or 1 | It wants the rule, or wants out of it |
| **Lesser** | 0 to 4 | It lives under the ruler: a guild, an order, a circle, a great house, a chartered town, an enclave |

The **form** is the name of a type in a role. It is a label for the authoring and no rule
reads it. It is race-neutral: a crown of the Northmen is a jarldom.

| Type | Ruling | Opposition | Lesser |
|---|---|---|---|
| Family | Crown | House | House |
| Race | Horde, Clan or Hold | Horde, Clan or Hold | Enclave |
| Vocation | Merchant republic | Guild | Guild |
| Belief | Temple state | Sect or Order | Order |
| Magic | Magocracy | Cabal | Circle |
| Place | Free city or Commune | Free city or Commune | Chartered town |

- A Race ruler or opposition is a Horde when `herd · open ≥ max(0.6 · rough, 0.35)`, a
  Hold when `rough ≥ 0.5` and `ore ≥ 0.4`, else a Clan.
- A Place ruler or opposition is a Free city when `urban · trade ≥ 0.35`, else a Commune.
- A Belief opposition is a Sect when `poor > 0.4`, else an Order.

The Stub of a faction holds:

| Fact | Meaning |
|---|---|
| Slot | Its identity is the region and the slot: 0 the ruling faction, 1 the opposition, 2 to 5 the lesser factions. Slot 1 is empty in a region without an opposition |
| Role, type, form | As above |
| Lead race | The race of its leaders. A faction of the type Race holds that race only. Any other faction is open to the races of its seat |
| Seat | A Prospect on show of the region. None in a region without a Prospect on show |
| Kind | The kind of a lesser faction (section 4.7), and the trade of a guild in opposition |
| Vocations | The vocations a guild, an order and a circle gather |
| Ground | What an opposition stands on (section 4.6) |
| Grip | 0 to 1. The ruling faction: 1 − the tension. The opposition: the tension. A lesser faction: its fit |

The seat of the ruling faction is the capital of the region.

### 4.3 The indices of a region

The rule reads the region through indices from 0 to 1.

```
ramp(x, low, high) = clamp((x − low) / (high − low), 0, 1)
mean(k)   = the area-weighted mean amount of resource k over the land patches, in their final form
listed(k) = the share of the land area where the amount of resource k is 0.25 or more
persons   = the potential population of the region, not rounded (section 3.4)
share(r)  = the persons of race r / persons
```

The area of a patch is that of section 3.1. The Prospects are all Prospects of the region
where the row says so, else the ones on show. A harbour is a Prospect on show with a
harbour, or with a river mouth of 8 or more.

| Index | Value | Low | High |
|---|---|---|---|
| grain | ramp of mean(Grain) | 0.17 | 0.45 |
| herd | 0.5 · ramp of mean(Livestock) | 0.33 | 0.53 |
| | + 0.5 · ramp of mean(Horses & pack animals) | 0.22 | 0.40 |
| open | ramp of the share of the land in Cold steppe, Temperate grassland, Savanna, Cold desert, Hot desert, Thorn scrub and Tundra | 0.25 | 0.66 |
| forest | ramp of the share in Boreal forest, the three temperate forests, the two tropical forests, Mangrove and Woodland & shrubland | 0.29 | 0.69 |
| cold | ramp of the share in Ice & snow, Tundra and Boreal forest | 0.1 | 0.6 |
| dry | ramp of the share in Hot desert, Cold desert, Cold steppe and Thorn scrub | 0.1 | 0.6 |
| rough | ramp of the share in Hills zones + 1.5 · the share in Mountains zones | 0 | 0.45 |
| trade | 0.5 · ramp of the best Trade factor of a Prospect on show | 0.5 | 1 |
| | + 0.5 · ramp of the number of harbours | 0 | 4 |
| wealth | 0.5 · ramp of the mean Prosperity factor of all Prospects of the region | 0.33 | 0.69 |
| | + 0.5 · ramp of the number of Rich Prospects on show | 0 | 9 |
| urban | ramp of the number of Prospects on show | 6 | 19 |
| ore | ramp of listed(Iron ore) + listed(Copper ore) + listed(Tin) + listed(Silver & lead) + listed(Gold) + listed(Gems & pearls) + listed(Salt) | 0.03 | 0.18 |
| precious | ramp of listed(Silver & lead) + listed(Gold) + listed(Gems & pearls) | 0 | 0.06 |
| arcane | ramp of the area-weighted mean of clamp((Supernatural − 60) / 40, 0, 1) over the land patches, with the Supernatural field at the site of each patch | 0.014 | 0.032 |
| people | ramp of persons | 160 | 600 |
| small | 1 − ramp of persons | 60 | 260 |
| mixed | 1 − ramp of the largest share(r) | 0.24 | 0.32 |
| poor | ramp of the Poor Prospects on show / the Prospects on show; 0 without one on show | 0 | 0.5 |
| uneven | ramp of the highest − the lowest Prosperity factor of the Prospects on show; 0 with fewer than two | 0.1 | 0.5 |

- The lows and highs are parameters. They are about the 10th and the 90th percentile of
  generated regions, so a change to the map parameters can call for new ones. The range of
  `arcane` is the 25th percentile and about the 95th: the Supernatural field is fine
  grained, so every region has some, and only the upper half counts.
- `city` is 1 when a Prospect for a city is on show, else 0.4. The fits of section 4.7 use
  it.

### 4.4 The ruling faction

The fit of the land for each type as the ruler:

```
fit(t) = base(t) + (1 − base(t)) · land(t)

land(Family)   = 0.5·grain + 0.3·people + 0.2·(1 − rough)
land(Race)     = max(herd·open, 0.6·rough + 0.4·forest·(1 − grain), 0.8·small)
land(Vocation) = trade · (0.4 + 0.6·wealth) · (0.5 + 0.5·urban)
land(Belief)   = 0.5·arcane + 0.15·mixed + 0.3·poor·(1 − trade)
land(Magic)    = 0.8 · arcane³
land(Place)    = max(urban·trade, 0.8·small, 0.7·rough·(1 − people), 0.6·cold)
```

Grain land with many persons gets a house. Open herding land, rough land, forest without
grain and very small regions get a people. Trading towns with wealth get the guilds.
Supernatural land, and poor land off the trade routes, get a faith. Only the most
supernatural land gets the mages. Land with many trading towns, very small regions,
thinly peopled rough land and the cold north get self-rule.

The lead race and the type are drawn together:

```
lean(r, t)   = the leaning of race r to type t / 3                 # Appendix A; a negative leaning counts as 0
weight(r, t) = share(r)^race power · lean(r, t) · fit(t)^sharpness   over the races r of the region and the six types t
(lead race, type) = one draw by weight, with u = toUnit(hash4(seed, 0x4b01, i, j))
```

- The weights are walked race by race in the order of the Parameter Set, and for each race
  type by type in the order Family, Race, Vocation, Belief, Magic, Place. The first whose
  running sum passes `u · the sum of the weights` is drawn.
- The powers go through the deterministic exponential and logarithm. A power of 0 gives 1.
- If every weight is 0, the lead race is the largest race of the region and the type the
  one with the best fit.
- The race power makes the largest race lead in most regions without making it certain.
  The sharpness is how much the land decides: with 1 the leanings decide more, with a high
  number the best fit always wins.

The seat is the Prospect on show with the highest score among those that hold the lead
race (section 5.1). When none holds it, the Prospect on show with the highest score. Ties
go to the first in the order of the Prospects.

### 4.5 The tension

```
typeWeight(t) = Σ weight(r, t) over the races
contest   = ramp(1 − typeWeight(ruling type) / Σ typeWeight, 0.4, 0.9)
rivalSeat = the highest ramp(score(p) / score(best), 0.8, 1) · ramp(km from p to best, 40, 100)
            over the Prospects p on show other than best, the one with the highest score
poorLand  = 0.5·poor + 0.5·uneven

tension = w(contest)·contest + w(rough land)·rough + w(rival seat)·rivalSeat
        + w(precious finds)·precious + w(poor land)·poorLand

there is an opposition when persons ≥ the persons an opposition needs
    and tension + jitter · (u − 0.5) ≥ the tension an opposition needs,   u = toUnit(hash4(seed, 0x4b02, i, j))
```

| Term | What it says |
|---|---|
| contest | The land and the races would as well carry another kind of ruler. A merchant republic in grain land is contested, a crown there is not |
| rough land | Hills and mountains shelter those who do not obey |
| rival seat | A second Prospect nearly as good as the best, and far from it. The distance is between the two Spots |
| precious finds | Silver, gold and gems pay for a rising and are worth one |
| poor land | Poor Prospects, and rich and poor Prospects side by side |

How divided a region is by race is not a term: every region holds six races of about the
same size, so it is nearly the same everywhere. The races decide who opposes, not whether.

### 4.6 The opposition

```
ground(t)    = w(contest)·contest·fit(t)
             + (w(rough land)·rough·favour(rough land, t) + w(rival seat)·rivalSeat·favour(rival seat, t)
              + w(precious finds)·precious·favour(precious finds, t) + w(poor land)·poorLand·favour(poor land, t)) / 2
weight(r, t) = share(r) · lean(r, t) · cleavage(ruling type, t) · ground(t)
(lead race, type) = one draw by weight, with u = toUnit(hash4(seed, 0x4b03, i, j)), walked as in section 4.4
```

- A faction of the type Race is never of the lead race of the ruler: a people does not
  oppose its own ruler as a people. A Magic opposition needs `arcane ≥ 0.3`.
- If every weight is 0 the region has no opposition.
- `cleavage` says which type opposes which ruler:

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
- `favour` says which type each term feeds:

  | Term | Family | Race | Vocation | Belief | Magic | Place |
  |---|---|---|---|---|---|---|
  | rough land | 1 | 2 | 0 | 1 | 0 | 2 |
  | rival seat | 2 | 0 | 2 | 0 | 0 | 2 |
  | precious finds | 2 | 0 | 2 | 0 | 0 | 0 |
  | poor land | 0 | 1 | 0 | 2 | 0 | 2 |

- **The ground** of the opposition is the term with the largest weighted value in the
  tension: contest, rough land, rival seat, precious finds or poor land, the first of them
  in a tie. It is kept so that the authoring knows why the opposition is there.
- **A guild in opposition** has a trade: the kind of the type Vocation with the highest fit
  (section 4.7, without the random part). That kind is then not also a lesser faction.
- **The seat** is the Prospect on show, other than the seat of the ruler, with the highest
  `score · (0.5 + 0.5 · ramp(km from the seat of the ruler, 20, 80))` among those that hold
  the lead race of the opposition. When none holds it, among all the others. In a region
  with one Prospect on show it shares the seat of the ruler.

### 4.7 The lesser factions

```
slots = min(the most lesser factions, ⌊persons / the persons per lesser faction⌋)
for every kind of the table, in its order: value = fit + lesser jitter · (u − 0.5)
the kinds are taken by value, highest first, while value ≥ the lowest fit and a slot is free
```

- `u` comes from one sfc32 generator per region, seeded with `hash4(seed, 0x4b04, i, j)`. It
  gives one number per kind in the order of the table, also for a kind that is not taken.
- A kind is passed over, and the next is tried, when:
  - a lesser faction of its type is taken already, for every type but Vocation;
  - it is the chartered town under a Place ruler, or the circle under a Magic ruler: the
    ruler is that council or that circle already;
  - it is the trade of the guild in opposition;
  - none of the vocations it gathers is on;
  - it is the enclave and the region has no other race than the lead race of the ruler.
- The kinds are a table in the code. The fits read the resources of the land. They should
  read the vocations of the cities and towns once those are drawn (section 8).

| Kind | Type | Vocations it gathers | Fit | Seat resources |
|---|---|---|---|---|
| Merchants' guild | Vocation | Merchant, Shopkeeper, Carter | (0.4 + 0.6·(0.65·trade + 0.35·wealth)) · city | |
| Miners' guild | Vocation | Miner, Quarryman, Smelter | 0.9 · ore | Iron ore, Copper ore, Tin, Silver & lead, Gold, Gems & pearls |
| Smiths' guild | Vocation | Smith, Artificer | ramp(listed(Iron ore), 0.02, 0.08) · (0.5 + 0.5·ramp(mean(Fuel), 0.33, 0.6)) · city | Iron ore |
| Mariners' guild | Vocation | Sailor, Fisher, Shipwright | ramp(harbours, 0, 3) · (0.5 + 0.5·ramp(mean(Fish), 0.28, 0.41)) | Fish; on water |
| Clothiers' guild | Vocation | Weaver, Tailor, Tanner, Leatherworker | (0.5·ramp(mean(Fibre), 0.2, 0.47) + 0.3·ramp(mean(Dyes), 0.03, 0.17) + 0.2·ramp(mean(Livestock), 0.33, 0.53)) · city | Fibre, Dyes |
| Builders' guild | Vocation | Mason, Carpenter | (0.6·ramp(mean(Stone), 0.04, 0.22) + 0.4·ramp(mean(Timber), 0.3, 0.65)) · city · (0.5 + 0.5·urban) | Stone, Timber |
| Salters' guild | Vocation | Salter | 0.9 · ramp(listed(Salt), 0, 0.04) | Salt |
| Caravan company | Vocation | Carter, Merchant, Innkeeper | dry · ramp(mean(Horses & pack animals), 0.22, 0.4) · (0.5 + 0.5·ramp(best Trade factor, 0.5, 1)) | Horses & pack animals |
| Foresters' guild | Vocation | Forester, Hunter, Charcoal burner | 0.9 · (0.5·ramp(mean(Timber), 0.3, 0.65) + 0.5·ramp(mean(Furs & ivory), 0.02, 0.21)) | Timber, Furs & ivory |
| Drovers' guild | Vocation | Herder, Butcher | 0.9 · herd · open | Livestock, Horses & pack animals |
| Apothecaries' guild | Vocation | Healer, Alchemist | 0.4·ramp(mean(Herbs), 0.3, 0.52) + 0.3·ramp(mean(Reagents), 0, 0.015) + 0.3·ramp(mean(Spices), 0, 0.12) | Herbs, Reagents, Spices |
| Victuallers' guild | Vocation | Miller, Baker, Brewer, Innkeeper | (0.5·grain + 0.5·ramp(mean(Orchard), 0.22, 0.56)) · urban · city | Grain, Orchard |
| Goldsmiths' guild | Vocation | Jeweller | precious · (0.5 + 0.5·wealth) · city | Gold, Silver & lead, Gems & pearls |
| Free company | Vocation | Soldier, Guard | 0.5·rough + 0.5·poor | |
| Thieves' guild | Vocation | Thief, Beggar | wealth · urban · (0.5 + 0.5·uneven) · city | |
| Order | Belief | Priest, Scribe, Healer | 0.35 + 0.65 · max(arcane, 0.6·poor) | |
| Circle | Magic | Mage, Alchemist, Seer | arcane | Arcane materials |
| Great house | Family | Noble | 0.8 · (0.6·grain + 0.4·people) | |
| Chartered town | Place | | urban · (0.4 + 0.6·trade) | |
| Enclave | Race | | 0.85 · the highest lean(r, Race) · (1 for a minority race, 0.6 for a main race) over the races of the region other than the lead race of the ruler | |

The merchants have a floor in a region with a city, and the four trades of primary
production (miners, salters, foresters, drovers) a factor of 0.9: merchant guilds came
first and primary production was rarely guilded
([research/faction-generation.md](research/faction-generation.md) section 5). A vocation
that is off is left out of what a kind gathers.

**The lead race** of a lesser faction is drawn among the races of the region, walked in the
order of the Parameter Set, each with the weight

```
share(r) · lean(r, type) · (the minority boost of section 5.1 for a minority race, else 1)
```

- The draws use one sfc32 generator per region, seeded with `hash4(seed, 0x4b05, i, j)`. It
  gives one number per lesser faction, in the order they are taken, and none for an
  enclave.
- If every weight is 0 the lead race is the largest race of the region.
- An enclave is of the race that gave its fit, the first such race in the order of the
  Parameter Set.
- The boost is where the rare races get their factions: a minority race leads nearly half
  of the lesser factions.

**The seat** of a lesser faction is the Prospect on show with the highest pull among those
that hold its lead race, and among all on show when none does:

```
local(p) = the highest amount of a seat resource of the kind on the patch of p and its neighbouring land patches of the region; 0 for a kind without
pull(p)  = score(p) · (1 + local(p)) · (0.25 if the kind is on water and p has no harbour and no river of 8 or more, else 1)
```

An enclave sits on the Prospect on show, other than the seat of the ruler, where its race
has the most persons on the patch of the Prospect and its neighbouring land patches
(`local(r)` of section 5.1). It only takes the seat of the ruler when no other Prospect on
show holds its race.

### 4.8 Measured

The generator on 1,154 regions with land (the 17 × 17 regions around (0, 0) of the seeds
`cellgen`, `gameworld`, `atlas` and `ember`), with the default Parameter Set. Every one of
them has persons and a ruling faction.

| Ruling faction | Regions |
|---|---|
| Family: Crown | 42.8 % |
| Race: Horde 8.1 %, Clan 6.3 %, Hold 4.1 % | 18.5 % |
| Place: Commune 10.5 %, Free city 6.1 % | 16.6 % |
| Vocation: Merchant republic | 10.6 % |
| Belief: Temple state | 10.0 % |
| Magic: Magocracy | 1.6 % |

- The type with the best fit rules 43 % of the regions. In the others the leaning of the
  races or the draw decided.
- The lead race is the largest race of the region in 54 % of the regions and a minority
  race in 1 %. Midlanders lead 32 % of the ruling factions, Canal folk 25 %, Saltroad folk
  17 %, Sunlanders 14 %, Northmen 4 %, Rainkeepers 3 %, Orcs 3 % and Grass riders 1 %. All
  other races together lead 10 regions.
- The seat holds the lead race in 99.6 % of the regions. It is the best Prospect of the
  region in 94 % and a Prospect for a city in 80 %.
- 41.7 % of the regions have an opposition: 22 % of those under a Crown, 50 to 67 % of
  those under another ruler. It is a House in 35 % of the cases, a Guild in 19 %, a Sect in
  10 %, a Commune in 9 %, a Free city in 8 %, an Order in 5 %, a Clan in 5 %, a Hold in
  5 %, a Horde in 4 % and a Cabal in 1 %. Its ground is contest in 64 %, rough land in
  20 %, a rival seat in 7 %, poor land in 6 % and precious finds in 3 %. It sits 86 km from
  the seat of the ruler in the median case.
- Lesser factions: none in 24 % of the regions, one in 34 %, two in 24 %, three in 13 % and
  four in 6 %. 60 % of the regions have a guild. Regions with each kind: merchants 21 %,
  enclave 18 %, clothiers 15 %, order 12 %, mariners 10 %, victuallers 10 %, salters 9 %,
  free company 7 %, smiths 6 %, chartered town 5 %, goldsmiths 5 %, thieves 5 %, foresters
  4 %, circle 4 %, miners 3 %, builders 2 %, apothecaries 2 %, great house 2 %, caravan
  company 2 %, drovers 1 %.
- A minority race leads 45 % of the lesser factions, and half of the regions have a
  faction led by a minority race. All 28 races lead a lesser faction somewhere. The human
  races lead 64 % of them, the classic races 19 % and the new races 17 %.
- 53 % of the lesser factions sit on the seat of the ruler: three merchants' guilds in
  four, two mariners' guilds in five, one salters' guild in three and no enclave.
- A region has 1 to 6 factions and 2.8 in the mean: one in 17 % of the regions, two in
  26 %, three in 28 %, four in 18 %, five in 9 % and six in 2 %.

Open:

- A Belief faction has no faith to believe in: belief is not a concept of the readme.
- Factions that cross regions (a church, a merchant league, a horde), and how factions
  stand to each other across region borders. Both need the bordering regions, so they
  belong to the Settled stage.
- Whether the rule should read the land around each Prospect instead of the whole region,
  which is flat: a region spans many biomes.
- Where the wealth lies: the sources tie the kind of opposition to precious finds near the
  seat or far from it. The rule reads how much a region has, not where.
- Outlaws away from the Prospects: bandits, pirates and cults.
- The kinds of lesser faction as a list in the Parameter Set, and their fits from the
  vocations of the cities and towns (section 8).
- How a person gets its faction membership (section 7).

Material: [research/factions.md](research/factions.md) holds 48 faction archetypes, the
axes that tell them apart and their relations.
[research/faction-generation.md](research/faction-generation.md) holds how generators and
games compute the kind of ruler of a land, and what decides a ruler, an opposition and
guilds in the scholarship. Every entry of [races-basis.md](races-basis.md) lists the kinds
of group the race forms.

---

## 5. Cities and towns

Stage: Settled. Not specified yet, apart from the races of a city or a town (section 5.1).

A city or a town is an actual settlement on a Prospect. The Prospects are the map's
answer to where a settlement can lie, and how good the place is
(map-generation.md section 15). A region has 20 Prospects by default, of which up to 4 are
for cities.

Open:

- Which Prospects become cities and towns.
- How the potential population of a region is turned into the population of its cities,
  its towns and the land between them.
- What a city holds as a Stub, and what the parent of its identity is.
- The names of cities and regions. [RegionNameGenerator-Reuse.md](RegionNameGenerator-Reuse.md)
  describes the region-name generator of an earlier project. It has not been ported.

### 5.1 The races of a city or a town

Stage: Planned. The races are worked out for every Prospect of the region, on show or not,
so a city or a town takes them from its Prospect. They need the region itself only.

| Parameter | Default |
|---|---|
| Races per city | 5 |
| Races per town | 4 |
| Minority boost | 5 |
| Hinterland share | 0.02 |

A city or a town holds races of its region only: the largest around its Prospect, and a
last one that is drawn with the minority races of the region weighted up.

```
local(r)  = Σ capacity(patch) · share(r, patch)   # section 3.4, over the Prospect's patch and its neighbouring land patches of the region
weight(r) = local(r) + hinterland share · population(r)        # population(r) of section 3.4, not rounded
n         = races per city for a Prospect for a city, races per town for a Prospect for a town

main races: the n − 1 races of the region with the largest weight(r) > 0; ties in the order of the Parameter Set
last race:  one is drawn among the other races of the region with weight(r) > 0, each with
            weight(r) · (minority boost if r is a minority race of the region, else 1)
```

- The draw uses `u = toUnit(hash4(seed, 0x4a03, I, J))`, with `(I, J)` the root cell of the
  Prospect's patch. The races are walked in the order of the Parameter Set, as in section
  3.3.
- The result is the list of race ids, the largest first and the drawn one last.
- The hinterland share lets a race of the region that does not live near the Prospect into
  the city or town. With 0 a city or a town only holds races of its own patches.
- A minority boost of 1 gives the minority races no advantage, and 0 keeps them out of the
  last place.
- With `n` 0 the city or town holds no race. It never holds more races than its region.

With the defaults every city Prospect holds five races and every town Prospect four
(measured on the same 1,079 regions: 3,652 Prospects for cities and 17,612 for towns). A
minority race is in a city or a town on show of its region in 97 % of the cases, and in
half of them in the median case. A cluster of 30 regions holds all 28 races in its cities
and towns on show in 90 % of the cases.

How many persons of each race a city or a town holds is open, with the population of the
city or town itself.

---

## 6. Families

Stage: Settled. Not specified yet.

A family is a family of NPCs, typically living together in a household. It can include
servants or slaves.

Open:

- What a family holds as a Stub, and what the parent of its identity is.
- How families follow from the population of a city or a town.
- The race of a family, and how family links between persons are drawn.

---

## 7. Persons

Stage: Settled for the important persons of a region. Other persons are generated later,
as play progresses. Not specified yet, apart from the names.

A person is an NPC in the game. The Stub of a person holds name, age, sex, vocation, role,
family links, faction membership and home. The vocation can have a by-work beside it
(section 8). The textual description, the history and the portrait are authored
(section 1.2).

**Names.** The name of a person is not authored. It comes from the name generator, so a
person has a name already as a Stub. The generator is a character-level Markov chain with
nine pregenerated models. The code is `src/Gameworld.Generation/Names`, and
[NameGenerator-Reuse.md](NameGenerator-Reuse.md) describes it.

Open:

- Which persons of a region are important, and how many there are.
- How age, sex, vocation, role, family links, faction membership and home are drawn.
- Which name model a race uses.

---

## 8. Vocations

Stage: Planned for the countryside, Settled for the cities and towns. Not specified yet.

A vocation is the work a person does: farmer, smith, priest, guard and so on.

The rules below were decided on 2026-11-15. The list of vocations is in the Parameter Set
since 2026-11-19 (see "In the Parameter Set" below). No other rule is specified in detail
or implemented, and no vocation is drawn for a place or a person.

**Vocation and role are two facts.** The vocation is the work, one of the list, with a
variant (a herder who is a shepherd). The role is the position of the person: in the
household, in the vocation (apprentice, master) or in a faction. A slave who is a smith
has the vocation Smith and the role slave. The Stub of a person holds both.

**A person has one vocation and can have a by-work.** The by-work is a second vocation
beside the main one: the farmer who brews, the smith with a field. Every vocation lists
the by-works that fit it, and a person can only get one of those. By-work is more frequent
in the countryside than in a town or a city.

**The vocations are a list in the Parameter Set**, like the races. The code knows no
vocation by name. The default list has 51 vocations in nine sectors:

| Sector | Vocations |
|---|---|
| Land and water | Farmer, Gardener, Herder, Fisher, Hunter, Forester, Miner, Quarryman, Salter |
| Processing | Miller, Baker, Brewer, Butcher, Charcoal burner, Smelter, Tanner, Weaver |
| Craft | Smith, Carpenter, Shipwright, Mason, Potter, Glassmaker, Leatherworker, Tailor, Jeweller, Artist, Artificer |
| Trade and transport | Merchant, Shopkeeper, Carter, Sailor, Innkeeper |
| Service | Healer, Servant, Entertainer, Courtesan |
| Governance and war | Noble, Official, Soldier, Guard |
| Faith and learning | Priest, Scribe, Scholar |
| Supernatural | Mage, Alchemist, Seer |
| Margins | Labourer, Beggar, Thief, Bandit |

A smaller trade is a variant of one of them: the monk of Priest, the armourer of Smith,
the guide of Hunter, the dyer of Weaver, the boatman of Sailor and the banker of Merchant.

**In the Parameter Set.** The list is `Vocations`. A vocation holds:

| Value | Meaning |
|---|---|
| `Id` | What the generated data knows the vocation by. Unique in the list |
| `Name` | The name shown |
| `Enabled` | Whether the vocation exists in the world |
| `Sector` | One of the nine sectors |
| `LowestTier` | Countryside, Town, City or Capital |
| `SharePercent` | The starting share of the labour force of a whole region, in per cent |
| `OnWater` | The vocation is only found in a place on water: the shipwright and the sailor. Left out means no |
| `Variants` | The names of the smaller trades the vocation covers |

The defaults of `LowestTier`, `SharePercent`, `OnWater` and `Variants` are those of
[research/vocations.md](research/vocations.md) section 3: the index table and the
"Covers" line of each vocation. The shares sum to 100. They are starting values and are
not tuned against generated regions. A vocation does not yet hold its inputs and outputs,
its by-works or its share by sex.

**Products are names only.** A vocation lists what it consumes and what it produces by id.
The ids are the resources of the map and the products made from them: iron, cloth, flour,
and the by-products wool, hides, dairy, tallow, horn, tanbark, potash, resin and pitch. A
product has no amount on the map and does not flow between places. The lists say which
vocations a place can have (a smelter needs ore and fuel) and what a place must bring in
or can send out (smiths without a smelter in reach bring in iron).

**A rural person belongs to a patch.** There are no villages. The vocations of the
countryside are worked out per land patch, from its resources and its potential
population. The vocations of a city or a town are worked out per city or town.

**The tiers are relative.** A vocation has a lowest tier in which it is found:
countryside, town, city or capital. The tier of a place follows from what the place is,
not from its head count.

**A race shapes the vocations of its people.** A race can carry a factor per vocation:
above 1 for the work the race is known for, and 0 for work the race never does. The
factors live on the race, so adding a race touches one record.

**Work is divided by sex as in the medieval record.** Every vocation carries a share by
sex. A race can switch the division off for its people. The Twofold do.

**Slave is a role, and slavery is a fact of the race.** Whether a race keeps slaves is
said on the race.

**The supernatural vocations are rare and follow the Supernatural Field.** Mages,
alchemists and seers are few, and their share rises and falls with the field at the place.

**The map gets the resources the vocations lack.** Three new generated resources:
reagents (alum, sulphur, quicksilver, saltpetre), herbs, and arcane materials, which
follow the Supernatural Field. Two sets of new variants: gypsum and alabaster of Stone,
and elephants, reindeer and sled dogs of Horses & pack animals. Their rules are in
[map-generation.md](map-generation.md) section 14 and Appendices C, D and G, and they
are implemented.

Open:

- What more a vocation holds in the Parameter Set: the inputs and outputs, the by-works,
  the shares by sex and the condition of each variant. Whether the starting shares and
  lowest tiers of the research note stand.
- The rule that gives the vocations of a place from its resources, its size, its races and
  the fields, and the purpose numbers of its draws.
- The list of roles, and which roles fit which vocations.
- The age at which a person gets a vocation, and what a child and an old person hold.
- The factors per race, which races keep slaves, and which races other than the Twofold
  switch the division by sex off.
- The capital of a region is the seat of its ruling faction (section 4.4). What a vocation
  of the tier capital needs beyond that.

Material: [research/vocations.md](research/vocations.md) describes each of the 51
vocations (what it covers, its lore, its resources, where it is found, how frequent it is,
its races and factions), maps the resources and the biomes to the vocations, proposes the
new resources and holds a design note for the generator. Every entry of
[races-basis.md](races-basis.md) lists the work the race is known for.

---

## 9. Looks

Not specified yet.

The looks of a person are what a portrait must show: the body and the face, the hair, the
outfit and the accessories. The portrait itself is authored by a diffusion model.

Open:

- Which looks are facts of the Stub and which are left to the authoring.
- How the looks follow from the race, the sex, the age, the vocation and the climate of
  the home.
- The lists to draw from, and where they live.
- Whether the outfits get a list per vocation or per sector of vocations. This waits for
  the vocation list of section 8 to be specified.

Material: the Appearance part of every entry of [races-basis.md](races-basis.md) says what
a portrait of the race must show. [research/portrait-components.md](research/portrait-components.md)
holds lists of hairstyles, outfits, hair, skin and eye colours, race markers and
accessories, a fit to races and climates, and what the image-model vendors say about
keeping a person consistent between pictures.

---

## Appendix A. The default races

The default list of races of the Parameter Set (section 2): 28 races, all on. The first
seven are the human races, the next five the classic races and the last sixteen the new
races. [races-basis.md](races-basis.md) describes each of them.

The frequency of a race in a biome is its habitat weight in that biome times how common
the race is:

```
frequency(race, biome) = habitat(race, biome) · common(race)
```

The habitat weights are 0 avoided, 1 tolerated, 2 good and 3 native. The Parameter Set
only holds the frequencies: the Midlander frequency in Temperate grassland is 3 · 5 = 15.
A biome with the weight 0 is left out of the race's entry in the file.

Columns: Ice & snow, Tundra, Cold steppe, Boreal forest, Cold desert, Temperate grassland,
Woodland & shrubland, Temperate mixed forest, Temperate broadleaf forest, Temperate rain
forest, Hot desert, Thorn scrub, Savanna, Tropical dry forest, Tropical rain forest,
Mangrove.

| Id | Name | Common | Ice | Tun | CSt | Bor | CDs | TGr | WSh | TMx | TBl | TRn | HDs | Thn | Sav | TDf | TRf | Mgv |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| `northmen` | Northmen | 3 | 1 | 2 | 1 | 3 | 0 | 1 | 0 | 3 | 1 | 2 | 0 | 0 | 0 | 0 | 0 | 0 |
| `midlanders` | Midlanders | 5 | 0 | 0 | 1 | 1 | 0 | 3 | 2 | 2 | 3 | 2 | 0 | 0 | 0 | 0 | 0 | 0 |
| `saltroadFolk` | Saltroad folk | 3 | 0 | 0 | 0 | 0 | 2 | 1 | 2 | 0 | 0 | 0 | 3 | 3 | 2 | 1 | 0 | 0 |
| `sunlanders` | Sunlanders | 4 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 1 | 2 | 3 | 3 | 2 | 1 |
| `grassRiders` | Grass riders | 3 | 0 | 1 | 3 | 1 | 2 | 3 | 1 | 0 | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 |
| `rainkeepers` | Rainkeepers | 4 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 2 | 3 | 2 | 2 |
| `canalFolk` | Canal folk | 5 | 0 | 0 | 1 | 0 | 0 | 2 | 1 | 2 | 3 | 3 | 0 | 0 | 0 | 2 | 1 | 1 |
| `elf` | Elves | 2 | 0 | 0 | 0 | 2 | 0 | 1 | 1 | 3 | 3 | 3 | 0 | 0 | 0 | 1 | 2 | 0 |
| `dwarf` | Dwarves | 2 | 1 | 2 | 1 | 2 | 1 | 1 | 1 | 2 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 |
| `halfling` | Halflings | 2 | 0 | 0 | 1 | 1 | 0 | 3 | 3 | 2 | 2 | 1 | 0 | 0 | 1 | 1 | 1 | 0 |
| `gnome` | Gnomes | 1 | 0 | 1 | 1 | 2 | 1 | 1 | 2 | 2 | 3 | 1 | 0 | 1 | 1 | 1 | 0 | 0 |
| `orc` | Orcs | 2 | 0 | 1 | 3 | 1 | 2 | 2 | 2 | 1 | 1 | 1 | 2 | 2 | 2 | 1 | 0 | 0 |
| `hoarkin` | Hoarkin | 1 | 3 | 2 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| `hearthbloods` | Hearthbloods | 0.5 | 2 | 3 | 1 | 2 | 0 | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| `widegaze` | Widegaze | 1 | 0 | 0 | 3 | 0 | 1 | 3 | 0 | 0 | 0 | 0 | 0 | 1 | 2 | 0 | 0 | 0 |
| `amberbloods` | Amberbloods | 1 | 0 | 0 | 0 | 3 | 0 | 0 | 0 | 3 | 2 | 2 | 0 | 0 | 0 | 0 | 0 | 0 |
| `brinefolk` | Brinefolk | 1 | 0 | 0 | 1 | 0 | 3 | 0 | 0 | 0 | 0 | 0 | 2 | 1 | 0 | 0 | 0 | 1 |
| `duskreaders` | Duskreaders | 1 | 0 | 0 | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 3 | 2 | 1 | 0 | 0 | 0 |
| `noonfolk` | Noonfolk | 1 | 0 | 0 | 0 | 0 | 0 | 1 | 1 | 0 | 0 | 0 | 2 | 3 | 3 | 1 | 0 | 0 |
| `boughwalkers` | Boughwalkers | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 0 | 2 | 3 | 1 |
| `shoalfolk` | Shoalfolk | 1 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2 | 0 | 0 | 0 | 1 | 1 | 3 |
| `swiftborn` | Swiftborn | 1 | 0 | 0 | 0 | 0 | 0 | 2 | 3 | 1 | 2 | 0 | 0 | 1 | 1 | 0 | 0 | 0 |
| `flinchless` | Flinchless | 0.5 | 0 | 0 | 1 | 0 | 0 | 2 | 2 | 1 | 2 | 0 | 0 | 0 | 1 | 0 | 0 | 0 |
| `lanternkin` | Lanternkin | 0.5 | 0 | 0 | 0 | 0 | 0 | 1 | 1 | 1 | 1 | 2 | 0 | 0 | 1 | 2 | 2 | 1 |
| `stillfaces` | Stillfaces | 1 | 0 | 0 | 0 | 2 | 0 | 0 | 1 | 3 | 2 | 1 | 0 | 0 | 0 | 0 | 0 | 0 |
| `twofold` | Twofold | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 1 | 2 | 3 | 2 | 1 |
| `bitterbloods` | Bitterbloods | 0.5 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 0 | 1 | 3 | 3 |
| `catfolk` | Catfolk | 1 | 0 | 0 | 1 | 0 | 1 | 2 | 1 | 1 | 1 | 1 | 2 | 2 | 3 | 2 | 2 | 1 |

**Height modifiers.** Control points `(height in m, factor)` (section 3.2).

| Race | Points | Meaning |
|---|---|---|
| Elves | `(200, 1), (200, 0)` | Only below 200 m |
| Dwarves | `(200, 0), (200, 1)` | Only from 200 m and up |
| Hearthbloods | `(100, 0.5), (400, 1)` | Half weight below 100 m, full weight from 400 m |
| Widegaze | `(300, 1), (500, 0)` | Full weight below 300 m, none from 500 m |
| Shoalfolk | `(20, 1), (80, 0)` | Full weight below 20 m, none from 80 m |

The other 23 races have none.

**Leanings to the types of faction.** 0 never, 1 seldom, 2 fits, 3 the kind of group the
race forms first (section 4.4). They are read from the "Factions" line of each entry of
races-basis.md, and they are starting values.

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

**How common.** `common` is 3 to 5 for the human races, 2 for Elves, Dwarves, Halflings
and Orcs, 1 for Gnomes, and 1 or 0.5 for a new race: 1 when it has a homeland of its own,
0.5 when it lives scattered among others or is rare by its nature.

The seven human races live in every biome and at every height between them, so no land
is without a race by the plain shares. A patch can still hold nobody, when none of the six
races of its region lives on it (section 3.4). In Ice & snow there are Northmen, Hoarkin and Hearthbloods, and
dwarves from 200 m and up.

---

## Appendix B. Source material

What exists on the society outside this document. None of it is binding: a rule is decided
when it stands in sections 1 to 9.

| Document | What it holds | Used by |
|---|---|---|
| [adding-a-race.md](adding-a-race.md) | The steps to add a race, to one world or to the default list | Section 2 |
| [races-basis.md](races-basis.md) | The 28 races of the default list. Each entry has a description, a history, the biomes, the vocations and the factions of the race. Also the measured shares per race | Sections 2, 3.3, 4, 8, 9, Appendix A |
| [races-and-worldbuilding.md](races-and-worldbuilding.md) | The framework the new races of the proposal were built with | Section 2 |
| [map-generation-research.md](map-generation-research.md) §10.7 | Why the race and potential population rules are as they are, and what was weighed against them | Sections 2, 3 |
| [research/races.md](research/races.md) | 41 race archetypes from RPGs, fantasy settings and strategy games, and the habitat weights of the first default list of 23 races. The classic races and the Catfolk keep their weights from it. The races that were left out are still described there | Section 2, Appendix A |
| [research/human-races.md](research/human-races.md) | An earlier candidate list in which every race looks like a regular human or nearly so. Not adopted | Section 2 |
| [research/factions.md](research/factions.md) | Faction archetypes, their axes and relations, and a design note | Section 4 |
| [research/faction-generation.md](research/faction-generation.md) | How generators and games compute the kind of ruler of a land, and the scholarship on what decides a ruler, an opposition and guilds | Section 4 |
| [plans/region-factions.md](plans/region-factions.md) | The plan behind the factions of a region: the proposal, its test on 1,154 regions, what was weighed and what changed when it was implemented | Section 4 |
| [research/vocations.md](research/vocations.md) | 51 vocations with their lore, resources, biomes, races and factions, the proposals for new resources, and a design note | Section 8 |
| [tools-basis.md](tools-basis.md) | 146 real tools for the 51 vocations, each with a sourced description, lore and the fitting vocations. Material: no rule reads it | Sections 8, 9 |
| [research/portrait-components.md](research/portrait-components.md) | Lists of hairstyles, outfits, colours, race markers and accessories for portraits. Written for the 32 races of the first version of races-basis.md and extended for the Catfolk: it still covers the five that were left out | Section 9 |
| [NameGenerator-Reuse.md](NameGenerator-Reuse.md) | The person-name generator, ported from an earlier project | Section 7 |
| [RegionNameGenerator-Reuse.md](RegionNameGenerator-Reuse.md) | The region-name generator of the same project, not ported | Section 5 |
