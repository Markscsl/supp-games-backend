using SuppGamesBack.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using SuppGamesBack.Data;
using SuppGamesBack.Models;
using SuppGamesBack.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SuppGamesBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        private int? GetCurrentUser()
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userIdString, out var userId))
            {
                return userId;
            }

            return null;
        }
        public UsersController(IUserRepository userRepository, ITokenService tokenService)
        {
            _tokenService = tokenService;
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

        [HttpPost("register")]

        public async Task<ActionResult<UserReponseDTO>> AddUser([FromBody] CreateUserDTO userDto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(userDto.Email);

            if (existingUser != null)
            {
                return Conflict("Este email ja está em uso.");
            }

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
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDTO userDto)
        {
            var userIdFromToken = GetCurrentUser();

            if (userIdFromToken == null)
            {
                return Unauthorized();
            }

            if (id != userIdFromToken)
            {
                return Forbid("ACESSO NEGADO!");
            }

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
        [Authorize]

        public async Task<IActionResult> DeactivateUser(int id)
        {
            var userIdFromToken = GetCurrentUser();

            if (userIdFromToken == null)
            {
                return Unauthorized();
            }

            if (id != userIdFromToken)
            {
                return Forbid("ACESSO NEGADO!");
            }


            var existingUser = await _userRepository.GetByIdAsync(id);

            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.IsAtivo = false;
            await _userRepository.UpdateAsync(existingUser);

            return NoContent();
        }

        [HttpPost("change-password/")]
        [Authorize]

        public async Task<ActionResult> ChangePassword([FromBody] NewPasswordDTO newPassword)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var existingUser = await _userRepository.GetByIdAsync(int.Parse(userId));

            if (existingUser == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            bool isCurrentPasswordCorrect = BCrypt.Net.BCrypt.Verify(newPassword.CurrentPassword, existingUser.Password);

            if (!isCurrentPasswordCorrect)
            {
                return BadRequest("A senha atual está incorreta.");
            }

            existingUser.Password = BCrypt.Net.BCrypt.HashPassword(newPassword.NewPassword);

            await _userRepository.UpdateAsync(existingUser);

            return Ok("Senha alterada com sucesso.");
        }

        [HttpPost("login")]

        public async Task<IActionResult> UserLogin([FromBody] LoginUserDTO loginUser)
        {

            var existingUser = await _userRepository.GetByEmailAsync(loginUser.Email);

            if (existingUser == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            bool passVerify = BCrypt.Net.BCrypt.Verify(loginUser.Password, existingUser.Password);

            if (!passVerify)
            {
                return BadRequest("Senha ou email incorretos.");
            }

            var token = _tokenService.CreateToken(existingUser);

            return Ok(new { token = token });
        }

        [HttpGet("public-lists")]
        public async Task<ActionResult<List<PublicUserListDTO>>> GetPublicUserLists([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            
            var usersWithFavorites = await _userRepository.GetPagedUsersWithFavoritesAsync(pageNumber, pageSize);

            
            var response = usersWithFavorites.Select(user => new PublicUserListDTO
            {
                UserId = user.Id,
                UserName = user.Name,

                FavoriteGames = user.FavoriteGames.Select(favGame => new PublicGameInfoDTO
                {
                    Id = favGame.Game.Id,
                    Name = favGame.Game.Name,
                    ImageUrl = favGame.Game.ImageUrl
                }).ToList()
            }).ToList();

            return Ok(response);
        }
    }
}