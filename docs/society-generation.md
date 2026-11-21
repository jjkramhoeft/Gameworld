# How the society is generated

This document describes how the map is populated: the races, the potential population, the
factions, the cities and towns, the households and the persons, with their vocations and
their looks. It is the specification of the society generation, as
[map-generation.md](map-generation.md) is of the map. The [readme](../readme.md) defines
the concepts and holds the names, and the names in this document follow the readme.

These steps are specified and implemented: the races (section 2), the races of a
region with their potential population (section 3), the factions of a region (section 4)
and its name (section 4.10), the races of a city or a town (section 5.1), the cities
and towns of a region with their persons (section 5.2) and their names (section 5.3), the
households of the cities and towns (section 6), the persons of the households (section 7)
and the tools of the vocations (section 8.1). The portrait components of section 9 are
specified and in the Parameter Set: the lists the looks of a person are picked from, and
what the looks hold. No rule draws them yet. The rest of sections 8 and 9 holds what is
decided and what is still open. Detailing it is a later task.

Sections 2 and 3 and Appendix A were section 19 and Appendix E of map-generation.md until
2026-11-15. The research notes cite them under those numbers.

## Contents

1. [Overview](#1-overview)
2. [Races](#2-races)
3. [Potential population](#3-potential-population)
4. [Factions](#4-factions)
5. [Cities and towns](#5-cities-and-towns)
6. [Households](#6-households)
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
| The name of a region | Planned | Specified and implemented (section 4.10): from the region-name generator |
| Cities and towns | Settled; their races and their names Planned | Specified and implemented: the races of a city or a town (section 5.1), which Prospects get one and how many persons live in each (section 5.2), and the name of each (section 5.3) |
| Households | Housed | Specified and implemented (section 6): the households of the factions, then the others of the cities and the towns. The households of the land are not specified |
| Persons | Peopled | Specified and implemented (section 7): the persons of every household, with name, sex, age, family relations and vocation. The persons of the land are not specified |
| Vocations | Peopled for the persons of the cities and towns; not decided for the countryside | The list is in the Parameter Set. A person of a city or a town gets a vocation (section 7). The rules of a place are decided in part and not specified (section 8) |
| Tools | none: a list in the Parameter Set | Specified (section 8.1): the tools of each vocation and their emblem weights. Nothing draws a tool yet |
| Looks | not decided | Not specified (section 9) |

### 1.1 What holds for every step

These rules come from the readme and are decided.

- **Stubs are derived data.** Every gameplay-relevant fact of a city, a faction, a household
  or a person is a pure function of the foundation. The rules of the map hold here too:
  every random choice comes from a hash of (foundation seed, entity identity, purpose),
  and no result depends on the order the regions are generated in.
- **Presentation is authored data.** Descriptions, histories and portraits come from an
  LLM or a diffusion model. They cost to produce, so they are stored as the source of
  truth and are never regenerated or evicted. Authored data must not contradict the Stub.
- **Identity is deterministic.** A city, a faction, a household or a person is its parent
  plus a slot index. So a Stub is addressable before it is Realised, and player state and
  authored data can point at it. The parent of a faction is its region (section 4.2), and
  so is the parent of a city or a town (section 5.2) and of a household (section 6). The
  parent of a person is its household (section 7).
- **Only a small and relevant set is precalculated.** The game supports a very large
  number of persons, cities and towns. Only a small and relevant set of them is generated
  at game start, and more are generated as play progresses. A region is the unit: the
  households and the persons of its cities and towns are made when the region is taken to
  the Housed and the Peopled stage, as Stubs. With the default density that is about 370
  persons a region.

### 1.2 Entity generation states

Cities, factions, households and persons each have their own generation state. It is
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
are needed to generate factions, households and persons.

The races are a list in the Parameter Set, not a fixed set in the code. The list is
expected to change late in phase 2, so a race is added or changed by editing the list
(see [adding-a-race.md](adding-a-race.md)). Each race has:

- An **id** and a **name**. Generated data points at a race by its id. The ids must be
  given and unique. A Parameter Set that breaks this is refused when the world is created.
- A **toggle**: the race is on or off. A race that is off does not live in the world.
- Whether it is **human**: a human people or not. The name of a region is drawn from other
  word lists when the lead race of its ruling faction is human (section 4.10). A race that
  does not say so is not human.
- A **name style**: which name model the names of the race are drawn from. There are
  three: Common (British given names and surnames), Exotic (elven) and Dark (dark elven).
  The surname of a house that rules a region comes from the model of its lead race
  (section 4.10), and so do the surname of a household (section 6) and the names of the
  persons of the race (section 7). A
  race that does not say so has the style Common.
- A **frequency for each of the 17 biomes**: how frequent the race is in that biome,
  relative to the other races. 0 means the race does not live there.
- A **height modifier**: a factor on the frequencies of the race that depends on the
  height of the land. It can cut a race off at a height or fade it in and out. A race
  without a height modifier lives at every height.
- A **leaning to each of the six types of faction**, from 0 to 3: 3 for the kind of group
  the race forms first, 0 for a kind it never forms. It weighs the race in the draws for
  the type and the lead race of a faction (section 4.4). A race without a leaning leans to
  every type alike, with 1.
- A list of the **races it appreciates** and a list of the **races it hates**: the ids of
  other races. To appreciate is to trade with a people, live well beside it or look up to
  it. To hate is to fear it, fight it or hold a grudge against it. The lists run one way:
  the Midlanders appreciate the Halflings whether or not the Halflings appreciate them. An
  id in a list must be another race of the Parameter Set, and no race stands in both lists
  of one race. A Parameter Set that breaks this is refused when the world is created. A
  race that is off can still be listed. A race without the lists appreciates and hates no
  race in particular. No rule reads the lists yet: they are meant for the factions of a
  region (section 4).
- A list of **favourite colours**, the dearest first: the ids of colours of the colour
  list (section 9.1). They are the colours of its dress, its banners and its goods. The
  colours of a faction are drawn mostly from the favourite colours of its lead race
  (section 4.9). An id that is not a colour of the Parameter Set is refused when the world
  is created. A race without the list has no favourite colour.
- Three lists of the **colours of its persons**: the ids of the skin colours, the eye
  colours and the hair colours a person of the race can have, the commonest first
  (section 9.2). A race that lists none of a kind has persons without that component: the
  Noonfolk list no hair colour, since they have no hair. No rule reads the lists yet.
- A **description** and a **lore**. The description says what a person of the race looks
  like: build, skin, hair, eyes, dress and the marks that tell the race apart. The lore
  says where the race comes from and what it tells about itself. A portrait is drawn from
  the description, so that a race that exists nowhere else looks the same every time. No
  rule reads the two texts. Those of the default races are the parts "Appearance" and
  "History and lore" of [races-basis.md](races-basis.md)
  (`src/Gameworld.Domain/DefaultTexts.Races.cs`): change them together. A race without
  them has the text `**missing description**` for each, which marks a text that is not
  written yet. The vocations, the tools, the flora, the fauna and the resources carry the
  same two texts (section 8, section 8.1 and
  [map-generation.md](map-generation.md) sections 13.4 and 14).

In a Parameter Set file a race lists only the biomes it lives in. A biome that is left out
has the frequency 0, and a frequency of 0 is not written to the file. A race without a
height modifier does not carry one in the file.

| Parameter | Default |
|---|---|
| Races | 28, all on (Appendix A) |
| Human | The first seven races (Appendix A) |
| Name styles | Common for 14 races, Exotic for 8, Dark for 6 (Appendix A) |
| Height modifiers | Elves, Dwarves, Hearthbloods, Widegaze and Shoalfolk (Appendix A) |
| Leanings to the types of faction | One row per race (Appendix A) |
| Appreciated and hated races | Two lists per race (Appendix A) |
| Favourite colours | Three or four per race (Appendix A) |

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

- Whether a family can be mixed. A family has one race today, and only its servants can
  be of another (section 6).
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
feed, for each race of the region. It is not a count of people that exist: cities and
towns come in the Settled stage, and their households and persons in the Housed and
Peopled stages.

A region does not hold every race that is on. It holds its **main races**, the largest by
the plain shares of section 3.2, and its **minority races**, which get their place by turn
(section 3.3). Only the races of the region share its land (section 3.4).

| Parameter | Default |
|---|---|
| Persons per km² at a food supply of 1 and a middling Prosperity | 0.3 |
| Prosperity effect | 0.5 |
| Races per region | 6 |
| Minority races per region | 2 |
| Persons a race needs in a region to take a minority place | 0.5 |
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
  turn, not by its size. It only needs the land of the region to feed half a person of it
  by the plain shares, so it takes no place where it has next to no land.
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

With the defaults the median region has a potential population of about 1,820 and the
range is 35 to 5,940 (measured on 1,079 regions of four seeds at the earlier density of 6
persons per km², and divided by 20). Every one of them holds
six races. The largest race of a region holds 27 % of it in the median region and 19 to
43 % over all regions, so no race has a majority anywhere. A minority race holds 4.9 % of
its region in the median case, and about 100 persons. 0.9 % of the capacity lies on patches
that hold nobody.

The density was 0.06 persons per km² until 2026-11-21 and is five times that since. The
parameters that are counted in persons were raised five times with it: the persons a race
needs for a minority place, the persons an opposition needs and the persons per lesser
faction (section 4), the two ranges of the indices people and small (section 4.3) and the
two limits of the name of a region (section 4.10). So the races, the factions and the
names of a region are what they were, and every region holds five times the persons.
Measured on 676 regions, each against itself at the earlier density: the persons of every
race of every region are five times what they were before they are rounded, so the
potential population of each of the 674 regions with persons is exactly five times the
earlier one. In whole persons the regions hold 1,264,947 for 252,980, and a region
differs from five times its earlier whole persons by 10 at most, because the earlier
count rounded each race first. Every region has the same races and the same factions, the
cities and towns the same names, and one region another name: its rounded persons crossed
a limit.

Of all persons 83.5 % are of the seven human races, 10.4 % of the five classic races and
6.1 % of the sixteen new races. By the plain shares it would be 58.7 %, 20.2 % and 21.1 %:
the main races are human in most regions. The largest race is the Canal folk with 22 %.
They are a race of 90 % of the regions, the Elves of 10 %, the Dwarves of 9 % and each
new race of 6 to 10 %. The Dwarves were made a tenth more numerous after this measurement
(`common` 2.3 for 2, races-basis.md "Measured shares"); the other numbers moved by less
than their last digit and were not measured again.

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
| Persons an opposition needs | 400 |
| Persons per lesser faction | 750 |
| Most lesser factions | 4 |
| Lowest fit of a lesser faction | 0.75 |
| Lesser jitter | 0.1 |
| Favourite colour weight | 4 |
| Other colour weight | 0.05 |
| Taken colour factor | 0 |
| Colour contrast | 0.25 |
| Cleavage and favour | the tables of section 4.6 |
| Index ranges | the table of section 4.3 |
| Leaning of each race to each type | Appendix A |
| Favourite colours of each race | Appendix A |
| Colours | the list of section 9.1 |

The parameters are `Planning.Factions` in the Parameter Set. The leaning is
`FactionLeaning` on each race, and its favourite colours are `FavouriteColours`. The
colours are `Colours` at the top of the Parameter Set. The draws use the hash purposes
0x4b01 to 0x4b06.

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

**The texts.** Four lists of the Parameter Set hold a `Kind`, a `Name`, a `Description`
and a `Lore` for each value of a fixed set: `FactionTypes` (the six types above),
`FactionForms` (the forms of section 4.2), `SettlementKinds` (the city and the town,
section 5.2) and `SettlementTiers` (countryside, town, city and capital, section 8). The
lists hold texts only: no rule reads them, and the rules alone decide the type and the
form of a faction and what a place is. They are what a picture or a text of a faction or
a place is made from. The default description of a type is its line "Bound by" above, and
that of a form its one line in `src/Gameworld.Domain/Factions.cs` ("A ruling house" for
the crown). The descriptions of the kinds of settlement and of the tiers are written in
the defaults: what the place looks like and what life in the tier is like. The lore of
every value is written there too. A value that a Parameter Set file gives no text has
`**missing description**`, the text that marks one that is not written yet. The defaults are in `src/Gameworld.Domain/KindTexts.cs`.

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
| Main colour | The id of a colour (section 4.9): the field of its banner and the cloth of its livery |
| Secondary colour | The id of another colour (section 4.9): the device on the banner and the trim of the livery |

The seat of the ruling faction is the capital of the region. The capital is a city,
whatever its Prospect is for (section 5.2).

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
| people | ramp of persons | 800 | 3,000 |
| small | 1 − ramp of persons | 300 | 1,300 |
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
- A faction has the households of its leaders (section 6). Who else is a member of a
  faction is open.

Material: [research/factions.md](research/factions.md) holds 48 faction archetypes, the
axes that tell them apart and their relations.
[research/faction-generation.md](research/faction-generation.md) holds how generators and
games compute the kind of ruler of a land, and what decides a ruler, an opposition and
guilds in the scholarship. Every entry of [races-basis.md](races-basis.md) lists the kinds
of group the race forms.

### 4.9 The colours

Every faction has a **main colour** and a **secondary colour** from the colour list
(section 9.1). The main colour is the field of its banner and the cloth of its livery, and
the secondary colour is the device on the banner and the trim. They are facts of the Stub
and material for the looks (section 9): no other rule reads them.

The colours come mostly from the favourite colours of the lead race, so the factions of
the Northmen run to red, blue and silver. The factions of a region are told apart by their
main colour, and the secondary colour stands out against the main one. With the default
taken colour factor of 0, no two factions of a region share a main colour while there are
colours left, so a ruler and its opposition never fly the same banner. Two factions of one
race can still fly each other's colours the other way round: red on gold and gold on red.

```
weight(c) = favourite colour weight / k    when c is the k-th favourite colour of the lead race, k from 1
          = other colour weight            for every other colour
lightness(c) = (0.299 · R + 0.587 · G + 0.114 · B) / 255

for the factions of the region in the order of their slots:
    main      = one draw by weight over all colours, with
                weight(c) · taken colour factor   when a faction in a lower slot has c as its main colour
    secondary = one draw by weight(c) over the colours other than main
                with |lightness(c) − lightness(main)| ≥ colour contrast
```

- When no colour stands out that much against the main colour, the secondary colour is
  drawn from all colours other than the main one.
- When no colour that is allowed has a weight above 0, the colours that are allowed weigh
  alike.
- The colours are walked in the order of the Parameter Set, and a draw falls on the first
  colour where the running sum of the weights passes `u` times the total.
- The draws use one sfc32 generator per region, seeded with `hash4(seed, 0x4b06, i, j)`. It
  gives two numbers per faction in the order of the slots: one for the main colour, then
  one for the secondary colour.
- A lead race without favourite colours draws from every colour alike.
- With no colours in the Parameter Set a faction has no colours, and with one colour it
  has a main colour only.

The lightness rule is the old rule of the heralds in a plain form: a light device on a
dark field or a dark one on a light field, never red on blue. With the default list and a
lead race with three favourite colours, the main colour of the first faction of that race
in a region is a favourite colour about seven times in eight: 4 + 2 + 1.33 against
21 · 0.05. A race that leads more factions in a region than it has favourite colours gives
the later ones another colour.

Open:

- A tint from the type of the faction: white and gold for a faith, purple for the mages.
- A charge on the banner. [research/heraldry-coat-of-arms-generation.md](research/heraldry-coat-of-arms-generation.md)
  holds the material.
- Whether the factions of neighbouring regions should be told apart too. That would read
  another region, which no rule of section 4 does.

### 4.10 The name of the region

Every region with a ruling faction has a **name**: "Western Watson Kingdom", "New Sumerian
State", "Icag". It is the name of the realm of the ruling faction. The name is not
authored. It comes from the region-name generator, as the name of a person comes from the
name generator (section 7), so a region has its name as soon as it is Planned. A region
without persons has no ruling faction and no name.

The generator is a port of the region-name generator of an earlier project, in its
corrected form. [RegionNameGenerator-Reuse.md](RegionNameGenerator-Reuse.md) describes the
original, its defects and the corrections. The code is `src/Gameworld.Generation/Names`:
`RegionNameGenerator` holds the word lists and the rules, and `RegionNameRules` what a
region gives it.

| Parameter | Default |
|---|---|
| Small region | Fewer than 1,250 persons |
| Large region | More than 2,500 persons |
| Human | A flag on each race: the seven human races (Appendix A) |
| Name style | One per race (Appendix A) |

The two limits are `Planning.Names` in the Parameter Set. The flag is `Human` on each
race and the name style is `NameStyle`. The draws use the hash purposes 0x4b07 and 0x4b08.

**What the generator is given.** Four facts of the region, and nothing from another region:

| Input | Where it comes from |
|---|---|
| Human or not | The lead race of the ruling faction: whether it is human (section 2) |
| Size | The potential population of the region (section 3.4): small below the small limit, large above the large limit, middling between |
| Ruler | A ruling faction of the type Family is a house. A ruling faction of any other type is not |
| Ruling house | The surname of the house, when a house rules |

**The ruling house.** The surname comes from the name generator (section 7): from the
surname model of the name style of the lead race (section 2). So a house of the Midlanders
has a common surname, a house of the Elves an exotic one and a house of the Orcs a dark
one. The draws use one sfc32 generator per region, seeded with `hash4(seed, 0x4b08, i, j)`. A
surname of more than 12 characters is drawn again from the same generator, and the tenth
surname is kept whatever its length.

**The name.** A name is prefix + middle + suffix. The prefix and the suffix can be empty.
Every word has a weight, and a word is drawn by weight from the lists that hold for the
region. The lists with their weights are in `RegionNameGenerator`.

| Part | List | Words |
|---|---|---|
| Prefix | every region | North, East, South, West, Northern, Eastern, Southern, Western |
| | human | none, New, Neo-, United, Free |
| | not human | none, Old |
| | not human, with a suffix | none, Second, Third, Fourth, Fifth, Sixth |
| | human and large, with a suffix | none, Gran, Grand, Great |
| Middle | every region | none and 305 words: Abbasid, Khmer, Sokoto, Zimshath |
| | human | 53 words: Roman, British, Ottoman, Zulu |
| | not human | 29 words: Eternia, Florania, Ashanti, Xopfand |
| Suffix | every region | Kingdom, Dynasty, Empire, Region, Nation |
| | human | none, Republic, Alliance, State, Union, Territory |
| | human and small | Duchy, Province, Len, County |
| | not human | none, Ground, Sect, Land, land, Khaganate, Sultanate |

The rules, in the order they are applied:

1. The lists of the region are put together: the lists of every region, the lists of the
   human races or of the others, and the small suffixes for a small human region.
2. The suffix follows the ruler. Under a house there is no Republic, Alliance or Union.
   Without a house there is no Kingdom, Dynasty, Empire, Duchy, County, Len, Khaganate or
   Sultanate.
3. The suffix is drawn, then the middle.
4. A name with a suffix gets more prefixes to draw from: the ordinals for a region that is
   not human, and Gran, Grand and Great for a large human region. Then the prefix is drawn.
5. A prefix that starts with the same two letters as the suffix is dropped: no "United …
   Union" and no "Second … Sect".
6. In a name with a suffix, the surname of the ruling house takes the place of the middle
   in three cases of six.
7. The middle can be empty only between a prefix and a suffix: "North Kingdom". Otherwise
   it is drawn again without the empty entry.
8. The parts are joined, a double space becomes one and the ends are trimmed.

- The draws use one sfc32 generator per region, seeded with `hash4(seed, 0x4b07, i, j)`.
  It gives its numbers in this order: the suffix, the middle, the prefix, the ruling house
  (only in a name with a suffix, where a house rules) and the second middle (only when the
  first one may not be empty).
- A draw takes `x = floor(u · total)`, the total being the sum of the weights. The lists
  are walked in the order of rule 1, and the draw falls on the first word where the
  running sum of the weights passes `x`.
- Under a house a human region of middling size has a suffix in 21 cases of 36, so the
  surname of its house is in 29 % of the names.

**What differs from the original.**

- It is the Corrected mode of the reuse doc: the draws are independent, every word has
  exactly its weight, a large region gets the large prefixes and the middle is seldom
  empty.
- The numbers come from sfc32 and the hash of the region, not from the seeded random
  generator of .NET. So a region does not get the name the earlier project gave a seed.
- Rule 2 is new. The original knows nothing about the ruler.
- The surname is used only where a house rules. In the original every region has a ruling
  family.
- The limits are 1,250 and 2,500 persons for 10,000 and 100,000: a region of this world holds
  far fewer persons.
- Ten misspelled words are corrected: Southen, Greath, Bazil, Brazillian, Ethioian, Italin,
  Portugese, Selecuid and Xiognu, and Gran, which had no space after it. Portuguese,
  Seleucid and Xiongnu now stand twice in the list, as Seljuk did, and are twice as likely.

A test holds the reference names of the reuse doc: the lists and the rules give the names
of the Corrected mode for the same seeds, apart from the corrected words.

**Measured.** The generator on 324 regions (the 9 × 9 regions around (0, 0) of the seeds
`cellgen`, `gameworld`, `atlas` and `north`), with the default Parameter Set. 323 of them
have a ruling faction, and so a name.

- The lead race of the ruling faction is human in 95 % of the regions.
- 27 % of the regions are small and 22 % are large.
- A house rules 42 % of the regions, and its surname is in the name of 12 %. Two of the
  houses are of a race that is not human, both Orcish with a dark surname. Every other
  surname is a common one.
- 321 of the 323 names are different. The longest has 28 characters.

Open:

- The word lists are those of the earlier project. The middles hold real peoples, dynasties
  and countries (Roman, British, Brazil, Israel) beside invented words and words that are
  no name alone (Later, Central, Golden). Whether real names belong in this world is not
  decided.
- A race is human or not to the generator. Nothing in a name tells an Elven realm from an
  Orcish one, apart from the surname of its house.
- The suffix follows the house only. The other forms of a ruling faction (section 4.2) have
  no words of their own: a Temple state can be a Nation or a State, and a Merchant republic
  is not always a Republic.
- Two regions can get the same name. Telling them apart would read another region, which
  no rule of section 4 does.
- The surname of the ruling house is not a fact of the Stub of the faction. The households
  of the house carry it (section 6).
- The name styles of the default races are a first assignment (Appendix A), and there are
  only three models: the seven human races share one.
- The word lists as lists in the Parameter Set.

---

## 5. Cities and towns

Stage: Settled. The races of a city or a town are worked out in the Planned stage
(section 5.1), and so is its name (section 5.3). Which Prospects get a city or a town, and
how many persons live in each, is section 5.2.

A city or a town is an actual settlement on a Prospect. The Prospects are the map's
answer to where a settlement can lie, and how good the place is
(map-generation.md section 15). A region has 20 Prospects by default, of which up to 4 are
for cities. A city or a town stands on every Prospect on show, and on no other.

Open:

- What a city or a town holds beside its name, its persons and their races. Its
  households are those of section 6. Its vocations as a place are open (section 8).
- Whether a Prospect that is not on show gets a city or a town later, as play progresses.
  Its slot is kept free for it (section 5.2), and it has its name already (section 5.3).

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

How many persons a city or a town holds, and how many of each race, follows in section
5.2.

### 5.2 The cities and towns of a region

Stage: Settled. The rule needs the region itself only: its Prospects on show with their
races (section 5.1) and their names (section 5.3), its potential population (section 3.4)
and the seat of its ruling faction (section 4.4). Nothing is read from another region, and the rule holds no random
choice. It was adopted on 2026-11-20 as planned in
[plans/cities-and-towns.md](plans/cities-and-towns.md), which holds what was measured and
what was weighed.

| Parameter | Default |
|---|---|
| City share | 0.1 |
| Town share | 0.1 |
| Rank exponent | 1 |

The parameters are `Settlement` in the Parameter Set.

In the Settled stage the potential population of a region becomes its persons. A share of
them lives in its cities, a share in its towns and the rest on the land.

**Which Prospects get a city or a town.**

```
capital = the Prospect on show that is the seat of the ruling faction (section 4.4)
city    = the capital, and every other Prospect on show that is for a city
town    = every other Prospect on show
```

- A Prospect that is not on show gets no city and no town.
- The capital is a city whatever its Prospect is for. The best place of a region is a
  Prospect for a town when it lies one patch from the border patches
  (map-generation.md section 15.5), and such a Prospect is the capital in about one region
  in five. Its races are still those of its Prospect (section 5.1), so such a capital
  holds up to four races.
- A region without persons has no ruling faction and so no capital. A region without a
  Prospect on show has no city and no town, and all its persons live on the land.

**The rank.** The cities and towns of a region are ranked together:

```
order     = the capital, the other cities, then the towns
            # the other cities and the towns each in the order of the Prospects (map-generation.md section 15.5): the best score first
rank(s)   = 1, 2, 3 … in that order
weight(s) = rank(s)^(−rank exponent)
```

- With the default exponent of 1 the weight is exactly `1 / rank`: the rank-size rule. An
  exponent of 0 gives every weight 1, and a negative one counts as 0. Any other exponent
  goes through the deterministic exponential and logarithm: `exp(−exponent · ln(rank))`.
- The towns continue the ranks of the cities. In a region with three cities the first town
  has rank 4, so the towns of a region differ less in size than its cities do.

**The persons of the cities and of the towns.**

```
P = Σ population(r)                          # section 3.4, each race rounded

pool(share, n) = 0                           if n = 0 or share = 0
                 max(round(share · P), n)    otherwise; round is half up

cityPersons = pool(city share, the number of cities)
townPersons = pool(town share, the number of towns)
if cityPersons + townPersons > P:   cityPersons = min(cityPersons, P),  townPersons = P − cityPersons
```

- Each share is clamped to 0–1. If the two sum to more than 1, both are divided by the sum.
- A region without a city keeps the city share on the land, and a region without a town
  the town share.
- The pool is never smaller than the number of places that share it, so that no city or
  town stands empty. This lifts a pool only in a region with fewer than ten persons per
  place at the default shares, which the default density gives next to never.
- A share of 0 leaves the cities, or the towns, standing with nobody in them.

The cities share `cityPersons` by their weights, and the towns `townPersons` by theirs:

```
quota(s)   = pool · weight(s) / Σ weight          over the cities, or over the towns
persons(s) = ⌊quota(s)⌋
the persons left in the pool go one each to the largest remainders quota(s) − ⌊quota(s)⌋; ties to the lower rank
then, by rank: a place with nobody takes one person from the place of its kind with the most persons
               (ties to the lower rank), as long as that place holds two or more
```

- A region with fewer persons than places fills the lowest ranks first, one person each,
  and the last places stand empty.

**The races of a city or a town.** Its persons are shared between the races of its
Prospect by the weights of section 5.1. A race is never placed beyond the persons it has
in the region.

```
left(r) = population(r)                           # section 3.4, rounded; before the first city

for each city and town s, by rank:
    list     = the races of its Prospect (section 5.1), in their order
    w(r)     = weight(r) of section 5.1           # local(r) + hinterland share · population(r)
    quota(r) = persons(s) · w(r) / Σ w            over the races of the list
    n(r)     = min(⌊quota(r)⌋, left(r))
    while persons(s) > Σ n:
        one more person to the race of the list with left(r) > n(r) and the largest quota(r) − n(r); ties to the first in the list
        if the list has no such race: the rest goes to the other races of the region, the one with the
        largest left(r) first, ties in the order of the Parameter Set. They join the list at its end
    left(r) = left(r) − n(r)

rural(r) = left(r) after the last city or town
```

- Without a cap this is the largest remainder again. In a small place a race of the list
  can hold nobody: the list says which races the place is open to, the persons say who
  lives there now.
- A race from outside the list only enters when the shares are set far above the defaults,
  so that the cities and towns ask for more of a race than the region has.
- Every person of the region is counted once: for every race,
  `population(r) = the persons of r in the cities and towns + rural(r)`.

**The Stub** of a city or a town holds:

| Fact | Meaning |
|---|---|
| Slot | Its identity is the region and the slot. The slot is the index of its Prospect among the Prospects of the region (map-generation.md section 15.5), so the slot of a Prospect that is not on show stays free |
| Name | The name of its Prospect (section 5.3). No other city or town of the region has it |
| Kind | City or town |
| Capital | Whether it is the capital of the region |
| Patch | The patch of its Prospect |
| Rank | Its rank in the region, from 1 |
| Persons | How many persons live in it |
| Races | The persons of each race, in the order of the races of its Prospect |

The result of a region is its cities and towns by rank, and `rural(r)` for every race that
is on, in the order of the Parameter Set.

**The land.** `rural(r)` is the persons of race `r` that live on the land of the region.
A rural person belongs to a patch (section 8). The share of a patch is that of section
3.4: `rural(r) · the persons of r on the patch / population(r)`. This is not rounded.
Whole persons per patch are open, with the vocations of the countryside.

With the defaults 10.0 % of all persons live in cities, 9.9 % in towns and 80.1 % on the
land (measured on 676 regions of four seeds with 1,264,947 persons). A region has 3.0 cities
and 9.8 towns on average. Every region with persons has a capital, and 1 in 28 has no
town. No city or town stands empty, and no pool is lifted. The share of one region lies
within a person of a tenth: 10.0 % in cities for nine regions in ten.

| Persons | Lowest | 5 % | Median | 95 % | Highest |
|---|---|---|---|---|---|
| The capital | 4 | 39 | 100 | 169 | 285 |
| Another city | 10 | 18 | 39 | 75 | 143 |
| A town | 3 | 8 | 17 | 38 | 100 |

The capital holds 5.4 % of its region in the median region, and the whole city share
where it is the only city. In 61 % of the regions the largest town is larger than the
smallest city: with equal shares ten towns hold as many persons as three cities. A
minority race has a person in a city or a town of its region in 94 % of the cases, and
3.5 % of the listed races of all places hold nobody.

With a city share of 0.05 and a town share of 0.2 the same regions hold 5.0 % of all
persons in cities and 19.9 % in towns.

### 5.3 The name of a city or a town

Stage: Planned. The name is worked out for every Prospect of the region, on show or not,
as the races of section 5.1 are. So a city or a town takes its name from its Prospect, and
a Prospect that gets a city or a town later has its name already. The rule needs the region
itself only: its Prospects, the estimated biome and resources of their patches, the races
of each Prospect and the factions of the region. It was adopted on 2026-11-20.
[research/city-and-town-naming.md](research/city-and-town-naming.md) holds the sources and
the rule it proposed, and [city-and-town-names.md](city-and-town-names.md) what the words
of each race are built from.

Every Prospect has a **name**: "Jarlhavn", "Khar Nuur", "Qasr Mehret", "Temple Abbey". The
name is not authored. It comes from the city and town name generator, as the name of a
region comes from the region-name generator (section 4.10). The code is
`src/Gameworld.Generation/Names`: `SettlementNameGenerator` draws and joins a name,
`SettlementNameLexicons` holds the words of each race, and `SettlementNameRules` what a
Prospect gives the generator.

A name is a **first part** and a **place word**: Jarl + havn, Khar + Nuur, and Qasr +
Mehret with the place word in front. The place word says what kind of place it is, and the
first part tells it from the other places of its kind. Two things decide the name:

- The **race** that names the place gives the words: its first parts and its place words.
- The **site** of the Prospect gives the class of the place word: a harbour, a ford, a
  hill, a city or a town.

The rule has no parameters in the Parameter Set. Its numbers are constants of the code:

| Constant | Value |
|---|---|
| Navigable river | 8 patches drained and up (map-generation.md section 15) |
| River to ford | 2 patches drained and up, below navigable |
| Meeting of rivers | The smaller branch drains 4 patches or more |
| Market | A trade factor of 0.9 or more |
| Length of a name | 4 to 24 characters |
| Draws for one Prospect | 20 at most |
| Longest surname | 8 characters, in 10 draws at most |

**The race that names the place.**

```
capital:          the lead race of the ruling faction (section 4.4)
any other place:  one of the races of its Prospect (section 5.1), drawn with weight(r) of section 5.1
```

- The capital is the Prospect that is the seat of the ruling faction.
- The races are walked in the order of the list of the Prospect, and the draw falls on the
  first race where the running sum of the weights passes it. Where no race has a weight
  the first race of the list names the place.
- So the largest race around a Prospect names most of its places, and a small race names a
  few: the names of a region come in layers, as the names of a land with several peoples
  do.
- A Prospect without a race is named from the lexicon of the Common name style (see "A
  race without a lexicon").

**The site classes.** A Prospect holds every class whose condition is true. Its kind adds
one more: Fort for a city, Farm for a town. The classes read the facts of the Prospect, the
estimated biome and resources of its patch (as the Prospect itself does, map-generation.md
section 15), the elevation type of its zone and the factions that sit on it.

| Class | Condition | Weight |
|---|---|---|
| Mouth | The largest river mouth on the patch is navigable | 6 |
| Haven | The patch has a harbour | 5 |
| Shore | The patch is on the coast and has no harbour | 3 |
| Landing | The patch holds the head of navigation | 4 |
| Meeting | A meeting of rivers | 4 |
| Ford | The largest river on the patch is one to ford | 3 |
| Bridge | The largest river on the patch is navigable | 3 |
| Mere | The patch has a lake | 4 |
| Hill | The zone is Hills or Mountains | 3 |
| Wood | The biome is Boreal forest, Woodland & shrubland, one of the three temperate forests, one of the two tropical forests or Mangrove | 2 |
| Field | The biome is Cold steppe, Temperate grassland or Savanna | 2 |
| Waste | The biome is Ice & snow, Tundra, Cold desert, Hot desert or Thorn scrub | 2 |
| Salt | Salt is listed on the patch: an amount of 0.25 or more | 4 |
| Delve | The patch has a listed deposit of iron ore, copper ore, tin, silver and lead, gold, or gems and pearls | 4 |
| Shrine | A faction of the type Belief sits on the Prospect | 5 |
| Market | A faction of the type Vocation or Place sits on the Prospect, or its trade factor makes a market | 4 |
| Fort | The place is a city: the capital, or a Prospect for a city | 4 |
| Farm | The place is a town | 4 |

- The rarer and the more telling a fact is, the more it weighs. A town with a harbour is
  named for the harbour in 5 cases of 9, where its race has words for both.
- Volcano gives none of Wood, Field and Waste.

**The lexicon of a race.** A lexicon is one or more **tongues**: the Dwarves have the
hold's own name and the gate name, the Orcs orcish, the names of the legion and plain
names. A tongue holds:

| Part | Meaning |
|---|---|
| Weight | How often the tongue is drawn among the tongues of its lexicon |
| Listed first parts | Words that are written as they stand: "Hrafn", "Temple", "Khar" |
| Syllables | How a first part is coined: one first syllable, some middle ones (a least and a most) and one last one where the tongue has last syllables. A mirrored tongue writes the syllables, a letter in the middle and the syllables backwards |
| Coined share | How often the first part is coined, where the tongue has both listed first parts and syllables |
| Bare share | How often a coined first part is the whole name, without a place word |
| Worn | Whether the tongue wears its seams down (see the join) |
| Place words | A list of words for each site class the tongue has words for |

A tongue need not have words for every class. A class it has no word for is never drawn
for its names, so a race names a place for what it cares about: the Shoalfolk have words
for the coast and none for a hill. Every tongue with place words has them for Fort and
Farm, so every site has one.

The form of a place word says where it stands:

| Form | Where | Example |
|---|---|---|
| It ends in a space or a hyphen | In front of the first part | "Bir " + Sallah = Bir Sallah, "Ile-" + Kingora = Ile-Kingora |
| It starts with a space or a hyphen | After the first part, as it stands | Khar + " Nuur" = Khar Nuur, Bobbin + "-under-Oak" |
| Any other | Joined to the first part | Jarl + "havn" = Jarlhavn |

The default races, with the first parts of their tongues and names the generator gives:

| Race | Its tongues | Names |
|---|---|---|
| Northmen | One, worn: 59 listed first parts (animals, founders, trades) and joined place words | Jarlhavn, Skjaldby, Stormaros |
| Midlanders | One: 99 listed first parts (the land, its powers and its founders), joined place words and a few of their own (Abbey, Cross, Pont, Castel, Chipping) | Priorshythe, Temple Abbey, Pont Aure |
| Saltroad folk | One, worn: the place word stands in front. Three first parts in four are coined, the others are among 33 listed. 35 % of the coined ones stand alone | Qasr Mehret, Bir Sallah, Ifazelt |
| Sunlanders | One, worn: every first part is coined, a first syllable and one or two more. 40 % stand alone | Mandasakro, Ile-Kingora, Kibidu |
| Grass riders | One: 56 listed first parts (colours, animals, numbers) and a place word as a word of its own | Khar Nuur, Doloon Khot |
| Rainkeepers | One, worn: 57 listed first parts and joined place words | Padmagarh, Shvetasangam |
| Canal folk | One: every first part is coined, of two syllables, and the kind of place is joined to it. 30 % stand alone | Jiangmingcheng, Mingkaoqiao, Tianhai |
| Elves | One, worn: every first part is coined, a root and a linking syllable | Melrenne, Valovel |
| Dwarves | The hold's own name, coined and worn (weight 5). The gate name in the trade tongue, 46 listed first parts (weight 4) | Tholargrund, Morr Dun, Pledgeplate |
| Halflings | One: 58 listed first parts | Hivesbrook, Pantry Hundred |
| Gnomes | One: 59 listed first parts | Fennickroot, Bezel Landing |
| Orcs | Orcish, coined and worn, half of the names bare (weight 5). The names the legion left, 31 listed first parts (weight 3). Plain names of a hard land, 34 listed (weight 3) | Krurnak Wall, Burlgal, Old Garrison, Marrowfort |
| Hoarkin | Their own tongue, coined, worn and always bare (weight 6). The trade names, 35 listed first parts (weight 4) | Kaalholeesk, Seal Moot |
| Twofold | Their own tongue, mirrored and always bare: every name reads the same both ways (weight 6). The trade names, 34 listed first parts (weight 4) | Atidita, Pavisivap, Yoke Door |
| The other fourteen new races | One each, in the trade tongue: 45 to 48 listed first parts, and place words for the 4 to 7 site classes the race cares about | Emberhall, Tall Barrow, Sprucestand, Dryshore, Duskwall, Sunfed, Motherhall, Reefhold, Torchcrown, Marshal's Well, Vialhall, Mint Lodge, Curebank, Roofway |

The words of each race are built from what [city-and-town-names.md](city-and-town-names.md)
says its names are made of, and the lists themselves are in `SettlementNameLexicons`. The
50 names of a race in that document are not a list the generator draws from.

**A race without a lexicon.** A lexicon belongs to a race id of the default list. A race
that is added through a Parameter Set file has none, and takes the lexicon of its name
style (section 2): the first part is a surname from the surname model of the style, drawn
again while it is longer than 8 characters and the tenth kept whatever its length, and the
place words are those of the trade tongue (mouth, haven, ford, bridge, mere, don, ley,
field, wich, minster, bury, ton and the like). "Goodby", "Tyrport", "Fireton".

**One draw.** A name is drawn in this order:

1. The tongue, by weight among the tongues of the lexicon.
2. The site class, by weight among the classes of the Prospect that the tongue has words
   for, walked in the order of the table.
3. The place word, each word of the class as likely as the others.
4. Whether the first part is coined: always in a tongue without listed first parts,
   never in one that cannot coin, else in the coined share of the cases.
5. The first part: one of the listed ones, each as likely as the others, or a coined one.
   A coined one draws how many middle syllables it has, then its syllables in order.
6. Whether the name is bare: a coined first part is the whole name in the bare share of
   the cases, and so is any first part where the tongue has no place word for the site.

**The join.**

- A place word in front, or one that starts with a space or a hyphen, is written beside
  the first part as it stands.
- A first part that ends in an apostrophe, with or without an s, keeps a joined place word
  as a word of its own with a capital: Marshal's + field = Marshal's Field.
- Any other place word is joined. In a worn tongue a letter doubled across the seam is
  written once, and a vowel at the end of a first part of three letters or more falls
  before a vowel: Ulf + fell = Ulfell, Jala + uppalam = Jaluppalam. The syllables of a
  coined first part are joined the same way.
- A tongue that is not worn joins the two as they stand.

**A valid name.** A drawn name is valid when all of this holds:

- It has 4 to 24 characters, and only the letters A to Z, spaces, hyphens and apostrophes.
  A surname model can give other letters.
- The first part is not one syllable said twice: no "Yuyu".
- The first part does not end in the place word: no "Reefreef" and no "Hollow Hollow".
- In a tongue that is not worn, the first part and a joined place word do not meet in the
  same letter: no "Rafttown".

**The names of a region.** The Prospects are taken in their order (map-generation.md
section 15.5), which is the order of the slots:

```
for each Prospect, in order:
    rng  = sfc32 seeded with hash4(seed, 0x4c01, I, J)        # (I, J) the root cell of the Prospect's patch
    race = the race that names the place, with u = rng.next()
    up to 20 times:
        name = one draw from rng, with the lexicon of the race and the site classes of the Prospect
        stop when the name is valid and no Prospect before this one in the order has it
    the last name drawn is the name of the Prospect
```

- No two Prospects of a region have the same name. Names are compared without regard to
  capitals.
- A name that is drawn again comes from the same generator, so the race stays and the
  site class, the place word and the first part are all drawn anew.
- The twentieth name is kept whatever it is. The lexicons give thousands of names each, so
  this has not happened in any region measured.
- The rule reads the region only, so a name does not depend on the order the regions are
  generated in. The purpose 0x4c01 is used by no other draw.

**What is stored.** `ProspectNames` of the Planned region holds the name of every
Prospect, in the order of the Prospects. The Stub of a city or a town holds the name of its
Prospect (section 5.2). The race that named the place and the site class of its place word
are not stored: they can be worked out again from the Planned region.

**What differs from the research note.** The note proposes a rule in its §6.3 and leaves
eight decisions open in its §7.

- The first part comes from word lists of each race, not from the surname model of its
  name style. The surname models are British, elven and dark elven, so the seven human
  races would have shared one sound. The surname model is kept for a race without a
  lexicon.
- The place words are those of each race, and a race has words only for the classes it
  cares about. The note has one list of English place words, and asks in D2 what the other
  styles should use.
- The race is drawn by weight among the races of the Prospect. The note recommends the
  largest race (D3). Measured with the largest race, the four largest human races named
  92 % of all places and the sixteen new races 0.3 %.
- The name is derived data (D1) and every Prospect is named (D4), as the note recommends.
  The stage is Planned, not Settled: the name reads Planned data only, and the Prospects
  layer of a map can show it.
- There is no step against the bordering regions (D5, step 7 of the note): see Measured
  and Open.
- The surname of the ruling house is not used for the capital (D6): the house has a
  British, an elven or a dark elven surname, which does not fit the words of most races.
- There is no Market, Little, Upper, Nether or Great in front of a name. A market is a
  site class with place words of its own, and a name that is taken is drawn again.
- The Market class needs a trade factor of 0.9, not 0.7: 43 % of the Prospects on show
  have 0.7 or more, and 19 % have 0.9 or more.
- The lists are in the code and the numbers are constants (D8).

**Measured.** The generator on 324 regions (the 9 × 9 regions around (0, 0) of the seeds
`cellgen`, `gameworld`, `atlas` and `north`), with the default Parameter Set: 6,366
Prospects, of which 4,156 are on show and so have a city or a town.

- No two Prospects of a region have the same name, in any region.
- The 6,366 Prospects have 5,929 different names. The 4,156 cities and towns have 3,960:
  3,772 names stand once, 180 twice and 8 three times.
- Three pairs of cities or towns in bordering regions have the same name. Six of the 324
  regions hold one of them. Counted over all Prospects it is nine pairs.
- Every one of the 28 races names a place. A human race names 87 % of the cities and
  towns, and the four that name the most, the Canal folk, the Midlanders, the Saltroad
  folk and the Sunlanders, name 75 %. The sixteen new races name 4 %.
- A name has 9.7 characters on average. The shortest has 4 and the longest 23 ("Heather
  Below the Ridge"). 18 % have a space or a hyphen.
- What the place word of a city or a town says:

| Place word | Share | Place word | Share |
|---|---|---|---|
| Farm | 27.2 % | Meeting | 1.9 % |
| No place word | 17.3 % | Haven | 1.9 % |
| Bridge | 11.8 % | Mouth | 1.5 % |
| Wood | 9.9 % | Shore | 1.5 % |
| Fort | 6.7 % | Hill | 1.3 % |
| Ford | 4.5 % | Mere | 1.3 % |
| Market | 3.9 % | Waste | 1.2 % |
| Landing | 3.0 % | Shrine | 0.5 % |
| Delve | 2.4 % | Salt | 0.1 % |
| Field | 2.0 % | | |

Open:

- Two cities or towns in bordering regions can have the same name, and two further apart
  more often. Telling them apart would read the Planned names of the bordering regions in
  the Settled stage (step 7 of the research note), which no rule of this section does. A
  display can add the name of the region.
- The lexicons as lists in the Parameter Set, with the word lists of the region names
  (section 4.10). Until then a race that is added through a Parameter Set file has no
  words of its own.
- A coined first part can be a word that means something, or something unfortunate: the
  lists are not filtered.
- Rivers, lakes, hills and forests have no names, so a town cannot take the name of its
  river.
- One name per place. A place that holds several races could have a name in the tongue of
  each.
- There is no New and no Old: the world has no history yet.
- The capital of a region ruled by a house does not carry the surname of the house.
- The weights of the site classes and the shares of the tongues are first values. They are
  not measured against a target.

---

## 6. Households

Stage: Housed, the stage after Settled. The rule needs the region itself only: its
factions (section 4), the surname of its ruling house (section 4.10) and its cities and
towns with their persons per race (section 5.2). Nothing is read from another region. It
was adopted on 2026-11-21 as planned in
[plans/households-and-persons.md](plans/households-and-persons.md), which holds what was
measured and what was weighed.

A household is a family and its servants, living together in one city or town. A
household shares its wealth and its faction. The family is the kin in the household. The
households of one faction of the type Family are a house: one family in several
households.

| Parameter | Default |
|---|---|
| Households per faction | 2 |
| Family size weights, for a family of 1 to 8 persons | 2, 3, 3, 3, 2, 1, 0.5, 0.25 |
| Wealth weights in a town: Destitute, Poor, Common, Comfortable, Rich | 1, 3, 4, 2, 0.3 |
| Wealth weights in a city | 1.5, 3, 3.5, 2.5, 0.8 |
| Wealth weights of a household of a faction | 0, 0, 1, 3, 2 |
| Servants by wealth | 0, 0, 0, 0 to 1, 1 to 3 |
| Head age | 20 to 65 |
| Male head chance | 0.8 |

The parameters are `Housing` in the Parameter Set.

The draws use the hash purposes 0x4d01 (a household of a faction), 0x4d02 (the other
households of a city or a town) and 0x4d03 (the surname of a household). A thing inside a
region has no grid coordinate of its own, so its draws are keyed in two steps:

```
key(purpose, a, b) = hash(hash(seed, purpose, region i, region j), purpose, a, b)
```

`hash` is the hash of map-generation.md section 2.2. A household of a faction takes its
numbers from the stream of `key(0x4d01, faction slot, round)`, and a surname from the
stream of `key(0x4d03, household slot, 0)`. The other households of a city or a town take
theirs from one stream, `hash(seed, 0x4d02, i, j)` of the root cell of the patch of its
Prospect, as its name does (section 5.3).

**The pool.** Every household takes its persons from the persons of its city or town:

```
left(s, r) = the persons of race r in the city or town s        # section 5.2
```

A household never takes more than is left, and the rule ends when nothing is left. So the
households of a city or a town hold exactly its persons, race by race.

**Step 1: the households of the factions.** First every household of a faction gets its
head, one person:

```
for pass in (seat, lead race), (other places, lead race), (seat, any race), (other places, any race):
    for round = 0 … households per faction − 1:
        for each faction, by slot, that has exactly `round` households:
            kept   = the race of its first household         if round > 0 and the faction is of the type Family
            wanted = kept, or else its lead race
            in a pass for any race: a faction of the type Race is skipped, and so is a faction with a kept race
            places = the city or town on its seat            in a seat pass
                     the other cities and towns, by rank     otherwise
            the first of the places with a fitting person left gives the head:
                race = wanted                                                    in a pass for the lead race
                       the race with the most persons left there, the first in the list of equals    in a pass for any race
                left(place, race) = left(place, race) − 1
```

- **The lead race comes before the seat.** The leaders of a faction are of its lead race
  (section 4.2) wherever a city or a town of the region holds a person of it. Only then
  does the seat decide. So a household of a faction can sit in another place than the
  seat.
- **Every faction gets its first head before any gets a second** in the same pass. Three
  factions on one seat of four persons get two, one and one household.
- **A faction of the type Race holds its race only**, so it has no household where no city
  or town holds its race.
- **A house keeps its race.** A later household of a house is of the race of the first.
- **A faction can have fewer households.** It has one, or none, where no fitting person
  is left. A region without a city or a town has no household at all.

Then the households of the factions are filled, by faction slot and round. Each takes
seven numbers from its stream: wealth, family, servants, sex, age, kin, kin age.

```
wealth = Rich                                    the first household of the ruling faction
         by the wealth weights of a faction      any other
family = 1 + min(size − 1, left(place, race))    # size by the family size weights; the head is taken already
servants, head sex, head age and surname: below
```

**A house.** A household of a faction of the type Family after the first is a branch of
the house:

```
surname = the surname of the first household
kin     = Child      if the head age of the first household − 18 ≥ the lowest head age, and the kin number < 0.5
                     head age = even in [the lowest head age, the head age of the first household − 18]
          Sibling    otherwise
                     head age = the head age of the first household + even in [−10, 10], within the head age range
```

- The first household of the house that rules the region takes the surname of the ruling
  house (section 4.10): the one that can stand in the name of the region. The first
  household of any other house draws a surname as below.
- The kin of a branch is between the two heads only: the head of the branch is a child or
  a sibling of the head of the first household.

**Steps 2 and 3: the other households.** City by city and then town by town, by rank
(section 5.2). Each household takes six numbers from the stream of its place: race,
wealth, family, servants, sex, age.

```
while a person is left in s:
    race   = by the weights left(s, r)
    family = min(size, left(s, race))            # size by the family size weights
    wealth = by the wealth weights of a city or of a town      # the capital is a city
```

**What every household draws the same way.**

```
servants = even in the range of its wealth, and no more than the place has left
           each servant is of the race with the most persons left in the place, the first in the list of equals
           a household of a faction of the type Race takes servants of its own race only
head sex = a man with the male head chance        in a family of two or more
           a man or a woman with even chance      a person who is alone in its family
head age = even in the head age range             # the range never starts below 16
surname  = from the surname model of the name style of the race of the family (section 7)
           one of more than 12 characters, or one that an earlier household of the region has, is drawn again
           from the same stream, 10 times at most: the last one is kept
```

**The Stub** of a household holds:

| Fact | Meaning |
|---|---|
| Slot | Its identity is the region and the slot. The households of the factions come first, by faction slot and round, then the others by the rank of their city or town, in the order they are drawn |
| Settlement | The slot of the city or town it lives in |
| Faction | The slot of its faction. None for a household of no faction |
| Race | The race of the family |
| Wealth | Destitute, Poor, Common, Comfortable or Rich |
| Surname | The surname of the family. A house shares it |
| Head sex, head age | The sex and the age of the head. They are in the Stub so that the kin of a house is consistent before the persons are made |
| Family | How many persons the family holds, the head included |
| Servants | How many servants it holds, per race |
| Kin | For a branch of a house: the slot of the first household, and Child or Sibling |

**Measured** on 676 regions: the 13 × 13 regions around (0, 0) of the seeds `cellgen`,
`atlas`, `ember` and `tide`, default Parameter Set. 674 of them have a city or a town.
Their 8,674 cities and towns hold 252,203 persons in 85,775 households.

| | Lowest | 5 % | 25 % | Median | 75 % | 95 % | Highest |
|---|---|---|---|---|---|---|---|
| Households of a region | 0 | 37 | 87 | 124 | 166 | 227 | 356 |
| Households of a capital | 2 | 12 | 21 | 28 | 36 | 46 | 75 |
| Households of another city | 3 | 7 | 10 | 13 | 17 | 23 | 42 |
| Households of a town | 1 | 4 | 5 | 7 | 9 | 13 | 35 |

| Fact | Value |
|---|---|
| Factions with two households, with one, with none | 99.8 %, 0.1 %, 0.1 % of 1,934: two factions with one and two with none, all four of the type Race |
| The first household of a faction is at its seat | 99.1 % |
| The first household of a faction is of its lead race | 99.8 % |
| The second household is at the seat, is of the lead race | 95.5 %, 99.5 % |
| Households of a faction | 4.5 % of the households, with 6.1 % of the persons |
| Houses with two households | all 393. The head of the branch is a child in 125 and a sibling in 268 |
| Two households of no faction with the same surname in one region | none |
| Wealth of a household of no faction | Destitute 11.3 %, Poor 28.0 %, Common 35.4 %, Comfortable 20.6 %, Rich 4.8 % |
| Wealth of a household of a faction | Common 14.5 %, Comfortable 40.5 %, Rich 45.0 % |
| Persons in a household | 2.94 on average. One person 22.2 %, two 24.6 %, three 20.2 %, four 15.8 %, five or more 17.2 % |
| Persons in a family | 2.72 on average, where the weights alone give 3.4 |

**A family is cut to what its race has left in the place.** A city or a town holds 29
persons on average, of 4.1 races. So the last family of a race in a place is often smaller
than drawn, and a race with one person in a place is a household of one person: 4.7 % of
all households. That is why the mean family is 2.7 persons and not 3.4.

**The density decides the size of the households.** Until 2026-11-21 the density was a
fifth of this (section 3.4). A city or a town then held 5.8 persons of 3.1 races, 58 % of
the households held one person and the mean household 1.7.

Open:

- The households of the land. Four persons in five live outside the cities and towns and
  have no household. The user named villages on 2026-11-21 as places of households that
  come later, and section 8 says there are no villages: a rural person belongs to a patch.
  One of the two must give way first.
- The person who is alone of its race in a place: whether it joins another household.
- What the wealth of a household reads. It reads the kind of its place and its faction
  only: not the Prosperity field, the wealth of the region (section 4.3) or the resources
  of its patch.
- Kin between the households of no faction, and marriages between houses.
- Slaves (section 8), lodgers and apprentices in a household.
- Who but its leaders belongs to a faction (section 4.8).
- The defaults are first values. Only the count of households per faction is measured
  against what was asked.

---

## 7. Persons

Stage: Peopled, the stage after Housed. The rule needs the region itself only: its
households (section 6), its factions (section 4) and, of each city or town, what kind of
place it is and whether its Prospect is on water. Nothing is read from another region. It
was adopted on 2026-11-21 with the households, as planned in
[plans/households-and-persons.md](plans/households-and-persons.md).

A person is an NPC in the game. Every person of a household is generated as a Stub, so
every person of a city or a town is an individual: about 360 persons in the median region.
The persons of the land are a count per race (section 5.2). The Stub of a person holds
name, sex, age, race, vocation, role, family links and its household. The household gives
the home, the wealth and the faction. The role is the position in the household for now.
The Stub also has a place for the looks of the person (section 9), which no rule fills
yet. The textual description, the history and the portrait are authored (section 1.2).

| Parameter | Default |
|---|---|
| Spouse chance | 0.75 |
| Working age | 14 |
| Land work factor | 0.1 |
| Same vocation chance | 0.6 |
| Servant vocation | `servant` |

The parameters are `People` in the Parameter Set. The rule has fixed numbers too:

| Fixed number | Value |
|---|---|
| A parent is older than its child by at least | 16 years |
| A mother is older than her child by at most | 45 years |
| A father of the head is older than the eldest of the head and its siblings by at most | 35 years |
| The age of a husband minus that of his wife | −3 to 10 years |
| A husband or a wife is at least | 16 |
| A sibling of the head is older or younger than the head by at most | 10 years, and is at least 14 |
| Nobody is older than | 90 |
| A servant is | 12 to 50 |
| The weights of a child, a parent and a sibling of the head | 6, 1, 1 |

The draws use the hash purposes 0x4e01 (the persons of a household: who they are, their
ages and their vocations) and 0x4e02 (the name of a person), keyed as in section 6:
`key(0x4e01, household slot, 0)` gives the stream of a household and
`key(0x4e02, household slot, person slot)` the stream of the name of one person. So a
household is drawn from a stream of its own, and no household depends on another.

**The head** has slot 0. Its sex and its age are those of the Stub of the household.

**The husband or the wife of the head.**

```
if family ≥ 2 and a number < spouse chance:
    gap    = even in [−3, 10]                     # the age of the husband minus that of the wife
    spouse = of the other sex than the head
             age = head age − gap  (the head is the husband)  or  head age + gap  (the head is the wife), within 16 to 90
```

**The rest of the family**, until the family is as large as the Stub says. Each person
takes five numbers: role, sex, age, same vocation, vocation.

```
mother, father   = the woman and the man of the head and its spouse
eldest, youngest = the highest and the lowest age of the head and its siblings so far

Child    weight 6    if low ≤ high:   low  = max(0, age of the mother − 45), or 0 without a mother
                                      high = the lower age of the head and its spouse − 16
                     sex even, age even in [low, high]. Its father and mother are the head and its spouse
Parent   weight 1    if the household is not a branch of a house, the head lacks a father or a mother in the
                     household, and eldest + 16 ≤ 90
                     sex = the one that is missing; even where both are
                     a mother: age even in [eldest + 16, min(90, youngest + 45)]
                     a father: age even in [eldest + 16, min(90, eldest + 35)]
Sibling  weight 1    always
                     sex even, age even in [low, high]:
                     low  = max(14, head age − 10, age of the mother of the head − 45)
                     high = min(head age + 10, age of each parent of the head − 16)
```

- The parents of the head are the parents of its siblings too, and husband and wife.
- A branch of a house has no parent of the head in it: that parent is the head of the
  first household of the house, or lives there.
- A person who is alone with a family of two or more has no spouse. A child of such a head
  has one parent in the household.
- The bounds hold in every household: a father and a mother are at least 16 years older
  than each of their children in the household, and a mother at most 45.

**The servants** follow the family, race by race as the Stub of the household lists them.
Each takes two numbers: sex even, age even in 12 to 50.

**The vocation.**

```
tier(place)      = Capital for the capital, City for another city, Town for a town
weight(v, place) = 0                                   if v is off, its lowest tier is above tier(place),
                                                       or v is on water and the place is not
                   share(v) · land work factor         if v is of the sector Land and water
                   share(v)                            otherwise
fits(v, wealth)  = the standing of v meets the standing of the wealth:
                   Destitute 1, Poor 2, Common 3, Comfortable 4, Rich 5 to 6
fitting(wealth)  = by weight(v, place) among the vocations that fit the wealth
                   where none has a weight: the nearest lower wealth that has one, then the nearest higher
                   none when no vocation has a weight in the place

the head of a household of a faction:
    one of the vocations the faction gathers (section 4.7), by share      if it gathers one that is on
    the vocation of those who lead a faction of its type, if it is on:
        Family noble · Race noble · Vocation merchant · Belief priest · Magic mage · Place official
    fitting(wealth of the household)                                      otherwise
any other head:                  fitting(wealth of the household)
another person of the family:    none below the working age
                                 the vocation of the head     with the same vocation chance
                                 fitting(wealth of the household)     otherwise
a servant:                       the servant vocation, if it is on
```

- A place is on water when its Prospect has a harbour, a river mouth of 8 patches or more,
  or a river of 8 patches or more: the same test as for a seat on water (section 4.7).
- The tier and the water do not bind the head of a household of a faction: the mages of a
  town that is the seat of a circle are mages.
- A vocation is the id of one of the list. No variant and no by-work is drawn (section 8).
- The work of the land has a small weight inside a city or a town: the farmers, the herders
  and the fishers are mostly the persons of the land.

**The name.** The name of a person is not authored. It comes from the name generator, so a
person has a name already as a Stub. The generator is a character-level Markov chain with
nine pregenerated models: a male, a female and a surname model for each of the three name
styles. The code is `src/Gameworld.Generation/Names`, and
[NameGenerator-Reuse.md](NameGenerator-Reuse.md) describes it. A race has a name style
(section 2), and the names of a person come from the models of the style of its race.

```
given name = from the male or the female model, from the stream key(0x4e02, household slot, person slot)
             one that a person of the household has already is drawn again, 5 times at most
surname    = the surname of the household                      a person of the family
             from the surname model, from the same stream      a servant
```

**The Stub** of a person holds:

| Fact | Meaning |
|---|---|
| Household | The slot of its household. Its identity is the household and the slot |
| Slot | The slot in the household: 0 the head, then the family in the order drawn, then the servants |
| Given name, surname | From the name generator |
| Race | The race of the family, or the race of the servant |
| Sex, age | |
| Role | Head, Spouse, Child, Parent, Sibling or Servant: the position in the household |
| Vocation | The id of its vocation. None for a child, and where no vocation has a weight |
| Spouse, father, mother | The slots of the husband or wife, the father and the mother, where they live in the household |

The home of a person is the city or town of its household, and its faction and its wealth
are those of its household.

**Measured** on the same 676 regions, 252,203 persons:

| Fact | Value |
|---|---|
| Persons in households, per region | median 356, 102 to 704 for nine regions in ten, at most 1,188 |
| Head, spouse, child, parent, sibling, servant | 34.0 %, 18.9 %, 29.8 %, 4.9 %, 5.0 %, 7.4 % |
| Heads of a family of two or more with a husband or a wife | 74.8 % |
| Men | 51.9 % |
| Under 14 | 17.7 %. The mean age is 34 |
| Without a vocation | 17.3 %: the children under the working age |
| The most held vocations, of the persons with one | Servant 18.7 %, Labourer 13.6 %, Beggar 7.0 %, Weaver 6.4 %, Farmer 6.1 %, Priest 5.2 %, Brewer 3.6 %, Soldier 3.0 %, Shopkeeper 2.9 %, Carter 2.3 %, Noble 2.0 % |

Every default vocation is held by somebody.

Open:

- The persons of the land, and which persons are important enough to be authored first.
- The role of a person in its vocation (apprentice, master) and in a faction, and the
  list of roles (section 8).
- The variant of a vocation, the by-work, the share by sex, the factors of the races and
  the Supernatural Field (section 8). A woman holds a vocation like a man today.
- The ages of the races. Every race ages as the human races do, and no race has a lifespan
  in the Parameter Set. [races-basis.md](races-basis.md) gives each race one: about 60
  years for the Northmen, 500 to 600 for the Elves.
- Family links between households. Only the heads of the households of a house are kin
  (section 6).
- More name models. The seven human races share the common one, which is British.
- The fixed numbers are first values.

---

## 8. Vocations

Stage: Peopled for the persons of the cities and towns (section 7). Not decided for the
countryside, and not specified as a rule of a place.

A vocation is the work a person does: farmer, smith, priest, guard and so on.

The rules below were decided on 2026-11-15. The list of vocations is in the Parameter Set
since 2026-11-19 (see "In the Parameter Set" below). Since 2026-11-21 a person of a city
or a town gets a vocation from the list, by the rule of section 7: the wealth of its
household and the standing of the vocation decide. No other rule below is specified in
detail or implemented: no place has a mix of vocations, and no person has a variant or a
by-work.

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
| `Standing` | The social standing, as a range `Min` to `Max` on a scale of six: 1 outcast, 2 poor, 3 common, 4 middling, 5 wealthy, 6 ruling. The wealth of a household picks among the vocations whose standing fits it (section 7) |
| `OnWater` | The vocation is only found in a place on water: the shipwright and the sailor. Left out means no |
| `Variants` | The smaller trades the vocation covers. Each has a `Name`, a `Description` and a `Lore` |
| `Description` | What the vocation is: the work a person of it does. Left out means `**missing description**` |
| `Lore` | The standing, the customs and the quarrels of the vocation. Left out means `**missing description**` |

The default `Description` and `Lore` are the opening paragraph and the line "Lore" of the
entry of the vocation in [research/vocations.md](research/vocations.md) section 3, without
the citations (`src/Gameworld.Domain/DefaultTexts.Vocations.cs`). No rule reads them.
A variant has the same two texts, so that a shepherd is not drawn as any herder. No doc
describes a variant, so the two texts of the 476 default variants are written in
`src/Gameworld.Domain/DefaultTexts.VocationVariants.cs`, keyed by "vocation id/variant
name": the work of the variant and what a picture of the person shows. A name that two
vocations share has a text for each (the sawyer of the forester saws over a pit, that of
the miller runs a sawmill). The lore is the standing, the customs and the quarrels of
the variant, and names a race of [races-basis.md](races-basis.md) where one is known for
the work. A variant that a Parameter Set file gives no text has
`**missing description**`. In a Parameter Set file a variant is an
object with a `Name`; until 2026-11-21 it was a plain name, and a file from before that
must be changed to load.
The defaults of `LowestTier`, `SharePercent`, `Standing`, `OnWater` and `Variants` are
those of [research/vocations.md](research/vocations.md) section 3: the index table and the
"Covers" line of each vocation. The scale of the standing is that of its section 2.4, and
13 of the 51 vocations have a range: a healer stands from 3 to 5, a courtesan from 1 to 5.
The shares sum to 100. They are starting values and are not tuned against generated
regions. A vocation does not yet hold its inputs and outputs, its by-works or its share
by sex.

**Products are names only.** A vocation lists what it consumes and what it produces by id.
The ids are the resources of the map and the products made from them: iron, cloth, flour,
and the by-products wool, hides, dairy, tallow, horn, tanbark, potash, resin and pitch. A
product has no amount on the map and does not flow between places. The lists say which
vocations a place can have (a smelter needs ore and fuel) and what a place must bring in
or can send out (smiths without a smelter in reach bring in iron).

**A rural person belongs to a patch.** There are no villages. The vocations of the
countryside are worked out per land patch, from its resources and its potential
population. The vocations of a city or a town are worked out per city or town. Until that
rule exists, a person of a city or a town draws its vocation by itself (section 7), and
nothing reads the resources of the place.

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
- What a child and an old person hold. A person holds a vocation from the working age
  (section 7), a child holds none and an old person keeps its vocation.
- Villages. On 2026-11-21 the user named villages as places of households, to be added
  later. The rule above says there are none and puts a rural person on a patch. One of the
  two must give way before the households of the land are specified (section 6).
- The factors per race, which races keep slaves, and which races other than the Twofold
  switch the division by sex off.
- The capital of a region is the seat of its ruling faction (section 4.4), and it is a
  city (section 5.2). What a vocation of the tier capital needs beyond that.

Material: [research/vocations.md](research/vocations.md) describes each of the 51
vocations (what it covers, its lore, its resources, where it is found, how frequent it is,
its races and factions), maps the resources and the biomes to the vocations, proposes the
new resources and holds a design note for the generator. Every entry of
[races-basis.md](races-basis.md) lists the work the race is known for.

### 8.1 Tools

Stage: none, a list in the Parameter Set. Specified and in the Parameter Set since
2026-11-21. Nothing draws a tool yet.

A tool is a thing a vocation works with: the anvil of the smith, the sickle of the farmer,
the fleam of the healer. The tools are a list in the Parameter Set, like the vocations, so
the code knows no tool by name. A tool of this list is not the produced resource group
Tools of [map-generation.md](map-generation.md) Appendix G.1: the group is the goods a
place makes and trades, a tool of the list is a kind of thing a person of a vocation holds.

What the tools are for:

- **Emblems.** The coats of arms of houses, guilds and some cities are to take their
  charges from the tools of a vocation, as the guilds of the record bear the tools of their
  trade: three hammers for the smiths, three fleams for the barbers
  ([research/heraldry-coat-of-arms-generation.md](research/heraldry-coat-of-arms-generation.md)
  sections 4.2 and 4.3). A family named for its work, a guild, and a city or a town known for
  one trade are the bearers in view. The rule of the coats of arms is not specified.
- **Portraits.** What a person of a vocation holds or wears at work (section 9).

**In the Parameter Set.** The list is `Tools`. A tool holds:

| Value | Meaning |
|---|---|
| `Id` | What the generated data knows the tool by, in camelCase. Unique in the list |
| `Name` | The name shown |
| `Enabled` | Whether the tool exists in the world. A tool that is off is no tool of any vocation |
| `Vocations` | The ids of the vocations that work with the tool, the vocation it belongs to most first. At least one, each a vocation of the `Vocations` list |
| `EmblemWeight` | How well the tool serves as the emblem of its vocations: its weight when an emblem is drawn among the tools of a vocation. 0 means never. Left out means 1 |
| `Description` | What it is and looks like: the text a picture of it is drawn from. Left out means `**missing description**` |
| `Lore` | What the peoples of the world make of it and tell about it. Left out means `**missing description**` |

The description and the lore of a default tool are those of its entry in
[tools-basis.md](tools-basis.md), the description without its source. They are in
`src/Gameworld.Domain/DefaultTexts.Tools.cs`. No rule reads them: they are what a picture
of the tool is drawn from.

The tools of a vocation are the tools that are on and list it, in the order of the list.
A vocation that is off has no tools. Its emblems are those of its tools with an emblem
weight above 0. The relation is many to many: the longbow is a tool of the hunter, the
soldier and the bandit. It lives on the tool, so a new tool touches one record, and a new
vocation gets its tools by naming it in their `Vocations`.

A Parameter Set with a vocation or a tool without an id, with the same id twice, with a
tool that lists no vocation or a vocation that is not in the list, or with an emblem weight
below 0 is refused when the world is created.

**The default list** holds 146 tools, all on: those of [tools-basis.md](tools-basis.md), in
its order, with its names. The `Vocations` of a tool are its line "Fitting vocations",
the vocation it stands under first. Every one of the 51 default vocations has at least two
tools and at least one with an emblem weight above 0.

The emblem weights of the defaults run from 0 to 3. They are the judgement of this
section and starting values:

| Weight | Meaning | Count | Examples |
|---|---|---|---|
| 0 | Cannot be drawn as one plain figure: a building, a pit, a heap, a set of small parts | 19 | Charcoal clamp, tan pits, bloomery furnace, treadwheel crane, kneading trough |
| 1 | Can be drawn, but does not say its trade at a glance | 59 | Seine net, windlass, awl, tally stick, rune stick |
| 2 | A plain figure that says its trade | 51 | Heavy plough, cleaver, trowel, potter's wheel, quill pen and penknife |
| 3 | The sign of its trade, borne as one in the record or as a common charge | 17 | Sickle, bee skep, shepherd's crook, hunting horn, felling axe, miner's iron and hammer, pole axe, anvil, hammers and tongs, coin balance, fleam and bleeding bowl, mortar and pestle, harp, knightly sword, mace of office, chalice, armillary sphere |

Six vocations have no tool above 1: the salter, the charcoal burner, the tanner, the
jeweller, the artist and the artificer. For two of them the arms of the record show the
product rather than a tool: the covered salts of the salters, the covered cup of the
goldsmiths. Products are not a list yet.

Open:

- The rule of the coats of arms, and with it whether a product of a vocation can be an
  emblem beside its tools.
- Whether a tool belongs to some vocations more than to others, beyond the first
  vocation of its list.
- Whether a race has tools of its own, or a factor per tool as it is to have per vocation.

---

## 9. Looks

Stage: none yet. The lists and what the looks of a person hold are specified and in the
Parameter Set since 2026-11-21. No rule draws the looks of a person yet.

The looks of a person are what a portrait must show beyond the race: the skin, the eyes,
the hair, the outfit and the accessories. The portrait itself is authored by a diffusion
model. The looks are not: they are derived data and part of the Stub of the person, drawn
by the person generator like the name and the vocation, so that a person has its looks
before any picture is made and every picture of the person repeats them.

**Portrait components.** The looks are built from portrait components. A component is one
fixed piece of the description of a person, picked from a list and then repeated word for
word in every picture of that person. That is what the makers of the image models ask for:
write the description of a character once, in concrete visual terms, and repeat it in full
in every prompt ([research/portrait-components.md](research/portrait-components.md)
section 10). There are six lists of components, all in the Parameter Set, so the code knows
no component by name:

| List | Entries | What an entry is | Section |
|---|---|---|---|
| `SkinColours` | 32 | A tone of skin, with the marking of the skin where that is the mark of a race | 9.2 |
| `EyeColours` | 27 | A colour of the eyes, with the pupil where that is the mark of a race | 9.2 |
| `HairColours` | 23 | A colour of the hair, and the colour it turns into with age | 9.2 |
| `Hairstyles` | 46 | How the hair is worn, with the beard or the head covering where that is the point | 9.3 |
| `Outfits` | 72 | What is worn, from the head covering to the shoes | 9.4 |
| `Accessories` | 51 | A piece of jewellery, of one of eight kinds | 9.5 |

The colours of an outfit are not a list of their own: they are two colours of the
`Colours` list of section 9.1.

**The looks of a person.** `Looks` on the Stub of a person holds the id of one entry of
each list, and the two colours of the outfit:

| Value | Meaning |
|---|---|
| `SkinColour` | The id of a skin colour. None when the race of the person lists none |
| `EyeColour` | The id of an eye colour. None when the race lists none |
| `HairColour` | The id of a hair colour. None when the race lists none: a race without hair |
| `Hairstyle` | The id of a hairstyle. None when no hairstyle fits the person |
| `Outfit` | The id of an outfit. None when no outfit fits the person |
| `OutfitMainColour` | The id of a colour of the `Colours` list: the body of the outfit. None without an outfit |
| `OutfitAccentColour` | The id of a colour of the `Colours` list: the trim, the sash or the lining. None without an outfit |
| `Accessories` | The ids of the accessories the person wears: none, one or several, at most one of a kind |

A person has no looks until they are drawn, and no rule draws them yet: `Looks` is empty on
every generated person.

**What a portrait is drawn from.** The description of the race (section 2) says what every
person of the race looks like: the build, the face and the marks that tell the race apart,
such as the tusks of an Orc or the ears of a Catfolk. The components say what differs
from person to person. A prompt is to hold both: the age and the sex of the Stub, the
description of the race, and the description of each component of the looks.

**Texts.** Every hairstyle, outfit and accessory has a `Description` and a `Lore`, like a
tool (section 8.1). A skin, eye or hair colour has a `Description` and no lore: it is a
colour. The description of a colour is a part of a sentence, such as "golden-tan skin with
a warm yellow-gold undertone", so that the descriptions of a person can be set one after
the other. No rule reads the texts. The default texts are in
`src/Gameworld.Domain/DefaultTexts.Hairstyles.cs`, `DefaultTexts.Outfits.cs` and
`DefaultTexts.Accessories.cs`, and those of the colours in `Looks.Defaults.cs`.

A Parameter Set with a component without an id or with the same id twice in a list is
refused when the world is created, and so is one that breaks a rule of sections 9.2 to
9.5.

Material: [research/portrait-components.md](research/portrait-components.md) holds the
lists the defaults are built from, with the sources of each entry, the fit to races and
climates, and what the makers of the image models say about keeping a person the same
between pictures. Section 9.6 says what the defaults take from it and what they leave. The
Appearance part of every entry of [races-basis.md](races-basis.md) says what a portrait of
the race must show.

### 9.1 Colours

A colour is a named colour of cloth, paint and banners. The colours are a list in the
Parameter Set, like the races, so a colour is added or changed by editing the list. Each
colour has:

- An **id** and a **name**. Generated data points at a colour by its id. The ids must be
  given and unique.
- A **value** as `#RRGGBB`: what a picture shows. Its lightness decides which secondary
  colour stands out against a main colour (section 4.9).

A Parameter Set with a colour without an id, with the same id twice or with a value that
is not `#RRGGBB` is refused when the world is created.

Four things point at the list: the favourite colours of a race (section 2), the main and
the secondary colour of a faction (section 4.9), the colours an outfit decides and the two
colours of the outfit of a person (section 9.4). The list holds the colours of things that
are made: the colours of skin, eyes and hair are three lists of their own (section 9.2).

The default list holds 24 colours, from undyed cloth to the costly dyes. The values are
starting values.

| Id | Name | Value | Lightness | What it is |
|---|---|---|---|---|
| `white` | White | `#F2F0E6` | 0.94 | Bleached linen, chalk, lime wash |
| `cream` | Cream | `#E8DCC0` | 0.86 | Undyed wool and linen, ivory, pearl |
| `silver` | Silver | `#C0C4C8` | 0.77 | Silver, tin, polished steel |
| `grey` | Grey | `#8A8D8F` | 0.55 | Undyed grey wool, ash, stone |
| `black` | Black | `#1E1E22` | 0.12 | Oak gall and iron, soot |
| `tan` | Tan | `#C8A977` | 0.68 | Raw hide, sand, straw |
| `brown` | Brown | `#6B4423` | 0.30 | Walnut husk, bark, undyed dark wool |
| `russet` | Russet | `#8B3A1E` | 0.31 | Rust, tanned leather, red earth |
| `ochre` | Ochre | `#C98A2B` | 0.57 | Yellow earth, amber, honey |
| `gold` | Gold | `#D4A017` | 0.63 | Gold leaf, brass, gilt thread |
| `yellow` | Yellow | `#F2CB05` | 0.75 | Weld, sulphur |
| `saffron` | Saffron | `#F28C0F` | 0.61 | Saffron, turmeric |
| `orange` | Orange | `#E0651A` | 0.51 | Madder over weld, flame |
| `red` | Red | `#C1272D` | 0.34 | Madder |
| `crimson` | Crimson | `#8C1127` | 0.22 | Kermes and lac: a costly dye |
| `rose` | Rose | `#D9778B` | 0.59 | Weak madder, coral, pink salt |
| `purple` | Purple | `#6A2C8C` | 0.29 | Sea-snail purple, lichen, wine: the costliest dye |
| `indigo` | Indigo | `#2B2D6E` | 0.20 | Indigo dyed deep |
| `blue` | Blue | `#1F5FA8` | 0.33 | Woad |
| `skyBlue` | Sky blue | `#7FB6DE` | 0.67 | Weak woad, the clear sky |
| `teal` | Teal | `#1F7A7A` | 0.37 | Woad over weld, jade, the sea |
| `green` | Green | `#2E8B3D` | 0.40 | Weld over woad, new leaves |
| `forestGreen` | Forest green | `#1F4D2B` | 0.23 | Deep green of old forest |
| `olive` | Olive | `#6B7A2A` | 0.43 | Olive leaf, moss, bronze gone green |

Open:

- A cost per colour, so that crimson and purple are the colours of the rich.

### 9.2 Skin, eye and hair colours

Stage: none, three lists in the Parameter Set. Nothing draws a colour yet.

The colour of the skin, the eyes and the hair of a person is one of the colours of its
race. The three lists are `SkinColours`, `EyeColours` and `HairColours`. A colour holds:

| Value | Meaning |
|---|---|
| `Id` | What the generated data knows the colour by, in camelCase. Unique in its list |
| `Name` | The name shown |
| `Description` | What a picture shows, as a part of a sentence: "pale grey eyes with a dark rim around the iris" |
| `Greying` | Hair colours only. The id of the hair colour this one turns into as the person greys. Left out, the colour does not change with age |

A colour is more than a hue where the mark of a race lies in the skin, the eye or the hair
itself: the blue-grey lips of the Hoarkin are in their skin colour, the bar pupil of the
Widegaze and the slit pupil of the Catfolk in their eye colours, and the rings of the hair
of the Swiftborn in their hair colour. So a colour has no value as `#RRGGBB`, and the
three lists are not the `Colours` list of section 9.1.

**The colours of a race.** A race lists the ids of its colours in `SkinColours`,
`EyeColours` and `HairColours`, the commonest first (section 2). The lists live on the
race, because the colours of a people are a fact of the people, and a new race names its
colours in its own entry. A race that lists none of a kind has persons without that
component.

**Greying.** A race lists the colours of the hair before it greys. `Greying` chains them to
the colours of age: black turns to greying dark, greying dark to iron grey, iron grey to
white, and the fair and the red colours to greying fair and then to white. The colours
that are the mark of a race and not of an age, such as the silver-white of the Elves of
the cold woods, do not grey. At what age the hair of a person turns is not specified.

A Parameter Set is refused when a race lists a colour that is not in its list, when a hair
colour greys into a colour that is not in the list, and when a chain of greying does not
end.

**The default lists** hold 32 skin colours, 27 eye colours and 23 hair colours. Twelve of
the skin colours, eight of the eye colours and ten of the hair colours are those of human
peoples, four hair colours are those of age, and the rest belong to one or two races each.
Every default race has at least one skin colour and one eye colour, and every race but the
Noonfolk a hair colour:

| Race | Skin colours | Eye colours | Hair colours |
|---|---|---|---|
| Northmen | `fairNeutral`, `ruddyFair`, `veryFairCool` | `blue`, `grey`, `green` | `flaxen`, `goldenBlond`, `sandy`, `lightBrown`, `copperRed`, `auburn` |
| Midlanders | `fairNeutral`, `lightBeigeWarm`, `ruddyFair`, `olive`, `veryFairCool` | `earthBrown`, `clearBrown`, `blue`, `grey`, `hazelGreen`, `green`, `amber`, `jet` | `darkBrown`, `lightBrown`, `chestnut`, `sandy`, `black`, `goldenBlond`, `softBlack`, `auburn`, `copperRed`, `flaxen` |
| Saltroad folk | `redBrown`, `lightBrownGolden`, `deepRedBrown` | `earthBrown`, `green`, `amber` | `black`, `softBlack`, `darkBrown` |
| Sunlanders | `darkBrownWarm`, `deepBrownCool` | `earthBrown`, `jet` | `black` |
| Grass riders | `weatheredBronze`, `goldenTan` | `earthBrown`, `jet` | `black` |
| Rainkeepers | `mediumBrownNeutral`, `lightBrownGolden`, `darkBrownWarm` | `earthBrown`, `jet` | `black`, `softBlack` |
| Canal folk | `goldenTan`, `lightBeigeWarm` | `earthBrown`, `jet` | `black` |
| Elves | `fairNeutral`, `veryFairCool`, `mediumBrownNeutral`, `darkBrownWarm` | `green`, `gold`, `grey` | `darkBrown`, `chestnut`, `silverWhite`, `black` |
| Dwarves | `seamedRuddy`, `seamedBrown` | `earthBrown`, `clearBrown`, `grey` | `darkBrown`, `black`, `auburn`, `copperRed`, `chestnut` |
| Halflings | `fairNeutral`, `ruddyFair`, `lightBeigeWarm` | `clearBrown`, `hazelGreen`, `blue` | `darkBrown`, `lightBrown`, `sandy` |
| Gnomes | `fairNeutral`, `lightBeigeWarm` | `grey`, `clearBrown` | `snowWhite` |
| Orcs | `greyGreen`, `deepOlive`, `dunBrown` | `yellow`, `rustBrown` | `black` |
| Hoarkin | `frostPale` | `rimmedPaleGrey` | `snowWhite`, `ashGrey` |
| Hearthbloods | `flushedCopper` | `emberDark` | `black`, `darkRed` |
| Widegaze | `tawny` | `barPupilGold` | `sandy`, `lightBrown`, `darkBrown` |
| Amberbloods | `resinFreckledGold` | `beadAmber` | `chestnut`, `copperRed`, `barkBrown` |
| Brinefolk | `shellGreyWhite` | `veryLightGrey`, `veryLightGreen` | `saltPale` |
| Duskreaders | `deepBrownGrey` | `allBlack` | `black` |
| Noonfolk | `copperBrownGreenVeined`, `deepOliveGreenVeined` | `green`, `gold` | None |
| Boughwalkers | `darkBrownWarm`, `deepRedBrown` | `hazelGreen` | `black` |
| Shoalfolk | `greyCastBrown`, `greenCastBrown` | `earthBrown`, `jet` | `black` |
| Swiftborn | `olive`, `mediumBrownNeutral`, `deepOlive` | `earthBrown`, `clearBrown`, `hazelGreen` | `ringedDarkAndPale` |
| Flinchless | `fairNeutral`, `lightBeigeWarm`, `olive`, `lightBrownGolden`, `mediumBrownNeutral`, `veryFairCool`, `ruddyFair`, `goldenTan`, `weatheredBronze`, `redBrown`, `darkBrownWarm`, `deepBrownCool` | `silverGrey` | `darkBrown`, `black`, `lightBrown`, `chestnut`, `softBlack`, `sandy`, `goldenBlond`, `flaxen`, `copperRed`, `auburn` |
| Lanternkin | `clearPale` | `lightGreyBlue` | `ashGrey` |
| Stillfaces | `tawnyGrey` | `earthBrown`, `jet` | `brindledBrownGrey` |
| Twofold | `mediumBrownNeutral`, `darkBrownWarm` | `earthBrown`, `jet` | `black` |
| Bitterbloods | `oliveBrownBrightPatched` | `yellowRingedDark` | `black` |
| Catfolk | `lightBeigeWarm`, `tawny`, `fairNeutral`, `redBrown`, `lightBrownGolden`, `darkBrownWarm`, `smokeGrey`, `deepBrownCool` | `slitPupilAmber`, `slitPupilGold`, `slitPupilGreen`, `slitPupilCopper`, `slitPupilPaleBlue`, `oddSlitPupilEyes` | `black`, `darkBrown`, `sandy`, `copperRed`, `patchedTwoColour`, `ashGrey`, `snowWhite` |

The Appearance part of races-basis.md names no skin for the Halflings and the Gnomes, no
eyes for the Sunlanders, the Dwarves, the Halflings, the Swiftborn, the Twofold and the
Elves of the cold woods, and no hair colour for the Dwarves and the Duskreaders. Their
colours in the table are a first assignment of this section.

### 9.3 Hairstyles, and who wears what

Stage: none, a list in the Parameter Set. Nothing draws a hairstyle yet.

A hairstyle is how a person wears the hair. It carries the beard where the beard is the
point (the ringed beard braids of the Dwarves) and the head covering where the hair is
hidden under one (the veil and wimple of a Midlander wife). It names no hair colour. The
list is `Hairstyles`. A hairstyle holds:

| Value | Meaning |
|---|---|
| `Id` | What the generated data knows the hairstyle by, in camelCase. Unique in the list |
| `Name` | The name shown |
| `Enabled` | Whether the hairstyle exists in the world. One that is off is worn by nobody |
| `WornBy` | Who wears it: see below. Left out, everybody can |
| `Description` | What it looks like: the text a picture is drawn from. Left out means `**missing description**` |
| `Lore` | Who wears it, what it says of the wearer and what is told about it. Left out means `**missing description**` |

**Who wears what.** A hairstyle, an outfit and an accessory each say who wears them in
`WornBy`. It holds six limits, and a person can wear the thing only when every one of them
holds for the person. A limit that is left out limits nothing.

| Value | Limit | Left out |
|---|---|---|
| `Sex` | The sex it is worn by | Both sexes |
| `Races` | The ids of the races that wear it | Every race |
| `LowestWealth`, `HighestWealth` | The wealth of the household (section 6), from the lowest level to the highest | Destitute to Rich |
| `Vocations` | The ids of the vocations it is the dress or the mark of. Only a person with one of them wears it | The vocation does not matter |
| `LowestAge`, `HighestAge` | The ages it is worn at, in years | Any age |
| `Married` | `true`: only the married. `false`: only the unmarried. Married is a person whose husband or wife lives in the household (section 7) | Both |

The limits read facts the Stub already holds: the race, the sex, the age, the vocation and
the husband or wife of the person, and the wealth of its household. They are the columns
"Worn by" of the note made into data: hair and dress mark age and marriage more often than
rank, and rank shows as wealth.

The relation to the races and the vocations lives on the thing that is worn, as the
vocations of a tool live on the tool (section 8.1). A new outfit touches one record, and a
new race gets its dress by being named in the `Races` of what it wears.

What a person can wear is every entry that is on and whose limits hold. Which of them the
person gets is the draw that is not specified yet.

A Parameter Set is refused when a `WornBy` names a race or a vocation that is not in its
list, when its lowest wealth is above its highest, and when its ages are below 0 or run
backwards.

**The default list** holds 46 hairstyles, all on: 18 worn by women only, 11 by men only
and 17 by both sexes. Every default race has a hairstyle for each sex, at each wealth,
married or not. The limits of the defaults are the judgement of this
section and starting values.

### 9.4 Outfits

Stage: none, a list in the Parameter Set. Nothing draws an outfit yet.

An outfit is what a person wears, from the head covering to the shoes. The list is
`Outfits`. An outfit holds what a hairstyle holds, and its colours:

| Value | Meaning |
|---|---|
| `Id`, `Name`, `Enabled`, `WornBy`, `Lore` | As for a hairstyle (section 9.3) |
| `MainColour` | The id of the main colour, where the outfit decides it. A colour of the `Colours` list. Left out, the colour is picked for the person |
| `AccentColour` | The id of the accent colour, where the outfit decides it. Left out, the colour is picked for the person |
| `Description` | What it looks like. What a head-and-shoulders picture shows comes first, and the rest after "Below:". The marks `{main}` and `{accent}` stand for the two colours |

**The two colours.** The outfit of a person has a main colour and an accent colour, both
colours of the `Colours` list of section 9.1. The main colour is that of the body of the
outfit: the gown, the coat, the wrap. The accent colour is that of what is set against it:
the trim, the sash, the lining, the border or the second garment.

An outfit either decides a colour or leaves it to the person:

- **Decided.** The habit of a nun is black and white, the gown of a peasant undyed grey,
  the robe of a desert clansman indigo. Such an outfit names the colour in `MainColour` or
  `AccentColour`, and its description writes the colour out.
- **Left to the person.** A dyed coat can be any colour. Such an outfit leaves the value
  out, and its description holds the mark `{main}` or `{accent}` where the colour stands:
  "Long coat of wool or cotton in {main}, ..., bound at the waist with a broad {accent}
  silk sash". The colour is then a fact of the person, in `OutfitMainColour` and
  `OutfitAccentColour` of its looks.

A picture is drawn from the description with the names of the two colours in the place of
the marks, in small letters: "in sky blue". The colour an outfit decides is used before
the colour of the person, so `OutfitMainColour` of a person in a habit is black. An outfit
can decide one colour and leave the other, and it can leave the accent colour unused.

How the colours that are left to the person are picked is not specified: from the
favourite colours of the race, from the colours of the faction of the household, or from
both.

A Parameter Set is refused when an outfit names a colour that is not in the `Colours` list.

**The default list** holds 72 outfits, all on: 19 worn by women only, 16 by men only and
37 by both sexes. 33 of them decide their main colour and 39 leave it to the person. Every
default race has an outfit for each sex at each of the five levels of wealth. Twenty
outfits are the dress of a vocation, among them the apron of the smith, the habit of the
priest and the mail of the soldier.

### 9.5 Accessories

Stage: none, a list in the Parameter Set. Nothing draws an accessory yet.

An accessory is a piece of jewellery worn on the body or in the hair. It differs from the
other components in one way: a person can have none, one or several. The list is
`Accessories`. An accessory holds what a hairstyle holds, and a kind:

| Value | Meaning |
|---|---|
| `Id`, `Name`, `Enabled`, `WornBy`, `Description`, `Lore` | As for a hairstyle (section 9.3) |
| `Kind` | Where the piece sits: one of the eight kinds below |

| Kind | What it is | Defaults |
|---|---|---|
| `Necklace` | Worn around the neck or at the throat: a string of beads, a pendant, a neck ring, a collar | 13 |
| `Earrings` | Worn in or on the ears | 4 |
| `Bracelet` | Worn on the wrist or the upper arm: bracelets, bangles and arm bands | 6 |
| `Ring` | Worn on a finger or a thumb | 7 |
| `Brooch` | Pinned to the dress: brooches, badges and plaques | 7 |
| `Crown` | Worn on the head: crowns, circlets, diadems and wreaths | 5 |
| `BodyJewellery` | Worn anywhere else: on the nose, the ankle, a tusk or the brow, or hung from the belt | 5 |
| `HairJewellery` | Worn in the hair | 4 |

A person wears at most one accessory of a kind, so that no two pieces sit in the same
place. How many a person wears is not specified. The note suggests none or one for most
persons and two or three for the rich.

The description writes one material into each piece, so that the text stays fixed. Where
the same piece is made cheaper or dearer, the lore says so.

**The default list** holds 51 accessories, all on: 44 drawn from history and 7 written for
single races. Three races have none, since their dress leaves no place for one: the
Duskreaders, the Boughwalkers and the Twofold.

### 9.6 The defaults and the note

The default lists are built from
[research/portrait-components.md](research/portrait-components.md): its entries, in its
order, with its names in sentence case and its descriptions without the sources. A test
holds the names against the tables of the note. What differs:

- **Entries that are left out.** The note was written for 32 races, and five of them are
  not in the default list (Tinefolk, Hushfolk, Longsleepers, Eyrie folk and Lineminds).
  The entries written for them alone are left out: three skin colours, two hair colours,
  four eye colours, one hairstyle, four outfits and two accessories. So are one outfit and
  one accessory of the high valleys, the pinned mountain dress and the gold ear spools,
  which no race of the list wears. The lore of one accessory lost a sentence about the
  Hushfolk.
- **The race markers are not a list.** The note has 26 race markers: antlers, tusks, a
  salt crust. A marker is fixed for the race and not picked for the person, so it stays in
  the description of the race (section 2).
- **One list for both sexes.** The note has a list for women, one for men and one for the
  races. Here each kind is one list, and the sex is a limit of `WornBy`.
- **The colours of the outfits.** The descriptions of the note name the colours of a dyed
  outfit. Here they hold the marks `{main}` and `{accent}`, or the one colour the outfit
  decides (section 9.4).
- **Who wears what is data.** The columns "Worn by" and the tables by race of the note are
  free text and a suggestion. `WornBy` and the colours of each race are their reading as
  data. Where the note gives a people no race, the reading is of this section: the warm
  coast dress goes to the Swiftborn, the fur parka and the herder tunic to the Northmen,
  the breast cloth and sarong to the Rainkeepers. The Flinchless, the Stillfaces and the
  Amberbloods, whose entry in races-basis.md names no dress, dress as the Midlanders do.
- **The lore.** The note has lore for the accessories only. The lore of the hairstyles and
  the outfits is written in the default text files, from the columns "Worn by" of the note
  and from races-basis.md.

What each default race wears:

| Race | Hairstyles | Outfits | Accessories |
|---|---|---|---|
| Northmen | Single back braid, Crown braid, Knotted tail, Long and loose, Combed long with braided beard | Strap dress and brooches, Nun's habit, Fur parka, Banded herder tunic, Healer's gown and satchel, Hunter's hood and jerkin, Sorceress robe, Padded coat and mail shirt, Beggar's rags, Player's motley, Smith's leather apron, Miner's hood and seat leather, Monk's habit, Padded jack and kettle hat, Shoulder-pinned cloak and tunic, Wizard's robe and pointed hat | Amber and glass bead string, Hammer amulet, Twisted neck ring, Penannular cloak brooch |
| Midlanders | Loose with circlet, Single back braid, Twin long plaits, Crown braid, Veil and wimple, Netted side coils, Half-up side braids, Short crop, Bowl crop, Chin-length bob, Tonsure, Long and loose | Kirtle and apron, Sideless surcoat, High-belted gown, Queen's mantle, Nun's habit, Healer's gown and satchel, Hunter's hood and jerkin, Sorceress robe, Padded coat and mail shirt, Beggar's rags, Player's motley, Field tunic and hood, Smith's leather apron, Miner's hood and seat leather, Furred gown and wound hood, Doublet and hose, King's mantle and crown, Monk's habit, Mail and surcoat, Padded jack and kettle hat, Wizard's robe and pointed hat, Guild-cut town dress | Twisted neck ring, Chain of office, Gold signet ring, Bronze seal ring, Clasped-hands ring, Cabochon stone ring, Bow fibula, Ring brooch, Garnet disc brooch, Pilgrim badge, Plain brow circlet, Oak-leaf wreath, Temple rings |
| Saltroad folk | Nape knot, Tiered curls, Blunt braided wig, Shaved head, Short crop, Bare scalp, Oiled ringlets | Jewelled court collar, Linen sheath and broad collar, Desert body wrap, Court kaftan, Beggar's rags, Linen kilt and broad collar, Veil-turban and robe, Wide-sleeved grand robe | Blue glazed bead string, Silver amulet case, Plain hoop earrings, Pearl drop earrings, Single ear ring, Stacked glass bangles, Beast-head bangle, Serpent armlet, Gold signet ring, Scarab swivel ring, Chained triangle brooches, White cloth diadem, Gold nose ring, Belled anklets |
| Sunlanders | Beaded cornrows, Ochre plaits, Shaved head, Tall headwrap, Bare scalp, Warrior braids, Pattern-shaved scalp, Clay and bead crown | Wrapper and blouse, Strip-woven royal cloth, Red herder cloth, Beggar's rags, Wide-sleeved grand robe, Indigo wrapped cotton | Tooth and bone string, Cowrie shell string, Red coral bead collar, Plain hoop earrings, Bronze upper-arm band, Open brass bangle, Beaded fringe crown |
| Grass riders | Single back braid, Winged horns, Shaved crown with side braids | Wrapped riding coat, Beggar's rags, Lamellar coat | Twisted neck ring, Plain hoop earrings, Single ear ring, Archer's thumb ring, Gold stag plaque |
| Rainkeepers | Single back braid, Flowered braid, Oiled long hair | Draped sari, Breast cloth and sarong, Beggar's rags, Waist cloth and shoulder cloth | Marriage thread, Plain hoop earrings, Filigree flower earrings, Bronze upper-arm band, Stacked glass bangles, Gold signet ring, Gold nose ring, Belled anklets, Forehead pendant |
| Canal folk | Pinned high knot, Trailing court hair, Topknot | Jacket and high skirt, Layered court robes, Field jacket and straw hat, Beggar's rags, Lamellar coat, Official's badge robe, Crossed robe and sash | Archer's thumb ring, Phoenix crown, Jade belt pendant, Carved hair comb |
| Elves | Loose with circlet, Half-up side braids, Long and swept back | Woodland greys and greens | Plain brow circlet |
| Dwarves | Ringed beard braids | Smith's leather apron, Miner's hood and seat leather, Thick wool and tool belt | Bronze upper-arm band, Gold signet ring, Bronze seal ring, Cabochon stone ring |
| Halflings | Curly mop | Beggar's rags, Bright waistcoat and wide hat | Gold signet ring, Bronze seal ring, Clasped-hands ring, Cabochon stone ring, Ring brooch |
| Gnomes | White tufts, Pointed cap | Beggar's rags, Many-pocketed coat | Gold signet ring, Bronze seal ring, Cabochon stone ring |
| Orcs | Coarse crest, Rough knot | Beggar's rags, Hide and won armour | Twisted neck ring, Tooth and bone string, Bronze upper-arm band, Banded tusks |
| Hoarkin | Dense close crop | Single sealskin | Tooth and bone string, Ivory amulet string |
| Hearthbloods | Cropped short, Short crop | Kilt and open vest | Twisted neck ring, Bronze upper-arm band |
| Widegaze | Hard tie-back | Plain long coat and cut-away hat | Twisted neck ring, Penannular cloak brooch, Garnet disc brooch, Signal mirror disc |
| Amberbloods | Single back braid, Nape knot, Short crop, Long and loose | Kirtle and apron, Sideless surcoat, High-belted gown, Queen's mantle, Nun's habit, Healer's gown and satchel, Hunter's hood and jerkin, Sorceress robe, Padded coat and mail shirt, Beggar's rags, Player's motley, Field tunic and hood, Smith's leather apron, Miner's hood and seat leather, Furred gown and wound hood, Doublet and hose, King's mantle and crown, Monk's habit, Mail and surcoat, Padded jack and kettle hat, Wizard's robe and pointed hat, Guild-cut town dress | Tooth and bone string |
| Brinefolk | Single back braid, Nape knot, Short crop, Long and loose | Pale wrapped linen | Plain hoop earrings, Beast-head bangle, Gold signet ring, Shaped salt crust |
| Duskreaders | Single back braid, Nape knot, Short crop, Long and loose | Quilted robe and cloth mask, Quilted robe and horn visor | None |
| Noonfolk | Hairless | Hip wrap, bare back | Plain hoop earrings, Open brass bangle |
| Boughwalkers | Flat skull braids | Short wraps and pouch belts | None |
| Shoalfolk | Beaded cornrows, Cropped short, Short crop | Loincloth and dive belt | Cowrie shell string, Shell braid beads |
| Swiftborn | Single back braid, Long and loose | Nun's habit, Pinned tunic and wrap, Priestess whites, Healer's gown and satchel, Hunter's hood and jerkin, Sorceress robe, Padded coat and mail shirt, Beggar's rags, Player's motley, Smith's leather apron, Miner's hood and seat leather, Monk's habit, Padded jack and kettle hat, Citizen's toga, One-shoulder work tunic, Wizard's robe and pointed hat | Plain hoop earrings, Grooved relay ring |
| Flinchless | Loose with circlet, Single back braid, Twin long plaits, Crown braid, Veil and wimple, Netted side coils, Nape knot, Cropped short, Short crop, Bowl crop, Chin-length bob, Tonsure, Long and loose | Kirtle and apron, Sideless surcoat, High-belted gown, Queen's mantle, Nun's habit, Healer's gown and satchel, Hunter's hood and jerkin, Sorceress robe, Padded coat and mail shirt, Beggar's rags, Player's motley, Field tunic and hood, Smith's leather apron, Miner's hood and seat leather, Furred gown and wound hood, Doublet and hose, King's mantle and crown, Monk's habit, Mail and surcoat, Padded jack and kettle hat, Wizard's robe and pointed hat, Guild-cut town dress | Chain of office, Gold signet ring, Bronze seal ring, Clasped-hands ring, Ring brooch |
| Lanternkin | Single back braid, Nape knot, Short crop, Long and loose | Hood, long sleeves and gloves | Cabochon stone ring, Blood-lamp vial |
| Stillfaces | Single back braid, Nape knot, Short crop, Long and loose | Kirtle and apron, Sideless surcoat, High-belted gown, Queen's mantle, Nun's habit, Healer's gown and satchel, Hunter's hood and jerkin, Sorceress robe, Padded coat and mail shirt, Beggar's rags, Player's motley, Field tunic and hood, Smith's leather apron, Miner's hood and seat leather, Furred gown and wound hood, Doublet and hose, King's mantle and crown, Monk's habit, Mail and surcoat, Padded jack and kettle hat, Wizard's robe and pointed hat, Guild-cut town dress | Scented wrist cord |
| Twofold | Oiled long hair | Wrapped cotton, saffron sash, Wrapped cotton, indigo sash | None |
| Bitterbloods | Oiled slick-back | Bare-shouldered home wrap, Gloves and waxed skin | Plain hoop earrings, Cabochon stone ring |
| Catfolk | Single back braid, Nape knot, Cropped short, Short crop, Long and loose | Beggar's rags, Loose tail-slit clothes, Hood and bound tail | Tooth and bone string, Cowrie shell string, Bronze upper-arm band, Open brass bangle |

Open:

- **The draw.** How the looks of a person are drawn in the Peopled stage: the purpose
  numbers, the weight of each entry a person can wear, whether a person with a vocation
  takes the dress of the vocation before any other, and how many accessories a person has.
- **The colours of an outfit.** Whether they come from the favourite colours of the race,
  from the colours of the faction of the household, or from both.
- **Children.** The default lists are written for adults. What a child wears, and whether
  the hairstyle of a person changes with its age, is open. So is the age at which hair
  greys, which differs between the races: a Swiftborn of twenty-five is old, and an Elf
  shows no age.
- **Clashes.** Two components can repeat or contradict each other: a veil and a crown, a
  hat in the outfit and a hairstyle that is a head wrap, a necklace under a beard. The note
  lists them in its section 9.4. They are not data yet: only the kind of an accessory is.
- **States.** Some dress follows a state of the person that the Stub does not hold: at
  home or abroad (the Duskreaders, the Bitterbloods, the Catfolk), a feast day, a season.
  Each person gets one outfit.
- **Material.** Rank shows in cloth and metal more than in cut. An outfit and an accessory
  name one material, and the wealth of the household does not change it.
- **What else a portrait needs.** The build, the height and the shape of the face are in
  the description of the race and in no list. Facial hair is part of four hairstyles and
  not a component of its own.
- Whether the texts give the same person in every picture is untested: no prompt was run.

---

## Appendix A. The default races

The default list of races of the Parameter Set (section 2): 28 races, all on. The first
seven are the human races, the next five the classic races and the last sixteen the new
races. The seven human races carry the flag `Human`, and no other race does.
[races-basis.md](races-basis.md) describes each of them.

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
Mangrove. The seventeenth biome, Volcano, has no column: the Dwarves are native there,
with the weight 3 and so the frequency 6.9, and every other race has the weight 0. A
Volcano patch holds Dwarves where they are a race of the region, and nobody elsewhere.

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
| `dwarf` | Dwarves | 2.3 | 1 | 2 | 1 | 2 | 1 | 1 | 1 | 2 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 |
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

**The races a race appreciates and hates.** Two lists of other races per race (section 2).
They are read from the "Relations" part of each entry of races-basis.md, and they are a
first assignment: starting values, like the leanings. "Hates" gathers fear, rivalry and an
old grudge. Where the lore is silent or only hints, the reading is marked as judgement.
The lists run one way. Orcs are hated by the most races (8), then Canal folk (6). Catfolk are
appreciated by the most (8).

| Race | Appreciates | Hates | Read from |
|---|---|---|---|
| Northmen | Dwarves, Hoarkin, Widegaze | Elves, Hearthbloods | Honours Dwarves as smiths, buys ivory from the Hoarkin, hires Widegaze lookouts. Elves make them uneasy, and hungry Hearthblood halls raid them |
| Midlanders | Halflings, Dwarves, Gnomes, Flinchless, Catfolk | Northmen, Grass riders, Elves | Fed by the Halflings in the ash, dwarven iron, Gnomes and Flinchless in their towns, a Catfolk ward by the granary. Fears Northmen and Grass riders, and the Elves stand between the plough and the forest |
| Saltroad folk | Sunlanders, Duskreaders, Lanternkin, Noonfolk, Catfolk | Grass riders, Orcs | The oldest partnership with the Sunlanders, Duskreader night guides, Lanternkin letters, Noonfolk grain, Catfolk scouts. Grass riders are rivals for the trade. Orcs raid the dry edge (judgement) |
| Sunlanders | Saltroad folk, Noonfolk, Boughwalkers, Twofold, Catfolk | Orcs, Brinefolk | Partners in the gold trade, neighbours and trading partners of forest and grassland. Orcs raid the dry edge. The Brinefolk set the price of salt (judgement) |
| Grass riders | Widegaze, Brinefolk | Orcs, Canal folk, Saltroad folk | The Widegaze are allies and lookouts, the Brinefolk sell salt for the herds. Orcs are rivals for the grass, the Canal folk build walls, the Saltroad folk are rivals for the trade |
| Rainkeepers | Boughwalkers, Saltroad folk, Shoalfolk, Catfolk | Canal folk, Twofold | Boughwalker dyes, Saltroad carriers, Shoalfolk pearls, a Catfolk ward. Canal folk are rivals in trade, and the Twofold fit no rank of birth |
| Canal folk | Lanternkin, Gnomes, Dwarves, Catfolk | Grass riders, Elves, Shoalfolk | Lanternkin notaries, Gnomes in their towns, dwarven iron, a Catfolk ward. The Grass riders are the danger the walls face, the Elves guard the forest, and the Shoalfolk pay no land tax and smuggle (judgement) |
| Elves | Halflings, Swiftborn, Amberbloods | Midlanders, Canal folk, Dwarves | Fond of Halflings, drawn to the Swiftborn, kin of a prickly kind with the Amberbloods. The slow war with those who clear the forest, and the quarrel with the Dwarves over charcoal |
| Dwarves | Gnomes, Hearthbloods, Northmen, Midlanders | Orcs, Elves, Amberbloods | Gnomes are cousins, Hearthbloods work the forges, Northmen and Midlanders buy the iron and grow the grain. Orcs fight them for the passes, and charcoal is a dispute with Elves and Amberbloods |
| Halflings | Midlanders, Elves, Gnomes, Swiftborn, Catfolk | Orcs, Grass riders | Gets on with nearly everyone. Fears Orcs and Grass riders above all |
| Gnomes | Dwarves, Halflings, Lanternkin, Swiftborn | Orcs | Dwarves are cousins, Halflings share the markets, Lanternkin are partners in banking, a Gnome holds a Swiftborn trust. The lore names no enemy: Orcs as raiders of a people that never fights (judgement) |
| Orcs | Widegaze, Sunlanders, Flinchless | Dwarves, Catfolk, Grass riders | Keeps its treaties with the Widegaze, serves in the Sunlander empires, respects the other legion of the Old Realms (judgement). Fights Dwarves for the passes, competes with Catfolk for game and guard contracts and with Grass riders for the grass |
| Hoarkin | Northmen | Hearthbloods | Trades with the Northmen. The Hearthbloods are their opposite, and a handshake hurts both |
| Hearthbloods | Dwarves, Northmen, Midlanders | Hoarkin | Dwarves hire them and sell them grain, Northmen and Midlanders pay them as pass wardens. The Hoarkin are their opposite |
| Widegaze | Grass riders, Orcs, Northmen | Catfolk | Allies of the Grass riders, treaties with the Orcs, hired by the Northmen. Their mothers were hunted by great cats (judgement) |
| Amberbloods | Elves, Stillfaces | Dwarves, Midlanders, Hearthbloods | Shares the forest with the Elves, Stillfaces are quiet neighbours. Dwarven charcoal, the loggers and tappers of the old provinces, and a people of fire: resin burns (judgement) |
| Brinefolk | Saltroad folk, Grass riders, Shoalfolk | Canal folk | The Saltroad folk carry the salt, the Grass riders buy it, the Shoalfolk share the salt-pan coasts. The Canal folk run a state monopoly of salt (judgement) |
| Duskreaders | Noonfolk, Saltroad folk | Midlanders, Catfolk | The Day-and-Night Pact with the Noonfolk, hired by the Saltroad folk. The mobs of the fallen Realms, and the Catfolk as rivals for the night watch (judgement) |
| Noonfolk | Duskreaders, Sunlanders, Catfolk, Swiftborn, Flinchless | Midlanders, Northmen | Those who guard their nights, and the other made races. The heirs of the Realm that claim them as property, and a people that keeps thralls (judgement) |
| Boughwalkers | Rainkeepers, Sunlanders, Shoalfolk | Bitterbloods | Sells dyes to the Rainkeepers, trades at the root markets of the Sunlanders, rigs for the Shoalfolk. The Bitterbloods hold the floor beneath them |
| Shoalfolk | Boughwalkers, Brinefolk, Northmen | Canal folk, Bitterbloods | Boughwalkers work their rigging, Brinefolk share the coasts, the cold-water branch deals with the Northmen. No ground, no lord: the officials of the Realm (judgement). Keeps out of the creeks of the Bitterbloods |
| Swiftborn | Elves, Gnomes, Halflings, Noonfolk, Flinchless, Bitterbloods | Midlanders | An Elf or a Gnome as trustee, Halflings hire the harvest gangs, the other made races are kin. Human lords treat them as children and outwait them |
| Flinchless | Midlanders, Noonfolk, Swiftborn, Bitterbloods | Orcs | Lives among the Midlanders, a fellowship of the made races. Orcs are the rival companies for hire (judgement) |
| Lanternkin | Gnomes, Canal folk, Saltroad folk | Midlanders | Gnomes build the vaults, the Canal folk employ them, Saltroad merchants honour their letters. Lords who owe them money, hunt them or keep one as a living lamp |
| Stillfaces | Amberbloods, Catfolk | Bitterbloods | Likes the resin smell of the Amberbloods and finds the Catfolk restful. Bitterbloods smell of every warning there is |
| Twofold | Sunlanders, Rainkeepers | Canal folk | Trades with the Sunlanders and lives beside the Rainkeepers. The lore names no enemy: a people that registers every race in a named category (judgement) |
| Bitterbloods | Noonfolk, Swiftborn, Flinchless | Stillfaces | The other made races are kin. Stillfaces shun them and are the poison finders of the courts (judgement) |
| Catfolk | Halflings, Noonfolk, Sunlanders, Stillfaces, Midlanders | Orcs, Duskreaders | Wards and night watch of those who grow grain and herd. Orcs compete for game and contracts, and a people behind masks to one that has no use for a straight face (judgement) |

**Favourite colours.** The favourite colours of each race, the dearest first (section 2).
They are read from the "Appearance" and "Way of life" parts of each entry of
races-basis.md, and they are a first assignment. Where the lore names no colour, the
reading is marked as judgement.

| Race | Favourite colours | Read from |
|---|---|---|
| Northmen | Red, Blue, Silver | Madder and woad on wool, and wealth worn as silver arm-rings |
| Midlanders | Blue, Red, Gold, Green | Dyed cloth cut to the fashion of a guild, and coats of arms (judgement) |
| Saltroad folk | Indigo, White, Ochre | The wound cloth and veil of the desert clans, loose pale robes, sand and mud brick (judgement) |
| Sunlanders | Indigo, Gold, Red, Orange | Wrapped cotton in strong dyes, indigo first among them, and gold |
| Grass riders | Sky blue, Red, Gold | They worship the Sky. Red and gold are judgement |
| Rainkeepers | Saffron, Crimson, Green, Rose | Cotton in colours that tell the community, and a mark of coloured paste on the brow (judgement) |
| Canal folk | Red, Yellow, Teal | Silk in the colour of a grade, lacquer, jade (judgement) |
| Elves | Forest green, Grey, Brown, Silver | They dress in greens, greys and browns that are hard to see between trees. Silver for the silver-haired of the cold woods |
| Dwarves | Gold, Russet, Black | Gold and gems, leather, the forge (judgement) |
| Halflings | Yellow, Green, Red | Bright waistcoats, garden and orchard (judgement) |
| Gnomes | Purple, Gold, Teal | The lore names no colour: fine work for those who pay for the costly dyes (judgement) |
| Orcs | Black, Crimson, Tan | Scars rubbed with ash, hide and leather, the standard of a legion (judgement) |
| Hoarkin | White, Sky blue, Grey | Skin as pale as skimmed milk shading to blue-grey, snow and sea ice |
| Hearthbloods | Orange, Red, Black | The fire inside them, flushed copper skin, the last coals |
| Widegaze | Gold, Tan, Black | Pale gold eyes with a dark bar, tawny skin, bold striped weaving |
| Amberbloods | Ochre, Forest green, Brown | Amber eyes and resin, the old forest, bark-brown hair |
| Brinefolk | White, Rose, Russet | Pale wrapped linen, and pans in squares of white, pink and rust |
| Duskreaders | Black, Grey, Indigo | Colour means little to them: eyes black from lid to lid, dark gauze, the night |
| Noonfolk | Yellow, Olive, Cream | The sun, the green-bronze veining, a plain wrap at the hips |
| Boughwalkers | Brown, White, Purple | Cloth that looks plain brown or white to everyone else. Purple stands for the high colour beyond violet |
| Shoalfolk | Teal, Cream, Rose | The shallows, pearl and shell, coral |
| Swiftborn | Purple, Olive, Orange | The vineyards and the olive harvest of the province they were made to fill, and a festival every month (judgement) |
| Flinchless | Silver, White, Crimson | Pale silver eyes, a lattice of white scars, the legion (judgement) |
| Lanternkin | Teal, Silver, Black | The blue-green light in the veins, ash-coloured hair, the shadow it shows in |
| Stillfaces | Grey, Brown, Olive | Tawny-grey skin, hair brindled in brown and grey, the mixed forest |
| Twofold | Saffron, Indigo, Gold | The sash: saffron before the Turning, indigo after it. The patch at the throat is pale gold, then indigo |
| Bitterbloods | Sky blue, Yellow, Orange | The mottling worn with pride: sky blue, sulphur yellow, sometimes orange |
| Catfolk | Tan, Russet, Cream, Gold | The colours of a cat's coat: sand, russet, cream, tawny gold |

**Name styles.** The name style of each race (section 2). They are a first assignment, by
judgement from the entries of races-basis.md: Common for the human races and the races
that are their kin or live among them, Exotic for the Elves and the races of the forest,
the water and the gods, Dark for the Orcs and the races of harsh land, night and poison.
The earlier project gave the Elves the exotic names, the Orcs the dark ones and the
Dwarves and Halflings the common ones, and that is kept.

| Name style | Races | Read from |
|---|---|---|
| Common | Northmen, Midlanders, Saltroad folk, Sunlanders, Grass riders, Rainkeepers, Canal folk | The human races. The common model is the only human one |
| | Dwarves, Halflings, Gnomes | As in the earlier project. Halflings are small cousins of the humans, and Gnomes are kin of the Dwarves |
| | Hoarkin | Cousins of the humans who live beside the Northmen |
| | Swiftborn, Flinchless, Lanternkin | Made or shaped in the realms of humans, and living in their provinces, legions and towns |
| Exotic | Elves | As in the earlier project |
| | Amberbloods, Boughwalkers, Stillfaces | Peoples of the forest: a compact with the old forest, the canopy of the rain forest, a speech of scent |
| | Shoalfolk | A people of the shallows |
| | Noonfolk, Twofold, Catfolk | Peoples of the sun and of the gods |
| Dark | Orcs | As in the earlier project |
| | Hearthbloods, Widegaze, Brinefolk | Peoples of harsh land: the tundra, the cold steppe, the cold desert |
| | Duskreaders | A people of the night |
| | Bitterbloods | A people of poison |

**How common.** `common` is 3 to 5 for the human races, 2 for Elves, Halflings
and Orcs, 2.3 for Dwarves, 1 for Gnomes, and 1 or 0.5 for a new race: 1 when it has a homeland of its own,
0.5 when it lives scattered among others or is rare by its nature.

The seven human races live in every biome but the Volcano and at every height between
them, and the Dwarves live on the Volcano from 200 m and up, so only a Volcano patch below
200 m is without a race by the plain shares. A patch can still hold nobody, when none of the six
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
| [plans/cities-and-towns.md](plans/cities-and-towns.md) | The plan behind the cities and towns of a region: what a region knows when they are made, the rule, its measurement on 676 regions and what was weighed | Section 5.2 |
| [research/settlement-siting.md](research/settlement-siting.md) §2.6, §2.7 and §6.7 | The rank-size rule and the urban shares of medieval Europe. A map note: it uses the names of an earlier prototype | Section 5.2 |
| [research/vocations.md](research/vocations.md) | 51 vocations with their lore, resources, biomes, races and factions, the proposals for new resources, and a design note | Section 8 |
| [research/heraldry-coat-of-arms-generation.md](research/heraldry-coat-of-arms-generation.md) | How coats of arms could be generated for factions, cities, towns and families: the parts of a coat, its rules, where the charges come from, and a proposal | Sections 4.9, 8.1 |
| [tools-basis.md](tools-basis.md) | 146 real tools for the 51 vocations, each with a sourced description, lore and the fitting vocations. The default tool list is built from it: its names, its order and its fitting vocations | Sections 8.1, 9 |
| [research/portrait-components.md](research/portrait-components.md) | Lists of hairstyles, outfits, colours, race markers and accessories for portraits, each with its sources. The six default lists of the portrait components are built from it (section 9.6). Written for the 32 races of the first version of races-basis.md and extended for the Catfolk: it still covers the five that were left out | Section 9 |
| [NameGenerator-Reuse.md](NameGenerator-Reuse.md) | The person-name generator, ported from an earlier project | Section 7 |
| [city-and-town-names.md](city-and-town-names.md) | 50 invented city and town names for each of the 28 default races, with what the names of each race are built from. Material: no rule reads it, and the word lists of section 5.3 are built from its notes | Section 5.3 |
| [research/city-and-town-naming.md](research/city-and-town-naming.md) | How settlement names are built in real place-names, in fantasy literature and in generators and games, and the rule it proposed for this generator | Section 5.3 |
| [RegionNameGenerator-Reuse.md](RegionNameGenerator-Reuse.md) | The region-name generator of the same project: the original, its defects and the corrected form that was ported | Section 4.10 |
