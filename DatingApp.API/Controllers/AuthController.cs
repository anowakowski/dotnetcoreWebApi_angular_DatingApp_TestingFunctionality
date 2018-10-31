using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            this.authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register (UserForRegisterDto userForRegister)
        {
            userForRegister.Username = userForRegister.Username .ToLower();

            if (await authRepository.UserExists(userForRegister.Username))
            {
                return BadRequest("User name already exists");
            }

            var userToCreate = new User
            {
                Username = userForRegister.Username
            };
            
            var createdUser = await authRepository.Register(userToCreate, userForRegister.Password);

            return StatusCode(201);
        }
    }
}