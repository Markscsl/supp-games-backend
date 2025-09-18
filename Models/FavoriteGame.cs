using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SuppGamesBack.Models;


namespace SuppGamesBack.Models
{
    public class FavoriteGame
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Column("e_favorite")]
        public bool IsFavorite { get; set; }
        [Required]
        public string Slug { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public string Platform { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime ReleaseDate { get; set; }
        public bool Excluded { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public virtual User? User { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        [Required]
        public DateTime LastUpdate { get; set; }

        public virtual ICollection<Anot> Annotations { get; set; } = new List<Anot>();
    }
}
