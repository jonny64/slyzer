namespace lab
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mstripMainMenu = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.открытьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сохранитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.анализToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rtbOutput = new System.Windows.Forms.RichTextBox();
            this.scEditArea = new System.Windows.Forms.SplitContainer();
            this.listBoxMsg = new System.Windows.Forms.ListBox();
            this.openFileDialogInput = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialogInput = new System.Windows.Forms.SaveFileDialog();
            this.tbInput = new System.Windows.Forms.TextBox();
            this.synToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mstripMainMenu.SuspendLayout();
            this.scEditArea.Panel1.SuspendLayout();
            this.scEditArea.Panel2.SuspendLayout();
            this.scEditArea.SuspendLayout();
            this.SuspendLayout();
            // 
            // mstripMainMenu
            // 
            this.mstripMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.анализToolStripMenuItem});
            this.mstripMainMenu.Location = new System.Drawing.Point(0, 0);
            this.mstripMainMenu.Name = "mstripMainMenu";
            this.mstripMainMenu.Size = new System.Drawing.Size(513, 24);
            this.mstripMainMenu.TabIndex = 1;
            this.mstripMainMenu.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.открытьToolStripMenuItem,
            this.сохранитьToolStripMenuItem,
            this.выходToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // открытьToolStripMenuItem
            // 
            this.открытьToolStripMenuItem.Name = "открытьToolStripMenuItem";
            this.открытьToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.открытьToolStripMenuItem.Text = "Открыть";
            this.открытьToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // сохранитьToolStripMenuItem
            // 
            this.сохранитьToolStripMenuItem.Name = "сохранитьToolStripMenuItem";
            this.сохранитьToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.сохранитьToolStripMenuItem.Text = "Сохранить";
            this.сохранитьToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.выходToolStripMenuItem.Text = "Выход";
            this.выходToolStripMenuItem.Click += new System.EventHandler(this.Exit);
            // 
            // анализToolStripMenuItem
            // 
            this.анализToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lexToolStripMenuItem,
            this.synToolStripMenuItem});
            this.анализToolStripMenuItem.Name = "анализToolStripMenuItem";
            this.анализToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.анализToolStripMenuItem.Text = "Пуск";
            // 
            // lexToolStripMenuItem
            // 
            this.lexToolStripMenuItem.Name = "lexToolStripMenuItem";
            this.lexToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.lexToolStripMenuItem.Text = "Лексический анализ";
            this.lexToolStripMenuItem.Click += new System.EventHandler(this.StartLexicalAnalysis);
            // 
            // rtbOutput
            // 
            this.rtbOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbOutput.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbOutput.Location = new System.Drawing.Point(0, 0);
            this.rtbOutput.Name = "rtbOutput";
            this.rtbOutput.Size = new System.Drawing.Size(263, 385);
            this.rtbOutput.TabIndex = 0;
            this.rtbOutput.Text = "";
            // 
            // scEditArea
            // 
            this.scEditArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scEditArea.Location = new System.Drawing.Point(0, 24);
            this.scEditArea.Name = "scEditArea";
            // 
            // scEditArea.Panel1
            // 
            this.scEditArea.Panel1.Controls.Add(this.tbInput);
            // 
            // scEditArea.Panel2
            // 
            this.scEditArea.Panel2.Controls.Add(this.rtbOutput);
            this.scEditArea.Size = new System.Drawing.Size(513, 385);
            this.scEditArea.SplitterDistance = 246;
            this.scEditArea.TabIndex = 0;
            // 
            // listBoxMsg
            // 
            this.listBoxMsg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listBoxMsg.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBoxMsg.FormattingEnabled = true;
            this.listBoxMsg.ItemHeight = 16;
            this.listBoxMsg.Location = new System.Drawing.Point(0, 373);
            this.listBoxMsg.MinimumSize = new System.Drawing.Size(100, 50);
            this.listBoxMsg.Name = "listBoxMsg";
            this.listBoxMsg.Size = new System.Drawing.Size(513, 36);
            this.listBoxMsg.TabIndex = 4;
            // 
            // openFileDialogInput
            // 
            this.openFileDialogInput.FileName = "openFileDialog1";
            // 
            // tbInput
            // 
            this.tbInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbInput.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbInput.Location = new System.Drawing.Point(0, 0);
            this.tbInput.Multiline = true;
            this.tbInput.Name = "tbInput";
            this.tbInput.Size = new System.Drawing.Size(246, 385);
            this.tbInput.TabIndex = 0;
            // 
            // synToolStripMenuItem
            // 
            this.synToolStripMenuItem.Name = "synToolStripMenuItem";
            this.synToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.synToolStripMenuItem.Text = "Нисходящий разбор";
            this.synToolStripMenuItem.Click += new System.EventHandler(this.synToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 409);
            this.Controls.Add(this.listBoxMsg);
            this.Controls.Add(this.scEditArea);
            this.Controls.Add(this.mstripMainMenu);
            this.Name = "frmMain";
            this.Text = "main";
            this.mstripMainMenu.ResumeLayout(false);
            this.mstripMainMenu.PerformLayout();
            this.scEditArea.Panel1.ResumeLayout(false);
            this.scEditArea.Panel1.PerformLayout();
            this.scEditArea.Panel2.ResumeLayout(false);
            this.scEditArea.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mstripMainMenu;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem анализToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lexToolStripMenuItem;
        private System.Windows.Forms.RichTextBox rtbOutput;
        private System.Windows.Forms.SplitContainer scEditArea;
        private System.Windows.Forms.ListBox listBoxMsg;
        private System.Windows.Forms.OpenFileDialog openFileDialogInput;
        private System.Windows.Forms.SaveFileDialog saveFileDialogInput;
        private System.Windows.Forms.ToolStripMenuItem открытьToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сохранитьToolStripMenuItem;
        private System.Windows.Forms.TextBox tbInput;
        private System.Windows.Forms.ToolStripMenuItem synToolStripMenuItem;
    }
}

