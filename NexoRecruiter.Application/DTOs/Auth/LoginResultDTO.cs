using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NexoRecruiter.Domain.Helpers;

namespace NexoRecruiter.Application.DTOs.Auth
{
    public class LoginResultDTO
    {
        public bool OK { get; set; }
        public string? Error { get; set; }
        public string ReturnUrl { get; set; } = AppRoutes.Login;
    }
}