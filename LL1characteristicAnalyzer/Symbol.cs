using System;
using System.Collections.Generic;
using System.Text;

namespace LL1AnalyzerTool
{
    class Symbol
    {
        private string representation;
    
        public Symbol(string representation)
        {
            this.representation = representation;
        }

        public override string ToString()
        {
            return representation;
        }
    }
}
