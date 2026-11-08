using System.Text.Json;

namespace KnowledgeEditor;

public sealed class SettingsDialog : Form
{
    public AppConfig Config { get; private set; }

    private readonly TextBox txtConnection = new();
    private readonly TextBox txtEmbedUrl = new();
    private readonly TextBox txtEmbedModel = new();
    private readonly TextBox txtApiKey = new();
    private readonly Button btnOk = new();
    private readonly Button btnCancel = new();

    public SettingsDialog(AppConfig current)
    {
        Config = new AppConfig
        {
            ConnectionString = current.ConnectionString,
            EmbedUrl = current.EmbedUrl,
            EmbedModel = current.EmbedModel,
            EmbedApiKey = current.EmbedApiKey,
        };

        Text = "Настройки подключения";
        ClientSize = new Size(640, 340);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;

        var lblConn = new Label { Text = "Строка подключения PostgreSQL (DATABASE_URL):", Location = new Point(16, 16), AutoSize = true };
        txtConnection.Location = new Point(16, 44);
        txtConnection.Size = new Size(600, 27);
        txtConnection.Text = Config.ConnectionString;

        var lblEmbed = new Label { Text = "URL эмбеддинг-сервера:", Location = new Point(16, 82), AutoSize = true };
        txtEmbedUrl.Location = new Point(16, 110);
        txtEmbedUrl.Size = new Size(600, 27);
        txtEmbedUrl.Text = Config.EmbedUrl;

        var lblModel = new Label { Text = "Модель эмбеддинга:", Location = new Point(16, 148), AutoSize = true };
        txtEmbedModel.Location = new Point(16, 176);
        txtEmbedModel.Size = new Size(600, 27);
        txtEmbedModel.Text = Config.EmbedModel;

        var lblKey = new Label { Text = "API-ключ Mistral:", Location = new Point(16, 214), AutoSize = true };
        txtApiKey.Location = new Point(16, 242);
        txtApiKey.Size = new Size(600, 27);
        txtApiKey.Text = Config.EmbedApiKey;

        btnOk.Text = "Сохранить";
        btnOk.DialogResult = DialogResult.OK;
        btnOk.Location = new Point(430, 290);
        btnOk.Size = new Size(90, 34);
        btnCancel.Text = "Отмена";
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.Location = new Point(526, 290);
        btnCancel.Size = new Size(90, 34);

        Controls.AddRange(new Control[] { lblConn, txtConnection, lblEmbed, txtEmbedUrl, lblModel, txtEmbedModel, lblKey, txtApiKey, btnOk, btnCancel });
        AcceptButton = btnOk;
        CancelButton = btnCancel;

        btnOk.Click += (_, _) =>
        {
            Config.ConnectionString = txtConnection.Text.Trim();
            Config.EmbedUrl = txtEmbedUrl.Text.Trim();
            Config.EmbedModel = txtEmbedModel.Text.Trim();
            Config.EmbedApiKey = txtApiKey.Text.Trim();
        };
    }
}