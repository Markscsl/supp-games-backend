using System.ComponentModel.DataAnnotations;

namespace SuppGamesBack.Models.DTOs
{
    public class CreateFavoriteGameDTO
    {
        [Required]
        public string Slug { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
