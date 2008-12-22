/*Copyleft (l) 2008 ������� �.
 * ���������� ������� �� ���������
 * �� ���� � �����������  - ������ ����� - ��������� , 
 * �������������� ��������� ������ ���� ���������
 * � ������ ��������� ��������� ��� �����������
 */
using System;
using System.Collections;
using System.Collections.Generic;

namespace LL1AnalyzerTool
{
    internal class GrammarAnalyzer
    {
        public char EPSILON_CHAR = '#';
        //�������� empty ������ ������� �����������
        public Hashtable m_empty = new Hashtable();
        //��������������� ������� ��������� first,follow
        private bool[,] m_first;
        private bool[,] m_follow;
        protected string[] m_grammar;
        public char[] m_grammarSyms = {};
        public int m_symCount;

        public GrammarAnalyzer(string[] productions)
        {
            m_grammar = productions;
            m_symCount = GetGrammarSymbols().Length;
            m_grammarSyms = GetGrammarSymbols();

            //����������� empty ����� ������������
            //��������� ������� ��������� m_first
            //��������� ������� ��������� m_follow
            FillEmptySymHashtable();
            FillFirstRelationTable();
            FillFollowRelationTable();
        }

        //��� ����������� (eps-���������� ��� ���)

        public char[] GetDirectSymbols(string production)
        {
            char head = production[0];
            string rightPart = production.Substring(1);
            char[] ds = {EPSILON_CHAR};
            //if (rightPart == EPSILON_CHAR.ToString())
            //return ds;
            if (Empty(rightPart))
                ds = Union(First(rightPart), Follow(head));
            else
                ds = First(rightPart);
            return ds;
        }

        //�������� �� ������ eps-����������
        public bool Empty(string alpha)
        {
            bool generateEps = true;
            //������ �� eps-���������� ���� ���� �� 1 �� 
            //���������� �� eps-����������
            for (int i = 0; i < alpha.Length; i++)
                if (EmptySym(alpha[i]) == EmptyState.IS_NON_EMPTY)
                {
                    generateEps = false;
                    break;
                }
            return generateEps;
        }

        //��������� ��������� First ��� ������ alpha
        public char[] First(string alpha)
        {
            char[] first = {};
            //��������� First ���������� ������������
            //�������� first ��� ������� �������
            for (int i = 0; i < alpha.Length; i++)
            {
                if (Empty(alpha.Substring(0, i)))
                    first = Union(first, First(alpha[i]));
            }
            return first;
        }

        private void FillFirstRelationTable()
        {
            int grammarSymsCount = GetGrammarSymbols().Length;
            m_first = new bool[grammarSymsCount,grammarSymsCount];

            //���������� ��������� ����������_�����_�
            for (int prodIndex = 0; prodIndex < m_grammar.Length; prodIndex++)
            {
                string production = m_grammar[prodIndex];
                char head = production[0];
                for (int symIndex = 1; symIndex < production.Length; symIndex++)
                {
                    char sym = production[symIndex];
                    string alpha = production.Substring(1, symIndex - 1);
                    if (Empty(alpha) && (sym != EPSILON_CHAR))
                    {
                        m_first[GetSymIndex(head), GetSymIndex(sym)] = true;
                    }
                }
            }
            //���������� �������������� ��������� ��������� ����������_�����_�
            char[] grammarSyms = GetGrammarSymbols();
            for (int symIndex = 0; symIndex < grammarSyms.Length; symIndex++)
            {
                char sym = grammarSyms[symIndex];
                m_first[GetSymIndex(sym), GetSymIndex(sym)] = true;
            }
            //���������� ������������� ��������� ��������� ����������_�����_�
            m_first = Transitive(m_first);
        }

        //��������� ������������ ��������� ������� � ������� ��������� ��������
        private bool[,] Transitive(bool[,] m)
        {
            bool[,] w = m;
            int size = m.GetLength(0);
            for (int k = 0; k < size; k++)
                for (int i = 0; i < size; i++)
                    for (int j = 0; j < size; j++)
                        w[i, j] = w[i, j] || (w[i, k] && w[k, j]);
            return w;
        }

        //���������� ���� ��������
        public char[] Union(char[] setOne, char[] setTwo)
        {
            List<char> resultSet = new List<char>();
            for (int i = 0; i < setOne.Length; i++)
            {
                if (resultSet.BinarySearch(setOne[i]) < 0)
                    resultSet.Add(setOne[i]);
            }
            for (int i = 0; i < setTwo.Length; i++)
            {
                if (resultSet.BinarySearch(setTwo[i]) < 0)
                    resultSet.Add(setTwo[i]);
            }
            return resultSet.ToArray();
        }

        //��������� ������� � ���������
        public char[] Union(char[] setOne, char sym)
        {
            if (Array.BinarySearch(setOne, sym) < 0)
            {
                char[] resultSet = new char[setOne.Length + 1];
                resultSet[resultSet.Length - 1] = sym;
                setOne.CopyTo(resultSet, 0);
                return resultSet;
            }
            else return setOne;
        }

        //��������� ��������� First ��� ���������� �������
        public char[] First(char sym)
        {
            char[] result = {};
            char[] grammarSyms = GetGrammarSymbols();
            for (int colIndex = 0; colIndex < grammarSyms.Length; colIndex++)
            {
                //���� ��������� � ��� �������� - ��������� � ���������
                bool terminal = Terminal(m_grammarSyms[colIndex]);
                if ((m_first[GetSymIndex(sym), colIndex]) && terminal)
                {
                    result = Union(result, grammarSyms[colIndex]);
                }
            }
            return result;
        }

        // TODO ��� ���������� char.IsLower � ���� �������� �� ���� �����
        //�������������� ������ ��� ��������
        public bool Terminal(char sym)
        {
            if (char.IsLower(sym)) return true;
            else return false;
        }

        private int GetSymIndex(char sym)
        {
            return Array.BinarySearch(m_grammarSyms, sym);
        }

        // TODO ������ ����� ������ �� ����� ���� �������� ����������� � ����� 
        // ������ m_grammarSyms. GetGrammarSymbols().Length �������� �� m_symCount
        // ����� ����� ������ ��� ������������ m_grammarSyms
        private char[] GetGrammarSymbols()
        {
            List<char> grammarSymbols = new List<char>();

            for (int prodIndex = 0; prodIndex < m_grammar.Length; prodIndex++)
            {
                for (int symIndex = 0; symIndex < m_grammar[prodIndex].Length; symIndex++)
                {
                    char sym = m_grammar[prodIndex][symIndex];
                    if (!grammarSymbols.Contains(sym))
                        grammarSymbols.Add(sym);
                }
            }
            grammarSymbols.Sort();
            return grammarSymbols.ToArray();
        }

        //�������� �� ��������/���������� eps-����������
        private EmptyState EmptySym(char sym)
        {
            if (char.IsLower(sym)) return EmptyState.IS_NON_EMPTY; //���� ��������
            else //����� ������� � �������
            {
                object state = m_empty[sym];
                EmptyState[] equals = {EmptyState.IS_NON_EMPTY /*false*/, EmptyState.IS_EMPTY /*true*/};
                if (state != null)
                    return equals[Convert.ToByte((bool) state)];
                else return EmptyState.EMPTY_STATE_UNKNOWN;
            }
        }

        //������ ������ ���� ������������ ����������
        private char[] GetNonTerminals()
        {
            List<char> NonTerminals = new List<char>();
            for (int productionIndex = 0; productionIndex < m_grammar.Length; productionIndex++)
                for (int terminalIndex = 0; terminalIndex < m_grammar[productionIndex].Length; terminalIndex++)
                {
                    char term = m_grammar[productionIndex][terminalIndex];
                    if (!(Terminal(term) || NonTerminals.Contains(term) || (term == EPSILON_CHAR)))
                        NonTerminals.Add(term);
                }
            return NonTerminals.ToArray();
        }

        //Copyleft (l) 2008 ������� �
        //�������������� ������� empty ������������

        private void FillFollowRelationTable()
        {
            //������ ��������� �����_�����
            bool[,] straightBefore = GetStraightBeforeRelation();
            //������ ��������� �����_��_�����
            bool[,] straightAtTheEnd = GetStraightAtTheEndRelation();

            //�������� ��������� ��_����� ��� �����������-������������ ��������� �����_��_�����
            bool[,] atTheEnd = straightAtTheEnd;
            for (int symIndex = 0; symIndex < m_symCount; symIndex++)
            {
                char sym = m_grammarSyms[symIndex];
                atTheEnd[GetSymIndex(sym), GetSymIndex(sym)] = true;
            }
            atTheEnd = Transitive(atTheEnd);
            //�������� ��������� ����� ��� ������������ ���������
            //��_����� * �����_����� * ����������_�
            m_follow = Multiply(atTheEnd, straightBefore);
            m_follow = Multiply(m_follow, m_first);
        }

        private bool[,] Multiply(bool[,] p, bool[,] q)
        {
            /*����� � � Q � ��� �������� ���������; ����� �� ������������ PQ � 
             * ��������� �������� ��������� ��������� � ������������ ��� � � � 
             * (�.�. ������������ xPQy ����������� ��������) ����� � ������ �����,
             * ����� � � ���������� ������� z �����, ��� ����� ��� xPz, ��� � zQy.*/
            bool[,] result = new bool[m_symCount,m_symCount];
            for (int xIndex = 0; xIndex < m_symCount; xIndex++)
            {
                for (int zIndex = 0; zIndex < m_symCount; zIndex++)
                {
                    if (p[xIndex, zIndex]) //xPz
                        for (int yIndex = 0; yIndex < m_symCount; yIndex++)
                        {
                            if (q[zIndex, yIndex]) //zQy
                                result[xIndex, yIndex] = true; //xPQy
                        }
                    ;
                }
            }
            return result;
        }

        private bool[,] GetStraightAtTheEndRelation()
        {
            int size = m_grammarSyms.Length;
            bool[,] straightAtTheEnd = new bool[size,size];

            for (int prodIndex = 0; prodIndex < m_grammar.Length; prodIndex++)
            {
                string production = m_grammar[prodIndex];
                char head = production[0]; //B
                for (int aIndex = 1; aIndex < production.Length; aIndex++)
                {
                    //���� ��������� B>alphaAbeta, ��� beta eps �����������
                    char a = production[aIndex];
                    if (!Terminal(a) && (a != EPSILON_CHAR)) //A
                    {
                        string beta = production.Substring(aIndex + 1);
                        if (Empty(beta))
                            straightAtTheEnd[GetSymIndex(a), GetSymIndex(head)] = true;
                    }
                }
            }
            return straightAtTheEnd;
        }

        //Copyleft (l) 2008 ������� �
        private bool[,] GetStraightBeforeRelation()
        {
            int size = m_grammarSyms.Length;
            bool[,] straightBefore = new bool[size,size];

            for (int prodIndex = 0; prodIndex < m_grammar.Length; prodIndex++)
            {
                string production = m_grammar[prodIndex];
                char head = production[0];
                for (int symIndex = 1; symIndex < production.Length; symIndex++)
                {
                    char a = production[symIndex];
                    if (!Terminal(a)) //A
                    {
                        for (int bIndex = symIndex + 1; bIndex < production.Length; bIndex++)
                        {
                            char b = production[bIndex]; //B
                            string beta = production.Substring(symIndex + 1, bIndex - symIndex - 1);
                            if (Empty(beta))
                                straightBefore[GetSymIndex(a), GetSymIndex(b)] = true;
                        }
                    }
                }
            }

            return straightBefore;
        }

        //�������� (eps �����������) ������ ������������
        private void FillEmptySymHashtable()
        {
            bool[] excluded = new bool[m_grammar.Length]; //��������, ��� ���������� ��������� �� ������������

            //����������� ������� ������ eps-���������
            DeleteEpsProductions(excluded);

            do
            {
                excluded = PerformBasicResearch(excluded);
                if (!FoundEachNonTerminalState()) PerformAdditionalResearch(excluded, m_grammar);
            } while (!FoundEachNonTerminalState());
        }

        //�������� �� ���������� eps����������
        public bool Empty(char sym)
        {
            if (!Terminal(sym) && (m_empty[sym] != null))
                return (bool) m_empty[sym];
            else return false;
        }

        private void DeleteEpsProductions(bool[] excluded)
        {
            //����������� ������� ������ eps-���������
            for (int prodIndex = 0; prodIndex < m_grammar.Length; prodIndex++)
            {
                string production = m_grammar[prodIndex];
                char head = production[0];

                bool productionIsEps = (production.Length == 2) && (production[1] == EPSILON_CHAR);
                if (productionIsEps)
                {
                    m_empty[head] = true;
                    excluded[prodIndex] = true;
                    DeleteAllTermProductions(head, excluded, m_grammar);
                }
            }
        }

        private bool[] PerformBasicResearch(bool[] excluded)
        {
            for (int prodIndex = 0; prodIndex < m_grammar.Length; prodIndex++)
            {
                string production = m_grammar[prodIndex];
                char head = production[0];
                if (excluded[prodIndex]) continue;

                if (RightPartContainsTeriminal(production))
                {
                    excluded[prodIndex] = true;
                }
                if (AllProductionsForHeadExcluded(ref excluded, head, ref m_grammar))
                {
                    m_empty[head] = false;
                    excluded[prodIndex] = true;
                }

                bool productionIsEps = (production.Length == 2) && (production[1] == EPSILON_CHAR);
                if (productionIsEps)
                {
                    m_empty[head] = true;
                    excluded[prodIndex] = true;
                    DeleteAllTermProductions(head, excluded, m_grammar);
                }
            }
            return excluded;
        }

        private void PerformAdditionalResearch(bool[] excludedProductions, string[] m_grammar)
        {
            for (int prodIndex = 0; prodIndex < m_grammar.Length; prodIndex++)
            {
                string production = m_grammar[prodIndex];
                char head = production[0];
                if (excludedProductions[prodIndex]) continue;

                if (HasNonEpsGenSymInRightPart(production))
                {
                    excludedProductions[prodIndex] = true;
                    if (AllProductionsForHeadExcluded(ref excludedProductions, head, ref m_grammar))
                        m_empty[head] = false;
                }
                if (AllProductionsForHeadExcluded(ref excludedProductions, head, ref m_grammar))
                {
                    m_empty[head] = false;
                    excludedProductions[prodIndex] = true;
                }

                // TODO ���������� ��������� ���� �� ���-������ ���������

                // � ������ ����� ���� eps ���������� ����������
                int EmptyNonTerminalIndex = GetEpsGenSymIndex(production);
                while (EmptyNonTerminalIndex > -1)
                {
                    production = production.Remove(EmptyNonTerminalIndex, 1);
                    //������ ����� ��������� �����
                    if (production.Length == 1)
                    {
                        //��������� �� � ��� ��������� ��� ������� �����������
                        m_empty[head] = true;
                        excludedProductions[prodIndex] = true;
                        DeleteAllTermProductions(head, excludedProductions, m_grammar);
                        break;
                    }
                    EmptyNonTerminalIndex = GetEpsGenSymIndex(production);
                }
            }
        }

        private int GetEpsGenSymIndex(string production)
        {
            //���� ������ ������� ������ ����� ���������, ���-�� eps ����������
            for (int termIndex = 1; termIndex < production.Length; termIndex++)
                if (EmptySym(production[termIndex]) == EmptyState.IS_EMPTY) return termIndex;
            return -1;
        }

        private bool HasNonEpsGenSymInRightPart(string production)
        {
            //���� ���� �� 1 ������ ������ ����� ��������� �� eps ����������
            for (int i = 1; i < production.Length; i++)
                if (EmptySym(production[i]) == EmptyState.IS_NON_EMPTY) return true;
            return false;
        }

        private void DeleteAllTermProductions(char term, bool[] excluded, string[] m_grammar)
        {
            //��������� �� ������������ ��� ��������� ��������������� ����������
            for (int prodIndex = 0; prodIndex < m_grammar.Length; prodIndex++)
                if (m_grammar[prodIndex][0] == term) excluded[prodIndex] = true;
        }

        private bool RightPartContainsTeriminal(string production)
        {
            foreach (char c in production)
                if (char.IsLower(c) && (c != EPSILON_CHAR)) return true;
            return false;
        }

        private bool AllProductionsForHeadExcluded(ref bool[] excluded, char head, ref string[] productions)
        {
            for (int prodIndex = 0; prodIndex < excluded.Length; prodIndex++)
                if ((excluded[prodIndex] == false) &&
                    (productions[prodIndex][0] == head)) return false;
            return true;
        }

        //���������, ��� ���� �� ������������ ������� ������ empty
        private bool FoundEachNonTerminalState()
        {
            char[] NonTerminals = GetNonTerminals();
            //������� �� empty ������ ������� �����������?
            for (int i = 0; i < NonTerminals.Length; i++)
                if (m_empty[NonTerminals[i]] == null) return false;
            return true;
        }

        //��������� follow ��� eps ����������� ������������
        public char[] Follow(char NonTerm)
        {
            char[] result = {};
            //��� ��������� ���������� a ��� sym ����� a
            for (int symIndex = 0; symIndex < m_grammarSyms.Length; symIndex++)
            {
                char sym = m_grammarSyms[symIndex];
                if (Terminal(sym) &&
                    m_follow[GetSymIndex(NonTerm), GetSymIndex(sym)])
                    result = Union(result, sym);
            }
            return result;
        }

        #region Nested type: EmptyState

        private enum EmptyState
        {
            IS_EMPTY,
            IS_NON_EMPTY,
            EMPTY_STATE_UNKNOWN
        } ;

        #endregion
    }
}

//Copyleft (l) 2008 ������� �