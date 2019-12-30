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
using DatingApp.API.DTOs;
using System.Security.Claims;
using CloudinaryDotNet.Actions;
using DatingApp.Domain.Models;

namespace DatingApp.API.Controller
{
    // [Authorize]
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

                var userFromRepo = await repository.GetUserWithUnapprovedPhotos(userId);

            // var values = await repository.GetUser(userId);

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

            photo.IsApproved = false;
            photo.IsMain = false;

            // if(!userFromRepo.Photos.Any(u => u.IsMain))
                // photo.IsMain = true;

             userFromRepo.Photos.Add(photo);

            if(await repository.SaveAll())
            {
                var photoForReturn = mapper.Map<PhotosForReturnDTO>(photo);
                return CreatedAtRoute("GetPhoto", new {userId = userId, id = photo.Id}, photoForReturn );
                // return CreatedAtRoute("GetPhoto", new {userId = userId, photo.Id}, photoForReturn );
            }
    
            return BadRequest("Could not add the photo");
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> ApprovePhoto(int userId, int id)
        {
             
            var userFromRepo = await repository.GetUserWithUnapprovedPhotos(userId);
            if(userFromRepo == null)
                return BadRequest("User not found");

            var moderator = await repository.GetUserWithUnapprovedPhotos(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var userHasRole = false;

            foreach (var role in moderator.UserRoles)
            {
                if(role.RoleId == 2 || role.RoleId == 3){
                    userHasRole = true;
                    break;
                }
            }

            if (!userHasRole)
                return BadRequest("Action not Allowed");

            if(!userFromRepo.Photos.Any(p => p.Id == id))
            return Unauthorized();

             var photoFromRepo = await repository.GetPhoto(id);

            if(!userFromRepo.Photos.Any(u => u.IsMain))
                 photoFromRepo.IsMain = true;

            photoFromRepo.IsApproved = true;

               if (await repository.SaveAll())
                return NoContent();

              return BadRequest("Could not set photo to main");
        }


        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
              if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                 return Unauthorized();

              var userFromRepo = await repository.GetUserWithUnapprovedPhotos(userId);
            
            if(!userFromRepo.Photos.Any(p => p.Id == id))
                return Unauthorized();

             var photoFromRepo = await repository.GetPhoto(id);

            if(photoFromRepo.IsMain)
                return BadRequest("This is already the main photo");

            if (!photoFromRepo.IsApproved)
                return BadRequest("This photo is awaiting for approval");

            var currentMainPhoto = await repository.GetMainPhotoForUser(userFromRepo.Id);

            currentMainPhoto.IsMain = false;
            photoFromRepo.IsMain =  true;

            if (await repository.SaveAll())
                return NoContent();

            return BadRequest("Could not set photo to main");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
            var moderator = await repository.GetUserWithUnapprovedPhotos(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            var userIsModerator = false;

            foreach (var role in moderator.UserRoles)
            {
                if(role.RoleId == 2 || role.RoleId == 3){
                    userIsModerator = true;
                    break;
                }
            }
    
            if(!userIsModerator){
             if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                 return Unauthorized();
            }
                var userFromRepo = await repository.GetUserWithUnapprovedPhotos(userId);
            
            if(!userFromRepo.Photos.Any(p => p.Id == id))
                return Unauthorized();

                var photoFromRepo = await repository.GetPhoto(id);

            if(photoFromRepo.IsMain)
                return BadRequest("You cannot delete your main photo!");


            //checking wether the photo is in cloudinary or was mocked for test purposes
            if(photoFromRepo.PublicId != null){
         
            var deleteParams = new DeletionParams(photoFromRepo.PublicId);

            var result = cloudinary.Destroy(deleteParams);

            if(result.Result.Equals("ok")){
                repository.Delete(photoFromRepo);
            }
            }
            else{
               repository.Delete(photoFromRepo);
            }
          
            if(await repository.SaveAll())
            return Ok();

            return BadRequest("Failed to delete the photo");
        }

    }
}