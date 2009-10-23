using System.Collections.Generic;
using LL1AnalyzerTool;

namespace LL1Parser
{
    internal class LL1Analyzer
    {
        private const char TERMINATOR = 't';
        public string ErrorMessage = "<error message not set>";
        private Lexan lexan;
        private Token m_currToken;
        private ParsTable m_parsTable;
        private string m_program;

        public LL1Analyzer(ParsTable parsTable)
        {
            m_parsTable = parsTable;
            //UsePredefinedTable
        }

        public LL1Analyzer()
        {
            m_parsTable = new ParsTable(Grammar.LoadFromFile("Grammars\\program.txt"));
        }

        private void UsePredefinedTable()
        {
            TableRow[] rows = {
                                  new TableRow(new Set(new Symbol("program")), 1, false, false, true),
                                  new TableRow(new Set(new Symbol("program")), 43, false, true, true),
                                  new TableRow(new Set(new Symbol("begin"), new Symbol("type"), new Symbol("var")), 8,
                                               false, true, true),
                                  new TableRow(new Set(new Symbol("point")), -1, true, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), 5, false, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), 157, false, true, true),
                                  new TableRow(new Set(new Symbol("assign")), 7, true, false, true),
                                  new TableRow(
                                      new Set(new Symbol("identifier"), new Symbol("left_parenthesis"),
                                              new Symbol("not"), new Symbol("number")), 31, false, false, true),
                                  new TableRow(new Set(new Symbol("begin"), new Symbol("type"), new Symbol("var")), 9,
                                               false, false, true),
                                  new TableRow(new Set(new Symbol("begin"), new Symbol("type"), new Symbol("var")), 151,
                                               false, true, true),
                                  new TableRow(new Set(new Symbol("begin"), new Symbol("var")), 179, false, true, true),
                                  new TableRow(new Set(new Symbol("begin")), 79, false, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), 13, false, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), 93, false, true, true),
                                  new TableRow(new Set(new Symbol("end"), new Symbol("semicolon")), 97, false, false,
                                               true),
                                  new TableRow(new Set(new Symbol("begin")), 16, false, false, true),
                                  new TableRow(new Set(new Symbol("begin")), 17, true, false, true),
                                  new TableRow(
                                      new Set(new Symbol("begin"), new Symbol("identifier"), new Symbol("if")), 73,
                                      false, true, true),
                                  new TableRow(new Set(new Symbol("end"), new Symbol("semicolon")), 81, false, true,
                                               true),
                                  new TableRow(new Set(new Symbol("end")), -1, true, false, true),
                                  new TableRow(new Set(new Symbol("if")), 21, false, false, true),
                                  new TableRow(new Set(new Symbol("if")), 22, true, false, true),
                                  new TableRow(
                                      new Set(new Symbol("identifier"), new Symbol("left_parenthesis"),
                                              new Symbol("not"), new Symbol("number")), 31, false, true, true),
                                  new TableRow(new Set(new Symbol("then")), 24, true, false, true),
                                  new TableRow(new Set(new Symbol("begin")), 15, false, true, true),
                                  new TableRow(new Set(new Symbol("else"), new Symbol("end"), new Symbol("semicolon")),
                                               26, false, false, true),
                                  new TableRow(new Set(new Symbol("end"), new Symbol("semicolon")), 28, false, false,
                                               false),
                                  new TableRow(new Set(new Symbol("else")), 29, false, false, true),
                                  new TableRow(new Set(new Symbol("end"), new Symbol("semicolon")), -1, false, false,
                                               true),
                                  new TableRow(new Set(new Symbol("else")), 30, true, false, true),
                                  new TableRow(
                                      new Set(new Symbol("begin"), new Symbol("identifier"), new Symbol("if")), 73,
                                      false, false, true),
                                  new TableRow(
                                      new Set(new Symbol("identifier"), new Symbol("left_parenthesis"),
                                              new Symbol("not"), new Symbol("number")), 32, false, false, true),
                                  new TableRow(
                                      new Set(new Symbol("identifier"), new Symbol("left_parenthesis"),
                                              new Symbol("not"), new Symbol("number")), 103, false, true, true),
                                  new TableRow(
                                      new Set(new Symbol("end"), new Symbol("equality"), new Symbol("equality_op"),
                                              new Symbol("right_parenthesis"), new Symbol("semicolon"),
                                              new Symbol("then")), 109, false, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), 35, false, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), 36, true, false, true),
                                  new TableRow(new Set(new Symbol("point")), 37, true, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), -1, true, false, true),
                                  new TableRow(new Set(new Symbol("right_parenthesis")), 40, false, false, false),
                                  new TableRow(new Set(new Symbol("comma")), 41, false, false, true),
                                  new TableRow(new Set(new Symbol("right_parenthesis")), -1, false, false, true),
                                  new TableRow(new Set(new Symbol("comma")), 42, true, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), 34, false, false, true),
                                  new TableRow(new Set(new Symbol("program")), 44, false, false, true),
                                  new TableRow(new Set(new Symbol("program")), 45, true, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), 46, true, false, true),
                                  new TableRow(new Set(new Symbol("left_parenthesis")), 47, true, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), 34, false, true, true),
                                  new TableRow(new Set(new Symbol("comma"), new Symbol("right_parenthesis")), 38, false,
                                               true, true),
                                  new TableRow(new Set(new Symbol("right_parenthesis")), 50, true, false, true),
                                  new TableRow(new Set(new Symbol("semicolon")), -1, true, false, true),
                                  new TableRow(new Set(new Symbol("colon")), 53, false, false, false),
                                  new TableRow(new Set(new Symbol("comma")), 54, false, false, true),
                                  new TableRow(new Set(new Symbol("colon")), -1, false, false, true),
                                  new TableRow(new Set(new Symbol("comma")), 55, true, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), -1, true, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), 60, false, false, false),
                                  new TableRow(new Set(new Symbol("left_parenthesis")), 61, false, false, false),
                                  new TableRow(new Set(new Symbol("not")), 64, false, false, false),
                                  new TableRow(new Set(new Symbol("number")), 66, false, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), -1, true, false, true),
                                  new TableRow(new Set(new Symbol("left_parenthesis")), 62, true, false, true),
                                  new TableRow(
                                      new Set(new Symbol("identifier"), new Symbol("left_parenthesis"),
                                              new Symbol("not"), new Symbol("number")), 31, false, true, true),
                                  new TableRow(new Set(new Symbol("right_parenthesis")), -1, true, false, true),
                                  new TableRow(new Set(new Symbol("not")), 65, true, false, true),
                                  new TableRow(
                                      new Set(new Symbol("identifier"), new Symbol("left_parenthesis"),
                                              new Symbol("not"), new Symbol("number")), 56, false, false, true),
                                  new TableRow(new Set(new Symbol("number")), -1, true, false, true),
                                  new TableRow(
                                      new Set(new Symbol("add_op"), new Symbol("end"), new Symbol("equality"),
                                              new Symbol("equality_op"), new Symbol("right_parenthesis"),
                                              new Symbol("semicolon"), new Symbol("then")), 69, false, false, false),
                                  new TableRow(new Set(new Symbol("mult_op")), 70, false, false, true),
                                  new TableRow(
                                      new Set(new Symbol("add_op"), new Symbol("end"), new Symbol("equality"),
                                              new Symbol("equality_op"), new Symbol("right_parenthesis"),
                                              new Symbol("semicolon"), new Symbol("then")), -1, false, false, true),
                                  new TableRow(new Set(new Symbol("mult_op")), 71, true, false, true),
                                  new TableRow(
                                      new Set(new Symbol("identifier"), new Symbol("left_parenthesis"),
                                              new Symbol("not"), new Symbol("number")), 56, false, true, true),
                                  new TableRow(
                                      new Set(new Symbol("add_op"), new Symbol("end"), new Symbol("equality"),
                                              new Symbol("equality_op"), new Symbol("mult_op"),
                                              new Symbol("right_parenthesis"), new Symbol("semicolon"),
                                              new Symbol("then")), 67, false, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), 76, false, false, false),
                                  new TableRow(new Set(new Symbol("begin")), 77, false, false, false),
                                  new TableRow(new Set(new Symbol("if")), 78, false, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), 4, false, false, true),
                                  new TableRow(new Set(new Symbol("begin")), 15, false, false, true),
                                  new TableRow(new Set(new Symbol("if")), 20, false, false, true),
                                  new TableRow(new Set(new Symbol("begin")), 80, false, false, true),
                                  new TableRow(new Set(new Symbol("begin")), 15, false, false, true),
                                  new TableRow(new Set(new Symbol("end")), 83, false, false, false),
                                  new TableRow(new Set(new Symbol("semicolon")), 84, false, false, true),
                                  new TableRow(new Set(new Symbol("end")), -1, false, false, true),
                                  new TableRow(new Set(new Symbol("semicolon")), 85, true, false, true),
                                  new TableRow(
                                      new Set(new Symbol("begin"), new Symbol("identifier"), new Symbol("if")), 73,
                                      false, true, true),
                                  new TableRow(new Set(new Symbol("end"), new Symbol("semicolon")), 81, false, false,
                                               true),
                                  new TableRow(new Set(new Symbol("colon")), 89, false, false, false),
                                  new TableRow(new Set(new Symbol("comma")), 90, false, false, true),
                                  new TableRow(new Set(new Symbol("colon")), -1, false, false, true),
                                  new TableRow(new Set(new Symbol("comma")), 91, true, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), 92, true, false, true),
                                  new TableRow(new Set(new Symbol("colon"), new Symbol("comma")), 87, false, false, true)
                                  ,
                                  new TableRow(new Set(new Symbol("identifier")), 94, false, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), 176, false, true, true),
                                  new TableRow(new Set(new Symbol("colon")), 96, true, false, true),
                                  new TableRow(
                                      new Set(new Symbol("boolean"), new Symbol("char"), new Symbol("identifier"),
                                              new Symbol("integer"), new Symbol("number"), new Symbol("real"),
                                              new Symbol("record")), 123, false, false, true),
                                  new TableRow(new Set(new Symbol("end")), 99, false, false, false),
                                  new TableRow(new Set(new Symbol("semicolon")), 100, false, false, true),
                                  new TableRow(new Set(new Symbol("end")), -1, false, false, true),
                                  new TableRow(new Set(new Symbol("semicolon")), 101, true, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), 93, false, true, true),
                                  new TableRow(new Set(new Symbol("end"), new Symbol("semicolon")), 97, false, false,
                                               true),
                                  new TableRow(
                                      new Set(new Symbol("identifier"), new Symbol("left_parenthesis"),
                                              new Symbol("not"), new Symbol("number")), 104, false, false, true),
                                  new TableRow(
                                      new Set(new Symbol("identifier"), new Symbol("left_parenthesis"),
                                              new Symbol("not"), new Symbol("number")), 106, false, true, true),
                                  new TableRow(
                                      new Set(new Symbol("add_op"), new Symbol("end"), new Symbol("equality"),
                                              new Symbol("equality_op"), new Symbol("right_parenthesis"),
                                              new Symbol("semicolon"), new Symbol("then")), 117, false, false, true),
                                  new TableRow(
                                      new Set(new Symbol("identifier"), new Symbol("left_parenthesis"),
                                              new Symbol("not"), new Symbol("number")), 107, false, false, true),
                                  new TableRow(
                                      new Set(new Symbol("identifier"), new Symbol("left_parenthesis"),
                                              new Symbol("not"), new Symbol("number")), 56, false, true, true),
                                  new TableRow(
                                      new Set(new Symbol("add_op"), new Symbol("end"), new Symbol("equality"),
                                              new Symbol("equality_op"), new Symbol("mult_op"),
                                              new Symbol("right_parenthesis"), new Symbol("semicolon"),
                                              new Symbol("then")), 67, false, false, true),
                                  new TableRow(
                                      new Set(new Symbol("end"), new Symbol("right_parenthesis"),
                                              new Symbol("semicolon"), new Symbol("then")), 112, false, false, false),
                                  new TableRow(new Set(new Symbol("equality")), 113, false, false, false),
                                  new TableRow(new Set(new Symbol("equality_op")), 115, false, false, true),
                                  new TableRow(
                                      new Set(new Symbol("end"), new Symbol("right_parenthesis"),
                                              new Symbol("semicolon"), new Symbol("then")), -1, false, false, true),
                                  new TableRow(new Set(new Symbol("equality")), 114, true, false, true),
                                  new TableRow(
                                      new Set(new Symbol("identifier"), new Symbol("left_parenthesis"),
                                              new Symbol("not"), new Symbol("number")), 103, false, false, true),
                                  new TableRow(new Set(new Symbol("equality_op")), 116, true, false, true),
                                  new TableRow(
                                      new Set(new Symbol("identifier"), new Symbol("left_parenthesis"),
                                              new Symbol("not"), new Symbol("number")), 103, false, false, true),
                                  new TableRow(
                                      new Set(new Symbol("end"), new Symbol("equality"), new Symbol("equality_op"),
                                              new Symbol("right_parenthesis"), new Symbol("semicolon"),
                                              new Symbol("then")), 119, false, false, false),
                                  new TableRow(new Set(new Symbol("add_op")), 120, false, false, true),
                                  new TableRow(
                                      new Set(new Symbol("end"), new Symbol("equality"), new Symbol("equality_op"),
                                              new Symbol("right_parenthesis"), new Symbol("semicolon"),
                                              new Symbol("then")), -1, false, false, true),
                                  new TableRow(new Set(new Symbol("add_op")), 121, true, false, true),
                                  new TableRow(
                                      new Set(new Symbol("identifier"), new Symbol("left_parenthesis"),
                                              new Symbol("not"), new Symbol("number")), 106, false, true, true),
                                  new TableRow(
                                      new Set(new Symbol("add_op"), new Symbol("end"), new Symbol("equality"),
                                              new Symbol("equality_op"), new Symbol("right_parenthesis"),
                                              new Symbol("semicolon"), new Symbol("then")), 117, false, false, true),
                                  new TableRow(new Set(new Symbol("boolean")), 130, false, false, false),
                                  new TableRow(new Set(new Symbol("char")), 131, false, false, false),
                                  new TableRow(new Set(new Symbol("identifier")), 132, false, false, false),
                                  new TableRow(new Set(new Symbol("integer")), 133, false, false, false),
                                  new TableRow(new Set(new Symbol("number")), 134, false, false, false),
                                  new TableRow(new Set(new Symbol("real")), 137, false, false, false),
                                  new TableRow(new Set(new Symbol("record")), 138, false, false, true),
                                  new TableRow(new Set(new Symbol("boolean")), -1, true, false, true),
                                  new TableRow(new Set(new Symbol("char")), -1, true, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), -1, true, false, true),
                                  new TableRow(new Set(new Symbol("integer")), -1, true, false, true),
                                  new TableRow(new Set(new Symbol("number")), 135, true, false, true),
                                  new TableRow(new Set(new Symbol("two_points")), 136, true, false, true),
                                  new TableRow(new Set(new Symbol("number")), -1, true, false, true),
                                  new TableRow(new Set(new Symbol("real")), -1, true, false, true),
                                  new TableRow(new Set(new Symbol("record")), 139, true, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), 12, false, true, true),
                                  new TableRow(new Set(new Symbol("end")), -1, true, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), 142, false, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), 143, true, false, true),
                                  new TableRow(new Set(new Symbol("equality")), 144, true, false, true),
                                  new TableRow(
                                      new Set(new Symbol("boolean"), new Symbol("char"), new Symbol("identifier"),
                                              new Symbol("integer"), new Symbol("number"), new Symbol("real"),
                                              new Symbol("record")), 123, false, true, true),
                                  new TableRow(new Set(new Symbol("semicolon")), -1, true, false, true),
                                  new TableRow(new Set(new Symbol("begin"), new Symbol("var")), 148, false, false, false)
                                  ,
                                  new TableRow(new Set(new Symbol("identifier")), 149, false, false, true),
                                  new TableRow(new Set(new Symbol("begin"), new Symbol("var")), -1, false, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), 141, false, true, true),
                                  new TableRow(
                                      new Set(new Symbol("begin"), new Symbol("identifier"), new Symbol("var")), 146,
                                      false, false, true),
                                  new TableRow(new Set(new Symbol("begin"), new Symbol("var")), 153, false, false, false)
                                  ,
                                  new TableRow(new Set(new Symbol("type")), 154, false, false, true),
                                  new TableRow(new Set(new Symbol("begin"), new Symbol("var")), -1, false, false, true),
                                  new TableRow(new Set(new Symbol("type")), 155, true, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), 141, false, true, true),
                                  new TableRow(
                                      new Set(new Symbol("begin"), new Symbol("identifier"), new Symbol("var")), 146,
                                      false, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), 158, false, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), 159, true, false, true),
                                  new TableRow(new Set(new Symbol("assign"), new Symbol("point")), 170, false, false,
                                               true),
                                  new TableRow(new Set(new Symbol("identifier")), 161, false, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), 162, true, false, true),
                                  new TableRow(new Set(new Symbol("colon"), new Symbol("comma")), 51, false, true, true)
                                  ,
                                  new TableRow(new Set(new Symbol("colon")), 164, true, false, true),
                                  new TableRow(
                                      new Set(new Symbol("boolean"), new Symbol("char"), new Symbol("identifier"),
                                              new Symbol("integer"), new Symbol("number"), new Symbol("real"),
                                              new Symbol("record")), 123, false, true, true),
                                  new TableRow(new Set(new Symbol("semicolon")), -1, true, false, true),
                                  new TableRow(new Set(new Symbol("begin")), 168, false, false, false),
                                  new TableRow(new Set(new Symbol("identifier")), 169, false, false, true),
                                  new TableRow(new Set(new Symbol("begin")), -1, false, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), 160, false, false, true),
                                  new TableRow(new Set(new Symbol("assign")), 172, false, false, false),
                                  new TableRow(new Set(new Symbol("point")), 173, false, false, true),
                                  new TableRow(new Set(new Symbol("assign")), -1, false, false, true),
                                  new TableRow(new Set(new Symbol("point")), 174, true, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), 175, true, false, true),
                                  new TableRow(new Set(new Symbol("assign"), new Symbol("point")), 170, false, false,
                                               true),
                                  new TableRow(new Set(new Symbol("identifier")), 177, false, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), 178, true, false, true),
                                  new TableRow(new Set(new Symbol("colon"), new Symbol("comma")), 87, false, false, true)
                                  ,
                                  new TableRow(new Set(new Symbol("begin")), 181, false, false, false),
                                  new TableRow(new Set(new Symbol("var")), 182, false, false, true),
                                  new TableRow(new Set(new Symbol("begin")), -1, false, false, true),
                                  new TableRow(new Set(new Symbol("var")), 183, true, false, true),
                                  new TableRow(new Set(new Symbol("identifier")), 160, false, true, true),
                                  new TableRow(new Set(new Symbol("begin"), new Symbol("identifier")), 166, false, false,
                                               true)
                              };
            m_parsTable = new ParsTable(rows);
        }

        private Symbol readSym()
        {
            m_currToken = lexan.GetToken();
            return new Symbol(m_currToken.type.ToString().ToLower());
        }

        public bool Check(string input)
        {
            m_program = input;
            lexan = new Lexan(input);

            int i = 0; // parse table row
            var S = new Stack<int>();
            S.Push(ParsTable.JUMP_FINISH);
            bool la = true;
            Symbol sym = readSym();
            while (i != ParsTable.JUMP_FINISH)
            {
                TableRow row = m_parsTable[i];
                if (row.terminals.Contains(sym))
                {
                    la = row.accept;
                    // if (return == true)...
                    if (row.jump == ParsTable.JUMP_FINISH)
                    {
                        i = S.Pop();
                    }
                    else
                    {
                        if (row.stack)
                            S.Push(i + 1);
                        i = row.jump;
                    }
                }
                else
                {
                    if (row.error)
                    {
                        ErrorMessage = GetErrMsg(i, sym);
                        return false; // check failed
                    }
                    else
                    {
                        i++; // try ing alternative production
                        la = false;
                    }
                }
                if (la)
                    sym = readSym();
            }

            if ((sym == Symbol.TERMINATOR) && (S.Count == 0))
                return true;

            // check failed
            if (S.Count != 0)
            {
                ErrorMessage = GetErrMsg(i, sym);
                return false;
            }

            ErrorMessage = "Excessive " + sym + " line " + m_currToken.lineno;
            return false;
        }

        /// <summary>
        /// forms error message
        /// </summary>
        /// <param name="errorRow">where</param>
        /// <param name="sym">what we have</param>
        /// <returns></returns>
        private string GetErrMsg(int errorRow, Symbol sym)
        {
            // expected symbol from terminals
            Set suggestedSyms = m_parsTable[errorRow].terminals;
            // moving down (exploring alternative productions)
            // and add them to expecting
            for (int row = errorRow - 1; row > -1; row--)
            {
                if (!m_parsTable[row].error)
                    suggestedSyms += m_parsTable[row].terminals;
                else
                    break;
            }

            string errMsg = "have  " + sym +
                            " expected " + suggestedSyms + " line " + m_currToken.lineno;

            return errMsg;
        }
    }
}