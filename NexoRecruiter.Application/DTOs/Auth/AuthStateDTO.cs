using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexoRecruiter.Application.DTOs.Auth
{
    public class AuthStateDTO
    {
        public AppUserDTO? User { get; set; }
        public bool IsAuthenticated => User != null;
    }
}