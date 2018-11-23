namespace SampleStore.Data.Seed.Commands
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;

    using SampleStore.Common.Commands;
    using SampleStore.Common.Extensions;
    using SampleStore.Common.Services;
    using SampleStore.Data.Entities.Identity;

    /// <summary>
    /// Class encapsulating seeder command factory.
    /// </summary>
    /// <seealso cref="IAddUserToRolesCommandFactory" />
    /// <seealso cref="ICreateRolesCommandFactory" />
    /// <seealso cref="ICreateSuperAdminCommandFactory" />
    public class SeederCommandFactory : IAddUserToRolesCommandFactory, ICreateRolesCommandFactory, ICreateSuperAdminCommandFactory
    {
        #region Fields

        /// <summary>
        /// The date time service
        /// </summary>
        private readonly IDateTime _dateTimeService;

        /// <summary>
        /// The query materializer
        /// </summary>
        private readonly IQueryMaterializer _queryMaterializer;

        /// <summary>
        /// The role manager
        /// </summary>
        private readonly RoleManager<Role> _roleManager;

        /// <summary>
        /// The user manager
        /// </summary>
        private readonly UserManager<User> _userManager;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SeederCommandFactory" /> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="roleManager">The role manager.</param>
        /// <param name="queryMaterializer">The query materializer.</param>
        /// <param name="dateTimeService">The date time service.</param>
        public SeederCommandFactory(UserManager<User> userManager, RoleManager<Role> roleManager, IQueryMaterializer queryMaterializer, IDateTime dateTimeService)
        {
            _userManager = userManager.ThrowIfArgumentIsNull(nameof(userManager));
            _roleManager = roleManager.ThrowIfArgumentIsNull(nameof(roleManager));
            _queryMaterializer = queryMaterializer.ThrowIfArgumentIsNull(nameof(queryMaterializer));
            _dateTimeService = dateTimeService.ThrowIfArgumentIsNull(nameof(dateTimeService));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Creates the command.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="roles">The roles.</param>
        /// <returns>
        /// The command.
        /// </returns>
        ICommand<bool> IAddUserToRolesCommandFactory.CreateCommand(User user, IEnumerable<Role> roles)
        {
            return new AddUserToRolesCommand(_userManager, _roleManager, user, roles);
        }

        /// <summary>
        /// Creates the command.
        /// </summary>
        /// <returns>
        /// The command.
        /// </returns>
        ICommand<List<Role>> ICreateRolesCommandFactory.CreateCommand()
        {
            return new CreateRolesCommand(_roleManager, _queryMaterializer);
        }

        /// <summary>
        /// Creates the command.
        /// </summary>
        /// <param name="prototype">The prototype.</param>
        /// <param name="password">The password.</param>
        /// <returns>
        /// The command.
        /// </returns>
        ICommand<User> ICreateSuperAdminCommandFactory.CreateCommand(User prototype, string password)
        {
            return new CreateSuperAdminCommand(_userManager, _dateTimeService, prototype, password);
        }

        #endregion Methods
    }
}