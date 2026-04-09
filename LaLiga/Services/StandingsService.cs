using LaLiga.Data;
using LaLiga.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaLiga.Services
{
    public class StandingsService(LaligaDbContext context) : IStandingsService
    {
        private readonly LaligaDbContext _context = context;

        public async Task<List<Standings>> GetStandingsAsync()
        {
            var teams = await _context.Teams.ToListAsync();
            var matches = await _context.Matches.ToListAsync();

            var table = teams.Select(team => new Standings
            {
                Position = 0,
                Team = team.Name,

                Played = matches.Count(m =>
                    m.HomeTeamID == team.TeamID ||
                    m.AwayTeamID == team.TeamID),

                Wins = matches.Count(m =>
                    (m.HomeTeamID == team.TeamID && m.HomeScore > m.AwayScore) ||
                    (m.AwayTeamID == team.TeamID && m.AwayScore > m.HomeScore)),

                Draws = matches.Count(m =>
                    (m.HomeTeamID == team.TeamID && m.HomeScore == m.AwayScore) ||
                    (m.AwayTeamID == team.TeamID && m.AwayScore == m.HomeScore)),

                Losses = matches.Count(m =>
                    (m.HomeTeamID == team.TeamID && m.HomeScore < m.AwayScore) ||
                    (m.AwayTeamID == team.TeamID && m.AwayScore < m.HomeScore)),

                GoalsFor = matches
                    .Where(m => m.HomeTeamID == team.TeamID || m.AwayTeamID == team.TeamID)
                    .Sum(m => m.HomeTeamID == team.TeamID ? m.HomeScore : m.AwayScore),

                GoalsAgainst = matches
                    .Where(m => m.HomeTeamID == team.TeamID || m.AwayTeamID == team.TeamID)
                    .Sum(m => m.HomeTeamID == team.TeamID ? m.AwayScore : m.HomeScore),

                GoalDifference =
                    matches.Where(m => m.HomeTeamID == team.TeamID || m.AwayTeamID == team.TeamID)
                    .Sum(m => m.HomeTeamID == team.TeamID ? m.HomeScore : m.AwayScore)
                                        -
                    matches.Where(m => m.HomeTeamID == team.TeamID || m.AwayTeamID == team.TeamID)
                    .Sum(m => m.HomeTeamID == team.TeamID ? m.AwayScore : m.HomeScore),

                Points =
                    matches.Count(m =>
                    (m.HomeTeamID == team.TeamID && m.HomeScore > m.AwayScore) ||
                    (m.AwayTeamID == team.TeamID && m.AwayScore > m.HomeScore)) * 3
                                            +
                    matches.Count(m =>
                    (m.HomeTeamID == team.TeamID && m.HomeScore == m.AwayScore) ||
                    (m.AwayTeamID == team.TeamID && m.HomeScore == m.AwayScore))
            });

            var standings = table.OrderByDescending(t => t.Points).ThenByDescending(t => t.GoalDifference)
                .ThenByDescending(t => t.GoalsFor).ToList();

            for (int i = 0; i < standings.Count; i++)
            {
                standings[i].Position = i + 1;
            }

            return standings;
        }

        public async Task<Standings?> GetStandingByTeamIdAsync(string teamId)
        {
            var teams = await _context.Teams.ToListAsync();
            var matches = await _context.Matches.ToListAsync();

            var table = teams.Select(team => new Standings
            {
                Team = team.Name,

                Played = matches.Count(m =>
                    m.HomeTeamID == team.TeamID ||
                    m.AwayTeamID == team.TeamID),

                Wins = matches.Count(m =>
                    (m.HomeTeamID == team.TeamID && m.HomeScore > m.AwayScore) ||
                    (m.AwayTeamID == team.TeamID && m.AwayScore > m.HomeScore)),

                Draws = matches.Count(m =>
                    (m.HomeTeamID == team.TeamID && m.HomeScore == m.AwayScore) ||
                    (m.AwayTeamID == team.TeamID && m.AwayScore == m.HomeScore)),

                Losses = matches.Count(m =>
                    (m.HomeTeamID == team.TeamID && m.HomeScore < m.AwayScore) ||
                    (m.AwayTeamID == team.TeamID && m.AwayScore < m.HomeScore)),

                GoalsFor = matches
                    .Where(m => m.HomeTeamID == team.TeamID || m.AwayTeamID == team.TeamID)
                    .Sum(m => m.HomeTeamID == team.TeamID ? m.HomeScore : m.AwayScore),

                GoalsAgainst = matches
                    .Where(m => m.HomeTeamID == team.TeamID || m.AwayTeamID == team.TeamID)
                    .Sum(m => m.HomeTeamID == team.TeamID ? m.AwayScore : m.HomeScore),

                GoalDifference =
                    matches.Where(m => m.HomeTeamID == team.TeamID || m.AwayTeamID == team.TeamID)
                    .Sum(m => m.HomeTeamID == team.TeamID ? m.HomeScore : m.AwayScore)
                    -
                    matches.Where(m => m.HomeTeamID == team.TeamID || m.AwayTeamID == team.TeamID)
                    .Sum(m => m.HomeTeamID == team.TeamID ? m.AwayScore : m.HomeScore),

                Points =
                    matches.Count(m =>
                    (m.HomeTeamID == team.TeamID && m.HomeScore > m.AwayScore) ||
                    (m.AwayTeamID == team.TeamID && m.AwayScore > m.HomeScore)) * 3
                    +
                    matches.Count(m =>
                    (m.HomeTeamID == team.TeamID && m.HomeScore == m.AwayScore) ||
                    (m.AwayTeamID == team.TeamID && m.HomeScore == m.AwayScore))
            });

            var standings = table
                .OrderByDescending(t => t.Points)
                .ThenByDescending(t => t.GoalDifference)
                .ThenByDescending(t => t.GoalsFor)
                .ToList();

            for (int i = 0; i < standings.Count; i++)
            {
                standings[i].Position = i + 1;
            }

            var teamStanding = standings.FirstOrDefault(t => teams
                .First(team => team.Name == t.Team).TeamID == teamId);

            return teamStanding;
        }
    }
}
