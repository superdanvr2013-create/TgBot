namespace KnowledgeEditor;

public sealed class ErrorDialog : Form
{
    private readonly TextBox txt = new();

    public ErrorDialog(string caption, string message)
    {
        Text = caption;
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(640, 380);
        MinimumSize = new Size(400, 300);
        FormBorderStyle = FormBorderStyle.SizableToolWindow;
        MaximizeBox = false;

        txt.Multiline = true;
        txt.ReadOnly = true;
        txt.ScrollBars = ScrollBars.Both;
        txt.WordWrap = false;
        txt.Font = new Font("Consolas", 10F);
        txt.Location = new Point(12, 12);
        txt.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        txt.Size = new Size(616, 320);
        txt.Text = message;

        var btnCopy = new Button
        {
            Text = "📋 Копировать",
            Dock = DockStyle.Bottom,
            Height = 40,
        };
        btnCopy.Click += (_, _) =>
        {
            try
            {
                Clipboard.SetText(txt.Text);
                MessageBox.Show(this, "Текст ошибки скопирован в буфер обмена.", "Готово",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show(this, "Не удалось скопировать в буфер.", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        };

        var btnClose = new Button
        {
            Text = "Закрыть",
            DialogResult = DialogResult.OK,
            Dock = DockStyle.Bottom,
            Height = 40,
        };

        var panel = new Panel { Dock = DockStyle.Bottom, Height = 44 };
        // every button Dock=Bottom stacks top-to-bottom reversed; just size them side by side
        btnCopy.Dock = DockStyle.None;
        btnClose.Dock = DockStyle.None;
        panel.Controls.Add(btnClose);
        panel.Controls.Add(btnCopy);

        btnClose.Location = new Point(536, 6);
        btnClose.Size = new Size(92, 34);
        btnCopy.Location = new Point(412, 6);
        btnCopy.Size = new Size(118, 34);
        btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnCopy.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

        Controls.Add(txt);
        Controls.Add(panel);

        // panel stretch across bottom
        panel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        panel.Location = new Point(0, ClientSize.Height - 44);
        panel.Width = ClientSize.Width;
        panel.BackColor = SystemColors.Control;

        AcceptButton = btnClose;
        CancelButton = btnClose;
        Shown += (_, _) => txt.Focus();
    }
}