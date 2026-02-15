using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using NexoRecruiter.Domain.Integrations.Email;
using NexoRecruiter.Domain.Repositories.Email.ValueObjects;

namespace NexoRecruiter.Infrastructure.Integrations.Email
{
    public class SmtpEmailIntegration(IOptionsSnapshot<EmailSettings> settings) : IEmailIntegration
    {
        private readonly IOptionsSnapshot<EmailSettings> settings = settings;
        public async Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default)
        {
            var emailSettings = this.settings.Value;
            emailSettings.Validate();

            using (var smtpClient = new SmtpClient(emailSettings.Host, emailSettings.Port))
            {
                smtpClient.EnableSsl = emailSettings.EnableSSL;
                smtpClient.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);
                smtpClient.Timeout = 10000;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(emailSettings.SenderEmail, emailSettings.SenderName),
                    Subject = subject,
                    Body = htmlBody,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(to);

                try
                {
                    await smtpClient.SendMailAsync(mailMessage, ct);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Error al enviar email a {to}: {ex.Message}", ex);
                }
            }
        }
    }
}