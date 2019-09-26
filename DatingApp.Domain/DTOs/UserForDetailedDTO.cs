using System;
using System.Collections.Generic;
using DatingApp.Domain.Models;

namespace DatingApp.Domain.DTOs
{
    public class UserForDetailedDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string KnownAs { get; set; }
        public string  Gender { get; set; }
        public int Age { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastActive { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhotoUrl { get; set; }
        ICollection<PhotosForDetailedDTO> Photos { get; set; }
    }
}