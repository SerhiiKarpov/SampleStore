using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

using SampleStore.Common.Commands;
using SampleStore.Data.Entities.Identity;
using SampleStore.Data.Seed.Extensions;

namespace SampleStore.Data.Seed.Commands;

public class AddUserToRolesCommand : ICommand<bool>
{
    private readonly RoleManager<Role> _roleManager;
    private readonly IEnumerable<Role> _roles;
    private readonly User _user;
    private readonly UserManager<User> _userManager;

    public AddUserToRolesCommand(UserManager<User> userManager, RoleManager<Role> roleManager, User user, IEnumerable<Role> roles)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        _user = user ?? throw new ArgumentNullException(nameof(user));
        _roles = roles ?? throw new ArgumentNullException(nameof(roles));
    }

    public async Task<bool> Do()
    {
        var roleNames = await Task.WhenAll(_roles.Select(role => _roleManager.GetRoleNameAsync(role)));
        var addToRolesResult = await _userManager.AddToRolesAsync(_user, roleNames!);
        addToRolesResult.ThrowIfFailed(() => $"Failed to add user {_user.Email} to roles: {string.Join(", ", roleNames)}. Check logs for details.");
        return true;
    }
}