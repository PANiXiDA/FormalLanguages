namespace LexicalAnalyzer.Grammar
{
    public static class GrammarRules
    {
        public static bool IsKeyword(string value)
        {
            return value == "while" || value == "do" || value == "end" || value == "and" || value == "or";
        }

        public static bool IsRelationalOperator(char c)
        {
            return c == '<' || c == '=' || c == '>';
        }

        public static bool IsArithmeticOperator(char c)
        {
            return c == '+' || c == '-';
        }

        public static bool IsIdentifierStart(char c)
        {
            return char.IsLetter(c);
        }

        public static bool IsIdentifierPart(char c)
        {
            return char.IsLetterOrDigit(c);
        }
    }
}
