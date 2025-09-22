using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuppGamesBack.Models
{
    public class FavoriteGame
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        public int GameId { get; set; }
        public virtual Game Game { get; set; }

        public bool Excluded { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }

        public virtual ICollection<Anot> Annotations { get; set; } = new List<Anot>();
    }
}