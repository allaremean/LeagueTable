using LaLiga.Data;
using LaLiga.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaLiga.Services
{
    public class MatchService(LaligaDbContext context) : IMatchService
    {
        private readonly LaligaDbContext _context = context;

        public async Task<IEnumerable<object>> GetMatchesAsync()
        {
            return await _context.Matches
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
        }

        public async Task<object?> GetMatchByIdAsync(string id)
        {
            return await _context.Matches
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Where(m => m.MatchID == id)
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
        }

        public async Task<(bool Success, string Message, Match? CreatedMatch)> CreateMatchAsync(Match newMatch)
        {
            if (newMatch.HomeTeamID == newMatch.AwayTeamID)
                return (false, "A team cannot play itself.", null);

            var homeExists = await _context.Teams.AnyAsync(t => t.TeamID == newMatch.HomeTeamID);
            var awayExists = await _context.Teams.AnyAsync(t => t.TeamID == newMatch.AwayTeamID);

            if (!homeExists || !awayExists)
                return (false, "One or both teams do not exist.", null);

            if (newMatch.HomeScore < 0 || newMatch.AwayScore < 0)
                return (false, "Scores cannot be negative.", null);

            _context.Matches.Add(newMatch);
            await _context.SaveChangesAsync();

            return (true, "Success", newMatch);
        }

        public async Task<(bool Success, string Message)> UpdateMatchAsync(string id, Match updatedMatch)
        {
            var match = await _context.Matches.FindAsync(id);

            if (match == null)
                return (false, "Not found");

            if (updatedMatch.HomeTeamID == updatedMatch.AwayTeamID)
                return (false, "A team cannot play itself.");

            var homeExists = await _context.Teams
                .AnyAsync(t => t.TeamID == updatedMatch.HomeTeamID);

            var awayExists = await _context.Teams
                .AnyAsync(t => t.TeamID == updatedMatch.AwayTeamID);

            if (!homeExists || !awayExists)
                return (false, "One or both teams do not exist.");

            if (updatedMatch.HomeScore < 0 || updatedMatch.AwayScore < 0)
                return (false, "Scores cannot be negative.");

            match.HomeTeamID = updatedMatch.HomeTeamID;
            match.AwayTeamID = updatedMatch.AwayTeamID;
            match.HomeScore = updatedMatch.HomeScore;
            match.AwayScore = updatedMatch.AwayScore;

            await _context.SaveChangesAsync();

            return (true, "Success");
        }

        public async Task<bool> DeleteMatchAsync(string id)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match is null)
                return false;

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
