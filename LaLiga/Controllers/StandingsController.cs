using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LaLiga.Services;
using Microsoft.AspNetCore.Authorization;

namespace LaLiga.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StandingsController(IStandingsService standingsService) : ControllerBase
    {
        private readonly IStandingsService _standingsService = standingsService;
        
        [HttpGet]
        public async Task<ActionResult> GetStandings()
        {
            var standings = await _standingsService.GetStandingsAsync();
            return Ok(standings);
        }

        [HttpGet("{teamId}")]
        public async Task<ActionResult> GetStandingByTeamId(string teamId)
        {
            var teamStanding = await _standingsService.GetStandingByTeamIdAsync(teamId);

            if (teamStanding == null)
                return NotFound();

            return Ok(teamStanding);
        }
    }
}
