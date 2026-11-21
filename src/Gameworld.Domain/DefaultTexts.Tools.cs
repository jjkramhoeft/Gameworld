namespace Gameworld.Domain;

// What each default tool is and its lore: the parts "Description" (without the source) and "Lore" of docs/tools-basis.md.
// A kind that has no text here gets DefaultTexts.Missing.

public static partial class DefaultTexts
{
    internal static readonly Dictionary<string, (string Description, string Lore)> Tools = new()
    {
        ["heavyPlough"] = (
            "A heavy plough carries an iron coulter, a vertical blade mounted like a knife that slices the soil ahead of the share. It cuts long narrow strips and deep furrows (ridge and furrow), which lets heavy, poorly drained soil be farmed, and it was pulled by teams of up to eight oxen. An iron coulter excavated at Lyminge in Kent is dated to the first half of the 7th century.",
            "Midlanders say the heavy plough is the one thing the Old Realms left that still does what it was made for. Eight oxen are more than one farm owns, so a plough team is shared, and half the quarrels of a river plain start over whose strip it turns first. The Catfolk will guard the grain but never walk behind it."),
        ["sickle"] = (
            "A curved, hand-held iron tool whose cutting edge is the inside of the curve and is serrated. It harvests cereal crops or cuts grass for hay: the blade is drawn against the base of the stems and cuts them with a sawing action. The Pitt Rivers Museum holds English examples, among them an ancient socketed iron sickle.",
            "On the reservoir lands of the Rainkeepers the sickle is handed down with the field, and a child is counted as grown the first harvest it keeps pace with the row. Swiftborn harvest hands hire out by the sickle and bring their own: they have too few seasons to waste one on a borrowed blade."),
        ["flail"] = (
            "A wooden staff with a shorter stick, the swingle or swipple, hung from it by a leather hinge or thong so that it swings freely. It is used to thresh corn by hand and separate the grain from the husks. The Pitt Rivers examples are English, the one dated example from Sussex in 1894.",
            "Threshing is winter work, done on the barn floor when the fields are shut. The Flinchless thresh in pairs and stop at a fixed count, because a man who cannot feel the swingle catch his own head must be watched by another."),
        ["scythe"] = (
            "A long steel blade fastened by a tang at its heel to a shaft (the snath or sned), usually of willow or ash, with two hand grips. The mower keeps the heel of the blade close to the ground and the toe a little above it, and a good mower cuts about an acre a day. Good mowing depends on whetting the blade often with a stone.",
            "Hay harvest is when a farm hires, and a day labourer with a scythe and a whetstone of his own is paid half again what a field hand gets. The Noonfolk mow from first light and lay the scythe down where they stand at sunset, so that nobody trips on it in the dark they never see."),
        ["spadeIron"] = (
            "Medieval spades generally had wooden blades, sometimes fitted with a separate iron cutting edge. A U-shaped spade iron from Wick in the Vale of Glamorgan is 274 mm long and was held to the wooden blade by a groove along its inner edge and two lugs. In 1356 the stock of a London ironmonger included a \"spadierne\", a spade iron.",
            "Iron is too dear to make a whole spade of, so only the edge is shod and the smith re-lays it when it wears thin. A Halfling garden plot is dug with a spade cut to its owner's height, and lending it is a mark of friendship that is noticed."),
        ["beeSkep"] = (
            "A domed basket hive of woven straw, about 14 inches across, with willow strips inside to strengthen the dome. The bees fix their combs directly to the walls. Wicker hives of privet or hazel daubed with manure and clay were also used. Domed straw skeps were typical in the 16th century.",
            "Honey is the sugar of every land too cold for cane, and a row of skeps under the orchard wall is a Halfling household's savings. The Stillfaces keep bees without smoke: they say a hive tells by its scent when it may be opened."),
        ["billhook"] = (
            "An iron blade with a hooked end set on a wooden handle, used for cutting branches. Museum Wales holds an excavated iron billhook from Radyr near Cardiff with an iron collar for its handle, catalogued as medieval or post-medieval. The record notes that the form has changed little since Roman times.",
            "The vine dresser, the hedger and the firewood cutter carry the same hook, ground to three different edges. An Amberblood grove warden will let a stranger carry a billhook into the trees, but not an axe."),
        ["shepherdsCrook"] = (
            "A wooden shaft with a metal hook. The Food Museum example is 129 cm long and was used by a shepherd on a Suffolk farm from 1855.",
            "A Midlander shepherd is hired at the autumn fair by standing with his crook in his hand. The Widegaze range herders carry one too, though they rarely need to walk to the sheep to know which one is missing."),
        ["ironShears"] = (
            "Iron shears with two long blades and a round knob at the end of the handle, 137 mm long, from London, early 15th century. A manuscript of about 1475-1525 (British Library, Harley 1892) shows a woman shearing a sheep with shears, and another (Harley 6563) a tailor cutting out a coat with them.",
            "Wool is the fibre of the cold and temperate lands, and the shearing of early summer brings the whole household to the fold. A tailor's shears are smaller and kept sharper, and in a town of the Canal folk they are weighed with the cloth so that no customer pays for what was trimmed away."),
        ["plungeChurn"] = (
            "A tall coopered oak barrel bound with iron bands, tapering from the base to a loose collar. A plunger on a long wooden handle is worked up and down and turned to churn the cream to butter. Museum Wales says the type was common from the 1600s to the late 1800s.",
            "Butter and cheese are how milk keeps through winter, and the dairymaid's churn song sets the pace of the plunger. The Hearthbloods trade for butter by the tub: a people who must feed the fire inside them twice a day do not haggle long over fat."),
        ["seineNet"] = (
            "A seine is a fishing net weighted at the bottom and floated at the top, cast into the water and hauled in from the bank or a boat. 1,109 lead-alloy sinkers from such a net were found in a 15th-century wreck in the Thames at Blackfriars. The net is calculated at about 56.5 m long and needed several people to handle.",
            "A seine is too long for one family, so the net belongs to the boat's company and the catch is split by shares. The herring nets of the Northmen are barked brown with tree tannin each spring, and the Shoalfolk knot theirs of bast that does not rot in brackish water."),
        ["eelTrap"] = (
            "A trap woven from green willow, with a mouth cone and a second, narrower inner cone that the eel must pass to reach the bait. The far end is closed with a stopper. Small traps (grigs) were laid at dusk, weighted with stones and baited with small fish or fish gut. Large ones (bucks) were mounted on stages across a river to take migrating eels. The Luttrell Psalter of about 1330 shows a wicker eel trap.",
            "Eels are rent in many river lands: a mill or a weir owes its lord so many sticks of them a year. The osier for the trap is cut from the same willows that hold the bank."),
        ["eelSpear"] = (
            "A hand-forged iron spear head with several barbed prongs, thrust into river mud to catch eels. The London Museum example is 430 mm long, came from the Thames foreshore and is dated to the 17th or 18th century. The record notes that eel spears were officially illegal.",
            "Where the fishing of a river is let to one holder, the spear is the poor man's tool and the water bailiff's grievance. The Hoarkin use a longer one through a hole in the sea ice, and have no bailiff to ask."),
        ["boarSpear"] = (
            "A heavy hunting spear with a steel head on a wooden shaft. A German example of about 1550 in the Metropolitan Museum is 220 cm long overall, with a head of 33 cm, and weighs about 1.7 kg.",
            "The wild boar of the broadleaf forest is game for a lord and a danger to a farm, so the spear hangs in halls and in barns alike. Where the Supernatural runs high the same spear is carried against worse, by the monster hunters who are paid by the head."),
        ["falconsHood"] = (
            "A snug leather hood placed over a falcon's head to train the bird and to keep a trained falcon quiet and still on the falconer's glove. Most have a plume on top that also serves as a handle for taking it off. The V&A example, early 17th century and probably English, is tooled and gilded leather with embroidered velvet.",
            "The Grass riders fly eagles from the saddle and hood them in felt and horsehide. At a Midlander court the falconer ranks above the huntsman, and the hood of the lord's best bird is worth more than the falconer's coat."),
        ["huntingHorn"] = (
            "A horn carved from an elephant tusk, called an oliphant. The V&A example is 54.5 cm long, from southern or central Italy, of the first half of the 11th century. Its carved bands show a stag, a deer, dogs or wolves, mounted hunters with hawks and a mounted man blowing a horn. The museum takes this piece to be probably ceremonial.",
            "A plain hunter's horn is a cow's. A lord's is ivory traded up from the Sunlander savanna or walrus tooth bought from the Hoarkin, and each call on it has one meaning that every beater knows."),
        ["longbow"] = (
            "A bow made from a single piece of yew, 1.84 to 2.11 m long, mostly D-shaped in section. The draw weights are estimated at 65 to 175 lb. 172 were recovered from the Mary Rose, with over 2,000 complete arrows, mostly of poplar. They are the only English longbows that can be dated precisely.",
            "Yew is a timber of the map, and a kingdom that depends on archers buys staves from wherever it grows. Elven and Amberblood bowyers are counted the best, and the same bow that wins a lord's battle takes his deer on the way home."),
        ["fellingAxe"] = (
            "The Science Museum Group holds two medieval iron axe heads dated to the 14th or 15th century, catalogued among its hand tools.",
            "Timber is the first resource a new settlement spends, and the woodcutter's axe is what it spends it with. Elves count the trees that fall to it, and the Amberblood wardenships hold that one who cuts without leave owes the grove a life: usually not his own, but a sapling tended for twenty years."),
        ["twoManSaw"] = (
            "A long saw worked by two men to cut timber lengthwise. The top sawyer stood on the log and guided the saw, and the bottom sawyer pulled it from underneath. In the Middle Ages the log was raised on trestles, as medieval drawings show. Sawpits were in use by the early 16th century and in time replaced trestles on well-drained ground.",
            "The top sawyer is the master and the one below takes the sawdust in his eyes, which is why \"under-sawyer\" is an insult in most timber towns. Shipyards and great halls keep a pair of sawyers the year round."),
        ["minersIronAndHammer"] = (
            "Agricola describes iron tools of one shape, broad and square at the top to take the hammer and pointed at the bottom, most with an eye for a wooden handle. The one in daily use is three quarters of a foot long. The miner holds the handle in one hand, sets the point against the rock and strikes the head with a hammer. Small hammers are held in one hand, larger ones swung with both.",
            "Crossed, the iron and the hammer are the badge on every Dwarven mine gate and on the seal of every mining contract kept behind it. A Dwarf is buried with the iron he was given as an apprentice, worn to half its length."),
        ["wedgeCrowbarAndPick"] = (
            "Agricola's wedge is driven into cracks in the rock with the largest hammer. His crowbars are of two kinds, both pointed at one end: a round one to pierce through to a flooded shaft and a flat one to knock out rock that fire has softened. The miner's pick is pointed, unlike the peasant's wide one, and digs the softer ore.",
            "Hard rock is first cracked with a wood fire and then prised out with wedge and bar, so a mine eats timber and fuel before it yields any ore. The Hearthbloods work the high winter mines that no one else can stand, and are paid in food."),
        ["windlass"] = (
            "A wooden barrel resting in iron sockets on posts set over the shaft, turned by cranks at its ends. A rope wound round the barrel has an iron hook at each end for the bail of a bucket. Two strong men turn it to raise buckets of ore, earth or water. A second form adds a wheel, which can carry lead weights to keep it turning.",
            "Windlass work is where a mine puts its hired hands and its greenest apprentices: Orc quarry hands and porters are sought for it. A well digger in the dry lands of the Saltroad folk uses the same machine and calls it by the same word as the well."),
        ["diviningRod"] = (
            "A forked twig, usually cut from hazel. Agricola (1556) reports that dowsers used hazel for silver, ash for copper, pitch pine for lead and tin, and rods of iron or steel for gold. The dowser grips the two branches in clenched fists and walks over the hills, and the twig is said to turn when his feet are over a vein. Agricola is sceptical: he puts the motion down to the holder and concludes that a forked stick is of no use to a prudent miner.",
            "Dwarven prospectors read the rock and scorn the rod in public. The well-finders of the Duskreaders, who see warmth as others see light, carry one mostly so that the customer has something to watch."),
        ["quarryWedges"] = (
            "Small blocks were split by hammering in wedges, the large wedges driven with sledge hammers. For larger pieces, shallow holes were drilled about 23 cm apart, split iron rods (feathers) dropped in and iron wedges (plugs) driven between them to give a clean break. Edinburgh records of October 1531 note old picks and mattocks remade into a mattock and six new wedges.",
            "A quarryman reads the bed of the stone before he sets a wedge, and a line of wedges struck in turn rings higher just before the block lets go. The Noonfolk quarry by daylight only and leave the wedges standing in the stone at dusk."),
        ["quarryPick"] = (
            "An iron pick used, with mattocks, to uncover the stone and to lever blocks off the bed. Edinburgh's Masters of Works paid in July 1530 for sharpening three dozen picks at two quarries. In Bath stone quarrying a long-handled pick cut a slot along the top of the bed so that a saw could enter.",
            "Picks blunt in a day on granite, so every quarry of any size feeds a smith who does little else but draw out their points. Millstone cutters work with a lighter pick and are paid by the finished stone, not by the day."),
        ["crowbarAndSledge"] = (
            "Crowbars or picks were used to lever the stone off its bed so that it could be lifted with a crane, the cranes at first worked by men or horses. Sledge or striking hammers drove the large splitting wedges. The Edinburgh records note repairs to heavy hammers, picks and crowbars.",
            "The sledge is quarry-hand work, and an Orc who swings one well can find a place at any stone face between the steppe and the sea. Marble and alabaster are levered, never struck, and the men who do it say so often."),
        ["leadSaltPan"] = (
            "A shallow pan of lead in which brine is boiled over a fire. By the 14th century the pans were made of sheets of cast lead beaten up at the sides, about 170 cm by 90 cm. Lead pans were used in Cheshire for some fifteen centuries and gave way to iron pans in the 17th century, when coal replaced wood as the fuel. Scale was picked off the base with an iron tool and a damaged pan was recast.",
            "A boiling house is counted by its pans, and among the Brinefolk a pan master answers for every one of his by name. The lead comes from the same veins as the silver, so a salt town and a mining town are often bound by one old contract."),
        ["saltRakeAndShovel"] = (
            "In Agricola's account of making salt from sea water, the salt that forms in the basins is raked together with wooden rakes and thrown out with shovels.",
            "The rakes are wood because brine eats iron within a season. A salt raker of the cold desert flats works barefoot on the crust, and the Brinefolk hold that salt raked by one who has wept into the pan must not be sold."),
        ["saltBasket"] = (
            "In Agricola's boiling house the helper sets two baskets, two feet high and as wide at the top and a palm wide at the bottom, on beams. The master throws the salt into them with a shovel, and in the baskets it takes the form of a cone.",
            "The cone of salt that comes out of the basket is the unit of the trade: caravans of the Saltroad folk count their load in cones, and a weigh-master's stamp goes on the flat end."),
        ["millstones"] = (
            "A pair of grinding stones. The runner stone is the one that turns: it is very slightly concave, with a central eye where the grain enters. The faces carry large grooves (furrows) and flat areas (lands). The furrows feed the grain out onto the lands and let air cool the stones.",
            "Good millstone is a variant of the Stone resource and travels far: a pair is the costliest thing in a mill after the wheel. Lords who own the mill forbid hand querns, and the miller who takes his toll of every sack is the least trusted man of the countryside."),
        ["millBillAndThrift"] = (
            "A hand tool for dressing millstones, which is re-cutting the grooves in the grinding faces. The steel head (the bill), somewhere between a pick and a chisel, is held in a wooden handle called the thrift.",
            "A stone dresser travels from mill to mill, and millers ask to see his hands: the backs of them are blue with steel splinters if he has done the work he claims. The Flinchless are barred from the trade by their own law, since they would not feel the splinters go in."),
        ["rotaryQuern"] = (
            "A hand mill of two stones. The upper stone has a funnel-shaped hole at its centre for the grain and a hole in the side for a handle. Querns were mainly used to grind grain by hand into flour for bread. The London Museum example is Roman, 31 cm across.",
            "Where there is no stream for a mill, on the steppe and the dry scrub, flour is still ground by hand each morning. In a milled land a quern in the house is either a memory or a crime."),
        ["bakersPeel"] = (
            "A flat shovel used to get bread and other goods in and out of the oven. It is the device on a farthing trade token issued by a London baker in 1659.",
            "The keeper of a common oven bakes the dough of the whole street and knows every household by the mark pricked in its loaf. A peel hung over a door is the baker's sign in towns where few can read."),
        ["kneadingTrough"] = (
            "The ark (or hutch) is an oak chest used to store grain, meal or bread from the medieval period onwards. Its concave top could normally be lifted off and turned over to form a kneading trough for making bread. The V&A example is English, of panelled oak, 1550-1650.",
            "A baker's trough is never washed out fully: the scrapings carry the leaven from one day's batch to the next, and a trough of a good line is part of a daughter's dowry. Halfling bakers name theirs."),
        ["mashTunAndMasher"] = (
            "The mash tun is the vessel in which the mash is made: oak tuns were built by a cooper for a university recreation of 16th-century brewing in Dublin. The mash is mixed with a wooden masher. The Food Museum example is a five-barred wooden frame on a shaft 124 cm long.",
            "Brewing is by-work on most farms and the alewife's own income. Dwarves brew behind the gate from grain seven years stored and hold that a masher must never be lent, as it carries the house's ale in its wood."),
        ["brewingKettle"] = (
            "A large kettle set over an open fire, in which the brewer stirs the brew. The oldest picture of a brewer in the Mendel housebook of Nuremberg (1425) shows a brewing kettle with two rings on its sides and two wooden vats beside it. The Dublin recreation of 16th-century brewing used a large copper boiler made by coppersmiths.",
            "The copper is the brewer's capital: a town brewer is taxed by its size, and the official's sworn measurer comes with a rod to gauge it. In the north bog myrtle goes into the kettle, further south hops."),
        ["alembicAndCucurbit"] = (
            "A two-part still. The cucurbit, a gourd-shaped vessel, holds the substance and is heated. The vapour rises into the alembic set on top, condenses on its inner surface, runs into a channel and out through a spout. A ceramic alembic from London of about 1240-1400 is 28 cm high, and the record says apothecaries used such vessels to make healing waters. A glass set from the Middle East is dated 901-1200.",
            "The distiller draws spirit from wine and the perfumer rose oil from petals with the same vessel, and the Bitterbloods draw the antidotes they sell. The vial blowers of the Lanternkin make alembics of clear glass so thin that the work inside can be watched."),
        ["cleaver"] = (
            "A large knife or cleaver with a broad, straight-backed iron blade and a wooden handle that widens toward the end. The London Museum example is late 15th or early 16th century, 31 cm long and 7 cm wide. The record links it to jointing, carving and slicing meat, fish and pies.",
            "Butchers are kept to one street and the downstream end of town, and their guild is among the richest. The Catfolk, who cannot eat grain, are butchers and tanners wherever they settle, and an Orc butcher is said to waste nothing at all."),
        ["poleAxe"] = (
            "A long-handled axe with a steel head on a wooden handle, used to fell or stun animals at slaughter. The Science Museum example is English, 1880-1925.",
            "The autumn slaughter, when the beasts that cannot be fed through winter are killed and salted, is the slaughterer's season and the salter's. One clean blow is the measure of the trade."),
        ["charcoalClamp"] = (
            "A stack of about three tons of timber built round a central chimney and blanketed in hay, turf and green branches to control the air. It is lit down the chimney and tended day and night for about 24 hours, with gaps in the covering mended, so that the wood cooks at around 400 degrees and does not burn. It is then sealed with earth to smother the fire, cooled and opened. The museum says the practice reaches back to the Bronze Age.",
            "A collier lives in the wood beside his clamp for the whole burn and sleeps on a one-legged stool, so that he falls over if he nods off. Smelters and smiths need charcoal more than they need ore, and the Elves hold the collier in deeper dislike than the woodcutter."),
        ["corrackAndLongShovel"] = (
            "The charcoal burner's rake, known as a corrack, is used to rake out the clamp at the end of the burn. A long-handled shovel is used to tend the dome of the clamp. Other kit includes an axe, a water barrel with buckets and a yoke, a basket for the charcoal and sacks. The evidence is from the Wyre Forest in Worcestershire and undated.",
            "The long shovel patches the turf where the clamp breathes, and the collier reads the colour of the smoke to know where. Duskreaders see the heat through the cover itself and are hired as kiln readers for the largest burns."),
        ["limeKiln"] = (
            "A structure used to make lime by burning limestone or chalk, and now and then shells, at more than 900 degrees. Lime burning seems to have come to England with the Romans. Most kilns supplied lime for the mortar of a building beside them.",
            "Every castle, temple and town wall begins with a lime kiln at its foot, and the lime burner is the first craftsman on the site and the first to leave. The chalk and limestone lands sell lime to the granite lands that have none."),
        ["bloomeryFurnace"] = (
            "A furnace built of clay, sometimes also stone, with thick walls to hold the heat and an inside diameter of 0.3 to 1 m at the base, fuelled with charcoal. Air is blown in through holes in the side, sometimes through clay tubes (tuyeres). The iron does not melt: it sticks together as a spongy lump, the bloom, and in some furnaces molten slag is tapped off near the ground. The bloomery was in use in England from about the 8th century BC and gave way to the blast furnace from the late 15th century.",
            "The Sunlanders were the first to smelt iron, and their furnace masters still open a smelt with words that no outsider is taught. Bog iron in the cold forest, ironstone on the savanna: the furnace is the same clay shaft, rebuilt for every few smelts."),
        ["bellows"] = (
            "Bellows blow air into furnaces and hearths to reach the heat needed. In the smith's forge a pair is set behind the hearth with its nose in the tuyere of the back plate. One board is fixed, and a rocker with a handle raises the moving board, which a weight brings down again. From about the 14th century water power began to drive the bellows of bloomery furnaces.",
            "The forge hand at the bellows is the lowest place in a smithy and the one every master started in. Hearthbloods are welcome there in any land: they do not mind the heat, and they are fed at the forge."),
        ["cupel"] = (
            "A small dish of bone ash with a shallow hollow. In cupellation the metal is melted with lead, which oxidises to litharge, takes up the base metals and soaks into the dish, leaving the precious metal. Small cupels were used in fire assay to measure the precious metal in a sample of ore or metal. A London example of the 15th or 16th century is 37 mm across and still holds droplets of silver.",
            "Silver comes out of lead ore only by this dish, so the silver refiner and the assayer are the same craft at two sizes. Gnome assayers stamp the bead as well as the bar, and a Lanternkin banker will not take a bar without the bead beside it."),
        ["unhairingKnifeAndBeam"] = (
            "After the hides have lain in lime pits to loosen the hair, fat and flesh, the hair is scraped off with a blunt-bladed unhairing knife, curved to fit the rounded beam over which the hide is laid. The example from the Rhayader tannery is 72 cm long. The hair was sold to plasterers and masons for mortar.",
            "The beam man stands bent over wet hide all day, and the trade is known by its stoop and its smell. Tanners are set downstream and downwind of every town, beside the butchers who supply them."),
        ["tanPits"] = (
            "Pits in which raw hides are soaked in an increasingly strong solution of ground oak bark. The whole process took eighteen months. The Rhayader tannery, of the late 18th century and now at St Fagans, ground its bark in a water-powered bark mill.",
            "A hide spends a year and a half in the pits, so a tanner's wealth is always under water and a tannery passes from parent to child half full. Tanbark is a by-product the forester sells; the Brinefolk tan with salt and alum of the dry lake beds."),
        ["lunellumAndHerse"] = (
            "After liming, unhairing and fleshing, the skin is fixed to a frame, the herse, and dries under tension. Once stretched it is thinned and evened by scraping with a curved knife called a lunellum, until it is fit for writing or binding.",
            "Every contract of the Dwarves, every genealogy of the Halflings and every ledger of the Saltroad folk begins as a calf or a goat on a herse. The Canal folk write on paper and regard parchment as a waste of a good animal."),
        ["spindleWhorlAndDistaff"] = (
            "A wooden drop spindle carries a pierced weight, the whorl, whose weight keeps the spindle turning while the fibres are twisted into yarn. Whorls are of lead, stone, ceramic, bone, antler or wood. The technique runs in Britain from the Iron Age to early post-medieval times. In medieval Scandinavia a distaff with notches along its upper part held the fibre.",
            "Spinning fills every idle hour of every woman of the wool and flax lands, and it takes several spinners to keep one loom fed. A whorl is the commonest thing a traveller finds in an abandoned house, and the Twofold bury the one a person spun with in the Staying half of life."),
        ["loom"] = (
            "The warp-weighted loom has two uprights about 2.2 m high carrying a beam, with a shed rod and one to three heddle rods. The warp is held taut by loom weights, in Norway mostly of soapstone. It was the common loom for everyday cloth in Iceland and Norway. The horizontal loom with treadles is documented in 11th-century France and seems to have reached Sweden around 1200.",
            "The upright loom stands in the farmhouse and the treadle loom in the town workshop: the first makes a household's cloth, the second a guild's. The Rainkeepers weave cotton, the Canal folk silk, and the Boughwalkers paint finished cloth in a colour nobody else can see."),
        ["croppingShearsAndTeasels"] = (
            "To finish woollen cloth, a tool set with teasels raises the nap. Then large, heavy hand shears are worked across the cloth, spread on a cropping table, to shave the surface smooth, and a hand brush sweeps off the loose wool. The workers were the croppers or shearmen.",
            "The shearman is the best paid hand in the cloth trade, for one slip of the shears ruins a bolt that took a month to weave. Teasels are a garden crop, grown beside the dye plants."),
        ["anvil"] = (
            "An iron block whose flat top, the face, is hardened steel, so hard that a file will not touch it. Some anvils have a pointed end, the pike, bickern or beak-iron, for hollow work. The anvil is set on a wooden block about two feet high, or as suits the workman.",
            "An anvil outlasts its smith and is willed like land. Northmen honour Dwarves as the only smiths better than their own, and a Dwarven anvil that has left the hold is either a king's gift or stolen. Star metal, where it falls, is said to mark the face that works it."),
        ["hammersAndTongs"] = (
            "The hand-hammer is light enough to wield with one hand at the anvil. The up-hand sledge is held in both hands and seldom raised above the head, and the about-sledge, the largest, is swung overhead for the heaviest work: both are used by the under-workmen. Straight-nosed tongs hold short, flat work, and crooked-nosed tongs hold small bars within their curved jaws.",
            "The master taps where the blow should fall and the striker's sledge follows. The Flinchless make strikers who never flag, and a Duskreader temper judge stands by to call the heat of the iron by its glow in the dark of the shop."),
        ["carpentersAxe"] = (
            "A deep, heavy axe, heavier than the joiner's hatchet, with a long handle because it is used with both hands. It squares or bevels timbers, which are propped on logs so that the edge does not strike the ground. On the warship Mary Rose axes served both to chop wood and to trim timbers with care.",
            "A beam is hewn square long before a saw touches it, and the marks of the axe show on every rafter of a Northman hall. A carpenter is found in every patch of the countryside, where he is also the wheelwright, the cooper and the coffin maker."),
        ["adze"] = (
            "A tool with a thin, somewhat arched blade whose edge lies across the handle. It takes thin chips off timber or boards and removes what the axe cannot reach. The workman holds the end of the handle in both hands and stands astride the work. On the Mary Rose the adze was used to smooth timbers.",
            "The Northmen say the first Shipwright shaped the sky's planking with an adze, and the stars are where he riveted it. Every curved timber of a hull is brought to its line by this tool, by eye."),
        ["auger"] = (
            "A boring tool whose work is to make great round holes. Moxon shows it used in hanging doors, where the hole is bored slightly smaller than the hook so that the hook is forced in tight. A smaller piercer and a gimlet make the lesser holes.",
            "A timber frame is held by oak pegs, not nails, so a house is as many auger holes as it is beams. Iron nails are for those near a smith with iron to spare."),
        ["plane"] = (
            "A tool for smoothing and shaping wood, made in many sizes. On the Mary Rose large planes smoothed planks and beams and small ones did work such as smoothing arrows, and they were of wood, not metal. Moxon's joiner starts with the fore-plane and finishes with the jointer or the smoothing plane.",
            "The plane tells the joiner of the town from the carpenter of the countryside: one makes chests and panelling, the other barns. Fletchers and bowyers keep planes no longer than a finger."),
        ["poleLathe"] = (
            "A turner's lathe in which the work is held between two centres. A cord is wound two or three times round the work and runs from a springy pole above, often a fir pole fixed to a ceiling timber, down to a treadle. Treading down spins the work and the pole draws the treadle up again. The cutting is done with gouges, chisels and hooks.",
            "The turner sets his lathe up in the wood where the timber is, with a living sapling for the pole, and comes out in autumn with a cartload of bowls and chair legs. Every household eats from his work."),
        ["plumbRuleLevelAndSquare"] = (
            "The plumb rule is a rule about four feet long with a line and a lead plummet, used to keep work upright. The level is ten or twelve feet long and sets out foundations and checks walls as they rise. A large square sets walls at right angles. Without one, the workman measures 6 feet along one wall and 8 along the other and adjusts until the diagonal is 10.",
            "The square and the plumb line are the master's tools and the signs of a masons' lodge. The Gnomes make them in brass for the canal engineers of the Canal folk, who level a dike over a day's walk with them."),
        ["caulkingIronAndMallet"] = (
            "Caulking irons are hand-forged, fishtail-shaped chisels. A making iron tucks rolled oakum into the seam between planks and a grooved hardening iron drives it firm. They are struck with a caulking mallet, which has a long, heavy wooden head bound with metal rings. The seam is then sealed with melted pitch. On the Mary Rose horsehair and rope were hammered into the gaps and sealed with pitch.",
            "A shipyard is heard before it is seen, by the ring of the caulkers' mallets. The pitch comes from the tar burners of the boreal forest, and the Shoalfolk hull menders do the same work under water, by feel."),
        ["clenchNailsAndRoves"] = (
            "Iron fastenings for clinker planking, in which the overlapping planks are fastened edge to edge. A rove is a small pierced metal plate over which the point of the nail is clenched, so that it cannot draw. Riveted planking from boats of the 12th and 13th centuries was excavated at Perth. The Viking Ship Museum's smith makes about 450 rivets and roves for a boat of 7.7 m and just under 8,000 for a longship of 30 m.",
            "The Northmen say the stars are the rivets of the upturned hull that is the world. A ship fellowship that owns a hull in shares counts the shares in rivets, and a new longship keeps a smith at the slip for a whole winter."),
        ["malletAndChisels"] = (
            "The mason's mallet is round, so that it need not be held any particular way and wears evenly. It is heavy but of fairly soft material, since a metal hammer would damage the struck end of the chisel. Bolsters are wide-bladed chisels for clearing waste and dressing flat faces, and narrower chisels carve the finer detail.",
            "A mason cuts his own mark into every block he dresses, and is paid by counting them. Dwarven masons use no mortar where a joint can be cut true, and a Duskreader wall mason works the cool of the night."),
        ["treadwheelCrane"] = (
            "A large wooden wheel, turned by men walking inside it, for hoisting loads. The original wheel survives high in the Bell Harry Tower of Canterbury Cathedral, which was finished in 1498. It is large enough for two men and lifted the stone, bricks and tools into place when the tower was built.",
            "The wheel is left in the tower when the work is done, since nothing but itself could lower it. Walking it is labourer's work, and the Boughwalkers, who have no fear of height, are the steeplejacks who rig it."),
        ["lewis"] = (
            "A lifting cramp for large, heavy blocks of stone. For a chain lewis a narrow slot is cut in the top of the block and two curved pieces are put in back to back. When the crane pulls the chain they bite into the sides of the slot and carry the weight of the block.",
            "The lewis slot stays in the stone for ever, hidden in the joint. Ruins of the Old Realms show slots of a shape no living mason's lewis fits."),
        ["trowel"] = (
            "The bricklayer's trowel is used to take up the mortar and spread it, to cut bricks to length and to stop the joints. Moxon lists a longer and narrower trowel for tiling, and laying, setting and pointing trowels for the plasterer.",
            "Where the land is alluvium and has clay but only field stone, the mason is a bricklayer and the trowel his whole kit. The mud-brick masons of the Saltroad folk plaster with the hand and keep the trowel for the finish."),
        ["pottersWheel"] = (
            "A turning wheel on which pots are thrown. In England a number of wheel-throwing potteries appear in the 9th century, and by the middle of the 13th the wheel was widely used, most of all by the larger producers. Thrown pots show spiral ribbing inside and cutting-off marks on the base. Remains of wheels are rare: a probable kick wheel with a stone flywheel is Roman, and a few medieval manuscripts show potters at the wheel.",
            "Clay is in every river plain, so every town has a potter, but only kaolin makes the fine white ware of the Canal folk that is traded across half the world. Noonfolk potters throw in the open yard and stop when the light goes."),
        ["potteryKiln"] = (
            "A kiln, usually oval or round, with a stoking pit at the front, a flue that holds the fire and an oven above, where the stacked pots stand on a raised floor. The dome was of clay, or of stone lined with clay, with a vent at the top that could be sealed to give grey ware. The fuel was mostly wood or peat. In England such kilns returned in the 9th century and served for pottery and tile through the Middle Ages.",
            "A firing is the potter's whole month at stake in one night, and a kiln master is paid for the pots that come out whole. Duskreaders judge the heat of a kiln through its wall and hire out as kiln readers."),
        ["stampsAndRoulettes"] = (
            "Purpose-made dies, stamps and roulettes (roller stamps) of antler, bone, stone, wood, fired clay or metal, used to press the more complex patterns into the clay. Pieces of potters' tools for shaping and decorating are sometimes found at kiln sites.",
            "A potter's stamp is as good as a signature, and a jar of oil or wine is known across a sea by the band rolled round its shoulder. A stamp cut from dragonbone is a boast more than a tool."),
        ["blowpipe"] = (
            "An iron or steel tube, usually four to five feet long, for blowing glass. It has a mouthpiece at one end and a metal ring at the other to hold the gather of molten glass.",
            "Glass is sand, ash and fire, and the Brinefolk soda burners supply the ash, so the glasshouses of the world stand nearer their salt flats than anyone planned. The Lanternkin vial blowers work the thinnest glass, for vessels that must show a glow."),
        ["pontil"] = (
            "A solid metal rod tipped with hot glass and stuck to the base of a vessel to hold it during the making, after it leaves the blowpipe. Taking it off often leaves a pontil mark on the base.",
            "The rough scar under a cup is where the pontil let go, and a buyer runs a thumb over it to know the cup was blown and not cast. Glass is city work: no town has the custom for it."),
        ["jacksAndShears"] = (
            "Jacks are a tool with two metal arms joined by a spring. The glassworker sets the spacing of the arms to shape the hot glass, for instance the mouth of an open vessel. They are also called pucellas or borsella. Shears trim off excess hot glass during the work.",
            "The master sits at the bench with the jacks and never leaves it: the boys bring the glass to him. A glasshouse that works stormglass or mana crystal keeps its jacks apart from the others, and does not say why."),
        ["grozingIron"] = (
            "A metal slot or hook into which the edge of a piece of glass is slipped and chipped to shape. In medieval stained glass the sheet was first divided by laying a heated iron on the surface, then the rough pieces were shaped with the grozing iron and fitted into H-shaped strips of lead. From the 16th century diamond cutters spread and left cleaner edges.",
            "The glazier travels to the temple or the hall with his glass in straw, and the nibbled edge his iron leaves is hidden in the lead. A coloured window is the costliest wall a patron can buy."),
        ["roundKnife"] = (
            "The medieval shoemaker's knife, the trenket or tranchet, now called the round knife. It appears in nearly all pictures of medieval shoemakers and is the knife of the shoemakers' arms. It is attested in word lists from about 1220 to about 1440, with finds from London, Stockholm and Scandinavia.",
            "Shoemakers are the most numerous craftsmen of any town, and their knife is on more guild seals than the smith's hammer. The saddlers of the Grass riders cut with the same blade and call it the half-moon."),
        ["awl"] = (
            "A pointed metal tool, called elsyn or bodkin in medieval word lists. It is shown in the housebook of the Mendel foundation in Nuremberg and on an altarpiece of 1500-1510, and metal awls are recorded from Greenland (about 1350), Poland and London.",
            "Leather is not sewn but pierced and then laced with a waxed thread and a boar's bristle, hole by hole. The cobbler who mends at a street corner owns an awl, a knife and a last and nothing else."),
        ["shoemakersLast"] = (
            "A wooden model of the foot on which the shoemaker shapes boots and shoes. Lasts are recorded from Hedeby, from a shoemaker's workshop at Bergen (late 12th to early 13th century), from Novgorod, Greenland (about 1350), Stockholm, Lubeck and Warsaw. The author holds that by about 1300 lasts were used by some makers, possibly to shape the uppers.",
            "A shoemaker with many lasts has many customers, and a great house keeps the lasts of its family at the maker's shop. A shop that serves Halflings, Gnomes and Orcs alike needs a wall of them."),
        ["thimbleAndNeedle"] = (
            "A small copper-alloy cap covered in pits, about 2 cm high in a London example of the 15th or 16th century. It protects the finger that pushes the needle through the cloth, and the head of the needle sits in a pit so that it cannot slip. The museum says thimbles came into use in England in the 14th century and were mostly imported, above all from Nuremberg.",
            "Needles are a smith's finest small work and a pedlar's lightest stock, and a seamstress guards hers like coin. A tailor sits cross-legged on the board by the window for the light, and his thimble has no top so that he can push with the side of the finger."),
        ["touchstoneAndTouchNeedles"] = (
            "The touchstone is a black, fine-grained stone. The touch needles are copper needles tipped with gold or silver alloys of known fineness. The metal under test is rubbed on the stone and its streak compared with the streak of a needle. In 1532 a London goldsmith owned fifteen gold-tipped needles, silver needles and a touchstone.",
            "No power of the Scattered Age reaches further than a few weeks' travel, so there are a hundred coinages and the moneychanger's stone decides between them. Gnome assayers carry the needles in a roll, and a Lanternkin notary will witness a touch as readily as a contract."),
        ["coinDies"] = (
            "A pair of iron dies engraved in reverse. The lower die, the pile, has a spike that fixes it in a wooden block. A blank is laid on it and the upper die, the trussel, is struck with a hammer. A pair in the British Museum was used at Durham in the 1350s.",
            "The dies are the ruler's face and are locked up each night by an official who is not the moneyer. A kingdom is the land one court can ride across, and its penny is good about as far."),
        ["drawPlate"] = (
            "A hardened metal plate with a row of tapered holes. Annealed wire is filed to a point, put through a hole slightly smaller than itself and pulled through with tongs, hole after hole, to make it thinner. The first known reference to the draw plate is in the treatise of Theophilus, of the first half of the 12th century.",
            "Gold wire for filigree, silver wire for the arm-rings the Northmen cut up to pay their debts, and truesilver wire, which is said to sing as it passes the plate. Gnomes draw wire finer than hair for their clockwork."),
        ["graver"] = (
            "A tool with a long steel shaft, a narrow sharp point and a wooden handle. Engravers mostly use gravers (burins) with a lozenge-shaped section to cut designs into metal. The cut is a tapered line that swells in the middle.",
            "Every seal, every coin die and every inscribed ring passes under a graver. The Gnomes engraved the measuring rings of the sorcerer-kings with it, and their map engravers still sign in letters too small to read without a lens."),
        ["hessianCrucible"] = (
            "A ceramic melting vessel made in Hesse in Germany from the Middle Ages on, central to the kit of the alchemist and the early metallurgist. It withstood strong reagents and high heat: analysis shows that the makers fired a kaolin clay above 1100 degrees. Examples are found across Europe and in colonial America.",
            "One clay bed in one hill makes crucibles that do not crack, and every alchemist and goldsmith of a dozen regions sends there. Sulphur, quicksilver and saltpetre, the Reagents of the map, are all tried in it."),
        ["grindingSlabAndMuller"] = (
            "Cennini tells the painter to take a slab of red porphyry, a strong and solid stone, a foot or more across and not highly polished: serpentine and marble are too soft. The stone held in the hand is also porphyry, flat underneath and rounded on top. The pigment is crushed on the slab and ground with water for half an hour, an hour or longer.",
            "An apprentice painter grinds colour for years before he is given a brush. Lapis for blue, malachite and azurite from the copper lands for green and blue, kermes and madder for red: the slab is where the Luxuries of half a world end up."),
        ["mineverAndBristleBrushes"] = (
            "Minever brushes are made from squirrel tails: the hairs are tied in bunches, pushed into the quill of a vulture or a goose and fitted on a tapered stick of maple or chestnut. Bristle brushes use the bristles of a white domestic hog, tied with waxed thread to a small stick, and are broken in by whitewashing walls.",
            "Squirrel is one of the furs of the boreal forest, and the trapper sells the tails to the painter separately. The Boughwalkers paint a second picture over the first in the colour only they see, and sell the secret of it to nobody."),
        ["silverStyle"] = (
            "A slender, smooth style of silver, or of brass or anything else with a tip of silver. It draws on a small boxwood panel coated with finely ground burnt bone mixed with saliva. The strokes are laid in very lightly and built up by going over them for the shadows.",
            "A line of silver cannot be rubbed out, so a drawing in it is a proof of the hand, and masters set it as the test for an apprentice. The illuminators of a temple scriptorium lay out a page with it before any colour goes on."),
        ["turnsAndBow"] = (
            "A small frame of brass and steel gripped in a vice, with a spindle that holds a drill or a small part and a pulley for the bow string. The bow is strung with a fine gut band wound once round the spindle. Drawing it back and forth turns the part, so that it can be shaped or drilled.",
            "The Gnomes built the clockwork that the sorcery of the Old Realms needed, on turns no bigger than a hand, and have refused a single master ever since. An artificer is found only in a capital, where a court can pay for a year's work on one mechanism."),
        ["screwPlate"] = (
            "A plate with threading holes of graded sizes, used to form a thread on brass, iron or steel wire and so make screws. Steel taps cut the matching thread inside drilled holes. The Winterthur examples come from a clockmaking shop of the 18th and early 19th centuries.",
            "No two workshops cut the same thread, so a lock or a clock can be mended only by the house that made it, or by someone who has stolen its plate. Fine locksmiths count this an advantage."),
        ["coinBalance"] = (
            "A small balance whose two arms fold flat along a fixed central rod, with pans that hang from loops. A 14th-century example was excavated at a church in Oxford. Coins were worth their weight in silver or gold, so merchants and administrators carried a balance to check that worn or clipped coins held their full value.",
            "A merchant who crosses three kingdoms meets three pennies and trusts none of them without the balance. The Saltroad folk carry theirs in a wooden case at the belt, and weigh gold dust against seeds."),
        ["jettonsAndCountingBoard"] = (
            "Small coin-like counters of brass, about 22 mm across, laid out in rows on a chequered board or cloth as a counting aid like an abacus. They served for nearly all arithmetic in the later Middle Ages and into the 17th century. The English Exchequer took its name from the chequered cloth on which the sums demanded and received were laid out.",
            "Sums are cast on the cloth and only the result is written down, so a bookkeeper who cannot write a line can still keep a great house's accounts. Rainkeeper mathematicians reckon with written figures instead and think the board a toy."),
        ["tallyStick"] = (
            "A wooden stick notched to record a sum and then split lengthwise. The lender kept the longer part, the stock, and the debtor the foil. When the debt was paid the halves were matched to show that they tallied, and the grain of the wood made forgery hard. The English Exchequer used tallies from the 12th to the 19th century to account for taxes. The Science Museum holds sixteen stocks of about 1440.",
            "A tally needs no writing and no witness, only two halves that fit, so it is the contract of those who cannot read. The alehouse keeper cuts one per customer and hangs them over the barrel, and the tax collector's bundle is the most hated firewood in the region."),
        ["steelyard"] = (
            "A balance on which a weight hangs from the long arm. The part that survives is mostly the weight: a London example of the mid to late 13th century is a globe of copper alloy with a lead core, 674 g, bearing the arms of an earl. The museum says steelyards were often used by merchants to weigh goods.",
            "A steelyard weighs a sack of wool with one small weight, which is why stallholders like it and customers do not. The weigh-master of a market keeps a sealed one against which any trader's can be tried."),
        ["standardYard"] = (
            "An octagonal bronze bar, the standard yard made in London in 1495-1497 as the royal standard of length. Copies were issued to boroughs round the country to enforce true measure in trade.",
            "Each free town has its own yard, let into the wall of the market hall for anyone to lay cloth against. The draper's wand is checked against it once a year by the sworn measurer."),
        ["standardBushel"] = (
            "A bronze bushel measure, 71 cm across, made at a king's command between 1495 and 1509 as the primary standard of capacity. It is one of a series issued to some 60 cities to encourage and enforce fair trade.",
            "Whether grain is sold heaped or struck level with the rim has started riots. A Catfolk granary warden measures out with the town's bushel and is believed, since everyone knows the warden cannot eat what is in it."),
        ["horseCollar"] = (
            "A heavily padded collar of leather round the horse's neck. Iron or wooden hames surround the padding and carry the eyes for the reins and the traces. It rests on the shoulders, so that the horse is not choked as it was by the older harness across the windpipe. It came into general use in Europe during the 12th century.",
            "The collar is the harness maker's masterpiece, stuffed with rye straw and fitted to one horse. With it a horse does the work of the ox team in half the time, where there is oats to feed it."),
        ["twoWheeledCart"] = (
            "A two-wheeled vehicle, usually drawn by a single animal, with shafts that often continue the frame of the body, which may be a few crossed stakes or a sturdy box. The Luttrell Psalter of 1325-1340 shows a farm cart and a harvest cart going uphill.",
            "The roads of the travel network are cart roads only where the land is flat: in the hills the load goes onto backs. Grass riders move their whole household on carts, felt tent and all."),
        ["packSaddle"] = (
            "A wooden frame of shaped boards that rest on either side of the animal's back, joined by two curved stretchers with holes for tying on the load and a gap over the spine. Goods were carried in baskets (panniers) on either side. The Museum Wales example is pine, of the early 19th century.",
            "Mules in the mountains, camels on the salt road, yaks on the high passes and reindeer in the north carry the same frame under different names. A caravan master of the Saltroad folk loads salt one way and gold the other and balances the two sides by hand."),
        ["soundingLeadAndLine"] = (
            "A lead weight on a rope, lowered over the side until it touches bottom to measure the depth of the water. The base is hollowed and packed with tallow, which brings up a sample of the seabed that helps to fix the ship's position. Royal Museums Greenwich calls it probably one of the oldest instruments of navigation.",
            "A pilot knows a coast by what sticks to the tallow: grey sand here, shell there, black mud off the river mouth. Shoalfolk pilots do without the lead in their own shallows and are paid well by those who cannot."),
        ["marinersCompass"] = (
            "A magnetised iron needle, stroked with a lodestone, balanced on a spike under a card with north marked, in a wooden box. The box hangs in two brass gimbal rings that keep it level as the ship pitches and rolls. Three compasses were found on the Mary Rose, one in a chest thought to be from the pilot's cabin.",
            "The needle is the ship's master's own and goes ashore with him. Sailors of every race agree that it wanders where the Supernatural is high, and a careful master then trusts the lead and the lookout: a Widegaze one if he can hire it."),
        ["marinersAstrolabe"] = (
            "A heavy wheel of cast brass, with parts of the disc cut away against the wind and a weighted lower half so that it hangs upright, fitted with a turning sight arm. It gives the latitude from the height of the Pole Star or the Sun. It was adapted from the astronomer's astrolabe and came into use at sea by about 1470.",
            "Only the masters of deep-sea ships carry one. The coasting trade of the Northmen and the canal boats of the Canal folk never lose sight of land long enough to need it."),
        ["blackJack"] = (
            "Tankard-shaped drinking vessels of leather were called black jacks, and the larger leather jug a bombard. The museum describes them as robust, waterproof containers for water and drink, a popular choice of London's alehouse, tavern and inn keepers. The example is leather and pewter, 29 cm high, of the 16th or 17th century.",
            "Leather does not break when it is thrown, which is the whole of its merit in a tavern. A Halfling inn serves in turned wood and counts the cups back in."),
        ["aleStake"] = (
            "A pole projecting from a tavern over the street as its sign. A London ordinance of 1375 limited ale-stakes to seven feet over the highway, since longer ones hindered riders and people on foot, with a fine of 40 pence for each offence.",
            "When a new brew is ready the alewife puts the stake out, and the official's ale-taster comes before the customers do. On the roads between Prospects a stake at a farm gate means a bed as well."),
        ["fleamAndBleedingBowl"] = (
            "A fleam is a small steel blade for bloodletting. A set in the Science Museum, probably German and dated 1544, has two fleams with triangular blades and one with a round blade in a carved bone case. The blood was caught in a handled bleeding bowl. Bloodletting was used against many fevers and diseases thought to come from a build-up of blood.",
            "The barber-surgeon bleeds in spring and autumn as a matter of course. The Flinchless, who examine each other at dawn and dusk and became the world's surgeons, bleed less than anyone, and an Amberblood's cut closes with a golden bead before the bowl is in place."),
        ["uroscopyFlask"] = (
            "A glass flask in which a patient's urine is held up and inspected. Physicians judged its colour, consistency, clarity and sediment as signs of imbalance in the four humours, with the help of charts (urine wheels) that show rows of flasks. Urine was examined in ancient Greece, medieval Europe and the Islamic world, and a urine wheel was printed in Venice in 1491.",
            "The flask is the sign of the learned physician as the peel is the baker's. The Stillfaces skip it: their diagnosticians read a sickness from the scent of the skin and are consulted, quietly, by physicians of every race."),
        ["mortarAndPestle"] = (
            "A heavy bowl of cast copper alloy with a pestle for grinding. The V&A calls mortars and pestles standard tools of the trade for physicians and apothecaries, and everyday utensils in the household for grinding spices, sugar and grains. They were associated with alchemy too. The example is Netherlandish, about 1540.",
            "Herbs and Reagents are both resources of the map, and both pass through a mortar before they do anyone good or harm. A Bitterblood apothecary keeps two, and customers are shown which one their cure is ground in."),
        ["foldingAlmanac"] = (
            "A manuscript on vellum of the late 15th century, of seven leaves folded to about 14 by 5 cm, in a limp cover with a tag for hanging it by a cord from the girdle. It holds a calendar, tables of the planets and the moon, tables of eclipses for 1461-1481, a vein-man and a zodiac man.",
            "The astrologer and the physician consult the same folded leaves: one for a wedding day, the other for which vein to open under which sign. A weather watcher of the Widegaze adds a column the almanac lacks, of what the horizon looked like at dawn."),
        ["cauldron"] = (
            "A cooking vessel of copper alloy. A London example of the 14th or 15th century is 22 cm high and weighs 4 kg. Most cooking was done over an open fire, and many vessels had three feet to raise them above the hearth. Metal vessels like this were more common in the wealthier households.",
            "The pot is never emptied in a great kitchen: what is left of one day's pottage starts the next. Among the Orcs the cauldron belongs to the hearth-mother, and who eats from it is who belongs."),
        ["roastingSpit"] = (
            "An iron spit about 1.26 m long, with a point, a long blade and a crank, from London, late 15th or early 16th century. Joints of meat or whole animals were skewered on it. It rested on a rack in front of the fire and was turned slowly, so that the meat cooked without burning.",
            "Turning the spit is the first work a kitchen boy is given and the one he is gladdest to leave. Roast meat is the food of a hall: a farmhouse boils."),
        ["boxwoodComb"] = (
            "A comb carved from boxwood, with thick teeth on one side and fine teeth on the other. It dressed the hair, and the fine side removed lice and nits. Boxwood combs were fashionable with men and women from about 1400 into the 17th century, were often love gifts carrying an inscription, and were kept in decorated leather cases.",
            "Northmen comb their hair daily and are thought vain for it by their neighbours. A lady's maid, a bathhouse attendant and a valet all work with this comb, and a carved one is the usual first gift of a suitor."),
        ["harp"] = (
            "A plucked, framed string instrument, strung with gut, or in Ireland with metal strings from an early date. By the 14th century it sometimes had as many as twenty-five strings. Around 1400 it took a slimmer form with a small sound-box of hardwood and bray pins that made the strings buzz.",
            "A good poet eats at the high table of a Northman hall and the harp is why. Elven harpers play at human courts for a season that the court remembers for three generations and the harper forgets."),
        ["pipeAndTabor"] = (
            "A recorder-like pipe with only three finger holes, played with one hand so that the other can beat a drum: something like a one-man dance band. It is often shown in pictures of music-making out of doors. Tabor pipes and drums were recovered from the Mary Rose.",
            "One player is a whole dance, which suits a fair, a wedding or a company of Swiftborn players who cross a region in a season. The drummers and praise-singers of the Sunlanders would call the tabor a child's drum."),
        ["hurdyGurdy"] = (
            "A string instrument sounded by a rosined wooden wheel turned by a crank. The wheel keeps several strings droning, and one is a melody string shortened by keys. It first appears in the 10th century as a large instrument for two players, the organistrum, and was made smaller in the 13th, as the symphonia.",
            "It began in the temples and ended at their doors: the instrument a blind singer can play all day for alms, since the wheel never needs to draw breath."),
        ["mirrorInAnIvoryCase"] = (
            "A small hand mirror held in a case of two carved discs of elephant ivory. The V&A example, from Paris or possibly Cologne, early 14th century, is 11.5 cm across and carved with an assembly of lovers. Mirror cases, ivory combs and hair-parters made up the dressing case of the wealthy and were costly gifts between friends, family and lovers.",
            "A companion's standing is read from the gifts she is seen to use, and the mirror case comes out where others can see who carved it. The ivory is Sunlander elephant or northern walrus, and the mirror makers of the cities silver the glass."),
        ["pomander"] = (
            "A small hollow pierced ball of silver-gilt, in two halves that unscrew so that it can be filled with aromatic substances to scent the air. The V&A example is German, about 1500, 3.7 cm high. Pomanders were widely used as pendants on prayer beads, and a comparable piece was worn hanging from a girdle or on a chain round the neck.",
            "Ambergris, musk, frankincense and myrrh are Luxuries of the map and travel far to hang at a belt. The perfumers of the Stillfaces compose scents that say a whole sentence to their own people and merely smell pleasant to everyone else."),
        ["sealMatrix"] = (
            "An engraved die, usually of lead or copper alloy and now and then of silver, for making an impression in wax to authenticate a document or keep it closed. Flat matrices are round or pointed-oval with a pierced lug, and conical ones have a pierced end for hanging. The inscription is cut in mirror image. Matrices with a personal name are thought to date to the 13th century, and official seals carry the name of the office.",
            "A lord who cannot write still seals, and a matrix is broken at its owner's death so that no one can speak for him afterwards. The Lanternkin became the notaries of the world because they seal with a drop of their own blood, which glows for as long as they live."),
        ["knightlySword"] = (
            "A double-edged iron sword, 108 cm long with a cross-guard of 20 cm, weighing 2 kg, of the early to mid 14th century, found in the Thames. The museum states that large double-edged war swords like this were the chief weapons of armoured knights, and that an ordinary Londoner found carrying one could be arrested.",
            "A sword is the one tool on this list that is also a rank: among the Midlanders it is girded on at a knighting and carried before a lord at his burial. Rainkeeper steel makers sell the best blades and Dwarven ones the best kept."),
        ["spurs"] = (
            "Spurs of iron or copper alloy strapped to the rider's heel. Prick spurs carry a single point and declined in England at the end of the 13th century. Rowel spurs, with a turning toothed wheel, came in during the early 13th century and were the only form from the early 14th.",
            "Gilt spurs mark a knight and are hacked off the heels of one who is disgraced. The Grass riders, who live in the saddle, wear none and consider them a confession."),
        ["maceOfOffice"] = (
            "A staff of authority descended from the weapon carried by royal bodyguards from the 13th century. From the late 14th century a wider range of royal and civic officials carried maces. A mace of about 1475 at Stratford is silver over an iron shaft with the royal arms: it was carried by or before the high bailiff, and the serjeant used it to bang on doors and summon defendants to court.",
            "The bailiff who comes without his mace comes as a private man and can be refused. A magistrate of the Canal folk carries a seal instead, and a Northman law-speaker carries nothing but a third of the law in his head."),
        ["billAndPike"] = (
            "The bill derives from a farm pruning tool: an iron head of over 2 kg with a curved blade and a spike at the top, on a stout ash staff. The pike is like a spear, with a long staff and a small blade, used on land by squares of foot soldiers to hold a barrier of points against cavalry. The staffs are mostly ash, about 2 to 5 m long.",
            "The bill is the billhook of the hedge on a longer pole, which is how a levy of farmers arms itself. Orc soldiers for hire stand in pike squares that the cavalry of the Grass riders will not charge twice."),
        ["mailShirt"] = (
            "A shirt of about 28,000 small rings of iron and steel, most closed with a rivet, weighing about 14 kg, of the late 14th or early 15th century. Mail was the main metal body armour until the 14th century and stayed in use until the 16th. It took much labour and was very expensive, so most soldiers wore a padded coat instead.",
            "A mail shirt is a year of an armourer's work and is inherited, mended and re-cut for three generations. The heavy foot of the Dwarves are hired with their mail, and the hire is higher for it."),
        ["halberd"] = (
            "An axe mounted on a long pole: a steel head with an axe blade for hacking, a spike at the top for thrusting and a beak at the back for piercing armour or unseating a rider, about 2 m overall. It was an infantry weapon favoured by Swiss and German mercenaries, and by the late 1500s had become a ceremonial weapon of princely bodyguards.",
            "The gate guard leans on it all day and uses it once a year, and the bodyguard's is gilded. Northmen sworn guards abroad carry a long axe in its place and are hired by kings who trust no one born nearby."),
        ["caltrop"] = (
            "Four iron spikes joined so that, however it lands, three form a tripod and the fourth points up. Caltrops were thrown on the ground to slow an advance by wounding the feet of men and horses. The specimen described is Roman. The Science Museum Group holds one of brass and iron, probably English, of the 13th to 17th century.",
            "A handful scattered in a ford stops a merchant's wagon or a pursuing sheriff equally well. The Flinchless walk across them without a sound, which is a reason of its own to check their feet at dusk."),
        ["watchmansHorn"] = (
            "An animal horn blown to set the watch. At Ripon the Wakeman answered for the prevention of crime from nine in the evening until dawn, and a horn is still blown each night at the four corners of the market cross. The city's Charter Horn is many centuries old and hangs on a belt with silver badges.",
            "One blast at the closing of the gates, and after that the watch answers for what is stolen. Duskreaders and Catfolk fill the night watch of half the towns of the world, and the beacon keepers of the Widegaze pass the alarm faster than a horse."),
        ["hornLantern"] = (
            "A cylindrical lantern of tin plate and iron with windows of horn, 26 cm high.",
            "Horn is a by-product of the butcher, scraped thin enough to let a candle through and tough enough to drop. A caravan or a ship that can afford it hires a Lanternkin instead, whose light needs no flame."),
        ["fireHook"] = (
            "A long wooden pole with an iron hook: the London Museum example is 4.33 m long and weighs 34.5 kg. Hooks were used to pull down buildings ahead of a fire, or burning houses, to stop the fire spreading. Each parish kept its own firefighting equipment, usually in the church.",
            "A town of timber and thatch fights fire by tearing down the next house, and the hook hangs under the eaves of the market hall for it. The Flinchless serve as firefighters and are checked over by their fellows afterwards, since they do not know when they are burned."),
        ["censerAndIncenseBoat"] = (
            "A metal vessel with a base that holds glowing charcoal and a pierced lid, hung on chains so that it can be swung. Incense is spooned onto the coals from a separate incense boat. An English censer and boat of the 14th century, of silver and silver-gilt, were found in 1850 when a mere was drained.",
            "Frankincense and myrrh come up the salt road from the dry scrub to temples that have never seen the tree. A temple priest's rank shows in the metal of the censer, and the village priest burns pine resin in a clay one."),
        ["chalice"] = (
            "A stemmed cup with a bowl, a stem and a round foot, holding the wine that the priest consecrates in the rite. Because the wine was held to become holy, chalices were usually of precious metal. The V&A example is silver-gilt, Spanish, about 1550-75.",
            "The cup is the first thing a temple buys and the first thing raiders take. The priests of the Pair among the Twofold use a cup with two bowls on one stem."),
        ["shamansDrum"] = (
            "An oval frame drum with a skin of reindeer hide bearing figures, struck with a T-shaped beater of reindeer antler. By the museum record, such drums were made as an aid to reach a trance and speak with gods and spirits. A second use was to foretell: a pointer was laid on the skin and the future read from the figure it came to rest on.",
            "The shamans of the Grass riders and of the reindeer herders of the tundra drum where a priest of the settled lands would pray. The figures on the skin are the shaman's own map of the three worlds, and no two drums agree."),
        ["quillPenAndPenknife"] = (
            "A pen cut from a wing feather, most often of a goose. The barrel is hardened in hot sand, the tip sliced at an angle and slit to hold and feed the ink, then trimmed to a nib whose shape affects the script. It was the main writing tool from about the 7th century to the 19th. The scribe re-cuts it with a penknife.",
            "The Saltroad folk kept the books when everyone else burned theirs, and a clerk of theirs cuts a pen in four strokes without looking. The Canal folk write with a brush and print what must be read by many."),
        ["waxTabletAndStylus"] = (
            "A flat board, usually of wood, hollowed and filled with wax. A pointed stylus scratches the letters into the wax and its other end smooths it for reuse. A London stylus of the 13th to 15th century is bone with a metal tip, 7 cm long, with a round head used as the eraser. Tablets served for school exercises, letters and records.",
            "Everything is drafted in wax and only what matters is copied to parchment: a schoolmaster's lesson, a steward's count of sheep, a poet's first try. The wax is the beekeeper's second crop."),
        ["pricker"] = (
            "An awl or other sharp point used to lay out a page before writing. After ruling the first leaf of a gathering, the scribe pricks holes at the edges where the lines end. The holes guide the ruling of the other leaves from hole to hole, so that margins and lines are the same on every page. The prickings were usually trimmed off in binding.",
            "A copyist is paid by the gathering, and the pricking is the part of the work no reader ever sees. Dwarven records clerks leave the prick holes in, as proof that no leaf was added later."),
        ["astrolabe"] = (
            "A portable instrument for calculating with the positions of the Sun and the stars. The body (the mater) carries scales of degrees and hours on its rim and holds plates, each a projection of the sky for one latitude. Over them turns the rete, a cut-out star map with pointers for the stars. On the back a sighting bar is aimed at a star or the Sun to read its height.",
            "Astronomers of the Saltroad folk and the Rainkeepers own the best, and Gnome instrument makers cut them. The Noonfolk, who fall asleep at sunset and have never seen the stars, buy astrolabes to learn what the sky does when they are not there."),
        ["armillarySphere"] = (
            "A model of the universe built of rings that stand for the circles of the sky, such as the equator and the tropics, commonly of brass. Large ones were used for observing. Smaller ones, known in western Europe from the later Middle Ages, taught students the geometry of the celestial sphere. A late medieval example in the Whipple Museum was probably held by a handle at the south pole.",
            "A teacher holds it up by the handle and turns the sky for a room of students. The Northmen, who hold that the sky is the planking of an upturned hull, find the rings a poor likeness of a ship."),
        ["sealsOfWax"] = (
            "Discs of wax engraved with magical names, symbols and signs, of 1550-1599, in the British Museum. The two smaller ones are what survive of four that stood under the legs of John Dee's \"table of practice\". The larger, the Seal of God, matches a drawing in Dee's manuscripts and supported one of his \"shew-stones\". Dee used them in his occult work with his medium Edward Kelly.",
            "A mage is found only in a capital and only where the Supernatural is high, and the table is set out before anything else is done. Whether the seals protect the mage or only steady the hand is disputed, mostly by those who have never worked without them."),
        ["bookOfConjurations"] = (
            "A working manual of ritual magic. One in Chetham's Library, of the second half of the 16th century and in Latin with English additions, holds conjurations, astrological tables and signs of the planets. It prescribes consecrated tools (a copper plate inscribed with a pentangle, a ring, a sceptre and a sword) and a circle of two chalk rings, at most fifteen feet across, inscribed with holy names. Its wear suggests that it was used.",
            "The sorcerer-kings of the Old Realms reshaped rivers, forests and peoples, and what a court mage of the Scattered Age owns is a copy of a copy of a fragment of how. A mage's book is in his own hand, since nobody sells theirs."),
        ["runeStick"] = (
            "A small stick, about 8.5 cm long, from Odense in Denmark, of the early 13th century. Its runic inscription is in Latin and includes the words for good health and a man's name. The researcher of the National Museum of Denmark who read it suggests that it was an amulet to protect its owner from illness and evil.",
            "The rune carver of the Northmen cuts a name and a wish into wood for a sailor to carry, and into stone for kin who died abroad. A stick of elderwood is said to hold its wish longer, and costs accordingly."),
        ["athanor"] = (
            "An enclosed furnace built to keep a flask at a constant heat. A Science Museum example of grey-glazed stoneware, German or French, 1501-1700, is 45 cm high and built to give an equal heat throughout. An athanor is drawn in a manuscript of about 1450-1500, and a 13th-century book of furnaces shows a vessel in a pan of ashes inside one.",
            "An alchemist's work is counted in weeks of even heat, so the furnace must feed itself with charcoal while its owner sleeps. Ghostlight peat burns cool and steady and is prized for it."),
        ["shewStone"] = (
            "John Dee's \"shew-stones\" were polished translucent or reflective objects used in his occult work: his medium Edward Kelly reported visions in them. A crystal sphere in the British Museum may be one of the stones named in Dee's manuscripts, though the museum says that this cannot be proven.",
            "Rock crystal is a resource of the volcanic mountains, and a flawless ball of it is a seer's whole fortune. An oracle looks into the stone and a second person writes down what is said, since the oracle rarely remembers."),
        ["geomanticInstrument"] = (
            "A brass plate inlaid with gold and silver, about 27 by 34 cm, made in 1241/42 and now in the British Museum. It mechanises geomancy, divination from patterns of dots traditionally traced in sand. It has sixteen dials that each show a pattern in a small window: four are turned at random, twelve more are derived from them, and a panel gives the meaning of the last.",
            "The geomancers of the Canal folk are asked where a house, a grave or a canal lock may stand. A village diviner casts the same sixteen figures with a stick in the dust, and charges an egg."),
        ["woodenShovel"] = (
            "A blade of oak in one piece, 32 cm long, from a medieval castle site in Wales, of a type dated to the 10th to 14th centuries. A shaft was fitted into a slot in the blade and pegged. With no metal shoe, and shoulders that slope too sharply for the foot, it was for moving loose material, not for cutting.",
            "The labourer is a ninth of all who work and owns the cheapest tool of them all. Every ditch, dike, moat and grave of the world was moved one such shovelful at a time, and the Rainkeepers' ten thousand reservoirs with it."),
        ["wheelbarrow"] = (
            "A barrow with one wheel, in medieval Europe usually at or near the front. It appeared between about 1170 and 1250: the earliest record is a purchase of 1222 for the king's works at Dover, and the earliest picture is of about 1250. It was used in building, mining and farming but stayed fairly rare until the 15th century.",
            "One man with a barrow does the work of two with a hand-barrow between them, which is why a master of works buys them and labourers distrust them. The Canal folk fit theirs with a sail on the dike roads."),
        ["lepersClapper"] = (
            "A hand-held wooden clapper. People with leprosy wore distinctive clothing and carried a clapper or a bell to warn others of their approach, and it may also have drawn attention for alms. The Science Museum piece is a copy, about 26 cm long, of a 17th-century original from a leper hospital in Kent.",
            "The sound clears a street and fills a bowl, and both are the point. The Bitterbloods, who are poisonous to the touch, once carried clappers by law in the towns that feared them."),
        ["almsBowl"] = (
            "A wooden bowl on a handle just under 40 cm long, so that money or food could be given to people with leprosy without coming close. The original was used in the 1400s at a leper hospital in Kent founded in 1084. The museum object is a wooden model made in 1917-1920.",
            "An almsman of a temple has a licensed place at its door and a bowl with the temple's mark. The ascetics of the Rainkeepers carry a bowl by choice and are fed before the household eats."),
        ["pilgrimBadge"] = (
            "A small cast badge of lead-tin alloy bought at a shrine as a token of the pilgrimage. A London example of 1350-1400, about 75 mm across, shows a saint returning from exile on a ship. Many Londoners travelled to the saint's shrine to pray and bought badges there.",
            "A hat full of badges is a pilgrim's passport and a beggar's credential: the more shrines, the more alms. Shrines sell them by the thousand, and some of the wearers have been no further than the mould."),
        ["falseDice"] = (
            "A set of 24 bone dice of the late 15th century, found on the foreshore of the Thames in a pewter pot. X-rays show that 18 hold drops of mercury: eleven tend to fall on five or six and seven on one or two. The other six carry repeated numbers, three with only one to three and three with only four to six. Loaded dice were called fulhams.",
            "Quicksilver is a Reagent of the volcanic lands, and more of it goes into dice than the alchemists like to admit. A swindler changes the true dice for the false in the palm, which is why a careful tavern keeps its own dice chained to the table."),
        ["picklock"] = (
            "The Pitt Rivers Museum catalogues an English picklock, undated, with a stem ending in a hook. The locks of the period were warded: the Science Museum Group holds a 14th-century bronze key made for a lock of five wards, and English iron keys of the 15th and 16th centuries.",
            "A warded lock stops an honest key of the wrong shape and little else, as every burglar knows. The fine locks of the Gnomes are another matter, and a Dwarven vault keeper trusts a guard more than either."),
        ["bollockDagger"] = (
            "A dagger with a one-piece wooden handle ending in two oval lobes at the blade, with an iron blade of 195 mm, of the mid 15th to early 16th century. Such daggers were often civilian weapons: although wearing armour and swords in the street was banned, most men carried a dagger in a sheath on the belt, and fights often ended in stabbings.",
            "The knife every man carries to cut his bread is the one that cuts a purse string in a crowd or settles a quarrel on the road. Because everyone has one, nobody can be hanged for owning it."),
        ["falchion"] = (
            "A medieval single-handed sword with a large curved blade. The early, broad ones are called cleaver falchions. The Conyers falchion at Durham Cathedral has a steel blade, a guard and pommel of bronze and a grip of ash, and its heraldry suggests a date before 1272. It was presented to each new bishop as a sword of tenure.",
            "It is cheaper than a knight's sword and asks less skill, a butcher's cleaver made long. Raiders, pirates and the veiled raiders of the salt road carry it, and so do the caravan guards hired against them."),
    };
}
