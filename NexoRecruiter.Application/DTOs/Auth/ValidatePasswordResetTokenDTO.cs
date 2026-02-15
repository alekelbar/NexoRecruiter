using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexoRecruiter.Application.DTOs.Auth
{
    public class ValidatePasswordResetTokenDTO
    {
        public string Email { get; set; } = default!;
        public string Token { get; set; } = default!;
    }
}