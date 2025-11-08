using LibraryAPI.Entities;
using LibraryAPI.Models;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthRegister(LibraryContext ctx, AuthService service, HasherService hasher) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([Required] RegisterModel user)
        {
            if (String.IsNullOrEmpty(user.Nickname)) return Conflict("Nickname cannot be null");
            if (String.IsNullOrEmpty(user.Email)) return Conflict("Email cannot be null!");
            if (!service.VerifyEmail(user.Email).Result) return Conflict("Account already exist!");
            if (!service.VerifyPasswordEquality(user.Password, user.ConfirmedPassword)) return Conflict("Email already exist or email does not pass throught a validation!");
            if (!service.VerifyPasswordRequirements(user.Password)) return Conflict("Email Password does not meet the requirements!");
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
        [HttpPost("login")]
        public async Task<IActionResult> Login([Required] LoginModel user)
        {
            if (!service.VerifyAccount(user.Email).Result) return Unauthorized("Account does not exist!");
            string password = await service.GetPasswordDb(user.Email);
            if (!hasher.VerifyPassword(user.Password, password)) return Unauthorized("Wrong password!");
            else return Ok();
        }
    }
}
