using System;

namespace MedReminder.Core.Entities;

public class Reminder : IAuditableEntity, ITenantEntity, ISoftDelete
{
    public int Id { get; set; }
    public int MedicationId { get; set; }
    public TimeSpan TimeOfDay { get; set; }
    public string TimezoneId { get; set; } = "UTC"; // IANA timezone string
    public FrequencyType Frequency { get; set; }
    
    // Tracking properties
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    
    // Multi-tenant & Soft Delete
    public Guid UserId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    
    public Medication? Medication { get; set; }
}
