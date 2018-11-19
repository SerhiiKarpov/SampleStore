namespace SampleStore.Data.EF.Extensions
{
    using Microsoft.EntityFrameworkCore;

    using SampleStore.Data.Entities.Domain;
    using SampleStore.Data.Entities.Identity;

    /// <summary>
    /// Class encapsulating model builder extensions.
    /// </summary>
    public static class ModelBuilderExtensions
    {
        #region Fields

        /// <summary>
        /// The identity schema
        /// </summary>
        private const string IdentitySchema = "identity";

        #endregion Fields

        #region Methods

        /// <summary>
        /// Builds the domain model.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The specified builder to chain method calls.</returns>
        public static ModelBuilder BuildDomainModel(this ModelBuilder builder)
        {
            builder.Entity<Product>();
            builder.Entity<Photo>(
                x =>
                {
                    x.HasMany<Product>().WithOne().HasForeignKey(p => p.PhotoId).IsRequired(false);
                });

            return builder;
        }

        /// <summary>
        /// Builds the identity model.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>The specified builder to chain method calls.</returns>
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

        #endregion Methods
    }
}