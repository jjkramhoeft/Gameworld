namespace Gameworld.Domain;

// What each default outfit looks like and its lore. The descriptions follow
// docs/research/portrait-components.md sections 4, 5 and 8.2, without the sources: where an outfit
// is dyed, the marks {main} and {accent} stand in the place of its colours, and an outfit that
// decides its colours names them. The lore is written in this file, from the column "Worn by"
// of the note and from docs/races-basis.md.
// A kind that has no text here gets DefaultTexts.Missing.

public static partial class DefaultTexts
{
    internal static readonly Dictionary<string, (string Description, string Lore)> Outfits = new()
    {
        ["kirtleAndApron"] = (
            "Fitted gown of undyed grey wool laced at the front over a white linen shift that shows at the neck and wrists, sleeves pushed up, a knotted linen kerchief on the head. Below: a linen apron tied at the waist and an ankle-length skirt tucked up for work.",
            "The dress of every farm and kitchen of the ploughed lands. A girl sews her first one herself, and the apron is the only part she owns more than one of. Cloth that has never seen a dye vat shows honest work, the peasants say. The townsfolk say it shows an empty purse."),
        ["strapDressAndBrooches"] = (
            "Tube dress of coarse {main} wool hung from a strap over each shoulder, each strap fastened at the front with a domed oval bronze brooch, a string of glass and amber beads slung between the two brooches, over a long-sleeved linen shift; a {accent} wool cloak pinned at the chest. Below: a belt with a small leather purse.",
            "The dress of the mistress of a farm on the cold coasts, who carries its keys at her belt. The brooches are the first thing a bride is given and the last thing a widow sells, and a woman is buried in hers. Blue is the dear dye, so a blue dress says that the last voyage went well."),
        ["sidelessSurcoat"] = (
            "Floor-length sleeveless overgown of {main} cloth with armholes cut open from shoulder to hip and a narrow front panel edged in white fur, showing a tight-sleeved {accent} gown and a jewelled hip belt beneath.",
            "Court dress of the kingdoms that claim the Realm. Priests preach against the open sides, which they call the windows of hell, and every noblewoman owns one all the same. It is worn at court and at ceremony, and never at work."),
        ["highBeltedGown"] = (
            "Voluminous gown of heavy {main} wool with a high standing collar and wide funnel sleeves lined with fur, belted just under the bust, the sleeve edges cut into scallops and lined in {accent}. Below: the skirt falls in deep folds to the floor.",
            "The winter and feast-day gown of a merchant's wife or a lady of the gentry. The cloth in the sleeves alone would dress a peasant family, which is the point of them. Town councils pass laws on how much fur a guild wife may wear, and fine her husband when she wears more."),
        ["queensMantle"] = (
            "Semicircular mantle of {main} cloth lined with white fur and fastened across the chest with a jewelled clasp, over a gown embroidered with {accent} thread, and a gold crown set with stones. Below: the mantle trails behind as a train.",
            "The mantle is put on a queen at her crowning and is worn when the court sits. It belongs to the crown and not to the woman: a queen who is set aside leaves it on the chair. The length of the train is fixed by custom, and a duchess whose own train comes near it is asking a question."),
        ["nunsHabit"] = (
            "Loose tunic of black wool, a long plain panel hanging from the shoulders front and back, a white linen wimple covering the throat and cheeks, and a black veil over a white under-veil. Below: a knotted cord at the waist and a hem to the ground.",
            "A woman who enters a temple house gives up her own clothes at the gate and is given these. A novice wears a white veil until her vows, and the black one is put on her by the eldest sister. The knots in the cord are her vows, and she is buried in it."),
        ["furParka"] = (
            "Hooded pullover coat of brown caribou hide worn fur outward over an inner coat with the fur turned in, the hood edged with a ruff of long wolf fur around the face, bands of pale fur set in at the shoulders. Below: long rounded flaps front and back, fur trousers and sealskin boots.",
            "The coat of those who hunt and fish at the edge of the ice. It is sewn by the women of the house, with sinew, in stitches so small that water cannot find them, and a hunter's life hangs on his wife's needle. A coat is made for one body and is not lent."),
        ["bandedHerderTunic"] = (
            "High-collared pullover tunic of {main} cloth with bands of {accent} at the collar, shoulders and cuffs, the collar embroidered in pewter thread, a fringed scarf at the neck and silver jewellery. Below: a belt set with silver buttons, a flared hem to the knee, hide leggings and boots.",
            "The tunic of those who follow the herds across the tundra and the northern forest. The bands tell the valley a herder comes from, and the buttons of the belt tell whether to ask after a wife or a husband: square for the married, round for the unmarried. Hide is for work and cloth for feasts."),
        ["pinnedTunicAndWrap"] = (
            "Ankle-length tunic of fine pleated white linen pinned along the shoulders and upper arms with small brooches to make elbow sleeves, belted and bloused at the waist, with a large rectangular {main} wool wrap draped over the left shoulder and around the body; a key-pattern border in {accent} at the hems.",
            "The dress of the women of the warm-coast towns, of every rank. The finer the linen, the richer the house, and the wrap is drawn over the head out of doors. It is woven at home, and a bride brings the loom with her."),
        ["priestessWhites"] = (
            "White wool gown falling in long straight folds from shoulder straps, a white linen mantle over it, a white woollen band wound around the head with red and white ribbons hanging in loops over each shoulder, and a short white veil with a purple border over the head when outdoors.",
            "The dress of the women who tend a sacred fire. They are chosen as children and serve for thirty years, and the fire must not go out in that time. Even a magistrate gives way to them in the street, and a condemned man who meets one by chance on his way to the block goes free."),
        ["jewelledCourtCollar"] = (
            "Tight-sleeved silk tunic in {main} under a wide collar of cloth of gold that covers both shoulders, thick with gems and edged with rows of pearls, a long gold-embroidered panel hanging down the front, and a crown with strings of pearls hanging at the temples.",
            "Court dress of the old cities on the salt road, kept from before the Ashen Years. The collar is so heavy that a lady of the court learns to stand in it as a child, and it is handed down from mother to daughter. Its pearls are counted when it is put on and again when it is taken off."),
        ["linenSheathAndBroadCollar"] = (
            "Close-fitting sheath of white linen held up by two shoulder straps, a sheer pleated linen shawl over the shoulders, a broad collar of rows of blue, green and red beads lying on the chest, and black lines of kohl around the eyes. Below: the sheath reaches the ankle.",
            "The dress of the women of rank in the hot river towns. The sheerer the linen, the higher the house, and the best is called woven air. The collar is laid on the chest of the dead as well, so that they are known for who they were."),
        ["desertBodyWrap"] = (
            "One long cotton cloth, about four metres, dyed {main}, wrapped around the whole body over a shirt and drawn up over the head as a hood, the face left open.",
            "One cloth is a woman's whole wardrobe in the dry country: dress, hood, blanket and shade for a child. It is plain for every day and bright for a feast. A woman can tell a friend at half a mile by the way she has thrown hers."),
        ["courtKaftan"] = (
            "Front-opening robe of patterned {main} silk velvet woven with gold thread, closed down the chest by a row of small buttons, over a fine shirt, with a wide {accent} sash at the waist.",
            "A kaftan is the gift of a ruler: a governor, an envoy or a merchant who has pleased the court is sent one as a robe of honour, and wears it at every audience after. To be seen in a better one than one's patron is unwise. They are stored flat, wrapped in cloth, and left by will."),
        ["wrappedRidingCoat"] = (
            "Long coat of wool or cotton in {main}, the left flap wrapped over the right and fastened with clasps at the right shoulder and under the arm, bound at the waist with a broad {accent} silk sash, and a fur-trimmed hat. Below: the coat flares to below the knee over felt boots.",
            "Both sexes of the Grass riders wear the same coat, and sleep in it. The fold over the chest is a pocket that holds a bowl, a flint and sometimes a lamb. Silk is for feasts and plain wool for every day, and the sash is taken off only among kin."),
        ["drapedSari"] = (
            "One unsewn {main} cloth, wrapped and pleated at the waist, its decorated end carried across the chest and over the left shoulder, with a border in {accent}, over a short fitted bodice. Below: the pleats fall to the feet.",
            "A Rainkeeper woman of every rank wears one unsewn cloth, and its colours tell her community as surely as a name. It is cotton for most and silk with a gold-thread border for the rich. A bride's is red, and she keeps it for life."),
        ["jacketAndHighSkirt"] = (
            "Short cross-collared jacket of thin {accent} silk with long sleeves, tucked under a long {main} pleated skirt that is tied high above the bust with a ribbon, and a long sheer scarf draped over both arms.",
            "The dress of the gentry women of the Canal folk. The skirt is tied higher or lower as the fashion of the capital changes, and a province is judged by how many years behind it is. Ordinary women wear the same cut in plain cloth, with the skirt tied at the waist."),
        ["layeredCourtRobes"] = (
            "Many unlined silk robes worn one over another, each a different shade, so that bands of colour show in steps at the neck and at the wide sleeve openings, with a short patterned {main} silk jacket on top. Below: long {accent} pleated trousers and a white train trailing behind.",
            "A lady of a great house chooses the colours of her layers by the season and the day, and is judged on the choice. A set that is one shade wrong is remembered for years. The whole weighs as much as armour, and its wearer does no work of any kind."),
        ["fieldJacketAndStrawHat"] = (
            "Plain hip-length jacket of undyed hemp with the sleeves rolled, and a wide conical hat of woven straw tied under the chin with a cloth strap. Below: loose trousers rolled to the knee, bare feet.",
            "The dress of those who grow the rice and carry the loads of the Canal folk, men and women alike. The hat is sun shade, rain cover and basket. Officials are taught that the whole realm stands on these bare feet."),
        ["breastClothAndSarong"] = (
            "A long patterned cloth in {main} wound tightly around the torso from the armpits to the waist, the shoulders bare, held by a {accent} sash at the midriff. Below: a second patterned cloth wrapped as an ankle-length skirt.",
            "The dress of the women of the wet coasts and the islands, from the dyers and weavers to the ladies of a court, who wear it in finer cloth. The patterns are drawn in wax before the dyeing. Some of them belong to one family, and to wear another's is theft."),
        ["wrapperAndBlouse"] = (
            "Loose wide-sleeved blouse of {main} tie-dyed cotton, a matching cloth tied as a head wrap, and a second cloth in {accent} folded over one shoulder. Below: a large rectangular cloth wrapped around the waist to the ankle.",
            "The dress of the market women of the Sunlander towns, who hold the trade in cloth and in food. A trader wears her best to market, since nobody buys from a woman who looks as if she needs the sale. Hand-woven strip cloth is kept for feasts."),
        ["stripWovenRoyalCloth"] = (
            "A great cloth made of narrow hand-woven strips sewn edge to edge, in blocks of {main} and {accent} geometric pattern, wrapped around the body and over the left shoulder, with heavy gold jewellery.",
            "The cloth of Sunlander kings, queen mothers and title-holders. Every pattern has a name and a proverb, and a king answers a petitioner by the cloth he puts on that morning. The weavers are men of one town, and they weave nothing else."),
        ["redHerderCloth"] = (
            "A sheet of red cloth, or of softened hide rubbed red, wrapped around the body and knotted over one shoulder, a second sheet worn as a cape, stacked flat collars of coloured beads around the neck and metal hoops in stretched earlobes.",
            "The cattle herders of the open grass wear red, the colour that lions are said to fear. The bead collars are made by the women, and their colours give the age set and whether the wearer is married. Before cloth came up the trade roads the sheet was hide."),
        ["healersGownAndSatchel"] = (
            "Plain long-sleeved gown of undyed linen with the sleeves tied back, a clean white apron and head cloth, a leather satchel on a shoulder strap and bundles of dried herbs at the belt.",
            "A healer or a midwife is known by her clean apron and her satchel, and is let through any door at any hour. The satchel passes from a woman to the girl she has taught. What is in it is not shown to the temple."),
        ["huntersHoodAndJerkin"] = (
            "Deep hood with a short shoulder cape of olive-green wool, a sleeveless jerkin of brown leather laced at the front over a rough linen shirt, a quiver strap across the chest and a leather bracer on the left forearm. Below: a belt with a long knife, wool hose and soft high boots.",
            "The dress of those who live by the forest: hunters, trappers and a lord's foresters. The green is boiled from weld and woad until it is the colour of nothing in particular. A forester's hood is his warrant, and a poacher caught in one pays double."),
        ["sorceressRobe"] = (
            "Floor-length robe of {main} wool with wide sleeves and a deep hood, clasped at the throat with a silver brooch, the hems embroidered with small symbols in {accent}, an amulet on a long chain and a girdle hung with pouches.",
            "A woman who works the arcane dresses so that she is known before she speaks: half of the craft is being believed. The symbols on the hem are her own and are stitched by nobody else. A hedge witch wears the same cut in undyed cloth and says the colour makes no difference."),
        ["paddedCoatAndMailShirt"] = (
            "Thigh-length coat of quilted linen stitched in vertical rows, under a short-sleeved shirt of iron mail, a mail hood pushed back onto the shoulders and a leather sword belt.",
            "A woman who takes pay to fight wears what the men wear, cut down to fit. The mail is the dear part: it is bought link by link out of wages, or taken from someone who no longer needs it. Guard captains say that they ask one thing of a recruit, and it is not her sex."),
        ["beggarsRags"] = (
            "Hooded cloak of patched, faded brown wool frayed at the edges, the hood up, over a stained and torn linen shift, a cord for a belt and a wooden bowl. Below: bare or rag-wrapped feet.",
            "What is left when everything else has been sold: a cloak that was somebody else's twice over. The bowl is the one thing a beggar owns outright, and in most towns it is the law that it may not be taken for a debt. Temple doors are where the cloaks gather."),
        ["playersMotley"] = (
            "Close-fitting bodice and tunic split down the middle into two bright colours, one half {main} and the other {accent}, the edges cut into points, small brass bells at the collar and sleeves, ribbons in the hair, and a small drum or pipe in hand.",
            "The dress of those who are paid to be looked at: tumblers, minstrels, jugglers and fools. The two colours say that the wearer belongs to no lord and to no guild. A fool in motley may say to a king what would hang anyone else, and the bells give warning that it is coming."),
        ["fieldTunicAndHood"] = (
            "Knee-length belted tunic of coarse undyed brown wool with long sleeves over a linen shirt, and a wool hood with a short shoulder cape, pushed back; a straw hat in summer. Below: loose linen drawers, wool hose and thick leather shoes, or bare feet.",
            "The dress of the men who plough, herd and cart on the ploughed lands. A man has one tunic for work and, if the year was good, one for the temple. The hood is his pocket: bread, a whetstone and whatever he found on the road ride in its tail."),
        ["smithsLeatherApron"] = (
            "Long apron of thick dark leather, scorched and scarred, hung from the neck and tied at the waist over a linen shirt with the sleeves rolled above sooty forearms, and a leather band around the brow.",
            "A smith's apron is cut from the back of a whole ox hide and lasts a working life. The burns on it are read like a ledger of what its wearer has made. An apprentice is given his own on the day he is trusted with the fire, and Dwarves hang a dead master's in the forge."),
        ["minersHoodAndSeatLeather"] = (
            "Close-fitting hood of undyed linen with a short cape over the shoulders, grey with rock dust, over a short belted tunic, a pick in hand. Below: a triangular leather apron worn at the back, hung from the belt to sit on.",
            "The seat leather is the miner's mark: he slides down the shaft on it and sits on it at the face. Miners wear it to weddings and to the temple, and are carried to the grave on it. To have it cut from the belt before the whole shift is to be cast out of the mine."),
        ["furredGownAndWoundHood"] = (
            "Long full gown of {main} wool with a high collar and wide sleeves, lined and edged with brown fur, belted at the waist with a purse, and on the head a {accent} hood wound into a padded roll with its tail draped over one shoulder.",
            "The gown of a man who no longer needs to move quickly: a merchant, a guild master, a physician, a councillor. Its length is his standing, and the fur is his banker's reference. The hood is wound afresh each morning, and each town has its own way of doing it."),
        ["doubletAndHose"] = (
            "Short close-fitting padded jacket of {main} wool laced down the front, with puffed upper sleeves, over a white linen shirt that shows at the neck. Below: tight hose tied to the jacket with laces, one leg {main} and the other {accent}.",
            "The dress of the young men of a court or a great household: squires, pages and retainers, who wear the colours of the house on their legs. Old men call it indecent and remember wearing it. The laces are the first gift a girl gives."),
        ["kingsMantleAndCrown"] = (
            "Long mantle of {main} cloth lined with white fur and fastened at the right shoulder with a large gold clasp, over a long tunic embroidered with {accent} thread, and a gold crown set with stones.",
            "The mantle is laid on a king at his crowning and is worn when he judges. It is fastened at the right shoulder so that the sword arm is free, which every king hopes is only a custom. The same mantle has been on every king of the line, and it has been let out and taken in many times."),
        ["monksHabit"] = (
            "Ankle-length tunic of brown wool with a hooded panel over the shoulders, the hood lying back, tied at the waist with a knotted cord.",
            "A man who enters a temple house is given one habit, and it is replaced when it can no longer be patched. He sleeps in it. The scribes and teachers of the temple wear the same as the brother who digs the garden, and that is the point of it."),
        ["mailAndSurcoat"] = (
            "Knee-length shirt of iron mail with long sleeves and a mail hood framing the face, under a sleeveless {main} cloth surcoat with the wearer's device in {accent} on the chest, and a conical iron helmet with a nose bar. Below: the surcoat hangs to mid-calf, split front and back for riding.",
            "A shirt of mail costs as much as a farm, so the man inside it is a lord or a lord's sworn man. The surcoat keeps the sun off the iron and tells friend from enemy when faces are hidden. A son wears his father's mail and has the device sewn anew."),
        ["paddedJackAndKettleHat"] = (
            "Thigh-length coat of many layers of quilted linen stitched in vertical rows, off-white and stained, a wide-brimmed iron helmet shaped like a hat, and a spear in hand.",
            "What a town gives the men who stand on its wall. Twenty layers of linen will stop a blade, and they cost a tenth of what mail costs. The iron hat is hung over the hearth between musters and has boiled more than one supper."),
        ["lamellarCoat"] = (
            "Sleeveless coat of small rectangular plates of iron or lacquered leather laced together with {accent} cord in overlapping horizontal rows, worn over a long wrapped {main} coat, with a pointed iron helmet.",
            "The armour of the horse archers of the steppe and of the guards of the river-plain realms. It is laced at home through the winter, a row at a time, and a cut lace can be mended in the saddle. The colour of the cord tells the company."),
        ["shoulderPinnedCloakAndTunic"] = (
            "Knee-length long-sleeved wool tunic in {main} with woven braid at the neck and cuffs, a heavy {accent} wool cloak edged with fur and pinned at the right shoulder with a round brooch so that the sword arm is free, and twisted silver arm rings. Below: trousers gathered under the knee, cloth strips wound around the shins, a belt with knife and purse.",
            "The dress of a free Northman, at the plough or at the oar. The cloak is his blanket on a ship, and the arm rings are his purse: a piece is cut off when a payment falls due. A chieftain's has silk at the hems and a fur lining, and the brooch was a gift."),
        ["citizensToga"] = (
            "A great semicircular cloth of white wool draped over the left shoulder, around the back, under the right arm and back over the left shoulder, in deep curved folds across the chest, over a white tunic, with a border in {accent} along its edge.",
            "The dress of a citizen on public business in the warm-coast towns. It cannot be put on without help or worked in at all, which is what it says about its wearer. The border is the office, and it is unpicked on the day the office ends."),
        ["oneShoulderWorkTunic"] = (
            "Short tunic of coarse undyed linen belted at the waist with the cloth bloused over the belt, fastened on the left shoulder only, so that the right shoulder and half the chest are bare.",
            "The tunic of a man who works with his arms in a hot town: a potter, a porter, a deckhand. One shoulder is left free for the work. It is washed in the harbour and dried on the wearer."),
        ["linenKiltAndBroadCollar"] = (
            "Bare chest, a broad collar of rows of {accent} beads over the shoulders, armlets, and black lines of kohl around the eyes. Below: a wrap-around kilt of pleated white linen held by a sash, to the knee, with a starched triangular front panel.",
            "The dress of the men of the hot river towns, from the scribe to the governor. The pleats are pressed into the wet linen with a board and last one day. A labourer wears a plain short kilt and nothing else, and the kohl is worn by all, against the glare."),
        ["veilTurbanAndRobe"] = (
            "A long cotton cloth dyed so deep with indigo that it shines blue-black, wound many times around the head as a turban and across the face so that only the eyes show, over a loose long-sleeved robe of white cotton; blue dye stains the skin of the cheeks and hands.",
            "Among the desert clans of the Saltroad folk it is the men who veil, from the day they are counted as men. A man of standing is never seen without it, and he eats behind it. The deeper the dye, the richer the man, and the fold tells the clan."),
        ["waistClothAndShoulderCloth"] = (
            "Bare chest with a loose {accent} cloth laid over one shoulder, a large wound turban and bracelets. Below: a long unsewn {main} cotton cloth wrapped around the waist, pleated at the front, passed between the legs and tucked at the back, falling to the ankle.",
            "A Rainkeeper man wears unsewn cloth, as his gods are said to. The colours tell his community, and the turban is wound in its way. A farmer wears it short and plain, and it is yellow silk for a feast day."),
        ["officialsBadgeRobe"] = (
            "Full-cut round-collared robe of {main} silk with wide sleeves, a large embroidered square on the chest showing a bird for a civil official or a beast for a military one, a stiff hooped belt and a black gauze cap.",
            "An official of the Canal folk wears silk in the colour of his grade, and the creature on the badge gives the grade itself to anyone who can read it. The robe is handed in with the seal when the post ends. A clerk can recite all the grades of birds before he can shave."),
        ["wideSleevedGrandRobe"] = (
            "A very wide flowing robe of {main} cotton damask reaching the ankles, its huge open sleeves folded back onto the shoulders, dense {accent} embroidery around the neck opening and down the chest, over a long shirt and loose trousers, with a wrapped turban.",
            "The robe of the merchants, scholars and caravan masters of the dry-grass trade towns, and of the women who trade beside them. Its width is its wearer's credit. The embroidery is done by men, who are paid by the month, and a great robe keeps one of them for a year."),
        ["crossedRobeAndSash"] = (
            "Long wide-sleeved robe of plain {main} cloth, the left front crossed over the right to make a V at the neck, tied at the waist with a {accent} cloth sash. Below: the robe falls in folds to the feet.",
            "The robe of the Canal folk, men and women, townsfolk and gentry: silk for the rich, hemp or cotton for the rest. It is always crossed left over right. Right over left is how the dead are dressed, and a child who gets it wrong is corrected at once."),
        ["wizardsRobeAndPointedHat"] = (
            "Long loose robe of grey wool with very wide sleeves, a tall pointed hat with a wide brim in the same cloth, a rope belt hung with pouches and a gnarled wooden staff.",
            "The grey robe and the tall hat are older than any school of the arcane, and no two wearers agree on what they mean. The sleeves hold a great deal. Innkeepers give the hat the seat by the fire and count the spoons afterwards."),
        ["guildCutTownDress"] = (
            "Hip-length coat of good wool dyed {main}, cut close and buttoned to the neck, a soft cap of the same cloth, and a small metal guild badge pinned at the breast. Below: a belt with a purse, {accent} hose and leather shoes.",
            "Midlander townsfolk wear dyed cloth cut to the fashion of their guild, and a guild fines a member who is seen in another's cut. The badge is given with the freedom of the craft. Journeymen wear the coat a hand shorter than masters do."),
        ["indigoWrappedCotton"] = (
            "A long cotton cloth dyed deep indigo, wrapped around the body and thrown over the left shoulder with the right shoulder bare, a band of {accent} along its edge. Below: the cloth falls to the calf.",
            "Indigo is first among the Sunlander dyes. The dye pits belong to the women, are handed down from mother to daughter, and are never let dry. New cloth leaves blue on the skin, and that is worn as proof that the cloth is new."),
        ["woodlandGreysAndGreens"] = (
            "Long-sleeved tunic of soft wool in dull green under a hooded cloak mottled in grey and brown, closed at the throat with a plain leaf-shaped clasp, with no bright metal or colour anywhere. Below: a narrow belt, close hose and soft boots in the same dull colours.",
            "Elves dress to be hard to see between trees. The mottled cloaks are dyed with bark and lichen by a method they do not explain, and no two are alike. A human who is given one has been paid a great compliment, and is usually lost in it by evening."),
        ["thickWoolAndToolBelt"] = (
            "Heavy tunic of thick {main} wool under a sleeveless jerkin of stiff brown leather, thick leather bracers, and a broad belt with a hammer or a pick hung from it. Below: wool trousers and heavy boots.",
            "A Dwarf is rarely without a tool on the belt, and the tool tells the craft. Men and women dress alike. The jerkin is made to outlast its wearer, and a good one has been worn by three generations of the same clan."),
        ["brightWaistcoatAndWideHat"] = (
            "Bright {main} waistcoat with brass buttons over a full-sleeved white linen shirt, and a wide-brimmed {accent} felt hat. Below: breeches and good polished leather boots.",
            "Halflings wear their prosperity on their backs: the brighter the waistcoat and the more buttons, the better the year. A new waistcoat is shown off on market day. Boots are the one thing a Halfling will not buy cheap."),
        ["manyPocketedCoat"] = (
            "Knee-length coat of sturdy {main} cloth covered in pockets of every size, with flaps and loops and tools and rolled papers showing from them, and round spectacles of ground glass on the nose.",
            "A Gnome's coat is a workshop that walks. Each pocket is sewn for one tool, and its owner can find any of them in the dark. An apprentice starts with six pockets and adds one for each thing learned, and a master's coat weighs as much as a child."),
        ["hideAndWonArmour"] = (
            "Sleeveless jerkin of thick raw hide laced with thongs, one dented iron shoulder plate strapped on one side and a strip of mail hung at the throat, no two pieces matching, the arms bare and scarred. Below: hide breeches and a wide belt.",
            "An Orc wears what was taken: every piece of iron has a story, and the story is told. Nothing is polished and nothing is matched. Armour that was bought is hidden under the hide."),
        ["singleSealskin"] = (
            "One thin, close, hooded coat of sleek grey sealskin worn hair outward, with no fur ruff and nothing under it, the throat and hands bare. Below: sealskin trousers and boots.",
            "A Hoarkin wears one layer, against wind and wet, and does not need it against the cold. The skins are sewn with a waterproof seam. Other peoples of the ice coasts find the bare throat harder to look at than the blue lips."),
        ["kiltAndOpenVest"] = (
            "Open sleeveless leather vest over a bare chest, the shoulders and arms bare. Below: a knee-length kilt of heavy {main} wool held by a wide belt, and bare legs.",
            "Hearthbloods dress for the heat they carry, in weather that kills others. A coat is for the sick. The kilt is woven in the hall through the winter, and a guest who is cold is lent the host's own."),
        ["plainLongCoatAndCutAwayHat"] = (
            "Long plain coat of undyed grey wool buttoned to the neck, without trim, and a hat whose brim is cut away at both sides, so that it shades the brow and the nape and leaves the view to left and right clear. Below: the coat falls to mid-calf.",
            "A Widegaze dresses so that nothing moves at the edge of sight: no trim, no fringe, no hanging sleeve. The hat is cut to the wearer's own eyes by a hatter who measures with a candle. They buy fine needlework and hang it on the wall."),
        ["paleWrappedLinen"] = (
            "Pale undyed linen wrapped around the body in several turns and over one shoulder, and a line of dark paint drawn around each eye. Below: the linen falls to the ankle.",
            "The linen of the Brinefolk is pale because salt takes every dye within a season. Both sexes line the eyes with dark paint against the glare of the flats. A cloth stiff with salt is not a dirty one: it is how one tells who works the pans."),
        ["quiltedRobeAndClothMask"] = (
            "Thick quilted robe of black cloth closed high at the throat, long sleeves ending in gloves, and a mask of stiffened cloth over the whole face. Below: the robe falls to the feet.",
            "Among their own, Duskreaders cover the warmth of the body, since they read it as others read a face. The robe is a matter of manners, as a lowered voice is. To take off the mask before someone is trust, or a declaration."),
        ["quiltedRobeAndHornVisor"] = (
            "Thick quilted robe of black cloth closed high at the throat, gloves, and across the eyes a visor of smoked horn with one narrow level slit. Below: the robe falls to the feet.",
            "By day a Duskreader wears the visor, for daylight is a blow to those eyes. Each visor is cut from one horn and fitted to its wearer by a maker who works in the dark. Among other peoples it is also a courtesy: it hides the eyes that others cannot meet."),
        ["hipWrapBareBack"] = (
            "Nothing worn above the waist: the chest, the shoulders and the whole back bare to the sun. Below: a plain {main} cloth wrapped at the hips to the knee.",
            "Noonfolk wear nothing that shades the back, where the sun is taken in. To cover another's shoulders is to say that they are ill. The wrap is for the comfort of other peoples."),
        ["shortWrapsAndPouchBelts"] = (
            "Bare shoulders and arms, a short wrap of plain brown cloth around the chest, and belts crossed over the torso hung with small pouches. Below: a second short wrap at the hips, bare legs and feet.",
            "To everyone else the cloth of a Boughwalker looks plain brown or white. To their own eyes it blazes with pattern, and a weaver's name is known three valleys off. Everything worn is tied on twice, since whatever falls is gone."),
        ["loinclothAndDiveBelt"] = (
            "Bare chest and shoulders, a knife in a sheath on a cord at the hip. Below: a plain undyed loincloth and, for diving, a belt of lead weights.",
            "A Shoalfolk owns a loincloth, a knife and a share in a reef, and counts that as wealth. The lead weights are handed down from a diver to the child who takes over the dive. Clothes are what the shore people need."),
        ["hoodLongSleevesAndGloves"] = (
            "Deep {main} hood that shades the whole face, a long-sleeved tunic of the same cloth closed at the throat, and thin gloves, so that no skin but the face shows. Below: a long skirt or hose to the ankle.",
            "The sun burns a Lanternkin quickly, so out of doors nothing shows but the face in the shade of the hood. At home the hood comes off first, and the gloves last. A Lanternkin who meets you bare-handed is a friend."),
        ["wrappedCottonSaffronSash"] = (
            "Plain undyed cotton cloth wrapped around the body and over one shoulder, cut the same for everyone, tied at the waist with a saffron-yellow sash.",
            "Everyone among the Twofold dresses alike, and only the sash differs. Saffron is worn before the Turning, by all who have not yet lived their second life. It is tied on a child at the first birthday, by both parents."),
        ["wrappedCottonIndigoSash"] = (
            "Plain undyed cotton cloth wrapped around the body and over one shoulder, cut the same for everyone, tied at the waist with a deep indigo sash.",
            "The indigo sash is tied on at the end of the year of the Turning, before the whole house, and the saffron one is burned. Its wearers have lived both lives and are the ones asked to judge a quarrel. It is worn until death."),
        ["bareShoulderedHomeWrap"] = (
            "A {main} cloth wrapped under the arms and tied at the chest, leaving the throat, the shoulders and the arms bare, so that the patches of bright skin show.",
            "At home a Bitterblood shows the patches with pride: the brighter they are, the stronger the blood. The wrap is cut low for that. A child's first bright patch is shown to the neighbours, and food is sent round."),
        ["glovesAndWaxedSkin"] = (
            "Long-sleeved {main} robe closed at the throat, thin leather gloves, a dull waxy coating on the skin of the face and neck, and a small cup hung at the belt.",
            "Among other peoples a Bitterblood covers up, so that a handshake harms nobody. The wax is put on fresh each morning, and the cup is their own, since no host should have to wash it. A traveller who does all this is thanked for it less often than is fair."),
        ["looseTailSlitClothes"] = (
            "A loose, light {main} tunic of thin cloth with short sleeves, and loose {accent} trousers cut with a slit at the back that the tail passes through. The head is bare.",
            "Catfolk wear light, loose clothes cut to leave the tail free, and nothing that covers the ears, since both are half of what they say. A tailor who can hang a slit so that it neither gapes nor pinches is paid well and never lacks work."),
        ["hoodAndBoundTail"] = (
            "A long plain {main} coat tied at the waist with a broad {accent} cloth sash, the tail wound around the waist and hidden under the sash, and a deep hood drawn up over the head so that it covers the ears.",
            "A Catfolk who deals with outsiders, in a court or a merchant house, binds the tail under a sash and wears a hood. This is called going wrapped. It hides what the ears and the tail would give away in a bargain, and the old say it hides the person as well."),
    };
}
