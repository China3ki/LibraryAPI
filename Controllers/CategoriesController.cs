using LibraryAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(LibraryContext ctx) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCategories(string? sortBy, int? limit, int? start, bool desc = false)
        {
            var categories =  ctx.Categories.Select(c => new { c.CategoryId, c.CategoryName }).AsQueryable();

            if (!string.IsNullOrEmpty(sortBy))
            {
                categories = sortBy.ToLower() switch
                {
                    "id" => desc ? categories.OrderByDescending(c => c.CategoryId) : categories.OrderBy(c => c.CategoryId),
                    "name" => desc ? categories.OrderByDescending(c => c.CategoryName) : categories.OrderBy(c => c.CategoryName),
                    _ => categories
                };
            }

            if (limit != null) categories = categories.Take((int)limit);
            if (start != null) categories =categories.Skip((int)start);

            var query = await categories.ToListAsync();
            if (query.Count == 0) return NotFound();
            else return Ok(categories);
        }
        [HttpGet("count")]
        public async Task<IActionResult> CountCategories()
        {
            var count = await ctx.Categories.CountAsync();
            return Ok(count);
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory(string name)
        {
            ctx.Add<Category>(new Category { CategoryName = name });
            await ctx.SaveChangesAsync();
            return Created();
        }
    }
}
