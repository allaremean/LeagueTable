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
    public class TeamsController(LaligaDbContext context) : ControllerBase
    {
        private readonly LaligaDbContext _context = context;

        [Authorize(Roles ="SuperAdmin,Admin,User")]
        [HttpGet]
        public async Task<ActionResult<List<Team>>> GetTeams()
        {
            return Ok(await _context.Teams.ToListAsync());
        }

        [Authorize(Roles = "SuperAdmin,Admin,User")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeamById(string id)
        {
            var game = await _context.Teams.FindAsync(id);
            if (game is null)
            {
                return NotFound();
            }
            return Ok(game);

        }
        [Authorize(Roles ="SuperAdmin,Admin")]
        [HttpPost]
        public async Task<ActionResult<Team>> AddTeam(Team newTeam)
        {
            if (newTeam is null)
            {
                return BadRequest();
            }
            _context.Teams.Add(newTeam);
            await _context.SaveChangesAsync();
            

            return CreatedAtAction(nameof(GetTeamById), new { id = newTeam.TeamID }, newTeam);

        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(string id, Team updatedTeam)
        {
            var game = await _context.Teams.FindAsync(id);
            if (game is null)
            {
                return NotFound();
            }
            game.Name = updatedTeam.Name;
            game.City = updatedTeam.City;
            game.Stadium= updatedTeam.Stadium;
            
            await _context.SaveChangesAsync();

            return NoContent();

        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(string id)
        {
            var team = await _context.Teams.FindAsync(id);

            if (team is null)
                return NotFound();

            var hasMatches = await _context.Matches
                .AnyAsync(m => m.HomeTeamID == id || m.AwayTeamID == id);

            if (hasMatches)
                return BadRequest("Cannot delete a team that has matches recorded.");

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
