using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace lab
{
    //���� ������
    enum Error { UNKNOWN_SYMBOL};
    //��� ������

    //�����
    struct Token
    {
        //��������� ������ (���� ������ ������� �� ������� ���������
        //����� �������� ��� ����� � �.�.)
        public struct Position
        {
            public Position(int _beg, int _end) { beg = _beg; end = _end; }
            int beg;
            int end;
        };
        Position position;

        public AnalysisStage.TokenTypes type;        //��� ������
        public int attribute;       //�������
        public Token(Position _pos, AnalysisStage.TokenTypes _type, int _attribute)
        {
            position = _pos; type = _type; attribute = _attribute;
        }
    }

    class Lexan : lab.AnalysisStage
    {
        public string exp;  //������ �� ������ ��������������� ���������
        private int m_baseIndex;      //������� ������-���� � ���������
        private int m_expansionIndex; //������ �����������
        private int m_currLine;       //������� ������ � ����������(����� ����������� � ���  '\n')
        //string token;   //������� �������
        //Types tokType;  //��� �������
        //������ ������ ������������ ��� �������(��������. ������������� ����. ���������)
        public List<string> errorMessages=new List<string>();
        //������� ���������������
        //public IdentHashtable identTable = new IdentHashtable();
        //public Hashtable numConstTable = new Hashtable();
        //�������, ������������ �������� ���������
        class NumberDFA
        {
            int m_state=0;                                            //��������
            
            public bool EndState = false;                             //������� � �������� ���������
            public bool error = false;
            public string errorMsg = "";

            //����������� �������� ������� ������, �������� ��� � ����� ���������
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
                                m_state = 7;          //�����
                        break;

                    case 2: 
                        if (char.IsDigit(sym)) 
                            m_state = 3;
                        else
                            GoToErrorState("����� '.' ��������� ����������� ���������");
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
                                GoToErrorState("��������� +- ��� ����� �����");
                        break;

                    case 5: 
                        if (char.IsDigit(sym))
                            m_state = 6; 
                        else 
                            GoToErrorState("��������� ����� �����");
                        break;

                    case 6: 
                        if (!char.IsDigit(sym)) 
                            m_state = 7;                  //�����
                        break;
                }

                if (m_state == 7) 
                    EndState = true;
            }
            //��������� ������� � �������� ��������� � �������
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
            //��������� �������� ���������
            private int[] m_endStates = {-1,5,6,7,8,9,10,11,12,13};
            //������� ���������
            private int[,] m_transitionTable = {
                /*����/ 0   1   2   3   4   5   6   7   8   9   10  11  12  13*/
                /*'.'*/{1,  5,  3,  10, 13  },
                /*':'*/{2,  6,  8,  10, 13  },
                /*'>'*/{3,  6,  8,  10, 12  },
                /*'<'*/{4,  6,  8,  10, 13  },
                /*'='*/{-1, 6,  7,  9,  11  },
             /*������*/{-1, 6,  8,  10,  13  }
            };
            //����������� �������� ������� ������, �������� ��� � ����� ���������
            
            public bool EndState = false;                             //������� � �������� ���������
            public TokenTypes acceptedStringType;
            public void ReadSymbol(char sym)
            {
                //������������ ������� ������ � ������� ���������
                string punc =".:><=";
                int inpCharIndex = punc.IndexOf(sym);
                if (inpCharIndex < 0) inpCharIndex = 5;//'������'

                state=m_transitionTable[inpCharIndex,state];
                
                //��������
                if (Array.BinarySearch(m_endStates, state) > -1)
                {
                    EndState = true;
                    //������������� ���������� 5-8
                    TokenTypes[] equalTypes ={ TokenTypes.TWO_POINTS, TokenTypes.POINT,
                                                TokenTypes.ASSIGN,TokenTypes.COLON};
                    if (state < 9) acceptedStringType = equalTypes[state-5];
                    else acceptedStringType = TokenTypes.EQUALITY_OP;
                }
            }
        }

        //����������� � �������������� ������� ����������
        public Lexan(string expression)
        {
            exp = expression;
            Array.Sort(m_keywords);
        }

        //�������� ��������� �����
        public Token GetToken()
        {
            Token token = new Token();
            token.type = TokenTypes.TERMINATOR;
            
            //������� ������������ � ������������ (�������, ���������, etc.)
            //����� ��� ���������� �������������� ����� �������� �� ������ ('���������')
            while (SkipDelimiters() || SkipMyltiLineComms() || SkipComms()) ;

            if (m_expansionIndex < exp.Length)
            {
                //������������� ��������������
                if (TryCastAsIdent(ref token))
                { m_baseIndex = m_expansionIndex; return token; }

                //������������� �������� ���������
                if (TryCastAsNumConst(ref token)) 
                { m_baseIndex = m_expansionIndex; return token; }

                //������������� �������������� ��������� ����� ,;+-*/=()
                if (TryCastAsOneCharTerminal(ref token))
                { m_baseIndex = m_expansionIndex; return token; }

                //������������� '�������' ���������� ����� <,>,>=,<=,<>,:,:=, . , ..
                //������� ����� �������� ������� ������
                if (TryCastAsComplexTerminal(ref token))
                { m_baseIndex = m_expansionIndex; return token; }

                //���������� �������: �������� �� ����������� � �����
                token.type = TokenTypes.UNKNOWN;
            } 
            return token;
        }

        private bool TryCastAsComplexTerminal(ref Token token)
        {
            //� ���� ���������� '�������' ���������
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
                //���������� c�������������� ����� (�������� ��� - ������� -1)
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
            //��������� �������� �� ������ '��������������'
            int index = punc.IndexOf(exp[m_expansionIndex]);
            if (index > -1)//���� ��
            {
                m_expansionIndex++;
                //���������� ��������������� �����
                token = new Token(new Token.Position(m_baseIndex, m_expansionIndex), equalTypes[index],
                            -1);
                return true;
            }
            return false;
        }
        //���������� �������� ���������
        private bool TryCastAsNumConst(ref Token token)
        {
            //�������� ��������� ���������� � �����
            if (Char.IsDigit(exp[m_baseIndex]))
            {
                //���������� ��
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
                while (!numberDFA.EndState);         //���� �� �� � �������� ��������

                string foundNumConstAsString = exp.Substring(m_baseIndex, m_expansionIndex - m_baseIndex);
                //������������ ��������� - ������ � ������ ������ � ����������
                if (numberDFA.error) AddWarningMessage(numberDFA.errorMsg);
                //'�������' ����� ���������  - � ������� ��������
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
            //������������� (���������� � ����� ��� ������� '_')
            if (Char.IsLetter(exp[m_expansionIndex]) || exp[m_expansionIndex] == '_')
            {
                m_baseIndex = m_expansionIndex;
                //����� ���� ����� ��� ����� ��� '_'
                do
                    ++m_expansionIndex;
                while (m_expansionIndex != exp.Length &&
                    (Char.IsLetterOrDigit(exp[m_expansionIndex]) || exp[m_expansionIndex] == '_'));
                
                //��������� ������������� - ���������: name
                string name = exp.Substring(m_baseIndex, m_expansionIndex - m_baseIndex);

                //������������ ������������� ����� ���� �������� ������
                int kwIndex = Array.BinarySearch(lab.AnalysisStage.m_keywords, name);
                if (kwIndex >-1)
                    token = new Token(new Token.Position(m_baseIndex, m_expansionIndex), m_keywords_enum[kwIndex], -1);
                else
                {
                    int identID = identHashtable[name];
                    //���� ������� �������� - � ������� ���������������
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
            errorMessages.Add("������ " + m_currLine + ": " + msg);
        }
        
        //������� {������������� ������������}
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
        //������� //������������ ������������}
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
        //������� ������������ (�������, ������� ������)
        private bool SkipDelimiters()
        {
            bool foundDelimeter = false;
            //������
            while ((m_expansionIndex < exp.Length) &&
                    (exp[m_expansionIndex]==' '))
            { ++m_expansionIndex; ++m_baseIndex; foundDelimeter = true; };
            //������� ������
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
