using System;
using System.Collections.Generic;
using System.Text;

namespace LL1AnalyzerTool
{
    class Production
    {
        private List<Symbol> prod = new List<Symbol>();
    
        public Production(string prodString)
        {
            char[] seps ={ ' ' };
            string[] syms = prodString.Split(seps,
                StringSplitOptions.RemoveEmptyEntries);
            foreach (string symString in syms)
            {
                prod.Add(new Symbol(symString));
            }
        }

        public Production(List<Symbol> prodList)
        {
            foreach (Symbol sym in prodList)
                this.prod.Add(sym);
        }
    
        public Symbol Head
        {
            get
            {
                return prod.ToArray()[0];
            }
        }

        public Set Tail
        {
            get
            {
                object[] values = prod.GetRange(1, prod.Count - 1).ToArray();
                return new Set(values);
            }
        }

        // right part of production consists only of one epsilon sym
        public bool Epsilon
        {
            get
            {
                return (Tail / (new Set(new Symbol(Symbol.EPSILON_STRING))) ).Empty;
            }
        }

        public LinkedList<Symbol> ToLinkedList()
        {
            return new LinkedList<Symbol>(prod);
        }

        public override string ToString()
        {
            return Head.ToString() + "->" + Tail.ToString();
        }

        public Set ToSet()
        {
            Set syms = new Set();
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

            LinkedList<Symbol> alpha = new LinkedList<Symbol>();
            for (int i = 0; i < endIndex; i++)
			{
			    alpha.AddLast(rightPart[i]);
			}
            
            return alpha;
        }

        private Symbol[] TailToSymbolArray()
        {
            List<Symbol> syms = new List<Symbol>();
            object[] content = Tail.ToArray();
            foreach (object obj in content)
            {
                Symbol sym = (Symbol)obj;
                syms.Add(sym);
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
            prod.Remove(sym);
        }

        internal Symbol TailAt(int i)
        {
            return TailToSymbolArray()[i];
        }

        #region ICloneable Members

        public Production Clone()
        {
            return new Production(this.prod);
        }

        #endregion

        internal LinkedList<Symbol> SubTail(int startIndex, int endIndex)
        {
            Symbol[] rightPart = TailToSymbolArray();

            LinkedList<Symbol> alpha = new LinkedList<Symbol>();
            for (int i = startIndex; i < endIndex; i++)
            {
                alpha.AddLast(rightPart[i]);
            }

            return alpha;
        }
    }
}
