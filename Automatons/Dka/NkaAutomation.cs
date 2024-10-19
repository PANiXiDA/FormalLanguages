using System;
using System.Collections.Generic;
using System.Linq;
using Task0.Automation.Models;

namespace Task0.Automation.Automatons.Nka
{
    public class NkaAutomation
    {
        private HashSet<string> _currentStates;
        private HashSet<string> _finalStates;
        private Dictionary<string, Dictionary<char, List<string>>> _transitions;
        private const string UndefinedState = "~";
        private List<string> _states;
        private List<char> _alphabet;

        public NkaAutomation(NkaDescription nfaDescription)
        {
            _currentStates = new HashSet<string> { nfaDescription.InitialState };
            _finalStates = nfaDescription.FinalStates.ToHashSet();
            _transitions = nfaDescription.Transitions;
            _states = nfaDescription.States;
            _alphabet = nfaDescription.Alphabet;
        }

        public void DisplayAutomatonInfo()
        {
            Console.WriteLine("Недетерминированный КА");
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

            foreach (var symbol in input)
            {
                if (!_alphabet.Contains(symbol))
                {
                    Console.WriteLine($"Ошибка: символ '{symbol}' не входит в алфавит. Обработка завершена.");
                    Console.WriteLine();
                    return;
                }

                Console.WriteLine($"Обрабатывается символ: '{symbol}'");
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

                _currentStates = nextStates;
                Console.WriteLine($"Новые состояния: {string.Join(", ", _currentStates)}");
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

        public void NkaToDka()
        {
            Console.WriteLine("\nПреобразование НКА в ДКА:\n");

            var dfaStates = new HashSet<HashSet<string>> { new HashSet<string> { _states.First() } };
            var dfaTransitions = new Dictionary<string, Dictionary<char, string>>();
            var dfaFinalStates = new HashSet<string>();
            var processedStates = new HashSet<HashSet<string>>();
            var initialState = _states.First();

            while (dfaStates.Any(state => !processedStates.Contains(state)))
            {
                var currentState = dfaStates.First(state => !processedStates.Contains(state));
                processedStates.Add(currentState);
                string stateName = FormatStateName(currentState);

                dfaTransitions[stateName] = new Dictionary<char, string>();

                foreach (var symbol in _alphabet)
                {
                    var nextState = new HashSet<string>();

                    foreach (var subState in currentState)
                    {
                        if (_transitions[subState].TryGetValue(symbol, out var nextSubStates))
                        {
                            nextState.UnionWith(nextSubStates);
                        }
                    }

                    if (nextState.Any())
                    {
                        string nextStateName = FormatStateName(nextState);
                        dfaTransitions[stateName][symbol] = nextStateName;

                        if (!dfaStates.Any(s => s.SetEquals(nextState)))
                        {
                            dfaStates.Add(nextState);
                        }
                    }
                    else
                    {
                        dfaTransitions[stateName][symbol] = "~";
                    }
                }

                if (currentState.Any(state => _finalStates.Contains(state)))
                {
                    dfaFinalStates.Add(stateName);
                }
            }

            Console.WriteLine("Детерминированный КА");
            Console.WriteLine($"Состояния ДКА: {string.Join(" ", dfaTransitions.Keys)}");
            Console.WriteLine($"Алфавит: {string.Join(" ", _alphabet)}");
            Console.WriteLine($"Начальное состояние: {FormatStateName(new HashSet<string> { initialState })}");
            Console.WriteLine($"Финальные состояния ДКА: {string.Join(" ", dfaFinalStates)}");
            Console.WriteLine("Таблица переходов ДКА:");

            Console.Write("".PadLeft(23) + ":   |");
            foreach (var symbol in _alphabet)
            {
                Console.Write($"  {symbol}".PadRight(20) + "||");
            }
            Console.WriteLine();

            foreach (var state in dfaTransitions.Keys)
            {
                string stateMarker = (state == FormatStateName(new HashSet<string> { initialState })) ? "->" : "  ";
                if (dfaFinalStates.Contains(state))
                {
                    stateMarker = "* ";
                }

                Console.Write($"{stateMarker}{state.PadRight(20)} :   |");

                foreach (var symbol in _alphabet)
                {
                    if (dfaTransitions[state].TryGetValue(symbol, out var nextState))
                    {
                        if (nextState == "~")
                        {
                            Console.Write($" {"~".PadRight(18)} ||");
                        }
                        else
                        {
                            Console.Write($" {nextState.PadRight(18)} ||");
                        }
                    }
                    else
                    {
                        Console.Write($" {"~".PadRight(18)} ||");
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine("--------------------------------------------------");
        }



        private string FormatStateName(HashSet<string> states)
        {
            return states.Count > 1 ? "{" + string.Join(",", states.OrderBy(s => s)) + "}" : string.Join(",", states);
        }

        private void ResetAutomaton()
        {
            _currentStates = new HashSet<string> { _states.First() };
        }
    }
}
