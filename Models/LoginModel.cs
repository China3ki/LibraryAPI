using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models
{
    public class LoginModel
    {
        [MinLength(1)]
        public string Email { get; set; }
        [MinLength(8)]
        public string Password { get; set; }
    }
}
