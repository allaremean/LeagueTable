using LaLiga.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaLiga.Core.Interfaces
{
    public interface IStandingsService
    {
        Task<List<Standings>> GetStandingsAsync();
        Task<Standings?> GetStandingByTeamIdAsync(string teamId);
    }
}
