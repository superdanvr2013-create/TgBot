// Заливка словаря ругательств в moderation_words.
// Источники: itlessons/js-antimat (MIT), i-jurij/censure-js (словарь по книге
// «Русский мат. Толковый словарь» Ахметовой Т.В.), + базовый список из кода бота.
import "dotenv/config";
import pg from "pg";

// Корни/основы слов: матчинг по подстроке (в moderateMessage проверяется вхождение корня в текст).
const WORDS = [
  // классический мат (корни)
  "хуй", "хуе", "хуё", "хуи", "хуя", "хулю", "хую", "хуле",
  "пизд", "пизж", "спизд",
  "еба", "ебл", "ебы", "ебу", "ебан", "ебат", "ебиц", "ёба", "ёбн",
  "бля", "бляд", "блят",
  "сук", "сука", "суки", "сучк", "сукин",
  "манд", "манда",
  "елд", "елда",
  // производные и грубые
  "гандон", "гондон", "ганд",
  "пидор", "пидар", "пидр", "пид",
  "мудак", "мудил", "мудо", "мудозвон",
  "лох", "лошар",
  "уёб", "уеб",
  "чмо",
  "тварь",
  "мразь",
  "сволоч",
  "падла", "падл",
  "шлюх", "шалав", "шалаш",
  "проститут", "простит",
  "курв",
  "потаск",
  // оскорбления
  "идиот", "кретин", "тупиц", "тупой", "тупоголов", "туполоб", "тупоумн",
  "дебил", "придур", "долбо", "долба",
  "имбецл", "имбецил",
  "урод", "уродств",
  "бесполезн", "никчёмн", "ничтож",
  "псих", "шизик", "шизоид",
  "бомж", "нищ",
  "ублюд",
  "выродок", "отродь",
  "омерз", "мерзав",
  "подонок", "падаль", "паскуд",
  "стерв",
  "холуй", "хам",
  "задрот",
  "дрист", "дерьм", "говн", "говён",
  "анал", "анус",
  "сперм",
  "залуп",
  "хер",
  "хрен", // мягкое, но пусть будет под контролем — можно удалить из интерфейса
  "хуйл", "хуил",
  "чурбан", "чучел",
  "остолоп", "обалдуй",
  "пентюх", "перечниц",
  "фефел",
  "хрыч",
  "шваль", "шантрап", "шаромыж",
  "юлдак",
  "ядрён", "ядре",
  "лохудр", "лахудр",
  "казлин", "козлин",
  "додик", "дойк",
  "гомик",
  "дилдо",
  "жид",
  "лохан",
  "обезьян", // нейтральное, убрать при желании
  "кретин",
  "мегер",
  "хайл",
  "халд",
  "хмыр",
  "хлюст",
  "пустозвон",
  "пустобрёх",
  "тряпк",
  "шкет",
  "кретин",
];

const client = new pg.Client({ connectionString: process.env.DATABASE_URL, ssl: false, connectionTimeoutMillis: 15000 });

let connected = false;
for (let attempt = 1; attempt <= 5 && !connected; attempt++) {
  try {
    await client.connect();
    connected = true;
  } catch (e) {
    console.warn(`connect attempt ${attempt} failed: ${e.message}`);
    await new Promise((r) => setTimeout(r, attempt * 3000));
  }
}
if (!connected) { console.error("Could not connect to DB"); process.exit(1); }

let inserted = 0, skipped = 0;
for (const w of [...new Set(WORDS)]) {
  try {
    await client.query(
      "INSERT INTO moderation_words (word, note) VALUES ($1, $2) ON CONFLICT (word) DO NOTHING",
      [w, "seed: js-antimat (MIT) + censure-js + базовый список"]
    );
    inserted++;
  } catch (e) { skipped++; }
}
const total = await client.query("SELECT count(*)::int AS c FROM moderation_words");
console.log(`inserted: ${inserted}, skipped dupes: ${skipped}, total in table: ${total.rows[0].c}`);
await client.end();
