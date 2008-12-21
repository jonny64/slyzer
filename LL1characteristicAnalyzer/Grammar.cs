using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

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
            LinkedList<Production> grammar = new LinkedList<Production>(this.grammar);

            LinkedList<Production> updatedGrammar = new LinkedList<Production>(grammar);
            // delete productions with terminals
            foreach (Production prod in grammar)
            {
                if (prod.ContainsTerminals())
                {
                    updatedGrammar.Remove(prod);
                    if (!ProductionForSym(prod.Head, updatedGrammar))
                        m_empty[prod.Head] = EmptyState.EMPTY;
                }
            }
            grammar = updatedGrammar;

            LinkedList<Production> grammarWihoutEpsProd = new LinkedList<Production>(grammar);
            // delete epsilon productions
            foreach (Production prod in grammar)
            {
                if (prod.Epsilon)
                {
                    grammarWihoutEpsProd.Remove(prod);
                    m_empty[prod.Head] = EmptyState.EMPTY;
                }
            }
            grammar = grammarWihoutEpsProd;

            do
            {
                PerformBasicResearch(ref grammar);
                if (!FoundEachNonTerminalState()) 
                    PerformAdditionalResearch(ref grammar);
            }
            while (!FoundEachNonTerminalState());
        }

        // exclude every non-eps gen right part producton from grammar
        private void PerformBasicResearch(ref LinkedList<Production> grammar)
        {
            LinkedList<Production> updatedGrammar = new LinkedList<Production>(grammar);

            foreach (Production prod in grammar)
            {
                foreach (Symbol sym in prod.Tail)
                {
                    bool epsGen;
                    if (m_empty.ContainsKey(sym))
                    {
                        // if sym is non eps gen and no alternatives for him
                        // mark prod head sym as non eps genarating
                        if (EmptyState.NON_EMPTY == m_empty[sym])
                        {
                            updatedGrammar.Remove(prod);
                            if (!ProductionForSym(prod.Head, updatedGrammar))
                                m_empty[prod.Head] = EmptyState.NON_EMPTY;
                        }
                    }
                }
            }
            grammar = updatedGrammar;
        }

        private bool ProductionForSym(Symbol symbol, LinkedList<Production> updatedGrammar)
        {
            foreach (Production prod in updatedGrammar)
            {
                if (prod.Head.Equals(symbol) )
                    return true;
            };
            return false;
        }

        private void PerformAdditionalResearch(ref LinkedList<Production> grammar)
        {
            LinkedList<Production> updatedGrammar = new LinkedList<Production>(grammar);

            foreach (Production prod in grammar)
            {
                foreach (Symbol sym in prod.Tail)
                {
                    bool epsGen;
                    if ((!sym.Terminal) && m_empty.ContainsKey(sym))
                    {
                        // if sym is eps gen and no alternatives for him
                        // mark prod head sym as non eps genarating
                        if (EmptyState.EMPTY == m_empty[sym])
                        {
                            prod.RemoveFromTail(sym);
                            if (prod.Tail.Empty)
                            {    
                                m_empty[prod.Head] = EmptyState.EMPTY;
                                foreach (Production p in grammar)
                                {
                                    if (p.Head.Equals(sym))
                                        updatedGrammar.Remove(p);
                                }
                            }
                        }
                    }
                }
            }
            grammar = updatedGrammar;
        }

        private bool FoundEachNonTerminalState()
        {
            foreach (Symbol sym in GetGrammarSymbols())
            {
                if (!sym.Terminal && !m_empty.ContainsKey(sym))
                    return false;
            };
            return true;
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
        private enum EmptyState
        { EMPTY, NON_EMPTY, UNKNOWN };
        private Dictionary<Symbol, EmptyState> m_empty = new Dictionary<Symbol,EmptyState>();
        
    }
}
