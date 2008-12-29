using System;
using System.Collections.Generic;
using System.Text;

namespace lab
{
    //строка таблицы идентификаторов
    public class Ident
    {
        public string name;
        public Ident(string _name) { name = _name; }
        //другие поля
    }

    //строка таблицы констант
    class NumConst
    {
        public string name;    //представление коннстанты строкой
        public NumConst(string _name) { name = _name; }
        //другие поля
    }


    //здесь размещены "глобальные данные", разделяемые всеми частями компилятора
    abstract class AnalysisStage
    {
        private const int MAX_IDENT_TABLE_SIZE = 1024;
        private const int MAX_NUM_CONST_TABLE_SIZE = 1024;
        //таблица идентификаторов
        //таблица констант
        //таблица ключевых слов
        protected static Ident[] m_identTable = new Ident[MAX_IDENT_TABLE_SIZE];
        protected static NumConst[] m_numConstTable = new NumConst[MAX_NUM_CONST_TABLE_SIZE];

        protected static string[] m_keywords ={"and","begin","boolean","char","else",
                                "end","if","integer","not","or","program",
                                "real","record","then","type","var"};
        public enum TokenTypes
        {
            TERMINATOR, MULT_OP, ADD_OP, IDENTIFIER, KEYWORD, NUMBER,
            EQUALITY_OP, COMMA, SEMICOLON, LEFT_PARENTHESIS,
            RIGHT_PARENTHESIS, POINT, TWO_POINTS, ASSIGN, COLON, UNKNOWN,
            AND, BEGIN, BOOLEAN, CHAR, ELSE,
            END, IF, INTEGER, NOT, OR, PROGRAM,
            REAL, RECORD, THEN, TYPE, VAR
        };
        protected static TokenTypes[] m_keywords_enum ={
            TokenTypes.AND,TokenTypes.BEGIN,TokenTypes.BOOLEAN,
            TokenTypes.CHAR,TokenTypes.ELSE,TokenTypes.END,
            TokenTypes.IF,TokenTypes.INTEGER,TokenTypes.NOT,
            TokenTypes.OR,TokenTypes.PROGRAM,TokenTypes.REAL,
            TokenTypes.RECORD,TokenTypes.THEN,TokenTypes.TYPE,TokenTypes.VAR};
        protected static int m_identTableSize = 0;
        protected static int m_numConstTableSize = 0;

        protected MyHashtable identHashtable = new MyHashtable();
        protected MyHashtable numConstHashtable = new MyHashtable();
        
        public static Ident[] GetIdentTable()
        {
            return m_identTable;
        }
        public static int GetNumConstTableSize()
        {
            return m_numConstTableSize;
        }

        public static int GetIdentTableSize()
        {
            return m_identTableSize;
        }
        protected int AddToIdentTable(string name)
        {
            m_identTable[m_identTableSize] = new Ident(name);
            m_identTableSize++;
            return m_identTableSize-1;
        }

        protected int AddToNumConstTable(string name)
        {
            m_numConstTable[m_numConstTableSize] = new NumConst(name);
            m_numConstTableSize++;
            return m_numConstTableSize-1;
        }
        // вспомогательная хеш-таблица хранит индексы в таблице идентификаторов
        protected class MyHashtable
        {
            const int HASH_TABLE_SIZE = 7;
            private LinkedList<int>[] m_hashTable = new LinkedList<int>[HASH_TABLE_SIZE];
            //доступ к таблице через перегруженный оператор индексирования
            public int this[string key]
            {
                get
                {
                    try
                    {
                        LinkedList<int> candidates = GetCandidatesList(key);
                        if (candidates == null) return -1;
                        LinkedListNode<int> candidate = candidates.First;
                        while (candidate != null)
                        {
                            if (m_identTable[candidate.Value].name == key)
                            {
                                return candidate.Value;
                            }
                            candidate = candidate.Next;
                        }
                    }
                    catch (Exception e)
                    {
                        return -1;
                    }
                    //не нашли
                    return -1;
                }
                set
                {
                    LinkedList<int> candidates = GetCandidatesList(key);
                    if (candidates == null)
                        candidates = new LinkedList<int>();
                    LinkedListNode<int> candidate = candidates.First;
                    if (candidate == null)
                    {
                        AddToIdentTable(value, key, candidates);
                    }
                    else
                    {
                        try
                        {
                            // TODO линейный поиск замнить чем-то другим
                            while (candidate != null)
                            {
                                if (m_identTable[candidate.Value].name == key)
                                    return;//уже есть в таблице
                                candidate = candidate.Next;
                            }
                        }
                        catch (Exception e)
                        {
                        }
                        //не нашли - придется добавить
                        AddToIdentTable(value, key, candidates);
                    }
                }
            }
            //список кандидатов с одинаковым значением хеш-функции
            private LinkedList<int> GetCandidatesList(string key)
            {
                int hash = key.GetHashCode();
                int candidatesListID = System.Math.Abs(hash) % HASH_TABLE_SIZE;
                LinkedList<int> candidates = m_hashTable[candidatesListID];
                return candidates;
            }
            //добавляем переданное целое значение соответствующее ключу key в хеш-таблицу
            private void AddToIdentTable(int value, string key, LinkedList<int> candidates)
            {
                int hash = key.GetHashCode();
                int candidatesListID = System.Math.Abs(hash) % HASH_TABLE_SIZE;
                candidates.AddFirst(value);
                m_hashTable[candidatesListID] = candidates;
            }
        }

        internal static NumConst[] GetNumConstTable()
        {
            return m_numConstTable;
        }

        internal static string[] GetKeyWords()
        {
            return m_keywords;
        }
    }
}
