using System;
using System.ComponentModel.DataAnnotations;

namespace SampleStore.Data.Entities.Identity;

public class UserLogin : Entity
{
    [Required]
    [StringLength(50)]
    public required string LoginProvider { get; set; }

    [Required]
    [StringLength(100)]
    public required string ProviderDisplayName { get; set; }

    [Required]
    [StringLength(500)]
    public required string ProviderKey { get; set; }

    public Guid UserId { get; set; }
}