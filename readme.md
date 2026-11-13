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
#### Region
The map is structured in large regions the size of Middle Age European kingdoms (approximately 100-200km x 100-200km). The regions are divided into zones (approximately 8-12 zones per region). The zones are divided into patches (approximately 8-18 patches per zone).

A region is a generation unit and it should be self contained. It should be possible to fully generate a region if all bordering regions are partly generated. A region could be partly generated without any outside info.

#### Zone
Zones have an elevation type: Ocean, Flat Land, low Land, Hills or Mountains

#### Patch
Patches have one biome associated with them. The biome drives the patch flora and fauna.

#### Biome
Biomes will eventually be a fixed set of predetermined biomes (10-20). Geography and climate will drive biomes.

#### Rivers
Rivers are calculated with a space-colonization algorithm, and it is the rivers that drive the height.

#### Preconditions
Perlin Noise Maps that are used to drive climate, biomes and resources
* Precipitation
* Initial Temperature
* Prosperity
* Supernatural

#### Resource
##### Groups
* Food 
* Materials
* Fuel
* Luxuries
* Animals
* Building
* Tools
* Gear
* Weapons
* Clothing
##### Raw or Processed
##### Potentials or Deposits 
##### variants


#### Product
     
#### Prospect 
    
#### Faction
#### Family
#### Person

