// Обработка изображений для vision-запросов ботов.
// Скачивает вложение, сжимает до MAX_W x MAX_H (вписывает с сохранением пропорций),
// кодирует в base64 data-URL. Мелкие картинки (≤576px по меньшей стороне) не увеличиваем.
import sharp from "sharp";

const MAX_W = 1024;
const MAX_H = 768;

/**
 * @param {Response} fetchResponse ответ fetch() на файл изображения
 * @param {string} contentType исходный content-type (image/jpeg, image/png, ...)
 * @returns {Promise<string>} data-URL (data:image/jpeg;base64,...)
 */
export async function fetchImageAsDataUrl(fetchResponse, contentType = "image/jpeg") {
  if (!fetchResponse.ok) {
    throw new Error(`image download failed: HTTP ${fetchResponse.status}`);
  }
  const buffer = Buffer.from(await fetchResponse.arrayBuffer());

  // Ресайз: вписываем в 1024x768 без увеличения маленьких картинок.
  const resized = await sharp(buffer)
    .rotate() // уважаем EXIF-ориентацию (фото с телефона часто перевёрнуты)
    .resize(MAX_W, MAX_H, { fit: "inside", withoutEnlargement: true })
    .jpeg({ quality: 85 })
    .toBuffer();

  return `data:image/jpeg;base64,${resized.toString("base64")}`;
}

/**
 * Строит vision-content для OpenAI-совместимого API: текст + изображения.
 * @param {string} userText текст пользователя
 * @param {string[]} imageDataUrls массив data-URL картинок
 * @returns {Array} content-массив
 */
export function buildVisionContent(userText, imageDataUrls) {
  const parts = [{ type: "text", text: userText }];
  for (const url of imageDataUrls) {
    parts.push({ type: "image_url", image_url: { url } });
  }
  return parts;
}
