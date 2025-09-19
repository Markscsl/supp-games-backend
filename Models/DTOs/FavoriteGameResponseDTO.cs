namespace SuppGamesBack.Models.DTOs
{
    public class FavoriteGameResponseDTO
    {
        public int Id { get; set; } 
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string ImageUrl { get; set; }
        public string Platform { get; set; }
        public string Gender { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}