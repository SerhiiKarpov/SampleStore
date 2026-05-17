using System;
using System.ComponentModel.DataAnnotations;

namespace SampleStore.Data.Entities.Identity;

public class UserToken : Entity
{
    [Required]
    [StringLength(50)]
    public required string LoginProvider { get; set; }

    [Required]
    [StringLength(100)]
    public required string Name { get; set; }

    public Guid UserId { get; set; }

    [StringLength(500)]
    public string? Value { get; set; }
}