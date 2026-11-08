// Чат-рулетка для Telegram: псевдоанимация колеса через editMessageMedia.
// Участники хранятся в PostgreSQL (та же таблица roulette_participants).
import { InputFile } from "grammy";
import { drawWheelFrame, planFrames } from "../discord/roulette/wheel.mjs";
import { makeWinnerBanner } from "./winner_anim.mjs";
import * as repo from "../discord/roulette/repo.mjs";

const sleep = (ms) => new Promise((r) => setTimeout(r, ms));

export const OWNER_USERNAME = "danvr2013";

// Состояние сбора: chatId → { messageId } (объявление о наборе открыто)
const collecting = new Map();

export function isOwner(username) {
  return (username ?? "").toLowerCase() === OWNER_USERNAME;
}

/** Команда: «набери участников». */
export async function startCollect(ctx) {
  const chatId = ctx.chat.id;
  if (collecting.has(chatId)) {
    await ctx.reply("⚠️ Сбор участников уже идёт! Напиши «участвую», чтобы присоединиться.");
    return;
  }
  const msg = await ctx.reply(
    "🎰 *ЧАТ-РУЛЕТКА DAN GAMES* 🎰\n\n" +
      "Набор участников *открыт*!\n\n" +
      "Чтобы участвовать — напиши в чат: *участвую*\n\n" +
      "Когда наберётся народ, владелец запустит колесо командой:\n" +
      "«бот запускай чат-рулетку»",
    { parse_mode: "Markdown" }
  );
  collecting.set(chatId, { messageId: msg.message_id });
}

/** Команда: «сколько набрано участников». */
export async function showCount(ctx) {
  try {
    const rows = await repo.listParticipants();
    const c = rows.length;
    let text = `🎰 *Чат-рулетка — статистика*\n\nНабрано участников: *${c}*`;
    if (c > 0 && c <= 20) {
      text += "\n\n" + rows.map((r, i) => `${i + 1}. ${r.display_name}`).join("\n");
    }
    await ctx.reply(text, { parse_mode: "Markdown" });
  } catch (err) {
    console.error("[tg roulette] count error:", err);
    await ctx.reply("❌ Не удалось получить количество (проблема с базой данных).");
  }
}

/** Присоединение участника по тексту «участвую». */
export async function joinByText(ctx) {
  const chatId = ctx.chat.id;
  if (!collecting.has(chatId)) return false;
  const user = ctx.from;
  const displayName = user.first_name || user.username || `user_${user.id}`;
  try {
    await repo.addParticipant({
      discordId: String(user.id),
      username: user.username ?? `user_${user.id}`,
      displayName,
    });
    await ctx.reply(`✅ *${displayName}* в игре!`, { parse_mode: "Markdown" });
    return true;
  } catch (err) {
    console.error("[tg roulette] join error:", err);
    await ctx.reply("❌ Не удалось записаться (проблема с базой данных).");
    return true;
  }
}

/** Команда: «добавь участника ... : ник» (только владелец). */
export async function addManual(ctx, name) {
  try {
    await repo.addParticipant({
      discordId: `manual_${Date.now()}`,
      username: name,
      displayName: name,
    });
    await ctx.reply(`✅ Участник *${name}* добавлен в рулетку.`, { parse_mode: "Markdown" });
  } catch (err) {
    console.error("[tg roulette] addManual error:", err);
    await ctx.reply("❌ Не удалось добавить участника.");
  }
}

/** Команда: «удали участника ... : ник» (только владелец). */
export async function removeManual(ctx, name) {
  try {
    const removed = await repo.removeParticipantByName(name);
    await ctx.reply(
      removed > 0
        ? `🗑️ Участник *${name}* удалён из рулетки.`
        : `⚠️ Участник *${name}* не найден.`,
      { parse_mode: "Markdown" }
    );
  } catch (err) {
    console.error("[tg roulette] removeManual error:", err);
    await ctx.reply("❌ Не удалось удалить участника.");
  }
}

/** Команда: «запускай чат-рулетку». Псевдоанимация + победитель. */
export async function runRoulette(ctx) {
  const chatId = ctx.chat.id;
  const rows = await repo.listParticipants();
  if (rows.length < 2) {
    await ctx.reply(`⚠️ Слишком мало участников: *${rows.length}*. Нужно минимум 2.`, { parse_mode: "Markdown" });
    return;
  }

  const names = rows.map((r) => r.display_name);
  const winIndex = Math.floor(Math.random() * rows.length);
  const winner = rows[winIndex];
  const { frames, shares } = planFrames(names, winIndex, 16);

  // стартовый кадр
  const startBuf = drawWheelFrame(names, 0, shares);
  const startMsg = await ctx.replyWithPhoto(new InputFile(startBuf, "wheel.png"), {
    caption: `🎰 *ЧАТ-РУЛЕТКА — КОЛЕСО КРУТИТСЯ…*\n\nУчастников: *${rows.length}*`,
    parse_mode: "Markdown",
  });

  // псевдоанимация: editMessageMedia с нарастающей задержкой
  for (const { spin, delay } of frames) {
    await sleep(delay);
    const buf = drawWheelFrame(names, spin, shares);
    try {
      await ctx.api.editMessageMedia(
        chatId,
        startMsg.message_id,
        {
          type: "photo",
          media: new InputFile(buf, "wheel.png"),
          caption: `🎰 *ЧАТ-РУЛЕТКА — КОЛЕСО КРУТИТСЯ…*\n\nУчастников: *${rows.length}*`,
          parse_mode: "Markdown",
        }
      );
    } catch (err) {
      console.warn("[tg roulette] edit frame skipped:", err.error_code ?? err.message);
      await sleep(1200);
    }
  }

  // победитель: статичный кадр + отдельное красивое объявление
  const winBuf = drawWheelFrame(names, frames[frames.length - 1].spin, shares);
  const isRealUser = /^\d+$/.test(winner.discord_id);
  const mention = isRealUser
    ? `[${winner.display_name}](tg://user?id=${winner.discord_id})`
    : `*${winner.display_name}*`;

  try {
    await ctx.api.editMessageMedia(
      chatId,
      startMsg.message_id,
      {
        type: "photo",
        media: new InputFile(winBuf, "wheel.png"),
        caption: `🎰 *ЧАТ-РУЛЕТКА — РЕЗУЛЬТАТ*\n\nПобедитель: ${mention}`,
        parse_mode: "Markdown",
      }
    );
  } catch (err) {
    console.warn("[tg roulette] final edit skipped:", err.error_code ?? err.message);
  }

  // красивое объявление с баннером
  const banner = makeWinnerBanner(winner.display_name);
  await ctx.replyWithPhoto(new InputFile(banner, "winner.png"), {
    caption:
      `# 🏆 ПОБЕДИТЕЛЬ ЧАТ-РУЛЕТКИ 🏆\n\n` +
      `## 🎉 ${winner.display_name} 🎉\n\n` +
      `Поздравляем, ${mention}!\n` +
      `Колесо остановилось на тебе! 🎊`,
    parse_mode: "Markdown",
  });

  // финиш: чистим базу и состояние
  try {
    await repo.clearParticipants();
  } catch (err) {
    console.error("[tg roulette] clear error:", err);
  }
  collecting.delete(chatId);
  console.log(`[tg roulette] finished, winner=${winner.display_name}`);
}
