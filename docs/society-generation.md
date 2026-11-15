# How the society is generated

This document describes how the map is populated: the races, the potential population, the
factions, the cities and towns, the families and the persons, with their vocations and
their looks. It is the specification of the society generation, as
[map-generation.md](map-generation.md) is of the map. The [readme](../readme.md) defines
the concepts and holds the names, and the names in this document follow the readme.

Only the first step is specified and implemented: the races and the potential population
(sections 2 and 3). Sections 4 to 9 hold what is decided about the rest and what is still
open. Detailing them is a later task.

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
| Potential population | Planned | Specified and implemented (section 3) |
| Factions | Planned | Not specified (section 4) |
| Cities and towns | Settled | Not specified (section 5) |
| Families | Settled | Not specified (section 6) |
| Persons | Settled for the important persons, later for the rest | The names are implemented, the rest is not specified (section 7) |
| Vocations | Planned for the countryside, Settled for the cities and towns | Decided in part, not specified (section 8) |
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
  authored data can point at it. What the parent of each kind is has not been decided.
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

In a Parameter Set file a race lists only the biomes it lives in. A biome that is left out
has the frequency 0, and a frequency of 0 is not written to the file. A race without a
height modifier does not carry one in the file.

| Parameter | Default |
|---|---|
| Races | 28, all on (Appendix A) |
| Height modifiers | Elves, Dwarves, Hearthbloods, Widegaze and Shoalfolk (Appendix A) |

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

- How a faction, a family or a person gets its race, and whether a family can be mixed.
- What a race can ask for beyond the biome and the height: coast, river, elevation type,
  bedrock, the Supernatural and Prosperity fields, a town, another race to live among
  (collected in races-basis.md, "What the entries ask for that a race cannot say yet").
- A presence per race, so that a race clusters in some regions and is absent from others.
- Size, lifespan and fertility per race.
- The factors per vocation, the switch for the division of work by sex and whether the
  race keeps slaves. They are decided in section 8 and are not in the Parameter Set yet.

---

## 3. Potential population

Stage: Planned.

The potential population is an estimate of how many persons the land of a region can
feed, for each race that is on. It is not a count of people that exist: cities, families
and persons come in the Settled stage.

| Parameter | Default |
|---|---|
| Persons per km² at a food supply of 1 and a middling Prosperity | 6 |
| Prosperity effect | 0.5 |

The inputs are the final form of the region's land patches (the Elevated stage,
map-generation.md sections 12 to 14): the height of each patch and the biome and resources
calculated with it. The height of a patch is its mean height in the height map, not below
0, or the typical height of its elevation type (map-generation.md section 13.1) when no
raster cell centre lies in the patch. So the potential population depends on the height
map, unlike the Prospects (map-generation.md section 15). There is no random choice in
this section, so it needs no hash purpose.

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

### 3.2 Shares of the races

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

### 3.3 Potential population of a region

```
population(r) = round(Σ capacity(patch) · share(r, patch))   over the region's land patches, in key order
```

The result has one entry for every race that is on, in the order of the Parameter Set. A
race that finds no land to live on has 0. Each race is rounded on its own, half up, so the
sum over the races can differ from the rounded sum of the capacities by a few persons.

The land patches of a region are the patches of its zones (map-generation.md section
6.2), so every land patch counts in exactly one region.

With the defaults the median region has a potential population of about 35,100 and the
range is 11,200 to 83,400 (measured on 50 regions of two seeds). Of all persons 59 % are
of the seven human races, 20 % of the five classic races and 21 % of the sixteen new
races. The largest race is the Canal folk with 12 %, and no race has a majority in any of
the 50 regions. The shares per race are in [races-basis.md](races-basis.md), "Measured
shares". 39 % of the land patches lie at 200 m or higher, and every one of the 50 regions
has both dwarves and elves.

---

## 4. Factions

Stage: Planned. Not specified yet.

A faction is a group of people in one region: a political grouping. It could be a
kingdom, a thieves' guild or a tribe.

Open:

- What a faction holds, and what the parent of its identity is.
- How many factions a region has, and how they follow from the potential population and
  the Prospects.
- The race of a faction.
- How factions stand to each other, inside a region and across region borders.

Material: [research/factions.md](research/factions.md) holds the faction archetypes, the
axes that tell them apart, their relations and a design note for the generator. Every
entry of [races-basis.md](races-basis.md) lists the kinds of group the race forms.

---

## 5. Cities and towns

Stage: Settled. Not specified yet.

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

The rules below were decided on 2026-11-15. None of them is specified in detail or
implemented.

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

- What a vocation holds in the Parameter Set, and the default values: the shares, the
  inputs and outputs, the lowest tier, the by-works and the shares by sex.
- The rule that gives the vocations of a place from its resources, its size, its races and
  the fields, and the purpose numbers of its draws.
- The list of roles, and which roles fit which vocations.
- The age at which a person gets a vocation, and what a child and an old person hold.
- The factors per race, which races keep slaves, and which races other than the Twofold
  switch the division by sex off.
- What makes a city the capital.

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

**How common.** `common` is 3 to 5 for the human races, 2 for Elves, Dwarves, Halflings
and Orcs, 1 for Gnomes, and 1 or 0.5 for a new race: 1 when it has a homeland of its own,
0.5 when it lives scattered among others or is rare by its nature.

The seven human races live in every biome and at every height between them, so no land
is without a race. In Ice & snow there are Northmen, Hoarkin and Hearthbloods, and
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
| [research/vocations.md](research/vocations.md) | 51 vocations with their lore, resources, biomes, races and factions, the proposals for new resources, and a design note | Section 8 |
| [research/portrait-components.md](research/portrait-components.md) | Lists of hairstyles, outfits, colours, race markers and accessories for portraits. Written for the 32 races of the first version of races-basis.md and extended for the Catfolk: it still covers the five that were left out | Section 9 |
| [NameGenerator-Reuse.md](NameGenerator-Reuse.md) | The person-name generator, ported from an earlier project | Section 7 |
| [RegionNameGenerator-Reuse.md](RegionNameGenerator-Reuse.md) | The region-name generator of the same project, not ported | Section 5 |
