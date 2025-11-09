using LibraryAPI.Entities;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksReadController(LibraryContext ctx): ControllerBase
    {
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetWantReadBooks(int userid)
        {
            var books = await ctx.UsersReadeds.Where(b => b.ReadUserId == userid).Select(b => new { b.ReadBook.BookTitle, b.ReadBook.BookAuthor.AuthorName, b.ReadBook.BookAuthor.AuthorSurname, b.ReadBook.BookCover }).ToListAsync();
            if (books.Count == 0) return NotFound();
            else return Ok(books);
        }
        [HttpPost]
        public async Task<IActionResult> AddBookWantToRead(BookToReadDto newRead)
        {
            UsersReaded read = new UsersReaded
            {
                ReadBookId = newRead.BookId,
                ReadUserId = newRead.UserId
            };
            ctx.Add<UsersReaded>(read);
            await ctx.SaveChangesAsync();
            return Created();
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveBookWantToRead(int readId)
        {
            var book = await ctx.UsersReadeds.FirstOrDefaultAsync(b => b.ReadId == readId);
            if (book is null) return BadRequest();
            ctx.Remove<UsersReaded>(book);
            await ctx.SaveChangesAsync();
            return NoContent();
        }
    }
}
