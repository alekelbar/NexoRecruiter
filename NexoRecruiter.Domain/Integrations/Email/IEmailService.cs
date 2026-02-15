using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace NexoRecruiter.Domain.Integrations.Email
{
    public interface IEmailIntegration
    {
        Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default);
    }
}