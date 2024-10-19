using Newtonsoft.Json;
using System;
using System.IO;
using Task0.Automation.Models;

namespace Task0.Automation.Helpers
{
    public class NkaEReader
    {
        public static NkaEDescription LoadFromJson()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configs", "NkaE.json");
            var jsonData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<NkaEDescription>(jsonData);
        }
    }
}
