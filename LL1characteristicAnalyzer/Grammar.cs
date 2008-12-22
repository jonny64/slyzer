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
        private readonly List<Production> m_grammar = new List<Production>();
        private bool[,] m_first;
        private bool[,] m_follow;

        public Grammar(string[] productions)
        {
            foreach (string prodString in productions)
            {
                Production prod = new Production(prodString);
                grammar.Add(prod);
            }
            CreateEmptySymTable();
            CreateFirstRelationTable();
            CreateFollowRelationTable();
        }

        public List<Production> grammar
        {
            get { return m_grammar; }
        }

        public decimal Length
        {
            get { return m_grammar.Count; }
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

        public Set Follow(Symbol sym)
        {
            if (sym.Terminal)
                throw new Exception("Follow can be calculated only for non terminal");

            Set follow = new Set();
            Set terminals = GetTerminals();
            foreach (Symbol terminal in terminals)
            {
                if (GetFollow(sym, terminal))
                    follow.Add(terminal);
            }
            return follow;
        }

        private bool GetFollow(Symbol nonTerminal, Symbol terminal)
        {
            return m_follow[GetSymIndex(nonTerminal), GetSymIndex(terminal)];
        }

        public Set First(Symbol sym)
        {
            Set first = new Set();

            if (sym.Terminal)
            {
                first.Add(sym);
                return first;
            }

            Set terminals = GetTerminals();
            foreach (Symbol terminal in terminals)
            {
                if (GetFirst(sym, terminal))
                    first.Add(terminal);
            }
            return first;
        }

        private Set GetTerminals()
        {
            Set terminals = new Set();
            foreach (object o in GetGrammarSymbols())
            {
                Symbol sym = (Symbol) o;
                if (sym.Terminal)
                    terminals.Add(sym);
            }
            return terminals;
        }

        private Set First(LinkedList<Symbol> sequence)
        {
            Set first = new Set();

            LinkedListNode<Symbol> currNode = sequence.First;
            while (currNode != null)
            {
                if (!currNode.Value.Epsilon)
                    first += First(currNode.Value);

                if (Empty(currNode.Value))
                    currNode = currNode.Next;
                else
                    break;
            }

            return first;
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
            if (sym.Epsilon)
                return true;
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
                log += "DS[" + production + "] = " +
                       dirSyms + "\r\n";
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
                        // mark prod head sym as non eps generating
                        if (EmptyState.EMPTY == m_empty[sym])
                        {
                            prod.RemoveFromTail(sym);
                            if (prod.HasEmptyTail())
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

            //���������� ��������� ����������_�����_�
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
            //���������� �������������� ��������� ��������� ����������_�����_�
            Set grammarSyms = GetGrammarSymbols();
            foreach (Symbol sym in grammarSyms)
            {
                SetFirst(sym, sym, true);
            }
            //���������� ������������� ��������� ��������� ����������_�����_�
            m_first = Transitive(m_first);
        }

        //��������� ������������ ��������� ������� � ������� ��������� ��������
        private bool[,] Transitive(bool[,] m)
        {
            bool[,] w = m;
            int size = m.GetLength(0);
            for (int k = 0; k < size; k++)
                for (int i = 0; i < size; i++)
                    for (int j = 0; j < size; j++)
                        w[i, j] = w[i, j] || (w[i, k] && w[k, j]);
            return w;
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

        private bool GetFirst(Symbol nonTerminal, Symbol terminal)
        {
            return m_first[GetSymIndex(nonTerminal), GetSymIndex(terminal)];
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

        private void CreateFollowRelationTable()
        {
            //������ ��������� �����_�����
            bool[,] straightBefore = GetStraightBeforeRelation();
            //������ ��������� �����_��_�����
            bool[,] straightAtTheEnd = GetStraightAtTheEndRelation();

            //�������� ��������� ��_����� ��� �����������-������������ ��������� �����_��_�����
            bool[,] atTheEnd = straightAtTheEnd;
            Set grammarSyms = GetGrammarSymbols();
            foreach (Symbol sym in grammarSyms)
            {
                atTheEnd[GetSymIndex(sym), GetSymIndex(sym)] = true;
            }
            atTheEnd = Transitive(atTheEnd);
            //�������� ��������� ����� ��� ������������ ���������
            //��_����� * �����_����� * ����������_�
            m_follow = Multiply(atTheEnd, straightBefore);
            m_follow = Multiply(m_follow, m_first);
        }

        private bool[,] GetStraightAtTheEndRelation()
        {
            int size = GetGrammarSymbols().Count;
            bool[,] straightAtTheEnd = new bool[size,size];
            // A GetStraightAtTheEnd B if exists production
            // B > alpha A beta, where beta is eps gen
            foreach (Production production in grammar)
            {
                for (int aIndex = 0; aIndex < production.Tail.Count; aIndex++)
                {
                    Symbol A = production.TailAt(aIndex);
                    if (!A.Terminal && !A.Epsilon) //A
                    {
                        LinkedList<Symbol> beta = production.SubTail(aIndex + 1, production.Tail.Count);
                        if (Empty(beta))
                            straightAtTheEnd[GetSymIndex(A), GetSymIndex(production.Head)] = true;
                    }
                }
            }
            return straightAtTheEnd;
        }

        private bool[,] GetStraightBeforeRelation()
        {
            int size = GetGrammarSymbols().Count;
            bool[,] straightBefore = new bool[size,size];

            // A straightBefore B if
            // exists production D > alpha A beta B gamma
            foreach (Production production in grammar)
            {
                for (int aIndex = 0; aIndex < production.Tail.Count - 1; aIndex++)
                {
                    Symbol A = production.TailAt(aIndex);
                    if (!A.Terminal) //A
                    {
                        for (int bIndex = aIndex + 1; bIndex < production.Tail.Count; bIndex++)
                        {
                            Symbol B = production.TailAt(bIndex); //B
                            LinkedList<Symbol> beta = production.SubTail(aIndex + 1, bIndex);
                            if (Empty(beta) && !B.Epsilon)
                                straightBefore[GetSymIndex(A), GetSymIndex(B)] = true;
                        }
                    }
                }
            }

            return straightBefore;
        }

        private bool[,] Multiply(bool[,] p, bool[,] q)
        {
            /*����� � � Q � ��� �������� ���������; ����� �� ������������ PQ � 
             * ��������� �������� ��������� ��������� � ������������ ��� � � � 
             * (�.�. ������������ xPQy ����������� ��������) ����� � ������ �����,
             * ����� � � ���������� ������� z �����, ��� ����� ��� xPz, ��� � zQy.*/
            int grammarSymsCount = GetGrammarSymbols().Count;
            bool[,] result = new bool[grammarSymsCount,grammarSymsCount];
            ;
            for (int xIndex = 0; xIndex < grammarSymsCount; xIndex++)
            {
                for (int zIndex = 0; zIndex < grammarSymsCount; zIndex++)
                {
                    if (p[xIndex, zIndex]) //xPz
                        for (int yIndex = 0; yIndex < grammarSymsCount; yIndex++)
                        {
                            if (q[zIndex, yIndex]) //zQy
                                result[xIndex, yIndex] = true; //xPQy
                        }
                }
            }
            return result;
        }

        private int GetSymIndex(Symbol symbol)
        {
            Set grammarSyms = GetGrammarSymbols();
            List<Symbol> syms = new List<Symbol>();
            foreach (object obj in grammarSyms)
            {
                Symbol sym = (Symbol) obj;
                syms.Add(sym);
            }
            return Array.BinarySearch(syms.ToArray(), symbol);
        }

        public Dictionary<Symbol, EmptyState> GetEmptyHashtable()
        {
            return m_empty;
        }

        public Production GetProduction(int i)
        {
            return grammar.ToArray()[i];
        }
    }
}