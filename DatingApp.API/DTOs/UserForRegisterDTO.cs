using System;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTOs
{
    public class UserForRegisterDTO
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(8,MinimumLength = 4, ErrorMessage = "Password specified must have between 4 and 8 characters")]
        public string Password { get; set; }

        public string Gender { get; set; }

        public string  KnownAs { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string City { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastActive { get; set; }

        public UserForRegisterDTO()
        {
            CreatedAt = DateTime.Now;
            LastActive = DateTime.Now;
        }
    }
}