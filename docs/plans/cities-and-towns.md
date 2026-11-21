# Plan: from Prospects to cities and towns

Status: implemented 2026-11-20, as planned. The rule is in
[society-generation.md](../society-generation.md) section 5.2, and the readme has the
concept under "City and town". This document keeps what was asked, what a region knows when
its cities and towns are made, the decisions, the measurement and what was weighed. Where
the two differ, society-generation.md holds.

The numbers in persons below are those of the density of the time, 0.06 persons per km².
The default is 0.3 since 2026-11-21, so a region, a city and a town hold five times the
persons, and section 5.2 has the numbers measured since.

## What was asked

- Plan the step from the Prospects of a region to its final cities and smaller urban
  communities.
- Two parameters govern how much of the whole population lives in cities and in the
  smaller urban communities. The defaults aim at about 10 % in cities and about 10 % in
  the smaller communities.
- Implement the plan if it is feasible.

The smaller urban community is the **town** of the readme. No new name was needed.

## What exists

- **Prospects** (map-generation.md section 15, Planned stage). A region has up to 20: up
  to 4 for cities and the rest for towns. The sum of their scores decides how many are on
  show. Only those are drawn on a map and connected by the travel network.
- **The potential population** (society-generation.md section 3, Planned stage): how many
  persons the land of the region feeds, for each of its races.
- **The races of a city or a town** on each Prospect (section 5.1, Planned stage): a list
  of up to five races for a city and four for a town, with a weight per race.
- **The factions** (section 4, Planned stage). The seat of each is a Prospect on show, and
  the seat of the ruling faction is the capital of the region.
- **The Settled stage** held the travel network only. The readme places "actual cities
  and towns" there.

Section 5 of society-generation.md listed three things as open: which Prospects become
cities and towns, how the potential population becomes the persons of the cities, the
towns and the land, and what a city holds as a Stub and what its identity is.

## What a region knows when its cities and towns are made

Measured on 676 regions: the 13 × 13 regions around (0, 0) of the seeds `cellgen`,
`atlas`, `ember` and `tide`, default Parameter Set. 674 of them have persons.

| | Lowest | 5 % | 25 % | Median | 75 % | 95 % | Highest |
|---|---|---|---|---|---|---|---|
| Persons (potential population) | 0 | 107 | 247 | 356 | 496 | 704 | 1,188 |

| Fact | Value |
|---|---|
| Prospects on show per region | 12.8 on average |
| Prospects for cities on show | none in 18 regions, 1 in 61, 2 in 171, 3 in 191, 4 in 235 |
| Regions without a Prospect for a city on show | 2.7 %, with 0.5 % of all persons |
| Regions without a Prospect for a town on show | 1.9 %, with 0.3 % of all persons |
| The capital is on a Prospect for a town | 141 of 674 regions (21 %), with 15 % of all persons. 125 of them also have a Prospect for a city on show |
| The capital is the best Prospect on show by score | 630 of 674 |
| A tenth of the region is fewer persons than it has cities, or than it has towns | 1 region |

Two things follow.

- **The region is small in persons.** The default density is 0.06 persons per km², so a
  region has a few hundred persons. A tenth of it is about 36 persons, shared by 3 cities
  or by 10 towns. The rule must work in whole persons, and it must not lose or invent a
  person when it rounds.
- **The best place of a region is often a Prospect for a town.** A Prospect for a city
  must lie two patches inside the region, a Prospect for a town one patch. The best place
  by score is one patch inside in a fifth of the regions, and the ruling faction sits on
  it.

## Decisions

1. **The cities and towns are made in the Settled stage**, as the readme says. They read
   the region's own Planned data only, so the rule is a pure function of one region and
   needs nothing from the bordering regions.
2. **A city or a town stands on every Prospect on show, and on no other.** The Prospects
   on show are what a map draws, what the travel network connects and where the factions
   sit. A place the player can travel to is a place where somebody lives.
3. **A Prospect for a city gets a city and a Prospect for a town a town. The capital is
   always a city.** Without the exception a fifth of the regions would have a town as
   their capital, smaller than their cities, on the best place they have. The races of a
   capital are still those of its Prospect.
4. **Two shares, per region.** `CityShare` and `TownShare` are shares of the persons of a
   region: a tenth each by default. Every region with a city gives its cities the city
   share, and every region with a town gives its towns the town share. A region without a
   city, or without a town, keeps that share on the land.
5. **The cities and towns of a region are ranked together, and a place weighs one over its
   rank.** The capital is first, then the other cities, then the towns, each in the order
   of the Prospects, which is by score. The cities share the city persons by these weights
   and the towns the town persons. This is the rank-size rule of research/settlement-siting.md
   (§2.6 and §6.7), with the two shares pinned. A third parameter, `RankExponent`, sets
   how unequal the places are. 1 is the rank-size rule and 0 gives equal sizes.
6. **Whole persons by largest remainder.** A pool of persons is shared by quota: the whole
   part first, then one more to the largest remainders. The sum is always the pool.
7. **No city or town stands empty** where the region has a person for it. A pool is at
   least one person per place, and a place the rounding left empty takes a person from
   the largest of its kind.
8. **The persons of a place are shared between the races of its Prospect** by the weights
   of section 5.1, again by largest remainder. A race is never placed beyond the persons
   it has in the region, so the persons on the land are never negative.
9. **The land keeps the rest.** For every race, the persons of the region are those of
   its cities and towns plus those on the land.
10. **The identity of a city or a town is its region and a slot**, as for a faction. The
    slot is the index of its Prospect among the Prospects of the region, so a Prospect
    that is not on show keeps its slot free.
11. **The generator version stays 1** during phase 1.

## The rule

The full text is society-generation.md section 5.2. In short:

```
capital = the Prospect on show that is the seat of the ruling faction
city    = the capital, and every other Prospect on show that is for a city
town    = every other Prospect on show

rank    = 1, 2, 3 … over the capital, the other cities, then the towns
weight  = 1 / rank^exponent

P            = the persons of the region
cityPersons  = max(round(city share · P), the number of cities)      # 0 without a city
townPersons  = max(round(town share · P), the number of towns)       # 0 without a town

the cities share cityPersons by weight, the towns townPersons: largest remainder
the persons of a place are shared between the races of its Prospect by their weight (5.1),
no race beyond the persons it has left in the region
rural(r) = the persons of race r that are in no city and no town
```

| Parameter | In the Parameter Set | Default |
|---|---|---|
| City share | `Settlement.CityShare` | 0.1 |
| Town share | `Settlement.TownShare` | 0.1 |
| Rank exponent | `Settlement.RankExponent` | 1 |

## Measured

The same 676 regions, 252,980 persons, with the rule as implemented.

| | City share 0.1, town share 0.1 | City share 0.05, town share 0.2 |
|---|---|---|
| Persons in cities | 10.01 % | 5.00 % |
| Persons in towns | 9.95 % | 19.87 % |
| Persons on the land | 80.04 % | 75.13 % |
| Cities, towns per region | 3.05, 9.82 | the same |
| Cities and towns without persons | 0 | 0 |

The towns fall a little short of their share because 24 regions have no town. No region
is without a city, since the capital is one.

With the defaults:

| | Lowest | 5 % | 25 % | Median | 75 % | 95 % | Highest |
|---|---|---|---|---|---|---|---|
| Share of a region in its cities | 9.1 % | 9.8 % | 9.9 % | 10.0 % | 10.1 % | 10.3 % | 14.3 % |
| Share of a region in its towns | 0 % | 9.8 % | 9.9 % | 10.0 % | 10.1 % | 10.2 % | 10.7 % |
| Persons of the capital | 1 | 8 | 15 | 20 | 25 | 34 | 57 |
| Share of the region in its capital | 4.3 % | 4.7 % | 4.8 % | 5.4 % | 6.6 % | 10.0 % | 14.3 % |
| Persons of another city | 2 | 4 | 6 | 8 | 10 | 15 | 29 |
| Persons of a town | 1 | 2 | 2 | 3 | 5 | 8 | 20 |

- The region with 14.3 % has 7 persons and one city, which holds one of them.
- No pool is lifted by "one person per place" at the defaults.
- Every person is in a city, in a town or on the land, race by race, in all 676 regions.
  No race is placed beyond its persons, and no place gets a race from outside its list.
- A quarter of the listed races of all places hold nobody (9,372 of 36,612): a town of
  3 persons cannot hold four races.
- A minority race has a person in a city or a town of its region in 80 % of the cases
  (1,072 of 1,348). The median region has persons of all six of its races in its cities
  and towns.
- In 55 % of the regions the largest town is larger than the smallest city, mostly by a
  person or two. With equal shares about ten towns hold as many persons as three cities,
  so the first town comes out near the last city. A city is a city by its Prospect, not by
  its head count. Raise the city share or lower the town share to pull them apart.

## Weighed and dropped

- **A capital that stays a town** when its Prospect is for a town. It is the simplest
  rule, and the kind of a place would follow its Prospect without exception. Dropped: in
  21 % of the regions the capital would be a town on the best place of the region. It
  would share the town persons with about nine other towns, while lesser places held the
  city persons.
- **Moving the capital to a Prospect for a city.** It would change the seat rule of the
  factions (section 4.4), which is adopted and measured.
- **One share for all urban persons, split by rank alone.** By the rank-size rule the
  three cities of a region would hold about 1.4 times what its ten towns hold
  (1 + 1/2 + 1/3 against 1/4 + … + 1/13). The two shares could then not be set apart,
  which is what was asked for.
- **A share of the world, not of the region:** the size of a place from its own land,
  scaled so that the world comes out at a tenth. The share of a region would then vary,
  and the result of a region would depend on a number measured over other regions. A
  share per region is exact, local and easy to test.
- **Weights from the score** of the Prospect. The scores of the Prospects on show lie
  close together: more than half are between 62 and 75 (map-generation.md section 15.6).
  The places of a region would come out nearly equal. The rank keeps the order of the
  scores and gives the sizes a spread.
- **A person to every listed race before a second to any.** A town of 4 persons would
  then always be of four races. Sharing by weight gives a small town the one or two races
  that live around it, at the price that a listed race can hold nobody.
- **Random rounding.** It would be deterministic through the hash, but the sums would
  only be right on average. The largest remainder is exact and needs no draw.
- **A smallest city and a smallest town as parameters**, with fewer cities and towns
  where the persons do not reach. A Prospect on show would then sometimes be empty, though
  the roads lead to it and a faction can sit on it. The density of persons is the
  parameter for how large places are.

## Open

- The names of cities and towns.
- Whole rural persons per patch. The rule gives the persons on the land per race for the
  region, and a share per patch that is not rounded.
- Whether a Prospect that is not on show gets a city or a town later in play.
- Whether the share of a region should follow the region: more of a trading region in its
  cities, less of a herding one. The shares are the same everywhere now.
- The families, the vocations and the important persons of a city or a town (sections 6
  to 8 of society-generation.md).

## What changed

1. `src/Gameworld.Domain/ParameterSet.cs`: the group `Settlement` with `CityShare`,
   `TownShare` and `RankExponent`. `docs/default-parameter-set.json` and the generated
   `docs/parameter-set.schema.json` follow.
2. `src/Gameworld.Domain/Region.cs`: `Settlement`, `SettlementKind` and `RacePersons`.
   `RegionSettlement`, what the Settled stage adds, now holds the cities and towns and
   the persons on the land beside the travel network.
3. `src/Gameworld.Generation/Settlements/SettlementRules.cs`: the rule.
   `Stages/SettlementBuilder.cs` reads a Planned region for it, and `World` runs it when a
   region becomes Settled.
4. `src/Gameworld.Generation/Rendering`: the capital is drawn as a larger diamond with a
   light ring.
5. The map generation app prints the cities and towns of each region with their persons.
   The demo app shows the persons of a Settled region in its cities, in its towns and on
   the land, and the city or town on a clicked Prospect.
6. `tests/Gameworld.Tests/SettlementTests.cs`: the rule on a small made-up region, and on
   generated regions.
7. `readme.md`, `docs/society-generation.md` sections 1, 4.2 and 5, `docs/map-generation.md`
   sections 15.6 and 17, and `CLAUDE.md`.

## Done when

All of these hold.

- `dotnet test -c Release` passes, with the tests of the rule.
- Over the measured regions the persons in cities and in towns are each within half a
  percentage point of their share.
- Every person of every region is in a city, in a town or on the land, race by race.
- The stored form of a Settled region holds its cities and towns, and the result does not
  depend on the order the regions are generated in.
