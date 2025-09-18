using System.ComponentModel.DataAnnotations;

namespace SuppGamesBack.Models.DTOs
{
    public class UpdateUserDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
