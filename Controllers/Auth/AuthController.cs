using LibraryAPI.Entities;
using LibraryAPI.Models;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class Auth(LibraryContext ctx, AuthService service, HasherService hasher) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([Required] RegisterDto user)
        {
            if (String.IsNullOrEmpty(user.Nickname)) return Conflict("Nickname cannot be null");
            if (String.IsNullOrEmpty(user.Email)) return Conflict("Email cannot be null!");
            if (!service.VerifyEmail(user.Email).Result) return Conflict("Account already exist or email does not meet the requirements!");
            if (!service.VerifyPasswordEquality(user.Password, user.ConfirmedPassword)) return Conflict("Password are not the same!!");
            if (!service.VerifyPasswordRequirements(user.Password)) return Conflict("Password does not meet the requirements!");
            User newUser = new User
            {
                UserNick = user.Nickname.Trim(),
                UserEmail = user.Email.Trim(),
                UserPassword = hasher.HashPassword(user.Password),
                UserJoiningDate = DateOnly.FromDateTime(DateTime.Now),
                UserType = 1
            };
            ctx.Add<User>(newUser);
            await ctx.SaveChangesAsync();
            return Created();
        }
        [HttpPost("email/{email}")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            if (!service.VerifyEmail(email).Result) return Conflict("Account already exist or email does not meet the requirements!");
            else return Ok();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([Required] LoginDto user)
        {
            if (!service.VerifyAccount(user.LoginEmail).Result) return Unauthorized("Account does not exist!");
            string password = await service.GetPasswordDb(user.LoginEmail);
            if (!hasher.VerifyPassword(user.LoginPassword, password)) return Unauthorized("Wrong password!");
            else return Ok();
        }
    }
}
