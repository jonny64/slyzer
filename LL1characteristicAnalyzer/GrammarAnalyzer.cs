/*Copyleft (l) 2008 Груздев М.
 * Анализатор свойств КС грамматик
 * на вход в конструктор  - массив строк - продукций , 
 * альтернативные продукции должны быть разделены
 * в каждой продукции исключены все разделители
 */
using System;
using System.Collections;
using System.Collections.Generic;

namespace LL1AnalyzerTool
{
    internal class GrammarAnalyzer
    {
        public char EPSILON_CHAR = '#';
        //содержит empty статус каждого нетерминала
        public Hashtable m_empty = new Hashtable();
        //вспомогательные матрицы отношений first,follow
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

            //опрдеделяем empty стаус нетерминалов
            //заполняем таблицу отношения m_first
            //заполняем таблицу отношения m_follow
            FillEmptySymHashtable();
            FillFirstRelationTable();
            FillFollowRelationTable();
        }

        //тип нетерминала (eps-образующий или нет)

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

        //является ли строка eps-образующей
        public bool Empty(string alpha)
        {
            bool generateEps = true;
            //строка не eps-образующая если хотя бы 1 ее 
            //комнпонент не eps-образующий
            for (int i = 0; i < alpha.Length; i++)
                if (EmptySym(alpha[i]) == EmptyState.IS_NON_EMPTY)
                {
                    generateEps = false;
                    break;
                }
            return generateEps;
        }

        //вычисляет множество First для строки alpha
        public char[] First(string alpha)
        {
            char[] first = {};
            //множество First получением объединением
            //множеств first для каждого символа
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

            //вычисление отношения начинается_прямо_с
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
            //вычисление рефлексивноого замыкания отношения начинается_прямо_с
            char[] grammarSyms = GetGrammarSymbols();
            for (int symIndex = 0; symIndex < grammarSyms.Length; symIndex++)
            {
                char sym = grammarSyms[symIndex];
                m_first[GetSymIndex(sym), GetSymIndex(sym)] = true;
            }
            //вычисление транзитивного замыкания отношения начинается_прямо_с
            m_first = Transitive(m_first);
        }

        //вычисляет транзитивное замыкание матрицы с помощью алгоритма Уоршелла
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

        //объединяет пару множеств
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

        //добавляет элемент в множество
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

        //вычисляет множество First для одиночного символа
        public char[] First(char sym)
        {
            char[] result = {};
            char[] grammarSyms = GetGrammarSymbols();
            for (int colIndex = 0; colIndex < grammarSyms.Length; colIndex++)
            {
                //есть отношение и это терминал - добавляем в множество
                bool terminal = Terminal(m_grammarSyms[colIndex]);
                if ((m_first[GetSymIndex(sym), colIndex]) && terminal)
                {
                    result = Union(result, grammarSyms[colIndex]);
                }
            }
            return result;
        }

        // TODO все оставшиеся char.IsLower в коде заменить на этот метод
        //классифицирует символ как терминал
        public bool Terminal(char sym)
        {
            if (char.IsLower(sym)) return true;
            else return false;
        }

        private int GetSymIndex(char sym)
        {
            return Array.BinarySearch(m_grammarSyms, sym);
        }

        // TODO вызовы этого метода по всему коду заменить обращениями к члену 
        // класса m_grammarSyms. GetGrammarSymbols().Length поменять на m_symCount
        // метод нужен только для нициализации m_grammarSyms
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

        //является ли терминал/нетерминал eps-образующим
        private EmptyState EmptySym(char sym)
        {
            if (char.IsLower(sym)) return EmptyState.IS_NON_EMPTY; //если терминал
            else //иначе смотрим в таблице
            {
                object state = m_empty[sym];
                EmptyState[] equals = {EmptyState.IS_NON_EMPTY /*false*/, EmptyState.IS_EMPTY /*true*/};
                if (state != null)
                    return equals[Convert.ToByte((bool) state)];
                else return EmptyState.EMPTY_STATE_UNKNOWN;
            }
        }

        //выдает список всех нетерминалов грамматики
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

        //Copyleft (l) 2008 Груздев М
        //инициализирует таблицу empty нетерминалов

        private void FillFollowRelationTable()
        {
            //строим отношение прямо_перед
            bool[,] straightBefore = GetStraightBeforeRelation();
            //строим отношение прямо_на_конце
            bool[,] straightAtTheEnd = GetStraightAtTheEndRelation();

            //вычислим отношение на_конце как рефлексивно-транзитивное замыкание прямо_на_конце
            bool[,] atTheEnd = straightAtTheEnd;
            for (int symIndex = 0; symIndex < m_symCount; symIndex++)
            {
                char sym = m_grammarSyms[symIndex];
                atTheEnd[GetSymIndex(sym), GetSymIndex(sym)] = true;
            }
            atTheEnd = Transitive(atTheEnd);
            //вычислим отношение перед как произведение отношений
            //на_конце * прямо_перед * начинается_с
            m_follow = Multiply(atTheEnd, straightBefore);
            m_follow = Multiply(m_follow, m_first);
        }

        private bool[,] Multiply(bool[,] p, bool[,] q)
        {
            /*Пусть Р и Q — два бинарных отношения; тогда их произведе­ние PQ — 
             * результат операции умножения отношений — выполня­ется для х и у 
             * (т.е. высказывание xPQy оказывается истинным) тогда и только тогда,
             * когда в М существует элемент z такой, что верно как xPz, так и zQy.*/
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
                    //есть продукция B>alphaAbeta, где beta eps порождающая
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

        //Copyleft (l) 2008 Груздев М
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

        //выясняет (eps порождающий) статус нетерминалов
        private void FillEmptySymHashtable()
        {
            bool[] excluded = new bool[m_grammar.Length]; //признаки, что исключения продукции из рассмотрения

            //обязательно сначала отсечь eps-продукции
            DeleteEpsProductions(excluded);

            do
            {
                excluded = PerformBasicResearch(excluded);
                if (!FoundEachNonTerminalState()) PerformAdditionalResearch(excluded, m_grammar);
            } while (!FoundEachNonTerminalState());
        }

        //является ли нетерминал epsобразующим
        public bool Empty(char sym)
        {
            if (!Terminal(sym) && (m_empty[sym] != null))
                return (bool) m_empty[sym];
            else return false;
        }

        private void DeleteEpsProductions(bool[] excluded)
        {
            //обязательно сначала отсечь eps-продукции
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

                // TODO переделать следующий цикл во что-нибудь поизящнее

                // в правой части есть eps образующий нетерминал
                int EmptyNonTerminalIndex = GetEpsGenSymIndex(production);
                while (EmptyNonTerminalIndex > -1)
                {
                    production = production.Remove(EmptyNonTerminalIndex, 1);
                    //правая часть продукции пуста
                    if (production.Length == 1)
                    {
                        //исключаем ее и все продукции для данного нетерминала
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
            //ищем индекс символа правой части продукции, явл-ся eps образующим
            for (int termIndex = 1; termIndex < production.Length; termIndex++)
                if (EmptySym(production[termIndex]) == EmptyState.IS_EMPTY) return termIndex;
            return -1;
        }

        private bool HasNonEpsGenSymInRightPart(string production)
        {
            //если хотя бы 1 символ правой части продукции не eps образующий
            for (int i = 1; i < production.Length; i++)
                if (EmptySym(production[i]) == EmptyState.IS_NON_EMPTY) return true;
            return false;
        }

        private void DeleteAllTermProductions(char term, bool[] excluded, string[] m_grammar)
        {
            //исключаем из рассмотрения все продукции соответствующие нтерминалу
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

        //проверяет, для всех ли нетерминалов выяснен статус empty
        private bool FoundEachNonTerminalState()
        {
            char[] NonTerminals = GetNonTerminals();
            //выяснен ли empty статус каждого нетерминала?
            for (int i = 0; i < NonTerminals.Length; i++)
                if (m_empty[NonTerminals[i]] == null) return false;
            return true;
        }

        //множество follow для eps порождающих нетерминалов
        public char[] Follow(char NonTerm)
        {
            char[] result = {};
            //это множество терминалов a что sym перед a
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

//Copyleft (l) 2008 Груздев М