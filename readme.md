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
  Algorithms and parameter settlement. With a fair amount of manual testing to evaluate look and feel.
2) Gameplay development
  Game Client/UI interaction, and persisting player interaction with the gameworld. Here the core game mechanics for travel/exploration and progression are evolved.
3) Playable game - initial release
  Also prelaunch automated play and statistics on the distribution and variety of the gameworld and people.
4) Updates and new features
  Changes in world and map parameters can occur - but then a migration plan for player data should be included.

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

A foundation is immutable. Changing any parameter means creating a new foundation, never editing an existing one, so every generated map can be traced to exactly what produced it.

Many foundations can coexist in storage (e.g. to compare maps side by side in phase 1). A playthrough references exactly one foundation, and the game is modelled as single-player per foundation.

The world of a foundation is unbounded. Regions are not fixed up front, they come into existence lazily as play moves outward.

#### Parameter Set
An immutable set of all map and world parameters. Referenced by a foundation.

#### Derived and Authored data
All generated data belongs to one of two classes:

* Derived data is a pure function of the foundation (seed + parameter set + generator version). Regions, zones, patches, rivers, biomes, resources, prospects and all Stubs are derived. The database is only a cache of derived data - it can be evicted and recomputed with an identical result.
* Authored data comes from an LLM or a diffusion model. It cannot be reproduced, so it is stored as the source of truth and is never regenerated or evicted.

#### Randomness
There is no shared random stream. Every generation step seeds itself from a hash of (foundation seed, entity identity, purpose). A region gets the same result whether it is generated first or thousandth, so generation is independent of the order the player explores in.

#### Identity
All structural identities are deterministic paths. No database generated ids appear in the domain.

* Region = grid coordinate
* Zone cell = region + index
* Patch cell = zone cell + index
* City, Faction, Family and Person = parent + slot index

A Cell is the Voronoi site fixed when the region is Sited, and its path never changes. Zones and patches are built from cells and take their identity from them:

* A Zone is one or more zone cells. It has the identity of its surviving cell (see Zone and Patch transformation). The indices of absorbed zone cells are retired, so there can be gaps.
* A Patch keeps the identity from its original zone cell, also when that zone cell has been absorbed by another zone. The patch refers to the zone it currently belongs to.

This lets player state and authored data point at derived data that may be evicted and recomputed, and it makes a Stub addressable before it is Realised.

#### Region Generation Stages
A region moves through the stages below. Stages only advance - a region never moves backwards. Evicting derived data from storage and recomputing it is not a stage change.

Sited is not a state a region is in. It needs only the seed, so it is a pure function that can be calculated for any region at any time, and it is never persisted. Stored regions start at Outlined. The Sited row describes what the function yields.

| Stage | Contains | Needs |
|---|---|---|
| Sited | Region, Zone and Patch cell location, Zones initial type and bedrock. Ocean seeds, Determinated number of links, expansions and mergers | Seed only |
| Outlined | Zones linked, expaned and merged. Sea access | All regions within two rings Sited |
| Detailed | Patch mergers, rivers | All regions within two rings Sited |
| Elevated | heights, biomes, resources | All bordering regions Outlined |
| Settled | Prospects, and Stubs for cities and factions | The region Detailed |

Region sites come from a seed-hashed jittered grid, so the cell of any region and its neighbours can be computed locally without generating the rest of the world.

The policy for when a region advances (e.g. Detailed within some distance of the player) is not part of the domain model. The model only knows the stage and its preconditions.

#### Entity Generation States
Cities, factions, families and people each have their own generation state, independent of the region stage:

Stub → Pending → Realised

* A Stub holds every gameplay-relevant fact, and is derived data. For a person: age, sex, role, family links, faction membership and home.
* Pending means the authoring by LLM or diffusion model is in flight. On failure the entity returns to Stub.
* Realised adds the authored presentation only: name, biography and portrait. Authored data must not contradict the Stub.

Text and portrait are tracked separately, so a person can be shown with a name and biography before the picture arrives. A city can exist with a population count long before any individual in it is authored.

#### Migration
A migration maps an old foundation to a new foundation with the same seed (phase 4). Regions the player has reached Detailed are frozen: they keep the foundation they were generated under, so nothing the player has seen moves. Regions that have not reached Detailed regenerate under the new foundation. Each region therefore records which foundation it was generated under.

#### Region
The map is structured in large regions the size of Middle Ages European kingdoms (approximately 100-200km x 100-200km). The regions are divided into zones (approximately 8-12 zones per region). The zones are divided into patches (approximately 8-18 patches per zone).

These counts are Parameter Set values for the number of cells when the region is Sited. After the mergers the resulting number of zones and patches is lower - that is an outcome to evaluate in phase 1, not a parameter.

A region is a generation unit and it should be self contained. A region can be Outlined when all regions within two rings are Sited, and it can be Detailed when all bordering regions are Outlined (see Region Generation Stages).

A Region is the top level of three levels of nested Voronoi cells. 

#### Zone
Zones have an elevation type: Ocean, Flat Land, Lowland, Hills or Mountains. And zones also have a bedrock type that drives mined resources in its patches

The elevation type is the authoritative elevation class of the zone, and it is an input to river generation. The actual height is calculated in the Detailed stage from the rivers, and it stays within the band the elevation type allows. Ocean zones are never touched by river-driven height.

A zone is the middle level of three levels of nested Voronoi cells. 

#### Patch
Patches have one biome associated with them. The biome drives the patch flora and fauna.

A patch is the bottom level of three levels of nested Voronoi cells. The patch level of Voronoi cells has its borders warped - to give the map a more natural aesthetic.

#### Zone and Patch transformation
To give greater variation in zone and patch size and to give lager oceans - zones and patchs undergo mergers.
When a region is initialised a number of its zones are determend to be of the ocean type - these initial ocean zones are marked as ocean seeds

The first step in the transformation is ocean linking. For this step all neighboring regions and there zones and ocean seeds must be known.
When linking each ocean seed in a region has a configures changes of linking to it nearest ocean seed. when linking all zones in a streaght line between the to ocean seeds are changed to ocean type zones.

* The nearest ocean seed of a seed in a bordering region can be two regions away. So all regions within two rings must be Sited before a region can be Outlined.
* A link only converts zones in the two regions that own the two ocean seeds. A zone of a third region under the line is left alone.
* There is a configured maximum link distance. An ocean seed with no other seed within that distance does not link.

The second step is ocean expansion. In each region a configured number of expansions are determend per ocean seed, for each the ocean seeds nearest land zones (in the same region) are converted to ocean zones.

The third step is merges. In each region a configured number of merges are determend. that number of zones is randomly chosen and merged with is nearest neighbor in the same region. only land-land or ocean-ocean mergers are allowed - so some mergers can not be completed (but still counts).

The randomly chosen zone is the survivor of a merger. It keeps its own elevation type and bedrock, and the absorbed zone takes them over. Apart from this and the conversion to ocean, the elevation type a zone got when Sited is final.

These three steps make the region Outlined.

The fourth step is patch merger.
A number of patches in each zones merges with a naiboring patch in the same zone - This is to give greater variation in patch size and form.

The patch merger is the first step of the Detailed stage. It runs when the zones are final, and only within a zone as it is after the zone mergers. Ocean zones keep their patch cells, but they skip patch merger, biomes and land resources - their patches only carry sea resources (e.g. fish).

#### Sea
A sea is a connected set of ocean zones. It can span several regions. An ocean seed that never links gives an enclosed sea inside one region.

A region has sea access if it has at least one ocean zone when it is Outlined. Whether two coasts are on the same sea is not stored as a region fact, because the world is unbounded. It is answered by a search over Outlined regions when gameplay needs it.

#### Bedrock
Bedrock will eventually be a fixed set of predetermined bedrock types (approximately 10).

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

Examples (not binding yet)

Tropical rain forest, Tropical seasonal forest, Savanna, Subtropical desert, Temperate rain forest, Temperate seasonal forest, Woodland, Shrubland, Temperate grassland, Desert, Boreal forest, Tundra, Ice, Mangrove

#### Rivers
Rivers are calculated with a space-colonization algorithm, and it is the rivers that drive the height of the landscape.

Rivers never cross region borders. A landlocked region only gets some lakes and small rivers, and it has no access to ocean sailing.

#### Preconditions
Perlin Noise Maps that are used to drive climate, biomes and resources. They are Fields: stateless functions of foundation and coordinate that can be sampled anywhere at any time. Fields are never stored and have no generation stage - a stage only records the values it sampled.
* Precipitation
* Initial Temperature
* Prosperity
* Supernatural

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
    
#### Faction
A group of people. A political grouping. Could be a Kingdom, a thieves guild or a tribe of people.

#### Family
A family of NPCs. Typically living together in a household. Can include servants or slaves.

#### Person
An NPC in the game. A person.
