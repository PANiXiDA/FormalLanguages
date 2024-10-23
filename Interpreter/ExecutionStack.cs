using System;
using System.Collections.Generic;

namespace Interpreter.Stack
{
    public class ExecutionStack
    {
        private Stack<int> _stack;

        public ExecutionStack()
        {
            _stack = new Stack<int>();
        }

        public int PopVal()
        {
            if (_stack.Count == 0)
                throw new InvalidOperationException("Ошибка: попытка извлечь значение из пустого стека.");
            return _stack.Pop();
        }

        public void PushVal(int value)
        {
            _stack.Push(value);
        }

        public void PrintStack()
        {
            Console.WriteLine("Текущее состояние стека: ");
            foreach (var item in _stack)
            {
                Console.WriteLine(item);
            }
        }
    }
}
