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
    public class MatchesController(LaligaDbContext context) : ControllerBase
    {
        private readonly LaligaDbContext _context = context;
        [Authorize(Roles ="SuperAdmin,Admin,User")]
        [HttpGet]
        public async Task<ActionResult> GetMatches()
        {
            var matches = await _context.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Select(m => new
                {
                    m.MatchID,
                    HomeTeam = m.HomeTeam.Name,
                    m.HomeTeamID,
                    AwayTeam = m.AwayTeam.Name,
                    m.AwayTeamID,
                    m.HomeScore,
                    m.AwayScore
                })
                .ToListAsync();

            return Ok(matches);
        }

        [Authorize(Roles = "SuperAdmin,Admin,User")]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetMatchById (string id)
        {
            var match = await _context.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Where(m=> m.MatchID == id)
                .Select(m => new
                {
                    m.MatchID,
                    HomeTeam = m.HomeTeam!.Name,
                    m.HomeTeamID,
                    AwayTeam = m.AwayTeam!.Name,
                    m.AwayTeamID,
                    m.HomeScore,
                    m.AwayScore
                })
                .FirstOrDefaultAsync();
            if(match is null)
            {
                return NotFound();
            }
            return Ok(match);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost]
        public async Task<ActionResult> CreateMatch(Match newMatch)
        {
            if (newMatch.HomeTeamID == newMatch.AwayTeamID)
                return BadRequest("A team cannot play itself.");

            var homeExists = await _context.Teams.AnyAsync(t => t.TeamID == newMatch.HomeTeamID);
            var awayExists = await _context.Teams.AnyAsync(t => t.TeamID == newMatch.AwayTeamID);

            if (!homeExists || !awayExists)
                return BadRequest("One or both teams do not exist.");

            if (newMatch.HomeScore < 0 || newMatch.AwayScore < 0)
                return BadRequest("Scores cannot be negative.");

            _context.Matches.Add(newMatch);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMatches), new { id = newMatch.MatchID }, newMatch);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMatch(string id, Match updatedMatch)
        {
            var match = await _context.Matches.FindAsync(id);

            if (match == null)
                return NotFound();

            if (updatedMatch.HomeTeamID == updatedMatch.AwayTeamID)
                return BadRequest("A team cannot play itself.");

            var homeExists = await _context.Teams
                .AnyAsync(t => t.TeamID == updatedMatch.HomeTeamID);

            var awayExists = await _context.Teams
                .AnyAsync(t => t.TeamID == updatedMatch.AwayTeamID);

            if (!homeExists || !awayExists)
                return BadRequest("One or both teams do not exist.");

            if (updatedMatch.HomeScore < 0 || updatedMatch.AwayScore < 0)
                return BadRequest("Scores cannot be negative.");

            match.HomeTeamID = updatedMatch.HomeTeamID;
            match.AwayTeamID = updatedMatch.AwayTeamID;
            match.HomeScore = updatedMatch.HomeScore;
            match.AwayScore = updatedMatch.AwayScore;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatch(string id)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match is null)
            {
                return NotFound();
            }
            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();


            return NoContent();
        }
    }
}
