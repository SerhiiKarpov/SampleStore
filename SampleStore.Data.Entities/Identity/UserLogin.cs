using System;
using System.ComponentModel.DataAnnotations;

namespace SampleStore.Data.Entities.Identity;

public sealed class UserLogin : Entity
{
    public const int LoginProviderMaxLength = 50;
    public const int ProviderDisplayNameMaxLength = 100;
    public const int ProviderKeyMaxLength = 500;

    [Required]
    [StringLength(LoginProviderMaxLength)]
    public string LoginProvider { get; set; } = string.Empty;

    [Required]
    [StringLength(ProviderDisplayNameMaxLength)]
    public string ProviderDisplayName { get; set; } = string.Empty;

    [Required]
    [StringLength(ProviderKeyMaxLength)]
    public string ProviderKey { get; set; } = string.Empty;

    public Guid UserId { get; set; }
}