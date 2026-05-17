using System;
using System.ComponentModel.DataAnnotations;

namespace SampleStore.Data.Entities;

public abstract class Entity
{
    protected Entity()
    {
        Id = Guid.NewGuid();
    }

    [Key]
    public Guid Id { get; set; }

    [Timestamp]
#pragma warning disable CA1819 // Properties should not return arrays
    public byte[]? RowVersion { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays
}