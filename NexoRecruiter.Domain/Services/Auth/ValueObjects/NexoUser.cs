using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexoRecruiter.Domain.Services.Auth.ValueObjects
{
    public class NexoUser
    {
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string NickName { get; set; } = default!;
        public string? JobTitle { get; set; }
        public bool IsActive { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
    }
}