# Flora and Fauna: Plants and Animals for the Biomes, as Input for the Generator

*Research date: 2026-11-18. Scope: two long lists, one of **flora** and one of **fauna**, for the 16 biomes of the map and for its waters. Each entry has a name, a type, a description, the biomes it belongs to, a line of lore and the vocations it matters to, and it names the generated resource it yields where there is one. §1 states the method and the counts. §2 lists the biomes as used, with the real-world land each stands for. §3 defines the types, the markers and the conventions. §4 is the flora list (207 entries in eleven types). §5 is the fauna list (167 entries: critters, animals, beasts and monsters). §6 is the index per biome, with the counts. §7 is a design note: what an entry needs as a record, how the lists could enter the generator, what the lists found in the repo's documents, and the open questions. §8 lists the sources.*

**How to read the citations**

- Each factual claim cites the source that owns it, by key in square brackets. The keys are listed in §8. The order of trust is: scientific and reference databases for real species (Animal Diversity Web, Plants For A Future, Useful Tropical Plants, Encyclopaedia Britannica), and the primary or near-primary text for folklore (Pliny, Herodotus, Caesar, Homer, a medieval bestiary, named folklore collections).
- The keys [ADW], [PFAF] and [UTP] stand for the database page of the species whose Latin name is in the entry's heading. [EB] stands for the Britannica article on the subject of the entry. §8 gives the address patterns, lists every Britannica article that was opened, and lists the pages that stood in for another species.
- Web pages were read through a fetch tool that returns an extraction of the page, not the page itself. Sizes and facts are as the tool returned them. The species pages were read by eight parallel research passes, each told to report only what the page it opened says and to mark a row as failed otherwise. The folklore texts and the biome pages I read myself.
- **[partially verified]** means the source exists and was opened, but the claim was confirmed only from a contents list or a search-index excerpt. This holds for the five creatures from Cox's *Fearsome Creatures of the Lumberwoods* (the scan was too large for the tool, so only the list of creatures was read) and for Marco Polo's roc.
- **[unverified]** means general knowledge that was not checked against any source in this pass. It is used in 8 entries and always in the entry itself.
- **Lore** is written for this world. A lore sentence with a key after it reports real folklore from that source. A lore sentence without a key is my invention, or a common tradition that I did not check: treat it as invented. `(RB §n)` points to the entry of a race in `races-basis.md` that the lore builds on. The fact it builds on is in that entry, the rest of the sentence is invented.
- The **biomes** of a real species are my mapping of the habitat and range that the source states onto the 16 biomes of the map. The source never names a biome of this repo. The **resource** line uses the names of `map-generation.md` Appendices B, D and G. The **vocations** are those of `society-generation.md` section 8, with the variant in brackets. All three are my own engineering judgement and not sourced claims.
- No entry takes a creature, a plant or a name from an existing game, novel or setting. The folklore entries come from texts in the public domain. One name in the repo itself does not pass this test: see §7.4.

**Hooks in this repo** (working tree at commit `24a5176`, with an uncommitted edit to `src/Gameworld.Domain/Resources.cs`)

| Hook | Where | What it gives a flora and fauna pass |
|---|---|---|
| Patch | `readme.md`, "Patch" | "A land patch has one biome, which drives its flora and fauna, and an amount of each resource. An ocean patch only carries sea resources." The readme says no more about flora and fauna |
| Biome | `readme.md`, "Biome"; `map-generation.md` §13 | 15 land biomes and Mangrove, "not binding yet". A biome follows from the temperature after cooling by height and from the aridity. Mangrove is an override on warm, wet, low coasts |
| Biome factors | `map-generation.md` Appendix C | Per biome: soil, tree ground, pasture, game, wood, fur, mounts, honey, herb. They already say how much game, timber, fur and herb a biome holds |
| Resource | `map-generation.md` §14, Appendices B, D, G | 27 raw resources with a variant each. Many variants are species: "timber: teak", "furs: sable and marten", "herbs: foxglove", 108 crops. The lists below name the variant wherever an entry yields one |
| Ocean patches | `map-generation.md` §14.4, Appendix D.3 | Fish, seaweed, seals, whales, pearls and coral. No reagents, herbs or arcane materials |
| Water site | `map-generation.md` §14.1 | `river`, `mouth`, `lake`, `harbour`, coast. Several entries live only at one of them |
| Fields | `readme.md`, "Field"; `map-generation.md` §9, §14.1 | Supernatural, 0 to 100, with `power = clamp((supernatural − 60) / 30, 0, 1)`: zero on nine tenths of the map. Prosperity, which marks the land that people have left alone |
| Arcane materials | `map-generation.md` Appendix D.2, kinds 60 to 73 | 14 kinds. Five are plants or fungi (glowcap fungi, frost-lotus, elderwood, sap of Elanor, dreamleaf), one is a pearl (moonpearls), one a bone (dragonbone) |
| Race | `society-generation.md` Appendix A; `races-basis.md` | 28 races with a habitat weight per biome. Their ways of life name animals and plants: reindeer, camels, elephants, cattle, mulberry, tea, dates |
| Vocation | `society-generation.md` §8; `research/vocations.md` | 51 vocations in nine sectors, with variants such as trapper, falconer, herbalist, apothecary, beekeeper, furrier, dyer |
| Earlier research | `research/biomes-and-climate.md` §2.6 | Which classes of Whittaker, Köppen and the WWF biomes each generator biome lines up with |

---

## 1. Overview and method

**Scope.** Flora and fauna here are what a traveller, a hunter or a herbalist would name in a patch: the trees that make the forest, the herbs that are gathered, the game, the pests, the beasts that are feared and the monsters that are told of. The lists are not a census. A real biome holds thousands of species, and each list keeps those that carry a use, a danger or a story.

**Method.**

1. I read the repo first: the readme for the names, `map-generation.md` §13 and §14 with Appendices B, C, D and G for the biomes and the resource variants, `society-generation.md` §8 and `research/vocations.md` for the vocations, and `races-basis.md` for the animals and plants in the lives of the 28 races.
2. I drew up the candidates per biome from the resource variants (every timber, fur, herb, game and mount variant of Appendix D is a species or a group of species) and added what the biome is known for beyond them.
3. Each real species was then checked on one page of a reference database. An entry says only what its page says. Where a page could not be opened and no other authoritative page was found, the entry was dropped (the ice worm, the sandgrouse, the purple saxifrage, the bay laurel, the Sitka spruce, the tinder fungus and the termite mushroom went this way) or kept with **[unverified]**.
4. The folklore creatures and plants were taken from texts I could open and read: Pliny's *Natural History*, Herodotus, Caesar, the *Odyssey*, the Aberdeen Bestiary, Frazer, Sikes, Rink and Yeats. A creature I could not find in a text I could open was left out: the kelpie under that name, the kraken, the black dog, the barnacle tree.
5. Where a biome still lacked a monster or an arcane plant, and where an arcane material of the map had no living source, I invented one and marked it Invented.

**Counts.** 374 entries: 207 flora and 167 fauna.

| List | Type | Entries | Real | Real-derived | Folklore | Invented |
|---|---|---|---|---|---|---|
| Flora | Trees | 51 | 51 | 0 | 0 | 0 |
| Flora | Shrubs | 21 | 21 | 0 | 0 | 0 |
| Flora | Flowers | 12 | 12 | 0 | 0 | 0 |
| Flora | Herbs | 35 | 34 | 1 | 0 | 0 |
| Flora | Fruit | 31 | 31 | 0 | 0 | 0 |
| Flora | Grasses, sedges and reeds | 12 | 12 | 0 | 0 | 0 |
| Flora | Vines and climbers | 9 | 9 | 0 | 0 | 0 |
| Flora | Fungi | 7 | 7 | 0 | 0 | 0 |
| Flora | Mosses, lichens, ferns and algae | 8 | 8 | 0 | 0 | 0 |
| Flora | Aquatic plants | 6 | 6 | 0 | 0 | 0 |
| Flora | Arcane plants | 15 | 0 | 1 | 4 | 10 |
| **Flora** | **all** | **207** | **191** | **2** | **4** | **10** |
| Fauna | Critters | 44 | 44 | 0 | 0 | 0 |
| Fauna | Animals | 56 | 56 | 0 | 0 | 0 |
| Fauna | Beasts | 27 | 25 | 2 | 0 | 0 |
| Fauna | Monsters | 40 | 0 | 2 | 33 | 5 |
| **Fauna** | **all** | **167** | **125** | **4** | **33** | **5** |

**Six findings shape the lists.**

1. **The map already holds a flora and a fauna, as resource variants.** Appendix D names the timbers, furs, game animals, mounts and herbs by species, and Appendix B names 108 crops. 293 of the 374 entries below name a resource of the map: a variant the generator can already pick, or a looser link that is marked as such. The lists add the description, the lore and the vocations to names the map has, and add the species that carry no resource: the pests, the predators and the monsters.
2. **The ancient natural histories sort their marvels by climate, as the generator does.** Pliny and Herodotus put the basilisk, the amphisbaena, the phoenix and the winged serpents in the hot deserts, the catoblepas, the manticore and the yale in the hot grasslands, and the griffin in the cold north [PLIN-H 8.21] [HDT 3.107] [HDT 3.116]. The monsters fall into the biomes without force.
3. **The same sources tie monsters to trade goods.** Winged serpents guard frankincense, great birds nest in cinnamon, ants dig gold, a dragon's blood is a pigment [HDT 3.107-111] [HDT 3.102] [PLIN-B 33.38]. Pliny already suspects that such tales are told to raise the price [PLIN-H 12.19]. In a generated world this is a rule: a monster entry can name the luxury resource it guards.
4. **The Supernatural field is the natural gate for the monsters.** Its power is zero on nine tenths of the map (`map-generation.md` §14.1). An entry of the type Monster or Arcane plant carries a threshold on that field, so the same biome has basilisks in one region and none in the next.
5. **The cold end is poor in plants and the sea carries the animals.** Ice & snow has four flora entries, and no honest research gives it more: Britannica's account of the icy barrens is "lichens, mosses" and a very few flowering plants [EB-TUN]. Its fauna is seals, bears and birds that feed at sea. The generator says the same: on ice, wild food and mounts are zero unless the patch is coastal (Appendix D.1).
6. **Tropical highlands carry temperate biomes.** The cooling by height turns a hot lowland climate into a temperate one (`map-generation.md` §13.1), so coffee, fever bark, the llama and dreamleaf, which the map places on "tropical highland", stand in patches whose biome is a temperate forest. An entry that depends on this says so.

---

## 2. The biomes as used

The names are those of the readme. The codes are those of `society-generation.md` Appendix A. The rule and the share of the land are from `map-generation.md` §13.2 (T is the yearly temperature after cooling by height, m the aridity). The last column is the real land that the biome stands for in this note. It is my reading of `research/biomes-and-climate.md` §2.6, which is itself "not a sourced crosswalk".

| Code | Biome | Rule | Share | Stands for |
|---|---|---|---|---|
| Ice | Ice & snow | T below −10 °C | 7 % | Ice caps and polar barrens: the high Arctic, Greenland, the Antarctic coast |
| Tun | Tundra | T −10 to −3 °C | 5.5 % | Arctic tundra, and alpine tundra above the tree line |
| CSt | Cold steppe | T −3 to 3 °C, m 0.5 to 2 | 3 % | The Mongolian and Kazakh steppe, the high plateau steppe, Patagonia |
| Bor | Boreal forest | T −3 to 3 °C, m from 2 | 9.5 % | The taiga of Siberia, Scandinavia and Canada |
| CDs | Cold desert | T −3 to 18 °C, m below 0.5 | 8 % | The Gobi, the Taklamakan, the Great Basin |
| TGr | Temperate grassland | T 3 to 18 °C, m 0.5 to 1 | 4.5 % | Prairie, the Pontic steppe, the pampas. Most of it is farmland where people live |
| WSh | Woodland & shrubland | T 3 to 18 °C, m 1 to 2 | 8 % | Mediterranean woodland and scrub, chaparral. At its cool end an open oak, pine and juniper woodland |
| TMx | Temperate mixed forest | T 3 to 10 °C, m 2 to 6 | 5.5 % | The conifer and hardwood forests of the Baltic, New England and Manchuria |
| TBl | Temperate broadleaf forest | T 10 to 18 °C, m 2 to 6 | 6 % | The oak and beech forests of western Europe, and the broadleaf forests of eastern China and Japan |
| TRn | Temperate rain forest | T 3 to 18 °C, m from 6 | 7 % | The Pacific Northwest, Valdivia, New Zealand, the Atlantic oakwoods |
| HDs | Hot desert | T from 18 °C, m below 0.5 | 11 % | The Sahara, Arabia, the Sonoran desert |
| Thn | Thorn scrub | T from 18 °C, m 0.5 to 1 | 5 % | The Sahel, the Horn of Africa, the Caatinga, the Chaco, the Indian thorn forest |
| Sav | Savanna | T from 18 °C, m 1 to 2 | 6.5 % | The African and Indian savannas |
| TDf | Tropical dry forest | T from 18 °C, m 2 to 4 | 6 % | The monsoon forests of India and Indochina (teak and sal), the dry forests of Central America |
| TRf | Tropical rain forest | T from 18 °C, m from 4 | 4.5 % | The Amazon, the Congo, the Malay archipelago |
| Mgv | Mangrove | Override: coastal, flat or lowland, T from 20 °C, m from 0.5 | 2 % | The Sundarbans and other mangrove coasts |
| Sea | (ocean patches, rivers and lakes) | Not a biome | | The shelf, the open water, and the fresh water of a land patch |

**How the waters are treated.** An ocean patch has no biome and "only carries sea resources" (readme, "Patch"). Sea life is therefore listed under the code **Sea** and kept short: the species behind the sea resources of Appendix D.3 (cod, herring, tuna, seals, whales, pearl oyster, kelp), the coast animals of the ice, and three sea monsters. River and lake species (salmon, sturgeon, eel, beaver, lotus, reed) are listed under the land biomes they occur in, and the entry names the water site. An index line for Sea is in §6.

---

## 3. Types, markers and conventions

**Flora types.** The user named Tree, Flower, Herb and Fruit. Seven more were needed.

| Type | Id | What it holds |
|---|---|---|
| Tree | TR | Woody plants of tree height, palms included. A tree grown for its fruit or nut is under Fruit |
| Shrub | SH | Woody or succulent plants below tree height: bushes, cacti, agaves, and the one parasite (mistletoe) |
| Flower | FL | Plants known for the bloom |
| Herb | HE | Soft plants gathered or grown for medicine, poison, dye, fibre or fodder. A woody plant used the same way is here if it is small (sage, senna) |
| Fruit | FR | Plants known for an edible fruit, nut or berry, whatever their form |
| Grass | GR | Grasses, sedges, reeds and bamboo, with the wild kin of the grains |
| Vine | VI | Climbers and trailers |
| Fungus | FU | Mushrooms and the fungi of grain and wood |
| Moss | MO | Mosses, lichens, ferns and algae |
| Aquatic | AQ | Plants of fresh water and of the sea |
| Arcane plant | AR | Plants and fungi with a supernatural property that is true in this world. The counterpart of Monster |

**Fauna types.** As the user named them.

| Type | Id | What it holds |
|---|---|---|
| Critter | CR | Small animals: harmless, a nuisance, or useful in a small way. Insects, small birds, rodents, shellfish |
| Animal | AN | Ordinary wildlife and livestock, fish included |
| Beast | BE | Large or dangerous natural creatures: the great predators, the giants, the animals that kill people |
| Monster | MN | Supernatural or legendary creatures |

**Basis.** Every entry is marked with one of four.

| Basis | Meaning |
|---|---|
| **Real** | A real species or group, described as the source describes it |
| **Real-derived** | A real species that is extinct in our world and alive in this one (mammoth, aurochs, silphium), or a real species that also has the powers folklore gave it (salamander, mandrake, the stay-ship). The entry says which part is which |
| **Folklore** | A creature or plant from a named text in the public domain. The description reports the text |
| **Invented** | Made for this world in this note. It needs no source and has none |

**The Supernatural field.** A Monster or an Arcane plant ends its description with "Field:" and one of three tiers. The tiers use the power of `map-generation.md` §14.1.

| Tier | Supernatural | Power | Share of the map (at the defaults) | Meant for |
|---|---|---|---|---|
| S1 | above 60 | above 0 | about a tenth | Minor wonders: a will-o'-the-wisp, a glowcap |
| S2 | from 75 | from 0.5 | less | Creatures a region is known for: a basilisk, a griffin |
| S3 | from 90 | 1 | least | One of a kind: a phoenix, a dragon, a unicorn |

The tiers are a proposal. The readme says of the field only that the arcane materials lie where it is high.

**The resource line.** "FUR ermine" means the variant "ermine" of the generated resource Furs & ivory, with the abbreviation of Appendix G.3. "(catalogue)" means the variant stands in the catalogue of Appendix G.3 and the generation cannot pick it yet. "minor", "as game", "proposal" and "not a catalogue variant" mark a link that the map does not have: my suggestion. "a harm" marks a pest.

**Entry form.** A heading line with the id, the name, the Latin name where there is one, the basis and the biome codes. Then one paragraph: the description with its citations, the resource, the lore and the vocations.

---

## 4. Flora

### 4.1 Trees (51)

**TR-01 Spruce** (*Picea abies*) · Real · Bor, TMx  
Fast evergreen conifer to about 30 m of northern and central Europe. Timber, paper pulp, pitch, turpentine, tannin and resin. The shoot tips make a tea, and the inner bark is food in need [PFAF]. **Resource:** TMB spruce. **Lore:** The Northmen roof-tree is a spruce felled at midwinter, and the house stands as long as its stump is left alone. **Vocations:** Forester; Carpenter; Charcoal burner (pitch boiler).

**TR-02 Scots pine** (*Pinus sylvestris*) · Real · Bor, WSh, TMx  
Evergreen conifer to about 25 m, from Europe into temperate Asia. Timber for building and furniture, resin, turpentine, pitch, fuel and rope fibre. The inner bark is famine food [PFAF]. **Resource:** TMB Scots pine; pine resin, tar and pitch (catalogue). **Lore:** Tar is the Northmen's second export after ships (RB §1 lists the tar burner), and every hull is a pine forest boiled down. **Vocations:** Forester (resin tapper); Charcoal burner (tar burner); Shipwright.

**TR-03 Larch** (*Larix sibirica*) · Real · Bor  
Deciduous conifer to about 30 m, of mountains and lowland taiga. The wood is strong, heavy and durable, for building, bridges and poles, and the bark is a commercial source of tannin [PFAF]. **Resource:** TMB larch (hills and mountains). **Lore:** The one conifer that goes bare in winter. Amberbloods (RB §16) say it is the tree that remembers being a person. **Vocations:** Forester; Carpenter; Tanner.

**TR-04 Birch** (*Betula pendula*) · Real · Tun, Bor, TMx  
Fast deciduous tree to about 20 m of open woodland and heath. Sap and inner bark are eaten. It gives timber, firewood, charcoal, tar oil, bark for thatching, fibre and tannin [PFAF]. **Resource:** TMB (minor); FUL firewood. **Lore:** The first tree back after fire and ice. A Northmen child's cradle is birch, and so is the bark its first letters are scratched on. **Vocations:** Forester; Charcoal burner; Tanner; Scribe.

**TR-05 Poplar and aspen** (*Populus tremula; P. euphratica*) · Real · CSt, Bor, CDs, TGr, TMx, HDs  
Fast deciduous trees of 15 to 18 m. Aspen grows in open woodland on poor soils, and its soft wood makes paper and charcoal. The Euphrates poplar stands on ground flooded part of the year and gives building wood and a poor fuel [PFAF]. **Resource:** TMB willow and poplar (rivers of grassland and steppe); poplar (rivers of the deserts). **Lore:** In the deserts a line of poplars is a river seen from a day away, and the only roof beams for a week's ride. **Vocations:** Forester; Carpenter.

**TR-06 Rowan** (*Sorbus aucuparia*) · Real · Bor, TMx, TRn  
Deciduous tree to about 15 m of woods, scrub and mountain rocks. The fruit is eaten cooked and is astringent. The wood is turned and makes hoops, and the tree gives a black dye and tannin [PFAF]. **Resource:** none. **Lore:** A rowan growing on another tree guards against witchcraft [FRAZER 68]. A sprig hangs over the door of every byre in the north. **Vocations:** Seer (wise woman); Carpenter; Herder.

**TR-07 Alder** (*Alnus glutinosa*) · Real · Bor, TMx, TBl, TRn  
Short-lived deciduous tree to about 25 m of wet ground by streams and lakes. The wood lasts under water. It gives charcoal, dyes, ink and tannin [PFAF]. **Resource:** TMB (minor). **Lore:** Canal folk drive alder piles under their quays (RB §7): a town on water stands on a drowned alder wood. **Vocations:** Carpenter; Mason; Weaver (dyer); Charcoal burner.

**TR-08 Juniper** (*Juniperus communis*) · Real · CSt, Bor, CDs, TGr, WSh  
Evergreen shrub or small tree, from under 1 m to 9 m, of downs, heaths and moors across the northern temperate zone. The cones flavour food and spirits and are a kidney remedy. Fuel, incense and tinder [PFAF]. **Resource:** TMB juniper. **Lore:** The smoke cleans a tent after sickness. Grass riders (RB §5) carry a branch from the home camp and burn the last of it on arrival. **Vocations:** Brewer (distiller); Healer (herbalist); Priest.

**TR-09 Willow** (*Salix alba*) · Real · CSt, TGr, TMx, TBl  
Fast deciduous tree to about 25 m of streams, marshes and wet fens. The rods make baskets. The bark is taken for pain and fever. Charcoal, poles and turnery [PFAF]. **Resource:** TMB willow and poplar. **Lore:** The osier beds of a river town are rented by the row, and the rent is paid in eel traps. **Vocations:** Weaver (basket maker, assumed); Healer (herbalist); Fisher (eel trapper).

**TR-10 Oak** (*Quercus robur*) · Real · WSh, TMx, TBl  
Slow deciduous tree to about 30 m, dominant on clay soils. Durable timber, fuel and charcoal. The bark tans. The galls make ink and dye. Acorns are eaten leached [PFAF]. **Resource:** TMB oak; WLD acorns. **Lore:** The Old Realms planted oaks in rows for ships they never built. The rows still stand, and courts are held under them. **Vocations:** Forester; Carpenter; Shipwright; Tanner; Scribe; Herder (swineherd).

**TR-11 Beech** (*Fagus sylvatica*) · Real · TMx, TBl  
Deciduous tree to about 30 m, dominant on chalk and limestone. Durable wood, good fuel and charcoal. The nuts give an oil, and the dry leaves stuff mattresses [PFAF]. **Resource:** TMB beech. **Lore:** Gnomes dug their warrens under the beech roots (RB §11) and still pay the tree a yearly rent in mast. **Vocations:** Forester; Carpenter; Charcoal burner; Herder (swineherd).

**TR-12 Ash** (*Fraxinus excelsior*) · Real · TMx, TBl  
Fast deciduous tree to about 30 m of woods and hedges on limy soils. The wood makes tool handles and furniture and is a good fuel. The bark tans and the leaves give a green dye [PFAF]. **Resource:** TMB ash (floodplains). **Lore:** Every spear shaft and plough handle in the Midlands is ash. A lord's levy is counted in ash trees as well as men. **Vocations:** Carpenter (wheelwright, bowyer); Smith; Forester.

**TR-13 Elm** (*Ulmus glabra*) · Real · TMx, TBl  
Fast deciduous tree to about 30 m of woods, hedges and streamsides. The wood lasts in water and makes pipes, wheels and keels. The inner bark gives fibre for mats and rope [PFAF]. **Resource:** TMB elm (floodplains). **Lore:** A town's water runs through bored elm trunks, and the plumber is a kind of carpenter. **Vocations:** Carpenter (wheelwright); Shipwright; Weaver (rope maker).

**TR-14 Lime** (*Tilia cordata*) · Real · TMx, TBl  
Deciduous tree to about 30 m of woods on fertile, limy soils. The bast gives fibre for mats, rope and cloth. The soft wood is carved and makes drawing charcoal. The flowers make a calming tea [PFAF]. **Resource:** TMB lime; FBR nettle and lime-bast. **Lore:** Stillfaces (RB §25) carve their masks of lime wood, one for each feeling they wish to be seen having. **Vocations:** Carpenter (woodcarver); Weaver (rope maker); Artist; Healer (herbalist).

**TR-15 Fir** (*Abies alba*) · Real · TMx  
Fast evergreen conifer to about 45 m of mountain forests. Light soft timber and pulp. The resin goes to perfumery and to caulking ships [PFAF]. **Resource:** TMB fir (mountains). **Lore:** The tallest masts come down the mountain rivers one at a time, each with a rider. **Vocations:** Forester; Shipwright; Carpenter.

**TR-16 Sugar maple** (*Acer saccharum*) · Real · TMx, TBl  
Large tree to about 40 m with a dense crown. Syrup and sugar are boiled from its sap, and its hard wood makes furniture and floors [EB]. **Resource:** WLD maple sap (catalogue). **Lore:** The sap run is the first harvest of the year and the only one the frost helps. **Vocations:** Forester; Gardener; Carpenter.

**TR-17 Yew** (*Taxus baccata*) · Real · WSh, TMx, TBl, TRn  
Slow, long-lived evergreen to 25 m. The hard fine-grained wood makes bows, cabinet work and carvings. Leaves and seeds are poisonous and sometimes kill livestock [EB]. **Resource:** TMB yew (catalogue). **Lore:** Planted where the dead lie, and cut for the weapon that sends them there. Elves (RB §8) fell no healthy tree, so their bows are of windfall yew. **Vocations:** Carpenter (bowyer); Soldier; Priest.

**TR-18 Mulberry** (*Morus alba*) · Real · WSh, TBl, TDf  
Tree with sweet white to dull red fruit, widely grown. Its leaves feed silkworms [EB]. **Resource:** FBR silk (the tree behind it). **Lore:** Canal folk plant it on the dikes between the paddies (RB §7), and a daughter's dowry is counted in trees. **Vocations:** Gardener (silk grower).

**TR-19 Cedar** (*Cedrus libani*) · Real · WSh, TBl  
Long-lived evergreen conifer with a massive spreading crown, of mountains. Aromatic wood that resists decay, used for building and for ships [EB]. **Resource:** TMB cedar (warm broadleaf mountains). **Lore:** The sorcerer-kings roofed their halls with it. A cedar beam from an Old Realms ruin is sold by weight and never burnt. **Vocations:** Forester; Shipwright; Carpenter; Priest.

**TR-20 Giant conifers** (*Sequoia sempervirens; Thuja plicata*) · Real · TRn  
The redwood is the tallest living tree, often over 90 m, of a coastal fog belt. Its timber goes to building, shingles and posts. The red cedar reaches about 60 m with a buttressed base, and its wood resists damp and decay: shingles, pilings and boats [EB]. **Resource:** TMB giant conifers. **Lore:** One trunk is a ship's keel, a hall's ridge and a winter's work for a village. Elves count the felling of one as a killing. **Vocations:** Forester; Shipwright; Carpenter.

**TR-21 Hemlock tree** (*Tsuga heterophylla*) · Real · TMx, TRn  
Evergreen conifer of about 60 m with a trunk 2 to 3 m across. A valuable timber [EB]. **Resource:** TMB giant conifers. **Lore:** Its bark fills the tanners' pits of the wet coast, and the stripped trunks are left to the mushrooms. **Vocations:** Forester; Tanner.

**TR-22 Lacquer tree** (*Toxicodendron vernicifluum*) · Real · TBl  
The tree whose sap is true lacquer. Purified and brushed on wood or metal, it cures to a firm shining finish. Raw, it blisters the skin [EB]. **Resource:** none in the catalogue (proposal: a variant of DYE or a product). **Lore:** Canal folk lacquer (RB §7) takes thirty coats and a tapper with scarred hands. **Vocations:** Forester (resin tapper); Artist (lacquer worker, assumed); Carpenter.

**TR-23 Cork oak** (*Quercus suber*) · Real · WSh  
Evergreen oak of about 18 m. The bark is stripped every eight to ten years without harm to the tree, for stoppers, floats and fishing gear [EB]. **Resource:** TMB cork oak. **Lore:** The year of the last stripping is painted on the trunk, and a grove is sold with its dates. **Vocations:** Forester; Weaver (net maker); Brewer (wine maker).

**TR-24 Kermes oak** (*Quercus coccifera*) · Real · WSh  
Dwarf evergreen oak with tannin-rich bark, on which the red dye insect lives [EB]. **Resource:** DYE kermes (deposit: shrubland hills, T ≥ 12 °C). **Lore:** Goats may browse any scrub but this. The fine for a goat in the kermes oaks is the goat. **Vocations:** Hunter (forager); Tanner; Weaver (dyer).

**TR-25 Cypress** (*Cupressus sempervirens*) · Real · WSh  
Narrow evergreen conifer of about 24 m with aromatic foliage. Grown for its form. The timber is often knotty [EB]. **Resource:** TMB cypress (hills and mountains). **Lore:** Planted in pairs at a gate: one for welcome, one for the day the guest is carried out. **Vocations:** Carpenter; Priest; Gardener.

**TR-26 Stone pine** (*Pinus pinea*) · Real · WSh  
Pine with a wide flat crown on a tall bare trunk. Its nuts have been valued since ancient times [EB]. **Resource:** TMB Mediterranean pines; OCH stone pine nuts (catalogue). **Lore:** Swiftborn pickers (RB §22) strip a grove in a morning and are paid by the sack. **Vocations:** Gardener; Forester; Baker.

**TR-27 Saxaul** (*Haloxylon ammodendron*) · Real · CSt, CDs  
Branched shrub or tree of 1 to 5 m with deep roots, of foothill deserts, dry riverbeds and salt flats. It bears drought and salt. Desert people burn it, it holds sand, and sheep and camels browse it [AGRO]. **Resource:** FUL firewood (the only wood of many cold deserts). **Lore:** Brinefolk pan towns (RB §17) boil their brine over saxaul and plant two for each they burn. **Vocations:** Forester; Salter; Herder.

**TR-28 Acacia** (*Acacia senegal; A. tortilis*) · Real · HDs, Thn, Sav  
Thorny trees of 6 to 15 m of dry scrub and wooded grassland, the umbrella thorn with a flat crown. The gum goes into food, medicine, textiles and ink. Posts, poles, fuel and charcoal. The bark tans, and the pods and seeds are eaten [UTP]. **Resource:** SPC gum arabic; TMB thorn-scrub wood. **Lore:** A Sunlander cattle fold is fenced with cut thorn (RB §4). The gum tappers work the same trees their grandmothers scarred. **Vocations:** Forester (resin tapper); Charcoal burner; Herder; Scribe.

**TR-29 Frankincense tree** (*Boswellia sacra*) · Real · HDs, Thn  
Evergreen tree to 8 m of dry mountains and desert woodland. The resin is burnt as incense and used in perfume and medicine [UTP]. Pliny has it cut in summer and again in spring, the gatherers kept from women and funerals, and the gum taxed before it may leave [PLIN-H 12.14]. **Resource:** SPC frankincense (hills and mountains). **Lore:** Saltroad incense gatherers (RB §3) inherit their trees by name and may not sell them outside the clan. **Vocations:** Gardener (incense gatherer); Priest; Merchant.

**TR-30 Myrrh tree** (*Commiphora myrrha*) · Real · HDs, Thn  
Spiny deciduous shrub or small tree of about 5 m, of desert scrub on shallow limestone soil. The resin that weeps from cuts in the bark is used in medicine, perfume, incense and embalming [UTP]. Pliny has the trees cut twice a year [PLIN-H 12.15]. **Resource:** SPC myrrh. **Lore:** The Brinefolk embalmers (RB §17) are its best customers. **Vocations:** Gardener (incense gatherer); Alchemist (embalmer, perfumer); Healer.

**TR-31 Dragon's blood tree** (*Dracaena cinnabari*) · Real · HDs, Thn  
Evergreen tree of about 9 m with a dense umbrella crown, on rock and limestone. Its red resin is used in medicine, varnish, pottery colour and cosmetics [UTP]. **Resource:** none in the catalogue (proposal: a variant of DYE). **Lore:** Pliny's tale that the red is the blood of a dragon crushed by an elephant [PLIN-B 33.38] is told of this resin too. Apothecaries sell it by the tear. **Vocations:** Forester (resin tapper); Artist; Alchemist (apothecary); Potter.

**TR-32 Baobab** (*Adansonia digitata*) · Real · Thn, Sav  
Deciduous tree to about 20 m with a swollen trunk up to 10 m across, of the thorn woodlands of the savanna. The fruit pulp, leaves and seeds are eaten. The bark gives cordage and cloth [UTP]. **Resource:** OCH baobab. **Lore:** A village meets under its baobab. The Sunlander council that can depose a king (RB §4) sits in the shade of the oldest. **Vocations:** Gardener; Weaver (rope maker); Official.

**TR-33 Mesquite** (*Prosopis juliflora*) · Real · HDs, Thn  
Thorny flat-topped tree of 5 to 10 m on light sandy soils of arid land. The pods make flour and drink. Posts, firewood and charcoal, and a tannin [UTP]. **Resource:** OCH mesquite pods (catalogue). **Lore:** The pod flour keeps a year. Duskreader night markets (RB §18) price everything else against it. **Vocations:** Miller; Charcoal burner; Herder.

**TR-34 Teak** (*Tectona grandis*) · Real · TDf  
Large, mostly deciduous tree over 30 m of the tropical deciduous forests. A first-rate timber. Bark and leaves give a dye [UTP]. **Resource:** TMB teak. **Lore:** Rainkeeper kings own every teak in the realm, standing or fallen, and mark each with a seal. **Vocations:** Forester; Shipwright; Carpenter; Carter (elephant driver).

**TR-35 Sal** (*Shorea robusta*) · Real · TDf  
Tree of 20 to 25 m, the main forest tree of its range. Timber. The resin is burnt as incense and caulks boats. The seed gives a butter and a lamp oil, and the leaves serve as plates [UTP]. **Resource:** TMB sal (wetter dry forest); copal and dammar (catalogue). **Lore:** A Rainkeeper feast is eaten off sal leaves, and the count of leaves swept up is the count of guests. **Vocations:** Forester; Shipwright; Priest.

**TR-36 Sandalwood** (*Santalum album*) · Real · TDf  
Small slow evergreen tree of 4 to 20 m of dry deciduous and scrub forest. The scented heartwood is carved, burnt as incense and pressed for a perfume oil [UTP]. **Resource:** SPC sandalwood. **Lore:** The tree is weighed, not measured, and the roots are dug: nothing of it is left in the ground. **Vocations:** Forester; Carpenter (woodcarver); Alchemist (perfumer); Priest.

**TR-37 Banyan and sacred fig** (*Ficus benghalensis; F. religiosa*) · Real · TDf, TRf  
Wide-crowned evergreen figs of 15 to 30 m that often start as a seedling on another tree. The banyan drops roots from its branches. Fruit, medicine, a bark fibre, and a latex used as varnish and sealing wax [UTP]. Pliny wonders at the tree whose branches root and make arbours for shepherds [PLIN-H 12.5]. **Resource:** none. **Lore:** A Rainkeeper village council of five (RB §6) sits under one, and nothing said outside its shade is binding. **Vocations:** Priest; Official; Healer (herbalist).

**TR-38 Neem** (*Azadirachta indica*) · Real · TDf, TRf  
Fast, long-lived evergreen of about 15 m with a dense crown. Bark, leaves and seed are a mainstay of medicine. Timber, firewood, and a resin used in soap [UTP]. **Resource:** HRB neem. **Lore:** Twigs for the teeth, leaves in the grain bin, a branch at the door in fever time. **Vocations:** Healer (herbalist); Alchemist (apothecary); Farmer.

**TR-39 Kapok** (*Ceiba pentandra*) · Real · TDf, TRf  
Deciduous tree of 10 to 30 m, at times 70 m, often spiny and buttressed. The floss around the seeds is a fibre for stuffing. The soft wood is of low grade [UTP]. **Resource:** FBR kapok. **Lore:** The tallest tree of the forest and the ladder of the dead. Boughwalkers (RB §20) build no platform in one. **Vocations:** Weaver; Carpenter; Priest.

**TR-40 Mahogany** (*Swietenia macrophylla*) · Real · TDf, TRf  
Slow evergreen of 30 to 40 m of mixed forest and riverbanks. A high-class furniture timber. The bark gives a dye and a tannin [UTP]. **Resource:** TMB mahogany. **Lore:** One log floated to the coast buys a village's salt for a year. **Vocations:** Forester; Carpenter; Merchant.

**TR-41 Ebony and rosewood** (*Diospyros ebenum; Dalbergia latifolia*) · Real · TDf, TRf  
Ebony is a slow evergreen to 20 m with a black heartwood for furniture, instruments and carving. Rosewood is a tree of 30 to 40 m of monsoon forest with a high-value timber [UTP]. Pliny notes ebony carried in a triumph [PLIN-H 12.4]. **Resource:** TMB ebony, rosewood. **Lore:** The black keys of every court harp were once one tree, and its stump is still shown. **Vocations:** Forester; Carpenter (woodcarver); Artist; Merchant.

**TR-42 Rubber tree** (*Hevea brasiliensis*) · Real · TRf  
Fast deciduous tree, over 40 m in the wild, of riverbanks and flooded rain forest. Its latex is the main source of natural rubber [UTP]. **Resource:** SPC rubber latex. **Lore:** Boughwalkers seal their rope bridges with it. Lowlanders buy it for balls and for the soles of thieves. **Vocations:** Forester (resin tapper); Leatherworker; Thief.

**TR-43 Cinnamon** (*Cinnamomum verum*) · Real · TDf, TRf  
Slow evergreen of 7 to 18 m of moist forest, kept as a bush when cultivated. Bark and leaves flavour food, and the oils go to incense and medicine [UTP]. **Resource:** SPC cinnamon. **Lore:** See the cinnamon bird. Rainkeeper spice growers (RB §6) peel it by the quill. **Vocations:** Gardener (spice grower); Merchant; Alchemist (perfumer).

**TR-44 Clove and nutmeg** (*Syzygium aromaticum; Myristica fragrans*) · Real · TRf  
Evergreen trees of maritime forest and humid volcanic lowlands. The clove is a dried flower bud, taken as a spice and for toothache. The nutmeg seed and its mace are spices, and about 8 g of ground nutmeg can kill [UTP]. **Resource:** SPC clove; nutmeg and mace (coasts). **Lore:** Each grows on a handful of coasts. Wars of the Scattered Age have been fought over a single grove. **Vocations:** Gardener (spice grower); Merchant; Healer.

**TR-45 Agarwood** (*Aquilaria malaccensis*) · Real · TRf  
Evergreen tree of about 20 m of primary and secondary forest. A dark resin forms in the wood and is burnt as incense, used in perfume and medicine. The inner bark gives a fibre for cloth and rope [UTP]. **Resource:** SPC agarwood (catalogue). **Lore:** Only a wounded tree makes the resin, and one in many. The gatherers say the tree must first be given a grief. **Vocations:** Hunter (forager); Alchemist (perfumer); Priest; Merchant.

**TR-46 Fever-bark tree** (*Cinchona officinalis*) · Real · TDf, TRf  
Evergreen shrub or small tree of 6 to 20 m of cool humid mountains between 1,200 and 3,000 m. The bark is the remedy for fevers and malaria. Too much brings headache, deafness and blindness [UTP]. In the generator it stands where the rain forest climbs. **Resource:** HRB fever bark. **Lore:** Bitterblood apothecaries (RB §27) sell it and are the only ones trusted to dose it. **Vocations:** Healer (physician); Alchemist (apothecary); Hunter (forager).

**TR-47 Logwood** (*Haematoxylum campechianum*) · Real · TDf, TRf, Mgv  
Thorny, gnarled tree to about 15 m of flat marshy lowland that rivers flood. The heartwood gives red and darker dyes, which is its main use. The flowers give honey [UTP]. **Resource:** DYE red dyewoods. **Lore:** Cut by gangs who live half the year in the swamp and are paid in the other half, if they come out. **Vocations:** Forester; Weaver (dyer); Labourer.

**TR-48 Flame of the forest** (*Butea monosperma*) · Real · Sav, TDf  
Slow, crooked deciduous tree of 5 to 15 m of drier plains and grassland. The flowers give a yellow to orange dye. A red gum, a bark fibre, and a soft wood that lasts under water [UTP]. **Resource:** DYE (flower dye; not a catalogue variant). **Lore:** It flowers leafless at the end of the dry season. Twofold (RB §26) set the Going out by its bloom. **Vocations:** Weaver (dyer); Priest.

**TR-49 Quebracho** (*Schinopsis lorentzii*) · Real · Thn, TDf  
Tree of 20 to 25 m of dry plains. The wood is very durable, for posts, and a good fuel. Tannin is extracted from bark and heartwood on a commercial scale [UTP]. **Resource:** TMB thorn-scrub wood; tanbark (a by-product in society-generation.md section 8). **Lore:** An axe that fells one is retired and hung up. **Vocations:** Forester; Tanner; Charcoal burner.

**TR-50 Mangrove** (*Rhizophora mangle; Avicennia marina*) · Real · Mgv  
Trees and shrubs of muddy, salt or brackish shores. The red mangrove stands 5 to 20 m on arching stilt roots. Timber for posts and marine piling, firewood and charcoal, and a bark that tans. The grey mangrove gives boat timber and red and brown dyes [UTP]. **Resource:** TMB mangrove timber; FUL charcoal. **Lore:** Shoalfolk (RB §21) do not cut the seaward row. The one town that did is a sandbar now. **Vocations:** Forester; Charcoal burner; Tanner; Shipwright.

**TR-51 Nipa palm** (*Nypa fruticans*) · Real · Mgv  
Clumping palm with leaves to 6 m from a stem that lies in the mud of tidal flats. The sap gives syrup, sugar, vinegar and drink. The leaves thatch and are woven into baskets and mats [UTP]. **Resource:** STM palm sugar and toddy. **Lore:** Every roof in a Shoalfolk town is nipa, renewed after the storm season by the whole street. **Vocations:** Mason (thatcher); Brewer; Weaver (basket maker, assumed).

### 4.2 Shrubs (21)

**SH-01 Dwarf willow** (*Salix*) · Real · Tun  
The "low willows" that Britannica lists with the sedges, cotton grass and rushes of the tundra, where plants keep close to the ground against the wind [EB-TUN]. No species page was opened. **Resource:** FUL (a little firewood where there is no other). **Lore:** The Hearthbloods' only firewood above the tree line, cut with a knife and carried in a hat. **Vocations:** Herder; Labourer.

**SH-02 Labrador tea** (*Ledum palustre*) · Real · Tun, Bor  
Evergreen shrub of about 1 m of acid bogs. The aromatic leaves make a tea for coughs and colds and stood in for hops in beer. They repel insects. Brewed long in a closed pot, they are narcotic [PFAF]. **Resource:** HRB Labrador tea. **Lore:** A Northmen ale brewed with it is called "the quarrel": it goes to the head and then to the fists. **Vocations:** Healer (herbalist); Brewer.

**SH-03 Bog myrtle** (*Myrica gale*) · Real · Bor, TMx, TRn  
Deciduous shrub of about 2 m of bogs, fens and wet heath. Leaves and fruit flavour ale. A wax for candles, a yellow dye, and a repellent of insects and moths. Not for the pregnant [PFAF]. **Resource:** HRB bog myrtle. **Lore:** The ale herb of every people north of the hop line. **Vocations:** Brewer; Healer (herbalist); Weaver (dyer).

**SH-04 Hawthorn** (*Crataegus monogyna*) · Real · TGr, WSh, TMx, TBl  
Thorny deciduous shrub to 6 m of woods, hedges and thickets. The fruit is eaten. A remedy for the heart. It is planted as a hedge and a windbreak and gives tool handles and fuel [PFAF]. **Resource:** none. **Lore:** Halfling fields (RB §10 has them in the hedged corners of the lowlands) are hawthorn-hedged, and a gap in the hedge is a gap in the law. **Vocations:** Farmer; Healer (herbalist); Carpenter.

**SH-05 Elder** (*Sambucus nigra*) · Real · TGr, TMx, TBl  
Fast deciduous shrub of about 6 m of hedgerows and rich disturbed ground. Flowers and cooked fruit go into wine and preserves. Dyes from bark and fruit. The leaves keep insects off and are poisonous [PFAF]. **Resource:** WLD elderberry (catalogue). **Lore:** Nobody burns elder or cuts it without asking. What is asked is not agreed between any two villages. **Vocations:** Brewer (wine maker); Healer (herbalist); Seer (wise woman).

**SH-06 Heather** (*Calluna vulgaris*) · Real · Tun, Bor, TMx, TRn  
Evergreen shrub of about 0.6 m of acid moors and heaths. It flavours beer, gives a yellow dye and a tannin, and serves as fuel, thatch, brooms, bedding and rope [PFAF]. **Resource:** FUL (heath fuel; not a catalogue variant). **Lore:** A moorland house is heather from the roof to the bed to the fire. **Vocations:** Mason (thatcher); Brewer; Gardener (beekeeper).

**SH-07 Sagebrush** (*Artemisia tridentata*) · Real · CSt, CDs  
Evergreen shrub of about 2.5 m of dry plains and hills. A bitter leaf tea for colds and aches. Fuel, tinder, a bark fibre and a yellow dye. The seeds are ground for meal [PFAF]. **Resource:** FUL firewood (minor); HRB wormwood (its kin). **Lore:** The smell of the cold desert after rain. Brinefolk say they can smell it through a wall of salt. **Vocations:** Healer (herbalist); Herder.

**SH-08 Ephedra** (*Ephedra sinica*) · Real · CSt, CDs  
Evergreen shrub of about 0.4 m of desert, sandy ground and mountain slopes. An old remedy for asthma and hay fever. Dangerous to a weak heart [PFAF]. **Resource:** HRB ephedra. **Lore:** Grass rider post riders (RB §5) chew it to stay in the saddle for two days. The third day is paid for. **Vocations:** Healer (physician); Alchemist (apothecary); Carter (post rider).

**SH-09 Saltbush** (*Atriplex canescens*) · Real · CDs, HDs  
Evergreen shrub of about 1.8 m of sandy and gravelly desert. Leaves are eaten cooked and the seed ground. Fodder, a windbreak, and a lather for washing [PFAF]. **Resource:** LVS (browse for sheep and camels). **Lore:** Where it grows the well is brackish and the mutton needs no salt. **Vocations:** Herder; Farmer.

**SH-10 Camel thorn** (*Alhagi maurorum*) · Real · CSt, CDs, HDs  
Deciduous spiny shrub to 2 m of saline, dry and barren ground. A sweet "manna" forms on the twigs and pods. The root is a famine food [PFAF]. **Resource:** WLD (manna; not a catalogue variant). **Lore:** The manna is shaken onto a cloth at dawn. Caravan law gives it to whoever spread the cloth, on anyone's land. **Vocations:** Hunter (forager); Carter (caravan driver).

**SH-11 Creosote bush** (*Larrea tridentata*) · Real · HDs  
Slow evergreen shrub of about 4 m of hot desert. A leaf tea for wounds and chests that harms the liver if taken long. Its resin keeps fat from going rancid [PFAF]. **Resource:** none. **Lore:** A ring of bushes is one plant older than any kingdom, the Duskreaders say, and they hold their markets inside the rings. **Vocations:** Healer (herbalist); Butcher.

**SH-12 Saguaro** (*Carnegiea gigantea*) · Real · HDs  
Columnar ribbed cactus that branches at about 5 m and reaches 15 m. It first flowers at 50 to 75 years and lives 150 to 200. The fruit is eaten, and the woody skeleton is used [EB]. **Resource:** WLD (fruit; not a catalogue variant). **Lore:** A giant with its arms raised marks a grave or a well. The traveller is expected to know which. **Vocations:** Hunter (forager); Carpenter.

**SH-13 Sisal and agave** (*Agave sisalana*) · Real · HDs, Thn  
Evergreen succulent rosette of 1.5 to 2 m of the drier tropics that sends up one flower stem of 5 to 6 m and dies. Leaf fibre for rope, nets and paper. A fermented drink from the heart. Living fences [UTP]. **Resource:** FBR sisal and agave. **Lore:** Noonfolk estates (RB §19) are fenced with it, and the fence is the rope walk's raw stock. **Vocations:** Weaver (rope maker, net maker); Brewer; Farmer.

**SH-14 Mastic** (*Pistacia lentiscus*) · Real · WSh  
Slow evergreen shrub of about 5 m of dry hillsides near the sea. The resin is chewed and flavours food. Varnish, a fixative for perfume, and a tannin [PFAF]. **Resource:** SPC mastic and balsams (catalogue). **Lore:** Chewed by sailors to sweeten the breath before they lie to the harbour master. **Vocations:** Forester (resin tapper); Artist; Alchemist (perfumer).

**SH-15 Oleander** (*Nerium oleander*) · Real · WSh, HDs  
Fast evergreen shrub of about 4 m of river banks and gravels. The whole plant is very poisonous: one leaf has killed a child. A hedge and an insecticide [PFAF]. **Resource:** none. **Lore:** An army once roasted its meat on oleander spits, the story goes. The other army won without a fight. **Vocations:** Alchemist (poisoner); Gardener.

**SH-16 Henna** (*Lawsonia inermis*) · Real · HDs, Thn, TDf  
Evergreen shrub or small tree to 7 m of dry scrub and riverbeds. The leaves dye hair, skin and nails. The flowers give a perfume oil [PFAF]. **Resource:** DYE henna. **Lore:** A Saltroad bride's hands are painted with the route of her family's caravan (RB §3). **Vocations:** Gardener (dye-plant grower); Courtesan; Alchemist (perfumer).

**SH-17 Tea** (*Camellia sinensis*) · Real · TBl, TRn  
Slow evergreen shrub of about 4 m of shaded mountain forest. The leaf is drunk as a stimulant and an astringent. The seed gives an oil [PFAF]. **Resource:** STM tea (hills and mountains). **Lore:** Canal folk grow it on the hills above the paddies (RB §7). An official's rank shows in the leaf he is served. **Vocations:** Gardener (tea grower); Merchant; Innkeeper.

**SH-18 Coffee** (*Coffea arabica*) · Real · TDf, TRf  
Evergreen shrub of 1.5 to 3 m of the understorey of humid highland forest. The roasted seed is a stimulant drink [UTP]. In the generator it stands on tropical highlands. **Resource:** STM coffee (tropical highland). **Lore:** Saltroad coffee growers (RB §3) sell the bean and keep the husk for themselves. **Vocations:** Gardener (coffee grower); Merchant; Innkeeper.

**SH-19 Indigo** (*Indigofera tinctoria*) · Real · Thn, Sav, TDf  
Erect shrub of 1 to 2 m of the tropics. The leaves give a deep blue dye [UTP]. **Resource:** DYE indigo. **Lore:** The dyers' quarter is downstream and downwind, and its people are known by their forearms. **Vocations:** Gardener (dye-plant grower); Weaver (dyer).

**SH-20 Cotton** (*Gossypium herbaceum*) · Real · HDs, Thn, Sav, TDf  
Plant of about 1.5 m known only from cultivation. The seed floss is spun for cloth and used as stuffing. Twine, rope and paper. The seed gives an oil [UTP]. Pliny knew of trees that bear wool [PLIN-H 12.10-11]. **Resource:** FBR cotton. **Lore:** Rainkeeper cotton weavers (RB §6) clothe three peoples and pay the temple a tenth of every bolt. **Vocations:** Farmer; Weaver; Tailor; Merchant.

**SH-21 Mistletoe** (*Viscum album*) · Real · WSh, TMx, TBl  
Evergreen shrub about 1 m across that grows on the branches of poplar, apple and other trees. It slows the heart and calms. All parts are slightly poisonous [PFAF]. **Resource:** none. **Lore:** Pliny's druids cut it from an oak on the fifth day of the moon, a priest in white with a golden sickle, and catch it in a white cloak. Taken in drink it makes barren beasts fertile and is an antidote to all poisons [PLIN-B 16.95]. Frazer takes it for the golden bough [FRAZER 68]. **Vocations:** Priest; Healer (herbalist); Alchemist (antidote maker).

### 4.3 Flowers (12)

**FL-01 Arctic poppy** (*Papaver nudicaule*) · Real · Ice, Tun  
Short-lived perennial of about 0.3 m on stony slopes and river gravels of the far north. Flowers and capsules ease pain and scurvy. The flowers give red and beige dyes. Mildly toxic [PFAF]. **Resource:** none. **Lore:** It turns its face with the sun through the nightless summer. Hoarkin children are told it is counting the days for them. **Vocations:** Healer (herbalist); Weaver (dyer).

**FL-02 Fireweed** (*Epilobium angustifolium*) · Real · Tun, Bor, TMx  
Fast perennial of about 1.7 m on rocky and waste ground and woodland edges. Shoots, leaves and pith are eaten. The stems give cordage and the seed hairs tinder. An astringent for the gut and for burns [PFAF]. **Resource:** none. **Lore:** The first colour on burnt ground. Charcoal burners' camps are found by it the year after. **Vocations:** Hunter (forager); Healer (herbalist); Charcoal burner.

**FL-03 Tulip** (*Tulipa*) · Real · CSt, CDs, TGr  
Bulb with a few blue-green leaves and one bell-shaped flower, native to the inner steppe lands. Grown in gardens for a thousand years, and once the object of a trade mania [EB]. **Resource:** none. **Lore:** Grass riders pick the wild ones for a girl's hair. In Canal folk cities a striped bulb has been sold for a house. **Vocations:** Gardener; Merchant.

**FL-04 Sunflower** (*Helianthus annuus*) · Real · TGr  
Fast annual of 0.5 to 3 m of open dry plains. Seeds and their oil are eaten. A yellow dye from the flowers, a purple-black one from the seeds, and fibre from the stem [PFAF]. **Resource:** OCH sunflower. **Lore:** Noonfolk (RB §19), who live by the sun, plant it at the field corners and take its turning as a salute. **Vocations:** Farmer; Miller (oil presser); Weaver (dyer).

**FL-05 Coneflower** (*Echinacea angustifolia*) · Real · TGr  
Perennial of about 1.2 m of dry limestone prairie. The root is a remedy for sores, wounds and burns and against infection [PFAF]. **Resource:** HRB (proposal: a grassland variant). **Lore:** The Flinchless (RB §23), who do not feel their wounds, are taught to chew it whenever they see their own blood. **Vocations:** Healer (herbalist, surgeon).

**FL-06 Peony** (*Paeonia officinalis*) · Real · WSh, TMx, TBl  
Perennial of about 0.6 m of rocky woods and meadows on limestone. The root calms spasms and was given for the falling sickness. The whole plant is poisonous in quantity [PFAF]. **Resource:** none. **Lore:** Canal folk courts (RB §7) grow a hundred kinds, and an official is judged by his peony bed. **Vocations:** Gardener; Healer (physician).

**FL-07 Asphodel** (*Asphodelus albus*) · Real · WSh  
Perennial of about 1 m of dry hills, rough ground and mountain meadows. The tubers are a cough remedy, were once eaten, and are fermented for spirit. The root is poisonous raw [PFAF]. **Resource:** none. **Lore:** It covers the fields no one ploughs any more. The dead are said to walk in a meadow of it. **Vocations:** Brewer (distiller); Priest; Healer (herbalist).

**FL-08 Saffron crocus** (*Crocus sativus*) · Real · CSt, WSh  
Small corm about 0.1 m high, not known wild. The dried styles flavour and colour food and give a yellow dye. A sedative. Five to ten grams can kill [PFAF]. **Resource:** SPC saffron (catalogue). **Lore:** Picked before sunrise by the Swiftborn (RB §22). A pound is a hundred thousand flowers and a month of fingers. **Vocations:** Gardener (spice grower); Weaver (dyer); Merchant.

**FL-09 Dog rose** (*Rosa canina*) · Real · TGr, WSh, TBl  
Fast deciduous shrub of about 3 m of hedges, scrub and roadsides. The hips are taken against colds and scurvy and made into syrup, jam and tea. The petals are eaten [PFAF]. The scented damask rose was not verified. **Resource:** WLD (hips; not a catalogue variant). **Lore:** A rose over a door means the house keeps a secret. Over an inn, that it keeps everyone's. **Vocations:** Gardener; Healer (herbalist); Alchemist (perfumer); Innkeeper.

**FL-10 Lavender** (*Lavandula angustifolia*) · Real · WSh  
Slow evergreen shrub of about 1.2 m of dry, grassy, rocky slopes, often on chalk. The oil scents soap and perfume, calms and cleans wounds, and the flowers keep insects away [PFAF]. **Resource:** SPC lavender and rose oil (catalogue). **Lore:** Midlander washerwomen take their trade name from it, and their guild is older than the weavers'. **Vocations:** Gardener; Alchemist (perfumer); Servant.

**FL-11 Rose of Jericho** (*Anastatica hierochuntica*) · Real · HDs  
Small grey desert plant that curls its branches into a ball when dry and opens to 30 cm across when wetted [EB]. **Resource:** none. **Lore:** Saltroad midwives set a dry one in water when labour begins. The child is due when it has opened. **Vocations:** Healer (midwife); Seer.

**FL-12 Rafflesia** (*Rafflesia arnoldii*) · Real · TRf  
A parasite with no leaves, roots or stem, living as threads inside a forest vine. Only the flower shows: about 1 m across, up to 11 kg, smelling of carrion [EB]. **Resource:** none. **Lore:** Bitterbloods (RB §27) call it the honest flower: it smells like what it is. **Vocations:** Alchemist; Scholar.

### 4.4 Herbs (35)

**HE-01 Roseroot** (*Rhodiola rosea*) · Real · Tun, Bor  
Evergreen clumping perennial of about 0.3 m in the crevices of mountain rocks and sea cliffs of the cold north. A tonic against fatigue. Leaves, shoots and roots are eaten [PFAF]. **Resource:** HRB roseroot. **Lore:** Northmen crews chew it on the oar bench. A bride is given a root so that the house will have strong sons. **Vocations:** Healer (herbalist); Sailor.

**HE-02 Yarrow** (*Achillea millefolium*) · Real · Bor, TGr, TMx  
Upright perennial of about 0.6 m of meadows and pastures. Used for wounds and bleeding, colds and fevers. A hop substitute in beer, and a yellow and green dye [PFAF]. **Resource:** HRB yarrow. **Lore:** Soldier's woundwort. A Flinchless drill master (RB §23) will not march without a sack of it. **Vocations:** Healer (herbalist, surgeon); Brewer; Soldier.

**HE-03 Wormwood** (*Artemisia absinthium*) · Real · CSt, TGr  
Perennial of about 1 m of waste land, rocks and screes. A bitter tonic for the liver and digestion and a worm remedy. It drives off insects and mice. Poisonous in quantity [PFAF]. **Resource:** HRB wormwood. **Lore:** Strewn in a granary against mice, and steeped in the spirit that Orc hearths (RB §12) drink before a raid. **Vocations:** Healer (herbalist); Brewer (distiller); Farmer.

**HE-04 Liquorice** (*Glycyrrhiza glabra*) · Real · CSt, WSh  
Perennial to about 1.2 m of dry open places, spreading by rhizomes. The root sweetens and flavours and soothes coughs. Too much raises the blood pressure [PFAF]. **Resource:** HRB liquorice. **Lore:** Caravan children are paid in liquorice root, and so are informers. **Vocations:** Healer (herbalist); Brewer; Merchant.

**HE-05 Chamomile** (*Matricaria chamomilla*) · Real · CSt, TGr, TBl  
Annual to 0.5 m of sandy arable land and saline steppe. The flower tea calms the stomach and the nerves. A yellow dye and a hair rinse [PFAF]. **Resource:** HRB chamomile. **Lore:** The Halfling answer to every illness, and to most bad news. **Vocations:** Healer (herbalist); Innkeeper.

**HE-06 Sage** (*Salvia officinalis*) · Real · WSh  
Upright evergreen shrub of about 0.6 m of dry banks and stony ground. Antiseptic and astringent, a gargle. It flavours food. Toxic in excess [PFAF]. **Resource:** HRB sage. **Lore:** "Why should he die who has sage in his garden" is a Midlander proverb and a Saltroad joke. **Vocations:** Healer (herbalist); Butcher; Gardener.

**HE-07 Thyme and rosemary** (*Thymus vulgaris; Rosmarinus officinalis*) · Real · WSh  
Evergreen shrubs of dry slopes, rocks and scrub near the sea, thyme a mat of 0.2 m and rosemary a bush of 1.5 m. They flavour food. Thyme is a remedy for coughs, rosemary a tonic for low spirits. Their oils go into soap and perfume [PFAF]. **Resource:** HRB thyme and rosemary. **Lore:** Rosemary is carried at weddings and funerals. The same sprig will do for both, say the Swiftborn (RB §22), who have little time between. **Vocations:** Healer (herbalist); Butcher; Alchemist (perfumer); Gardener (beekeeper).

**HE-08 Opium poppy** (*Papaver somniferum*) · Real · TGr, WSh  
Annual of about 0.6 m, not known wild. The dried latex kills pain and brings sleep and is highly addictive. The seeds and their oil are eaten, and the oil serves for lamps and paint [PFAF]. **Resource:** HRB poppy. **Lore:** The surgeon's friend and the debtor's ruin. In many towns only the temple may sell it. **Vocations:** Healer (physician, surgeon); Alchemist (apothecary); Priest; Thief.

**HE-09 Valerian** (*Valeriana officinalis*) · Real · TMx, TBl, TRn  
Perennial of about 1.5 m of grassland, scrub and woods. The root is a tranquilliser, for sleep and cramps. Long use becomes a habit [PFAF]. **Resource:** HRB valerian. **Lore:** Cats will follow whoever carries it. Catfolk (RB §28) consider the joke old. **Vocations:** Healer (herbalist); Alchemist (apothecary).

**HE-10 St John's wort** (*Hypericum perforatum*) · Real · TGr, TMx, TBl  
Perennial of about 0.9 m of open woods, hedge banks and dry grassland. Taken against low spirits. Flowers and leaves give yellow, gold, brown and red dyes. It makes the skin burn in sunlight [PFAF]. **Resource:** HRB St John's wort. **Lore:** Gathered at midsummer and hung in the byre against fire and worse. **Vocations:** Healer (herbalist); Weaver (dyer); Seer (wise woman).

**HE-11 Foxglove** (*Digitalis purpurea*) · Real · TBl, TRn  
Biennial of about 1.2 m of acid soils in woods and on heaths. The leaves act on the heart. The dose that heals is close to the dose that kills [PFAF]. **Resource:** HRB foxglove. **Lore:** A physician is someone who knows how much. A poisoner is someone who knows a little more. **Vocations:** Healer (physician); Alchemist (poisoner).

**HE-12 Comfrey** (*Symphytum officinale*) · Real · TBl  
Fast upright perennial of about 1.2 m of damp meadows, woods and streamsides. Laid on cuts, bruises, sprains and broken bones. Taken inwardly, it harms the liver [PFAF]. **Resource:** HRB comfrey. **Lore:** "Knitbone" in every language of the Midlands. **Vocations:** Healer (bonesetter, herbalist); Gardener.

**HE-13 Angelica** (*Angelica archangelica*) · Real · Bor, TMx, TRn  
Biennial of about 1.5 m of moist shady places. Stalks, leaves and roots are eaten and the seed flavours drink. A remedy for the digestion and the circulation [PFAF]. **Resource:** HRB angelica. **Lore:** Northmen candy the stalks for the winter feast. It is the only sweet thing in the house until the bees wake. **Vocations:** Healer (herbalist); Brewer (distiller); Gardener.

**HE-14 Gentian and arnica** (*Gentiana lutea; Arnica montana*) · Real · Bor, TGr, TMx  
Perennials of high mountain pastures. The gentian root is a bitter tonic for the digestion and once flavoured beer. Arnica is laid on bruises and sprains, on unbroken skin only: swallowed, it can kill [PFAF]. **Resource:** HRB gentian, arnica (mountains). **Lore:** Dwarf brewers (RB §9) bitter their ale with gentian. Avalanche rescuers (RB §14) carry arnica. **Vocations:** Healer (herbalist, bonesetter); Brewer.

**HE-15 Nettle** (*Urtica dioica*) · Real · TGr, TMx, TBl  
Fast perennial of about 1.2 m of hedgerows, woods and rich waste ground. Young leaves are eaten as greens. The stem fibre makes string and cloth. Green and yellow dyes. The sting goes with cooking [PFAF]. **Resource:** FBR nettle and lime-bast. **Lore:** Shirts of nettle cloth were what the poor wore through the Ashen Years, when the flax failed. **Vocations:** Weaver; Hunter (forager); Healer (herbalist).

**HE-16 Woad** (*Isatis tinctoria*) · Real · TGr, TBl  
Biennial of about 1 m of cliffs and cornfields, often on chalk. The leaves give a blue dye and preserve wood [PFAF]. **Resource:** DYE woad. **Lore:** The blue of the north. Northmen sworn guards (RB §1) are painted with it before a fight they do not expect to leave. **Vocations:** Gardener (dye-plant grower); Weaver (dyer).

**HE-17 Madder** (*Rubia tinctorum*) · Real · TGr, WSh  
Evergreen perennial of about 1 m of hedgerows and neglected ground. The roots give a red dye. The prickly leaves polish metal [PFAF]. **Resource:** DYE madder. **Lore:** Soldiers' red. A realm's madder fields are counted with its armouries. **Vocations:** Gardener (dye-plant grower); Weaver (dyer); Smith.

**HE-18 Flax** (*Linum usitatissimum*) · Real · TGr, TMx, TBl  
Annual of about 0.7 m, not known wild. The stem fibre makes cloth, sails, nets and paper. The seed is eaten, and its oil goes into paint and varnish [PFAF]. **Resource:** FBR flax. **Lore:** A Midlander girl spins her own shroud first, so that everything she spins afterwards is profit. **Vocations:** Farmer; Weaver (linen weaver); Miller (oil presser).

**HE-19 Hemp** (*Cannabis sativa*) · Real · CSt, TGr, TBl  
Annual of about 2.5 m of rich soils near houses. Fibre for cloth and rope. Seed and oil are eaten. It eases pain and sickness and can intoxicate [PFAF]. **Resource:** FBR hemp. **Lore:** Every hull that carries Northmen carries a field of hemp in its rigging. **Vocations:** Farmer; Weaver (rope maker); Shipwright; Healer.

**HE-20 Ginseng** (*Panax ginseng*) · Real · TMx  
Perennial of about 0.8 m of mountain forests. The root is a tonic against weakness, sleeplessness and strain [PFAF]. **Resource:** HRB (proposal: a mixed-forest variant). **Lore:** A root shaped like a man is worth his weight in silver. Canal folk physicians (RB §7) pay Amberblood diggers and ask no questions. **Vocations:** Hunter (forager); Healer (physician); Merchant.

**HE-21 Monkshood** (*Aconitum napellus*) · Real · Bor, TMx  
Perennial of about 1.5 m of damp shady places and moist meadows. Highly toxic to the nerves: even touching it numbs the skin. The root was used outwardly against pain [PFAF]. **Resource:** HRB (as a poison). **Lore:** Wolfsbane: herders of the north tip their wolf arrows with it and bury the wolf where it falls. **Vocations:** Alchemist (poisoner); Hunter; Herder.

**HE-22 Deadly nightshade** (*Atropa belladonna*) · Real · WSh, TBl  
Perennial of about 0.9 m of woods, thickets and hedges on limy ground. The whole plant is very poisonous, most of all the root and the ripe berries. It widens the pupils [PFAF]. **Resource:** HRB (as a poison). **Lore:** Courtesans drop it in the eyes to look wide awake, and in the cup to make sure the other does not. **Vocations:** Alchemist (poisoner); Courtesan; Healer (physician).

**HE-23 Hemlock** (*Conium maculatum*) · Real · TGr, WSh, TBl  
Biennial of about 2 m of waste ground and damp places. Very poisonous, the seeds most: death comes by suffocation. Once given as a sedative and against pain [PFAF]. **Resource:** HRB (as a poison). **Lore:** The cup that the old city courts offered a condemned citizen, as a courtesy. **Vocations:** Alchemist (poisoner); Official.

**HE-24 Rhubarb** (*Rheum palmatum*) · Real · CSt, CDs  
Clumping perennial of up to 3 m of scrub, rocky ground and streamsides at high elevation. The root is a laxative and a tonic for the digestion. The leaves are poisonous [PFAF]. **Resource:** HRB (proposal: a steppe variant). **Lore:** It crosses half the world by caravan, and each hand that carries it doubles the price. **Vocations:** Healer (physician); Merchant; Carter (caravan driver).

**HE-25 Glasswort** (*Salicornia europaea*) · Real · CDs, Mgv, Sea  
Annual of about 0.3 m of coastal sands, mudflats and salt marsh. The young stems are a pot herb. The ash is rich in soda, for soap and glass [PFAF]. The placement on inland salt flats is my inference. **Resource:** RGT (soda ash; compare SAL natron and soda). **Lore:** Brinefolk soda burners and glassmakers (RB §17) cut it by the cartload at the autumn low tides. **Vocations:** Salter; Glassmaker; Charcoal burner (ash burner).

**HE-26 Clover and lucerne** (*Trifolium pratense; Medicago sativa*) · Real · TGr, TBl  
Perennials of meadows and pastures, lucerne with a deep taproot. They feed the soil and are dug in as green manure. Leaves and sprouts are eaten. A yellow dye [PFAF]. **Resource:** LVS alfalfa (catalogue). **Lore:** "Clover in, wheat out" is the whole of Halfling farming as they explain it to others. **Vocations:** Farmer; Herder; Gardener (beekeeper).

**HE-27 Silphium** (*Ferula*) · Real-derived · WSh, HDs  
A herb of one dry coast whose juice was prized as medicine and perfume and sold for its weight in silver. It vanished when the land was let for sheep, and the last stalk found was sent to the emperor as a curiosity [PLIN-B 19.15]. Extinct in our world. Its relative the giant fennel is a perennial of 2.5 m of dry hills whose gum is burnt as incense and whose pith is a slow tinder [PFAF]. **Resource:** HRB (proposal: a rare shrubland variant, gated like the luxury crops). **Lore:** Here it survives in a few zones. The Saltroad clans that hold them do not say which. **Vocations:** Hunter (forager); Healer (physician); Alchemist (apothecary, perfumer); Merchant.

**HE-28 Dittany** (*Origanum dictamnus*) · Real · WSh  
Perennial of about 0.2 m of shady rocks in dry high mountains, native to one island. Once a wound herb. The leaves flavour drink and make a tea [PFAF]. **Resource:** HRB (proposal: a mountain variant of the shrubland). **Lore:** Wounded goats are said to seek it out, and goatherds to follow the goats. **Vocations:** Herder (goatherd); Healer (herbalist); Brewer.

**HE-29 Aloe** (*Aloe vera*) · Real · HDs, Thn  
Evergreen succulent of 0.6 to 1 m of maritime sands and rocks. The leaf gel is laid on wounds and burns. The sap is a laxative [PFAF]. **Resource:** HRB aloe. **Lore:** Every caravan cook grows one in a pot on the lead camel. **Vocations:** Healer (herbalist); Carter (caravan driver).

**HE-30 Senna** (*Senna alexandrina*) · Real · HDs, Thn, Sav  
Deciduous shrub to 3 m of semi-desert scrub, grassland and riverbanks. Leaves and pods are an old purge [UTP]. **Resource:** HRB senna. **Lore:** Sold in the night markets of the Duskreaders (RB §18) next to the dates, as its remedy. **Vocations:** Healer (herbalist); Merchant.

**HE-31 Devil's claw** (*Harpagophytum procumbens*) · Real · Thn, Sav  
Sprawling perennial with fleshy tubers, of veld and waste ground on clay or sand. The bitter root eases pain and swelling. The spiny fruits lame the feet [UTP]. **Resource:** HRB devil's claw. **Lore:** Sunlander herders dig the root for their own joints and curse the fruit for their cattle's hooves. **Vocations:** Healer (herbalist); Herder.

**HE-32 Holy basil** (*Ocimum tenuiflorum*) · Real · TDf  
Aromatic perennial of 0.3 to 1 m with a woody base. A remedy for fevers, colds and the digestion. The stems are cut into prayer beads, and the oil repels insects [UTP]. **Resource:** HRB holy basil. **Lore:** A pot of it stands in every Rainkeeper courtyard. The house is not a house until it is planted. **Vocations:** Healer (herbalist); Priest; Gardener.

**HE-33 Ipecac** (*Carapichea ipecacuanha*) · Real · TRf  
Shrub of about 0.5 m of the rain forest understorey. The root brings up vomit and phlegm and is given for coughs and dysentery. An overdose is dangerous [UTP]. **Resource:** HRB ipecac. **Lore:** What a Bitterblood antidote maker (RB §27) reaches for first: most poisons are best met coming back up. **Vocations:** Alchemist (antidote maker); Healer (physician).

**HE-34 Ginger and turmeric** (*Zingiber officinale; Curcuma longa*) · Real · TDf, TRf  
Perennial herbs of about 1 m with spreading rhizomes, known only from cultivation. Ginger flavours food and settles the stomach. Turmeric is a spice and a yellow to orange dye for silk and wool [UTP]. **Resource:** SPC ginger, turmeric. **Lore:** Rainkeeper brides are rubbed yellow with turmeric (RB §6 lists the dyer and the spice grower). **Vocations:** Gardener (spice grower); Weaver (dyer); Healer (herbalist).

**HE-35 Thorn apple** (*Datura metel*) · Real · HDs, Thn, TDf  
Erect annual to 2 m of waste places and sandy riverbanks. All parts are poisonous and narcotic. Given for asthma and pain and before surgery. The gap between dose and death is small [UTP]. **Resource:** HRB (as a poison). **Lore:** A Sunlander diviner (RB §4) takes it once, at initiation, and speaks of what he saw for the rest of his life. **Vocations:** Seer (diviner); Alchemist (poisoner); Healer (surgeon); Thief.

### 4.5 Fruit (31)

**FR-01 Cloudberry and crowberry** (*Rubus chamaemorus; Empetrum nigrum*) · Real · Tun, Bor  
Low plants of about 0.3 m of cool bogs, moors and mountain tops. Both fruits are eaten raw or cooked and give a purple dye [PFAF]. **Resource:** WLD cloudberry and crowberry. **Lore:** Cloudberry bogs are owned by families in Northmen law, and trespass in berry time is paid for in silver. **Vocations:** Hunter (forager); Gardener.

**FR-02 Bilberry** (*Vaccinium myrtillus*) · Real · Tun, Bor, TMx  
Suckering shrub of about 0.2 m of heaths, moors and woods on acid soil. Sweet fruit for eating and preserves. A blue or black dye and an ink from the fruit [PFAF]. **Resource:** WLD bilberry and blueberry. **Lore:** Children come home blue to the elbows, and the scribe buys what they did not eat. **Vocations:** Hunter (forager); Scribe; Weaver (dyer).

**FR-03 Raspberry and bramble** (*Rubus idaeus*) · Real · TMx, TBl, TRn  
Suckering shrub of about 2 m of hedgerows, woodland edges and neglected land. Fruit, shoots and a leaf tea that is given in childbirth. A purple-blue dye [PFAF]. **Resource:** WLD raspberry and bramble. **Lore:** The hedge that feeds the poor. No lord has managed to tax it. **Vocations:** Hunter (forager); Healer (midwife).

**FR-04 Salmonberry** (*Rubus spectabilis*) · Real · TRn  
Deciduous shrub of about 1.8 m of moist places in and around woods on the wet coast. Fruit and peeled shoots are eaten. The leaves and root are astringent [PFAF]. **Resource:** WLD raspberry and bramble. **Lore:** It ripens when the first salmon run, and is eaten with them. **Vocations:** Hunter (forager); Fisher.

**FR-05 Sea buckthorn** (*Hippophae rhamnoides*) · Real · CSt, CDs, Sea  
Deciduous thorny shrub of about 6 m of coasts, dunes and sea cliffs, and inland across Asia. The sour fruit and its oil are a tonic and are laid on burns. Yellow and brown dyes, fuel and charcoal. It holds loose soil [PFAF]. **Resource:** WLD sea buckthorn (catalogue). **Lore:** Grass riders feed it to tired horses, and their word for it means "shining horse". **Vocations:** Hunter (forager); Healer (herbalist); Herder.

**FR-06 Hazel** (*Corylus avellana*) · Real · TMx, TBl  
Deciduous shrub or tree of about 6 m of woods and hedgerows. Nuts and their oil. The rods make baskets. Charcoal [PFAF]. **Resource:** OCH hazel. **Lore:** A forked hazel rod finds water and ore in the right hands. Every well-finder owns one, and no two agree on how it is held. **Vocations:** Gardener; Weaver (basket maker, assumed); Seer (dowser, well-finder).

**FR-07 Apple** (*Malus domestica*) · Real · TMx, TBl  
Deciduous tree of about 9 m, not known truly wild. The fruit is eaten raw, cooked and dried. The wood is turned and burnt. The seeds are poisonous in quantity [PFAF]. **Resource:** OCH apple. **Lore:** Halflings (RB §10) keep an orchard behind every burrow and sell cider to people who have never seen a Halfling sober or drunk. **Vocations:** Gardener (orchard keeper); Brewer (cider maker).

**FR-08 Sweet chestnut** (*Castanea sativa*) · Real · WSh, TBl  
Deciduous tree to about 30 m of mountain woods. The nuts are eaten roasted or ground to flour. Good fuel, fence posts and carpentry. The bark tans [PFAF]. **Resource:** OCH sweet chestnut. **Lore:** The bread tree of hill villages that have no ploughland. **Vocations:** Gardener; Miller; Carpenter; Tanner.

**FR-09 Walnut** (*Juglans regia*) · Real · CSt, WSh, TBl  
Rounded deciduous tree of about 20 m. Nuts and oil. Dyes and a tannin. A fine timber and a drying oil for paint [PFAF]. **Resource:** OCH walnut. **Lore:** Planted at a son's birth for the furniture of his wedding. Nothing grows under it, and nobody sleeps there twice. **Vocations:** Gardener; Carpenter; Weaver (dyer); Artist.

**FR-10 Olive** (*Olea europaea*) · Real · WSh  
Slow evergreen tree of about 10 m of woods and scrub in dry rocky places. The cured fruit is eaten. The oil is food, medicine, soap, lamp fuel and lubricant. A hard wood for turning [PFAF]. **Resource:** OCH olive. **Lore:** A grove outlives the realm that planted it. Swiftborn pickers (RB §22) were made for the harvest and are still paid by it. **Vocations:** Gardener (olive grower); Miller (oil presser); Merchant.

**FR-11 Fig** (*Ficus carica*) · Real · WSh, HDs  
Deciduous tree of about 6 m among rocks and scrub on hot dry soils. The fruit is eaten fresh and dried and loosens the bowels. The latex curdles milk and burns the eyes [PFAF]. **Resource:** OCH fig. **Lore:** The traveller's tree: it fruits twice and asks for nothing. To cut one down by a road is a curse. **Vocations:** Gardener; Innkeeper.

**FR-12 Pomegranate** (*Punica granatum*) · Real · WSh, HDs, Thn  
Deciduous shrub or small tree of 4 to 10 m on dry limestone soils. The fruit and juice are eaten. Rind and flowers give a red dye, an ink and a tannin. The bark kills tapeworm and can kill the patient [PFAF]. **Resource:** OCH pomegranate. **Lore:** Whoever eats in the house of the dead stays there, the old stories say. Saltroad hosts offer one to seal a debt. **Vocations:** Gardener; Tanner; Weaver (dyer); Scribe.

**FR-13 Almond** (*Prunus dulcis*) · Real · CSt, WSh  
Deciduous tree of about 6 m of thickets and rocky places. The seeds are eaten raw, roasted and ground, and give an oil for soap and the skin. The bitter kind is poisonous [PFAF]. **Resource:** OCH almond, pistachio. **Lore:** The first blossom of the year. Poisoners prize the bitter tree that looks the same. **Vocations:** Gardener; Baker; Alchemist (poisoner).

**FR-14 Carob** (*Ceratonia siliqua*) · Real · WSh  
Evergreen tree of about 15 m of rocky places near the sea. The sweet pods are eaten or ground and feed stock. The seed gives a flour and a gum [PFAF]. **Resource:** OCH carob (catalogue). **Lore:** Jewellers weigh gems against its seeds, which are all of one weight, or so the jewellers say. **Vocations:** Gardener; Herder; Jeweller.

**FR-15 Citron** (*Citrus medica*) · Real · WSh, TBl, TDf  
Evergreen shrub or small tree to 4 m, known only in cultivation. The fruit and its candied peel are eaten. A perfume, and a repellent of moths [UTP]. **Resource:** OCH citrus. **Lore:** A ship that carries them loses no teeth, and Canal folk captains guard the knowledge like a chart. **Vocations:** Gardener (orchard keeper); Sailor; Alchemist (perfumer).

**FR-16 Date palm** (*Phoenix dactylifera*) · Real · HDs  
Evergreen palm of 25 to 30 m, not known truly wild. The fruit is a staple and a sweetener. Sap, thatch, fibre for rope, baskets and mats, wood and fuel [PFAF]. **Resource:** OCH date palm (needs a river). **Lore:** The Saltroad date gardener (RB §3) counts his palms as a herder counts cattle, and knows each by the taste. **Vocations:** Gardener (date gardener); Weaver (rope maker); Mason (thatcher).

**FR-17 Prickly pear** (*Opuntia ficus-indica*) · Real · WSh, HDs, Thn  
Evergreen cactus to about 5 m of dry, rocky places. The sweet fruit and the cooked pads are eaten. The pads feed stock, and the plant makes a fence. The fine barbed spines stay in the skin [PFAF]. It is the host of the cochineal insect [EB]. **Resource:** OCH prickly pear (catalogue); DYE cochineal (the host). **Lore:** A hedge that feeds the goats, the children and the dyer. **Vocations:** Gardener (dye-plant grower); Herder; Farmer.

**FR-18 Jujube** (*Ziziphus jujuba*) · Real · CSt, HDs, Thn  
Fast, often thorny deciduous tree of about 10 m on dry stony slopes. The fruit is eaten fresh and dried and is a tonic for sleep. Fuel, charcoal and hedging [PFAF]. **Resource:** OCH jujube (catalogue). **Lore:** Travellers who eat it in a strange land are said to forget the way home. The innkeepers who serve it do not deny this. **Vocations:** Gardener; Innkeeper; Healer (herbalist).

**FR-19 Watermelon** (*Citrullus lanatus*) · Real · HDs, Thn, Sav  
Trailing annual with stems to 4 m of open ground in desert country. The fruit is water and food. The seeds are roasted and pressed for oil [UTP]. **Resource:** PRD melons. **Lore:** The wild kind is the hunter's well. Duskreaders (RB §18) find the cool fruit in the dark as others find a lamp. **Vocations:** Farmer; Hunter (forager).

**FR-20 Shea** (*Vitellaria paradoxa*) · Real · Sav  
Deciduous tree of 7 to 15 m with a dense spreading crown, of open parkland savanna. The seed fat is shea butter. Timber and charcoal [UTP]. **Resource:** OCH shea. **Lore:** The women's tree: the nuts and the butter money are theirs by Sunlander custom. **Vocations:** Gardener; Miller (oil presser); Merchant.

**FR-21 Tamarind** (*Tamarindus indica*) · Real · Sav, TDf  
Slow tree of 20 to 30 m of low woodland, savanna and riverbanks, often by termite mounds. Pods, leaves and seeds are eaten. Timber, charcoal, a tannin and a leaf dye [UTP]. **Resource:** OCH tamarind. **Lore:** The sour pulp polishes brass, and temple lamps are cleaned with nothing else. **Vocations:** Gardener; Innkeeper; Smith.

**FR-22 Mango** (*Mangifera indica*) · Real · TDf, TRf  
Large evergreen of 10 to 45 m with an umbrella crown. Fruit. Timber for building and boats. Bark and leaves dye silk. The sap can raise a rash [UTP]. **Resource:** OCH mango. **Lore:** A Rainkeeper king is remembered by the mango groves he planted along the roads. **Vocations:** Gardener (orchard keeper); Carpenter.

**FR-23 Banana and plantain** (*Musa acuminata*) · Real · TDf, TRf  
Evergreen giant herb of about 3 m of moist ravines and slopes. Fruit, flower and stem core are eaten. Fibre for cloth, rugs and paper. The leaves wrap food [UTP]. **Resource:** PRD banana and plantain. **Lore:** The plate, the roof in a shower and the meal are the same plant. **Vocations:** Farmer; Weaver; Innkeeper.

**FR-24 Coconut** (*Cocos nucifera*) · Real · TDf, TRf, Mgv  
Single-stemmed palm to 30 m of tropical seashores. The seed gives food, milk and oil. The husk fibre makes mats and rope. Timber, and charcoal from the shell [UTP]. **Resource:** OCH coconut (needs a coast). **Lore:** It floats to a new shore by itself. Shoalfolk (RB §21) claim a beach by planting three. **Vocations:** Gardener; Weaver (rope maker); Sailor.

**FR-25 Cacao** (*Theobroma cacao*) · Real · TRf  
Small evergreen tree of about 8 m of the understorey of wet lowland rain forest. The seeds give cocoa and a fat [UTP]. **Resource:** STM cacao. **Lore:** Boughwalkers (RB §20) drink it bitter and sell it to people who add honey and call it a luxury. **Vocations:** Gardener; Merchant; Innkeeper.

**FR-26 Kola nut** (*Cola nitida*) · Real · TRf  
Evergreen understorey tree of 9 to 12 m of lowland forest. The seeds are a stimulant, chewed or made into drink. Timber for furniture and boats [UTP]. **Resource:** STM kola nut. **Lore:** Split and shared before any Sunlander bargain. To refuse the half is to refuse the deal. **Vocations:** Gardener; Merchant; Official.

**FR-27 Oil palm** (*Elaeis guineensis*) · Real · TRf  
Single-stemmed palm of 20 to 30 m of open forest on wet sandy soils by rivers. Palm oil and palm wine. The leaflets give rope fibre and the shells fuel a furnace [UTP]. **Resource:** OCH oil palm. **Lore:** Sunlander smelters (RB §4) fire their furnaces with the shells and oil their iron with the fruit. **Vocations:** Gardener; Miller (oil presser); Brewer; Smelter.

**FR-28 Brazil nut** (*Bertholletia excelsa*) · Real · TRf  
Very large evergreen of 25 to 56 m on deep alluvial soil that rarely floods. The seeds are eaten. Durable timber [UTP]. **Resource:** OCH Brazil nut. **Lore:** The fruit falls like a thrown stone. Gatherers work in wide hats and short visits. **Vocations:** Hunter (forager); Merchant.

**FR-29 Areca palm** (*Areca catechu*) · Real · TDf, TRf  
Single-stemmed palm of 15 to 30 m of the understorey of wet forest. The seed is chewed as a stimulant. A tannin for dyeing. Too much brings vomiting and stupor [UTP]. **Resource:** STM betel nut. **Lore:** Offered to every guest in Rainkeeper country. The red spit on a road tells how far the next town is. **Vocations:** Gardener; Innkeeper; Merchant.

**FR-30 Sago palm** (*Metroxylon sagu*) · Real · TRf, Mgv  
Clumping palm of 6 to 25 m of lowland freshwater swamp. The pith of the stem is washed for starch, and a stem dies after it flowers. The leaves thatch [UTP]. **Resource:** PRD sago palm. **Lore:** One trunk feeds a family for a month, and Bitterbloods (RB §27) farm nothing else. **Vocations:** Farmer; Mason (thatcher).

**FR-31 Palmyra palm** (*Borassus flabellifer*) · Real · Sav, TDf  
Very tall single-stemmed palm of 20 to 30 m of hot dry monsoon country on sandy and alluvial soil. Sap, fruit and young seeds are food. Timber, fibre and thatch [UTP]. **Resource:** STM palm sugar and toddy. **Lore:** The leaf is the Rainkeeper scribe's page, scratched with a stylus and rubbed with soot. **Vocations:** Brewer; Scribe; Mason (thatcher).

### 4.6 Grasses, sedges and reeds (12)

**GR-01 Cottongrass** (*Eriophorum angustifolium*) · Real · Tun, Bor  
Sedge of about 0.6 m of peat bogs, acid meadows and marshes. The white seed hairs make candle wicks, tinder and pillow stuffing. The leaves are woven into mats [PFAF] [EB-TUN]. **Resource:** none. **Lore:** When the bog turns white, the reindeer calves have been born and the herders may move. **Vocations:** Herder (reindeer herder); Weaver.

**GR-02 Feather grass** (*Stipa*) · Real · CSt, TGr  
The plumed bunch grass that gives the steppe its silver sheen in the wind. General knowledge **[unverified]**: no page on it could be opened. **Resource:** LVS (the pasture of the steppe). **Lore:** Grass riders say the steppe has a pelt, and that it should be stroked the right way: they ride with the wind when they can. **Vocations:** Herder.

**GR-03 Tallgrass** (*Andropogon gerardii*) · Real · TGr  
Clump-forming perennial grass about 2 m tall of prairies, open ground and open woods. Forage and hay for horses and cattle [PFAF]. **Resource:** LVS cattle; HOR horses (their pasture). **Lore:** A rider can be lost in it standing. Widegaze (RB §15) build their mounds to see over it. **Vocations:** Herder; Farmer.

**GR-04 Wild barley** (*Hordeum spontaneum*) · Real · CSt, TGr, WSh  
The wild grass from which barley was bred about ten thousand years ago. Barley resists dry heat, grows from the subarctic to the desert edge, and is fodder, malt for beer, and bread and porridge [EB]. **Resource:** GRN barley (its wild kin). **Lore:** Gnome seedsmen (RB §11) keep the wild grass in sealed jars beside the tame, in case the tame is lost again. **Vocations:** Farmer; Brewer (maltster); Gardener (seedsman).

**GR-05 Esparto** (*Stipa tenacissima*) · Real · WSh, HDs  
Grass of 1 to 1.2 m with clustered wiry stems, of dry sandy soil near southern coasts. Fibre for ropes, sandals, baskets and mats. The leaves make paper. Young plants are forage [EB]. **Resource:** FBR esparto grass. **Lore:** The sandals of every Saltroad porter, and the rope of every well. **Vocations:** Weaver (rope maker); Leatherworker; Scribe.

**GR-06 Reed and cattail** (*Phragmites australis; Typha latifolia*) · Real · Bor, TGr, WSh, TMx, TBl, TRn, Mgv  
Tall perennials of 1.5 to 3.6 m of shallow water and wet ground, found almost worldwide. Reed thatches and makes baskets, paper and string. Cattail gives starchy roots, mats, tinder, rush lights and stuffing [PFAF]. **Resource:** WLD cattail (catalogue). **Lore:** A marsh village owes its lord reeds, not grain, and thatches his hall every seventh year. **Vocations:** Mason (thatcher); Weaver (basket maker, assumed); Hunter (forager, fowler).

**GR-07 Papyrus** (*Cyperus papyrus*) · Real · HDs, Sav, TRf  
Clump-forming sedge to 5 m in great stands in swamps, shallow lakes and along streams. Paper, boats, cordage, sandals, mats and huts. Rhizomes and stems are eaten [UTP]. Herodotus has the marsh people eat the lower stalk baked [HDT 2.92]. **Resource:** FBR (paper; not a catalogue variant). **Lore:** The archives of the Old Realms were written on it, and what survives of them survives where it is dry. **Vocations:** Scribe; Weaver; Shipwright; Fisher.

**GR-08 Bamboo** (*Phyllostachys edulis*) · Real · TBl, TRn, TDf, TRf  
Fast evergreen giant grass of about 8 m, of woodland and mountain slopes. The young shoots are eaten. Timber, water pipes, weaving and paper [PFAF]. **Resource:** TMB (minor); PRD (shoots). **Lore:** Canal folk scaffolding, water pipes and brush handles (RB §7). It flowers once in a lifetime, and a famine follows the rats that follow the seed. **Vocations:** Carpenter; Mason; Weaver (basket maker, assumed); Scribe.

**GR-09 Elephant grass** (*Pennisetum purpureum*) · Real · Sav, TRf  
Clumping grass with cane-like stems of 3 to 7 m, of damp grassland and forest edges. Forage. Fences, huts and thatch [UTP]. **Resource:** LVS cattle (their pasture). **Lore:** The grass that hides the elephant and the lion. Sunlander herders burn it at the end of the dry season and walk behind the fire. **Vocations:** Herder; Mason (thatcher); Hunter.

**GR-10 Sorghum** (*Sorghum bicolor*) · Real · Thn, Sav  
Vigorous annual grass up to 5 m of the semi-arid tropics. Grain and flour, and a syrup from the sweet stems. Brooms, fencing, mats and roofing. The young or wilted plant is poisonous to stock [UTP]. **Resource:** GRN sorghum. **Lore:** Millet and sorghum are the Sunlander grains (RB §4), and the beer of every funeral. **Vocations:** Farmer; Brewer; Miller.

**GR-11 Sugar cane** (*Saccharum officinarum*) · Real · TDf, TRf  
Large perennial grass with canes of 2 to 3 m and more, known only from cultivation. Sugar, syrup and molasses. The stem is chewed [UTP]. **Resource:** STM sugar cane. **Lore:** White sugar is a Rainkeeper secret and a physician's drug everywhere else. **Vocations:** Farmer; Miller; Merchant.

**GR-12 Wild rice** (*Oryza rufipogon*) · Real · TDf, TRf, Mgv  
Perennial grass to 2 m of swamps, pond and stream edges and ditches, in water to 30 cm. The seed is cooked like rice and is a famine food. It crosses with the crop and is a weed of the paddy [UTP]. **Resource:** GRN paddy rice (its wild kin); wild rice (catalogue). **Lore:** Rainkeeper reservoir keepers (RB §6) weed it out of the paddies and eat it in bad years. **Vocations:** Farmer (paddy farmer); Hunter (forager).

### 4.7 Vines and climbers (9)

**VI-01 Hops** (*Humulus lupulus*) · Real · TMx, TBl  
Fast perennial climber of about 7 m of riverbanks and moist thickets. The cones flavour beer and bring sleep. Young shoots are eaten. Stem fibre for cloth and paper [PFAF]. The page read describes the plant from another continent's range. **Resource:** HRB hops. **Lore:** The line between ale lands and beer lands is the hop line, and brewers have fought over it. **Vocations:** Brewer; Gardener; Healer (herbalist).

**VI-02 Grape vine** (*Vitis vinifera*) · Real · WSh, TBl  
Fast deciduous climber of about 15 m of riversides and damp woods. The fruit is eaten fresh and dried and made into wine. The leaves are eaten. Seed oil and cream of tartar [PFAF]. **Resource:** OCH grape. **Lore:** The vineyards of the Old Realms went wild in the Ashen Years (RB §22). The Swiftborn were made to dress them. **Vocations:** Gardener (vine dresser); Brewer (wine maker); Merchant.

**VI-03 Black pepper** (*Piper nigrum*) · Real · TRf  
Woody climber to 10 m with clinging roots. The dried fruit is black and white pepper. A remedy for the digestion and for fever [UTP]. Pliny knew the long, the white and the black kind and their prices [PLIN-H 12.7]. **Resource:** SPC black pepper. **Lore:** Counted out by the corn in northern markets, and taken as rent. **Vocations:** Gardener (spice grower); Merchant.

**VI-04 Vanilla** (*Vanilla planifolia*) · Real · TRf  
Climbing orchid with a succulent stem of 5 to 15 m and aerial roots, of warm wet lowland forest. The cured seed pods flavour sweet food and scent perfume [UTP]. **Resource:** SPC vanilla. **Lore:** Each flower opens for one morning and is fertilised by hand. Boughwalker fingers (RB §20) are the right length. **Vocations:** Gardener (spice grower); Alchemist (perfumer).

**VI-05 Rattan** (*Calamus rotang*) · Real · TDf, TRf  
Spiny climbing palm with stems as thick as a finger that pass 30 m. The stems make baskets, furniture, rope and hanging bridges. The shoots are eaten [UTP]. **Resource:** FBR (cane; not a catalogue variant). **Lore:** The Boughwalker high roads (RB §20) are rattan, renewed strand by strand so that no bridge is ever old or new. **Vocations:** Weaver (basket maker, assumed; rope maker); Carpenter.

**VI-06 Curare vine** (*Chondrodendron tomentosum*) · Real · TRf  
Woody climber with stems to 30 m into the canopy. The source of an arrow poison that kills by stopping the breath, and only if it enters the blood: swallowed, it does no harm [UTP]. **Resource:** HRB arrow-poison vine. **Lore:** A hunter tests the brew on a frog and counts its jumps. Three is good poison. **Vocations:** Alchemist (maker of arrow poison); Hunter.

**VI-07 Yam** (*Dioscorea alata*) · Real · Sav, TDf, TRf  
Perennial climber with yearly stems to 15 m and a large tuber, not known wild. The tuber is boiled or baked. Raw, it is toxic [UTP]. **Resource:** PRD greater yam. **Lore:** The yam barn shows a Sunlander farmer's standing (RB §4 names the yam farmer), and the first tuber of the year is the king's. **Vocations:** Farmer; Priest.

**VI-08 Bottle gourd** (*Lagenaria siceraria*) · Real · Thn, Sav, TDf  
Fast annual climber with stems of about 9 m, not known wild. The young fruit is eaten. The hard dry shell makes bottles, bowls, ladles and instruments [UTP]. **Resource:** PRD bottle gourd (catalogue). **Lore:** The first pot and the first drum. A hunter calls the jaguar with one [ADW]. **Vocations:** Farmer; Potter; Entertainer (drummer).

**VI-09 Colocynth** (*Citrullus colocynthis*) · Real · HDs, Thn  
Trailing perennial of sand dunes with a woody root. The dried pulp of the unripe fruit is a violent purge and a poison in excess. The seeds are roasted and give a lamp oil [UTP]. **Resource:** HRB (as a purge). **Lore:** The bitter apple: it looks like water in the sand and has killed more lost men than thirst. **Vocations:** Healer (physician); Alchemist (poisoner).

### 4.8 Fungi (7)

**FU-01 Fly agaric** (*Amanita muscaria*) · Real · Bor, TMx, TBl, TRn  
Mushroom with a red to orange cap set with white warts, of forests and pastures of the temperate and boreal north. Poisonous and hallucinogenic: sickness and confusion, rarely death. Once used to kill flies [EB]. **Resource:** WLD forest mushrooms (the one not to eat). **Lore:** Northmen seers eat it dried before they speak for the dead. The reindeer find it first. **Vocations:** Seer; Alchemist; Hunter (forager).

**FU-02 Cep** (*Boletus edulis*) · Real · Bor, TMx, TBl, TRn  
Mushroom with pores in place of gills that lives with the roots of spruce, pine, oak and beech. Valued for its nutty taste and meaty flesh [EB]. **Resource:** WLD forest mushrooms. **Lore:** Dwarves and Gnomes grow mushrooms in the dark (RB §9, §11). This one they have to walk out and find, like everyone else. **Vocations:** Hunter (forager); Innkeeper.

**FU-03 Truffle** (*Tuber*) · Real · WSh, TBl  
Round fruiting body that grows underground on tree roots in open woodland on chalky soil. A prized food [EB]. Pliny describes it as a growth without root in dry sandy ground and rates the southern kind highest [PLIN-B 19.11]. **Resource:** WLD (a luxury; proposal: a variant). **Lore:** Found with a pig or a dog. Stillfaces (RB §25), whose nose is as keen as a hound's, need neither. **Vocations:** Hunter (forager); Herder (swineherd); Merchant; Innkeeper.

**FU-04 Death cap** (*Amanita phalloides*) · Real · WSh, TMx, TBl  
Mushroom with a greenish yellow to tan cap of 4 to 16 cm, white gills, a ring and a bulb at the base, near oaks, birches and elms. It causes most deaths by mushroom, and cooking does not make it safe [EB]. **Resource:** HRB (as a poison). **Lore:** Bitterbloods eat toadstools at table (RB §27). A guest who is served the same dish should think about why. **Vocations:** Alchemist (poisoner); Hunter (forager).

**FU-05 Fairy ring** (*Marasmius oreades*) · Real · TGr, TBl  
A fungus that grows outward underground in a circle and puts up its mushrooms as a ring that widens each year. Old rings can be hundreds of metres across [EB]. **Resource:** WLD forest mushrooms. **Lore:** Nobody ploughs through one. The Elves say the ring is a door, and do not say to where. **Vocations:** Farmer; Seer.

**FU-06 Ergot** (*Claviceps purpurea*) · Real · Bor, TGr, TMx, Sav  
A fungus of rye, and of sorghum and pearl millet, that replaces the grain with purple-black horns. Bread from such rye brings convulsions, visions, gangrene and death. In small doses it stops bleeding after childbirth [EB]. **Resource:** none (a harm to GRN rye, sorghum, pearl millet). **Lore:** A village that dances and burns in a wet year has eaten the rye wolf's bread. **Vocations:** Miller; Baker; Healer (midwife); Priest.

**FU-07 Foxfire** (*Panellus stipticus*) · Real · TMx, TBl, TRn, TRf  
The glow of rotting wood, made by luminous fungi on dead wood in moist forest, most often in the tropics [EB]. **Resource:** ARC glowcap fungi (its natural kin). **Lore:** Lanternkin (RB §24) mark their night paths with foxfire logs. **Vocations:** Hunter (tracker); Mage; Carter.

### 4.9 Mosses, lichens, ferns and algae (8)

**MO-01 Snow algae** (*Chlamydomonas*) · Real · Ice, Tun  
Algae that live on the surface of snow and ice and stain it red [EB]. **Resource:** none. **Lore:** Red snow is the blood of the amarok's last kill. Hoarkin step around it. **Vocations:** Seer.

**MO-02 Ice-edge lichens and mosses** · Real · Ice, Tun  
Lichens and mosses are almost the whole plant cover of the icy barrens, with a very few flowering plants in the most sheltered places [EB-TUN]. **Resource:** none. **Lore:** The ring of grey on a rock tells a Hoarkin guide how long the ice has been gone from it. **Vocations:** Hunter (guide); Scholar.

**MO-03 Reindeer lichen** (*Cladonia rangiferina*) · Real · Tun, Bor  
Bushy, branched lichen about 8 cm high that covers great areas of tundra and taiga and grows a few millimetres a year. The winter grazing of reindeer, moose and musk ox. A spirit is distilled from it [EB]. **Resource:** LVS herded reindeer (their winter pasture). **Lore:** Grazed ground takes a generation to grow back, so a herding family's round is as long as a life. **Vocations:** Herder (reindeer herder); Brewer (distiller).

**MO-04 Iceland moss** (*Cetraria islandica*) · Real · Tun, Bor  
Branched bushy lichen about 7 cm high, brown to grey-white, of alpine and northern ground. Soaked, dried and ground, it goes into bread, soups and jellies. Reindeer and stock eat it [EB]. **Resource:** HRB Iceland moss. **Lore:** The bread of the Ashen Years in the north, and still the bread of the last weeks of winter. **Vocations:** Hunter (forager); Baker; Healer (herbalist).

**MO-05 Beard lichen** (*Usnea*) · Real · Bor, TMx, TRn, TRf  
Yellow or greenish hanging lichen of tree branches, in humid mountains up to 1.5 m long. An old remedy for coughs. Some kinds give an orange dye. It burns readily when dry [EB]. **Resource:** DYE lichen dyes. **Lore:** Old forest wears a beard. Elves judge a wood's age and health by it and settle only under bearded trees. **Vocations:** Healer (herbalist); Weaver (dyer); Forester.

**MO-06 Dye lichen** (*Roccella tinctoria*) · Real · Tun, WSh, TRn  
Lichens of sea rocks that give a purple dye, orchil, when fermented. It dyed silk, wool and parchment and was the cheap everyday purple [EB]. **Resource:** DYE lichen dyes (tundra and temperate rain forest). **Lore:** The poor man's purple. Sumptuary law forbids it as strictly as the real one, and is obeyed less. **Vocations:** Hunter (forager); Weaver (dyer).

**MO-07 Peat moss** (*Sphagnum*) · Real · Tun, Bor, TMx, TRn  
Mosses in dense cushions up to 30 cm tall of bogs, swamps and lake shores. The dead cells hold twenty times their weight in water. Dried peat is fuel. The moss dresses wounds and serves as bedding and lamp wick [EB]. **Resource:** FUL peat. **Lore:** What sinks in a bog does not rot. The Northmen have found Old Realms soldiers in the peat with their faces on. **Vocations:** Labourer (peat cutter, assumed); Healer (surgeon); Charcoal burner.

**MO-08 Bracken** (*Pteridium aquilinum*) · Real · TGr, TMx, TBl, TRn  
Fern with creeping black rhizomes and fronds that can reach 5 m, one of the most widespread plants and a weed of pasture. The fronds thatch and are fed to animals, though the plant is toxic [EB]. **Resource:** none. **Lore:** See fern seed. **Vocations:** Mason (thatcher); Herder.

### 4.10 Aquatic plants (6)

**AQ-01 Kelp** (*Laminaria*) · Real · Sea  
Large brown seaweeds of cold coasts, with blades of 1 to 3 m and giant kinds of 60 m. A forest for fish. The ash gave potash and iodine [EB]. **Resource:** WLD edible seaweed and kelp (cool shelf). **Lore:** Shoalfolk of the cold branch dive the kelp coasts, greased with seal fat (RB §21). **Vocations:** Fisher; Glassmaker; Salter.

**AQ-02 Eelgrass** (*Zostera*) · Real · Mgv, Sea  
Flowering plants with long leaves that grow from rhizomes in shallow, muddy or sandy coastal water. Food and shelter for sea life. The dried leaves packed glass and stuffed cushions [EB]. Dugongs graze such beds [ADW]. **Resource:** none. **Lore:** Shoalfolk fence the meadows where their dugongs graze and count them as pasture. **Vocations:** Fisher; Glassmaker; Servant.

**AQ-03 Lotus** (*Nelumbo nucifera*) · Real · TBl, TDf, TRf  
Water plant about 1 m tall with a tuberous rootstock in the mud of large lakes. Roots, young leaves, stems and seeds are eaten. The leaves serve as plates and wrappers [UTP]. Herodotus has the marsh people bake bread from a lotus [HDT 2.92]. **Resource:** WLD lotus (tropical lakes). **Lore:** It rises clean out of mud. Every Rainkeeper temple pond (RB §6 has the temple as bank, school and granary) grows it. **Vocations:** Farmer; Priest; Gardener.

**AQ-04 Water chestnut** (*Trapa natans*) · Real · TMx, TBl, TDf  
Floating annual of slow rivers, lakes and ponds. The seed is eaten cooked or ground: raw, it is toxic. An important crop in the east. The seeds are strung as beads [UTP]. **Resource:** WLD water chestnut (catalogue). **Lore:** Canal folk grow it in the same water as the fish and the ducks and tax all three. **Vocations:** Farmer; Fisher.

**AQ-05 Water hyssop** (*Bacopa monnieri*) · Real · TDf, Mgv  
Creeping perennial herb of estuary mud flats, channel banks and wet fields. A chief remedy for complaints of the nerves. The leaves are eaten [UTP]. **Resource:** HRB water hyssop. **Lore:** Rainkeeper pupils are dosed with it before the examination of the scribes. **Vocations:** Healer (herbalist); Scribe; Scholar.

**AQ-06 Taro** (*Colocasia esculenta*) · Real · TDf, TRf  
Evergreen perennial of 0.4 to 2 m with a large tuber, of low stream and river banks. Corm, leaves and stems are eaten cooked. Raw, they burn the mouth [UTP]. **Resource:** PRD taro. **Lore:** The first crop planted on new-cleared wet ground, before anyone trusts it with rice. **Vocations:** Farmer.

### 4.11 Arcane plants (15)

**AR-01 Frost-lotus** · Invented · Tun  
A pale water flower that opens on tundra lakes and rivers for the few days between the thaw and the midges, and whose petals stay frozen in the hand. Field: S1, at a lake or a river. **Resource:** ARC frost-lotus. **Lore:** Hearthbloods (RB §14), whose blood is at fever heat, pay anything for it: it is the one thing that cools them. **Vocations:** Hunter (forager); Alchemist; Healer.

**AR-02 Glowcap** · Invented · Bor, TMx, TBl, TRn, TDf, TRf  
A mushroom that glows steadily, bright enough to read by, in limestone country: cave mouths, sinkholes and the roots above them. Foxfire is its faint natural cousin [EB]. Field: S1, on carbonate bedrock, not on flat land, where it is humid. **Resource:** ARC glowcap fungi. **Lore:** Gnome and Dwarf halls are lit with it (RB §9, §11 have both grow mushrooms). It dims when someone in the hall lies, the Gnomes claim. **Vocations:** Gardener (mushroom grower); Miner; Mage; Scribe.

**AR-03 Elderwood** · Invented · Bor, TMx, TBl, TRn, TRf  
A tree of any forest kind that has stood since the Dawn and woken a little. The wood does not rot and remembers the shape it is given. Field: S1, in forest where prosperity is low: forest that people have left alone. **Resource:** ARC elderwood. **Lore:** Some elderwoods are Amberbloods who took root (RB §16). To fell one unasked is murder in their law and a fortune in everyone else's. **Vocations:** Forester (grove warden, tree shaper); Carpenter; Mage; Artificer.

**AR-04 Elven sapflower** · Invented · WSh, TMx, TBl  
A small golden flower of low woodland whose sap keeps what is steeped in it from spoiling. The map calls the resource "sap of Elanor". That name is J. R. R. Tolkien's and should be replaced (§7.4). Field: S1, on flat land and lowland. **Resource:** ARC sap of Elanor (name to be replaced). **Lore:** Elves plant it where one of their own lies. A meadow of it is a graveyard, and its keepers do not sell. **Vocations:** Gardener; Alchemist (embalmer); Healer; Butcher.

**AR-05 Dreamleaf** · Invented · TMx, TBl, TRn  
A broad-leaved shrub of wet tropical highlands, which carry temperate biomes in the generator. The chewed leaf gives true dreams: of what is, not of what will be. Field: S1, on a humid tropical highland. **Resource:** ARC dreamleaf. **Lore:** A Rainkeeper astrologer (RB §6) who is caught using it is dismissed for cheating. **Vocations:** Seer (oracle, diviner); Gardener; Priest.

**AR-06 Mandrake** (*Mandragora officinarum*) · Real-derived · WSh, TBl  
The real plant is a perennial rosette of open woodland, deserted fields and stony places. All parts are poisonous. It was a narcotic and a purge, and it holds hyoscine [PFAF]. Pliny's diggers take care that the wind does not blow in their faces, draw three circles round it with a sword and face west as they pull. The juice was given before cutting to dull the pain [PLIN-B 25.94]. Field: S1 for the kind that screams. **Resource:** HRB (as a narcotic). **Lore:** The scream is why a dog is tied to the root, in the tale every digger tells and none has tested. **Vocations:** Healer (surgeon); Alchemist (apothecary); Seer (hedge witch); Thief.

**AR-07 Moly** · Folklore · WSh  
A herb with a black root and a white flower. Men cannot dig it, only gods. It guards the one who carries it against drugs and spells [HOM 10]. Field: S3. **Resource:** ARC (proposal: a variant). **Lore:** Sold in every market by people who could not have dug it. **Vocations:** Mage; Seer; Alchemist (antidote maker).

**AR-08 Fern seed** (*Pteridium aquilinum*) · Folklore · Bor, TMx, TBl, TRn  
The seed of the fern, which has none. It is said to bloom like gold or fire on Midsummer Eve. Whoever catches it finds buried treasure, and kept among coins it stops them from growing fewer [FRAZER 68]. The fern itself is bracken. Field: S2, one night a year. **Resource:** ARC (proposal: a variant). **Lore:** Thieves wait in the bracken at midsummer with a white cloth. Guards wait for the thieves. **Vocations:** Thief; Seer; Miner.

**AR-09 Perindens tree** · Folklore · Sav, TDf, TRf  
A tree of the hot east with sweet fruit. Doves live in its branches and its shade, and the dragon, their enemy, fears both the tree and its shadow and keeps to the far side [AB f64v]. Field: S2. **Resource:** none. **Lore:** A village under one sleeps without a watch. It pays for that in doves. **Vocations:** Priest; Gardener; Hunter (bird catcher).

**AR-10 Vegetable lamb** · Folklore · CSt, CDs, TGr  
A plant of the steppe whose gourd-like fruit opens on a small living lamb, in the tale told in Mandeville's Travels **[unverified]**: the book could not be opened, and the entry is from memory. Field: S2. **Resource:** FBR (a wool that was never shorn; proposal). **Lore:** Grass riders say it is cotton, described by someone who had seen neither cotton nor a lamb. They are not always right. **Vocations:** Herder (shepherd); Weaver; Merchant.

**AR-11 Singing moss** · Invented · Ice, Tun  
A grey moss on wind-scoured rock that hums when the wind crosses the crystal under it. Field: S1, in mountains of the polar and subpolar belts. **Resource:** ARC windcrystal (marks it). **Lore:** Miners follow the note to the vein, and stop their ears on the way down. **Vocations:** Miner; Seer; Entertainer.

**AR-12 Mirage lily** · Invented · CDs, HDs  
A white flower that opens for one night after rain where star metal has fallen. Its scent shows the walker water that is not there. Field: S1. **Resource:** ARC star metal (marks the fall). **Lore:** Duskreaders (RB §18), who see heat and not pictures, gather it safely and sell it to people who should not buy it. **Vocations:** Seer; Alchemist; Carter (caravan driver).

**AR-13 Stormthorn** · Invented · CSt, TGr, Thn, Sav  
A thorn tree that lightning seeks. Each strike leaves a bead of glass at its foot, and the tree lives. Field: S1, on flat land and lowland. **Resource:** ARC stormglass. **Lore:** Herders camp well away from it, and glassmakers camp under it between storms. **Vocations:** Glassmaker; Seer (weather watcher); Herder.

**AR-14 Lantern mangrove** · Invented · Mgv  
A mangrove whose breathing roots glow blue-green at the ebb. Field: S1, on coasts warmer than 24 °C. **Resource:** ARC moonpearls (the moon oyster beds lie under it). **Lore:** The Shoalfolk harbour lights (RB §21). A pilot steers by which trees are lit. **Vocations:** Sailor (pilot); Fisher (pearl diver); Mage.

**AR-15 Snare vine** · Invented · TDf, TRf  
A liana that tightens on what struggles in it and slackens on what holds still. Field: S1. **Resource:** FBR (a living rope; proposal). **Lore:** Boughwalkers (RB §20) train it across the paths they do not want used. Children learn to hold still before they learn to climb. **Vocations:** Hunter (trapper); Weaver (rope maker); Guard.

---

## 5. Fauna

### 5.1 Critters (44)

**CR-01 Lemming** (*Lemmus sibiricus*) · Real · Tun  
Stout rodent, 13 to 18 cm, tawny brown, of low treeless tundra meadows. It keeps the same coat in winter [ADW] and stays active under the snow [EB-TUN]. **Resource:** none. **Lore:** In a lemming year the foxes and owls breed double. The Hoarkin count lemming runs in autumn to judge the winter. **Vocations:** Seer (weather watcher); Hunter (trapper), as bait.

**CR-02 Arctic hare** (*Lepus arcticus*) · Real · Ice, Tun  
Large hare, 3 to 5 kg, white in winter and grey-brown in summer, of tundra and polar coasts. Northern peoples use the fur for clothing and blankets and eat the meat [ADW]. **Resource:** WLD, as small game. **Lore:** Its tracks look three-legged [ADW]. Children are told that a hare leaving four prints is something else wearing the skin. **Vocations:** Hunter (trapper); Tailor.

**CR-03 Ptarmigan** (*Lagopus muta*) · Real · Ice, Tun, Bor  
Grouse with feathered feet like snowshoes, white in winter with a black tail, barred brown in summer. A popular game bird [ADW], and the one tundra bird that does not migrate [EB-TUN]. **Resource:** WLD upland game birds. **Lore:** Northmen call a man who stays at his post through the dark a ptarmigan. **Vocations:** Hunter (fowler).

**CR-04 Snow bunting** (*Plectrophenax nivalis*) · Real · Ice, Tun  
Small black-and-white bird of rocky slopes that burrows deep into snow to survive the cold [ADW]. **Resource:** none. **Lore:** The first bunting on the ice tells the Hoarkin that the seals will soon pup. **Vocations:** Seer (weather watcher).

**CR-05 Biting swarm** (*Culicidae*) · Real · Tun, Bor, TRn, TDf, TRf, Mgv  
Mosquitoes: slim flies with piercing mouthparts. The females take blood and carry fevers, and they are most abundant in the warm humid tropics [EB]. The summer clouds over tundra and taiga are general knowledge **[unverified]**. **Resource:** none (a harm). **Lore:** Herds walk into the wind to lose them. A camp is sited by the swarm before it is sited by the water. **Vocations:** Alchemist (maker of insect smoke); Healer.

**CR-06 Red squirrel** (*Sciurus vulgaris*) · Real · Bor, TMx, TBl  
Tuft-eared squirrel of conifer and broadleaf forest, red to black with a pale belly. Hunted for its fur. Its forgotten caches sprout into trees [ADW]. **Resource:** FUR squirrel. **Lore:** Elves say the squirrels planted the first oak woods and are owed a tithe of nuts. **Vocations:** Hunter (trapper); Tanner (furrier).

**CR-07 Stoat (ermine)** (*Mustela erminea*) · Real · Tun, Bor, TGr, TMx  
Small weasel-shaped hunter, brown in summer, white in winter with a black tail tip. The white fur trims coats and stoles, and it is an excellent mouser [ADW]. **Resource:** FUR ermine. **Lore:** Ermine was the fur of the judges of the Old Realms. A court still pays more for a white pelt than a trapper earns in a month. **Vocations:** Hunter (trapper); Tanner (furrier); Tailor.

**CR-08 Pika** (*Ochotona dauurica*) · Real · CSt, CDs, TGr  
Small short-eared relative of the hare, straw-grey, of steppe and semi-desert. It cuts grass and stacks it to dry. Herders use the hay piles, and the fur once made felt [ADW]. **Resource:** none. **Lore:** Grass riders who take pika hay in a hard winter leave a pinch of salt on the burrow. **Vocations:** Herder; Weaver (felt maker).

**CR-09 Bobak marmot** (*Marmota bobak*) · Real · CSt, TGr  
Stocky straw-coloured burrowing rodent, about half a metre long, of open steppe. Sentinels whistle the alarm. The pelts made hats and coats, and it was eaten in famines [ADW]. **Resource:** WLD, as small game. **Lore:** An editor's note to Herodotus suggests that the gold-digging ants were marmots [HDT 3.102]. Steppe prospectors sift marmot spoil for gold dust. **Vocations:** Hunter (trapper); Miner (gold washer).

**CR-10 Jerboa** (*Jaculus jaculus*) · Real · CDs, HDs  
Very small hopping rodent with large three-toed hind feet and a long tufted tail, of sandy and stony desert. Eaten by some people [ADW]. **Resource:** none. **Lore:** Duskreaders, who see warmth in the dark (RB §18), follow jerboa trails to cool seeps under the sand. **Vocations:** Hunter (tracker).

**CR-11 Hare and rabbit** (*Lepus europaeus*) · Real · CDs, TGr, WSh, TMx, TBl  
Brown hare of 3 to 5 kg with long black-tipped ears, of open fields and pastures with hedgerows. Prized game with tasty meat, and a pest of crops and young orchards. It runs at about 60 km/h [ADW]. **Resource:** WLD hares and rabbits. **Lore:** A hare that crosses the plough line before sowing is chased off the field by the whole household. **Vocations:** Hunter (trapper, falconer); Gardener.

**CR-12 Ground squirrel (souslik)** (*Spermophilus citellus*) · Real · CSt, TGr  
Small yellowish burrowing squirrel of short-grass steppe and pasture. Eaten on a small scale and a pest in vineyards [ADW]. **Resource:** none. **Lore:** Widegaze lookouts (RB §15) read a rider's approach from the sousliks going to ground, long before the dust shows. **Vocations:** Farmer; Hunter (ratcatcher).

**CR-13 Locust** (*Schistocerca gregaria*) · Real · TGr, HDs, Thn, Sav  
The desert locust is camouflaged when solitary and black and yellow when it swarms. A destructive migratory pest of dry grassland and desert. One swarm crossing a sea was estimated at 5,000 km² [EB]. **Resource:** none (a harm to GRN). **Lore:** The last swarms of the Ashen Years ate the thatch off the roofs. Sealed granaries date from then. **Vocations:** Farmer; Priest; Seer (weather watcher).

**CR-14 Honeybee** (*Apis mellifera*) · Real · TGr, WSh, TMx, TBl, Thn, Sav, TDf  
Banded reddish-brown bee of meadows, open woodland and gardens. It pollinates crops and gives honey and wax. Foragers dance to show where food lies [ADW]. **Resource:** STM honey and beeswax. **Lore:** Halflings keep hives by every burrow (RB §10) and tell the bees of each birth and death in the house. **Vocations:** Gardener (beekeeper); Hunter (honey hunter); Brewer (mead maker).

**CR-15 Hedgehog** (*Erinaceus europaeus*) · Real · TGr, WSh, TMx, TBl  
Small round animal with brown-and-white spines, of fields, field edges and hedgerows. It curls into a ball when threatened. Used for food and for pest control [ADW]. **Resource:** none. **Lore:** A hedgehog in the garden is lucky, and a gardener who kills one loses the year's cabbages. **Vocations:** Gardener.

**CR-16 Edible dormouse** (*Glis glis*) · Real · WSh, TMx, TBl  
The largest dormouse, grey with a bushy tail, of deciduous and mixed forest. A true hibernator that lives on its fat from autumn to spring. Trapped for fur and meat [ADW]. **Resource:** WLD, as small game. **Lore:** Fattened dormice were a dish of Old Realms banquets. Midlander inns still keep them in clay jars. **Vocations:** Hunter (trapper); Innkeeper.

**CR-17 Woodpecker** (*Dryocopus martius*) · Real · Bor, TMx, TBl  
A large woodpecker, about 46 cm, of conifer and beech woodland. Woodpeckers cut their own nest holes in dead wood [EB]. **Resource:** none. **Lore:** Pliny reports that a woodpecker reopens a plugged nest hole with a certain herb [PLIN-H 10.18]. Thieves pay well for "woodpecker's herb", said to open any lock. **Vocations:** Forester; Thief.

**CR-18 Rat** (*Rattus rattus*) · Real · TGr, WSh, TMx, TBl, TRn, HDs, Thn, Sav, TDf, TRf, Mgv  
Agile climbing rodent with a tail longer than its body, of coasts, buildings and fields. It damages crops and fruit trees and fouls food, and its fleas spread plague [ADW]. **Resource:** none (a harm). **Lore:** Grain towns give their Catfolk wardens meat and fish (RB §28) and count the rats they bring as rent paid. **Vocations:** Hunter (ratcatcher).

**CR-19 Banana slug** (*Ariolimax columbianus*) · Real · TRn  
Slug up to 25 cm long, mostly yellow with brown spots, of moist dark forest floors under logs. A garden pest [ADW]. **Resource:** none. **Lore:** Its slime numbs the tongue, and children of the wet coast dare each other to lick one. **Vocations:** Gardener.

**CR-20 Rough-skinned newt** (*Taricha granulosa*) · Real · TMx, TRn  
Newt of 13 to 22 cm with grainy dark skin and an orange belly, of forest ponds and streams. One holds enough toxin to kill several people. Threatened, it bends back to show the belly [ADW]. **Resource:** none. **Lore:** Bitterbloods, who eat what poisons others (RB §27), buy them alive. **Vocations:** Alchemist (poisoner, antidote maker).

**CR-21 Leech** (*Hirudo medicinalis*) · Real · TGr, TMx, TBl, TRn  
Flattened dark worm with reddish stripes, of small muddy reed-edged ponds. Long used for bloodletting. It comes to moving shadows [ADW]. **Resource:** none. **Lore:** A barber's leech pond is fenced like a fish pond and inherited like one. **Vocations:** Healer (barber-surgeon).

**CR-22 Silkworm** (*Bombyx mori*) · Real · WSh, TBl, TDf  
A fully domesticated moth that is not found in the wild. The caterpillar spins a cocoon of one strand about 915 m long, and silk has been taken from it for thousands of years [EB]. It is fed on white mulberry leaves [EB]. **Resource:** FBR silk. **Lore:** Canal folk grow mulberry on the dikes for it (RB §7). Taking the eggs out of a silk province is a hanging matter. **Vocations:** Gardener (silk grower); Weaver (silk weaver).

**CR-23 Kermes scale** (*Kermes ilicis*) · Real · WSh  
Scale insect whose dried bodies give the oldest known red dye. The dye was once paid as tribute and taken as rent [EB]. It lives on the kermes oak [EB]. **Resource:** DYE kermes. **Lore:** "Oak blood" pays the rent of many a hill village, gathered by lamplight before the dew dries. **Vocations:** Hunter (forager); Weaver (dyer).

**CR-24 Cochineal insect** (*Dactylopius coccus*) · Real · HDs, Thn  
Small grey wingless insect under a white waxy coat, feeding on prickly pear. About 70,000 make one pound of scarlet dye [EB]. **Resource:** DYE cochineal. **Lore:** Noonfolk estates fence their cactus rows and brush the insects off by hand at noon. **Vocations:** Gardener (dye-plant grower); Weaver (dyer).

**CR-25 Lac insect** (*Laccifer lacca*) · Real · TDf, TRf  
Scale insect whose resin is shellac, used as wood varnish, sealer and cement [EB]. The page read does not name its host trees or its range. **Resource:** DYE lac. **Lore:** Rainkeeper temples seal their deeds with red lac. **Vocations:** Weaver (dyer); Carpenter; Scribe.

**CR-26 Cicada** (*Cicadidae*) · Real · WSh, Thn, Sav, TDf, TRf  
Insect of 2 to 5 cm with clear wings and large eyes, of forests, grasslands and deserts. Used in folk medicine, as a symbol and as food. Some broods emerge in vast numbers every 13 or 17 years [EB]. **Resource:** none. **Lore:** A brood year is a census year: whoever was born in the last one comes of age. **Vocations:** Healer; Seer.

**CR-27 Tortoise** (*Testudo hermanni*) · Real · WSh, TBl  
Land tortoise of 12 to 23 cm, of forest, scrub and grassland. Kept as a pet and eaten in places [ADW]. **Resource:** none. **Lore:** Halflings carve a child's birth year on a garden tortoise, which outlives the child. **Vocations:** Hunter (forager).

**CR-28 Scorpion** (*Leiurus quinquestriatus*) · Real · CDs, HDs, Thn  
Large yellowish scorpion of 8 to 11 cm that lives under rocks and in burrows in arid land and comes out at night. One of the most dangerous scorpions [ADW]. **Resource:** none. **Lore:** Saltroad caravans shake out every boot at dawn, and the guide who forgets pays for the camp's water. **Vocations:** Alchemist (antidote maker); Healer.

**CR-29 Scarab** (*Scarabaeus sacer*) · Real · WSh, HDs, Thn, Sav  
Dung beetles are round, dark and 5 to 30 mm long. They shape manure into a ball and bury it, which turns dung into soil. One of them was the sacred scarab of ancient Egypt [EB]. **Resource:** none. **Lore:** Along the desert river the beetle rolling its ball is the sun crossing the sky, and it is carved on every seal. **Vocations:** Priest; Jeweller (engraver).

**CR-30 Termite** (*Isoptera*) · Real · Thn, Sav, TDf, TRf  
Soft pale insects that build domed or conical mounds of earth, clay and saliva. They destroy wooden structures and crops, though only one species in ten is a pest [EB]. **Resource:** none (a harm to buildings). **Lore:** Sunlander potters dig their clay from old mounds. **Vocations:** Carpenter; Potter; Mason.

**CR-31 Tsetse fly** (*Glossina*) · Real · Sav, TDf, TRf  
Brown fly of 6 to 16 mm, of woodland and sometimes open grassland. It gives sleeping sickness to people and nagana to cattle and horses [EB]. **Resource:** none (bars LVS cattle and HOR horses). **Lore:** The fly that no mounted army of the Old Realms could pass (RB §4). Where it lives, wealth is counted in yams and iron, not cattle. **Vocations:** Herder; Healer.

**CR-32 Honeyguide** (*Indicator indicator*) · Real · Thn, Sav, TDf  
Bird of about 20 cm of savanna, scrub and forest edge. It calls and flashes its tail to lead honey badgers and others to hives [ADW]. **Resource:** STM honey and beeswax. **Lore:** A hunter who follows the bird and leaves it no comb is led to a snake the next time. **Vocations:** Hunter (honey hunter).

**CR-33 Quelea (weaver bird)** (*Quelea quelea*) · Real · Thn, Sav  
Small weaver bird of the savanna. Colonies nest over several square miles of trees in millions, and it can become an agricultural pest [EB]. **Resource:** none (a harm to GRN). **Lore:** Noonfolk children earn their first wage scaring queleas off the millet. **Vocations:** Farmer; Hunter (fowler).

**CR-34 Mongoose** (*Herpestes*) · Real · HDs, Thn, Sav, TDf  
Small short-legged carnivore with a pointed nose and a long furry tail, grey to brown [EB]. The Egyptian kind keeps down rats, mice and snakes and raids chicken yards [ADW]. Pliny has it coat itself in dried mud to fight the asp [PLIN-H 8.24]. **Resource:** none. **Lore:** Rainkeeper houses keep one as others keep a cat. **Vocations:** Hunter (ratcatcher); Entertainer (snake charmer, assumed).

**CR-35 Poison frog** (*Phyllobates terribilis*) · Real · TRf  
Frog of about 47 mm, bright yellow, green or orange, that sits in the open on the rain forest floor. Skin contact can kill. Its poison tips blowgun darts [ADW]. **Resource:** none. **Lore:** Bitterbloods keep them in pens (RB §27) and sell the dart poison by the thorn. **Vocations:** Alchemist (keeper of snakes and frogs, maker of arrow poison); Hunter.

**CR-36 Leafcutter ant** (*Atta*) · Real · TDf, TRf  
Ants that carry leaf pieces along trails of over 30 m to underground nests of more than 1,000 chambers, where they grow a fungus on the chewed leaves. They can cut the value of pasture [EB]. **Resource:** none. **Lore:** Boughwalkers say the ants taught them to tend the forest as a garden (RB §20). **Vocations:** Gardener; Farmer.

**CR-37 Firefly** (*Lampyridae*) · Real · TMx, TBl, Sav, TDf, TRf, Mgv  
Soft-bodied beetles of 5 to 25 mm whose flash patterns are signals between the sexes, in most tropical and temperate regions [EB]. **Resource:** none. **Lore:** Lanternkin (RB §24) call them cousins and will not have them caught in jars. **Vocations:** Entertainer; Seer.

**CR-38 Fiddler crab** (*Uca*) · Real · Mgv  
Small crab whose male waves one enlarged claw, on intertidal mud of marshes and estuaries. The page read is of a temperate marsh species [ADW]. The mangrove placement is my inference. **Resource:** FSH, as bait. **Lore:** Shoalfolk children learn to count on the waving claws. **Vocations:** Fisher (crab and shrimp catcher).

**CR-39 Mudskipper** (*Periophthalmus*) · Real · Mgv  
Fish up to 30 cm with large mobile eyes set close together, of swamps, estuaries and mudflats. It leaves the water to walk and climb, breathing through its gill chambers and skin [EB]. **Resource:** FSH, minor. **Lore:** "The fish that could not decide" is a Shoalfolk name for a person of two loyalties. **Vocations:** Fisher.

**CR-40 Mud crab** (*Scylla serrata*) · Real · Mgv  
Crab with a shell of 25 to 28 cm and a weight of 2 to 3 kg. It burrows in mangroves and soft intertidal mud, the young live in estuaries, and it is commercially important [SLB]. **Resource:** FSH crabs, lobsters and shrimp. **Lore:** A Shoalfolk family owns its crab holes as others own fields (RB §21). **Vocations:** Fisher (crab and shrimp catcher).

**CR-41 Junglefowl** (*Gallus gallus*) · Real · TDf, TRf  
Bird of about 70 cm with gold, red and metallic green plumage, of secondary forest and scrub. Raised for eggs and meat, for cockfighting and for feathers [ADW]. **Resource:** LVS poultry. **Lore:** Every village cock is said to descend from a pair given by a forest god, and the first egg of the year goes back to the forest. **Vocations:** Farmer; Entertainer.

**CR-42 Flying fox** (*Pteropus giganteus*) · Real · TDf, TRf, Mgv  
Fruit bat with a wingspan of 1.2 to 1.5 m. Several hundred roost in one tree near water. Hunted for food and medicine, and a pest of orchards [ADW]. **Resource:** WLD, as small game. **Lore:** A roost tree is never felled: the bats carry the names of the dead out at dusk and back at dawn. **Vocations:** Gardener; Hunter.

**CR-43 Murex** (*Bolinus brandaris*) · Real · WSh, Sea  
Sea snails whose dye, Tyrian purple, was highly valued in antiquity [EB]. Pliny says the dye lies in a small vein in the neck, the fishing is best after the dog star rises, and the finest purple wool cost five times the common kind [PLIN-H 9.36-40]. **Resource:** DYE murex. **Lore:** Purple was the colour of the sorcerer-kings. In some realms it is still death to wear it without leave. **Vocations:** Fisher (shellfish gatherer); Weaver (dyer).

**CR-44 Pearl oyster** (*Pinctada*) · Real · Mgv, Sea  
Oysters of warm seas that lay nacre around an intruder. Pearls have long been prized as gems [EB]. Pliny held that the best came from the eastern ocean and the gulfs of Arabia [PLIN-H 9.35]. **Resource:** GEM pearl oyster. **Lore:** A Shoalfolk family owns its oyster bed and guards it (RB §21). **Vocations:** Fisher (pearl diver); Jeweller.

### 5.2 Animals (56)

**AN-01 Ringed seal** (*Pusa hispida*) · Real · Ice, Tun, Sea  
Small seal of 1.1 to 1.75 m with a dark coat ringed in silver, of ice floes and pack ice. It keeps breathing holes open in the ice. Hunted for food, oil and skins [ADW]. **Resource:** WLD seals; FUR seals. **Lore:** The Hoarkin followed the seals out onto the ice (RB §13). A hunter sits at a breathing hole for a day without moving. **Vocations:** Fisher (sealer); Hunter; Tanner.

**AN-02 Penguin** (*Aptenodytes forsteri*) · Real · Ice, Sea  
Flightless seabird of 1 to 1.3 m and 25 to 45 kg, black above and white below with yellow ear patches, on polar ice and coasts. It huddles in thousands through the winter [ADW]. **Resource:** none. **Lore:** Hoarkin say the penguins are a people who chose the sea and lost their hands for it. **Vocations:** Hunter (fowler).

**AN-03 Arctic fox** (*Vulpes lagopus*) · Real · Ice, Tun  
Small fox, white in winter and grey to brown in summer, of coastal and alpine tundra. It stores summer food under stones. The fur is prized, and it takes lambs [ADW]. **Resource:** FUR fox and Arctic fox. **Lore:** It follows the white bear for scraps. Trappers follow the fox to find the bear. **Vocations:** Hunter (trapper); Tanner (furrier).

**AN-04 Snowy owl** (*Bubo scandiacus*) · Real · Ice, Tun  
White owl of 63 to 73 cm with yellow eyes, of open tundra, hunting largely by day. It keeps rodents down and robs traplines [ADW]. **Resource:** none. **Lore:** Hearthblood pass wardens (RB §14) take the owl as their badge: it sees the storm first. **Vocations:** Hunter (trapper); Seer.

**AN-05 Sled dog** (*Canis familiaris*) · Real · Ice, Tun, Bor  
Large, powerful, thick-coated dogs of great endurance that pull sleds over snow and ice with people and supplies, a dozen or more in a fan hitch [EB]. **Resource:** HOR sled dogs. **Lore:** A Hoarkin team is named as one person and inherited as one. **Vocations:** Carter (dog driver, assumed); Herder; Hunter.

**AN-06 Reindeer** (*Rangifer tarandus*) · Real · Ice, Tun, Bor  
Deer up to 1.2 m at the shoulder with a hollow-haired coat from dark brown to nearly white. Both sexes carry antlers. It migrates over 5,000 km a year and digs through snow to feed. Herded for thousands of years for meat, fur and antler [ADW]. **Resource:** LVS herded reindeer; WLD caribou; HOR reindeer. **Lore:** The herd is the calendar of the north: every feast is fixed by where it stands. **Vocations:** Herder (reindeer herder); Hunter; Carter.

**AN-07 Eider** (*Somateria mollissima*) · Real · Ice, Tun, Sea  
The largest northern duck, of rocky coasts and offshore islands. The male is black below and white above. Taken for its down, for sport and for food [ADW]. **Resource:** FUR eider down (catalogue). **Lore:** Northmen farm the down by building the ducks stone shelters and taking a share from each nest. **Vocations:** Hunter (fowler); Tailor.

**AN-08 Wolverine** (*Gulo gulo*) · Real · Tun, Bor  
Stocky animal of 9 to 30 kg with brown-black fur and pale shoulder stripes, of boreal forest and tundra. It drives bears and wolves off their kills and raids traps, cabins and food stores. The fur resists cold [ADW]. **Resource:** FUR, minor. **Lore:** Trappers do not say its name in the forest. They call it "the partner", because it takes half. **Vocations:** Hunter (trapper).

**AN-09 Moose** (*Alces alces*) · Real · Bor, TMx  
The largest deer, up to 2.3 m at the shoulder, dark, with huge antlers on the male, of taiga and wetlands near water. Largely solitary. Taken for meat, and captives give milk [ADW]. **Resource:** WLD deer and elk. **Lore:** Caesar was told that the elk has no joints, sleeps leaning on a tree, and is caught by sawing the tree [CAES 6.27]. Northmen tell the tale to strangers with a straight face. **Vocations:** Hunter; Butcher.

**AN-10 Beaver** (*Castor fiber*) · Real · Bor, TMx, TBl, TRn  
Rodent of 13 to 35 kg with a dark coat, of forested lakes, ponds and rivers. Pelts, castoreum and meat are prized. Its dams flood farmland and roads [ADW]. The bestiary has the hunted beaver bite off the parts wanted for medicine and throw them to the hunter [AB f11r]. **Resource:** FUR beaver. **Lore:** A beaver pond is a boundary mark in Northmen law: who drains it moves the border. **Vocations:** Hunter (trapper); Tanner (furrier); Alchemist (perfumer).

**AN-11 Sable and marten** (*Martes zibellina*) · Real · Bor, TMx, TRn  
Marten of about 1 to 2 kg of dense taiga, light to dark brown with a richer winter coat. Fur hunting once cut its numbers [ADW]. **Resource:** FUR sable and marten. **Lore:** "Soft gold": the Northmen paid their grain debts of the Ashen Years in sable (RB §1). **Vocations:** Hunter (trapper); Merchant.

**AN-12 Lynx** (*Lynx lynx*) · Real · Bor, WSh, TMx  
Cat of 18 to 36 kg with a short black-tipped tail, grey to rusty, spotted or plain. A solitary ambush hunter that caches its prey. It kills livestock [ADW]. **Resource:** FUR, minor. **Lore:** Seen once, it is luck. Seen twice, someone is hunting you. **Vocations:** Hunter (trapper).

**AN-13 Capercaillie** (*Tetrao urogallus*) · Real · Bor, TMx  
The largest grouse: the cock weighs 4 to 5 kg and is glossy black with red eye combs. It lives in old conifer forest and displays at leks. Taken as game [ADW]. **Resource:** WLD upland game birds. **Lore:** The cock is deaf while it sings, and so is a man in love, say the Northmen. **Vocations:** Hunter (fowler).

**AN-14 Musk deer** (*Moschus moschiferus*) · Real · Bor  
Small antlerless deer of 15 to 17 kg of mountain taiga. The male has long downward canines and a musk pouch. The musk is valued for perfume and medicine, and hunting for it has made the deer rare [ADW]. **Resource:** FUR musk deer (deposit: mountains of the boreal belt). **Lore:** A pod of musk is worth a horse. Dwarf holds buy it for their dead. **Vocations:** Hunter (trapper); Alchemist (perfumer).

**AN-15 Saiga** (*Saiga tatarica*) · Real · CSt, CDs, TGr  
Antelope of 30 to 45 kg with a large head and a mobile swollen nose, yellowish red in summer and grey in winter, of dry steppe and semi-desert. Taken for meat, fur and horn, which is ground for medicine [ADW]. **Resource:** WLD antelope and gazelle. **Lore:** Grass rider boys must run one down on horseback before they may marry. **Vocations:** Hunter; Alchemist (apothecary).

**AN-16 Horse** (*Equus caballus*) · Real · CSt, TGr, WSh, TBl  
Grazer of 227 to 900 kg, of temperate grassland, steppe and savanna, living in herds with a dominance order. Used for food, transport, war and sport [ADW]. The wild steppe horse is stocky, reddish brown, with a stiff dark mane, and was never tamed [ADW]. **Resource:** HOR horses. **Lore:** Children of the Grass riders ride before they walk well, and the theft of a horse is death (RB §5). **Vocations:** Herder (horse breeder, horse breaker); Soldier; Carter; Smith (farrier).

**AN-17 Sheep** (*Ovis aries*) · Real · CSt, CDs, TGr, WSh, TMx, TRn  
Flocking grazer, white to near black, kept in deserts, grasslands, scrub and mountains for meat, milk, wool and hides. Flocks of up to 1,000 move without a leader [ADW]. Herodotus tells of a breed whose tails are so long that each rests on a little cart [HDT 3.113]. **Resource:** LVS sheep. **Lore:** The Widegaze keep sheep on open range without fences and watch them from a mound (RB §15). **Vocations:** Herder (shepherd); Weaver; Butcher.

**AN-18 Wild sheep and ibex** (*Ovis ammon; Capra ibex*) · Real · CSt, Bor, CDs, WSh, TMx  
The argali is the largest sheep, 60 to 185 kg with corkscrew horns, of steep slopes above 1,000 m [ADW]. The ibex is a bearded mountain goat taken for food, horn and medicine [ADW]. The bestiary says the ibex lands on its horns when it falls [AB f11r]. **Resource:** WLD wild sheep and ibex (mountains). **Lore:** A dwarf who brings down a ram on the heights hangs the horns over the hold gate. **Vocations:** Hunter; Jeweller.

**AN-19 Yak** (*Bos grunniens*) · Real · Tun, CSt, CDs  
Long-haired blackish ox of alpine meadow and steppe with upcurved horns. It gives meat, milk, butter and cheese, hair, pack transport and dung for fuel [ADW]. **Resource:** LVS yak (cold mountains). **Lore:** Hearthbloods herd yaks on the high passes (RB §14) and hire them out with their drivers. **Vocations:** Herder (yak herder); Carter.

**AN-20 Bactrian camel** (*Camelus bactrianus*) · Real · CSt, CDs  
Two-humped camel of 300 to 690 kg with a long thick coat, of cold desert and steppe. Used for transport, meat, milk, hides, hair and dung fuel [ADW]. **Resource:** HOR Bactrian camel. **Lore:** Brinefolk salt caravans (RB §17) count distance in camel days and pay tolls in stamped salt. **Vocations:** Herder (camel breeder); Carter (camel driver).

**AN-21 Onager** (*Equus hemionus*) · Real · CSt, CDs, HDs  
Wild ass of 200 to 260 kg, reddish brown with a white belly and a dark stripe down the back, of desert and steppe within reach of water. Hunted for meat and hide. Herders see it as a rival for water [ADW]. **Resource:** WLD, as game. **Lore:** No one has broken an onager to the saddle. Orc hearths (RB §12) count the attempt as a rite. **Vocations:** Hunter.

**AN-22 Golden eagle** (*Aquila chrysaetos*) · Real · Tun, CSt, Bor, CDs, WSh  
Dark brown eagle with a golden nape and a wingspan of about 2 m, of open country from sea level to 3,600 m. Pairs hunt together. It sometimes kills livestock [ADW]. **Resource:** none. **Lore:** Grass rider falconers (RB §5) fly eagles at foxes and wolves. A trained bird is set free after ten winters. **Vocations:** Hunter (falconer).

**AN-23 Falcon** (*Falco cherrug*) · Real · CSt, CDs, TGr, HDs  
The saker is a brown, streaked falcon of about 50 cm, of open country with cliffs and scattered trees. A persistent hunter that follows prey into brush. It is taken for the bird trade [ADW]. **Resource:** none. **Lore:** A white falcon is a gift between khans and a cause of war when it is refused. **Vocations:** Hunter (falconer); Noble.

**AN-24 Cattle** (*Bos taurus*) · Real · TGr, TMx, TBl, TRn, Sav, TDf  
Herd animal of 150 to 1,360 kg, black, white, red or brown, kept on grassland and farmland for milk, meat and leather, for dung, and for ploughing and hauling [ADW]. **Resource:** LVS cattle. **Lore:** Among Sunlanders cattle are bride-price, blood-price and the measure of a man (RB §4). **Vocations:** Herder (cowherd, dairymaid); Farmer (ploughman); Butcher; Tanner.

**AN-25 Red fox** (*Vulpes vulpes*) · Real · CSt, Bor, TGr, WSh, TMx, TBl  
Fox with a coat from yellowish red to deep red-brown and black lower legs, of forest, prairie, farmland and towns: the widest range of any wild dog. Its fur is valuable, and it takes poultry [ADW]. **Resource:** FUR fox and Arctic fox. **Lore:** In Halfling tales the fox is the tax collector. **Vocations:** Hunter (trapper); Tanner (furrier).

**AN-26 Red deer** (*Cervus elaphus*) · Real · Bor, WSh, TMx, TBl, TRn  
Large deer, dark brown in winter and tan in summer with a pale rump. The stag carries antlers up to 1.5 m and bellows in autumn. Taken for food, hide and antler, and a pest of crops [ADW]. **Resource:** WLD deer. **Lore:** The stag is the lord's game: most Midlander forests are closed to commoners. **Vocations:** Hunter; Noble; Bandit (poacher).

**AN-27 Wild boar and pig** (*Sus scrofa*) · Real · WSh, TMx, TBl, TDf, TRf, Mgv  
Boar of 66 to 272 kg, black to red-brown, of forest, shrubland and marsh near water. Sows live in groups. Hunted for sport and food, and it ruins crops [ADW]. The pig is its domestic form. **Resource:** WLD wild boar; LVS pigs. **Lore:** Halflings keep a pig by every burrow (RB §10). Orcs keep them half wild (RB §12). **Vocations:** Herder (swineherd); Hunter; Butcher.

**AN-28 Goat** (*Capra hircus*) · Real · CSt, CDs, WSh, HDs, Thn, Sav  
Bearded, horned browser of about 45 kg kept in nearly all habitats for milk, meat, wool and leather. Gone wild, it grazes land bare [ADW]. Herodotus and Pliny have the resin ladanum combed from goats' beards [HDT 3.112] [PLIN-H 12.17]. **Resource:** LVS goats. **Lore:** Dwarves keep goats on the high pastures and make hard cheese (RB §9). Saltroad nomads weave their tents of goat hair (RB §3). **Vocations:** Herder (goatherd); Weaver; Tanner.

**AN-29 Donkey and mule** (*Equus asinus*) · Real · CSt, WSh, HDs, Thn, Sav  
Grey ass of 1.1 to 1.4 m at the shoulder with a dark stripe down the back, of desert and mountain country. It carries heavy loads and guards livestock [ADW]. **Resource:** HOR donkeys and mules (hills and mountains). **Lore:** A mule knows the mountain road better than the muleteer, and the toll is charged per mule for that reason. **Vocations:** Carter (muleteer); Herder; Miller.

**AN-30 Raven** (*Corvus corax*) · Real · Tun, CSt, Bor, CDs, WSh, TMx, TRn  
Large black bird up to 69 cm with a wedge-shaped tail, of open country from tundra to desert. Highly intelligent. It clears carrion and eats grain [ADW]. **Resource:** none. **Lore:** Soldiers say the ravens know the outcome of a battle the day before, by which camp they roost above. **Vocations:** Seer; Soldier.

**AN-31 Crane** (*Grus grus*) · Real · CSt, Bor, TGr, TMx, TBl  
Tall wading bird, larger than a heron, of marshes and plains. Pairs reuse the same nest for years [EB]. Pliny says cranes post sentinels at night that hold a stone in one foot, so that it falls and wakes them if they sleep [PLIN-H 10.23]. **Resource:** WLD waterfowl. **Lore:** The crane with the stone is the badge of sworn sentries (RB §15 names such companies). **Vocations:** Guard; Hunter (fowler).

**AN-32 Otter** (*Lutra lutra*) · Real · Bor, TMx, TBl, TRn, Mgv  
Playful swimmer of about 7 kg of lakes, rivers and coasts, also of peat swamp and rice fields. Its pelt is valuable, and fishers see it as a rival [ADW]. **Resource:** FUR, minor. **Lore:** Canal folk boatmen train otters to drive fish into the nets. **Vocations:** Fisher; Hunter (trapper).

**AN-33 Waterfowl** (*Anser anser*) · Real · Tun, Bor, TGr, TMx, TBl, TRn, Mgv  
The greylag goose is grey-brown, about 80 cm long, with an orange bill, and lives in marshes, fens, estuaries and farmland. It was domesticated for meat and down. Flocks fly in a V [ADW]. **Resource:** WLD waterfowl; LVS poultry. **Lore:** Halflings keep geese as watchdogs (RB §10). **Vocations:** Hunter (fowler); Farmer.

**AN-34 Dromedary** (*Camelus dromedarius*) · Real · HDs, Thn  
One-humped camel of 400 to 600 kg, sandy brown, of deserts with long dry seasons. It can drink 100 litres in ten minutes. A beast of burden that also gives milk, meat, wool, leather and dung fuel [ADW]. **Resource:** HOR dromedary camel. **Lore:** A caravan of a thousand camels carries salt south (RB §3). The lead camel has a name and a share of the profit. **Vocations:** Herder (camel breeder); Carter (camel driver); Merchant.

**AN-35 Gazelle and oryx** (*Gazella dorcas; Oryx leucoryx*) · Real · HDs, Thn, Sav  
The dorcas gazelle is pale with brown flank stripes and curved horns, of semi-desert, dunes and wadis. The oryx is larger, cream to grey-brown, with long horns, and travels towards distant rain. Both are hunted for meat and hide [ADW]. **Resource:** WLD antelope and gazelle. **Lore:** Duskreaders hunt them at night by the warmth of the ground a herd has lain on (RB §18). **Vocations:** Hunter; Tanner.

**AN-36 Fennec** (*Vulpes zerda*) · Real · HDs  
The smallest fox, 0.8 to 1.5 kg, with very large ears and thick pale fur, of sandy desert. It caches food and remembers the caches [ADW]. **Resource:** none. **Lore:** A fennec den marks water within a day's dig, say the well diggers. **Vocations:** Hunter (tracker).

**AN-37 Ostrich** (*Struthio camelus*) · Real · HDs, Thn, Sav  
Flightless bird about 2 m tall, of dry savanna and scrub. The cock is black and white. Taken for hide, feathers and eggs. The shells carry water [ADW]. **Resource:** FUR ostrich. **Lore:** Sunlander title-holders wear the plumes. The eggshell is the traveller's flask. **Vocations:** Hunter; Tanner; Tailor.

**AN-38 Horned viper** (*Cerastes cerastes*) · Real · HDs, Thn  
Snake of 30 to 60 cm, sand-coloured, with a scale "horn" over each eye. It buries itself in sand with only eyes and nostrils showing. Its bite causes swelling and bleeding. Kept by snake charmers and milked for venom [ADW]. Herodotus knew small horned snakes that were buried in a temple [HDT 2.74]. **Resource:** none. **Lore:** The Duskreaders see them as warm commas on the cold sand and walk around them. **Vocations:** Alchemist (antidote maker); Entertainer (snake charmer, assumed).

**AN-39 Cobra** (*Naja naja*) · Real · Thn, Sav, TDf, TRf  
Snake of about 2 m that raises the front third of its body and spreads a marked hood. It lives in forest and farmland, eats rats, and its bite kills many each year [ADW]. **Resource:** none. **Lore:** Rainkeeper granaries tolerate one for the rats and set out milk for it. **Vocations:** Alchemist (keeper of snakes and frogs); Entertainer (snake charmer, assumed); Priest.

**AN-40 Wildebeest and zebra** (*Connochaetes taurinus; Equus burchellii*) · Real · Sav  
The wildebeest is a slate-grey, maned and bearded antelope of open grass and floodplain. The zebra is a black-and-white striped horse led in file by a mare. Both are taken for meat and skins and compete with cattle [ADW]. **Resource:** WLD antelope and gazelle. **Lore:** The great herds follow the rains, and the Sunlander herders move their cattle behind them. **Vocations:** Hunter; Herder.

**AN-41 Giraffe** (*Giraffa camelopardalis*) · Real · Thn, Sav  
The tallest animal: bulls reach 5.7 m. It has a spotted coat and lives in savanna and open woodland. Taken for meat and hide [ADW]. **Resource:** WLD, as game. **Lore:** Widegaze say the giraffe is the only beast that has seen as far as they have. **Vocations:** Hunter.

**AN-42 Vulture** (*Gyps*) · Real · CSt, WSh, HDs, Thn, Sav  
Large carrion birds, many with bare heads, of open country, with keen eyesight and strong beaks. They feed in a strict order [EB]. **Resource:** none. **Lore:** Where the vultures circle, a caravan has lost a beast or a kingdom a battle. Either is news worth selling. **Vocations:** Seer; Priest.

**AN-43 Water buffalo** (*Bubalus bubalis*) · Real · TDf, TRf, Mgv  
Dark, thin-haired ox with wide horns, of marsh, swamp and river land. It wallows in mud. It gives milk, meat and leather and draws the plough [ADW]. **Resource:** LVS water buffalo (floodplains of the dry forest). **Lore:** The paddy cannot be worked without it. Rainkeepers grow rice in the wet lands (RB §6), and a village counts its wealth in buffaloes. **Vocations:** Farmer (paddy farmer); Herder.

**AN-44 Peafowl** (*Pavo cristatus*) · Real · TDf, TRf  
Bird with a bright blue neck. The cock carries a long train of eyespot feathers. It lives in open forest, roosts high in trees, and is kept for its feathers and against pests [ADW]. **Resource:** FUR exotic feathers. **Lore:** A hundred eyes that never close: the peacock is the bird of the Rainkeeper tax office. **Vocations:** Hunter (bird catcher); Tailor; Noble.

**AN-45 Monkeys** (*Semnopithecus entellus*) · Real · Sav, TDf, TRf, Mgv  
The langur is a grey, black-faced monkey of about 13 kg, of forest, scrub and towns. It is held sacred, and it raids crops and steals food from houses [ADW]. The proboscis monkey of the mangrove has a large nose and swims well [ADW]. **Resource:** none. **Lore:** Temple monkeys are fed on feast days and cursed the rest of the year. **Vocations:** Priest; Gardener; Entertainer.

**AN-46 Tropical deer** (*Axis axis*) · Real · Sav, TDf, TRf  
The chital stands 0.6 to 1 m at the shoulder, with a red-brown coat in rows of white spots. The stag has three-tined antlers. It lives in tropical grassland and along rivers and is hunted [ADW]. **Resource:** WLD deer. **Lore:** It grazes under the langurs, which drop fruit and bark at the tiger. **Vocations:** Hunter.

**AN-47 Parrot and macaw** (*Ara macao*) · Real · TDf, TRf  
The scarlet macaw is about 89 cm long, scarlet with yellow and blue wings, and lives in the rain forest canopy. Pairs mate for life. Taken for the cage trade and for feathers [ADW]. **Resource:** FUR exotic feathers. **Lore:** Boughwalker bird catchers (RB §20) sell birds that speak, and what a bird repeats in a market has started feuds. **Vocations:** Hunter (bird catcher); Merchant; Tailor.

**AN-48 Sloth** (*Bradypus variegatus*) · Real · TRf  
Slow tree-dweller of about 4 kg whose coat is green with algae. It sleeps 14 to 16 hours a day in the forest canopy [ADW]. **Resource:** none. **Lore:** To the Swiftborn (RB §22), who live fast, the sloth is a holy animal or a joke, by temperament. **Vocations:** Scholar.

**AN-49 Tapir** (*Tapirus terrestris*) · Real · TDf, TRf  
Dark, heavy browser of 150 to 250 kg with a short trunk, of lowland and swamp forest near rivers. Taken for hide and meat, and trained to pull ploughs [ADW]. **Resource:** WLD, as game. **Lore:** A forest plough ox for those the tsetse has denied cattle. **Vocations:** Hunter; Farmer.

**AN-50 Dugong** (*Dugong dugon*) · Real · Mgv, Sea  
Grey-brown sea mammal of 2.4 to 4 m and up to 400 kg that grazes seagrass in shallow sheltered bays. Taken for food and oil, and its tusks for medicine [ADW]. **Resource:** FSH, as meat and oil. **Lore:** Shoalfolk herd them in fenced bays and will not eat a cow that has calved. **Vocations:** Fisher; Herder.

**AN-51 Monitor lizard** (*Varanus salvator*) · Real · TDf, TRf, Mgv  
Dark lizard, commonly 1.5 m and up to 3 m, of forest rivers and swamps. It climbs trees and dives when threatened. The skin is used for leather, food and medicine [ADW]. **Resource:** none. **Lore:** Bitterblood tanners make a leather of it that rot will not touch. **Vocations:** Hunter; Tanner; Leatherworker.

**AN-52 Llama** (*Lama glama*) · Real · TGr, TMx, TBl  
Woolly camel relative of about 1.2 m and 140 kg, of high plateaus up to 4,000 m. A pack animal that gives wool and guards sheep [ADW]. In the generator it stands on tropical highlands, which carry temperate biomes. **Resource:** HOR llama and alpaca-like camelids (tropical highland in mountains). **Lore:** The highland road has no wheels. Everything a mountain kingdom owns has been on a llama's back. **Vocations:** Herder; Carter; Weaver.

**AN-53 Salmon** (*Salmo salar*) · Real · Bor, TMx, TRn, Sea  
Silver-sided fish of 2 to 9 kg that lives at sea and returns to its home stream to spawn. A prized food fish [ADW]. **Resource:** FSH salmon runs (cold river mouths). **Lore:** The run is counted at the first weir, and the count sets the year's river tolls. **Vocations:** Fisher (river fisher); Official.

**AN-54 Cod and herring** (*Gadus morhua; Clupea harengus*) · Real · Sea  
Cod is a green-brown fish up to 2 m of the shelf down to 600 m, a major fishery and the source of cod liver oil. Herring is a small silver fish in schools several miles long, eaten for centuries [ADW]. **Resource:** FSH cod banks, herring. **Lore:** Northmen are herring fishers (RB §1). A bad herring year empties the fjords of young men. **Vocations:** Fisher (sea fisher, fish drier); Salter; Merchant.

**AN-55 Sturgeon and eel** (*Huso huso; Anguilla anguilla*) · Real · CSt, TGr, TMx, TBl, Sea  
The sturgeon is a long-snouted fish with rows of bony plates that runs up rivers to spawn and is caught for its roe. The eel is a snake-shaped fish of rivers and coasts, a popular food [ADW]. **Resource:** FSH sturgeon (large cold rivers); eels (navigable temperate rivers). **Lore:** Sturgeon is the lord's fish on most rivers. Eels pay the rent of the mill. **Vocations:** Fisher (river fisher, eel trapper); Miller.

**AN-56 Tuna** (*Thunnus thynnus*) · Real · Sea  
Fish of 0.5 to 2 m and up to 680 kg, metallic blue above and silver below, of coastal and open water down to 1,000 m. A food fish and the object of a large fishery [ADW]. **Resource:** FSH tuna and large pelagics. **Lore:** Coast towns build watchtowers for the tuna run before they build walls. **Vocations:** Fisher (sea fisher); Sailor.

### 5.3 Beasts (27)

**BE-01 Polar bear** (*Ursus maritimus*) · Real · Ice, Tun, Sea  
Bear of 300 to 800 kg in the male, with translucent fur that looks white over black skin, of pack ice and polar coasts. It waits at the breathing holes of seals. Dangerous to people. Taken for food and medicine [ADW]. **Resource:** FUR bear. **Lore:** The Hoarkin hunter and the bear sit at the same holes. Each leaves the other the hole he came to first. **Vocations:** Hunter; Tanner (furrier).

**BE-02 Walrus** (*Odobenus rosmarus*) · Real · Ice, Tun, Sea  
Sea beast of 1,200 to 1,500 kg and about 3.2 m with thick grey-brown skin and long tusks, of polar ice and coasts. Bulls fight with the tusks. Taken for food, oil, ivory and hide [ADW]. **Resource:** FUR walrus (ivory). **Lore:** Northmen paid for grain with walrus ivory in the Ashen Years (RB §1). Hoarkin carve it small and patiently (RB §13). **Vocations:** Fisher (sealer); Jeweller (ivory carver).

**BE-03 Narwhal** (*Monodon monoceros*) · Real · Ice, Sea  
Whale of 4 to 5 m, mottled blue-grey, of deep polar water. The male often carries one straight tusk up to 3 m long. Pods join in herds of hundreds. Taken for food, lamp oil and the tusk [ADW]. **Resource:** FUR narwhal tusk. **Lore:** South of the ice the tusk is sold as the horn of the unicorn, and no northern trader corrects the buyer. **Vocations:** Fisher (whaler); Merchant; Alchemist (apothecary).

**BE-04 Whale** (*Balaena mysticetus*) · Real · Sea  
The bowhead is 14 to 18 m long and up to 100 tonnes, black, with the largest baleen of any whale, of cold polar water. It gives meat, baleen and blubber [ADW]. The bestiary whale opens its mouth and breathes out a sweet scent that draws small fish in [AB f73r]. **Resource:** FUR whales (open cold water). **Lore:** One whale lights a Northmen fjord for a winter. The harpooner's share is fixed by the oldest law they have. **Vocations:** Fisher (whaler); Sailor.

**BE-05 Musk ox** (*Ovibos moschatus*) · Real · Tun  
Barrel-shaped, short-legged ox with a shaggy outer coat and a fine underwool, of tundra north of the tree line. Bulls butt heads in the rut. The underwool is valuable for garments [ADW] [EB-TUN]. **Resource:** WLD, as game; the underwool is not in the catalogue. **Lore:** Hearthbloods comb the shed wool off the willows and sell it for its weight in silver. **Vocations:** Hunter; Weaver.

**BE-06 Woolly mammoth** (*Mammuthus primigenius*) · Real-derived · Ice, Tun, CSt  
Elephant of 3 to 3.7 m and 6 to 8 tonnes with long dark outer hair, a dense undercoat and thick fat, of cold steppe-tundra. Extinct in our world. Its ivory has been exported from the far north since medieval times [EB]. **Resource:** FUR, as ivory. **Lore:** The Ashen Years killed the southern herds. The last walk the tundra edge, and the Hoarkin name each one. **Vocations:** Hunter; Jeweller (ivory carver).

**BE-07 Grey wolf** (*Canis lupus*) · Real · Tun, CSt, Bor, CDs, TGr, WSh, TMx, TBl, TRn  
The largest wild dog, about 55 kg in the male, white in the far north and grey, brown or black elsewhere. It lives in packs with a strict order. It kills livestock, though losses are often overstated [ADW]. The bestiary has the she-wolf whelp only in May, when it thunders [AB f16v]. **Resource:** FUR, minor. **Lore:** Every herding people has a wolf oath. The Grass riders swear by it, the Midlanders against it. **Vocations:** Hunter (scout); Herder; Soldier.

**BE-08 Brown bear** (*Ursus arctos*) · Real · Tun, CSt, Bor, WSh, TMx, TBl, TRn  
Bear of 80 to over 600 kg, usually dark brown, of tundra, forest and mountain. It dens for a deep winter sleep. Considered dangerous, though attacks are rare. Taken for hide and body parts [ADW]. The bestiary has the mother lick her shapeless cubs into form [AB f15r]. **Resource:** FUR bear (hills and mountains of the temperate forests). **Lore:** Amberbloods hold that the bear is the oldest person in the forest and is not to be named aloud. **Vocations:** Hunter; Tanner (furrier).

**BE-09 Tiger** (*Panthera tigris*) · Real · Bor, TMx, TDf, TRf, Mgv  
Great cat with a red-orange coat and black stripes, of forest, rain forest, scrub and marsh. It swims rivers several kilometres wide. It injures people and rarely becomes a man-eater. Its body parts are traded [ADW]. **Resource:** FUR big cats. **Lore:** Rainkeeper kings hunt it from elephants (RB §6 has them ride elephants to war). A village that loses three people to one tiger is excused the year's tax. **Vocations:** Hunter (monster hunter); Noble; Alchemist (apothecary).

**BE-10 Snow leopard** (*Panthera uncia*) · Real · Tun, CSt, CDs  
Pale grey to cream cat of 1 to 1.3 m with dark rosettes, a long tail and large paws, of steep rocky ground in high mountains. Active at dawn and dusk. It kills livestock and is not known to attack people [ADW]. **Resource:** FUR big cats. **Lore:** Dwarves call it the ghost of the scree. A hold that finds its tracks at the gate expects a death. **Vocations:** Hunter (trapper); Herder.

**BE-11 Bison** (*Bison bonasus*) · Real · CSt, TGr, TMx, TBl  
Wild ox about 2.9 m long and 800 to 1,000 kg, golden to dark brown, of broadleaf and mixed forest with meadows and clearings. It damages crops [ADW]. **Resource:** WLD bison and wild cattle. **Lore:** The Grass riders of the west follow bison as their cousins follow horses. **Vocations:** Hunter; Tanner; Butcher.

**BE-12 Aurochs** (*Bos primigenius*) · Real-derived · TGr, WSh, TMx, TBl  
Wild ox about 1.8 m at the shoulder, black, with forward-curving horns, the likely ancestor of cattle. Extinct in our world since 1627 [EB]. Caesar's uri are a little smaller than an elephant, spare neither man nor beast, are taken in pits, and cannot be tamed. Their horns are rimmed with silver as cups [CAES 6.28]. **Resource:** WLD bison and wild cattle. **Lore:** A young Midlander lord shows the horns in the hall before he is thought fit to lead, as Caesar says of the Germans [CAES 6.28]. **Vocations:** Hunter; Noble; Jeweller.

**BE-13 Cougar** (*Puma concolor*) · Real · CDs, WSh, TMx, TRn  
Large slender cat, yellowish to grey-brown, of forest, scrub, desert and mountain. It caches large prey and drags it far. It threatens livestock and now and then attacks people [ADW]. **Resource:** FUR big cats. **Lore:** The cat nobody sees. Catfolk (RB §28) leave a share of a kill where its tracks cross theirs. **Vocations:** Hunter (trapper); Herder.

**BE-14 Lion** (*Panthera leo*) · Real · WSh, HDs, Thn, Sav  
Tawny cat. The male is maned and weighs about 189 kg. It lives in prides on plains, savanna and scrub. The lionesses raise cubs together. It attacks people and livestock [ADW]. **Resource:** FUR big cats. **Lore:** Sunlander herders pay Catfolk to watch the herd at dusk, when the lions come (RB §28). **Vocations:** Hunter (monster hunter); Herder; Noble.

**BE-15 Leopard** (*Panthera pardus*) · Real · WSh, Thn, Sav, TDf, TRf  
Cat of 17 to 65 kg, tawny with black rosettes, from desert to rain forest. A solitary night hunter that caches its kills. It takes livestock, and an injured one may hunt people [ADW]. **Resource:** FUR big cats. **Lore:** The leopard skin is the cloak of Sunlander title-holders (RB §4 names them). It may not be bought, only earned. **Vocations:** Hunter; Tanner (furrier).

**BE-16 Cheetah** (*Acinonyx jubatus*) · Real · HDs, Thn, Sav  
Slim long-legged cat of 21 to 72 kg, pale with small black spots, of grassland and desert. It was kept as a hunting animal in antiquity. Herders see it as a pest [ADW]. **Resource:** FUR big cats. **Lore:** Saltroad princes course gazelle with hooded cheetahs carried on the saddle. **Vocations:** Hunter; Noble.

**BE-17 Hyena** (*Crocuta crocuta*) · Real · HDs, Thn, Sav  
Sandy to grey spotted hunter with a massive neck. The females are heavier and rule the clan. It takes livestock and attacks people [ADW]. Pliny and the bestiary say it mimics the human voice to call victims out and digs up graves [PLIN-H 8.30] [AB f11v]. **Resource:** none. **Lore:** Noonfolk bury their dead under stones for the reason the bestiary gives. **Vocations:** Hunter; Guard.

**BE-18 African elephant** (*Loxodonta africana*) · Real · Thn, Sav, TRf  
The largest land animal: bulls weigh 4.5 to 6 tonnes. Grey creased skin and ivory tusks. Matriarchal herds in savanna, woodland and rain forest. Taken for meat and ivory. It wrecks crops and kills people in raids [ADW]. **Resource:** FUR elephant ivory. **Lore:** Sunlander hunters' brotherhoods (RB §4 names them) take one bull a year and bury the tusks' worth in gold with the hunt master. **Vocations:** Hunter (monster hunter); Jeweller (ivory carver); Merchant.

**BE-19 Asian elephant** (*Elephas maximus*) · Real · TDf, TRf  
Elephant of 3.5 to 6 tonnes of which only the male is tusked, of forest, scrub and grassland. It hauls timber and serves in ritual. A bull in musth is highly aggressive. It kills people and ruins crops [ADW]. **Resource:** HOR elephants; FUR elephant ivory. **Lore:** Rainkeeper kings ride to war on elephants and keep an elephant corps (RB §6). **Vocations:** Carter (elephant driver); Soldier; Forester; Priest.

**BE-20 Rhinoceros** (*Diceros bicornis*) · Real · Thn, Sav, TDf  
Grey hairless browser of 800 to 1,400 kg with two horns and a grasping upper lip, of grassland, scrub and forest edge. It charges. The horn is sold as medicine and for dagger hilts [ADW]. **Resource:** FUR, as horn. **Lore:** A cup of the horn is said to sweat when poison is poured in it. Kings buy the cup and keep the taster. **Vocations:** Hunter (monster hunter); Alchemist (apothecary); Jeweller.

**BE-21 Hippopotamus** (*Hippopotamus amphibius*) · Real · HDs, Sav, TDf, TRf  
River beast of 1.3 to 3.2 tonnes with slate-grey skin, of shallow lakes, rivers and swamps. Attacks are sometimes fatal. Taken for meat, tusks and hide, which makes shields and whips [ADW]. Herodotus calls it the river horse and says its dried hide makes spear shafts [HDT 2.71]. **Resource:** FUR hippopotamus ivory (catalogue). **Lore:** River traders (RB §4) fear it more than the crocodile: it upsets the boat, and the crocodile only waits. **Vocations:** Hunter; Sailor (boatman); Leatherworker.

**BE-22 Wild buffalo** (*Syncerus caffer*) · Real · Sav, TDf, TRf  
Very large dark ox with heavy horns in both sexes. Bulls pass 835 kg. It keeps near water in savanna and forest. Taken for meat. It raids crops and gives its diseases to cattle [ADW]. **Resource:** WLD bison and wild cattle. **Lore:** The beast that waits for the hunter who wounded it. A buffalo's horns on a hut mean the owner is not to be argued with. **Vocations:** Hunter; Herder.

**BE-23 Crocodile** (*Crocodylus niloticus*) · Real · HDs, Sav, TDf, TRf, Mgv  
Reptile of 4 to 4.5 m and about 410 kg of lakes, rivers, swamps and brackish water. It kills several hundred people a year and is hunted for meat and skin [EB]. The saltwater kind reaches 6 to 7 m [ADW]. Herodotus has it sacred in one town and eaten in the next, and caught on a hook baited with pork [HDT 2.68-70]. **Resource:** none. **Lore:** Along the desert river a temple keeps a tame one, adorned and fed, and embalms it when it dies, as Herodotus tells [HDT 2.69]. **Vocations:** Hunter (monster hunter); Tanner; Priest.

**BE-24 Python** (*Python reticulatus*) · Real · Sav, TDf, TRf, Mgv  
Snake of about 4.8 m, yellowish to brown with black diamonds, of rain forest, wetland and swamp. It kills by constriction. Hunted for skin and meat. It has killed people [ADW]. **Resource:** none. **Lore:** Bitterbloods keep them for the skins and say a python fed at the new moon never bites. **Vocations:** Hunter; Tanner; Alchemist (keeper of snakes and frogs).

**BE-25 Jaguar** (*Panthera onca*) · Real · TDf, TRf, Mgv  
Cat of 68 to 136 kg, pale yellow to red-brown with black rosettes, often all black, of dense lowland forest, swamp and riverside. It kills livestock and injures people. Hunters call it with hollow gourds [ADW]. **Resource:** FUR big cats. **Lore:** The black jaguar is the night with teeth. Boughwalkers (RB §20) do not come down while one calls. **Vocations:** Hunter (monster hunter); Tanner (furrier).

**BE-26 Bull shark** (*Carcharhinus leucas*) · Real · Mgv, Sea  
Shark about 2.4 m long, grey above and white below, of warm coasts, estuaries, rivers and lakes: it tolerates fresh water. It can injure people. Taken for meat, skin, fins and liver oil [ADW]. **Resource:** FSH tuna and large pelagics. **Lore:** Shoalfolk divers (RB §21) carry a bone knife for it and hope not to learn if it works. **Vocations:** Fisher; Sailor.

**BE-27 Komodo dragon** (*Varanus komodoensis*) · Real · Sav, TDf  
Lizard of over 3 m and up to 165 kg, dull brown-grey, of dry island forest and tall-grass lowland. It attacks livestock and now and then people. The males wrestle upright [ADW]. **Resource:** none. **Lore:** Sailors who land for water on the wrong island bring back the tale of the small dragon. Nobody inland believes them. **Vocations:** Hunter (monster hunter); Sailor.

### 5.4 Monsters (40)

**MN-01 Amarok** · Folklore · Ice, Tun, Bor  
A great lone beast of the north that roars and hunts by land and lake. In the tale a man kills its young and hides in a cave. The amarok, which sees what is hidden, takes his companion's soul, and he falls dead [RINK 132]. Field: S2. **Resource:** none. **Lore:** Hoarkin do not hunt alone at night. They say the amarok takes only those who do. **Vocations:** Hunter (monster hunter); Seer.

**MN-02 Snow wasset** · Folklore · Ice, Tun, Bor  
A winter predator of the far northern logging camps in the lumberjack tales that Cox collected [COX] **[partially verified]**: the book's creature list was read, the entry was not. That it sleeps the summer away and hunts under the snow in winter is from memory **[unverified]**. Field: S1. **Resource:** FUR, as a rare pelt. **Lore:** Trappers blame it for every sprung and empty trap. **Vocations:** Hunter (trapper).

**MN-03 Rime drake** · Invented · Ice, Tun  
A wingless white drake that dens in glacier caves of the polar and subpolar mountains. Its breath is a freezing wind. Field: S2, and only in mountains. **Resource:** ARC windcrystal (found where it dens). **Lore:** Hearthbloods (RB §14) say the windcrystal is its breath, frozen in the rock. Miners listen for it before they dig. **Vocations:** Hunter (monster hunter); Miner; Mage.

**MN-04 Griffin** · Folklore · CSt, Bor, CDs  
A beast said to guard the gold of the far north, from which one-eyed men steal it. Herodotus does not believe the tale [HDT 3.116]. Pliny gives the griffin long ears and a hooked bill and calls it a fable [PLIN-H 10.49]. Field: S2, in hills and mountains with gold. **Resource:** GLD (guards it). **Lore:** Grass riders trade for "griffin claws" that are the horns of some old beast. Dwarves know which. **Vocations:** Hunter (monster hunter); Miner (gold washer).

**MN-05 Gold-digging ant** · Folklore · CSt, CDs, HDs  
Ants larger than foxes and smaller than dogs that throw up sand full of gold from their burrows in a sandy desert. Men take the sand at the hottest hour, when the ants are below, and flee on camels [HDT 3.102-105]. Field: S1. **Resource:** GLD placer gold. **Lore:** Saltroad folk sell maps to the ant hills. Nobody who has one has come back rich. Some have not come back. **Vocations:** Miner (gold washer); Carter (camel driver); Merchant.

**MN-06 Steppe pegasus** · Folklore · CSt, TGr  
Pliny lists "pegasi", birds with the heads of horses, among the fables of the open north [PLIN-H 10.49]. Here: a great crane-legged bird with a horse's head that runs with the wild herds. Field: S2. **Resource:** none. **Lore:** A Grass rider who rides one, once, becomes khan. The tale does not say what happens to those who try. **Vocations:** Herder (horse breaker); Hunter.

**MN-07 Bonnacon** · Folklore · CSt, TGr, WSh  
A wild ox with a horse's mane and horns curled back so far that they cannot wound. In flight it throws out dung that burns over a great distance [PLIN-H 8.15] [AB f12r]. Field: S1. **Resource:** WLD, as game for the brave. **Lore:** Its hide is the best in the grassland. The first tanner to bring one in was not thanked by his street. **Vocations:** Hunter; Tanner.

**MN-08 Rye wolf** · Folklore · TGr, TMx, TBl  
The spirit of the corn as a wolf. When wind moves the standing grain, peasants say that the wolf goes through the corn. Children are kept out of the fields by it, and it sits in the last sheaf, which the reapers shun [FRAZER 48]. Field: S1, on farmland. **Resource:** GRN (guards it). **Lore:** Since the Ashen Years no Midlander cuts the last sheaf without leaving bread for the wolf. **Vocations:** Farmer; Priest; Seer.

**MN-09 Unicorn** · Folklore · TMx, TBl, Sav, TDf  
A beast with the body of a horse, the head of a stag, the feet of an elephant and one black horn. It can be killed and never taken alive [PLIN-H 8.21] [AB f15r]. Field: S3. **Resource:** FUR, as horn. **Lore:** Every court has a horn. Most are narwhal tusks. The one true horn in a region is the reason for its oldest feud. **Vocations:** Hunter (monster hunter); Alchemist (apothecary); Noble.

**MN-10 Hercynian stag** · Folklore · Bor, TMx, TBl  
An ox in the shape of a stag with a single horn between the ears, taller and straighter than any known horn and branching at the top like a palm [CAES 6.26]. Field: S2, in old forest. **Resource:** none. **Lore:** Elves hold that each great forest has one and dies with it. **Vocations:** Hunter; Forester (grove warden).

**MN-11 Werewolf** · Folklore · Bor, WSh, TMx, TBl  
A person who becomes a wolf. Pliny tells of a man who lived as a wolf for nine years, and does not believe it [PLIN-H 8.22]. Field: S2. **Resource:** none. **Lore:** It began in the Ashen Years, the tales say, with the first family that ate what it should not have. **Vocations:** Hunter (monster hunter); Priest; Guard.

**MN-12 Will-o'-the-wisp** · Folklore · Tun, Bor, TMx, TBl, TRn  
A goblin fire that dances over marshy ground and leads the late traveller into bogs and pools. It waits when he stops and moves when he follows. Draining the bogs starves it [SIKES II]. Field: S1, on wet low ground. **Resource:** ARC ghostlight peat (burns where it does). **Lore:** The peat under it burns with a cold blue flame, and a mage pays for it by the basket. **Vocations:** Seer; Mage; Labourer (peat cutter, assumed).

**MN-13 Water horse** · Folklore · Tun, Bor, TMx, TRn  
A horse-shaped thing that haunts a lake. Yeats tells of a farmer who dragged his lake with nets and then tried to pump it dry to be rid of one [YEATS]. Field: S1, at lakes and navigable rivers. **Resource:** none. **Lore:** It stands saddled by the ford at dusk. Seven children can sit on its back, and there is room for one more. **Vocations:** Hunter (monster hunter); Sailor (boatman); Priest.

**MN-14 Hercinia** · Folklore · Bor, TMx, TBl  
Birds of a great northern forest whose feathers shine like fire in the night. Pliny has nothing more to tell of them [PLIN-H 10.47]. Field: S1. **Resource:** FUR exotic feathers. **Lore:** A feather lights a path for one night. Charcoal burners follow the flock home and sell the feathers they find to the lost. **Vocations:** Hunter (bird catcher); Carter; Charcoal burner.

**MN-15 Squonk** · Folklore · TMx, TRn  
A shy creature of the hemlock woods in the lumberjack tales that Cox collected [COX] **[partially verified]**: the creature list was read, the entry was not. The ill-fitting warty skin, the weeping and the dissolving into tears when caught are from memory **[unverified]**. Field: S1. **Resource:** none. **Lore:** A sack that held a squonk holds salt water in the morning. Brinefolk find this very funny. **Vocations:** Hunter (trapper).

**MN-16 Wapaloosie** · Folklore · TRn  
A small tree-climber of the wet coast forests in the tales that Cox collected [COX] **[partially verified]**. That it climbs the tallest trunks to eat shelf fungus is from memory **[unverified]**. Field: S1. **Resource:** none. **Lore:** Mittens of its skin climb the tent pole by themselves, the loggers say. **Vocations:** Forester; Hunter (trapper).

**MN-17 Salamander** (*Salamandra salamandra*) · Real-derived · TMx, TBl, TRn  
The real fire salamander is 15 to 25 cm long, black with yellow or orange spots, lives in shady woodland near streams, and sprays a toxin [ADW]. Pliny's is spotted like stars, comes out only in great rain, and is so cold that it puts out fire [PLIN-H 10.67]. The bestiary gives it the strongest of poisons: it taints every apple on a tree it climbs [AB f69v]. Field: S1 for the fire-quenching kind. **Resource:** ARC mana crystal (it nests by it in wet volcanic highlands: my invention). **Lore:** Smiths keep one in the quench trough, or say they do. **Vocations:** Alchemist; Smith; Mage.

**MN-18 Caladrius** · Folklore · TGr, WSh, TBl  
A white bird that knows whether a sick man will live. If he will, it looks him in the face, takes the sickness into itself and flies towards the sun, which burns it away. If not, it turns its head aside [AB f57r]. Field: S2. **Resource:** none. **Lore:** A court that owns one hides it. The price of a healer falls when the bird is known to have looked away. **Vocations:** Healer (physician); Noble; Priest.

**MN-19 Basilisk** · Folklore · CDs, WSh, HDs, Thn  
A small serpent with a pale star-like mark on its head. Other snakes flee its hiss, and its touch and breath kill plants. Only the weasel can kill it [PLIN-H 8.21]. The bestiary adds that its glance kills a man and burns a bird out of the air [AB f66r]. Field: S2. **Resource:** none. **Lore:** Dead ground in a green valley is a basilisk's doing. Duskreaders (RB §18) hunt it blindfold, by its cold. **Vocations:** Hunter (monster hunter, ratcatcher); Alchemist (poisoner).

**MN-20 Amphisbaena** · Folklore · HDs, Thn, Sav  
A serpent with a second head on its tail that runs either way with its body in a hoop. Its eyes glow like lamps, and it is the first snake out in spring [PLIN-H 8.23] [AB f68v]. Field: S1. **Resource:** none. **Lore:** Its skin, wound on a staff, is the well digger's charm: it finds the way down and the way back. **Vocations:** Alchemist (antidote maker); Seer (dowser).

**MN-21 Phoenix** · Folklore · HDs, Thn  
A bird like an eagle, red and gold, that comes once in five hundred years carrying its dead father in an egg of myrrh [HDT 2.73]. Pliny has it build a nest of cinnamon and frankincense and die on it, and the new bird rise from the remains [PLIN-H 10.2]. Field: S3. One at most in a region. **Resource:** ARC sunstone (left in the ash, by my invention); SPC frankincense, myrrh. **Lore:** Nobody has seen it. Every incense merchant has a cousin who has. **Vocations:** Priest; Scholar; Merchant.

**MN-22 Winged serpent** · Folklore · HDs, Thn  
Small snakes of many colours with bat-like wings that guard the frankincense trees and are driven off only with the smoke of storax [HDT 3.107]. They fly towards the river land in spring and are killed by the ibis [HDT 2.75]. The bestiary's kind outruns horses and kills before the bite is felt [AB f69v]. Field: S1. **Resource:** SPC frankincense (guards it). **Lore:** The price of incense is the price of the smoke that buys a day among the trees. **Vocations:** Gardener (incense gatherer); Alchemist (maker of insect smoke); Hunter.

**MN-23 Cactus cat** · Folklore · HDs, Thn  
A cat of the cactus country in the tales that Cox collected [COX] **[partially verified]**. The thorny coat and the habit of slashing cactus to drink the fermented sap are from memory **[unverified]**. Field: S1. **Resource:** none. **Lore:** It sings on the nights it is drunk. Caravan guards are forbidden to join in. **Vocations:** Hunter (trapper); Carter (caravan driver).

**MN-24 Roperite** · Folklore · CDs, WSh  
A runner of the dry foothill scrub in the tales that Cox collected [COX] **[partially verified]**. The rope-like beak with which it snares hares is from memory **[unverified]**. Field: S1. **Resource:** none. **Lore:** Shepherds find their dogs hobbled at dawn and know what passed. **Vocations:** Herder (shepherd); Hunter.

**MN-25 Hill drake** · Invented · CSt, CDs, WSh, HDs, Thn  
A wingless dragon the size of a cart ox that dens in dry sandstone hills and hunts by ambush. It is the small remnant of the great dragons whose bones lie in the same rock. Field: S2, in hills. **Resource:** ARC dragonbone (dens among it). **Lore:** Dwarves mine dragonbone only after buying the hill from its drake, with a goat a week. **Vocations:** Hunter (monster hunter); Miner; Herder.

**MN-26 Catoblepas** · Folklore · Thn, Sav  
A sluggish beast whose head is too heavy to lift, so that it hangs to the ground. Anyone who meets its eyes dies [PLIN-H 8.21]. Field: S2, at marshes and springs. **Resource:** none. **Lore:** Herders water their cattle at noon, when it sleeps. Noonfolk (RB §19) are hired to stand watch. **Vocations:** Hunter (monster hunter); Herder.

**MN-27 Manticore** · Folklore · Thn, Sav, TDf  
A beast with a man's face, a lion's body, a tail that stings like a scorpion's, three rows of teeth and a hunger for human flesh. Pliny has it from Ctesias [PLIN-H 8.21]. Field: S3. **Resource:** none. **Lore:** It calls travellers by name from the scrub in a voice like a flute. The right answer is to keep walking. **Vocations:** Hunter (monster hunter); Soldier.

**MN-28 Leucrocuta** · Folklore · Thn, Sav  
A beast the size of an ass with a lion's body, a badger's head and one ridge of bone for teeth, that copies human speech [PLIN-H 8.21]. The bestiary makes it the young of a hyena and a lioness [AB f12r]. Field: S2. **Resource:** none. **Lore:** It learns the names of a camp by listening at night. Sunlander herders answer only to a name they chose that day. **Vocations:** Hunter (monster hunter); Guard.

**MN-29 Yale** · Folklore · Thn, Sav, TDf  
A black beast as large as a horse with an elephant's tail, a boar's jaws and long horns that it can swing at will: it fights with one and keeps the other folded back [PLIN-H 8.21] [AB f16v]. Field: S1. **Resource:** FUR, as horn. **Lore:** A pair of yale horns is the best bow a Sunlander smith can mount, and the hardest to match. **Vocations:** Hunter; Smith; Carpenter (bowyer).

**MN-30 Dragon** · Folklore · Sav, TDf, TRf  
A crested serpent of the hot lands whose strength is in its tail. It lies on the paths of elephants, knots itself round their legs and strangles them, and the two often die together [PLIN-H 8.11-12] [AB f66r]. Field: S3. One at most in a region. **Resource:** ARC dragonbone (as old remains, in dry clastic hills); the resin "dragon's blood". **Lore:** Pliny says the red pigment cinnabar is the blood of a dragon crushed under a dying elephant [PLIN-B 33.38]. Painters pay for it as if that were true. **Vocations:** Hunter (monster hunter); Mage; Artist.

**MN-31 Roc** · Folklore · Sav, TDf, TRf, Mgv  
A bird like an eagle and of enormous size, of the islands of the southern sea. In Marco Polo's account it seizes an elephant in its talons, carries it up and drops it to break it [POLO] **[partially verified]**: read in a search excerpt of Yule's edition only. Field: S3, near coasts. **Resource:** FUR exotic feathers (one quill is a ship's cargo). **Lore:** Shoalfolk pilots (RB §21) mark its islands on no chart and steer by where the charts are blank. **Vocations:** Sailor; Hunter (monster hunter); Merchant.

**MN-32 Cinnamon bird** · Folklore · TDf, TRf  
A great bird that builds its nest of cinnamon sticks on a crag. The gatherers lay out joints of ox and ass. The birds carry them up, the nests break under the weight, and the sticks fall [HDT 3.111]. Pliny has the nests shot down with leaded arrows, and calls all such tales lies told to raise the price [PLIN-H 10.33] [PLIN-H 12.19]. Field: S1. **Resource:** SPC cinnamon. **Lore:** The spice guilds keep the tale alive, as Pliny guessed. Now and then a real nest is found, and that keeps it alive too. **Vocations:** Gardener (spice grower); Merchant; Hunter (bird catcher).

**MN-33 Scitalis** · Folklore · TDf, TRf  
A serpent whose back glitters with so many colours that the onlooker stands dazed. It is slow and takes those it has stunned. It runs so hot that it sheds its skin even in winter [AB f68v]. Field: S1. **Resource:** none. **Lore:** Boughwalker painters (RB §20) grind the shed skin for a colour that only they can see. **Vocations:** Artist; Alchemist; Hunter.

**MN-34 Javelin snake** · Folklore · Sav, TDf, TRf, Mgv  
A flying serpent that throws itself from a tree onto whatever passes below and kills it [AB f69v]. Field: S1. **Resource:** none. **Lore:** Forest porters carry their loads on their heads for this reason, they say, and not for comfort. **Vocations:** Carter (porter); Hunter.

**MN-35 Hydrus** · Folklore · Sav, TDf, Mgv  
A water snake of the great river [AB f68v]. Pliny tells of the small enemy of the crocodile that slips into its open mouth and gnaws out through the belly [PLIN-H 8.25]. Here the two are one creature. Field: S1, at river mouths and large rivers. **Resource:** none. **Lore:** Fishers free a hydrus from the net with an apology. The crocodile is everyone's enemy. **Vocations:** Fisher; Hunter.

**MN-36 Storm crane** · Invented · CSt, TGr, Thn, Sav  
A tall grey bird that follows thunderheads over open plains and nests where lightning has fused the soil to glass. Field: S1, on flat land and lowland. **Resource:** ARC stormglass (lines its nest). **Lore:** Widegaze (RB §15) see the flock a day before the storm and move the herds. **Vocations:** Seer (weather watcher); Glassmaker; Hunter (bird catcher).

**MN-37 Canopy spinner** · Invented · TDf, TRf  
A spider the size of a dog that webs the gaps between the tallest trees and takes birds and monkeys. Field: S1. **Resource:** FBR wild silk (catalogue). **Lore:** Boughwalkers (RB §20) cut the old webs for rope bridges and leave the spinner a parrot. **Vocations:** Weaver (silk weaver, rope maker); Hunter (monster hunter).

**MN-38 Moon oyster** · Invented · Mgv, Sea  
A giant shell of warm shallow water that opens only under a full moon and closes on whatever reaches in. Field: S1, on coasts warmer than 24 °C. **Resource:** ARC moonpearls. **Lore:** A Shoalfolk diver (RB §21) who brings up a moonpearl may name the price of the family's reef. **Vocations:** Fisher (pearl diver); Jeweller; Mage.

**MN-39 Serra** · Folklore · Sea  
A sea monster with huge wings that races a ship under sail for three or four miles, tires, folds its wings and is carried back to the deep [AB f73r]. Field: S1, in open water. **Resource:** none. **Lore:** Sailors bet on the race. The serra has never won, and they say that is why the sea is still crossed. **Vocations:** Sailor; Fisher (whaler).

**MN-40 Stay-ship** (*Echeneis naucrates*) · Real-derived · Mgv, Sea  
The real sharksucker is a slim fish up to 1 m that rides on sharks and whales by a sucker on its head, and is used as living bait on a line [ADW]. Pliny's echeneis clings to a keel and holds the ship back [PLIN-H 9.25]. Field: S1 for the kind that stops a ship. **Resource:** FSH, minor. **Lore:** A becalmed captain has the hull scraped before he has the crew flogged. **Vocations:** Sailor; Fisher; Shipwright.

---

## 6. Index per biome

The first table counts the entries per biome and type. An entry counts once in every biome it is listed for, so the columns do not sum to the totals of §1. The lists after it name the entries.

| Biome | TR | SH | FL | HE | FR | GR | VI | FU | MO | AQ | AR | Flora | CR | AN | BE | MN | Fauna |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| Ice & snow | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 2 | 0 | 1 | **4** | 3 | 7 | 4 | 3 | **17** |
| Tundra | 1 | 3 | 2 | 1 | 2 | 1 | 0 | 0 | 6 | 0 | 2 | **18** | 6 | 11 | 7 | 5 | **29** |
| Cold steppe | 4 | 3 | 2 | 5 | 4 | 2 | 0 | 0 | 0 | 0 | 2 | **22** | 3 | 16 | 5 | 6 | **30** |
| Boreal forest | 8 | 3 | 1 | 5 | 2 | 2 | 0 | 3 | 4 | 0 | 3 | **31** | 5 | 18 | 3 | 8 | **34** |
| Cold desert | 3 | 4 | 1 | 2 | 1 | 0 | 0 | 0 | 0 | 0 | 2 | **13** | 4 | 10 | 3 | 5 | **22** |
| Temperate grassland | 3 | 2 | 4 | 13 | 0 | 4 | 0 | 2 | 1 | 0 | 2 | **31** | 10 | 10 | 3 | 5 | **28** |
| Woodland & shrubland | 10 | 4 | 5 | 9 | 9 | 3 | 1 | 2 | 1 | 0 | 3 | **47** | 11 | 12 | 6 | 6 | **35** |
| Temperate mixed forest | 16 | 5 | 2 | 9 | 4 | 1 | 1 | 5 | 3 | 1 | 5 | **52** | 11 | 18 | 6 | 9 | **44** |
| Temperate broadleaf forest | 12 | 4 | 2 | 12 | 6 | 2 | 2 | 6 | 1 | 2 | 6 | **55** | 11 | 11 | 4 | 8 | **34** |
| Temperate rain forest | 5 | 3 | 0 | 3 | 2 | 2 | 0 | 3 | 4 | 0 | 4 | **26** | 5 | 9 | 3 | 5 | **22** |
| Hot desert | 6 | 8 | 1 | 4 | 6 | 2 | 1 | 0 | 0 | 0 | 1 | **29** | 7 | 10 | 5 | 7 | **29** |
| Thorn scrub | 7 | 4 | 0 | 4 | 4 | 1 | 2 | 0 | 0 | 0 | 1 | **23** | 11 | 9 | 6 | 11 | **37** |
| Savanna | 3 | 2 | 0 | 2 | 4 | 3 | 2 | 1 | 0 | 0 | 2 | **19** | 11 | 11 | 11 | 11 | **44** |
| Tropical dry forest | 14 | 4 | 0 | 3 | 7 | 3 | 3 | 0 | 0 | 4 | 3 | **41** | 14 | 10 | 10 | 10 | **44** |
| Tropical rain forest | 11 | 1 | 1 | 2 | 9 | 5 | 5 | 1 | 1 | 2 | 4 | **42** | 11 | 10 | 9 | 6 | **36** |
| Mangrove | 3 | 0 | 0 | 1 | 2 | 2 | 0 | 0 | 0 | 2 | 1 | **11** | 8 | 7 | 5 | 5 | **25** |
| Sea | 0 | 0 | 0 | 1 | 1 | 0 | 0 | 0 | 0 | 2 | 0 | **4** | 2 | 8 | 5 | 3 | **18** |

**Reading the counts.**

- Every biome has at least three entries of each fauna type.
- Ice & snow has the fewest plants, and Mangrove and Cold desert the next fewest. This follows the land (§1, finding 5): the wood, herb and soil factors of Appendix C are lowest in the same biomes.
- The temperate forests and the tropical forests are the richest, as they are in Appendix C.
- A real biome shares most of its species with its neighbours, so most entries stand in two to four biomes. The biome index is for checking coverage. It is not a proposal for frequencies.

### Ice & snow (Ice): 4 flora, 17 fauna

- **Flowers:** Arctic poppy
- **Mosses, lichens, ferns and algae:** Snow algae, Ice-edge lichens and mosses
- **Arcane plants:** Singing moss
- **Critters:** Arctic hare, Ptarmigan, Snow bunting
- **Animals:** Ringed seal, Penguin, Arctic fox, Snowy owl, Sled dog, Reindeer, Eider
- **Beasts:** Polar bear, Walrus, Narwhal, Woolly mammoth
- **Monsters:** Amarok, Snow wasset, Rime drake

### Tundra (Tun): 18 flora, 29 fauna

- **Trees:** Birch
- **Shrubs:** Dwarf willow, Labrador tea, Heather
- **Flowers:** Arctic poppy, Fireweed
- **Herbs:** Roseroot
- **Fruit:** Cloudberry and crowberry, Bilberry
- **Grasses, sedges and reeds:** Cottongrass
- **Mosses, lichens, ferns and algae:** Snow algae, Ice-edge lichens and mosses, Reindeer lichen, Iceland moss, Dye lichen, Peat moss
- **Arcane plants:** Frost-lotus, Singing moss
- **Critters:** Lemming, Arctic hare, Ptarmigan, Snow bunting, Biting swarm, Stoat (ermine)
- **Animals:** Ringed seal, Arctic fox, Snowy owl, Sled dog, Reindeer, Eider, Wolverine, Yak, Golden eagle, Raven, Waterfowl
- **Beasts:** Polar bear, Walrus, Musk ox, Woolly mammoth, Grey wolf, Brown bear, Snow leopard
- **Monsters:** Amarok, Snow wasset, Rime drake, Will-o'-the-wisp, Water horse

### Cold steppe (CSt): 22 flora, 30 fauna

- **Trees:** Poplar and aspen, Juniper, Willow, Saxaul
- **Shrubs:** Sagebrush, Ephedra, Camel thorn
- **Flowers:** Tulip, Saffron crocus
- **Herbs:** Wormwood, Liquorice, Chamomile, Hemp, Rhubarb
- **Fruit:** Sea buckthorn, Walnut, Almond, Jujube
- **Grasses, sedges and reeds:** Feather grass, Wild barley
- **Arcane plants:** Vegetable lamb, Stormthorn
- **Critters:** Pika, Bobak marmot, Ground squirrel (souslik)
- **Animals:** Saiga, Horse, Sheep, Wild sheep and ibex, Yak, Bactrian camel, Onager, Golden eagle, Falcon, Red fox, Goat, Donkey and mule, Raven, Crane, Vulture, Sturgeon and eel
- **Beasts:** Woolly mammoth, Grey wolf, Brown bear, Snow leopard, Bison
- **Monsters:** Griffin, Gold-digging ant, Steppe pegasus, Bonnacon, Hill drake, Storm crane

### Boreal forest (Bor): 31 flora, 34 fauna

- **Trees:** Spruce, Scots pine, Larch, Birch, Poplar and aspen, Rowan, Alder, Juniper
- **Shrubs:** Labrador tea, Bog myrtle, Heather
- **Flowers:** Fireweed
- **Herbs:** Roseroot, Yarrow, Angelica, Gentian and arnica, Monkshood
- **Fruit:** Cloudberry and crowberry, Bilberry
- **Grasses, sedges and reeds:** Cottongrass, Reed and cattail
- **Fungi:** Fly agaric, Cep, Ergot
- **Mosses, lichens, ferns and algae:** Reindeer lichen, Iceland moss, Beard lichen, Peat moss
- **Arcane plants:** Glowcap, Elderwood, Fern seed
- **Critters:** Ptarmigan, Biting swarm, Red squirrel, Stoat (ermine), Woodpecker
- **Animals:** Sled dog, Reindeer, Wolverine, Moose, Beaver, Sable and marten, Lynx, Capercaillie, Musk deer, Wild sheep and ibex, Golden eagle, Red fox, Red deer, Raven, Crane, Otter, Waterfowl, Salmon
- **Beasts:** Grey wolf, Brown bear, Tiger
- **Monsters:** Amarok, Snow wasset, Griffin, Hercynian stag, Werewolf, Will-o'-the-wisp, Water horse, Hercinia

### Cold desert (CDs): 13 flora, 22 fauna

- **Trees:** Poplar and aspen, Juniper, Saxaul
- **Shrubs:** Sagebrush, Ephedra, Saltbush, Camel thorn
- **Flowers:** Tulip
- **Herbs:** Rhubarb, Glasswort
- **Fruit:** Sea buckthorn
- **Arcane plants:** Vegetable lamb, Mirage lily
- **Critters:** Pika, Jerboa, Hare and rabbit, Scorpion
- **Animals:** Saiga, Sheep, Wild sheep and ibex, Yak, Bactrian camel, Onager, Golden eagle, Falcon, Goat, Raven
- **Beasts:** Grey wolf, Snow leopard, Cougar
- **Monsters:** Griffin, Gold-digging ant, Basilisk, Roperite, Hill drake

### Temperate grassland (TGr): 31 flora, 28 fauna

- **Trees:** Poplar and aspen, Juniper, Willow
- **Shrubs:** Hawthorn, Elder
- **Flowers:** Tulip, Sunflower, Coneflower, Dog rose
- **Herbs:** Yarrow, Wormwood, Chamomile, Opium poppy, St John's wort, Gentian and arnica, Nettle, Woad, Madder, Flax, Hemp, Hemlock, Clover and lucerne
- **Grasses, sedges and reeds:** Feather grass, Tallgrass, Wild barley, Reed and cattail
- **Fungi:** Fairy ring, Ergot
- **Mosses, lichens, ferns and algae:** Bracken
- **Arcane plants:** Vegetable lamb, Stormthorn
- **Critters:** Stoat (ermine), Pika, Bobak marmot, Hare and rabbit, Ground squirrel (souslik), Locust, Honeybee, Hedgehog, Rat, Leech
- **Animals:** Saiga, Horse, Sheep, Falcon, Cattle, Red fox, Crane, Waterfowl, Llama, Sturgeon and eel
- **Beasts:** Grey wolf, Bison, Aurochs
- **Monsters:** Steppe pegasus, Bonnacon, Rye wolf, Caladrius, Storm crane

### Woodland & shrubland (WSh): 47 flora, 35 fauna

- **Trees:** Scots pine, Juniper, Oak, Yew, Mulberry, Cedar, Cork oak, Kermes oak, Cypress, Stone pine
- **Shrubs:** Hawthorn, Mastic, Oleander, Mistletoe
- **Flowers:** Peony, Asphodel, Saffron crocus, Dog rose, Lavender
- **Herbs:** Liquorice, Sage, Thyme and rosemary, Opium poppy, Madder, Deadly nightshade, Hemlock, Silphium, Dittany
- **Fruit:** Sweet chestnut, Walnut, Olive, Fig, Pomegranate, Almond, Carob, Citron, Prickly pear
- **Grasses, sedges and reeds:** Wild barley, Esparto, Reed and cattail
- **Vines and climbers:** Grape vine
- **Fungi:** Truffle, Death cap
- **Mosses, lichens, ferns and algae:** Dye lichen
- **Arcane plants:** Elven sapflower, Mandrake, Moly
- **Critters:** Hare and rabbit, Honeybee, Hedgehog, Edible dormouse, Rat, Silkworm, Kermes scale, Cicada, Tortoise, Scarab, Murex
- **Animals:** Lynx, Horse, Sheep, Wild sheep and ibex, Golden eagle, Red fox, Red deer, Wild boar and pig, Goat, Donkey and mule, Raven, Vulture
- **Beasts:** Grey wolf, Brown bear, Aurochs, Cougar, Lion, Leopard
- **Monsters:** Bonnacon, Werewolf, Caladrius, Basilisk, Roperite, Hill drake

### Temperate mixed forest (TMx): 52 flora, 44 fauna

- **Trees:** Spruce, Scots pine, Birch, Poplar and aspen, Rowan, Alder, Willow, Oak, Beech, Ash, Elm, Lime, Fir, Sugar maple, Yew, Hemlock tree
- **Shrubs:** Bog myrtle, Hawthorn, Elder, Heather, Mistletoe
- **Flowers:** Fireweed, Peony
- **Herbs:** Yarrow, Valerian, St John's wort, Angelica, Gentian and arnica, Nettle, Flax, Ginseng, Monkshood
- **Fruit:** Bilberry, Raspberry and bramble, Hazel, Apple
- **Grasses, sedges and reeds:** Reed and cattail
- **Vines and climbers:** Hops
- **Fungi:** Fly agaric, Cep, Death cap, Ergot, Foxfire
- **Mosses, lichens, ferns and algae:** Beard lichen, Peat moss, Bracken
- **Aquatic plants:** Water chestnut
- **Arcane plants:** Glowcap, Elderwood, Elven sapflower, Dreamleaf, Fern seed
- **Critters:** Red squirrel, Stoat (ermine), Hare and rabbit, Honeybee, Hedgehog, Edible dormouse, Woodpecker, Rat, Rough-skinned newt, Leech, Firefly
- **Animals:** Moose, Beaver, Sable and marten, Lynx, Capercaillie, Sheep, Wild sheep and ibex, Cattle, Red fox, Red deer, Wild boar and pig, Raven, Crane, Otter, Waterfowl, Llama, Salmon, Sturgeon and eel
- **Beasts:** Grey wolf, Brown bear, Tiger, Bison, Aurochs, Cougar
- **Monsters:** Rye wolf, Unicorn, Hercynian stag, Werewolf, Will-o'-the-wisp, Water horse, Hercinia, Squonk, Salamander

### Temperate broadleaf forest (TBl): 55 flora, 34 fauna

- **Trees:** Alder, Willow, Oak, Beech, Ash, Elm, Lime, Sugar maple, Yew, Mulberry, Cedar, Lacquer tree
- **Shrubs:** Hawthorn, Elder, Tea, Mistletoe
- **Flowers:** Peony, Dog rose
- **Herbs:** Chamomile, Valerian, St John's wort, Foxglove, Comfrey, Nettle, Woad, Flax, Hemp, Deadly nightshade, Hemlock, Clover and lucerne
- **Fruit:** Raspberry and bramble, Hazel, Apple, Sweet chestnut, Walnut, Citron
- **Grasses, sedges and reeds:** Reed and cattail, Bamboo
- **Vines and climbers:** Hops, Grape vine
- **Fungi:** Fly agaric, Cep, Truffle, Death cap, Fairy ring, Foxfire
- **Mosses, lichens, ferns and algae:** Bracken
- **Aquatic plants:** Lotus, Water chestnut
- **Arcane plants:** Glowcap, Elderwood, Elven sapflower, Dreamleaf, Mandrake, Fern seed
- **Critters:** Red squirrel, Hare and rabbit, Honeybee, Hedgehog, Edible dormouse, Woodpecker, Rat, Leech, Silkworm, Tortoise, Firefly
- **Animals:** Beaver, Horse, Cattle, Red fox, Red deer, Wild boar and pig, Crane, Otter, Waterfowl, Llama, Sturgeon and eel
- **Beasts:** Grey wolf, Brown bear, Bison, Aurochs
- **Monsters:** Rye wolf, Unicorn, Hercynian stag, Werewolf, Will-o'-the-wisp, Hercinia, Salamander, Caladrius

### Temperate rain forest (TRn): 26 flora, 22 fauna

- **Trees:** Rowan, Alder, Yew, Giant conifers, Hemlock tree
- **Shrubs:** Bog myrtle, Heather, Tea
- **Herbs:** Valerian, Foxglove, Angelica
- **Fruit:** Raspberry and bramble, Salmonberry
- **Grasses, sedges and reeds:** Reed and cattail, Bamboo
- **Fungi:** Fly agaric, Cep, Foxfire
- **Mosses, lichens, ferns and algae:** Beard lichen, Dye lichen, Peat moss, Bracken
- **Arcane plants:** Glowcap, Elderwood, Dreamleaf, Fern seed
- **Critters:** Biting swarm, Rat, Banana slug, Rough-skinned newt, Leech
- **Animals:** Beaver, Sable and marten, Sheep, Cattle, Red deer, Raven, Otter, Waterfowl, Salmon
- **Beasts:** Grey wolf, Brown bear, Cougar
- **Monsters:** Will-o'-the-wisp, Water horse, Squonk, Wapaloosie, Salamander

### Hot desert (HDs): 29 flora, 29 fauna

- **Trees:** Poplar and aspen, Acacia, Frankincense tree, Myrrh tree, Dragon's blood tree, Mesquite
- **Shrubs:** Saltbush, Camel thorn, Creosote bush, Saguaro, Sisal and agave, Oleander, Henna, Cotton
- **Flowers:** Rose of Jericho
- **Herbs:** Silphium, Aloe, Senna, Thorn apple
- **Fruit:** Fig, Pomegranate, Date palm, Prickly pear, Jujube, Watermelon
- **Grasses, sedges and reeds:** Esparto, Papyrus
- **Vines and climbers:** Colocynth
- **Arcane plants:** Mirage lily
- **Critters:** Jerboa, Locust, Rat, Cochineal insect, Scorpion, Scarab, Mongoose
- **Animals:** Onager, Falcon, Goat, Donkey and mule, Dromedary, Gazelle and oryx, Fennec, Ostrich, Horned viper, Vulture
- **Beasts:** Lion, Cheetah, Hyena, Hippopotamus, Crocodile
- **Monsters:** Gold-digging ant, Basilisk, Amphisbaena, Phoenix, Winged serpent, Cactus cat, Hill drake

### Thorn scrub (Thn): 23 flora, 37 fauna

- **Trees:** Acacia, Frankincense tree, Myrrh tree, Dragon's blood tree, Baobab, Mesquite, Quebracho
- **Shrubs:** Sisal and agave, Henna, Indigo, Cotton
- **Herbs:** Aloe, Senna, Devil's claw, Thorn apple
- **Fruit:** Pomegranate, Prickly pear, Jujube, Watermelon
- **Grasses, sedges and reeds:** Sorghum
- **Vines and climbers:** Bottle gourd, Colocynth
- **Arcane plants:** Stormthorn
- **Critters:** Locust, Honeybee, Rat, Cochineal insect, Cicada, Scorpion, Scarab, Termite, Honeyguide, Quelea (weaver bird), Mongoose
- **Animals:** Goat, Donkey and mule, Dromedary, Gazelle and oryx, Ostrich, Horned viper, Cobra, Giraffe, Vulture
- **Beasts:** Lion, Leopard, Cheetah, Hyena, African elephant, Rhinoceros
- **Monsters:** Basilisk, Amphisbaena, Phoenix, Winged serpent, Cactus cat, Hill drake, Catoblepas, Manticore, Leucrocuta, Yale, Storm crane

### Savanna (Sav): 19 flora, 44 fauna

- **Trees:** Acacia, Baobab, Flame of the forest
- **Shrubs:** Indigo, Cotton
- **Herbs:** Senna, Devil's claw
- **Fruit:** Watermelon, Shea, Tamarind, Palmyra palm
- **Grasses, sedges and reeds:** Papyrus, Elephant grass, Sorghum
- **Vines and climbers:** Yam, Bottle gourd
- **Fungi:** Ergot
- **Arcane plants:** Perindens tree, Stormthorn
- **Critters:** Locust, Honeybee, Rat, Cicada, Scarab, Termite, Tsetse fly, Honeyguide, Quelea (weaver bird), Mongoose, Firefly
- **Animals:** Cattle, Goat, Donkey and mule, Gazelle and oryx, Ostrich, Cobra, Wildebeest and zebra, Giraffe, Vulture, Monkeys, Tropical deer
- **Beasts:** Lion, Leopard, Cheetah, Hyena, African elephant, Rhinoceros, Hippopotamus, Wild buffalo, Crocodile, Python, Komodo dragon
- **Monsters:** Unicorn, Amphisbaena, Catoblepas, Manticore, Leucrocuta, Yale, Dragon, Roc, Javelin snake, Hydrus, Storm crane

### Tropical dry forest (TDf): 41 flora, 44 fauna

- **Trees:** Mulberry, Teak, Sal, Sandalwood, Banyan and sacred fig, Neem, Kapok, Mahogany, Ebony and rosewood, Cinnamon, Fever-bark tree, Logwood, Flame of the forest, Quebracho
- **Shrubs:** Henna, Coffee, Indigo, Cotton
- **Herbs:** Holy basil, Ginger and turmeric, Thorn apple
- **Fruit:** Citron, Tamarind, Mango, Banana and plantain, Coconut, Areca palm, Palmyra palm
- **Grasses, sedges and reeds:** Bamboo, Sugar cane, Wild rice
- **Vines and climbers:** Rattan, Yam, Bottle gourd
- **Aquatic plants:** Lotus, Water chestnut, Water hyssop, Taro
- **Arcane plants:** Glowcap, Perindens tree, Snare vine
- **Critters:** Biting swarm, Honeybee, Rat, Silkworm, Lac insect, Cicada, Termite, Tsetse fly, Honeyguide, Mongoose, Leafcutter ant, Firefly, Junglefowl, Flying fox
- **Animals:** Cattle, Wild boar and pig, Cobra, Water buffalo, Peafowl, Monkeys, Tropical deer, Parrot and macaw, Tapir, Monitor lizard
- **Beasts:** Tiger, Leopard, Asian elephant, Rhinoceros, Hippopotamus, Wild buffalo, Crocodile, Python, Jaguar, Komodo dragon
- **Monsters:** Unicorn, Manticore, Yale, Dragon, Roc, Cinnamon bird, Scitalis, Javelin snake, Hydrus, Canopy spinner

### Tropical rain forest (TRf): 42 flora, 36 fauna

- **Trees:** Banyan and sacred fig, Neem, Kapok, Mahogany, Ebony and rosewood, Rubber tree, Cinnamon, Clove and nutmeg, Agarwood, Fever-bark tree, Logwood
- **Shrubs:** Coffee
- **Flowers:** Rafflesia
- **Herbs:** Ipecac, Ginger and turmeric
- **Fruit:** Mango, Banana and plantain, Coconut, Cacao, Kola nut, Oil palm, Brazil nut, Areca palm, Sago palm
- **Grasses, sedges and reeds:** Papyrus, Bamboo, Elephant grass, Sugar cane, Wild rice
- **Vines and climbers:** Black pepper, Vanilla, Rattan, Curare vine, Yam
- **Fungi:** Foxfire
- **Mosses, lichens, ferns and algae:** Beard lichen
- **Aquatic plants:** Lotus, Taro
- **Arcane plants:** Glowcap, Elderwood, Perindens tree, Snare vine
- **Critters:** Biting swarm, Rat, Lac insect, Cicada, Termite, Tsetse fly, Poison frog, Leafcutter ant, Firefly, Junglefowl, Flying fox
- **Animals:** Wild boar and pig, Cobra, Water buffalo, Peafowl, Monkeys, Tropical deer, Parrot and macaw, Sloth, Tapir, Monitor lizard
- **Beasts:** Tiger, Leopard, African elephant, Asian elephant, Hippopotamus, Wild buffalo, Crocodile, Python, Jaguar
- **Monsters:** Dragon, Roc, Cinnamon bird, Scitalis, Javelin snake, Canopy spinner

### Mangrove (Mgv): 11 flora, 25 fauna

- **Trees:** Logwood, Mangrove, Nipa palm
- **Herbs:** Glasswort
- **Fruit:** Coconut, Sago palm
- **Grasses, sedges and reeds:** Reed and cattail, Wild rice
- **Aquatic plants:** Eelgrass, Water hyssop
- **Arcane plants:** Lantern mangrove
- **Critters:** Biting swarm, Rat, Firefly, Fiddler crab, Mudskipper, Mud crab, Flying fox, Pearl oyster
- **Animals:** Wild boar and pig, Otter, Waterfowl, Water buffalo, Monkeys, Dugong, Monitor lizard
- **Beasts:** Tiger, Crocodile, Python, Jaguar, Bull shark
- **Monsters:** Roc, Javelin snake, Hydrus, Moon oyster, Stay-ship

### Sea (ocean patches, not a biome) (Sea): 4 flora, 18 fauna

- **Herbs:** Glasswort
- **Fruit:** Sea buckthorn
- **Aquatic plants:** Kelp, Eelgrass
- **Critters:** Murex, Pearl oyster
- **Animals:** Ringed seal, Penguin, Eider, Dugong, Salmon, Cod and herring, Sturgeon and eel, Tuna
- **Beasts:** Polar bear, Walrus, Narwhal, Whale, Bull shark
- **Monsters:** Moon oyster, Serra, Stay-ship

---

## 7. Design note: flora and fauna in the generator

Everything in this section is my own engineering judgement. Nothing here is decided, and none of it is a rule of the readme or of the two generation documents.

### 7.1 What an entry needs as a record

The races show the pattern (`society-generation.md` §2): a list in the Parameter Set, each row with an id, a name, a toggle and a frequency per biome. A flora or fauna row could hold:

| Field | Example | Note |
|---|---|---|
| id, name, type, basis | `stoat`, Stoat (ermine), Critter, Real | The type decides how the entry is used: a Monster is an encounter, a Tree is scenery and timber |
| frequency per biome | Tun 2, Bor 3, TMx 2, TGr 1 | As for races: habitat weight times how common. The biome codes of §2 |
| conditions | `mountains`, `lake`, `river ≥ 8`, `coastal`, `tropical highland` | The same vocabulary as Appendix D. The descriptions above say where a condition is needed |
| Supernatural threshold | S2 | Only for monsters and arcane plants |
| resource and variant | `furs: ermine` | Where the entry is the species behind a variant |
| vocations | Hunter (trapper), Tanner (furrier) | A hint for the vocation mix of a patch (`research/vocations.md` §7.2) |
| danger | none, nuisance, dangerous, deadly | Not researched here. The four fauna types are a first cut of it |

### 7.2 Three ways to use the lists

1. **As material for authoring only.** The lists stay documents. When an LLM writes the description of a patch, a city or a person, it is handed the entries of the biome. Nothing is derived, nothing is stored, and the lists can change at any time. This is the cheapest way and fits phase 1.
2. **As a derived fact of a patch.** The lists become a Parameter Set list. Each land patch draws a handful of "notable species" by hash from the entries of its biome, weighted by frequency and filtered by the conditions and the Supernatural field. It is a pure function of the foundation, like the resources, and gives a hunter's patch its game and a region its monster. It needs a purpose number and a rule for how many.
3. **As the name behind a resource variant.** Nothing new is drawn. The variant the map already picked ("furs: sable and marten") points to the entry, and the entry supplies the description, the lore and the vocations. Species without a resource (pests, predators, monsters) would still need way 1 or 2.

Ways 2 and 3 can be combined: the variant fixes some species of a patch, the draw adds the rest.

### 7.3 Resources: what the lists use and what they lack

The resource line names a resource of the map in 293 of the 374 entries: a variant, or a looser link marked as such. The entries below point at something the catalogue lacks. They are proposals.

| Entry | What is missing |
|---|---|
| Lacquer tree | Lacquer. The Canal folk are known for it (`races-basis.md` §7) and no resource gives it |
| Dragon's blood tree | A red resin used as pigment and medicine. A variant of Dyes |
| Papyrus, Bamboo, Esparto | Paper. `research/vocations.md` has the scribe consume paper "from fibre rags". A writing material as a variant of Fibre would tie the Scribe to the map |
| Quebracho, Oak, Hemlock tree, Larch, Mangrove | Tanbark. It is a by-product in `society-generation.md` §8 and has no source on the map |
| Glasswort, Kelp | Soda ash. Salt has the variant "natron and soda". The plant ash is the other source, and the Brinefolk burn it (`races-basis.md` §17) |
| Truffle, Silphium, Ginseng, Rhubarb, Coneflower | Herbs and wild foods that are luxuries. Gated variants, like the luxury crops of `map-generation.md` §14.2 |
| Monkshood, Deadly nightshade, Hemlock, Thorn apple, Death cap | Poisons. The catalogue has one ("arrow-poison vine"). The Alchemist and the Bitterbloods want more |
| Musk ox, Woolly mammoth, Rhinoceros, Yale, Unicorn | Wool, ivory and horn outside the present fur variants |
| Moly, Fern seed, Vegetable lamb, Snare vine | Arcane materials from folklore, beside the 14 invented kinds |

In the other direction, these variants of Appendix D have no entry of their own in this note: the stones, ores, gems and salts (not flora or fauna), ambergris and jet, and among the living things the crops that were left to Appendix B (most grains and pulses, tobacco, coca, cardamom, chili, annatto, safflower, weld, jute, ramie, kenaf, abaca, New Zealand flax, cassava, sweet potato, enset, quinoa, teff, avocado, cashew, breadfruit, pear, peach), and the fish of the reefs.

### 7.4 What the lists found in the repo's documents

Reported here and not changed, as `CLAUDE.md` asks.

1. **A name from Tolkien.** The arcane material "sap of Elanor" (`map-generation.md` Appendix D.2, kind 70, and Appendix G.3: "sap of the Elven flower Elanor") uses the name of a flower from J. R. R. Tolkien's books. `races-basis.md` already avoids "Hobbit" for the same reason. The entry AR "Elven sapflower" describes the plant under a neutral name. The resource variant needs a new name.
2. **Two sets of biome labels.** The readme, `society-generation.md` Appendix A and `map-generation.md` §13.2 ("Shares") and §13.3 use the full names: Temperate grassland, Woodland & shrubland, Temperate mixed forest, Temperate broadleaf forest. The classification table of §13.2 and all of Appendix D use short forms: Grassland, Shrubland, Mixed forest, broadleaf. The set is the same. This note uses the readme's names.
3. **24 or 27 resources.** `research/vocations.md` speaks of "the 24 generated resources" throughout. The readme and `map-generation.md` §14 have 27 since reagents, herbs and arcane materials were added on 2026-11-15. The vocations note predates them. This note uses 27.
4. **Tropical highland.** Coffee, coca, teff, quinoa, enset, avocado, the llama and dreamleaf are placed on "tropical highland" (`map-generation.md` §14.1), where the biome after cooling is temperate. A list keyed by biome alone would put the llama in a temperate forest anywhere. The condition must travel with the entry (§7.1).
5. **The readme on flora and fauna.** The readme says that the biome "drives its flora and fauna" and defines neither. If the lists are taken up, Flora and Fauna need a concept in the readme, or a sentence under Biome.

### 7.5 Open questions for the user

1. Do flora and fauna become lists in the Parameter Set, like the races and the vocations, or do they stay material for authoring (§7.2)?
2. If they are derived: per patch, per zone or per region? A monster of tier S3 reads as a fact of a region. A herb reads as a fact of a patch.
3. Are the four fauna types the right cut, or does the game need a danger level beside them?
4. Should a monster be able to guard a resource, so that the richest spice or gold patch of a region is the one with the cinnamon bird or the griffin (§1, finding 3)?
5. Are the Real-derived revivals wanted: the mammoth, the aurochs, silphium? They cost nothing and give the cold steppe and the old forests a beast of their own.
6. How far may the monsters reach into the list of peoples? This note left out every folklore being that walks and talks (trolls, giants, merfolk, satyrs, werefolk other than the werewolf), because the 28 races are the peoples of this world. The werewolf is kept as a curse on a person, not as a people.
7. Should the sea get biomes of its own (shelf, open water, reef, ice edge)? The sea entries have no finer home than "Sea" today.
8. The tiers S1 to S3 on the Supernatural field are a proposal (§3). Are three steps wanted, or does each entry get its own threshold?
9. The lore lines lean on the four ages and on the races of `races-basis.md`, which "binds nothing yet". Should lore wait until that document is settled?

### 7.6 Vocations assumed

The vocation names are those of `society-generation.md` §8 and the variants those of `research/vocations.md` §3. Five variants used above are not in either document. They are marked "assumed" where they occur.

| Variant | Under | Used for |
|---|---|---|
| snake charmer | Entertainer | Cobra, Horned viper, Mongoose |
| basket maker | Weaver | Willow, Hazel, Reed and cattail, Bamboo, Rattan, Nipa palm |
| lacquer worker | Artist | Lacquer tree |
| dog driver | Carter | Sled dog |
| peat cutter | Labourer | Peat moss, Will-o'-the-wisp |

No new vocation was needed. Hunter (with trapper, fowler, falconer, forager, honey hunter, bird catcher, ratcatcher and monster hunter), Healer (herbalist), Alchemist (apothecary, poisoner, antidote maker, perfumer, embalmer), Gardener (beekeeper, spice grower, dye-plant grower), Forester (resin tapper), Tanner (furrier), Weaver (dyer, rope maker) and Jeweller (ivory carver) cover nearly every entry.

### 7.7 Gaps in this research

- **Sources that could not be reached.** The USDA Fire Effects Information System (403), Kew's Plants of the World Online (403), the Smithsonian Ocean pages (403), Project Gutenberg (503), sacred-texts.com (the fetch tool failed on its headers, so two texts were read on a mirror), the scan of Cox's book (too large, and 403 at a second address), and the Wikisource pages tried for the Prose Edda, Cox, Marco Polo and Mandeville (404). Plants For A Future timed out during one pass, and Britannica stood in for a dozen trees. Philemon Holland's Pliny is online for books 8, 9, 10, 12 and 16 at the address used, and not for books 19 and 25, which were read in Bostock and Riley's translation instead.
- **No Norse source.** The Prose Edda could not be opened, so the world ash, the ravens of Odin and the Midgard serpent are not cited and not used.
- **Thin pages.** Britannica's pages on the firefly, the weaver bird, the mongoose, the vulture, the crane and the yew are about the family or the genus, not the species. The fiddler crab page is of a temperate species. The hop page describes the plant from North America. The entries say so.
- **Biome fit is inferred.** No source names this repo's biomes. Each placement is my reading of a stated habitat and range (§2).
- **One continent's bias.** Animal Diversity Web and Plants For A Future are strongest on the northern temperate zone. The tropical flora rests on one database, Useful Tropical Plants.
- **No frequencies.** The lists say where an entry occurs, not how often. Frequencies per biome, as the races have them, were not attempted.
- **Not covered.** Domestic breeds, diseases other than those carried by an entry, the insects beyond a dozen, the fish beyond the resource variants, and the birds beyond those with a use or a story.

---

## 8. Sources

### 8.1 Real species

- **[ADW]** Animal Diversity Web, University of Michigan Museum of Zoology. One account per species at `https://animaldiversity.org/accounts/Genus_species/`. Opened for every entry that cites it. Where a heading names two species, both accounts were read. Pages that differ from the Latin name in the heading: Snowy owl at `Nyctea_scandiaca`; Snow leopard at `Uncia_uncia`; the wild steppe horse (in Horse) at `Equus_caballus_przewalskii`; Mongoose at `Herpestes_ichneumon`; the saltwater crocodile (in Crocodile) at `Crocodylus_porosus`; Fiddler crab at `Uca_pugnax`; the proboscis monkey (in Monkeys) at `Nasalis_larvatus`; the dugong (in Eelgrass) at `Dugong_dugon`; the jaguar (in Bottle gourd) at `Panthera_onca`. The accounts of the sandgrouse, the spur-thighed tortoise, the common crane, the quelea, the leafcutter ant, the white-backed vulture, the Nile crocodile, the mud crab and the black woodpecker hold only a classification, so another page was used or the entry dropped.
- **[PFAF]** Plants For A Future, plant database. One page per species at `https://pfaf.org/user/Plant.aspx?LatinName=Genus+species`. Where a heading names two species, both pages were read. The giant fennel (in Silphium) was read at `Ferula+communis`.
- **[UTP]** Useful Tropical Plants database (Ken Fern). One page per species at `https://tropical.theferns.info/viewtropical.php?id=Genus+species`.
- **[EB]** Encyclopaedia Britannica online, `https://www.britannica.com/`. Articles opened, by path: `animal/black-woodpecker`, `animal/silkworm-moth`, `technology/kermes`, `technology/cochineal`, `technology/shellac`, `animal/cicada`, `animal/crane-bird`, `animal/aurochs`, `technology/Tyrian-purple`, `animal/pearl-oyster`, `animal/woolly-mammoth`, `animal/sled-dog`, `animal/mosquito-insect`, `animal/desert-locust`, `animal/dung-beetle`, `animal/termite`, `animal/tsetse-fly`, `animal/weaver-bird`, `animal/mongoose`, `animal/leafcutter-ant`, `animal/firefly-insect`, `animal/mudskipper`, `animal/vulture`, `animal/Nile-crocodile`, `plant/sugar-maple`, `plant/yew`, `plant/white-mulberry`, `plant/cedar-of-Lebanon`, `plant/coast-redwood`, `plant/western-red-cedar`, `plant/western-hemlock`, `plant/cork-oak`, `plant/kermes-oak`, `plant/Italian-cypress`, `plant/stone-pine`, `technology/lacquer`, `plant/saguaro`, `plant/tulip`, `plant/rose-of-Jericho`, `plant/Rafflesiaceae`, `plant/esparto`, `plant/barley-cereal`, `plant/bracken`, `plant/Iceland-moss`, `science/reindeer-lichen`, `science/beard-lichen`, `technology/orchil`, `plant/peat-moss`, `science/red-snow`, `science/fly-agaric`, `science/boletus`, `science/truffle-fungus`, `science/death-cap`, `science/fairy-ring`, `science/ergot`, `science/foxfire`, `science/kelp`, `plant/eelgrass`.
- **[EB-TUN]** "Tundra", Encyclopaedia Britannica. https://www.britannica.com/science/tundra. Read for the plants and animals it names for the Arctic tundra and for the icy barrens.
- **[SLB]** SeaLifeBase, *Scylla serrata*. https://www.sealifebase.se/summary/Scylla-serrata.html
- **[AGRO]** AgroAtlas, Interactive Agricultural Ecological Atlas of Russia and Neighboring Countries, *Haloxylon ammodendron*. https://agroatlas.ru/en/content/related/Haloxylon_ammodendron/index.html

### 8.2 Folklore and ancient natural history

- **[PLIN-H b.c]** Pliny the Elder, *Natural History*, in Philemon Holland's translation of 1601, by book and Holland's chapter. `https://penelope.uchicago.edu/holland/pliny8.html` (books 8, 9, 10, 12 read at `pliny8.html`, `pliny9.html`, `pliny10.html`, `pliny12.html`). Holland's chapter numbers differ from those of modern editions.
- **[PLIN-B b.c]** Pliny the Elder, *Natural History*, translated by John Bostock and H. T. Riley (1855), by book and chapter, on the Perseus Digital Library, text `Perseus:text:1999.02.0137`. Read: 16.95 (mistletoe), 19.11 (truffles), 19.15 (silphium), 25.94 (mandrake), 33.38 (cinnabar). https://www.perseus.tufts.edu/hopper/text?doc=Perseus%3Atext%3A1999.02.0137
- **[HDT b.c]** Herodotus, *Histories*, the Loeb Classical Library edition of 1921, by book and chapter, on LacusCurtius. The page does not name the translator. The Loeb translator is A. D. Godley. Read: 2.68 to 2.76 and 2.92 at `https://penelope.uchicago.edu/Thayer/E/Roman/Texts/Herodotus/2A*.html`; 3.102 to 3.116 at `.../Herodotus/3D*.html`.
- **[CAES 6.c]** Caesar, *The Gallic War*, book 6, chapters 25 to 28, translated by W. A. McDevitte and W. S. Bohn. https://classics.mit.edu/Caesar/gallic.6.6.html
- **[HOM 10]** Homer, *Odyssey*, book 10, translated by Samuel Butler. https://classics.mit.edu/Homer/odyssey.10.x.html
- **[AB fN]** The Aberdeen Bestiary, Aberdeen University Library MS 24, about 1200, by folio, with the university's translation. `https://www.abdn.ac.uk/bestiary/ms24/f11r` and so on. Folios read: 11r, 11v, 12r, 15r, 16v, 55v, 57r, 64v, 65r, 66r, 68v, 69v, 73r.
- **[FRAZER n]** J. G. Frazer, *The Golden Bough*, abridged edition of 1922, by chapter, on Wikisource. Chapter 48, "The Corn-Spirit as an Animal": https://en.wikisource.org/wiki/The_Golden_Bough/The_Corn-Spirit_as_an_Animal. Chapter 68, "The Golden Bough": https://en.wikisource.org/wiki/The_Golden_Bough/The_Golden_Bough
- **[SIKES II]** Wirt Sikes, *British Goblins: Welsh Folk-lore, Fairy Mythology, Legends and Traditions* (1880), chapter II. Read on a mirror of sacred-texts.com: https://neonvagabond.xyz/mirrors/sacred-texts/neu/celt/wfl/wfl02.htm
- **[RINK 132]** Henry Rink, *Tales and Traditions of the Eskimo* (1875), tale 132, "The Amarok". Read on the same mirror: https://neonvagabond.xyz/mirrors/sacred-texts/nam/inu/tte/tte2-132.htm. The page gives the tale number and not the book's title. The title is from the archive's index.
- **[YEATS]** W. B. Yeats, *The Celtic Twilight* (1893, edition of 1902), "A Remonstrance with Scotsmen for Having Soured the Disposition of Their Ghosts and Faeries". Read on the same mirror: https://neonvagabond.xyz/mirrors/sacred-texts/neu/yeats/twi/twi36.htm
- **[COX]** William T. Cox and Coert Du Bois, *Fearsome Creatures of the Lumberwoods: With a Few Desert and Mountain Beasts*, Judd & Detweiler, Washington, 1910. Public domain. Record and list of the twenty creatures read at https://digitalcommons.unl.edu/nebraskianapubs/8/. The text of the entries was not read **[partially verified]**.
- **[POLO]** *The Book of Ser Marco Polo*, translated and edited by Henry Yule, book 3, chapter 33. Known from a search-index excerpt only **[partially verified]**. The page opened at https://dsr.nii.ac.jp/toyobunko/III-2-F-c-30/V-2/page/0476.html.en is from Yule's notes to the chapter and does not hold the passage.

### 8.3 Repository

- `readme.md`: Patch, Biome, Field, Resource, Race, Vocation.
- `docs/map-generation.md`: §13 (biomes), §14 (resources), Appendix B (crops), C (factors by biome), D (resource rules, deposits, ocean patches), G (groups and catalogue).
- `docs/society-generation.md`: §8 (vocations), Appendix A (the default races).
- `docs/races-basis.md`: the entries of the 28 races, cited as (RB §n) by the number of the race.
- `docs/research/vocations.md`: §3 (the 51 vocations and their variants).
- `docs/research/biomes-and-climate.md`: §2.6 (how the biome lists line up).
