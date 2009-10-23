using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;

namespace LL1AnalyzerTool
{
    /// <summary>
    /// since input files are linked in as resources in the test assembly
    /// this is a way to pull them out
    /// </summary>
    public static class ResLoader
    {
        public static string AsString<T>(string resName)
        {
            using (var reader = GetReader<T>(resName) )
            {
                return reader.ReadToEnd();
            }
        }
        public static StreamReader GetReader<T>(string resName)
        {
            return new StreamReader(Assembly.GetAssembly(typeof(T))
                                   .GetManifestResourceStream(resName));
        }
    }

    public class Grammar
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
        private LinkedList<Production> m_productions = new LinkedList<Production>();
        private bool[,] m_first;
        private bool[,] m_follow;
        private Set m_syms;

        public Production this[int arg]
        {
          get
          {
            return GetProductionAt(arg);
          }
        }

        public Grammar(string[] productions)
        {
            foreach (string prodString in productions)
            {
                Production prod = new Production(prodString);
                Productions.AddLast(prod);
            }
            
            m_syms = new Set();
            foreach (Production prod in Productions)
            {
                m_syms = m_syms + prod.ToSet();
            }

            CreateEmptySymTable();
            CreateFirstRelationTable();
            CreateFollowRelationTable();
        }

        public Grammar(Grammar other)
        {
            m_empty = other.m_empty;
            m_first = other.m_first;
            m_follow = other.m_follow;
            m_productions = other.m_productions;
            m_syms = other.m_syms;
        }

        public void Sort()
        {
            Production[] arr = new Production[m_productions.Count];
            m_productions.CopyTo(arr, 0);
            Array.Sort(arr);
            
            LinkedList<Production> updatedProductions = new LinkedList<Production>(arr);
            // move starter (S->alpha) productions to beginning
            foreach (Production production in m_productions)
            {
                if (production.Starter)
                {
                    updatedProductions.Remove(production);
                    updatedProductions.AddFirst(production);
                }
            }

            m_productions = updatedProductions;
        }

        public LinkedList<Production> Productions
        {
            get { return m_productions; }
        }

        public int Length
        {
            get { return m_productions.Count; }
        }

        public Set GetDirectionSymbols(Production production)
        {
            return GetDirectionSymbols(production.ToLinkedList());
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
            if ((sym.Terminator)||(sym.Epsilon))
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
            foreach (Production production in Productions)
            {
                Set dirSyms = GetDirectionSymbols(production.ToLinkedList());
                log += "DS[" + production + "] = " +
                       dirSyms + "\r\n";
            }
            return log;
        }

        private void CreateEmptySymTable()
        {
            LinkedList<Production> grammar = new LinkedList<Production>(this.Productions);

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

            //вычисление отношения начинается_прямо_с
            foreach (Production prod in Productions)
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
            //вычисление рефлексивноого замыкания отношения начинается_прямо_с
            Set grammarSyms = GetGrammarSymbols();
            foreach (Symbol sym in grammarSyms)
            {
                SetFirst(sym, sym, true);
            }
            //вычисление транзитивного замыкания отношения начинается_прямо_с
            m_first = Transitive(m_first);
        }

        //вычисляет транзитивное замыкание матрицы с помощью алгоритма Уоршелла
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
            return m_syms;
        }

        private void CreateFollowRelationTable()
        {
            //строим отношение прямо_перед
            bool[,] straightBefore = GetStraightBeforeRelation();
            //строим отношение прямо_на_конце
            bool[,] straightAtTheEnd = GetStraightAtTheEndRelation();

            //вычислим отношение на_конце как рефлексивно-транзитивное замыкание прямо_на_конце
            bool[,] atTheEnd = straightAtTheEnd;
            Set grammarSyms = GetGrammarSymbols();
            foreach (Symbol sym in grammarSyms)
            {
                atTheEnd[GetSymIndex(sym), GetSymIndex(sym)] = true;
            }
            atTheEnd = Transitive(atTheEnd);
            //вычислим отношение перед как произведение отношений
            //на_конце * прямо_перед * начинается_с
            m_follow = Multiply(atTheEnd, straightBefore);
            m_follow = Multiply(m_follow, m_first);
        }

        private bool[,] GetStraightAtTheEndRelation()
        {
            int size = GetGrammarSymbols().Count;
            bool[,] straightAtTheEnd = new bool[size,size];
            // A GetStraightAtTheEnd B if exists production
            // B > alpha A beta, where beta is eps gen
            foreach (Production production in Productions)
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
            foreach (Production production in Productions)
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
            /*Пусть Р и Q — два бинарных отношения; тогда их произведе­ние PQ — 
             * результат операции умножения отношений — выполня­ется для х и у 
             * (т.е. высказывание xPQy оказывается истинным) тогда и только тогда,
             * когда в М существует элемент z такой, что верно как xPz, так и zQy.*/
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

        public Production GetProductionAt(int i)
        {
            Production[] arr = new Production[m_productions.Count];
            m_productions.CopyTo(arr, 0);
            return arr[i];
        }

        public static Grammar LoadFromFile(string filename)
        {
            StreamReader sr = new StreamReader(filename);

            return LoadFromStream(sr);
        }

        public static Grammar LoadFromStream(StreamReader sr)
        {
            List<string> productions = new List<string>();
            while (sr.Peek() != -1)
            {
                string line = sr.ReadLine();
                if ((line.Length == 0) || (line[0] == ';') || (line == "\n"))
                {
                    continue;
                }
                productions.Add(line);
            }
            Grammar result = new Grammar(productions.ToArray());
            return result;
        }

        public Set GetDirectSymbols(Production production)
        {
            return GetDirectionSymbols(production.ToLinkedList());
        }

        // determines if grammar is an LL1 grammar
        public bool LL1
        {
            get
            {
                // alternative productions will be sticked together
                Grammar copy  = new Grammar(this);
                copy.Sort();

                Symbol currHead = new Symbol("");
                Set accumulator = new Set();
                for (int i = 0; i < copy.Length; i++)
                {
                    if (!copy[i].Head.Equals(currHead))
                    {
                        // switch no next alternative productions block
                        currHead = copy[i].Head;
                        accumulator = copy.GetDirectionSymbols(copy[i]);
                    }
                    else
                    {
                        // if alternative productions have some crosses in DS
                        Set currProdDS = copy.GetDirectionSymbols(copy[i]);
                        if ( !(accumulator * currProdDS).Empty )
                        {
                            return false;
                        }
                        accumulator += currProdDS;
                    }

                }
                return true;
            }
        }
    }
}