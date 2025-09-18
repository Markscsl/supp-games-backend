using System.ComponentModel.DataAnnotations;
using SuppGamesBack.Models;

namespace SuppGamesBack.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public bool IsAtivo { get; set; }

        public virtual ICollection<FavoriteGame> FavoriteGames { get; set; } = new List<FavoriteGame>();
    }
}
