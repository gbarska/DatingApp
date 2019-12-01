using System;
using DatingApp.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace DatingApp.API.DTOs
{
    public class PhotosForReturnDTO
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
    }
}