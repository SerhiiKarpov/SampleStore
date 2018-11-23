namespace SampleStore.Data.Seed.Commands
{
    using SampleStore.Common.Commands;
    using SampleStore.Data.Entities.Identity;

    /// <summary>
    /// An interface for create super admin command factory.
    /// </summary>
    public interface ICreateSuperAdminCommandFactory
    {
        #region Methods

        /// <summary>
        /// Creates the command.
        /// </summary>
        /// <param name="prototype">The prototype.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// The command.
        /// </returns>
        ICommand<User> CreateCommand(User prototype, string password);

        #endregion Methods
    }
}