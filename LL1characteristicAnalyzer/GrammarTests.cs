using System;
using System.Collections.Generic;
using System.Text;

namespace LL1AnalyzerTool
{
    using NUnit.Framework;
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
    }
}
