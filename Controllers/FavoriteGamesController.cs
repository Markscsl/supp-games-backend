using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuppGamesBack.Data;
using SuppGamesBack.Models;
using SuppGamesBack.Models.DTOs;
using SuppGamesBack.Services;
using System.Security.Claims;

namespace SuppGamesBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FavoriteGamesController : ControllerBase
    {
        private readonly IFavoriteGameRepository _favoriteGameRepo;
        private readonly IGameRepository _gameRepo; 
        private readonly IRawgClient _rawgClient;

        public FavoriteGamesController(
            IFavoriteGameRepository favoriteGameRepository,
            IGameRepository gameRepository,
            IRawgClient rawgClient)
        {
            _favoriteGameRepo = favoriteGameRepository;
            _gameRepo = gameRepository;
            _rawgClient = rawgClient;
        }


        [HttpPost]
        public async Task<IActionResult> FavoriteGame([FromBody] CreateFavoriteGameDTO favoriteDto)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();


            var game = await _gameRepo.GetBySlugAsync(favoriteDto.Slug);

            if (game == null)
            {
                var rawgGame = await _rawgClient.GetGameBySlugAsync(favoriteDto.Slug);
                if (rawgGame == null) return NotFound("Jogo não encontrado na base de dados externa.");

                var platformNames = rawgGame.platforms?.Select(p => p.platform.name).ToList() ?? new List<string?>();
                var genresNames = rawgGame.genre?.Select(g => g.name).ToList() ?? new List<string?>();
           
                game = new Game
                {
                    Name = rawgGame.name,
                    Slug = rawgGame.slug,
                    Description = rawgGame.Description ?? "N/A",
                    ImageUrl = rawgGame.ImageUrl ?? "",
                    ReleaseDate = DateTime.TryParse(rawgGame.ReleaseDate, out var date) ? date : DateTime.MinValue,
                    Platform = string.Join(", ", platformNames),
                    Genres = string.Join(", ", genresNames)
                };
                await _gameRepo.AddAsync(game);
            }


            if (await _favoriteGameRepo.ExistsAsync(userId.Value, game.Id))
            {
                return Conflict("Este jogo já está na sua lista de favoritos.");
            }

            
            var newFavorite = new FavoriteGame
            {
                UserId = userId.Value,
                GameId = game.Id,
                CreateDate = DateTime.UtcNow
            };
            await _favoriteGameRepo.AddAsync(newFavorite);

            return Ok(game); 
        }


        [HttpGet]
        public async Task<IActionResult> GetMyFavoriteGames()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var favoriteGames = await _favoriteGameRepo.GetByUserIdAsync(userId.Value);

  
            var response = favoriteGames.Select(fg => new GameResponseDTO
            {
                Id = fg.Game.Id,
                Name = fg.Game.Name,
                Slug = fg.Game.Slug,
                Description = fg.Game.Description,
                ImageUrl = fg.Game.ImageUrl,
                ReleaseDate = fg.Game.ReleaseDate,
                Platform = fg.Game.Platform,
                Genres = fg.Game.Genres
            });

            return Ok(response);
        }


        [HttpDelete("{gameId}")]
        public async Task<IActionResult> UnfavoriteGame(int gameId)
        {
            var userId = GetCurrentUserId();
            if (userId == null) return Unauthorized();


            var favoriteToRemove = await _favoriteGameRepo.FindAsync(userId.Value, gameId);
            if (favoriteToRemove == null)
            {
                return NotFound("Este jogo não está na sua lista de favoritos.");
            }


            await _favoriteGameRepo.DeleteAsync(favoriteToRemove.Id);

            return NoContent();
        }


        private int? GetCurrentUserId()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdString, out var userId))
            {
                return userId;
            }
            return null;
        }
    }
}