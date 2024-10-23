using Interpreter.Grammar;
using System;
using System.Collections.Generic;

namespace Interpreter.Lexer
{
    public class Lexer
    {
        private readonly string _input;
        private int _position;

        public Lexer(string input)
        {
            _input = input.ToLower();
            _position = 0;
        }

        public List<Token> Tokenize()
        {
            var tokens = new List<Token>();

            while (_position < _input.Length)
            {
                var currentChar = _input[_position];

                if (char.IsWhiteSpace(currentChar))
                {
                    _position++;
                    continue;
                }

                if (char.IsLetter(currentChar))
                {
                    var identifier = ReadIdentifier();
                    if (identifier == "for")
                    {
                        tokens.Add(new Token(TokenType.FOR, identifier));
                    }
                    else if (identifier == "while")
                    {
                        tokens.Add(new Token(TokenType.WHILE, identifier));
                    }
                    else if (identifier == "and")
                    {
                        tokens.Add(new Token(TokenType.AND, identifier));
                    }
                    else if (identifier == "or")
                    {
                        tokens.Add(new Token(TokenType.OR, identifier));
                    }
                    else if (identifier == "do")
                    {
                        tokens.Add(new Token(TokenType.DO, identifier));
                    }
                    else if (identifier == "loop")
                    {
                        tokens.Add(new Token(TokenType.LOOP, identifier));
                    }
                    else if (identifier == "until")
                    {
                        tokens.Add(new Token(TokenType.UNTIL, identifier));
                    }
                    else if (identifier == "end")
                    {
                        tokens.Add(new Token(TokenType.END, identifier));
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.ID, identifier));
                    }
                    continue;
                }

                if (char.IsDigit(currentChar))
                {
                    var number = ReadNumber();
                    tokens.Add(new Token(TokenType.CONST, number));
                    continue;
                }

                switch (currentChar)
                {
                    case '=':
                        if (_position + 1 < _input.Length && _input[_position + 1] == '=')
                        {
                            tokens.Add(new Token(TokenType.REL, "=="));
                            _position += 2;
                        }
                        else
                        {
                            tokens.Add(new Token(TokenType.ASSIGN, "="));
                            _position++;
                        }
                        break;
                    case '<':
                        if (_position + 1 < _input.Length && _input[_position + 1] == '=')
                        {
                            tokens.Add(new Token(TokenType.REL, "<="));
                            _position += 2;
                        }
                        else
                        {
                            tokens.Add(new Token(TokenType.REL, "<"));
                            _position++;
                        }
                        break;
                    case '>':
                        if (_position + 1 < _input.Length && _input[_position + 1] == '=')
                        {
                            tokens.Add(new Token(TokenType.REL, ">="));
                            _position += 2;
                        }
                        else
                        {
                            tokens.Add(new Token(TokenType.REL, ">"));
                            _position++;
                        }
                        break;
                    case '+':
                        if (_position + 1 < _input.Length && _input[_position + 1] == '+')
                        {
                            tokens.Add(new Token(TokenType.INCREMENT, "++"));
                            _position += 2;
                        }
                        else
                        {
                            tokens.Add(new Token(TokenType.PLUS, "+"));
                            _position++;
                        }
                        break;
                    case '-':
                        tokens.Add(new Token(TokenType.MINUS, "-"));
                        _position++;
                        break;
                    case '*':
                        tokens.Add(new Token(TokenType.MULT, "*"));
                        _position++;
                        break;
                    case '(':
                        tokens.Add(new Token(TokenType.OPENPAREN, "("));
                        _position++;
                        break;
                    case ')':
                        tokens.Add(new Token(TokenType.CLOSEPAREN, ")"));
                        _position++;
                        break;
                    case '{':
                        tokens.Add(new Token(TokenType.OPENBRACE, "{"));
                        _position++;
                        break;
                    case '}':
                        tokens.Add(new Token(TokenType.CLOSEBRACE, "}"));
                        _position++;
                        break;
                    case ';':
                        tokens.Add(new Token(TokenType.SEMICOLON, ";"));
                        _position++;
                        break;
                    case ':':
                        tokens.Add(new Token(TokenType.COLON, ":"));
                        _position++;
                        break;
                    case '/':
                        tokens.Add(new Token(TokenType.DIV, "/"));
                        _position++;
                        break;
                    default:
                        throw new Exception($"Неизвестный символ: {currentChar}");
                }
            }

            return tokens;
        }

        private string ReadIdentifier()
        {
            var start = _position;
            while (_position < _input.Length && char.IsLetterOrDigit(_input[_position]))
            {
                _position++;
            }

            return _input.Substring(start, _position - start);
        }

        private string ReadNumber()
        {
            var start = _position;
            while (_position < _input.Length && char.IsDigit(_input[_position]))
            {
                _position++;
            }

            return _input.Substring(start, _position - start);
        }
    }
}
