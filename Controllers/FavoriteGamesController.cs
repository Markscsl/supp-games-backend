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
        private readonly IImageService _imageService;
        private readonly IFavoriteGameRepository _favoriteGameRepo;
        private readonly IGameRepository _gameRepo; 
        private readonly IRawgClient _rawgClient;

        public FavoriteGamesController(
            IFavoriteGameRepository favoriteGameRepository,
            IGameRepository gameRepository,
            IRawgClient rawgClient,
            IImageService imageService)
        {
            _favoriteGameRepo = favoriteGameRepository;
            _gameRepo = gameRepository;
            _rawgClient = rawgClient;
            _imageService = imageService;
        }


        [HttpPost]
        public async Task<IActionResult> FavoriteGame([FromBody] CreateFavoriteGameDTO favoriteDto)
        {
            var userId = GetCurrentUserId();
            if (userId == null) 
            { 
                return Unauthorized(); 
            }


            var game = await _gameRepo.GetBySlugAsync(favoriteDto.Slug);

            if (game == null)
            {
                var rawgGame = await _rawgClient.GetGameBySlugAsync(favoriteDto.Slug);
                if (rawgGame == null) return NotFound("Jogo não encontrado na base de dados externa.");

                var platformNames = rawgGame.platforms?.Select(p => p.platform.name).ToList() ?? new List<string?>();
                var genresNames = rawgGame.genres?.Select(g => g.name).ToList() ?? new List<string?>();
           
                game = new Game
                {
                    Name = rawgGame.name,
                    Slug = rawgGame.slug,
                    Description = rawgGame.Description ?? "N/A",
                    ImageUrl = rawgGame.ImageUrl ?? "",
                    ReleaseDate = DateTime.TryParse(rawgGame.ReleaseDate, out var date) ? date : DateTime.MinValue,
                    Platform = string.Join(", ", platformNames),
                    Genres = string.Join(", ",  genresNames)
                };
                await _gameRepo.AddAsync(game);
            }

            var responseDto = new GameResponseDTO
            {
                Id = game.Id,
                Name = game.Name,
                Slug = game.Slug,
                Description = game.Description,
                ImageUrl = _imageService.TransformUrl(game.ImageUrl, 400, 300),
                ReleaseDate = game.ReleaseDate,
                Platform = game.Platform,
                Genres = game.Genres
            };


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

            return Ok(responseDto); 
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyFavoriteGames([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 12)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return Unauthorized();
            }

            var favoriteGames = await _favoriteGameRepo.GetByUserIdAsync(userId.Value, pageNumber, pageSize);


            var response = favoriteGames.Select(fav => new MyFavoriteGameDTO
            {
                Id = fav.Id, // O ID da relação
                Game = new GameResponseDTO
                {
                    Id = fav.Game.Id,
                    Name = fav.Game.Name,
                    Slug = fav.Game.Slug,
                    Description = fav.Game.Description,
                    ImageUrl = _imageService.TransformUrl(fav.Game.ImageUrl, 400, 300),
                    ReleaseDate = fav.Game.ReleaseDate,
                    Platform = fav.Game.Platform,
                    Genres = fav.Game.Genres
                },
                Annotations = fav.Annotations.Select(anot => new AnnotationResponseDTO
                {
                    Id = anot.Id,
                    FavoriteGameId = anot.FavoriteGameId,
                    Text = anot.Text,
                    CreatedDateAt = anot.CreateDateAt,
                    LastUpdateAt = anot.LastUpdatedAt
                }).ToList()
            }).ToList();

            return Ok(response);
        }


        [HttpDelete("{gameId}")]
        [Authorize]
        public async Task<IActionResult> UnfavoriteGame(int gameId)
        {
            var userId = GetCurrentUserId();
            if (userId == null) 
            { 
                return Unauthorized(); 
            } 


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