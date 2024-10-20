using LexicalAnalyzer.Grammar;

namespace LexicalAnalyzer.Lexer
{
    public class Token
    {
        public string Value { get; }
        public TokenType Type { get; }
        public int Position { get; }

        public Token(string value, TokenType type, int position)
        {
            Value = value;
            Type = type;
            Position = position;
        }

        public override string ToString()
        {
            return $"{Type}('{Value}') at position {Position}";
        }
    }
}
