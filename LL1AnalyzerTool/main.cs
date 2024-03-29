﻿using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace LL1AnalyzerTool
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent();
        }

        private void BuildAnalysisTable(object sender, EventArgs e)
        {
            string[] productions = GetProductions();
            var parseTable = new ParsTable(new Grammar(productions));
            tbOutput.AppendText(parseTable.ToString());
        }

        private void ViewDirectionSymbols(object sender, EventArgs e)
        {
            string[] productions = GetProductions();
            var myGrammar = new Grammar(productions);

            tbOutput.Clear();

            // выводим множество направляющих символов для каждой продукции
            tbOutput.AppendText("\r\n");
            tbOutput.AppendText(myGrammar.GetDirectionSymbolsLog());
            tbOutput.AppendText("LL1: " + myGrammar.LL1);
        }

        private string[] GetProductions()
        {
            string grammar = tbGrammar.Text;
            string[] productions = PreParse(grammar);
            return productions;
        }

        private string[] PreParse(string grammar)
        {
            //удаляем вхождения >
            grammar = grammar.Replace(">", " ");
            //разобьем выражение на строки
            char[] seps = {'\r', '\n'};
            string[] productions = grammar.Split(seps,
                                                 StringSplitOptions.RemoveEmptyEntries);
            return productions;
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenGrammarFile();
        }

        private void OpenGrammarFile()
        {
            if (openFileDialogGrammar.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Stream s;
                    if ((s = openFileDialogGrammar.OpenFile()) != null)
                    {
                        using (var sr = new StreamReader(s, Encoding.UTF8))
                        {
                            // Insert code to read the stream here.
                            tbGrammar.Text = sr.ReadToEnd();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveGrammarFile();
        }

        private void SaveGrammarFile()
        {
            if (saveFileDialogGrammar.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Stream s;
                    if ((s = saveFileDialogGrammar.OpenFile()) != null)
                    {
                        using (var sw = new StreamWriter(s, Encoding.UTF8))
                        {
                            string grammar = tbGrammar.Text;
                            sw.Write(grammar);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Не могу записать файл: " + ex.Message);
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Анализатор LL(1)-свойств контекстно-свободных грамматик\n" +
                            "Copyleft (l) 2008", "О программе",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LL1tableForCsharp_Click(object sender, EventArgs e)
        {
            string[] productions = GetProductions();
            var parseTable = new ParsTable(new Grammar(productions));
            tbOutput.AppendText("\r\n\r\n");
            tbOutput.AppendText(parseTable.ToCsharpSyntaxAnalyzerTable());
        }

        private void clearOutputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tbOutput.Clear();
        }
    }
}