using System;
using System.Collections.Generic;
using System.Linq;
using Task0.Automation.Models;

namespace Task0.Automation.Automatons.Dka
{
    public class NkaEAutomation
    {
        private HashSet<string> _currentStates;
        private HashSet<string> _finalStates;
        private Dictionary<string, Dictionary<char, List<string>>> _transitions;
        private const string UndefinedState = "~";
        private List<string> _states;
        private List<char> _alphabet;

        public NkaEAutomation(NkaDescription nfaDescription)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            _currentStates = new HashSet<string> { nfaDescription.InitialState };
            _finalStates = nfaDescription.FinalStates.ToHashSet();
            _transitions = nfaDescription.Transitions;
            _states = nfaDescription.States;
            _alphabet = nfaDescription.Alphabet;
        }

        public void DisplayAutomatonInfo()
        {
            Console.WriteLine("Недетерминированный КА с ε переходами");
            Console.WriteLine($"Состояния: {string.Join(" ", _states)}");
            Console.WriteLine($"Алфавит: {string.Join(" ", _alphabet)}");
            Console.WriteLine($"Начальное состояние: {string.Join(", ", _currentStates)}");
            Console.WriteLine($"Финальное(ые) состояние(я): {string.Join(" ", _finalStates)}");
            Console.WriteLine();
            Console.WriteLine("Таблица переходов автомата:");

            Console.Write("     :   |");
            foreach (var symbol in _alphabet)
            {
                Console.Write($"  {symbol}".PadRight(12) + "||");
            }
            Console.WriteLine();

            foreach (var state in _states)
            {
                if (state == _states.First())
                {
                    Console.Write($"->{state} :   |");
                }
                else
                {
                    string stateMarker = _finalStates.Contains(state) ? " *" : "  ";
                    Console.Write($"{stateMarker}{state} :   |");
                }

                foreach (var symbol in _alphabet)
                {
                    if (_transitions[state].TryGetValue(symbol, out var nextStates) && nextStates.Any())
                    {
                        Console.Write($" {string.Join(",", nextStates).PadRight(10)} ||");
                    }
                    else
                    {
                        Console.Write("  ~".PadRight(12) + "||");
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine("--------------------------------------------------");
        }

        public void ProcessInput(string input)
        {
            Console.WriteLine($"Начальные состояния: {string.Join(", ", _currentStates)}");

            int step = 1;

            foreach (var symbol in input)
            {
                if (!_alphabet.Contains(symbol))
                {
                    Console.WriteLine($"Ошибка: символ '{symbol}' не входит в алфавит. Обработка завершена.");
                    Console.WriteLine();
                    return;
                }

                Console.WriteLine($"\nТакт №{step}. Считан символ '{symbol}'");;

                var nextStates = new HashSet<string>();

                foreach (var state in _currentStates)
                {
                    if (_transitions[state].TryGetValue(symbol, out var possibleNextStates))
                    {
                        nextStates.UnionWith(possibleNextStates);
                    }
                }

                if (!nextStates.Any())
                {
                    Console.WriteLine($"Переход невозможен, пустое множество состояний.");
                    Console.WriteLine();
                    _currentStates = new HashSet<string> { UndefinedState };
                    break;
                }

                var closure = new HashSet<string>(nextStates);
                foreach (var state in nextStates)
                {
                    var stateClosure = EpsilonClosure(new HashSet<string> { state });
                    if (!stateClosure.SetEquals(new HashSet<string> { state }))
                    {
                        Console.WriteLine($"Состояние {state} образует следующее замыкание: {state}, {string.Join(", ", stateClosure.Except(new HashSet<string> { state }))}");
                        closure.UnionWith(stateClosure);
                    }
                }

                _currentStates = closure;
                Console.WriteLine($" - Текущее(ие) состояние(ия): {string.Join(", ", _currentStates)}");

                step++;
            }

            if (_currentStates.All(state => state != UndefinedState))
            {
                Console.WriteLine($"\nОбработка завершена. Слово: {input}");
                Console.WriteLine($"Финальные состояния: {string.Join(", ", _currentStates)}");

                var finalStatesInCurrent = _currentStates.Where(state => _finalStates.Contains(state)).ToList();

                if (finalStatesInCurrent.Any())
                {
                    Console.WriteLine($"Состояния, входящие в число финальных: {string.Join(", ", finalStatesInCurrent)}");
                }
                else
                {
                    Console.WriteLine("Ни одно из состояний не входит в число финальных.");
                }
                Console.WriteLine();
            }

            ResetAutomaton();
        }

        public void ConvertToNKA()
        {
            Console.WriteLine("\nПреобразование НКА с ε-переходами в обычный НКА:\n");

            var newTransitions = new Dictionary<string, Dictionary<char, List<string>>>();

            foreach (var state in _states)
            {
                var epsilonClosure = EpsilonClosure(new HashSet<string> { state });

                var transitionsForState = new Dictionary<char, List<string>>();
                foreach (var symbol in _alphabet.Where(c => c != 'ε'))
                {
                    var reachableStates = new HashSet<string>();

                    foreach (var closureState in epsilonClosure)
                    {
                        if (_transitions[closureState].TryGetValue(symbol, out var nextStates))
                        {
                            foreach (var nextState in nextStates)
                            {
                                var nextEpsilonClosure = EpsilonClosure(new HashSet<string> { nextState });
                                reachableStates.UnionWith(nextEpsilonClosure);
                            }
                        }
                    }

                    if (reachableStates.Any())
                    {
                        transitionsForState[symbol] = reachableStates.ToList();
                    }
                }

                newTransitions[state] = transitionsForState;
            }

            var newFinalStates = new HashSet<string>(_finalStates);
            foreach (var state in _states)
            {
                var epsilonClosure = EpsilonClosure(new HashSet<string> { state });
                if (epsilonClosure.Any(s => _finalStates.Contains(s)))
                {
                    newFinalStates.Add(state);
                }
            }

            Console.WriteLine("Недетерминированный КА (без ε-переходов)");
            Console.WriteLine($"Состояния: {string.Join(" ", _states)}");
            Console.WriteLine($"Алфавит: {string.Join(" ", _alphabet.Where(c => c != 'ε'))}");
            Console.WriteLine($"Начальное состояние: {string.Join(", ", _currentStates)}");
            Console.WriteLine($"Финальное(ые) состояние(я): {string.Join(" ", newFinalStates)}");
            Console.WriteLine();
            Console.WriteLine("Таблица переходов автомата:");

            Console.Write("     :   |");
            foreach (var symbol in _alphabet.Where(c => c != 'ε'))
            {
                Console.Write($"  {symbol}".PadRight(17) + "||");
            }
            Console.WriteLine();

            foreach (var state in _states)
            {
                string stateMarker = newFinalStates.Contains(state) ? " *" : "  ";
                if (state == _currentStates.First())
                {
                    stateMarker = "->";
                }

                Console.Write($"{stateMarker}{state} :   |");

                foreach (var symbol in _alphabet.Where(c => c != 'ε'))
                {
                    if (newTransitions[state].TryGetValue(symbol, out var nextStates) && nextStates.Any())
                    {
                        Console.Write($" {string.Join(",", nextStates).PadRight(15)} ||");
                    }
                    else
                    {
                        Console.Write("  ~".PadRight(17) + "||");
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine("--------------------------------------------------");
        }

        private HashSet<string> EpsilonClosure(HashSet<string> states)
        {
            var closure = new HashSet<string>(states);
            var stack = new Stack<string>(states);

            while (stack.Count > 0)
            {
                var state = stack.Pop();
                if (_transitions[state].TryGetValue('ε', out var epsilonStates))
                {
                    foreach (var epsilonState in epsilonStates)
                    {
                        if (!closure.Contains(epsilonState))
                        {
                            closure.Add(epsilonState);
                            stack.Push(epsilonState);
                        }
                    }
                }
            }

            return closure;
        }

        private void ResetAutomaton()
        {
            _currentStates = new HashSet<string> { _states.First() };
        }
    }
}
