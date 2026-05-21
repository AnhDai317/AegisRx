using System;

namespace MedReminder.Core.Services;

public interface ICurrentUserService
{
    Guid UserId { get; }
}

// Temporary Mock Implementation for local development
public class MockCurrentUserService : ICurrentUserService
{
    // Hardcoded UserId representing a logged-in user
    public Guid UserId { get; } = Guid.Parse("11111111-1111-1111-1111-111111111111");
}
