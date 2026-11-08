import { countAbuse } from "./moderation.mjs";

const cases = [
  ["ты идиот", 1],
  ["Привет, как дела?", 0],
  ["бот привет", 0],
  ["ты тупой кретин и дурак", 4],
  ["нормальное сообщение без мата", 0],
  ["ты мразь", 1],
];

let pass = 0;
for (const [text, expected] of cases) {
  const got = countAbuse(text);
  const ok = got === expected;
  if (ok) pass++;
  console.log(ok ? "PASS" : "FAIL", JSON.stringify(text), "→", got, "(expected", expected + ")");
}
console.log(pass === cases.length ? "ALL PASS" : "SOME FAILED");