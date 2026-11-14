# Gameworld

## Purpose

Gameworld is a sandbox, prototype and research repo for a larger PC game. It explores new gameplay and generation algorithms for map-based generated worlds, using diffusion models and LLMs to populate huge worlds on demand. A priority is an aesthetically pleasing and believable (but fantasy/magical elements are allowed) base map that can support the desired gameplay.

## Aim

* The game world should be larger than any player reasonably can explore, even if playing multiple hours per day for half a year.

* There should be a very large number of people in the gameworld known to an active player.

* The geography, map, cities, factions, families and people should be procedurally generated.

* A fine balance between believability and wonder.

* Variation, exploration and progression that foster continued play and replayability.
 
## Lifecycle Phases

1) World Generation Development
  Algorithms and parameter settlement. With a fair amount of manual testing to evaluate look and feel. Includes internal map demo apps.
2) Gameplay development
  Game Client/UI interaction, and persisting player interaction with the gameworld. Here the core game mechanics for travel/exploration and progression are evolved.
3) Playable game - initial release
  Also prelaunch automated play and statistics on the distribution and variety of the gameworld and people.

## First Step Domain Model - Need and drivers

* Phase 1 will include changing map and world parameters and evaluating the resulting maps and worlds, so parameters need to be easily configurable and testable.

* Storage. At game start a big area of the game world and map are calculated to facilitate the populating with Cities, Factions, Families and individual People. To avoid repetitive map and world generation calculation significant data are persisted and stored in a database.

  A large number of people and town/cities are supported by the game - but only a small and relevant set is precalculated. The generation of people and locations is dependent on LLMs for text and diffusion models for pictures. As play progresses, more people and locations are calculated.

* Data transfer between backend and frontend clients

### Some High Level Details - Decided in advance

#### Foundation
The foundation is the root of the game world. It has:

* The seed for all the generation algorithms.
* The generator version. This versions the generation algorithms only - not the parameters and not the storage schema.
* A reference to exactly one Parameter Set.
* It can load and save all parameters to and from a json file

During phase 1 the parameters of a foundation can be changed and reloaded. All derived data of the foundation is then discarded and recomputed.

After phase 1 a foundation is immutable. Parameters will never change after the initial load (there might be different sets of parameters for different game modes or difficulty levels - but they do NOT change during gameplay)

Many foundations can coexist in storage (e.g. to compare maps side by side in phase 1). A playthrough references exactly one foundation, and the game is modelled as single-player per foundation.

The world of a foundation is unbounded. Regions are not fixed up front, they come into existence lazily as play moves outward.

#### Parameter Set
The set of all map and world parameters. Referenced by a foundation, and immutable after phase 1 like the foundation. Can be json files or exported/loaded from json files.

#### Derived and Authored data
All generated data belongs to one of two classes:

* Derived data is a pure function of the foundation (seed + parameter set + generator version). Regions, zones, patches, rivers, lakes, heights, biomes, resources, prospects, the travel network and all Stubs are derived. The database is only a cache of derived data - it can be evicted and recomputed with an identical result.
* Authored data comes from an LLM or a diffusion model. It costs to produce, so it is stored as the source of truth and is never regenerated or evicted.

#### Randomness
There is no shared random stream. Every generation step seeds itself from a hash of (foundation seed, entity identity, purpose). A region gets the same result whether it is generated first or thousandth, so generation is independent of the order the player explores in.

Where a step needs a sequence of random numbers (e.g. pick 3 of the zone cells of a region), a small generator is seeded from the hash of that one cell and consumed in a fixed order.

#### Identity
All structural identities are deterministic. No database generated ids appear in the domain.

* Region = coordinate in the region grid
* Zone cell = coordinate in the zone grid
* Patch cell = coordinate in the patch grid
* City, Faction, Family and Person = parent + slot index

Each of the three levels has its own grid, and every grid square holds exactly one Voronoi site. A Cell is that site. It is a pure function of the foundation and its coordinate never changes.

The parent of a cell is not part of its identity, it is derived: a zone cell is owned by the region that contains its site, and a patch cell is owned by the zone cell that contains its site.

Zones and patches are built from cells and take their identity from them:

* A Zone is one or more zone cells that have merged. It has the identity of its root cell - the cell the others have merged into (see Zone and Patch transformation).
* A Patch is one or more patch cells that have merged. It has the identity of its root cell, and it belongs to the zone of that cell.
* For gameplay a merged zone or patch is considered to be one large cell. The merging is there to give greater variation in size and form.

This lets player state and authored data point at derived data that may be evicted and recomputed, and it makes a Stub addressable before it is Realised.

#### Region Generation Stages
A region moves through the stages below, in the order of the table and one stage at a time. No stage is skipped: a region is Elevated before it is Planned. Stages only advance - a region never moves backwards. Evicting derived data from storage and recomputing it is not a stage change.

Sited is not a state a region is in. It needs only the seed, so it is a pure function that can be calculated for any region at any time, and it is never persisted. Stored regions start at Outlined. The Sited row describes what the function yields.

| Stage | Contains | Needs |
|---|---|---|
| Sited | Region, Zone and Patch cell location, Zone cells initial type and bedrock. Ocean seeds, Determined number of links, expansions and mergers | foundation only |
| Outlined | Zones linked, expanded and merged. Sea access | All regions within the link range + 5 rings Sited (7 rings at the defaults) |
| Detailed | Patch mergers, rivers, lakes. Estimated biomes and resources | All bordering regions Outlined |
| Elevated | Height map. Final biomes and resources | All bordering regions Detailed |
| Planned | Factions and Prospects for cities and towns | The region Elevated |
| Settled | Travel network. Actual Cities and towns, families and important persons | The region and all bordering regions Planned |

The sites of all three levels come from seed-hashed jittered grids, so the cell of any region, zone or patch and its neighbours can be computed locally without generating the rest of the world.

Biomes and resources exist in two forms. The estimated form is calculated in the Detailed stage with a typical height per elevation type instead of the real height. The final form is calculated in the Elevated stage with the height from the height map. Lakes, Prospects and the travel network only read the estimated form, so they never depend on the height map. The region is still Elevated before it is Planned: the Planned stage reads the Detailed data, but it does not come before the Elevated stage.

The policy for when a region advances (e.g. Detailed within some distance of the player) is not part of the domain model. The model only knows the stage and its preconditions.

#### Entity Generation States
Cities, factions, families and people each have their own generation state, independent of the region stage:

Stub → Pending → Realised

* A Stub holds every gameplay-relevant fact, and is derived data. For a person: age, sex, role, family links, faction membership and home.
* Pending means the authoring by LLM or diffusion model is in flight. On failure the entity returns to Stub.
* Realised adds the authored presentation only: name, biography and portrait. Authored data must not contradict the Stub.

Text and portrait are tracked separately, so a person can be shown with a name and biography before the picture arrives. A city can exist with a population count long before any individual in it is authored.

#### Region
The map is structured in large regions the size of Middle Ages European kingdoms (approximately 100-200km x 100-200km, 128 km by default). The regions are divided into zones (32 km by default). The zones are divided into patches (8 km by default).

The cell sizes are the Parameter Set values - not the counts. With the default sizes a region owns about 16 zone cells and a zone cell owns about 16 patch cells, and the exact count varies with where the sites fall. After the mergers the resulting number of zones and patches is lower - that is an outcome to evaluate in phase 1, not a parameter.

A region is the generation unit: rivers, lakes, the height map and prospects are calculated one region at a time. A region is not fully self contained:

* Ocean links and ocean expansion convert zone cells in any region.
* A zone cell can merge into a zone of a neighbouring region. It then moves to that region, so region borders follow the merged zones.

So the zones of a region are the zones whose root cell the region owns - not the zone cells the region owns.

A region can be Outlined when all regions within the link range + 5 rings are Sited, and it can be Detailed when all bordering regions are Outlined (see Region Generation Stages).

A Region is the top level of three levels of nested Voronoi cells. The levels are nested through the patch: a point belongs to a patch, the patch to a zone and the zone to a region. Zone borders, region borders and coastlines therefore all run along patch borders.

#### Zone
Zones have an elevation type: Ocean, Flat Land, Lowland, Hills or Mountains. And zones also have a bedrock type that drives mined resources in its patches

The elevation type is the authoritative elevation class of the zone, and it is an input to river generation. The actual height is calculated in the Elevated stage from the rivers. The elevation type does not give a height band - it gives how steeply the land rises from rivers and coast, the gradient of the rivers and the roughness of the surface. Ocean zones are never touched by river-driven height, their depth follows the distance from the coast.

The elevation type and the bedrock are drawn by configured weights per zone cell when the region is Sited. The bedrock weights depend on the elevation type.

A zone is the middle level of three levels of nested Voronoi cells. 

#### Patch
Patches have one biome associated with them. The biome drives the patch flora and fauna.

A patch is the bottom level of three levels of nested Voronoi cells. The patch level of Voronoi cells has its borders warped - to give the map a more natural aesthetic. Because the levels are nested through the patch, the coastlines and the zone and region borders get the same warped shape.

#### Zone and Patch transformation
To give greater variation in zone and patch size and to give larger oceans - zones and patches undergo mergers.
When a region is initialised a number of the zone cells it owns are determined to be of the ocean type - these initial ocean zone cells are marked as ocean seeds.

The first step in the transformation is ocean linking.
When linking, each ocean seed makes zero, one or two links by configured chances. It links to its nearest other ocean seeds. When linking, all zone cells in a straight line between the two ocean seeds are changed to ocean type.

* A link converts every zone cell under the line, whichever region owns it.
* There is a configured link range, counted in regions (2 by default). An ocean seed only links to the seeds of regions within that range, and an ocean seed with no other seed within the range does not link.
* A zone cell can be converted by a link that starts several regions away. So all regions within the link range + 5 rings must be Sited before a region can be Outlined.

The second step is ocean expansion. Every zone cell on a link - the two ocean seeds and the cells between them - converts a number of its nearest land neighbours to ocean. The number is 0 to 3 and it is drawn by configured weights. The neighbours can be in another region. An ocean seed that is not on any link does not expand. The expansion can run in a configured number of passes (1 by default), where each pass expands from the cells the previous pass converted.

The third step is zone mergers. In each region a configured number of mergers are determined. That number of the zone cells the region owns is randomly chosen, and each merges into its nearest neighbour on the same side of the coast. Only land-land or ocean-ocean mergers are allowed - so some mergers can not be completed (but still count). The neighbour can be owned by another region.

The neighbour is the survivor of a merger. It can itself have merged into a third zone cell. The cell at the end of that chain is the root, and all zone cells with the same root are one zone. The zone takes everything from its root: ocean or land, the region, the elevation type and the bedrock. Apart from this and the conversion to ocean, the elevation type a zone cell got when Sited is final.

These three steps make the region Outlined.

The fourth step is patch merger.
A number of the patch cells of each zone cell merge into their nearest neighbour patch in the same zone - This is to give greater variation in patch size and form. Roots are found as for zones.

The patch merger is the first step of the Detailed stage. It runs when the zones are final, and only within a zone as it is after the zone mergers. Ocean zones have patch mergers like land zones, but they skip biomes and land resources - their patches only carry sea resources (e.g. fish).

See [docs/map-generation.md](docs/map-generation.md) for the details of the zone and patch transformation (sections 5 and 6), and for all the later generation steps. The reasons behind the algorithms are in [docs/map-generation-research.md](docs/map-generation-research.md).

#### Sea
A sea is a connected set of ocean zones. It can span several regions. An ocean seed that is not on any link gives a small enclosed sea of one zone.

A region has sea access if an ocean zone touches its land when it is Outlined. The ocean zone can belong to a neighbouring region. A region without sea access is landlocked. Whether two coasts are on the same sea is not stored as a region fact, because the world is unbounded. It is answered by a search over Outlined regions when gameplay needs it.

#### Bedrock
Bedrock will eventually be a fixed set of predetermined bedrock types (approximately 10). The generation currently uses the nine below, with weights per elevation type (see docs/map-generation.md section 14.1).

Example table (not binding yet)
| Id          | Name                 | Mostly under            | Gives                                                    |
| ----------- | -------------------- | ----------------------- | -------------------------------------------------------- |
| `shield`    | Crystalline shield   | flat, lowland, hills    | Granite, banded iron, lode gold (hills), diamonds, kaolin |
| `foldBelt`  | Fold belt            | hills, mountains        | Slate, marble, silver, lode gold, garnet, emerald        |
| `granite`   | Granite intrusion    | hills, mountains        | Granite, tin lodes, skarn iron, topaz and beryl, kaolin  |
| `volcanic`  | Volcanic arc         | mountains               | Basalt, obsidian, pumice, porphyry copper, gold, rock crystal |
| `basalt`    | Flood basalt         | flat, lowland, hills    | Basalt, native copper, agate, black sands                |
| `ophiolite` | Ophiolite            | hills, mountains        | Soapstone, massive-sulfide copper, jade                  |
| `carbonate` | Carbonate platform   | lowland, hills          | Limestone, chalk, flint, lead and zinc, skarn iron       |
| `clastic`   | Clastic basin        | flat, lowland           | Sandstone, coal, lignite, red-bed copper, rock salt, amber |
| `alluvium`  | Unconsolidated cover | flat, lowland           | Field stone only; clay                                   |


#### Biome
Biomes will eventually be a fixed set of predetermined biomes (10-20). Geography and climate will drive biomes.

The biome of a patch comes from its temperature and its aridity. The temperature is the Initial Temperature field cooled by the height of the patch. The aridity is the precipitation relative to what the temperature demands. Mangrove is an override on warm, wet and low coasts.

The current set (not binding yet) is 15 land biomes and Mangrove:

Ice & snow, Tundra, Cold steppe, Boreal forest, Cold desert, Temperate grassland, Woodland & shrubland, Temperate mixed forest, Temperate broadleaf forest, Temperate rain forest, Hot desert, Thorn scrub, Savanna, Tropical dry forest, Tropical rain forest, Mangrove

#### Rivers
Rivers are calculated per region with a space-colonization algorithm, growing from the sea into the land. It is the rivers that drive the height of the landscape.

A river never runs on the land of another region. Region borders are therefore watersheds, and the height map puts a ridge on them. The exception is a landlocked region: its rivers search up to 4 zones away for an ocean, and they cross the land of a neighbouring region to reach it.

#### Lakes
Lakes are placed per region after the rivers. A lake always sits on a river, so every lake has a river running in and out. There are no lakes in mountain zones, in deserts or in river-poor flat zones (dry flat zones that get few rivers).

Lakes do not change the height map. They count as fresh water for resources and prospects.

#### Fields
Perlin Noise Maps that are used to drive climate, biomes and resources. They are Fields: stateless functions of foundation and coordinate that can be sampled anywhere at any time. Fields are never stored and have no generation stage - a stage only records the values it sampled. Precipitation and Initial Temperature are warped, so their highs and lows form winding belts instead of round spots.
* Precipitation
* Initial Temperature
* Prosperity
* Supernatural

The height map uses two more noise functions of the same kind: a relief field that makes neighbouring zones rugged or gentle together, and a detail noise on the surface.

#### Resource

This section focuses on resources found in patches, these are generated together with the map and world. Resources that are produced (e.g during gameplay) are only covered lightly in this section.

##### Groups

Resources are split in groups to make trade and needs calculation more manageable.

* Food 
* Materials
* Fuel
* Luxuries 
* Metals
* Animals
* Building (Only produced)
* Tools (Only produced)
* Gear (Only produced)
* Weapons (Only produced)
* Clothing (Only produced)

##### Potentials or Deposits
Potentials are what the land can yield. Deposits are rare finds that often need to be mined.

##### Variants
Variants of the same resource group

##### Product
Products are produced from resources

##### Generated resources
The generation gives every patch an amount for each of 24 raw resources, and a variant (see docs/map-generation.md section 14). The example table below is finer than these 24 in some rows:

* Wild food and game are one generated resource. The same goes for solid and liquid fuel, silver ore and lead ore, gems and pearls, spices and incense, furs and ivory, and mounts and pack animals.
* Salt is a generated resource of its own. In the table it is a variant of Preserver.
* Star metal and the sap from Elanor are fantasy resources. The generation has no rule for them yet.

Example table (not sufficient)

| Group | Resources | Variant |
|---|---|---|
| Food | Grain | wheat, durum, spelt, barley, rye, oats, maize, sorghum, pearl millet, teff, buckwheat, quinoa, fonio, wild rice |
| Food | Produce| Potato, cassava, yam, taro, banana, sago palm, breadfruit, faba bean, pea, lentil, chickpea, common bean, cowpea, pigeon pea, onion and alliums, cabbage and kale, carrot, beet, melons, tiger nut, bottle gourd |
| Food | Orchard | Olive, grape, date palm, fig, pomegranate, almond, pistachio, carob, apple, pear, peach and stone fruit, citrus, walnut, sweet chestnut, hazel, stone pine nuts, cedar-pine nuts, coconut, oil palm, mango, avocado, tamarind, baobab, shea, Brazil nut, cashew, palmyra palm, prickly pear, jujube, mesquite pods, pineapple, soybean, groundnut, sesame, rapeseed, sunflower |
| Food | livestock | Cattle, sheep, goats, pigs, yak, herded reindeer, water buffalo, poultry, alfalfa, dairy products |
| Food | fish | River fish, salmon runs, sturgeon, eels, lake fish, cod banks, herring/sardine/anchovy, tuna and large pelagics, oysters and mussels, clams, crabs, lobsters, shrimp, octopus |
| Food | wild food | Bilberry, blueberry, cloudberry and crowberry, raspberry and bramble, sea buckthorn, elderberry, forest mushrooms, acorns, wild grass seed, lotus, water chestnut, cattail, arrowhead, maple sap, edible seaweed, kelp, wild herbs  |
| Food | game | deer, wild boar, caribou, bison and wild cattle, antelope and gazelle, wild sheep and ibex, waterfowl, upland game birds, hares and rabbits  |
| Materials | Timber | Oak, beech, ash, elm, linden, willow and osier, poplar, Scots pine, spruce, fir, larch, giant conifers, cedar, yew, cypress, Mediterranean pines, cork oak, teak, sal, mahogany, Spanish cedar, rosewood, ebony, African mahogany, iroko, balsa, eucalypt-type hardwood, thorn-scrub wood, juniper, mangrove timber, driftwood, bog oak  |
| Materials | Stone | granite, basalt, limestone, chalk, marble, sandstone, slate, flint and chert, obsidian, pumice and pozzolana, soapstone, millstone and quern stone, whetstone |
| Materials | Clay | Clay, kaolin  |
| Materials | Crystal | quartz, Rock crystal   |
| Materials | Fibre | Flax, hemp, cotton, wool, silk, wild silk, jute, ramie, kenaf, sisal and agave, abaca, New Zealand flax, esparto grass, kapok, nettle, lime-bast |
| Materials | Preserver | Salt, Sap from the Elven flower Elanor  |
| Fuel | Solid | Firewood, charcoal, peat, dung fuel, coal, lignite, turf and sod |
| Fuel | Liquid | oil, naphtha seeps |
| Metals | Iron ore | Bog iron, lateritic iron and ironstone, banded iron formation, magnetite skarn iron, black sands |
| Metals | copper ore | Porphyry copper, red-bed copper, massive-sulfide copper, native copper, malachite, azurite  |
| Metals | tin ore | Tin lode, stream tin |
| Metals | silver ore | Silver (argentiferous galena) |
| Metals | lead ore | lead-zinc in carbonates, lead-zinc skarns and veins, calamine |
| Metals | gold ore | gold ore |
| Metals | star metal | star metal |
| Luxuries | Gems| Diamond, Ruby, Sapphire, Emerald, Aquamarine, Amethyst, Garnet, Topaz, Tourmaline, Opal, Jade, Lapis, Turquoise, Spinel, Zircon |
| Luxuries | Pearls | Pearls |
| Luxuries | spices| Black pepper, clove, nutmeg and mace, cinnamon, cardamom, ginger, turmeric, vanilla, allspice, chili pepper, saffron, cumin, coriander, caraway, mustard |
| Luxuries | incense | frankincense, myrrh, gum arabic, sandalwood, agarwood, pine resin/tar/pitch, copal and dammar, camphor, lavender and rose oil, cinchona bark, mastic and balsams, rubber latex  |
| Luxuries | stimulants & sugar | Honey, sugar cane, palm sugar, tea, coffee, cacao, kola nut, betel nut, coca, tobacco |
| Luxuries | furs | Sable and marten, beaver, fox and Arctic fox, ermine, squirrel, bear, big cats, seals, ostrich, exotic feathers, ambergris, eider down, musk deer |
| Luxuries | ivory | walrus, narwhal tusk, elephant ivory, hippopotamus ivory |
| Luxuries | dyes | Cochineal, kermes, lac, murex, woad, indigo, madder, weld, safflower, henna, annatto, red sanders and brazilwood, logwood, lichen dyes |
| Animals | Mounts | Horse |
| Animals | Pack Animals | Horses, donkeys, mules, dromedary camel, Bactrian camel, llama, alpaca-like camelids  |
     
#### Prospect 
A potential location for a City or Town. Has information about resources, travel options etc.

Every land patch of a region gets a score from five factors: resources, geography, trade, climate and prosperity. The best patches become the Prospects of the region (20 by default, of which up to 4 are for cities), with a minimum spacing between them. A Prospect never touches the land of another region, which keeps Prospects apart across region borders without reading the neighbour.

Prospects only read the estimated biomes and resources (see Region Generation Stages).

#### Travel network
Roads, river routes and sea lanes that connect the Prospects. The network of a region is calculated from the Prospects of the region and its bordering regions. A link between Prospects in two regions is decided by exactly one region, so the network does not depend on the order the regions are generated in.
    
#### Faction
A group of people in one region. A political grouping. Could be a Kingdom, a thieves guild or a tribe of people.

#### Family
A family of NPCs. Typically living together in a household. Can include servants or slaves.

#### Person
An NPC in the game. A person.

## Setup and run

The code is one .NET solution, `Gameworld.slnx`. It needs the .NET 11 SDK and nothing else: no database and no external services. The NuGet packages of the test project are restored on the first build.

| Project | What it is |
|---|---|
| `src/Gameworld.Domain` | The domain model of this readme |
| `src/Gameworld.Generation` | The generation algorithms of [docs/map-generation.md](docs/map-generation.md) and the map renderer |
| `src/Gameworld.MapGenerator` | The map generation app (console) |
| `src/Gameworld.Demo` | The demo app (web map viewer) |
| `tests/Gameworld.Tests` | The tests |

Run the commands from the repository root.

### Build and test

```bash
dotnet build Gameworld.slnx -c Release
```

```bash
dotnet test Gameworld.slnx -c Release
```

### Map generation app

Generates a block of regions of one foundation up to a stage and writes the result to a folder: `map.png`, `biomes.png` and `height.png`, the foundation as `foundation.json` and each region as `regions/<i>_<j>.json`.

```bash
dotnet run --project src/Gameworld.MapGenerator -c Release
```

Without options it takes the 3 × 3 regions around region (0, 0) of the seed `cellgen` to the Settled stage with the default Parameter Set, and writes to `out/`. Options go after `--`:

```bash
dotnet run --project src/Gameworld.MapGenerator -c Release -- --seed atlas --region 2,2 --rings 2 --out out/atlas
```

| Option | Meaning | Default |
|---|---|---|
| `--seed <text>` | The seed text of the foundation | `cellgen` |
| `--parameters <file.json>` | The Parameter Set | the built-in defaults |
| `--region <i,j>` | The centre region | `0,0` |
| `--rings <n>` | Rings of regions around the centre (1 gives 3 × 3 regions) | `1` |
| `--stage <stage>` | Outlined, Detailed, Elevated, Planned or Settled | `Settled` |
| `--out <folder>` | Where to write | `out` |
| `--scale <m/px>` | Metres per pixel of the maps | `250` |
| `--modes <list>` | Which maps to draw: `map`, `biomes`, `height` | all three |
| `--no-regions` | Do not write the regions as json | |
| `--export-parameters <file>` | Write the default Parameter Set to a json file and stop | |
| `--export-schema <file>` | Write the JSON Schema of the Parameter Set to a file and stop | |

To try other parameters, export the defaults, edit the file and pass it back. A parameter that is left out of the file keeps its default.

```bash
dotnet run --project src/Gameworld.MapGenerator -c Release -- --export-parameters my-parameters.json
```

```bash
dotnet run --project src/Gameworld.MapGenerator -c Release -- --parameters my-parameters.json --out out/mine
```

### Help text for the parameters

Every parameter has a description: what it does, its unit and the section of [docs/map-generation.md](docs/map-generation.md) it belongs to. The descriptions are written once, on the `ParameterSet` types in `src/Gameworld.Domain/ParameterSet.cs`, and [docs/parameter-set.schema.json](docs/parameter-set.schema.json) is generated from them as a JSON Schema, together with the default, the range and the allowed values of each parameter.

An editor that knows the schema shows the description when you hover a name, completes names and values, and underlines unknown names and values out of range.

* In VS Code this works without setup for every file in this folder named like `*parameters*.json` or `*parameter-set*.json` (see `.vscode/settings.json`).
* In any other editor that follows `"$schema"`, put it first in the file, with the path to the schema from the file, as [docs/default-parameter-set.json](docs/default-parameter-set.json) does: `"$schema": "./parameter-set.schema.json"`. The line is ignored when the file is loaded.

After changing a description or adding a parameter, regenerate the schema (a test fails while it is out of date):

```bash
dotnet run --project src/Gameworld.MapGenerator -c Release -- --export-schema docs/parameter-set.schema.json
```

### Demo app

A map viewer in the browser, on top of the same generator.

```bash
dotnet run --project src/Gameworld.Demo -c Release
```

Then open http://localhost:5062. Drag to pan, scroll to zoom, and click the map to see the region, zone and patch at that point. The side panel switches the base map (elevation types, biomes, height, or none) and the layers, among them the four Fields, and it replaces the foundation: a new seed, or an edited Parameter Set under "Parameter Set (json)". Replacing the foundation discards all derived data.

Regions are generated when they come into view, so the first view of an area takes longer than the next. The Prospects and travel layers take the regions through the Elevated stage first, which builds their height maps.
