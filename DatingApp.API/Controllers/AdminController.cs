using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.DTOs;
using DatingApp.Domain.Interfaces;
using DatingApp.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly UserManager<User> _userManager;
        public AdminController(IDatingRepository repo, UserManager<User> userManager)
        {
            _userManager = userManager;
            _repo = repo;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("usersWithRoles")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var userList = await _repo.GetUsersWithRoles();

            return Ok(userList);
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("getPhotosForModeration")]
        public IActionResult GetPhotosForModeration()
        {
            return Ok("Admins or moderators can see this");
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("editRole/{userName}")]
        public async Task<IActionResult> EditRoles(string userName, RoleEditDTO roleEditDTO)
        {
            var user = await _userManager.FindByNameAsync(userName);

            var userRoles = await _userManager.GetRolesAsync(user);

            var selectedRoles = roleEditDTO.RoleNames;

            selectedRoles = selectedRoles ?? new string[] {};

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if(!result.Succeeded)
              return BadRequest("Failed to add Roles");
            
            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

            if(!result.Succeeded)
              return BadRequest("Failed to add Roles");

            return Ok(await _userManager.GetRolesAsync(user));
        }

    }
}