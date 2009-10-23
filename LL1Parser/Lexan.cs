using System;
using System.Collections.Generic;

namespace LL1Parser
{
    // error types
    internal enum Error
    {
        UNKNOWN_SYMBOL
    };

    internal class Lexan : AnalysisStage
    {
        public List<string> errorMessages = new List<string>();
        private int m_baseIndex; // token start pos in input
        private int m_currLine; // current line in input
        private int m_expansionIndex; // last read position in input (possible token end position)
        public string program; // program text
        // after finding first error lexan should try to recover and continue analysis

        public Lexan(string expression)
        {
            program = expression.Replace("\r", "");
            Array.Sort(m_keywords);
        }

        // returns next toke from input
        public Token GetToken()
        {
            var token = new Token(TokenType.TERMINATOR);

            // skip comments and delimeters(spaces, tabs etc.)

            while (SkipDelimiters() || SkipMyltiLineComms() || SkipComms()) ;

            if (m_expansionIndex < program.Length)
            {
                // indentifier reñognition
                if (TryCastAsIdent(ref token))
                {
                    m_baseIndex = m_expansionIndex;
                    return token;
                }

                // numeric const reñognition
                if (TryCastAsNumConst(ref token))
                {
                    m_baseIndex = m_expansionIndex;
                    return token;
                }

                // reñognition of ,;+-*/=()
                if (TryCastAsOneCharTerminal(ref token))
                {
                    m_baseIndex = m_expansionIndex;
                    return token;
                }

                // reñognition of <,>,>=,<=,<>,:,:=, . , ..

                if (TryCastAsComplexTerminal(ref token))
                {
                    m_baseIndex = m_expansionIndex;
                    return token;
                }

                // terminal not belongs to language
                token.type = TokenType.UNKNOWN;
            }
            return token;
        }

        private bool TryCastAsComplexTerminal(ref Token token)
        {
            // if next character is one from that begins 'complex' terminals
            string punc = ".:><";
            if (punc.IndexOf(program[m_expansionIndex]) > -1)
            {
                TokenType tokenType = TokenType.UNKNOWN;
                int tokenLength = 1;
                switch (program[m_expansionIndex])
                {
                    case '.':
                        if ((m_expansionIndex + 1 < program.Length) && (program[m_expansionIndex + 1] == '.'))
                        {
                            tokenType = TokenType.TWO_POINTS;
                            tokenLength = 2;
                        }
                        else
                            tokenType = TokenType.POINT;
                        break;

                    case ':':
                        if (program[m_expansionIndex + 1] == '=')
                        {
                            tokenType = TokenType.ASSIGN;
                            tokenLength = 2;
                        }
                        else
                            tokenType = TokenType.COLON;
                        break;

                    case '>':
                        if (program[m_expansionIndex + 1] == '=')
                        {
                            tokenLength = 2;
                        }
                        tokenType = TokenType.EQUALITY_OP;
                        break;

                    case '<':
                        if ((program[m_expansionIndex + 1] == '=') ||
                            (program[m_expansionIndex + 1] == '>'))
                        {
                            tokenLength = 2;
                            tokenType = TokenType.EQUALITY_OP;
                        }
                        break;
                }

                string term = program.Substring(m_baseIndex, tokenLength);
                token = new Token(m_currLine, tokenType, -1);
                m_expansionIndex += tokenLength;
                return true;
            }
            return false;
        }

        private bool TryCastAsOneCharTerminal(ref Token token)
        {
            string punc = ",;+-*/=()";
            TokenType[] tokenType = {
                                        TokenType.COMMA, TokenType.SEMICOLON,
                                        TokenType.ADD_OP, TokenType.ADD_OP,
                                        TokenType.MULT_OP, TokenType.MULT_OP,
                                        TokenType.EQUALITY, TokenType.LEFT_PARENTHESIS,
                                        TokenType.RIGHT_PARENTHESIS
                                    };
            // if next char is one of above
            int index = punc.IndexOf(program[m_expansionIndex]);
            if (index > -1)
            {
                m_expansionIndex++;

                token = new Token(m_currLine, tokenType[index], -1);
                return true;
            }

            return false;
        }

        private bool TryCastAsNumConst(ref Token token)
        {
            // numeric const starts from digit
            if (Char.IsDigit(program[m_baseIndex]))
            {
                var numberDFA = new NumberDFA();
                m_expansionIndex--;
                do
                {
                    m_expansionIndex++;
                    if (m_expansionIndex < program.Length)
                        numberDFA.ReadSymbol(program[m_expansionIndex]);
                    else
                        //eof
                        numberDFA.ReadSymbol('#');
                } while (!numberDFA.EndState);

                string foundNumConstAsString = program.Substring(m_baseIndex, m_expansionIndex - m_baseIndex);
                if (numberDFA.error)
                    AddWarningMessage(numberDFA.errorMsg);
                else
                {
                    int constId = m_constTable.Add(foundNumConstAsString);
                    token = new Token(m_currLine, TokenType.NUMBER, constId);
                }

                return true;
            }
            return false;
        }

        private bool TryCastAsIdent(ref Token token)
        {
            // identifier starts from char or _
            if (Char.IsLetter(program[m_expansionIndex]) || program[m_expansionIndex] == '_')
            {
                m_baseIndex = m_expansionIndex;
                // then car or digit or '_'
                do
                    ++m_expansionIndex; while (m_expansionIndex != program.Length &&
                                               (Char.IsLetterOrDigit(program[m_expansionIndex]) ||
                                                program[m_expansionIndex] == '_'))
                    ;


                string name = program.Substring(m_baseIndex, m_expansionIndex - m_baseIndex);

                // identifiercan be a keyword
                if (Keyword(name))
                    token = new Token(m_currLine, GetKeywordType(name), -1);
                else
                {
                    int identID = m_identTable.Add(name);
                    token = new Token(m_currLine, TokenType.IDENTIFIER, identID);
                }
                return true;
            }

            return false;
        }


        private void AddWarningMessage(string msg)
        {
            errorMessages.Add("Line " + m_currLine + ": " + msg);
        }

        private bool SkipMyltiLineComms()
        {
            if (m_expansionIndex > program.Length - 1)
                return false;
            if (program[m_expansionIndex] == '{')
            {
                while (m_expansionIndex < program.Length &&
                       (program[m_expansionIndex] != '}'))
                {
                    if (program[m_expansionIndex] == '\n') ++m_currLine;
                    ++m_expansionIndex;
                }
                m_baseIndex = ++m_expansionIndex;
                return true;
            }
            return false;
        }

        private bool SkipComms()
        {
            if (m_expansionIndex > program.Length - 1)
                return false;
            if ((program[m_expansionIndex] == '/') &&
                (m_expansionIndex < program.Length - 1) &&
                (program[m_expansionIndex + 1] == '/'))
            {
                while (m_expansionIndex < program.Length &&
                       (program[m_expansionIndex] != '\n'))
                    m_expansionIndex++;
                m_baseIndex = ++m_expansionIndex;
                return true;
            }
            return false;
        }

        private bool SkipDelimiters()
        {
            bool foundDelimeter = false;
            // white space
            while ((m_expansionIndex < program.Length) &&
                   (program[m_expansionIndex] == ' '))
            {
                ++m_expansionIndex;
                ++m_baseIndex;
                foundDelimeter = true;
            }
            ;
            // newline
            while (m_expansionIndex < program.Length &&
                   ((program[m_expansionIndex] == '\n')))
            {
                ++m_expansionIndex;
                ++m_baseIndex;
                foundDelimeter = true;
                ++m_currLine;
            }
            ;
            return foundDelimeter;
        }

        #region Nested type: ComplexTerminalDFA

        private class ComplexTerminalDFA
        {
            // end states set
            private readonly int[] m_endStates = {-1, 5, 6, 7, 8, 9, 10, 11, 12, 13};
            // state transition table
            private readonly int[,] m_transitionTable = {
                                                            /*inpt/ 0   1   2   3   4   5   6   7   8   9   10  11  12  13*/
                                                            /*'.'*/{1, 5, 3, 10, 13},
                                                                   /*':'*/{2, 6, 8, 10, 13},
                                                                   /*'>'*/{3, 6, 8, 10, 12},
                                                                   /*'<'*/{4, 6, 8, 10, 13},
                                                                   /*'='*/{-1, 6, 7, 9, 11},
                                                                   /*another*/{-1, 6, 8, 10, 13}
                                                        };


            public TokenType acceptedStringType;
            public bool EndState;
            private int state;

            public void ReadSymbol(char sym)
            {
                // find match row in transition table to symbol
                string punc = ".:><=";
                int inpCharIndex = punc.IndexOf(sym);
                if (inpCharIndex < 0) inpCharIndex = 5; //'äðóãîé'

                state = m_transitionTable[inpCharIndex, state];

                // if we came to end state
                if (Array.BinarySearch(m_endStates, state) > -1)
                {
                    EndState = true;
                    // states 5-8 are corresponedtly .. . := : 
                    TokenType[] equalTypes = {
                                                 TokenType.TWO_POINTS, TokenType.POINT,
                                                 TokenType.ASSIGN, TokenType.COLON
                                             };
                    if (state < 9)
                        acceptedStringType = equalTypes[state - 5];
                    else
                        acceptedStringType = TokenType.EQUALITY_OP;
                }
            }
        }

        #endregion

        #region Nested type: NumberDFA

        private class NumberDFA
        {
            public bool EndState;
            public bool error;
            public string errorMsg = "";
            private int m_state;

            public void ReadSymbol(char sym)
            {
                switch (m_state)
                {
                    case 0:
                        if (char.IsDigit(sym))
                            m_state = 1;
                        break;

                    case 1:
                        if (sym == '.')
                            m_state = 2;
                        else if ((sym == 'e') || (sym == 'E'))
                            m_state = 4;
                        else if (!char.IsDigit(sym))
                            m_state = 7;
                        break;

                    case 2:
                        if (char.IsDigit(sym))
                            m_state = 3;
                        else
                            GoToErrorState("expected digit after '.' ");
                        break;

                    case 3:
                        if ((sym == 'e') || (sym == 'E'))
                            m_state = 4;
                        else if (!char.IsDigit(sym))
                            m_state = 7;
                        break;

                    case 4:
                        if ((sym == '+') || (sym == '-'))
                            m_state = 5;
                        else if (char.IsDigit(sym))
                            m_state = 6;
                        else
                            GoToErrorState("+- or digit expected");
                        break;

                    case 5:
                        if (char.IsDigit(sym))
                            m_state = 6;
                        else
                            GoToErrorState("digit expected");
                        break;

                    case 6:
                        if (!char.IsDigit(sym))
                            m_state = 7; //êîíåö
                        break;
                }

                if (m_state == 7)
                    EndState = true;
            }

            private void GoToErrorState(string msg)
            {
                m_state = 7;
                error = true;
                errorMsg = msg;
            }
        }

        #endregion
    }
}