# Adding a race

The races are a list in the Parameter Set, not a fixed set in the code. So there are two
ways to add one, and they differ a lot in work:

- **To one world**: add an entry to the `Races` list of a Parameter Set file. No code
  changes. Use this to try a race out.
- **To the default list**: change the built-in defaults, so every world without its own
  Parameter Set file has the race. This touches code, tests and four docs.

The rules behind both are in [society-generation.md](society-generation.md) sections 2
and 3, and the default list is its Appendix A. The term Race is defined in the
[readme](../readme.md).

A change to the list is a new Parameter Set, and with it a new foundation. Foundations
that exist are not touched.

## 1. Decide what the race is

A race is five things:

| Field | What to decide |
|---|---|
| `Id` | The id generated data knows the race by, in camelCase like `frostFolk`. It must be unique in the list. Keep it when the name changes. |
| `Name` | The name shown for the race. |
| `Enabled` | Whether the race lives in the world. A race that is off gets no population. |
| `BiomeFrequency` | How frequent the race is in each of the 16 biomes, relative to the other races. |
| `HeightModifier` | Optional. How the height of the land changes the frequency. Leave it out when the height does not matter to the race. |

**The frequencies.** On one patch the races that are on share the land in proportion to
their frequency in its biome. Only the proportions between the races within one biome
matter. The default list builds each frequency as

```
frequency(race, biome) = habitat(race, biome) · common(race)
```

- `habitat` is 0 avoided, 1 tolerated, 2 good or 3 native.
- `common` is how common the race is in the world: 10 for humans, 2 for the other five
  races that are on, and 1 or 0.5 for the rest.

Follow the same scheme, so the new race compares to the others. A race with `common` 2
that is native to a biome gets 6 there, against 30 for humans in Temperate grassland.

The biome keys of a Parameter Set file are: `IceAndSnow`, `Tundra`, `ColdSteppe`,
`BorealForest`, `ColdDesert`, `TemperateGrassland`, `WoodlandAndShrubland`,
`TemperateMixedForest`, `TemperateBroadleafForest`, `TemperateRainForest`, `HotDesert`,
`ThornScrub`, `Savanna`, `TropicalDryForest`, `TropicalRainForest`, `Mangrove`.

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
| 0 to 50 m | 15.0 % | 16.3 % | 3.1 |
| 50 to 100 m | 17.6 % | 18.1 % | 2.9 |
| 100 to 200 m | 29.2 % | 31.0 % | 2.9 |
| 200 to 300 m | 17.7 % | 17.8 % | 2.8 |
| 300 to 500 m | 12.8 % | 11.6 % | 2.5 |
| 500 to 750 m | 4.5 % | 3.4 % | 2.1 |
| 750 to 1,000 m | 1.9 % | 1.3 % | 2.0 |
| 1,000 to 1,500 m | 1.2 % | 0.5 % | 1.2 |
| 1,500 to 2,000 m | 0.2 % | 0.03 % | 0.5 |

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

So a hot desert race that only lives from 200 m and up has about a fifth of the hot
desert, and a tundra race with the same limit keeps more than half of the tundra.

**What a new race does to the world.** Adding a race, or toggling one on, does not change
how many persons the land feeds. It changes how the land is shared: the new race takes its
share from the races already there. The one exception is land where nobody lived before.
With the six default races, Ice & snow below 200 m holds nobody, so a race that lives there
adds persons.

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
     "HeightModifier": [
       { "HeightM": 100, "Factor": 0 },
       { "HeightM": 300, "Factor": 1 }
     ]
   }
   ```

   Keep the other 23 entries. A file that gives `Races` replaces the whole default list,
   so a file with only the new race is a world with only that race.

3. Generate with the file:

   ```bash
   dotnet run --project src/Gameworld.MapGenerator -c Release -- --parameters out/my-parameters.json --stage Planned
   ```

   Or use the demo app: paste the entry into the `Races` list under "Parameter Set (json)"
   and press Apply. The potential population of a region then lists the race.

4. Check the result. The region files in `out/` hold the potential population per race.
   A race that is on but has 0 persons everywhere finds no land: check its biomes and its
   height modifier against each other, with the tables of step 1. A race that only lives
   from 1,000 m and up has 1.4 % of the land, and no patch lies above 2,000 m.

A Parameter Set with a race without an id, or with the same id twice, is refused when the
world is created.

## 3. Add the race to the default list

Do step 2 first and look at the numbers. Then change these together:

| Where | What |
|---|---|
| [src/Gameworld.Domain/ParameterSet.cs](../src/Gameworld.Domain/ParameterSet.cs) | Add a `Make(...)` line to `RaceParameters.Defaults`: id, name, on or off, `common`, then the 16 habitat weights in the order of the column header. Add `with { HeightModifier = ... }` for a height modifier; `OnlyFrom` and `OnlyBelow` make the two steps. |
| [default-parameter-set.json](default-parameter-set.json) | Add the same entry to `Races`, at the same place in the list, with the frequencies multiplied out. A test checks that this file equals `ParameterSet.Default`. |
| [society-generation.md](society-generation.md) Appendix A | Add the row to the table. Update the height modifier note below it ("The other 21 races have none"). |
| [society-generation.md](society-generation.md) section 2 | Update the count in the parameter table ("23, of which 6 are on"). |
| [readme.md](../readme.md), Race | Update the count ("The default list holds 23 races") and the races that are on. |
| [map-generation-research.md](map-generation-research.md) section 10.7 | Update "All 23" and say where the weights of the new race come from. The 23 came from the research note RACES §4.1; a race from elsewhere is a new decision. |
| [tests/Gameworld.Tests/PopulationTests.cs](../tests/Gameworld.Tests/PopulationTests.cs) | The count 23 is asserted in two tests. |

The JSON Schema ([parameter-set.schema.json](parameter-set.schema.json)) does not hold the
race list, so it is not regenerated.

**If the race is on by default**, more changes:

- `ClassicSix` in PopulationTests.cs is the list of races that are on. The tests around it
  also assume that only dwarves and elves have a height modifier, and compute shares from
  the six frequencies in one biome.
- society-generation.md section 3.3 quotes measured shares per race (66 % human and so on).
  Measure them again. Its Appendix A also describes which of the races that are on live in
  which biome.
- Every world on the defaults changes, since the new race takes land from the other six.

Then run the tests:

```bash
dotnet test -c Release
```

## What a race does not have yet

- **Names.** The name generator has three styles (GB, elven and dark elven) and nothing
  maps a race to a style. See [NameGenerator-Reuse.md](NameGenerator-Reuse.md).
- **Factions, families and persons.** How they get their race is not decided.
- **Its own density.** A race changes its share of the land, not the capacity of the land.
- **Other habitat rules.** Bedrock, water, the Supernatural field and clustering per race
  are open. See the open topics of map-generation-research.md section 10.7.
