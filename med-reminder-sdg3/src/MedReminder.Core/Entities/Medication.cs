using System;

namespace MedReminder.Core.Entities;

public class Medication : IAuditableEntity, ITenantEntity, ISoftDelete
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateOnly ExpiryDate { get; set; }
    
    // Tracking properties
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    
    // Multi-tenant & Soft Delete
    public Guid UserId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}
