using System;

namespace MedReminder.Core.Entities;

public interface ITenantEntity
{
    Guid UserId { get; set; }
}
