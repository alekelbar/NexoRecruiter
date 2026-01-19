using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NexoRecruiter.Domain.repositories;
using NexoRecruiter.Domain.Repositories;
using NexoRecruiter.Infrastructure;
using NexoRecruiter.Infrastructure.Persistence;

namespace NexoRecruiter.Infrastructure
{
    public static class InfraestructureRegistration
    {
        public static void AddInfraestructure(this IServiceCollection services)    
        {
            services.AddScoped<RecruiterDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}