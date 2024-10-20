namespace LexicalAnalyzer.Grammar
{
    public enum TokenType
    {
        KeywordWhile,
        KeywordDo,
        KeywordEnd,
        KeywordAnd,
        KeywordOr,
        RelationalOperator,
        ArithmeticOperator,
        AssignmentOperator,
        Identifier,
        Constant,
        Unknown
    }
}
