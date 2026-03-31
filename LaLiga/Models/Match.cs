using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace LaLiga.Models
{
    public class Match
    {
        // Primary Key
        [Key]
        [Column(TypeName = "char(6)")]
        
        public required string MatchID { get; set; }

        // Foreign Keys
        [ForeignKey("HomeTeam")]
        public string? HomeTeamID { get; set; } = null!;

        [ForeignKey("AwayTeam")]
        public string? AwayTeamID { get; set; } = null!;

        public int HomeScore { get; set; }
        public int AwayScore { get; set; }

        // Navigation Properties
        public Team? HomeTeam { get; set; } = null!;
        public Team? AwayTeam { get; set; } = null!;
       
        }
}