namespace KnowledgeEditor;

public sealed class SettingsDialog : Form
{
    public AppConfig Config { get; private set; }

    private readonly ComboBox cmbMode = new();
    private readonly TextBox txtApiUrl = new();
    private readonly TextBox txtApiSecret = new();
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
            DbMode = current.DbMode,
            ApiUrl = current.ApiUrl,
            ApiSecret = current.ApiSecret,
            ConnectionString = current.ConnectionString,
            EmbedUrl = current.EmbedUrl,
            EmbedModel = current.EmbedModel,
            EmbedApiKey = current.EmbedApiKey,
        };

        Text = "Настройки подключения";
        ClientSize = new Size(640, 470);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        AutoScroll = true;

        var lblMode = new Label { Text = "Режим доступа к БД:", Location = new Point(16, 16), AutoSize = true };
        cmbMode.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbMode.Location = new Point(16, 44);
        cmbMode.Size = new Size(300, 28);
        cmbMode.Items.AddRange(new object[] { "api — через Vercel (из РФ без VPN)", "direct — Npgsql напрямую (VPN)" });
        cmbMode.SelectedIndex = Config.DbMode == "direct" ? 1 : 0;
        cmbMode.SelectedIndexChanged += (_, _) => UpdateVisibility();

        var lblApiUrl = new Label { Text = "URL Vercel API (/api/db):", Location = new Point(16, 82), AutoSize = true };
        txtApiUrl.Location = new Point(16, 110);
        txtApiUrl.Size = new Size(600, 27);
        txtApiUrl.Text = Config.ApiUrl;

        var lblApiSecret = new Label { Text = "Секрет API (POLL_SECRET):", Location = new Point(16, 148), AutoSize = true };
        txtApiSecret.Location = new Point(16, 176);
        txtApiSecret.Size = new Size(600, 27);
        txtApiSecret.UseSystemPasswordChar = true;
        txtApiSecret.Text = Config.ApiSecret;

        var lblConn = new Label { Text = "Строка подключения PostgreSQL (только для direct):", Location = new Point(16, 214), AutoSize = true };
        txtConnection.Location = new Point(16, 242);
        txtConnection.Size = new Size(600, 27);
        txtConnection.Text = Config.ConnectionString;

        var lblEmbed = new Label { Text = "URL эмбеддинг-сервера:", Location = new Point(16, 280), AutoSize = true };
        txtEmbedUrl.Location = new Point(16, 308);
        txtEmbedUrl.Size = new Size(600, 27);
        txtEmbedUrl.Text = Config.EmbedUrl;

        var lblModel = new Label { Text = "Модель эмбеддинга:", Location = new Point(16, 346), AutoSize = true };
        txtEmbedModel.Location = new Point(16, 374);
        txtEmbedModel.Size = new Size(600, 27);
        txtEmbedModel.Text = Config.EmbedModel;

        var lblKey = new Label { Text = "API-ключ Mistral:", Location = new Point(16, 412), AutoSize = true };
        txtApiKey.Location = new Point(16, 440);
        txtApiKey.Size = new Size(600, 27);
        txtApiKey.Text = Config.EmbedApiKey;

        btnOk.Text = "Сохранить";
        btnOk.DialogResult = DialogResult.OK;
        btnOk.Location = new Point(430, 478);
        btnOk.Size = new Size(90, 34);
        btnCancel.Text = "Отмена";
        btnCancel.DialogResult = DialogResult.Cancel;
        btnCancel.Location = new Point(526, 478);
        btnCancel.Size = new Size(90, 34);

        Controls.AddRange(new Control[]
        {
            lblMode, cmbMode, lblApiUrl, txtApiUrl, lblApiSecret, txtApiSecret, lblConn, txtConnection,
            lblEmbed, txtEmbedUrl, lblModel, txtEmbedModel, lblKey, txtApiKey, btnOk, btnCancel
        });
        AcceptButton = btnOk;
        CancelButton = btnCancel;

        btnOk.Click += (_, _) =>
        {
            Config.DbMode = cmbMode.SelectedIndex == 1 ? "direct" : "api";
            Config.ApiUrl = txtApiUrl.Text.Trim();
            Config.ApiSecret = txtApiSecret.Text.Trim();
            Config.ConnectionString = txtConnection.Text.Trim();
            Config.EmbedUrl = txtEmbedUrl.Text.Trim();
            Config.EmbedModel = txtEmbedModel.Text.Trim();
            Config.EmbedApiKey = txtApiKey.Text.Trim();
        };

        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        var apiMode = cmbMode.SelectedIndex != 1;
        txtApiUrl.Visible = apiMode;
        txtApiSecret.Visible = apiMode;
        txtConnection.Visible = !apiMode;
    }
}
