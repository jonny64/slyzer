using System;
using System.Collections.Generic;
using System.Text;
using csUnit;

namespace LL1AnalyzerTool
{
    [TestFixture]
    public class GrammarTests
    {
        [Test]
        public void TestEmptySet()
        {
            string[] productions = { "S A B C",
                "A D E",
                "B F G",
                "C #",
                "D a",
                "D #",
                "E a a",
                "E #",
                "F H K",
                "G b b",
                "H c c",
                "K d d"
            };
            Grammar grammar = new Grammar(productions);

            Assert.Equals("", grammar.ToString());
        }
    }
}
