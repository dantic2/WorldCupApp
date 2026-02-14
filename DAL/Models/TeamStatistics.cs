using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL.Models
{
    public class TeamStatistics
    {
        [JsonProperty("starting_eleven")]
        public List<Player> StartingEleven { get; set; } = new List<Player>();

        [JsonProperty("substitutes")]
        public List<Player> Substitutes { get; set; } = new List<Player>();

        //HELPERS

        public List<Player> AllPlayers()
        {
            return (StartingEleven ?? new List<Player>())
                .Concat(Substitutes ?? new List<Player>())
                .Distinct()
                .ToList();
        }

        public override bool Equals(object obj)
        {
            return obj is TeamStatistics other &&
                   AllPlayers().Count == other.AllPlayers().Count &&
                   !AllPlayers().Except(other.AllPlayers()).Any();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                foreach (var p in AllPlayers())
                    hash = hash * 23 + p.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            int s = StartingEleven?.Count ?? 0;
            int sub = Substitutes?.Count ?? 0;
            return $"Starting: {s}, Subs: {sub}";
        }




    }
}
