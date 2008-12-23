using System;
using System.Collections.Generic;
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
            ParsTable parseTable = new ParsTable(Grammar.LoadFromFile("Grammars\\test5.txt"));
            tbOutput.AppendText(parseTable.ToString());
        }

        //переводит массив символов {a,b,c} в красивую строку  'a', 'b', 'c'
        private string ConvertToCharList(char[] p)
        {
            if (p == null) return "";
            List<char> result = new List<char>();
            for (int charIndex = 0; charIndex < p.Length; charIndex++)
            {
                result.Add('\'');
                result.Add(p[charIndex]);
                result.Add('\'');
                if (charIndex != p.Length - 1)
                {
                    result.Add(',');
                    result.Add(' ');
                }
            }
            return new string(result.ToArray());
        }

        private void ViewDirectionSymbols(object sender, EventArgs e)
        {
            string[] productions = GetProductions();

            Grammar myGrammar = new Grammar(productions);

            tbOutput.Clear();

            // выводим множество направляющих символов для каждой продукции
            tbOutput.AppendText("\r\n");
            tbOutput.AppendText(myGrammar.GetDirectionSymbolsLog());
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
                        using (StreamReader sr = new StreamReader(s, Encoding.UTF8))
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
                        using (StreamWriter sw = new StreamWriter(s, Encoding.UTF8))
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

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Анализатор LL(1)-свойств контекстно-свободных грамматик\n" +
                            "Copyleft (l) 2008 Груздев М.", "О программе",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}