using System.Collections.Generic;

namespace Task0.Automation.Models
{
    public class NkaDescription
    {
        public List<string> States { get; set; }
        public List<char> Alphabet { get; set; }
        public Dictionary<string, Dictionary<char, List<string>>> Transitions { get; set; }
        public string InitialState { get; set; }
        public List<string> FinalStates { get; set; }
    }
}
