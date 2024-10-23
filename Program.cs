using Interpreter.Lexer;
using System;
using System.Collections.Generic;

namespace Interpreter
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Введите выражение для обработки или exit для выхода:");
                string input = Console.ReadLine();
                Console.WriteLine();

                if (input == "exit")
                {
                    break;
                }
                try
                {
                    var lexer = new Lexer.Lexer(input);
                    List<Token> tokens = lexer.Tokenize();

                    var parser = new Parser.Parser(tokens);
                    parser.ParseDoLoopUntil();

                    Console.WriteLine("Сгенерированный ПОЛИЗ:");
                    parser.PrintPostfix();

                    Console.WriteLine("Начало интерпретации ПОЛИЗа:");
                    var postfix = parser.GetPostfixForm();
                    postfix.Interpret();
                    postfix.PrintVariables();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }

                Console.WriteLine();
            }
        }
    }
}
