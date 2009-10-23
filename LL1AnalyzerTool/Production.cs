using System;
using System.Collections.Generic;

namespace LL1AnalyzerTool
{
    public class Production : IComparable<Production>
    {
        private readonly string representation = "";
        public Symbol FIRST_GRAMMAR_SYMBOL = new Symbol("S");
        private LinkedList<Symbol> prod = new LinkedList<Symbol>();

        public Production(string prodString)
        {
            representation = prodString;

            char[] seps = {' '};
            string[] syms = prodString.Split(seps,
                                             StringSplitOptions.RemoveEmptyEntries);
            foreach (string symString in syms)
            {
                prod.AddLast(new Symbol(symString));
            }
            if (Starter &&
                !HasEpsilonTail())
                prod.AddLast(Symbol.TERMINATOR);
        }

        public Production(LinkedList<Symbol> prodList)
        {
            foreach (Symbol sym in prodList)
                prod.AddLast(sym);
        }

        public int LengthWithoutTerminator
        {
            get
            {
                if (Tail.Contains(Symbol.TERMINATOR))
                    return prod.Count - 1;
                return prod.Count;
            }
        }

        public int Length
        {
            get { return prod.Count; }
        }

        public bool Starter
        {
            get { return Head.Equals(FIRST_GRAMMAR_SYMBOL); }
        }

        public Symbol Head
        {
            get { return prod.First.Value; }
        }

        public LinkedList<Symbol> Tail
        {
            get
            {
                var tail = new LinkedList<Symbol>(prod);
                tail.RemoveFirst();
                return tail;
            }
            set
            {
                var newProd = new LinkedList<Symbol>();
                newProd.AddLast(Head);
                foreach (Symbol sym in value)
                {
                    newProd.AddLast(sym);
                }
                prod = newProd;
            }
        }

        // right part of production consists only of one epsilon sym
        public bool Epsilon
        {
            get { return ((Tail.Count == 1) && Tail.First.Value.Epsilon); }
        }

        #region IComparable<Production> Members

        public int CompareTo(Production other)
        {
            return representation.CompareTo(other.representation);
        }

        #endregion

        private bool HasEpsilonTail()
        {
            return TailAt(Tail.Count - 1).Epsilon;
        }

        public override string ToString()
        {
            return Head + "->" + TailToString();
        }

        private string TailToString()
        {
            string result = "";
            foreach (Symbol sym in Tail)
            {
                result += sym + " ";
            }
            return result;
        }

        public LinkedList<Symbol> ToLinkedList()
        {
            return new LinkedList<Symbol>(prod);
        }

        public Set ToSet()
        {
            var syms = new Set();
            foreach (Symbol sym in prod)
            {
                if (!sym.Epsilon)
                    syms = syms + new Set(sym);
            }
            return syms;
        }

        public LinkedList<Symbol> SubTail(int endIndex)
        {
            Symbol[] rightPart = TailToSymbolArray();

            var alpha = new LinkedList<Symbol>();
            for (int i = 0; i < endIndex; i++)
            {
                alpha.AddLast(rightPart[i]);
            }

            return alpha;
        }

        private Symbol[] TailToSymbolArray()
        {
            var syms = new List<Symbol>();
            LinkedListNode<Symbol> currNode = prod.First.Next;
            while (currNode != null)
            {
                syms.Add(currNode.Value);
                currNode = currNode.Next;
            }
            return syms.ToArray();
        }

        internal bool ContainsTerminals()
        {
            foreach (Symbol sym in Tail)
            {
                if (sym.Terminal)
                    return true;
            }
            return false;
        }

        internal void RemoveFromTail(Symbol sym)
        {
            var newTail = new LinkedList<Symbol>(Tail);
            newTail.Remove(sym);
            Tail = newTail;
        }

        internal Symbol TailAt(int i)
        {
            return TailToSymbolArray()[i];
        }

        public Production Clone()
        {
            return new Production(prod);
        }

        internal LinkedList<Symbol> SubTail(int startIndex, int endIndex)
        {
            Symbol[] rightPart = TailToSymbolArray();

            var alpha = new LinkedList<Symbol>();
            for (int i = startIndex; i < endIndex; i++)
            {
                alpha.AddLast(rightPart[i]);
            }

            return alpha;
        }

        internal bool HasEmptyTail()
        {
            return (Tail.Count == 0);
        }

        internal Set TailToSet()
        {
            Symbol[] rightPart = TailToSymbolArray();

            var alpha = new Set();
            for (int i = 0; i < rightPart.Length; i++)
            {
                alpha.Add(rightPart[i]);
            }

            return alpha;
        }
    }
}