using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleStore.Data.EF.Constants;
using SampleStore.Data.Entities.Identity;

namespace SampleStore.Data.EF.Configuration.Identity;

internal sealed class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
{
    public void Configure(EntityTypeBuilder<RoleClaim> builder)
    {
        builder.ToTable(nameof(RoleClaim), Schemas.Identity);
    }
}
