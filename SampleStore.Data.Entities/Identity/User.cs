using System;
using System.ComponentModel.DataAnnotations;

namespace SampleStore.Data.Entities.Identity;

public class User : Entity
{
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    public bool EmailConfirmed { get; set; }

    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [StringLength(100)]
    public string? PasswordHash { get; set; }

    [StringLength(50)]
    public string? PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }
}