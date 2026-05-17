using System;
using System.ComponentModel.DataAnnotations;

namespace SampleStore.Data.Entities.Identity;

public sealed class User : Entity
{
    public const int EmailMaxLength = 100;
    public const int FullNameMaxLength = 100;
    public const int PasswordHashMaxLength = 100;
    public const int PhoneNumberMaxLength = 50;

    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    [StringLength(EmailMaxLength)]
    public string Email { get; set; } = string.Empty;

    public bool EmailConfirmed { get; set; }

    [Required]
    [StringLength(FullNameMaxLength)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(PasswordHashMaxLength)]
    public string? PasswordHash { get; set; }

    [StringLength(PhoneNumberMaxLength)]
    public string? PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }
}