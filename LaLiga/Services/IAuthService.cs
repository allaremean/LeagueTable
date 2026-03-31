using LaLiga.Models;

namespace LaLiga.Services
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
