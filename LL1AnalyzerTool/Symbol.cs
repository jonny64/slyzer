using System;

namespace LL1AnalyzerTool
{
    public class Symbol : IComparable, IEquatable<Symbol>
    {
        public const string EPSILON_STRING = "#";
        public const string TERMINATOR_STRING = "terminator";
        private readonly string representation;

        public Symbol(string representation)
        {
            this.representation = representation;
        }

        public bool Terminator
        {
            get { return representation == TERMINATOR_STRING; }
        }

        // terminal identification rule
        public bool Terminal
        {
            get
            {
                return (Char.IsLower(representation[0])) &&
                       (!Epsilon);
            }
        }

        public bool Epsilon
        {
            get { return representation == EPSILON_STRING; }
        }

        public static Symbol TERMINATOR
        {
            get { return new Symbol(TERMINATOR_STRING); }
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj.GetType() != GetType())
                throw new NotImplementedException();

            var rhs = (Symbol) obj;
            if (rhs.representation == representation)
                return 0;

            // nonterminal is 'greater' than non terminal
            return representation.CompareTo(rhs.representation);
        }

        #endregion

        #region IEquatable<Symbol> Members

        public bool Equals(Symbol other)
        {
            return representation == other.representation;
        }

        #endregion

        public static bool operator ==(Symbol x, Symbol y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(Symbol x, Symbol y)
        {
            return !x.Equals(y);
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
    }
}