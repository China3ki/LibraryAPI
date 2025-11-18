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
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var user = await ctx.Users.Select(u => new { u.UserId, u.UserNick, u.UserImage }).FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null) return NotFound();
            else return Ok(user);
        }

        [HttpGet("followers/{userId}")]
        public async Task<IActionResult> GetFollowers(int userId)
        {
            var user = await ctx.UsersFollowers.Where(f => f.UserFollowedId == userId).Select(f => new { f.UserFollow.UserId, f.UserFollow.UserNick, f.UserFollow.UserImage }).ToListAsync();
            if (user.Count == 0) return NotFound();
            else return Ok(user);
        }
        [HttpGet("followed/{userId}")]
        public async Task<IActionResult> GetFollowed(int userId)
        {
            var user = await ctx.UsersFollowers.Where(f => f.UserFollowId == userId).Select(f => new { f.UserFollowed.UserId, f.UserFollowed.UserNick, f.UserFollowed.UserImage }).ToListAsync();
            if (user.Count == 0) return NotFound();
            else return Ok(user);
        }
        [HttpGet("session/{email}")]
        public async Task<IActionResult> GetSessionData(string email)
        {
            var user = await ctx.Users.Select(u => new { u.UserId, u.UserNick, u.UserImage, u.UserEmail, u.UserAdmin } ).FirstOrDefaultAsync(u => u.UserEmail == email);
            if (user == null) return NotFound();
            else return Ok(user);
        }

        [HttpPut]
        public async Task<IActionResult> UploadAvatar(UploadDto avatar)
        {
            var user = await ctx.Users.FirstOrDefaultAsync(u => u.UserId == avatar.UserId);
            if (user is null) return BadRequest();
            var path = await service.Upload(user.UserNick, avatar.File, ImageType.User);
            user.UserImage = path;
            ctx.Entry(user).CurrentValues.SetValues(path);
            await ctx.SaveChangesAsync();
            return Ok();
        }
    }
}
