using DAL.Interfaces;
using DAL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class WorldCupService
    {
        private readonly IWorldCupDataSource _dataSource;

        public WorldCupService(IWorldCupDataSource dataSource)
        {
            _dataSource = dataSource ?? throw new ArgumentNullException(nameof(dataSource));
        }

        public async Task<List<TeamResult>> GetTeamsAsync(Championship championship)
        {
            var json = await _dataSource.GetTeamsResultsJsonAsync(championship);

            var teams = JsonConvert.DeserializeObject<List<TeamResult>>(json);

            return teams ?? new List<TeamResult>();
        }

        public async Task<List<Match>> GetMatchesAsync(Championship championship)
        {
            var json = await _dataSource.GetMatchesJsonAsync(championship);
            var matches = JsonConvert.DeserializeObject<List<Match>>(json);
            return matches ?? new List<Match>();
        }

        public async Task<List<Player>> GetPlayersForTeamAsync(Championship championship, string fifaCode)
        {
            if (string.IsNullOrEmpty(fifaCode))
                return new List<Player>();

            var matches = await GetMatchesAsync(championship);

            var firstMatch = matches
                .Where(m =>
                    string.Equals(m.HomeTeam?.Code, fifaCode, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(m.AwayTeam?.Code, fifaCode, StringComparison.OrdinalIgnoreCase))
                .OrderBy(m => m.DateTime ?? DateTime.MaxValue)
                .FirstOrDefault();

            if (firstMatch == null)
                return new List<Player>();

            var isHome = string.Equals(firstMatch.HomeTeam?.Code, fifaCode, StringComparison.OrdinalIgnoreCase);

            var stats = isHome ? firstMatch.HomeTeamStatistics : firstMatch.AwayTeamStatistics;

            if (stats == null)
                return new List<Player>();

            return stats.AllPlayers();

        }

        public async Task<List<Team>> GetOpponentsAsync(Championship championship, string fifaCode)
        {
            if (string.IsNullOrWhiteSpace(fifaCode))
                return new List<Team>();

            var matches = await GetMatchesAsync(championship);

            var opponents = matches
                .Where(m =>
                    string.Equals(m.HomeTeam?.Code, fifaCode, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(m.AwayTeam?.Code, fifaCode, StringComparison.OrdinalIgnoreCase))
                .Select(m =>
                    string.Equals(m.HomeTeam?.Code, fifaCode, StringComparison.OrdinalIgnoreCase)
                        ? m.AwayTeam
                        : m.HomeTeam)
                .Where(t => t != null)
                .Distinct()
                .OrderBy(t => t.Country)
                .ToList();

            return opponents;
        }

        public async Task<Match> GetMatchBetweenAsync(Championship championship, string fifaCodeA, string fifaCodeB)
        {
            if (string.IsNullOrWhiteSpace(fifaCodeA) || string.IsNullOrWhiteSpace(fifaCodeB))
                return null;

            var matches = await GetMatchesAsync(championship);

            // find the first match between the two teams no matter who is home/away
            var match = matches
                .Where(m =>
                    (string.Equals(m.HomeTeam?.Code, fifaCodeA, StringComparison.OrdinalIgnoreCase) &&
                     string.Equals(m.AwayTeam?.Code, fifaCodeB, StringComparison.OrdinalIgnoreCase))
                 || (string.Equals(m.HomeTeam?.Code, fifaCodeB, StringComparison.OrdinalIgnoreCase) &&
                     string.Equals(m.AwayTeam?.Code, fifaCodeA, StringComparison.OrdinalIgnoreCase)))
                .OrderBy(m => m.DateTime ?? DateTime.MaxValue)
                .FirstOrDefault();

            return match;
        }

        public async Task<List<(string PlayerName, int Goals)>> GetGoalsRankingAsync(Championship championship, string fifaCode)
        {
            if (string.IsNullOrWhiteSpace(fifaCode))
                return new List<(string PlayerName, int Goals)>();

            var matches = await GetMatchesAsync(championship);

            var relevantMatches = matches
                .Where(m =>
                    string.Equals(m.HomeTeam?.Code, fifaCode, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(m.AwayTeam?.Code, fifaCode, StringComparison.OrdinalIgnoreCase))
                .ToList();

            // skupljanje eventova samo za zadani tim
            var events = relevantMatches
                .SelectMany(m =>
                    string.Equals(m.HomeTeam?.Code, fifaCode, StringComparison.OrdinalIgnoreCase)
                        ? (m.HomeTeamEvents ?? new List<TeamEvent>())
                        : (m.AwayTeamEvents ?? new List<TeamEvent>()))
                .Where(e => e != null && e.isGoal && !string.IsNullOrWhiteSpace(e.Player))
                .ToList();

            var ranking = events
                .GroupBy(e => e.Player.Trim(), StringComparer.OrdinalIgnoreCase)
                .Select(g => (PlayerName: g.Key, Goals: g.Count()))
                .OrderByDescending(x => x.Goals)
                .ThenBy(x => x.PlayerName)
                .ToList();

            return ranking;
        }

        public async Task<List<(string PlayerName, int YellowCards)>> GetYellowCardsRankingAsync(Championship championship, string fifaCode)
        {
            if (string.IsNullOrWhiteSpace(fifaCode))
                return new List<(string PlayerName, int YellowCards)>();

            var matches = await GetMatchesAsync(championship);

            var relevantMatches = matches
                .Where(m =>
                    string.Equals(m.HomeTeam?.Code, fifaCode, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(m.AwayTeam?.Code, fifaCode, StringComparison.OrdinalIgnoreCase))
                .ToList();

            var events = relevantMatches
                .SelectMany(m =>
                    string.Equals(m.HomeTeam?.Code, fifaCode, StringComparison.OrdinalIgnoreCase)
                        ? (m.HomeTeamEvents ?? new List<TeamEvent>())
                        : (m.AwayTeamEvents ?? new List<TeamEvent>()))
                .Where(e => e != null && e.isYellowCard && !string.IsNullOrWhiteSpace(e.Player))
                .ToList();

            var ranking = events
                .GroupBy(e => e.Player.Trim(), StringComparer.OrdinalIgnoreCase)
                .Select(g => (PlayerName: g.Key, YellowCards: g.Count()))
                .OrderByDescending(x => x.YellowCards)
                .ThenBy(x => x.PlayerName)
                .ToList();

            return ranking;
        }

        public async Task<List<(string Location, int Attendance, string Home, string Away, string Score)>> GetAttendanceRankingAsync(Championship championship, string fifaCode)
        {
            if (string.IsNullOrWhiteSpace(fifaCode))
                return new List<(string Location, int Attendance, string Home, string Away, string Score)>();

            var matches = await GetMatchesAsync(championship);

            var ranking = matches
                .Where(m =>
                    string.Equals(m.HomeTeam?.Code, fifaCode, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(m.AwayTeam?.Code, fifaCode, StringComparison.OrdinalIgnoreCase))
                .Select(m => (
                    Location: m.Location ?? "",
                    Attendance: m.Attendance,
                    Home: m.HomeTeam?.Country ?? "",
                    Away: m.AwayTeam?.Country ?? "",
                    Score: $"{m.HomeTeam?.Goals ?? 0} : {m.AwayTeam?.Goals ?? 0}"
                ))
                .OrderByDescending(x => x.Attendance)
                .ThenBy(x => x.Location)
                .ToList();

            return ranking;
        }

        public async Task<TeamSummary> GetTeamSummaryAsync(Championship championship, string fifaCode)
        {
            if (string.IsNullOrWhiteSpace(fifaCode))
                return null;

            var teams = await GetTeamsAsync(championship);
            var team = teams.FirstOrDefault(t => string.Equals(t.FifaCode, fifaCode, StringComparison.OrdinalIgnoreCase));

            var matches = await GetMatchesAsync(championship);

            var relevant = matches.Where(m =>
                string.Equals(m.HomeTeam?.Code, fifaCode, StringComparison.OrdinalIgnoreCase) ||
                string.Equals(m.AwayTeam?.Code, fifaCode, StringComparison.OrdinalIgnoreCase));

            var summary = new TeamSummary
            {
                Country = team?.Country ?? fifaCode,
                FifaCode = fifaCode
            };

            foreach (var m in relevant)
            {
                summary.Played++;

                bool isHome = string.Equals(m.HomeTeam?.Code, fifaCode, StringComparison.OrdinalIgnoreCase);

                int gf = isHome ? (m.HomeTeam?.Goals ?? 0) : (m.AwayTeam?.Goals ?? 0);
                int ga = isHome ? (m.AwayTeam?.Goals ?? 0) : (m.HomeTeam?.Goals ?? 0);

                summary.GoalsFor += gf;
                summary.GoalsAgainst += ga;

                if (gf > ga) summary.Wins++;
                else if (gf < ga) summary.Losses++;
                else summary.Draws++;
            }

            return summary;
        }
    }
}
