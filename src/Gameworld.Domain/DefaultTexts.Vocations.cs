namespace Gameworld.Domain;

// What each default vocation does and its lore: the opening paragraph and the line "Lore" of docs/research/vocations.md section 3.
// A kind that has no text here gets DefaultTexts.Missing.

public static partial class DefaultTexts
{
    internal static readonly Dictionary<string, (string Description, string Lore)> Vocations = new()
    {
        ["farmer"] = (
            "Works arable land: ploughs, sows, weeds, reaps and threshes, and keeps a few animals and a garden beside it. Most farmers feed their own house first and owe the rest to a lord, a temple or the market.",
            "The common estate, the measure against which every other standing is set. Bound by custom more than by law: the days owed on the lord's land, the common field, the village assembly. Harvest and sowing rule the year and its feasts. The conflicts are with the lord over dues, with the herder over trampled crops, with the miller over his toll. Fantasy twists: a people that will not plough (the Catfolk), a people that works only by day (the Noonfolk), land whose yield a seer blesses."),
        ["gardener"] = (
            "Tends trees, vines and gardens: crops that stand for years or need daily care, and that are sold more than eaten at home.",
            "Patient work with a long horizon: an olive tree outlives its planter. Orchards and vineyards are wealth that an army can destroy in a day, so gardeners favour peace and strong walls. The luxury crops are regional secrets, guarded by a town or a lineage. Fantasy twists: a grove that only one people can tend, a poison garden, a spice whose source is kept off every map."),
        ["herder"] = (
            "Keeps animals on pasture and moves with them: guards, breeds, milks, shears and drives to market.",
            "Lives apart from the village for much of the year, on the summer pasture or the open range, and is thought half wild for it. Wealth is counted in heads. Whole peoples are herders and scorn the plough. The conflicts are over grazing rights, wells, rustling and winter fodder. Fantasy twists: herds of beasts no farmer could keep, mounts bred for one clan only."),
        ["fisher"] = (
            "Takes food and goods from water: with net, line, trap and spear, from a boat or the shore, and by diving.",
            "A hard and uncertain living with its own saints and taboos: words not said at sea, days on which no boat goes out. The catch spoils in a day, so the fisher depends on the salter and the fishmonger. Fishing villages are poor, close and suspicious of the land. Fantasy twists: a people that dives where others drown, things in the net that should not be."),
        ["hunter"] = (
            "Lives from the wild land: hunts, traps, gathers, and knows the way through it.",
            "Free where the game is free, a poacher where the lord has claimed it. Hunting is the sport of nobles and the bread of the poor, and the two meet in the courts. The hunter is the one who knows the pass, the ford and the safe spring, and is hired for that knowledge by caravans and armies. Fantasy twists: prey that hunts back, a guild that takes contracts on beasts, a people that tracks by scent."),
        ["forester"] = (
            "Fells, hauls and saws timber, cuts firewood, and keeps the wood: plants, coppices and guards it.",
            "Two faces. The woodcutter is a poor man with an axe. The forester is the lord's officer who fines him. Forest law is hated law. Old woods have their own rules about which tree may fall. Fantasy twists: woods with a will, elder trees worth a ship each, peoples who judge every felling."),
        ["miner"] = (
            "Wins ore, coal and gems from the ground: digs pits and shafts, washes river gravel, follows a vein.",
            "Dangerous work, and free where it is rare: miners held their own courts and laws in many lands. A strike makes a town in a year and an empty one in ten. Miners are clannish and proud and carry tales of what knocks in the dark. Fantasy twists: the dwarven hold, things dug up that should have stayed, ore that only one people can smelt."),
        ["quarryman"] = (
            "Breaks, splits and dresses stone in an open quarry, and digs sand, gravel and clay.",
            "Heavy, dusty work for the strong and the unfree: quarries are where convicts and captives are sent. A good quarry is known by name, and a cathedral may be built from one hill. Fantasy twists: stone that holds a charm, quarries worked by giants or by the dead."),
        ["salter"] = (
            "Makes salt: boils brine, rakes sea salt from pans, cuts rock salt.",
            "Salt is the one thing every house must buy, so rulers tax it and smugglers run it. A salt town is rich, hot and bare, since the pans eat the woods around it. Oaths are sworn on salt. Fantasy twists: a people with salt in the blood, salts that keep the dead from rising."),
        ["miller"] = (
            "Runs a mill: grinds grain to flour and meal, presses oil, and takes a share of what he grinds as toll.",
            "Holds the lord's monopoly, since tenants must grind at the lord's mill, and is suspected by all of a heavy thumb on the scale. The mill stands apart, by the water, and draws tales. The miller is the richest commoner of a village and the least loved. Fantasy twists: a mill that grinds at night for other customers."),
        ["baker"] = (
            "Turns flour into bread and other cooked food for sale, and bakes the dough that households bring to the common oven.",
            "The most watched trade in a town. The weight and price of the loaf are fixed by law, and a baker who cheats is paraded with the loaf round his neck. Up before dawn. Bread riots start at the baker's door."),
        ["brewer"] = (
            "Makes drink: malts and brews ale and beer, presses and ferments wine and cider, makes mead.",
            "In the country brewing is women's by-work in every second house, and the ale-stake over the door is the first inn. In towns it becomes a trade, and in monasteries an art. Drink is safer than water and is wages, medicine and sacrament. Fantasy twists: dwarven beer, halfling cider, a vintage that remembers."),
        ["butcher"] = (
            "Slaughters and cuts up animals and sells the meat, and deals with all that an animal or a fish leaves: salts and smokes it, renders the fat.",
            "A rich and rough trade, kept to its own street and banned from fouling the river. Butchers are strong, armed with their tools and quick to riot, and town councils fear them. Blood and offal give the trade a taint that money does not wash off."),
        ["charcoalBurner"] = (
            "Turns wood into the fuels and stuffs that industry needs, by slow fire in the forest: charcoal, tar, pitch, potash, and lime from limestone.",
            "Lives in the wood by the smoking clamp for weeks, black, poor and seldom seen in the village, and is suspected of dealing with whatever else lives there. Outlaws hide among charcoal burners. Without them no iron is made."),
        ["smelter"] = (
            "Turns ore into metal: roasts, smelts and refines it in a furnace, and casts or hammers it into bars.",
            "A secret art. The smelter knows by the colour of the flame when the bloom is ready, and passes that on to a son only. In many peoples the smelter is a ritual figure, feared and set apart. The works move when the wood is gone."),
        ["tanner"] = (
            "Makes leather, fur and parchment from raw hides and pelts, by months of soaking in lime, bark and worse.",
            "The foulest trade in a town, banished downstream and downwind, and its people marry among themselves. Yet leather is in every shoe, saddle, belt and book, and furs are the mark of rank. The furriers are rich where the tanners are shunned."),
        ["weaver"] = (
            "Makes cloth and cordage from fibre and wool: spins, weaves, fulls, dyes and finishes.",
            "The first industry, and the first to split master from hand. Spinning is done in every cottage, mostly by women; weaving is a guild trade; the dyers and fullers are its foul end, the cloth merchants its rich one. Weavers' quarters are crowded, radical and quick to strike. Fantasy twists: cloth that hides its wearer, dyes that only one people can see."),
        ["smith"] = (
            "Works hot metal with hammer and anvil: makes and mends tools, shoes horses, and in a town forges blades, locks, nails and armour.",
            "The one craftsman every village must have, and the one most wrapped in tale: fire, iron and a strength that seems more than natural. The smithy is where news is traded. In a city the trade splits into a dozen guilds that quarrel over who may make what. Fantasy twists: the smith who forges under a ban or a blessing, rune blades, a people whose smiths are its priests."),
        ["carpenter"] = (
            "Works wood into buildings, vehicles, vessels and tools.",
            "The builder of the common world: house frames, carts, barrels, looms and mills are all his. A master carpenter raises a hall with a crew and a rope. Less secret than the smith and less grand than the mason, and found everywhere. The cooper's barrel is the shipping container of the age."),
        ["shipwright"] = (
            "Builds and repairs boats and ships.",
            "A ship is the largest machine the age can build, and the yard that builds it is a town's pride and its debt. The craft is by eye and by inheritance, with no drawings. Launches are blessed, and a ship has a name and a luck. Fantasy twists: keels of elder wood, hulls sung into shape."),
        ["mason"] = (
            "Builds in stone, brick and earth: cuts and lays stone, burns and lays brick, plasters, tiles and thatches, and plans works such as walls, bridges, wells and canals.",
            "The travelling aristocracy of the crafts. Masons go where the great works are, lodge together on the site, and keep their geometry to themselves. A master mason is architect and contractor in one. Fantasy twists: lodges with real secrets, dwarven stonework that needs no mortar, walls with something bound into the foundation."),
        ["potter"] = (
            "Shapes and fires clay: pots, bowls and jars, bricks and roof tiles.",
            "A humble and ancient trade that leaves more behind than any other. Potters work at the edge of town, where the clay and the fire risk are. A fine ware (a glaze, a white body) can make a town's name and is a guarded recipe."),
        ["glassmaker"] = (
            "Melts sand or crystal with a flux and blows, casts or grinds the glass.",
            "A prized and jealous art. Glass towns forbid their masters to leave, on pain of death in the tale. Window glass is for temples and the rich; a lens is a wonder. The furnaces never go out. Fantasy twists: glass that holds light, a scrying mirror, vials that keep a living flame."),
        ["leatherworker"] = (
            "Makes goods of leather: shoes above all, and saddles, harness, belts, gloves, purses, sheaths and bottles.",
            "The most numerous craft in any town, since shoes wear out. The shoemaker in his open stall hears and repeats everything, and is the proverbial town philosopher and radical. Shoemakers of new leather and cobblers of old fight over the line between them."),
        ["tailor"] = (
            "Cuts and sews cloth and fur into clothes, makes hats and hose, and deals in used clothes.",
            "Dress is rank made visible, and laws say who may wear what, so the tailor knows everyone's true standing and debts. Most people wear clothes second or third hand; the old-clothes dealer is as common as the tailor. A sedentary trade, mocked as unmanly and feared as gossip."),
        ["jeweller"] = (
            "Works precious metal, stones and ivory: makes plate, rings and reliquaries, cuts gems, strikes coin, and tests the fineness of metal.",
            "The richest of the crafts and the nearest to banking: the goldsmith keeps other people's treasure and strikes the ruler's coin. Trusted with wealth, so bound by the hardest oaths and the longest terms of office. Fantasy twists: gems that hold a spell, coin that cannot be clipped, a people that reads a stone's flaws at a glance."),
        ["artist"] = (
            "Makes images and ornaments: paints, carves and gilds for temples, courts and rich houses.",
            "A craftsman, not a genius: the painter belongs to a guild, often the saddlers' or the apothecaries', and is paid by the foot. Work is to order, for a patron who sets the subject. Only at a court does an artist gain a name. Fantasy twists: portraits that age for the sitter, statues that watch."),
        ["artificer"] = (
            "Makes mechanisms and instruments, and in a world with magic, devices that hold it.",
            "The fantasy vocation with the thinnest historical root: the makers of clocks, scales and locks, who were few and admired. In a fantasy world the artificer stands between the smith and the mage, and is distrusted by both guilds. Patents and secrets are the trade's whole capital. Typical conflicts: a device that puts a craft out of work, a commission from a ruler that cannot be refused."),
        ["merchant"] = (
            "Buys in one place and sells in another, in bulk and over distance, and lends, changes and insures money.",
            "The estate that fits no estate: neither fights, prays nor ploughs, and grows richer than those who do. Merchants rule the towns through their guild and buy their way into the nobility within three generations. Usury is a sin they pay the temple to forgive. Fantasy twists: houses that trade between peoples who cannot meet, contracts witnessed by something that enforces them."),
        ["shopkeeper"] = (
            "Sells small quantities to those who live near: from a stall, a shop or a pack.",
            "The small end of trade, mostly poor and often a woman. Hucksters buy at dawn and sell dearer by noon, and are fined for it every week. The pedlar carries needles, ribbons and news to places no merchant visits, and is welcome and suspected in equal parts."),
        ["carter"] = (
            "Moves goods and messages by land: with cart, pack animal or his own back.",
            "Rough men of the road who know every inn, toll and ford. Caravan masters are trusted with fortunes; porters are the poorest free men in a town. The road has its own law, and its own tolls, bandits and guides. Fantasy twists: beasts of burden no stable could hold, roads that are only open at certain hours."),
        ["sailor"] = (
            "Works a vessel: on the sea, on a river, on a lake or a canal.",
            "A people apart, with their own speech, pay by share and long absences. Half of every port is waiting for a ship. Pilots are licensed and guard their knowledge of the bar and the channel. The ferryman owns his crossing by right and is paid by all. Fantasy twists: winds that are bought, a people that sees the reef through the water."),
        ["innkeeper"] = (
            "Keeps a house where strangers eat, drink, sleep, wash and stable their animals.",
            "The host knows who came, with what and with whom, and is by turns informer, banker, broker and fence. An inn is where a stranger must lodge by law, and where a deal is witnessed. The tavern is the common room of the poor; the bathhouse has a worse name."),
        ["healer"] = (
            "Treats the sick, the wounded and women in childbed: with herbs, the knife, the hand and advice.",
            "Three ranks that despise each other. The learned physician reads and prescribes and never cuts. The barber-surgeon cuts, bleeds and shaves. The village healer and the midwife know herbs and are one accusation away from being called witches. Plague makes and breaks them all. Fantasy twists: healing that works, and its price."),
        ["servant"] = (
            "Works in another's household for board, lodging and a small wage: in the house, the kitchen, the stable and the yard.",
            "Service is a stage of life as much as a class: the young of the poor, and of the not so poor, serve in another house until they marry. A great household is a small court with its own ranks. The servant sees everything and is seen by no one, which makes the vocation the natural spy."),
        ["entertainer"] = (
            "Performs for pay: sings, plays, tells, acts, tumbles, dances or wrestles.",
            "Masterless and on the road, so classed with vagabonds; yet the same person at a court is a poet with a pension. Entertainers carry news and reputations between courts, and a mocking song can do a lord more harm than a raid. Fantasy twists: songs that bind, a people that lives a whole career in seven years."),
        ["courtesan"] = (
            "Sells company and the bed: from the woman or man of a town's public house to the kept companion of a court, who is paid as much for wit, music and discretion as for anything else.",
            "Tolerated, taxed and despised at once. A town licenses a public house or a street and makes the trade wear a mark; a preacher closes the house and the next council opens it again. At the top the courtesan keeps a salon, reads, plays and is painted, and hears what a minister says when he is off guard, so the vocation is a natural keeper of secrets, beside the servant. Most enter through poverty or are sold into it, and few leave rich. The conflicts are with the church, with the wives of the town and with the keeper who holds the debts. Fantasy twists: a people that changes sex in mid-life, a charm that is more than charm."),
        ["noble"] = (
            "Holds land and people by right of birth or grant: rules, judges, leads in war and lives from rents and dues.",
            "Those who fight, in the old division of society into those who pray, fight and work. Honour, lineage and land are the whole of life, and each is defended with the sword and the lawsuit. A noble who works with his hands loses the rank. Younger sons go to the temple, to war or to the bad. Fantasy twists: houses of another race, bloodlines with a gift or a curse."),
        ["official"] = (
            "Carries out the orders of a ruler, a lord, a town or a temple: collects, counts, judges, inspects and announces.",
            "The reeve was \"the pivot man of the manorial system\": a peasant, chosen each year, who answered with his own goods for the lord's harvest. Above him the bailiff and the steward ride between manors and audit the rolls. Officials are hated for the tax and needed for the peace, and every one of them is assumed to take a cut. Fantasy twists: an office that passes by examination, a judge who can tell a lie when he hears one."),
        ["soldier"] = (
            "Fights for pay, for a lord or for an oath: in the field, on the wall and at sea.",
            "A trade like another, hired by the season and paid late. In peace the soldier is a guard, a bully or a bandit; in war a looter. Companies have their own law, share out plunder by rank and sell themselves to the higher bidder. Veterans come home with scars, a little money and no craft. Fantasy twists: a people that feels no pain, companies of another race for hire."),
        ["guard"] = (
            "Keeps watch and order in one place: a gate, a wall, a road, a store, a person.",
            "The watch is a duty of every householder that the rich pay others to do, so the guard is a poor man with a lantern and a staff. He knows every face in his ward and can be bought for a drink. A bodyguard or a vault keeper is another matter: chosen for loyalty and paid for it."),
        ["priest"] = (
            "Serves a god or the spirits for a community: leads the rites, marks birth, marriage and death, teaches, and keeps the holy place. Includes those who live under a rule apart from the world.",
            "Those who pray. The village priest is a farmer's son with a little learning and a field of his own; the high priest is a lord. A monastery is a great estate with a rule: the cellarer feeds it, the sacrist keeps the church, the infirmarian nurses, the precentor leads the singing, the guest-master receives travellers. The clergy are judged by their own courts and pay no lay tax. Fantasy twists: gods that answer, rites that hold something back."),
        ["scribe"] = (
            "Writes for others: letters, deeds, accounts, court rolls and copies of books; and knows the forms that make a writing valid.",
            "Literacy is a trade. The clerk is often in minor orders and half a priest; the notary's sign makes a contract stand in any court; the advocate is rich and unloved. Scribes serve whoever pays, know every secret that was ever put in writing, and can forge it. Fantasy twists: a people whose word is physically binding, script that only some eyes can read."),
        ["scholar"] = (
            "Studies and teaches: letters, law, the stars, numbers, the past.",
            "Learning is slow, costly and mostly in the hands of the temple. A school teaches boys their letters; a college of masters and students is a rare and privileged body that answers to no town court. Scholars are poor until a patron finds them. Fantasy twists: lore older than the kingdoms, an examination that makes or breaks a house."),
        ["mage"] = (
            "Works magic by learning or by gift: casts, wards, enchants and advises.",
            "The learned mage is a scholar with a dangerous subject: trained for years in a college or by a single master, bound by the college's oath, and watched by temple and crown. The born mage is a child that a village fears and a college claims. Mages serve courts as advisers and weapons, and are the first blamed when a harvest fails. Typical conflicts: college against cabal, mage against witch hunter."),
        ["alchemist"] = (
            "Prepares substances: medicines, poisons, perfumes, acids, pigments and, by repute, gold.",
            "The respectable end is the apothecary, a guild shopkeeper who sells spices, drugs and sweets alike. The other end works at night with a furnace and a patron's money, promising gold. Both are licensed, taxed and suspected. An alchemist's shop is where a healer buys, a mage orders and a murderer asks no questions. Fantasy twists: potions that work, a people immune to their own poisons."),
        ["seer"] = (
            "Knows what others do not: reads signs, finds water and lost things, speaks with spirits, lifts and lays curses.",
            "The village's own magic, cheap, unlicensed and older than the temple. The seer is consulted for a lost cow, a sick child, a good day to sow, and is the first accused when things go wrong. At a court the same art is the astrologer's, well paid and politically deadly. The line between seer, healer and priest is thin, and the temple polices it. Typical conflicts: seer against priest, and the inquisition against anyone unlicensed."),
        ["labourer"] = (
            "Sells the strength of the body by the day: digs, carries, loads, mows, threshes and cleans, wherever hands are wanted.",
            "The landless and the toolless. In the country the labourer has a cottage and a garden and works other men's fields; in the town he waits at the hiring place each dawn. Wages are set by law after a plague and paid above it. The labourer is the reserve of every other vocation and the first to go hungry."),
        ["beggar"] = (
            "Lives on alms: too old, sick, maimed or unlucky to work, or unwilling.",
            "The deserving poor have a place: at the temple door, on the almoner's list, in the hospital. The sturdy beggar is whipped out of town. Beggars see and hear everything in a street and sell it. Fantasy twists: a beggars' king, a curse that passes with the coin."),
        ["thief"] = (
            "Takes by stealth or deceit, in a town: cuts purses, breaks houses, receives, smuggles, forges, and kills for pay.",
            "Crime in a town is organised like a craft: with masters, territories, dues and a peace of its own. The fence is a shopkeeper by day; the smuggler is a sailor or a carter who carries one more bale. The law hangs the poor thief and fines the rich one."),
        ["bandit"] = (
            "Takes by force, outside the towns: robs on the road, raids, rustles, poaches, and at sea takes ships.",
            "Most bandits were something else first: a soldier without pay, a peasant without land, a herder in a hungry year. The outlaw is outside the law's protection and may be killed by anyone. Some bands are a lord's deniable arm, some a people's resistance, some only hungry. Raiding is a season's work for whole peoples, with no shame in it."),
    };
}
