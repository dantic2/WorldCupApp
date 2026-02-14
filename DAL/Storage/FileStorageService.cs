using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Storage
{
    public class FileStorageService
    {
        private readonly string _storageDir;
        private readonly string _setttingPath;
        private readonly string _favouritesPath;
        private readonly string _playerImagesPath;
        public string StorageDirectory => _storageDir;

        public FileStorageService()
        {

            var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);

            while (dir != null && !string.Equals(dir.Name, "WorldCupApp", StringComparison.OrdinalIgnoreCase))
                dir = dir.Parent; //IDI U root solutiona u WorldCupAPp 

            //fallback
            var solutionDir = dir?.FullName ?? AppDomain.CurrentDomain.BaseDirectory;

            _storageDir = Path.Combine(solutionDir, "Storage");
            _setttingPath = Path.Combine(_storageDir, "settings.json");
            _favouritesPath = Path.Combine(_storageDir, "favourites.json");
            _playerImagesPath = Path.Combine(_storageDir, "playerImages.json");
        }

        public void SaveSettings(AppSettings settings) 
        { 
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            EnsureStorageDir();

            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(_setttingPath, json, Encoding.UTF8);
        }

        public AppSettings LoadSettings()
        {
            EnsureStorageDir();

            if (!File.Exists(_setttingPath))
                return null;

            var json = File.ReadAllText(_setttingPath, Encoding.UTF8);

            
            if (string.IsNullOrWhiteSpace(json))
                return null;

            return JsonConvert.DeserializeObject<AppSettings>(json);
        }

        public void SaveFavourites(Favourites favourites)
        {
            if (favourites == null)
                throw new ArgumentNullException(nameof(favourites));

            EnsureStorageDir();

            var json = JsonConvert.SerializeObject(favourites, Formatting.Indented);
            File.WriteAllText(_favouritesPath, json, Encoding.UTF8);
        }

        public Favourites LoadFavourites()
        {
            EnsureStorageDir();

            if (!File.Exists(_favouritesPath))
                return null;

            var json = File.ReadAllText(_favouritesPath, Encoding.UTF8);
            if (string.IsNullOrWhiteSpace(json))
                return null;

            return JsonConvert.DeserializeObject<Favourites>(json);
        }

        public void SavePlayerImageMap(PlayerImageMap map)
        {
            if (map == null)
                throw new ArgumentNullException(nameof(map));

            EnsureStorageDir();

            var json = JsonConvert.SerializeObject(map, Formatting.Indented);
            File.WriteAllText(_playerImagesPath, json, Encoding.UTF8);
        }

        public PlayerImageMap LoadPlayerImageMap()
        {
            EnsureStorageDir();

            if (!File.Exists(_playerImagesPath))
                return null;

            var json = File.ReadAllText(_playerImagesPath, Encoding.UTF8);
            if (string.IsNullOrWhiteSpace(json))
                return null;

            return JsonConvert.DeserializeObject<PlayerImageMap>(json);
        }

        private void EnsureStorageDir()
        {
            if (!Directory.Exists(_storageDir))
                Directory.CreateDirectory(_storageDir);
        }
    }
}
