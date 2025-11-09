using LibraryAPI.Entities;
using LibraryAPI.Models;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(LibraryContext ctx, UploadService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await ctx.Users.Select(u => new { u.UserName, u.UserSurname, u.UserNick, u.UserEmail, u.UserJoiningDate, u.UserImage }).ToListAsync();
            if (users.Count == 0) return NotFound();
            else return Ok(users);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await ctx.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (user is null) return BadRequest();
            ctx.Remove<User>(user);
            await ctx.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut]
        public async Task<IActionResult> UploadAvatar(UploadDto avatar)
        {
            var user = await ctx.Users.FirstOrDefaultAsync(u => u.UserId == avatar.UserId);
            if (user is null) return BadRequest();
            var path = service.Upload(user.UserNick, avatar.File, ImageType.User);
            ctx.Entry(user).CurrentValues.SetValues(avatar.File);
            await ctx.SaveChangesAsync();
            return Ok();
        }
    }
}
