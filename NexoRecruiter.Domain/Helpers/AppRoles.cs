using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexoRecruiter.Domain.Helpers
{
    public static class AppRoles
    {
        public const string Admin = "Admin";
        public const string Recruiter = "Recruiter";

        public static readonly string[] AllRoles = new[] { Admin, Recruiter };
    }
}