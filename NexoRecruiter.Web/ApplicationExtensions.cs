using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NexoRecruiter.Application.Services.Auth;
using NexoRecruiter.Application.Services.Session;

namespace NexoRecruiter.Web
{
    public static class ApplicationExtensions
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IUserService, UserService>();
        }
    }
}