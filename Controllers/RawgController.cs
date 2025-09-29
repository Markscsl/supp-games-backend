using Microsoft.AspNetCore.Mvc;
using SuppGamesBack.Models;
using SuppGamesBack.Services;

namespace SuppGamesBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RawgController : ControllerBase
    {
        private readonly IRawgClient _rawgClient;
        private readonly IImageService _imageService;

        public RawgController(IRawgClient rawgClient, IImageService imageService)
        {
            _rawgClient = rawgClient;
            _imageService = imageService;
        }

        [HttpGet("search")]

        public async Task<IActionResult> SearchGames([FromQuery] string query)
        {
            var games = await _rawgClient.SearchGamesAsync(query);


            if (games?.results != null)
            {
                foreach (var game in games.results)
                {
                    if (!string.IsNullOrEmpty(game.ImageUrl))
                    {
                        game.ImageUrl = _imageService.TransformUrl(game.ImageUrl, 200, 150);
                    }
                }
            }

            return Ok(games);
        }

        [HttpGet("random-game")]

        public async Task<IActionResult> GetRandomGame()
        {
            var game = await _rawgClient.GetRandomGameAsync();

            if (game == null)
            {
                return NotFound();
            }
            
                game.ImageUrl = _imageService.TransformUrl(game.ImageUrl, 800, 600);  

            return Ok(game);
        }
    }
}
