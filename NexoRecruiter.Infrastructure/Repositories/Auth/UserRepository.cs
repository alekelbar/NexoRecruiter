using System.Net;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using NexoRecruiter.Domain.Integrations.Email;
using NexoRecruiter.Domain.Helpers;
using NexoRecruiter.Domain.Repositories.Auth;
using NexoRecruiter.Domain.Repositories.Auth.ValueObjects;
using NexoRecruiter.Infrastructure.Helpers;
using NexoRecruiter.Web.Features.Auth.Models;

namespace NexoRecruiter.Infrastructure.Repositories.Auth
{
    public class UserRepository(UserManager<ApplicationUser> userManager, IEmailIntegration emailService) : IUserRepository
    {
        private readonly UserManager<ApplicationUser> userManager = userManager;
        private readonly IEmailIntegration emailService = emailService;

        public async Task<bool> ChangePasswordAsync(string userEmail, string currentPassword, string newPassword, CancellationToken ct = default)
        {
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null)
                return false;

            var result = await userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string token, CancellationToken ct = default)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            var isAlreadyConfirmed = await userManager.IsEmailConfirmedAsync(user);
            if (isAlreadyConfirmed) return true;

            var tokenBytes = WebEncoders.Base64UrlDecode(token);
            var decodedToken = Encoding.UTF8.GetString(tokenBytes);

            var result = await userManager.ConfirmEmailAsync(user, decodedToken);
            if (result.Succeeded)
            {
                user.IsActive = true;
                await userManager.UpdateAsync(user);
            }
            return result.Succeeded;
        }

        public async Task CreateUserAsync(User user, List<string> roles, CancellationToken ct = default)
        {
            var result = await userManager.CreateAsync(new ApplicationUser
            {
                FullName = user.FullName,
                Email = user.Email,
                UserName = user.Email,
                JobTitle = user.JobTitle,
                IsActive = false,
                EmailConfirmed = false,
            }, "DefaultPassword123!");

            if (!result.Succeeded)
            {
                throw new InvalidOperationException("User was not created successfully");
            }

            var createdUser = await userManager.FindByEmailAsync(user.Email ?? string.Empty) ?? throw new InvalidOperationException("User was not created successfully");
            await userManager.AddToRolesAsync(createdUser, roles);
        }

        public string DecodedResetPasswordTokenAsync(string token, CancellationToken ct = default)
        {
            var decodedTokenBytes = WebEncoders.Base64UrlDecode(token);
            var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);
            return decodedToken;
        }

        public async Task DeleteUserAsync(string id, CancellationToken ct = default)
        {
            var user = await userManager.FindByIdAsync(id) ?? throw new InvalidOperationException("User not found");
            var result = await userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to delete user: {errors}");
            }
        }

        public async Task<User?> FindUserByEmailOrDefault(string email, CancellationToken ct = default)
        {
            var realUser = await userManager.FindByEmailAsync(email) ?? null;
            if (realUser != null)
            {
                User nexoUser = new()
                {
                    FullName = realUser.FullName,
                    Email = realUser.Email ?? string.Empty,
                    CreatedAt = realUser.CreatedAt,
                    IsActive = realUser.IsActive,
                    JobTitle = realUser.JobTitle,
                    LastLoginAt = realUser.LastLoginAt,
                };

                return nexoUser;
            }
            return null;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync(CancellationToken ct = default)
        {
            var users = await userManager.Users.ToListAsync(ct);
            var nexoUsers = users.Select(u => NexoUserHelper.MapFromApplicationUser(u)).ToList();
            return nexoUsers ?? [];
        }

        public async Task<bool> IsValidResetPasswordToken(PasswordResetToken token, CancellationToken ct = default)
        {
            var findUser = await userManager.FindByEmailAsync(token.Email);
            if (findUser == null) return false; // TODO: No se puede generar un token para un usuario inexistente, no puede ser válido

            var result = await userManager.VerifyUserTokenAsync(
                findUser,
                userManager.Options.Tokens.PasswordResetTokenProvider,
                "ResetPassword",
                token.Value
            );

            return result;
        }

        public async Task<EmailConfirmationToken> RequestEmailConfirmationTokenAsync(string email, CancellationToken ct = default)
        {
            var user = await userManager.FindByEmailAsync(email) ?? throw new InvalidOperationException("User not found in database");
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            var tokenBytes = Encoding.UTF8.GetBytes(token);
            var tokenEncoded = WebEncoders.Base64UrlEncode(tokenBytes);
            var escapedUserId = Uri.EscapeDataString(user.Id);

            var confirmationLink = AppRoutes.ConfirmEmail(escapedUserId, tokenEncoded);
            var emailBody = $@"
                <h2>Email Confirmation Request</h2>
                <p>Click the link below to confirm your email. This link expires in 24 hours.</p>
                <a href='{confirmationLink}' style='padding: 10px 20px; background-color: #1976d2; color: white; text-decoration: none; border-radius: 4px;'>
                    Confirm Email
                </a>
                <p>If you didn't request this, ignore this email.</p>
            ";

            await emailService.SendAsync(
                to: user.Email ?? "",
                subject: "Email Confirmation Request - NexoRecruiter",
                htmlBody: emailBody,
                ct: ct
            );

            return new EmailConfirmationToken()
            {
                Token = token,
                Email = user.Email ?? string.Empty,
                Expiration = DateTime.UtcNow.AddHours(24) // TODO: Esto debería ser configurable, no hardcodeado
            };
        }

        public async Task<PasswordResetToken> RequestResetPasswordTokenAsync(string email, CancellationToken ct = default)
        {
            var user = await userManager.FindByEmailAsync(email) ??
                throw new InvalidOperationException("User not found");

            var tokenResult = await userManager.GeneratePasswordResetTokenAsync(user);

            var tokenBytes = Encoding.UTF8.GetBytes(tokenResult);
            var tokenEncoded = WebEncoders.Base64UrlEncode(tokenBytes);
            var escapedEmail = Uri.EscapeDataString(email);

            var resetLink = AppRoutes.ResetPassword(escapedEmail, tokenEncoded);
            // TODO: En producción, esto debería ser desde configuración, no hardcodeado

            var emailBody = $@"
                <h2>Password Reset Request</h2>
                <p>Click the link below to reset your password. This link expires in 24 hours.</p>
                <a href='{resetLink}' style='padding: 10px 20px; background-color: #1976d2; color: white; text-decoration: none; border-radius: 4px;'>
                    Reset Password
                </a>
                <p>If you didn't request this, ignore this email.</p>
            ";

            await emailService.SendAsync(
                to: user.Email ?? "",
                subject: "Password Reset Request - NexoRecruiter",
                htmlBody: emailBody,
                ct: ct
            );

            // TODO: Se usa la expiración de 24 horas como ejemplo, pero esto debería ser configurable
            return new PasswordResetToken(tokenResult, email);
        }

        public async Task<bool> UpdateUserAsync(User user, CancellationToken ct = default)
        {
            var currentUser = await userManager.FindByIdAsync(user.Id);
            if (currentUser == null)
                return false;

            var result = await userManager.UpdateAsync(currentUser);
            return result.Succeeded;
        }

        public async Task<bool> ValidateAndConsumeResetTokenAsync(string email, string token, string newPassword, CancellationToken ct = default)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            var result = await userManager.ResetPasswordAsync(user, token, newPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return false;
            }
            return true;
        }
    }
}