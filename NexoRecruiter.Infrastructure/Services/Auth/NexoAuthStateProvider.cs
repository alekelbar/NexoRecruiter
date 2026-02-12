using NexoRecruiter.Domain.Services.Auth;
using NexoRecruiter.Domain.Services.Auth.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NexoRecruiter.Web.Features.Auth.Models;
using System.Security.Claims;
using NexoRecruiter.Infrastructure.Helpers;

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

            return NexoUserHelper.MapFromApplicationUser(user, roles: await userManager.GetRolesAsync(user));
        }

        public void NotifyAuthenticationStateChanged(ClaimsPrincipal? user)
        {
            AuthenticationStateChanged?.Invoke(this, new AuthenticationChangedEventArgs
            {
                User = user,
                ChangedAt = DateTime.UtcNow
            });
        }

        public async Task<NexoAuthenticationState> GetAuthenticationStateAsync()
        {
            var claimsPrincipal = httpContextAccessor.HttpContext?.User;
            if (claimsPrincipal == null || !claimsPrincipal.Identity?.IsAuthenticated == true)
            {
                return new NexoAuthenticationState { User = null };
            }

            var user = await userManager.GetUserAsync(claimsPrincipal);

            return new NexoAuthenticationState { User = NexoUserHelper.MapFromApplicationUser(user!, await userManager.GetRolesAsync(user!)) };
        }
    }
}