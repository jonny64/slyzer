using System;

namespace LL1AnalyzerTool
{
    //строка таблицы разбора
    public struct TableRow
    {
        public bool accept;
        public bool error;
        public string errorMsg;
        public int jump;
        public bool stack;
        public char[] terminals;
    }

    //строит таблицу разбора дл€ заданной грамматики
    internal class GrammarTableBuilder : GrammarAnalyzer
    {
        //нумера всех символов грамматики
        private readonly int[][] prodIDs;

        public GrammarTableBuilder(string[] productions) : base(productions)
        {
            prodIDs = new int[m_grammar.Length][];
            for (int prodIndex = 0; prodIndex < m_grammar.Length; prodIndex++)
            {
                string production = m_grammar[prodIndex];
                prodIDs[prodIndex] = new int[production.Length];
            }
        }

        //получить таблицу разбора
        public TableRow[] GetParsingTable()
        {
            TableRow[] parsTable = new TableRow[GetGrammarSize()];
            NumerateProductions();

            int currProdID;
            for (int prodIndex = 0; prodIndex < m_grammar.Length; prodIndex++)
            {
                currProdID = prodIDs[prodIndex][0];
                string production = m_grammar[prodIndex];
                char head = production[0];

                //не последн€€ альтернатива -> error:=false;
                bool LastAlternative = (prodIndex == m_grammar.Length - 1) ||
                                       (m_grammar[prodIndex + 1][0] != head);
                parsTable[currProdID].error = LastAlternative;

                //заполн€ем строку дл€ головы продукции
                parsTable[currProdID].terminals = GetDirectSymbols(production);
                parsTable[currProdID].jump = prodIDs[prodIndex][1];

                string rightPart = production.Substring(1);

                //если это eps - права€ часть eps продукции
                if (rightPart == EPSILON_CHAR.ToString())
                {
                    currProdID = prodIDs[prodIndex][1];
                    parsTable[currProdID].terminals = GetDirectSymbols(production);
                    parsTable[currProdID].jump = -1;
                    parsTable[currProdID].error = true;
                }
                else
                    //заполн€ем строку дл€ правой части продукции
                    FillTableForRightPart(ref parsTable, prodIndex, production);
            }
            return parsTable;
        }

        private void FillTableForRightPart(ref TableRow[] parsTable, int prodIndex, string production)
        {
            for (int symIndex = 1; symIndex < production.Length; symIndex++)
            {
                int currProdID = prodIDs[prodIndex][symIndex];
                char sym = production[symIndex];
                parsTable[currProdID].error = true;
                //терминал в правой части
                if (Terminal(sym))
                {
                    char[] term = {sym};
                    parsTable[currProdID].terminals = term;
                    parsTable[currProdID].accept = true;
                    // крайний правый терминал: return=true
                    if (symIndex == production.Length - 1)
                        parsTable[currProdID].jump = -1;
                    else
                        parsTable[currProdID].jump = prodIDs[prodIndex][symIndex + 1];
                }
                else
                {
                    //нетерминал в правой части
                    parsTable[currProdID].terminals = GetDSUnionForHeadNonTerm(sym);
                    if (symIndex < production.Length - 1)
                        parsTable[currProdID].stack = true;
                    parsTable[currProdID].jump = GetFirstAltProdID(sym);
                }
            }
        }

        //сколько всего не уникальных символов в грамматике
        private uint GetGrammarSize()
        {
            uint sz = 0;
            for (int prodIndex = 0; prodIndex < m_grammar.Length; prodIndex++)
            {
                sz += (uint) m_grammar[prodIndex].Length;
            }
            return sz;
        }

        private int GetFirstAltProdID(char sym)
        {
            for (int prodIndex = 0; prodIndex < m_grammar.Length; prodIndex++)
            {
                char head = m_grammar[prodIndex][0];
                if (head == sym) return prodIDs[prodIndex][0];
            }
            throw new Exception("Ќет продукции, описывающей символ '" + sym + "'");
        }

        //объединение направл€ющих символов всех продукций с этим нетерминалов в левой части
        private char[] GetDSUnionForHeadNonTerm(char sym)
        {
            char[] result = {};
            for (int prodIndex = 0; prodIndex < m_grammar.Length; prodIndex++)
                if (m_grammar[prodIndex][0] == sym)
                    result =
                        Union(result, GetDirectSymbols(m_grammar[prodIndex]));
            return result;
        }

        private void NumerateProductions()
        {
            //нумеруем все символы грамматики
            int counter = 0;
            for (int prodIndex = 0; prodIndex < m_grammar.Length; prodIndex++)
            {
                string production = m_grammar[prodIndex];
                char head = production[0];

                //prodIDs[prodIndex][symIndex] = counter++;
                //альтернативные должны следовать подр€д
                for (int altProdIndex = prodIndex; altProdIndex < m_grammar.Length; altProdIndex++)
                {
                    if (m_grammar[altProdIndex][0] != head) break;
                    if (prodIDs[altProdIndex][0] == 0)
                        prodIDs[altProdIndex][0] = counter++;
                }
                for (int symIndex = 1; symIndex < production.Length; symIndex++)
                {
                    prodIDs[prodIndex][symIndex] = counter++;
                }
            }
        }
    }
}