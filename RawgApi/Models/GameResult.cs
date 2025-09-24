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

        public List<PlatformContainer>? platforms { get; set; }
        public List<Genre>? genres { get; set; }
    }

    public class PlatformContainer
    {
        public Platform? platform { get; set; }
    }

    public class Platform
    {
        public int id { get; set; }
        public string? name { get; set; }
    }

    public class Genre
    {
        public int id { get; set; }
        public string? name { get; set; }
    }
}
