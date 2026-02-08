using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NexoRecruiter.Application.Abstractions;
using NexoRecruiter.Application.DTOs.Auth;
using NexoRecruiter.Domain.Abstractions;
using NexoRecruiter.Domain.interfaces.Auth;

namespace NexoRecruiter.Application.UseCases.Auth
{
    public class RequestResetPasswordUseCase : IUseCase<PasswordResetRequestDTO, bool>
    {
        private readonly IEmailService emailService;
        private readonly INexoUserManager userManager;

        public RequestResetPasswordUseCase(IEmailService emailService, INexoUserManager userManager)
        {
            this.emailService = emailService;
            this.userManager = userManager;
        }
        public async Task<bool> ExecuteAsync(PasswordResetRequestDTO request, CancellationToken ct = default)
        {
            var token = await userManager.RequestResetPasswordTokenAsync(request.Email, ct);
            await emailService.SendAsync(request.Email, "Password Reset Request", $"Your password reset token is: {token.Token}", ct);
            return true;
        }
    }
}