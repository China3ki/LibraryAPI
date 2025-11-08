using LibraryAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(LibraryContext ctx) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await ctx.Users.Select(u => new { u.UserName, u.UserSurname, u.UserNick, u.UserEmail, u.UserJoiningDate, u.UserImage }).ToListAsync();
            if (users.Count == 0) return NotFound();
            else return Ok(users);
        }
    }
}
