using LexicalAnalyzer.SymbolTable;
using System;

namespace LexicalAnalyzer
{
    public class Program
    {
        static void Main(string[] args)
        {
            TerminalSymbolTable.PrintTable();
            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("Введите строку для анализа или exit для завершения программы или menu для показа таблицы терминала:");
                string input = Console.ReadLine();
                Console.WriteLine();

                if (input.ToLower() == "exit")
                {
                    break;
                }
                else if (input.ToLower() == "menu")
                {
                    TerminalSymbolTable.PrintTable();
                }
                else
                {
                    var lexer = new Lexer.Lexer(input);
                    var tokens = lexer.Analyze();

                    foreach (var token in tokens)
                    {
                        Console.WriteLine(token);
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
