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
            Assert.IsTrue(grammar.LL1);

            ParsTable table = new ParsTable(grammar);
            string csharptable = table.ToCsharpSyntaxAnalyzerTable(); 
        }
    }
}
