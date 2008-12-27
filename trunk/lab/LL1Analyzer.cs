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
        Lexan myParser;

        public string program;
        int symIndex = 0;
        //������� �������
        ParsTable parsTable = new ParsTable(Grammar.LoadFromFile("Grammars\\record.txt"));

        public LL1Analyzer(string input)
        {
            program = input;
            //������� ����������� ����������, �� ���� ������ �����
            myParser = new Lexan(input);
            //Token token;
            //while ((token = myParser.GetToken()).type != AlalysisStage.TokenTypes.TERMINATOR)
            //{
            //    OutText("(" + token.type.ToString() + ", " + token.attribute + " )\n");
            //}
            //string[] errors = myParser.errorMessages.ToArray();
            //for (int errorIndex = 0; errorIndex < errors.Length; errorIndex++)
            //    OutText(errors[errorIndex] + '\n');
            
        }

        //������ ������;
        private Symbol readSym()
        {
            return new Symbol( myParser.GetToken().type.ToString().ToLower() );
        }

        public bool InputCorrect()
        {
            symIndex = 0;
            int i = 0;//����� ������ ������� �������
            Stack<int> S = new Stack<int>();
            bool la = true;
            Symbol sym = readSym();
            while ((sym != Symbol.TERMINATOR) && (i != -1))
            {
                TableRow row = parsTable[i];
                if ( row.terminals.Contains(sym) )
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
            if (i == parsTable.Length - 1)
                return true;//�������� ������� (����� �� ������� ����������� � ��������� �������)
            else
            {
                ErrorMessage = GetErrMsg(i,sym);
                return false;//�������� ���������
            }   
        }

        //����������� ��������� �� ������, ������������ ��� ������� �� ������ i
        private string GetErrMsg(int errRow, Symbol sym)
        {
            //throw new Exception("The method or operation is not implemented.");
            //�������������� �������
            Set suggestedSyms=parsTable[errRow].terminals;
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
