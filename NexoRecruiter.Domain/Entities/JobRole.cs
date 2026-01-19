using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NexoRecruiter.Domain.Enums;

namespace NexoRecruiter.Domain.Entities
{
    /// <summary>
    /// Propósito: una vacante que la empresa tiene abierta (o tuvo).
    /// </summary>
    public class JobRole
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Seniority Seniority { get; set; }
        public JobStatus Status { get; set; }
        public bool IsPublished { get; set; } = false;
        public Guid CreatedByUserId { get; set; } 
        public DateTime? PublishedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}