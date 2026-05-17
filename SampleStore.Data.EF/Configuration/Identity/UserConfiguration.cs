using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using SampleStore.Data.EF.Constants;
using SampleStore.Data.Entities.Identity;

namespace SampleStore.Data.EF.Configuration.Identity;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User), Schemas.Identity);
        builder.HasMany<UserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
        builder.HasMany<UserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
        builder.HasMany<UserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
        builder.HasMany<UserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
    }
}
