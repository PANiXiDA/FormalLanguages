using System.Collections.Generic;

namespace LexicalAnalyzer.SymbolTable
{
    public class SymbolTable
    {
        private readonly Dictionary<string, int> _identifiers = new Dictionary<string, int>();
        private readonly Dictionary<string, int> _constants = new Dictionary<string, int>();

        public void AddIdentifier(string identifier)
        {
            if (!_identifiers.ContainsKey(identifier))
            {
                _identifiers[identifier] = _identifiers.Count + 1;
            }
        }

        public void AddConstant(string constant)
        {
            if (!_constants.ContainsKey(constant))
            {
                _constants[constant] = _constants.Count + 1;
            }
        }

        public int GetIdentifierId(string identifier)
        {
            return _identifiers.ContainsKey(identifier) ? _identifiers[identifier] : -1;
        }

        public int GetConstantId(string constant)
        {
            return _constants.ContainsKey(constant) ? _constants[constant] : -1;
        }
    }
}
