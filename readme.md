# Gameworld

Gameworld generates a map-based fantasy world on demand: an unbounded map of regions with seas, rivers, heights, biomes and resources, and a society of races, factions, cities, families and persons that lives on it. It is a sandbox, prototype and research repo for a larger PC game.

This readme defines the concepts of the world and is the source of every name. The rules of the generation are in two documents:

| Subject | Document | Covers |
| --- | --- | --- |
| The map | [docs/map-generation.md](docs/map-generation.md) | Geography, climate, biomes, resources, Prospects and the travel network |
| The society | [docs/society-generation.md](docs/society-generation.md) | Races, the potential population, factions, cities and towns, families, persons, vocations and looks |

Contents: [Overview](#overview) · [Concepts](#concepts) · [Documentation](#documentation) · [Build and run](#build-and-run)

## Overview

### Purpose

Gameworld explores new gameplay and generation algorithms for map-based generated worlds, using diffusion models and LLMs to populate huge worlds on demand. A priority is an aesthetically pleasing and believable base map that can support the desired gameplay. Fantasy and magical elements are allowed.

### Goals

* The game world should be larger than any player reasonably can explore, even if playing multiple hours per day for half a year.
* There should be a very large number of people in the game world known to an active player.
* The geography, map, cities, factions, families and people should be procedurally generated.
* A fine balance between believability and wonder.
* Variation, exploration and progression that foster continued play and replayability.

### Phases

| Phase | What happens in it |
| --- | --- |
| 1. World generation development | Algorithms and parameter settlement, with a fair amount of manual testing to evaluate look and feel. Includes internal map demo apps. |
| 2. Gameplay development | Game client and UI interaction, and persisting player interaction with the game world. Here the core game mechanics for travel, exploration and progression are evolved. |
| 3. Playable game, initial release | Also prelaunch automated play, and statistics on the distribution and variety of the game world and its people. |

The project is in phase 1.

### What drives the design

* **Parameters must be easy to change and test.** Phase 1 consists of changing map and world parameters and evaluating the resulting maps and worlds.
* **Storage.** At game start a big area of the game world and map is calculated, to facilitate the populating with cities, factions, families and persons. To avoid repeating that calculation, significant data is persisted in a database.
* **Generation on demand.** The game supports a large number of persons, towns and cities, but only a small and relevant set is precalculated. Persons and locations depend on LLMs for text and diffusion models for pictures. As play progresses, more persons and locations are calculated.
* **Data transfer** between the backend and the frontend clients.

### Status

The map is generated through all its stages, from the cells to the travel network, with the flora and fauna of every patch. Of the society, the races of each region, their potential population, the factions of each region, the name of each region, and the cities and towns of each region with their names, their persons and their races are generated. The persons of a city or a town are a count per race so far. The person-name generator is in place, and for now it gives the surnames of the houses that name a region and some of the first parts of city and town names. The vocations are a list in the Parameter Set and are not drawn yet. Families and individual persons are defined below but not generated yet, and neither is there a database: the apps compute everything in memory.

## Concepts

Each concept below says what the thing is, what it is called and how it relates to the others. The rules behind a concept are in the document its group links to.

```text
Foundation = seed + generator version + Parameter Set
└─ World, unbounded
   ├─ Map                               docs/map-generation.md
   │  ├─ Region ▸ Zone ▸ Patch          three nested levels, each built from Cells
   │  ├─ Zone                           elevation type, bedrock; ocean zones form Seas
   │  ├─ Patch                          biome, flora and fauna, resources
   │  ├─ Region                         rivers, lakes, height map, Prospects, travel network
   │  └─ Fields                         precipitation, initial temperature, prosperity, supernatural
   └─ Society                           docs/society-generation.md
      ├─ Race                           six of them share the potential population of a region
      ├─ Faction                        a ruling faction per region, an opposition, lesser factions
      ├─ Region name                    the realm of the ruling faction
      ├─ City and town                  one on every Prospect on show, with a name and races
      ├─ Family, Person                 a person with a vocation and looks
      └─ Colour                         favourite colours of a race, the banner of a faction
```

### The world

These concepts hold for the map and the society alike.

#### Foundation

The root of a game world. It holds the seed for all the generation algorithms, the generator version and a reference to exactly one Parameter Set. The generator version versions the generation algorithms only, not the parameters and not the storage schema. A foundation can load and save its parameters from and to a json file.

During phase 1 the parameters of a foundation can be changed and reloaded. All derived data of the foundation is then discarded and recomputed. After phase 1 a foundation is immutable: there can be different sets of parameters for different game modes or difficulty levels, but they do not change during gameplay.

Many foundations can coexist in storage, for one to compare maps side by side in phase 1. A playthrough references exactly one foundation, and the game is modelled as single-player per foundation. The world of a foundation is unbounded: regions are not fixed up front, they come into existence lazily as play moves outward.

#### Parameter Set

The set of all map and world parameters. It is referenced by a foundation and is immutable after phase 1, like the foundation. It is a json file, or is exported to and loaded from one.

The Parameter Set also holds the lists that are expected to change: the bedrock types, the races, the vocations, the flora and the fauna. A change to a list after phase 1 is a new Parameter Set, and with it a new foundation. Foundations that exist are not touched.

#### Derived and authored data

All generated data belongs to one of two classes:

* **Derived data** is a pure function of the foundation (seed + Parameter Set + generator version). Regions, zones, patches, rivers, lakes, heights, biomes, flora and fauna, resources, Prospects, the potential population, the travel network and all Stubs are derived. The database is only a cache of derived data: it can be evicted and recomputed with an identical result.
* **Authored data** comes from an LLM or a diffusion model. It costs to produce, so it is stored as the source of truth and is never regenerated or evicted.

#### Randomness

There is no shared random stream. Every generation step seeds itself from a hash of (foundation seed, entity identity, purpose). A region gets the same result whether it is generated first or thousandth, so generation is independent of the order the player explores in.

Where a step needs a sequence of random numbers (pick 3 of the zone cells of a region, say), a small generator is seeded from the hash of that one cell and consumed in a fixed order.

#### Identity

All structural identities are deterministic. No database generated ids appear in the domain.

* Region, zone cell and patch cell = coordinate in the grid of its level
* Zone and patch = the identity of its root cell (see Zone)
* City, faction, family and person = parent + slot index. The parent of a faction, a city and a town is its region

This lets player state and authored data point at derived data that may be evicted and recomputed, and it makes a Stub addressable before it is Realised.

#### Region generation stage

A region moves through the stages below, in the order of the table and one stage at a time. No stage is skipped: a region is Elevated before it is Planned. Stages only advance, a region never moves backwards. Evicting derived data from storage and recomputing it is not a stage change.

| Stage | Contains | Needs |
| --- | --- | --- |
| Sited | Region, zone and patch cell location, zone cells' initial elevation type and bedrock. Ocean seeds, the determined number of links, expansions and mergers | The foundation only |
| Outlined | Zones linked, expanded and merged. Sea access | All regions within the link range + 5 rings Sited (7 rings at the defaults) |
| Detailed | Patch mergers, rivers, lakes. Estimated biomes and resources | All bordering regions Outlined |
| Elevated | Height map. Final biomes and resources | All bordering regions Detailed |
| Planned | Factions and Prospects for cities and towns. The races of the region, their potential population, and the races and the name of a city or a town on each Prospect. The name of the region | The region Elevated |
| Settled | Travel network. Actual cities and towns, families and important persons | The region and all bordering regions Planned |

Sited is not a state a region is in. It needs only the seed, so it is a pure function that can be calculated for any region at any time, and it is never persisted. Stored regions start at Outlined. The Sited row describes what the function yields.

Biomes and resources exist in two forms. The estimated form is calculated in the Detailed stage with a typical height per elevation type, and the final form in the Elevated stage with the height from the height map. The flora and fauna of a patch follow its biome, so they exist in the same two forms. Lakes, Prospects and the travel network only read the estimated form, so they never depend on the height map. The potential population reads the final form. The region is still Elevated before it is Planned.

The policy for when a region advances (Detailed within some distance of the player, say) is not part of the domain model. The model only knows the stage and its preconditions.

#### Entity generation state

Cities, factions, families and persons each have their own generation state, independent of the region generation stage: Stub → Pending → Realised.

A Stub holds every gameplay-relevant fact and is derived data. Pending means the authoring by LLM or diffusion model is in flight, and on failure the entity returns to Stub. Realised adds the authored presentation only, which must not contradict the Stub. Text and portrait are tracked separately. See [docs/society-generation.md](docs/society-generation.md) section 1.2.

### The map

The geography, the climate and what the land holds. Everything in this group is derived data. The rules are in [docs/map-generation.md](docs/map-generation.md), and the reasons behind them in [docs/map-generation-research.md](docs/map-generation-research.md).

#### Region

The top level of the map, the size of a Middle Ages European kingdom (approximately 100-200 km by 100-200 km, 128 km by default). A region is divided into zones.

The region is the generation unit: rivers, lakes, the height map and Prospects are calculated one region at a time, and the region is what moves through the region generation stages. It is not fully self contained: ocean links and ocean expansion convert zone cells in any region, and a zone cell can merge into a zone of a neighbouring region and then moves to that region. So the zones of a region are the zones whose root cell the region owns, and region borders follow the merged zones.

#### Zone

The middle level of the map (32 km by default): one or more zone cells that have merged. A zone is the unit of land and ocean. It has an elevation type and a bedrock, and it is divided into patches.

A zone has the identity of its root cell, the cell the others have merged into, and it takes everything from that cell: ocean or land, the region, the elevation type and the bedrock. For gameplay a merged zone is considered to be one large cell. The merging is there to give greater variation in size and form.

#### Patch

The bottom level of the map (8 km by default): one or more patch cells of the same zone that have merged. A patch has the identity of its root cell and belongs to the zone of that cell, and like a merged zone it is considered to be one large cell for gameplay. A land patch has one biome, a few kinds of flora and fauna that fit the biome, and an amount of each resource. An ocean patch has no biome: it carries sea resources, and flora and fauna of the sea.

The three levels are nested through the patch: a point belongs to a patch, the patch to a zone and the zone to a region. The borders of the patch cells are warped to give the map a more natural look, so the coastlines and the zone and region borders run along patch borders and get the same warped shape.

#### Cell

Each of the three levels has its own grid, and every grid square holds exactly one Voronoi site. A cell is that site: a region, a zone cell or a patch cell. It is a pure function of the foundation and its coordinate never changes. The sites come from seed-hashed jittered grids, so any cell and its neighbours can be computed locally, without generating the rest of the world.

The parent of a cell is not part of its identity, it is derived: a zone cell is owned by the region that contains its site, and a patch cell by the zone cell that contains its site. The cell sizes are Parameter Set values, the counts are not. With the default sizes a region owns about 16 zone cells and a zone cell about 16 patch cells, and the exact count varies with where the sites fall. After the mergers the number of zones and patches is lower, which is an outcome to evaluate in phase 1 and not a parameter.

#### Elevation type

The authoritative elevation class of a zone: Ocean, Flat Land, Lowland, Hills or Mountains. It is drawn by configured weights per zone cell when the region is Sited, and apart from the conversion to ocean and the mergers it is final.

The elevation type is an input to river generation, and it does not give a height band. It gives how steeply the land rises from rivers and coast, the gradient of the rivers and the roughness of the surface.

#### Bedrock

The rock under a zone. It drives the mined resources of the patches of the zone. The bedrock is drawn per zone cell when the region is Sited, by configured weights that depend on the elevation type.

Bedrock will eventually be a fixed set of about 10 predetermined types. The generation currently uses nine (docs/map-generation.md Appendix F).

#### Ocean seed, link and merger

How the zone cells and patch cells of the Sited stage become the zones and patches of the map. It gives larger oceans and greater variation in size and form. There are four steps:

1. **Ocean linking.** Some of the zone cells of a region are ocean from the start: the ocean seeds. An ocean seed links to its nearest other ocean seeds within the link range, and every zone cell under the straight line between them becomes ocean, whichever region owns it.
2. **Ocean expansion.** The zone cells on a link convert some of their nearest land neighbours to ocean. The neighbours can be in another region.
3. **Zone mergers.** A number of the zone cells of each region merge into their nearest neighbour on the same side of the coast.
4. **Patch mergers.** A number of the patch cells of each zone cell merge into their nearest neighbour patch in the same zone. Ocean zones have patch mergers like land zones.

The first three steps make the region Outlined. The patch mergers are the first step of the Detailed stage, when the zones are final.

#### Sea

A connected set of ocean zones. A sea can span several regions, and an ocean seed that is not on any link gives a small enclosed sea of one zone.

A region has sea access if an ocean zone touches its land when it is Outlined. The ocean zone can belong to a neighbouring region. A region without sea access is landlocked. Whether two coasts are on the same sea is not stored as a region fact, because the world is unbounded. It is answered by a search over Outlined regions when gameplay needs it.

#### Field

A noise map that drives climate, biomes and resources: a stateless function of foundation and coordinate that can be sampled anywhere at any time. Fields are never stored and have no generation stage. A stage only records the values it sampled.

There are four Fields: Precipitation, Initial Temperature, Prosperity and Supernatural. Precipitation and Initial Temperature are warped, so their highs and lows form winding belts instead of round spots. They also have a distribution in the Parameter Set, which decides how the land is shared between the biomes. The height map uses two more noise functions of the same kind, a relief field and a detail noise.

#### River

Rivers are calculated per region, growing from the sea into the land. It is the rivers that drive the height of the landscape, not the other way round.

A river never runs on the land of another region, so region borders are watersheds. The exception is a landlocked region: its rivers cross the land of a neighbouring region to reach an ocean.

#### Lake

Lakes are placed per region after the rivers. A lake always sits on a river, so every lake has a river running in and out. There are no lakes in mountain zones, in deserts or in river-poor flat zones (dry flat zones that get few rivers).

Lakes do not change the height map. They count as fresh water for resources and Prospects.

#### Height map

The actual height of the land of a region. It is calculated in the Elevated stage from the rivers, so that the rivers run downhill, and from the elevation type of the zones. It puts a ridge on the region borders. Ocean zones are never touched by river-driven height: their depth follows the distance from the coast.

#### Biome

The kind of nature of a land patch. It comes from the temperature of the patch (the Initial Temperature field cooled by the height) and its aridity (the precipitation relative to what the temperature demands). Mangrove is an override on warm, wet and low coasts. Volcano is an override on rare patches of the mountains, whatever their climate: about one region in nine has one.

Biomes will eventually be a fixed set of 10-20 predetermined biomes. The current set, not binding yet, is 15 land biomes and the two overrides, Mangrove and Volcano:

Ice & snow, Tundra, Cold steppe, Boreal forest, Cold desert, Temperate grassland, Woodland & shrubland, Temperate mixed forest, Temperate broadleaf forest, Temperate rain forest, Hot desert, Thorn scrub, Savanna, Tropical dry forest, Tropical rain forest, Mangrove, Volcano

#### Flora and fauna

The plants and the creatures of a patch. Flora has eleven types: Tree, Shrub, Flower, Herb, Fruit, Grass, Vine, Fungus, Moss, Aquatic and Arcane. Fauna has four: Critter, Animal, Beast and Monster. A kind is one entry of either: the oak, the grey wolf, the griffin.

The flora and the fauna are two lists in the Parameter Set, not fixed sets in the code. Each kind has an id, a name, a type, a toggle, the biomes it lives in (or the sea), a frequency and the Supernatural it needs. A kind of flora can also stand in for a type: a patch that gets it gets no kind of that type. The default lists hold 208 kinds of flora and 167 kinds of fauna. Only the arcane plants and the monsters need the Supernatural Field, so they are found where it is high. One arcane plant, the giant mushrooms, stands in for the trees: a rare patch of the forests where the Supernatural is high has mushrooms of tree height and no trees.

A patch gets a few kinds of each (3 to 6 by default), drawn by frequency from the kinds that fit it, when its biome is chosen. The flora and fauna of a patch do not change its resources yet.

#### Resource

What a patch holds for people to use. The generation gives every patch an amount for each of 27 raw resources, and a variant of each: rye is a variant of grain. One of them, the arcane materials, lies only where the Supernatural Field is high. A potential is what the land can yield. A deposit is a rare find that often needs to be mined.

Resources are split in groups to make trade and needs manageable: Food, Materials, Fuel, Luxuries, Metals and Animals are found in patches, and Building, Tools, Gear, Weapons and Clothing are only produced. A product is produced from resources and is not generated with the map. Every generated resource has an abbreviation of three letters that no other resource has, with a capital at the start of each word: Grn is grain and IrO iron ore. The groups, the abbreviations and the catalogue of variants are in docs/map-generation.md Appendix G.

#### Prospect

A potential location for a city or a town, with information about its resources, its travel options and so on. Every land patch of a region gets a score, and the best patches become the Prospects of the region (20 by default, of which up to 4 are for cities), with a minimum spacing between them. A Prospect never touches the land of another region.

A Prospect is Rich, Average or Poor by its score. The sum of the scores of a region decides how many of its Prospects are on show: all of them in the best regions, a single one in the poorest. Only the Prospects on show are drawn on a map and connected by the travel network, and only they get a city or a town.

#### Travel network

Roads, river routes and sea lanes that connect the Prospects on show. The network of a region is calculated from the Prospects of the region and its bordering regions. A link between Prospects in two regions is decided by exactly one region, so the network does not depend on the order the regions are generated in.

### The society

The peoples that live on the map. The Stub of every entity in this group is derived data, and its presentation is authored. The rules are in [docs/society-generation.md](docs/society-generation.md). Only the races, the potential population, the factions and the cities and towns are specified there so far. Detailing the rest is a later task.

#### Race

A people that can live in the world: Northmen, Elves, Dwarves and so on. Races are needed to generate factions, families and persons. Race is the only level: a race has no sub-races, and each human people is a race of its own, so there is no plain human race.

The races are a list in the Parameter Set, not a fixed set in the code. Each race has an id, a name, a toggle (on or off), a mark for a human race, a name style (which of the three name models its names come from), a frequency for each biome, a height modifier, a leaning to each type of faction, two lists of other races (the races it appreciates and the races it hates) and a list of favourite colours. The default list holds 28 races, all on: seven human races (Northmen, Midlanders, Saltroad folk, Sunlanders, Grass riders, Rainkeepers, Canal folk), five classic races (Elves, Dwarves, Halflings, Gnomes, Orcs) and sixteen new near-human races (docs/society-generation.md Appendix A). A faction has a lead race. How a family or a person gets its race is not decided yet.

#### Main race and minority race

A region does not hold every race. It holds up to six: four main races and two minority races. The main races are the largest by how well the land of the region suits them. A minority place goes to the race whose turn it is in the region. The turns follow a pattern over the region grid that gives every race a turn in every cluster of about 30 regions, so a race that is nowhere large can still be met within reach. A city holds up to five of the races of its region and a town up to four. All these numbers are Parameter Set values.

#### Potential population

An estimate of how many persons the land of a region can feed, for each race of the region. It is not a count of people that exist: cities, families and persons come in the Settled stage, where the potential population becomes the persons of the cities, the towns and the land.

Every land patch has a capacity that comes from its area, its food resources and the Prosperity field. The races of the region share it by their frequency in the biome of the patch and their height modifier at its height. So the races compete for the same land, and toggling a race off leaves its share to the others. A minority race is lifted towards a twentieth of the region, so that it is large enough to be met. A patch where no race of the region lives holds nobody. The potential population is calculated in the Planned stage.

#### Faction

A group of people in one region: a political grouping. It could be a kingdom, a thieves' guild or a tribe. Factions belong to the Planned stage, and the identity of a faction is its region and a slot.

The type of a faction is what binds its members. There are six: Family (a house and its household), Race (a people), Vocation (a trade), Belief (a faith), Magic (the arcane) and Place (a town or a valley that rules itself). The form is the name of a type in a role: a Family that rules is a Crown, a Vocation under the ruler is a Guild. It is a label for the authoring, and it is race-neutral: a crown of the Northmen is a jarldom.

A faction has a lead race, the race of its leaders. A faction of the type Race holds that race only. Any other faction is open to the races of its seat. The seat is the Prospect the faction sits on, and the seat of the ruling faction is the capital of the region.

A faction has a main colour and a secondary colour: the field and the device of its banner. They are drawn mostly from the favourite colours of its lead race, and the secondary colour is one that stands out against the main one.

#### Ruling faction, opposition and lesser faction

The role of a faction is how it stands in its region. Every region with persons has exactly one ruling faction, which governs it. A region can have one opposition, which wants the rule or wants out of it, and up to four lesser factions, which live under the ruler: a guild, an order, a circle, a great house, a chartered town or an enclave.

The type and the lead race of the ruling faction are drawn from how well the land suits each type and how much each race of the region leans to it: grain land with many persons gets a house, open herding land a people, trading towns the guilds, supernatural land a faith. A region has an opposition when its tension is high: another kind of ruler would fit it as well, its land is rough, it has a second city, precious finds or poor land. The ground of an opposition is the largest of these. The lesser factions follow the resources of the region and its persons. The grip of a faction says how firmly it stands: the ruling faction has what the tension leaves, the opposition the tension.

#### Region name

The name of a region is the name of the realm of its ruling faction: Western Watson Kingdom, New Sumerian State, Icag. It belongs to the Planned stage, and a region without a ruling faction has no name. The name is not authored: it comes from the region-name generator, so a region has its name as soon as it is Planned.

A name is a prefix, a middle and a suffix from weighted word lists, and the prefix and the suffix can be missing. The lead race of the ruling faction picks the lists: the human races have theirs and the other races theirs. A region with few persons can be a Duchy or a County and one with many can be Grand or Great. A ruling faction of the type Family is a house: only a house rules a Kingdom or an Empire, and its surname can be the middle of the name. The surname comes from the name model of the lead race.

#### City and town

An actual settlement on a Prospect. Cities and towns belong to the Settled stage. A city can exist with a population count long before any individual in it is authored.

A city or a town stands on every Prospect on show, and on no other: a city on a Prospect for a city, a town on a Prospect for a town. The capital of the region, the seat of its ruling faction, is always a city, whatever its Prospect is for. The identity of a city or a town is its region and a slot, the place of its Prospect among the Prospects of the region.

The potential population of a region becomes its persons: a share of them lives in its cities, a share in its towns and the rest on the land. The two shares are Parameter Set values, a tenth each by default. The cities of a region share their persons by rank, and so do the towns: the capital is the first, then the other cities, then the towns, and a place weighs one over its rank. No city or town stands empty where the region has a person for it.

A city or a town holds some of the races of its region: the largest around its Prospect, and a last one that is drawn with the minority races weighted up. Which races a city or a town on a Prospect holds is worked out in the Planned stage, with the Prospects. Its persons are shared between these races by how many of each live around it, so every person of a region has a race and lives in a city, in a town or on the land.

A city or a town has a name: Jarlhavn, Khar Nuur, Qasr Mehret. The name is not authored: it comes from the city and town name generator, as the name of a region comes from its generator. A name is a first part and a place word. One of the races of the place names it, drawn by how many of each race live around it, and the capital is named by the lead race of the ruling faction. The race gives the words, each race from word lists of its own, and the site gives the kind of place word: a harbour, a ford, a hill, a city or a town. The name is worked out in the Planned stage for every Prospect, with its races. So a Prospect has its name before a city or a town stands on it, and no two cities or towns of a region have the same name.

#### Family

A family of NPCs, typically living together in a household. It can include servants or slaves.

#### Person

An NPC in the game. The Stub of a person holds name, age, sex, vocation, role, family links, faction membership and home. The name is not authored: it comes from the name generator, so a person has a name already as a Stub. The textual description, the history and the portrait are authored.

#### Vocation

The work a person does: farmer, smith, priest. A person has one vocation and can have a by-work, a second vocation beside it. The vocation is not the role: the role is the position of the person in the household, in the vocation or in a faction. The vocations are a list in the Parameter Set, like the races. Each vocation has an id, a name, a toggle, a sector (one of nine), the lowest tier of place it is found in (countryside, town, city or capital), a starting share of the labour force and its variants: the shepherd is a variant of the herder. The default list holds 51 vocations. How a place and a person get their vocations is not specified yet, so no vocation is generated.

#### Looks

What a portrait of a person must show: the body and the face, the hair, the outfit and the accessories. Looks are not specified yet, apart from the colours.

#### Colour

A named colour of cloth, paint and banners: red, indigo, saffron. The colours are a list in the Parameter Set, like the races. Each colour has an id, a name and a value as #RRGGBB. The default list holds 24 colours. A race has favourite colours, and a faction has a main colour and a secondary colour. The colours of hair, skin and eyes are not in the list.

## Documentation

| Document | What it holds |
| --- | --- |
| [docs/map-generation.md](docs/map-generation.md) | The rules of the map generation, section by section, with the data tables as appendices |
| [docs/society-generation.md](docs/society-generation.md) | The rules of the society generation, with the default race list as an appendix |
| [docs/map-generation-research.md](docs/map-generation-research.md) | Why a rule is as it is, what was weighed against it and what is still open |
| [docs/default-parameter-set.json](docs/default-parameter-set.json) | The default Parameter Set |
| [docs/parameter-set.schema.json](docs/parameter-set.schema.json) | The JSON Schema of a Parameter Set file. Generated, do not edit |
| [docs/adding-a-race.md](docs/adding-a-race.md) | The steps to add a race |
| [docs/races-basis.md](docs/races-basis.md) | The 28 default races: looks, history, way of life, vocations and factions |
| [docs/tools-basis.md](docs/tools-basis.md) | 146 real tools for the 51 default vocations: a sourced description, lore and the fitting vocations. Material, it binds nothing |
| [docs/city-and-town-names.md](docs/city-and-town-names.md) | 50 invented city and town names for each of the 28 default races, and what the names of each race are built from. Material: the word lists of the city and town name generator are built from it |
| [docs/races-and-worldbuilding.md](docs/races-and-worldbuilding.md) | A framework for building a near-human race: origin, anchor traits, culture and the relation to humans |
| [docs/NameGenerator-Reuse.md](docs/NameGenerator-Reuse.md) | The person-name generator |
| [docs/RegionNameGenerator-Reuse.md](docs/RegionNameGenerator-Reuse.md) | The region-name generator of an earlier project: the original and the corrected form that names the regions here |
| [docs/research/](docs/research/) | 22 source notes: the map notes behind the research document, and research for the society and the living world (races, human races, factions, faction generation, vocations, portrait components, heraldry, flora and fauna, city and town naming), and three notes on sequential art for graphic short stories |
| [docs/plans/](docs/plans/) | Plans for changes, each with its status |
| [braindump.md](braindump.md) | Notes and ideas in progress, not decided |

## Build and run

The code is one .NET solution, `Gameworld.slnx`. It needs the .NET 11 SDK and nothing else: no database and no external services. The NuGet packages of the test project are restored on the first build.

| Project | What it is |
| --- | --- |
| `src/Gameworld.Domain` | The domain model of this readme |
| `src/Gameworld.Generation` | The generation algorithms of [docs/map-generation.md](docs/map-generation.md) and [docs/society-generation.md](docs/society-generation.md), and the map renderer |
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

Without options it takes the 3 × 3 regions around region (0, 0) of the seed `cellgen` to the Settled stage with the default Parameter Set, and writes to `out/`. It prints a line per region: its stage, the counts of zones, patches, rivers, lakes, Prospects, persons, factions, cities, towns and travel links, the time it took, the name of the region and the name of its capital. Options go after `--`:

```bash
dotnet run --project src/Gameworld.MapGenerator -c Release -- --seed atlas --region 2,2 --rings 2 --out out/atlas
```

| Option | Meaning | Default |
| --- | --- | --- |
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

Every parameter has a description: what it does, its unit and the section of [docs/map-generation.md](docs/map-generation.md) or [docs/society-generation.md](docs/society-generation.md) it belongs to. The descriptions are written once, on the `ParameterSet` types in `src/Gameworld.Domain/ParameterSet.cs`, and [docs/parameter-set.schema.json](docs/parameter-set.schema.json) is generated from them as a JSON Schema, together with the default, the range and the allowed values of each parameter.

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

Then open <http://localhost:5062>. Drag to pan, scroll to zoom, and click the map to see the region, zone and patch at that point. The side panel switches the base map (elevation types, biomes, height, or none) and the layers, among them the four Fields, and it replaces the foundation: a new seed, or an edited Parameter Set under "Parameter Set (json)". Replacing the foundation discards all derived data.

"Under the pointer" has a section for the region, one for the zone and one for the patch, each with groups that fold away. The region section shows the stages the region has reached, with a button to take it to each later stage. A row of a Prospect, a city or a town moves the pointer to it. A Prospect, a city or a town shows its name, the race that named it and what the place word says about its site. "Statistics" counts the current view: the land, the biomes and the climate, the heights, the water, the resources, the flora and fauna, and for the regions that are Planned or Settled the Prospects, the potential population, the factions and the cities and towns.

The Prospects layer draws the Prospects on show and writes the name of the city or town of each beside it: the capitals first, then the other cities and the towns as you zoom in.

Regions are generated when they come into view, so the first view of an area takes longer than the next. The Prospects and travel layers take the regions through the Elevated stage first, which builds their height maps.
