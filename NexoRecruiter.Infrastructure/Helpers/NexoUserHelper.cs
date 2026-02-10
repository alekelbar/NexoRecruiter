using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NexoRecruiter.Domain.Services.Auth.ValueObjects;
using NexoRecruiter.Web.Features.Auth.Models;

namespace NexoRecruiter.Infrastructure.Helpers
{
    public class NexoUserHelper
    {
       public static ApplicationUser MapToApplicationUser(NexoUser nexoUser)
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
                NickName = nexoUser.NickName
            };
        }

        public static NexoUser MapFromApplicationUser(ApplicationUser appUser)
        {
            return new NexoUser
            {
                Id = appUser.Id,
                FullName = appUser.FullName,
                Email = appUser.Email,
                CreatedAt = appUser.CreatedAt,
                IsActive = appUser.IsActive,
                JobTitle = appUser.JobTitle,
                LastLoginAt = appUser.LastLoginAt,
                NickName = appUser.NickName
            };
        } 
    }
}