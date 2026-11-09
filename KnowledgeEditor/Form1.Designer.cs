namespace KnowledgeEditor;

partial class Form1
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
        System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
        this.lblTitle = new System.Windows.Forms.Label();
        this.lblTheme = new System.Windows.Forms.Label();
        this.txtTheme = new System.Windows.Forms.TextBox();
        this.lblDescription = new System.Windows.Forms.Label();
        this.txtDescription = new System.Windows.Forms.TextBox();
        this.btnSave = new System.Windows.Forms.Button();
        this.btnDelete = new System.Windows.Forms.Button();
        this.btnUpdate = new System.Windows.Forms.Button();
        this.dgvKnowledge = new System.Windows.Forms.DataGridView();
        this.btnSettings = new System.Windows.Forms.Button();
        this.btnTest = new System.Windows.Forms.Button();
        this.lblStatus = new System.Windows.Forms.Label();
        this.lblConnection = new System.Windows.Forms.Label();
        this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colTheme = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colEmbed = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colUpdated = new System.Windows.Forms.DataGridViewTextBoxColumn();
        // ── TabControl: вкладки «Знания», «Словарь», «ИИ-провайдеры», «Обращения» ──
        this.tabControl = new System.Windows.Forms.TabControl();
        this.tabKnowledge = new System.Windows.Forms.TabPage();
        this.tabWords = new System.Windows.Forms.TabPage();
        this.tabProviders = new System.Windows.Forms.TabPage();
        this.tabIndividual = new System.Windows.Forms.TabPage();
        // ── Контролы вкладки «Обращения» ──
        this.lblIndNick = new System.Windows.Forms.Label();
        this.txtIndNick = new System.Windows.Forms.TextBox();
        this.lblIndDesc = new System.Windows.Forms.Label();
        this.txtIndDesc = new System.Windows.Forms.TextBox();
        this.btnIndAdd = new System.Windows.Forms.Button();
        this.btnIndUpdate = new System.Windows.Forms.Button();
        this.btnIndDelete = new System.Windows.Forms.Button();
        this.dgvIndividual = new System.Windows.Forms.DataGridView();
        this.colIndId = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colIndNick = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colIndDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colIndUpdated = new System.Windows.Forms.DataGridViewTextBoxColumn();
        // ── Контролы вкладки «ИИ-провайдеры» ──
        this.lblProvName = new System.Windows.Forms.Label();
        this.txtProvName = new System.Windows.Forms.TextBox();
        this.lblProvUrl = new System.Windows.Forms.Label();
        this.txtProvUrl = new System.Windows.Forms.TextBox();
        this.lblProvKey = new System.Windows.Forms.Label();
        this.txtProvKey = new System.Windows.Forms.TextBox();
        this.lblProvModel = new System.Windows.Forms.Label();
        this.txtProvModel = new System.Windows.Forms.TextBox();
        this.lblProvDesc = new System.Windows.Forms.Label();
        this.txtProvDesc = new System.Windows.Forms.TextBox();
        this.chkProvActive = new System.Windows.Forms.CheckBox();
        this.btnProvAdd = new System.Windows.Forms.Button();
        this.btnProvUpdate = new System.Windows.Forms.Button();
        this.btnProvDelete = new System.Windows.Forms.Button();
        this.btnProvActivate = new System.Windows.Forms.Button();
        this.btnProvTest = new System.Windows.Forms.Button();
        this.dgvProviders = new System.Windows.Forms.DataGridView();
        this.colProvId = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colProvName = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colProvUrl = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colProvModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colProvActive = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colProvDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
        // ── Контролы вкладки «Словарь оскорблений» ──
        this.lblWord = new System.Windows.Forms.Label();
        this.txtWord = new System.Windows.Forms.TextBox();
        this.lblWordNote = new System.Windows.Forms.Label();
        this.txtWordNote = new System.Windows.Forms.TextBox();
        this.btnAddWord = new System.Windows.Forms.Button();
        this.btnUpdateWord = new System.Windows.Forms.Button();
        this.btnDeleteWord = new System.Windows.Forms.Button();
        this.btnSearchWord = new System.Windows.Forms.Button();
        this.txtSearchWord = new System.Windows.Forms.TextBox();
        this.dgvWords = new System.Windows.Forms.DataGridView();
        this.colWordId = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colWordText = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colWordNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
        this.colWordAdded = new System.Windows.Forms.DataGridViewTextBoxColumn();
        ((System.ComponentModel.ISupportInitialize)(this.dgvKnowledge)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dgvWords)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dgvProviders)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.dgvIndividual)).BeginInit();
        this.tabControl.SuspendLayout();
        this.tabKnowledge.SuspendLayout();
        this.tabWords.SuspendLayout();
        this.tabProviders.SuspendLayout();
        this.tabIndividual.SuspendLayout();
        this.SuspendLayout();
        // 
        // lblTitle
        // 
        this.lblTitle.AutoSize = true;
        this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
        this.lblTitle.Location = new System.Drawing.Point(16, 14);
        this.lblTitle.Name = "lblTitle";
        this.lblTitle.Size = new System.Drawing.Size(280, 37);
        this.lblTitle.TabIndex = 0;
        this.lblTitle.Text = "База знаний Dan Games";
        // 
        // lblConnection
        // 
        this.lblConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.lblConnection.AutoSize = true;
        this.lblConnection.Location = new System.Drawing.Point(610, 24);
        this.lblConnection.Name = "lblConnection";
        this.lblConnection.Size = new System.Drawing.Size(160, 20);
        this.lblConnection.TabIndex = 1;
        this.lblConnection.Text = "БД: настроена";
        // 
        // lblTheme
        // 
        this.lblTheme.AutoSize = true;
        this.lblTheme.Location = new System.Drawing.Point(16, 66);
        this.lblTheme.Name = "lblTheme";
        this.lblTheme.Size = new System.Drawing.Size(54, 20);
        this.lblTheme.TabIndex = 2;
        this.lblTheme.Text = "Тема";
        // 
        // txtTheme
        // 
        this.txtTheme.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
        this.txtTheme.Location = new System.Drawing.Point(120, 63);
        this.txtTheme.Name = "txtTheme";
        this.txtTheme.Size = new System.Drawing.Size(650, 27);
        this.txtTheme.TabIndex = 3;
        // 
        // lblDescription
        // 
        this.lblDescription.AutoSize = true;
        this.lblDescription.Location = new System.Drawing.Point(16, 102);
        this.lblDescription.Name = "lblDescription";
        this.lblDescription.Size = new System.Drawing.Size(85, 20);
        this.lblDescription.TabIndex = 4;
        this.lblDescription.Text = "Описание";
        // 
        // txtDescription
        // 
        this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
        this.txtDescription.Location = new System.Drawing.Point(120, 97);
        this.txtDescription.MaxLength = 10000000;
        this.txtDescription.Multiline = true;
        this.txtDescription.Name = "txtDescription";
        this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
        this.txtDescription.Size = new System.Drawing.Size(650, 100);
        this.txtDescription.TabIndex = 5;
        this.txtDescription.WordWrap = false;
        // 
        // btnSave
        // 
        this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnSave.BackColor = System.Drawing.Color.FromArgb(88, 101, 242);
        this.btnSave.ForeColor = System.Drawing.Color.White;
        this.btnSave.Location = new System.Drawing.Point(636, 209);
        this.btnSave.Name = "btnSave";
        this.btnSave.Size = new System.Drawing.Size(134, 38);
        this.btnSave.TabIndex = 6;
        this.btnSave.Text = "💾 Сохранить";
        this.btnSave.UseVisualStyleBackColor = false;
        this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
        // 
        // btnDelete
        // 
        this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnDelete.Location = new System.Drawing.Point(120, 209);
        this.btnDelete.Name = "btnDelete";
        this.btnDelete.Size = new System.Drawing.Size(120, 38);
        this.btnDelete.TabIndex = 7;
        this.btnDelete.Text = "🗑 Удалить";
        this.btnDelete.UseVisualStyleBackColor = true;
        this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
        // 
        // btnUpdate
        // 
        this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnUpdate.BackColor = System.Drawing.Color.FromArgb(38, 166, 91);
        this.btnUpdate.ForeColor = System.Drawing.Color.White;
        this.btnUpdate.Enabled = false;
        this.btnUpdate.Location = new System.Drawing.Point(254, 209);
        this.btnUpdate.Name = "btnUpdate";
        this.btnUpdate.Size = new System.Drawing.Size(120, 38);
        this.btnUpdate.TabIndex = 8;
        this.btnUpdate.Text = "✏️ Обновить";
        this.btnUpdate.UseVisualStyleBackColor = false;
        this.btnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
        // 
        // btnTest
        // 
        this.btnTest.Location = new System.Drawing.Point(386, 209);
        this.btnTest.Name = "btnTest";
        this.btnTest.Size = new System.Drawing.Size(160, 38);
        this.btnTest.TabIndex = 9;
        this.btnTest.Text = "🔌 Тест эмбеддинга";
        this.btnTest.UseVisualStyleBackColor = true;
        this.btnTest.Click += new System.EventHandler(this.BtnTest_Click);
        // 
        // btnSettings
        // 
        this.btnSettings.Location = new System.Drawing.Point(558, 209);
        this.btnSettings.Name = "btnSettings";
        this.btnSettings.Size = new System.Drawing.Size(130, 38);
        this.btnSettings.TabIndex = 10;
        this.btnSettings.Text = "⚙ Настройки";
        this.btnSettings.UseVisualStyleBackColor = true;
        this.btnSettings.Click += new System.EventHandler(this.BtnSettings_Click);
        // 
        // dgvKnowledge
        // 
        this.dgvKnowledge.AllowUserToAddRows = false;
        this.dgvKnowledge.AllowUserToDeleteRows = false;
        this.dgvKnowledge.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
        | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
        this.dgvKnowledge.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        this.dgvKnowledge.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvKnowledge.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colTheme,
            this.colDescription,
            this.colEmbed,
            this.colUpdated});
        dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
        dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
        dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
        this.dgvKnowledge.DefaultCellStyle = dataGridViewCellStyle1;
        this.dgvKnowledge.Location = new System.Drawing.Point(16, 262);
        this.dgvKnowledge.MultiSelect = false;
        this.dgvKnowledge.Name = "dgvKnowledge";
        this.dgvKnowledge.ReadOnly = true;
        this.dgvKnowledge.RowHeadersVisible = false;
        this.dgvKnowledge.RowTemplate.Height = 29;
        this.dgvKnowledge.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.dgvKnowledge.Size = new System.Drawing.Size(754, 280);
        this.dgvKnowledge.TabIndex = 10;
        this.dgvKnowledge.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvKnowledge_CellDoubleClick);
        // 
        // lblStatus
        // 
        this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
        this.lblStatus.AutoSize = true;
        this.lblStatus.Location = new System.Drawing.Point(16, 550);
        this.lblStatus.Name = "lblStatus";
        this.lblStatus.Size = new System.Drawing.Size(120, 20);
        this.lblStatus.TabIndex = 11;
        this.lblStatus.Text = "Готово";
        // 
        // colId
        // 
        this.colId.FillWeight = 40F;
        this.colId.HeaderText = "ID";
        this.colId.MinimumWidth = 50;
        this.colId.Name = "colId";
        // 
        // colTheme
        // 
        this.colTheme.FillWeight = 90F;
        this.colTheme.HeaderText = "Тема";
        this.colTheme.MinimumWidth = 80;
        this.colTheme.Name = "colTheme";
        // 
        // colDescription
        // 
        this.colDescription.FillWeight = 160F;
        this.colDescription.HeaderText = "Описание";
        this.colDescription.MinimumWidth = 120;
        this.colDescription.Name = "colDescription";
        // 
        // colEmbed
        // 
        this.colEmbed.FillWeight = 30F;
        this.colEmbed.HeaderText = "Вектор";
        this.colEmbed.MinimumWidth = 50;
        this.colEmbed.Name = "colEmbed";
        // 
        // colUpdated
        // 
        this.colUpdated.FillWeight = 70F;
        this.colUpdated.HeaderText = "Обновлено";
        this.colUpdated.MinimumWidth = 90;
        this.colUpdated.Name = "colUpdated";
        // 
        // tabControl
        // 
        this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
        | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
        this.tabControl.Controls.Add(this.tabKnowledge);
        this.tabControl.Controls.Add(this.tabWords);
        this.tabControl.Controls.Add(this.tabProviders);
        this.tabControl.Controls.Add(this.tabIndividual);
        this.tabControl.Location = new System.Drawing.Point(16, 55);
        this.tabControl.Name = "tabControl";
        this.tabControl.SelectedIndex = 0;
        this.tabControl.Size = new System.Drawing.Size(754, 487);
        this.tabControl.TabIndex = 0;
        // 
        // tabKnowledge — все существующие контролы переезжают внутрь этой вкладки
        // 
        this.tabKnowledge.Controls.Add(this.dgvKnowledge);
        this.tabKnowledge.Controls.Add(this.btnSettings);
        this.tabKnowledge.Controls.Add(this.btnTest);
        this.tabKnowledge.Controls.Add(this.btnUpdate);
        this.tabKnowledge.Controls.Add(this.btnDelete);
        this.tabKnowledge.Controls.Add(this.btnSave);
        this.tabKnowledge.Controls.Add(this.txtDescription);
        this.tabKnowledge.Controls.Add(this.lblDescription);
        this.tabKnowledge.Controls.Add(this.txtTheme);
        this.tabKnowledge.Controls.Add(this.lblTheme);
        this.tabKnowledge.Location = new System.Drawing.Point(4, 29);
        this.tabKnowledge.Name = "tabKnowledge";
        this.tabKnowledge.Padding = new System.Windows.Forms.Padding(3);
        this.tabKnowledge.Size = new System.Drawing.Size(746, 454);
        this.tabKnowledge.TabIndex = 0;
        this.tabKnowledge.Text = "📚 База знаний";
        this.tabKnowledge.UseVisualStyleBackColor = true;
        // 
        // tabWords — словарь оскорблений
        // 
        this.tabWords.Controls.Add(this.dgvWords);
        this.tabWords.Controls.Add(this.btnDeleteWord);
        this.tabWords.Controls.Add(this.btnUpdateWord);
        this.tabWords.Controls.Add(this.btnAddWord);
        this.tabWords.Controls.Add(this.txtWordNote);
        this.tabWords.Controls.Add(this.lblWordNote);
        this.tabWords.Controls.Add(this.txtWord);
        this.tabWords.Controls.Add(this.lblWord);
        this.tabWords.Controls.Add(this.btnSearchWord);
        this.tabWords.Controls.Add(this.txtSearchWord);
        this.tabWords.Location = new System.Drawing.Point(4, 29);
        this.tabWords.Name = "tabWords";
        this.tabWords.Padding = new System.Windows.Forms.Padding(3);
        this.tabWords.Size = new System.Drawing.Size(746, 454);
        this.tabWords.TabIndex = 1;
        this.tabWords.Text = "🤬 Словарь оскорблений";
        this.tabWords.UseVisualStyleBackColor = true;
        //
        // tabProviders — ИИ-провайдеры
        //
        this.tabProviders.Controls.Add(this.dgvProviders);
        this.tabProviders.Controls.Add(this.btnProvTest);
        this.tabProviders.Controls.Add(this.btnProvActivate);
        this.tabProviders.Controls.Add(this.btnProvDelete);
        this.tabProviders.Controls.Add(this.btnProvUpdate);
        this.tabProviders.Controls.Add(this.btnProvAdd);
        this.tabProviders.Controls.Add(this.chkProvActive);
        this.tabProviders.Controls.Add(this.txtProvDesc);
        this.tabProviders.Controls.Add(this.lblProvDesc);
        this.tabProviders.Controls.Add(this.txtProvModel);
        this.tabProviders.Controls.Add(this.lblProvModel);
        this.tabProviders.Controls.Add(this.txtProvKey);
        this.tabProviders.Controls.Add(this.lblProvKey);
        this.tabProviders.Controls.Add(this.txtProvUrl);
        this.tabProviders.Controls.Add(this.lblProvUrl);
        this.tabProviders.Controls.Add(this.txtProvName);
        this.tabProviders.Controls.Add(this.lblProvName);
        this.tabProviders.Location = new System.Drawing.Point(4, 29);
        this.tabProviders.Name = "tabProviders";
        this.tabProviders.Padding = new System.Windows.Forms.Padding(3);
        this.tabProviders.Size = new System.Drawing.Size(746, 454);
        this.tabProviders.TabIndex = 2;
        this.tabProviders.Text = "🤖 ИИ-провайдеры";
        this.tabProviders.UseVisualStyleBackColor = true;
        //
        // lblProvName
        //
        this.lblProvName.AutoSize = true;
        this.lblProvName.Location = new System.Drawing.Point(13, 16);
        this.lblProvName.Name = "lblProvName";
        this.lblProvName.Size = new System.Drawing.Size(85, 20);
        this.lblProvName.TabIndex = 0;
        this.lblProvName.Text = "Название";
        //
        // txtProvName
        //
        this.txtProvName.Location = new System.Drawing.Point(110, 13);
        this.txtProvName.Name = "txtProvName";
        this.txtProvName.Size = new System.Drawing.Size(190, 27);
        this.txtProvName.TabIndex = 1;
        //
        // lblProvUrl
        //
        this.lblProvUrl.AutoSize = true;
        this.lblProvUrl.Location = new System.Drawing.Point(310, 16);
        this.lblProvUrl.Name = "lblProvUrl";
        this.lblProvUrl.Size = new System.Drawing.Size(72, 20);
        this.lblProvUrl.TabIndex = 2;
        this.lblProvUrl.Text = "Base URL";
        //
        // txtProvUrl
        //
        this.txtProvUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
        this.txtProvUrl.Location = new System.Drawing.Point(400, 13);
        this.txtProvUrl.Name = "txtProvUrl";
        this.txtProvUrl.Size = new System.Drawing.Size(330, 27);
        this.txtProvUrl.TabIndex = 3;
        //
        // lblProvKey
        //
        this.lblProvKey.AutoSize = true;
        this.lblProvKey.Location = new System.Drawing.Point(13, 50);
        this.lblProvKey.Name = "lblProvKey";
        this.lblProvKey.Size = new System.Drawing.Size(68, 20);
        this.lblProvKey.TabIndex = 4;
        this.lblProvKey.Text = "API-ключ";
        //
        // txtProvKey
        //
        this.txtProvKey.Location = new System.Drawing.Point(110, 47);
        this.txtProvKey.Name = "txtProvKey";
        this.txtProvKey.Size = new System.Drawing.Size(300, 27);
        this.txtProvKey.TabIndex = 5;
        //
        // lblProvModel
        //
        this.lblProvModel.AutoSize = true;
        this.lblProvModel.Location = new System.Drawing.Point(420, 50);
        this.lblProvModel.Name = "lblProvModel";
        this.lblProvModel.Size = new System.Drawing.Size(59, 20);
        this.lblProvModel.TabIndex = 6;
        this.lblProvModel.Text = "Модель";
        //
        // txtProvModel
        //
        this.txtProvModel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
        this.txtProvModel.Location = new System.Drawing.Point(490, 47);
        this.txtProvModel.Name = "txtProvModel";
        this.txtProvModel.Size = new System.Drawing.Size(240, 27);
        this.txtProvModel.TabIndex = 7;
        //
        // lblProvDesc
        //
        this.lblProvDesc.AutoSize = true;
        this.lblProvDesc.Location = new System.Drawing.Point(13, 84);
        this.lblProvDesc.Name = "lblProvDesc";
        this.lblProvDesc.Size = new System.Drawing.Size(85, 20);
        this.lblProvDesc.TabIndex = 8;
        this.lblProvDesc.Text = "Описание";
        //
        // txtProvDesc
        //
        this.txtProvDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
        this.txtProvDesc.Location = new System.Drawing.Point(110, 81);
        this.txtProvDesc.Name = "txtProvDesc";
        this.txtProvDesc.Size = new System.Drawing.Size(620, 27);
        this.txtProvDesc.TabIndex = 9;
        //
        // chkProvActive
        //
        this.chkProvActive.AutoSize = true;
        this.chkProvActive.Location = new System.Drawing.Point(13, 118);
        this.chkProvActive.Name = "chkProvActive";
        this.chkProvActive.Size = new System.Drawing.Size(300, 24);
        this.chkProvActive.TabIndex = 10;
        this.chkProvActive.Text = "Приоритетный (галочка использования)";
        this.chkProvActive.UseVisualStyleBackColor = true;
        //
        // btnProvAdd
        //
        this.btnProvAdd.BackColor = System.Drawing.Color.FromArgb(88, 101, 242);
        this.btnProvAdd.ForeColor = System.Drawing.Color.White;
        this.btnProvAdd.Location = new System.Drawing.Point(330, 112);
        this.btnProvAdd.Name = "btnProvAdd";
        this.btnProvAdd.Size = new System.Drawing.Size(120, 34);
        this.btnProvAdd.TabIndex = 11;
        this.btnProvAdd.Text = "➕ Добавить";
        this.btnProvAdd.UseVisualStyleBackColor = false;
        this.btnProvAdd.Click += new System.EventHandler(this.BtnProvAdd_Click);
        //
        // btnProvUpdate
        //
        this.btnProvUpdate.BackColor = System.Drawing.Color.FromArgb(38, 166, 91);
        this.btnProvUpdate.ForeColor = System.Drawing.Color.White;
        this.btnProvUpdate.Location = new System.Drawing.Point(460, 112);
        this.btnProvUpdate.Name = "btnProvUpdate";
        this.btnProvUpdate.Size = new System.Drawing.Size(120, 34);
        this.btnProvUpdate.TabIndex = 12;
        this.btnProvUpdate.Text = "✏️ Обновить";
        this.btnProvUpdate.UseVisualStyleBackColor = false;
        this.btnProvUpdate.Click += new System.EventHandler(this.BtnProvUpdate_Click);
        //
        // btnProvDelete
        //
        this.btnProvDelete.Location = new System.Drawing.Point(590, 112);
        this.btnProvDelete.Name = "btnProvDelete";
        this.btnProvDelete.Size = new System.Drawing.Size(120, 34);
        this.btnProvDelete.TabIndex = 13;
        this.btnProvDelete.Text = "🗑 Удалить";
        this.btnProvDelete.UseVisualStyleBackColor = true;
        this.btnProvDelete.Click += new System.EventHandler(this.BtnProvDelete_Click);
        //
        // btnProvActivate
        //
        this.btnProvActivate.BackColor = System.Drawing.Color.FromArgb(253, 185, 19);
        this.btnProvActivate.Location = new System.Drawing.Point(590, 152);
        this.btnProvActivate.Name = "btnProvActivate";
        this.btnProvActivate.Size = new System.Drawing.Size(120, 34);
        this.btnProvActivate.TabIndex = 14;
        this.btnProvActivate.Text = "⭐ Приоритет";
        this.btnProvActivate.UseVisualStyleBackColor = false;
        this.btnProvActivate.Click += new System.EventHandler(this.BtnProvActivate_Click);
        //
        // btnProvTest
        //
        this.btnProvTest.Location = new System.Drawing.Point(460, 152);
        this.btnProvTest.Name = "btnProvTest";
        this.btnProvTest.Size = new System.Drawing.Size(120, 34);
        this.btnProvTest.TabIndex = 15;
        this.btnProvTest.Text = "🔌 Тест API";
        this.btnProvTest.UseVisualStyleBackColor = true;
        this.btnProvTest.Click += new System.EventHandler(this.BtnProvTest_Click);
        //
        // dgvProviders
        //
        this.dgvProviders.AllowUserToAddRows = false;
        this.dgvProviders.AllowUserToDeleteRows = false;
        this.dgvProviders.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
        | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
        this.dgvProviders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        this.dgvProviders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvProviders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colProvId,
            this.colProvName,
            this.colProvUrl,
            this.colProvModel,
            this.colProvActive,
            this.colProvDesc});
        dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
        dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
        dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
        this.dgvProviders.DefaultCellStyle = dataGridViewCellStyle2;
        this.dgvProviders.Location = new System.Drawing.Point(13, 194);
        this.dgvProviders.MultiSelect = false;
        this.dgvProviders.Name = "dgvProviders";
        this.dgvProviders.ReadOnly = true;
        this.dgvProviders.RowHeadersVisible = false;
        this.dgvProviders.RowTemplate.Height = 29;
        this.dgvProviders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.dgvProviders.Size = new System.Drawing.Size(717, 254);
        this.dgvProviders.TabIndex = 16;
        this.dgvProviders.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvProviders_CellDoubleClick);
        //
        // colProvId
        //
        this.colProvId.FillWeight = 25F;
        this.colProvId.HeaderText = "ID";
        this.colProvId.MinimumWidth = 40;
        this.colProvId.Name = "colProvId";
        //
        // colProvName
        //
        this.colProvName.FillWeight = 60F;
        this.colProvName.HeaderText = "Название";
        this.colProvName.MinimumWidth = 70;
        this.colProvName.Name = "colProvName";
        //
        // colProvUrl
        //
        this.colProvUrl.FillWeight = 90F;
        this.colProvUrl.HeaderText = "Base URL";
        this.colProvUrl.MinimumWidth = 80;
        this.colProvUrl.Name = "colProvUrl";
        //
        // colProvModel
        //
        this.colProvModel.FillWeight = 70F;
        this.colProvModel.HeaderText = "Модель";
        this.colProvModel.MinimumWidth = 60;
        this.colProvModel.Name = "colProvModel";
        //
        // colProvActive
        //
        this.colProvActive.FillWeight = 30F;
        this.colProvActive.HeaderText = "⭐";
        this.colProvActive.MinimumWidth = 30;
        this.colProvActive.Name = "colProvActive";
        //
        // colProvDesc
        //
        this.colProvDesc.FillWeight = 90F;
        this.colProvDesc.HeaderText = "Описание";
        this.colProvDesc.MinimumWidth = 70;
        this.colProvDesc.Name = "colProvDesc";
        //
        // tabIndividual — персональные обращения
        //
        this.tabIndividual.Controls.Add(this.dgvIndividual);
        this.tabIndividual.Controls.Add(this.btnIndDelete);
        this.tabIndividual.Controls.Add(this.btnIndUpdate);
        this.tabIndividual.Controls.Add(this.btnIndAdd);
        this.tabIndividual.Controls.Add(this.txtIndDesc);
        this.tabIndividual.Controls.Add(this.lblIndDesc);
        this.tabIndividual.Controls.Add(this.txtIndNick);
        this.tabIndividual.Controls.Add(this.lblIndNick);
        this.tabIndividual.Location = new System.Drawing.Point(4, 29);
        this.tabIndividual.Name = "tabIndividual";
        this.tabIndividual.Padding = new System.Windows.Forms.Padding(3);
        this.tabIndividual.Size = new System.Drawing.Size(746, 454);
        this.tabIndividual.TabIndex = 3;
        this.tabIndividual.Text = "👋 Обращения";
        this.tabIndividual.UseVisualStyleBackColor = true;
        //
        // lblIndNick
        //
        this.lblIndNick.AutoSize = true;
        this.lblIndNick.Location = new System.Drawing.Point(13, 16);
        this.lblIndNick.Name = "lblIndNick";
        this.lblIndNick.Size = new System.Drawing.Size(70, 20);
        this.lblIndNick.TabIndex = 0;
        this.lblIndNick.Text = "Ник";
        //
        // txtIndNick
        //
        this.txtIndNick.Location = new System.Drawing.Point(90, 13);
        this.txtIndNick.Name = "txtIndNick";
        this.txtIndNick.Size = new System.Drawing.Size(220, 27);
        this.txtIndNick.TabIndex = 1;
        //
        // lblIndDesc
        //
        this.lblIndDesc.AutoSize = true;
        this.lblIndDesc.Location = new System.Drawing.Point(13, 50);
        this.lblIndDesc.Name = "lblIndDesc";
        this.lblIndDesc.Size = new System.Drawing.Size(170, 20);
        this.lblIndDesc.TabIndex = 2;
        this.lblIndDesc.Text = "Варианты обращений (через ;)";
        //
        // txtIndDesc
        //
        this.txtIndDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
        this.txtIndDesc.Location = new System.Drawing.Point(13, 74);
        this.txtIndDesc.Multiline = true;
        this.txtIndDesc.Name = "txtIndDesc";
        this.txtIndDesc.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.txtIndDesc.Size = new System.Drawing.Size(717, 70);
        this.txtIndDesc.TabIndex = 3;
        //
        // btnIndAdd
        //
        this.btnIndAdd.BackColor = System.Drawing.Color.FromArgb(88, 101, 242);
        this.btnIndAdd.ForeColor = System.Drawing.Color.White;
        this.btnIndAdd.Location = new System.Drawing.Point(90, 152);
        this.btnIndAdd.Name = "btnIndAdd";
        this.btnIndAdd.Size = new System.Drawing.Size(130, 34);
        this.btnIndAdd.TabIndex = 4;
        this.btnIndAdd.Text = "➕ Добавить";
        this.btnIndAdd.UseVisualStyleBackColor = false;
        this.btnIndAdd.Click += new System.EventHandler(this.BtnIndAdd_Click);
        //
        // btnIndUpdate
        //
        this.btnIndUpdate.BackColor = System.Drawing.Color.FromArgb(38, 166, 91);
        this.btnIndUpdate.ForeColor = System.Drawing.Color.White;
        this.btnIndUpdate.Location = new System.Drawing.Point(230, 152);
        this.btnIndUpdate.Name = "btnIndUpdate";
        this.btnIndUpdate.Size = new System.Drawing.Size(130, 34);
        this.btnIndUpdate.TabIndex = 5;
        this.btnIndUpdate.Text = "✏️ Обновить";
        this.btnIndUpdate.UseVisualStyleBackColor = false;
        this.btnIndUpdate.Click += new System.EventHandler(this.BtnIndUpdate_Click);
        //
        // btnIndDelete
        //
        this.btnIndDelete.Location = new System.Drawing.Point(370, 152);
        this.btnIndDelete.Name = "btnIndDelete";
        this.btnIndDelete.Size = new System.Drawing.Size(130, 34);
        this.btnIndDelete.TabIndex = 6;
        this.btnIndDelete.Text = "🗑 Удалить";
        this.btnIndDelete.UseVisualStyleBackColor = true;
        this.btnIndDelete.Click += new System.EventHandler(this.BtnIndDelete_Click);
        //
        // dgvIndividual
        //
        this.dgvIndividual.AllowUserToAddRows = false;
        this.dgvIndividual.AllowUserToDeleteRows = false;
        this.dgvIndividual.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
        | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
        this.dgvIndividual.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        this.dgvIndividual.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvIndividual.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colIndId,
            this.colIndNick,
            this.colIndDesc,
            this.colIndUpdated});
        dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
        dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
        dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
        this.dgvIndividual.DefaultCellStyle = dataGridViewCellStyle2;
        this.dgvIndividual.Location = new System.Drawing.Point(13, 194);
        this.dgvIndividual.MultiSelect = false;
        this.dgvIndividual.Name = "dgvIndividual";
        this.dgvIndividual.ReadOnly = true;
        this.dgvIndividual.RowHeadersVisible = false;
        this.dgvIndividual.RowTemplate.Height = 29;
        this.dgvIndividual.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.dgvIndividual.Size = new System.Drawing.Size(717, 254);
        this.dgvIndividual.TabIndex = 7;
        this.dgvIndividual.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvIndividual_CellDoubleClick);
        //
        // colIndId
        //
        this.colIndId.FillWeight = 25F;
        this.colIndId.HeaderText = "ID";
        this.colIndId.MinimumWidth = 40;
        this.colIndId.Name = "colIndId";
        //
        // colIndNick
        //
        this.colIndNick.FillWeight = 60F;
        this.colIndNick.HeaderText = "Ник";
        this.colIndNick.MinimumWidth = 70;
        this.colIndNick.Name = "colIndNick";
        //
        // colIndDesc
        //
        this.colIndDesc.FillWeight = 150F;
        this.colIndDesc.HeaderText = "Варианты обращений";
        this.colIndDesc.MinimumWidth = 100;
        this.colIndDesc.Name = "colIndDesc";
        //
        // colIndUpdated
        //
        this.colIndUpdated.FillWeight = 55F;
        this.colIndUpdated.HeaderText = "Обновлено";
        this.colIndUpdated.MinimumWidth = 80;
        this.colIndUpdated.Name = "colIndUpdated";
        // 
        // lblWord
        // 
        this.lblWord.AutoSize = true;
        this.lblWord.Location = new System.Drawing.Point(13, 16);
        this.lblWord.Name = "lblWord";
        this.lblWord.Size = new System.Drawing.Size(46, 20);
        this.lblWord.TabIndex = 0;
        this.lblWord.Text = "Слово";
        // 
        // txtWord
        // 
        this.txtWord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
        this.txtWord.Location = new System.Drawing.Point(85, 13);
        this.txtWord.Name = "txtWord";
        this.txtWord.Size = new System.Drawing.Size(300, 27);
        this.txtWord.TabIndex = 1;
        // 
        // lblWordNote
        // 
        this.lblWordNote.AutoSize = true;
        this.lblWordNote.Location = new System.Drawing.Point(400, 16);
        this.lblWordNote.Name = "lblWordNote";
        this.lblWordNote.Size = new System.Drawing.Size(66, 20);
        this.lblWordNote.TabIndex = 2;
        this.lblWordNote.Text = "Заметка";
        // 
        // txtWordNote
        // 
        this.txtWordNote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
        this.txtWordNote.Location = new System.Drawing.Point(478, 13);
        this.txtWordNote.Name = "txtWordNote";
        this.txtWordNote.Size = new System.Drawing.Size(252, 27);
        this.txtWordNote.TabIndex = 3;
        // 
        // btnAddWord
        // 
        this.btnAddWord.BackColor = System.Drawing.Color.FromArgb(88, 101, 242);
        this.btnAddWord.ForeColor = System.Drawing.Color.White;
        this.btnAddWord.Location = new System.Drawing.Point(85, 50);
        this.btnAddWord.Name = "btnAddWord";
        this.btnAddWord.Size = new System.Drawing.Size(130, 38);
        this.btnAddWord.TabIndex = 4;
        this.btnAddWord.Text = "➕ Добавить";
        this.btnAddWord.UseVisualStyleBackColor = false;
        this.btnAddWord.Click += new System.EventHandler(this.BtnAddWord_Click);
        // 
        // btnUpdateWord
        // 
        this.btnUpdateWord.BackColor = System.Drawing.Color.FromArgb(38, 166, 91);
        this.btnUpdateWord.ForeColor = System.Drawing.Color.White;
        this.btnUpdateWord.Enabled = false;
        this.btnUpdateWord.Location = new System.Drawing.Point(225, 50);
        this.btnUpdateWord.Name = "btnUpdateWord";
        this.btnUpdateWord.Size = new System.Drawing.Size(130, 38);
        this.btnUpdateWord.TabIndex = 5;
        this.btnUpdateWord.Text = "✏️ Обновить";
        this.btnUpdateWord.UseVisualStyleBackColor = false;
        this.btnUpdateWord.Click += new System.EventHandler(this.BtnUpdateWord_Click);
        // 
        // btnDeleteWord
        // 
        this.btnDeleteWord.Location = new System.Drawing.Point(365, 50);
        this.btnDeleteWord.Name = "btnDeleteWord";
        this.btnDeleteWord.Size = new System.Drawing.Size(130, 38);
        this.btnDeleteWord.TabIndex = 6;
        this.btnDeleteWord.Text = "🗑 Удалить";
        this.btnDeleteWord.UseVisualStyleBackColor = true;
        this.btnDeleteWord.Click += new System.EventHandler(this.BtnDeleteWord_Click);
        // 
        // txtSearchWord
        // 
        this.txtSearchWord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.txtSearchWord.Location = new System.Drawing.Point(478, 57);
        this.txtSearchWord.Name = "txtSearchWord";
        this.txtSearchWord.Size = new System.Drawing.Size(150, 27);
        this.txtSearchWord.TabIndex = 7;
        // 
        // btnSearchWord
        // 
        this.btnSearchWord.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.btnSearchWord.Location = new System.Drawing.Point(634, 53);
        this.btnSearchWord.Name = "btnSearchWord";
        this.btnSearchWord.Size = new System.Drawing.Size(96, 31);
        this.btnSearchWord.TabIndex = 8;
        this.btnSearchWord.Text = "🔍 Поиск";
        this.btnSearchWord.UseVisualStyleBackColor = true;
        this.btnSearchWord.Click += new System.EventHandler(this.BtnSearchWord_Click);
        // 
        // dgvWords
        // 
        this.dgvWords.AllowUserToAddRows = false;
        this.dgvWords.AllowUserToDeleteRows = false;
        this.dgvWords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
        | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
        this.dgvWords.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        this.dgvWords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvWords.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colWordId,
            this.colWordText,
            this.colWordNote,
            this.colWordAdded});
        dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
        dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
        dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
        dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
        dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
        dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
        this.dgvWords.DefaultCellStyle = dataGridViewCellStyle2;
        this.dgvWords.Location = new System.Drawing.Point(13, 100);
        this.dgvWords.MultiSelect = false;
        this.dgvWords.Name = "dgvWords";
        this.dgvWords.ReadOnly = true;
        this.dgvWords.RowHeadersVisible = false;
        this.dgvWords.RowTemplate.Height = 29;
        this.dgvWords.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        this.dgvWords.Size = new System.Drawing.Size(717, 348);
        this.dgvWords.TabIndex = 9;
        this.dgvWords.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvWords_CellDoubleClick);
        // 
        // colWordId
        // 
        this.colWordId.FillWeight = 30F;
        this.colWordId.HeaderText = "ID";
        this.colWordId.MinimumWidth = 50;
        this.colWordId.Name = "colWordId";
        // 
        // colWordText
        // 
        this.colWordText.FillWeight = 70F;
        this.colWordText.HeaderText = "Слово / корень";
        this.colWordText.MinimumWidth = 80;
        this.colWordText.Name = "colWordText";
        // 
        // colWordNote
        // 
        this.colWordNote.FillWeight = 100F;
        this.colWordNote.HeaderText = "Заметка";
        this.colWordNote.MinimumWidth = 80;
        this.colWordNote.Name = "colWordNote";
        // 
        // colWordAdded
        // 
        this.colWordAdded.FillWeight = 60F;
        this.colWordAdded.HeaderText = "Добавлено";
        this.colWordAdded.MinimumWidth = 90;
        this.colWordAdded.Name = "colWordAdded";
        // 
        // Form1
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(786, 581);
        this.Controls.Add(this.lblStatus);
        this.Controls.Add(this.tabControl);
        this.Controls.Add(this.lblConnection);
        this.Controls.Add(this.lblTitle);
        this.MinimumSize = new System.Drawing.Size(700, 480);
        this.Name = "Form1";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "База знаний Dan Games";
        ((System.ComponentModel.ISupportInitialize)(this.dgvKnowledge)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dgvWords)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dgvProviders)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.dgvIndividual)).EndInit();
        this.tabControl.ResumeLayout(false);
        this.tabKnowledge.ResumeLayout(false);
        this.tabKnowledge.PerformLayout();
        this.tabWords.ResumeLayout(false);
        this.tabWords.PerformLayout();
        this.tabProviders.ResumeLayout(false);
        this.tabProviders.PerformLayout();
        this.tabIndividual.ResumeLayout(false);
        this.tabIndividual.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Label lblConnection;
    private System.Windows.Forms.Label lblTheme;
    private System.Windows.Forms.TextBox txtTheme;
    private System.Windows.Forms.Label lblDescription;
    private System.Windows.Forms.TextBox txtDescription;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Button btnDelete;
    private System.Windows.Forms.Button btnUpdate;
    private System.Windows.Forms.Button btnTest;
    private System.Windows.Forms.Button btnSettings;
    private System.Windows.Forms.DataGridView dgvKnowledge;
    private System.Windows.Forms.Label lblStatus;
    private System.Windows.Forms.TabControl tabControl;
    private System.Windows.Forms.TabPage tabKnowledge;
    private System.Windows.Forms.TabPage tabWords;
    private System.Windows.Forms.Label lblWord;
    private System.Windows.Forms.TextBox txtWord;
    private System.Windows.Forms.Label lblWordNote;
    private System.Windows.Forms.TextBox txtWordNote;
    private System.Windows.Forms.Button btnAddWord;
    private System.Windows.Forms.Button btnUpdateWord;
    private System.Windows.Forms.Button btnDeleteWord;
    private System.Windows.Forms.Button btnSearchWord;
    private System.Windows.Forms.TextBox txtSearchWord;
    private System.Windows.Forms.DataGridView dgvWords;
    private System.Windows.Forms.TabPage tabProviders;
    private System.Windows.Forms.Label lblProvName;
    private System.Windows.Forms.TextBox txtProvName;
    private System.Windows.Forms.Label lblProvUrl;
    private System.Windows.Forms.TextBox txtProvUrl;
    private System.Windows.Forms.Label lblProvKey;
    private System.Windows.Forms.TextBox txtProvKey;
    private System.Windows.Forms.Label lblProvModel;
    private System.Windows.Forms.TextBox txtProvModel;
    private System.Windows.Forms.Label lblProvDesc;
    private System.Windows.Forms.TextBox txtProvDesc;
    private System.Windows.Forms.CheckBox chkProvActive;
    private System.Windows.Forms.Button btnProvAdd;
    private System.Windows.Forms.Button btnProvUpdate;
    private System.Windows.Forms.Button btnProvDelete;
    private System.Windows.Forms.Button btnProvActivate;
    private System.Windows.Forms.Button btnProvTest;
    private System.Windows.Forms.DataGridView dgvProviders;
    private System.Windows.Forms.DataGridViewTextBoxColumn colProvId;
    private System.Windows.Forms.DataGridViewTextBoxColumn colProvName;
    private System.Windows.Forms.DataGridViewTextBoxColumn colProvUrl;
    private System.Windows.Forms.DataGridViewTextBoxColumn colProvModel;
    private System.Windows.Forms.DataGridViewTextBoxColumn colProvActive;
    private System.Windows.Forms.DataGridViewTextBoxColumn colProvDesc;
    private System.Windows.Forms.TabPage tabIndividual;
    private System.Windows.Forms.Label lblIndNick;
    private System.Windows.Forms.TextBox txtIndNick;
    private System.Windows.Forms.Label lblIndDesc;
    private System.Windows.Forms.TextBox txtIndDesc;
    private System.Windows.Forms.Button btnIndAdd;
    private System.Windows.Forms.Button btnIndUpdate;
    private System.Windows.Forms.Button btnIndDelete;
    private System.Windows.Forms.DataGridView dgvIndividual;
    private System.Windows.Forms.DataGridViewTextBoxColumn colIndId;
    private System.Windows.Forms.DataGridViewTextBoxColumn colIndNick;
    private System.Windows.Forms.DataGridViewTextBoxColumn colIndDesc;
    private System.Windows.Forms.DataGridViewTextBoxColumn colIndUpdated;
    private System.Windows.Forms.DataGridViewTextBoxColumn colWordId;
    private System.Windows.Forms.DataGridViewTextBoxColumn colWordText;
    private System.Windows.Forms.DataGridViewTextBoxColumn colWordNote;
    private System.Windows.Forms.DataGridViewTextBoxColumn colWordAdded;
    private System.Windows.Forms.DataGridViewTextBoxColumn colId;
    private System.Windows.Forms.DataGridViewTextBoxColumn colTheme;
    private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
    private System.Windows.Forms.DataGridViewTextBoxColumn colEmbed;
    private System.Windows.Forms.DataGridViewTextBoxColumn colUpdated;
}