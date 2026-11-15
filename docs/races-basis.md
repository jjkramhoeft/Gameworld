# Races basis

The basis of the default race list: 28 races, each with a description, a history and the
facts needed to place it in the biomes and to draw vocations and factions from it.

- 7 human races (1 to 7), each built on a real historical people. There is no plain
  human race.
- 5 classic races (8 to 12): Elves, Dwarves, Halflings, Gnomes and Orcs.
- 16 new near-human races (13 to 28), built with the framework of
  [races-and-worldbuilding.md](races-and-worldbuilding.md).

These 28 races are the default list of [society-generation.md](society-generation.md)
Appendix A, decided on 2026-11-15. The ids, the names, the habitat weights, the `common`
values and the height modifiers here are those of the Parameter Set. This document adds
what the Parameter Set does not hold: the looks, the history, the way of life, the
vocations and the factions of each race. That part is material for sections 4 to 9 of
society-generation.md and binds nothing yet.
[adding-a-race.md](adding-a-race.md) lists what must change with the list.

The biome weights are judgement. The shares they give are measured on 1,079 regions (see
Measured shares), and the weights have not been tuned afterwards.

Five races of the first version of this document were left out of the list and are not
described here: Tinefolk, Hushfolk, Longsleepers, Eyrie folk and Lineminds. The Catfolk
were added from the earlier default list, with a new look.

## How to read an entry

Every entry has the same parts.

| Part | What it holds |
|---|---|
| Fact table | The basis or origin, the looks in one line, the lifespan. A new race also has its two anchor traits: one **visible marker** and one **hidden trait** |
| Appearance | What a portrait must show |
| History and lore | Where the race comes from and what it tells about itself |
| Way of life | Housing, food, family, law and custom. For a new race: how the hidden trait shapes them |
| Relations | How the race stands to the human races and to its neighbours |
| Biomes | Habitat weights as in society-generation.md Appendix A: **native (3)**, **good (2)**, **tolerated (1)**. A biome that is not named is 0. Then the height modifier, `common`, and what the race wants that a race cannot ask for yet |
| Vocations | Work the race is known for. A source for the roles of persons |
| Factions | The kinds of group the race forms. A source for the factions of a region |

**Names.** Every name is a working name. The human races have a name from their land or
way of life, with the real basis in the fact table. The names of the new races are plain
English compounds. None has been searched for earlier use.

**No fixed places.** The world is unbounded and generated, so the lore names kinds of
places (a salt flat, a fjord coast, a river plain) and ages, never one location. Any
region can hold a hall, a hold or a court of a race.

**Ids.** The ids are those of the Parameter Set. The five classic races and the Catfolk
keep the ids of the earlier default list. `halfling` stands for the Hobbits that were
asked for: "Hobbit" is a protected name of the Tolkien estate, so the race is called
Halfling, and its lore is written fresh.

## The four ages

The histories share four ages, so that they fit together.

1. **The Dawn.** Before anyone counted years. The first peoples spread over the land and
   grew apart. Elves and Dwarves remember it, the Elves in person through three or four
   lives laid end to end.
2. **The Old Realms.** An age of great states ruled by sorcerer-kings, who reshaped
   rivers, forests and peoples. Four of the new races were made in their workshops.
   Their roads, laws and ruins lie under most of the settled land.
3. **The Ashen Years.** Seven years when ash hid the sun. The first three had no summer
   at all. Harvests failed everywhere, the Old Realms broke, and whole peoples moved.
   Many races date their present way of life from these years.
4. **The Scattered Age.** Now. No power reaches further than a few weeks' travel, and a
   kingdom is the land one court can ride across.

## The list

| # | Race | Id | Group | Basis or origin | Home biomes |
|---|---|---|---|---|---|
| 1 | Northmen | `northmen` | Human | Scandinavian | Boreal forest, Temperate mixed forest |
| 2 | Midlanders | `midlanders` | Human | European | Temperate grassland, Temperate broadleaf forest |
| 3 | Saltroad folk | `saltroadFolk` | Human | African (brown) | Hot desert, Thorn scrub |
| 4 | Sunlanders | `sunlanders` | Human | African (black) | Savanna, Tropical dry forest |
| 5 | Grass riders | `grassRiders` | Human | Mongol | Cold steppe, Temperate grassland |
| 6 | Rainkeepers | `rainkeepers` | Human | Indian | Tropical dry forest |
| 7 | Canal folk | `canalFolk` | Human | Imperial Chinese | Temperate broadleaf forest, Temperate rain forest |
| 8 | Elves | `elf` | Classic | First people | Temperate forests |
| 9 | Dwarves | `dwarf` | Classic | First people | High land of the cold and temperate belts |
| 10 | Halflings | `halfling` | Classic | Divergent evolution | Temperate grassland, Woodland & shrubland |
| 11 | Gnomes | `gnome` | Classic | Divergent evolution | Temperate broadleaf forest |
| 12 | Orcs | `orc` | Classic | Divergent evolution | Cold steppe |
| 13 | Hoarkin | `hoarkin` | New | Divergent evolution | Ice & snow |
| 14 | Hearthbloods | `hearthbloods` | New | Magical shaping | Tundra |
| 15 | Widegaze | `widegaze` | New | Divergent evolution | Cold steppe, Temperate grassland |
| 16 | Amberbloods | `amberbloods` | New | Magical shaping | Boreal forest, Temperate mixed forest |
| 17 | Brinefolk | `brinefolk` | New | Convergent evolution | Cold desert |
| 18 | Duskreaders | `duskreaders` | New | Divergent evolution | Hot desert |
| 19 | Noonfolk | `noonfolk` | New | Artificial creation | Savanna, Thorn scrub |
| 20 | Boughwalkers | `boughwalkers` | New | Divergent evolution | Tropical rain forest |
| 21 | Shoalfolk | `shoalfolk` | New | Divergent evolution | Mangrove |
| 22 | Swiftborn | `swiftborn` | New | Artificial creation | Woodland & shrubland |
| 23 | Flinchless | `flinchless` | New | Artificial creation | Temperate grassland and farmland |
| 24 | Lanternkin | `lanternkin` | New | Magical shaping | Wet forests, and towns everywhere |
| 25 | Stillfaces | `stillfaces` | New | Convergent evolution | Temperate mixed forest |
| 26 | Twofold | `twofold` | New | Divine shaping | Tropical dry forest |
| 27 | Bitterbloods | `bitterbloods` | New | Artificial creation | Tropical rain forest, Mangrove |
| 28 | Catfolk | `catfolk` | New | Divine shaping | Savanna |

---

# Human races

Plain humans. They differ in homeland, way of life, build and colouring, and in nothing
else: any two can have children, and a child raised by another people grows up as one of
them. Each is built on a real historical people, but none is that people. Every
description gives a range of looks, not a type.

## 1. Northmen (`northmen`)

> Farmers of the cold forest coast who took to the sea when the summers failed, and never quite came home.

| | |
|---|---|
| **Group** | Human |
| **Basis** | Scandinavian: the Norse of the Viking Age |
| **Looks** | Tall and big-boned, fair to ruddy, hair flaxen to red-brown |
| **Lifespan** | About 60 years |

**Appearance.** Tall, long in the leg and heavy in the shoulder. The skin is fair and
burns before it browns. Hair runs from flaxen through red to light brown and is worn
long and combed daily. Men braid their beards. Eyes are blue, grey or green. They dress
in wool and linen under a fur-lined cloak pinned at one shoulder, and wear their wealth
as silver arm-rings that are cut up when a payment falls due.

**History and lore.** Before the Ashen Years the Northmen were forest farmers and
fishers at the cold edge of the ploughed world, living on rye, barley, cattle and
herring. They call the Ashen Years the Great Winter: three winters with no summer
between them, and four more years of bad hay. A farm that fed ten now fed four. The
younger sons took the boats that had been built for fishing and went to find out who
still had grain. Some traded furs, amber and walrus ivory for it. Some took it. Most did
both on the same voyage, and the choice depended on how well the town was walled.

The hunger ended, and the sea-road did not. Their shallow clinker-built ships can cross
open water, run up a river and be dragged over a watershed, so Northmen turned up far
inland as traders, settlers of empty coasts, and hired guards of kings who trusted no
one born nearby.

They hold that the world is the upturned hull of a ship, built by the first Shipwright:
the sky is its planking and the stars are the rivets. One day it will be righted and
launched, and those who died well will row it. A person who dies in bed can still earn a
bench by being remembered, which is why they carve stones for kin who died abroad and
why a good poet eats at the high table.

**Way of life.** They live on scattered farmsteads, not in villages: a longhouse with a
turf roof, people at one end and cattle at the other. The free farmer is the backbone.
Above him is the jarl, who leads in war and must give gifts to keep his following. Below
him is the thrall, taken on a raid or born to it. Women hold the keys, run the farm
while the ships are out and can divorce by declaring it before witnesses.

No king stands above the law. Free people meet at the assembly, the Thing, where a
law-speaker recites a third of the law from memory each year and cases are heard in the
open. A killing can be paid for with silver, by a fixed scale. The heaviest sentence is
outlawry: the outlaw is outside the law, and anyone may kill him. The year has two
halves. In summer they sow, sail and come home for the harvest. In winter they weave,
carve, mend gear and tell stories.

**Relations.** To the Midlanders they are raiders in bad years and the best customers
in good ones, and more than one Midlander port began as a Northmen winter camp. They buy
ivory from the Hoarkin, and they honour Dwarves as the only smiths better than their
own. Elves make them uneasy: a people that needs no poets to be remembered.

**Biomes.** Native (3): Boreal forest, Temperate mixed forest. Good (2): Tundra,
Temperate rain forest. Tolerated (1): Ice & snow, Cold steppe, Temperate grassland,
Temperate broadleaf forest. Height: no modifier. Common: 3. Also wants: a coast or a
navigable river, more than any biome.

**Vocations.** Shipwright, sailor, pilot, raider, trader, free farmer, herring fisher,
whaler, trapper, tar burner, smith, rune carver, poet, law-speaker, sworn guard abroad.

**Factions.** A jarldom. A Thing district of free farmers with no lord. A ship
fellowship that owns a hull in shares. A king's household troop. A trading town under
its own law. A sworn guard in the pay of a foreign court.

## 2. Midlanders (`midlanders`)

> Plough farmers, knights and townsmen living in the shell of an Old Realm, each kingdom sure that it is the heir.

| | |
|---|---|
| **Group** | Human |
| **Basis** | European: the Latin West of the high Middle Ages |
| **Looks** | The most varied of the human races: fair to olive, any hair |
| **Lifespan** | About 60 years |

**Appearance.** Middling in height and build, and mixed in colouring: skin from fair to
olive, hair from blond to black, straight or curled, eyes of any colour. A crowd of
Midlanders looks like several peoples, which it once was. Rank shows in the clothes.
Peasants wear undyed wool and wooden shoes. Townsfolk wear dyed cloth cut to the fashion
of their guild. The nobility wear fur trim, long sleeves and a coat of arms.

**History and lore.** The Midlanders are the people of the old provinces. When an Old
Realm ruled the temperate plains, their ancestors were its farmers, soldiers and clerks,
drawn from many conquered tribes and taught one script and one law. When the Realm broke
in the Ashen Years they stayed where they were. The roads were still there, and the
bridges, the boundary stones and the habit of writing things down. The sorcery was gone.
What grew over the ruins was smaller and tougher: a lord with a wooden tower and a
dozen horsemen, who protected the villages in his sight and took a share of their
harvest for it.

Everything since has been built up from that bargain. Lords swore to greater lords, and
the greatest called themselves kings. Each Midlander kingdom claims to be the true
successor of the Realm and keeps a little of its ceremony: a crown, an anointing, a
court language no peasant speaks. Their histories are full of the wish to put the Realm
back together and the wars fought over who would hold it.

The Midlanders are also the people of the heavy plough. Eight oxen and an iron share
turn the wet clay that lighter ploughs cannot, and that opened the forest lowlands to
grain. Their saying is that land unploughed is land unfinished. The Elves have another
word for it.

**Way of life.** Most live in villages of twenty to a hundred households around a
temple and a mill. The fields lie open in long strips, worked in a three-year turn of
winter grain, spring grain and fallow. Most villagers hold their land from a lord and
owe him labour or rent. A minority are free. Above the village stands the castle, and
between the castles the walled town. Town air makes a person free after a year and a
day. Craft guilds govern the towns: they fix prices, train apprentices and bury their
members.

A third power is the Temple, whose clergy are the only people sure to read. It keeps
the calendar, runs the schools and hospitals, owns a great deal of land and can shame a
king. Its monasteries drain marshes, copy books and brew. Midlanders build in stone when
they can afford it and compete in the height of their temple towers.

**Relations.** They share the best farmland with the Halflings, who are tenants,
neighbours and now and then a county of their own. They fear the Northmen from the sea
and the Grass riders from the plain, and hire both. Dwarves sell them iron. Gnomes live
in their towns as clockmakers and jewellers. They are the human race the new races most
often live among, and the one most likely to make laws about them.

**Biomes.** Native (3): Temperate grassland, Temperate broadleaf forest. Good (2):
Woodland & shrubland, Temperate mixed forest, Temperate rain forest. Tolerated (1):
Cold steppe, Boreal forest. Height: no modifier. Common: 5. Also wants: a river for the
mill and deep soil for the plough.

**Vocations.** Ploughman, shepherd, miller, brewer, vintner, mason, carpenter, weaver,
fuller, smith, merchant, clerk, priest, monk, knight, man-at-arms, bailiff, miner.

**Factions.** A kingdom. A barony or county held from a king. A free town, or a league
of towns. A craft guild. A monastery or a monastic order. An order of knights under a
vow. A school of masters and students. A village commune that holds its own charter.

## 3. Saltroad folk (`saltroadFolk`)

> The brown-skinned people of desert, oasis and dry highland, who carry salt one way and gold the other and kept the books when everyone else burned theirs.

| | |
|---|---|
| **Group** | Human |
| **Basis** | African (brown): the peoples of North Africa, the Sahara, the Sahel edge and the Horn |
| **Looks** | Lean and long-limbed, light brown to deep brown, dark wavy or curled hair |
| **Lifespan** | About 60 years |

**Appearance.** Lean, fine-boned and long in the limb, built to shed heat. The skin runs
from light brown at the cooler coasts to a deep red-brown in the far desert and the
highlands. Hair is black or dark brown, wavy to tightly curled. Eyes are dark brown,
now and then green or amber. They wear loose robes in layers and a long cloth wound as
turban and veil. Among the desert clans it is the men who veil their faces, and a man
of standing is never seen without it.

**History and lore.** The Saltroad folk say their cities are older than the Old Realms,
and the ruins bear them out. Along the one great river of a desert country their
ancestors built in stone before anyone else did, and counted years by the river's flood.
When the sorcerer-kings rose elsewhere, the desert was too wide to be worth conquering
and too hard to cross without a guide. That has been the fortune of the Saltroad folk
ever since: they live on land nobody else can use, and they own the way across it.

The way is the salt road. South of the desert lie the gold fields of the Sunlanders,
where salt is scarce enough to be weighed against gold. North of it are the coasts that
want gold. Between them are forty days of sand and a chain of wells that only the desert
clans can find. A caravan of a thousand camels carries slabs of salt south and comes
back with gold, ivory and kola nuts, and every oasis on the route takes its fee.

In the Ashen Years the desert suffered least, since it had no harvest to lose. Scholars
fled to the oasis towns with their books, and the towns have been places of learning
since. A Saltroad library is a family's treasure, walled into the house and passed
down. They follow one unseen god, whose law is written, and whose judges are the
scholars. In the dry highlands an older rite survives, with priest-kings and temples
cut down into the living rock.

**Way of life.** They live in three ways. The townsfolk build in mud brick around a
well and a market: flat roofs, narrow shaded lanes, palm gardens watered by tunnels that
bring water from the hills. Water is shared by the hour and the shares are inherited.
The nomads keep camels and goats, live in tents of woven hair and move by a yearly round
that every clan knows. They guide the caravans, guard them, or raid the ones that hired
another clan. The highlanders plough terraces of grain and coffee on cool plateaus.

Hospitality is law: a guest is safe for three days, even an enemy. Descent decides
rank, and a clan remembers its line for twenty generations. Trade runs on trust and
paper. A merchant's letter of credit is honoured two months' journey away, because a
house that breaks its word once never trades again.

**Relations.** With the Sunlanders they have the oldest partnership in the world, and
the oldest quarrel, over who sets the price. They trade with the Brinefolk for salt and
hire Duskreaders as night guides. Midlanders know them as the source of gold, paper,
numbers and medicine. Grass riders are their match in open country and their rivals for
the trade of the cold deserts.

**Biomes.** Native (3): Hot desert, Thorn scrub. Good (2): Cold desert, Woodland &
shrubland, Savanna. Tolerated (1): Temperate grassland, Tropical dry forest. Height: no
modifier. Common: 3. Also wants: a river or a well in the desert, and a place on a
trade route.

**Vocations.** Caravan master, guide, camel breeder, well digger, date gardener,
scholar, judge, scribe, astronomer, physician, merchant banker, leatherworker, mud-brick
mason, incense gatherer, coffee grower, veiled raider.

**Factions.** A caravan company. An oasis emirate. A confederation of desert clans. A
city of scholars ruled by its judges. A river kingdom. A highland priest-king and his
rock temples. A merchant house with partners in a dozen towns.

## 4. Sunlanders (`sunlanders`)

> The black-skinned people of the long grass and the deep forest, first to smelt iron, rich in gold, cattle and remembered words.

| | |
|---|---|
| **Group** | Human |
| **Basis** | African (black): the peoples of West, Central, East and Southern Africa |
| **Looks** | From tall and slender to short and compact, dark brown to blue-black, tightly coiled hair |
| **Lifespan** | About 60 years |

**Appearance.** The Sunlanders vary more in build than any other human race. The cattle
herders of the open grass are among the tallest people in the world, slender and
long-legged. The farmers of the forest edge are of middling height and strongly built,
and the hunters of the deep forest are short and light. The skin runs from dark brown
to a black with a blue sheen. Hair is black and tightly coiled, and is braided, shaved
in patterns or built up with clay and beads to show age, rank and home. Dress is wrapped
cotton in strong dyes, indigo first among them.

**History and lore.** The Sunlanders hold that they were the first to draw iron from
stone, and nobody has proof against it. Their smiths smelted in tall clay furnaces while
the Old Realms were still working bronze. A Sunlander smith is more than a craftsman:
he commands fire, makes the hoe and the spear, and is feared and honoured like a priest.
In many of their kingdoms the first king is remembered as a smith.

Iron cleared the forest edge and armed the grassland, and gold paid for the rest. The
gold fields lie in Sunlander country, washed from river gravel and dug from shallow
pits. The kings who held them grew rich enough that one, on a journey, gave away so
much gold that its price fell for a generation. That is the tale the Saltroad folk
tell, and the Sunlanders do not deny it.

The Old Realms never ruled them. A fly of the wet forest kills horses and cattle with a
sleeping sickness, and no mounted army could pass it. So the grassland built empires on
cavalry, and the forest built walled city-states that no cavalry could reach. In the
Ashen Years the rains failed, the herders moved south and east, and the kingdoms they
met or founded on the way make up the Sunlander lands today.

Little of this is written down. It is kept by the praise-singers, a hereditary class
who hold the genealogy and deeds of every great family in memory and sing them on
demand. A king without a praise-singer has no past.

**Way of life.** In the grassland the village is a ring of round houses of clay and
thatch around a cattle fold fenced with cut thorn. Millet and sorghum are the grains.
Cattle are bride-price, blood-price and the measure of a man. Boys are initiated
together and form an age-set that stays together for life, as herders, then warriors,
then elders. In the forest zone the staple is yam, the towns are large, with walls and
moats of earth, and the crafts are of the highest order: cast bronze and brass, carved
ivory, woven and dyed cloth.

Kingship is sacred but not absolute. A council of title-holders can depose a king, and
the queen mother holds a court of her own. Masked societies keep the law in the king's
shadow, and their masks speak with the voice of the ancestors. Messages cross a kingdom
in an hour, by drums that copy the tones of speech.

**Relations.** The Saltroad folk are their partners and rivals in the gold trade. The
Noonfolk share their grassland and sleep through its nights, which the herders guard
for a fee. Boughwalkers and Twofold are forest neighbours and trading partners. Orcs
raid the dry edge and serve in the cavalry empires as heavy foot.

**Biomes.** Native (3): Savanna, Tropical dry forest. Good (2): Thorn scrub, Tropical
rain forest. Tolerated (1): Hot desert, Mangrove. Height: no modifier. Common: 4. Also
wants: gold and iron in the bedrock, and a river.

**Vocations.** Smith, iron smelter, gold washer, cattle herder, millet or yam farmer,
praise-singer, drummer, bronze caster, ivory carver, weaver, indigo dyer, hunter,
cavalryman, river trader, diviner, title-holder.

**Factions.** A grassland empire with tributary kings. A walled forest city-state. A
cattle clan. An age-regiment. A caste of smiths. A brotherhood of hunters. A masked
society that keeps the law. The court of a queen mother.

## 5. Grass riders (`grassRiders`)

> Herders of the open steppe who live in the saddle, obey one law, and every few generations ride out together under a single khan.

| | |
|---|---|
| **Group** | Human |
| **Basis** | Mongol: the horse nomads of the Eurasian steppe |
| **Looks** | Compact and deep-chested, weathered tan to bronze, straight black hair |
| **Lifespan** | About 55 years |

**Appearance.** Compact, deep-chested and bow-legged from a life on horseback, with
broad faces, high cheekbones and narrow dark eyes creased against wind and glare. The
skin is tan to bronze and weathered early. Hair is straight and black, and men shave
the crown and braid what is left behind the ears. Both sexes wear a long wrapped coat
belted at the waist, felt boots and a fur hat, and carry a bow case and a knife.

**History and lore.** The steppe cannot be ploughed. It has grass, and what turns grass
into a living is the horse, the sheep and the will to move. The Grass riders have lived
that way since the Dawn: a few families and their herds, shifting camp four or five
times a year, each clan at feud or at marriage with the next.

Their history has one shape, which repeats. For generations the clans raid each other
and the farmers at the steppe's edge, and no one outside pays them much attention. Then
a leader appears who can make the clans obey. He breaks up the old clans and sorts the
fighting men into tens, hundreds and thousands under officers chosen for merit. He
gives one written law for everyone. In a few years he has an army in which every man
has three remounts and can shoot from the saddle at a gallop, and no farming kingdom
can stand against it. The Old Realms paid tribute to such khans. The Ashen Years, which
killed the herds and drove the clans outward together, broke the realms that still
stood.

Then the khan dies, his heirs divide the conquests, and within a few generations each
is ruling farmers from a palace and is a steppe lord in name only. The clans at home
begin again. Every Grass rider knows the shape and believes the next khan will be the
one whose realm holds.

They worship the Sky, which is everywhere and needs no temple. A shaman speaks to the
lesser spirits for a fee. They care little what conquered people believe, tax every
faith alike and enjoy setting priests to argue before the khan.

**Way of life.** Home is a round tent of felt on a wooden lattice, which two people can
raise in an hour and a cart can carry. The door faces south, the hearth is in the
middle, and there are rules for where each person sits. They eat meat and milk: dried
curd, butter, boiled mutton and fermented mare's milk. Grain is a luxury taken in trade
or tribute.

Children ride before they walk well. Women manage the camp and herds, drive the carts
and advise in council, and a khan's widow may rule until the heirs meet. The law is
short and harsh: theft of a horse is death, and so is desertion, or fouling running
water. The same law protects envoys and merchants without exception. A relay of post
riders, with fresh horses kept ready a day apart, carries the khan's orders faster than
any news travels elsewhere. The great assembly of all the clans chooses a khan and
decides on war.

**Relations.** Farmers fear them: Midlanders to one side, Canal folk to the other. The
Canal folk build walls against them and are conquered anyway about once in a dynasty.
The Widegaze share the steppe as allies, valued as lookouts. Orcs are rivals for the
same grass and are taken into the hordes as foot soldiers. Merchants of every race
love a strong khan, since under one a caravan can cross the plain without a guard.

**Biomes.** Native (3): Cold steppe, Temperate grassland. Good (2): Cold desert.
Tolerated (1): Tundra, Boreal forest, Woodland & shrubland, Hot desert. Height: no
modifier. Common: 3. Also wants: open, level ground. Forest and marsh stop a horse army.

**Vocations.** Herder, horse breaker, mounted archer, bowyer, felt maker, cart driver,
post rider, scout, falconer, wrestler, shaman, caravan guard, tax collector over
conquered farmers.

**Factions.** A clan and its herds. A horde under a khan. The khan's sworn guard. The
great assembly. The post relay. An overlordship that takes tribute from farming
kingdoms. A band of landless riders for hire.

## 6. Rainkeepers (`rainkeepers`)

> Farmers of the monsoon lands who catch a season's rain in ten thousand reservoirs and are born into the work they will do.

| | |
|---|---|
| **Group** | Human |
| **Basis** | Indian: the kingdoms of the Indian subcontinent |
| **Looks** | Slight to middling, light brown to very dark brown, black hair |
| **Lifespan** | About 60 years |

**Appearance.** Slight to middling in height and fine-featured. The skin runs from a
light wheat-brown to very dark brown, often within one town. Hair is black, straight or
wavy, oiled, and worn long by both sexes. Eyes are large and dark. Dress is unsewn
cotton: a length of cloth wrapped and pleated at the waist for a man, a longer one drawn
over the shoulder for a woman, in colours that tell the wearer's community. A mark of
coloured paste on the brow shows which god the wearer follows.

**History and lore.** The monsoon lands get a year's rain in three months and none in
the other nine. The Rainkeepers became a people when they learned to hold on to it.
They dammed a shallow valley with an earth bank to make a reservoir, then another below
it to catch the overflow, and another below that. A chain of reservoirs feeds rice
fields through the dry season, and the kingdom that builds the chains can feed cities.
Their kings are judged by their dams, and the proudest boast in their inscriptions is
that not one drop reaches the sea without first serving the people.

The Old Realms traded with them and did not conquer them. Their own empires rose and
fell on the rain: a run of bad monsoons broke more dynasties than any enemy. The Ashen
Years stopped the rain for three seasons. The cities emptied and the forest took the
canals. What the Rainkeepers rebuilt afterwards was a land of many middling kingdoms,
each around a temple town.

Their thought runs to great spans of time. They hold that the world is made, destroyed
and made again in cycles of millions of years, and that a soul is reborn through many
lives, higher or lower by its deeds, until it wins release. They have a thousand gods,
whom their teachers call so many faces of one. Their epics are the longest poems in the
world. Their scholars gave the world its numerals, the zero and the game of kings.

**Way of life.** A Rainkeeper is born into a community that has one occupation: potter,
weaver, oil presser, goldsmith, priest, washerman, soldier. One marries inside it, eats
with it, and is judged by its elders. The communities are ranked, with priests and
rulers at the top and those who handle the dead at the bottom, and the ranking is
enforced by rules about food and touch. It gives everyone a place and a trade, and it
is very hard to leave. Those who leave become wandering ascetics, who own nothing, and
whom even kings greet standing.

The village is ruled by a council of five elders, and the king's officers only collect
the tax. The staples are rice in the wet lands and millet in the dry, with pulses,
clarified butter and spices. Many do not eat meat, and the cow is not killed. The great
buildings are temples, towers of carved stone that hold land, employ hundreds and act
as bank, school and granary. Kings ride to war on elephants.

**Relations.** Their merchant guilds sail the warm seas and keep trading posts in every
port, so they are known far beyond their own land for cotton cloth, pepper, gems and
steel. The Twofold live beside them in the dry forest and fit awkwardly into their
ranks. Boughwalkers sell them dyes. Canal folk are their rivals in trade and their
pupils in philosophy, and Saltroad merchants carry their goods west.

**Biomes.** Native (3): Tropical dry forest. Good (2): Savanna, Tropical rain forest,
Mangrove. Tolerated (1): Thorn scrub, Temperate broadleaf forest. Height: no modifier.
Common: 4. Also wants: a river and gently sloping ground for the reservoir chains.

**Vocations.** Rice farmer, reservoir keeper, cotton weaver, dyer, spice grower,
elephant driver, temple sculptor, priest, astrologer, mathematician, physician,
jeweller, steel maker, sea merchant, dancer, ascetic.

**Factions.** A temple town and its lands. A king's court. A merchant guild with its
own ships and guards. An occupational community under its elders. An order of ascetics.
An elephant corps. A village council of five.

## 7. Canal folk (`canalFolk`)

> The people of the dike, the paddy and the written examination, who serve an empire that has fallen many times and has never agreed that it ended.

| | |
|---|---|
| **Group** | Human |
| **Basis** | Imperial Chinese: the dynasties of imperial China |
| **Looks** | Middling and slender, light tan to golden-brown, straight black hair |
| **Lifespan** | About 60 years |

**Appearance.** Middling in height, slender in the south and sturdier in the north. The
skin runs from pale tan to golden-brown. Hair is straight and black, never cut, and
worn in a topknot under a cap. Eyes are dark and narrow-lidded. Dress is a robe crossed
left over right and tied with a sash. Farmers wear hemp or cotton and a wide straw
hat. Officials wear silk in the colour of their grade, with a square badge on the chest
that shows their rank to anyone who can read it.

**History and lore.** The Canal folk live on river plains that flood. Their first hero
was an engineer who tamed the flood by digging channels to lead the water away, after
his father had failed with dams. The lesson shaped everything after: water is managed
by many people working to one plan for many years, and that needs someone to keep the
plan. So they have the oldest continuous state in the world, built to maintain dikes,
canals and granaries.

They call that state the Realm, and they hold that there is only one. Heaven grants its
mandate to a ruling house for as long as the house rules well. Floods, famines and
rebellions are signs that the mandate has been withdrawn, and whoever then wins the
throne has received it. So a dynasty may fall, and the Realm goes on. The Canal folk
alone among peoples say that their Old Realm never ended. It lost its sorcery in the
Ashen Years and kept its files.

In the Scattered Age the throne is disputed or empty, and each Canal folk state is
ruled by a governor or king who holds it, by his own account, in trust for the emperor
to come. All of them keep the same calendar, script, weights and ritual, so that the
pieces will fit when they are put back together.

Their teaching is about conduct, not gods: the duty of child to parent, subject to
ruler, younger to elder, and the duty of the ruler to deserve it. Ancestors are fed and
informed at the family altar. Other faiths are welcome so long as they do not question
the order.

**Way of life.** The village works flooded rice in the warm south, wheat and millet in
the cooler north, with mulberry trees on the dikes for silkworms and tea on the hills.
Every scrap of land is used, and human labour replaces animals. Families are large,
three generations under one roof, ruled by the eldest man, with the lineage's deeds and
rules kept in an ancestral hall.

The Realm is run by officials chosen by written examination, which is open in
principle to any farmer's son who can master the classics. Few can afford the years of
study, but a village will pool its money to send one clever boy, and his success lifts
them all. An official never serves in his home district. Soldiers and merchants rank
below farmers in theory and live better in practice. The Canal folk make paper, printed
books, porcelain, silk, lacquer, cast iron and the compass, and they consider the people
beyond their borders unfinished.

**Relations.** The Grass riders are the danger their walls face, and the source of
about one dynasty in three. They trade silk and porcelain to the Saltroad folk and the
Rainkeepers. They employ Lanternkin as notaries, register every other race in a named
category, and tax each accordingly.

**Biomes.** Native (3): Temperate broadleaf forest, Temperate rain forest. Good (2):
Temperate grassland, Temperate mixed forest, Tropical dry forest. Tolerated (1): Cold
steppe, Woodland & shrubland, Tropical rain forest, Mangrove. Height: no modifier.
Common: 5. Also wants: a river plain. They are the race that most wants flat land by a
large river.

**Vocations.** Paddy farmer, silk grower, tea grower, potter, canal engineer, boatman,
magistrate, clerk, examiner, tutor, printer, physician, salt commissioner, wall
soldier, merchant, geomancer.

**Factions.** A governor's court. An academy that prepares for the examination. A
lineage with its ancestral hall. A merchant house. A state monopoly of salt or iron. A
guild of canal boatmen. A monastery. A sworn brotherhood that waits for the mandate to
change hands.

---

# Classic races

The five races every reader of fantasy expects. They keep the ids and the biome weights
of the earlier default list. The lore is written for this world.

## 8. Elves (`elf`)

> The long-lived gardeners of the old forest, who remember the Dawn at second hand and are losing a very slow war.

| | |
|---|---|
| **Group** | Classic |
| **Origin** | A first people of the Dawn |
| **Looks** | Tall and slender, ears drawn to a point, ageless faces |
| **Lifespan** | 500 to 600 years, with one or two children in a life |

**Appearance.** A head taller than most humans and lighter in build, with long hands
and a way of standing entirely still. The ears rise to a point. The face is fine-boned
and shows no age after the thirtieth year: an Elf of four hundred has a smooth brow and
old eyes. Colouring follows the forest. Elves of the cold woods are pale and
silver-haired, those of the oak woods brown-haired and green-eyed, those of the warm
forests dark and gold-eyed. They dress in greens, greys and browns that are hard to see
between trees.

**History and lore.** The Elves say the temperate world was once forest from shore to
shore and that they planted a good part of it. It may be so. An Elf thinks nothing of
setting an acorn where a hall will be wanted in two hundred years. In the Dawn they
tended the woods as a farmer tends a field, but on a rotation of centuries. They thinned
and coppiced, moved saplings, bred trees for nuts and fruit, and grew their halls from
living trunks.

Then humans came with fire and the plough. The Elves did not at first understand that a
people could clear land and never let it grow back. By the time of the Old Realms they
understood it well. The Realms cut whole forests for fleets and furnaces, and the Elves
fought them in a string of wars that each lasted one human generation and that the
Elves count as a single war, not yet over. They also taught the first sorcerer-kings.
Elves do not like to be reminded of it.

In the Ashen Years the old forest lived on its stored strength while the grain fields
died. Some elven houses opened their stores of nuts and dried fruit to the starving
farmers at the forest edge. Others closed the paths. That choice still divides them
into the Open houses and the Closed, and it still decides how each forest is regarded
by its neighbours.

They are few and grow fewer. A couple may have two children in five centuries, and a
war that kills fifty is a disaster that takes three hundred years to mend.

**Way of life.** Elves live in the oldest and tallest forest, which grows in sheltered
low ground, so their halls lie below the first hills. A hall is grown, not built: trees
trained over generations into pillars and roofs, with woven walls between. They do not
fell a healthy tree. They eat nuts, fruit, mushrooms, honey, game taken sparingly, and
bread of chestnut flour. From a flower only they can grow they draw a sap that keeps
food from spoiling, the one thing they sell that everyone wants.

A long life changes the worth of everything. An Elf learns four or five crafts in turn
and is a master of each. A decision that a human council would reach in a week takes an
elven council ten years, and is then kept for a thousand. A grudge can outlast the
kingdom that caused it. They keep treaties to the letter and are baffled when the other
side's grandchildren do not, since to the Elf it was signed recently, by people they
knew.

**Relations.** To Midlanders and Canal folk they are the reason the forest is
dangerous to cut. Halflings they like. Dwarves they quarrel with over charcoal.
Amberbloods share their woods, and each calls the other the lesser friend of trees. A
human court that can get an elven tutor or envoy treasures the connection, and does not
quite trust it.

**Biomes.** Native (3): Temperate mixed forest, Temperate broadleaf forest, Temperate
rain forest. Good (2): Boreal forest, Tropical rain forest. Tolerated (1): Temperate
grassland, Woodland & shrubland, Tropical dry forest. Height: `(200, 1), (200, 0)`, only
below 200 m. Common: 2. Also wants: old forest far from towns, and a high Supernatural
value.

**Vocations.** Forester, tree shaper, bowyer, archer, ranger of the forest edge,
herbalist, healer, harper, weaver, vintner, lore keeper, envoy, tutor at a human court.

**Factions.** A forest realm under a lord or lady and a council. A house, with a policy
older than its neighbours' kingdoms. The rangers who hold the forest edge. An Open house
that trades and teaches. A Closed house that admits no one. An embassy in a human city.

## 9. Dwarves (`dwarf`)

> Miners and smiths of the high rock, who keep every contract and every grudge in writing, and seven years of grain behind the gate.

| | |
|---|---|
| **Group** | Classic |
| **Origin** | A first people of the Dawn |
| **Looks** | Short, very broad, bearded |
| **Lifespan** | About 250 years |

**Appearance.** Four to four and a half feet tall and nearly as broad, with thick
wrists, big hands and a heavy brow. The skin is ruddy or brown and seamed like worked
leather. Both men and women grow beards, which are braided and ringed to show clan,
craft and standing, so outsiders often cannot tell the sexes apart and Dwarves see no
reason to help them. They see well in very little light. They dress in leather and
thick wool and are rarely without a tool on the belt.

**History and lore.** The Dwarves hold that the first of them were not born. They woke
inside the rock and dug their way out, and the urge to dig is the memory of it. They
have a sense no other race has: a Dwarf feels the depth, the direction and the grain of
the stone around him, and is never lost underground.

Through the Dawn and the Old Realms they went deeper. A hold begins as a mine and
becomes a city: halls, forges, cisterns and tombs cut into a mountain behind one gate.
The sorcerer-kings never ruled them, since a hold cannot be taken except by starving
it, and the Realms wanted dwarven steel more than dwarven obedience. They paid in
grain. For a thousand years the holds grew rich and stopped keeping fields.

Then the Ashen Years came and the grain carts did not. The Dwarves call this the Hungry
Gate. Holds that had never been breached starved behind their own doors. Others opened
the gate and sent their people out as smiths, masons and soldiers to anyone who could
feed them. The holds that came through drew two lessons, which are now law in all of
them. Every hold keeps seven years of grain in sealed halls. And every hold keeps
treaties with the farmers below it, and honours them to the grain.

**Way of life.** A hold is ruled by a king and a council of the clans. A clan owns a
craft or a seam, and membership comes by birth or by a long adoption. A young Dwarf is
apprenticed for thirty years and becomes an adult on the day a masterwork is accepted.
Contracts are engraved on metal plates and kept in the hall of records, and to break
one is the worst thing a Dwarf can do. Wrongs done to the hold are written in a
separate book, and a debt or a grudge passes to the heirs until it is settled.

They mine iron, copper, tin, silver, gold and gems, and they know which bedrock holds
which. They grow mushrooms, keep goats on the high pastures, make hard cheese and brew
a great deal of beer. Everything else they buy. A hold therefore has a market town at
its gate, where the surface peoples meet the only Dwarves most of them will ever see.

**Relations.** Midlanders, Northmen and Canal folk all buy dwarven iron, and all three
have fought wars over who the nearest hold sells to. The quarrel with the Elves is
about charcoal: a forge eats forests. Gnomes are cousins who went down the hill
instead of into it. Orcs fight them for the passes. Hearthbloods share the cold heights
and hire out to the forges.

**Biomes.** Good (2): Tundra, Boreal forest, Temperate mixed forest. Tolerated (1): Ice
& snow, Cold steppe, Cold desert, Temperate grassland, Woodland & shrubland, Temperate
broadleaf forest, Temperate rain forest. Height: `(200, 0), (200, 1)`, only from 200 m.
Common: 2. Also wants: hill and mountain zones, and a bedrock with ore: fold belt,
granite or volcanic arc.

**Vocations.** Miner, prospector, smith, armourer, mason, engineer, tunnel warden,
brewer, goat herd, records clerk, vault keeper, gate merchant, heavy foot soldier for
hire.

**Factions.** A hold under its king. A clan. A craft guild inside a hold. The gate
market and its wardens. A company of soldiers for hire. A venture sent to open a new
mine. The keepers of the book of grudges.

## 10. Halflings (`halfling`)

> The small folk of garden, orchard and river barge, who came through the famine with full cellars and shared them.

| | |
|---|---|
| **Group** | Classic. The Hobbits that were asked for |
| **Origin** | Divergent evolution: small cousins of the humans |
| **Looks** | The height of a human child, round-faced, curly-haired |
| **Lifespan** | About 90 years |

**Appearance.** Three to three and a half feet tall, built like small adults, not like
children: sturdy, round in the face and inclined to be stout. The hair is thick and
curly, brown or sandy. The eyes are bright and the hands are deft. They walk very
quietly without trying. They have a sense of taste and smell far finer than a human's
and can name the field a cheese came from. They dress in bright waistcoats, wide hats
and good boots, and wear their prosperity on their backs.

**History and lore.** The small folk have always lived in the gaps the big folk left:
river meadows, the hedged corners of the lowlands, the sunny slopes too steep for the
heavy plough. They have no tale of where they came from and no interest in one. Their
history is family history, kept in genealogies that every Halfling can recite to the
seventh cousin.

Their name is a tax record. The registers of an Old Realm entered each of them at half
a head, since they ate half a ration and could carry half a load. The clerks wrote
"halfling" in the margin and the word stuck. Their own word for themselves just means
the Folk.

A Halfling household has always kept a deep cellar, and custom says it should hold
enough to feed the family for seven years, though no one remembers who set the number.
In the Ashen Years the custom paid. Small bodies need little, the cellars were full,
and the Halflings were the one people of the farmland who did not starve. Most of them
fed their human neighbours as well. The neighbours have not forgotten. The charters
that give Halfling districts their own law inside human kingdoms nearly all date from
the first years after the ash.

**Way of life.** They live in villages of low houses with turf roofs, half dug into a
bank for warmth, each with a garden, an orchard, beehives, geese and a pig. They farm
on a small scale and to a very high standard, and they eat six meals a day. They brew,
press, smoke, pickle and bake better than anyone, and their cheeses and ciders travel
far.

A district of about a hundred households, called a Hundred, governs itself. The heads
of the families meet in a moot and elect a reeve for a year. There are no lords and no
standing soldiers, only a watch with slings and staves. Order is kept by gossip: a
Halfling who cheats a neighbour finds that everyone knows by evening and no one will
sell them beer.

Not all of them stay at home. The barge families live on the rivers and carry the
goods of the farmland to the towns. Others keep inns, and on many roads every inn for
a week's travel is kept by Halflings who are cousins to each other and pass news along
the road faster than a rider.

**Relations.** They are tenants and neighbours of the Midlanders and get on with nearly
everyone. Elves are fond of them. Gnomes are friendly rivals at market. They hire
Swiftborn at harvest. The open farmland has no defences, so they fear Orcs and Grass
riders above all, and they pay, to a human lord or to the raiders, for protection.

**Biomes.** Native (3): Temperate grassland, Woodland & shrubland. Good (2): Temperate
mixed forest, Temperate broadleaf forest. Tolerated (1): Cold steppe, Boreal forest,
Temperate rain forest, Savanna, Tropical dry forest, Tropical rain forest. Height: no
modifier. Common: 2. Also wants: flat and lowland zones, a river, and a high Prosperity
value.

**Vocations.** Gardener, orchard keeper, beekeeper, brewer, cider maker, cheesemaker,
baker, cook, innkeeper, miller, bargeman, peddler, tobacco grower, taster, slinger of
the watch, keeper of genealogies.

**Factions.** A Hundred under its elected reeve. A moot of family heads. A great
family with a hall and a hundred cousins. A barge family. A chain of inns along one
road. A village watch. A yearly fair.

## 11. Gnomes (`gnome`)

> Small makers of small exact things, who built the instruments of the sorcerer-kings and have refused a single master ever since.

| | |
|---|---|
| **Group** | Classic |
| **Origin** | Divergent evolution: kin of the Dwarves who stayed in the foothills |
| **Looks** | Three feet tall, slight, large-eyed, long-nosed |
| **Lifespan** | About 200 years |

**Appearance.** About three feet tall and lightly built, with a large head, a long
nose, big ears and very large eyes of a clear grey or brown. Hair goes white early and
is worn in tufts or under a pointed cap. The fingers are long and thin. A Gnome sees
close detail as a human sees through a lens: the facets of a seed, the scratches on a
gear. At thirty paces the same Gnome sees a blur and wears ground glass spectacles to
correct it. They dress in many pockets.

**History and lore.** The Gnomes are hill folk. Where the Dwarves went into the
mountain, the Gnomes dug warrens under the roots of the oak and beech woods in the
foothills, shallow, dry and warm. They were never numerous and never fought anyone if
there was a back door.

Their close sight made them the makers of everything fine. In the Old Realms the
sorcerer-kings prized them above gold. Gnomes ground the lenses, cut the crystals,
engraved the measuring rings and built the clockwork that the sorcery needed, and they
were kept in comfortable workshops that had locks on the outside. Being Gnomes, they
kept a copy of every design.

When the Realms broke, the Gnomes walked out with the drawings and without the masters.
They drew one conclusion from the whole age and made it a rule. No workshop takes more
than a third of its work from one patron. A Gnome will sell to a king, and will not
belong to one.

They went into their warrens in the Ashen Years with mushroom beds and with something
nobody else had thought to save in good order: seed. Sorted, labelled and sealed, the
seed vaults of the warrens resowed a good many fields when the sun came back. Farmers
still buy their seed from Gnomes when they want it true to kind.

**Way of life.** A warren holds a few hundred Gnomes under a council of masters. Each
family is a workshop: gem cutters, clockmakers, locksmiths, lens grinders, engravers,
instrument makers. A maker's design is registered with the college of the warren under
a personal mark, and for fifty years nobody else may copy it. That is the nearest
thing they have to property in land.

Every Gnome also has a life-piece: one work begun at coming of age and added to for
two centuries, never sold and never finished. It may be a clock, a book, a model of a
city or a mechanical bird. At death it goes into the warren's gallery, which is the
only temple a warren has.

They love riddles, puns and practical jokes. Their names grow through life as
nicknames are added. They are sceptical of magic, having seen how it is made, and they
trust what can be measured.

**Relations.** Many live in the towns of Midlanders and Canal folk, in a quarter of
their own with low doors, as jewellers, clockmakers and moneychangers. Dwarves sell
them metal and buy their tools. Halflings share their markets. Lanternkin are their
partners in banking: the Lanternkin sign and the Gnomes build the vault. Rulers court
them, and watch them, because a Gnome's lock can be opened by the Gnome who made it.

**Biomes.** Native (3): Temperate broadleaf forest. Good (2): Boreal forest, Woodland &
shrubland, Temperate mixed forest. Tolerated (1): Tundra, Cold steppe, Cold desert,
Temperate grassland, Temperate rain forest, Thorn scrub, Savanna, Tropical dry forest.
Height: no modifier. Common: 1. Also wants: hill zones, and a town nearby to sell in.

**Vocations.** Gem cutter, jeweller, clockmaker, locksmith, lens grinder, engraver,
instrument maker, assayer, moneychanger, seedsman, grafter, mushroom grower, tinker,
map engraver, riddling tutor.

**Factions.** A warren under its masters. A family workshop. A gnomish quarter in a
town of the big folk. The college that registers makers' marks. A seed vault and its
keepers. A family of travelling tinkers.

## 12. Orcs (`orc`)

> A hard people of the poor land, soldiers of the fallen Realms, who can eat anything and so outlast everyone.

| | |
|---|---|
| **Group** | Classic |
| **Origin** | Divergent evolution: cousins of the humans, shaped by land that feeds few |
| **Looks** | Tall and heavy, grey-green to olive-brown skin, tusks in the lower jaw |
| **Lifespan** | About 45 years. Adult at twelve |

**Appearance.** Six to seven feet tall and heavy in the bone, with long arms, a thick
neck, a low brow and two tusks rising from the lower jaw. The skin is grey-green,
olive or dun brown. Hair is coarse and black and is worn in a crest or a knot. The
eyes are small, yellow or red-brown, and see well at dusk. Scars are left to show and
are sometimes rubbed with ash so that they stand out: an Orc's skin is a record. Dress
is leather, hide and whatever armour was won.

**History and lore.** In the Dawn the Orcs lived on good land too. Humans with ploughs
fed ten on the ground that fed one Orc hunter, and over the generations the Orcs were
pushed outward to the dry plains, the scrub and the badlands, which is where they are
now. The land made them what they are. An Orc can eat carrion, bone, bark and meat a
week dead and take no harm, heals from wounds that would kill a human, and is full
grown at twelve.

The Old Realms saw a use for that. They raised orc legions, the heavy infantry of the
sorcerer-kings, paid them in loot and settled the veterans in forts along the
frontiers. When the Realms fell the garrisons did not go home, because the forts were
home. Many orc holds today stand on the walls of an old frontier fort, and their chiefs
keep the standard of a legion that no state has paid for a thousand years.

They have a name for the Ashen Years, and it is the Lean Feast. When the harvests
failed, the Orcs lived on what the others could not eat. They came out of the poor
land into country emptied by famine and took a good deal of it. The stories that make
them monsters date from then: they arrived when everyone was starving, and they were
not.

**Way of life.** The basic unit is the hearth: a few families who share fire, herds
and food, led by a hearth-mother who owns the tents and the animals. The fighting
strength of several hearths is the warband, led by a chief. A chief is chosen by
challenge and kept by success. One who loses two raids is replaced. Chiefs may be
women, though hearth-mothers seldom want the lesser post.

They herd goats, pigs and tough cattle, hunt, and raid. Nothing is wasted: hide, bone,
sinew and gut all have a use, and the cook pot takes the rest. Law is spoken, sworn
before the hearth fire and remembered by the old. With so short a life, few Orcs see
forty. Those who do are the greybacks, who hold the law and the lore and are obeyed
without argument. The dead are laid out on a hill for the vultures, since the earth
that fed so little should be given something back.

Orcs respect strength, a kept oath and an open hand. They despise haggling and admire
a good enemy.

**Relations.** Everyone has fought them and everyone has hired them. Midlander kings
settle orc bands on their borders, to guard them against other Orcs. Grass riders are
rivals for the same grass and take Orcs into the hordes as foot soldiers, and the
Sunlander empires do the same. Dwarves fight them for the passes. The Widegaze cannot
be ambushed, so Orcs make treaties with them and keep them.

**Biomes.** Native (3): Cold steppe. Good (2): Cold desert, Temperate grassland,
Woodland & shrubland, Hot desert, Thorn scrub, Savanna. Tolerated (1): Tundra, Boreal
forest, Temperate mixed forest, Temperate broadleaf forest, Temperate rain forest,
Tropical dry forest. Height: no modifier. Common: 2. Also wants: poor land, with a low
Prosperity value, and the borderland between kingdoms.

**Vocations.** Warrior, soldier for hire, herder, hunter, scout, smith, butcher,
tanner, bone carver, quarry hand, porter, caravan guard, chief, hearth-mother,
greyback.

**Factions.** A hearth under its hearth-mother. A warband under a chief. A hold in an
old frontier fort. A company for hire. A horde: many warbands under one warlord. A
treaty band that guards a human border. A band of outcasts.

---

# New races

Sixteen near-human races, built with the framework of
[races-and-worldbuilding.md](races-and-worldbuilding.md). Each has one of its four
origins, one visible marker, one hidden trait, a culture that follows from the hidden
trait and a defined standing with the human races. An actor could play any of them
with light make-up, and a Catfolk with ears, a tail and a pair of lenses added.

| Origin | Races |
|---|---|
| Divergent evolution | Hoarkin, Widegaze, Duskreaders, Boughwalkers, Shoalfolk |
| Magical or divine shaping | Hearthbloods, Amberbloods, Lanternkin, Twofold, Catfolk |
| Convergent evolution | Brinefolk, Stillfaces |
| Artificial creation | Noonfolk, Swiftborn, Flinchless, Bitterbloods |

## 13. Hoarkin (`hoarkin`)

> The cold-bodied people of the sea ice, who gave up their own warmth and sleep through the dark half of the year.

| | |
|---|---|
| **Group** | New |
| **Origin** | Divergent evolution |
| **Visible marker** | Blue-grey lips, nails and eyelids. No mist on the breath |
| **Hidden trait** | A cold body. The blood does not freeze, and the body does not try to keep warm |
| **Lifespan** | About 100 years |

**Appearance.** Stocky and short-limbed, with skin as pale as skimmed milk that shades
to blue-grey at the lips, the nails, the eyelids and the tips of the ears. Hair is
white or ash-grey, fine and dense. The eyes are pale grey with a dark rim. A Hoarkin
never shivers and moves slowly and exactly. Their breath makes no cloud in the frost,
and their hand is as cold as a stone from the beach. They wear one layer of sealskin,
against wind and wet. They do not need it against cold.

**History and lore.** The Hoarkin are cousins of the humans who followed the seals out
onto the ice in the Dawn and stayed, for more generations than human history has. A
warm body on the ice burns food all day to stay alive. Over that long time the Hoarkin
body stopped trying. Something in their blood keeps it liquid far below freezing. At
rest the heart beats twenty times a minute. A Hoarkin can sit by a seal's breathing
hole from one tide to the next without moving and without harm, and needs a third of
the food a human does. That is how the ice can feed a people at all.

Their own account is a bargain. The First Mother met Cold on the ice, and Cold was
killing her children. She gave him their fire, and he agreed to stop. So the Hoarkin
have no fire inside them, and they call every other people the Burning: folk who are
alight within, eat without end to feed the flame, and die of it young.

In the Ashen Years the ice spread south and the Hoarkin came with it, further than
they had ever been, onto coasts emptied by famine. They call that time the Wide Years.
When the sun returned the ice drew back and most of them followed it home. A few bands
were stranded on high glaciers, where they still are.

**Way of life.** The year has a waking half and a sleeping half. Through the dark
months a lodge of several families lies down together under the snow. Their hearts
slow to a few beats a minute, and they wake once in each moon to eat and to check the
roof. This is the Long Lying. The bright months are frantic by their standards:
hunting, storing meat, courting and settling every lawsuit of the year at a moot on the
shore.

Houses are built of snow, ice and whalebone and are never heated. The centre of the
house is the meat pit, where another people would have the hearth. Fire is a tool for
working iron and rendering blubber. It is kept outside and watched with distrust. They
eat meat and fat raw and frozen. Cooked food is, to them, dead twice.

Warmth is the danger. A heated room makes a Hoarkin slow, then stupid, then ill, and a
fever that a human would sweat out will kill one. They trade at a cold table set
outside the walls of a port, and do not shake hands. The touch chills the other party
and stings the Hoarkin. Their carving, in walrus ivory, is tiny and very patient, and
their songs last a day.

**Relations.** The Northmen trade iron and grain to them for ivory, oil and pilotage
through the pack ice. Inland humans who have never met one tell of the cold drowned,
who walk out of the sea in winter, and a Hoarkin who strays south is likely to be
taken for a corpse. The Hearthbloods are their opposite, and an old tale makes the two
one people who split the bargain between them. They keep a careful, courteous distance.

**Biomes.** Native (3): Ice & snow. Good (2): Tundra. Tolerated (1): Boreal forest.
Height: no modifier. Common: 1. Also wants: a coast with sea ice. They live on land
that otherwise holds nobody.

**Vocations.** Seal hunter, whaler, ice pilot, ivory carver, blubber renderer, guide
over the frozen sea, sled maker, diver in freezing water, wreck salvager, winter
courier, keeper of an ice cellar in a southern town.

**Factions.** A winter lodge of families that sleep together. A hunting company of the
bright months. The summer moot on the shore. A brotherhood of ice pilots. A cold table
partnership with a port. A stranded glacier band.

## 14. Hearthbloods (`hearthbloods`)

> A people of the high cold who took the fire inside them, and must now feed it twice a day or die.

| | |
|---|---|
| **Group** | New |
| **Origin** | Magical shaping, in the Ashen Years |
| **Visible marker** | Flushed copper skin that steams in cold air |
| **Hidden trait** | Blood at fever heat. They cannot freeze. They need twice the food of a human, and starve in days |
| **Lifespan** | About 45 years |

**Appearance.** Broad, deep-chested and well fleshed, with skin of a flushed copper
red, like someone just in from a hard run. Hair is black or dark red and cropped
short. The eyes are dark with a ruddy light in them. In frost a Hearthblood walks in a
cloud of their own breath, steam rises from the bare shoulders, and snow melts where
they sit. They wear a kilt and an open vest in weather that kills. Their touch is as
hot as a brow in a fever.

**History and lore.** The Hearthbloods are the youngest of the races, and they
remember their making. They were a human people of the high valleys. When the Ashen
Years came, the snow in the passes did not melt for three summers, and they were shut
in. They burned the firewood, then the furniture, then the roof beams. In the third
winter the hearth-keeper of the last hall that still had a fire stood over the last
coals. The hall stood by a warm spring, in a place where the world is thin and things
asked for are sometimes given. She asked that the fire should not go out. Then she
stirred the coals into the last broth, and everyone drank.

Nobody in that hall was ever cold again, and neither were their children. No
Hearthblood hall has had a hearth since. The people are the hearth.

The price showed within the week. A fire has to be fed. A Hearthblood eats twice what
a human eats and is in danger after two days without food. On the fifth day they are
dead. They also burn out early, and forty-five is old.

**Way of life.** A hall is large, low and thick-walled, and has no fire. Forty
sleepers heat it. Guests are welcome, because a guest is warmed at no cost and brings
news. The most important person in a hall is the larder-lord, who keeps the stores,
sets the ration and outranks the war leader, since hunger kills them faster than any
enemy. They herd yaks and goats, hunt, and sell their labour for grain.

Their labour is worth a great deal, since they can work bare-handed where no one else
can work at all. They mine through the winter. They keep the high passes open, dig
travellers out of avalanches and warm the half-frozen back to life with their own
bodies. Many kingdoms pay a station of Hearthbloods at each pass in grain. A sick child
of another race may be laid beside a Hearthblood nurse through a cold night.

They marry at fifteen and are elders at thirty-five. Feasting is their sacrament, and a
fast is a kind of blasphemy. When one dies, the body cools for the first time, and the
mourners say the hearth is out. Heat is the one thing they cannot bear. A lowland
summer strikes them down, and they do not go south.

**Relations.** Dwarves are their neighbours on the heights, hire them for the forges
and sell them grain. Midlanders and Northmen pay them as pass wardens and fear them in
famine years, when hungry halls come down as raiders. The Hoarkin are their opposite.
The same old tale that the Hoarkin tell makes the two races halves of one bargain, and
a handshake between them hurts both.

**Biomes.** Native (3): Tundra. Good (2): Ice & snow, Boreal forest. Tolerated (1):
Cold steppe, Temperate mixed forest. Height: `(100, 0.5), (400, 1)`: half weight below
100 m, full weight from 400 m. Common: 0.5. Also wants: hill and mountain zones, and a
route through a pass.

**Vocations.** Pass warden, avalanche rescuer, winter miner, forge hand, yak herder,
hunter, larder-lord, warming nurse, winter courier, ice cutter, wrestler, raider in a
hungry year.

**Factions.** A hall under its larder-lord. A pass station under contract to a
kingdom. A league of halls that share granaries. A company of forge hands in a dwarven
hold. A raiding band of a famine winter.

## 15. Widegaze (`widegaze`)

> Bar-eyed watchers of the open plain, who see nearly the whole horizon at once and sell what they see to every side.

| | |
|---|---|
| **Group** | New |
| **Origin** | Divergent evolution |
| **Visible marker** | Wide-set golden eyes with a horizontal, bar-shaped pupil |
| **Hidden trait** | Sight almost all the way round, and the smallest movement caught at great range. Close up, poor judgement of depth |
| **Lifespan** | About 60 years |

**Appearance.** Tall, lean and long-necked, with tawny skin and sand-coloured or brown
hair tied back hard, so that it never crosses the eyes. The eyes are set wide, almost
at the corners of the face. The iris is pale gold, and the pupil is a dark bar that
stays level with the ground however the head is tilted. A Widegaze never seems to look
at the person they are talking to, because they have no need to turn towards what they
watch. They wear long plain coats and a brimmed hat cut away at the sides.

**History and lore.** The Widegaze say plainly that their mothers were hunted. In the
Dawn their ancestors lived on open grassland among great cats and running wolves, with
no trees to climb and no caves. Those who saw the grass move lived. After enough
generations of that, they were a people who could not be surprised: able to see in
front, to both sides and most of the way behind in a single glance, and to pick out a
rider at the edge of sight.

They never built a wall. Their safety was to see first and to be gone.

The Old Realms gave them a second trade. A sorcerer-king's frontier was too long to
garrison, so the Realm raised a line of earth mounds across the plain, each within
sight of the next, and set a Widegaze family on every one. A message passed by mirror
flash and smoke from mound to mound crossed the Realm in an afternoon. The Realm fell,
and the line remained. The mounds are now the Widegaze villages, the families still
pass messages along them for a fee, and nobody else has ever learned the code. They
call the line the Watch.

In the Ashen Years they were the first on the plain to see the ash coming, as a brown
wall on the horizon, three days out. They moved their herds early and lost fewer than
anyone.

**Way of life.** A village sits on a mound or a low rise. The houses are round, with
one window slit running the whole way round at eye height, and no inner walls. Since
nobody can avoid seeing what a neighbour does, their manners rest on not remarking on
it, and to mention what one was not shown is the height of rudeness. They keep cattle
and sheep on open range without fences and watch them from the mound.

Forest, fog, narrow streets and high walls distress them. They call such places blind
country and will not enter thick woods. Their close sight is poor, so they buy fine
metalwork and needlework, and their own crafts are bold and large: striped weaving,
earthworks, beacons. One person is always on the high seat at the top of the village,
day and night, by rota. Their law rests on witness. What a Widegaze says they saw is
believed, and to lie about seeing is the one crime for which they cast a person out.

**Relations.** They sell sight to anyone. The Watch warns the Grass riders of a
kingdom's army and warns the kingdom of the Grass riders, at the same price, and has
never been caught in a false report. Its neutrality is its protection, since every
power on the plain needs it. Midlanders and Northmen hire them as sentries, surveyors
and ships' lookouts. Orcs cannot ambush them, so they treat with them instead. Most
humans find the eyes hard to bear, and the saying is that a Widegaze watches you from
the back of the head.

**Biomes.** Native (3): Cold steppe, Temperate grassland. Good (2): Savanna. Tolerated
(1): Cold desert, Thorn scrub. Height: `(300, 1), (500, 0)`: full weight below 300 m,
none from 500 m. Common: 1. Also wants: flat and lowland zones. No forest.

**Vocations.** Lookout, signaller, beacon keeper, range herder, scout, surveyor, ship's
lookout, sentry, witness for hire, spotter for a hunt, weather watcher, star watcher,
road warden.

**Factions.** A mound village. The Watch: the signal line and the families that hold
it. A herding kindred. A sworn company of sentries. A guild of witnesses. A circle of
star watchers.

## 16. Amberbloods (`amberbloods`)

> Forest wardens with resin in their veins, who cannot bleed to death and who end their lives standing as trees.

| | |
|---|---|
| **Group** | New |
| **Origin** | Magical shaping: a compact with the old forest |
| **Visible marker** | Amber-gold eyes, and freckles like drops of resin |
| **Hidden trait** | Blood that sets hard in air. A wound seals at once. In old age the whole body stiffens, roots and becomes a tree |
| **Lifespan** | About 110 years walking, then centuries standing |

**Appearance.** Sturdy and broad-handed, with brown-gold skin scattered with freckles
the colour of clear resin. The eyes are amber and catch the light like a bead held to
the sun. Hair is chestnut, copper or bark-brown. A faint grain, like that of planed
wood, runs along the backs of the hands and down the spine, and deepens with age. A
cut does not bleed. It wells one golden bead, which hardens in a breath. The old move
stiffly, their skin is ridged, and their voices creak.

**History and lore.** In the age of the Old Realms the loggers came north. The Realms
wanted masts and charcoal, and they cut the great cold forests a valley at a time. The
people who lived there were human then: woodcutters, trappers and charcoal burners,
paid by the Realm to fell their own home. One winter they stopped.

Their story says that the forest itself made the offer, through whatever it is that
lives in the oldest stands, where the world is thin. Trees cannot walk and cannot
fight. The forest needed a people who could. It would give them its blood, so that
steel could hardly kill them. In return they would give it their ending: when one of
them could walk no longer, they would come back and stand. The people agreed, drove
the loggers out over thirty years, and have kept the terms since.

The Elves say the compact was offered to them first and that they refused the price.
The Amberbloods say nobody asked the Elves.

Later ages brought a worse enemy than loggers. Their blood, once set, is the finest
varnish and glue there is, staunches any wound, and after some years in the ground
becomes true amber. In the Tapping Wars, human slavers took Amberbloods alive to bleed
them, a cup a week. They do not forgive it, and the trade in blood amber is not quite
dead.

**Way of life.** A village stands in old forest around its Standing Grove, a ring of
trees that were people, each with a name. Council is held sitting among them. Nothing
is heard, but what is decided in the grove is binding. Houses are built of windfall
and thinned timber. No healthy old tree is felled, and a grove tree is never touched.

Fire is their terror, for resin burns. They keep no open hearth and cook in closed
clay stoves. A fire-watcher stands on duty every day of a dry summer. They drink a
great deal of water and tree sap and feel thirst more sharply than hunger. A little
blood, freely given, is sold as wound salve at a high price.

At about a hundred comes the slowing. The Amberblood chooses a place in the grove and
stands there. In one season the feet take root, and in three years there is a tree,
with a burl on the trunk that the family says is the face. The young, before they
settle, go out for some years as archers, healers or guides. These are the Unrooted.

**Relations.** With the Elves they share the forest and a prickly kinship. The
Northmen want timber and tar, and deal with the Amberblood timber courts, which sell
the right to fell a marked tree and no other. Dwarven charcoal is a standing dispute.
Stillfaces are quiet neighbours. Humans are uneasy near a grove, and say that an axe
laid to one of those trees draws blood.

**Biomes.** Native (3): Boreal forest, Temperate mixed forest. Good (2): Temperate
broadleaf forest, Temperate rain forest. Height: no modifier. Common: 1. Also wants:
old forest far from towns, and a high Supernatural value.

**Vocations.** Forester, grove warden, timber judge, resin tapper, varnish maker,
wound healer, bowyer, archer, woodcarver, beekeeper, fire-watcher, tracker, herbalist.

**Factions.** A grove village. A wardenship that patrols against loggers and fire. A
timber court that sells felling rights. A band of the Unrooted. A hidden refuge
founded by those who fled the tappers.

## 17. Brinefolk (`brinefolk`)

> Children of a sea that dried up, who weep salt, drink brine and hold the salt flats in trust until the water comes back.

| | |
|---|---|
| **Group** | New |
| **Origin** | Convergent evolution: not kin to humans |
| **Visible marker** | A crust of white salt at the corners of the eyes, on the lashes and along the hairline |
| **Hidden trait** | They drink brine and sea water, and need salt as others need water. Fresh water alone makes them ill |
| **Lifespan** | About 70 years |

**Appearance.** Spare and smooth-skinned, with a grey-white skin that has the faint
sheen of the inside of a shell. Hair is pale and stiff. The eyes are a very light grey
or green. Beside each eye lies a gland that sheds the salt they take in, so a Brinefolk
weeps brine all day without sorrow. It dries to a white crust on the lashes, in the
corners of the eyes and along the brow, which they brush away or shape into ornament
as fashion bids. They lick their lips often. They dress in pale wrapped linen, and
both sexes line the eyes with dark paint against the glare of the flats.

**History and lore.** The Brinefolk are not cousins of the humans, and their oldest
carvings say so. The basins of the cold desert once held an inland sea. In its shallows
lived a people the carvings show with fins along the arms, who gathered shellfish and
never left the water for long. Over the Dawn the sea shrank. Each generation waded in
shallower, saltier water than the last, and each stood a little straighter. When the
last of the sea had gone into the sky, what was left on the white flats walked on two
legs and looked, to a stranger, like a pale kind of human.

The clerks of the Old Realms listed them as human and taxed them in salt. The Realms
needed salt in quantity, to keep meat for their armies and fish for their cities, and
the Brinefolk had the pans and the patience. They grew rich on it. In the Ashen Years
they grew richer. What little food the world had needed keeping, and for seven years a
block of salt bought its weight in silver. Other peoples remember those years with
grief, and remember who set the price.

They believe the sea will come back. The flats are its bed, kept in trust. So they do
not plough there or build higher than one storey, and they bury their dead packed in
salt, sitting up, so that the dead will be whole and ready when the water returns.

**Way of life.** A Brinefolk town stands at the edge of a salt pan or a bitter lake.
The houses are of salt block and mud brick. Around them lie the evaporation pans, in
squares of white, pink and rust, each owned by a family and worked through the dry
season. Where the bedrock holds rock salt they mine it in galleries. From the soda that
crusts the lake shores they make glass.

They drink from the lake. A well of sweet water is kept for guests and for washing the
salt. Their food is brined, pickled, dried or fermented, and each adult eats half a
handful of salt a day. A Brinefolk kept from salt weakens within a week. They call the
state sweet-sickness, and travellers carry a salt purse against it. They can cross
flats that have no drinkable water, which no other carrier can do.

Salt is their money, pressed into stamped cakes. A bargain is sealed by both parties
eating salt from one dish, and a false weight is punished harder than a theft.

**Relations.** Everyone needs them and few like them. The Saltroad folk are their
oldest partners: the Brinefolk make the salt and the Saltroad folk carry it, and each
is sure the other takes too much. Grass riders buy salt for their herds, and kings tax
it. Humans read the salt tears as grief or as false grief, and have a proverb about
trusting neither. The salted dead, sitting in rows in their halls, are the thing
visitors remember.

**Biomes.** Native (3): Cold desert. Good (2): Hot desert. Tolerated (1): Cold steppe,
Thorn scrub, Mangrove. Height: no modifier. Common: 1. Also wants: a clastic basin
bedrock with rock salt, a river-poor flat zone, or a coast for sea-salt pans.

**Vocations.** Salt boiler, pan master, rock-salt miner, salt carrier, weigh-master,
pickler, fish salter, tanner, embalmer, soda burner, glassmaker, guide over waterless
flats, witness to a salt oath.

**Factions.** A pan town under its pan masters. A salt league that fixes the price
across several regions. A brotherhood of miners. A company of carriers. A tax farm
that collects a king's salt duty. The Keepers of the Bed, who tend the dead and wait
for the sea.

## 18. Duskreaders (`duskreaders`)

> A night people of the hot desert, black-eyed, who see warmth as others see light and have sworn to keep quiet about it.

| | |
|---|---|
| **Group** | New |
| **Origin** | Divergent evolution |
| **Visible marker** | Eyes that are black from lid to lid |
| **Hidden trait** | They see heat. Colour means little to them, and full daylight dazzles |
| **Lifespan** | About 65 years |

**Appearance.** Slight and narrow-built, with deep brown skin that has a grey
undertone. The eyes are large and wholly black, without a visible white or iris, and
glossy as wet stone. By day they wear a visor of smoked horn with a slit in it, or a
veil of dark gauze. Among themselves they dress in thick quilted robes, gloves and
often a mask of stiffened cloth. The robes are not for warmth. They hide the body's
heat, and with it everything the body gives away.

**History and lore.** In the Dawn their ancestors sheltered from the desert day in
deep canyons and caves and came out to hunt at night. Over many generations in the
dark, their eyes learned to see warmth: a hare under a bush, a snake on a rock, the
ground a herd lay on an hour ago. A cool seep under sand shows dark to them, so they
find water where others die.

The same sight shows them a blush, a fever, a quickened pulse in the throat and the
heat of a hand that has just held a hidden knife. The sorcerer-kings of the Old Realms
saw the use in it at once. They set Duskreaders beside their thrones as examiners.
Subjects called them the King's Eyes, since nobody could stand in front of one and
lie calmly. The kings were served, and their examiners were hated. When the Realms fell
the mobs went for them first, and the survivors fled back into the deep desert.

Out of that they made the rule every Duskreader child learns: we read, we do not tell.
A Duskreader says what they have been hired to see and nothing else, ever. An order of
them, the Discreet, takes it as a formal vow.

The Ashen Years they remember differently from everyone else. The dimmed sun made the
desert day mild, and for seven years they walked abroad at noon. They call it the Kind
Time, and they have learned not to say so in company.

**Way of life.** They sleep through the heat in cellars and canyon houses and wake at
sunset. Markets are held at midnight, gardens are watered before dawn, and caravans
move in the dark. Their towns are cut into canyon walls or built with walls an arm
thick.

Their writing is in the walls. A mason sets dark stone that holds the sun's heat beside
pale stone that sheds it, in patterns. For an hour after sunset, as the wall cools
unevenly, the pattern stands out to their eyes: a family's history, a law, a prayer.
Then it fades until the next evening. Each house keeps such a wall, and the temple
walls are read aloud at dusk, which is the whole of their worship. For a quick message
they draw on slate with a heated rod, and the words are gone in minutes.

To show a bare face or bare hands is to show one's feelings. It is done between
spouses, and in court when taking an oath.

**Relations.** The Saltroad folk hire them as night guides and well-finders. With the
Noonfolk they keep the Day-and-Night Pact: the Noonfolk sleep helpless from sunset,
and the Duskreaders are half blind at noon, so each guards the other's sleep. Human
courts hire an examiner to sit in on a hearing, and humans in general suspect them of
seeing through clothes, which in a way they do. Smiths and potters of every race value
them, since a Duskreader can read the heat of a kiln or a blade at a glance.

**Biomes.** Native (3): Hot desert. Good (2): Cold desert, Thorn scrub. Tolerated (1):
Savanna. Height: no modifier. Common: 1. Also wants: rock to cut a town into, and a
place on a caravan route.

**Vocations.** Night guide, well-finder, night watch, tracker, night hunter, examiner
of witnesses, fever reader, midwife, kiln reader, temper judge at a forge, wall mason,
goat herd, trader of the midnight market.

**Factions.** A canyon town under the elders of its houses. A night caravan company. A
Day-and-Night Pact with a Noonfolk settlement. A house of examiners for hire. A guild
of kiln readers. The Discreet.

## 19. Noonfolk (`noonfolk`)

> A people made to live on sunlight so that a king need not feed them, who fall asleep at sunset and have never seen the stars.

| | |
|---|---|
| **Group** | New |
| **Origin** | Artificial creation, in an Old Realm |
| **Visible marker** | A fine green-bronze veining under the skin, like the veins of a leaf. No hair |
| **Hidden trait** | They draw most of their strength from sunlight. At sunset they sleep, and cannot be woken before dawn |
| **Lifespan** | About 70 years |

**Appearance.** Tall, slender and wide in the shoulder. The skin runs from copper-brown
to deep olive and carries a tracery of green-bronze lines, most clearly across the
shoulders, the back and the upper arms. The lines darken through the day and pale
overnight. Noonfolk have no hair at all. The eyes are green or gold, with pupils that
shrink to a point in full sun. They wear a wrap at the hips, and nothing that shades
the back.

**History and lore.** There was a kingdom of the Old Realms in the dry grassland that
suffered one famine after another. Its sorcerer-king saw that most of every harvest
went to feed the people who grew it. He set his makers to breed field hands who would
not need feeding. They took debtors and convicts and worked on them, generation after
generation, with a craft borrowed from the leaf. The result was the Noonfolk, who
laboured from dawn to dusk on a bowl of gruel and a great deal of water.

The makers left two flaws. A Noonfolk falls asleep when the sun goes down, wherever
they are standing, and nothing wakes them before dawn. So they were useless as
soldiers, sentries or house servants. And nobody had planned for a sky without sun.
When the Ashen Years came, the made people starved faster than anyone. Their stomachs
were too small to live on food alone. Nine in ten died. They call it the Dark Hunger.
The kingdom died in the same years, and the Noonfolk who were left walked off the
estates and were not stopped.

They say of it: the king made us to eat light. Nobody asked the Sun. The Sun feeds us
anyway.

**Way of life.** A Noonfolk house is a walled yard open to the sky, with one small
roofed room for stores and one for sleeping. Life is lived outdoors. At midday all work
stops and the whole village stands facing the sun for an hour. Outsiders take this for
worship, and it is a meal. They eat once a day, a little fruit and grain, and drink
through the day, so they settle near wells and rivers in the driest, brightest land.

Because they eat so little of what they grow, a Noonfolk village sells most of its
harvest, and whole districts of hungrier peoples live on Noonfolk grain. The cloudy
season is their lean time. Against another Dark Hunger every village keeps a sealed
granary that is never sold from.

The night is their weakness. They sleep in a barred tower in the middle of the village,
and they hire others to guard it. A night guard's contract is the most solemn document
they have, and the betrayal of one is their oldest horror story. The stars are a thing
they know by report. A few take stimulants, kola or coffee, to stay awake for one
night and see them. This is the Vigil. It is dangerous, and those who have done it are
treated as holy.

Having been made as slaves, they will not keep any. A village is governed by a council
that meets at noon, so that nothing is decided in the dark.

**Relations.** Sunlander herders guard their nights for a fee, and so do Orcs and
Catfolk. The Duskreaders keep the Day-and-Night Pact with them. Saltroad caravans buy
their grain. In a few regions the heirs of the old kingdom still claim Noonfolk as
property under a law no one else recognises, and there the Noonfolk are held or hunted.
Humans find them unsettling at two hours of the day: at noon, when they stand like
statues, and after dusk, when they lie like the dead.

**Biomes.** Native (3): Savanna, Thorn scrub. Good (2): Hot desert. Tolerated (1):
Temperate grassland, Woodland & shrubland, Tropical dry forest. Height: no modifier.
Common: 1. Also wants: a river or a well, and a clear sky. Low precipitation suits
them, as long as there is water on the ground.

**Vocations.** Field hand, grain farmer, orchard keeper, well keeper, brick maker,
quarryman, potter, weaver, grain factor, day courier, keeper of the sundial, speaker
of the noon council. Never sailor, miner or sentry.

**Factions.** A free commune under a noon council. A grain cooperative that sells to a
whole region. A night guard company and the village that hires it. The keepers of the
sealed granary. Those who have kept the Vigil. An estate where Noonfolk are still held.

## 20. Boughwalkers (`boughwalkers`)

> Long-armed people of the forest canopy, who see a colour nobody else can and write in it.

| | |
|---|---|
| **Group** | New |
| **Origin** | Divergent evolution |
| **Visible marker** | Long arms, and fingers and toes a joint longer than a human's |
| **Hidden trait** | They see one more colour than other peoples, beyond violet |
| **Lifespan** | About 65 years |

**Appearance.** Slender and very light, with arms that reach the knee, long hands, and
feet that grip like a second pair of hands. The skin is dark brown to red-brown. Hair
is black and braided flat to the skull, so that nothing can catch on it. The eyes are
large and hazel-green. A Boughwalker has perfect balance and no fear of a drop. They
wear belts, pouches and short wraps of cloth that looks plain brown or white to
everyone else. To their own eyes it blazes with pattern.

**History and lore.** In the Dawn their ancestors went up into the canopy of the wet
forest and stayed. The fruit is there, with the light and the breeze. The forest floor
is dark, flooded half the year, and full of fever and things with teeth. Countless
generations aloft gave them their reach and grip. It also gave them eyes for the work
that matters most up there: telling ripe fruit from unripe, and wholesome from
poisonous, among a thousand shades of green. Flowers, fruit, feathers and some poisons
show a colour beyond violet. Boughwalkers see it. They call it the high colour and
cannot describe it to anyone else.

The Old Realms never reached them. The Ashen Years did. The canopy thinned, the fruit
failed, and for the first time in memory the Boughwalkers came down to the ground to
look for food. They call it the Year on the Floor. Many died of the fevers of the
floor, and those who lived had met the ground peoples and started to trade with them.
One custom has been kept from it. Every young Boughwalker spends a floor-year among
the ground peoples before marrying.

**Way of life.** A village is a cluster of platforms and woven hanging houses thirty
paces up in the great trees, joined by rope bridges and trained vines. High roads of
the same kind run for days through the forest. The ground beneath is not theirs, and
they come down only to the root markets, where ground folk bring iron, salt and cloth.

They eat fruit, nuts, shoots, eggs and honey, and meat makes them ill. They do not
farm. They tend the forest as an orchard that is spread over miles. Each fruit tree
belongs to a family and is inherited through the mother. A family's map of its trees is
its title to wealth.

Their great craft is dye. They make colours in the high range, so that a cloth, a
seal or a line of script can carry a pattern nobody else can see. A Boughwalker mark
on a document cannot be forged by a forger who cannot see it, and merchants and kings
far away pay to have contracts and coin dies marked. They also grade gems, pick out
tainted food and judge when a crop is ripe.

Open country with no branches overhead makes them giddy. The dead are carried to the
highest fork of the family's oldest tree and left for the birds.

**Relations.** The Rainkeepers buy their dyes and feathers, and the Sunlander forest
cities trade with them at the root markets. Bitterbloods live on the floor beneath
them, and the two leave each other alone. Shoalfolk and sea merchants hire them as
riggers, and many ships of the warm seas have a Boughwalker in the top. Midlander
temple builders send for them to work at the top of the towers. Humans find their
cloth drab, and they find human towns ugly: everything in a town clashes in a colour
its builders cannot see.

**Biomes.** Native (3): Tropical rain forest. Good (2): Tropical dry forest. Tolerated
(1): Temperate rain forest, Mangrove. Height: no modifier. Common: 1. Also wants: tall
closed forest, with a high timber potential.

**Vocations.** Fruit gatherer, tree keeper, honey hunter, dyer, painter of hidden
cloth, marker of seals and contracts, spice gatherer, bird catcher, gem grader, food
taster, rigger, topman, steeplejack, bridge builder, runner of the high roads.

**Factions.** A canopy village. A tree lineage, with its map. A dye house and its
secret patterns. A root market, held jointly with a ground people. A company of
riggers that serves the ships of one port. The wardens of a high road.

## 21. Shoalfolk (`shoalfolk`)

> Divers of the tidal shallows, who live on stilts and boats beyond the reach of any land law.

| | |
|---|---|
| **Group** | New |
| **Origin** | Divergent evolution |
| **Visible marker** | A clear inner eyelid that slides across the eye, and nostrils that close to slits |
| **Hidden trait** | They hold their breath for a quarter of an hour and see sharply under water. Their skin must be wetted with salt water every day |
| **Lifespan** | About 60 years |

**Appearance.** Lean, sleek and very deep in the chest. The skin is brown with a grey
or green cast, smooth and faintly oiled. Hair is black, cropped short or braided with
shell. The eyes are large and dark. When a Shoalfolk blinks, a clear membrane flicks
sideways across the eye, and it stays shut under water. The nostrils are slits that
close at will. They wear a loincloth and a knife, and add a belt of lead weights when
diving. Children swim before they walk.

**History and lore.** Their ancestors were shore gatherers on the warm tidal coasts,
where the mangroves leave no land to farm and the sea is full of food. Generations of
diving for it remade them. The lungs and spleen grew and the eyes learned to focus in
water. The skin came to need the sea.

The Old Realms found them useful. Officials set a quota of pearls on every Shoalfolk
village and sent the divers deeper each year to fill it. Many came up bleeding from
the ears, or did not come up. The Shoalfolk answer was to leave the land altogether.
They rebuilt their villages on stilts in the shallows, or on rafts of boats, past the
low-water mark where a king's law stops. No ground, no lord, they say, and they have
kept to it. To this day a Shoalfolk village pays no land tax, because it stands on
none.

In the Ashen Years the fields failed, and the tide still came in twice a day and
brought fish with it. The Shoalfolk ate, and sold dried fish to the coast farmers who
had looked down on them. They took the lesson that the land fails and the sea does
not, and they do not let the farmers forget it.

**Way of life.** A village is fifty houses on mangrove posts over the shallows, joined
by plank walks, or a flotilla of houseboats that moves with the season. Nobody owns
land. A family owns a reef, an oyster bed or a stretch of channel, and guards it. They
reckon time by tide and moon.

They dive for pearls, sponges, coral and shellfish. They salvage wrecks, clean and
mend ships' hulls below the waterline, and pilot strangers through the shoals. They
build their own boats of mangrove timber, tight and light.

A Shoalfolk who goes a week without salt water suffers. The skin cracks, sores open,
and a fever follows. So they are never found far inland, and one who must travel
carries a barrel of brine. A dispute between two of them is settled by the breath:
both parties dive, and the first to surface has lost. The dead are sunk, weighted, on
the family's reef.

**Relations.** They sell pearls and dried fish in the ports of the Rainkeepers,
Sunlanders and Canal folk, and every harbour master wants a few as pilots and divers.
The same knowledge of every creek makes them the smugglers of the coast. A cold-water
branch dives the kelp coasts further north, greased with seal fat, and deals with the
Northmen. Bitterbloods share the mangroves, and the Shoalfolk keep out of their
creeks. Boughwalkers work their rigging, and Brinefolk share the salt-pan coasts.

**Biomes.** Native (3): Mangrove. Good (2): Temperate rain forest. Tolerated (1):
Boreal forest, Woodland & shrubland, Tropical dry forest, Tropical rain forest. Height:
`(20, 1), (80, 0)`: full weight below 20 m, none from 80 m. Common: 1. Also wants: a
coast, and nothing else. The height modifier is a stand-in for tidewater.

**Vocations.** Pearl diver, sponge diver, coral cutter, fisher, shellfish gatherer,
fish drier, pilot, salvager, hull mender, harbour diver, boatbuilder, net maker,
ferryman, smuggler, reef warden.

**Factions.** A stilt village under a tide council. A flotilla of houseboats. A family
that holds a reef. The pilots' guild of a harbour. A salvage company. A ring of
smugglers. A pearl cooperative that sells to one merchant house.

## 22. Swiftborn (`swiftborn`)

> A people made to fill an empty land in one generation, who are grown at seven and old at thirty and spend every day accordingly.

| | |
|---|---|
| **Group** | New |
| **Origin** | Artificial creation, in an Old Realm |
| **Visible marker** | Hair banded in rings of dark and pale, one pair for each year of life |
| **Hidden trait** | They live fast. The heart beats twice as quickly as a human's, the skin is hot, they sleep three hours and eat for two. They are adult at seven and dead at thirty |
| **Lifespan** | About 30 years |

**Appearance.** Small, wiry and quick, with warm olive or brown skin that is hot to the
touch, and a pulse that can be seen in the throat. The eyes are bright and never
still. The hair grows a hand's width in a year, dark in summer and pale in winter, so
that it is ringed like a cut tree and an age can be counted off a braid. It is worn
long for that reason. A Swiftborn of ten is a grown adult. One of twenty-five has the
lined face and white brows of a human of seventy.

**History and lore.** Late in the age of the Old Realms a plague emptied a warm and
fertile province. The vineyards went wild, the olive harvest fell unpicked, and whole
towns stood with their doors open. The sorcerer who governed the province wanted it
full again in his own lifetime. His makers took settlers and quickened them. Their
children were grown in seven years and had large families of their own, and in thirty
years the province was full.

Nobody had asked what would happen in the thirty-first. The first quickened
generation died of old age, all within a few years of each other, and the governor
who had made them watched it. He lived to bury four generations of his people. They
say it broke his mind, and that it was the fifth generation that pulled him down.

The Ashen Years lasted seven years. That is a quarter of a Swiftborn life. A whole
generation was born, grew up and had children without once seeing a clear sun, and is
remembered as the Ash-born. The Swiftborn came through by numbers alone. They died
fast, and were born faster.

**Way of life.** Everything is done quickly. A courtship takes a week, an
apprenticeship a season, and mourning a day. Their arts are those that happen in the
moment: song, dance, theatre and cooking. A Swiftborn town holds a festival every
month. No office is held for longer than a season, and those born in the same year
stand together through life as a year-class, which is the nearest thing they have to
a party.

Their answer to short lives is the relay. Each town has a Long Work, such as a
terraced hillside, a tower or an aqueduct, begun by its founders and handed on at a
ceremony from each generation to the next, with a ring carved for every one. For
anything that must be remembered longer than a century they appoint a trustee from a
long-lived race to hold their charters.

Time is what they value. To waste a Swiftborn's day is an insult, and to make one
wait is a known way of cheating them. A year in prison is a thirtieth of a life, so
they punish by fine or by the lash. Two rules of theirs surprise others: no debt
outlives the year it was made in, and no grudge is inherited.

They work two shifts on three hours' sleep and are the best seasonal labour in the
world.

**Relations.** Midlanders and Halflings hire their harvest gangs, and humans in
general treat them as children, which a magistrate of ten resents. The long-lived can
outwait them: a human lord has only to delay for fifteen years to be dealing with
people who never met the ones he promised. Elves are drawn to them and grieved by
them. An Elf may stand as friend and trustee to twenty generations of one family.
The other made races count them as kin.

**Biomes.** Native (3): Woodland & shrubland. Good (2): Temperate grassland, Temperate
broadleaf forest. Tolerated (1): Temperate mixed forest, Thorn scrub, Savanna. Height:
no modifier. Common: 1. Also wants: fertile land with a high Prosperity value, since
they eat for two.

**Vocations.** Harvest hand, vine dresser, olive picker, builder's labourer, runner,
courier, player, singer, dancer, acrobat, cook, soldier, midwife, market trader,
chronicler, magistrate for a season.

**Factions.** A town and its Long Work. A harvest gang that follows the season. A
company of players. A relay of runners. A free company of soldiers. A year-class. A
trust held for a town by an Elf or a Gnome.

## 23. Flinchless (`flinchless`)

> The descendants of a legion bred to feel no pain, who survive by examining each other at dawn and dusk and have become the world's surgeons.

| | |
|---|---|
| **Group** | New |
| **Origin** | Artificial creation, in an Old Realm |
| **Visible marker** | Pale silver eyes, and a lattice of fine white scars |
| **Hidden trait** | They feel no physical pain. Touch, heat and cold they feel faintly, and hurt not at all |
| **Lifespan** | About 50 years |

**Appearance.** Tall, strong and upright. They were bred from prisoners of many
peoples, so skin and hair are of every kind. The eyes are always the same, a pale
silver-grey, which was the maker's mark. The skin is crossed with thin white scars,
and a missing fingertip or a crooked, badly healed bone is common. What unsettles a
stranger is what is absent. A Flinchless does not blink at a raised hand, does not
favour a hurt leg and does not wince.

**History and lore.** A war-wizard of an Old Realm, remembered only as the Marshal,
wanted foot soldiers who would not break. He bred them from captives and took the pain
out of them. The legion he raised never lost a battle line. It also lost a third of
its strength every winter, to wounds nobody had noticed, frostbite nobody had felt,
and broken feet that had been marched on for a week.

The makers could not put the pain back. They solved it with drill. Every soldier was
paired with another, and at dawn and dusk each pair stripped and examined one another
from scalp to sole. This was called the Looking-over. It worked, and it is the root of
everything the Flinchless are.

When the Realm fell the legion was left without a master. Part of it marched into the
farmland, took up empty holdings and married. Part stayed under arms as companies for
hire. Both parts still exist. They divide on one question: whether the oath sworn to
the Marshal passed to the kingdoms that claim to be the Realm's heirs. The Oathkept
say yes and serve those crowns. The Released say the oath died with him.

**Way of life.** Every Flinchless has a second: the partner of the Looking-over,
chosen in childhood and kept for life. A second is not always a spouse. To live alone
is slow suicide, and a person whose second has died is given another before the
funeral. Twice a day, without fail, the pairs examine each other, and they log what
they find.

Children are the great worry. A small child will chew its own tongue or hold a hand
in the fire to watch. So children wear padded clothes and mittens until the age of
care, at about seven, when they can recite the Rules: test water before washing, count
when near a flame, check the feet every night. Houses have rounded corners and
guarded hearths. Food is eaten lukewarm.

A life of routine measurement made them the best physicians anywhere. They know the
body from having to watch it, and a Flinchless surgeon learns the craft on patients
who can lie awake and describe what the knife is doing. They set bones, pull teeth
and cut for the stone in the towns of every race.

They feel grief, fear and joy in full. Pain of the heart is the only pain they know,
and they treat it as a wound. A mourner is watched by a second, day and night, until
it has closed. They wear out early, since a joint that never hurts is never rested,
and few can still walk well at forty-five.

**Relations.** They live among Midlanders more than anyone, as farmers, soldiers and
surgeons. Kings covet their companies, and the Oathkept give some of them a legal
claim. Their calm under injury horrifies humans, and no duellist will meet one. Since
torture gets nothing from them, they are trusted with secrets. They keep a quiet
fellowship with the Noonfolk, Swiftborn and Bitterbloods, as the children of one
workshop age.

**Biomes.** Good (2): Temperate grassland, Woodland & shrubland, Temperate broadleaf
forest. Tolerated (1): Cold steppe, Temperate mixed forest, Savanna. Height: no
modifier. Common: 0.5. Also wants: nothing of the land. They live where the old
frontier ran and where the Midlanders live now.

**Vocations.** Soldier, shield bearer, bodyguard, drill master, surgeon, bonesetter,
tooth puller, anatomist, nurse, farmer, striker at a forge, firefighter, courier of
secrets.

**Factions.** A legion village under a captain and a chief surgeon. A free company.
The Oathkept in the service of a crown. The Released. A surgeons' hall in a town of
another race. A fellowship of the made races.

## 24. Lanternkin (`lanternkin`)

> Pale people with light in their veins, whose shed blood glows for as long as they live, and who became the notaries, bankers and envoys of the world on the strength of it.

| | |
|---|---|
| **Group** | New |
| **Origin** | Magical shaping, in the lightless wet forest |
| **Visible marker** | Skin thin enough that the veins show as faint lines of light at the wrists, throat and temples |
| **Hidden trait** | The blood glows. A drop sealed in glass shines until the one who gave it dies, and goes dark at that moment, at any distance |
| **Lifespan** | About 70 years |

**Appearance.** Slight and very pale, with skin so thin that it seems almost clear,
and fine hair the colour of ash. The eyes are a light grey-blue. In daylight a
Lanternkin looks only sickly. In shadow a blue-green tracery shows at the wrists, the
throat and the temples, and it brightens and dims with the heartbeat. The sun burns
them quickly, so out of doors they wear a hood, long sleeves and gloves.

**History and lore.** Their ancestors lived on the floor of the deepest wet forest,
where daylight never reaches, among fungi that glow. It was a place where the world is
thin. In a famine in the Dawn they ate the glowing caps, having nothing else. Their
children's blood shone. It has shone in every generation since.

For a long time it was only a way to find each other in the dark. Then a mother
noticed that the drop she had kept in a glass bead from each of her children still
glowed, year after year, until the one bead that went dark on the night her son
drowned three valleys away. That is the lamp, and it changed what the Lanternkin were
for.

The sorcerer-kings were quick to see the use of it. An Old Realm placed a Lanternkin
on the staff of every governor and general, and kept the lamps together in a hall in
the capital, so that the death of an officer's household was known at court the same
hour. Other peoples' blood was tried and did not work.

In the Ashen Years, when the days were dark at noon, villages of every race took
Lanternkin in as the light of the house. Many never went back to the forest. Most
Lanternkin have lived in other peoples' towns ever since.

**Way of life.** A few home villages remain under the thickest canopy, lit by their
own people and by gardens of fungus. The rest live in the towns, in a quarter called a
lamp court. At its centre is the lamp hall: racks of sealed glass vials, each
labelled, each shining. When one goes dark a bell is rung, and the family and the
clients are told the same day.

The trades follow from the blood. A Lanternkin notary seals a contract with a drop of
their own blood under glass. The seal cannot be forged, because nothing else glows.
It is also plainly void once it is dark, so contracts are renewed when a notary dies,
and that keeps the guild in fees. A merchant house sends a Lanternkin with each ship
or caravan and keeps the lamp at home. If the lamp goes out, the house knows the
venture is lost months before any survivor could bring the news, and the first
insurance in the world was written on that. As envoys they are seldom harmed, since
their own court knows of a murder within the hour.

Vials are exchanged at a wedding and one is filled at every birth. To break a
person's lamp is to disown them. The glow also quickens with fear or anger, so
Lanternkin bargain in bright rooms, and those who deal with them ask for the lamps to
be turned down.

**Relations.** Gnomes build their vaults. The Canal folk use them as notaries
throughout the bureaucracy, and Saltroad merchants honour their letters across the
desert. Humans owe them money, and resent it in the usual way. Old superstition holds
that lamp blood lengthens life, and Lanternkin have been hunted for it. Some lords
still keep one as a hostage, to serve as a living lamp.

**Biomes.** Good (2): Temperate rain forest, Tropical dry forest, Tropical rain
forest. Tolerated (1): Temperate grassland, Woodland & shrubland, Temperate mixed
forest, Temperate broadleaf forest, Savanna, Mangrove. Height: no modifier. Common:
0.5. Also wants: a town, a port or a trade route. They follow the Prospects more than
any biome.

**Vocations.** Notary, banker, moneylender, insurer, envoy, lamp on a ship or caravan,
keeper of the lamp hall, clerk, bookkeeper, assayer, vial blower, fungus gardener.

**Factions.** A lamp court and its hall. A guild of notaries. A ring of insurers that
covers the ships of one port. An envoy service. A home village in the forest. A
secret society that frees Lanternkin who are held as living lamps.

## 25. Stillfaces (`stillfaces`)

> A forest people with faces that never move, who say everything by scent and think humans lie with the face and tell the truth with the skin.

| | |
|---|---|
| **Group** | New |
| **Origin** | Convergent evolution: not kin to humans |
| **Visible marker** | A face without expression, and a line of small dark scent glands along the jaw |
| **Hidden trait** | They speak in scent. They give off and read smells that carry mood and meaning, and their nose is as keen as a hound's |
| **Lifespan** | About 75 years |

**Appearance.** Of middling height, sturdy and soft-footed, with tawny-grey skin and
thick hair brindled in brown and grey. The eyes are dark, round and steady. The nose
is broad, with wide nostrils that flare when they attend to something. The face is
well made and entirely still. It does not smile, frown or lift a brow, and the mouth
moves only to eat and to speak. A row of dark spots, like a string of moles, runs
along each side of the jaw and behind the ear. Their voices are level. They stand
close to a stranger and breathe in.

**History and lore.** The Stillfaces say they were in the forest before the upright
peoples came. In their own telling their ancestors went on four feet and spoke
entirely by scent. When the first humans walked into the woods, they stood up to meet
them, out of courtesy, and stayed standing. The face came last, they say, and they
never learned to work it. Scholars who have examined them agree at least that the
bones are arranged differently, and that nothing in them is cousin to a human.

The Old Realms listed them as the mask people and used them as trackers and as
finders of poison at court. They left little mark on them.

The Ashen Years left a deep one. The ash that hid the sun also smothered scent. For
seven years the forest smelled of nothing but cinders. To a Stillface that was like
going deaf and dumb at once. They call those years the Silence. Villages that had
talked across a valley on the wind fell out of touch, families broke, and many went
mad. In that need they learned speech in the mouth, properly, from their human
neighbours. Their spoken language to this day is a borrowed human one, used flatly,
like a tool.

**Way of life.** A village lies in mixed forest, in still hollows where the air
holds. The houses are low and close and never aired, since the scent of a house is
its record. A visitor who knows how can smell yesterday's quarrel in a room. Posts at
the village boundary carry the news, renewed each morning. Messages are sent on
scented cords, which keep for some weeks. For anything longer they must carve or
speak, and they keep poor archives.

Among themselves they cannot hide a feeling. Fear, anger and desire announce
themselves. Their manners therefore concern what a person does with a feeling, never
whether they show it. The one great offence is masking: to cover one's own scent with
another. A perfumed human is, to them, a person shouting nonsense, and a soaped one
is a person with no face.

They compose scents as others compose music, and perform them in closed halls. They
dislike wind, which scatters speech, and smoke, which blinds it. They cook over small
smokeless fires.

**Relations.** Humans read the still face as coldness, sulking or menace, and need
time to learn better. Some young Stillfaces train for years to move their features on
purpose, so as to work in human towns. Their own people call them the Learned Faces
and find the result slightly indecent. The work is there, though. Stillfaces smell
illness before a physician sees it. They find lost children, hidden cargo, truffles
and poison, and can tell one vineyard's wine from the next. Amberbloods are neighbours
whose resin smell they like. Catfolk they find restful: a people whose ears and tail
say what the mouth says. Bitterbloods smell to them of every warning there is.

**Biomes.** Native (3): Temperate mixed forest. Good (2): Boreal forest, Temperate
broadleaf forest. Tolerated (1): Woodland & shrubland, Temperate rain forest. Height:
no modifier. Common: 1. Also wants: forest away from the smoke of large towns.

**Vocations.** Tracker, hunting guide, finder of the lost, truffle hunter, herb
gatherer, perfumer, composer of scents, diagnostician, poison finder at a court,
customs searcher, grader of wine, tea and spice, beekeeper, swineherd.

**Factions.** A village kindred. A lodge of trackers for hire. A perfumers' house that
sells to the towns. A circle of diagnosticians. A customs service under contract to a
kingdom. The Learned Faces.

## 26. Twofold (`twofold`)

> A people who each live the first half of adult life as a man and the second as a woman, and who have built their houses, their law and their god on it.

| | |
|---|---|
| **Group** | New |
| **Origin** | Divine shaping: the answer to an old war |
| **Visible marker** | A thumb-sized patch of skin at the hollow of the throat: pale gold in the first half of life, deep indigo in the second |
| **Hidden trait** | Each of them is male from youth to about thirty, then changes over the course of a year and is female for the rest of life |
| **Lifespan** | About 65 years |

**Appearance.** Of middling height, graceful and even-featured. At any age an outsider
finds it hard to say whether a given Twofold is a man or a woman, and the Twofold are
amused by the effort. The skin is warm brown to dark brown. The hair is black and worn
long by everyone. In the hollow of the throat lies a small patch of skin, pale gold
in the young and deep indigo in the old, which changes during the year of the Turning.
Everyone dresses alike, in wrapped cotton. Only the sash differs: saffron before the
Turning, indigo after it.

**History and lore.** The Twofold tell it like this. Once they were as other peoples
are, and their men and women fell into a war with each other that lasted three
generations. Each half held its own villages. Children were stolen back and forth
across the line. At the end both sides were too few to go on. Both went to the shrine
of the Pair, the god with two faces, and each asked for judgement against the other.

The Pair gave one judgement to both: each of you will be both, and will judge the
other as yourself. Since then no Twofold has been only a man or only a woman, and
there has been nothing left to have that war about.

The Old Realms had no column in their registers for them, and in the end taxed them
by the hearth. In the Ashen Years many Twofold turned early, some at twenty, as if
the body knew that mothers were needed. Their houses refilled within a generation.

**Way of life.** Life has three parts. The young, who are male, are called the Going.
They herd, hunt, trade and fight, travel with the caravans, and marry into a house
that is not their own. The old, who are female, are the Staying. They bear the
children, own the fields, the cattle and the house, and govern. Between the two lies
the Turning, a year spent apart in a house set aside for it. A person in the Turning
belongs to neither side and is sacred. They may not be touched in a feud, and they
serve as the judges and go-betweens of the village.

A house is a chain. A Going husband joins the house of a Staying wife and fathers her
children. When he turns, he becomes a Staying of that same house, and in time takes a
younger husband of her own. Property passes down the chain, and no house is left
without a head. Every mother has been a father. Every elder has once been the
stranger at the door.

The tension among them is between ages. The Going want to go, and have no voice in
the house. The Staying want them home, and do not go to war. A Twofold who dies before
the Turning is mourned as half-lived, which is their word for the worst of fates. A
very few never turn at all. They are called the long Going. They lead the war bands,
and are thought to be either blessed or unfinished.

**Relations.** The Rainkeepers live beside them and cannot place them in their ranks
of birth, which trouble the Twofold not at all. The Sunlanders trade with the
companies of the Going. Other peoples call on a Twofold elder to judge a quarrel
between husband and wife, since nobody else can claim to have been both, and their
midwives and physicians are trusted for the same reason. Human merchants are thrown,
at least once, by returning after ten years to find a trading partner changed.

**Biomes.** Native (3): Tropical dry forest. Good (2): Savanna, Tropical rain forest.
Tolerated (1): Thorn scrub, Mangrove. Height: no modifier. Common: 1. Also wants:
nothing beyond the biome.

**Vocations.** Of the Going: herder, hunter, caravan trader, pedlar, soldier, sailor,
messenger. Of the Turning: judge, go-between. Of the Staying: farmer, weaver, head of
a house, priest of the Pair, midwife, physician, teacher, keeper of the house's
wealth.

**Factions.** A house and its chain. A company of the Going, drawn from several
houses. The court of the Turning. A temple of the Pair. A council of the Staying. A
war band under one of the long Going.

## 27. Bitterbloods (`bitterbloods`)

> Bred as the poison tasters of a court that ruled by poison, they became poisonous themselves, fled to the swamps, and now sell the cure.

| | |
|---|---|
| **Group** | New |
| **Origin** | Artificial creation, in an Old Realm |
| **Visible marker** | Vivid mottling in blue and yellow on the throat, shoulders and forearms |
| **Hidden trait** | Their blood, sweat and spittle are poisonous to others. They are proof against nearly every poison, and against the fevers of the swamp |
| **Lifespan** | About 55 years |

**Appearance.** Lean and smooth-skinned, with a dark olive-brown skin broken by
patches of bright colour: sky blue, sulphur yellow, sometimes orange, across the
throat, the shoulders and the outer arms. The patches are brightest in those whose
blood is strongest, and are worn uncovered with pride at home. Hair is black and
slicked back with oil. The eyes are dark with a yellow ring. Among other peoples a
Bitterblood wears gloves and a coat of wax on the skin, and carries a cup of their
own.

**History and lore.** One of the Old Realms lay in the wet tropics, and its court
ruled by poison. Every heir came to the throne over the bodies of the others. The
dynasty wanted tasters who could not be killed at the table. Over forty generations
the court alchemists raised a caste of servants on rising doses of every venom, root
and mineral in the kingdom, and worked the tolerance into the blood, so that it was
passed to the children.

They did it too well. The tasters became proof against poison, and then they became
poison. A king died of a cup that a taster had sipped from first. A prince died of a
kiss. The makers had given the caste its colours so that anyone at court could tell a
taster at sight, as a livery worn in the skin. The court now read those colours as a
threat, and ordered the caste destroyed.

The tasters fled into the swamp forest and the mangroves, where the fevers killed the
soldiers sent after them and did not touch the fugitives. The dynasty starved in the
Ashen Years. The Bitterbloods did not, because they can eat what poisons everyone
else: bitter roots raw, toadstools, puffer fish, the frogs of the forest floor. No one
has ever competed with them for their food.

**Way of life.** A village stands on a mound or on stilts in the deep swamp, where
others take fever in a week. Mosquitoes that bite a Bitterblood die, and leeches drop
off. They keep poison gardens, and pens of snakes and frogs. From these they make the
things the world wants: antidotes, fever bark, sleeping draughts, salves, fish poison
for the rivers and arrow poison for hunters. Their teaching is that the dose makes
the medicine, and they are the best apothecaries there are.

Touch is the difficulty. A bare handshake raises a rash on an outsider, a shared cup
sickens, and blood in a wound can kill. So they have the courtesy of gloves, and to
touch an outsider bare-handed without warning is an assault in their law. Each
village's blood has its own blend. Even two Bitterbloods of different villages must
exchange a drop a day for a month before they can safely share a cup. That exchange
is their wedding rite, and they call it the mixing. An outsider can marry in after
ten years of small doses, and the few who have done it are called the Seasoned.

Nobody raids them.

**Relations.** The ports of the Rainkeepers and Sunlanders buy their medicines, and
kings hire them as tasters. A Bitterblood cannot die of a poisoned dish, and can name
what is in it. The same kings ask for the other service too. Most villages refuse it,
and an order called the Clean Hands swears to sell the cure and never the poison.
Some do sell. The Shoalfolk keep out of their creeks, and the Boughwalkers stay
overhead. Humans tell stories of the poison bride. The other made races count them as
kin, and shake hands with gloves on.

**Biomes.** Native (3): Tropical rain forest, Mangrove. Tolerated (1): Temperate rain
forest, Tropical dry forest. Height: no modifier. Common: 0.5. Also wants: wet, flat
ground, a river mouth or a swamp.

**Vocations.** Apothecary, antidote maker, poison gardener, keeper of snakes and
frogs, gatherer of fever bark, physician of fevers, taster at a court, swamp guide,
maker of arrow and fish poison, maker of insect smoke, and the poisoner whom most
villages disown.

**Factions.** A swamp village and its blend. An apothecaries' house that sells to the
ports. The office of a court taster. The Clean Hands. A hidden school of poisoners.
The Seasoned.

## 28. Catfolk (`catfolk`)

> A people with the ears and the tail of a cat and the body of a hunter, who cannot eat grain and so are trusted to guard it.

| | |
|---|---|
| **Group** | New |
| **Origin** | Divine shaping |
| **Visible marker** | Furred cat ears set high on the head, a furred cat tail, and eyes with the slit pupil of a cat |
| **Hidden trait** | A hunter's body. They live on meat and fish and cannot live on grain. They sleep in short naps by day and by night, and are most awake at dusk and at dawn |
| **Lifespan** | About 65 years |

**Appearance.** Apart from the ears, the tail and the eyes, a Catfolk looks like a human.
The skin is human skin, bare and smooth, with no fur on the face or the body. There are
no whiskers, no muzzle and no claws: the nose, the mouth, the teeth, the hands and the
nails are human. They are lean, a little under middling height, and light on their
feet.

The ears are a cat's ears: upright, pointed and furred, set high on the head. A Catfolk
has no other ears, and the sides of the head are smooth under the hair. The ears turn
toward a sound. The tail is as long as the leg, furred all over, and never quite still.
Ears and tail show what the person feels, and a Catfolk cannot stop them.

The colouring follows the cat. The skin runs through the colours of a cat's coat: cream,
sand, tawny gold, russet, warm brown, smoke grey and near black. The hair is human hair,
worn at any length, in black, brown, sand, ginger, grey or white, and often in two of
them, streaked or patched. The fur of the ears and the tail matches the hair, sometimes
with a darker tip or with rings. The eyes are amber, gold, copper, green or pale blue,
brighter than a human's, and now and then one of each. The pupil is a cat's: an upright
slit in daylight, wide and round in the dark. They wear light, loose clothes cut to
leave the tail free, and nothing that covers the ears.

**History and lore.** The Catfolk were human once. They were the granary keepers of the
Old Realms. Every city of that age lived on its stores, and a guild of wardens watched
them, with cats to keep down the mice. Their patron was a small goddess of the
storehouse, the Small Huntress, who has a cat's head in the old carvings.

In the Ashen Years the stores were all a city had. In the second year without a summer
the mice and the rats bred in the granaries, and the wardens starved beside the grain
they guarded. Their story is that they went to the Small Huntress and asked for two
things: to keep the stores from the vermin, and to keep them from themselves. She gave
both. The children born that winter had her ears and her tail. They heard a mouse
through a wall, and they could not eat bread. The Catfolk call them the First Litter.

A warden who cannot eat grain can be trusted with it. By the Catfolk account, the cities
that came through the Ashen Years with their stores whole were the ones that kept such
wardens, and the charters date from then. A grain town gives its Catfolk meat and fish,
and they keep the town's stores. This is the Warden's Portion, and many towns of the
Scattered Age still pay it.

Not all of them stayed. When the Old Realms broke, most wardens had no city left to
serve. They went out into the open grassland, where the game was, and became hunters.
The two kinds, the wards of the towns and the bands of the grassland, still count
themselves one people and marry between them.

**Way of life.** A Catfolk eats meat, fish and eggs, and very little else. Bread and
porridge pass through them and leave them hungry. So they plough nothing. A band hunts,
fishes and keeps fowl and goats, and it needs a wide country to feed a few people. A
ward lives on its Portion. Either way they are few, and a Catfolk settlement of more
than a few hundred is rare.

They sleep as cats do: an hour here and two there, a dozen times in a day and a night.
They are sharpest at dusk and at dawn. A household is never all asleep and never all
awake, and has no bedtime and no fixed meal. The house is built for it, with a flat
roof, ledges and sleeping shelves in the sun and in the shade. Somebody is always
watching. That makes them the best night watch there is, and thieves keep away from a
Catfolk quarter. Business is done at the Dusk Meeting, the one hour when everyone is
awake.

They hunt by waiting and by one short rush. A Catfolk is very fast over fifty paces and
spent after it, and marches badly. They balance like cats, and in a town their road is
the roofs.

The ears and the tail give them away, so they have no use for a straight face. They name
the price they mean. Among themselves a lie is nearly impossible, and their law rests on
that: a witness is watched, not sworn. A Catfolk who must deal with outsiders binds the
tail under a sash and wears a hood. This is called going wrapped, and their own people
think it rude.

**Relations.** Every people that grows grain wants them. Midlanders, Rainkeepers, Canal
folk and Halflings keep a Catfolk ward beside the granary. Noonfolk, who sell most of
their harvest and sleep from sunset, hire them for the night. Sunlander herders sell
them meat and pay them to watch the herd at dusk, when the lions come. Saltroad caravans
carry a pair as scouts and night watch. Orcs compete with them for the game of the dry
grassland and for the same guard contracts. Humans find them easy to like and easy to
cheat, since anyone can read them, and call them lazy, because some of them are asleep
whenever one looks. Sailors hold that a ship with a Catfolk in the rigging does not
founder. Stillfaces find them restful.

**Biomes.** Native (3): Savanna. Good (2): Temperate grassland, Hot desert, Thorn scrub,
Tropical dry forest, Tropical rain forest. Tolerated (1): Cold steppe, Cold desert,
Woodland & shrubland, Temperate mixed forest, Temperate broadleaf forest, Temperate rain
forest, Mangrove. Height: no modifier. Common: 1. Also wants: open country with game, or
a grain town to ward. The weights are those of the earlier default list.

**Vocations.** Granary warden, ratcatcher, night watch, hunter, tracker, trapper,
fowler, fisher, herd guard, caravan scout, rigger, roofer, steeplejack, rope dancer,
courier over a short distance, butcher, tanner, furrier, priest of the Small Huntress.
Never ploughman, miller or baker.

**Factions.** A granary ward and the town that pays its Portion. A hunting band of the
open grassland. A night watch company. The scouts of a caravan house. A temple of the
Small Huntress. A roof guild that holds the roofs of a city quarter. The wrapped:
Catfolk in the service of a court or a merchant house.

---

# Summary

## Habitat weights

The 28 races in the format of [society-generation.md](society-generation.md) Appendix A. The
frequency of a race in a biome is its habitat weight times `common`. Columns: Ice &
snow, Tundra, Cold steppe, Boreal forest, Cold desert, Temperate grassland, Woodland &
shrubland, Temperate mixed forest, Temperate broadleaf forest, Temperate rain forest,
Hot desert, Thorn scrub, Savanna, Tropical dry forest, Tropical rain forest, Mangrove.

| # | Id | Common | Ice | Tun | CSt | Bor | CDs | TGr | WSh | TMx | TBl | TRn | HDs | Thn | Sav | TDf | TRf | Mgv |
|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|---|
| 1 | `northmen` | 3 | 1 | 2 | 1 | 3 | 0 | 1 | 0 | 3 | 1 | 2 | 0 | 0 | 0 | 0 | 0 | 0 |
| 2 | `midlanders` | 5 | 0 | 0 | 1 | 1 | 0 | 3 | 2 | 2 | 3 | 2 | 0 | 0 | 0 | 0 | 0 | 0 |
| 3 | `saltroadFolk` | 3 | 0 | 0 | 0 | 0 | 2 | 1 | 2 | 0 | 0 | 0 | 3 | 3 | 2 | 1 | 0 | 0 |
| 4 | `sunlanders` | 4 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 1 | 2 | 3 | 3 | 2 | 1 |
| 5 | `grassRiders` | 3 | 0 | 1 | 3 | 1 | 2 | 3 | 1 | 0 | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 |
| 6 | `rainkeepers` | 4 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 2 | 3 | 2 | 2 |
| 7 | `canalFolk` | 5 | 0 | 0 | 1 | 0 | 0 | 2 | 1 | 2 | 3 | 3 | 0 | 0 | 0 | 2 | 1 | 1 |
| 8 | `elf` | 2 | 0 | 0 | 0 | 2 | 0 | 1 | 1 | 3 | 3 | 3 | 0 | 0 | 0 | 1 | 2 | 0 |
| 9 | `dwarf` | 2 | 1 | 2 | 1 | 2 | 1 | 1 | 1 | 2 | 1 | 1 | 0 | 0 | 0 | 0 | 0 | 0 |
| 10 | `halfling` | 2 | 0 | 0 | 1 | 1 | 0 | 3 | 3 | 2 | 2 | 1 | 0 | 0 | 1 | 1 | 1 | 0 |
| 11 | `gnome` | 1 | 0 | 1 | 1 | 2 | 1 | 1 | 2 | 2 | 3 | 1 | 0 | 1 | 1 | 1 | 0 | 0 |
| 12 | `orc` | 2 | 0 | 1 | 3 | 1 | 2 | 2 | 2 | 1 | 1 | 1 | 2 | 2 | 2 | 1 | 0 | 0 |
| 13 | `hoarkin` | 1 | 3 | 2 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| 14 | `hearthbloods` | 0.5 | 2 | 3 | 1 | 2 | 0 | 0 | 0 | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 |
| 15 | `widegaze` | 1 | 0 | 0 | 3 | 0 | 1 | 3 | 0 | 0 | 0 | 0 | 0 | 1 | 2 | 0 | 0 | 0 |
| 16 | `amberbloods` | 1 | 0 | 0 | 0 | 3 | 0 | 0 | 0 | 3 | 2 | 2 | 0 | 0 | 0 | 0 | 0 | 0 |
| 17 | `brinefolk` | 1 | 0 | 0 | 1 | 0 | 3 | 0 | 0 | 0 | 0 | 0 | 2 | 1 | 0 | 0 | 0 | 1 |
| 18 | `duskreaders` | 1 | 0 | 0 | 0 | 0 | 2 | 0 | 0 | 0 | 0 | 0 | 3 | 2 | 1 | 0 | 0 | 0 |
| 19 | `noonfolk` | 1 | 0 | 0 | 0 | 0 | 0 | 1 | 1 | 0 | 0 | 0 | 2 | 3 | 3 | 1 | 0 | 0 |
| 20 | `boughwalkers` | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 0 | 2 | 3 | 1 |
| 21 | `shoalfolk` | 1 | 0 | 0 | 0 | 1 | 0 | 0 | 1 | 0 | 0 | 2 | 0 | 0 | 0 | 1 | 1 | 3 |
| 22 | `swiftborn` | 1 | 0 | 0 | 0 | 0 | 0 | 2 | 3 | 1 | 2 | 0 | 0 | 1 | 1 | 0 | 0 | 0 |
| 23 | `flinchless` | 0.5 | 0 | 0 | 1 | 0 | 0 | 2 | 2 | 1 | 2 | 0 | 0 | 0 | 1 | 0 | 0 | 0 |
| 24 | `lanternkin` | 0.5 | 0 | 0 | 0 | 0 | 0 | 1 | 1 | 1 | 1 | 2 | 0 | 0 | 1 | 2 | 2 | 1 |
| 25 | `stillfaces` | 1 | 0 | 0 | 0 | 2 | 0 | 0 | 1 | 3 | 2 | 1 | 0 | 0 | 0 | 0 | 0 | 0 |
| 26 | `twofold` | 1 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 1 | 2 | 3 | 2 | 1 |
| 27 | `bitterbloods` | 0.5 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 0 | 1 | 0 | 0 | 0 | 1 | 3 | 3 |
| 28 | `catfolk` | 1 | 0 | 0 | 1 | 0 | 1 | 2 | 1 | 1 | 1 | 1 | 2 | 2 | 3 | 2 | 2 | 1 |

**Height modifiers.** Control points `(height in m, factor)`, as in
[adding-a-race.md](adding-a-race.md).

| Race | Points | Meaning |
|---|---|---|
| Elves | `(200, 1), (200, 0)` | Only below 200 m |
| Dwarves | `(200, 0), (200, 1)` | Only from 200 m |
| Hearthbloods | `(100, 0.5), (400, 1)` | Half weight below 100 m, full weight from 400 m |
| Widegaze | `(300, 1), (500, 0)` | Full weight below 300 m, none from 500 m |
| Shoalfolk | `(20, 1), (80, 0)` | Full weight below 20 m, none from 80 m |

The other 23 races have none.

**`common`.** 3 to 5 for the human races, 2 for four of the classic races and 1 for
the Gnomes. A new race has 1 when it has a homeland of its own and 0.5 when it lives
scattered among others or is rare by its nature. The human races sum to 27, against 10
for the single `human` of the earlier default list.

**Catfolk.** The Catfolk keep the row of the earlier default list, which comes from
[research/races.md](research/races.md) §4.1: `common` 1 and a weight in 13 of the 16
biomes. The other new races have a weight in 3 to 9 biomes.

## Measured shares

Measured on 1,079 regions: the 18 × 15 regions around region (0, 0) (i from −9 to 8, j
from −7 to 7) of the seeds `cellgen`, `gameworld`, `atlas` and `ember`, taken to the
Planned stage with the default Parameter Set. One of the 1,080 regions has no land. The
regions hold about 407,600 persons, 364 in a median region.

The measurement was made at 6 persons per km². The default is 0.06 since 2026-11-15, and
the persons below are the measured numbers divided by 100. The races of a region and the
shares are the same at both densities, apart from the rounding to whole persons.

A region holds six races: four main races and two minority races
([society-generation.md](society-generation.md) section 3.3). The plain share is what a
race would hold if every race shared every region, by the frequencies and the height
modifiers alone. It is what the measurement gave before the regions were limited to six
races.

| # | Race | Group | Plain share | Share of all persons | Regions that hold it | As a main race | Largest race in (of 1,079) | Persons in a median region that holds it |
|---|---|---|---|---|---|---|---|---|
| 1 | Northmen | Human | 5.44 % | 4.18 % | 33 % | 25.4 % | 29 | 47 |
| 2 | Midlanders | Human | 11.16 % | 19.21 % | 80 % | 77.8 % | 352 | 88 |
| 3 | Saltroad folk | Human | 9.10 % | 15.13 % | 64 % | 60.2 % | 244 | 83 |
| 4 | Sunlanders | Human | 8.99 % | 13.55 % | 63 % | 58.9 % | 90 | 79 |
| 5 | Grass riders | Human | 4.62 % | 1.94 % | 18 % | 9.5 % | 6 | 36 |
| 6 | Rainkeepers | Human | 7.43 % | 7.54 % | 45 % | 39.1 % | 3 | 60 |
| 7 | Canal folk | Human | 11.97 % | 21.93 % | 90 % | 88.8 % | 340 | 90 |
| 8 | Elves | Classic | 3.04 % | 0.65 % | 10 % | 1.7 % | 0 | 22 |
| 9 | Dwarves | Classic | 1.46 % | 0.36 % | 8 % | 0.2 % | 0 | 16 |
| 10 | Halflings | Classic | 5.50 % | 2.44 % | 20 % | 10.9 % | 0 | 45 |
| 11 | Gnomes | Classic | 3.04 % | 0.72 % | 9 % | 0.0 % | 0 | 28 |
| 12 | Orcs | Classic | 7.14 % | 6.23 % | 35 % | 26.7 % | 15 | 63 |
| 13 | Hoarkin | New | 0.47 % | 0.27 % | 8 % | 0.4 % | 0 | 11 |
| 14 | Hearthbloods | New | 0.31 % | 0.24 % | 7 % | 0.1 % | 0 | 13 |
| 15 | Widegaze | New | 1.17 % | 0.37 % | 8 % | 0.0 % | 0 | 16 |
| 16 | Amberbloods | New | 1.65 % | 0.41 % | 8 % | 0.0 % | 0 | 19 |
| 17 | Brinefolk | New | 1.35 % | 0.38 % | 9 % | 0.1 % | 0 | 15 |
| 18 | Duskreaders | New | 1.71 % | 0.41 % | 8 % | 0.2 % | 0 | 17 |
| 19 | Noonfolk | New | 2.38 % | 0.48 % | 8 % | 0.0 % | 0 | 20 |
| 20 | Boughwalkers | New | 0.89 % | 0.31 % | 8 % | 0.0 % | 0 | 14 |
| 21 | Shoalfolk | New | 0.20 % | 0.18 % | 6 % | 0.0 % | 0 | 11 |
| 22 | Swiftborn | New | 2.08 % | 0.40 % | 8 % | 0.0 % | 0 | 19 |
| 23 | Flinchless | New | 0.81 % | 0.38 % | 8 % | 0.0 % | 0 | 20 |
| 24 | Lanternkin | New | 0.95 % | 0.35 % | 8 % | 0.0 % | 0 | 18 |
| 25 | Stillfaces | New | 1.68 % | 0.44 % | 9 % | 0.0 % | 0 | 18 |
| 26 | Twofold | New | 1.57 % | 0.34 % | 8 % | 0.1 % | 0 | 17 |
| 27 | Bitterbloods | New | 0.43 % | 0.29 % | 7 % | 0.0 % | 0 | 16 |
| 28 | Catfolk | New | 3.45 % | 0.89 % | 10 % | 0.0 % | 0 | 32 |

| Group | Plain share | Share of all persons | Seed `cellgen` | Seed `gameworld` | Seed `atlas` | Seed `ember` |
|---|---|---|---|---|---|---|
| Human races (7) | 58.7 % | 83.5 % | 82.5 % | 83.4 % | 84.1 % | 83.9 % |
| Classic races (5) | 20.2 % | 10.4 % | 11.5 % | 10.7 % | 9.7 % | 9.8 % |
| New races (16) | 21.1 % | 6.1 % | 5.9 % | 5.9 % | 6.3 % | 6.3 % |

| Per region | Lowest | Median | Highest |
|---|---|---|---|
| Races of the region | 6 | 6 | 6 |
| Races with 1 % or more of the persons | 5 | 6 | 6 |
| Share of the largest race | 19 % | 27 % | 43 % |
| Share of a minority race | 0.4 % | 4.9 % | 25 % |
| Persons | 7 | 364 | 1,188 |

What the numbers show:

- **Humans hold 83.5 %.** By the plain shares it is 58.7 %. The main races of a
  region are its four largest, and in most regions those are human races. The share a
  minority race is lifted towards (0.05) moves this: the two minority races of a region
  hold about a tenth of it between them.
- **A region has a few peoples.** The largest race holds 27 % of a median region, and
  between 19 % and 43 % over all regions. No race has a majority anywhere. By the plain
  shares the largest race of a median region had 15 %, and the region held 20 races at
  1 % or more.
- **Canal folk and Midlanders are main races nearly everywhere**, in 89 % and 78 % of
  the regions. Both have `common` 5 and share the temperate biomes. A region that is the
  homeland of one race needs something the rule does not have: a presence per race and
  region. This is the open topic of
  [map-generation-research.md](map-generation-research.md) section 10.7.
- **Grass riders and Rainkeepers are seldom the largest race of a region**, in 6 and 3
  regions. Grass riders are native to Cold steppe and Temperate grassland, which are 3 %
  and 4.5 % of the land, and in the grassland the Midlanders outweigh them 15 to 9.
  Rainkeepers share five of their six biomes with the Sunlanders, at an equal or lower
  frequency everywhere except Mangrove, which is 2 % of the land.
- **Orcs are a main race in 27 % of the regions**, more often than Northmen and Grass
  riders. They have a weight of 2 in six biomes and live in 13 of the 16. Halflings are
  a main race in 11 %. Both keep the weights of the earlier default list, where they
  stood against one human race with `common` 10.
- **Elves, Dwarves and Gnomes are minority races.** They are a main race in 1.7 %, 0.2 %
  and 0.0 % of the regions, and a race of the region in 10 %, 8 % and 9 %. By the plain
  shares nearly every region has both elves and dwarves.
- **Each new race is in 6 to 10 % of the regions**, always as a minority race or nearly so.
  Together the new races hold 6.1 % of the persons. Where a new race is present it
  has 11 to 32 persons in the median region.
- **The turns even out the rare races.** By the plain shares the Shoalfolk hold 0.20 %
  and the Catfolk 3.45 %, 17 times as much. As minority races they are in 6 % and 10 % of
  the regions. The Shoalfolk miss a turn where no coast below 80 m feeds 0.1 of them.

The numbers were read from the Planned regions of a `World` in a throwaway program. The
map generation app writes the same data for a block of regions: the region files hold
the potential population per race, with the standing of each race, and the races of each
Prospect. This gives the 25 regions around region (0, 0) of one seed:

```bash
dotnet run --project src/Gameworld.MapGenerator -c Release -- --seed cellgen --rings 2 --stage Planned --modes biomes --out out/races/cellgen
```

## What the entries ask for that a race cannot say yet

A race is an id, a name, a toggle, a frequency per biome and a height modifier. The
"Also wants" of the entries name more than that. They are collected here.

| Want | Races |
|---|---|
| Coast or tidewater | Northmen, Hoarkin, Shoalfolk, Brinefolk |
| River, well or river plain | Midlanders, Saltroad folk, Rainkeepers, Canal folk, Halflings, Noonfolk, Bitterbloods |
| Elevation type | Dwarves, Hearthbloods and Gnomes want hills or mountains. Halflings, Widegaze and Grass riders want flat land or lowland |
| Bedrock | Dwarves: an ore-bearing bedrock. Sunlanders: gold and iron. Brinefolk: rock salt |
| Supernatural field | Elves, Amberbloods |
| Prosperity field | High for Halflings and Swiftborn. Low for Orcs |
| A town, a court or a trade route | Lanternkin, Gnomes, Saltroad folk, Duskreaders, Catfolk |
| Another race to live among | Flinchless (Midlanders), most Lanternkin, and the Catfolk wards (a people that grows grain) |

Several races also carry a rule that touches how a family or a person gets its race,
which the readme leaves undecided: a Twofold changes sex at about thirty, a Swiftborn
generation is seven years, a Hoarkin lodge sleeps through the winter, and a Bitterblood
marries an outsider only after ten years of seasoning.

## Not done

- The weights and `common` values are a first proposal. They are measured (see
  Measured shares) and have not been tuned after the measurement.
- The Catfolk weights were not written for the entry here. They are the row of the
  earlier default list, and they give the Catfolk more land than any other new race.
- The names have not been searched for earlier use.
- The seven human races are built on real peoples.
