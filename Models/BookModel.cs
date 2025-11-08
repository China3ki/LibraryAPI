using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models
{
    public class BookModel
    {
        [StringLength(50, MinimumLength = 5)]
        public string Title { get; set; }
        [MinLength(5)]
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int AuthorId { get; set; }
        public short ReleaseDate { get; set; }
        public int Amount { get; set; }
        public IFormFile Image { get; set; }
    }
}
