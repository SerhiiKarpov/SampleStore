namespace SampleStore.Data.Seed.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;

    using SampleStore.Common;
    using SampleStore.Data.Entities.Identity;
    using SampleStore.Data.Seed.Extensions;
    using SampleStore.Identity.Constants;

    /// <summary>
    /// Class encapsulating create roles command.
    /// </summary>
    /// <seealso cref="ICreateRolesCommand" />
    public class CreateRolesCommand : ICreateRolesCommand
    {
        #region Fields

        /// <summary>
        /// The query materializer
        /// </summary>
        private readonly IQueryMaterializer _queryMaterializer;

        /// <summary>
        /// The role manager
        /// </summary>
        private readonly RoleManager<Role> _roleManager;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateRolesCommand" /> class.
        /// </summary>
        /// <param name="roleManager">The role manager.</param>
        /// <param name="queryMaterializer">The query materializer.</param>
        public CreateRolesCommand(RoleManager<Role> roleManager, IQueryMaterializer queryMaterializer)
        {
            _roleManager = roleManager.ThrowIfArgumentIsNull(nameof(roleManager));
            _queryMaterializer = queryMaterializer.ThrowIfArgumentIsNull(nameof(queryMaterializer));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Creates the roles.
        /// </summary>
        /// <returns>The created roles.</returns>
        public async Task<List<Role>> CreateRoles()
        {
            var existingRoles = await _queryMaterializer.ToList(_roleManager.Roles);
            var existingRoleNames = await Task.WhenAll(existingRoles.Select(_roleManager.GetRoleNameAsync));

            foreach (var role in RolePrototypes.Roles.ToList())
            {
                var roleName = await _roleManager.GetRoleNameAsync(role);
                if (existingRoleNames.Any(existingRoleName => string.Equals(existingRoleName, roleName, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                var roleResult = await _roleManager.CreateAsync(role);
                roleResult.ThrowIfFailed(() => $"Failed to create role {roleName}.");
            }

            var roles = await _queryMaterializer.ToList(_roleManager.Roles);
            return roles;
        }

        #endregion Methods
    }
}