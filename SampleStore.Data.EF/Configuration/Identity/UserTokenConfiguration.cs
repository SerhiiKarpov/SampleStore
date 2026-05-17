using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SampleStore.Data.EF.Constants;

using SampleStore.Data.Entities.Identity;

namespace SampleStore.Data.EF.Configuration.Identity;

internal sealed class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        builder.ToTable(nameof(UserToken), Schemas.Identity);
    }
}
