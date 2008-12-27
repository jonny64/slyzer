using System;
using System.Collections.Generic;
using System.Text;
using LL1AnalyzerTool;
namespace lab
{
    class LL1Analyzer
    {
        public string ErrorMessage = "<�� ���������� ����� ��� ������>";
        
        const char TERMINATOR='t';
        //������ ������� �������
        //����������� ���������� - "��������� ��������"
        Lexan lexan;

        public string program;
        int symIndex = 0;
        //������� �������
        ParsTable parsTable = new ParsTable(Grammar.LoadFromFile("Grammars\\record.txt"));

        public LL1Analyzer(string input)
        {
            program = input;
            lexan = new Lexan(input);
        }

        //������ ������;
        private Symbol readSym()
        {
            return new Symbol( lexan.GetToken().type.ToString().ToLower() );
        }

        public bool InputCorrect()
        {
            symIndex = 0;
            int i = 0;//����� ������ ������� �������
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
                        return false;//�������� ���������
                    }
                    else
                    {
                        i++;    //������� �������������� ���������
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

            ErrorMessage = "������ " + sym;
            return false;
        }

        //����������� ��������� �� ������, ������������ ��� ������� �� ������ i
        private string GetErrMsg(int errRow, Symbol sym)
        {
            //throw new Exception("The method or operation is not implemented.");
            //�������������� �������
            Set suggestedSyms = parsTable[errRow].terminals;
            //�������� ���� (�� �������������� ����������, ���� ��� ����)
            //� ��������� �� terminals
            for (int row = errRow - 1; row > -1; row--)
            {
                if (!parsTable[row].error) 
                    suggestedSyms += parsTable[row].terminals;
                else 
                    break;
            }

            string errMsg = "����� " + sym +
                " ��������� " + suggestedSyms;

            return errMsg;
        }

    }
}
