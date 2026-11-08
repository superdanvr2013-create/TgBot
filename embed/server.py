"""Local embedding server for Dan Games RAG.

Serves POST /v1/embeddings (OpenAI-compatible) using FastEmbed + nomic-embed-text-v1.5.
Runs CPU-only (ONNX). Port 8899 by default.
"""
import os, json, sys
from http.server import BaseHTTPRequestHandler, HTTPServer

os.environ.setdefault("HF_ENDPOINT", "https://hf-mirror.com")

from fastembed import TextEmbedding  # noqa: E402

MODEL_NAME = os.environ.get("EMBED_MODEL", "nomic-ai/nomic-embed-text-v1.5")
PORT = int(os.environ.get("EMBED_PORT", "8899"))

print(f"Loading model {MODEL_NAME} ...", flush=True)
model = TextEmbedding(model_name=MODEL_NAME)
print("Model ready.", flush=True)


class Handler(BaseHTTPRequestHandler):
    def _json(self, code, obj):
        body = json.dumps(obj).encode("utf-8")
        self.send_response(code)
        self.send_header("Content-Type", "application/json")
        self.send_header("Content-Length", str(len(body)))
        self.end_headers()
        self.wfile.write(body)

    def do_GET(self):
        if self.path.startswith("/health"):
            self._json(200, {"ok": True, "model": MODEL_NAME})
        else:
            self._json(404, {"error": "not found"})

    def do_POST(self):
        if self.path.rstrip("/") != "/v1/embeddings":
            self._json(404, {"error": "not found"})
            return
        try:
            n = int(self.headers.get("Content-Length", "0"))
            raw = self.rfile.read(n)
            req = json.loads(raw or b"{}")
            texts = req.get("input")
            if isinstance(texts, str):
                texts = [texts]
            if not texts:
                self._json(400, {"error": "input required"})
                return
            vecs = list(model.embed(texts))
            data = [
                {"index": i, "embedding": [float(x) for x in v.tolist()]}
                for i, v in enumerate(vecs)
            ]
            self._json(200, {"object": "list", "model": MODEL_NAME, "data": data})
        except Exception as e:  # noqa: BLE001
            self._json(500, {"error": str(e)})

    def log_message(self, *args):
        sys.stderr.write("embed-srv: " + args[0] + "\n")


if __name__ == "__main__":
    srv = HTTPServer(("127.0.0.1", PORT), Handler)
    print(f"Serving on http://127.0.0.1:{PORT}", flush=True)
    srv.serve_forever()