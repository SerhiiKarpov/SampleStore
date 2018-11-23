namespace SampleStore.Services.Identity.Extensions
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    using SampleStore.Common.Extensions;
    using SampleStore.Data.Entities.Identity;

    /// <summary>
    /// Class encapsulating service collection extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        #region Methods

        /// <summary>
        /// Adds the customized identity.
        /// </summary>
        /// <param name="services">The services.</param>
        public static void AddCustomizedIdentity(this IServiceCollection services)
        {
            services.ThrowIfArgumentIsNull(nameof(services));

            services.AddIdentity<User, Role>()
                .AddDefaultTokenProviders();

            services.AddScoped<IUserStore<User>, UserStore>();
            services.AddScoped<IRoleStore<Role>, RoleStore>();
        }

        #endregion Methods
    }
}