using System;
using System.Collections.Generic;
using System.Text;
using LL1AnalyzerTool;
namespace lab
{
    class LL1Analyzer
    {
        public string ErrorMessage = "<не установлен текст для ошибки>";

        Token m_currToken;
        const char TERMINATOR='t';
        Lexan lexan;
        string m_program;
        ParsTable m_parsTable;

        public LL1Analyzer(ParsTable parsTable)
        {
            m_parsTable = parsTable;
        }

        public LL1Analyzer()
        {
            m_parsTable = new ParsTable(Grammar.LoadFromFile("Grammars\\program.txt"));
        }

        //читает символ;
        private Symbol readSym()
        {
            m_currToken = lexan.GetToken();
            return new Symbol(m_currToken.type.ToString().ToLower());
        }

        public bool Check(string input)
        {
            m_program = input;
            lexan = new Lexan(input);

            int i = 0;//номер строки таблицы разбора
            Stack<int> S = new Stack<int>();
            S.Push(ParsTable.JUMP_FINISH);
            bool la = true;
            Symbol sym = readSym();
            while (i != ParsTable.JUMP_FINISH)
            {
                TableRow row = m_parsTable[i];
                if ( row.terminals.Contains(sym) )
                {
                    la = row.accept;
                    if (row.jump == ParsTable.JUMP_FINISH) //return
                    {
                        i = S.Pop();
                    }
                    else
                    {
                        if (row.stack) 
                            S.Push( i + 1 );
                        i=row.jump;
                    }
                }
                else
                {
                    if (row.error)
                    {
                        ErrorMessage = GetErrMsg(i,sym);
                        return false;//проверка неуспешна
                    }
                    else
                    {
                        i++;    //пробуем альтернативную продукцию
                        la = false;
                    }
                }
                if (la) 
                    sym = readSym();
            }

            if ((sym == Symbol.TERMINATOR) && (S.Count == 0) )
                return true;

            if (S.Count != 0)
            {
                ErrorMessage = GetErrMsg(i, sym);
                return false;
            }

            ErrorMessage = "Лишнее " + sym + " в строке " + m_currToken.lineno;
            return false;
        }

        //выбрасывает сообщение об ошибке, произошедшей при разборе по строке i
        private string GetErrMsg(int errRow, Symbol sym)
        {
            //throw new Exception("The method or operation is not implemented.");
            //предполагаемые символы
            Set suggestedSyms = m_parsTable[errRow].terminals;
            //движемся вниз (по альтернативным продукциям, если они были)
            //и добавляем их terminals
            for (int row = errRow - 1; row > -1; row--)
            {
                if (!m_parsTable[row].error) 
                    suggestedSyms += m_parsTable[row].terminals;
                else 
                    break;
            }

            string errMsg = "имеем " + sym +
                " ожидается " + suggestedSyms + " в строке " + m_currToken.lineno;

            return errMsg;
        }

    }
}
