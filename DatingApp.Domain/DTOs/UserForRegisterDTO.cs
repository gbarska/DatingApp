using System.ComponentModel.DataAnnotations;

namespace DatingApp.Domain.DTOs
{
    public class UserForRegisterDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(8,MinimumLength = 4, ErrorMessage = "Password specified must have between 4 and 8 characters")]
        public string Password { get; set; }
    }
}