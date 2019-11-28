using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using DatingApp.API.Helpers;
using DatingApp.Data;
using DatingApp.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using DatingApp.Domain.DTOs;
using System.Security.Claims;
using CloudinaryDotNet.Actions;
using DatingApp.Domain.Models;

namespace DatingApp.UI.Controller
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]

    //the ControllerBase doesn't offer view support it's perfect for API only 
    public class PhotosController : ControllerBase
    {
        private readonly IDatingRepository repository;
        private readonly IMapper mapper;
        private readonly IOptions<CloudinarySettings> cloudinaryConfig;
        private Cloudinary cloudinary;

        public PhotosController(IDatingRepository repository, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.cloudinaryConfig = cloudinaryConfig;

            Account acc = new Account(cloudinaryConfig.Value.CloudName,
            cloudinaryConfig.Value.ApiKey,
            cloudinaryConfig.Value.ApiSecret);

            cloudinary = new Cloudinary(acc);
            
        }
       
        [AllowAnonymous]
        [HttpGet("{id}", Name="GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await repository.GetPhoto(id);

            var photo = mapper.Map<PhotosForReturnDTO>(photoFromRepo);

            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm]PhotosForCreationDTO photoForCreationDTO)
        {
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

                var userFromRepo = await repository.GetUser(userId);

            var values = await repository.GetUser(userId);

            var file = photoForCreationDTO.File;

            var uploadResult = new  ImageUploadResult();

            if(file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation()
                        .Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = cloudinary.Upload(uploadParams);
                }
            }

            photoForCreationDTO.Url = uploadResult.Uri.ToString();
            photoForCreationDTO.PublicId = uploadResult.PublicId;

            var photo = mapper.Map<Photo> (photoForCreationDTO);

            if(!userFromRepo.Photos.Any(u => u.IsMain))
                photo.IsMain = true;

                userFromRepo.Photos.Add(photo);


                if(await repository.SaveAll())
                {
                    var photoForReturn = mapper.Map<PhotosForReturnDTO>(photo);
                    return CreatedAtRoute("GetPhoto", new {userId = userId, id = photo.Id}, photoForReturn );
                    // return CreatedAtRoute("GetPhoto", new {userId = userId, photo.Id}, photoForReturn );
                }
      
            return BadRequest("Could not add the photo");
        }


       
    }
}