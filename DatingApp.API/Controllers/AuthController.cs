using System.Threading.Tasks;
using DatingApp.API.DTOs;
using DatingApp.Domain.Interfaces;
using DatingApp.Domain.Models;
using DatingApp.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public AuthController(IAuthRepository repo, IConfiguration configuration, IMapper mapper)
        {
            this._mapper = mapper;
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

            // var userToCreate = new User
            // {
            //     Username = user.Username
            // };

            var userToCreate = _mapper.Map<User>(user);

            var createdUser = await _repo.Register(userToCreate, user.Password);
            var userToReturn = _mapper.Map<UserForDetailedDTO>(createdUser);
            return CreatedAtRoute("GetUser", new {controller = "Users", id = createdUser.Id},userToReturn );
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDTO user)
        {

            var userFromRepo = await _repo.Login(user.Username, user.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var tokenCreated = AuthService.Generate(userFromRepo, _config);

            var userData = _mapper.Map<UserForListDTO>(userFromRepo);

            return Ok(
                new
                {
                    token = AuthService.Write(tokenCreated),
                    userData
                }
            );
        }
    }
}