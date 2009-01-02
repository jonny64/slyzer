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

        public string m_program;
        //������� �������
        ParsTable m_parsTable;

        public LL1Analyzer(ParsTable parsTable)
        {
            m_parsTable = parsTable;
        }

        public LL1Analyzer()
        {
            m_parsTable = new ParsTable(Grammar.LoadFromFile("Grammars\\program.txt"));
        }

        //������ ������;
        private Symbol readSym()
        {
            return new Symbol( lexan.GetToken().type.ToString().ToLower() );
        }

        public bool Check(string input)
        {
            m_program = input;
            lexan = new Lexan(input);

            int i = 0;//����� ������ ������� �������
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
            Set suggestedSyms = m_parsTable[errRow].terminals;
            //�������� ���� (�� �������������� ����������, ���� ��� ����)
            //� ��������� �� terminals
            for (int row = errRow - 1; row > -1; row--)
            {
                if (!m_parsTable[row].error) 
                    suggestedSyms += m_parsTable[row].terminals;
                else 
                    break;
            }

            string errMsg = "����� " + sym +
                " ��������� " + suggestedSyms;

            return errMsg;
        }

    }
}
