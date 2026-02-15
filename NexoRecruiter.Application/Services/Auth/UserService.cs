using System.Data;
using NexoRecruiter.Application.DTOs.Auth;
using NexoRecruiter.Application.DTOs.Users;
using NexoRecruiter.Domain.Repositories.Auth;
using NexoRecruiter.Domain.Repositories.Auth.ValueObjects;

namespace NexoRecruiter.Application.Services.Auth
{
    public class UserService(IUserRepository userRepository) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        public event EventHandler? UserChangedEvent;

        public void OnUserChanged()
        {
            UserChangedEvent?.Invoke(this, EventArgs.Empty);
        }

        public Task<bool> ChangePasswordAsync(ChangePasswordDTO dto, CancellationToken ct = default)
        {
            return _userRepository.ChangePasswordAsync(dto.Email, dto.CurrentPassword, dto.NewPassword, ct);
        }

        public Task<bool> ConfirmEmailAsync(string email, string token, CancellationToken ct = default)
        {
            return _userRepository.ConfirmEmailAsync(email, token, ct);
        }

        public async Task CreateUserAsync(CreateAppUserDTO dto, CancellationToken ct = default)
        {
            await _userRepository.CreateUserAsync(new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                JobTitle = dto.JobTitle,
            }, dto.Roles, ct);
            OnUserChanged();
        }

        public string DecodeResetPasswordToken(string token)
        {
            return _userRepository.DecodedResetPasswordTokenAsync(token);
        }

        public async Task DeleteUserAsync(string id, CancellationToken ct = default)
        {
            await _userRepository.DeleteUserAsync(id, ct);
            OnUserChanged();
        }

        public async Task<AppUserDTO?> FindUserByEmailOrDefaultAsync(string email, CancellationToken ct = default)
        {
            var user = await _userRepository.FindUserByEmailOrDefault(email, ct);
            return user == null ? null : MapToAppUserDTOs(user);
        }

        public async Task<IEnumerable<AppUserDTO>> GetAllUsersAsync(CancellationToken ct = default)
        {
            var users = await _userRepository.GetAllUsersAsync(ct);
            return users.Select(MapToAppUserDTOs);
        }

        public Task<bool> IsValidResetPasswordToken(string token, string email)
        {
            var requestObj = new PasswordResetToken(token, email);
            return _userRepository.IsValidResetPasswordToken(requestObj);
        }

        public async Task<bool> RequestEmailConfirmationAsync(RequestEmailConfirmationDTO dto, CancellationToken ct = default)
        {
            await _userRepository.RequestEmailConfirmationTokenAsync(dto.Email, ct);
            return true;
        }

        public async Task RequestPasswordResetAsync(RequestPasswordResetDTO dto, CancellationToken ct = default)
        {
            await _userRepository.RequestResetPasswordTokenAsync(dto.Email, ct);
        }

        public async Task<bool> UpdateUserAsync(UpdateAppUserDTO dto, CancellationToken ct = default)
        {
            User requestObj = new()
            {
                Id = dto.Id,
                FullName = dto.FullName,
                Email = dto.Email,
                EmailConfirmed = dto.EmailConfirmed,
                JobTitle = dto.JobTitle,
            };
            await _userRepository.UpdateUserAsync(requestObj, ct);
            OnUserChanged();
            return true;
        }

        public async Task<bool> ValidateAndResetPasswordAsync(string email, string token, CancellationToken ct = default)
        {
            var requestObj = new PasswordResetToken(token, email); // El expiresAt real se maneja en el repositorio, esto es solo para validar la estructura del token
            return await _userRepository.IsValidResetPasswordToken(requestObj, ct);
        }

        private AppUserDTO MapToAppUserDTOs(User user)
        {
            return new AppUserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive,
                JobTitle = user.JobTitle,
                LastLoginAt = user.LastLoginAt,
                EmailConfirmed = user.EmailConfirmed
            };
        }
    }
}