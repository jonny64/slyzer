using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace lab
{
    //типы ошибок
    enum Error { UNKNOWN_SYMBOL};
    //тип токена

    //токен
    struct Token
    {
        //положение токена (пока только индексы во входном выражении
        //можно добавить имя файла и т.п.)
        public struct Position
        {
            public Position(int _beg, int _end) { beg = _beg; end = _end; }
            int beg;
            int end;
        };
        Position position;

        public AnalysisStage.TokenTypes type;        //тип токена
        public int attribute;       //атрибут
        public Token(Position _pos, AnalysisStage.TokenTypes _type, int _attribute)
        {
            position = _pos; type = _type; attribute = _attribute;
        }
    }

    class Lexan : lab.AnalysisStage
    {
        public string exp;  //ссылка на строку обрабатываемого выражения
        private int m_baseIndex;      //текущий индекс-база в выражении
        private int m_expansionIndex; //индекс продвижения
        private int m_currLine;       //текущая строка в выражениии(число встреченных в нем  '\n')
        //string token;   //текущая лексема
        //Types tokType;  //тип лексемы
        //список ошибок обнаруженный при анализе(например. незавершенная числ. константа)
        public List<string> errorMessages=new List<string>();
        //таблица идентификаторов
        //public IdentHashtable identTable = new IdentHashtable();
        //public Hashtable numConstTable = new Hashtable();
        //автомат, распознающий числовую константу
        class NumberDFA
        {
            int m_state=0;                                            //состояие
            
            public bool EndState = false;                             //автомат в конечном состоянии
            public bool error = false;
            public string errorMsg = "";

            //скармливает автомату входной символ, переволя его в новое состояние
            public void ReadSymbol(char sym)
            {
                switch(m_state)
                {
                    case 0: 
                        if (char.IsDigit(sym)) 
                            m_state = 1;
                        break;

                    case 1: 
                        if (sym == '.') 
                            m_state = 2; 
                        else
                            if ((sym == 'e')||(sym=='E')) 
                                m_state=4 ; 
                            else 
                                m_state = 7;          //конец
                        break;

                    case 2: 
                        if (char.IsDigit(sym)) 
                            m_state = 3;
                        else
                            GoToErrorState("после '.' ожидается продолжение константы");
                        break;

                    case 3: if ((sym == 'e')||(sym=='E')) 
                        m_state = 4; 
                        else 
                            if (!char.IsDigit(sym)) 
                                m_state = 7;
                        break;

                    case 4: 
                        if ((sym == '+') || (sym == '-'))
                            m_state = 5;
                        else
                            if (char.IsDigit(sym))
                                m_state = 6;
                            else 
                                GoToErrorState("ожидается +- или целое число");
                        break;

                    case 5: 
                        if (char.IsDigit(sym))
                            m_state = 6; 
                        else 
                            GoToErrorState("ожидается целое число");
                        break;

                    case 6: 
                        if (!char.IsDigit(sym)) 
                            m_state = 7;                  //конец
                        break;
                }

                if (m_state == 7) 
                    EndState = true;
            }
            //переводит автомат в конечное состояние с ошибкой
            void GoToErrorState(string msg)
            {
                m_state = 7;
                error = true;
                errorMsg = msg;
            }

        }
        class ComplexTerminalDFA
        {
            private int state = 0;
            //множество конечных состояний
            private int[] m_endStates = {-1,5,6,7,8,9,10,11,12,13};
            //таьлица переходов
            private int[,] m_transitionTable = {
                /*вход/ 0   1   2   3   4   5   6   7   8   9   10  11  12  13*/
                /*'.'*/{1,  5,  3,  10, 13  },
                /*':'*/{2,  6,  8,  10, 13  },
                /*'>'*/{3,  6,  8,  10, 12  },
                /*'<'*/{4,  6,  8,  10, 13  },
                /*'='*/{-1, 6,  7,  9,  11  },
             /*другой*/{-1, 6,  8,  10,  13  }
            };
            //скармливает автомату входной символ, переволя его в новое состояние
            
            public bool EndState = false;                             //автомат в конечном состоянии
            public TokenTypes acceptedStringType;
            public void ReadSymbol(char sym)
            {
                //сопоставляем символу строку в таблице переходов
                string punc =".:><=";
                int inpCharIndex = punc.IndexOf(sym);
                if (inpCharIndex < 0) inpCharIndex = 5;//'другой'

                state=m_transitionTable[inpCharIndex,state];
                
                //конечное
                if (Array.BinarySearch(m_endStates, state) > -1)
                {
                    EndState = true;
                    //соответствует состояниям 5-8
                    TokenTypes[] equalTypes ={ TokenTypes.TWO_POINTS, TokenTypes.POINT,
                                                TokenTypes.ASSIGN,TokenTypes.COLON};
                    if (state < 9) acceptedStringType = equalTypes[state-5];
                    else acceptedStringType = TokenTypes.EQUALITY_OP;
                }
            }
        }

        //конструктор с инициализацией входным выражением
        public Lexan(string expression)
        {
            exp = expression;
            Array.Sort(m_keywords);
        }

        //выделяет очередной токен
        public Token GetToken()
        {
            Token token = new Token();
            token.type = TokenTypes.TERMINATOR;
            
            //пропуск комментариев и разделителей (пробелы, табуляция, etc.)
            //токен для заполнения информационных полей передаем по ссылке ('указателю')
            while (SkipDelimiters() || SkipMyltiLineComms() || SkipComms()) ;

            if (m_expansionIndex < exp.Length)
            {
                //Распознование идентификатора
                if (TryCastAsIdent(ref token))
                { m_baseIndex = m_expansionIndex; return token; }

                //Распознавание числовой константы
                if (TryCastAsNumConst(ref token)) 
                { m_baseIndex = m_expansionIndex; return token; }

                //Распознавание односимвольных элементов языка ,;+-*/=()
                if (TryCastAsOneCharTerminal(ref token))
                { m_baseIndex = m_expansionIndex; return token; }

                //Распознавание 'сложных' терминалов языка <,>,>=,<=,<>,:,:=, . , ..
                //которые могут являться началом других
                if (TryCastAsComplexTerminal(ref token))
                { m_baseIndex = m_expansionIndex; return token; }

                //оставшийся вариант: терминал не принадлежит к языку
                token.type = TokenTypes.UNKNOWN;
            } 
            return token;
        }

        private bool TryCastAsComplexTerminal(ref Token token)
        {
            //с чего начинается 'сложные' терминалы
            string punc = ".:><";
            if (punc.IndexOf(exp[m_expansionIndex]) > -1)
            {
                TokenTypes tokenType=TokenTypes.UNKNOWN;
                int tokenLength = 1;
                switch (exp[m_expansionIndex])
                {
                    case '.': if (exp[m_expansionIndex + 1] == '.')
                        {
                            tokenType = TokenTypes.TWO_POINTS;
                            tokenLength = 2;
                        }
                        else tokenType = TokenTypes.POINT;
                        break;
                    case ':': if (exp[m_expansionIndex + 1] == '=')
                        {
                            tokenType = TokenTypes.ASSIGN;
                            tokenLength = 2;
                        }
                        else tokenType = TokenTypes.COLON;
                        break;
                    case '>': if (exp[m_expansionIndex + 1] == '=') tokenLength = 2;
                        tokenType = TokenTypes.EQUALITY_OP;
                        break;
                    case '<': if ((exp[m_expansionIndex + 1] == '=') || (exp[m_expansionIndex + 1] == '>'))
                            tokenLength = 2;
                        tokenType = TokenTypes.EQUALITY_OP;
                        break;

                }
                //сформируем cоттветствующий токен (атрибута нет - поэтому -1)
                string term = exp.Substring(m_baseIndex, tokenLength);              
                token = new Token(new Token.Position(m_baseIndex, m_expansionIndex), 
                    tokenType, -1);
                m_expansionIndex += tokenLength;
                return true;
            }
            return false;
        }

        private bool TryCastAsOneCharTerminal(ref Token token)
        {
            string punc = ",;+-*/=()";
            TokenTypes[] equalTypes ={ TokenTypes.COMMA, TokenTypes.SEMICOLON,
                                        TokenTypes.ADD_OP,TokenTypes.ADD_OP,
                                        TokenTypes.MULT_OP,TokenTypes.MULT_OP,
                                        TokenTypes.EQUALITY_OP,TokenTypes.LEFT_PARENTHESIS,
                                        TokenTypes.RIGHT_PARENTHESIS};
            //проверяем является ли символ 'пунктуационным'
            int index = punc.IndexOf(exp[m_expansionIndex]);
            if (index > -1)//если да
            {
                m_expansionIndex++;
                //возвращаем соответствующий токен
                token = new Token(new Token.Position(m_baseIndex, m_expansionIndex), equalTypes[index],
                            -1);
                return true;
            }
            return false;
        }
        //распознаем числовую константу
        private bool TryCastAsNumConst(ref Token token)
        {
            //числовая константа начинается с цифры
            if (Char.IsDigit(exp[m_baseIndex]))
            {
                //используем КА
                NumberDFA numberDFA = new NumberDFA();
                m_expansionIndex--;
                do
                {
                    m_expansionIndex++;
                    if (m_expansionIndex < exp.Length)
                        numberDFA.ReadSymbol(exp[m_expansionIndex]);
                    else
                        //eof
                        numberDFA.ReadSymbol('#');
                }
                while (!numberDFA.EndState);         //пока КА не в конечном сстоянии

                string foundNumConstAsString = exp.Substring(m_baseIndex, m_expansionIndex - m_baseIndex);
                //неоконченная константа - ошибку в список ошибок и продолжаем
                if (numberDFA.error) AddWarningMessage(numberDFA.errorMsg);
                //'хорошие' новые константы  - в таблицу констант
                else
                {
                    int constId = numConstHashtable[foundNumConstAsString];
                    if (constId == -1)
                    {
                        constId = AddToNumConstTable(foundNumConstAsString);
                        numConstHashtable[foundNumConstAsString] = constId;
                    }
                    token = new Token(new Token.Position(m_baseIndex, m_expansionIndex), TokenTypes.NUMBER,
                            constId);
                }
                
                return true;
            }
            return false;
        }

        private bool TryCastAsIdent(ref Token token)
        {
            //идентификатор (начинается с буквы или символа '_')
            if (Char.IsLetter(exp[m_expansionIndex]) || exp[m_expansionIndex] == '_')
            {
                m_baseIndex = m_expansionIndex;
                //потом идет буква или цифра или '_'
                do
                    ++m_expansionIndex;
                while (m_expansionIndex != exp.Length &&
                    (Char.IsLetterOrDigit(exp[m_expansionIndex]) || exp[m_expansionIndex] == '_'));
                
                //распознан идентификатор - подстрока: name
                string name = exp.Substring(m_baseIndex, m_expansionIndex - m_baseIndex);

                //распознанный идентификатор может быть ключевое словом
                int kwIndex = Array.BinarySearch(lab.AnalysisStage.m_keywords, name);
                if (kwIndex >-1)
                    token = new Token(new Token.Position(m_baseIndex, m_expansionIndex), m_keywords_enum[kwIndex], -1);
                else
                {
                    int identID = identHashtable[name];
                    //если впервые встречен - в таблицу идентификаторов
                    if (identID == -1)
                    {
                        identID=AddToIdentTable(name);
                        identHashtable[name]=identID;
                    }
                    token = new Token(new Token.Position(m_baseIndex, m_expansionIndex), 
                        TokenTypes.IDENTIFIER, identID);
                }
                   return true;
            }
            return false;
        }


        private void AddWarningMessage(string msg)
        {
            errorMessages.Add("Строка " + m_currLine + ": " + msg);
        }
        
        //пропуск {многострочных комментариев}
        private bool SkipMyltiLineComms()
        {
            if (m_expansionIndex > exp.Length - 1) return false;
            if (exp[m_expansionIndex] == '{')
            {
                while (m_expansionIndex < exp.Length &&
                       (exp[m_expansionIndex] != '}'))
                {
                    if (exp[m_expansionIndex] == '\n') ++m_currLine;
                    ++m_expansionIndex;
                }
                m_baseIndex = ++m_expansionIndex;
                return true;
            }
            return false;
        }
        //пропуск //однострочных комментариев}
        private bool SkipComms()
        {
            if (m_expansionIndex > exp.Length - 1) return false; 
            if ((exp[m_expansionIndex] == '/') && (m_expansionIndex < exp.Length - 1) && (exp[m_expansionIndex++] == '/'))
            {
                while (m_expansionIndex < exp.Length &&
                       (exp[m_expansionIndex] != '\n')) ++m_expansionIndex;
                m_baseIndex = ++m_expansionIndex;
                return true;
            }
            return false;
        }
        //пропуск разделителей (пробелы, перевод строки)
        private bool SkipDelimiters()
        {
            bool foundDelimeter = false;
            //пробел
            while ((m_expansionIndex < exp.Length) &&
                    (exp[m_expansionIndex]==' '))
            { ++m_expansionIndex; ++m_baseIndex; foundDelimeter = true; };
            //перевод строки
            while (m_expansionIndex < exp.Length &&
                            ((exp[m_expansionIndex] == '\n') ))
            {
                ++m_expansionIndex; ++m_baseIndex; foundDelimeter = true;
                ++m_currLine;   
            };
            return foundDelimeter;
        }
    }
}
