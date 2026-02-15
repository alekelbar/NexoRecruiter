using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using NexoRecruiter.Application.Services.Auth;
using NexoRecruiter.Application.Services.Session;
using NexoRecruiter.Domain.Helpers;

namespace NexoRecruiter.Web.Features.Auth.Pages.ConfirmEmailPage
{
    public partial class ConfirmEmailPage : ComponentBase
    {
        [Inject]
        private ISnackbar SnackbarService { get; set; } = default!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        private ISessionService AuthService { get; set; } = default!;

        [Inject]
        private IUserService UserManager { get; set; } = default!;

        [SupplyParameterFromQuery]
        public string UserId { get; set; } = default!;

        [SupplyParameterFromQuery]
        public string Token { get; set; } = default!;
        public bool isLoading = true;
        public bool isInvalidToken = false;
        public bool isEmailConfirmed = false;

        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Token))
            {
                (isInvalidToken, isEmailConfirmed, isLoading) = (true, false, false);
                return;
            }

            var result = await UserManager.ConfirmEmailAsync(UserId, Token);
            if (result)
            {
                (isInvalidToken, isEmailConfirmed, isLoading) = (false, true, false);
                SnackbarService.Add("Email confirmed successfully!", Severity.Success);
            }
            else
            {
                (isInvalidToken, isEmailConfirmed, isLoading) = (true, false, false);
            }
        }
        public async Task HandleNavigation()
        {
            var authState = await AuthService.GetAuthenticationStateAsync();
            if (authState.IsAuthenticated)
            {
                NavigationManager.NavigateTo(AppRoutes.Dashboard);
                return;
            }
            else
            {
                NavigationManager.NavigateTo(AppRoutes.Login);
                return;
            }
        }
    }
}