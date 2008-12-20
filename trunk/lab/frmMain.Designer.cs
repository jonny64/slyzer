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
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.анализToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.пускToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.синтаксическийАнализToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rtbOutput = new System.Windows.Forms.RichTextBox();
            this.rtbInput = new System.Windows.Forms.RichTextBox();
            this.scEditArea = new System.Windows.Forms.SplitContainer();
            this.listBoxMsg = new System.Windows.Forms.ListBox();
            this.нисходящийРазборToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.восходящийРазборToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.выходToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.выходToolStripMenuItem.Text = "Выход";
            this.выходToolStripMenuItem.Click += new System.EventHandler(this.Exit);
            // 
            // анализToolStripMenuItem
            // 
            this.анализToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.пускToolStripMenuItem,
            this.синтаксическийАнализToolStripMenuItem});
            this.анализToolStripMenuItem.Name = "анализToolStripMenuItem";
            this.анализToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.анализToolStripMenuItem.Text = "Пуск";
            // 
            // пускToolStripMenuItem
            // 
            this.пускToolStripMenuItem.Name = "пускToolStripMenuItem";
            this.пускToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.пускToolStripMenuItem.Text = "Лексический анализ";
            this.пускToolStripMenuItem.Click += new System.EventHandler(this.StartLexicalAnalysis);
            // 
            // синтаксическийАнализToolStripMenuItem
            // 
            this.синтаксическийАнализToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.нисходящийРазборToolStripMenuItem,
            this.восходящийРазборToolStripMenuItem});
            this.синтаксическийАнализToolStripMenuItem.Name = "синтаксическийАнализToolStripMenuItem";
            this.синтаксическийАнализToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.синтаксическийАнализToolStripMenuItem.Text = "Синтаксический анализ";
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
            // rtbInput
            // 
            this.rtbInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbInput.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.rtbInput.Location = new System.Drawing.Point(0, 0);
            this.rtbInput.Name = "rtbInput";
            this.rtbInput.Size = new System.Drawing.Size(246, 385);
            this.rtbInput.TabIndex = 0;
            this.rtbInput.Text = "record\nfield1:boolean;\nfield3,field4:integer\nend";
            // 
            // scEditArea
            // 
            this.scEditArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scEditArea.Location = new System.Drawing.Point(0, 24);
            this.scEditArea.Name = "scEditArea";
            // 
            // scEditArea.Panel1
            // 
            this.scEditArea.Panel1.Controls.Add(this.rtbInput);
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
            // нисходящийРазборToolStripMenuItem
            // 
            this.нисходящийРазборToolStripMenuItem.Name = "нисходящийРазборToolStripMenuItem";
            this.нисходящийРазборToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.нисходящийРазборToolStripMenuItem.Text = "Нисходящий разбор";
            this.нисходящийРазборToolStripMenuItem.Click += new System.EventHandler(this.синтаксическийАнализToolStripMenuItem_Click);
            // 
            // восходящийРазборToolStripMenuItem
            // 
            this.восходящийРазборToolStripMenuItem.Name = "восходящийРазборToolStripMenuItem";
            this.восходящийРазборToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.восходящийРазборToolStripMenuItem.Text = "Восходящий разбор";
            this.восходящийРазборToolStripMenuItem.Click += new System.EventHandler(this.восходящийРазборToolStripMenuItem_Click);
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
        private System.Windows.Forms.ToolStripMenuItem пускToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem синтаксическийАнализToolStripMenuItem;
        private System.Windows.Forms.RichTextBox rtbOutput;
        private System.Windows.Forms.RichTextBox rtbInput;
        private System.Windows.Forms.SplitContainer scEditArea;
        private System.Windows.Forms.ListBox listBoxMsg;
        private System.Windows.Forms.ToolStripMenuItem нисходящийРазборToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem восходящийРазборToolStripMenuItem;
    }
}

