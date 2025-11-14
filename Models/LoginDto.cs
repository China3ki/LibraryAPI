using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models
{
    public class LoginDto
    {
        [MinLength(1)]
        public string LoginEmail { get; set; } = string.Empty;
        [MinLength(8)]
        public string LoginPassword { get; set; } = string.Empty;
    }
}
