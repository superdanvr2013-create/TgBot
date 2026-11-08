// Чат-рулетка: сбор участников реакцией 🎰, псевдоанимация колеса, объявление победителя.
import { AttachmentBuilder, EmbedBuilder } from "discord.js";
import { drawWheelFrame, planFrames } from "./wheel.mjs";
import * as repo from "./repo.mjs";

const sleep = (ms) => new Promise((r) => setTimeout(r, ms));

export const OWNER_USERNAME = "danvr2013";
export const JOIN_EMOJI = "🎰";

// Состояние сбора (в памяти; участники хранятся в PostgreSQL).
let collect = null; // { channelId, messageId, guildId }

export function isOwner(username) {
  return (username ?? "").toLowerCase() === OWNER_USERNAME;
}

/** Команда 1: «набери участников». */
export async function startCollect(message) {
  if (collect) {
    await message.reply("⚠️ Сбор участников уже идёт! Отметьтесь 🎰 под тем сообщением.");
    return;
  }
  const embed = new EmbedBuilder()
    .setColor(0xfee75c)
    .setTitle("🎰 ЧАТ-РУЛЕТКА DAN GAMES 🎰")
    .setDescription(
      "Набор участников **открыт**!\n\n" +
        `Чтобы участвовать — поставь реакцию ${JOIN_EMOJI} под этим сообщением.\n\n` +
        "⏳ Когда наберётся народ, владелец запустит колесо командой:\n" +
        "«бот запускай чат-рулетку»"
    )
    .setFooter({ text: "Участники сохраняются в базу данных" })
    .setTimestamp();
  const msg = await message.channel.send({ embeds: [embed] });
  await msg.react(JOIN_EMOJI);
  collect = { channelId: msg.channel.id, messageId: msg.id, guildId: msg.guild?.id ?? null };
  console.log(`[roulette] collect started: msg=${msg.id}`);
}

/** Команда 2: «сколько набрано участников». */
export async function showCount(message) {
  try {
    const rows = await repo.listParticipants();
    const c = rows.length;
    const embed = new EmbedBuilder()
      .setColor(0x5865f2)
      .setTitle("🎰 Чат-рулетка — статистика")
      .setDescription(`Набрано участников: **${c}**`)
      .setTimestamp();
    if (c > 0 && c <= 20) {
      embed.addFields({ name: "Участники", value: rows.map((r, i) => `${i + 1}. ${r.display_name}`).join("\n") });
    }
    await message.reply({ embeds: [embed] });
  } catch (err) {
    console.error("[roulette] count error:", err);
    await message.reply("❌ Не удалось получить количество (проблема с базой данных).");
  }
}

/** Команда: «добавь участника ... : ник» (только владелец). */
export async function addManual(message, name) {
  try {
    await repo.addParticipant({ discordId: `manual_${Date.now()}`, username: name, displayName: name });
    await message.reply(`✅ Участник **${name}** добавлен в рулетку.`);
  } catch (err) {
    console.error("[roulette] addManual error:", err);
    await message.reply("❌ Не удалось добавить участника (проблема с базой данных).");
  }
}

/** Команда: «удали участника ... : ник» (только владелец). */
export async function removeManual(message, name) {
  try {
    const removed = await repo.removeParticipantByName(name);
    await message.reply(
      removed > 0
        ? `🗑️ Участник **${name}** удалён из рулетки.`
        : `⚠️ Участник **${name}** не найден в списке.`
    );
  } catch (err) {
    console.error("[roulette] removeManual error:", err);
    await message.reply("❌ Не удалось удалить участника (проблема с базой данных).");
  }
}

/** Команда 3: «запускай чат-рулетку». */
export async function runRoulette(client, message) {
  const rows = await repo.listParticipants();
  if (rows.length < 2) {
    await message.reply(`⚠️ Слишком мало участников: **${rows.length}**. Нужно минимум 2.`);
    return;
  }

  const names = rows.map((r) => r.display_name);
  const winIndex = Math.floor(Math.random() * rows.length);
  const winner = rows[winIndex];

  const { frames, shares } = planFrames(names, winIndex, 16);

  // стартовый кадр
  const startBuf = drawWheelFrame(names, 0, shares);
  const startMsg = await message.channel.send({
    embeds: [
      new EmbedBuilder()
        .setColor(0xfee75c)
        .setTitle("🎰 ЧАТ-РУЛЕТКА — КОЛЕСО КРУТИТСЯ…")
        .setDescription(`Участников: **${rows.length}**`)
        .setImage("attachment://wheel.png"),
    ],
    files: [new AttachmentBuilder(startBuf, { name: "wheel.png" })],
  });

  // псевдоанимация: редактируем сообщение с нарастающей задержкой (easing)
  for (const { spin, delay } of frames) {
    await sleep(delay);
    const buf = drawWheelFrame(names, spin, shares);
    try {
      await startMsg.edit({
        embeds: [
          new EmbedBuilder()
            .setColor(0xfee75c)
            .setTitle("🎰 ЧАТ-РУЛЕТКА — КОЛЕСО КРУТИТСЯ…")
            .setDescription(`Участников: **${rows.length}**`)
            .setImage("attachment://wheel.png"),
        ],
        files: [new AttachmentBuilder(buf, { name: "wheel.png" })],
      });
    } catch (err) {
      console.warn("[roulette] edit frame skipped:", err.code ?? err.message);
      await sleep(1200); // запас при rate limit
    }
  }

  // победитель: крупное цветное объявление + финальный кадр с победным углом
  const winBuf = drawWheelFrame(names, frames[frames.length - 1].spin, shares);
  const isRealUser = /^\d{15,25}$/.test(winner.discord_id);
  const mention = isRealUser ? `<@${winner.discord_id}>` : `**${winner.display_name}**`;
  const winEmbed = new EmbedBuilder()
    .setColor(0xf1c40f)
    .setTitle("# 🏆 ПОБЕДИТЕЛЬ ЧАТ-РУЛЕТКИ 🏆")
    .setDescription(
      `# 🎉 ${winner.display_name} 🎉\n\n` +
        `Поздравляем, ${mention}!\n` +
        "Колесо остановилось на тебе! 🎊"
    )
    .setImage("attachment://wheel.png")
    .setFooter({ text: `Всего участников: ${rows.length}` })
    .setTimestamp();
  await startMsg.edit({ embeds: [winEmbed], files: [new AttachmentBuilder(winBuf, { name: "wheel.png" })] });

  // финиш: чистим базу и состояние сбора
  try {
    await repo.clearParticipants();
  } catch (err) {
    console.error("[roulette] clear error:", err);
  }
  collect = null;
  console.log(`[roulette] finished, winner=${winner.display_name}`);
}

/** Обработчик добавления реакции (сбор участников). */
export async function onReactionAdd(reaction, user) {
  try {
    if (user.bot) return;
    if (!collect) return;
    if (reaction.message.id !== collect.messageId) return;
    if (reaction.emoji.name !== JOIN_EMOJI) return;
    if (reaction.message.channel.id !== collect.channelId) return;

    const displayName = user.username; // user из события — плоский, displayName недоступен; берём username
    await repo.addParticipant({
      discordId: user.id,
      username: user.username,
      displayName,
    });
    console.log(`[roulette] joined: ${user.username} (${user.id})`);
  } catch (err) {
    console.error("[roulette] reaction add error:", err.message);
  }
}
