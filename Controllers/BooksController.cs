using LibraryAPI.Entities;
using LibraryAPI.Models;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController(LibraryContext ctx,  UploadService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetBooks(int? categoryId, string? bookTitle, int? startRate, int? endRate, int? startDate, int? endDate, string? sortBy, int? start, int? limit,  bool desc = false)
        {
            var books = ctx.Books.Select(b => new { b.BookId, b.BookTitle, b.BookDescription, b.BookCategoryId, b.BookAuthor.AuthorName, b.BookAuthor.AuthorSurname, b.BookCategory.CategoryName, b.BookReleaseDate, b.BookCover, AverageRate = b.BooksReviews.Where(r => r.ReviewBookId == b.BookId).Select(r => r.ReviewRate).Average() }).AsQueryable();
            
            if (categoryId != null) books = books.Where(b => b.BookCategoryId == categoryId);
            if (!String.IsNullOrEmpty(bookTitle)) books = books.Where(b => b.BookTitle.Contains(bookTitle));
            if (startDate != null) books = books.Where(b => b.BookReleaseDate >= startDate);
            if (endDate != null) books = books.Where(b => b.BookReleaseDate <= endDate);
            if (startRate != null) books = books.Where(b => b.AverageRate >= startRate);
            if (endRate != null) books = books.Where(b => b.AverageRate <= endRate);

            if (start != null) books = books.Skip((int)start);
            // Sorting options
            if(!String.IsNullOrEmpty(sortBy))
            {
                books = sortBy.ToLower() switch
                {
                    "rate" => desc ? books.OrderByDescending(b => b.AverageRate) : books.OrderBy(b => b.AverageRate),
                    "date" => desc ? books.OrderByDescending(b => b.BookReleaseDate) : books.OrderBy(b => b.BookReleaseDate),
                    "title" => desc ? books.OrderByDescending(b => b.BookTitle) : books.OrderBy(b => b.BookTitle),
                    "id" => desc ? books.OrderByDescending(b => b.BookId) : books.OrderBy(b => b.BookId),
                    "category" => desc ? books.OrderByDescending(b => b.CategoryName) : books.OrderBy(b => b.CategoryName),
                    "author" => desc ? books.OrderByDescending(b => b.AuthorName) : books.OrderBy(b => b.AuthorName),
                    _ => books
                };
            }
            if (limit != null) books = books.Take((int)limit);
            var query = await books.ToListAsync();
            if (query.Count == 0) return NotFound();
            else return Ok(query);
        }
        [HttpGet("count")]
        public async Task<IActionResult> GetNumberOfBooks()
        {
            var count = await ctx.Books.CountAsync();
            return Ok(count);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await ctx.Books.Select(b => new { b.BookId, b.BookTitle, b.BookDescription, b.BookCategoryId, b.BookAuthorId, b.BookAuthor.AuthorName, b.BookAuthor.AuthorSurname, b.BookCategory.CategoryName, b.BookReleaseDate, b.BookAmount, AverateRate = b.BooksReviews.Where(r => r.ReviewBookId == b.BookId).Select(r => r.ReviewRate).Average() }).FirstOrDefaultAsync(b => b.BookId == id);
            if (book == null) return NotFound();
            else return Ok(book);
        }
        [HttpGet("favourite/{userId}")]
        public async Task<IActionResult> GetFavouriteBooks(int userId)
        {
            var books = await ctx.UsersFavourites.Where(f => f.FavouriteUserId == userId).Select(f => new { f.FavouriteBook.BookId, f.FavouriteBook.BookCover }).ToListAsync();
            if (books.Count == 0) return NotFound();
            else return Ok(books);
        }

        [HttpPost]
        public async Task<IActionResult> AddBook([Required] [FromForm] BookDto newBook) 
        {
            string pathfile = await service.Upload(newBook.Title, newBook.Image, ImageType.Cover);
            Book book = new Book
            {
                BookTitle = newBook.Title,
                BookDescription = newBook.Description,
                BookCategoryId = newBook.CategoryId,
                BookAuthorId = newBook.AuthorId,
                BookReleaseDate = newBook.ReleaseDate,
                BookAmount = newBook.Amount,
                BookCover = pathfile
            };
            ctx.Add<Book>(book);
            await ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBook), new { id = book.BookId }, newBook);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateBook([FromForm] BookDto updateBook, [FromForm][Required] int bookId)
        {
            var book = await ctx.Books.FirstOrDefaultAsync(b => b.BookId == bookId);
            if (book is null) return BadRequest( new { message = "Book is null"});
            book.BookTitle = updateBook.Title;
            book.BookDescription = updateBook.Description;
            book.BookCategoryId = updateBook.CategoryId;
            book.BookAuthorId = updateBook.AuthorId;
            book.BookReleaseDate = updateBook.ReleaseDate;
            book.BookAmount = updateBook.Amount;
            if (updateBook.Image != null)
            {
                string path = await service.Upload(updateBook.Title, updateBook.Image, ImageType.Cover);
                book.BookCover = path;
            }
            await ctx.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteBook(int BookId)
        {
            var book = await ctx.Books.Where(b => b.BookId == BookId).FirstOrDefaultAsync();
            if (book is null) return BadRequest();
            ctx.Remove<Book>(book);
            await ctx.SaveChangesAsync();
            return NoContent();
        }
    }
}
