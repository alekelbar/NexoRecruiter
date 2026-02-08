using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexoRecruiter.Domain.Services.Auth.ValueObjects
{
    public class PasswordResetToken
    {
        public string Token { get; }
        public string Email { get; }
        public DateTime ExpiresAt { get; }
        public DateTime CreatedAt { get; }

        public PasswordResetToken(string token, string email, DateTime expiresAt)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token no puede estar vacío");
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email no puede estar vacío");
            if (expiresAt <= DateTime.UtcNow)
                throw new ArgumentException("ExpiresAt debe ser en el futuro");

            Token = token;
            Email = email;
            ExpiresAt = expiresAt;
            CreatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Lógica pura del dominio: validar si el token expiró
        /// </summary>
        public bool IsExpired() => DateTime.UtcNow > ExpiresAt;

        /// <summary>
        /// Lógica pura: validar si el token es válido
        /// </summary>
        public bool IsValid(string token, string email)
            => Token == token && Email == email && !IsExpired();
    }
}