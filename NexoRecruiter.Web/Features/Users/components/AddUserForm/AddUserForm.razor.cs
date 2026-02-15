using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using NexoRecruiter.Application.DTOs.Users;
using NexoRecruiter.Application.Services.Auth;
using NexoRecruiter.Web.Features.Users.components.ViewModels;

namespace NexoRecruiter.Web.Features.Users.components.AddUserForm
{
    public partial class AddUserForm : ComponentBase
    {
        [Inject]
        public IUserService UserService { get; set; } = null!;

        [CascadingParameter]
        private IMudDialogInstance? MudDialog { get; set; }
        private readonly AddUserPageViewModel newUser = new();
        private async void HandleValidSubmit()
        {
            if (!newUser.SelectedRoles.Any())
            {
                return;
            }
            await Submit();
        }

        public void OnSelectRole(IEnumerable<string> selectedRoles)
        {
            newUser.SelectedRoles = selectedRoles;
        }

        private async Task Submit()
        {
            try
            {
                Console.WriteLine($"Creating user: {newUser.FullName} with email: {newUser.Email} and roles: {string.Join(", ", newUser.SelectedRoles)}");
                var createUserDto = new CreateAppUserDTO
                {
                    Email = newUser.Email,
                    FullName = newUser.FullName,
                    JobTitle = newUser.JobTitle,
                    Roles = new List<string>(newUser.SelectedRoles)
                };
                await UserService.CreateUserAsync(createUserDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating user: {ex.Message}");
                return;
            }
            finally
            {
                MudDialog?.Close(DialogResult.Ok(true));
            }
        }

        private void Cancel() => MudDialog?.Cancel();
    }
}