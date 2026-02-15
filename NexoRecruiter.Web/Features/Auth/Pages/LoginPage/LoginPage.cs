using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using NexoRecruiter.Domain.Helpers;
using Microsoft.JSInterop;
using NexoRecruiter.Application.Services.Session;
namespace NexoRecruiter.Web.Features.Auth.Pages.LoginPage
{
    public partial class LoginPage : ComponentBase
    {
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private ISessionService AuthenticationStateProvider { get; set; } = default!;
        [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

        private bool ShowPassword = false;
        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;

        private bool _emailError = false;
        private string _emailErrorText = string.Empty;
        private bool _passwordError = false;
        private string _passwordErrorText = string.Empty;

        [SupplyParameterFromQuery]
        [Parameter]
        public string ReturnUrl { get; set; } = AppRoutes.Dashboard;

        [SupplyParameterFromQuery]
        public string? Error { get; set; }

        private void ShowPasswordField()
        {
            ShowPassword = !ShowPassword;
        }

        private void ValidateBeforeSubmit()
        {
            // Limpiar errores previos
            _errorMessage = string.Empty;
            _emailError = false;
            _emailErrorText = string.Empty;
            _passwordError = false;
            _passwordErrorText = string.Empty;

            bool isValid = true;

            // Validar email
            if (string.IsNullOrWhiteSpace(_email))
            {
                _emailError = true;
                _emailErrorText = "El correo es requerido";
                isValid = false;
            }
            else if (!_email.Contains("@"))
            {
                _emailError = true;
                _emailErrorText = "El correo no es válido";
                isValid = false;
            }

            // Validar password
            if (string.IsNullOrWhiteSpace(_password))
            {
                _passwordError = true;
                _passwordErrorText = "La contraseña es requerida";
                isValid = false;
            }

            // Si todo válido, submit el form
            if (isValid)
            {
                SubmitForm();
            }
        }

        private async void SubmitForm()
        {
            // Submit nativo del formulario HTML
            await Task.CompletedTask;
            var form = await JSRuntime.InvokeAsync<IJSObjectReference>("eval", "document.getElementById('loginForm')");
            await form.InvokeVoidAsync("submit");
        }

        protected override void OnParametersSet()
        {
            switch (Error)
            {
                case "1":
                case "3":
                    _errorMessage = "Credenciales inválidas. Por favor, verifica tu email y contraseña.";
                    break;
                case "2":
                    _errorMessage = "Tu cuenta no ha sido confirmada. Por favor, verifica tu correo electrónico.";
                    break;
                case "4":
                    _errorMessage = "Tu cuenta ha sido bloqueada. Por favor, contacta a soporte.";
                    break;
                default:
                    _errorMessage = string.Empty;
                    break;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
                if (authState.IsAuthenticated)
                {
                    NavigationManager.NavigateTo(AppRoutes.Dashboard, replace: true);
                }
            }
        }
    }
}