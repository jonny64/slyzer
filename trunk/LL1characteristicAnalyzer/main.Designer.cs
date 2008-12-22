﻿namespace LL1AnalyzerTool
{
    partial class main
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
            this.components = new System.ComponentModel.Container();
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Open = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.анализToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.пускToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lL1ТаблицаРазбораToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialogGrammar = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialogGrammar = new System.Windows.Forms.SaveFileDialog();
            this.toolTipMain = new System.Windows.Forms.ToolTip(this.components);
            this.tbGrammar = new System.Windows.Forms.TextBox();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.tbTerminal = new System.Windows.Forms.TextBox();
            this.tbOutput = new System.Windows.Forms.TextBox();
            this.menuStripMain.SuspendLayout();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.анализToolStripMenuItem,
            this.справкаToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(709, 24);
            this.menuStripMain.TabIndex = 2;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Open,
            this.saveToolStripMenuItem,
            this.выходToolStripMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // Open
            // 
            this.Open.Name = "Open";
            this.Open.Size = new System.Drawing.Size(161, 22);
            this.Open.Text = "Открыть";
            this.Open.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.saveToolStripMenuItem.Text = "Сохранить как";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveToolStripMenuItem_Click);
            // 
            // выходToolStripMenuItem
            // 
            this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
            this.выходToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.выходToolStripMenuItem.Text = "Выход";
            this.выходToolStripMenuItem.Click += new System.EventHandler(this.выходToolStripMenuItem_Click);
            // 
            // анализToolStripMenuItem
            // 
            this.анализToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.пускToolStripMenuItem,
            this.lL1ТаблицаРазбораToolStripMenuItem});
            this.анализToolStripMenuItem.Name = "анализToolStripMenuItem";
            this.анализToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.анализToolStripMenuItem.Text = "Вычислить";
            // 
            // пускToolStripMenuItem
            // 
            this.пускToolStripMenuItem.Name = "пускToolStripMenuItem";
            this.пускToolStripMenuItem.Size = new System.Drawing.Size(273, 22);
            this.пускToolStripMenuItem.Text = "Множество направляющих символов";
            this.пускToolStripMenuItem.Click += new System.EventHandler(this.ViewDirectionSymbols);
            // 
            // lL1ТаблицаРазбораToolStripMenuItem
            // 
            this.lL1ТаблицаРазбораToolStripMenuItem.Name = "lL1ТаблицаРазбораToolStripMenuItem";
            this.lL1ТаблицаРазбораToolStripMenuItem.Size = new System.Drawing.Size(273, 22);
            this.lL1ТаблицаРазбораToolStripMenuItem.Text = "LL(1) таблицу разбора";
            this.lL1ТаблицаРазбораToolStripMenuItem.Click += new System.EventHandler(this.BuildAnalysisTable);
            // 
            // справкаToolStripMenuItem
            // 
            this.справкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
            this.справкаToolStripMenuItem.Size = new System.Drawing.Size(62, 20);
            this.справкаToolStripMenuItem.Text = "Справка";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.aboutToolStripMenuItem.Text = "О программе";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // openFileDialogGrammar
            // 
            this.openFileDialogGrammar.Filter = "Текстовые файлы|*.txt|Все файлы|*.*";
            this.openFileDialogGrammar.InitialDirectory = ".\\";
            this.openFileDialogGrammar.RestoreDirectory = true;
            // 
            // saveFileDialogGrammar
            // 
            this.saveFileDialogGrammar.FileName = "grammar.txt";
            this.saveFileDialogGrammar.Filter = "Текстовые файлы|*.txt|Все файлы|*.*";
            // 
            // toolTipMain
            // 
            this.toolTipMain.IsBalloon = true;
            this.toolTipMain.ShowAlways = true;
            // 
            // tbGrammar
            // 
            this.tbGrammar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbGrammar.Location = new System.Drawing.Point(0, 0);
            this.tbGrammar.Multiline = true;
            this.tbGrammar.Name = "tbGrammar";
            this.tbGrammar.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbGrammar.Size = new System.Drawing.Size(139, 403);
            this.tbGrammar.TabIndex = 10;
            this.tbGrammar.Text = "S>A B C\r\nA>D E\r\nB>F G\r\nC>#\r\nD>a\r\nD>#\r\nE>a a\r\nE>#\r\nF>H K\r\nG>b b\r\nH>c c\r\nK>d d";
            this.toolTipMain.SetToolTip(this.tbGrammar, "Продукции должны быть в формате A>BCd, по одной на строку. Как eps символ испоьзу" +
                    "йте #.");
            this.tbGrammar.WordWrap = false;
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 24);
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.tbGrammar);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tbTerminal);
            this.splitContainerMain.Panel2.Controls.Add(this.tbOutput);
            this.splitContainerMain.Size = new System.Drawing.Size(709, 403);
            this.splitContainerMain.SplitterDistance = 139;
            this.splitContainerMain.TabIndex = 3;
            // 
            // tbTerminal
            // 
            this.tbTerminal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbTerminal.Location = new System.Drawing.Point(15, 15);
            this.tbTerminal.Multiline = true;
            this.tbTerminal.Name = "tbTerminal";
            this.tbTerminal.Size = new System.Drawing.Size(539, 87);
            this.tbTerminal.TabIndex = 11;
            this.tbTerminal.Text = "b d a b e d c";
            // 
            // tbOutput
            // 
            this.tbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbOutput.Location = new System.Drawing.Point(15, 108);
            this.tbOutput.Multiline = true;
            this.tbOutput.Name = "tbOutput";
            this.tbOutput.ReadOnly = true;
            this.tbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbOutput.Size = new System.Drawing.Size(539, 295);
            this.tbOutput.TabIndex = 11;
            this.tbOutput.WordWrap = false;
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(709, 427);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.menuStripMain);
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "main";
            this.Text = "Анализатор LL(1)-свойств контекстно-свободных грамматик";
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel1.PerformLayout();
            this.splitContainerMain.Panel2.ResumeLayout(false);
            this.splitContainerMain.Panel2.PerformLayout();
            this.splitContainerMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Open;
        private System.Windows.Forms.OpenFileDialog openFileDialogGrammar;
        private System.Windows.Forms.SaveFileDialog saveFileDialogGrammar;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem анализToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem пускToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTipMain;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.TextBox tbGrammar;
        private System.Windows.Forms.TextBox tbOutput;
        private System.Windows.Forms.ToolStripMenuItem lL1ТаблицаРазбораToolStripMenuItem;
        private System.Windows.Forms.TextBox tbTerminal;
    }
}
