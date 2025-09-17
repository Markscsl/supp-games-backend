using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuppGamesBack.Models
{
    public class Annotation
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        [ForeignKey("FavoriteGame")]
        public int FavoriteGameId { get; set; }
        public virtual FavoriteGame? FavoriteGame { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User? User { get; set; }
        [Required]
        public DateTime CreateDateAt { get; set; }
        [Required]
        public DateTime LastUpdatedAt { get; set; }
        public bool Excluded { get; set; }
    }
}
