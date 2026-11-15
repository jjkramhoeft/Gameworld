using Gameworld.Domain;
using Gameworld.Generation.Biomes;
using Gameworld.Generation.Cells;
using Gameworld.Generation.Lakes;
using Gameworld.Generation.Resources;
using Gameworld.Generation.Rivers;

namespace Gameworld.Generation.Stages;

/// <summary>A patch of a Detailed region with everything the later layers read from it.</summary>
public sealed class PatchDetail
{
    public required PatchShape Shape { get; init; }
    public GridCoord Root => Shape.Root;
    /// <summary>The elevation type of the patch's zone; Ocean for an ocean patch.</summary>
    public required ElevationType ZoneType { get; init; }
    public required string? Bedrock { get; init; }
    /// <summary>The patch lies in a river-poor flat zone.</summary>
    public required bool RiverPoor { get; init; }
    /// <summary>The Initial Temperature field at the patch's site.</summary>
    public required double SeaLevelTemperatureC { get; init; }
    /// <summary>The Precipitation field at the patch's site.</summary>
    public required double FieldPrecipitationMm { get; init; }
    /// <summary>The Prosperity field at the patch's site, rescaled to 0–1.</summary>
    public required double Prosperity { get; init; }
    /// <summary>The Supernatural field at the patch's site, 0–100.</summary>
    public required double Supernatural { get; init; }
    /// <summary>The estimated biome (typical height of the elevation type); null for an ocean patch.</summary>
    public BiomeRecord? Biome { get; init; }
    /// <summary>The biome computed without height cooling is hot or cold desert.</summary>
    public bool DesertAtSeaLevel { get; init; }
    public WaterSite Water { get; set; }
    /// <summary>The counting river nodes on the patch, in index order.</summary>
    public List<int> RiverNodes { get; } = new();
    /// <summary>The estimated resources.</summary>
    public PatchResources Resources { get; set; } = PatchResources.None;

    /// <summary>A patch may not hold a lake if it is ocean, lies in a mountains zone, is a desert at sea level or lies in a river-poor flat zone.</summary>
    public bool MayHoldLake =>
        !Shape.Ocean && ZoneType != ElevationType.Mountains && !DesertAtSeaLevel && !RiverPoor;
}

/// <summary>The working data of a Detailed region: what the Detailed stage adds, indexed for the later layers.</summary>
public sealed class DetailWork
{
    private readonly Dictionary<long, PatchDetail> _byRoot;

    public DetailWork(GridCoord region, PatchDetail[] patches, RiverResult rivers, CellInfo[] nodeInfo, List<LakeShape> lakes)
    {
        Region = region;
        Patches = patches;
        Rivers = rivers;
        NodeInfo = nodeInfo;
        Lakes = lakes;
        _byRoot = patches.ToDictionary(p => p.Root.Key);
    }

    public GridCoord Region { get; }
    /// <summary>All patches of the region, land and ocean, sorted by key.</summary>
    public PatchDetail[] Patches { get; }
    public RiverResult Rivers { get; }
    /// <summary>The classification of every river node.</summary>
    public CellInfo[] NodeInfo { get; }
    public List<LakeShape> Lakes { get; }

    public PatchDetail? Find(GridCoord root) => _byRoot.GetValueOrDefault(root.Key);

    /// <summary>A river node that counts for the patches of this region: not a root, not over the sea, on land of this region.</summary>
    public bool Counts(int node)
    {
        var n = Rivers.Nodes[node];
        return !n.IsRoot && !n.Wet && NodeInfo[node].Region == Region;
    }

    public bool InLake(double x, double y)
    {
        foreach (var lake in Lakes)
            if (lake.Contains(x, y)) return true;
        return false;
    }

    public RegionDetail ToDomain() => new(
        Patches.Select(p => new Patch(
            new PatchId(new PatchCellId(p.Root)),
            new ZoneId(new ZoneCellId(p.Shape.Zone)),
            p.Shape.Site,
            p.Shape.Cells.Select(c => new PatchCellId(c)).ToArray(),
            p.Shape.Ocean,
            p.Shape.Coastal,
            p.Biome,
            p.Resources)).ToArray(),
        Rivers.Rivers,
        Lakes.Select(l => l.ToDomain()).ToArray());
}

/// <summary>
/// The Detailed stage: patch mergers, rivers, lakes and the estimated biomes and resources
/// (map-generation.md sections 6.3, 10, 11, 13 and 14).
/// </summary>
internal static class DetailBuilder
{
    public static DetailWork Build(World world, GridCoord region)
    {
        var cells = world.Cells;
        var p = world.Parameters;
        var seed = world.Foundation.Seed.Value;
        var biomes = world.BiomeRules;

        // Patches with their climate and estimated biome.
        var shapes = cells.PatchesOf(region);
        var patches = new PatchDetail[shapes.Patches.Length];
        for (var i = 0; i < patches.Length; i++)
        {
            var shape = shapes.Patches[i];
            var t0 = world.Fields.InitialTemperature(shape.Site);
            var p0 = world.Fields.Precipitation(shape.Site);
            var prosperity = world.Fields.Sample01(FieldKind.Prosperity, shape.Site);
            var supernatural = world.Fields.Sample(FieldKind.Supernatural, shape.Site);
            if (shape.Ocean)
            {
                patches[i] = new PatchDetail
                {
                    Shape = shape,
                    ZoneType = ElevationType.Ocean,
                    Bedrock = null,
                    RiverPoor = false,
                    SeaLevelTemperatureC = t0,
                    FieldPrecipitationMm = p0,
                    Prosperity = prosperity,
                    Supernatural = supernatural,
                };
                continue;
            }

            var type = cells.ZoneType(shape.Zone);
            var riverPoor = world.IsRiverPoorZone(shape.Zone);
            var seaLevel = biomes.Classify(t0, p0, 0, type, shape.Coastal, riverPoor);
            patches[i] = new PatchDetail
            {
                Shape = shape,
                ZoneType = type,
                Bedrock = cells.ZoneBedrock(shape.Zone),
                RiverPoor = riverPoor,
                SeaLevelTemperatureC = t0,
                FieldPrecipitationMm = p0,
                Prosperity = prosperity,
                Supernatural = supernatural,
                Biome = biomes.Classify(t0, p0, BiomeRules.TypicalHeightM(type), type, shape.Coastal, riverPoor),
                DesertAtSeaLevel = seaLevel.Biome is Biome.HotDesert or Biome.ColdDesert,
            };
        }
        var byRoot = patches.ToDictionary(x => x.Root.Key);

        // Rivers, then lakes on them.
        var rivers = RiverBuilder.Build(cells, region, seed, p.Detail.Rivers, world.IsRiverPoorZone);
        var nodes = rivers.Nodes;
        var nodeInfo = new CellInfo[nodes.Count];
        for (var k = 0; k < nodes.Count; k++) nodeInfo[k] = cells.CellAt(nodes[k].Position);

        var lakes = LakeBuilder.Build(cells, region, seed, p.Detail.Lakes, nodes,
            root => byRoot.TryGetValue(root.Key, out var pd) && pd.MayHoldLake);

        var work = new DetailWork(region, patches, rivers, nodeInfo, lakes);
        WaterSites(world, work);

        // Estimated resources.
        foreach (var patch in patches)
            patch.Resources = ResourcesOf(world, patch, patch.Biome);
        return work;
    }

    /// <summary>The resources of a patch for a biome record (estimated or final).</summary>
    public static PatchResources ResourcesOf(World world, PatchDetail patch, BiomeRecord? biome)
    {
        var seed = world.Foundation.Seed.Value;
        if (patch.Shape.Ocean)
            return world.OceanResources(new OceanResourceInput(
                seed, patch.Root, patch.Shape.Coastal, patch.SeaLevelTemperatureC, patch.Prosperity));
        return world.LandResources(new LandResourceInput(
            seed, patch.Root, patch.Shape.Zone, biome!, patch.ZoneType, patch.Bedrock,
            patch.Shape.Coastal, patch.RiverPoor, patch.Water, patch.Prosperity, patch.Supernatural));
    }

    /// <summary>The water site of every land patch (section 14.1).</summary>
    private static void WaterSites(World world, DetailWork work)
    {
        var cells = world.Cells;
        var nodes = work.Rivers.Nodes;
        var perPatch = (double)Math.Max(1, world.Parameters.Detail.Rivers.AttractorsPerPatchCell);

        // What every node carries down: the land nodes at and upstream of it, and how many of
        // them lie in hills or mountains zones on gold-bearing bedrock and on granite.
        var land = new int[nodes.Count];
        var gold = new int[nodes.Count];
        var tin = new int[nodes.Count];
        for (var k = 0; k < nodes.Count; k++)
        {
            if (nodes[k].Wet) continue;
            land[k] = 1;
            var zone = work.NodeInfo[k].Zone;
            if (cells.ZoneType(zone) is not (ElevationType.Hills or ElevationType.Mountains)) continue;
            switch (cells.ZoneBedrock(zone))
            {
                case BedrockIds.FoldBelt or BedrockIds.Volcanic or BedrockIds.Shield:
                    gold[k] = 1;
                    break;
                case BedrockIds.Granite:
                    tin[k] = 1;
                    break;
            }
        }
        for (var k = nodes.Count - 1; k >= 0; k--)
        {
            var parent = nodes[k].Parent;
            if (parent < 0) continue;
            land[parent] += land[k];
            gold[parent] += gold[k];
            tin[parent] += tin[k];
        }

        for (var k = 0; k < nodes.Count; k++)
            if (work.Counts(k) && work.Find(work.NodeInfo[k].Patch) is { } patch)
                patch.RiverNodes.Add(k);

        foreach (var patch in work.Patches)
        {
            if (patch.Shape.Ocean) continue;
            int largest = -1, river = 0, mouth = 0;
            foreach (var k in patch.RiverNodes)
            {
                var n = nodes[k];
                if (n.Flow > river)
                {
                    river = n.Flow;
                    largest = k;
                }
                var parent = nodes[n.Parent];
                if ((parent.IsRoot || parent.Wet) && n.Flow > mouth) mouth = n.Flow;
            }
            var lakeArea = 0.0;
            foreach (var lake in work.Lakes)
                if (lake.Patch == patch.Root && lake.AreaM2 > lakeArea) lakeArea = lake.AreaM2;

            patch.Water = new WaterSite(
                river / perPatch,
                mouth / perPatch,
                lakeArea,
                cells.HasHarbour(patch.Shape),
                largest >= 0 && land[largest] > 0 ? (double)gold[largest] / land[largest] : 0,
                largest >= 0 && land[largest] > 0 ? (double)tin[largest] / land[largest] : 0);
        }
    }
}
