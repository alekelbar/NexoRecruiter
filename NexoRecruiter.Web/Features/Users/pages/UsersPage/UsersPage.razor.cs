using Microsoft.AspNetCore.Components;
using MudBlazor;
using NexoRecruiter.Application.DTOs.Auth;
using NexoRecruiter.Application.Services.Auth;
using NexoRecruiter.Domain.Helpers;
using NexoRecruiter.Web.Features.Dashboard.Layouts.DashboardLayout;
using NexoRecruiter.Web.Features.Dashboard.Layouts.DashboardLayout.Helpers;
using NexoRecruiter.Web.Features.Shared.Components.ConfirmDialog;
using NexoRecruiter.Web.Features.Users.components.AddUserForm;

namespace NexoRecruiter.Web.Features.Users.pages.UsersPage
{
    public partial class UsersPage : ComponentBase
    {
        // DI
        [Inject]
        public IUserService UserService { get; set; } = null!;
        [Inject]
        public IDialogService DialogService { get; set; } = null!;

        [CascadingParameter(Name = "DashboardLayout")]
        public DashboardLayout? DrawerController { get; set; } = null;
        [CascadingParameter(Name = "CurrentUser")]
        public AppUserDTO? CurrentUser { get; set; } = null;
        public IEnumerable<AppUserDTO> Items { get; set; } = [];
        private bool IsTheSameUser(AppUserDTO user) => CurrentUser is not null && user.Id == CurrentUser.Id;

        public async Task RequestResetPassword(AppUserDTO user)
        {
            var confirm = await ConfirmDialogHelper.PreConfirmActionAsync(DialogService, $"Are you sure you want to request a password reset for user {user.FullName}?");
            if (!confirm) return;

            await UserService.RequestPasswordResetAsync(new RequestPasswordResetDTO { Email = user.Email });
        }

        public async Task RequestEmailVerification(AppUserDTO user)
        {
            var confirm = await ConfirmDialogHelper.PreConfirmActionAsync(DialogService, $"Are you sure you want to request an email verification for user {user.FullName}?");
            if (!confirm) return;

            await UserService.RequestEmailConfirmationAsync(new RequestEmailConfirmationDTO { Email = user.Email });
        }

        public async Task<bool> PreConfirmAction(string action)
        {
            var dialogParams = new DialogParameters<ConfirmDialog>()
            {
                ["Title"] = action
            };

            var dialog = await DialogService.ShowAsync<ConfirmDialog>("", dialogParams, new DialogOptions { MaxWidth = MaxWidth.ExtraSmall, NoHeader = true, Position = DialogPosition.Center });
            if (dialog is null) return false;

            var result = await dialog.Result;
            if (result is not null && result.Canceled) return false;

            return true;
        }

        private async Task OnDeleteUser(AppUserDTO user)
        {
            var confirm = await ConfirmDialogHelper.PreConfirmActionAsync(DialogService, $"Are you sure you want to delete user {user.FullName}?");
            if (!confirm) return;

            await UserService.DeleteUserAsync(user.Id);
        }

        private async Task OnCreateUser()
        {
            var options = new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true };
            await DialogService.ShowAsync<AddUserForm>("Add User", options);
        }

        protected override async Task OnInitializedAsync()
        {
            Items = await UserService.GetAllUsersAsync();
            DashboardLayoutHelper.SetCurrentPageTitle(DrawerController, "Users", AppRoutes.Users);

            UserService.UserChangedEvent += async (sender, args) =>
            {
                Items = await UserService.GetAllUsersAsync();
                StateHasChanged();
            };
        }

    }
}