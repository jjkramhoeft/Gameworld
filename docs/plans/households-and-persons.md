# Plan: households and persons of a region

Status: implemented 2026-11-21. The rules are in
[society-generation.md](../society-generation.md) sections 6 (households) and 7 (persons),
and the readme has the concepts under "Household", "Family and house" and "Person". This
document keeps what was asked, what a region knows when its households are made, the
decisions, the measurement and what was weighed. Where the two differ,
society-generation.md holds. One thing differs from the plan as it was approved: the order
in which a faction looks for its households (decision 5). After the first measurement the
user asked for five times the persons in each region (decision 10), and the numbers below
are measured at that density.

## What was asked

- Generate people for each region, in stages like the rest of the generation.
- Households first, to keep family relations consistent: the households of the factions,
  then the plain households of the cities, then those of the towns.
- For now a household is a family plus servants. A household shares wealth and faction.
- Households are located in cities, towns and villages. Villages are skipped for now.
- A faction always has at least two households.
- Then the persons of each household: sex, age, vocation and family relationship.
- A last stage, the content of a person (images and history), is skipped for now.

## What exists

- **The factions** of a region (society-generation.md section 4, Planned stage): one
  ruling faction, sometimes an opposition, up to four lesser factions. A faction has a
  type, a lead race and a seat, and no members.
- **The cities and towns** (section 5.2, Settled stage): one on every Prospect on show,
  each with a number of persons per race. The persons are counts.
- **The person-name generator** (section 7): a given name by sex and a surname, in the
  name style of a race. It gave the surname of the house that rules a region only.
- **The vocations** (section 8): a list of 51 in the Parameter Set, each with a sector, a
  lowest tier and a share. Nothing drew one.
- Sections 6 (Families) and 7 (Persons) were "not specified". The readme had the concept
  Family: "A family of NPCs, typically living together in a household. It can include
  servants or slaves."

## What a region knows when its households are made

Measured on 676 regions: the 13 × 13 regions around (0, 0) of the seeds `cellgen`,
`atlas`, `ember` and `tide`, default Parameter Set. 674 of them have a city or a town.

| | Lowest | 5 % | 25 % | Median | 75 % | 95 % | Highest |
|---|---|---|---|---|---|---|---|
| Persons of a region | 0 | 552 | 1,236 | 1,781 | 2,476 | 3,521 | 5,939 |
| Persons in the cities and towns of a region | 0 | 102 | 248 | 356 | 496 | 704 | 1,188 |
| Persons of a capital | 4 | 39 | 73 | 100 | 126 | 169 | 285 |
| Persons of another city | 10 | 18 | 29 | 39 | 52 | 75 | 143 |
| Persons of a town | 3 | 8 | 12 | 17 | 24 | 38 | 100 |

| Fact | Value |
|---|---|
| Cities and towns | 8,674, with 252,203 persons: 29 persons a place |
| Races with a person in a city or a town | 4.1 on average |
| A race with exactly one person in a city or a town | 4,018 cases |
| Factions | 1,934: 674 ruling, 278 in opposition, 982 lesser |

Two things follow.

- **The counts are small enough to generate every person.** The cities and towns of a
  region hold about 360 persons. No sample of "important persons" is needed: every person
  of a city or a town can be a Stub.
- **The counts are shared between several races.** A town holds 17 persons in the median,
  of four races. A family is of one race, so it can be no larger than the persons of its
  race in the place.

## Decisions

1. **Two new region generation stages, Housed and Peopled, after Settled.** The stages are
   linear, and each needs the region itself only at the stage before. The three steps that
   were asked for (factions, cities, towns) are three ordered steps of the Housed stage: a
   region is never left between them.
2. **The households of a city or a town hold exactly its persons, race by race.** The
   counts of the Settled stage stand. A household takes its persons from them, and the
   rule ends when none is left.
3. **Household replaces Family as the concept and the entity.** A household is a family
   and its servants. The family is the kin in a household, and a house is the households
   of one faction of the type Family. The parent of a household is its region, and the
   parent of a person is its household.
4. **A faction has two households where it can, and fewer where it must.** It has one or
   none when its region has no fitting person left in a city or a town. The count is a
   parameter. The households of the land can mend this when they come.
5. **The leaders of a faction are of its lead race before they sit at its seat.** A
   faction looks for a person of its lead race at its seat, then in the other cities and
   towns; only then for a person of any race, at the seat first. The plan as approved
   looked at the seat for any race first. That gives leaders of another race than the
   lead race, which the readme defines as the race of the leaders.
6. **Kin runs inside a household, and between the households of a house.** A house shares
   a surname and a race, and the head of a later household is a child or a sibling of the
   head of the first. The sex and the age of a head are in the Stub of the household, so
   the kin of a house is consistent before any person is made.
7. **The wealth of a household is a level of five, and it meets the vocation through a
   standing.** Every vocation has a standing, a range on the scale of six of
   [research/vocations.md](../research/vocations.md) section 2.4. The head of a household
   holds a vocation whose standing fits the wealth. The servants follow the wealth.
8. **The head of a household of a faction holds a vocation of the faction**: one the
   faction gathers, or the one of those who lead a faction of its type (a noble for a
   house, a priest for a faith).
9. **A person who is alone in its family is a man or a woman with even chance.** The male
   head chance holds for a family of two or more. With it for everybody, 63.5 % of all
   persons were men at the first density, where most households held one person.
10. **Five times the persons in each region.** The first measurement was made at 0.06
   persons per km²: a city or a town held 5.8 persons of 3.1 races, 58 % of the households
   held one person and the mean household 1.7. The user then asked for five times the
   persons. The density is 0.3 since 2026-11-21, and the seven parameters that are counted
   in persons were raised five times with it (society-generation.md section 3.4), so the
   races, the factions and the names of a region stayed as they were. Each of the 674
   regions with persons was compared with itself at the first density: before the
   rounding to whole persons every race of every region holds exactly five times what it
   held, and in whole persons a region is within 10 persons of five times its first count.

## The rule

Sections 6 and 7 of society-generation.md hold the rule. In short:

- **Housed.** The pool of a city or a town is its persons per race. The heads of the
  households of the factions are taken first, one person each, in four passes (decision
  5), every faction getting its first head before any gets a second. Those households are
  then filled: a wealth, a family as large as its race has persons left, servants from
  what the place has left. Then the other households, city by city and town by town by
  rank, until the pool is empty.
- **Peopled.** One stream per household. The head is that of the Stub. A husband or a
  wife comes by chance, then children, parents and siblings of the head with ages that
  fit, then the servants. Each person gets a name, and from the working age a vocation.

| Parameter | In the Parameter Set | Default |
|---|---|---|
| Households per faction | `Housing.FactionHouseholds` | 2 |
| Family size weights, 1 to 8 persons | `Housing.FamilySizeWeights` | 2, 3, 3, 3, 2, 1, 0.5, 0.25 |
| Wealth weights in a town | `Housing.TownWealth` | 1, 3, 4, 2, 0.3 |
| Wealth weights in a city | `Housing.CityWealth` | 1.5, 3, 3.5, 2.5, 0.8 |
| Wealth weights of a household of a faction | `Housing.FactionWealth` | 0, 0, 1, 3, 2 |
| Servants by wealth | `Housing.Servants` | 0, 0, 0, 0 to 1, 1 to 3 |
| Head age | `Housing.HeadAge` | 20 to 65 |
| Male head chance | `Housing.MaleHeadChance` | 0.8 |
| Spouse chance | `People.SpouseChance` | 0.75 |
| Working age | `People.WorkingAge` | 14 |
| Land work factor | `People.LandWorkFactor` | 0.1 |
| Same vocation chance | `People.SameVocationChance` | 0.6 |
| Servant vocation | `People.ServantVocation` | `servant` |
| Standing of a vocation | `Vocations[].Standing` | from research/vocations.md section 3 |

The hash purposes are 0x4d01 to 0x4d03 for the households and 0x4e01 and 0x4e02 for the
persons.

## Measured

On the same 676 regions, with the defaults: 85,775 households and 252,203 persons.

| | Lowest | 5 % | 25 % | Median | 75 % | 95 % | Highest |
|---|---|---|---|---|---|---|---|
| Households of a region | 0 | 37 | 87 | 124 | 166 | 227 | 356 |
| Households of a capital | 2 | 12 | 21 | 28 | 36 | 46 | 75 |
| Households of another city | 3 | 7 | 10 | 13 | 17 | 23 | 42 |
| Households of a town | 1 | 4 | 5 | 7 | 9 | 13 | 35 |

| Fact | At 0.3 persons per km² | At 0.06, the first measurement |
|---|---|---|
| Households, persons in them | 85,775, 252,203 | 29,512, 50,495 |
| Factions with two households, with one, with none | 99.8 %, 0.1 %, 0.1 % | 97.5 %, 1.8 %, 0.7 % |
| The first household of a faction is at its seat, is of its lead race | 99.1 %, 99.8 % | 92.7 %, 97.8 % |
| The second household is at the seat, is of the lead race | 95.5 %, 99.5 % | 74.4 %, 94.9 % |
| Households of a faction, of all households, of all persons | 4.5 %, 6.1 % | 12.9 %, 20.5 % |
| Cities and towns where every household is of a faction | 0.0 % | 1.4 % |
| Houses with two households | 393 of 393 | 390 of 393 |
| The head of the branch of a house is a child, a sibling | 125, 268 | 125, 265 |
| Persons in a household, on average | 2.94 | 1.71 |
| Households of one, two, three, four, five or more persons | 22.2 %, 24.6 %, 20.2 %, 15.8 %, 17.2 % | 58.2 %, 25.1 %, 9.2 %, 4.2 %, 3.1 % |
| Persons in a family, on average | 2.72 | 1.48 |
| A race with one person in a place, of all households | 4.7 % | 55.1 % |
| Wealth of a household of no faction, Destitute to Rich | 11.3 %, 28.0 %, 35.4 %, 20.6 %, 4.8 % | 10.9 %, 28.1 %, 36.1 %, 20.5 %, 4.4 % |
| Head, spouse, child, parent, sibling, servant, of the persons | 34.0 %, 18.9 %, 29.8 %, 4.9 %, 5.0 %, 7.4 % | 58.4 %, 14.1 %, 10.5 %, 1.7 %, 1.8 %, 13.4 % |
| Men | 51.9 % | 51.7 % |
| Under 14 | 17.7 % | 6.9 % |
| The most held vocations, of the persons with one | Servant 18.7 %, Labourer 13.6 %, Beggar 7.0 %, Weaver 6.4 %, Farmer 6.1 %, Priest 5.2 % | Servant 22.3 %, Labourer 11.6 %, Beggar 5.9 %, Weaver 5.6 %, Farmer 5.2 %, Priest 5.2 % |

Three things follow.

- **"At least two households" holds for all but four factions in 1,934.** The four are of
  the type Race, and their race has no person, or one, in the cities and towns of the
  region. At the first density 49 factions were short, 45 of them of the type Race.
- **The density decides the size of the households.** At the first density a race had
  exactly one person in a place 16,271 times, and each was a household of one: 55 % of
  all households. With five times the persons it is 4.7 %. The family size weights give a
  mean family of 3.4 persons where they are free; the measured mean is 2.7, because the
  last family of a race in a place is cut to what is left.
- **There are children and family links now.** Three persons in ten are a child of a head
  and 18 % are under 14, for one in ten and 7 % at the first density.

## Weighed and dropped

- **One region stage for each step: four new stages.** It follows the request to the
  letter. Dropped: a region would have three stages that no reader can tell apart, and the
  three steps share one pool.
- **Filling the Settled stage.** The readme had families and important persons there.
  Dropped: the travel network of Settled needs the bordering regions, the households do
  not, and a region should be able to stop before its persons are made.
- **Households that set the count of their place.** Draw the households freely and let the
  persons of the city or town be their sum. It gives the family sizes of the weights.
  Dropped: it undoes the measured shares of section 5.2, a tenth of the persons in cities
  and a tenth in towns.
- **Only notable households.** Generate the households of the factions and a few more, and
  leave the rest a count. Dropped: a few hundred persons a region are few enough to make
  all.
- **Persons added for a faction that finds no room**, taken from the land, or households
  without a city or town. Both keep "at least two" to the letter. Dropped: the first
  breaks the counts of the place, the second puts a household where nothing can show it
  until the land has places.
- **The seat before the lead race.** The plan as approved. Dropped: see decision 5.
- **The highest standing of a sector for the leaders of a faction.** No vocation is named
  in the rule then. Dropped: the alchemist and the mage both reach standing 5, and by
  share an alchemist (0.05) would lead a magocracy more often than a mage (0.03). The rule
  names the six vocations instead and falls back when one is off.
- **A variant of the vocation for every person.** Dropped: a variant has no condition yet
  (section 8), so an even draw makes a man a "lady" and a servant of twelve a "household
  steward".
- **Wealth as an amount.** Dropped: nothing reads an amount yet, and a level is enough to
  pick a vocation and a number of servants.
- **Kin between any two households of a region.** Dropped for now: it needs a rule for
  who married out and when.

## Open

1. The households of the land, and villages: the request names villages, and
   society-generation.md section 8 says there are none. To be settled first.
2. The person who is alone of its race in a place, and the family that is cut to what its
   race has left: fewer races in a small place, or mixed households.
3. Who but its leaders belongs to a faction.
4. What the wealth of a household reads beside its place and its faction.
5. The share by sex, the factors of the races and the variant of a vocation (section 8).
6. The lifespans of the races: races-basis.md has one for each, the rule has none.
7. The content of a person: the description, the history and the portrait.

## What changed

1. `src/Gameworld.Domain`: the stages Housed and Peopled (`Stages.cs`); `Household`,
   `Person`, `WealthLevel`, `Sex`, `HouseholdRole` (`Households.cs`); the two stages on
   `Region` and `RegionSnapshot`; `EntityKind.Household`; `Standing` on a vocation; the
   parameter groups `Housing` and `People`.
2. `src/Gameworld.Generation`: `Households/HouseholdRules.cs`, `People/PersonRules.cs`,
   `Stages/HouseholdBuilder.cs`, and the two stages in `World`.
3. `src/Gameworld.MapGenerator`: the two stages, and households and persons in the table.
   `src/Gameworld.Demo`: the households and persons of the city or town of a patch.
4. `tests/Gameworld.Tests`: `HouseholdTests`, `PersonTests`, and the stage, round-trip and
   region-order tests.
5. The readme, society-generation.md, the default Parameter Set and its schema, CLAUDE.md.

## Done when

- A region advances to Housed and to Peopled, and comes back from its stored form.
- The households of every city and town hold exactly its persons, race by race.
- A faction has two households where its region has the persons, and a house shares a
  surname.
- Every person has a name, a sex, an age, a role and, from the working age, a vocation
  where the place has one, and the ages of parents and children fit.
- The same region gives the same households and persons whatever was generated before.
