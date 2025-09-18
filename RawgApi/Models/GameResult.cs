using System.Text.Json.Serialization;

namespace SuppGamesBack.RawgApi.Models
{
    public class GameResult
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? slug { get; set; }

        [JsonPropertyName("description_raw")]
        public string? Description { get; set; }

        [JsonPropertyName("background_image")]
        public string? ImageUrl { get; set; }

        [JsonPropertyName("released")]
        public string? ReleaseDate { get; set; }
    }
}
