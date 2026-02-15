using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MudBlazor;

namespace NexoRecruiter.Web.Features.Shared.Components.ConfirmDialog
{
    public static class ConfirmDialogHelper
    {
        public static async Task<bool> PreConfirmActionAsync(IDialogService dialogService, string action)
        {
            var dialogParams = new DialogParameters<ConfirmDialog>()
            {
                ["Title"] = action
            };

            var dialog = await dialogService.ShowAsync<ConfirmDialog>("", dialogParams, new DialogOptions { MaxWidth = MaxWidth.ExtraSmall, NoHeader = true, Position = DialogPosition.Center });
            if (dialog is null) return false;

            var result = await dialog.Result;
            if (result is not null && result.Canceled) return false;

            return true;
        }
    }
}