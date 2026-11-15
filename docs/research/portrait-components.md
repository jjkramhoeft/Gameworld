# Portrait Components: Hairstyles, Outfits, Hair Colours and Skin Colours for Generated Persons

*Research date: 2026-11-15. Question: which fixed lists of **hairstyles**, **outfits**, **hair colours** and **skin colours** can a person be described from, so that every portrait of that person is prompted with the same words? Scope: a pre-industrial fantasy world, roughly antiquity to the early Renaissance, across several cultural models, plus fantasy convention where history has no model. §1 states the method and the findings. §2 and §3 are the hairstyles (20 for women, 10 for men). §4 and §5 are the outfits (30 for women, 20 for men). §6 and §7 are the hair colours (25) and the skin colours (35). §8 is the lists added for the races of `races-basis.md`: hairstyles (17), outfits (27), eye colours (31) and race markers (26). §9 is the accessories (54): 45 drawn from history and nine written for single races, with their fit to the races and their clashes with the other lists. §10 is what the image-model vendors' own documentation says about keeping a character consistent. §11 is a suggested fit to the races and climates of this repo. §12 lists where the existing docs constrained the lists and what could not be sourced. §13 lists the sources. §14 is the open questions.*

**Extended 2026-11-15.** The lists were extended to fit the 32 races of `races-basis.md`: 22 skin colours (§7.2), nine hair colours (HC16 to HC24), 17 hairstyles (§8.1) and 25 outfits (§8.2). Two kinds of component were added after that: 25 eye colours (§8.3) and 25 race markers (§8.4). All of these rest on the Appearance paragraphs of that doc, not on outside sources, and §11.2 now maps every one of its races.

**Extended 2026-11-15 (accessories).** A seventh kind of component was added: 54 accessories (§9), which are necklaces, earrings, bracelets, rings, brooches, crowns, body jewellery and hair jewellery. Forty-five are drawn from history, and each carries a few lines of invented in-world lore. Nine are written for single races of `races-basis.md`. §9.3 fits them to the 32 races and §9.4 lists where they repeat or clash with the older lists. Their sources are in §13.8.

**Extended 2026-11-15 (Catfolk).** On 2026-11-15 the default race list was fixed at 28 races, and `races-basis.md` now describes those 28. The Catfolk are new to that doc: humans with furred cat ears, a furred cat tail and the slit pupil of a cat. They were added here with one skin colour (SC35), one hair colour (HC25), two outfits (RO26, RO27), six eye colours (EC26 to EC31) and one race marker (RM26), and with a row in §9.3, §9.4 and §11.2. The counts in this note include them. Five races of the first version of that doc were left out of the list: Tinefolk, Hushfolk, Longsleepers, Eyrie folk and Lineminds. Their entries are kept here, and the words quoted for them are those of the first version, which the doc no longer holds (§12.1 item 22).

**Summary.** The note gives six lists of components drawn from history, and extensions of them for the races of `races-basis.md`. Every hairstyle and outfit has a short stable name, a description written to be pasted into an image prompt, and a line on who wears it: rank, occupation, age or marital status, climate and occasion. The entries are spread over rank (beggar to ruler), occupation, climate and cultural model (north and west European, Mediterranean, Near Eastern, steppe, South Asian, East Asian, sub-Saharan African, Arctic and Andean), so that many different races can draw on them. Seventy-one of the 80 hairstyle and outfit entries of §2 to §5 have a historical model with a cited source. Nine are fantasy convention or composed types and are marked as such. The 101 entries of §8 have no historical model: each quotes the race it was written for. The existing docs describe looks only in the Appearance paragraphs of `races-basis.md` and the Appearance column of `human-races.md`; §11 maps the lists onto those and says where they give too little to go on. The accessories of §9 came later and are not counted in the figures above: 44 of the 45 in §9.1 have a historical model with a cited source, and one is a composed type.

**How to read the citations**

- Each entry cites the source that backs the garment or hairstyle, by key in square brackets. The keys are listed in §13. Only pages that were opened in this pass are cited.
- **[partially verified]** means the source exists and is cited, but the claim was confirmed only from a search-index excerpt, not by reading the page itself.
- **[unverified]** means general knowledge that was not checked against any source in this pass. It is attached to the single detail it concerns, not to the whole entry.
- **Fantasy convention** means the entry has no historical model and none is claimed. **Composed type** means a plausible working outfit put together from ordinary garments, not a documented costume.
- The sources were read with a fetch tool that returns a summary of the first 100,000 characters of a page. Museum pages: the National Museum of Denmark, the Asia and Pacific Museum in Warsaw, the Weltmuseum Wien and the V&A. Encyclopaedia: eight pages of Britannica. Vendor documentation: OpenAI, Black Forest Labs, Google. Everything else is a Wikipedia article, used for the definition of a named garment or hairstyle. The Met returned 429 on every attempt (§12.3).
- The accessories of §9 were sourced in a later pass with the same tool: three pages of the V&A, one each of the Louvre, the National Museum of Denmark and the Portable Antiquities Scheme, eight pages of Britannica's article on jewellery, and Wikipedia for the named pieces (§13.8).
- Source wording is paraphrased, not quoted. Every **Description** is written fresh for this note. The "Model" column names the real culture and period behind an entry; the description itself names no real nation, because the world is invented.
- Everything in §11, §12.1 and every **Design note** is my own judgement, not a sourced claim.

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
| Metals, gems, pearls and ivory | `readme.md`, "Resource" and the bedrock table | Metal ores (iron, copper, tin, silver, gold), gems (garnet, sapphire, jade, lapis and turquoise among them), pearls, ivory (walrus among them) and amber in a bedrock already exist. The accessories of §9 use those materials. Coral and cowrie shell are not in the readme |
| Fibres and dyes | `readme.md`, "Resource" | Fibre variants (flax, hemp, cotton, wool, silk) and dye variants (indigo, woad, madder, kermes, murex) already exist. The descriptions use those materials. Clothing is a resource group that is only produced |

---

## 1. Overview and method

### 1.1 Scope

A **component** here is one fixed piece of a person's description, picked from a list and then repeated word for word in every prompt for that person. This note supplies seven kinds: hairstyle, outfit, hair colour, skin colour, eye colour, race marker and accessory. The first four are drawn from history and extended for the races of `races-basis.md`; eye colours and race markers exist only as written from that doc (§8.3, §8.4); accessories are jewellery, drawn from history, with nine more written from that doc (§9). The note does not supply faces, builds, facial hair, weapons, tools and other things carried, or the art style of the picture (§14). A skin or eye entry of a race carries that race's marker where the marker is a colouring of the skin or the eye, and two hairstyles of §8.1 carry a beard because the race's paragraph makes the beard the point.

The lists are for adults. They are split by sex because the request asked for that; §14 item 5 asks whether the split holds for every race.

Excluded on purpose: anything industrial or modern (zips, machine-printed cloth, knitwear cut like a modern jumper, tailored suits, clipper cuts, dyed neon hair), and anything later than the early Renaissance in its model. For the accessories the same limit leaves out faceted stones, machine-made chains and clasps, and piercings that have no old model. Eleven entries rest on evidence that is later than their look is likely to be; they are flagged in §12.2.

### 1.2 Method

1. Read what the repo already says about looks (`readme.md`, `map-generation.md` §19 and Appendix E, `races-basis.md`, `human-races.md`, `races.md`).
2. Draw up the cells to cover: rank, occupation, climate, age or marital status, cultural model.
3. Pick named historical garments and hairstyles that fill the cells and that a museum, an encyclopaedia or a sourced article describes. Open the source and check the cut, the materials and who wore it.
4. Write each description fresh: what is seen in a head-and-shoulders to waist-up portrait first, the rest second.
5. Fill the cells that history does not fill (wizard, sorceress, ranger, woman-at-arms) with fantasy convention, marked as such.
6. Read the image-model vendors' own prompt guides for what keeps a character consistent (§10).

### 1.3 Findings

1. **Head covering and hairstyle are one decision in most of the sources.** A married woman of the European Middle Ages covered her hair [WIMPLE] [EUR-1100]; a steppe wife's hair was built into her headdress [KHALKHA]; an adult man of the Chinese model wore his topknot under a cap [GUAN]. Five of the 20 women's hairstyles are in fact coverings or include one. Whether headwear is its own component is the first open question (§14 item 4).
2. **Hair and dress mark age and marriage more often than rank.** Girls braid, wives cover or pin up [EUR-1300] [HAIRPIN]; a belt button says married or unmarried [GAKTI]; the colour of a veil says novice or professed [HABIT]; a warrior's long braids are shaved off when he becomes an elder [MAASAI]. A person's Stub has age and family links, so these can be derived.
3. **Rank shows in material and colour, not in cut.** The same cut is coarse undyed wool on a peasant and dyed, fur-lined cloth on a lord [ENG-MED]; the same wrap is cotton on the poor and silk with gold thread on the rich [SARI]; pins are wood for the poor and gold or ivory for the rich [ROMAN-HAIR]. One outfit entry can therefore serve several ranks if the material and colour are chosen separately (§14 item 11).
4. **Many garments are worn by both sexes with small differences**: the steppe coat [DEEL], the Arctic parka [INUIT], the herder's tunic [GAKTI], the West African wrapper and grand robe [WRAPPER] [BOUBOU], the kaftan [KAFTAN]. They are listed once and named in the note under §5.
5. **The vendors agree on one method**: write the character's description once, in concrete visual terms, and repeat it in full in every prompt; put the subject first; where the model takes reference images, reuse the first good portrait [OAI-PROMPT] [BFL-FLUX2] [GOOGLE-IMG]. That is what fixed components give (§10).
6. **The repo's looks live in two proposals that do not agree.** `races-basis.md` builds each human race on a real people and dresses it as that people. `human-races.md` says of its human peoples that none copies the real peoples who share its way of life. The default list has one `human` with no looks at all (§12.1).

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
| WH14 | Tall Headwrap | Hair hidden under a length of stiff patterned cloth wound and tied into a tall, wide, sculpted wrap that rises well above the head, the ears showing. | Married women, market traders and senior women of hot lands; larger and finer for weddings and feasts. | West Africa: head tie, the large gele [HEADTIE]. The page gives no date for the style (§12.2) |
| WH15 | Pinned High Knot | Hair swept up into a tall knot high on the crown and fixed with long pins of gold, silver or jade, one pin hung with small pendants that sway. | Adult women of river-plain realms after the hair-pinning rite at fifteen; the pendant pin was first for noblewomen only. | China: buns held by hairpins after the *ji li* rite [HAIRPIN]; the *buyao* pendant pin, Han and Tang [BUYAO] |
| WH16 | Trailing Court Hair | Straight black hair never cut, worn loose down the back to the floor, parted in the centre, with shorter locks cut at cheek level to frame the face, and tied once low behind the shoulders. | Court ladies and noblewomen who do no manual work. | Japan, Heian court: *suberakashi* [JUNI] [BRIT-JKS] |
| WH17 | Flowered Braid | Oiled black hair in one long braid down the back, with a string of small white jasmine flowers coiled along its length. | Women of the monsoon lands, any rank; daily for the well-off, feasts and weddings for all. | South Asia: gajra on a braid or bun [GAJRA]; the long braid [BRAID] |
| WH18 | Winged Horns | Hair parted in the centre and pinned up in two broad flat arches, wider than the head, that reach out to the sides and down to the shoulders like horns, held by rows of silver clips, under a cap of silver filigree set with red coral; the ends hang in front in silver sleeves. | Married women of wealthy herding families on the steppe; the silver is the dowry. | Khalkha Mongols, evidence from the 19th century [KHALKHA] [WMW] (§12.2) |
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
| MH5 | Long and Loose | Hair grown past the shoulders and combed back, loose or tied at the nape with a thong, worn with a full beard. | Farmers, raiders and hunters of the cold lands, hermits, men of peoples who hold that hair is strength. | Early Romans, long hair and beards [ROMAN-HAIR]; Turkic men, loose hair [QUEUE]; Viking Age, combs among the commonest finds [NATMUS-A]. See §12.1 item 4 |
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
| WO8 | Banded Herder Tunic | High-collared pullover tunic in one strong colour with bands of contrasting colours at the collar, shoulders and cuffs, the collar embroidered in pewter thread, a fringed scarf at the neck and silver jewellery. Below: a belt set with silver buttons, a flared hem to the knee, hide leggings and boots. | Reindeer herders of tundra and northern forest, both sexes; the men's is shorter. Square belt buttons for the married, round for the unmarried. Hide for work, cloth for feasts. | Sámi gákti [GAKTI] (§12.2) |
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
| WO22 | Strip-woven Royal Cloth | A great cloth made of narrow hand-woven strips sewn edge to edge, silk and cotton, in blocks of gold, green, red and blue geometric pattern, wrapped around the body and over the left shoulder, with heavy gold jewellery. | Kings, queen mothers, chiefs and their courts in hot lands; feasts and audiences. Men wear it like a toga with the right shoulder bare. | Asante and Ewe: kente, cotton from the 17th century, silk from the late 18th [KENTE] [WRAPPER] (§12.2). How women drape it is **[unverified]** |
| WO23 | Red Herder Cloth | A sheet of red cloth or of softened hide rubbed red, wrapped around the body and knotted over one shoulder, a second sheet worn as a cape, stacked flat collars of coloured beads around the neck and metal hoops in stretched earlobes. | Cattle herders of the savanna, women and men; the jewellery shows age and marriage. | Maasai: red wrapped sheets, hides before cotton cloth, jewellery as a sign of status [MAASAI] (§12.2). The bead collars are **[unverified]** |
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

## 6. Hair colours (25)

Natural colours only. Hair colour comes from two pigments: more of the dark one gives brown and black, more of the red one gives red, and little of either gives blond; grey and white hair has lost its pigment [HAIRCOL]. Greying can start in the twenties and ends in grey and then white [HAIRCOL]. The first ten entries are base colours. HC11 to HC14 are for age. HC15 is not an age colour. HC16 to HC25 were added for the races of `races-basis.md`: each takes its colour from the Appearance paragraph of the race named in the note, and no outside source was read for them.

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
| HC25 | Patched Two-colour | hair of two colours, black broken by uneven patches and streaks of ginger | Catfolk: the hair is "often in two of them, streaked or patched". The two colours should be drawn per person from black, brown, sand, ginger, grey and white; this text takes one pair. The single colours are HC1, HC3, HC6, HC9, HC16 and HC17 |

---

## 7. Skin colours (35)

§7.1 holds the twelve tones of human skin. §7.2 holds 23 tones added for the races of `races-basis.md` whose skin §7.1 does not reach.

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

### 7.2 Tones added for the races of `races-basis.md` (23)

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
| SC35 | Smoke Grey | smooth, even grey skin with a cool cast, the colour of wood smoke | Catfolk ("smoke grey"). The comparison is my addition |

Races that need no entry of their own: Northmen, Midlanders, Sunlanders, Grass riders, Rainkeepers, Canal folk, Elves and Twofold are covered by §7.1. Flinchless and Lineminds have skin "of every kind" and "like the human peoples they live among". The skin of Halflings and Gnomes is not stated. Catfolk skin "runs through the colours of a cat's coat". SC35 is the one colour of it that the other rows lack. The rest are SC2 ("cream"), SC4 ("sand"), SC22 ("tawny gold"), SC9 ("russet"), SC8 and SC11 ("warm brown") and SC12 ("near black").

---

## 8. Lists added for the races of `races-basis.md`

§2 to §5 were drawn from history and reach the seven human races well. They reach the five classic races and the twenty new races poorly, because those wear what their Appearance paragraph says and nothing else. §8.1 and §8.2 add an entry for every hairstyle and every dress that an Appearance paragraph names and that §2 to §5 lack. §8.3 and §8.4 add two kinds of component the first pass left out: eye colours and race markers.

- **Source.** Each entry rests on the Appearance paragraph of the race in the last column, and the quoted words are that paragraph's. No outside source was read. Whatever the description adds beyond the quoted words (a cut, a material, a number) is my addition and is named as such.
- **Sex.** Every entry is for either sex unless "Worn by" says otherwise. The paragraphs split dress by sex for no classic or new race.
- **Labels.** RH is a hairstyle and RO an outfit, numbered apart from the lists of §2 to §5. EC is an eye colour and RM a race marker.
- **Catfolk.** RO26, RO27, EC26 to EC31 and RM26 were added later on 2026-11-15, when the Catfolk came into `races-basis.md` with a human look. RO27 rests on the Way of life paragraph, not on the Appearance paragraph.

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

Races whose paragraph gives a hair colour or texture and no style: Midlanders, Saltroad folk, Tinefolk, Amberbloods, Brinefolk, Hushfolk, Lanternkin, Stillfaces and Catfolk, whose hair is "worn at any length". The hair of Duskreaders, Longsleepers, Flinchless and Lineminds is not described.

### 8.2 Outfits (27)

| # | Name | Description | Worn by | From `races-basis.md` |
|---|---|---|---|---|
| RO1 | Guild-cut Town Dress | Hip-length coat of good wool dyed one strong colour, cut close and buttoned to the neck, a soft cap of the same cloth, and a small metal guild badge pinned at the breast. Below: a belt with a purse, hose and leather shoes. | Townsfolk of temperate lands: guild masters, journeymen and their households. | Midlanders: "dyed cloth cut to the fashion of their guild". The cut and the badge are my addition |
| RO2 | Indigo Wrapped Cotton | A long cotton cloth dyed deep indigo, wrapped around the body and thrown over the left shoulder with the right shoulder bare, a band of a second strong colour along its edge. Below: the cloth falls to the calf. | Men and women of hot lands, every day. | Sunlanders: "wrapped cotton in strong dyes, indigo first among them". The drape is my addition, after WO22 |
| RO3 | Woodland Greys and Greens | Long-sleeved tunic of soft wool in dull green under a hooded cloak mottled in grey and brown, closed at the throat with a plain leaf-shaped clasp, with no bright metal or colour anywhere. Below: a narrow belt, close hose and soft boots in the same dull colours. | Elves. | Elves: "greens, greys and browns that are hard to see between trees". The garments are fantasy convention; the paragraph gives the colours only |
| RO4 | Thick Wool and Tool Belt | Heavy tunic of thick dark wool under a sleeveless jerkin of stiff brown leather, thick leather bracers, and a broad belt with a hammer or a pick hung from it. Below: wool trousers and heavy boots. | Dwarves. | Dwarves: "leather and thick wool and are rarely without a tool on the belt". The cut is my addition |
| RO5 | Bright Waistcoat and Wide Hat | Bright waistcoat of yellow or red cloth with brass buttons over a full-sleeved white linen shirt, and a wide-brimmed felt hat. Below: breeches and good polished leather boots. | Halflings; the brighter and finer, the more prosperous. | Halflings: "bright waistcoats, wide hats and good boots". A waistcoat is later than the period limit of this note (§12.1 item 5); the entry follows the doc |
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
| RO26 | Loose Tail-slit Clothes | A loose, light tunic of thin cloth with short sleeves, and loose trousers cut with a slit at the back that the tail passes through. The head is bare. | Catfolk at home and among their own. | Catfolk: "light, loose clothes cut to leave the tail free, and nothing that covers the ears". The garments and the slit are my addition |
| RO27 | Hood and Bound Tail | A long plain coat tied at the waist with a broad cloth sash, the tail wound around the waist and hidden under the sash, and a deep hood drawn up over the head so that it covers the ears. | Catfolk who deal with outsiders, in a court or a merchant house. | Catfolk, Way of life: one "binds the tail under a sash and wears a hood. This is called going wrapped". The coat is my addition. It hides RM26 |

Dress that §4 and §5 already cover: Northmen (WO2, MO11), Midlander peasants and nobility (WO1, MO1; WO3, WO4, MO4, MO8), Saltroad folk (WO13, MO15), Grass riders (WO15), Rainkeepers (WO16, MO16) and Canal folk (MO19 for both sexes, WO19, MO17).

Races whose paragraph names no dress: Amberbloods, Swiftborn, Flinchless, Stillfaces and Lineminds.

### 8.3 Eye colours (31)

Eye colour is picked per person, like hair and skin colour. EC1 to EC8 are ordinary human colours. EC9 to EC12 are plain colours that only a non-human race has. EC13 to EC25 belong to one race each and carry that race's marker where the marker is in the eye. EC26 to EC31 are the Catfolk's and carry their slit pupil.

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
| EC26 | Slit-pupil Amber | bright amber eyes whose pupil is a narrow upright slit, like a cat's | Catfolk: "amber, gold, copper, green or pale blue, brighter than a human's"; "The pupil is a cat's: an upright slit in daylight, wide and round in the dark". Narrow is my addition |
| EC27 | Slit-pupil Gold | bright gold eyes whose pupil is a narrow upright slit, like a cat's | Catfolk |
| EC28 | Slit-pupil Copper | bright copper eyes, a red-gold, whose pupil is a narrow upright slit, like a cat's | Catfolk. Red-gold is my addition |
| EC29 | Slit-pupil Green | bright green eyes whose pupil is a narrow upright slit, like a cat's | Catfolk |
| EC30 | Slit-pupil Pale Blue | bright pale blue eyes whose pupil is a narrow upright slit, like a cat's | Catfolk |
| EC31 | Odd Slit-pupil Eyes | one bright amber eye and one bright pale blue eye, each with a pupil that is a narrow upright slit, like a cat's | Catfolk: "now and then one of each". The two colours should be drawn per person; this text takes one pair |

The eye colour of Sunlanders, Dwarves, Halflings, Swiftborn and Twofold is not stated, nor that of Elves of the cold woods. Lineminds have the eyes of the human race they live among. The six Catfolk entries give the pupil as it is in daylight. In a dark scene it is "wide and round", and the words about the slit should be left out.

### 8.4 Race markers (26)

A marker is what a portrait must show of a race beyond skin, hair, eyes and dress. It is fixed for the race, not picked for the person: every person of the race gets the same text, or one of two texts where the marker changes with age or season. RM1 to RM5 are for the classic races, which have a Looks line and no "visible marker"; the word is extended to them here. RM6 to RM23 are for the new races. RM24 and RM25 are not marks of the body: they are things two human races always wear. RM26 is for the Catfolk, who were added later.

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
| RM26 | Cat Ears and Tail | upright, pointed, furred cat ears set high on the head in place of human ears, the sides of the head smooth under the hair, and a furred cat tail as long as the leg, the fur the same colour as the hair | Catfolk: "upright, pointed and furred, set high on the head. A Catfolk has no other ears, and the sides of the head are smooth under the hair"; the tail is "as long as the leg, furred all over"; "The fur of the ears and the tail matches the hair, sometimes with a darker tip or with rings". The tip and the rings should be drawn per person; this text takes neither. With HC25 the fur is patched as the hair is. The pupil is in EC26 to EC31 | Ears: head and shoulders. Tail: full figure |

Races whose marker is wholly in another list and that have no entry here:

- Hoarkin: the blue-grey lips, nails and eyelids are in SC19. The fact table adds "No mist on the breath", which is an absence and cannot be put in a prompt.
- Duskreaders: EC19. Lanternkin: SC32. Bitterbloods: SC34 and EC25.

The human races other than Northmen and Rainkeepers have no marker. Their Appearance paragraphs describe build and face (the broad faces and high cheekbones of the Grass riders, the three builds of the Sunlanders), which no list here supplies (§14 item 8).

---

## 9. Accessories (54)

An **accessory** is a piece of jewellery worn on the body or in the hair. It is picked per person, like a hairstyle, and it differs from the other components in one way: a person can have none, one or several (§9.5). §9.1 holds 45 accessories drawn from history. §9.2 holds nine written for single races of `races-basis.md`. §9.3 fits the list to the 32 races of the first version of that doc and to the Catfolk, §9.4 lists where an accessory repeats or clashes with an entry of §2 to §8, and §9.5 is a design note on how many a person carries.

- **Kind.** One of eight: necklace, earrings, bracelet, ring, brooch, crown, body jewellery, hair jewellery. A bracelet includes bangles and the ring worn on the upper arm. A crown includes circlets, diadems and wreaths. Body jewellery is whatever sits elsewhere: on the nose, the ankle, a tusk, an antler or the brow, or hung from the belt.
- **Description.** One material is written into each description, so that the text stays fixed. Where the same piece is made cheaper or dearer, the Worn by column names the other material; a prompt for it needs the one word changed (§14 item 11).
- **Worn by.** Sex, then wealth (common, wealthy, or both), then rank, age, marriage or occasion where it matters.
- **Lore.** In-world lore, written for this note and not sourced history. Every Lore cell ends by saying what it is: *Invented*, or the words of `races-basis.md` it builds on, followed by *the rest invented*. It keeps to the four ages of that doc and names kinds of places, never one location. Where a custom was taken from the historical model, the cell says *after the model* and the last column carries the source.
- **Quotation marks.** In §9 double quotation marks hold words of `races-basis.md` and nothing else.
- **Sources.** The keys are in §13.8. Museum pages: the V&A, the Louvre, the National Museum of Denmark and the Portable Antiquities Scheme. Encyclopaedia: eight pages of Britannica's article on jewellery. The rest is Wikipedia, for the definition of a named piece. The Met returned 429 again and the British Museum's object pages returned navigation only (§12.3).

### 9.1 Accessories drawn from history (45)

| # | Name | Kind | Description | Worn by | Lore | Model and source |
|---|---|---|---|---|---|---|
| AC1 | Amber and Glass Bead String | Necklace | A single string of beads worn close under the collarbone: rough honey-coloured amber beads alternating with small glass beads in blue, green and yellow, on a linen thread. | Women and girls of the cold coasts and the temperate farmland; common. A rich woman's string has a few silver beads in it. | A girl gets her first beads from her mother's kin and adds one for each great event of her life, so an old woman's string can be read like a tally. The amber is picked up on cold beaches after a storm, and it was among the first goods the ships carried south. From `races-basis.md`: "Some traded furs, amber and walrus ivory for it"; the rest invented. | Viking Age Scandinavia: beads of glass, amber, bone and silver, often strung between the two dress brooches; men and women of every layer wore necklaces [NATMUS-C]. Worn as one string at the neck is my addition |
| AC2 | Hammer Amulet | Necklace | A small flat hammer of dark forged iron, as long as a thumb joint, hung head down on a leather thong at the base of the throat. | Men and women of the cold coasts; both: iron for the common, silver for the better-off. | The hammer is the tool of the first Shipwright, who drove the rivets that are the stars. It is given before a first voyage and touched before casting off, and it is buried with the wearer, who should come to the rowing bench with a tool in hand. From `races-basis.md`: "built by the first Shipwright" and "the stars are the rivets"; the rest invented. | Viking Age: about 1,000 hammer pendants are known, most of them plain, in iron or silver [MJOLNIR]; jewellery with a symbolic value [NATMUS-C]. The thong is my addition. In the model it is a god's hammer; the lore gives it to the Shipwright |
| AC3 | Twisted Neck Ring | Necklace | A rigid ring of thick gold rods twisted like a rope, worn close around the base of the neck, with its opening at the front between two round knobs. | Chieftains, war leaders and the women of ruling houses; wealthy. | A neck ring is given by a lord or taken from a beaten enemy and is never bought. Once closed around the neck it stays there, and the wearer is buried in it. Hill lords of the Midlanders wear one to claim descent from the tribes the Realm conquered. From `races-basis.md`: "drawn from many conquered tribes"; the rest invented. | Iron Age Europe: torc, a stiff neck ring of twisted gold or bronze, open at the front and hard to take off; gold marked high rank; at first mostly women, from the 3rd century BCE mostly men; a Roman award to soldiers; silver neck rings again in the Viking Age [TORC] |
| AC4 | Tooth and Bone String | Necklace | A cord of twisted sinew strung with the pierced teeth and claws of animals, with small carved bone beads between them, hanging to the breastbone. | Hunters, trappers, herders and warriors of either sex; common. Orcs of every standing. | Every tooth is from an animal the wearer killed, and the old will ask about each one. An Orc adds the teeth of enemies worth remembering, and a bone carver makes the beads between. A string is laid out with its owner and never inherited. From `races-basis.md`: "hide, bone, sinew and gut all have a use"; the rest invented. | The oldest ornaments were shells, fish teeth, animal bones, tusks and claws [BRIT-JEWEL] [BRIT-JEWEL-HIST]. The string as described is a composed type |
| AC5 | Blue Glazed Bead String | Necklace | Three short strings of small glazed pottery beads in bright blue and blue-green, worn together close around the neck, with one beetle-shaped bead at the centre. | Men, women and children of the hot dry river lands; common: labourers, potters, boatmen. | The beads come out of the same kilns as the pots and are sold by the handful in every river town. Blue is the colour of the flood that the old cities counted their years by. At a burial the string is broken and the beads are shared among the mourners. From `races-basis.md`: "counted years by the river's flood"; the rest invented. | Ancient Egypt: beads of gold, stone or glazed ceramic in rows [BRIT-JEWEL-EG]; pottery beads for the poor [EGYPT]; scarabs of glazed ware, blue or green, strung on a thread [SCARAB]. Three strings is my addition |
| AC6 | Cowrie Shell String | Necklace | A row of small glossy white cowrie shells sewn side by side onto a leather band, worn as a close collar around the neck. | Women and girls of hot lands and warm coasts; common. Diviners of either sex. | Cowries pass as small change in the grassland markets, so a collar is a purse worn in the open, and a mother snaps shells off it to pay the midwife. Diviners throw them to read an answer. The Shoalfolk who gather them think the price a fine joke. Invented. | Cowries were worn as jewellery and charms; in parts of Africa a sign of womanhood, fertility and wealth; money across Africa, South Asia and East Asia [COWRIE] [SHELL-MONEY]. The leather band is my addition |
| AC7 | Red Coral Bead Collar | Necklace | Many strings of polished red coral beads stacked into a high collar that covers the neck from the collarbone to the chin, with longer strings of the same beads falling over the chest. | Kings, queen mothers and title-holders of hot lands, of either sex; wealthy. | Coral comes from a sea the forest kingdoms never see, so every bead counts as the king's and is lent to a title-holder for life. At the holder's death the collar goes back to the palace. A title-holder who comes to council without it has resigned. From `races-basis.md`: "A council of title-holders can depose a king"; the rest invented. | Yoruba and Bini: red coral necklaces, wristlets and anklets mark high rank and are worn by kings and titled chiefs [CORAL]. The page gives no date (§12.2). The high stacked collar is **[unverified]** |
| AC8 | Marriage Thread | Necklace | A cotton thread dyed yellow with turmeric, strung with small black beads and one gold pendant the size of a fingernail, tied close around the neck. | Married women of the monsoon lands; both: the poorest wear the thread and the beads alone, the wealthy a larger pendant. | The groom ties the thread at the wedding in front of both families, with three knots. The shape of the pendant tells the wife's community, as the colour of her cloth does. She wears it until one of the two dies. From `races-basis.md`: "in colours that tell the wearer's community"; the rest invented. | South Asia: mangala sutra, black beads on a black or yellow thread prepared with turmeric, with a pendant that differs by community, tied by the groom at the wedding; at first a plain thread, named in texts of the 4th and 6th centuries CE [MANGALSUTRA]. When it comes off is not in the source |
| AC9 | Silver Amulet Case | Necklace | A flat square case of hammered silver the size of a palm, stamped with lines of script and rows of small triangles, hung on a leather cord at the middle of the chest, over the robe. | Men and women of desert and dry country: guides, herders, traders; both: a sewn leather pouch for the common, silver for those of standing. | Inside is a strip of paper with a few lines of the written law, copied by a scholar for the wearer by name. The case is closed for good, and to open another's is theft of more than silver. A guide who loses a caravan gives up his case to the clan of the dead. From `races-basis.md`: "They follow one unseen god, whose law is written"; the rest invented. | Tuareg: silver amulets made by the smith caste and stamped with designs or verses; men wear necklaces, amulets and rings; silver is preferred and gold held unlucky [TUAREG]. Rolled texts kept in silver cases [VAM-AMULET]; pouches with written prayers [TAWIZ]. The museum case is of about 1800 (§12.2). The square form and the size are **[unverified]** |
| AC10 | Chain of Office | Necklace | A heavy collar of flat gold links, each as wide as two fingers, lying over both shoulders and across the chest, with a badge hanging from it at the breastbone. | Men of rank in temperate kingdoms: a lord's closest followers, knights of an order, officers of a court; wealthy. | A lord hangs the collar on a follower with his own hands, and the badge on it is the lord's. Sending it back ends the service without a word. The kingdoms that claim the Realm copy the links from its old statues. From `races-basis.md`: "keeps a little of its ceremony"; the rest invented. | Europe from the 14th century: livery collar, a heavy collar usually of gold, given by a lord to his closest associates with his badge hanging from it; collars of the knightly orders [LIVERY]. The shape of the links is **[unverified]** |
| AC11 | Ivory Amulet String | Necklace | A thong of seal hide strung with five tiny figures carved from walrus ivory, a seal, a bear, a bird, a fish and a whale, each smaller than a thumbnail, worn at the throat over the coat. | Hunters and their families on ice and tundra coasts, of either sex; common. | A figure takes one bright season to carve, and each stands for a kind of animal the wearer has promised to take only in need. A child is given the first figure by the lodge. At the cold table a Hoarkin sells ivory by the tusk and never a figure off the string. From `races-basis.md`: "Their carving, in walrus ivory, is tiny and very patient"; the rest invented. | Inuit: ornaments carved from bone, ivory and antler; pendants, beads and carved toggles on clothing and belts [INUIT]. The figures and the string are my addition; the part of the page on amulets was not reached |
| AC12 | Plain Hoop Earrings | Earrings | A plain hoop of bronze wire, as wide as a thumbnail, in each earlobe. | Women of most peoples and men of some; both: bronze for the common, silver or gold wire for the wealthy. | The ears are pierced in childhood by a grandmother, with a thorn and a thread. The first hoops are the first thing a person owns that is not a garment, and a pawned pair is the last thing a poor house gives up. Invented. | Hoop earrings of gold, silver and bronze in the Aegean of the 2nd millennium BCE; earrings were common among Turkic nomads and worn by both sexes in Korea [EARRING]. The model of the Northmen wore none [NATMUS-C] |
| AC13 | Pearl Drop Earrings | Earrings | From each earlobe a short gold wire carrying two white pearls that hang side by side and touch as the head moves. | Women of warm-coast cities; wealthy; matrons. | Pearls come up from the tidal shallows one dive at a time, and a pair is priced at a year's rent of a farm. A matron wears them to be heard as well as seen. The divers who sell them still tell of the quota. From `races-basis.md`: "Officials set a quota of pearls on every Shoalfolk village"; the rest invented. | Rome: earrings with stones and pendants; pearls among the stones of rich families [BRIT-JEWEL-ANT]. Two pearls on one wire is **[unverified]** |
| AC14 | Filigree Flower Earrings | Earrings | A flower of gold wirework as wide as two fingers covering each earlobe, set with small pearls around a bead of red coral at its centre. | Women of the monsoon lands; wealthy. Brides of any house, who may borrow a pair. | The earrings are made by a community of goldsmiths and are part of a daughter's portion, weighed and listed before witnesses. A bride from a poor house borrows a pair from the temple for the day and returns it with a measure of rice. From `races-basis.md`: "act as bank, school and granary"; the rest invented. | India: earrings of pearls, filigree, gems and coral in floral designs, known from sculpture of the 2nd century BCE and paintings of the 4th and 5th centuries CE [BRIT-JEWEL-IND]; ear ornaments from ancient times [EARRING] |
| AC15 | Single Ear Ring | Earrings | One thick ring of gold in the left earlobe, with a small bead of blue stone threaded on it. | Men: soldiers, riders, sailors and caravan guards; both: bronze for the common, gold for an officer. | Among riders and sailors the ring is grave money: whoever finds the body may take the ring and owes the dead a burial. A man who has sold his ring is said to have nothing left to lose. Invented. | Soldiers of the Persian Empire are shown with earrings on palace reliefs; earrings were common among Turkic nomads; men wore them in Korea [EARRING]. One ear only, and the bead, are my addition |
| AC16 | Gold Ear Spools | Earrings | A round spool of sheet gold as wide as two fingers set into each stretched earlobe, its flat face turned forward. | Men of the high valleys; both: gold for the nobility, wood or shell for lesser men. | A boy of a ruling line has his ears opened at his coming of age, and the spools are widened at each step of rank, so that the width of a man's ear is his title. Lowlanders call such men the big-ears. Invented. | Inca: men wore gold or silver plugs in stretched lobes as a sign of nobility, and the conquerors' name for them meant big ears; earspools of gold, silver, wood or shell in Mesoamerica [PLUG]. In the Andes ornaments of the nose and the head marked rank [BRIT-JEWEL-AM] |
| AC17 | Bronze Upper-arm Band | Bracelet | A broad band of polished bronze, three fingers wide, worn tight around the bare upper arm above the elbow. | Men: warriors, wrestlers, wardens and guards; both: bronze for the common, gold for a chief. | A leader gives a band to a man who has stood by him once, and men count each other's arms. In a Hearthblood hall the larder-lord gives the first band after a first full winter of work in the pass. It is worn on the bare arm in any weather. From `races-basis.md`: "must give gifts to keep his following"; the rest invented. | Arm rings in Scandinavian Bronze Age graves, mostly of men; lords may have given them to hold a following [ARMRING]. Armlets on the upper arm in India from the earliest sculpture [BRIT-JEWEL-IND]. The width is my addition |
| AC18 | Stacked Glass Bangles | Bracelet | A dozen thin bangles of coloured glass, red and green, stacked on each wrist from the hand to the middle of the forearm. | Women of the monsoon lands and of hot dry towns; both: glass for the common, gold bangles among the glass for the wealthy. Brides and wives. | A bride is given her bangles by her mother's sisters and wears them day and night; a house knows by their sound where she is. They are broken on a stone on the day she is widowed. Invented. | India: bangles of glass, shell, lac, copper and gold, tied to brides and married women; shown on a figurine of the 3rd millennium BCE [BANGLE] [BRIT-JEWEL-IND]. The number and the colours are my addition. The breaking of the bangles is **[unverified]** as history |
| AC19 | Beast-head Bangle | Bracelet | An open bangle of solid gold on each wrist, its two ends cast as the heads of lions that face each other across the gap, their manes inlaid with blue stone. | Men and women of old river cities and courts of the hot dry lands: governors, envoys, royal kin; wealthy. | The bangle is a court's gift of honour: a ruler closes one on the wrist of an envoy who has pleased him, and the beast tells which court. A merchant house will honour a letter from a man who wears one. From `races-basis.md`: "A merchant's letter of credit is honoured two months' journey away"; the rest invented. | Achaemenid Persia, 5th to 4th century BCE: gold armlets with animal-head ends and inlay, given at court as gifts of honour and shown as tribute on reliefs [OXUS]; a gold bracelet with two inlaid lion heads, lapis lazuli and turquoise, from a princely tomb of about 350 BCE [LOUVRE-SUSA] |
| AC20 | Serpent Armlet | Bracelet | A gold snake coiled three times around the bare upper arm, its head raised and its eyes set with small red stones. | Women of warm-coast cities; wealthy. | The snake is worn against poison and fever, and the port cities say the first was made for a queen who feared her own table. A husband gives one after the safe birth of a first child. Invented. | Hellenistic world, from the 3rd century BCE: serpent bracelets; Roman gold bracelets and armbands in the form of a serpent in a double spiral [BRIT-JEWEL-ANT]. Three coils and the red eyes are my addition |
| AC21 | Open Brass Bangle | Bracelet | A thick open ring of yellow brass around each wrist, shaped like a horseshoe, its two flared ends facing each other. | Women of hot lands, and men, who wear one; both: one or two for the common, a row up each forearm for a rich trader's wife. | The bangles are small money that is worn: a market debt or a diviner's fee is paid by slipping one off the arm. The big payments are still made in cattle. Casters of the forest towns make them in clay moulds, a hundred at a pour. From `races-basis.md`: "cast bronze and brass" and "Cattle are bride-price, blood-price and the measure of a man"; the rest invented. | West Africa: manilla, an open bracelet of brass, bronze or copper with ends that face each other, used as money and worn mostly by women to show a husband's wealth; first recorded in 1505 [MANILLA] (§12.2) |
| AC22 | Gold Signet Ring | Ring | A heavy gold ring on the little finger of the left hand, its flat oval top set with a dark red stone cut with a small device in reverse. | Men of rank and business: lords, officials, judges, merchant bankers; wealthy. A woman who heads a house. | Pressed into wax, the ring binds its owner as his presence would. It goes to the heir at the deathbed, and the ring of a man with no heir is broken with a hammer before witnesses. A stolen ring is a stolen name. From `races-basis.md`: "Trade runs on trust and paper"; the rest invented, the breaking after the model. | Signet ring: a device cut in reverse in carnelian, agate or sardonyx, worn on the little finger; known from Egypt and Greece; seals broken at the holder's death [SEAL]. In Rome the gold ring was first a mark of envoys, nobles and senators [BRIT-JEWEL-ANT] |
| AC23 | Bronze Seal Ring | Ring | A plain bronze ring on the forefinger, its round flat top engraved with a simple mark of crossed lines. | Men and women with something to sign for: craftsmen, small traders, bailiffs, free farmers; common. | The mark is entered with a guild or a moot and is the nearest thing to a written name its owner has. A journeyman is given the ring on the day he is made free of his craft. Lost, it is cried in the market for three days. From `races-basis.md`: "Craft guilds govern the towns"; the rest invented. | Medieval Europe: signets reached the less noble classes from the 13th century [SEAL]; rings of the High and Late Middle Ages were mostly of copper alloy, silver or gold [RING]; the lower ranks wore base metal [VAM-JEWEL]. The mark and the finger are my addition |
| AC24 | Clasped-hands Ring | Ring | A silver ring whose top is formed as two small hands clasped together. | Betrothed and married people of either sex in temperate lands; both: silver for the common, gold for the wealthy. | It is given at the betrothal, hand to hand, before witnesses, and the pair wear matching rings. Sent back, it ends the promise without a word. A widow moves hers to the other hand. Invented. | From ancient Rome, common from the 12th century: fede ring, two clasped hands, for love, betrothal and marriage [FEDE] [RING]. Silver is my addition |
| AC25 | Cabochon Stone Ring | Ring | A gold ring set with one large polished stone, a deep blue sapphire rounded like a drop of water, held high in a plain gold collar. | Nobles, high clergy and rich merchants of either sex; wealthy. | The stone is worn for what it does: blue against poison and false witness, red for land and titles. The ring of a high priest of the Temple is kissed by anyone who asks a favour. Such stones are left by will, by name. Invented, the powers of the stones after the model. | Medieval Europe: gold and gems for royalty and nobility; until the late 14th century gems were polished, not cut, and valued for size and colour; stones were held to protect [VAM-JEWEL]. Rings as talismans and signs of office [BRIT-JEWEL-MED]; a ruby ring held to bring lands and titles if worn on the left hand [BRIT-JEWEL]. The sapphire is my addition |
| AC26 | Scarab Swivel Ring | Ring | A ring of gold wire on which a blue-green glazed beetle the size of a fingertip turns on a pin, its flat underside cut with signs. | Officials, scribes, stewards and householders of the hot dry river lands, of either sex; both: copper wire for the common, gold for rank. | Turned over and pressed into wet clay, the beetle seals a jar, a door or a letter. An official's beetle carries his office, and when he is dismissed its face is ground flat. The river towns bury a man with his. Invented. | Ancient Egypt: scarab seals of glazed ware or stone, blue or green, drilled through and set in swivel rings from the late Old Kingdom, with the names and titles of officials [SCARAB] [RING] |
| AC27 | Archer's Thumb Ring | Ring | A broad ring of pale polished horn on the right thumb, with a flat lip that covers the pad of the thumb. | Mounted archers and hunters of the steppe, men and the women who shoot; both: horn or bone for the common, green jade for a commander. | A child gets a ring on the day they can draw a grown bow, cut to that one thumb, and a ring is never lent. A khan's officers are known by green stone. A dead archer's ring is tied to the bow case that hangs in the tent. From `races-basis.md`: "can shoot from the saddle at a gallop" and "under officers chosen for merit"; the rest invented. | Asia: the archer's thumb ring, worn on the right thumb for the thumb draw, of horn, bone, leather or stone; jade rings marked military rank in ancient China [THUMB]. Its use by the Mongols is not in the part of the page that was read **[unverified]** |
| AC28 | Bow Fibula | Brooch | A bronze brooch shaped like a small bow, with a coiled spring at one end and a catch at the other, like a large safety pin, fastening the cloak at the right shoulder. | Men and women of the warm coasts and the old provinces; both: bronze or iron for the common, silver for the well-off. Soldiers. | They are sold by the dozen at every market on an old Realm road. Soldiers lose them and ploughmen find them. Custom lets the finder keep one that turns up in a furrow, and asks for one found by a boundary stone to be handed to the lord. From `races-basis.md`: "the boundary stones"; the rest invented. | Europe and the Mediterranean, late Bronze Age to the early Middle Ages: fibula, on the principle of the safety pin, with bow, spring, pin and catch, mostly of bronze or iron, worn by men and women, typically at the right shoulder [FIBULA]; bow brooches of the early Roman period [PAS-BROOCH] |
| AC29 | Ring Brooch | Brooch | A small ring of silver as wide as two fingers with a pin across it, closing the neck opening of the gown, the ring engraved with a short line of letters. | Men and women of temperate lands, of every rank; both: copper alloy or pewter for the common, gold set with stones for the wealthy. | The usual gift between lovers: the letters say something only the giver and the wearer need to understand. Pinning it on is the answer. A brooch given back unpinned is a refusal that nobody else need hear of. Invented, after the model. | Europe, 13th to 15th century: ring brooch closing a garment at the neck, worn by both sexes, with inscriptions of love, friendship and faith; given as a token of love [BROOCH] [VAM-RINGBROOCH]. Base metal for the lower ranks [VAM-JEWEL]; the pin sits in a constriction of the frame [PAS-BROOCH] |
| AC30 | Penannular Cloak Brooch | Brooch | A ring of silver as wide as a palm, open at one point between two flat round ends, with a pin twice as long as the ring is wide thrust through the cloak at the right shoulder and pointing upward. | Men of rank of the cold coasts and the hill country: chieftains, law-speakers, poets; both: bronze or iron and half the size for the common. Women wear it on the breast. | The width of the ring is the rank of the wearer. A leader gives one to a poet for a praise poem that is still said a year later. The law of the assembly says how far the pin may stand out before its wearer pays for a neighbour's scratch. From `races-basis.md`: "a good poet eats at the high table"; the rest invented, the pin law after the model. | Ireland and Scotland about 700 to 900, and the Viking Age, when most were of silver: penannular brooch, a ring with a gap and a long pin; men at the shoulder, women on the breast, pin pointing up; a law on how far the pin may stand out; plain ones of bronze or iron [CELTIC-BROOCH] |
| AC31 | Garnet Disc Brooch | Brooch | A round gold brooch as wide as three fingers, its whole face covered with small flat dark-red garnets set in a pattern of gold cells, worn at the centre of the chest. | Women of temperate and cold lands: the wives of lords and great farmers; wealthy. | Garnet work dates from the first generations after the Ashen Years, when a lord had gold from the fallen Realm and nobody to tell him what his wife might wear. The brooches are heirlooms now. Dwarves buy broken ones for the stones. From `races-basis.md`: "a lord with a wooden tower"; the rest invented. | England and the Continent, 5th to 7th century: circular brooches set with dark red garnets in cells; the jewelled disc brooches of Kent [BROOCH]. Plate brooches come mostly from women's graves [FIBULA]. Where it sat is **[unverified]** |
| AC32 | Pilgrim Badge | Brooch | A thin badge of grey cast pewter the size of a large coin, showing a small temple tower in a round frame, pinned to the front of the hood or the hat. | People of temperate lands who have walked to a shrine, of either sex: peasants, craftsmen, widows; common. | Every great temple sells its own badge at the gate, cast by the thousand. Three or four on one hat mark a person who has seen the world, or the walking part of it. A badge is buried with the pilgrim who earned it. From `races-basis.md`: "compete in the height of their temple towers"; the rest invented. | Europe, 14th and 15th century: pilgrim badges of lead alloy, cast cheaply in moulds, pinned or sewn to the hat or the outer clothing; within reach of an ordinary pilgrim; a great shrine sold more than 100,000 in a year [PILGRIM]. Pewter is my word for the alloy of lead and tin, and the tower is my addition |
| AC33 | Chained Triangle Brooches | Brooch | Two large triangular brooches of silver, each as long as a hand, pinned point down at the shoulders to hold the draped dress, and joined across the chest by a silver chain hung with red coral beads. | Women of desert, oasis and dry highland, from the wedding on; both: thin plain silver for the common, heavy enamelled silver for the wealthy. | The silver is given at the wedding and is the woman's own, whatever becomes of the marriage. In a bad year she sells it a link at a time, and buys the links back before she buys anything else. A daughter inherits the pair. Invented, after the model. | Maghreb: triangular silver fibulae worn in pairs and joined by a chain, with coral, amber and enamel; a wedding gift, a woman's own property and her savings. Fibulae are old there, but most surviving pieces are of the late 19th or early 20th century [BERBER] (§12.2). Worn in pairs to hold unsewn draped dress [CELTIC-BROOCH] |
| AC34 | Gold Stag Plaque | Brooch | A flat pierced plaque of gold the size of a palm in the shape of a running stag, its legs folded under it and its antlers laid along its back, fixed to the breast of the coat. | Riders of the steppe of either sex: clan heads and a khan's officers; wealthy. | The beast is the clan's. Plaques for sworn brothers are cut from one sheet of gold, so two that fit edge to edge prove an oath. A khan who breaks up the clans has the old plaques melted. From `races-basis.md`: "each clan at feud or at marriage with the next" and "He breaks up the old clans"; the rest invented. | Scythians of the steppe, 5th to 4th century BCE: shaped and pierced gold plaques of running or fighting animals in the animal style [BRIT-JEWEL-IND]. The pose and where it is worn are **[unverified]** |
| AC35 | Plain Brow Circlet | Crown | A narrow band of dull silver, a finger wide, worn level around the head across the brow, over loose hair. | Young nobles and heirs of either sex, elven lords and ladies; wealthy. | A circlet says that the wearer is of a ruling house and does not rule yet; it is put off on the day a crown is put on. Elves leave theirs unpolished, so that it does not flash among the trunks. From `races-basis.md`: "hard to see between trees"; the rest invented. | Europe, 14th century: a circlet on uncovered hair [EUR-1300]; a circlet is a plain open band, the word used much as diadem is [CIRCLET]. On elves and princes it is fantasy convention. Dull silver is my addition |
| AC36 | White Cloth Diadem | Crown | A white silk ribbon two fingers wide tied around the head across the brow and knotted at the back, its two fringed ends hanging to the shoulders. | Men: kings and ruling princes of warm-coast and hot dry realms; wealthy. | The eldest of the council ties it on before the people, and whoever unties it has unmade the king. It costs less than any crown, which is the point: the king is the knot. A dead king's ribbon is burned. Invented. | Persian and Hellenistic kings: the diadem, a white silk ribbon ending in a knot and two fringed strips, the sign of kingship [DIADEM] |
| AC37 | Oak-leaf Wreath | Crown | A wreath of fresh green oak leaves with a few acorns, bound on a ring of twig and worn level on the head. | Men: soldiers of any rank honoured for saving a life in battle; common. | A custom kept from the Realm. The wreath is given by the one whose life was saved, in front of the whole line. The leaves wither in a week, and the right to a seat at any feast of that company lasts for life. From `races-basis.md`: "keeps a little of its ceremony"; the rest invented. | Rome: the civic crown of oak leaves, given to a soldier of any rank who saved a citizen's life in battle [WREATH]. The acorns and the twig ring are my addition |
| AC38 | Beaded Fringe Crown | Crown | A tall cone-shaped crown covered all over in small beads of blue, white and red, with a beaded bird at its tip and a fringe of bead strings hanging from its rim to the chin, veiling the face. | Men: sacred kings of hot lands; wealthy. | Behind the fringe the king is every king of his line, and nobody has seen his face since he was crowned. A council that deposes him sends for the crown and leaves the man. Only a smith may mend it. From `races-basis.md`: "Kingship is sacred but not absolute"; the rest invented. | Yoruba: the king's crown, a cone covered in beadwork with birds on top and a veil of beads over the face; the veil makes the wearer the dynasty. The dated examples are of the 19th century [OBA-CROWN] (§12.2) |
| AC39 | Phoenix Crown | Crown | A tall crown of gold wire covering the whole top of the head, set with gold dragons and with phoenixes inlaid in bright blue feather, with pearls and red stones, and a string of pearls hanging at each temple. | Women: empresses, princesses and the wives of high officials of river-plain realms; wealthy. A bride wears a plain copy for one day. | The number of phoenixes is the rank, and any clerk can count it. In the Scattered Age no governor's wife wears nine: nine is for the empress of the emperor to come. From `races-basis.md`: "in trust for the emperor to come"; the rest invented. | China, Sui to Ming, most of all Ming: fengguan, with phoenixes of kingfisher feather, gold dragons, pearls and gemstones; the number of dragons and phoenixes gives the rank; worn by brides too [FENGGUAN]. Phoenix headdresses in Song texts [BRIT-JEWEL-EAST] |
| AC40 | Gold Nose Ring | Body jewellery | A thin gold ring as wide as a thumbnail through the left nostril, with one small pearl threaded on it. | Married women of hot dry lands and monsoon lands; both: a silver ring for the common, gold for the wealthy. | The husband's mother puts it in at the wedding feast, and while the husband lives it stays. A wife who takes it out in front of his kin has left him. Invented. | Middle East, for more than 4,000 years; South Asia from about the 10th century and common by the 15th; a sign of a married woman [NOSE]. Worn in the left nostril in India [BRIT-JEWEL-IND]. The pearl is my addition |
| AC41 | Belled Anklets | Body jewellery | A chain of silver links around each ankle, hung with a row of tiny round bells. | Women and girls of monsoon lands and hot dry lands, dancers; both: silver for most, gold for the highest rank. | A daughter gets her first pair when she can walk, and a house listens for its women. Temple dancers wear a hundred bells on each leg. In a Hushfolk town the sound is an injury, and a trader takes them off at the edge of it. Invented. | India: anklets as chains with small bells, worn by women and by dancers, silver for most and gold kept for the highest rank in some regions; anklets on Egyptian women of every class [ANKLET]. Ankle bracelets in India [BRIT-JEWEL-IND] |
| AC42 | Jade Belt Pendant | Body jewellery | A flat carved disc of white jade the size of a palm, hung from the sash on a knotted red silk cord, with a red silk tassel below it. | Gentry and officials of river-plain realms, men and women; wealthy. | Jade is conduct made visible. The pendant knocks against the sash when its wearer hurries, so a person of standing walks in a way that keeps it quiet. A disgraced official hands it in with his badge. From `races-basis.md`: "Their teaching is about conduct, not gods"; the rest invented. | China: jade ornaments attached to the dress; white jade pieces in hangings of silk and beads under the Ming; white jade the most prized [BRIT-JEWEL-EAST] [JADE]. The disc, the cord and the tassel are **[unverified]** |
| AC43 | Forehead Pendant | Hair jewellery | A fine gold chain laid along the centre parting of the hair and hooked into it at the crown, ending in a small round gold pendant set with a red stone that rests on the middle of the forehead. | Women of the monsoon lands: brides, and wives on feast days; wealthy. | The bride's mother puts it on her on the wedding morning, and it is worn on feast days after that. The stone is said to keep the thoughts cool. A widow gives hers to her eldest daughter. Invented. | South Asia: forehead jewels [BRIT-JEWEL-IND]; maang tikka, hooked into the hair at the parting with the pendant on the forehead, part of a bride's set [TIKKA]. The page gives no early date (§12.2) |
| AC44 | Carved Hair Comb | Hair jewellery | A small curved comb of dark carved wood pushed into the front of the hair knot, so that its carved back stands above the hair. | Women of river-plain realms; both: wood or horn for the common, silver worked in relief for the wealthy. | A mother gives a comb at the hair-pinning, and a woman is laid out with the one she was married in. The carving shows the flower of the month she was born. Invented. | China, Tang and Song: combs among the finds from tombs; comb heads worked in relief [BRIT-JEWEL-EAST]. That the comb stands in the dressed hair is **[unverified]** |
| AC45 | Temple Rings | Hair jewellery | Three rings of bronze wire, each as wide as two fingers, hanging one below another beside each temple from a cloth band tied around the head. | Women and girls of temperate and cold farmland; both: bronze or iron for the common, silver for the wealthy. | The pattern of the rings tells the village a woman was born in, and she keeps it when she marries into another. A girl wears one ring a side and a wife three. They are taken off for mourning. Invented. | Slavic peoples, 8th to 12th century: temple rings hung from a headdress or woven into the braids, mostly of copper alloy or iron, also silver; each tribe had its own form [TEMPLE-RING]. Three rings a side is my addition |

**What a picture must show.** A head-and-shoulders portrait shows the necklaces, the earrings, the crowns, the hair jewellery, AC40 and the brooches worn at the neck or the shoulder. AC17 and AC20 need a bare upper arm in view. The bangles (AC18, AC19, AC21) and the rings (AC22 to AC27) need a waist-up picture with the hands in view, and so does AC42. AC41 needs the full figure. Of §9.2, AC46 and AC51 to AC54 show in a head-and-shoulders portrait, AC47 and AC48 hang at the chest, and AC49 and AC50 are on the hand.

**Sex and wealth.** By sex: 15 of the 45 are for women, nine for men and 21 for either. By wealth: eight are common, 17 wealthy and 20 are made in both a cheap and a dear material. Every kind has an entry that a common person can wear and one for a wealthy person. Every kind but hair jewellery has an entry that a man can wear; what men wear in the hair is already in MH6 (a pin) and RH6 (beard rings).

### 9.2 Accessories written for single races (9)

These have no historical model. Each rests on words of `races-basis.md`, quoted in the Lore cell, and each Lore cell names what was added beyond them. They are for either sex and are not tied to wealth, except where the Worn by column says so.

| # | Name | Kind | Description | Worn by | Lore | From `races-basis.md` |
|---|---|---|---|---|---|---|
| AC46 | Blood-lamp Vial | Necklace | A sealed glass vial as long as a finger joint, holding one drop that gives a steady blue-green glow, hung on a fine chain at the hollow of the throat. | Married Lanternkin. | Each spouse wears the other's drop, so each knows at any hour that the other is alive. A vial that has gone dark is worn for one more year and then given to the lamp hall. From `races-basis.md`: "Vials are exchanged at a wedding and one is filled at every birth", "A drop sealed in glass shines until the one who gave it dies" and "To break a person's lamp is to disown them". That the vial is worn, and the year of mourning, are invented; the doc keeps the vials in the lamp hall. | Lanternkin, Way of life and fact table. The chain and the size are my addition |
| AC47 | Antler Tine Pendant | Necklace | One polished point of antler as long as a finger, capped with a band of copper and hung on a hide thong at the middle of the chest. | Married Tinefolk. | At the wedding each cuts one point from the antlers the other shed that winter, and wears it from then on. It is taken off only in the bare days. A widowed Tinefolk hangs it with the racks of the dead. From `races-basis.md`: "A marriage is sealed by an exchange of that year's antlers", "the bare days, when no oath is sworn" and "The racks of the dead hang in the winter camp". The cut point, the copper band and when it is taken off are invented. | Tinefolk, Way of life |
| AC48 | Signal Mirror Disc | Necklace | A round disc of polished bronze as wide as a palm, bright as a mirror on its face, hung on a leather strap at the middle of the chest. | Widegaze of the families that hold the Watch. | A child of a Watch family is given the mirror on the day they can send and read the whole code. It is used every day and polished by nobody else. The mirror of a person cast out for lying about seeing is scratched across before they go. From `races-basis.md`: "A message passed by mirror flash and smoke from mound to mound", "nobody else has ever learned the code" and "their own crafts are bold and large". That the mirror is worn on the chest, and the customs, are invented. | Widegaze, History and lore, Way of life |
| AC49 | Scented Wrist Cord | Bracelet | A cord of undyed plant fibre knotted at close intervals and wound three times around the left wrist. | Stillfaces. | The cord holds the scent of the wearer's house and is renewed at each visit home; a Stillface far away reads it as others read a letter. To a human it is a piece of string. A cord that has lost its scent is burned. From `races-basis.md`: "Messages are sent on scented cords, which keep for some weeks". That a cord is worn, and what it carries, are invented. | Stillfaces, Way of life |
| AC50 | Grooved Relay Ring | Ring | A broad ring of plain bronze on the forefinger, its outer face cut with fine parallel grooves that run all the way round, one beside another. | Swiftborn of a town with a Long Work, from adulthood at seven. | Each groove is one generation that has held the town's Long Work, and the ring is given at the handing-on. A Swiftborn far from home turns it on the finger to remember that the work is still being built. From `races-basis.md`: "handed on at a ceremony from each generation to the next, with a ring carved for every one". The doc does not say where that ring is carved; the finger ring is my reading and is invented. | Swiftborn, Way of life |
| AC51 | Shaped Salt Crust | Body jewellery | The white salt crust along the brow worked into a row of small even points, like the edge of a saw, from temple to temple. | Brinefolk. | Fashion changes with the season: points one year, a smooth ridge the next. A pan master brushes the brow clean before a weighing, to show a plain face. A mourner lets the crust grow as it will. From `races-basis.md`: "which they brush away or shape into ornament as fashion bids". The shape and the customs are invented. | Brinefolk, Appearance. Use it in place of the brow of RM11 |
| AC52 | Banded Tusks | Body jewellery | A ring of hammered iron around the base of each tusk, and a cap of bright brass over the tip of the left one. | Orcs: the iron rings for proven warriors, the brass cap for a chief. | A ring is hammered on cold, by the smith of the hearth, for each raid a warrior has come back from. The brass cap is a chief's, cut from something taken. A chief who loses the post loses the cap and keeps the rings. From `races-basis.md`: "whatever armour was won" and "A chief is chosen by challenge and kept by success". The doc names no ornament on the tusks; the whole entry is invented. | Orcs, Appearance and Way of life. Fantasy convention. Add it to RM5 |
| AC53 | Antler Tine Rings | Body jewellery | Narrow rings of copper wire and coloured thread wound around the bare polished antlers, three on each. | Tinefolk in autumn, with RM7 only. | The rings go on when the velvet has been rubbed off and come off before the midwinter shedding, so that a shed antler is bare. The colours of the thread are those of the clan's route. From `races-basis.md`: "bare and polished in the autumn, and shed at midwinter". The doc says nothing of ornament on the antlers and nothing against it; the whole entry is invented. | Tinefolk, Appearance |
| AC54 | Shell Braid Beads | Hair jewellery | Small white shells and discs cut from mother-of-pearl threaded onto short braids, so that they lie along each braid from the scalp to the nape. | Shoalfolk who braid the hair. | The shells are taken from the family's own reef, and a diver adds one for each season of deep diving. The loser of a dispute settled by the breath cuts one off. From `races-basis.md`: "cropped short or braided with shell" and "A family owns a reef". The kind of shell, where it sits and the customs are invented. | Shoalfolk, Appearance and Way of life. Compare WH11 |

### 9.3 By race

*This table is a suggestion, like §11. It is my own judgement built on the Appearance paragraph of each race and on what its other parts say of metal, wealth and ornament.*

The four middle columns give the accessories for common women, common men, wealthy women and wealthy men. *None* means that nothing is suggested, and the note says why. *As common* means that the doc names no rank or wealth for the race. A note that begins *None for* bars a kind or an entry for the whole race.

| Race | Common women | Common men | Wealthy women | Wealthy men | Note |
|---|---|---|---|---|---|
| Northmen | AC1, AC2 | AC2 | AC1, AC3 | AC3, AC30 | They "wear their wealth as silver arm-rings", which is RM24 already, so no bracelet is added. None for earrings: the model wore none [NATMUS-C]. WO2 and MO11 already carry brooches (§9.4) |
| Midlanders | AC24, AC28, AC29, AC32, AC45 | AC23, AC24, AC28, AC29, AC32, AC37 | AC24, AC25, AC29, AC31, AC35 | AC3, AC10, AC22, AC24, AC25, AC29, AC35 | "Rank shows in the clothes", so the accessory follows the outfit. No earrings were sourced for this model. A wife's hair is covered (WH6), which hides AC45 |
| Saltroad folk | AC5, AC9, AC12, AC18, AC33 | AC5, AC9, AC15, AC26 | AC13, AC19, AC20, AC33, AC40, AC41 | AC9, AC19, AC22, AC26, AC36 | "Among the desert clans it is the men who veil their faces", and MO15 hides AC15. Silver for the desert clans, whose model holds gold unlucky [TUAREG]; gold in the towns, which sit on a road that carries "salt one way and gold the other" |
| Sunlanders | AC6, AC12, AC21 | AC4, AC17, AC21 | AC7, AC12, AC21 | AC7, AC17, AC38 | The hair already carries ornament: "built up with clay and beads to show age, rank and home" (RH3). Gold for the wealthy: "The gold fields lie in Sunlander country". AC38 is for a king only. AC4 is for "the hunters of the deep forest" |
| Grass riders | AC12, AC27 | AC15, AC27 | AC12, AC34 | AC3, AC15, AC27, AC34 | They "carry a bow case and a knife", and AC27 goes with the bow. None for crowns: both sexes wear "a fur hat". WH18 carries its own silver and coral |
| Rainkeepers | AC8, AC12, AC18, AC41 | AC12 | AC8, AC14, AC18, AC40, AC41, AC43 | AC12, AC17, AC22 | AC43 lies where the brow mark is (RM25); use one of the two. None for the wandering ascetics, "who own nothing". The goldsmith is one of the occupations a Rainkeeper is born into |
| Canal folk | AC44 | None | AC39, AC42, AC44 | AC27, AC42 | The model's jewellery dresses the costume more than the wearer, hairpins and headdresses come before necklaces, bracelets and earrings, and a man's ornament is in his accessories [BRIT-JEWEL-EAST]. Rank is on the robe: "silk in the colour of their grade, with a square badge on the chest". A farmer has "a wide straw hat"; his topknot pin is in MH6 |
| Elves | None | None | AC35 | AC35 | The paragraph gives no rich and poor, only dress "hard to see between trees". RO3 reads that as no bright metal, so AC35 is dull silver and nothing else is suggested. Fantasy convention |
| Dwarves | AC17, AC23 | AC17, AC23 | AC17, AC22, AC25 | AC17, AC22, AC25 | The sexes dress alike: "outsiders often cannot tell the sexes apart". Standing is in the beard, "braided and ringed to show clan, craft and standing" (RH6). None for necklaces: the beard covers the throat and the chest |
| Halflings | AC24, AC29 | AC23, AC24, AC29 | AC24, AC25, AC29 | AC22, AC24, AC29 | They "wear their prosperity on their backs": the cloth comes first, and RO5 has brass buttons already |
| Gnomes | AC23 | AC23 | AC22, AC25 | AC22, AC25 | A maker's design is entered "under a personal mark", which is what a seal ring carries. They are jewellers by trade; what they wear themselves is not stated. None for crowns: the cap of RH9 |
| Orcs | AC4, AC52 | AC4, AC52 | AC3, AC4, AC17, AC52 | AC3, AC4, AC17, AC52 | Wealth is "whatever armour was won", so the pieces of metal are loot. The columns for the sexes are the same: "Chiefs may be women" |
| Hoarkin | AC4, AC11 | AC4, AC11 | As common | As common | "They wear one layer of sealskin", and the string goes over it. No rank is named beyond the lodge |
| Tinefolk | AC4, AC47, AC53 | AC4, AC47, AC53 | As common | As common | "They own no land. They own routes". None for crowns and for AC45: an antler grows "From the brow above each temple". AC53 in autumn only |
| Hearthbloods | AC17 | AC17 | AC3, AC17 | AC3, AC17 | "They wear a kilt and an open vest", so the arms and the throat are bare and there is no cloak to pin. None for brooches. The paragraph gives one dress for both sexes, so the columns are the same. The wealthy here are the larder-lord and the war leader |
| Widegaze | AC48 | AC48 | AC3, AC31, AC48 | AC3, AC30, AC48 | "they buy fine metalwork and needlework, and their own crafts are bold and large". None for earrings, AC43 and AC45: the hair is "tied back hard, so that it never crosses the eyes", and I extend that to anything that hangs beside them |
| Amberbloods | AC4 | AC4 | None | None | Never AC1: their set blood "becomes true amber", and "the trade in blood amber is not quite dead". The paragraph names no dress and no rank, so nothing is suggested for the wealthy |
| Brinefolk | AC12, AC51 | AC12, AC51 | AC12, AC19, AC51 | AC12, AC19, AC22, AC51 | "They grew rich on it". "From the soda that crusts the lake shores they make glass", and a string of glass beads would suit them; no entry is of glass alone |
| Duskreaders | None | None | None | None | The robes, the gloves and the mask or visor hide every place an accessory sits: "They hide the body's heat, and with it everything the body gives away". "Colour means little to them" |
| Noonfolk | AC12, AC21 | AC12, AC21 | As common | As common | "They wear a wrap at the hips, and nothing that shades the back". None for necklaces and for AC17 and AC20, which would shade the veining of "the shoulders, the back and the upper arms". None for hair jewellery: "Noonfolk have no hair at all" |
| Boughwalkers | None | None | None | None | The hair is "braided flat to the skull, so that nothing can catch on it". I extend that to everything that hangs or rings a finger on a people who climb. They wear "belts, pouches and short wraps" |
| Shoalfolk | AC6, AC54 | AC54 | As common | As common | "They wear a loincloth and a knife". "No ground, no lord", so no rank. They sell pearls; that they do not wear them is my reading |
| Hushfolk | None | None | None | None | They "wear nothing that jingles", which bars every bead, bangle, bell, chain and hanging pendant. A single ring would not break the doc, but RO18 already rules out metal, beads and ornament |
| Swiftborn | AC12, AC50 | AC50 | As common | As common | The paragraph names no dress. "No office is held for longer than a season", so no ornament of rank. None for hair jewellery: the hair is worn long so that "an age can be counted off a braid" |
| Flinchless | AC24, AC29 | AC23, AC24, AC29 | AC24, AC29 | AC10, AC22, AC24 | The paragraph names no dress. "They live among Midlanders more than anyone", so they take the Midlander pieces. AC10 is for the Oathkept, who "serve those crowns" |
| Longsleepers | AC24 | AC24 | AC24, AC25 | AC24, AC25 | The robes "serve for day and night alike". None for brooches, crowns and hair jewellery: I read that as nothing pinned or hard that a sleeper would lie on |
| Eyrie folk | AC23 | AC23 | AC22 | AC22 | They "wear nothing loose", and "They cannot carry loads". None for anything that hangs or weighs. None for hair jewellery: down in place of hair |
| Lanternkin | AC46 | AC46 | AC25, AC46 | AC25, AC46 | Out of doors "they wear a hood, long sleeves and gloves", which hide the ears and the hands. AC46 shows best in shadow, as their veins do |
| Stillfaces | AC49 | AC49 | As common | As common | The paragraph names no dress and no rank. None for earrings: the spots run "along each side of the jaw and behind the ear" and must show. Nothing scented: "The one great offence is masking" |
| Lineminds | As the human race they live among | As that race | As that race | As that race | None for AC35, AC36, AC38, AC43 and AC45: each lies across "a band of fine dark strokes" at the hairline (RM21) |
| Twofold | None | None | None | None | "Everyone dresses alike, in wrapped cotton. Only the sash differs". An ornament picked per person would break that, and one that everybody wore would be a marker. A necklace would also cover the patch "In the hollow of the throat" (RM22, RM23) |
| Bitterbloods | AC12 | AC12 | AC12, AC25 | AC12, AC25 | None for necklaces and for AC17 and AC20: the patches of the throat, the shoulders and the outer arms are "worn uncovered with pride at home". Among other peoples a Bitterblood "wears gloves and a coat of wax on the skin", which hides AC25 |
| Catfolk | AC6, AC21 | AC4, AC17 | As common | As common | AC4 is for the hunting bands, and AC17 for the granary wardens and the night watch. The doc names no rank beyond the ward and the band. None for earrings: AC12 to AC16 hang from a human ear, and a Catfolk "has no other ears" than the cat's. None for crowns and for AC45: they wear "nothing that covers the ears", and I extend that to what sits beside them. Going wrapped (RO27) hides the ears altogether |

- **AC16 is not assigned.** No race of `races-basis.md` is built on the model of the high valleys. It goes with WO24, which §11.2 gives to the Terrace folk of `human-races.md`.
- **Seven races have a None.** Duskreaders, Boughwalkers, Hushfolk and Twofold have nothing at all. Elves and Amberbloods have nothing in two columns, and Canal folk nothing for common men.
- **The human peoples of `human-races.md`** can be matched by way of life only, as in §11.2: Floe hunters to AC11, Terrace folk to AC16, Wellkeepers and Dunefarers to AC9 and AC33, Rainkeepers to the entries of that row.

### 9.4 Repeats and clashes with the other lists

The components are picked independently. An accessory can repeat a piece that an outfit, a hairstyle or a marker already names, or it can sit where that entry puts something else. The table lists every entry of §2 to §8 that carries jewellery or covers a place where an accessory sits. A row for one entry gives its name; a row for several gives the labels only. A picker can drop the accessories named in the last two columns.

| Entry | What it carries or covers | Accessories that repeat it | Accessories that clash with it |
|---|---|---|---|
| WH1 Loose with Circlet | A circlet or a wreath of flowers | AC35, AC37 | AC36, AC38, AC39 |
| WH6; WO6 | Veil and wimple over the hair, the ears and the throat | | All earrings; all crowns and hair jewellery; AC1, AC2, AC3, AC6, AC7, AC8, AC46 at the throat |
| WH7 Netted Side Coils | Gold nets set with jewels at the temples, a band across the brow, a veil | AC35 | All crowns; AC43, AC44, AC45; earrings, hidden by the coils |
| WH8 Nape Knot | A band wound twice around the head, long pins | AC36 | AC35, AC44 |
| WH9; RH3; RH10 | A tall crest of curls; hair built up with clay and beads; a stiff crest | AC54 with RH3 | All crowns |
| WH10 Blunt Braided Wig | A wig with a narrow band around the crown | AC35, AC36 | AC43, AC44, AC45 |
| WH11 Beaded Cornrows | Beads or small shells on the braid ends | AC54 | |
| WH13 Shaved Head | Large earrings or a narrow brow band | All earrings; AC35, AC36 | All hair jewellery, for want of hair |
| WH14; WO21 | A tall headwrap, the ears showing; a head wrap | | All crowns and hair jewellery |
| WH15; MH6 | Long pins, one with pendants; a pin through the topknot | AC44, which fits beside the pins | AC39, which takes the place of the pins; AC43 |
| WH17 Flowered Braid | A string of jasmine along the braid | | AC45 |
| WH18 Winged Horns | Silver clips, a cap of silver and coral, silver sleeves | | All crowns and hair jewellery; all earrings, hidden by the arches |
| MH10 Oiled Ringlets | A band around the brow | AC35, AC36 | AC38, AC39 |
| RH1; RH6; MH5; MH10 | A full beard, braided or curled | | Necklaces at the throat (AC1, AC2, AC3, AC5, AC6, AC8), hidden by the beard; AC3 under a ringed beard |
| RH14; MH8; WH13 | No hair, or a shaved head | | All hair jewellery |
| RH15 Flat Skull Braids | Braids sewn flat, no ornament | | All hair jewellery |
| RH16 Scalp Down | Down in place of hair | | All hair jewellery |
| WO2 Strap Dress and Brooches | Two oval brooches, a string of glass and amber beads, a cloak pinned at the chest | AC1; AC28 to AC31 | |
| WO3 Sideless Surcoat | A jewelled hip belt | | AC42 |
| WO4; MO4 | A high standing collar | | Necklaces at the throat; AC29 |
| WO5; MO6 | A gold crown, a jewelled clasp | All crowns; AC28 to AC31 | |
| WO7; RO8 | A hood around the face, a coat closed to the chin | | All earrings, crowns and hair jewellery; AC11 goes over the coat |
| WO8 Banded Herder Tunic | Silver jewellery, a belt with silver buttons | AC29, AC30 | |
| WO9 Pinned Tunic and Wrap | Small brooches along the shoulders | AC28, AC33 | |
| WO10 Priestess Whites | A wool band and ribbons on the head, a veil out of doors | | All crowns and hair jewellery |
| WO11 Jewelled Court Collar | A jewelled collar over both shoulders, a crown with strings of pearls at the temples | All crowns; AC10; AC13 | All necklaces |
| WO12; MO14 | A broad collar of beads; armlets on MO14 | AC5, AC7; AC17 and AC20 with MO14 | All other necklaces |
| WO13 Desert Body Wrap | A cloth drawn over the head as a hood | | All crowns and hair jewellery; earrings, when the cloth is close |
| WO22 Strip-woven Royal Cloth | Heavy gold jewellery, not named | AC7, AC21 fit it | |
| WO23 Red Herder Cloth | Stacked collars of beads, metal hoops in stretched earlobes | AC12; AC6, AC7 | All other necklaces and earrings |
| WO24 Pinned Mountain Dress | Two silver pins at the shoulders and a third at the chest | AC28, AC33 | AC29, AC30, AC31 |
| WO27 Sorceress Robe | A silver brooch at the throat, an amulet on a long chain | All necklaces; all brooches | |
| WO30 Player's Motley | Brass bells at the collar and the sleeves | AC41 | |
| MO8; MO9; MO10; WO28 | A mail hood, a helmet | | All crowns, earrings and hair jewellery; necklaces under mail |
| MO11; RM24 | A round brooch at the right shoulder, twisted silver arm rings | AC28, AC30; AC17 | AC18, AC19, AC21, where the arm rings sit |
| MO15 Veil-turban and Robe | A veil-turban that leaves only the eyes | | All earrings, crowns and hair jewellery; AC40; AC38 |
| MO16; MO18 | A turban; bracelets on MO16 | AC18, AC19, AC21 with MO16 | All crowns |
| MO17 Official's Badge Robe | A gauze cap, a hooped belt | | All crowns; AC42 hangs from a sash, not from this belt |
| Hats and hoods: WO1, WO15, WO19, WO25, WO26, WO29, MO1, MO3, MO4, MO20, RH9, RO5, RO11, RO21 | A kerchief, hat, cap or hood on the head | AC32 is pinned to the hat or hood | All crowns; hair jewellery under the hat |
| RO1 Guild-cut Town Dress | A guild badge pinned at the breast | AC32 | AC31, AC34 |
| RO3 Woodland Greys and Greens | A leaf-shaped clasp, no bright metal | AC28 to AC30 | Every accessory of bright metal; AC35 is dull |
| RO13; RO14 | A mask or a visor, gloves, a robe closed at the throat | | All accessories |
| RO18 Soft Felt | No metal, bead or ornament | | All accessories |
| RO20 Close Quilted Silk | Nothing loose | | Everything that hangs: all necklaces except AC3, and AC13, AC14, AC18, AC41, AC42, AC45 |
| RO21; RO25 | Gloves; a deep hood on RO21 | | All rings; earrings, crowns and hair jewellery with RO21 |
| RO27 Hood and Bound Tail | A deep hood over the head and the ears | | All earrings, crowns and hair jewellery |
| RO10; RO15; RO17 | Bare chest and arms, no cloak | | All brooches. AC17 and AC20 fit these and clash with every outfit that has sleeves |
| RM5 Tusks and Ashed Scars | Tusks | AC52 adds to it | |
| RM6; RM7 | Antlers above the temples | AC53 adds to RM7 | All crowns; AC45; AC53 with RM6 |
| RM11 Salt Crust | A salt crust along the brow | AC51, which replaces its brow | AC35, AC36, AC43 |
| RM20 Still Face and Jaw Spots | Spots along the jaw and behind the ear | | All earrings |
| RM21 Hairline Script | A band of strokes at the hairline | | AC35, AC36, AC38, AC43, AC45 |
| RM22; RM23 | A patch in the hollow of the throat | | All necklaces |
| RM25 Brow Paste Mark | A mark of paste on the brow | | AC43; AC35, AC36 |
| RM26 Cat Ears and Tail | Cat ears high on the head, and no human ear | | All earrings; all crowns; AC45 |
| SC26; SC27; SC34 | Veining or bright patches on the throat, shoulders and arms | | AC3, AC7, AC10, AC17, AC20 |

Three clashes are not with one entry:

- **A face that is hidden.** AC38 veils the face, and with it the eyes, the skin and any marker of the face. It should be used only where the hidden face is the point.
- **Rank that does not match.** A wealthy accessory on WO1, WO29, MO1, WO6 or MO7, or a common one on WO5 or MO6, reads as a mistake. The rank of the outfit should bound the accessory (§14 item 10).
- **Two accessories in one place.** Two necklaces at the throat, two crowns, earrings of two kinds, or AC17 and AC20 on one arm.

### 9.5 How many

**Design note.** The sources do not give a number, and the repo has no wealth or rank on a person (§14 item 10), so none is set here. A plausible rule to try: most common people carry none or one, the wealthy two or three, a ruler or a bride more; at most one accessory per kind, and none that §9.4 bars for the outfit, the hairstyle and the marker already picked. The pick would be seeded like every other choice. Each accessory adds 13 to 40 words to a prompt that can already pass 100 (§10), which is a second reason to keep the number low. Whether a person has any accessory at all is left open in §14 item 16.

---

## 10. What the image-model vendors say about consistency

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

**Design note.** A possible order for one portrait prompt, following the table: age and sex from the Stub, the marker of the race (§8.4), the skin colour, the eye colour, the hair colour with the hairstyle and any crown or hair jewellery (§9), the outfit, the other accessories as one sentence each, and last the framing and the art style, which are the same for every person. Example with WH4, HC4, SC3 and WO1: "A woman of about forty, fair skin with a ruddy, wind-reddened flush across the cheeks and nose, chestnut brown hair with a reddish cast, two braids wound around the head like a band and pinned flat above the forehead, leaving the neck and ears bare. Fitted gown of undyed grey-brown wool laced at the front over a white linen shift that shows at the neck and wrists, sleeves pushed up." No prompt was run in this pass; the order is untested. Where an accessory goes was not tested either. It is put after the outfit because the vendors give more weight to what comes early and an accessory is the smallest thing in the picture; a crown is put with the hair because it sits there. With AC29 the example would end: "A small ring of silver as wide as two fingers with a pin across it, closing the neck opening of the gown, the ring engraved with a short line of letters."

---

## 11. Suggested fit to races and climates

*This section is a suggestion, not a rule. It is my own judgement built on the Appearance text of the existing docs. Where those say too little, the cell says so.*

### 11.1 By climate

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

### 11.2 By race

Human peoples are separate races in this project. The table covers the 32 races of the first version of `races-basis.md` and the Catfolk, because its Appearance paragraphs are the only place that says "what a portrait must show". The default list is now the 28 races that the doc describes (§12.1 item 22).

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
| Catfolk | SC2, SC4, SC8, SC9, SC11, SC12, SC22, SC35 | HC1, HC3, HC6, HC9, HC16, HC17, HC25 | Not stated: "worn at any length" | RO26; RO27 among outsiders | EC26 to EC31 | RM26; the pupil is in EC26 to EC31 |

- **Goblin** was on in the first default list and is in no list now. No doc describes how a goblin looks. Nothing to map.
- **Catfolk hairstyles.** Any entry of §2 and §3 fits that leaves the top of the head clear for the ears. A style that bares the side of the head shows smooth skin where a human ear would be. WH6, WH7, WH10, WH14 and WH18 are coverings or include one, and a Catfolk covers the ears only when going wrapped (RO27).
- **WH16 and WO18** (the Heian court model) have no race in the docs built on that model. The Canal folk are the nearest; the fit is loose.
- **The 16 human peoples of `human-races.md`** can be matched by way of life only: Floe hunters to WO7, Snow herders to WO8, Grass riders to WO15 and MH7, Terrace folk to WO24, Wellkeepers and Dunefarers to WO13 and MO15, Thornfold folk to WO23 and MH9, Rainkeepers to WO16 and MO16, Clearing folk and Stilt folk to WO20, Ploughlanders and Holt folk to the temperate entries. Their skin and hair must come from their own Appearance column, not from the model culture (§12.1 item 3).
- **Group N of `human-races.md`** was not mapped.
- **Accessories** are fitted to the races in §9.3, with a column each for common and wealthy women and men, and are not in this table.
- **Still outside every list:** build, height and the shape of the face, and things carried, such as the bow case and knife of the Grass riders (§14 item 8).

---

## 12. Constraints, conflicts and gaps

### 12.1 Where the existing docs constrained or conflict with the lists

1. **One `human` in the default list, many in the proposals.** The readme and Appendix E hold one `human` race. The decision that human peoples are separate races exists so far in two proposals with different sets: seven in `races-basis.md`, sixteen in `human-races.md`. §11.2 follows `races-basis.md`. Nothing in the default list says how a human looks.
2. **The two proposals describe the same people differently.** Grass riders have "straight and black" hair and "tan to bronze" skin in `races-basis.md`, and "hair black to sandy red" and "ruddy to bronze" skin in `human-races.md`. Rainkeepers differ less. §11.2 uses `races-basis.md`.
3. **Real model or not.** `races-basis.md` builds each human race on a real people and dresses it as that people (veil and turban, wrapped coat, rank badge). `human-races.md` says of its peoples that "none copies the real peoples who share its way of life", and §6.4 item 16 there asks for a stereotype review. The lists in this note are modelled on real cultures, so they fit the first doc directly and the second only by way of life.
4. **Northmen men's hair.** `races-basis.md` says the hair is "worn long and combed daily" and the beards are braided. The National Museum of Denmark and the curators quoted by ScienceNordic describe a long fringe with the hair short and the neck shaved at the back [NATMUS-A] [SCINORD]. MH5 and RH1 follow `races-basis.md`. The museum's style is not in the list.
5. **Halfling waistcoats.** `races-basis.md` dresses Halflings in "bright waistcoats, wide hats and good boots". A waistcoat is later than the period these lists stop at. RO5 follows the doc all the same and is the one entry that breaks the period limit. Either that is accepted for Halflings, or their dress is restated in `races-basis.md`.
6. **Dwarf women are bearded** in `races-basis.md` and outsiders cannot tell the sexes apart. Sex-split lists and a list without facial hair do not fit that race (§14 items 3 and 5). RH6 therefore carries the beard and is for both sexes.
7. **Details the lists do not carry.** The Rainkeepers' mark of coloured paste on the brow, the Northmen's arm-rings as money, and the Orcs' ash-rubbed scars are in the Appearance paragraphs and are not hairstyles, outfits or colours. The same holds for the marker of most new races: antlers, a salt crust, a throat patch, a band of strokes at the hairline. §8.4 now holds them as race markers. Build, height and the shape of the face are still in no list.
8. **Skin can only follow hot and open against cold and shaded.** `human-races.md` §6.4 item 10 notes that the map has no latitude. Section 7 therefore gives tones without tying them to a place; §11 ties them to races.
9. **Nothing contradicts the readme.** The lists add no domain term. "Component" is the task's word, not a term of the readme.
10. **Dress that changes with the person's state.** Several races wear one thing in one state and another in the next, and a fixed component cannot follow that. A Twofold wears a saffron sash before the Turning and an indigo one after it (RO22, RO23). A Duskreader wears a mask at home and a visor by day (RO13, RO14). A Bitterblood bares the patches at home and covers up abroad (RO24, RO25). A Lanternkin wears RO21 only out of doors. The rings of HC21 count the years of a Swiftborn. Markers and eyes change too: the antlers and the eyes of the Tinefolk follow the season (RM6, RM7, EC9, EC12), the eyes of a Longsleeper follow the hour (EC22, EC23, EC7), the throat patch of a Twofold follows age (RM22, RM23), and RM8 and RM12 show only in a cold or a sunlit scene. Which state a portrait shows needs a rule (§14 item 12).
11. **Dress that hides the marker.** RO13 hides the whole face of a Duskreader and RO14 hides the black eyes that are the race's marker. RO21 shades the face of a Lanternkin, whose tracery shows only in shadow. A portrait in that dress does not show what the Appearance paragraph says it must.
12. **One dress for both sexes, with the chest bare.** The paragraphs give the Noonfolk, the Shoalfolk and the Hearthbloods one dress each, which leaves the chest bare (RO15, RO17, RO10). The entries follow the doc. Whether a portrait of a woman of those races is wanted that way is for the user, and an image model may refuse it.
13. **Paragraphs that say too little.** No dress is named for Amberbloods, Swiftborn, Flinchless, Stillfaces and Lineminds, no skin for Halflings, Gnomes and Eyrie folk, and no hair colour for Dwarves, Duskreaders and Longsleepers. Eight races have a hair colour or texture and no style. §8 adds nothing for those gaps.
14. **The arm-rings of the Northmen are in three places.** The paragraph, MO11 and RM24 all carry them, so §9.3 gives the Northmen no bracelet. The model backs the doc: silver was the currency, paid by weight, and the amount needed was cut off [NATMUS-SILVER] [ARMRING]. The same museum says its people wore no earrings [NATMUS-C], so §9.3 gives the Northmen none.
15. **Wealth that is not worn as jewellery.** Midlanders: "Rank shows in the clothes". Halflings "wear their prosperity on their backs". Canal folk show rank in "silk in the colour of their grade, with a square badge on the chest", and the source for their model puts hairpins and headdresses before all other jewellery [BRIT-JEWEL-EAST]. §9.3 gives these races few accessories. The Twofold get none, because "Everyone dresses alike".
16. **Amber and the Amberbloods.** The set blood of an Amberblood "becomes true amber", and people were bled for it. AC1 is a string of amber. §9.3 bars it for that race; whether other peoples may wear it near them is not decided.
17. **Rows of §8 that say more than the doc.** RO3 says "no bright metal or colour anywhere", RO18 "no metal, bead or ornament anywhere" and RH15 "no ornament". Those words are my additions in those rows. The doc itself bars only what is easy to see between trees, what jingles and what can catch. §9.3 follows the stricter rows, so that no prompt contradicts itself. If the rows are loosened, Elves and Hushfolk could take quiet pieces.
18. **Where a model and the doc pull apart.** The model of AC21 was used to pay a bride-price [MANILLA]; among the Sunlanders "Cattle are bride-price", so the lore of AC21 keeps the bangles to small payments. The model of AC9 holds gold unlucky [TUAREG]; the Saltroad folk carry gold, so §9.3 gives silver to the desert clans and gold to the towns. The model of AC2 is the hammer of a god the doc does not give the Northmen; the lore gives it to "the first Shipwright".
19. **Gnome gem cutters and the period limit.** `races-basis.md` makes Gnomes gem cutters. Stones were polished, not cut, until the late 14th century [VAM-JEWEL], and the faceted cuts a reader pictures are later than these lists go. No accessory has a faceted stone, and AC25 is polished. A cut stone for Gnomes would break the limit as the waistcoats of the Halflings do (item 5).
20. **The lore of §9 is new text about the races.** The Lore column says who gives a piece, when it comes off and what it means. Nothing in it contradicts `races-basis.md`, and none of it is in that doc, apart from the quoted words. That doc is where the lore of a race lives. If the lore is kept, it should be moved there or checked against it by the user (§14 item 18).
21. **Accessories that follow a state.** Like the dress of item 10, some accessories are worn only in a state the Stub may or may not hold: married (AC8, AC24, AC40, AC46, AC47), on a feast day (AC43), in autumn (AC53), or while a post is held (AC10, AC38, AC52).
22. **The default list was fixed after this note was written.** Since 2026-11-15 the default list is the 28 races that `races-basis.md` now describes: seven human races, five classic races and sixteen new ones, with no plain `human`. That settles item 1. Goblin is gone. Tinefolk, Hushfolk, Longsleepers, Eyrie folk and Lineminds were left out of the list and of that doc, so the words quoted for them here can no longer be checked against it. The entries written for them alone are kept (among them SC20, SC30, SC31, EC12, EC20, EC22, EC23, RM6, RM7, RM15, RM18, RM19, RM21, AC47 and AC53), and no race of the list uses them.
23. **The Catfolk have two looks in the docs.** `races.md` F2 gives the Catfolk a cat's head, and `human-races.md` says the same of them. `races-basis.md` gives them a human face and body with cat ears, a tail and a slit pupil, and the entries here follow it. No entry of this note is for a furred or cat-headed person.

### 12.2 Entries that rest on late evidence

The look is pre-industrial in each case, but the source that was read dates it late or not at all.

| Entry | What the source dates |
|---|---|
| WH14 Tall Headwrap | [HEADTIE] describes present-day use and gives no date of origin |
| WH18 Winged Horns | Museum pieces of the 19th century; the custom ended in the 1920s and 1930s [KHALKHA] [WMW] |
| WO8 Banded Herder Tunic | [GAKTI] describes the present garment; reindeer hide is given as the older material |
| WO22 Strip-woven Royal Cloth | Cotton strip cloth from the 17th century, silk from the late 18th [KENTE] |
| WO23 Red Herder Cloth | Cotton sheets replaced hides from the 1960s [MAASAI]; the description offers hide as the alternative |
| AC7 Red Coral Bead Collar | [CORAL] names the wearers and gives no date for the custom |
| AC9 Silver Amulet Case | The museum case is of about 1800 [VAM-AMULET]; [TUAREG] describes present practice |
| AC21 Open Brass Bangle | First recorded in 1505; its origin is disputed [MANILLA] |
| AC33 Chained Triangle Brooches | Most surviving pieces are of the late 19th or early 20th century; the form is said to be ancient in the region [BERBER] |
| AC38 Beaded Fringe Crown | The dated crowns are of the 19th century [OBA-CROWN] |
| AC43 Forehead Pendant | [TIKKA] gives no early date; [BRIT-JEWEL-IND] names forehead jewels without one |

### 12.3 Gaps in this research

- **The Met refused every request** (HTTP 429): the Heilbrunn essays on ancient Greek dress and on kente, and the object page for a tupu pin. The Greek entries rest on Britannica instead. The tupu page is cited from a search excerpt and marked [partially verified].
- **Pages that were missing** (404): Wikipedia "Crespine", "Sideless surcoat" and "Ji (hairpin)"; the Fashion History Timeline page for the crespine; Stability AI's prompt guide. The British Museum object page returned navigation only. Google's skin tone research page returned a title only, so the Monk Skin Tone Scale is not used.
- **Not read:** the British Museum, the Museum of London and the Kyoto Costume Institute; any printed costume history; the vendors' guides beyond the three pages named. The Wikipedia article on Bantu knots gave no history, so that style was dropped.
- **Thin pages.** [SANGTU] says only that it is the topknot of married Korean men. [CHIGNON] does not say where on the head the Greek knot sat; [BRIT-GR] does. [MAASAI] does not describe bead collars or women's shaved heads in the part that was read.
- **In the accessories pass:** the Met returned 429 again, on the Heilbrunn essay on gold. The British Museum's object page for a brass head with a beaded crown returned navigation only. Wikipedia has no page at "Gold wreath", "Ade (Yoruba crown)", "Jewellery of ancient Rome" or "Ancient Roman jewelry". "Jhumka" is the page of a town and "Shubi" a disambiguation page, so a bell-shaped earring was dropped and the comb of AC44 rests on Britannica alone. [PAS-BROOCH] was read to character 100,000 of 220,000, which stops before its medieval part. [INUIT] was read again to character 100,000, and its part on amulets was not reached. [CIRCLET] does not say who wore a circlet or when. [TIKKA] treats the piece in a list and a footnote.
- **Not read for the accessories:** the British Museum and the Met; any printed history of jewellery; anything on the jewellery of the steppe beyond [KHALKHA], [WMW] and the Scythian paragraph of [BRIT-JEWEL-IND]; anything on the Horn of Africa; anything on jewellery in the Latin West between the 7th and the 12th century.
- **Unverified details carried in §9:** the high stacked collar (AC7); the square form and the size of the amulet case (AC9); the shape of the links (AC10); two pearls on one wire (AC13); the breaking of the bangles (AC18); the thumb ring among the Mongols (AC27); where the disc brooch sat (AC31); the pose of the stag and where the plaque is worn (AC34); the disc, the cord and the tassel (AC42); the comb standing in the dressed hair (AC44).
- **Unverified details carried in the lists:** loose hair as the sign of a maiden (WH1); ribbon on the plaits (WH3); the cut of the wig (WH10); the curled-under bob (MH3); ermine (WO5, MO6); the scarf (WO17); the women's drape of the royal cloth (WO22); the bead collars (WO23); leather for the smith's apron (MO2); the miner's hood (MO3); the lamellar helmet (MO10); fishers and sailors in the work tunic (MO13); the official's belt and cap (MO17).
- **Composed or fantasy entries, nine in all:** WH20, WO25, WO26, WO27, WO28, WO29, WO30, MO20, and WH19 apart from its one historical case. In §9, AC4 is a composed type, AC35 is fantasy convention when it is worn by elves and princes, and AC52 is fantasy convention.
- **Entries with no outside source, 143 in all:** the 23 skin colours of §7.2, hair colours HC16 to HC25, the 101 entries of §8 (hairstyles, outfits, eye colours and race markers) and the nine accessories of §9.2. Each rests on a paragraph of `races-basis.md`. RH5, the garments of RO3 and AC52 are fantasy convention. Every cut, material or number that goes beyond the quoted words is named in its row as my addition.
- **Invented:** every description, the names, the hair and skin colour names, all of §11, and in §9 the Lore column, the fit to the races (§9.3) and the rule of §9.5.
- **No prompt was run.** Whether the descriptions give consistent pictures on any model is untested.

---

## 13. Sources

### 13.1 Museums

- **NATMUS-A** — National Museum of Denmark, "The Viking Age: The people: Appearance". https://en.natmus.dk/historical-knowledge/denmark/prehistoric-period-until-1050-ad/the-viking-age/the-people/appearance/
- **NATMUS-C** — National Museum of Denmark, "The Viking Age: The people: Clothes and jewellery". https://en.natmus.dk/historical-knowledge/denmark/prehistoric-period-until-1050-ad/the-viking-age/the-people/clothes-and-jewellery/
- **KHALKHA** — Asia and Pacific Museum, Warsaw, "Married Mongolian woman's bonnet" (object of the month). https://www.muzeumazji.pl/en/obiekt-miesiaca/59369/
- **WMW** — Weltmuseum Wien, "Schmuckgehänge" (head jewellery of a married Mongolian woman, 19th century, inv. no. 64851). https://www.weltmuseumwien.at/en/objects/schmuckgehaenge-441118
- **VAM** — Ruth Hibbard, "What to wear when you don't have a halo", V&A Blog, 14 June 2015. https://www.vam.ac.uk/blog/museum-life/what-to-wear-when-you-dont-have-a-halo
- **MET-TUPU** — The Metropolitan Museum of Art, "Tupu (pin)", object 309227. https://www.metmuseum.org/art/collection/search/309227 **[partially verified: search excerpt; the site returned 429]**

### 13.2 Encyclopaedia Britannica

- **BRIT-MESO** — "Dress", section "Mesopotamia". https://www.britannica.com/topic/dress-clothing/Mesopotamia
- **BRIT-GR** — "Dress", section "Ancient Greece". https://www.britannica.com/topic/dress-clothing/Ancient-Greece
- **BRIT-AM** — "Dress", section "The pre-Columbian Americas". https://www.britannica.com/topic/dress-clothing/The-pre-Columbian-Americas
- **BRIT-ME** — "Dress", sections "The Middle East from the 6th century" and "The Byzantine Empire". https://www.britannica.com/topic/dress-clothing/The-Middle-East-from-the-6th-century
- **BRIT-MED** — "Dress", section "Medieval Europe". https://www.britannica.com/topic/dress-clothing/Medieval-Europe
- **BRIT-EAST** — "Dress", section "The history of Eastern dress" (China). https://www.britannica.com/topic/dress-clothing/The-history-of-Eastern-dress
- **BRIT-JKS** — "Dress", sections "Japan", "Korea" and "South Asia". https://www.britannica.com/topic/dress-clothing/Japan
- **BRIT-HAIR** — "Hairdressing". https://www.britannica.com/art/hairdressing

### 13.3 Wikipedia: hair

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

### 13.4 Wikipedia: dress

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

### 13.5 Hair and skin colour

- **HAIRCOL** — Wikipedia, "Human hair color". https://en.wikipedia.org/wiki/Human_hair_color
- **SKINCOL** — Wikipedia, "Human skin color" (read to character 100,000 of 201,000). https://en.wikipedia.org/wiki/Human_skin_color
- **FITZ** — Wikipedia, "Fitzpatrick scale". https://en.wikipedia.org/wiki/Fitzpatrick_scale

### 13.6 Press

- **SCINORD** — ScienceNordic, "What Vikings really looked like" (quotes curators of the Viking Ship Museum in Roskilde and the Danish National Museum). https://sciencenordic.com/archaeology-denmark-history/what-vikings-really-looked-like/1374457

### 13.7 Image-model vendors (first-party)

- **OAI-PROMPT** — OpenAI, "Image prompting", OpenAI Cookbook (read to character 100,000 of 128,000). https://developers.openai.com/cookbook/examples/multimodal/image-gen-models-prompting-guide
- **BFL-FLUX2** — Black Forest Labs, "Prompting Guide - FLUX.2 [pro] & [max]". https://docs.bfl.ml/guides/prompting_guide_flux2
- **GOOGLE-IMG** — Google, "Image generation", Gemini API documentation (read to character 100,000 of 241,000). https://aistudio.google.com/docs/image-generation

### 13.8 Jewellery

Museums:

- **VAM-JEWEL** — V&A, "A history of jewellery". https://www.vam.ac.uk/articles/a-history-of-jewellery
- **VAM-RINGBROOCH** — V&A, "Ring Brooch", England or France, 1200–1300, object O15219. https://collections.vam.ac.uk/item/O15219/ring-brooch-unknown/
- **VAM-AMULET** — V&A, "Amulet Case", Iran, about 1800, museum no. 1302-1873. https://collections.vam.ac.uk/item/O325257/amulet-case/
- **LOUVRE-SUSA** — Musée du Louvre, "Bracelet décoré de têtes de lions", Susa, about 350 BCE, inv. SB 2761. https://collections.louvre.fr/en/ark:/53355/cl010176848
- **NATMUS-SILVER** — National Museum of Denmark, "The silver hoards of the Vikings". https://en.natmus.dk/historical-knowledge/denmark/prehistoric-period-until-1050-ad/the-viking-age/the-silver-hoards-of-the-vikings/
- **PAS-BROOCH** — Portable Antiquities Scheme, "Brooches" (finds recording guide; read to character 100,000 of 220,000). https://finds.org.uk/counties/findsrecordingguides/brooches-2/

Encyclopaedia Britannica, article "Jewelry":

- **BRIT-JEWEL** — Introduction and "Materials and methods". https://www.britannica.com/art/jewelry
- **BRIT-JEWEL-HIST** — "The history of jewelry design": introduction and "Sumerian". https://www.britannica.com/art/jewelry/The-history-of-jewelry-design
- **BRIT-JEWEL-EG** — Sections "Egyptian", "Aegean" and "Phoenician". https://www.britannica.com/art/jewelry/Egyptian
- **BRIT-JEWEL-ANT** — Sections "Etruscan", "Greek", "Hellenistic" and "Roman". https://www.britannica.com/art/jewelry/Etruscan
- **BRIT-JEWEL-MED** — Section "Middle Ages" (Byzantine, Islamic, Teutonic, Western European). https://www.britannica.com/art/jewelry/Middle-Ages
- **BRIT-JEWEL-EAST** — Section "Non-Western cultures" (Chinese, Japanese). https://www.britannica.com/art/jewelry/Non-Western-cultures
- **BRIT-JEWEL-IND** — Sections "Indian", "Southeast Asian", "Scythian" and "African". https://www.britannica.com/art/jewelry/Indian
- **BRIT-JEWEL-AM** — Section "American Indian" (pre-Columbian Central and South America, North America). https://www.britannica.com/art/jewelry/American-Indian

Wikipedia:

- **TORC** — "Torc". https://en.wikipedia.org/wiki/Torc
- **MJOLNIR** — "Mjölnir" (read to character 100,000 of 134,000). https://en.wikipedia.org/wiki/Mj%C3%B6lnir
- **SCARAB** — "Scarab (artifact)". https://en.wikipedia.org/wiki/Scarab_(artifact)
- **COWRIE** — "Cowrie". https://en.wikipedia.org/wiki/Cowrie
- **SHELL-MONEY** — "Shell money". https://en.wikipedia.org/wiki/Shell_money
- **CORAL** — "Precious coral". https://en.wikipedia.org/wiki/Precious_coral
- **MANGALSUTRA** — "Mangala sutra". https://en.wikipedia.org/wiki/Mangala_sutra
- **TUAREG** — "Tuareg people" (read to character 100,000 of 217,000). https://en.wikipedia.org/wiki/Tuareg_people
- **TAWIZ** — "Ta'wiz" (the address leads to the article "Amulet"; read to character 100,000 of 161,000). https://en.wikipedia.org/wiki/Ta%27wiz
- **LIVERY** — "Livery collar". https://en.wikipedia.org/wiki/Livery_collar
- **EARRING** — "Earring" (read to character 100,000 of 144,000). https://en.wikipedia.org/wiki/Earring
- **PLUG** — "Plug (jewellery)". https://en.wikipedia.org/wiki/Plug_(jewellery)
- **ARMRING** — "Arm ring". https://en.wikipedia.org/wiki/Arm_ring
- **BANGLE** — "Bangle". https://en.wikipedia.org/wiki/Bangle
- **OXUS** — "Oxus Treasure" (read to character 100,000 of 135,000). https://en.wikipedia.org/wiki/Oxus_Treasure
- **MANILLA** — "Manillas". https://en.wikipedia.org/wiki/Manillas
- **SEAL** — "Seal (emblem)" (read to character 100,000 of 118,000). https://en.wikipedia.org/wiki/Seal_(emblem)
- **RING** — "Ring (jewellery)" (read to character 100,000 of 103,000). https://en.wikipedia.org/wiki/Ring_(jewellery)
- **FEDE** — "Fede ring". https://en.wikipedia.org/wiki/Fede_ring
- **THUMB** — "Thumb ring". https://en.wikipedia.org/wiki/Thumb_ring
- **FIBULA** — "Fibula (brooch)". https://en.wikipedia.org/wiki/Fibula_(brooch)
- **BROOCH** — "Brooch". https://en.wikipedia.org/wiki/Brooch
- **CELTIC-BROOCH** — "Celtic brooch" (read to character 100,000 of 133,000). https://en.wikipedia.org/wiki/Celtic_brooch
- **PILGRIM** — "Pilgrim badge". https://en.wikipedia.org/wiki/Pilgrim_badge
- **BERBER** — "Jewellery of the Berber cultures" (read to character 100,000 of 106,000). https://en.wikipedia.org/wiki/Jewellery_of_the_Berber_cultures
- **CIRCLET** — "Circlet". https://en.wikipedia.org/wiki/Circlet
- **DIADEM** — "Diadem". https://en.wikipedia.org/wiki/Diadem
- **WREATH** — "Wreath (attire)". https://en.wikipedia.org/wiki/Wreath_(attire)
- **OBA-CROWN** — "Oba's crown". https://en.wikipedia.org/wiki/Oba's_crown
- **FENGGUAN** — "Fengguan". https://en.wikipedia.org/wiki/Fengguan
- **NOSE** — "Nose piercing". https://en.wikipedia.org/wiki/Nose_piercing
- **ANKLET** — "Anklet". https://en.wikipedia.org/wiki/Anklet
- **JADE** — "Chinese jade". https://en.wikipedia.org/wiki/Chinese_jade
- **TIKKA** — "Maang tikka". https://en.wikipedia.org/wiki/Maang_tikka
- **TEMPLE-RING** — "Temple ring". https://en.wikipedia.org/wiki/Temple_ring

---

## 14. Open questions

1. **Where a race marker lives.** §7.2 and §8 now give the races of `races-basis.md` their skin, hair, dress, eyes and marker. A marker is fixed per race, not picked per person, so it may belong on the race as a line of text and not in a list a person picks from. The marker of several races is split over lists (the Noonfolk's over SC26, RH14 and RM12), which a pick per list can get wrong unless the race restricts each list to its own entries. AC51, AC52 and AC53 are ornaments on a marker (the salt crust, the tusks, the antlers) and raise the same question from the other side. Goblins have no described looks at all.
2. **Children and the old.** The lists are for adults. Several styles are tied to an age or a rite: braids before the hair-pinning at fifteen, a topknot from the capping at twenty, ochre plaits from puberty, warrior braids until elderhood. Whether a person's hairstyle changes with the age in the Stub, and what a child wears, is open.
3. **Facial hair.** MH5, MH10, RH1 and RH6 include a beard and the others say nothing. A beard changes a face more than a haircut does. It is probably its own component, and for Dwarves it applies to both sexes.
4. **Headwear as its own component.** Five women's hairstyles are coverings or include one (WH6, WH7, WH10, WH14, WH18) and many outfits name a hat, hood, veil or crown. A hairstyle and an outfit picked independently can clash (WH14 with the straw hat of WO19) or repeat each other (WH6 with WO6). Either headwear is split out, or each outfit lists the hairstyles it allows. The crowns of §9 (AC35 to AC39) are headwear too, and §9.4 lists what each cannot sit on.
5. **Sex-split lists for every race.** Eleven outfits of §4 and §5 are worn by both sexes, and none of the 101 entries of §8 is split by sex, because the paragraphs of the classic and new races do not split them. Dwarves of the two sexes look alike, and a Twofold changes sex at about thirty (`races-basis.md`). One list per race with a sex column may fit better than two lists. The accessories are already one list, with the sex in the Worn by column.
6. **Hair texture.** Straight, wavy, curly and coiled hair is a property of the person, and it limits the hairstyles: WH11 and WH16 do not suit the same hair. The lists do not carry it.
7. **Age on skin and hair.** Greying is given as four entries. It could instead be a modifier on a base colour, drawn from the age in the Stub. Skin has no entry for age at all.
8. **What else a portrait needs.** Build, height and the shape of the face are in the Appearance paragraphs of `races-basis.md` and in no list here. So are the things a race carries, such as a bow case, and the eye colour of five races is not stated at all (§8.3).
9. **Derived or authored.** The readme makes the portrait authored and the Stub derived. A pick from these lists is a pure function of the person if it goes through `Hashing` with a purpose number, which would make the components derived data in or beside the Stub. If an LLM picks them, they are authored. This decides where they are stored and is for the user.
10. **Role and rank.** The "Worn by" column is free text. The Stub has a role, but the repo has no list of roles and a person has no rank or wealth. An outfit cannot be picked by rule until those exist. The Vocations of `races-basis.md` are the nearest list. The accessories need the same: §9.3 splits every race into common and wealthy, and a person is neither yet.
11. **Material and colour as a separate pick.** Rank shows in cloth and dye more than in cut (§1.3 finding 3). An outfit could take its dye and fibre from a second pick, tied to the rank of the person or to the dyes and fibres the region has, which the readme already lists as resource variants. Twenty accessories of §9.1 are made in a cheap and a dear material and name one of them in the description; the same second pick would serve them, from the metals and gems of the readme.
12. **One outfit or several.** A person may need an everyday outfit and a feast-day one, and a cold-season one in a cold region. The lists give one look each. For some races the dress follows a state of the person: age, home or abroad, indoors or out (§12.1 item 10). Some accessories follow a state too: marriage, a feast day, a season, a post held (§12.1 item 21).
13. **Which lists a race draws from.** §11 is a suggestion. The docs give no rule for it, and two of them disagree on whether a human race should look like a real people (§12.1 item 3). A rule that mixes models on purpose, such as a steppe coat with a northern hairstyle, would answer the stereotype concern of `human-races.md` §6.4 item 16, at the cost of the recognisable types of `races-basis.md`.
14. **Framing and art style.** A fixed style line for every portrait (medium, lighting, framing, background) probably matters as much for consistency as the components do. It is not part of this note.
15. **Testing.** No prompt was run. The descriptions should be tried on the model that will be used, and the ones that drift should be rewritten.
16. **Whether a person has accessories, and how many.** The other components are one pick each. An accessory can be absent, and a person can carry several. §9.5 suggests none or one for most common people and two or three for the wealthy, with at most one per kind. That rule needs the wealth or rank of item 10, and it is untested. Small pieces such as a ring or an earring may also be the first thing an image model drops or changes between two pictures.
17. **The clashes as data.** §9.4 is a table for a reader. A picker needs the same as data: for each accessory the place on the body where it sits, and for each outfit, hairstyle and marker the places it covers or already fills. That is item 4 made general, and it would settle the headwear question as well.
18. **Where the lore lives.** A prompt needs none of the Lore column. The textual description of a Realised person could use it. It is invented text about the races (§12.1 item 20), so the user decides whether it is kept here, moved into `races-basis.md` or dropped.
19. **Accessories for the gaps of §9.3.** No entry is a string of glass beads alone, which the Brinefolk would wear. Men have no hair jewellery beyond MH6 and RH6. Nothing was written for Amberbloods, Flinchless, Longsleepers, Eyrie folk, Lineminds, Hoarkin, Hearthbloods or Catfolk beyond the historical entries. No earring is written for a cat's ear. AC16 has no race.
