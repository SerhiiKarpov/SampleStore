namespace SampleStore.Mapping.DataToUI.Extensions
{
    using SampleStore.Data.Entities.Identity;
    using SampleStore.Mapping.Extensions;
    using SampleStore.UI.ViewModels.Identity;

    /// <summary>
    /// Class encapsulating mapping configuration extensions.
    /// </summary>
    public static class MappingConfigurationExtensions
    {
        #region Methods

        /// <summary>
        /// Adds the data to UI mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>
        /// The data to UI mappings.
        /// </returns>
        public static IMappingConfiguration AddDataToUIMappings(this IMappingConfiguration configuration)
        {
            return configuration
                .Configure<ExternalLoginViewModel, User>(
                    mapping => mapping
                        .Map(u => u.DateOfBirth, elvm => elvm.DateOfBirth)
                        .Map(u => u.Email, elvm => elvm.Email)
                        .Ignore(u => u.EmailConfirmed)
                        .Map(u => u.FullName, elvm => elvm.Name)
                        .Ignore(u => u.Id)
                        .Ignore(u => u.PasswordHash)
                        .Ignore(u => u.PhoneNumber)
                        .Ignore(u => u.PhoneNumberConfirmed)
                        .Ignore(u => u.RowVersion)
                        .Ignore(u => u.TwoFactorEnabled))
                .Configure<RegistrationViewModel, User>(
                    mapping => mapping
                        .Map(u => u.DateOfBirth, rvm => rvm.DateOfBirth)
                        .Map(u => u.Email, rvm => rvm.Email)
                        .Ignore(u => u.EmailConfirmed)
                        .Map(u => u.FullName, rvm => rvm.Name)
                        .Ignore(u => u.Id)
                        .Ignore(u => u.PasswordHash)
                        .Ignore(u => u.PhoneNumber)
                        .Ignore(u => u.PhoneNumberConfirmed)
                        .Ignore(u => u.RowVersion)
                        .Ignore(u => u.TwoFactorEnabled));
        }

        #endregion Methods
    }
}