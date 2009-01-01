using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using LL1AnalyzerTool;

namespace lab
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        public void Exit(object sender, EventArgs e)
        {
            this.Close();
        }

        public void StartLexicalAnalysis(object sender, EventArgs e)
        {
            Lexan myParser = new Lexan(tbInput.Text);
            rtbOutput.Clear();
            Token token;
            while ((token = myParser.GetToken()).type != AnalysisStage.TokenType.TERMINATOR)
            {
                OutText("(" + token.type.ToString() + ", " + token.attribute + " )\n");
            }
            string[] errors = myParser.errorMessages.ToArray();
            for (int errorIndex = 0; errorIndex < errors.Length; errorIndex++)
                OutText(errors[errorIndex] + '\n');
        }

        private void SaveFile()
        {
            if (saveFileDialogInput.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Stream s;
                    if ((s = saveFileDialogInput.OpenFile()) != null)
                    {
                        using (StreamWriter sw = new StreamWriter(s, Encoding.UTF8))
                        {
                            sw.Write(tbInput.Text);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не могу записать файл: " + ex.Message);
                }
            }
        }

        private void OpenFile()
        {
            if (openFileDialogInput.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Stream s;
                    if ((s = openFileDialogInput.OpenFile()) != null)
                    {
                        using (StreamReader sr = new StreamReader(s, Encoding.UTF8))
                        {
                            // Insert code to read the stream here.
                            tbInput.Text = sr.ReadToEnd();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void OutText(string p)
        {
            rtbOutput.AppendText(p);
        }

        private void synToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartLL1SyntaxCheck();
        }

        private void StartLL1SyntaxCheck()
        {
            listBoxMsg.Items.Clear();
            listBoxMsg.Items.Add("Поехали...");
            LL1Analyzer myAnalyzer = new LL1Analyzer();
            if (myAnalyzer.Check(tbInput.Text))
            {
                //MessageBox.Show("Все правильно");
                listBoxMsg.Items.Add("Синтаксис в порядке");
            }
            else
            {
                //listBoxMsg.MessageBox.Show("Ошибка: "+myAnalyzer.errMsg);
                listBoxMsg.Items.Add("Ошибка: " + myAnalyzer.ErrorMessage);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }


    }
}