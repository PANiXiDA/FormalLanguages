using System;
using System.Collections.Generic;

namespace LexicalAnalyzer.SymbolTable
{
    public class TerminalSymbol
    {
        public int Index { get; set; }
        public string Symbol { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Comment { get; set; }
    }

    public class TerminalSymbolTable
    {
        public static List<TerminalSymbol> GetTable()
        {
            return new List<TerminalSymbol>
            {
                new TerminalSymbol { Index = 0, Symbol = "while", Category = "Ключевое слово", Type = "KeywordWhile", Comment = "Начало заголовка цикла" },
                new TerminalSymbol { Index = 1, Symbol = "do", Category = "Ключевое слово", Type = "KeywordDo", Comment = "Начало тела цикла" },
                new TerminalSymbol { Index = 2, Symbol = "end", Category = "Ключевое слово", Type = "KeywordEnd", Comment = "Конец тела цикла" },
                new TerminalSymbol { Index = 3, Symbol = "and", Category = "Ключевое слово", Type = "KeywordAnd", Comment = "Логическая операция 'И'" },
                new TerminalSymbol { Index = 4, Symbol = "or", Category = "Ключевое слово", Type = "KeywordOr", Comment = "Логическая операция 'ИЛИ'" },
                new TerminalSymbol { Index = 5, Symbol = "<", Category = "Специальный символ", Type = "RelationalOperator", Comment = "Операция сравнения 'меньше'" },
                new TerminalSymbol { Index = 6, Symbol = "<=", Category = "Специальный символ", Type = "RelationalOperator", Comment = "Операция сравнения 'меньше или равно'" },
                new TerminalSymbol { Index = 7, Symbol = "<>", Category = "Специальный символ", Type = "RelationalOperator", Comment = "Операция сравнения 'неравно'" },
                new TerminalSymbol { Index = 8, Symbol = "==", Category = "Специальный символ", Type = "RelationalOperator", Comment = "Операция сравнения 'равно'" },
                new TerminalSymbol { Index = 9, Symbol = "=", Category = "Специальный символ", Type = "AssignmentOperator", Comment = "Операция присваивания" },
                new TerminalSymbol { Index = 10, Symbol = "+", Category = "Специальный символ", Type = "ArithmeticOperator", Comment = "Операция сложения" },
                new TerminalSymbol { Index = 11, Symbol = "-", Category = "Специальный символ", Type = "ArithmeticOperator", Comment = "Операция вычитания" },
                new TerminalSymbol { Index = 12, Symbol = ">", Category = "Специальный символ", Type = "RelationalOperator", Comment = "Операция сравнения 'больше'" },
                new TerminalSymbol { Index = 13, Symbol = ">=", Category = "Специальный символ", Type = "RelationalOperator", Comment = "Операция сравнения 'больше или равно'" }
            };
        }

        public static void PrintTable()
        {
            var table = GetTable();
            Console.WriteLine("{0,-5} {1,-10} {2,-20} {3,-25} {4,-40}", "Индекс", "Символ", "Категория", "Тип", "Комментарий");
            foreach (var symbol in table)
            {
                Console.WriteLine("{0,-5} {1,-10} {2,-20} {3,-25} {4,-40}",
                                  symbol.Index, symbol.Symbol, symbol.Category, symbol.Type, symbol.Comment);
            }
        }
    }
}
