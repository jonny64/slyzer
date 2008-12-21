using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace LL1AnalyzerTool
{
    public class Symbol : IComparable, IEquatable<Symbol>
    {
        private string representation;
        public const string EPSILON_STRING = "#";
        // terminal identification rule
        public bool Terminal
        {
            get { return Char.IsLower(representation[0]); }
        }

        public bool Epsilon
        {
            get
            {
                return representation == EPSILON_STRING;
            }
        }
    
        public Symbol(string representation)
        {
            this.representation = representation;
        }

        public override string ToString()
        {
            return representation;
        }

        public override int GetHashCode()
        {
            int sum = 0;
            for (int i = 0; i < representation.Length; i++)
            {
                sum += representation[i];
            }
            return sum;
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
            return this.representation.CompareTo(rhs.representation);
        }

        #endregion

        #region IEquatable<Symbol> Members

        public bool Equals(Symbol other)
        {
            return this.representation == other.representation;
        }

        #endregion
    }
}
