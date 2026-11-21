namespace Gameworld.Domain;

// What each default hairstyle looks like and its lore. The descriptions are those of
// docs/research/portrait-components.md sections 2, 3 and 8.1, without the sources. The lore is
// written in this file, from the column "Worn by" of the note and from docs/races-basis.md.
// A kind that has no text here gets DefaultTexts.Missing.

public static partial class DefaultTexts
{
    internal static readonly Dictionary<string, (string Description, string Lore)> Hairstyles = new()
    {
        ["looseWithCirclet"] = (
            "Waist-length hair worn loose and uncovered, parted in the centre and combed smooth, held off the face by a narrow metal circlet or a wreath of fresh flowers.",
            "Loose hair says a girl is not yet married, and she wears it so for the last time on her wedding day. The circlet is whatever the house can afford: silver for a lord's daughter, a wreath of field flowers for a farmer's. Midlander wives say the wind in it is the thing they miss."),
        ["singleBackBraid"] = (
            "Long hair drawn straight back and plaited into one thick braid that hangs down the back to the waist, tied at the end with a plain cord.",
            "The braid of every working day: it keeps the hair out of the fire, the dough and the loom. A mother braids her daughter's until the girl can do it behind her own back, and that morning is counted the end of childhood. Among the Swiftborn the braid is also a calendar, read ring by ring."),
        ["twinLongPlaits"] = (
            "Hair parted in the centre and worn in two very long plaits that hang in front of the shoulders to the hip, each bound in crossed ribbon.",
            "The longer the plaits, the less their owner has ever had to carry, and a rich townsman's daughter is not above a little false hair to reach the hip. The ribbons are a suitor's gift. A girl who binds her plaits in a new colour has given the town something to talk about."),
        ["crownBraid"] = (
            "Two braids wound around the head like a band and pinned flat above the forehead, leaving the neck and ears bare.",
            "The farmers' crown: a wife pins her braids up when she has work to do, which is every day. It leaves the neck cool at the oven and gives the cattle nothing to chew. The Northmen say a woman with her braids up has no time for your story."),
        ["knottedTail"] = (
            "Long hair pulled back and tied in a single knot at the back of the head, the loose tail falling to the shoulder blades, with a strip of coloured woven tape worked into the knot.",
            "The knot of the women of the cold coasts, tied the same way since before the Great Winter. The tape in it is woven at home on a small loom, and its pattern tells the farm. A woman reties the knot before she speaks at the assembly."),
        ["veilAndWimple"] = (
            "Hair fully hidden: a white linen cloth wrapped under the chin and around the throat, and a second white veil pinned over the top of the head and falling to the shoulders, so that only the face shows.",
            "A Midlander wife covers her hair on the morning after the wedding and does not show it out of doors again. The linen is her pride: bleached, starched and pinned afresh each day. A widow keeps the veil, and a wife who walks bareheaded to market is making a point."),
        ["nettedSideCoils"] = (
            "Hair plaited and coiled into a cylinder at each temple, each coil held in a net of gold wire set with small jewels, the two joined by a narrow band across the brow; a sheer veil hangs behind.",
            "A fashion of the courts that takes a maid an hour each morning. The gold nets are listed in a lady's marriage portion, pearl by pearl. Preachers call the coils horns, and the ladies answer by building them wider."),
        ["napeKnot"] = (
            "Hair waved back from the forehead and gathered into a soft knot low at the nape, held by a cloth band wound twice around the head and by long pins of bone, ivory or gold.",
            "The plain knot of the warm coasts, worn by a fishwife and a magistrate's wife alike. Only the pins differ: wood, bone, ivory or gold. In the port towns a woman lends her pins to nobody, since a pin is the first thing to hand in a quarrel."),
        ["tieredCurls"] = (
            "A tall crest of tight curls built up in rows above the forehead with the help of false hair, the back hair plaited and coiled into a flat bun behind.",
            "It cannot be dressed by its wearer, and that is its meaning: a house that shows these curls keeps a hairdresser. The crest rose higher with each generation of the old river cities until it needed a wire frame. Half of it is other women's hair, bought by weight."),
        ["bluntBraidedWig"] = (
            "A shoulder-length black wig of many narrow braids and tight curls, cut straight at the ends and across the brow, worn over a shaved head, with a narrow band around the crown.",
            "In the hot river lands the head is shaved against heat and lice, and the wig is put on for company. A good one is a year's work of a wigmaker and is left by will. To come to a feast without it is to say that the host is not worth dressing for."),
        ["beadedCornrows"] = (
            "Hair braided close to the scalp in narrow raised rows that run back from the forehead in straight or curved lines, the ends hanging to the collarbone and finished with beads or small shells.",
            "The rows are braided by a kinswoman over a long afternoon, and the pattern is a message: it can say a village, an age set, a betrothal or a mourning. Sunlander children learn to read heads before they learn to count cattle. The Shoalfolk finish theirs with shell from the family's reef."),
        ["ochrePlaits"] = (
            "Long hair in thick plaits coated in a paste of red ochre and butterfat, matte brick-red from root to tip and lengthened with goat hair; the skin of the neck and shoulders carries the same red sheen.",
            "A herder girl is given her first ochre when she comes of age, and renews it every morning with butterfat. The red is the colour of earth and of cattle, and it keeps off sun and flies. A woman without it is in mourning or far from her own herds."),
        ["shavedHead"] = (
            "Head shaved clean or to dark stubble, the shape of the skull plain to see, set off by large earrings or a narrow brow band.",
            "A shaved head means different things in different country: a vow among the temple women, mourning among the herders, and plain good sense in the hot river towns, where a wig is kept for feast days. The razor is the same. Only the reason is asked about."),
        ["tallHeadwrap"] = (
            "Hair hidden under a length of stiff patterned cloth wound and tied into a tall, wide, sculpted wrap that rises well above the head, the ears showing.",
            "A Sunlander wife ties her first wrap on the day she moves to her husband's compound, and the market reads her standing from its height and its cloth. Each town has its own fold. A wrap tied carelessly tells the whole market that something is wrong at home."),
        ["pinnedHighKnot"] = (
            "Hair swept up into a tall knot high on the crown and fixed with long pins of gold, silver or jade, one pin hung with small pendants that sway.",
            "A Canal folk girl puts her hair up at the pinning rite in her fifteenth year, and from then on she is addressed as a woman. The pins are given by her mother's family. The pin with the swaying pendants was once for noblewomen only, and the wives of officials still count each other's."),
        ["trailingCourtHair"] = (
            "Straight black hair never cut, worn loose down the back to the floor, parted in the centre, with shorter locks cut at cheek level to frame the face, and tied once low behind the shoulders.",
            "Hair that has never been cut and never been tied up for work: only a lady of a great house can keep it, and it takes two maids to wash. It is measured against her height each new year. The court poets of the Canal folk praise a woman's hair before they mention her face."),
        ["floweredBraid"] = (
            "Oiled black hair in one long braid down the back, with a string of small white jasmine flowers coiled along its length.",
            "The jasmine is strung fresh each morning by the flower sellers who sit at every Rainkeeper temple gate. A husband who brings a string home has said all he needs to. By evening the flowers have browned, and the string is left at a shrine and not thrown away."),
        ["wingedHorns"] = (
            "Hair parted in the centre and pinned up in two broad flat arches, wider than the head, that reach out to the sides and down to the shoulders like horns, held by rows of silver clips, under a cap of silver filigree set with red coral; the ends hang in front in silver sleeves.",
            "A wife of the Grass riders wears her dowry on her head: the silver clips and the coral cap were weighed out when the marriage was agreed. The wings are said to be those of the bird that mothered the first clan. A woman takes them down only in mourning."),
        ["croppedShort"] = (
            "Hair cut short around the head at ear level with a straight fringe, uneven where it was cut with a knife.",
            "Hair cut with a knife, by its owner. On the ploughed lands a woman crops her hair when she takes up a spear, goes to sea or means not to be known. Among the Hearthbloods and the Shoalfolk it means nothing at all: long hair is only a thing that stays wet."),
        ["halfUpSideBraids"] = (
            "Long hair loose down the back, with a thin braid from each temple drawn back and joined behind the head, the ears showing.",
            "The two small braids keep the hair from the face and do nothing else, so the style belongs to those with no work that would dirty the rest: elven ladies and the daughters of great houses. Midlander girls copy it from tapestries and are told to put their hair up properly."),
        ["shortCrop"] = (
            "Hair cut short all over and combed forward onto the brow, the ears and neck clear, the face clean-shaven or with stubble.",
            "The soldier's cut of the warm coasts: nothing for an enemy to hold and nothing to keep clean. The soldiers of the Realm wore it, and the towns on the old roads still do. A man who lets it grow is in mourning, or wishes to be taken for a philosopher."),
        ["bowlCrop"] = (
            "A straight fringe across the forehead and the hair cut to one length all round at the top of the ears, as if around a bowl, with the hair below that line cut very short.",
            "Cut at home, around a bowl, by whoever has the shears: no barber is paid for it. Every ploughboy has one. When the young men of a court took it up one year as a fashion, the peasants thought it the best joke of the reign."),
        ["chinLengthBob"] = (
            "Hair cut level with the jaw, parted in the centre or with a short fringe, the ends curled under.",
            "The cut of a man who sits to work: a clerk, a page, a townsman with a counting board. The ends are curled under with a hot iron by those who can afford a barber. Soldiers call it a wife's haircut and wear it themselves as soon as they are paid off."),
        ["tonsure"] = (
            "The crown shaved bare, leaving a ring of short hair around the head.",
            "The crown is shaved when a man enters the temple, by the hands of the one who receives him, and is kept shaved for life. It marks a man who is under the temple's law and not the lord's, which has saved more than one clerk from the gallows. A tonsure allowed to grow over is a vow given up."),
        ["longAndLoose"] = (
            "Hair grown past the shoulders and combed back, loose or tied at the nape with a thong, worn with a full beard.",
            "The Northmen hold that a man's strength is in his hair, and comb it every day to show that they deserve it. A thrall's hair is cut. Farmers and hunters of the forest edge wear it long for a plainer reason: there is nobody for a week's walk who owns shears."),
        ["topknot"] = (
            "Long uncut hair drawn up and tied in a tight knot on the crown, fixed with a single pin, the face and neck clear.",
            "A Canal folk man never cuts his hair, which is his parents' gift, and binds it up at the capping rite in his twentieth year. An official wears his cap of rank over it. To cut off a man's topknot is a punishment written in the law, one step below exile."),
        ["shavedCrownWithSideBraids"] = (
            "The crown shaved, a short tuft left on the forehead, and the remaining hair at the sides plaited into a braid behind each ear.",
            "The cut of the Grass riders, made with the same knife that gelds the colts. Each clan leaves the forelock a little differently, and scouts read it at a distance. A boy's crown is first shaved on the day he rides alone."),
        ["bareScalp"] = (
            "Head and face shaved clean, the scalp smooth.",
            "The priests of the hot lands shave every third day, so that nothing unclean is carried before a god, and labourers shave because the sun asks it. A bare scalp oiled until it shines is a point of pride. A wig waits at home for the feast days."),
        ["warriorBraids"] = (
            "Shoulder-length hair woven into many thin braids, coloured red with ochre and drawn back from the face.",
            "A young Sunlander herder grows his hair when he joins the warriors of his age set, and reddens it with ochre. His mother shaves it off on the day the set become elders, and he is expected to weep. Until then no blade but hers may touch it."),
        ["oiledRinglets"] = (
            "Shoulder-length black hair, oiled and set in rows of tight ringlets held by a band around the brow, with a long squared beard curled the same way.",
            "The old river cities set their beards in rows of curls before the Ashen Years, and their statues show it. It takes a barber, hot irons and scented oil, and a man of standing has it done every week. An untended beard there means a man is ruined or grieving."),
        ["combedLongWithBraidedBeard"] = (
            "Hair grown past the shoulders and combed smooth straight back from the brow, with a full beard plaited into two braids.",
            "A Northman combs his hair every morning and braids his beard before he goes among other men. The comb hangs at his belt beside the knife and is buried with him. To pull a man's beard is an insult the law prices above a broken arm."),
        ["patternShavedScalp"] = (
            "Tightly coiled black hair cropped close to the scalp, with lines and curved bands shaved into it down to the skin.",
            "The lines are cut with a razor by the barber of the age set, and each set has its own. A stranger's head tells a Sunlander his people, his village and how long ago he was a boy. The pattern is renewed at every new moon and changed only when the wearer's standing changes."),
        ["clayAndBeadCrown"] = (
            "Tightly coiled hair worked with clay into a smooth, matte, raised mass on the crown of the head, set with rows of coloured beads.",
            "The clay is worked in over many days and worn for a season. It belongs to elders and title-holders, who sleep on a wooden neck rest to keep it whole. The beads are added one at a time, each for a thing done, and the oldest heads are heavy with them."),
        ["oiledLongHair"] = (
            "Long black hair, straight or wavy, glossy with oil, combed back from the brow and falling loose past the shoulders.",
            "Rainkeepers oil the hair every morning, and a child's is oiled by its grandmother with a blessing. Hair is cut only as an offering: a pilgrim leaves it at the temple. The Twofold wear the same long hair before and after the Turning, which gives an outsider one clue fewer."),
        ["longAndSweptBack"] = (
            "Long straight hair to the middle of the back, combed back from a smooth brow and tucked behind the ears so that the whole ear shows, the face beardless.",
            "Elves wear the hair so that the ear shows: to cover it is to hide what one is. The hair is cut once, at the end of childhood, and the lock is buried under a sapling. Nothing is worn in it that would catch the light."),
        ["ringedBeardBraids"] = (
            "A full beard plaited into three thick braids, each bound with metal rings, and the hair of the head drawn back into one heavy plait.",
            "A Dwarf's beard is a record that any other Dwarf can read: the rings give clan, craft and standing. The first ring is hammered on at the end of the apprenticeship, by the master. A beard shorn by judgement of the hold grows back, and the shame does not."),
        ["curlyMop"] = (
            "Thick, springy curls cut short enough to clear the collar, falling over the brow and around the ears.",
            "Halfling hair does as it likes, and Halflings let it. It is cut at the kitchen door a few times a year, and the cuttings go on the garden. A Halfling with tidy hair is suspected of ambition."),
        ["whiteTufts"] = (
            "Fine hair standing out in tufts above the ears and at the back of the head, thin on the crown.",
            "Gnome hair whitens before thirty and thins on the crown soon after, and the tufts that are left stand out whatever is done with them. Gnomes take the white as the mark of a finished apprenticeship. They say the thinking wears the hair away from the inside."),
        ["pointedCap"] = (
            "Hair hidden under a soft pointed cap of felt pulled down to the tops of the ears, a few tufts showing at the temples.",
            "The cap keeps the hair out of the lens and the lamp flame, and a Gnome at the bench is never without it. Its colour is the workshop's. A master gives a new cap with the first paid commission, and the old one is kept in a drawer for life."),
        ["coarseCrest"] = (
            "The sides of the head shaved bare and a strip of coarse hair, a hand wide, standing up stiffly from the brow to the nape.",
            "An Orc shaves the sides before a raid, so that a blow to the head shows and can be boasted of. The crest is stiffened with fat and ash. A chief's stands highest, and a warrior who has been beaten cuts his own down before anyone can say it."),
        ["roughKnot"] = (
            "Coarse hair dragged back and tied in a rough knot at the back of the crown with a strip of hide, loose strands escaping.",
            "The knot of an Orc with work to do: hair dragged back and tied with whatever strip of hide is to hand. It is worn in the camp, on the herd trail and at the forge. The crest is for war, and the knot is for all the days between."),
        ["denseCloseCrop"] = (
            "Fine, very dense hair cut to a finger's length all over the head, lying as close and even as a pelt.",
            "Hoarkin hair grows as thick as a pelt and is cut twice a year, at the turn of the light. The cuttings are saved and felted into the lining of a child's first boots. They never let it grow long: wet hair freezes, and a Hoarkin does not notice until it breaks."),
        ["hardTieBack"] = (
            "Hair scraped straight back from the brow and temples and bound tight at the nape, with no fringe and no loose strand anywhere near the eyes.",
            "Nothing may cross a Widegaze's eyes, so the hair is bound back before a child can walk and kept so for life. A loose strand is seen to as others would see to grit in the eye. To cut the tie of a sentry is counted the same as blinding him for the hour."),
        ["hairless"] = (
            "No hair at all: a smooth bare scalp without stubble, and no eyebrows.",
            "Noonfolk grow no hair anywhere, and find the hair of other peoples a little animal. The bare scalp is oiled against cracking and takes the sun like the back and the shoulders. A hat is for the sick."),
        ["flatSkullBraids"] = (
            "Black hair braided in tight narrow rows flat against the skull, the ends tucked under and sewn down, with nothing hanging and no ornament.",
            "The braids are sewn down so that no twig can take hold: a Boughwalker caught by the hair forty feet up does not get a second lesson. Kin rebraid each other every nine days. Ornament in the hair is for the dead, who no longer climb."),
        ["oiledSlickBack"] = (
            "Black hair combed straight back from the brow and held flat to the skull with oil, ending at the nape.",
            "The oil is pressed from a bitter seed that only the Bitterbloods handle with bare hands, and it keeps lice, flies and strangers away. The hair is slicked each morning with two fingers. They say a loose lock is the first sign of a careless brewer."),
    };
}
