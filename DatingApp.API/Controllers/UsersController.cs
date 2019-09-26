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

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _repo.GetUser(id);
            var userToReturn = _mapper.Map<UserForDetailedDTO>(user);
            return Ok(userToReturn);
        }

        [AllowAnonymous]
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

        // [HttpPut("{id}")]
        // public IActionResult Update(int id)
        // {
        //     return Ok();
        // }


    }
}