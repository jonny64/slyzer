using System;
using System.Collections.Generic;
using System.Text;
using LL1AnalyzerTool;
namespace lab
{
    class LL1Analyzer
    {
        public string ErrorMessage = "<не установлен текст для ошибки>";
        
        const char TERMINATOR='t';
        //строка таблицы разбора
        //лексический анализатор - "подносчик патронов"
        Lexan lexan;

        public string program;
        int symIndex = 0;
        //ТАБЛИЦА РАЗБОРА
        ParsTable parsTable = new ParsTable(Grammar.LoadFromFile("Grammars\\record.txt"));

        public LL1Analyzer(string input)
        {
            program = input;
            lexan = new Lexan(input);
        }

        //читает символ;
        private Symbol readSym()
        {
            return new Symbol( lexan.GetToken().type.ToString().ToLower() );
        }

        public bool InputCorrect()
        {
            symIndex = 0;
            int i = 0;//номер строки таблицы разбора
            Stack<int> S = new Stack<int>();
            S.Push(ParsTable.JUMP_FINISH);
            bool la = true;
            Symbol sym = readSym();
            while ((sym != Symbol.TERMINATOR) && (i != ParsTable.JUMP_FINISH))
            {
                TableRow row = parsTable[i];
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

            ErrorMessage = "Лишнее " + sym;
            return false;
        }

        //выбрасывает сообщение об ошибке, произошедшей при разборе по строке i
        private string GetErrMsg(int errRow, Symbol sym)
        {
            //throw new Exception("The method or operation is not implemented.");
            //предполагаемые символы
            Set suggestedSyms = parsTable[errRow].terminals;
            //движемся вниз (по альтернативным продукциям, если они были)
            //и добавляем их terminals
            for (int row = errRow - 1; row > -1; row--)
            {
                if (!parsTable[row].error) 
                    suggestedSyms += parsTable[row].terminals;
                else 
                    break;
            }

            string errMsg = "имеем " + sym +
                " ожидается " + suggestedSyms;

            return errMsg;
        }

    }
}
