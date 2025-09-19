using System.ComponentModel.DataAnnotations;

namespace SuppGamesBack.Models.DTOs
{
    public class NewPasswordDTO
    {
        [Required(ErrorMessage = "Forneça a senha atual.")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Forneça a nova senha.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$&*])\S{8,20}$", ErrorMessage = "A senha deve conter pelo menos uma letra maiúscula, um número e um caractere especial (!@#$&*).")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirme a sua senha.")]
        [Compare("NewPassword", ErrorMessage = "As senhas não coincidem")]
        public string ConfirmNewPassword { get; set; }
    }
}
