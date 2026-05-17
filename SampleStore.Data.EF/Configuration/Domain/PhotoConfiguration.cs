using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleStore.Data.Entities.Domain;

namespace SampleStore.Data.EF.Configuration.Domain;

internal sealed class PhotoConfiguration : IEntityTypeConfiguration<Photo>
{
    public void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder.HasMany<Product>().WithOne().HasForeignKey(p => p.PhotoId).IsRequired(false);
    }
}
