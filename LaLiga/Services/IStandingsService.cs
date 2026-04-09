using LaLiga.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaLiga.Services
{
    public interface IStandingsService
    {
        Task<List<Standings>> GetStandingsAsync();
        Task<Standings?> GetStandingByTeamIdAsync(string teamId);
    }
}
