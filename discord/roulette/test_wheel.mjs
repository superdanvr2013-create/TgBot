// Самопроверка генерации кадров рулетки: 6 участников, 2 кадра (старт + финал), контроль победителя.
import { writeFileSync } from "node:fs";
import { drawWheelFrame, planFrames, segmentAtPointer } from "./wheel.mjs";

const names = ["danvr2013", "User2", "User3", "User4", "User5", "User6"];
const winIndex = 3; // User4 должен оказаться под стрелкой в финальном кадре

const { frames, shares } = planFrames(names, winIndex, 16);
console.log("frames:", frames.length, "shares:", shares.map((s) => s.toFixed(3)).join(","));

// стартовый кадр
writeFileSync(new URL("./frame_start.png", import.meta.url), drawWheelFrame(names, 0, shares));
// финальный кадр
const last = frames[frames.length - 1];
writeFileSync(new URL("./frame_final.png", import.meta.url), drawWheelFrame(names, last.spin, shares));

// проверка: в финале под стрелкой должен быть winIndex
import { fileURLToPath } from "node:url";
const finalIdx = segmentAtPointer(shares, last.spin);
console.log("expected winner:", winIndex, "| segment at pointer (final):", finalIdx);
console.log(finalIdx === winIndex ? "PASS" : "FAIL");