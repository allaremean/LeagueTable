using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaLiga.Models
{
    public class Standings
    {
        public int Position { get; set; }
        public string? Team { get; set; }
        public int Played { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalDifference { get; set; }
        public int Points { get; set; }
    }
}
