using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using NexoRecruiter.Application.DTOs.Auth;
using NexoRecruiter.Application.Services.Session;
using NexoRecruiter.Domain.Repositories.Auth;

namespace NexoRecruiter.Web.Features.Shared.Components.CurrentUserProvider
{
    public partial class CurrentUserProvider : ComponentBase, IDisposable
    {
        [Inject]
        private ISessionService NexoAuthStateProvider { get; set; } = default!;
        public AppUserDTO? _currentUser { get; set; }
        private bool _isLoading = true;

        private readonly SemaphoreSlim _refreshLock = new(1, 1);

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        protected override async Task OnInitializedAsync()
        {
            NexoAuthStateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;
            await RefreshUserAsync();
            _isLoading = false;
        }

        private async void OnAuthenticationStateChanged(object? sender, AuthenticationChangedEventArgs e)
        {
            await RefreshUserAsync();
            await InvokeAsync(StateHasChanged);
        }

        private async Task RefreshUserAsync()
        {
            await _refreshLock.WaitAsync();
            try
            {
                var authState = await NexoAuthStateProvider.GetAuthenticationStateAsync();
                _currentUser = authState.User;
            }
            finally
            {
                _refreshLock.Release();
            }
        }

        public void Dispose()
        {
            NexoAuthStateProvider.AuthenticationStateChanged -= OnAuthenticationStateChanged;
            _refreshLock.Dispose();
        }
    }
}