using System;

namespace SampleStore.Data.Entities.Identity;

public sealed class RoleClaim : Entity
{
    public Guid RoleId { get; set; }
}