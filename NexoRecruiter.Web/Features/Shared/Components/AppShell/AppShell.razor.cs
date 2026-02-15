using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace NexoRecruiter.Web.Features.Shared.Components.AppShell
{
    public partial class AppShell : ComponentBase
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        // Theme mínimo para activar CSS Variables
        // Los colores reales se sobrescriben en _mudblazor-overrides.scss
        private readonly MudTheme _theme = new();
    }
}