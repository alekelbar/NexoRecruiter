using NexoRecruiter.Domain.Services.Auth;
using NexoRecruiter.Domain.Services.Auth.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NexoRecruiter.Web.Features.Auth.Models;
using System.Security.Claims;

namespace NexoRecruiter.Infrastructure.Services.Auth
{
    public class NexoAuthStateProvider(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager) : INexoAuthStateProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager = userManager;

        public event EventHandler<AuthenticationChangedEventArgs>? AuthenticationStateChanged;

        public async Task<NexoUser?> GetCurrentUserAsync()
        {
            var claimsPrincipal = httpContextAccessor.HttpContext?.User;
            if (claimsPrincipal == null || !claimsPrincipal.Identity?.IsAuthenticated == true)
            {
                return null;
            }

            var user = await userManager.GetUserAsync(claimsPrincipal);
            if (user == null)
                return null;

            return MapToNexoUser(user, await userManager.GetRolesAsync(user));
        }

        public void NotifyAuthenticationStateChanged(ClaimsPrincipal? user)
        {
            AuthenticationStateChanged?.Invoke(this, new AuthenticationChangedEventArgs
            {
                User = user,
                ChangedAt = DateTime.UtcNow
            });
        }

        private static NexoUser MapToNexoUser(ApplicationUser user, IEnumerable<string> roles)
        {
            return new NexoUser
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive,
                JobTitle = user.JobTitle,
                LastLoginAt = user.LastLoginAt,
                NickName = user.NickName,
                Roles = [.. roles]
            };
        }

        public async Task<NexoAuthenticationState> GetAuthenticationStateAsync()
        {
            var claimsPrincipal = httpContextAccessor.HttpContext?.User;
            if (claimsPrincipal == null || !claimsPrincipal.Identity?.IsAuthenticated == true)
            {
                return new NexoAuthenticationState { User = null };
            }

            var user = await userManager.GetUserAsync(claimsPrincipal);

            return new NexoAuthenticationState { User = MapToNexoUser(user!, await userManager.GetRolesAsync(user!)) };
        }
    }
}