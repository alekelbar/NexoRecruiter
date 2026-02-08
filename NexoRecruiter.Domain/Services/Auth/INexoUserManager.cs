using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NexoRecruiter.Domain.Services.Auth.ValueObjects;

namespace NexoRecruiter.Domain.interfaces.Auth
{
    /// <summary>
    /// Contrato de un servicio de dominio para gestionar usuarios
    /// Define SOLO contratos, sin DTOs
    /// </summary>
    public interface INexoUserManager
    {
        /// <summary>
        /// Genera un token de reset de contraseña para un usuario
        /// Retorna un ValueObject (no DTO)
        /// </summary>
        Task<PasswordResetToken> RequestResetPasswordTokenAsync(
            string email,
            CancellationToken ct = default);

        /// <summary>
        /// Valida y consume un token de reset
        /// </summary>
        Task<bool> ValidateAndConsumeResetTokenAsync(
            string email,
            string token,
            string newPassword,
            CancellationToken ct = default);

        /// <summary>
        /// Obtiene un usuario por email
        /// </summary>
        Task<NexoUser?> FindUserByEmailOrDefault(string email, CancellationToken ct = default);

        /// <summary>
        /// Decodifica un token de reset de contraseña
        /// </summary>
        string DecodedResetPasswordTokenAsync(string token, CancellationToken ct = default);

        /// <summary> 
        /// Valida si un token de reset de contraseña es válido (sin consumirlo)
        /// </summary>
        Task<bool> IsValidResetPasswordToken(string token, string userEmail);
    }
}