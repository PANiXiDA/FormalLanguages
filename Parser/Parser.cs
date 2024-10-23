using Interpreter.Grammar;
using Interpreter.Postfix;
using Interpreter.Lexer;
using System;
using System.Collections.Generic;

namespace Interpreter.Parser
{
    public class Parser
    {
        private readonly List<Token> _tokens;
        private PostfixForm _postfix;
        private int _currentTokenIndex;

        #region Constructor and Helpers
        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _currentTokenIndex = 0;
            _postfix = new PostfixForm();
        }

        private Token CurrentToken
        {
            get
            {
                if (HasMoreTokens())
                {
                    return _tokens[_currentTokenIndex];
                }
                else
                {
                    throw new Exception("Ошибка: неожиданный конец выражения.");
                }
            }
        }

        private bool HasMoreTokens()
        {
            return _currentTokenIndex < _tokens.Count;
        }

        private void Match(TokenType expectedType)
        {
            if (CurrentToken.Type == expectedType)
            {
                _currentTokenIndex++;
            }
            else
            {
                throw new Exception($"Ошибка: ожидалось {expectedType}, но найдено {CurrentToken.Type}");
            }
        }
        #endregion

        #region Assignment and Expression Parsing
        public void ParseAssignment()
        {
            if (CurrentToken.Type == TokenType.ID)
            {
                string varName = CurrentToken.Value;
                Match(TokenType.ID);
                Match(TokenType.ASSIGN);

                ParseExpression();

                _postfix.WriteCmd(ECmd.SET);
                _postfix.PushVar(varName);

                if (CurrentToken.Type == TokenType.SEMICOLON)
                {
                    Match(TokenType.SEMICOLON);
                }
                else
                {
                    throw new Exception("Ошибка: Ожидалась ';' после присваивания.");
                }
            }
        }

        private void ParseExpression()
        {
            ParseTerm();
            while (CurrentToken.Type == TokenType.PLUS || CurrentToken.Type == TokenType.MINUS)
            {
                if (CurrentToken.Type == TokenType.PLUS)
                {
                    Match(TokenType.PLUS);
                    ParseTerm();
                    _postfix.WriteCmd(ECmd.ADD);
                }
                else if (CurrentToken.Type == TokenType.MINUS)
                {
                    Match(TokenType.MINUS);
                    ParseTerm();
                    _postfix.WriteCmd(ECmd.SUB);
                }
            }
        }

        private void ParseTerm()
        {
            ParseFactor();
            while (CurrentToken.Type == TokenType.MULT || CurrentToken.Type == TokenType.DIV)
            {
                if (CurrentToken.Type == TokenType.MULT)
                {
                    Match(TokenType.MULT);
                    ParseFactor();
                    _postfix.WriteCmd(ECmd.MUL);
                }
                else if (CurrentToken.Type == TokenType.DIV)
                {
                    Match(TokenType.DIV);
                    ParseFactor();
                    _postfix.WriteCmd(ECmd.DIV);
                }
            }
        }

        private void ParseFactor()
        {
            if (CurrentToken.Type == TokenType.ID)
            {
                _postfix.PushVar(CurrentToken.Value);
                Match(TokenType.ID);
            }
            else if (CurrentToken.Type == TokenType.CONST)
            {
                _postfix.PushConst(int.Parse(CurrentToken.Value));
                Match(TokenType.CONST);
            }
            else
            {
                throw new Exception("Ожидалось арифметическое выражение");
            }
        }
        #endregion

        #region Logical and Relational Expressions
        private void ParseLogicalExpression()
        {
            ParseRelationalExpression();

            while (CurrentToken.Type == TokenType.AND || CurrentToken.Type == TokenType.OR)
            {
                if (CurrentToken.Type == TokenType.AND)
                {
                    Match(TokenType.AND);
                    ParseRelationalExpression();
                    _postfix.WriteCmd(ECmd.AND);
                }
                else if (CurrentToken.Type == TokenType.OR)
                {
                    Match(TokenType.OR);
                    ParseRelationalExpression();
                    _postfix.WriteCmd(ECmd.OR);
                }

                if (_currentTokenIndex == _tokens.Count)
                {
                    break;
                }
            }
        }

        private void ParseRelationalExpression()
        {
            ParseOperand();

            if (CurrentToken.Type == TokenType.REL)
            {
                string relOp = CurrentToken.Value;
                Match(TokenType.REL);

                ParseOperand();

                switch (relOp)
                {
                    case ">":
                        _postfix.WriteCmd(ECmd.CMPG);
                        break;
                    case "<":
                        _postfix.WriteCmd(ECmd.CMPL);
                        break;
                    case ">=":
                        _postfix.WriteCmd(ECmd.CMPGE);
                        break;
                    case "<=":
                        _postfix.WriteCmd(ECmd.CMPLE);
                        break;
                    case "==":
                        _postfix.WriteCmd(ECmd.CMPE);
                        break;
                    default:
                        throw new Exception($"Неизвестная операция сравнения: {relOp}");
                }
            }
        }

        private void ParseOperand()
        {
            if (CurrentToken.Type == TokenType.ID)
            {
                _postfix.PushVar(CurrentToken.Value);
                Match(TokenType.ID);
            }
            else if (CurrentToken.Type == TokenType.CONST)
            {
                _postfix.PushConst(int.Parse(CurrentToken.Value));
                Match(TokenType.CONST);
            }
            else
            {
                throw new Exception("Ошибка: ожидался операнд");
            }
        }
        #endregion

        #region Loop Parsing
        public void ParseDoLoopUntil()
        {
            int startLoopIndex = _postfix.GetCurrentAddress();

            Match(TokenType.DO);

            while (HasMoreTokens() && CurrentToken.Type != TokenType.LOOP)
            {
                ParseAssignment();
            }

            Match(TokenType.LOOP);
            Match(TokenType.UNTIL);

            ParseLogicalExpression();

            int jzIndex = _postfix.WriteCmd(ECmd.JZ);

            _postfix.WriteCmd(ECmd.JMP);
            _postfix.SetCmdPtr(jzIndex, _postfix.GetCurrentAddress() + 1);
        }
        #endregion

        public void PrintPostfix()
        {
            _postfix.PrintPostfix();
        }

        public PostfixForm GetPostfixForm()
        {
            return _postfix;
        }
    }
}
