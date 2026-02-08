using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NexoRecruiter.Domain.Abstractions;
using NexoRecruiter.Domain.interfaces.Auth;
using NexoRecruiter.Domain.repositories;
using NexoRecruiter.Domain.Repositories;
using NexoRecruiter.Infrastructure;
using NexoRecruiter.Infrastructure.Persistence;
using NexoRecruiter.Infrastructure.Services.Auth;
using NexoRecruiter.Infrastructure.Services.Email;

namespace NexoRecruiter.Infrastructure
{
    public static class InfraestructureRegistration
    {
        public static void AddInfraestructure(this IServiceCollection services)    
        {
            services.AddScoped<RecruiterDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // servicio de envio de correos.
            services.AddTransient<IEmailService, EmailService>();

            // Servicio de usuario
            services.AddTransient<INexoUserManager, NexoUserManager>();
        }
    }
}