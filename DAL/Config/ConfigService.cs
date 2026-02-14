using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace DAL.Config
{
    public sealed class ConfigService
    {
        private readonly string _configPath;

        public ConfigService(string storageDir)
        {
            if (string.IsNullOrWhiteSpace(storageDir))
                throw new ArgumentNullException(nameof(storageDir));

            _configPath = Path.Combine(storageDir, "datasource.json");
        }

        public DataSourceConfig Load()
        {
            try
            {
                if (!File.Exists(_configPath))
                {
                    // default config
                    return new DataSourceConfig
                    {
                        Mode = DataSourceMode.Api,
                        JsonFolder = @"Resources\Data"
                    };
                }

                var json = File.ReadAllText(_configPath, Encoding.UTF8);

                var cfg = JsonConvert.DeserializeObject<DataSourceConfig>(json);
                if (cfg == null)
                    throw new InvalidOperationException("Config file is empty or invalid.");

                // default fallback
                if (string.IsNullOrWhiteSpace(cfg.JsonFolder))
                    cfg.JsonFolder = @"Resources\Data";

                return cfg;
            }
            catch
            {
                // ako je config ostecen, fallback na API ili jsonfiles 
                return new DataSourceConfig
                {
                    Mode = DataSourceMode.Api,
                    JsonFolder = @"Resources\Data"
                };
            }
        }

        public void Save(DataSourceConfig cfg)
        {
            if (cfg == null) throw new ArgumentNullException(nameof(cfg));

            var json = JsonConvert.SerializeObject(cfg, Formatting.Indented);
            Directory.CreateDirectory(Path.GetDirectoryName(_configPath));
            File.WriteAllText(_configPath, json, Encoding.UTF8);
        }
    }
}
