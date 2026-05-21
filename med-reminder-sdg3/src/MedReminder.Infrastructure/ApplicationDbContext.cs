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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Medication>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            // Optional: Index on Name if searches are frequent
            entity.HasIndex(e => e.Name);
        });

        modelBuilder.Entity<Reminder>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Frequency)
                  .IsRequired()
                  .HasConversion<string>() // Store Enum as string in DB for readability
                  .HasMaxLength(50);
                  
            entity.HasOne(d => d.Medication)
                  .WithMany()
                  .HasForeignKey(d => d.MedicationId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
