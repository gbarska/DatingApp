using System;
using DatingApp.Domain.Models;

namespace DatingApp.Domain.DTOs
{
    public class UserForUpdateDTO
    {
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}