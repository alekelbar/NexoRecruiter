using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexoRecruiter.Web.Features.Dashboard.Layouts.DashboardLayout.Helpers
{
    public static class DashboardLayoutHelper
    {
        public static void SetCurrentPageTitle(DashboardLayout? dashboardLayout, string title, string path)
        {
            dashboardLayout?.SetCurrentPage(title, path);
        }
    }
}