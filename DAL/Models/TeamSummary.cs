namespace DAL.Models
{
    public class TeamSummary
    {
        public string Country { get; set; }
        public string FifaCode { get; set; }

        public int Played { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }

        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }

        public int GoalDifference => GoalsFor - GoalsAgainst;
    }
}
