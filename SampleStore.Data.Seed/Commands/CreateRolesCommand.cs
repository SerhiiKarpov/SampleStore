using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SampleStore.Common.Commands;
using SampleStore.Data.Entities.Identity;
using SampleStore.Data.Seed.Extensions;
using SampleStore.Services.Identity;
using SampleStore.Services.Identity.Constants;

namespace SampleStore.Data.Seed.Commands;

public class CreateRolesCommand(IRoleManager roleManager, IQueryMaterializer queryMaterializer) : ICommand<List<Role>>
{
    private readonly IQueryMaterializer _queryMaterializer = queryMaterializer ?? throw new ArgumentNullException(nameof(queryMaterializer));
    private readonly IRoleManager _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));

    public async Task<List<Role>> Do()
    {
        var existingRoles = await _queryMaterializer.ToList(_roleManager.Roles);
        var existingRoleNames = existingRoles.Select(x => x.Name).ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var roleName in Roles.All)
        {
            if (existingRoleNames.Contains(roleName))
            {
                continue;
            }

            var roleResult = await _roleManager.CreateAsync(new() { Name = roleName });
            if (!roleResult.Succeeded)
            {
                roleResult.ThrowIfFailed(() => $"Failed to create role {roleName}.");
            }
        }

        return await _queryMaterializer.ToList(_roleManager.Roles);
    }
}