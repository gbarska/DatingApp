using System;
using DatingApp.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace DatingApp.Domain.DTOs
{
    public class PhotosForCreationDTO
    {
        public string Url { get; set; }
        public IFormFile File { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public string PublicId { get; set; }
        public bool IsMain { get; set; }

        public PhotosForCreationDTO()
        {
            DateAdded = DateTime.Now;
        }
    }
}