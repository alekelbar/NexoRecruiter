using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using NexoRecruiter.Application.DTOs.Auth;
using NexoRecruiter.Domain.Helpers;
using NexoRecruiter.Web.Features.Dashboard.Layouts.DashboardLayout;
using NexoRecruiter.Web.Features.Dashboard.Layouts.DashboardLayout.Helpers;

namespace NexoRecruiter.Web.Features.Dashboard.Pages.AdminDashboard
{
    public partial class AdminDashboard : ComponentBase
    {
        [CascadingParameter(Name = "CurrentUser")]
        public AppUserDTO? CurrentUser { get; set; }

        [CascadingParameter(Name = "DashboardLayout")]
        public DashboardLayout? DashboardLayout { get; set; }

        protected override Task OnInitializedAsync()
        {
            DashboardLayoutHelper.SetCurrentPageTitle(DashboardLayout, "Dashboard", AppRoutes.Dashboard);
            return base.OnInitializedAsync();
        }
    }
}