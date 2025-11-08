using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models
{
    public class AuthorModel
    {
        [StringLength(50, MinimumLength = 1)]
        public string Name { get; set; }
        [StringLength(50, MinimumLength = 1)]
        public string Surname { get; set; }
        public IFormFile? Image { get; set; }
    }
}
