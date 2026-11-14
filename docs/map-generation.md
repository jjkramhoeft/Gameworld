# How the map is generated

This document describes every step that turns a foundation seed into the map: the cell
hierarchy, oceans, elevation types, fields, rivers, lakes, the height map, biomes,
resources, Prospects for cities and towns and the travel network. It is the specification
of the generation algorithms. The [readme](../readme.md) holds the domain model around
them (foundation, Parameter Set, identity, region generation stages), and the names in
this document follow the readme. Default parameter values are given in parentheses or in
tables. The parameters and their defaults are collected in
[default-parameter-set.json](default-parameter-set.json), grouped by region generation stage.

Each section states the region generation stage that its result belongs to. The reasons
behind the algorithms are in [map-generation-research.md](map-generation-research.md).
The algorithms were first worked out in an earlier prototype called Sandbox 4.

## Contents

1. [Overview](#1-overview)
2. [Basics: units, hashing, random numbers, noise](#2-basics)
3. [One level of cells: a jittered-grid Voronoi diagram](#3-one-level-of-cells)
4. [The three-level hierarchy](#4-the-three-level-hierarchy)
5. [Oceans](#5-oceans)
6. [Zone and patch mergers](#6-zone-and-patch-mergers)
7. [Classifying a point](#7-classifying-a-point)
8. [Elevation types](#8-elevation-types)
9. [Fields](#9-fields)
10. [Rivers](#10-rivers)
11. [Lakes](#11-lakes)
12. [Height map](#12-height-map)
13. [Biomes](#13-biomes)
14. [Resources](#14-resources)
15. [Prospects for cities and towns](#15-prospects-for-cities-and-towns)
16. [Travel network](#16-travel-network)
17. [Drawing the map](#17-drawing-the-map)
18. [Order of work and pitfalls](#18-order-of-work-and-pitfalls)

Appendices: [A. Exact generators](#appendix-a-exact-generators) ·
[B. Crop tables](#appendix-b-crop-tables) ·
[C. Factors by biome](#appendix-c-factors-by-biome) ·
[D. Resource rules in full](#appendix-d-resource-rules-in-full)

---

## 1. Overview

The map is an unbounded plane measured in metres. Nothing is generated up front. Every
layer is a pure function of `(foundation seed, Parameter Set, position)` and is computed on
demand, then cached. Two rules make that possible:

- **Per-cell randomness comes from a hash**, never from a shared random stream:
  `hash(seed, purpose, i, j)` gives the random numbers of grid square `(i, j)` for one
  purpose. The result does not depend on the order in which cells are visited.
- **Every value depends on a bounded neighbourhood.** A cell can be evaluated by looking
  at a fixed number of cells around it, so the map is the same whether you approach a
  place from the east or the west.

The layers, in dependency order:

```
seed ──► cell hierarchy (regions 128 km ▸ zones 32 km ▸ patches 8 km)
          │
          ├─► oceans (ocean seeds → links → expansion, on zone cells)
          ├─► mergers (zone cells merge into a neighbour, patch cells into a neighbour in the same zone)
          ├─► elevation type (Flat Land / Lowland / Hills / Mountains)
          │
seed ──► fields (initial temperature, precipitation, prosperity, supernatural)
          │
          ├─► rivers      per region: space colonization from the sea into the land
          ├─► lakes       per region: on rivers
          ├─► height map  per region (raster): fitted to the rivers and the coast
          ├─► biomes      per patch: fields + height
          ├─► resources   per patch: biome, water, elevation type, bedrock, prosperity
          ├─► Prospects   per region: scored patches → Prospects for cities and towns
          └─► travel      roads, river routes, sea lanes between Prospects
```

The first block (hierarchy, oceans, mergers) is "the map" in the narrow sense: it decides
where land and sea are and how the land is divided. Everything after it is a layer on top
that reads the map but never changes it. In particular the **height map is derived from
the rivers**, not the other way round: rivers are drawn first as branching trees, and the
height map is then built so that those rivers run downhill.

A **region** is the unit of work for most layers: rivers, lakes, Prospects and the height
map are computed one region at a time, and rivers never cross from one region's land into
another's. Region borders therefore become watersheds, and the height map puts a ridge on
them.

If you only need a finite map, you can drop the laziness: generate every cell inside your
rectangle plus a margin, in the order above. To match the unbounded version right up to
the edge, the ocean seeds and links need a margin of link range + 5 = 7 regions
(section 5.2); the other layers need one or two regions.

Throughout this document `seed` in a formula such as `hash4(seed, purpose, i, j)` is the
**foundation seed** (the number made from the seed text). The only exceptions are the three
Voronoi levels and the fields, which use the derived seeds given in their tables
(sections 4 and 9).

---

## 2. Basics

### 2.1 Units and coordinates

- World coordinates are metres, `x` to the east and `y` to the south (screen order). The
  world is unbounded in both directions, including negative coordinates.
- Grid squares are addressed by integer indices `(i, j)`; cell `(i, j)` of a grid with
  spacing `size` covers `[i·size, (i+1)·size) × [j·size, (j+1)·size)`.
- A cell key packs `(i, j)` into one number: `key = (i + 2²¹)·2²² + (j + 2²¹)`. Keys order
  cells by `i` first, then `j`; several steps break ties "by key".

### 2.2 Hashing and random numbers

All arithmetic is 32-bit integer arithmetic with wrap-around.

```
pcg3(a, b, c):                       # pcg3d (Jarzynski & Olano 2020), first lane
    x = a·1664525 + 1013904223
    y = b·1664525 + 1013904223
    z = c·1664525 + 1013904223
    x += y·z;  y += z·x;  z += x·y
    x ^= x >>> 16;  y ^= y >>> 16;  z ^= z >>> 16
    x += y·z;  y += z·x;  z += x·y
    return x as uint32

fmix32(h):                           # MurmurHash3 finalizer
    h ^= h >>> 16;  h *= 0x85ebca6b
    h ^= h >>> 13;  h *= 0xc2b2ae35
    h ^= h >>> 16
    return h as uint32

hash4(seed, purpose, x, y) = fmix32(pcg3(x, y, seed XOR (purpose·0x9e3779b9)))
toUnit(h) = h / 2³²                  # uniform in [0, 1)
```

- **Seed text → number**: a string of 1–9 digits is used as a number, anything else is
  hashed to a uint32 (xmur3). The default seed text is `cellgen`.
- **Per-cell values** use `toUnit(hash4(seed, purpose, i, j))`. Each purpose has its own
  constant so the streams are independent.
- **Sequences** (for example "pick 3 of this region's zone cells", or the river growth of one
  region) use a small sequential generator, sfc32, seeded with `hash4(seed, purpose, i, j)`
  of the cell: the state `a, b, c, d` is filled from SplitMix32 and 12 outputs are
  discarded (exact code in Appendix A). Any good 32-bit PRNG seeded per cell will do.

Two helpers are used throughout:

```
between(lo, hi, u):                  # uniform integer in [lo, hi], u in [0, 1)
    a = min(lo, hi);  b = max(lo, hi)
    return a + min(b − a, floor(u·(b − a + 1)))

pick(items, k, rng):                 # k distinct items: partial Fisher–Yates on a copy
    a = copy(items);  n = min(k, len(a))
    for t in 0 .. n−1:
        r = t + floor(rng()·(len(a) − t))
        swap(a[t], a[r])
    return a[0 .. n−1]
```

You do not need the same hash to get the same *kind* of map, only a hash of comparable
quality. You do need it to reproduce a given seed bit for bit.

### 2.3 Noise

- **Simplex noise** `simplex2(seed, x, y)` in about [−1, 1]: standard 2D simplex noise
  whose gradient at grid point `(i, j)` is one of 12 unit vectors at 30° steps, chosen
  by `pcg3(i, j, seed) mod 12` (no permutation table, so it never repeats).
- **Perlin noise** `perlin2(seed, x, y)` in [−1, 1]: classic gradient noise on the integer
  grid with the same 12 gradients and hash, Perlin's quintic fade
  `t³(t(6t − 15) + 10)`, and the result multiplied by √2.
- **fBm** (fractal sum) over either basis, with `octaves`, `lacunarity` and `gain`:

  ```
  sum = 0; norm = 0; amp = 1; (px, py) = (x, y)
  for o in 0 .. octaves−1:
      sum  += amp · basis(seed + o·0x3c6ef372, px, py)
      norm += amp
      amp  *= gain
      (px, py) = ((0.8·px − 0.6·py)·lacunarity + 17.3,
                  (0.6·px + 0.8·py)·lacunarity − 9.1)      # rotate, scale, offset
  return sum / norm
  ```

  `fbm` uses simplex noise, `perlinFbm` uses Perlin noise. The rotation between octaves
  hides the grid.
- **Ridged noise** `ridged(seed, x, y)` in [−1, 1] (Musgrave's ridged multifractal over
  simplex noise): per octave `r = (1 − |noise|)²·weight`, `weight = min(1, 2r)` for the
  next octave, same octave stepping as fBm, result `2·sum/norm − 1`.

### 2.4 Monotone spline

Several curves (floodplain width over height, cooling over height) are monotone cubic
splines through a few control points (Fritsch & Carlson 1980): sort the points by x, take
the secant slopes `d[i]`, set the tangent at an inner point to the mean of its two secants
(0 where they change sign), end tangents to the end secants, and wherever
`(m[i]/d[i])² + (m[i+1]/d[i])² > 9` scale both tangents by `3/√(that sum)`. On a flat
segment (`d[i] = 0`) both of its tangents are 0. Points with an x already used are
dropped. Evaluate with cubic Hermite interpolation; the curve is constant beyond its end
points.

### 2.5 Priority queue

Dijkstra searches and the pit fill use a binary min-heap of `(key, cell index)` pairs. Ties
on the key are broken by the smaller cell index, so results do not depend on insertion
order.

---

## 3. One level of cells

Stage: Sited.

Each of the three levels of the hierarchy is an infinite Voronoi diagram defined without a
mesh. A level has five parameters: `seed`, `size` (grid spacing in metres), `jitter`
(0–1), `warp` (border warp amplitude in cell sizes) and `warpFreq` (warp noise cycles per
cell).

### 3.1 Sites

Every grid square `(i, j)` holds exactly one site:

```
siteX(i, j) = (i + 0.5 + (toUnit(hash4(seed, 0x3a01, i, j)) − 0.5)·jitter)·size
siteY(i, j) = (j + 0.5 + (toUnit(hash4(seed, 0x3a02, i, j)) − 0.5)·jitter)·size
```

With `jitter = 0` the sites form a square grid; with `jitter = 1` (the default on all
levels) a site may be anywhere inside its grid square. The cell of the diagram is named
after the grid square of its site.

### 3.2 Which cell contains a point

```
query(wx, wy):
    (x, y) = (wx, wy)
    if warp > 0:
        f = warpFreq / size;  a = warp·size
        x += a·fbm(seed XOR 0x2f6b1e3d, wx·f,        wy·f;        2 octaves, lacunarity 2, gain 0.5)
        y += a·fbm(seed XOR 0x5bd1e995, wx·f + 31.7, wy·f − 47.3; same)
    return the grid square whose site is nearest to (x, y)
```

The warp moves the **query point**, not the sites, so cell borders become wavy while the
sites stay put. (A site can therefore lie just outside its own warped cell. Everything
that asks "which parent does this cell belong to" uses the site position.)

The nearest site is found by searching the grid squares around the cell containing
`(x, y)` in square rings of radius 0, 1, 2, 3. A site in ring `r` is at least
`r − 1 + (1 − jitter)/2` grid units away, so the search stops as soon as that bound
exceeds the best distance found. Equal distances are resolved in favour of the smaller
`j`, then the smaller `i`.

### 3.3 Neighbours

Two cells are neighbours when their sites are **Gabriel neighbours**: no third site lies
strictly inside the circle whose diameter is the segment between the two sites. Candidates
are the 24 grid squares within ±2 of the cell; for each candidate, test the sites of all
grid squares overlapping the circle's bounding box.

The neighbour list of a cell is **sorted by site distance, nearest first** (ties by key).
The Gabriel graph is a subgraph of the Delaunay triangulation, it is symmetric, and it
always contains a cell's nearest neighbour. Many later steps say "the nearest neighbour
that …": they walk this sorted list and take the first cell that qualifies. Neighbours
are computed on the unwarped sites.

---

## 4. The three-level hierarchy

Stage: Sited.

| Level | Default size | Jitter | Warp | Warp freq. | Level seed | Role |
|---|---|---|---|---|---|---|
| Regions | 128 km | 1 | 0 (never warped) | – | `seed XOR 0x1b873593` | unit of work; rivers stay inside; decides ocean seeds and zone mergers |
| Zones | 32 km | 1 | 0 | 0.6 | `seed XOR 0x7f4a7c15` | the land/ocean unit; carries the elevation type and the bedrock |
| Patches | 8 km | 1 | 0.26 | 0.33 | `seed XOR 0x6c8e9cf5` | detail: coastline shape, biomes, resources, Prospects |

The sizes are kept nested: the zone size is clamped to `[regionSize/16, regionSize/1.5]`
and the patch size to `[zoneSize/16, zoneSize/1.5]`.

Every Voronoi cell of the zone level is a **zone cell** and every cell of the patch level
is a **patch cell**. Zones and patches are built from them by the mergers of section 6.

**Ownership goes by site.**

- A zone cell is owned by the region that contains its site
  (`regionLevel.query(zone cell site)`).
- A patch cell is owned by the zone cell that contains its site
  (`zoneLevel.query(patch cell site)`, including the zone level's warp if it has one).

To list the zone cells a region `(i, j)` owns, scan the zone grid squares that cover the
region's grid square plus one region grid square on every side, and keep those whose
owning region is this one, **in grid order** (row by row: `j` outer, `i` inner). With
region size `R` and zone size `B` the scanned zone grid coordinates are, inclusive on both
ends,

```
zi from floor((i − 1)·R / B) to floor((i + 2)·R / B)        # same for zj with j
```

The patch cells of a zone cell `(zi, zj)` are listed the same way, with patch size `S` and
a margin of `m = ⌈warp of the zone level⌉ + 1` zone grid squares (1 at the defaults):

```
pi from floor((zi − m)·B / S) to floor((zi + 1 + m)·B / S)  # same for pj with zj
```

The grid order and the exact window matter because random picks are made from these
lists.

**Nesting.** With "nest levels" on (the default) a point is classified through its patch
only: point → patch → the zone of that patch → the region of that zone. Zone borders,
region borders and coastlines therefore all run along patch borders, and the 8 km warped
patch cells give the coast its shape. (With nesting off, each level is looked up at the
point itself and borders of different levels cross each other; the rest of this document
assumes nesting.)

---

## 5. Oceans

Stage: ocean seeds are Sited; linking and expansion make the region Outlined.

Oceans are decided on **zone cells** (before the mergers), in three steps.

### 5.1 Ocean seeds

Each region `(i, j)` chooses some of the zone cells it owns as ocean seeds:

```
rng    = sfc32(hash4(seed, 0x5e01, i, j))
nSeeds = between(oceanSeedsMin, oceanSeedsMax, rng())   # defaults 0 and 2
seeds  = pick(ownedZoneCells, nSeeds, rng)
```

### 5.2 Links

Each ocean seed `s` (grid coordinate `(si, sj)`) decides how many links it makes:

```
if   toUnit(hash4(seed, 0x5e05, si, sj)) < doubleLinkChance:  n = 2     # default 5 %
elif toUnit(hash4(seed, 0x5e03, si, sj)) < linkChance:        n = 1     # default 40 %
else:                                                          n = 0
```

It links to its `n` nearest **other ocean seeds**, searching the seeds of all regions
within the link range (2 grid squares) of its own region (a 5 × 5 block), ordered by
squared site distance, ties by key.

A link turns every zone cell on the straight segment between the two seed sites into
ocean: divide the segment into `n = max(1, ⌈length / (0.12 × zone size)⌉)` equal parts and
query the zone level at the interior points `t = k/n`, `k = 1 … n − 1`. Each zone cell hit,
and the two end seeds themselves, are flagged **LINKED**. The zone cells in between are
called *chain* cells. Because chains from several seeds can overlap, several seeds can end
up in one sea.

Each zone cell thus carries two flags, `SEED` and `LINKED`, OR-ed together from every
region that touches it. A lone seed that links to nothing and is not on anyone's chain
stays a sea of a single zone cell.

A zone cell can be flagged by links that start several regions away. When evaluating
lazily, process the seeds and links of every region within link range + 5 grid squares of
a zone cell's region before reading its flags.

### 5.3 Expansion

Expansion runs for a number of expansion passes (1). In each pass some ocean zone cells
convert a few of their land neighbours.

- **Pass 1** expands from every zone cell flagged LINKED (with the option "expand from
  all": from every zone cell with any flag, lone seeds too).
- **Pass p > 1** expands from the zone cells that became ocean in pass p − 1.

Each expanding cell `g` at grid coordinate `(gi, gj)` draws how many neighbours it
converts:

```
u = toUnit(hash4(seed, 0x5e10 + p, gi, gj))
n = 0, 1, 2 or 3 with relative weights expansionWeights[0..3]   # defaults 23, 33, 33, 10
    # normalise the weights (negative → 0), cdf[k] = w[0] + … + w[k];
    # n = the first k with u < cdf[k], at most 3
```

and converts its `n` **nearest neighbours that were still land after pass p − 1**. All
expanding cells of a pass act on the state after the previous pass, so the order does not
matter; a zone cell is ocean after pass `p` if it was ocean before or any neighbouring
expanding cell picked it.

With the defaults about 18 % of the map ends up as sea (measured over three seeds):
scattered seas of a single zone cell, and larger seas where chains and their expansion run
together. More ocean seeds, a higher link chance and more passes give more water.

---

## 6. Zone and patch mergers

Stage: zone mergers make the region Outlined; patch mergers are the first step of Detailed.

The mergers make cell sizes irregular: some zone cells and patch cells merge into a
neighbour and form a double (or larger) zone or patch.

### 6.1 Zone mergers

Each region `(i, j)` chooses zone cells to merge away:

```
rng      = sfc32(hash4(seed, 0x5e02, i, j))
nMergers = between(mergersMin, mergersMax, rng())        # defaults 2 and 4
chosen   = pick(ownedZoneCells, nMergers, rng)
```

A chosen zone cell merges into its **nearest neighbour on the same side of the coast**
(ocean with ocean, land with land; with the option "merge across coast", simply its
nearest neighbour). The neighbour may be owned by another region. If none qualifies, the
zone cell does not merge.

### 6.2 Root cells

A zone cell that merges points at its target, which may itself have merged into a third
zone cell. Follow the targets until a zone cell that has not merged: that is the **root
cell**, and all zone cells with the same root cell form one **zone**. If the chain loops
(A → B → A), the root cell is the one with the smallest key in the loop. (Give up after
64 steps.)

A zone takes everything from its root cell: ocean or land, its region (the region that
owns the root cell), its elevation type and its bedrock. So a zone cell that merges into a
zone of the neighbouring region moves to that region: region borders follow the zones.

From here on a **zone** is such a set of zone cells, addressed by the grid coordinate of
its root cell, and a **zone cell** is one Voronoi cell. A zone cell that has not merged is
a zone of one cell. **Patch** and **patch cell** are used the same way.

### 6.3 Patch mergers

Each zone cell `(i, j)` chooses patch cells to merge away:

```
rng      = sfc32(hash4(seed, 0x5e04, i, j))
nMergers = between(patchMergersMin, patchMergersMax, rng())   # defaults 2 and 4
chosen   = pick(ownedPatchCells, nMergers, rng)
```

A chosen patch cell merges into its **nearest neighbour patch cell in the same zone** (the
root cell of the neighbour's owning zone cell equals the root cell of its own). Root cells
are resolved as for zones.

---

## 7. Classifying a point

With the pieces above, any world point is classified like this:

```
cellAt(x, y):
    p    = patchLevel.query(x, y)             # patch cell (warped borders)
    pr   = root(p)                            # patch
    z    = zone cell owning pr (by pr's site)
    zr   = root(z)                            # zone
    return {
        patchCell: p,  patch: pr,
        zoneCell: z,  zone: zr,
        region: region owning zr (by zr's site),
        ocean:  zr is ocean after the last expansion pass
    }
```

Memoise the result per patch cell: every point of a patch cell gives the same answer. This
function is the whole "map" as far as the later layers are concerned.

Two derived notions are used later:

- **The zone cells drawn in region R**: the zone cells whose root cell belongs to R. They
  are found among the zone cells owned by R and by its 8 grid neighbours, and sorted by
  key. This differs from "the zone cells R owns" because of mergers across region borders.
- **Coastal patch cell** (as the rivers use it): a patch cell with at least one neighbour
  patch cell (Gabriel neighbours) on the other side of the coast. The biomes and resources
  use the same idea on patches: a land patch is coastal when any of its member patch cells
  has an ocean patch cell as neighbour.

---

## 8. Elevation types

Stage: Sited for the type of each zone cell; a zone has the type of its root cell.

Every land zone is one of four types, chosen by weight from a hash of the grid coordinate
`(i, j)` of its root cell:

```
h = hash4(seed, 0x0c01, i, j) mod 13
```

| Type | Weight | Share of land | Map colour (RGB) |
|---|---|---|---|
| Ocean | – | – | 38, 84, 132 |
| Flat Land | 5 | 38 % | 143, 174, 93 |
| Lowland | 6 | 46 % | 92, 134, 74 |
| Hills | 1 | 8 % | 154, 138, 98 |
| Mountains | 1 | 8 % | 124, 116, 112 |

Walk the table from the top, subtracting each weight from `h`, and take the first type
where `h` goes negative. The hash includes the foundation seed, like every other step, so
every world has its own pattern of elevation types.

The text below writes "flat zone" for a zone of the type Flat Land.

The type is a label at this point. It becomes height in section 12, where each type has
its own hillslope steepness, river gradient and surface roughness, and it feeds the
biomes, resources and travel costs.

---

## 9. Fields

Stage: none, the fields can be sampled at any time.

Four scalar fields are defined over the plane. Each is an
independent Perlin fBm stretched to a `[min, max]` range. They do not depend on the map.
The two climate fields are warped: the fBm is read at a position that a second noise has
moved, which draws its round features out into winding belts. They also have a
distribution (9.1), which says what share of the map lies below a value. The supernatural
field has a distribution too.

| Field | Seed | Feature size | Octaves | Lacunarity | Gain | Min | Max | Scale | Warp | Warp feature size |
|---|---|---|---|---|---|---|---|---|---|---|
| Initial Temperature (°C, at sea level) | `seed XOR 0x7e3a11c5` | 110 km | 2 | 2 | 0.5 | −16 | 30 | linear | 300 km | 800 km |
| Precipitation (mm/yr) | `seed XOR 0x2b9d40f3` | 80 km | 2 | 2.2 | 0.55 | 100 | 12000 | logarithmic | 200 km | 500 km |
| Prosperity (index) | `seed XOR 0x5c1f86a9` | 200 km | 3 | 2 | 0.5 | 1 | 100 | linear | 0 | – |
| Supernatural (index) | `seed XOR 0x3ad5e217` | 10 km | 3 | 2 | 0.5 | 0 | 100 | linear | 0 | – |

```
fieldValue(x, y):
    if warp > 0 and warpFeatureSize > 0:
        u = x / warpFeatureSize;  v = y / warpFeatureSize        # both from the unmoved position
        x = x + warp · perlinFbm(fieldSeed XOR 0x2f6b1e3d, u, v, 2, 2, 0.5)
        y = y + warp · perlinFbm(fieldSeed XOR 0x5bd1e995, u + 31.7, v − 47.3, 2, 2, 0.5)
    n = perlinFbm(fieldSeed, x / featureSize, y / featureSize)
    n = n · (Σ aₒ / √(Σ aₒ²))        # aₒ = 1, gain, gain², … : spread independent of the octave count
    if the field has a distribution:
        return distributed(n)        # section 9.1
    n = clamp(n / 0.8, −1, 1)        # 0.8 ≈ the 99.7th percentile of |n|
    t = (n + 1) / 2
    linear:       return min + t·(max − min)
    logarithmic:  return exp(ln(min) + t·(ln(max) − ln(min)))   # min and max at least 1; result clamped to [min, max]
```

The warp is domain warping, the same idea as the border warp of the cell levels (3.2).
`warp` is about the furthest a position is moved; a typical move is a third of it. The
belts appear when the warp is a few feature sizes of the field, and they get longer and
straighter as the warp feature size grows. Where the warp squeezes the plane the field
changes faster than its feature size says, so belts can be narrower than a zone. The two
climate fields have their own warp noises, so their belts do not line up. A warp of 0
gives the plain fBm.

Uses: initial temperature and precipitation feed the biomes; precipitation also thins the rivers
of dry flat zones (next section); prosperity feeds resources and Prospect scores; the
supernatural field feeds nothing yet.

### 9.1 Distribution

Without a distribution the values of a field follow the bell shape of the noise: half
the map lies in the middle quarter of the range, and only the strongest highs and lows
reach `min` and `max`. Prosperity is like that.

A **distribution** is a list of points `(share, value)`. Each point says what share of the
map lies below a value. `min` is the point at share 0 and `max` the point at share 1.

```
distributed(n):
    u = normalShare(n / 0.305)       # the share of the map with a lower noise value
    d = spline(u)                    # monotone spline (2.4) through (0, min), the points, (1, max)
    return clamp(d, min, max)        # logarithmic: the spline runs through ln(value); return exp(d), clamped

normalShare(z):                      # the normal distribution below z (Abramowitz & Stegun 7.1.26)
    x = |z| / √2;  k = 1 / (1 + 0.3275911·x)
    poly = ((((1.061405429·k − 1.453152027)·k + 1.421413741)·k − 0.284496736)·k + 0.254829592)·k
    tail = 0.5 · poly · exp(−x²)     # the deterministic exp of Appendix A
    return z < 0 ? tail : 1 − tail
```

The noise after the spread is close to a normal distribution with standard deviation
0.305, so `u` is the rank of the place among all places. The distribution only changes
the value given to a rank: a place that is warmer than another stays warmer, so the belts
keep their shape. A point whose share is not between 0 and 1 is ignored, and the values
must rise with the shares.

The two climate fields have a distribution because the bell shape starves the biomes at
the ends of the classification (13.2): with it the tropical belt held a ninth of the land
and the polar belt a hundredth. The points lie on the bounds of the classification, so a
share can be read as the share of a class.

| Initial Temperature | below −10 °C | below −3 °C | below 3 °C | below 18 °C |
|---|---|---|---|---|
| Share of the map | 5.5 % | 9 % | 21 % | 59 % |

| Precipitation | below 150 mm | below 350 mm | below 700 mm | below 1400 mm | below 2800 mm |
|---|---|---|---|---|---|
| Share of the map | 5 % | 12 % | 43 % | 66 % | 88 % |

The temperature points are the bounds of the thermal belts, so the shares are the belts
at sea level: 5.5 % polar, 3.5 % subpolar, 12 % boreal, 38 % temperate and 41 % tropical.
Height cooling (13.1) then moves land to the colder belts. The precipitation points are
close to the bounds of the moisture classes in the temperate and tropical belts.

The shares hold over a large area, some thousand kilometres across. One view of a few
regions can differ by several points, because a climate belt is as large as a region.

The supernatural field has a distribution to keep the supernatural rare. The bell shape
put 6 % of the map above 80 and gave a mean of 50. The points lie on the bounds of the
bands the field is drawn in (17), so the shares are the shares of the bands: 40 %, 30 %,
20 %, 9 % and 1 % from the lowest band to the highest, with a mean of 30. Measured, the
highest band holds 0.75 %: the tail of the noise is a little thinner than a normal one.

| Supernatural | below 20 | below 40 | below 60 | below 80 |
|---|---|---|---|---|
| Share of the map | 40 % | 70 % | 90 % | 99 % |

Its features are 10 km across, so the shares hold within a single region.

---

## 10. Rivers

Stage: Detailed.

Rivers are grown per region with the **space colonization
algorithm** (Runions et al. 2005, the open venation variant): attraction points are
scattered over the land, tree nodes start at the sea, and the trees grow step by step
towards the attraction points until all are reached. The result is a set of trees rooted
in the sea whose branches are the rivers. Flow direction is from the leaves to the root.

| Parameter | Default | Meaning |
|---|---|---|
| Attractors per patch | 10 | attraction points in every eligible patch |
| Step `D` | 1 km | length of one growth step (one river segment) |
| Influence `dᵢ` | 47.8 km | an attractor pulls its nearest node only within this radius |
| Kill `dₖ` | 5 km | an attractor is reached once a node is this close |
| Random motion | 0.4 | weight of a random unit vector added to each step |
| Wander | 0.23 | how much of a step's random vector is inherited from the parent |
| Perception cone | 120° | a node is only pulled by attractors within ±60° of its heading |
| Main-stem dominance | 0.24 | weak side branches are suppressed |
| Pull power | normal | every pull has equal weight |
| Inertia | 0 | weight of the current heading |
| Skip coast patches | on | no attractors in coastal patches |
| Flat sparse cutoff | 1470 mm/yr | dry flat zones get few attractors |

`dₖ` is raised to at least `1.05·D` and `dᵢ` to at least `1.5·D`.

### 10.1 Attractors

For region `(ri, rj)`, with `rng = sfc32(hash4(seed, 0x7e01, ri, rj))`:

```
for each zone cell drawn in the region (sorted by key), if it is land:
    sparse = its zone is a river-poor flat zone (see below)
    for each patch cell the zone cell owns (grid order):
        skip if the patch cell is ocean or not in this region
        skip if it is a coastal patch cell                # "skip coast patches"
        count = sparse ? (hash4(seed, 0x7e04, patch cell i, patch cell j) AND 1) : attractorsPerPatch
        repeat count times:
            try up to 24 times:
                q = patch cell site + (rng() − 0.5, rng() − 0.5) · 1.6 · patchSize
                accept q if patchLevel.query(q) is this patch cell
            if none accepted: use the patch cell site itself, provided the site is land of
                              this region; otherwise drop this attractor
```

**River-poor flat zones.** A flat zone whose site (the site of its root cell) gets less
precipitation than the cutoff (1470 mm/yr; about two thirds of the flat zones with the
default fields) is river-poor: each of its patch cells gets 0 or 1 attractor (a coin flip
from the patch cell's hash) instead of 10. These become the dry plains with few rivers;
the same flag later makes them drier for the biomes and bars lakes.

Leaving the coastal patch cells empty keeps river mouths apart and gives the rivers a
clear run from the coast into the interior.

### 10.2 River roots

A river root is the node a river tree starts from. It has nothing to do with the root
cell of a merger. Every ocean **zone cell** that touches the region's land gets one river
root (a sea of several zone cells gets one per zone cell that qualifies):

- every ocean zone cell that is a neighbour of a land zone cell drawn in the region, and
- every ocean zone cell drawn in the region that has a land neighbour.

The root sits at the ocean zone cell's **site**, out in the water, so each river first
grows across the sea to the coast (that stretch is hidden when drawing). If the site
happens to fall on land (coasts follow patch cells), use the site of the nearest patch
cell of the zone cell whose site is ocean; if there is none, the zone cell gets no root.
Roots are created in order of ocean zone cell key. A region without attractors gets no
roots and no rivers.

A **landlocked** region (no ocean zone cell touches its land) searches outwards
breadth-first over zone cell neighbours, up to 4 steps, and takes the first ocean zone
cells it finds. Such a region is exempt from the border rule below, since its rivers must
cross a neighbour.

### 10.3 Growth

State per node: position, parent, unit heading (parent → node; none for roots), the wander
vector of the step that created it, its first child, a count of children, a "dead" flag,
and the number of attractors it reached. State per attractor: alive or reached, and its
current nearest node with the squared distance.

**Adding a node** updates the attractors near it (use a grid with cell size `dᵢ` and scan
the 3 × 3 cells around the node): an attractor within `dₖ` is **reached** (removed, and
counted for this node); otherwise, if the new node is nearer than the attractor's current
nearest node **and the node can see it**, it becomes the attractor's nearest node. "Can
see" is the perception cone: the attractor must lie within ±cone/2 of the node's heading
(roots see all round).

Each iteration, with `rng = sfc32(hash4(seed, 0x7e02, ri, rj))` for the whole region:

1. **Pulls.** Every alive attractor whose nearest node is within `dᵢ` adds its unit vector
   (node → attractor) to that node's pull, and the node counts it. (Pull power: the
   vector's weight is 1, `(d/dᵢ)²` or `√(d/dᵢ)`.) Each pulled node also remembers its
   closest pulling attractor.
2. **No pulls at all → bridge.** See 10.4.
3. **Each pulled node, in index order**, grows one child:
   1. *Main-stem dominance* (if on and the node has already grown a child in a normal
      growth step; children made by a bridge do not count here or in step 8): see 10.5. A
      suppressed node does not grow.
   2. Direction `v` = the summed pull, normalised. (If the pulls cancel out, aim at the
      closest pulling attractor.)
   3. *Inertia*: `v = normalise(v + inertia · heading)`.
   4. *Random motion*: draw a random unit vector `r` (uniform direction). With wander,
      `r = normalise(wander · parentWander + (1 − wander) · r)` where `parentWander` is the
      random vector stored on this node. Then `v = normalise(v + jitter · r)`. The child
      stores `r` as its own wander vector, so bends persist over several steps instead of
      wiggling.
   5. Target = node + `D · v`.
   6. *Stall guard*: if the target is within `0.05·D` of the node's previous child, step
      straight towards the closest pulling attractor instead.
   7. *Border rule* (10.6). The step may be redirected or the node stopped.
   8. A node that has grown 8 children and wants another is stopped ("dead").
   9. Add the child node (which reaches attractors and updates nearest nodes as above).

Repeat until no attractor is alive, or 6000 iterations, or 120 000 nodes.

### 10.4 Bridges

When no attractor is within `dᵢ` of any node, growth would stop with attractors left over.
Instead: for every alive attractor find its closest live node (any node that is not
stopped and not banned for that attractor; the perception cone is ignored; ties go to the
lower index), sort these pairs by distance (ties by attractor index), and take the first
pair whose straight connection stays inside the region. Connect it with a straight run of
nodes one step apart, `max(1, ⌈(d − dᵢ/2)/D⌉)` steps long, stopping early if the attractor
is reached. Bridge nodes carry no wander vector. Normal growth then resumes. Pairs whose
straight line leaves the region are banned (10.6); an attractor with 12 banned nodes is
given up. A bridge attempt that finds no usable pair is retried only if it produced new
bans, and growth ends at the sixth such failed attempt.

### 10.5 Main-stem dominance

Without it, every node near a cluster of attractors sprouts short side twigs. With it, a
node that already has a child may only sprout a side branch if it is pulled strongly
enough compared with the stem it sits on:

- For each node, let `subtreePulls` be the number of attractors pulling this iteration on
  the node itself or on any of its descendants (towards the leaves, away from the root)
  up to 47 generations away. Compute it by walking from every pulled node through itself
  and its next 47 ancestors, adding the node's pull count to each.
- A pulled node `k` with a first child `c` grows only if
  `pulls(k) ≥ apical · subtreePulls(c)`.
- Otherwise `k` does not grow, and those of its pulling attractors that lie within
  `dₖ + 2D` of it are reached on the spot (they count as flow into `k` without a visible
  channel). The farther ones stay and turn to other nodes later.

### 10.6 Staying inside the region

A river node may lie over any ocean or on land of its own region, never on another
region's land (landlocked regions are exempt).

- A step is allowed if its end point and its midpoint are inside.
- If a step is not allowed: every alive attractor whose nearest node is this node and
  that the node cannot reach in a straight line inside the region (sampled every `D/2`)
  is **banned** from that node and re-assigned (below). If any attractor was banned the
  node simply tries again next iteration with the remaining pulls. If none was, step
  straight towards the closest pulling attractor; if even that leaves the region, the node
  is stopped and its attractors are re-assigned.
- **Re-assigning an attractor**: among the nodes in the 3 × 3 grid cells around it that
  are not stopped, not banned for it and can see it (cone), take them nearest first (ties
  by index). The first one within `dᵢ` with a clear straight line becomes its nearest
  node; each one within `dᵢ` that fails the straight-line test is banned on the way. If
  none is left the attractor has no nearest node until a new node comes close.

Attractors in pockets of land that other regions wall off from the sea stay unreached.

### 10.7 Flow

When growth has finished:

- `flow(node)` = the attractors reached at the node plus the flow of all its children
  (sum from the last node to the first; children always have larger indices than their
  parents). The flow at a river mouth is the number of attractors its whole tree reached.
  Flow stands in for drainage area and discharge everywhere below.
- `wet(node)` = the node lies over ocean (the hidden stretch between root and coast).
- `tree(node)` = the root of the node's tree; one tree is one river system.

At the defaults a region has around 1 100–1 400 attractors (a few hundred in a mostly dry
or mostly sea region, up to about 3 000) and about as many river nodes. Every attractor
was reached in the 75 regions measured.

---

## 11. Lakes

Stage: Detailed.

Lakes are placed per region after the rivers and always sit on
a river, so every lake has a river running in and out.

| Parameter | Default |
|---|---|
| Frequency | 0.06 |
| Minimum size (diameter of a circle of equal area) | 1.5 km |
| Maximum size | 5 km |

**Where a lake may be.** A patch may not hold a lake if it is ocean, lies in a *mountains*
zone, is a desert at sea level (its biome computed without height cooling is hot or cold
desert, section 13), or lies in a river-poor flat zone. A point is *lake-eligible* for
region R when it is land of R in a patch that may hold a lake.

**Candidates.** Collect the river nodes of the region that are on land, are not roots,
carry a flow of at least 4, and lie at a lake-eligible point; group them by patch cell, in
node index order.

**Placement.** With `rng = sfc32(hash4(seed, 0x1a01, ri, rj))`, visit the region's land
patch cells zone cell by zone cell (zone cells drawn in the region by key, skipping ocean
and mountains zones; the patch cells each owns in grid order, skipping ocean patch cells
and patch cells of another region) and draw three numbers for *every* such patch cell,
coastal and river-poor ones included, so the stream does not depend on which patch cells
have rivers:

```
roll = rng();  u = rng();  pickU = rng()
if the patch cell has no candidate nodes: continue
if roll ≥ frequency: continue
centre  = the candidate node at index floor(pickU · count)
nominal = minSize + (maxSize − minSize)·u²          # skewed towards small lakes
R       = nominal / 2
drop if the centre is closer than 1.15·(R + R') to the centre of an earlier lake (nominal radius R')
shape the lake (below); drop it if the shaping fails
```

**Shape.** The outline is a star-shaped polygon of 64 rays from the centre, at equal
angles (ray `k` points at angle `2πk/64`). It uses a second generator
`srng = sfc32(s)` with `s = hash4(seed, 0x1a02, pi·4099 + id, pj)`, where `(pi, pj)` is the
grid coordinate of the patch and `id` is the number of lakes already accepted in
the region. The draws are, in order: the axis (rejection sampling, Appendix A), `u₁`, `u₂`,
`nx`, `ny`, and the ring `u`. The noise seed is `s XOR 0x5bd1e995`.

1. Pick a random axis direction and an elongation `e = 1 + 1.2·u₁·u₂` (1 to 2.2, usually
   mild). The ellipse has semi-axes `√e` (along the axis) and `1/√e` (area unchanged).
2. Pick a random noise offset `nx = 1000·srng()`, `ny = 1000·srng()` and a ring radius
   `0.75·(0.6 + 0.8·u)`.
3. For each ray with unit direction `d`:
   `wanted = R · max(0.25, ellipse radius along d · (1 + 0.3 · fbm(nx + d.x·ring, ny + d.y·ring)))`
   with a 2-octave simplex fBm (lacunarity 2, gain 0.35). Reading the noise on a small
   circle gives one to three broad lobes and a closed outline.
4. **Clip.** Walk each ray outwards in steps of `max(60 m, R/12)` and stop one step before
   the first point that is not lake-eligible.
5. **Edges.** For each pair of neighbouring rays (`k` and `k + 1`, for `k = 0 … 63` with
   wrap-around), walk the edge between their end points at the same step length (interior
   points only); while it crosses ineligible ground, multiply both rays by 0.85 (up to 12
   times, then set both to 0).
6. **Taper.** Twice: a forward pass `r[k] = min(r[k], r[k−1] + 0.09R)` and a backward
   pass `r[k] = min(r[k], r[k+1] + 0.09R)`, updating in place, so that neighbouring rays
   differ by at most `0.09·R`.
7. **Round.** Three passes of `r[k] = min(r[k], (r[k−1] + 2r[k] + r[k+1])/4)`, each pass
   computed from the previous pass's values. Steps 5–7 only ever shorten rays, so the lake
   stays on eligible ground.
8. Area = `½·sin(2π/64)·Σ r[k]·r[k+1]`; size = `2√(area/π)`. Drop the lake if size <
   minimum or its shortest ray is under `0.2·R`. If size > maximum, scale all rays by
   `max/size`.

Lakes do not change the height map; they are drawn over it and they count as fresh water
for the later layers.

---

## 12. Height map

Stage: Elevated.

The height map is **fitted to the map and its rivers**. It is
built so that every river runs downhill to the sea, land rises away from rivers and
coasts, and region borders and the boundaries between river systems become ridges. It is
a raster (1 km cells by default) computed per region.

| Parameter | Default | Meaning |
|---|---|---|
| Resolution | 1000 m | raster cell size |
| Channel slope `S₀` | 0.01 | river gradient at a flow of 1 |
| Concavity `θ` | 7⁄16 | rivers flatten downstream as `flow^−θ` |
| Hillslope multiplier | 1 | scales every type's hillslope |
| Level flat zones | 1 | how strongly low-gradient zones are levelled |
| Mountain belts | 0.5 | strength of the region-scale relief field |
| Region ridge | 80 m | ridge height along region borders |
| Basin divides | 30 m | ridge height between two river systems |
| Detail noise | 1 | multiplier on the surface noise |
| Channel depth | 3 m | how deep rivers are cut below their bed profile |
| Sea depth | 300 m | depth 40 km or more from the coast |

Per elevation type:

| Type | Hillslope (m/m) | River gradient factor | Noise amplitude (m) | Ridged share |
|---|---|---|---|---|
| Flat Land | 0.006 | 0.7 | 8 | 0 |
| Lowland | 0.06 | 1 | 25 | 0.15 |
| Hills | 0.15 | 1.8 | 80 | 1 |
| Mountains | 0.277 | 3 | 180 | 1 |

Floodplain half-width as a function of the river bed height (a monotone spline through
four points): 400 m at 0 m, 200 m at 100 m, 90 m at 400 m, 30 m at 1000 m. With "scale
with river size" on (the default) this is the half-width at a flow of 100, and other flows
get `× √(flow/100)`. The half-width is never less than `0.75 ×` the raster cell size.

### 12.1 The raster

The raster of a region is aligned to the world: cell `(i, j)` has its centre at
`((i + 0.5)·res, (j + 0.5)·res)`. It covers the bounding box of the sites of all zone cells
drawn in or owned by the region, plus `1.6 × zone size × (1 + zone warp)` and a **halo of
24 km**. The halo
exists because neighbouring regions' rivers and coasts shape the land near the border;
with it, the rasters of two neighbouring regions agree where they overlap.

Classify every cell with `cellAt` at its centre: ocean or land, region, zone and
its type, patch.

(One raster can be computed for a *tile* of 2 × 2 regions with a shared halo; that is
an optimisation and gives identical heights.)

**Per-type parameters are blended across zone borders** so there are no cliffs between a
flat zone and a mountain zone: box-blur the hillslope, the noise amplitude and the ridged
share over land cells only, with a radius of 4 km. Then multiply the blurred hillslope by
the hillslope multiplier and the relief field, and the noise amplitude by the noise
multiplier and the relief field.

**Relief field** ("mountain belts"): a slow noise that makes groups of neighbouring zones
rugged or gentle together:

```
relief(x, y) = max(0.25, 1 + reliefStrength · 1.6 · fbm(seedR, x / 220 km, y / 220 km;
                                                     3 octaves, lacunarity 2, gain 0.5))
seedR = hash4(seed, 0x7e11, 0, 0)
```

### 12.2 Step 0: river bed profiles

Each river node gets a bed height. Roots and nodes over the sea are at 0. Going upstream,
each node rises above its parent by

```
rise(k) = Δx · min(channelSlope, hillslope)
channelSlope = S₀ · gradientFactor(elevation type at k) · relief(k) · flow(k)^−θ
hillslope    = the blended hillslope at k, read from the coarse grid described below
Δx           = distance from k to its parent
```

This is the steady-state stream-power profile: small headwater streams are steep, large
rivers are nearly flat, so profiles are concave. The `min` keeps a river from climbing
faster than its valley sides. Processing nodes in index order works because a parent
always has a smaller index than its children:

```
z(k) = max(z(parent), min(z(parent) + rise(k), cap(k)))        # cap(k) = ∞ at first
```

Two corrections add caps and are iterated until nothing changes.

**Levelling of low-gradient zones.** A zone whose gradient factor `c` is below 1 (flat
zones: 0.7) should read as a plain at roughly one level, but rivers enter it at several
points, each at whatever height it reached outside. An *entry* is a node whose zone
differs from its parent's zone. For each zone find the lowest `z(parent)` over its
entries. Every entry of a zone with `c < 1` is then capped at

```
keep = 1 − level · (1 − clamp(c, 0, 1))
cap(k) = lowest + keep · (z(parent) − lowest) + rise(k)     # only if this lowers cap(k) by more than 1 m
```

A river cannot run uphill, so a cap also limits everything downstream of it. Push caps
down the tree (from the last node to the first):

```
cap(parent) = min(cap(parent), max(0, cap(k) − min(rise(k), 0.0005 · Δx)))
```

Below a cap the river only has to keep descending at its natural rise or at a gorge slope
of 0.0005, whichever is gentler. The effect is that a river leaving a high plain cuts a
gorge through the steeper land downstream. Recompute `z` and repeat (lowering one entry
can change another zone's lowest entry): at most 32 times, stopping when no cap was
lowered or the largest change of `z` is under 10⁻⁶ m.

**No river above its land.** Step 1 below builds the land surface as the lowest surface
that rises from every river and from the coast at the hillslope. A river bed that stands
higher than that surface (a headwater that wandered close to a much lower river or to the
coast) would be perched above its surroundings. To prevent it, build the same surface on
a coarse 2 km grid around the region's rivers:

- **grid**: world-aligned 2 km cells covering the bounding box of the region's river nodes
  plus 4 cells on every side, each classified at its centre (sea or land). The slope of a
  land cell is the type hillslope averaged over the land cells within ±2 cells (±4 km),
  × the hillslope multiplier × the relief at the cell centre. A node's slope is the slope
  of its cell; this is also the `hillslope` in `rise(k)` above;
- **seeds**: sea cells next to land at height 0, and every river segment: for the 3 × 3
  cells around the segment's node, `bed at the projection of the cell centre +
  max(node slope, cell slope) · max(0, distance − floodplain half-width)`. The segments are
  the region's own at their current beds, and those of neighbouring regions that have
  nodes within 10 km of the region's river bounding box. The neighbours' beds are computed
  with rise and levelling only (no caps of this kind), using the unblurred type hillslope
  × multiplier × relief at the node, so the calculation does not chain from region to
  region;
- **spread** with Dijkstra over land, 8 neighbours, step cost
  `2 km · mean slope of the two cells · (1 or √2)`.

Then cap every node at `min over its 3 × 3 cells of (surface + max(node slope, cell
slope) · distance to the cell centre) + 5 m`, if the node stands above that and the cap
drops by more than 1 m. Push the caps downstream, recompute `z`, redo the levelling and
repeat (at most 6 rounds, until no cap changes).

### 12.3 Step 1: the envelope

The envelope is the land surface before ridges and noise: the cheapest "climb" from any
river bed or the coast, where climbing costs the local hillslope.

1. **River segments.** Every segment parent → node with the node on land becomes a
   source with bed heights at both ends, a floodplain half-width `w` (from the mean bed
   height, see above) and a label for its river system (region + tree). A river only
   shapes land of its own region (unless the region is landlocked).
2. **Nearest river.** For every cell, the distance to the nearest segment (within
   `w + max(2 cells, w)` of it). For a land cell only segments that may shape it count
   (rivers of its own region, or of a landlocked region); a neighbouring region's river
   just across the border is ignored. A cell is shaped by a segment only if that segment
   is its nearest one (within 1 m) or the cell is within 0.75 cell of the segment. This stops a
   big river's wide floodplain from flattening a smaller, higher tributary beside it.
3. **Stamp the floodplains.** Every land cell within `w` of a segment gets
   `E = bed height at the cell's projection onto the segment` (the lowest if several
   apply), remembers that height as its source height `src`, and takes the segment's label.
4. **Coast.** Every ocean cell with a land neighbour (8 neighbours) is a source with
   `E = 0`, `src = 0`, no label.
5. **Spread.** Dijkstra over land cells, 8 neighbours:
   `E(next) = E(cell) + res · ½(slope(cell) + slope(next)) · (1 or √2)`
   (from an ocean cell, use the slope of the land cell). `src` and the label travel with
   the path.

After this every land cell has a height `E`, the height `src` of the river or coast it
drains to, and the river system it belongs to. Land rises gently in flat zones and
steeply in mountains because the hillslope is the cost.

### 12.4 Step 2: ridges

Compute three distance fields (8-neighbour chamfer distances, starting at half a cell):

- `dBorder`: distance to the nearest land cell that touches (4 neighbours) land of another
  region; cut off at 10 km.
- `dDivide`: distance to the nearest land cell with a river-system label that touches
  (4 neighbours) a land cell with a *different river-system label*; cut off at 6 km.
  Cells that drain straight to the coast have no label and never form a divide, on either
  side.
- `dCoast`: for ocean cells, distance to the coast; cut off at 40 km.

### 12.5 Step 3: assemble and add detail

```
smooth(t) = t²(3 − 2t) clamped to [0, 1]

ocean cell:  h = −(2 + seaDepth · smooth(dCoast / 40 km))

land cell:
    m = smooth((E − src) / 40 m)          # 0 on the valley floor, 1 from 40 m above it
    h = E
    h += ridge  · (1 − dBorder/10 km)² · m      where dBorder < 10 km
    h += divide · (1 − dDivide/6 km)²  · m      where dDivide < 6 km
    n  = (1 − r) · fbm(seedN, x/6 km, y/6 km) + r · ridged(seedN, x/6 km, y/6 km)
                                          # r = blended ridged share; 4 octaves, lacunarity 2.1, gain 0.5
    h += amplitude · m · (0.5·n + 0.5)
seedN = hash4(seed, 0x7e12, 0, 0)
```

The mask `m` keeps valley floors smooth: ridges and noise fade in with height above the
river. The noise term lies in [0, amplitude], so it only ever adds height. A land cell the
envelope never reached uses the largest `E` on the raster. From here on heights are
stored as 32-bit floats, and the fill in step 5 compares and raises in that
precision (this matters only for bit-exact output).

### 12.6 Step 4: burn the rivers in

For every segment and every land cell within `w + buffer` of it (`buffer = max(2 cells, w)`),
under the same nearest-river and own-region rules as step 1:

```
if distance ≤ w:      target = bed − channelDepth
else if h > bed:      target = bed + (h − bed) · (distance − w) / buffer
h = min(h, target)
```

This cuts a trench along the channel and a linear ramp from the floodplain edge up to the
surrounding surface, so the river is always the lowest line of its valley.

### 12.7 Step 5: fill pits

Noise and rasterisation leave small pits. Fill them per region with ε-Priority-Flood
(Barnes et al. 2014), ε = 0.01 m:

- seeds: the ocean cells next to the region's land, and the region's land cells on the
  raster edge or touching another region's land (these keep their heights);
- repeatedly take the lowest cell from the heap; each unvisited land neighbour (8
  neighbours) of the same region is raised to at least `that height + ε` and pushed.

Afterwards every land cell has a strictly descending path to the sea or to the region
border. On the tile measured at the defaults (2 × 2 regions, about 140 000 cells) the
fill raised about 1 500 cells, by 9 m at most.

### 12.8 Results

Heights are stored per raster cell and read with bilinear interpolation. Also store the
**mean height of every patch** (over its land cells): the biomes use it. A patch
with no land cell centre in it has no mean height; the biomes then use the typical height
of its elevation type (section 13.1).

Measured at the defaults on one tile: median patch height about 85 m in flat zones, 160 m
in lowland, 260 m in hills and 410 m in mountains; the highest patches are near 1 000 m
and single peaks reach above 3 000 m.

---

## 13. Biomes

Stage: Detailed (estimated biome) and Elevated (final biome).

Every land patch gets one biome, from the fields at
the patch's site and the patch's height. The estimated biome uses the typical height of
the elevation type, and the final biome uses the height from the height map (13.1).

### 13.1 Inputs

```
T₀ = Initial Temperature field at the site (°C, sea level)
P₀ = precipitation field at the site (mm/yr)
h  = mean height of the patch's land (section 12.8), at least 0
T  = T₀ − cooling(h)
P  = P₀ · 0.2   if the patch lies in a river-poor flat zone (section 10.1), else P₀
```

`cooling(h)` is a monotone spline through five points, default
`(0 m, 0°), (200, 1°), (400, 5°), (900, 11°), (1250, 20°)`. Below the first point it is
constant; above the last it continues along the last segment's slope. The defaults are
much steeper than the real atmosphere (6.5 °C per km): with them the generated land,
which is mostly below 1 000 m, still gets cold highlands.

Layers that must not depend on the height map (lakes, resources as used by the
Prospects, Prospects, travel) use a **typical height per elevation type** instead of `h`:
70 m flat, 140 m lowland, 270 m hills, 470 m mountains. The biome with the typical height
is the **estimated biome**, and the biome with `h` is the **final biome**.

### 13.2 Classification

Aridity is precipitation relative to Köppen's dryness threshold, which grows with
temperature (warm air needs more rain):

```
P_th = max(40, 20·T + 140)        # mm/yr
m    = P / P_th                   # < 0.5 desert, 0.5–1 steppe, > 1 humid
```

| Thermal belt | T (°C) | m < 0.5 | 0.5 – 1 | 1 – 2 | 2 – wet bound | ≥ wet bound |
|---|---|---|---|---|---|---|
| Polar | below −10 | Ice & snow | Ice & snow | Ice & snow | Ice & snow | Ice & snow |
| Subpolar | −10 to −3 | Tundra | Tundra | Tundra | Tundra | Tundra |
| Boreal | −3 to 3 | Cold desert | Cold steppe | Cold steppe | Boreal forest | Boreal forest |
| Temperate | 3 to 18 | Cold desert | Grassland | Shrubland | Mixed forest (T < 10) or broadleaf forest (T ≥ 10) | Temperate rain forest |
| Tropical | 18 and up | Hot desert | Thorn scrub | Savanna | Tropical dry forest | Tropical rain forest |

The wet bound is `m = 6` in the temperate belt and `m = 4` in the tropical belt.

In the boreal belt the steppe runs up to `m = 2`. The dryness threshold is low in the
cold (140 mm/yr at 0 °C), so `m` between 1 and 2 is 140 to 280 mm/yr there: the rainfall
of a cold steppe, too little for a closed forest.

**Mangrove** overrides the table: a patch becomes mangrove when it is coastal (one of its
patch cells has an ocean patch as neighbour), lies in a flat or lowland zone, has
`T ≥ 20 °C` and `m ≥ 0.5`.

The same classification with `T₀` in place of `T` gives the patch's **sea-level biome**,
which the lakes use to rule out deserts without needing the height map.

**Shares.** Measured at the defaults on the final biomes of 16 seeds of 5 × 5 regions, as
shares of the land: hot desert 11 %, boreal forest 9.5 %, cold desert 8 %, woodland &
shrubland 8 %, temperate rain forest 7 %, ice & snow 7 %, savanna 6.5 %, tropical dry
forest 6 %, temperate broadleaf forest 6 %, tundra 5.5 %, temperate mixed forest 5.5 %,
thorn scrub 5 %, tropical rain forest 4.5 %, temperate grassland 4.5 %, cold steppe 3 %
and mangrove 2 %. The shares follow from the distributions of the two climate fields
(9.1), the cooling curve and the river-poor factor. Mangrove is the smallest because it
is bound to warm low coasts.

### 13.3 Colours

| Biome | RGB | Biome | RGB |
|---|---|---|---|
| Ocean | 38, 84, 132 | Temperate broadleaf forest | 96, 190, 56 |
| Ice & snow | 242, 246, 250 | Temperate rain forest | 0, 176, 170 |
| Tundra | 168, 160, 208 | Hot desert | 255, 234, 150 |
| Cold steppe | 176, 170, 130 | Thorn scrub | 214, 112, 48 |
| Boreal forest | 20, 88, 80 | Savanna | 240, 176, 40 |
| Cold desert | 216, 184, 184 | Tropical dry forest | 136, 136, 24 |
| Temperate grassland | 184, 222, 84 | Tropical rain forest | 0, 92, 36 |
| Woodland & shrubland | 150, 104, 70 | Mangrove | 168, 70, 150 |
| Temperate mixed forest | 76, 148, 84 | | |

---

## 14. Resources

Stage: Detailed (estimated) and Elevated (final).

Every patch gets an amount in [0, 1] for each of 24 raw
resources, plus a named variant ("grain: rye", "timber: teak"). A resource is listed for
a patch when its amount is at least 0.25. The layer is a pure function of the patch.

The 24 resources: grain, produce, orchard, livestock, fish, wild food & game (food);
timber, stone, clay, crystal, fibre (materials); fuel; salt; iron ore, copper ore, tin,
silver & lead, gold (metals); gems & pearls, spices, stimulants & sugar, furs & ivory,
dyes (luxuries); horses & pack animals.

### 14.1 Inputs per patch

- The patch's **biome record** (section 13), estimated or final: `T`, `T₀`,
  `P`, aridity, thermal belt, moisture class, biome, elevation type, coastal flag, sparse flag.
- **Bedrock** of the zone (Sited per zone cell; a zone has the bedrock of its root cell),
  hashed by weight from the elevation type: take
  `h = hash4(seed, 0x5e01, zone i, zone j) mod (sum of the type's column)` and walk the
  table from the top, subtracting each weight, to the first row where `h` goes negative:

  | Bedrock | Flat Land | Lowland | Hills | Mountains |
  |---|---|---|---|---|
  | Crystalline shield | 20 | 20 | 20 | 10 |
  | Fold belt | 0 | 5 | 30 | 40 |
  | Granite intrusion | 0 | 3 | 15 | 20 |
  | Volcanic arc | 0 | 2 | 10 | 30 |
  | Flood basalt | 10 | 10 | 15 | 5 |
  | Ophiolite | 0 | 2 | 10 | 15 |
  | Carbonate platform | 15 | 30 | 30 | 15 |
  | Clastic basin | 40 | 40 | 20 | 5 |
  | Unconsolidated cover | 60 | 40 | 0 | 0 |

- **Water site**, from the region's rivers and lakes. Only river nodes that are not roots,
  not over the sea, and on land of this region count, here and for the loads:
  - `river` = the largest flow of a river node in the patch ÷ attractors per patch (so it
    reads as "patches drained upstream"); 8 and up is *navigable*, 80 and up a *large river*;
  - `mouth` = the same for nodes whose parent is a root or lies over the sea;
  - `lake` = area of the largest lake placed in the patch;
  - `harbour` = the patch borders a *bay*: an ocean patch with at least 3 land neighbours
    and more land than ocean neighbours;
  - `goldLoad`, `tinLoad` = of all land river nodes at and upstream of the patch's largest
    river node, the share lying in hills or mountains zones on gold-bearing bedrock (fold
    belt, volcanic arc, shield) and on granite respectively. This is what the river
    carries down as placer deposits. (On equal flows the first node, by index, is used.)
- **Prosperity** at the site, rescaled to 0–1 between the field's min and max, and its
  boost `(2 · prosperity)²` (¼ in poor land, 1 in the middle, 4 at the top). The boost
  multiplies the chances of the valuable resources only: silver & lead, gold, gems,
  spices, stimulants, furs, dyes.

Whenever a rule sets an amount, the amount is first clamped to [0, 1] and replaces the
resource's current amount (and variant) only if it is strictly larger, so on a tie the
earlier rule wins.

Derived values:

```
growing-season temperature  tg = T + clamp(0.5 · (24 − T₀), 0, 12)
season length               season = clamp((T + 4) / 10, 0, 1)
irrigation (mm)             irr = (river > 1 ? 100 · (min(river, 13) − 1) · irrZone : 0) + (lake ? 200 : 0)
                            irrZone = 1 flat, 1 lowland, 0.5 hills, 0.25 mountains
tropical highland           T₀ ≥ 18 and the belt after cooling is temperate
floodplain                  river ≥ 1 and the zone is flat or lowland
```

### 14.2 Crops

Seven resources are crops: grain, produce, orchard, fibre, spices, stimulants, dyes. Each
has a table of crops (108 in all, listed in full in Appendix B). A crop has a temperature trapezoid
and a water trapezoid from the FAO ECOCROP database: `[absolute min, optimum min, optimum
max, absolute max]`. For example bread wheat: 5 / 15 / 23 / 27 °C and 300 / 750 / 900 /
1600 mm; paddy rice: 10 / 20 / 30 / 36 °C and 1000 / 1500 / 2000 / 4000 mm.

```
trap(v, a, b, c, d) = 0 outside (a, d), 1 on [b, c], linear in between

score(crop):
    s = trap(tg, crop.t)
    water = P;  if water < crop.p.optMin: water = min(crop.p.optMin, water + irr)   # rivers irrigate
    s *= trap(water, crop.p)
    s *= farmland(crop, elevation type)                 # default 1 / 1 / 0.6 / 0.3 for flat … mountains
    s *= limits on the yearly mean T, where the crop has them:
         0 at minT − 2 rising to 1 at minT;  1 at maxT falling to 0 at maxT + 2
    s *= tree crop ? treeGround(biome) : season · soil(biome)
    needs river and has neither river nor lake:   s *= 0.25
    needs coast and is not coastal:               s = 0
    needs highland and is not a tropical highland: s = 0
    s *= crop.k                                    # yield factor, default 1
    gated crop: s = 0 unless hash(seed, crop, zone) < gate · boost      # regional speciality
```

The resource's amount is the best score of its table and the variant is that crop's name.
Luxury crops (all spices, stimulants and dyes, and silk) are *gated*: each grows only in a
hashed 5–30 % of the zones, more where prosperity is high, which makes them regional
specialities worth trading.

### 14.3 Other potentials

These come from small tables over biome and elevation type plus the water site. The table
below is an overview; where its wording is looser than Appendix D, the appendix is exact:

| Resource | Rule |
|---|---|
| Livestock | `pasture(biome) · zone factor (1, 1, 0.9, 0.7)`, + 0.1 beside a river outside the polar belts |
| Fish | coast: 0.8 (1 in a harbour, 0.6 polar, 0.9 mangrove, 0.85–1 at a cold river mouth); river: 0.15 / 0.3 / 0.6 / 0.7 by size; lake: 0.45–0.75 by area. The best applies |
| Wild food | `game(biome)`, + 0.15 at a lake or on a floodplain |
| Timber | `wood(biome)`; gallery woods add 0.2 along rivers in grassland; deserts 0.3 only along a river; × 0.8 in mountains when above 0.6 |
| Clay | zone factor (0.7, 0.6, 0.25, 0.1), + 0.3 beside a lowland river, + 0.1 on unconsolidated cover |
| Fuel | 0.9 × timber; peat 0.6 on cool wet low ground; dung 0.35 × livestock; plus coal deposits |
| Furs & ivory | `fur(biome)`, only in a hashed share of zones (half of the cold ones, three tenths of the hot ones, × boost) |
| Honey (stimulants) | `honey(biome)`, 0–0.3 |
| Horses | `mounts(biome) · zone factor (1, 1, 0.7, 0.4)`; camels in deserts, mules in hills |

This table is a summary. The exact rules, including the variant names, are in Appendix D.1
and the biome factors (pasture, game, wood, fur, mounts, honey, soil, tree ground) in
Appendix C.

### 14.4 Deposits

Deposits are rare point resources. Each kind has a fixed purpose number; for a patch:

```
present  = toUnit(hash4(seed, 0x5e400 + kind, patch i, patch j)) < chance (× boost for valuables)
richness = rich · (0.35 + 0.65 · toUnit(hash4(seed, 0x5e800 + kind, patch i, patch j)))
```

A resource keeps the largest amount any of its kinds gives. The chances depend on bedrock,
elevation type, climate and water. In summary (all 51 kinds with their exact conditions,
chances and kind numbers are in Appendix D.2):

| Resource | Where (chance per patch) |
|---|---|
| Stone | by elevation type: 0.04 flat, 0.12 lowland, 0.75 hills, 0.95 mountains; × 2.5 on carbonate, × 2 on basalt, × 1.6 on clastic, × 0.5 on cover |
| Crystal | hills and mountains 0.03–0.05 (× 2 granite or fold belt, × 3 volcanic); glass sand on coasts and navigable rivers |
| Coal, lignite, oil seeps | clastic basins: 0.04, 0.04, 0.01 |
| Salt | solar salt on dry warm low coasts 0.25; playa salt in 8 % of arid river-poor flat zones 0.3; rock salt and brine in 5 % of clastic basins 0.12 / 0.08 |
| Iron | bog iron in wet forest lowlands 0.05–0.1; laterite in tropical lowlands 0.1; banded iron on shields 0.08; skarn on granite or carbonate highlands 0.06 |
| Copper | volcanic highlands 0.05–0.1; ophiolite highlands 0.08; basalt 0.02–0.06; clastic red beds 0.03 |
| Tin | granite highlands 0.06; stream tin below them: `0.35 · tinLoad` on rivers outside copper bedrock |
| Silver & lead | fold-belt highlands 0.05; carbonate lowland and hills 0.025–0.06; volcanic mountains 0.03 |
| Gold | lodes in fold-belt highlands, shield hills, volcanic mountains 0.03; placer gold `0.2 · goldLoad` on rivers below; beach gold at river mouths |
| Gems | by bedrock in highlands (0.004 for diamond and emerald up to 0.04 for jade, agate, topaz); amber on cool clastic coasts; pearls on warm coasts 0.04 |

Copper and tin come from different bedrock typees, so no zone has both: bronze needs
trade.

**Ocean patches** have fish (0.9 next to land, 0.5 in open water) and, next to land,
seaweed in cool water, seals in cold water, pearls or coral in warm water (Appendix D.3).

### 14.5 Summaries

Two summary numbers per patch feed the Prospect scores:

```
food   = clamp(0.75 · max(grain, 0.8 · produce) + 0.2 · livestock + 0.1 · orchard + 0.05 · wildFood, 0, 1)
wealth = Σ tradeValue(r) · amount(r)   over resources with amount ≥ 0.25
tradeValue: gold 1, tin 0.9, silver 0.9, gems 0.8, salt 0.7, spices 0.7, copper 0.6, iron 0.5,
            crystal 0.4, stimulants 0.4, furs 0.35, dyes 0.3, horses 0.2, orchard 0.1, fibre 0.1, others 0
```

---

## 15. Prospects for cities and towns

Stage: Planned.

Per region, every land patch is scored as a place
to settle, and the best patches become the region's Prospects.

| Parameter | Default |
|---|---|
| Prospects per region | 20 |
| Of these, Prospects for cities | 4 |
| Spacing | 20 km |
| Weights: resources / geography / trade / climate / prosperity | 0.3 / 0.15 / 0.25 / 0.15 / 0.15 |
| Score sum of a region at which 1 Prospect is on show / all are | 700 / 1,500 |
| Score from which a Prospect is Rich / below which it is Poor | 75 / 62 |

All inputs use the estimated biomes and resources, so Prospects never depend on the
height map.

### 15.1 Facts per patch

- **Neighbours** (via the member patch cells' Gabriel neighbours): own-region land
  patches, ocean patches, other regions' land patches. From these: `coast` (any ocean
  neighbour), `oceanFrac` (share of ocean neighbours), `harbour` (borders a bay, section
  14.1), `border` (touches another region's land).
- **Depth**: patch steps to the nearest border patch (breadth-first over own land; 99 for
  patches with no land connection to a border patch, such as a region without a land
  border).
- **Lake**: area of the largest lake in the patch.
- **River** facts from the region's river nodes on the patch: `river` and `mouth` as in
  14.1; `confluence` = the second-largest child flow at any node in the patch (the smaller
  branch of its biggest junction), in patches drained; `head` = the patch holds the
  upstream end of navigation (a navigable node none of whose children is navigable). Only
  children on the region's own land count for both.
- **Spot**: where the Prospect is placed. Choose one river node of the patch among those
  not inside a lake: a mouth node beats any other node; within the same kind the larger
  flow wins, then the lower index. If that node carries a flow of at least 3 (attractors,
  not patches drained), the spot is on it. Otherwise the spot is the patch's site.
  If the site lies outside its own patch (warp) or in a lake, try instead the sites of the
  patch's member patch cells, then, ring by ring (`ring = 1 … 4`), points around each member site
  at `ring · patchSize / 4` in the 8 directions E, S, W, N, SE, SW, NW, NE (diagonals as
  `(±0.7, ±0.7)`); the first point inside the patch and outside any lake wins.

### 15.2 Central places

How much of the region's food lies within reach of each patch. Build a graph over the
region's land patches:

- land edge between neighbouring patches: `km · ½(rough_i + rough_j) · ½(veg_i + veg_j)`
  with `rough` = 1, 1, 1.3, 2 for flat … mountains and `veg` = 1 for open land, 1.3 for
  forest, 2 for rain forest and mangrove (and similar);
- river edge between consecutive patches along a navigable river: `km · 0.4`.

`km` is the straight distance between the two patches' sites. All edges work in
both directions; river edges are added on top of the land edges. Neighbouring patches
here are land patches of this region only.

For each patch `s`, run Dijkstra over this graph and sum

```
acc(s) = Σ_u  food(u) · area(u) / (1 + d(s, u) / 35 km)²
centrality(s) = clamp(acc(s) / (0.55 · ideal), 0, 1)
```

where `area(u)` is the number of patch cells in the patch × the patch size squared
(km²), and `ideal` is the same sum for the centre of a disc of full farmland with the area of
one region: with `R = regionSize / √π / 35 km`,
`ideal = 2π · 35² · (ln(1 + R) + 1/(1 + R) − 1)` km².

### 15.3 The five factors

```
resources = 0.45·food + 0.2·water + 0.15·materials + 0.1·fish + 0.1·goods
    food      = (2·food(here) + Σ food(land neighbours)) / (2 + land neighbours + ocean neighbours)
    water     = max(clamp(river/2), ramp(aridity, 0.5, 1.5), coast ? 0.5 : 0, lake ? 1 : 0)
    materials = ½·max(timber, stone, clay here; 0.7 × best neighbour's timber or stone)
              + ½·max(fuel here, 0.7 × best neighbour's fuel, 1 − fuelNeed(belt))
                fuelNeed = 1, 1, 0.8, 0.5, 0.1 for polar … tropical
    goods     = clamp((wealth(here) + 0.5 · best neighbour's wealth) / 1.2)

geography = buildable · (0.5 + 0.3·waterside + 0.2·defence) · (0.75 + 0.25·clamp(depth/3))
    buildable = 1, 0.9, 0.55, 0.25 for flat … mountains; × 0.5 in mangrove
    waterside = max(river: 1 navigable, 0.7 from 2, 0.4 any; harbour 1, coast 0.7; lake 0.8)
    defence   = max(0, 0.1, 0.6, 0.7 by elevation type; 0.5 if oceanFrac ≥ 0.5; 0.4 if confluence ≥ 4)

trade = clamp(max(gateway, centrality) + 0.25·min(gateway, centrality))
    items   = [ clamp(mouth/80), harbour ? 0.6 : coast ? 0.24 : 0, 0.5·ramp(river, 8, 80),
                head ? 0.6 : 0, 0.4·clamp(confluence/8), lake ? 0.15 : 0 ]
    gateway = clamp((largest item + 0.25 · sum of the others) / 1.5)

climate = comfort · (0.4 + 0.6·ramp(aridity, 0.2, 1)) · (aridity > 6 ? 0.85 : 1)
    comfort = trap(T, −8, 4, 24, 34)

prosperity = the prosperity field at the spot, rescaled to 0–1
```

In these formulas "land neighbours" are neighbouring land patches of the same region
(land of other regions is counted nowhere), "ocean neighbours" are neighbouring
ocean patches, `fish` is the patch's own fish amount (section 14), and `river`, `mouth`
and `confluence` are in patches drained.

### 15.4 Score

```
need  = clamp(2 · min(water, comfort))          # no fresh water or unlivable cold/heat → 0
score = 100 · need · weighted mean of the five factors
        # negative weights count as 0; if all weights are 0 the plain mean is used
```

### 15.5 Selection

Eligible candidates have `score > 0` and `depth ≥ 1` (they do not touch another region's
land, which keeps Prospects apart across borders without looking at the neighbour). Sort
by score, best first; ties by `hash4(seed, 0x10c1, patch i, patch j)`, then by patch key.
Distances are measured between spots.

1. **Prospects for cities**: walk the list and take up to 4 candidates (never more than
   the Prospects per region) with `depth ≥ 2` that are at least `2.5 × spacing` from every
   city Prospect already taken. They are chosen once and kept.
2. **Prospects for towns**: walk the list again and take candidates at least `spacing`
   from every Prospect taken so far, until the region has 20.
3. If the region ends up with fewer than 20, multiply the spacing by 0.85 (not below one
   patch size), discard the town Prospects and redo step 2. Repeat until there are 20 or the
   spacing has reached one patch size.

Regions with little habitable land end up with fewer Prospects.

### 15.6 On show and class

Not every Prospect is shown. The sum of the scores of a region's Prospects decides how
many of them are **on show**: a map draws only these, and the travel network (section 16)
connects only these.

```
sum    = Σ score   over the region's Prospects
t      = clamp((sum − 700) / (1500 − 700), 0, 1)
onShow = min(number of Prospects, floor(1 + (20 − 1) · t + ½))
```

A region with a sum of 700 or less has 1 Prospect on show, a region with 1,500 or more has
all 20 (the Prospects per region), and the number grows evenly between. If the two sums
are set equal, the number steps from 1 to all at that sum. A region without Prospects
shows none. The Prospects on show are the best by score, in the order of 15.5 (ties
included), whether they are for a city or a town.

With the defaults about 1 region in 20 has a single Prospect on show, about 1 in 10 has
all 20, and the median is 13 (measured on 600 regions of six seeds).

Every Prospect has a **class** by its score: **Rich** from 75, **Poor** below 62 and
**Average** between. About a quarter of the Prospects on show are Rich and a fifth are
Poor.

---

## 16. Travel network

Stage: Settled.

Roads, river routes and sea lanes connect the Prospects on
show (15.6). The network of a region is computed from the Prospects and cost grids of the 3 × 3
regions around it.

### 16.1 Cost grid (per region, 2 km cells)

For every cell of the region (classified at its centre):

- **Sea cells**: cost 0.3 per km (0.6 right at the shore, so lanes keep off capes).
- **Land cells**: road cost per km relative to open flat land =
  zone factor (1, 1.15, 1.8, 4 for flat … mountains) × biome factor (1 for grassland; 1.3
  forests; 2–2.2 rain forests; 3 mangrove; 3.5–4 deserts; 5 ice) × 8 if the cell centre is
  in a lake.
- **Estimated height** (no height raster needed). The hillslope here is the **unblurred**
  one: the elevation type's hillslope × the hillslope multiplier × the relief at the point.
  River beds get the simple rise of 12.2 with that hillslope (no levelling, no caps) and
  seed the cells containing river nodes (the lowest bed per cell); land cells with an
  ocean cell among their 8 neighbours are seeded with `hillslope · 1 km`; then spread with
  Dijkstra over the region's own land cells, 8 neighbours, step cost
  `length · mean hillslope of the two cells`. Land that nothing reaches gets 150 m.
- **Navigable river links**: for every river node with flow ≥ 8 × attractors per patch,
  walk its segment to the parent in quarter-cell steps and record a link between each cell
  and the next cell the segment enters, remembering the river node. Links are used in
  both directions. A link between two sea cells is dropped (boats need no river there).

### 16.2 Shortest-path forests

Put the nine regions' cells on one raster. Land cost is multiplied by
`1 + height / 1500 m`.

**Seeds.** Searches are square windows of cells around the Prospect's own cell, and the
nearest matching cell centre wins.

- Road forest: the nearest land cell within ±2 cells that is not already another
  Prospect's seed, at cost 0. A Prospect with no such cell has no roads.
- Water forest, river landing: the nearest cell holding a river link within ±2 cells, at
  cost `6 + distance to the cell centre in km`.
- Water forest, port (coastal Prospects only): the nearest sea cell within
  `±⌈1.5 · patch size / 2 km⌉` cells (6 at the defaults), at cost `12 + distance in km`.
- A water seed is set only where it is cheaper than a seed already on that cell.

**Road forest.** Multi-source Dijkstra over land cells with 16 moves (the 8 neighbours and
the 8 knight moves, which need the two cells they pass between to be land):

```
step cost = length (km) · c · (1 + min(12, (slope / 0.05)²))
          + 1.5 if exactly one of the two cells holds a navigable river   # half a bridge or ferry
slope = climb / length

neighbour move:  c = mean cost of the two cells;  climb = |h(to) − h(from)|
knight move:     c = mean cost of four cells (both ends and the two passed between)
                 climb = |hm − h(from)| + |h(to) − hm|,  hm = mean height of the two passed cells
```

**Water forest.** Multi-source Dijkstra with 8 moves: along a river link (either
direction) `length · 0.4`; between two sea cells `length · mean sea cost`. A diagonal sea
move is not allowed when both cells it squeezes between are not sea.

Each cell ends up with its cheapest Prospect and the path to it: a Voronoi diagram of the
Prospects under travel cost, one for roads and one for water.

### 16.3 Links

- **Candidates.** Wherever two cells of different Prospects are one move apart (for
  roads any of the 16 moves, for water the 8 neighbours), the two Prospects can be linked
  through that crossing at cost `dist(cell a) + step + dist(cell b)`. Keep the cheapest
  crossing per pair of Prospects, separately for road and water.
- **Pruning.** Drop a link if the same two places are connected by a path of *cheaper*
  links costing at most 1.6 × the link. For a road, count other roads only at 1.6 ×; a
  route that uses water replaces a road only if it costs at most 0.35 × the road.
  "Cheaper" is a strict order: lower cost, then road before water, then the lower Prospect
  indices. The test is made once per link against **all candidate links**, not against
  the links that survive, so it is a single pass with no iteration. Because only cheaper
  links can replace a link, the dearest link of a cycle goes and the network stays
  connected.
- **Ownership.** A link between Prospects of two regions `(ia, ja)` and `(ib, jb)` is
  decided by one region: that region itself if they are the same; the smaller key if they
  are neighbours (8 neighbours); the region `(⌊(ia + ib)/2⌋, ⌊(ja + jb)/2⌋)` if they are
  two apart; no link if they are further apart. This keeps the result independent of the
  order regions are computed in.

### 16.4 Routes

- **Road**: the cells along the path from one Prospect through the crossing to the other,
  with the end points moved to the Prospect spots, smoothed with two passes of a ¼ ½ ¼
  filter. Links out of one place share their first stretch, like real roads.
- **Water**: one water link can mix river and sea stretches. Split its cell path wherever
  the steps change between sea-to-sea and anything else.
  - **River stretch**: the river's own nodes between the two ends (the cell centres if
    the two ends are not on the same river tree).
  - **Sea stretch**: the cell path, pulled straight wherever the line of sight is open
    water (sampled every 1 km).

  The link is called a river route or a sea lane after its longer part.

---

## 17. Drawing the map

The map is drawn per pixel: classify
the pixel's world position with `cellAt` and colour it.

- **Base colour** ("Map" mode): the elevation type colour (section 8), with the lightness
  varied by ±8 % per patch from a hash so the patches read. Ocean: its colour varied ±7 %
  per zone and ±3 % per patch. "Biomes" mode: the exact biome colour (13.3), not
  varied, so a patch can be matched against a legend. "None" mode: white (255, 255, 255),
  land and sea alike, for looking at the layers alone.
- **Fields** (optional on any mode, one layer per field of section 9): on land pixels the
  field is sampled at the pixel itself, not at the site of its patch, and the colour of
  the band the value falls in is multiplied onto the base colour, channel by channel
  (`base · band / 255`). On the None mode the result is exactly the band colour; several
  field layers at once mix like inks. The sea is not tinted. A band holds the values from
  its lower bound up to, not including, its upper bound; the bounds are fixed values, so
  a band means the same under every Parameter Set.

  | Field | Bounds | Band colours, lowest band first |
  |---|---|---|
  | Initial Temperature (°C) | −5, 0, 5, 10, 15, 20, 25 | (69, 117, 180), (116, 173, 209), (171, 217, 233), (224, 243, 248), (254, 224, 144), (253, 174, 97), (244, 109, 67), (215, 48, 39) |
  | Precipitation (mm/yr) | 250, 500, 1000, 2000, 4000, 8000 | (140, 81, 10), (191, 129, 45), (223, 194, 125), (199, 220, 120), (127, 198, 78), (49, 163, 60), (0, 109, 44) |
  | Prosperity (index) | 20, 40, 60, 80 | (255, 255, 212), (254, 217, 142), (254, 153, 41), (217, 95, 14), (153, 52, 4) |
  | Supernatural (index) | 20, 40, 60, 80 | (254, 235, 226), (251, 180, 185), (247, 104, 161), (197, 27, 138), (122, 1, 119) |
- **Borders**, found by comparing each pixel with its left and upper neighbour: coastline
  in near-black (14, 20, 30); region borders as a dotted red line (235, 28, 36): a 3 px dot on every point of a 6 px
  lattice, anchored to world pixels, that the border passes within 3 px of;
  zone borders darken the pixel to 50 %, patch borders to 80 %. Zone and patch borders
  are dropped when their cells get small on screen.
- **Height mode**: a hypsometric tint, linear between stops at 0 m (72, 128, 76), 60 m
  (104, 152, 84), 180 m (150, 176, 98), 400 m (206, 194, 122), 700 m (182, 142, 94),
  1100 m (150, 120, 98), 1600 m (168, 160, 156), 2400 m (246, 246, 246); the sea from
  (84, 150, 200) at the shore to (22, 46, 100) at 1200 m depth.
- **Hillshade** (optional on any mode): from the height differences to the left and upper
  pixel, exaggerated 4 ×, lit from the north-west at 45°; the pixel is scaled by
  `1 + 1.25 · (shade − √½)`, clamped to [0.35, 1.45].
- **Rivers**: one line per segment (node to parent), skipping the stretch over the sea.
  Width in pixels `= scale · zoom · (0.45 + 3 · (flow / 100)^b)` with `b = 0.4`,
  `scale = 0.8`, and `zoom` = (2 km on screen in px) / 7 clamped to [0.45, 2.2]. Real
  rivers widen with about the square root of discharge (`b = 0.5`).
- **Lakes**: their polygons, filled, drawn together with the rivers.
- **Prospects**: only the ones on show (15.6). A diamond for a city Prospect and a dot for
  a town Prospect, with a dark edge, coloured by class: Rich in gold (244, 196, 48),
  Average in silver (228, 231, 236), Poor in bronze (170, 104, 50).
- **Travel**: roads, river routes and sea lanes as lines; stretches shared by several
  links are drawn once.

---

## 18. Order of work and pitfalls

### 18.1 Order of work

1. Hash, PRNG, simplex and Perlin noise, fBm (section 2). Test: the same inputs give the
   same outputs on every run.
2. One level: sites, `query`, Gabriel neighbours (section 3). Draw it; try jitter 0
   (squares) and warp on and off.
3. Three levels with ownership and nesting (section 4). Dpatch cells coloured by their
   zone and check that zone borders follow patch borders.
4. Oceans (section 5). Draw ocean seeds, links and expansion separately; expect roughly a fifth of
   the map as sea at the defaults.
5. Mergers (section 6) and `cellAt` (section 7). Everything later only needs `cellAt`, the
   neighbour lists and the per-region lists of zone cells and patch cells.
6. Elevation types and the fields (sections 8, 9).
7. Rivers (section 10). Start with plain space colonization (pulls, step, kill, flow);
   add bridges and the region border rule next, then cone, wander and main-stem dominance.
8. Height map (section 12). Start with the simple bed rise and steps 1, 3, 4, 5. Add the
   ridges, then levelling and the bed caps.
9. Biomes (section 13), lakes (section 11).
10. Resources, Prospects, travel (sections 14–16), in that order.

### 18.2 What gives the map its look

- **Jittered-grid Voronoi on three scales**, with the coast following the smallest,
  warped cells. This gives angular but irregular coastlines without any noise threshold.
- **Oceans from linked ocean seeds**: straight chains of zone cells make elongated seas and straits
  rather than round blobs.
- **Rivers before height.** Space colonization gives evenly spread, tree-like drainage
  that fills every region. A large influence radius (48 km by default) gives fewer,
  straighter rivers and a small one many independent branches; the cone stops branches
  hooking back and main-stem dominance removes short stubs.
- **The height map as an envelope over the rivers.** Valleys are exactly where the rivers are,
  and the per-type hillslope decides whether the land between them is a plain or a range.
- **Regions as watersheds.** Rivers never cross region land borders, and the border gets
  a ridge.

### 18.3 Pitfalls

- **Order independence.** Never draw per-cell random numbers from a shared stream. Where
  a sequence is needed, seed it from the cell and consume it in a fixed order (grid
  order for cells, key order for zone cells, index order for nodes). Draw the random numbers
  even for items you then skip (see the lakes), or the stream shifts.
- **Lists must have a defined order.** "Pick 3 of the region's zone cells" depends on the order
  of the list. Sort or enumerate in grid order.
- **Ties.** Nearest-site ties, heap ties and equal scores need a fixed rule (smaller index
  or key, or a hash).
- **Ownership by site, classification by point.** A warped cell does not contain exactly
  the points nearest its site. Use the site for "which parent owns this cell" and
  `query` for "which cell is this point in". With nesting on, never ask for the zone or
  region of a point directly; go through the patch.
- **Mergers cross borders.** A zone cell that merges into a neighbouring region's zone
  belongs to that region. Always work with "zone cells drawn in the region", not "zone
  cells owned by it".
- **Rivers over the sea.** River roots sit in the water; treat nodes over ocean as height 0,
  leave them out of the height map and hide them when drawing.
- **Halo.** Anything rasterised per region (height map, travel costs) needs sources from
  beyond the region or the seams show. 24 km is enough for the height map at the defaults.
- **Bit-exact reproduction** across machines additionally needs deterministic `exp`,
  `log`, `sin` and `cos` (given in Appendix A) and no
  `pow` in generation code. If you only need maps of the same kind, ignore this.

---

## Appendix A. Exact generators

Sections 2.2 and 2.3 are enough for maps of the same kind. To reproduce a given seed
exactly you also need the following, bit for bit. All integer arithmetic is 32-bit with
wrap-around; `>>>` is an unsigned shift; `imul` is 32-bit multiplication.

```
hashString(str):                        # seed text → uint32 (xmur3)
    h = 1779033703 XOR len(str)
    for each UTF-16 code unit c of str:
        h = imul(h XOR c, 3432918353)
        h = (h << 13) | (h >>> 19)
    h = imul(h XOR (h >>> 16), 2246822507)
    h = imul(h XOR (h >>> 13), 3266489909)
    return (h XOR (h >>> 16)) as uint32

splitmix32(seed):                       # returns a generator of uint32
    s = seed
    next():
        s = s + 0x9e3779b9
        z = s
        z = imul(z XOR (z >>> 16), 0x85ebca6b)
        z = imul(z XOR (z >>> 13), 0xc2b2ae35)
        return (z XOR (z >>> 16)) as uint32

sfc32(seed):                            # returns a generator of floats in [0, 1)
    sm = splitmix32(seed);  a = sm(); b = sm(); c = sm(); d = sm()
    next():
        t = a + b + d
        d = d + 1
        a = b XOR (b >>> 9)
        b = c + (c << 3)
        c = (c << 21) | (c >>> 11)
        c = c + t
        return (t as uint32) / 2³²
    call next() 12 times and discard the results
    return next
```

Simplex noise, with `F = (√3 − 1)/2`, `G = (3 − √3)/6` and the 12 gradients
`gradient[n] = (cos(n·30°), sin(n·30°))`, n = 0 … 11:

```
simplex2(seed, x, y):
    s = (x + y)·F;  i = floor(x + s);  j = floor(y + s)
    t = (i + j)·G;  x0 = x − (i − t);  y0 = y − (j − t)
    (i1, j1) = x0 > y0 ? (1, 0) : (0, 1)
    x1 = x0 − i1 + G;      y1 = y0 − j1 + G
    x2 = x0 − 1 + 2G;      y2 = y0 − 1 + 2G
    n = 0
    for (cx, cy, dx, dy) in [(i, j, x0, y0), (i + i1, j + j1, x1, y1), (i + 1, j + 1, x2, y2)]:
        w = 0.5 − dx² − dy²
        if w > 0:
            g = gradient[pcg3(cx, cy, seed) mod 12]
            n += w⁴ · (g.x·dx + g.y·dy)
    return 99.204 · n

perlin2(seed, x, y):
    i = floor(x);  j = floor(y);  fx = x − i;  fy = y − j
    u = fade(fx);  v = fade(fy)                  # fade(t) = t³(t(6t − 15) + 10)
    n00 = g(i, j)·(fx, fy);          n10 = g(i+1, j)·(fx − 1, fy)
    n01 = g(i, j+1)·(fx, fy − 1);    n11 = g(i+1, j+1)·(fx − 1, fy − 1)
                                     # g(a, b) = gradient[pcg3(a, b, seed) mod 12]
    a = n00 + u·(n10 − n00);  b = n01 + u·(n11 − n01)
    return √2 · (a + v·(b − a))
```

`pcg3(a, b, c) mod 12` uses the unsigned 32-bit result. Further details that matter only
for bit-exact output:

- The 12 gradients are literal constants built from `0`, `±0.5`, `±0.8660254037844387`
  and `±1`, not computed with `cos` and `sin` at run time.
- `w⁴` is computed as `(w·w)·(w·w)`.
- Divisions by a size are done as multiplication by a stored reciprocal: `inv = 1/size`,
  then `x·inv` and `warpFreq·inv` in the level query; `f = 1/featureSize`, then `x·f` in
  the fields.
- The octave count of fBm and ridged noise is `max(1, floor(octaves))`. A level clamps
  jitter to [0, 1] and warp to at least 0.
- The seed text is trimmed of surrounding white space before it is parsed or hashed.
- Random unit vectors (rivers, lake axis) come from rejection sampling: draw
  `x = 2·rng() − 1`, `y = 2·rng() − 1` until `d = x² + y²` satisfies `d ≤ 1` and
  `d > 10⁻¹²` (rivers) or `d > 10⁻⁶` (lake axis), then divide by `√d`.

**Deterministic `exp`, `ln`, `sin`, `cos`.** Platform maths functions may differ in the
last bit between machines, so the generation uses its own, built
from `+ − × ÷`, rounding and the float's bit pattern only. Everything else in generation
uses `+ − × ÷ √` and comparisons. They are used in five places: `exp` and `ln` for the
log-scaled precipitation field, `exp` for the distribution of a field (9.1), `ln` for the
`ideal` term of the central places (15.2),
`cos` and `sin` for the 64 lake ray directions, and `sin(2π/64)` for the lake area.

```
exp(x):   k = round(x / ln2);  r = x − k·0.6931471803691238 − k·1.9082149292705877e−10
          s = 1;  for n = 16 down to 1:  s = 1 + r·s / n
          return s · 2^k                               # 2^k built from the exponent bits

ln(x):    split x = m · 2^e with m in [1, 2);  if m > √2:  m = m/2, e = e + 1
          s = (m − 1)/(m + 1);  t = 0
          for n = 25, 23, …, 1:  t = 1/n + s²·t
          return 2·s·t + e · 0.6931471805599453

sin(x), cos(x):
          x = x − 2π·round(x / 2π);  q = round(x / (π/2));  r = x − q·(π/2)
          sinP(r) = r·t  with t = 1;  for n = 17, 15, …, 3:  t = 1 − r²/(n(n − 1)) · t
          cosP(r) = t    with t = 1;  for n = 18, 16, …, 2:  t = 1 − r²/(n(n − 1)) · t
          q mod 4 = 0, 1, 2, 3 →  sin = sinP, cosP, −sinP, −cosP;  cos = cosP, −sinP, −cosP, sinP
```

---

## Appendix B. Crop tables

Columns: the temperature trapezoid over the growing-season temperature and the water
trapezoid over the yearly water (absolute min / optimum min / optimum max / absolute max);
the farmland factor by elevation type (flat / lowland / hills / mountains); limits on the yearly
mean temperature; whether it is a tree crop; a requirement (river, coast or tropical
highland); the gate (share of zones where the crop grows at all); and the yield factor `k`
(1 when blank). Section 14.2 gives the scoring.

Crops are numbered 0 … 107 in the order listed here (grain first, dyes last). A gated
crop number `n` grows in zone `(i, j)` when
`toUnit(hash4(seed, 0x5e100 + n, i, j)) < gate · boost` (`boost` = 1 for silk, which is
a fibre; the prosperity boost for spices, stimulants and dyes).

**Grain**

| Crop | Temperature °C | Water mm | Farmland F/L/H/M | min T | max T | Tree | Needs | Gate | k |
|---|---|---|---|---|---|---|---|---|---|
| bread wheat | 5 / 15 / 23 / 27 | 300 / 750 / 900 / 1600 | 1 / 1 / 0.6 / 0.3 |  |  |  |  |  |  |
| durum wheat | 6 / 17 / 25 / 30 | 400 / 500 / 700 / 800 | 1 / 1 / 0.6 / 0.3 | 9 |  |  |  |  |  |
| barley | 2 / 15 / 20 / 40 | 200 / 500 / 1000 / 2000 | 1 / 1 / 0.6 / 0.3 |  |  |  |  |  | 0.85 |
| rye | 3 / 15 / 20 / 31 | 400 / 600 / 1000 / 2000 | 1 / 1 / 0.6 / 0.3 |  | 12 |  |  |  | 0.8 |
| oats | 5 / 16 / 20 / 30 | 250 / 600 / 1000 / 1500 | 1 / 1 / 0.6 / 0.3 |  | 13 |  |  |  | 0.8 |
| paddy rice | 10 / 20 / 30 / 36 | 1000 / 1500 / 2000 / 4000 | 1 / 1 / 0.35 / 0 | 12 |  |  |  |  |  |
| maize | 10 / 18 / 33 / 47 | 400 / 600 / 1200 / 1800 | 1 / 1 / 0.6 / 0.3 | 9 |  |  |  |  |  |
| sorghum | 8 / 22 / 35 / 40 | 300 / 400 / 600 / 700 | 1 / 1 / 0.6 / 0.3 | 16 |  |  |  |  | 0.6 |
| pearl millet | 12 / 25 / 35 / 40 | 200 / 400 / 900 / 1700 | 1 / 1 / 0.6 / 0.3 | 18 |  |  |  |  | 0.5 |
| finger millet | 8 / 18 / 30 / 35 | 300 / 500 / 1100 / 4300 | 0.4 / 0.6 / 1 / 0.9 | 14 |  |  |  |  | 0.6 |
| proso millet | 15 / 20 / 32 / 45 | 200 / 500 / 750 / 1000 | 1 / 1 / 0.6 / 0.3 |  | 16 |  |  |  | 0.6 |
| buckwheat | 7 / 17 / 27 / 40 | 400 / 700 / 1000 / 1300 | 0.6 / 0.85 / 1 / 0.6 |  | 12 |  |  |  | 0.6 |
| fonio | 18 / 22 / 27 / 31 | 400 / 900 / 1600 / 2800 | 1 / 1 / 0.6 / 0.3 | 20 |  |  |  |  | 0.4 |
| teff | 2 / 22 / 28 / 30 | 300 / 600 / 1200 / 2500 | 0.4 / 0.6 / 1 / 0.9 |  |  |  | highland |  | 0.6 |
| quinoa | 2 / 14 / 18 / 35 | 250 / 500 / 1000 / 2600 | 0.4 / 0.6 / 1 / 0.9 |  |  |  | highland |  | 0.6 |

**Produce**

| Crop | Temperature °C | Water mm | Farmland F/L/H/M | min T | max T | Tree | Needs | Gate | k |
|---|---|---|---|---|---|---|---|---|---|
| potato | 7 / 15 / 25 / 30 | 250 / 500 / 800 / 2000 | 0.8 / 1 / 1 / 0.7 |  | 17 |  |  |  |  |
| cassava | 10 / 20 / 29 / 35 | 500 / 1000 / 1500 / 5000 | 1 / 1 / 0.6 / 0.3 | 18 |  |  |  |  |  |
| sweet potato | 10 / 18 / 28 / 38 | 350 / 750 / 2000 / 5000 | 1 / 1 / 0.6 / 0.3 | 15 |  |  |  |  |  |
| greater yam | 14 / 20 / 32 / 40 | 700 / 1200 / 4000 / 8000 | 1 / 1 / 0.6 / 0.3 | 20 |  |  |  |  |  |
| taro | 10 / 21 / 28 / 35 | 1000 / 1800 / 2700 / 4100 | 1 / 1 / 0.35 / 0 | 19 |  |  |  |  |  |
| banana and plantain | 12 / 23 / 33 / 42 | 650 / 1200 / 3600 / 5000 | 1 / 1 / 0.6 / 0.3 | 20 |  | yes |  |  | 0.6 |
| sago palm | 18 / 25 / 36 / 40 | 2100 / 3000 / 4500 / 5800 | 1 / 1 / 0 / 0 | 23 |  | yes |  |  | 0.6 |
| faba bean | 5 / 18 / 28 / 32 | 250 / 650 / 1000 / 2600 | 1 / 1 / 0.6 / 0.3 |  | 16 |  |  |  |  |
| pea | 4 / 10 / 24 / 30 | 350 / 800 / 1200 / 2500 | 1 / 1 / 0.6 / 0.3 |  | 14 |  |  |  |  |
| lentil | 5 / 15 / 29 / 32 | 250 / 600 / 1000 / 2500 | 1 / 1 / 0.6 / 0.3 | 8 | 19 |  |  |  |  |
| chickpea | 7 / 15 / 29 / 35 | 300 / 600 / 1000 / 1800 | 1 / 1 / 0.6 / 0.3 | 10 |  |  |  |  |  |
| common bean | 7 / 16 / 25 / 32 | 300 / 500 / 2000 / 4300 | 1 / 1 / 0.6 / 0.3 | 10 |  |  |  |  |  |
| cowpea | 15 / 25 / 35 / 40 | 300 / 500 / 1500 / 4100 | 1 / 1 / 0.6 / 0.3 | 19 |  |  |  |  |  |
| cabbage and kale | 5 / 10 / 25 / 30 | 450 / 600 / 1100 / 1900 | 1 / 1 / 0.6 / 0.3 |  | 15 |  |  |  |  |
| onion and alliums | 4 / 12 / 25 / 30 | 300 / 350 / 600 / 2800 | 1 / 1 / 0.6 / 0.3 |  |  |  |  |  | 0.8 |
| carrot and root vegetables | 3 / 15 / 24 / 30 | 400 / 600 / 1200 / 4000 | 1 / 1 / 0.6 / 0.3 |  | 16 |  |  |  |  |
| melons | 15 / 20 / 35 / 40 | 400 / 500 / 700 / 1800 | 1 / 1 / 0.6 / 0.3 | 14 |  |  |  |  | 0.8 |
| enset | 8 / 16 / 24 / 30 | 900 / 1100 / 1500 / 1700 | 0.4 / 0.6 / 1 / 0.9 |  |  | yes | highland |  |  |

**Orchard**

| Crop | Temperature °C | Water mm | Farmland F/L/H/M | min T | max T | Tree | Needs | Gate | k |
|---|---|---|---|---|---|---|---|---|---|
| olive | 5 / 20 / 34 / 40 | 200 / 400 / 700 / 1200 | 0.6 / 0.85 / 1 / 0.6 | 12 | 21 | yes |  |  |  |
| grape | 10 / 18 / 30 / 38 | 400 / 700 / 850 / 1200 | 0.6 / 0.85 / 1 / 0.6 | 9 | 20 | yes |  |  |  |
| date palm | 10 / 26 / 45 / 52 | 100 / 200 / 300 / 400 | 1 / 1 / 0.35 / 0 | 19 |  | yes | river |  |  |
| fig | 4 / 16 / 26 / 38 | 300 / 700 / 1500 / 2700 | 1 / 1 / 0.6 / 0.3 | 11 |  | yes |  |  | 0.8 |
| pomegranate | 8 / 23 / 32 / 40 | 400 / 900 / 1200 / 4200 | 1 / 1 / 0.6 / 0.3 | 13 |  | yes |  |  | 0.8 |
| almond | 10 / 12 / 35 / 40 | 250 / 600 / 900 / 1500 | 0.6 / 0.85 / 1 / 0.6 | 11 | 21 | yes |  |  | 0.8 |
| pistachio | 12 / 25 / 35 / 40 | 250 / 400 / 700 / 1100 | 0.6 / 0.85 / 1 / 0.6 | 13 | 22 | yes |  |  | 0.7 |
| apple | 8 / 14 / 27 / 33 | 500 / 700 / 2500 / 3200 | 0.6 / 0.85 / 1 / 0.6 | 5 | 15 | yes |  |  |  |
| pear | 10 / 20 / 35 / 37 | 400 / 600 / 900 / 2100 | 0.6 / 0.85 / 1 / 0.6 | 6 | 16 | yes |  |  | 0.8 |
| peach and stone fruit | 7 / 20 / 33 / 35 | 750 / 900 / 1100 / 1600 | 1 / 1 / 0.6 / 0.3 | 10 | 19 | yes |  |  | 0.9 |
| citrus | 13 / 20 / 30 / 38 | 450 / 1200 / 2000 / 2700 | 1 / 1 / 0.6 / 0.3 | 16 |  | yes |  |  |  |
| walnut | 7 / 15 / 30 / 40 | 400 / 800 / 1700 / 2200 | 0.6 / 0.85 / 1 / 0.6 | 7 | 17 | yes |  |  | 0.8 |
| sweet chestnut | 7 / 15 / 25 / 30 | 500 / 900 / 1300 / 1600 | 0.6 / 0.85 / 1 / 0.6 | 8 | 16 | yes |  |  | 0.8 |
| hazel | 5 / 10 / 24 / 35 | 600 / 900 / 1100 / 1400 | 1 / 1 / 0.6 / 0.3 | 4 | 14 | yes |  |  | 0.6 |
| coconut | 14 / 22 / 34 / 38 | 650 / 1200 / 2400 / 4000 | 1 / 1 / 0.6 / 0.3 | 22 |  | yes | coast |  |  |
| oil palm | 12 / 20 / 35 / 38 | 1000 / 1500 / 3000 / 8000 | 1 / 1 / 0.35 / 0 | 23 |  | yes |  |  |  |
| mango | 8 / 24 / 30 / 48 | 300 / 600 / 1500 / 2600 | 1 / 1 / 0.6 / 0.3 | 20 |  | yes |  |  | 0.8 |
| cashew | 5 / 15 / 35 / 46 | 400 / 750 / 1600 / 4000 | 1 / 1 / 0.6 / 0.3 | 21 |  | yes |  |  | 0.7 |
| shea | 18 / 24 / 38 / 43 | 300 / 500 / 1000 / 1800 | 1 / 1 / 0.6 / 0.3 | 23 |  | yes |  |  | 0.7 |
| baobab | 14 / 26 / 36 / 42 | 250 / 600 / 1500 / 4200 | 1 / 1 / 0.6 / 0.3 | 23 |  | yes |  |  | 0.5 |
| tamarind | 12 / 20 / 35 / 45 | 300 / 800 / 3000 / 4500 | 1 / 1 / 0.6 / 0.3 | 21 |  | yes |  |  | 0.6 |
| breadfruit | 16 / 21 / 33 / 40 | 1000 / 1500 / 3000 / 3500 | 1 / 1 / 0.6 / 0.3 | 23 |  | yes |  |  | 0.8 |
| Brazil nut | 12 / 20 / 36 / 40 | 1400 / 2400 / 2800 / 3500 | 1 / 1 / 0.35 / 0 | 23 |  | yes |  |  | 0.7 |
| avocado | 10 / 14 / 40 / 45 | 300 / 500 / 2000 / 2500 | 0.4 / 0.6 / 1 / 0.9 |  |  | yes | highland |  | 0.8 |
| sunflower | 5 / 17 / 34 / 45 | 300 / 600 / 1000 / 1600 | 1 / 1 / 0.6 / 0.3 | 6 | 17 |  |  |  | 0.6 |
| rapeseed | 5 / 15 / 25 / 41 | 400 / 500 / 1000 / 2800 | 1 / 1 / 0.6 / 0.3 |  | 13 |  |  |  | 0.6 |
| sesame | 10 / 20 / 30 / 40 | 300 / 500 / 1000 / 1500 | 1 / 1 / 0.6 / 0.3 | 17 |  |  |  |  | 0.6 |
| groundnut | 10 / 22 / 32 / 45 | 400 / 600 / 1500 / 4000 | 1 / 1 / 0.6 / 0.3 | 18 |  |  |  |  | 0.6 |
| soybean | 10 / 20 / 33 / 38 | 450 / 600 / 1500 / 1800 | 1 / 1 / 0.6 / 0.3 | 10 | 19 |  |  |  | 0.6 |

**Fibre**

| Crop | Temperature °C | Water mm | Farmland F/L/H/M | min T | max T | Tree | Needs | Gate | k |
|---|---|---|---|---|---|---|---|---|---|
| flax | 5 / 16 / 24 / 30 | 250 / 500 / 800 / 1300 | 1 / 1 / 0.6 / 0.3 |  | 13 |  |  |  |  |
| hemp | 6 / 15 / 28 / 32 | 350 / 600 / 1200 / 4000 | 1 / 1 / 0.6 / 0.3 | 5 | 17 |  |  |  |  |
| cotton | 15 / 22 / 36 / 42 | 450 / 750 / 1200 / 1500 | 1 / 1 / 0.6 / 0.3 | 17 |  |  |  |  |  |
| jute | 13 / 20 / 37 / 45 | 900 / 1500 / 3000 / 3600 | 1 / 1 / 0.35 / 0 | 21 |  |  | river |  |  |
| ramie | 10 / 17 / 27 / 34 | 1000 / 1500 / 2500 / 4000 | 1 / 1 / 0.6 / 0.3 | 14 |  |  |  |  | 0.8 |
| kenaf | 10 / 15 / 28 / 35 | 450 / 600 / 2000 / 3000 | 1 / 1 / 0.6 / 0.3 | 21 |  |  |  |  | 0.7 |
| sisal and agave | 10 / 15 / 27 / 45 | 500 / 900 / 1250 / 1800 | 0.6 / 0.85 / 1 / 0.6 | 18 |  | yes |  |  | 0.7 |
| abaca | 16 / 20 / 29 / 33 | 700 / 1800 / 3000 / 4400 | 1 / 1 / 0.6 / 0.3 | 23 |  | yes |  |  | 0.7 |
| esparto grass | 3 / 15 / 25 / 35 | 100 / 200 / 500 / 600 | 0.6 / 0.85 / 1 / 0.6 | 10 |  | yes |  |  | 0.5 |
| silk | 13 / 20 / 28 / 45 | 300 / 700 / 2500 / 5100 | 0.6 / 0.85 / 1 / 0.6 | 12 | 20 | yes |  | 0.2 |  |
| kapok | 12 / 17 / 38 / 40 | 750 / 1500 / 2500 / 3000 | 1 / 1 / 0.6 / 0.3 | 22 |  | yes |  |  | 0.5 |
| New Zealand flax | 9 / 13 / 25 / 30 | 1300 / 2000 / 3000 / 3800 | 1 / 1 / 0.6 / 0.3 | 8 | 15 | yes |  |  | 0.6 |
| nettle and lime-bast | 5 / 12 / 22 / 28 | 400 / 500 / 700 / 800 | 1 / 1 / 0.6 / 0.3 |  | 12 | yes |  |  | 0.35 |

**Spices & incense**

| Crop | Temperature °C | Water mm | Farmland F/L/H/M | min T | max T | Tree | Needs | Gate | k |
|---|---|---|---|---|---|---|---|---|---|
| black pepper | 10 / 22 / 35 / 40 | 2000 / 2500 / 4000 / 5500 | 1 / 1 / 0.6 / 0.3 | 22 |  | yes |  | 0.08 |  |
| clove | 20 / 24 / 34 / 38 | 1200 / 1500 / 3600 / 4000 | 1 / 1 / 0.35 / 0 | 24 |  | yes | coast | 0.06 |  |
| nutmeg and mace | 12 / 22 / 34 / 38 | 1200 / 1500 / 3500 / 4000 | 1 / 1 / 0.35 / 0 | 24 |  | yes | coast | 0.06 |  |
| cinnamon | 18 / 24 / 30 / 36 | 1200 / 2000 / 2600 / 3000 | 1 / 1 / 0.6 / 0.3 | 22 |  | yes |  | 0.08 |  |
| cardamom | 10 / 22 / 30 / 35 | 1500 / 3000 / 6000 / 7000 | 0.4 / 0.6 / 1 / 0.9 | 20 |  | yes |  | 0.1 |  |
| ginger | 13 / 19 / 29 / 35 | 700 / 1400 / 3000 / 4000 | 1 / 1 / 0.6 / 0.3 | 20 |  |  |  | 0.1 | 0.8 |
| turmeric | 18 / 20 / 28 / 32 | 800 / 1000 / 2000 / 3000 | 1 / 1 / 0.6 / 0.3 | 20 |  |  |  | 0.1 | 0.8 |
| vanilla | 10 / 21 / 30 / 33 | 1500 / 2000 / 2500 / 3000 | 1 / 1 / 0.35 / 0 | 22 |  | yes |  | 0.05 |  |
| chili pepper | 8 / 17 / 30 / 35 | 500 / 600 / 1250 / 1700 | 1 / 1 / 0.6 / 0.3 | 20 |  |  |  | 0.1 | 0.7 |
| myrrh | 18 / 23 / 33 / 40 | 250 / 400 / 800 / 1000 | 0.6 / 0.85 / 1 / 0.6 | 22 |  | yes |  | 0.1 |  |
| frankincense | 18 / 26 / 40 / 47 | 100 / 150 / 400 / 600 | 0 / 0.5 / 1 / 1 | 22 |  | yes |  | 0.1 |  |
| gum arabic | 14 / 24 / 32 / 48 | 200 / 300 / 800 / 2300 | 1 / 1 / 0.6 / 0.3 | 21 |  | yes |  | 0.12 | 0.7 |
| sandalwood | 10 / 22 / 30 / 38 | 450 / 800 / 1400 / 3000 | 0.6 / 0.85 / 1 / 0.6 | 21 |  | yes |  | 0.07 |  |
| rubber latex | 10 / 24 / 33 / 45 | 1200 / 2000 / 4000 / 6000 | 1 / 1 / 0.35 / 0 | 23 |  | yes |  | 0.07 | 0.7 |

**Stimulants & sugar**

| Crop | Temperature °C | Water mm | Farmland F/L/H/M | min T | max T | Tree | Needs | Gate | k |
|---|---|---|---|---|---|---|---|---|---|
| sugar cane | 15 / 24 / 37 / 41 | 1000 / 1500 / 2000 / 5000 | 1 / 1 / 0.35 / 0 | 20 |  |  |  | 0.2 |  |
| palm sugar and toddy | 16 / 24 / 32 / 36 | 700 / 1000 / 1500 / 4200 | 1 / 1 / 0.6 / 0.3 | 22 |  | yes |  | 0.12 | 0.5 |
| tea | 8 / 20 / 30 / 35 | 1000 / 1400 / 2000 / 5000 | 0 / 0.4 / 1 / 1 | 12 |  | yes |  | 0.15 |  |
| coffee | 10 / 14 / 28 / 34 | 750 / 1400 / 2300 / 4200 | 0 / 0.4 / 1 / 1 |  |  | yes | highland | 0.3 |  |
| cacao | 10 / 21 / 32 / 38 | 900 / 1200 / 3000 / 7600 | 1 / 1 / 0.35 / 0 | 22 |  | yes |  | 0.12 |  |
| kola nut | 10 / 21 / 30 / 35 | 1200 / 1400 / 2500 / 3500 | 1 / 1 / 0.35 / 0 | 23 |  | yes |  | 0.08 | 0.7 |
| betel nut | 7 / 16 / 33 / 39 | 750 / 1500 / 3000 / 5000 | 1 / 1 / 0.6 / 0.3 | 21 |  | yes |  | 0.1 | 0.7 |
| coca | 14 / 17 / 23 / 27 | 700 / 1000 / 2100 / 4000 | 0 / 0 / 0.8 / 1 |  |  | yes | highland | 0.15 | 0.8 |
| tobacco | 7 / 15 / 30 / 35 | 350 / 500 / 750 / 3000 | 1 / 1 / 0.6 / 0.3 | 11 |  |  |  | 0.12 | 0.8 |

**Dyes**

| Crop | Temperature °C | Water mm | Farmland F/L/H/M | min T | max T | Tree | Needs | Gate | k |
|---|---|---|---|---|---|---|---|---|---|
| woad | 5 / 14 / 22 / 28 | 450 / 600 / 1000 / 1500 | 1 / 1 / 0.6 / 0.3 |  | 14 |  |  | 0.12 | 0.7 |
| madder | 8 / 16 / 26 / 32 | 350 / 500 / 900 / 1300 | 1 / 1 / 0.6 / 0.3 | 9 | 19 |  |  | 0.12 | 0.7 |
| weld | 6 / 14 / 24 / 30 | 400 / 500 / 900 / 1200 | 1 / 1 / 0.6 / 0.3 | 7 | 17 |  |  | 0.1 | 0.5 |
| indigo | 7 / 22 / 28 / 32 | 640 / 1300 / 1700 / 3000 | 1 / 1 / 0.6 / 0.3 | 19 |  |  |  | 0.15 |  |
| safflower | 5 / 20 / 32 / 45 | 300 / 600 / 1000 / 1400 | 1 / 1 / 0.6 / 0.3 | 13 |  |  |  | 0.12 | 0.6 |
| henna | 13 / 19 / 27 / 33 | 250 / 500 / 1500 / 4200 | 1 / 1 / 0.6 / 0.3 | 20 |  | yes |  | 0.12 | 0.6 |
| annatto | 15 / 20 / 28 / 36 | 1250 / 1500 / 2000 / 2500 | 1 / 1 / 0.6 / 0.3 | 22 |  | yes |  | 0.1 | 0.6 |
| red dyewoods | 7 / 26 / 32 / 46 | 875 / 1000 / 1900 / 2800 | 1 / 1 / 0.6 / 0.3 | 22 |  | yes |  | 0.07 |  |
| lac | 12 / 22 / 32 / 40 | 700 / 1000 / 1800 / 2600 | 1 / 1 / 0.6 / 0.3 | 21 |  | yes |  | 0.1 | 0.7 |
| cochineal | 10 / 18 / 26 / 30 | 300 / 700 / 1300 / 1700 | 0.6 / 0.85 / 1 / 0.6 | 14 |  | yes |  | 0.06 |  |

---

## Appendix C. Factors by biome

Soil and tree ground are used by the crops (14.2); pasture, game, wood, fur, mounts and
honey by the other potentials (Appendix D); "veg" is the travel slowdown of the
central-place graph (15.2); "road cost" is the biome factor of the travel grid (16.1).

| Biome | Soil | Tree ground | Pasture | Game | Wood | Fur | Mounts | Honey | Veg | Road cost |
|---|---|---|---|---|---|---|---|---|---|---|
| Ocean | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 1 | 1 |
| Ice & snow | 0 | 0 | 0 | 0.05 | 0 | 0 | 0 | 0 | 1.5 | 5 |
| Tundra | 0.3 | 0.2 | 0.25 | 0.3 | 0 | 0.5 | 0 | 0 | 1.1 | 1.6 |
| Cold steppe | 1 | 1 | 0.7 | 0.3 | 0.08 | 0.15 | 0.9 | 0.1 | 1 | 1.1 |
| Boreal forest | 0.55 | 1 | 0.25 | 0.7 | 1 | 0.85 | 0 | 0.2 | 1.3 | 1.4 |
| Cold desert | 1 | 1 | 0.2 | 0.1 | 0 | 0 | 0.5 | 0 | 1 | 3.5 |
| Temperate grassland | 1 | 1 | 1 | 0.4 | 0.1 | 0 | 0.9 | 0.2 | 1 | 1 |
| Woodland & shrubland | 1 | 1 | 0.7 | 0.45 | 0.4 | 0 | 0.3 | 0.3 | 1.1 | 1.1 |
| Temperate mixed forest | 1 | 1 | 0.6 | 0.8 | 1 | 0.3 | 0.15 | 0.2 | 1.3 | 1.3 |
| Temperate broadleaf forest | 1 | 1 | 0.6 | 0.8 | 1 | 0.1 | 0.15 | 0.3 | 1.3 | 1.3 |
| Temperate rain forest | 0.55 | 1 | 0.5 | 0.8 | 1 | 0.2 | 0 | 0.15 | 2 | 2 |
| Hot desert | 1 | 1 | 0.15 | 0.05 | 0 | 0 | 0.5 | 0 | 1.1 | 4 |
| Thorn scrub | 0.7 | 1 | 0.5 | 0.3 | 0.25 | 0.25 | 0.4 | 0.15 | 1.1 | 1.5 |
| Savanna | 0.6 | 1 | 0.8 | 0.6 | 0.35 | 0.4 | 0.3 | 0.2 | 1 | 1.05 |
| Tropical dry forest | 0.7 | 1 | 0.45 | 0.6 | 0.8 | 0.35 | 0.1 | 0.2 | 1.3 | 1.3 |
| Tropical rain forest | 0.4 | 1 | 0.15 | 0.6 | 1 | 0.35 | 0 | 0.15 | 2 | 2.2 |
| Mangrove | 0.3 | 0.4 | 0.1 | 0.4 | 0.5 | 0 | 0 | 0.1 | 2 | 3 |

---

## Appendix D. Resource rules in full

Notation: `zt` = elevation type; *low* = flat or lowland, *high* = hills or mountains;
`river`, `mouth`, `lake`, `harbour`, `goldLoad`, `tinLoad` as in 14.1; `T` = temperature
after cooling, `T₀` = at sea level; `m` = aridity; *humid* = `m ≥ 2`; belts in the order
polar, subpolar, boreal, temperate, tropical. `put(resource, amount, variant)` keeps the
larger amount. `pick[a, b, …]` chooses one name by
`hash4(seed, 0x5e10 + resource index, patch i, patch j) mod count`, where the resource
index is its position in this list (from 0): grain, produce, orchard, livestock, fish, wild
food, timber, stone, clay, crystal, fibre, fuel, salt, iron ore, copper ore, tin,
silver & lead, gold, gems, spices, stimulants, furs, dyes, horses.

The rules run in this order, because fuel reads the timber and livestock amounts: grain,
produce, orchard (crops) → livestock → fish → wild food → timber → stone → clay → crystal →
fibre (crops) → fuel → salt → metals → gems → spices, stimulants (crops) → honey → furs →
dyes → horses.

### D.1 Potentials

**Livestock.** Amount = `pasture · (1, 1, 0.9, 0.7 by elevation type)`, + 0.1 if `river ≥ 1`
and the belt is boreal or warmer. Variant, first match: mountains with `T < 5` and not
tundra → yak; mountains → goats (shrubland, thorn scrub) or alpine summer pasture; tundra
or boreal forest → herded reindeer; cold steppe or cold desert → sheep; grassland → pick[cattle,
sheep]; shrubland → goats in hills, else sheep; broadleaf → pick[pigs, cattle]; mixed
forest or temperate rain forest → pick[cattle, sheep]; hot desert or thorn scrub → goats;
dry forest → water buffalo on a floodplain, else cattle; rain forest or mangrove →
pick[pigs, poultry]; otherwise cattle.

**Fish.** All that apply are `put`:

- Coastal patch: mangrove → 0.9 "crabs, lobsters and shrimp"; else `mouth ≥ 2` and
  `T₀ < 10` → 1 in a harbour, else 0.85, "salmon runs"; else amount 0.6 in the polar belt,
  1 in a harbour, otherwise 0.8, with the variant by `T₀`: below 4 cod banks, below 11
  herring, below 18 pick[sardine and anchovy, oysters and mussels], else pick[reef fish
  and shellfish, tuna and large pelagics].
- `river > 0` outside the polar belt: 0.7 from 80, 0.6 from 8, 0.3 from 2, else 0.15.
  Variant: sturgeon (`river ≥ 80`, temperate or colder), pick[river fish, eels]
  (`river ≥ 8`, temperate), else river fish.
- Lake: `0.45 + 0.3 · clamp(lake area / 20 km²)`, "lake fish".

**Wild food.** Amount = `game`, + 0.15 with a lake or on a floodplain outside the polar
belt; on ice it is 0 unless coastal. Variant by biome: ice seals; tundra pick[caribou,
cloudberry and crowberry]; boreal forest pick[deer and elk, bilberry and blueberry, forest
mushrooms]; mixed forest pick[deer, wild boar, forest mushrooms]; broadleaf pick[deer,
wild boar, acorns]; temperate rain forest pick[deer, raspberry and bramble]; grassland
pick[bison and wild cattle, hares and rabbits]; cold steppe pick[antelope and gazelle,
upland game birds]; shrubland pick[upland game birds, wild herbs, hares and rabbits];
mangrove waterfowl; cold desert pick[hares and rabbits, upland game birds]; rain forest
and dry forest pick[wild boar, deer, wild herbs]; others antelope and gazelle. Overrides:
in mountains (not ice or tundra) wild sheep and ibex; otherwise with a lake in the
temperate belt waterfowl, in the tropical belt lotus.

**Timber.** Amount = `wood`. Grassland and cold steppe: + 0.2 if `river ≥ 1`, else + 0.07
in hills and mountains. Hot and cold desert: 0.3 if `river ≥ 2`, else 0. Tundra: 0.1 if
coastal, else 0. In mountains an amount above 0.6 is multiplied by 0.8. Listed only when
above 0. Variant:

- boreal forest: high → pick[larch, spruce]; else pick[Scots pine, spruce]
- mixed forest: mountains fir; hills pick[beech, spruce]; else pick[oak, Scots pine, lime]
- broadleaf: mountains cedar (`T ≥ 12`) or beech; hills pick[beech, oak]; else on a
  floodplain pick[oak, ash, elm], otherwise oak
- temperate rain forest: giant conifers
- shrubland: `T < 10` → juniper (high) or pick[oak, Scots pine, juniper]; else
  pick[cypress, Mediterranean pines] (high) or pick[cork oak, Mediterranean pines]
- dry forest: `P ≥ 1500` → pick[teak, sal]; else pick[teak, Spanish cedar, rosewood]
- rain forest: pick[mahogany, iroko, rosewood, ebony, mahogany, iroko]
- savanna: African mahogany if `river ≥ 1`, else thorn-scrub wood; thorn scrub:
  thorn-scrub wood; mangrove: mangrove timber; deserts: poplar; tundra and ice: driftwood
- grassland and cold steppe: willow and poplar if `river ≥ 1`, else juniper

**Clay** (not on ice). Amount = `(0.7, 0.6, 0.25, 0.1 by elevation type)`, + 0.3 if
`river ≥ 1` on low ground, + 0.1 on unconsolidated cover, × 0.3 on tundra. Variant, first
match: `m < 1` in the temperate or tropical belt → adobe and mud-brick earth; `river ≥ 1`
→ brick and pottery clay; mixed or broadleaf forest → wattle and daub; `m < 1` or
shrubland → rammed earth and cob; else brick and pottery clay.

**Fuel.** All that apply are `put`: `0.9 · timber` when timber > 0 (charcoal in mangrove;
pick[firewood, charcoal] when timber ≥ 0.7; else firewood); peat 0.6 on low ground that is
humid, `T < 10`, boreal or warmer; turf and sod on tundra, 0.35 on low ground, else 0.2;
dung fuel `0.35 · livestock` when livestock ≥ 0.15.

**Honey.** `put(stimulants, honey, "honey and beeswax")`.

**Furs & ivory.** Amount = `fur`. It counts only where the zone passes its gate: in the
tropical belt `toUnit(hash4(seed, 0x5e04, zone i, zone j)) < 0.3 · boost`, elsewhere
`toUnit(hash4(seed, 0x5e05, zone i, zone j)) < 0.5 · boost`. Variant by biome: boreal forest →
pick[beaver, sable and marten] if `river ≥ 1`, else pick[sable and marten, squirrel,
ermine]; tundra → fox and Arctic fox, or on the coast pick[walrus, seals, fox and Arctic
fox] with + 0.15; ice → pick[walrus, seals, narwhal tusk] with amount 0.5 on the coast and
0 inland; mixed forest and temperate rain forest → bear (high), beaver (`river ≥ 1`), else
pick[squirrel, sable and marten]; broadleaf and cold steppe → fox and Arctic fox; savanna →
pick[elephant ivory, ostrich, big cats]; thorn scrub → ostrich; dry forest →
pick[elephant ivory, big cats]; rain forest → pick[exotic feathers, big cats, elephant
ivory]. A biome with none of these on a coast with `T₀ < 3` gives seals at 0.3.

**Dyes.** The dye crops, plus lichen dyes at 0.15 on tundra and in temperate rain forest,
plus the deposits murex and kermes below.

**Horses & pack animals.** Amount = `mounts · (1, 1, 0.7, 0.4 by elevation type)`. Variant,
first match: hot desert or thorn scrub → donkeys and mules (high) or dromedary camel; cold
desert → Bactrian camel; tropical highland in mountains → llama and alpaca-like camelids,
amount at least `0.5 · pasture`; high → donkeys and mules, amount at least `0.3 · pasture`;
else horses.

### D.2 Deposits

`#` is the kind number of the hash purpose (14.4). The chance is multiplied by the
prosperity boost for silver & lead, gold, gems, furs and dyes. "Rich" is the richness
factor (1 when blank).

| # | Resource | Variant | Condition | Chance | Rich |
|---|---|---|---|---|---|
| 1 | Stone | by bedrock, see below | any land patch | elevation type 0.04 / 0.12 / 0.75 / 0.95 × bedrock factor | elevation type 0.35 / 0.5 / 0.85 / 1 |
| 2 | Clay | kaolin | granite or shield, `T ≥ 10`, humid, not mountains | 0.05 | 1.2 |
| 3 | Crystal | rock crystal | high | 0.05 mountains, 0.03 hills; × 3 volcanic, × 2 granite or fold belt | |
| 4 | Crystal | glass sand | low | 0.03 if coastal + 0.015 if `river ≥ 8` | 0.8 |
| 5 | Fuel | coal | clastic; hills or lowland | 0.04 | |
| 6 | Fuel | lignite | clastic; low | 0.04 | 0.7 |
| 7 | Fuel | oil and naphtha seeps | clastic; low | 0.01 | 0.8 |
| 8 | Salt | solar sea salt | coastal, low, `m < 1`, `T ≥ 12` | 0.25 | |
| 9 | Salt | lake and playa salt | river-poor flat zone, `m < 0.5`, zone gate 8 % (purpose 0x5e02) | 0.3 | |
| 10 | Salt | natron and soda | as 9 | 0.05 | 0.8 |
| 11 | Salt | rock salt | clastic, not mountains, zone gate 5 % (purpose 0x5e03) | 0.12 | |
| 12 | Salt | brine springs | as 11 | 0.08 | 0.8 |
| 13 | Iron | bog iron | low, humid, boreal forest / mixed / broadleaf / temperate rain forest | 0.1 with a river (`≥ 1`) or lake, else 0.05 | 0.7 |
| 14 | Iron | lateritic iron and ironstone | not mountains; rain forest, dry forest or savanna | 0.1 | 0.8 |
| 15 | Iron | banded iron formation | shield; hills or lowland | 0.08 | |
| 16 | Iron | magnetite skarn iron | granite or carbonate; high | 0.06 | |
| 17 | Iron | black sands | coastal; basalt | 0.05 | 0.6 |
| 18 | Copper | pick[porphyry copper, malachite and azurite, porphyry copper] | volcanic; high | 0.1 mountains, 0.05 hills | |
| 19 | Copper | red-bed copper | clastic; lowland or hills | 0.03 | 0.8 |
| 20 | Copper | massive-sulfide copper | ophiolite; high | 0.08 | |
| 21 | Copper | native copper | basalt; not flat | 0.02 lowland, 0.06 high | 0.8 |
| 22 | Tin | tin lode | granite; high | 0.06 | |
| 23 | Tin | stream tin | bedrock not volcanic, basalt, ophiolite or clastic; not mountains; `river ≥ 1` | `0.35 · tinLoad` | `0.4 + 0.6 · tinLoad` |
| 24 | Silver & lead | silver | fold belt; high | 0.05 | |
| 25 | Silver & lead | pick[lead-zinc in carbonates, calamine, lead-zinc in carbonates] | carbonate; hills or lowland | 0.06 hills, 0.025 lowland | 0.8 |
| 26 | Silver & lead | lead-zinc skarns and veins | volcanic; mountains | 0.03 | |
| 27 | Gold | lode gold | fold belt; high | 0.03 | |
| 28 | Gold | lode gold | shield; hills | 0.03 | |
| 29 | Gold | epithermal and hot-spring gold-silver | volcanic; mountains | 0.03 | |
| 30 | Gold | placer gold | not mountains; `river ≥ 1` | `0.2 · goldLoad` | `0.4 + 0.6 · goldLoad` |
| 31 | Gold | beach gold | coastal; `mouth ≥ 2` | `0.1 · goldLoad` | 0.6 |
| 32 | Gems | diamond | shield; not mountains | 0.004 | |
| 33 | Gems | topaz, beryl and tourmaline | granite; high | 0.04 | |
| 34 | Gems | garnet | fold belt; high | 0.03 | 0.7 |
| 35 | Gems | emerald | fold belt; high | 0.004 | |
| 36 | Gems | amethyst and agate | volcanic or basalt; high | 0.04 | 0.7 |
| 37 | Gems | ruby and sapphire | basalt or carbonate; mountains | 0.008 | |
| 38 | Gems | jade | ophiolite; high | 0.04 | |
| 39 | Gems | lapis lazuli | carbonate; mountains | 0.012 | |
| 40 | Gems | turquoise | hot desert, cold desert or thorn scrub; high; volcanic or clastic | 0.04 | |
| 41 | Gems | opal | hot desert, cold desert or thorn scrub; lowland or hills; clastic or basalt | 0.012 | |
| 42 | Gems | amber | coastal; `T < 12`; boreal or warmer; clastic or cover | 0.03 | |
| 43 | Gems | jet | coastal; clastic; hills | 0.02 | 0.6 |
| 44 | Gems | pearl oyster | coastal; `T₀ ≥ 20` | 0.04 | |
| 45 | Gems | freshwater pearl mussel | `river ≥ 2`; `T < 9`; boreal or warmer | 0.006 | 0.7 |
| 46 | Gems | precious red coral | coastal; `11 ≤ T₀ < 20`; high | 0.03 | |
| 47 | Dyes | murex | coastal; high; `12 ≤ T₀ < 21` | 0.06 | |
| 48 | Dyes | kermes | shrubland; hills; `T ≥ 12` | 0.06 | |
| 49 | Furs | musk deer | mountains; boreal belt | 0.03 | |
| 50 | Gems | pearl oyster (ocean patch) | next to land; `T₀ ≥ 20` | 0.05 | |
| 51 | Gems | precious red coral (ocean patch) | next to land; `11 ≤ T₀ < 20` | 0.03 | |

**Stone by bedrock** (bedrock factor on the chance, then the variant): shield and granite
× 1, granite; fold belt × 1, pick[slate, slate, marble] in mountains, else pick[slate,
whetstone, slate]; volcanic × 1, pick[basalt, pumice and pozzolana, obsidian, basalt];
basalt × 2, pick[basalt, basalt, millstone and quern stone]; ophiolite × 1, soapstone;
carbonate × 2.5, pick[chalk, limestone, flint and chert] on low ground, else
pick[limestone, limestone, marble]; clastic × 1.6, pick[sandstone, sandstone, millstone
and quern stone]; unconsolidated cover × 0.5, field stone and boulders.

### D.3 Ocean patches

An ocean patch is *on the shelf* when any of its patch cells has a land patch as
neighbour. With `T₀` at its site:

- Shelf: fish 0.9 (cod banks below 4 °C, herring below 11, sardine and anchovy below 18,
  else reef fish and shellfish); wild food 0.3 "edible seaweed and kelp" if `T₀ < 12`;
  furs 0.4 "seals" if `T₀ < 3` and the patch passes the fur gate; pearl oyster (kind 50)
  if `T₀ ≥ 20`, else precious red coral (kind 51) if `T₀ ≥ 11`.
- Open water: fish 0.5 (cod banks below 8 °C, herring below 16, else tuna and large
  pelagics); furs 0.3 "whales" if `T₀ < 6` and the patch passes the fur gate.

The fur gate of an ocean patch is `toUnit(hash4(seed, 0x5e05, patch i, patch j)) < 0.5 · boost`.
Kinds 50 and 51 use `chance · boost` and richness `0.35 + 0.65 · hash` as in 14.4.
