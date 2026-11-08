// Рулетка-колесо: генерация PNG-кадров через @napi-rs/canvas (prebuilt, без нативной сборки).
// Рендерит цветные сегменты с именами участников, стрелку-указатель сверху и пропеллер в центре.
import { createCanvas } from "@napi-rs/canvas";

const SEGMENT_COLORS = [
  "#5865F2", "#57F287", "#FEE75C", "#ED4245", "#EB459E",
  "#F47BFF", "#F8B26A", "#3BA55D", "#4E5D94", "#E78F45",
  "#9B59B6", "#1ABC9C", "#E67E22", "#3498DB", "#95A5A6",
];

export const WHEEL_FONT = "bold 24px 'Segoe UI', 'DejaVu Sans', sans-serif";

// Возвращает пропорции (0..1) ширины имён относительно окружности — используется для расчёта углов.
function measureShares(names) {
  const c = createCanvas(16, 16);
  const ctx = c.getContext("2d");
  ctx.font = WHEEL_FONT;
  const widths = names.map((n) => ctx.measureText(n).width + 44);
  const total = widths.reduce((a, b) => a + b, 0);
  return widths.map((w) => w / total);
}

/**
 * Генерирует один кадр колеса.
 * @param {string[]} names   имена участников
 * @param {number} spinAngle угол поворота колеса (рад), сегмент победителя = names[winIndex] окажется под стрелкой при финальном угле
 * @param {number[]} shares  пропорции сегментов (0..1), из measureShares
 * @returns {Buffer} PNG
 */
export function drawWheelFrame(names, spinAngle, shares) {
  const size = 600;
  const center = size / 2;
  const radius = 260;
  const canvas = createCanvas(size, size);
  const ctx = canvas.getContext("2d");

  // фон
  ctx.fillStyle = "#2b2d31";
  ctx.fillRect(0, 0, size, size);

  // обводка колеса
  ctx.beginPath();
  ctx.arc(center, center, radius + 18, 0, Math.PI * 2);
  ctx.fillStyle = "#1e1f22";
  ctx.fill();
  ctx.lineWidth = 4;
  ctx.strokeStyle = "#5865F2";
  ctx.stroke();

  // сегменты
  let ang = spinAngle;
  const angles = [];
  for (let i = 0; i < names.length; i++) {
    const sweep = shares[i] * Math.PI * 2;
    angles.push({ start: ang, end: ang + sweep });
    ctx.beginPath();
    ctx.moveTo(center, center);
    ctx.arc(center, center, radius, ang, ang + sweep);
    ctx.closePath();
    ctx.fillStyle = SEGMENT_COLORS[i % SEGMENT_COLORS.length];
    ctx.fill();
    ctx.lineWidth = 3;
    ctx.strokeStyle = "#1e1f22";
    ctx.stroke();

    // имя — по дуге сегмента, читается слева направо
    const mid = ang + sweep / 2;
    ctx.save();
    ctx.translate(center, center);
    ctx.rotate(mid);
    ctx.textAlign = "center";
    ctx.textBaseline = "middle";
    ctx.fillStyle = "#ffffff";
    ctx.font = WHEEL_FONT;
    ctx.fillText(names[i], radius * 0.62, 0);
    ctx.restore();
    ang += sweep;
  }

  // пропеллер в центре
  ctx.beginPath();
  ctx.arc(center, center, 26, 0, Math.PI * 2);
  ctx.fillStyle = "#1e1f22";
  ctx.fill();
  ctx.lineWidth = 4;
  ctx.strokeStyle = "#FEE75C";
  ctx.stroke();
  for (let b = 0; b < 3; b++) {
    const ba = spinAngle * 2 + (b * Math.PI * 2) / 3;
    ctx.beginPath();
    ctx.moveTo(center, center);
    ctx.lineTo(center + Math.cos(ba) * 20, center + Math.sin(ba) * 20);
    ctx.lineWidth = 6;
    ctx.strokeStyle = "#FEE75C";
    ctx.stroke();
  }

  // стрелка сверху (указатель)
  const arrowY = center - radius - 18;
  ctx.beginPath();
  ctx.moveTo(center, arrowY + 26);
  ctx.lineTo(center - 16, arrowY - 8);
  ctx.lineTo(center + 16, arrowY - 8);
  ctx.closePath();
  ctx.fillStyle = "#FEE75C";
  ctx.fill();
  ctx.lineWidth = 3;
  ctx.strokeStyle = "#1e1f22";
  ctx.stroke();

  return canvas.encodeSync("png");
}

/**
 * Планирует кадры так, чтобы последний кадр остановился на победителе.
 * Возвращает массив { spin, delayBefore } — delayBefore задаётся при редактировании (easing).
 */
export function planFrames(names, winIndex, totalFrames = 16) {
  const shares = measureShares(names);
  // угол центра сегмента победителя в кадре 0 (без поворота)
  let start = 0;
  const centers = shares.map((sh) => {
    const c = start + (sh * Math.PI * 2) / 2;
    start += sh * Math.PI * 2;
    return c;
  });
  // стрелка смотрит вверх = -PI/2; колесо крутится, сегмент должен оказаться под стрелкой
  const targetSpin = -Math.PI / 2 - centers[winIndex];
  const fullTurns = Math.PI * 2 * 5; // 5 оборотов до остановки
  const totalSpin = fullTurns + (targetSpin % (Math.PI * 2) + Math.PI * 2) % (Math.PI * 2);

  // ease-out cubic: spin(t) = totalSpin * (1 - (1-t)^3)
  const frames = [];
  for (let k = 0; k < totalFrames; k++) {
    const t = (k + 1) / totalFrames;
    const spin = totalSpin * (1 - Math.pow(1 - t, 3));
    const delay = 320 + (t * t) * 680; // 320 мс → 1000 мс
    frames.push({ spin, delay });
  }
  return { frames, shares };
}

/** Возвращает индекс сегмента под стрелкой для данного угла (для проверки/отладки). */
export function segmentAtPointer(shares, spin) {
  const pointer = (-Math.PI / 2 - spin) % (Math.PI * 2);
  const p = (pointer + Math.PI * 2) % (Math.PI * 2);
  let acc = 0;
  for (let i = 0; i < shares.length; i++) {
    acc += shares[i] * Math.PI * 2;
    if (p < acc) return i;
  }
  return shares.length - 1;
}