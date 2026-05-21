using System;

namespace MedReminder.Core.Entities;

public class Medication : IAuditableEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateOnly ExpiryDate { get; set; }
    
    // Tracking properties
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
