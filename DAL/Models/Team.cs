using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// home_team and away_team in matches.json
namespace DAL.Models
{
    public class Team
    {
        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public string Country { get; set; }

        [JsonProperty("code", NullValueHandling = NullValueHandling.Ignore)]
        public string Code { get; set; }

        [JsonProperty("goals", NullValueHandling = NullValueHandling.Ignore)]
        public int? Goals { get; set; }


        //HELPERS

        public string DisplayName => $"{Country} ({Code})";

        public override bool Equals(object obj)
        {
            return obj is Team team &&
                   Code == team.Code;
        }

        public override int GetHashCode()
        {
            return -434485196 + EqualityComparer<string>.Default.GetHashCode(Code);
        }

        public override string ToString()
        {
            return DisplayName;
        }

    }
}
