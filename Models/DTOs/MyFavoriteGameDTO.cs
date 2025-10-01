namespace SuppGamesBack.Models.DTOs
{
    public class MyFavoriteGameDTO
    {
        public int Id { get; set; }
        public GameResponseDTO Game { get; set; }
        public List<AnnotationResponseDTO> Annotations { get; set; }
    }
}
