using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace lab
{

    class IdentifierTable
    {
        private const int MAX_IDENT_TABLE_SIZE = 1024;
        Hashtable m_identTable = new Hashtable(MAX_IDENT_TABLE_SIZE);
        int id = 0;
        //MyHashtable identHashtable = new MyHashtable();
        
        public int Add(string name)
        {
            if (!Lookup(name))
            {
                m_identTable[name] = id;
            }
            return id++;
        }

        bool Lookup(string name)
        {
            return m_identTable.Contains(name);
        }
    }

    class NumericConstantTable
    {
        private const int MAX_NUM_CONST_TABLE_SIZE = 1024;
        Hashtable m_constTable = new Hashtable(MAX_NUM_CONST_TABLE_SIZE);
        int id = 0;
        //MyHashtable numConstHashtable = new MyHashtable();

        public int Add(string constant)
        {
            if (!Lookup(constant))
            {
                m_constTable[constant] = id;
            }
            return id++;
        }

        bool Lookup(string constant)
        {
            return m_constTable.Contains(constant);
        }
    }

    // token
    struct Token
    {
        int lineno;

        public AnalysisStage.TokenType type;
        public int attribute;
        public Token(int _pos, AnalysisStage.TokenType _type, int _attribute)
        {
            lineno = _pos; type = _type; attribute = _attribute;
        }
        public Token(AnalysisStage.TokenType _type)
        {
            type = _type;
            lineno = -1;
            attribute = -1;
        }
    }

    //здесь размещены "глобальные данные", разделяемые всеми частями компилятора
    abstract class AnalysisStage
    {
        //таблица идентификаторов
        protected IdentifierTable m_identTable = new IdentifierTable();
        //таблица констант
        protected NumericConstantTable m_constTable = new NumericConstantTable();
        //таблица ключевых слов
        protected static string[] m_keywords ={"and","begin","boolean","char","else",
                                "end","if","integer","not","or","program",
                                "real","record","then","type","var"};

        protected static TokenType[] m_keywords_enum =
        {
            TokenType.MULT_OP,TokenType.BEGIN,TokenType.BOOLEAN,
            TokenType.CHAR,TokenType.ELSE,TokenType.END,
            TokenType.IF,TokenType.INTEGER,TokenType.NOT,
            TokenType.ADD_OP,TokenType.PROGRAM,TokenType.REAL,
            TokenType.RECORD,TokenType.THEN,TokenType.TYPE,TokenType.VAR
        };
        protected bool Keyword(string name)
        {
            int kwIndex = Array.BinarySearch(m_keywords, name);
            return (kwIndex > -1);
        }

        protected TokenType GetKeywordType(string name)
        {
            int kwIndex = Array.BinarySearch(m_keywords, name);
            return m_keywords_enum[kwIndex];
        }

        public enum TokenType
        {
            TERMINATOR, MULT_OP, ADD_OP, IDENTIFIER, KEYWORD, NUMBER,
            EQUALITY_OP, EQUALITY,  COMMA, SEMICOLON, LEFT_PARENTHESIS,
            RIGHT_PARENTHESIS, POINT, TWO_POINTS, ASSIGN, COLON, UNKNOWN,
            AND, BEGIN, BOOLEAN, CHAR, ELSE,
            END, IF, INTEGER, NOT, OR, PROGRAM,
            REAL, RECORD, THEN, TYPE, VAR
        };

        internal static string[] GetKeyWords()
        {
            return m_keywords;
        }

        internal string GetIdentTable()
        {
            return m_identTable.ToString();
        }
    }

    //// вспомогательная хеш-таблица хранит индексы в таблице идентификаторов
    //public class MyHashtable
    //{
    //    const int HASH_TABLE_SIZE = 7;
    //    private LinkedList<int>[] m_hashTable = new LinkedList<int>[HASH_TABLE_SIZE];
    //    //доступ к таблице через перегруженный оператор индексирования
    //    public int this[string key]
    //    {
    //        get
    //        {
    //            try
    //            {
    //                LinkedList<int> candidates = GetCandidatesList(key);
    //                if (candidates == null) return -1;
    //                LinkedListNode<int> candidate = candidates.First;
    //                while (candidate != null)
    //                {
    //                    if (m_identTable[candidate.Value].name == key)
    //                    {
    //                        return candidate.Value;
    //                    }
    //                    candidate = candidate.Next;
    //                }
    //            }
    //            catch (Exception e)
    //            {
    //                return -1;
    //            }
    //            //не нашли
    //            return -1;
    //        }
    //        set
    //        {
    //            LinkedList<int> candidates = GetCandidatesList(key);
    //            if (candidates == null)
    //                candidates = new LinkedList<int>();
    //            LinkedListNode<int> candidate = candidates.First;
    //            if (candidate == null)
    //            {
    //                AddToIdentTable(value, key, candidates);
    //            }
    //            else
    //            {
    //                try
    //                {
    //                    // TODO линейный поиск замнить чем-то другим
    //                    while (candidate != null)
    //                    {
    //                        if (m_identTable[candidate.Value].name == key)
    //                            return;//уже есть в таблице
    //                        candidate = candidate.Next;
    //                    }
    //                }
    //                catch (Exception e)
    //                {
    //                }
    //                //не нашли - придется добавить
    //                AddToIdentTable(value, key, candidates);
    //            }
    //        }
    //    }
    //    //список кандидатов с одинаковым значением хеш-функции
    //    private LinkedList<int> GetCandidatesList(string key)
    //    {
    //        int hash = key.GetHashCode();
    //        int candidatesListID = System.Math.Abs(hash) % HASH_TABLE_SIZE;
    //        LinkedList<int> candidates = m_hashTable[candidatesListID];
    //        return candidates;
    //    }
    //    //добавляем переданное целое значение соответствующее ключу key в хеш-таблицу
    //    private void AddToIdentTable(int value, string key, LinkedList<int> candidates)
    //    {
    //        int hash = key.GetHashCode();
    //        int candidatesListID = System.Math.Abs(hash) % HASH_TABLE_SIZE;
    //        candidates.AddFirst(value);
    //        m_hashTable[candidatesListID] = candidates;
    //    }
    //}
}
