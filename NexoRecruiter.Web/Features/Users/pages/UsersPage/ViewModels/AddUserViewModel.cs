using System.ComponentModel.DataAnnotations;

namespace NexoRecruiter.Web.Features.Users.components.ViewModels
{
    public class AddUserPageViewModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Full Name is required")]
        public string FullName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Job Title is required")]
        public string JobTitle { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select at least one role")]
        public IEnumerable<string> SelectedRoles { get; set; } = new HashSet<string>();
    }
}