using System;
using System.Collections.Generic;
using LexicalAnalyzer.Grammar;

namespace LexicalAnalyzer.Lexer
{
    public class Lexer
    {
        private readonly string _input;
        private int _position;

        public Lexer(string input)
        {
            _input = input;
            _position = 0;
        }

        public List<Token> Analyze()
        {
            var tokens = new List<Token>();
            while (_position < _input.Length)
            {
                if (char.IsWhiteSpace(CurrentChar()))
                {
                    _position++;
                }
                else if (IsLetter(CurrentChar()))
                {
                    var identifier = ReadWhile(IsLetterOrDigit);
                    tokens.Add(new Token(identifier, IdentifyKeyword(identifier), _position));
                }
                else if (char.IsDigit(CurrentChar()))
                {
                    var constant = ReadWhile(char.IsDigit);
                    tokens.Add(new Token(constant, TokenType.Constant, _position));
                }
                else if (CurrentChar() == '<' || CurrentChar() == '=')
                {
                    var symbol = ReadWhile(c => c == '<' || c == '=');
                    tokens.Add(new Token(symbol, TokenType.RelationalOperator, _position));
                }
                else if (CurrentChar() == '+' || CurrentChar() == '-')
                {
                    var symbol = ReadWhile(c => c == '+' || c == '-');
                    tokens.Add(new Token(symbol, TokenType.ArithmeticOperator, _position));
                }
                else if (CurrentChar() == '/' || CurrentChar() == '*')
                {
                    var symbol = ReadWhile(c => c == '/' || c == '*');
                    tokens.Add(new Token(symbol, TokenType.AssignmentOperator, _position));
                }
                else
                {
                    tokens.Add(new Token(CurrentChar().ToString(), TokenType.Unknown, _position));
                    _position++;
                }
            }

            return tokens;
        }

        private char CurrentChar()
        {
            return _input[_position];
        }

        private string ReadWhile(Func<char, bool> condition)
        {
            var start = _position;
            while (_position < _input.Length && condition(_input[_position]))
            {
                _position++;
            }

            return _input.Substring(start, _position - start);
        }

        private bool IsLetter(char ch) => char.IsLetter(ch);

        private bool IsLetterOrDigit(char ch) => char.IsLetterOrDigit(ch);

        private TokenType IdentifyKeyword(string value)
        {
            switch (value)
            {
                case "while": return TokenType.KeywordWhile;
                case "do": return TokenType.KeywordDo;
                case "end": return TokenType.KeywordEnd;
                case "and": return TokenType.KeywordAnd;
                case "or": return TokenType.KeywordOr;
                default: return TokenType.Identifier;
            }
        }
    }
}
