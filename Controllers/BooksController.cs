using LibraryAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController(LibraryContext ctx) : ControllerBase
    {
        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await ctx.Books.Select(b => new { b.BookId, b.BookTitle, b.BookAuthor.AuthorName, b.BookAuthor.AuthorSurname, b.BookCategory.CategoryName }).FirstOrDefaultAsync(b => b.BookId == id);
            if (book == null) return NotFound();
            else return Ok(book);
        }
        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetBooksByCategory(string category)
        {
            var books = await ctx.Books.Select(b => new { b.BookId, b.BookTitle, b.BookAuthor.AuthorName, b.BookAuthor.AuthorSurname, b.BookCategory.CategoryName }).ToListAsync();
            if (books == null) return NotFound();
            else return Ok(books);
        }
    }
}
