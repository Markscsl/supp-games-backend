using SuppGamesBack.RawgApi.Models;
using System.Text.Json;

namespace SuppGamesBack.Services
{
    public class RawgClient : IRawgClient
    {

        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly List<string> _curatedGameSlugs = new List<string>
    {
        "the-witcher-3-wild-hunt", "portal-2", "the-legend-of-zelda-breath-of-the-wild",
        "red-dead-redemption-2", "mass-effect-2", "half-life-2", "grand-theft-auto-v",
        "bioshock", "the-last-of-us", "dark-souls", "super-mario-64", "elden-ring",
        "metal-gear-solid-3-snake-eater", "chrono-trigger", "hollow-knight", "celeste",
        "stardew-valley", "minecraft", "terraria", "the-elder-scrolls-v-skyrim",
        "fallout-new-vegas", "persona-5-royal", "bloodborne", "resident-evil-4",
        "shadow-of-the-colossus", "god-of-war-2018", "nier-automata", "undertale",
        "super-metroid", "castlevania-symphony-of-the-night", "street-fighter-ii",
        "tetris-effect-connected", "pac-man-championship-edition-dx", "hades-2",
        "super-mario-odyssey", "divinity-original-sin-2", "disco-elysium", "civilization-vi",
        "into-the-breach", "slay-the-spire", "outer-wilds", "return-of-the-obra-dinn",
        "what-remains-of-edith-finch", "kentucky-route-zero", "the-witness", "inside",
        "limbo", "journey", "flower", "fe", "gris", "ori-and-the-blind-forest",
        "cuphead", "dead-cells", "rogue-legacy-2", "hollow-knight-silksong",
        "kerbal-space-program", "factorio", "rimworld", "oxygen-not-included",
        "cities-skylines", "planet-coaster", "rollercoaster-tycoon-2", "age-of-empires-2-definitive-edition",
        "starcraft-2", "warcraft-3-reforged", "diablo-2-resurrected", "path-of-exile",
        "final-fantasy-xiv-online", "world-of-warcraft-classic", "guild-wars-2",
        "the-elder-scrolls-online", "runescape", "eve-online", "league-of-legends",
        "dota-2", "counter-strike-global-offensive", "valorant", "overwatch-2",
        "apex-legends", "fortnite", "pubg-battlegrounds", "call-of-duty-warzone",
        "rocket-league", "fall-guys", "among-us", "genshin-impact", "pokemon-unite",
        "brawlhalla", "smite", "paladins", "warframe", "destiny-2", "team-fortress-2"
    };

        public RawgClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["RawgApi:ApiKey"];
        }

        public static class DailyGameCache
        {
            public static GameResult? CachedGame { get; set; }
            public static DateTime CacheDate { get; set; }
        }

        public async Task<RawgSearchResponse> SearchGamesAsync(string query)
        {
            var response = await _httpClient.GetAsync($"https://api.rawg.io/api/games?key={_apiKey}&search={query}");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var searchResults = JsonSerializer.Deserialize<RawgSearchResponse>(jsonString);

            return searchResults;
        }

        public async Task<GameResult> GetGameBySlugAsync(string slug)
        {
            var response = await _httpClient.GetAsync($"https://api.rawg.io/api/games/{slug}?key={_apiKey}");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content?.ReadAsStringAsync();
            var gameResult = JsonSerializer.Deserialize<GameResult>(jsonString);

            return gameResult;
        }

        public async Task<GameResult?> GetRandomGameAsync()
        {
            if (DailyGameCache.CacheDate.Date == DateTime.Today && DailyGameCache.CachedGame != null)
            {
                return DailyGameCache.CachedGame;
            }

            var random = new Random(DateTime.Today.DayOfYear);

            var dailyShuffledSlugs = _curatedGameSlugs.OrderBy(s => random.Next()).ToList();

            foreach (var slug in dailyShuffledSlugs)
            {
                try
                {
                    var gameOfTheDay = await GetGameBySlugAsync(slug);

                    if (gameOfTheDay != null)
                    {
                        DailyGameCache.CachedGame = gameOfTheDay;
                        DailyGameCache.CacheDate = DateTime.Today;
                        return gameOfTheDay;
                    }
                }
                catch (HttpRequestException)
                {
                    Console.WriteLine($"Falha ao buscar o slug '{slug}'. Tentando o próximo...");
                }
            }

            return null;
        }

    }
}
