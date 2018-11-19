namespace SampleStore.Data.Seed
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using SampleStore.Common;
    using SampleStore.Data.Entities.Identity;
    using SampleStore.Data.Extensions;
    using SampleStore.Data.Seed.Commands;

    /// <summary>
    /// Class encapsulating database seeder.
    /// </summary>
    public class DatabaseSeeder
    {
        #region Fields

        /// <summary>
        /// The add user to roles command
        /// </summary>
        private readonly IAddUserToRolesCommand _addUserToRolesCommand;

        /// <summary>
        /// The create roles command
        /// </summary>
        private readonly ICreateRolesCommand _createRolesCommand;

        /// <summary>
        /// The create super admin command
        /// </summary>
        private readonly ICreateSuperAdminCommand _createSuperAdminCommand;

        /// <summary>
        /// The query materializer
        /// </summary>
        private readonly IQueryMaterializer _queryMaterializer;

        /// <summary>
        /// The user manager
        /// </summary>
        private readonly UserManager<User> _userManager;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseSeeder" /> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="queryMaterializer">The query materializer.</param>
        /// <param name="createSuperAdminCommand">The create super admin command.</param>
        /// <param name="createRolesCommand">The create roles command.</param>
        /// <param name="addUserToRolesCommand">The add user to roles command.</param>
        public DatabaseSeeder(
            UserManager<User> userManager,
            IQueryMaterializer queryMaterializer,
            ICreateSuperAdminCommand createSuperAdminCommand,
            ICreateRolesCommand createRolesCommand,
            IAddUserToRolesCommand addUserToRolesCommand)
        {
            _userManager = userManager.ThrowIfArgumentIsNull(nameof(userManager));
            _queryMaterializer = queryMaterializer.ThrowIfArgumentIsNull(nameof(queryMaterializer));
            _createSuperAdminCommand = createSuperAdminCommand.ThrowIfArgumentIsNull(nameof(createSuperAdminCommand));
            _createRolesCommand = createRolesCommand.ThrowIfArgumentIsNull(nameof(createRolesCommand));
            _addUserToRolesCommand = addUserToRolesCommand.ThrowIfArgumentIsNull(nameof(addUserToRolesCommand));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Checks if seeding is needed.
        /// </summary>
        /// <returns><c>true</c> if seeding is need, <c>false</c> otherwise.</returns>
        public async Task<bool> NeedsSeeding()
        {
            var hasUsers = await _queryMaterializer.Any(_userManager.Users);
            return !hasUsers;
        }

        /// <summary>
        /// Seeds this instance.
        /// </summary>
        /// <param name="superAdminPrototype">The super admin prototype.</param>
        /// <param name="defaultPassword">The default password.</param>
        /// <returns>
        /// The <see cref="Task" />.
        /// </returns>
        public async Task Seed(User superAdminPrototype, string defaultPassword)
        {
            var roles = await _createRolesCommand.CreateRoles();
            var superAdmin = await _createSuperAdminCommand.CreateSuperAdmin(superAdminPrototype, defaultPassword);
            await _addUserToRolesCommand.AddUserToRoles(superAdmin, roles);
        }

        #endregion Methods
    }
}