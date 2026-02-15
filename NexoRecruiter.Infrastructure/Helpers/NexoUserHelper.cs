using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NexoRecruiter.Domain.Repositories.Auth.ValueObjects;
using NexoRecruiter.Web.Features.Auth.Models;

namespace NexoRecruiter.Infrastructure.Helpers
{
    public class NexoUserHelper
    {
        public static ApplicationUser MapToApplicationUser(User nexoUser)
        {
            return new ApplicationUser
            {
                Id = nexoUser.Id,
                FullName = nexoUser.FullName,
                Email = nexoUser.Email,
                CreatedAt = nexoUser.CreatedAt,
                IsActive = nexoUser.IsActive,
                JobTitle = nexoUser.JobTitle,
                LastLoginAt = nexoUser.LastLoginAt,
                EmailConfirmed = nexoUser.EmailConfirmed
            };
        }

        public static User MapFromApplicationUser(ApplicationUser appUser, IList<string>? roles = null)
        {
            return new User
            {
                Id = appUser.Id,
                FullName = appUser.FullName,
                Email = appUser.Email ?? appUser.UserName ?? "",
                CreatedAt = appUser.CreatedAt,
                IsActive = appUser.IsActive,
                JobTitle = appUser.JobTitle,
                LastLoginAt = appUser.LastLoginAt,
                EmailConfirmed = appUser.EmailConfirmed,
                Roles = roles?.ToList() ?? []
            };
        }
    }
}