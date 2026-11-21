namespace Gameworld.Domain;

// What each default accessory looks like and its lore: the columns "Description" and "Lore"
// of docs/research/portrait-components.md sections 9.1 and 9.2, without the sources and the
// remarks on what is invented.
// A kind that has no text here gets DefaultTexts.Missing.

public static partial class DefaultTexts
{
    internal static readonly Dictionary<string, (string Description, string Lore)> Accessories = new()
    {
        ["amberAndGlassBeadString"] = (
            "A single string of beads worn close under the collarbone: rough honey-coloured amber beads alternating with small glass beads in blue, green and yellow, on a linen thread.",
            "A girl gets her first beads from her mother's kin and adds one for each great event of her life, so an old woman's string can be read like a tally. The amber is picked up on cold beaches after a storm, and it was among the first goods the ships carried south."),
        ["hammerAmulet"] = (
            "A small flat hammer of dark forged iron, as long as a thumb joint, hung head down on a leather thong at the base of the throat.",
            "The hammer is the tool of the first Shipwright, who drove the rivets that are the stars. It is given before a first voyage and touched before casting off, and it is buried with the wearer, who should come to the rowing bench with a tool in hand."),
        ["twistedNeckRing"] = (
            "A rigid ring of thick gold rods twisted like a rope, worn close around the base of the neck, with its opening at the front between two round knobs.",
            "A neck ring is given by a lord or taken from a beaten enemy and is never bought. Once closed around the neck it stays there, and the wearer is buried in it. Hill lords of the Midlanders wear one to claim descent from the tribes the Realm conquered."),
        ["toothAndBoneString"] = (
            "A cord of twisted sinew strung with the pierced teeth and claws of animals, with small carved bone beads between them, hanging to the breastbone.",
            "Every tooth is from an animal the wearer killed, and the old will ask about each one. An Orc adds the teeth of enemies worth remembering, and a bone carver makes the beads between. A string is laid out with its owner and never inherited."),
        ["blueGlazedBeadString"] = (
            "Three short strings of small glazed pottery beads in bright blue and blue-green, worn together close around the neck, with one beetle-shaped bead at the centre.",
            "The beads come out of the same kilns as the pots and are sold by the handful in every river town. Blue is the colour of the flood that the old cities counted their years by. At a burial the string is broken and the beads are shared among the mourners."),
        ["cowrieShellString"] = (
            "A row of small glossy white cowrie shells sewn side by side onto a leather band, worn as a close collar around the neck.",
            "Cowries pass as small change in the grassland markets, so a collar is a purse worn in the open, and a mother snaps shells off it to pay the midwife. Diviners throw them to read an answer. The Shoalfolk who gather them think the price a fine joke."),
        ["redCoralBeadCollar"] = (
            "Many strings of polished red coral beads stacked into a high collar that covers the neck from the collarbone to the chin, with longer strings of the same beads falling over the chest.",
            "Coral comes from a sea the forest kingdoms never see, so every bead counts as the king's and is lent to a title-holder for life. At the holder's death the collar goes back to the palace. A title-holder who comes to council without it has resigned."),
        ["marriageThread"] = (
            "A cotton thread dyed yellow with turmeric, strung with small black beads and one gold pendant the size of a fingernail, tied close around the neck.",
            "The groom ties the thread at the wedding in front of both families, with three knots. The shape of the pendant tells the wife's community, as the colour of her cloth does. She wears it until one of the two dies."),
        ["silverAmuletCase"] = (
            "A flat square case of hammered silver the size of a palm, stamped with lines of script and rows of small triangles, hung on a leather cord at the middle of the chest, over the robe.",
            "Inside is a strip of paper with a few lines of the written law, copied by a scholar for the wearer by name. The case is closed for good, and to open another's is theft of more than silver. A guide who loses a caravan gives up his case to the clan of the dead."),
        ["chainOfOffice"] = (
            "A heavy collar of flat gold links, each as wide as two fingers, lying over both shoulders and across the chest, with a badge hanging from it at the breastbone.",
            "A lord hangs the collar on a follower with his own hands, and the badge on it is the lord's. Sending it back ends the service without a word. The kingdoms that claim the Realm copy the links from its old statues."),
        ["ivoryAmuletString"] = (
            "A thong of seal hide strung with five tiny figures carved from walrus ivory, a seal, a bear, a bird, a fish and a whale, each smaller than a thumbnail, worn at the throat over the coat.",
            "A figure takes one bright season to carve, and each stands for a kind of animal the wearer has promised to take only in need. A child is given the first figure by the lodge. At the cold table a Hoarkin sells ivory by the tusk and never a figure off the string."),
        ["plainHoopEarrings"] = (
            "A plain hoop of bronze wire, as wide as a thumbnail, in each earlobe.",
            "The ears are pierced in childhood by a grandmother, with a thorn and a thread. The first hoops are the first thing a person owns that is not a garment, and a pawned pair is the last thing a poor house gives up."),
        ["pearlDropEarrings"] = (
            "From each earlobe a short gold wire carrying two white pearls that hang side by side and touch as the head moves.",
            "Pearls come up from the tidal shallows one dive at a time, and a pair is priced at a year's rent of a farm. A matron wears them to be heard as well as seen. The divers who sell them still tell of the quota."),
        ["filigreeFlowerEarrings"] = (
            "A flower of gold wirework as wide as two fingers covering each earlobe, set with small pearls around a bead of red coral at its centre.",
            "The earrings are made by a community of goldsmiths and are part of a daughter's portion, weighed and listed before witnesses. A bride from a poor house borrows a pair from the temple for the day and returns it with a measure of rice."),
        ["singleEarRing"] = (
            "One thick ring of gold in the left earlobe, with a small bead of blue stone threaded on it.",
            "Among riders and sailors the ring is grave money: whoever finds the body may take the ring and owes the dead a burial. A man who has sold his ring is said to have nothing left to lose."),
        ["bronzeUpperArmBand"] = (
            "A broad band of polished bronze, three fingers wide, worn tight around the bare upper arm above the elbow.",
            "A leader gives a band to a man who has stood by him once, and men count each other's arms. In a Hearthblood hall the larder-lord gives the first band after a first full winter of work in the pass. It is worn on the bare arm in any weather."),
        ["stackedGlassBangles"] = (
            "A dozen thin bangles of coloured glass, red and green, stacked on each wrist from the hand to the middle of the forearm.",
            "A bride is given her bangles by her mother's sisters and wears them day and night; a house knows by their sound where she is. They are broken on a stone on the day she is widowed."),
        ["beastHeadBangle"] = (
            "An open bangle of solid gold on each wrist, its two ends cast as the heads of lions that face each other across the gap, their manes inlaid with blue stone.",
            "The bangle is a court's gift of honour: a ruler closes one on the wrist of an envoy who has pleased him, and the beast tells which court. A merchant house will honour a letter from a man who wears one."),
        ["serpentArmlet"] = (
            "A gold snake coiled three times around the bare upper arm, its head raised and its eyes set with small red stones.",
            "The snake is worn against poison and fever, and the port cities say the first was made for a queen who feared her own table. A husband gives one after the safe birth of a first child."),
        ["openBrassBangle"] = (
            "A thick open ring of yellow brass around each wrist, shaped like a horseshoe, its two flared ends facing each other.",
            "The bangles are small money that is worn: a market debt or a diviner's fee is paid by slipping one off the arm. The big payments are still made in cattle. Casters of the forest towns make them in clay moulds, a hundred at a pour."),
        ["goldSignetRing"] = (
            "A heavy gold ring on the little finger of the left hand, its flat oval top set with a dark red stone cut with a small device in reverse.",
            "Pressed into wax, the ring binds its owner as his presence would. It goes to the heir at the deathbed, and the ring of a man with no heir is broken with a hammer before witnesses. A stolen ring is a stolen name."),
        ["bronzeSealRing"] = (
            "A plain bronze ring on the forefinger, its round flat top engraved with a simple mark of crossed lines.",
            "The mark is entered with a guild or a moot and is the nearest thing to a written name its owner has. A journeyman is given the ring on the day he is made free of his craft. Lost, it is cried in the market for three days."),
        ["claspedHandsRing"] = (
            "A silver ring whose top is formed as two small hands clasped together.",
            "It is given at the betrothal, hand to hand, before witnesses, and the pair wear matching rings. Sent back, it ends the promise without a word. A widow moves hers to the other hand."),
        ["cabochonStoneRing"] = (
            "A gold ring set with one large polished stone, a deep blue sapphire rounded like a drop of water, held high in a plain gold collar.",
            "The stone is worn for what it does: blue against poison and false witness, red for land and titles. The ring of a high priest of the Temple is kissed by anyone who asks a favour. Such stones are left by will, by name."),
        ["scarabSwivelRing"] = (
            "A ring of gold wire on which a blue-green glazed beetle the size of a fingertip turns on a pin, its flat underside cut with signs.",
            "Turned over and pressed into wet clay, the beetle seals a jar, a door or a letter. An official's beetle carries his office, and when he is dismissed its face is ground flat. The river towns bury a man with his."),
        ["archersThumbRing"] = (
            "A broad ring of pale polished horn on the right thumb, with a flat lip that covers the pad of the thumb.",
            "A child gets a ring on the day they can draw a grown bow, cut to that one thumb, and a ring is never lent. A khan's officers are known by green stone. A dead archer's ring is tied to the bow case that hangs in the tent."),
        ["bowFibula"] = (
            "A bronze brooch shaped like a small bow, with a coiled spring at one end and a catch at the other, like a large safety pin, fastening the cloak at the right shoulder.",
            "They are sold by the dozen at every market on an old Realm road. Soldiers lose them and ploughmen find them. Custom lets the finder keep one that turns up in a furrow, and asks for one found by a boundary stone to be handed to the lord."),
        ["ringBrooch"] = (
            "A small ring of silver as wide as two fingers with a pin across it, closing the neck opening of the gown, the ring engraved with a short line of letters.",
            "The usual gift between lovers: the letters say something only the giver and the wearer need to understand. Pinning it on is the answer. A brooch given back unpinned is a refusal that nobody else need hear of."),
        ["penannularCloakBrooch"] = (
            "A ring of silver as wide as a palm, open at one point between two flat round ends, with a pin twice as long as the ring is wide thrust through the cloak at the right shoulder and pointing upward.",
            "The width of the ring is the rank of the wearer. A leader gives one to a poet for a praise poem that is still said a year later. The law of the assembly says how far the pin may stand out before its wearer pays for a neighbour's scratch."),
        ["garnetDiscBrooch"] = (
            "A round gold brooch as wide as three fingers, its whole face covered with small flat dark-red garnets set in a pattern of gold cells, worn at the centre of the chest.",
            "Garnet work dates from the first generations after the Ashen Years, when a lord had gold from the fallen Realm and nobody to tell him what his wife might wear. The brooches are heirlooms now. Dwarves buy broken ones for the stones."),
        ["pilgrimBadge"] = (
            "A thin badge of grey cast pewter the size of a large coin, showing a small temple tower in a round frame, pinned to the front of the hood or the hat.",
            "Every great temple sells its own badge at the gate, cast by the thousand. Three or four on one hat mark a person who has seen the world, or the walking part of it. A badge is buried with the pilgrim who earned it."),
        ["chainedTriangleBrooches"] = (
            "Two large triangular brooches of silver, each as long as a hand, pinned point down at the shoulders to hold the draped dress, and joined across the chest by a silver chain hung with red coral beads.",
            "The silver is given at the wedding and is the woman's own, whatever becomes of the marriage. In a bad year she sells it a link at a time, and buys the links back before she buys anything else. A daughter inherits the pair."),
        ["goldStagPlaque"] = (
            "A flat pierced plaque of gold the size of a palm in the shape of a running stag, its legs folded under it and its antlers laid along its back, fixed to the breast of the coat.",
            "The beast is the clan's. Plaques for sworn brothers are cut from one sheet of gold, so two that fit edge to edge prove an oath. A khan who breaks up the clans has the old plaques melted."),
        ["plainBrowCirclet"] = (
            "A narrow band of dull silver, a finger wide, worn level around the head across the brow, over loose hair.",
            "A circlet says that the wearer is of a ruling house and does not rule yet; it is put off on the day a crown is put on. Elves leave theirs unpolished, so that it does not flash among the trunks."),
        ["whiteClothDiadem"] = (
            "A white silk ribbon two fingers wide tied around the head across the brow and knotted at the back, its two fringed ends hanging to the shoulders.",
            "The eldest of the council ties it on before the people, and whoever unties it has unmade the king. It costs less than any crown, which is the point: the king is the knot. A dead king's ribbon is burned."),
        ["oakLeafWreath"] = (
            "A wreath of fresh green oak leaves with a few acorns, bound on a ring of twig and worn level on the head.",
            "A custom kept from the Realm. The wreath is given by the one whose life was saved, in front of the whole line. The leaves wither in a week, and the right to a seat at any feast of that company lasts for life."),
        ["beadedFringeCrown"] = (
            "A tall cone-shaped crown covered all over in small beads of blue, white and red, with a beaded bird at its tip and a fringe of bead strings hanging from its rim to the chin, veiling the face.",
            "Behind the fringe the king is every king of his line, and nobody has seen his face since he was crowned. A council that deposes him sends for the crown and leaves the man. Only a smith may mend it."),
        ["phoenixCrown"] = (
            "A tall crown of gold wire covering the whole top of the head, set with gold dragons and with phoenixes inlaid in bright blue feather, with pearls and red stones, and a string of pearls hanging at each temple.",
            "The number of phoenixes is the rank, and any clerk can count it. In the Scattered Age no governor's wife wears nine: nine is for the empress of the emperor to come."),
        ["goldNoseRing"] = (
            "A thin gold ring as wide as a thumbnail through the left nostril, with one small pearl threaded on it.",
            "The husband's mother puts it in at the wedding feast, and while the husband lives it stays. A wife who takes it out in front of his kin has left him."),
        ["belledAnklets"] = (
            "A chain of silver links around each ankle, hung with a row of tiny round bells.",
            "A daughter gets her first pair when she can walk, and a house listens for its women. Temple dancers wear a hundred bells on each leg."),
        ["jadeBeltPendant"] = (
            "A flat carved disc of white jade the size of a palm, hung from the sash on a knotted red silk cord, with a red silk tassel below it.",
            "Jade is conduct made visible. The pendant knocks against the sash when its wearer hurries, so a person of standing walks in a way that keeps it quiet. A disgraced official hands it in with his badge."),
        ["foreheadPendant"] = (
            "A fine gold chain laid along the centre parting of the hair and hooked into it at the crown, ending in a small round gold pendant set with a red stone that rests on the middle of the forehead.",
            "The bride's mother puts it on her on the wedding morning, and it is worn on feast days after that. The stone is said to keep the thoughts cool. A widow gives hers to her eldest daughter."),
        ["carvedHairComb"] = (
            "A small curved comb of dark carved wood pushed into the front of the hair knot, so that its carved back stands above the hair.",
            "A mother gives a comb at the hair-pinning, and a woman is laid out with the one she was married in. The carving shows the flower of the month she was born."),
        ["templeRings"] = (
            "Three rings of bronze wire, each as wide as two fingers, hanging one below another beside each temple from a cloth band tied around the head.",
            "The pattern of the rings tells the village a woman was born in, and she keeps it when she marries into another. A girl wears one ring a side and a wife three. They are taken off for mourning."),
        ["bloodLampVial"] = (
            "A sealed glass vial as long as a finger joint, holding one drop that gives a steady blue-green glow, hung on a fine chain at the hollow of the throat.",
            "Each spouse wears the other's drop, so each knows at any hour that the other is alive. A vial that has gone dark is worn for one more year and then given to the lamp hall."),
        ["signalMirrorDisc"] = (
            "A round disc of polished bronze as wide as a palm, bright as a mirror on its face, hung on a leather strap at the middle of the chest.",
            "A child of a Watch family is given the mirror on the day they can send and read the whole code. It is used every day and polished by nobody else. The mirror of a person cast out for lying about seeing is scratched across before they go."),
        ["scentedWristCord"] = (
            "A cord of undyed plant fibre knotted at close intervals and wound three times around the left wrist.",
            "The cord holds the scent of the wearer's house and is renewed at each visit home; a Stillface far away reads it as others read a letter. To a human it is a piece of string. A cord that has lost its scent is burned."),
        ["groovedRelayRing"] = (
            "A broad ring of plain bronze on the forefinger, its outer face cut with fine parallel grooves that run all the way round, one beside another.",
            "Each groove is one generation that has held the town's Long Work, and the ring is given at the handing-on. A Swiftborn far from home turns it on the finger to remember that the work is still being built."),
        ["shapedSaltCrust"] = (
            "The white salt crust along the brow worked into a row of small even points, like the edge of a saw, from temple to temple.",
            "Fashion changes with the season: points one year, a smooth ridge the next. A pan master brushes the brow clean before a weighing, to show a plain face. A mourner lets the crust grow as it will."),
        ["bandedTusks"] = (
            "A ring of hammered iron around the base of each tusk, and a cap of bright brass over the tip of the left one.",
            "A ring is hammered on cold, by the smith of the hearth, for each raid a warrior has come back from. The brass cap is a chief's, cut from something taken. A chief who loses the post loses the cap and keeps the rings."),
        ["shellBraidBeads"] = (
            "Small white shells and discs cut from mother-of-pearl threaded onto short braids, so that they lie along each braid from the scalp to the nape.",
            "The shells are taken from the family's own reef, and a diver adds one for each season of deep diving. The loser of a dispute settled by the breath cuts one off."),
    };
}
