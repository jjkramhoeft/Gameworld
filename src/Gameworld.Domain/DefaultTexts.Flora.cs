namespace Gameworld.Domain;

// What each default kind of flora is and its lore: the description and the part "Lore" of its entry in docs/research/flora-and-fauna.md section 4, without the citations.
// A kind that has no text here gets DefaultTexts.Missing.

public static partial class DefaultTexts
{
    internal static readonly Dictionary<string, (string Description, string Lore)> Flora = new()
    {
        ["spruce"] = (
            "Fast evergreen conifer to about 30 m of northern and central Europe. Timber, paper pulp, pitch, turpentine, tannin and resin. The shoot tips make a tea, and the inner bark is food in need.",
            "The Northmen roof-tree is a spruce felled at midwinter, and the house stands as long as its stump is left alone."),
        ["scotsPine"] = (
            "Evergreen conifer to about 25 m, from Europe into temperate Asia. Timber for building and furniture, resin, turpentine, pitch, fuel and rope fibre. The inner bark is famine food.",
            "Tar is the Northmen's second export after ships, and every hull is a pine forest boiled down."),
        ["larch"] = (
            "Deciduous conifer to about 30 m, of mountains and lowland taiga. The wood is strong, heavy and durable, for building, bridges and poles, and the bark is a commercial source of tannin.",
            "The one conifer that goes bare in winter. Amberbloods say it is the tree that remembers being a person."),
        ["birch"] = (
            "Fast deciduous tree to about 20 m of open woodland and heath. Sap and inner bark are eaten. It gives timber, firewood, charcoal, tar oil, bark for thatching, fibre and tannin.",
            "The first tree back after fire and ice. A Northmen child's cradle is birch, and so is the bark its first letters are scratched on."),
        ["poplarAndAspen"] = (
            "Fast deciduous trees of 15 to 18 m. Aspen grows in open woodland on poor soils, and its soft wood makes paper and charcoal. The Euphrates poplar stands on ground flooded part of the year and gives building wood and a poor fuel.",
            "In the deserts a line of poplars is a river seen from a day away, and the only roof beams for a week's ride."),
        ["rowan"] = (
            "Deciduous tree to about 15 m of woods, scrub and mountain rocks. The fruit is eaten cooked and is astringent. The wood is turned and makes hoops, and the tree gives a black dye and tannin.",
            "A rowan growing on another tree guards against witchcraft. A sprig hangs over the door of every byre in the north."),
        ["alder"] = (
            "Short-lived deciduous tree to about 25 m of wet ground by streams and lakes. The wood lasts under water. It gives charcoal, dyes, ink and tannin.",
            "Canal folk drive alder piles under their quays: a town on water stands on a drowned alder wood."),
        ["juniper"] = (
            "Evergreen shrub or small tree, from under 1 m to 9 m, of downs, heaths and moors across the northern temperate zone. The cones flavour food and spirits and are a kidney remedy. Fuel, incense and tinder.",
            "The smoke cleans a tent after sickness. Grass riders carry a branch from the home camp and burn the last of it on arrival."),
        ["willow"] = (
            "Fast deciduous tree to about 25 m of streams, marshes and wet fens. The rods make baskets. The bark is taken for pain and fever. Charcoal, poles and turnery.",
            "The osier beds of a river town are rented by the row, and the rent is paid in eel traps."),
        ["oak"] = (
            "Slow deciduous tree to about 30 m, dominant on clay soils. Durable timber, fuel and charcoal. The bark tans. The galls make ink and dye. Acorns are eaten leached.",
            "The Old Realms planted oaks in rows for ships they never built. The rows still stand, and courts are held under them."),
        ["beech"] = (
            "Deciduous tree to about 30 m, dominant on chalk and limestone. Durable wood, good fuel and charcoal. The nuts give an oil, and the dry leaves stuff mattresses.",
            "Gnomes dug their warrens under the beech roots and still pay the tree a yearly rent in mast."),
        ["ash"] = (
            "Fast deciduous tree to about 30 m of woods and hedges on limy soils. The wood makes tool handles and furniture and is a good fuel. The bark tans and the leaves give a green dye.",
            "Every spear shaft and plough handle in the Midlands is ash. A lord's levy is counted in ash trees as well as men."),
        ["elm"] = (
            "Fast deciduous tree to about 30 m of woods, hedges and streamsides. The wood lasts in water and makes pipes, wheels and keels. The inner bark gives fibre for mats and rope.",
            "A town's water runs through bored elm trunks, and the plumber is a kind of carpenter."),
        ["lime"] = (
            "Deciduous tree to about 30 m of woods on fertile, limy soils. The bast gives fibre for mats, rope and cloth. The soft wood is carved and makes drawing charcoal. The flowers make a calming tea.",
            "Stillfaces carve their masks of lime wood, one for each feeling they wish to be seen having."),
        ["fir"] = (
            "Fast evergreen conifer to about 45 m of mountain forests. Light soft timber and pulp. The resin goes to perfumery and to caulking ships.",
            "The tallest masts come down the mountain rivers one at a time, each with a rider."),
        ["sugarMaple"] = (
            "Large tree to about 40 m with a dense crown. Syrup and sugar are boiled from its sap, and its hard wood makes furniture and floors.",
            "The sap run is the first harvest of the year and the only one the frost helps."),
        ["yew"] = (
            "Slow, long-lived evergreen to 25 m. The hard fine-grained wood makes bows, cabinet work and carvings. Leaves and seeds are poisonous and sometimes kill livestock.",
            "Planted where the dead lie, and cut for the weapon that sends them there. Elves fell no healthy tree, so their bows are of windfall yew."),
        ["mulberry"] = (
            "Tree with sweet white to dull red fruit, widely grown. Its leaves feed silkworms.",
            "Canal folk plant it on the dikes between the paddies, and a daughter's dowry is counted in trees."),
        ["cedar"] = (
            "Long-lived evergreen conifer with a massive spreading crown, of mountains. Aromatic wood that resists decay, used for building and for ships.",
            "The sorcerer-kings roofed their halls with it. A cedar beam from an Old Realms ruin is sold by weight and never burnt."),
        ["giantConifers"] = (
            "The redwood is the tallest living tree, often over 90 m, of a coastal fog belt. Its timber goes to building, shingles and posts. The red cedar reaches about 60 m with a buttressed base, and its wood resists damp and decay: shingles, pilings and boats.",
            "One trunk is a ship's keel, a hall's ridge and a winter's work for a village. Elves count the felling of one as a killing."),
        ["hemlockTree"] = (
            "Evergreen conifer of about 60 m with a trunk 2 to 3 m across. A valuable timber.",
            "Its bark fills the tanners' pits of the wet coast, and the stripped trunks are left to the mushrooms."),
        ["lacquerTree"] = (
            "The tree whose sap is true lacquer. Purified and brushed on wood or metal, it cures to a firm shining finish. Raw, it blisters the skin.",
            "Canal folk lacquer takes thirty coats and a tapper with scarred hands."),
        ["corkOak"] = (
            "Evergreen oak of about 18 m. The bark is stripped every eight to ten years without harm to the tree, for stoppers, floats and fishing gear.",
            "The year of the last stripping is painted on the trunk, and a grove is sold with its dates."),
        ["kermesOak"] = (
            "Dwarf evergreen oak with tannin-rich bark, on which the red dye insect lives.",
            "Goats may browse any scrub but this. The fine for a goat in the kermes oaks is the goat."),
        ["cypress"] = (
            "Narrow evergreen conifer of about 24 m with aromatic foliage. Grown for its form. The timber is often knotty.",
            "Planted in pairs at a gate: one for welcome, one for the day the guest is carried out."),
        ["stonePine"] = (
            "Pine with a wide flat crown on a tall bare trunk. Its nuts have been valued since ancient times.",
            "Swiftborn pickers strip a grove in a morning and are paid by the sack."),
        ["saxaul"] = (
            "Branched shrub or tree of 1 to 5 m with deep roots, of foothill deserts, dry riverbeds and salt flats. It bears drought and salt. Desert people burn it, it holds sand, and sheep and camels browse it.",
            "Brinefolk pan towns boil their brine over saxaul and plant two for each they burn."),
        ["acacia"] = (
            "Thorny trees of 6 to 15 m of dry scrub and wooded grassland, the umbrella thorn with a flat crown. The gum goes into food, medicine, textiles and ink. Posts, poles, fuel and charcoal. The bark tans, and the pods and seeds are eaten.",
            "A Sunlander cattle fold is fenced with cut thorn. The gum tappers work the same trees their grandmothers scarred."),
        ["frankincenseTree"] = (
            "Evergreen tree to 8 m of dry mountains and desert woodland. The resin is burnt as incense and used in perfume and medicine. Pliny has it cut in summer and again in spring, the gatherers kept from women and funerals, and the gum taxed before it may leave.",
            "Saltroad incense gatherers inherit their trees by name and may not sell them outside the clan."),
        ["myrrhTree"] = (
            "Spiny deciduous shrub or small tree of about 5 m, of desert scrub on shallow limestone soil. The resin that weeps from cuts in the bark is used in medicine, perfume, incense and embalming. Pliny has the trees cut twice a year.",
            "The Brinefolk embalmers are its best customers."),
        ["dragonsBloodTree"] = (
            "Evergreen tree of about 9 m with a dense umbrella crown, on rock and limestone. Its red resin is used in medicine, varnish, pottery colour and cosmetics.",
            "Pliny's tale that the red is the blood of a dragon crushed by an elephant is told of this resin too. Apothecaries sell it by the tear."),
        ["baobab"] = (
            "Deciduous tree to about 20 m with a swollen trunk up to 10 m across, of the thorn woodlands of the savanna. The fruit pulp, leaves and seeds are eaten. The bark gives cordage and cloth.",
            "A village meets under its baobab. The Sunlander council that can depose a king sits in the shade of the oldest."),
        ["mesquite"] = (
            "Thorny flat-topped tree of 5 to 10 m on light sandy soils of arid land. The pods make flour and drink. Posts, firewood and charcoal, and a tannin.",
            "The pod flour keeps a year. Duskreader night markets price everything else against it."),
        ["teak"] = (
            "Large, mostly deciduous tree over 30 m of the tropical deciduous forests. A first-rate timber. Bark and leaves give a dye.",
            "Rainkeeper kings own every teak in the realm, standing or fallen, and mark each with a seal."),
        ["sal"] = (
            "Tree of 20 to 25 m, the main forest tree of its range. Timber. The resin is burnt as incense and caulks boats. The seed gives a butter and a lamp oil, and the leaves serve as plates.",
            "A Rainkeeper feast is eaten off sal leaves, and the count of leaves swept up is the count of guests."),
        ["sandalwood"] = (
            "Small slow evergreen tree of 4 to 20 m of dry deciduous and scrub forest. The scented heartwood is carved, burnt as incense and pressed for a perfume oil.",
            "The tree is weighed, not measured, and the roots are dug: nothing of it is left in the ground."),
        ["banyanAndSacredFig"] = (
            "Wide-crowned evergreen figs of 15 to 30 m that often start as a seedling on another tree. The banyan drops roots from its branches. Fruit, medicine, a bark fibre, and a latex used as varnish and sealing wax. Pliny wonders at the tree whose branches root and make arbours for shepherds.",
            "A Rainkeeper village council of five sits under one, and nothing said outside its shade is binding."),
        ["neem"] = (
            "Fast, long-lived evergreen of about 15 m with a dense crown. Bark, leaves and seed are a mainstay of medicine. Timber, firewood, and a resin used in soap.",
            "Twigs for the teeth, leaves in the grain bin, a branch at the door in fever time."),
        ["kapok"] = (
            "Deciduous tree of 10 to 30 m, at times 70 m, often spiny and buttressed. The floss around the seeds is a fibre for stuffing. The soft wood is of low grade.",
            "The tallest tree of the forest and the ladder of the dead. Boughwalkers build no platform in one."),
        ["mahogany"] = (
            "Slow evergreen of 30 to 40 m of mixed forest and riverbanks. A high-class furniture timber. The bark gives a dye and a tannin.",
            "One log floated to the coast buys a village's salt for a year."),
        ["ebonyAndRosewood"] = (
            "Ebony is a slow evergreen to 20 m with a black heartwood for furniture, instruments and carving. Rosewood is a tree of 30 to 40 m of monsoon forest with a high-value timber. Pliny notes ebony carried in a triumph.",
            "The black keys of every court harp were once one tree, and its stump is still shown."),
        ["rubberTree"] = (
            "Fast deciduous tree, over 40 m in the wild, of riverbanks and flooded rain forest. Its latex is the main source of natural rubber.",
            "Boughwalkers seal their rope bridges with it. Lowlanders buy it for balls and for the soles of thieves."),
        ["cinnamon"] = (
            "Slow evergreen of 7 to 18 m of moist forest, kept as a bush when cultivated. Bark and leaves flavour food, and the oils go to incense and medicine.",
            "See the cinnamon bird. Rainkeeper spice growers peel it by the quill."),
        ["cloveAndNutmeg"] = (
            "Evergreen trees of maritime forest and humid volcanic lowlands. The clove is a dried flower bud, taken as a spice and for toothache. The nutmeg seed and its mace are spices, and about 8 g of ground nutmeg can kill.",
            "Each grows on a handful of coasts. Wars of the Scattered Age have been fought over a single grove."),
        ["agarwood"] = (
            "Evergreen tree of about 20 m of primary and secondary forest. A dark resin forms in the wood and is burnt as incense, used in perfume and medicine. The inner bark gives a fibre for cloth and rope.",
            "Only a wounded tree makes the resin, and one in many. The gatherers say the tree must first be given a grief."),
        ["feverBarkTree"] = (
            "Evergreen shrub or small tree of 6 to 20 m of cool humid mountains between 1,200 and 3,000 m. The bark is the remedy for fevers and malaria. Too much brings headache, deafness and blindness. In the generator it stands where the rain forest climbs.",
            "Bitterblood apothecaries sell it and are the only ones trusted to dose it."),
        ["logwood"] = (
            "Thorny, gnarled tree to about 15 m of flat marshy lowland that rivers flood. The heartwood gives red and darker dyes, which is its main use. The flowers give honey.",
            "Cut by gangs who live half the year in the swamp and are paid in the other half, if they come out."),
        ["flameOfTheForest"] = (
            "Slow, crooked deciduous tree of 5 to 15 m of drier plains and grassland. The flowers give a yellow to orange dye. A red gum, a bark fibre, and a soft wood that lasts under water.",
            "It flowers leafless at the end of the dry season. Twofold set the Going out by its bloom."),
        ["quebracho"] = (
            "Tree of 20 to 25 m of dry plains. The wood is very durable, for posts, and a good fuel. Tannin is extracted from bark and heartwood on a commercial scale.",
            "An axe that fells one is retired and hung up."),
        ["mangrove"] = (
            "Trees and shrubs of muddy, salt or brackish shores. The red mangrove stands 5 to 20 m on arching stilt roots. Timber for posts and marine piling, firewood and charcoal, and a bark that tans. The grey mangrove gives boat timber and red and brown dyes.",
            "Shoalfolk do not cut the seaward row. The one town that did is a sandbar now."),
        ["nipaPalm"] = (
            "Clumping palm with leaves to 6 m from a stem that lies in the mud of tidal flats. The sap gives syrup, sugar, vinegar and drink. The leaves thatch and are woven into baskets and mats.",
            "Every roof in a Shoalfolk town is nipa, renewed after the storm season by the whole street."),
        ["dwarfWillow"] = (
            "The \"low willows\" that Britannica lists with the sedges, cotton grass and rushes of the tundra, where plants keep close to the ground against the wind. No species page was opened.",
            "The Hearthbloods' only firewood above the tree line, cut with a knife and carried in a hat."),
        ["labradorTea"] = (
            "Evergreen shrub of about 1 m of acid bogs. The aromatic leaves make a tea for coughs and colds and stood in for hops in beer. They repel insects. Brewed long in a closed pot, they are narcotic.",
            "A Northmen ale brewed with it is called \"the quarrel\": it goes to the head and then to the fists."),
        ["bogMyrtle"] = (
            "Deciduous shrub of about 2 m of bogs, fens and wet heath. Leaves and fruit flavour ale. A wax for candles, a yellow dye, and a repellent of insects and moths. Not for the pregnant.",
            "The ale herb of every people north of the hop line."),
        ["hawthorn"] = (
            "Thorny deciduous shrub to 6 m of woods, hedges and thickets. The fruit is eaten. A remedy for the heart. It is planted as a hedge and a windbreak and gives tool handles and fuel.",
            "Halfling fields are hawthorn-hedged, and a gap in the hedge is a gap in the law."),
        ["elder"] = (
            "Fast deciduous shrub of about 6 m of hedgerows and rich disturbed ground. Flowers and cooked fruit go into wine and preserves. Dyes from bark and fruit. The leaves keep insects off and are poisonous.",
            "Nobody burns elder or cuts it without asking. What is asked is not agreed between any two villages."),
        ["heather"] = (
            "Evergreen shrub of about 0.6 m of acid moors and heaths. It flavours beer, gives a yellow dye and a tannin, and serves as fuel, thatch, brooms, bedding and rope.",
            "A moorland house is heather from the roof to the bed to the fire."),
        ["sagebrush"] = (
            "Evergreen shrub of about 2.5 m of dry plains and hills. A bitter leaf tea for colds and aches. Fuel, tinder, a bark fibre and a yellow dye. The seeds are ground for meal.",
            "The smell of the cold desert after rain. Brinefolk say they can smell it through a wall of salt."),
        ["ephedra"] = (
            "Evergreen shrub of about 0.4 m of desert, sandy ground and mountain slopes. An old remedy for asthma and hay fever. Dangerous to a weak heart.",
            "Grass rider post riders chew it to stay in the saddle for two days. The third day is paid for."),
        ["saltbush"] = (
            "Evergreen shrub of about 1.8 m of sandy and gravelly desert. Leaves are eaten cooked and the seed ground. Fodder, a windbreak, and a lather for washing.",
            "Where it grows the well is brackish and the mutton needs no salt."),
        ["camelThorn"] = (
            "Deciduous spiny shrub to 2 m of saline, dry and barren ground. A sweet \"manna\" forms on the twigs and pods. The root is a famine food.",
            "The manna is shaken onto a cloth at dawn. Caravan law gives it to whoever spread the cloth, on anyone's land."),
        ["creosoteBush"] = (
            "Slow evergreen shrub of about 4 m of hot desert. A leaf tea for wounds and chests that harms the liver if taken long. Its resin keeps fat from going rancid.",
            "A ring of bushes is one plant older than any kingdom, the Duskreaders say, and they hold their markets inside the rings."),
        ["saguaro"] = (
            "Columnar ribbed cactus that branches at about 5 m and reaches 15 m. It first flowers at 50 to 75 years and lives 150 to 200. The fruit is eaten, and the woody skeleton is used.",
            "A giant with its arms raised marks a grave or a well. The traveller is expected to know which."),
        ["sisalAndAgave"] = (
            "Evergreen succulent rosette of 1.5 to 2 m of the drier tropics that sends up one flower stem of 5 to 6 m and dies. Leaf fibre for rope, nets and paper. A fermented drink from the heart. Living fences.",
            "Noonfolk estates are fenced with it, and the fence is the rope walk's raw stock."),
        ["mastic"] = (
            "Slow evergreen shrub of about 5 m of dry hillsides near the sea. The resin is chewed and flavours food. Varnish, a fixative for perfume, and a tannin.",
            "Chewed by sailors to sweeten the breath before they lie to the harbour master."),
        ["oleander"] = (
            "Fast evergreen shrub of about 4 m of river banks and gravels. The whole plant is very poisonous: one leaf has killed a child. A hedge and an insecticide.",
            "An army once roasted its meat on oleander spits, the story goes. The other army won without a fight."),
        ["henna"] = (
            "Evergreen shrub or small tree to 7 m of dry scrub and riverbeds. The leaves dye hair, skin and nails. The flowers give a perfume oil.",
            "A Saltroad bride's hands are painted with the route of her family's caravan."),
        ["tea"] = (
            "Slow evergreen shrub of about 4 m of shaded mountain forest. The leaf is drunk as a stimulant and an astringent. The seed gives an oil.",
            "Canal folk grow it on the hills above the paddies. An official's rank shows in the leaf he is served."),
        ["coffee"] = (
            "Evergreen shrub of 1.5 to 3 m of the understorey of humid highland forest. The roasted seed is a stimulant drink. In the generator it stands on tropical highlands.",
            "Saltroad coffee growers sell the bean and keep the husk for themselves."),
        ["indigo"] = (
            "Erect shrub of 1 to 2 m of the tropics. The leaves give a deep blue dye.",
            "The dyers' quarter is downstream and downwind, and its people are known by their forearms."),
        ["cotton"] = (
            "Plant of about 1.5 m known only from cultivation. The seed floss is spun for cloth and used as stuffing. Twine, rope and paper. The seed gives an oil. Pliny knew of trees that bear wool.",
            "Rainkeeper cotton weavers clothe three peoples and pay the temple a tenth of every bolt."),
        ["mistletoe"] = (
            "Evergreen shrub about 1 m across that grows on the branches of poplar, apple and other trees. It slows the heart and calms. All parts are slightly poisonous.",
            "Pliny's druids cut it from an oak on the fifth day of the moon, a priest in white with a golden sickle, and catch it in a white cloak. Taken in drink it makes barren beasts fertile and is an antidote to all poisons. Frazer takes it for the golden bough."),
        ["arcticPoppy"] = (
            "Short-lived perennial of about 0.3 m on stony slopes and river gravels of the far north. Flowers and capsules ease pain and scurvy. The flowers give red and beige dyes. Mildly toxic.",
            "It turns its face with the sun through the nightless summer. Hoarkin children are told it is counting the days for them."),
        ["fireweed"] = (
            "Fast perennial of about 1.7 m on rocky and waste ground and woodland edges. Shoots, leaves and pith are eaten. The stems give cordage and the seed hairs tinder. An astringent for the gut and for burns.",
            "The first colour on burnt ground. Charcoal burners' camps are found by it the year after."),
        ["tulip"] = (
            "Bulb with a few blue-green leaves and one bell-shaped flower, native to the inner steppe lands. Grown in gardens for a thousand years, and once the object of a trade mania.",
            "Grass riders pick the wild ones for a girl's hair. In Canal folk cities a striped bulb has been sold for a house."),
        ["sunflower"] = (
            "Fast annual of 0.5 to 3 m of open dry plains. Seeds and their oil are eaten. A yellow dye from the flowers, a purple-black one from the seeds, and fibre from the stem.",
            "Noonfolk, who live by the sun, plant it at the field corners and take its turning as a salute."),
        ["coneflower"] = (
            "Perennial of about 1.2 m of dry limestone prairie. The root is a remedy for sores, wounds and burns and against infection.",
            "The Flinchless, who do not feel their wounds, are taught to chew it whenever they see their own blood."),
        ["peony"] = (
            "Perennial of about 0.6 m of rocky woods and meadows on limestone. The root calms spasms and was given for the falling sickness. The whole plant is poisonous in quantity.",
            "Canal folk courts grow a hundred kinds, and an official is judged by his peony bed."),
        ["asphodel"] = (
            "Perennial of about 1 m of dry hills, rough ground and mountain meadows. The tubers are a cough remedy, were once eaten, and are fermented for spirit. The root is poisonous raw.",
            "It covers the fields no one ploughs any more. The dead are said to walk in a meadow of it."),
        ["saffronCrocus"] = (
            "Small corm about 0.1 m high, not known wild. The dried styles flavour and colour food and give a yellow dye. A sedative. Five to ten grams can kill.",
            "Picked before sunrise by the Swiftborn. A pound is a hundred thousand flowers and a month of fingers."),
        ["dogRose"] = (
            "Fast deciduous shrub of about 3 m of hedges, scrub and roadsides. The hips are taken against colds and scurvy and made into syrup, jam and tea. The petals are eaten.",
            "A rose over a door means the house keeps a secret. Over an inn, that it keeps everyone's."),
        ["lavender"] = (
            "Slow evergreen shrub of about 1.2 m of dry, grassy, rocky slopes, often on chalk. The oil scents soap and perfume, calms and cleans wounds, and the flowers keep insects away.",
            "Midlander washerwomen take their trade name from it, and their guild is older than the weavers'."),
        ["roseOfJericho"] = (
            "Small grey desert plant that curls its branches into a ball when dry and opens to 30 cm across when wetted.",
            "Saltroad midwives set a dry one in water when labour begins. The child is due when it has opened."),
        ["rafflesia"] = (
            "A parasite with no leaves, roots or stem, living as threads inside a forest vine. Only the flower shows: about 1 m across, up to 11 kg, smelling of carrion.",
            "Bitterbloods call it the honest flower: it smells like what it is."),
        ["roseroot"] = (
            "Evergreen clumping perennial of about 0.3 m in the crevices of mountain rocks and sea cliffs of the cold north. A tonic against fatigue. Leaves, shoots and roots are eaten.",
            "Northmen crews chew it on the oar bench. A bride is given a root so that the house will have strong sons."),
        ["yarrow"] = (
            "Upright perennial of about 0.6 m of meadows and pastures. Used for wounds and bleeding, colds and fevers. A hop substitute in beer, and a yellow and green dye.",
            "Soldier's woundwort. A Flinchless drill master will not march without a sack of it."),
        ["wormwood"] = (
            "Perennial of about 1 m of waste land, rocks and screes. A bitter tonic for the liver and digestion and a worm remedy. It drives off insects and mice. Poisonous in quantity.",
            "Strewn in a granary against mice, and steeped in the spirit that Orc hearths drink before a raid."),
        ["liquorice"] = (
            "Perennial to about 1.2 m of dry open places, spreading by rhizomes. The root sweetens and flavours and soothes coughs. Too much raises the blood pressure.",
            "Caravan children are paid in liquorice root, and so are informers."),
        ["chamomile"] = (
            "Annual to 0.5 m of sandy arable land and saline steppe. The flower tea calms the stomach and the nerves. A yellow dye and a hair rinse.",
            "The Halfling answer to every illness, and to most bad news."),
        ["sage"] = (
            "Upright evergreen shrub of about 0.6 m of dry banks and stony ground. Antiseptic and astringent, a gargle. It flavours food. Toxic in excess.",
            "\"Why should he die who has sage in his garden\" is a Midlander proverb and a Saltroad joke."),
        ["thymeAndRosemary"] = (
            "Evergreen shrubs of dry slopes, rocks and scrub near the sea, thyme a mat of 0.2 m and rosemary a bush of 1.5 m. They flavour food. Thyme is a remedy for coughs, rosemary a tonic for low spirits. Their oils go into soap and perfume.",
            "Rosemary is carried at weddings and funerals. The same sprig will do for both, say the Swiftborn, who have little time between."),
        ["opiumPoppy"] = (
            "Annual of about 0.6 m, not known wild. The dried latex kills pain and brings sleep and is highly addictive. The seeds and their oil are eaten, and the oil serves for lamps and paint.",
            "The surgeon's friend and the debtor's ruin. In many towns only the temple may sell it."),
        ["valerian"] = (
            "Perennial of about 1.5 m of grassland, scrub and woods. The root is a tranquilliser, for sleep and cramps. Long use becomes a habit.",
            "Cats will follow whoever carries it. Catfolk consider the joke old."),
        ["stJohnsWort"] = (
            "Perennial of about 0.9 m of open woods, hedge banks and dry grassland. Taken against low spirits. Flowers and leaves give yellow, gold, brown and red dyes. It makes the skin burn in sunlight.",
            "Gathered at midsummer and hung in the byre against fire and worse."),
        ["foxglove"] = (
            "Biennial of about 1.2 m of acid soils in woods and on heaths. The leaves act on the heart. The dose that heals is close to the dose that kills.",
            "A physician is someone who knows how much. A poisoner is someone who knows a little more."),
        ["comfrey"] = (
            "Fast upright perennial of about 1.2 m of damp meadows, woods and streamsides. Laid on cuts, bruises, sprains and broken bones. Taken inwardly, it harms the liver.",
            "\"Knitbone\" in every language of the Midlands."),
        ["angelica"] = (
            "Biennial of about 1.5 m of moist shady places. Stalks, leaves and roots are eaten and the seed flavours drink. A remedy for the digestion and the circulation.",
            "Northmen candy the stalks for the winter feast. It is the only sweet thing in the house until the bees wake."),
        ["gentianAndArnica"] = (
            "Perennials of high mountain pastures. The gentian root is a bitter tonic for the digestion and once flavoured beer. Arnica is laid on bruises and sprains, on unbroken skin only: swallowed, it can kill.",
            "Dwarf brewers bitter their ale with gentian. Avalanche rescuers carry arnica."),
        ["nettle"] = (
            "Fast perennial of about 1.2 m of hedgerows, woods and rich waste ground. Young leaves are eaten as greens. The stem fibre makes string and cloth. Green and yellow dyes. The sting goes with cooking.",
            "Shirts of nettle cloth were what the poor wore through the Ashen Years, when the flax failed."),
        ["woad"] = (
            "Biennial of about 1 m of cliffs and cornfields, often on chalk. The leaves give a blue dye and preserve wood.",
            "The blue of the north. Northmen sworn guards are painted with it before a fight they do not expect to leave."),
        ["madder"] = (
            "Evergreen perennial of about 1 m of hedgerows and neglected ground. The roots give a red dye. The prickly leaves polish metal.",
            "Soldiers' red. A realm's madder fields are counted with its armouries."),
        ["flax"] = (
            "Annual of about 0.7 m, not known wild. The stem fibre makes cloth, sails, nets and paper. The seed is eaten, and its oil goes into paint and varnish.",
            "A Midlander girl spins her own shroud first, so that everything she spins afterwards is profit."),
        ["hemp"] = (
            "Annual of about 2.5 m of rich soils near houses. Fibre for cloth and rope. Seed and oil are eaten. It eases pain and sickness and can intoxicate.",
            "Every hull that carries Northmen carries a field of hemp in its rigging."),
        ["ginseng"] = (
            "Perennial of about 0.8 m of mountain forests. The root is a tonic against weakness, sleeplessness and strain.",
            "A root shaped like a man is worth his weight in silver. Canal folk physicians pay Amberblood diggers and ask no questions."),
        ["monkshood"] = (
            "Perennial of about 1.5 m of damp shady places and moist meadows. Highly toxic to the nerves: even touching it numbs the skin. The root was used outwardly against pain.",
            "Wolfsbane: herders of the north tip their wolf arrows with it and bury the wolf where it falls."),
        ["deadlyNightshade"] = (
            "Perennial of about 0.9 m of woods, thickets and hedges on limy ground. The whole plant is very poisonous, most of all the root and the ripe berries. It widens the pupils.",
            "Courtesans drop it in the eyes to look wide awake, and in the cup to make sure the other does not."),
        ["hemlock"] = (
            "Biennial of about 2 m of waste ground and damp places. Very poisonous, the seeds most: death comes by suffocation. Once given as a sedative and against pain.",
            "The cup that the old city courts offered a condemned citizen, as a courtesy."),
        ["rhubarb"] = (
            "Clumping perennial of up to 3 m of scrub, rocky ground and streamsides at high elevation. The root is a laxative and a tonic for the digestion. The leaves are poisonous.",
            "It crosses half the world by caravan, and each hand that carries it doubles the price."),
        ["glasswort"] = (
            "Annual of about 0.3 m of coastal sands, mudflats and salt marsh. The young stems are a pot herb. The ash is rich in soda, for soap and glass.",
            "Brinefolk soda burners and glassmakers cut it by the cartload at the autumn low tides."),
        ["cloverAndLucerne"] = (
            "Perennials of meadows and pastures, lucerne with a deep taproot. They feed the soil and are dug in as green manure. Leaves and sprouts are eaten. A yellow dye.",
            "\"Clover in, wheat out\" is the whole of Halfling farming as they explain it to others."),
        ["silphium"] = (
            "A herb of one dry coast whose juice was prized as medicine and perfume and sold for its weight in silver. It vanished when the land was let for sheep, and the last stalk found was sent to the emperor as a curiosity. Extinct in our world. Its relative the giant fennel is a perennial of 2.5 m of dry hills whose gum is burnt as incense and whose pith is a slow tinder.",
            "Here it survives in a few zones. The Saltroad clans that hold them do not say which."),
        ["dittany"] = (
            "Perennial of about 0.2 m of shady rocks in dry high mountains, native to one island. Once a wound herb. The leaves flavour drink and make a tea.",
            "Wounded goats are said to seek it out, and goatherds to follow the goats."),
        ["aloe"] = (
            "Evergreen succulent of 0.6 to 1 m of maritime sands and rocks. The leaf gel is laid on wounds and burns. The sap is a laxative.",
            "Every caravan cook grows one in a pot on the lead camel."),
        ["senna"] = (
            "Deciduous shrub to 3 m of semi-desert scrub, grassland and riverbanks. Leaves and pods are an old purge.",
            "Sold in the night markets of the Duskreaders next to the dates, as its remedy."),
        ["devilsClaw"] = (
            "Sprawling perennial with fleshy tubers, of veld and waste ground on clay or sand. The bitter root eases pain and swelling. The spiny fruits lame the feet.",
            "Sunlander herders dig the root for their own joints and curse the fruit for their cattle's hooves."),
        ["holyBasil"] = (
            "Aromatic perennial of 0.3 to 1 m with a woody base. A remedy for fevers, colds and the digestion. The stems are cut into prayer beads, and the oil repels insects.",
            "A pot of it stands in every Rainkeeper courtyard. The house is not a house until it is planted."),
        ["ipecac"] = (
            "Shrub of about 0.5 m of the rain forest understorey. The root brings up vomit and phlegm and is given for coughs and dysentery. An overdose is dangerous.",
            "What a Bitterblood antidote maker reaches for first: most poisons are best met coming back up."),
        ["gingerAndTurmeric"] = (
            "Perennial herbs of about 1 m with spreading rhizomes, known only from cultivation. Ginger flavours food and settles the stomach. Turmeric is a spice and a yellow to orange dye for silk and wool.",
            "Rainkeeper brides are rubbed yellow with turmeric."),
        ["thornApple"] = (
            "Erect annual to 2 m of waste places and sandy riverbanks. All parts are poisonous and narcotic. Given for asthma and pain and before surgery. The gap between dose and death is small.",
            "A Sunlander diviner takes it once, at initiation, and speaks of what he saw for the rest of his life."),
        ["cloudberryAndCrowberry"] = (
            "Low plants of about 0.3 m of cool bogs, moors and mountain tops. Both fruits are eaten raw or cooked and give a purple dye.",
            "Cloudberry bogs are owned by families in Northmen law, and trespass in berry time is paid for in silver."),
        ["bilberry"] = (
            "Suckering shrub of about 0.2 m of heaths, moors and woods on acid soil. Sweet fruit for eating and preserves. A blue or black dye and an ink from the fruit.",
            "Children come home blue to the elbows, and the scribe buys what they did not eat."),
        ["raspberryAndBramble"] = (
            "Suckering shrub of about 2 m of hedgerows, woodland edges and neglected land. Fruit, shoots and a leaf tea that is given in childbirth. A purple-blue dye.",
            "The hedge that feeds the poor. No lord has managed to tax it."),
        ["salmonberry"] = (
            "Deciduous shrub of about 1.8 m of moist places in and around woods on the wet coast. Fruit and peeled shoots are eaten. The leaves and root are astringent.",
            "It ripens when the first salmon run, and is eaten with them."),
        ["seaBuckthorn"] = (
            "Deciduous thorny shrub of about 6 m of coasts, dunes and sea cliffs, and inland across Asia. The sour fruit and its oil are a tonic and are laid on burns. Yellow and brown dyes, fuel and charcoal. It holds loose soil.",
            "Grass riders feed it to tired horses, and their word for it means \"shining horse\"."),
        ["hazel"] = (
            "Deciduous shrub or tree of about 6 m of woods and hedgerows. Nuts and their oil. The rods make baskets. Charcoal.",
            "A forked hazel rod finds water and ore in the right hands. Every well-finder owns one, and no two agree on how it is held."),
        ["apple"] = (
            "Deciduous tree of about 9 m, not known truly wild. The fruit is eaten raw, cooked and dried. The wood is turned and burnt. The seeds are poisonous in quantity.",
            "Halflings keep an orchard behind every burrow and sell cider to people who have never seen a Halfling sober or drunk."),
        ["sweetChestnut"] = (
            "Deciduous tree to about 30 m of mountain woods. The nuts are eaten roasted or ground to flour. Good fuel, fence posts and carpentry. The bark tans.",
            "The bread tree of hill villages that have no ploughland."),
        ["walnut"] = (
            "Rounded deciduous tree of about 20 m. Nuts and oil. Dyes and a tannin. A fine timber and a drying oil for paint.",
            "Planted at a son's birth for the furniture of his wedding. Nothing grows under it, and nobody sleeps there twice."),
        ["olive"] = (
            "Slow evergreen tree of about 10 m of woods and scrub in dry rocky places. The cured fruit is eaten. The oil is food, medicine, soap, lamp fuel and lubricant. A hard wood for turning.",
            "A grove outlives the realm that planted it. Swiftborn pickers were made for the harvest and are still paid by it."),
        ["fig"] = (
            "Deciduous tree of about 6 m among rocks and scrub on hot dry soils. The fruit is eaten fresh and dried and loosens the bowels. The latex curdles milk and burns the eyes.",
            "The traveller's tree: it fruits twice and asks for nothing. To cut one down by a road is a curse."),
        ["pomegranate"] = (
            "Deciduous shrub or small tree of 4 to 10 m on dry limestone soils. The fruit and juice are eaten. Rind and flowers give a red dye, an ink and a tannin. The bark kills tapeworm and can kill the patient.",
            "Whoever eats in the house of the dead stays there, the old stories say. Saltroad hosts offer one to seal a debt."),
        ["almond"] = (
            "Deciduous tree of about 6 m of thickets and rocky places. The seeds are eaten raw, roasted and ground, and give an oil for soap and the skin. The bitter kind is poisonous.",
            "The first blossom of the year. Poisoners prize the bitter tree that looks the same."),
        ["carob"] = (
            "Evergreen tree of about 15 m of rocky places near the sea. The sweet pods are eaten or ground and feed stock. The seed gives a flour and a gum.",
            "Jewellers weigh gems against its seeds, which are all of one weight, or so the jewellers say."),
        ["citron"] = (
            "Evergreen shrub or small tree to 4 m, known only in cultivation. The fruit and its candied peel are eaten. A perfume, and a repellent of moths.",
            "A ship that carries them loses no teeth, and Canal folk captains guard the knowledge like a chart."),
        ["datePalm"] = (
            "Evergreen palm of 25 to 30 m, not known truly wild. The fruit is a staple and a sweetener. Sap, thatch, fibre for rope, baskets and mats, wood and fuel.",
            "The Saltroad date gardener counts his palms as a herder counts cattle, and knows each by the taste."),
        ["pricklyPear"] = (
            "Evergreen cactus to about 5 m of dry, rocky places. The sweet fruit and the cooked pads are eaten. The pads feed stock, and the plant makes a fence. The fine barbed spines stay in the skin. It is the host of the cochineal insect.",
            "A hedge that feeds the goats, the children and the dyer."),
        ["jujube"] = (
            "Fast, often thorny deciduous tree of about 10 m on dry stony slopes. The fruit is eaten fresh and dried and is a tonic for sleep. Fuel, charcoal and hedging.",
            "Travellers who eat it in a strange land are said to forget the way home. The innkeepers who serve it do not deny this."),
        ["watermelon"] = (
            "Trailing annual with stems to 4 m of open ground in desert country. The fruit is water and food. The seeds are roasted and pressed for oil.",
            "The wild kind is the hunter's well. Duskreaders find the cool fruit in the dark as others find a lamp."),
        ["shea"] = (
            "Deciduous tree of 7 to 15 m with a dense spreading crown, of open parkland savanna. The seed fat is shea butter. Timber and charcoal.",
            "The women's tree: the nuts and the butter money are theirs by Sunlander custom."),
        ["tamarind"] = (
            "Slow tree of 20 to 30 m of low woodland, savanna and riverbanks, often by termite mounds. Pods, leaves and seeds are eaten. Timber, charcoal, a tannin and a leaf dye.",
            "The sour pulp polishes brass, and temple lamps are cleaned with nothing else."),
        ["mango"] = (
            "Large evergreen of 10 to 45 m with an umbrella crown. Fruit. Timber for building and boats. Bark and leaves dye silk. The sap can raise a rash.",
            "A Rainkeeper king is remembered by the mango groves he planted along the roads."),
        ["bananaAndPlantain"] = (
            "Evergreen giant herb of about 3 m of moist ravines and slopes. Fruit, flower and stem core are eaten. Fibre for cloth, rugs and paper. The leaves wrap food.",
            "The plate, the roof in a shower and the meal are the same plant."),
        ["coconut"] = (
            "Single-stemmed palm to 30 m of tropical seashores. The seed gives food, milk and oil. The husk fibre makes mats and rope. Timber, and charcoal from the shell.",
            "It floats to a new shore by itself. Shoalfolk claim a beach by planting three."),
        ["cacao"] = (
            "Small evergreen tree of about 8 m of the understorey of wet lowland rain forest. The seeds give cocoa and a fat.",
            "Boughwalkers drink it bitter and sell it to people who add honey and call it a luxury."),
        ["kolaNut"] = (
            "Evergreen understorey tree of 9 to 12 m of lowland forest. The seeds are a stimulant, chewed or made into drink. Timber for furniture and boats.",
            "Split and shared before any Sunlander bargain. To refuse the half is to refuse the deal."),
        ["oilPalm"] = (
            "Single-stemmed palm of 20 to 30 m of open forest on wet sandy soils by rivers. Palm oil and palm wine. The leaflets give rope fibre and the shells fuel a furnace.",
            "Sunlander smelters fire their furnaces with the shells and oil their iron with the fruit."),
        ["brazilNut"] = (
            "Very large evergreen of 25 to 56 m on deep alluvial soil that rarely floods. The seeds are eaten. Durable timber.",
            "The fruit falls like a thrown stone. Gatherers work in wide hats and short visits."),
        ["arecaPalm"] = (
            "Single-stemmed palm of 15 to 30 m of the understorey of wet forest. The seed is chewed as a stimulant. A tannin for dyeing. Too much brings vomiting and stupor.",
            "Offered to every guest in Rainkeeper country. The red spit on a road tells how far the next town is."),
        ["sagoPalm"] = (
            "Clumping palm of 6 to 25 m of lowland freshwater swamp. The pith of the stem is washed for starch, and a stem dies after it flowers. The leaves thatch.",
            "One trunk feeds a family for a month, and Bitterbloods farm nothing else."),
        ["palmyraPalm"] = (
            "Very tall single-stemmed palm of 20 to 30 m of hot dry monsoon country on sandy and alluvial soil. Sap, fruit and young seeds are food. Timber, fibre and thatch.",
            "The leaf is the Rainkeeper scribe's page, scratched with a stylus and rubbed with soot."),
        ["cottongrass"] = (
            "Sedge of about 0.6 m of peat bogs, acid meadows and marshes. The white seed hairs make candle wicks, tinder and pillow stuffing. The leaves are woven into mats.",
            "When the bog turns white, the reindeer calves have been born and the herders may move."),
        ["featherGrass"] = (
            "The plumed bunch grass that gives the steppe its silver sheen in the wind. General knowledge no page on it could be opened.",
            "Grass riders say the steppe has a pelt, and that it should be stroked the right way: they ride with the wind when they can."),
        ["tallgrass"] = (
            "Clump-forming perennial grass about 2 m tall of prairies, open ground and open woods. Forage and hay for horses and cattle.",
            "A rider can be lost in it standing. Widegaze build their mounds to see over it."),
        ["wildBarley"] = (
            "The wild grass from which barley was bred about ten thousand years ago. Barley resists dry heat, grows from the subarctic to the desert edge, and is fodder, malt for beer, and bread and porridge.",
            "Gnome seedsmen keep the wild grass in sealed jars beside the tame, in case the tame is lost again."),
        ["esparto"] = (
            "Grass of 1 to 1.2 m with clustered wiry stems, of dry sandy soil near southern coasts. Fibre for ropes, sandals, baskets and mats. The leaves make paper. Young plants are forage.",
            "The sandals of every Saltroad porter, and the rope of every well."),
        ["reedAndCattail"] = (
            "Tall perennials of 1.5 to 3.6 m of shallow water and wet ground, found almost worldwide. Reed thatches and makes baskets, paper and string. Cattail gives starchy roots, mats, tinder, rush lights and stuffing.",
            "A marsh village owes its lord reeds, not grain, and thatches his hall every seventh year."),
        ["papyrus"] = (
            "Clump-forming sedge to 5 m in great stands in swamps, shallow lakes and along streams. Paper, boats, cordage, sandals, mats and huts. Rhizomes and stems are eaten. Herodotus has the marsh people eat the lower stalk baked.",
            "The archives of the Old Realms were written on it, and what survives of them survives where it is dry."),
        ["bamboo"] = (
            "Fast evergreen giant grass of about 8 m, of woodland and mountain slopes. The young shoots are eaten. Timber, water pipes, weaving and paper.",
            "Canal folk scaffolding, water pipes and brush handles. It flowers once in a lifetime, and a famine follows the rats that follow the seed."),
        ["elephantGrass"] = (
            "Clumping grass with cane-like stems of 3 to 7 m, of damp grassland and forest edges. Forage. Fences, huts and thatch.",
            "The grass that hides the elephant and the lion. Sunlander herders burn it at the end of the dry season and walk behind the fire."),
        ["sorghum"] = (
            "Vigorous annual grass up to 5 m of the semi-arid tropics. Grain and flour, and a syrup from the sweet stems. Brooms, fencing, mats and roofing. The young or wilted plant is poisonous to stock.",
            "Millet and sorghum are the Sunlander grains, and the beer of every funeral."),
        ["sugarCane"] = (
            "Large perennial grass with canes of 2 to 3 m and more, known only from cultivation. Sugar, syrup and molasses. The stem is chewed.",
            "White sugar is a Rainkeeper secret and a physician's drug everywhere else."),
        ["wildRice"] = (
            "Perennial grass to 2 m of swamps, pond and stream edges and ditches, in water to 30 cm. The seed is cooked like rice and is a famine food. It crosses with the crop and is a weed of the paddy.",
            "Rainkeeper reservoir keepers weed it out of the paddies and eat it in bad years."),
        ["hops"] = (
            "Fast perennial climber of about 7 m of riverbanks and moist thickets. The cones flavour beer and bring sleep. Young shoots are eaten. Stem fibre for cloth and paper. The page read describes the plant from another continent's range.",
            "The line between ale lands and beer lands is the hop line, and brewers have fought over it."),
        ["grapeVine"] = (
            "Fast deciduous climber of about 15 m of riversides and damp woods. The fruit is eaten fresh and dried and made into wine. The leaves are eaten. Seed oil and cream of tartar.",
            "The vineyards of the Old Realms went wild in the Ashen Years. The Swiftborn were made to dress them."),
        ["blackPepper"] = (
            "Woody climber to 10 m with clinging roots. The dried fruit is black and white pepper. A remedy for the digestion and for fever. Pliny knew the long, the white and the black kind and their prices.",
            "Counted out by the corn in northern markets, and taken as rent."),
        ["vanilla"] = (
            "Climbing orchid with a succulent stem of 5 to 15 m and aerial roots, of warm wet lowland forest. The cured seed pods flavour sweet food and scent perfume.",
            "Each flower opens for one morning and is fertilised by hand. Boughwalker fingers are the right length."),
        ["rattan"] = (
            "Spiny climbing palm with stems as thick as a finger that pass 30 m. The stems make baskets, furniture, rope and hanging bridges. The shoots are eaten.",
            "The Boughwalker high roads are rattan, renewed strand by strand so that no bridge is ever old or new."),
        ["curareVine"] = (
            "Woody climber with stems to 30 m into the canopy. The source of an arrow poison that kills by stopping the breath, and only if it enters the blood: swallowed, it does no harm.",
            "A hunter tests the brew on a frog and counts its jumps. Three is good poison."),
        ["yam"] = (
            "Perennial climber with yearly stems to 15 m and a large tuber, not known wild. The tuber is boiled or baked. Raw, it is toxic.",
            "The yam barn shows a Sunlander farmer's standing, and the first tuber of the year is the king's."),
        ["bottleGourd"] = (
            "Fast annual climber with stems of about 9 m, not known wild. The young fruit is eaten. The hard dry shell makes bottles, bowls, ladles and instruments.",
            "The first pot and the first drum. A hunter calls the jaguar with one."),
        ["colocynth"] = (
            "Trailing perennial of sand dunes with a woody root. The dried pulp of the unripe fruit is a violent purge and a poison in excess. The seeds are roasted and give a lamp oil.",
            "The bitter apple: it looks like water in the sand and has killed more lost men than thirst."),
        ["flyAgaric"] = (
            "Mushroom with a red to orange cap set with white warts, of forests and pastures of the temperate and boreal north. Poisonous and hallucinogenic: sickness and confusion, rarely death. Once used to kill flies.",
            "Northmen seers eat it dried before they speak for the dead. The reindeer find it first."),
        ["cep"] = (
            "Mushroom with pores in place of gills that lives with the roots of spruce, pine, oak and beech. Valued for its nutty taste and meaty flesh.",
            "Dwarves and Gnomes grow mushrooms in the dark. This one they have to walk out and find, like everyone else."),
        ["truffle"] = (
            "Round fruiting body that grows underground on tree roots in open woodland on chalky soil. A prized food. Pliny describes it as a growth without root in dry sandy ground and rates the southern kind highest.",
            "Found with a pig or a dog. Stillfaces, whose nose is as keen as a hound's, need neither."),
        ["deathCap"] = (
            "Mushroom with a greenish yellow to tan cap of 4 to 16 cm, white gills, a ring and a bulb at the base, near oaks, birches and elms. It causes most deaths by mushroom, and cooking does not make it safe.",
            "Bitterbloods eat toadstools at table. A guest who is served the same dish should think about why."),
        ["fairyRing"] = (
            "A fungus that grows outward underground in a circle and puts up its mushrooms as a ring that widens each year. Old rings can be hundreds of metres across.",
            "Nobody ploughs through one. The Elves say the ring is a door, and do not say to where."),
        ["ergot"] = (
            "A fungus of rye, and of sorghum and pearl millet, that replaces the grain with purple-black horns. Bread from such rye brings convulsions, visions, gangrene and death. In small doses it stops bleeding after childbirth.",
            "A village that dances and burns in a wet year has eaten the rye wolf's bread."),
        ["foxfire"] = (
            "The glow of rotting wood, made by luminous fungi on dead wood in moist forest, most often in the tropics.",
            "Lanternkin mark their night paths with foxfire logs."),
        ["snowAlgae"] = (
            "Algae that live on the surface of snow and ice and stain it red.",
            "Red snow is the blood of the amarok's last kill. Hoarkin step around it."),
        ["iceEdgeLichensAndMosses"] = (
            "Lichens and mosses are almost the whole plant cover of the icy barrens, with a very few flowering plants in the most sheltered places.",
            "The ring of grey on a rock tells a Hoarkin guide how long the ice has been gone from it."),
        ["reindeerLichen"] = (
            "Bushy, branched lichen about 8 cm high that covers great areas of tundra and taiga and grows a few millimetres a year. The winter grazing of reindeer, moose and musk ox. A spirit is distilled from it.",
            "Grazed ground takes a generation to grow back, so a herding family's round is as long as a life."),
        ["icelandMoss"] = (
            "Branched bushy lichen about 7 cm high, brown to grey-white, of alpine and northern ground. Soaked, dried and ground, it goes into bread, soups and jellies. Reindeer and stock eat it.",
            "The bread of the Ashen Years in the north, and still the bread of the last weeks of winter."),
        ["beardLichen"] = (
            "Yellow or greenish hanging lichen of tree branches, in humid mountains up to 1.5 m long. An old remedy for coughs. Some kinds give an orange dye. It burns readily when dry.",
            "Old forest wears a beard. Elves judge a wood's age and health by it and settle only under bearded trees."),
        ["dyeLichen"] = (
            "Lichens of sea rocks that give a purple dye, orchil, when fermented. It dyed silk, wool and parchment and was the cheap everyday purple.",
            "The poor man's purple. Sumptuary law forbids it as strictly as the real one, and is obeyed less."),
        ["peatMoss"] = (
            "Mosses in dense cushions up to 30 cm tall of bogs, swamps and lake shores. The dead cells hold twenty times their weight in water. Dried peat is fuel. The moss dresses wounds and serves as bedding and lamp wick.",
            "What sinks in a bog does not rot. The Northmen have found Old Realms soldiers in the peat with their faces on."),
        ["bracken"] = (
            "Fern with creeping black rhizomes and fronds that can reach 5 m, one of the most widespread plants and a weed of pasture. The fronds thatch and are fed to animals, though the plant is toxic.",
            "See fern seed."),
        ["kelp"] = (
            "Large brown seaweeds of cold coasts, with blades of 1 to 3 m and giant kinds of 60 m. A forest for fish. The ash gave potash and iodine.",
            "Shoalfolk of the cold branch dive the kelp coasts, greased with seal fat."),
        ["eelgrass"] = (
            "Flowering plants with long leaves that grow from rhizomes in shallow, muddy or sandy coastal water. Food and shelter for sea life. The dried leaves packed glass and stuffed cushions. Dugongs graze such beds.",
            "Shoalfolk fence the meadows where their dugongs graze and count them as pasture."),
        ["lotus"] = (
            "Water plant about 1 m tall with a tuberous rootstock in the mud of large lakes. Roots, young leaves, stems and seeds are eaten. The leaves serve as plates and wrappers. Herodotus has the marsh people bake bread from a lotus.",
            "It rises clean out of mud. Every Rainkeeper temple pond grows it."),
        ["waterChestnut"] = (
            "Floating annual of slow rivers, lakes and ponds. The seed is eaten cooked or ground: raw, it is toxic. An important crop in the east. The seeds are strung as beads.",
            "Canal folk grow it in the same water as the fish and the ducks and tax all three."),
        ["waterHyssop"] = (
            "Creeping perennial herb of estuary mud flats, channel banks and wet fields. A chief remedy for complaints of the nerves. The leaves are eaten.",
            "Rainkeeper pupils are dosed with it before the examination of the scribes."),
        ["taro"] = (
            "Evergreen perennial of 0.4 to 2 m with a large tuber, of low stream and river banks. Corm, leaves and stems are eaten cooked. Raw, they burn the mouth.",
            "The first crop planted on new-cleared wet ground, before anyone trusts it with rice."),
        ["frostLotus"] = (
            "A pale water flower that opens on tundra lakes and rivers for the few days between the thaw and the midges, and whose petals stay frozen in the hand.",
            "Hearthbloods, whose blood is at fever heat, pay anything for it: it is the one thing that cools them."),
        ["glowcap"] = (
            "A mushroom that glows steadily, bright enough to read by, in limestone country: cave mouths, sinkholes and the roots above them. Foxfire is its faint natural cousin.",
            "Gnome and Dwarf halls are lit with it. It dims when someone in the hall lies, the Gnomes claim."),
        ["elderwood"] = (
            "A tree of any forest kind that has stood since the Dawn and woken a little. The wood does not rot and remembers the shape it is given.",
            "Some elderwoods are Amberbloods who took root. To fell one unasked is murder in their law and a fortune in everyone else's."),
        ["elvenSapflower"] = (
            "A small golden flower of low woodland whose sap keeps what is steeped in it from spoiling.",
            "Elves plant it where one of their own lies. A meadow of it is a graveyard, and its keepers do not sell."),
        ["dreamleaf"] = (
            "A broad-leaved shrub of wet tropical highlands, which carry temperate biomes in the generator. The chewed leaf gives true dreams: of what is, not of what will be.",
            "A Rainkeeper astrologer who is caught using it is dismissed for cheating."),
        ["mandrake"] = (
            "The real plant is a perennial rosette of open woodland, deserted fields and stony places. All parts are poisonous. It was a narcotic and a purge, and it holds hyoscine. Pliny's diggers take care that the wind does not blow in their faces, draw three circles round it with a sword and face west as they pull. The juice was given before cutting to dull the pain.",
            "The scream is why a dog is tied to the root, in the tale every digger tells and none has tested."),
        ["moly"] = (
            "A herb with a black root and a white flower. Men cannot dig it, only gods. It guards the one who carries it against drugs and spells.",
            "Sold in every market by people who could not have dug it."),
        ["fernSeed"] = (
            "The seed of the fern, which has none. It is said to bloom like gold or fire on Midsummer Eve. Whoever catches it finds buried treasure, and kept among coins it stops them from growing fewer. The fern itself is bracken.",
            "Thieves wait in the bracken at midsummer with a white cloth. Guards wait for the thieves."),
        ["perindensTree"] = (
            "A tree of the hot east with sweet fruit. Doves live in its branches and its shade, and the dragon, their enemy, fears both the tree and its shadow and keeps to the far side.",
            "A village under one sleeps without a watch. It pays for that in doves."),
        ["vegetableLamb"] = (
            "A plant of the steppe whose gourd-like fruit opens on a small living lamb, as the old travellers' tale has it.",
            "Grass riders say it is cotton, described by someone who had seen neither cotton nor a lamb. They are not always right."),
        ["singingMoss"] = (
            "A grey moss on wind-scoured rock that hums when the wind crosses the crystal under it.",
            "Miners follow the note to the vein, and stop their ears on the way down."),
        ["mirageLily"] = (
            "A white flower that opens for one night after rain where star metal has fallen. Its scent shows the walker water that is not there.",
            "Duskreaders, who see heat and not pictures, gather it safely and sell it to people who should not buy it."),
        ["stormthorn"] = (
            "A thorn tree that lightning seeks. Each strike leaves a bead of glass at its foot, and the tree lives.",
            "Herders camp well away from it, and glassmakers camp under it between storms."),
        ["lanternMangrove"] = (
            "A mangrove whose breathing roots glow blue-green at the ebb.",
            "The Shoalfolk harbour lights. A pilot steers by which trees are lit."),
        ["snareVine"] = (
            "A liana that tightens on what struggles in it and slackens on what holds still.",
            "Boughwalkers train it across the paths they do not want used. Children learn to hold still before they learn to climb."),
        // Not in the note: an invented kind, described here.
        ["giantMushrooms"] = (
            "Mushrooms of tree height that stand where the trees should: pale stalks as thick as an oak, caps as wide as a barn roof, brown, red or violet above and gilled beneath. Under them it is dim, damp and still, the ground is soft with spores, and the gills glow faintly at night. The flesh cuts like cork and dries as hard as wood.",
            "A wood of them springs up in one season where the trees fell sick, and no one sees it grow. Gnomes and Dwarves cut the caps for food and the stalks for beams, and count their rings as they would a tree's. Elves will not walk under them: they say the forest is being remembered wrongly."),
    };
}
