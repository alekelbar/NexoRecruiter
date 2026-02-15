using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NexoRecruiter.Domain.Repositories.Auth;
using NexoRecruiter.Domain.Repositories;
using NexoRecruiter.Infrastructure;
using NexoRecruiter.Infrastructure.Persistence;
using NexoRecruiter.Infrastructure.Repositories.Auth;
using NexoRecruiter.Domain.Integrations.Email;
using NexoRecruiter.Infrastructure.Integrations.Email;

namespace NexoRecruiter.Infrastructure
{
    public static class InfraestructureRegistration
    {
        public static void AddInfraestructure(this IServiceCollection services)
        {
            services.AddScoped<RecruiterDbContext>();
            services.AddTransient<IEmailIntegration, SmtpEmailIntegration>();
        }

        public static void AddDomainRepositories(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
        }
    }
}