using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NexoRecruiter.Domain.Entities
{
    /// <summary>
    /// Propósito: persona que aplica a uno o varios roles. 
    /// </summary>
    public class Candidate
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public ICollection<Candidate> Applications = [];
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}