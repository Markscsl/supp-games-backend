using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuppGamesBack.Models.DTOs
{
    public class UpdateAnnotationDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
