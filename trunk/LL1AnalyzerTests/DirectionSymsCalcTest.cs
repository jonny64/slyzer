using System;
using System.Collections.Generic;
using System.IO;
using LL1AnalyzerTool;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LL1AnalyzerTests
{
    /// <summary>
    ///This is a test class for SetTest and is intended
    ///to contain all SetTest Unit Tests
    ///</summary>
    [TestClass]
    public class DirectionSymsCalcTest
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

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

        #region Setup/Teardown

        private static Grammar grammar;

        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            string[] productions = {
                                       "S A B C",
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
            grammar = new Grammar(productions);
        }

        #endregion

        private Set[] LoadDirectionSymsFromFile(string filename)
        {
            var sr = new StreamReader(filename);
            return LoadDirectionSymsFromStream(sr);
        }

        private static Set[] LoadDirectionSymsFromStream(StreamReader sr)
        {
            var sets = new List<Set>();
            while (sr.Peek() != -1)
            {
                string line = sr.ReadLine();
                if (line[0] == ';')
                {
                    continue;
                }
                if (line == "\n")
                {
                    break;
                }

                string[] syms = line.Split(' ');
                var set = new Set();
                foreach (string sym in syms)
                {
                    set.Add(new Symbol(sym));
                }
                sets.Add(set);
            }
            return sets.ToArray();
        }

        [TestMethod]
        public void DirSymbols()
        {
            const int DIR_SYMS_TEST_COUNT = 5;
            for (int grFile = 0; grFile < DIR_SYMS_TEST_COUNT; grFile++)
            {
                string grammarResourceName = String.Format("LL1AnalyzerTests.Resources.Grammars.test{0}.txt", grFile + 1);
                string dirSymsResourceName = String.Format("LL1AnalyzerTests.Resources.DirectionSets.set{0}.txt",
                                                           grFile + 1);
                Grammar simpleGrammar = Grammar.LoadFromStream(
                    ResLoader.GetReader<DirectionSymsCalcTest>(grammarResourceName)
                    );
                Set[] dirSyms = LoadDirectionSymsFromStream(
                    ResLoader.GetReader<DirectionSymsCalcTest>(dirSymsResourceName)
                    );

                for (int i = 0; i < simpleGrammar.Length; i++)
                {
                    Set actual =
                        simpleGrammar.GetDirectionSymbols(simpleGrammar[i]);
                    Set difference = dirSyms[i]/actual;
                    Assert.AreEqual(0, difference.Count,
                                    String.Format("Grammar {3}; Production {0}; actual set {1}; expected {2};",
                                                  simpleGrammar.GetProductionAt(i), actual, dirSymsResourceName,
                                                  grammarResourceName)
                        );
                }
            }
        }


        [TestMethod]
        public void EmptySet()
        {
            string dirSymsResourceName = "LL1AnalyzerTests.Resources.Grammars.test1.txt";
            Grammar grammar = Grammar.LoadFromStream(
                ResLoader.GetReader<DirectionSymsCalcTest>(dirSymsResourceName)
                );
            Symbol[] syms = {
                                new Symbol("S"),
                                new Symbol("A"),
                                new Symbol("B"),
                                new Symbol("C"),
                                new Symbol("D"),
                                new Symbol("E"),
                                new Symbol("F"),
                                new Symbol("G"),
                                new Symbol("H"),
                                new Symbol("K")
                            };
            Grammar.EmptyState[] empty = {
                                             Grammar.EmptyState.NON_EMPTY,
                                             Grammar.EmptyState.EMPTY,
                                             Grammar.EmptyState.NON_EMPTY,
                                             Grammar.EmptyState.EMPTY,
                                             Grammar.EmptyState.EMPTY,
                                             Grammar.EmptyState.EMPTY,
                                             Grammar.EmptyState.NON_EMPTY,
                                             Grammar.EmptyState.NON_EMPTY,
                                             Grammar.EmptyState.NON_EMPTY,
                                             Grammar.EmptyState.NON_EMPTY,
                                         };
            for (int i = 0; i < syms.Length; i++)
            {
                Grammar.EmptyState actual = grammar.GetEmptyHashtable()[syms[i]];
                Assert.AreEqual(actual, empty[i]);
            }
        }

        [TestMethod]
        public void FirstSet()
        {
            Symbol[] syms = {
                                new Symbol("S"),
                                new Symbol("A"),
                                new Symbol("B"),
                                new Symbol("C"),
                                new Symbol("D"),
                                new Symbol("E"),
                                new Symbol("F"),
                                new Symbol("G"),
                                new Symbol("H"),
                                new Symbol("K")
                            };
            Set[] firsts = {
                               new Set(new Symbol("a"), new Symbol("c")),
                               new Set(new Symbol("a")),
                               new Set(new Symbol("c")),
                               new Set(),
                               new Set(new Symbol("a")),
                               new Set(new Symbol("a")),
                               new Set(new Symbol("c")),
                               new Set(new Symbol("b")),
                               new Set(new Symbol("c")),
                               new Set(new Symbol("d"))
                           };
            for (int i = 0; i < syms.Length; i++)
            {
                Set actual = grammar.First(syms[i]);
                Set difference = firsts[i]/actual;
                Assert.AreEqual(
                    0,
                    difference.Count,
                    String.Format("Symbol: {0}, actual set: {1}, expected: {2}", syms[i], actual, firsts[i])
                    );
            }
        }

        [TestMethod]
        public void FollowSet()
        {
            Symbol[] syms = {
                                new Symbol("A"),
                                new Symbol("C"),
                                new Symbol("D"),
                                new Symbol("E"),
                                new Symbol("F"),
                                new Symbol("G"),
                                new Symbol("H"),
                                new Symbol("K")
                            };
            Set[] follows = {
                                new Set(new Symbol("c")),
                                new Set(Symbol.TERMINATOR),
                                new Set(new Symbol("a"), new Symbol("c")),
                                new Set(new Symbol("c")),
                                new Set(new Symbol("b")),
                                new Set(Symbol.TERMINATOR),
                                new Set(new Symbol("d")),
                                new Set(new Symbol("b"))
                            };
            for (int i = 0; i < syms.Length; i++)
            {
                Set actual = grammar.Follow(syms[i]);
                Set difference = follows[i]/actual;
                Assert.AreEqual(
                    0,
                    difference.Count,
                    String.Format("Symbol: {0}, actual set: {1}, expected: {2}", syms[i], actual, follows[i])
                    );
            }
        }
    }
}