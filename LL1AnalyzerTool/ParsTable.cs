using System;

namespace LL1AnalyzerTool
{
    //������ ������� �������
    public struct TableRow
    {
        public bool accept;
        public string dbgMsg;
        public bool error;
        public int jump;
        public bool stack;
        public Set terminals;

        public TableRow(Set _terms, int _jump, bool _accept, bool _stack, bool _error)
        {
            terminals = _terms;
            jump = _jump;
            accept = _accept;
            stack = _stack;
            error = _error;
            dbgMsg = "";
        }

        public override string ToString()
        {
            return terminals + dbgMsg;
        }
    }

    //������ ������� ������� ��� �������� ����������
    public class ParsTable
    {
        //������ ���� �������� ����������

        public const int JUMP_FINISH = -1;
        private readonly Grammar m_grammar;
        private TableRow[] m_table;
        private int[][] prodIDs;

        public ParsTable(Grammar grammar)
        {
            m_grammar = grammar;
            BuildTable();
        }

        public ParsTable(TableRow[] rows)
        {
            m_table = rows;
        }

        public TableRow this[int i]
        {
            get { return m_table[i]; }
        }

        public int Length
        {
            get { return m_table.Length; }
        }

        public override string ToString()
        {
            string view = String.Format("{0,3} {1,21} {2,4} {3,6} {4,6} {5,6}",
                                        "row",
                                        "terminals",
                                        "jump",
                                        "accept",
                                        "stack",
                                        "error"
                );
            for (int rowIndex = 0; rowIndex < m_table.Length; rowIndex++)
            {
                view += String.Format("\r\n{0,3} {1,21} {2,4} {3,6} {4,6} {5,6}",
                                      rowIndex,
                                      m_table[rowIndex].terminals,
                                      m_table[rowIndex].jump.ToString().ToLower(),
                                      m_table[rowIndex].accept.ToString().ToLower(),
                                      m_table[rowIndex].stack.ToString().ToLower(),
                                      m_table[rowIndex].error.ToString().ToLower()
                    );
            }
            return view;
        }

        public string ToCsharpSyntaxAnalyzerTable()
        {
            string view = "";
            for (int rowIndex = 0; rowIndex < m_table.Length; rowIndex++)
            {
                view += String.Format("\r\nnew TableRow({0,21}, {1,4}, {2,6}, {3,6}, {4,6}),",
                                      ToCsharpContructor(m_table[rowIndex].terminals),
                                      m_table[rowIndex].jump.ToString().ToLower(),
                                      m_table[rowIndex].accept.ToString().ToLower(),
                                      m_table[rowIndex].stack.ToString().ToLower(),
                                      m_table[rowIndex].error.ToString().ToLower()
                    );
            }
            // delete last ","
            view = view.Remove(view.Length - 1);
            return view;
        }

        private string ToCsharpContructor(Set set)
        {
            string result = "new Set(";
            foreach (Symbol sym in set)
            {
                result += String.Format("new Symbol(\"{0}\"), ", sym);
            }
            // remove last ", "
            result = result.Remove(result.Length - 2);
            result += ")";
            return result;
        }

        //�������� ������� �������
        public void BuildTable()
        {
            NumerateProductions();

            int currProdID;
            for (int prodIndex = 0; prodIndex < m_grammar.Length; prodIndex++)
            {
                currProdID = prodIDs[prodIndex][0];
                Production production = m_grammar.GetProductionAt(prodIndex);

                // �� ��������� ������������ -> error = false;
                bool LastAlternative = (prodIndex == m_grammar.Length - 1) ||
                                       (!m_grammar[prodIndex + 1].Head.Equals(production.Head));
                m_table[currProdID].error = LastAlternative;

                // ��������� ������ ��� ������ ���������
                m_table[currProdID].terminals = m_grammar.GetDirectSymbols(production);
                m_table[currProdID].jump = prodIDs[prodIndex][1];
                m_table[currProdID].dbgMsg = String.Format("For head of production {0}", production);

                // ���� ������ ����� ��������� eps
                if (production.Epsilon)
                {
                    currProdID = prodIDs[prodIndex][1];
                    m_table[currProdID].terminals = m_grammar.GetDirectSymbols(production);
                    m_table[currProdID].jump = JUMP_FINISH;
                    m_table[currProdID].error = true;
                    m_table[currProdID].dbgMsg = String.Format("For right part of eps production {0}", production);
                }
                else
                    // ��������� ������ ��� ������ ����� ���������
                    FillTableForRightPart(prodIndex, production);
            }
        }

        private void FillTableForRightPart(int prodIndex, Production production)
        {
            for (int symIndex = 1; symIndex < production.LengthWithoutTerminator; symIndex++)
            {
                int currProdID = prodIDs[prodIndex][symIndex];
                Symbol sym = production.TailAt(symIndex - 1);

                m_table[currProdID].error = true;
                //�������� � ������ �����
                if (sym.Terminal)
                {
                    m_table[currProdID].terminals = new Set(sym);
                    m_table[currProdID].accept = true;
                    // ������� ������ ��������: return = true
                    if (symIndex == production.LengthWithoutTerminator - 1)
                    {
                        m_table[currProdID].jump = JUMP_FINISH;
                    }
                    else
                        m_table[currProdID].jump = prodIDs[prodIndex][symIndex + 1];
                    m_table[currProdID].dbgMsg = String.Format("For right part sym {1} of production {0}", production,
                                                               sym);
                }
                else
                {
                    //���������� � ������ �����
                    m_table[currProdID].terminals = GetDSUnionForHeadNonTerm(sym);
                    if (symIndex < production.LengthWithoutTerminator - 1)
                        m_table[currProdID].stack = true;

                    m_table[currProdID].jump = GetFirstAlternativeProdID(sym);

                    m_table[currProdID].dbgMsg = String.Format("For right part nonterm {1} of production {0}",
                                                               production, sym);
                }
            }
        }

        //������� ����� �� ���������� �������� � ����������
        private uint GetGrammarSize()
        {
            uint sz = 0;
            for (int prodIndex = 0; prodIndex < m_grammar.Length; prodIndex++)
            {
                sz += (uint) m_grammar[prodIndex].LengthWithoutTerminator;
            }
            return sz;
        }

        private int GetFirstAlternativeProdID(Symbol sym)
        {
            for (int prodIndex = 0; prodIndex < m_grammar.Length; prodIndex++)
            {
                if (m_grammar.GetProductionAt(prodIndex).Head.Equals(sym))
                    return prodIDs[prodIndex][0];
            }
            throw new Exception("��� ���������, ����������� ������ '" + sym + "'");
        }

        // ����������� ������������ �������� ���� ��������� � ���� ������������ � ����� �����
        private Set GetDSUnionForHeadNonTerm(Symbol sym)
        {
            var result = new Set();
            foreach (Production production in m_grammar.Productions)
                if (production.Head.Equals(sym))
                    result += m_grammar.GetDirectSymbols(production);
            return result;
        }

        private void NumerateProductions()
        {
            m_grammar.Sort();
            prodIDs = new int[m_grammar.Length][];
            for (int prodIndex = 0; prodIndex < m_grammar.Length; prodIndex++)
            {
                Production production = m_grammar[prodIndex];
                prodIDs[prodIndex] = new int[production.LengthWithoutTerminator];
            }

            // �������� ��� ������� ���������� � 0
            int counter = 0;
            for (int prodIndex = 0; prodIndex < m_grammar.Length; prodIndex++)
            {
                Production production = m_grammar.GetProductionAt(prodIndex);

                // prodIDs[prodIndex][symIndex] = counter++;
                // ������ �������������� ���������� ������ ��������� ������
                for (int altProdIndex = prodIndex; altProdIndex < m_grammar.Length; altProdIndex++)
                {
                    bool currProdIsAlternative = m_grammar.GetProductionAt(altProdIndex).Head.Equals(production.Head);
                    bool currProdAlreadyNumerated = (prodIDs[altProdIndex][0] != 0);
                    if (!currProdIsAlternative || currProdAlreadyNumerated)
                        break;

                    prodIDs[altProdIndex][0] = counter++;
                }
                // ��������� ������ ���������
                for (int symIndex = 1; symIndex < production.LengthWithoutTerminator; symIndex++)
                {
                    prodIDs[prodIndex][symIndex] = counter++;
                }
            }

            m_table = new TableRow[counter];
        }
    }
}