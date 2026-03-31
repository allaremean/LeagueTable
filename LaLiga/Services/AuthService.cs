
using LaLiga.Data;
using LaLiga.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LaLiga.Services
{
    public class AuthService(LaligaDbContext context, IConfiguration configuration): IAuthService
    {
       public async Task<User?> RegisterAsync(UserDto request)
        {
            if(await context.Users.AnyAsync(u=>u.Username == request.Username))
            {
                return null;
            }
            var user = new User(); 
            var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.Password);

            user.Username = request.Username;
            user.PasswordHash = hashedPassword;
            user.Role = "User";

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return user;
        }
        public async Task<string?> LoginAsync(UserDto request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if(user is null)
            {
                return null;
            }
            if(new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password)
                == PasswordVerificationResult.Failed)
            {
                return null;
            }
            return CreateToken(user);
        }

        public async Task<bool> DeleteUserAsync(string username)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return false;
            }

            context.Users.Remove(user);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> PromoteToAdmin(string username)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return false;
            }

            user.Role = "Admin";
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DemoteToUser(string username)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return false;
            }

            user.Role = "User";
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await context.Users.Select(u => new User
            {
                Id = u.Id,
                Username = u.Username,
                Role = u.Role,

            }).ToListAsync();
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)

            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        }
    }
}
