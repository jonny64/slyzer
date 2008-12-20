using System;
using System.Collections.Generic;
using System.Text;

namespace LL1AnalyzerTool
{
    class Grammar
    {
        public Grammar(string[] productions, string[] terminalWords)
        {
            throw new System.NotImplementedException();
        }

        public List<Production> grammar
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public Set GetDirectionSymbols(List<Symbol> sequence)
        {
            Set directionSymbols = new Set();
            directionSymbols.Add(new Symbol("test") );
            
            return directionSymbols;
        }

        public Set GetDirectionSymbols(string[] productions, string[] terminalWords)
        {
            throw new System.NotImplementedException();
        }

        // show direction syms summary for each production
        internal string GetDirectionSymbolsLog()
        {
            string log = "";
            foreach (Production production in grammar)
            {
                Set dirSyms = GetDirectionSymbols(production.ToList());
                log += "DS(" + production.Head + ">" + 
                    production.Tail + 
                    ") = " + dirSyms.ToString() + "\n";
            }
            return log;
        }
    }
}
