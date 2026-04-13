using LaLiga.Core.Models;

namespace LaLiga.Core.Interfaces
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDto request);
        Task<string?> LoginAsync(UserDto request);

        Task<bool> DeleteUserAsync (string username);
        Task<bool> PromoteToAdmin(string username);

        Task<bool> DemoteToUser(string username);

        Task<IEnumerable<User>> GetAllUsersAsync();
    }
}
