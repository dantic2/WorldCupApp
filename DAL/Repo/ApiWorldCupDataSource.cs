using DAL.Interfaces;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace DAL.Repo
{
    public sealed class ApiWorldCupDataSource : IWorldCupDataSource
    {
        private const string BaseUrl = "https://worldcup-vua.nullbit.hr";
        private static readonly HttpClient _http = new HttpClient();

        public Task<string> GetTeamsResultsJsonAsync(Championship championship)
        {
            var url = $"{BaseUrl}/{ToRoute(championship)}/teams/results";
            return _http.GetStringAsync(url);
        }

        public Task<string> GetMatchesJsonAsync(Championship championship)
        {
            var url = $"{BaseUrl}/{ToRoute(championship)}/matches";
            return _http.GetStringAsync(url);
        }

        private static string ToRoute(Championship c)
            => c == Championship.Women ? "women" : "men";
    }
}
