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
        public async Task<IActionResult> GetAuthors(string? sortBy, int? limit, int? start, bool desc = false)
        {
            var authors =  ctx.Authors.Select(a => new { a.AuthorId, a.AuthorName, a.AuthorSurname }).AsQueryable();

            if (start != null) authors = authors.Skip((int)start);

            if(!string.IsNullOrEmpty(sortBy))
            {
                authors = sortBy.ToLower() switch
                {
                    "id" => desc ? authors.OrderByDescending(a => a.AuthorId) : authors.OrderBy(a => a.AuthorId),
                    "name" => desc ? authors.OrderByDescending(a => a.AuthorName) : authors.OrderBy(a => a.AuthorName),
                    "surname" => desc ? authors.OrderByDescending(a => a.AuthorSurname)  : authors.OrderBy(a => a.AuthorSurname),
                    _ => authors
                };
            }

            if (limit != null) authors = authors.Take((int)limit);
            var query = await  authors.ToListAsync();
            if (query.Count == 0) return NotFound();
            else return Ok(query);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthor(int id)
        {
            var author = await ctx.Authors.Select(a => new { a.AuthorId, a.AuthorName, a.AuthorSurname }).FirstOrDefaultAsync(a => a.AuthorId == id);
            if (author == null) return NotFound();
            else return Ok(author);
        }
        [HttpGet("count")]
        public async Task<IActionResult> GetNumberOfAuthors()
        {
            var count = await ctx.Authors.CountAsync();
            return Ok(count);
        }
        [HttpPost]
        public async Task<IActionResult> AddAuthor([FromForm][Required] AuthorDto newAuthor)
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
        [HttpDelete("{authorId}")]
        public async Task<IActionResult> DeleteAuthor(int authorId)
        {
            var author = await ctx.Authors.FirstOrDefaultAsync(a => a.AuthorId == authorId);
            if (author is null) return BadRequest();
            ctx.Remove<Author>(author);
            await ctx.SaveChangesAsync();
            return NoContent();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateAuthor([FromForm] AuthorDto authorDto, [FromForm][Required] int authorId)
        {
            var author = await ctx.Authors.FirstOrDefaultAsync(a => a.AuthorId == authorId);
            if (author is null) return BadRequest();
            author.AuthorName = authorDto.Name;
            author.AuthorSurname = authorDto.Surname;
            if(authorDto.Image != null)
            {
                string filepath = await service.Upload($"{authorDto.Name} {authorDto.Surname}", authorDto.Image, ImageType.Author);
                author.AuthorImage = filepath;
            }
            await ctx.SaveChangesAsync();
            return Ok();
        }

    }
}
