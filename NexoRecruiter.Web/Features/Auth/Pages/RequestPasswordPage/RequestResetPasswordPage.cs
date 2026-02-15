using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using NexoRecruiter.Application.DTOs.Auth;
using NexoRecruiter.Domain.Helpers;
using NexoRecruiter.Web.Features.Auth.ViewModels;

namespace NexoRecruiter.Web.Features.Auth.Pages.RequestPasswordPage
{
    public partial class RequestResetPasswordPage : ComponentBase
    {
        public RequestPasswordResetViewModel recoveryPasswordDTO { get; set; } = new RequestPasswordResetViewModel();
        public bool IsLoading { get; set; } = false;

        private async Task Submit()
        {
            try
            {
                IsLoading = true;
                await userManager.RequestPasswordResetAsync(new RequestPasswordResetDTO { Email = recoveryPasswordDTO.Email });
            }
            catch (Exception ex)
            {
                // TODO: implementar sistema de logging para registrar errores
                Console.WriteLine($"Error requesting password reset token: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
                Snackbar.Add("Te hemos enviado un correo con el enlace para restablecer tu contraseña.", Severity.Success);
                navigationManager.NavigateTo(AppRoutes.Login, replace: true);
            }
        }
    }
}