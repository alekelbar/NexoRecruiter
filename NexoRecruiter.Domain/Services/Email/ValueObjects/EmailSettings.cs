using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexoRecruiter.Domain.Services.Email.ValueObjects
{
    public class EmailSettings
    {
        public string Host { get; set; } = default!;
        public int Port { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public bool EnableSSL { get; set; } = true;
        public string SenderEmail { get; set; } = default!;
        public string SenderName { get; set; } = default!;

        public void Validate()
        {
           if(string.IsNullOrWhiteSpace(Host))
                throw new ArgumentException("Host no puede estar vacío");
           if(Port <= 0)
                throw new ArgumentException("Port debe ser un número positivo");
           if(string.IsNullOrWhiteSpace(Username))
                throw new ArgumentException("Username no puede estar vacío");
           if(string.IsNullOrWhiteSpace(Password))
                throw new ArgumentException("Password no puede estar vacío");
           if(string.IsNullOrWhiteSpace(SenderEmail))
                throw new ArgumentException("SenderEmail no puede estar vacío");
           if(string.IsNullOrWhiteSpace(SenderName))
                throw new ArgumentException("SenderName no puede estar vacío");
        }
    }
}