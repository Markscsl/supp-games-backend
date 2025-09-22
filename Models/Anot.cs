using SuppGamesBack.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Anot
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Text { get; set; }

    [Required]
    public int FavoriteGameId { get; set; }
    public virtual FavoriteGame FavoriteGame { get; set; }

    [Required]
    public DateTime CreateDateAt { get; set; }
    [Required]
    public DateTime LastUpdatedAt { get; set; }
    public bool Excluded { get; set; }
}