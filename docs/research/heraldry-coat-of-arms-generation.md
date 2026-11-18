# Heraldry: What an Achievement of Arms Is Made Of, Where Its Symbols Come From, and How Gameworld Could Generate One per Faction

*Research date: 2026-11-18. Scope: how coats of arms could be generated for the factions, cities, towns and families of Gameworld. §1 is the summary. §2 sets out the parts of an achievement of arms and how they nest. §3 gives the rules that make a design valid: the rule of tincture, the order of blazon, uniqueness and the growth of complexity. §4 says where a bearer's symbols come from, first by mechanism and then by type of bearer, and what the sources say about "meanings". §5 covers regional styles. §6 surveys existing generators, parsers and charge libraries with their licences. §7 is the proposal for Gameworld: which inputs exist today, a data model, a pipeline, what each input contributes, validity, determinism and rendering. §8 lists the open questions and §9 the sources.*

**How to read the citations**

- Each claim cites the source that owns it by a key in square brackets. The key is a link to the source, and §9 lists every key with what was read.
- Most web pages were read through a fetch tool that summarises a page and relays short quotes. Wording in quotation marks comes from public-domain texts (Fox-Davies 1909, Parker 1894, Woodward & Burnett 1892, Bartolus in Hauptmann's 1883 edition) and was relayed by that tool, so check a quote against the page before printing it elsewhere. Modern sources are paraphrased.
- **(secondary)** means the claim rests on a secondary source because the primary one could not be read. **[not verified]** means general knowledge that was not checked against any source in this pass.
- Everything in §7 and §8, and every sentence that maps a heraldic fact onto this repo, is my own engineering judgement and not a sourced claim.
- The names are those of the [readme](../../readme.md): region, zone, patch, biome, resource, Prospect, race, faction, city and town, family, person, vocation, Stub, derived and authored data.

**Hooks in this repo** (working tree at commit `24a5176`)

| Hook | Where | What it gives a heraldry pass |
|---|---|---|
| Faction | `readme.md`, "Faction"; `society-generation.md` §4 | A group of people in one region: a kingdom, a thieves' guild, a tribe. Planned stage. Not specified: what it holds, its race, its relations, the parent of its identity |
| City and town | `readme.md`; `society-generation.md` §5, §5.1 | A settlement on a Prospect. Only its races are specified and implemented (`RegionPlan.ProspectRaces`) |
| Family, person | `society-generation.md` §6, §7 | Not specified, apart from person names |
| Vocation | `society-generation.md` §8 | 51 vocations in nine sectors are decided as a default list. Not specified in detail, not implemented |
| Race | `society-generation.md` §2, Appendix A; `races-basis.md` | 28 races in the Parameter Set with an id, a name, a toggle, biome frequencies and a height modifier. Looks, way of life, vocations and factions are prose in `races-basis.md` and bind nothing |
| Prospect | `src/Gameworld.Domain/Prospects.cs` | `ForCity`, `Score`, `Class` (Rich, Average, Poor), `OnShow`, and the facts `Coast`, `Harbour`, `River`, `Mouth`, `Confluence`, `HeadOfNavigation`, `LakeAreaM2`, `Depth` |
| Patch | `map-generation.md` §13, §14, Appendices B to D and G | One of 16 biomes; 27 resources, each with an amount and a variant (rye, oak, beaver, cod banks) |
| Zone | `map-generation.md` §8, Appendix F | Elevation type and bedrock |
| Travel network | `map-generation.md` §16 | Roads, river routes and sea lanes between the Prospects on show (Settled stage) |
| Identity | `src/Gameworld.Domain/Identity.cs` | `SlotId<TParent>(Parent, Slot)`. No concrete city, faction, family or person identity exists, because the parent is not decided |
| Randomness | `src/Gameworld.Generation/Basics/Hashing.cs`; `map-generation.md` §2.2 | `Hash4(seed, purpose, x, y)` and the sequential generator `Sfc32`. The society uses the purposes `0x4a01` to `0x4a03` |
| Earlier note | `research/factions.md` §5.1 | Lists "heraldry" among the authored text of a Realised faction. §7.2 below proposes otherwise |

---

## 1. Summary

1. **An achievement is a tree, and the shield is its only required part.** Fox-Davies: "It must as a minimum consist of a shield of arms" [FD5]. Everything else (helm, torse, mantling, crest, coronet, supporters, compartment, motto, collar, badge, banner) hangs on rank, sex, office or national custom (§2).
2. **Blazon is a formal, ordered description, and the drawing is free.** The field comes first, then the principal ordinary or charge, then what lies on it, then bordure, canton and chief, and the mark of cadency last [FD8] [PK-B]. The shape of the shield and the style of drawing are not part of it [FD6] [LL-B]. So the structured blazon is the natural source of truth, with text and picture derived from it.
3. **Few rules decide validity.** The rule of tincture (no metal on metal, no colour on colour) with a short list of exceptions [FD7] [PK-T]; uniqueness within the domain of one authority [HS-FAQ]; and readability. Violations of the tincture rule stay under about 2 % in medieval armorials outside Castile and Granada [VELDE-T] (secondary, reporting Pastoureau).
4. **Symbols come from provenance, not from a code of meanings.** The verified mechanisms are: a pun on the name (at least 20 % of medieval arms [PAST-P]), an office or trade, the arms of an overlord, the land, a deed or legend, a patron saint, marriage and inheritance, and a grant by a sovereign (§4.1). The Heraldry Society states that tinctures, lines and charges have no fixed meanings [HS-FAQ], and Fox-Davies denies it flatly [FD1]. Tables of virtues per colour start with Sicily Herald in the fifteenth century [SICILE].
5. **Early arms are simple, and complexity is accumulation.** In the Dering Roll (1270s) 72 % of the shields use exactly two tinctures and about 84 % have a plain field [DERING] (secondary). Quarterings pile up through heiresses: four at most in Plantagenet times, over 200 on record later [FD33].
6. **Styles differ by nation in ways a generator can parametrise:** how a branch differences its arms (small marks in England, bordures in Scotland and Spain, the crest or the tinctures in Germany, not at all in Poland), who shares a coat (one person in Scotland, a whole clan in Poland), and what the emblem is at all (a Japanese mon is one motif, often in a ring) (§5).
7. **Existing generators confirm the shape of the problem.** Armoria and Fantasy Map Generator draw from weighted tables, pick the charge tincture from the opposite class, and inherit field, ordinary and charge from a parent with a "kinship" probability; settlement type swaps the charge table (§6.1). Crusader Kings III and Imperator: Rome use layered emblems, a `parent` to inherit from and `sub` for quartering (§6.3). Imperator's wiki says there is no duplicate check, and none was found in the Armoria code that was read.
8. **In Gameworld the map side is ready and the society side is not.** Landscape, biome, resources and Prospect facts exist as derived data. Factions, cities, families, names, rank, age, overlords and religion do not. There is no fauna list: animals appear only as variants of five resources, and the heraldic staples (lion, eagle, wolf, raven, monsters) are absent (§7.1).
9. **The proposal:** a structured `Blazon` as derived Stub data that records the provenance of every element; a heraldic tradition per race; a pipeline that collects weighted motif candidates from each input, composes within a complexity budget, applies the tincture rule and differences against a per-region register in slot order; SVG and blazon text as pure renderers; a painted emblazonment as optional authored data (§7).
10. **Decisions for the user** are in §8. The largest are: whether a noble house is a faction or a family, whether canting runs from the name to the arms or the arms to the name, where the fauna list and the charge catalogue live, and how uniqueness is scoped in an unbounded world.

---

## 2. Anatomy of an achievement

The whole emblazonment is the achievement; the coat of arms proper is what is on the shield [FD5].

```text
Achievement
├─ Shield (escutcheon)                 required
│  ├─ Field                            a tincture, a division, a variation, or semé
│  ├─ Ordinaries and sub-ordinaries    with a line of partition
│  ├─ Charges                          kind, attitude, attributes, number, arrangement
│  ├─ Chief, canton, bordure           blazoned late; often carry differences and augmentations
│  ├─ Mark of cadency                  blazoned last
│  └─ or a marshalled shield           impaled, quartered, with an inescutcheon
├─ Helm                                type and position by rank
│  ├─ Torse (wreath) or crest-coronet
│  ├─ Mantling
│  └─ Crest
├─ Coronet or crown of rank            above the shield, below the helm
├─ Supporters, on a compartment
├─ Motto on a scroll; war-cry above the crest
├─ Collar or circlet of an order       around the shield
└─ Badge, banner, standard             separate from the achievement, used by followers
```

### 2.1 The shield

The shield is "the most important part of the achievement" [FD6]. Its shape is free. "There is no law upon the subject", and shapes are "purely the result of artistic design" [FD6]. The Court of the Lord Lyon says the same: the shape does not matter unless the blazon names it [LL-B]. Three shapes carry a meaning by custom:

- The heater shape is "to all intents and purposes the earliest shape which was used for armorial purposes" [FD6].
- "The lozenge certainly is reserved for and indicative of the achievements of the female sex" [FD6].
- The oval or cartouche was "very nearly universal with ecclesiastics in France, Spain, and Italy", though never reserved to them [FD6].

Dexter and sinister are the bearer's right and left, not the viewer's [FD8].

### 2.2 Tinctures

| Class | Tincture | Colour | Note |
|---|---|---|---|
| Metal | Or | gold, yellow | [FD7] |
| Metal | Argent | silver, white | [FD7] |
| Colour | Gules | red | [FD7] |
| Colour | Azure | blue | [FD7] |
| Colour | Sable | black | Fox-Davies notes it "is not really a colour" [FD7] |
| Colour | Vert | green | [FD7] |
| Colour | Purpure | purple | [FD7] |
| Stain | Murrey, sanguine | mulberry, blood red | "between gules and purpure"; Fox-Davies treats sanguine as the same as murrey [FD7] |
| Stain | Tenné | orange-tawny | [FD7]; Parker lists tenné and sanguine as tinctures 8 and 9, "not so clearly recognised as the seven others" [PK-T] |
| Fur | Ermine | white with black spots | Variants by swapping the two: ermines (black, white spots), erminois (gold, black spots), pean (black, gold spots) [FD7] |
| Fur | Vair, potent | blue and white pieces | "Vair is always blue and white" unless other tinctures are named [FD7] |
| Proper | (natural colours) | | "Anything, alive or otherwise, which is depicted in its natural colours is termed 'proper'" [FD7] |

The stains are late. Fox-Davies says they "were perhaps invented by the old heralds" for their system of abatements, and that in liveries they had no bad meaning [FD7]. A generator can leave them out of ordinary arms or keep them very rare, as Armoria does (§6.1).

### 2.3 The field: divisions, variations and lines

- **Divisions.** A field can be parted along the direction of almost any ordinary: per pale, per fess, per bend, per bend sinister, per chevron, per saltire, and per cross, for which "the more usual term" is quarterly; gyronny is a further division [FD7]. A field cannot be "per pile" or "per chief" [FD7]. Tierced, in three parts, is continental; Parker has it per fess, per pale or per bend [PK-T].
- **Variations.** An even number of stripes or a tiling in two tinctures: barry, paly, bendy, chequy, lozengy [FD7]. Semé means "powdered with" a charge in no stated number [FD7]. Diaper is decoration and "never forms any part of the blazon" [FD7].
- **Lines of partition.** Any dividing line, and the edge of any ordinary, can be other than straight: engrailed, invected ("the precise opposite of engrailed"), wavy or undy ("supposed to have been taken from water"), nebuly ("supposed to be derived from clouds"), indented, dancetty (usually three large teeth across the field), embattled, raguly, dovetailed, potenty [FD7]. Dovetailed is late: Parker dates it to 1720 [PK-D].

### 2.4 Ordinaries and sub-ordinaries

The textbooks do not agree on the classes, with "scarcely any two writers giving similar classifications", and Fox-Davies holds that ordinaries "are no more than first charges" [FD9]. The distinction that has an effect is precedence in blazon: no charge precedes a bend, fess, pale, pile, chevron, cross or saltire [FD9]. His one list [FD9]:

| Group | Members |
|---|---|
| Bands across the field | bend, bend sinister, fess, bar, pale, chevron, cross, saltire, pile, pall (pairle, shakefork) |
| At the edge | chief, quarter, canton, bordure, orle ("a narrow bordure following the exact outline of the shield"), tressure (a double orle), flaunches |
| Small geometric charges | lozenge, fusil, mascle ("a lozenge voided"), rustre (a lozenge pierced round), billet, gyron, fret, label, inescutcheon, roundel |
| Diminutives | bendlet (half a bend), cottise (half a bendlet), and likewise for the others |

Roundels have a name per tincture: bezant (or), plate (argent), torteau (gules), hurt (azure), pellet (sable), pomeis (vert), golpe (purpure); a fountain is a roundel barry wavy argent and azure [FD9]. For a generator this is a text rule only: the data stays "roundel + tincture" and the blazon writer picks the name.

### 2.5 Charges

Fox-Davies's chapter order is a usable taxonomy [FD]: the human figure, the lion, other beasts, monsters, birds, fish, reptiles, insects, trees and plants, inanimate objects. There is "scarcely an object under the sun which has not at some time or other been introduced into a coat of arms" [FD19], so the catalogue is open-ended.

**Frequency.** The lion is on more than 15 % of medieval arms, the eagle on under 3 % and the fess, the second most frequent figure, on under 6 % [PAST-L].

**Attitudes** (the posture of a living charge):

| Kind | Attitudes |
|---|---|
| Beasts | rampant (erect, on one hind paw), passant (walking, one forepaw raised), statant (four paws down), sejant (seated), couchant (lying, head up), dormant, salient (leaping), courant (running) [FD11] [PK-C]. Head: guardant (facing the viewer), regardant (looking back) [FD11] |
| Deer | at gaze, lodged, trippant, courant [FD12] |
| Birds | close (the default for every bird but the eagle [PK-B]), rising, displayed, volant. Fox-Davies defines the bird attitudes less formally than the textbooks [FD14] |
| Fish | naiant (horizontal), hauriant (upright) [FD15], urinant (head down) [PK-F]; a dolphin is embowed [FD15] |

**Attributes** (parts in another tincture than the body): armed (claws, horns, beak of an eagle), langued (tongue), attired (antlers), unguled (hooves), crined (hair or mane), membered (legs of a bird), beaked and legged [FD11] [FD12] [FD14] [PK-C]. A lion is armed and langued gules by default and azure when the lion or the field is gules [FD11]. Heads and limbs are couped (cut straight [PK-C]) or erased (torn, with a ragged edge; used throughout Fox-Davies but not defined in the part read).

**Stars.** A star with wavy rays is an estoile, with straight rays a mullet [FD19].

### 2.6 Number and arrangement

Three charges stand two and one unless the blazon says otherwise: it is "the normal position of three charges in any coat of arms" [FD8], and "it is superfluous to say so" [PK-B]. Other arrangements are named after the ordinary whose direction they follow: in pale, in fess, in bend, in chevron, in cross, in saltire, in orle [FD8]. Charges stand between or within an ordinary, or on it. No statement on preferred numbers was found. For numbers in practice, see the Dering Roll figures in §3.4.

### 2.7 Marshalling

Marshalling is "the conjoining of two or more coats of arms upon one shield" [FD33].

| Method | What it shows | Source |
|---|---|---|
| Dimidiation | Two coats cut in half and joined. Short-lived, because the halves read as one new coat: the lions joined to ship hulks of the Cinque Ports | [FD33] |
| Impalement | Two whole coats side by side. Husband dexter, wife sinister. An office the same way: "the official arms being placed on the dexter side and the personal arms on the sinister" | [FD33] |
| Escutcheon of pretence | The husband of an heiress places her family's arms on a small shield over the centre of his own | [FD33] |
| Quartering | The children of an heiress quarter her arms with their father's. The quarters then accumulate | [FD33] |
| Grand quarters | A quarter that is itself quartered: "the first attempt to break away from the four quarters" | [FD33] |

The first quartered coat is that of Castile and León, adopted by Ferdinand III in 1230 [HS-CL]. Fox-Davies names a seal of 1272 as the earliest [FD33]; the Heraldry Society paper is the later scholarship and is preferred here. In Germany a quartering often marks a fief gained otherwise than by marriage [FD33].

### 2.8 Cadency, differencing and bastardy

Arms belong to one bearer, so kin must difference them. The methods vary by period and nation.

| Method | Detail | Source |
|---|---|---|
| Change of tincture; added small charges | The earliest methods | [FD31] |
| Label, then bordure | "Next to the use of the label in British heraldry came the use of the bordure" | [FD31] |
| English brisures | First son a label of three points, then crescent, mullet, martlet, annulet, fleur-de-lis, rose, cross moline, double quatrefoil for sons two to nine. "The use of these difference marks is optional", and daughters have none. Parker doubts that the marks for sons seven to nine were ever used | [FD31] [PK-C] |
| Scottish bordures | The second son a plain bordure in the tincture of the principal charge; in the next generation engrailed, invected, indented; then bordures parted per pale, per fess, per saltire (the Stodart scheme). Matriculation with a difference is required | [FD31] [LL-1672] |
| German practice | The crest or the tinctures change, the shield often not (§5) | [FD21] [WB2] |
| Illegitimacy | In England the bordure wavy, in Scotland the bordure compony; the baton sinister is for royal bastards. "There were no rules until the eighteenth" century. A "bar sinister" does not exist | [FD32] |

### 2.9 Augmentations

An augmentation is an addition granted by a sovereign as a mark of honour [PK-A]. It is granted for favour or for service [FD37]. Its usual forms are a chief, a canton, an inescutcheon, a quartering, a second crest or supporters [FD37]. The Howard augmentation for Flodden shows the lion of Scotland cut in half and "pierced in the mouth with an arrow"; Nelson's is a fess wavy with the word "Trafalgar" [FD37]. An augmentation is exempt from the rule of tincture [FD7].

### 2.10 The exterior parts

| Part | What it is | What it signified | Source |
|---|---|---|---|
| Helm | The helmet above the shield | In England, by a convention of "about the Stuart times": sovereign gold, affronty, barred; peer silver with gold bars, in profile; knight and baronet steel, affronty, visor open; esquire and gentleman steel, in profile, visor closed. Fox-Davies wants the rules "wholly abolished". No woman but a reigning sovereign bears a helm | [FD20] |
| Torse | A twisted wreath that joins crest and helm | Six twists, alternately of the principal metal and the principal colour of the arms, metal first | [FD25] |
| Mantling | Cloth hanging from the helm | The principal colour lined with the principal metal: the livery colours. Earlier mostly gules; the royal mantling is cloth of gold | [FD24] |
| Crest | The figure on top of the helm | Cannot exist without arms. Earliest known 1198. No woman bears or transmits one, and none is used with a mitre or an ecclesiastical hat | [FD21] [FD38] |
| Crest-coronet | A coronet in place of the torse | "bore no relation to any coronet of rank" | [FD23] |
| Coronet of rank | Above the shield | Duke: strawberry leaves. Marquess: leaves and balls. Earl: balls on tall points between leaves. Viscount: a row of balls. Baron: fewer balls. Assigned by royal warrant only in 1660 | [FD22] |
| Supporters | Figures holding the shield | In England "very rigidly confined to peers, to Knights of the Garter, Thistle, and St. Patrick", and granted to corporations; in Scotland also to chiefs of clans. In origin "the invention of the engraver, who filled up the spaces" | [FD26] |
| Compartment | The ground the supporters stand on | In England left out of the blazon | [FD27] |
| Motto | A phrase on a scroll | In England "mottoes are not hereditary" and not part of the grant; in Scotland the motto is in the patent and cannot be changed without a new one. A war-cry goes above the crest | [FD28] [HS-FAQ] |
| Collar, circlet | The insignia of an order around the shield | A Knight of the Garter "encircles his escutcheon" with the garter | [FD34] |
| Badge | A separate device | "never placed upon the helmet, but was worn by the servants and retainers" | [FD29] |
| Banner | A square flag | Shows the field of the shield, that is, the arms | [FD30] [PK-B] |
| Standard | A long tapering flag | Shows the badge, the crest and the motto on the livery colours, not the arms. Its length grows with rank | [FD30] [PK-F] |
| Hat and tassels | Clergy, in place of helm and crest | A cardinal's hat is scarlet with fifteen tassels a side; an archbishop has ten a side | [FD38] [CE] |

Two points matter for a game. The helm rules, the coronets and the reserved supporters are a rank display system, late and national, which a fantasy world can adopt, vary per race or drop. The badge and the livery colours are the parts followers wear, so they are what a guard, a retainer or a ship of a faction would show.

---

## 3. Rules of blazon and validity

### 3.1 The rule of tincture

"Colour cannot be placed upon colour, nor metal upon metal", and it "must practically always be rigidly observed" [FD7]. The Lord Lyon gives it as the basic rule [LL-B]. It is not in the earliest treatise (c. 1340) and is first stated as a rule about 1410 [VELDE-T] (secondary).

The exceptions:

| Exception | Source |
|---|---|
| A fur goes on metal or colour, and on another fur | [FD7] |
| A charge proper goes on either | [FD7] |
| A charge of a metal and a colour together goes on either | [FD7] |
| A party field can be of two metals or two colours, and a charge over a party field is free | [FD7] [PK-T] |
| Details of a charge (armed, attired, unguled, crowned, chained) | [PK-T] |
| Marks of cadency: labels, bendlets, batons; Scottish cadency bordures | [PK-T] [FD7] |
| Augmentations | [FD7] |
| A chief, canton or bordure that breaks the rule is excused as *cousu* ("sewn") by French heralds | [PK-T] |
| Deliberate breach, *armes à enquérir*: Jerusalem, Argent, a cross potent between four crosslets or | [FD7] [PK-T] |

How often it is broken: Pastoureau's sample of about 10,000 arms from thirteenth- to fifteenth-century armorials has violations above 2 % only in Castile and Granada, and Heim counted about 1,500 metal-on-metal cases among the 120,000 arms of Rietstap, about 1.2 % [VELDE-T] (secondary). Woodward lists twenty-six instances [FD7].

No verified quotation gives visibility as the reason. Fox-Davies does not argue it in the chapter read. Pastoureau ties the rule to both visibility and the symbolism of colours [PAST-U] (read through the summarising tool).

### 3.2 The order of blazon

To blazon is "to describe in words"; to emblazon is "to depict in colour" [FD8]. The order, from Fox-Davies and Parker, with the same five steps on the Lord Lyon's page [FD8] [PK-B] [LL-B]:

1. The field: its tincture, or its division or variation with the tinctures.
2. The principal ordinary, or the principal charge if there is no ordinary.
3. The charges around it, then the charges on it.
4. The bordure, then the canton or the chief, each with what it carries. These come late whatever their visual weight.
5. The mark of cadency, "for difference".

Conventions: the tincture follows the thing it colours; a number precedes the charge; a tincture name is not said twice, hence "of the first" and "of the field" [FD8] [PK-B]. Fox-Davies warns that these "certainly are not laws, they are hardly 'rules'" [FD8].

Read as a grammar, the order is a pre-order walk over a layer tree, which is why a structured blazon turns into text mechanically:

```text
arms       = field , { "," element } [ "," peripheral ] [ "," brisure ]
           | marshalled
field      = tincture
           | "per" division [ line ] tincture "and" tincture
           | variation [ "of" count ] [ line ] tincture "and" tincture
           | tincture "semé of" charge tincture
element    = ordinary [ line ] tincture [ "between" group ] [ "charged with" group ]
           | group
group      = count charge [ attitude ] [ arrangement ] tincture { attribute tincture }
peripheral = "a bordure" … | "a canton" … | "a chief" …
marshalled = arms "impaling" arms | "quarterly" , quarter { quarter } | arms "over all an inescutcheon" arms
```

This sketch is mine. Two real grammars exist: Heraldicon's 2,248-line EBNF and DrawShield's hand-written parser with lexeme files (§6).

### 3.3 Uniqueness

Within the domain of one heraldic authority every coat is unique, and there is no such thing as arms for a surname [HS-FAQ]. In Scotland personal arms are borne by one person at a time and a clan cannot have arms [LL-FAQ]; the register is by statute the "true and unrepeallable rule of all Armes and Bearings in Scotland" [LL-1672]. Danish municipal arms are refused registration when they go against heraldic principles, which includes being confusable with arms already registered [DK-600] (read as the English summary on WIPO Lex).

The medieval record is looser than the maxim. Bartolus held that anyone may assume arms and use them on his own things, and that another can object only if he is injured by it [BART] [VELDE-B]. Scrope v Grosvenor ran from 1385 to 1390 over Azure, a bend or, and left no reasoned judgement; in the related Carminow v Scrope both parties kept the arms [HS-LAW]. So the same simple arms did arise independently in different places, and the conflict only came when the bearers met.

Uniqueness also drives complexity: English crests "must be differentiated as are the arms", which is why modern grants crowd them, while "no such rule holds good in Scotland, hence their simple crests" [FD21].

### 3.4 Simplicity, and how complexity grew

- A very simple design often indicates great age, though no single feature dates arms reliably [HS-FAQ].
- The Dering Roll, England, 1270s, 324 or 325 shields [BL-DER]. An analysis of its blazons gives: gules on 64 % of the shields, argent 52 %, or 50 %, azure 31 %, sable 19 %, ermine 7 %, vair 6 %, vert 0.9 %, purpure 0.3 %; 72 % of the shields use exactly two tinctures; about 84 % have a plain field; of 265 primary charges 138 are ordinaries (fess or bars 53, bends 30, chevrons 20, crosses 20) and 39 are lions [DERING] (secondary: an amateur analysis of a published blazon list).
- Azure was rare in the twelfth and thirteenth centuries and rose from the mid-fourteenth [not verified beyond a French Wikipedia summary of Pastoureau].
- Crests kept "their original simplicity" to the end of the Tudor period [FD21].
- Quarterings: four in Plantagenet times, grand quarters next, then schemes of over 200 recorded quarterings, from which a bearer may select [FD33]. Scotland still allows four [FD33].

So there are two separate effects. The age of the base coat sets its simplicity. The number of generations, marriages and honours since sets how much has been added on top.

---

## 4. Where the symbols come from

### 4.1 Origin of heraldry

Evidence begins toward the end of the second quarter of the twelfth century [HS-BEG]. Between 1140 and 1159 heraldic shields appear on seals in eight countries of western Europe [HS-BEG], and the oldest exactly dated armorial seal is that of Raoul of Vermandois, 1146 [VELDE-O] (secondary, a summary of Pastoureau).

The famous early piece is the enamel plate from the tomb of Geoffrey Plantagenet with a blue shield of gold lions, and the chronicle story that a shield of gold lions was hung about his neck at his knighting in 1127 [FD]. The sources disagree on the date of the enamel (1151 in [HS-BEG], 1160 to 1165 in [VELDE-O]) and the chronicle is later than the event, so 1127 is not contemporary evidence [VELDE-O].

Why then: Pastoureau puts it down to military equipment that hid the face, and sees three older stocks merging: banners gave the colours, the geometric figures and the link to fiefs; seals gave family emblems, canting figures and heredity [VELDE-O] (secondary). Fox-Davies doubts the helm as the cause, since shields were painted long before [FD1]. Pastoureau places the first arms on the fields of battle and tournament [PAST-P].

Heralds grew out of the tournament, where someone had to know the arms of those taking part [FD]. They recorded arms in rolls: Glover's Roll about 1255, the earliest collection of English arms [HS-HHH]; the Dering Roll, 1270 to 1280 [BL-DER]; the Zürich roll, 1330 to 1345, with 559 arms [ECOD]. The English heralds were incorporated in 1484 [HS-COA].

### 4.2 The mechanisms

| Mechanism | How it works | Examples | Source |
|---|---|---|---|
| **Canting** (*armes parlantes*) | The charge puns on the name. At least 20 % of medieval arms, more in German lands, and in Pastoureau's judgement no younger and no less honourable than the rest. The pun can run on the sound alone | Castile (a castle), León (a lion), Lucy (pike, "luce"), Trumpington (trumpets), Arundel (swallows, *hirondelles*), Lille (a lily) | [PAST-P] [PK-C] [FD] [ERIS] |
| **Office or trade** | The charge is the tool or sign of an office, a tenure or a trade. Name and arms can be taken together | Butler: three covered cups, taken with the surname on appointment as Butler of Ireland. Forrester: three hunting horns. Fletcher: an arrow | [PK-C] [WB1] [FD1] |
| **Feudal allegiance** | A tenant or kinsman takes the lord's charge, or his tinctures. Parker's term is arms of patronage | The garbs of the Earls of Chester in Cheshire families; the cinquefoil of Leicester; the quarterly coat of Mandeville in a whole family group; "changes of tincture were also used to denote feudal connection" | [PK-A] [FD] [WB1] [WB2] [ROUND] |
| **Territory and landscape** | Features of the land become figures | Water as "barry wavy argent and azure"; the Stourton fountains for six springs; the mount in base of German and Swiss arms; the lymphad of Orkney and Caithness; ship hulks for the Cinque Ports | [FD] [EB] [WB1] [FD33] |
| **Local fauna and flora** | An animal or plant of the place | A maple tree and a beaver for a Canadian peer. The Canadian authority warns that lions and maple leaves are overused and have lost their force | [FD] [CHA] |
| **Deed or legend** | A story of the founder, true or not | Hay of Erroll: the ploughman with his yokes. Velde judges that such stories are more often untrue than true | [FD] [VELDE-M] |
| **Religion, patron saint** | The saint's attribute or cross | Chelsea: a winged bull for St Luke, patron of the parish. Abbeys add "a crosier, mitre, or key". The Mercers bear the Virgin | [BPA] [PK-A] |
| **Marriage and inheritance** | Impalement and quartering (§2.7) | Castile and León quartered, 1230 | [HS-CL] |
| **Grant or augmentation** | A sovereign adds a charge (§2.9), or a family adopts one as a mark of allegiance | Italian families with an eagle sable for the Emperor | [FD37] [VELDE-M] |

Not verified in this pass: the bear of Berne and of Berlin, the chevrons of Clare as a feudal group, the Bruce saltire in Annandale families, the Hungarian triple mount, the Douglas heart beyond Fox-Davies's remark that the story "is too well known to need repetition".

### 4.3 By type of bearer

| Bearer | What is typical | Exterior | Source |
|---|---|---|---|
| **Person and family** | Arms are granted to one named person and that person's heirs. The stock of §4.2, above all canting, allegiance and inheritance | Helm, torse, mantling, crest; coronet and supporters by rank | [HS-FAQ] |
| **City and town** | Parker's "arms of community". Many come from the town seal. Walls, gates, towers, ships, rivers, the patron saint, the lord's arms. The Nordic authorities publish explicit design rules (below) | The mural crown is common, but in Britain it had "no authority whatever" until shortly before 1915. Supporters for great corporations | [PK-A] [BPA] [RSE] |
| **Guild, company** | The tools and the product of the trade, often three of them around a chevron; sometimes the patron | Great companies were granted crests and supporters | [BPA] |
| **Church** | Arms of the see or house, impaled with the personal arms of the holder, the see on the dexter side | Hat and tassels by rank, or a mitre; no helm, no crest | [FD38] [CE] |
| **Realm** | Arms of dominion "are not personal": they belong to the state a prince rules. Arms of pretension claim a state he does not rule | Crown, supporters, compartment, orders, pavilion | [FD] [PK-A] |

**Guild examples**, all from Fox-Davies's *Book of Public Arms* [BPA]:

| Company | Arms | Allusion |
|---|---|---|
| Grocers | Argent, a chevron gules between nine cloves sable | The spice trade |
| Vintners | Sable, a chevron between three tuns argent | Wine casks |
| Brewers | Barley garbs in saltire and tuns | Malt and casks |
| Blacksmiths | Sable, a chevron or between three crowned hammers | The tool |
| Carpenters | A chevron engrailed between three pairs of compasses | The tool |
| Salters | Per chevron azure and gules, three covered salts argent | The product |
| Barbers and Surgeons | Sable, a chevron between three fleams argent | The lancet |
| Bakers | A hand holding a balance between three garbs | Fair weight; wheat |
| Fishmongers | Three dolphins between pairs of lucies in saltire; keys in chief | The product |
| Goldsmiths | A leopard's face; a covered cup and two buckles | The assay mark; the product |

The pattern is constant enough to be a template: a field, a chevron (or another ordinary), three copies of the tool or the product.

**Nordic civic rules.** These are the most explicit design rules found.

- Norway: a municipal coat normally has one figure, one colour and one metal; simplicity is the core requirement; the figure is to be generic, not a picture of one particular building, animal or person. State approval ended on 1 January 2018 [SNL] (an encyclopedia article, not the regulation text).
- Sweden: metal may not border metal nor colour colour; no shading and no perspective; the tinctures are gold, silver, red, blue, black and green; arms often build on the old town seals [RSE].
- Denmark: the National Archives is the State Heraldic Consultant and advises on public arms, among them municipal arms [RDK]. Municipal arms are registered with the Patent and Trademark Office under Order no. 600 of 12 June 2007, which refuses a design that goes against heraldic principles or can be confused with registered arms, and requires a statement and a description from the National Archives [DK-600]. No published Danish design rules were found.
- Canada: no letters or numbers in arms [CHA].
- Finland: no official source was found.

### 4.4 What colours and charges "mean"

The tables found in popular books (gules for courage, a lion for bravery) are not a medieval system. What the sources say:

1. **Bartolus, c. 1355**, ranks colours by what they represent in nature, not by virtues. Gold is the noblest because it stands for light; red or purple stands for fire and is noble; azure is third and stands for air; white is nobler than black, and black is the lowest [BART]. He also says animals should be drawn "in nobiliori actu eorum", in the act in which they best show their vigour, facing the staff on a banner [BART]. The treatise is dated after the grant of arms to Bartolus by Charles IV in 1355 [VELDE-B].
2. **Sicily Herald, *Le Blason des couleurs***, is the first systematic table. It gives each colour a virtue, a gem, a planet, an age of man, a temperament and a weekday; azure "en vertus se dit Loyaulté" [SICILE]. The 1860 editor dates it between 1435 and 1458 and thinks the second part is by another hand [SICILE]; a modern note puts the herald's death in 1436 or 1437 [ROM].
3. **Planets and gems as names for tinctures** followed. Fox-Davies calls it "this aberration of intellect on the part of mediæval heraldic writers" [FD]. The gem names are credited to Sicily Herald [LONDON].
4. **Fox-Davies:** "That an argent field meant purity… or a lion rampant noble and enviable qualities, I utterly deny" [FD1].
5. **Modern authorities:** the Heraldry Society states that metals, colours, furs, lines and charges have no fixed meanings [HS-FAQ]. Britannica discounts the explanations because nobody wrote about heraldry until it was two centuries old [EB]. Velde calls the search for the meaning of a given coat fruitless, with one nuance: bestiary lore did give animals a character, but a coat with that animal was not thereby meant to recall it [VELDE-M] (secondary).
6. The College of Arms FAQ could not be opened. Pastoureau's own position on virtue tables could not be read.

For a generator the conclusion is practical. What a coat means is its provenance: the pun, the trade, the lord, the river. That is what should be stored. A table of virtues can still exist inside the fiction as something a herald of the world believes, but it is not how the arms should be chosen.

---

## 5. Regional styles

| Style | What sets it apart | Source |
|---|---|---|
| **English, French** | Small marks of cadency on the shield. One crest, itself subject to differencing in England, which "is not the case, however, in any other country". Quartering by descent from heiresses | [FD31] [FD21] [FD33] |
| **Scottish** | One bearer per coat, enforced by a court. Cadets matriculate with a bordure. At most four quarters. The motto is part of the grant | [LL-FAQ] [FD31] [FD28] |
| **German, Nordic** | The crest carries the difference: "a change in the crest itself is often the only mark of distinction between different branches". Crests repeat the arms or their tinctures, and many derive from a pair of bull's horns. Arms differenced only by a change of tincture. Several crested helms on one shield (heading in [WB2]; the body text was not read). Burghers bear arms, with supporters from the fifteenth century. A mount in base is common | [FD21] [WB2] [FD26] [WB1] |
| **Iberian** | Bordures, often gules with eight small saltires, used for maternal descent where other nations use cadency marks. The motto placed in the bordure or on the shield. The cauldron as a frequent charge. The highest rate of tincture-rule violations | [WB1] [WB2] [VELDE-T] |
| **Italian** | A chief of allegiance that shows the party: the *capo d'Angiò* (three fleurs-de-lis under a label) for the Guelphs, the *capo dell'Impero* (an eagle sable on gold) for the Ghibellines. Emperors also granted the chief of the Empire as an augmentation | [VELDE-M] (secondary) [WB2] |
| **Polish** | One coat (*herb*) per clan (*ród*), shared by all its families, often unrelated. No differencing or cadency. Each coat has a name that comes from an old war-cry. Most charges come from clan property marks older than heraldry | [BAJER] (secondary) |
| **Hungarian** | Scenes rather than abstractions, much bloodshed (severed Turks' heads from 1540 on). Ordinaries almost never used. The crest often repeats the shield | [VELDE-HU] (secondary) |
| **Japanese mon** | A family crest in use from about the tenth century among the aristocracy, then the warrior class. Merchants put crests on shop signs, and from the Edo period commoners chose their own. 20,000 to 25,000 are thought to be in use. Motifs: plants (wisteria, peony), animals (crane, turtle), tools, wheels, the moon, mountains, line patterns. Variants change a count or a detail: one, two or three bars in a circle. Unofficial altered crests and women's crests exist | [NIPPON] |
| **Tamga** | A mark of kinship, status and property among steppe peoples, used as a cattle brand, passed from father to son. An heir with a mark of his own puts it beside the old one | [TAMGA] |

Not verified in this pass: that mon are drawn in one colour and that the enclosing ring is a standard device for differencing; the Italian horse-head shield; German house marks (*Hausmarken*); Iberian quartering by the four grandparents; the total number of Polish *herby*; Mamluk office emblems; West African emblem traditions.

What this gives a generator is four switches that can differ per tradition, and so per race:

1. **Who shares an emblem:** one person (Scotland), a family with differences (England), a whole clan without differences (Poland), a household or shop (mon).
2. **Where the difference goes:** a small mark, a bordure, the tinctures, the crest, or nowhere.
3. **What the emblem is made of:** the full grammar of §2, a single figure in a ring, a linear mark, or colours alone.
4. **What stands around it:** helm and crest, several helms, a hat, a crown, or nothing.

---

## 6. Existing generators and what to take from them

### 6.1 Armoria and Fantasy Map Generator

Armoria is Azgaar's heraldry generator; Fantasy Map Generator (FMG) carries a variant of the same code. Both are MIT for the code [ARM-README] [FMG-LIC].

**Data model.** A coat is a small JSON object: `t1` (the field tincture), an optional `division` with its tincture and line, a list of `ordinaries` and a list of `charges`, each charge with a tincture `t`, a position string `p` and a `size` [FMG-TYPES] [ARM-API]. A patterned field is a string, `pattern-tincture1-tincture2-size`, and semé is `semy_of_<charge>` [ARM-GEN]. The shield shape is stored beside the coat.

**Positions.** A position is a string of letters, one charge per letter, and the letters are points on a grid per shield shape: `a b c` across the top, `d e f` across the middle with `e` the centre, `g h i` in base, `j` to `o` an inner ring, `A` to `L` twelve spots on the bordure [ARM-SHD]. The size follows from the string: 1.5 for a single central charge, 0.7 for three, down to 0.18 for more than ten [ARM-GEN]. Each ordinary lists the positions that fit on it and beside it: a fess has `def` on it and `abc` above it [ARM-ORD].

**Weights.** Every choice is a draw from a `{name: weight}` table.

| Table | Weights | Source |
|---|---|---|
| Field class | metals 32, colours 49, stains 1, patterns 14 | [ARM-DEF] |
| Charge class | metals 16, colours 24, stains 1 | [ARM-DEF] |
| Metals | argent 3, or 2 | [ARM-DEF] |
| Colours | gules 5, azure 4, sable 3, purpure 3, vert 2 | [ARM-DEF] |
| Structure | a charge in about 80 % of the coats, an ordinary in about 36 %, a division in about 33 % (the authors' comments on the code) | [ARM-GEN] |
| Divisions | per pale 25, per fess 25, per cross 25, per bend 10, the rest 5 or 1 | [ARM-DATA] |
| Lines | straight 50, wavy 8, engrailed 4, invected 3, about thirty others at 1 to 3 | [ARM-DATA] |
| Charge categories | conventional 33, crosses 13, beasts 7, fantastic 5, arms 5, heads 3, birds 3, plants 3, architecture 3, seafaring 3, tools 3 and so on | [ARM-CHG] |
| Positions | centre 20, three (two and one) 6, the rest 1 to 4 | [ARM-SHD] |

Against the historical figures of §2.5 and §3.4 these weights give too much purpure and vert and too few lions. They are tuned for variety, not for a medieval look.

**Rule of tincture.** One function picks the tincture of an element from the class opposite to what lies under it, and leaves out tinctures already used on the field [ARM-GEN]. It is bypassed for patterns, for charges with a natural colour, and in 2 % of divisions [ARM-GEN]. FMG adds a guard against using both metals [FMG-GEN].

**Context in FMG.** The generator takes a parent coat, a kinship, a dominion flag and a type [FMG-GEN]:

- With probability `kinship` the child takes the parent's field tincture; likewise the parent's lined ordinary; with `kinship − 0.1` the division and the first charge [FMG-GEN].
- A town starts at 0.25 against its state, plus 0.1 if it is the capital, minus 0.1 if it is a port, minus 0.25 if its culture differs from the state's [FMG-BURG]. A province has 0.4, or 0.8 when it is named after its town [FMG-PROV].
- A dominion (a colony, an island province) gets a canton with the parent's tincture and first charge [FMG-GEN].
- The type of a town swaps the charge table with probability 0.3: Naval gives anchors and ships, Highland towers, ravens, wolves and goats, Nomadic camels, wheels, sabres and bows, Hunting stags, Capital crowns [FMG-MAP]. The type comes from the map: a port is Naval, high ground Highland, a large river River [FMG-BURG].
- The shield shape comes from the culture: each named culture carries one (heater, swiss, french, round, oval, banner) [FMG-CULT] [FMG-SHD].

**What to take:** the weighted tables as the mechanism; the opposite-class tincture pick; kinship as a probability per element; a charge table per settlement type; the shield shape per culture. **What not to take:** the position letters as the data model (they describe a drawing, not a blazon); the lack of a uniqueness check (none was found in the files read); the lack of provenance, so a generated coat cannot say why it has a ship.

### 6.2 DrawShield

DrawShield turns blazon text into SVG [DS-README]. It is PHP, GPL-3.0, and runs a tokeniser, a hand-written top-down parser, an XML parse tree, an analyser that applies heraldic knowledge and a renderer [DS-WIKI] [DS-PARSER]. The vocabulary lives in regular-expression lexeme files per language; the English charge file has about 2,400 lines [DS-LEX]. The repository holds 2,788 charge SVGs (counted from the file tree). The source images carry mixed licences and a rendered image is released as CC BY-SA [DS-FAQ]. It has a web API that takes a blazon and returns an image [DS-API], and a random blazon generator that expands templates such as "a lion {pose} {tincture}" with a chance of 60 % each for a division, an ordinary and a charge [DS-RAND]. A sibling project with a Flex and Bison grammar has been idle since 2021 [DS-BP].

**What to take:** proof that blazon text is a sufficient description for a renderer, and the pipeline of parse tree, analyser, renderer. As GPL code it can be studied and used as a test oracle (render our blazon text through its API and compare by eye), but not copied into the game.

### 6.3 Crusader Kings III and Imperator: Rome

The Paradox format is a layer stack, not a blazon [CK3-COA]: a `pattern` texture with up to five colours, then `colored_emblem` and `textured_emblem` entries, each with one or more `instance` blocks giving position, scale, rotation and depth. Three mechanisms matter here:

- `parent` fetches another coat and fills every field that is not set, so a coat can be a parent with one change [CK3-COA].
- `sub` places whole coats in parts of the shield: England and France quarterly is two `sub` blocks with two instances each. Subs cannot nest [CK3-COA].
- A coat named like a title, a house or a dynasty is bound to it at game start, and a title can switch coats by trigger [CK3-COA] [CK3-DYN].

Random generation is documented for Imperator: a template whose fields take a weighted list instead of a value, with lists per colour, pattern and emblem, and entries that are added by trigger, such as a culture group; the result comes from a fixed seed, and there is no duplicate check [IMP-COA]. That CK3 uses the same template lists is my assumption. How CK3 derives the arms of a cadet house was not found documented.

**What to take:** inheritance as "parent plus overrides" and marshalling as "whole coats placed in sub-areas" are both small, proven mechanisms. The trigger lists are the same idea as a candidate pool gated by culture.

### 6.4 Other parsers and grammars

| Project | What it is | Licence | State |
|---|---|---|---|
| Heraldicon [HCON-README] | Editor and renderer that converts between blazon and a data format in both directions; publishes an EBNF grammar of blazon of 2,248 lines [HCON-GR] | GPL-3.0 | Active (November 2026) |
| the-herald-playground [THP] | Blazon parser in TypeScript, English and French; a side project by its own description | MIT | Active (October 2026) |
| drawshield/Blazon-Parser [DS-BP] | LR grammar for blazon | not read | Idle since 2021 |

### 6.5 Charge artwork and licences

A blazon is free to describe; a drawing of it is a work of its artist. Wikimedia Commons states that the copyright attaches to the representation and not to the definition [COMMONS-COA]. So the charge library is the licence question.

| Source | Licence as stated | Use in a commercial game |
|---|---|---|
| Armoria, self-made charges | CC0; every SVG carries its licence, source and author as attributes, so the subset can be filtered [ARM-README] [ARM-SVG] | Yes |
| Armoria, charges from WappenWiki | Non-commercial; the sampled file says CC BY-NC-SA 3.0 [ARM-SVG] [WW] | No |
| Wikimedia Commons heraldic elements | Per file [COMMONS] | Per file: public domain and CC BY yes, CC BY-SA with share-alike |
| Sodacan's Commons work (one file sampled) | GFDL or CC BY-SA [SODACAN] | Yes, share-alike for the artwork |
| Book of Traceable Heraldic Art, about 6,500 drawings | The collection is CC BY-SA, but commercial use needs a check of each source [BTHA] | Only after a per-image check |
| Viking Answer Lady SVGs | CC BY-SA 3.0 [VAL] | Yes, share-alike |
| Mistholme Pictorial Dictionary | All rights reserved [MIST] | No |
| Heraldicon library | Per item, from public domain to CC BY-NC-SA [HCON-ATTR] | Per item |
| DrawShield charges | Mixed; output CC BY-SA [DS-FAQ] | Treat as share-alike at best |
| game-icons.net, about 4,000 icons | CC BY 3.0 [GI] | Yes, with attribution; not heraldic in style |

Whether share-alike on a set of charges reaches the rest of a game that ships them is a legal question this note does not answer.

---

## 7. Proposal for Gameworld

This section is engineering judgement. It uses the names of the readme and proposes nothing that the readme or the generation documents already decide differently. Where it needs a decision, §8 lists it.

### 7.1 What exists today and what does not

**Available as derived data now:**

| Input | From | Use |
|---|---|---|
| Seed, `Hash4`, `Sfc32` | Foundation; `Hashing` | Every draw |
| Region coordinate | `RegionId` | Identity, jurisdiction of the register |
| Elevation type and bedrock of each zone | Outlined stage | Mount, rocks, walls; stone variant |
| Biome of each patch | Detailed (estimated), Elevated (final) | Palette lean, plants, animals |
| 27 resources per patch, each with an amount and a variant | Detailed, Elevated | Charges for produce, timber, metals; furs |
| Rivers with flow, lakes | Detailed | Wavy ordinaries, fountains |
| Prospect facts: coast, harbour, river, mouth, confluence, head of navigation, lake, depth, class, for a city or a town, on show | Planned | Civic charges |
| Races of the region with standing and potential population; races of each Prospect | Planned | Tradition |
| Travel links by kind (road, river route, sea lane) | Settled | Ship, bridge, wheel |
| Prosperity and Supernatural Fields | Any time | Richness of the achievement; monsters |

**Decided but not specified or implemented:** the 51 vocations and their sectors; a factor per vocation on a race; the tiers countryside, town, city and capital; identity as parent plus slot; Stub, Pending, Realised.

**Missing, and needed by one or more inputs the task names:**

| Missing | Needed for | Note |
|---|---|---|
| Factions: kinds, count, race, seat, relations | Everything | `society-generation.md` §4 is open. `research/factions.md` has 48 archetypes and a proposed Stub |
| Cities and towns: which Prospects, population | Civic arms | The Prospect facts are enough for the arms themselves |
| Families and family links | Cadency, marshalling | §6 is open |
| Names of factions, cities and families | Canting | The person-name generator is a character-level Markov chain: its names have no meaning to pun on. The region-name generator is not ported and its word lists are titles and historical names |
| Rank of a faction or family | Helm, coronet, supporters | "What makes a city the capital" is open in §8 of the society document |
| Overlord, parent faction | Inheritance of tinctures and charges | A faction is in one region by the readme. Relations inside and across regions are open |
| Age of a faction | Simplicity of the base coat; accumulated additions | `races-basis.md` has four ages, which bind nothing |
| Religion, patron | Saint's attributes, church arms | Nothing in the repo models gods or cults beyond a line per race |
| A fauna list | Animal charges | See below |

**Fauna.** The readme says the biome of a patch "drives its flora and fauna", but `map-generation.md` has no fauna list. Animals occur only as variants of five resources (Appendix D): wild food and game, furs and ivory, livestock, horses and pack animals, and fish. What that yields per biome:

| Biome | Animals the map names |
|---|---|
| Ice & snow | seals, walrus, narwhal; sled dogs |
| Tundra | caribou, fox and Arctic fox, walrus and seals on the coast; reindeer, sled dogs |
| Boreal forest | deer and elk, beaver, sable and marten, squirrel, ermine; reindeer; musk deer in mountains |
| Cold steppe | antelope and gazelle, upland game birds, fox; sheep; horses |
| Cold desert | hares and rabbits, upland game birds; sheep; Bactrian camel |
| Temperate grassland | bison and wild cattle, hares and rabbits; cattle, sheep; horses |
| Woodland & shrubland | upland game birds, hares and rabbits; goats, sheep |
| Temperate mixed forest | deer, wild boar, bear (high ground), beaver, squirrel, sable and marten; cattle, sheep |
| Temperate broadleaf forest | deer, wild boar, fox; pigs, cattle |
| Temperate rain forest | deer, bear, beaver, squirrel; cattle, sheep |
| Hot desert | antelope and gazelle; goats; dromedary camel, donkeys and mules |
| Thorn scrub | antelope and gazelle, ostrich; goats; dromedary camel |
| Savanna | antelope and gazelle, elephant, ostrich, big cats; cattle |
| Tropical dry forest | wild boar, deer, elephant, big cats; water buffalo, cattle; elephants |
| Tropical rain forest | wild boar, deer, big cats, elephant, exotic feathers; pigs, poultry; elephants |
| Mangrove | waterfowl; crabs, lobsters and shrimp; pigs, poultry |
| Overrides | mountains: wild sheep and ibex, yak, goats, llama; lakes: waterfowl; rivers: salmon, sturgeon, eels; coast: cod, herring, sardine, oysters, tuna; open sea: whales |

This covers the stag, the boar, the bear, the fox, the elephant, the camel, the horse, the ram, the bull, fish and a generic big cat. It lacks the charges heraldry uses most: the lion as such (15 % of medieval arms [PAST-L]), the eagle, the wolf, the raven, the falcon, the swan, the crane, the serpent, and every monster. "Dragonbone" among the arcane materials is the only hint of one.

What would be needed is a small **emblem fauna** table: for each biome a weighted list of the animals a people living there would know and admire (predators, birds of prey, reptiles), plus monsters gated by the Supernatural Field. It is not a resource and has no amount. Where it lives is a decision (§8). Flora needs nothing new: the timber, crop, orchard, herb and dye variants already name oak, beech, pine, cedar, wheat, vine, olive, date palm, apple, rose-like herbs and many more.

### 7.2 Derived and authored

Heraldry already has the split the readme makes. The blazon is a definition; the emblazonment is one artist's drawing of it (§3.2, §6.5).

| Part | Class | Reason |
|---|---|---|
| The structured blazon, with the provenance of each element | Derived, part of the Stub | It is a gameplay fact: a player recognises a faction by it, and other arms inherit from it |
| Blazon text, a plain-language description, a flat SVG | Derived, computed on demand | Pure functions of the structured blazon |
| The legend of the arms, the wording of a motto, a painted emblazonment | Authored | An LLM or a diffusion model writes them from the Stub and must not contradict it |

`research/factions.md` §5.1 puts "heraldry" among the authored text of a Realised faction. If the arms are authored, they cannot be inherited by a vassal or a cadet before the overlord is Realised, and they cannot be checked for uniqueness. So this note proposes the blazon as derived data.

### 7.3 Data model

A sketch in the style of `Gameworld.Domain`. The names are proposals.

```csharp
public enum TinctureClass { Metal, Colour, Stain, Fur, Proper }

/// <summary>What gave an element: the answer to "why does it have a ship?".</summary>
public enum ArmsSource
{
    Tradition, Name, Vocation, Overlord, Landscape, Fauna, Flora, Resource,
    Deed, Marriage, Cadency, Augmentation, Difference,
}

/// <summary>A reference to the thing that gave the element: a vocation id, a resource and variant, a faction identity.</summary>
public readonly record struct Provenance(ArmsSource Source, string? Reference);

public abstract record Field;
public sealed record PlainField(string Tincture) : Field;
public sealed record PartedField(string Division, string Line, IReadOnlyList<string> Tinctures) : Field;
public sealed record VariedField(string Variation, int Pieces, string Line, string First, string Second) : Field;
public sealed record SemyField(string Tincture, string Charge, string ChargeTincture) : Field;

public abstract record ArmsElement(Provenance Provenance);

/// <summary>An ordinary or sub-ordinary. Charges on it are its own; charges around it are siblings that name it in their arrangement.</summary>
public sealed record OrdinaryElement(
    string Ordinary, string Line, Field Tincture, int Count,
    ChargeGroup? Charged, Provenance Provenance) : ArmsElement(Provenance);

public sealed record ChargeGroup(
    string Charge, string? Attitude, int Count, string Arrangement, string Tincture,
    IReadOnlyList<(string Part, string Tincture)> Attributes,
    Provenance Provenance) : ArmsElement(Provenance);

/// <summary>One coat: what the blazon of an unmarshalled shield says, in the order it says it.</summary>
public sealed record Coat(
    Field Field, Provenance FieldProvenance,
    IReadOnlyList<ArmsElement> Elements,       // principal first
    OrdinaryElement? Bordure, OrdinaryElement? Canton, OrdinaryElement? Chief,
    ChargeGroup? Brisure);

public abstract record Arms;
public sealed record SimpleArms(Coat Coat) : Arms;
public sealed record ImpaledArms(Arms Dexter, Arms Sinister) : Arms;
public sealed record QuarteredArms(IReadOnlyList<Arms> Quarters) : Arms;       // at most four, see 7.7
public sealed record ArmsWithInescutcheon(Arms Under, Arms Over) : Arms;

public sealed record Exterior(
    string? Helm, string? Coronet, ChargeGroup? Crest,
    ChargeGroup? Supporters, string? Compartment, bool Motto);

/// <summary>The heraldic facts of a bearer. Derived data, part of its Stub.</summary>
public sealed record Achievement(
    string Tradition,            // decides the shield shape and the drawing style; not part of the blazon
    Arms Arms,
    Exterior Exterior,
    ChargeGroup? Badge,          // what followers wear; the form shown at icon size
    string LiveryColour, string LiveryMetal);
```

Points of the design:

- **Ids are strings into lists**, as race ids are. Tinctures, divisions, lines, ordinaries, charges, attitudes and arrangements are catalogue entries, so the code knows none of them by name, in line with the rule for races and vocations.
- **Arrangement, not coordinates.** `Count` and `Arrangement` ("twoAndOne", "inPale", "aroundPrincipal", "onPrincipal") are what a blazon says. The renderer owns the layout table per shield shape. Armoria's letter grid (§6.1) is a good renderer table and a poor source of truth.
- **Provenance on every element.** It is the meaning of the arms (§4.4), the input to the authored legend, and the hook for gameplay ("these three houses bear the garb of their old lord").
- **Marshalling is recursion over whole arms**, as `sub` is in the Paradox format (§6.3). A quarter can be `QuarteredArms`: a grand quarter.
- **The shield shape is not in `Arms`.** It follows from the tradition at render time (§2.1).
- **Livery and badge are stored**, because the map and the crowd need a form that reads at 24 pixels: two colours for a border or a guard's coat, one figure for an icon. Historically these are exactly what followers wore (§2.10).

The model covers the non-European parallels as subsets: a mon-like crest is a `Coat` with a plain field, one `ChargeGroup` and an annulet as a bordure-like enclosure; a tamga is one linear charge; a colour code is a field with no elements.

### 7.4 Traditions: what a race contributes

A race should not pick charges directly. It should pick a **tradition**, a small record of weights and switches, and several races can share one. The switches are those of §5.

| Field of a tradition | Values |
|---|---|
| Emblem kind | full arms; single figure in an enclosure; linear mark; colours only; none |
| Shield shape | heater, round, oval, lozenge, banner, … (drawing only) |
| Palette | a weight per tincture; whether furs, stains and proper are used |
| Composition weights | a weight per layout (§7.5), per ordinary, per line, per division |
| Charge class weights | beasts, birds, fish, monsters, plants, tools, weapons, buildings, ships, celestial, geometric |
| Who shares | person; family with differences; clan without differences |
| Difference method | brisure; bordure; tincture change; crest change; added mark; none |
| Marshalling | impale and quarter; quarter only for fiefs; none |
| Exterior by rank | which ranks get helm, crest, coronet, supporters, motto |
| Rule of tincture | strict; with a small rate of breaches; not applicable |

A sketch for the 28 default races. The historical parallel is cited where §5 verified it. The fit to each race is a suggestion built on `races-basis.md`, which binds nothing, and several rows rest on traditions this note did not research.

| Tradition | Parallel | Races | Why, from `races-basis.md` |
|---|---|---|---|
| **Armorial** (full arms, brisures, quartering, helm and crest) | English and French (§5) | Midlanders | "The nobility wear fur trim, long sleeves and a coat of arms"; kingdoms, baronies, free towns, craft guilds, orders of knights |
| **Armorial, archaic** (one charge, two tinctures, plants; no quartering, nothing added for centuries) | Early arms (§3.4) | Elves | Houses "with a policy older than its neighbours' kingdoms": great age reads as great simplicity |
| **Armorial, plain** (one figure, one colour, one metal; shield only) | The Norwegian municipal rule [SNL] | Northmen, Halflings, Hearthbloods | Thing districts, ship fellowships, Hundreds, inns: bearers without lords. Halfling inn chains suggest signboard figures |
| **Hold arms** (difference by crest and tincture; tools, mounts and metals; a strict register) | German (§5) | Dwarves, Gnomes | Dwarves "keep every contract and every grudge in writing". The gnomish college registers each maker's "personal mark, and for fifty years nobody else may copy it": a uniqueness rule that already exists in the lore |
| **Clan mark** (one named emblem per clan, shared by all its families, no differences; linear marks, brands) | Polish *herby* [BAJER], tamga [TAMGA] | Grass riders, Orcs, Widegaze | Clans and their herds, hordes, warbands. Orc chiefs "keep the standard of a legion", so an orc hold can also carry an inherited standard |
| **Roundel crest** (one motif, often in a ring; variants by count and detail) | Mon [NIPPON] | Canal folk lineages | "A lineage with its ancestral hall", merchant houses. Officials add "a square badge on the chest that shows their rank": a rank mark separate from the lineage crest |
| **Colour code** (field only: divisions, stripes and lines; no figures) | Not researched | Rainkeepers, Boughwalkers, Shoalfolk | Rainkeeper dress is "in colours that tell the wearer's community". Boughwalkers make dyes "in the high range", so a mark "can carry a pattern nobody else can see": an extra tincture that only they read |
| **Form only** (a figure or mark without tinctures) | Mon drawn in one colour [not verified] | Duskreaders, Hoarkin | To Duskreaders "colour means little"; Hoarkin carve walrus ivory |
| **Totem figure** (one animal or object, large, naturalistic) | Not researched | Sunlanders, Catfolk, Amberbloods | Cattle clans, a masked society, the cat-headed Small Huntress, tree lineages |
| **Seal** (an impressed or written mark; lettering allowed) | Not researched | Saltroad folk, Lanternkin, Brinefolk | The people who "kept the books"; Lanternkin notaries seal with a drop of glowing blood under glass |
| **Corporate only** (the group has an emblem, persons and families do not) | Legion standards [not verified] | Flinchless, Swiftborn, Noonfolk, Twofold, Bitterbloods | Legion villages, year-classes, communes, houses "and their chain": races whose factions are institutions |
| **None visual** | | Stillfaces | They "speak in scent" and "keep poor archives" |

Small touches fall out of the hidden traits and cost nothing: Noonfolk have "never seen the stars", so their catalogue has no mullet, estoile or moon; a Twofold person could move from a shield to a lozenge at the Turning, since the lozenge is the women's shape (§2.1); Brinefolk lean to argent.

A tradition is a property of a race and not of a region, so a region with six races has several traditions side by side. The tradition of a faction, a city or a family then follows from its race, which is open in `society-generation.md` §2 and §4. A city of five races needs a rule: the tradition of its largest race, or of its ruling faction.

### 7.5 The pipeline

For one bearer (a faction, a city or town, a family), in this order. Each step has its own purpose number (§7.9), so a change to one step does not move the draws of another.

```text
1. tradition   = the tradition of the bearer's race                              (7.4)
2. class       = realm | noble house | city | town | guild | religious house | order | kin group | outlaw
3. budget      = complexity budget from age, rank and tradition                  (7.7)
4. candidates  = the motif pool: one weighted list per source                    (7.6)
5. inherit     = from the overlord or parent, per element, with a kinship        (7.6, overlord)
6. layout      = a composition drawn by weight, within the budget
7. motifs      = fill the slots of the layout from the pool, by weight
8. tinctures   = field from the palette; each upper layer from the opposite class (7.8)
9. difference  = check against the register of the region in slot order; difference on a clash   (7.8)
10. lineage    = brisure for a cadet, marshalling for a marriage, augmentation for a deed       (7.6)
11. exterior   = by rank and tradition                                           (7.6, rank)
12. livery, badge = the first metal and colour; the principal charge
```

**Layouts.** A layout is a shape with slots. Starting weights for the Armorial tradition, set between the Dering Roll (plain fields, many ordinaries, §3.4) and Armoria (§6.1). They are a first calibration and nothing more.

| Layout | Budget | Weight | Example |
|---|---|---|---|
| Field alone, parted or varied | 1 | 5 | Barry wavy argent and azure |
| Field and one ordinary | 1 | 20 | Azure, a bend or |
| Field and one charge | 1 | 25 | Or, a lion rampant gules |
| Field and two to six of one charge | 2 | 12 | Gules, three hammers or |
| An ordinary between charges | 2 | 18 | Sable, a chevron between three tuns argent |
| Charges on an ordinary | 2 | 8 | Argent, on a fess azure three garbs or |
| Parted field with a charge, counterchanged or not | 2 | 7 | Per pale or and vert, a tree counterchanged |
| Semé field with a charge or an ordinary | 3 | 3 | Azure semé of mullets, a tower argent |
| Any of the above with a chief, a canton or a bordure | + 1 | by class | … and on a chief gules a crown or |

The guild pattern of §4.3 is the fifth layout with the tool in the charge slot. The civic pattern is the third or the sixth with a wall, tower or ship and a wavy ordinary.

**The motif pool.** Every source adds candidates of the form (slot kind, value, weight, provenance). A slot kind is a principal charge, a secondary charge, an ordinary, a line, a field treatment or a tincture lean. Step 7 draws without replacement, so one source cannot fill every slot, and a source with nothing to say adds nothing.

### 7.6 What each input contributes

| Input | Contributes | How | Exists today |
|---|---|---|---|
| **Race** | The tradition: emblem kind, shield shape, palette, composition weights, charge classes, who shares, how branches difference, the exterior | A tradition id on the race record, like the factor per vocation that is already decided to live on the race | The race list exists. The tradition does not |
| **Biome: fauna** | Animal charges; supporters and crests for the higher ranks | Candidates from the animals of the patches of the seat and its domain, weighted by patch count. The resource variants of today (table in §7.1) plus the emblem fauna list | Partly: resource variants only |
| **Biome: flora** | Tree, leaf, flower, fruit and sheaf charges; the compartment | Candidates from the timber, crop, orchard, herb and dye variants of the patches: oak, pine, vine, olive, date palm, a garb for grain | Yes |
| **Biome: palette** | A lean on the tinctures | A weight per biome and tincture (vert and or on grassland, argent and azure on ice, or and gules in desert). Fiction, not history; keep it weak | Yes |
| **Landscape** | Ordinaries and lines that stand for water and height; buildings and ships | River on the patch: a fess, bend or pale wavy, or bars wavy from a large river. Confluence: a pall wavy. Lake: a fountain. Coast or harbour: a ship, an anchor; a base barry wavy. Mountains or hills in the zone: a mount in base, a chevron, an embattled or indented line. Head of navigation, a road over a river: a bridge. Sea lane: a ship; road hub: a wheel | Yes: Prospect facts, zone elevation type, travel links (Settled) |
| **City or town** | The civic class: walls, a gate, a tower; the Prospect facts above; the overlord's arms in a chief or canton; richness | A city (`ForCity`) may bear a wall or gate and a mural crown; a town a single tower or only the landscape motif. `Class` (Rich, Average, Poor) moves the budget by one. The capital takes the realm's tinctures | The Prospect exists. The city, its rank and its overlord do not |
| **Vocation** | The tool and the product of the trade, for a guild, a company or a family named for its work | A list of charges per vocation in the Parameter Set, beside its inputs and outputs. From the guild evidence of §4.3: Smith a hammer, anvil or tongs; Carpenter compasses or an axe; Brewer a tun and a garb; Baker a balance and a garb; Salter a covered salt; Fisher fish; Miner a pick; Miller a millrind or a wheel; Weaver a shuttle; Merchant a balance or a ship; Sailor an anchor; Soldier a sword; Priest a key or a staff; Scribe a pen or a book; Healer a fleam; Mage a star | The list of 51 is decided. Its records are not specified |
| **Resources** | The product of the place: charges, and furs as tinctures | The dominant resources of the domain by amount, with their variants: grain a garb; orchard a bunch of grapes; timber a tree; iron a pick or hammer; gold bezants; salt a salt; fish a fish; gems a lozenge. A patch with the fur variant ermine can make the fur ermine available as a tincture | Yes |
| **Name** | A canting charge or field | Three options, in §8: (a) names built from a lexicon whose parts map to charges; (b) the arms first, and the name from the principal charge, as with Butler and the named Polish coats (§4.2, §5); (c) a search of a generated name for the sound of a charge word. Historically at least a fifth of all arms cant, so the weight should be high where a name allows it | No. The person names carry no meaning |
| **Overlord, parent faction** | Tinctures, the principal charge, an ordinary, a chief or canton of allegiance | Per element with a kinship probability, as in FMG (§6.1): 0.25 for a town, more for the capital, less for a port or another race. Historical forms (§4.2): the lord's charge in other tinctures; the lord's tinctures with another charge; a chief of allegiance (§5, Italian); the lord's coat in a canton for a dependency | No. Factions and their relations are open |
| **Lineage** | Differences for branches; marshalling for marriages | By the tradition: a brisure by birth order, a bordure, a change of tincture, or nothing. A marriage impales for the lifetime of the couple. Only the children of an heiress, a woman without brothers, quarter. Quarters are capped at four with a selection rule (§7.7) | No. Families and their links are open |
| **Age** | The simplicity of the base coat; what has been added since | An old foundation draws from the low-budget layouts with few tinctures. Each generation, marriage or honour since can add one thing: a quarter, an augmentation, a chief. Two numbers, not one (§3.4) | No. A faction has no age |
| **Rank** | The exterior | A table per tradition. For the Armorial tradition: realm a crown, supporters, a compartment, a motto; great house a coronet, helm, crest, supporters; lesser house helm and crest; city a mural crown, supporters if it is the capital; town and guild the shield alone; a great guild a crest. Persons below the arms-bearing ranks have a badge or a house mark, or nothing | No. Rank is not modelled |
| **Supernatural Field** | Monsters as charges and supporters; an arcane material as a charge | Monster candidates only where the field is high at the seat. Fiction | Yes |
| **Deed** | One odd charge with a story | A small chance of a charge from none of the sources, with the provenance `Deed`. The authored legend explains it. History is full of such stories, mostly invented afterwards (§4.2) | Yes (a hash) |

Not every faction should bear arms. A kingdom, a noble house, a city, a guild and a religious house do. A warband or a tribe has a totem or a clan mark. A thieves' guild has a sign. So the pipeline should produce an *emblem* for every faction and a full achievement only for the classes and traditions that use one.

**A worked example.** A smiths' guild in a city on a navigable river. The realm of the region bears Gules, a tower or.

```text
tradition   armorial (Midlanders)
class       guild; budget 2
candidates  vocation Smith     → principal charge: hammer (weight 6), anvil (3)
            landscape, river   → ordinary: fess wavy (4)
            flora, oak timber  → secondary charge: acorn (1)
            overlord           → tinctures gules and or (kinship 0.35)
layout      an ordinary between charges
motifs      fess wavy; three hammers
tinctures   field gules (inherited); fess: metal, argent (or is kept for the charges);
            hammers: metal, or (inherited)
register    no clash in the region
```

```json
{
  "tradition": "armorial",
  "arms": { "coat": {
    "field": { "plain": "gules" }, "fieldProvenance": { "source": "Overlord", "reference": "R(0, 0)#0" },
    "elements": [
      { "ordinary": "fess", "line": "wavy", "tincture": { "plain": "argent" }, "count": 1,
        "provenance": { "source": "Landscape", "reference": "river" } },
      { "charge": "hammer", "count": 3, "arrangement": "aroundPrincipal", "tincture": "or",
        "provenance": { "source": "Vocation", "reference": "smith" } }
    ] } },
  "exterior": { "motto": false },
  "badge": { "charge": "hammer", "count": 1, "tincture": "or" },
  "liveryColour": "gules", "liveryMetal": "or"
}
```

Blazon text: *Gules, a fess wavy argent between three hammers or.* Plain description: a red shield with a wavy white band across the middle and three gold hammers, two above and one below.

### 7.7 Complexity and readability

```text
budget = base(tradition) + ageTerm + rankTerm + richTerm      clamped to 1 … 4
    ageTerm:  −1 for the oldest foundations, 0, +1 for the youngest
    rankTerm: +1 for a realm or a great house
    richTerm: +1 for a Rich Prospect, −1 for a Poor one (cities and towns)
```

Limits that hold whatever the budget. They are design judgement, set from the Dering Roll figures (§3.4) and the Nordic rules (§4.3):

| Limit | Value |
|---|---|
| Tinctures on one coat, not counting attributes and proper | 2 at budget 1, 3 otherwise |
| Kinds of charge on one coat | 1 at budget 1 or 2, 2 otherwise, plus one on a chief or canton |
| Count of one charge | 1, 2, 3, 4, 5, 6 or semé |
| Lines other than straight | at most 1 per coat |
| Nesting | a charge on an ordinary; nothing on that charge |
| Quarters | at most 4, with no grand quarters by default. When more are inherited: the paternal coat first, the most recent heiress last, the highest-ranked in between |
| Letters and numerals | none, except in a tradition that allows them (the Seal tradition) |

Three levels of detail, all derived from one achievement: the livery (two colours) for map borders and dress; the badge or the plain shield for icons; the full achievement for a detail view.

### 7.8 Validity

**The rule of tincture as a check on the layer tree.** For every element and what lies directly under it:

```text
ok(upper, lower) =
       class(upper) ≠ class(lower)                 # metal on colour, colour on metal
    or upper or lower is a fur or proper
    or lower is a parted, varied or semé field
    or upper is an attribute, a brisure or an augmentation
```

The generator does not test and retry. It draws the upper tincture from the allowed class, as Armoria does (§6.1), so the rule holds by construction. A tradition can set a breach rate; 1 to 2 % matches the record (§3.1). Stains count as colours and stay out of the default palette (§2.2).

**Uniqueness.** The check needs a jurisdiction and an order, and both must be local for the result to be independent of the order regions are generated in.

- **Jurisdiction: the region.** The readme defines a faction as a group in one region, and the region is the generation unit. All factions of a region get their arms in the Planned stage and all cities and towns in Settled.
- **Order: rank class, then slot index.** The realm first, then houses, cities, towns and guilds. A later bearer that clashes with an earlier one is the one that changes.
- **Clash:** the canonical forms are equal. The canonical form is the `Arms` tree with the provenance left out.
- **Too close** (optional): the two coats differ in fewer than two of: field, principal element, its tincture, its count or line, the secondary element. Unrelated bearers should differ in at least two; a cadet differs from the head in exactly one. This is a design rule, not a sourced one.
- **On a clash, difference as history did** (§2.8): first change one tincture, then a line, then add a secondary charge from the pool, then a bordure. Each step is a draw with the provenance `Difference`.

Across regions no check is possible without reading regions that may not exist. Three answers are in §8. The historical record supports the cheapest one: identical simple arms did arise independently, and Scrope and Carminow both kept theirs (§3.3).

For families the count is large and generation is on demand. Two ways to keep it deterministic: the arms of slot *n* are checked against slots 0 to *n* − 1 of the same parent, which means computing those first; or only families above a rank bear arms at all, which keeps the set small. The Clan mark tradition avoids the problem: families share the clan's emblem.

### 7.9 Determinism

- **A pure function of the foundation and the bearer's Stub.** No arms read anything authored, and nothing reads the arms of a region that is not at the stage the reader's own stage already requires.
- **Purposes.** One purpose number per step of §7.5, so that a change to the charge pool does not move the tinctures. The society uses `0x4a01` to `0x4a03`; a free block such as `0x4b01` to `0x4b0c` would fit. The numbers must be assigned in `society-generation.md`, not here.
- **Seeding.** `Hash4` takes two integers, and a slot identity has three or more parts (the parent's coordinate and the slot index). A sequence per step can be seeded by nesting: `Sfc32(Hash4(Hash4(seed, purpose, i, j), purpose, slot, kind))`, consumed in a fixed order, as the readme describes for sequences. This is the first place the society needs it, so the rule for hashing a slot identity is worth deciding once for all entities.
- **Weighted draws** walk their candidates in a fixed order (the order of the Parameter Set list or of the catalogue) and take the first whose running sum passes `u · total`, as section 3.3 of the society document does.
- **Inheritance across a region border** needs the parent's arms. If the parent is a faction of a neighbouring region, its arms exist only when that region is Planned. The Settled stage already requires all bordering regions Planned, so cross-border inheritance fits there and not in Planned. An alternative is a livery per region that is a pure function of the seed and the region coordinate, like the Sited stage: any neighbour can read it at any time. It cannot follow the race, because the races of a region are a Planned fact.
- **Stages.** Faction arms in Planned read the final biomes and resources of the Elevated stage, which Planned already requires. Travel links are Settled, so a bridge or a wheel from the travel network can only go on arms made in Settled: cities and towns, not factions.
- **Tests.** The existing order-independence test should cover arms: generate a block of regions in two orders and compare every achievement.
- **Lists.** Adding a charge to a catalogue changes weighted draws, so the catalogues are part of the Parameter Set or of the generator version, as the race list is.

### 7.10 Rendering

**Blazon text.** A pre-order walk of the `Arms` tree in the order of §3.2, with a small set of text rules: the name of a roundel by its tincture (§2.4), "two and one" left unsaid for three charges (§2.6), the plural of each charge, the attitude after the charge, the tincture last. I would not implement "of the first" and "of the field": modern blazons often repeat the tincture, and repetition is clearer for a player. A second writer produces the plain description. Both are a few hundred lines and need no assets.

**SVG.** The repo draws maps on its own raster `Canvas`; arms want vectors, and SVG is text, so the renderer is a pure function to a string and the demo app can show it as is. The parts:

| Part | How |
|---|---|
| Shield | One outline path per shape, used as a clip path |
| Field | A fill; a division as polygons clipped to the shield; a variation or a semé as an SVG pattern |
| Lines of partition | One path fragment per line, repeated along the edge of a division or an ordinary. Armoria stores its lines this way [ARM-DATA] |
| Ordinaries | A parametric shape per ordinary, with the line applied to its edges |
| Charges | One SVG symbol per charge and attitude, with up to three colour slots (body, armed, langued). Armoria uses up to three colours per charge [ARM-CHG]; the Paradox format uses a mask with three colours [CK3-COA] |
| Layout | A table per shield shape and arrangement giving position and scale for each count. Armoria's position grid and size rule are a tested starting point [ARM-SHD] [ARM-GEN] |
| Exterior | Fixed frames per tradition and rank, coloured with the livery |

DrawShield's API can serve as an oracle while the renderer is built: send it our blazon text and compare the pictures (§6.2).

**Painted form.** Optional and authored: a diffusion model paints the achievement from the blazon text with the flat SVG as a guide image. It is stored like a portrait and must not contradict the Stub. This note did not test whether an image model keeps a blazon accurate.

**Assets** (§6.5). The clean sources for a game that may be sold are: Armoria's CC0 charges, filtered by the licence attribute in each file; game-icons.net under CC BY 3.0 for tools and objects; Commons files that are public domain or CC BY, checked one by one; and charges drawn for the project. The geometry (shields, divisions, ordinaries, lines) needs no library. The WappenWiki-derived charges in Armoria are non-commercial and must be left out. GPL code (DrawShield, Heraldicon) can be read but not copied.

---

## 8. Open questions for the user

**Domain model**

1. **Is a noble house a faction or a family?** The readme has both concepts: a faction is a political grouping in one region, a family is a household of NPCs. Arms are historically borne by a lineage. Cadency and marshalling need family links; inheritance from an overlord needs faction relations. The task named "guilds, kingdoms, families" together as factions. The readme and `society-generation.md` agree with each other here; neither says which of the two a house is.
2. **Who bears an emblem at all?** Every faction, or only some classes? Every family, or only those above a rank? The answer bounds the uniqueness problem (§7.8).
3. **Does an achievement belong to the Stub?** This note proposes yes, as derived data (§7.2). `research/factions.md` §5.1 listed heraldry as authored text. If the user prefers authored arms, inheritance and uniqueness fall away and the rest of §7 shrinks to a prompt for the LLM.
4. **Rank and age** are needed for the exterior and for complexity, and neither exists. Are they facts of a faction Stub? The four ages of `races-basis.md` bind nothing; should a faction have a founding age?
5. **Religion.** Patron saints are a main source of civic and guild arms (§4.2), and the church is a class of bearer (§4.3). The repo models no gods. Leave the source out, or wait for a decision on religion?

**Names**

6. **Canting.** At least a fifth of historical arms pun on the name, and the person-name generator produces names without meaning. Which way round: (a) a new name generator for factions, cities and families that builds names from meaningful parts, each mapped to a charge; (b) arms first and the name from the principal charge ("the Three Hammers", "Oakford"); (c) a sound match of generated names against charge words; or no canting until names are specified? Option (b) needs no new input and suits inns, guilds, towns and clans.

**Lists and where they live**

7. **The emblem fauna.** The readme says a biome drives flora and fauna, and `map-generation.md` has no fauna list (§7.1). Should a fauna list per biome be added to the map document, to the society document, or to the Parameter Set? It would also serve the looks of a place and hunting. This is a gap between the readme and the map document, recorded here and not resolved.
8. **Traditions.** A list in the Parameter Set with an id on each race, like the planned factor per vocation? Or a fixed table in the generation code?
9. **The charge catalogue.** Charges, attitudes, ordinaries and lines as a Parameter Set list, or as data embedded in the Generation assembly like the name models? It will have hundreds of entries, and the artwork must match it one to one.
10. **Charges per vocation.** Add a list of charges to each vocation record when section 8 of the society document is specified?

**Rules**

11. **Uniqueness across regions.** (a) Accept that two regions can hold the same simple arms, as history did; (b) make it unlikely with a region-specific lean on palette and motifs; (c) add a pass in the Settled stage that compares realm-level arms with the bordering regions and differences by a fixed rule. Option (c) means the realm arms of a Planned region can still change when it is Settled, unless they are only fixed in Settled.
12. **Factions that span regions.** `research/factions.md` §5.5 asks the same for guild networks and orders. If a network gets a key that is a pure function of the foundation, its arms can be a function of that key, and each chapter adds a local difference.
13. **Overlords across a border.** Planned-stage arms can only inherit within the region. Inheritance from a neighbour fits the Settled stage (§7.9). Is that acceptable, or should a livery per region be a Sited-level function?
14. **How strict?** Should a learned player find the arms correct by the rules of §3, or is a loose, readable fantasy style enough? This decides whether the stains, the furs, the cadency systems and the blazon conventions are worth their cost.
15. **How far do the races diverge?** Twelve traditions (§7.4) are a lot of catalogue and artwork. A first version could ship the Armorial, Clan mark and Roundel crest traditions and map every race onto one of the three.

**Presentation**

16. **Language of the blazon.** Classic Anglo-Norman blazon, plain language, or both? A world of 28 races has no reason to blazon in Norman French, but the vocabulary is what players of the genre recognise.
17. **Artwork.** Flat SVG from a licensed or commissioned charge set, or a painted emblazonment by a diffusion model on top of it? The second is authored data and costs per coat.
18. **Mottoes.** Authored by an LLM from the provenance, or left out? In the record a motto is the least fixed part (§2.10).

**Gaps in this research**

- The College of Arms site refused the fetch tool, so nothing is cited from it.
- Boutell could not be opened, and Woodward & Burnett was read in parts only.
- Pastoureau's books were not readable; his figures come from two articles and from Velde's summary.
- The Danish order on municipal arms was read as an English summary, and no Danish design rules were found. The Norwegian rule was read from an encyclopedia, not the regulation. Nothing official was found for Finland.
- CK3's own random generation is not documented on its wiki; Imperator's is.
- The non-European parallels beyond mon and tamga (Mamluk emblems, West African and Indian emblem traditions, Chinese rank badges) were not researched, so six of the twelve traditions in §7.4 have no cited parallel.
- No source was found that states the visibility rationale of the tincture rule in so many words.

---

## 9. Sources

Read means the page or file was opened in this pass, mostly through the summarising fetch tool. Partly means only part of a long text was read.

### 9.1 Treatises and reference works

| Key | Source | Read |
|---|---|---|
| FD, FD1 … FD38 | A. C. Fox-Davies, *A Complete Guide to Heraldry* (1909), Wikisource, by chapter; also the Project Gutenberg text no. 41617 for passages cited as [FD] without a chapter | Chapters 1, 2, 5 to 9, 11, 12, 14, 15, 19 to 34, 37, 38 |
| PK-A … PK-T | James Parker, *A Glossary of Terms Used in Heraldry* (1894), heraldsnet.org, by letter | Entries cited |
| WB1, WB2 | J. Woodward and G. Burnett, *A Treatise on Heraldry British and Foreign* (1892), archive.org, OCR | Partly |
| BPA | A. C. Fox-Davies, *The Book of Public Arms* (1915), archive.org, OCR | Partly |
| CE | A. C. Fox-Davies, "Ecclesiastical Heraldry", *Catholic Encyclopedia* (1910) | Read |
| ROUND | J. H. Round, *Geoffrey de Mandeville* (1892), archive.org, OCR | Partly |
| BART | Bartolus de Saxoferrato, *Tractatus de insigniis et armis*, ed. F. Hauptmann (Bonn, 1883), archive.org, Latin OCR. The English glosses in §4.4 are not a published translation | Partly |
| SICILE | *Le Blason des couleurs*, ed. H. Cocheris (1860), archive.org, OCR | Partly |
| EB | *Encyclopaedia Britannica*, "Heraldry" | Read |

### 9.2 Authorities and societies

| Key | Source | Read |
|---|---|---|
| LL-B, LL-FAQ | Court of the Lord Lyon, "Blazoning" and FAQ | Read |
| LL-1672 | Lyon King of Arms Act 1672, legislation.gov.uk | Read |
| HS-FAQ | The Heraldry Society, FAQ | Read, and checked a second time |
| HS-BEG | The Heraldry Society, "As it was in the beginning" | Read |
| HS-HHH | The Heraldry Society, *Historic Heraldry Handbook* (2018) | Read |
| HS-CL | Pardo de Vera y Díaz, Heraldry Society paper (2023) on the arms of Castile and León | Read |
| HS-LAW | G. D. Squibb, "The Law of Arms in England", *The Coat of Arms* no. 15 (1953) | Read |
| HS-COA | The Heraldry Society, "The College of Arms" | Read |
| CHA | Canadian Heraldic Authority, FAQ | Read |
| RDK | Rigsarkivet, "Heraldik" (Statens Heraldiske Konsulent) | Read |
| DK-600 | Bekendtgørelse nr. 600 af 12. juni 2007 om registrering af kommunevåben og -segl, as summarised on WIPO Lex | Summary only |
| RSE | Riksarkivet (Sweden), "Vägledning om kommunvapen" | Read |
| SNL | *Store norske leksikon*, "kommunevåpen" | Read |

### 9.3 Scholarship and secondary sources

| Key | Source | Read |
|---|---|---|
| PAST-P | M. Pastoureau, "Une écriture en images : les armoiries parlantes", *Extrême-Orient Extrême-Occident* 30 (2008) | Read |
| PAST-L | M. Pastoureau, "Quel est le roi des animaux ?", Actes de la SHMESP 15 (1984), p. 133 | One page |
| PAST-U | M. Pastoureau, "Héraldique: les règles du blason", *Encyclopædia Universalis* | Read |
| VELDE-O, VELDE-M, VELDE-T, VELDE-B, VELDE-HU | F. Velde, heraldica.org: "Origins of Armory", "Meaning of Coats of Arms", "The Tincture Rule", "Bartolo's De Insigniis et Armis", "Hungary". Secondary; the first is a summary of Pastoureau's *Traité d'héraldique* | Read; the tincture page checked a second time |
| LONDON | H. S. London, "Some medieval treatises on English heraldry" (1953), on heraldica.org | Read |
| ROM | Note in *Romania* 112 (1991) on a heraldic treatise | Read |
| ERIS | Erisman, "Canting arms: a comparison of two regional styles" (2008) | Read |
| DERING | K. Rhodes, analysis of the Dering Roll | Read |
| BL-DER | British Library, "The Dering Roll" | Read |
| ECOD | e-codices, the Zürich armorial (Swiss National Museum AG 2760) | Read |
| BAJER | P. Bajer, "Polish Nobility and Its Heraldry: An Introduction" | Read |
| NIPPON | nippon.com, "Kamon: Japan's Family Crests" (2023) | Read, and checked a second time |
| TAMGA | Bravina, Popova and Vasiliev, "Tamgas of the 19th and Early 20th Century Yakuts", *Journal of Siberian Federal University* 14(8), 2021 | Read |

### 9.4 Generators, parsers and artwork

| Key | Source | Read |
|---|---|---|
| ARM-README, ARM-GEN, ARM-DEF, ARM-DATA, ARM-ORD, ARM-CHG, ARM-SHD, ARM-SVG | Azgaar, Armoria, branch `master`: README, `src/scripts/generator.js`, `src/config/defaults.ts`, `src/data/dataModel.js`, `ordinaries.js`, `charges.js`, `shields.ts`, one charge SVG | Read; `defaults.ts` checked a second time; three files partly |
| ARM-API | Azgaar, armoria-api, README | Read |
| FMG-GEN, FMG-TYPES, FMG-BURG, FMG-PROV, FMG-MAP, FMG-CULT, FMG-SHD, FMG-LIC | Azgaar, Fantasy Map Generator, branch `master`: `emblems-generator.ts`, `types/emblems.ts`, `burgs-generator.ts`, `provinces-generator.ts`, `type-mapping.ts`, `cultures-generator.ts`, `shields.ts`, LICENSE | Read; `provinces-generator.ts` as matching lines only |
| DS-README, DS-WIKI, DS-PARSER, DS-LEX, DS-FAQ, DS-API, DS-RAND, DS-BP | DrawShield: README, wiki, parser page, English charge lexemes, FAQ, API page, `randomblazon.php`, Blazon-Parser README | Read |
| CK3-COA, CK3-DYN | Crusader Kings III wiki, "Coat of arms modding", "Dynasties modding" | Read |
| IMP-COA | Imperator: Rome wiki, "Coat of arms modding" | Read |
| HCON-README, HCON-GR, HCON-ATTR | Heraldicon: README, `blazonry/grammar.ebnf`, attribution model | Read; the grammar partly |
| THP | the-herald-playground, README | Read |
| WW | WappenWiki, main page footer | Footer only |
| COMMONS, COMMONS-COA | Wikimedia Commons, "SVG coat of arms elements"; "Commons:Coats of arms" | Read |
| SODACAN | Wikimedia Commons, one file by Sodacan | Read |
| BTHA | Book of Traceable Heraldic Art, credits page | Read |
| VAL | Viking Answer Lady, heraldry SVG images | Read |
| MIST | Mistholme, *Pictorial Dictionary of Heraldry* | Read |
| GI | game-icons.net, about page | Read |

[FD]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry
[FD1]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry/Chapter_1
[FD5]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry/Chapter_5
[FD6]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry/Chapter_6
[FD7]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry/Chapter_7
[FD8]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry/Chapter_8
[FD9]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry/Chapter_9
[FD11]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry/Chapter_11
[FD12]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry/Chapter_12
[FD14]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry/Chapter_14
[FD15]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry/Chapter_15
[FD19]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry/Chapter_19
[FD20]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry/Chapter_20
[FD21]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry/Chapter_21
[FD22]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry/Chapter_22
[FD23]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry/Chapter_23
[FD24]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry/Chapter_24
[FD25]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry/Chapter_25
[FD26]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry/Chapter_26
[FD27]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry/Chapter_27
[FD28]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry/Chapter_28
[FD29]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry/Chapter_29
[FD30]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry/Chapter_30
[FD31]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry/Chapter_31
[FD32]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry/Chapter_32
[FD33]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry/Chapter_33
[FD34]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry/Chapter_34
[FD37]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry/Chapter_37
[FD38]: https://en.wikisource.org/wiki/A_Complete_Guide_to_Heraldry/Chapter_38
[PK-A]: https://www.heraldsnet.org/saitou/parker/Jpglossa.htm
[PK-B]: https://www.heraldsnet.org/saitou/parker/Jpglossb.htm
[PK-C]: https://www.heraldsnet.org/saitou/parker/Jpglossc.htm
[PK-D]: https://www.heraldsnet.org/saitou/parker/Jpglossd.htm
[PK-F]: https://www.heraldsnet.org/saitou/parker/Jpglossf.htm
[PK-T]: https://www.heraldsnet.org/saitou/parker/Jpglosst.htm
[WB1]: https://archive.org/details/treatiseonherald01wood
[WB2]: https://archive.org/details/treatiseonherald02wood
[BPA]: https://archive.org/details/cu31924029798927
[CE]: https://www.newadvent.org/cathen/07243a.htm
[ROUND]: https://archive.org/details/bwb_S0-ABE-763
[BART]: https://archive.org/details/tractatusdeinsig00bart
[SICILE]: https://archive.org/details/leblasondescoul00sicigoog
[EB]: https://www.britannica.com/topic/heraldry/The-nature-and-origins-of-heraldic-terminology
[LL-B]: https://www.courtofthelordlyon.scot/blazoning.htm
[LL-FAQ]: https://www.courtofthelordlyon.scot/faqs.htm
[LL-1672]: https://www.legislation.gov.uk/aosp/1672/47
[HS-FAQ]: https://www.theheraldrysociety.com/about-heraldry/frequently-asked-questions/
[HS-BEG]: https://www.theheraldrysociety.com/articles/as-it-was-in-the-beginning/
[HS-HHH]: https://www.theheraldrysociety.com/wp-content/uploads/2018/03/Historic-Heraldry-Handbook.pdf
[HS-CL]: https://www.theheraldrysociety.com/wp-content/uploads/2023/10/34.-Pardo-de-Vera-y-Diaz.pdf
[HS-LAW]: https://www.theheraldrysociety.com/articles/the-law-of-arms-in-england/
[HS-COA]: https://www.theheraldrysociety.com/articles/the-college-of-arms/
[CHA]: https://gg.ca/en/heraldry/faq
[RDK]: https://www.rigsarkivet.dk/heraldik/
[DK-600]: https://www.wipo.int/wipolex/en/legislation/details/19709
[RSE]: https://riksarkivet.se/resurser/vagledning-om-kommunvapen
[SNL]: https://snl.no/kommunev%C3%A5pen
[PAST-P]: https://journals.openedition.org/extremeorient/110
[PAST-L]: https://www.persee.fr/doc/shmes_1261-9078_1985_act_15_1_1442
[PAST-U]: https://www.universalis.fr/encyclopedie/heraldique/2-les-regles-du-blason/
[VELDE-O]: https://www.heraldica.org/topics/origins.htm
[VELDE-M]: https://www.heraldica.org/topics/meaning.htm
[VELDE-T]: https://heraldica.org/topics/tinctrul.htm
[VELDE-B]: https://www.heraldica.org/topics/bartolo.htm
[VELDE-HU]: https://www.heraldica.org/topics/national/hungary.htm
[LONDON]: https://www.heraldica.org/topics/britain/London(1953).htm
[ROM]: https://www.persee.fr/doc/roma_0035-8029_1991_num_112_447_1680
[ERIS]: https://www.ellipsis.cx/~liana/latex/kw/gwenllian/gwenllian.pdf
[DERING]: https://herald.poore-house.com/design/armory/deringroll/
[BL-DER]: https://support.bl.uk/Page/The-Dering-Roll
[ECOD]: https://www.e-codices.unifr.ch/en/list/one/snm/AG002760
[BAJER]: https://pgsa.org/wp-content/uploads/2025/07/PL-Nobility-Heraldry-Intro.pdf
[NIPPON]: https://www.nippon.com/en/japan-data/h01578
[TAMGA]: https://cyberleninka.ru/article/n/tamgas-of-the-19th-and-early-20th-century-yakuts-sakha
[ARM-README]: https://raw.githubusercontent.com/Azgaar/Armoria/master/README.md
[ARM-GEN]: https://raw.githubusercontent.com/Azgaar/Armoria/master/src/scripts/generator.js
[ARM-DEF]: https://raw.githubusercontent.com/Azgaar/Armoria/master/src/config/defaults.ts
[ARM-DATA]: https://raw.githubusercontent.com/Azgaar/Armoria/master/src/data/dataModel.js
[ARM-ORD]: https://raw.githubusercontent.com/Azgaar/Armoria/master/src/data/ordinaries.js
[ARM-CHG]: https://raw.githubusercontent.com/Azgaar/Armoria/master/src/data/charges.js
[ARM-SHD]: https://raw.githubusercontent.com/Azgaar/Armoria/master/src/data/shields.ts
[ARM-SVG]: https://raw.githubusercontent.com/Azgaar/Armoria/master/public/charges/lionRampant.svg
[ARM-API]: https://github.com/Azgaar/armoria-api
[FMG-GEN]: https://raw.githubusercontent.com/Azgaar/Fantasy-Map-Generator/master/src/model/political/emblems-generator.ts
[FMG-TYPES]: https://raw.githubusercontent.com/Azgaar/Fantasy-Map-Generator/master/src/types/emblems.ts
[FMG-BURG]: https://raw.githubusercontent.com/Azgaar/Fantasy-Map-Generator/master/src/model/political/burgs-generator.ts
[FMG-PROV]: https://raw.githubusercontent.com/Azgaar/Fantasy-Map-Generator/master/src/model/political/provinces-generator.ts
[FMG-MAP]: https://raw.githubusercontent.com/Azgaar/Fantasy-Map-Generator/master/src/data/emblems/type-mapping.ts
[FMG-CULT]: https://raw.githubusercontent.com/Azgaar/Fantasy-Map-Generator/master/src/model/society/cultures-generator.ts
[FMG-SHD]: https://raw.githubusercontent.com/Azgaar/Fantasy-Map-Generator/master/src/data/emblems/shields.ts
[FMG-LIC]: https://raw.githubusercontent.com/Azgaar/Fantasy-Map-Generator/master/LICENSE
[DS-README]: https://raw.githubusercontent.com/drawshield/Drawshield-Code/stable/README.md
[DS-WIKI]: https://github.com/drawshield/Drawshield-Code/wiki
[DS-PARSER]: https://github.com/drawshield/Drawshield-Code/wiki/The-Parser
[DS-LEX]: https://raw.githubusercontent.com/drawshield/Drawshield-Code/stable/parser/english/lexemes/charges.txt
[DS-FAQ]: https://drawshield.net/docs/FAQs.html
[DS-API]: https://github.com/drawshield/Drawshield-Code/wiki/The-DrawShield-API
[DS-RAND]: https://raw.githubusercontent.com/drawshield/Drawshield-Code/stable/randomblazon.php
[DS-BP]: https://raw.githubusercontent.com/drawshield/Blazon-Parser/master/README.md
[CK3-COA]: https://ck3.paradoxwikis.com/Coat_of_arms_modding
[CK3-DYN]: https://ck3.paradoxwikis.com/Dynasties_modding
[IMP-COA]: https://imperator.paradoxwikis.com/Coat_of_arms_modding
[HCON-README]: https://github.com/heraldry/heraldicon
[HCON-GR]: https://raw.githubusercontent.com/heraldry/heraldicon/main/blazonry/grammar.ebnf
[HCON-ATTR]: https://raw.githubusercontent.com/heraldry/heraldicon/main/src/heraldicon/entity/attribution.cljs
[THP]: https://raw.githubusercontent.com/vincent-psarga/the-herald-playground/main/README.md
[WW]: http://wappenwiki.org/index.php/Main_Page
[COMMONS]: https://commons.wikimedia.org/wiki/Category:SVG_coat_of_arms_elements
[COMMONS-COA]: https://commons.wikimedia.org/wiki/Commons:Coats_of_arms
[SODACAN]: https://commons.wikimedia.org/wiki/File:Royal_Coat_of_Arms_of_the_United_Kingdom.svg
[BTHA]: https://heraldicart.org/credits
[VAL]: http://www.vikinganswerlady.com/Stars/Heraldry_SVG_Images/index.htm
[MIST]: https://mistholme.com/pictorial-dictionary-of-heraldry/
[GI]: https://game-icons.net/about.html
