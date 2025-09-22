using Microsoft.AspNetCore.Mvc;
using SuppGamesBack.Services;

namespace SuppGamesBack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RawgController : ControllerBase
    {
        private readonly IRawgClient _rawgClient;

        public RawgController(IRawgClient rawgClient)
        {
            _rawgClient = rawgClient;
        }

        [HttpGet("search")]

        public async Task<IActionResult> SearchGames([FromQuery] string query)
        {
            var games = await _rawgClient.SearchGamesAsync(query);
            return Ok(games);
        }

        [HttpGet("random-game")]

        public async Task<IActionResult> GetRandomGame()
        {
            var game = await _rawgClient.GetRandomGameAsync();
            
            if(game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }
    }
}
