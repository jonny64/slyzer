using System;
using System.Collections.Generic;
using System.Text;

namespace LL1AnalyzerTool
{
    class Production
    {
        private List<Symbol> prod;
    
        public Production(string[] prodString)
        {
            throw new System.NotImplementedException();
        }
    
        public Symbol Head
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public List<Symbol> Tail
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public List<Symbol> ToList()
        {
            return prod;
        }

        public override string ToString()
        {
            return Head.ToString() + "->" + Tail.ToString();
        }
    }
}
