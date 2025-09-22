using System.ComponentModel.DataAnnotations;

namespace SuppGamesBack.Models.DTOs
{
    public class CreateAnnotationDTO
    {
        [Required(ErrorMessage = "Texto obrigatório")]
        public string Text { get; set; }

        [Required]
        public int FavoriteGameId { get; set; }
    }
}
