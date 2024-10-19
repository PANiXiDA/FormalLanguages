using System;
using Task0.Automation.Automatons.Dka;
using Task0.Automation.Automatons.Nka;
using Task0.Automation.Helpers;

namespace Task0.Automation
{
    public class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Меню:");
                Console.WriteLine("1) DKA (Детерминированный КА)");
                Console.WriteLine("2) NKA (Недетерминированный КА)");
                Console.WriteLine("3) NKA с ε-переходами (Недетерминированный КА с эпсилон переходами)");
                Console.WriteLine("4) Exit (Выйти из приложения)");
                Console.Write("\nВыберите автомат (1, 2, 3) или введите '4' для выхода: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        var dfaDescription = DkaReader.LoadFromJson();
                        var dfa = new DkaAutomation(dfaDescription);

                        while (true)
                        {
                            dfa.DisplayAutomatonInfo();

                            Console.WriteLine("\nВведите слово для обработки (или 'exit' для завершения, 'menu' для возврата в меню):");
                            string input = Console.ReadLine();
                            Console.WriteLine();

                            if (input.ToLower() == "exit")
                            {
                                Console.WriteLine("Программа завершена.");
                                return;
                            }

                            if (input.ToLower() == "menu")
                            {
                                break;
                            }

                            dfa.ProcessInput(input);
                        }
                        break;

                    case "2":
                        var nkaDescription = NkaReader.LoadFromJson();
                        var nka = new NkaAutomation(nkaDescription);

                        while (true)
                        {
                            nka.DisplayAutomatonInfo();

                            Console.WriteLine("\nВведите слово для обработки (или 'exit' для завершения, 'toDka' для преобразования в DKA, 'menu' для возврата в меню):");
                            string input = Console.ReadLine();

                            if (input.ToLower() == "exit")
                            {
                                Console.WriteLine("Программа завершена.");
                                return;
                            }

                            if (input.ToLower() == "menu")
                            {
                                break;
                            }

                            if (input.ToLower() == "todka")
                            {
                                nka.NkaToDka();
                            }
                            else
                            {
                                nka.ProcessInput(input);
                            }
                        }
                        break;

                    case "3":
                        var nkaEDescription = NkaEReader.LoadFromJson();
                        var nkaE = new NkaEAutomation(nkaEDescription);

                        while (true)
                        {
                            nkaE.DisplayAutomatonInfo();

                            Console.WriteLine("\nВведите слово для обработки (или 'exit' для завершения, 'toNka' для преобразования в NKA, 'menu' для возврата в меню):");
                            string input = Console.ReadLine();

                            if (input.ToLower() == "exit")
                            {
                                Console.WriteLine("Программа завершена.");
                                return;
                            }

                            if (input.ToLower() == "menu")
                            {
                                break;
                            }

                            if (input.ToLower() == "tonka")
                            {
                                nkaE.ConvertToNKA();
                            }
                            else
                            {
                                nkaE.ProcessInput(input);
                            }
                        }
                        break;

                    case "4":
                        Console.WriteLine("Программа завершена.");
                        return;

                    default:
                        Console.WriteLine("Неправильный ввод. Пожалуйста, выберите 1, 2, 3 или 4.");
                        break;
                }
            }
        }
    }
}
