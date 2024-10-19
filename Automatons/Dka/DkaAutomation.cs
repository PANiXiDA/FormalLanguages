using System;
using System.Collections.Generic;
using System.Linq;
using Task0.Automation.Models;

namespace Task0.Automation.Automatons.Dka
{
    public class DkaAutomation
    {
        private string _currentState;
        private HashSet<string> _finalStates;
        private Dictionary<string, Dictionary<char, string>> _transitions;
        private const string UndefinedState = "~";
        private List<string> _states;
        private List<char> _alphabet;

        public DkaAutomation(DkaDescription dfaDescription)
        {
            _currentState = dfaDescription.InitialState;
            _finalStates = dfaDescription.FinalStates.ToHashSet();
            _transitions = dfaDescription.Transitions;
            _states = dfaDescription.States;
            _alphabet = dfaDescription.Alphabet;
        }

        public void DisplayAutomatonInfo()
        {
            Console.WriteLine("Детерминированный КА");
            Console.WriteLine($"Состояния: {string.Join(" ", _states)}");
            Console.WriteLine($"Алфавит: {string.Join(" ", _alphabet)}");
            Console.WriteLine($"Начальное состояние: {_currentState}");
            Console.WriteLine($"Финальное(ые) состояние(я): {string.Join(" ", _finalStates)}");
            Console.WriteLine();
            Console.WriteLine("Таблица переходов автомата:");

            Console.Write("     :   |");
            foreach (var symbol in _alphabet)
            {
                Console.Write($"  {symbol} ||");
            }
            Console.WriteLine();

            foreach (var state in _states)
            {
                string stateMarker = _finalStates.Contains(state) ? " *" : "  ";
                if (state == _currentState)
                {
                    stateMarker = "->";
                }

                Console.Write($"{stateMarker}{state} :   |");
                foreach (var symbol in _alphabet)
                {
                    if (_transitions[state].TryGetValue(symbol, out var nextState) && nextState != UndefinedState)
                    {
                        Console.Write($" {nextState} ||");
                    }
                    else
                    {
                        Console.Write("  ~ ||");
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine("--------------------------------------------------");
        }

        public void ProcessInput(string input)
        {
            Console.WriteLine($"Начальное состояние: {_currentState}");

            bool isValidState = true;

            foreach (var symbol in input)
            {
                if (!_alphabet.Contains(symbol))
                {
                    Console.WriteLine($"Ошибка: символ '{symbol}' не входит в алфавит. Обработка завершена.");
                    Console.WriteLine();
                    isValidState = false;
                    break;
                }

                Console.WriteLine($"Обрабатывается символ: '{symbol}'");

                if (_transitions[_currentState].TryGetValue(symbol, out var nextState) && nextState != UndefinedState)
                {
                    _currentState = nextState;
                    Console.WriteLine($"Перешли в состояние: {_currentState}");
                }
                else
                {
                    _currentState = UndefinedState;
                    Console.WriteLine($"Текущее состояние: {UndefinedState} — пустое множество, поэтому переход невозможен");
                    Console.WriteLine();
                    break;
                }
            }

            if (!(_currentState == UndefinedState) && isValidState)
            {
                Console.WriteLine($"\nОбработка завершена. Слово: {input}");
                Console.WriteLine($"Финальное состояние: {_currentState}");

                if (_finalStates.Contains(_currentState))
                {
                    Console.WriteLine("Состояние входит в число финальных.");
                }
                else
                {
                    Console.WriteLine("Состояние не входит в число финальных.");
                }
                Console.WriteLine();
            }

            ResetAutomaton();
        }

        private void ResetAutomaton()
        {
            _currentState = _states.First();
        }
    }
}
