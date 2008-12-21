using System;
using System.Collections.Generic;
using System.Text;

namespace LL1AnalyzerTool
{
    class Grammar
    {
        // FIRST and FOLLOW relations matrices
        bool[,] m_first;
        bool[,] m_follow;

        public Grammar(string[] productions, string[] terminalWords)
        {
            foreach (string prodString in productions)
            {
                Production prod = new Production(prodString);
                grammar.Add(prod);
            }
            CreateEmptySymTable();
            CreateFirstRelationTable();
            CreateFollowRealationTable();
        }

        List<Production> m_grammar = new List<Production>();
        public List<Production> grammar
        {
            get
            {
                return m_grammar;
            }
            set
            {
                m_grammar = value;
            }
        }

        public Set GetDirectionSymbols(string[] productions, string[] terminalWords, List<Symbol> sequence)
        {
            throw new System.NotImplementedException();
        }

        public Set GetDirectionSymbols(List<LL1AnalyzerTool.Symbol> sequence, string[] productions, string[] terminalWords)
        {
            throw new System.NotImplementedException();
        }

        public Set GetDirectionSymbols(LinkedList<Symbol> sequence)
        {
            Symbol head = sequence.First.Value;
            sequence.RemoveFirst();
            LinkedList<Symbol> rightPart = sequence;
            Set ds = new Set();

            if (Empty(rightPart))
                ds = First(rightPart) + Follow(head);
            else
                ds = First(rightPart);
            return ds;
        }

        private Set Follow(Symbol head)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        private Set First(LinkedList<Symbol> sequence)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        private bool Empty(LinkedList<Symbol> sequence)
        {
            return false;
        }

        // show direction syms summary for each production
        internal string GetDirectionSymbolsLog()
        {
            string log = "";
            foreach (Production production in grammar)
            {
                Set dirSyms = GetDirectionSymbols(production.ToLinkedList());
                log += "DS[" + production.Head + ">" + 
                    production.Tail.ToString() + 
                    "] = " + dirSyms.ToString() + "\r\n";
            }
            return log;
        }

        private void CreateEmptySymTable()
        {
            m_empty = new Dictionary<Symbol, bool>();

            LinkedList<Production> grammar = new LinkedList<Production>(this.grammar);
            LinkedList<Production> grammarWihoutEpsProd = new LinkedList<Production>(this.grammar);
            
            // delete epsilon productions
            foreach (Production prod in grammar)
            {
                if (prod.Epsilon)
                {
                    grammarWihoutEpsProd.Remove(prod);
                }
            }

            //do
            //{

            //    excluded = PerformBasicResearch(excluded);
            //    if (!FoundEachNonTerminalState()) PerformAdditionalResearch(excluded, m_grammar);
            //}
            //while (!FoundEachNonTerminalState());
        }

        private void CreateFirstRelationTable()
        {
            int grammarSymsCount = GetGrammarSymbols().Count;
            m_first = new bool[grammarSymsCount, grammarSymsCount];

            ////вычисление отношения начинается_прямо_с
            //for (int prodIndex = 0; prodIndex < m_grammar.Length; prodIndex++)
            //{
            //    string production = m_grammar[prodIndex];
            //    char head = production[0];
            //    for (int symIndex = 1; symIndex < production.Length; symIndex++)
            //    {
            //        char sym = production[symIndex];
            //        string alpha = production.Substring(1, symIndex - 1);
            //        if (Empty(alpha) && (sym != EPSILON_CHAR))
            //        {
            //            m_first[GetSymIndex(head), GetSymIndex(sym)] = true;
            //        }
            //    }
            //}
            ////вычисление рефлексивноого замыкания отношения начинается_прямо_с
            //char[] grammarSyms = GetGrammarSymbols();
            //for (int symIndex = 0; symIndex < grammarSyms.Length; symIndex++)
            //{
            //    char sym = grammarSyms[symIndex];
            //    m_first[GetSymIndex(sym), GetSymIndex(sym)] = true;
            //}
            ////вычисление транзитивного замыкания отношения начинается_прямо_с
            //m_first = Transitive(m_first);
        }

        // retrieve all grammar symbols set
        private Set GetGrammarSymbols()
        {
            Set syms = new Set();
            foreach (Production prod in grammar)
            {
                syms = syms + prod.ToSet();
            }
            return syms;
        }

        private void CreateFollowRealationTable()
        {
            ;
        }

        private Dictionary<Symbol, bool> m_empty;
    }
}
