using DAL.Config;
using DAL.Interfaces;
using DAL.Repo;
using DAL.Storage;
using System;
using System.IO;

namespace DAL.Services
{
    public sealed class AppContext
    {
        public FileStorageService Storage { get; }
        public WorldCupService WorldCup { get; }

        public string ActiveDataSourceName { get; private set; }

        public AppContext()
        {
            Storage = new FileStorageService();

            // load datasource config
            var cfgService = new ConfigService(Storage.StorageDirectory);
            var cfg = cfgService.Load();

            // choose data source
            IWorldCupDataSource ds = CreateDataSource(cfg);

            // services
            WorldCup = new WorldCupService(ds);
        }

        private IWorldCupDataSource CreateDataSource(DataSourceConfig cfg)
        {
            switch (cfg.Mode)
            {
                case DataSourceMode.Json:
                    ActiveDataSourceName = "JSON FILES";
                    var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                    var dataRoot = Path.Combine(baseDir, cfg.JsonFolder);
                    return new FileWorldCupDataSource(dataRoot);

                case DataSourceMode.Api:
                default:
                    ActiveDataSourceName = "API";
                    return new ApiWorldCupDataSource();
            }
        }
    }
}
