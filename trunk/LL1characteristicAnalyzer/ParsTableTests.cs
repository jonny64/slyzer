using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace LL1AnalyzerTool
{
    [TestFixture]
    public class ParsTableTests
    {
        [Test]
        public void Test()
        {
            Grammar grammar = Grammar.LoadFromFile("Grammars\\test1.txt");
            ParsTable table = new ParsTable(grammar);
            System.Windows.Forms.MessageBox.Show(table.ToString(), "ss");
        }
    }
}
