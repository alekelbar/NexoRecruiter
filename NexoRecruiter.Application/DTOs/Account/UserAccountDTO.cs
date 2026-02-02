
namespace NexoRecruiter.Application.DTOs.Account
{
    public class UserAccountDTO
    {
       public string FullName { get; set; } = default!;
       public string Nickname {get; set; } = default!;
       public DateTime? CreatedAt { get; set; }
       public string JobTitle { get; set; } = default!;
       public DateTime? LastLoginAt { get; set; } 
       public string Email { get; set; } = default!;
       public bool Status { get; set; } = default!;
    }
}