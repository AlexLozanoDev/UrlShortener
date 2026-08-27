using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Link> Links => Set<Link>();
        public DbSet<LinkClickStats> LinkClickStats => Set<LinkClickStats>();
        public DbSet<User> Users => Set<User>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RefreshToken>(builder =>
            {
                builder.HasKey(t => t.Id);
                builder.Property(t => t.Token).IsRequired();

                builder.HasOne<User>()
                   .WithMany(u => u.RefreshTokens)
                   .HasForeignKey(t => t.UserId);
            });
        }

    }
}
