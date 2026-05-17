using System;
using System.ComponentModel.DataAnnotations;

namespace SampleStore.Data.Entities.Identity;

public class UserClaim : Entity
{
    [StringLength(100)]
    public string? Type { get; set; }

    public Guid UserId { get; set; }

    [StringLength(200)]
    public string? Value { get; set; }
}