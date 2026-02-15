using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexoRecruiter.Domain.Repositories.Auth.ValueObjects
{
    public class EmailConfirmationToken
    {
        public string Token { get; set; } = default!;
        public string Email { get; set; } = default!;
        public DateTime Expiration { get; set; }

    }
}