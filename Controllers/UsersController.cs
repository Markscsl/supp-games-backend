using SuppGamesBack.Models.DTOs;
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

        public async Task<ActionResult<IEnumerable<UserReponseDTO>>> GetAllUsers()
        {
            var users = await _userRepository.GetAllAsync();

            var usersDto = users.Select(user => new UserReponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                IsAtivo = user.IsAtivo,
            }).ToList();

            return Ok(usersDto);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<UserReponseDTO>> GetUserById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            var userDto = new UserReponseDTO
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                IsAtivo = user.IsAtivo
            };

            return Ok(userDto);
        }

        [HttpPost]

        public async Task<ActionResult<UserReponseDTO>> AddUser([FromBody] CreateUserDTO userDto)
        {
            var newUser = new User
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Password = userDto.Password,
                IsAtivo = true
            };

            var createdUser = await _userRepository.AddAsync(newUser);

            var responseDto = new UserReponseDTO
            {
                Id = createdUser.Id,
                Name = createdUser.Name,
                Email = createdUser.Email,
                IsAtivo = createdUser.IsAtivo
            };

            return CreatedAtAction(nameof(GetUserById), new { id  = newUser.Id }, responseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDTO userDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);

            
            if (existingUser == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            existingUser.Name = userDto.Name;
            existingUser.Email = userDto.Email;

            
            await _userRepository.UpdateAsync(existingUser);

            return NoContent();
        }

        [HttpPut("deactivate/{id}")]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);

            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.IsAtivo = false;
            await _userRepository.UpdateAsync(existingUser);

            return NoContent();
        }
    }
}