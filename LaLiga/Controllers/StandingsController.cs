using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using LaLiga.Data;
using LaLiga.Models;
using Microsoft.AspNetCore.Authorization;
namespace LaLiga.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StandingsController(LaligaDbContext context) : ControllerBase
    {
        private readonly LaligaDbContext _context = context;
        
        [HttpGet]
        public async Task<ActionResult> GetStandings()
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

            return Ok(standings);
        }


        [HttpGet("{teamId}")]
        public async Task<ActionResult> GetStandingByTeamId(string teamId)
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

            if (teamStanding == null)
                return NotFound();

            return Ok(teamStanding);
        }
    }
}
