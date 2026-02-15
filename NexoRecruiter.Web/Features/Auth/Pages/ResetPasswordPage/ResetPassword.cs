using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using NexoRecruiter.Application.DTOs.Auth;
using NexoRecruiter.Application.Services.Auth;
using NexoRecruiter.Domain.Helpers;

namespace NexoRecruiter.Web.Features.Auth.Pages.ResetPasswordPage
{
    public partial class ResetPassword : ComponentBase
    {
        [Inject] public IUserService NexoUserManager { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;
        [Inject] public ISnackbar Snackbar { get; set; } = default!;

        [SupplyParameterFromQuery]
        public string? Email { get; set; }

        [SupplyParameterFromQuery]
        public string? Token { get; set; }

        private ChangePasswordDTO resetForm = new();
        private bool isLoading = true;
        private bool isTokenValid = false;
        private bool isSubmitting = false;

        protected override async Task OnInitializedAsync()
        {
            // 1. Validar que tenemos email y token
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Token))
            {
                isTokenValid = false;
                isLoading = false;
                return;
            }

            // 2. Decodificar el token, que esta en base64
            var decodedToken = NexoUserManager.DecodeResetPasswordToken(Token);

            try
            {
                // 3. Buscar usuario
                var user = await NexoUserManager.FindUserByEmailOrDefaultAsync(Email);
                if (user == null)
                {
                    isTokenValid = false;
                    isLoading = false;
                    return;
                }

                var isValidResetPassToken = await NexoUserManager.IsValidResetPasswordToken(decodedToken, user?.Email ?? string.Empty);

                isTokenValid = isValidResetPassToken;
                resetForm.Email = Email;
                Token = decodedToken;
            }
            catch
            {
                isTokenValid = false;
            }
            finally
            {
                isLoading = false;
            }
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
                var result = await NexoUserManager.ValidateAndResetPasswordAsync(resetForm.Email, Token ?? string.Empty);
                if (!result)
                {
                    Snackbar.Add($"Failed to reset password: try again", Severity.Error);
                    return;
                }

                Snackbar.Add("Password reset successfully! Please login with your new password.", Severity.Success);
                NavigationManager.NavigateTo(AppRoutes.Login, replace: true);
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