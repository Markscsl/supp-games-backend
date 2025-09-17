using SuppGamesBack.RawgApi.Models;
using System.Text.Json;

namespace SuppGamesBack.Services
{
    public class RawgClient : IRawgClient
    {

        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "5fd14da7e31042d883f77ab14624cdfd";

        public RawgClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<RawgSearchResponse> SearchGamesAsync(string query)
        {
            var response = await _httpClient.GetAsync($"https://api.rawg.io/api/games?key={_apiKey}&search={query}");
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var searchResults = JsonSerializer.Deserialize<RawgSearchResponse>(jsonString);

            return searchResults;
        }



    }
}
