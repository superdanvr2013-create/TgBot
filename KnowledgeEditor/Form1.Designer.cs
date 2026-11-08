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
        ((System.ComponentModel.ISupportInitialize)(this.dgvKnowledge)).BeginInit();
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
        // Form1
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(786, 581);
        this.Controls.Add(this.lblStatus);
        this.Controls.Add(this.dgvKnowledge);
        this.Controls.Add(this.btnSettings);
        this.Controls.Add(this.btnTest);
        this.Controls.Add(this.btnUpdate);
        this.Controls.Add(this.btnDelete);
        this.Controls.Add(this.btnSave);
        this.Controls.Add(this.txtDescription);
        this.Controls.Add(this.lblDescription);
        this.Controls.Add(this.txtTheme);
        this.Controls.Add(this.lblTheme);
        this.Controls.Add(this.lblConnection);
        this.Controls.Add(this.lblTitle);
        this.MinimumSize = new System.Drawing.Size(700, 480);
        this.Name = "Form1";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "База знаний Dan Games";
        ((System.ComponentModel.ISupportInitialize)(this.dgvKnowledge)).EndInit();
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
    private System.Windows.Forms.DataGridViewTextBoxColumn colId;
    private System.Windows.Forms.DataGridViewTextBoxColumn colTheme;
    private System.Windows.Forms.DataGridViewTextBoxColumn colDescription;
    private System.Windows.Forms.DataGridViewTextBoxColumn colEmbed;
    private System.Windows.Forms.DataGridViewTextBoxColumn colUpdated;
}