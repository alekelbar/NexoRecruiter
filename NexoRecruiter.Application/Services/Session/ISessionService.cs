using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NexoRecruiter.Application.DTOs.Account;
using NexoRecruiter.Application.DTOs.Auth;
using NexoRecruiter.Domain.Repositories.Auth;

namespace NexoRecruiter.Application.Services.Session
{
    public interface ISessionService
    {
       Task<AppUserDTO?> GetCurrentUserAsync();
       Task<AuthStateDTO> GetAuthenticationStateAsync();

       event EventHandler<AuthenticationChangedEventArgs>? AuthenticationStateChanged;
    }
}