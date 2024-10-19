using Newtonsoft.Json;
using System;
using System.IO;
using Task0.Automation.Models;

namespace Task0.Automation.Helpers
{
    public class DkaReader
    {
        public static DkaDescription LoadFromJson()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configs", "Dka.json");
            var jsonData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<DkaDescription>(jsonData);
        }
    }
}
