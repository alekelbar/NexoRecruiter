using System.ComponentModel.DataAnnotations;

namespace NexoRecruiter.Application.DTOs.Auth
{
    public class PasswordResetRequestDTO
    {
        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "Ingresa un email válido")]
        public string Email { get; set; } = string.Empty;
    }
}