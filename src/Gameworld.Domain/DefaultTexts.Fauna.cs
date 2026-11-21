namespace Gameworld.Domain;

// What each default kind of fauna is and its lore: the description and the part "Lore" of its entry in docs/research/flora-and-fauna.md section 5, without the citations.
// A kind that has no text here gets DefaultTexts.Missing.

public static partial class DefaultTexts
{
    internal static readonly Dictionary<string, (string Description, string Lore)> Fauna = new()
    {
        ["lemming"] = (
            "Stout rodent, 13 to 18 cm, tawny brown, of low treeless tundra meadows. It keeps the same coat in winter and stays active under the snow.",
            "In a lemming year the foxes and owls breed double. The Hoarkin count lemming runs in autumn to judge the winter."),
        ["arcticHare"] = (
            "Large hare, 3 to 5 kg, white in winter and grey-brown in summer, of tundra and polar coasts. Northern peoples use the fur for clothing and blankets and eat the meat.",
            "Its tracks look three-legged. Children are told that a hare leaving four prints is something else wearing the skin."),
        ["ptarmigan"] = (
            "Grouse with feathered feet like snowshoes, white in winter with a black tail, barred brown in summer. A popular game bird, and the one tundra bird that does not migrate.",
            "Northmen call a man who stays at his post through the dark a ptarmigan."),
        ["snowBunting"] = (
            "Small black-and-white bird of rocky slopes that burrows deep into snow to survive the cold.",
            "The first bunting on the ice tells the Hoarkin that the seals will soon pup."),
        ["bitingSwarm"] = (
            "Mosquitoes: slim flies with piercing mouthparts. The females take blood and carry fevers, and they are most abundant in the warm humid tropics. The summer clouds over tundra and taiga are general knowledge.",
            "Herds walk into the wind to lose them. A camp is sited by the swarm before it is sited by the water."),
        ["redSquirrel"] = (
            "Tuft-eared squirrel of conifer and broadleaf forest, red to black with a pale belly. Hunted for its fur. Its forgotten caches sprout into trees.",
            "Elves say the squirrels planted the first oak woods and are owed a tithe of nuts."),
        ["stoat"] = (
            "Small weasel-shaped hunter, brown in summer, white in winter with a black tail tip. The white fur trims coats and stoles, and it is an excellent mouser.",
            "Ermine was the fur of the judges of the Old Realms. A court still pays more for a white pelt than a trapper earns in a month."),
        ["pika"] = (
            "Small short-eared relative of the hare, straw-grey, of steppe and semi-desert. It cuts grass and stacks it to dry. Herders use the hay piles, and the fur once made felt.",
            "Grass riders who take pika hay in a hard winter leave a pinch of salt on the burrow."),
        ["bobakMarmot"] = (
            "Stocky straw-coloured burrowing rodent, about half a metre long, of open steppe. Sentinels whistle the alarm. The pelts made hats and coats, and it was eaten in famines.",
            "An editor's note to Herodotus suggests that the gold-digging ants were marmots. Steppe prospectors sift marmot spoil for gold dust."),
        ["jerboa"] = (
            "Very small hopping rodent with large three-toed hind feet and a long tufted tail, of sandy and stony desert. Eaten by some people.",
            "Duskreaders, who see warmth in the dark, follow jerboa trails to cool seeps under the sand."),
        ["hareAndRabbit"] = (
            "Brown hare of 3 to 5 kg with long black-tipped ears, of open fields and pastures with hedgerows. Prized game with tasty meat, and a pest of crops and young orchards. It runs at about 60 km/h.",
            "A hare that crosses the plough line before sowing is chased off the field by the whole household."),
        ["groundSquirrel"] = (
            "Small yellowish burrowing squirrel of short-grass steppe and pasture. Eaten on a small scale and a pest in vineyards.",
            "Widegaze lookouts read a rider's approach from the sousliks going to ground, long before the dust shows."),
        ["locust"] = (
            "The desert locust is camouflaged when solitary and black and yellow when it swarms. A destructive migratory pest of dry grassland and desert. One swarm crossing a sea was estimated at 5,000 km².",
            "The last swarms of the Ashen Years ate the thatch off the roofs. Sealed granaries date from then."),
        ["honeybee"] = (
            "Banded reddish-brown bee of meadows, open woodland and gardens. It pollinates crops and gives honey and wax. Foragers dance to show where food lies.",
            "Halflings keep hives by every burrow and tell the bees of each birth and death in the house."),
        ["hedgehog"] = (
            "Small round animal with brown-and-white spines, of fields, field edges and hedgerows. It curls into a ball when threatened. Used for food and for pest control.",
            "A hedgehog in the garden is lucky, and a gardener who kills one loses the year's cabbages."),
        ["edibleDormouse"] = (
            "The largest dormouse, grey with a bushy tail, of deciduous and mixed forest. A true hibernator that lives on its fat from autumn to spring. Trapped for fur and meat.",
            "Fattened dormice were a dish of Old Realms banquets. Midlander inns still keep them in clay jars."),
        ["woodpecker"] = (
            "A large woodpecker, about 46 cm, of conifer and beech woodland. Woodpeckers cut their own nest holes in dead wood.",
            "Pliny reports that a woodpecker reopens a plugged nest hole with a certain herb. Thieves pay well for \"woodpecker's herb\", said to open any lock."),
        ["rat"] = (
            "Agile climbing rodent with a tail longer than its body, of coasts, buildings and fields. It damages crops and fruit trees and fouls food, and its fleas spread plague.",
            "Grain towns give their Catfolk wardens meat and fish and count the rats they bring as rent paid."),
        ["bananaSlug"] = (
            "Slug up to 25 cm long, mostly yellow with brown spots, of moist dark forest floors under logs. A garden pest.",
            "Its slime numbs the tongue, and children of the wet coast dare each other to lick one."),
        ["roughSkinnedNewt"] = (
            "Newt of 13 to 22 cm with grainy dark skin and an orange belly, of forest ponds and streams. One holds enough toxin to kill several people. Threatened, it bends back to show the belly.",
            "Bitterbloods, who eat what poisons others, buy them alive."),
        ["leech"] = (
            "Flattened dark worm with reddish stripes, of small muddy reed-edged ponds. Long used for bloodletting. It comes to moving shadows.",
            "A barber's leech pond is fenced like a fish pond and inherited like one."),
        ["silkworm"] = (
            "A fully domesticated moth that is not found in the wild. The caterpillar spins a cocoon of one strand about 915 m long, and silk has been taken from it for thousands of years. It is fed on white mulberry leaves.",
            "Canal folk grow mulberry on the dikes for it. Taking the eggs out of a silk province is a hanging matter."),
        ["kermesScale"] = (
            "Scale insect whose dried bodies give the oldest known red dye. The dye was once paid as tribute and taken as rent. It lives on the kermes oak.",
            "\"Oak blood\" pays the rent of many a hill village, gathered by lamplight before the dew dries."),
        ["cochinealInsect"] = (
            "Small grey wingless insect under a white waxy coat, feeding on prickly pear. About 70,000 make one pound of scarlet dye.",
            "Noonfolk estates fence their cactus rows and brush the insects off by hand at noon."),
        ["lacInsect"] = (
            "Scale insect whose resin is shellac, used as wood varnish, sealer and cement. The page read does not name its host trees or its range.",
            "Rainkeeper temples seal their deeds with red lac."),
        ["cicada"] = (
            "Insect of 2 to 5 cm with clear wings and large eyes, of forests, grasslands and deserts. Used in folk medicine, as a symbol and as food. Some broods emerge in vast numbers every 13 or 17 years.",
            "A brood year is a census year: whoever was born in the last one comes of age."),
        ["tortoise"] = (
            "Land tortoise of 12 to 23 cm, of forest, scrub and grassland. Kept as a pet and eaten in places.",
            "Halflings carve a child's birth year on a garden tortoise, which outlives the child."),
        ["scorpion"] = (
            "Large yellowish scorpion of 8 to 11 cm that lives under rocks and in burrows in arid land and comes out at night. One of the most dangerous scorpions.",
            "Saltroad caravans shake out every boot at dawn, and the guide who forgets pays for the camp's water."),
        ["scarab"] = (
            "Dung beetles are round, dark and 5 to 30 mm long. They shape manure into a ball and bury it, which turns dung into soil. One of them was the sacred scarab of ancient Egypt.",
            "Along the desert river the beetle rolling its ball is the sun crossing the sky, and it is carved on every seal."),
        ["termite"] = (
            "Soft pale insects that build domed or conical mounds of earth, clay and saliva. They destroy wooden structures and crops, though only one species in ten is a pest.",
            "Sunlander potters dig their clay from old mounds."),
        ["tsetseFly"] = (
            "Brown fly of 6 to 16 mm, of woodland and sometimes open grassland. It gives sleeping sickness to people and nagana to cattle and horses.",
            "The fly that no mounted army of the Old Realms could pass. Where it lives, wealth is counted in yams and iron, not cattle."),
        ["honeyguide"] = (
            "Bird of about 20 cm of savanna, scrub and forest edge. It calls and flashes its tail to lead honey badgers and others to hives.",
            "A hunter who follows the bird and leaves it no comb is led to a snake the next time."),
        ["quelea"] = (
            "Small weaver bird of the savanna. Colonies nest over several square miles of trees in millions, and it can become an agricultural pest.",
            "Noonfolk children earn their first wage scaring queleas off the millet."),
        ["mongoose"] = (
            "Small short-legged carnivore with a pointed nose and a long furry tail, grey to brown. The Egyptian kind keeps down rats, mice and snakes and raids chicken yards. Pliny has it coat itself in dried mud to fight the asp.",
            "Rainkeeper houses keep one as others keep a cat."),
        ["poisonFrog"] = (
            "Frog of about 47 mm, bright yellow, green or orange, that sits in the open on the rain forest floor. Skin contact can kill. Its poison tips blowgun darts.",
            "Bitterbloods keep them in pens and sell the dart poison by the thorn."),
        ["leafcutterAnt"] = (
            "Ants that carry leaf pieces along trails of over 30 m to underground nests of more than 1,000 chambers, where they grow a fungus on the chewed leaves. They can cut the value of pasture.",
            "Boughwalkers say the ants taught them to tend the forest as a garden."),
        ["firefly"] = (
            "Soft-bodied beetles of 5 to 25 mm whose flash patterns are signals between the sexes, in most tropical and temperate regions.",
            "Lanternkin call them cousins and will not have them caught in jars."),
        ["fiddlerCrab"] = (
            "Small crab whose male waves one enlarged claw, on intertidal mud of marshes and estuaries.",
            "Shoalfolk children learn to count on the waving claws."),
        ["mudskipper"] = (
            "Fish up to 30 cm with large mobile eyes set close together, of swamps, estuaries and mudflats. It leaves the water to walk and climb, breathing through its gill chambers and skin.",
            "\"The fish that could not decide\" is a Shoalfolk name for a person of two loyalties."),
        ["mudCrab"] = (
            "Crab with a shell of 25 to 28 cm and a weight of 2 to 3 kg. It burrows in mangroves and soft intertidal mud, the young live in estuaries, and it is commercially important.",
            "A Shoalfolk family owns its crab holes as others own fields."),
        ["junglefowl"] = (
            "Bird of about 70 cm with gold, red and metallic green plumage, of secondary forest and scrub. Raised for eggs and meat, for cockfighting and for feathers.",
            "Every village cock is said to descend from a pair given by a forest god, and the first egg of the year goes back to the forest."),
        ["flyingFox"] = (
            "Fruit bat with a wingspan of 1.2 to 1.5 m. Several hundred roost in one tree near water. Hunted for food and medicine, and a pest of orchards.",
            "A roost tree is never felled: the bats carry the names of the dead out at dusk and back at dawn."),
        ["murex"] = (
            "Sea snails whose dye, Tyrian purple, was highly valued in antiquity. Pliny says the dye lies in a small vein in the neck, the fishing is best after the dog star rises, and the finest purple wool cost five times the common kind.",
            "Purple was the colour of the sorcerer-kings. In some realms it is still death to wear it without leave."),
        ["pearlOyster"] = (
            "Oysters of warm seas that lay nacre around an intruder. Pearls have long been prized as gems. Pliny held that the best came from the eastern ocean and the gulfs of Arabia.",
            "A Shoalfolk family owns its oyster bed and guards it."),
        ["ringedSeal"] = (
            "Small seal of 1.1 to 1.75 m with a dark coat ringed in silver, of ice floes and pack ice. It keeps breathing holes open in the ice. Hunted for food, oil and skins.",
            "The Hoarkin followed the seals out onto the ice. A hunter sits at a breathing hole for a day without moving."),
        ["penguin"] = (
            "Flightless seabird of 1 to 1.3 m and 25 to 45 kg, black above and white below with yellow ear patches, on polar ice and coasts. It huddles in thousands through the winter.",
            "Hoarkin say the penguins are a people who chose the sea and lost their hands for it."),
        ["arcticFox"] = (
            "Small fox, white in winter and grey to brown in summer, of coastal and alpine tundra. It stores summer food under stones. The fur is prized, and it takes lambs.",
            "It follows the white bear for scraps. Trappers follow the fox to find the bear."),
        ["snowyOwl"] = (
            "White owl of 63 to 73 cm with yellow eyes, of open tundra, hunting largely by day. It keeps rodents down and robs traplines.",
            "Hearthblood pass wardens take the owl as their badge: it sees the storm first."),
        ["sledDog"] = (
            "Large, powerful, thick-coated dogs of great endurance that pull sleds over snow and ice with people and supplies, a dozen or more in a fan hitch.",
            "A Hoarkin team is named as one person and inherited as one."),
        ["reindeer"] = (
            "Deer up to 1.2 m at the shoulder with a hollow-haired coat from dark brown to nearly white. Both sexes carry antlers. It migrates over 5,000 km a year and digs through snow to feed. Herded for thousands of years for meat, fur and antler.",
            "The herd is the calendar of the north: every feast is fixed by where it stands."),
        ["eider"] = (
            "The largest northern duck, of rocky coasts and offshore islands. The male is black below and white above. Taken for its down, for sport and for food.",
            "Northmen farm the down by building the ducks stone shelters and taking a share from each nest."),
        ["wolverine"] = (
            "Stocky animal of 9 to 30 kg with brown-black fur and pale shoulder stripes, of boreal forest and tundra. It drives bears and wolves off their kills and raids traps, cabins and food stores. The fur resists cold.",
            "Trappers do not say its name in the forest. They call it \"the partner\", because it takes half."),
        ["moose"] = (
            "The largest deer, up to 2.3 m at the shoulder, dark, with huge antlers on the male, of taiga and wetlands near water. Largely solitary. Taken for meat, and captives give milk.",
            "Caesar was told that the elk has no joints, sleeps leaning on a tree, and is caught by sawing the tree. Northmen tell the tale to strangers with a straight face."),
        ["beaver"] = (
            "Rodent of 13 to 35 kg with a dark coat, of forested lakes, ponds and rivers. Pelts, castoreum and meat are prized. Its dams flood farmland and roads. The bestiary has the hunted beaver bite off the parts wanted for medicine and throw them to the hunter.",
            "A beaver pond is a boundary mark in Northmen law: who drains it moves the border."),
        ["sableAndMarten"] = (
            "Marten of about 1 to 2 kg of dense taiga, light to dark brown with a richer winter coat. Fur hunting once cut its numbers.",
            "\"Soft gold\": the Northmen paid their grain debts of the Ashen Years in sable."),
        ["lynx"] = (
            "Cat of 18 to 36 kg with a short black-tipped tail, grey to rusty, spotted or plain. A solitary ambush hunter that caches its prey. It kills livestock.",
            "Seen once, it is luck. Seen twice, someone is hunting you."),
        ["capercaillie"] = (
            "The largest grouse: the cock weighs 4 to 5 kg and is glossy black with red eye combs. It lives in old conifer forest and displays at leks. Taken as game.",
            "The cock is deaf while it sings, and so is a man in love, say the Northmen."),
        ["muskDeer"] = (
            "Small antlerless deer of 15 to 17 kg of mountain taiga. The male has long downward canines and a musk pouch. The musk is valued for perfume and medicine, and hunting for it has made the deer rare.",
            "A pod of musk is worth a horse. Dwarf holds buy it for their dead."),
        ["saiga"] = (
            "Antelope of 30 to 45 kg with a large head and a mobile swollen nose, yellowish red in summer and grey in winter, of dry steppe and semi-desert. Taken for meat, fur and horn, which is ground for medicine.",
            "Grass rider boys must run one down on horseback before they may marry."),
        ["horse"] = (
            "Grazer of 227 to 900 kg, of temperate grassland, steppe and savanna, living in herds with a dominance order. Used for food, transport, war and sport. The wild steppe horse is stocky, reddish brown, with a stiff dark mane, and was never tamed.",
            "Children of the Grass riders ride before they walk well, and the theft of a horse is death."),
        ["sheep"] = (
            "Flocking grazer, white to near black, kept in deserts, grasslands, scrub and mountains for meat, milk, wool and hides. Flocks of up to 1,000 move without a leader. Herodotus tells of a breed whose tails are so long that each rests on a little cart.",
            "The Widegaze keep sheep on open range without fences and watch them from a mound."),
        ["wildSheepAndIbex"] = (
            "The argali is the largest sheep, 60 to 185 kg with corkscrew horns, of steep slopes above 1,000 m. The ibex is a bearded mountain goat taken for food, horn and medicine. The bestiary says the ibex lands on its horns when it falls.",
            "A dwarf who brings down a ram on the heights hangs the horns over the hold gate."),
        ["yak"] = (
            "Long-haired blackish ox of alpine meadow and steppe with upcurved horns. It gives meat, milk, butter and cheese, hair, pack transport and dung for fuel.",
            "Hearthbloods herd yaks on the high passes and hire them out with their drivers."),
        ["bactrianCamel"] = (
            "Two-humped camel of 300 to 690 kg with a long thick coat, of cold desert and steppe. Used for transport, meat, milk, hides, hair and dung fuel.",
            "Brinefolk salt caravans count distance in camel days and pay tolls in stamped salt."),
        ["onager"] = (
            "Wild ass of 200 to 260 kg, reddish brown with a white belly and a dark stripe down the back, of desert and steppe within reach of water. Hunted for meat and hide. Herders see it as a rival for water.",
            "No one has broken an onager to the saddle. Orc hearths count the attempt as a rite."),
        ["goldenEagle"] = (
            "Dark brown eagle with a golden nape and a wingspan of about 2 m, of open country from sea level to 3,600 m. Pairs hunt together. It sometimes kills livestock.",
            "Grass rider falconers fly eagles at foxes and wolves. A trained bird is set free after ten winters."),
        ["falcon"] = (
            "The saker is a brown, streaked falcon of about 50 cm, of open country with cliffs and scattered trees. A persistent hunter that follows prey into brush. It is taken for the bird trade.",
            "A white falcon is a gift between khans and a cause of war when it is refused."),
        ["cattle"] = (
            "Herd animal of 150 to 1,360 kg, black, white, red or brown, kept on grassland and farmland for milk, meat and leather, for dung, and for ploughing and hauling.",
            "Among Sunlanders cattle are bride-price, blood-price and the measure of a man."),
        ["redFox"] = (
            "Fox with a coat from yellowish red to deep red-brown and black lower legs, of forest, prairie, farmland and towns: the widest range of any wild dog. Its fur is valuable, and it takes poultry.",
            "In Halfling tales the fox is the tax collector."),
        ["redDeer"] = (
            "Large deer, dark brown in winter and tan in summer with a pale rump. The stag carries antlers up to 1.5 m and bellows in autumn. Taken for food, hide and antler, and a pest of crops.",
            "The stag is the lord's game: most Midlander forests are closed to commoners."),
        ["wildBoarAndPig"] = (
            "Boar of 66 to 272 kg, black to red-brown, of forest, shrubland and marsh near water. Sows live in groups. Hunted for sport and food, and it ruins crops. The pig is its domestic form.",
            "Halflings keep a pig by every burrow. Orcs keep them half wild."),
        ["goat"] = (
            "Bearded, horned browser of about 45 kg kept in nearly all habitats for milk, meat, wool and leather. Gone wild, it grazes land bare. Herodotus and Pliny have the resin ladanum combed from goats' beards.",
            "Dwarves keep goats on the high pastures and make hard cheese. Saltroad nomads weave their tents of goat hair."),
        ["donkeyAndMule"] = (
            "Grey ass of 1.1 to 1.4 m at the shoulder with a dark stripe down the back, of desert and mountain country. It carries heavy loads and guards livestock.",
            "A mule knows the mountain road better than the muleteer, and the toll is charged per mule for that reason."),
        ["raven"] = (
            "Large black bird up to 69 cm with a wedge-shaped tail, of open country from tundra to desert. Highly intelligent. It clears carrion and eats grain.",
            "Soldiers say the ravens know the outcome of a battle the day before, by which camp they roost above."),
        ["crane"] = (
            "Tall wading bird, larger than a heron, of marshes and plains. Pairs reuse the same nest for years. Pliny says cranes post sentinels at night that hold a stone in one foot, so that it falls and wakes them if they sleep.",
            "The crane with the stone is the badge of sworn sentries."),
        ["otter"] = (
            "Playful swimmer of about 7 kg of lakes, rivers and coasts, also of peat swamp and rice fields. Its pelt is valuable, and fishers see it as a rival.",
            "Canal folk boatmen train otters to drive fish into the nets."),
        ["waterfowl"] = (
            "The greylag goose is grey-brown, about 80 cm long, with an orange bill, and lives in marshes, fens, estuaries and farmland. It was domesticated for meat and down. Flocks fly in a V.",
            "Halflings keep geese as watchdogs."),
        ["dromedary"] = (
            "One-humped camel of 400 to 600 kg, sandy brown, of deserts with long dry seasons. It can drink 100 litres in ten minutes. A beast of burden that also gives milk, meat, wool, leather and dung fuel.",
            "A caravan of a thousand camels carries salt south. The lead camel has a name and a share of the profit."),
        ["gazelleAndOryx"] = (
            "The dorcas gazelle is pale with brown flank stripes and curved horns, of semi-desert, dunes and wadis. The oryx is larger, cream to grey-brown, with long horns, and travels towards distant rain. Both are hunted for meat and hide.",
            "Duskreaders hunt them at night by the warmth of the ground a herd has lain on."),
        ["fennec"] = (
            "The smallest fox, 0.8 to 1.5 kg, with very large ears and thick pale fur, of sandy desert. It caches food and remembers the caches.",
            "A fennec den marks water within a day's dig, say the well diggers."),
        ["ostrich"] = (
            "Flightless bird about 2 m tall, of dry savanna and scrub. The cock is black and white. Taken for hide, feathers and eggs. The shells carry water.",
            "Sunlander title-holders wear the plumes. The eggshell is the traveller's flask."),
        ["hornedViper"] = (
            "Snake of 30 to 60 cm, sand-coloured, with a scale \"horn\" over each eye. It buries itself in sand with only eyes and nostrils showing. Its bite causes swelling and bleeding. Kept by snake charmers and milked for venom. Herodotus knew small horned snakes that were buried in a temple.",
            "The Duskreaders see them as warm commas on the cold sand and walk around them."),
        ["cobra"] = (
            "Snake of about 2 m that raises the front third of its body and spreads a marked hood. It lives in forest and farmland, eats rats, and its bite kills many each year.",
            "Rainkeeper granaries tolerate one for the rats and set out milk for it."),
        ["wildebeestAndZebra"] = (
            "The wildebeest is a slate-grey, maned and bearded antelope of open grass and floodplain. The zebra is a black-and-white striped horse led in file by a mare. Both are taken for meat and skins and compete with cattle.",
            "The great herds follow the rains, and the Sunlander herders move their cattle behind them."),
        ["giraffe"] = (
            "The tallest animal: bulls reach 5.7 m. It has a spotted coat and lives in savanna and open woodland. Taken for meat and hide.",
            "Widegaze say the giraffe is the only beast that has seen as far as they have."),
        ["vulture"] = (
            "Large carrion birds, many with bare heads, of open country, with keen eyesight and strong beaks. They feed in a strict order.",
            "Where the vultures circle, a caravan has lost a beast or a kingdom a battle. Either is news worth selling."),
        ["waterBuffalo"] = (
            "Dark, thin-haired ox with wide horns, of marsh, swamp and river land. It wallows in mud. It gives milk, meat and leather and draws the plough.",
            "The paddy cannot be worked without it. Rainkeepers grow rice in the wet lands, and a village counts its wealth in buffaloes."),
        ["peafowl"] = (
            "Bird with a bright blue neck. The cock carries a long train of eyespot feathers. It lives in open forest, roosts high in trees, and is kept for its feathers and against pests.",
            "A hundred eyes that never close: the peacock is the bird of the Rainkeeper tax office."),
        ["monkeys"] = (
            "The langur is a grey, black-faced monkey of about 13 kg, of forest, scrub and towns. It is held sacred, and it raids crops and steals food from houses. The proboscis monkey of the mangrove has a large nose and swims well.",
            "Temple monkeys are fed on feast days and cursed the rest of the year."),
        ["tropicalDeer"] = (
            "The chital stands 0.6 to 1 m at the shoulder, with a red-brown coat in rows of white spots. The stag has three-tined antlers. It lives in tropical grassland and along rivers and is hunted.",
            "It grazes under the langurs, which drop fruit and bark at the tiger."),
        ["parrotAndMacaw"] = (
            "The scarlet macaw is about 89 cm long, scarlet with yellow and blue wings, and lives in the rain forest canopy. Pairs mate for life. Taken for the cage trade and for feathers.",
            "Boughwalker bird catchers sell birds that speak, and what a bird repeats in a market has started feuds."),
        ["sloth"] = (
            "Slow tree-dweller of about 4 kg whose coat is green with algae. It sleeps 14 to 16 hours a day in the forest canopy.",
            "To the Swiftborn, who live fast, the sloth is a holy animal or a joke, by temperament."),
        ["tapir"] = (
            "Dark, heavy browser of 150 to 250 kg with a short trunk, of lowland and swamp forest near rivers. Taken for hide and meat, and trained to pull ploughs.",
            "A forest plough ox for those the tsetse has denied cattle."),
        ["dugong"] = (
            "Grey-brown sea mammal of 2.4 to 4 m and up to 400 kg that grazes seagrass in shallow sheltered bays. Taken for food and oil, and its tusks for medicine.",
            "Shoalfolk herd them in fenced bays and will not eat a cow that has calved."),
        ["monitorLizard"] = (
            "Dark lizard, commonly 1.5 m and up to 3 m, of forest rivers and swamps. It climbs trees and dives when threatened. The skin is used for leather, food and medicine.",
            "Bitterblood tanners make a leather of it that rot will not touch."),
        ["llama"] = (
            "Woolly camel relative of about 1.2 m and 140 kg, of high plateaus up to 4,000 m. A pack animal that gives wool and guards sheep. In the generator it stands on tropical highlands, which carry temperate biomes.",
            "The highland road has no wheels. Everything a mountain kingdom owns has been on a llama's back."),
        ["salmon"] = (
            "Silver-sided fish of 2 to 9 kg that lives at sea and returns to its home stream to spawn. A prized food fish.",
            "The run is counted at the first weir, and the count sets the year's river tolls."),
        ["codAndHerring"] = (
            "Cod is a green-brown fish up to 2 m of the shelf down to 600 m, a major fishery and the source of cod liver oil. Herring is a small silver fish in schools several miles long, eaten for centuries.",
            "Northmen are herring fishers. A bad herring year empties the fjords of young men."),
        ["sturgeonAndEel"] = (
            "The sturgeon is a long-snouted fish with rows of bony plates that runs up rivers to spawn and is caught for its roe. The eel is a snake-shaped fish of rivers and coasts, a popular food.",
            "Sturgeon is the lord's fish on most rivers. Eels pay the rent of the mill."),
        ["tuna"] = (
            "Fish of 0.5 to 2 m and up to 680 kg, metallic blue above and silver below, of coastal and open water down to 1,000 m. A food fish and the object of a large fishery.",
            "Coast towns build watchtowers for the tuna run before they build walls."),
        ["polarBear"] = (
            "Bear of 300 to 800 kg in the male, with translucent fur that looks white over black skin, of pack ice and polar coasts. It waits at the breathing holes of seals. Dangerous to people. Taken for food and medicine.",
            "The Hoarkin hunter and the bear sit at the same holes. Each leaves the other the hole he came to first."),
        ["walrus"] = (
            "Sea beast of 1,200 to 1,500 kg and about 3.2 m with thick grey-brown skin and long tusks, of polar ice and coasts. Bulls fight with the tusks. Taken for food, oil, ivory and hide.",
            "Northmen paid for grain with walrus ivory in the Ashen Years. Hoarkin carve it small and patiently."),
        ["narwhal"] = (
            "Whale of 4 to 5 m, mottled blue-grey, of deep polar water. The male often carries one straight tusk up to 3 m long. Pods join in herds of hundreds. Taken for food, lamp oil and the tusk.",
            "South of the ice the tusk is sold as the horn of the unicorn, and no northern trader corrects the buyer."),
        ["whale"] = (
            "The bowhead is 14 to 18 m long and up to 100 tonnes, black, with the largest baleen of any whale, of cold polar water. It gives meat, baleen and blubber. The bestiary whale opens its mouth and breathes out a sweet scent that draws small fish in.",
            "One whale lights a Northmen fjord for a winter. The harpooner's share is fixed by the oldest law they have."),
        ["muskOx"] = (
            "Barrel-shaped, short-legged ox with a shaggy outer coat and a fine underwool, of tundra north of the tree line. Bulls butt heads in the rut. The underwool is valuable for garments.",
            "Hearthbloods comb the shed wool off the willows and sell it for its weight in silver."),
        ["woollyMammoth"] = (
            "Elephant of 3 to 3.7 m and 6 to 8 tonnes with long dark outer hair, a dense undercoat and thick fat, of cold steppe-tundra. Extinct in our world. Its ivory has been exported from the far north since medieval times.",
            "The Ashen Years killed the southern herds. The last walk the tundra edge, and the Hoarkin name each one."),
        ["greyWolf"] = (
            "The largest wild dog, about 55 kg in the male, white in the far north and grey, brown or black elsewhere. It lives in packs with a strict order. It kills livestock, though losses are often overstated. The bestiary has the she-wolf whelp only in May, when it thunders.",
            "Every herding people has a wolf oath. The Grass riders swear by it, the Midlanders against it."),
        ["brownBear"] = (
            "Bear of 80 to over 600 kg, usually dark brown, of tundra, forest and mountain. It dens for a deep winter sleep. Considered dangerous, though attacks are rare. Taken for hide and body parts. The bestiary has the mother lick her shapeless cubs into form.",
            "Amberbloods hold that the bear is the oldest person in the forest and is not to be named aloud."),
        ["tiger"] = (
            "Great cat with a red-orange coat and black stripes, of forest, rain forest, scrub and marsh. It swims rivers several kilometres wide. It injures people and rarely becomes a man-eater. Its body parts are traded.",
            "Rainkeeper kings hunt it from elephants. A village that loses three people to one tiger is excused the year's tax."),
        ["snowLeopard"] = (
            "Pale grey to cream cat of 1 to 1.3 m with dark rosettes, a long tail and large paws, of steep rocky ground in high mountains. Active at dawn and dusk. It kills livestock and is not known to attack people.",
            "Dwarves call it the ghost of the scree. A hold that finds its tracks at the gate expects a death."),
        ["bison"] = (
            "Wild ox about 2.9 m long and 800 to 1,000 kg, golden to dark brown, of broadleaf and mixed forest with meadows and clearings. It damages crops.",
            "The Grass riders of the west follow bison as their cousins follow horses."),
        ["aurochs"] = (
            "Wild ox about 1.8 m at the shoulder, black, with forward-curving horns, the likely ancestor of cattle. Extinct in our world since 1627. Caesar's uri are a little smaller than an elephant, spare neither man nor beast, are taken in pits, and cannot be tamed. Their horns are rimmed with silver as cups.",
            "A young Midlander lord shows the horns in the hall before he is thought fit to lead, as Caesar says of the Germans."),
        ["cougar"] = (
            "Large slender cat, yellowish to grey-brown, of forest, scrub, desert and mountain. It caches large prey and drags it far. It threatens livestock and now and then attacks people.",
            "The cat nobody sees. Catfolk leave a share of a kill where its tracks cross theirs."),
        ["lion"] = (
            "Tawny cat. The male is maned and weighs about 189 kg. It lives in prides on plains, savanna and scrub. The lionesses raise cubs together. It attacks people and livestock.",
            "Sunlander herders pay Catfolk to watch the herd at dusk, when the lions come."),
        ["leopard"] = (
            "Cat of 17 to 65 kg, tawny with black rosettes, from desert to rain forest. A solitary night hunter that caches its kills. It takes livestock, and an injured one may hunt people.",
            "The leopard skin is the cloak of Sunlander title-holders. It may not be bought, only earned."),
        ["cheetah"] = (
            "Slim long-legged cat of 21 to 72 kg, pale with small black spots, of grassland and desert. It was kept as a hunting animal in antiquity. Herders see it as a pest.",
            "Saltroad princes course gazelle with hooded cheetahs carried on the saddle."),
        ["hyena"] = (
            "Sandy to grey spotted hunter with a massive neck. The females are heavier and rule the clan. It takes livestock and attacks people. Pliny and the bestiary say it mimics the human voice to call victims out and digs up graves.",
            "Noonfolk bury their dead under stones for the reason the bestiary gives."),
        ["africanElephant"] = (
            "The largest land animal: bulls weigh 4.5 to 6 tonnes. Grey creased skin and ivory tusks. Matriarchal herds in savanna, woodland and rain forest. Taken for meat and ivory. It wrecks crops and kills people in raids.",
            "Sunlander hunters' brotherhoods take one bull a year and bury the tusks' worth in gold with the hunt master."),
        ["asianElephant"] = (
            "Elephant of 3.5 to 6 tonnes of which only the male is tusked, of forest, scrub and grassland. It hauls timber and serves in ritual. A bull in musth is highly aggressive. It kills people and ruins crops.",
            "Rainkeeper kings ride to war on elephants and keep an elephant corps."),
        ["rhinoceros"] = (
            "Grey hairless browser of 800 to 1,400 kg with two horns and a grasping upper lip, of grassland, scrub and forest edge. It charges. The horn is sold as medicine and for dagger hilts.",
            "A cup of the horn is said to sweat when poison is poured in it. Kings buy the cup and keep the taster."),
        ["hippopotamus"] = (
            "River beast of 1.3 to 3.2 tonnes with slate-grey skin, of shallow lakes, rivers and swamps. Attacks are sometimes fatal. Taken for meat, tusks and hide, which makes shields and whips. Herodotus calls it the river horse and says its dried hide makes spear shafts.",
            "River traders fear it more than the crocodile: it upsets the boat, and the crocodile only waits."),
        ["wildBuffalo"] = (
            "Very large dark ox with heavy horns in both sexes. Bulls pass 835 kg. It keeps near water in savanna and forest. Taken for meat. It raids crops and gives its diseases to cattle.",
            "The beast that waits for the hunter who wounded it. A buffalo's horns on a hut mean the owner is not to be argued with."),
        ["crocodile"] = (
            "Reptile of 4 to 4.5 m and about 410 kg of lakes, rivers, swamps and brackish water. It kills several hundred people a year and is hunted for meat and skin. The saltwater kind reaches 6 to 7 m. Herodotus has it sacred in one town and eaten in the next, and caught on a hook baited with pork.",
            "Along the desert river a temple keeps a tame one, adorned and fed, and embalms it when it dies, as Herodotus tells."),
        ["python"] = (
            "Snake of about 4.8 m, yellowish to brown with black diamonds, of rain forest, wetland and swamp. It kills by constriction. Hunted for skin and meat. It has killed people.",
            "Bitterbloods keep them for the skins and say a python fed at the new moon never bites."),
        ["jaguar"] = (
            "Cat of 68 to 136 kg, pale yellow to red-brown with black rosettes, often all black, of dense lowland forest, swamp and riverside. It kills livestock and injures people. Hunters call it with hollow gourds.",
            "The black jaguar is the night with teeth. Boughwalkers do not come down while one calls."),
        ["bullShark"] = (
            "Shark about 2.4 m long, grey above and white below, of warm coasts, estuaries, rivers and lakes: it tolerates fresh water. It can injure people. Taken for meat, skin, fins and liver oil.",
            "Shoalfolk divers carry a bone knife for it and hope not to learn if it works."),
        ["komodoDragon"] = (
            "Lizard of over 3 m and up to 165 kg, dull brown-grey, of dry island forest and tall-grass lowland. It attacks livestock and now and then people. The males wrestle upright.",
            "Sailors who land for water on the wrong island bring back the tale of the small dragon. Nobody inland believes them."),
        ["amarok"] = (
            "A great lone beast of the north that roars and hunts by land and lake. In the tale a man kills its young and hides in a cave. The amarok, which sees what is hidden, takes his companion's soul, and he falls dead.",
            "Hoarkin do not hunt alone at night. They say the amarok takes only those who do."),
        ["snowWasset"] = (
            "A winter predator of the far northern logging camps, known from the tales of the lumberjacks. It sleeps the summer away and hunts under the snow in winter.",
            "Trappers blame it for every sprung and empty trap."),
        ["rimeDrake"] = (
            "A wingless white drake that dens in glacier caves of the polar and subpolar mountains. Its breath is a freezing wind.",
            "Hearthbloods say the windcrystal is its breath, frozen in the rock. Miners listen for it before they dig."),
        ["griffin"] = (
            "A beast said to guard the gold of the far north, from which one-eyed men steal it. Herodotus does not believe the tale. Pliny gives the griffin long ears and a hooked bill and calls it a fable.",
            "Grass riders trade for \"griffin claws\" that are the horns of some old beast. Dwarves know which."),
        ["goldDiggingAnt"] = (
            "Ants larger than foxes and smaller than dogs that throw up sand full of gold from their burrows in a sandy desert. Men take the sand at the hottest hour, when the ants are below, and flee on camels.",
            "Saltroad folk sell maps to the ant hills. Nobody who has one has come back rich. Some have not come back."),
        ["steppePegasus"] = (
            "Pliny lists \"pegasi\", birds with the heads of horses, among the fables of the open north. Here: a great crane-legged bird with a horse's head that runs with the wild herds.",
            "A Grass rider who rides one, once, becomes khan. The tale does not say what happens to those who try."),
        ["bonnacon"] = (
            "A wild ox with a horse's mane and horns curled back so far that they cannot wound. In flight it throws out dung that burns over a great distance.",
            "Its hide is the best in the grassland. The first tanner to bring one in was not thanked by his street."),
        ["ryeWolf"] = (
            "The spirit of the corn as a wolf. When wind moves the standing grain, peasants say that the wolf goes through the corn. Children are kept out of the fields by it, and it sits in the last sheaf, which the reapers shun.",
            "Since the Ashen Years no Midlander cuts the last sheaf without leaving bread for the wolf."),
        ["unicorn"] = (
            "A beast with the body of a horse, the head of a stag, the feet of an elephant and one black horn. It can be killed and never taken alive.",
            "Every court has a horn. Most are narwhal tusks. The one true horn in a region is the reason for its oldest feud."),
        ["hercynianStag"] = (
            "An ox in the shape of a stag with a single horn between the ears, taller and straighter than any known horn and branching at the top like a palm.",
            "Elves hold that each great forest has one and dies with it."),
        ["werewolf"] = (
            "A person who becomes a wolf. Pliny tells of a man who lived as a wolf for nine years, and does not believe it.",
            "It began in the Ashen Years, the tales say, with the first family that ate what it should not have."),
        ["willOTheWisp"] = (
            "A goblin fire that dances over marshy ground and leads the late traveller into bogs and pools. It waits when he stops and moves when he follows. Draining the bogs starves it.",
            "The peat under it burns with a cold blue flame, and a mage pays for it by the basket."),
        ["waterHorse"] = (
            "A horse-shaped thing that haunts a lake. Yeats tells of a farmer who dragged his lake with nets and then tried to pump it dry to be rid of one.",
            "It stands saddled by the ford at dusk. Seven children can sit on its back, and there is room for one more."),
        ["hercinia"] = (
            "Birds of a great northern forest whose feathers shine like fire in the night. Pliny has nothing more to tell of them.",
            "A feather lights a path for one night. Charcoal burners follow the flock home and sell the feathers they find to the lost."),
        ["squonk"] = (
            "A shy creature of the hemlock woods in the tales of the lumberjacks. It has an ill-fitting warty skin, weeps as it goes and dissolves into tears when it is caught.",
            "A sack that held a squonk holds salt water in the morning. Brinefolk find this very funny."),
        ["wapaloosie"] = (
            "A small tree-climber of the wet coast forests in the tales of the lumberjacks. It climbs the tallest trunks to eat shelf fungus.",
            "Mittens of its skin climb the tent pole by themselves, the loggers say."),
        ["salamander"] = (
            "The real fire salamander is 15 to 25 cm long, black with yellow or orange spots, lives in shady woodland near streams, and sprays a toxin. Pliny's is spotted like stars, comes out only in great rain, and is so cold that it puts out fire. The bestiary gives it the strongest of poisons: it taints every apple on a tree it climbs.",
            "Smiths keep one in the quench trough, or say they do."),
        ["caladrius"] = (
            "A white bird that knows whether a sick man will live. If he will, it looks him in the face, takes the sickness into itself and flies towards the sun, which burns it away. If not, it turns its head aside.",
            "A court that owns one hides it. The price of a healer falls when the bird is known to have looked away."),
        ["basilisk"] = (
            "A small serpent with a pale star-like mark on its head. Other snakes flee its hiss, and its touch and breath kill plants. Only the weasel can kill it. The bestiary adds that its glance kills a man and burns a bird out of the air.",
            "Dead ground in a green valley is a basilisk's doing. Duskreaders hunt it blindfold, by its cold."),
        ["amphisbaena"] = (
            "A serpent with a second head on its tail that runs either way with its body in a hoop. Its eyes glow like lamps, and it is the first snake out in spring.",
            "Its skin, wound on a staff, is the well digger's charm: it finds the way down and the way back."),
        ["phoenix"] = (
            "A bird like an eagle, red and gold, that comes once in five hundred years carrying its dead father in an egg of myrrh. Pliny has it build a nest of cinnamon and frankincense and die on it, and the new bird rise from the remains. One at most in a region.",
            "Nobody has seen it. Every incense merchant has a cousin who has."),
        ["wingedSerpent"] = (
            "Small snakes of many colours with bat-like wings that guard the frankincense trees and are driven off only with the smoke of storax. They fly towards the river land in spring and are killed by the ibis. The bestiary's kind outruns horses and kills before the bite is felt.",
            "The price of incense is the price of the smoke that buys a day among the trees."),
        ["cactusCat"] = (
            "A cat of the cactus country in the tales of the lumberjacks. It has a thorny coat and slashes cactus to drink the fermented sap.",
            "It sings on the nights it is drunk. Caravan guards are forbidden to join in."),
        ["roperite"] = (
            "A runner of the dry foothill scrub in the tales of the lumberjacks. It snares hares with a rope-like beak.",
            "Shepherds find their dogs hobbled at dawn and know what passed."),
        ["hillDrake"] = (
            "A wingless dragon the size of a cart ox that dens in dry sandstone hills and hunts by ambush. It is the small remnant of the great dragons whose bones lie in the same rock.",
            "Dwarves mine dragonbone only after buying the hill from its drake, with a goat a week."),
        ["catoblepas"] = (
            "A sluggish beast whose head is too heavy to lift, so that it hangs to the ground. Anyone who meets its eyes dies.",
            "Herders water their cattle at noon, when it sleeps. Noonfolk are hired to stand watch."),
        ["manticore"] = (
            "A beast with a man's face, a lion's body, a tail that stings like a scorpion's, three rows of teeth and a hunger for human flesh. Pliny has it from Ctesias.",
            "It calls travellers by name from the scrub in a voice like a flute. The right answer is to keep walking."),
        ["leucrocuta"] = (
            "A beast the size of an ass with a lion's body, a badger's head and one ridge of bone for teeth, that copies human speech. The bestiary makes it the young of a hyena and a lioness.",
            "It learns the names of a camp by listening at night. Sunlander herders answer only to a name they chose that day."),
        ["yale"] = (
            "A black beast as large as a horse with an elephant's tail, a boar's jaws and long horns that it can swing at will: it fights with one and keeps the other folded back.",
            "A pair of yale horns is the best bow a Sunlander smith can mount, and the hardest to match."),
        ["dragon"] = (
            "A crested serpent of the hot lands whose strength is in its tail. It lies on the paths of elephants, knots itself round their legs and strangles them, and the two often die together. One at most in a region.",
            "Pliny says the red pigment cinnabar is the blood of a dragon crushed under a dying elephant. Painters pay for it as if that were true."),
        ["roc"] = (
            "A bird like an eagle and of enormous size, of the islands of the southern sea. In Marco Polo's account it seizes an elephant in its talons, carries it up and drops it to break it read in a search excerpt of Yule's edition only.",
            "Shoalfolk pilots mark its islands on no chart and steer by where the charts are blank."),
        ["cinnamonBird"] = (
            "A great bird that builds its nest of cinnamon sticks on a crag. The gatherers lay out joints of ox and ass. The birds carry them up, the nests break under the weight, and the sticks fall. Pliny has the nests shot down with leaded arrows, and calls all such tales lies told to raise the price.",
            "The spice guilds keep the tale alive, as Pliny guessed. Now and then a real nest is found, and that keeps it alive too."),
        ["scitalis"] = (
            "A serpent whose back glitters with so many colours that the onlooker stands dazed. It is slow and takes those it has stunned. It runs so hot that it sheds its skin even in winter.",
            "Boughwalker painters grind the shed skin for a colour that only they can see."),
        ["javelinSnake"] = (
            "A flying serpent that throws itself from a tree onto whatever passes below and kills it.",
            "Forest porters carry their loads on their heads for this reason, they say, and not for comfort."),
        ["hydrus"] = (
            "A water snake of the great river. Pliny tells of the small enemy of the crocodile that slips into its open mouth and gnaws out through the belly. Here the two are one creature.",
            "Fishers free a hydrus from the net with an apology. The crocodile is everyone's enemy."),
        ["stormCrane"] = (
            "A tall grey bird that follows thunderheads over open plains and nests where lightning has fused the soil to glass.",
            "Widegaze see the flock a day before the storm and move the herds."),
        ["canopySpinner"] = (
            "A spider the size of a dog that webs the gaps between the tallest trees and takes birds and monkeys.",
            "Boughwalkers cut the old webs for rope bridges and leave the spinner a parrot."),
        ["moonOyster"] = (
            "A giant shell of warm shallow water that opens only under a full moon and closes on whatever reaches in.",
            "A Shoalfolk diver who brings up a moonpearl may name the price of the family's reef."),
        ["serra"] = (
            "A sea monster with huge wings that races a ship under sail for three or four miles, tires, folds its wings and is carried back to the deep.",
            "Sailors bet on the race. The serra has never won, and they say that is why the sea is still crossed."),
        ["stayShip"] = (
            "The real sharksucker is a slim fish up to 1 m that rides on sharks and whales by a sucker on its head, and is used as living bait on a line. Pliny's echeneis clings to a keel and holds the ship back.",
            "A becalmed captain has the hull scraped before he has the crew flogged."),
    };
}
