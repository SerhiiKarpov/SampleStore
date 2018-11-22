namespace SampleStore.Mapping.DataToServices.AutoMapper.Extensions
{
    using System.Security.Claims;

    using Microsoft.AspNetCore.Identity;

    using SampleStore.Data.Entities.Identity;

    /// <summary>
    /// Class encapsulating mapper configuration expression extensions.
    /// </summary>
    public static class MappingConfigurationExtensions
    {
        #region Methods

        /// <summary>
        /// Adds the automatic mapper data to services mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>
        /// The automatic mapper data to services mappings.
        /// </returns>
        public static IMappingConfiguration AddDataToServicesMappings(this IMappingConfiguration configuration)
        {
            return configuration
                .Configure<UserLoginInfo, UserLogin>(
                    mapping => mapping
                        .Ignore(ul => ul.Id)
                        .Ignore(ul => ul.UserId)
                        .Ignore(ul => ul.RowVersion))
                .Configure<UserLogin, UserLoginInfo>(
                    mapping => mapping
                        .Construct(ul => new UserLoginInfo(ul.LoginProvider, ul.ProviderKey, ul.ProviderDisplayName)))
                .Configure<Claim, UserClaim>(
                    mapping => mapping
                        .Ignore(uc => uc.Id)
                        .Ignore(uc => uc.RowVersion)
                        .Ignore(uc => uc.UserId))
                .Configure<UserClaim, Claim>(
                    mapping => mapping
                        .Construct(uc => new Claim(uc.Type, uc.Value))
                        .Ignore(c => c.Properties));
        }

        #endregion Methods
    }
}