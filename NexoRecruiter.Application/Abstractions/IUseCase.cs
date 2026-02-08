using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexoRecruiter.Application.Abstractions
{
    /// <summary>
    /// Interfaz base para todos los UseCases (opcional, para documentación)
    /// </summary>
    public interface IUseCase<in TRequest, TResponse>
    {
        Task<TResponse> ExecuteAsync(TRequest request, CancellationToken ct = default);
    }

    /// <summary>
    /// Para UseCases que no retornan valor
    /// </summary>
    public interface IUseCase<in TRequest>
    {
        Task ExecuteAsync(TRequest request, CancellationToken ct = default);
    }
}