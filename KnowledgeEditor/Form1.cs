namespace KnowledgeEditor;

public partial class Form1 : Form
{
    private AppConfig _config = AppConfig.Load();
    private KnowledgeRepository? _repo;
    private EmbeddingClient? _embed;
    private List<KnowledgeRow> _rows = new();
    private long? _editingId;
    private WordsRepository? _wordsRepo;
    private List<WordRow> _wordRows = new();
    private long? _editingWordId;

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
        RefreshWordsGrid();
    }

    private void SetupServices()
    {
        try
        {
            _repo = new KnowledgeRepository(_config);
            _embed = new EmbeddingClient(_config.EmbedUrl, _config.EmbedApiKey, _config.EmbedModel);
            _wordsRepo = new WordsRepository(_config);
            SetStatus(_config.DbMode == "direct"
                ? "Подключение настроено (прямое к БД)."
                : "Подключение настроено (через Vercel API).", StatusKind.Ok);
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
            RefreshWordsGrid();
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

    // ── Вкладка «Словарь оскорблений» ───────────────────────────────────────

    private async void RefreshWordsGrid(string? filter = null)
    {
        if (_wordsRepo is null)
        {
            dgvWords.Rows.Clear();
            return;
        }
        try
        {
            _wordRows = (await _wordsRepo.ListAsync(filter)).ToList();
            dgvWords.Rows.Clear();
            foreach (var row in _wordRows)
            {
                dgvWords.Rows.Add(row.Id, row.Word, row.Note, row.AddedAt.ToLocalTime().ToString("g"));
            }
            SetStatus($"Словарь: загружено слов: {_wordRows.Count}", StatusKind.Ok);
        }
        catch (Exception ex)
        {
            SetStatus("Не удалось загрузить словарь: " + ex.Message, StatusKind.Error);
            ShowError("Ошибка загрузки словаря", ex);
        }
    }

    private async void BtnAddWord_Click(object? sender, EventArgs e)
    {
        var word = txtWord.Text.Trim().ToLowerInvariant();
        if (word.Length == 0)
        {
            SetStatus("Введите слово.", StatusKind.Warn);
            txtWord.Focus();
            return;
        }
        if (_wordsRepo is null)
        {
            SetStatus("Сначала настройте подключение к БД.", StatusKind.Warn);
            return;
        }
        try
        {
            SetBusy(true, "Добавление слова…");
            var id = await _wordsRepo.AddAsync(word, txtWordNote.Text.Trim());
            SetStatus($"Слово «{word}» добавлено (id={id}).", StatusKind.Ok);
            txtWord.Clear();
            txtWordNote.Clear();
            await Task.Run(() => RefreshWordsGrid(txtSearchWord.Text.Trim()));
        }
        catch (Exception ex)
        {
            SetStatus("Ошибка добавления слова: " + ex.Message, StatusKind.Error);
            ShowError("Ошибка добавления слова", ex);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async void BtnUpdateWord_Click(object? sender, EventArgs e)
    {
        if (_editingWordId is null)
        {
            SetStatus("Выберите слово (двойной клик в таблице), затем нажмите «Обновить».", StatusKind.Warn);
            return;
        }
        var word = txtWord.Text.Trim().ToLowerInvariant();
        if (word.Length == 0)
        {
            SetStatus("Введите слово.", StatusKind.Warn);
            txtWord.Focus();
            return;
        }
        if (_wordsRepo is null)
        {
            SetStatus("Сначала настройте подключение к БД.", StatusKind.Warn);
            return;
        }
        try
        {
            SetBusy(true, "Обновление слова…");
            await _wordsRepo.UpdateAsync(_editingWordId.Value, word, txtWordNote.Text.Trim());
            SetStatus($"Слово «{word}» обновлено.", StatusKind.Ok);
            txtWord.Clear();
            txtWordNote.Clear();
            _editingWordId = null;
            ResetWordButtonState();
            await Task.Run(() => RefreshWordsGrid(txtSearchWord.Text.Trim()));
        }
        catch (Exception ex)
        {
            SetStatus("Ошибка обновления слова: " + ex.Message, StatusKind.Error);
            ShowError("Ошибка обновления слова", ex);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async void BtnDeleteWord_Click(object? sender, EventArgs e)
    {
        if (dgvWords.SelectedRows.Count == 0)
        {
            SetStatus("Выберите слово для удаления.", StatusKind.Warn);
            return;
        }
        var id = (long)dgvWords.SelectedRows[0].Cells[0].Value!;
        var word = (string)dgvWords.SelectedRows[0].Cells[1].Value!;
        if (MessageBox.Show($"Удалить слово «{word}»?", "Удаление", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;
        if (_wordsRepo is null)
        {
            SetStatus("БД не настроена.", StatusKind.Warn);
            return;
        }
        try
        {
            SetBusy(true, "Удаление слова…");
            await _wordsRepo.DeleteAsync(id);
            SetStatus($"Слово «{word}» удалено.", StatusKind.Ok);
            await Task.Run(() => RefreshWordsGrid(txtSearchWord.Text.Trim()));
        }
        catch (Exception ex)
        {
            SetStatus("Ошибка удаления слова: " + ex.Message, StatusKind.Error);
            ShowError("Ошибка удаления слова", ex);
        }
        finally
        {
            SetBusy(false);
        }
    }

    private async void BtnSearchWord_Click(object? sender, EventArgs e)
    {
        await Task.Run(() => RefreshWordsGrid(txtSearchWord.Text.Trim()));
    }

    private void DgvWords_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var r = _wordRows[e.RowIndex];
        _editingWordId = r.Id;
        txtWord.Text = r.Word;
        txtWordNote.Text = r.Note;
        ResetWordButtonState();
        SetStatus($"Загружено слово «{r.Word}». Измените и нажмите «Обновить».", StatusKind.Ok);
    }

    private void ResetWordButtonState()
    {
        btnUpdateWord.Enabled = _editingWordId is not null;
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