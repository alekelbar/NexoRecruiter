using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using NexoRecruiter.Application.DTOs.Auth;
using NexoRecruiter.Domain.Helpers;
using NexoRecruiter.Web.Features.Account.ViewModels;
using NexoRecruiter.Web.Features.Dashboard.Layouts.DashboardLayout;
using NexoRecruiter.Web.Features.Dashboard.Layouts.DashboardLayout.Helpers;

namespace NexoRecruiter.Web.Features.Account.Pages.ChangePasswordPage
{
    public partial class ChangePasswordPage : ComponentBase
    {
        [CascadingParameter(Name = "CurrentUser")]
        public AppUserDTO? CurrentUser { get; set; }

        private readonly ChangePasswordViewModel resetForm = new();
        private bool isLoading = true;
        private bool isSubmitting = false;
        private bool showCurrentPassword = false;
        private bool showNewPassword = false;
        private bool showConfirmPassword = false;

        [CascadingParameter(Name = "DashboardLayout")]
        public DashboardLayout? DashboardLayout { get; set; }

        [Inject]
        private IJSRuntime JS { get; set; } = default!;

        private ElementReference logoutForm;

        private async Task HandleLogout()
        {
            var form = await JS.InvokeAsync<IJSObjectReference>("eval", "document.getElementById('logout-form')");
            await form.InvokeVoidAsync("submit");
        }

        protected override void OnInitialized()
        {
            DashboardLayoutHelper.SetCurrentPageTitle(DashboardLayout, "Change password", AppRoutes.ChangePassword);
            isLoading = false;
        }

        private void ToggleShowCurrentPassword()
        {
            showCurrentPassword = !showCurrentPassword;
        }

        private void ToggleShowNewPassword()
        {
            showNewPassword = !showNewPassword;
        }

        private void ToggleShowConfirmPassword()
        {
            showConfirmPassword = !showConfirmPassword;
        }

        private async Task SubmitResetPassword()
        {
            // 1. Validar que las contraseñas coinciden
            if (resetForm.NewPassword != resetForm.ConfirmPassword)
            {
                Snackbar.Add("Passwords do not match", Severity.Error);
                return;
            }

            // 2. Validar complejidad (opcional, pero recomendado)
            if (resetForm.NewPassword.Length < 6)
            {
                Snackbar.Add("Password must be at least 6 characters", Severity.Error);
                return;
            }

            isSubmitting = true;
            try
            {
                var result = await userManager.ChangePasswordAsync(new ChangePasswordDTO
                {
                    ConfirmPassword = resetForm.ConfirmPassword,
                    CurrentPassword = resetForm.CurrentPassword,
                    NewPassword = resetForm.NewPassword,
                    Email = CurrentUser?.Email ?? string.Empty
                });

                if (!result)
                {
                    Snackbar.Add($"Failed to change password: try again", Severity.Error);
                    return;
                }

                Snackbar.Add("Password changed successfully!", Severity.Success);
                await HandleLogout();
            }
            catch
            {
                // TODO: Log exception
                Snackbar.Add($"An error occurred:", Severity.Error);
            }
            finally
            {
                isSubmitting = false;
            }
        }
    }
}