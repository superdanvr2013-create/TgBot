// Слой доступа к таблице roulette_participants.
import pg from "pg";

let pool = null;
function getPool() {
  if (!process.env.DATABASE_URL) throw new Error("DATABASE_URL not set");
  if (!pool) {
    pool = new pg.Pool({
      connectionString: process.env.DATABASE_URL,
      max: 4,
      idleTimeoutMillis: 30000,
      connectionTimeoutMillis: 15000,
      keepAlive: true,
      keepAliveInitialDelayMillis: 10000,
    });
    pool.on("error", (err) => {
      console.error("[roulette pg pool] idle client error:", err.message);
      pool = null;
    });
  }
  return pool;
}

export async function addParticipant({ discordId, username, displayName }) {
  await getPool().query(
    `INSERT INTO roulette_participants (discord_id, username, display_name)
     VALUES ($1, $2, $3)
     ON CONFLICT (discord_id) DO NOTHING`,
    [discordId, username, displayName]
  );
}

export async function countParticipants() {
  const r = await getPool().query("SELECT count(*)::int AS c FROM roulette_participants");
  return r.rows[0].c;
}

export async function listParticipants() {
  const r = await getPool().query(
    "SELECT discord_id, username, display_name FROM roulette_participants ORDER BY joined_at"
  );
  return r.rows;
}

export async function removeParticipantByName(name) {
  const r = await getPool().query(
    "DELETE FROM roulette_participants WHERE lower(display_name) = lower($1)",
    [name]
  );
  return r.rowCount ?? 0;
}

export async function clearParticipants() {
  await getPool().query("TRUNCATE TABLE roulette_participants");
}
