using System;
using System.Collections.Generic;

namespace LL1AnalyzerTool
{
    internal class Grammar
    {
        // FIRST and FOLLOW relations matrices

        #region EmptyState enum

        public enum EmptyState
        {
            EMPTY,
            NON_EMPTY,
            UNKNOWN
        } ;

        #endregion

        private readonly Dictionary<Symbol, EmptyState> m_empty = new Dictionary<Symbol, EmptyState>();
        private bool[,] m_first;
        private bool[,] m_follow;

        private List<Production> m_grammar = new List<Production>();

        public Grammar(string[] productions)
        {
            foreach (string prodString in productions)
            {
                Production prod = new Production(prodString);
                grammar.Add(prod);
            }
            CreateEmptySymTable();
            CreateFirstRelationTable();
            return;
            CreateFollowRealationTable();
        }

        public List<Production> grammar
        {
            get { return m_grammar; }
        }

        public Set GetDirectionSymbols(string[] productions, string[] terminalWords, List<Symbol> sequence)
        {
            throw new NotImplementedException();
        }

        public Set GetDirectionSymbols(List<Symbol> sequence, string[] productions, string[] terminalWords)
        {
            throw new NotImplementedException();
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
            return new Set();
        }

        private bool Empty(Symbol[] sequence)
        {
            bool empty = true;
            foreach (Symbol sym in sequence)
            {
                empty &= Empty(sym);
            }
            return empty;
        }

        private bool Empty(Symbol sym)
        {
            if (m_empty.ContainsKey(sym))
                if (m_empty[sym] == EmptyState.EMPTY)
                    return true;
            return false;
        }

        private bool Empty(LinkedList<Symbol> sequence)
        {
            Symbol[] seq = new Symbol[sequence.Count];
            sequence.CopyTo(seq, 0);
            return Empty(seq);
        }

        // show direction syms summary for each production
        internal string GetDirectionSymbolsLog()
        {
            string log = "";
            foreach (Production production in grammar)
            {
                Set dirSyms = GetDirectionSymbols(production.ToLinkedList());
                log += "DS[" + production.Head + ">" +
                       production.Tail +
                       "] = " + dirSyms + "\r\n";
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
                        m_empty[prod.Head] = EmptyState.NON_EMPTY;
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
                    grammar = PerformAdditionalResearch(grammar);
            } while (!FoundEachNonTerminalState());
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
                if (prod.Head.Equals(symbol))
                    return true;
            }
            ;
            return false;
        }

        private LinkedList<Production> PerformAdditionalResearch(LinkedList<Production> aGrammar)
        {
            LinkedList<Production> updatedGrammar = new LinkedList<Production>(aGrammar);

            foreach (Production original in aGrammar)
            {
                Production prod = original.Clone();
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
                                foreach (Production p in aGrammar)
                                {
                                    if (p.Head.Equals(sym))
                                        updatedGrammar.Remove(p);
                                }
                            }
                        }
                    }
                }
            }
            return updatedGrammar;
        }

        private bool FoundEachNonTerminalState()
        {
            foreach (Symbol sym in GetGrammarSymbols())
            {
                if (!sym.Terminal && !m_empty.ContainsKey(sym))
                    return false;
            }
            
            return true;
        }

        private void CreateFirstRelationTable()
        {
            int grammarSymsCount = GetGrammarSymbols().Count;
            m_first = new bool[grammarSymsCount,grammarSymsCount];

            //вычисление отношения начинается_прямо_с
            foreach (Production prod in grammar)
            {
                for (int i = 0; i < prod.Tail.Count; i++)
                {
                    LinkedList<Symbol> alpha = prod.SubTail(i);
                    if (Empty(alpha) && !prod.Epsilon)
                    {
                        SetFirst(prod.Head, prod.TailAt(i), true);
                    }
                }
            }
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

        private void SetFirst(Symbol prodHead, Symbol symbol, bool p)
        {
            Set grammarSyms = GetGrammarSymbols();
            List<Symbol> syms = new List<Symbol>();
            foreach (object obj in grammarSyms)
            {
                Symbol sym = (Symbol) obj;
                syms.Add(sym);
            }
            int prodHeadIndex = Array.BinarySearch(syms.ToArray(), prodHead);
            int symIndex = Array.BinarySearch(syms.ToArray(), symbol);

            m_first[prodHeadIndex, symIndex] = p;
        }

        private int GetSymIndex(Symbol sym)
        {
            throw new Exception("The method or operation is not implemented.");
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

        public Dictionary<Symbol, EmptyState> GetEmptyHashtable()
        {
            return m_empty;
        }
    }
}