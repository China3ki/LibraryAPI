using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models
{
    public class LoginDto
    {
        [MinLength(1)]
        public string Email { get; set; } = string.Empty;
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;
    }
}
