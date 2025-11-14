using LibraryAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController(LibraryContext ctx) : ControllerBase
    {
        [HttpGet("{limit}")]
        public async Task<IActionResult> GetReviews(int limit)
        {
            var reviews = await ctx.BooksReviews.Select(r => new { r.ReviewId, r.ReviewText, r.ReviewRate, r.ReviewUser.UserId, r.ReviewUser.UserNick, r.ReviewUser.UserImage }).Take(limit).ToListAsync();
            if (reviews.Count == 0) return NotFound();
            else return Ok(reviews);
        }
    }
}
