using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NexoRecruiter.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NexoRecruiter.Web.Features.Auth.Models;

namespace NexoRecruiter.Infrastructure.Persistence
{
    public class RecruiterDbContext : IdentityDbContext<ApplicationUser>
    {

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Candidate>()
                .HasIndex(c => c.Email);

            modelBuilder.Entity<Candidate>()
                .Property(u => u.FullName)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<Candidate>()
                .Property(u => u.Email)
                .HasMaxLength(256)
                .IsRequired();

            modelBuilder.Entity<Candidate>()
                .Property(u => u.Phone)
                .HasMaxLength(50)
                .IsRequired();
        }

        public RecruiterDbContext(DbContextOptions options) : base(options)
        {
        }

        protected RecruiterDbContext()
        {
        }

        public DbSet<Application> Applications { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<JobRole> JobRoles { get; set; }
    }
}