using LL1Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using LL1AnalyzerTool;
namespace LL1ParserTests
{
    
    /// <summary>
    ///This is a test class for LL1AnalyzerTest and is intended
    ///to contain all LL1AnalyzerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class LL1AnalyzerTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        [TestMethod]
        public void RecordGrammarTests()
        {
            bool[] correctList = { true, true, false, false, false, false, false, false, true, false };
            int inputFilesListSize = 10;
            
            string grammarResourceName = String.Format("LL1ParserTests.Resources.Grammars.record.txt");
            Grammar grammar = Grammar.LoadFromStream(
                ResLoader.GetReader<LL1AnalyzerTest>(grammarResourceName)
                );
            Assert.IsTrue(grammar.LL1);

            for (int i = 0; i < inputFilesListSize; i++)
            {
                string inputResourceName = String.Format("LL1ParserTests.Resources.Inputs.record{0}.txt", i);
                string input = ResLoader.AsString<LL1AnalyzerTest>(inputResourceName);
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

        [TestMethod]
        public void ExpressionGrammarTests()
        {
            bool[] correctList = { true, true, false, true, true, true, false };//, false, true, false };
            int inputFilesListSize = 6;

            string grammarResourceName = String.Format("LL1ParserTests.Resources.Grammars.expression.txt");
            Grammar grammar = Grammar.LoadFromStream(
                ResLoader.GetReader<LL1AnalyzerTest>(grammarResourceName)
                );
            Assert.IsTrue(grammar.LL1);
            LL1Analyzer analyzer = new LL1Analyzer(new ParsTable(grammar));

            for (int i = 0; i < inputFilesListSize; i++)
            {
                string inputResourceName = String.Format("LL1ParserTests.Resources.Inputs.expr{0}.txt", i);
                string input = ResLoader.AsString<LL1AnalyzerTest>(inputResourceName);
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

        [TestMethod]
        public void OperatorGrammarTests()
        {
            bool[] correctList = { true, false, true, true, false, true, true };
            
            string grammarResourceName = String.Format("LL1ParserTests.Resources.Grammars.operator.txt");
            Grammar grammar = Grammar.LoadFromStream(
                ResLoader.GetReader<LL1AnalyzerTest>(grammarResourceName)
                );
            Assert.IsTrue(grammar.LL1);
            LL1Analyzer analyzer = new LL1Analyzer(new ParsTable(grammar));

            for (int i = 0; i < correctList.Length; i++)
            {
                string inputResourceName = String.Format("LL1ParserTests.Resources.Inputs.operator{0}.txt", i);
                string input = ResLoader.AsString<LL1AnalyzerTest>(inputResourceName);
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

        [TestMethod]
        public void ProgramGrammarTests()
        {
            bool[] correctList = { true, true, true };
            int inputFilesListSize = 3;

            string grammarResourceName = String.Format("LL1ParserTests.Resources.Grammars.program.txt");
            Grammar grammar = Grammar.LoadFromStream(
                ResLoader.GetReader<LL1AnalyzerTest>(grammarResourceName)
                );
            Assert.IsTrue(grammar.LL1);
            LL1Analyzer analyzer = new LL1Analyzer(new ParsTable(grammar));

            for (int i = 0; i < inputFilesListSize; i++)
            {
                string inputResourceName = String.Format("LL1ParserTests.Resources.Inputs.program{0}.txt", i);
                string input = ResLoader.AsString<LL1AnalyzerTest>(inputResourceName);
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

    }
}
