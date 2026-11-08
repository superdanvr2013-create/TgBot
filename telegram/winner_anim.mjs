// Генерация победного баннера (PNG) — большой цветной ник с конфетти.
import { createCanvas } from "@napi-rs/canvas";

const COLORS = ["#FEE75C", "#ED4245", "#57F287", "#5865F2", "#EB459E", "#F47BFF"];

function mulberry32(a) {
  return function () {
    a |= 0; a = (a + 0x6d2b79f5) | 0;
    let t = Math.imul(a ^ (a >>> 15), 1 | a);
    t = (t + Math.imul(t ^ (t >>> 7), 61 | t)) ^ t;
    return ((t ^ (t >>> 14)) >>> 0) / 4294967296;
  };
}

function drawConfetti(ctx, w, h) {
  const rnd = mulberry32(99);
  for (let i = 0; i < 90; i++) {
    const x = rnd() * w;
    const y = rnd() * h;
    const s = 4 + rnd() * 8;
    const rot = rnd() * Math.PI;
    ctx.save();
    ctx.translate(x, y);
    ctx.rotate(rot);
    ctx.fillStyle = COLORS[Math.floor(rnd() * COLORS.length)];
    ctx.fillRect(-s / 2, -s / 4, s, s / 2);
    ctx.restore();
  }
}

/** Возвращает Buffer PNG — баннер победителя. */
export function makeWinnerBanner(name) {
  const w = 800, h = 400;
  const canvas = createCanvas(w, h);
  const ctx = canvas.getContext("2d");

  ctx.fillStyle = "#2b2d31";
  ctx.fillRect(0, 0, w, h);
  drawConfetti(ctx, w, h);

  ctx.textAlign = "center";
  ctx.textBaseline = "middle";
  ctx.fillStyle = "#FEE75C";
  ctx.font = "bold 56px 'Segoe UI', 'DejaVu Sans', sans-serif";
  ctx.fillText("ПОБЕДИТЕЛЬ", w / 2, 90);

  ctx.fillStyle = COLORS[1]; // красный акцент для победителя
  ctx.font = "bold 72px 'Segoe UI', 'DejaVu Sans', sans-serif";
  ctx.fillText(name, w / 2, 210);

  ctx.fillStyle = "#b5bac1";
  ctx.font = "28px 'Segoe UI', 'DejaVu Sans', sans-serif";
  ctx.fillText("ЧАТ-РУЛЕТКА DAN GAMES", w / 2, 320);

  return canvas.encodeSync("png");
}
