namespace KnowledgeEditor;

public partial class Form1 : Form
{
    private AppConfig _config = AppConfig.Load();
    private KnowledgeRepository? _repo;
    private EmbeddingClient? _embed;
    private List<KnowledgeRow> _rows = new();
    private long? _editingId;

    public Form1()
    {
        InitializeComponent();
        InitializeState();
    }

    private void InitializeState()
    {
        UpdateConnectionStatus();

        // If config is valid, wire up the repository + load rows.
        if (_config.IsConfigured)
        {
            SetupServices();
        }
        else
        {
            SetStatus("Настройте подключение к БД (файл config.json или кнопка «Настройки»).", StatusKind.Warn);
        }

        // Preload the theme list.
        RefreshGrid();
    }

    private void SetupServices()
    {
        try
        {
            _repo = new KnowledgeRepository(_config.ConnectionString);
            _embed = new EmbeddingClient(_config.EmbedUrl, _config.EmbedApiKey, _config.EmbedModel);
            SetStatus("Подключение настроено.", StatusKind.Ok);
        }
        catch (Exception ex)
        {
            SetStatus("Ошибка инициализации: " + ex.Message, StatusKind.Error);
        }
    }

    private void UpdateConnectionStatus()
    {
        lblConnection.Text = _config.IsConfigured
            ? "БД: настроена"
            : "БД: НЕ настроена (открой «Настройки»)";
        lblConnection.ForeColor = _config.IsConfigured ? Color.DarkGreen : Color.Firebrick;
    }

    private async void RefreshGrid()
    {
        if (_repo is null)
        {
            dgvKnowledge.Rows.Clear();
            return;
        }
        try
        {
            _rows = (await _repo.ListAsync()).ToList();
            dgvKnowledge.Rows.Clear();
            foreach (var row in _rows)
            {
                dgvKnowledge.Rows.Add(row.Id, row.Theme, row.Description, row.HasEmbedding ? "✓" : "—", row.UpdatedAt.ToLocalTime().ToString("g"));
            }
            SetStatus($"Загружено записей: {_rows.Count}", StatusKind.Ok);
        }
        catch (Exception ex)
        {
            SetStatus("Не удалось загрузить записи: " + ex.Message, StatusKind.Error);
            ShowError("Ошибка загрузки", ex);
        }
    }

    private async void BtnSave_Click(object? sender, EventArgs e)
    {
        var theme = txtTheme.Text.Trim();
        var description = txtDescription.Text.Trim();
        if (theme.Length == 0)
        {
            SetStatus("Введите тему.", StatusKind.Warn);
            txtTheme.Focus();
            return;
        }
        if (description.Length == 0)
        {
            SetStatus("Введите описание.", StatusKind.Warn);
            txtDescription.Focus();
            return;
        }
        if (_repo is null || _embed is null)
        {
            SetStatus("Сначала настройте подключение к БД.", StatusKind.Warn);
            return;
        }

        try
        {
            SetBusy(true, "Сохранение… (эмбеддинг + запись)");
            var vec = await _embed.EmbedAsync($"{theme}\n{description}");
            var id = await _repo.UpsertAsync(theme, description, vec);
            SetStatus($"Сохранено (id={id}).", StatusKind.Ok);
            ClearFields();
            await Task.Run(RefreshGrid);
        }
        catch (Exception ex)
        {
            SetStatus("Ошибка сохранения: " + ex.Message, StatusKind.Error);
            ShowError("Ошибка сохранения", ex);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async void BtnUpdate_Click(object? sender, EventArgs e)
    {
        if (_editingId is null)
        {
            SetStatus("Выберите запись (двойной клик в таблице), затем нажмите «Обновить».", StatusKind.Warn);
            return;
        }
        var theme = txtTheme.Text.Trim();
        var description = txtDescription.Text.Trim();
        if (theme.Length == 0)
        {
            SetStatus("Введите тему.", StatusKind.Warn);
            txtTheme.Focus();
            return;
        }
        if (description.Length == 0)
        {
            SetStatus("Введите описание.", StatusKind.Warn);
            txtDescription.Focus();
            return;
        }
        if (_repo is null || _embed is null)
        {
            SetStatus("Сначала настройте подключение к БД.", StatusKind.Warn);
            return;
        }

        try
        {
            SetBusy(true, "Обновление… (эмбеддинг + запись)");
            var vec = await _embed.EmbedAsync($"{theme}\n{description}");
            var id = await _repo.UpdateAsync(_editingId.Value, theme, description, vec);
            SetStatus($"Обновлено (id={id}).", StatusKind.Ok);
            ClearFields();
            _editingId = null;
            ResetButtonState();
            await Task.Run(RefreshGrid);
        }
        catch (Exception ex)
        {
            SetStatus("Ошибка обновления: " + ex.Message, StatusKind.Error);
            ShowError("Ошибка обновления", ex);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async void BtnDelete_Click(object? sender, EventArgs e)
    {
        if (dgvKnowledge.SelectedRows.Count == 0)
        {
            SetStatus("Выберите запись для удаления.", StatusKind.Warn);
            return;
        }
        var id = (long)dgvKnowledge.SelectedRows[0].Cells[0].Value!;
        if (MessageBox.Show($"Удалить запись id={id}?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        if (_repo is null)
        {
            SetStatus("БД не настроена.", StatusKind.Warn);
            return;
        }
        try
        {
            SetBusy(true, "Удаление…");
            await _repo.DeleteAsync(id);
            SetStatus($"Запись id={id} удалена.", StatusKind.Ok);
            await Task.Run(RefreshGrid);
        }
        catch (Exception ex)
        {
            SetStatus("Ошибка удаления: " + ex.Message, StatusKind.Error);
            ShowError("Ошибка удаления", ex);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void DgvKnowledge_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var r = _rows[e.RowIndex];
        _editingId = r.Id;
        txtTheme.Text = r.Theme;
        txtDescription.Text = r.Description;
        ResetButtonState();
        SetStatus($"Загружена запись id={r.Id}. Измените тему/описание и нажмите «Обновить».", StatusKind.Ok);
    }

    private void BtnSettings_Click(object? sender, EventArgs e)
    {
        using var dlg = new SettingsDialog(_config);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            _config = dlg.Config;
            _config.Save();
            UpdateConnectionStatus();
            SetupServices();
            RefreshGrid();
        }
    }

    private async void BtnTest_Click(object? sender, EventArgs e)
    {
        if (_embed is null)
        {
            SetStatus("Эмбеддинг-сервер не настроен.", StatusKind.Warn);
            return;
        }
        try
        {
            SetBusy(true, "Проверка эмбеддинг-сервера…");
            var vec = await _embed.EmbedAsync("тест подключения");
            SetStatus($"Эмбеддинг ОК: {vec.Length} измерений.", StatusKind.Ok);
        }
        catch (Exception ex)
        {
            SetStatus("Эмбеддинг недоступен: " + ex.Message, StatusKind.Error);
            ShowError("Ошибка эмбеддинга", ex);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private void ClearFields()
    {
        txtTheme.Clear();
        txtDescription.Clear();
        _editingId = null;
        ResetButtonState();
    }

    private void ResetButtonState()
    {
        btnUpdate.Enabled = _editingId is not null;
    }

    private void SetBusy(bool busy, string? text = null)
    {
        btnSave.Enabled = !busy;
        btnDelete.Enabled = !busy;
        btnTest.Enabled = !busy;
        btnSettings.Enabled = !busy;
        if (busy)
        {
            btnUpdate.Enabled = false;
        }
        else
        {
            ResetButtonState();
        }
        if (text is not null) SetStatus(text, StatusKind.Info);
    }

    /// <summary>Выводит полный текст ошибки в отдельном окне с кнопкой «Копировать».</summary>
    private void ShowError(string caption, Exception ex)
    {
        var text = $"{ex.GetType().Name}: {ex.Message}";
        if (ex.InnerException is not null)
        {
            text += $"\n\nInner: {ex.InnerException.GetType().Name}: {ex.InnerException.Message}";
        }
        if (ex.StackTrace is not null)
        {
            text += $"\n\nStackTrace:\n{ex.StackTrace}";
        }
        using var dlg = new ErrorDialog(caption, text);
        dlg.ShowDialog(this);
    }

    private void SetStatus(string message, StatusKind kind)
    {
        lblStatus.Text = message;
        lblStatus.ForeColor = kind switch
        {
            StatusKind.Ok => Color.DarkGreen,
            StatusKind.Warn => Color.DarkOrange,
            StatusKind.Error => Color.Firebrick,
            _ => Color.Gray,
        };
    }

    private enum StatusKind { Info, Ok, Warn, Error }
}