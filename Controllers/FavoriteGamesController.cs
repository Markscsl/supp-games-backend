using Microsoft.AspNetCore.Mvc;
using SuppGamesBack.Data;
using SuppGamesBack.Models;
using SuppGamesBack.Models.DTOs;
using SuppGamesBack.Services;

namespace SuppGamesBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoriteGamesController : ControllerBase
    {
        private readonly IFavoriteGameRepository _favoriteGameRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRawgClient _rawgClient;

        public FavoriteGamesController(
            IFavoriteGameRepository favoriteGameRepository,
            IUserRepository userRepository,
            IRawgClient rawgClient)
        {
            _favoriteGameRepository = favoriteGameRepository;
            _userRepository = userRepository;
            _rawgClient = rawgClient;
        }

        [HttpPost]
        public async Task<IActionResult> AddFavoriteGame([FromBody] CreateFavoriteGameDTO favoriteDto)
        {
            var user = await _userRepository.GetByIdAsync(favoriteDto.UserId);
            if (user == null || !user.IsAtivo)
            {
                return NotFound("Usuário não encontrado ou inativo.");
            }

            if (await _favoriteGameRepository.ExistsAsync(favoriteDto.UserId, favoriteDto.Slug))
            {
                return Conflict("Este jogo já está na sua lista de favoritos."); 
            }

            var rawgGame = await _rawgClient.GetGameBySlugAsync(favoriteDto.Slug);
            if (rawgGame == null)
            {
                return NotFound("Jogo não encontrado na base de dados externa.");
            }

            var newGame = new FavoriteGame
            {
                Name = rawgGame.name,
                Slug = rawgGame.slug,
                UserId = favoriteDto.UserId,
                IsFavorite = true,
                CreateDate = DateTime.UtcNow,
                LastUpdate = DateTime.UtcNow,
                Excluded = false,
                Description = rawgGame.Description ?? "Descrição não disponível.",
                ImageUrl = rawgGame.ImageUrl ?? "",
                ReleaseDate = DateTime.TryParse(rawgGame.ReleaseDate, out var releaseDate) ? releaseDate : DateTime.MinValue,
                Platform = "Plataformas...", 
                Gender = "Gêneros..." 
            };

            var createdGame = await _favoriteGameRepository.AddAsync(newGame);

            var responseDto = new FavoriteGameResponseDTO
            {
                Id = createdGame.Id,
                UserId = createdGame.UserId,
                Name = createdGame.Name,
                Slug = createdGame.Slug,
                ImageUrl = createdGame.ImageUrl,
                Platform = createdGame.Platform,
                Gender = createdGame.Gender,
                Description = createdGame.Description,
                ReleaseDate = createdGame.ReleaseDate
            };


            return CreatedAtAction(nameof(GetFavoriteGameById), new { id = createdGame.Id }, responseDto);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetFavoriteGamesByUserId(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            var games = await _favoriteGameRepository.GetByUserIdAsync(userId);

            var responseDtos = games.Select(game => new FavoriteGameResponseDTO
            {
                Id = game.Id,
                UserId = game.UserId,
                Name = game.Name,
                Slug = game.Slug,
                ImageUrl = game.ImageUrl,
                Description = game.Description,
                ReleaseDate = game.ReleaseDate
            }).ToList();

            return Ok(responseDtos);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFavoriteGameById(int id)
        {
            var game = await _favoriteGameRepository.GetByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            var responseDto = new FavoriteGameResponseDTO
            {
                Id = game.Id,
                UserId = game.UserId,
                Name = game.Name,
                Slug = game.Slug,
                ImageUrl = game.ImageUrl,
                Description = game.Description,
                ReleaseDate = game.ReleaseDate
            };

            return Ok(responseDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFavoriteGame(int id)
        {
            var success = await _favoriteGameRepository.DeleteAsync(id);
            if (!success)
            {
                return NotFound("Registro de jogo favorito não encontrado.");
            }

            return NoContent();
        }
    }
}