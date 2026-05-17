using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleStore.Data.Entities.Domain;

namespace SampleStore.Data.EF.Configuration.Domain;

internal sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
    }
}
