using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace SuppGamesBack.Models
{
    public class Game
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Slug { get; set; }
        public string Description { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public string Platform { get; set; }
        public string Genres { get; set; }
        public DateTime ReleaseDate { get; set; }

        public virtual ICollection<FavoriteGame> FavoriteGames { get; set; } = new List<FavoriteGame>();
    }
}