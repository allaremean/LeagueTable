using LaLiga.Infrastructure.Data;
using LaLiga.Core.Models;
using LaLiga.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaLiga.Infrastructure.Services
{
    public class TeamService(LaligaDbContext context) : ITeamService
    {
        private readonly LaligaDbContext _context = context;

        public async Task<List<Team>> GetTeamsAsync()
        {
            return await _context.Teams.ToListAsync();
        }

        public async Task<Team?> GetTeamByIdAsync(string id)
        {
            return await _context.Teams.FindAsync(id);
        }

        public async Task<Team?> AddTeamAsync(Team newTeam)
        {
            if (newTeam is null) return null;

            _context.Teams.Add(newTeam);
            await _context.SaveChangesAsync();
            return newTeam;
        }

        public async Task<bool> UpdateTeamAsync(string id, Team updatedTeam)
        {
            var game = await _context.Teams.FindAsync(id);
            if (game is null) return false;

            game.Name = updatedTeam.Name;
            game.City = updatedTeam.City;
            game.Stadium = updatedTeam.Stadium;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(bool Success, string Message)> DeleteTeamAsync(string id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team is null) return (false, "Team not found.");

            var hasMatches = await _context.Matches
                .AnyAsync(m => m.HomeTeamID == id || m.AwayTeamID == id);

            if (hasMatches) return (false, "Cannot delete a team that has matches recorded.");

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return (true, "Success");
        }
    }
}
