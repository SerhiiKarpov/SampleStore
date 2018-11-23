namespace SampleStore.Data.Seed.Commands
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using SampleStore.Common.Commands;
    using SampleStore.Common.Extensions;
    using SampleStore.Data.Entities.Identity;
    using SampleStore.Data.Seed.Extensions;

    /// <summary>
    /// Class encapsulating add user to roles command.
    /// </summary>
    /// <seealso cref="IAddUserToRolesCommandFactory" />
    public class AddUserToRolesCommand : ICommand<bool>
    {
        #region Fields

        /// <summary>
        /// The role manager
        /// </summary>
        private readonly RoleManager<Role> _roleManager;

        /// <summary>
        /// The roles
        /// </summary>
        private readonly IEnumerable<Role> _roles;

        /// <summary>
        /// The user
        /// </summary>
        private readonly User _user;

        /// <summary>
        /// The user manager
        /// </summary>
        private readonly UserManager<User> _userManager;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AddUserToRolesCommand" /> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="roleManager">The role manager.</param>
        /// <param name="user">The user.</param>
        /// <param name="roles">The roles.</param>
        public AddUserToRolesCommand(UserManager<User> userManager, RoleManager<Role> roleManager, User user, IEnumerable<Role> roles)
        {
            _userManager = userManager.ThrowIfArgumentIsNull(nameof(userManager));
            _roleManager = roleManager.ThrowIfArgumentIsNull(nameof(roleManager));
            _user = user.ThrowIfArgumentIsNull(nameof(user));
            _roles = roles.ThrowIfArgumentIsNull(nameof(roles));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Executes the command encapsulated by this instance.
        /// </summary>
        /// <returns>
        /// The command result.
        /// </returns>
        public async Task<bool> Do()
        {
            var roleNames = await Task.WhenAll(_roles.Select(role => _roleManager.GetRoleNameAsync(role)));
            var addToRolesResult = await _userManager.AddToRolesAsync(_user, roleNames);
            addToRolesResult.ThrowIfFailed(() => $"Failed to add user {_user.Email} to roles: {string.Join(", ", roleNames)}. Check logs for details.");
            return true;
        }

        #endregion Methods
    }
}