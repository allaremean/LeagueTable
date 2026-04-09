using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LaLiga.Models;
using LaLiga.Services;
using Microsoft.AspNetCore.Authorization;

namespace LaLiga.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController(IMatchService matchService) : ControllerBase
    {
        private readonly IMatchService _matchService = matchService;

        [Authorize(Roles ="SuperAdmin,Admin,User")]
        [HttpGet]
        public async Task<ActionResult> GetMatches()
        {
            var matches = await _matchService.GetMatchesAsync();
            return Ok(matches);
        }

        [Authorize(Roles = "SuperAdmin,Admin,User")]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetMatchById (string id)
        {
            var match = await _matchService.GetMatchByIdAsync(id);
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
            var result = await _matchService.CreateMatchAsync(newMatch);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return CreatedAtAction(nameof(GetMatches), new { id = result.CreatedMatch!.MatchID }, result.CreatedMatch);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMatch(string id, Match updatedMatch)
        {
            var result = await _matchService.UpdateMatchAsync(id, updatedMatch);

            if (!result.Success && result.Message == "Not found")
            {
                return NotFound();
            }

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return NoContent();
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatch(string id)
        {
            var success = await _matchService.DeleteMatchAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
