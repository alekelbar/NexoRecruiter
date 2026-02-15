using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexoRecruiter.Application.DTOs.Auth
{
    public class UpdateAppUserDTO
    {
        public string Id { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public bool EmailConfirmed { get; set; } = false;
        public string? JobTitle { get; set; }
    }
}