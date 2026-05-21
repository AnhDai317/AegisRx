using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MedReminder.Core.Entities;

namespace MedReminder.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<Medication> Medications { get; set; } = null!;
    public DbSet<Reminder> Reminders { get; set; } = null!;
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries<IAuditableEntity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
                // Ensure CreatedAt is not modified
                entry.Property(nameof(IAuditableEntity.CreatedAt)).IsModified = false;
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
