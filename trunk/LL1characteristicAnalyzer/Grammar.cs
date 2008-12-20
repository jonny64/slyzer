using System;
using System.Collections.Generic;
using System.Text;

namespace LL1AnalyzerTool
{
    class Grammar
    {
        public Grammar(string[] productions, string[] terminalWords)
        {
            foreach (string production in productions)
            {
                Production prod = new Production(production);
                grammar.Add(prod);
            }
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
            Set directionSymbols = new Set();
            directionSymbols.Add(new Symbol("test") );
            
            return directionSymbols;
        }

        public Set GetDirectionSymbols(List<LL1AnalyzerTool.Symbol> sequence, string[] productions, string[] terminalWords)
        {
            throw new System.NotImplementedException();
        }

        public Set GetDirectionSymbols(List<Symbol> sequence)
        {
            Set directionSymbols = new Set();
            directionSymbols.Add(new Symbol("test"));

            return directionSymbols;
        }

        // show direction syms summary for each production
        internal string GetDirectionSymbolsLog()
        {
            string log = "";
            foreach (Production production in grammar)
            {
                Set dirSyms = GetDirectionSymbols(production.ToList());
                log += "DS[" + production.Head + ">" + 
                    production.Tail.ToString() + 
                    "] = " + dirSyms.ToString() + "\r\n";
            }
            return log;
        }
    }
}
