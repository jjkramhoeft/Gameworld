# Plan: keep Prospects clear of patch borders

Status: planned 2026-11-14, not implemented.

## Problem

A Prospect is often drawn on the border of its patch, so the map does not show which patch it
belongs to. In the demo app a click on such a marker can hit the neighbouring patch, which has
no Prospect.

## Cause

The Spot rule (map-generation.md section 15.1) puts the Prospect on the river node with the
largest flow in the patch. The flow grows downstream, so that node is where the river leaves
the patch: on the border.

Measured on 3 × 3 regions around (0, 0), default Parameter Set, 180 Prospects per seed. The
distance is to the nearest other land patch; ocean does not count.

| Spot kind (seed `cellgen`) | Prospects | Median distance | Under 1 km |
|---|---|---|---|
| River node, not a mouth | 130 | 400 m | 95 % |
| River mouth | 12 | 3.2 km | 0 % |
| Patch site | 37 | 2.5 km | 5 % |

Over all Prospects the median is 600 m and the smallest 100 m, on both `cellgen` and `atlas`.
On `cellgen` every Prospect patch has an interior point at least 1.75 km from its border.

## Decisions

1. Only the Spot rule changes. Scoring (15.3, 15.4) and selection (15.5) keep their text.
2. One new parameter: `Planning.BorderClearanceKm`, default 2, range 0 and up. 0 gives today's
   placement.
3. The clearance is measured to other **land** patches. A Prospect may stand on the coast: an
   ocean patch never holds a Prospect, so the coast is not ambiguous.
4. A river is preferred over the full clearance. A Prospect leaves its river only when no
   river node of the patch is 1 km (half the clearance) from the border.
5. The navigable river is preferred over a tributary, so the travel network keeps its river
   landings.
6. The river landing of the travel network (16.2) searches ±3 cells instead of ±2, because a
   Prospect can now stand up to about 6 km from the navigable river of its patch.
7. The generator version stays 1 during phase 1.

## The new Spot rule (replaces the Spot bullet of 15.1)

```
c = border clearance (2 km)

clear(q, r):  q is in the patch and outside any lake, and none of the 16 points at distance
              r and r/2 from q in the 8 directions E, S, W, N, SE, SW, NW, NE (diagonals as
              (±0.7, ±0.7)) lies in another land patch

candidates, in three groups:
  1. navigable river nodes of the patch (flow ≥ 8 × attractors per patch cell)
  2. all river nodes of the patch with a flow of at least 3
  3. the patch's site, the sites of the member patch cells, then the ring points of today's
     rule (ring = 1 … 4, ring · patchSize / 4, the 8 directions)

  River nodes inside a lake are left out. Within groups 1 and 2: a mouth node first, then the
  larger flow, then the lower index.

for each group in order:
    for r in (c, ¾·c, ½·c):
        the first candidate of the group with clear(candidate, r) is the spot

if no candidate is clear: the spot of today's rule
```

On a river this gives the most downstream node that is `c` from the border: the Prospect moves
about 2 km upstream from where the river leaves the patch. The rule is a pure function of the
patch and uses no random numbers.

## Expected result

Prototype of the rule on the same Prospects (selection not re-run):

| | `cellgen` now | `cellgen` new | `atlas` now | `atlas` new |
|---|---|---|---|---|
| Smallest distance to a land patch border | 100 m | 1.0 km | 100 m | 1.0 km |
| Median distance | 600 m | 2.3 km | 600 m | 2.3 km |
| Under 1.5 km | 77 % | 11 % | over 75 % | 8 % |
| Prospects on a river, of the patches with one | 142 of 142 | 127 | 146 of 146 | 126 |
| On the navigable river, of the patches with one | 77 of 77 | 64 | 84 of 84 | 68 |
| Within 6 km of the navigable river | 100 % | 99 % | 100 % | 94 % |

Weighed and dropped:

- **Full clearance before river** (a non-river point at 2 km beats a river point at less): 1 %
  under 1.5 km, but only 89 of 142 river Prospects stay on a river.
- **1.5 km clearance:** 135 of 142 stay on a river, but 14 % end up under 1.5 km. At 250 m per
  pixel a city marker is 2.1 km in radius and a town marker 1.2 km.
- **2.5 km clearance:** 118 of 142 stay on a river for little gain in clearance.
- **A display-only fix** (select the nearest Prospect on a click, outline its patch): it helps
  the demo app, but the game client would meet the same problem.

## Steps

1. `docs/map-generation.md`: replace the Spot bullet of 15.1 with the rule above; add
   "Border clearance | 2 km" to the parameter table of section 15; change the river landing
   of 16.2 to ±3 cells.
2. `src/Gameworld.Domain/ParameterSet.cs`: add `BorderClearanceKm` to `PlanningParameters`
   with a `[Description]` and `[Range(0, double.MaxValue)]`. Add it to
   `docs/default-parameter-set.json`. Regenerate `docs/parameter-set.schema.json`.
3. `src/Gameworld.Generation/Stages/PlanBuilder.cs`: rewrite `Spot`.
4. `src/Gameworld.Generation/Travel/TravelBuilder.cs`: the landing search uses a reach of 3.
5. `tests/Gameworld.Tests/GenerationTests.cs`, in
   `Prospects_keep_their_spacing_and_stay_off_the_region_border`: assert that every Prospect
   is clear at ½ · c by the rule's own test, or is the fallback of today's rule.
6. `docs/map-generation-research.md` section 10.6: update the "Spot" row and say why (the
   largest flow is at the river's exit from the patch).
7. Re-measure the shares of 15.6 (the median of 13 of the 20 on show, the regions with 1
   and with all, the Rich and Poor shares) and the parameter descriptions of
   `OneOnShowScoreSum`, `AllOnShowScoreSum`, `RichScore` and `PoorScore`. Spacing is measured between spots and prosperity is sampled at the
   spot, so the selection shifts a little.

## Done when

- `dotnet test -c Release` passes, with the new assertion.
- On `cellgen` and `atlas`, 3 × 3 regions: no Prospect is under 1 km from another land patch.
- The number of river routes on the same maps is not lower than before the change.
- `out/map.png` regenerated and looked at: no marker sits on a patch border.
