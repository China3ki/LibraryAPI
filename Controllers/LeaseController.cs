using LibraryAPI.Entities;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaseController(LibraryContext ctx) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetLease()
        {
            var lease = await ctx.BooksLeaseds.Select(l => new { l.LeaseId, l.LeaseUser.UserId, l.LeaseBook.BookTitle, l.LeaseStartDate, l.LeaseEndDate }).ToListAsync();
            if (lease.Count == 0) return NotFound();
            else return Ok(lease);
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetLease(int userId)
        {
            var lease = await ctx.BooksLeaseds.Select(l => new { l.LeaseId, l.LeaseUser.UserId, l.LeaseBook.BookTitle, l.LeaseStartDate, l.LeaseEndDate }).Where(i => i.UserId == userId).ToListAsync();
            if (lease.Count == 0) return NotFound();
            else return Ok(lease);
        }
        [HttpPost]
        public async Task<IActionResult> AddLease(LeaseModel newLease)
        {
            var checkIfUserExist = await ctx.Users.FirstOrDefaultAsync(u => u.UserId == newLease.UserId);
            if (checkIfUserExist == null) return BadRequest();
            var checkIfBookExist = await ctx.Books.FirstOrDefaultAsync(b => b.BookId == newLease.BookId);
            if (checkIfBookExist == null) return BadRequest();
            if (checkIfBookExist.BookAmount == 0) return BadRequest();
            var checkIfUserLeaseBook = await ctx.BooksLeaseds.FirstOrDefaultAsync(l => l.LeaseBookId == newLease.BookId && l.LeaseUserId == newLease.UserId);
            if (checkIfUserLeaseBook != null) return Conflict();
            BooksLeased lease = new BooksLeased
            {
                LeaseUserId = newLease.UserId,
                LeaseBookId = newLease.BookId,
                LeaseStartDate = DateOnly.FromDateTime(DateTime.Now),
                LeaseEndDate = DateOnly.FromDateTime(DateTime.Now.AddDays(7))
            };
            ctx.Add<BooksLeased>(lease);
            checkIfBookExist.BookAmount--;
            await ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLease), new { id = lease.LeaseUserId }, newLease);
        }
    }
}
