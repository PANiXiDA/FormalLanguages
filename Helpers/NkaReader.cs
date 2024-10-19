using Newtonsoft.Json;
using System;
using System.IO;
using Task0.Automation.Models;

namespace Task0.Automation.Helpers
{
    public class NkaReader
    {
        public static NkaDescription LoadFromJson()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configs", "Nka.json");
            var jsonData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<NkaDescription>(jsonData);
        }
    }
}
