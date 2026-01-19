using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NexoRecruiter.Domain.Repositories;
using NexoRecruiter.Infrastructure.Persistence;

namespace NexoRecruiter.Infrastructure
{
    public class UnitOfWork(RecruiterDbContext context) : IUnitOfWork
    {
        private readonly RecruiterDbContext context = context;

        public Task SaveChangesAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}