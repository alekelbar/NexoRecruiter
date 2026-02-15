using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexoRecruiter.Application.DTOs.Auth
{
    public class RequestEmailConfirmationDTO
    {
       public string Email { get; set; } = default!; 
    }
}