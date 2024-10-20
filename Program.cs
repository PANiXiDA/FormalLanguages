using SemanticAnalyzer.Lexer;
using SemanticAnalyzer.Parser;
using System;
using System.Collections.Generic;

namespace SemanticAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите выражение для обработки:");
            string input = Console.ReadLine();

            try
            {
                var lexer = new Lexer.Lexer(input);
                List<Token> tokens = lexer.Tokenize();

                var parser = new Parser.Parser(tokens);
                parser.ParseDoLoopUntil();

                Console.WriteLine("Сгенерированный ПОЛИЗ:");
                parser.PrintPostfix();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}
