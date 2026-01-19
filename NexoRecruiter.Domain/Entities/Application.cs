using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NexoRecruiter.Domain.Enums;

namespace NexoRecruiter.Domain.Entities
{
    /// <summary>
    /// Propósito: una postulación concreta de un candidato a un rol. 
    /// </summary>
    public class Application
    {
        public Guid Id { get; set; }
        public Guid CandidateId { get; set; }   // FK real en BD
        public Candidate Candidate { get; set; } = null!;
        public Guid JobRoleId { get; set; }
        public JobRole Role { get; set; } = null!;
        public string CvFilePath { get; set; } = null!;
        public JobStatus Status { get; set; } 
        public ApplicationSource Source { get; set; }
        public double? Score { get; set; }
        public string? Strengths { get; set; } 
        public string? Weaknesses { get; set; }
        public string? LlmModelUsed { get; set; } 
        public DateTime? EvaluatedAt { get; set; } 
        public DateTime CreatedAt {get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}