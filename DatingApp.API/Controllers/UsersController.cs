using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.Domain.DTOs;
using DatingApp.Domain.Interfaces;
using DatingApp.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Authorize]
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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _repo.GetUser(id);
            var userToReturn = _mapper.Map<UserForDetailedDTO>(user);
            return Ok(userToReturn);
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
          var values = await _repo.GetUsers();
               
            var usersToReturn = _mapper.Map<IEnumerable<UserForListDTO>>(values);
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


    }
}