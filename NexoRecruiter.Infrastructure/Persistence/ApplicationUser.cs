using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace NexoRecruiter.Web.Features.Auth.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = default!;
        public string? JobTitle { get; set; }
        public bool IsActive { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }

        public void Apply(DateTime? lastLoginAt = null, string? fullName = null, string? jobTitle = null, bool? isActive = null)
        {
            if (fullName is not null) FullName = fullName;
            if (jobTitle is not null) JobTitle = jobTitle;
            if (isActive.HasValue) IsActive = isActive.Value;
            if (lastLoginAt.HasValue) LastLoginAt = lastLoginAt.Value;
        }
    }
}