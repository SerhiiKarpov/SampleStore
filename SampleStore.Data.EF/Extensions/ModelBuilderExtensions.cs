using Microsoft.EntityFrameworkCore;

using SampleStore.Data.Entities.Domain;
using SampleStore.Data.Entities.Identity;

namespace SampleStore.Data.EF.Extensions;

public static class ModelBuilderExtensions
{
    private const string IdentitySchema = "identity";

    public static ModelBuilder BuildDomainModel(this ModelBuilder builder)
    {
        builder.Entity<Product>(
            x =>
            {
                x.Property(p => p.Price).HasColumnType("decimal(18,2)");
            });
        builder.Entity<Photo>(
            x =>
            {
                x.HasMany<Product>().WithOne().HasForeignKey(p => p.PhotoId).IsRequired(false);
            });

        return builder;
    }

    public static ModelBuilder BuildIdentityModel(this ModelBuilder builder)
    {
        builder.Entity<User>(
            x =>
            {
                x.ToTable(nameof(User), IdentitySchema);
                x.HasMany<UserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
                x.HasMany<UserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
                x.HasMany<UserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
                x.HasMany<UserRole>().WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
            });
        builder.Entity<Role>(
            x =>
            {
                x.ToTable(nameof(Role), IdentitySchema);
                x.HasMany<RoleClaim>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
                x.HasMany<UserRole>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();
            });
        builder.Entity<RoleClaim>(x => x.ToTable(nameof(RoleClaim), IdentitySchema));
        builder.Entity<UserRole>(x => x.ToTable(nameof(UserRole), IdentitySchema));
        builder.Entity<UserClaim>(x => x.ToTable(nameof(UserClaim), IdentitySchema));
        builder.Entity<UserLogin>(x => x.ToTable(nameof(UserLogin), IdentitySchema));
        builder.Entity<UserToken>(x => x.ToTable(nameof(UserToken), IdentitySchema));

        return builder;
    }
}