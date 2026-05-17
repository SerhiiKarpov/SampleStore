using System;
using System.ComponentModel.DataAnnotations;

namespace SampleStore.Data.Entities.Domain;

public class Product : Entity
{
    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    [StringLength(50)]
    public required string Name { get; set; }

    public Guid? PhotoId { get; set; }

    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

    public double Quantity { get; set; }
}