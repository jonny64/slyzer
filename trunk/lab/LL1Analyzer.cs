using System;
using System.Collections.Generic;
using System.Text;

namespace lab
{
    class LL1Analyzer
    {
        public string errMsg = "<не установлен текст для ошибки>";
        
        const char TERMINATOR='t';
        //строка таблицы разбора
        //лексический анализатор - "подносчик патронов"
        Lexan myParser;

        public string exp;
        int symIndex = 0;
        public struct TableRow
        {
            public char[] terminals;
            public int jump;
            public bool accept, stack, error;
            public TableRow(char[] terms, int jmp, bool accpt,bool stck, 
                                                    bool err)
            {
                terminals=terms;
                jump=jmp;
                accept=accpt;stack=stck;error=err;
                //else errorMsg = "<не установлен текст для ошибки>";
            }
                
        }
        //ТАБЛИЦА РАЗБОРА
        TableRow[] parsTable ={
new TableRow( new char[] {                  'r'},	1,	false,	false,	true),
new TableRow( new char[] {                  'r'},	2,	true,	false,	true),
new TableRow( new char[] {                  'i'},	4,	false,	true,	true),
new TableRow( new char[] {                  'e'},	34,	true,	false,	true),
new TableRow( new char[] {                  'i'},	5,	false,	false,	true),
new TableRow( new char[] {                  'i'},	13,	false,	true,	true),
new TableRow( new char[] {             's', 'e'},	7,	false,	false,	true),
new TableRow( new char[] {                  's'},	9,	false,	false,	false),
new TableRow( new char[] {                  'e'},	12,	false,	false,	true),
new TableRow( new char[] {                  's'},	10,	true,	false,	true),
new TableRow( new char[] {                  'i'},	13,	false,	true,	true),
new TableRow( new char[] {             's', 'e'},	7,	false,	false,	true),
new TableRow( new char[] {                  'e'},	-1,	false,	false,	true),
new TableRow( new char[] {                  'i'},	14,	false,	false,	true),
new TableRow( new char[] {                  'i'},	17,	false,	true,	true),
new TableRow( new char[] {                  'c'},	16,	true,	false,	true),
new TableRow( new char[] {   'n', 'a', 'b', 'h'},	26,	false,	false,	true),
new TableRow( new char[] {                  'i'},	18,	false,	false,	true),
new TableRow( new char[] {                  'i'},	19,	true,	false,	true),
new TableRow( new char[] {             'o', 'c'},	20,	false,	false,	true),
new TableRow( new char[] {                  'o'},	22,	false,	false,	false),
new TableRow( new char[] {                  'c'},	25,	false,	false,	true),
new TableRow( new char[] {                  'o'},	23,	true,	false,	true),
new TableRow( new char[] {                  'i'},	24,	true,	false,	true),
new TableRow( new char[] {             'o', 'c'},	20,	false,	false,	true),
new TableRow( new char[] {                  'c'},	-1,	false,	false,	true),
new TableRow( new char[] {                  'n'},	30,	false,	false,	false),
new TableRow( new char[] {                  'a'},	31,	false,	false,	false),
new TableRow( new char[] {                  'b'},	32,	false,	false,	false),
new TableRow( new char[] {                  'h'},	33,	false,	false,	true),
new TableRow( new char[] {                  'n'},	-1,	true,	false,	true),
new TableRow( new char[] {                  'a'},	-1,	true,	false,	true),
new TableRow( new char[] {                  'b'},	-1,	true,	false,	true),
new TableRow( new char[] {                  'h'},	-1,	true,	false,	true),
new TableRow( new char[] {                  't'},	-1,	true,	false,	true)
        };



        public LL1Analyzer(string _exp)
        {
            exp = _exp;
            //создаем лексический анализатор, на вход подаем текст
            myParser = new Lexan(_exp);
            //Token token;
            //while ((token = myParser.GetToken()).type != AlalysisStage.TokenTypes.TERMINATOR)
            //{
            //    OutText("(" + token.type.ToString() + ", " + token.attribute + " )\n");
            //}
            //string[] errors = myParser.errorMessages.ToArray();
            //for (int errorIndex = 0; errorIndex < errors.Length; errorIndex++)
            //    OutText(errors[errorIndex] + '\n');
            
        }

        //читает символ;
        private AlalysisStage.TokenTypes readSym()
        {
            //return exp[symIndex++];
            return myParser.GetToken().type;
        }

        public bool Check()
        {
            symIndex = 0;
            int i = 0;//номер строки таблицы разбора
            Stack<int> S = new Stack<int>();
            bool la = true;
            AlalysisStage.TokenTypes sym = readSym();
            while ((sym != AlalysisStage.TokenTypes.TERMINATOR) && (i != -1))
            {
                TableRow row = parsTable[i];
                if ( Contain(row.terminals,sym) )
                {
                    la = row.accept;
                    if (row.jump==-1)
                    {
                        i = S.Pop();
                    }
                    else
                    {
                        if (row.stack) S.Push( i + 1 );
                        i=row.jump;
                    }
                }
                else
                {
                    if (row.error)
                    {
                        errMsg = GetErrMsg(i,sym);
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
            if (i == parsTable.Length - 1)
                return true;//проверка успешна (дошли до символа терминатора в последней строчке)
            else
            {
                errMsg = GetErrMsg(i,sym);
                return false;//проверка неуспешна
            }   
        }

        //выбрасывает сообщение об ошибке, произошедшей при разборе по строке i
        private string GetErrMsg(int errRow, AlalysisStage.TokenTypes sym)
        {
            //throw new Exception("The method or operation is not implemented.");
            //предполагаемые символы
            char[] suggestedSyms=parsTable[errRow].terminals;
            //движемся вниз (по альтернативным продукциям, если они были)
            //и добавляем их terminals
            for (int row = errRow - 1; row > -1; row--)
            {
                if (!parsTable[row].error) suggestedSyms = Union(suggestedSyms, parsTable[row].terminals);
                else break;
            }

            string errMsg = "имеем " + SymToString(GetCharForTokenType(sym)) + 
                " ожидается ";
            for (int i = 0; i < suggestedSyms.Length; i++)
            {
                errMsg+=" " + SymToString(suggestedSyms[i]);
            }
            return errMsg;
        }

        private string SymToString(char p)
        {
            char[] syms ={ 'r', 'e', 's', 'c', 'i', 'o', 'n', 'a', 'b', 'h','t'};
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
        private bool Contain(char[] p, AlalysisStage.TokenTypes tokentype)
        {
            char sym = GetCharForTokenType(tokentype);

            for (int symIndex = 0; symIndex < p.Length; symIndex++)
            {
                if (p[symIndex] == sym) return true;
            }
            return false;
        }

        private char GetCharForTokenType(AlalysisStage.TokenTypes tokentype)
        {
            char[] syms ={ 'r', 'e', 's', 'c', 'i', 'o', 'n','a','b','h', 't'};
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
                if (eqTypes[i] == tokentype) return syms[i];
            }

            throw new Exception("Не могу преобраовать токен в char");
        }

    }
}
