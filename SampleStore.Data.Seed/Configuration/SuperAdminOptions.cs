using System.ComponentModel.DataAnnotations;

using SampleStore.Data.Entities.Identity;

namespace SampleStore.Data.Seed.Configuration;

public sealed class SuperAdminOptions
{
    public const string Key = "SuperAdmin";

    [Required]
    public User Prototype { get; set; } = null!;

    [Required]
    public string Password { get; set; } = string.Empty;
}