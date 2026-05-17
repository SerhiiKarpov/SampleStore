using System;
using System.ComponentModel.DataAnnotations;

namespace SampleStore.Data.Entities.Domain;

public sealed class Product : Entity
{
    public const int DescriptionMaxLength = 500;
    public const int NameMaxLength = 50;

    [StringLength(DescriptionMaxLength)]
    public string? Description { get; set; }

    [Required]
    [StringLength(NameMaxLength)]
    public string Name { get; set; } = string.Empty;

    public Guid? PhotoId { get; set; }

    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

    public double Quantity { get; set; }
}