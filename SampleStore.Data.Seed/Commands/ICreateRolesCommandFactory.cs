namespace SampleStore.Data.Seed.Commands
{
    using System.Collections.Generic;

    using SampleStore.Common.Commands;
    using SampleStore.Data.Entities.Identity;

    /// <summary>
    /// An interface for create roles command factory.
    /// </summary>
    public interface ICreateRolesCommandFactory
    {
        #region Methods

        /// <summary>
        /// Creates the command.
        /// </summary>
        /// <returns>
        /// The command.
        /// </returns>
        ICommand<List<Role>> CreateCommand();

        #endregion Methods
    }
}