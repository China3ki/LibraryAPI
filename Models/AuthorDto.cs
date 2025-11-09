using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models
{
    public class AuthorDto
    {
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;
        [StringLength(50, MinimumLength = 1)]
        public string Surname { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
    }
}
