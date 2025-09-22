namespace SuppGamesBack.Models.DTOs
{
    public class AnnotationResponseDTO
    {
        public int Id { get; set; }
        public int FavoriteGameId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDateAt { get; set; }
        public DateTime LastUpdateAt { get; set; }
    }
}
