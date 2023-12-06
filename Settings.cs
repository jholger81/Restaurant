using System;
using System.IO;
using System.Text.Json;

namespace Restaurant
{
    public class Settings
    {
        public int? Port { get; set; }

        internal Settings Load()
        {
            var settings = new Settings();

            // Load and deserialize settings
            if (!File.Exists("config.json"))
                throw new Exception($"config not found at {System.IO.Directory.GetCurrentDirectory()}");

            string json = File.ReadAllText("config.json");
            settings = JsonSerializer.Deserialize<Settings>(json);

            return settings;
        }
    }
}
