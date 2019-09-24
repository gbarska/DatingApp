using System;
using System.Text;
using System.Security.Claims;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using DatingApp.Domain.DTOs;
using DatingApp.Domain.Interfaces;
using DatingApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using DatingApp.Domain.Services;
// using Microsoft.IdentityModel.Tokens;
// using System.IdentityModel.Tokens.Jwt;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    //allow the api to infer the types from body without adding the [FromBody] properties in the methods paramters
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo, IConfiguration configuration)
        {
            this._config = configuration;
            this._repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDTO user)
        {
            user.Username = user.Username.ToLower();

            // in case we don't use [ApiController] tags
            // if (!ModelState.IsValid)
            //     return BadRequest(ModelState);

            if (await _repo.UserExists(user.Username))
                return BadRequest("Username already exists");

            var userToCreate = new User
            {
                Username = user.Username
            };

            var createdUser = await _repo.Register(userToCreate, user.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDTO user)
        {
            var userFromRepo = await _repo.Login(user.Username, user.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var tokenCreated = AuthService.Generate(userFromRepo, _config);

            return Ok(
                new {
                    token = AuthService.Write(tokenCreated)
                }
            );
    }
}
}