using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    // men/results.json
    public class TeamResult
    {
        [JsonProperty("id", NullValueHandling= NullValueHandling.Ignore)]
        public int Id { get; set; }

        [JsonProperty("country", NullValueHandling= NullValueHandling.Ignore)]
        public string Country { get; set; }

        [JsonProperty("fifa_code", NullValueHandling= NullValueHandling.Ignore)]
        public string FifaCode { get; set; }

        [JsonProperty("games_played", NullValueHandling= NullValueHandling.Ignore)]
        public int GamesPlayed { get; set; }

        [JsonProperty("wins", NullValueHandling= NullValueHandling.Ignore)]
        public int Wins { get; set; }

        [JsonProperty("draws", NullValueHandling= NullValueHandling.Ignore)]
        public int Draws { get; set; }

        [JsonProperty("losses", NullValueHandling= NullValueHandling.Ignore)]
        public int Losses { get; set; }

        [JsonProperty("goals_for", NullValueHandling= NullValueHandling.Ignore)]    
        public int GoalsFor { get; set; }

        [JsonProperty("goals_against", NullValueHandling= NullValueHandling.Ignore)]
        public int GoalsAgainst { get; set; }

        [JsonProperty("goal_differential", NullValueHandling= NullValueHandling.Ignore)]
        public int GoalDifferential { get; set; }


        [JsonIgnore]
        public string DisplayName => $"{Country} ({FifaCode})";

        public override string ToString()
        {
            return $"{Country} ({FifaCode})";
        }

        public int CompareTo(TeamResult other) 
        { 
            return Country.CompareTo(other.Country);
        }

        public override bool Equals(object obj)
        {
            return obj is TeamResult result &&
                   Id == result.Id &&
                   FifaCode == result.FifaCode &&
                   GamesPlayed == result.GamesPlayed &&
                   Wins == result.Wins &&
                   Draws == result.Draws &&
                   Losses == result.Losses &&
                   GoalsFor == result.GoalsFor &&
                   GoalsAgainst == result.GoalsAgainst &&
                   GoalDifferential == result.GoalDifferential;
        }


        public override int GetHashCode()
        {
            int hashCode = 1861217430;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FifaCode);
            hashCode = hashCode * -1521134295 + GamesPlayed.GetHashCode();
            hashCode = hashCode * -1521134295 + Wins.GetHashCode();
            hashCode = hashCode * -1521134295 + Draws.GetHashCode();
            hashCode = hashCode * -1521134295 + Losses.GetHashCode();
            hashCode = hashCode * -1521134295 + GoalsFor.GetHashCode();
            hashCode = hashCode * -1521134295 + GoalsAgainst.GetHashCode();
            hashCode = hashCode * -1521134295 + GoalDifferential.GetHashCode();
            return hashCode;
        }
    }
}
