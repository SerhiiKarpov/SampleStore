using System.ComponentModel.DataAnnotations;

namespace SampleStore.Data.Entities.Identity;

public sealed class Role : Entity
{
    public const int NameMaxLength = 50;

    [Required]
    [StringLength(NameMaxLength)]
    public string Name { get; set; } = string.Empty;
}