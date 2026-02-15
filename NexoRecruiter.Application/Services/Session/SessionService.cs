using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using NexoRecruiter.Application.DTOs.Auth;
using NexoRecruiter.Domain.Helpers;
using NexoRecruiter.Domain.Repositories.Auth;
using NexoRecruiter.Domain.Repositories.Auth.ValueObjects;

namespace NexoRecruiter.Application.Services.Session
{
    public class SessionService : ISessionService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;

        public SessionService(IAuthRepository authRepository, IUserRepository userRepository)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _authRepository.AuthenticationStateChanged += (sender, args) =>
            {
                AuthenticationStateChanged?.Invoke(this, args);
            };
        }

        public event EventHandler<AuthenticationChangedEventArgs>? AuthenticationStateChanged;

        public async Task<AuthStateDTO> GetAuthenticationStateAsync()
        {
            var authState = await _authRepository.GetAuthenticationStateAsync();
            return new AuthStateDTO
            {
                User = authState.User != null ? MapToAppUserDTO(authState.User) : null
            };

        }

        public async Task<AppUserDTO?> GetCurrentUserAsync()
        {
            var user = await _authRepository.GetCurrentUserAsync();
            if (user == null) return null;

            return MapToAppUserDTO(user);
        }

        public async Task NotifyAuthenticationStateChangedAsync(ClaimsPrincipal? user = null)
        {
            var userDto = await _userRepository.FindUserByEmailOrDefault(user?.Identity?.Name ?? string.Empty);
            AuthenticationStateChanged?.Invoke(this, new AuthenticationChangedEventArgs() { User = userDto, ChangedAt = DateTime.UtcNow });
        }

        private static AppUserDTO MapToAppUserDTO(User user)
        {
            return new AppUserDTO
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                JobTitle = user.JobTitle,
                CreatedAt = user.CreatedAt,
                EmailConfirmed = user.EmailConfirmed,
                IsActive = user.IsActive,
                LastLoginAt = user.LastLoginAt,
                Roles = user.Roles
            };
        }

        private static User MapFromAppUserDTO(AppUserDTO user)
        {
            return new User
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                JobTitle = user.JobTitle,
                CreatedAt = user.CreatedAt,
                EmailConfirmed = user.EmailConfirmed,
                IsActive = user.IsActive,
                LastLoginAt = user.LastLoginAt,
                Roles = user.Roles
            };
        }
    }
}
