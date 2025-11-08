using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models
{
    public class RegisterModel
    {
        [MaxLength(50)]
        public string Nickname { get; set; }
        [MaxLength(50)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string Password { get; set; }
        [MaxLength(50)]
        public string ConfirmedPassword { get; set; }

    }
}
