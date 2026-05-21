using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MedReminder.Core.Entities;
using MedReminder.Core.Services;

namespace MedReminder.Infrastructure;

public class ApplicationDbContext : DbContext
{
    private readonly ICurrentUserService _currentUserService;

    public DbSet<Medication> Medications { get; set; } = null!;
    public DbSet<Reminder> Reminders { get; set; } = null!;
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService) : base(options)
    {
        _currentUserService = currentUserService;
    }

    public override int SaveChanges()
    {
        ApplyConcepts();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyConcepts();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyConcepts()
    {
        var entries = ChangeTracker.Entries();

        foreach (var entry in entries)
        {
            if (entry.Entity is IAuditableEntity auditableEntity)
            {
                if (entry.State == EntityState.Added)
                {
                    auditableEntity.CreatedAt = DateTimeOffset.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    auditableEntity.UpdatedAt = DateTimeOffset.UtcNow;
                    entry.Property(nameof(IAuditableEntity.CreatedAt)).IsModified = false;
                }
            }

            if (entry.Entity is ITenantEntity tenantEntity && entry.State == EntityState.Added)
            {
                tenantEntity.UserId = _currentUserService.UserId;
            }

            if (entry.Entity is ISoftDelete softDeleteEntity && entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                softDeleteEntity.IsDeleted = true;
                softDeleteEntity.DeletedAt = DateTimeOffset.UtcNow;
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Global Query Filters for Soft Delete & Multi-Tenant
        modelBuilder.Entity<Medication>().HasQueryFilter(e => !e.IsDeleted && e.UserId == _currentUserService.UserId);
        modelBuilder.Entity<Reminder>().HasQueryFilter(e => !e.IsDeleted && e.UserId == _currentUserService.UserId);

        modelBuilder.Entity<Medication>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.HasIndex(e => e.Name);
        });

        modelBuilder.Entity<Reminder>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Frequency)
                  .IsRequired()
                  .HasConversion<string>()
                  .HasMaxLength(50);
            
            entity.Property(e => e.TimezoneId)
                  .IsRequired()
                  .HasMaxLength(100);
                  
            entity.HasOne(d => d.Medication)
                  .WithMany()
                  .HasForeignKey(d => d.MedicationId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
