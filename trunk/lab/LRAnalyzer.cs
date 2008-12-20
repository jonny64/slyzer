using System;
using System.Collections.Generic;
using System.Text;

namespace lab
{
    class LRAnalyzer
    {
        //состояние
        private int m_state=0;
        private Stack<int> m_stateStack = new Stack<int>();
        private Stack<char> m_symStack = new Stack<char>();
        private string m_exp="";
        //лексический анализатор - "подносчик патронов"
        private Lexan m_lexan;
        //длины продукций(требуются при приведении)
        private string[] m_grammar = 
            { "SrL", "LET", "TsET", "Te", "XcY","XoiX", "EiX", "Yn", "Yb", "Ya", "Yh"};
        private char[][] parsTable ={
            new char[]{   'E',     'E',     'E',     'E',     'E',     'E',     'S',    'E',
                          'E',     'E',     'E',     'E',     'E',     'E',     'E',     'E', 'E' }, 
            new char[]{   'E',   'S',   'S',     'E',     'E',     'E',     'E',    'E',
                          'E',     'E',     'E',   'S',     'E',     'E',     'E',     'E', 'E' },
            new char[]{ 'E',   'E',    'E',   'E',    'E',    'E',     'E',  'E',
                        'E',   'E',    'E',   'E',    'E',    'E',     'E',  'E', 'S' },
            new char[]{ 'E',     'E',     'E',   'S',     'E',     'E',     'E',  'S',
                        'S',     'E',     'E',     'E',     'E',     'E',     'E',     'E', 'E' },
            new char[]{ 'R',   'R',    'R',   'R',    'R',    'R',     'R',  'R',
                        'R',   'R',    'R',   'R',    'R',    'R',     'R',  'R', 'R' },
            new char[]{   'E',     'E',   'S',     'E',     'E',     'E',     'E',    'E',
                          'E',     'E',     'E',   'S',     'E',     'E',     'E',     'E', 'E' },
            new char[]{ 'E',   'E',    'E',   'S',    'E',    'E',     'E',  'S',
                        'S',   'E',    'E',   'E',    'E',    'E',     'E',  'E', 'E' },
            new char[]{ 'E',     'E',     'E',     'E',   'S',     'E',     'E',    'E',
                        'E',  'S',  'S',     'E',     'E',     'E',     'E',     'E', 'E' },
            new char[]{ 'R',   'R',    'R',   'R',    'R',    'R',  'R',  'R',
                        'R',   'R',    'R',   'R',    'R',    'R',  'R',  'R', 'R' },
           new char[]{   'E',     'E',     'E',     'E',     'E',     'E',     'E',    'E',
                         'E',     'E',     'E',  'S',     'E',     'E',     'E',     'E', 'E' },
            new char[]{   'E',     'E',     'E',     'E',  'S',     'E',     'E',    'E',
                          'E',  'S',  'S',     'E',     'E',     'E',     'E',     'E', 'E' },
            new char[]{ 'R',   'R',    'R',   'R',    'R',    'R',     'R',  'R',
                        'R',   'R',    'R',   'R',    'R',    'R',     'R',  'R', 'R' },
            new char[]{   'E',     'E',     'E',     'E',     'E',     'S',     'E',    'E',
                          'E',     'E',     'E',     'E',  'S',  'S',  'S', 'S', 'E' },
            new char[]{ 'R',   'R',    'R',   'R',    'R',    'R',     'R',  'R',
                        'R',   'R',    'R',   'R',    'R',    'R',     'R',  'R', 'R' },
            new char[]{ 'R',   'R',    'R',   'R',    'R',    'R',     'R',  'R',
                        'R',   'R',    'R',   'R',    'R',    'R',     'R',  'R', 'R' },
            new char[]{ 'R',   'R',    'R',   'R',    'R',    'R',     'R',  'R',
                        'R',   'R',    'R',   'R',    'R',    'R',     'R',  'R', 'R' },
            new char[]{ 'R',   'R',    'R',   'R',    'R',    'R',     'R',  'R',
                        'R',   'R',    'R',   'R',    'R',    'R',     'R',  'R', 'R' },
            new char[]{ 'R',   'R',    'R',   'R',    'R',    'R',     'R',  'R',
                        'R',   'R',    'R',   'R',    'R',    'R',     'R',  'R', 'R' },
            new char[]{ 'R',   'R',    'R',   'R',    'R',    'R',     'R',  'R',
                        'R',   'R',    'R',   'R',    'R',    'R',     'R',  'R', 'R' },
            new char[]{ 'R',   'R',    'R',   'R',    'R',    'R',     'R',  'R',
                        'R',   'R',    'R',   'R',    'R',    'R',     'R',  'R', 'R' },
            new char[]{ '!',   '!',    '!',   '!',    '!',    '!',     '!',  '!',
                        '!',   '!',    '!',   '!',    '!',    '!',     '!',  '!', '!' }
        };

        private int[][] numTable ={
            new int[]{    0,      0,      0,      0,      0,      0,       2 ,     0,
                          0,      0,      0,      0,      0,      0,      0,      0, 0 }, 
            new int[]{    0,     3 ,     4 ,      0,      0,      0,      0,     0,
                          0,      0,      0,     8 ,      0,      0,      0,      0, 0 },
            new int[]{   0 ,     0 ,      0 ,     0 ,      0 ,      0 ,       0 ,    0 ,
                         0 ,     0 ,      0 ,     0 ,      0 ,      0 ,       0 ,    0 , 21 },
            new int[]{    0,      0,      0,     5 ,      0,      0,      0,    6 ,
                         19,      0,      0,      0,      0,      0,      0,      0, 0 },
            new int[]{   2 ,     2 ,      2 ,     2 ,      2 ,      2 ,       2 ,    2 ,
                         2 ,     2 ,      2 ,     2 ,      2 ,      2 ,       2 ,    2 , 2 },
            new int[]{    0,      0,     7 ,      0,      0,      0,      0,     0,
                          0,      0,      0,     8 ,      0,      0,      0,      0, 0 },
            new int[]{   0 ,     0 ,      0 ,     20 ,      0 ,      0 ,       0 ,    6 ,
                         19 ,     0 ,      0 ,     0 ,      0 ,      0 ,       0 ,    0 , 0 },
            new int[]{    0,      0,      0,      0,     9 ,      0,      0,     0,
                          0,    13 ,    10 ,      0,      0,      0,      0,      0, 0 },
            new int[]{   7 ,     7 ,      7 ,     7 ,      7 ,      7 ,    7 ,    7 ,
                         7 ,     7 ,      7 ,     7 ,      7 ,      7 ,    7 ,    7 , 7 },
            new int[]{    0,      0,      0,      0,      0,      0,      0,     0,
                          0,      0,      0,    11 ,      0,      0,      0,      0, 0 },
            new int[]{    0,      0,      0,      0,    12 ,      0,      0,     0,
                          0,    13 ,    10 ,      0,      0,      0,      0,      0, 0 },
            new int[]{   6 ,     6 ,      6 ,     6 ,      6 ,      6 ,       6 ,    6 ,
                         6 ,     6 ,      6 ,     6 ,      6 ,      6 ,       6 ,    6 , 6 },
            new int[]{    0,      0,      0,      0,      0,      18,      0,     0,
                          0,      0,      0,      0,    14 ,    15 ,    16 ,   17 , 0 },
            new int[]{   8 ,     8 ,      8 ,     8 ,      8 ,      8 ,       8 ,    8 ,
                         8 ,     8 ,      8 ,     8 ,      8 ,      8 ,       8 ,    8 , 8 },
            new int[]{   9 ,     9 ,      9 ,     9 ,      9 ,      9 ,       9 ,    9 ,
                         9 ,     9 ,      9 ,     9 ,      9 ,      9 ,       9 ,    9 , 9 },
            new int[]{   10 ,     10 ,      10 ,     10 ,      10 ,      10 ,       10 ,    10 ,
                         10 ,     10 ,      10 ,     10 ,      10 ,      10 ,       10 ,    10 , 10 },
            new int[]{   11 ,     11 ,      11 ,     11 ,      11 ,      11 ,       11 ,    11 ,
                         11 ,     11 ,      11 ,     11 ,      11 ,      11 ,       11 ,    11 , 11 },
            new int[]{   5 ,     5 ,      5 ,     5 ,      5 ,      5 ,       5 ,    5 ,
                         5 ,     5 ,      5 ,     5 ,      5 ,      5 ,       5 ,    5 , 5 },
            new int[]{   4 ,     4 ,      4 ,     4 ,      4 ,      4 ,       4 ,    4 ,
                         4 ,     4 ,      4 ,     4 ,      4 ,      4 ,       4 ,    4 , 4 },
            new int[]{   3 ,     3 ,      3 ,     3 ,      3 ,      3 ,       3 ,    3 ,
                         3 ,     3 ,      3 ,     3 ,      3 ,      3 ,       3 ,    3 ,  3 },
            new int[]{   1 ,     1 ,      1 ,     1 ,      1 ,      1 ,       1 ,    1 ,
                         1 ,     1 ,      1 ,     1 ,      1 ,      1 ,       1 ,    1 ,  1 }

        };

        public string m_errMsg="";
        public LRAnalyzer(string _exp)
        {
            m_exp = _exp;
            //создаем лексический анализатор, на вход подаем текст
            m_lexan = new Lexan(_exp);
        }

        //проверка
        public bool Check()
        {
            m_stateStack.Push(0);//начальное состояние

            char inputSym = GetCharForTokenType(m_lexan.GetToken().type);
            int col = GetSymColumn(inputSym);

            //анализатор находится в состоянии,хранящемс в вершине стека состояний
            m_state = m_stateStack.Peek();
            while ((parsTable[m_state][col] != '!'))
            {
                switch (parsTable[m_state][col])
                {
                        //синтаксическая ошибка
                    case 'E':
                        {
                            m_errMsg = GetErrMsg(m_state, inputSym);
                            return false;
                        }; break;
                        //сдвиг
                    case 'S':
                        {
                            m_symStack.Push(inputSym);
                            int newState = numTable[m_state][col]-1;
                            m_stateStack.Push(newState);

                            if (parsTable[newState][0]!='R')
                                inputSym = GetCharForTokenType(m_lexan.GetToken().type);
                            col = GetSymColumn(inputSym);
                        } break;
                        //приведение
                    case 'R':
                        {
                            //номер продукции, по которой выполняется приведение
                            int productionID=numTable[m_state][col] - 1;
                            for (int i = 0; i < m_grammar[productionID].Length - 1; i++)
                            {
                                m_symStack.Pop();
                                m_stateStack.Pop();
                            }
                            inputSym=m_grammar[productionID][0];//голова продукции
                            col = GetSymColumn(inputSym);
                        }; break;
                }//switch
                //анализатор находится в состоянии,хранящемс в вершине стека состояний
                m_state = m_stateStack.Peek();
            }//while
            return true;
        }


        //выбрасывает сообщение об ошибке, произошедшей при разборе в состоянии i
        private string GetErrMsg(int errRow, char errSym)
        {
            string errMsg = "имеем " + SymToString(errSym) +
                " ожидается ";
            for (int col = 0; col < parsTable[0].Length; col++)
            {
                char sym=parsTable[errRow][col];
                if ((sym != 'E') &&
                    Terminal(GetColumnSym(col)))
                    errMsg += "" + SymToString(GetColumnSym(col));
            }
            return errMsg;
        }

        private int GetSymColumn(char sym)
        {
            char[] syms ={ 'S', 'L', 'E', 'T', 'X', 'Y', 'r', 's', 'e', 'c', 'o', 'i', 'n', 'b', 'a', 'h', 't' };
            for (int i = 0; i < syms.Length; i++)
            {
                if (syms[i] == sym) return i;
            }
            throw new Exception("Не могу сопоставить");
        }

        private char GetColumnSym(int col)
        {
            char[] syms ={ 'S', 'L', 'E', 'T', 'X', 'Y', 'r', 's', 'e', 'c', 'o', 'i', 'n', 'b', 'a', 'h', 't' };
            return syms[col];
        }

        private bool Terminal(char sym)
        {
            if (char.IsLower(sym)) return true;
            else return false;
        }

        private string SymToString(char p)
        {
            char[] syms ={ 'r', 'e', 's', 'c', 'i', 'o', 'n', 'a', 'b', 'h', 't' };
            string[] eqTypes ={ "record", "end",";",":","<идентификатор>",
                ",","integer", "real","boolean", "char","<конец>"};
            // TODO возможно, избавиться от линейного поиска
            //return syms[Array.BinarySearch(eqTypes, tokentype)];
            for (int i = 0; i < eqTypes.Length; i++)
            {
                if (syms[i] == p) return eqTypes[i];
            }
            throw new Exception("Не могу преобраовать char в текст");
        }

        private char GetCharForTokenType(AlalysisStage.TokenTypes tokentype)
        {
            char[] syms ={ 'r', 'e', 's', 'c', 'i', 'o', 'n', 'a', 'b', 'h', 't' };
            AlalysisStage.TokenTypes[] eqTypes ={ AlalysisStage.TokenTypes.RECORD, AlalysisStage.TokenTypes.END,
                                                AlalysisStage.TokenTypes.SEMICOLUMN, AlalysisStage.TokenTypes.COLON,
                                                AlalysisStage.TokenTypes.IDENT, AlalysisStage.TokenTypes.COMMA,
                                                AlalysisStage.TokenTypes.INTEGER, AlalysisStage.TokenTypes.REAL,
                                                AlalysisStage.TokenTypes.BOOLEAN, AlalysisStage.TokenTypes.CHAR,
                                                AlalysisStage.TokenTypes.TERMINATOR};
            // TODO возможно, избавиться от линейного поиска
            //return syms[Array.BinarySearch(eqTypes, tokentype)];
            for (int i = 0; i < eqTypes.Length; i++)
            {
                if (eqTypes[i] == tokentype) 
                    return syms[i];
            }

            throw new Exception("Не могу преобраовать токен в char");
        }


    }
}
