using RcursiveDescentParser.Grammar;
using System;
using System.Collections.Generic;

namespace RcursiveDescentParser.Postfix
{
    public class PostfixForm
    {
        private List<PostfixEntry> _postfix;

        public PostfixForm()
        {
            _postfix = new List<PostfixEntry>();
        }

        public int WriteCmd(ECmd cmd)
        {
            _postfix.Add(new PostfixEntry(EEntryType.etCmd, (int)cmd));
            return _postfix.Count - 1;
        }

        public int WriteVar(string varName)
        {
            int index = varName.GetHashCode();
            _postfix.Add(new PostfixEntry(EEntryType.etVar, index));
            return _postfix.Count - 1;
        }

        public int WriteConst(int ind)
        {
            _postfix.Add(new PostfixEntry(EEntryType.etConst, ind));
            return _postfix.Count - 1;
        }

        public int WriteCmdPtr(int ptr)
        {
            _postfix.Add(new PostfixEntry(EEntryType.etCmdPtr, ptr));
            return _postfix.Count - 1;
        }

        public void SetCmdPtr(int ind, int ptr)
        {
            _postfix[ind] = new PostfixEntry(EEntryType.etCmdPtr, ptr);
        }

        public int GetCurrentAddress()
        {
            return _postfix.Count - 1;
        }

        public void PrintPostfix()
        {
            foreach (var entry in _postfix)
            {
                string entryDescription = "";

                switch (entry.Type)
                {
                    case EEntryType.etCmd:
                        entryDescription = $"Команда: {((ECmd)entry.Index)}";
                        break;
                    case EEntryType.etVar:
                        entryDescription = $"Переменная: {entry.Index} (хэш)";
                        break;
                    case EEntryType.etConst:
                        entryDescription = $"Константа: {entry.Index}";
                        break;
                    case EEntryType.etCmdPtr:
                        entryDescription = $"Указатель команды на адрес: {entry.Index}";
                        break;
                    default:
                        entryDescription = "Неизвестный тип";
                        break;
                }

                Console.WriteLine(entryDescription);
            }
        }
    }
}
