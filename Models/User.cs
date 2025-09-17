using System.ComponentModel.DataAnnotations;

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
        public bool IsAtivo { get; set; } = true;

        public virtual ICollection<FavoriteGame> FavoriteGames { get; set; } = new List<FavoriteGame>();
        public virtual ICollection<Annotation> Annotations { get; set; } = new List<Annotation>();
    }
}
