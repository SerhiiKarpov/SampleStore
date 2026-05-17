using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleStore.Data.EF.Constants;
using SampleStore.Data.Entities.Identity;

namespace SampleStore.Data.EF.Configuration.Identity;

internal sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable(nameof(UserRole), Schemas.Identity);
    }
}
