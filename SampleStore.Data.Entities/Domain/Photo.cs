using System.ComponentModel.DataAnnotations;

namespace SampleStore.Data.Entities.Domain;

public class Photo : Entity
{
#pragma warning disable CA1819 // Properties should not return arrays
    public byte[]? Image { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

    [Required]
    [StringLength(50)]
    public required string MimeType { get; set; }
}