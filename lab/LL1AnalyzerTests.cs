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
        public void InputCheck()
        {
            bool[] correctList = { true };
            string[] inputFilesList = { "record1.txt" };

            Grammar grammar = Grammar.LoadFromFile("Grammars\\record.txt");
            Assert.IsTrue(grammar.LL1);

            for (int inFile = 0; inFile < inputFilesList.Length; inFile++)
            {
                String input = LoadFromFile("Inputs\\" + inputFilesList[inFile]);
                LL1Analyzer analyzer = new LL1Analyzer(input);
                Assert.IsTrue(analyzer.InputCorrect(), analyzer.ErrorMessage) ;
            }
        }

        private string LoadFromFile(string filename)
        {
            return (new System.IO.StreamReader(filename).ReadToEnd());
        }
    }
}
