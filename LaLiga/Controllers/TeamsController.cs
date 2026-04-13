using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using LaLiga.Core.Models;
using LaLiga.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace LaLiga.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController(ITeamService teamService) : ControllerBase
    {
        private readonly ITeamService _teamService = teamService;

        [Authorize(Roles ="SuperAdmin,Admin,User")]
        [HttpGet]
        public async Task<ActionResult<List<Team>>> GetTeams()
        {
            return Ok(await _teamService.GetTeamsAsync());
        }

        [Authorize(Roles = "SuperAdmin,Admin,User")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Team>> GetTeamById(string id)
        {
            var game = await _teamService.GetTeamByIdAsync(id);
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
            var result = await _teamService.AddTeamAsync(newTeam);
            if (result is null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetTeamById), new { id = result.TeamID }, result);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(string id, Team updatedTeam)
        {
            var success = await _teamService.UpdateTeamAsync(id, updatedTeam);
            if (!success)
            {
                return NotFound();
            }

            return NoContent(); 
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(string id)
        {
            var result = await _teamService.DeleteTeamAsync(id);

            if (!result.Success && result.Message == "Team not found.")
                return NotFound();

            if (!result.Success)
                return BadRequest(result.Message);

            return NoContent();
        }
    }
}
