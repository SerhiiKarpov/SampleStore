using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using SampleStore.Common.Commands;
using SampleStore.Data.Entities.Identity;
using SampleStore.Data.Seed.Extensions;
using SampleStore.Services.Identity.Constants;

namespace SampleStore.Data.Seed.Commands;

public class CreateRolesCommand : ICommand<List<Role>>
{
    private readonly IQueryMaterializer _queryMaterializer;

    private readonly RoleManager<Role> _roleManager;

    public CreateRolesCommand(RoleManager<Role> roleManager, IQueryMaterializer queryMaterializer)
    {
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        _queryMaterializer = queryMaterializer ?? throw new ArgumentNullException(nameof(queryMaterializer));
    }

    public async Task<List<Role>> Do()
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
            if (!roleResult.Succeeded)
            {
                roleResult.ThrowIfFailed(() => $"Failed to create role {roleName}.");
            }
        }

        var roles = await _queryMaterializer.ToList(_roleManager.Roles);
        return roles;
    }
}