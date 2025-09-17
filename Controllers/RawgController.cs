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
    }
}
