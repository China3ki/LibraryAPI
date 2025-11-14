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
    public class AuthorsController(LibraryContext ctx, UploadService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            var authors = await ctx.Authors.Select(a => new { a.AuthorId, a.AuthorName, a.AuthorSurname }).ToListAsync();
            if (authors.Count == 0) return NotFound();
            else return Ok(authors);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthor(int id)
        {
            var author = await ctx.Authors.Select(a => new { a.AuthorId, a.AuthorName, a.AuthorSurname }).FirstOrDefaultAsync(a => a.AuthorId == id);
            if (author == null) return NotFound();
            else return Ok(author);
        }
        [HttpGet("search/{name}")]
        public async Task<IActionResult> GetAuthorsForSearch(string name)
        {
            var authors = await ctx.Authors.Where(a => a.AuthorName.Contains(name) || a.AuthorSurname.Contains(name)).Select(a => new { a.AuthorId, a.AuthorName, a.AuthorSurname, a.AuthorImage, bookCreated = a.Books.Count(b => b.BookAuthorId == a.AuthorId) }).Take(5).ToListAsync();
            if (authors.Count == 0) return NotFound();
            else return Ok(authors);
        }
        [HttpPost]
        public async Task<IActionResult> AddAuthor([Required] AuthorDto newAuthor)
        {
            string? filepath = null;
            filepath = await service.Upload($"{newAuthor.Name} {newAuthor.Surname}", newAuthor.Image, ImageType.Author);
            Author author = new Author
            {
                AuthorName = newAuthor.Name,
                AuthorSurname = newAuthor.Surname,
                AuthorImage = filepath
            };
            ctx.Add<Author>(author);
            await ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAuthor), new { id = author.AuthorId }, newAuthor);
        }
    }
}
