using LaLiga.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaLiga.Services
{
    public interface IMatchService
    {
        Task<IEnumerable<object>> GetMatchesAsync();
        Task<object?> GetMatchByIdAsync(string id);
        Task<(bool Success, string Message, Match? CreatedMatch)> CreateMatchAsync(Match newMatch);
        Task<(bool Success, string Message)> UpdateMatchAsync(string id, Match updatedMatch);
        Task<bool> DeleteMatchAsync(string id);
    }
}
