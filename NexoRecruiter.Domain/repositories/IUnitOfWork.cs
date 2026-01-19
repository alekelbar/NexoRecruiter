using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexoRecruiter.Domain.Repositories
{
    public interface IUnitOfWork
    {
        public Task SaveChangesAsync();
    }
}