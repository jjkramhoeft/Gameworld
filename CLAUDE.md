# Gameworld

[readme.md](readme.md) is the domain model and the source of every name. Read it first.

## Docs

- [docs/map-generation.md](docs/map-generation.md): the generation rules. Read the section for the layer at hand; each section states its region generation stage. Sections 1 and 3-8 hold the structure. Section 19 is the races and the potential population. Appendices B-D are crop and resource data tables, and Appendix E is the default race list.
- [docs/default-parameter-set.json](docs/default-parameter-set.json): the default Parameter Set, grouped by region generation stage. map-generation.md quotes the same defaults in its text; change both together.
- [docs/parameter-set.schema.json](docs/parameter-set.schema.json): generated, do not edit. It is the JSON Schema of a Parameter Set file: the description, default and range of every parameter. The descriptions live in `[Description]` attributes in `src/Gameworld.Domain/ParameterSet.cs`; give every new parameter one, then regenerate with `dotnet run --project src/Gameworld.MapGenerator -c Release -- --export-schema docs/parameter-set.schema.json`.
- [docs/map-generation-research.md](docs/map-generation-research.md): why a rule is as it is, what was weighed against it and what is still open. Read it when a rule is questioned or changed.
- [docs/NameGenerator-Reuse.md](docs/NameGenerator-Reuse.md): the person-name generator, a port from an earlier project (IdleGame; the doc's links into that repo do not resolve here). The code is `src/Gameworld.Generation/Names`. Its nine models are the files of `pregenerated-data/`, embedded in the Generation assembly; never re-sort or regenerate them, the names of a seed depend on their exact content.
- [docs/research/](docs/research/): the ten source notes behind the research doc, 560 KB in all. Read a note only when the user asks for it, or when the research doc cites a section whose detail you need. The notes use the names of an earlier prototype: elevation zone = Zone, zone type = elevation type, lattice = grid, location = Prospect.

## Authority

The readme decides the domain model and the names. map-generation.md decides the generation rules. When the two disagree, report it to the user before changing either.

## Code

.NET 11, one solution: `Gameworld.slnx`.

- `src/Gameworld.Domain`: the domain model of the readme. No generation logic. `ParameterSet.Default` must equal docs/default-parameter-set.json (a test checks it).
- `src/Gameworld.Generation`: the algorithms of map-generation.md, one folder per layer. `World` is the entry point: it holds one foundation and computes and caches every layer on demand.
- `src/Gameworld.MapGenerator`: the map generation app (console). `dotnet run --project src/Gameworld.MapGenerator -c Release -- --help`. It writes maps and regions to `out/`.
- `src/Gameworld.Demo`: the demo app (web map viewer). `dotnet run --project src/Gameworld.Demo -c Release`, then open http://localhost:5062.
- `tests/Gameworld.Tests`: `dotnet test -c Release`.

Generation code must stay a pure function of the foundation: every random choice goes through `Hashing` with the purpose number the doc gives, and no step may depend on the order regions are generated in (a test checks that too).
