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
                syms = syms + new Set(sym);
            }
            return syms;
        }
    }
}
