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
        public async Task<IActionResult> GetCategories()
        {
            var categories = await ctx.Categories.Select(c => new { c.CategoryId, c.CategoryName }).ToListAsync();
            if (categories.Count == 0) return NotFound();
            else return Ok(categories);
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
