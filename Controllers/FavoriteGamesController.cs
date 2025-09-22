using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuppGamesBack.Data;
using SuppGamesBack.Models;
using SuppGamesBack.Models.DTOs;
using SuppGamesBack.Services;
using System.Security;

namespace SuppGamesBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoriteGamesController : ControllerBase
    {
        private readonly IFavoriteGameRepository _favoriteGameRepository;
        private readonly IRawgClient _rawgClient;
        private int? GetCurrentUser()
        {
            var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userIdString, out var userId))
            {
                return userId;
            }

            return null;
        }

        public FavoriteGamesController(
            IFavoriteGameRepository favoriteGameRepository,
            IUserRepository userRepository,
            IRawgClient rawgClient)
        {
            _favoriteGameRepository = favoriteGameRepository;
            _rawgClient = rawgClient;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddFavoriteGame([FromBody] CreateFavoriteGameDTO favoriteDto)
        {

            var userIdFromToken = GetCurrentUser();
            if (userIdFromToken == null)
            {
                return Unauthorized();
            }

            if (await _favoriteGameRepository.ExistsAsync(userIdFromToken.Value, favoriteDto.Slug))
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
                UserId = userIdFromToken.Value,
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

        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetMyFavoriteGames()
        {
            var userIdFromToken = GetCurrentUser();

            if(userIdFromToken == null)
            {
                return Unauthorized();
            }

            var games = await _favoriteGameRepository.GetByUserIdAsync(userIdFromToken.Value);

            var responseDtos = games.Select(game => new FavoriteGameResponseDTO
            {
                Id = game.Id,
                UserId = userIdFromToken.Value,
                Name = game.Name,
                Slug = game.Slug,
                ImageUrl = game.ImageUrl,
                Description = game.Description,
                ReleaseDate = game.ReleaseDate
            }).ToList();

            return Ok(responseDtos);

        }

        [HttpGet("{id}")]
        [Authorize]

        public async Task<IActionResult> GetFavoriteGameById(int id)
        {
            var userIdFromToken = GetCurrentUser();

            var game = await _favoriteGameRepository.GetByIdAsync(id);

            if(userIdFromToken == null)
            {
                return Unauthorized();
            }

            if (game == null)
            {
                return NotFound();
            }

            if(game.UserId != userIdFromToken)
            {
                return Forbid();
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
        [Authorize]

        public async Task<IActionResult> DeleteFavoriteGame(int id)
        {
            var userIdFromToken = GetCurrentUser();
            var gameToDelete = await _favoriteGameRepository.GetByIdAsync(id);

            if(userIdFromToken == null)
            {
                return Unauthorized();
            }

            if(gameToDelete == null)
            {
                return NotFound("Registro de jogo não encontrado.");
            }

            if(gameToDelete.UserId != userIdFromToken)
            {
                return Forbid();
            }

            await _favoriteGameRepository.DeleteAsync(id);

            return NoContent();
        }
    }
}