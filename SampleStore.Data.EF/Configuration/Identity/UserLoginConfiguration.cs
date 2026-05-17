using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleStore.Data.EF.Constants;
using SampleStore.Data.Entities.Identity;

namespace SampleStore.Data.EF.Configuration.Identity;

internal sealed class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
{
    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {
        builder.ToTable(nameof(UserLogin), Schemas.Identity);
    }
}
