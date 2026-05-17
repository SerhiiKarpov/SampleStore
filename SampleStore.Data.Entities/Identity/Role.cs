using System.ComponentModel.DataAnnotations;

namespace SampleStore.Data.Entities.Identity;

public class Role : Entity
{
    [Required]
    [StringLength(50)]
    public required string Name { get; set; }
}