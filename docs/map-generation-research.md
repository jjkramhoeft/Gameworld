# Map generation research

*Compiled 2026-11-14 from the ten research notes in [research/](research/) (research dates 2026-11-07 to 2026-11-12).*

This document combines the ten research notes into one text, ordered the way the map is built: cells, oceans, rivers, height, fields, biomes, resources, Prospects for cities and towns, travel network. For each layer it gives what the research found, which alternatives were weighed, what the generation adopted, and what is still open.

It is the companion of [map-generation.md](map-generation.md). That document says **how** the map is generated, step by step. This one says **why** it is generated that way and what else the research has in store. Topics in the notes that the generation does not use (cellular-automaton caves, Wave Function Collapse, hex grids, noise-line networks) are only summarised in [§4.6](#46-surveyed-but-not-used).

The names in this document follow the [readme](../readme.md). The notes themselves were written for an earlier prototype called Sandbox 4 and keep its names: "elevation zone" there is Zone here, "zone type" is elevation type, "lattice" is grid, and "location" is Prospect. Where the notes point at source files, they mean the files of that prototype.

## Contents

1. [The source notes](#1-the-source-notes)
2. [Summary: decisions and their research](#2-summary-decisions-and-their-research)
3. [Constraints that shaped every choice](#3-constraints-that-shaped-every-choice)
4. [Cells: the base map](#4-cells-the-base-map)
5. [Rivers: space colonization](#5-rivers-space-colonization)
6. [Height map: terrain fitted to the rivers](#6-height-map-terrain-fitted-to-the-rivers)
7. [Tiles and cost](#7-tiles-and-cost)
8. [Climate and biomes](#8-climate-and-biomes)
9. [Resources](#9-resources)
10. [Prospects for cities and towns](#10-prospects-for-cities-and-towns)
11. [Travel and trade](#11-travel-and-trade)
12. [Research not yet used: a backlog](#12-research-not-yet-used-a-backlog)
13. [Open questions](#13-open-questions)
14. [Numeric quick reference](#14-numeric-quick-reference)
15. [Sources](#15-sources)

**How to read this document**

- **Citations.** A reference such as (HEIGHT §3.2) points to a section of a source note; the keys are in §1. The notes hold the full citations (paper, section, figure, or repository, file, lines and commit). §15 lists the main external sources by topic.
- **Sourced or design.** The notes separate sourced claims from engineering judgement, which they mark "Design note" or "Recommendation". This document keeps that separation: a paragraph that begins **Design** is the notes' own judgement, not a sourced fact.
- **Verification.** The notes mark claims they could not check against a primary text as *[unverified]* or *[partially verified]*. Where such a claim carries weight here, the mark is repeated.
- **Status.** Each layer ends with a status table. **Adopted** means the generation does it, as [map-generation.md](map-generation.md) describes. **Partly** means a simpler form is adopted. **Open** means the research is there and the generation does not use it. The status says nothing about what is adopted in code.
- **Numbers drift.** The notes were written against earlier versions of the generator. Several defaults have changed since (river step and radii, the terrain halo, the precipitation field). This document gives the current value and says so where it differs from the note.

---

## 1. The source notes

| Key | Note | Date | Covers | Used in |
|---|---|---|---|---|
| CELLS | [procedural-cell-generation.md](research/procedural-cell-generation.md) | 2026-11-07 | First survey: Voronoi and Delaunay partitions, grids and chunks, hashing, cellular automata, Wave Function Collapse, noise, erosion, and a recommended architecture | §3, §4 |
| GAPS | [readme-topics-gaps.md](research/readme-topics-gaps.md) | 2026-11-07 | Second pass: space colonization, distance functions, line networks from noise, deterministic noise in the browser, GitHub Pages delivery | §3, §4, §5 |
| RIVERS | [space-colonization-branch-variants.md](research/space-colonization-branch-variants.md) | 2026-11-07 | 22 techniques that change the branching and look of space-colonization rivers, each with a hook in the river code | §5 |
| HEIGHT | [heightmap-from-biomes-and-rivers.md](research/heightmap-from-biomes-and-rivers.md) | 2026-11-07 | Methods that fit a height map to an existing map and river network; three pipelines (A, B, C) | §6 |
| TILES | [sandbox4-tiling.md](research/sandbox4-tiling.md) | 2026-11-07 | Generating the map tile by tile; measurements; required changes | §7 |
| BIOMES | [biomes-and-climate.md](research/biomes-and-climate.md) | 2026-11-08 | Biome schemes (Whittaker, Holdridge, Köppen, WWF, IUCN), aridity, lapse rates, treeline, azonal biomes; a classifier design | §8 |
| CHARTS | [biome-climate-charts.md](research/biome-climate-charts.md) | 2026-11-12 | A catalogue of temperature × precipitation charts and rule tables, with data availability and licences | §8 |
| RES | [patch-resources.md](research/patch-resources.md) | 2026-11-10 | A catalogue of about 370 natural resources and 28 fantasy resources keyed to the map's fields; missing map features; a bedrock attribute; generation rules | §9 |
| TRADE | [game-resources-and-trade.md](research/game-resources-and-trade.md) | 2026-11-10 | How games choose, place and trade resources; trade theory; a 39-resource set; price and flow models | §9, §11 |
| SITES | [settlement-siting.md](research/settlement-siting.md) | 2026-11-10 | Where settlements form; suitability methods; how generators and games place cities; a siting design | §10, §11 |

Three later notes are used in §10.7 and §10.8 only. They are not part of the compilation above, and they follow the names of the readme:

| Key | Note | Date | Covers | Used in |
|---|---|---|---|---|
| RACES | [races.md](research/races.md) | 2026-11-14 | The peoples that RPGs, fantasy settings and strategy games use again and again, as 41 race archetypes; a race × biome and race × terrain matrix; what a generator needs per race | §10.7 |
| FACTIONS | [factions.md](research/factions.md) | 2026-11-14 | 48 faction archetypes from RPGs, strategy games, tabletop settings and history; the axes that tell them apart; their rivalries and alliances | §10.8 |
| FACGEN | [faction-generation.md](research/faction-generation.md) | 2026-11-19 | How generators and games compute the kind of ruler of a land; the scholarship on what decides a ruler, an opposition and guilds | §10.8 |

---

## 2. Summary: decisions and their research

| Layer | What is adopted | Research basis | Main alternatives weighed |
|---|---|---|---|
| Cells | Three nested levels of jittered-grid Voronoi cells (128, 32 and 8 km), looked up without a mesh | Worley's hashed feature points and Azgaar's jittered grid give "infinite Voronoi" (CELLS §3.3) | Poisson-disc points with Delaunator; Lloyd relaxation; hex or square grids |
| Randomness | `hash(seed, purpose, i, j)` from pcg3d and a MurmurHash3 finaliser; sfc32 for sequences | Jarzynski & Olano 2020; exactly specified IEEE arithmetic (CELLS §3.3, GAPS §4) | Stateful RNG; shader-style `sin` hashes; an unseeded platform generator |
| Noise | In-house simplex and Perlin with hashed gradients, fBm, ridged; warp on the query point | Gustavson, Perlin, Quilez (CELLS §6, GAPS §3.3, §4.3) | `simplex-noise` v4; OpenSimplex2 (no JS port, 64-bit hash) |
| Rivers | Space colonization per region, grown upstream from the sea, before any terrain | Runions et al. 2005, 2007; Palubicki et al. 2009 (GAPS §1, RIVERS) | Flow accumulation on a heightfield with depression filling; noise isolines |
| Height | River beds by the χ integral of the stream-power law; land as a geodesic envelope over beds and coast; ridges; burn; Priority-Flood ("Option B") | Mudd et al. 2014; Génevaux et al. 2013; Hellweger & Maidment 1997; Barnes et al. 2014 (HEIGHT) | Height first and rivers from it; diffusion; pinned stream-power simulation |
| Tiles | Height map in tiles of about 256 km with a 24 km halo | Measurements on the prototype (TILES) | Per-region rasters; one fixed 4 × 4 tile size |
| Climate | Two independent Perlin fBm fields, each domain-warped into belts: temperature (°C) and precipitation (mm/yr, log scale) | The schemes all use temperature and precipitation, never relative humidity (BIOMES §3.4) | Latitude bands; wind and rain sweep; a humidity field |
| Biomes | Thermal belt × aridity class, with aridity m = P / (20·T + 140) and height cooling; mangrove override | Köppen's dryness threshold; Holdridge's belts; Whittaker as cross-check (BIOMES §7.3, CHARTS §8) | Whittaker lookup grid; Holdridge hexagons; full Köppen rules |
| Resources | 27 raw resources per patch: potentials from crop trapezoids and biome tables, hashed deposits on a bedrock type per zone, river placers | FAO ECOCROP; USGS deposit models; game survey (RES §7, TRADE §5) | One good per zone; climate-only deposits |
| Prospects | About 20 scored Prospects for cities and towns per region from five factors, selected inside the region | Break of bulk, navigable rivers, central places, Hansen accessibility (SITES §2–§6) | Global greedy placement as in Azgaar and O'Leary; one centre per grid square |
| Travel | Roads, river routes and sea lanes on a 2 km cost grid; links between travel-cost neighbours, pruned by detour | Water is far cheaper than land; a fixed charge per mode change (TRADE §4.2, SITES §3.3, §6.8) | Straight-line graphs; a patch-graph model |

Two decisions run through all of it:

1. **Rivers come before height.** Almost every published generator builds height first and derives rivers from it. The generator grows the rivers first and fits the terrain to them (HEIGHT §1). That removes depression handling from the river step and guarantees that the drawn rivers are the valleys.
2. **The region is the unit of work.** Rivers never enter another region's land. Every non-landlocked region is therefore a closed drainage unit, its land border is a divide, and any step that looks global (filling, routing, selection) can run per region with a bounded halo (HEIGHT §1, SITES §1).

---

## 3. Constraints that shaped every choice

The map is unbounded, generated on demand, the same in any order of exploration, and bit-identical on every machine. The notes test every method against these demands. HEIGHT states them as five criteria, which the later notes reuse:

| Key | Question |
|---|---|
| **C1 Local** | Can it be computed per region or tile with a bounded halo, so the lazy map stays order-independent and seamless? |
| **C2 Downhill** | Does every river run monotonically downhill to the sea? If not, how is it repaired? |
| **C3 Cost** | What does it cost at about 100,000 river nodes, and per pixel when rendering? |
| **C4 Bit-exact** | Can it be built from IEEE `+ − × ÷ √` only? |
| **C5 Height after rivers** | Does it work when height is derived from the rivers and not the other way round? |

### 3.1 Pure functions of position

- **Chunked worlds need pure content.** A chunk must be a pure function of the world seed, its coordinate and any coarser global data it reads (CELLS §3.2).
- **Neighbour-dependent steps break that.** Smoothing, erosion, flow and constraint solving read their neighbours. The survey lists three fixes (CELLS §3.2):
  - (a) compute with a **halo** of extra cells and keep only the interior;
  - (b) run in **phases**, where phase k of a chunk may read phase k − 1 of its neighbours;
  - (c) push long-range processes into a **coarser layer** that chunks only sample.
- **Avoid order-dependent seam repair.** Johnson et al.'s infinite caves smooth across a new chunk and its neighbours, which makes the result depend on generation order. A generator that wants exact reproducibility should not do that (CELLS §3.2).
- **What the generator uses.** All three fixes appear: the height map raster has a 24 km halo (a); the oceans are built in phases, ocean seeds, then links, then expansion, each reading only the phase before (b); regions are the coarse layer that owns rivers (c).

### 3.2 Randomness from hashes

- **Hash the coordinates.** To generate any cell independently, draw its random numbers from `hash(seed, stage, x, y)`, not from a sequential generator (CELLS §3.3).
- **Which hash.** Jarzynski & Olano tested over 100 hash variants for quality and speed and name pcg3d and pcg4d for N-D input and output. They warn that generators which are good sequentially "fall short" when used in parallel with neighbouring seeds, which is exactly the per-cell case (CELLS §3.3).
- **Worley already did this.** Each unit cube's feature points come from a generator seeded by the hashed cube id, with no tables (CELLS §3.3, §6.3).
- **Sequential stages.** Dart throwing, attractor placement and shuffles are inherently sequential. For those, seed a small PRNG per cell. GAPS §4.2 compares five and recommends **sfc32 seeded with four words from SplitMix32** applied to the stage hash. sfc32 passes PractRand and BigCrush per bryc's notes; Mulberry32 is not equidistributed (about a third of the 32-bit outputs never occur).
- **The generator.** `hash4(seed, purpose, x, y) = fmix32(pcg3(x, y, seed XOR purpose·0x9e3779b9))`, with one constant per purpose, and sfc32 seeded per region for sequences, with 12 outputs discarded as in the original seeding.

### 3.3 Bit-exact arithmetic

The same foundation must give the same map on every machine. The notes worked the rules out for one platform (GAPS §4.1), and the conclusion holds in general:

- IEEE 754 add, subtract, multiply, divide and square root are exactly defined. They give the same bits everywhere, as long as nothing fuses `a*b + c` into one operation or keeps extended precision.
- The transcendental functions are not: `sin`, `cos`, `exp`, `log`, `pow`, `atan2`, `hypot` and `cbrt` may differ in the last bit between implementations.

Rules that follow, used throughout the notes:

| Rule | Reason | Source |
|---|---|---|
| No platform `sin`, `cos`, `exp`, `log`, `pow` or `hypot` in generation code | Results may differ between machines | GAPS §4.1 |
| Hash arithmetic in 32-bit unsigned integers with wrap-around | The low bits a hash depends on must not be lost | GAPS §4.1 |
| No shader-style `fract(sin(x)·43758.5453)` hash | Uses `sin` | GAPS §4.1 |
| Angles as cosine literals or `√` constructions: cos 30° = √3/2, cos 45° = √½, cos 60° = ½, cos 72° = (√5 − 1)/4 | Avoids trig | RIVERS, header |
| Rotation by the rational form `c = (1 − t²)/(1 + t²)`, `s = 2t/(1 + t²)`, with `t = tan(θ/2)` as a literal | Avoids trig | RIVERS, header |
| Dyadic exponents by nested roots: `A^−7⁄16 = 1/(⁴√A · ⁸√A · ¹⁶√A)` | Avoids `pow` | HEIGHT §3.2 |
| Logarithmic axes as comparisons against powers of 2, or as products compared with a constant | Avoids `log` | BIOMES §7.4 |
| Seasonal cycles from a literal table of 12 cosine constants | Avoids `cos` | BIOMES §7.2 |
| Tobler's walking speed with a deterministic `exp`, or as `6 / (1 + a + a²/2 + a³/6 + a⁴/24)` | Avoids `exp` | SITES §3.3 |
| Heap ties break on the cell index; sums run in a fixed order | Float sums and equal keys depend on order | HEIGHT §4.1, SITES §5.3 |
| Keep generation off the GPU | GPU float rules are looser [unverified] | GAPS §5.2 |

**Design.** Thresholds are safe: constants such as 0.0065 are not exact in binary, but they are the same double on every machine, so comparisons stay deterministic (BIOMES §7.4).

**The generator.** The rule is in force: generation uses hashing, seeded PRNGs and exactly specified arithmetic only. The few places that need `exp`, `ln`, `sin` or `cos`, such as the logarithmic precipitation scale, use deterministic versions built from `+ − × ÷` (map-generation.md, Appendix A).

---

## 4. Cells: the base map

### 4.1 Four meanings of "cell"

The first survey separates four things that are all called cells (CELLS §1):

| Meaning | The cell is | Typical job |
|---|---|---|
| Space partition | A Voronoi polygon around an irregularly placed point | Organic map regions |
| Grid cell | A square, hex or triangle tile, often in chunks | Indexing, storage, on-demand generation |
| Automaton cell | A grid cell updated by a local rule | Caves, smoothing, erosion |
| Constraint cell | A slot holding the tiles still possible | Tile layouts that obey adjacency rules |

The generator uses the first, addressed through the second: a Voronoi partition whose sites live on a grid.

### 4.2 Choosing the points

| Method | How | Fit for an unbounded map |
|---|---|---|
| Uniform random | Independent points | Clumps and gaps; Red Blob Games rejects it |
| **Jittered grid** | One point per grid square at a random offset | Local by construction. Azgaar uses a jitter of ±0.9 × half the spacing. With jitter 0 the cells are the grid itself, so jitter is a single knob from regular to organic |
| Lloyd relaxation | Move each site to the centroid of its cell, repeatedly | Global and iterative. O'Leary uses one pass |
| Poisson disc (Bridson 2007) | Samples at least `r` apart, grown from one seed with `k` = 30 tries, O(N) | Global growth order; needs a per-chunk variant |

(CELLS §2.2.)

**The key observation.** If each grid square owns one hashed point, as in Worley noise and Azgaar's grid, the Voronoi cell containing any location can be found by checking the grid squares around it. No triangulation is needed. Poisson-disc spacing is lost, but cells stay reasonably regular (CELLS §3.3).

### 4.3 Exactness of the neighbourhood search

The survey suggested a 3 × 3 search. The second pass corrected that (GAPS §2.2):

- With one point per unit grid square, every point outside the 3 × 3 block is at least 1 away in the L∞ metric, and every Lp distance is at least the L∞ distance.
- The point in the query's own cell is within 1 (L∞), √2 (L2) or 2 (L1). So **3 × 3 is provably exact for the nearest point only under L∞**. For Euclidean distance a closer point outside the block is possible, though rare.
- The exact method is Worley's pruning: visit neighbouring cells in order of their minimum box distance and stop when that distance exceeds the best found.

**The generator** searches square rings of radius 0 to 3 and stops when the ring's lower bound, `r − 1 + (1 − jitter)/2` grid units, exceeds the best distance. That is the pruned, exact form.

### 4.4 Triangulation, and why the generator skips it

Mesh-based generators triangulate their points. The survey compares Fortune's sweepline (O(n log n), hard to make robust), Bowyer–Watson (simple, incremental) and Delaunator (about 1 s per million points, flat integer arrays) (CELLS §2.3). All are global constructions over a finite point set.

The generator needs adjacency, not a mesh. It uses the **Gabriel graph**: two cells are neighbours when no third site lies inside the circle whose diameter joins their sites. It is a subgraph of the Delaunay triangulation, it is symmetric, it always contains the nearest neighbour, and it can be tested locally on a grid.

### 4.5 Borders, distance functions and warping

- **`F2 − F1` is not a distance.** Thresholding it to draw cell borders gives uneven line widths, because the value stretches with the spacing of the two nearest points (GAPS §2.2, after Quilez).
- **The exact border distance.** In coordinates relative to the query, `(|b|² − |a|²) / (2|b − a|)` is the exact distance to the bisector between the nearest point `a` and another point `b`. Quilez's two-pass form finds the nearest point in 3 × 3 cells and then takes the minimum bisector distance over 5 × 5 cells (GAPS §2.2).
- **Metrics.** Manhattan distance gives "rigidly rectangular" regions, Chebyshev blocky ones; squared Euclidean gives the same diagram as Euclidean without the root (GAPS §2.1).
- **Signed distance functions** turn a polygon or a set of capsules into a smooth field. The notes propose them for island masks (`e = fbm − α·sdf`), coastal bands and river corridors (GAPS §2.3).
- **Domain warping** evaluates a pattern at a displaced position, `f(p + h(p))`, with `h` built from fBm. Quilez's canonical constants are a warp scale of 4 and the offsets (5.2, 1.3), (1.7, 9.2) and (8.3, 2.8) (GAPS §3.3).

**The generator** warps the **query point**, not the sites: patch borders become wavy (amplitude 0.26 cell sizes) while sites stay put. Borders are drawn pixel-exact in the raster by comparing neighbouring pixels, so the uneven-width problem does not arise. Signed distance functions and non-Euclidean metrics are not used.

### 4.6 Surveyed but not used

The first survey covers more than the generator needs. In brief:

| Topic | Finding | Why it is not adopted |
|---|---|---|
| Square and hex grids | Hex neighbours are uniform; axial coordinates are the recommended storage (CELLS §3.1) | The Voronoi levels give the organic look; a hex overlay for gameplay stays possible |
| Cellular-automaton caves | The "4-5 rule" with 40–45 % initial fill; a flood fill for connectivity (CELLS §4.1) | No underground layer |
| Erosion | Pipe-model, droplet and flux erosion; all iterative and costly on tiles (CELLS §4.3) | Terrain is constructed from the rivers; no simulation is needed for drainage |
| Wave Function Collapse and model synthesis | Local adjacency rules; failure grows with size; scanline order plus blocks scales better (CELLS §5) | No tile layouts yet; a candidate for towns later |
| Tectonic plates | Cortial et al. 2019; a cheap Voronoi approximation is sketched (CELLS §7) | Zone types and a relief noise stand in for tectonics |
| Line networks from noise | A single field's zero isoline has no junctions; the 2D AND of two fields gives dots, the OR gives a network; gradient normalisation keeps width constant (GAPS §3) | Rivers come from space colonization and roads from routing |

The survey's architecture recommendation (CELLS §9) was a finite Voronoi **world layer** of 10,000–50,000 cells plus chunked local layers, with a `CellGraph` interface and `Field` arrays. The generator went another way: there is no finite world layer. The "infinite alternative" the survey listed as an option, the jittered grid, became the whole base map, and regions took the place of the world layer. The survey's open question 2 ("a coarse infinite layer with bounded-range hydrology is an unsolved design area") is what the per-region rivers answer.

### 4.7 What other generators do

| Generator | Cells | Height | Rivers | Climate and biomes |
|---|---|---|---|---|
| Red Blob mapgen2 | Poisson disc, Delaunay, centroid dual | Distance from the coast | Follow the downslope graph | Moisture from distance to fresh water; Whittaker-style table |
| Red Blob mapgen4 | Same | Noise plus mountain distance fields | Binary trees on triangles, flow accumulated in reverse | Wind sweep with orographic rain |
| O'Leary | 16,384 points, one Lloyd pass | Primitives, then flux erosion | From erosion flux | — |
| Azgaar | Jittered square grid | Templates of hills, ranges and troughs | Depressions raised, flux downhill | Winds along the grid; 5 × 26 biome matrix |
| **This generator** | Three nested jittered grids, no mesh | Fitted to the rivers | Space colonization per region | Noise fields; belt × aridity |

(CELLS §2.4, HEIGHT §8.) All four reference generators are finite and global. None is order-independent on an unbounded map.

### 4.8 The hierarchy, oceans and mergers

The three-level structure follows the owner's own spec, not a research note: regions of 128 km own ocean seeds and zone mergers, zones of 32 km are the land or ocean unit, patches of 8 km carry detail. The research contributes the construction (stateless grid Voronoi) and, later, a reading of the sizes:

- **The sizes are Christaller's K = 4 system.** In the traffic principle, market-area spacings double per order: 4, 8, 16, 32, 64, 128, 256 km. Patches, zones and regions are orders 2, 4 and 6, and K = 4 predicts 16 lower areas per area two orders up, which is 16 patches per zone and 16 zones per region (SITES §5.2) [partially verified].
- **The patch is a farming catchment.** A mean area of 64 km² is a circle of radius 4.5 km, the one-hour catchment of site-catchment analysis (SITES §2.2).

| Topic | Status |
|---|---|
| Stateless grid Voronoi with exact pruned search | Adopted |
| Per-cell hashing, per-region sfc32 streams | Adopted |
| Order independence | Adopted |
| Query-point warping; nesting through the patch | Adopted |
| Distance metric as a parameter; SDF masks | Open (not needed so far) |

---

## 5. Rivers: space colonization

### 5.1 The algorithm

Space colonization comes from two papers by Runions et al.: leaf venation (2005) and trees (2007). Feng, Wünsche & Shaw (2023) applied it to rivers and report that some variants were indistinguishable from real river patterns in a user study of 55 people; only the abstract could be read [partially verified] (GAPS §1.1, §1.6).

Inputs are a set of **attraction points**, one or more **roots**, and three lengths: the segment length `D`, the radius of influence `dᵢ` and the kill distance `dₖ`. Each iteration (GAPS §1.2):

1. **Associate.** Each attraction point finds its closest node. If that node is within `dᵢ`, the point joins the node's influence set.
2. **Grow.** Every node with a non-empty set sums the unit vectors towards its points, normalises the sum and adds a child at distance `D` in that direction.
3. **Kill.** Remove every attraction point that now has a node within `dₖ`.
4. **Stop** when no points remain or no node is within `dᵢ` of any point.

Branches appear when the set of points pulling one node later **splits** between two nodes. Anything that makes pull sets larger and more coherent gives fewer, smoother branches; anything that makes them small and noisy gives more, irregular ones (RIVERS §1).

### 5.2 What the parameters do

| Parameter | Effect | Values in the papers |
|---|---|---|
| Number of points | Fewer points give a sparser, more irregular structure, because one point swings a tip's direction | 12,000, 1,500, 375 |
| `dₖ` | Larger values give sparser, smoother branches; smaller values give denser patterns with higher-order branches | 2D and 20D; 40 down to 1 |
| `dᵢ` | Smaller values make tips meander between zones of influence, a "wiggly or gnarly" look; infinite is smoothest | ∞, 17D, 8D |
| `D` | The unit of resolution; run time "strongly depends" on it | none given |

(GAPS §1.3, RIVERS §3.1.) Documented ratios in other implementations: Coding Train `dᵢ` = 20D, `dₖ` = 2D; Webb 6D and 1D; Palubicki 4–6 and 2 internodes.

**The defaults** are now `D` = 1 km, `dᵢ` = 47.8 km and `dₖ` = 5 km. At the time of the notes they were 2, 16 and 3 km. The large influence radius gives fewer, straighter rivers.

### 5.3 Cost, pitfalls and determinism

- **Cost.** Naively, finding each point's closest node is O(points × nodes) per iteration. The 2005 paper keeps Voronoi diagrams; Webb rebuilds a k-d tree each frame (GAPS §1.4).
- **Design: an incremental nearest-node cache.** Nodes are only added, never moved. So each attraction point can cache its nearest node, and after a growth step only the points near each new node need updating, found through a grid with cell size about `dᵢ`. The kill test comes for free (GAPS §1.4). The generator does exactly this.
- **Stalled tips.** When a tip's points lie on opposite sides, the pulls cancel. Guards: skip growth when the sum is near zero, reject a node that lands on an earlier one, and keep `dₖ > D` so a tip cannot step over a point (GAPS §1.5). The generator raises `dₖ` to at least 1.05·D and has a stall guard that steps straight at the closest attractor.
- **Determinism.** The 2005 paper breaks ties at random, and float sums depend on order. Break ties by node index, iterate in a fixed order, and seed the placement from the stage hash (GAPS §1.5).
- **It is global and sequential.** The result depends on growth order across the whole domain, so it cannot be computed chunk by chunk like noise. It needs an owner with a closed domain (GAPS §1.5). In the generator that owner is the region.

### 5.4 Rivers first

**Design** (GAPS §1.6): grow the network **upstream** from river mouths over the land cells, with flow the reverse of the parent links, then build elevation after the network. Compared with flow accumulation on a heightfield:

- It needs no drainable heightfield and no depression filling.
- It gives direct control over density (`dₖ`) and wiggliness (`dᵢ`).
- Nothing guarantees that the rivers run downhill on a terrain that already exists. That consistency has to be built in afterwards, which is §6.

How the generator applies it:

| Element | Research suggestion | Adopted |
|---|---|---|
| Domain | Land cells of a world layer | The land of one region |
| Attraction points | Land-cell centres, density weighted by rainfall | 10 random points inside every inland patch; 0 or 1 in a river-poor flat zone (a flat zone whose site precipitation is below 1,660 mm/yr) |
| Roots | Coastal cells with a minimum spacing | The site of every ocean zone that touches the region's land, so each river first crosses the water |
| Coast | — | Coastal patches get no points, which keeps mouths apart |
| Width | Murray's rule `rⁿ = r₁ⁿ + r₂ⁿ`, n ≈ 2–3 | Width ∝ flow^b with b = 0.4; flow = attractors reached upstream |
| Termination | Points exhausted or out of range | Runs until every attractor is reached: a gap wider than `dᵢ` is bridged with a straight run |

The region rule is the generator's own: a river node may lie over any ocean or on its own region's land, never on another region's land. Landlocked regions are exempt and search up to 4 zone steps for an ocean.

### 5.5 Knobs that change the look

RIVERS lists 22 techniques, each with the place in the river code where it hooks in, its cost and whether it can be bit-exact. Its ranking by look change per effort put inertia, the perception cone, correlated wander and tropism first. Status:

| # | Technique | Visual effect | Source | Status |
|---|---|---|---|---|
| 2.1 | Blue-noise attractor spacing (a birth distance) | Smoother trunks, evenly spaced tributaries | Runions 2005 | Open |
| 2.2 | Density gradients (by coast distance, moisture or noise) | Bare lower trunks and branchy headwaters, or the reverse | Runions 2007 | Partly: the river-poor flat zones are a density rule by precipitation |
| 2.3 | Incremental release (headward waves, coarse-to-fine) | Long trunks, a size hierarchy | Runions 2005, 2007 | Open |
| 2.4 | Warped or clustered attractors | Tributary fans and gaps | Webb (effect undocumented) | Open |
| 3.1 | `dᵢ` and `dₖ` as multiples of `D` | The character survives a change of `D` | Runions 2007 | Open (minimum ratios only) |
| 3.2 | **Perception cone** | Forward forks, no hooking back, a cap on junction angles | Palubicki 2009 | **Adopted** (120° by default) |
| 4.1 | **Inertia** | Straighter reaches, wider bends | Palubicki 2009; Coding Train | **Adopted** (default 0) |
| 4.2 | Tropism (an inland bias vector) | Parallel drainage | Runions 2007 | Open |
| 4.3 | Turn limit per step | No hairpins | Design | Open |
| 4.4 | **Correlated wander** | Bends instead of wiggle | Langbein & Leopold 1966 (motivation) | **Adopted** (0.23) |
| 5.1 | Coherence-scaled step | Nodes bunch at junctions | Coding Train (side effect) | Open |
| 5.2 | **Vigour and apical control** | Dominant main stems, fewer stubs | Palubicki 2009 | **Adopted** as main-stem dominance (0.24) |
| 5.3 | Child cap, lateral gap, alternating sides | Bifurcations, spacing, herringbone | Design | Open (a safety cap of 8 children) |
| 6 | Closed venation (loops) | Braids and deltas | Runions 2005 | Open; flow assumes a tree |
| 7.1 | Junction angle by climate | 45° arid, 72° humid | Seybold et al. 2017 | Open |
| 7.3 | Drainage-pattern presets | Dendritic, parallel, trellis, rectangular, radial | Howard 1967 | Open |
| 7.4 | Meanders as a post-process | Bends on big rivers | Langbein & Leopold 1966 | Open |
| 8.1 | **Width exponent** | Trunk dominance | Leopold & Maddock 1953 | **Adopted** (b = 0.4; ½ is the hydraulic-geometry value) |
| 8.2 | Strahler order | Stepped widths, pruning | Strahler 1957 [unverified] | Open |
| 8.3 | Spur pruning | Fewer "hairs" | Design | Open |
| 8.4 | Decimate, relax, smooth | Smaller junction angles, smooth curves | Runions 2007 | Open |
| 8.5 | Optimal-channel-network rewiring | Geomorphic statistics | OCNet | Open |

The generator adds a sixth branching knob that is not in the note: **pull power** (equal pulls, or weighted by distance² or √distance).

Details worth keeping from the adopted four:

- **Cone.** It comes from Palubicki et al. 2009, not from the 2007 tree paper, which uses a sphere. Every node is a "bud" here, so the cone half-angle caps the junction angle. The nearest-node bookkeeping must skip nodes that cannot see the attractor (RIVERS §3.2).
- **Inertia.** `dir = unit(unit(Σ pulls) + w·heading)`. High values overshoot attractors, which the stall guard catches (RIVERS §4.1).
- **Wander.** The white-noise jitter draws an independent vector each step and gives wiggle, not bends. A child inheriting part of its parent's random vector gives long, coherent bends (RIVERS §4.4).
- **Main-stem dominance.** A node that already has a child may sprout a lateral only if its pull count is large enough against the continuing stem's (RIVERS §5.2). The generator compares it with the pulls on the child's subtree up to 47 generations.

### 5.6 Realism targets

| Target | Value | Source | Use |
|---|---|---|---|
| Junction angle | About 45° in the driest climates, about 72° in humid ones | Seybold et al. 2017 [partially verified] | A cone or turn limit by patch moisture |
| Groundwater-fed branching | 71.9° ± 0.8°, the predicted 2π/5 | Devauchelle et al. 2012 [partially verified] | Same |
| Hack's law | Stream length ∝ area^0.6: `L = 1.4·A^0.6` in miles | Hack 1957 | A diagnostic: fit log length against log flow per region |
| Bifurcation ratio | About 3–5 | [unverified] | A diagnostic |
| Width | `w = a·Q^b` with b close to 0.5 downstream | Leopold & Maddock 1953 | With additive flow this equals the pipe rule with n = 1/b, so b = ½ is Leonardo's rule |
| Meanders | Direction is a sine of path distance; meander length ÷ bend radius ≈ 4.7 | Langbein & Leopold 1966 | A trig-free post-process with the parabolic arc `4x(1 − x)` |

(RIVERS §7, §8.1.) Flow is a drainage-area proxy because attractors are spread evenly over the land. One attractor stands for about `patchSize² / perPatch` = 6.4 km² (HEIGHT §2).

### 5.7 Status

| Topic | Status |
|---|---|
| Open-venation space colonization per region, upstream from the sea | Adopted |
| Grid-accelerated nearest-node cache, fixed orders, index tie-breaks | Adopted |
| Region border rule, bridges, landlocked search | Adopted (the generator's own additions) |
| Cone, inertia, wander, main-stem dominance, pull power, width exponent | Adopted |
| Tropism, blue-noise spacing, incremental release | Open |
| Hack exponent and bifurcation ratio as tuning gauges | Open |
| Meanders, smoothing, Strahler order, pruning, OCN rewiring | Open |
| Loops for deltas | Open |

---

## 6. Height map: terrain fitted to the rivers

### 6.1 The problem and two observations

The literature that derives height **from** rivers is small: Kelley, Malin & Nielson 1988 (a surface under tension fitted to a stream network), Prusinkiewicz & Hammel 1993 (river and mountain by subdivision) and Génevaux et al. 2013 (river network first, terrain second) (HEIGHT §1).

Two observations shape the method (HEIGHT §1):

1. **River bed elevation follows from flow alone.** With the drainage network fixed, the steady-state stream-power law fixes the channel slope from drainage area. Integrating it from the coast gives every river node's height, exactly, monotonically and per region, before any terrain exists. Everything else is "fill the land between the channels so that it drains into them".
2. **The map already has its main divides.** Rivers never enter another region's land, so each region is a closed hydrological unit and its land border is a divide. Any global-looking step can run per region, as long as both sides agree on the border values.

### 6.2 River beds: the χ integral

- **The law.** Channel evolution is `∂z/∂t = U − K·A^m·|∂z/∂x|^n`, with uplift U, erodibility K and area A. At steady state the slope is `S = k_s·A^−θ` with concavity θ = m/n; a reference value of 0.45–0.5 is common (HEIGHT §3.1, after Mudd et al. 2014).
- **The integral.** Integrating upstream from base level gives `z(x) = z_b + k·χ` with `χ = ∫ (A₀/A)^θ dx`. At steady state all channels of a network are straight lines in χ–z space (HEIGHT §3.1).
- **The algorithm.** Visit nodes in increasing index order, so parents come first. A node over the sea is at base level. A dry node gets `z = z_parent + Δx · S₀ · (A₀/A)^θ` (HEIGHT §3.2).
- **Guarantee.** `Δx > 0` and `S > 0`, so every child is strictly above its parent. C2 holds by construction.
- **Bit-exactness.** θ must be dyadic: ½, ⅜ or 7⁄16 = 0.4375, the closest to the reference 0.45, as a product of nested roots.
- **Cost.** One pass, about 10 operations per node; 100,000 nodes in well under 1 ms.
- **Look.** Concave long profiles: steep headwaters and nearly flat lower trunks. With θ = ½ and S₀ = 0.02 a 100 km main stem rises to about 450 m. S₀ is the knob for overall relief.
- **Equivalence with simulation.** Running an implicit stream-power solver with the receivers pinned to the given rivers converges to this same profile, so simulating is only worth it for the land between the rivers (HEIGHT §3.1).
- **Alternative.** Génevaux sets river slope from a position map, not from area. That loses the concave profile, so the note keeps it only as a per-type multiplier on S₀ (HEIGHT §3.3).
- **Repair.** If anything later lowers or raises nodes, one reverse sweep `z[parent] = min(z[parent], z[k] − ε)` restores monotonicity. mapgen4 and Azgaar both carve this way (HEIGHT §3.4).

**The generator:** S₀ = 0.01, θ selectable among ⅜, 7⁄16 (default) and ½, and a gradient factor per elevation type (flat 0.7, lowland 1, hills 1.8, mountains 3). One addition is not in the note: **Level flat zones**, which puts a zone whose gradient factor is below 1 at one level by lowering the rivers that enter it, cutting gorges through the steeper land downstream.

### 6.3 Filling the land between the rivers

| Method | Idea | Local | Drains | Look | Verdict |
|---|---|---|---|---|---|
| **Lower envelope of slope cones** (§4.1) | `h(p) = min` over channel segments and the coast of bed height + hillslope × distance | Yes, provably: a source farther than `h_max / s_min` cannot win | Yes, if the slope varies slowly and channels are not perched | V valleys, creased divides; a flat floor ∝ √flow gives floodplains | The simple core |
| **Eikonal or fast marching** (§4.2) | Solve `|∇h| = s(x)` from the sources: a geodesic distance | Region raster with a halo | Yes, for any positive slope field | Slopes change at type borders | The correct form when steepness varies |
| Génevaux crests and primitives (§4.3) | Voronoi cells of river nodes; crest height `max(a, b, c) + α·d`; blended primitives | Yes | Rivers yes; small pits from noise | Smooth; "meandering crests" | Its crest rule and noise scaling are reused |
| Diffusion (§4.4) | Laplace interpolation between constraints (Hnaidi et al. 2010) | Per region with a fixed border | No pits without noise | Soap film; needs noise | Heavier; not chosen |
| Constrained midpoint displacement (§4.5) | River and mountain by subdivision | Hashed and local | Yes, with a flat bed | No tributaries | Ruled out |
| Pinned stream-power relaxation (§4.6) | Simulate only the free cells with channels and border pinned | Per region | Yes | Dendritic gullies below river resolution | Option C, a separate project |

(HEIGHT §4.) Points from the envelope method that carry over:

- **Locality bound.** With a maximum height of 600 m and a minimum slope of 0.02 the search radius is 30 km, far below the 128 km region size, so a 3 × 3 block of regions always suffices (HEIGHT §4.1).
- **Perched rivers.** A river node can end up above the envelope when a lower trunk of another branch is close. Génevaux avoids it with a Lipschitz condition; keeping hillslopes much steeper than channel slopes does the same (HEIGHT §4.1).
- **Global normalisation must go.** mapgen2 divides by the global maximum distance; an unbounded map needs constant scales.
- **Floodplains.** A flat floor whose half-width grows with √flow matches hydraulic geometry.

### 6.4 Repairing drainage

- **Depression filling.** Priority-Flood floods inward from the edge with a priority queue, in O(n log n) for floats, and labels watersheds. Planchon & Darboux and O'Leary's ε-fill are the iterative alternatives (HEIGHT §5.1).
- **Why per region works.** Depressions can be arbitrarily large, so filling is non-local in general; Barnes 2016 needs a global spillover graph for tiles. Here the region is the whole hydrological domain: seed the flood from the region's ocean cells and hold the border fixed (HEIGHT §5.1).
- **Stream burning.** AGREE lowers the cells under the vector streams, buffers the lines, interpolates up to the surface at the buffer edge, then lowers the line cells again so trenches are not filled. The note maps buffer to valley half-width, smooth drop to valley depth and sharp drop to channel depth (HEIGHT §5.2).
- **Order.** Burn first, then fill: the standard GIS order.
- **Fill or carve.** Carve along rivers, because the channel must win. Fill off-river pits. Filling across a river would turn it into a lake; breaching a ridge with no river would invent a channel the map does not show (HEIGHT §5.3).

### 6.5 Divides and relief

- **Region borders are the primary divides.** **Design:** add a ridge that both sides compute identically, tapered near rivers and suppressed where a landlocked region's river crosses a border (HEIGHT §6.1).
- **Basin divides inside a region.** The `tree` id of each node separates river systems that reach the sea independently. Where the two nearest candidates belong to different trees and are about equally far, add a boost, so divides between river systems are higher than those between tributaries (HEIGHT §6.2).
- **The coastal strip.** Coastal patches have no attractors, so a band about one patch wide carries only mouths. The coast cone makes it rise gently (HEIGHT §6.3).
- **Relief by type.** Génevaux steers with a river slope map and a terrain slope map; Hnaidi diffuses noise amplitude and roughness; uplift maps do the same in simulations (HEIGHT §7.1).
- **A caveat about hashed types.** When the note was written, land types were a hash of the cell key, so "Hills can neighbour Marsh at random". The recommendation was a low-frequency, region-scale relief field so that mountain belts span several cells (HEIGHT §7.2).
- **No seams.** Blend **parameters**, not heights, then evaluate one noise and one envelope. Mask detail noise to zero on the bed so rivers stay lowest (HEIGHT §7.3).

### 6.6 The three pipelines and what was built

| Option | Steps | Guarantees | Cost |
|---|---|---|---|
| **A, envelope** | χ profile; min over segments within R; blended slopes; masked noise | Rivers monotone; every pixel drains, before noise | Low; no global structures |
| **B, raster with repair** | A on a per-region raster with fast marching; border ridge and divide boost; noise; AGREE burn; ε-Priority-Flood from the region's ocean cells | Every cell has a downhill path to the sea through the given rivers | Medium |
| **C, pinned relaxation** | B, then implicit stream power on the free cells with uplift and diffusion | Sub-resolution gullies that feed the drawn rivers | High; a background job and a cache per region |

(HEIGHT §9.) The note recommended building A first. **The generator uses Option B:**

| Step | Research | Adopted |
|---|---|---|
| River beds | χ integration with dyadic θ | As researched, plus the gradient factor per elevation type and flat-zone levelling |
| Hillslopes | Fast marching with a varying slope | A geodesic distance by 8-connected Dijkstra from every bed and the coast, with the elevation type's hillslope as the cost (flat 0.006, lowland 0.06, hills 0.15, mountains 0.277) |
| Floodplain | Half-width ∝ √flow | A monotone spline over bed height (400 m at sea level to 30 m at 1,000 m), × √(flow/100) |
| Relief belts | A region-scale relief field | An fBm with 220 km features, strength 0.5 |
| Blending | Compact polynomial weights on parameters | A 4 km box blur of hillslope, noise amplitude and ridged share over land cells |
| Divides | Border ridge; other-tree boost | Region ridge 80 m; basin divides 30 m; both fade near rivers |
| Detail | fBm or ridged per type, masked on beds | As researched (amplitude 8, 25, 80 and 180 m) |
| Repair | AGREE burn, then ε-Priority-Flood | As researched; channel depth 3 m; the fill is seeded from the sea and the region border |
| Sea floor | A depth by distance from the coast | Deepens to 300 m at 40 km |

Relief is steered by the **elevation type** (flat, lowland, hills, mountains, hashed with weights 5 / 6 / 1 / 1), which replaced the cosmetic land types the note worked with.

| Topic | Status |
|---|---|
| Option B, all five steps | Adopted |
| Properties to test: monotone beds, every cell drains, seamless tiles, determinism | Adopted |
| Lipschitz clamp against perched rivers | Not needed so far (hillslopes are much steeper than channels) |
| Lakes levelled in the terrain | Open: lakes are a separate layer and the height map does not level them |
| Pockets walled off from the sea | They fill up to the border ridge; not drawn as lakes |
| Option C (pinned stream-power relaxation) | Open |
| Smooth-min for rounded crests; parabolic U valleys | Open |

---

## 7. Tiles and cost

TILES asked what it takes to generate the map tile by tile, so that moving over the map only computes what is new. The figures were measured on the prototype (seed 12345, default parameters). They show the relative cost of the layers, not the speed of any later implementation.

- **The map is local already.** Regions, zones, patches, oceans and mergers are a memoised pure function of the point, independent of the order of exploration. Rivers are a pure function of the region. Only the height map carries a large halo.
- **Ocean halo.** The first query in new territory reads 428 regions and 8,477 zone cells in 20 ms. The radius is the link range + 5 = 7 regions. Cheap enough to leave alone.
- **Rivers.** About 14 ms per region.
- **Per-pixel classification.** A picture of 1600 × 1000 pixels costs about 0.9 s of `cellAt`. The cost is the Voronoi query plus the warp noise per pixel, so memo caches barely help. A map display should cache its picture in tiles.
- **Per-region height map overhead.** One region's raster is about 190 × 185 cells at 2 km: 9.3 raster cells per own land cell.
- **One raster per tile against one per region** (16 regions):

  | Resolution | 16 region rasters | One tile raster | Time, 16 regions | Time, tile |
  |---|---|---|---|---|
  | 2 km | 519,894 cells | 160,801 | 1,329 ms | 683 ms |
  | 1 km | 2,068,808 cells | 641,601 | 3,801 ms | 1,567 ms |

  That is 3.2 times fewer cells and about 2.4 times less time.
- **Equality.** Heights on every own-land cell are bit-identical between the tile raster and the per-region rasters (202,671 cells at 1 km).
- **Edge effects.** The heights before the fill differ between overlapping rasters only near a raster edge: up to 3,706 m within 10 km, 0.2 m at 10–20 km, zero beyond 20 km. So a halo of 24 km is enough.

| Recommendation | Status |
|---|---|
| Own cells by region grid coordinate, never by point | Adopted |
| One raster per tile; steps 1–4 once, the fill once per owned region | Adopted as an optimisation (map-generation.md 12.1) |
| Tile size by a roughly constant world size: about 256 km, which is 2 × 2 regions at the default size | Adopted |
| Halo 24 km | Adopted |
| The drawn map as a pyramid of cached picture tiles | Open |

---

## 8. Climate and biomes

### 8.1 What every scheme agrees on

A biome is a large-scale vegetation formation. The major schemes agree on four points (BIOMES §1):

1. **Heat and water come first.** Every scheme uses a temperature variable and a precipitation variable.
2. **Water availability is relative to evaporative demand.** The same rainfall is wet in the cold and arid in the heat.
3. **Elevation acts like latitude.** Temperature drops about 5–6.5 °C per km, so mountains carry a compressed pole-to-equator sequence.
4. **Climate does not decide everything.** Fire decides between savanna and forest in a wide band of intermediate rainfall; soil, flooding and salt water create azonal biomes.

### 8.2 The schemes

| Scheme | Inputs | Output | Thresholds | Use for a generator |
|---|---|---|---|---|
| **Whittaker** (1975, via Ricklefs 2008) | Mean annual temperature (°C) and precipitation (cm) | 9 biomes | Hand-drawn polygons; no formulas | Simplest; a 2-D lookup; ignores seasonality; the cold-wet corner is empty |
| **Holdridge** (1947, 1967) | Biotemperature, precipitation, their ratio (PET ratio), elevation for the belt | 38 hexagons, about 120 zones with belts | Guide lines at powers of 2 | Explicit numbers; humidity built in; needs biotemperature |
| **Köppen–Geiger** (1936; Peel et al. 2007) | Monthly temperature and precipitation | 5 classes, 30 types | Explicit inequalities | Exact rules; needs a 12-month year; gives climate classes, not biomes |
| **WWF** (Olson et al. 2001) | Expert-mapped ecoregions | 14 biomes | None | A palette; three biomes are azonal (flooded grassland, montane grassland, mangrove) |
| **IUCN GET** (Keith et al. 2022) | Ecological drivers | 7 terrestrial biomes, each split into functional groups | None | Names the drivers: "pyric", "seasonally dry", "oceanic", "alpine" |

Numbers worth keeping:

- **Whittaker.** The digitised chart spans −10 to 30 °C and 0 to 400 cm. It is triangular because no climate is both very cold and very wet. Between forest and desert lies a zone where grassland, shrubland or woodland can each prevail depending on soil, fire and seasonality (BIOMES §2.1; confirmed for the Ricklefs caption in CHARTS §2.2).
- **Holdridge.** Biotemperature is the mean with values below 0 and above 30 °C counted as zero. PET is 58.93 mm × biotemperature, so the humidity axis is effectively precipitation ÷ temperature. Guide lines: biotemperature 1.5, 3, 6, 12, 24 °C; precipitation 62.5 to 8,000 mm in doublings; PET ratio from 32 to 0.125. The same temperature lines bound the latitudinal regions and the altitudinal belts. The frost line lies at about 16–18 °C (BIOMES §2.2).
- **Köppen.** Arid (B) is tested first: mean annual precipitation below 10 × P_th, with P_th = 2·MAT plus 0, 14 or 28 by rain season. For evenly spread rain the boundary is **MAP = 20·MAT + 140 mm**; half of that is the desert line. Tropical means the coldest month is at least 18 °C; polar means the warmest month is at most 10 °C. Land shares: B 30.2 %, D 24.6 %, A 19.0 %, C 13.4 %, E 12.8 %. Peel uses no highland class: mountains are handled by lowering the temperature and re-running the rules (BIOMES §2.3).

### 8.3 Temperature sets the demand

- **Example.** At a biotemperature of 3 °C, Holdridge's PET is about 177 mm, and 500 mm of rain is perhumid. At 24 °C the PET is about 1,414 mm, and the same 500 mm is semiarid (BIOMES §3.1).
- **Aridity index.** AI = precipitation ÷ potential evapotranspiration. Classes after UNEP: hyper-arid below 0.03, arid to 0.2, semi-arid to 0.5, dry sub-humid to 0.65, humid above (BIOMES §3.2). Holdridge's PET ratio is its reciprocal.
- **Relative humidity is the wrong variable.** It is a state of the air, it swings daily, and no biome scheme uses it (BIOMES §3.4). The second climate field was documented as "relative humidity, %" when the note was written. **Design:** read it as a moisture index, or replace it with precipitation in mm/yr. **The generator replaced it:** the second field is precipitation, 100 to 12,000 mm/yr on a log scale.
- **Seasonality.** Köppen's subtypes test the driest month against the wettest; Holdridge treats monsoon and Mediterranean regimes as associations inside a life zone (BIOMES §3.3). The generator has no seasonality field, so it cannot tell a Mediterranean climate from a summer-wet one with the same totals.

### 8.4 Elevation

| Quantity | Value | Source |
|---|---|---|
| Standard tropospheric lapse rate | 6.5 K/km | U.S. Standard Atmosphere 1976 |
| Holdridge's working rate | 6 °C per 1,000 m | Holdridge 1967 |
| Observed near-surface rates | 3.9–5.2 °C/km (Cascades), 5.4–5.8 (Alps) | Minder et al. 2010; Rolland 2003 [partially verified] |
| Treeline | Growing-season mean of 6.7 ± 0.8 °C in the soil; 6.4 °C in the air | Körner & Paulsen 2004; Paulsen & Körner 2014 [partially verified] |
| Treeline in Holdridge | Biotemperature 3 °C | Holdridge 1967 |

- **Belts mirror regions.** At 6 °C/km the 3–6, 6–12 and 12–24 °C bands are 500, 1,000 and 2,000 m thick (BIOMES §4.2).
- **Why the two treeline numbers differ.** Körner's isotherm is a growing-season mean; Holdridge's biotemperature averages the whole year. With one annual-mean field, a treeline rule needs a seasonality field or a proxy (BIOMES §4.3).
- **Orographic rain.** Windward flanks get much more precipitation than leeward ones. The rain shadow is "one of the most confident expectations in atmospheric science" (Roe 2005). Simple upslope models match observations at scales of tens of kilometres (BIOMES §4.4).

### 8.5 Other drivers

- **Latitude.** Holdridge's bands imply about 0.4 °C per degree, roughly 0.004 °C per km (BIOMES §5.1).
- **Fire.** In sub-Saharan Africa tree cover is bimodal between 1,000 and 2,000 mm of rain: savanna or forest, with fire deciding. **Design:** this is the strongest evidence that a climate-to-biome function should be one-to-many in places; a hash or a fire field can make the choice (BIOMES §5.3, after Staver et al. 2011).
- **Azonal biomes.** Mangroves follow tidal estuaries, flooded grasslands follow hydrology, montane grasslands follow elevation (BIOMES §5.4).

### 8.6 The scale gap

With a feature size of 110 km, the temperature field goes from coldest to warmest over about 55 km: roughly 0.7 °C per km. Earth's equator-to-pole gradient is about 0.004 °C per km. At the defaults the whole tundra-to-rain forest sequence fits inside one or two zones. That may be intended for a game-scale map; the feature-size range of 20–4,000 km allows Earth-like scales (BIOMES §7.1). One consequence the trade note relies on: belts change over a few regions, so climate-locked luxuries are typically 2–5 regions from their consumers (TRADE §5.5).

### 8.7 The recommended pipeline and classifier

**Design** (BIOMES §7.2):

1. Initial Temperature T₀ (at sea level) from the field.
2. Local temperature `T = T₀ − Γ·z` with the height from the terrain. There is no cycle: biomes depend on height, and height depends on elevation types and rivers, not on biomes.
3. Moisture from a precipitation field, optionally times an orographic factor.
4. Optionally a seasonality amplitude, which would make biotemperature and the full Köppen rules computable.
5. Classify.
6. Overrides for azonal biomes: alpine above the treeline, flooded grassland on warm floodplains, mangrove on frost-free coasts, savanna or forest by a hash in the hot 1,000–2,000 mm band.

| Classifier | Needs | For | Against |
|---|---|---|---|
| Whittaker grid | T, P | Simplest; recognisable | Ignores temperature-dependent demand at the margins; undefined corner |
| Holdridge | Biotemperature, P, elevation | Explicit thresholds; belts native | Needs a seasonality proxy |
| Köppen | 12 monthly values | Published inequalities | Needs a synthetic year and a class-to-biome table |
| **Aridity × thermal belt** | An aridity index and T | Two short threshold lists, easy to tune | Coarser; the note's own composition |

The recommendation was the last one, with Whittaker's grid as a cross-check and Köppen later once seasonality exists (BIOMES §7.3).

### 8.8 What the chart catalogue adds

CHARTS catalogues 24 charts and rule tables in six lineages: Whittaker–Ricklefs, Holdridge, Walter diagrams, model rule tables (BIOME1), empirical climate space, and procedural-generation tables. Five findings:

1. **Whittaker-type charts disagree in °C.** Boreal forest spans −6.7 to 4.4 °C in the `plotbiomes` digitisation, about 1 to 8 °C in the chart on Wikipedia's "Biome" article and about −10 to 1 °C in another redraw. The regions are hand-drawn; no version is "the" boundary set.
2. **Only three packages ship the boundaries as data:** `plotbiomes` (MIT, 775 vertices) and `BIOMEplot` and `ggbiome` (GPL, 106 vertices). All trace the same Ricklefs figure.
3. **Holdridge is the only chart defined by numbers.** The geometric centre of the 12–24 °C row puts the frost line at about 16.97 °C. One published implementation measures distance on linear axes, by the note's reading of its code, which does not match Holdridge's log axes.
4. **Real biomes overlap widely** in temperature × precipitation space.
5. **Game generators use coarse grids of explicit thresholds.** Azgaar's is the only one in real units, and its moisture bands do not depend on temperature: 600 mm counts as forest at both 5 °C and 19 °C.

**Design** (CHARTS §8): keep `plotbiomes` as the reference but match its **topology**, not its cells; treat boundaries as soft; use Holdridge for explicit reusable thresholds without copying GPL code; use empirical sources only as a sanity check; accept that a Mediterranean biome needs seasonality or a small hashed region at about 13–20 °C and 700–1,300 mm. For an image, the Navarras chart on Commons is CC0.

### 8.9 What is adopted

| Step | Adopted | Research link |
|---|---|---|
| Fields | Initial Temperature −16 to 30 °C (110 km features); precipitation 100–12,000 mm/yr on a log scale (80 km) | Fields in the schemes' own units (BIOMES §7.1) |
| Distribution | Each climate field has points "this share of the map lies below this value", on the bounds of the classification: 5.5 % below −10 °C, 9 % below −3, 21 % below 3, 59 % below 18; 5 % below 150 mm, 12 % below 350, 43 % below 700, 66 % below 1,400, 88 % below 2,800 | The generator's own rule, measured (see below) |
| Height | The mean height of the patch's land from its terrain tile; a typical height per elevation type until the tile exists (70, 140, 270, 470 m) | Step 2 of the pipeline |
| Cooling | `T = T₀ − cooling(h)`, a monotone spline through 5 points. The defaults are (0 m, 0°), (200, 1°), (400, 5°), (900, 11°), (1250, 20°): far steeper than 6.5 °C/km, so terrain that is mostly below 1,000 m still gets cold highlands | The lapse-rate step, made a tunable curve |
| Moisture | P, × 0.2 in a river-poor flat zone, so those zones favour desert | The generator's own rule |
| Aridity | `m = P / max(40, 20·T + 140)` | Köppen's evenly-spread dryness threshold (BIOMES §2.3) |
| Belts | Polar below −10 °C, subpolar to −3, boreal to 3, temperate to 18, tropical above | Holdridge-like belts; thresholds calibrated against the `plotbiomes` grid |
| Classes | m < 0.5 desert, 0.5–1 steppe or scrub, 1–2 woodland or savanna, then forest; rain forest from m ≥ 6 (temperate) or ≥ 4 (tropical). In the boreal belt cold steppe runs to m = 2 | The belt × aridity table; real cold steppes get 150–350 mm/yr, which is m 1–2 at 0 °C |
| Result | 15 land biomes, plus **mangrove** on coastal patches of flat or lowland zones with T ≥ 20 °C and m ≥ 0.5 | Azonal override, with an annual-mean test because there is no coldest-month value |

The note's own caveat applies (RES §7.5): crop altitude limits from ECOCROP assume real lapse rates, so with the steep default curve crops drop out lower than their limits suggest.

**Why the fields have a distribution.** Measured on the final biomes of 8 seeds of 5 × 5 regions (2026-11-14), the plain fields gave seven biomes less than 2 % of the land each: the five tropical biomes shared 11 %, ice & snow had 1.1 %, cold steppe 1.3 % and mangrove 0.6 %, while boreal forest and temperate rain forest had 17 % and 15 %. The cause is the bell shape of the noise: half the map lies in the middle quarter of a field's range, and that middle is the temperate belt. Three remedies were weighed, each scored by the smallest share among the 15 zonal biomes and by the effective number of biomes (the exponential of the entropy of the shares; 16 when all are equal, 11.3 before):

| Remedy | Smallest zonal share | Effective biomes | Verdict |
|---|---|---|---|
| Parameters only: wider field ranges, river-poor factor, mangrove threshold | 2.0 % | 13.1 | Needs −18 to 40 °C; the tails of the bell stay thin |
| Flatten the bell towards an even spread, with one strength per field | 2.5 % | 12.9 | Tundra grows to 16 % and the deserts to 25 %: one belt holds one biome, another five |
| Points "share below value" on the bounds of the classification | 1.8 % (cold steppe), the next 4.3 % | 14.6 | **Adopted** |

The points were fitted on 8 seeds and confirmed on 8 others. The river-poor cutoff moved from 1,660 to 1,470 mm/yr, which is the same share of the precipitation field, so the river-poor zones stayed the same (1 zone in 1,300 changed) and with them the rivers and the height map. The river-poor factor and the mangrove threshold were left as they were: the fit did not need them.

**Why cold steppe runs to m = 2.** After the distribution cold steppe still held 1.8 % of the land. The dryness threshold is low in the cold, so the steppe class (m 0.5–1) is only 70 to 140 mm/yr at 0 °C, a band no precipitation spread can fill without flooding the warm belts with desert. The boreal belt had no biome of its own for the sub-humid class (m 1–2, 140 to 280 mm/yr at 0 °C) and gave it to boreal forest. That class now goes to cold steppe, which matches the rainfall of real cold steppes. Cold steppe rose to 3.1–3.4 %, boreal forest fell from 11 % to 9.5 %, and the effective number of biomes rose to 15.0.

| Topic | Status |
|---|---|
| Aridity × thermal belt classifier; height cooling | Adopted |
| Mangrove override | Adopted |
| Sea-level biome per patch (finds tropical highlands) | Adopted |
| Distribution of the climate fields by share points | Adopted |
| Cold steppe on the sub-humid class of the boreal belt (m 1–2) | Adopted |
| Orographic precipitation, rain shadows | Open |
| Seasonality field; Köppen rules; Mediterranean biome | Open |
| Alpine, flooded-grassland and fire overrides | Open |
| Smooth biome transitions | Open |

---

## 9. Resources

### 9.1 What the land must give

A world economy needs three things from the land: **food** (how many people a place can feed), **construction materials** (what they build with) and **wealth** (what they trade). Four points shape the catalogue (RES §1):

1. **Climate decides most of the above-ground list.** For over 150 plants, FAO's ECOCROP gives numeric limits that map almost directly onto the map's temperature and precipitation.
2. **Geology decides most of the underground list.** Ores, gems, stone, coal and salt beds depend on rock type and tectonic history, which the map did not model.
3. **Water and coast are the cheapest wealth:** irrigation, fish, mill power, clay, placer gold, salt, trade.
4. **What a place lacks makes trade.** Salt, iron, timber in the steppe, and fuel for smelting are the classic deficits.

### 9.2 The catalogue

RES §4 lists about 370 resources in 18 groups, from cereals to water features, plus 28 fantasy resources (§5). Each row has its role (food, construction, wealth), position (above ground, underground, water), elevation types, biomes, sourced conditions, rarity, value class and processing chain.

- **Crops** carry ECOCROP numbers: optimal and absolute temperature, optimal and absolute annual rainfall, maximum altitude, killing temperature. Bread wheat, for example: T 15–23 (5–27) °C, P 750–900 (300–1,600) mm, altitude 4,500 m.
- **Caveats.** ECOCROP temperatures are crop-cycle means, while the map's temperature is an annual mean. The values are "expert opinion and not necessarily optimal". A few rows are visibly damaged (RES, header).
- **Minerals** carry the USGS deposit model number (Cox & Singer 1986) and its geological environment. Placer gold collects "where gradients flatten"; porphyry copper sits at convergent plate boundaries; tin veins sit with late granitoids in fold belts.
- **Weak spot.** Rows on livestock, game, furs, dyes and historical trade are mostly general knowledge, marked `U`. The note calls this its weakest part.

### 9.3 Missing map features

A resource that depends on a feature the map lacks should not be forced onto a biome (RES §3). Status of the proposed features:

| Feature | Resources that need it | Status |
|---|---|---|
| Lake | Lake fish, reeds, wild rice, ice harvest | **Adopted** as polygons on rivers, with a host patch |
| Mangrove | Mangrove poles and charcoal, tannin bark, crabs, oysters | **Adopted** as a biome |
| Bedrock type per zone | Every ore, gem, stone, coal and salt bed | **Adopted** (9 classes) |
| Oasis; estuary and delta | Dates, irrigated grain; oysters, salt meadows | Partly: river irrigation and river-mouth features exist as patch facts |
| Endorheic lake, salt flat | Lake salt, natron, borax | Partly: salt flats in a few arid river-poor flat zones |
| Marsh, fen, peat bog | Reeds, wildfowl, bog iron, peat | Open |
| Coral reef, kelp coast, upwelling coast | Reef fish, pearls, kelp ash, large fisheries | Partly: shelf fish, pearls and coral by water temperature on ocean patches |
| Alpine meadow; cloud forest | Summer pasture; coffee, tea | Partly: tropical highlands are found from the sea-level biome |
| Mediterranean and monsoon climates | Olive, vine; unirrigated rice | Open: needs seasonality |
| Karst, volcanic ground, hot springs, aquifers | Caves, obsidian, sulfur, wells | Partly: volcanic arc and carbonate platform are bedrock types; caves, springs and aquifers are open |
| Fire regime | Savanna against dry forest | Open |

### 9.4 Two kinds of resources

**Design** (RES §7.1, §7.2):

1. **Potentials** (crops, timber, pasture, game, fish): every patch has a suitability score from climate and position. They are not placed; a settlement picks the best few.
   - Use the **ECOCROP trapezoid**: 0 outside the absolute limits, 1 inside the optimum, linear between. It is bit-exact.
   - Compare ECOCROP's annual rainfall directly with the patch's precipitation, and add an irrigation bonus next to rivers, so desert rivers grow wheat and dates.
   - For temperature, use an offset by belt until there is a seasonality field.
2. **Deposits** (ores, gems, stone, salt, coal): rare point resources. Each is a hashed draw against a probability from geology × climate × elevation type. A patch gets 0 to 3.
   - One hash salt per resource family, so adding a resource does not reshuffle the others.
   - Starting probabilities per eligible patch: common 0.3–0.6, uncommon 0.05–0.2, rare 0.005–0.03, very rare below 0.002. With 16 patches per zone, a rare deposit appears in every 2nd to 12th eligible zone.

### 9.5 Geology: the minimal attribute

The map has relief classes, not rocks. **Design:** one **bedrock type per zone**, drawn like the elevation type and correlated with it (RES §7.3):

| Bedrock type | Typical elevation types | Unlocks |
|---|---|---|
| Crystalline shield | Flat, lowland, hills | Granite, kaolin, banded iron, diamonds, lode gold |
| Fold belt | Hills, mountains | Slate, marble, lode gold, lead-zinc and silver veins |
| Granite intrusion | Mountains, hills | Tin, tungsten, topaz and beryl |
| Volcanic arc | Mountains | Porphyry copper, epithermal gold and silver, obsidian, sulfur |
| Flood basalt | Hills, lowland, flat | Basalt, native copper, agate |
| Ophiolite | Mountains, hills | Chromite, copper, jade, soapstone |
| Carbonate platform | Hills, lowland | Limestone, karst, lead-zinc |
| Clastic basin | Lowland, flat, hills | Sandstone, coal, red-bed copper, evaporites |
| Unconsolidated cover | Flat, lowland | Clay, sand and gravel, peat, bog iron, aquifers |

- **Sediment route.** Lodes form and are exposed in mountains and hills; rivers carry the heavy minerals down; lowlands collect fine sediment; coasts collect the most resistant grains.
- **Placer rule.** For each river node, carry a heavy-mineral load summed over upstream nodes in lode-bearing patches. Deposit a placer where the bed gradient drops. This stays inside the region's river computation.
- **Weathering.** Laterite iron and bauxite need warm-wet climates on stable flat land; evaporites need aridity and no outlet or a coast; peat needs humidity and a low gradient.
- **Provinces.** Real ore types cluster in belts; a low-frequency "province" noise per region could bias the bedrock [unverified as a general statement].

### 9.6 What games do

TRADE §2 surveys about 25 games. Six findings (TRADE §1):

1. **Games use few resources and give each one a job.** 4 in Age of Empires II, about 20 in Patrician IV, 31 in Europa Universalis IV, about 50 in Civilization VI and Victoria 3.
2. **Placement is a terrain or biome rule plus a hash.** Civilization lists terrains per resource; EU4 draws a province's good from weights modified by terrain and climate; Anno gives each island random "fertilities" and fixed "lodes".
3. **Deficits are designed in.** Patrician IV lets each city produce only 4–5 export goods and gives each of 8 regions one product made nowhere else.
4. **Trade is a graph with costs.** EU4 routes value through a fixed directed graph of nodes; Humankind counts a deposit only if an unbroken path reaches the capital.
5. **Prices are a clamped function of supply and demand.** Victoria 3: `price = base × (1 + 0.75 × clamp((buy − sell) / min(buy, sell), ±1))`. It is bit-exact.
6. **Real geography gives the same structure for free** (§11).

Taxonomy by role: basic, strategic, luxury, industrial, military, money-like. By kind and need: food, material, fuel, metal, preservative, prestige (TRADE §3.1, §3.2). **Design:** about 6–8 groups the player thinks in and 20–40 goods the simulation tracks; a chain depth of 3 with at most 2–3 inputs per output (TRADE §3.3).

### 9.7 The game resource set

**Design** (TRADE §5.1): two layers. The catalogue stays as the realism layer that names variants ("this valley's grain is rye"); a smaller game layer is what settlements, prices and trade use. The set has 39 resources in 7 groups and 3 tiers.

| Tier | Resources |
|---|---|
| **1, raw (24)** | Food: grain, produce, orchard, livestock, fish, wild food & game. Materials: timber, stone, clay & earth, crystal, fibre. Fuel. Salt. Metals: iron ore, copper ore, tin, silver & lead, gold. Luxuries: gems & pearls, spices & incense, stimulants & sugar, furs & ivory, dyes. Animals: horses & pack animals |
| **2, intermediate (9)** | Charcoal, iron, bronze, cloth, leather, bricks & lime, glass, preserved food, naval stores |
| **3, finished (6)** | Tools, arms & armour, clothing, drink & oil, fine goods, coin |

Processing is placed by Weber's material index: weight-losing steps (smelting, charcoal, salt boiling, lime) sit at the raw material; weight-adding ones (brewing, baking) at the market (TRADE §4.3). An optional fantasy layer maps the 28 fantasy rows onto three game resources (star metal, arcane reagents, wonder materials), each downstream of a real resource so that turning it off leaves a consistent economy (RES §5, TRADE §5.2).

Which level owns what (TRADE §5.4): the **patch** owns potentials, 0–3 deposit slots and enabler flags; the **zone** owns the production profile, the market town and processing sites; the **region** owns the trade balance, prices, the city and its signature export.

### 9.8 Making trade inevitable

**Design** (TRADE §5.5). Six rules, each a check the generator can test:

1. **Salt is sparse.** At least half of all regions have no salt deposit.
2. **Iron needs two inputs:** ore, and fuel in the same or the next zone.
3. **Bronze needs two distant metals.** Copper and tin come from different bedrock types, so no zone has both.
4. **Luxuries are climate-locked:** spices in the tropical belt, furs in boreal and subpolar.
5. **Every region has a signature export:** a tradeable resource with a location quotient of at least 1.5, raised by a hashed draw if none qualifies.
6. **Every region has a deficit.**

A `thrive` score from food, fuel, material, salt, an export and access to water could replace the prosperity noise (TRADE §5.5).

### 9.9 What is adopted

Every patch gets an amount from 0 to 1 for each of the **27 raw resources**, with the variant that fits best ("grain: barley", "iron ore: bog iron"). A resource is listed from an amount of 0.25; a typical land patch lists about eight.

| Element | Adopted | Research link |
|---|---|---|
| Crops | The best of a table of crops, each an ECOCROP trapezoid over growing-season temperature and yearly water, × farmland by elevation type × soil by biome × season length | RES §7.1 |
| Irrigation | A river adds 100 mm a year per patch it drains beyond the first, up to 1,200 mm | "Desert rivers grow wheat and dates" |
| Other potentials | Tables by biome and elevation type, plus coast, harbour, river size, lake | The catalogue's Biomes and Zones columns |
| Bedrock | One of the nine classes per land zone, hashed by weight from its elevation type | RES §7.3 |
| Deposits | A hashed draw per patch against a chance; the amount is the richness | RES §7.2 |
| Placers | Each river node knows the share of its upstream network in gold-bearing or tin-bearing hills and mountains; placers form where the river reaches flatter ground | The placer rule |
| Tin and copper | From different bedrock types, so no zone has both | Rule 3 |
| Salt | Solar pans on dry warm low coasts, salt flats, rock salt in some clastic basins; about half the regions have none | Rule 1 |
| Luxuries | Regional: a hashed gate per crop and zone; spices only in the tropical belt | Rule 4 |
| Prosperity | The deposit chances of gold, silver and gems and the share of zones with a luxury are × (2 × prosperity)² | **Not from the notes.** The notes proposed deriving prosperity from resources; the generator does the reverse and lets the prosperity field decide where the valuable things are |

| Topic | Status |
|---|---|
| 27 raw resources with variants; potentials and deposits | Adopted (24 at first, three added 2026-11-15) |
| Bedrock classes; placers; scarce salt; tin and copper apart | Adopted |
| Ocean patches: fish, shelf, seaweed, seals, pearls, coral | Adopted |
| Tier 2 and tier 3 goods; processing sites by Weber's rule | Open |
| Reagents, herbs and arcane materials; gypsum; reindeer, sled dogs and elephants | Adopted 2026-11-15 (below) |
| Fantasy resources on the supernatural map | Adopted as the arcane materials |
| Quantities, yields, populations | Open |
| Location quotient, signature export, deficit check, `thrive` | Open |
| Metallogenic province noise | Open |

**The three resources of 2026-11-15.** The vocation research
([research/vocations.md](research/vocations.md) §4) mapped 50 vocations onto the 24
resources and found inputs the map could not show: the mordant of the dyer and the tawyer,
the materials of the glassmaker and the alchemist, the herbs of the healer and the brewer,
and anything for the mage. The user accepted three new resources and two sets of variants.
The rules in map-generation.md section 14 are a first design. The chances and factors
were set by reasoning and then measured once, over 81 regions (section 14.4 has the
numbers); they were not tuned after the measurement. The choices:

| Choice | Why | Weighed against it |
|---|---|---|
| The three resources are last in the resource index | The picks and the deposit kinds of the first 24 keep their hashes, so their amounts and variants do not move | Placing them by group, which would renumber the resources |
| Reagents are one resource with seven kinds | A generator does not tell alum from vitriol; the variant carries the name. The kinds sit on bedrock the catalogue already names (`patch-resources.md` rows 172, 173, 315, 351) | Six resources |
| Saltpetre and borax share the zone gate of the playa salt | They form in the same dry basins, and no new purpose number is needed | A gate of their own |
| Herbs are a potential with a factor per biome, like honey | Every healer needs them, so they must be common. The wild herbs of wild food stay | Deriving herbs from wild food & game, which gives the healer no siting; removing wild herbs from wild food, which would change the variants of three biomes |
| Arcane materials are one resource with 14 kinds | The 28 fantasy rows of `patch-resources.md` (F1 to F28) were cut to the ones whose placement needs only what a patch already knows. The rows that need a volcanic flag, hot springs, a confluence or a history layer were left out | Three resources (star metal, arcane reagents, wonder materials), as TRADE §5.2 proposed |
| The supernatural power starts at 60 | 60 is the bound of the two highest bands of the field, a tenth of the map. A field that never passes 60 turns the layer off, so no toggle is needed | A toggle in the Parameter Set |
| Arcane materials are in Luxuries; reagents and herbs in Materials | No new group in the readme. The catalogue's rows for star metal (Metals) and the sap of Elanor (Preserver) became variants | A group of their own |
| Gypsum is a deposit kind of Stone, not a name in the pick of the clastic basins | A longer pick list would change the stone variant of every clastic patch | The pick list |
| Reindeer, sled dogs and elephants raise the mounts factor of five biomes | The carter, the herder and the soldier had no animal in the cold and in the tropical forests. This is the one change to the amounts of an existing resource | New variants at the old factor 0, which would never be listed |
| Trade values: arcane materials 0.9, reagents 0.5, herbs 0 | Alum was a monopoly good, herbs are everywhere. The wealth of a patch feeds the Prospect scores, so the Prospects move a little where the new resources lie | Leaving all three at 0, which keeps the Prospects where they are |

---

## 10. Prospects for cities and towns

### 10.1 Seven findings

(SITES §1.)

1. **Site and situation are different questions at different scales.** Situation, which part of the region has food, water and routes, is a patch-level question the map can answer. Site, the exact dry, flat, defensible spot, needs the height raster.
2. **Agriculture explains where people are; trade explains where cities are.** The score should be food catchment × water × (1 + a bounded trade bonus).
3. **The hierarchy is Christaller's K = 4 system, two orders per level** (§4.8).
4. **The 8 km patch is a farming catchment.**
5. **Generators use the same recipe with different constants,** and none is order-independent.
6. **A global greedy pass does not fit a lazy map, but a per-owner one does.** Region borders are divides with a ridge, where real towns rarely sit. Requiring a minimum **border depth** in patch steps keeps towns apart across borders without reading the neighbour.
7. **Sizes follow from catchments.** If each centre houses a fixed share of its market area's population, K = 4 nesting gives Zipf's law with exponent 1.

### 10.2 The evidence

| Study | Finding |
|---|---|
| Bosker & Buringh (European cities 800–1800) | A **navigable river** was the main positive geographic effect: about +6 points on an unconditional chance of about 12 %. Ruggedness lowers the chance. A competitor within 20 km subtracts about 1.5 points; a city 20–100 km away adds about 1 |
| Henderson et al. 2018 | For early developers the agricultural variables add over 6 times the explanatory power of the trade variables. The trade variables are coast, navigable river, major lake and natural harbour within 25 km |
| Bleakley & Lin 2012 | Portage sites on the US fall line are 50–110 % denser today |
| Cooley 1894 | "Population and wealth tend to collect wherever there is a break in transportation" |
| Burghardt 1971 | A gateway sits at one end of its tributary area; a central place sits in the middle [partially verified] |

**Design.** Two kinds of city follow: a **central place** at the centre of its catchment and a **gateway** at a mode change (river mouth, harbour, head of navigation, pass). The city score takes the maximum of a centrality term and a gateway term, so both kinds arise. Defence gets a small weight and is never a necessity, because rugged ground hurts cities (SITES §2.3, §2.4).

Classic site factors and their map signals (SITES §2.1): water supply (a river node or lake in the patch; aridity as a proxy for wells), floodplain against terrace (height above the nearest drainage), defensibility (elevation type, confluence, peninsula), river crossing and lowest bridging point (narrow floodplain), confluence, head of navigation and fall line (a knickpoint, or a river leaving hills for a lowland), natural harbour (a coastal patch next to mostly enclosed sea), estuary (where a river crosses the coast).

Scales (SITES §2.2):

| Map unit | Mean area | Radius | Real analogue |
|---|---|---|---|
| Patch (8 km) | 64 km² | 4.5 km | Farming catchment of 1 hour or 5 km |
| Ring of patches | about 450 km² | about 12 km | Bracton's market reach of a third of a day, 10.7 km |
| Zone (32 km) | 1,024 km² | 18 km | A 20 km one-day round trip |
| Region (128 km) | 16,384 km² | 72 km | 2–3 days on foot |

### 10.3 Methods

- **Weighted linear combination** lets a surplus in one criterion make up for a deficit in another, which is wrong for necessities: a site with no water is not half good. **Design:** a multiplicative core of necessities times bounded bonuses (SITES §3.1).
- **Walking time.** Tobler: `W = 6·exp(−3.5·|S + 0.05|)` km/h. Naismith: 5 km/h plus an hour per 600 m of ascent, which is linear and bit-exact (SITES §3.3).
- **Hansen accessibility.** `A_i = Σ S_j · f(d_ij)`; with `f = 1/(1 + c/c₀)²` it needs only basic arithmetic (SITES §3.4).
- **HAND,** height above the nearest drainage: 0–5 m is a near-surface water table, 5–15 m shallow, above 15 m deep. The terrain pipeline already stores the height each cell's path starts from, so HAND costs no extra pass. A good village site has a HAND of about 3–15 m within about 1 km of a river (SITES §3.5).

### 10.4 How generators and games do it

| | Azgaar | O'Leary | Civilization V | Civilization IV |
|---|---|---|---|---|
| Food | Biome habitability (forest 100 … desert 4) | — | Ring-weighted yields; food 15 against production 3 | Tile yields |
| River | Normalised flux and confluence × 250 | √flux | +15 % | +40 |
| Coast | +5; estuary +15; safe harbour +20 | — | +40 % | Water-tile terms |
| Defence | −(height − 50)/5 | — | Hills +3 | Hills +200 |
| Spacing | A quadtree radius that shrinks on failure | −0.02/distance to each city | A sweet spot of 5 tiles | A penalty band, 4 times steeper for "too close" |
| Randomness | `Math.random`, Gaussian jitter | None | None | None |

(SITES §4.) All rely on a global pass. Civilization IV's asymmetric spacing penalty has the same shape as Bosker & Buringh's findings.

### 10.5 The research design

**Design** (SITES §6). Two tiers:

| Tier | Decides | Inputs | Rule |
|---|---|---|---|
| **M (map)** | Which patches hold a village, market village, town, county town or city; rank and population | The map, elevation types, coast, the region's rivers and bed heights, climate, biome on **estimated** height | A pure function of the region |
| **T (terrain)** | The exact point inside the chosen patch | The height tile: height, HAND, slope, relative height | Never changes a Tier M choice |

Why Tier M must not read exact heights: the biome of a patch is an estimate until its tile arrives. A choice that read it would change when the user zooms in.

- **Features per patch:** elevation type, climate and biome, coast, lake, harbour, the largest drainage in the patch, the drainage at a river mouth, a confluence measure, head of navigation, fall line, bridging point, and border depth.
- **Score terms:** food density K (people per km² by biome and elevation type, plus irrigation and fish), water W and climate comfort C as necessities, material M, defence D, trade X with diminishing returns (`max + 0.25 × rest`), and Hansen centrality.
- **Composites:** `village = P_v · W · C · (0.8 + 0.2·M) · (1 + 0.2·X)`; the city score ranks towns by `max(centrality, gateway)`.
- **Selection inside one region:** villages above a threshold; one market town per zone by greedy suppression at 0.6 × the K = 4 spacing; up to 3 market villages per zone; one city among towns at depth ≥ 2; up to 3 county towns.
- **Population:** `pop_k = a · P(market area) · (1 + γ·X)` with an urban share a = 0.035 per order, about 11 % urban in total; a temperate region of about 330,000 people gets a city of 12,000–25,000.
- **Tests:** order independence, the tier contract, locality, nesting caps, spacing, geography sanity (at least 60 % of cities on water), a rank-size slope between −1.4 and −0.8.

### 10.6 What is adopted

The generator uses Tier M "in a first, simpler form": a ranked list of scored sites, not a settlement hierarchy with populations.

| Element | Research design | Adopted |
|---|---|---|
| Output | Five orders with populations | About 20 Prospects per region: up to 4 for cities, the rest for towns |
| Candidates | Land patches of the region | Same; a candidate must not touch another region's land |
| Inputs | Map only, estimated biomes | Same: rivers, lakes, elevation types, climate, estimated biomes and the resource layer; no terrain tiles |
| Score | A product of necessities times bounded bonuses | `100 × need × weighted mean of five factors`, with `need = min(fresh water, temperature comfort) × 2`, capped at 1. Necessities still veto; the factors compensate each other |
| Factors | K, W, C, M, D, X, centrality | Resources 30 %, geography 15 %, trade & travel 25 %, climate 15 %, prosperity 15 % |
| Food | Calibrated density per biome | Read from the resource layer: grain, roots, herds, orchards and game of the catchment |
| Trade | `max + 0.25 × rest` over gateway features; Hansen centrality; the maximum of the two | The same construction: the better of a gateway and a central place |
| Trade goods | Open question 7: should salt or iron add to the trade term? | Yes: salt, ores, gems, spices, furs, dyes and horses count as trade goods of the resources factor |
| Navigability | From 30 patches drained; large from 120 | From 8 patches drained; large from 80 |
| Spacing | Per order, by K = 4 | Cities 2.5 × the spacing apart and two patches inside the region; towns at least 20 km apart, relaxed step by step when fewer than 20 fit |
| Cross-border | Border depth | Border depth |
| Spot | Tier T on the raster | On the patch's river (the mouth, or where the river is largest) or at the patch's site |
| On show | Villages above a threshold | The sum of the scores of a region's Prospects sets how many are on show: 1 at a sum of 700 or less, all 20 at 1,500 or more, the best by score first. A fixed cutoff (score of at least 59) came first; it left 15 of 600 measured regions that have Prospects with none on show, and the number on show followed single scores, not the region as a whole. The two sums are near the 5 % and 93 % points of the measured sums, which keeps the median at 13 on show |
| Class | Five orders | Rich from a score of 75, Poor below 62, Average between: about a quarter and a fifth of the Prospects on show. It only colours the map so far |

| Topic | Status |
|---|---|
| Per-region, order-independent scoring and selection | Adopted |
| Gateway and central-place terms; Hansen accessibility | Adopted |
| Resources factor from the resource layer | Adopted |
| Settlement hierarchy (villages, market villages, county towns) and populations | Open |
| Rank-size check | Open |
| Fall lines and lowest bridging points (need bed heights) | Open |
| Tier T: site refinement by HAND, slope and domination; pass towns | Open |
| Settlement potential replacing the prosperity noise | Open (prosperity is an input instead) |

### 10.7 Races and potential population

This section rests on a later note, RACES (see §1). It was added on 2026-11-14 with the rule that is now [society-generation.md](society-generation.md) sections 2 and 3 (it was map-generation.md section 19 until 2026-11-15).

**What the note found.** The core roster is stable: humans are in all 20 surveyed rosters, elves and dwarves in 17, orcs in 12, goblins in 10 and halflings in 9 (RACES §1.3). The strategy games make habitat a rule: each race has terrain or climate it likes, dislikes or cannot live in (RACES §1.4). The note turns that into a table of habitat weights from 0 to 3 for 23 races and the 16 biomes (RACES §4.1). The weights are the note's own judgement, anchored on sourced claims.

**Design** (decided with the user, 2026-11-14):

| Question | Adopted | Weighed against it |
|---|---|---|
| Where the races live | A list in the Parameter Set, like the bedrock types. Each race has an id, a name, a toggle, a frequency per biome and a height modifier | A catalogue in the code with only the toggles in the Parameter Set: a tweak would need a new generator version. A race file beside the Parameter Set: the foundation would reference two things |
| Tweaks after phase 1 | A changed list is a new Parameter Set and a new foundation | Reopening the Parameter Set of an existing foundation: it would break "derived data is a pure function of the foundation" |
| Which races | 28 races, all on: seven human races, five classic races and sixteen new near-human races, described in [races-basis.md](races-basis.md). No plain human race and no sub-races (decided by the user, 2026-11-15) | The first list (2026-11-14): all 23 of RACES §4.1, with the six most common on (human, elf, dwarf, halfling, orc, goblin). Only those six in the list; all 23 on; humans only on. The 32 races of the first version of races-basis.md: the user left out five of them |
| From frequency to persons | The land has one capacity per patch, and the races that are on share it by their frequency in the biome | Each race on its own (frequency × food × Prosperity): the total of a region would grow with every race that is toggled on, so the toggles would also tune the size of the world |
| Capacity | Area × 0.06 persons per km² × food supply × a Prosperity factor of 0.5 to 1.5 | The calibrated density per biome and elevation type of SITES §6 (the K term): the resource layer already holds the food, as for the Prospect scores (§10.6) |
| Height | A height modifier per race: control points from the height of a patch to a factor on the biome frequencies, with a step where two points share a height. Asked for by the user: dwarves only from 200 m and up, elves only below 200 m | A lowest and a highest height per race: it cannot fade a race in. The elevation type weights of RACES §4.2: the user asked for a limit in metres |
| Which height | The mean height of the patch in the height map | The typical height of the elevation type (70, 140, 270 and 470 m): a limit at 200 m would then only tell hills and mountains from flat land and lowland |
| How many races a region holds | Up to 6: its 4 main races, the largest by the plain shares, and 2 minority races. A city holds up to 5 of them and a town up to 4. Asked for by the user on 2026-11-15 as 5, 4 and 3 with one boosted race, and one more of each if that proved too tight. It did | Every race in every region: a median region held 20 races at 1 % or more, and no region had a people of its own. 5, 4 and 3 with one boosted place: a cluster of 30 regions holds all 28 races in 23 % of the cases. A fifth main race instead of a second minority race: 25 to 28 %, since the largest races are the same nine nearly everywhere. Five races with 3 main and 2 minority: the same per region, but fewer clusters hold every race in a city or a town |
| Who gets a minority place | The race whose turn it is in the region, when the land feeds 0.1 of it. The turns form a lattice over the region grid, so every 6 × 5 block of regions holds a turn for each of the 28 races in each place. Otherwise a race drawn by its plain persons. All 28 races in 96 % of the clusters of 30 regions | A draw weighted towards the rare races: 23 of 28 races in a cluster, never all. The race that is most over-represented in the region: neighbouring regions pick the same race, 19 races. Turns from a hash order inside 7 × 4 blocks of regions: all 28 in half of the blocks. Looking at what the neighbouring regions picked: the Planned stage would need the neighbours Planned, and the result would depend on the order of generation. A floor as a share of the region instead of a number of persons: the Shoalfolk and the Hoarkin fail it in most regions |
| The size of a minority race | Lifted towards 5 % of the region, by a factor on its weights. A race that lives on a few patches takes those patches and stays below | No lift: a tenth of the minority races hold about 1 % of their region or less |
| Land where no race of the region lives | It holds nobody: 0.9 % of the capacity, 16 % in the worst region | Handing the patch to the races of the region: it would put Midlanders on an ice cap |
| The races of a city or a town | Races of its region: the largest around the Prospect (its patch and the land patches around it, and 2 % of the region), and a last one drawn with the minority races weighted up 5 times. A minority race is in a city or a town on show of its region in 97 % of the cases | The largest only: the minority races would live in the countryside and seldom be met |
| Biomes and resources read | The final ones, calculated with the same height. The region is Elevated when it is Planned, so they are there | The estimated ones, as the Prospects read: the biome of a patch would come from the typical height and its dwarves from the real one. The first version read the estimated ones, before the height modifier |

**The default frequencies.** A frequency is a habitat weight from 0 to 3 times how common the race is. The five classic races and the Catfolk keep the habitat weights of RACES §4.1. The weights of the seven human races and of the other fifteen new races are those of races-basis.md. `common` is 3 to 5 for the human races (27 in all), 2 for Elves, Dwarves, Halflings and Orcs, 1 for Gnomes, and 1 or 0.5 for a new race. All of it is judgement, it is not sourced, and it has not been tuned after the measurement. With the five height modifiers the plain shares of the list give 59 % to the human races, 20 % to the classic races and 21 % to the new races (races-basis.md, "Measured shares"). With six races per region the persons are 83.5 %, 10.4 % and 6.1 %: the main races are human in most regions. 39 % of the land patches lie at 200 m or higher. The Catfolk row was written for the first list. It has a weight in 13 biomes and makes the Catfolk the largest new race by the plain shares, with 3.4 %. The first list gave 66 % humans, 4.5 % elves and 2 % dwarves: its one human race had `common` 10 and the other five that were on had 2.

**The density.** 0.06 persons per km² at a food supply of 1 gives a median region of about 364 persons. The first default was 60, which gave about 351,000 and matched the 330,000 that SITES §6 expects for a temperate region. The user found that world too populated and asked for a tenth of it (2026-11-14), and then for a hundredth of that (2026-11-15), so the default no longer follows SITES §6. The persons a race needs for a minority place went from 10 to 0.1 with the second change, so the same races hold the same regions. The measured persons of races-basis.md and society-generation.md were not measured again: they are the numbers at 6 persons per km², divided by 100.

| Topic | Status |
|---|---|
| A list of races in the Parameter Set, toggled per race | Adopted |
| Potential population per region and race from biome frequency, food and Prosperity | Adopted |
| A height modifier per race | Adopted, with defaults for five races: Elves, Dwarves, Hearthbloods, Widegaze and Shoalfolk |
| Elevation type, bedrock, water and Supernatural weights per race (RACES §4.2, §5.2) | Open |
| At most six races per region, five per city and four per town, with the minority races by turn | Adopted (2026-11-15). The test is in [plans/race-caps.md](plans/race-caps.md) |
| A presence field per race, so that a race clusters in some regions and is absent from others (RACES §5.4) | Open. A race is absent from most regions now, but the turns spread a minority race evenly: it does not cluster |
| Variants of a race picked by the biome (RACES §5.3) | Rejected: a race has no sub-races (2026-11-15) |
| The race of a faction (RACES §5.4) | Adopted (2026-11-19): a faction has a lead race, and a faction of the type Race holds that race only (§10.8) |
| The race of a family and a person; mixed families (RACES §5.4, §5.5) | Open |
| Size, lifespan and fertility per race, and a density that follows from them (RACES §5.1) | Open |
| Underground and sea peoples (RACES §5.6) | Open |

### 10.8 Factions

This section rests on two later notes, FACTIONS and FACGEN (see §1). It was added on 2026-11-19 with the rule that is [society-generation.md](society-generation.md) section 4. The test of the rule, with every number, is in [plans/region-factions.md](plans/region-factions.md).

**What the notes found.** Nearly every fantasy faction is a medieval institution with one twist, and factions split cleanly by what binds their members (FACTIONS §1, §2.1). The one generator that publishes its weights draws a monarchy for 89 % of its inland states and a republic more often on the coast, and gives rule by a faith a roll of 10 %; resources, wealth and the peoples do not enter, and it has no opposition and no guilds (FACGEN §2.1). Dwarf Fortress lets the people decide how a polity is built (FACGEN §2.2). In the scholarship hierarchy follows grain, which can be seized, not fertile land as such (FACGEN §3.1); rough and marginal land is where states did not reach (§3.2); merchant republics are small and mercantile (§3.5); a mine gave its miners a charter, not the rule (§3.6); civil war follows rough terrain, poverty and numbers, and not diversity (§4.1); wealth that can be taken feeds a rising (§4.3); the lasting rivals are few (§4.4, §4.5); and guilds follow towns, with the merchants first and primary production rarely guilded (§5). The notes give directions, not sizes: every weight of the rule is this project's own judgement.

**Design** (proposed and adopted with the user, 2026-11-19):

| Question | Adopted | Weighed against it |
|---|---|---|
| The types | Six, by what binds the members: Family, Race, Vocation, Belief, Magic and Place. The first five were asked for by the user. Each rests on a fact of a person | The nine bases of FACTIONS §2.1: arms and crime are bound by work, and a cause is a role. The five types of FACGEN §6.2: they leave out the free town and the commune, which six races form first. The 48 archetypes drawn directly: 48 weights against 18 indices cannot be tuned by eye |
| The roles | Ruling (exactly one in a region with persons), opposition (none or one) and lesser (up to four). Every type can hold every role | A ruling faction for each race of a region: six rulers of 60 persons each, in a region the size of one kingdom. The five stances of FACTIONS §2.3: outlawed and independent factions away from the Prospects are left open |
| What decides the ruler | The fit of the land for each type, from 18 indices of the region, times the leaning of each race, times its share of the persons. The lead race and the type are drawn together | The best fit always: a house in 60 % of the regions and a merchant republic in 3 of 1,154, since a region spans many biomes and its averages are flat. Grain against roots (FACGEN §3.1): the share of Grain in Grain and Produce is 0.43 to 0.48 in four regions in five. Sea access: 94 % of the regions have it |
| Who leads | The lead race is drawn with its share to the power 5: the largest race in 54 % of the regions, a minority race in 1 % | Only the largest race: a foreign ruler is a story worth having. The power 2: a minority race rules 5 % of the regions, and rulers of small races are everywhere |
| Whether there is an opposition | A tension from five terms: contest (another type fits nearly as well), rough land, a rival seat, precious finds and poor land. An opposition from a tension of 0.5, with a random part: 42 % of the regions | How divided the region is by race (FACGEN §4.2): measured, the polarization is 0.61 to 0.68 in four regions in five, since every region holds six races of about the same size. The races decide who opposes, not whether |
| Who opposes | A draw by the share and the leaning of each race, a cleavage table from the type of the ruler and what each term of the tension favours | The runner-up of the ruling draw alone: the opposition would not follow the land that causes it |
| Ore | A miners' guild under the ruler, and tension from precious finds | A mining league as a ruler: the first version had miners ruling 5 % of the regions, and the sources know chartered miners under a lord (FACGEN §3.6) |
| The lesser factions | Twenty kinds, fifteen of them guilds of one trade, taken by their fit to the resources of the region, as many as its persons carry. The merchants have a floor and primary production a factor of 0.9 | Guilds from the vocations of each city: the better rule, but no vocation is drawn yet. The kinds as a list in the Parameter Set: their fits are formulas, so they are a table in the code for now |
| The rare races | The lead race of a lesser faction is drawn with the minority races weighted up 5 times, and an enclave is a people other than that of the ruler. A minority race leads 45 % of the lesser factions | No boost: 23 %. The Elves, the Dwarves and the new races rule next to no region, so without the lesser factions they would lead nothing |
| The Supernatural | The mean over the land of how far the field lies above 60, read from the 25th percentile to about the 95th | The amount of Arcane materials, which the test of the plan used: with its range the real field gave a faith to 19 % of the regions and the mages to 14 %, until the range was set from the measured values. The field has features of 10 km, so every region holds some and regions differ little |
| The neighbours | Nothing is read from another region: a region is Planned from itself | Leagues, shared churches and relations across borders: the Planned stage would need the neighbours Planned, and the result would depend on the order of generation. They belong to the Settled stage |
| The capital | The seat of the ruling faction: the best Prospect on show that holds its lead race. It is the best Prospect of the region in 94 % of the regions | County towns as faction capitals (SITES §6.14): the zones play no part in the rule |

| Topic | Status |
|---|---|
| Six types and three roles of faction | Adopted (2026-11-19) |
| A ruling faction for every region with persons, from the land and the races | Adopted |
| An opposition from the tension of the region | Adopted |
| Lesser factions from the resources and the persons of the region | Adopted, with the kinds in the code |
| A leaning to each type per race, in the Parameter Set | Adopted, as starting values read from races-basis.md |
| The archetypes of FACTIONS §3 as the forms of a faction | Partly: 17 race-neutral forms. The archetype and the race-specific name are left to the authoring |
| Wilderness factions away from the Prospects (FACTIONS §5.3) | Open |
| Factions that span regions, and relations across borders (FACTIONS §5.5) | Open |
| The kind of opposition from where the wealth lies (FACGEN §4.3) | Open: the rule reads how much precious ore a region has, not where |
| Belief as a concept: which faith a faction of the type Belief holds | Open |
| Guilds from the vocations of the cities and towns | Open, until the vocations are drawn |

---

## 11. Travel and trade

### 11.1 Transport cost

- **Water beats land.** Adam Smith: a waggon with two men and eight horses carries about four tons from London to Edinburgh in six weeks; a ship with six or eight men carries two hundred tons in about the same time. Per ton, the land route needs 12–17 times the crew (TRADE §4.2).
- **ORBIS:** in the Roman network "cost, rather than distance, is the principal determinant of connectivity". The often-quoted sea : river : land ratio of 1 : 5 : 28 could not be traced to Scheidel's text [unverified].
- **Medieval England:** land carriage about twice the cost of river carriage [partially verified].
- **Design.** Mode costs per km of roughly sea 1, river downstream 2–3, river upstream 4–6, road 10–20, trackless land 20–40. The ratios matter more than the values. A **mode change costs a fixed transshipment charge**, which is what makes break-of-bulk points attract cities in a routing model (TRADE §4.2, SITES §3.3).

### 11.2 Economic geography that the map can use

| Idea | Statement | Use |
|---|---|---|
| Comparative advantage (Ricardo) | Trade follows relative cost, not absolute | A region exports what its potentials are relatively best at, by location quotient |
| Location theory (Weber) | Weight-losing processing sits at the raw material | Places tier 2 goods |
| Rings around a market (von Thünen) | Rent falls with distance; perishable and heavy goods stay close | Produce is not traded beyond a zone; timber travels far only by water |
| Central places (Christaller) | Higher orders are fewer and farther apart | The settlement hierarchy |
| Core and periphery (Krugman) | Cheap transport concentrates manufacturing | A pre-industrial world should have many small manufacturing towns, concentrated only at sea and river hubs |

(TRADE §4.) Three goods are short almost everywhere: **salt**, **iron** (ore and fuel must meet) and **timber and fuel** [unverified as stated].

### 11.3 The travel model in the research and in the generator

SITES §6.8 proposed a graph with one node per land patch, Gabriel edges and water edges. The generator built the travel network on a finer structure:

| Element | Research proposal | Adopted |
|---|---|---|
| Structure | The patch graph (8 km) | A 2 km cost grid per region, with heights estimated the way the height map builds them (beds by the stream-power law, land rising at its elevation type's hillslope). It needs no terrain tile |
| Land cost | Naismith time × roughness by elevation type × vegetation | Per km: elevation type (flat 1, lowland 1.15, hills 1.8, mountains 4) × biome (grassland 1, forest 1.3, rain forest 2.2, mangrove 3, deserts 3.5–4, ice 5) × slope (1 + (slope / 5 %)², at most 13) × height (twice at 1,500 m) |
| River | Downstream faster than upstream, on navigable chains | 0.4 per km on navigable rivers, plus 6 km at each end |
| Sea | The cheapest mode | 0.3 per km (0.6 at the shore), plus 12 km at each end |
| Mode change | A fixed charge | The per-link loading costs, so short hops go by road and long hauls by water |
| Crossings | A bridging point as a feature | +3 km to cross a navigable river; × 8 across a lake |

In units of open flat road the adopted ratio is about sea 1 : river 1.3 : road 3.3, rising to 13 and more through mountains or rain forest. That is flatter than the researched 1 : 2–6 : 10–40; the terminal charges do part of the work.

**Which places are linked** is the generator's own design. Two shortest-path forests are grown over a region and its eight neighbours, one over land and one over water. Each cell ends up with its cheapest Prospect, a Voronoi diagram under travel cost. Two Prospects whose cells touch get a candidate link, the cheapest route through their common border. A link is dropped when cheaper links make a detour of at most 1.6 × its cost; a road gives way to water only when the water route costs at most 0.35 × the road. Every pair of regions has one owner, so the network does not depend on the order regions are computed in.

### 11.4 Prices and flows

The full spatial price equilibrium is global: every market's price depends on every other's. Two local models fit a lazy map (TRADE §5.7):

- **Model A, market-potential price.** A region's price for a resource depends only on the **static** supply and demand of the regions in its k-ring, weighted by `1 / (1 + cost / c₀)`, put through Victoria 3's clamped price formula. It never reads another region's computed price, so it is order-independent.
- **Model B, flows between neighbours.** A resource flows from i to j when the price gap exceeds the transport cost. To stay order-independent, each exporter's surplus is shared among importers in proportion to their margin, for a fixed number of rounds, each reading only the previous round.
- **Value density.** Bulk goods (base price 2) reach about one zone by land and many regions by sea; trade goods (5) about one region; luxuries (20) go anywhere. Von Thünen-like rings then appear around cities without being placed.
- **One direction.** Settlement reads static supply and topology; prices read settlement populations as demand. No step reads its own output, so there is no fixed point to iterate. The one optional feedback, a trade bonus to town size, runs once on a frozen earlier round and changes sizes but not sites (SITES §6.9).

Generation order (TRADE §5.8): potentials and deposits per patch → zone sums, market town and processing → region balances, export, deficits, city → prices, then flows.

| Topic | Status |
|---|---|
| Travel network: roads, river routes, sea lanes; terrain-following routes | Adopted |
| One owner per pair of regions; one connected network | Adopted |
| Upstream and downstream river costs | Open |
| Routing on the real height raster; bridges, fords and passes as places; road classes | Open |
| Goods, prices (Model A), flows (Model B) | Open |
| Needs tiers for settlements | Open |

---

## 12. Research not yet used: a backlog

Ordered by layer. "Value" is the notes' own assessment where they give one.

| Layer | Item | Value and cost | Source |
|---|---|---|---|
| Rivers | Tropism (an inland bias) | One knob from dendritic to parallel drainage; trivial | RIVERS §4.2 |
| Rivers | Half-way relaxation and Chaikin smoothing | Smaller junction angles, smooth curves; a pure post-pass | RIVERS §8.4 |
| Rivers | Blue-noise attractor spacing | Smoother trunks; low cost | RIVERS §2.1 |
| Rivers | Junction angle by patch moisture (45° to 72°) | Climate shows in the river pattern | RIVERS §7.1 |
| Rivers | Hack exponent and bifurcation ratio as diagnostics | A cheap realism gauge | RIVERS §7.2 |
| Rivers | Meanders on large rivers | Polynomial shape keeps it bit-exact | RIVERS §7.4 |
| Height | Level the lakes; show filled pockets | Consistency between layers | HEIGHT §9 |
| Height | Pinned stream-power relaxation (Option C) | Sub-resolution gullies; high cost | HEIGHT §4.6 |
| Rendering | 2D raster as a tile pyramid | Makes panning incremental; the larger change | TILES §2 |
| Climate | Orographic precipitation with a prevailing wind | Rain shadows; needs a bounded halo | BIOMES §4.4, §7.2 |
| Climate | A seasonality amplitude field | Unlocks biotemperature, Köppen, Mediterranean and monsoon climates, a treeline rule | BIOMES §7.2 |
| Biomes | Alpine, flooded grassland, savanna-or-forest by hash | Azonal variety | BIOMES §7.2 |
| Biomes | Wetlands and peat bogs as overlay flags | Unlocks reeds, bog iron, peat | RES §3.1 |
| Resources | Tier 2 and 3 goods placed by the material index | Completes the 39-resource set | TRADE §5.2 |
| Resources | Signature export and deficit checks | Guarantees trade | TRADE §5.5 |
| Resources | Fantasy layer triggered by the supernatural field | Done as the arcane materials (§9.9) | RES §5 |
| Prospects | Hierarchy and populations with a rank-size test | Turns scored sites into settlements | SITES §6.5–§6.7 |
| Prospects | Tier T site refinement; fall lines; bridging points | The exact spot; needs bed heights and the tile | SITES §6.6 |
| Trade | Model A prices, then Model B flows | Order-independent trade on the existing network | TRADE §5.7 |

---

## 13. Open questions

Collected from the notes; questions that have since been settled are left out.

**Cells and structure**
- Should the distance metric be a parameter of the levels (CELLS, GAPS §6)?
- How strongly should a later local layer (towns, dungeons) obey the map: hard constraints or biased weights (CELLS §10)?

**Rivers**
- Which extensions did Feng et al. 2023 use, and with what parameters? The paper could not be read (GAPS §7).
- Should navigability thresholds scale with precipitation, so desert rivers are not navigable at the same drainage area (SITES §6.14)?

**Height**
- Fill walled-off pockets as lakes, or lower the ridge there (HEIGHT §5.1)?
- Is graph-level or pinned erosion worth its cost at this scale (CELLS §10, HEIGHT §9)?

**Climate and biomes**
- Is the game-scale climate (the whole sequence within one or two zones) intended, or should the fields be smoother (BIOMES §7.1)?
- Estimated against exact biome: how often does scoring on a typical height change a patch's belt or moisture class (SITES §6.14)?
- The steep cooling curve against ECOCROP altitude limits (RES §7.5).

**Resources and trade**
- Yields and carrying capacity per staple and biome: the catalogue gives occurrence, not quantity (RES §7.5, TRADE §5.9).
- Market reach: neighbouring regions only, or two rings; do sea links count as adjacent regardless of distance (TRADE §5.9)?
- Static or depleting deposits; dormant late-technology resources (TRADE §5.9, RES §7.5)?
- Is seven groups and 39 resources the right player-facing count (TRADE §5.9)?
- Should the fantasy layer be a toggle, and can it be a region's signature export?

**Settlements**
- Should passes feed the travel graph as cheap land edges (SITES §6.14)?
- Should county towns seed the seats of the lesser factions? The capital is the seat of the ruling faction (§10.8). Zones are "also meant for factions" (SITES §6.14).
- Which Christaller distance series is right, the 4 km or the 7 km one? The translation that would settle it was not reachable (SITES §2.5).

**Races**
- Is race a Stub fact of a person and a family, or a "people" layer under the factions (RACES §5.6)? A faction has a lead race (§10.8).
- Should a rare race rule a region now and then? Today the large human races lead nearly every ruling faction (§10.8).

**Factions**
- Should the rule read the land around each Prospect instead of the whole region, which is flat (§10.8)?
- Should a faction span regions, as a church, a merchant league and a horde do in every source (FACTIONS §5.5)?
- What does a faction of the type Belief believe in?
- Should a race have one number for how common it is, apart from its frequency per biome? Today the default frequencies carry both (§10.7).
- A dwarven region scores low on food. Should a race change the capacity of the land, or only its share of it (RACES §5.6)?

---

## 14. Numeric quick reference

**Map scales**

| Unit | Size | Mean area | Count per parent |
|---|---|---|---|
| Patch | 8 km | 64 km² | 16 per zone |
| Zone | 32 km | 1,024 km² | 16 per region |
| Region | 128 km | 16,384 km² | — |
| Terrain tile | about 256 km, 24 km halo | — | 2 × 2 regions |
| Terrain cell | 1 km (2 km or 500 m) | — | — |
| Travel cell | 2 km | — | — |

**Rivers**

| Quantity | Value |
|---|---|
| Step, influence, kill (defaults) | 1 km, 47.8 km, 5 km |
| Attractors per patch | 10; one stands for about 6.4 km² |
| Width exponent | 0.4 (hydraulic geometry: 0.5) |
| Concavity θ | 7⁄16 (reference 0.45–0.5) |
| Junction angles | 45° arid to 72° humid |
| Hack's law | Length ∝ area^0.6 |

**Climate and biomes**

| Quantity | Value |
|---|---|
| Lapse rate | 6.5 K/km standard; 6 Holdridge; about 4–6 observed |
| Köppen dryness threshold, even rain | 20·T + 140 mm; desert below half |
| Thermal belts, as adopted | −10, −3, 3, 18 °C |
| Aridity classes, as adopted | 0.5, 1, 2, then 6 (temperate) or 4 (tropical) |
| Holdridge guide lines | 1.5, 3, 6, 12, 24 °C; 62.5 mm doubling to 8,000 |
| Holdridge PET | 58.93 mm × biotemperature |
| Treeline | 6.4–6.7 °C growing-season mean; biotemperature 3 °C |
| Savanna–forest bistable band | 1,000–2,000 mm (Africa) |
| Mangrove, as adopted | T ≥ 20 °C, m ≥ 0.5, coastal, flat or lowland |

**Settlement and travel**

| Quantity | Value |
|---|---|
| K = 4 spacings | 4, 8, 16, 32, 64, 128, 256 km |
| Farming catchment | 1 hour, about 5 km |
| Market reach (Bracton) | 6⅔ miles ≈ 10.7 km |
| Navigable-river effect on becoming a city | about +6 points on about 12 % |
| Portage sites | 50–110 % denser |
| Rank-size exponent | about 1 (national means about 1.1) |
| Urban share, Europe | about 3 % (800) to about 15 % (1800) |
| Travel per day | Horse 32–48 km, legion 24–32 km, hiker 16–24 km |
| Researched freight ratios | Sea 1 : river 2–6 : road 10–20 : trackless 20–40 |
| Adopted costs per km | Sea 0.3, river 0.4, flat road 1, mountain road 4 |
| Navigable, as adopted | From 8 patches drained; large from 80 |

**Resources**

| Quantity | Value |
|---|---|
| Catalogue | About 370 real and 28 fantasy resources |
| Game set | 39: 24 raw, 9 intermediate, 6 finished; 7 groups |
| Deposit chances per eligible patch | Common 0.3–0.6, uncommon 0.05–0.2, rare 0.005–0.03, very rare < 0.002 |
| Listed from an amount of | 0.25 |
| Victoria 3 price range | 25–175 % of base |

---

## 15. Sources

The notes hold the full citations with sections, lines and commit hashes, and say for each source how much of it was read. The main external sources by topic:

**Cells, hashing and noise**
- Worley, "A Cellular Texture Basis Function", SIGGRAPH 1996.
- Bridson, "Fast Poisson Disk Sampling in Arbitrary Dimensions", SIGGRAPH 2007.
- Jarzynski & Olano, "Hash Functions for GPU Rendering", JCGT 9(3), 2020.
- Perlin, Improved Noise, 2002; Gustavson, "Simplex noise demystified", 2005.
- Quilez, articles on Voronoi edges, distance functions, smooth minimum, domain warping and fBm.
- ECMA-262, §6.1.6.1 and §21.3; bryc, notes on JavaScript PRNGs.
- Patel (Red Blob Games), mapgen2 and mapgen4; O'Leary, `mewo2/terrain`; Azgaar, Fantasy Map Generator.

**Rivers**
- Runions et al., "Modeling and visualization of leaf venation patterns", 2005; Runions, Lane & Prusinkiewicz, "Modeling Trees with a Space Colonization Algorithm", 2007.
- Palubicki et al., "Self-organizing tree models for image synthesis", 2009.
- Feng, Wünsche & Shaw, "Generating Realistic River Patterns with Space Colonization", WSCG 2023 [abstract only].
- Hack 1957; Leopold & Maddock 1953; Langbein & Leopold 1966; Seybold, Rothman & Kirchner 2017; Devauchelle et al. 2012.

**Height**
- Mudd et al. 2014 (stream-power law and the χ integral); Perron & Royden 2013.
- Génevaux et al., "Terrain generation using procedural models based on hydrology", 2013.
- Hnaidi et al. 2010; Prusinkiewicz & Hammel 1993; Kelley, Malin & Nielson 1988.
- Barnes, Lehman & Mulla 2014 (Priority-Flood); Barnes 2016.
- Hellweger & Maidment 1997 (AGREE).
- Cordonnier et al. 2016; Schott et al. 2023; `fastscapelib`.

**Climate and biomes**
- Holdridge, *Life Zone Ecology*, 1967.
- Peel, Finlayson & McMahon 2007; Beck et al. 2018 (Köppen–Geiger).
- Ricklefs, *The Economy of Nature*, Fig. 5.5 (Whittaker's diagram); the `plotbiomes` digitisation.
- Dinerstein et al. 2017 (WWF biomes); Keith et al. 2022 (IUCN typology).
- Zomer, Xu & Trabucco 2022 (aridity index); U.S. Standard Atmosphere 1976.
- Körner & Paulsen 2004; Roe 2005; Staver, Archibald & Levin 2011.

**Resources**
- FAO ECOCROP (as `EcoCrop_DB.csv`); the `Recocrop` manual.
- Cox & Singer (eds.), *Mineral Deposit Models*, USGS Bulletin 1693, 1986.
- USGS Mineral Commodity Summaries 2025.
- Game sources: Civilopedia (Civilization VI), the Paradox wikis (EU4, CK3, Victoria 3), the Patrician IV manual, Anno Union and TaleWorlds developer blogs, the Factorio and RimWorld wikis.

**Settlement, travel and trade**
- Smith 1776; Ricardo 1817; von Thünen 1826; Weber 1909; Christaller 1933; Krugman 1991.
- Cooley, "The Theory of Transportation", 1894.
- Bosker & Buringh, "City seeds", 2017 (2012 version read); Henderson et al. 2018; Bleakley & Lin 2012.
- King, *Central Place Theory*, 1984; Skinner 1964; Gabaix 1999; Soo 2004.
- Tobler 1993; Hansen 1959; Emilien et al. 2012; Nobre et al. 2011 (HAND).
- Scheidel, ORBIS working papers 2013 and 2015.
- Code read: Azgaar's population and burg generators; O'Leary's `terrain.js`; the Civilization IV and V site evaluators.
