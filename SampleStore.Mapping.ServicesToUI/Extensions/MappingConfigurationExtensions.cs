namespace SampleStore.Mapping.ServicesToUI.Extensions
{
    using System;
    using System.Security.Claims;

    using Microsoft.AspNetCore.Identity;

    using SampleStore.UI.ViewModels.Identity;

    /// <summary>
    /// Class encapsulating mapping configuration extensions.
    /// </summary>
    public static class MappingConfigurationExtensions
    {
        #region Methods

        /// <summary>
        /// Adds the services to UI mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>
        /// The services to UI mappings.
        /// </returns>
        public static IMappingConfiguration AddServicesToUIMappings(this IMappingConfiguration configuration)
        {
            return configuration
                .Configure<ExternalLoginInfo, ExternalLoginViewModel>(
                    mapping => mapping
                        .Map(elvm => elvm.Email, eli => eli.Principal.FindFirstValue(ClaimTypes.Email))
                        .Map(
                            elvm => elvm.Name,
                            eli => eli.Principal.HasClaim(claim => claim.Type == ClaimTypes.GivenName)
                                    && eli.Principal.HasClaim(claim => claim.Type == ClaimTypes.Surname)
                                ? $"{eli.Principal.FindFirstValue(ClaimTypes.GivenName)} {eli.Principal.FindFirstValue(ClaimTypes.Surname)}"
                                : eli.Principal.FindFirstValue(ClaimTypes.Name))
                        .Map(elvm => elvm.DateOfBirth, eli => DateTime.Parse(eli.Principal.FindFirstValue(ClaimTypes.DateOfBirth))));
        }

        #endregion Methods
    }
}