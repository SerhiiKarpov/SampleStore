
using System.Collections.Generic;

using SampleStore.Common.Commands;
using SampleStore.Data.Entities.Identity;

namespace SampleStore.Data.Seed.Commands;
/// <summary>
/// An interface for add user to roles command factory.
/// </summary>
public interface IAddUserToRolesCommandFactory
{
    #region Methods

    /// <summary>
    /// Creates the command.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <param name="roles">The roles.</param>
    /// <returns>
    /// The command.
    /// </returns>
    ICommand<bool> CreateCommand(User user, IEnumerable<Role> roles);

    #endregion Methods
}