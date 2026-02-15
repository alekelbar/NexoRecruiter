using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NexoRecruiter.Domain.Repositories.Auth.ValueObjects;

namespace NexoRecruiter.Domain.Repositories.Auth
{
    /// <summary>
    /// Contrato de un servicio de dominio para gestionar usuarios
    /// Define SOLO contratos, sin DTOs
    /// </summary>
    public interface IUserRepository
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
        Task<User?> FindUserByEmailOrDefault(string email, CancellationToken ct = default);

        /// <summary>
        /// Decodifica un token de reset de contraseña
        /// </summary>
        string DecodedResetPasswordTokenAsync(string token, CancellationToken ct = default);

        /// <summary> 
        /// Valida si un token de reset de contraseña es válido (sin consumirlo)
        /// </summary>
        Task<bool> IsValidResetPasswordToken(PasswordResetToken token, CancellationToken ct = default);

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
        Task<bool> UpdateUserAsync(User user, CancellationToken ct = default);

        /// <summary>
        /// Genera un token de confirmación de email para el usuario actual
        /// </summary>
        /// <param name="email"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<EmailConfirmationToken> RequestEmailConfirmationTokenAsync(string email, CancellationToken ct = default);

        /// <summary>/ Confirma el email de un usuario dado un token
        /// </summary> <param name="userId"></param>
        /// <param name="token"></param> <param name="ct"></param>
        /// <returns></returns>
        Task<bool> ConfirmEmailAsync(string email, string token, CancellationToken ct = default);

        /// <summary>
        ///  Obtiene todos los usuarios de la aplicación (sin incluir contraseñas ni información sensible)
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        public Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken ct = default);
        /// <summary>
        /// Crea un nuevo usuario con roles asignados
        /// </summary>
        /// <param name="user"></param>
        /// <param name="roles"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task CreateUserAsync(User user, List<string> roles, CancellationToken ct = default);
        /// <summary>
        /// Elimina un usuario por su ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task DeleteUserAsync(string id, CancellationToken ct = default);
    }
}