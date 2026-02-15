using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using NexoRecruiter.Application.DTOs.Account;
using NexoRecruiter.Application.DTOs.Auth;
using NexoRecruiter.Domain.Helpers;
using NexoRecruiter.Web.Features.Dashboard.Layouts.DashboardLayout;
using NexoRecruiter.Web.Features.Dashboard.Layouts.DashboardLayout.Helpers;

namespace NexoRecruiter.Web.Features.Account.Pages.AccountPage
{
    public partial class AccountPage : ComponentBase
    {
        [CascadingParameter(Name = "CurrentUser")]
        public AppUserDTO? CurrentUser { get; set; }
        [CascadingParameter(Name = "DashboardLayout")]
        public DashboardLayout? DashboardLayout { get; set; }
        private UserAccountDTO UserAccountDTO { get; set; } = new UserAccountDTO();

        private bool isNotEditable = true;
        private static readonly System.Globalization.CultureInfo SpanishCulture =
            System.Globalization.CultureInfo.GetCultureInfo("es-ES");

        private string CreatedAtText =>
            UserAccountDTO.CreatedAt?.ToString("dd 'de' MMMM 'de' yyyy", SpanishCulture) ?? "";

        private string LastLoginText => GetRelativeTime(UserAccountDTO.LastLoginAt ?? DateTime.Now);

        private string AvatarInitial
        {
            get
            {
                var name = CurrentUser?.FullName ?? CurrentUser?.Email ?? "";
                return string.IsNullOrWhiteSpace(name) ? "" : name[..1];
            }
        }

        public void ToggleEdit()
        {
            isNotEditable = !isNotEditable;
        }

        public void GoToChangePassword()
        {
            navigationManager.NavigateTo(AppRoutes.ChangePassword);
        }


        public async Task SaveChanges()
        {

            var result = await _nexoUserManager.UpdateUserAsync(new UpdateAppUserDTO
            {
                FullName = UserAccountDTO.FullName,
                JobTitle = UserAccountDTO.JobTitle,
            });

            if (result)
            {
                Snackbar.Add("Tus cambios fueron guardados correctamente", Severity.Success, config =>
                {
                    config.VisibleStateDuration = 3000;
                    config.ShowCloseIcon = true;
                    config.Icon = Icons.Material.Filled.CheckCircle;
                });
            }
            else
            {
                Snackbar.Add("Ocurrió un error al guardar tus cambios", Severity.Error, config =>
                {
                    config.VisibleStateDuration = 5000;
                    config.ShowCloseIcon = true;
                    config.Icon = Icons.Material.Filled.Error;
                });
            }
            isNotEditable = true;
        }


        protected override Task OnInitializedAsync()
        {
            if (CurrentUser is not null)
            {
                UserAccountDTO = new UserAccountDTO
                {
                    CreatedAt = CurrentUser?.CreatedAt.Date ?? DateTime.Now.Date,
                    Email = CurrentUser?.Email ?? "",
                    FullName = CurrentUser?.FullName ?? "",
                    LastLoginAt = CurrentUser?.LastLoginAt?.Date ?? DateTime.Now.Date,
                    JobTitle = CurrentUser?.JobTitle ?? "",
                };
            }

            DashboardLayoutHelper.SetCurrentPageTitle(DashboardLayout, "Account", AppRoutes.Account);
            return base.OnInitializedAsync();
        }

        private string GetRelativeTime(DateTime date)
        {
            var timeSpan = DateTime.Now - date;

            if (timeSpan.TotalMinutes < 1) return "Justo ahora";
            if (timeSpan.TotalHours < 1) return $"Hace {(int)timeSpan.TotalMinutes} min";
            if (timeSpan.TotalDays < 1) return $"Hace {(int)timeSpan.TotalHours}h";
            if (timeSpan.TotalDays < 7) return $"Hace {(int)timeSpan.TotalDays} días";
            if (timeSpan.TotalDays < 30) return date.ToString("dd 'de' MMM", new System.Globalization.CultureInfo("es-ES"));

            return date.ToString("dd/MM/yyyy");
        }
    }
}