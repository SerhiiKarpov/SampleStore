using System;
using System.ComponentModel.DataAnnotations;

namespace SampleStore.Data.Entities.Identity;

public sealed class UserClaim : Entity
{
    public const int TypeMaxLength = 100;
    public const int ValueMaxLength = 200;

    [StringLength(TypeMaxLength)]
    public string? Type { get; set; }

    public Guid UserId { get; set; }

    [StringLength(ValueMaxLength)]
    public string? Value { get; set; }
}