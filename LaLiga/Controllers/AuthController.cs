using LaLiga.Core.Models;
using LaLiga.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LaLiga.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService): ControllerBase
    {
        public static User user = new User();

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register (UserDto request)
        {
            var user = await authService.RegisterAsync(request);
            if (user is null)
                return BadRequest("Username already exists");

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login (UserDto request)
        {
           var token = await authService.LoginAsync(request);
            if (token is null)
                return BadRequest("Invalid Username or Password");
            return Ok(token);   
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("getusers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await authService.GetAllUsersAsync();
            return Ok(users);
        }


        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("promote")]
        public async Task<IActionResult> Promote(string username)
        {
            var result = await authService.PromoteToAdmin(username);
            if (!result)
            {
                return NotFound("User doesn't exist");
            }

            return Ok($"User {username} has been promoted to Admin");
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("demote")]
        public async Task<IActionResult> Demote(string username)
        {
            var result = await authService.DemoteToUser(username);
            if (!result)
            {
                return NotFound("User doesn't exist");
            }

            return Ok($"Admin {username} has been demoted to User");
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {
            var result = await authService.DeleteUserAsync(username);

            if (!result)
            {
                return NotFound("User not found");
            }

            return Ok("User deleted successfully");
        }

    }
}
