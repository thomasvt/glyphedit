using System;
using System.IO;
using Newtonsoft.Json;

namespace GlyphEdit.Configuration
{
    public static class ConfigLoader
    {
        public static Config Load(string filename = "config.json")
        {
            if (!File.Exists(filename))
            {
                return CreateDefaultConfig(filename);
            }

            try
            {
                return JsonConvert.DeserializeObject<Config>(File.ReadAllText(filename));
            }
            catch
            {
                return CreateDefaultConfig(filename);
            }
        }

        private static Config CreateDefaultConfig(string filename)
        {
            var config = Config.GetDefault();
            File.WriteAllText(filename, JsonConvert.SerializeObject(config, Formatting.Indented));
            return config;
        }
    }
}
