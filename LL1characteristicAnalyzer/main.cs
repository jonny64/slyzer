using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            GrammarTableBuilder builder = new GrammarTableBuilder(productions);
            tbOutput.AppendText("rowIndex\tterminals\t\t\t\tjump\taccept\tstack\terror\n");
            tbOutput.AppendText("{\n");

            // TODO переписать здесь код сделать понятным
            TableRow[] parsTable = builder.GetParsingTable();
            for (int rowIndex = 0; rowIndex < parsTable.Length; rowIndex++)
            {
                string terminals = ConvertToCharList(parsTable[rowIndex].terminals);
                
                tbOutput.AppendText(""+
                                    //rowIndex    +"\t"+
                                    "new TableRow( new char[] { " + terminals.PadLeft(20,' ') + "},\t" +
                                    parsTable[rowIndex].jump.ToString().ToLower()+",\t"+
                                    parsTable[rowIndex].accept.ToString().ToLower() + ",\t" +
                                    parsTable[rowIndex].stack.ToString().ToLower() + ",\t" +
                                    parsTable[rowIndex].error.ToString().ToLower() + ", \"\" ),\n");
            }
            tbOutput.AppendText("};\n");
        }
        //переводит массив символов {a,b,c} в красивую строку  'a', 'b', 'c'
        private string ConvertToCharList(char[] p)
        {
            if (p==null) return "";
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
            string[] terminals = GetTerminals();

            Grammar myGrammar = new Grammar(productions, terminals);

            tbOutput.Clear();
 
            // выводим множество направляющих символов для каждой продукции
            tbOutput.AppendText("\r\n");
            tbOutput.AppendText(myGrammar.GetDirectionSymbolsLog());
        }

        private string[] GetTerminals()
        {
            char [] seps = {' '};
            string[] terms = tbTerminal.Text.Split(seps, 
                StringSplitOptions.RemoveEmptyEntries);
            return terms;
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
            char[] seps ={ '\r', '\n' };
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
                        using (StreamReader sr=new StreamReader(s,Encoding.UTF8))
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
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Анализатор LL(1)-свойств контекстно-свободных грамматик\n"+
            "Copyleft (l) 2008 Груздев М.", "О программе", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}