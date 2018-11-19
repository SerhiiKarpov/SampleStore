namespace SampleStore.Data.Seed.Commands
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using SampleStore.Common;
    using SampleStore.Data.Entities.Identity;
    using SampleStore.Data.Seed.Extensions;

    /// <summary>
    /// Class encapsulating add user to roles command.
    /// </summary>
    /// <seealso cref="IAddUserToRolesCommand" />
    public class AddUserToRolesCommand : IAddUserToRolesCommand
    {
        #region Fields

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
        /// Initializes a new instance of the <see cref="AddUserToRolesCommand" /> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="roleManager">The role manager.</param>
        public AddUserToRolesCommand(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager.ThrowIfArgumentIsNull(nameof(userManager));
            _roleManager = roleManager.ThrowIfArgumentIsNull(nameof(roleManager));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Adds the user to roles.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="roles">The roles.</param>
        /// <returns>
        /// The <see cref="T:System.Threading.Tasks.Task" />.
        /// </returns>
        public async Task AddUserToRoles(User user, IEnumerable<Role> roles)
        {
            user.ThrowIfArgumentIsNull(nameof(user));
            roles.ThrowIfArgumentIsNull(nameof(roles));

            var roleNames = new List<string>();
            foreach (var role in roles)
            {
                var roleName = await _roleManager.GetRoleNameAsync(role);
                roleNames.Add(roleName);
            }

            var addToRolesResult = await _userManager.AddToRolesAsync(user, roleNames);
            addToRolesResult.ThrowIfFailed(() => $"Failed to add user {user.Email} to roles: {string.Join(", ", roleNames)}. Check logs for details.");
        }

        #endregion Methods
    }
}