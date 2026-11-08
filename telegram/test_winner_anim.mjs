// Самотест: генерация PNG-победителя.
import { writeFileSync } from "node:fs";
import { makeWinnerBanner } from "./winner_anim.mjs";

const png = makeWinnerBanner("danvr2013");
writeFileSync(new URL("./winner_test.png", import.meta.url), png);
console.log("png bytes:", png.length);
