using LaLiga.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaLiga.Core.Interfaces
{
    public interface ITeamService
    {
        Task<List<Team>> GetTeamsAsync();
        Task<Team?> GetTeamByIdAsync(string id);
        Task<Team?> AddTeamAsync(Team newTeam);
        Task<bool> UpdateTeamAsync(string id, Team updatedTeam);
        Task<(bool Success, string Message)> DeleteTeamAsync(string id);
    }
}
