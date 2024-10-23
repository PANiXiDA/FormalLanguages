using Interpreter.Grammar;

namespace Interpreter.Postfix
{
    public class PostfixEntry
    {
        public EEntryType Type { get; set; }
        public int Index { get; set; }

        public PostfixEntry(EEntryType type, int index)
        {
            Type = type;
            Index = index;
        }
    }
}
