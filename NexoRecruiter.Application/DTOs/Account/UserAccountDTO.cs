
using System.ComponentModel.DataAnnotations;

namespace NexoRecruiter.Application.DTOs.Account
{
    public class UserAccountDTO
    {

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        public string FullName { get; set; } = default!;

        [Required(ErrorMessage = "El nickname es obligatorio.")]
        [RegularExpression(@"^\S*$", ErrorMessage = "El nickname no puede contener espacios.")]
        public string Nickname { get; set; } = default!;
        public DateTime? CreatedAt { get; set; }
        public string JobTitle { get; set; } = default!;
        public DateTime? LastLoginAt { get; set; }
        public string Email { get; set; } = default!;
        public bool Status { get; set; } = default!;
    }
}