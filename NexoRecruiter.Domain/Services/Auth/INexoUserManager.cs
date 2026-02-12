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

        /// <summary>
        /// Cambia la contraseña del usuario actual
        /// </summary>
        Task<bool> ChangePasswordAsync(
            string userEmail,
            string currentPassword,
            string newPassword,
            CancellationToken ct = default);
        /// <summary>
        /// Actualiza la información del usuario (sin incluir contraseña)
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<bool> UpdateUserAsync(NexoUser user, CancellationToken ct = default);

        /// <summary>
        /// Genera un token de confirmación de email para el usuario actual
        /// </summary>
        /// <param name="email"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<EmailConfirmationToken> RequestEmailConfirmationTokenAsync(CancellationToken ct = default);

        /// <summary>/ Confirma el email de un usuario dado un token
        /// </summary> <param name="userId"></param>
        /// <param name="token"></param> <param name="ct"></param>
        /// <returns></returns>
        Task<bool> ConfirmEmailAsync(string userId, string token, CancellationToken ct = default);
    }
}