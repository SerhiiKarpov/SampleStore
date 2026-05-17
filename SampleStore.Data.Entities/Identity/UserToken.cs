using System;
using System.ComponentModel.DataAnnotations;

namespace SampleStore.Data.Entities.Identity;

public sealed class UserToken : Entity
{
    public const int LoginProviderMaxLength = 50;
    public const int NameMaxLength = 100;
    public const int ValueMaxLength = 500;

    [Required]
    [StringLength(LoginProviderMaxLength)]
    public string LoginProvider { get; set; } = string.Empty;

    [Required]
    [StringLength(NameMaxLength)]
    public string Name { get; set; } = string.Empty;

    public Guid UserId { get; set; }

    [StringLength(ValueMaxLength)]
    public string? Value { get; set; }
}