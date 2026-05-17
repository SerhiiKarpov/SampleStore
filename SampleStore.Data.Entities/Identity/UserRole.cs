using System;

namespace SampleStore.Data.Entities.Identity;

public sealed class UserRole : Entity
{
    public Guid RoleId { get; set; }

    public Guid UserId { get; set; }
}