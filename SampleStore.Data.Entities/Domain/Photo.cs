using System.ComponentModel.DataAnnotations;

namespace SampleStore.Data.Entities.Domain;

public sealed class Photo : Entity
{
    public const int MimeTypeMaxLength = 50;

#pragma warning disable CA1819 // Properties should not return arrays
    public byte[]? Image { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays

    [Required]
    [StringLength(MimeTypeMaxLength)]
    public string MimeType { get; set; } = string.Empty;
}