using LaLiga.Core.Models;
using Microsoft.EntityFrameworkCore;
namespace LaLiga.Infrastructure.Data
{
    public class LaligaDbContext(DbContextOptions<LaligaDbContext> options) : DbContext(options)
    {
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<Match> Matches => Set<Match>();

        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Match>().HasOne(m => m.HomeTeam).WithMany()
            .HasForeignKey(m => m.HomeTeamID)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>().HasOne(m => m.AwayTeam).WithMany()
            .HasForeignKey(m => m.AwayTeamID)
            .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Team>().HasData(
                new Team
                {
                    TeamID = "RMA",
                    Name = "Real Madrid",
                    Stadium = "Esatdio Santiago Bernabeu",
                    City = "Madrid"
                },
                new Team
                {
                    TeamID = "FCB",
                    Name = "FC Barcelona",
                    Stadium = "Spotify Camp Nou",
                    City = "Barcelona"

                },
                new Team
                {
                    TeamID = "ATM",
                    Name = "Atletico Madrid",
                    Stadium = "Wanda Metropolitano",
                    City = "Madrid"
                }
                );
            modelBuilder.Entity<Match>().HasData(
                new Match
                {
                    MatchID = "RMAFCB",
                    HomeTeamID = "RMA",
                    AwayTeamID = "FCB",
                    HomeScore = 2,
                    AwayScore = 1,


                },
                new Match
                {
                    MatchID = "ATMFCB",
                    HomeTeamID = "ATM",
                    AwayTeamID = "FCB",
                    HomeScore = 4,
                    AwayScore = 0
                }
           );

        
        }
    }
}
