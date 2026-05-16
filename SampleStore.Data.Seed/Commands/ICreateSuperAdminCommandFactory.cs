
using SampleStore.Common.Commands;
using SampleStore.Data.Entities.Identity;

namespace SampleStore.Data.Seed.Commands;
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