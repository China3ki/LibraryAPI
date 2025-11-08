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
        public async Task<IActionResult> GetBooks(int? categoryId, string? bookTitle, int? startRate, int? endRate, int? startDate, int? endDate, string? sortBy, int? limit,  bool desc = false)
        {
            var books = ctx.Books.Select(b => new { b.BookId, b.BookTitle, b.BookDescription, b.BookCategoryId, b.BookAuthor.AuthorName, b.BookAuthor.AuthorSurname, b.BookCategory.CategoryName, b.BookReleaseDate, AverateRate = b.BooksReviews.Where(r => r.ReviewBookId == b.BookId).Select(r => r.ReviewRate).Average() }).AsQueryable();
            
            if (categoryId != null) books = books.Where(b => b.BookCategoryId == categoryId);
            if (!String.IsNullOrEmpty(bookTitle)) books = books.Where(b => b.BookTitle.Contains(bookTitle));
            if (startDate != null) books = books.Where(b => b.BookReleaseDate >= startDate);
            if (endDate != null) books = books.Where(b => b.BookReleaseDate <= endDate);
            if (startRate != null) books = books.Where(b => b.AverateRate >= startRate);
            if (endRate != null) books = books.Where(b => b.AverateRate <= endRate);

            // Sorting options
            if(!String.IsNullOrEmpty(sortBy))
            {
                books = sortBy.ToLower() switch
                {
                    "rate" => desc ? books.OrderByDescending(b => b.AverateRate) : books.OrderBy(b => b.AverateRate),
                    "date" => desc ? books.OrderByDescending(b => b.BookReleaseDate) : books.OrderBy(b => b.BookReleaseDate),
                    "title" => desc ? books.OrderByDescending(b => b.BookTitle) : books.OrderBy(b => b.BookTitle),
                    "id" => desc ? books.OrderByDescending(b => b.BookId) : books.OrderBy(b => b.BookId),
                    _ => books
                };
            }
            if (limit != null) books.Take((int)limit);
            var query = await books.ToListAsync();
            if (query.Count == 0) return NotFound();
            else return Ok(query);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var book = await ctx.Books.Select(b => new { b.BookId, b.BookTitle, b.BookDescription, b.BookCategoryId, b.BookAuthor.AuthorName, b.BookAuthor.AuthorSurname, b.BookCategory.CategoryName, b.BookReleaseDate, AverateRate = b.BooksReviews.Where(r => r.ReviewBookId == b.BookId).Select(r => r.ReviewRate).Average() }).FirstOrDefaultAsync(b => b.BookId == id);
            if (book == null) return NotFound();
            else return Ok(book);
        }
        [HttpPost]
        public async Task<IActionResult> AddBook(BookModel newBook) 
        {
            string pathfile = await service.UploadCover(newBook.Title, newBook.Image, ImageType.Author);
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
    }
}
