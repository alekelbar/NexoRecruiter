using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using NexoRecruiter.Web.Features.Dashboard.Helpers;
using NexoRecruiter.Web.Features.Shared.Layouts;

namespace NexoRecruiter.Web.Features.Dashboard.Layouts.DashboardLayout
{
    public partial class DashboardLayout : LayoutComponentBase, ILayoutHeadHost
    {
        private List<BreadcrumbItem> _breadcrumbItems = [
        new("Dashboard", "/dashboard"),
    ];

        public void SetCurrentPage(string currentPage, string path)
        {
            if (_breadcrumbItems.Count > 1) _breadcrumbItems.RemoveAt(1);
            _breadcrumbItems.Add(new BreadcrumbItem(text: currentPage, path));
            StateHasChanged();
        }

        private RenderFragment? PageHead;
        public DrawerController DrawerController = new();

        public void SetPageHead(RenderFragment? fragment)
        {
            PageHead = fragment;
            StateHasChanged();
        }

        public void ClearPageHead()
        {
            PageHead = null;
            StateHasChanged();
        }

        protected override void OnInitialized()
        {
            DrawerController.OnChange += () => InvokeAsync(StateHasChanged);
        }
    }
}