module.exports = {
  apps: [
    {
      name: "dan-games-discord-bot",
      script: "bot.mjs",
      cwd: __dirname,
      interpreter: "node",
      autorestart: true,
      restart_delay: 5000,
      max_restarts: 20,
      exp_backoff_restart_delay: 100,
      kill_timeout: 10000,
      env: {
        NODE_ENV: "production"
      }
    }
  ]
};
