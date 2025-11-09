using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models
{
    public class RegisterDto
    {
        [MaxLength(50)]
        public string Nickname { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Email { get; set; } = string.Empty;
        [MaxLength(50)]
        public string Password { get; set; } = string.Empty;
        [MaxLength(50)]
        public string ConfirmedPassword { get; set; } = string.Empty;

    }
}
