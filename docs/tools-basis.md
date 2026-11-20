# Tools basis

The tools of the vocations: 146 real, historically used tools and pieces of working
equipment, two or more for each of the 51 vocations of the default list. Each tool has
a sourced description of the real thing and a short piece of lore that places it in this
world.

The default tool list of the Parameter Set is built from this document
([society-generation.md](society-generation.md) section 8.1): one tool per entry, with the
name of its heading, in the order of the document, and its line "Fitting vocations" as the
vocations of the tool. A change to a name, the order or the fitting vocations here must be
made in `src/Gameworld.Domain/Tools.Defaults.cs` and
[default-parameter-set.json](default-parameter-set.json) too. The emblem weights of the
list are not in this document: section 8.1 gives them.

The rest is material, like [races-basis.md](races-basis.md): the descriptions and the lore
bind nothing, and no generation rule reads this file. They are a source for the text and the
portrait of a person (what a smith holds, what hangs on a healer's belt) and for the produced
resource group Tools of [map-generation.md](map-generation.md) Appendix G.1, when that group
is detailed.

Research date: 2026-11-19.

## How to read an entry

- **The vocations** are the `Vocations` list of
  [default-parameter-set.json](default-parameter-set.json), which is the list of
  [society-generation.md](society-generation.md) section 8: 51 vocations in nine sectors.
  The entries follow the order of that list, and the vocation names are the names of the
  list. A smaller trade (the beekeeper, the parchment maker, the moneyer) is a variant of
  one of the 51, and its tools stand under that vocation.
- **A tool is listed once**, under the first vocation of the list that uses it, with all
  its vocations in the line "Fitting vocations". The other vocations point to it in a line
  "Also used by".
- **Description** is sourced. It says what the real tool is and how it is used, and it
  holds only what the cited page states. Every link was opened during the research, through
  a fetch tool that returns an extraction of the page and not the page itself. A note in
  italics after the source says where the support is thin:
  - *Partially verified* means that only a part of the description is supported, or that
    it rests on a search excerpt or on Wikipedia.
  - A note on the date means that the form and the use are supported, but that the object
    on the page is later than about 1500, or undated.
  - "The judgement of this document" marks a fitting vocation that the source does not
    name.
- **Lore** is invented for this document and is not sourced. It uses the races of
  [races-basis.md](races-basis.md), the four ages of that document and the resources and
  biomes of [map-generation.md](map-generation.md). It names kinds of places and never one
  location, as races-basis.md does. It is flavour for authoring and decides nothing about
  the domain model.
- **Fitting vocations** is the judgement of this document, in the names of the list.

## Technology level

No document of this repository states a technology level as a rule. What they say:
[research/vocations.md](research/vocations.md) calls the world "pre-industrial" and builds
the vocations on medieval records (1086 to 1427), society-generation.md section 8 divides
work "as in the medieval record", and the readme sizes a region as "a Middle Ages European
kingdom". The variants of the vocations reach to the end of that period: printer,
clockmaker, lens grinder, distiller, crossbowman, bell caster. No document names a firearm
or gunpowder: saltpetre is a reagent only.

The assumption made here: the tools are those of the late Middle Ages, about 1200 to 1500,
in Europe and its neighbours, with hand, animal, water and wind power. No firearms, no
gunpowder, no steam. A tool that the sources show only in a later object (a Tudor ship
find of 1545, a treatise of 1556 or 1703, a museum object of the 18th or 19th century) is
kept when its form and use are those of the hand tool before 1500, and the entry says so.

The three vocations of the Supernatural sector have no working magic behind them in the
sources. Their tools are real historical objects of learned magic, alchemy and divination,
described by what their users did with them and believed about them.

## What is weak

- The period. Many museum records that could be opened describe a later object of an old
  type. The descriptions of the smith, the carpenter and the mason lean on Joseph Moxon's
  *Mechanick Exercises* of 1703.
- 30 of the 146 entries are marked *partially verified*. The weakest vocations are the
  Salter (two of three tools rest on search excerpts of Agricola), the Artificer (both
  tools are from objects of the 18th to 20th century), the Thief (no page on a medieval
  lock-picking tool could be opened) and the Leatherworker (one scholar's site for all
  three tools).
- Sources that refused the fetch tool: the collection pages of the British Museum and the
  Metropolitan Museum of Art, the Museum of English Rural Life and Project Gutenberg. The
  British Museum and Met objects are cited through the museums' pages at Google Arts &
  Culture. Agricola is cited from the Internet Archive and a Gutenberg mirror. Theophilus,
  *On Divers Arts*, and Diderot's plates were not read.
- Tools that were looked for and left out because no page supported them: the washing
  beetle and the besom of the Servant, the crossbow, the netting needle, the snare, the
  wine press, the tar kiln, the pedlar's pack, the lens-grinding lap, the hand printing
  press, the quarterstaff and the coin clipper's shears.
- The examples are mostly English and European, because the museum records that could be
  opened are. The lore carries the tools to the other peoples of the world. The sources do
  not.

---

## Land and water

### Farmer

#### Heavy plough

- **Description.** A heavy plough carries an iron coulter, a vertical blade mounted like a
  knife that slices the soil ahead of the share. It cuts long narrow strips and deep furrows
  (ridge and furrow), which lets heavy, poorly drained soil be farmed, and it was pulled by
  teams of up to eight oxen. An iron coulter excavated at Lyminge in Kent is dated to the
  first half of the 7th century. Source: [Medievalists.net, report of the University of
  Reading excavation at
  Lyminge](https://www.medievalists.net/2011/04/08/7th-century-plough-discovery-redraws-map-of-rural-england-by-400-years/).
  *Partially verified. The page is a news report of the excavation and describes the
  coulter, not the mouldboard.*
- **Lore.** Midlanders say the heavy plough is the one thing the Old Realms left that still
  does what it was made for. Eight oxen are more than one farm owns, so a plough team is
  shared, and half the quarrels of a river plain start over whose strip it turns first. The
  Catfolk will guard the grain but never walk behind it.
- **Fitting vocations.** Farmer.

#### Sickle

- **Description.** A curved, hand-held iron tool whose cutting edge is the inside of the
  curve and is serrated. It harvests cereal crops or cuts grass for hay: the blade is drawn
  against the base of the stems and cuts them with a sawing action. The Pitt Rivers Museum
  holds English examples, among them an ancient socketed iron sickle. Source: [Pitt Rivers
  Museum, "Analysing the English Collections": harvesting
  tools](https://england.prm.ox.ac.uk/englishness-harvesting-tools.html). *Form and use are
  verified. The page gives no example dated to the Middle Ages.*
- **Lore.** On the reservoir lands of the Rainkeepers the sickle is handed down with the
  field, and a child is counted as grown the first harvest it keeps pace with the row.
  Swiftborn harvest hands hire out by the sickle and bring their own: they have too few
  seasons to waste one on a borrowed blade.
- **Fitting vocations.** Farmer.

#### Flail

- **Description.** A wooden staff with a shorter stick, the swingle or swipple, hung from it
  by a leather hinge or thong so that it swings freely. It is used to thresh corn by hand
  and separate the grain from the husks. The Pitt Rivers examples are English, the one dated
  example from Sussex in 1894. Source: [Pitt Rivers Museum, "Analysing the English
  Collections": harvesting
  tools](https://england.prm.ox.ac.uk/englishness-harvesting-tools.html). *Form and use are
  verified. No medieval example on the page.*
- **Lore.** Threshing is winter work, done on the barn floor when the fields are shut. The
  Flinchless thresh in pairs and stop at a fixed count, because a man who cannot feel the
  swingle catch his own head must be watched by another.
- **Fitting vocations.** Farmer.

#### Scythe

- **Description.** A long steel blade fastened by a tang at its heel to a shaft (the snath
  or sned), usually of willow or ash, with two hand grips. The mower keeps the heel of the
  blade close to the ground and the toe a little above it, and a good mower cuts about an
  acre a day. Good mowing depends on whetting the blade often with a stone. Source: [Tools
  and Trades History Society, "The Art of the Scythe" (S. Tallents,
  1947)](https://www.taths.org.uk/tools-trades/articles/51-the-art-of-the-scythe).
  *Partially verified: form and use only, with no medieval attestation on the page.*
- **Lore.** Hay harvest is when a farm hires, and a day labourer with a scythe and a
  whetstone of his own is paid half again what a field hand gets. The Noonfolk mow from
  first light and lay the scythe down where they stand at sunset, so that nobody trips on it
  in the dark they never see.
- **Fitting vocations.** Farmer, Labourer.

### Gardener

#### Spade iron

- **Description.** Medieval spades generally had wooden blades, sometimes fitted with a
  separate iron cutting edge. A U-shaped spade iron from Wick in the Vale of Glamorgan is
  274 mm long and was held to the wooden blade by a groove along its inner edge and two
  lugs. In 1356 the stock of a London ironmonger included a "spadierne", a spade iron.
  Source: [Medieval London (Fordham University), spade blade from the Museum of
  London](https://medievallondon.ace.fordham.edu/items/show/499), [Amgueddfa Cymru - Museum
  Wales, iron spade
  2018.8H](https://museum.wales/collections/online/object/48a5411b-ff46-3339-9d78-816a294335ee).
- **Lore.** Iron is too dear to make a whole spade of, so only the edge is shod and the
  smith re-lays it when it wears thin. A Halfling garden plot is dug with a spade cut to its
  owner's height, and lending it is a mark of friendship that is noticed.
- **Fitting vocations.** Gardener, Labourer.

#### Bee skep

- **Description.** A domed basket hive of woven straw, about 14 inches across, with willow
  strips inside to strengthen the dome. The bees fix their combs directly to the walls.
  Wicker hives of privet or hazel daubed with manure and clay were also used. Domed straw
  skeps were typical in the 16th century. Source: [Shakespeare Birthplace Trust,
  "Shakespeare in 100 Objects: Bee
  Skep"](https://shakespeare.org.uk/explore-shakespeare/blogs/shakespeare-100-objects-bee-skep).
  *The attestation on the page is 16th and early 17th century.*
- **Lore.** Honey is the sugar of every land too cold for cane, and a row of skeps under the
  orchard wall is a Halfling household's savings. The Stillfaces keep bees without smoke:
  they say a hive tells by its scent when it may be opened.
- **Fitting vocations.** Gardener.

#### Billhook

- **Description.** An iron blade with a hooked end set on a wooden handle, used for cutting
  branches. Museum Wales holds an excavated iron billhook from Radyr near Cardiff with an
  iron collar for its handle, catalogued as medieval or post-medieval. The record notes that
  the form has changed little since Roman times. Source: [Amgueddfa Cymru - Museum Wales,
  billhook
  80.46H/2.1](https://museum.wales/collections/online/object/74c03e95-85b9-331a-86ee-cbad1a65a448).
  *Partially verified. The dating of the object is uncertain by the museum's own record, and
  pruning vines with it is not on the page.*
- **Lore.** The vine dresser, the hedger and the firewood cutter carry the same hook, ground
  to three different edges. An Amberblood grove warden will let a stranger carry a billhook
  into the trees, but not an axe.
- **Fitting vocations.** Gardener, Forester.

### Herder

#### Shepherd's crook

- **Description.** A wooden shaft with a metal hook. The Food Museum example is 129 cm long
  and was used by a shepherd on a Suffolk farm from 1855. Source: [Food Museum, Stowmarket,
  crook STMEA:1987-3.2](https://foodmuseum.org.uk/ehive/65689). *Partially verified. The
  object is 19th century and the record does not explain the use. No medieval example was
  found.*
- **Lore.** A Midlander shepherd is hired at the autumn fair by standing with his crook in
  his hand. The Widegaze range herders carry one too, though they rarely need to walk to the
  sheep to know which one is missing.
- **Fitting vocations.** Herder.

#### Iron shears

- **Description.** Iron shears with two long blades and a round knob at the end of the
  handle, 137 mm long, from London, early 15th century. A manuscript of about 1475-1525
  (British Library, Harley 1892) shows a woman shearing a sheep with shears, and another
  (Harley 6563) a tailor cutting out a coat with them. Source: [Medieval London (Fordham
  University), "Shears"](https://medievallondon.ace.fordham.edu/items/show/470), [the
  collection page with both
  miniatures](https://medievallondon.ace.fordham.edu/collections/show/155). *The object
  record names no trade. The link to shearing and tailoring comes from the two miniatures.*
- **Lore.** Wool is the fibre of the cold and temperate lands, and the shearing of early
  summer brings the whole household to the fold. A tailor's shears are smaller and kept
  sharper, and in a town of the Canal folk they are weighed with the cloth so that no
  customer pays for what was trimmed away.
- **Fitting vocations.** Herder, Tailor.

#### Plunge churn

- **Description.** A tall coopered oak barrel bound with iron bands, tapering from the base
  to a loose collar. A plunger on a long wooden handle is worked up and down and turned to
  churn the cream to butter. Museum Wales says the type was common from the 1600s to the
  late 1800s. Source: [Amgueddfa Cymru - Museum Wales, churn
  62.323.2](https://museum.wales/collections/online/object/60b5dd48-dc3e-3c4f-b340-124a121fb7d2).
  *Partially verified. The page gives no attestation before 1600.*
- **Lore.** Butter and cheese are how milk keeps through winter, and the dairymaid's churn
  song sets the pace of the plunger. The Hearthbloods trade for butter by the tub: a people
  who must feed the fire inside them twice a day do not haggle long over fat.
- **Fitting vocations.** Herder.

### Fisher

#### Seine net

- **Description.** A seine is a fishing net weighted at the bottom and floated at the top,
  cast into the water and hauled in from the bank or a boat. 1,109 lead-alloy sinkers from
  such a net were found in a 15th-century wreck in the Thames at Blackfriars. The net is
  calculated at about 56.5 m long and needed several people to handle. Source: [London
  Museum, net sinkers
  26710](https://www.londonmuseum.org.uk/collections/v/object-36027/net-sinker-fishing-weights).
- **Lore.** A seine is too long for one family, so the net belongs to the boat's company and
  the catch is split by shares. The herring nets of the Northmen are barked brown with tree
  tannin each spring, and the Shoalfolk knot theirs of bast that does not rot in brackish
  water.
- **Fitting vocations.** Fisher.

#### Eel trap

- **Description.** A trap woven from green willow, with a mouth cone and a second, narrower
  inner cone that the eel must pass to reach the bait. The far end is closed with a stopper.
  Small traps (grigs) were laid at dusk, weighted with stones and baited with small fish or
  fish gut. Large ones (bucks) were mounted on stages across a river to take migrating eels.
  The Luttrell Psalter of about 1330 shows a wicker eel trap. Source: [The Fishing Museum
  Online (Medlar Press), "The Fine Art of Trapping Eels" by A.
  Herd](https://fishingmuseum.org.uk.medlarpress.com/eel_bucks.html). *A specialist article,
  not a museum record.*
- **Lore.** Eels are rent in many river lands: a mill or a weir owes its lord so many sticks
  of them a year. The osier for the trap is cut from the same willows that hold the bank.
- **Fitting vocations.** Fisher.

#### Eel spear

- **Description.** A hand-forged iron spear head with several barbed prongs, thrust into
  river mud to catch eels. The London Museum example is 430 mm long, came from the Thames
  foreshore and is dated to the 17th or 18th century. The record notes that eel spears were
  officially illegal. Source: [London Museum, eel spear
  DK2001.68](https://www.londonmuseum.org.uk/collections/v/object-542652/spear-eel-spear/).
  *Partially verified for the period: this example is post-medieval.*
- **Lore.** Where the fishing of a river is let to one holder, the spear is the poor man's
  tool and the water bailiff's grievance. The Hoarkin use a longer one through a hole in the
  sea ice, and have no bailiff to ask.
- **Fitting vocations.** Fisher.

### Hunter

#### Boar spear

- **Description.** A heavy hunting spear with a steel head on a wooden shaft. A German
  example of about 1550 in the Metropolitan Museum is 220 cm long overall, with a head of 33
  cm, and weighs about 1.7 kg. Source: [The Metropolitan Museum of Art through Google Arts &
  Culture, "Boar Spear"](https://artsandculture.google.com/asset/boar-spear/lgFfdo-_tDZslQ).
  *Partially verified. The record gives catalogue data only (about 1550) and says nothing of
  the use.*
- **Lore.** The wild boar of the broadleaf forest is game for a lord and a danger to a farm,
  so the spear hangs in halls and in barns alike. Where the Supernatural runs high the same
  spear is carried against worse, by the monster hunters who are paid by the head.
- **Fitting vocations.** Hunter.

#### Falcon's hood

- **Description.** A snug leather hood placed over a falcon's head to train the bird and to
  keep a trained falcon quiet and still on the falconer's glove. Most have a plume on top
  that also serves as a handle for taking it off. The V&A example, early 17th century and
  probably English, is tooled and gilded leather with embroidered velvet. Source: [Victoria
  and Albert Museum, falcon's hood
  T.244-1960](https://collections.vam.ac.uk/item/O318742/falcons-hood-unknown/). *The object
  is early 17th century.*
- **Lore.** The Grass riders fly eagles from the saddle and hood them in felt and horsehide.
  At a Midlander court the falconer ranks above the huntsman, and the hood of the lord's
  best bird is worth more than the falconer's coat.
- **Fitting vocations.** Hunter.

#### Hunting horn

- **Description.** A horn carved from an elephant tusk, called an oliphant. The V&A example
  is 54.5 cm long, from southern or central Italy, of the first half of the 11th century.
  Its carved bands show a stag, a deer, dogs or wolves, mounted hunters with hawks and a
  mounted man blowing a horn. The museum takes this piece to be probably ceremonial. Source:
  [Victoria and Albert Museum, horn
  8035-1862](https://collections.vam.ac.uk/item/O313757/horn-unknown). *Partially verified.
  The record shows a hunter blowing a horn but does not describe the use in the hunt.*
- **Lore.** A plain hunter's horn is a cow's. A lord's is ivory traded up from the Sunlander
  savanna or walrus tooth bought from the Hoarkin, and each call on it has one meaning that
  every beater knows.
- **Fitting vocations.** Hunter.

#### Longbow

- **Description.** A bow made from a single piece of yew, 1.84 to 2.11 m long, mostly
  D-shaped in section. The draw weights are estimated at 65 to 175 lb. 172 were recovered
  from the Mary Rose, with over 2,000 complete arrows, mostly of poplar. They are the only
  English longbows that can be dated precisely. Source: [Mary Rose Trust, "Longbows and
  arrows"](https://maryrose.org/discover/collections/the-weaponry-of-the-mary-rose/longbows-and-arrows/).
  *The page describes a weapon of war. Its use by hunters and poachers is the judgement of
  this document.*
- **Lore.** Yew is a timber of the map, and a kingdom that depends on archers buys staves
  from wherever it grows. Elven and Amberblood bowyers are counted the best, and the same
  bow that wins a lord's battle takes his deer on the way home.
- **Fitting vocations.** Hunter, Soldier, Bandit.

### Forester

#### Felling axe

- **Description.** The Science Museum Group holds two medieval iron axe heads dated to the
  14th or 15th century, catalogued among its hand tools. Source: [Science Museum Group,
  medieval iron axe head
  1925-970/1](https://collection.sciencemuseumgroup.org.uk/objects/co8629101/medieval-iron-axe-head).
  *Partially verified. The record gives the object, the material and the date only, and does
  not state that it was used for felling.*
- **Lore.** Timber is the first resource a new settlement spends, and the woodcutter's axe
  is what it spends it with. Elves count the trees that fall to it, and the Amberblood
  wardenships hold that one who cuts without leave owes the grove a life: usually not his
  own, but a sapling tended for twenty years.
- **Fitting vocations.** Forester.

#### Two-man saw

- **Description.** A long saw worked by two men to cut timber lengthwise. The top sawyer
  stood on the log and guided the saw, and the bottom sawyer pulled it from underneath. In
  the Middle Ages the log was raised on trestles, as medieval drawings show. Sawpits were in
  use by the early 16th century and in time replaced trestles on well-drained ground.
  Source: [Weald & Downland Living Museum, "Sawpit Shed from Sheffield
  Park"](https://www.wealddown.co.uk/buildings/sawpit-shed-from-sheffield-park/). *The page
  does not describe the blade itself.*
- **Lore.** The top sawyer is the master and the one below takes the sawdust in his eyes,
  which is why "under-sawyer" is an insult in most timber towns. Shipyards and great halls
  keep a pair of sawyers the year round.
- **Fitting vocations.** Forester, Carpenter.

Also used by the Forester: [Billhook](#billhook) (under Gardener).

### Miner

#### Miner's iron and hammer

- **Description.** Agricola describes iron tools of one shape, broad and square at the top
  to take the hammer and pointed at the bottom, most with an eye for a wooden handle. The
  one in daily use is three quarters of a foot long. The miner holds the handle in one hand,
  sets the point against the rock and strikes the head with a hammer. Small hammers are held
  in one hand, larger ones swung with both. Source: [Georgius Agricola, De re metallica
  (1556), trans. H. C. and L. H. Hoover, Book VI (Internet Archive full
  text)](https://archive.org/stream/deremetallica50agri/deremetallica50agri_djvu.txt).
- **Lore.** Crossed, the iron and the hammer are the badge on every Dwarven mine gate and on
  the seal of every mining contract kept behind it. A Dwarf is buried with the iron he was
  given as an apprentice, worn to half its length.
- **Fitting vocations.** Miner.

#### Wedge, crowbar and pick

- **Description.** Agricola's wedge is driven into cracks in the rock with the largest
  hammer. His crowbars are of two kinds, both pointed at one end: a round one to pierce
  through to a flooded shaft and a flat one to knock out rock that fire has softened. The
  miner's pick is pointed, unlike the peasant's wide one, and digs the softer ore. Source:
  [Georgius Agricola, De re metallica (1556), trans. Hoover, Book VI (Internet Archive full
  text)](https://archive.org/stream/deremetallica50agri/deremetallica50agri_djvu.txt).
  *Verified for mining. Agricola does not describe quarrying here; for the quarry see the
  next two tools of the Quarryman.*
- **Lore.** Hard rock is first cracked with a wood fire and then prised out with wedge and
  bar, so a mine eats timber and fuel before it yields any ore. The Hearthbloods work the
  high winter mines that no one else can stand, and are paid in food.
- **Fitting vocations.** Miner, Quarryman.

#### Windlass

- **Description.** A wooden barrel resting in iron sockets on posts set over the shaft,
  turned by cranks at its ends. A rope wound round the barrel has an iron hook at each end
  for the bail of a bucket. Two strong men turn it to raise buckets of ore, earth or water.
  A second form adds a wheel, which can carry lead weights to keep it turning. Source:
  [Georgius Agricola, De re metallica (1556), trans. Hoover, Book VI (Internet Archive full
  text)](https://archive.org/stream/deremetallica50agri/deremetallica50agri_djvu.txt).
- **Lore.** Windlass work is where a mine puts its hired hands and its greenest apprentices:
  Orc quarry hands and porters are sought for it. A well digger in the dry lands of the
  Saltroad folk uses the same machine and calls it by the same word as the well.
- **Fitting vocations.** Miner.

#### Divining rod

- **Description.** A forked twig, usually cut from hazel. Agricola (1556) reports that
  dowsers used hazel for silver, ash for copper, pitch pine for lead and tin, and rods of
  iron or steel for gold. The dowser grips the two branches in clenched fists and walks over
  the hills, and the twig is said to turn when his feet are over a vein. Agricola is
  sceptical: he puts the motion down to the holder and concludes that a forked stick is of
  no use to a prudent miner. Source: [Georgius Agricola, De re metallica (1556), trans.
  Hoover, Book II (Project Gutenberg
  mirror)](https://gutenberg.pglaf.org/3/8/0/1/38015/38015-h/38015-h.htm).
- **Lore.** Dwarven prospectors read the rock and scorn the rod in public. The well-finders
  of the Duskreaders, who see warmth as others see light, carry one mostly so that the
  customer has something to watch.
- **Fitting vocations.** Miner, Seer.

### Quarryman

#### Quarry wedges

- **Description.** Small blocks were split by hammering in wedges, the large wedges driven
  with sledge hammers. For larger pieces, shallow holes were drilled about 23 cm apart,
  split iron rods (feathers) dropped in and iron wedges (plugs) driven between them to give
  a clean break. Edinburgh records of October 1531 note old picks and mattocks remade into a
  mattock and six new wedges. Source: [British Geological Survey, "Building stones of
  Edinburgh: quarrying
  methods"](https://earthwise.bgs.ac.uk/index.php/Building_stones_of_Edinburgh:_quarrying_methods).
  *The page does not date the plug and feathers to the Middle Ages.*
- **Lore.** A quarryman reads the bed of the stone before he sets a wedge, and a line of
  wedges struck in turn rings higher just before the block lets go. The Noonfolk quarry by
  daylight only and leave the wedges standing in the stone at dusk.
- **Fitting vocations.** Quarryman.

#### Quarry pick

- **Description.** An iron pick used, with mattocks, to uncover the stone and to lever
  blocks off the bed. Edinburgh's Masters of Works paid in July 1530 for sharpening three
  dozen picks at two quarries. In Bath stone quarrying a long-handled pick cut a slot along
  the top of the bed so that a saw could enter. Source: [British Geological Survey,
  "Building stones of Edinburgh: quarrying
  methods"](https://earthwise.bgs.ac.uk/index.php/Building_stones_of_Edinburgh:_quarrying_methods),
  [Bradford on Avon Museum, "quarrying and
  stone-masons"](https://www.bradfordonavonmuseum.co.uk/?p=1910).
- **Lore.** Picks blunt in a day on granite, so every quarry of any size feeds a smith who
  does little else but draw out their points. Millstone cutters work with a lighter pick and
  are paid by the finished stone, not by the day.
- **Fitting vocations.** Quarryman.

#### Crowbar and sledge

- **Description.** Crowbars or picks were used to lever the stone off its bed so that it
  could be lifted with a crane, the cranes at first worked by men or horses. Sledge or
  striking hammers drove the large splitting wedges. The Edinburgh records note repairs to
  heavy hammers, picks and crowbars. Source: [British Geological Survey, "Building stones of
  Edinburgh: quarrying
  methods"](https://earthwise.bgs.ac.uk/index.php/Building_stones_of_Edinburgh:_quarrying_methods).
- **Lore.** The sledge is quarry-hand work, and an Orc who swings one well can find a place
  at any stone face between the steppe and the sea. Marble and alabaster are levered, never
  struck, and the men who do it say so often.
- **Fitting vocations.** Quarryman.

Also used by the Quarryman: [Wedge, crowbar and pick](#wedge-crowbar-and-pick) (under
Miner).

### Salter

#### Lead salt pan

- **Description.** A shallow pan of lead in which brine is boiled over a fire. By the 14th
  century the pans were made of sheets of cast lead beaten up at the sides, about 170 cm by
  90 cm. Lead pans were used in Cheshire for some fifteen centuries and gave way to iron
  pans in the 17th century, when coal replaced wood as the fuel. Scale was picked off the
  base with an iron tool and a damaged pan was recast. Source: [Salt Association, "Salt
  During Our Early
  History"](https://saltassociation.co.uk/education/salt-history/early-history/), ["Salt in
  the Middle Ages"](https://saltassociation.co.uk/education/salt-history/middle-ages/).
  *Trade-association history pages, not a museum or a treatise.*
- **Lore.** A boiling house is counted by its pans, and among the Brinefolk a pan master
  answers for every one of his by name. The lead comes from the same veins as the silver, so
  a salt town and a mining town are often bound by one old contract.
- **Fitting vocations.** Salter.

#### Salt rake and shovel

- **Description.** In Agricola's account of making salt from sea water, the salt that forms
  in the basins is raked together with wooden rakes and thrown out with shovels. Source:
  [Georgius Agricola, De re metallica (1556), trans. Hoover, Book XII (scan at e-rara, ETH
  Zurich)](https://www.e-rara.ch/ebs/content/pagetext/28966762). *Partially verified: from a
  search excerpt of Book XII. The page itself could not be opened.*
- **Lore.** The rakes are wood because brine eats iron within a season. A salt raker of the
  cold desert flats works barefoot on the crust, and the Brinefolk hold that salt raked by
  one who has wept into the pan must not be sold.
- **Fitting vocations.** Salter.

#### Salt basket

- **Description.** In Agricola's boiling house the helper sets two baskets, two feet high
  and as wide at the top and a palm wide at the bottom, on beams. The master throws the salt
  into them with a shovel, and in the baskets it takes the form of a cone. Source: [Georgius
  Agricola, De re metallica (1556), trans. Hoover, Book XII (scan at e-rara, ETH
  Zurich)](https://www.e-rara.ch/ebs/content/pagetext/28966768). *Partially verified: from a
  search excerpt of Book XII. The page itself could not be opened.*
- **Lore.** The cone of salt that comes out of the basket is the unit of the trade: caravans
  of the Saltroad folk count their load in cones, and a weigh-master's stamp goes on the
  flat end.
- **Fitting vocations.** Salter.

---

## Processing

### Miller

#### Millstones

- **Description.** A pair of grinding stones. The runner stone is the one that turns: it is
  very slightly concave, with a central eye where the grain enters. The faces carry large
  grooves (furrows) and flat areas (lands). The furrows feed the grain out onto the lands
  and let air cool the stones. Source: [Friends of New Hall Mill, "Dressing the
  Millstones"](https://www.newhallmill.uk/dressing-the-millstones/). *A working-mill
  volunteer body, not a museum. The page gives no medieval date.*
- **Lore.** Good millstone is a variant of the Stone resource and travels far: a pair is the
  costliest thing in a mill after the wheel. Lords who own the mill forbid hand querns, and
  the miller who takes his toll of every sack is the least trusted man of the countryside.
- **Fitting vocations.** Miller.

#### Mill bill and thrift

- **Description.** A hand tool for dressing millstones, which is re-cutting the grooves in
  the grinding faces. The steel head (the bill), somewhere between a pick and a chisel, is
  held in a wooden handle called the thrift. Source: [Science Museum Group, thrift and
  mill-bills
  1966-395](https://collection.sciencemuseumgroup.org.uk/objects/co154189/collecton-of-thrift-and-mill-bill),
  [Friends of New Hall Mill, "Dressing the
  Millstones"](https://www.newhallmill.uk/dressing-the-millstones/). *Form and use are
  verified. The objects are undated and no medieval example was found.*
- **Lore.** A stone dresser travels from mill to mill, and millers ask to see his hands: the
  backs of them are blue with steel splinters if he has done the work he claims. The
  Flinchless are barred from the trade by their own law, since they would not feel the
  splinters go in.
- **Fitting vocations.** Miller.

#### Rotary quern

- **Description.** A hand mill of two stones. The upper stone has a funnel-shaped hole at
  its centre for the grain and a hole in the side for a handle. Querns were mainly used to
  grind grain by hand into flour for bread. The London Museum example is Roman, 31 cm
  across. Source: [London Museum, quern
  NN50774](https://www.londonmuseum.org.uk/collections/v/object-142445/quern/). *The example
  is Roman. Medieval use was not sourced.*
- **Lore.** Where there is no stream for a mill, on the steppe and the dry scrub, flour is
  still ground by hand each morning. In a milled land a quern in the house is either a
  memory or a crime.
- **Fitting vocations.** Miller.

### Baker

#### Baker's peel

- **Description.** A flat shovel used to get bread and other goods in and out of the oven.
  It is the device on a farthing trade token issued by a London baker in 1659. Source:
  [London Museum, "The Baker's Peel", token
  96.66/223](https://www.londonmuseum.org.uk/collections/v/object-431578/the-bakers-peel/).
  *The only opened source is a depiction of 1659, not a medieval object.*
- **Lore.** The keeper of a common oven bakes the dough of the whole street and knows every
  household by the mark pricked in its loaf. A peel hung over a door is the baker's sign in
  towns where few can read.
- **Fitting vocations.** Baker.

#### Kneading trough

- **Description.** The ark (or hutch) is an oak chest used to store grain, meal or bread
  from the medieval period onwards. Its concave top could normally be lifted off and turned
  over to form a kneading trough for making bread. The V&A example is English, of panelled
  oak, 1550-1650. Source: [Victoria and Albert Museum, ark
  W.54-1950](https://collections.vam.ac.uk/item/O130488/ark-unknown/).
- **Lore.** A baker's trough is never washed out fully: the scrapings carry the leaven from
  one day's batch to the next, and a trough of a good line is part of a daughter's dowry.
  Halfling bakers name theirs.
- **Fitting vocations.** Baker.

### Brewer

#### Mash tun and masher

- **Description.** The mash tun is the vessel in which the mash is made: oak tuns were built
  by a cooper for a university recreation of 16th-century brewing in Dublin. The mash is
  mixed with a wooden masher. The Food Museum example is a five-barred wooden frame on a
  shaft 124 cm long. Source: [Food Museum, Stowmarket, masher
  STMEA:B.230](https://foodmuseum.org.uk/ehive/369923), [Trinity College Dublin, on
  recreating 16th-century
  beer](https://www.tcd.ie/news_events/articles/2024/five-things-our-research-uncovered-when-we-recreated-16th-century-beer-and-barrels/).
  *Form and use are verified. The masher is undated.*
- **Lore.** Brewing is by-work on most farms and the alewife's own income. Dwarves brew
  behind the gate from grain seven years stored and hold that a masher must never be lent,
  as it carries the house's ale in its wood.
- **Fitting vocations.** Brewer.

#### Brewing kettle

- **Description.** A large kettle set over an open fire, in which the brewer stirs the brew.
  The oldest picture of a brewer in the Mendel housebook of Nuremberg (1425) shows a brewing
  kettle with two rings on its sides and two wooden vats beside it. The Dublin recreation of
  16th-century brewing used a large copper boiler made by coppersmiths. Source: [Schlenkerla
  brewery, historical depictions of
  brewing](https://www.schlenkerla.de/biergeschichte/brauerstern/html/brauerdarstellungene.html),
  [Trinity College Dublin, on recreating 16th-century
  beer](https://www.tcd.ie/news_events/articles/2024/five-things-our-research-uncovered-when-we-recreated-16th-century-beer-and-barrels/).
  *Partially verified. The first page is a brewery's history page that describes the
  manuscript image, not the library that owns it.*
- **Lore.** The copper is the brewer's capital: a town brewer is taxed by its size, and the
  official's sworn measurer comes with a rod to gauge it. In the north bog myrtle goes into
  the kettle, further south hops.
- **Fitting vocations.** Brewer.

#### Alembic and cucurbit

- **Description.** A two-part still. The cucurbit, a gourd-shaped vessel, holds the
  substance and is heated. The vapour rises into the alembic set on top, condenses on its
  inner surface, runs into a channel and out through a spout. A ceramic alembic from London
  of about 1240-1400 is 28 cm high, and the record says apothecaries used such vessels to
  make healing waters. A glass set from the Middle East is dated 901-1200. Source: [London
  Museum, pottery
  alembic](https://www.londonmuseum.org.uk/collections/v/object-534829/pottery-vessel-alembic),
  [Science Museum Group, Islamic alembic
  1978-219](https://collection.sciencemuseumgroup.org.uk/objects/co19063/islamic-alembic).
- **Lore.** The distiller draws spirit from wine and the perfumer rose oil from petals with
  the same vessel, and the Bitterbloods draw the antidotes they sell. The vial blowers of
  the Lanternkin make alembics of clear glass so thin that the work inside can be watched.
- **Fitting vocations.** Brewer, Alchemist.

### Butcher

#### Cleaver

- **Description.** A large knife or cleaver with a broad, straight-backed iron blade and a
  wooden handle that widens toward the end. The London Museum example is late 15th or early
  16th century, 31 cm long and 7 cm wide. The record links it to jointing, carving and
  slicing meat, fish and pies. Source: [London Museum, cleaver
  9354](https://www.londonmuseum.org.uk/collections/v/object-36013/cleaver/). *The record
  does not name the butcher's trade.*
- **Lore.** Butchers are kept to one street and the downstream end of town, and their guild
  is among the richest. The Catfolk, who cannot eat grain, are butchers and tanners wherever
  they settle, and an Orc butcher is said to waste nothing at all.
- **Fitting vocations.** Butcher.

#### Pole axe

- **Description.** A long-handled axe with a steel head on a wooden handle, used to fell or
  stun animals at slaughter. The Science Museum example is English, 1880-1925. Source:
  [Science Museum Group, pole axe for animal slaughter
  1987-1040](https://collection.sciencemuseumgroup.org.uk/objects/co144553/pole-axe-for-animal-slaughter-pole-axes).
  *Partially verified. The object is 19th or 20th century and no medieval attestation was
  opened.*
- **Lore.** The autumn slaughter, when the beasts that cannot be fed through winter are
  killed and salted, is the slaughterer's season and the salter's. One clean blow is the
  measure of the trade.
- **Fitting vocations.** Butcher.

### Charcoal burner

#### Charcoal clamp

- **Description.** A stack of about three tons of timber built round a central chimney and
  blanketed in hay, turf and green branches to control the air. It is lit down the chimney
  and tended day and night for about 24 hours, with gaps in the covering mended, so that the
  wood cooks at around 400 degrees and does not burn. It is then sealed with earth to
  smother the fire, cooled and opened. The museum says the practice reaches back to the
  Bronze Age. Source: [Weald & Downland Living Museum, "Charcoal
  burning"](https://www.wealddown.co.uk/museum-news/charcoal-burning/).
- **Lore.** A collier lives in the wood beside his clamp for the whole burn and sleeps on a
  one-legged stool, so that he falls over if he nods off. Smelters and smiths need charcoal
  more than they need ore, and the Elves hold the collier in deeper dislike than the
  woodcutter.
- **Fitting vocations.** Charcoal burner.

#### Corrack and long shovel

- **Description.** The charcoal burner's rake, known as a corrack, is used to rake out the
  clamp at the end of the burn. A long-handled shovel is used to tend the dome of the clamp.
  Other kit includes an axe, a water barrel with buckets and a yoke, a basket for the
  charcoal and sacks. The evidence is from the Wyre Forest in Worcestershire and undated.
  Source: [Revolutionary Players (History West Midlands, objects of Bewdley Museum),
  "Charcoal Burning Tools (4)"](https://revolutionaryplayers.org.uk/?p=2386), ["Charcoal
  Burning Tools (1)"](https://www.revolutionaryplayers.org.uk/?p=2377). *Undated traditional
  practice with no medieval attestation.*
- **Lore.** The long shovel patches the turf where the clamp breathes, and the collier reads
  the colour of the smoke to know where. Duskreaders see the heat through the cover itself
  and are hired as kiln readers for the largest burns.
- **Fitting vocations.** Charcoal burner.

#### Lime kiln

- **Description.** A structure used to make lime by burning limestone or chalk, and now and
  then shells, at more than 900 degrees. Lime burning seems to have come to England with the
  Romans. Most kilns supplied lime for the mortar of a building beside them. Source:
  [Historic England, "Pre-industrial Lime Kilns" (Introductions to Heritage
  Assets)](https://historicengland.org.uk/images-books/publications/iha-preindustrial-lime-kilns).
  *From the summary of the publication. The form of the kiln and its fuel are not described
  there.*
- **Lore.** Every castle, temple and town wall begins with a lime kiln at its foot, and the
  lime burner is the first craftsman on the site and the first to leave. The chalk and
  limestone lands sell lime to the granite lands that have none.
- **Fitting vocations.** Charcoal burner, Mason.

### Smelter

#### Bloomery furnace

- **Description.** A furnace built of clay, sometimes also stone, with thick walls to hold
  the heat and an inside diameter of 0.3 to 1 m at the base, fuelled with charcoal. Air is
  blown in through holes in the side, sometimes through clay tubes (tuyeres). The iron does
  not melt: it sticks together as a spongy lump, the bloom, and in some furnaces molten slag
  is tapped off near the ground. The bloomery was in use in England from about the 8th
  century BC and gave way to the blast furnace from the late 15th century. Source: [Historic
  England, "Pre-industrial Ironworks" (Introductions to Heritage
  Assets)](https://historicengland.org.uk/images-books/publications/iha-preindustrial-ironworks/heag221-pre-industrial-ironworks).
- **Lore.** The Sunlanders were the first to smelt iron, and their furnace masters still
  open a smelt with words that no outsider is taught. Bog iron in the cold forest, ironstone
  on the savanna: the furnace is the same clay shaft, rebuilt for every few smelts.
- **Fitting vocations.** Smelter.

#### Bellows

- **Description.** Bellows blow air into furnaces and hearths to reach the heat needed. In
  the smith's forge a pair is set behind the hearth with its nose in the tuyere of the back
  plate. One board is fixed, and a rocker with a handle raises the moving board, which a
  weight brings down again. From about the 14th century water power began to drive the
  bellows of bloomery furnaces. Source: [Historic England, "Pre-industrial
  Ironworks"](https://historicengland.org.uk/images-books/publications/iha-preindustrial-ironworks/heag221-pre-industrial-ironworks),
  [Joseph Moxon, Mechanick Exercises (3rd ed., 1703), smithing (Internet Archive full
  text)](https://archive.org/stream/mechanickexercis00moxo_1/mechanickexercis00moxo_1_djvu.txt).
  *The construction is from Moxon, a treatise of 1703. The medieval use and the water power
  are from Historic England.*
- **Lore.** The forge hand at the bellows is the lowest place in a smithy and the one every
  master started in. Hearthbloods are welcome there in any land: they do not mind the heat,
  and they are fed at the forge.
- **Fitting vocations.** Smelter, Smith.

#### Cupel

- **Description.** A small dish of bone ash with a shallow hollow. In cupellation the metal
  is melted with lead, which oxidises to litharge, takes up the base metals and soaks into
  the dish, leaving the precious metal. Small cupels were used in fire assay to measure the
  precious metal in a sample of ore or metal. A London example of the 15th or 16th century
  is 37 mm across and still holds droplets of silver. Source: [London Museum, cupel
  WFG18[214]<103>](https://www.londonmuseum.org.uk/collections/v/object-526525/cupel/).
- **Lore.** Silver comes out of lead ore only by this dish, so the silver refiner and the
  assayer are the same craft at two sizes. Gnome assayers stamp the bead as well as the bar,
  and a Lanternkin banker will not take a bar without the bead beside it.
- **Fitting vocations.** Smelter, Jeweller.

### Tanner

#### Unhairing knife and beam

- **Description.** After the hides have lain in lime pits to loosen the hair, fat and flesh,
  the hair is scraped off with a blunt-bladed unhairing knife, curved to fit the rounded
  beam over which the hide is laid. The example from the Rhayader tannery is 72 cm long. The
  hair was sold to plasterers and masons for mortar. Source: [Amgueddfa Cymru - Museum
  Wales, unhairing knife
  68.121.293](https://museum.wales/collections/online/object/c6a8624e-f5ef-3035-a8d3-ff6bb2f62bb8?page=45).
  *From a tannery of the 18th and 19th centuries. No medieval attestation.*
- **Lore.** The beam man stands bent over wet hide all day, and the trade is known by its
  stoop and its smell. Tanners are set downstream and downwind of every town, beside the
  butchers who supply them.
- **Fitting vocations.** Tanner.

#### Tan pits

- **Description.** Pits in which raw hides are soaked in an increasingly strong solution of
  ground oak bark. The whole process took eighteen months. The Rhayader tannery, of the late
  18th century and now at St Fagans, ground its bark in a water-powered bark mill. Source:
  [Amgueddfa Cymru - Museum Wales, St Fagans,
  "Tannery"](https://museum.wales/stfagans/buildings/tannery/). *The process is verified
  from a tannery of the 18th century. Medieval dating was not opened.*
- **Lore.** A hide spends a year and a half in the pits, so a tanner's wealth is always
  under water and a tannery passes from parent to child half full. Tanbark is a by-product
  the forester sells; the Brinefolk tan with salt and alum of the dry lake beds.
- **Fitting vocations.** Tanner.

#### Lunellum and herse

- **Description.** After liming, unhairing and fleshing, the skin is fixed to a frame, the
  herse, and dries under tension. Once stretched it is thinned and evened by scraping with a
  curved knife called a lunellum, until it is fit for writing or binding. Source: [Cornell
  University Library Conservation,
  "Parchment-making"](https://blogs.cornell.edu/culconservation/2015/04/03/parchment-making/),
  [HMML School lexicon, "Herse"](https://hmmlschool.org/lexicon/149229/). *The Cornell page
  describes a present-day workshop that works in the traditional way. The medieval use of
  the knife is not on it.*
- **Lore.** Every contract of the Dwarves, every genealogy of the Halflings and every ledger
  of the Saltroad folk begins as a calf or a goat on a herse. The Canal folk write on paper
  and regard parchment as a waste of a good animal.
- **Fitting vocations.** Tanner.

### Weaver

#### Spindle, whorl and distaff

- **Description.** A wooden drop spindle carries a pierced weight, the whorl, whose weight
  keeps the spindle turning while the fibres are twisted into yarn. Whorls are of lead,
  stone, ceramic, bone, antler or wood. The technique runs in Britain from the Iron Age to
  early post-medieval times. In medieval Scandinavia a distaff with notches along its upper
  part held the fibre. Source: [Portable Antiquities Scheme, "Spindle Whorls" recording
  guide](https://finds.org.uk/counties/findsrecordingguides/spindle-whorls/), [M. Hoffmann,
  "Clothmaking", in Medieval Scandinavia: An Encyclopedia (1993), hosted by UC
  Davis](https://medieval.ucdavis.edu/SWEDEN/Clothmaking.html).
- **Lore.** Spinning fills every idle hour of every woman of the wool and flax lands, and it
  takes several spinners to keep one loom fed. A whorl is the commonest thing a traveller
  finds in an abandoned house, and the Twofold bury the one a person spun with in the
  Staying half of life.
- **Fitting vocations.** Weaver.

#### Loom

- **Description.** The warp-weighted loom has two uprights about 2.2 m high carrying a beam,
  with a shed rod and one to three heddle rods. The warp is held taut by loom weights, in
  Norway mostly of soapstone. It was the common loom for everyday cloth in Iceland and
  Norway. The horizontal loom with treadles is documented in 11th-century France and seems
  to have reached Sweden around 1200. Source: [M. Hoffmann, "Clothmaking", in Medieval
  Scandinavia: An Encyclopedia (1993), hosted by UC
  Davis](https://medieval.ucdavis.edu/SWEDEN/Clothmaking.html).
- **Lore.** The upright loom stands in the farmhouse and the treadle loom in the town
  workshop: the first makes a household's cloth, the second a guild's. The Rainkeepers weave
  cotton, the Canal folk silk, and the Boughwalkers paint finished cloth in a colour nobody
  else can see.
- **Fitting vocations.** Weaver.

#### Cropping shears and teasels

- **Description.** To finish woollen cloth, a tool set with teasels raises the nap. Then
  large, heavy hand shears are worked across the cloth, spread on a cropping table, to shave
  the surface smooth, and a hand brush sweeps off the loose wool. The workers were the
  croppers or shearmen. Source: [Science Museum Group, "Cropping Shears, Bradford Industrial
  Museum"](https://collection.sciencemuseumgroup.org.uk/objects/co8647038/cropping-shears-bradford-industrial-museum),
  [MyLearning (Leeds Museums and Galleries), "A Cropping
  Table"](https://mylearning.org/resources/a-cropping-table). *Form and use are verified
  from 18th- and 19th-century Yorkshire. A medieval date is not supported by the pages.*
- **Lore.** The shearman is the best paid hand in the cloth trade, for one slip of the
  shears ruins a bolt that took a month to weave. Teasels are a garden crop, grown beside
  the dye plants.
- **Fitting vocations.** Weaver.

---

## Craft

### Smith

#### Anvil

- **Description.** An iron block whose flat top, the face, is hardened steel, so hard that a
  file will not touch it. Some anvils have a pointed end, the pike, bickern or beak-iron,
  for hollow work. The anvil is set on a wooden block about two feet high, or as suits the
  workman. Source: [Joseph Moxon, Mechanick Exercises (3rd ed., 1703), smithing (Internet
  Archive full
  text)](https://archive.org/stream/mechanickexercis00moxo_1/mechanickexercis00moxo_1_djvu.txt).
  *Form and use are verified from a treatise of 1703.*
- **Lore.** An anvil outlasts its smith and is willed like land. Northmen honour Dwarves as
  the only smiths better than their own, and a Dwarven anvil that has left the hold is
  either a king's gift or stolen. Star metal, where it falls, is said to mark the face that
  works it.
- **Fitting vocations.** Smith.

#### Hammers and tongs

- **Description.** The hand-hammer is light enough to wield with one hand at the anvil. The
  up-hand sledge is held in both hands and seldom raised above the head, and the
  about-sledge, the largest, is swung overhead for the heaviest work: both are used by the
  under-workmen. Straight-nosed tongs hold short, flat work, and crooked-nosed tongs hold
  small bars within their curved jaws. Source: [Joseph Moxon, Mechanick Exercises (3rd ed.,
  1703), smithing (Internet Archive full
  text)](https://archive.org/stream/mechanickexercis00moxo_1/mechanickexercis00moxo_1_djvu.txt).
  *Verified from a treatise of 1703.*
- **Lore.** The master taps where the blow should fall and the striker's sledge follows. The
  Flinchless make strikers who never flag, and a Duskreader temper judge stands by to call
  the heat of the iron by its glow in the dark of the shop.
- **Fitting vocations.** Smith.

Also used by the Smith: [Bellows](#bellows) (under Smelter).

### Carpenter

#### Carpenter's axe

- **Description.** A deep, heavy axe, heavier than the joiner's hatchet, with a long handle
  because it is used with both hands. It squares or bevels timbers, which are propped on
  logs so that the edge does not strike the ground. On the warship Mary Rose axes served
  both to chop wood and to trim timbers with care. Source: [Joseph Moxon, Mechanick
  Exercises (3rd ed., 1703), house
  carpentry](https://archive.org/stream/mechanickexercis00moxo_1/mechanickexercis00moxo_1_djvu.txt),
  [Mary Rose Trust, "Meet the Carpenter"](https://maryrose.org/meet-the-carpenter/).
- **Lore.** A beam is hewn square long before a saw touches it, and the marks of the axe
  show on every rafter of a Northman hall. A carpenter is found in every patch of the
  countryside, where he is also the wheelwright, the cooper and the coffin maker.
- **Fitting vocations.** Carpenter, Shipwright.

#### Adze

- **Description.** A tool with a thin, somewhat arched blade whose edge lies across the
  handle. It takes thin chips off timber or boards and removes what the axe cannot reach.
  The workman holds the end of the handle in both hands and stands astride the work. On the
  Mary Rose the adze was used to smooth timbers. Source: [Joseph Moxon, Mechanick Exercises
  (3rd ed., 1703), house
  carpentry](https://archive.org/stream/mechanickexercis00moxo_1/mechanickexercis00moxo_1_djvu.txt),
  [Mary Rose Trust, "Meet the Carpenter"](https://maryrose.org/meet-the-carpenter/).
- **Lore.** The Northmen say the first Shipwright shaped the sky's planking with an adze,
  and the stars are where he riveted it. Every curved timber of a hull is brought to its
  line by this tool, by eye.
- **Fitting vocations.** Carpenter, Shipwright.

#### Auger

- **Description.** A boring tool whose work is to make great round holes. Moxon shows it
  used in hanging doors, where the hole is bored slightly smaller than the hook so that the
  hook is forced in tight. A smaller piercer and a gimlet make the lesser holes. Source:
  [Joseph Moxon, Mechanick Exercises (3rd ed., 1703), joinery and house
  carpentry](https://archive.org/stream/mechanickexercis00moxo_1/mechanickexercis00moxo_1_djvu.txt).
  *Partially verified. The form of the handle was not in the parts of the treatise that were
  read.*
- **Lore.** A timber frame is held by oak pegs, not nails, so a house is as many auger holes
  as it is beams. Iron nails are for those near a smith with iron to spare.
- **Fitting vocations.** Carpenter.

#### Plane

- **Description.** A tool for smoothing and shaping wood, made in many sizes. On the Mary
  Rose large planes smoothed planks and beams and small ones did work such as smoothing
  arrows, and they were of wood, not metal. Moxon's joiner starts with the fore-plane and
  finishes with the jointer or the smoothing plane. Source: [Mary Rose Trust, "Meet the
  Carpenter"](https://maryrose.org/meet-the-carpenter/), [Joseph Moxon, Mechanick Exercises
  (3rd ed., 1703),
  joinery](https://archive.org/stream/mechanickexercis00moxo_1/mechanickexercis00moxo_1_djvu.txt).
- **Lore.** The plane tells the joiner of the town from the carpenter of the countryside:
  one makes chests and panelling, the other barns. Fletchers and bowyers keep planes no
  longer than a finger.
- **Fitting vocations.** Carpenter.

#### Pole lathe

- **Description.** A turner's lathe in which the work is held between two centres. A cord is
  wound two or three times round the work and runs from a springy pole above, often a fir
  pole fixed to a ceiling timber, down to a treadle. Treading down spins the work and the
  pole draws the treadle up again. The cutting is done with gouges, chisels and hooks.
  Source: [Joseph Moxon, Mechanick Exercises (3rd ed., 1703),
  turning](https://archive.org/stream/mechanickexercis00moxo_1/mechanickexercis00moxo_1_djvu.txt).
  *Verified from a treatise of 1703.*
- **Lore.** The turner sets his lathe up in the wood where the timber is, with a living
  sapling for the pole, and comes out in autumn with a cartload of bowls and chair legs.
  Every household eats from his work.
- **Fitting vocations.** Carpenter.

#### Plumb rule, level and square

- **Description.** The plumb rule is a rule about four feet long with a line and a lead
  plummet, used to keep work upright. The level is ten or twelve feet long and sets out
  foundations and checks walls as they rise. A large square sets walls at right angles.
  Without one, the workman measures 6 feet along one wall and 8 along the other and adjusts
  until the diagonal is 10. Source: [Joseph Moxon, Mechanick Exercises (3rd ed., 1703),
  bricklayers' work and house
  carpentry](https://archive.org/stream/mechanickexercis00moxo_1/mechanickexercis00moxo_1_djvu.txt).
  *Verified from a treatise of 1703.*
- **Lore.** The square and the plumb line are the master's tools and the signs of a masons'
  lodge. The Gnomes make them in brass for the canal engineers of the Canal folk, who level
  a dike over a day's walk with them.
- **Fitting vocations.** Carpenter, Mason.

Also used by the Carpenter: [Two-man saw](#two-man-saw) (under Forester).

### Shipwright

#### Caulking iron and mallet

- **Description.** Caulking irons are hand-forged, fishtail-shaped chisels. A making iron
  tucks rolled oakum into the seam between planks and a grooved hardening iron drives it
  firm. They are struck with a caulking mallet, which has a long, heavy wooden head bound
  with metal rings. The seam is then sealed with melted pitch. On the Mary Rose horsehair
  and rope were hammered into the gaps and sealed with pitch. Source: [Historic Environment
  Scotland, "The tools and techniques of traditional ship
  caulking"](https://blog.engineshed.scot/2026/07/02/traditional-ship-caulking/), [Mary Rose
  Trust, "Meet the Carpenter"](https://maryrose.org/meet-the-carpenter/). *Partially
  verified. Form and use are from a present-day conservation account. No page that was read
  attests the tools before 1500.*
- **Lore.** A shipyard is heard before it is seen, by the ring of the caulkers' mallets. The
  pitch comes from the tar burners of the boreal forest, and the Shoalfolk hull menders do
  the same work under water, by feel.
- **Fitting vocations.** Shipwright.

#### Clench nails and roves

- **Description.** Iron fastenings for clinker planking, in which the overlapping planks are
  fastened edge to edge. A rove is a small pierced metal plate over which the point of the
  nail is clenched, so that it cannot draw. Riveted planking from boats of the 12th and 13th
  centuries was excavated at Perth. The Viking Ship Museum's smith makes about 450 rivets
  and roves for a boat of 7.7 m and just under 8,000 for a longship of 30 m. Source: [C. M.
  Martin and N. Q. Bogdan, "The boat timbers", Perth High Street Excavation, Fascicule
  2](https://www.tafac.org.uk/wp-content/uploads/2023/09/PHSE-Fasc-2_p317-322.boattimbers.pdf),
  [Viking Ship Museum, Roskilde, "The
  smith"](https://www.vikingeskibsmuseet.dk/en/professions/education/all-about-the-viking-ship/skibets-haandvaerkere/smeden).
- **Lore.** The Northmen say the stars are the rivets of the upturned hull that is the
  world. A ship fellowship that owns a hull in shares counts the shares in rivets, and a new
  longship keeps a smith at the slip for a whole winter.
- **Fitting vocations.** Shipwright.

Also used by the Shipwright: [Carpenter's axe](#carpenters-axe) (under Carpenter),
[Adze](#adze) (under Carpenter).

### Mason

#### Mallet and chisels

- **Description.** The mason's mallet is round, so that it need not be held any particular
  way and wears evenly. It is heavy but of fairly soft material, since a metal hammer would
  damage the struck end of the chisel. Bolsters are wide-bladed chisels for clearing waste
  and dressing flat faces, and narrower chisels carve the finer detail. Source: [Bradford on
  Avon Museum, "quarrying and
  stone-masons"](https://www.bradfordonavonmuseum.co.uk/?p=1910). *Partially verified. Form
  and use are supported, and the page gives no dates.*
- **Lore.** A mason cuts his own mark into every block he dresses, and is paid by counting
  them. Dwarven masons use no mortar where a joint can be cut true, and a Duskreader wall
  mason works the cool of the night.
- **Fitting vocations.** Mason.

#### Treadwheel crane

- **Description.** A large wooden wheel, turned by men walking inside it, for hoisting
  loads. The original wheel survives high in the Bell Harry Tower of Canterbury Cathedral,
  which was finished in 1498. It is large enough for two men and lifted the stone, bricks
  and tools into place when the tower was built. Source: [Canterbury Cathedral Learning,
  "How did they build that?
  Stone"](https://learning.canterbury-cathedral.org/how-did-they-build-that/stone).
- **Lore.** The wheel is left in the tower when the work is done, since nothing but itself
  could lower it. Walking it is labourer's work, and the Boughwalkers, who have no fear of
  height, are the steeplejacks who rig it.
- **Fitting vocations.** Mason.

#### Lewis

- **Description.** A lifting cramp for large, heavy blocks of stone. For a chain lewis a
  narrow slot is cut in the top of the block and two curved pieces are put in back to back.
  When the crane pulls the chain they bite into the sides of the slot and carry the weight
  of the block. Source: [Bradford on Avon Museum, "quarrying and
  stone-masons"](https://www.bradfordonavonmuseum.co.uk/?p=1910). *Partially verified. Form
  and use are supported, and the page gives no date.*
- **Lore.** The lewis slot stays in the stone for ever, hidden in the joint. Ruins of the
  Old Realms show slots of a shape no living mason's lewis fits.
- **Fitting vocations.** Mason.

#### Trowel

- **Description.** The bricklayer's trowel is used to take up the mortar and spread it, to
  cut bricks to length and to stop the joints. Moxon lists a longer and narrower trowel for
  tiling, and laying, setting and pointing trowels for the plasterer. Source: [Joseph Moxon,
  Mechanick Exercises (3rd ed., 1703), bricklayers'
  work](https://archive.org/stream/mechanickexercis00moxo_1/mechanickexercis00moxo_1_djvu.txt).
  *Verified from a treatise of 1703.*
- **Lore.** Where the land is alluvium and has clay but only field stone, the mason is a
  bricklayer and the trowel his whole kit. The mud-brick masons of the Saltroad folk plaster
  with the hand and keep the trowel for the finish.
- **Fitting vocations.** Mason.

Also used by the Mason: [Lime kiln](#lime-kiln) (under Charcoal burner), [Plumb rule, level
and square](#plumb-rule-level-and-square) (under Carpenter).

### Potter

#### Potter's wheel

- **Description.** A turning wheel on which pots are thrown. In England a number of
  wheel-throwing potteries appear in the 9th century, and by the middle of the 13th the
  wheel was widely used, most of all by the larger producers. Thrown pots show spiral
  ribbing inside and cutting-off marks on the base. Remains of wheels are rare: a probable
  kick wheel with a stone flywheel is Roman, and a few medieval manuscripts show potters at
  the wheel. Source: [Historic England, "Archaeological and Historic Pottery Production
  Sites"
  (2015)](https://historicengland.org.uk/images-books/publications/archaeological-and-historic-pottery-production-sites/heag019-pottery-production-sites/).
  *Medieval wheel throwing is verified. That the medieval wheel was kicked is not shown.*
- **Lore.** Clay is in every river plain, so every town has a potter, but only kaolin makes
  the fine white ware of the Canal folk that is traded across half the world. Noonfolk
  potters throw in the open yard and stop when the light goes.
- **Fitting vocations.** Potter.

#### Pottery kiln

- **Description.** A kiln, usually oval or round, with a stoking pit at the front, a flue
  that holds the fire and an oven above, where the stacked pots stand on a raised floor. The
  dome was of clay, or of stone lined with clay, with a vent at the top that could be sealed
  to give grey ware. The fuel was mostly wood or peat. In England such kilns returned in the
  9th century and served for pottery and tile through the Middle Ages. Source: [Historic
  England, "Roman and Medieval Pottery and Tile Production" (Introductions to Heritage
  Assets,
  2018)](https://historicengland.org.uk/images-books/publications/iha-roman-medieval-pottery-tile-production/heag228-roman-medieval-pottery-tile-production/).
- **Lore.** A firing is the potter's whole month at stake in one night, and a kiln master is
  paid for the pots that come out whole. Duskreaders judge the heat of a kiln through its
  wall and hire out as kiln readers.
- **Fitting vocations.** Potter.

#### Stamps and roulettes

- **Description.** Purpose-made dies, stamps and roulettes (roller stamps) of antler, bone,
  stone, wood, fired clay or metal, used to press the more complex patterns into the clay.
  Pieces of potters' tools for shaping and decorating are sometimes found at kiln sites.
  Source: [Historic England, "Archaeological and Historic Pottery Production Sites"
  (2015)](https://historicengland.org.uk/images-books/publications/archaeological-and-historic-pottery-production-sites/heag019-pottery-production-sites/).
  *The tool is verified. The page attaches no date to it.*
- **Lore.** A potter's stamp is as good as a signature, and a jar of oil or wine is known
  across a sea by the band rolled round its shoulder. A stamp cut from dragonbone is a boast
  more than a tool.
- **Fitting vocations.** Potter.

### Glassmaker

#### Blowpipe

- **Description.** An iron or steel tube, usually four to five feet long, for blowing glass.
  It has a mouthpiece at one end and a metal ring at the other to hold the gather of molten
  glass. Source: [Corning Museum of Glass, Rakow Research Library, glassmaking
  terms](https://libanswers.cmog.org/faq/143658). *Form and use are verified. The page gives
  no period.*
- **Lore.** Glass is sand, ash and fire, and the Brinefolk soda burners supply the ash, so
  the glasshouses of the world stand nearer their salt flats than anyone planned. The
  Lanternkin vial blowers work the thinnest glass, for vessels that must show a glow.
- **Fitting vocations.** Glassmaker.

#### Pontil

- **Description.** A solid metal rod tipped with hot glass and stuck to the base of a vessel
  to hold it during the making, after it leaves the blowpipe. Taking it off often leaves a
  pontil mark on the base. Source: [Corning Museum of Glass, Rakow Research Library,
  glassmaking terms](https://libanswers.cmog.org/faq/143658). *Form and use are verified.
  The page gives no period.*
- **Lore.** The rough scar under a cup is where the pontil let go, and a buyer runs a thumb
  over it to know the cup was blown and not cast. Glass is city work: no town has the custom
  for it.
- **Fitting vocations.** Glassmaker.

#### Jacks and shears

- **Description.** Jacks are a tool with two metal arms joined by a spring. The glassworker
  sets the spacing of the arms to shape the hot glass, for instance the mouth of an open
  vessel. They are also called pucellas or borsella. Shears trim off excess hot glass during
  the work. Source: [Corning Museum of Glass, Rakow Research Library, glassmaking
  terms](https://libanswers.cmog.org/faq/143658). *Form and use are verified. The page gives
  no period.*
- **Lore.** The master sits at the bench with the jacks and never leaves it: the boys bring
  the glass to him. A glasshouse that works stormglass or mana crystal keeps its jacks apart
  from the others, and does not say why.
- **Fitting vocations.** Glassmaker.

#### Grozing iron

- **Description.** A metal slot or hook into which the edge of a piece of glass is slipped
  and chipped to shape. In medieval stained glass the sheet was first divided by laying a
  heated iron on the surface, then the rough pieces were shaped with the grozing iron and
  fitted into H-shaped strips of lead. From the 16th century diamond cutters spread and left
  cleaner edges. Source: [Victoria and Albert Museum, "Stained glass: an
  introduction"](https://www.vam.ac.uk/articles/stained-glass-an-introduction).
- **Lore.** The glazier travels to the temple or the hall with his glass in straw, and the
  nibbled edge his iron leaves is hidden in the lead. A coloured window is the costliest
  wall a patron can buy.
- **Fitting vocations.** Glassmaker.

### Leatherworker

#### Round knife

- **Description.** The medieval shoemaker's knife, the trenket or tranchet, now called the
  round knife. It appears in nearly all pictures of medieval shoemakers and is the knife of
  the shoemakers' arms. It is attested in word lists from about 1220 to about 1440, with
  finds from London, Stockholm and Scandinavia. Source: [I. Marc Carlson, "The Medieval
  Shoemaker's Tools", Footwear of the Middle Ages
  (archived)](https://sorgm.github.io/carlson1999-archive/shoe/APP1.HTM). *A scholar's site,
  not a museum. The page does not describe the form of the knife.*
- **Lore.** Shoemakers are the most numerous craftsmen of any town, and their knife is on
  more guild seals than the smith's hammer. The saddlers of the Grass riders cut with the
  same blade and call it the half-moon.
- **Fitting vocations.** Leatherworker.

#### Awl

- **Description.** A pointed metal tool, called elsyn or bodkin in medieval word lists. It
  is shown in the housebook of the Mendel foundation in Nuremberg and on an altarpiece of
  1500-1510, and metal awls are recorded from Greenland (about 1350), Poland and London.
  Source: [I. Marc Carlson, "The Medieval Shoemaker's Tools", Footwear of the Middle Ages
  (archived)](https://sorgm.github.io/carlson1999-archive/shoe/APP1.HTM). *Partially
  verified. The page gives the names, the pictures and the finds, but not the use: that it
  pierces the leather for the stitch is general knowledge.*
- **Lore.** Leather is not sewn but pierced and then laced with a waxed thread and a boar's
  bristle, hole by hole. The cobbler who mends at a street corner owns an awl, a knife and a
  last and nothing else.
- **Fitting vocations.** Leatherworker.

#### Shoemaker's last

- **Description.** A wooden model of the foot on which the shoemaker shapes boots and shoes.
  Lasts are recorded from Hedeby, from a shoemaker's workshop at Bergen (late 12th to early
  13th century), from Novgorod, Greenland (about 1350), Stockholm, Lubeck and Warsaw. The
  author holds that by about 1300 lasts were used by some makers, possibly to shape the
  uppers. Source: [I. Marc Carlson, "Lasts, History and use in Medieval Shoes", Footwear of
  the Middle Ages (archived)](https://sorgm.github.io/carlson1999-archive/shoe/APP2.HTM). *A
  scholar's site, not a museum.*
- **Lore.** A shoemaker with many lasts has many customers, and a great house keeps the
  lasts of its family at the maker's shop. A shop that serves Halflings, Gnomes and Orcs
  alike needs a wall of them.
- **Fitting vocations.** Leatherworker.

### Tailor

#### Thimble and needle

- **Description.** A small copper-alloy cap covered in pits, about 2 cm high in a London
  example of the 15th or 16th century. It protects the finger that pushes the needle through
  the cloth, and the head of the needle sits in a pit so that it cannot slip. The museum
  says thimbles came into use in England in the 14th century and were mostly imported, above
  all from Nuremberg. Source: [London Museum, thimble
  A3541](https://www.londonmuseum.org.uk/collections/v/object-36218/thimble).
- **Lore.** Needles are a smith's finest small work and a pedlar's lightest stock, and a
  seamstress guards hers like coin. A tailor sits cross-legged on the board by the window
  for the light, and his thimble has no top so that he can push with the side of the finger.
- **Fitting vocations.** Tailor.

Also used by the Tailor: [Iron shears](#iron-shears) (under Herder).

### Jeweller

#### Touchstone and touch needles

- **Description.** The touchstone is a black, fine-grained stone. The touch needles are
  copper needles tipped with gold or silver alloys of known fineness. The metal under test
  is rubbed on the stone and its streak compared with the streak of a needle. In 1532 a
  London goldsmith owned fifteen gold-tipped needles, silver needles and a touchstone.
  Source: [Victoria and Albert Museum, touch needles
  M.49-1971](https://api.vam.ac.uk/v2/object/O324088), [The Goldsmiths' Company, "Though the
  coin be
  fair"](https://www.thegoldsmiths.co.uk/hallmarking-stories/though-the-coin-be-fair-hallmarking-and-language).
  *The sources name goldsmiths only. That moneychangers use it too is not confirmed by
  them.*
- **Lore.** No power of the Scattered Age reaches further than a few weeks' travel, so there
  are a hundred coinages and the moneychanger's stone decides between them. Gnome assayers
  carry the needles in a roll, and a Lanternkin notary will witness a touch as readily as a
  contract.
- **Fitting vocations.** Jeweller, Merchant.

#### Coin dies

- **Description.** A pair of iron dies engraved in reverse. The lower die, the pile, has a
  spike that fixes it in a wooden block. A blank is laid on it and the upper die, the
  trussel, is struck with a hammer. A pair in the British Museum was used at Durham in the
  1350s. Source: [The British Museum through Google Arts & Culture, "Coin dies and silver
  penny of Edward
  III"](https://artsandculture.google.com/asset/coin-dies-and-silver-penny-of-edward-iii/YwFXV9FNXIJG0Q).
- **Lore.** The dies are the ruler's face and are locked up each night by an official who is
  not the moneyer. A kingdom is the land one court can ride across, and its penny is good
  about as far.
- **Fitting vocations.** Jeweller.

#### Draw plate

- **Description.** A hardened metal plate with a row of tapered holes. Annealed wire is
  filed to a point, put through a hole slightly smaller than itself and pulled through with
  tongs, hole after hole, to make it thinner. The first known reference to the draw plate is
  in the treatise of Theophilus, of the first half of the 12th century. Source: [Fordham
  Medieval Sourcebook, introduction to
  Theophilus](https://sourcebooks.web.fordham.edu/source/theophilus.asp), [Wikipedia, "Draw
  plate"](https://en.wikipedia.org/wiki/Draw_plate). *Partially verified. Form and use come
  from Wikipedia only. Theophilus's own chapter could not be opened.*
- **Lore.** Gold wire for filigree, silver wire for the arm-rings the Northmen cut up to pay
  their debts, and truesilver wire, which is said to sing as it passes the plate. Gnomes
  draw wire finer than hair for their clockwork.
- **Fitting vocations.** Jeweller.

#### Graver

- **Description.** A tool with a long steel shaft, a narrow sharp point and a wooden handle.
  Engravers mostly use gravers (burins) with a lozenge-shaped section to cut designs into
  metal. The cut is a tapered line that swells in the middle. Source: [Victoria and Albert
  Museum, engraving burin](https://api.vam.ac.uk/v2/object/O126830). *Partially verified.
  The object is a printmaker's tool of the 19th century, with no medieval attestation on the
  page.*
- **Lore.** Every seal, every coin die and every inscribed ring passes under a graver. The
  Gnomes engraved the measuring rings of the sorcerer-kings with it, and their map engravers
  still sign in letters too small to read without a lens.
- **Fitting vocations.** Jeweller.

#### Hessian crucible

- **Description.** A ceramic melting vessel made in Hesse in Germany from the Middle Ages
  on, central to the kit of the alchemist and the early metallurgist. It withstood strong
  reagents and high heat: analysis shows that the makers fired a kaolin clay above 1100
  degrees. Examples are found across Europe and in colonial America. Source: [World
  Archaeology 22, "Hessian crucibles reveals alchemists'
  secrets"](https://www.world-archaeology.com/world/europe/germany/hessian-crucibles-reveals-alchemists-secrets/).
- **Lore.** One clay bed in one hill makes crucibles that do not crack, and every alchemist
  and goldsmith of a dozen regions sends there. Sulphur, quicksilver and saltpetre, the
  Reagents of the map, are all tried in it.
- **Fitting vocations.** Jeweller, Alchemist.

Also used by the Jeweller: [Cupel](#cupel) (under Smelter).

### Artist

#### Grinding slab and muller

- **Description.** Cennini tells the painter to take a slab of red porphyry, a strong and
  solid stone, a foot or more across and not highly polished: serpentine and marble are too
  soft. The stone held in the hand is also porphyry, flat underneath and rounded on top. The
  pigment is crushed on the slab and ground with water for half an hour, an hour or longer.
  Source: [Cennino Cennini, The Craftsman's Handbook, trans. D. V. Thompson, ch.
  XXXVI](https://noteaccess.com/Texts/Cennini/2.htm).
- **Lore.** An apprentice painter grinds colour for years before he is given a brush. Lapis
  for blue, malachite and azurite from the copper lands for green and blue, kermes and
  madder for red: the slab is where the Luxuries of half a world end up.
- **Fitting vocations.** Artist.

#### Minever and bristle brushes

- **Description.** Minever brushes are made from squirrel tails: the hairs are tied in
  bunches, pushed into the quill of a vulture or a goose and fitted on a tapered stick of
  maple or chestnut. Bristle brushes use the bristles of a white domestic hog, tied with
  waxed thread to a small stick, and are broken in by whitewashing walls. Source: [Cennino
  Cennini, The Craftsman's Handbook, trans. D. V. Thompson, ch.
  LXIIII-LXV](https://noteaccess.com/Texts/Cennini/2a.htm).
- **Lore.** Squirrel is one of the furs of the boreal forest, and the trapper sells the
  tails to the painter separately. The Boughwalkers paint a second picture over the first in
  the colour only they see, and sell the secret of it to nobody.
- **Fitting vocations.** Artist.

#### Silver style

- **Description.** A slender, smooth style of silver, or of brass or anything else with a
  tip of silver. It draws on a small boxwood panel coated with finely ground burnt bone
  mixed with saliva. The strokes are laid in very lightly and built up by going over them
  for the shadows. Source: [Cennino Cennini, The Craftsman's Handbook, trans. D. V.
  Thompson, ch. V-VIII](https://noteaccess.com/Texts/Cennini/1.htm).
- **Lore.** A line of silver cannot be rubbed out, so a drawing in it is a proof of the
  hand, and masters set it as the test for an apprentice. The illuminators of a temple
  scriptorium lay out a page with it before any colour goes on.
- **Fitting vocations.** Artist.

### Artificer

#### Turns and bow

- **Description.** A small frame of brass and steel gripped in a vice, with a spindle that
  holds a drill or a small part and a pulley for the bow string. The bow is strung with a
  fine gut band wound once round the spindle. Drawing it back and forth turns the part, so
  that it can be shaped or drilled. Source: [Science Museum Group, watchmaker's drill turn
  with whalebone bow
  1910-16](https://collection.sciencemuseumgroup.org.uk/objects/co46413/watchmakers-drill-turn-with-whalebone-bow).
  *Partially verified. Form and use are from an undated object of modern times, with no
  attestation before 1500.*
- **Lore.** The Gnomes built the clockwork that the sorcery of the Old Realms needed, on
  turns no bigger than a hand, and have refused a single master ever since. An artificer is
  found only in a capital, where a court can pay for a year's work on one mechanism.
- **Fitting vocations.** Artificer.

#### Screw plate

- **Description.** A plate with threading holes of graded sizes, used to form a thread on
  brass, iron or steel wire and so make screws. Steel taps cut the matching thread inside
  drilled holes. The Winterthur examples come from a clockmaking shop of the 18th and early
  19th centuries. Source: [Winterthur Museum, Dominy Collections, "Screw Plates and Screw
  Taps"](https://dominycollections.winterthur.org/tools-of-the-trade/clockmaking-shop/screw-plate/).
  *Partially verified. The examples are 18th century, with no attestation before 1500.*
- **Lore.** No two workshops cut the same thread, so a lock or a clock can be mended only by
  the house that made it, or by someone who has stolen its plate. Fine locksmiths count this
  an advantage.
- **Fitting vocations.** Artificer.

---

## Trade and transport

### Merchant

#### Coin balance

- **Description.** A small balance whose two arms fold flat along a fixed central rod, with
  pans that hang from loops. A 14th-century example was excavated at a church in Oxford.
  Coins were worth their weight in silver or gold, so merchants and administrators carried a
  balance to check that worn or clipped coins held their full value. Source: [Oxfordshire
  Museum Service, folding coin
  balance](https://portal.oxfordshire.gov.uk/content/publicnet/museums/oikos/oikos95.htm).
- **Lore.** A merchant who crosses three kingdoms meets three pennies and trusts none of
  them without the balance. The Saltroad folk carry theirs in a wooden case at the belt, and
  weigh gold dust against seeds.
- **Fitting vocations.** Merchant, Shopkeeper.

#### Jettons and counting board

- **Description.** Small coin-like counters of brass, about 22 mm across, laid out in rows
  on a chequered board or cloth as a counting aid like an abacus. They served for nearly all
  arithmetic in the later Middle Ages and into the 17th century. The English Exchequer took
  its name from the chequered cloth on which the sums demanded and received were laid out.
  Source: [Hampshire Cultural Trust, Anglo-Gallic jetton
  WINCM:C2442](https://collections.hampshireculture.org.uk/object/jetton-anglo-gallic),
  [London Museum, Nuremberg
  jetton](https://www.londonmuseum.org.uk/collections/v/object-119527/nuremburg-jetton),
  [GOV.UK History of government blog, "The Exchequer: a chequered
  history"](https://history.blog.gov.uk/2013/08/14/the-exchequer-a-chequered-history/).
- **Lore.** Sums are cast on the cloth and only the result is written down, so a bookkeeper
  who cannot write a line can still keep a great house's accounts. Rainkeeper mathematicians
  reckon with written figures instead and think the board a toy.
- **Fitting vocations.** Merchant, Shopkeeper, Official, Scribe, Scholar.

#### Tally stick

- **Description.** A wooden stick notched to record a sum and then split lengthwise. The
  lender kept the longer part, the stock, and the debtor the foil. When the debt was paid
  the halves were matched to show that they tallied, and the grain of the wood made forgery
  hard. The English Exchequer used tallies from the 12th to the 19th century to account for
  taxes. The Science Museum holds sixteen stocks of about 1440. Source: [Science Museum
  Group, medieval Exchequer tally sticks
  1952-431](https://collection.sciencemuseumgroup.org.uk/objects/co60506/medieval-exchequer-tally-sticks-exchequer-tallies).
  *The page covers the use by the Exchequer. An innkeeper's credit on a tally is not on it.*
- **Lore.** A tally needs no writing and no witness, only two halves that fit, so it is the
  contract of those who cannot read. The alehouse keeper cuts one per customer and hangs
  them over the barrel, and the tax collector's bundle is the most hated firewood in the
  region.
- **Fitting vocations.** Merchant, Innkeeper, Official.

#### Steelyard

- **Description.** A balance on which a weight hangs from the long arm. The part that
  survives is mostly the weight: a London example of the mid to late 13th century is a globe
  of copper alloy with a lead core, 674 g, bearing the arms of an earl. The museum says
  steelyards were often used by merchants to weigh goods. Source: [London Museum, steelyard
  weight
  2008.134](https://www.londonmuseum.org.uk/collections/v/object-773802/weight-steelyard-weight/).
  *The page does not explain how the weight is moved along the arm.*
- **Lore.** A steelyard weighs a sack of wool with one small weight, which is why
  stallholders like it and customers do not. The weigh-master of a market keeps a sealed one
  against which any trader's can be tried.
- **Fitting vocations.** Merchant, Shopkeeper.

Also used by the Merchant: [Touchstone and touch needles](#touchstone-and-touch-needles)
(under Jeweller).

### Shopkeeper

#### Standard yard

- **Description.** An octagonal bronze bar, the standard yard made in London in 1495-1497 as
  the royal standard of length. Copies were issued to boroughs round the country to enforce
  true measure in trade. Source: [Science Museum Group, Exchequer standard yard of King
  Henry
  VII](https://collection.sciencemuseumgroup.org.uk/objects/co57527/exchequer-standard-yard-of-king-henry-vii-1495-1497).
- **Lore.** Each free town has its own yard, let into the wall of the market hall for anyone
  to lay cloth against. The draper's wand is checked against it once a year by the sworn
  measurer.
- **Fitting vocations.** Shopkeeper, Official.

#### Standard bushel

- **Description.** A bronze bushel measure, 71 cm across, made at a king's command between
  1495 and 1509 as the primary standard of capacity. It is one of a series issued to some 60
  cities to encourage and enforce fair trade. Source: [Science Museum Group, Exchequer
  standard bushel measure of King Henry
  VII](https://collection.sciencemuseumgroup.org.uk/objects/co57551/exchequer-standard-bushel-measure-of-king-henry-vii-1495-1497).
  *The dates on the record disagree: 1495-1497 in the title and 1497-1509 in the text.*
- **Lore.** Whether grain is sold heaped or struck level with the rim has started riots. A
  Catfolk granary warden measures out with the town's bushel and is believed, since everyone
  knows the warden cannot eat what is in it.
- **Fitting vocations.** Shopkeeper, Official.

Also used by the Shopkeeper: [Coin balance](#coin-balance) (under Merchant), [Jettons and
counting board](#jettons-and-counting-board) (under Merchant), [Steelyard](#steelyard)
(under Merchant).

### Carter

#### Horse collar

- **Description.** A heavily padded collar of leather round the horse's neck. Iron or wooden
  hames surround the padding and carry the eyes for the reins and the traces. It rests on
  the shoulders, so that the horse is not choked as it was by the older harness across the
  windpipe. It came into general use in Europe during the 12th century. Source:
  [Encyclopaedia Britannica, "Horse
  collar"](https://www.britannica.com/technology/horse-collar). *A reference work, not a
  museum.*
- **Lore.** The collar is the harness maker's masterpiece, stuffed with rye straw and fitted
  to one horse. With it a horse does the work of the ox team in half the time, where there
  is oats to feed it.
- **Fitting vocations.** Carter.

#### Two-wheeled cart

- **Description.** A two-wheeled vehicle, usually drawn by a single animal, with shafts that
  often continue the frame of the body, which may be a few crossed stakes or a sturdy box.
  The Luttrell Psalter of 1325-1340 shows a farm cart and a harvest cart going uphill.
  Source: [Encyclopaedia Britannica, "Cart"](https://www.britannica.com/technology/cart),
  [British Library, catalogue of Add MS 42130 (Luttrell
  Psalter)](https://searcharchives.bl.uk/catalog/032-002085986). *The catalogue lists the
  cart scenes but does not describe the carts.*
- **Lore.** The roads of the travel network are cart roads only where the land is flat: in
  the hills the load goes onto backs. Grass riders move their whole household on carts, felt
  tent and all.
- **Fitting vocations.** Carter.

#### Pack saddle

- **Description.** A wooden frame of shaped boards that rest on either side of the animal's
  back, joined by two curved stretchers with holes for tying on the load and a gap over the
  spine. Goods were carried in baskets (panniers) on either side. The Museum Wales example
  is pine, of the early 19th century. Source: [Amgueddfa Cymru - Museum Wales, pack
  saddle](https://museum.wales/collections/online/object/04a8a947-3a0d-3185-bc2b-9fdaeb4f675c),
  [Herefordshire Through Time, "Pack horse
  transport"](https://htt.herefordshire.gov.uk/herefordshires-past/the-post-medieval-period/transport/guest-author-essay-pack-horse-transport).
  *Partially verified. The object is 19th century, with no medieval attestation on either
  page.*
- **Lore.** Mules in the mountains, camels on the salt road, yaks on the high passes and
  reindeer in the north carry the same frame under different names. A caravan master of the
  Saltroad folk loads salt one way and gold the other and balances the two sides by hand.
- **Fitting vocations.** Carter.

### Sailor

#### Sounding lead and line

- **Description.** A lead weight on a rope, lowered over the side until it touches bottom to
  measure the depth of the water. The base is hollowed and packed with tallow, which brings
  up a sample of the seabed that helps to fix the ship's position. Royal Museums Greenwich
  calls it probably one of the oldest instruments of navigation. Source: [Royal Museums
  Greenwich, sounding lead and line
  NAV0676](https://rmg.co.uk/collections/objects/rmgc-object-42888). *Use is verified. The
  object shown is from about 1950.*
- **Lore.** A pilot knows a coast by what sticks to the tallow: grey sand here, shell there,
  black mud off the river mouth. Shoalfolk pilots do without the lead in their own shallows
  and are paid well by those who cannot.
- **Fitting vocations.** Sailor.

#### Mariner's compass

- **Description.** A magnetised iron needle, stroked with a lodestone, balanced on a spike
  under a card with north marked, in a wooden box. The box hangs in two brass gimbal rings
  that keep it level as the ship pitches and rolls. Three compasses were found on the Mary
  Rose, one in a chest thought to be from the pilot's cabin. Source: [Mary Rose Trust,
  gimballed compass 81A0071](https://maryrose.org/artefacts/gimballed-compass), [Mary Rose
  Trust, "The Pilot"](https://maryrose.org/meet-the-crew/the-pilot/). *The finds are Tudor
  (the ship sank in 1545). The earlier history of the compass was not sourced.*
- **Lore.** The needle is the ship's master's own and goes ashore with him. Sailors of every
  race agree that it wanders where the Supernatural is high, and a careful master then
  trusts the lead and the lookout: a Widegaze one if he can hire it.
- **Fitting vocations.** Sailor.

#### Mariner's astrolabe

- **Description.** A heavy wheel of cast brass, with parts of the disc cut away against the
  wind and a weighted lower half so that it hangs upright, fitted with a turning sight arm.
  It gives the latitude from the height of the Pole Star or the Sun. It was adapted from the
  astronomer's astrolabe and came into use at sea by about 1470. Source: [Royal Museums
  Greenwich, Valentia astrolabe
  NAV0022](https://www.rmg.co.uk/collections/objects/rmgc-object-42234). *The object shown
  is from about 1588.*
- **Lore.** Only the masters of deep-sea ships carry one. The coasting trade of the Northmen
  and the canal boats of the Canal folk never lose sight of land long enough to need it.
- **Fitting vocations.** Sailor.

### Innkeeper

#### Black jack

- **Description.** Tankard-shaped drinking vessels of leather were called black jacks, and
  the larger leather jug a bombard. The museum describes them as robust, waterproof
  containers for water and drink, a popular choice of London's alehouse, tavern and inn
  keepers. The example is leather and pewter, 29 cm high, of the 16th or 17th century.
  Source: [London Museum, black jack
  4566](https://www.londonmuseum.org.uk/collections/v/object-440161/black-jack/). *The
  object is 16th or 17th century.*
- **Lore.** Leather does not break when it is thrown, which is the whole of its merit in a
  tavern. A Halfling inn serves in turned wood and counts the cups back in.
- **Fitting vocations.** Innkeeper.

#### Ale-stake

- **Description.** A pole projecting from a tavern over the street as its sign. A London
  ordinance of 1375 limited ale-stakes to seven feet over the highway, since longer ones
  hindered riders and people on foot, with a fine of 40 pence for each offence. Source:
  [British History Online, H. T. Riley, Memorials of London and London Life, "Memorials:
  1375"](https://www.british-history.ac.uk/no-series/memorials-london-life/pp381-392).
- **Lore.** When a new brew is ready the alewife puts the stake out, and the official's
  ale-taster comes before the customers do. On the roads between Prospects a stake at a farm
  gate means a bed as well.
- **Fitting vocations.** Innkeeper.

Also used by the Innkeeper: [Tally stick](#tally-stick) (under Merchant).

---

## Service

### Healer

#### Fleam and bleeding bowl

- **Description.** A fleam is a small steel blade for bloodletting. A set in the Science
  Museum, probably German and dated 1544, has two fleams with triangular blades and one with
  a round blade in a carved bone case. The blood was caught in a handled bleeding bowl.
  Bloodletting was used against many fevers and diseases thought to come from a build-up of
  blood. Source: [Science Museum Group, fleams with
  case](https://collection.sciencemuseumgroup.org.uk/objects/co142081/fleams-with-case),
  [Science Museum Group, bleeding
  bowl](https://collection.sciencemuseumgroup.org.uk/objects/co136049/bleeding-bowl-england-1671-1730).
  *Both objects are later than 1500 (1544, and about 1700).*
- **Lore.** The barber-surgeon bleeds in spring and autumn as a matter of course. The
  Flinchless, who examine each other at dawn and dusk and became the world's surgeons, bleed
  less than anyone, and an Amberblood's cut closes with a golden bead before the bowl is in
  place.
- **Fitting vocations.** Healer.

#### Uroscopy flask

- **Description.** A glass flask in which a patient's urine is held up and inspected.
  Physicians judged its colour, consistency, clarity and sediment as signs of imbalance in
  the four humours, with the help of charts (urine wheels) that show rows of flasks. Urine
  was examined in ancient Greece, medieval Europe and the Islamic world, and a urine wheel
  was printed in Venice in 1491. Source: [Royal College of Physicians, "The urine
  wheel"](https://history.rcp.ac.uk/blog/urine-wheel), [Science Museum Group, glass urinal
  or
  matula](https://collection.sciencemuseumgroup.org.uk/objects/co147557/glass-urinal-urinal).
  *The object in the Science Museum is 19th century.*
- **Lore.** The flask is the sign of the learned physician as the peel is the baker's. The
  Stillfaces skip it: their diagnosticians read a sickness from the scent of the skin and
  are consulted, quietly, by physicians of every race.
- **Fitting vocations.** Healer.

#### Mortar and pestle

- **Description.** A heavy bowl of cast copper alloy with a pestle for grinding. The V&A
  calls mortars and pestles standard tools of the trade for physicians and apothecaries, and
  everyday utensils in the household for grinding spices, sugar and grains. They were
  associated with alchemy too. The example is Netherlandish, about 1540. Source: [Victoria
  and Albert Museum, mortar](https://collections.vam.ac.uk/item/O129007/mortar-unknown/).
  *The object is about 1540.*
- **Lore.** Herbs and Reagents are both resources of the map, and both pass through a mortar
  before they do anyone good or harm. A Bitterblood apothecary keeps two, and customers are
  shown which one their cure is ground in.
- **Fitting vocations.** Healer, Alchemist.

#### Folding almanac

- **Description.** A manuscript on vellum of the late 15th century, of seven leaves folded
  to about 14 by 5 cm, in a limp cover with a tag for hanging it by a cord from the girdle.
  It holds a calendar, tables of the planets and the moon, tables of eclipses for 1461-1481,
  a vein-man and a zodiac man. Source: [Wellcome Collection, folding almanac
  MS.40](https://wellcomecollection.org/works/bvgyb226).
- **Lore.** The astrologer and the physician consult the same folded leaves: one for a
  wedding day, the other for which vein to open under which sign. A weather watcher of the
  Widegaze adds a column the almanac lacks, of what the horizon looked like at dawn.
- **Fitting vocations.** Healer, Seer.

### Servant

#### Cauldron

- **Description.** A cooking vessel of copper alloy. A London example of the 14th or 15th
  century is 22 cm high and weighs 4 kg. Most cooking was done over an open fire, and many
  vessels had three feet to raise them above the hearth. Metal vessels like this were more
  common in the wealthier households. Source: [London Museum,
  cauldron](https://www.londonmuseum.org.uk/collections/v/object-35519/cauldron).
- **Lore.** The pot is never emptied in a great kitchen: what is left of one day's pottage
  starts the next. Among the Orcs the cauldron belongs to the hearth-mother, and who eats
  from it is who belongs.
- **Fitting vocations.** Servant.

#### Roasting spit

- **Description.** An iron spit about 1.26 m long, with a point, a long blade and a crank,
  from London, late 15th or early 16th century. Joints of meat or whole animals were
  skewered on it. It rested on a rack in front of the fire and was turned slowly, so that
  the meat cooked without burning. Source: [London Museum, spit
  84.314/10](https://www.londonmuseum.org.uk/collections/v/object-109846/spit).
- **Lore.** Turning the spit is the first work a kitchen boy is given and the one he is
  gladdest to leave. Roast meat is the food of a hall: a farmhouse boils.
- **Fitting vocations.** Servant.

#### Boxwood comb

- **Description.** A comb carved from boxwood, with thick teeth on one side and fine teeth
  on the other. It dressed the hair, and the fine side removed lice and nits. Boxwood combs
  were fashionable with men and women from about 1400 into the 17th century, were often love
  gifts carrying an inscription, and were kept in decorated leather cases. Source: [Victoria
  and Albert Museum, comb 88-1892](https://collections.vam.ac.uk/item/O125617/comb-unknown).
  *This object is catalogued as French, 16th century.*
- **Lore.** Northmen comb their hair daily and are thought vain for it by their neighbours.
  A lady's maid, a bathhouse attendant and a valet all work with this comb, and a carved one
  is the usual first gift of a suitor.
- **Fitting vocations.** Servant, Courtesan.

### Entertainer

#### Harp

- **Description.** A plucked, framed string instrument, strung with gut, or in Ireland with
  metal strings from an early date. By the 14th century it sometimes had as many as
  twenty-five strings. Around 1400 it took a slimmer form with a small sound-box of hardwood
  and bray pins that made the strings buzz. Source: [Case Western Reserve University, Early
  Music Instrument Database, "Harp
  (Medieval)"](http://caslabs.case.edu/medren/medieval-instruments/harp-medieval/).
- **Lore.** A good poet eats at the high table of a Northman hall and the harp is why. Elven
  harpers play at human courts for a season that the court remembers for three generations
  and the harper forgets.
- **Fitting vocations.** Entertainer.

#### Pipe and tabor

- **Description.** A recorder-like pipe with only three finger holes, played with one hand
  so that the other can beat a drum: something like a one-man dance band. It is often shown
  in pictures of music-making out of doors. Tabor pipes and drums were recovered from the
  Mary Rose. Source: [Case Western Reserve University, Early Music Instrument Database,
  "Tabor pipe"](https://caslabs.case.edu/medren/renaissance-instruments/tabor-pipe/), [Mary
  Rose Trust, "Christmas on the Mary
  Rose"](https://maryrose.org/christmas-on-the-mary-rose/).
- **Lore.** One player is a whole dance, which suits a fair, a wedding or a company of
  Swiftborn players who cross a region in a season. The drummers and praise-singers of the
  Sunlanders would call the tabor a child's drum.
- **Fitting vocations.** Entertainer.

#### Hurdy-gurdy

- **Description.** A string instrument sounded by a rosined wooden wheel turned by a crank.
  The wheel keeps several strings droning, and one is a melody string shortened by keys. It
  first appears in the 10th century as a large instrument for two players, the organistrum,
  and was made smaller in the 13th, as the symphonia. Source: [Case Western Reserve
  University, Early Music Instrument Database, "Hurdy-gurdy
  (Medieval)"](http://caslabs.case.edu/medren/medieval-instruments/hurdy-gurdy-medieval/).
  *The page describes the instrument of minstrels. That beggars play it is the judgement of
  this document.*
- **Lore.** It began in the temples and ended at their doors: the instrument a blind singer
  can play all day for alms, since the wheel never needs to draw breath.
- **Fitting vocations.** Entertainer, Beggar.

### Courtesan

#### Mirror in an ivory case

- **Description.** A small hand mirror held in a case of two carved discs of elephant ivory.
  The V&A example, from Paris or possibly Cologne, early 14th century, is 11.5 cm across and
  carved with an assembly of lovers. Mirror cases, ivory combs and hair-parters made up the
  dressing case of the wealthy and were costly gifts between friends, family and lovers.
  Source: [Victoria and Albert Museum, "An Assembly of Lovers", mirror back
  220-1867](https://collections.vam.ac.uk/item/O106330/an-assembly-of-lovers-mirror-back-unknown).
  *The page describes the dressing case of the wealthy in general. That a courtesan owns one
  is the judgement of this document.*
- **Lore.** A companion's standing is read from the gifts she is seen to use, and the mirror
  case comes out where others can see who carved it. The ivory is Sunlander elephant or
  northern walrus, and the mirror makers of the cities silver the glass.
- **Fitting vocations.** Courtesan.

#### Pomander

- **Description.** A small hollow pierced ball of silver-gilt, in two halves that unscrew so
  that it can be filled with aromatic substances to scent the air. The V&A example is
  German, about 1500, 3.7 cm high. Pomanders were widely used as pendants on prayer beads,
  and a comparable piece was worn hanging from a girdle or on a chain round the neck.
  Source: [Victoria and Albert Museum, pomander 918:1,
  2-1853](https://collections.vam.ac.uk/item/O105969/pomander-unknown/). *The page describes
  general wear. The link to the courtesan is the judgement of this document.*
- **Lore.** Ambergris, musk, frankincense and myrrh are Luxuries of the map and travel far
  to hang at a belt. The perfumers of the Stillfaces compose scents that say a whole
  sentence to their own people and merely smell pleasant to everyone else.
- **Fitting vocations.** Courtesan.

Also used by the Courtesan: [Boxwood comb](#boxwood-comb) (under Servant).

---

## Governance and war

### Noble

#### Seal matrix

- **Description.** An engraved die, usually of lead or copper alloy and now and then of
  silver, for making an impression in wax to authenticate a document or keep it closed. Flat
  matrices are round or pointed-oval with a pierced lug, and conical ones have a pierced end
  for hanging. The inscription is cut in mirror image. Matrices with a personal name are
  thought to date to the 13th century, and official seals carry the name of the office.
  Source: [Portable Antiquities Scheme, "Seal matrices" recording
  guide](https://finds.org.uk/counties/findsrecordingguides/seal-matrices/).
- **Lore.** A lord who cannot write still seals, and a matrix is broken at its owner's death
  so that no one can speak for him afterwards. The Lanternkin became the notaries of the
  world because they seal with a drop of their own blood, which glows for as long as they
  live.
- **Fitting vocations.** Noble, Official.

#### Knightly sword

- **Description.** A double-edged iron sword, 108 cm long with a cross-guard of 20 cm,
  weighing 2 kg, of the early to mid 14th century, found in the Thames. The museum states
  that large double-edged war swords like this were the chief weapons of armoured knights,
  and that an ordinary Londoner found carrying one could be arrested. Source: [London
  Museum, double-edged sword
  52.12(1)](https://londonmuseum.org.uk/collections/v/object-37133/double-edged-sword).
- **Lore.** A sword is the one tool on this list that is also a rank: among the Midlanders
  it is girded on at a knighting and carried before a lord at his burial. Rainkeeper steel
  makers sell the best blades and Dwarven ones the best kept.
- **Fitting vocations.** Noble, Soldier.

#### Spurs

- **Description.** Spurs of iron or copper alloy strapped to the rider's heel. Prick spurs
  carry a single point and declined in England at the end of the 13th century. Rowel spurs,
  with a turning toothed wheel, came in during the early 13th century and were the only form
  from the early 14th. Source: [Portable Antiquities Scheme, "Spurs" recording
  guide](https://finds.org.uk/counties/findsrecordingguides/spurs/). *The guide does not
  discuss the rank of those who wore them.*
- **Lore.** Gilt spurs mark a knight and are hacked off the heels of one who is disgraced.
  The Grass riders, who live in the saddle, wear none and consider them a confession.
- **Fitting vocations.** Noble.

### Official

#### Mace of office

- **Description.** A staff of authority descended from the weapon carried by royal
  bodyguards from the 13th century. From the late 14th century a wider range of royal and
  civic officials carried maces. A mace of about 1475 at Stratford is silver over an iron
  shaft with the royal arms: it was carried by or before the high bailiff, and the serjeant
  used it to bang on doors and summon defendants to court. Source: [Shakespeare Birthplace
  Trust, "Stratford's Ceremonial
  Maces"](https://www.shakespeare.org.uk/explore-shakespeare/blogs/stratfords-ceremonial-maces).
- **Lore.** The bailiff who comes without his mace comes as a private man and can be
  refused. A magistrate of the Canal folk carries a seal instead, and a Northman law-speaker
  carries nothing but a third of the law in his head.
- **Fitting vocations.** Official.

Also used by the Official: [Jettons and counting board](#jettons-and-counting-board) (under
Merchant), [Tally stick](#tally-stick) (under Merchant), [Standard yard](#standard-yard)
(under Shopkeeper), [Standard bushel](#standard-bushel) (under Shopkeeper), [Seal
matrix](#seal-matrix) (under Noble).

### Soldier

#### Bill and pike

- **Description.** The bill derives from a farm pruning tool: an iron head of over 2 kg with
  a curved blade and a spike at the top, on a stout ash staff. The pike is like a spear,
  with a long staff and a small blade, used on land by squares of foot soldiers to hold a
  barrier of points against cavalry. The staffs are mostly ash, about 2 to 5 m long. Source:
  [Mary Rose Trust, "Swords, daggers and staff
  weapons"](https://maryrose.org/the-weaponry-of-the-mary-rose/swords-daggers-and-staff-weapons/).
- **Lore.** The bill is the billhook of the hedge on a longer pole, which is how a levy of
  farmers arms itself. Orc soldiers for hire stand in pike squares that the cavalry of the
  Grass riders will not charge twice.
- **Fitting vocations.** Soldier, Guard.

#### Mail shirt

- **Description.** A shirt of about 28,000 small rings of iron and steel, most closed with a
  rivet, weighing about 14 kg, of the late 14th or early 15th century. Mail was the main
  metal body armour until the 14th century and stayed in use until the 16th. It took much
  labour and was very expensive, so most soldiers wore a padded coat instead. Source:
  [London Museum, hauberk
  35.52](https://www.londonmuseum.org.uk/collections/v/object-36460/hauberk-mail-shirt).
- **Lore.** A mail shirt is a year of an armourer's work and is inherited, mended and re-cut
  for three generations. The heavy foot of the Dwarves are hired with their mail, and the
  hire is higher for it.
- **Fitting vocations.** Soldier.

#### Halberd

- **Description.** An axe mounted on a long pole: a steel head with an axe blade for
  hacking, a spike at the top for thrusting and a beak at the back for piercing armour or
  unseating a rider, about 2 m overall. It was an infantry weapon favoured by Swiss and
  German mercenaries, and by the late 1500s had become a ceremonial weapon of princely
  bodyguards. Source: [Cleveland Museum of Art, halberd
  1916.1542](https://www.clevelandart.org/art/1916.1542). *The object is German, about
  1550.*
- **Lore.** The gate guard leans on it all day and uses it once a year, and the bodyguard's
  is gilded. Northmen sworn guards abroad carry a long axe in its place and are hired by
  kings who trust no one born nearby.
- **Fitting vocations.** Soldier, Guard.

#### Caltrop

- **Description.** Four iron spikes joined so that, however it lands, three form a tripod
  and the fourth points up. Caltrops were thrown on the ground to slow an advance by
  wounding the feet of men and horses. The specimen described is Roman. The Science Museum
  Group holds one of brass and iron, probably English, of the 13th to 17th century. Source:
  [Amgueddfa Cymru - Museum Wales, Roman iron caltrop
  32.60/4.3](https://museum.wales/collections/online/object/6ee6fabb-dbbe-360c-badf-de51f30ce157),
  [Science Museum Group, caltrop
  A201111](https://collection.sciencemuseumgroup.org.uk/objects/co143784/caltrop-probably-english-13th-17th-century-caltrops).
  *Partially verified for the period: form and use come from the Roman specimen. Use by
  robbers is the judgement of this document.*
- **Lore.** A handful scattered in a ford stops a merchant's wagon or a pursuing sheriff
  equally well. The Flinchless walk across them without a sound, which is a reason of its
  own to check their feet at dusk.
- **Fitting vocations.** Soldier, Bandit.

Also used by the Soldier: [Knightly sword](#knightly-sword) (under Noble),
[Longbow](#longbow) (under Hunter).

### Guard

#### Watchman's horn

- **Description.** An animal horn blown to set the watch. At Ripon the Wakeman answered for
  the prevention of crime from nine in the evening until dawn, and a horn is still blown
  each night at the four corners of the market cross. The city's Charter Horn is many
  centuries old and hangs on a belt with silver badges. Source: [Ripon City Council, "The
  Hornblower"](https://www.riponcity.gov.uk/history/the-hornblower). *Partially verified. A
  municipal history page, whose early dates are tradition.*
- **Lore.** One blast at the closing of the gates, and after that the watch answers for what
  is stolen. Duskreaders and Catfolk fill the night watch of half the towns of the world,
  and the beacon keepers of the Widegaze pass the alarm faster than a horse.
- **Fitting vocations.** Guard.

#### Horn lantern

- **Description.** A cylindrical lantern of tin plate and iron with windows of horn, 26 cm
  high. Source: [Science Museum Group, lantern
  A45674](https://collection.sciencemuseumgroup.org.uk/objects/co155085/lantern-tin-plate-with-horn-reflectors).
  *Partially verified. The record gives the materials and the size only and is undated. That
  watchmen carried it is not supported by the page.*
- **Lore.** Horn is a by-product of the butcher, scraped thin enough to let a candle through
  and tough enough to drop. A caravan or a ship that can afford it hires a Lanternkin
  instead, whose light needs no flame.
- **Fitting vocations.** Guard.

#### Fire hook

- **Description.** A long wooden pole with an iron hook: the London Museum example is 4.33 m
  long and weighs 34.5 kg. Hooks were used to pull down buildings ahead of a fire, or
  burning houses, to stop the fire spreading. Each parish kept its own firefighting
  equipment, usually in the church. Source: [London Museum, fire hook
  B737](https://www.londonmuseum.org.uk/collections/v/object-119065/fire-hook/). *Partially
  verified for the period: the object and its context are 17th century.*
- **Lore.** A town of timber and thatch fights fire by tearing down the next house, and the
  hook hangs under the eaves of the market hall for it. The Flinchless serve as firefighters
  and are checked over by their fellows afterwards, since they do not know when they are
  burned.
- **Fitting vocations.** Guard.

Also used by the Guard: [Bill and pike](#bill-and-pike) (under Soldier), [Halberd](#halberd)
(under Soldier).

---

## Faith and learning

### Priest

#### Censer and incense boat

- **Description.** A metal vessel with a base that holds glowing charcoal and a pierced lid,
  hung on chains so that it can be swung. Incense is spooned onto the coals from a separate
  incense boat. An English censer and boat of the 14th century, of silver and silver-gilt,
  were found in 1850 when a mere was drained. Source: [Victoria and Albert Museum, "Ramsey
  Abbey censer and incense
  boat"](https://www.vam.ac.uk/blog/museum-life/twenty-objects-twenty-years-ramsey-abbey-censer-and-incense-boat).
- **Lore.** Frankincense and myrrh come up the salt road from the dry scrub to temples that
  have never seen the tree. A temple priest's rank shows in the metal of the censer, and the
  village priest burns pine resin in a clay one.
- **Fitting vocations.** Priest.

#### Chalice

- **Description.** A stemmed cup with a bowl, a stem and a round foot, holding the wine that
  the priest consecrates in the rite. Because the wine was held to become holy, chalices
  were usually of precious metal. The V&A example is silver-gilt, Spanish, about 1550-75.
  Source: [Victoria and Albert Museum,
  chalice](https://collections.vam.ac.uk/item/O108995/chalice-unknown). *The example is 16th
  century.*
- **Lore.** The cup is the first thing a temple buys and the first thing raiders take. The
  priests of the Pair among the Twofold use a cup with two bowls on one stem.
- **Fitting vocations.** Priest.

#### Shaman's drum

- **Description.** An oval frame drum with a skin of reindeer hide bearing figures, struck
  with a T-shaped beater of reindeer antler. By the museum record, such drums were made as
  an aid to reach a trance and speak with gods and spirits. A second use was to foretell: a
  pointer was laid on the skin and the future read from the figure it came to rest on.
  Source: [Museum of Archaeology and Anthropology, Cambridge, drum
  2008.118](https://collections.maa.cam.ac.uk/objects/553931/). *The specimen is a drum made
  in 2007 in the traditional form, not a historical one.*
- **Lore.** The shamans of the Grass riders and of the reindeer herders of the tundra drum
  where a priest of the settled lands would pray. The figures on the skin are the shaman's
  own map of the three worlds, and no two drums agree.
- **Fitting vocations.** Priest.

### Scribe

#### Quill pen and penknife

- **Description.** A pen cut from a wing feather, most often of a goose. The barrel is
  hardened in hot sand, the tip sliced at an angle and slit to hold and feed the ink, then
  trimmed to a nib whose shape affects the script. It was the main writing tool from about
  the 7th century to the 19th. The scribe re-cuts it with a penknife. Source: [UCLA, History
  of the Book, "Scribes: Quill
  Pens"](https://pages.gseis.ucla.edu/faculty/drucker/HoB_Site/HoB_Scribes_Exhibit/HoB_Scribes_Quill_Pens.html).
- **Lore.** The Saltroad folk kept the books when everyone else burned theirs, and a clerk
  of theirs cuts a pen in four strokes without looking. The Canal folk write with a brush
  and print what must be read by many.
- **Fitting vocations.** Scribe.

#### Wax tablet and stylus

- **Description.** A flat board, usually of wood, hollowed and filled with wax. A pointed
  stylus scratches the letters into the wax and its other end smooths it for reuse. A London
  stylus of the 13th to 15th century is bone with a metal tip, 7 cm long, with a round head
  used as the eraser. Tablets served for school exercises, letters and records. Source:
  [British Library, "Keep taking the (wax)
  tablets"](https://www.bl.uk/stories/blogs/posts/keep-taking-the-wax-tablets), [London
  Museum, stylus A1319](https://www.londonmuseum.org.uk/collections/v/object-36078/stylus).
- **Lore.** Everything is drafted in wax and only what matters is copied to parchment: a
  schoolmaster's lesson, a steward's count of sheep, a poet's first try. The wax is the
  beekeeper's second crop.
- **Fitting vocations.** Scribe, Scholar.

#### Pricker

- **Description.** An awl or other sharp point used to lay out a page before writing. After
  ruling the first leaf of a gathering, the scribe pricks holes at the edges where the lines
  end. The holes guide the ruling of the other leaves from hole to hole, so that margins and
  lines are the same on every page. The prickings were usually trimmed off in binding.
  Source: [UCLA, History of the Book, "Scribes:
  Prickers"](https://pages.gseis.ucla.edu/faculty/drucker/HoB_Site/HoB_Scribes_Exhibit/HoB_Scribes_Prickers.html).
- **Lore.** A copyist is paid by the gathering, and the pricking is the part of the work no
  reader ever sees. Dwarven records clerks leave the prick holes in, as proof that no leaf
  was added later.
- **Fitting vocations.** Scribe.

Also used by the Scribe: [Jettons and counting board](#jettons-and-counting-board) (under
Merchant).

### Scholar

#### Astrolabe

- **Description.** A portable instrument for calculating with the positions of the Sun and
  the stars. The body (the mater) carries scales of degrees and hours on its rim and holds
  plates, each a projection of the sky for one latitude. Over them turns the rete, a cut-out
  star map with pointers for the stars. On the back a sighting bar is aimed at a star or the
  Sun to read its height. Source: [Whipple Museum of the History of Science, Cambridge, "The
  Parts of an
  Astrolabe"](https://www.whipplemuseum.cam.ac.uk/explore-whipple-collections/astronomy/medieval-astrolabe/parts-astrolabe).
  *The page describes the astronomer's use. That an astrologer casts a horoscope with it is
  the judgement of this document.*
- **Lore.** Astronomers of the Saltroad folk and the Rainkeepers own the best, and Gnome
  instrument makers cut them. The Noonfolk, who fall asleep at sunset and have never seen
  the stars, buy astrolabes to learn what the sky does when they are not there.
- **Fitting vocations.** Scholar, Seer.

#### Armillary sphere

- **Description.** A model of the universe built of rings that stand for the circles of the
  sky, such as the equator and the tropics, commonly of brass. Large ones were used for
  observing. Smaller ones, known in western Europe from the later Middle Ages, taught
  students the geometry of the celestial sphere. A late medieval example in the Whipple
  Museum was probably held by a handle at the south pole. Source: [Whipple Museum of the
  History of Science, Cambridge, "Armillary Spheres and Teaching
  Astronomy"](https://www.whipplemuseum.cam.ac.uk/node/59).
- **Lore.** A teacher holds it up by the handle and turns the sky for a room of students.
  The Northmen, who hold that the sky is the planking of an upturned hull, find the rings a
  poor likeness of a ship.
- **Fitting vocations.** Scholar.

Also used by the Scholar: [Jettons and counting board](#jettons-and-counting-board) (under
Merchant), [Wax tablet and stylus](#wax-tablet-and-stylus) (under Scribe).

---

## Supernatural

### Mage

#### Seals of wax

- **Description.** Discs of wax engraved with magical names, symbols and signs, of
  1550-1599, in the British Museum. The two smaller ones are what survive of four that stood
  under the legs of John Dee's "table of practice". The larger, the Seal of God, matches a
  drawing in Dee's manuscripts and supported one of his "shew-stones". Dee used them in his
  occult work with his medium Edward Kelly. Source: [The British Museum through Google Arts
  & Culture, "Dr Dee's
  magic"](https://artsandculture.google.com/asset/dr-dee-s-magic/jgH1be-D72po1Q).
- **Lore.** A mage is found only in a capital and only where the Supernatural is high, and
  the table is set out before anything else is done. Whether the seals protect the mage or
  only steady the hand is disputed, mostly by those who have never worked without them.
- **Fitting vocations.** Mage.

#### Book of conjurations

- **Description.** A working manual of ritual magic. One in Chetham's Library, of the second
  half of the 16th century and in Latin with English additions, holds conjurations,
  astrological tables and signs of the planets. It prescribes consecrated tools (a copper
  plate inscribed with a pentangle, a ring, a sceptre and a sword) and a circle of two chalk
  rings, at most fifteen feet across, inscribed with holy names. Its wear suggests that it
  was used. Source: [Chetham's Library, "Conjuring spirits in the 'Tractatus de
  Nigromantia'"](https://library.chethams.com/?p=575).
- **Lore.** The sorcerer-kings of the Old Realms reshaped rivers, forests and peoples, and
  what a court mage of the Scattered Age owns is a copy of a copy of a fragment of how. A
  mage's book is in his own hand, since nobody sells theirs.
- **Fitting vocations.** Mage.

#### Rune stick

- **Description.** A small stick, about 8.5 cm long, from Odense in Denmark, of the early
  13th century. Its runic inscription is in Latin and includes the words for good health and
  a man's name. The researcher of the National Museum of Denmark who read it suggests that
  it was an amulet to protect its owner from illness and evil. Source: [Medievalists.net,
  "13th century rune stick discovered in
  Denmark"](https://www.medievalists.net/2015/05/13th-century-rune-stick-discovered-in-denmark/).
  *Partially verified: a news article that reports the museum's reading.*
- **Lore.** The rune carver of the Northmen cuts a name and a wish into wood for a sailor to
  carry, and into stone for kin who died abroad. A stick of elderwood is said to hold its
  wish longer, and costs accordingly.
- **Fitting vocations.** Mage.

### Alchemist

#### Athanor

- **Description.** An enclosed furnace built to keep a flask at a constant heat. A Science
  Museum example of grey-glazed stoneware, German or French, 1501-1700, is 45 cm high and
  built to give an equal heat throughout. An athanor is drawn in a manuscript of about
  1450-1500, and a 13th-century book of furnaces shows a vessel in a pan of ashes inside
  one. Source: [Science Museum Group, alchemical furnace
  A634411](https://collection.sciencemuseumgroup.org.uk/objects/co115035/alchemical-furnace),
  [Princeton University Library, "The Philosophers'
  Glass"](https://dpul.princeton.edu/alchemy/feature/the-philosophers-glass).
- **Lore.** An alchemist's work is counted in weeks of even heat, so the furnace must feed
  itself with charcoal while its owner sleeps. Ghostlight peat burns cool and steady and is
  prized for it.
- **Fitting vocations.** Alchemist.

Also used by the Alchemist: [Alembic and cucurbit](#alembic-and-cucurbit) (under Brewer),
[Mortar and pestle](#mortar-and-pestle) (under Healer), [Hessian
crucible](#hessian-crucible) (under Jeweller).

### Seer

#### Shew-stone

- **Description.** John Dee's "shew-stones" were polished translucent or reflective objects
  used in his occult work: his medium Edward Kelly reported visions in them. A crystal
  sphere in the British Museum may be one of the stones named in Dee's manuscripts, though
  the museum says that this cannot be proven. Source: [The British Museum through Google
  Arts & Culture, "Dr Dee's
  magic"](https://artsandculture.google.com/asset/dr-dee-s-magic/jgH1be-D72po1Q).
- **Lore.** Rock crystal is a resource of the volcanic mountains, and a flawless ball of it
  is a seer's whole fortune. An oracle looks into the stone and a second person writes down
  what is said, since the oracle rarely remembers.
- **Fitting vocations.** Seer.

#### Geomantic instrument

- **Description.** A brass plate inlaid with gold and silver, about 27 by 34 cm, made in
  1241/42 and now in the British Museum. It mechanises geomancy, divination from patterns of
  dots traditionally traced in sand. It has sixteen dials that each show a pattern in a
  small window: four are turned at random, twelve more are derived from them, and a panel
  gives the meaning of the last. Source: [The British Museum through Google Arts & Culture,
  brass geomantic
  instrument](https://artsandculture.google.com/asset/brass-geomantic-instrument-made-by-muhammad-ibn-khutlukh-al-mawsili/1QEEJWn3U-nLWg).
- **Lore.** The geomancers of the Canal folk are asked where a house, a grave or a canal
  lock may stand. A village diviner casts the same sixteen figures with a stick in the dust,
  and charges an egg.
- **Fitting vocations.** Seer.

Also used by the Seer: [Astrolabe](#astrolabe) (under Scholar), [Divining
rod](#divining-rod) (under Miner), [Folding almanac](#folding-almanac) (under Healer).

---

## Margins

### Labourer

#### Wooden shovel

- **Description.** A blade of oak in one piece, 32 cm long, from a medieval castle site in
  Wales, of a type dated to the 10th to 14th centuries. A shaft was fitted into a slot in
  the blade and pegged. With no metal shoe, and shoulders that slope too sharply for the
  foot, it was for moving loose material, not for cutting. Source: [Amgueddfa Cymru - Museum
  Wales, medieval wooden shovel
  2005.25H/11.3](https://museum.wales/collections/online/object/1c4f322d-d062-32ff-9436-e79ec55acbc4).
- **Lore.** The labourer is a ninth of all who work and owns the cheapest tool of them all.
  Every ditch, dike, moat and grave of the world was moved one such shovelful at a time, and
  the Rainkeepers' ten thousand reservoirs with it.
- **Fitting vocations.** Labourer.

#### Wheelbarrow

- **Description.** A barrow with one wheel, in medieval Europe usually at or near the front.
  It appeared between about 1170 and 1250: the earliest record is a purchase of 1222 for the
  king's works at Dover, and the earliest picture is of about 1250. It was used in building,
  mining and farming but stayed fairly rare until the 15th century. Source: [Wikipedia,
  "Wheelbarrow" (citing A. L. Matthies, "The Medieval Wheelbarrow", Technology and Culture
  32, 1991)](https://en.wikipedia.org/wiki/Wheelbarrow). *Partially verified. Wikipedia
  only: the article it cites could not be opened.*
- **Lore.** One man with a barrow does the work of two with a hand-barrow between them,
  which is why a master of works buys them and labourers distrust them. The Canal folk fit
  theirs with a sail on the dike roads.
- **Fitting vocations.** Labourer.

Also used by the Labourer: [Scythe](#scythe) (under Farmer), [Spade iron](#spade-iron)
(under Gardener).

### Beggar

#### Leper's clapper

- **Description.** A hand-held wooden clapper. People with leprosy wore distinctive clothing
  and carried a clapper or a bell to warn others of their approach, and it may also have
  drawn attention for alms. The Science Museum piece is a copy, about 26 cm long, of a
  17th-century original from a leper hospital in Kent. Source: [Science Museum Group, copy
  of a 1600s wooden leper clapper
  A635021](https://collection.sciencemuseumgroup.org.uk/objects/co147425/copy-of-a-1600s-wooden-leper-clapper-england).
  *A museum copy of a 17th-century original.*
- **Lore.** The sound clears a street and fills a bowl, and both are the point. The
  Bitterbloods, who are poisonous to the touch, once carried clappers by law in the towns
  that feared them.
- **Fitting vocations.** Beggar.

#### Alms bowl

- **Description.** A wooden bowl on a handle just under 40 cm long, so that money or food
  could be given to people with leprosy without coming close. The original was used in the
  1400s at a leper hospital in Kent founded in 1084. The museum object is a wooden model
  made in 1917-1920. Source: [Science Museum Group, wooden model of a 1400s alms bowl
  A635019](https://collection.sciencemuseumgroup.org.uk/objects/co90594/wooden-model-of-a-1400s-alms-bowl-england-1917-1920).
  *A museum model of a 15th-century original.*
- **Lore.** An almsman of a temple has a licensed place at its door and a bowl with the
  temple's mark. The ascetics of the Rainkeepers carry a bowl by choice and are fed before
  the household eats.
- **Fitting vocations.** Beggar.

#### Pilgrim badge

- **Description.** A small cast badge of lead-tin alloy bought at a shrine as a token of the
  pilgrimage. A London example of 1350-1400, about 75 mm across, shows a saint returning
  from exile on a ship. Many Londoners travelled to the saint's shrine to pray and bought
  badges there. Source: [London Museum, pilgrim badge
  82.8/3](https://www.londonmuseum.org.uk/collections/v/object-29293/st-thomas-becket-return-from-exile/).
- **Lore.** A hat full of badges is a pilgrim's passport and a beggar's credential: the more
  shrines, the more alms. Shrines sell them by the thousand, and some of the wearers have
  been no further than the mould.
- **Fitting vocations.** Beggar.

Also used by the Beggar: [Hurdy-gurdy](#hurdy-gurdy) (under Entertainer).

### Thief

#### False dice

- **Description.** A set of 24 bone dice of the late 15th century, found on the foreshore of
  the Thames in a pewter pot. X-rays show that 18 hold drops of mercury: eleven tend to fall
  on five or six and seven on one or two. The other six carry repeated numbers, three with
  only one to three and three with only four to six. Loaded dice were called fulhams.
  Source: [London Museum, false die
  84.136/3](https://www.londonmuseum.org.uk/collections/v/object-41592/false-die-die).
- **Lore.** Quicksilver is a Reagent of the volcanic lands, and more of it goes into dice
  than the alchemists like to admit. A swindler changes the true dice for the false in the
  palm, which is why a careful tavern keeps its own dice chained to the table.
- **Fitting vocations.** Thief.

#### Picklock

- **Description.** The Pitt Rivers Museum catalogues an English picklock, undated, with a
  stem ending in a hook. The locks of the period were warded: the Science Museum Group holds
  a 14th-century bronze key made for a lock of five wards, and English iron keys of the 15th
  and 16th centuries. Source: [Science Museum Group, medieval keys
  1934-376](https://collection.sciencemuseumgroup.org.uk/objects/co50332/1-14th-century-bronze-key-key-hardware).
  *Partially verified. The opened page supports the medieval keys and the warded lock. The
  picklock rests on a search excerpt of the Pitt Rivers catalogue, and no opened page
  describes a medieval lock-picking tool.*
- **Lore.** A warded lock stops an honest key of the wrong shape and little else, as every
  burglar knows. The fine locks of the Gnomes are another matter, and a Dwarven vault keeper
  trusts a guard more than either.
- **Fitting vocations.** Thief.

#### Bollock dagger

- **Description.** A dagger with a one-piece wooden handle ending in two oval lobes at the
  blade, with an iron blade of 195 mm, of the mid 15th to early 16th century. Such daggers
  were often civilian weapons: although wearing armour and swords in the street was banned,
  most men carried a dagger in a sheath on the belt, and fights often ended in stabbings.
  Source: [London Museum, bollock dagger
  96.25/6](https://www.londonmuseum.org.uk/collections/v/object-138912/dagger-bollock-dagger).
- **Lore.** The knife every man carries to cut his bread is the one that cuts a purse string
  in a crowd or settles a quarrel on the road. Because everyone has one, nobody can be
  hanged for owning it.
- **Fitting vocations.** Thief, Bandit.

### Bandit

#### Falchion

- **Description.** A medieval single-handed sword with a large curved blade. The early,
  broad ones are called cleaver falchions. The Conyers falchion at Durham Cathedral has a
  steel blade, a guard and pommel of bronze and a grip of ash, and its heraldry suggests a
  date before 1272. It was presented to each new bishop as a sword of tenure. Source:
  [Durham Cathedral, "The Conyers
  Falchion"](https://www.durhamcathedral.co.uk/explore/treasures-collections/our-most-famous-items/the-conyers-falchion).
  *The specimen is a ceremonial knightly sword. That outlaws carried falchions is the
  judgement of this document.*
- **Lore.** It is cheaper than a knight's sword and asks less skill, a butcher's cleaver
  made long. Raiders, pirates and the veiled raiders of the salt road carry it, and so do
  the caravan guards hired against them.
- **Fitting vocations.** Bandit.

Also used by the Bandit: [Longbow](#longbow) (under Hunter), [Bollock
dagger](#bollock-dagger) (under Thief), [Caltrop](#caltrop) (under Soldier).

---

## Summary

| Tool | Vocations | What it is |
|---|---|---|
| [Heavy plough](#heavy-plough) | Farmer | Wheeled plough with an iron coulter and share that cuts deep furrows in heavy soil |
| [Sickle](#sickle) | Farmer | Curved iron blade with a serrated inner edge for reaping grain |
| [Flail](#flail) | Farmer | Two sticks joined by a leather thong, for threshing grain by hand |
| [Scythe](#scythe) | Farmer, Labourer | Long blade on a two-handled shaft for mowing hay |
| [Spade iron](#spade-iron) | Gardener, Labourer | Wooden spade with a separate iron cutting edge |
| [Bee skep](#bee-skep) | Gardener | Domed hive of coiled straw |
| [Billhook](#billhook) | Gardener, Forester | Hooked iron blade on a short handle for pruning, hedging and cutting branches |
| [Shepherd's crook](#shepherds-crook) | Herder | Long staff with a hook for catching a sheep |
| [Iron shears](#iron-shears) | Herder, Tailor | One-piece sprung iron shears for shearing sheep and cutting cloth |
| [Plunge churn](#plunge-churn) | Herder | Tall coopered barrel with a plunger for making butter |
| [Seine net](#seine-net) | Fisher | Long net with lead sinkers and floats, hauled from a bank or a boat |
| [Eel trap](#eel-trap) | Fisher | Funnel-mouthed wicker basket set on a river bed for eels |
| [Eel spear](#eel-spear) | Fisher | Barbed iron fork thrust into river mud for eels |
| [Boar spear](#boar-spear) | Hunter | Heavy hunting spear with a broad steel head |
| [Falcon's hood](#falcons-hood) | Hunter | Leather hood that keeps a trained falcon calm on the glove |
| [Hunting horn](#hunting-horn) | Hunter | Horn blown to signal in the hunt; the finest are carved from ivory |
| [Felling axe](#felling-axe) | Forester | Iron axe head on a long haft for felling trees |
| [Two-man saw](#two-man-saw) | Forester, Carpenter | Long rip saw worked by two sawyers, one above the log and one below |
| [Miner's iron and hammer](#miners-iron-and-hammer) | Miner | Pointed iron bar held against the rock and struck with a hammer |
| [Wedge, crowbar and pick](#wedge-crowbar-and-pick) | Miner, Quarryman | Iron wedges, bars and a pointed pick for breaking and prising rock |
| [Windlass](#windlass) | Miner | Hand-cranked barrel over a shaft that winds buckets up on a rope |
| [Quarry wedges](#quarry-wedges) | Quarryman | Iron wedges, later plugs and feathers, hammered in to split blocks of stone |
| [Quarry pick](#quarry-pick) | Quarryman | Iron pick for uncovering stone and cutting it free of the bed |
| [Crowbar and sledge](#crowbar-and-sledge) | Quarryman | Iron bar to lever stone off its bed and heavy hammer to drive wedges |
| [Lead salt pan](#lead-salt-pan) | Salter | Shallow lead pan in which brine is boiled down to salt |
| [Salt rake and shovel](#salt-rake-and-shovel) | Salter | Wooden rake and shovel for drawing salt out of the pans and basins |
| [Salt basket](#salt-basket) | Salter | Tall conical basket in which wet salt drains into a loaf |
| [Millstones](#millstones) | Miller | Pair of grooved grinding stones; the upper one turns |
| [Mill bill and thrift](#mill-bill-and-thrift) | Miller | Steel pick in a wooden handle for re-cutting the grooves of a millstone |
| [Rotary quern](#rotary-quern) | Miller | Hand mill of two round stones for grinding grain at home |
| [Baker's peel](#bakers-peel) | Baker | Long-handled flat shovel for moving loaves in and out of the oven |
| [Kneading trough](#kneading-trough) | Baker | Wooden trough in which dough is worked; often the lid of the meal chest |
| [Mash tun and masher](#mash-tun-and-masher) | Brewer | Wooden vat where malt is steeped in hot water, stirred with a barred paddle |
| [Brewing kettle](#brewing-kettle) | Brewer | Large kettle over an open fire in which the brew is boiled |
| [Alembic and cucurbit](#alembic-and-cucurbit) | Brewer, Alchemist | Two-part still: a heated gourd-shaped vessel under a head that condenses the vapour |
| [Cleaver](#cleaver) | Butcher | Broad heavy iron knife for jointing carcasses |
| [Pole axe](#pole-axe) | Butcher | Long-handled axe for felling cattle at slaughter |
| [Charcoal clamp](#charcoal-clamp) | Charcoal burner | Stack of wood under turf, burned slowly to charcoal |
| [Corrack and long shovel](#corrack-and-long-shovel) | Charcoal burner | Collier's rake for drawing the finished charcoal and long shovel for tending the clamp |
| [Lime kiln](#lime-kiln) | Charcoal burner, Mason | Kiln where limestone or chalk is burned to lime for mortar |
| [Bloomery furnace](#bloomery-furnace) | Smelter | Clay shaft furnace that reduces iron ore to a spongy bloom |
| [Bellows](#bellows) | Smelter, Smith | Hinged boards and leather that blow air into a furnace or forge |
| [Cupel](#cupel) | Smelter, Jeweller | Small bone-ash dish that soaks up lead and leaves a bead of silver or gold |
| [Unhairing knife and beam](#unhairing-knife-and-beam) | Tanner | Blunt two-handled curved knife that scrapes hair from a hide laid over a rounded beam |
| [Tan pits](#tan-pits) | Tanner | Row of pits where hides soak in ever stronger oak-bark liquor |
| [Lunellum and herse](#lunellum-and-herse) | Tanner | Crescent knife that scrapes a skin stretched on a frame down to parchment |
| [Spindle, whorl and distaff](#spindle-whorl-and-distaff) | Weaver | Weighted stick that twists fibre into yarn, fed from a staff that holds the wool or flax |
| [Loom](#loom) | Weaver | Frame that holds the warp taut for weaving: upright with weights, or horizontal with treadles |
| [Cropping shears and teasels](#cropping-shears-and-teasels) | Weaver | Teasel heads that raise the nap of wool cloth and great shears that crop it smooth |
| [Anvil](#anvil) | Smith | Iron block with a hardened steel face on which hot metal is hammered |
| [Hammers and tongs](#hammers-and-tongs) | Smith | Hand-hammer and two-handed sledges, with tongs to hold the hot iron |
| [Carpenter's axe](#carpenters-axe) | Carpenter, Shipwright | Heavy two-handed axe for squaring and trimming timbers |
| [Adze](#adze) | Carpenter, Shipwright | Axe-like tool with the blade set across the handle, for shaving timber smooth |
| [Auger](#auger) | Carpenter | Boring tool turned by hand to make large round holes in timber |
| [Plane](#plane) | Carpenter | Wooden block with a set blade that shaves a board smooth |
| [Pole lathe](#pole-lathe) | Carpenter | Lathe spun by a treadle and a cord, sprung back by a bent pole |
| [Caulking iron and mallet](#caulking-iron-and-mallet) | Shipwright | Blunt chisel and ringed mallet that drive fibre into the seams of a hull |
| [Clench nails and roves](#clench-nails-and-roves) | Shipwright | Iron rivets and washers that fasten overlapping hull planks |
| [Mallet and chisels](#mallet-and-chisels) | Mason | Round wooden mallet and iron chisels for dressing and carving stone |
| [Plumb rule, level and square](#plumb-rule-level-and-square) | Carpenter, Mason | Weighted line on a rule, long level and large square for building true |
| [Treadwheel crane](#treadwheel-crane) | Mason | Great wooden wheel walked by men inside it, to hoist stone |
| [Lewis](#lewis) | Mason | Iron cramp that grips a slot cut in a stone block so that a crane can lift it |
| [Trowel](#trowel) | Mason | Flat iron blade for taking up and spreading mortar and trimming bricks |
| [Potter's wheel](#potters-wheel) | Potter | Turning wheel on which pots are thrown |
| [Pottery kiln](#pottery-kiln) | Potter | Updraught kiln with a fire below and the stacked pots above |
| [Stamps and roulettes](#stamps-and-roulettes) | Potter | Small dies and roller stamps that press patterns into wet clay |
| [Blowpipe](#blowpipe) | Glassmaker | Long iron tube on which molten glass is gathered and blown |
| [Pontil](#pontil) | Glassmaker | Solid iron rod stuck to the base of a vessel to hold it while the rim is worked |
| [Jacks and shears](#jacks-and-shears) | Glassmaker | Sprung two-armed tool that shapes hot glass and shears that trim it |
| [Grozing iron](#grozing-iron) | Glassmaker | Notched iron that nibbles a pane of glass to shape for a leaded window |
| [Round knife](#round-knife) | Leatherworker | The shoemaker's broad cutting knife, the badge of the trade |
| [Awl](#awl) | Leatherworker | Pointed iron tool that pierces leather for the stitch |
| [Shoemaker's last](#shoemakers-last) | Leatherworker | Wooden model of a foot over which a shoe is shaped |
| [Thimble and needle](#thimble-and-needle) | Tailor | Pitted metal cap that pushes the needle through cloth |
| [Touchstone and touch needles](#touchstone-and-touch-needles) | Jeweller, Merchant | Black stone on which gold is streaked and compared with needles of known fineness |
| [Coin dies](#coin-dies) | Jeweller | Pair of engraved iron dies between which a blank is struck into a coin |
| [Draw plate](#draw-plate) | Jeweller | Hard metal plate with tapered holes through which wire is pulled thinner |
| [Graver](#graver) | Jeweller | Steel point in a wooden handle that cuts lines into metal |
| [Grinding slab and muller](#grinding-slab-and-muller) | Artist | Slab of hard stone and a hand stone for grinding pigments to paint |
| [Minever and bristle brushes](#minever-and-bristle-brushes) | Artist | Fine brushes of squirrel tail in a quill and coarse ones of hog bristle |
| [Silver style](#silver-style) | Artist | Silver-tipped stylus that draws a fine grey line on a prepared surface |
| [Turns and bow](#turns-and-bow) | Artificer | Small bow-driven lathe held in a vice, for turning and drilling fine parts |
| [Screw plate](#screw-plate) | Artificer | Steel plate with graded threaded holes for cutting screws |
| [Coin balance](#coin-balance) | Merchant, Shopkeeper | Small folding scales for checking the weight of coins |
| [Jettons and counting board](#jettons-and-counting-board) | Merchant, Shopkeeper, Official, Scribe, Scholar | Brass counters moved on a chequered board or cloth to reckon sums |
| [Tally stick](#tally-stick) | Merchant, Innkeeper, Official | Notched stick split in two, each party keeping half as proof of a debt |
| [Steelyard](#steelyard) | Merchant, Shopkeeper | Balance with one long arm along which a weight is hung |
| [Standard yard](#standard-yard) | Shopkeeper, Official | Bronze bar that fixes the length by which cloth is measured |
| [Standard bushel](#standard-bushel) | Shopkeeper, Official | Bronze vessel that fixes the measure by which grain is sold |
| [Horse collar](#horse-collar) | Carter | Padded collar that lets a horse pull with its shoulders |
| [Two-wheeled cart](#two-wheeled-cart) | Carter | Cart with shafts for one draught animal |
| [Pack saddle](#pack-saddle) | Carter | Wooden frame on an animal's back that carries a load on each side |
| [Sounding lead and line](#sounding-lead-and-line) | Sailor | Lead weight on a marked line, lowered to measure depth and sample the seabed |
| [Mariner's compass](#mariners-compass) | Sailor | Magnetised needle under a card in a box, hung in rings to stay level |
| [Mariner's astrolabe](#mariners-astrolabe) | Sailor | Heavy brass ring with a sighting arm for taking the height of the sun or a star at sea |
| [Black jack](#black-jack) | Innkeeper | Leather tankard or jug for ale |
| [Ale-stake](#ale-stake) | Innkeeper | Pole thrust out over the street as the sign of a tavern |
| [Fleam and bleeding bowl](#fleam-and-bleeding-bowl) | Healer | Small blade for opening a vein and a bowl to catch the blood |
| [Uroscopy flask](#uroscopy-flask) | Healer | Clear glass flask in which a physician inspects a patient's urine |
| [Mortar and pestle](#mortar-and-pestle) | Healer, Alchemist | Heavy bronze bowl and club for grinding herbs, spices and drugs |
| [Cauldron](#cauldron) | Servant | Metal pot on feet that cooks over the open hearth |
| [Roasting spit](#roasting-spit) | Servant | Long iron rod with a crank on which meat is turned before the fire |
| [Harp](#harp) | Entertainer | Framed, plucked string instrument of the hall |
| [Pipe and tabor](#pipe-and-tabor) | Entertainer | Three-holed pipe played with one hand while the other beats a small drum |
| [Hurdy-gurdy](#hurdy-gurdy) | Entertainer, Beggar | String instrument sounded by a cranked wheel, with keys and drone strings |
| [Mirror in an ivory case](#mirror-in-an-ivory-case) | Courtesan | Small hand mirror kept between two carved ivory discs |
| [Boxwood comb](#boxwood-comb) | Servant, Courtesan | Two-sided comb with coarse teeth for dressing hair and fine teeth for cleaning it |
| [Pomander](#pomander) | Courtesan | Pierced metal ball worn on a chain, filled with scent |
| [Seal matrix](#seal-matrix) | Noble, Official | Engraved die pressed into wax to authenticate a document |
| [Knightly sword](#knightly-sword) | Noble, Soldier | Long double-edged sword, the chief weapon of the armoured knight |
| [Spurs](#spurs) | Noble | Pointed or rowelled irons strapped to a rider's heels |
| [Mace of office](#mace-of-office) | Official | Ornamented staff carried as the sign of an official's authority |
| [Longbow](#longbow) | Hunter, Soldier, Bandit | Bow of a single stave of yew, as tall as the archer |
| [Bill and pike](#bill-and-pike) | Soldier, Guard | Hooked blade on a staff, and a very long spear used in close ranks |
| [Mail shirt](#mail-shirt) | Soldier | Shirt of thousands of riveted iron rings |
| [Halberd](#halberd) | Soldier, Guard | Axe blade, spike and hook on a long pole |
| [Watchman's horn](#watchmans-horn) | Guard | Horn blown to set the night watch and raise the alarm |
| [Horn lantern](#horn-lantern) | Guard | Lantern with thin panes of horn in place of glass |
| [Fire hook](#fire-hook) | Guard | Great iron hook on a long pole for pulling down buildings in the path of a fire |
| [Censer and incense boat](#censer-and-incense-boat) | Priest | Pierced metal vessel swung on chains to burn incense |
| [Chalice](#chalice) | Priest | Stemmed cup of precious metal for the wine of the rite |
| [Shaman's drum](#shamans-drum) | Priest | Painted frame drum struck with an antler hammer to reach a trance |
| [Quill pen and penknife](#quill-pen-and-penknife) | Scribe | Pen cut from a wing feather, re-cut with a small knife |
| [Wax tablet and stylus](#wax-tablet-and-stylus) | Scribe, Scholar | Wooden board filled with wax, written on with a point and smoothed for reuse |
| [Pricker](#pricker) | Scribe | Sharp point that pricks guide holes for ruling the lines of a page |
| [Astrolabe](#astrolabe) | Scholar, Seer | Brass disc with a turning star map for finding the time and the positions of the sun and stars |
| [Armillary sphere](#armillary-sphere) | Scholar | Model of the heavens built of brass rings |
| [Seals of wax](#seals-of-wax) | Mage | Wax discs cut with names and signs, set under the table and the stone of a magical working |
| [Book of conjurations](#book-of-conjurations) | Mage | Handwritten working manual of rites, tables and signs, with the tools it prescribes |
| [Rune stick](#rune-stick) | Mage | Small stick cut with runes and worn or carried as a charm |
| [Athanor](#athanor) | Alchemist | Enclosed furnace that keeps a vessel at a steady heat |
| [Hessian crucible](#hessian-crucible) | Jeweller, Alchemist | Ceramic melting pot that withstands great heat and strong reagents |
| [Shew-stone](#shew-stone) | Seer | Polished crystal in which a seer looks for visions |
| [Divining rod](#divining-rod) | Miner, Seer | Forked twig held in both fists, said to dip over a vein of ore or water |
| [Geomantic instrument](#geomantic-instrument) | Seer | Brass plate of dials that casts and reads the dot figures of geomancy |
| [Folding almanac](#folding-almanac) | Healer, Seer | Folded parchment calendar of moon, planets and eclipses, hung from the belt |
| [Wooden shovel](#wooden-shovel) | Labourer | One-piece wooden blade on a shaft, for moving loose earth |
| [Wheelbarrow](#wheelbarrow) | Labourer | One-wheeled barrow pushed by a single worker |
| [Leper's clapper](#lepers-clapper) | Beggar | Wooden clapper shaken to warn of the bearer's approach and to ask for alms |
| [Alms bowl](#alms-bowl) | Beggar | Wooden bowl on a long handle, held out for coins or food |
| [Pilgrim badge](#pilgrim-badge) | Beggar | Small cast pewter badge bought at a shrine and worn to show the journey |
| [False dice](#false-dice) | Thief | Loaded and mis-numbered dice for cheating at play |
| [Picklock](#picklock) | Thief | Hooked iron for turning a lock without its key |
| [Bollock dagger](#bollock-dagger) | Thief, Bandit | Belt dagger with a two-lobed wooden hilt |
| [Falchion](#falchion) | Bandit | Short, broad, single-edged sword |
| [Caltrop](#caltrop) | Soldier, Bandit | Four iron spikes joined so that one always points up, strewn to lame horses and men |

## Tools per vocation

| Vocation | Tools |
|---|---|
| Farmer | Heavy plough, Sickle, Flail, Scythe |
| Gardener | Spade iron, Bee skep, Billhook |
| Herder | Shepherd's crook, Iron shears, Plunge churn |
| Fisher | Seine net, Eel trap, Eel spear |
| Hunter | Boar spear, Falcon's hood, Hunting horn, Longbow |
| Forester | Billhook, Felling axe, Two-man saw |
| Miner | Miner's iron and hammer, Wedge, crowbar and pick, Windlass, Divining rod |
| Quarryman | Wedge, crowbar and pick, Quarry wedges, Quarry pick, Crowbar and sledge |
| Salter | Lead salt pan, Salt rake and shovel, Salt basket |
| Miller | Millstones, Mill bill and thrift, Rotary quern |
| Baker | Baker's peel, Kneading trough |
| Brewer | Mash tun and masher, Brewing kettle, Alembic and cucurbit |
| Butcher | Cleaver, Pole axe |
| Charcoal burner | Charcoal clamp, Corrack and long shovel, Lime kiln |
| Smelter | Bloomery furnace, Bellows, Cupel |
| Tanner | Unhairing knife and beam, Tan pits, Lunellum and herse |
| Weaver | Spindle, whorl and distaff, Loom, Cropping shears and teasels |
| Smith | Bellows, Anvil, Hammers and tongs |
| Carpenter | Two-man saw, Carpenter's axe, Adze, Auger, Plane, Pole lathe, Plumb rule, level and square |
| Shipwright | Carpenter's axe, Adze, Caulking iron and mallet, Clench nails and roves |
| Mason | Lime kiln, Mallet and chisels, Plumb rule, level and square, Treadwheel crane, Lewis, Trowel |
| Potter | Potter's wheel, Pottery kiln, Stamps and roulettes |
| Glassmaker | Blowpipe, Pontil, Jacks and shears, Grozing iron |
| Leatherworker | Round knife, Awl, Shoemaker's last |
| Tailor | Iron shears, Thimble and needle |
| Jeweller | Cupel, Touchstone and touch needles, Coin dies, Draw plate, Graver, Hessian crucible |
| Artist | Grinding slab and muller, Minever and bristle brushes, Silver style |
| Artificer | Turns and bow, Screw plate |
| Merchant | Touchstone and touch needles, Coin balance, Jettons and counting board, Tally stick, Steelyard |
| Shopkeeper | Coin balance, Jettons and counting board, Steelyard, Standard yard, Standard bushel |
| Carter | Horse collar, Two-wheeled cart, Pack saddle |
| Sailor | Sounding lead and line, Mariner's compass, Mariner's astrolabe |
| Innkeeper | Tally stick, Black jack, Ale-stake |
| Healer | Fleam and bleeding bowl, Uroscopy flask, Mortar and pestle, Folding almanac |
| Servant | Cauldron, Roasting spit, Boxwood comb |
| Entertainer | Harp, Pipe and tabor, Hurdy-gurdy |
| Courtesan | Mirror in an ivory case, Boxwood comb, Pomander |
| Noble | Seal matrix, Knightly sword, Spurs |
| Official | Jettons and counting board, Tally stick, Standard yard, Standard bushel, Seal matrix, Mace of office |
| Soldier | Knightly sword, Longbow, Bill and pike, Mail shirt, Halberd, Caltrop |
| Guard | Bill and pike, Halberd, Watchman's horn, Horn lantern, Fire hook |
| Priest | Censer and incense boat, Chalice, Shaman's drum |
| Scribe | Jettons and counting board, Quill pen and penknife, Wax tablet and stylus, Pricker |
| Scholar | Jettons and counting board, Wax tablet and stylus, Astrolabe, Armillary sphere |
| Mage | Seals of wax, Book of conjurations, Rune stick |
| Alchemist | Alembic and cucurbit, Mortar and pestle, Athanor, Hessian crucible |
| Seer | Astrolabe, Shew-stone, Divining rod, Geomantic instrument, Folding almanac |
| Labourer | Scythe, Spade iron, Wooden shovel, Wheelbarrow |
| Beggar | Hurdy-gurdy, Leper's clapper, Alms bowl, Pilgrim badge |
| Thief | False dice, Picklock, Bollock dagger |
| Bandit | Longbow, Bollock dagger, Falchion, Caltrop |

## Sources

The 149 pages behind the descriptions, by owner, each with the tools it supports. All were
opened on 2026-11-19. The notes in the entries say what a page does not support.

- "Charcoal Burning Tools (1)". <https://www.revolutionaryplayers.org.uk/?p=2377>. Used for:
  Corrack and long shovel.
- "Salt in the Middle Ages".
  <https://saltassociation.co.uk/education/salt-history/middle-ages/>. Used for: Lead salt
  pan.
- Amgueddfa Cymru - Museum Wales, billhook 80.46H/2.1.
  <https://museum.wales/collections/online/object/74c03e95-85b9-331a-86ee-cbad1a65a448>.
  Used for: Billhook.
- Amgueddfa Cymru - Museum Wales, churn 62.323.2.
  <https://museum.wales/collections/online/object/60b5dd48-dc3e-3c4f-b340-124a121fb7d2>.
  Used for: Plunge churn.
- Amgueddfa Cymru - Museum Wales, iron spade 2018.8H.
  <https://museum.wales/collections/online/object/48a5411b-ff46-3339-9d78-816a294335ee>.
  Used for: Spade iron.
- Amgueddfa Cymru - Museum Wales, medieval wooden shovel 2005.25H/11.3.
  <https://museum.wales/collections/online/object/1c4f322d-d062-32ff-9436-e79ec55acbc4>.
  Used for: Wooden shovel.
- Amgueddfa Cymru - Museum Wales, pack saddle.
  <https://museum.wales/collections/online/object/04a8a947-3a0d-3185-bc2b-9fdaeb4f675c>.
  Used for: Pack saddle.
- Amgueddfa Cymru - Museum Wales, Roman iron caltrop 32.60/4.3.
  <https://museum.wales/collections/online/object/6ee6fabb-dbbe-360c-badf-de51f30ce157>.
  Used for: Caltrop.
- Amgueddfa Cymru - Museum Wales, St Fagans, "Tannery".
  <https://museum.wales/stfagans/buildings/tannery/>. Used for: Tan pits.
- Amgueddfa Cymru - Museum Wales, unhairing knife 68.121.293.
  <https://museum.wales/collections/online/object/c6a8624e-f5ef-3035-a8d3-ff6bb2f62bb8?page=45>.
  Used for: Unhairing knife and beam.
- Bradford on Avon Museum, "quarrying and stone-masons".
  <https://www.bradfordonavonmuseum.co.uk/?p=1910>. Used for: Quarry pick, Mallet and
  chisels, Lewis.
- British Geological Survey, "Building stones of Edinburgh: quarrying methods".
  <https://earthwise.bgs.ac.uk/index.php/Building_stones_of_Edinburgh:_quarrying_methods>.
  Used for: Quarry wedges, Quarry pick, Crowbar and sledge.
- British History Online, H. T. Riley, Memorials of London and London Life, "Memorials:
  1375". <https://www.british-history.ac.uk/no-series/memorials-london-life/pp381-392>. Used
  for: Ale-stake.
- British Library, "Keep taking the (wax) tablets".
  <https://www.bl.uk/stories/blogs/posts/keep-taking-the-wax-tablets>. Used for: Wax tablet
  and stylus.
- British Library, catalogue of Add MS 42130 (Luttrell Psalter).
  <https://searcharchives.bl.uk/catalog/032-002085986>. Used for: Two-wheeled cart.
- C. M. Martin and N. Q. Bogdan, "The boat timbers", Perth High Street Excavation, Fascicule
  2.
  <https://www.tafac.org.uk/wp-content/uploads/2023/09/PHSE-Fasc-2_p317-322.boattimbers.pdf>.
  Used for: Clench nails and roves.
- Canterbury Cathedral Learning, "How did they build that? Stone".
  <https://learning.canterbury-cathedral.org/how-did-they-build-that/stone>. Used for:
  Treadwheel crane.
- Case Western Reserve University, Early Music Instrument Database, "Harp (Medieval)".
  <http://caslabs.case.edu/medren/medieval-instruments/harp-medieval/>. Used for: Harp.
- Case Western Reserve University, Early Music Instrument Database, "Hurdy-gurdy
  (Medieval)". <http://caslabs.case.edu/medren/medieval-instruments/hurdy-gurdy-medieval/>.
  Used for: Hurdy-gurdy.
- Case Western Reserve University, Early Music Instrument Database, "Tabor pipe".
  <https://caslabs.case.edu/medren/renaissance-instruments/tabor-pipe/>. Used for: Pipe and
  tabor.
- Cennino Cennini, The Craftsman's Handbook, trans. D. V. Thompson, ch. LXIIII-LXV.
  <https://noteaccess.com/Texts/Cennini/2a.htm>. Used for: Minever and bristle brushes.
- Cennino Cennini, The Craftsman's Handbook, trans. D. V. Thompson, ch. V-VIII.
  <https://noteaccess.com/Texts/Cennini/1.htm>. Used for: Silver style.
- Cennino Cennini, The Craftsman's Handbook, trans. D. V. Thompson, ch. XXXVI.
  <https://noteaccess.com/Texts/Cennini/2.htm>. Used for: Grinding slab and muller.
- Chetham's Library, "Conjuring spirits in the 'Tractatus de Nigromantia'".
  <https://library.chethams.com/?p=575>. Used for: Book of conjurations.
- Cleveland Museum of Art, halberd 1916.1542. <https://www.clevelandart.org/art/1916.1542>.
  Used for: Halberd.
- Cornell University Library Conservation, "Parchment-making".
  <https://blogs.cornell.edu/culconservation/2015/04/03/parchment-making/>. Used for:
  Lunellum and herse.
- Corning Museum of Glass, Rakow Research Library, glassmaking terms.
  <https://libanswers.cmog.org/faq/143658>. Used for: Blowpipe, Pontil, Jacks and shears.
- Durham Cathedral, "The Conyers Falchion".
  <https://www.durhamcathedral.co.uk/explore/treasures-collections/our-most-famous-items/the-conyers-falchion>.
  Used for: Falchion.
- Encyclopaedia Britannica, "Cart". <https://www.britannica.com/technology/cart>. Used for:
  Two-wheeled cart.
- Encyclopaedia Britannica, "Horse collar".
  <https://www.britannica.com/technology/horse-collar>. Used for: Horse collar.
- Food Museum, Stowmarket, crook STMEA:1987-3.2. <https://foodmuseum.org.uk/ehive/65689>.
  Used for: Shepherd's crook.
- Food Museum, Stowmarket, masher STMEA:B.230. <https://foodmuseum.org.uk/ehive/369923>.
  Used for: Mash tun and masher.
- Fordham Medieval Sourcebook, introduction to Theophilus.
  <https://sourcebooks.web.fordham.edu/source/theophilus.asp>. Used for: Draw plate.
- Friends of New Hall Mill, "Dressing the Millstones".
  <https://www.newhallmill.uk/dressing-the-millstones/>. Used for: Millstones, Mill bill and
  thrift.
- Georgius Agricola, De re metallica (1556), trans. H. C. and L. H. Hoover, Book VI
  (Internet Archive full text).
  <https://archive.org/stream/deremetallica50agri/deremetallica50agri_djvu.txt>. Used for:
  Miner's iron and hammer, Wedge, crowbar and pick, Windlass.
- Georgius Agricola, De re metallica (1556), trans. Hoover, Book II (Project Gutenberg
  mirror). <https://gutenberg.pglaf.org/3/8/0/1/38015/38015-h/38015-h.htm>. Used for:
  Divining rod.
- Georgius Agricola, De re metallica (1556), trans. Hoover, Book XII (scan at e-rara, ETH
  Zurich). <https://www.e-rara.ch/ebs/content/pagetext/28966762>. Used for: Salt rake and
  shovel.
- Georgius Agricola, De re metallica (1556), trans. Hoover, Book XII (scan at e-rara, ETH
  Zurich). <https://www.e-rara.ch/ebs/content/pagetext/28966768>. Used for: Salt basket.
- GOV.UK History of government blog, "The Exchequer: a chequered history".
  <https://history.blog.gov.uk/2013/08/14/the-exchequer-a-chequered-history/>. Used for:
  Jettons and counting board.
- Hampshire Cultural Trust, Anglo-Gallic jetton WINCM:C2442.
  <https://collections.hampshireculture.org.uk/object/jetton-anglo-gallic>. Used for:
  Jettons and counting board.
- Herefordshire Through Time, "Pack horse transport".
  <https://htt.herefordshire.gov.uk/herefordshires-past/the-post-medieval-period/transport/guest-author-essay-pack-horse-transport>.
  Used for: Pack saddle.
- Historic England, "Archaeological and Historic Pottery Production Sites" (2015).
  <https://historicengland.org.uk/images-books/publications/archaeological-and-historic-pottery-production-sites/heag019-pottery-production-sites/>.
  Used for: Potter's wheel, Stamps and roulettes.
- Historic England, "Pre-industrial Ironworks" (Introductions to Heritage Assets).
  <https://historicengland.org.uk/images-books/publications/iha-preindustrial-ironworks/heag221-pre-industrial-ironworks>.
  Used for: Bloomery furnace, Bellows.
- Historic England, "Pre-industrial Lime Kilns" (Introductions to Heritage Assets).
  <https://historicengland.org.uk/images-books/publications/iha-preindustrial-lime-kilns>.
  Used for: Lime kiln.
- Historic England, "Roman and Medieval Pottery and Tile Production" (Introductions to
  Heritage Assets, 2018).
  <https://historicengland.org.uk/images-books/publications/iha-roman-medieval-pottery-tile-production/heag228-roman-medieval-pottery-tile-production/>.
  Used for: Pottery kiln.
- Historic Environment Scotland, "The tools and techniques of traditional ship caulking".
  <https://blog.engineshed.scot/2026/07/02/traditional-ship-caulking/>. Used for: Caulking
  iron and mallet.
- HMML School lexicon, "Herse". <https://hmmlschool.org/lexicon/149229/>. Used for: Lunellum
  and herse.
- I. Marc Carlson, "Lasts, History and use in Medieval Shoes", Footwear of the Middle Ages
  (archived). <https://sorgm.github.io/carlson1999-archive/shoe/APP2.HTM>. Used for:
  Shoemaker's last.
- I. Marc Carlson, "The Medieval Shoemaker's Tools", Footwear of the Middle Ages (archived).
  <https://sorgm.github.io/carlson1999-archive/shoe/APP1.HTM>. Used for: Round knife, Awl.
- Joseph Moxon, Mechanick Exercises (3rd ed., 1703), smithing (Internet Archive full text).
  <https://archive.org/stream/mechanickexercis00moxo_1/mechanickexercis00moxo_1_djvu.txt>.
  Used for: Bellows, Anvil, Hammers and tongs, Carpenter's axe, Adze, Auger, Plane, Pole
  lathe, Plumb rule, level and square, Trowel.
- London Museum, "The Baker's Peel", token 96.66/223.
  <https://www.londonmuseum.org.uk/collections/v/object-431578/the-bakers-peel/>. Used for:
  Baker's peel.
- London Museum, black jack 4566.
  <https://www.londonmuseum.org.uk/collections/v/object-440161/black-jack/>. Used for: Black
  jack.
- London Museum, bollock dagger 96.25/6.
  <https://www.londonmuseum.org.uk/collections/v/object-138912/dagger-bollock-dagger>. Used
  for: Bollock dagger.
- London Museum, cauldron.
  <https://www.londonmuseum.org.uk/collections/v/object-35519/cauldron>. Used for: Cauldron.
- London Museum, cleaver 9354.
  <https://www.londonmuseum.org.uk/collections/v/object-36013/cleaver/>. Used for: Cleaver.
- London Museum, cupel WFG18[214]<103>.
  <https://www.londonmuseum.org.uk/collections/v/object-526525/cupel/>. Used for: Cupel.
- London Museum, double-edged sword 52.12(1).
  <https://londonmuseum.org.uk/collections/v/object-37133/double-edged-sword>. Used for:
  Knightly sword.
- London Museum, eel spear DK2001.68.
  <https://www.londonmuseum.org.uk/collections/v/object-542652/spear-eel-spear/>. Used for:
  Eel spear.
- London Museum, false die 84.136/3.
  <https://www.londonmuseum.org.uk/collections/v/object-41592/false-die-die>. Used for:
  False dice.
- London Museum, fire hook B737.
  <https://www.londonmuseum.org.uk/collections/v/object-119065/fire-hook/>. Used for: Fire
  hook.
- London Museum, hauberk 35.52.
  <https://www.londonmuseum.org.uk/collections/v/object-36460/hauberk-mail-shirt>. Used for:
  Mail shirt.
- London Museum, net sinkers 26710.
  <https://www.londonmuseum.org.uk/collections/v/object-36027/net-sinker-fishing-weights>.
  Used for: Seine net.
- London Museum, Nuremberg jetton.
  <https://www.londonmuseum.org.uk/collections/v/object-119527/nuremburg-jetton>. Used for:
  Jettons and counting board.
- London Museum, pilgrim badge 82.8/3.
  <https://www.londonmuseum.org.uk/collections/v/object-29293/st-thomas-becket-return-from-exile/>.
  Used for: Pilgrim badge.
- London Museum, pottery alembic.
  <https://www.londonmuseum.org.uk/collections/v/object-534829/pottery-vessel-alembic>. Used
  for: Alembic and cucurbit.
- London Museum, quern NN50774.
  <https://www.londonmuseum.org.uk/collections/v/object-142445/quern/>. Used for: Rotary
  quern.
- London Museum, spit 84.314/10.
  <https://www.londonmuseum.org.uk/collections/v/object-109846/spit>. Used for: Roasting
  spit.
- London Museum, steelyard weight 2008.134.
  <https://www.londonmuseum.org.uk/collections/v/object-773802/weight-steelyard-weight/>.
  Used for: Steelyard.
- London Museum, stylus A1319.
  <https://www.londonmuseum.org.uk/collections/v/object-36078/stylus>. Used for: Wax tablet
  and stylus.
- London Museum, thimble A3541.
  <https://www.londonmuseum.org.uk/collections/v/object-36218/thimble>. Used for: Thimble
  and needle.
- M. Hoffmann, "Clothmaking", in Medieval Scandinavia: An Encyclopedia (1993), hosted by UC
  Davis. <https://medieval.ucdavis.edu/SWEDEN/Clothmaking.html>. Used for: Spindle, whorl
  and distaff, Loom.
- Mary Rose Trust, "Christmas on the Mary Rose".
  <https://maryrose.org/christmas-on-the-mary-rose/>. Used for: Pipe and tabor.
- Mary Rose Trust, "Longbows and arrows".
  <https://maryrose.org/discover/collections/the-weaponry-of-the-mary-rose/longbows-and-arrows/>.
  Used for: Longbow.
- Mary Rose Trust, "Meet the Carpenter". <https://maryrose.org/meet-the-carpenter/>. Used
  for: Carpenter's axe, Adze, Plane, Caulking iron and mallet.
- Mary Rose Trust, "Swords, daggers and staff weapons".
  <https://maryrose.org/the-weaponry-of-the-mary-rose/swords-daggers-and-staff-weapons/>.
  Used for: Bill and pike.
- Mary Rose Trust, "The Pilot". <https://maryrose.org/meet-the-crew/the-pilot/>. Used for:
  Mariner's compass.
- Mary Rose Trust, gimballed compass 81A0071.
  <https://maryrose.org/artefacts/gimballed-compass>. Used for: Mariner's compass.
- Medieval London (Fordham University), "Shears".
  <https://medievallondon.ace.fordham.edu/items/show/470>. Used for: Iron shears.
- Medieval London (Fordham University), spade blade from the Museum of London.
  <https://medievallondon.ace.fordham.edu/items/show/499>. Used for: Spade iron.
- Medievalists.net, "13th century rune stick discovered in Denmark".
  <https://www.medievalists.net/2015/05/13th-century-rune-stick-discovered-in-denmark/>.
  Used for: Rune stick.
- Medievalists.net, report of the University of Reading excavation at Lyminge.
  <https://www.medievalists.net/2011/04/08/7th-century-plough-discovery-redraws-map-of-rural-england-by-400-years/>.
  Used for: Heavy plough.
- Museum of Archaeology and Anthropology, Cambridge, drum 2008.118.
  <https://collections.maa.cam.ac.uk/objects/553931/>. Used for: Shaman's drum.
- MyLearning (Leeds Museums and Galleries), "A Cropping Table".
  <https://mylearning.org/resources/a-cropping-table>. Used for: Cropping shears and
  teasels.
- Oxfordshire Museum Service, folding coin balance.
  <https://portal.oxfordshire.gov.uk/content/publicnet/museums/oikos/oikos95.htm>. Used for:
  Coin balance.
- Pitt Rivers Museum, "Analysing the English Collections": harvesting tools.
  <https://england.prm.ox.ac.uk/englishness-harvesting-tools.html>. Used for: Sickle, Flail.
- Portable Antiquities Scheme, "Seal matrices" recording guide.
  <https://finds.org.uk/counties/findsrecordingguides/seal-matrices/>. Used for: Seal
  matrix.
- Portable Antiquities Scheme, "Spindle Whorls" recording guide.
  <https://finds.org.uk/counties/findsrecordingguides/spindle-whorls/>. Used for: Spindle,
  whorl and distaff.
- Portable Antiquities Scheme, "Spurs" recording guide.
  <https://finds.org.uk/counties/findsrecordingguides/spurs/>. Used for: Spurs.
- Princeton University Library, "The Philosophers' Glass".
  <https://dpul.princeton.edu/alchemy/feature/the-philosophers-glass>. Used for: Athanor.
- Revolutionary Players (History West Midlands, objects of Bewdley Museum), "Charcoal
  Burning Tools (4)". <https://revolutionaryplayers.org.uk/?p=2386>. Used for: Corrack and
  long shovel.
- Ripon City Council, "The Hornblower".
  <https://www.riponcity.gov.uk/history/the-hornblower>. Used for: Watchman's horn.
- Royal College of Physicians, "The urine wheel".
  <https://history.rcp.ac.uk/blog/urine-wheel>. Used for: Uroscopy flask.
- Royal Museums Greenwich, sounding lead and line NAV0676.
  <https://rmg.co.uk/collections/objects/rmgc-object-42888>. Used for: Sounding lead and
  line.
- Royal Museums Greenwich, Valentia astrolabe NAV0022.
  <https://www.rmg.co.uk/collections/objects/rmgc-object-42234>. Used for: Mariner's
  astrolabe.
- Salt Association, "Salt During Our Early History".
  <https://saltassociation.co.uk/education/salt-history/early-history/>. Used for: Lead salt
  pan.
- Schlenkerla brewery, historical depictions of brewing.
  <https://www.schlenkerla.de/biergeschichte/brauerstern/html/brauerdarstellungene.html>.
  Used for: Brewing kettle.
- Science Museum Group, "Cropping Shears, Bradford Industrial Museum".
  <https://collection.sciencemuseumgroup.org.uk/objects/co8647038/cropping-shears-bradford-industrial-museum>.
  Used for: Cropping shears and teasels.
- Science Museum Group, alchemical furnace A634411.
  <https://collection.sciencemuseumgroup.org.uk/objects/co115035/alchemical-furnace>. Used
  for: Athanor.
- Science Museum Group, bleeding bowl.
  <https://collection.sciencemuseumgroup.org.uk/objects/co136049/bleeding-bowl-england-1671-1730>.
  Used for: Fleam and bleeding bowl.
- Science Museum Group, caltrop A201111.
  <https://collection.sciencemuseumgroup.org.uk/objects/co143784/caltrop-probably-english-13th-17th-century-caltrops>.
  Used for: Caltrop.
- Science Museum Group, copy of a 1600s wooden leper clapper A635021.
  <https://collection.sciencemuseumgroup.org.uk/objects/co147425/copy-of-a-1600s-wooden-leper-clapper-england>.
  Used for: Leper's clapper.
- Science Museum Group, Exchequer standard bushel measure of King Henry VII.
  <https://collection.sciencemuseumgroup.org.uk/objects/co57551/exchequer-standard-bushel-measure-of-king-henry-vii-1495-1497>.
  Used for: Standard bushel.
- Science Museum Group, Exchequer standard yard of King Henry VII.
  <https://collection.sciencemuseumgroup.org.uk/objects/co57527/exchequer-standard-yard-of-king-henry-vii-1495-1497>.
  Used for: Standard yard.
- Science Museum Group, fleams with case.
  <https://collection.sciencemuseumgroup.org.uk/objects/co142081/fleams-with-case>. Used
  for: Fleam and bleeding bowl.
- Science Museum Group, glass urinal or matula.
  <https://collection.sciencemuseumgroup.org.uk/objects/co147557/glass-urinal-urinal>. Used
  for: Uroscopy flask.
- Science Museum Group, Islamic alembic 1978-219.
  <https://collection.sciencemuseumgroup.org.uk/objects/co19063/islamic-alembic>. Used for:
  Alembic and cucurbit.
- Science Museum Group, lantern A45674.
  <https://collection.sciencemuseumgroup.org.uk/objects/co155085/lantern-tin-plate-with-horn-reflectors>.
  Used for: Horn lantern.
- Science Museum Group, medieval Exchequer tally sticks 1952-431.
  <https://collection.sciencemuseumgroup.org.uk/objects/co60506/medieval-exchequer-tally-sticks-exchequer-tallies>.
  Used for: Tally stick.
- Science Museum Group, medieval iron axe head 1925-970/1.
  <https://collection.sciencemuseumgroup.org.uk/objects/co8629101/medieval-iron-axe-head>.
  Used for: Felling axe.
- Science Museum Group, medieval keys 1934-376.
  <https://collection.sciencemuseumgroup.org.uk/objects/co50332/1-14th-century-bronze-key-key-hardware>.
  Used for: Picklock.
- Science Museum Group, pole axe for animal slaughter 1987-1040.
  <https://collection.sciencemuseumgroup.org.uk/objects/co144553/pole-axe-for-animal-slaughter-pole-axes>.
  Used for: Pole axe.
- Science Museum Group, thrift and mill-bills 1966-395.
  <https://collection.sciencemuseumgroup.org.uk/objects/co154189/collecton-of-thrift-and-mill-bill>.
  Used for: Mill bill and thrift.
- Science Museum Group, watchmaker's drill turn with whalebone bow 1910-16.
  <https://collection.sciencemuseumgroup.org.uk/objects/co46413/watchmakers-drill-turn-with-whalebone-bow>.
  Used for: Turns and bow.
- Science Museum Group, wooden model of a 1400s alms bowl A635019.
  <https://collection.sciencemuseumgroup.org.uk/objects/co90594/wooden-model-of-a-1400s-alms-bowl-england-1917-1920>.
  Used for: Alms bowl.
- Shakespeare Birthplace Trust, "Shakespeare in 100 Objects: Bee Skep".
  <https://shakespeare.org.uk/explore-shakespeare/blogs/shakespeare-100-objects-bee-skep>.
  Used for: Bee skep.
- Shakespeare Birthplace Trust, "Stratford's Ceremonial Maces".
  <https://www.shakespeare.org.uk/explore-shakespeare/blogs/stratfords-ceremonial-maces>.
  Used for: Mace of office.
- The British Museum through Google Arts & Culture, "Coin dies and silver penny of Edward
  III".
  <https://artsandculture.google.com/asset/coin-dies-and-silver-penny-of-edward-iii/YwFXV9FNXIJG0Q>.
  Used for: Coin dies.
- The British Museum through Google Arts & Culture, "Dr Dee's magic".
  <https://artsandculture.google.com/asset/dr-dee-s-magic/jgH1be-D72po1Q>. Used for: Seals
  of wax, Shew-stone.
- The British Museum through Google Arts & Culture, brass geomantic instrument.
  <https://artsandculture.google.com/asset/brass-geomantic-instrument-made-by-muhammad-ibn-khutlukh-al-mawsili/1QEEJWn3U-nLWg>.
  Used for: Geomantic instrument.
- the collection page with both miniatures.
  <https://medievallondon.ace.fordham.edu/collections/show/155>. Used for: Iron shears.
- The Fishing Museum Online (Medlar Press), "The Fine Art of Trapping Eels" by A. Herd.
  <https://fishingmuseum.org.uk.medlarpress.com/eel_bucks.html>. Used for: Eel trap.
- The Goldsmiths' Company, "Though the coin be fair".
  <https://www.thegoldsmiths.co.uk/hallmarking-stories/though-the-coin-be-fair-hallmarking-and-language>.
  Used for: Touchstone and touch needles.
- The Metropolitan Museum of Art through Google Arts & Culture, "Boar Spear".
  <https://artsandculture.google.com/asset/boar-spear/lgFfdo-_tDZslQ>. Used for: Boar spear.
- Tools and Trades History Society, "The Art of the Scythe" (S. Tallents, 1947).
  <https://www.taths.org.uk/tools-trades/articles/51-the-art-of-the-scythe>. Used for:
  Scythe.
- Trinity College Dublin, on recreating 16th-century beer.
  <https://www.tcd.ie/news_events/articles/2024/five-things-our-research-uncovered-when-we-recreated-16th-century-beer-and-barrels/>.
  Used for: Mash tun and masher, Brewing kettle.
- UCLA, History of the Book, "Scribes: Prickers".
  <https://pages.gseis.ucla.edu/faculty/drucker/HoB_Site/HoB_Scribes_Exhibit/HoB_Scribes_Prickers.html>.
  Used for: Pricker.
- UCLA, History of the Book, "Scribes: Quill Pens".
  <https://pages.gseis.ucla.edu/faculty/drucker/HoB_Site/HoB_Scribes_Exhibit/HoB_Scribes_Quill_Pens.html>.
  Used for: Quill pen and penknife.
- Victoria and Albert Museum, "An Assembly of Lovers", mirror back 220-1867.
  <https://collections.vam.ac.uk/item/O106330/an-assembly-of-lovers-mirror-back-unknown>.
  Used for: Mirror in an ivory case.
- Victoria and Albert Museum, "Ramsey Abbey censer and incense boat".
  <https://www.vam.ac.uk/blog/museum-life/twenty-objects-twenty-years-ramsey-abbey-censer-and-incense-boat>.
  Used for: Censer and incense boat.
- Victoria and Albert Museum, "Stained glass: an introduction".
  <https://www.vam.ac.uk/articles/stained-glass-an-introduction>. Used for: Grozing iron.
- Victoria and Albert Museum, ark W.54-1950.
  <https://collections.vam.ac.uk/item/O130488/ark-unknown/>. Used for: Kneading trough.
- Victoria and Albert Museum, chalice.
  <https://collections.vam.ac.uk/item/O108995/chalice-unknown>. Used for: Chalice.
- Victoria and Albert Museum, comb 88-1892.
  <https://collections.vam.ac.uk/item/O125617/comb-unknown>. Used for: Boxwood comb.
- Victoria and Albert Museum, engraving burin. <https://api.vam.ac.uk/v2/object/O126830>.
  Used for: Graver.
- Victoria and Albert Museum, falcon's hood T.244-1960.
  <https://collections.vam.ac.uk/item/O318742/falcons-hood-unknown/>. Used for: Falcon's
  hood.
- Victoria and Albert Museum, horn 8035-1862.
  <https://collections.vam.ac.uk/item/O313757/horn-unknown>. Used for: Hunting horn.
- Victoria and Albert Museum, mortar.
  <https://collections.vam.ac.uk/item/O129007/mortar-unknown/>. Used for: Mortar and pestle.
- Victoria and Albert Museum, pomander 918:1, 2-1853.
  <https://collections.vam.ac.uk/item/O105969/pomander-unknown/>. Used for: Pomander.
- Victoria and Albert Museum, touch needles M.49-1971.
  <https://api.vam.ac.uk/v2/object/O324088>. Used for: Touchstone and touch needles.
- Viking Ship Museum, Roskilde, "The smith".
  <https://www.vikingeskibsmuseet.dk/en/professions/education/all-about-the-viking-ship/skibets-haandvaerkere/smeden>.
  Used for: Clench nails and roves.
- Weald & Downland Living Museum, "Charcoal burning".
  <https://www.wealddown.co.uk/museum-news/charcoal-burning/>. Used for: Charcoal clamp.
- Weald & Downland Living Museum, "Sawpit Shed from Sheffield Park".
  <https://www.wealddown.co.uk/buildings/sawpit-shed-from-sheffield-park/>. Used for:
  Two-man saw.
- Wellcome Collection, folding almanac MS.40.
  <https://wellcomecollection.org/works/bvgyb226>. Used for: Folding almanac.
- Whipple Museum of the History of Science, Cambridge, "Armillary Spheres and Teaching
  Astronomy". <https://www.whipplemuseum.cam.ac.uk/node/59>. Used for: Armillary sphere.
- Whipple Museum of the History of Science, Cambridge, "The Parts of an Astrolabe".
  <https://www.whipplemuseum.cam.ac.uk/explore-whipple-collections/astronomy/medieval-astrolabe/parts-astrolabe>.
  Used for: Astrolabe.
- Wikipedia, "Draw plate". <https://en.wikipedia.org/wiki/Draw_plate>. Used for: Draw plate.
- Wikipedia, "Wheelbarrow" (citing A. L. Matthies, "The Medieval Wheelbarrow", Technology
  and Culture 32, 1991). <https://en.wikipedia.org/wiki/Wheelbarrow>. Used for: Wheelbarrow.
- Winterthur Museum, Dominy Collections, "Screw Plates and Screw Taps".
  <https://dominycollections.winterthur.org/tools-of-the-trade/clockmaking-shop/screw-plate/>.
  Used for: Screw plate.
- World Archaeology 22, "Hessian crucibles reveals alchemists' secrets".
  <https://www.world-archaeology.com/world/europe/germany/hessian-crucibles-reveals-alchemists-secrets/>.
  Used for: Hessian crucible.
