using System;
using System.Collections.Generic;
using System.Text;

namespace LL1AnalyzerTool
{
    class Symbol : IComparable
    {
        private string representation;

        // terminal identification rule
        public bool Terminal
        {
            get { return Char.IsLower(representation[0]); }
        }
    
        public Symbol(string representation)
        {
            this.representation = representation;
        }

        public override string ToString()
        {
            return representation;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj.GetType() != this.GetType())
                throw new System.NotImplementedException();
            
            Symbol rhs = (Symbol)obj;
            if (rhs.representation == this.representation)
                return 0;

            // nonterminal is 'greater' than non terminal
            if (!this.Terminal && rhs.Terminal)
                return 1;
            
            return -1;
        }

        #endregion
    }
}
