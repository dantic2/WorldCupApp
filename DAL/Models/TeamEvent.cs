using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DAL.Models
{

    public class TeamEvent
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public int? Id { get; set; }

        [JsonProperty("type_of_event", NullValueHandling = NullValueHandling.Ignore)]
        public string TypeOfEvent { get; set; }

        [JsonProperty("player", NullValueHandling = NullValueHandling.Ignore)]
        public string Player { get; set; }

        [JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
        public string Time { get; set; }

        [JsonProperty("extra_info", NullValueHandling = NullValueHandling.Ignore)]
        public string ExtraInfo { get; set; }

        //HELPERS

        public bool isGoal
        {
            get
            {
                var t = TypeOfEvent ?? string.Empty;
                return t.IndexOf("goal", StringComparison.OrdinalIgnoreCase) >= 0;
            }
        }

        public bool isYellowCard
        {
            get
            {
                var t = TypeOfEvent ?? string.Empty;
                return t.IndexOf("yellow-card", StringComparison.OrdinalIgnoreCase) >= 0;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is TeamEvent @event &&
                   Id == @event.Id &&
                   TypeOfEvent == @event.TypeOfEvent &&
                   Player == @event.Player &&
                   Time == @event.Time &&
                   ExtraInfo == @event.ExtraInfo;
        }

        public override int GetHashCode()
        {
            int hashCode = -2134516259;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(TypeOfEvent);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Player);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Time);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ExtraInfo);
            return hashCode;
        }

        public override string ToString()
        {
            return $"{Time} {TypeOfEvent} - {Player} ({ExtraInfo})";
        }


    }
}
