using Interpreter.Grammar;
using Interpreter.Stack;
using System;
using System.Collections.Generic;

namespace Interpreter.Postfix
{
    public class PostfixForm
    {
        private List<PostfixEntry> _postfix;
        private ExecutionStack _stack;
        private Dictionary<string, int> _variables;
        private Dictionary<int, string> _varNames = new Dictionary<int, string>();

        public PostfixForm()
        {
            _postfix = new List<PostfixEntry>();
            _stack = new ExecutionStack();
            _variables = new Dictionary<string, int>();
        }

        public void Interpret()
        {
            int pos = 0;
            while (pos < _postfix.Count)
            {
                var entry = _postfix[pos];
                switch (entry.Type)
                {
                    case EEntryType.etCmd:
                        var cmd = (ECmd)entry.Index;
                        pos = ExecuteCommand(cmd, pos);
                        break;
                    case EEntryType.etVar:
                        _stack.PushVal(GetVarValue(entry.Index));
                        pos++;
                        break;
                    case EEntryType.etConst:
                        _stack.PushVal(entry.Index);
                        pos++;
                        break;
                    case EEntryType.etCmdPtr:
                        int condition = _stack.PopVal();
                        if (condition == 0)
                        {
                            pos = entry.Index;
                        }
                        else
                        {
                            pos = pos+ 1;
                            // TODO возвращаемся в начало цикла, хардкод
                            _stack.PushVal(0);
                        }
                        break;
                    default:
                        throw new Exception($"Неизвестный тип записи: {entry.Type}");
                }
            }
        }

        public int ExecuteCommand(ECmd cmd, int pos)
        {
            switch (cmd)
            {
                case ECmd.SET:
                    int value = _stack.PopVal();
                    int varHash = _postfix[++pos].Index;
                    SetVarValue(varHash, value);
                    return pos + 1;

                case ECmd.ADD:
                    _stack.PushVal(_stack.PopVal() + _stack.PopVal());
                    return pos + 1;

                case ECmd.SUB:
                    int subtrahend = _stack.PopVal();
                    _stack.PushVal(_stack.PopVal() - subtrahend);
                    return pos + 1;

                case ECmd.MUL:
                    _stack.PushVal(_stack.PopVal() * _stack.PopVal());
                    return pos + 1;

                case ECmd.DIV:
                    int divisor = _stack.PopVal();
                    if (divisor == 0)
                        throw new DivideByZeroException("Ошибка: деление на ноль.");
                    _stack.PushVal(_stack.PopVal() / divisor);
                    return pos + 1;

                case ECmd.CMPL:
                    _stack.PushVal(_stack.PopVal() > _stack.PopVal() ? 1 : 0);
                    return pos + 1;

                case ECmd.CMPLE:
                    _stack.PushVal(_stack.PopVal() >= _stack.PopVal() ? 1 : 0);
                    return pos + 1;

                case ECmd.CMPG:
                    _stack.PushVal(_stack.PopVal() < _stack.PopVal() ? 1 : 0);
                    return pos + 1;

                case ECmd.CMPGE:
                    _stack.PushVal(_stack.PopVal() <= _stack.PopVal() ? 1 : 0);
                    return pos + 1;

                case ECmd.CMPE:
                    _stack.PushVal(_stack.PopVal() == _stack.PopVal() ? 1 : 0);
                    return pos + 1;

                case ECmd.AND:
                    _stack.PushVal((_stack.PopVal() != 0 && _stack.PopVal() != 0) ? 1 : 0);
                    return pos + 1;

                case ECmd.OR:
                    _stack.PushVal((_stack.PopVal() != 0 || _stack.PopVal() != 0) ? 1 : 0);
                    return pos + 1;

                case ECmd.JMP:
                    return _stack.PopVal();

                case ECmd.JZ:
                    int address = _stack.PopVal();
                    int condition = _stack.PopVal();
                    return condition == 0 ? address : pos + 1;

                default:
                    throw new Exception($"Неизвестная команда: {cmd}");
            }
        }

        private int GetVarValue(int varHash)
        {
            string varKey = varHash.ToString();

            if (!_variables.ContainsKey(varKey))
            {
                Console.WriteLine($"Инициализация переменной: {varKey} со значением по умолчанию 0.");
                _variables[varKey] = 0;
            }

            return _variables[varKey];
        }


        private void SetVarValue(int varHash, int value)
        {
            string varKey = varHash.ToString();
            _variables[varKey] = value;
        }

        public int WriteCmd(ECmd cmd)
        {
            _postfix.Add(new PostfixEntry(EEntryType.etCmd, (int)cmd));
            return _postfix.Count - 1;
        }

        public int PushVar(string varName)
        {
            int index = varName.GetHashCode();
            _postfix.Add(new PostfixEntry(EEntryType.etVar, index));

            if (!_varNames.ContainsKey(index))
            {
                _varNames.Add(index, varName);
            }

            return _postfix.Count - 1;
        }

        public int PushConst(int value)
        {
            _postfix.Add(new PostfixEntry(EEntryType.etConst, value));
            return _postfix.Count - 1;
        }

        public int WriteCmdPtr(int ptr)
        {
            _postfix.Add(new PostfixEntry(EEntryType.etCmdPtr, ptr));
            return _postfix.Count - 1;
        }

        public void SetCmdPtr(int index, int ptr)
        {
            _postfix[index] = new PostfixEntry(EEntryType.etCmdPtr, ptr);
        }

        public int GetCurrentAddress()
        {
            return _postfix.Count - 1;
        }

        public void PrintPostfix()
        {
            Console.WriteLine("ПОЛИЗ:");
            foreach (var entry in _postfix)
            {
                string entryDescription;
                switch (entry.Type)
                {
                    case EEntryType.etCmd:
                        entryDescription = $"Команда: {((ECmd)entry.Index)}";
                        break;
                    case EEntryType.etVar:
                        entryDescription = $"Переменная (хэш): {entry.Index}";
                        break;
                    case EEntryType.etConst:
                        entryDescription = $"Константа: {entry.Index}";
                        break;
                    case EEntryType.etCmdPtr:
                        entryDescription = $"Указатель команды на адрес: {entry.Index}";
                        break;
                    default:
                        entryDescription = "Неизвестный тип записи";
                        break;
                }
                Console.WriteLine(entryDescription);
            }
        }

        public void SetVarAndPop(string varName)
        {
            int varHash = varName.GetHashCode();
            int value = _stack.PopVal();
            SetVarValue(varHash, value);
        }

        public void PrintVariables()
        {
            Console.WriteLine("Значения переменных:");
            foreach (var kvp in _variables)
            {
                int varHash = int.Parse(kvp.Key);
                int value = kvp.Value;

                if (_varNames.ContainsKey(varHash))
                {
                    string varName = _varNames[varHash];
                    Console.WriteLine($"{varName} = {value}");
                }
                else
                {
                    Console.WriteLine($"Переменная с хэшом {varHash} = {value}");
                }
            }
        }
    }
}
