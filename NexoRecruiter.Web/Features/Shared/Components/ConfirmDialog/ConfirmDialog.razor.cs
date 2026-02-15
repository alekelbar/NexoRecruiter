using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace NexoRecruiter.Web.Features.Shared.Components.ConfirmDialog
{
    public partial class ConfirmDialog : ComponentBase
    {
        [Inject]
        public IDialogService DialogService { get; set; } = default!;

        [CascadingParameter]
        private IMudDialogInstance? MudDialog { get; set; }
        [Parameter]
        public string Title { get; set; } = "Are you sure?";

        private void Submit() => MudDialog?.Close(DialogResult.Ok(true));

        private void Cancel() => MudDialog?.Cancel();
    }
}