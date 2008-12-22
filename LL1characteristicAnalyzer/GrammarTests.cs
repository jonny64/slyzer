using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace LL1AnalyzerTool
{
    [TestFixture]
    public class GrammarTests
    {
        #region Setup/Teardown

        [SetUp]
        public void Init()
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

        private Grammar grammar;

        private Set[] LoadDirectionSymsFromFile(string filename)
        {
            StreamReader sr = new StreamReader(filename);
            List<Set> sets = new List<Set>();
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
                Set set = new Set();
                foreach (string sym in syms)
                {
                    set.Add(new Symbol(sym));
                }
                sets.Add(set);
            }
            return sets.ToArray();
        }

        [Test]
        public void DirSymbols()
        {
            string[] grFilesList = { "test1.txt", "test2.txt", "test3.txt", "test4.txt", "test5.txt" };
            string[] dirSymsFilesList = { "set1.txt", "set2.txt", "set3.txt", "set4.txt", "set5.txt" };

            for (int grFile = 0; grFile < grFilesList.Length; grFile++)
            {
                Grammar simpleGrammar = Grammar.LoadFromFile("Grammars\\" + grFilesList[grFile]);
                Set[] dirSyms = LoadDirectionSymsFromFile("DirectionSets\\" + dirSymsFilesList[grFile]);

                for (int i = 0; i < simpleGrammar.Length; i++)
                {
                    Set actual =
                        simpleGrammar.GetDirectionSymbols(simpleGrammar[i]);
                    Set difference = dirSyms[i]/actual;
                    Assert.AreEqual(0, difference.Count,
                                    String.Format("Grammar {3}; Production {0}; actual set {1}; expected {2};",
                                                  simpleGrammar.GetProductionAt(i), actual, dirSyms[i],
                                                  grFilesList[grFile])
                        );
                }
            }
        }

        [Test]
        public void EmptySet()
        {
            string testfile = "Grammars\\test1.txt";
            Grammar grammar = Grammar.LoadFromFile(testfile);
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

        [Test]
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
                Assert.AreEqual(0, difference.Count,
                                String.Format("Symbol: {0}, actual set: {1}, expected: {2}",
                                              syms[i], actual, firsts[i])
                    );
            }
        }

        [Test]
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
                                new Set(Symbol.NewTerminator()),
                                new Set(new Symbol("a"), new Symbol("c")),
                                new Set(new Symbol("c")),
                                new Set(new Symbol("b")),
                                new Set(Symbol.NewTerminator()),
                                new Set(new Symbol("d")),
                                new Set(new Symbol("b"))
                            };
            for (int i = 0; i < syms.Length; i++)
            {
                Set actual = grammar.Follow(syms[i]);
                Set difference = follows[i]/actual;
                Assert.AreEqual(0, difference.Count,
                                String.Format("Symbol: {0}, actual set: {1}, expected: {2}",
                                              syms[i], actual, follows[i])
                    );
            }
        }
    }
}