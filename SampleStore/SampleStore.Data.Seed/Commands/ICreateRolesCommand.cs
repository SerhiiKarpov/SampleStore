namespace SampleStore.Data.Seed.Commands
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using SampleStore.Data.Entities.Identity;

    /// <summary>
    /// An interface for create roles command.
    /// </summary>
    public interface ICreateRolesCommand
    {
        #region Methods

        /// <summary>
        /// Creates the roles.
        /// </summary>
        /// <returns>
        /// The roles.
        /// </returns>
        Task<List<Role>> CreateRoles();

        #endregion Methods
    }
}