using Microsoft.AspNetCore.Mvc;
using SuppGamesBack.Data;
using SuppGamesBack.Models;


namespace SuppGamesBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepositor)
        {
            _userRepository = userRepositor;
        }

        [HttpGet]

        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(users);
        }

        [HttpPost]

        public async Task<ActionResult<List<User>>> AddUser(User user)
        {
            user = await _userRepository.AddAsync(user);
            return Created(user);
        }

    }
}
