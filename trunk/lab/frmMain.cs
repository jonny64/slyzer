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
            //создаем лексический анализатор, на вход подаем текст из Memo rtbInput
            Lexan myParser = new Lexan(this.rtbInput.Text);
            PrepareOutput();
            Token token;
            while ((token = myParser.GetToken()).type != AnalysisStage.TokenTypes.TERMINATOR)
            {
                OutText("(" + token.type.ToString() + ", " + token.attribute + " )\n");
            }
            string[] errors = myParser.errorMessages.ToArray();
            for (int errorIndex = 0; errorIndex < errors.Length; errorIndex++)
                OutText(errors[errorIndex] + '\n');
            
            //ShowNumConstTable(myParser);
            //ShowIdentTable(myParser);
            //ShowKeyWords();

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
                            sw.Write(rtbInput.Text);
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
                            rtbInput.Text = sr.ReadToEnd();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void ShowKeyWords()
        {
            OutText("Таблица ключевых слов\n");
            string[] keyWords = lab.AnalysisStage.GetKeyWords();
            for (int kwIndex = 0; kwIndex < keyWords.Length; kwIndex++)
            {
                OutText(kwIndex.ToString() + '\t' + keyWords[kwIndex] + '\n');
            }
        }

        private void PrepareOutput()
        {
            rtbOutput.Clear();
        }

        private void ShowIdentTable(Lexan Parser)
        {
            OutText("Таблица идентификаторов\n");
            
            Ident[] identTable=lab.AnalysisStage.GetIdentTable();
            for (int identIndex = 0; identIndex < lab.AnalysisStage.GetIdentTableSize(); identIndex++)
            {
                OutText(identIndex.ToString()+'\t' + identTable[identIndex].name + '\n');
            }
        }

        private void ShowNumConstTable(Lexan Parser)
        {
            OutText("Таблица числовых констант\n");

            NumConst[] numConstTable = lab.AnalysisStage.GetNumConstTable();
            for (int numConstIndex = 0; numConstIndex < lab.AnalysisStage.GetNumConstTableSize(); numConstIndex++)
            {
                OutText(numConstIndex.ToString() + '\t' + 
                                    numConstTable[numConstIndex].name + '\n');
            }
        }

        private void OutText(string p)
        {
            rtbOutput.AppendText(p);
        }

        private void синтаксическийАнализToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartLL1SyntaxCheck();
        }

        private void StartLL1SyntaxCheck()
        {
            listBoxMsg.Items.Clear();
            listBoxMsg.Items.Add("Поехали...");
            LL1Analyzer myAnalyzer = new LL1Analyzer();
            if (myAnalyzer.Check(rtbInput.Text))
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

        private void восходящийРазборToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartLRSyntaxCheck();
        }

        private void StartLRSyntaxCheck()
        {
            listBoxMsg.Items.Clear();
            listBoxMsg.Items.Add("Поехали...");
            LRAnalyzer myAnalyzer = new LRAnalyzer(rtbInput.Text);
            if (myAnalyzer.Check())
            {
                //MessageBox.Show("Все правильно");
                listBoxMsg.Items.Add("Синтаксис в порядке");
            }
            else
            {
                //listBoxMsg.MessageBox.Show("Ошибка: "+myAnalyzer.errMsg);
                listBoxMsg.Items.Add("Ошибка: " + myAnalyzer.m_errMsg);
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }


    }
}