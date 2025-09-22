using SuppGamesBack.RawgApi.Models;
using System.Text.Json;

namespace SuppGamesBack.Services
{
    public class RawgClient : IRawgClient
    {

        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public RawgClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["RawgApi:ApiKey"];
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
            var random = new Random();

            int randomPage = random.Next(1, 101);

            var response = await _httpClient.GetAsync($"https://api.rawg.io/api/games?key={_apiKey}&page={randomPage}&page_size=40&ordering=-rating");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var searchResults = JsonSerializer.Deserialize<RawgSearchResponse>(jsonString);

            if (searchResults?.results == null || !searchResults.results.Any())
            {
                return null;
            }

            int randomIndex = random.Next(0, searchResults.results.Count);
            return searchResults.results[randomIndex];
        }

    }
}
