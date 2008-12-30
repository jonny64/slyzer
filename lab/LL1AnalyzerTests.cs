using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using LL1AnalyzerTool;

namespace lab
{
    [TestFixture]
    public class LL1AnalyzerTests
    {
        [Test]
        public void RecordGrammarTests()
        {
            bool[] correctList = { true, true, false, false, false, false, false, false, true, false};
            int inputFilesListSize = 10;

            Grammar grammar = Grammar.LoadFromFile("Grammars\\record.txt");
            Assert.IsTrue(grammar.LL1);

            for (int i = 0; i < inputFilesListSize; i++)
            {
                String input = LoadFromFile("Inputs\\" + "record" + i + ".txt");
                input = input.Replace("\r", "");
                LL1Analyzer analyzer = new LL1Analyzer(new ParsTable(grammar));
                Assert.AreEqual(
                    correctList[i],
                    analyzer.Check(input),
                    String.Format("Номер теста {0}; Сообщение: {1}; Тест:\n {2}",
                    i,
                    analyzer.ErrorMessage,
                    input
                    )
                );

            }
        }

        [Test]
        public void ExpressionGrammarTests()
        {
            bool[] correctList = { true, true, false, true, true, true, false};//, false, true, false };
            int inputFilesListSize = 6;

            Grammar grammar = Grammar.LoadFromFile("Grammars\\expression.txt");
            Assert.IsTrue(grammar.LL1);
            LL1Analyzer analyzer = new LL1Analyzer(new ParsTable(grammar));

            for (int i = 0; i < inputFilesListSize; i++)
            {
                String input = LoadFromFile("Inputs\\" + "expr" + i + ".txt");
                input = input.Replace("\r", "");
                Assert.AreEqual(
                    correctList[i],
                    analyzer.Check(input),
                    String.Format("Номер теста {0}; Сообщение: {1}; Тест:\n {2}",
                    i,
                    analyzer.ErrorMessage,
                    input
                    )
                );
            }
        }

        private string LoadFromFile(string filename)
        {
            return (new System.IO.StreamReader(filename).ReadToEnd());
        }
    }
}
