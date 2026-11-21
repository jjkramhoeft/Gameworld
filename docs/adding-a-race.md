# Adding a race

The races are a list in the Parameter Set, not a fixed set in the code. So there are two
ways to add one, and they differ a lot in work:

- **To one world**: add an entry to the `Races` list of a Parameter Set file. No code
  changes. Use this to try a race out.
- **To the default list**: change the built-in defaults, so every world without its own
  Parameter Set file has the race. This touches code, tests and five docs.

The rules behind both are in [society-generation.md](society-generation.md) sections 2,
3 and 4, and the default list is its Appendix A. The term Race is defined in the
[readme](../readme.md).

A change to the list is a new Parameter Set, and with it a new foundation. Foundations
that exist are not touched.

## 1. Decide what the race is

A race is thirteen things:

| Field | What to decide |
|---|---|
| `Id` | The id generated data knows the race by, in camelCase like `frostFolk`. It must be unique in the list. Keep it when the name changes. |
| `Name` | The name shown for the race. |
| `Enabled` | Whether the race lives in the world. A race that is off gets no population. |
| `Human` | Optional. `true` for a human people. A region whose ruling faction it leads is then named from the word lists of the human races (society-generation.md section 4.10). Leave it out for any other race. |
| `NameStyle` | Optional. Which name model the names of the race come from: `Common` (British), `Exotic` (elven) or `Dark` (dark elven). Leave it out and the race has `Common`. |
| `BiomeFrequency` | How frequent the race is in each of the 17 biomes, relative to the other races. |
| `HeightModifier` | Optional. How the height of the land changes the frequency. Leave it out when the height does not matter to the race. |
| `FactionLeaning` | Optional. How much the race leans to each of the six types of faction, 0 to 3. Leave it out and the race leans to every type alike, with 1. |
| `AppreciatedRaces` | Optional. The ids of the other races the race appreciates. |
| `HatedRaces` | Optional. The ids of the other races the race hates. |
| `FavouriteColours` | Optional. The ids of the favourite colours of the race, the dearest first. |
| `Description` | Optional. What a person of the race looks like: build, skin, hair, eyes, dress and the marks that tell the race apart. A portrait is drawn from it, so write what a picture must show. Leave it out and the race has `**missing description**`. |
| `Lore` | Optional. Where the race comes from and what it tells about itself. Leave it out and the race has `**missing description**`. |

**The frequencies.** By the plain shares, the races that are on share a patch in proportion
to their frequency in its biome. Only the proportions between the races within one biome
matter. The plain shares decide the main races of a region and are the weights its races
share the land by (society-generation.md sections 3.2 to 3.4). The default list builds each frequency as

```
frequency(race, biome) = habitat(race, biome) · common(race)
```

- `habitat` is 0 avoided, 1 tolerated, 2 good or 3 native.
- `common` is how common the race is in the world: 3 to 5 for the human races, 2 for
  Elves, Halflings and Orcs, 2.3 for Dwarves, 1 for Gnomes, and 1 or 0.5 for the new races.

Follow the same scheme, so the new race compares to the others. A race with `common` 1
that is native to a biome gets 3 there, against 15 for Midlanders in Temperate grassland.

The biome keys of a Parameter Set file are: `IceAndSnow`, `Tundra`, `ColdSteppe`,
`BorealForest`, `ColdDesert`, `TemperateGrassland`, `WoodlandAndShrubland`,
`TemperateMixedForest`, `TemperateBroadleafForest`, `TemperateRainForest`, `HotDesert`,
`ThornScrub`, `Savanna`, `TropicalDryForest`, `TropicalRainForest`, `Mangrove`, `Volcano`.

**The height modifier.** A list of control points `(height in m, factor)`. The factor
multiplies the frequencies of the race. It runs in a straight line between two points and
is constant below the first and above the last. Two points at the same height make a step.

| Points | Meaning |
|---|---|
| `(200, 0), (200, 1)` | Only from 200 m and up |
| `(200, 1), (200, 0)` | Only below 200 m |
| `(100, 0), (300, 1)` | Fades in: none below 100 m, half weight at 200 m, full weight from 300 m |

**The heights of the land.** A height modifier acts on the mean height of a land patch,
so pick its heights from how the land is spread. Measured on 8,394 land patches of 50
regions (the 25 regions around region 0,0 of the seeds `cellgen` and `gameworld`) with the
default Parameter Set:

| Share of the land patches below | Height |
|---|---|
| 5 % | 23 m |
| 10 % | 37 m |
| 25 % | 77 m |
| 50 % (the median) | 155 m |
| 75 % | 270 m |
| 90 % | 432 m |
| 95 % | 629 m |
| 99 % | 1,084 m |
| 100 % (the highest patch) | 2,000 m |

| Height band | Share of the land patches | Share of the potential population | Persons per km² |
|---|---|---|---|
| 0 to 50 m | 15.0 % | 16.3 % | 0.155 |
| 50 to 100 m | 17.6 % | 18.1 % | 0.145 |
| 100 to 200 m | 29.2 % | 31.0 % | 0.145 |
| 200 to 300 m | 17.7 % | 17.8 % | 0.140 |
| 300 to 500 m | 12.8 % | 11.6 % | 0.125 |
| 500 to 750 m | 4.5 % | 3.4 % | 0.105 |
| 750 to 1,000 m | 1.9 % | 1.3 % | 0.100 |
| 1,000 to 1,500 m | 1.2 % | 0.5 % | 0.060 |
| 1,500 to 2,000 m | 0.2 % | 0.03 % | 0.025 |

| A race that only lives from | gets this share of the land patches | which feed this share of all persons |
|---|---|---|
| 100 m and up | 67 % | 66 % |
| 200 m and up | 38 % | 35 % |
| 300 m and up | 21 % | 17 % |
| 500 m and up | 8 % | 5 % |
| 1,000 m and up | 1.4 % | 0.5 % |

- The land is low. Half of it lies below 155 m and nine tenths below 432 m. A limit above
  500 m leaves a race with very little land.
- High land also feeds fewer persons per km², so a highland race gets less than its share
  of the land suggests.
- These are the heights of patches, the mean over 64 km² or more. Peaks in the height map
  are higher than the highest patch.
- The two seeds differ a little: 39 % of the patches lie at 200 m or higher in one and
  37 % in the other.

The biome and the height are not independent: cold biomes lie higher, deserts lower.

| Biome | Share of the land patches | Median height | 10 % below | 10 % above | At 200 m or higher |
|---|---|---|---|---|---|
| Ice & snow | 7.3 % | 183 m | 36 m | 651 m | 46 % |
| Tundra | 5.9 % | 263 m | 51 m | 821 m | 58 % |
| Cold steppe | 2.9 % | 131 m | 36 m | 308 m | 30 % |
| Boreal forest | 10.0 % | 182 m | 38 m | 472 m | 47 % |
| Cold desert | 9.0 % | 115 m | 28 m | 306 m | 25 % |
| Temperate grassland | 4.0 % | 145 m | 45 m | 495 m | 38 % |
| Woodland & shrubland | 6.6 % | 188 m | 51 m | 601 m | 47 % |
| Temperate mixed forest | 5.5 % | 184 m | 59 m | 529 m | 45 % |
| Temperate broadleaf forest | 6.1 % | 171 m | 37 m | 498 m | 42 % |
| Temperate rain forest | 7.1 % | 144 m | 25 m | 395 m | 37 % |
| Hot desert | 10.1 % | 114 m | 30 m | 287 m | 22 % |
| Thorn scrub | 5.0 % | 164 m | 50 m | 338 m | 40 % |
| Savanna | 6.4 % | 181 m | 48 m | 401 m | 42 % |
| Tropical dry forest | 7.6 % | 140 m | 36 m | 329 m | 31 % |
| Tropical rain forest | 4.1 % | 161 m | 35 m | 396 m | 38 % |
| Mangrove | 2.4 % | 119 m | 23 m | 308 m | 26 % |

Volcano is not in the table. It came after the measurement and is 0.07 % of the land:
single patches of mountains zones, about one in nine regions.

So a hot desert race that only lives from 200 m and up has about a fifth of the hot
desert, and a tundra race with the same limit keeps more than half of the tundra.

**The leaning.** One number from 0 to 3 for each type of faction: `Family`, `Race`,
`Vocation`, `Belief`, `Magic` and `Place`. 3 is the kind of group the race forms first, 2
one that fits it, 1 one it seldom forms and 0 one it never forms. The leaning weighs the
race when the type and the lead race of a faction are drawn
(society-generation.md section 4.4). A race that leans 3 to Race and 0 to Place is a people
under its chiefs wherever it leads, and never a free town.

**The appreciated and the hated races.** Two lists of ids of other races. To appreciate
is to trade with a people, live well beside it or look up to it. To hate is to fear it,
fight it or hold a grudge against it. The lists run one way, so decide both directions:
whom the new race appreciates and hates, and which of the other races should list the new
race. Read them from the relations of the race, as the default list does from the
"Relations" part of races-basis.md. No generation rule reads the lists yet. They are meant
for the factions of a region.

**The favourite colours.** A list of ids from the `Colours` list of the Parameter Set
(society-generation.md section 9.1), the dearest first: the colours of the dress, the
banners and the goods of the race. Three or four is enough. The main and the secondary
colour of a faction are drawn mostly from the favourite colours of its lead race, the first
ones most often (section 4.9). A colour the list lacks is added to `Colours` first.

**What a new race does to the world.** Adding a race, or toggling one on, does not change
how many persons the land could feed. It changes how the land is shared: the new race takes
its share from the races already there. The one exception is land where nobody lived before.
With the default races the only such land by the plain shares is a Volcano patch below
200 m: the seven human races live in every other biome and at every height between them,
and the Dwarves live on the Volcano from 200 m and up. In a world with fewer races a new
race can add persons.

**Where the new race shows up.** A region holds six races: its four main races and two
minority races (society-generation.md section 3.3). The new race is a race of a region
where it is among the four largest by the plain shares, or where it has its turn for a
minority place and the land feeds at least half a person of it. A race that is never among the four
largest is in about one region in ten. Elsewhere it has 0 persons.

**The turns move.** The order of the turns and the stride of the turn pattern follow from
the races that are on. So one more race gives other minority races to regions all over
the world, not only where the new race lives. With 29 races a race has its turn in a
minority place once in every 29 regions.

## 2. Add the race to one world

1. Write the default Parameter Set to a file:

   ```bash
   dotnet run --project src/Gameworld.MapGenerator -c Release -- --export-parameters out/my-parameters.json
   ```

2. Add an entry at the end of the `Races` list of that file. List only the biomes the
   race lives in: a biome that is left out has the frequency 0.

   ```json
   {
     "Id": "moonfolk",
     "Name": "Moon folk",
     "Enabled": true,
     "BiomeFrequency": {
       "BorealForest": 2,
       "TemperateMixedForest": 6,
       "TemperateBroadleafForest": 4
     },
     "NameStyle": "Exotic",
     "HeightModifier": [
       { "HeightM": 100, "Factor": 0 },
       { "HeightM": 300, "Factor": 1 }
     ],
     "FactionLeaning": { "Family": 1, "Race": 3, "Vocation": 1, "Belief": 2, "Magic": 2, "Place": 1 },
     "AppreciatedRaces": ["elf", "stillfaces"],
     "HatedRaces": ["orc"],
     "FavouriteColours": ["silver", "indigo", "white"],
     "Description": "Slight and pale, with hair the white of hoarfrost and grey eyes. They dress in undyed wool and wear a silver ring in the left ear.",
     "Lore": "They say the first of them walked out of a winter that did not end, and that the frost followed."
   }
   ```

   Keep the other 28 entries. A file that gives `Races` replaces the whole default list,
   so a file with only the new race is a world with only that race.

3. Generate with the file:

   ```bash
   dotnet run --project src/Gameworld.MapGenerator -c Release -- --parameters out/my-parameters.json --stage Planned
   ```

   Or use the demo app: paste the entry into the `Races` list under "Parameter Set (json)"
   and press Apply. The potential population of a region then lists the race where it is
   a main race or a minority race of the region.

4. Check the result. The region files in `out/` hold the potential population per race,
   with the standing of the race in the region: `Main`, `Minority` or `Absent`, and the
   factions of the region with their lead race. A race has
   0 persons in a region it is not a race of, so look at more than the nine regions of
   the default block: `--rings 3` gives 49. A race that is on but is `Absent` everywhere
   and never has 0.5 persons by the plain shares finds no land: check its biomes and its
   height modifier against each other, with the tables of step 1. A race that only lives
   from 1,000 m and up has 1.4 % of the land, and no patch lies above 2,000 m.

A race that is added this way has no word lists for the names of its cities and towns. A
place it names gets a surname from the model of its `NameStyle` as its first part, and a
place word of the trade tongue: "Goodby", "Tyrport" (society-generation.md section 5.3).

A Parameter Set with a race without an id, or with the same id twice, is refused when the
world is created. So is one where a race lists itself, an id that is not in the `Races`
list, the same race as both appreciated and hated, or a favourite colour that is not in
the `Colours` list. Taking a race out of the list
therefore means taking its id out of the lists of the other races. Toggling it off does
not.

## 3. Add the race to the default list

Do step 2 first and look at the numbers. Then change these together:

| Where | What |
|---|---|
| [src/Gameworld.Domain/ParameterSet.cs](../src/Gameworld.Domain/ParameterSet.cs) | Add a `Make(...)` line to `RaceParameters.Defaults`: id, name, `common`, then the 16 habitat weights in the order of the column header. The Volcano has no column: a seventeenth weight is the one on a Volcano, and without it the race has the frequency 0 there. Only the Dwarves have one. Every race of the default list is on. Add `with { HeightModifier = ... }` for a height modifier; `OnlyFrom` and `OnlyBelow` make the two steps. Add the race to `DefaultLeanings` in the same record: its leaning to the six types of faction. Add it to `DefaultRelations` too: the races it appreciates and hates, and add its id to the lists of the races that appreciate or hate it. Add it to `DefaultFavouriteColours`: its favourite colours. Add it to `DefaultNameStyles`: its name style. A default race without an entry in any of the four fails when the list is built. Add a human race to `DefaultHumanRaces` too. |
| [src/Gameworld.Domain/DefaultTexts.Races.cs](../src/Gameworld.Domain/DefaultTexts.Races.cs) | Add the description and the lore of the race under its id: the parts "Appearance" and "History and lore" of its entry in races-basis.md, each paragraph on one line. Without an entry the race has `**missing description**` for both. |
| [default-parameter-set.json](default-parameter-set.json) | Add the same entry to `Races`, at the same place in the list, with the frequencies multiplied out. A test checks that this file equals `ParameterSet.Default`. |
| [src/Gameworld.Generation/Names/SettlementNameLexicons.cs](../src/Gameworld.Generation/Names/SettlementNameLexicons.cs) | Add the lexicon of the race under its id: its first parts and its place words for the names of its cities and towns (society-generation.md section 5.3). A test checks that every default race has one, that every tongue has place words for a city and for a town, and that no first part or place word stands twice in a list. Adding a lexicon does not change the names the other races give. |
| [city-and-town-names.md](city-and-town-names.md) | Add the section of the race: what its names are built from, and 50 names. Update the counts at the top. |
| [society-generation.md](society-generation.md) section 2 | Update the count in the parameter table ("28, all on") and the list of names below it. |
| [society-generation.md](society-generation.md) section 5.3 | Add the row of the race to the table of lexicons, and update the count of 28. |
| [society-generation.md](society-generation.md) Appendix A | Add the row to the table. Update the height modifier table and the note below it ("The other 23 races have none"). Add the row to the table of leanings and to the table of appreciated and hated races, and the name to the rows of the races that list it. Add the row to the table of favourite colours, and the name to its row in the table of name styles. Update the counts of the name styles in section 2. |
| [races-basis.md](races-basis.md) | Add the entry of the race, its row in the list and in the habitat table, and the counts at the top. |
| [readme.md](../readme.md), Race | Update the count ("The default list holds 28 races") and the groups. |
| [map-generation-research.md](map-generation-research.md) section 10.7 | Update the count of 28 and say where the weights of the new race come from. |
| [tests/Gameworld.Tests/PopulationTests.cs](../tests/Gameworld.Tests/PopulationTests.cs) | `DefaultRaces` lists the ids in order, and the count 28 is asserted in several tests. |

The JSON Schema ([parameter-set.schema.json](parameter-set.schema.json)) does not hold the
race list, so it is not regenerated.

Every race of the default list is on, so more changes with it:

- PopulationTests.cs computes shares from the frequencies in Temperate broadleaf forest
  and in Ice & snow. A race that lives in one of them changes those sums. Another test
  names the five races with a height modifier.
- society-generation.md sections 3.3, 3.4 and 5.1 and races-basis.md "Measured shares" quote
  measured shares. Measure them again (see the end of that part of races-basis.md).
- PopulationTests.cs asserts the turn pattern of 28 races: the stride 6, and a turn for
  each of the 28 races in every 6 × 5 block of regions. With 29 races both change.
- Every world on the defaults changes, since the new race takes land from the other 28
  and moves the turns of the minority races.

Then run the tests:

```bash
dotnet test -c Release
```

## What a race does not have yet

- **Person names of its own.** The name generator has three styles (GB, elven and dark elven),
  and a race picks one of them with `NameStyle`. A new name model needs a new style in the
  code. See [NameGenerator-Reuse.md](NameGenerator-Reuse.md), "Making a new model". So far
  only the surname of a house that rules a region is drawn from the style
  (society-generation.md section 4.10), and the first part of a place name of a race
  without a lexicon (section 5.3). The names of cities and towns are another matter: every
  default race has word lists of its own for them.
- **Families and persons.** How they get their race is not decided. A faction has a lead
  race (society-generation.md section 4).
- **Its own density.** A race changes its share of the land, not the capacity of the land.
- **Other habitat rules.** Bedrock, water, the Supernatural field and clustering per race
  are open. See the open topics of map-generation-research.md section 10.7.
