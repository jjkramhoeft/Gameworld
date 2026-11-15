# Portrait Components: Hairstyles, Outfits, Hair Colours and Skin Colours for Generated Persons

*Research date: 2026-11-15. Question: which fixed lists of **hairstyles**, **outfits**, **hair colours** and **skin colours** can a person be described from, so that every portrait of that person is prompted with the same words? Scope: a pre-industrial fantasy world, roughly antiquity to the early Renaissance, across several cultural models, plus fantasy convention where history has no model. §1 states the method and the findings. §2 and §3 are the hairstyles (20 for women, 10 for men). §4 and §5 are the outfits (30 for women, 20 for men). §6 and §7 are the hair colours (24) and the skin colours (34). §8 is the lists added for the races of `races-basis.md`: hairstyles (17), outfits (25), eye colours (25) and race markers (25). §9 is what the image-model vendors' own documentation says about keeping a character consistent. §10 is a suggested fit to the races and climates of this repo. §11 lists where the existing docs constrained the lists and what could not be sourced. §12 lists the sources. §13 is the open questions.*

**Extended 2026-11-15.** The lists were extended to fit the 32 races of `races-basis.md`: 22 skin colours (§7.2), nine hair colours (HC16 to HC24), 17 hairstyles (§8.1) and 25 outfits (§8.2). Two kinds of component were added after that: 25 eye colours (§8.3) and 25 race markers (§8.4). All of these rest on the Appearance paragraphs of that doc, not on outside sources, and §10.2 now maps every one of its races.

**Summary.** The note gives six lists of components drawn from history, and extensions of them for the races of `races-basis.md`. Every hairstyle and outfit has a short stable name, a description written to be pasted into an image prompt, and a line on who wears it: rank, occupation, age or marital status, climate and occasion. The entries are spread over rank (beggar to ruler), occupation, climate and cultural model (north and west European, Mediterranean, Near Eastern, steppe, South Asian, East Asian, sub-Saharan African, Arctic and Andean), so that many different races can draw on them. Seventy-one of the 80 hairstyle and outfit entries of §2 to §5 have a historical model with a cited source. Nine are fantasy convention or composed types and are marked as such. The 92 entries of §8 have no historical model: each quotes the race it was written for. The existing docs describe looks only in the Appearance paragraphs of `races-basis.md` and the Appearance column of `human-races.md`; §10 maps the lists onto those and says where they give too little to go on.

**How to read the citations**

- Each entry cites the source that backs the garment or hairstyle, by key in square brackets. The keys are listed in §12. Only pages that were opened in this pass are cited.
- **[partially verified]** means the source exists and is cited, but the claim was confirmed only from a search-index excerpt, not by reading the page itself.
- **[unverified]** means general knowledge that was not checked against any source in this pass. It is attached to the single detail it concerns, not to the whole entry.
- **Fantasy convention** means the entry has no historical model and none is claimed. **Composed type** means a plausible working outfit put together from ordinary garments, not a documented costume.
- The sources were read with a fetch tool that returns a summary of the first 100,000 characters of a page. Museum pages: the National Museum of Denmark, the Asia and Pacific Museum in Warsaw, the Weltmuseum Wien and the V&A. Encyclopaedia: eight pages of Britannica. Vendor documentation: OpenAI, Black Forest Labs, Google. Everything else is a Wikipedia article, used for the definition of a named garment or hairstyle. The Met returned 429 on every attempt (§11.3).
- Source wording is paraphrased, not quoted. Every **Description** is written fresh for this note. The "Model" column names the real culture and period behind an entry; the description itself names no real nation, because the world is invented.
- Everything in §10, §11.1 and every **Design note** is my own judgement, not a sourced claim.

**Hooks in this repo** (working tree at commit `3211278`)

| Hook | Where | What it means for these lists |
|---|---|---|
| Person | `readme.md`, "Person" and "Entity Generation States" | A Stub holds name, age, sex, role, family links, faction membership and home. Realised adds the textual description, the history and the portrait, which are authored and must not contradict the Stub. Text and portrait are tracked separately |
| Race | `readme.md`, "Race" | A race is an id, a name, a toggle, a frequency per biome and an optional height modifier. It carries no looks. "How a faction, a family or a person gets its race is not decided yet" |
| Default list | `map-generation.md` §19 and Appendix E | 23 races; Human, Elf, Dwarf, Halfling, Orc and Goblin are on. `human` is one race |
| Human races | `races-basis.md`, "Human races" | Seven human races, each built on a real historical people. The Appearance paragraph of every entry is "what a portrait must show": build, skin, hair, eyes and dress |
| Human peoples | `human-races.md` §4, group P | Sixteen human peoples with a one-line Appearance each. §6.2 there names three places where the looks of a race could live and leaves the choice to the user |
| Variants | `races.md` §5.1, §5.3 | Size and a biome variant were proposed as inputs to the portrait prompt |
| Prompts | `races.md` §5.6 item 9; `human-races.md` §6.4 items 14 and 16 | Prompts must not name proprietary races; portraits of the human peoples should be reviewed for stereotype |
| Randomness | `readme.md`, "Randomness" | Every random choice is seeded from a hash of (foundation seed, entity identity, purpose). A pick from these lists would be such a choice |
| Fibres and dyes | `readme.md`, "Resource" | Fibre variants (flax, hemp, cotton, wool, silk) and dye variants (indigo, woad, madder, kermes, murex) already exist. The descriptions use those materials. Clothing is a resource group that is only produced |

---

## 1. Overview and method

### 1.1 Scope

A **component** here is one fixed piece of a person's description, picked from a list and then repeated word for word in every prompt for that person. This note supplies six kinds: hairstyle, outfit, hair colour, skin colour, eye colour and race marker. The first four are drawn from history and extended for the races of `races-basis.md`; eye colours and race markers exist only as written from that doc (§8.3, §8.4). The note does not supply faces, builds, facial hair or the art style of the picture (§13). A skin or eye entry of a race carries that race's marker where the marker is a colouring of the skin or the eye, and two hairstyles of §8.1 carry a beard because the race's paragraph makes the beard the point.

The lists are for adults. They are split by sex because the request asked for that; §13 item 5 asks whether the split holds for every race.

Excluded on purpose: anything industrial or modern (zips, machine-printed cloth, knitwear cut like a modern jumper, tailored suits, clipper cuts, dyed neon hair), and anything later than the early Renaissance in its model. Five entries rest on evidence that is later than their look is likely to be; they are flagged in §11.2.

### 1.2 Method

1. Read what the repo already says about looks (`readme.md`, `map-generation.md` §19 and Appendix E, `races-basis.md`, `human-races.md`, `races.md`).
2. Draw up the cells to cover: rank, occupation, climate, age or marital status, cultural model.
3. Pick named historical garments and hairstyles that fill the cells and that a museum, an encyclopaedia or a sourced article describes. Open the source and check the cut, the materials and who wore it.
4. Write each description fresh: what is seen in a head-and-shoulders to waist-up portrait first, the rest second.
5. Fill the cells that history does not fill (wizard, sorceress, ranger, woman-at-arms) with fantasy convention, marked as such.
6. Read the image-model vendors' own prompt guides for what keeps a character consistent (§9).

### 1.3 Findings

1. **Head covering and hairstyle are one decision in most of the sources.** A married woman of the European Middle Ages covered her hair [WIMPLE] [EUR-1100]; a steppe wife's hair was built into her headdress [KHALKHA]; an adult man of the Chinese model wore his topknot under a cap [GUAN]. Five of the 20 women's hairstyles are in fact coverings or include one. Whether headwear is its own component is the first open question (§13 item 4).
2. **Hair and dress mark age and marriage more often than rank.** Girls braid, wives cover or pin up [EUR-1300] [HAIRPIN]; a belt button says married or unmarried [GAKTI]; the colour of a veil says novice or professed [HABIT]; a warrior's long braids are shaved off when he becomes an elder [MAASAI]. A person's Stub has age and family links, so these can be derived.
3. **Rank shows in material and colour, not in cut.** The same cut is coarse undyed wool on a peasant and dyed, fur-lined cloth on a lord [ENG-MED]; the same wrap is cotton on the poor and silk with gold thread on the rich [SARI]; pins are wood for the poor and gold or ivory for the rich [ROMAN-HAIR]. One outfit entry can therefore serve several ranks if the material and colour are chosen separately (§13 item 11).
4. **Many garments are worn by both sexes with small differences**: the steppe coat [DEEL], the Arctic parka [INUIT], the herder's tunic [GAKTI], the West African wrapper and grand robe [WRAPPER] [BOUBOU], the kaftan [KAFTAN]. They are listed once and named in the note under §5.
5. **The vendors agree on one method**: write the character's description once, in concrete visual terms, and repeat it in full in every prompt; put the subject first; where the model takes reference images, reuse the first good portrait [OAI-PROMPT] [BFL-FLUX2] [GOOGLE-IMG]. That is what fixed components give (§9).
6. **The repo's looks live in two proposals that do not agree.** `races-basis.md` builds each human race on a real people and dresses it as that people. `human-races.md` says of its human peoples that none copies the real peoples who share its way of life. The default list has one `human` with no looks at all (§11.1).

---

## 2. Women's hairstyles (20)

The label (WH1 and so on) is for cross-reference inside this note. The **Name** is the stable part.

| # | Name | Description | Worn by | Model and source |
|---|---|---|---|---|
| WH1 | Loose with Circlet | Waist-length hair worn loose and uncovered, parted in the centre and combed smooth, held off the face by a narrow metal circlet or a wreath of fresh flowers. | Girls and young unmarried women of any rank in temperate lands; brides and festival days. | Europe, 14th century: circlet or coronet of flowers, uncovered hair [EUR-1300]; Roman girls wore long hair down the back [ROMAN-HAIR]. Loose hair as the sign of a maiden is a convention of later art **[unverified]** |
| WH2 | Single Back Braid | Long hair drawn straight back and plaited into one thick braid that hangs down the back to the waist, tied at the end with a plain cord. | Working women and girls everywhere: farm, workshop, road, camp. The everyday default. | Unmarried girls braided the hair to keep the dirt out [EUR-1300]; a long braid at the back in India [BRAID] |
| WH3 | Twin Long Plaits | Hair parted in the centre and worn in two very long plaits that hang in front of the shoulders to the hip, each bound in crossed ribbon. | Young noblewomen and rich townswomen of temperate lands; the length is helped with false hair. | Europe, 12th century: centre parting, long braids, false hair [EUR-1100] [VAM]. The ribbon binding is **[unverified]** |
| WH4 | Crown Braid | Two braids wound around the head like a band and pinned flat above the forehead, leaving the neck and ears bare. | Farm wives, artisans and servants of temperate and cold lands; married women at work. | Europe from the 11th century, the "farmer crown" [BRAID]; Italian women wound ribbon-twisted hair round the head [EUR-1300] |
| WH5 | Knotted Tail | Long hair pulled back and tied in a single knot at the back of the head, the loose tail falling to the shoulder blades, with a strip of coloured woven tape worked into the knot. | Free farmers' wives and women of the cold coasts; all ages. | Viking Age: hair long, well kept and styled, shown on small silver and bronze figures [NATMUS-A]; "probably" a knot at the back with coloured tape [SCINORD] |
| WH6 | Veil and Wimple | Hair fully hidden: a white linen cloth wrapped under the chin and around the throat, and a second white veil pinned over the top of the head and falling to the shoulders, so that only the face shows. | Married women and widows of middling and high rank in temperate lands, older women, nuns; lower-class women wear a plain cloth version. | Europe, 12th to 14th century [WIMPLE] [EUR-1300] [BRIT-MED] [VAM] |
| WH7 | Netted Side Coils | Hair plaited and coiled into a cylinder at each temple, each coil held in a net of gold wire set with small jewels, the two joined by a narrow band across the brow; a sheer veil hangs behind. | Noblewomen, ladies of a court, rich merchants' wives. | Europe, 13th and 14th century: net headdress, metal cauls at the temples [EUR-1300] [VAM] [CAUL] [BRIT-MED] |
| WH8 | Nape Knot | Hair waved back from the forehead and gathered into a soft knot low at the nape, held by a cloth band wound twice around the head and by long pins of bone, ivory or gold. | Townswomen of the warm coasts, any rank; the pin material shows the wealth. | Greece: chignon at the nape, fillets, pins of gold or ivory [BRIT-GR] [CHIGNON]; Rome: simple bun, wooden pins for the poor [ROMAN-HAIR] |
| WH9 | Tiered Curls | A tall crest of tight curls built up in rows above the forehead with the help of false hair, the back hair plaited and coiled into a flat bun behind. | High-rank matrons of rich cities; it needs a hairdresser, so it marks a household with servants. | Rome, Flavian and Trajanic period [ROMAN-HAIR] |
| WH10 | Blunt Braided Wig | A shoulder-length black wig of many narrow braids and tight curls, cut straight at the ends and across the brow, worn over a shaved head, with a narrow band around the crown. | Wealthy women, and men, of hot dry river lands; feasts and ceremonies. | Ancient Egypt: wigs of human hair and palm fibre in tight curls and narrow braids, worn by the wealthy over shaved heads [EGYPT] [BRIT-HAIR]. The blunt shoulder-length cut is from Egyptian art in general **[unverified]** |
| WH11 | Beaded Cornrows | Hair braided close to the scalp in narrow raised rows that run back from the forehead in straight or curved lines, the ends hanging to the collarbone and finished with beads or small shells. | Women and girls, and men, of hot lands; the pattern can show kin, age and standing. | Africa: Saharan rock paintings from about 3000 BCE, later West African and Ethiopian evidence; beads and shells [CORNROWS] |
| WH12 | Ochre Plaits | Long hair in thick plaits coated in a paste of red ochre and butterfat, matte brick-red from root to tip and lengthened with goat hair; the skin of the neck and shoulders carries the same red sheen. | Herder women of hot dry country, from puberty on. | Himba of Namibia: otjize [OTJIZE] |
| WH13 | Shaved Head | Head shaved clean or to dark stubble, the shape of the skull plain to see, set off by large earrings or a narrow brow band. | Nuns and ascetics; women of hot dry lands who wear a wig on feast days; women of some herding peoples; mourners. | Egypt: men and women often shaved the head [EGYPT]; Buddhist nuns [TONSURE]; short hair and ritual shaving among the Maasai [MAASAI] |
| WH14 | Tall Headwrap | Hair hidden under a length of stiff patterned cloth wound and tied into a tall, wide, sculpted wrap that rises well above the head, the ears showing. | Married women, market traders and senior women of hot lands; larger and finer for weddings and feasts. | West Africa: head tie, the large gele [HEADTIE]. The page gives no date for the style (§11.2) |
| WH15 | Pinned High Knot | Hair swept up into a tall knot high on the crown and fixed with long pins of gold, silver or jade, one pin hung with small pendants that sway. | Adult women of river-plain realms after the hair-pinning rite at fifteen; the pendant pin was first for noblewomen only. | China: buns held by hairpins after the *ji li* rite [HAIRPIN]; the *buyao* pendant pin, Han and Tang [BUYAO] |
| WH16 | Trailing Court Hair | Straight black hair never cut, worn loose down the back to the floor, parted in the centre, with shorter locks cut at cheek level to frame the face, and tied once low behind the shoulders. | Court ladies and noblewomen who do no manual work. | Japan, Heian court: *suberakashi* [JUNI] [BRIT-JKS] |
| WH17 | Flowered Braid | Oiled black hair in one long braid down the back, with a string of small white jasmine flowers coiled along its length. | Women of the monsoon lands, any rank; daily for the well-off, feasts and weddings for all. | South Asia: gajra on a braid or bun [GAJRA]; the long braid [BRAID] |
| WH18 | Winged Horns | Hair parted in the centre and pinned up in two broad flat arches, wider than the head, that reach out to the sides and down to the shoulders like horns, held by rows of silver clips, under a cap of silver filigree set with red coral; the ends hang in front in silver sleeves. | Married women of wealthy herding families on the steppe; the silver is the dowry. | Khalkha Mongols, evidence from the 19th century [KHALKHA] [WMW] (§11.2) |
| WH19 | Cropped Short | Hair cut short around the head at ear level with a straight fringe, uneven where it was cut with a knife. | Fantasy convention for women soldiers, sailors and wanderers. In history a sign of illness, penance or disguise. | One historical case: a woman who cut her hair round like a young man's in 1429 [BOWL]. Otherwise fantasy convention |
| WH20 | Half-up Side Braids | Long hair loose down the back, with a thin braid from each temple drawn back and joined behind the head, the ears showing. | Fantasy convention for elves, rangers and noble maidens; it marks someone who does not work with her hands. | Fantasy convention. No historical model |

---

## 3. Men's hairstyles (10)

| # | Name | Description | Worn by | Model and source |
|---|---|---|---|---|
| MH1 | Short Crop | Hair cut short all over and combed forward onto the brow, the ears and neck clear, the face clean-shaven or with stubble. | Soldiers, townsmen and officials of the warm coasts; any man who wants no trouble with his hair. | Rome from about 300 BCE, close-trimmed hair and a clean shave [ROMAN-HAIR]; later Greek men cut the hair short [BRIT-GR] |
| MH2 | Bowl Crop | A straight fringe across the forehead and the hair cut to one length all round at the top of the ears, as if around a bowl, with the hair below that line cut very short. | Common men everywhere, since no barber is needed; soldiers and young men of fashion in temperate lands. | Europe, late Middle Ages, most of all the first half of the 15th century [BOWL] |
| MH3 | Chin-length Bob | Hair cut level with the jaw, parted in the centre or with a short fringe, the ends curled under. | Gentry, townsmen, pages and clerks of temperate lands. | Europe, 13th century: men's hair long or short [BRIT-MED]. The curled-under bob is from manuscript art in general **[unverified]** |
| MH4 | Tonsure | The crown shaved bare, leaving a ring of short hair around the head. | Monks, priests, temple scribes and copyists. | Latin Church, the Roman tonsure [TONSURE] |
| MH5 | Long and Loose | Hair grown past the shoulders and combed back, loose or tied at the nape with a thong, worn with a full beard. | Farmers, raiders and hunters of the cold lands, hermits, men of peoples who hold that hair is strength. | Early Romans, long hair and beards [ROMAN-HAIR]; Turkic men, loose hair [QUEUE]; Viking Age, combs among the commonest finds [NATMUS-A]. See §11.1 item 4 |
| MH6 | Topknot | Long uncut hair drawn up and tied in a tight knot on the crown, fixed with a single pin, the face and neck clear. | Adult men of river-plain realms, from the capping rite at twenty; officials add a cap over it. | China: hairpins secured the bun for men and women, capping at twenty [HAIRPIN] [GUAN]; Korea: the married man's topknot [SANGTU] |
| MH7 | Shaved Crown with Side Braids | The crown shaved, a short tuft left on the forehead, and the remaining hair at the sides plaited into a braid behind each ear. | Steppe riders: herders, horse archers, khans. | Mongols: part-shaved forehead with strands left, hair plaited behind the ears; Khitan: shaved crown, hair at the temples [QUEUE] |
| MH8 | Bare Scalp | Head and face shaved clean, the scalp smooth. | Priests and monks of hot lands, ascetics, labourers and slaves; men who wear a wig on feast days. | Egypt [EGYPT] [BRIT-HAIR]; Buddhist monks [TONSURE] |
| MH9 | Warrior Braids | Shoulder-length hair woven into many thin braids, coloured red with ochre and drawn back from the face. | Young warriors of herding peoples; shaved off when they become elders. | Maasai warriors [MAASAI] [DREAD] |
| MH10 | Oiled Ringlets | Shoulder-length black hair, oiled and set in rows of tight ringlets held by a band around the brow, with a long squared beard curled the same way. | Kings, courtiers, officials and rich merchants of old river cities in hot dry lands. | Assyria and Babylonia [BRIT-MESO] [BRIT-HAIR] |

---

## 4. Women's outfits (30)

Each description gives what a head-and-shoulders to waist-up portrait shows first, and the rest after "Below".

| # | Name | Description | Worn by | Model and source |
|---|---|---|---|---|
| WO1 | Kirtle and Apron | Fitted gown of undyed grey-brown wool laced at the front over a white linen shift that shows at the neck and wrists, sleeves pushed up, a knotted linen kerchief on the head. Below: a linen apron tied at the waist and an ankle-length skirt tucked up for work. | Peasant and servant women, farm wives; temperate lands; everyday. | Europe, 13th to 15th century [KIRTLE] [EUR-1300] [ENG-MED] |
| WO2 | Strap Dress and Brooches | Tube dress of coarse blue or brown wool hung from a strap over each shoulder, each strap fastened at the front with a domed oval bronze brooch, a string of glass and amber beads slung between the two brooches, over a long-sleeved linen shift; a wool cloak pinned at the chest. Below: a belt with a small leather purse. | Free farmers' wives and mistresses of a household on the cold coasts; the metal of the brooches and blue dye show wealth. | Viking Age Scandinavia [NATMUS-C] |
| WO3 | Sideless Surcoat | Floor-length sleeveless overgown with armholes cut open from shoulder to hip and a narrow front panel edged in white fur, showing a tight-sleeved gown of a contrasting colour and a jewelled hip belt beneath. | Noblewomen and ladies of a court in temperate lands; later a dress of ceremony. | Europe, 14th century [SURCOAT] [BRIT-MED] [EUR-1300] |
| WO4 | High-belted Gown | Voluminous gown of heavy dyed wool with a high standing collar and wide funnel sleeves lined with fur, belted just under the bust, the sleeve edges cut into scallops. Below: the skirt falls in deep folds to the floor. | Wealthy merchants' wives, gentry and court ladies in cool lands; winter and formal days. | Europe from about 1360: houppelande [HOUPP] [BRIT-MED] |
| WO5 | Queen's Mantle | Semicircular mantle of crimson or purple cloth lined with white fur and fastened across the chest with a jewelled clasp, over a gown embroidered with gold thread, and a gold crown set with stones. Below: the mantle trails behind as a train. | Queens and ruling princesses, at coronations and when the court sits. | Europe: royal mantle [MANTLE]; scarlet and purple kept for kings, fur linings for the rich [ENG-MED]. Ermine as the fur is **[unverified]** |
| WO6 | Nun's Habit | Loose tunic of black or undyed wool, a long plain panel hanging from the shoulders front and back, a white linen wimple covering the throat and cheeks, and a black veil over a white under-veil. Below: a knotted cord at the waist and a hem to the ground. | Nuns, lay sisters and temple women of any age; a novice wears a white veil. | Western Christian orders [HABIT] [WIMPLE] |
| WO7 | Fur Parka | Hooded pullover coat of caribou hide worn fur outward over an inner coat with the fur turned in, the hood edged with a ruff of long wolf fur around the face, bands of pale fur set in at the shoulders. Below: a wide pouch at the back for an infant, long rounded flaps front and back, fur trousers and sealskin boots. | Sea-mammal hunters, fishers and their families on ice and tundra coasts. Men wear the same with a close hood and a straight hem. | Inuit [INUIT] |
| WO8 | Banded Herder Tunic | High-collared pullover tunic in one strong colour with bands of contrasting colours at the collar, shoulders and cuffs, the collar embroidered in pewter thread, a fringed scarf at the neck and silver jewellery. Below: a belt set with silver buttons, a flared hem to the knee, hide leggings and boots. | Reindeer herders of tundra and northern forest, both sexes; the men's is shorter. Square belt buttons for the married, round for the unmarried. Hide for work, cloth for feasts. | Sámi gákti [GAKTI] (§11.2) |
| WO9 | Pinned Tunic and Wrap | Ankle-length tunic of fine pleated linen pinned along the shoulders and upper arms with small brooches to make elbow sleeves, belted and bloused at the waist, with a large rectangular wool wrap draped over the left shoulder and around the body; a key-pattern border at the hems. | Townswomen and citizens' wives of the warm coasts, any rank; the finer the linen, the richer the house. The wrap is drawn over the head outdoors. | Classical Greece: chiton and himation [BRIT-GR] |
| WO10 | Priestess Whites | White wool gown falling in long straight folds from shoulder straps, a white linen mantle over it, a white woollen band wound around the head with red and white ribbons hanging in loops over each shoulder, and a short white veil with a purple border over the head when outdoors. | Priestesses who tend a sacred fire; any order of temple virgins. | Rome: the Vestals [VESTAL] [STOLA] |
| WO11 | Jewelled Court Collar | Tight-sleeved silk tunic in deep purple or red under a wide collar of cloth of gold that covers both shoulders, thick with gems and edged with rows of pearls, a long gold-embroidered panel hanging down the front, and a crown with strings of pearls hanging at the temples. | Empresses, princesses and court ladies of an old imperial city; purple is kept for the ruling family. | Byzantium [BYZ] [BRIT-ME] |
| WO12 | Linen Sheath and Broad Collar | Close-fitting sheath of white linen held up by two shoulder straps, a sheer pleated linen shawl over the shoulders, a broad collar of rows of blue, green and red beads lying on the chest, and black lines of kohl around the eyes. Below: the sheath reaches the ankle. | Women of rank and rich households in hot dry river lands; the sheerer the linen, the higher the rank. The poor wear coarse linen and pottery beads. | Ancient Egypt [EGYPT] |
| WO13 | Desert Body Wrap | One long cotton cloth, about four metres, wrapped around the whole body over a shirt and drawn up over the head as a hood, the face left open, dyed a single strong colour. | Women of desert and dry-grass country, nomad and oasis alike; plain for every day, bright or embroidered for feasts. | Sahara and Sahel: melhfa [MELHFA]; loose garments that cover the body against heat and dust [BRIT-ME] |
| WO14 | Court Kaftan | Front-opening robe of patterned silk velvet woven with gold thread, in crimson, violet, indigo or yellow, closed down the chest by a row of small buttons, over a fine shirt, with a wide sash at the waist. | Court women and men, governors and rich merchants of hot dry and warm-coast cities; given by a ruler as a robe of honour. | Abbasid and Ottoman courts [KAFTAN] [BRIT-ME] |
| WO15 | Wrapped Riding Coat | Long coat of wool or cotton in blue, olive green or burgundy, the left flap wrapped over the right and fastened with clasps at the right shoulder and under the arm, bound at the waist with a broad silk sash of another colour, and a fur-trimmed hat. Below: the coat flares to below the knee over felt boots. | Steppe herders and riders of both sexes; silk for feasts, plain wool or cotton for every day. | Mongolia: deel [DEEL]. Fur hat and felt boots as in `races-basis.md`, Grass riders |
| WO16 | Draped Sari | One unsewn cloth four to eight metres long, wrapped and pleated at the waist, its decorated end carried across the chest and over the left shoulder, over a short fitted bodice; cotton for common wear, silk with gold-thread borders for the rich. Below: the pleats fall to the feet. | Women of the monsoon lands of every rank; red silk for brides. | South Asia [SARI] [BRIT-JKS] |
| WO17 | Jacket and High Skirt | Short cross-collared jacket of thin silk with long sleeves, tucked under a long red or yellow pleated skirt that is tied high above the bust with a ribbon, and a long sheer scarf draped over both arms. | Court ladies and gentry women of river-plain realms; ordinary women wear plain cloth with the skirt tied at the waist. | China, Sui and Tang: ruqun [RUQUN]. The scarf is **[unverified]** |
| WO18 | Layered Court Robes | Many unlined silk robes worn one over another, each a different colour, so that bands of colour show in steps at the neck and at the wide sleeve openings, with a short patterned silk jacket on top. Below: long red pleated trousers and a white train trailing behind. | Noblewomen and ladies-in-waiting at court; the set of colours follows the season. | Japan, Heian court: jūnihitoe [JUNI] [BRIT-JKS] |
| WO19 | Field Jacket and Straw Hat | Plain hip-length jacket of undyed hemp or cotton with the sleeves rolled, and a wide conical hat of woven straw or bamboo tied under the chin with a cloth strap. Below: loose trousers rolled to the knee, bare feet. | Farmers, rice growers, fishers and porters of both sexes in warm wet lowlands. | China: jacket over loose trousers for both sexes [BRIT-EAST]; conical hat [CONICAL]. Hemp or cotton and the straw hat as in `races-basis.md`, Canal folk |
| WO20 | Breast Cloth and Sarong | A long patterned cloth wound tightly around the torso from the armpits to the waist, the shoulders bare, held by a sash of another colour at the midriff, in brown, indigo and cream. Below: a second patterned cloth wrapped as an ankle-length skirt. | Women of hot-humid coasts and islands: villagers, dyers and weavers, dancers, court ladies in finer cloth. | Java and Bali: kemben [KEMBEN] |
| WO21 | Wrapper and Blouse | Loose wide-sleeved blouse of indigo tie-dyed cotton, a matching cloth tied as a head wrap, and a second cloth folded over one shoulder. Below: a large rectangular cloth wrapped around the waist to the ankle. | Market women, traders and artisans' wives of hot lands; hand-woven strip cloth for feasts. | West Africa: wrapper, blouse and head tie [WRAPPER] [HEADTIE]. Indigo as in `races-basis.md`, Sunlanders. Machine wax prints are left out as industrial |
| WO22 | Strip-woven Royal Cloth | A great cloth made of narrow hand-woven strips sewn edge to edge, silk and cotton, in blocks of gold, green, red and blue geometric pattern, wrapped around the body and over the left shoulder, with heavy gold jewellery. | Kings, queen mothers, chiefs and their courts in hot lands; feasts and audiences. Men wear it like a toga with the right shoulder bare. | Asante and Ewe: kente, cotton from the 17th century, silk from the late 18th [KENTE] [WRAPPER] (§11.2). How women drape it is **[unverified]** |
| WO23 | Red Herder Cloth | A sheet of red cloth or of softened hide rubbed red, wrapped around the body and knotted over one shoulder, a second sheet worn as a cape, stacked flat collars of coloured beads around the neck and metal hoops in stretched earlobes. | Cattle herders of the savanna, women and men; the jewellery shows age and marriage. | Maasai: red wrapped sheets, hides before cotton cloth, jewellery as a sign of status [MAASAI] (§11.2). The bead collars are **[unverified]** |
| WO24 | Pinned Mountain Dress | A large rectangle of fine wool wrapped around the body as a dress and pinned at the shoulders with a pair of long silver pins with flat round heads, a square shoulder cloth woven in bright stripes fastened at the chest with a third pin. Below: a wide woven belt and a hem to the ankle. | Women of the high valleys: farmers and herders; noblewomen have silver pins and a mantle with fine geometric bands. | Inca and later Andean dress: wrapped dress, shoulder cloth and tupu pins [MET-TUPU] **[partially verified]** [LLICLLA] [BRIT-AM] |
| WO25 | Healer's Gown and Satchel | Plain long-sleeved gown of undyed linen with the sleeves tied back, a clean white apron and head cloth, a leather satchel on a shoulder strap and bundles of dried herbs at the belt. | Healers, midwives and herbalists of any people. | Composed type **[unverified]**. Working women tied their sleeves up [BYZ] |
| WO26 | Hunter's Hood and Jerkin | Deep hood with a short shoulder cape of moss-green wool, a sleeveless jerkin of brown leather laced at the front over a rough linen shirt, a quiver strap across the chest and a leather bracer on the left forearm. Below: a belt with a long knife, wool hose and soft high boots. | Hunters, trappers, scouts and rangers of either sex in forest and hill country. | Fantasy convention (the ranger). The hood with a shoulder cape is historical [CHAPERON] |
| WO27 | Sorceress Robe | Floor-length robe of deep blue or wine-red wool with wide sleeves and a deep hood, clasped at the throat with a silver brooch, the hems embroidered with small symbols, an amulet on a long chain and a girdle hung with pouches. | Mages, seers and court sorceresses; a hedge witch wears the same cut in undyed cloth. | Fantasy convention. No historical model |
| WO28 | Padded Coat and Mail Shirt | Thigh-length coat of quilted linen stitched in vertical rows, under a short-sleeved shirt of iron mail, a mail hood pushed back onto the shoulders and a leather sword belt. | Women-at-arms, guards, mercenaries and shieldmaidens. | The armour is historical [GAMBESON] [HAUBERK]. Women in it are fantasy convention |
| WO29 | Beggar's Rags | Hooded cloak of patched, faded brown wool frayed at the edges, the hood up, over a stained and torn linen shift, a cord for a belt and a wooden bowl. Below: bare or rag-wrapped feet. | Beggars, the destitute, refugees and outcasts of either sex. | Composed type **[unverified]**. Coarse undyed wool as the lowest cloth and bare feet among the poor [ENG-MED] |
| WO30 | Player's Motley | Close-fitting bodice and tunic split down the middle into two bright colours, one half red and the other yellow, the edges cut into points, small brass bells at the collar and sleeves, ribbons in the hair, and a small drum or pipe in hand. | Travelling entertainers of either sex: dancers, tumblers, minstrels, jugglers and fools. | Bright motley, cap and bells; troupes hired for acrobatics and juggling [JESTER]; parti-colour from the mid 14th century [BRIT-MED]. On a woman it is a composed type |

---

## 5. Men's outfits (20)

| # | Name | Description | Worn by | Model and source |
|---|---|---|---|---|
| MO1 | Field Tunic and Hood | Knee-length belted tunic of coarse undyed wool with long sleeves over a linen shirt, and a wool hood with a short shoulder cape, pushed back; a straw hat in summer. Below: loose linen drawers, wool hose and thick leather shoes, or bare feet. | Peasants, farm hands, shepherds and carters of temperate lands. | Europe, 12th to 14th century [ENG-MED] [EUR-1100] [CHAPERON] |
| MO2 | Smith's Leather Apron | Long apron of thick dark leather, scorched and scarred, hung from the neck and tied at the waist over a linen shirt with the sleeves rolled above sooty forearms, and a leather band around the brow. | Smiths, farriers, founders and armourers. | Smiths wore aprons in medieval Europe [APRON]. Leather for the smith's apron is **[unverified]** |
| MO3 | Miner's Hood and Seat Leather | Close-fitting hood of undyed linen with a short cape over the shoulders, grey with rock dust, over a short belted tunic, a pick in hand. Below: a triangular leather apron worn at the back, hung from the belt to sit on. | Miners, tunnellers and quarrymen of hill and mountain country. | German-speaking Europe from the 15th century: the miner's apron [MINER]. The hood is **[unverified]** |
| MO4 | Furred Gown and Wound Hood | Long full gown of dark dyed wool with a high collar and wide sleeves, lined and edged with brown fur, belted at the waist with a purse, and on the head a hood wound into a padded roll with its tail draped over one shoulder. | Merchants, guild masters, lawyers, physicians and town councillors; older men; cool lands. | Europe, 14th and 15th century: houppelande and chaperon [HOUPP] [CHAPERON] [BRIT-MED] |
| MO5 | Doublet and Hose | Short close-fitting padded jacket of coloured wool laced down the front, with puffed upper sleeves, over a white linen shirt that shows at the neck. Below: tight hose tied to the jacket with laces, the two legs in different colours. | Young nobles, courtiers, squires, pages and household retainers; town youths of fashion. | Europe, 14th and 15th century [DOUBLET]; parti-coloured hose [BRIT-MED] |
| MO6 | King's Mantle and Crown | Long mantle of crimson or purple cloth lined with white fur and fastened at the right shoulder with a large gold clasp, over a long tunic embroidered with gold, and a gold crown set with stones. | Kings and ruling princes, at coronations, courts and days of judgement. | Europe: royal mantle, fastened at the shoulder [MANTLE]; scarlet and purple kept for kings [ENG-MED]. Ermine as the fur is **[unverified]** |
| MO7 | Monk's Habit | Ankle-length tunic of black, brown or undyed white wool with a hooded panel over the shoulders, the hood lying back, tied at the waist with a knotted cord. | Monks, friars, temple scribes, copyists and teachers; the colour tells the order. | Western Christian orders [HABIT] |
| MO8 | Mail and Surcoat | Knee-length shirt of iron mail with long sleeves and a mail hood framing the face, under a sleeveless cloth surcoat in the wearer's colours with his arms on the chest, and a conical iron helmet with a nose bar. Below: the surcoat hangs to mid-calf, split front and back for riding. | Knights, mounted men-at-arms and lords at war; too costly for most foot soldiers. | Europe, 11th to 14th century [HAUBERK] [SURCOAT] |
| MO9 | Padded Jack and Kettle Hat | Thigh-length coat of many layers of quilted linen stitched in vertical rows, off-white and stained, a wide-brimmed iron helmet shaped like a hat, and a spear in hand. | Town guards, militia, common foot soldiers and caravan guards. | Europe, 13th to 15th century [GAMBESON] [KETTLE] |
| MO10 | Lamellar Coat | Sleeveless coat of small rectangular plates of iron or lacquered leather laced together in overlapping horizontal rows, worn over a long wrapped coat, with a pointed iron helmet. | Steppe horse archers, heavy cavalry of eastern empires, guards of river-plain realms. | Steppe peoples, Byzantium, China [LAMELLAR]. The helmet is **[unverified]** |
| MO11 | Shoulder-pinned Cloak and Tunic | Knee-length long-sleeved wool tunic in a dyed colour with woven braid at the neck and cuffs, a heavy wool cloak edged with fur and pinned at the right shoulder with a round brooch so that the sword arm is free, and twisted silver arm rings. Below: trousers gathered under the knee, cloth strips wound around the shins, a belt with knife and purse. | Free farmers, sailors, traders, raiders and sworn guards of the cold coasts; silk trim and a fur lining for chieftains. | Viking Age Scandinavia [NATMUS-C]; as in `races-basis.md`, Northmen |
| MO12 | Citizen's Toga | A great semicircular cloth of white wool draped over the left shoulder, around the back, under the right arm and back over the left shoulder, in deep curved folds across the chest, over a white tunic; a broad purple border along the edge for a magistrate. | Citizens on public business, magistrates, senators and priests of warm-coast cities; dark wool for mourning, purple with gold for a triumph. | Rome [TOGA] |
| MO13 | One-shoulder Work Tunic | Short tunic of coarse undyed linen belted at the waist with the cloth bloused over the belt, fastened on the left shoulder only, so that the right shoulder and half the chest are bare. | Labourers, potters and light infantry of hot coasts; by extension fishers and sailors. | Greece: exomis [EXOMIS]. Fishers and sailors are **[unverified]** |
| MO14 | Linen Kilt and Broad Collar | Bare chest, a broad collar of rows of coloured beads over the shoulders, armlets, a shaved head or a short black wig, and black lines of kohl around the eyes. Below: a wrap-around kilt of pleated white linen held by a sash, to the knee, with a starched triangular front panel for a man of rank. | Scribes, officials, priests and nobles of hot dry river lands; a labourer wears a plain short kilt and nothing else. | Ancient Egypt [EGYPT] |
| MO15 | Veil-turban and Robe | A long cotton cloth dyed so deep with indigo that it shines blue-black, wound many times around the head as a turban and across the face so that only the eyes show, over a loose long-sleeved robe of white cotton; blue dye stains the skin of the cheeks and hands. | Adult men of desert nomad clans: camel herders, caravan guides, guards and chiefs. The deeper the dye, the richer the man; the fold tells the clan. | Tuareg: tagelmust [TAGELMUST]; ankle-length long-sleeved robe [THAWB]; as in `races-basis.md`, Saltroad folk |
| MO16 | Waist Cloth and Shoulder Cloth | Bare chest with a loose cloth laid over one shoulder, a large wound turban and bracelets. Below: a long unsewn white cotton cloth wrapped around the waist, pleated at the front, passed between the legs and tucked at the back, falling to the ankle. | Men of the monsoon lands of most ranks: farmers in a short plain one, priests, merchants; yellow silk for feast days. | South Asia: dhoti; turbans from the 2nd century BCE [DHOTI] [BRIT-JKS] |
| MO17 | Official's Badge Robe | Full-cut round-collared robe of red silk with wide sleeves, a large embroidered square on the chest showing a bird for a civil official or a beast for a military one, a stiff hooped belt and a black gauze cap over the topknot. | Officials, magistrates and scribes of a river-plain realm; the creature on the badge gives the grade. | China, Ming: rank badge on a red robe [MANDARIN] [BRIT-EAST]; as in `races-basis.md`, Canal folk. The belt and cap are **[unverified]** |
| MO18 | Wide-sleeved Grand Robe | A very wide flowing robe of dyed cotton damask reaching the ankles, its huge open sleeves folded back onto the shoulders, dense pale embroidery around the neck opening and down the chest, over a long shirt and loose trousers, with a wrapped turban. | Merchants, scholars, caravan masters, chiefs and nobles of dry-grass trade towns; women wear it too. | Sahel and West Africa from the 12th century: boubou [BOUBOU] |
| MO19 | Crossed Robe and Sash | Long wide-sleeved robe of plain dark cloth, the left front crossed over the right to make a V at the neck, tied at the waist with a cloth sash, hair in a topknot. Below: the robe falls in folds to the feet. | Townsmen, scholars, shopkeepers and gentry of a river-plain realm; silk for the rich, hemp or cotton for the rest. | China, Han: wide-sleeved robe belted at the waist [BRIT-EAST]; cross-collar [RUQUN]. Left over right and the sash as in `races-basis.md`, Canal folk |
| MO20 | Wizard's Robe and Pointed Hat | Long loose robe of grey wool with very wide sleeves, a tall pointed hat with a wide brim in the same cloth, a rope belt hung with pouches and a gnarled wooden staff. | Wizards, hedge mages, wandering sages; usually old. | Fantasy convention. No historical model |

**Entries for either sex.** These are listed once and can be worn by men and women, by the cited source or by convention: WO7 Fur Parka, WO8 Banded Herder Tunic, WO14 Court Kaftan, WO15 Wrapped Riding Coat, WO19 Field Jacket and Straw Hat, WO22 Strip-woven Royal Cloth, WO23 Red Herder Cloth, WO26 Hunter's Hood and Jerkin, WO29 Beggar's Rags, WO30 Player's Motley and MO18 Wide-sleeved Grand Robe. WO28 Padded Coat and Mail Shirt is MO9 and MO8 on a woman. Among the hairstyles, WH11 Beaded Cornrows and WH10 Blunt Braided Wig are worn by men too, and WH13 Shaved Head and MH8 Bare Scalp are one style under two names.

---

## 6. Hair colours (24)

Natural colours only. Hair colour comes from two pigments: more of the dark one gives brown and black, more of the red one gives red, and little of either gives blond; grey and white hair has lost its pigment [HAIRCOL]. Greying can start in the twenties and ends in grey and then white [HAIRCOL]. The first ten entries are base colours. HC11 to HC14 are for age. HC15 is not an age colour. HC16 to HC24 were added for the races of `races-basis.md`: each takes its colour from the Appearance paragraph of the race named in the note, and no outside source was read for them.

| # | Name | Prompt-ready description | Note |
|---|---|---|---|
| HC1 | Black | jet black hair with a faint blue sheen | The most common colour worldwide [HAIRCOL] |
| HC2 | Soft Black | off-black hair that shows dark brown in sunlight | |
| HC3 | Dark Brown | dark brown hair | Brown is the second most common colour [HAIRCOL] |
| HC4 | Chestnut | chestnut brown hair with a reddish cast | A darker reddish brown [HAIRCOL] |
| HC5 | Light Brown | light ash-brown hair | |
| HC6 | Sandy | sandy dark-blond hair | Ash and sandy tones [HAIRCOL] |
| HC7 | Golden Blond | golden blond hair | |
| HC8 | Flaxen | pale flaxen blond hair, the colour of dry straw | The word is used for the Northmen in `races-basis.md` |
| HC9 | Copper Red | bright copper-red hair | Red is the rarest colour, 1 to 2 % in west Eurasia [HAIRCOL] |
| HC10 | Auburn | dark auburn hair, red-brown | |
| HC11 | Greying Dark | dark hair streaked with grey at the temples and through the length | Middle age, for HC1 to HC5 |
| HC12 | Greying Fair | faded blond or red hair turning ash-white at the temples | Middle age, for HC6 to HC10 |
| HC13 | Iron Grey | fully iron-grey hair | Late middle age and after |
| HC14 | White | thin white hair of old age | Old age |
| HC15 | Silver-white | smooth, shining silver-white hair on a young face | Not from age. Elves of the cold woods are "silver-haired", Hoarkin hair is white or ash-grey and Gnomes whiten early (`races-basis.md`). Rare among humans |
| HC16 | Ash Grey | pale ash-grey hair, even in colour from root to tip, on a young face | Not from age. Hoarkin ("white or ash-grey") and Lanternkin ("the colour of ash"); grey down of the Eyrie folk |
| HC17 | Snow White | dense pure white hair with no yellow in it, on an unlined face | Not from age. Hoarkin and Hushfolk ("fine white or silver"); Gnomes, whose hair "goes white early"; white down of the Eyrie folk |
| HC18 | Dark Red | deep dark red hair, near black in shadow and wine-red in the light | Hearthbloods ("black or dark red") |
| HC19 | Bark Brown | dull grey-brown hair, the colour of tree bark | Amberbloods ("chestnut, copper or bark-brown") |
| HC20 | Salt Pale | pale, stiff hair bleached almost white, with a dull straw cast | Brinefolk ("pale and stiff"). The straw cast is my addition |
| HC21 | Ringed Dark and Pale | long hair banded along its whole length in rings of dark brown and pale blond, each ring a hand's width | Swiftborn: dark growth in summer and pale in winter, one pair of rings for each year of life. The number of rings should follow the age in the Stub |
| HC22 | Buff | soft buff, a pale yellow-brown | Down of the Eyrie folk ("grey, buff, white or barred brown") |
| HC23 | Barred Brown | brown crossed with narrow darker bars, like the feathers of a hawk | Down of the Eyrie folk. The hawk comparison is my addition |
| HC24 | Brindled Brown-grey | thick hair streaked unevenly in brown and grey | Not from age. Stillfaces ("brindled in brown and grey") |

---

## 7. Skin colours (34)

§7.1 holds the twelve tones of human skin. §7.2 holds 22 tones added for the races of `races-basis.md` whose skin §7.1 does not reach.

### 7.1 Human tones (12)

Each entry gives a tone and an undertone. The entries run from light to dark so that one can be found; the order is not a ranking. Human skin colour ranges from the lightest hues to the darkest brown and is in general darker where the ultraviolet light is stronger [SKINCOL]. The six types of the Fitzpatrick scale are given as a rough anchor only: that scale was made to predict sunburn, began with four types for white skin, and is criticised for describing dark skin poorly [FITZ].

Food words are avoided, with one exception: "olive" is kept because it is the standard word for that tone [FITZ] and `races-basis.md` uses it for Midlanders and Orcs.

| # | Name | Prompt-ready description | Anchor |
|---|---|---|---|
| SC1 | Very Fair, Cool | very fair skin with a cool pink undertone, lightly freckled | Type I [FITZ] |
| SC2 | Fair, Neutral | fair skin with a neutral ivory undertone | Type II [FITZ] |
| SC3 | Ruddy Fair | fair skin with a ruddy, wind-reddened flush across the cheeks and nose | Type I to II. "Fair to ruddy" Northmen and ruddy Dwarves in `races-basis.md` |
| SC4 | Light Beige, Warm | light beige skin with a warm yellow undertone | Type III [FITZ] |
| SC5 | Olive | light olive skin, a light brown with a muted green-grey undertone | Type III to IV [FITZ] |
| SC6 | Golden Tan | golden-tan skin with a warm yellow-gold undertone | Type III to IV. "Pale tan to golden-brown" Canal folk in `races-basis.md` |
| SC7 | Weathered Bronze | sun-weathered bronze skin with a warm red-brown cast, creased at the eyes | Type IV. "Tan to bronze and weathered early" Grass riders in `races-basis.md` |
| SC8 | Light Brown, Golden | light brown skin with a warm golden undertone | Type IV [FITZ] |
| SC9 | Red-brown | medium red-brown skin with a warm copper undertone | Type V. "Deep red-brown" Saltroad folk in `races-basis.md` |
| SC10 | Medium Brown, Neutral | medium brown skin with a neutral undertone | Type V [FITZ] |
| SC11 | Dark Brown, Warm | dark brown skin with a warm red undertone | Type VI [FITZ] |
| SC12 | Deep Brown, Cool | very deep brown skin, near black, with a cool blue sheen in the highlights | Type VI. "A black with a blue sheen" Sunlanders in `races-basis.md` |

### 7.2 Tones added for the races of `races-basis.md` (22)

Every entry takes its colour from the Appearance paragraph of the race in the last column, and the quoted words are that paragraph's. No outside source was read for them. Where the visible marker of a race is a colouring of the skin itself (blue-grey lips, a green-bronze veining, bright patches), the entry carries it, because a skin colour without it would be wrong for that race. Markers that are not skin colour are in §8.3 (eyes) and §8.4 (antlers, tusks, ears, salt crust, scars).

SC13 and SC14 are ordinary human tones that §7.1 lacked. The rest belong to one race each.

| # | Name | Prompt-ready description | Race and its words |
|---|---|---|---|
| SC13 | Deep Red-brown | deep red-brown skin with a warm copper undertone | Saltroad folk of the far desert and the highlands ("a deep red-brown"); Boughwalkers ("dark brown to red-brown"). Darker than SC9 |
| SC14 | Deep Olive | deep olive skin, a medium brown with a muted green undertone | Orcs ("olive"); the dark end of the Swiftborn ("warm olive or brown"). Darker than SC5 |
| SC15 | Seamed Ruddy | ruddy, wind-reddened skin, thick and deeply creased like worked leather | Dwarves ("ruddy or brown and seamed like worked leather") |
| SC16 | Seamed Brown | medium brown skin, thick and deeply creased like worked leather | Dwarves |
| SC17 | Grey-green | dull grey-green skin, the colour of weathered slate with a moss cast | Orcs ("grey-green, olive or dun brown"). The slate and moss comparison is my addition |
| SC18 | Dun Brown | dull grey-brown skin with no warmth in it | Orcs |
| SC19 | Frost Pale | very pale white skin with a cold cast, shading to blue-grey at the lips, the eyelids, the nails and the tips of the ears | Hoarkin ("as pale as skimmed milk that shades to blue-grey") |
| SC20 | Wind-browned | light brown skin darkened and roughened by wind and sun on the face and hands | Tinefolk ("wind-browned") |
| SC21 | Flushed Copper | flushed copper-red skin, as if just in from a hard run | Hearthbloods ("a flushed copper red"). In frost, steam rises from the bare shoulders; that belongs to the scene, not to the skin |
| SC22 | Tawny | tawny skin, a light yellow-brown | Widegaze ("tawny skin") |
| SC23 | Resin-freckled Gold | brown-gold skin scattered with small freckles the colour of clear amber resin | Amberbloods. The old have ridged skin and a faint wood grain on the backs of the hands |
| SC24 | Shell Grey-white | smooth grey-white skin with the faint pearly sheen of the inside of a shell | Brinefolk |
| SC25 | Deep Brown, Grey | deep brown skin with a cool grey undertone | Duskreaders ("deep brown skin that has a grey undertone") |
| SC26 | Copper-brown, Green-veined | copper-brown skin with a fine tracery of green-bronze lines, like the veins of a leaf, across the shoulders and upper arms | Noonfolk. The lines darken through the day and pale overnight |
| SC27 | Deep Olive, Green-veined | deep olive skin with a fine tracery of green-bronze lines, like the veins of a leaf, across the shoulders and upper arms | Noonfolk ("from copper-brown to deep olive") |
| SC28 | Grey-cast Brown | smooth medium brown skin with a grey cast and a faint oiled sheen | Shoalfolk ("brown with a grey or green cast, smooth and faintly oiled") |
| SC29 | Green-cast Brown | smooth medium brown skin with a green cast and a faint oiled sheen | Shoalfolk |
| SC30 | Sunless Grey | pale grey skin that has seen little sun | Hushfolk |
| SC31 | Indoor Pale | soft pale skin with no trace of sun, as of someone who lives indoors | Longsleepers ("pale from a life spent mostly indoors") |
| SC32 | Clear Pale | very pale, thin skin that seems almost clear, with a faint blue-green tracery of veins at the wrists, the throat and the temples | Lanternkin. The tracery shows only in shadow and glows faintly there; in daylight the skin looks only sickly |
| SC33 | Tawny-grey | tawny-grey skin, a light yellow-brown dulled with grey | Stillfaces |
| SC34 | Olive-brown, Bright-patched | dark olive-brown skin broken by patches of bright colour, sky blue and sulphur yellow, across the throat, the shoulders and the outer arms | Bitterbloods. Orange patches occur. The brighter the patches, the stronger the blood |

Races that need no entry of their own: Northmen, Midlanders, Sunlanders, Grass riders, Rainkeepers, Canal folk, Elves and Twofold are covered by §7.1. Flinchless and Lineminds have skin "of every kind" and "like the human peoples they live among". The skin of Halflings and Gnomes is not stated.

---

## 8. Lists added for the races of `races-basis.md`

§2 to §5 were drawn from history and reach the seven human races well. They reach the five classic races and the twenty new races poorly, because those wear what their Appearance paragraph says and nothing else. §8.1 and §8.2 add an entry for every hairstyle and every dress that an Appearance paragraph names and that §2 to §5 lack. §8.3 and §8.4 add two kinds of component the first pass left out: eye colours and race markers.

- **Source.** Each entry rests on the Appearance paragraph of the race in the last column, and the quoted words are that paragraph's. No outside source was read. Whatever the description adds beyond the quoted words (a cut, a material, a number) is my addition and is named as such.
- **Sex.** Every entry is for either sex unless "Worn by" says otherwise. The paragraphs split dress by sex for no classic or new race.
- **Labels.** RH is a hairstyle and RO an outfit, numbered apart from the lists of §2 to §5. EC is an eye colour and RM a race marker.

### 8.1 Hairstyles (17)

| # | Name | Description | Worn by | From `races-basis.md` |
|---|---|---|---|---|
| RH1 | Combed Long with Braided Beard | Hair grown past the shoulders and combed smooth straight back from the brow, with a full beard plaited into two braids. | Men of the cold coasts. | Northmen: hair "worn long and combed daily. Men braid their beards". Two braids is my addition. Compare MH5 |
| RH2 | Pattern-shaved Scalp | Tightly coiled black hair cropped close to the scalp, with lines and curved bands shaved into it down to the skin. | Either sex in hot lands; the pattern shows age, rank and home. | Sunlanders: "shaved in patterns" |
| RH3 | Clay and Bead Crown | Tightly coiled hair worked with clay into a smooth, matte, raised mass on the crown of the head, set with rows of coloured beads. | Either sex in hot lands; elders and people of rank. | Sunlanders: "built up with clay and beads to show age, rank and home". The shape is my addition. Compare WH12 |
| RH4 | Oiled Long Hair | Long black hair, straight or wavy, glossy with oil, combed back from the brow and falling loose past the shoulders. | Men and women of the monsoon lands. | Rainkeepers: "oiled, and worn long by both sexes". Twofold: "black and worn long by everyone" |
| RH5 | Long and Swept Back | Long straight hair to the middle of the back, combed back from a smooth brow and tucked behind the ears so that the whole ear shows, the face beardless. | Elves of either sex. | Fantasy convention. The paragraph gives the hair colour of an Elf and no style; the ears are shown because they "rise to a point" |
| RH6 | Ringed Beard Braids | A full beard plaited into three thick braids, each bound with metal rings, and the hair of the head drawn back into one heavy plait. | Dwarves, men and women alike; the rings show clan, craft and standing. | Dwarves: beards "braided and ringed". Three braids and the plait of the head hair are my addition |
| RH7 | Curly Mop | Thick, springy curls cut short enough to clear the collar, falling over the brow and around the ears. | Halflings. | Halflings: "thick and curly". The length is my addition |
| RH8 | White Tufts | Fine hair standing out in tufts above the ears and at the back of the head, thin on the crown. | Gnomes. | Gnomes: "worn in tufts". The thin crown is my addition |
| RH9 | Pointed Cap | Hair hidden under a soft pointed cap of felt pulled down to the tops of the ears, a few tufts showing at the temples. | Gnomes, at work and out of doors. | Gnomes: "or under a pointed cap". Felt is my addition |
| RH10 | Coarse Crest | The sides of the head shaved bare and a strip of coarse hair, a hand wide, standing up stiffly from the brow to the nape. | Orcs. | Orcs: "coarse and black and is worn in a crest". The shaved sides are my addition |
| RH11 | Rough Knot | Coarse hair dragged back and tied in a rough knot at the back of the crown with a strip of hide, loose strands escaping. | Orcs. | Orcs: "or a knot". Compare MH6, which is neat and pinned |
| RH12 | Dense Close Crop | Fine, very dense hair cut to a finger's length all over the head, lying as close and even as a pelt. | Hoarkin. | Hoarkin: "fine and dense". The paragraph gives no cut; the cut is my addition |
| RH13 | Hard Tie-back | Hair scraped straight back from the brow and temples and bound tight at the nape, with no fringe and no loose strand anywhere near the eyes. | Widegaze. | Widegaze: "tied back hard, so that it never crosses the eyes" |
| RH14 | Hairless | No hair at all: a smooth bare scalp without stubble, and no eyebrows. | Noonfolk. | Noonfolk: "no hair at all". No eyebrows is my reading of that. Not the same as WH13 and MH8, which are shaved |
| RH15 | Flat Skull Braids | Black hair braided in tight narrow rows flat against the skull, the ends tucked under and sewn down, with nothing hanging and no ornament. | Boughwalkers. | Boughwalkers: "braided flat to the skull, so that nothing can catch on it". Compare WH11, whose ends hang with beads |
| RH16 | Scalp Down | Fine, soft, close down in place of hair, thick on the scalp and the nape and thinning down the neck. | Eyrie folk. | Eyrie folk: "fine, soft and close ... thick on the scalp and nape and thinning along the forearms". Colour from HC16, HC17, HC22 or HC23 |
| RH17 | Oiled Slick-back | Black hair combed straight back from the brow and held flat to the skull with oil, ending at the nape. | Bitterbloods. | Bitterbloods: "slicked back with oil". The length is my addition |

Hair that §2 and §3 already cover:

- Hearthbloods, "cropped short": MH1 and WH19.
- Shoalfolk, "cropped short or braided with shell": MH1, WH19 and WH11.
- Swiftborn, "worn long" so that an age can be counted "off a braid": WH2 with hair colour HC21, for men too.
- Grass riders: MH7. Canal folk: WH15 and MH6.

Races whose paragraph gives a hair colour or texture and no style: Midlanders, Saltroad folk, Tinefolk, Amberbloods, Brinefolk, Hushfolk, Lanternkin and Stillfaces. The hair of Duskreaders, Longsleepers, Flinchless and Lineminds is not described.

### 8.2 Outfits (25)

| # | Name | Description | Worn by | From `races-basis.md` |
|---|---|---|---|---|
| RO1 | Guild-cut Town Dress | Hip-length coat of good wool dyed one strong colour, cut close and buttoned to the neck, a soft cap of the same cloth, and a small metal guild badge pinned at the breast. Below: a belt with a purse, hose and leather shoes. | Townsfolk of temperate lands: guild masters, journeymen and their households. | Midlanders: "dyed cloth cut to the fashion of their guild". The cut and the badge are my addition |
| RO2 | Indigo Wrapped Cotton | A long cotton cloth dyed deep indigo, wrapped around the body and thrown over the left shoulder with the right shoulder bare, a band of a second strong colour along its edge. Below: the cloth falls to the calf. | Men and women of hot lands, every day. | Sunlanders: "wrapped cotton in strong dyes, indigo first among them". The drape is my addition, after WO22 |
| RO3 | Woodland Greys and Greens | Long-sleeved tunic of soft wool in dull green under a hooded cloak mottled in grey and brown, closed at the throat with a plain leaf-shaped clasp, with no bright metal or colour anywhere. Below: a narrow belt, close hose and soft boots in the same dull colours. | Elves. | Elves: "greens, greys and browns that are hard to see between trees". The garments are fantasy convention; the paragraph gives the colours only |
| RO4 | Thick Wool and Tool Belt | Heavy tunic of thick dark wool under a sleeveless jerkin of stiff brown leather, thick leather bracers, and a broad belt with a hammer or a pick hung from it. Below: wool trousers and heavy boots. | Dwarves. | Dwarves: "leather and thick wool and are rarely without a tool on the belt". The cut is my addition |
| RO5 | Bright Waistcoat and Wide Hat | Bright waistcoat of yellow or red cloth with brass buttons over a full-sleeved white linen shirt, and a wide-brimmed felt hat. Below: breeches and good polished leather boots. | Halflings; the brighter and finer, the more prosperous. | Halflings: "bright waistcoats, wide hats and good boots". A waistcoat is later than the period limit of this note (§11.1 item 5); the entry follows the doc |
| RO6 | Many-pocketed Coat | Knee-length coat of sturdy brown cloth covered in pockets of every size, with flaps and loops and tools and rolled papers showing from them, and round spectacles of ground glass on the nose. | Gnomes. | Gnomes: "They dress in many pockets", and "ground glass spectacles". The coat is my addition |
| RO7 | Hide and Won Armour | Sleeveless jerkin of thick raw hide laced with thongs, one dented iron shoulder plate strapped on one side and a strip of mail hung at the throat, no two pieces matching, the arms bare and scarred. Below: hide breeches and a wide belt. | Orcs. | Orcs: "leather, hide and whatever armour was won". The pieces are my addition |
| RO8 | Single Sealskin | One thin, close, hooded coat of sleek grey sealskin worn hair outward, with no fur ruff and nothing under it, the throat and hands bare. Below: sealskin trousers and boots. | Hoarkin. | Hoarkin: "one layer of sealskin, against wind and wet. They do not need it against cold". Compare WO7, which is two layers with a ruff |
| RO9 | Fitted Hide, Hair Inward | Close-fitting pullover tunic of pale scraped reindeer hide with the hair turned inward, showing as a rim of fur at the high collar and the cuffs, and no hood. Below: fitted hide leggings and boots. | Tinefolk. | Tinefolk: "fitted reindeer hide with the hair turned inward". No hood, because of the antlers, is my addition |
| RO10 | Kilt and Open Vest | Open sleeveless leather vest over a bare chest, the shoulders and arms bare. Below: a knee-length kilt of heavy wool held by a wide belt, and bare legs. | Hearthbloods, in any weather. | Hearthbloods: "a kilt and an open vest in weather that kills". The materials are my addition |
| RO11 | Plain Long Coat and Cut-away Hat | Long plain coat of undyed wool buttoned to the neck, without trim, and a hat whose brim is cut away at both sides, so that it shades the brow and the nape and leaves the view to left and right clear. Below: the coat falls to mid-calf. | Widegaze. | Widegaze: "long plain coats and a brimmed hat cut away at the sides" |
| RO12 | Pale Wrapped Linen | Pale undyed linen wrapped around the body in several turns and over one shoulder, and a line of dark paint drawn around each eye. Below: the linen falls to the ankle. | Brinefolk of both sexes. | Brinefolk: "pale wrapped linen, and both sexes line the eyes with dark paint". The drape is my addition |
| RO13 | Quilted Robe and Cloth Mask | Thick quilted robe of dark cloth closed high at the throat, long sleeves ending in gloves, and a mask of stiffened cloth over the whole face. Below: the robe falls to the feet. | Duskreaders among themselves. | Duskreaders: "thick quilted robes, gloves and often a mask of stiffened cloth". The dark colour is my addition |
| RO14 | Quilted Robe and Horn Visor | Thick quilted robe of dark cloth closed high at the throat, gloves, and across the eyes a visor of smoked horn with one narrow level slit. Below: the robe falls to the feet. | Duskreaders by day and among other peoples. | Duskreaders: "a visor of smoked horn with a slit in it, or a veil of dark gauze" |
| RO15 | Hip Wrap, Bare Back | Nothing worn above the waist: the chest, the shoulders and the whole back bare to the sun. Below: a plain cloth wrapped at the hips to the knee. | Noonfolk. | Noonfolk: "a wrap at the hips, and nothing that shades the back" |
| RO16 | Short Wraps and Pouch Belts | Bare shoulders and arms, a short wrap of plain brown or white cloth around the chest, and belts crossed over the torso hung with small pouches. Below: a second short wrap at the hips, bare legs and feet. | Boughwalkers. | Boughwalkers: "belts, pouches and short wraps of cloth that looks plain brown or white to everyone else". The chest wrap and the crossed belts are my addition |
| RO17 | Loincloth and Dive Belt | Bare chest and shoulders, a knife in a sheath on a cord at the hip. Below: a plain loincloth and, for diving, a belt of lead weights. | Shoalfolk. | Shoalfolk: "a loincloth and a knife, and add a belt of lead weights when diving" |
| RO18 | Soft Felt | Loose tunic and hooded shoulder cape of thick, soft grey felt, closed with cloth ties, with no metal, bead or ornament anywhere. Below: felt trousers and soft felt shoes. | Hushfolk. | Hushfolk: "soft felted cloth that does not rustle and wear nothing that jingles". The cut is my addition |
| RO19 | Loose Layered Robes | Two or three loose robes of soft pale cloth worn one over another, open at the neck, wide in the sleeve and tied with a soft sash. Below: the robes fall to the ankle over bare feet. | Longsleepers, day and night. | Longsleepers: "loose layered robes that serve for day and night alike, and they go barefoot indoors" |
| RO20 | Close Quilted Silk | Close-fitting jacket of quilted silk stitched in narrow rows, high at the neck and tight at the wrists, with no cloak, no hanging sleeve and nothing loose. Below: close quilted silk leggings. | Eyrie folk. | Eyrie folk: "close quilted silk and wear nothing loose" |
| RO21 | Hood, Long Sleeves and Gloves | Deep hood that shades the whole face, a long-sleeved tunic closed at the throat, and thin gloves, so that no skin but the face shows. Below: a long skirt or hose to the ankle. | Lanternkin out of doors. | Lanternkin: "out of doors they wear a hood, long sleeves and gloves" |
| RO22 | Wrapped Cotton, Saffron Sash | Plain cotton cloth wrapped around the body and over one shoulder, cut the same for everyone, tied at the waist with a saffron-yellow sash. | Twofold before the Turning, up to about thirty. | Twofold: "Everyone dresses alike, in wrapped cotton. Only the sash differs: saffron before the Turning, indigo after it" |
| RO23 | Wrapped Cotton, Indigo Sash | Plain cotton cloth wrapped around the body and over one shoulder, cut the same for everyone, tied at the waist with a deep indigo sash. | Twofold after the Turning. | Twofold, as RO22 |
| RO24 | Bare-shouldered Home Wrap | A cloth wrapped under the arms and tied at the chest, leaving the throat, the shoulders and the arms bare, so that the patches of bright skin show. | Bitterbloods at home. | Bitterbloods: the patches are "worn uncovered with pride at home". The garment is my addition |
| RO25 | Gloves and Waxed Skin | Long-sleeved robe closed at the throat, thin leather gloves, a dull waxy coating on the skin of the face and neck, and a small cup hung at the belt. | Bitterbloods among other peoples. | Bitterbloods: "gloves and a coat of wax on the skin, and carries a cup of their own". The robe is my addition |

Dress that §4 and §5 already cover: Northmen (WO2, MO11), Midlander peasants and nobility (WO1, MO1; WO3, WO4, MO4, MO8), Saltroad folk (WO13, MO15), Grass riders (WO15), Rainkeepers (WO16, MO16) and Canal folk (MO19 for both sexes, WO19, MO17).

Races whose paragraph names no dress: Amberbloods, Swiftborn, Flinchless, Stillfaces and Lineminds.

### 8.3 Eye colours (25)

Eye colour is picked per person, like hair and skin colour. EC1 to EC8 are ordinary human colours. EC9 to EC12 are plain colours that only a non-human race has. EC13 to EC25 belong to one race each and carry that race's marker where the marker is in the eye.

| # | Name | Prompt-ready description | Race and its words |
|---|---|---|---|
| EC1 | Blue | clear blue eyes | Northmen ("blue, grey or green"); Midlanders ("eyes of any colour") |
| EC2 | Grey | clear grey eyes | Northmen; Midlanders; Gnomes ("a clear grey or brown") |
| EC3 | Green | green eyes | Northmen; Midlanders; Saltroad folk ("now and then green or amber"); Elves of the oak woods ("green-eyed"); Noonfolk ("green or gold") |
| EC4 | Hazel-green | hazel-green eyes, green at the rim and brown around the pupil | Boughwalkers ("large and hazel-green"); Midlanders. Where the green and the brown sit is my addition |
| EC5 | Clear Brown | clear light brown eyes | Gnomes; Midlanders |
| EC6 | Earth Brown | dark brown eyes | Saltroad folk ("dark brown"); the "dark" eyes of Grass riders, Rainkeepers, Canal folk, Shoalfolk, Stillfaces and Eyrie folk |
| EC7 | Jet | eyes so dark that iris and pupil read as one black | The same "dark" eyes; Longsleepers in the last hour before sleep ("to black") |
| EC8 | Amber | amber eyes, a clear golden brown | Saltroad folk, now and then; Eyrie folk ("amber or dark"); Midlanders |
| EC9 | Gold | clear gold eyes | Elves of the warm forests ("gold-eyed"); Noonfolk; Tinefolk in summer ("gold in summer") |
| EC10 | Yellow | small yellow eyes | Orcs ("small, yellow or red-brown") |
| EC11 | Rust Brown | small red-brown eyes | Orcs |
| EC12 | Deep Blue | deep blue eyes, dark as the sky after sunset | Tinefolk in the dark months ("turning deep blue"). The comparison is my addition |
| EC13 | Rimmed Pale Grey | pale grey eyes with a dark rim around the iris | Hoarkin ("pale grey with a dark rim") |
| EC14 | Ember Dark | dark eyes with a ruddy light in them, like a coal seen through ash | Hearthbloods ("dark with a ruddy light in them"). The comparison is my addition |
| EC15 | Bar-pupil Gold | pale gold eyes whose pupil is a dark horizontal bar, level with the ground | Widegaze: "The iris is pale gold, and the pupil is a dark bar that stays level with the ground" |
| EC16 | Bead Amber | amber-gold eyes that catch the light like a bead of resin held to the sun | Amberbloods ("amber and catch the light like a bead held to the sun") |
| EC17 | Very Light Grey | very light grey eyes, almost without colour | Brinefolk ("a very light grey or green") |
| EC18 | Very Light Green | very light green eyes, almost without colour | Brinefolk |
| EC19 | All Black | large eyes that are wholly black from lid to lid, with no white and no iris, glossy as wet stone | Duskreaders ("wholly black, without a visible white or iris, and glossy as wet stone") |
| EC20 | Clouded White | eyes clouded milk-white from edge to edge, with no iris or pupil to be seen | Hushfolk ("clouded white from edge to edge") |
| EC21 | Silver-grey | pale silver-grey eyes | Flinchless ("a pale silver-grey, which was the maker's mark") |
| EC22 | Lilac | pale lilac eyes | Longsleepers on waking ("a pale lilac on waking") |
| EC23 | Violet | violet eyes | Longsleepers by afternoon ("to violet by afternoon") |
| EC24 | Light Grey-blue | light grey-blue eyes | Lanternkin ("a light grey-blue") |
| EC25 | Yellow-ringed Dark | dark eyes with a narrow yellow ring around the iris | Bitterbloods ("dark with a yellow ring"). Where the ring sits is my addition |

The eye colour of Sunlanders, Dwarves, Halflings, Swiftborn and Twofold is not stated, nor that of Elves of the cold woods. Lineminds have the eyes of the human race they live among.

### 8.4 Race markers (25)

A marker is what a portrait must show of a race beyond skin, hair, eyes and dress. It is fixed for the race, not picked for the person: every person of the race gets the same text, or one of two texts where the marker changes with age or season. RM1 to RM5 are for the classic races, which have a Looks line and no "visible marker"; the word is extended to them here. RM6 to RM23 are for the new races. RM24 and RM25 are not marks of the body: they are things two human races always wear.

"Seen in" says how much of the figure a picture must show before the marker is visible.

| # | Name | Prompt-ready description | Race and its words | Seen in |
|---|---|---|---|---|
| RM1 | Pointed Ears and Ageless Face | ears that rise to a point, and a fine-boned face without a single line on it | Elves: "The ears rise to a point"; "shows no age after the thirtieth year" | Head and shoulders |
| RM2 | Heavy Brow and Big Hands | a heavy brow, thick wrists and big hands, on a body nearly as broad as it is tall | Dwarves: "nearly as broad, with thick wrists, big hands and a heavy brow". The beard is in RH6 | Brow: head and shoulders. The rest: waist-up |
| RM3 | Round Face and Stout Build | a round face and a sturdy, stout body, built like a small adult and not like a child | Halflings: "built like small adults, not like children: sturdy, round in the face and inclined to be stout" | Head and shoulders |
| RM4 | Large Head, Nose and Ears | a large head on a slight body, a long nose, big ears, very large eyes, and long thin fingers | Gnomes: "a large head, a long nose, big ears and very large eyes"; "The fingers are long and thin". The spectacles are in RO6 | Head and shoulders |
| RM5 | Tusks and Ashed Scars | two tusks rising from the lower jaw, a low brow and a thick neck, and old scars left to show, some rubbed pale with ash so that they stand out | Orcs: "a low brow and two tusks rising from the lower jaw"; scars "sometimes rubbed with ash so that they stand out" | Head and shoulders |
| RM6 | Velvet Antlers | an antler growing from the brow above each temple, as long as a forearm, with four points, covered in grey velvet | Tinefolk in summer: "between a hand's length and a forearm's, with two to five points ... covered in grey velvet through the summer". The length and the number of points should be drawn per person; this text takes one value | Head and shoulders |
| RM7 | Bare Antlers | an antler growing from the brow above each temple, as long as a forearm, with four points, bare and polished | Tinefolk in autumn: "bare and polished in the autumn, and shed at midwinter". From midwinter no antler is shown; when the new ones show is not stated | Head and shoulders |
| RM8 | Steaming Skin | steam rising from the bare shoulders, and a cloud of breath around the head | Hearthbloods, in cold air only: "walks in a cloud of their own breath, steam rises from the bare shoulders". The skin colour is SC21 | Head and shoulders, in a cold scene |
| RM9 | Wide-set Eyes | eyes set very wide, almost at the corners of the face, and a long neck | Widegaze: "The eyes are set wide, almost at the corners of the face". The pupil is in EC15 | Head and shoulders |
| RM10 | Wood-grain Hands | a faint grain, like that of planed wood, along the backs of the hands | Amberbloods: "A faint grain, like that of planed wood, runs along the backs of the hands and down the spine, and deepens with age". The freckles are in SC23 and the eyes in EC16. For the old, add ridged skin | Waist-up, hands in view |
| RM11 | Salt Crust | a crust of white salt on the lashes, in the corners of the eyes and along the brow | Brinefolk: "a white crust on the lashes, in the corners of the eyes and along the brow". The fact table says "along the hairline" | Head and shoulders |
| RM12 | Point Pupils | pupils shrunk to a point | Noonfolk, in full sun only: "pupils that shrink to a point in full sun". The veining is in SC26 and SC27, the bare scalp in RH14 | Head and shoulders, in a sunlit scene |
| RM13 | Long Arms and Hands | arms that reach the knee, and long hands with fingers a joint longer than a human's | Boughwalkers: "arms that reach the knee, long hands"; the fact table adds "fingers and toes a joint longer than a human's" | Full figure. Hands: waist-up |
| RM14 | Slit Nostrils and Deep Chest | nostrils that are narrow slits, and a very deep chest on a lean, sleek body | Shoalfolk: "very deep in the chest"; "The nostrils are slits that close at will". The clear inner eyelid shows only in a blink and is left out | Nostrils: head and shoulders. Chest: waist-up |
| RM15 | Forward Ears and Tilted Head | ears a little large and set forward, and the head held at a slight tilt, as if listening | Hushfolk: "The ears are a little large and set forward. A Hushfolk's head is never quite still. It tilts and turns". The eyes are in EC20 | Head and shoulders |
| RM16 | Throat Pulse and Early Age | a pulse that shows in the throat | Swiftborn: "a pulse that can be seen in the throat". The hair is HC21. The age shown must be scaled: "One of twenty-five has the lined face and white brows of a human of seventy" | Head and shoulders |
| RM17 | White Scar Lattice | thin white scars crossing the skin of the face, the neck and the hands, and one crooked, badly healed finger | Flinchless: "The skin is crossed with thin white scars, and a missing fingertip or a crooked, badly healed bone is common". The eyes are in EC21. Where the scars lie and the finger are my addition | Head and shoulders. Finger: waist-up |
| RM18 | Heavy Eyelids | heavy eyelids and a soft, unhurried face | Longsleepers: "Soft-featured, unhurried"; "The eyelids are heavy". The eyes are EC22, EC23 or EC7 by the hour | Head and shoulders |
| RM19 | Keeled Chest and Forearm Down | a deep keeled chest on a tall, narrow frame with almost no flesh, a strong brow, and fine down thinning along the forearms | Eyrie folk: "a deep keeled chest and almost no flesh on the frame"; "set under a strong brow". The down of the scalp is RH16 | Brow: head and shoulders. The rest: waist-up |
| RM20 | Still Face and Jaw Spots | a well-made face entirely without expression, a broad nose with wide nostrils, and a row of small dark spots, like a string of moles, along each side of the jaw and behind the ear | Stillfaces: "It does not smile, frown or lift a brow"; "A row of dark spots, like a string of moles, runs along each side of the jaw and behind the ear" | Head and shoulders |
| RM21 | Hairline Script | a band of fine dark strokes across the brow at the hairline, like a line of close script | Lineminds: "a band of fine dark strokes, like a line of close script". The band is the same for everyone of one line, with one stroke added per generation | Head and shoulders |
| RM22 | Gold Throat Patch | a thumb-sized patch of pale gold skin in the hollow of the throat, and even features that could be a man's or a woman's | Twofold before the Turning: "a small patch of skin, pale gold in the young and deep indigo in the old"; thumb-sized is from the fact table | Head and shoulders, throat bare |
| RM23 | Indigo Throat Patch | a thumb-sized patch of deep indigo skin in the hollow of the throat, and even features that could be a man's or a woman's | Twofold after the Turning | Head and shoulders, throat bare |
| RM24 | Silver Arm-rings | twisted silver rings on the upper arms and wrists, some cut through | Northmen: "wear their wealth as silver arm-rings that are cut up when a payment falls due". Twisted, and where they sit, are my addition. MO11 already names arm rings | Waist-up |
| RM25 | Brow Paste Mark | a small mark of coloured paste on the brow | Rainkeepers: "A mark of coloured paste on the brow shows which god the wearer follows". The colour and the shape should follow the god, which the doc does not list | Head and shoulders |

Races whose marker is wholly in another list and that have no entry here:

- Hoarkin: the blue-grey lips, nails and eyelids are in SC19. The fact table adds "No mist on the breath", which is an absence and cannot be put in a prompt.
- Duskreaders: EC19. Lanternkin: SC32. Bitterbloods: SC34 and EC25.

The human races other than Northmen and Rainkeepers have no marker. Their Appearance paragraphs describe build and face (the broad faces and high cheekbones of the Grass riders, the three builds of the Sunlanders), which no list here supplies (§13 item 8).

---

## 9. What the image-model vendors say about consistency

This section holds only what first-party documentation says. Three guides were read: OpenAI's "Image prompting" cookbook page [OAI-PROMPT], the Black Forest Labs prompting guide for FLUX.2 [BFL-FLUX2] and Google's image generation page for the Gemini API [GOOGLE-IMG].

| What the vendor says | Source | What it means for the components |
|---|---|---|
| Define the character once: appearance, proportions, outfit. Restate the defining details in every later prompt; "same style as before" does not guarantee it | [OAI-PROMPT] | A person's components are stored and pasted whole into every prompt |
| Repeat the full character description (skin tone, hairstyle, clothing, distinctive features) in every prompt of a series | [BFL-FLUX2] | The same four kinds of component this note supplies |
| Be specific: name materials, colours and the visual medium. More specific means more control | [OAI-PROMPT] [GOOGLE-IMG] | Each description names cut, material and colour |
| Put the subject first. The model gives more weight to what comes early. Order: subject, action, style, context | [BFL-FLUX2] | Components come before the setting and the art style |
| A prompt of 30 to 80 words is usually the best length; 80 and more for complex scenes | [BFL-FLUX2] | One or two sentences per component. The longest outfit is 63 words and the longest hairstyle 54, so a full set can pass 100 words. For a head-and-shoulders portrait the "Below" part of an outfit can be left out |
| Negative prompts are not supported; describe what is wanted | [BFL-FLUX2] | The descriptions say what is there ("hair fully hidden under a white veil"), never what is absent |
| Make one reference picture of the character and pass it as input for later pictures. Up to four or five character reference images, by model | [OAI-PROMPT] [GOOGLE-IMG] | The first accepted portrait can serve as reference for later ones. The components still go in the prompt |
| Consistency is not guaranteed; inspect each result | [OAI-PROMPT] | A check is needed whatever the method |

**Design note.** A possible order for one portrait prompt, following the table: age and sex from the Stub, the marker of the race (§8.4), the skin colour, the eye colour, the hair colour with the hairstyle, the outfit, and last the framing and the art style, which are the same for every person. Example with WH4, HC4, SC3 and WO1: "A woman of about forty, fair skin with a ruddy, wind-reddened flush across the cheeks and nose, chestnut brown hair with a reddish cast, two braids wound around the head like a band and pinned flat above the forehead, leaving the neck and ears bare. Fitted gown of undyed grey-brown wool laced at the front over a white linen shift that shows at the neck and wrists, sleeves pushed up." No prompt was run in this pass; the order is untested.

---

## 10. Suggested fit to races and climates

*This section is a suggestion, not a rule. It is my own judgement built on the Appearance text of the existing docs. Where those say too little, the cell says so.*

### 10.1 By climate

The biomes are the readme's. The docs tie no dress to a biome directly, only through the races that live there, so this table follows the climate of each entry's model.

| Climate | Biomes | Women's hair | Men's hair | Women's outfits | Men's outfits |
|---|---|---|---|---|---|
| Cold | Ice & snow, Tundra, Boreal forest | WH2, WH4, WH5 | MH5 | WO2, WO7, WO8 | MO11, WO7, WO8 |
| Temperate | Temperate grassland, Temperate mixed, broadleaf and rain forest | WH1 to WH7, WH19, WH20 | MH2, MH3, MH4, MH5 | WO1, WO3 to WO6, WO25 to WO30 | MO1 to MO9, MO20 |
| Warm coast | Woodland & shrubland | WH8, WH9 | MH1 | WO9, WO10, WO11, WO14 | MO12, MO13 |
| Hot and dry | Hot desert, Thorn scrub, Cold desert | WH10, WH12, WH13 | MH8, MH10 | WO12, WO13, WO14 | MO14, MO15, MO18 |
| Steppe | Cold steppe, Temperate grassland | WH2, WH18 | MH7 | WO15 | MO10, WO15 |
| Savanna | Savanna, Tropical dry forest | WH11 to WH14, WH17 | MH8, MH9 | WO16, WO21, WO22, WO23 | MO16, MO18, WO23 |
| Hot and humid | Tropical rain forest, Mangrove | WH2, WH11, WH14, WH17 | MH1, MH8 | WO16, WO19, WO20, WO21 | MO13, MO16, WO19 |
| River plain | Temperate broadleaf forest, Temperate rain forest (the Canal folk's land) | WH15, WH16 | MH6 | WO17, WO18, WO19 | MO17, MO19 |
| High land | Any belt | | | WO24 | MO3 |

The entries of §8 and §7.2 belong to a race, not to a climate, and are not in this table.

### 10.2 By race

Human peoples are separate races in this project. The table covers all 32 races of `races-basis.md`, because its Appearance paragraphs are the only place that says "what a portrait must show". The default list of Appendix E still has one `human`, with no looks (§11.1 item 1).

"Not stated" means the Appearance paragraph says nothing, so nothing is suggested. The Marker column names the entry of §8.4 and, where the marker of the race is carried by a skin, eye or hair entry, that entry.

| Race | Skin | Hair colour | Hairstyles | Outfits | Eyes | Marker |
|---|---|---|---|---|---|---|
| Northmen | SC1, SC2, SC3 | HC5 to HC10 | WH2, WH4, WH5; MH5, RH1 | WO2; MO11 | EC1, EC2, EC3 | RM24 |
| Midlanders | SC1 to SC5 | HC1 to HC10 | WH1 to WH7; MH2, MH3, MH4 | WO1, WO3 to WO6; MO1 to MO9; RO1 | EC1 to EC8 | None |
| Saltroad folk | SC8, SC9, SC13 | HC1, HC2, HC3 | WH10, WH13; MH8, MH10 | WO12, WO13, WO14; MO14, MO15, MO18 | EC6; now and then EC3, EC8 | None |
| Sunlanders | SC11, SC12 | HC1 | WH11 to WH14; MH8, MH9; RH2, RH3 | WO21, WO22, WO23; MO18; RO2 | Not stated | None |
| Grass riders | SC6, SC7 | HC1 | WH2, WH18; MH7 | WO15; MO10 | EC6, EC7 | None |
| Rainkeepers | SC8, SC10, SC11 | HC1, HC2 | WH2, WH17; RH4 | WO16; MO16 | EC6, EC7 | RM25 |
| Canal folk | SC4, SC6 | HC1 | WH15; MH6 | WO17, WO19; MO17, MO19 | EC6, EC7 | None |
| Elves | SC1, SC2 in cold woods; SC10, SC11 in warm forests; oak woods not stated | HC15 in cold woods; HC3, HC4 in oak woods; HC1 in warm forests | WH1, WH20; RH5 | RO3; WO26 in the same colours | EC3 in oak woods; EC9 in warm forests; cold woods not stated | RM1 |
| Dwarves | SC15, SC16 | Not stated | RH6 | RO4; MO2, MO3 | Not stated | RM2; the beard is in RH6 |
| Halflings | Not stated | HC3, HC5, HC6 | RH7 | RO5 | Not stated | RM3 |
| Gnomes | Not stated | HC17; HC14 for the old | RH8, RH9 | RO6 | EC2, EC5 | RM4; the spectacles are in RO6 |
| Orcs | SC14, SC17, SC18 | HC1 | RH10, RH11 | RO7; MO9, MO10 in mismatched pieces | EC10, EC11 | RM5 |
| Hoarkin | SC19 | HC16, HC17 | RH12 | RO8 | EC13 | In SC19 |
| Tinefolk | SC20 | HC1, HC2, HC3 | Style not stated; WH2 and MH5 leave the brow clear for the antlers | RO9 | EC9 in summer; EC12 in the dark months | RM6 in summer; RM7 in autumn; none after midwinter |
| Hearthbloods | SC21 | HC1, HC18 | MH1, WH19 | RO10 | EC14 | In SC21; RM8 in a cold scene |
| Widegaze | SC22 | HC3, HC5, HC6 | RH13 | RO11 | EC15 | RM9; the pupil is in EC15 |
| Amberbloods | SC23 | HC4, HC9, HC19 | Not stated | Not stated | EC16 | In SC23 and EC16; RM10 |
| Brinefolk | SC24 | HC20 | Not stated | RO12 | EC17, EC18 | RM11; the eye paint is in RO12 |
| Duskreaders | SC25 | Not stated | Not stated | RO13, RO14 | EC19 | In EC19 |
| Noonfolk | SC26, SC27 | None | RH14 | RO15 | EC3, EC9 | In SC26, SC27 and RH14; RM12 in full sun |
| Boughwalkers | SC11, SC13 | HC1 | RH15 | RO16 | EC4 | RM13 |
| Shoalfolk | SC28, SC29 | HC1 | MH1, WH19, WH11 with shell | RO17 | EC6, EC7 | RM14 |
| Hushfolk | SC30 | HC15, HC17 | Not stated | RO18 | EC20 | In EC20; RM15 |
| Swiftborn | SC5, SC10, SC14 | HC21 | WH2, for men too | Not stated | Not stated | In HC21; RM16 |
| Flinchless | Any of §7.1 | Any of HC1 to HC14 | Not stated | Not stated | EC21 | In EC21; RM17 |
| Longsleepers | SC31 | Not stated | Not stated | RO19 | EC22 on waking; EC23 by afternoon; EC7 before sleep | In the eyes; RM18 |
| Eyrie folk | Not stated | HC16, HC17, HC22, HC23 | RH16 | RO20 | EC6, EC7, EC8 | In RH16; RM19 |
| Lanternkin | SC32 | HC16 | Not stated | RO21 | EC24 | In SC32 |
| Stillfaces | SC33 | HC24 | Not stated | Not stated | EC6, EC7 | RM20 |
| Lineminds | As the human race they live among | As that race | As that race | As that race | As that race | RM21 |
| Twofold | SC10, SC11 | HC1 | RH4 | RO22, RO23 | Not stated | RM22 before the Turning; RM23 after it |
| Bitterbloods | SC34 | HC1 | RH17 | RO24, RO25 | EC25 | In SC34 and EC25 |

- **Goblin** is on in the default list, but it is not a race of `races-basis.md` and no doc describes how a goblin looks. Nothing to map.
- **WH16 and WO18** (the Heian court model) have no race in the docs built on that model. The Canal folk are the nearest; the fit is loose.
- **The 16 human peoples of `human-races.md`** can be matched by way of life only: Floe hunters to WO7, Snow herders to WO8, Grass riders to WO15 and MH7, Terrace folk to WO24, Wellkeepers and Dunefarers to WO13 and MO15, Thornfold folk to WO23 and MH9, Rainkeepers to WO16 and MO16, Clearing folk and Stilt folk to WO20, Ploughlanders and Holt folk to the temperate entries. Their skin and hair must come from their own Appearance column, not from the model culture (§11.1 item 3).
- **Group N of `human-races.md`** was not mapped.
- **Still outside every list:** build, height and the shape of the face, and things carried, such as the bow case and knife of the Grass riders (§13 item 8).

---

## 11. Constraints, conflicts and gaps

### 11.1 Where the existing docs constrained or conflict with the lists

1. **One `human` in the default list, many in the proposals.** The readme and Appendix E hold one `human` race. The decision that human peoples are separate races exists so far in two proposals with different sets: seven in `races-basis.md`, sixteen in `human-races.md`. §10.2 follows `races-basis.md`. Nothing in the default list says how a human looks.
2. **The two proposals describe the same people differently.** Grass riders have "straight and black" hair and "tan to bronze" skin in `races-basis.md`, and "hair black to sandy red" and "ruddy to bronze" skin in `human-races.md`. Rainkeepers differ less. §10.2 uses `races-basis.md`.
3. **Real model or not.** `races-basis.md` builds each human race on a real people and dresses it as that people (veil and turban, wrapped coat, rank badge). `human-races.md` says of its peoples that "none copies the real peoples who share its way of life", and §6.4 item 16 there asks for a stereotype review. The lists in this note are modelled on real cultures, so they fit the first doc directly and the second only by way of life.
4. **Northmen men's hair.** `races-basis.md` says the hair is "worn long and combed daily" and the beards are braided. The National Museum of Denmark and the curators quoted by ScienceNordic describe a long fringe with the hair short and the neck shaved at the back [NATMUS-A] [SCINORD]. MH5 and RH1 follow `races-basis.md`. The museum's style is not in the list.
5. **Halfling waistcoats.** `races-basis.md` dresses Halflings in "bright waistcoats, wide hats and good boots". A waistcoat is later than the period these lists stop at. RO5 follows the doc all the same and is the one entry that breaks the period limit. Either that is accepted for Halflings, or their dress is restated in `races-basis.md`.
6. **Dwarf women are bearded** in `races-basis.md` and outsiders cannot tell the sexes apart. Sex-split lists and a list without facial hair do not fit that race (§13 items 3 and 5). RH6 therefore carries the beard and is for both sexes.
7. **Details the lists do not carry.** The Rainkeepers' mark of coloured paste on the brow, the Northmen's arm-rings as money, and the Orcs' ash-rubbed scars are in the Appearance paragraphs and are not hairstyles, outfits or colours. The same holds for the marker of most new races: antlers, a salt crust, a throat patch, a band of strokes at the hairline. §8.4 now holds them as race markers. Build, height and the shape of the face are still in no list.
8. **Skin can only follow hot and open against cold and shaded.** `human-races.md` §6.4 item 10 notes that the map has no latitude. Section 7 therefore gives tones without tying them to a place; §10 ties them to races.
9. **Nothing contradicts the readme.** The lists add no domain term. "Component" is the task's word, not a term of the readme.
10. **Dress that changes with the person's state.** Several races wear one thing in one state and another in the next, and a fixed component cannot follow that. A Twofold wears a saffron sash before the Turning and an indigo one after it (RO22, RO23). A Duskreader wears a mask at home and a visor by day (RO13, RO14). A Bitterblood bares the patches at home and covers up abroad (RO24, RO25). A Lanternkin wears RO21 only out of doors. The rings of HC21 count the years of a Swiftborn. Markers and eyes change too: the antlers and the eyes of the Tinefolk follow the season (RM6, RM7, EC9, EC12), the eyes of a Longsleeper follow the hour (EC22, EC23, EC7), the throat patch of a Twofold follows age (RM22, RM23), and RM8 and RM12 show only in a cold or a sunlit scene. Which state a portrait shows needs a rule (§13 item 12).
11. **Dress that hides the marker.** RO13 hides the whole face of a Duskreader and RO14 hides the black eyes that are the race's marker. RO21 shades the face of a Lanternkin, whose tracery shows only in shadow. A portrait in that dress does not show what the Appearance paragraph says it must.
12. **One dress for both sexes, with the chest bare.** The paragraphs give the Noonfolk, the Shoalfolk and the Hearthbloods one dress each, which leaves the chest bare (RO15, RO17, RO10). The entries follow the doc. Whether a portrait of a woman of those races is wanted that way is for the user, and an image model may refuse it.
13. **Paragraphs that say too little.** No dress is named for Amberbloods, Swiftborn, Flinchless, Stillfaces and Lineminds, no skin for Halflings, Gnomes and Eyrie folk, and no hair colour for Dwarves, Duskreaders and Longsleepers. Eight races have a hair colour or texture and no style. §8 adds nothing for those gaps.

### 11.2 Entries that rest on late evidence

The look is pre-industrial in each case, but the source that was read dates it late or not at all.

| Entry | What the source dates |
|---|---|
| WH14 Tall Headwrap | [HEADTIE] describes present-day use and gives no date of origin |
| WH18 Winged Horns | Museum pieces of the 19th century; the custom ended in the 1920s and 1930s [KHALKHA] [WMW] |
| WO8 Banded Herder Tunic | [GAKTI] describes the present garment; reindeer hide is given as the older material |
| WO22 Strip-woven Royal Cloth | Cotton strip cloth from the 17th century, silk from the late 18th [KENTE] |
| WO23 Red Herder Cloth | Cotton sheets replaced hides from the 1960s [MAASAI]; the description offers hide as the alternative |

### 11.3 Gaps in this research

- **The Met refused every request** (HTTP 429): the Heilbrunn essays on ancient Greek dress and on kente, and the object page for a tupu pin. The Greek entries rest on Britannica instead. The tupu page is cited from a search excerpt and marked [partially verified].
- **Pages that were missing** (404): Wikipedia "Crespine", "Sideless surcoat" and "Ji (hairpin)"; the Fashion History Timeline page for the crespine; Stability AI's prompt guide. The British Museum object page returned navigation only. Google's skin tone research page returned a title only, so the Monk Skin Tone Scale is not used.
- **Not read:** the British Museum, the Museum of London and the Kyoto Costume Institute; any printed costume history; the vendors' guides beyond the three pages named. The Wikipedia article on Bantu knots gave no history, so that style was dropped.
- **Thin pages.** [SANGTU] says only that it is the topknot of married Korean men. [CHIGNON] does not say where on the head the Greek knot sat; [BRIT-GR] does. [MAASAI] does not describe bead collars or women's shaved heads in the part that was read.
- **Unverified details carried in the lists:** loose hair as the sign of a maiden (WH1); ribbon on the plaits (WH3); the cut of the wig (WH10); the curled-under bob (MH3); ermine (WO5, MO6); the scarf (WO17); the women's drape of the royal cloth (WO22); the bead collars (WO23); leather for the smith's apron (MO2); the miner's hood (MO3); the lamellar helmet (MO10); fishers and sailors in the work tunic (MO13); the official's belt and cap (MO17).
- **Composed or fantasy entries, nine in all:** WH20, WO25, WO26, WO27, WO28, WO29, WO30, MO20, and WH19 apart from its one historical case.
- **Entries with no outside source, 123 in all:** the 22 skin colours of §7.2, hair colours HC16 to HC24, and the 92 entries of §8 (hairstyles, outfits, eye colours and race markers). Each rests on a paragraph of `races-basis.md`. RH5 and the garments of RO3 are fantasy convention. Every cut, material or number that goes beyond the quoted words is named in its row as my addition.
- **Invented:** every description, the names, the hair and skin colour names, and all of §10.
- **No prompt was run.** Whether the descriptions give consistent pictures on any model is untested.

---

## 12. Sources

### 12.1 Museums

- **NATMUS-A** — National Museum of Denmark, "The Viking Age: The people: Appearance". https://en.natmus.dk/historical-knowledge/denmark/prehistoric-period-until-1050-ad/the-viking-age/the-people/appearance/
- **NATMUS-C** — National Museum of Denmark, "The Viking Age: The people: Clothes and jewellery". https://en.natmus.dk/historical-knowledge/denmark/prehistoric-period-until-1050-ad/the-viking-age/the-people/clothes-and-jewellery/
- **KHALKHA** — Asia and Pacific Museum, Warsaw, "Married Mongolian woman's bonnet" (object of the month). https://www.muzeumazji.pl/en/obiekt-miesiaca/59369/
- **WMW** — Weltmuseum Wien, "Schmuckgehänge" (head jewellery of a married Mongolian woman, 19th century, inv. no. 64851). https://www.weltmuseumwien.at/en/objects/schmuckgehaenge-441118
- **VAM** — Ruth Hibbard, "What to wear when you don't have a halo", V&A Blog, 14 June 2015. https://www.vam.ac.uk/blog/museum-life/what-to-wear-when-you-dont-have-a-halo
- **MET-TUPU** — The Metropolitan Museum of Art, "Tupu (pin)", object 309227. https://www.metmuseum.org/art/collection/search/309227 **[partially verified: search excerpt; the site returned 429]**

### 12.2 Encyclopaedia Britannica

- **BRIT-MESO** — "Dress", section "Mesopotamia". https://www.britannica.com/topic/dress-clothing/Mesopotamia
- **BRIT-GR** — "Dress", section "Ancient Greece". https://www.britannica.com/topic/dress-clothing/Ancient-Greece
- **BRIT-AM** — "Dress", section "The pre-Columbian Americas". https://www.britannica.com/topic/dress-clothing/The-pre-Columbian-Americas
- **BRIT-ME** — "Dress", sections "The Middle East from the 6th century" and "The Byzantine Empire". https://www.britannica.com/topic/dress-clothing/The-Middle-East-from-the-6th-century
- **BRIT-MED** — "Dress", section "Medieval Europe". https://www.britannica.com/topic/dress-clothing/Medieval-Europe
- **BRIT-EAST** — "Dress", section "The history of Eastern dress" (China). https://www.britannica.com/topic/dress-clothing/The-history-of-Eastern-dress
- **BRIT-JKS** — "Dress", sections "Japan", "Korea" and "South Asia". https://www.britannica.com/topic/dress-clothing/Japan
- **BRIT-HAIR** — "Hairdressing". https://www.britannica.com/art/hairdressing

### 12.3 Wikipedia: hair

- **BRAID** — "Braid (hairstyle)". https://en.wikipedia.org/wiki/Braid_(hairstyle)
- **CORNROWS** — "Cornrows". https://en.wikipedia.org/wiki/Cornrows
- **OTJIZE** — "Otjize". https://en.wikipedia.org/wiki/Otjize
- **HEADTIE** — "Head tie". https://en.wikipedia.org/wiki/Head_tie
- **WIMPLE** — "Wimple". https://en.wikipedia.org/wiki/Wimple
- **CAUL** — "Caul (headgear)". https://en.wikipedia.org/wiki/Caul_(headgear)
- **ROMAN-HAIR** — "Roman hairstyles". https://en.wikipedia.org/wiki/Roman_hairstyles
- **CHIGNON** — "Chignon (hairstyle)". https://en.wikipedia.org/wiki/Chignon_(hairstyle)
- **GAJRA** — "Gajra". https://en.wikipedia.org/wiki/Gajra
- **HAIRPIN** — "Hairpin". https://en.wikipedia.org/wiki/Hairpin
- **BUYAO** — "Buyao". https://en.wikipedia.org/wiki/Buyao
- **GUAN** — "Guan (headwear)". https://en.wikipedia.org/wiki/Guan_(headwear)
- **SANGTU** — "Sangtu". https://en.wikipedia.org/wiki/Sangtu
- **TONSURE** — "Tonsure". https://en.wikipedia.org/wiki/Tonsure
- **BOWL** — "Bowl cut". https://en.wikipedia.org/wiki/Bowl_cut
- **QUEUE** — "Queue (hairstyle)" (read to character 100,000 of 162,000). https://en.wikipedia.org/wiki/Queue_(hairstyle)
- **DREAD** — "Dreadlocks" (read to character 100,000 of 185,000). https://en.wikipedia.org/wiki/Dreadlocks

### 12.4 Wikipedia: dress

- **EUR-1100** — "1100–1200 in European fashion". https://en.wikipedia.org/wiki/1100%E2%80%931200_in_European_fashion
- **EUR-1300** — "1300–1400 in European fashion". https://en.wikipedia.org/wiki/1300%E2%80%931400_in_European_fashion
- **ENG-MED** — "English medieval clothing". https://en.wikipedia.org/wiki/English_medieval_clothing
- **KIRTLE** — "Kirtle". https://en.wikipedia.org/wiki/Kirtle
- **SURCOAT** — "Surcoat". https://en.wikipedia.org/wiki/Surcoat
- **HOUPP** — "Houppelande". https://en.wikipedia.org/wiki/Houppelande
- **CHAPERON** — "Chaperon (headgear)". https://en.wikipedia.org/wiki/Chaperon_(headgear)
- **DOUBLET** — "Doublet (clothing)". https://en.wikipedia.org/wiki/Doublet_(clothing)
- **MANTLE** — "Royal mantle". https://en.wikipedia.org/wiki/Royal_mantle
- **HABIT** — "Religious habit" (read to character 100,000 of 116,000). https://en.wikipedia.org/wiki/Religious_habit
- **APRON** — "Apron". https://en.wikipedia.org/wiki/Apron
- **MINER** — "Miner's apron". https://en.wikipedia.org/wiki/Miner%27s_apron
- **JESTER** — "Jester". https://en.wikipedia.org/wiki/Jester
- **HAUBERK** — "Hauberk". https://en.wikipedia.org/wiki/Hauberk
- **GAMBESON** — "Gambeson". https://en.wikipedia.org/wiki/Gambeson
- **KETTLE** — "Kettle hat". https://en.wikipedia.org/wiki/Kettle_hat
- **LAMELLAR** — "Lamellar armour". https://en.wikipedia.org/wiki/Lamellar_armour
- **INUIT** — "Inuit clothing" (read to character 100,000 of 234,000). https://en.wikipedia.org/wiki/Inuit_clothing
- **GAKTI** — "Gákti". https://en.wikipedia.org/wiki/G%C3%A1kti
- **STOLA** — "Stola". https://en.wikipedia.org/wiki/Stola
- **VESTAL** — "Vestal Virgin" (read to character 100,000 of 123,000). https://en.wikipedia.org/wiki/Vestal_Virgin
- **TOGA** — "Toga" (read to character 100,000 of 132,000). https://en.wikipedia.org/wiki/Toga
- **EXOMIS** — "Exomis". https://en.wikipedia.org/wiki/Exomis
- **BYZ** — "Byzantine dress" (read to character 100,000 of 111,000). https://en.wikipedia.org/wiki/Byzantine_dress
- **EGYPT** — "Clothing in ancient Egypt". https://en.wikipedia.org/wiki/Clothing_in_ancient_Egypt
- **MELHFA** — "Melhfa". https://en.wikipedia.org/wiki/Melhfa
- **TAGELMUST** — "Tagelmust". https://en.wikipedia.org/wiki/Tagelmust
- **THAWB** — "Thawb". https://en.wikipedia.org/wiki/Thawb
- **KAFTAN** — "Kaftan" (read to character 100,000 of 139,000). https://en.wikipedia.org/wiki/Kaftan
- **DEEL** — "Deel (clothing)". https://en.wikipedia.org/wiki/Deel_(clothing)
- **SARI** — "Sari" (read to character 100,000 of 206,000). https://en.wikipedia.org/wiki/Sari
- **DHOTI** — "Dhoti". https://en.wikipedia.org/wiki/Dhoti
- **RUQUN** — "Ruqun" (read to character 100,000 of 156,000). https://en.wikipedia.org/wiki/Ruqun
- **MANDARIN** — "Mandarin square". https://en.wikipedia.org/wiki/Mandarin_square
- **JUNI** — "Jūnihitoe". https://en.wikipedia.org/wiki/J%C5%ABnihitoe
- **CONICAL** — "Asian conical hat". https://en.wikipedia.org/wiki/Asian_conical_hat
- **KEMBEN** — "Kemben". https://en.wikipedia.org/wiki/Kemben
- **WRAPPER** — "Wrapper (clothing)". https://en.wikipedia.org/wiki/Wrapper_(clothing)
- **KENTE** — "Kente cloth". https://en.wikipedia.org/wiki/Kente_cloth
- **BOUBOU** — "Boubou (clothing)". https://en.wikipedia.org/wiki/Boubou_(clothing)
- **MAASAI** — "Maasai people" (the references beyond character 100,000 hold nothing on dress). https://en.wikipedia.org/wiki/Maasai_people
- **LLICLLA** — "Lliclla". https://en.wikipedia.org/wiki/Lliclla

### 12.5 Hair and skin colour

- **HAIRCOL** — Wikipedia, "Human hair color". https://en.wikipedia.org/wiki/Human_hair_color
- **SKINCOL** — Wikipedia, "Human skin color" (read to character 100,000 of 201,000). https://en.wikipedia.org/wiki/Human_skin_color
- **FITZ** — Wikipedia, "Fitzpatrick scale". https://en.wikipedia.org/wiki/Fitzpatrick_scale

### 12.6 Press

- **SCINORD** — ScienceNordic, "What Vikings really looked like" (quotes curators of the Viking Ship Museum in Roskilde and the Danish National Museum). https://sciencenordic.com/archaeology-denmark-history/what-vikings-really-looked-like/1374457

### 12.7 Image-model vendors (first-party)

- **OAI-PROMPT** — OpenAI, "Image prompting", OpenAI Cookbook (read to character 100,000 of 128,000). https://developers.openai.com/cookbook/examples/multimodal/image-gen-models-prompting-guide
- **BFL-FLUX2** — Black Forest Labs, "Prompting Guide - FLUX.2 [pro] & [max]". https://docs.bfl.ml/guides/prompting_guide_flux2
- **GOOGLE-IMG** — Google, "Image generation", Gemini API documentation (read to character 100,000 of 241,000). https://aistudio.google.com/docs/image-generation

---

## 13. Open questions

1. **Where a race marker lives.** §7.2 and §8 now give the races of `races-basis.md` their skin, hair, dress, eyes and marker. A marker is fixed per race, not picked per person, so it may belong on the race as a line of text and not in a list a person picks from. The marker of several races is split over lists (the Noonfolk's over SC26, RH14 and RM12), which a pick per list can get wrong unless the race restricts each list to its own entries. Goblins have no described looks at all.
2. **Children and the old.** The lists are for adults. Several styles are tied to an age or a rite: braids before the hair-pinning at fifteen, a topknot from the capping at twenty, ochre plaits from puberty, warrior braids until elderhood. Whether a person's hairstyle changes with the age in the Stub, and what a child wears, is open.
3. **Facial hair.** MH5, MH10, RH1 and RH6 include a beard and the others say nothing. A beard changes a face more than a haircut does. It is probably its own component, and for Dwarves it applies to both sexes.
4. **Headwear as its own component.** Five women's hairstyles are coverings or include one (WH6, WH7, WH10, WH14, WH18) and many outfits name a hat, hood, veil or crown. A hairstyle and an outfit picked independently can clash (WH14 with the straw hat of WO19) or repeat each other (WH6 with WO6). Either headwear is split out, or each outfit lists the hairstyles it allows.
5. **Sex-split lists for every race.** Eleven outfits of §4 and §5 are worn by both sexes, and none of the 92 entries of §8 is split by sex, because the paragraphs of the classic and new races do not split them. Dwarves of the two sexes look alike, and a Twofold changes sex at about thirty (`races-basis.md`). One list per race with a sex column may fit better than two lists.
6. **Hair texture.** Straight, wavy, curly and coiled hair is a property of the person, and it limits the hairstyles: WH11 and WH16 do not suit the same hair. The lists do not carry it.
7. **Age on skin and hair.** Greying is given as four entries. It could instead be a modifier on a base colour, drawn from the age in the Stub. Skin has no entry for age at all.
8. **What else a portrait needs.** Build, height and the shape of the face are in the Appearance paragraphs of `races-basis.md` and in no list here. So are the things a race carries, such as a bow case, and the eye colour of five races is not stated at all (§8.3).
9. **Derived or authored.** The readme makes the portrait authored and the Stub derived. A pick from these lists is a pure function of the person if it goes through `Hashing` with a purpose number, which would make the components derived data in or beside the Stub. If an LLM picks them, they are authored. This decides where they are stored and is for the user.
10. **Role and rank.** The "Worn by" column is free text. The Stub has a role, but the repo has no list of roles and a person has no rank or wealth. An outfit cannot be picked by rule until those exist. The Vocations of `races-basis.md` are the nearest list.
11. **Material and colour as a separate pick.** Rank shows in cloth and dye more than in cut (§1.3 finding 3). An outfit could take its dye and fibre from a second pick, tied to the rank of the person or to the dyes and fibres the region has, which the readme already lists as resource variants.
12. **One outfit or several.** A person may need an everyday outfit and a feast-day one, and a cold-season one in a cold region. The lists give one look each. For some races the dress follows a state of the person: age, home or abroad, indoors or out (§11.1 item 10).
13. **Which lists a race draws from.** §10 is a suggestion. The docs give no rule for it, and two of them disagree on whether a human race should look like a real people (§11.1 item 3). A rule that mixes models on purpose, such as a steppe coat with a northern hairstyle, would answer the stereotype concern of `human-races.md` §6.4 item 16, at the cost of the recognisable types of `races-basis.md`.
14. **Framing and art style.** A fixed style line for every portrait (medium, lighting, framing, background) probably matters as much for consistency as the components do. It is not part of this note.
15. **Testing.** No prompt was run. The descriptions should be tried on the model that will be used, and the ones that drift should be rewritten.
