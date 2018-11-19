namespace SampleStore.Data.Seed.Commands
{
    using System.Threading.Tasks;

    using SampleStore.Data.Entities.Identity;

    /// <summary>
    /// An interface for create super admin command.
    /// </summary>
    public interface ICreateSuperAdminCommand
    {
        #region Methods

        /// <summary>
        /// Creates the super admin.
        /// </summary>
        /// <param name="prototype">The prototype.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// The super admin user.
        /// </returns>
        Task<User> CreateSuperAdmin(User prototype, string password);

        #endregion Methods
    }
}