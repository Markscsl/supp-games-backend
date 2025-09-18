using System.ComponentModel.DataAnnotations;

namespace SuppGamesBack.Models.DTOs
{
    public class CreateUserDTO
    {
        [Required]
        public string Name { get; set; }

        [Required] 
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$&*])\S{8,20}$", ErrorMessage = "A senha deve conter pelo menos uma letra maiúscula, um número e um caractere especial (!@#$&*).")]
        public string Password { get; set; }

    }
}
