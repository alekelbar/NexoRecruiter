using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NexoRecruiter.Application.DTOs.Auth;
using NexoRecruiter.Application.DTOs.Users;

namespace NexoRecruiter.Application.Services.Auth
{
    public interface IUserService
    {
        /// <summary>
        /// Event that is triggered whenever a user is created, updated, or deleted.
        /// </summary>
        event EventHandler? UserChangedEvent;
        Task<IEnumerable<AppUserDTO>> GetAllUsersAsync(CancellationToken ct = default);
        Task<bool> UpdateUserAsync(UpdateAppUserDTO dto, CancellationToken ct = default);
        Task<AppUserDTO?> FindUserByEmailOrDefaultAsync(string email, CancellationToken ct = default);
        Task<bool> ChangePasswordAsync(ChangePasswordDTO dto, CancellationToken ct = default);
        Task RequestPasswordResetAsync(RequestPasswordResetDTO dto, CancellationToken ct = default);
        Task<bool> ValidateAndResetPasswordAsync(string email, string token, CancellationToken ct = default);
        Task<bool> RequestEmailConfirmationAsync(RequestEmailConfirmationDTO dto, CancellationToken ct = default);
        Task<bool> ConfirmEmailAsync(string email, string token, CancellationToken ct = default);
        string DecodeResetPasswordToken(string token);
        Task<bool> IsValidResetPasswordToken(string token, string email);
        Task CreateUserAsync(CreateAppUserDTO dto, CancellationToken ct = default);
        Task DeleteUserAsync(string id, CancellationToken ct = default);
    }
}