using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexoRecruiter.Domain.Repositories.Auth.ValueObjects
{
    public class PasswordResetToken
    {
        public string Value { get; }
        public string Email { get; }

        public PasswordResetToken(string token, string email)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token no puede estar vacío");
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email no puede estar vacío");
            Value = token;
            Email = email;
        }
    }
}