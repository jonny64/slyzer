using System;
using System.Collections.Generic;
using System.Text;

namespace LL1AnalyzerTool
{
    using NUnit.Framework;
    [TestFixture]
    public class GrammarTests
    {
        private Grammar grammar;

        [SetUpAttribute]
        public void Init()
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
            grammar = new Grammar(productions);
        }

        [Test]
        public void EmptySet()
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
            Symbol[] syms = {new Symbol("S"),
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
            Symbol[] syms = {new Symbol("S"),
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
                Assert.AreEqual(0, difference.Count);
            }
        }
    }
}
