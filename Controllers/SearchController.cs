using LibraryAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController(LibraryContext ctx) : ControllerBase
    {
        [HttpGet("books/{title}")]
        public async Task<IActionResult> GetBookForSearch(string title)
        {
            var books = await ctx.Books.Where(b => b.BookTitle.Contains(title)).Select(b => new { b.BookId, b.BookTitle, b.BookAuthor.AuthorName, b.BookAuthor.AuthorSurname, b.BookCover, AverateRate = b.BooksReviews.Where(r => r.ReviewBookId == b.BookId).Select(r => r.ReviewRate).Average(), WantsToRead = b.UsersReadeds.Count(w => w.ReadBookId == b.BookId) }).Take(5).ToListAsync();
            if (books.Count == 0) return NotFound();
            else return Ok(books);
        }
        [HttpGet("authors/{name}")]
        public async Task<IActionResult> GetAuthorsForSearch(string name)
        {
            var authors = await ctx.Authors.Where(a => a.AuthorName.Contains(name) || a.AuthorSurname.Contains(name)).Select(a => new { a.AuthorId, a.AuthorName, a.AuthorSurname, a.AuthorImage, bookCreated = a.Books.Count(b => b.BookAuthorId == a.AuthorId) }).Take(5).ToListAsync();
            if (authors.Count == 0) return NotFound();
            else return Ok(authors);
        }
        [HttpGet("users/{nick}")]
        public async Task<IActionResult> GetUser(string nick)
        {
            var users = await ctx.Users.Where(u => u.UserNick.Contains(nick)).Select(u => new { u.UserId, u.UserNick, u.UserImage, userWantRead = u.UsersReadeds.Count(r => r.ReadUserId == u.UserId), userReviews = u.BooksReviews.Count(r => r.ReviewUserId == u.UserId), UserFollowers = u.UsersFollowerUserFolloweds.Count(f => f.UserFollowedId == u.UserId), userFollowed = u.UsersFollowerUserFollows.Count(f => f.UserFollowId == u.UserId) }).ToListAsync();
            if (users.Count == 0) return NotFound();
            else return Ok(users);
        }
    }
}
