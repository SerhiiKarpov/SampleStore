namespace SampleStore.Data.Seed.Commands
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SampleStore.Data.Entities.Identity;

    /// <summary>
    /// An interface for add user to roles command.
    /// </summary>
    public interface IAddUserToRolesCommand
    {
        #region Methods

        /// <summary>
        /// Adds the user to roles.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="roles">The roles.</param>
        /// <returns>
        /// The <see cref="Task" />.
        /// </returns>
        Task AddUserToRoles(User user, IEnumerable<Role> roles);

        #endregion Methods
    }
}