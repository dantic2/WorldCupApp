using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DAL.Models
{


//Ovo je glavni model za jednu utakmicu iz matches.json.Trebat će ti za:

//dohvat igrača(starting_eleven + substitutes)

//rezultate utakmica

//događaje(golovi/kartoni)

//rang listu posjetitelja(attendance)

    public class Match
    {

        [JsonProperty("venue", NullValueHandling = NullValueHandling.Ignore)]
        public string Venue { get; set; }

        [JsonProperty("attendance", NullValueHandling = NullValueHandling.Ignore)]
        public string AttendanceRaw { get; set; }  // dolazi kao string npr. "43,319"

        [JsonProperty("location", NullValueHandling = NullValueHandling.Ignore)]
        public string Location { get; set; }

        [JsonProperty("datetime")]
        public DateTime? DateTime { get; set; }
        
        [JsonProperty("home_team")]
        public Team HomeTeam { get; set; }

        [JsonProperty("away_team")]
        public Team AwayTeam { get; set; }

        [JsonProperty("home_team_events")]
        public List<TeamEvent> HomeTeamEvents { get; set; } = new List<TeamEvent>();

        [JsonProperty("away_team_events")]
        public List<TeamEvent> AwayTeamEvents { get; set; } = new List<TeamEvent>();

        // Početne postave i zamjene (za obje strane)
        [JsonProperty("home_team_statistics")]
        public TeamStatistics HomeTeamStatistics { get; set; }

        [JsonProperty("away_team_statistics")]
        public TeamStatistics AwayTeamStatistics { get; set; }


        //HELPERS
        public int Attendance
        {
            get
            {
                // "43,319" -> 43319 (tolerantno)
                if (string.IsNullOrWhiteSpace(AttendanceRaw))
                    return 0;

                var digitsOnly = new string(AttendanceRaw.Where(char.IsDigit).ToArray());
                if (int.TryParse(digitsOnly, out int value))
                    return value;

                return 0;
            }
        }

        public bool InvolvesTeam(string fifaCode)
        {
            if (string.IsNullOrWhiteSpace(fifaCode))
                return false;

            return string.Equals(HomeTeam?.Code, fifaCode, StringComparison.OrdinalIgnoreCase)
                || string.Equals(AwayTeam?.Code, fifaCode, StringComparison.OrdinalIgnoreCase);
        }

        public IEnumerable<TeamEvent> AllEvents()
            => (HomeTeamEvents ?? Enumerable.Empty<TeamEvent>())
               .Concat(AwayTeamEvents ?? Enumerable.Empty<TeamEvent>());
        public string Score => $"{HomeTeam?.Goals ?? 0} : {AwayTeam?.Goals ?? 0}";

        public override bool Equals(object obj)
        {
            return obj is Match match &&
                   DateTime == match.DateTime &&
                   EqualityComparer<Team>.Default.Equals(HomeTeam, match.HomeTeam) &&
                   EqualityComparer<Team>.Default.Equals(AwayTeam, match.AwayTeam);
        }

        public override int GetHashCode()
        {
            int hashCode = 442661537;
            hashCode = hashCode * -1521134295 + DateTime.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Team>.Default.GetHashCode(HomeTeam);
            hashCode = hashCode * -1521134295 + EqualityComparer<Team>.Default.GetHashCode(AwayTeam);
            return hashCode;
        }

        public override string ToString()
        {
            return $"{HomeTeam?.DisplayName} vs {AwayTeam?.DisplayName} ({Score}) @ {Location}";
        }



    }
}
