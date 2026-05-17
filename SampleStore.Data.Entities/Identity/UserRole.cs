using System;

namespace SampleStore.Data.Entities.Identity;

public class UserRole : Entity
{
    public Guid RoleId { get; set; }

    public Guid UserId { get; set; }
}