using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models
{
    public class BookDto
    {
        [StringLength(50)]
        public string Title { get; set; } = string.Empty;
        [MinLength(5)]
        public string Description { get; set; } = string.Empty;
        public int? CategoryId { get; set; }
        public int? AuthorId { get; set; }
        public short? ReleaseDate { get; set; }
        public int? Amount { get; set; }
        public IFormFile? Image { get; set; }
    }
}
