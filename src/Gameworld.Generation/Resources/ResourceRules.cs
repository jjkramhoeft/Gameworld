using Gameworld.Domain;
using Gameworld.Generation.Basics;

namespace Gameworld.Generation.Resources;

/// <summary>
/// The patch resource rules (map-generation.md section 14 and Appendices B–D): an amount in
/// [0, 1] and a named variant for each of the 24 raw resources. A pure function of its input.
/// </summary>
public static class ResourceRules
{
    /// <summary>The resources of a land patch (sections 14.1–14.5, Appendix D.1 and D.2).</summary>
    public static PatchResources Land(in LandResourceInput input) => new LandRules(input).Run();

    /// <summary>The resources of an ocean patch (Appendix D.3).</summary>
    public static PatchResources Ocean(in OceanResourceInput input)
    {
        var boost = Boost(input.Prosperity);
        var ledger = new ResourceLedger(input.Seed, input.Patch, boost);
        var t0 = input.SeaLevelTemperatureC;

        if (input.OnShelf)
        {
            ledger.Put(Resource.Fish, 0.9,
                t0 < 4 ? "cod banks" : t0 < 11 ? "herring" : t0 < 18 ? "sardine and anchovy" : "reef fish and shellfish");
            if (t0 < 12) ledger.Put(Resource.WildFoodAndGame, 0.3, "edible seaweed and kelp");
            if (t0 < 3 && OceanFurGate(input, boost)) ledger.Put(Resource.FursAndIvory, 0.4, "seals");
            // The "(ocean patch)" of the D.2 table marks the kind, not the variant name.
            if (t0 >= 20) ledger.Deposit(50, Resource.GemsAndPearls, "pearl oyster", 0.05);
            else if (t0 >= 11) ledger.Deposit(51, Resource.GemsAndPearls, "precious red coral", 0.03);
        }
        else
        {
            ledger.Put(Resource.Fish, 0.5,
                t0 < 8 ? "cod banks" : t0 < 16 ? "herring" : "tuna and large pelagics");
            if (t0 < 6 && OceanFurGate(input, boost)) ledger.Put(Resource.FursAndIvory, 0.3, "whales");
        }

        return ledger.Build();
    }

    /// <summary>The prosperity boost (14.1): (2 · prosperity)², ¼ in poor land, 1 in the middle, 4 at the top.</summary>
    public static double Boost(double prosperity)
    {
        var b = 2 * prosperity;
        return b * b;
    }

    /// <summary>The trade value of a resource in the wealth summary (14.5).</summary>
    public static double TradeValue(Resource resource) => resource switch
    {
        Resource.Gold => 1,
        Resource.Tin => 0.9,
        Resource.SilverAndLead => 0.9,
        Resource.GemsAndPearls => 0.8,
        Resource.Salt => 0.7,
        Resource.Spices => 0.7,
        Resource.CopperOre => 0.6,
        Resource.IronOre => 0.5,
        Resource.Crystal => 0.4,
        Resource.StimulantsAndSugar => 0.4,
        Resource.FursAndIvory => 0.35,
        Resource.Dyes => 0.3,
        Resource.HorsesAndPackAnimals => 0.2,
        Resource.Orchard => 0.1,
        Resource.Fibre => 0.1,
        _ => 0,
    };

    /// <summary>The fur gate of an ocean patch (D.3): hashed on the patch, not the zone.</summary>
    private static bool OceanFurGate(in OceanResourceInput input, double boost) =>
        Hashing.Unit(input.Seed, 0x5e05, input.Patch.I, input.Patch.J) < 0.5 * boost;
}

/// <summary>
/// The amounts of one patch while the rules run: <c>put</c>, <c>pick</c> and the deposit roll
/// of sections 14.1 and 14.4, and the two summaries of 14.5.
/// </summary>
internal sealed class ResourceLedger(uint seed, GridCoord patch, double boost)
{
    private readonly ResourceAmount[] _amounts = new ResourceAmount[ResourceCatalog.Count];

    public double this[Resource resource] => _amounts[(int)resource].Amount;

    /// <summary>
    /// put (14.1): the amount is clamped to [0, 1] and replaces the current amount and variant
    /// only if it is strictly larger, so on a tie the earlier rule wins.
    /// </summary>
    public void Put(Resource resource, double amount, string variant, ResourceOrigin origin = ResourceOrigin.Potential)
    {
        amount = Math.Clamp(amount, 0, 1);
        if (amount > _amounts[(int)resource].Amount)
            _amounts[(int)resource] = new ResourceAmount(amount, variant, origin);
    }

    /// <summary>pick (Appendix D): one name by hash4(seed, 0x5e10 + resource index, patch) mod count.</summary>
    public string Pick(Resource resource, params ReadOnlySpan<string> names)
    {
        var h = Hashing.Hash4(seed, 0x5e10u + (uint)resource, patch.I, patch.J);
        return names[(int)(h % (uint)names.Length)];
    }

    /// <summary>
    /// A deposit kind (14.4): present when the patch's hash of the kind is below the chance
    /// (× boost for the valuables), with richness rich · (0.35 + 0.65 · hash).
    /// </summary>
    public void Deposit(int kind, Resource resource, string variant, double chance, double rich = 1)
    {
        if (chance <= 0) return;
        if (IsBoosted(resource)) chance *= boost;
        if (!(Hashing.Unit(seed, 0x5e400u + (uint)kind, patch.I, patch.J) < chance)) return;
        var richness = rich * (0.35 + 0.65 * Hashing.Unit(seed, 0x5e800u + (uint)kind, patch.I, patch.J));
        Put(resource, richness, variant, ResourceOrigin.Deposit);
    }

    /// <summary>D.2: the chance is multiplied by the prosperity boost for silver &amp; lead, gold, gems, furs and dyes.</summary>
    private static bool IsBoosted(Resource resource) =>
        resource is Resource.SilverAndLead or Resource.Gold or Resource.GemsAndPearls
            or Resource.FursAndIvory or Resource.Dyes;

    /// <summary>The amounts with the two summaries of 14.5.</summary>
    public PatchResources Build()
    {
        var food = Math.Clamp(
            0.75 * Math.Max(this[Resource.Grain], 0.8 * this[Resource.Produce])
            + 0.2 * this[Resource.Livestock]
            + 0.1 * this[Resource.Orchard]
            + 0.05 * this[Resource.WildFoodAndGame],
            0, 1);

        var wealth = 0.0;
        for (var r = 0; r < _amounts.Length; r++)
            if (_amounts[r].Amount >= ResourceCatalog.ListedAmount)
                wealth += ResourceRules.TradeValue((Resource)r) * _amounts[r].Amount;

        return new PatchResources(_amounts, food, wealth);
    }
}

/// <summary>One run of the land rules over one patch: the inputs and derived values of 14.1, then the rules of Appendix D in order.</summary>
internal sealed class LandRules
{
    private readonly ResourceLedger _out;
    private readonly uint _seed;
    private readonly GridCoord _zone;

    // Biome record (section 13).
    private readonly Biome _biome;
    private readonly ThermalBelt _belt;
    private readonly double _t;   // T, after cooling
    private readonly double _t0;  // T₀, at sea level
    private readonly double _p;   // P, mm
    private readonly double _m;   // aridity
    private readonly bool _humid; // m ≥ 2
    private readonly BiomeFactorRow _f;

    // Elevation type.
    private readonly ElevationType _zt;
    private readonly bool _flat, _lowland, _hills, _mountains, _low, _high;

    // Bedrock.
    private readonly bool _shield, _foldBelt, _granite, _volcanic, _basalt, _ophiolite, _carbonate, _clastic, _cover;

    // Water site and flags.
    private readonly double _river, _mouth, _lakeAreaM2, _goldLoad, _tinLoad;
    private readonly bool _lake, _harbour, _coastal, _riverPoor;

    // Derived values (14.1).
    private readonly double _boost, _tg, _season, _irr;
    private readonly bool _tropicalHighland, _floodplain;

    public LandRules(in LandResourceInput input)
    {
        if (input.ZoneType == ElevationType.Ocean)
            throw new ArgumentException("A land patch needs a land elevation type.", nameof(input));

        _seed = input.Seed;
        _zone = input.Zone;
        _boost = ResourceRules.Boost(input.Prosperity);
        _out = new ResourceLedger(input.Seed, input.Patch, _boost);

        var biome = input.Biome;
        _biome = biome.Biome;
        _belt = biome.Belt;
        _t = biome.TemperatureC;
        _t0 = biome.SeaLevelTemperatureC;
        _p = biome.PrecipitationMm;
        _m = biome.Aridity;
        _humid = _m >= 2;
        _f = BiomeFactors.Of(_biome);

        _zt = input.ZoneType;
        _flat = _zt == ElevationType.FlatLand;
        _lowland = _zt == ElevationType.Lowland;
        _hills = _zt == ElevationType.Hills;
        _mountains = _zt == ElevationType.Mountains;
        _low = _flat || _lowland;
        _high = _hills || _mountains;

        // A land zone always has a bedrock; without one no rule that names a bedrock matches, so there is no stone either.
        var bedrock = input.Bedrock;
        _shield = bedrock == BedrockIds.Shield;
        _foldBelt = bedrock == BedrockIds.FoldBelt;
        _granite = bedrock == BedrockIds.Granite;
        _volcanic = bedrock == BedrockIds.Volcanic;
        _basalt = bedrock == BedrockIds.Basalt;
        _ophiolite = bedrock == BedrockIds.Ophiolite;
        _carbonate = bedrock == BedrockIds.Carbonate;
        _clastic = bedrock == BedrockIds.Clastic;
        _cover = bedrock == BedrockIds.Alluvium;

        var water = input.Water;
        _river = water.River;
        _mouth = water.Mouth;
        _lakeAreaM2 = water.LakeAreaM2;
        _lake = water.HasLake;
        _harbour = water.Harbour;
        _goldLoad = water.GoldLoad;
        _tinLoad = water.TinLoad;
        _coastal = input.Coastal;
        _riverPoor = input.RiverPoor;

        _tg = _t + Math.Clamp(0.5 * (24 - _t0), 0, 12);
        _season = Math.Clamp((_t + 4) / 10, 0, 1);
        _irr = (_river > 1 ? 100 * (Math.Min(_river, 13) - 1) * ByZone(1, 1, 0.5, 0.25) : 0) + (_lake ? 200 : 0);
        _tropicalHighland = _t0 >= 18 && _belt == ThermalBelt.Temperate;
        _floodplain = _river >= 1 && _low;
    }

    /// <summary>The rules in the order of Appendix D: fuel reads the timber and livestock amounts.</summary>
    public PatchResources Run()
    {
        Crops(Resource.Grain);
        Crops(Resource.Produce);
        Crops(Resource.Orchard);
        Livestock();
        Fish();
        WildFood();
        Timber();
        Stone();
        Clay();
        Crystal();
        Crops(Resource.Fibre);
        Fuel();
        Salt();
        Iron();
        Copper();
        Tin();
        SilverAndLead();
        Gold();
        Gems();
        Crops(Resource.Spices);
        Crops(Resource.StimulantsAndSugar);
        Honey();
        Furs();
        Dyes();
        Horses();
        return _out.Build();
    }

    private double ByZone(double flat, double lowland, double hills, double mountains) =>
        _mountains ? mountains : _hills ? hills : _lowland ? lowland : flat;

    private bool ZoneGate(uint purpose, double share) =>
        Hashing.Unit(_seed, purpose, _zone.I, _zone.J) < share;

    // ---- 14.2 Crops -------------------------------------------------------------------------

    /// <summary>The resource's amount is the best score of its table; on a tie the crop listed first wins.</summary>
    private void Crops(Resource resource)
    {
        foreach (var crop in CropTables.Of(resource))
        {
            var s = Score(crop);
            if (s > 0) _out.Put(resource, s, crop.Name);
        }
    }

    /// <summary>score(crop) of section 14.2.</summary>
    private double Score(Crop crop)
    {
        var s = crop.Temperature.At(_tg);
        if (s <= 0) return 0;

        var water = _p;
        if (water < crop.Water.OptMin) water = Math.Min(crop.Water.OptMin, water + _irr); // rivers irrigate
        s *= crop.Water.At(water);
        s *= crop.Farmland.Of(_zt);

        // Limits on the yearly mean T: 0 at minT − 2 rising to 1 at minT; 1 at maxT falling to 0 at maxT + 2.
        if (crop.MinT is { } minT) s *= Math.Clamp((_t - (minT - 2)) / 2, 0, 1);
        if (crop.MaxT is { } maxT) s *= Math.Clamp((maxT + 2 - _t) / 2, 0, 1);

        s *= crop.Tree ? _f.TreeGround : _season * _f.Soil;

        switch (crop.Needs)
        {
            // "Has a river" is read as any river node in the patch (river > 0); the doc gives no threshold here.
            case CropNeed.River when !(_river > 0) && !_lake:
                s *= 0.25;
                break;
            case CropNeed.Coast when !_coastal:
            case CropNeed.Highland when !_tropicalHighland:
                return 0;
        }

        s *= crop.K;
        if (s <= 0) return 0;

        if (crop.IsGated)
        {
            // Appendix B: boost = 1 for silk, which is a fibre; the prosperity boost for spices, stimulants and dyes.
            var boost = crop.Resource == Resource.Fibre ? 1 : _boost;
            if (!ZoneGate(0x5e100u + (uint)crop.Number, crop.Gate * boost)) return 0;
        }

        return s;
    }

    // ---- D.1 Potentials ---------------------------------------------------------------------

    private void Livestock()
    {
        var amount = _f.Pasture * ByZone(1, 1, 0.9, 0.7);
        if (_river >= 1 && _belt >= ThermalBelt.Boreal) amount += 0.1;
        if (amount > 0) _out.Put(Resource.Livestock, amount, LivestockVariant());
    }

    private string LivestockVariant()
    {
        const Resource r = Resource.Livestock;
        if (_mountains && _t < 5 && _biome != Biome.Tundra) return "yak";
        if (_mountains)
            return _biome is Biome.WoodlandAndShrubland or Biome.ThornScrub ? "goats" : "alpine summer pasture";
        return _biome switch
        {
            Biome.Tundra or Biome.BorealForest => "herded reindeer",
            Biome.ColdSteppe or Biome.ColdDesert => "sheep",
            Biome.TemperateGrassland => _out.Pick(r, "cattle", "sheep"),
            Biome.WoodlandAndShrubland => _hills ? "goats" : "sheep",
            Biome.TemperateBroadleafForest => _out.Pick(r, "pigs", "cattle"),
            Biome.TemperateMixedForest or Biome.TemperateRainForest => _out.Pick(r, "cattle", "sheep"),
            Biome.HotDesert or Biome.ThornScrub => "goats",
            Biome.TropicalDryForest => _floodplain ? "water buffalo" : "cattle",
            Biome.TropicalRainForest or Biome.Mangrove => _out.Pick(r, "pigs", "poultry"),
            _ => "cattle",
        };
    }

    /// <summary>Fish: all that apply are put.</summary>
    private void Fish()
    {
        const Resource r = Resource.Fish;

        if (_coastal)
        {
            if (_biome == Biome.Mangrove)
            {
                _out.Put(r, 0.9, "crabs, lobsters and shrimp");
            }
            else if (_mouth >= 2 && _t0 < 10)
            {
                _out.Put(r, _harbour ? 1 : 0.85, "salmon runs");
            }
            else
            {
                // Read in the order written: the polar 0.6 comes before the harbour's 1.
                var amount = _belt == ThermalBelt.Polar ? 0.6 : _harbour ? 1 : 0.8;
                var variant =
                    _t0 < 4 ? "cod banks"
                    : _t0 < 11 ? "herring"
                    : _t0 < 18 ? _out.Pick(r, "sardine and anchovy", "oysters and mussels")
                    : _out.Pick(r, "reef fish and shellfish", "tuna and large pelagics");
                _out.Put(r, amount, variant);
            }
        }

        if (_river > 0 && _belt != ThermalBelt.Polar)
        {
            var amount = _river >= 80 ? 0.7 : _river >= 8 ? 0.6 : _river >= 2 ? 0.3 : 0.15;
            var variant =
                _river >= 80 && _belt <= ThermalBelt.Temperate ? "sturgeon"
                : _river >= 8 && _belt == ThermalBelt.Temperate ? _out.Pick(r, "river fish", "eels")
                : "river fish";
            _out.Put(r, amount, variant);
        }

        if (_lake)
            _out.Put(r, 0.45 + 0.3 * Math.Clamp(_lakeAreaM2 / 20e6, 0, 1), "lake fish"); // 20 km²
    }

    private void WildFood()
    {
        var amount = _f.Game;
        if ((_lake || _floodplain) && _belt != ThermalBelt.Polar) amount += 0.15;
        if (_biome == Biome.IceAndSnow && !_coastal) amount = 0;
        if (amount > 0) _out.Put(Resource.WildFoodAndGame, amount, WildFoodVariant());
    }

    private string WildFoodVariant()
    {
        const Resource r = Resource.WildFoodAndGame;
        if (_mountains && _biome is not (Biome.IceAndSnow or Biome.Tundra)) return "wild sheep and ibex";
        if (_lake && _belt == ThermalBelt.Temperate) return "waterfowl";
        if (_lake && _belt == ThermalBelt.Tropical) return "lotus";
        return _biome switch
        {
            Biome.IceAndSnow => "seals",
            Biome.Tundra => _out.Pick(r, "caribou", "cloudberry and crowberry"),
            Biome.BorealForest => _out.Pick(r, "deer and elk", "bilberry and blueberry", "forest mushrooms"),
            Biome.TemperateMixedForest => _out.Pick(r, "deer", "wild boar", "forest mushrooms"),
            Biome.TemperateBroadleafForest => _out.Pick(r, "deer", "wild boar", "acorns"),
            Biome.TemperateRainForest => _out.Pick(r, "deer", "raspberry and bramble"),
            Biome.TemperateGrassland => _out.Pick(r, "bison and wild cattle", "hares and rabbits"),
            Biome.ColdSteppe => _out.Pick(r, "antelope and gazelle", "upland game birds"),
            Biome.WoodlandAndShrubland => _out.Pick(r, "upland game birds", "wild herbs", "hares and rabbits"),
            Biome.Mangrove => "waterfowl",
            Biome.ColdDesert => _out.Pick(r, "hares and rabbits", "upland game birds"),
            Biome.TropicalRainForest or Biome.TropicalDryForest => _out.Pick(r, "wild boar", "deer", "wild herbs"),
            _ => "antelope and gazelle",
        };
    }

    private void Timber()
    {
        var amount = _f.Wood;
        switch (_biome)
        {
            case Biome.TemperateGrassland or Biome.ColdSteppe:
                if (_river >= 1) amount += 0.2;
                else if (_high) amount += 0.07;
                break;
            case Biome.HotDesert or Biome.ColdDesert:
                amount = _river >= 2 ? 0.3 : 0;
                break;
            case Biome.Tundra:
                amount = _coastal ? 0.1 : 0;
                break;
        }
        if (_mountains && amount > 0.6) amount *= 0.8;
        if (amount > 0) _out.Put(Resource.Timber, amount, TimberVariant());
    }

    private string TimberVariant()
    {
        const Resource r = Resource.Timber;
        switch (_biome)
        {
            case Biome.BorealForest:
                return _high ? _out.Pick(r, "larch", "spruce") : _out.Pick(r, "Scots pine", "spruce");
            case Biome.TemperateMixedForest:
                return _mountains ? "fir"
                    : _hills ? _out.Pick(r, "beech", "spruce")
                    : _out.Pick(r, "oak", "Scots pine", "lime");
            case Biome.TemperateBroadleafForest:
                if (_mountains) return _t >= 12 ? "cedar" : "beech";
                if (_hills) return _out.Pick(r, "beech", "oak");
                return _floodplain ? _out.Pick(r, "oak", "ash", "elm") : "oak";
            case Biome.TemperateRainForest:
                return "giant conifers";
            case Biome.WoodlandAndShrubland:
                if (_t < 10) return _high ? "juniper" : _out.Pick(r, "oak", "Scots pine", "juniper");
                return _high
                    ? _out.Pick(r, "cypress", "Mediterranean pines")
                    : _out.Pick(r, "cork oak", "Mediterranean pines");
            case Biome.TropicalDryForest:
                return _p >= 1500
                    ? _out.Pick(r, "teak", "sal")
                    : _out.Pick(r, "teak", "Spanish cedar", "rosewood");
            case Biome.TropicalRainForest:
                return _out.Pick(r, "mahogany", "iroko", "rosewood", "ebony", "mahogany", "iroko");
            case Biome.Savanna:
                return _river >= 1 ? "African mahogany" : "thorn-scrub wood";
            case Biome.ThornScrub:
                return "thorn-scrub wood";
            case Biome.Mangrove:
                return "mangrove timber";
            case Biome.HotDesert or Biome.ColdDesert:
                return "poplar";
            case Biome.TemperateGrassland or Biome.ColdSteppe:
                return _river >= 1 ? "willow and poplar" : "juniper";
            default: // tundra and ice
                return "driftwood";
        }
    }

    /// <summary>Stone is deposit kind 1 on any land patch: the chance and richness by elevation type, the factor and variant by bedrock.</summary>
    private void Stone()
    {
        const Resource r = Resource.Stone;
        double factor;
        string variant;
        if (_shield || _granite) (factor, variant) = (1, "granite");
        else if (_foldBelt)
            (factor, variant) = (1, _mountains ? _out.Pick(r, "slate", "slate", "marble") : _out.Pick(r, "slate", "whetstone", "slate"));
        else if (_volcanic) (factor, variant) = (1, _out.Pick(r, "basalt", "pumice and pozzolana", "obsidian", "basalt"));
        else if (_basalt) (factor, variant) = (2, _out.Pick(r, "basalt", "basalt", "millstone and quern stone"));
        else if (_ophiolite) (factor, variant) = (1, "soapstone");
        else if (_carbonate)
            (factor, variant) = (2.5, _low ? _out.Pick(r, "chalk", "limestone", "flint and chert") : _out.Pick(r, "limestone", "limestone", "marble"));
        else if (_clastic) (factor, variant) = (1.6, _out.Pick(r, "sandstone", "sandstone", "millstone and quern stone"));
        else if (_cover) (factor, variant) = (0.5, "field stone and boulders");
        else return;

        _out.Deposit(1, r, variant, ByZone(0.04, 0.12, 0.75, 0.95) * factor, ByZone(0.35, 0.5, 0.85, 1));
    }

    private void Clay()
    {
        const Resource r = Resource.Clay;
        if (_biome != Biome.IceAndSnow)
        {
            var amount = ByZone(0.7, 0.6, 0.25, 0.1);
            if (_river >= 1 && _low) amount += 0.3;
            if (_cover) amount += 0.1;
            if (_biome == Biome.Tundra) amount *= 0.3; // read as scaling the whole sum
            var dry = _m < 1;
            var variant =
                dry && _belt is ThermalBelt.Temperate or ThermalBelt.Tropical ? "adobe and mud-brick earth"
                : _river >= 1 ? "brick and pottery clay"
                : _biome is Biome.TemperateMixedForest or Biome.TemperateBroadleafForest ? "wattle and daub"
                : dry || _biome == Biome.WoodlandAndShrubland ? "rammed earth and cob"
                : "brick and pottery clay";
            _out.Put(r, amount, variant);
        }

        _out.Deposit(2, r, "kaolin", (_granite || _shield) && _t >= 10 && _humid && !_mountains ? 0.05 : 0, 1.2);
    }

    private void Crystal()
    {
        const Resource r = Resource.Crystal;
        if (_high)
        {
            var chance = _mountains ? 0.05 : 0.03;
            if (_volcanic) chance *= 3;
            else if (_granite || _foldBelt) chance *= 2;
            _out.Deposit(3, r, "rock crystal", chance);
        }
        else
        {
            _out.Deposit(4, r, "glass sand", (_coastal ? 0.03 : 0) + (_river >= 8 ? 0.015 : 0), 0.8);
        }
    }

    /// <summary>Fuel: all that apply are put, then the coal deposits.</summary>
    private void Fuel()
    {
        const Resource r = Resource.Fuel;

        var timber = _out[Resource.Timber];
        if (timber > 0)
        {
            var variant = _biome == Biome.Mangrove ? "charcoal"
                : timber >= 0.7 ? _out.Pick(r, "firewood", "charcoal")
                : "firewood";
            _out.Put(r, 0.9 * timber, variant);
        }

        if (_low && _humid && _t < 10 && _belt >= ThermalBelt.Boreal) _out.Put(r, 0.6, "peat");
        if (_biome == Biome.Tundra) _out.Put(r, _low ? 0.35 : 0.2, "turf and sod");

        var livestock = _out[Resource.Livestock];
        if (livestock >= 0.15) _out.Put(r, 0.35 * livestock, "dung fuel");

        if (_clastic)
        {
            _out.Deposit(5, r, "coal", _hills || _lowland ? 0.04 : 0);
            _out.Deposit(6, r, "lignite", _low ? 0.04 : 0, 0.7);
            _out.Deposit(7, r, "oil and naphtha seeps", _low ? 0.01 : 0, 0.8);
        }
    }

    private void Salt()
    {
        const Resource r = Resource.Salt;

        _out.Deposit(8, r, "solar sea salt", _coastal && _low && _m < 1 && _t >= 12 ? 0.25 : 0);

        // "River-poor flat zone" is the input's river-poor flag, which is only set in flat zones.
        if (_riverPoor && _m < 0.5 && ZoneGate(0x5e02, 0.08))
        {
            _out.Deposit(9, r, "lake and playa salt", 0.3);
            _out.Deposit(10, r, "natron and soda", 0.05, 0.8);
        }

        if (_clastic && !_mountains && ZoneGate(0x5e03, 0.05))
        {
            _out.Deposit(11, r, "rock salt", 0.12);
            _out.Deposit(12, r, "brine springs", 0.08, 0.8);
        }
    }

    // ---- D.2 Deposits: metals and gems ------------------------------------------------------

    private void Iron()
    {
        const Resource r = Resource.IronOre;

        var bogForest = _biome is Biome.BorealForest or Biome.TemperateMixedForest
            or Biome.TemperateBroadleafForest or Biome.TemperateRainForest;
        if (_low && _humid && bogForest)
            _out.Deposit(13, r, "bog iron", _river >= 1 || _lake ? 0.1 : 0.05, 0.7);

        if (!_mountains && _biome is Biome.TropicalRainForest or Biome.TropicalDryForest or Biome.Savanna)
            _out.Deposit(14, r, "lateritic iron and ironstone", 0.1, 0.8);

        if (_shield && (_hills || _lowland)) _out.Deposit(15, r, "banded iron formation", 0.08);
        if ((_granite || _carbonate) && _high) _out.Deposit(16, r, "magnetite skarn iron", 0.06);
        if (_coastal && _basalt) _out.Deposit(17, r, "black sands", 0.05, 0.6);
    }

    private void Copper()
    {
        const Resource r = Resource.CopperOre;

        if (_volcanic && _high)
            _out.Deposit(18, r, _out.Pick(r, "porphyry copper", "malachite and azurite", "porphyry copper"),
                _mountains ? 0.1 : 0.05);

        if (_clastic && (_lowland || _hills)) _out.Deposit(19, r, "red-bed copper", 0.03, 0.8);
        if (_ophiolite && _high) _out.Deposit(20, r, "massive-sulfide copper", 0.08);
        if (_basalt && !_flat) _out.Deposit(21, r, "native copper", _lowland ? 0.02 : 0.06, 0.8);
    }

    private void Tin()
    {
        const Resource r = Resource.Tin;

        if (_granite && _high) _out.Deposit(22, r, "tin lode", 0.06);

        var copperBedrock = _volcanic || _basalt || _ophiolite || _clastic;
        if (!copperBedrock && !_mountains && _river >= 1)
            _out.Deposit(23, r, "stream tin", 0.35 * _tinLoad, 0.4 + 0.6 * _tinLoad);
    }

    private void SilverAndLead()
    {
        const Resource r = Resource.SilverAndLead;

        if (_foldBelt && _high) _out.Deposit(24, r, "silver", 0.05);

        if (_carbonate && (_hills || _lowland))
            _out.Deposit(25, r, _out.Pick(r, "lead-zinc in carbonates", "calamine", "lead-zinc in carbonates"),
                _hills ? 0.06 : 0.025, 0.8);

        if (_volcanic && _mountains) _out.Deposit(26, r, "lead-zinc skarns and veins", 0.03);
    }

    private void Gold()
    {
        const Resource r = Resource.Gold;

        if (_foldBelt && _high) _out.Deposit(27, r, "lode gold", 0.03);
        if (_shield && _hills) _out.Deposit(28, r, "lode gold", 0.03);
        if (_volcanic && _mountains) _out.Deposit(29, r, "epithermal and hot-spring gold-silver", 0.03);
        if (!_mountains && _river >= 1) _out.Deposit(30, r, "placer gold", 0.2 * _goldLoad, 0.4 + 0.6 * _goldLoad);
        if (_coastal && _mouth >= 2) _out.Deposit(31, r, "beach gold", 0.1 * _goldLoad, 0.6);
    }

    private void Gems()
    {
        const Resource r = Resource.GemsAndPearls;
        var dryland = _biome is Biome.HotDesert or Biome.ColdDesert or Biome.ThornScrub;
        var borealOrWarmer = _belt >= ThermalBelt.Boreal;

        if (_shield && !_mountains) _out.Deposit(32, r, "diamond", 0.004);
        if (_granite && _high) _out.Deposit(33, r, "topaz, beryl and tourmaline", 0.04);
        if (_foldBelt && _high) _out.Deposit(34, r, "garnet", 0.03, 0.7);
        if (_foldBelt && _high) _out.Deposit(35, r, "emerald", 0.004);
        if ((_volcanic || _basalt) && _high) _out.Deposit(36, r, "amethyst and agate", 0.04, 0.7);
        if ((_basalt || _carbonate) && _mountains) _out.Deposit(37, r, "ruby and sapphire", 0.008);
        if (_ophiolite && _high) _out.Deposit(38, r, "jade", 0.04);
        if (_carbonate && _mountains) _out.Deposit(39, r, "lapis lazuli", 0.012);
        if (dryland && _high && (_volcanic || _clastic)) _out.Deposit(40, r, "turquoise", 0.04);
        if (dryland && (_lowland || _hills) && (_clastic || _basalt)) _out.Deposit(41, r, "opal", 0.012);
        if (_coastal && _t < 12 && borealOrWarmer && (_clastic || _cover)) _out.Deposit(42, r, "amber", 0.03);
        if (_coastal && _clastic && _hills) _out.Deposit(43, r, "jet", 0.02, 0.6);
        if (_coastal && _t0 >= 20) _out.Deposit(44, r, "pearl oyster", 0.04);
        if (_river >= 2 && _t < 9 && borealOrWarmer) _out.Deposit(45, r, "freshwater pearl mussel", 0.006, 0.7);
        if (_coastal && _t0 >= 11 && _t0 < 20 && _high) _out.Deposit(46, r, "precious red coral", 0.03);
    }

    // ---- D.1 Potentials, continued ----------------------------------------------------------

    private void Honey()
    {
        if (_f.Honey > 0) _out.Put(Resource.StimulantsAndSugar, _f.Honey, "honey and beeswax");
    }

    private void Furs()
    {
        const Resource r = Resource.FursAndIvory;

        // The zone gate is read as covering every potential of this rule, the coast seals included; musk deer is a deposit and rolls on its own.
        var gate = _belt == ThermalBelt.Tropical
            ? ZoneGate(0x5e04, 0.3 * _boost)
            : ZoneGate(0x5e05, 0.5 * _boost);

        if (gate)
        {
            var amount = _f.Fur;
            string? variant = null;
            switch (_biome)
            {
                case Biome.BorealForest:
                    variant = _river >= 1
                        ? _out.Pick(r, "beaver", "sable and marten")
                        : _out.Pick(r, "sable and marten", "squirrel", "ermine");
                    break;
                case Biome.Tundra:
                    if (_coastal)
                    {
                        amount += 0.15;
                        variant = _out.Pick(r, "walrus", "seals", "fox and Arctic fox");
                    }
                    else
                    {
                        variant = "fox and Arctic fox";
                    }
                    break;
                case Biome.IceAndSnow:
                    amount = _coastal ? 0.5 : 0;
                    variant = _out.Pick(r, "walrus", "seals", "narwhal tusk");
                    break;
                case Biome.TemperateMixedForest or Biome.TemperateRainForest:
                    variant = _high ? "bear" : _river >= 1 ? "beaver" : _out.Pick(r, "squirrel", "sable and marten");
                    break;
                case Biome.TemperateBroadleafForest or Biome.ColdSteppe:
                    variant = "fox and Arctic fox";
                    break;
                case Biome.Savanna:
                    variant = _out.Pick(r, "elephant ivory", "ostrich", "big cats");
                    break;
                case Biome.ThornScrub:
                    variant = "ostrich";
                    break;
                case Biome.TropicalDryForest:
                    variant = _out.Pick(r, "elephant ivory", "big cats");
                    break;
                case Biome.TropicalRainForest:
                    variant = _out.Pick(r, "exotic feathers", "big cats", "elephant ivory");
                    break;
                default:
                    // A biome with none of these on a coast with T₀ < 3 gives seals at 0.3.
                    if (_coastal && _t0 < 3) (amount, variant) = (0.3, "seals");
                    break;
            }
            if (variant is not null && amount > 0) _out.Put(r, amount, variant);
        }

        if (_mountains && _belt == ThermalBelt.Boreal) _out.Deposit(49, r, "musk deer", 0.03);
    }

    /// <summary>Dyes: the dye crops, lichen dyes, and the deposits murex and kermes.</summary>
    private void Dyes()
    {
        const Resource r = Resource.Dyes;

        Crops(r);
        if (_biome is Biome.Tundra or Biome.TemperateRainForest) _out.Put(r, 0.15, "lichen dyes");
        if (_coastal && _high && _t0 >= 12 && _t0 < 21) _out.Deposit(47, r, "murex", 0.06);
        if (_biome == Biome.WoodlandAndShrubland && _hills && _t >= 12) _out.Deposit(48, r, "kermes", 0.06);
    }

    private void Horses()
    {
        var amount = _f.Mounts * ByZone(1, 1, 0.7, 0.4);
        string variant;
        if (_biome is Biome.HotDesert or Biome.ThornScrub)
        {
            variant = _high ? "donkeys and mules" : "dromedary camel";
        }
        else if (_biome == Biome.ColdDesert)
        {
            variant = "Bactrian camel";
        }
        else if (_tropicalHighland && _mountains)
        {
            variant = "llama and alpaca-like camelids";
            amount = Math.Max(amount, 0.5 * _f.Pasture);
        }
        else if (_high)
        {
            variant = "donkeys and mules";
            amount = Math.Max(amount, 0.3 * _f.Pasture);
        }
        else
        {
            variant = "horses";
        }
        if (amount > 0) _out.Put(Resource.HorsesAndPackAnimals, amount, variant);
    }
}
