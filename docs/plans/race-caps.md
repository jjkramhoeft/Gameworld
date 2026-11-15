# Plan: a few races per region, city and town

Status: implemented 2026-11-15. The rules are in
[society-generation.md](../society-generation.md) sections 3.3, 3.4 and 5.1. This document
keeps the test behind them, what the prototype expected and what was weighed.

## Problem

Every region holds nearly every race. The rule of society-generation.md section 3 shares each
patch between all races by their frequency in its biome, and a region has many biomes. Cities
and towns have no race rule yet (section 5 is open), but the land around a Prospect is just as
mixed.

Measured today on 1,079 regions (the 18 × 15 regions around (0, 0) of the seeds `cellgen`,
`gameworld`, `atlas` and `ember`, default Parameter Set):

| | Lowest | Median | Highest |
|---|---|---|---|
| Races with any persons in a region | 11 | 28 | 28 |
| Races with 1 % or more of a region | 8 | 20 | 24 |
| Races with 1 % or more within 12 km of a city Prospect | 4 | 15 | 22 |
| Races with 1 % or more within 12 km of a town Prospect | 4 | 14 | 23 |

## What was asked

- A region has at most five races, a city at most four and a town at most three.
- The top races follow the settings of today. The last one is picked with the
  under-represented races boosted, so that every race is present somewhere and can be met.
- Test whether all races are present in a cluster of 30 regions. If the rule is too tight,
  allow one more race in a region, a city and a town.

## Result of the test

A cluster is 30 regions: every position of a 6 × 5 block (572 clusters) and every disc of the
30 regions nearest a region (432 clusters). Each rule was run with three different hash
salts, which gives the ranges. "In settlements" counts a race only when a city or a town on
show holds it.

| Rule | 6 × 5 blocks with all 28 races | Discs with all 28 races | Discs with all 28 in settlements | Fewest races in a cluster |
|---|---|---|---|---|
| 5 / 4 / 3, the five largest, no boost | 0 % | 0 % | – | 9 (median 10) |
| 5 / 4 / 3, four largest + one boosted | 53–57 % | 23–24 % | 16–17 % | 23 |
| 6 / 5 / 4, five largest + one boosted | 54–61 % | 25–28 % | 20–22 % | 24 |
| 5 / 4 / 3, three largest + two boosted | 96–98 % | 91–97 % | 77–90 % | 25 |
| **6 / 5 / 4, four largest + two boosted** | **96–98 %** | **91–97 %** | **89–94 %** | **26** |

What it shows:

- **The rule as asked is too tight.** With one boosted place a cluster has 30 boosted places
  for the 19 races that are almost never among the four largest of a region. A perfect
  assignment exists in each of the 36 blocks that were checked, but it needs all 30 regions to
  agree, and a region must be generated without looking at its neighbours.
- **One more race only helps when it is a second boosted place.** A fifth largest race adds
  nothing: the largest races are the same nine nearly everywhere.
- **With two boosted places a cluster holds all 28 races in 19 of 20 cases**, and 27 in nearly
  all the others. The race that is missing is most often the Shoalfolk, who only live on
  coasts below 80 m.

So the plan takes the fallback: **six races in a region, five in a city, four in a town**, and
in a region the last two places are boosted.

## Decisions

1. A region has up to 6 races: its 4 **main races** and 2 **minority races**. A city has up
   to 5 of the races of its region and a town up to 4. All four numbers are parameters.
2. The main races are the largest by the plain shares of today (frequency in the biome ×
   height factor). Nothing in the frequencies or the height modifiers changes.
3. A minority place goes to the race whose **turn** it is in the region, if the race can
   live there. The turns follow a fixed pattern over the region grid, so that every race
   has a turn in every cluster of regions. This is the boost: a race gets the place by its
   turn, not by its size.
4. When the race whose turn it is cannot take the place (it is a main race already, or the
   land would feed fewer than 10 of it), the place is drawn among the other races by their
   plain persons.
5. The persons of a region are shared between its races only. A minority race is lifted
   towards 5 % of the region, so that it is large enough to be met.
6. A patch where none of the region's races lives holds nobody. This is the rule of section
   3.2 today, applied to the races of the region.
7. The races of a city or a town are worked out for every Prospect in the Planned stage.
   They need only the region itself, so the stage rules do not change.
8. The last place of a city or a town is drawn with the minority races of the region
   weighted up 5 times.
9. The generator version stays 1 during phase 1.

"Main race" and "minority race" are new names. The readme decides the names, so they are
proposals until it is changed.

## The new rules

### Parameters (in `Planning.Population`)

| Parameter | Default | Meaning |
|---|---|---|
| `RacesPerRegion` | 6 | The most races a region holds |
| `MinorityRacesPerRegion` | 2 | How many of them are minority places |
| `RacesPerCity` | 5 | The most races a city holds |
| `RacesPerTown` | 4 | The most races a town holds |
| `MinorityMinPersons` | 10 | The plain persons a race needs in a region to take a minority place |
| `MinorityShare` | 0.05 | The share of the region a minority race is lifted towards. 0 turns the lift off |
| `MinorityBoost` | 5 | The factor on a minority race in the draw for the last place of a city or a town |
| `HinterlandShare` | 0.02 | How much of the whole region a city or a town draws on, beside its own patches |

`RacesPerRegion` 28 and `MinorityRacesPerRegion` 0 give the potential population of today.

### The races of a region (replaces society-generation.md 3.3)

```
plain(r) = Σ capacity(patch) · share(r, patch)        # sections 3.1 and 3.2, every race that is on

M      = the number of races that are on
b      = MinorityRacesPerRegion
order  = the races that are on, sorted by hash4(seed, 0x4a01, k, 0), then by k   # k: index among the races that are on
offset = hash4(seed, 0x4a01, −1, 0) mod M
W      = the number in 1 … M−1 whose turn pattern is the most even (below); 6 for 28 races

main races:
    the RacesPerRegion − b races with the largest plain(r) > 0; ties in the order of the Parameter Set

minority place z = 0 … b−1 of region (i, j):
    eligible = the races that are on, are not yet a race of the region, and have plain(r) ≥ MinorityMinPersons
    turn     = order[(i + W·j + offset + z·⌊M / b⌋) mod M]
    if turn is eligible:     turn takes the place
    else if any is eligible: one is drawn, each with the weight plain(r)
    else:                    the place stays empty
```

- **The turn pattern.** The regions where one race has its turn form a lattice: the regions
  with the same `(i + W·j) mod M`. `W` is chosen so that the shortest step between two such
  regions is as long as possible, and the lowest `W` wins a tie. With 28 races `W` is 6 and
  the steps are (−2, 5) and (4, 4) regions, about 5.5 regions or 700 km. Every 6 × 5 block of
  regions then holds a turn for every race in each minority place.
- **The draw** uses one sfc32 generator per region, seeded with `hash4(seed, 0x4a02, i, j)`.
  It takes one number per minority place, in the order of the places, also for a place that
  is taken by turn. The races are walked in the order of the Parameter Set.
- The order and the offset come from the seed, so two worlds do not share a pattern.

### The potential population (new 3.4)

```
boost(r) = 1                                           for a main race
s(r)     = the share of r in the region when every boost is 1
boost(r) = max(1, t·(1 − s) / (s·(1 − t)))             for a minority race, t = MinorityShare

weight(r, patch) = frequency(r, biome) · heightFactor(r, height) · boost(r)   over the races of the region
share(r, patch)  = weight(r, patch) / Σ weight(q, patch)
population(r)    = round(Σ capacity(patch) · share(r, patch))
```

- A race that is not a race of the region has 0.
- If the sum is 0 on a patch, none of the region's races lives there and the patch holds
  nobody.
- The lift is exact for a race that is spread evenly. A race that lives on a few patches
  takes those patches and stays below the share: the Shoalfolk hold the low coast of a
  region, not 5 % of all of it.

### The races of a city or a town (new, in section 5)

For every Prospect of the region:

```
local(r)  = Σ capacity(patch) · share(r, patch)   over the Prospect's patch and its neighbouring land patches of the region
weight(r) = local(r) + HinterlandShare · population(r)
n         = RacesPerCity for a Prospect for a city, RacesPerTown for a Prospect for a town

main races: the n − 1 races of the region with the largest weight(r) > 0; ties in the order of the Parameter Set
last race:  one is drawn among the other races of the region with weight(r) > 0, each with
            weight(r) · (MinorityBoost if r is a minority race of the region, else 1)
```

- The draw uses `toUnit(hash4(seed, 0x4a03, I, J))` of the root cell of the Prospect's patch.
- A city or a town never holds a race that its region does not hold.

## Measured after the implementation

The generator on the same 1,079 regions, with the default Parameter Set:

| | Prototype | Generator |
|---|---|---|
| Races in a region / a city / a town | 6 / 5 / 4 | 6 / 5 / 4 in every one |
| 6 × 5 blocks with all 28 races | 96–98 % | 99.3 % |
| Discs with all 28 races | 91–97 % | 96.1 % (27 races in 3.7 %, fewest 26) |
| Discs with all 28 races in cities and towns on show | 89–94 % | 90.5 % (fewest 25) |
| Minority places taken by turn | 84 % | 83 % |
| Share of a minority race, lowest tenth / median | 3.5 % / 5.0 % | 3.5 % / 4.9 % |
| A minority race is in a city or a town on show of its region | 97.5 % | 97.1 % |
| Persons, of the capacity of the land | 99.1 % | 99.1 % |
| Human / classic / new races, share of all persons | 83 % / 11 % / 6 % | 83.5 % / 10.4 % / 6.1 % |

## Expected result

Prototype of the rules on the same 1,079 regions. The Prospects on show stand in for the
cities and towns: 3,077 cities and 10,857 towns.

| | Today | New |
|---|---|---|
| Races in a region | 28 (median) | 6 in every region |
| Races in a city / in a town | no rule | 5 / 4 in every one |
| Share of the largest race of a region, median | 15 % (races-basis.md, 50 regions) | 27 % (24–32 % for 8 regions in 10) |
| Share of the smallest main race, lowest tenth | – | 15 % |
| Share of a minority race before the lift, lowest tenth / median | – | 1.1 % / 4.6 % |
| Share of a minority race after the lift, lowest tenth / median | – | 3.5 % / 5.0 % |
| Minority places taken by turn / by draw / empty | – | 84 % / 16 % / 0 |
| A minority race is in a city or a town of its region | – | 97.5 % of the cases, in half of the settlements (median) |
| Persons of all regions | 41.1 million | 40.8 million (99.1 %) |
| Capacity on patches that hold nobody, mean / worst region | 0 | 0.9 % / 14 % |

Where the races end up:

| | Persons today | Persons new | Regions that hold the race |
|---|---|---|---|
| Human races (7) | 59 % | 83 % | Canal folk 90 %, Midlanders 80 %, Saltroad folk 64 %, Sunlanders 63 %, Rainkeepers 45 %, Northmen 33 %, Grass riders 19 % |
| Classic races (5) | 20 % | 11 % | Orcs 36 %, Halflings 19 %, Elves 11 %, Gnomes 9 %, Dwarves 8 % |
| New races (16) | 21 % | 6 % | 6 to 9 % each |

Three things follow from the cap and should be weighed before the change is made:

- **Elves and Dwarves are in one region in ten.** Today they are in every region. The
  same holds for every race that is never among the four largest.
- **The human races gain.** The main races are human in most regions, and the minority
  races share about a tenth of a region. `MinorityShare` moves it: 0.10 gives 80 % human,
  11 % classic and 9 % new, and 0.15 gives 76 %, 11.5 % and 12.5 %.
- **The turn pattern is regular.** A race has its turns 700 km apart on a lattice. The
  failed turns, the draws and the order from the seed hide it, but it is a pattern.

The numbers come from a prototype outside the repository (a dump of the per-patch shares of
each region, and the rules in a script with its own hash). The implemented rule will give
slightly different numbers. The local weight was the Prospect's patch and its neighbouring
land patches of the region.

Weighed and dropped:

- **The largest five, no boost:** a cluster holds 10 races, and 9 races are in no region
  at all.
- **One boosted place, drawn at random** (weight: share, or share over how common the
  race is): a cluster holds 23 races and never all 28. With two such places it holds 27, and
  all 28 in 1 cluster in 6.
- **The race that is most over-represented in the region** (share over how common the race
  is, no draw): neighbouring regions pick the same race. A cluster holds 19 races.
- **Turns from a hash order inside 7 × 4 blocks of regions** instead of the lattice: all 28
  races in 18 of 36 separate 6 × 5 blocks with two places, against 33 of 36.
- **Looking at what the neighbouring regions picked:** it would give a perfect spread, but
  the Planned stage would need the neighbours Planned, and the result would depend on the
  order the regions are generated in.
- **A floor as a share of the region** (0.1 to 0.3 %) instead of 10 persons: the Shoalfolk
  and the Hoarkin fail it in most regions, and all 28 races are in a third of the clusters
  at 0.3 %.
- **Handing a patch that holds nobody to the races of the region:** it would put Midlanders
  on an ice cap. 0.9 % of the persons are lost instead.
- **Five races with three main and two minority:** it measures the same per region, but in
  the settlements all 28 races are in 77–90 % of the discs against 89–94 %. It is the choice
  if five must hold: set `RacesPerRegion` 5, `RacesPerCity` 4 and `RacesPerTown` 3.

## Steps

All ten are done.

1. `readme.md`: Race, Potential population and City and town say that a region holds its
   main races and its minority races, and a city or a town some of them. Add the two names.
2. `docs/society-generation.md`: section 2, take "a presence per race" out of the open
   list; section 3.3 becomes the rule above, 3.4 the potential population; section 5 gets
   the races of a city or a town; the measured numbers at the end of section 3 are replaced.
3. `src/Gameworld.Domain/ParameterSet.cs`: the eight parameters on `PopulationParameters`,
   each with a `[Description]` and a `[Range]`. Add them to
   `docs/default-parameter-set.json`. Regenerate `docs/parameter-set.schema.json`.
4. `src/Gameworld.Domain/Region.cs`: `PotentialPopulation` keeps one entry per race that is
   on, and each entry gets the standing of the race in the region: main, minority or
   absent. `RegionPlan` gets the races of each Prospect, in the order of the Prospects.
5. `src/Gameworld.Generation/Races/PopulationRules.cs`: the turn pattern (order, offset, `W`,
   worked out once per world), the races of a region, the boosted shares and the races of a
   Prospect. `Shares` and `Capacity` stay as they are.
6. `src/Gameworld.Generation/Stages/PlanBuilder.cs`: call the new rules, with the neighbours
   it already has for the Prospect scores.
7. `src/Gameworld.Demo`: the inspect panel shows the races of the region with the minority
   races marked, and the races of a Prospect.
8. `tests/Gameworld.Tests/PopulationTests.cs`: the two tests on a Planned region change
   (a region holds at most 6 races, and turning races off no longer keeps the total within
   28 persons). New tests: the caps, the turn pattern (every 6 × 5 block has a turn for
   every race), the lift, a city or a town only holds races of its region, and the 30
   regions around (0, 0) of `cellgen` hold at least 27 of the 28 races.
9. `docs/map-generation-research.md` section 10.7: the design rows for the caps and the
   turn pattern, with what was weighed against them. The "presence field" row changes from
   Open.
10. `docs/races-basis.md`, "Measured shares": measure again and rewrite what the numbers
    show. `docs/adding-a-race.md`: a new race changes the turn pattern of every region.

## Done when

All four hold. The second and the third were checked on the four seeds.

- `dotnet test -c Release` passes, with the new tests.
- On the 18 × 15 regions of `cellgen` and `gameworld`: no region holds more than 6 races,
  no city more than 5 and no town more than 4.
- On the same regions, at least 90 % of the discs of 30 regions hold all 28 races, and no
  disc holds fewer than 26.
- The demo app shows the races of a region and of a Prospect.
