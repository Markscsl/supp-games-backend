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
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]

        public async Task<ActionResult<User>> GetAllUsers()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(users);
        }

        [HttpPost]

        public async Task<ActionResult<User>> AddUser(User user)
        {
            user = await _userRepository.AddAsync(user);
            return CreatedAtAction(nameof(GetUserById), new {id = user.Id}, user);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound("Usuário não encontrado!");
            }

            return Ok(user);
        }

        [HttpPut("{id}")]

        public async Task<ActionResult<User>> UpdateUser(int id, User user)
        {

            if (id != user.Id)
            {
                return BadRequest("O ID fornecido não corresponde.");
            }

            var existingUser = await _userRepository.GetByIdAsync(id);

            if(user == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            await _userRepository.UpdateAsync(user);

            return NoContent();
        }

        [HttpPut("deactive/{id}")]

        public async Task<ActionResult<User>> DeactiveUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            if(user == null)
            {
                return NotFound();
            }

            user.IsAtivo = false;

            await _userRepository.UpdateAsync(user);

            return NoContent();
        }

    }
}
