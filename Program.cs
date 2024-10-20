using System;

namespace RcursiveDescentParser
{
    public class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Введите выражение или exit для выхода:");
                string input = Console.ReadLine();

                if (input.ToLower() == "exit")
                {
                    Console.WriteLine();
                    break;
                }
                try
                {
                    var lexer = new Lexer.Lexer(input);
                    var tokens = lexer.Tokenize();

                    var parser = new Parser.Parser(tokens);

                    parser.ParseStatement();
                    Console.WriteLine("Анализ успешно завершен.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.WriteLine();
            }
        }
    }
}
