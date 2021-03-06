using System.Xml.Linq;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.DTOs;
using DatingApp.Domain.Shared;
using DatingApp.Domain.Interfaces;
using DatingApp.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DatingApp.API.Helpers;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            this._mapper = mapper;
            this._repo = repo;

        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> Get(int id)
        {
            var user = new User();

            var requestUser= int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if(id == int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
              user = await _repo.GetUserWithUnapprovedPhotos(id);
            else
             user = await _repo.GetUser(id);

            var userToReturn = _mapper.Map<UserForDetailedDTO>(user);
            return Ok(userToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery]UserParams userParams)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var userFromRepo = await _repo.GetUser(currentUserId);

            userParams.UserId = currentUserId;

            if(string.IsNullOrEmpty(userParams.Gender)){
                userParams.Gender = userFromRepo.Gender == "male" ? "female" : "male";
            }

            var users = await _repo.GetUsers(userParams);
               
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDTO>>(users);

            Response.AddPagination(users.CurrentPage, users.PageSize, 
            users.TotalCount, users.TotalPages);

            return Ok(usersToReturn);
        }

        // [HttpPost]
        // public async Task<IActionResult> Create()
        // {
        //     var values = await _repo.Values.ToArrayAsync();
        //     return Ok(values);
        // }
        // [HttpDelete("{id}")]
        // public IActionResult Delete(int id)
        // {
        //     return Ok();
        // }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserForUpdateDTO user)
        {
            if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

                var userFromRepo = await _repo.GetUser(id);

                _mapper.Map(user, userFromRepo);

                if(await _repo.SaveAll())
                    return NoContent();

                throw new Exception($"Updating user {id} failed on save");
        }

        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int id, int recipientId)
        {
            if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var like = await _repo.GetLike(id,recipientId);

            if (like != null)
                return BadRequest("You already liked this user");

            if( await _repo.GetUser(recipientId) == null)
                return NotFound();
            
            like = new Like
            {
                LikerId = id,
                LikeeId = recipientId
            };

            _repo.Add<Like>(like);

            if (await _repo.SaveAll())
            {
               var userLike = await _repo.GetLike(recipientId,id);
               
               if(userLike != null)
                Response.AddLikers(true);
               else
                Response.AddLikers(false);
             return Ok("");
            }
               

            throw new Exception("Failed to like user");
        }

        [HttpGet("photosForApproval")]
        public async Task<IActionResult> GetUsersWithPhotosForApproval()
        {
            var moderator = await _repo.GetUserWithUnapprovedPhotos(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
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

            var usersWithPhotosForApproval = await _repo.GetUsersWithUnapprovedPhotos();
            

            foreach (var item in usersWithPhotosForApproval)
            {
                var photos = item.Photos.Where(y => y.IsApproved == false ).ToList();
                item.Photos = photos;
                
                if (photos.Count <= 0){
                   usersWithPhotosForApproval = usersWithPhotosForApproval.Where(x => x.Id != item.Id);
                }
            }

            var photosForReturn = _mapper.Map<IEnumerable<UserWhithPhotoForApprovalDTO>>(usersWithPhotosForApproval);
            
            // _mapper.Map(photosForApproval, photosForReturn);

            return Ok(photosForReturn);
        }



    }
}