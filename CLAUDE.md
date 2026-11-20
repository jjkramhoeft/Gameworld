# Gameworld

[readme.md](readme.md) defines every concept of the domain model in a few lines and is the source of every name. Read it first. The detail of a concept is in one of the two generation docs: the map or the society.

## Docs

- [docs/map-generation.md](docs/map-generation.md): the generation rules of the map (geography, climate, biomes, flora and fauna, resources, Prospects, travel network). Read the section for the layer at hand; each section states its region generation stage. Sections 1 and 3-8 hold the structure. Appendices B-D are crop and resource data tables, F is the bedrock types and G the resource groups and catalogue. Section 19 and Appendix E are only pointers: the races moved to society-generation.md.
- [docs/society-generation.md](docs/society-generation.md): the generation rules of the society (races, potential population, factions, cities and towns, families, persons, vocations, looks). Only sections 2, 3, 4 and 5.1 are specified and implemented: the races, the races of a region with their potential population, the factions of a region (a ruling faction, sometimes an opposition and lesser factions), and the races of a city or a town. The vocations of section 8 are a list in the Parameter Set and are not drawn yet. The rest of sections 5-9 holds what is decided and what is open. [docs/plans/region-factions.md](docs/plans/region-factions.md) holds the test behind the faction rule and what was weighed, and [docs/research/faction-generation.md](docs/research/faction-generation.md) its sources. Appendix A is the default race list: 28 races, with no plain human race and no sub-races. [docs/races-basis.md](docs/races-basis.md) describes each of them (looks, history, way of life, vocations, factions) and holds the measured shares. Appendix B lists the other society docs.
- [docs/default-parameter-set.json](docs/default-parameter-set.json): the default Parameter Set, grouped by region generation stage. map-generation.md and society-generation.md quote the same defaults in their text; change them together.
- [docs/parameter-set.schema.json](docs/parameter-set.schema.json): generated, do not edit. It is the JSON Schema of a Parameter Set file: the description, default and range of every parameter. The descriptions live in `[Description]` attributes in `src/Gameworld.Domain/ParameterSet.cs`; give every new parameter one, then regenerate with `dotnet run --project src/Gameworld.MapGenerator -c Release -- --export-schema docs/parameter-set.schema.json`.
- [docs/map-generation-research.md](docs/map-generation-research.md): why a rule is as it is, what was weighed against it and what is still open. Read it when a rule is questioned or changed.
- [docs/adding-a-race.md](docs/adding-a-race.md): the steps to add a race, to one world through a Parameter Set file or to the default list, and every place that must change with the default list.
- [docs/tools-basis.md](docs/tools-basis.md): 146 real tools for the 51 default vocations, each with a sourced description, invented lore and the fitting vocations. Material only: tools are not a concept of the readme and no generation rule reads the file. 30 entries are marked partially verified.
- [docs/NameGenerator-Reuse.md](docs/NameGenerator-Reuse.md): the person-name generator, a port from an earlier project (IdleGame; the doc's links into that repo do not resolve here). The code is `src/Gameworld.Generation/Names`. Its nine models are the files of `pregenerated-data/`, embedded in the Generation assembly; never re-sort or regenerate them, the names of a seed depend on their exact content.
- [docs/research/](docs/research/): 21 source notes, about 1.7 MB in all. Ten map notes (2026-11-07 to 11-12) are compiled in the research doc. Eight society notes (2026-11-14 to 11-19) are read directly by society-generation.md, the plans and the basis docs: races, human races, factions, faction generation, vocations, portrait components, heraldry, and flora and fauna. Three notes on comics craft do not feed the generator. Read a note only when the user asks for it, or when a doc cites a section whose detail you need. The ten map notes use the names of an earlier prototype: elevation zone = Zone, zone type = elevation type, lattice = grid, location = Prospect.

## Authority

The readme decides the domain model and the names. map-generation.md decides the generation rules of the map, and society-generation.md those of the society. When two of them disagree, report it to the user before changing either.

## Code

.NET 11, one solution: `Gameworld.slnx`.

- `src/Gameworld.Domain`: the domain model of the readme. No generation logic. `ParameterSet.Default` must equal docs/default-parameter-set.json (a test checks it).
- `src/Gameworld.Generation`: the algorithms of map-generation.md and society-generation.md, one folder per layer. `World` is the entry point: it holds one foundation and computes and caches every layer on demand.
- `src/Gameworld.MapGenerator`: the map generation app (console). `dotnet run --project src/Gameworld.MapGenerator -c Release -- --help`. It writes maps and regions to `out/`.
- `src/Gameworld.Demo`: the demo app (web map viewer). `dotnet run --project src/Gameworld.Demo -c Release`, then open http://localhost:5062.
- `tests/Gameworld.Tests`: `dotnet test -c Release`.

Generation code must stay a pure function of the foundation: every random choice goes through `Hashing` with the purpose number the doc gives, and no step may depend on the order regions are generated in (a test checks that too).
