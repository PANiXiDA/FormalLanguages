using RcursiveDescentParser.Grammar;
using RcursiveDescentParser.Lexer;
using System;
using System.Collections.Generic;

namespace RcursiveDescentParser.Parser
{
    public class Parser
    {
        private readonly List<Token> _tokens;
        private int _currentTokenIndex;

        #region Constructor and Helpers
        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _currentTokenIndex = 0;
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
            if (_currentTokenIndex >= _tokens.Count)
            {
                throw new Exception($"Ошибка: ожидался {expectedType}, но входная строка завершилась.");
            }
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
            if (HasMoreTokens() && CurrentToken.Type == TokenType.ID)
            {
                Match(TokenType.ID);
                Match(TokenType.ASSIGN);
                ParseExpression();

                if (HasMoreTokens() && CurrentToken.Type == TokenType.SEMICOLON)
                {
                    Match(TokenType.SEMICOLON);
                }
                else
                {
                    throw new Exception("Ошибка: ожидается ';' в конце выражения.");
                }
            }
            else
            {
                throw new Exception("Ошибка: ожидался идентификатор перед присваиванием.");
            }
        }

        private void ParseExpression()
        {
            ParseTerm();

            while (HasMoreTokens() && CurrentToken.Type == TokenType.PLUS)
            {
                Match(TokenType.PLUS);
                ParseTerm();
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
                }
                else if (CurrentToken.Type == TokenType.DIV)
                {
                    Match(TokenType.DIV);
                }

                ParseFactor();
            }
        }

        private void ParseFactor()
        {
            if (HasMoreTokens() && CurrentToken.Type == TokenType.ID)
            {
                Match(TokenType.ID);
                if (HasMoreTokens() && CurrentToken.Type == TokenType.OPENPAREN)
                {
                    Match(TokenType.OPENPAREN);
                    ParseParameters();
                    Match(TokenType.CLOSEPAREN);
                }
            }
            else if (HasMoreTokens() && CurrentToken.Type == TokenType.CONST)
            {
                Match(TokenType.CONST);
            }
            else if (HasMoreTokens() && CurrentToken.Type == TokenType.OPENPAREN)
            {
                Match(TokenType.OPENPAREN);
                ParseExpression();
                Match(TokenType.CLOSEPAREN);
            }
            else
            {
                throw new Exception("Ошибка: ожидался идентификатор, константа или выражение в скобках");
            }
        }

        private void ParseParameters()
        {
            ParseExpression();

            while (CurrentToken.Type == TokenType.COLON)
            {
                Match(TokenType.COLON);
                ParseExpression();
            }
        }

        public void ParseDeclarationOrAssignment()
        {
            if (HasMoreTokens() && IsTypeKeyword(CurrentToken))
            {
                Match(CurrentToken.Type);
                Match(TokenType.ID);
                Match(TokenType.ASSIGN);
                ParseExpression();

                if (HasMoreTokens() && CurrentToken.Type == TokenType.SEMICOLON)
                {
                    Match(TokenType.SEMICOLON);
                }
                else
                {
                    throw new Exception("Ошибка: ожидается ';' в конце объявления переменной.");
                }
            }
            else
            {
                ParseAssignment();
            }
        }

        private bool IsTypeKeyword(Token token)
        {
            return token.Value == "int" || token.Value == "float" || token.Value == "double";
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
                }
                else
                {
                    Match(TokenType.OR);
                }

                ParseRelationalExpression();
            }
        }

        private void ParseRelationalExpression()
        {
            ParseOperand();

            if (CurrentToken.Type == TokenType.REL)
            {
                Match(TokenType.REL);
                ParseOperand();
            }
        }

        private void ParseOperand()
        {
            if (CurrentToken.Type == TokenType.ID)
            {
                Match(TokenType.ID);
            }
            else if (CurrentToken.Type == TokenType.CONST)
            {
                Match(TokenType.CONST);
            }
            else
            {
                throw new Exception("Ошибка: ожидался операнд");
            }
        }
        #endregion

        #region Loop and Statement Parsing
        public void ParseDoWhileLoop()
        {
            Match(TokenType.DO);

            while (CurrentToken.Type != TokenType.LOOP && HasMoreTokens())
            {
                ParseStatement();
            }

            Match(TokenType.LOOP);
            Match(TokenType.UNTIL);

            ParseLogicalExpression();
        }

        public void ParseForLoop()
        {
            if (CurrentToken.Type == TokenType.FOR)
            {
                Match(TokenType.FOR);
                Match(TokenType.OPENPAREN);

                ParseDeclarationOrAssignment();

                ParseRelationalExpression();

                Match(TokenType.SEMICOLON);

                ParseIncrement();

                Match(TokenType.CLOSEPAREN);

                ParseStatement();
            }
            else
            {
                throw new Exception("Ошибка: ожидался оператор for.");
            }
        }

        public void ParseWhileLoop()
        {
            if (CurrentToken.Type == TokenType.WHILE)
            {
                Match(TokenType.WHILE);
                ParseLogicalExpression();

                Match(TokenType.DO);

                ParseStatement();

                Match(TokenType.END);
            }
            else
            {
                throw new Exception("Ошибка: ожидался оператор while.");
            }
        }

        private void ParseIncrement()
        {
            if (CurrentToken.Type == TokenType.ID)
            {
                Match(TokenType.ID);

                if (HasMoreTokens() && CurrentToken.Type == TokenType.INCREMENT)
                {
                    Match(TokenType.INCREMENT);
                }
                else if (HasMoreTokens() && CurrentToken.Type == TokenType.ASSIGN)
                {
                    Match(TokenType.ASSIGN);
                    ParseExpression();
                }
                else
                {
                    throw new Exception("Ошибка: ожидался инкремент или присваивание.");
                }
            }
            else
            {
                throw new Exception("Ошибка: ожидался идентификатор в инкременте цикла for.");
            }
        }

        public void ParseStatement()
        {
            if (CurrentToken.Type == TokenType.ID)
            {
                ParseAssignment();
            }
            else if (CurrentToken.Type == TokenType.FOR)
            {
                ParseForLoop();
            }
            else if (CurrentToken.Type == TokenType.WHILE)
            {
                ParseWhileLoop();
            }
            else if (CurrentToken.Type == TokenType.DO)
            {
                ParseDoWhileLoop();
            }
            else if (CurrentToken.Type == TokenType.OPENBRACE)
            {
                Match(TokenType.OPENBRACE);
                while (HasMoreTokens() && CurrentToken.Type != TokenType.CLOSEBRACE)
                {
                    ParseStatement();
                }
                Match(TokenType.CLOSEBRACE);
            }
            else
            {
                throw new Exception("Ошибка: неизвестный оператор.");
            }
        }
        #endregion
    }
}
