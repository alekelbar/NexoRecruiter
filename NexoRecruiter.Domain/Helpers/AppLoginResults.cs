using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexoRecruiter.Domain.Helpers
{
    public class AppLoginResults
    {
        public const int UserNotFound = 1;
        public const int EmailNotConfirmed = 2;
        public const int InvalidPassword = 3;
        public const int InactiveAccount = 4;
    }
}