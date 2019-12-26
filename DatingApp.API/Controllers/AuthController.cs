using System.Threading.Tasks;
using DatingApp.API.DTOs;
using DatingApp.Domain.Interfaces;
using DatingApp.Domain.Models;
using DatingApp.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
// using Microsoft.IdentityModel.Tokens;
// using System.IdentityModel.Tokens.Jwt;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    //allow the api to infer the types from body without adding the [FromBody] properties in the methods paramters
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        // private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        public SignInManager<User> _signInManager { get; set; }
        public UserManager<User> _userManager { get; set; }
        public AuthController(IConfiguration configuration,
        IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _config = configuration;
            // _repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDTO userForRegister)
        {
            // user.Username = user.Username.ToLower();

            // in case we don't use [ApiController] tags
            // if (!ModelState.IsValid)
            //     return BadRequest(ModelState);

            // if (await _repo.UserExists(user.Username))
            //     return BadRequest("Username already exists");

            // var userToCreate = new User
            // {
            //     Username = user.Username
            // };

            var userToCreate = _mapper.Map<User>(userForRegister);

            var result = await _userManager.CreateAsync(userToCreate, userForRegister.Password);
            var userToReturn = _mapper.Map<UserForDetailedDTO>(userToCreate);

            if (result.Succeeded){
               return CreatedAtRoute("GetUser", new { controller = "Users", id = userToCreate.Id }, userToReturn);
            }

            return BadRequest(result.Errors);
           
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDTO userForLogin)
        {
            var user = await _userManager.FindByNameAsync(userForLogin.Username);
            //  _repo.Login(user.Username, user.Password);

            if (user != null)
            {

                var result = await _signInManager.CheckPasswordSignInAsync(user, userForLogin.Password, false);

                if (result.Succeeded)
                {
                var appUser = _mapper.Map<UserForListDTO>(user);
                
                var roles = await _userManager.GetRolesAsync(user);

                return Ok(
                        new
                        {
                            token = AuthService.Generate(user, _config, roles),
                            userData = appUser
                        }
                    );
                }
            }
               
            return Unauthorized();
        }
    }
}