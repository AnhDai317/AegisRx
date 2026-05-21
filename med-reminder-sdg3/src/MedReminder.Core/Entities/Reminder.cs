using System;

namespace MedReminder.Core.Entities;

public class Reminder
{
    public int Id { get; set; }
    public int MedicationId { get; set; }
    public TimeSpan TimeOfDay { get; set; }
    public FrequencyType Frequency { get; set; }
    
    // Tracking properties
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public Medication? Medication { get; set; }
}
