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
        // ── TabControl: вкладки «Знания» и «Словарь оскорблений» ──
        this.tabControl = new System.Windows.Forms.TabControl();
        this.tabKnowledge = new System.Windows.Forms.TabPage();
        this.tabWords = new System.Windows.Forms.TabPage();
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
        this.tabControl.SuspendLayout();
        this.tabKnowledge.SuspendLayout();
        this.tabWords.SuspendLayout();
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
        this.txtDescription.Multiline = true;
        this.txtDescription.Name = "txtDescription";
        this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        this.txtDescription.Size = new System.Drawing.Size(650, 100);
        this.txtDescription.TabIndex = 5;
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
        this.tabControl.ResumeLayout(false);
        this.tabKnowledge.ResumeLayout(false);
        this.tabKnowledge.PerformLayout();
        this.tabWords.ResumeLayout(false);
        this.tabWords.PerformLayout();
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