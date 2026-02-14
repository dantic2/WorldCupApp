using DAL.Interfaces;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repo
{
    public sealed class FileWorldCupDataSource : IWorldCupDataSource
    {
        private readonly string _dataRoot;

        public FileWorldCupDataSource(string dataRoot)
        {
            _dataRoot = dataRoot ?? throw new ArgumentNullException(nameof(dataRoot));
        }

        public Task<string> GetTeamsResultsJsonAsync(Championship championship)
        {
            var folder = championship == Championship.Women ? "Women" : "Men";
            var path = Path.Combine(_dataRoot, folder, "results.json");
            return ReadAllTextAsync(path, Encoding.UTF8);
        }

        public Task<string> GetMatchesJsonAsync(Championship championship)
        {
            var folder = championship == Championship.Women ? "Women" : "Men";
            var path = Path.Combine(_dataRoot, folder, "matches.json");
            return ReadAllTextAsync(path, Encoding.UTF8);
        }

        private static async Task<string> ReadAllTextAsync(string path, Encoding encoding)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true))
            using (var reader = new StreamReader(stream, encoding))
            {
                return await reader.ReadToEndAsync().ConfigureAwait(false);
            }
        }
    }
}
