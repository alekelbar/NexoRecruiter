using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using NexoRecruiter.Domain.Services.Auth.ValueObjects;

namespace NexoRecruiter.Domain.Services.Auth
{
    public interface INexoAuthStateProvider
    {
        /// <summary>
        /// An event that is triggered whenever the authentication state changes (e.g., user logs in or out). Subscribers can listen to this event to react to authentication changes in real-time.
        /// </summary>
        event EventHandler<AuthenticationChangedEventArgs> AuthenticationStateChanged;
        /// <summary>
        /// Gets the current authenticated user. If no user is authenticated, returns null.
        /// </summary>
        /// <returns></returns>
        public Task<NexoUser?> GetCurrentUserAsync();

        /// <summary>
        /// Gets the current authentication state, including the authenticated user and their roles. If no user is authenticated, returns an authentication state with a null user.
        /// </summary>
        /// <returns></returns>
        public Task<NexoAuthenticationState> GetAuthenticationStateAsync();
    }

    public class AuthenticationChangedEventArgs : EventArgs
    {
        public ClaimsPrincipal? User { get; set; }
        public DateTime ChangedAt { get; set; }
    }

    public class NexoAuthenticationState
    {
        public NexoUser? User { get; set; }
        public bool IsAuthenticated => User != null;
    }
}