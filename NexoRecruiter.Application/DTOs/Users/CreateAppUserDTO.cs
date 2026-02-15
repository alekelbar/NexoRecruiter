namespace NexoRecruiter.Application.DTOs.Users
{
    public class CreateAppUserDTO
    {
        public string Email { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string JobTitle { get; set; } = default!;
        public string NickName { get; set; } = default!;
        public List<string> Roles { get; set; } = [];
    }
}