using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository datingRepository;
        private readonly IMapper mapper;

        public UsersController(IDatingRepository datingRepository, IMapper mapper)
        {
            this.datingRepository = datingRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await datingRepository.GetUsers();

            var usersToReturn = mapper.Map<IEnumerable<UserForListDto>>(users);

            return Ok(usersToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = datingRepository.GetUser(id);

            var UserToReturn = mapper.Map<UserForDetailDto>(user);

            return Ok(UserToReturn);
        }
    }
}